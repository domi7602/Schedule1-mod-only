using System;
using MelonLoader;

[assembly: MelonInfo(typeof(Weather.Mod), "Weather", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace Weather;

public class Mod : MelonMod
{
    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("[Weather] Initialized (v0.1.0) - phone app for live in-game weather.");
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasUnloaded(buildIndex, sceneName);
        if (sceneName.Equals("Main", StringComparison.OrdinalIgnoreCase))
        {
            // Drop the reference to the torn-down scene's app instance. The static
            // WeatherManager.OnWeatherChanged subscription stays (subscribed exactly once).
            WeatherApp.TearDownForSceneUnload();
        }
    }
}
