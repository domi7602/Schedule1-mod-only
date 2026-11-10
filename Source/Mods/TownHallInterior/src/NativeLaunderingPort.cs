using System;
using System.Collections.Generic;
using HarmonyLib;
using Il2CppFishNet.Object;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.Interaction;
using Il2CppScheduleOne.ObjectScripts;
using Il2CppScheduleOne.ObjectScripts.Cash;
using Il2CppScheduleOne.Property;
using Il2CppScheduleOne.State;
using Il2CppScheduleOne.UI;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;

namespace TownHallInterior;

/// <summary>
/// Native engine implementation of <see cref="ILaunderingPort"/>, bridging the Town Hall
/// business laundering cycle to the host wallet and tracking locally reserved capacity.
/// </summary>
public sealed class NativeLaunderingPort : ILaunderingPort
{
    private readonly NativeWorldPort _worldPort;
    private float _currentLaunderTotal;

    public NativeLaunderingPort(NativeWorldPort worldPort)
    {
        _worldPort = worldPort ?? throw new ArgumentNullException(nameof(worldPort));
    }

    public float GetLaunderCapacity()
    {
        return BusinessLaunderingManager.DefaultCapacity;
    }

    public float GetCurrentLaunderTotal()
    {
        return _currentLaunderTotal;
    }

    public float GetPlayerCash() => TownHallNativeMoney.ReadBalance(bank: false);

    public MoneyMutationOutcome DeductCash(float amount) =>
        TownHallNativeMoney.ChangeBalance(-amount, bank: false, playCashSound: true);

    public MoneyMutationOutcome RefundCash(float amount) =>
        TownHallNativeMoney.ChangeBalance(amount, bank: false);

    public bool StartNativeLaundering(float amount)
    {
        if (amount <= 0f || !_worldPort.IsRegistered || !_worldPort.IsOwned)
            return false;

        _currentLaunderTotal += amount;
        return true;
    }

    public MoneyMutationOutcome PayoutCleanFunds(float amount, string label, string note)
    {
        var outcome = TownHallNativeMoney.ChangeBalance(amount, bank: true, label, note);
        if (outcome == MoneyMutationOutcome.Applied)
            _currentLaunderTotal = MathF.Max(0f, _currentLaunderTotal - amount);
        return outcome;
    }

    public void Reset()
    {
        _currentLaunderTotal = 0f;
    }
}

/// <summary>
/// Clones a vanilla <see cref="LaunderingStation"/> visual and <see cref="LaunderingInterface"/> setup
/// onto the Mayor's desk in the Town Hall upper floor (<c>office-desk-0</c>), providing the exact
/// vanilla monitor UI, camera zoom, amount slider/input dialog, 24h timeline, cash counter animation,
/// and 3D <see cref="CashStackVisuals"/> while routing all persistence through <see cref="BusinessLaunderingManager"/>.
/// </summary>
public static class TownHallLaunderingDeskIntegration
{
    private static HarmonyLib.Harmony? _harmony;
    private static bool _patchesInstalled;
    private static GameObject? _inactiveBusinessHolder;
    private static Business? _inactiveBusiness;
    private static GameObject? _stationRoot;
    private static LaunderingInterface? _clonedInterface;
    private static InteractableObject? _clonedIntObj;
    private static CashCounter? _clonedCashCounter;
    private static readonly List<(Renderer Renderer, bool WasEnabled)> _hiddenFurnitureRenderers = new();
    private static readonly List<(Collider Collider, bool WasEnabled)> _hiddenFurnitureColliders = new();
    private static readonly Dictionary<string, LaunderingOperation> _nativeOperations = new(StringComparer.Ordinal);
    private static int _lastHoverFrame = -1;

    public static bool IsInstalled =>
        S1Mods.Shared.NetworkGuard.IsAlive(_stationRoot) &&
        S1Mods.Shared.NetworkGuard.IsAlive(_clonedInterface);

    public static bool IsInterfaceOpen =>
        IsInstalled && _clonedInterface!.IsOpen;

    public static void InstallPatches()
    {
        if (_patchesInstalled) return;
        try
        {
            _harmony = new HarmonyLib.Harmony("com.SCHEDULE1.TownHallInterior.LaunderingDesk");

            var confirmMethod = AccessTools.Method(typeof(LaunderingInterface), nameof(LaunderingInterface.ConfirmAmount))
                ?? throw new MissingMethodException(nameof(LaunderingInterface.ConfirmAmount));
            _harmony.Patch(
                confirmMethod,
                prefix: new HarmonyMethod(typeof(TownHallLaunderingDeskIntegration), nameof(OnConfirmAmountPrefix)));
            if (!HarmonyLib.Harmony.GetPatchInfo(confirmMethod).Owners.Contains(_harmony.Id))
                throw new InvalidOperationException("Laundering confirm was not patched.");

            var uiHoveredMethod = AccessTools.Method(typeof(LaunderingInterface), nameof(LaunderingInterface.Hovered));
            if (uiHoveredMethod != null)
            {
                _harmony.Patch(
                    uiHoveredMethod,
                    postfix: new HarmonyMethod(typeof(TownHallLaunderingDeskIntegration), nameof(OnInterfaceHoveredPostfix)));
            }

            var intHoveredMethod = AccessTools.Method(typeof(InteractableObject), nameof(InteractableObject.Hovered));
            if (intHoveredMethod != null)
            {
                _harmony.Patch(
                    intHoveredMethod,
                    prefix: new HarmonyMethod(typeof(TownHallLaunderingDeskIntegration), nameof(OnInteractableHoveredPrefix)));
            }

            var intStartMethod = AccessTools.Method(typeof(InteractableObject), nameof(InteractableObject.StartInteract));
            if (intStartMethod != null)
            {
                _harmony.Patch(
                    intStartMethod,
                    postfix: new HarmonyMethod(typeof(TownHallLaunderingDeskIntegration), nameof(OnInteractableStartInteractPostfix)));
            }

            _patchesInstalled = true;
            MelonLogger.Msg("TownHallInterior: laundering desk Harmony patches installed.");
        }
        catch (Exception ex)
        {
            UninstallPatches();
            MelonLogger.Warning("TownHallInterior: failed to install laundering desk Harmony patches: " + ex.Message);
        }
    }

    public static void UninstallPatches()
    {
        if (_harmony == null) return;
        try
        {
            _harmony?.UnpatchSelf();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior: failed to uninstall laundering desk Harmony patches: " + ex.Message);
        }
        finally
        {
            _harmony = null;
            _patchesInstalled = false;
        }
    }

    public static bool TryInstallStation(
        Transform? roomRoot,
        bool isOwned,
        IReadOnlyList<ActiveLaunderingOperationRecord>? activeOps = null)
    {
        if (!_patchesInstalled) return false;
        if (IsInstalled)
        {
            SyncOwnership(isOwned);
            SyncOperations(activeOps);
            return true;
        }

        if (!S1Mods.Shared.NetworkGuard.IsAlive(roomRoot))
            return false;

        try
        {
            var donor = FindDonorStation();
            if (donor == null || !S1Mods.Shared.NetworkGuard.IsAlive(donor.Interface))
                return false;

            var donorInterface = donor.Interface!;
            if (!S1Mods.Shared.NetworkGuard.IsAlive(donorInterface.canvas) ||
                !S1Mods.Shared.NetworkGuard.IsAlive(donorInterface.cameraPosition))
                return false;

            var receptionRoot = roomRoot!.Find("TownHallInterior.VanillaReception");
            var officeDesk = receptionRoot?.Find("office-desk-0");
            if (officeDesk == null)
                return false;

            // 1. Create an INACTIVE holder for our local Business shell so Awake/OnEnable/Start never fire
            // and the instance never enters Business.Businesses or SaveManager.Saveables.
            _inactiveBusinessHolder = new GameObject("TownHall_InactiveBusinessShell");
            _inactiveBusinessHolder.SetActive(false);
            _inactiveBusinessHolder.transform.SetParent(roomRoot, false);
            _inactiveBusiness = _inactiveBusinessHolder.AddComponent<Business>();
            _inactiveBusiness.propertyName = "Hyland Town Hall";
            _inactiveBusiness.propertyCode = "hyland_town_hall";
            _inactiveBusiness.LaunderCapacity = BusinessLaunderingManager.DefaultCapacity;
            _inactiveBusiness._IsOwned_k__BackingField = isOwned;

            // 2. Clone all direct children of donor station into an inactive container (skipping root GridItem/NetworkObject).
            _stationRoot = new GameObject("TownHall_LaunderingStation");
            _stationRoot.SetActive(false);
            _stationRoot.transform.SetParent(roomRoot, false);
            _stationRoot.transform.position = donor.transform.position;
            _stationRoot.transform.rotation = donor.transform.rotation;
            _stationRoot.transform.localScale = donor.transform.localScale;

            int childCount = donor.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var srcChild = donor.transform.GetChild(i);
                if (srcChild == null) continue;
                var clonedChild = UnityEngine.Object.Instantiate(srcChild.gameObject, _stationRoot.transform);
                clonedChild.name = srcChild.name;
                clonedChild.transform.localPosition = srcChild.localPosition;
                clonedChild.transform.localRotation = srcChild.localRotation;
                clonedChild.transform.localScale = srcChild.localScale;
            }

            // Strip any accidental NetworkBehaviour or NetworkObject on children while still inactive.
            foreach (var netBeh in _stationRoot.GetComponentsInChildren<NetworkBehaviour>(true))
            {
                if (netBeh != null) UnityEngine.Object.Destroy(netBeh);
            }
            foreach (var netObj in _stationRoot.GetComponentsInChildren<NetworkObject>(true))
            {
                if (netObj != null) UnityEngine.Object.Destroy(netObj);
            }

            _clonedInterface = _stationRoot.GetComponentInChildren<LaunderingInterface>(true);
            if (!S1Mods.Shared.NetworkGuard.IsAlive(_clonedInterface))
            {
                Teardown();
                return false;
            }

            // 3. Remap any cross-branch Transform / Component references from donor to _stationRoot.
            var remappedCam = RemapTransform(donor.transform, _stationRoot.transform, donorInterface.cameraPosition);
            var remappedCanvas = RemapTransform(donor.transform, _stationRoot.transform, donorInterface.canvas!.transform)
                ?.GetComponent<Canvas>();
            if (remappedCam == null || !remappedCam.IsChildOf(_stationRoot.transform) ||
                remappedCanvas == null || !remappedCanvas.transform.IsChildOf(_stationRoot.transform))
            {
                throw new InvalidOperationException("Laundering camera and canvas must belong to the cloned station.");
            }
            _clonedInterface!.cameraPosition = remappedCam;
            _clonedInterface.canvas = remappedCanvas;

            _clonedIntObj = _stationRoot.GetComponentInChildren<InteractableObject>(true);
            if (_clonedIntObj != null)
            {
                _clonedInterface!.intObj = _clonedIntObj;
                if (_clonedIntObj.displayLocationPoint != null)
                {
                    var remappedPt = RemapTransform(donor.transform, _stationRoot.transform, _clonedIntObj.displayLocationPoint);
                    if (remappedPt != null) _clonedIntObj.displayLocationPoint = remappedPt;
                }
                if (_clonedIntObj.displayLocationCollider != null)
                {
                    var remappedColTransform = RemapTransform(donor.transform, _stationRoot.transform, _clonedIntObj.displayLocationCollider.transform);
                    var remappedCol = remappedColTransform != null ? remappedColTransform.GetComponent<Collider>() : null;
                    if (remappedCol != null) _clonedIntObj.displayLocationCollider = remappedCol;
                }
            }

            var clonedStacks = _stationRoot.GetComponentsInChildren<CashStackVisuals>(true);
            if (clonedStacks != null && clonedStacks.Length > 0)
            {
                _clonedInterface!.cashStacks = new Il2CppReferenceArray<CashStackVisuals>(clonedStacks.Length);
                for (int i = 0; i < clonedStacks.Length; i++)
                    _clonedInterface.cashStacks[i] = clonedStacks[i];
            }

            _clonedCashCounter = _stationRoot.GetComponentInChildren<CashCounter>(true);
            if (_clonedCashCounter != null)
            {
                _clonedCashCounter.IsOn = false;
            }

            // 4. Clean donor dynamic UI notches/entries and MonoState delegates BEFORE activating & initializing.
            CleanDonorUiState(_clonedInterface!);

            // Scene donors can have an active interaction hierarchy and inactive
            // furniture. Enable the two visual roots on our clone.
            var table = _stationRoot.transform.Find("plastictable_2x1");
            var computer = _stationRoot.transform.Find("oldcomputer");
            if (table == null || computer == null)
                throw new InvalidOperationException("Cloned laundering table or computer is missing.");

            table.gameObject.SetActive(true);
            computer.gameObject.SetActive(true);

            // Keep the camera and PC together while orienting the station toward the chair.
            Vector3 sourceLookDir = Vector3.ProjectOnPlane(donorInterface.cameraPosition!.forward, Vector3.up);
            if (sourceLookDir.sqrMagnitude < 0.001f)
                sourceLookDir = Vector3.ProjectOnPlane(donor.transform.forward, Vector3.up);
            sourceLookDir.Normalize();

            // At office-desk-0 (X=56.4), office-chair-0 is at X=57.9 looking in the -X direction (Vector3.left).
            Quaternion rotDelta = Quaternion.FromToRotation(sourceLookDir, Vector3.left);
            _stationRoot.transform.rotation = rotDelta * donor.transform.rotation;
            Vector3 deskPosition = officeDesk.position;
            Vector3 tablePosition = table.position;
            _stationRoot.transform.position += new Vector3(
                deskPosition.x - tablePosition.x,
                deskPosition.y - GetTableBottom(table),
                deskPosition.z - tablePosition.z);

            // Detach donor lightmaps on all cloned renderers so upper-floor lighting applies cleanly.
            foreach (var r in _stationRoot.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (r == null) continue;
                r.lightmapIndex = -1;
                r.lightProbeUsage = LightProbeUsage.BlendProbes;
                r.reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
            }

            // 5. Run native LaunderingInterface.Initialize(_inactiveBusiness) to wire MonoState [ESC],
            // timeline notches, capacity label ($20,000), and InteractableObject listeners.
            _stationRoot.SetActive(true);
            _clonedInterface.Initialize(_inactiveBusiness);
            remappedCanvas.enabled = false;

            SyncOwnership(isOwned);
            SyncOperations(activeOps);

            // Keep the original furniture unless the actual table surface and PC body
            // can render after activation and native UI initialization.
            if (!HasDrawableMesh(table.Find("Table")) || !HasDrawableMesh(computer.Find("Base")))
                throw new InvalidOperationException("Cloned laundering table or computer is not drawable.");

            HideOverlappingFurniture(receptionRoot!);
            MelonLogger.Msg("TownHallInterior: laundering table and computer meshes active at upper-floor Mayor's desk.");
            return true;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior: failed to install laundering desk station: " + ex);
            Teardown();
            return false;
        }
    }

    public static void SyncOwnership(bool isOwned)
    {
        if (!S1Mods.Shared.NetworkGuard.IsAlive(_inactiveBusiness)) return;
        _inactiveBusiness!._IsOwned_k__BackingField = isOwned;

        if (S1Mods.Shared.NetworkGuard.IsAlive(_clonedInterface))
        {
            try
            {
                _clonedInterface!.RefreshLaunderButton();
                _clonedInterface.UpdateCurrentTotal();
            }
            catch
            {
                // Ignore transient UI refresh errors
            }
        }
    }

    public static void SyncOperations(IReadOnlyList<ActiveLaunderingOperationRecord>? activeOps)
    {
        if (!IsInstalled || !S1Mods.Shared.NetworkGuard.IsAlive(_inactiveBusiness))
            return;

        try
        {
            var opsList = _inactiveBusiness!.LaunderingOperations;
            if (opsList == null) return;

            var activeIds = new HashSet<string>(StringComparer.Ordinal);
            if (activeOps != null)
            {
                for (int i = 0; i < activeOps.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(activeOps[i].OperationId))
                        activeIds.Add(activeOps[i].OperationId);
                }
            }

            // 1. Remove completed operations
            var toRemove = new List<string>();
            foreach (var kvp in _nativeOperations)
            {
                if (!activeIds.Contains(kvp.Key))
                {
                    toRemove.Add(kvp.Key);
                }
            }

            bool structureChanged = false;
            foreach (string id in toRemove)
            {
                if (_nativeOperations.TryGetValue(id, out var finishedOp) && finishedOp != null)
                {
                    opsList.Remove(finishedOp);
                    try { _clonedInterface!.RemoveEntry(finishedOp); } catch { }
                    structureChanged = true;
                }
                _nativeOperations.Remove(id);
            }

            // 2. Add new or update existing operations
            if (activeOps != null)
            {
                for (int i = 0; i < activeOps.Count; i++)
                {
                    var rec = activeOps[i];
                    if (rec == null || string.IsNullOrWhiteSpace(rec.OperationId) || rec.Amount <= 0f)
                        continue;

                    int mins = rec.GetMinutesSinceStarted();
                    if (_nativeOperations.TryGetValue(rec.OperationId, out var existingOp) && existingOp != null)
                    {
                        existingOp.minutesSinceStarted = mins;
                    }
                    else
                    {
                        var newOp = new LaunderingOperation(_inactiveBusiness, rec.Amount, mins);
                        opsList.Add(newOp);
                        _nativeOperations[rec.OperationId] = newOp;
                        try { _clonedInterface!.CreateEntry(newOp); } catch { }
                        structureChanged = true;
                    }
                }
            }

            // 3. Refresh monitor timeline, entry countdowns, total label, 3D cash stacks, and cash counter animation
            _clonedInterface!.UpdateTimeline();
            _clonedInterface.UpdateEntryTimes();
            _clonedInterface.UpdateCurrentTotal();
            if (structureChanged || activeOps == null || activeOps.Count == 0)
            {
                _clonedInterface.UpdateCashStacks(null);
            }
            _clonedInterface.RefreshLaunderButton();

            if (S1Mods.Shared.NetworkGuard.IsAlive(_clonedCashCounter))
            {
                _clonedCashCounter!.IsOn = opsList.Count > 0;
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior: error syncing laundering desk operations: " + ex.Message);
        }
    }

    public static void Teardown()
    {
        try
        {
            if (S1Mods.Shared.NetworkGuard.IsAlive(_clonedInterface) && _clonedInterface!.IsOpen)
            {
                try { _clonedInterface.CloseAmountSelector(); } catch { }
                try { _clonedInterface.OnClose(); } catch { }
            }
        }
        catch
        {
            // Ignore teardown close errors
        }

        if (S1Mods.Shared.NetworkGuard.IsAlive(_stationRoot))
        {
            _stationRoot!.SetActive(false);
            UnityEngine.Object.Destroy(_stationRoot);
        }
        _stationRoot = null;

        foreach (var (renderer, wasEnabled) in _hiddenFurnitureRenderers)
        {
            if (S1Mods.Shared.NetworkGuard.IsAlive(renderer))
            {
                renderer.enabled = wasEnabled;
            }
        }
        _hiddenFurnitureRenderers.Clear();

        foreach (var (collider, wasEnabled) in _hiddenFurnitureColliders)
        {
            if (S1Mods.Shared.NetworkGuard.IsAlive(collider))
            {
                collider.enabled = wasEnabled;
            }
        }
        _hiddenFurnitureColliders.Clear();

        _nativeOperations.Clear();
        _clonedCashCounter = null;
        _clonedIntObj = null;
        _clonedInterface = null;

        _inactiveBusiness = null;
        if (S1Mods.Shared.NetworkGuard.IsAlive(_inactiveBusinessHolder))
        {
            UnityEngine.Object.Destroy(_inactiveBusinessHolder);
        }
        _inactiveBusinessHolder = null;
    }

    private static bool OnConfirmAmountPrefix(LaunderingInterface __instance)
    {
        if (__instance == null || !IsInstalled || __instance.Pointer != _clonedInterface!.Pointer)
            return true;

        try
        {
            int maxAmount = __instance.maxLaunderAmount;
            int minAmount = LaunderingInterface.MinLaunderAmount;
            int amount = Mathf.Clamp(__instance.selectedAmountToLaunder, minAmount, maxAmount);
            if (amount < minAmount || Mod.Instance == null)
            {
                __instance.CloseAmountSelector();
                return false;
            }

            var receipt = Mod.Instance.TryStartLaundering(amount);
            if (receipt != null && receipt.Success)
            {
                __instance.CloseAmountSelector();
                __instance.RefreshLaunderButton();
            }
            else
            {
                __instance.CloseAmountSelector();
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior: ConfirmAmount error on laundering desk: " + ex.Message);
        }

        return false;
    }

    private static void OnInterfaceHoveredPostfix(LaunderingInterface __instance)
    {
        if (__instance == null || !IsInstalled || __instance.Pointer != _clonedInterface!.Pointer)
            return;

        try
        {
            if (__instance.IsOpen || !S1Mods.Shared.NetworkGuard.IsAlive(__instance.intObj))
                return;

            bool owned = Mod.Instance?.IsTownHallOwned == true;
            if (!owned)
            {
                __instance.intObj!.SetMessage("Locked (Purchase Town Hall first)");
                __instance.intObj.SetInteractableState(InteractableObject.EInteractableState.Invalid);
            }
            else
            {
                __instance.intObj!.SetMessage("Use laundering computer");
                __instance.intObj.SetInteractableState(InteractableObject.EInteractableState.Default);
            }
        }
        catch
        {
            // Ignore hover state errors
        }
    }

    private static void OnInteractableHoveredPrefix(InteractableObject __instance)
    {
        if (__instance == null || !IsInstalled || _clonedIntObj == null || __instance.Pointer != _clonedIntObj.Pointer)
            return;

        int frame = Time.frameCount;
        if (_lastHoverFrame == frame) return;
        _lastHoverFrame = frame;

        try
        {
            _clonedInterface!.Hovered();
        }
        catch
        {
            // Ignore
        }
    }

    private static void OnInteractableStartInteractPostfix(InteractableObject __instance)
    {
        if (__instance == null || !IsInstalled || _clonedIntObj == null || __instance.Pointer != _clonedIntObj.Pointer)
            return;

        try
        {
            if (!_clonedInterface!.IsOpen && Mod.Instance?.IsTownHallOwned == true)
            {
                _clonedInterface.Interacted();
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior: error opening laundering desk interface: " + ex.Message);
        }
    }

    private static LaunderingStation? FindDonorStation()
    {
        var stations = UnityEngine.Object.FindObjectsOfType<LaunderingStation>(true);
        if (stations == null) return null;

        foreach (var station in stations)
        {
            if (!S1Mods.Shared.NetworkGuard.IsAlive(station) || !S1Mods.Shared.NetworkGuard.IsAlive(station.Interface))
                continue;
            if (!station.gameObject.scene.IsValid() || station.gameObject.scene.name != "Main")
                continue;
            return station;
        }
        return null;
    }

    private static void CleanDonorUiState(LaunderingInterface ui)
    {
        try
        {
            if (ui.notches != null)
            {
                for (int i = 0; i < ui.notches.Count; i++)
                {
                    var n = ui.notches[i];
                    if (S1Mods.Shared.NetworkGuard.IsAlive(n))
                        UnityEngine.Object.Destroy(n.gameObject);
                }
                ui.notches.Clear();
            }

            if (ui.operationToNotch != null)
            {
                foreach (var kvp in ui.operationToNotch)
                {
                    if (S1Mods.Shared.NetworkGuard.IsAlive(kvp.Value))
                        UnityEngine.Object.Destroy(kvp.Value.gameObject);
                }
                ui.operationToNotch.Clear();
            }

            if (ui.operationToEntry != null)
            {
                foreach (var kvp in ui.operationToEntry)
                {
                    if (S1Mods.Shared.NetworkGuard.IsAlive(kvp.Value))
                        UnityEngine.Object.Destroy(kvp.Value.gameObject);
                }
                ui.operationToEntry.Clear();
            }

            ResetMonoStateDelegates(ui.mainState);
            ResetMonoStateDelegates(ui.amountSelectorState);
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior: warning cleaning donor UI state: " + ex.Message);
        }
    }

    private static void ResetMonoStateDelegates(MonoState? state)
    {
        if (!S1Mods.Shared.NetworkGuard.IsAlive(state)) return;
        state!.OnAddedToStack = null;
        state.OnRemovedFromStack = null;
        state.OnStateActivate = null;
        state.OnStateDeactivate = null;
        state.OnBecomeTopSibling = null;
        state.OnNoLongerTopSibling = null;
    }

    private static Transform? RemapTransform(Transform donorRoot, Transform targetRoot, Transform? sourceNode)
    {
        if (sourceNode == null || donorRoot == null || targetRoot == null)
            return null;
        if (sourceNode == donorRoot)
            return targetRoot;
        if (!sourceNode.IsChildOf(donorRoot))
            return sourceNode;

        var indices = new Stack<int>();
        Transform? current = sourceNode;
        while (current != null && current != donorRoot)
        {
            indices.Push(current.GetSiblingIndex());
            current = current.parent;
        }

        Transform? mapped = targetRoot;
        while (indices.Count > 0 && mapped != null)
        {
            int idx = indices.Pop();
            if (idx < 0 || idx >= mapped.childCount)
                return null;
            mapped = mapped.GetChild(idx);
        }
        return mapped;
    }

    private static float GetTableBottom(Transform table)
    {
        float bottom = float.PositiveInfinity;
        foreach (var filter in table.GetComponentsInChildren<MeshFilter>(true))
        {
            var mesh = filter.sharedMesh;
            if (mesh == null || mesh.vertexCount == 0) continue;

            // Project the local mesh bounds onto world Y while the station is inactive.
            Bounds bounds = mesh.bounds;
            Matrix4x4 transform = filter.transform.localToWorldMatrix;
            Vector3 extent = bounds.extents;
            float height = Mathf.Abs(transform.m10) * extent.x + Mathf.Abs(transform.m11) * extent.y +
                Mathf.Abs(transform.m12) * extent.z;
            bottom = Mathf.Min(bottom, transform.MultiplyPoint3x4(bounds.center).y - height);
        }
        if (!float.IsFinite(bottom))
            throw new InvalidOperationException("Cloned laundering table has no usable mesh bounds.");
        return bottom;
    }

    private static bool HasDrawableMesh(Transform? node)
    {
        if (node == null || !node.gameObject.activeInHierarchy) return false;
        var renderer = node.GetComponent<MeshRenderer>();
        var mesh = node.GetComponent<MeshFilter>()?.sharedMesh;
        return renderer != null && renderer.enabled && !renderer.forceRenderingOff &&
            !renderer.isPartOfStaticBatch && mesh != null && mesh.vertexCount > 0;
    }

    private static void HideOverlappingFurniture(Transform receptionRoot)
    {
        string[] targets = { "office-desk-0", "office-computer-0", "office-plant-0" };
        foreach (string targetName in targets)
        {
            var node = receptionRoot.Find(targetName);
            if (node == null) continue;

            foreach (var r in node.GetComponentsInChildren<Renderer>(true))
            {
                if (r != null)
                {
                    _hiddenFurnitureRenderers.Add((r, r.enabled));
                    r.enabled = false;
                }
            }

            foreach (var c in node.GetComponentsInChildren<Collider>(true))
            {
                if (c != null)
                {
                    _hiddenFurnitureColliders.Add((c, c.enabled));
                    c.enabled = false;
                }
            }
        }
    }
}
