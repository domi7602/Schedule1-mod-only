using System;
using MelonLoader;
using S1Mods.Shared;
using TVBrowser.Config;

[assembly: MelonInfo(typeof(TVBrowser.Mod), "TVBrowser", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace TVBrowser;

public class Mod : MelonMod
{
    public static TVBrowserConfig Config => ModConfig<TVBrowserConfig>.Instance;

    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("Initializing TVBrowser (v0.1.0)...");

        // Initialize ModConfig
        ModConfig<TVBrowserConfig>.Initialize("TVBrowser");

        MelonLogger.Msg($"Config loaded. Home Page: {Config.HomePage}");
        MelonLogger.Msg("Initialized and registered with S1API TVApp.");
    }
}
