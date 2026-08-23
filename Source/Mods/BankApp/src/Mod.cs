using System;
using BankApp.Config;
using BankApp.Services;
using BankApp.UI;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(BankApp.Mod), "BankApp", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace BankApp;

public class Mod : MelonMod
{
    public static ModLogger Log { get; private set; } = null!;

    public override void OnInitializeMelon()
    {
        Log = new ModLogger("BankApp");
        ModConfig<BankAppConfig>.Initialize("BankApp", Log);

        try
        {
            ClassInjector.RegisterTypeInIl2Cpp<BankAppInputFocus>();
        }
        catch
        {
            // Already registered or handled by MelonLoader attribute
        }

        Log.Info("Initialized (v0.1.0).");
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasLoaded(buildIndex, sceneName);
        if (sceneName.Equals("Main", StringComparison.OrdinalIgnoreCase))
        {
            TransactionHistoryService.ResetForSceneUnload();
        }
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasUnloaded(buildIndex, sceneName);
        if (sceneName.Equals("Main", StringComparison.OrdinalIgnoreCase))
        {
            TransactionHistoryService.ResetForSceneUnload();
        }
    }
}
