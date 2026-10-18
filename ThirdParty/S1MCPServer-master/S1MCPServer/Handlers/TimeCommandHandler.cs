using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using S1MCPServer.Core;
using S1MCPServer.Models;
using S1MCPServer.Utils;
#if MONO
using ScheduleOne.GameTime;
#else
using Il2CppScheduleOne.GameTime;
#endif

namespace S1MCPServer.Handlers;

/// <summary>
/// Handles get_time_of_day / set_time_of_day against <c>ScheduleOne.GameTime.TimeManager</c>.
/// The in-game clock is stored as minutes-since-midnight (0-1439) plus elapsed days.
/// TimeManager is a NetworkSingleton that only exists once the game world is loaded; in the
/// main menu the handlers answer with a clean <c>available:false</c> instead of crashing.
/// </summary>
public class TimeCommandHandler : ICommandHandler
{
    private readonly ResponseQueue _responseQueue;

    public TimeCommandHandler(ResponseQueue responseQueue)
    {
        _responseQueue = responseQueue;
    }

    public void Handle(Request request)
    {
        switch (request.Method)
        {
            case "get_time_of_day":
                HandleGet(request);
                break;
            case "set_time_of_day":
                HandleSet(request);
                break;
            default:
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id,
                    -32601, // Method not found
                    $"Unknown time method: {request.Method}"));
                break;
        }
    }

    private void HandleGet(Request request)
    {
        try
        {
            if (!TimeManager.InstanceExists)
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id,
                    new Dictionary<string, object>
                    {
                        ["available"] = false,
                        ["reason"] = "the game world is not loaded (TimeManager has not spawned)"
                    }));
                return;
            }

            TimeManager tm = TimeManager.Instance;
            int minutes = tm.CurrentTime;
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id,
                new Dictionary<string, object>
                {
                    ["available"] = true,
                    ["time_minutes"] = minutes,
                    ["elapsed_days"] = tm.ElapsedDays,
                    ["display"] = TimeManager.GetMinutesToDisplayTime(minutes)
                }));
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Error in HandleGet (get_time_of_day): {ex.Message}");
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                request.Id,
                -32000, // Game error
                "Failed to get the in-game time",
                new { details = ex.Message }));
        }
    }

    private void HandleSet(Request request)
    {
        try
        {
            if (request.Params == null
                || (!request.Params.TryGetValue("time", out var timeObj)
                    && !request.Params.TryGetValue("minutes", out timeObj)))
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id,
                    -32602, // Invalid params
                    "provide \"time\" (hours, 0-24) or \"minutes\" (0-1439)"));
                return;
            }

            if (!TryGetDouble(timeObj, out double value))
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id,
                    -32602, // Invalid params
                    "time/minutes must be a number"));
                return;
            }

            bool asHours = request.Params.ContainsKey("time");
            if (double.IsNaN(value) || double.IsInfinity(value) || value < 0 || (asHours ? value > 24 : value > 1439))
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id,
                    -32602, // Invalid params
                    asHours
                        ? "time must be within [0, 24] hours"
                        : "minutes must be within [0, 1439]"));
                return;
            }

            // Validate on the double BEFORE casting: an unchecked (int) cast of an
            // out-of-range double silently becomes int.MinValue, which would survive the
            // range check above and reach SetTime_Server as garbage.
            int minutes = asHours
                ? (int)Math.Round(value * 60.0)
                : (int)Math.Round(value);

            minutes = ((minutes % 1440) + 1440) % 1440;

            if (!TimeManager.InstanceExists)
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id,
                    new Dictionary<string, object>
                    {
                        ["available"] = false,
                        ["reason"] = "the game world is not loaded (TimeManager has not spawned)"
                    }));
                return;
            }

            TimeManager tm = TimeManager.Instance;
            tm.SetTime_Server(minutes);
            int readback = tm.CurrentTime;
            bool changed = readback == minutes;

            var result = new Dictionary<string, object>
            {
                ["available"] = true,
                ["applied_minutes"] = minutes,
                ["readback_minutes"] = readback,
                ["changed"] = changed,
                ["display"] = TimeManager.GetMinutesToDisplayTime(readback)
            };
            if (!changed)
                result["note"] = "SetTime_Server is host-authoritative; the readback did not change, " +
                                 "so the request most likely ran on a non-host client";
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id, result));
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Error in HandleSet (set_time_of_day): {ex.Message}");
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                request.Id,
                -32000, // Game error
                "Failed to set the in-game time",
                new { details = ex.Message }));
        }
    }

    /// <summary>
    /// Converts a JSON-RPC params value into a double. System.Text.Json deserializes params
    /// values into <see cref="JsonElement"/>; raw doubles/strings are accepted as a fallback.
    /// </summary>
    private static bool TryGetDouble(object? value, out double result)
    {
        result = 0;
        if (value == null)
            return false;
        if (value is JsonElement je)
        {
            if (je.ValueKind == JsonValueKind.Number)
            {
                result = je.GetDouble();
                return true;
            }
            if (je.ValueKind == JsonValueKind.String)
                return double.TryParse(je.GetString(), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
            return false;
        }
        return double.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out result);
    }
}
