using System;
using CustomSkateboard.Commands;
using CustomSkateboard.Config;
using CustomSkateboard.Dialogue;
using CustomSkateboard.Items;
using CustomSkateboard.Visuals;
using HarmonyLib;
using Il2CppScheduleOne.Dialogue;
using Il2CppScheduleOne.Equipping;
using Il2CppScheduleOne.Skating;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(CustomSkateboard.Mod), "CustomSkateboard", "1.1.2", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace CustomSkateboard;

public sealed class Mod : MelonMod
{
    private static ModLogger? _log;
    public static ModLogger Log => _log ??= new ModLogger("CustomSkateboard");
    private static readonly SkateboardConfig _fallbackConfig = SkateboardConfig.Default();
    private static bool _isConfigInitialized = false;

    public static SkateboardConfig CurrentConfig
    {
        get
        {
            if (!_isConfigInitialized)
            {
                try
                {
                    ModConfig<SkateboardConfig>.Initialize("CustomSkateboard");
                    _isConfigInitialized = true;
                }
                catch
                {
                    return _fallbackConfig;
                }
            }
            try
            {
                return ModConfig<SkateboardConfig>.Instance;
            }
            catch
            {
                return _fallbackConfig;
            }
        }
    }

    public override void OnInitializeMelon()
    {
        try
        {
            ModConfig<SkateboardConfig>.Initialize("CustomSkateboard");
            _isConfigInitialized = true;
        }
        catch (Exception ex)
        {
            Log.Error($"Early config init failed: {ex.Message}");
        }

        Log.Info($"Initialized v{Info.Version} (Board: '{CurrentConfig.SkateboardName}', ${CurrentConfig.Price}, TopSpeed: {CurrentConfig.TopSpeed_Kmh} km/h, Jump: {CurrentConfig.JumpForce}x, Turn: {CurrentConfig.TurnForce}x, AntiGravel: {CurrentConfig.DisableTerrainSlowdown})");

        // Apply Harmony patches safely via PatchGuard
        ApplyHarmonyPatches();

        // Subscribe to S1API Lifecycle events
        GameLifecycle.OnPreLoad += OnPreLoad;
        GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete += OnLoadComplete;
    }

    public override void OnDeinitializeMelon()
    {
        // Clean up lifecycle event subscriptions
        GameLifecycle.OnPreLoad -= OnPreLoad;
        GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
        try { HarmonyInstance.UnpatchSelf(); } catch (Exception ex) { Log.Warn($"UnpatchSelf failed: {ex.Message}"); }
    }

    private void ApplyHarmonyPatches()
    {
        try
        {
            // Equip patch
            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(Skateboard_Equippable),
                nameof(Skateboard_Equippable.Equip),
                postfix: new HarmonyMethod(typeof(SkateboardVisualPatches), nameof(SkateboardVisualPatches.OnEquipPostfix)),
                log: Log);

            // Mount patch
            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(Skateboard_Equippable),
                "OnMount",
                postfix: new HarmonyMethod(typeof(SkateboardVisualPatches), nameof(SkateboardVisualPatches.OnMountPostfix)),
                log: Log);

            // Awake patch
            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(Skateboard),
                nameof(Skateboard.Awake),
                postfix: new HarmonyMethod(typeof(SkateboardVisualPatches), nameof(SkateboardVisualPatches.OnSkateboardAwakePostfix)),
                log: Log);

            // Weather change patch to preserve high-speed curves and physics
            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(Skateboard),
                nameof(Skateboard.OnWeatherChange),
                postfix: new HarmonyMethod(typeof(SkateboardVisualPatches), nameof(SkateboardVisualPatches.OnWeatherChangePostfix)),
                log: Log);

            // Fix 2026-09-02 (v3): instant full-charge jump for custom boards.
            // PREFIX (not postfix) so the snap to 1 happens BEFORE vanilla's charge/jump logic.
            // FixedUpdate is private on Skateboard — patch by name.
            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(Skateboard),
                "FixedUpdate",
                prefix: new HarmonyMethod(typeof(SkateboardVisualPatches), nameof(SkateboardVisualPatches.OnFixedUpdatePrefix)),
                log: Log);

            // NOTE 2026-08-30: Anti-gravel / terrain-slow patches removed (user option B).
            // Reason: board.SlowOnTerrain = !config.DisableTerrainSlowdown in TuneSkateboard
            // already achieves the same effect (vanilla then reports full smoothness on terrain).
            // The previous prefix patches were no-ops with a per-physics-tick overhead.
            // See SkateboardVisualPatches.cs for the removal rationale.

            // Seller dialogue choice hook — IL2CPP original uses Il2CppSystem.Collections.Generic.List,
            // so binding System.List param fails silently. Patch without parameterTypes and only bind __instance.
            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(DialogueController_SkateboardSeller),
                nameof(DialogueController_SkateboardSeller.ModifyChoiceList),
                prefix: new HarmonyMethod(typeof(SkateboardSellerInjector), nameof(SkateboardSellerInjector.OnModifyChoiceListPrefix)),
                log: Log);

            PatchGuard.Report(Log);
        }
        catch (Exception ex)
        {
            Log.Error("Failed to apply some Harmony patches via PatchGuard", ex);
        }
    }

    private void OnPreLoad()
    {
        Log.Info("GameLifecycle.OnPreLoad: Checking custom skateboard definition...");
        SkateboardItemFactory.CreateAndRegister(CurrentConfig);
    }

    private void OnSaveInfoLoaded()
    {
        Log.Info("GameLifecycle.OnSaveInfoLoaded: Ensuring custom skateboard definition is registered...");
        bool registered = SkateboardItemFactory.CreateAndRegister(CurrentConfig);
        var item = SkateboardItemFactory.CustomSkateboardItem;
        if (registered && item != null)
        {
            Log.Info($"[SaveRecovery] Custom board '{item.ID}' ready. Player can resume use on next equip.");
        }
        else if (!registered)
        {
            Log.Warn("[SaveRecovery] Custom board registration deferred — base board prefab not in Registry yet.");
        }
    }

    private void OnLoadComplete()
    {
        Log.Info("GameLifecycle.OnLoadComplete: Checking for active Skateboard seller dialogues...");
        if (CurrentConfig.AutoInjectToJeffGilmore)
        {
            var sellers = UnityEngine.Object.FindObjectsByType<DialogueController_SkateboardSeller>(FindObjectsSortMode.None);
            if (sellers != null)
            {
                foreach (var seller in sellers)
                {
                    if (seller != null && seller.Pointer != IntPtr.Zero)
                    {
                        SkateboardSellerInjector.TryInject(seller, CurrentConfig);
                    }
                }
            }
        }
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        // B9: additive scenes fire this too — don't wipe tuning state on load; only re-register if needed
        if (SkateboardItemFactory.CustomSkateboardItem == null && IsGameplayScene(sceneName))
        {
            SkateboardItemFactory.CreateAndRegister(CurrentConfig);
        }
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (!IsGameplayScene(sceneName)) return;
        SkateboardItemFactory.ClearTuningState();
    }

    private static bool IsGameplayScene(string sceneName)
    {
        return sceneName == "Main";
    }
}
