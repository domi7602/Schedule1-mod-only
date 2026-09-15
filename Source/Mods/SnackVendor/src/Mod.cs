using System;
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using SnackVendor.Config;
using SnackVendor.Items;
using SnackVendor.World;
using SnackVendor.Persistence;
using UnityEngine;
using ConfigInstance = S1Mods.Shared.ModConfig<SnackVendor.Config.SnackVendorConfig>;

// (MelonInfo/MelonGame attributes intentionally absent — our MelonMod registration is
// auto-discovered by MelonLoader's [MelonInfo] attribute scanning on the Mod class; we
// put them inline next to the class so the asmversion travels together with the class.
[assembly: MelonInfo(typeof(SnackVendor.Mod), "SnackVendor", "0.0.2-mvp", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace SnackVendor;

/// <summary>
/// SnackVendor — platzierbare Vending-Maschine, die der Spieler selbst mit
/// Zutaten (Gas-Markt) befüllt. NPCs kaufen daraus zum Vanilla-Marktpreis,
/// der Erlös geht als Cash an den Spieler. Siehe docs/SPEC.md.
/// </summary>
public sealed class Mod : MelonMod
{
    public static readonly ModLogger Log = new("SnackVendor");

    public static SnackVendorConfig CurrentConfig => ConfigInstance.Instance;

    public override void OnInitializeMelon()
    {
        try
        {
            ModConfig<SnackVendorConfig>.Initialize("SnackVendor", Log);
            // First-write default values to disk so the user has a stable
            // starting point.
            ModConfig<SnackVendorConfig>.Save();
            Log.Info("SnackVendor 0.0.1 initialized (config live).");
        }
        catch (Exception ex)
        {
            Log.Warn("config init failed; using defaults", ex);
        }

        // 1. Register IL2CPP types for our MonoBehaviours (vanilla-side hooks).
        try
        {
            ClassInjector.RegisterTypeInIl2Cpp<SnackVendorController>();
            ClassInjector.RegisterTypeInIl2Cpp<SnackVendorMarker>();
            Log.Info("Registered SnackVendorController + SnackVendorMarker in IL2CPP.");
        }
        catch (Exception ex)
        {
            Log.Error("IL2CPP type registration failed", ex);
        }

        // 2. Apply Harmony patches (Critic Pillar: gate the mutations, not the methods).
        try
        {
            var harmony = HarmonyInstance;
            var pl = new PatchClassProcessor(harmony, typeof(BuildableItem_Start_Patch));
            pl.Patch();
            var pl2 = new PatchClassProcessor(harmony, typeof(VendingMachinePatches));
            pl2.Patch();
            Log.Info("Harmony patches applied (BuildableItem.Start, VendingMachine.SendPurchase/Routine/DropItem/Cash).");
        }
        catch (Exception ex)
        {
            Log.Error("Harmony patch failed", ex);
        }

        // 3. Subscribe to S1API Lifecycle events.
        try
        {
            GameLifecycle.OnSaveInfoLoaded += SnackVendorItemFactory.RegisterItem;
            GameLifecycle.OnSaveInfoLoaded += SnackVendorItemFactory.InjectHardwareStoreListing;
            GameLifecycle.OnLoadComplete += SnackVendorItemFactory.InjectHardwareStoreListing;

            // Storage slot-resolution: S1API exposes the current save slot via
            // PlayerData events; if not, our store stays in UnknownSlot and
            // Save/Load become no-ops rather than writing to the wrong file.
            GameLifecycle.OnLoadComplete += ResolveAndStoreSlot;
            GameLifecycle.OnSaveComplete += PersistAllStationSlots;
            GameLifecycle.OnPreLoad += ResetStoredSlot;
        }
        catch (Exception ex)
        {
            Log.Warn("lifecycle subscribe failed", ex);
        }
    }

    public override void OnDeinitializeMelon()
    {
        try
        {
            GameLifecycle.OnSaveInfoLoaded -= SnackVendorItemFactory.RegisterItem;
            GameLifecycle.OnSaveInfoLoaded -= SnackVendorItemFactory.InjectHardwareStoreListing;
            GameLifecycle.OnLoadComplete -= SnackVendorItemFactory.InjectHardwareStoreListing;
            GameLifecycle.OnLoadComplete -= ResolveAndStoreSlot;
            GameLifecycle.OnSaveComplete -= PersistAllStationSlots;
            GameLifecycle.OnPreLoad -= ResetStoredSlot;
        }
        catch (Exception ex)
        {
            Log.Warn("lifecycle unsubscribe failed", ex);
        }
    }

    /// <summary>On scene-load, re-register the item (catch resets from patch-days).</summary>
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (sceneName == "Main")
        {
            try
            {
                SnackVendorItemFactory.RegisterItem();
                SnackVendorItemFactory.InjectHardwareStoreListing();
            }
            catch (Exception ex)
            {
                Log.Warn("scene-hook OnSceneWasLoaded failed", ex);
            }
        }
    }

    private void ResolveAndStoreSlot()
    {
        // Sonde: S1Mods.Shared.SaveSlots (Konsolidierung 2026-09-15) — probiert
        // PersistentSingleton/LoadManager.Instance/Singleton (früher zwei Blöcke hier).
        int slot = SaveSlots.GetActiveSlotNumber();
        if (slot >= 0)
        {
            SnackVendor.Persistence.SnackVendorStore.OnSaveSlotResolved(slot);
            Log.Info($"Resolved and stored save slot: {slot}");
        }
    }

    private void ResetStoredSlot() { SnackVendor.Persistence.SnackVendorStore.OnSaveSlotReset(); }
    private void PersistAllStationSlots()
    {
        // Iterate live controllers and ask each to flush its slots.
        try
        {
            if (!SnackVendorStore.IsSlotResolved) return;
            var live = UnityEngine.Object.FindObjectsOfType<SnackVendorController>();
            foreach (var c in live)
            {
                if (c == null || c.Pointer == IntPtr.Zero) continue;
                try { c.PersistSlotsToDisk(); }
                catch (Exception ex) { Log.Warn($"save-flush for {c.InstanceGuid}", ex); }
            }
        }
        catch (Exception ex)
        {
            Log.Warn("save-flush failed", ex);
        }
    }
}
