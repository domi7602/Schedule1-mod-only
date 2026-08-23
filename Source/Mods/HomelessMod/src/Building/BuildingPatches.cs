using System;
using System.Linq;
using HarmonyLib;
using HomelessMod.Items;
using HomelessMod.Quests;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.Building;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Tiles;
using S1Mods.Shared;
using UnityEngine;

namespace HomelessMod.Building;

/// <summary>
/// Harmony patches enabling unrestricted placement of items anywhere in the open game world with accurate ghost coloring and safe outdoor placement.
/// </summary>
public static class BuildingPatches
{
    private static readonly int GroundLayerMask = LayerMask.GetMask("Default", "Terrain", "Building", "Props");

    public static bool IsCustomPlacementValid = false;
    public static string LastInvalidReason = string.Empty;

    // Patches are applied explicitly via PatchGuard.TryPatch in Mod.ApplyHarmonyPatches.
    public static class BuildUpdate_Grid_CheckIntersections_Patch
    {
        public static void Postfix(BuildUpdate_Grid __instance)
        {
            try
            {
                IsCustomPlacementValid = false;
                if (!Mod.CurrentConfig.EnableEverywhereBuilding) return;
                if (__instance == null || __instance.Pointer == IntPtr.Zero) return;

                var ghost = __instance.GhostModel;
                if (ghost == null || ghost.Pointer == IntPtr.Zero) return;

                var buildMgr = NetworkSingleton<BuildManager>.Instance;
                if (buildMgr == null || buildMgr.Pointer == IntPtr.Zero) return;

                // If vanilla already found a valid position on a purchased property grid:
                if (__instance._validPosition)
                {
                    if (buildMgr.ghostMaterial_White != null)
                    {
                        __instance._currentGhostMaterial = buildMgr.ghostMaterial_White;
                        buildMgr.ApplyMaterial(ghost, buildMgr.ghostMaterial_White, true);
                    }
                    return;
                }

                // Resolve player camera safely
                Transform? camTransform = null;
                try
                {
                    var playerCam = PlayerSingleton<PlayerCamera>.Instance;
                    if (playerCam != null && playerCam.Pointer != IntPtr.Zero && playerCam.Camera != null)
                    {
                        camTransform = playerCam.Camera.transform;
                    }
                }
                catch { }

                if (camTransform == null && Camera.main != null)
                {
                    camTransform = Camera.main.transform;
                }

                if (camTransform == null) return;

                // Raycast along camera forward
                Ray ray = new Ray(camTransform.position, camTransform.forward);

                // Use a longer range for the raycast so we find ground even if it's slightly out of reach.
                // Our EvaluatePlacement will handle rejecting it if it's too far.
                float maxDist = 10.0f;

                // We use all layers (~0) and ignore triggers to ensure we hit the street, regardless of its specific layer
                // We must NOT exclude "Grid" because some street meshes might actually be on the Grid layer!
                int rayMask = ~LayerMask.GetMask("Ignore Raycast", "Player");
                bool hitSuccess = Physics.Raycast(ray, out RaycastHit hit, maxDist, rayMask, QueryTriggerInteraction.Ignore);

                if (!hitSuccess)
                {
                    // Fallback: Raycast straight down from a point 2 meters in front of the camera
                    // This catches cases where the player aims slightly too high, or hits a crack in the mesh.
                    Vector3 fallbackStart = camTransform.position + new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized * 2f;
                    fallbackStart.y += 1.5f; // Start a bit above the camera
                    hitSuccess = Physics.Raycast(fallbackStart, Vector3.down, out hit, maxDist + 2f, rayMask, QueryTriggerInteraction.Ignore);
                }

                if (hitSuccess)
                {
                    // Calculate accurate local bounding box by temporarily resetting rotation
                    Quaternion originalRot = ghost.transform.rotation;
                    ghost.transform.rotation = Quaternion.identity;
                    Physics.SyncTransforms();

                    Vector3 boxExtents = new Vector3(0.40f, 0.35f, 0.40f);
                    float bottomOffset = 0f;
                    try
                    {
                        var allCols = ghost.GetComponentsInChildren<Collider>(true);
                        if (allCols != null && allCols.Count > 0)
                        {
                            float minLocalY = float.MaxValue;
                            Vector3 maxExtents = Vector3.zero;

                            foreach (var col in allCols)
                            {
                                if (col == null || col.Pointer == IntPtr.Zero || col.WasCollected || col.gameObject == null || col.gameObject.Pointer == IntPtr.Zero) continue;

                                float minY = col.bounds.min.y;
                                if (minY < minLocalY)
                                {
                                    minLocalY = minY;
                                }
                                maxExtents = Vector3.Max(maxExtents, col.bounds.extents);
                            }

                            if (minLocalY != float.MaxValue)
                            {
                                boxExtents = Vector3.Max(maxExtents, new Vector3(0.15f, 0.15f, 0.15f));
                                bottomOffset = ghost.transform.position.y - minLocalY;
                                if (bottomOffset < 0f) bottomOffset = 0f;
                            }
                        }
                    }
                    catch { }

                    ghost.transform.rotation = originalRot;

                    float vOffset = 0f;
                    try { vOffset = __instance.verticalOffset; } catch { }

                    // Automatic pivot correction: prevents items from sinking into the ground if their pivot is centered
                    vOffset += bottomOffset;

                    bool isFreePlacement = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

                    // 5-Point Terrain Sampling & Smart Placement Evaluation
                    var result = GroundPlacementAssistant.EvaluatePlacement(
                        hit.point,
                        hit.normal,
                        __instance._rotation,
                        boxExtents,
                        vOffset,
                        Mod.CurrentConfig.OutdoorGridSnapSize,
                        isFreePlacement,
                        ghost,
                        camTransform.position,
                        rayMask);

                    ghost.transform.position = result.Position;
                    ghost.transform.rotation = result.Rotation;
                    LastInvalidReason = result.InvalidReason;

                    if (result.IsValid)
                    {
                        // VALID -> GREEN / WHITE
                        IsCustomPlacementValid = true;
                        __instance._validPosition = true;
                        if (buildMgr.ghostMaterial_White != null)
                        {
                            __instance._currentGhostMaterial = buildMgr.ghostMaterial_White;
                            buildMgr.ApplyMaterial(ghost, buildMgr.ghostMaterial_White, true);
                        }
                    }
                    else
                    {
                        // INVALID -> RED
                        __instance._validPosition = false;
                        if (buildMgr.ghostMaterial_Red != null)
                        {
                            __instance._currentGhostMaterial = buildMgr.ghostMaterial_Red;
                            buildMgr.ApplyMaterial(ghost, buildMgr.ghostMaterial_Red, true);
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            Mod.Log.Info($"[Placement Failed] {LastInvalidReason}");
                        }
                    }
                }
                else
                {
                    // Looking into sky / no hit -> RED
                    __instance._validPosition = false;
                    LastInvalidReason = "No ground detected";
                    if (buildMgr.ghostMaterial_Red != null)
                    {
                        __instance._currentGhostMaterial = buildMgr.ghostMaterial_Red;
                        buildMgr.ApplyMaterial(ghost, buildMgr.ghostMaterial_Red, true);
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        Mod.Log.Info($"[Placement Failed] {LastInvalidReason}");
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"CheckIntersections patch debug: {ex.ToString()}");
            }
        }
    }

    public static class BuildUpdate_Grid_Place_Patch
    {
        public static bool Prefix(BuildUpdate_Grid __instance, ref GridItem __result)
        {
            try
            {
                if (__instance == null || __instance.Pointer == IntPtr.Zero) return true;

                // If custom placement is NOT valid, let vanilla handle it (or reject it)
                if (!IsCustomPlacementValid)
                {
                    return true;
                }

                var ghost = __instance.GhostModel;
                if (ghost == null || ghost.Pointer == IntPtr.Zero) return false;

                var itemInstance = __instance.ItemInstance;
                if (itemInstance == null || itemInstance.Definition == null) return false;

                string itemId = itemInstance.Definition.ID;
                Vector3 spawnPos = ghost.transform.position;
                Quaternion spawnRot = ghost.transform.rotation;

                GameObject placedObj = null;
                string itemGuid = System.Guid.NewGuid().ToString();
                bool isCustomStation = itemId.Equals("autopackagingstation", StringComparison.OrdinalIgnoreCase);

                if (itemId.Equals(Mod.CurrentConfig.SleepingBagItemId, StringComparison.OrdinalIgnoreCase))
                {
                    placedObj = SleepingBagItemFactory.CreateSleepingBagPrefab();
                    placedObj.transform.position = spawnPos;
                    placedObj.transform.rotation = spawnRot;
                    placedObj.SetActive(true);
                    SleepingBagItemFactory.SetupPlacedSleepingBag(placedObj);
                    var comp = placedObj.GetComponent<SleepingBagInteractable>();
                    if (comp != null) comp.Guid = itemGuid;
                }
                else
                {
                    var bDef = itemInstance.Definition.TryCast<BuildableItemDefinition>();
                    if (bDef != null && bDef.BuiltItem != null)
                    {
                        bool wasActive = bDef.BuiltItem.gameObject.activeSelf;
                        if (wasActive) bDef.BuiltItem.gameObject.SetActive(false);

                        placedObj = GameObject.Instantiate(bDef.BuiltItem.gameObject, spawnPos, spawnRot);

                        if (wasActive) bDef.BuiltItem.gameObject.SetActive(true);

                        placedObj.transform.SetParent(StreetPropertyManager.StreetRoot.transform, true);

                        var buildMgrLocal = NetworkSingleton<BuildManager>.Instance;
                        if (buildMgrLocal != null && buildMgrLocal.Pointer != IntPtr.Zero)
                        {
                            buildMgrLocal.DisableNetworking(placedObj);
                            buildMgrLocal.DisableNavigation(placedObj);
                        }

                        if (!isCustomStation && placedObj.GetComponent<OutdoorItemInteractable>() == null)
                        {
                            var interactable = placedObj.AddComponent<OutdoorItemInteractable>();
                            interactable.ItemId = itemId;
                        }

                        if (isCustomStation)
                        {
                            var factoryType = TypeResolver.Find("AutoPackagingStation.Items.AutoPackagingItemFactory", "AutoPackagingStation");
                            var setupMethod = factoryType?.GetMethods().FirstOrDefault(m => m.Name == "SetupPlacedStation");
                            setupMethod?.Invoke(null, new object?[] { placedObj, null });

                            var controllerType = TypeResolver.Find("AutoPackagingStation.Entities.AutoPackStationController", "AutoPackagingStation");
                            if (controllerType != null)
                            {
                                var ctrlComp = placedObj.GetComponent(Il2CppType.From(controllerType));
                                if (ctrlComp != null)
                                {
                                    var prop = controllerType.GetProperty("StationGuid");
                                    prop?.SetValue(ctrlComp, itemGuid);
                                }
                            }
                        }

                        var footprintTiles = placedObj.GetComponentsInChildren<FootprintTile>(true);
                        foreach (var ft in footprintTiles)
                        {
                            if (ft == null || ft.Pointer == IntPtr.Zero || ft.WasCollected) continue;
                            if (ft.gameObject == null || ft.gameObject.Pointer == IntPtr.Zero) continue;
                            ft.gameObject.SetActive(false);
                        }

                        var gridItems = placedObj.GetComponentsInChildren<GridItem>(true);
                        foreach (var gi in gridItems)
                        {
                            if (gi == null || gi.Pointer == IntPtr.Zero || gi.WasCollected) continue;
                            try { gi.SetFootprintTileVisiblity(false); } catch { }
                            gi.enabled = false;
                        }

                        var buildables = placedObj.GetComponentsInChildren<BuildableItem>(true);
                        foreach (var b in buildables)
                        {
                            if (b == null || b.Pointer == IntPtr.Zero || b.WasCollected) continue;
                            b.enabled = false;
                        }

                        if (!isCustomStation)
                        {
                            var renderers = placedObj.GetComponentsInChildren<Renderer>(true);
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

                        var colliders = placedObj.GetComponentsInChildren<Collider>(true);
                        foreach (var c in colliders)
                        {
                            if (c == null || c.Pointer == IntPtr.Zero || c.WasCollected) continue;
                            c.enabled = true;
                        }

                        // Ensure rigidbodies don't cause the item to fall through the street
                        var rbs = placedObj.GetComponentsInChildren<Rigidbody>(true);
                        foreach (var rb in rbs)
                        {
                            if (rb == null || rb.Pointer == IntPtr.Zero || rb.WasCollected) continue;
                            rb.isKinematic = true;
                        }

                        placedObj.SetActive(true);
                    }
                }

                if (placedObj != null)
                {
                    StreetPropertyManager.RegisterStreetItem(placedObj, itemId, itemGuid);
                    Mod.Log.Info($"Successfully placed outdoor item '{itemId}' at {spawnPos}");
                    __result = null; // Prevent vanilla from tracking/destroying it

                    if (PlayerInventory.Instance != null && PlayerInventory.Instance.Pointer != IntPtr.Zero)
                    {
                        PlayerInventory.Instance.RemoveAmountOfItem(itemId, 1u);
                    }

                    // Play build sound
                    var buildMgr = NetworkSingleton<BuildManager>.Instance;
                    if (buildMgr != null && buildMgr.Pointer != IntPtr.Zero)
                    {
                        var bDef = itemInstance.Definition.TryCast<BuildableItemDefinition>();
                        if (bDef != null)
                        {
                            buildMgr.PlayBuildSound(bDef.BuildSoundType, spawnPos);
                        }
                    }

                    // Notify Quest system
                    HomelessQuestManager.NotifyItemPlaced(itemId);

                    // Exit/Stop current build mode session
                    __instance.Stop();

                    IsCustomPlacementValid = false;
                    return false; // Skip crashing native Place()
                }
                else
                {
                    Mod.Log.Warn($"Failed to instantiate outdoor item for '{itemId}'");
                    IsCustomPlacementValid = false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                IsCustomPlacementValid = false;
                Mod.Log.Error($"BuildUpdate_Grid.Place prefix error: {ex}");
                return false;
            }
        }
    }

    public static class GridItem_Destroy_Patch
    {
        public static bool Prefix(GridItem __instance)
        {
            try
            {
                if (__instance != null && __instance.Pointer != IntPtr.Zero && !__instance.WasCollected && __instance.gameObject != null && __instance.gameObject.Pointer != IntPtr.Zero)
                {
                    if (StreetPropertyManager.IsOutdoorItem(__instance.gameObject))
                    {
                        Mod.Log.Debug($"Prevented vanilla destruction of outdoor GridItem: {__instance.gameObject.name}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"GridItem_Destroy_Patch error: {ex.Message}");
            }
            return true;
        }
    }

    public static class BuildableItem_Destroy_Patch
    {
        public static bool Prefix(BuildableItem __instance)
        {
            try
            {
                if (__instance != null && __instance.Pointer != IntPtr.Zero && !__instance.WasCollected && __instance.gameObject != null && __instance.gameObject.Pointer != IntPtr.Zero)
                {
                    if (StreetPropertyManager.IsOutdoorItem(__instance.gameObject))
                    {
                        Mod.Log.Debug($"Prevented vanilla destruction of outdoor BuildableItem: {__instance.gameObject.name}");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"BuildableItem_Destroy_Patch error: {ex.Message}");
            }
            return true;
        }
    }

    public static class BuildableItem_SetCulled_Patch
    {
        public static bool Prefix(BuildableItem __instance, bool culled)
        {
            try
            {
                if (__instance != null && __instance.Pointer != IntPtr.Zero && !__instance.WasCollected && __instance.gameObject != null && __instance.gameObject.Pointer != IntPtr.Zero)
                {
                    if (StreetPropertyManager.IsOutdoorItem(__instance.gameObject))
                    {
                        // Outdoor street items must never be culled
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"BuildableItem_SetCulled_Patch error: {ex.Message}");
            }
            return true;
        }
    }

    public static class BuildableItem_Start_Patch
    {
        public static bool Prefix(BuildableItem __instance)
        {
            try
            {
                if (__instance != null && __instance.Pointer != IntPtr.Zero && !__instance.WasCollected && __instance.gameObject != null && __instance.gameObject.Pointer != IntPtr.Zero)
                {
                    if (StreetPropertyManager.IsOutdoorItem(__instance.gameObject))
                    {
                        Mod.Log.Debug($"Bypassed BuildableItem.Start for outdoor item '{__instance.gameObject.name}'");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Debug($"BuildableItem_Start_Patch error: {ex.Message}");
            }
            return true;
        }
    }
}
