using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HomelessMod.Items;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.Building;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Property;
using Il2CppScheduleOne.Tiles;
using MelonLoader.Utils;
using S1API.Items;
using S1Mods.Shared;
using UnityEngine;

namespace HomelessMod.Building;

/// <summary>
/// Serializable record for items placed anywhere on the streets/world.
/// </summary>
public class StreetItemRecord
{
    public string ItemId { get; set; } = "";
    public string Guid { get; set; } = "";
    public float PosX { get; set; }
    public float PosY { get; set; }
    public float PosZ { get; set; }
    public float RotX { get; set; }
    public float RotY { get; set; }
    public float RotZ { get; set; }
    public float RotW { get; set; }
}

/// <summary>
/// Data wrapper saved atomically into UserData/HomelessMod/street_items.json.
/// </summary>
public class StreetSaveData
{
    public int SaveVersion { get; set; } = 1;
    public List<StreetItemRecord> Items { get; set; } = new();
}

/// <summary>
/// Manages virtual street properties, world-grid instances, and persistent street items.
/// </summary>
public static class StreetPropertyManager
{
    private static GameObject? _streetRoot;
    private static readonly List<GameObject> _activeStreetObjects = new();
    private static readonly Dictionary<int, StreetItemRecord> _objectRecords = new();
    private static readonly HashSet<string> _knownGuids = new(StringComparer.OrdinalIgnoreCase);

    public static GameObject StreetRoot
    {
        get
        {
            if (_streetRoot == null || _streetRoot.Pointer == IntPtr.Zero)
            {
                _streetRoot = new GameObject("StreetNomad_WorldRoot");
                GameObject.DontDestroyOnLoad(_streetRoot);
            }
            return _streetRoot;
        }
    }

    public static bool HasStreetRoot => _streetRoot != null && _streetRoot.Pointer != IntPtr.Zero;

    public static bool IsRegisteredStreetObject(GameObject? go)
    {
        if (go == null || go.Pointer == IntPtr.Zero) return false;
        try
        {
            if (_objectRecords.ContainsKey(go.GetInstanceID()))
                return true;

            Transform? curr = go.transform;
            while (curr != null && curr.Pointer != IntPtr.Zero)
            {
                if (_objectRecords.ContainsKey(curr.gameObject.GetInstanceID()))
                    return true;
                if (HasStreetRoot && curr.gameObject == _streetRoot)
                    return true;
                curr = curr.parent;
            }
        }
        catch { }
        return false;
    }

    public static bool IsOutdoorItem(GameObject? go)
    {
        if (go == null || go.Pointer == IntPtr.Zero) return false;
        try
        {
            if (IsRegisteredStreetObject(go)) return true;

            Transform? curr = go.transform;
            while (curr != null && curr.Pointer != IntPtr.Zero)
            {
                if (HasStreetRoot && curr.gameObject == _streetRoot)
                    return true;

                if (curr.GetComponent<OutdoorItemInteractable>() != null ||
                    curr.GetComponent<SleepingBagInteractable>() != null)
                {
                    return true;
                }

                curr = curr.parent;
            }
        }
        catch { }
        return false;
    }

    public static IReadOnlyList<GameObject> ActiveStreetObjects => _activeStreetObjects;

    private static string _lastKnownSlot = "default";

    public static string GetSaveFilePath()
    {
        string slotSuffix = "default";
        bool resolved = false;
        try
        {
            var loadMgr = PersistentSingleton<LoadManager>.Instance;
            if (loadMgr != null && loadMgr.Pointer != IntPtr.Zero && !loadMgr.WasCollected)
            {
                var saveInfo = loadMgr.ActiveSaveInfo;
                if (saveInfo != null && saveInfo.Pointer != IntPtr.Zero && !saveInfo.WasCollected)
                {
                    if (saveInfo.SaveSlotNumber >= 0)
                    {
                        slotSuffix = $"slot_{saveInfo.SaveSlotNumber}";
                        resolved = true;
                    }
                    else if (!string.IsNullOrEmpty(saveInfo.SavePath))
                    {
                        slotSuffix = Path.GetFileName(saveInfo.SavePath);
                        resolved = true;
                    }
                }
            }
            if (!resolved)
            {
                var legacyMgr = Singleton<LoadManager>.Instance;
                if (legacyMgr != null && legacyMgr.Pointer != IntPtr.Zero && !legacyMgr.WasCollected)
                {
                    var saveInfo2 = legacyMgr.ActiveSaveInfo;
                    if (saveInfo2 != null && saveInfo2.Pointer != IntPtr.Zero && !saveInfo2.WasCollected)
                    {
                        if (saveInfo2.SaveSlotNumber >= 0)
                        {
                            slotSuffix = $"slot_{saveInfo2.SaveSlotNumber}";
                            resolved = true;
                        }
                        else if (!string.IsNullOrEmpty(saveInfo2.SavePath))
                        {
                            slotSuffix = Path.GetFileName(saveInfo2.SavePath);
                            resolved = true;
                        }
                    }
                }
            }
        }
        catch { }
        if (resolved) _lastKnownSlot = slotSuffix;
        else if (!string.IsNullOrEmpty(_lastKnownSlot)) slotSuffix = _lastKnownSlot;

        string dir = SafeStorage.GetUserDataPath("HomelessMod");
        string slotPath = Path.Combine(dir, $"street_items_{slotSuffix}.json");
        string legacyPath = Path.Combine(dir, "street_items.json");

        // Fallback / migration from legacy global save if slot file does not exist yet — atomically
        if (!File.Exists(slotPath) && File.Exists(legacyPath))
        {
            Mod.Log.Info($"Migrating legacy street_items.json to {slotPath}...");
            try
            {
                var legacyData = SafeStorage.LoadSafe<StreetSaveData>(legacyPath, new StreetSaveData(), Mod.Log);
                if (legacyData != null && legacyData.Items.Count > 0)
                {
                    SafeStorage.SaveAtomic(slotPath, legacyData, Mod.Log);
                }
                else
                {
                    SafeStorage.EnsureDirectoryForFile(slotPath);
                    File.Copy(legacyPath, slotPath, true);
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"Legacy save migration warning: {ex.Message}");
            }
        }

        return slotPath;
    }

    public static void RegisterStreetItem(GameObject go, string itemId, string? guid = null)
    {
        if (go == null || go.Pointer == IntPtr.Zero) return;

        int instanceId = go.GetInstanceID();
        if (_objectRecords.ContainsKey(instanceId)) return;

        string finalGuid = string.IsNullOrEmpty(guid) ? System.Guid.NewGuid().ToString() : guid;
        if (!_knownGuids.Add(finalGuid))
        {
            // Duplicate record (e.g. double restore) — ignore, the first copy wins.
            return;
        }
        var pos = go.transform.position;
        var rot = go.transform.rotation;

        var record = new StreetItemRecord
        {
            ItemId = itemId,
            Guid = finalGuid,
            PosX = pos.x,
            PosY = pos.y,
            PosZ = pos.z,
            RotX = rot.x,
            RotY = rot.y,
            RotZ = rot.z,
            RotW = rot.w
        };

        _activeStreetObjects.Add(go);
        _objectRecords[instanceId] = record;
        go.transform.SetParent(StreetRoot.transform, true);

        // Attach generic outdoor interactable if not a sleeping bag or custom station
        bool isCustomStation = itemId.Equals("autopackagingstation", StringComparison.OrdinalIgnoreCase);
        bool hasCustomController = false;
        try
        {
            var ctrlType = TypeResolver.Find("AutoPackagingStation.Entities.AutoPackStationController", "AutoPackagingStation");
            if (ctrlType != null && go.GetComponent(Il2CppType.From(ctrlType)) != null)
            {
                hasCustomController = true;
            }
        }
        catch { }

        if (!isCustomStation && !hasCustomController && go.GetComponent<SleepingBagInteractable>() == null && go.GetComponent<OutdoorItemInteractable>() == null)
        {
            var interactable = go.AddComponent<OutdoorItemInteractable>();
            interactable.ItemId = itemId;
        }

        Mod.Log.Info($"Registered street item '{itemId}' at ({pos.x:F2}, {pos.y:F2}, {pos.z:F2}) [GUID: {finalGuid}]");
    }

    public static void UnregisterStreetItem(GameObject go)
    {
        if (go == null || go.Pointer == IntPtr.Zero) return;
        int instanceId = go.GetInstanceID();
        if (_objectRecords.TryGetValue(instanceId, out var rec))
        {
            if (rec != null) _knownGuids.Remove(rec.Guid);
        }
        _activeStreetObjects.Remove(go);
        _objectRecords.Remove(instanceId);
    }

    public static void SaveStreetItems()
    {
        try
        {
            string saveFilePath = GetSaveFilePath();
            var data = new StreetSaveData();
            var writtenGuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = _activeStreetObjects.Count - 1; i >= 0; i--)
            {
                var go = _activeStreetObjects[i];
                if (go == null || go.Pointer == IntPtr.Zero)
                {
                    _activeStreetObjects.RemoveAt(i);
                    continue;
                }

                int instanceId = go.GetInstanceID();
                if (_objectRecords.TryGetValue(instanceId, out var record))
                {
                    // Never write the same street item twice (guards against
                    // duplicate records accumulating across restore cycles).
                    if (!writtenGuids.Add(record.Guid))
                    {
                        continue;
                    }

                    var pos = go.transform.position;
                    var rot = go.transform.rotation;
                    record.PosX = pos.x;
                    record.PosY = pos.y;
                    record.PosZ = pos.z;
                    record.RotX = rot.x;
                    record.RotY = rot.y;
                    record.RotZ = rot.z;
                    record.RotW = rot.w;
                    data.Items.Add(record);
                }
            }

            SafeStorage.SaveAtomic(saveFilePath, data, Mod.Log);
            Mod.Log.Debug($"Saved {data.Items.Count} street items to {saveFilePath}");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to save street items: {ex.Message}");
        }
    }

    public static void LoadAndSpawnStreetItems()
    {
        try
        {
            string saveFilePath = GetSaveFilePath();
            if (!File.Exists(saveFilePath))
            {
                Mod.Log.Info($"No existing street items save found at {saveFilePath}. Clean start.");
                return;
            }

            var data = SafeStorage.LoadSafe<StreetSaveData>(saveFilePath, new StreetSaveData(), Mod.Log);
            if (data == null || data.Items == null || data.Items.Count == 0)
            {
                Mod.Log.Info("Street items save is empty.");
                return;
            }

            Mod.Log.Info($"Restoring {data.Items.Count} street items from {saveFilePath}...");

            foreach (var item in data.Items)
            {
                if (!_knownGuids.Add(item.Guid))
                {
                    // Already restored in this session — skip duplicates from
                    // corrupted/legacy save files.
                    Mod.Log.Warn($"Skipping duplicate street item record '{item.ItemId}' (GUID {item.Guid}).");
                    continue;
                }
                SpawnSavedStreetItem(item);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to load street items: {ex.Message}");
        }
    }

    private static void SpawnSavedStreetItem(StreetItemRecord record)
    {
        try
        {
            Vector3 pos = new Vector3(record.PosX, record.PosY, record.PosZ);
            Quaternion rot = new Quaternion(record.RotX, record.RotY, record.RotZ, record.RotW);

            if (record.ItemId.Equals(Mod.CurrentConfig.SleepingBagItemId, StringComparison.OrdinalIgnoreCase))
            {
                var bag = SleepingBagItemFactory.CreateSleepingBagPrefab();
                bag.transform.position = pos;
                bag.transform.rotation = rot;
                bag.transform.SetParent(StreetRoot.transform, true);
                SleepingBagItemFactory.SetupPlacedSleepingBag(bag);

                var comp = bag.GetComponent<SleepingBagInteractable>();
                if (comp != null) comp.Guid = record.Guid;

                _activeStreetObjects.Add(bag);
                _objectRecords[bag.GetInstanceID()] = record;
                Mod.Log.Info($"Restored Sleeping Bag at {pos}");
            }
            else
            {
                var def = Il2CppScheduleOne.Registry.GetItem(record.ItemId);
                var bDef = def?.TryCast<Il2CppScheduleOne.ItemFramework.BuildableItemDefinition>();
                if (bDef != null && bDef.BuiltItem != null)
                {
                    bool wasActive = bDef.BuiltItem.gameObject.activeSelf;
                    if (wasActive) bDef.BuiltItem.gameObject.SetActive(false);

                    var obj = GameObject.Instantiate(bDef.BuiltItem.gameObject, pos, rot);

                    if (wasActive) bDef.BuiltItem.gameObject.SetActive(true);

                    obj.transform.SetParent(StreetRoot.transform, true);

                    var buildMgr = NetworkSingleton<BuildManager>.Instance;
                    if (buildMgr != null && buildMgr.Pointer != IntPtr.Zero)
                    {
                        buildMgr.DisableNetworking(obj);
                        buildMgr.DisableNavigation(obj);
                    }

                    bool isCustomStation = record.ItemId.Equals("autopackagingstation", StringComparison.OrdinalIgnoreCase);

                    if (!isCustomStation && obj.GetComponent<OutdoorItemInteractable>() == null)
                    {
                        var interactable = obj.AddComponent<OutdoorItemInteractable>();
                        interactable.ItemId = record.ItemId;
                    }

                    if (isCustomStation)
                    {
                        var factoryType = TypeResolver.Find("AutoPackagingStation.Items.AutoPackagingItemFactory", "AutoPackagingStation");
                        var setupMethod = factoryType?.GetMethods().FirstOrDefault(m => m.Name == "SetupPlacedStation");
                        setupMethod?.Invoke(null, new object?[] { obj, null });

                        var controllerType = TypeResolver.Find("AutoPackagingStation.Entities.AutoPackStationController", "AutoPackagingStation");
                        if (controllerType != null)
                        {
                            var ctrlComp = obj.GetComponent(Il2CppType.From(controllerType));
                            if (ctrlComp != null)
                            {
                                var prop = controllerType.GetProperty("StationGuid");
                                prop?.SetValue(ctrlComp, record.Guid);
                            }
                        }
                    }

                    var footprintTiles = obj.GetComponentsInChildren<FootprintTile>(true);
                    foreach (var ft in footprintTiles)
                    {
                        if (ft == null || ft.Pointer == IntPtr.Zero || ft.WasCollected) continue;
                        if (ft.gameObject == null || ft.gameObject.Pointer == IntPtr.Zero) continue;
                        ft.gameObject.SetActive(false);
                    }

                    var gridItems = obj.GetComponentsInChildren<GridItem>(true);
                    foreach (var gi in gridItems)
                    {
                        if (gi == null || gi.Pointer == IntPtr.Zero || gi.WasCollected) continue;
                        try { gi.SetFootprintTileVisiblity(false); } catch { }
                        gi.enabled = false;
                    }

                    var buildables = obj.GetComponentsInChildren<BuildableItem>(true);
                    foreach (var b in buildables)
                    {
                        if (b == null || b.Pointer == IntPtr.Zero || b.WasCollected) continue;
                        b.enabled = false;
                    }

                    if (!isCustomStation)
                    {
                        var renderers = obj.GetComponentsInChildren<Renderer>(true);
                        foreach (var r in renderers)
                        {
                            if (r == null || r.Pointer == IntPtr.Zero || r.WasCollected) continue;
                            if (r.GetComponentInParent<FootprintTile>() != null ||
                                r.GetComponentInParent<TileAppearance>() != null)
                            {
                                r.enabled = false;
                                continue;
                            }
                            r.enabled = true;
                        }
                    }

                    var colliders = obj.GetComponentsInChildren<Collider>(true);
                    foreach (var c in colliders)
                    {
                        if (c == null || c.Pointer == IntPtr.Zero || c.WasCollected) continue;
                        c.enabled = true;
                    }

                    // Ensure rigidbodies don't cause the item to fall through the street
                    var rbs = obj.GetComponentsInChildren<Rigidbody>(true);
                    foreach (var rb in rbs)
                    {
                        if (rb == null || rb.Pointer == IntPtr.Zero || rb.WasCollected) continue;
                        rb.isKinematic = true;
                    }

                    obj.SetActive(true);

                    _activeStreetObjects.Add(obj);
                    _objectRecords[obj.GetInstanceID()] = record;
                    Mod.Log.Info($"Restored outdoor item '{record.ItemId}' at {pos}");
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to spawn saved street item '{record.ItemId}': {ex.Message}");
        }
    }

    public static void ResetState(bool keepSlot = false)
    {
        for (int i = 0; i < _activeStreetObjects.Count; i++)
        {
            var go = _activeStreetObjects[i];
            if (go != null && go.Pointer != IntPtr.Zero && !go.WasCollected)
            {
                GameObject.Destroy(go);
            }
        }
        _activeStreetObjects.Clear();
        _objectRecords.Clear();
        _knownGuids.Clear();
        if (!keepSlot) _lastKnownSlot = "default";
    }

    public static void ResetForSceneUnload()
    {
        ResetState(keepSlot: true);
    }
}
