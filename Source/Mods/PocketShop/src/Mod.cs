using System;
using MelonLoader;
using Il2CppInterop.Runtime.Injection;
using PocketShop.Config;
using PocketShop.Services;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(PocketShop.Mod), "PocketShop", "0.2.7", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PocketShop;

public class Mod : MelonMod
{
    public const string Version = "0.2.7";
    public static ModLogger Log { get; private set; } = null!;

    public override void OnInitializeMelon()
    {
        try { ClassInjector.RegisterTypeInIl2Cpp<PocketShopInputFocus>(); } catch (Exception ex) { MelonLogger.Warning($"Failed to register PocketShopInputFocus: {ex.Message}"); }
        Log = new ModLogger("PocketShop");
        ModConfig<PocketShopConfig>.Initialize("PocketShop", Log);
        MelonLogger.Msg($"initialized (v{Version}).");
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasUnloaded(buildIndex, sceneName);
        if (sceneName.Equals("Main", StringComparison.OrdinalIgnoreCase))
        {
            PocketShopApp.TearDownForSceneUnload();
            ShopCatalog.ResetForSceneReload();
            NPCPortraitService.Reset();
        }
    }
}
