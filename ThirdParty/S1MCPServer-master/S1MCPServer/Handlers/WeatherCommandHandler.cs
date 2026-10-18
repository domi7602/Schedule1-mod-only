using System;
using System.Collections.Generic;
using S1MCPServer.Core;
using S1MCPServer.Models;
using S1MCPServer.Utils;
#if MONO
using ScheduleOne.Weather;
#else
using Il2CppScheduleOne.Weather;
#endif

namespace S1MCPServer.Handlers;

/// <summary>
/// Handles get_weather against <c>ScheduleOne.Weather.EnvironmentManager</c>.
/// Reads the nine native weather-condition weights (0-1) from the manager's
/// <c>_currentWeatherConditions</c> - the same members S1API's WeatherRuntime mirrors into
/// its WeatherState, which the Weather mod renders. EnvironmentManager is a NetworkSingleton
/// that only exists once the game world is loaded; in the main menu the handler answers with
/// a clean <c>available:false</c> instead of crashing. Read-only: this endpoint never
/// selects weather sequences or triggers effects.
/// </summary>
public class WeatherCommandHandler : ICommandHandler
{
    private readonly ResponseQueue _responseQueue;

    public WeatherCommandHandler(ResponseQueue responseQueue)
    {
        _responseQueue = responseQueue;
    }

    public void Handle(Request request)
    {
        if (request.Method != "get_weather")
        {
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                request.Id,
                -32601, // Method not found
                $"Unknown weather method: {request.Method}"));
            return;
        }

        try
        {
            if (!EnvironmentManager.InstanceExists)
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id,
                    new Dictionary<string, object>
                    {
                        ["available"] = false,
                        ["reason"] = "the game world is not loaded (EnvironmentManager has not spawned)"
                    }));
                return;
            }

            var conditions = EnvironmentManager.Instance._currentWeatherConditions;
            if (conditions == null)
            {
                _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id,
                    new Dictionary<string, object>
                    {
                        ["available"] = false,
                        ["reason"] = "EnvironmentManager has no active weather conditions yet"
                    }));
                return;
            }

            _responseQueue.EnqueueResponse(ProtocolHandler.CreateSuccessResponse(request.Id,
                new Dictionary<string, object>
                {
                    ["available"] = true,
                    ["conditions"] = new Dictionary<string, object>
                    {
                        ["sunny"] = conditions.Sunny,
                        ["cloudy"] = conditions.Cloudy,
                        ["rainy"] = conditions.Rainy,
                        ["stormy"] = conditions.Stormy,
                        ["snowy"] = conditions.Snowy,
                        ["foggy"] = conditions.Foggy,
                        ["windy"] = conditions.Windy,
                        ["hail"] = conditions.Hail,
                        ["sleet"] = conditions.Sleet
                    }
                }));
        }
        catch (Exception ex)
        {
            ModLogger.Error($"Error in HandleWeather (get_weather): {ex.Message}");
            _responseQueue.EnqueueResponse(ProtocolHandler.CreateErrorResponse(
                request.Id,
                -32000, // Game error
                "Failed to get the weather",
                new { details = ex.Message }));
        }
    }
}
