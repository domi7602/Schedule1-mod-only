using System;
using System.Collections.Generic;
using System.IO;
using AutoPackagingStation.Entities;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.Persistence;
using S1Mods.Shared;
using UnityEngine;
using NativeQualityItemInst = Il2CppScheduleOne.ItemFramework.QualityItemInstance;

namespace AutoPackagingStation.Persistence;

/// <summary>
/// Serializable state data for a slot within the auto-packaging station buffer.
/// </summary>
public class SlotItemData
{
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int Quantity { get; set; } = 0;
    public float QualityValue { get; set; } = 0.5f;
    public int QualityTier { get; set; } = 2; // 0=Trash, 1=Poor, 2=Standard, 3=Premium, 4=Heavenly
    public string PackagingId { get; set; } = string.Empty;
    public List<string> MixEffects { get; set; } = new();
}

/// <summary>
/// Persistent save state for a single Auto-Packaging Station in the game world.
/// </summary>
public class AutoPackStationSaveData
{
    public string Guid { get; set; } = string.Empty;
    public float[] Position { get; set; } = new float[3];
    public float[] Rotation { get; set; } = new float[4];
    public SlotItemData? InputProduct { get; set; }
    public SlotItemData? InputPackaging { get; set; }
    public SlotItemData? OutputProduct { get; set; }
    public string State { get; set; } = "Idle";
    public float Progress { get; set; } = 0f;
}

/// <summary>
/// Managed runtime state for an active auto-packaging station.
/// Completely decoupled from IL2CPP MonoBehaviours.
/// </summary>
public class AutoPackStationRuntimeData
{
    public string Guid { get; set; } = string.Empty;
    public StationState State { get; set; } = StationState.Idle;
    public float PackagingProgress { get; set; } = 0f;
    public SlotItemData? InputProduct { get; set; }
    public SlotItemData? InputPackaging { get; set; }
    public SlotItemData? OutputProduct { get; set; }

    public bool CanStartPackaging()
    {
        if (InputProduct == null || InputProduct.Quantity <= 0) return false;
        if (InputPackaging == null || InputPackaging.Quantity <= 0) return false;

        int requiredProductQty = 1;
        if (!string.IsNullOrEmpty(InputPackaging.ItemId))
        {
            var pDef = Il2CppScheduleOne.Registry.GetItem(InputPackaging.ItemId)?.TryCast<Il2CppScheduleOne.Product.Packaging.PackagingDefinition>();
            if (pDef != null && pDef.Quantity > 0) requiredProductQty = pDef.Quantity;
        }
        if (InputProduct.Quantity < requiredProductQty) return false;

        if (OutputProduct != null && OutputProduct.Quantity > 0)
        {
            // Must match both ItemId and PackagingId
            if (OutputProduct.ItemId != InputProduct.ItemId && !OutputProduct.ItemId.StartsWith(InputProduct.ItemId, StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.IsNullOrEmpty(OutputProduct.PackagingId) && !string.IsNullOrEmpty(InputPackaging.ItemId) &&
                !OutputProduct.PackagingId.Equals(InputPackaging.ItemId, StringComparison.OrdinalIgnoreCase))
                return false;

            int stackLimit = 20;
            try
            {
                var def = Il2CppScheduleOne.Registry.GetItem(OutputProduct.ItemId);
                if (def != null && def.Pointer != IntPtr.Zero)
                {
                    stackLimit = def.StackLimit;
                }
            }
            catch { }
            if (OutputProduct.Quantity >= stackLimit) return false;
        }

        return true;
    }
}

/// <summary>
/// Root container for all placed auto-packaging stations in a given save slot.
/// </summary>
public class AutoPackModSaveData
{
    public int SaveVersion { get; set; } = 1;
    public List<AutoPackStationSaveData> Stations { get; set; } = new();
}

/// <summary>
/// Manages atomic slot-isolated persistence and runtime data for AutoPackagingStation.
/// </summary>
public static class AutoPackStore
{
    private static readonly List<AutoPackStationController> _activeStations = new();
    private static readonly Dictionary<string, AutoPackStationRuntimeData> _runtimeData = new();
    private static readonly Dictionary<string, AutoPackStationSaveData> _stagedSaveData = new();
    private static string _lastKnownSlot = "";

    public static IReadOnlyList<AutoPackStationController> ActiveStations => _activeStations;

    public static AutoPackStationSaveData? GetStagedSaveData(string guid)
    {
        if (string.IsNullOrEmpty(guid)) return null;
        _stagedSaveData.TryGetValue(guid, out var data);
        return data;
    }

    public static AutoPackStationRuntimeData GetRuntimeData(string guid)
    {
        if (string.IsNullOrEmpty(guid)) guid = "default";
        if (!_runtimeData.TryGetValue(guid, out var data))
        {
            data = new AutoPackStationRuntimeData { Guid = guid };
            _runtimeData[guid] = data;
        }
        return data;
    }

    public static void RegisterStation(AutoPackStationController controller)
    {
        if (controller == null || controller.Pointer == IntPtr.Zero) return;
        if (!_activeStations.Contains(controller))
        {
            _activeStations.Add(controller);
            Mod.Log.Debug($"Registered AutoPackagingStation [{controller.StationGuid}]. Active count: {_activeStations.Count}");
        }
    }

    public static void UnregisterStation(AutoPackStationController controller)
    {
        if (controller == null) return;
        if (_activeStations.Remove(controller))
        {
            Mod.Log.Debug($"Unregistered AutoPackagingStation [{controller.StationGuid}]. Active count: {_activeStations.Count}");
        }
    }

    public static bool RemoveRuntimeData(string guid)
    {
        if (string.IsNullOrEmpty(guid)) return false;
        return _runtimeData.Remove(guid);
    }

    public static string GetActiveSlotSuffix()
    {
        try
        {
            var pLoadMgr = Il2CppScheduleOne.DevUtilities.PersistentSingleton<Il2CppScheduleOne.Persistence.LoadManager>.Instance;
            if (pLoadMgr != null && pLoadMgr.Pointer != IntPtr.Zero && !pLoadMgr.WasCollected)
            {
                var info = pLoadMgr.ActiveSaveInfo;
                if (info != null && info.Pointer != IntPtr.Zero && !info.WasCollected)
                {
                    _lastKnownSlot = info.SaveSlotNumber.ToString();
                    return _lastKnownSlot;
                }
            }

            var infoLegacy = LoadManager.Instance?.ActiveSaveInfo;
            if (infoLegacy != null && infoLegacy.Pointer != IntPtr.Zero && !infoLegacy.WasCollected)
            {
                _lastKnownSlot = infoLegacy.SaveSlotNumber.ToString();
                return _lastKnownSlot;
            }
        }
        catch
        {
            // Fallback to last known slot
        }
        if (string.IsNullOrEmpty(_lastKnownSlot)) return "0";
        return _lastKnownSlot;
    }

    public static string GetSaveFilePath()
    {
        string slot = GetActiveSlotSuffix();
        string targetPath = SafeStorage.GetUserDataPath("AutoPackagingStation", $"autopack_slot_{slot}.json");
        TryMigrateLegacy(targetPath);
        return targetPath;
    }

    private static void TryMigrateLegacy(string targetSlotPath)
    {
        try
        {
            string legacyPath = SafeStorage.GetUserDataPath("AutoPackagingStation", "autopack.json");
            if (!File.Exists(legacyPath)) return;

            if (File.Exists(targetSlotPath))
            {
                try { File.Delete(legacyPath); } catch { }
                return;
            }

            var legacyData = SafeStorage.LoadSafe<AutoPackModSaveData>(legacyPath, null, Mod.Log);
            if (legacyData != null && legacyData.Stations != null && legacyData.Stations.Count > 0)
            {
                if (SafeStorage.SaveAtomic(targetSlotPath, legacyData, Mod.Log))
                {
                    try { File.Delete(legacyPath); } catch { }
                    Mod.Log.Info($"Migrated legacy autopack.json -> {Path.GetFileName(targetSlotPath)} ({legacyData.Stations.Count} stations)");
                    return;
                }
            }

            try
            {
                File.Copy(legacyPath, targetSlotPath, true);
                File.Delete(legacyPath);
                Mod.Log.Info($"Migrated legacy autopack.json -> {Path.GetFileName(targetSlotPath)} (copy fallback)");
            }
            catch (Exception copyEx)
            {
                Mod.Log.Warn($"Legacy migration copy failed: {copyEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Legacy migration check failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Lifecycle handler: Called before a new save or scene loads. Clears in-memory station references.
    /// </summary>
    public static void OnPreLoad()
    {
        try
        {
            Mod.Log.Debug("OnPreLoad: Clearing active station references.");
            _activeStations.Clear();
            _runtimeData.Clear();
            _stagedSaveData.Clear();
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnPreLoad error: {ex.Message}");
        }
    }

    /// <summary>
    /// Lifecycle handler: Called after save info is loaded and game scene is built. Restores saved station data.
    /// </summary>
    public static void OnLoadComplete()
    {
        try
        {
            string path = GetSaveFilePath();
            Mod.Log.Info($"Loading AutoPackagingStation state from {Path.GetFileName(path)}...");

            var data = SafeStorage.LoadSafe<AutoPackModSaveData>(path, new AutoPackModSaveData(), Mod.Log);
            if (data == null || data.Stations == null || data.Stations.Count == 0)
            {
                Mod.Log.Info("No saved AutoPackagingStations found in active save slot.");
                return;
            }

            // Sync or spawn stations
            int restoredCount = 0;
            foreach (var stationData in data.Stations)
            {
                if (stationData == null) continue;

                // Match existing in-world controller or spawn new one
                AutoPackStationController? target = null;
                for (int i = 0; i < _activeStations.Count; i++)
                {
                    var active = _activeStations[i];
                    if (active != null && active.Pointer != IntPtr.Zero && active.StationGuid == stationData.Guid)
                    {
                        target = active;
                        break;
                    }
                }

                if (target != null)
                {
                    ApplySaveData(stationData.Guid, stationData);
                    restoredCount++;
                }
                else
                {
                    // Game restores buildables itself — pre-stage save data so the
                    // controller can pick it up via GetRuntimeData when it awakens.
                    ApplySaveData(stationData.Guid, stationData);
                    Mod.Log.Debug($"SaveData staged for GUID {stationData.Guid} (awaiting buildable restore).");
                }
            }

            Mod.Log.Info($"Restored {restoredCount}/{data.Stations.Count} AutoPackagingStation(s) for Slot {GetActiveSlotSuffix()}.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnLoadComplete restore error: {ex}");
        }
    }

    public static AutoPackStationSaveData CreateSaveData(AutoPackStationController stationController)
    {
        if (stationController == null || stationController.Pointer == IntPtr.Zero)
            return new AutoPackStationSaveData();

        var pos = stationController.transform.position;
        var rot = stationController.transform.rotation;
        string guid = stationController.StationGuid;
        var rData = GetRuntimeData(guid);

        SlotItemData? savedPkg = rData.InputPackaging;
        SlotItemData? savedProd = rData.InputProduct;
        SlotItemData? savedOut = rData.OutputProduct;

        var station = stationController.GetComponent<PackagingStation>() ?? stationController.GetComponentInParent<PackagingStation>();
        if (station != null && station.Pointer != IntPtr.Zero && station.InputSlots != null && station.InputSlots.Count >= 2 && station.OutputSlots != null && station.OutputSlots.Count >= 1)
        {
            // InputSlots[0] -> pkgSlot
            var pkgSlot = station.InputSlots[0];
            if (pkgSlot != null && pkgSlot.Pointer != IntPtr.Zero && pkgSlot.ItemInstance != null && pkgSlot.ItemInstance.Pointer != IntPtr.Zero && pkgSlot.Quantity > 0 && pkgSlot.ItemInstance.Definition != null)
            {
                savedPkg = new SlotItemData
                {
                    ItemId = pkgSlot.ItemInstance.Definition.ID ?? string.Empty,
                    ItemName = pkgSlot.ItemInstance.Definition.Name ?? string.Empty,
                    Quantity = pkgSlot.Quantity
                };
            }
            else
            {
                savedPkg = null;
            }

            // InputSlots[1] -> prodSlot
            var prodSlot = station.InputSlots[1];
            if (prodSlot != null && prodSlot.Pointer != IntPtr.Zero && prodSlot.ItemInstance != null && prodSlot.ItemInstance.Pointer != IntPtr.Zero && prodSlot.Quantity > 0 && prodSlot.ItemInstance.Definition != null)
            {
                var qInst = prodSlot.ItemInstance.TryCast<NativeQualityItemInst>();
                int tier = qInst != null && qInst.Pointer != IntPtr.Zero ? (int)qInst.Quality : 2;
                float qualVal = tier switch { 0 => 0.2f, 1 => 0.35f, 2 => 0.55f, 3 => 0.80f, _ => 0.95f };
                savedProd = new SlotItemData
                {
                    ItemId = prodSlot.ItemInstance.Definition.ID ?? string.Empty,
                    ItemName = prodSlot.ItemInstance.Definition.Name ?? string.Empty,
                    Quantity = prodSlot.Quantity,
                    QualityTier = tier,
                    QualityValue = qualVal
                };
            }
            else
            {
                savedProd = null;
            }

            // OutputSlots[0] -> outSlot
            var outSlot = station.OutputSlots[0];
            if (outSlot != null && outSlot.Pointer != IntPtr.Zero && outSlot.ItemInstance != null && outSlot.ItemInstance.Pointer != IntPtr.Zero && outSlot.Quantity > 0 && outSlot.ItemInstance.Definition != null)
            {
                var qInst = outSlot.ItemInstance.TryCast<NativeQualityItemInst>();
                int tier = qInst != null && qInst.Pointer != IntPtr.Zero ? (int)qInst.Quality : 2;
                float qualVal = tier switch { 0 => 0.2f, 1 => 0.35f, 2 => 0.55f, 3 => 0.80f, _ => 0.95f };
                var pInst = outSlot.ItemInstance.TryCast<Il2CppScheduleOne.Product.ProductItemInstance>();
                string pkgId = pInst?.AppliedPackaging?.ID ?? pInst?.PackagingID ?? string.Empty;
                savedOut = new SlotItemData
                {
                    ItemId = outSlot.ItemInstance.Definition.ID ?? string.Empty,
                    ItemName = outSlot.ItemInstance.Definition.Name ?? string.Empty,
                    Quantity = outSlot.Quantity,
                    QualityTier = tier,
                    QualityValue = qualVal,
                    PackagingId = pkgId
                };
            }
            else
            {
                savedOut = null;
            }

            // NOTE: Do NOT mutate rData here — CreateSaveData is a pure read for OnSaveComplete.
            // Mutating live runtime during save caused wipe if native slot empty (H10).
        }

        return new AutoPackStationSaveData
        {
            Guid = guid,
            Position = new float[] { pos.x, pos.y, pos.z },
            Rotation = new float[] { rot.x, rot.y, rot.z, rot.w },
            InputProduct = savedProd,
            InputPackaging = savedPkg,
            OutputProduct = savedOut,
            State = rData.State.ToString(),
            Progress = rData.PackagingProgress
        };
    }

    public static AutoPackStationSaveData CreateSaveData(string guid, Vector3 position, Quaternion rotation)
    {
        var rData = GetRuntimeData(guid);
        return new AutoPackStationSaveData
        {
            Guid = guid,
            Position = new float[] { position.x, position.y, position.z },
            Rotation = new float[] { rotation.x, rotation.y, rotation.z, rotation.w },
            InputProduct = rData.InputProduct,
            InputPackaging = rData.InputPackaging,
            OutputProduct = rData.OutputProduct,
            State = rData.State.ToString(),
            Progress = rData.PackagingProgress
        };
    }

    public static void ApplySaveData(string guid, AutoPackStationSaveData data)
    {
        if (data == null || string.IsNullOrEmpty(guid)) return;
        _stagedSaveData[guid] = data;

        var rData = GetRuntimeData(guid);
        rData.InputProduct = data.InputProduct;
        rData.InputPackaging = data.InputPackaging;
        rData.OutputProduct = data.OutputProduct;
        rData.PackagingProgress = data.Progress;
        if (Enum.TryParse<StationState>(data.State, true, out var parsed))
        {
            rData.State = parsed;
        }

        // Defensive recovery: if the saved state is Blocked or NoPackaging but we
        // actually have valid inputs ready to go, force Idle so the controller's
        // Update loop can auto-start packaging.
        bool hasInputs = rData.InputProduct != null && rData.InputProduct.Quantity > 0 &&
                         rData.InputPackaging != null && rData.InputPackaging.Quantity > 0;
        if (hasInputs && (rData.State == StationState.Blocked || rData.State == StationState.NoPackaging))
        {
            Mod.Log.Debug($"ApplySaveData: recovering stuck state {rData.State} → Idle (inputs present).");
            rData.State = StationState.Idle;
        }

        for (int i = 0; i < _activeStations.Count; i++)
        {
            var active = _activeStations[i];
            if (active != null && active.Pointer != IntPtr.Zero && active.StationGuid == guid)
            {
                active.RestoreNativeSlots(data);
                break;
            }
        }
    }

    /// <summary>
    /// Lifecycle handler: Called exclusively on GameLifecycle.OnSaveComplete. Flushes active stations atomically.
    /// </summary>
    public static void OnSaveComplete()
    {
        try
        {
            if (SceneGate.IsChangingScenes || !NetworkGuard.IsInMainScene)
            {
                Mod.Log.Debug("OnSaveComplete skipped: scene changing or not in Main.");
                return;
            }

            string path = GetSaveFilePath();
            Mod.Log.Info($"Saving {_activeStations.Count} AutoPackagingStation(s) to {Path.GetFileName(path)}...");

            var rootData = new AutoPackModSaveData
            {
                SaveVersion = 1,
                Stations = new List<AutoPackStationSaveData>()
            };

            // Clean up destroyed pointers and gather active data
            for (int i = _activeStations.Count - 1; i >= 0; i--)
            {
                var station = _activeStations[i];
                if (station == null || station.Pointer == IntPtr.Zero || station.WasCollected || station.gameObject == null || station.gameObject.Pointer == IntPtr.Zero)
                {
                    _activeStations.RemoveAt(i);
                    continue;
                }

                try
                {
                    var stationData = CreateSaveData(station);
                    rootData.Stations.Add(stationData);
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"Failed to serialize station [{station.StationGuid}]: {ex.Message}");
                }
            }

            bool success = SafeStorage.SaveAtomic(path, rootData, Mod.Log);
            if (success)
            {
                Mod.Log.Info($"Successfully saved {rootData.Stations.Count} station(s) atomically.");
            }
            else
            {
                Mod.Log.Error($"Failed to atomically write {path}!");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"OnSaveComplete error: {ex}");
        }
    }
}
