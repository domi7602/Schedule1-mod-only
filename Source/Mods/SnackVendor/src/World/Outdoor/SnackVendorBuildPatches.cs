using System;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.Building;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Tiles;
using S1Mods.Shared;
using SnackVendor.Items;
using UnityEngine;
using NativeBuildableItemDef = Il2CppScheduleOne.ItemFramework.BuildableItemDefinition;

namespace SnackVendor.World.Outdoor;

/// <summary>
/// Harmony patches on BuildUpdate_Grid enabling standalone outdoor placement strictly
/// for the SnackVendor machine outside purchased properties.
/// </summary>
public static class SnackVendorBuildPatches
{
    public static bool IsOutdoorPlacementValid = false;
    private static Vector3 _lastValidPos;
    private static Quaternion _lastValidRot;

    public static class BuildUpdate_Grid_CheckIntersections_Patch
    {
        public static void Postfix(BuildUpdate_Grid __instance)
        {
            try
            {
                if (!Mod.CurrentConfig.EnableStandaloneOutdoorPlacement)
                {
                    IsOutdoorPlacementValid = false;
                    return;
                }

                if (__instance == null || __instance.Pointer == IntPtr.Zero)
                {
                    IsOutdoorPlacementValid = false;
                    return;
                }

                var itemInstance = __instance.ItemInstance;
                if (itemInstance == null || itemInstance.Definition == null)
                {
                    IsOutdoorPlacementValid = false;
                    return;
                }

                // STRICT GATE: ONLY the SnackVendor machine is allowed outdoor building!
                string itemId = itemInstance.Definition.ID ?? string.Empty;
                if (!itemId.Equals(Mod.CurrentConfig.StationItemId, StringComparison.OrdinalIgnoreCase))
                {
                    IsOutdoorPlacementValid = false;
                    return;
                }

                // If vanilla already found a valid position on a purchased property grid, let vanilla handle it
                if (__instance._validPosition)
                {
                    IsOutdoorPlacementValid = false;
                    return;
                }

                // If HomelessMod is installed and actively handling placement, let HomelessMod do it
                if (HomelessModInterop.IsHomelessModHandlingPlacement())
                {
                    IsOutdoorPlacementValid = false;
                    return;
                }

                var ghost = __instance.GhostModel;
                if (ghost == null || ghost.Pointer == IntPtr.Zero)
                {
                    IsOutdoorPlacementValid = false;
                    return;
                }

                var buildMgr = NetworkSingleton<BuildManager>.Instance;
                if (buildMgr == null || buildMgr.Pointer == IntPtr.Zero)
                {
                    IsOutdoorPlacementValid = false;
                    return;
                }

                // Resolve player camera
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

                if (camTransform == null)
                {
                    IsOutdoorPlacementValid = false;
                    return;
                }

                Ray ray = new Ray(camTransform.position, camTransform.forward);
                int rayMask = SnackVendorGroundAssistant.BuildRayMask();
                bool hitSuccess = Physics.Raycast(ray, out RaycastHit hit, 10.0f, rayMask, QueryTriggerInteraction.Ignore);

                if (!hitSuccess)
                {
                    Vector3 fallbackStart = camTransform.position + new Vector3(camTransform.forward.x, 0f, camTransform.forward.z).normalized * 2f;
                    fallbackStart.y += 1.5f;
                    hitSuccess = Physics.Raycast(fallbackStart, Vector3.down, out hit, 12.0f, rayMask, QueryTriggerInteraction.Ignore);
                }

                if (hitSuccess)
                {
                    float rotationAngle = ghost.transform.rotation.eulerAngles.y;
                    var eval = SnackVendorGroundAssistant.EvaluatePlacement(
                        hit.point,
                        hit.normal,
                        rotationAngle,
                        ghost,
                        camTransform.position);

                    if (eval.IsValid)
                    {
                        ghost.transform.position = eval.Position;
                        ghost.transform.rotation = eval.Rotation;

                        _lastValidPos = eval.Position;
                        _lastValidRot = eval.Rotation;
                        IsOutdoorPlacementValid = true;

                        if (buildMgr.ghostMaterial_White != null)
                        {
                            __instance._currentGhostMaterial = buildMgr.ghostMaterial_White;
                            buildMgr.ApplyMaterial(ghost, buildMgr.ghostMaterial_White, true);
                        }
                        return;
                    }
                }

                // If not valid ground spot:
                IsOutdoorPlacementValid = false;
                if (buildMgr.ghostMaterial_Red != null)
                {
                    __instance._currentGhostMaterial = buildMgr.ghostMaterial_Red;
                    buildMgr.ApplyMaterial(ghost, buildMgr.ghostMaterial_Red, true);
                }
            }
            catch (Exception ex)
            {
                IsOutdoorPlacementValid = false;
                Mod.Log.Debug($"CheckIntersections_Postfix error: {ex.Message}");
            }
        }
    }

    public static class BuildUpdate_Grid_Place_Patch
    {
        public static bool Prefix(BuildUpdate_Grid __instance, ref GridItem __result)
        {
            try
            {
                if (!Mod.CurrentConfig.EnableStandaloneOutdoorPlacement) return true;
                if (__instance == null || __instance.Pointer == IntPtr.Zero) return true;

                var itemInstance = __instance.ItemInstance;
                if (itemInstance == null || itemInstance.Definition == null) return true;

                string itemId = itemInstance.Definition.ID ?? string.Empty;
                if (!itemId.Equals(Mod.CurrentConfig.StationItemId, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                // If our standalone outdoor placement was not validated, let vanilla handle/reject it
                if (!IsOutdoorPlacementValid)
                {
                    return true;
                }

                // If HomelessMod is present and handled placement, pass through to HomelessMod
                if (HomelessModInterop.IsHomelessModHandlingPlacement())
                {
                    return true;
                }

                if (!NetworkGuard.IsHostOrSingleplayer())
                {
                    IsOutdoorPlacementValid = false;
                    return true;
                }

                var bDef = itemInstance.Definition.TryCast<NativeBuildableItemDef>();
                if (bDef == null || bDef.BuiltItem == null)
                {
                    Mod.Log.Warn("Place_Patch: BuiltItem definition was null.");
                    IsOutdoorPlacementValid = false;
                    return true;
                }

                string itemGuid = Guid.NewGuid().ToString();
                Vector3 spawnPos = _lastValidPos;
                Quaternion spawnRot = _lastValidRot;

                bool wasActive = bDef.BuiltItem.gameObject.activeSelf;
                GameObject placedObj;
                try
                {
                    if (wasActive) bDef.BuiltItem.gameObject.SetActive(false);
                    placedObj = GameObject.Instantiate(bDef.BuiltItem.gameObject, spawnPos, spawnRot);
                }
                finally
                {
                    if (wasActive) bDef.BuiltItem.gameObject.SetActive(true);
                }

                placedObj.transform.SetParent(SnackVendorOutdoorManager.OutdoorRoot.transform, true);

                var buildMgr = NetworkSingleton<BuildManager>.Instance;
                if (buildMgr != null && buildMgr.Pointer != IntPtr.Zero)
                {
                    buildMgr.DisableNetworking(placedObj);
                    buildMgr.DisableNavigation(placedObj);
                }

                // Attach interactable
                var interactable = placedObj.AddComponent<SnackVendorOutdoorInteractable>();
                interactable.Guid = itemGuid;

                // Wire up controller
                SnackVendorItemFactory.SetupPlacedStation(placedObj, itemGuid);

                // Register with outdoor manager
                SnackVendorOutdoorManager.RegisterOutdoorStation(placedObj, itemGuid);

                // Deduct 1x item from player inventory
                if (PlayerInventory.Instance != null && PlayerInventory.Instance.Pointer != IntPtr.Zero)
                {
                    PlayerInventory.Instance.RemoveAmountOfItem(itemId, 1u);
                }

                // Play build sound
                if (buildMgr != null && buildMgr.Pointer != IntPtr.Zero)
                {
                    buildMgr.PlayBuildSound(bDef.BuildSoundType, spawnPos);
                }

                placedObj.SetActive(true);
                __instance.Stop();
                __result = null!;
                IsOutdoorPlacementValid = false;

                Mod.Log.Info($"Successfully placed standalone outdoor SnackVendor at {spawnPos}");
                return false; // Skip vanilla Place
            }
            catch (Exception ex)
            {
                Mod.Log.Error("BuildUpdate_Grid_Place_Patch failed", ex);
                IsOutdoorPlacementValid = false;
                return true;
            }
        }
    }
}
