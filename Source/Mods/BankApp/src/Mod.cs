using System;
using BankApp.Config;
using BankApp.Services;
using BankApp.UI;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using S1API.Lifecycle;
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

        // H4: Slot isolation lifecycle — GameLifecycle is source of truth (OnSceneWasLoaded kept as secondary)
        try
        {
            GameLifecycle.OnPreLoad += HandlePreLoad;
            GameLifecycle.OnSaveInfoLoaded += HandleSaveInfoLoaded;
            GameLifecycle.OnLoadComplete += HandleLoadComplete;
        }
        catch (Exception ex) { Log.Warn($"GameLifecycle hook failed: {ex.Message}"); }

        Log.Info("Initialized (v0.1.0).");
    }

    public override void OnDeinitializeMelon()
    {
        try
        {
            GameLifecycle.OnPreLoad -= HandlePreLoad;
            GameLifecycle.OnSaveInfoLoaded -= HandleSaveInfoLoaded;
            GameLifecycle.OnLoadComplete -= HandleLoadComplete;
        }
        catch { }
        // Also clear cache on deinit
        try { TransactionHistoryService.ResetCache(false); } catch { }
    }

    private static void HandlePreLoad() => TransactionHistoryService.ResetCache(false);
    private static void HandleSaveInfoLoaded() => TransactionHistoryService.ResetCache(false);
    private static void HandleLoadComplete() => TransactionHistoryService.ResetCache(false);

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
