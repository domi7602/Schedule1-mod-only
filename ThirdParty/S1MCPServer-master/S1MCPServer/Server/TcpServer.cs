using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using S1MCPServer.Core;
using S1MCPServer.Models;
using S1MCPServer.Utils;

namespace S1MCPServer.Server;

/// <summary>
/// TCP server that handles communication with the MCP server.
/// Runs on a background thread and marshals commands to the main thread via CommandQueue.
///
/// Robustness contract (v1.0.2):
///  - The listener accepts connections continuously; a client handler never blocks accepting.
///  - A new connection replaces a stale one instead of being rejected.
///  - Reads are cancellable, so a superseded or stopped connection is torn down at once.
///  - Writes are serialized, reads never hold the write lock. The old shared semaphore let an
///    idle read starve the heartbeat, which is exactly what made dead clients invisible.
/// </summary>
public class TcpServer
{
    private readonly CommandQueue _commandQueue;
    private readonly ResponseQueue _responseQueue;
    private readonly ServerStatus _status;
    /// <summary>
    /// Optional fast path. Returns true when the request was fully handled on the calling
    /// thread (server thread) and must not be queued for the main thread.
    /// </summary>
    private readonly Func<Request, bool> _tryHandleDirectly;

    private TcpListener? _tcpListener;
    // volatile: ServerLoop/ResponseLoop/HeartbeatLoop read these outside _clientLock. Reference
    // reads are atomic, but without volatile a stale cached value could make a replacement
    // loop skip its drop branch and leave two live handlers (review finding NB2).
    private volatile TcpClient? _connectedClient;
    private volatile NetworkStream? _clientStream;
    private CancellationTokenSource? _connectionCts;
    private volatile bool _isRunning;
    private Task? _serverTask;
    private Task? _responseTask;
    private Task? _heartbeatTask;

    private readonly SemaphoreSlim _writeSemaphore = new SemaphoreSlim(1, 1);
    private readonly object _clientLock = new object();
    private readonly ConcurrentDictionary<int, TaskCompletionSource<bool>> _pendingSends = new ConcurrentDictionary<int, TaskCompletionSource<bool>>();
    private int _heartbeatRequestId = 0;
    private readonly object _heartbeatIdLock = new object();

    private const int DefaultPort = 8765;
    private const int ResponseSendTimeoutMs = 10000;
    private const int WriteTimeoutMs = 5000;
    private const int HeartbeatIntervalSeconds = 30;

    public TcpServer(CommandQueue commandQueue, ResponseQueue responseQueue, ServerStatus status,
                     Func<Request, bool> tryHandleDirectly = null, int port = DefaultPort)
    {
        _commandQueue = commandQueue;
        _responseQueue = responseQueue;
        _status = status;
        _tryHandleDirectly = tryHandleDirectly;
        Port = port;
    }

    public int Port { get; }

    /// <summary>
    /// Starts the TCP server on a background thread.
    /// </summary>
    public void Start()
    {
        if (_isRunning)
        {
            ModLogger.Warn("TCP server is already running");
            return;
        }

        _isRunning = true;
        _serverTask = Task.Run(ServerLoop);
        _responseTask = Task.Run(ResponseLoop);
        _heartbeatTask = Task.Run(HeartbeatLoop);
        ModLogger.Info($"TCP server started on port {Port}");
    }

    /// <summary>
    /// Stops the TCP server gracefully.
    /// </summary>
    public void Stop()
    {
        if (!_isRunning)
        {
            return;
        }

        _isRunning = false;
        _status.SetListenerRunning(false);

        _tcpListener?.Stop();
        _tcpListener = null;

        DropCurrentClient("server stopping");

        _serverTask?.Wait(TimeSpan.FromSeconds(2));
        _responseTask?.Wait(TimeSpan.FromSeconds(2));
        _heartbeatTask?.Wait(TimeSpan.FromSeconds(2));

        ModLogger.Info("TCP server stopped");
    }

    /// <summary>
    /// Closes the current client connection and cancels its read loop. Safe to call when no
    /// client is connected. Also releases everyone waiting for a response that will never be sent.
    /// Returns true when this call dropped the connection that is currently installed; false when
    /// the referenced stream had already been superseded. Callers must not report a disconnect for
    /// the current generation in the false case - a superseded write failure would otherwise clear
    /// the healthy replacement client's connected flag (review finding NB1).
    /// </summary>
    private bool DropCurrentClient(string reason, NetworkStream expectedStream = null)
    {
        bool dropped;
        lock (_clientLock)
        {
            // A background loop may hold a reference to a connection that has since been
            // superseded. Never let a stale write failure tear down the new client.
            if (expectedStream != null && !ReferenceEquals(_clientStream, expectedStream))
            {
                ModLogger.Debug($"DropCurrentClient: ignored ({reason}) - connection already superseded");
                return false;
            }

            dropped = _connectedClient != null;

            try { _connectionCts?.Cancel(); } catch (Exception) { /* already disposed */ }

            try { _clientStream?.Close(); } catch (Exception) { /* ignore */ }
            try { _connectedClient?.Close(); } catch (Exception) { /* ignore */ }

            _clientStream = null;
            _connectedClient = null;
            _connectionCts = null;
        }

        // Nobody is going to receive these responses now.
        foreach (var pending in _pendingSends)
        {
            pending.Value.TrySetResult(false);
        }
        _pendingSends.Clear();

        ModLogger.Debug($"DropCurrentClient: {reason}");
        return dropped;
    }

    /// <summary>
    /// Main server loop that accepts client connections.
    /// </summary>
    private async Task ServerLoop()
    {
        try
        {
            ModLogger.Debug("ServerLoop started");
            _tcpListener = new TcpListener(IPAddress.Loopback, Port);
            _tcpListener.Start();
            _status.SetListenerRunning(true);
            ModLogger.Info($"TCP server listening on {IPAddress.Loopback}:{Port}");

            while (_isRunning)
            {
                TcpClient newClient;
                try
                {
                    newClient = await _tcpListener.AcceptTcpClientAsync();
                }
                catch (Exception ex) when (!_isRunning)
                {
                    ModLogger.Debug($"ServerLoop: accept aborted on shutdown ({ex.GetType().Name})");
                    break;
                }
                catch (Exception ex)
                {
                    ModLogger.Error($"TCP accept failed: {ex.Message}");
                    _status.OnError($"accept failed: {ex.Message}");
                    await Task.Delay(500);
                    continue;
                }

                var endPoint = newClient.Client.RemoteEndPoint?.ToString() ?? "unknown";

                // Single-client protocol: the newest connection wins. The previous client is
                // usually gone (a crashed tool left a half-open socket), occasionally superseded.
                // The old code rejected the newcomer while .Connected still reported true - which
                // is precisely how a dead client locked the server out of every new connection.
                if (_connectedClient != null)
                {
                    ModLogger.Warn($"Replacing existing client connection with new connection from {endPoint}");
                    _status.OnStaleClientReplaced();
                    DropCurrentClient("replaced by new client");
                }

                var generation = _status.OnClientAccepted(endPoint);
                ConfigureKeepAlive(newClient.Client);
                var stream = newClient.GetStream();

                CancellationToken token;
                lock (_clientLock)
                {
                    _connectedClient = newClient;
                    _clientStream = stream;
                    _connectionCts = new CancellationTokenSource();
                    token = _connectionCts.Token;
                }

                ModLogger.Info($"Client connected from {endPoint} (generation {generation})");

                // Fire and forget, but the handler owns its own error handling: the accept loop
                // must never be blocked by a client that stops talking.
                _ = Task.Run(() => HandleClient(stream, generation, token));
            }
        }
        catch (Exception ex)
        {
            if (_isRunning)
            {
                ModLogger.Error($"TCP server error: {ex.Message}");
                _status.OnError($"server loop: {ex.Message}");
            }
        }
        finally
        {
            _status.SetListenerRunning(false);
            try { _tcpListener?.Stop(); } catch (Exception) { /* ignore */ }
            _tcpListener = null;
            ModLogger.Debug("ServerLoop ended");
        }
    }

    /// <summary>
    /// Enables TCP keep-alive with short Windows intervals so the OS eventually reports a
    /// half-open connection (peer process died without FIN/RST) instead of leaving the read
    /// blocked forever. Best-effort: other platforms simply ignore the IOControl call.
    /// </summary>
    private static void ConfigureKeepAlive(Socket socket)
    {
        try
        {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            if (!OperatingSystem.IsWindows())
            {
                return; // SIO_KEEPALIVE_VALS is Windows-only; the option above still applies.
            }

            // SIO_KEEPALIVE_VALS: onoff (uint), keepalive time (ms), keepalive interval (ms)
            byte[] keepAlive = new byte[12];
            BitConverter.GetBytes((uint)1).CopyTo(keepAlive, 0);
            BitConverter.GetBytes((uint)10000).CopyTo(keepAlive, 4);
            BitConverter.GetBytes((uint)3000).CopyTo(keepAlive, 8);
            socket.IOControl(IOControlCode.KeepAliveValues, keepAlive, null);
        }
        catch (Exception ex)
        {
            ModLogger.Debug($"ConfigureKeepAlive failed (non-fatal): {ex.Message}");
        }
    }

    /// <summary>
    /// Handles communication with a connected client.
    /// </summary>
    private async Task HandleClient(NetworkStream stream, int generation, CancellationToken cancellationToken)
    {
        ModLogger.Debug($"HandleClient started (generation {generation})");

        // Give the client a moment to be ready
        try { await Task.Delay(100, cancellationToken); }
        catch (OperationCanceledException) { return; }

        while (_isRunning && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                ModLogger.Debug("Waiting to read message from client...");

                // Deliberately NOT taking the write lock here: there is exactly one reader per
                // connection, and holding that lock across an idle read is what used to starve
                // the heartbeat and hide dead clients.
                string jsonMessage;
                try
                {
                    jsonMessage = await ProtocolHandler.ReadMessageAsync(stream, cancellationToken);
                    ModLogger.DebugPayload("Received raw JSON message", jsonMessage);
                }
                catch (OperationCanceledException)
                {
                    ModLogger.Debug("HandleClient: read cancelled (connection superseded or server stopping)");
                    break;
                }
                catch (EndOfStreamException ex)
                {
                    ModLogger.Info($"Client disconnected (generation {generation}): {ex.Message}");
                    break;
                }
                catch (IOException ex)
                {
                    ModLogger.Info($"Client connection lost (generation {generation}): {ex.Message}");
                    break;
                }

                // Acknowledgments and other notifications carry no "method" property.
                // Consume and ignore them instead of feeding them to the deserializer.
                if (!ProtocolHandler.TryGetMethod(jsonMessage, out _))
                {
                    ModLogger.DebugPayload("Ignoring non-request message (acknowledgment)", jsonMessage);
                    continue;
                }

                // Deserialize request
                Request request;
                try
                {
                    ModLogger.Debug("Deserializing request...");
                    request = ProtocolHandler.DeserializeRequest(jsonMessage);
                    ModLogger.DebugPayload($"Deserialized request ID={request.Id} Method={request.Method} Params", System.Text.Json.JsonSerializer.Serialize(request.Params));
                }
                catch (Exception ex)
                {
                    ModLogger.Error($"Failed to deserialize request: {ex.Message}");
                    ModLogger.Debug($"Deserialization error type: {ex.GetType().Name}");
                    var errorResponse = ProtocolHandler.CreateErrorResponse(
                        0, // Unknown ID
                        -32700, // Parse error
                        "Invalid JSON",
                        new { details = ex.Message }
                    );
                    await SendDirectAsync(stream, errorResponse, cancellationToken);
                    continue;
                }

                _status.OnRequest(request.Method);

                // Fast path: handlers that never touch the Unity API run right here on the server
                // thread. This is what lets a client handshake, list methods and read `status`
                // while the game sits in the main menu and the main-thread queue is not draining.
                bool handledInline = false;
                if (_tryHandleDirectly != null)
                {
                    // Register the waiter BEFORE the handler runs. The inline handler enqueues its
                    // response synchronously, and the 10 ms-polling ResponseLoop could otherwise
                    // dequeue, write and CompleteSend it inside the window between EnqueueResponse
                    // and WaitForSendAsync's registration - the waiter then stalls for the full
                    // 10 s timeout even though the response is already on the socket.
                    RegisterPendingSend(request.Id);
                    try
                    {
                        handledInline = _tryHandleDirectly(request);
                    }
                    catch (Exception ex)
                    {
                        // The router catches its own handler failures; getting here means the fast
                        // path itself broke. Answer with an error instead of queueing, so the client
                        // never waits on a main thread that may be paused.
                        ModLogger.Error($"Fast path failed for '{request.Method}': {ex.Message}");
                        _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                            request.Id,
                            -32603, // Internal error
                            "Fast path failed",
                            new { details = ex.Message }
                        ));
                        handledInline = true;
                    }
                }

                if (handledInline)
                {
                    ModLogger.Debug($"Handled inline on server thread: {request.Method} (ID: {request.Id})");
                }
                else
                {
                    // No inline producer for this id, so the placeholder is dead weight. Register
                    // again BEFORE enqueueing: the main-thread handler is the only remaining
                    // producer, and its response must never be written before the waiter exists.
                    _pendingSends.TryRemove(request.Id, out _);
                    RegisterPendingSend(request.Id);
                    ModLogger.Debug($"Enqueuing command: {request.Method} (ID: {request.Id})");
                    _commandQueue.EnqueueCommand(request);
                }

                // Wait until THIS response has actually been written to the socket. This replaces
                // the old "poll _responseQueue.Count" heuristic, which could not tell our response
                // apart from unrelated queue activity and silently burned 10 seconds whenever the
                // write failed (that is the "Waited 10000ms for response to be sent" warning).
                bool sent = await WaitForRegisteredSendAsync(request.Id, cancellationToken);
                if (sent)
                {
                    ModLogger.Debug($"Response for request {request.Id} was sent");
                }
                else
                {
                    ModLogger.Warn($"Response for request {request.Id} was not sent within {ResponseSendTimeoutMs}ms");
                }
                
                // No separate acknowledgment read here: acks are ordinary messages on the same
                // connection and are consumed at the top of this loop. The old blocking ack read
                // had no timeout and would hang forever if the client sent none.
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                ModLogger.Error($"Error handling client: {ex.Message}");
                ModLogger.Debug($"Exception type: {ex.GetType().Name}");
                break;
            }
        }

        // Teardown on EVERY exit path (EOF, error, cancellation through replacement or stop). The
        // old code only reported the status event and left the socket open until the next accept
        // or a heartbeat failure up to ~30 s later, inflating stale_clients_replaced on the way.
        if (DropCurrentClient("client session ended", stream))
        {
            _status.OnClientDisconnected(_status.CurrentGeneration);
        }
        else
        {
            // Superseded (a replacement or the server stop already dropped it). Report against our
            // own captured generation: ServerStatus ignores generations that no longer match, so
            // this can never clear the healthy replacement client's state.
            _status.OnClientDisconnected(generation);
        }
        ModLogger.Debug($"HandleClient ended (generation {generation})");
    }

    /// <summary>
    /// Registers the waiter for <paramref name="responseId"/>. MUST be called before the code that
    /// can produce the response for this id (an inline handler or the command queue), so the
    /// response loop can never deliver the response before the waiter exists.
    /// </summary>
    private void RegisterPendingSend(int responseId)
    {
        _pendingSends[responseId] = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
    }

    /// <summary>
    /// Waits for the response loop to confirm that the response with <paramref name="responseId"/>
    /// was actually written to the socket. The waiter must have been registered with
    /// <see cref="RegisterPendingSend"/> beforehand. Returns false on timeout or cancellation.
    /// </summary>
    private async Task<bool> WaitForRegisteredSendAsync(int responseId, CancellationToken cancellationToken)
    {
        if (!_pendingSends.TryGetValue(responseId, out var tcs))
        {
            ModLogger.Warn($"No send waiter registered for response {responseId}; treating as unsent");
            return false;
        }

        try
        {
            var completed = await Task.WhenAny(tcs.Task, Task.Delay(ResponseSendTimeoutMs, cancellationToken));
            if (completed != tcs.Task)
            {
                return false;
            }
            return tcs.Task.Result;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
        finally
        {
            _pendingSends.TryRemove(responseId, out _);
        }
    }

    /// <summary>
    /// Writes a response directly from the client handler. Used for protocol-level errors that
    /// never reach the main thread (e.g. parse errors).
    /// </summary>
    private async Task SendDirectAsync(NetworkStream stream, Response response, CancellationToken cancellationToken)
    {
        try
        {
            await _writeSemaphore.WaitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(WriteTimeoutMs);
            await ProtocolHandler.WriteMessageAsync(stream, ProtocolHandler.SerializeResponse(response), cts.Token);
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Failed to send direct response: {ex.Message}");
        }
        finally
        {
            _writeSemaphore.Release();
        }
    }

    /// <summary>
    /// Response loop that sends responses back to the client.
    /// </summary>
    private async Task ResponseLoop()
    {
        try
        {
            ModLogger.Debug("ResponseLoop started");
            while (_isRunning)
            {
                try
                {
                    // Wait for responses from main thread
                    if (!_responseQueue.TryDequeue(out Response? response) || response == null)
                    {
                        await Task.Delay(10);
                        continue;
                    }

                    ModLogger.Debug($"Dequeued response for request ID: {response.Id}, has_error: {response.Error != null}, has_result: {response.Result != null}");

                    var stream = _clientStream;
                    if (stream == null)
                    {
                        ModLogger.Debug($"No client connected, discarding response for ID: {response.Id}");
                        CompleteSend(response.Id, false);
                        continue;
                    }

                    try
                    {
                        string jsonResponse = ProtocolHandler.SerializeResponse(response);
                        ModLogger.DebugPayload("Serialized response", jsonResponse);

                        await _writeSemaphore.WaitAsync();
                        try
                        {
                            using var cts = new CancellationTokenSource(WriteTimeoutMs);
                            await ProtocolHandler.WriteMessageAsync(stream, jsonResponse, cts.Token);
                        }
                        finally
                        {
                            _writeSemaphore.Release();
                        }

                        ModLogger.Debug($"Successfully sent response for request ID: {response.Id}");
                        CompleteSend(response.Id, true);
                    }
                    catch (Exception ex)
                    {
                        ModLogger.Error($"Failed to send response for ID {response.Id}: {ex.Message}");

                        // The write failed, so the client stopped reading. Treat the connection as
                        // dead and release the waiter instead of re-enqueueing forever (the old
                        // code retried an unbounded number of times into a broken stream).
                        CompleteSend(response.Id, false);
                        _status.OnError($"response write failed: {ex.Message}");
                        // Only report a disconnect when THIS call actually dropped the installed
                        // connection; a superseded stream's write failure must never clear the
                        // healthy replacement client's connected flag (review finding NB1).
                        if (DropCurrentClient("write failure", stream))
                        {
                            _status.OnClientDisconnected(_status.CurrentGeneration);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModLogger.Error($"Error in response loop: {ex.Message}");
                    ModLogger.Debug($"Exception type: {ex.GetType().Name}");
                    await Task.Delay(100);
                }
            }
        }
        catch (Exception ex)
        {
            // A background loop must never die silently - that is how a server looks healthy
            // while being unable to answer anything.
            ModLogger.Error($"ResponseLoop terminated: {ex.Message}");
            _status.OnError($"response loop terminated: {ex.Message}");
        }
        ModLogger.Debug("ResponseLoop ended");
    }

    /// <summary>Releases the client handler waiting for the send of <paramref name="responseId"/>.</summary>
    private void CompleteSend(int responseId, bool success)
    {
        if (_pendingSends.TryRemove(responseId, out var tcs))
        {
            tcs.TrySetResult(success);
        }
    }

    /// <summary>
    /// Heartbeat loop that sends periodic heartbeat messages to keep the connection alive.
    /// </summary>
    private async Task HeartbeatLoop()
    {
        try
        {
            ModLogger.Debug("HeartbeatLoop started");

            while (_isRunning)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(HeartbeatIntervalSeconds));

                    if (!_isRunning)
                    {
                        break;
                    }

                    var stream = _clientStream;
                    if (stream == null)
                    {
                        ModLogger.Debug("HeartbeatLoop: No connected client, skipping heartbeat");
                        continue;
                    }

                    // Server-initiated ids live in the NEGATIVE namespace: a client request id is
                    // always positive, so CompleteSend can never satisfy a client's waiter with a
                    // heartbeat frame (the old positive counter could collide exactly there).
                    int heartbeatId;
                    lock (_heartbeatIdLock)
                    {
                        heartbeatId = --_heartbeatRequestId;
                    }

                    // Server-initiated notification; the client needs no reply.
                    var heartbeatResponse = new Response
                    {
                        Id = heartbeatId,
                        Result = new Dictionary<string, object>
                        {
                            ["type"] = "server_heartbeat",
                            ["status"] = "alive",
                            ["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                        },
                        Error = null
                    };

                    try
                    {
                        await _writeSemaphore.WaitAsync();
                        try
                        {
                            using var cts = new CancellationTokenSource(WriteTimeoutMs);
                            await ProtocolHandler.WriteMessageAsync(stream, ProtocolHandler.SerializeResponse(heartbeatResponse), cts.Token);
                        }
                        finally
                        {
                            _writeSemaphore.Release();
                        }

                        ModLogger.Debug($"HeartbeatLoop: Server heartbeat sent successfully (ID: {heartbeatId})");
                    }
                    catch (Exception ex)
                    {
                        // A heartbeat that cannot be written is the cheapest dead-client detector we
                        // have: it needs no cooperation from the client. Treat it as fatal for the
                        // connection so the next tool run gets a clean slot.
                        ModLogger.Info($"HeartbeatLoop: heartbeat write failed, dropping client: {ex.Message}");
                        _status.OnError($"heartbeat write failed: {ex.Message}");
                        // Same guard as the response loop: a superseded stream's heartbeat failure
                        // must not clear the healthy replacement client's connected flag (NB1).
                        if (DropCurrentClient("heartbeat write failure", stream))
                        {
                            _status.OnClientDisconnected(_status.CurrentGeneration);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (_isRunning)
                    {
                        ModLogger.Debug($"HeartbeatLoop: Error in heartbeat loop: {ex.Message}");
                        await Task.Delay(1000); // Wait a bit before retrying
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ModLogger.Error($"HeartbeatLoop terminated: {ex.Message}");
            _status.OnError($"heartbeat loop terminated: {ex.Message}");
        }
        ModLogger.Debug("HeartbeatLoop ended");
    }
}

