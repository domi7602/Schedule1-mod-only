using System;
using System.Collections.Generic;
using S1MCPServer.Core;

namespace S1MCPServer.Server;

/// <summary>
/// Thread-safe runtime state of the TCP server, written by the server/connection
/// loops (background threads) and read by the <c>status</c> command handler,
/// which runs on the Unity main thread.
///
/// Purpose: when a client hangs or a connection goes stale, this is the one place
/// that answers "what was the server actually doing?" without digging through
/// Latest.log.
/// </summary>
public class ServerStatus
{
    private readonly object _lock = new object();
    private readonly CommandQueue _commandQueue;
    private readonly ResponseQueue _responseQueue;
    private readonly DateTime _startedUtc = DateTime.UtcNow;

    private bool _listenerRunning;
    private bool _clientConnected;
    private string? _clientEndPoint;
    private int _connectionGeneration;
    private int _totalConnections;
    private int _staleClientsReplaced;
    private string? _lastRequestMethod;
    private DateTime? _lastRequestUtc;
    private int _inlineRequests;
    private string? _lastInlineMethod;
    private string? _lastError;
    private DateTime? _lastErrorUtc;

    public ServerStatus(CommandQueue commandQueue, ResponseQueue responseQueue)
    {
        _commandQueue = commandQueue;
        _responseQueue = responseQueue;
    }

    public DateTime StartedUtc => _startedUtc;

    public void SetListenerRunning(bool running)
    {
        lock (_lock) { _listenerRunning = running; }
    }

    /// <summary>Records a newly accepted client and returns its generation number.</summary>
    public int OnClientAccepted(string? endPoint)
    {
        lock (_lock)
        {
            _clientConnected = true;
            _clientEndPoint = endPoint;
            _totalConnections++;
            _connectionGeneration++;
            return _connectionGeneration;
        }
    }

    public void OnClientDisconnected(int generation)
    {
        lock (_lock)
        {
            // Ignore a disconnect from a connection that has already been superseded,
            // otherwise the old handler clears the state of the new client.
            if (generation != _connectionGeneration) return;
            _clientConnected = false;
            _clientEndPoint = null;
        }
    }

    public void OnStaleClientReplaced()
    {
        lock (_lock) { _staleClientsReplaced++; }
    }

    public void OnRequest(string method)
    {
        lock (_lock)
        {
            _lastRequestMethod = method;
            _lastRequestUtc = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Records a request that was answered directly on the server thread instead of being
    /// queued for the main thread. A non-zero counter is proof the fast path is live.
    /// </summary>
    public void OnInlineRequest(string method)
    {
        lock (_lock)
        {
            _inlineRequests++;
            _lastInlineMethod = method;
        }
    }

    public void OnError(string message)
    {
        lock (_lock)
        {
            _lastError = message;
            _lastErrorUtc = DateTime.UtcNow;
        }
    }

    public int CurrentGeneration
    {
        get { lock (_lock) { return _connectionGeneration; } }
    }

    public Dictionary<string, object> Snapshot()
    {
        lock (_lock)
        {
            return new Dictionary<string, object>
            {
                ["status"] = _clientConnected ? "connected" : (_listenerRunning ? "listening" : "stopped"),
                ["listener_running"] = _listenerRunning,
                ["client_connected"] = _clientConnected,
                ["client_endpoint"] = _clientEndPoint ?? "none",
                ["connection_generation"] = _connectionGeneration,
                ["total_connections"] = _totalConnections,
                ["stale_clients_replaced"] = _staleClientsReplaced,
                ["last_request_method"] = _lastRequestMethod ?? "none",
                ["last_request_seconds_ago"] = SecondsAgo(_lastRequestUtc),
                ["inline_requests"] = _inlineRequests,
                ["last_inline_method"] = _lastInlineMethod ?? "none",
                ["last_error"] = _lastError ?? "none",
                ["last_error_seconds_ago"] = SecondsAgo(_lastErrorUtc),
                ["uptime_seconds"] = (long)(DateTime.UtcNow - _startedUtc).TotalSeconds,
                ["command_queue_depth"] = _commandQueue.Count,
                ["response_queue_depth"] = _responseQueue.Count
            };
        }
    }

    private static long SecondsAgo(DateTime? utc)
    {
        if (utc == null) return -1;
        return (long)(DateTime.UtcNow - utc.Value).TotalSeconds;
    }
}
