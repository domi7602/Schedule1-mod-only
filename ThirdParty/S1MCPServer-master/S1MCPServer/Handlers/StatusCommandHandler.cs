using System;
using S1MCPServer.Core;
using S1MCPServer.Models;
using S1MCPServer.Server;
using S1MCPServer.Utils;

namespace S1MCPServer.Handlers;

/// <summary>
/// Handles the <c>status</c> command: reports what the TCP server is doing right now
/// (listener state, client endpoint, connection generation, last request, queue depths).
///
/// Reads only <see cref="ServerStatus"/>, which is lock-protected, so it is marked
/// <see cref="IThreadSafeCommandHandler"/> and answers on the server thread - the client can
/// always ask whether the server is alive, even when the game is paused and the main-thread
/// queue is not draining.
/// </summary>
public class StatusCommandHandler : ICommandHandler, IThreadSafeCommandHandler
{
    private readonly ResponseQueue _responseQueue;
    private readonly ServerStatus _status;

    public StatusCommandHandler(ResponseQueue responseQueue, ServerStatus status)
    {
        _responseQueue = responseQueue;
        _status = status;
    }

    public void Handle(Request request)
    {
        try
        {
            var result = _status.Snapshot();
            var response = ProtocolHandler.CreateSuccessResponse(request.Id, result);
            _responseQueue.EnqueueResponse(response);
            ModLogger.Debug($"Status response sent (ID: {request.Id})");
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Error in HandleStatus: {ex.Message}");
            var errorResponse = ProtocolHandler.CreateErrorResponse(
                request.Id,
                -32603, // Internal error
                "Failed to build status response",
                new { details = ex.Message }
            );
            _responseQueue.EnqueueResponse(errorResponse);
        }
    }
}
