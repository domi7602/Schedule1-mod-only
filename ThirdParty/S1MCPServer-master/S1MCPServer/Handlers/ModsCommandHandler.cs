using System;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using S1MCPServer.Core;
using S1MCPServer.Models;
using S1MCPServer.Utils;

namespace S1MCPServer.Handlers;

/// <summary>
/// Handles list_mods: reports every registered Melon mod (MelonLoader registry) with name,
/// version and author. Purely managed - no game-world access - but it stays on the
/// main-thread queue (not the fast path) to keep MelonHandler reads single-threaded with
/// the loader.
/// </summary>
public class ModsCommandHandler : ICommandHandler
{
    private readonly ResponseQueue _responseQueue;

    public ModsCommandHandler(ResponseQueue responseQueue)
    {
        _responseQueue = responseQueue;
    }

    public void Handle(Request request)
    {
        if (request.Method != "list_mods")
        {
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                request.Id,
                -32601, // Method not found
                $"Unknown mods method: {request.Method}"));
            return;
        }

        try
        {
            var mods = MelonMod.RegisteredMelons
                .Select(m => new Dictionary<string, object>
                {
                    ["name"] = m.Info?.Name ?? "unknown",
                    ["version"] = m.Info?.Version ?? "unknown",
                    ["author"] = m.Info?.Author ?? "unknown",
                    ["type"] = m.GetType().Name
                })
                .OrderBy(m => (string)m["name"], StringComparer.OrdinalIgnoreCase)
                .ToList();

            _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id,
                new Dictionary<string, object>
                {
                    ["total_count"] = mods.Count,
                    ["mods"] = mods
                }));
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Error in HandleListMods: {ex.Message}");
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                request.Id,
                -32000, // Game error
                "Failed to list the registered mods",
                new { details = ex.Message }));
        }
    }
}
