using System;
using System.Reflection;
using HarmonyLib;
using Il2CppScheduleOne;
using Il2CppScheduleOne.Core.Items.Framework;
using Il2CppScheduleOne.ItemFramework;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(StackLimitMod.Mod), "StackLimitMod", "0.1.1", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace StackLimitMod;

public class Mod : MelonMod
{
    public static Mod Instance { get; private set; } = null!;
    public static ModLogger Log { get; private set; } = null!;
    public static StackLimitConfig Config { get; set; } = null!;

    public override void OnInitializeMelon()
    {
        Instance = this;
        Log = new ModLogger("StackLimitMod");

        // 1. Load config
        Config = StackLimitConfig.Load();

        // 2. Register Hash terminal bridge (reflective, optional dependency)
        TryRegisterHashPlugin();

        // 3. Apply Harmony patches safely via PatchGuard
        ApplyHarmonyPatches();

        // 4. Subscribe to S1API GameLifecycle events
        GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete += OnLoadComplete;

        Log.Info($"StackLimitMod v0.1.1 initialized with StackLimit={Config.StackLimit}.");
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
        GameLifecycle.OnLoadComplete -= OnLoadComplete;
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        // StackLimitEngine.ApplyStackLimits(Config) used to be called here, 
        // but it caused AccessViolationExceptions because native IL2CPP objects 
        // (like Registry or Resources) might not be fully initialized yet.
        // We now safely rely on GameLifecycle.OnLoadComplete instead.
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        // Cached get_StackLimit decisions are keyed by native object pointers, which are
        // per-session (and reusable after objects are destroyed) — drop them on scene change.
        StackLimitPatches.ClearDecisionCache();
    }

    private void OnSaveInfoLoaded()
    {
        try
        {
            long now = Environment.TickCount64;
            if (now - _lastApplyTicks < 1500 && StackLimitEngine.ModifiedItemCount > 0)
            {
                Log.Debug("OnSaveInfoLoaded: skip duplicate apply (already applied recently)");
                return;
            }
            StackLimitEngine.ApplyStackLimits(Config);
            _lastApplyTicks = now;
        }
        catch (Exception ex)
        {
            Log.Error($"Error in OnSaveInfoLoaded handler: {ex}");
        }
    }

    private void OnLoadComplete()
    {
        try
        {
            long now = Environment.TickCount64;
            if (now - _lastApplyTicks < 1500 && StackLimitEngine.ModifiedItemCount > 0)
            {
                Log.Debug("OnLoadComplete: skip duplicate apply (already applied recently)");
                return;
            }
            StackLimitEngine.ApplyStackLimits(Config);
            _lastApplyTicks = now;
        }
        catch (Exception ex)
        {
            Log.Error($"Error in OnLoadComplete handler: {ex}");
        }
    }

    private static long _lastApplyTicks = 0;

    private void ApplyHarmonyPatches()
    {
        try
        {
            // H-1: Specify overload to avoid AmbiguousMatchException on future game update
            try
            {
                PatchGuard.TryPatch(
                    HarmonyInstance,
                    typeof(Registry),
                    nameof(Registry.AddToRegistry),
                    postfix: new HarmonyMethod(typeof(StackLimitPatches), nameof(StackLimitPatches.Registry_AddToRegistry_Postfix)),
                    parameterTypes: new[] { typeof(ItemDefinition) },
                    log: Log);
            }
            catch (Exception ex)
            {
                Log.Warn($"AddToRegistry ItemDefinition overload failed: {ex.Message} — trying BaseItemDefinition fallback");
                PatchGuard.TryPatch(
                    HarmonyInstance,
                    typeof(Registry),
                    nameof(Registry.AddToRegistry),
                    postfix: new HarmonyMethod(typeof(StackLimitPatches), nameof(StackLimitPatches.Registry_AddToRegistry_Postfix)),
                    parameterTypes: new[] { typeof(BaseItemDefinition) },
                    log: Log);
            }

            PatchGuard.TryPatch(
                HarmonyInstance,
                typeof(BaseItemInstance),
                "get_StackLimit",
                postfix: new HarmonyMethod(typeof(StackLimitPatches), nameof(StackLimitPatches.BaseItemInstance_GetStackLimit_Postfix)),
                log: Log);

            // NOTE: BaseItemDefinition.get_DefaultStackLimit ist ein IL2CPP Field-Accessor und nicht patchbar
            // (Il2CppInterop: "field accessor, it can't be patched"). StackLimitEngine behandelt
            // DefaultStackLimit via direkten Field-Access / Scan; kein Harmony-Patch nötig.

            PatchGuard.Report(Log);
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to apply Harmony patches via PatchGuard: {ex.Message}");
        }
    }

    private static void TryRegisterHashPlugin()
    {
        try
        {
            var hashType = Type.GetType("Hash.Api.HashCommands, Hash");
            if (hashType == null) return;

            var availableProp = hashType.GetProperty("Available", BindingFlags.Public | BindingFlags.Static);
            if (availableProp == null || !(bool)(availableProp.GetValue(null) ?? false)) return;

            var addMethod = hashType.GetMethod("Add", BindingFlags.Public | BindingFlags.Static);
            if (addMethod == null) return;

            addMethod.Invoke(null, new object[]
            {
                "stack",
                "StackLimit: stats, set <amount>, reload, help",
                "stack stats"
            });
            Log.Info("Hash bridge registered: 'stack' command listed in terminal.");
        }
        catch (Exception ex)
        {
            Log.Debug($"Hash bridge registration skipped: {ex.Message}");
        }
    }
}
