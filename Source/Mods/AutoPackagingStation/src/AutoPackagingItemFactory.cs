using System;
using System.Collections;
using AutoPackagingStation.Entities;
using AutoPackagingStation.Persistence;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.Tiles;
using Il2CppScheduleOne.UI.Shop;
using MelonLoader;
using S1API.Items.Buildable;
using UnityEngine;
using BuildableItemCreator = S1API.Items.Buildable.BuildableItemCreator;
using BuildableItemDefinitionBuilder = S1API.Items.Buildable.BuildableItemDefinitionBuilder;
using GameRegistry = Il2CppScheduleOne.Registry;
using NativeBuildableItemDef = Il2CppScheduleOne.ItemFramework.BuildableItemDefinition;
using NativeStorableItemDef = Il2CppScheduleOne.ItemFramework.StorableItemDefinition;

namespace AutoPackagingStation.Items;

/// <summary>
/// Factory responsible for registering the 4x4 Auto-Packaging Station BuildableItem with S1API,
/// injecting hardware shop listings, and setting up placed 4x4 station instances in the world.
/// </summary>
public static class AutoPackagingItemFactory
{
    private static bool _isRegistered = false;
    private static int _injectRetryCount = 0;
    private const int MaxInjectRetries = 10;

    /// <summary>
    /// Critic Pillar 1: Clones 4x4 base entity via S1API BuildableItemCreator.CloneFrom("cauldron") / "packagingstation".
    /// </summary>
    public static void RegisterItem()
    {
        // Gatekeeper-fix 2026-08-29: was `if (_isRegistered) return;` which left the mod
        // silently broken after a Registry reset (patch-day, save-slot switch, dynamic reload).
        // Now we also re-verify the item actually exists in the Registry — if not, we retry.
        string itemId = Mod.CurrentConfig.StationItemId;
        if (_isRegistered && GameRegistry.ItemExists(itemId)) return;
        _isRegistered = false; // re-arm in case previous registration was lost (registry reset)

        try
        {
            // itemId hoisted to outer guard (Gatekeeper-fix 2026-08-29); do not redeclare.
            if (GameRegistry.ItemExists(itemId))
            {
                Mod.Log.Info($"Item '{itemId}' already exists in Registry.");
                _isRegistered = true;
                return;
            }

            Mod.Log.Info($"Registering custom buildable 4x4 item '{itemId}'...");

            // Look for existing buildable base item to inherit GridItem and 4x4 footprint tiles
            string[] candidateBases = new[] { "packagingstation", "packagingstationmk2", "cauldron", "brickpress", "dryingrack", "pot", "storage" };
            string? baseId = null;
            foreach (var candidate in candidateBases)
            {
                if (GameRegistry.ItemExists(candidate))
                {
                    var def = GameRegistry.GetItem(candidate);
                    if (def != null && def.Pointer != IntPtr.Zero && def.TryCast<NativeBuildableItemDef>() != null)
                    {
                        baseId = candidate;
                        break;
                    }
                }
            }

            BuildableItemDefinitionBuilder builder;
            if (!string.IsNullOrEmpty(baseId))
            {
                Mod.Log.Info($"Cloning Auto-Packaging Station definition from base item '{baseId}' (inherits GridItem & footprint)...");
                builder = BuildableItemCreator.CloneFrom(baseId);
            }
            else
            {
                Mod.Log.Info("Creating Auto-Packaging Station builder from scratch...");
                builder = BuildableItemCreator.CreateBuilder();
            }

            builder.WithBasicInfo(
                       itemId,
                       Mod.CurrentConfig.StationDisplayName,
                       "Industrial 4x4 automated packaging station. Automatically feeds raw product through an animated pneumatic conveyor system with +5% freshness quality bonus.",
                       S1API.Items.ItemCategory.Furniture)
                   .WithPricing(Mod.CurrentConfig.StationPurchasePrice, 0.5f)
                   .WithIcon(AutoPackMeshBuilder.GetOrCreateIconSprite())
                   .WithGhostVisual((Transform parent) =>
                   {
                       var ghost = CreateGhostPrefab();
                       ghost.transform.SetParent(parent, false);
                       ghost.name = "AutoPackagingStation_Ghost";
                       return ghost;
                   }, replaceExistingVisual: true);

            // Bug 3 Fix: Level-gate — Tier 3 packaging machine requires Hustler rank
            // (after Tier 1 "packagingstation" StreetRat, Tier 2 "packagingstationmk2" Peddler)
            builder.WithRequiredRank(new S1API.Leveling.FullRank(S1API.Leveling.Rank.Hustler, 1));

            var built = builder.Build();

            // Bug 1 Fix: Expand GridItem footprint from inherited 1x1 to 2x2 tiles
            // so the placement system blocks overlapping tiles (prevents clipping into shelves).
            ExpandFootprintTo2x2(built);

            _isRegistered = true;
            Mod.Log.Info($"Successfully registered '{itemId}' (2x2 footprint, Hustler I rank-gated).");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to register AutoPackagingStation item: {ex}");
        }
    }

    public static void OnSaveInfoLoaded()
    {
        RegisterItem();
    }

    /// <summary>
    /// Injects the Auto-Packaging Station into the hardware store catalogue (e.g. Handy Hank's).
    /// </summary>
    public static void InjectHardwareStoreListing()
    {
        try
        {
            string itemId = Mod.CurrentConfig.StationItemId;
            var def = GameRegistry.GetItem(itemId);
            if (def == null || def.Pointer == IntPtr.Zero) return;

            var storable = def.TryCast<NativeStorableItemDef>();
            if (storable == null || storable.Pointer == IntPtr.Zero) return;

            var shops = ShopInterface.AllShops;
            if (shops == null || shops.Count == 0)
            {
                if (_injectRetryCount < MaxInjectRetries)
                {
                    _injectRetryCount++;
                    MelonCoroutines.Start(DelayedShopRetry());
                }
                return;
            }
            _injectRetryCount = 0;

            for (int i = 0; i < shops.Count; i++)
            {
                var shop = shops[i];
                if (shop == null || shop.Pointer == IntPtr.Zero) continue;

                string code = (shop.ShopCode ?? string.Empty).ToLowerInvariant();
                string name = (shop.ShopName ?? string.Empty).ToLowerInvariant();

                if (!code.Contains("hardware") && !name.Contains("hardware") && !code.Contains("dan") && !name.Contains("dan") && !code.Contains("hank") && !name.Contains("hank")) continue;

                var listings = shop.Listings;
                if (listings == null) continue;

                // Check for existing listing — repair stock + vanilla UI row (idempotent).
                bool alreadyIn = false;
                for (int j = 0; j < listings.Count; j++)
                {
                    var existing = listings[j];
                    if (existing != null && existing.Item != null && existing.Item.ID == itemId)
                    {
                        S1Mods.Shared.ShopListingSync.OnListingExists(shop, existing, 10, msg => Mod.Log.Warn(msg));
                        alreadyIn = true;
                        break;
                    }
                }
                if (alreadyIn) continue;

                var listing = new ShopListing
                {
                    name = "AutoPackagingStation_ShopListing",
                    Item = storable,
                    OverridePrice = true,
                    OverriddenPrice = Mod.CurrentConfig.StationPurchasePrice,
                    LimitedStock = false,
                    DefaultStock = 10,
                    CanBeDelivered = true
                };

                listings.Add(listing);
                // Raw Add() is invisible to the vanilla UI — Initialize + ListingUI row required.
                S1Mods.Shared.ShopListingSync.OnListingAdded(shop, listing, 10, msg => Mod.Log.Info(msg), msg => Mod.Log.Warn(msg));
                Mod.Log.Info($"Injected '{itemId}' into hardware shop listing ('{shop.ShopName}').");
                continue;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Failed to inject hardware store listing: {ex.Message}");
        }
    }

    private static IEnumerator DelayedShopRetry()
    {
        yield return new WaitForSeconds(2.0f);
        InjectHardwareStoreListing();
    }

    /// <summary>
    /// Creates a visual ghost prefab for placement preview.
    /// </summary>
    public static GameObject CreateGhostPrefab()
    {
        var root = new GameObject("AutoPackagingStation_GhostRoot");

        // 2x2 Footprint Box Collider — matches 2m visual (H4: former 4x4 claim was inaccurate vs cloned 1x1 footprint tiles)
        var box = root.AddComponent(Il2CppType.Of<BoxCollider>()).Cast<BoxCollider>();
        box.center = new Vector3(0f, 0.70f, 0f);
        box.size = new Vector3(2.0f, 1.40f, 2.0f);

        // Build procedural visuals
        var dummyController = root.AddComponent(Il2CppType.Of<AutoPackStationController>()).Cast<AutoPackStationController>();
        AutoPackMeshBuilder.BuildStationVisuals(root, dummyController);
        GameObject.Destroy(dummyController);
        // Orphan AudioSource added by Awake — remove (M3)
        var orphanAudio = root.GetComponent<AudioSource>();
        if (orphanAudio != null && orphanAudio.Pointer != IntPtr.Zero) GameObject.Destroy(orphanAudio);

        return root;
    }

    /// <summary>
    /// Configures a placed station GameObject in the game world with 4x4 box collider, visual mesh, and controller.
    /// </summary>
    public static void SetupPlacedStation(GameObject go, AutoPackStationSaveData? data = null)
    {
        if (go == null || go.Pointer == IntPtr.Zero) return;

        try
        {
            // 1. Configure 4x4 BoxCollider
            var box = go.GetComponent<BoxCollider>() ?? go.AddComponent(Il2CppType.Of<BoxCollider>()).Cast<BoxCollider>();
            box.center = new Vector3(0f, 0.70f, 0f);
            box.size = new Vector3(2.0f, 1.40f, 2.0f);
            box.enabled = true;

            // 2. Ensure PackagingStation component and CameraPosition transform
            var station = go.GetComponent<Il2CppScheduleOne.ObjectScripts.PackagingStation>() ?? go.GetComponentInChildren<Il2CppScheduleOne.ObjectScripts.PackagingStation>(true);
            if (station == null || station.Pointer == IntPtr.Zero)
            {
                station = go.AddComponent(Il2CppType.Of<Il2CppScheduleOne.ObjectScripts.PackagingStation>()).Cast<Il2CppScheduleOne.ObjectScripts.PackagingStation>();
            }

            if (station != null && station.Pointer != IntPtr.Zero)
            {
                Transform camPos = station.CameraPosition;
                if (camPos == null || camPos.Pointer == IntPtr.Zero)
                {
                    var camChild = go.transform.Find("CameraPosition");
                    GameObject camGo;
                    if (camChild != null && camChild.Pointer != IntPtr.Zero)
                    {
                        camGo = camChild.gameObject;
                    }
                    else
                    {
                        camGo = new GameObject("CameraPosition");
                        camGo.transform.SetParent(go.transform, false);
                    }
                    camGo.transform.localPosition = new Vector3(0f, 1.6f, -1.8f);
                    camGo.transform.localRotation = Quaternion.LookRotation(new Vector3(0f, 0.7f, 0f) - new Vector3(0f, 1.6f, -1.8f));
                    station.CameraPosition = camGo.transform;
                }
                else
                {
                    camPos.localPosition = new Vector3(0f, 1.6f, -1.8f);
                    camPos.localRotation = Quaternion.LookRotation(new Vector3(0f, 0.7f, 0f) - new Vector3(0f, 1.6f, -1.8f));
                }
            }

            // 2b. Bug 2 Fix: Ensure InteractableObject exists for E-key storage interaction.
            // The native PackagingStation expects an InteractableObject component to trigger
            // its canvas (storage/slot UI). Without it, pressing E on the Kessel does nothing.
            if (station != null && station.Pointer != IntPtr.Zero)
            {
                try
                {
                    var interactable = go.GetComponent<Il2CppScheduleOne.Interaction.InteractableObject>();
                    if (interactable == null || interactable.Pointer == IntPtr.Zero)
                    {
                        interactable = go.AddComponent(Il2CppType.Of<Il2CppScheduleOne.Interaction.InteractableObject>()).Cast<Il2CppScheduleOne.Interaction.InteractableObject>();
                        Mod.Log.Info("Added InteractableObject to AutoPackagingStation for E-key interaction.");
                    }

                    // Configure the interactable
                    interactable.SetMessage("Open Packaging Station");
                    interactable.MaxInteractionRange = Mod.CurrentConfig.InteractionRange;
                    interactable.Priority = 0;
                    interactable.LimitInteractionAngle = false;

                    // Wire up E-key interaction — native PackagingStation.Interacted() opens the canvas
                    S1API.Utils.EventHelper.AddListener(
                        new Action(() =>
                        {
                            try
                            {
                                if (station.Pointer != IntPtr.Zero)
                                {
                                    var canvas = Il2CppScheduleOne.UI.Stations.PackagingStationCanvas.Instance;
                                    if (canvas != null && canvas.Pointer != IntPtr.Zero && !canvas.gameObject.activeSelf)
                                    {
                                        station.Interacted();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Mod.Log.Debug($"InteractableObject onInteractStart error: {ex.Message}");
                            }
                        }),
                        interactable.onInteractStart);
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"InteractableObject setup failed (non-fatal): {ex.Message}");
                }
            }

            // 3. Attach AutoPackStationController
            var controller = go.GetComponent<AutoPackStationController>() ?? go.AddComponent(Il2CppType.Of<AutoPackStationController>()).Cast<AutoPackStationController>();

            // 4. Ensure GUID
            if (data != null && !string.IsNullOrEmpty(data.Guid))
            {
                controller.StationGuid = data.Guid;
            }
            controller.EnsureGuid();

            // 5. Build 4x4 Industrial Visuals
            AutoPackMeshBuilder.BuildStationVisuals(go, controller);

            // 6. Clean and hide base renderers
            controller.HideBaseRenderers();

            // 7. Restore data if provided
            if (data != null)
            {
                AutoPackStore.ApplySaveData(controller.StationGuid, data);
                controller.RestoreNativeSlots(data);
            }
            else
            {
                var staged = AutoPackStore.GetStagedSaveData(controller.StationGuid);
                if (staged != null)
                {
                    controller.RestoreNativeSlots(staged);
                }
            }

            AutoPackStore.RegisterStation(controller);
            Mod.Log.Info($"Configured 4x4 AutoPackagingStation [{controller.StationGuid}] in world.");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"SetupPlacedStation error: {ex}");
        }
    }

    /// <summary>
    /// Bug 1 Fix: Expands the GridItem footprint from the inherited 1x1 to 2x2 tiles,
    /// matching the 2x2m physical footprint. This prevents tile overlap (e.g. clipping into shelves).
    /// Grid tile size is 0.5m per tile — a 2x2m station occupies 4x4 tiles (2x2 in each axis).
    /// </summary>
    private static void ExpandFootprintTo2x2(S1API.Items.Buildable.BuildableItemDefinition? built)
    {
        try
        {
            if (built == null) { Mod.Log.Warn("ExpandFootprint: built definition is null."); return; }

            // Access the native definition directly from the registry (the builder already registered it).
            string itemId = Mod.CurrentConfig.StationItemId;
            var nativeDef = GameRegistry.GetItem(itemId)?.TryCast<NativeBuildableItemDef>();
            if (nativeDef == null || nativeDef.Pointer == IntPtr.Zero)
            {
                Mod.Log.Warn("ExpandFootprint: Native definition not found in Registry.");
                return;
            }

            var builtItem = nativeDef.BuiltItem;
            if (builtItem == null || builtItem.Pointer == IntPtr.Zero)
            {
                Mod.Log.Warn("ExpandFootprint: BuiltItem is null on native definition.");
                return;
            }

            var gridItem = builtItem.GetComponent<Il2CppScheduleOne.EntityFramework.GridItem>();
            if (gridItem == null || gridItem.Pointer == IntPtr.Zero)
            {
                Mod.Log.Warn("ExpandFootprint: No GridItem on BuiltItem — skipping footprint expansion.");
                return;
            }

            var pairs = gridItem.CoordinateFootprintTilePairs;
            if (pairs == null || pairs.Count == 0)
            {
                Mod.Log.Warn("ExpandFootprint: CoordinateFootprintTilePairs is empty — cannot expand.");
                return;
            }

            // Clone the template tile from the first pair
            var templateTile = pairs[0].footprintTile;
            if (templateTile == null || templateTile.Pointer == IntPtr.Zero)
            {
                Mod.Log.Warn("ExpandFootprint: Template FootprintTile is null.");
                return;
            }

            // Build 2x2 grid: coordinates (0,0), (1,0), (0,1), (1,1) at 0.5m spacing
            const float tileSpacing = 0.5f;
            int[,] coords = new int[,] { { 0, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } };

            // Keep existing pair for (0,0), add (1,0), (0,1), (1,1)
            for (int i = 1; i < 4; i++)
            {
                int x = coords[i, 0];
                int y = coords[i, 1];

                // Check if already exists
                bool exists = false;
                for (int p = 0; p < pairs.Count; p++)
                {
                    if (pairs[p].coord.x == x && pairs[p].coord.y == y) { exists = true; break; }
                }
                if (exists) continue;

                // Clone tile under same parent
                var parent = templateTile.transform.parent;
                var newTileGo = UnityEngine.Object.Instantiate(templateTile.gameObject, parent);
                newTileGo.name = $"FootprintTile_{x}_{y}";
                newTileGo.transform.localPosition = new Vector3(x * tileSpacing, 0f, y * tileSpacing);
                newTileGo.SetActive(true);

                var newTile = newTileGo.GetComponent<Il2CppScheduleOne.Tiles.FootprintTile>();
                if (newTile == null || newTile.Pointer == IntPtr.Zero) continue;
                newTile.X = x;
                newTile.Y = y;

                var pair = new Il2CppScheduleOne.Tiles.CoordinateFootprintTilePair();
                pair.coord = new Il2CppScheduleOne.Tiles.Coordinate(x, y);
                pair.footprintTile = newTile;
                pairs.Add(pair);
            }

            Mod.Log.Info($"ExpandFootprint: Expanded to {pairs.Count} footprint tiles (2x2).");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"ExpandFootprintTo2x2 failed (non-fatal, 1x1 fallback): {ex.Message}");
        }
    }

    /// <summary>
    /// Spawns a new station in the world at the given position and rotation.
    /// </summary>
    public static GameObject? SpawnStationAt(Vector3 position, Quaternion rotation, AutoPackStationSaveData? data = null)
    {
        try
        {
            var go = new GameObject("AutoPackagingStation_Placed");
            go.transform.position = position;
            go.transform.rotation = rotation;

            SetupPlacedStation(go, data);
            return go;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"SpawnStationAt error: {ex}");
            return null;
        }
    }
}
