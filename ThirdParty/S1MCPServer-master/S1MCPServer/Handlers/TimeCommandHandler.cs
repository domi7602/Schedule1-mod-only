#nullable enable
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
/// <c>CurrentTime</c> is a 24-hour <b>HHMM</b> integer (1954 = 19:54), not minutes since
/// midnight - see <see cref="TimeOfDay"/> for the evidence. Responses therefore carry both
/// <c>*_hhmm</c> (the native value) and <c>*_minutes</c> (true minutes since midnight), so a
/// client never has to guess which shape it received.
/// <c>set_time_of_day</c> accepts exactly one of <c>time</c> (decimal hours, 0-24),
/// <c>minutes</c> (0-1439) or <c>hhmm</c> (0-2359, or "19:54").
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
            int hhmm = tm.CurrentTime;
            var result = new Dictionary<string, object>
            {
                ["available"] = true,
                ["time_hhmm"] = hhmm,
                ["elapsed_days"] = tm.ElapsedDays,
                ["display"] = TimeOfDay.Format(hhmm)
            };

            if (TimeOfDay.TryHhmmToMinutes(hhmm, out int minutes))
            {
                result["time_minutes"] = minutes;
            }
            else
            {
                // Never invent a minute value from a malformed clock reading.
                result["warning"] = $"the native clock returned {hhmm}, which is not a valid HHMM value";
            }

            _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id, result));
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
            if (!TryResolveRequestedTime(request.Params, out int hhmm, out int minutes, out string error))
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                    request.Id,
                    -32602, // Invalid params
                    error));
                return;
            }

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
            tm.SetTime_Server(hhmm);
            int readback = tm.CurrentTime;
            bool changed = readback == hhmm;

            var result = new Dictionary<string, object>
            {
                ["available"] = true,
                ["applied_hhmm"] = hhmm,
                ["applied_minutes"] = minutes,
                ["readback_hhmm"] = readback,
                ["changed"] = changed,
                ["display"] = TimeOfDay.Format(readback)
            };

            if (TimeOfDay.TryHhmmToMinutes(readback, out int readbackMinutes))
                result["readback_minutes"] = readbackMinutes;

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
    /// Resolves the requested clock value to a validated HHMM plus its minutes-since-midnight
    /// equivalent. Exactly one of <c>time</c>, <c>minutes</c> or <c>hhmm</c> must be supplied;
    /// validation happens on the raw double/string before any cast, so an out-of-range value can
    /// never silently become <see cref="int.MinValue"/> and reach <c>SetTime_Server</c> as garbage.
    /// </summary>
    private static bool TryResolveRequestedTime(
        Dictionary<string, object>? parameters,
        out int hhmm,
        out int minutes,
        out string error)
    {
        hhmm = -1;
        minutes = -1;
        error = string.Empty;

        if (parameters == null)
        {
            error = "provide \"time\" (hours, 0-24), \"minutes\" (0-1439) or \"hhmm\" (0-2359)";
            return false;
        }

        bool hasTime = parameters.TryGetValue("time", out var timeObj);
        bool hasMinutes = parameters.TryGetValue("minutes", out var minutesObj);
        bool hasHhmm = parameters.TryGetValue("hhmm", out var hhmmObj);
        int supplied = (hasTime ? 1 : 0) + (hasMinutes ? 1 : 0) + (hasHhmm ? 1 : 0);

        if (supplied == 0)
        {
            error = "provide \"time\" (hours, 0-24), \"minutes\" (0-1439) or \"hhmm\" (0-2359)";
            return false;
        }

        if (supplied > 1)
        {
            error = "provide only one of \"time\", \"minutes\" or \"hhmm\"";
            return false;
        }

        if (hasTime)
        {
            if (!TryGetDouble(timeObj, out double hours))
            {
                error = "time must be a number of hours within [0, 24]";
                return false;
            }

            if (!TimeOfDay.TryHoursToHhmm(hours, out hhmm))
            {
                error = "time must be within [0, 24] hours";
                return false;
            }

            TimeOfDay.TryHhmmToMinutes(hhmm, out minutes);
            return true;
        }

        if (hasMinutes)
        {
            if (!TryGetDouble(minutesObj, out double rawMinutes))
            {
                error = "minutes must be a number within [0, 1439]";
                return false;
            }

            if (double.IsNaN(rawMinutes) || double.IsInfinity(rawMinutes) || rawMinutes < 0 || rawMinutes > 1439)
            {
                error = "minutes must be within [0, 1439]";
                return false;
            }

            minutes = (int)Math.Round(rawMinutes, MidpointRounding.AwayFromZero);
            if (!TimeOfDay.TryMinutesToHhmm(minutes, out hhmm))
            {
                error = "minutes must be within [0, 1439]";
                return false;
            }

            return true;
        }

        if (!TryGetHhmm(hhmmObj, out hhmm))
        {
            error = "hhmm must be a 24-hour value within [0, 2359] (for example 1954 or \"19:54\")";
            return false;
        }

        TimeOfDay.TryHhmmToMinutes(hhmm, out minutes);
        return true;
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

    /// <summary>
    /// Accepts a numeric HHMM (1954) or the text forms "1954" / "19:54". A JSON number with a
    /// fractional part is rejected instead of being silently truncated - a caller that means
    /// decimal hours must use <c>time</c>.
    /// </summary>
    private static bool TryGetHhmm(object? value, out int hhmm)
    {
        hhmm = -1;
        if (value == null)
            return false;

        if (value is JsonElement je)
        {
            if (je.ValueKind == JsonValueKind.Number)
            {
                if (!je.TryGetInt32(out hhmm))
                    return false;
                return TimeOfDay.IsValidHhmm(hhmm);
            }

            if (je.ValueKind == JsonValueKind.String)
                return TimeOfDay.TryParseHhmmText(je.GetString(), out hhmm);

            return false;
        }

        if (value is int i)
        {
            hhmm = i;
            return TimeOfDay.IsValidHhmm(hhmm);
        }

        return int.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out hhmm)
               && TimeOfDay.IsValidHhmm(hhmm);
    }
}
