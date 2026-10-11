using System;
using MelonLoader;
using Il2CppInterop.Runtime.Injection;
using PocketShop.Config;
using PocketShop.Services;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(PocketShop.Mod), "PocketShop", "0.3.3", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PocketShop;

public class Mod : MelonMod
{
    public const string Version = "0.3.3";
    public static ModLogger Log { get; private set; } = null!;

    public override void OnInitializeMelon()
    {
        try { ClassInjector.RegisterTypeInIl2Cpp<PocketShopInputFocus>(); } catch (Exception ex) { MelonLogger.Warning($"Failed to register PocketShopInputFocus: {ex.Message}"); }
        Log = new ModLogger("PocketShop");
        ModConfig<PocketShopConfig>.Initialize("PocketShop", Log);

        // Deferred bank confirmation: record the exact transactions the vanilla MoneyManager
        // applies server-side (CreateOnlineTransaction is a FishNet ServerRpc, so its balance
        // change is not observable synchronously). Failure is non-fatal — the purchase flow
        // falls back to balance-delta observation.
        try { BankTransactionObserver.TryApplyPatch(HarmonyInstance, Log); }
        catch (Exception ex) { MelonLogger.Warning($"BankTransactionObserver patch failed: {ex.Message}"); }

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
            // Session boundary: drop pending/uncertain transaction state deliberately (a stale
            // RPC can no longer apply after the teardown) and reset the change-only gate logs.
            PurchaseService.ResetForSceneReload();
            BankTransactionObserver.ResetForSceneReload();
            ShopGateResolver.ResetLogState();
            PocketShopInputFocus.ResetGlobalTypingState();
        }
    }
}
