extern alias il2cpp;

using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Il2CppFishNet;
using Il2CppFishNet.Object;
using Il2CppScheduleOne.Building;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Heatmap;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.Interaction;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Storage;
using Il2CppScheduleOne.Tiles;
using Il2CppScheduleOne.UI;
using MelonLoader;
using UnityEngine;

namespace TownHallInterior;

// A hall-only grid copied from one live donor tile. It is not a Property.
// Placed items are remembered in the Rathaus checkpoint, not in the native business list.
internal static class HallGrid
{
    private static GameObject? _root;
    private static HarmonyLib.Harmony? _harmony;
    private static bool _heatmapWarned;
    private static bool _saveablePatched;
    internal static bool CheckpointSaveHookReady => _saveablePatched;
    internal static void RegisterCheckpointOwned(GameObject root) => HallOwned.Add(root.GetInstanceID());

    internal static void TryPublish(Transform hall)
    {
        if (_root != null || hall.Find(HallGridPlan.GroundName) != null) return;
        try
        {
            float size = Grid.TileSize;
            if (size < 0.2f || size > 1f)
            {
                MelonLogger.Warning($"TownHallInterior: Grid.TileSize is {size}; hall grid not published.");
                return;
            }

            var donor = FindDonor();
            if (donor == null)
            {
                MelonLogger.Warning("TownHallInterior: no donor tile found; hall grid not published.");
                return;
            }

            var root = new GameObject("TownHallInterior-grids");
            root.transform.SetParent(hall, false);
            _root = root;
            int ground = Publish(root.transform, HallGridPlan.GroundName, new[] { HallGridPlan.Ground }, null,
                PublicEntrance.FloorTop, size, donor);
            int upper = Publish(root.transform, HallGridPlan.UpperName, HallGridPlan.Upper, HallGridPlan.StairVoid,
                RoomPlan.Upper, size, donor);
            InstallPatch();
            if (!_saveablePatched)
                throw new InvalidOperationException("Hall checkpoint save exclusion hook unavailable.");
            var collider = donor.GetComponent<Collider>();
            MelonLogger.Msg($"TownHallInterior: hall grids published from '{PathOf(donor.transform)}' " +
                $"size={size:0.###} eg={ground} og={upper} layer={donor.layer} trigger={collider != null && collider.isTrigger}.");
            TryRestore();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior hall grid refused: " + ex.Message);
            Dispose();
        }
    }

    internal static void Dispose()
    {
        _records = null;
        FailedContents.Clear();
        if (_root != null) UnityEngine.Object.Destroy(_root);
        _root = null;
        PlacedIds.Clear();
        RecordIds.Clear();
        HallOwned.Clear();
        SpawnedRecords.Clear();
        Live.Clear();
    }

    private static int Publish(Transform parent, string name, IReadOnlyList<HallGridRect> floors, HallGridRect? blocked,
        float floorTop, float size, GameObject donor)
    {
        var tiles = HallGridPlan.Tiles(floors, blocked, size);
        float originX = float.MaxValue, originZ = float.MaxValue;
        foreach (var tile in tiles)
        {
            originX = Math.Min(originX, tile.WorldX - size * 0.5f);
            originZ = Math.Min(originZ, tile.WorldZ - size * 0.5f);
        }
        var host = new GameObject(name);
        host.transform.SetParent(parent, false);
        host.transform.position = new Vector3(originX, floorTop, originZ);
        host.SetActive(false);
        var grid = host.AddComponent<Grid>();
        grid.Tiles = new il2cpp::Il2CppSystem.Collections.Generic.List<Tile>();
        float offset = donor.GetComponent<Tile>().AvailableOffset;
        var template = UnityEngine.Object.Instantiate(donor, host.transform);
        template.SetActive(false);
        template.name = name + "-template";

        foreach (var tile in tiles)
        {
            var clone = UnityEngine.Object.Instantiate(template, host.transform);
            clone.name = name + "-" + tile.X + "-" + tile.Z;
            clone.transform.position = new Vector3(tile.WorldX, floorTop + 0.02f, tile.WorldZ);
            FitCollider(clone, size);
            var piece = clone.GetComponent<Tile>();
            piece.OwnerGrid = grid;
            piece.x = LocalIndex(tile.WorldX, originX, size);
            piece.y = LocalIndex(tile.WorldZ, originZ, size);
            piece.AvailableOffset = Math.Max(offset, 1f);
            piece.BuildableOccupants = new il2cpp::Il2CppSystem.Collections.Generic.List<GridItem>();
            piece.OccupantTiles = new il2cpp::Il2CppSystem.Collections.Generic.List<FootprintTile>();
            piece.LightExposureNode = null;
            clone.SetActive(true);
            grid.RegisterTile(piece);
        }

        UnityEngine.Object.Destroy(template);
        grid.ProcessCoordinateDataPairs();
        grid.SetGridSize();
        host.SetActive(true);
        MelonLogger.Msg($"TownHallInterior: {name} origin={host.transform.position} reported={grid.Origin} tiles={tiles.Count}.");
        return tiles.Count;
    }

    private static GameObject? FindDonor()
    {
        GameObject? fallback = null;
        foreach (var tile in UnityEngine.Object.FindObjectsOfType<Tile>())
        {
            if (tile == null || tile.OwnerGrid == null) continue;
            var go = tile.gameObject;
            if (go.name.StartsWith("TownHallInterior")) continue;
            if (go.GetComponent<Collider>() == null) continue;
            if (PathOf(go.transform).IndexOf("Catwalk", StringComparison.OrdinalIgnoreCase) >= 0) continue;
            if (go.GetComponent<IndoorTile>() != null) return go;
            fallback ??= go;
        }
        return fallback;
    }

    private static void FitCollider(GameObject clone, float size)
    {
        var box = clone.GetComponent<BoxCollider>();
        if (box == null)
        {
            foreach (var extra in clone.GetComponents<Collider>())
                extra.enabled = false;
            box = clone.AddComponent<BoxCollider>();
        }
        box.isTrigger = true;
        box.center = new Vector3(0f, 0.05f, 0f);
        box.size = new Vector3(size, 0.2f, size);
    }

    private static void InstallPatch()
    {
        if (_harmony != null) return;
        _harmony = new HarmonyLib.Harmony("TownHallInterior.HallGrid");
        MethodInfo? method = AccessTools.Method(typeof(Grid), nameof(Grid.IsTileValidAtCoordinate));
        S1Mods.Shared.PatchGuard.TryPatch(_harmony, method,
            prefix: new HarmonyMethod(typeof(HallGrid), nameof(ValidateHallTile)));
        MethodInfo? place = AccessTools.Method(typeof(BuildUpdate_Grid), nameof(BuildUpdate_Grid.Place));
        S1Mods.Shared.PatchGuard.TryPatch(_harmony, place,
            prefix: new HarmonyMethod(typeof(HallGrid), nameof(PlaceOnHall)));
        MethodInfo? intersections = AccessTools.Method(typeof(BuildUpdate_Grid), nameof(BuildUpdate_Grid.CheckIntersections));
        S1Mods.Shared.PatchGuard.TryPatch(_harmony, intersections,
            postfix: new HarmonyMethod(typeof(HallGrid), nameof(MarkHallPlacementValid)),
            finalizer: new HarmonyMethod(typeof(HallGrid), nameof(KeepHallIntersectionAlive)));
        MethodInfo? heatmap = AccessTools.Method(typeof(HeatmapManager), nameof(HeatmapManager.SetHeatmapActive),
            new[] { typeof(Il2CppScheduleOne.Property.Property), typeof(bool) });
        S1Mods.Shared.PatchGuard.TryPatch(_harmony, heatmap,
            prefix: new HarmonyMethod(typeof(HallGrid), nameof(SkipHeatmapWithoutProperty)));
        MethodInfo? saveable = AccessTools.Method(typeof(BuildableItem), nameof(BuildableItem.InitializeSaveable));
        _saveablePatched = S1Mods.Shared.PatchGuard.TryPatch(_harmony, saveable,
            prefix: new HarmonyMethod(typeof(HallGrid), nameof(SkipHallSaveable)));
        MethodInfo? pickup = AccessTools.Method(typeof(BuildableItem), nameof(BuildableItem.PickupItem));
        S1Mods.Shared.PatchGuard.TryPatch(_harmony, pickup,
            prefix: new HarmonyMethod(typeof(HallGrid), nameof(PickupHallItem)));
        MethodInfo? canPick = AccessTools.Method(typeof(BuildableItem), nameof(BuildableItem.CanBePickedUp));
        S1Mods.Shared.PatchGuard.TryPatch(_harmony, canPick,
            prefix: new HarmonyMethod(typeof(HallGrid), nameof(AllowHallPickup)));
        MethodInfo? hovered = AccessTools.Method(typeof(InteractableObject), nameof(InteractableObject.Hovered));
        S1Mods.Shared.PatchGuard.TryPatch(_harmony, hovered,
            postfix: new HarmonyMethod(typeof(HallGrid), nameof(OfferHallPickup)));
        MethodInfo? show = AccessTools.Method(typeof(InteractableObject), nameof(InteractableObject.ShowMessage));
        S1Mods.Shared.PatchGuard.TryPatch(_harmony, show,
            prefix: new HarmonyMethod(typeof(HallGrid), nameof(OfferHallPickup)));
        MethodInfo? start = AccessTools.Method(typeof(InteractableObject), nameof(InteractableObject.StartInteract));
        S1Mods.Shared.PatchGuard.TryPatch(_harmony, start,
            prefix: new HarmonyMethod(typeof(HallGrid), nameof(StartHallPickup)));
    }

    // The hall grid has no Property. Vanilla still calls the heatmap on every hover, and that
    // null aborts CheckIntersections before the ghost can turn valid.
    public static bool SkipHeatmapWithoutProperty(Il2CppScheduleOne.Property.Property __0)
    {
        if (__0 != null) return true;
        if (!_heatmapWarned)
        {
            _heatmapWarned = true;
            MelonLogger.Msg("TownHallInterior: heatmap skipped, the hall grid has no property.");
        }
        return false;
    }

    public static Exception? KeepHallIntersectionAlive(Exception? __exception, BuildUpdate_Grid __instance)
    {
        if (__exception == null) return null;
        if (__exception.ToString().IndexOf("NullReferenceException", StringComparison.Ordinal) < 0)
            return __exception;
        try
        {
            if (__instance?.GhostModel == null || !OverHall(__instance.GhostModel.transform.position))
                return __exception;
            MarkHallPlacementValid(__instance);
            if (!_heatmapWarned)
            {
                _heatmapWarned = true;
                MelonLogger.Warning("TownHallInterior: hall intersection check hit a null property; click kept.");
            }
            return null;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior hall ghost: " + ex.Message);
            return __exception;
        }
    }

    public static bool ValidateHallTile(Grid __instance, Coordinate gridCoord, FootprintTile tile, ref bool __result)
    {
        if (__instance == null || !IsOurs(__instance)) return true;
        __result = false;
        if (gridCoord == null) return false;
        Tile? found;
        try { found = __instance.GetTile(gridCoord); }
        catch { return false; }
        if (found == null) return false;
        if (tile != null && found.AvailableOffset + 0.001f < tile.RequiredOffset) return false;
        var occupants = found.BuildableOccupants;
        if (occupants != null && occupants.Count > 0) return false;
        __result = true;
        return false;
    }

    public static void MarkHallPlacementValid(BuildUpdate_Grid __instance)
    {
        try
        {
            if (__instance == null || __instance.GhostModel == null) return;
            if (!OverHall(__instance.GhostModel.transform.position)) return;
            var clear = FootprintClear(__instance.GhostModel);
            var manager = NetworkSingleton<BuildManager>.Instance;
            __instance._validPosition = clear;
            if (manager == null) return;
            var material = clear ? manager.ghostMaterial_White : manager.ghostMaterial_Red;
            if (material == null) return;
            __instance._currentGhostMaterial = material;
            manager.ApplyMaterial(__instance.GhostModel, material, true);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior hall ghost: " + ex.Message);
        }
    }

    public static bool PlaceOnHall(BuildUpdate_Grid __instance, ref GridItem? __result)
    {
        GameObject? placed = null;
        try
        {
            if (__instance == null || __instance.GhostModel == null) return true;
            if (!S1Mods.Shared.NetworkGuard.IsHostOrSingleplayer()) return true;
            if (!OverHall(__instance.GhostModel.transform.position)) return true;
            if (_records == null)
            {
                MelonLogger.Warning("TownHallInterior: hall place refused until the world checkpoint is bound.");
                __result = null;
                return false;
            }
            if (!FootprintClear(__instance.GhostModel))
            {
                MelonLogger.Warning("TownHallInterior: hall place refused, the tiles are taken.");
                __result = null;
                return false;
            }

            var item = __instance.ItemInstance;
            var definition = item?.Definition?.TryCast<BuildableItemDefinition>();
            if (definition == null || definition.BuiltItem == null)
            {
                MelonLogger.Warning("TownHallInterior: hall place has no built item.");
                __result = null;
                return false;
            }

            placed = SpawnBuilt(definition, __instance.GhostModel.transform.position,
                __instance.GhostModel.transform.rotation, item!.Definition!.ID, item);
            if (placed == null)
            {
                __result = null;
                return false;
            }
            RememberPlacement(placed);

            var inventory = PlayerInventory.Instance;
            if (inventory != null && item!.Definition != null)
                inventory.RemoveAmountOfItem(item.Definition.ID, 1u);
            __instance.Stop();
            __result = null;
            MelonLogger.Msg("TownHallInterior: placed " + item!.Definition!.ID + " in the hall.");
            return false;
        }
        catch (Exception ex)
        {
            if (placed != null)
            {
                ForgetPlacement(placed);
                UnityEngine.Object.Destroy(placed);
            }
            MelonLogger.Warning("TownHallInterior place hook: " + ex.Message);
            __result = null;
            return false;
        }
    }

    private const string PlacedPrefix = "TownHall-placed:";
    private static readonly Dictionary<int, string> PlacedIds = new();
    private static readonly Dictionary<int, string> RecordIds = new();
    private static readonly HashSet<int> HallOwned = new();
    private static List<HallPlacementRecord>? _records;
    private static readonly HashSet<string> SpawnedRecords = new(StringComparer.Ordinal);
    private static readonly Dictionary<string, GameObject> Live = new(StringComparer.Ordinal);
    private static readonly HashSet<string> FailedContents = new(StringComparer.Ordinal);

    internal static void Bind(List<HallPlacementRecord> records)
    {
        _records = records ?? new List<HallPlacementRecord>();
    }

    internal static void CaptureContents()
    {
        if (_records == null) return;
        foreach (var record in _records)
        {
            if (record == null || !Live.TryGetValue(record.Id, out var placed) || placed == null)
                throw new InvalidOperationException("Ein Rathausobjekt ist noch nicht wiederhergestellt.");
            if (placed.GetComponentInChildren<StorageEntity>(true) == null) continue;
            if (FailedContents.Contains(record.Id))
                throw new InvalidOperationException("Rathaus-Lagerrestore ungeklärt; Checkpoint-Inhalt bleibt geschützt.");
            record.NativeContentsJson = NativeHallStorage.Read(placed);
            record.Contents.Clear();
            MelonLogger.Msg($"TownHallInterior: captured native hall contents for {record.ItemId}.");
        }
    }

    private static List<HallSlotRecord>? ReadContents(GameObject placed)
    {
        try
        {
            var storage = placed.GetComponentInChildren<StorageEntity>(true);
            var slots = storage?.ItemSlots;
            if (storage != null && storage.ItemCount > 0 && slots == null) return null;
            if (slots == null) return new List<HallSlotRecord>();
            var found = new List<HallSlotRecord>();
            for (int i = 0; i < slots.Count && i < 64; i++)
            {
                var slot = slots[i];
                if (slot == null) continue;
                var item = slot.ItemInstance ?? slot._ItemInstance_k__BackingField;
                var id = item?.Definition?.ID;
                int quantity = slot.Quantity;
                if (quantity < 1 && item != null) quantity = 1;
                if (string.IsNullOrWhiteSpace(id) || quantity < 1) continue;
                found.Add(new HallSlotRecord { Slot = i, ItemId = id, Quantity = quantity });
            }
            if (storage != null && storage.ItemCount > found.Count) return null;
            return found;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior: hall contents were not read; previous contents kept. " + ex.Message);
            return null;
        }
    }

    private static void FillContents(GameObject placed, HallPlacementRecord record)
    {
        if (record.NativeContentsJson == null && (record.Contents == null || record.Contents.Count == 0)) return;
        try
        {
            if (record.NativeContentsJson != null)
            {
                NativeHallStorage.Restore(placed, record.NativeContentsJson);
                FailedContents.Remove(record.Id);
                MelonLogger.Msg("TownHallInterior: restored native hall contents for " + record.ItemId + ".");
                return;
            }
            var slots = placed.GetComponentInChildren<StorageEntity>(true)?.ItemSlots;
            if (slots == null)
            {
                throw new InvalidOperationException("Hall restore has no slots for " + record.ItemId);
            }
            int filled = 0;
            foreach (var line in record.Contents)
            {
                if (line == null || line.Slot < 0 || line.Slot >= slots.Count)
                    throw new InvalidOperationException("Hall restore has an unavailable slot.");
                var copy = Il2CppScheduleOne.Registry.GetItem(line.ItemId)?.GetDefaultInstance(line.Quantity);
                if (copy == null) throw new InvalidOperationException("Hall restore has an unavailable item.");
                slots[line.Slot].SetStoredItem(copy, true);
                if (slots[line.Slot].ItemInstance?.Definition?.ID != line.ItemId || slots[line.Slot].Quantity != line.Quantity)
                    throw new InvalidOperationException("Legacy hall slot restore was not confirmed.");
                filled++;
            }
            MelonLogger.Msg($"TownHallInterior: restored {filled} of {record.Contents.Count} hall slot items.");
        }
        catch (Exception ex)
        {
            FailedContents.Add(record.Id);
            placed.SetActive(false);
            MelonLogger.Warning("TownHallInterior: hall slot restore failed; contents kept. " + ex.Message);
        }
    }

    internal static void TryRestore()
    {
        if (_records == null || _root == null) return;
        int spawned = 0;
        int kept = 0;
        foreach (var record in _records.ToArray())
        {
            if (record == null || !record.IsPlausible()) continue;
            if (Live.TryGetValue(record.Id, out var existing) && existing != null) continue;
            SpawnedRecords.Remove(record.Id);
            if (!SpawnedRecords.Add(record.Id)) continue;
            kept++;
            if (RestoreOne(record)) spawned++;
            else SpawnedRecords.Remove(record.Id);
        }
        if (kept > 0)
            MelonLogger.Msg($"TownHallInterior: restored {spawned} of {kept} hall items from the checkpoint.");
    }

    public static bool SkipHallSaveable(BuildableItem __instance) =>
        __instance == null || !IsHallOwned(__instance.transform);

    private static bool RestoreOne(HallPlacementRecord record)
    {
        var definition = Il2CppScheduleOne.Registry.GetItem(record.ItemId)?.TryCast<BuildableItemDefinition>();
        if (definition?.BuiltItem == null)
        {
            MelonLogger.Warning("TownHallInterior: hall restore has no built item for " + record.ItemId + "; record kept.");
            return false;
        }
        var rotation = new Quaternion(record.RotX, record.RotY, record.RotZ, record.RotW);
        var placed = SpawnBuilt(definition, new Vector3(record.X, record.Y, record.Z), rotation, record.ItemId, null);
        if (placed == null) return false;
        RecordIds[placed.GetInstanceID()] = record.Id;
        Live[record.Id] = placed;
        FillContents(placed, record);
        return true;
    }

    private static GameObject? SpawnBuilt(BuildableItemDefinition definition, Vector3 position, Quaternion rotation,
        string itemId, ItemInstance? instance)
    {
        var source = definition.BuiltItem.gameObject;
        bool restore = source.activeSelf;
        if (restore) source.SetActive(false);
        GameObject? placed = null;
        try
        {
            placed = UnityEngine.Object.Instantiate(source, position, rotation);
            if (restore) { source.SetActive(true); restore = false; }
            if (placed == null) return null;
            HallOwned.Add(placed.GetInstanceID());
            Remember(placed, itemId);
            var built = placed.GetComponent<BuildableItem>() ?? placed.GetComponentInChildren<BuildableItem>(true);
            if (built != null && built.ItemInstance == null)
                built._ItemInstance_k__BackingField = instance?.GetCopy(1) ?? definition.GetDefaultInstance(1);
            var net = placed.GetComponent<NetworkObject>();
            var manager = InstanceFinder.NetworkManager;
            if (net != null && manager != null)
                manager.ServerManager.Spawn(net);
            Remember(placed, itemId);
            placed.SetActive(true);
            if (_root != null) placed.transform.SetParent(_root.transform, true);
            placed.transform.SetPositionAndRotation(position, rotation);
            ClaimFootprint(placed);
            return placed;
        }
        catch
        {
            if (restore) source.SetActive(true);
            if (placed != null) UnityEngine.Object.Destroy(placed);
            throw;
        }
    }

    private static void RememberPlacement(GameObject placed)
    {
        if (_records == null)
        {
            throw new InvalidOperationException("Hall place is not bound to the checkpoint.");
        }
        var rotation = placed.transform.rotation;
        var position = placed.transform.position;
        var record = new HallPlacementRecord
        {
            Id = Guid.NewGuid().ToString("N"),
            ItemId = ItemIdOf(placed),
            X = position.x,
            Y = position.y,
            Z = position.z,
            RotX = rotation.x,
            RotY = rotation.y,
            RotZ = rotation.z,
            RotW = rotation.w
        };
        if (!record.IsPlausible())
        {
            throw new InvalidOperationException("Hall place was not recorded, the pose is outside the hall.");
        }
        _records.Add(record);
        RecordIds[placed.GetInstanceID()] = record.Id;
        Live[record.Id] = placed;
    }

    private static void ForgetPlacement(GameObject placed)
    {
        if (!RecordIds.TryGetValue(placed.GetInstanceID(), out var id)) return;
        RecordIds.Remove(placed.GetInstanceID());
        Live.Remove(id);
        _records?.RemoveAll(record => record != null && record.Id == id);
    }

    private static bool IsHallOwned(Transform? node)
    {
        while (node != null)
        {
            if (HallOwned.Contains(node.gameObject.GetInstanceID())
                || node.name.StartsWith(PlacedPrefix, StringComparison.Ordinal))
                return true;
            node = node.parent;
        }
        return false;
    }

    private static void Remember(GameObject placed, string id)
    {
        placed.name = PlacedPrefix + id;
        PlacedIds[placed.GetInstanceID()] = id;
    }

    private static string ItemIdOf(GameObject placed)
    {
        if (PlacedIds.TryGetValue(placed.GetInstanceID(), out var id) && id.Length > 0) return id;
        return placed.name.StartsWith(PlacedPrefix, StringComparison.Ordinal)
            ? placed.name.Substring(PlacedPrefix.Length)
            : "";
    }

    public static bool AllowHallPickup(BuildableItem __instance, ref bool __result)
    {
        var placed = __instance == null ? null : PlacedRoot(__instance.transform);
        if (placed == null) return true;
        __result = !ContentsUnresolved(placed);
        return false;
    }

    private static bool ContentsUnresolved(GameObject placed) =>
        RecordIds.TryGetValue(placed.GetInstanceID(), out var id) && FailedContents.Contains(id);

    public static bool PickupHallItem(BuildableItem __instance)
    {
        var placed = PlacedRoot(__instance != null ? __instance.transform : null);
        if (placed == null) return true;
        if (ContentsUnresolved(placed))
        {
            Note("Storage restore unresolved; saved contents protected");
            return false;
        }
        var id = ItemIdOf(placed);
        if (id.Length == 0)
        {
            MelonLogger.Warning("TownHallInterior: hall pickup has no item id; object kept.");
            return false;
        }

        try
        {
            var returning = CopiesToReturn(placed, id);
            var inventory = PlayerInventory.Instance;
            if (inventory == null || returning.Count == 0)
            {
                MelonLogger.Warning("TownHallInterior: hall pickup could not build the item; object kept.");
                return false;
            }
            foreach (var copy in returning)
            {
                if (!inventory.CanItemFitInInventory(copy, 1))
                {
                    Note("Doesn't fit in inventory");
                    return false;
                }
            }
            foreach (var copy in returning)
                inventory.AddItemToInventory(copy);
            ReleaseFootprint(placed);
            ForgetPlacement(placed);
            UnityEngine.Object.Destroy(placed);
            MelonLogger.Msg("TownHallInterior: picked up " + id + " from the hall.");
            return false;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior hall pickup: " + ex.Message);
            return false;
        }
    }

    private static List<ItemInstance> CopiesToReturn(GameObject placed, string id)
    {
        var copies = new List<ItemInstance>();
        var definition = Il2CppScheduleOne.Registry.GetItem(id);
        var shelf = definition?.GetDefaultInstance(1);
        if (shelf != null) copies.Add(shelf);
        var storage = placed.GetComponentInChildren<StorageEntity>(true);
        var contents = storage != null ? storage.GetAllItems() : null;
        if (contents == null) return copies;
        for (int i = 0; i < contents.Count; i++)
        {
            var item = contents[i];
            if (item?.Definition == null) continue;
            var copy = item.GetCopy(-1);
            if (copy != null) copies.Add(copy);
        }
        return copies;
    }

    private static void Note(string text)
    {
        MelonLogger.Warning("TownHallInterior: " + text);
        var notes = NotificationsManager.Instance;
        if (notes != null) notes.SendNotification("Inventory", text, null, 4f, true);
    }

    private static bool FootprintClear(GameObject ghost)
    {
        var feet = ghost.GetComponentsInChildren<FootprintTile>(true);
        if (feet == null || feet.Length == 0)
            return !BlockedBySolid(ghost.transform.position, ghost);
        foreach (var foot in feet)
        {
            if (foot == null) continue;
            var tile = TileUnder(foot.transform.position);
            if (tile != null && TileTaken(tile)) return false;
            if (BlockedBySolid(foot.transform.position, ghost)) return false;
        }
        return true;
    }

    private static void ClaimFootprint(GameObject placed)
    {
        var item = placed.GetComponent<GridItem>() ?? placed.GetComponentInChildren<GridItem>(true);
        if (item == null) return;
        foreach (var foot in placed.GetComponentsInChildren<FootprintTile>(true))
        {
            var tile = TileUnder(foot.transform.position);
            if (tile == null || tile.OwnerGrid == null || !IsOurs(tile.OwnerGrid)) continue;
            tile.AddOccupant(item, foot);
        }
    }

    private static void ReleaseFootprint(GameObject placed)
    {
        var item = placed.GetComponent<GridItem>() ?? placed.GetComponentInChildren<GridItem>(true);
        if (item == null) return;
        foreach (var foot in placed.GetComponentsInChildren<FootprintTile>(true))
        {
            var tile = TileUnder(foot.transform.position);
            if (tile == null) continue;
            tile.RemoveOccupant(item, foot);
        }
    }

    private static bool TileTaken(Tile tile)
    {
        var occupants = tile.BuildableOccupants;
        return occupants != null && occupants.Count > 0;
    }

    public static void OfferHallPickup(InteractableObject __instance)
    {
        if (__instance == null || PlacedRoot(__instance.transform) == null) return;
        var text = SafeMessage(__instance);
        if (text == null || text.IndexOf("fit", StringComparison.OrdinalIgnoreCase) < 0) return;
        __instance.message = "Pick up";
        __instance.SetInteractableState(InteractableObject.EInteractableState.Default);
    }

    private static string? SafeMessage(InteractableObject? obj)
    {
        try { return obj?.message; }
        catch (ArgumentException) { return null; }
    }

    public static bool StartHallPickup(InteractableObject __instance)
    {
        var root = __instance == null ? null : PlacedRoot(__instance.transform);
        if (root == null) return true;
        if (ContentsUnresolved(root)) return false;
        var text = SafeMessage(__instance) ?? "";
        if (text.IndexOf("Pick up", StringComparison.Ordinal) < 0
            && text.IndexOf("fit", StringComparison.OrdinalIgnoreCase) < 0)
            return true;
        var item = root.GetComponent<BuildableItem>() ?? root.GetComponentInChildren<BuildableItem>(true);
        if (item == null) return true;
        PickupHallItem(item);
        return false;
    }

    private static GameObject? PlacedRoot(Transform? node)
    {
        while (node != null)
        {
            if (node.name.StartsWith(PlacedPrefix, StringComparison.Ordinal)
                || PlacedIds.ContainsKey(node.gameObject.GetInstanceID()))
                return node.gameObject;
            node = node.parent;
        }
        return null;
    }

    private static bool BlockedBySolid(Vector3 world, GameObject ghost)
    {
        var hits = Physics.OverlapBox(world + Vector3.up * 0.55f, new Vector3(0.18f, 0.28f, 0.18f),
            Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);
        foreach (var hit in hits)
        {
            if (hit == null || hit.isTrigger) continue;
            var node = hit.transform;
            if (node == ghost.transform || node.IsChildOf(ghost.transform)) continue;
            if (IsPlayer(node) || IsHallGrid(node)) continue;
            var size = hit.bounds.size;
            if (size.x > 4f || size.y > 4f || size.z > 4f) continue;
            return true;
        }
        return false;
    }

    private static bool IsPlayer(Transform node)
    {
        while (node != null)
        {
            if (node.GetComponent<Player>() != null) return true;
            node = node.parent;
        }
        return false;
    }

    private static bool IsHallGrid(Transform node)
    {
        while (node != null)
        {
            if (node.name.StartsWith("TownHallInterior-grid", StringComparison.Ordinal)) return true;
            node = node.parent;
        }
        return false;
    }

    private static Tile? TileUnder(Vector3 world)
    {
        if (_root == null) return null;
        Tile? best = null;
        float bestDistance = Grid.TileSize * 0.55f;
        foreach (var tile in _root.GetComponentsInChildren<Tile>())
        {
            if (tile == null) continue;
            var position = tile.transform.position;
            float distance = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(world.x, world.z));
            if (distance >= bestDistance) continue;
            bestDistance = distance;
            best = tile;
        }
        return best;
    }

    private static bool OverHall(Vector3 point)
    {
        if (Math.Abs(point.y - PublicEntrance.FloorTop) < 1.2f && Inside(HallGridPlan.Ground, point.x, point.z))
            return true;
        if (Math.Abs(point.y - RoomPlan.Upper) >= 1.2f) return false;
        if (Inside(HallGridPlan.StairVoid, point.x, point.z)) return false;
        foreach (var floor in HallGridPlan.Upper)
            if (Inside(floor, point.x, point.z)) return true;
        return false;
    }

    private static bool Inside(HallGridRect rect, float x, float z) =>
        x >= rect.MinX && x <= rect.MaxX && z >= rect.MinZ && z <= rect.MaxZ;

    private static int LocalIndex(float world, float origin, float size) =>
        (int)Math.Round((world - (origin + size * 0.5f)) / size);

    private static bool IsOurs(Grid grid) =>
        grid.gameObject != null && grid.gameObject.name.StartsWith("TownHallInterior-grid-");

    private static string PathOf(Transform node)
    {
        var names = new List<string>();
        while (node != null && names.Count < 6)
        {
            names.Add(node.name);
            node = node.parent;
        }
        names.Reverse();
        return string.Join("/", names);
    }
}
