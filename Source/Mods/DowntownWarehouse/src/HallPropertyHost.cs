extern alias il2cpp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using HarmonyLib;
using Il2Cpp;
using Il2CppScheduleOne;
using Il2CppFishNet;
using Il2CppFishNet.Object;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.Building;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Dialogue;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.Interaction;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Money;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.Persistence.Datas;
using Il2CppScheduleOne.Persistence.Loaders;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Storage;
using Il2CppScheduleOne.Tiles;
using MelonLoader;
using S1API.Lifecycle;
using S1API.Money;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.Events;

namespace DowntownWarehouse;

/// <summary>
/// Publishes the native hall behind Hank's Hardware as a singleplayer property with
/// a build grid, Ray purchase, working blue door, interior shell and slot-scoped
/// placement persistence in the Modded folder.
/// Pattern follows NorthIsland.HideoutPropertyHost; no third-party code is copied.
/// </summary>
internal static class HallPropertyHost
{
    private const string HarmonyId = "DowntownWarehouse.HallProperty";
    private const string PlacedPrefix = "DowntownWarehouse-placed:";
    private const string GridName = "DowntownWarehouse_Grid";

    private static HarmonyLib.Harmony? _harmony;
    private static bool _saveHooked;
    private static GameObject? _propertyRoot;
    private static GameObject? _gridHost;
    private static GameObject? _placedRoot;
    private static Il2CppScheduleOne.Property.Property? _property;
    private static Grid? _grid;
    private static readonly Dictionary<(int X, int Z), Tile> TileGrid = new();
    private static float _gridOriginX;
    private static float _gridOriginZ;
    private static bool _rayChoiceAdded;
    private static bool _restoring;
    private static int _suppressNativeSaveable;
    private static HallSaveFile? _save;
    private static HallSaveKind _saveKind = HallSaveKind.Missing;
    private static string _saveFolder = "";
    private static string _savePath = "";
    private static bool _owned;
    private static bool _purchaseReady;
    private static bool _purchaseBusy;
    private static int _tileCount;
    private static string _priceOpenTag = "<color=#4DA6FF>";
    private static readonly Regex PriceColorRx = new(@"<color=(#[0-9A-Fa-f]{6,8})>", RegexOptions.Compiled);
    private static readonly Dictionary<string, GameObject> Live = new(StringComparer.Ordinal);
    private static readonly Dictionary<int, string> RecordIds = new();
    private static readonly HashSet<string> FailedContents = new(StringComparer.Ordinal);
    private static readonly HashSet<string> Spawned = new(StringComparer.Ordinal);

    internal static string Status =>
        _property == null
            ? "Halle nicht registriert"
            : $"Halle {(_owned ? "gekauft" : "verkauft sich fuer " + HallPlan.PriceLabel)}, Kacheln={_tileCount}, Save={_saveKind}";

    internal static void Publish()
    {
        Shutdown();
        InstallPatches();
        HookSave();

        var hall = GameObject.Find(HallPlan.HallMainPath);
        if (hall == null)
        {
            MelonLogger.Warning("[DowntownWarehouse] Native Halle nicht gefunden. Property bleibt aus.");
            return;
        }

        if (!TryBindSave())
            MelonLogger.Warning("[DowntownWarehouse] Kauf gesperrt, bis der Spielstand eindeutig gelesen werden kann.");

        try
        {
            if (CodeAlreadyRegistered())
                throw new InvalidOperationException("property-code-collision");

            _placedRoot = new GameObject("DowntownWarehouse_Placed");
            _placedRoot.transform.SetParent(hall.transform.parent, false);
            BuildProperty(hall.transform, _placedRoot.transform);
            _propertyRoot!.SetActive(true);
            BuildGrid(hall.transform);
            HallDoor.Build();
            HallSkin.Prepare(hall);
            HallInterior.Prepare(hall, _property);
            HallSkin.Apply();
            HallInterior.Apply();
            RegisterLists();
            ApplyOwnership(_owned);
            if (_owned)
                RestorePlacements();
            MelonLogger.Msg("[DowntownWarehouse] " + Status + ".");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[DowntownWarehouse] Property verworfen: " + ex.Message);
            Shutdown();
        }
    }

    internal static void Shutdown()
    {
        RunCleanup(HallDoor.Reset);
        RunCleanup(HallInterior.Reset);
        RunCleanup(HallSkin.Reset);
        RunCleanup(DeregisterOwnedGuids);
        RunCleanup(DetachLists);
        Live.Clear();
        RecordIds.Clear();
        FailedContents.Clear();
        Spawned.Clear();
        _restoring = false;
        _suppressNativeSaveable = 0;
        _save = null;
        _saveKind = HallSaveKind.Missing;
        _saveFolder = "";
        _savePath = "";
        _owned = false;
        _purchaseReady = false;
        _purchaseBusy = false;
        _tileCount = 0;
        _property = null;
        _grid = null;
        TileGrid.Clear();
        _gridOriginX = 0f;
        _gridOriginZ = 0f;
        _rayChoiceAdded = false;
        DestroyIfAlive(_placedRoot);
        DestroyIfAlive(_gridHost);
        DestroyIfAlive(_propertyRoot);
        _placedRoot = null;
        _gridHost = null;
        _propertyRoot = null;
    }

    private static void RunCleanup(Action action)
    {
        try { action(); }
        catch (Exception ex) { MelonLogger.Warning("[DowntownWarehouse] Cleanup: " + ex.Message); }
    }

    internal static void DisposePatches()
    {
        if (_saveHooked)
        {
            GameLifecycle.OnSaveStart -= FlushSave;
            _saveHooked = false;
        }

        try { _harmony?.UnpatchSelf(); }
        catch (Exception ex) { MelonLogger.Warning("[DowntownWarehouse] Patches: " + ex.Message); }
        _harmony = null;
    }

    private static void InstallPatches()
    {
        if (_harmony != null)
            return;

        _harmony = new HarmonyLib.Harmony(HarmonyId);
        Patch(typeof(Il2CppScheduleOne.Property.Property), nameof(Il2CppScheduleOne.Property.Property.Awake), nameof(SkipNativeAwake), parameterTypes: Type.EmptyTypes);
        Patch(typeof(Il2CppScheduleOne.Property.Property), nameof(Il2CppScheduleOne.Property.Property.ShouldSave), nameof(SkipNativeSave));
        Patch(typeof(Il2CppScheduleOne.Property.Property), nameof(Il2CppScheduleOne.Property.Property.Load), nameof(SkipNativeLoad));
        Patch(typeof(Grid), nameof(Grid.IsTileValidAtCoordinate), nameof(ValidateHallTile));
        PatchPost(typeof(BuildUpdate_Grid), nameof(BuildUpdate_Grid.CheckIntersections), nameof(MarkHallGhost), finalizer: nameof(KeepHallGhostAlive));
        Patch(typeof(BuildUpdate_Grid), nameof(BuildUpdate_Grid.Place), nameof(PlaceOnHall));
        Patch(typeof(BuildUpdate_ProceduralGrid), nameof(BuildUpdate_ProceduralGrid.Place), nameof(GuardHallProceduralPlace));
        Patch(typeof(BuildUpdate_Surface), nameof(BuildUpdate_Surface.Place), nameof(GuardHallSurfacePlace));
        PatchFull(typeof(BuildManager), nameof(BuildManager.CreateProceduralGridItem), nameof(PreCreateProceduralGridItem), nameof(PostCreateProceduralGridItem), nameof(EndSuppressSaveable));
        PatchFull(typeof(BuildManager), nameof(BuildManager.CreateSurfaceItem), nameof(PreCreateSurfaceItem), nameof(PostCreateSurfaceItem), nameof(EndSuppressSaveable));
        PatchPost(typeof(BuildableItem), nameof(BuildableItem.GetProperty), nameof(ResolveHallProperty), parameterTypes: new[] { typeof(Transform) });
        Patch(typeof(BuildableItem), nameof(BuildableItem.InitializeSaveable), nameof(SkipPlacedSaveable));
        Patch(typeof(BuildableItem), nameof(BuildableItem.CanBePickedUp), nameof(AllowHallPickup));
        Patch(typeof(BuildableItem), nameof(BuildableItem.PickupItem), nameof(PickupHallItem));
        PatchPost(typeof(InteractableObject), nameof(InteractableObject.Hovered), nameof(OfferHallPickup));
        Patch(typeof(InteractableObject), nameof(InteractableObject.ShowMessage), nameof(OfferHallPickup));
        Patch(typeof(InteractableObject), nameof(InteractableObject.StartInteract), nameof(StartPrompt));
        PatchPost(typeof(ControlledDialogueHandler), "ModifyChoiceList", nameof(OfferAtRay));
        PatchPost(typeof(DialogueHandler_EstateAgent), nameof(DialogueHandler_EstateAgent.ModifyChoiceText), nameof(PriceAtRay));
        Patch(typeof(DialogueHandler_EstateAgent), nameof(DialogueHandler_EstateAgent.ShouldChoiceBeShown), nameof(ShowAtRay));
        Patch(typeof(DialogueHandler_EstateAgent), nameof(DialogueHandler_EstateAgent.CheckChoice), nameof(AllowAtRay));
        Patch(typeof(DialogueHandler_EstateAgent), nameof(DialogueHandler_EstateAgent.ChoiceCallback), nameof(BuyAtRay));
    }

    private static void Patch(Type type, string name, string prefix, string? finalizer = null, Type[]? parameterTypes = null)
    {
        var method = parameterTypes == null
            ? AccessTools.Method(type, name)
            : AccessTools.Method(type, name, parameterTypes);
        PatchGuard.TryPatch(
            _harmony!,
            method,
            prefix: new HarmonyMethod(typeof(HallPropertyHost), prefix),
            finalizer: finalizer == null ? null : new HarmonyMethod(typeof(HallPropertyHost), finalizer));
    }

    private static void PatchPost(Type type, string name, string postfix, string? finalizer = null, Type[]? parameterTypes = null)
    {
        var method = parameterTypes == null
            ? AccessTools.Method(type, name)
            : AccessTools.Method(type, name, parameterTypes);
        PatchGuard.TryPatch(
            _harmony!,
            method,
            postfix: new HarmonyMethod(typeof(HallPropertyHost), postfix),
            finalizer: finalizer == null ? null : new HarmonyMethod(typeof(HallPropertyHost), finalizer));
    }

    private static void PatchFull(Type type, string name, string prefix, string postfix, string finalizer)
    {
        var method = AccessTools.Method(type, name);
        PatchGuard.TryPatch(
            _harmony!,
            method,
            prefix: new HarmonyMethod(typeof(HallPropertyHost), prefix),
            postfix: new HarmonyMethod(typeof(HallPropertyHost), postfix),
            finalizer: new HarmonyMethod(typeof(HallPropertyHost), finalizer));
    }

    private static void HookSave()
    {
        if (_saveHooked)
            return;
        GameLifecycle.OnSaveStart += FlushSave;
        _saveHooked = true;
    }

    private static bool CodeAlreadyRegistered()
    {
        var list = Il2CppScheduleOne.Property.Property.Properties;
        if (list == null)
            return false;
        for (int i = 0; i < list.Count && i < 512; i++)
        {
            var property = list[i];
            if (NetworkGuard.IsAlive(property) && property!.propertyCode == HallPlan.Code)
                return true;
        }

        return false;
    }

    private static bool TryBindSave()
    {
        _purchaseReady = false;
        _owned = false;
        _save = null;
        _saveKind = HallSaveKind.Missing;
        _saveFolder = "";
        _savePath = "";

        var load = PersistentSingleton<LoadManager>.Instance;
        string folder = load != null ? load.LoadedGameFolderPath ?? "" : "";
        if (string.IsNullOrWhiteSpace(folder) || folder.IndexOf("..", StringComparison.Ordinal) >= 0)
        {
            _saveKind = HallSaveKind.Corrupt;
            return false;
        }

        _saveFolder = folder;
        _savePath = Path.Combine(folder, "Modded", "DowntownWarehouse", "hall.json");
        string fullFolder = Path.GetFullPath(folder).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;
        if (!Path.GetFullPath(_savePath).StartsWith(fullFolder, StringComparison.OrdinalIgnoreCase))
        {
            _saveKind = HallSaveKind.Corrupt;
            _savePath = "";
            return false;
        }

        bool exists = File.Exists(_savePath) || File.Exists(_savePath + ".bak");
        HallSaveFile? loaded = exists
            ? SafeStorage.LoadSafe<HallSaveFile>(_savePath, new HallSaveFile { Schema = 0, PropertyCode = "" })
            : null;
        if (exists && loaded != null && loaded.Schema == 0 && string.IsNullOrEmpty(loaded.PropertyCode))
            loaded = null;

        _saveKind = HallSaveRules.Classify(loaded, folder, exists);
        if (_saveKind == HallSaveKind.Missing)
        {
            _save = new HallSaveFile { SaveFolder = folder };
            _purchaseReady = true;
            return true;
        }

        if (_saveKind != HallSaveKind.Ready || loaded == null)
            return false;

        _save = loaded;
        _owned = loaded.Owned;
        _purchaseReady = true;
        return true;
    }

    private static void BuildProperty(Transform hall, Transform activeHost)
    {
        var root = new GameObject("DowntownWarehouse_Property");
        root.transform.SetParent(hall.parent, false);
        root.transform.position = new Vector3(HallPlan.CenterX, HallPlan.FloorTopY, HallPlan.CenterZ);
        root.SetActive(false);
        _propertyRoot = root;

        var network = root.AddComponent<NetworkObject>();
        var property = root.AddComponent<Il2CppScheduleOne.Property.Property>();
        network._networkBehaviours = new Il2CppReferenceArray<NetworkBehaviour>(new NetworkBehaviour[] { property });
        property._networkObjectCache = network;
        property._componentIndexCache = 0;
        property.propertyCode = HallPlan.Code;
        property.propertyName = HallPlan.Name;
        property.OwnedByDefault = false;
        property._IsOwned_k__BackingField = false;
        property.IsOwnedVariable = "";
        property.Price = HallPlan.Price;
        property.EmployeeCapacity = HallPlan.EmployeeCapacity;
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
        property.SpawnPoint = Point(root.transform, "Spawn", new Vector3(HallPlan.SpawnX, HallPlan.FloorTopY + 1.1f, HallPlan.SpawnZ));
        property.InteriorSpawnPoint = Point(root.transform, "InteriorSpawn", new Vector3(HallPlan.InteriorSpawnX, HallPlan.FloorTopY + 0.2f, HallPlan.InteriorSpawnZ));
        property.NPCSpawnPoint = Point(root.transform, "NPCSpawn", new Vector3(HallPlan.NpcSpawnX, HallPlan.FloorTopY + 1.1f, HallPlan.NpcSpawnZ));
        var idle = Point(root.transform, "Idle", new Vector3(HallPlan.CenterX - 2f, HallPlan.FloorTopY + 0.05f, HallPlan.CenterZ));
        property.EmployeeIdlePoints = new Il2CppReferenceArray<Transform>(new[] { idle });

        var boundsObject = new GameObject("Bounds");
        boundsObject.transform.SetParent(activeHost, false);
        boundsObject.transform.position = new Vector3(HallPlan.CenterX, HallPlan.FloorTopY, HallPlan.CenterZ);
        var bounds = boundsObject.AddComponent<BoxCollider>();
        bounds.isTrigger = true;
        bounds.center = new Vector3(0f, HallPlan.HeightY * 0.5f, 0f);
        bounds.size = new Vector3(HallPlan.WidthX + 1.2f, HallPlan.HeightY + 1.2f, HallPlan.LengthZ + 1.2f);
        property.BoundingBox = boundsObject;
        property.propertyBoundsColliders = new Il2CppReferenceArray<BoxCollider>(new[] { bounds });

        var container = new GameObject("Contents").AddComponent<Il2CppScheduleOne.Property.PropertyContentsContainer>();
        container.transform.SetParent(activeHost, false);
        container._Property_k__BackingField = property;
        property._Container_k__BackingField = container;
        var ui = new GameObject("WorldspaceUI").AddComponent<RectTransform>();
        ui.SetParent(activeHost, false);
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

    private static void BuildGrid(Transform hall)
    {
        float size = Grid.TileSize;
        var tiles = HallPlan.Tiles(size);
        if (tiles.Count == 0)
            throw new InvalidOperationException("hall-grid-empty");

        var donor = FindDonor();
        if (donor == null)
            throw new InvalidOperationException("hall-donor-missing");

        var (originX, originZ) = HallPlan.GridOrigin(tiles);
        _gridOriginX = originX;
        _gridOriginZ = originZ;
        TileGrid.Clear();

        var host = new GameObject(GridName);
        host.transform.SetParent(hall.parent, false);
        host.transform.position = new Vector3(originX, HallPlan.FloorTopY, originZ);
        host.SetActive(false);
        _gridHost = host;
        var gridOwner = host.AddComponent<Il2CppScheduleOne.Property.PropertyContentsContainer>();
        gridOwner._Property_k__BackingField = _property;
        var grid = host.AddComponent<Grid>();
        grid.Tiles = new il2cpp::Il2CppSystem.Collections.Generic.List<Tile>();
        grid._parentProperty = _property;
        grid._guid = HallPlan.GridGuidString;
        grid._GUID_k__BackingField = new il2cpp::Il2CppSystem.Guid(HallPlan.GridGuidString);

        float offset = donor.GetComponent<Tile>().AvailableOffset;
        var template = UnityEngine.Object.Instantiate(donor, host.transform);
        template.SetActive(false);
        template.name = GridName + "-template";
        foreach (var tile in tiles)
        {
            var clone = UnityEngine.Object.Instantiate(template, host.transform);
            clone.name = GridName + "-" + tile.X + "-" + tile.Z;
            clone.transform.position = new Vector3(tile.WorldX, HallPlan.FloorTopY + 0.02f, tile.WorldZ);
            FitCollider(clone, size);
            var piece = clone.GetComponent<Tile>();
            piece.OwnerGrid = grid;
            piece.x = tile.X;
            piece.y = tile.Z;
            piece.AvailableOffset = Math.Max(offset, 1f);
            piece.BuildableOccupants = new il2cpp::Il2CppSystem.Collections.Generic.List<GridItem>();
            piece.OccupantTiles = new il2cpp::Il2CppSystem.Collections.Generic.List<FootprintTile>();
            var exposureNode = clone.GetComponent<Il2CppScheduleOne.Lighting.LightExposureNode>()
                ?? clone.AddComponent<Il2CppScheduleOne.Lighting.LightExposureNode>();
            exposureNode.ambientExposure = 0f;
            exposureNode.sources = new il2cpp::Il2CppSystem.Collections.Generic.Dictionary<Il2CppScheduleOne.Lighting.UsableLightSource, float>();
            piece.LightExposureNode = exposureNode;
            clone.SetActive(true);
            grid.RegisterTile(piece);
            TileGrid[(tile.X, tile.Z)] = piece;
        }

        UnityEngine.Object.Destroy(template);
        host.SetActive(true);
        if (grid._coordinateToTile == null || grid._coordinateToTile.Count == 0)
            grid.ProcessCoordinateDataPairs();
        grid.SetGridSize();
        var resolvedGuid = new il2cpp::Il2CppSystem.Guid(HallPlan.GridGuidString);
        if (GUIDManager.GetObject<Grid>(resolvedGuid) == null)
            grid.SetGUID(resolvedGuid);
        AddOnce(_property?.Grids, grid);
        _grid = grid;
        _tileCount = tiles.Count;
    }

    private static GameObject? FindDonor()
    {
        GameObject? fallback = null;
        foreach (var tile in UnityEngine.Object.FindObjectsOfType<Tile>())
        {
            if (tile == null || tile.OwnerGrid == null)
                continue;
            var go = tile.gameObject;
            if (go.name.StartsWith("DowntownWarehouse") || go.name.StartsWith("Northshore_Hideout") || go.name.StartsWith("TownHallInterior"))
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

    private static void RegisterLists()
    {
        var property = _property ?? throw new InvalidOperationException("hall-property-missing");
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
        HallDoor.Apply(owned);
        HallInterior.SetSaveVisible(owned);
    }

    public static void OfferAtRay(
        ControlledDialogueHandler __instance,
        string dialogueLabel,
        ref il2cpp::Il2CppSystem.Collections.Generic.List<DialogueChoiceData> existingChoices)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero)
                return;
            if (existingChoices == null || existingChoices.Pointer == IntPtr.Zero || existingChoices.Count == 0 || _owned || !_purchaseReady)
                return;
            if (!__instance.GetIl2CppType().Name.Contains("EstateAgent"))
                return;

            bool propertyMenu = false;
            for (int i = 0; i < existingChoices.Count; i++)
            {
                var choice = existingChoices[i];
                if (choice == null)
                    continue;
                if (string.Equals(choice.ChoiceLabel, HallPlan.ChoiceLabel, StringComparison.Ordinal))
                    return;
                if (HallListing.IsPropertyMenu(choice.ChoiceLabel, choice.ChoiceText))
                    propertyMenu = true;
            }

            if (!propertyMenu)
                return;

            var ours = new DialogueChoiceData
            {
                Guid = Guid.NewGuid().ToString(),
                ChoiceLabel = HallPlan.ChoiceLabel,
                ChoiceText = HallPlan.Name
            };
            existingChoices.Insert(Math.Max(0, existingChoices.Count - 1), ours);
            if (!_rayChoiceAdded)
            {
                _rayChoiceAdded = true;
                MelonLogger.Msg("[DowntownWarehouse] Halle steht bei Ray in der Immobilienliste.");
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[DowntownWarehouse] Ray-Liste: " + ex.Message);
        }
    }

    public static void PriceAtRay(string choiceLabel, string choiceText, ref string __result)
    {
        if (string.Equals(choiceLabel, HallPlan.ChoiceLabel, StringComparison.Ordinal))
        {
            __result = HallPlan.Name + " (" + _priceOpenTag + HallPlan.PriceLabel + "</color>)";
            return;
        }

        // Learn the vanilla price colour from any other property choice in this dialogue.
        if (!string.IsNullOrEmpty(__result))
        {
            var match = PriceColorRx.Match(__result);
            if (match.Success)
                _priceOpenTag = "<color=" + match.Groups[1].Value + ">";
        }
    }

    public static bool ShowAtRay(string choiceLabel, ref bool __result)
    {
        if (!string.Equals(choiceLabel, HallPlan.ChoiceLabel, StringComparison.Ordinal))
            return true;
        __result = _purchaseReady && !_owned;
        return false;
    }

    public static bool AllowAtRay(string choiceLabel, ref string invalidReason, ref bool __result)
    {
        if (!string.Equals(choiceLabel, HallPlan.ChoiceLabel, StringComparison.Ordinal))
            return true;
        invalidReason = string.Empty;
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

        var decision = HallPurchase.Evaluate(true, false, bank, cash);
        __result = decision.Status == HallPurchaseStatus.Purchased;
        if (!__result)
            invalidReason = "INSUFFICIENT BALANCE";
        return false;
    }

    public static bool BuyAtRay(DialogueHandler_EstateAgent __instance, string choiceLabel)
    {
        if (!string.Equals(choiceLabel, HallPlan.ChoiceLabel, StringComparison.Ordinal))
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
            MelonLogger.Msg("[DowntownWarehouse] Kauf gibt es nur im Singleplayer.");
            return;
        }

        if (!TryReadWallet(out float bank, out float cash))
        {
            MelonLogger.Warning("[DowntownWarehouse] Kauf abgebrochen, Kontostand nicht lesbar.");
            return;
        }

        var decision = HallPurchase.Evaluate(_purchaseReady && _saveKind is HallSaveKind.Missing or HallSaveKind.Ready, _owned, bank, cash);
        if (decision.Status != HallPurchaseStatus.Purchased)
        {
            MelonLogger.Msg("[DowntownWarehouse] Kauf: " + decision.Status);
            return;
        }

        _purchaseBusy = true;
        try
        {
            var charge = ChangeBalance(-decision.Amount, decision.ChargeBank);
            if (charge != WalletOutcome.Applied)
            {
                MelonLogger.Warning(charge == WalletOutcome.Unknown
                    ? "[DowntownWarehouse] Zahlungsstatus unklar; keine automatische Rueckbuchung und kein Besitzwechsel."
                    : "[DowntownWarehouse] Zahlung nicht angewendet. Besitz unveraendert.");
                return;
            }

            // Ownership persists at the native OnSaveStart checkpoint (FlushSave) so
            // quitting without saving keeps money and hall ownership in sync.
            ApplyOwnership(true);
            if (_save != null)
            {
                _save.Owned = true;
                _save.SaveFolder = _saveFolder;
                _save.PropertyCode = HallPlan.Code;
                _save.Schema = HallPlan.Schema;
            }

            MelonLogger.Msg("[DowntownWarehouse] Downtown Warehouse gekauft (wird beim naechsten Spielstand-Save gesichert).");
            Notify("Halle erworben!", $"Erfolgreich für {HallPlan.PriceLabel} gekauft. Die Halle hinter Hank's Hardware gehört dir.", true);
        }
        finally
        {
            _purchaseBusy = false;
        }
    }

    private static void Notify(string title, string message, bool positive)
    {
        try
        {
            var mgr = Il2CppScheduleOne.UI.NotificationsManager.Instance;
            if (mgr != null)
                mgr.SendNotification(title, message, null, 6f, positive);
        }
        catch { /* notification is optional feedback */ }
    }

    public static bool SkipNativeAwake(Il2CppScheduleOne.Property.Property __instance)
    {
        if (!IsOurs(__instance))
            return true;
        __instance.NetworkInitialize___Early();
        __instance.NetworkInitialize__Late();
        return false;
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

    public static bool ValidateHallTile(Grid __instance, Coordinate gridCoord, FootprintTile tile, ref bool __result)
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

    public static void MarkHallGhost(BuildUpdate_Grid __instance)
    {
        try
        {
            if (__instance == null || !IsHallGhost(__instance.GhostModel))
                return;
            bool clear = _owned && FootprintClear(__instance.GhostModel!);
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
            MelonLogger.Warning("[DowntownWarehouse] Ghost: " + ex.Message);
        }
    }

    public static Exception? KeepHallGhostAlive(Exception? __exception, BuildUpdate_Grid __instance)
    {
        if (__exception == null)
            return null;
        if (__exception.ToString().IndexOf("NullReferenceException", StringComparison.Ordinal) < 0)
            return __exception;
        if (!IsHallGhost(__instance?.GhostModel))
            return __exception;
        MarkHallGhost(__instance!);
        return null;
    }

    public static bool PlaceOnHall(BuildUpdate_Grid __instance, ref GridItem? __result)
    {
        if (!IsHallGhost(__instance?.GhostModel))
            return true;
        __result = null;
        if (!NetworkGuard.IsHostOrSingleplayer() || !_owned || _save == null || !_purchaseReady)
        {
            MelonLogger.Warning("[DowntownWarehouse] Platzierung abgelehnt.");
            return false;
        }

        GameObject? placed = null;
        BuildableItem? placedBuilt = null;
        PlayerInventory? inventory = null;
        ItemInstance? refund = null;
        bool consumed = false;
        try
        {
            if (!FootprintClear(__instance!.GhostModel!))
                return false;
            var item = __instance.ItemInstance;
            var definition = item?.Definition?.TryCast<BuildableItemDefinition>();
            if (definition?.BuiltItem == null || item?.Definition == null)
                return false;

            string itemId = item.Definition.ID;
            inventory = PlayerInventory.Instance;
            if (!HasItemInInventory(inventory, itemId))
            {
                MelonLogger.Warning("[DowntownWarehouse] Platzierung abgelehnt, Item nicht mehr im Inventar: " + itemId);
                return false;
            }

            var position = __instance.GhostModel.transform.position;
            var rotation = __instance.GhostModel.transform.rotation;
            var record = BuildPlacementRecord(itemId, position, rotation);
            if (!record.IsPlausible())
            {
                MelonLogger.Warning("[DowntownWarehouse] Platzierung abgelehnt, Position ausserhalb der Halle.");
                return false;
            }

            refund = item.GetCopy(1) ?? definition.GetDefaultInstance(1);
            var itemCopy = item.GetCopy(1) ?? definition.GetDefaultInstance(1);
            int rotDeg = ((int)MathF.Round(rotation.eulerAngles.y / 90f) * 90 % 360 + 360) % 360;
            var originTile = FindOriginTile(__instance, definition, position, rotation);

            GridItem? nativeGridItem = null;
            var buildManager = NetworkSingleton<BuildManager>.Instance;
            if (originTile != null && _grid != null && itemCopy != null && NetworkGuard.IsAlive(buildManager))
            {
                _suppressNativeSaveable++;
                try
                {
                    nativeGridItem = buildManager!.CreateGridItem(
                        itemCopy,
                        _grid,
                        new Vector2(originTile.x, originTile.y),
                        rotDeg,
                        record.Id,
                        null);
                }
                catch (Exception ex)
                {
                    MelonLogger.Warning("[DowntownWarehouse] CreateGridItem Fallback auf SpawnBuilt: " + ex.Message);
                    nativeGridItem = null;
                }
                finally
                {
                    if (_suppressNativeSaveable > 0)
                        _suppressNativeSaveable--;
                }
            }

            if (NetworkGuard.IsAlive(nativeGridItem))
            {
                if (_property != null && nativeGridItem!.ParentProperty == null)
                    nativeGridItem._ParentProperty_k__BackingField = _property;
                if (_property?.Container != null && nativeGridItem!.transform.parent != _property.Container.transform)
                    nativeGridItem.transform.SetParent(_property.Container.transform, true);
                AddOnce(_property?.BuildableItems, nativeGridItem!.Cast<BuildableItem>());
                placed = nativeGridItem!.gameObject;
                placedBuilt = nativeGridItem.Cast<BuildableItem>();
                __result = nativeGridItem;
            }
            else
            {
                placed = SpawnBuilt(definition, position, rotation, itemId, item, record.Id, __instance);
                if (placed == null)
                    return false;
                placedBuilt = placed.GetComponent<BuildableItem>() ?? placed.GetComponentInChildren<BuildableItem>(true);
                __result = placedBuilt?.TryCast<GridItem>();
            }

            if (!ConsumePlacedItem(inventory, itemId, out int remainingInEquipped))
            {
                ReleasePlacementFootprints(placed);
                DeregisterBuildableGuid(placedBuilt);
                DetachPlacedFromProperty(placed);
                UnityEngine.Object.Destroy(placed);
                placed = null;
                __result = null;
                MelonLogger.Warning("[DowntownWarehouse] Platzierung abgelehnt, Item konnte nicht verbraucht werden: " + itemId);
                return false;
            }

            consumed = true;
            record.X = placed.transform.position.x;
            record.Y = placed.transform.position.y;
            record.Z = placed.transform.position.z;
            record.RotX = placed.transform.rotation.x;
            record.RotY = placed.transform.rotation.y;
            record.RotZ = placed.transform.rotation.z;
            record.RotW = placed.transform.rotation.w;
            if (placedBuilt != null)
                CaptureNativeState(placedBuilt, record);
            RegisterRecord(placed, record);
            if (remainingInEquipped <= 0)
                __instance.Stop();
            MelonLogger.Msg("[DowntownWarehouse] In der Halle platziert: " + itemId);
            return false;
        }
        catch (Exception ex)
        {
            if (placed != null)
            {
                Forget(placed);
                ReleasePlacementFootprints(placed);
                DeregisterBuildableGuid(placedBuilt);
                DetachPlacedFromProperty(placed);
                UnityEngine.Object.Destroy(placed);
            }
            if (consumed && refund != null && inventory != null)
            {
                try { inventory.AddItemToInventory(refund); }
                catch (Exception rx) { MelonLogger.Warning("[DowntownWarehouse] Rueckgabe nach Platzierungsfehler fehlgeschlagen: " + rx.Message); }
            }

            __result = null;
            MelonLogger.Warning("[DowntownWarehouse] Platzierung: " + ex.Message);
            return false;
        }
    }

    private static Tile? FindOriginTile(BuildUpdate_Grid buildUpdate, BuildableItemDefinition definition, Vector3 position, Quaternion rotation)
    {
        var hovered = buildUpdate.closestIntersection?.tile;
        if (hovered != null && IsOurGrid(hovered.OwnerGrid))
            return hovered;

        var template = definition.BuiltItem?.TryCast<GridItem>();
        if (template?.OriginFootprint != null)
        {
            Vector3 localOrigin = template.transform.InverseTransformPoint(template.OriginFootprint.transform.position);
            Vector3 worldOrigin = position + rotation * localOrigin;
            var byOrigin = TileUnder(worldOrigin);
            if (byOrigin != null)
                return byOrigin;
        }

        return TileUnder(position);
    }

    public static bool GuardHallProceduralPlace(BuildUpdate_ProceduralGrid __instance)
    {
        var tile = __instance?.bestIntersection?.procTile;
        var parent = tile?.ParentBuildableItem;
        if (parent == null || !IsOurs(parent.ParentProperty))
            return true;
        if (NetworkGuard.IsHostOrSingleplayer() && _owned && _save != null && _purchaseReady)
            return true;
        MelonLogger.Warning("[DowntownWarehouse] Prozedurale Platzierung abgelehnt.");
        return false;
    }

    public static bool GuardHallSurfacePlace(BuildUpdate_Surface __instance)
    {
        var surface = __instance?.hoveredValidSurface;
        if (surface == null || !IsOurs(surface.ParentProperty))
            return true;
        if (NetworkGuard.IsHostOrSingleplayer() && _owned && _save != null && _purchaseReady)
            return true;
        MelonLogger.Warning("[DowntownWarehouse] Wand-Platzierung abgelehnt.");
        return false;
    }

    public static void PreCreateProceduralGridItem(
        il2cpp::Il2CppSystem.Collections.Generic.List<CoordinateProceduralTilePair>? matches,
        ref bool __state)
    {
        __state = false;
        if (matches == null || !HasOurProceduralTarget(matches))
            return;
        _suppressNativeSaveable++;
        __state = true;
    }

    public static void PostCreateProceduralGridItem(ProceduralGridItem? __result, bool __state)
    {
        if (__result == null || _restoring)
            return;
        var built = __result.Cast<BuildableItem>();
        if (__state || IsHallOwnedBuildable(built))
            TrackNativePlaced(built);
    }

    public static void PreCreateSurfaceItem(Surface? parentSurface, ref bool __state)
    {
        __state = false;
        if (parentSurface == null || !IsOurs(parentSurface.ParentProperty))
            return;
        _suppressNativeSaveable++;
        __state = true;
    }

    public static void PostCreateSurfaceItem(SurfaceItem? __result, bool __state)
    {
        if (__result == null || _restoring)
            return;
        var built = __result.Cast<BuildableItem>();
        if (__state || IsHallOwnedBuildable(built))
            TrackNativePlaced(built);
    }

    public static Exception? EndSuppressSaveable(Exception? __exception, bool __state)
    {
        if (__state && _suppressNativeSaveable > 0)
            _suppressNativeSaveable--;
        return __exception;
    }

    private static bool HasOurProceduralTarget(il2cpp::Il2CppSystem.Collections.Generic.List<CoordinateProceduralTilePair> matches)
    {
        for (int i = 0; i < matches.Count; i++)
        {
            var pair = matches[i];
            if (pair == null)
                continue;
            var parent = pair.tile?.ParentBuildableItem;
            if (parent == null && pair.tileParent != null)
                parent = pair.tileParent.GetComponent<BuildableItem>() ?? pair.tileParent.GetComponentInChildren<BuildableItem>(true);
            if (parent != null && IsOurs(parent.ParentProperty))
                return true;
        }
        return false;
    }

    private static void TrackNativePlaced(BuildableItem built)
    {
        if (!NetworkGuard.IsAlive(built) || _save == null)
            return;
        var go = built.gameObject;
        if (RecordIds.ContainsKey(go.GetInstanceID()))
            return;

        if (_property != null && built.ParentProperty == null)
            built._ParentProperty_k__BackingField = _property;
        if (_property?.Container != null && go.transform.parent != _property.Container.transform)
            go.transform.SetParent(_property.Container.transform, true);
        if (built.TryCast<ProceduralGridItem>() is { } procPlaced)
        {
            try { procPlaced.RefreshTransform(); } catch { /* coroutine fallback */ }
        }
        AddOnce(_property?.BuildableItems, built);

        string guidStr = built.GUID.ToString();
        string recordId = Guid.TryParse(guidStr, out var parsed) && parsed != Guid.Empty
            ? parsed.ToString()
            : Guid.NewGuid().ToString();
        if (parsed == Guid.Empty)
            AssignBuiltGuid(built, recordId);
        if (Live.ContainsKey(recordId) || FailedContents.Contains(recordId))
            return;

        string itemId = built.ItemInstance?.Definition?.ID ?? "";
        if (string.IsNullOrWhiteSpace(itemId))
            return;

        var record = BuildPlacementRecord(itemId, go.transform.position, go.transform.rotation, recordId);
        CaptureNativeState(built, record);
        if (!record.IsPlausible())
        {
            MelonLogger.Warning("[DowntownWarehouse] Native Platzierung ausserhalb der Halle: " + itemId);
            return;
        }

        _save.Placements.Add(record);
        RecordIds[go.GetInstanceID()] = record.Id;
        Live[record.Id] = go;
        MelonLogger.Msg("[DowntownWarehouse] In der Halle platziert (nativ): " + itemId);
    }

    private static bool HasItemInInventory(PlayerInventory? inventory, string itemId)
    {
        if (inventory == null || string.IsNullOrWhiteSpace(itemId))
            return false;
        try
        {
            var equipped = inventory.equippedSlot;
            var held = equipped?.ItemInstance;
            if (held?.Definition != null
                && string.Equals(held.Definition.ID, itemId, StringComparison.Ordinal)
                && equipped!.Quantity >= 1)
                return true;
            return inventory.GetAmountOfItem(itemId) >= 1u;
        }
        catch
        {
            return false;
        }
    }

    private static bool ConsumePlacedItem(PlayerInventory? inventory, string itemId, out int remainingInEquipped)
    {
        remainingInEquipped = 0;
        if (inventory == null || string.IsNullOrWhiteSpace(itemId))
            return false;
        try
        {
            var equipped = inventory.equippedSlot;
            var held = equipped?.ItemInstance;
            if (held?.Definition != null
                && string.Equals(held.Definition.ID, itemId, StringComparison.Ordinal)
                && equipped!.Quantity >= 1)
            {
                int before = equipped.Quantity;
                equipped.ChangeQuantity(-1, false);
                remainingInEquipped = equipped.Quantity;
                if (remainingInEquipped < before)
                    return true;
            }
        }
        catch { /* fall back to RemoveAmountOfItem below */ }

        try
        {
            uint before = inventory.GetAmountOfItem(itemId);
            if (before < 1u)
                return false;
            inventory.RemoveAmountOfItem(itemId, 1u);
            uint after = inventory.GetAmountOfItem(itemId);
            return after < before;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsHallOwnedBuildable(BuildableItem? item)
    {
        if (_suppressNativeSaveable > 0)
            return true;
        if (item == null || item.Pointer == IntPtr.Zero)
            return false;
        if (IsOurs(item.ParentProperty))
            return true;
        if (item.TryCast<GridItem>() is { } gi && IsOurGrid(gi.OwnerGrid))
            return true;
        if (item.TryCast<SurfaceItem>() is { } si && si.ParentSurface != null && IsOurs(si.ParentSurface.ParentProperty))
            return true;
        if (PlacedRoot(item.transform) != null)
            return true;
        var container = _property?.Container;
        return container != null && item.transform.IsChildOf(container.transform);
    }

    public static void ResolveHallProperty(Transform? searchTransform, ref Il2CppScheduleOne.Property.Property? __result)
    {
        if (__result != null || _property == null || searchTransform == null)
            return;
        if ((_gridHost != null && searchTransform.IsChildOf(_gridHost.transform))
            || (_placedRoot != null && searchTransform.IsChildOf(_placedRoot.transform)))
        {
            __result = _property;
            return;
        }
        var grid = searchTransform.GetComponentInParent<Grid>();
        if (grid != null && IsOurGrid(grid))
        {
            __result = _property;
            return;
        }
        var surface = searchTransform.GetComponentInParent<Surface>();
        if (surface != null && IsOurs(surface.ParentProperty))
        {
            __result = _property;
            return;
        }
        var built = searchTransform.GetComponentInParent<BuildableItem>();
        if (built != null && IsHallOwnedBuildable(built))
            __result = _property;
    }

    public static bool SkipPlacedSaveable(BuildableItem __instance) =>
        __instance == null || !IsHallOwnedBuildable(__instance);

    private static bool ContentsUnresolved(GameObject placed) =>
        RecordIds.TryGetValue(placed.GetInstanceID(), out var id) && FailedContents.Contains(id);

    public static bool AllowHallPickup(BuildableItem __instance, ref string reason, ref bool __result)
    {
        var placed = __instance == null ? null : PlacedRoot(__instance.transform);
        if (placed == null)
            return true;
        if (ContentsUnresolved(placed))
        {
            reason = "Inhalt geschuetzt";
            __result = false;
            return false;
        }

        if (TryCheckNativePickupBlock(placed, __instance, out var blockReason))
        {
            reason = blockReason;
            __result = false;
            return false;
        }

        if (!CanFitPlacedInInventory(PlayerInventory.Instance, placed, __instance, ItemIdOf(placed)))
        {
            reason = "Passt nicht ins Inventar";
            __result = false;
            return false;
        }

        reason = string.Empty;
        __result = true;
        return false;
    }

    public static void OfferHallPickup(InteractableObject __instance)
    {
        if (__instance == null)
            return;
        var placed = PlacedRoot(__instance.transform);
        if (placed == null)
            return;
        var text = SafeMessage(__instance);
        if (!HallPickupPrompt.IsPickupMessage(text))
            return;

        if (ContentsUnresolved(placed))
        {
            __instance.message = "Inhalt geschuetzt";
            __instance.SetInteractableState(InteractableObject.EInteractableState.Invalid);
            return;
        }

        var built = placed.GetComponent<BuildableItem>() ?? placed.GetComponentInChildren<BuildableItem>(true);
        if (TryCheckNativePickupBlock(placed, built, out var blockReason))
        {
            __instance.message = blockReason;
            __instance.SetInteractableState(InteractableObject.EInteractableState.Invalid);
            return;
        }

        if (CanFitPlacedInInventory(PlayerInventory.Instance, placed, built, ItemIdOf(placed)))
        {
            __instance.message = "Pick up";
            __instance.SetInteractableState(InteractableObject.EInteractableState.Default);
        }
        else
        {
            __instance.message = "Won't fit in inventory";
            __instance.SetInteractableState(InteractableObject.EInteractableState.Invalid);
        }
    }

    private static bool TryCheckNativePickupBlock(GameObject placed, BuildableItem? built, out string reason)
    {
        reason = string.Empty;
        try
        {
            foreach (var storage in placed.GetComponentsInChildren<StorageEntity>(true))
            {
                if (storage != null && storage.IsOpened)
                {
                    reason = "Lager ist offen";
                    return true;
                }
            }

            foreach (var procTile in placed.GetComponentsInChildren<ProceduralTile>(true))
            {
                if (procTile?.Occupants != null && procTile.Occupants.Count > 0)
                {
                    reason = "Belegt";
                    return true;
                }
            }
        }
        catch { /* best-effort check */ }

        if (built == null)
            return false;

        try
        {
            if (!built.CanBeDestroyed(out var nativeReason)
                && !HallPickupPrompt.IsEmptyOnlyBlockReason(nativeReason))
            {
                reason = string.IsNullOrWhiteSpace(nativeReason) ? "In Benutzung" : nativeReason;
                return true;
            }
        }
        catch { /* fall through if native subclass check throws */ }

        return false;
    }

    private static string? SafeMessage(InteractableObject? obj)
    {
        try { return obj?.message; }
        catch (ArgumentException) { return null; }
    }

    public static bool StartPrompt(InteractableObject __instance)
    {
        if (__instance == null)
            return true;
        var placed = PlacedRoot(__instance.transform);
        if (placed == null)
            return true;
        var text = SafeMessage(__instance);
        if (!HallPickupPrompt.IsPickupMessage(text))
            return true;
        var item = placed.GetComponent<BuildableItem>() ?? placed.GetComponentInChildren<BuildableItem>(true);
        if (item != null)
            PickupHallItem(item);
        return false;
    }

    public static bool PickupHallItem(BuildableItem __instance)
    {
        var placed = __instance == null ? null : PlacedRoot(__instance.transform);
        if (placed == null)
            return true;
        if (!RecordIds.TryGetValue(placed.GetInstanceID(), out var id) || FailedContents.Contains(id))
        {
            MelonLogger.Warning("[DowntownWarehouse] Aufnahme blockiert, Inhalt nicht sicher.");
            Notify("Inhalt geschuetzt", "Gespeicherte Inhalte konnten nicht sicher geladen werden.", false);
            return false;
        }

        if (TryCheckNativePickupBlock(placed, __instance, out var blockReason))
        {
            Notify("Aufnahme blockiert", blockReason, false);
            return false;
        }

        try
        {
            string itemId = ItemIdOf(placed);
            var shelf = Il2CppScheduleOne.Registry.GetItem(itemId)?.GetDefaultInstance(1)
                ?? __instance?.ItemInstance?.GetCopy(1);
            if (shelf == null)
            {
                MelonLogger.Warning("[DowntownWarehouse] Aufnahme abgebrochen, Item-Definition fehlt: " + itemId);
                return false;
            }

            var slotCopies = new List<(ItemSlot Slot, ItemInstance Copy)>();
            var returning = new List<ItemInstance> { shelf };
            var slots = CollectItemSlots(placed);
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                var item = slot?.ItemInstance ?? slot?._ItemInstance_k__BackingField;
                int qty = slot?.Quantity ?? 0;
                if (qty < 1 && item != null)
                    qty = item.Quantity;
                if (item?.Definition == null || qty < 1)
                    continue;
                var copy = item.GetCopy(-1);
                if (copy == null)
                {
                    MelonLogger.Warning("[DowntownWarehouse] Aufnahme abgebrochen, Slot-Kopie fehlgeschlagen.");
                    return false;
                }
                slotCopies.Add((slot!, copy));
                returning.Add(copy);
            }

            var inventory = PlayerInventory.Instance;
            if (!CanFitAllInInventory(inventory, returning))
            {
                Notify("Inventar voll", "Nicht genug Platz für Objekt und Inhalt.", false);
                return false;
            }

            // Transfer slot contents first and clear each transferred slot immediately so a
            // mid-loop failure leaves zero duplicated and zero lost items.
            foreach (var (slot, copy) in slotCopies)
            {
                inventory!.AddItemToInventory(copy);
                slot.ClearStoredInstance(true);
            }

            inventory!.AddItemToInventory(shelf);
            ReleasePlacementFootprints(placed);
            DeregisterBuildableGuid(__instance);
            DetachPlacedFromProperty(placed);
            Forget(placed);
            UnityEngine.Object.Destroy(placed);
            return false;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[DowntownWarehouse] Aufnahme: " + ex.Message);
            return false;
        }
    }

    private static void RestorePlacements()
    {
        if (_save?.Placements == null)
            return;
        int restored = 0;
        var ordered = HallPlacementOrdering.SortForRestore(_save.Placements);
        _restoring = true;
        try
        {
            foreach (var record in ordered)
            {
                if (!record.IsPlausible() || !Spawned.Add(record.Id))
                    continue;
                if (RestoreOne(record))
                    restored++;
                else
                    Spawned.Remove(record.Id);
            }
        }
        finally
        {
            _restoring = false;
        }

        if (_save.Placements.Count > 0)
            MelonLogger.Msg($"[DowntownWarehouse] Objekte wiederhergestellt: {restored}/{_save.Placements.Count}.");
    }

    private static bool RestoreOne(HallPlacementRecord record)
    {
        if (!string.IsNullOrWhiteSpace(record.NativeDataJson) && !string.IsNullOrWhiteSpace(record.NativeDataType))
            return RestoreNative(record);

        var definition = Il2CppScheduleOne.Registry.GetItem(record.ItemId)?.TryCast<BuildableItemDefinition>();
        if (definition?.BuiltItem == null)
        {
            MelonLogger.Warning("[DowntownWarehouse] Restore ohne Bauvorbild: " + record.ItemId);
            return false;
        }

        var placed = SpawnBuilt(definition, new Vector3(record.X, record.Y, record.Z),
            new Quaternion(record.RotX, record.RotY, record.RotZ, record.RotW), record.ItemId, null, record.Id, null);
        if (placed == null)
            return false;
        RecordIds[placed.GetInstanceID()] = record.Id;
        Live[record.Id] = placed;
        FillContents(placed, record);
        return true;
    }

    private static bool RestoreNative(HallPlacementRecord record)
    {
        try
        {
            var payload = HallDynamicDataPayload.TryParse(record.NativeDataJson);
            if (payload == null)
                throw new InvalidDataException("native-json-ungueltig");

            string dataType = !string.IsNullOrWhiteSpace(payload.DataType) ? payload.DataType! : record.NativeDataType!;
            var loadManager = PersistentSingleton<LoadManager>.Instance;
            var loader = loadManager != null ? loadManager.GetObjectLoader(dataType) : null;
            if (loader == null)
                throw new InvalidOperationException("loader-fehlt:" + dataType);

            var dynamicData = new DynamicSaveData(new SaveData())
            {
                DataType = dataType,
                DataVersion = payload.DataVersion,
                GameVersion = payload.GameVersion ?? string.Empty,
                BaseData = payload.BaseData!
            };
            if (dynamicData.AdditionalDatas == null)
                dynamicData.AdditionalDatas = new il2cpp::Il2CppSystem.Collections.Generic.List<DynamicSaveData.AdditionalData>();
            if (payload.AdditionalDatas != null)
            {
                foreach (var extra in payload.AdditionalDatas)
                {
                    if (!string.IsNullOrEmpty(extra?.Name))
                        dynamicData.AddData(extra!.Name!, extra.Contents ?? string.Empty);
                }
            }

            var beforePtrs = SnapshotPropertyBuildablePointers();
            _suppressNativeSaveable++;
            try
            {
                loader.Load(dynamicData);
            }
            finally
            {
                if (_suppressNativeSaveable > 0)
                    _suppressNativeSaveable--;
            }

            var built = ResolveLoadedBuildable(record.Id, beforePtrs);
            if (!NetworkGuard.IsAlive(built))
                throw new InvalidOperationException("objekt-nicht-gespawnt");

            if (_property != null && built!.ParentProperty == null)
                built._ParentProperty_k__BackingField = _property;
            if (_property?.Container != null && built!.transform.parent != _property.Container.transform)
                built.transform.SetParent(_property.Container.transform, true);
            if (built!.TryCast<ProceduralGridItem>() is { } procRestored)
            {
                try { procRestored.RefreshTransform(); } catch { /* coroutine fallback */ }
            }
            AddOnce(_property?.BuildableItems, built);
            RecordIds[built!.gameObject.GetInstanceID()] = record.Id;
            Live[record.Id] = built.gameObject;
            FailedContents.Remove(record.Id);
            return true;
        }
        catch (Exception ex)
        {
            FailedContents.Add(record.Id);
            MelonLogger.Warning($"[DowntownWarehouse] Natives Restore fehlgeschlagen ({record.ItemId}/{record.NativeDataType}): {ex.Message}");
            return false;
        }
    }

    private static HashSet<IntPtr> SnapshotPropertyBuildablePointers()
    {
        var pointers = new HashSet<IntPtr>();
        var list = _property?.BuildableItems;
        if (list == null)
            return pointers;
        for (int i = 0; i < list.Count; i++)
        {
            var entry = list[i];
            if (entry != null && entry.Pointer != IntPtr.Zero)
                pointers.Add(entry.Pointer);
        }
        return pointers;
    }

    private static BuildableItem? ResolveLoadedBuildable(string recordId, HashSet<IntPtr> beforePtrs)
    {
        if (Guid.TryParse(recordId, out var parsed) && parsed != Guid.Empty)
        {
            try
            {
                var byGuid = GUIDManager.GetObject<BuildableItem>(new il2cpp::Il2CppSystem.Guid(parsed.ToString()));
                if (NetworkGuard.IsAlive(byGuid))
                    return byGuid;
            }
            catch { /* fall back to property list scan below */ }
        }

        var list = _property?.BuildableItems;
        if (list != null)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var entry = list[i];
                if (NetworkGuard.IsAlive(entry) && !beforePtrs.Contains(entry!.Pointer))
                    return entry;
            }
        }
        return null;
    }

    private static GameObject? SpawnBuilt(
        BuildableItemDefinition definition,
        Vector3 position,
        Quaternion rotation,
        string itemId,
        ItemInstance? instance,
        string recordId,
        BuildUpdate_Grid? buildUpdate)
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
            if (built != null)
            {
                if (built.ItemInstance == null)
                    built._ItemInstance_k__BackingField = instance?.GetCopy(1) ?? definition.GetDefaultInstance(1);
                if (_property != null)
                    built._ParentProperty_k__BackingField = _property;
                AssignBuiltGuid(built, recordId);
                built._Initialized_k__BackingField = true;
            }
            var net = placed.GetComponent<NetworkObject>();
            var manager = InstanceFinder.NetworkManager;
            if (net != null && manager != null)
                manager.ServerManager.Spawn(net);
            if (_placedRoot != null)
                placed.transform.SetParent(_placedRoot.transform, true);
            placed.transform.SetPositionAndRotation(position, rotation);
            if (built != null && built.TryCast<GridItem>() is { } gridItem)
            {
                if (_grid != null)
                {
                    gridItem._OwnerGrid_k__BackingField = _grid;
                    gridItem._ownerGridGUID = _grid.GUID;
                }
                int rotDeg = ((int)MathF.Round(rotation.eulerAngles.y / 90f) * 90 % 360 + 360) % 360;
                gridItem._rotation = rotDeg;
                var originTile = (gridItem.OriginFootprint != null ? TileUnder(gridItem.OriginFootprint.transform.position) : null)
                    ?? TileUnder(position);
                if (originTile != null)
                    gridItem._originCoordinate = new Vector2(originTile.x, originTile.y);
                if (buildUpdate != null)
                {
                    try { buildUpdate.OnPlacedObjectPreSpawn(gridItem); }
                    catch (Exception ex) { MelonLogger.Warning("[DowntownWarehouse] PreSpawn-Hook: " + ex.Message); }
                }
            }
            placed.SetActive(true);
            ClaimFootprint(placed);
            if (built != null && _property?.BuildableItems != null)
                AddOnce(_property.BuildableItems, built);
            return placed;
        }
        catch
        {
            if (restore)
                source.SetActive(true);
            if (placed != null)
            {
                ReleaseFootprint(placed);
                DetachPlacedFromProperty(placed);
                UnityEngine.Object.Destroy(placed);
            }
            throw;
        }
    }

    private static void AssignBuiltGuid(BuildableItem built, string recordId)
    {
        Guid parsed = Guid.TryParse(recordId, out var g) && g != Guid.Empty ? g : Guid.NewGuid();
        try
        {
            var il2cppGuid = new il2cpp::Il2CppSystem.Guid(parsed.ToString());
            built.SetGUID(il2cppGuid);
        }
        catch
        {
            try { built._GUID_k__BackingField = new il2cpp::Il2CppSystem.Guid(parsed.ToString()); }
            catch { /* non-fatal */ }
        }
    }

    private static void DetachPlacedFromProperty(GameObject placed)
    {
        if (_property?.BuildableItems == null || !NetworkGuard.IsAlive(placed))
            return;
        try
        {
            var built = placed.GetComponent<BuildableItem>() ?? placed.GetComponentInChildren<BuildableItem>(true);
            if (built != null)
                RemovePointer(_property.BuildableItems, built);
        }
        catch { /* best-effort */ }
    }

    private static HallPlacementRecord BuildPlacementRecord(string itemId, Vector3 position, Quaternion rotation, string? id = null) =>
        new()
        {
            Id = string.IsNullOrWhiteSpace(id) ? Guid.NewGuid().ToString() : id!,
            ItemId = itemId,
            X = position.x,
            Y = position.y,
            Z = position.z,
            RotX = rotation.x,
            RotY = rotation.y,
            RotZ = rotation.z,
            RotW = rotation.w
        };

    private static void RegisterRecord(GameObject placed, HallPlacementRecord record)
    {
        if (_save == null)
            throw new InvalidOperationException("hall-save-unbound");
        if (!record.IsPlausible())
            throw new InvalidOperationException("hall-pose-outside");
        _save.Placements.Add(record);
        RecordIds[placed.GetInstanceID()] = record.Id;
        Live[record.Id] = placed;
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

    private static List<ItemSlot> CollectItemSlots(GameObject placed)
    {
        var slots = new List<ItemSlot>();
        var seen = new HashSet<IntPtr>();

        void AddSlots(il2cpp::Il2CppSystem.Collections.Generic.List<ItemSlot>? list)
        {
            if (list == null)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                var slot = list[i];
                if (slot == null || slot.Pointer == IntPtr.Zero || !seen.Add(slot.Pointer))
                    continue;
                slots.Add(slot);
            }
        }

        foreach (var storage in placed.GetComponentsInChildren<StorageEntity>(true))
        {
            if (storage != null)
                AddSlots(storage.ItemSlots);
        }

        foreach (var component in placed.GetComponentsInChildren<Component>(true))
        {
            var owner = component == null ? null : component.TryCast<IItemSlotOwner>();
            if (owner != null)
                AddSlots(owner.ItemSlots);
        }

        return slots;
    }

    private static ItemInstance? DeserializeSavedItem(HallSlotRecord line)
    {
        if (!string.IsNullOrWhiteSpace(line.ItemJson))
        {
            var loaded = ItemDeserializer.LoadItem(line.ItemJson);
            if (loaded == null || loaded.Definition == null)
                throw new InvalidOperationException("item-deserialize-failed");
            if (loaded.Quantity != line.Quantity)
                loaded.Quantity = line.Quantity;
            return loaded;
        }

        return Il2CppScheduleOne.Registry.GetItem(line.ItemId)?.GetDefaultInstance(line.Quantity);
    }

    private static string? TrySerializeItemJson(ItemInstance? item)
    {
        if (item == null)
            return null;
        try
        {
            var data = item.GetItemData();
            var json = data?.GetJson(false);
            if (string.IsNullOrWhiteSpace(json) || json.Length > 4096)
                return null;
            return json;
        }
        catch
        {
            return null;
        }
    }

    private static void FillContents(GameObject placed, HallPlacementRecord record)
    {
        if (record.Contents == null || record.Contents.Count == 0)
            return;
        try
        {
            var slots = CollectItemSlots(placed);
            if (slots.Count == 0)
                throw new InvalidOperationException("slots-missing");
            foreach (var line in record.Contents)
            {
                if (line == null || line.Slot < 0 || line.Slot >= slots.Count)
                    throw new InvalidOperationException("slot-missing");
                var copy = DeserializeSavedItem(line);
                if (copy == null)
                    throw new InvalidOperationException("item-missing");
                slots[line.Slot].SetStoredItem(copy, true);
            }

            FailedContents.Remove(record.Id);
        }
        catch (Exception ex)
        {
            FailedContents.Add(record.Id);
            if (NetworkGuard.IsAlive(placed))
            {
                try { placed.SetActive(false); } catch { }
            }
            MelonLogger.Warning("[DowntownWarehouse] Inhalt bleibt geschuetzt (Objekt deaktiviert): " + ex.Message);
        }
    }

    private static void CaptureNativeState(BuildableItem built, HallPlacementRecord record)
    {
        try
        {
            var dynamicData = built.GetSaveData();
            if (dynamicData != null)
            {
                string? json = dynamicData.GetJson(false);
                string? dataType = dynamicData.DataType;
                if (!string.IsNullOrWhiteSpace(json)
                    && !string.IsNullOrWhiteSpace(dataType)
                    && json.Length <= HallPlacementRecord.MaxNativeDataJsonLength)
                {
                    record.NativeDataType = dataType;
                    record.NativeDataJson = json;
                }
            }

            var loader = built.Loader?.TryCast<BuildableItemLoader>();
            if (loader == null && !string.IsNullOrWhiteSpace(record.NativeDataType))
            {
                var loadManager = PersistentSingleton<LoadManager>.Instance;
                loader = loadManager != null ? loadManager.GetObjectLoader(record.NativeDataType!)?.TryCast<BuildableItemLoader>() : null;
            }
            if (loader != null)
                record.LoadOrder = loader.LoadOrder;
            else if (built.TryCast<ProceduralGridItem>() != null)
                record.LoadOrder = 1;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[DowntownWarehouse] Native SaveData nicht gelesen: " + ex.Message);
        }
    }

    private static void CaptureContents()
    {
        if (_save?.Placements == null)
            return;

        if (_property?.BuildableItems != null)
        {
            for (int i = 0; i < _property.BuildableItems.Count; i++)
            {
                var item = _property.BuildableItems[i];
                if (NetworkGuard.IsAlive(item) && !RecordIds.ContainsKey(item!.gameObject.GetInstanceID()))
                    TrackNativePlaced(item);
            }
        }

        foreach (var record in _save.Placements)
        {
            if (record == null || FailedContents.Contains(record.Id))
                continue;
            if (!Live.TryGetValue(record.Id, out var placed) || !NetworkGuard.IsAlive(placed))
                continue;

            var read = ReadContents(placed!);
            if (read != null)
                record.Contents = read;

            var built = placed!.GetComponent<BuildableItem>() ?? placed.GetComponentInChildren<BuildableItem>(true);
            if (NetworkGuard.IsAlive(built))
                CaptureNativeState(built!, record);

            var t = placed.transform;
            var pos = t.position;
            if (HallPlan.ContainsPlacement(pos.x, pos.y, pos.z))
            {
                record.X = pos.x;
                record.Y = pos.y;
                record.Z = pos.z;
                var rot = t.rotation;
                record.RotX = rot.x;
                record.RotY = rot.y;
                record.RotZ = rot.z;
                record.RotW = rot.w;
            }
        }
    }

    private static List<HallSlotRecord>? ReadContents(GameObject placed)
    {
        try
        {
            var slots = CollectItemSlots(placed);
            int expectedMin = 0;
            foreach (var storage in placed.GetComponentsInChildren<StorageEntity>(true))
            {
                if (storage != null && storage.ItemCount > expectedMin)
                    expectedMin = storage.ItemCount;
            }
            if (expectedMin > 0 && slots.Count == 0)
                return null;
            if (slots.Count == 0)
                return new List<HallSlotRecord>();

            var found = new List<HallSlotRecord>();
            int foundQuantity = 0;
            for (int i = 0; i < slots.Count && i < 64; i++)
            {
                var slot = slots[i];
                var item = slot?.ItemInstance ?? slot?._ItemInstance_k__BackingField;
                var id = item?.Definition?.ID;
                int quantity = slot?.Quantity ?? 0;
                if (quantity < 1 && item != null)
                    quantity = item.Quantity;
                if (string.IsNullOrWhiteSpace(id) || quantity < 1)
                    continue;
                foundQuantity += quantity;
                found.Add(new HallSlotRecord
                {
                    Slot = i,
                    ItemId = id!,
                    Quantity = quantity,
                    ItemJson = TrySerializeItemJson(item)
                });
            }

            if (expectedMin > foundQuantity)
                return null;
            return found;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[DowntownWarehouse] Inhalt nicht gelesen: " + ex.Message);
            return null;
        }
    }

    // Allocation-free hover/preflight check: inspects the live ItemInstance on the placed
    // object and its slots directly instead of cloning IL2CPP ItemInstances every frame.
    private static bool CanFitPlacedInInventory(PlayerInventory? inventory, GameObject placed, BuildableItem? built, string id)
    {
        if (inventory == null)
            return false;

        var shelf = built?.ItemInstance;
        if (shelf == null || shelf.Definition == null)
        {
            shelf = Il2CppScheduleOne.Registry.GetItem(id)?.GetDefaultInstance(1);
            if (shelf == null)
                return false;
        }

        var refs = new List<(ItemInstance Item, int Quantity)> { (shelf, 1) };
        var slots = CollectItemSlots(placed);
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            var item = slot?.ItemInstance ?? slot?._ItemInstance_k__BackingField;
            int qty = slot?.Quantity ?? 0;
            if (qty < 1 && item != null)
                qty = item.Quantity;
            if (item?.Definition == null || qty < 1)
                continue;
            refs.Add((item, qty));
        }

        return CanFitAllRefsInInventory(inventory, refs);
    }

    private static bool CanFitAllInInventory(PlayerInventory? inventory, IReadOnlyList<ItemInstance> returning)
    {
        if (inventory == null || returning.Count == 0)
            return false;
        var refs = new List<(ItemInstance Item, int Quantity)>(returning.Count);
        for (int i = 0; i < returning.Count; i++)
        {
            var copy = returning[i];
            if (copy == null)
                return false;
            refs.Add((copy, copy.Quantity));
        }
        return CanFitAllRefsInInventory(inventory, refs);
    }

    private static bool CanFitAllRefsInInventory(PlayerInventory? inventory, IReadOnlyList<(ItemInstance Item, int Quantity)> returning)
    {
        if (inventory == null || returning.Count == 0)
            return false;

        var invSlots = inventory.GetAllInventorySlots();
        if (invSlots == null || invSlots.Count == 0)
            return false;

        var budgets = new List<HallSlotBudget>(invSlots.Count);
        for (int i = 0; i < invSlots.Count; i++)
        {
            var slot = invSlots[i];
            if (slot == null)
                continue;
            var cur = slot.ItemInstance ?? slot._ItemInstance_k__BackingField;
            bool isEmpty = cur == null || slot.Quantity <= 0;
            budgets.Add(new HallSlotBudget
            {
                Index = i,
                Locked = slot.IsLocked || slot.IsAddLocked,
                IsEmpty = isEmpty,
                Quantity = isEmpty ? 0 : slot.Quantity
            });
        }

        var stackGroups = new List<(ItemInstance Rep, string Key)>();
        var needs = new List<HallReturnNeed>(returning.Count);

        for (int k = 0; k < returning.Count; k++)
        {
            var (copy, qty) = returning[k];
            if (copy?.Definition == null || qty < 1)
                return false;
            if (!inventory.CanItemFitInInventory(copy, 1))
                return false;

            string stackKey = "";
            for (int g = 0; g < stackGroups.Count; g++)
            {
                if (copy.CanStackWith(stackGroups[g].Rep, false))
                {
                    stackKey = stackGroups[g].Key;
                    break;
                }
            }
            if (stackKey.Length == 0)
            {
                stackKey = "G" + stackGroups.Count;
                stackGroups.Add((copy, stackKey));
            }

            var existingCaps = new Dictionary<int, int>();
            var allowedEmpty = new List<int>();
            int stackLimit = 0;

            for (int i = 0; i < invSlots.Count; i++)
            {
                var slot = invSlots[i];
                if (slot == null || slot.IsLocked || slot.IsAddLocked)
                    continue;
                int cap = slot.GetCapacityForItem(copy, true);
                if (cap <= 0)
                    continue;

                var cur = slot.ItemInstance ?? slot._ItemInstance_k__BackingField;
                if (cur == null || slot.Quantity <= 0)
                {
                    allowedEmpty.Add(i);
                    if (cap > stackLimit)
                        stackLimit = cap;
                }
                else
                {
                    existingCaps[i] = cap;
                    int totalCap = cap + slot.Quantity;
                    if (totalCap > stackLimit)
                        stackLimit = totalCap;
                }
            }

            if (stackLimit < 1)
                stackLimit = Math.Max(1, qty);

            needs.Add(new HallReturnNeed
            {
                StackKey = stackKey,
                Quantity = qty,
                StackLimit = stackLimit,
                ExistingStackCapacities = existingCaps,
                AllowedEmptySlots = allowedEmpty
            });
        }

        return HallInventoryPlanner.CanFitAll(budgets, needs);
    }

    private static void FlushSave()
    {
        if (_save == null)
            return;
        if (!HallSaveRules.ShouldWriteSave(_owned, _saveKind, _save.Placements?.Count ?? 0))
            return;
        try
        {
            CaptureContents();
            if (WriteSave())
                _saveKind = HallSaveKind.Ready;
            else
                MelonLogger.Warning("[DowntownWarehouse] Spielstand konnte nicht geschrieben werden (SafeStorage meldete false).");
        }
        catch (Exception ex) { MelonLogger.Warning("[DowntownWarehouse] Spielstand nicht geschrieben: " + ex.Message); }
    }

    private static bool WriteSave()
    {
        if (_save == null || string.IsNullOrWhiteSpace(_savePath) || _saveKind is HallSaveKind.ForeignSlot or HallSaveKind.Unsupported or HallSaveKind.Corrupt)
            return false;
        _save.Schema = HallPlan.Schema;
        _save.PropertyCode = HallPlan.Code;
        _save.SaveFolder = _saveFolder;
        _save.Owned = _owned;
        return SafeStorage.SaveAtomic(_savePath, _save);
    }

    private static bool OverInterior(Vector3 point) =>
        HallPlan.IsOverInterior(point.x, point.y, point.z);

    private static bool IsHallGhost(GameObject? ghost)
    {
        if (ghost == null)
            return false;
        if (OverInterior(ghost.transform.position))
            return true;
        var feet = ghost.GetComponentsInChildren<FootprintTile>(true);
        if (feet == null)
            return false;
        for (int i = 0; i < feet.Length; i++)
        {
            var foot = feet[i];
            if (foot != null && OverInterior(foot.transform.position))
                return true;
        }
        return false;
    }

    private static bool FootprintClear(GameObject ghost)
    {
        var feet = ghost.GetComponentsInChildren<FootprintTile>(true);
        if (feet == null || feet.Length == 0)
        {
            var single = TileUnder(ghost.transform.position);
            return single != null && !TileTaken(single);
        }
        foreach (var foot in feet)
        {
            if (foot == null)
                continue;
            var tile = TileUnder(foot.transform.position);
            if (tile == null || TileTaken(tile) || tile.AvailableOffset + 0.001f < foot.RequiredOffset)
                return false;
        }

        return true;
    }

    private static void ClaimFootprint(GameObject placed)
    {
        var item = placed.GetComponent<GridItem>() ?? placed.GetComponentInChildren<GridItem>(true);
        if (item == null)
            return;

        var coordPairs = item.CoordinatePairs;
        if (coordPairs == null)
        {
            coordPairs = new il2cpp::Il2CppSystem.Collections.Generic.List<CoordinatePair>();
            item.CoordinatePairs = coordPairs;
        }
        else
        {
            coordPairs.Clear();
        }

        var footprintPairs = item.CoordinateFootprintTilePairs;
        if (footprintPairs != null && footprintPairs.Count > 0)
        {
            for (int i = 0; i < footprintPairs.Count; i++)
            {
                var pair = footprintPairs[i];
                var foot = pair?.footprintTile;
                if (foot == null)
                    continue;
                var tile = TileUnder(foot.transform.position);
                if (tile != null && IsOurGrid(tile.OwnerGrid))
                {
                    tile.AddOccupant(item, foot);
                    if (pair!.coord != null)
                        coordPairs.Add(new CoordinatePair(pair.coord, new Coordinate(tile.x, tile.y)));
                }
            }
            return;
        }

        foreach (var foot in placed.GetComponentsInChildren<FootprintTile>(true))
        {
            var tile = TileUnder(foot.transform.position);
            if (tile != null && IsOurGrid(tile.OwnerGrid))
                tile.AddOccupant(item, foot);
        }
    }

    private static void ReleasePlacementFootprints(GameObject placed)
    {
        ReleaseFootprint(placed);
        if (!NetworkGuard.IsAlive(placed))
            return;
        try
        {
            var growLight = placed.GetComponent<Il2CppScheduleOne.ObjectScripts.GrowLight>()
                ?? placed.GetComponentInChildren<Il2CppScheduleOne.ObjectScripts.GrowLight>(true);
            if (NetworkGuard.IsAlive(growLight) && growLight!.usableLightSource != null && growLight.footprintTileMatches != null)
            {
                for (int i = 0; i < growLight.footprintTileMatches.Count; i++)
                {
                    var node = growLight.footprintTileMatches[i]?.tile?.MatchedFootprintTile?.MatchedStandardTile?.LightExposureNode;
                    if (node != null)
                        node.RemoveSource(growLight.usableLightSource);
                }
            }

            var proc = placed.GetComponent<ProceduralGridItem>() ?? placed.GetComponentInChildren<ProceduralGridItem>(true);
            if (NetworkGuard.IsAlive(proc))
                proc!.ClearPositionData();
        }
        catch { /* best-effort procedural tile release */ }
    }

    private static void DeregisterBuildableGuid(BuildableItem? built)
    {
        if (!NetworkGuard.IsAlive(built))
            return;
        try
        {
            var reg = built!.TryCast<IGUIDRegisterable>();
            if (reg != null)
                GUIDManager.DeregisterObject(reg);
        }
        catch { /* best-effort GUID deregistration */ }
    }

    private static void DeregisterOwnedGuids()
    {
        foreach (var go in Live.Values)
        {
            if (!NetworkGuard.IsAlive(go))
                continue;
            var built = go!.GetComponent<BuildableItem>() ?? go.GetComponentInChildren<BuildableItem>(true);
            DeregisterBuildableGuid(built);
        }

        var items = _property?.BuildableItems;
        if (items != null)
        {
            for (int i = 0; i < items.Count; i++)
                DeregisterBuildableGuid(items[i]);
        }

        if (NetworkGuard.IsAlive(_grid))
        {
            try
            {
                var reg = _grid!.TryCast<IGUIDRegisterable>();
                if (reg != null)
                    GUIDManager.DeregisterObject(reg);
            }
            catch { }
        }
    }

    private static void ReleaseFootprint(GameObject placed)
    {
        if (!NetworkGuard.IsAlive(placed))
            return;
        var item = placed.GetComponent<GridItem>() ?? placed.GetComponentInChildren<GridItem>(true);
        if (item == null)
            return;
        foreach (var foot in placed.GetComponentsInChildren<FootprintTile>(true))
        {
            if (foot == null)
                continue;
            var tile = TileUnder(foot.transform.position);
            tile?.RemoveOccupant(item, foot);
        }
        try { item.CoordinatePairs?.Clear(); } catch { }
    }

    private static bool TileTaken(Tile tile)
    {
        var occupants = tile.BuildableOccupants;
        return occupants != null && occupants.Count > 0;
    }

    private static Tile? TileUnder(Vector3 world)
    {
        if (_gridHost == null || TileGrid.Count == 0)
            return null;
        if (!HallPlan.TryWorldToTileCoord(world.x, world.z, _gridOriginX, _gridOriginZ, Grid.TileSize, out int x, out int z))
            return null;
        if (!TileGrid.TryGetValue((x, z), out var tile) || tile == null)
            return null;
        float half = Grid.TileSize * 0.55f;
        var position = tile.transform.position;
        if (Math.Abs(position.x - world.x) > half || Math.Abs(position.z - world.z) > half)
            return null;
        return tile;
    }

    private static GameObject? PlacedRoot(Transform? node)
    {
        while (node != null)
        {
            if (node.name.StartsWith(PlacedPrefix, StringComparison.Ordinal) || RecordIds.ContainsKey(node.gameObject.GetInstanceID()))
                return node.gameObject;
            var built = node.GetComponent<BuildableItem>();
            if (built != null && IsOurs(built.ParentProperty))
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

        if (placed.name.StartsWith(PlacedPrefix, StringComparison.Ordinal))
            return placed.name.Substring(PlacedPrefix.Length);

        var built = placed.GetComponent<BuildableItem>() ?? placed.GetComponentInChildren<BuildableItem>(true);
        return built?.ItemInstance?.Definition?.ID ?? "";
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
        if (NetworkGuard.IsAlive(go))
        {
            try { UnityEngine.Object.Destroy(go!); } catch { }
        }
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
        float before;
        try
        {
            manager = MoneyManager.Instance;
            if (!NetworkGuard.IsAlive(manager))
                return WalletOutcome.NotApplied;
            before = bank ? manager!.onlineBalance : manager!.cashBalance;
            float expected = before + delta;
            if (!float.IsFinite(expected) || (delta < 0f && expected < -0.001f))
                return WalletOutcome.NotApplied;
            if (bank)
                manager.RpcLogic___ReceiveOnlineTransaction_1419830531(HallPlan.Name, delta, 1f, HallPlan.Code);
            else
                manager.ChangeCashBalance(delta, true, true);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[DowntownWarehouse] Zahlung: " + ex.Message);
            return WalletOutcome.Unknown;
        }

        try
        {
            var current = MoneyManager.Instance;
            if (current != null && current.Pointer == manager.Pointer)
            {
                float now = bank ? current.onlineBalance : current.cashBalance;
                if (HallWalletCheck.MatchesExpected(before, delta, now))
                    return WalletOutcome.Applied;
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("[DowntownWarehouse] Kontostand: " + ex.Message);
        }

        return WalletOutcome.Unknown;
    }
}
