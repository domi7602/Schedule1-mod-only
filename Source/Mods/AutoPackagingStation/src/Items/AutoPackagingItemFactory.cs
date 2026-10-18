using System;
using System.Collections;
using System.Reflection;
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
    private static NativeBuildableItemDef? _preparedDefinition;
    private static BuildableItem? _preparedPrefab;
    private static bool _ghostPrepared;
    private static int _injectRetryCount = 0;
    private const int MaxInjectRetries = 10;

    /// <summary>
    /// Critic Pillar 1: Clones 4x4 base entity via S1API BuildableItemCreator.CloneFrom("cauldron") / "packagingstation".
    /// </summary>
    public static void RegisterItem()
    {
        string itemId = Mod.CurrentConfig.StationItemId;
        try
        {
            if (GameRegistry.ItemExists(itemId))
            {
                var existing = GameRegistry.GetItem(itemId)?.TryCast<NativeBuildableItemDef>();
                if (!IsReadyDefinition(existing))
                    Mod.Log.Error($"Registration refused: item '{itemId}' exists but is not our fully prepared definition/prefab.");
                return;
            }

            Mod.Log.Info($"Registering custom buildable 4x4 item '{itemId}'...");

            // Preserve the serialized PackagingStation/FishNet graph. A foreign machine
            // plus a late AddComponent has no registered network behaviour or native slots.
            string[] candidateBases = new[] { "packagingstation", "packagingstationmk2" };
            string? baseId = null;
            foreach (var candidate in candidateBases)
            {
                if (GameRegistry.ItemExists(candidate))
                {
                    var def = GameRegistry.GetItem(candidate);
                    var native = def != null && def.Pointer != IntPtr.Zero ? def.TryCast<NativeBuildableItemDef>() : null;
                    var prefab = native?.BuiltItem;
                    var nativeStation = prefab != null && prefab.Pointer != IntPtr.Zero
                        ? prefab.GetComponent<Il2CppScheduleOne.ObjectScripts.PackagingStation>() : null;
                    if (nativeStation != null && nativeStation.Pointer != IntPtr.Zero)
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
                Mod.Log.Warn("Registration deferred: no native PackagingStation donor is available.");
                return;
            }

            builder.WithBasicInfo(
                       itemId,
                       Mod.CurrentConfig.StationDisplayName,
                       "Industrial automated packaging station. Automatically feeds raw product through an animated pneumatic conveyor system. Freshness bonus: each packaging cycle has a small chance (configurable, default 5%) to upgrade the output quality by one tier.",
                       S1API.Items.ItemCategory.Furniture)
                   .WithPricing(Mod.CurrentConfig.StationPurchasePrice, 0.5f)
                   .WithIcon(AutoPackMeshBuilder.GetOrCreateIconSprite());

            // Bug 3 Fix: Level-gate — Tier 3 packaging machine requires Hustler rank
            // (after Tier 1 "packagingstation" StreetRat, Tier 2 "packagingstationmk2" Peddler)
            builder.WithRequiredRank(new S1API.Leveling.FullRank(S1API.Leveling.Rank.Hustler, 1));

            var packagingDefinition = GameRegistry.GetItem(baseId)?.TryCast<NativeBuildableItemDef>();
            var cauldronDefinition = GameRegistry.GetItem("cauldron")?.TryCast<NativeBuildableItemDef>();
            if (packagingDefinition == null || cauldronDefinition == null)
            {
                Mod.Log.Warn("Registration deferred: packaging and cauldron donors are both required.");
                return;
            }
            // Compose before publishing any definition/ghost, without touching either donor.
            var ownedPrefab = AutoPackPrefabComposer.Compose(packagingDefinition, cauldronDefinition);
            // Narrow S1API 3.2.1 compatibility bridge: its public builder cannot
            // assign BuiltItem, but the verified private getter exposes the unpublished
            // definition. Missing/drifted signatures abort BEFORE publication.
            var nativeDefinition = typeof(BuildableItemDefinitionBuilder)
                .GetProperty("BuildableDefinition", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(builder) as NativeBuildableItemDef
                ?? throw new InvalidOperationException("S1API buildable-definition bridge is unavailable.");
            nativeDefinition.BuiltItem = ownedPrefab;
            _preparedDefinition = nativeDefinition;
            _preparedPrefab = ownedPrefab;
            _ghostPrepared = false;
            RegisterPreparedGhost(itemId);
            _ghostPrepared = true;
            // Even if Build throws AFTER native publication, the only published
            // definition already has its owned prefab AND ghost. A retry accepts
            // exact prepared identity only, not the mere presence of an item ID.
            builder.Build();
            if (!IsReadyDefinition(GameRegistry.GetItem(itemId)?.TryCast<NativeBuildableItemDef>()))
                throw new InvalidOperationException("Published definition differs from the prepared definition.");

            Mod.Log.Info($"Successfully registered '{itemId}' (native '{baseId}' behavior + cauldron 4x4 footprint, Hustler I rank-gated).");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to register AutoPackagingStation item: {ex}");
        }
    }

    private static bool IsReadyDefinition(NativeBuildableItemDef? definition)
    {
        var prefab = definition?.BuiltItem;
        return RegistrationPolicy.CanReuse(
            definition != null && _preparedDefinition != null && definition.Pointer == _preparedDefinition.Pointer,
            prefab != null && _preparedPrefab != null && prefab.Pointer == _preparedPrefab.Pointer,
            _ghostPrepared);
    }

    private static void RegisterPreparedGhost(string itemId)
    {
        var runtime = typeof(BuildableItemDefinitionBuilder).Assembly
            .GetType("S1API.Internal.Building.BuildableGhostRuntime");
        var register = runtime?.GetMethod("RegisterVisual", BindingFlags.Static | BindingFlags.NonPublic,
            null, new[] { typeof(string), typeof(Func<Transform, GameObject>), typeof(bool) }, null)
            ?? throw new InvalidOperationException("S1API ghost-registration bridge is unavailable.");
        Func<Transform, GameObject> factory = parent =>
        {
            var ghost = CreateGhostPrefab();
            ghost.transform.SetParent(parent, false);
            ghost.name = "AutoPackagingStation_Ghost";
            return ghost;
        };
        register.Invoke(null, new object[] { itemId, factory, false });
    }

    public static void ShutdownPrefabCache()
    {
        _ghostPrepared = false;
        _preparedDefinition = null;
        _preparedPrefab = null;
        AutoPackPrefabComposer.Shutdown();
    }

    public static void OnSaveInfoLoaded()
    {
        // Fresh load: re-arm the bounded shop-injection retry budget (bug-audit 2026-10-13, BUG-017).
        _injectRetryCount = 0;
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
            if (def == null || def.Pointer == IntPtr.Zero || !IsReadyDefinition(def.TryCast<NativeBuildableItemDef>())) return;

            var storable = def.TryCast<NativeStorableItemDef>();
            if (storable == null || storable.Pointer == IntPtr.Zero) return;

            var shops = ShopInterface.AllShops;
            if (shops == null || shops.Count == 0)
            {
                ScheduleShopRetry("shop list empty");
                return;
            }

            bool targetShopSeen = false;

            for (int i = 0; i < shops.Count; i++)
            {
                var shop = shops[i];
                if (shop == null || shop.Pointer == IntPtr.Zero) continue;

                string code = (shop.ShopCode ?? string.Empty).ToLowerInvariant();
                string name = (shop.ShopName ?? string.Empty).ToLowerInvariant();

                if (!code.Contains("hardware") && !name.Contains("hardware") && !code.Contains("dan") && !name.Contains("dan") && !code.Contains("hank") && !name.Contains("hank")) continue;
                targetShopSeen = true;

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

            // Bug-audit 2026-10-13 (BUG-017): success is per TARGET shop — retry until at
            // least one hardware shop was processed, not merely until any shop exists.
            if (targetShopSeen)
            {
                _injectRetryCount = MaxInjectRetries;
            }
            else
            {
                ScheduleShopRetry("no target hardware shop found yet");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Failed to inject hardware store listing: {ex.Message}");
        }
    }

    private static void ScheduleShopRetry(string reason)
    {
        if (_injectRetryCount >= MaxInjectRetries) return;
        _injectRetryCount++;
        Mod.Log.Debug($"InjectHardwareStoreListing: {reason} — retry {_injectRetryCount}/{MaxInjectRetries} in 2s.");
        MelonCoroutines.Start(DelayedShopRetry());
    }

    private static IEnumerator DelayedShopRetry()
    {
        yield return new WaitForSeconds(2.0f);
        InjectHardwareStoreListing();
    }

    /// <summary>
    /// Creates a visual ghost prefab for placement preview.
    /// 2026-10-14: Baut NUR die Mod-Visuals (kein Controller, keine Logik). Der
    /// Native-Ghost ist die Instanz unseres isolierten BuiltItem-Prefabs — S1API hängt
    /// diese Visuals per WithGhostVisual darunter (replaceExistingVisual:false, damit
    /// die GridItem-Footprint-Anzeige sichtbar bleibt). KEIN BoxCollider hier: Der
    /// Native-Ghost bringt seine eigenen Collider/Tasks mit — ein zweiter Collider auf
    /// unserem Visual-Root hat Raycast/Platzierung gestört.
    /// </summary>
    public static GameObject CreateGhostPrefab()
    {
        var root = new GameObject("AutoPackagingStation_GhostRoot");

        // Build visuals WITHOUT a controller (bug-audit 2026-10-13, BUG-013): the old
        // flow attached a full AutoPackStationController (Awake side effects: GUID,
        // AudioSource) and destroyed it again — its OnDestroy destroyed the instanced
        // conveyor/LED materials the ghost renderers still used (pink/missing materials).
        // A ghost needs visuals only: no runtime data, no game logic, no material teardown.
        AutoPackMeshBuilder.BuildStationVisuals(root, null);

        return root;
    }

    /// <summary>
    /// Configures placed visuals/controller while retaining native colliders and interaction.
    /// 2026-10-14 (Platzierungs-Ghost fehlt + falsches Objekt beim Platzieren): Das vom
    /// Spiel übergebene Objekt ist die Instanz unseres isolierten BuiltItem-Prefabs —
    /// das vormals vermisste Setup (kein Controller, keine Mod-Visuals, keine
    /// Interaktion) lief, weil BuildableItem.Start für UNSERE Item-ID ggf. vor oder
    /// ohne Mod-Registrierung feuerte. SetupPlacedStation verifiziert jetzt die
    /// Item-ID, versteckt das Vanilla-Erbe und meldet Erfolg/Misserfolg ins Log.
    /// </summary>
    public static void SetupPlacedStation(GameObject go, AutoPackStationSaveData? data = null)
    {
        if (go == null || go.Pointer == IntPtr.Zero) return;

        try
        {
            // 0. Entry-Diagnose (2026-10-14): vorher war unklar, ob der Hook feuert.
            string entryName = string.Empty;
            try { entryName = go.name ?? string.Empty; } catch { }
            Mod.Log.Info($"SetupPlacedStation entry: object='{entryName}'.");

            // 0. Verify this GameObject really is OUR station (Item-ID check) — sonst
            // richten wir versehentlich eine Vanilla-Station als Mod-Station ein.
            bool isOurs = false;
            try
            {
                var buildableCheck = go.GetComponent<Il2CppScheduleOne.EntityFramework.BuildableItem>();
                var instCheck = buildableCheck != null && buildableCheck.Pointer != IntPtr.Zero ? buildableCheck.ItemInstance : null;
                string idCheck = instCheck != null && instCheck.Pointer != IntPtr.Zero && instCheck.Definition != null
                    ? instCheck.Definition.ID ?? string.Empty : string.Empty;
                isOurs = string.Equals(idCheck, Mod.CurrentConfig.StationItemId, StringComparison.OrdinalIgnoreCase)
                    && instCheck != null && IsReadyDefinition(instCheck.Definition?.TryCast<NativeBuildableItemDef>());
                if (!isOurs)
                {
                    Mod.Log.Debug($"SetupPlacedStation skipped: GameObject '{go.name}' carries item '{idCheck}', not '{Mod.CurrentConfig.StationItemId}'.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"SetupPlacedStation ID/ownership check failed: {ex.Message}");
                return;
            }

            // Collider A/B: keep every inherited native collider unchanged. The previous
            // full-model root collider could intercept selection before the child IntObj.
            // No replacement collider or custom E-input path is introduced here.

            // Do not add a NetworkBehaviour after spawn: keep the inherited native graph.
            var station = go.GetComponent<Il2CppScheduleOne.ObjectScripts.PackagingStation>();
            if (station == null || station.Pointer == IntPtr.Zero)
            {
                Mod.Log.Error($"Setup refused for '{entryName}': native PackagingStation is missing. No runtime NetworkBehaviour was added.");
                return;
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

            // Native PackagingStation.Awake owns its hover/start listeners. Do not add
            // another anonymous listener on both BuildEvents and Start (double invocation).
            // The native station's interactable may live on a child, not the root.
            // Keep its existing wiring and require ownership before changing its range.
            var interactable = station?.IntObj;
            if (interactable == null || interactable.Pointer == IntPtr.Zero)
                interactable = go.GetComponentInChildren<Il2CppScheduleOne.Interaction.InteractableObject>(true);
            if (interactable == null || interactable.Pointer == IntPtr.Zero || !interactable.transform.IsChildOf(go.transform))
            {
                Mod.Log.Error("Setup refused: native PackagingStation interaction is missing or references a foreign object.");
                return;
            }
            Mod.Log.Info($"[autopack-setup] native interaction resolved on '{interactable.gameObject.name}'.");
            interactable.MaxInteractionRange = Mod.CurrentConfig.InteractionRange;

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
            Mod.Log.Info($"Configured AutoPackagingStation [{controller.StationGuid}] in world (controller + visuals + native interaction).");
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"SetupPlacedStation error: {ex}");
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
