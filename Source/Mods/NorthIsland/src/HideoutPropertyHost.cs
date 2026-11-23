extern alias il2cpp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Il2CppFishNet;
using Il2CppFishNet.Object;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.Building;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Dialogue;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.Interaction;
using Il2CppScheduleOne.Map;
using Il2CppTMPro;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Persistence.Loaders;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Storage;
using Il2CppScheduleOne.Tiles;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.Events;

namespace NorthIsland;

/// <summary>
/// Publishes the accepted Hideout warehouse as a singleplayer property with a build grid
/// and a door-side purchase interaction. Ownership and placements live in the slot's
/// Modded folder, not in native Saveables.
/// </summary>
internal static class HideoutPropertyHost
{
    private const string HarmonyId = "NorthIsland.HideoutProperty";
    private const string PlacedPrefix = "NorthshoreHideout-placed:";
    private const string GridName = "Northshore_HideoutGrid";
    private static readonly Guid GridGuid = new("6f0c1a2e-91b4-4c3d-8e55-0a11b0de0001");

    private static HarmonyLib.Harmony? _harmony;
    private static bool _saveHooked;
    private static GameObject? _propertyRoot;
    private static GameObject? _gridHost;
    private static GameObject? _placedRoot;
    private static GameObject? _sign;
    private static Il2CppScheduleOne.Property.Property? _property;
    private static Grid? _grid;
    private static bool _rayChoiceAdded;
    private static HideoutSaveFile? _save;
    private static HideoutSaveKind _saveKind = HideoutSaveKind.Missing;
    private static string _saveFolder = "";
    private static string _savePath = "";
    private static bool _owned;
    private static bool _purchaseReady;
    private static bool _purchaseBusy;
    private static int _tileCount;
    private static readonly Dictionary<string, GameObject> Live = new(StringComparer.Ordinal);
    private static readonly Dictionary<int, string> RecordIds = new();
    private static readonly HashSet<string> FailedContents = new(StringComparer.Ordinal);
    private static readonly HashSet<string> Spawned = new(StringComparer.Ordinal);

    internal static string Status =>
        _property == null
            ? "Hideout nicht registriert"
            : $"Hideout {(_owned ? "gekauft" : "verkauft sich fuer $" + HideoutPropertyPlan.Price.ToString("0"))}, Kacheln={_tileCount}, Save={_saveKind}";

    internal static void Publish(Transform worldRoot)
    {
        Shutdown();
        InstallPatches();
        HookSave();

        var warehouse = FindWarehouse(worldRoot);
        if (warehouse == null)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Warehouse nicht gefunden. Property bleibt aus.");
            return;
        }

        if (!TryBindSave())
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Kauf ist gesperrt, bis der Spielstand eindeutig gelesen werden kann.");
        }

        try
        {
            if (CodeAlreadyRegistered())
                throw new InvalidOperationException("property-code-collision");

            BuildProperty(warehouse);
            if (!AttachNativeSign(warehouse))
                throw new InvalidOperationException("hideout-native-sign-missing");
            _propertyRoot!.SetActive(true);
            PaintSign();
            BuildGrid(warehouse);
            RegisterLists();
            ApplyOwnership(_owned);
            if (_owned)
                RestorePlacements();
            RefreshSign();
            MelonLogger.Msg($"[NorthIsland] {Status}.");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Property verworfen: " + ex.Message);
            Shutdown();
        }
    }

    internal static void Shutdown()
    {
        DetachLists();
        Live.Clear();
        RecordIds.Clear();
        FailedContents.Clear();
        Spawned.Clear();
        _save = null;
        _saveKind = HideoutSaveKind.Missing;
        _saveFolder = "";
        _savePath = "";
        _owned = false;
        _purchaseReady = false;
        _purchaseBusy = false;
        _tileCount = 0;
        _property = null;
        _grid = null;
        _rayChoiceAdded = false;
        DestroyIfAlive(_sign);
        DestroyIfAlive(_placedRoot);
        DestroyIfAlive(_gridHost);
        DestroyIfAlive(_propertyRoot);
        _sign = null;
        _placedRoot = null;
        _gridHost = null;
        _propertyRoot = null;
    }

    internal static void DisposePatches()
    {
        if (_saveHooked)
        {
            GameLifecycle.OnSaveStart -= FlushSave;
            _saveHooked = false;
        }

        try { _harmony?.UnpatchSelf(); }
        catch (Exception ex) { MelonLogger.Warning("[NorthIsland] Hideout-Patches: " + ex.Message); }
        _harmony = null;
    }

    private static void InstallPatches()
    {
        if (_harmony != null)
            return;

        _harmony = new HarmonyLib.Harmony(HarmonyId);
        Patch(typeof(Il2CppScheduleOne.Property.Property), nameof(Il2CppScheduleOne.Property.Property.ShouldSave), nameof(SkipNativeSave));
        Patch(typeof(Il2CppScheduleOne.Property.Property), nameof(Il2CppScheduleOne.Property.Property.Load), nameof(SkipNativeLoad));
        Patch(typeof(Grid), nameof(Grid.IsTileValidAtCoordinate), nameof(ValidateHideoutTile));
        Patch(typeof(BuildUpdate_Grid), nameof(BuildUpdate_Grid.CheckIntersections), nameof(MarkHideoutGhost), finalizer: nameof(KeepHideoutGhostAlive));
        Patch(typeof(BuildUpdate_Grid), nameof(BuildUpdate_Grid.Place), nameof(PlaceOnHideout));
        Patch(typeof(BuildableItem), nameof(BuildableItem.InitializeSaveable), nameof(SkipPlacedSaveable));
        Patch(typeof(BuildableItem), nameof(BuildableItem.PickupItem), nameof(PickupHideoutItem));
        Patch(typeof(InteractableObject), nameof(InteractableObject.StartInteract), nameof(StartPrompt));
        Patch(typeof(ControlledDialogueHandler), "ModifyChoiceList", nameof(OfferAtRay));
        Patch(typeof(DialogueHandler_EstateAgent), nameof(DialogueHandler_EstateAgent.ModifyChoiceText), nameof(PriceAtRay));
        Patch(typeof(DialogueHandler_EstateAgent), nameof(DialogueHandler_EstateAgent.ShouldChoiceBeShown), nameof(ShowAtRay));
        Patch(typeof(DialogueHandler_EstateAgent), nameof(DialogueHandler_EstateAgent.CheckChoice), nameof(AllowAtRay));
        Patch(typeof(DialogueHandler_EstateAgent), nameof(DialogueHandler_EstateAgent.ChoiceCallback), nameof(BuyAtRay));
    }

    private static void Patch(Type type, string name, string prefix, string? finalizer = null)
    {
        var method = AccessTools.Method(type, name);
        PatchGuard.TryPatch(
            _harmony!,
            method,
            prefix: new HarmonyMethod(typeof(HideoutPropertyHost), prefix),
            finalizer: finalizer == null ? null : new HarmonyMethod(typeof(HideoutPropertyHost), finalizer));
    }

    private static void HookSave()
    {
        if (_saveHooked)
            return;
        GameLifecycle.OnSaveStart += FlushSave;
        _saveHooked = true;
    }

    private static Transform? FindWarehouse(Transform worldRoot)
    {
        foreach (var transform in worldRoot.GetComponentsInChildren<Transform>(true))
        {
            if (transform != null && transform.name == "Northshore_HideoutWarehouse")
                return transform;
        }

        return null;
    }

    private static bool CodeAlreadyRegistered()
    {
        var list = Il2CppScheduleOne.Property.Property.Properties;
        if (list == null)
            return false;
        for (int i = 0; i < list.Count && i < 512; i++)
        {
            var property = list[i];
            if (property != null && property.propertyCode == HideoutPropertyPlan.Code)
                return true;
        }

        return false;
    }

    private static bool TryBindSave()
    {
        _purchaseReady = false;
        _owned = false;
        _save = null;
        _saveKind = HideoutSaveKind.Missing;
        _saveFolder = "";
        _savePath = "";

        var load = PersistentSingleton<LoadManager>.Instance;
        string folder = load != null ? load.LoadedGameFolderPath ?? "" : "";
        if (string.IsNullOrWhiteSpace(folder) || folder.IndexOf("..", StringComparison.Ordinal) >= 0)
        {
            _saveKind = HideoutSaveKind.Corrupt;
            return false;
        }

        _saveFolder = folder;
        _savePath = Path.Combine(folder, "Modded", "NorthIsland", "hideout.json");
        if (!Path.GetFullPath(_savePath).StartsWith(Path.GetFullPath(folder), StringComparison.OrdinalIgnoreCase))
        {
            _saveKind = HideoutSaveKind.Corrupt;
            _savePath = "";
            return false;
        }

        bool exists = File.Exists(_savePath);
        HideoutSaveFile? loaded = exists
            ? SafeStorage.LoadSafe<HideoutSaveFile>(_savePath, new HideoutSaveFile())
            : null;
        if (exists && loaded != null && loaded.Schema == 0 && string.IsNullOrEmpty(loaded.PropertyCode))
            loaded = null;

        _saveKind = HideoutSaveRules.Classify(loaded, folder, exists);
        if (_saveKind == HideoutSaveKind.Missing)
        {
            _save = new HideoutSaveFile { SaveFolder = folder };
            _purchaseReady = true;
            return true;
        }

        if (_saveKind != HideoutSaveKind.Ready || loaded == null)
            return false;

        _save = loaded;
        _owned = loaded.Owned;
        _purchaseReady = true;
        return true;
    }

    private static void BuildProperty(Transform warehouse)
    {
        var root = new GameObject("Northshore_HideoutProperty");
        root.transform.SetParent(warehouse, false);
        root.transform.position = warehouse.position;
        root.SetActive(false);
        _propertyRoot = root;

        var network = root.AddComponent<NetworkObject>();
        var property = root.AddComponent<Il2CppScheduleOne.Property.Property>();
        network._networkBehaviours = new Il2CppReferenceArray<NetworkBehaviour>(new NetworkBehaviour[] { property });
        property._networkObjectCache = network;
        property._componentIndexCache = 0;
        property.propertyCode = HideoutPropertyPlan.Code;
        property.propertyName = HideoutPropertyPlan.Name;
        property.OwnedByDefault = false;
        property._IsOwned_k__BackingField = false;
        property.IsOwnedVariable = "";
        property.Price = HideoutPropertyPlan.Price;
        property.EmployeeCapacity = HideoutPropertyPlan.EmployeeCapacity;
        property.AvailableInDemo = true;
        property.DefaultRotation = 270f;
        property.ContentCullingEnabled = false;
        property._hasBeenLoaded = true;
        property._HasChanged_k__BackingField = false;
        property.onThisPropertyAcquired = new UnityEvent();
        property._Employees_k__BackingField = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Employees.Employee>();
        property.BuildableItems = new il2cpp::Il2CppSystem.Collections.Generic.List<BuildableItem>();
        property.Configurables = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Management.IConfigurable>();
        property.Grids = new il2cpp::Il2CppSystem.Collections.Generic.List<Grid>();
        property.Switches = new il2cpp::Il2CppSystem.Collections.Generic.List<Il2CppScheduleOne.Misc.ModularSwitch>();
        property.Toggleables = new il2cpp::Il2CppSystem.Collections.Generic.List<InteractableToggleable>();
        property._LocalExtraFiles_k__BackingField = new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
        property._LocalExtraFolders_k__BackingField = new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
        property.savedObjectPaths = new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
        property.savedEmployeePaths = new il2cpp::Il2CppSystem.Collections.Generic.List<string>();
        property.ObjectsToCull = new Il2CppReferenceArray<GameObject>(0);
        property.LoadingDocks = new Il2CppReferenceArray<Il2CppScheduleOne.Delivery.LoadingDock>(0);
        property.SpawnPoint = Point(root.transform, "Spawn", new Vector3(HideoutPropertyPlan.SpawnX, HideoutPropertyPlan.SignY + 1.1f, HideoutPropertyPlan.SpawnZ));
        property.InteriorSpawnPoint = Point(root.transform, "InteriorSpawn", new Vector3(HideoutPropertyPlan.CenterX, HideoutPropertyPlan.FloorTopY + 0.2f, HideoutPropertyPlan.CenterZ));
        property.NPCSpawnPoint = Point(root.transform, "NPCSpawn", new Vector3(HideoutPropertyPlan.SpawnX, HideoutPropertyPlan.SignY + 1.1f, HideoutPropertyPlan.SpawnZ + 1.2f));
        var idle = Point(root.transform, "Idle", new Vector3(HideoutPropertyPlan.CenterX - 2f, HideoutPropertyPlan.FloorTopY + 0.05f, HideoutPropertyPlan.CenterZ));
        property.EmployeeIdlePoints = new Il2CppReferenceArray<Transform>(new[] { idle });

        var boundsObject = new GameObject("Bounds");
        boundsObject.transform.SetParent(root.transform, false);
        boundsObject.transform.position = warehouse.position;
        var bounds = boundsObject.AddComponent<BoxCollider>();
        bounds.isTrigger = true;
        bounds.center = new Vector3(0f, 2.6f, 0f);
        bounds.size = new Vector3(HideoutPropertyPlan.WidthX + 1.2f, 6.2f, HideoutPropertyPlan.LengthZ + 1.2f);
        property.BoundingBox = boundsObject;
        property.propertyBoundsColliders = new Il2CppReferenceArray<BoxCollider>(new[] { bounds });

        var container = new GameObject("Contents").AddComponent<Il2CppScheduleOne.Property.PropertyContentsContainer>();
        container.transform.SetParent(root.transform, false);
        container._Property_k__BackingField = property;
        property._Container_k__BackingField = container;
        var ui = new GameObject("WorldspaceUI").AddComponent<RectTransform>();
        ui.SetParent(root.transform, false);
        property._WorldspaceUIContainer_k__BackingField = ui;
        property.loader = new PropertyLoader();
        _property = property;
    }

    private static Transform Point(Transform parent, string name, Vector3 world)
    {
        var point = new GameObject(name);
        point.transform.SetParent(parent, false);
        point.transform.position = world;
        return point.transform;
    }

    private static void BuildGrid(Transform warehouse)
    {
        float size = Grid.TileSize;
        var tiles = HideoutPropertyPlan.Tiles(size);
        if (tiles.Count == 0)
            throw new InvalidOperationException("hideout-grid-empty");

        var donor = FindDonor();
        if (donor == null)
            throw new InvalidOperationException("hideout-donor-missing");

        float originX = float.MaxValue;
        float originZ = float.MaxValue;
        foreach (var tile in tiles)
        {
            originX = Math.Min(originX, tile.WorldX - size * 0.5f);
            originZ = Math.Min(originZ, tile.WorldZ - size * 0.5f);
        }

        var host = new GameObject(GridName);
        host.transform.SetParent(warehouse, false);
        host.transform.position = new Vector3(originX, HideoutPropertyPlan.FloorTopY, originZ);
        host.SetActive(false);
        _gridHost = host;
        var grid = host.AddComponent<Grid>();
        grid.Tiles = new il2cpp::Il2CppSystem.Collections.Generic.List<Tile>();
        grid._parentProperty = _property;
        try
        {
            grid.SetGUID(new il2cpp::Il2CppSystem.Guid(GridGuid.ToString()));
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Grid-GUID neu erzeugt: " + ex.Message);
            grid.RegenerateGUID();
        }

        float offset = donor.GetComponent<Tile>().AvailableOffset;
        var template = UnityEngine.Object.Instantiate(donor, host.transform);
        template.SetActive(false);
        template.name = GridName + "-template";
        foreach (var tile in tiles)
        {
            var clone = UnityEngine.Object.Instantiate(template, host.transform);
            clone.name = GridName + "-" + tile.X + "-" + tile.Z;
            clone.transform.position = new Vector3(tile.WorldX, HideoutPropertyPlan.FloorTopY + 0.02f, tile.WorldZ);
            FitCollider(clone, size);
            var piece = clone.GetComponent<Tile>();
            piece.OwnerGrid = grid;
            piece.x = tile.X;
            piece.y = tile.Z;
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
        if (_property?.Grids != null)
            _property.Grids.Add(grid);
        host.SetActive(true);
        _grid = grid;
        _tileCount = tiles.Count;

        _placedRoot = new GameObject("Northshore_HideoutPlaced");
        _placedRoot.transform.SetParent(warehouse, false);
    }

    private static GameObject? FindDonor()
    {
        GameObject? fallback = null;
        foreach (var tile in UnityEngine.Object.FindObjectsOfType<Tile>())
        {
            if (tile == null || tile.OwnerGrid == null)
                continue;
            var go = tile.gameObject;
            if (go.name.StartsWith("Northshore_Hideout") || go.name.StartsWith("TownHallInterior"))
                continue;
            if (go.GetComponent<Collider>() == null)
                continue;
            if (go.GetComponent<IndoorTile>() != null)
                return go;
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

    private static bool AttachNativeSign(Transform warehouse)
    {
        Il2CppScheduleOne.Property.Property? donor = null;
        int best = -1;
        foreach (var property in Resources.FindObjectsOfTypeAll<Il2CppScheduleOne.Property.Property>())
        {
            if (property == null || property.ForSaleSign == null || !property.gameObject.scene.IsValid())
                continue;
            if (property.propertyCode == HideoutPropertyPlan.Code)
                continue;
            int score = SignDonorScore(property);
            if (score > best)
            {
                best = score;
                donor = property;
            }
        }

        if (donor?.ForSaleSign == null || _property == null)
            return false;

        var sign = CloneInactive(donor.ForSaleSign, warehouse);
        sign.name = "Northshore_HideoutForSale";
        sign.transform.localScale = Vector3.one;
        SeatSign(sign);
        _sign = sign;
        MelonLogger.Msg("[NorthIsland] Kaufschild von " + (donor.propertyName ?? donor.gameObject.name) + " geklont.");
        _property.ForSaleSign = sign;
        if (donor.PoI != null && donor.PoI.gameObject != donor.gameObject && donor.ForSaleSign != donor.PoI.gameObject)
        {
            var poi = CloneInactive(donor.PoI.gameObject, _propertyRoot!.transform).GetComponent<POI>();
            if (poi != null)
            {
                poi.DefaultMainText = HideoutPropertyPlan.Name;
                poi._MainText_k__BackingField = HideoutPropertyPlan.Name;
                poi.AutoUpdatePosition = false;
                _property.PoI = poi;
            }
        }

        PaintSign();
        return true;
    }

    private static int SignDonorScore(Il2CppScheduleOne.Property.Property property)
    {
        string objectName = property.gameObject != null ? property.gameObject.name : "";
        string name = property.propertyName ?? objectName;
        bool barn = name.Equals("Barn", StringComparison.OrdinalIgnoreCase)
            || objectName.Equals("Barn", StringComparison.OrdinalIgnoreCase);
        bool pole = FindNamed(property.ForSaleSign.transform, "Pole") != null;
        if (barn && pole)
            return 3;
        if (pole)
            return 2;
        if (barn)
            return 1;
        return 0;
    }

    private static Transform? FindNamed(Transform root, string name)
    {
        if (root.name.Equals(name, StringComparison.OrdinalIgnoreCase))
            return root;
        for (int i = 0; i < root.childCount; i++)
        {
            var found = FindNamed(root.GetChild(i), name);
            if (found != null)
                return found;
        }

        return null;
    }

    private static void SeatSign(GameObject sign)
    {
        // Same yaw the readable face already used, but the root sits like the Barn:
        // 0.70 m above the soil, so the cloned Pole hangs down instead of the board sticking to the wall.
        sign.transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        sign.transform.position = new Vector3(
            HideoutPropertyPlan.SignX,
            HideoutPropertyPlan.SignY + HideoutPropertyPlan.SignRootLift,
            HideoutPropertyPlan.SignZ);

        var pole = FindNamed(sign.transform, "Pole");
        if (pole != null)
            pole.gameObject.SetActive(true);

        foreach (var collider in sign.GetComponentsInChildren<Collider>(true))
        {
            if (collider == null)
                continue;
            if (collider is not BoxCollider box)
                continue;
            var scaled = Vector3.Scale(box.size, collider.transform.lossyScale);
            if (Mathf.Abs(scaled.x) > 2.5f || Mathf.Abs(scaled.z) > 2.5f)
                collider.enabled = false;
        }

        MelonLogger.Msg($"[NorthIsland] Kaufschild auf den Stock gesetzt ({HideoutPropertyPlan.SignX:0.00}, {HideoutPropertyPlan.SignZ:0.00}, Pole={(pole != null)}).");
    }

    private static GameObject CloneInactive(GameObject donor, Transform parent)
    {
        bool restore = donor.activeSelf;
        if (restore)
            donor.SetActive(false);
        var clone = UnityEngine.Object.Instantiate(donor, parent);
        if (restore)
            donor.SetActive(true);
        clone.SetActive(false);
        return clone;
    }

    private static void PaintSign()
    {
        if (_sign == null)
            return;
        foreach (var text in _sign.GetComponentsInChildren<TMP_Text>(true))
        {
            if (text == null)
                continue;
            string name = text.gameObject.name;
            if (name.Equals("Name", StringComparison.OrdinalIgnoreCase) || name.IndexOf("Title", StringComparison.OrdinalIgnoreCase) >= 0)
                text.text = HideoutPropertyPlan.Name;
            else if (name.Equals("Price", StringComparison.OrdinalIgnoreCase) || name.IndexOf("Price", StringComparison.OrdinalIgnoreCase) >= 0)
                text.text = HideoutPropertyPlan.PriceLabel;
        }
    }

    private static void RegisterLists()
    {
        var property = _property ?? throw new InvalidOperationException("hideout-property-missing");
        AddOnce(Il2CppScheduleOne.Property.Property.Properties, property);
        if (_owned)
            AddOnce(Il2CppScheduleOne.Property.Property.OwnedProperties, property);
        else
            AddOnce(Il2CppScheduleOne.Property.Property.UnownedProperties, property);
    }

    private static void ApplyOwnership(bool owned)
    {
        if (_property == null)
            return;
        _owned = owned;
        _property._IsOwned_k__BackingField = owned;
        _property.IsOwnedVariable = owned ? "1" : "";
        RemovePointer(Il2CppScheduleOne.Property.Property.OwnedProperties, _property);
        RemovePointer(Il2CppScheduleOne.Property.Property.UnownedProperties, _property);
        AddOnce(owned
            ? Il2CppScheduleOne.Property.Property.OwnedProperties
            : Il2CppScheduleOne.Property.Property.UnownedProperties, _property);
        RefreshSign();
    }

    private static void RefreshSign()
    {
        PaintSign();
        if (_sign != null)
            _sign.SetActive(!_owned);
    }

    public static bool StartPrompt(InteractableObject __instance)
    {
        if (__instance == null)
            return true;
        var placed = PlacedRoot(__instance.transform);
        if (placed == null)
            return true;
        var text = __instance.message ?? "";
        if (text.IndexOf("Pick up", StringComparison.OrdinalIgnoreCase) < 0
            && text.IndexOf("fit", StringComparison.OrdinalIgnoreCase) < 0)
            return true;
        var item = placed.GetComponent<BuildableItem>() ?? placed.GetComponentInChildren<BuildableItem>(true);
        if (item != null)
            PickupHideoutItem(item);
        return false;
    }

    public static void OfferAtRay(
        ControlledDialogueHandler __instance,
        ref il2cpp::Il2CppSystem.Collections.Generic.List<DialogueChoiceData> existingChoices)
    {
        try
        {
            if (__instance == null || existingChoices == null || existingChoices.Count == 0 || _owned || !_purchaseReady)
                return;
            if (!__instance.GetIl2CppType().Name.Contains("EstateAgent"))
                return;

            bool propertyMenu = false;
            for (int i = 0; i < existingChoices.Count; i++)
            {
                var choice = existingChoices[i];
                if (choice == null)
                    continue;
                if (string.Equals(choice.ChoiceLabel, HideoutPropertyPlan.ChoiceLabel, StringComparison.Ordinal))
                    return;
                if (HideoutListing.IsPropertyMenu(choice.ChoiceLabel, choice.ChoiceText))
                    propertyMenu = true;
            }

            if (!propertyMenu)
                return;

            var ours = new DialogueChoiceData
            {
                Guid = Guid.NewGuid().ToString(),
                ChoiceLabel = HideoutPropertyPlan.ChoiceLabel,
                ChoiceText = HideoutPropertyPlan.Name
            };
            existingChoices.Insert(Math.Max(0, existingChoices.Count - 1), ours);
            if (!_rayChoiceAdded)
            {
                _rayChoiceAdded = true;
                MelonLogger.Msg("[NorthIsland] Hideout steht bei Ray in der Immobilienliste.");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[NorthIsland] Ray-Liste: " + ex.Message);
        }
    }

    public static void PriceAtRay(string choiceLabel, ref string __result)
    {
        if (string.Equals(choiceLabel, HideoutPropertyPlan.ChoiceLabel, StringComparison.Ordinal))
            __result = HideoutPropertyPlan.Name + " (" + HideoutPropertyPlan.PriceLabel + ")";
    }

    public static bool ShowAtRay(string choiceLabel, ref bool __result)
    {
        if (!string.Equals(choiceLabel, HideoutPropertyPlan.ChoiceLabel, StringComparison.Ordinal))
            return true;
        __result = _purchaseReady && !_owned;
        return false;
    }

    public static bool AllowAtRay(string choiceLabel, out string invalidReason, ref bool __result)
    {
        invalidReason = string.Empty;
        if (!string.Equals(choiceLabel, HideoutPropertyPlan.ChoiceLabel, StringComparison.Ordinal))
            return true;
        if (!_purchaseReady || _owned)
        {
            invalidReason = "UNAVAILABLE";
            __result = false;
            return false;
        }

        if (!TryReadWallet(out float bank, out float cash))
        {
            invalidReason = "UNAVAILABLE";
            __result = false;
            return false;
        }

        var decision = HideoutPurchase.Evaluate(true, false, bank, cash);
        __result = decision.Status == HideoutPurchaseStatus.Purchased;
        if (!__result)
            invalidReason = "INSUFFICIENT BALANCE";
        return false;
    }

    public static bool BuyAtRay(DialogueHandler_EstateAgent __instance, string choiceLabel)
    {
        if (!string.Equals(choiceLabel, HideoutPropertyPlan.ChoiceLabel, StringComparison.Ordinal))
            return true;
        TryPurchase();
        try { __instance?.EndDialogue(); } catch { /* dialogue can already be closing */ }
        return false;
    }

    private static void TryPurchase()
    {
        if (_purchaseBusy || _property == null)
            return;
        if (!NetworkGuard.IsHostOrSingleplayer())
        {
            MelonLogger.Msg("[NorthIsland] Hideout-Kauf gibt es nur im Singleplayer.");
            return;
        }

        if (!TryReadWallet(out float bank, out float cash))
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Kauf abgebrochen, Kontostand nicht lesbar.");
            return;
        }

        var decision = HideoutPurchase.Evaluate(_purchaseReady && _saveKind is HideoutSaveKind.Missing or HideoutSaveKind.Ready, _owned, bank, cash);
        if (decision.Status != HideoutPurchaseStatus.Purchased)
        {
            MelonLogger.Msg("[NorthIsland] Hideout-Kauf: " + decision.Status);
            RefreshSign();
            return;
        }

        _purchaseBusy = true;
        try
        {
            var charge = ChangeBalance(-decision.Amount, decision.ChargeBank);
            if (charge != WalletOutcome.Applied)
            {
                if (charge == WalletOutcome.Unknown)
                    ChangeBalance(decision.Amount, decision.ChargeBank);
                MelonLogger.Warning("[NorthIsland] Hideout-Zahlung nicht bestaetigt. Besitz unveraendert.");
                return;
            }

            ApplyOwnership(true);
            if (_save != null)
            {
                _save.Owned = true;
                _save.SaveFolder = _saveFolder;
                _save.PropertyCode = HideoutPropertyPlan.Code;
                _save.Schema = HideoutPropertyPlan.Schema;
            }

            if (!WriteSave())
            {
                ApplyOwnership(false);
                if (_save != null)
                    _save.Owned = false;
                var refund = ChangeBalance(decision.Amount, decision.ChargeBank);
                MelonLogger.Warning(refund == WalletOutcome.Applied
                    ? "[NorthIsland] Hideout-Speichern fehlgeschlagen. Zahlung zurueckgebucht."
                    : "[NorthIsland] Hideout-Speichern fehlgeschlagen und Rueckbuchung nicht bestaetigt.");
                return;
            }

            MelonLogger.Msg("[NorthIsland] Northshore Hideout gekauft.");
            RefreshSign();
        }
        finally
        {
            _purchaseBusy = false;
        }
    }

    public static bool SkipNativeSave(Il2CppScheduleOne.Property.Property __instance, ref bool __result)
    {
        if (!IsOurs(__instance))
            return true;
        __result = false;
        return false;
    }

    public static bool SkipNativeLoad(Il2CppScheduleOne.Property.Property __instance)
    {
        if (!IsOurs(__instance))
            return true;
        __instance._hasBeenLoaded = true;
        return false;
    }

    public static bool ValidateHideoutTile(Grid __instance, Coordinate gridCoord, FootprintTile tile, ref bool __result)
    {
        if (!IsOurGrid(__instance))
            return true;
        __result = false;
        if (!_owned || gridCoord == null)
            return false;
        Tile? found;
        try { found = __instance.GetTile(gridCoord); }
        catch { return false; }
        if (found == null)
            return false;
        if (tile != null && found.AvailableOffset + 0.001f < tile.RequiredOffset)
            return false;
        var occupants = found.BuildableOccupants;
        if (occupants != null && occupants.Count > 0)
            return false;
        __result = true;
        return false;
    }

    public static void MarkHideoutGhost(BuildUpdate_Grid __instance)
    {
        try
        {
            if (__instance?.GhostModel == null || !OverInterior(__instance.GhostModel.transform.position))
                return;
            bool clear = _owned && FootprintClear(__instance.GhostModel);
            var manager = NetworkSingleton<BuildManager>.Instance;
            __instance._validPosition = clear;
            if (manager == null)
                return;
            var material = clear ? manager.ghostMaterial_White : manager.ghostMaterial_Red;
            if (material == null)
                return;
            __instance._currentGhostMaterial = material;
            manager.ApplyMaterial(__instance.GhostModel, material, true);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Ghost: " + ex.Message);
        }
    }

    public static Exception? KeepHideoutGhostAlive(Exception? __exception, BuildUpdate_Grid __instance)
    {
        if (__exception == null)
            return null;
        if (__exception.ToString().IndexOf("NullReferenceException", StringComparison.Ordinal) < 0)
            return __exception;
        if (__instance?.GhostModel == null || !OverInterior(__instance.GhostModel.transform.position))
            return __exception;
        MarkHideoutGhost(__instance);
        return null;
    }

    public static bool PlaceOnHideout(BuildUpdate_Grid __instance, ref GridItem? __result)
    {
        if (__instance?.GhostModel == null || !OverInterior(__instance.GhostModel.transform.position))
            return true;
        __result = null;
        if (!NetworkGuard.IsHostOrSingleplayer() || !_owned || _save == null || !_purchaseReady)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Platzierung abgelehnt.");
            return false;
        }

        GameObject? placed = null;
        try
        {
            if (!FootprintClear(__instance.GhostModel))
                return false;
            var item = __instance.ItemInstance;
            var definition = item?.Definition?.TryCast<BuildableItemDefinition>();
            if (definition?.BuiltItem == null || item?.Definition == null)
                return false;

            placed = SpawnBuilt(definition, __instance.GhostModel.transform.position, __instance.GhostModel.transform.rotation, item.Definition.ID, item);
            if (placed == null)
                return false;
            var refund = item.GetCopy(1) ?? definition.GetDefaultInstance(1);
            var inventory = PlayerInventory.Instance;
            inventory?.RemoveAmountOfItem(item.Definition.ID, 1u);
            Remember(placed, item.Definition.ID);
            if (!WriteSave())
            {
                Forget(placed);
                ReleaseFootprint(placed);
                UnityEngine.Object.Destroy(placed);
                if (refund != null && inventory != null)
                    inventory.AddItemToInventory(refund);
                MelonLogger.Warning("[NorthIsland] Hideout-Platzierung nicht gespeichert. Objekt entfernt und Item zurueckgelegt.");
                return false;
            }

            __instance.Stop();
            MelonLogger.Msg("[NorthIsland] Im Hideout platziert: " + item.Definition.ID);
            return false;
        }
        catch (Exception ex)
        {
            if (placed != null)
            {
                Forget(placed);
                UnityEngine.Object.Destroy(placed);
            }

            MelonLogger.Warning("[NorthIsland] Hideout-Platzierung: " + ex.Message);
            return false;
        }
    }

    public static bool SkipPlacedSaveable(BuildableItem __instance) =>
        __instance == null || PlacedRoot(__instance.transform) == null;

    public static bool PickupHideoutItem(BuildableItem __instance)
    {
        var placed = __instance == null ? null : PlacedRoot(__instance.transform);
        if (placed == null)
            return true;
        if (!RecordIds.TryGetValue(placed.GetInstanceID(), out var id) || FailedContents.Contains(id))
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Aufnahme blockiert, Inhalt nicht sicher.");
            return false;
        }

        try
        {
            var returning = CopiesToReturn(placed, ItemIdOf(placed));
            var inventory = PlayerInventory.Instance;
            if (inventory == null || returning.Count == 0)
                return false;
            foreach (var copy in returning)
            {
                if (!inventory.CanItemFitInInventory(copy, 1))
                    return false;
            }

            var kept = _save?.Placements.Find(record => record != null && record.Id == id);
            Forget(placed);
            if (!WriteSave())
            {
                if (kept != null && _save != null)
                {
                    _save.Placements.Add(kept);
                    RecordIds[placed.GetInstanceID()] = kept.Id;
                    Live[kept.Id] = placed;
                }
                MelonLogger.Warning("[NorthIsland] Hideout-Aufnahme nicht gespeichert. Objekt bleibt stehen.");
                return false;
            }

            try
            {
                foreach (var copy in returning)
                    inventory.AddItemToInventory(copy);
            }
            catch (Exception ex)
            {
                if (kept != null && _save != null)
                {
                    _save.Placements.Add(kept);
                    RecordIds[placed.GetInstanceID()] = kept.Id;
                    Live[kept.Id] = placed;
                    WriteSave();
                }
                MelonLogger.Warning("[NorthIsland] Hideout-Aufnahme nicht bestaetigt: " + ex.Message);
                return false;
            }

            ReleaseFootprint(placed);
            UnityEngine.Object.Destroy(placed);
            return false;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Aufnahme: " + ex.Message);
            return false;
        }
    }

    private static void RestorePlacements()
    {
        if (_save?.Placements == null)
            return;
        int restored = 0;
        foreach (var record in _save.Placements)
        {
            if (record == null || !record.IsPlausible() || !Spawned.Add(record.Id))
                continue;
            if (RestoreOne(record))
                restored++;
            else
                Spawned.Remove(record.Id);
        }

        if (_save.Placements.Count > 0)
            MelonLogger.Msg($"[NorthIsland] Hideout-Objekte wiederhergestellt: {restored}/{_save.Placements.Count}.");
    }

    private static bool RestoreOne(HideoutPlacementRecord record)
    {
        var definition = Il2CppScheduleOne.Registry.GetItem(record.ItemId)?.TryCast<BuildableItemDefinition>();
        if (definition?.BuiltItem == null)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Restore ohne Bauvorbild: " + record.ItemId);
            return false;
        }

        var placed = SpawnBuilt(definition, new Vector3(record.X, record.Y, record.Z),
            new Quaternion(record.RotX, record.RotY, record.RotZ, record.RotW), record.ItemId, null);
        if (placed == null)
            return false;
        RecordIds[placed.GetInstanceID()] = record.Id;
        Live[record.Id] = placed;
        FillContents(placed, record);
        return true;
    }

    private static GameObject? SpawnBuilt(BuildableItemDefinition definition, Vector3 position, Quaternion rotation, string itemId, ItemInstance? instance)
    {
        var source = definition.BuiltItem.gameObject;
        bool restore = source.activeSelf;
        if (restore)
            source.SetActive(false);
        GameObject? placed = null;
        try
        {
            placed = UnityEngine.Object.Instantiate(source, position, rotation);
            if (restore)
            {
                source.SetActive(true);
                restore = false;
            }

            if (placed == null)
                return null;
            placed.name = PlacedPrefix + itemId;
            var built = placed.GetComponent<BuildableItem>() ?? placed.GetComponentInChildren<BuildableItem>(true);
            if (built != null && built.ItemInstance == null)
                built._ItemInstance_k__BackingField = instance?.GetCopy(1) ?? definition.GetDefaultInstance(1);
            var net = placed.GetComponent<NetworkObject>();
            var manager = InstanceFinder.NetworkManager;
            if (net != null && manager != null)
                manager.ServerManager.Spawn(net);
            if (_placedRoot != null)
                placed.transform.SetParent(_placedRoot.transform, true);
            placed.transform.SetPositionAndRotation(position, rotation);
            placed.SetActive(true);
            ClaimFootprint(placed);
            return placed;
        }
        catch
        {
            if (restore)
                source.SetActive(true);
            if (placed != null)
                UnityEngine.Object.Destroy(placed);
            throw;
        }
    }

    private static void Remember(GameObject placed, string itemId)
    {
        if (_save == null)
            throw new InvalidOperationException("hideout-save-unbound");
        var rotation = placed.transform.rotation;
        var position = placed.transform.position;
        var record = new HideoutPlacementRecord
        {
            Id = Guid.NewGuid().ToString("N"),
            ItemId = itemId,
            X = position.x,
            Y = position.y,
            Z = position.z,
            RotX = rotation.x,
            RotY = rotation.y,
            RotZ = rotation.z,
            RotW = rotation.w
        };
        if (!record.IsPlausible())
            throw new InvalidOperationException("hideout-pose-outside");
        _save.Placements.Add(record);
        RecordIds[placed.GetInstanceID()] = record.Id;
        Live[record.Id] = placed;
        placed.name = PlacedPrefix + itemId;
    }

    private static void Forget(GameObject placed)
    {
        if (!RecordIds.TryGetValue(placed.GetInstanceID(), out var id))
            return;
        RecordIds.Remove(placed.GetInstanceID());
        Live.Remove(id);
        FailedContents.Remove(id);
        _save?.Placements.RemoveAll(record => record != null && record.Id == id);
    }

    private static void FillContents(GameObject placed, HideoutPlacementRecord record)
    {
        if (record.Contents == null || record.Contents.Count == 0)
            return;
        try
        {
            var slots = placed.GetComponentInChildren<StorageEntity>(true)?.ItemSlots;
            if (slots == null)
                throw new InvalidOperationException("slots-missing");
            foreach (var line in record.Contents)
            {
                if (line == null || line.Slot < 0 || line.Slot >= slots.Count)
                    throw new InvalidOperationException("slot-missing");
                var copy = Il2CppScheduleOne.Registry.GetItem(line.ItemId)?.GetDefaultInstance(line.Quantity);
                if (copy == null)
                    throw new InvalidOperationException("item-missing");
                slots[line.Slot].SetStoredItem(copy, true);
            }

            FailedContents.Remove(record.Id);
        }
        catch (Exception ex)
        {
            FailedContents.Add(record.Id);
            MelonLogger.Warning("[NorthIsland] Hideout-Inhalt bleibt geschuetzt: " + ex.Message);
        }
    }

    private static void CaptureContents()
    {
        if (_save?.Placements == null)
            return;
        foreach (var record in _save.Placements)
        {
            if (record == null || !Live.TryGetValue(record.Id, out var placed) || placed == null)
                continue;
            var read = ReadContents(placed);
            if (read != null)
                record.Contents = read;
        }
    }

    private static List<HideoutSlotRecord>? ReadContents(GameObject placed)
    {
        try
        {
            var storage = placed.GetComponentInChildren<StorageEntity>(true);
            var slots = storage?.ItemSlots;
            if (storage != null && storage.ItemCount > 0 && slots == null)
                return null;
            if (slots == null)
                return new List<HideoutSlotRecord>();
            var found = new List<HideoutSlotRecord>();
            for (int i = 0; i < slots.Count && i < 64; i++)
            {
                var slot = slots[i];
                var item = slot?.ItemInstance ?? slot?._ItemInstance_k__BackingField;
                var id = item?.Definition?.ID;
                int quantity = slot?.Quantity ?? 0;
                if (quantity < 1 && item != null)
                    quantity = 1;
                if (string.IsNullOrWhiteSpace(id) || quantity < 1)
                    continue;
                found.Add(new HideoutSlotRecord { Slot = i, ItemId = id, Quantity = quantity });
            }

            if (storage != null && storage.ItemCount > found.Count)
                return null;
            return found;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Inhalt nicht gelesen: " + ex.Message);
            return null;
        }
    }

    private static List<ItemInstance> CopiesToReturn(GameObject placed, string id)
    {
        var copies = new List<ItemInstance>();
        var shelf = Il2CppScheduleOne.Registry.GetItem(id)?.GetDefaultInstance(1);
        if (shelf != null)
            copies.Add(shelf);
        var storage = placed.GetComponentInChildren<StorageEntity>(true);
        var contents = storage?.GetAllItems();
        if (contents == null)
            return copies;
        for (int i = 0; i < contents.Count; i++)
        {
            var item = contents[i];
            var copy = item?.GetCopy(-1);
            if (copy != null)
                copies.Add(copy);
        }

        return copies;
    }

    private static void FlushSave()
    {
        if (_save == null || _saveKind is HideoutSaveKind.ForeignSlot or HideoutSaveKind.Unsupported or HideoutSaveKind.Corrupt)
            return;
        try
        {
            CaptureContents();
            WriteSave();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Spielstand nicht geschrieben: " + ex.Message);
        }
    }

    private static bool WriteSave()
    {
        if (_save == null || string.IsNullOrWhiteSpace(_savePath) || _saveKind is HideoutSaveKind.ForeignSlot or HideoutSaveKind.Unsupported or HideoutSaveKind.Corrupt)
            return false;
        _save.Schema = HideoutPropertyPlan.Schema;
        _save.PropertyCode = HideoutPropertyPlan.Code;
        _save.SaveFolder = _saveFolder;
        _save.Owned = _owned;
        return SafeStorage.SaveAtomic(_savePath, _save);
    }

    private static bool OverInterior(Vector3 point) =>
        Math.Abs(point.y - HideoutPropertyPlan.FloorTopY) < 1.4f
        && HideoutPropertyPlan.ContainsInterior(point.x, point.z);

    private static bool FootprintClear(GameObject ghost)
    {
        var feet = ghost.GetComponentsInChildren<FootprintTile>(true);
        if (feet == null || feet.Length == 0)
            return TileUnder(ghost.transform.position) != null;
        foreach (var foot in feet)
        {
            if (foot == null)
                continue;
            var tile = TileUnder(foot.transform.position);
            if (tile == null || TileTaken(tile))
                return false;
        }

        return true;
    }

    private static void ClaimFootprint(GameObject placed)
    {
        var item = placed.GetComponent<GridItem>() ?? placed.GetComponentInChildren<GridItem>(true);
        if (item == null)
            return;
        foreach (var foot in placed.GetComponentsInChildren<FootprintTile>(true))
        {
            var tile = TileUnder(foot.transform.position);
            if (tile != null && IsOurGrid(tile.OwnerGrid))
                tile.AddOccupant(item, foot);
        }
    }

    private static void ReleaseFootprint(GameObject placed)
    {
        var item = placed.GetComponent<GridItem>() ?? placed.GetComponentInChildren<GridItem>(true);
        if (item == null)
            return;
        foreach (var foot in placed.GetComponentsInChildren<FootprintTile>(true))
        {
            var tile = TileUnder(foot.transform.position);
            tile?.RemoveOccupant(item, foot);
        }
    }

    private static bool TileTaken(Tile tile)
    {
        var occupants = tile.BuildableOccupants;
        return occupants != null && occupants.Count > 0;
    }

    private static Tile? TileUnder(Vector3 world)
    {
        if (_gridHost == null)
            return null;
        Tile? best = null;
        float bestDistance = Grid.TileSize * 0.55f;
        foreach (var tile in _gridHost.GetComponentsInChildren<Tile>())
        {
            if (tile == null)
                continue;
            var position = tile.transform.position;
            float distance = Vector2.Distance(new Vector2(position.x, position.z), new Vector2(world.x, world.z));
            if (distance >= bestDistance)
                continue;
            bestDistance = distance;
            best = tile;
        }

        return best;
    }

    private static GameObject? PlacedRoot(Transform? node)
    {
        while (node != null)
        {
            if (node.name.StartsWith(PlacedPrefix, StringComparison.Ordinal) || RecordIds.ContainsKey(node.gameObject.GetInstanceID()))
                return node.gameObject;
            node = node.parent;
        }

        return null;
    }

    private static string ItemIdOf(GameObject placed)
    {
        if (RecordIds.TryGetValue(placed.GetInstanceID(), out var id) && Live.TryGetValue(id, out _))
        {
            var record = _save?.Placements.Find(item => item != null && item.Id == id);
            if (!string.IsNullOrWhiteSpace(record?.ItemId))
                return record!.ItemId;
        }

        return placed.name.StartsWith(PlacedPrefix, StringComparison.Ordinal)
            ? placed.name.Substring(PlacedPrefix.Length)
            : "";
    }

    private static bool IsOurs(Il2CppScheduleOne.Property.Property? property) =>
        property != null && _property != null && property.Pointer == _property.Pointer;

    private static bool IsOurGrid(Grid? grid) =>
        grid != null && _grid != null && grid.Pointer == _grid.Pointer;

    private static void DetachLists()
    {
        if (_property == null)
            return;
        RemovePointer(Il2CppScheduleOne.Property.Property.Properties, _property);
        RemovePointer(Il2CppScheduleOne.Property.Property.OwnedProperties, _property);
        RemovePointer(Il2CppScheduleOne.Property.Property.UnownedProperties, _property);
    }

    private static void AddOnce<T>(il2cpp::Il2CppSystem.Collections.Generic.List<T>? list, T? item)
        where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        if (list == null || item == null)
            return;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null && list[i].Pointer == item.Pointer)
                return;
        }

        list.Add(item);
    }

    private static void RemovePointer<T>(il2cpp::Il2CppSystem.Collections.Generic.List<T>? list, T item)
        where T : Il2CppInterop.Runtime.InteropTypes.Il2CppObjectBase
    {
        if (list == null)
            return;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] != null && list[i].Pointer == item.Pointer)
                list.RemoveAt(i);
        }
    }

    private static void DestroyIfAlive(GameObject? go)
    {
        if (go != null)
            UnityEngine.Object.Destroy(go);
    }

    private enum WalletOutcome { Applied, NotApplied, Unknown }

    private static bool TryReadWallet(out float bank, out float cash)
    {
        bank = cash = 0f;
        try
        {
            var manager = MoneyManager.Instance;
            if (!NetworkGuard.IsAlive(manager))
                return false;
            bank = manager!.onlineBalance;
            cash = manager.cashBalance;
            return float.IsFinite(bank) && float.IsFinite(cash);
        }
        catch
        {
            return false;
        }
    }

    private static WalletOutcome ChangeBalance(float delta, bool bank)
    {
        if (!float.IsFinite(delta) || delta == 0f || !NetworkGuard.IsHostOrSingleplayer())
            return WalletOutcome.NotApplied;
        MoneyManager? manager;
        float expected;
        try
        {
            manager = MoneyManager.Instance;
            if (!NetworkGuard.IsAlive(manager))
                return WalletOutcome.NotApplied;
            float before = bank ? manager!.onlineBalance : manager!.cashBalance;
            expected = before + delta;
            if (!float.IsFinite(expected) || (delta < 0f && expected < -0.001f))
                return WalletOutcome.NotApplied;
            if (bank)
                manager.RpcLogic___ReceiveOnlineTransaction_1419830531(HideoutPropertyPlan.Name, delta, 1f, HideoutPropertyPlan.Code);
            else
                manager.ChangeCashBalance(delta, true, true);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Zahlung: " + ex.Message);
            return WalletOutcome.Unknown;
        }

        try
        {
            var current = MoneyManager.Instance;
            if (current != null && current.Pointer == manager.Pointer)
            {
                float now = bank ? current.onlineBalance : current.cashBalance;
                if (Math.Abs(now - expected) < 0.01f)
                    return WalletOutcome.Applied;
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[NorthIsland] Hideout-Kontostand: " + ex.Message);
        }

        return WalletOutcome.Unknown;
    }
}
