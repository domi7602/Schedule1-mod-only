using System;
using System.Collections.Generic;
using System.Diagnostics;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Misc;
using Il2CppScheduleOne.ObjectScripts;
using LightControl.Core;
using S1API.Building;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using NativeProperty = Il2CppScheduleOne.Property.Property;

namespace LightControl;

/// <summary>
/// One owned emitting LED surface slot of a grow lamp. The adapter caches the renderer, the exact
/// slot index and the native material it cloned from, so the per-frame path never instantiates a
/// material and never allocates a clone for the whole scene. The pure
/// <see cref="LedSurfacePolicy"/> owns every gating/colour decision; this type only holds the
/// Unity resources and their lifecycle.
/// </summary>
internal sealed class OwnedSurface
{
    /// <summary>The lamp's own emitting surface renderer (from <c>ToggleableLight.lightSurfacesMeshes</c>).</summary>
    public MeshRenderer? Renderer;

    /// <summary>The slot our clone currently occupies, or -1 while we own nothing.</summary>
    public int InstalledSlot = -1;

    /// <summary>Our own clone of the native LED panel material; never a shared/world material.</summary>
    public Material? Clone;

    /// <summary>The exact native material the clone was created from (restored verbatim).</summary>
    public Material? InstalledSource;

    /// <summary>One-shot guard so an unsupported material is reported once, not every frame.</summary>
    public bool Warned;

    /// <summary>
    /// Steady-state flag: true while this surface's last processed switch state and instruction
    /// produced no further work, so the next identical frame can skip the whole read/write path and
    /// allocate nothing.
    /// </summary>
    public bool Converged;

    /// <summary>The switch state of the last processed frame.</summary>
    public bool LastOn;

    /// <summary>
    /// The resolved instruction of the last processed frame (record value equality).
    /// </summary>
    public VisualInstruction? LastInstruction;

    public LedSurfacePolicy Policy { get; } = new();
}

/// <summary>
/// One owned native light source of a grow lamp: the exact Unity <c>Light</c> reached through the lamp's
/// own verified <c>ToggleableLight.lightSources[i]._Light</c>. Only its <c>color</c> and
/// <c>intensity</c> are ever written - never its transform, range, cone, cookie, shadows, culling mask,
/// render mode or enabled flag, and never the lamp's switch state. The pure
/// <see cref="NativeLightBinding"/> owns every ownership and scaling decision; it sequences the proven
/// <see cref="NativeLightPolicy"/> (intensity) and <see cref="NativeColorPolicy"/> (colour) unchanged,
/// and this type only holds the Unity handle.
/// </summary>
internal sealed class OwnedLightSource
{
    /// <summary>The lamp's own native light, or a dead wrapper once the fixture is gone.</summary>
    public Light? Handle;

    /// <summary>Coordinated intensity + colour ownership for this one native source.</summary>
    public NativeLightBinding Binding { get; } = new();
}

/// <summary>
/// One logical grow lamp: the placed object plus its proven emitting LED surface slots and its proven
/// own native light sources. A lamp whose native reference or parent property is temporarily
/// unavailable is marked <see cref="Available"/> false and kept in the record set so its state survives
/// until the same GUID reappears and is rebound.
/// </summary>
internal sealed class OwnedLamp
{
    public string Key = string.Empty;
    public LampIdentity? Identity;
    public GrowLight? GrowLight;
    /// <summary>The grow tent whose built-in light this record drives, when there is no separate GrowLight.</summary>
    public Pot? Container;
    /// <summary>Direct toggle for a grow-tent built-in light. Placeable lamps resolve theirs from <see cref="GrowLight"/>.</summary>
    public ToggleableLight? Toggle;
    /// <summary>Phone-app noun: "Grow lamp" or "Grow tent".</summary>
    public string Kind = "Grow lamp";
    public string? PropertyCode;
    public int Generation;
    public bool Available;
    /// <summary>Monotonic time of the next bounded ownership recheck for this lamp.</summary>
    public double NextOwnershipCheckAt;

    /// <summary>
    /// The exact bounded apply breadcrumb last emitted for this lamp, so a repeated identical
    /// successful install cannot log twice and no per-frame log spam is possible.
    /// </summary>
    public string? LastApplyLog;

    /// <summary>Monotonic time of the last emitted apply breadcrumb (rate limit, never per frame).</summary>
    public double LastApplyLogAt = -1;

    /// <summary>The lamp's own emitting LED panel slots (coupled output 1).</summary>
    public List<OwnedSurface> Surfaces { get; } = new();

    /// <summary>The lamp's own native light sources (coupled output 2).</summary>
    public List<OwnedLightSource> Lights { get; } = new();
}

/// <summary>
/// A discovered grow lamp whose coupled outputs are not both resolvable yet. The entry keeps only the
/// lamp's native handle and property code, is retried at a bounded cadence, and is never used to enable,
/// switch on or otherwise force the lamp into a controllable state.
/// </summary>
internal sealed class PendingLamp
{
    public GrowLight? GrowLight;
    public Pot? Container;
    public string? PropertyCode;
}

/// <summary>
/// Host/singleplayer runtime facade. It owns Unity references, the cooperative bounded discovery
/// scan, the coupled visual phase and the slot-scoped persistence handoff. Every decision is
/// delegated to the pure <c>LightControl.Core</c> model.
/// <para>
/// The lamp's LED panel and the light it emits always carry the same chosen colour, because both are
/// driven from the one <see cref="VisualInstruction"/> the model resolves for that lamp. Two coupled
/// outputs are written, and only these two:
/// </para>
/// <list type="bullet">
/// <item>the lamp's own emitting LED panel - an owned clone of the native LED material installed into
/// the one verified <c>ToggleableLight.MaterialIndex</c> slot of
/// <c>ToggleableLight.lightSurfacesMeshes</c>; and</item>
/// <item>the lamp's own native light sources - <c>ToggleableLight.lightSources[i]._Light</c>, where only
/// <c>color</c> and <c>intensity</c> are ever written.</item>
/// </list>
/// <para>
/// Emitted intensity is <i>relative</i>: the chosen factor scales that source's own freshly sampled
/// native intensity, never a panel HDR magnitude. Emitted colour is <i>absolute</i>: the chosen hue
/// replaces the native colour. Because a real light colours what it illuminates, the plants a lit lamp
/// shines on now receive coloured illumination - that is the requested behaviour, and it changes no
/// plant material, no shader and no game state. Shared/world materials are never mutated, the native
/// switch state, transform, cone, cookie, shadows, culling mask and enabled flag are never touched, and
/// every owned resource is restored and released on teardown.
/// </para>
/// </summary>
internal static class LightRuntime
{
    /// <summary>Reconcile cadence: a new bounded walk is started at most once per interval.</summary>
    private const double ReconcileIntervalSeconds = 20;

    private const double AutosaveIntervalSeconds = 2;

    /// <summary>Retry cadence for a deferred clone release: at most one attempt per second, never a busy loop.</summary>
    private const double PendingReleaseRetrySeconds = 1;

    /// <summary>
    /// Retry cadence for a lamp whose coupled outputs are not both resolvable yet (its native light may
    /// simply not have resolved while the lamp is off). Bounded: never per frame, never a busy loop.
    /// </summary>
    private const double PendingCouplingRetrySeconds = 2;

    /// <summary>Hard cap on retained pending-coupling entries, so an unreadable fixture cannot grow the set.</summary>
    private const int PendingCouplingCap = 256;

    /// <summary>
    /// Minimum interval between two emitted apply breadcrumbs. A per-frame changing instruction (a
    /// rainbow effect) would otherwise log every frame; the identical-message check alone cannot cap it.
    /// </summary>
    private const double ApplyLogIntervalSeconds = 1;

    /// <summary>The verified sender id used for the local host's own commands.</summary>
    private const string LocalSenderId = "local-host";

    /// <summary>Cooperative scan limits: nodes per frame, milliseconds per frame, hard node cap.</summary>
    private const int ScanNodesPerFrame = 64;
    private const double ScanMillisecondsPerFrame = 2.0;
    private const int ScanNodeCap = 200_000;

    /// <summary>Ownership is rechecked before every command and at most once per this interval.</summary>
    private const double OwnershipRecheckIntervalSeconds = 1;

    /// <summary>The URP HDR emission colour that drives the visible glow of an LED panel.</summary>
    private const string EmissionProperty = "_EmissionColor";

    /// <summary>
    /// The verified grow-lamp LED family: the shipped LEDLight prefab's native ON material
    /// (<c>ToggleableLight</c> mb 94512 in <c>sharedassets0.assets</c> -&gt; Material 637).
    /// </summary>
    private const string LedFamilyOnMaterial = "ledlight_light_on";

    /// <summary>
    /// The verified grow-lamp LED family: the shipped LEDLight prefab's native OFF material
    /// (<c>ToggleableLight</c> mb 94512 in <c>sharedassets0.assets</c> -&gt; Material 636).
    /// </summary>
    private const string LedFamilyOffMaterial = "ledlight_light_off";

    /// <summary>
    /// The only material names this build recolours: the LEDLight panel pair and the grow-tent
    /// halogen pair (<c>halogen light on mat</c> / <c>halogen light off mat</c>). Full-spectrum
    /// stays rejected. The names live in <see cref="PanelFamilyPolicy"/>.
    /// </summary>
    private static readonly string[] SupportedPanelMaterials = { LedFamilyOnMaterial, LedFamilyOffMaterial, PanelFamilyPolicy.HalogenOn, PanelFamilyPolicy.HalogenOff };

    private static readonly Stopwatch Clock = new();
    private static readonly Dictionary<string, OwnedLamp> Lamps = new(StringComparer.Ordinal);
    /// <summary>Lamps that are currently available and controllable (the phone app's list).</summary>
    private static readonly List<OwnedLamp> Order = new();
    private static readonly ScanCadence Reconcile = new(ReconcileIntervalSeconds);
    private static readonly SceneScanBudget ScanBudget = new(ScanNodesPerFrame, ScanMillisecondsPerFrame, ScanNodeCap);
    private static readonly Queue<Transform> ScanQueue = new();
    private static readonly HashSet<long> ScanVisited = new();
    private static readonly Stopwatch ScanWatch = new();
    private static readonly ModLogger Log = Mod.Log;

    private static LightStateStore? _store;
    private static JsonStatePersistence? _persistence;
    private static LightPersistence? _storage;
    private static bool _scanFailureLogged;
    private static bool _applyFailureLogged;
    private static bool _familySkipLogged;
    private static bool _couplingSkipLogged;
    private static bool _scanActive;
    private static bool _scanTruncated;
    private static int _scanVisitedCount;
    private static int _scanFoundCount;
    private static double _scanElapsedMilliseconds;
    private static int _scanRootCursor;
    private static List<string>? _ownedPropertyCodes;
    /// <summary>Distinct lamps reported as switched on with no readable native light source.</summary>
    private static readonly HashSet<string> UnsupportedCoupling = new(StringComparer.Ordinal);
    private static double _nextAutosave;
    private static long _savedRevision = -1;

    /// <summary>
    /// Owned surface clones whose release could not be proven safe (the renderer may still reference
    /// them). They are retried once per second from <see cref="UpdatePhase"/> — even while no session
    /// is ready — and are never destroyed while a renderer may still point at them, so a failed
    /// restore can never leave a dangling material reference.
    /// </summary>
    private static readonly List<OwnedSurface> PendingReleases = new();
    private static double _nextPendingRelease;

    /// <summary>
    /// Grow lamps whose coupled outputs are not both resolvable yet. Retried at a bounded cadence from
    /// <see cref="UpdatePhase"/>; an entry is never used to enable, switch on or otherwise force a lamp.
    /// </summary>
    private static readonly Dictionary<string, PendingLamp> PendingCoupling = new(StringComparer.Ordinal);
    private static double _nextPendingCoupling;

    /// <summary>
    /// Lamps that are on but expose no resolvable native light source, so coupled control cannot be
    /// applied. They are reported here (and in the log) and left completely untouched - the mod never
    /// pretends to control both outputs when only the panel is available.
    /// </summary>
    public static int UnsupportedLampCount { get; private set; }

    /// <summary>True while a host session exists and the coupled lamp outputs are being driven.</summary>
    public static bool Ready { get; private set; }

    /// <summary>Number of currently controllable logical grow lamps.</summary>
    public static int LampCount => Order.Count;

    /// <summary>Current authoritative revision of the pure model.</summary>
    public static long Revision => _store?.Revision ?? 0;

    /// <summary>Human-readable session state for the phone app.</summary>
    public static string StatusMessage { get; private set; } = "Waiting for a loaded save.";

    /// <summary>True when the initial lamp-data read failed, so this session cannot persist.</summary>
    public static bool StorageBlocked => _persistence?.Gate == PersistenceGate.Blocked;

    public static Guid SessionId => Ready && _store != null ? _store.SessionId : Guid.Empty;

    public static long CatalogRevision => Ready && _store != null ? _store.CatalogRevision : 0;

    public static long SavedRevision => _savedRevision;

    public static bool LastSaveFailed { get; private set; }

    public static bool SaveConfirmed { get; private set; }

    public static double ClockSeconds => Now;

    /// <summary>Monotonic, session-relative time. Never derived from Unity's scaled clock.</summary>
    private static double Now => Clock.Elapsed.TotalSeconds;

    public static void Initialize()
    {
        if (!Clock.IsRunning) Clock.Start();
    }

    // ---------------------------------------------------------------------------------------
    // Lifecycle
    // ---------------------------------------------------------------------------------------

    /// <summary>
    /// Begins the host session for the active save slot and starts the bounded discovery scan. The
    /// scan is cooperative: it is stepped with a per-frame budget and reports its real completion
    /// count, elapsed work and truncation status, so this method never claims a completed scan.
    /// Host/singleplayer only: a multiplayer client is explicitly read-only in this build.
    /// </summary>
    public static void LoadComplete()
    {
        if (Ready) return;
        if (!NetworkGuard.IsHostOrSingleplayer())
        {
            StatusMessage = "Multiplayer client: read-only. Lamp control is host-only in this build.";
            Log.Info("Multiplayer client detected; LightControl stays read-only and writes nothing.");
            return;
        }

        int slot = SaveSlots.GetActiveSlotNumber();
        string? saveId = PersistenceSlotPolicy.SaveIdForSlot(slot);
        if (saveId is null)
        {
            StatusMessage = "No active save slot yet.";
            Log.Info("No active save slot; LightControl will attach on the next load.");
            return;
        }

        var store = new LightStateStore();
        store.BeginSession(Guid.NewGuid(), saveId, SessionMode.Host);
        var persistence = new JsonStatePersistence(store);
        var storage = new LightPersistence(slot, saveId, Log);
        bool loaded = storage.Load(persistence);
        Log.Info($"Lamp data gate '{persistence.Gate}' for {saveId} (loaded={loaded}).");

        _store = store;
        _persistence = persistence;
        _storage = storage;
        Ready = true;
        _scanFailureLogged = false;
        _applyFailureLogged = false;
        _familySkipLogged = false;
        _couplingSkipLogged = false;
        UnsupportedLampCount = 0;
        UnsupportedCoupling.Clear();
        PendingCoupling.Clear();
        _nextPendingCoupling = 0;
        Reconcile.Reset(Now);
        RefreshOwnedPropertyCodes();

        StatusMessage = persistence.Gate == PersistenceGate.Blocked
            ? "Lamp data could not be read; changes are not saved this session."
            : "Host session ready. LED panel and emitted light use the same color.";

        BeginScan();
        _savedRevision = store.Revision;
        LastSaveFailed = false;
        SaveConfirmed = false;
        _nextAutosave = Now + AutosaveIntervalSeconds;
        Log.Info($"Host session ready; a bounded grow-lamp scan was started (gate '{persistence.Gate}').");
    }

    /// <summary>
    /// Restores every owned surface and native light output, releases every owned clone, persists once,
    /// then drops the session. Idempotent: safe to call from pre-load, pre-scene-change, scene-unload,
    /// quit and deinitialization paths. A clone that cannot be proven unreferenced is retained (never
    /// cleared here) and retried, so it is not destroyed while a renderer may still point at it.
    /// </summary>
    public static void Teardown()
    {
        Persist();
        foreach (var lamp in Lamps.Values)
        {
            foreach (var surface in lamp.Surfaces)
                ReleaseOwned(surface);
            foreach (var source in lamp.Lights)
                RestoreLightSource(source);
        }
        if (PendingReleases.Count > 0)
            Log.Warn($"{PendingReleases.Count} grow-lamp LED surface clone(s) could not be proven unreferenced; their release is retained and retried once per second, and they are never destroyed while a renderer may still reference them.");
        Order.Clear();
        Lamps.Clear();
        PendingCoupling.Clear();
        _nextPendingCoupling = 0;
        UnsupportedLampCount = 0;
        UnsupportedCoupling.Clear();
        ScanQueue.Clear();
        ScanVisited.Clear();
        _scanActive = false;
        _scanTruncated = false;
        _scanVisitedCount = 0;
        _scanFoundCount = 0;
        _scanElapsedMilliseconds = 0;
        _scanRootCursor = 0;
        _ownedPropertyCodes = null;

        if (_store is not null)
        {
            try { _store.EndSession(); }
            catch (Exception ex) { Log.Warn("EndSession failed", ex); }
            _store = null;
        }

        _persistence = null;
        _storage = null;
        Ready = false;
        _savedRevision = -1;
        StatusMessage = "Waiting for a loaded save.";
    }

    /// <summary>Persists the current lamp data immediately (save-start hook and teardown).</summary>
    public static void SaveNow() => Persist();

    /// <summary>Registers a lamp that was placed while the session was live.</summary>
    public static void OnBuildablePlaced(BuildEventArgs args)
    {
        if (!Ready || args?.GameObject is null) return;
        try
        {
            var growLight = args.GameObject.GetComponent<GrowLight>();
            if (growLight != null && TryRegister(growLight))
                Log.Info($"Registered newly placed grow lamp ({Order.Count} total).");
            var pot = args.GameObject.GetComponent<Pot>();
            if (pot != null && TryRegisterGrowTent(pot))
                Log.Info($"Registered newly placed grow tent ({Order.Count} total).");
        }
        catch (Exception ex) { Log.Warn("Newly placed buildable could not be inspected", ex); }
    }

    // ---------------------------------------------------------------------------------------
    // The visual phase: the lamp's own emitting LED panel and the light it emits
    // ---------------------------------------------------------------------------------------

    /// <summary>
    /// Update phase: retries any deferred clone release, hands every owned native light output back to
    /// the game's own writer, then steps the cooperative bounded scan, the bounded pending-coupling
    /// retry, the debounced autosave and the reconcile cadence. The undo runs for every lamp that still
    /// owns an output - including one that was just switched off - so the game's own value can never be
    /// re-scaled by us; the single visual write is the late-update phase.
    /// </summary>
    public static void UpdatePhase()
    {
        // A deferred clone release is retried even while no session is ready, so a teardown that could
        // not prove a clone unreferenced is still completed once the renderer allows it.
        RetryPendingReleases();
        if (!Ready) return;

        UndoLights();
        ScanStep();
        RetryPendingCoupling();

        double now = Now;
        if (Reconcile.ShouldScan(now))
        {
            RefreshOwnedPropertyCodes();
            ReconcileLamps();
            if (!_scanActive) BeginScan();
        }
        Autosave(now);
    }

    /// <summary>
    /// Removes only lamps whose placed object explicitly reports itself destroyed (permanent
    /// removal). A lamp whose native reference, surface renderer or parent property is unavailable
    /// is demoted to "temporarily unloaded" instead: it leaves the active list but keeps its state
    /// for a later rebind, so a missing or culled renderer is never treated as removal.
    /// Runs at the bounded reconcile cadence, never per frame.
    /// </summary>
    private static void ReconcileLamps()
    {
        if (_store is null) return;
        for (int index = Order.Count - 1; index >= 0; index--)
        {
            var lamp = Order[index];
            if (lamp.Identity is null) continue;
            if (IsPermanentlyGone(lamp))
            {
                RemoveDestroyed(lamp);
                continue;
            }
            if (!IsControllable(lamp))
                Demote(lamp, "its native surface renderer or parent property is unavailable");
        }
    }

    /// <summary>
    /// Late-update phase: recheck ownership, resolve the instruction from the pure model once per lamp
    /// and drive both coupled outputs of that lamp from it - the emitting LED panel and the lamp's own
    /// native light sources - so the panel and the light it casts always carry the same chosen colour.
    /// </summary>
    public static void LateUpdatePhase()
    {
        if (!Ready || _store is null) return;
        double now = Now;
        List<OwnedLamp>? demoted = null;
        foreach (var lamp in Order)
        {
            if (lamp.Identity is null) continue;
            // Recheck ownership before applying: a property sold or unowned mid-session must lose
            // its visual influence instead of staying controlled. Bounded to one native read per
            // lamp per interval so the per-frame path cannot issue an unbounded number of calls.
            if (now >= lamp.NextOwnershipCheckAt)
            {
                lamp.NextOwnershipCheckAt = now + OwnershipRecheckIntervalSeconds;
                if (!IsControllable(lamp)) { (demoted ??= new()).Add(lamp); continue; }
            }
            VisualInstruction instruction;
            try { instruction = _store.ResolveVisual(lamp.Identity, now); }
            catch (Exception ex)
            {
                if (!_applyFailureLogged) { _applyFailureLogged = true; Log.Warn("ResolveVisual failed for a lamp", ex); }
                continue;
            }
            ApplyLamp(lamp, instruction);
        }
        if (demoted is not null)
            foreach (var lamp in demoted)
                Demote(lamp, "its parent property is no longer owned");
    }

    /// <summary>
    /// Resolves the lamp's live switch state and surface slot once, then drives both coupled outputs
    /// from the same resolved instruction: the lamp's own emitting LED panel and the lamp's own native
    /// light sources. The switch state is read once per lamp and passed on, so the native toggle state
    /// and the owner verification gate both outputs identically.
    /// </summary>
    private static void ApplyLamp(OwnedLamp lamp, VisualInstruction instruction)
    {
        ToggleableLight? toggle = ResolveToggle(lamp);
        if (!NetworkGuard.IsAlive(toggle)) return;

        bool on;
        int slot;
        try
        {
            on = toggle!.isOn;
            slot = toggle.MaterialIndex;
        }
        catch (Exception ex) { LogWarnOnce("read the grow-lamp switch state", ex); return; }

        bool changed = false;
        foreach (var surface in lamp.Surfaces)
            changed |= ApplySurface(lamp, surface, toggle!, slot, on, instruction);
        foreach (var source in lamp.Lights)
            changed |= ApplyLightSource(source, on, instruction);
        if (changed) LogApplied(lamp, slot, instruction);
    }

    /// <summary>
    /// Applies the resolved instruction to exactly one owned surface slot. The native material in
    /// that slot is read through the reference-returning <c>sharedMaterials</c> accessor (never
    /// <c>material</c>/<c>materials</c>, which would instantiate a material per call), classified by
    /// the pure policy, and either replaced by our own clone or handed back to the game. Every
    /// comparison of native materials goes through <see cref="SameNativeMaterial"/>: IL2CPP hands out a
    /// fresh managed wrapper per read, so managed reference equality alone misclassifies our own clone
    /// as a foreign material and silently stops applying colours after the first one.
    /// Returns true only when the panel actually installed our output this frame.
    /// </summary>
    private static bool ApplySurface(OwnedLamp lamp, OwnedSurface surface, ToggleableLight toggle, int slot, bool on, VisualInstruction instruction)
    {
        var renderer = surface.Renderer;
        if (!NetworkGuard.IsAlive(renderer)) return false;

        // Steady state: the lamp's switch state and the resolved instruction are unchanged since the
        // last converged frame, so there is nothing to recompute. This is what keeps the per-frame
        // path allocation-free for the whole scene: no sharedMaterials array, no clone, no write.
        if (surface.Converged && surface.LastOn == on && Equals(surface.LastInstruction, instruction)) return false;
        surface.LastOn = on;
        surface.LastInstruction = instruction;
        surface.Converged = false;

        Il2CppReferenceArray<Material>? materials;
        try { materials = renderer!.sharedMaterials; }
        catch (Exception ex) { LogWarnOnce("read the grow-lamp surface materials", ex); return false; }

        Material? current = null;
        if (materials is not null && slot >= 0 && slot < materials.Length)
        {
            try { current = materials[slot]; } catch { current = null; }
        }

        bool ownClone = surface.Clone is not null && SameNativeMaterial(current, surface.Clone);
        LedSlotState state = LedSurfacePolicy.Classify(
            hasMaterial: current is not null && NetworkGuard.IsAlive(current),
            isOwnClone: ownClone,
            isSupportedPanel: IsSupportedPanel(current));

        if (state == LedSlotState.Foreign && !surface.Warned && current is not null)
        {
            surface.Warned = true;
            Log.Info($"Skipping an unsupported grow-lamp surface material '{MaterialName(current)}'; only the verified LED panel is recoloured.");
        }

        float baseR = 0f, baseG = 0f, baseB = 0f;
        // The clone source is always the native material: when the slot already holds our own clone
        // we keep the native source it was built from, never the clone itself, so the source can
        // never drift and the clones can never chain.
        Material? source = LedSurfaceOwnership.CloneSource(ownClone, current, surface.InstalledSource);
        if (state is LedSlotState.NativePanel or LedSlotState.OwnClone)
        {
            // Our own clone holds our output, so the baseline always comes from the native material.
            Material? baseline = ownClone ? surface.InstalledSource : current;
            if (!TryReadEmission(baseline, out baseR, out baseG, out baseB))
            {
                // No usable baseline: never claim ownership of a value we did not read. Converged stays
                // false, so the next frame retries instead of settling on an unread value.
                surface.Policy.RejectWrite();
                RestoreSurface(surface, PreferredNative(toggle, on));
                return false;
            }
        }

        LedSurfaceStep step = surface.Policy.Apply(on, state, baseR, baseG, baseB, instruction);
        switch (step.Action)
        {
            case LedSurfaceAction.Install:
                surface.Converged = InstallSurface(surface, renderer!, slot, source ?? surface.InstalledSource, materials, step);
                return surface.Converged;
            case LedSurfaceAction.Restore:
                // A deferred restore must not settle: it is retried until the outcome proves the slot
                // was handed back, otherwise the pending Original restore would be dropped forever.
                surface.Converged = LedSurfaceOwnership.RestoreConverged(RestoreSurface(surface, PreferredNative(toggle, on)));
                return false;
            default:
                // A switched-off lamp has nothing to do, so it settles; an on lamp whose slot is
                // foreign or empty keeps re-checking cheaply so a change is still noticed.
                surface.Converged = !on;
                return false;
        }
    }

    /// <summary>
    /// Applies the resolved instruction to one of the lamp's own native light sources. Intensity is
    /// <i>relative</i>: the chosen factor scales this source's own freshly sampled native value, never a
    /// panel HDR magnitude. Colour is <i>absolute</i>: the chosen hue replaces the native colour. A
    /// switched-off lamp is only handed back to its own baseline and is never forced on, and a native
    /// intensity of zero stays zero at every factor. Returns true only when a native property was written.
    /// </summary>
    private static bool ApplyLightSource(OwnedLightSource source, bool on, VisualInstruction instruction)
    {
        if (!TryReadNativeLight(source, out double intensity, out float r, out float g, out float b, out float a)) return false;
        NativeLightApply step = on
            ? source.Binding.Apply(intensity, r, g, b, a, instruction)
            : source.Binding.Restore(intensity, r, g, b, a);
        return WriteNative(source, step);
    }

    /// <summary>
    /// Performs the two native writes of one coordinated step, each independently guarded. A setter that
    /// throws releases ownership of exactly that property - a value we did not write is never claimed -
    /// and never aborts the other property, the rest of the lamp or another lamp's cleanup.
    /// </summary>
    private static bool WriteNative(OwnedLightSource source, NativeLightApply step)
    {
        var light = source.Handle;
        if (!NetworkGuard.IsAlive(light)) return false;
        bool written = false;
        if (step.Intensity.ShouldWrite)
        {
            try { light!.intensity = step.Intensity.Intensity; written = true; }
            catch (Exception ex) { source.Binding.RejectIntensityWrite(); LogWarnOnce("write the grow-lamp native light intensity", ex); }
        }
        if (step.Color.ShouldWrite)
        {
            try { light!.color = new Color(step.Color.R, step.Color.G, step.Color.B, step.Color.A); written = true; }
            catch (Exception ex) { source.Binding.RejectColorWrite(); LogWarnOnce("write the grow-lamp native light colour", ex); }
        }
        return written;
    }

    /// <summary>Reads one native source's live intensity and colour. A dead or unreadable source fails.</summary>
    private static bool TryReadNativeLight(OwnedLightSource source, out double intensity, out float r, out float g, out float b, out float a)
    {
        intensity = 0;
        r = 0f; g = 0f; b = 0f; a = 1f;
        var light = source.Handle;
        if (!NetworkGuard.IsAlive(light)) return false;
        try
        {
            intensity = light!.intensity;
            Color color = light.color;
            r = color.r; g = color.g; b = color.b; a = color.a;
            return true;
        }
        catch (Exception ex) { LogWarnOnce("read the grow-lamp native light", ex); return false; }
    }

    /// <summary>
    /// Update-phase undo for every owned native light output. It runs for a lamp that was switched off or
    /// has just lost ownership too, so our output never survives into the game's own writer and a
    /// released output is not left installed. A source that owns nothing costs one guard check and no
    /// native call, which keeps the steady-state per-frame work bounded and allocation-free.
    /// </summary>
    private static void UndoLights()
    {
        foreach (var lamp in Lamps.Values)
        {
            foreach (var source in lamp.Lights)
            {
                if (!source.Binding.OwnsAny) continue;
                if (!TryReadNativeLight(source, out double intensity, out float r, out float g, out float b, out float a)) continue;
                WriteNative(source, source.Binding.Undo(intensity, r, g, b, a));
            }
        }
    }

    /// <summary>
    /// Hands one owned native light output back to the game (demote, rebind, removal, teardown), but only
    /// while the native value still equals our own last write; an external change is preserved. A light
    /// wrapper that is no longer alive keeps its ownership so a later alive frame can still hand it back,
    /// and nothing is ever written for a dead handle.
    /// </summary>
    private static void RestoreLightSource(OwnedLightSource source)
    {
        if (!source.Binding.OwnsAny) return;
        if (!TryReadNativeLight(source, out double intensity, out float r, out float g, out float b, out float a)) return;
        WriteNative(source, source.Binding.Restore(intensity, r, g, b, a));
    }

    /// <summary>
    /// Ensures our own clone occupies the target slot with the resolved emission colour. The clone
    /// is created at most once per (renderer, native material) and reused; the slot array is only
    /// assigned when the slot actually changes. Returns true only when the slot is in the intended
    /// state, so a failed write is retried on the next frame instead of being treated as settled.
    /// </summary>
    private static bool InstallSurface(OwnedSurface surface, MeshRenderer renderer, int slot,
        Material? source, Il2CppReferenceArray<Material>? materials, LedSurfaceStep step)
    {
        try
        {
            if (LedSurfaceOwnership.NeedsClone(surface.Clone, source, surface.InstalledSource, NativeMaterialId))
            {
                // A replacement clone is only created once the previous one is proven to be
                // unreferenced by the renderer; otherwise this frame defers instead of overwriting a
                // live handle, which would leak the clone and leave the renderer pointing at it.
                if (surface.Clone is not null && !RendererNoLongerReferences(surface))
                {
                    surface.Policy.RejectWrite();
                    return false;
                }
                Material? created = CreateClone(source);
                if (created is null) { surface.Policy.RejectWrite(); return false; }
                if (surface.Clone is not null) ReleaseSurface(surface);
                surface.Clone = created;
                surface.InstalledSource = source;
                surface.InstalledSlot = -1;
            }

            Material clone = surface.Clone!;
            Color installed = clone.GetColor(EmissionProperty);
            if (installed.r != step.R || installed.g != step.G || installed.b != step.B)
                clone.SetColor(EmissionProperty, new Color(step.R, step.G, step.B, 1f));

            var array = materials ?? renderer.sharedMaterials;
            if (array is null || slot < 0 || slot >= array.Length) { surface.Policy.RejectWrite(); return false; }
            if (!SameNativeMaterial(array[slot], clone))
            {
                array[slot] = clone;
                renderer.sharedMaterials = array;
            }
            surface.InstalledSlot = slot;
            return true;
        }
        catch (Exception ex)
        {
            surface.Policy.RejectWrite();
            LogWarnOnce("install the grow-lamp LED surface colour", ex);
            return false;
        }
    }

    /// <summary>
    /// Puts the exact native material back, but only while the slot still holds our own clone. A slot
    /// changed by anything else (the native on/off swap, another writer) always wins and is never
    /// overwritten. Returns the release proof: the slot tracking is only dropped once the renderer is
    /// proven not to reference the clone (<see cref="LedReleaseOutcome.Disconnected"/> or
    /// <see cref="LedReleaseOutcome.Restored"/>); an unreadable renderer, a dead source or a failed
    /// setter keeps the tracked slot so a later attempt can still release the clone.
    /// </summary>
    private static LedReleaseOutcome RestoreSurface(OwnedSurface surface, Material? preferredNative)
    {
        surface.Converged = false;
        Material? clone = surface.Clone;
        int slot = surface.InstalledSlot;
        var renderer = surface.Renderer;
        bool rendererAlive = NetworkGuard.IsAlive(renderer);

        // The array is scanned for our clone through the pure accumulator: one unreadable (disposed or
        // collected) element makes the scan incomplete, and an incomplete scan can never prove the
        // clone absent. Treating such a scan as "disconnected" would destroy a material the renderer
        // still points at.
        var scan = new NativeMaterialArrayScan();
        bool exactSlotHolds = false;
        Il2CppReferenceArray<Material>? materials = null;
        if (clone is not null && rendererAlive)
        {
            try
            {
                materials = renderer!.sharedMaterials;
                if (materials is null)
                {
                    scan.MarkUnreadable();
                }
                else
                {
                    long cloneId = NativeMaterialId(clone);
                    for (int index = 0; index < materials.Length; index++)
                    {
                        Material? candidate;
                        try { candidate = materials[index]; } catch { scan.Record(0, cloneId); continue; }
                        scan.Record(NativeMaterialId(candidate), cloneId);
                    }
                    if (slot >= 0 && slot < materials.Length)
                    {
                        try { exactSlotHolds = SameNativeMaterial(materials[slot], clone); }
                        catch { exactSlotHolds = false; }
                    }
                }
            }
            catch (Exception ex) { LogWarnOnce("read the grow-lamp surface materials", ex); scan.MarkUnreadable(); }
        }

        Material? native = preferredNative;
        if (!NetworkGuard.IsAlive(native)) native = surface.InstalledSource;
        bool nativeAlive = NetworkGuard.IsAlive(native);

        // Write only into our exact owned slot, and only while it still holds our own clone.
        bool writeSucceeded = false;
        if (scan.FullyReadable && scan.ReferencesClone && exactSlotHolds && nativeAlive)
        {
            try
            {
                materials![slot] = native!;
                renderer!.sharedMaterials = materials;
                writeSucceeded = true;
            }
            catch (Exception ex) { LogWarnOnce("restore the grow-lamp native surface material", ex); }
        }

        LedReleaseOutcome outcome = LedSurfaceOwnership.DecideRelease(
            hasClone: clone is not null,
            rendererAlive: rendererAlive,
            slotReadable: scan.FullyReadable,
            arrayReferencesClone: scan.ReferencesClone,
            exactSlotHoldsClone: exactSlotHolds,
            nativeSourceAlive: nativeAlive,
            writeSucceeded: writeSucceeded);

        if (outcome is LedReleaseOutcome.Nothing or LedReleaseOutcome.Disconnected or LedReleaseOutcome.Restored)
            surface.InstalledSlot = -1;
        return outcome;
    }

    /// <summary>
    /// True only when the renderer is readable and provably references our clone nowhere in its
    /// material array. An unreadable array or element is treated as "still referenced", so a clone
    /// handle is never overwritten while the renderer may still point at it.
    /// </summary>
    private static bool RendererNoLongerReferences(OwnedSurface surface)
    {
        var renderer = surface.Renderer;
        var clone = surface.Clone;
        if (clone is null) return true;
        if (!NetworkGuard.IsAlive(renderer)) return false;
        var scan = new NativeMaterialArrayScan();
        try
        {
            var materials = renderer!.sharedMaterials;
            if (materials is null) return false;
            long cloneId = NativeMaterialId(clone);
            for (int index = 0; index < materials.Length; index++)
            {
                Material? candidate;
                try { candidate = materials[index]; } catch { scan.Record(0, cloneId); continue; }
                scan.Record(NativeMaterialId(candidate), cloneId);
            }
        }
        catch { return false; }
        return scan.ProvesDisconnected;
    }

    /// <summary>Hands one owned surface slot back to the game while keeping the clone for a rebind.</summary>
    private static void RestoreOwned(OwnedSurface surface)
    {
        RestoreSurface(surface, null);
        surface.Policy.RejectWrite();
    }

    /// <summary>
    /// Restores the slot and releases our own clone (teardown and permanent removal). The clone is
    /// destroyed only when the restore proves the renderer no longer references it; an unproven
    /// restore retains the surface for a bounded retry instead of destroying a still-referenced
    /// material and leaving a dangling reference in the renderer.
    /// </summary>
    private static void ReleaseOwned(OwnedSurface surface)
    {
        LedReleaseOutcome outcome = RestoreSurface(surface, null);
        if (outcome is LedReleaseOutcome.Nothing or LedReleaseOutcome.Disconnected or LedReleaseOutcome.Restored)
            ReleaseSurface(surface);
        else
            RetainForRelease(surface);
    }

    /// <summary>Destroys the owned clone. Only ever called once the slot no longer points at it.</summary>
    private static void ReleaseSurface(OwnedSurface surface)
    {
        PendingReleases.Remove(surface);
        var clone = surface.Clone;
        surface.Clone = null;
        surface.InstalledSource = null;
        surface.InstalledSlot = -1;
        surface.Converged = false;
        surface.Policy.RejectWrite();
        if (!NetworkGuard.IsAlive(clone)) return;
        try { UnityEngine.Object.Destroy(clone); }
        catch (Exception ex) { LogWarnOnce("destroy the grow-lamp surface material clone", ex); }
    }

    /// <summary>
    /// Retains one owned surface whose clone could not be proven unreferenced, so it is released later
    /// instead of being destroyed while a renderer may still point at it. Bounded: at most one retry
    /// per surface per second from <see cref="UpdatePhase"/>, never a busy loop, and a duplicate
    /// retain is ignored. An already-dead clone has nothing to destroy and is released immediately.
    /// </summary>
    private static void RetainForRelease(OwnedSurface surface)
    {
        if (!NetworkGuard.IsAlive(surface.Clone)) { ReleaseSurface(surface); return; }
        if (PendingReleases.Contains(surface)) return;
        PendingReleases.Add(surface);
        Log.Warn("A grow-lamp LED surface clone is still referenced by its renderer; its release is deferred and retried once per second until the renderer stops referencing it.");
    }

    /// <summary>
    /// Retries every deferred release at a bounded once-per-second cadence. Runs even while no session
    /// is ready, so a teardown that could not prove a clone unreferenced is still completed as soon as
    /// the renderer allows it. A retained surface that can never be proven is intentionally left to
    /// the loader rather than destroyed, which would leave a dangling material reference.
    /// </summary>
    private static void RetryPendingReleases()
    {
        if (PendingReleases.Count == 0) return;
        double now = Now;
        if (now < _nextPendingRelease) return;
        _nextPendingRelease = now + PendingReleaseRetrySeconds;
        for (int index = PendingReleases.Count - 1; index >= 0; index--)
        {
            var surface = PendingReleases[index];
            LedReleaseOutcome outcome = RestoreSurface(surface, null);
            if (outcome is LedReleaseOutcome.Nothing or LedReleaseOutcome.Disconnected or LedReleaseOutcome.Restored)
                ReleaseSurface(surface);
        }
    }

    /// <summary>Creates our own material clone of the verified native LED panel material.</summary>
    private static Material? CreateClone(Material? source)
    {
        if (!NetworkGuard.IsAlive(source)) return null;
        try
        {
            if (!source!.HasProperty(EmissionProperty)) return null;
            var clone = new Material(source);
            clone.name = source.name + " (LightControl)";
            return clone;
        }
        catch (Exception ex) { LogWarnOnce("clone the grow-lamp surface material", ex); return null; }
    }

    /// <summary>The native material that belongs in the slot for the lamp's current switch state.</summary>
    private static Material? PreferredNative(ToggleableLight toggle, bool on)
    {
        try { return on ? toggle.lightOnMat : toggle.lightOffMat; }
        catch { return null; }
    }

    /// <summary>True only for the verified LED panel materials that carry an HDR emission colour.</summary>
    private static bool IsSupportedPanel(Material? material)
    {
        if (!NetworkGuard.IsAlive(material)) return false;
        try
        {
            if (!material!.HasProperty(EmissionProperty)) return false;
            string name = StripInstanceSuffix(material.name);
            for (int index = 0; index < SupportedPanelMaterials.Length; index++)
                if (string.Equals(name, SupportedPanelMaterials[index], StringComparison.Ordinal)) return true;
            return false;
        }
        catch { return false; }
    }

    /// <summary>
    /// Family gate: LED grow lights and the grow-tent halogen pair. Full-spectrum stays rejected.
    /// <see cref="IsVerifiedLedFamily"/> remains the LED half so that pair cannot drift.
    /// </summary>
    private static bool IsVerifiedFamily(ToggleableLight toggle)
    {
        if (IsVerifiedLedFamily(toggle)) return true;
        try
        {
            string? on = NetworkGuard.IsAlive(toggle.lightOnMat) ? toggle.lightOnMat!.name : null;
            string? off = NetworkGuard.IsAlive(toggle.lightOffMat) ? toggle.lightOffMat!.name : null;
            return PanelFamilyPolicy.IsVerifiedPair(on, off);
        }
        catch { return false; }
    }

    /// <summary>
    /// Family gate: only a lamp whose native <c>ToggleableLight</c> carries exactly the verified
    /// LEDLight on/off material pair passes this half. Full-spectrum is never accepted here.
    /// </summary>
    private static bool IsVerifiedLedFamily(ToggleableLight toggle)
    {
        try
        {
            return IsPanelMaterial(toggle.lightOnMat, LedFamilyOnMaterial)
                && IsPanelMaterial(toggle.lightOffMat, LedFamilyOffMaterial);
        }
        catch { return false; }
    }

    /// <summary>True only for the expected LED panel material that carries an HDR emission colour.</summary>
    private static bool IsPanelMaterial(Material? material, string expectedName)
    {
        if (!NetworkGuard.IsAlive(material)) return false;
        try
        {
            if (!material!.HasProperty(EmissionProperty)) return false;
            return string.Equals(StripInstanceSuffix(material.name), expectedName, StringComparison.Ordinal);
        }
        catch { return false; }
    }

    /// <summary>Removes Unity's runtime " (Instance)" suffix so an instantiated material still matches.</summary>
    private static string StripInstanceSuffix(string? name)
    {
        string value = name ?? string.Empty;
        int suffix = value.IndexOf(" (Instance)", StringComparison.Ordinal);
        return suffix >= 0 ? value.Substring(0, suffix) : value;
    }

    /// <summary>Reads the native HDR emission colour. Never reads our own clone as a baseline.</summary>
    private static bool TryReadEmission(Material? material, out float r, out float g, out float b)
    {
        r = 0f; g = 0f; b = 0f;
        if (!NetworkGuard.IsAlive(material)) return false;
        try
        {
            if (!material!.HasProperty(EmissionProperty)) return false;
            Color color = material.GetColor(EmissionProperty);
            if (!LedSurfacePolicy.IsValid(color.r, color.g, color.b)) return false;
            r = color.r; g = color.g; b = color.b;
            return true;
        }
        catch { return false; }
    }

    /// <summary>
    /// The native pointer of a material handle, or 0 when it cannot be read. A disposed or collected
    /// IL2CPP wrapper throws on <c>Pointer</c>, so the read is guarded; the identity policy treats 0 as
    /// "unknown" — never equal to anything, and never a proof that a renderer stopped referencing our
    /// clone.
    /// </summary>
    private static long NativeMaterialId(Material? material)
    {
        if (material is null) return 0;
        try { return material.Pointer.ToInt64(); }
        catch { return 0; }
    }

    /// <summary>
    /// The single native-material identity decision for this file. IL2CPP hands out a fresh managed
    /// wrapper around the same native material on every array read, so <c>ReferenceEquals</c> alone
    /// misclassified our own clone as a foreign material after the first frame and silently stopped
    /// applying colours; the durable identity is the nonzero native pointer. A disposed wrapper fails
    /// closed to "not the same material", and no <c>operator==</c> is used (its fake-null check would
    /// turn an identity question into a liveness question).
    /// </summary>
    private static bool SameNativeMaterial(Material? left, Material? right) =>
        NativeMaterialIdentity.SameNative(left, right, NativeMaterialId);

    /// <summary>
    /// One bounded apply breadcrumb per real instruction change: the lamp's stable key, the requested
    /// model colour, the emission actually read back from our own panel clone, and the colour and
    /// intensity actually read back from the lamp's own native light. Only a successful coupled write
    /// logs; an identical line is suppressed and a per-frame changing instruction (a rainbow effect) is
    /// rate limited, so neither can spam the log and model state is never reported as a visual success.
    /// </summary>
    private static void LogApplied(OwnedLamp lamp, int slot, VisualInstruction instruction)
    {
        float panelR = 0f, panelG = 0f, panelB = 0f;
        foreach (var surface in lamp.Surfaces)
        {
            Material? clone = surface.Clone;
            if (!NetworkGuard.IsAlive(clone)) continue;
            try
            {
                Color readback = clone!.GetColor(EmissionProperty);
                panelR = readback.r; panelG = readback.g; panelB = readback.b;
                break;
            }
            catch { }
        }

        float lightR = 0f, lightG = 0f, lightB = 0f, lightIntensity = 0f;
        foreach (var source in lamp.Lights)
        {
            var light = source.Handle;
            if (!NetworkGuard.IsAlive(light)) continue;
            try
            {
                Color readback = light!.color;
                lightR = readback.r; lightG = readback.g; lightB = readback.b;
                lightIntensity = light.intensity;
                break;
            }
            catch { }
        }

        string message = $"[apply] lamp {lamp.Key} slot {slot} requested {DescribeRequest(instruction)} -> panel emission ({panelR:F3}, {panelG:F3}, {panelB:F3}) emitted ({lightR:F3}, {lightG:F3}, {lightB:F3}) x{lightIntensity:F2}";
        if (string.Equals(lamp.LastApplyLog, message, StringComparison.Ordinal)) return;
        double now = Now;
        if (now - lamp.LastApplyLogAt < ApplyLogIntervalSeconds) return;
        lamp.LastApplyLog = message;
        lamp.LastApplyLogAt = now;
        Log.Info(message);
    }

    /// <summary>The requested model colour of one instruction, for the bounded apply breadcrumb.</summary>
    private static string DescribeRequest(VisualInstruction instruction) =>
        instruction.ColorOverride is RgbColor color
            ? $"({color.R:F2}, {color.G:F2}, {color.B:F2}) x{instruction.IntensityFactor:F2}"
            : $"original x{instruction.IntensityFactor:F2}";

    private static string MaterialName(Material? material)
    {
        try { return material is null ? "<null>" : material.name ?? "<unnamed>"; }
        catch { return "<unknown>"; }
    }

    private static void LogWarnOnce(string context, Exception ex)
    {
        if (_applyFailureLogged) return;
        _applyFailureLogged = true;
        Log.Warn(context, ex);
    }

    // ---------------------------------------------------------------------------------------
    // Ownership and unload/rebind
    // ---------------------------------------------------------------------------------------

    private static bool IsGenuinelyDestroyed(GrowLight? growLight)
    {
        if (!NetworkGuard.IsAlive(growLight)) return false;
        try { return growLight!.IsDestroyed; }
        catch { return false; }
    }

    private static bool IsAnchorDestroyed(Il2CppScheduleOne.EntityFramework.BuildableItem? item)
    {
        if (!NetworkGuard.IsAlive(item)) return false;
        try { return item!.IsDestroyed; }
        catch { return false; }
    }

    /// <summary>
    /// The prologue RV is never a tracked property. Its lights stay in the world after the
    /// explosion, so exclusion is by property identity, not by <c>IsDestroyed</c>.
    /// </summary>
    private static bool IsParentPropertyDestroyed(Il2CppScheduleOne.EntityFramework.BuildableItem? item)
    {
        if (!NetworkGuard.IsAlive(item)) return false;
        NativeProperty? property;
        try { property = item!.ParentProperty; }
        catch { return false; }
        if (!NetworkGuard.IsAlive(property)) return false;
        string? code = null;
        bool isRv = false;
        try { code = property!.propertyCode; }
        catch { }
        try
        {
            var rv = property!.TryCast<Il2CppScheduleOne.Property.RV>();
            isRv = NetworkGuard.IsAlive(rv);
        }
        catch { }
        return DestroyedPropertyPolicy.IsPrologueOnly(code, isRv);
    }

    private static Il2CppScheduleOne.EntityFramework.BuildableItem? LiveAnchor(OwnedLamp lamp)
    {
        if (NetworkGuard.IsAlive(lamp.GrowLight)) return lamp.GrowLight;
        if (NetworkGuard.IsAlive(lamp.Container)) return lamp.Container;
        return null;
    }

    private static bool IsPermanentlyGone(OwnedLamp lamp)
    {
        bool lampDestroyed = IsAnchorDestroyed(lamp.GrowLight) || IsAnchorDestroyed(lamp.Container);
        bool parentDestroyed = IsParentPropertyDestroyed(LiveAnchor(lamp));
        return DestroyedPropertyPolicy.IsPermanentlyGone(lampDestroyed, parentDestroyed);
    }

    private static void RemoveDestroyed(OwnedLamp lamp)
    {
        if (lamp.Identity is not null && _store is not null)
        {
            try { _store.RemovePermanently(lamp.Identity, Now); }
            catch (Exception ex) { Log.Warn("Removing a destroyed grow lamp failed", ex); }
        }
        foreach (var surface in lamp.Surfaces) ReleaseOwned(surface);
        foreach (var source in lamp.Lights) RestoreLightSource(source);
        Order.Remove(lamp);
        Lamps.Remove(lamp.Key);
        if (!string.IsNullOrEmpty(lamp.Key)) PendingCoupling.Remove(lamp.Key);
        Log.Info($"Removed an untracked grow lamp ({Order.Count} left).");
    }

    private static ToggleableLight? ResolveToggle(OwnedLamp lamp)
    {
        if (NetworkGuard.IsAlive(lamp.Toggle)) return lamp.Toggle;
        if (!NetworkGuard.IsAlive(lamp.GrowLight)) return null;
        try { return lamp.GrowLight!.Light; }
        catch { return null; }
    }

    /// <summary>
    /// Registers the built-in light of an owned grow tent. The tent is a <c>Pot</c> with item id
    /// <c>growtent</c>; its halogen light is a child <c>ToggleableLight</c>, not a placed
    /// <c>GrowLight</c>, so the ordinary lamp scan never sees it.
    /// </summary>
    private static bool TryRegisterGrowTent(Pot? pot)
    {
        if (!Ready || _store is null) return false;
        if (!NetworkGuard.IsAlive(pot)) return false;
        if (IsAnchorDestroyed(pot) || IsParentPropertyDestroyed(pot))
        {
            // Drop a tent that was registered before the property exploded.
            return false;
        }

        string guidText;
        bool destroyed;
        bool initialized;
        string? itemId;
        try
        {
            guidText = pot!.GUID.ToString();
            destroyed = pot.IsDestroyed;
            initialized = pot.Initialized;
            itemId = pot.ItemInstance?.Definition?.ID;
        }
        catch (Exception ex) { LogWarnOnce("read native grow-tent identity", ex); return false; }
        if (destroyed || !initialized || !PanelFamilyPolicy.IsGrowTent(itemId)) return false;

        var native = LampIdentityPolicy.FromNativeGuid(guidText);
        LampIdentity? identity = native.Identity;
        if (!native.Accepted || identity is null) return false;
        if (IsParentPropertyDestroyed(pot))
        {
            if (Lamps.TryGetValue(identity.Key, out var stale)) RemoveDestroyed(stale);
            return false;
        }
        if (!TryReadOwnedPropertyCode(pot, out string propertyCode)) return false;

        var toggle = FindBuiltInToggle(pot);
        if (!NetworkGuard.IsAlive(toggle))
        {
            LogUnverifiedTentLight(pot!);
            return false;
        }

        if (Lamps.TryGetValue(identity.Key, out var known))
        {
            if (known.Available) return false;
            known.Container = pot;
            known.Toggle = toggle;
            known.Kind = "Grow tent";
            return RebindTent(known, toggle!, propertyCode);
        }

        var surfaces = CollectSurfaces(toggle!, ActiveSurfacePointers(null));
        var lights = CollectLightSources(toggle!, ActiveLightPointers(null));
        bool on;
        try { on = toggle!.isOn; }
        catch { on = false; }
        LampCouplingState coupling = NativeCouplingPolicy.Decide(surfaces.Count > 0, lights.Count > 0, on);
        if (coupling == LampCouplingState.Pending)
        {
            MarkPendingCoupling(identity.Key, null, propertyCode, pot);
            return false;
        }
        if (coupling == LampCouplingState.Unsupported)
        {
            ReportUnsupportedCoupling(identity.Key);
            return false;
        }

        var registration = new LampRegistration(identity, RuntimeInstanceId(identity, 0), propertyCode);
        try { _store.Register(registration, Now); }
        catch (Exception ex) { Log.Warn("Grow tent registration rejected", ex); return false; }

        var lamp = new OwnedLamp
        {
            Key = identity.Key,
            Identity = identity,
            Container = pot,
            Toggle = toggle,
            Kind = "Grow tent",
            PropertyCode = propertyCode,
            Generation = 0,
            Available = true
        };
        foreach (var surface in surfaces) lamp.Surfaces.Add(surface);
        foreach (var source in lights) lamp.Lights.Add(source);
        Lamps.Add(lamp.Key, lamp);
        PendingCoupling.Remove(lamp.Key);
        ClearUnsupportedCoupling(lamp.Key);
        Order.Add(lamp);
        Order.Sort(static (left, right) => string.CompareOrdinal(left.Key, right.Key));
        return true;
    }

    private static bool RebindTent(OwnedLamp lamp, ToggleableLight toggle, string propertyCode)
    {
        if (lamp.Identity is null || _store is null) return false;
        var surfaces = CollectSurfaces(toggle, ActiveSurfacePointers(lamp));
        var lights = CollectLightSources(toggle, ActiveLightPointers(lamp));
        bool on;
        try { on = toggle.isOn; }
        catch { on = false; }
        LampCouplingState coupling = NativeCouplingPolicy.Decide(surfaces.Count > 0, lights.Count > 0, on);
        if (coupling != LampCouplingState.Ready)
        {
            if (coupling == LampCouplingState.Pending)
                MarkPendingCoupling(lamp.Identity.Key, null, propertyCode, lamp.Container);
            else ReportUnsupportedCoupling(lamp.Identity.Key);
            return false;
        }
        lamp.Surfaces.Clear();
        foreach (var surface in surfaces) lamp.Surfaces.Add(surface);
        lamp.Lights.Clear();
        foreach (var source in lights) lamp.Lights.Add(source);
        lamp.Toggle = toggle;
        lamp.PropertyCode = propertyCode;
        lamp.Available = true;
        lamp.Kind = "Grow tent";
        if (!Order.Contains(lamp)) Order.Add(lamp);
        PendingCoupling.Remove(lamp.Key);
        ClearUnsupportedCoupling(lamp.Key);
        try { _store.RebindTemporarilyUnloaded(lamp.Identity, RuntimeInstanceId(lamp.Identity, lamp.Generation)); }
        catch (Exception ex) { Log.Warn("Grow tent rebind rejected", ex); return false; }
        return true;
    }

    /// <summary>Bounded child walk for the tent's one verified built-in toggle. Never a scene-wide query.</summary>
    private static ToggleableLight? FindBuiltInToggle(Pot pot)
    {
        Transform? root;
        try { root = pot.transform; }
        catch { return null; }
        if (!NetworkGuard.IsAlive(root)) return null;
        var queue = new Queue<Transform>();
        queue.Enqueue(root!);
        int visited = 0;
        while (queue.Count > 0 && visited < 64)
        {
            Transform? node;
            try { node = queue.Dequeue(); }
            catch { continue; }
            if (!NetworkGuard.IsAlive(node)) continue;
            visited++;
            ToggleableLight? toggle = null;
            try { toggle = node!.GetComponent<ToggleableLight>(); }
            catch { }
            if (NetworkGuard.IsAlive(toggle) && IsVerifiedFamily(toggle!)) return toggle;
            int children;
            try { children = node!.childCount; }
            catch { children = 0; }
            for (int index = 0; index < children && queue.Count < 64; index++)
            {
                try { queue.Enqueue(node!.GetChild(index)); }
                catch { }
            }
        }
        return null;
    }

    private static bool _tentLightLogged;

    /// <summary>One line when a grow tent has no verified built-in toggle, so the next log names the real materials.</summary>
    private static void LogUnverifiedTentLight(Pot pot)
    {
        if (_tentLightLogged) return;
        _tentLightLogged = true;
        string on = "<none>";
        string off = "<none>";
        Transform? root;
        try { root = pot.transform; }
        catch { root = null; }
        if (NetworkGuard.IsAlive(root))
        {
            var queue = new Queue<Transform>();
            queue.Enqueue(root!);
            int visited = 0;
            while (queue.Count > 0 && visited < 64)
            {
                Transform? node;
                try { node = queue.Dequeue(); }
                catch { continue; }
                if (!NetworkGuard.IsAlive(node)) continue;
                visited++;
                ToggleableLight? toggle = null;
                try { toggle = node!.GetComponent<ToggleableLight>(); }
                catch { }
                if (NetworkGuard.IsAlive(toggle))
                {
                    try { on = toggle!.lightOnMat != null ? toggle.lightOnMat.name : "<null>"; }
                    catch { on = "<unreadable>"; }
                    try { off = toggle!.lightOffMat != null ? toggle.lightOffMat.name : "<null>"; }
                    catch { off = "<unreadable>"; }
                    break;
                }
                int children;
                try { children = node!.childCount; }
                catch { children = 0; }
                for (int index = 0; index < children && queue.Count < 64; index++)
                {
                    try { queue.Enqueue(node!.GetChild(index)); }
                    catch { }
                }
            }
        }
        Log.Info($"A grow tent was found, but its built-in light is not a verified panel pair (on '{on}', off '{off}'). It is left untouched.");
    }

    /// <summary>
    /// True while the lamp is alive, owned by the player, and both coupled outputs are still live: a
    /// resolvable emitting LED panel slot <i>and</i> a resolvable native light source. Coupled control
    /// fails closed, so a lamp that lost either output is demoted (state retained) instead of being
    /// driven on one output only.
    /// </summary>
    private static bool IsControllable(OwnedLamp lamp)
    {
        var anchor = LiveAnchor(lamp);
        if (anchor == null) return false;
        if (IsParentPropertyDestroyed(anchor)) return false;
        if (!IsOwnedByPlayer(anchor, lamp.PropertyCode)) return false;
        bool surface = false;
        foreach (var owned in lamp.Surfaces)
            if (NetworkGuard.IsAlive(owned.Renderer)) { surface = true; break; }
        if (!surface) return false;
        foreach (var source in lamp.Lights)
            if (NetworkGuard.IsAlive(source.Handle)) return true;
        return false;
    }

    /// <summary>
    /// Ownership gate: the placed lamp's live parent property must be one the player owns, either by
    /// its live <c>IsOwned</c> flag or by appearing in the native owned-property list. A lamp without
    /// a live, owned parent property is never controlled.
    /// </summary>
    private static bool IsOwnedByPlayer(Il2CppScheduleOne.EntityFramework.BuildableItem item, string? cachedCode)
    {
        NativeProperty? property;
        try { property = item.ParentProperty; }
        catch { return false; }
        if (!NetworkGuard.IsAlive(property)) return false;

        string code;
        bool owned;
        try
        {
            code = property!.propertyCode ?? string.Empty;
            owned = property.IsOwned;
        }
        catch { return false; }
        if (string.IsNullOrEmpty(code)) code = cachedCode ?? string.Empty;
        return PropertyOwnershipPolicy.IsOwnedProperty(code, owned, _ownedPropertyCodes);
    }

    /// <summary>Reads the live property code of an owned parent property, or fails.</summary>
    private static bool TryReadOwnedPropertyCode(Il2CppScheduleOne.EntityFramework.BuildableItem item, out string propertyCode)
    {
        propertyCode = string.Empty;
        NativeProperty? property;
        try { property = item.ParentProperty; }
        catch { return false; }
        if (!NetworkGuard.IsAlive(property)) return false;
        try
        {
            string? code = property!.propertyCode;
            if (string.IsNullOrEmpty(code)) return false;
            if (!PropertyOwnershipPolicy.IsOwnedProperty(code, property.IsOwned, _ownedPropertyCodes)) return false;
            propertyCode = code;
            return true;
        }
        catch { return false; }
    }

    /// <summary>Moves a lamp out of the active list while keeping its record for a later rebind.</summary>
    private static void Demote(OwnedLamp lamp, string reason)
    {
        foreach (var surface in lamp.Surfaces) RestoreOwned(surface);
        foreach (var source in lamp.Lights) RestoreLightSource(source);
        lamp.Available = false;
        if (lamp.Identity is not null && _store is not null)
        {
            try { _store.MarkTemporarilyUnloaded(lamp.Identity); }
            catch (Exception ex) { Log.Warn("Marking a grow lamp temporarily unloaded failed", ex); }
        }
        Order.Remove(lamp);
        Log.Info($"Grow lamp is no longer controllable ({reason}); its state is retained for a later rebind.");
    }

    /// <summary>
    /// Rebinds a temporarily unloaded lamp to newly discovered native outputs of the same GUID. Coupled
    /// mode fails closed here too: the lamp is only made available again once both its emitting LED
    /// panel and its own native light source resolve, otherwise it stays pending for a bounded retry.
    /// </summary>
    private static bool Rebind(OwnedLamp lamp, GrowLight growLight, string propertyCode)
    {
        if (lamp.Identity is null || _store is null) return false;
        var surfaces = CollectSurfaces(growLight, ActiveSurfacePointers(lamp));
        var lights = CollectLightSources(growLight, ActiveLightPointers(lamp));
        LampCouplingState coupling = NativeCouplingPolicy.Decide(surfaces.Count > 0, lights.Count > 0, ReadLampSwitch(growLight));
        if (coupling == LampCouplingState.Pending)
        {
            MarkPendingCoupling(lamp.Identity.Key, growLight, propertyCode);
            return false;
        }
        if (coupling == LampCouplingState.Unsupported)
        {
            ReportUnsupportedCoupling(lamp.Identity.Key);
            return false;
        }

        lamp.Generation++;
        string runtimeId = RuntimeInstanceId(lamp.Identity, lamp.Generation);
        try { _store.RebindTemporarilyUnloaded(lamp.Identity, runtimeId); }
        catch (Exception ex) { Log.Warn("Rebinding a grow lamp failed", ex); return false; }

        foreach (var surface in lamp.Surfaces) ReleaseOwned(surface);
        foreach (var source in lamp.Lights) RestoreLightSource(source);
        lamp.Surfaces.Clear();
        lamp.Lights.Clear();
        foreach (var surface in surfaces) lamp.Surfaces.Add(surface);
        foreach (var source in lights) lamp.Lights.Add(source);
        lamp.GrowLight = growLight;
        lamp.PropertyCode = propertyCode;
        lamp.Available = true;
        PendingCoupling.Remove(lamp.Key);
        ClearUnsupportedCoupling(lamp.Key);
        if (!Order.Contains(lamp))
        {
            Order.Add(lamp);
            Order.Sort(static (left, right) => string.CompareOrdinal(left.Key, right.Key));
        }
        Log.Info($"Rebound a previously unloaded grow lamp ({Order.Count} active).");
        return true;
    }

    /// <summary>Native surface pointers another active lamp already owns, so none is owned twice.</summary>
    private static HashSet<long> ActiveSurfacePointers(OwnedLamp? exclude)
    {
        var pointers = new HashSet<long>();
        foreach (var lamp in Order)
        {
            if (ReferenceEquals(lamp, exclude)) continue;
            foreach (var surface in lamp.Surfaces)
            {
                var renderer = surface.Renderer;
                if (!NetworkGuard.IsAlive(renderer)) continue;
                try { pointers.Add(renderer!.Pointer.ToInt64()); } catch { }
            }
        }
        return pointers;
    }

    /// <summary>
    /// Native light pointers another active lamp already owns, so one Unity light shared by two fixtures
    /// (or listed twice) is never driven twice. Identity is the native pointer, never a managed
    /// reference: IL2CPP hands out a fresh wrapper per read.
    /// </summary>
    private static HashSet<long> ActiveLightPointers(OwnedLamp? exclude)
    {
        var pointers = new HashSet<long>();
        foreach (var lamp in Order)
        {
            if (ReferenceEquals(lamp, exclude)) continue;
            foreach (var source in lamp.Lights)
            {
                var light = source.Handle;
                if (!NetworkGuard.IsAlive(light)) continue;
                try { pointers.Add(light!.Pointer.ToInt64()); } catch { }
            }
        }
        return pointers;
    }

    private static string RuntimeInstanceId(LampIdentity identity, int generation) => $"{identity.Key}#{generation}";

    /// <summary>
    /// Refreshes the cached native owned-property code list. The live <c>IsOwned</c> flag is the
    /// per-frame signal; this list is the fallback and is refreshed at the reconcile cadence.
    /// </summary>
    private static void RefreshOwnedPropertyCodes()
    {
        var codes = new List<string>();
        try
        {
            var owned = NativeProperty.OwnedProperties;
            if (owned is not null)
            {
                for (int index = 0; index < owned.Count; index++)
                {
                    NativeProperty? property;
                    try { property = owned[index]; } catch { continue; }
                    if (!NetworkGuard.IsAlive(property)) continue;
                    try
                    {
                        string? code = property!.propertyCode;
                        if (!string.IsNullOrEmpty(code)) codes.Add(code);
                    }
                    catch { }
                }
            }
        }
        catch (Exception ex) { Log.Warn("Reading the native owned-property list failed", ex); }
        _ownedPropertyCodes = codes;
    }

    // ---------------------------------------------------------------------------------------
    // Cooperative bounded discovery
    // ---------------------------------------------------------------------------------------

    /// <summary>
    /// Starts one cooperative scan cycle: a snapshot of the owned-property roots and the active
    /// scene roots is queued once, and the traversal is then stepped with a per-frame node/time
    /// budget. No recursive whole-subtree component query is used; each visited node is inspected
    /// with a single <c>GetComponent&lt;GrowLight&gt;</c> and its children are enqueued by index. The
    /// start root rotates across cycles so no part of the scene is always starved.
    /// </summary>
    private static void BeginScan()
    {
        ScanQueue.Clear();
        ScanVisited.Clear();
        _scanVisitedCount = 0;
        _scanFoundCount = 0;
        _scanElapsedMilliseconds = 0;
        _scanTruncated = false;
        _scanActive = false;

        try
        {
            foreach (var property in OwnedPropertyRoots())
                Enqueue(property);
            var scene = SceneManager.GetActiveScene();
            if (!scene.isLoaded) { Log.Info("Bounded scan started: no loaded active scene."); return; }
            Il2CppReferenceArray<GameObject>? roots = scene.GetRootGameObjects();
            if (roots is null) { Log.Info("Bounded scan started: the active scene returned no roots."); return; }
            var order = SceneScanBudget.FairRootOrder(roots.Length, _scanRootCursor);
            _scanRootCursor = roots.Length == 0 ? 0 : (_scanRootCursor + 1) % roots.Length;
            for (int index = 0; index < order.Length; index++)
            {
                GameObject? root;
                try { root = roots[order[index]]; }
                catch { continue; }
                if (!NetworkGuard.IsAlive(root)) continue;
                try { Enqueue(root!.transform); } catch { }
            }
            _scanActive = ScanQueue.Count > 0;
        }
        catch (Exception ex)
        {
            _scanActive = false;
            if (!_scanFailureLogged) { _scanFailureLogged = true; Log.Warn("Starting the bounded scene scan failed", ex); }
        }
        Log.Info($"Bounded scan started: {ScanQueue.Count} root(s) queued.");
    }

    private static IEnumerable<Transform> OwnedPropertyRoots()
    {
        var roots = new List<Transform>();
        try
        {
            var owned = NativeProperty.OwnedProperties;
            if (owned is null) return roots;
            for (int index = 0; index < owned.Count; index++)
            {
                NativeProperty? property;
                try { property = owned[index]; } catch { continue; }
                if (!NetworkGuard.IsAlive(property)) continue;
                try { roots.Add(property!.transform); } catch { }
            }
        }
        catch (Exception ex) { Log.Warn("Enumerating owned-property roots failed", ex); }
        return roots;
    }

    private static void Enqueue(Transform? node)
    {
        if (node is null) return;
        try { if (NetworkGuard.IsAlive(node)) ScanQueue.Enqueue(node!); } catch { }
    }

    /// <summary>Steps the active scan cycle within the per-frame node/time budget.</summary>
    private static void ScanStep()
    {
        if (!_scanActive) return;
        int processed = 0;
        ScanWatch.Restart();
        while (ScanQueue.Count > 0)
        {
            if (!ScanBudget.CanVisit(processed, ScanWatch.Elapsed.TotalMilliseconds)) break;
            if (ScanBudget.IsTruncated(_scanVisitedCount)) { _scanTruncated = true; _scanActive = false; break; }

            Transform? node;
            try { node = ScanQueue.Dequeue(); } catch { continue; }
            if (!NetworkGuard.IsAlive(node)) continue;
            long key;
            try { key = node!.Pointer.ToInt64(); } catch { continue; }
            if (key == 0 || !ScanVisited.Add(key)) continue;
            _scanVisitedCount++;
            processed++;

            try
            {
                var growLight = node!.GetComponent<GrowLight>();
                if (growLight != null && TryRegister(growLight)) _scanFoundCount++;
                string? nodeName = null;
                try { nodeName = node!.name; } catch { }
                if (nodeName != null && (nodeName.Contains("GrowTent") || nodeName.Contains("Grow Tent")))
                {
                    var pot = node!.GetComponent<Pot>();
                    if (pot != null && TryRegisterGrowTent(pot)) _scanFoundCount++;
                }
            }
            catch (Exception ex)
            {
                if (!_scanFailureLogged) { _scanFailureLogged = true; Log.Warn("Inspecting a scene node failed", ex); }
            }

            int children;
            try { children = node!.childCount; } catch { children = 0; }
            for (int index = 0; index < children; index++)
            {
                try { Enqueue(node!.GetChild(index)); } catch { }
            }
        }
        _scanElapsedMilliseconds += ScanWatch.Elapsed.TotalMilliseconds;

        if (ScanQueue.Count == 0)
        {
            _scanActive = false;
            string truncated = _scanTruncated ? ", TRUNCATED at the node cap" : string.Empty;
            Log.Info($"Scan complete: {_scanFoundCount} grow lamp(s) registered, {_scanVisitedCount} node(s) visited, {_scanElapsedMilliseconds:F1} ms total{truncated}.");
        }
    }

    /// <summary>
    /// Registers or rebinds exactly one live, initialized, owned placed grow lamp with a nonempty
    /// native GUID and both coupled outputs resolvable: at least one verified LED panel slot and at least
    /// one of the lamp's own native light sources. An item definition id or a runtime instance id is never
    /// accepted as an identity, and a lamp on a property the player does not own is never controlled.
    /// A lamp whose light has not resolved yet is retained as pending and retried, never registered
    /// panel-only and never switched on by the mod.
    /// </summary>
    private static bool TryRegister(GrowLight? growLight)
    {
        if (!Ready || _store is null) return false;
        if (!NetworkGuard.IsAlive(growLight)) return false;

        string guidText;
        bool destroyed;
        bool initialized;
        try
        {
            guidText = growLight!.GUID.ToString();
            destroyed = growLight.IsDestroyed;
            initialized = growLight.Initialized;
        }
        catch (Exception ex) { LogWarnOnce("read native grow-lamp identity", ex); return false; }
        if (destroyed || !initialized) return false;

        var native = LampIdentityPolicy.FromNativeGuid(guidText);
        LampIdentity? identity = native.Identity;
        if (!native.Accepted || identity is null) return false;
        if (IsParentPropertyDestroyed(growLight!))
        {
            if (Lamps.TryGetValue(identity.Key, out var stale)) RemoveDestroyed(stale);
            return false;
        }
        if (!TryReadOwnedPropertyCode(growLight!, out string propertyCode)) return false;

        if (Lamps.TryGetValue(identity.Key, out var known))
            return !known.Available && Rebind(known, growLight!, propertyCode);

        var surfaces = CollectSurfaces(growLight!, ActiveSurfacePointers(null));
        var lights = CollectLightSources(growLight!, ActiveLightPointers(null));
        LampCouplingState coupling = NativeCouplingPolicy.Decide(surfaces.Count > 0, lights.Count > 0, ReadLampSwitch(growLight!));
        if (coupling == LampCouplingState.Pending)
        {
            MarkPendingCoupling(identity.Key, growLight!, propertyCode);
            return false;
        }
        if (coupling == LampCouplingState.Unsupported)
        {
            ReportUnsupportedCoupling(identity.Key);
            return false;
        }

        var registration = new LampRegistration(identity, RuntimeInstanceId(identity, 0), propertyCode);
        try { _store.Register(registration, Now); }
        catch (Exception ex) { Log.Warn("Lamp registration rejected", ex); return false; }

        var lamp = new OwnedLamp
        {
            Key = identity.Key,
            Identity = identity,
            GrowLight = growLight,
            PropertyCode = propertyCode,
            Generation = 0,
            Available = true
        };
        foreach (var surface in surfaces) lamp.Surfaces.Add(surface);
        foreach (var source in lights) lamp.Lights.Add(source);
        Lamps.Add(lamp.Key, lamp);
        PendingCoupling.Remove(lamp.Key);
        ClearUnsupportedCoupling(lamp.Key);
        Order.Add(lamp);
        Order.Sort(static (left, right) => string.CompareOrdinal(left.Key, right.Key));
        return true;
    }

    /// <summary>
    /// Retains a lamp whose coupled outputs are not both resolvable yet - typically a lamp that is off,
    /// whose native light has not resolved. The entry is not registered, is never shown in the phone app
    /// and is never used to enable or switch on the lamp; it is retried at a bounded cadence instead. The
    /// set is capped so an unreadable fixture cannot grow it without bound.
    /// </summary>
    private static void MarkPendingCoupling(string key, GrowLight? growLight, string? propertyCode, Pot? container = null)
    {
        if (!PendingCoupling.ContainsKey(key) && PendingCoupling.Count >= PendingCouplingCap) return;
        PendingCoupling[key] = new PendingLamp { GrowLight = growLight, Container = container, PropertyCode = propertyCode };
    }

    /// <summary>
    /// Reports a lamp that is switched on but exposes no readable native light source. Coupled mode fails
    /// closed: the lamp is left completely untouched rather than being driven panel-only, and the phone
    /// app surfaces the count, so the gap is never presented as working control. Counted per distinct
    /// lamp, never once per retry.
    /// </summary>
    private static void ReportUnsupportedCoupling(string key)
    {
        PendingCoupling.Remove(key);
        if (UnsupportedCoupling.Add(key)) UnsupportedLampCount = UnsupportedCoupling.Count;
        if (_couplingSkipLogged) return;
        _couplingSkipLogged = true;
        Log.Info("A grow lamp is switched on but exposes no readable native light source; coupled control fails closed and the lamp is left completely untouched (no panel-only fallback).");
    }

    /// <summary>
    /// Drops a lamp from the unsupported report once it is controllable again, so the phone app never
    /// shows a stale gap.
    /// </summary>
    private static void ClearUnsupportedCoupling(string key)
    {
        if (!UnsupportedCoupling.Remove(key)) return;
        UnsupportedLampCount = UnsupportedCoupling.Count;
    }

    /// <summary>
    /// Bounded retry for lamps whose coupled outputs were not both resolvable yet: at most one pass per
    /// <see cref="PendingCouplingRetrySeconds"/>, never per frame. An entry whose lamp is gone is dropped,
    /// and a lamp is only ever registered once both outputs resolve.
    /// </summary>
    private static void RetryPendingCoupling()
    {
        if (PendingCoupling.Count == 0) return;
        double now = Now;
        if (now < _nextPendingCoupling) return;
        _nextPendingCoupling = now + PendingCouplingRetrySeconds;

        // A snapshot of the keys: TryRegister can add or remove entries through the coupling decision.
        var keys = new List<string>(PendingCoupling.Keys);
        List<string>? stale = null;
        foreach (string key in keys)
        {
            if (!PendingCoupling.TryGetValue(key, out var entry)) continue;
            var growLight = entry.GrowLight;
            var container = entry.Container;
            bool lightDead = !NetworkGuard.IsAlive(growLight) || IsGenuinelyDestroyed(growLight);
            bool tentDead = container is null || !NetworkGuard.IsAlive(container) || IsAnchorDestroyed(container) || IsParentPropertyDestroyed(container);
            if (container is not null)
            {
                if (tentDead) (stale ??= new()).Add(key);
                else TryRegisterGrowTent(container);
            }
            else if (lightDead || IsParentPropertyDestroyed(growLight))
                (stale ??= new()).Add(key);
            else TryRegister(growLight);
        }
        if (stale is null) return;
        foreach (string key in stale) PendingCoupling.Remove(key);
    }

    /// <summary>Reads the lamp's live native switch state. An unreadable state is reported as off.</summary>
    private static bool ReadLampSwitch(GrowLight growLight)
    {
        ToggleableLight? toggle;
        try { toggle = growLight.Light; }
        catch { return false; }
        if (!NetworkGuard.IsAlive(toggle)) return false;
        try { return toggle!.isOn; }
        catch { return false; }
    }

    /// <summary>
    /// Collects only the lamp's proven emitting LED surface slots: the
    /// <c>ToggleableLight.MaterialIndex</c> slot of each <c>lightSurfacesMeshes</c> renderer, and only
    /// while that slot holds a verified LED panel material. Shared renderer pointers are
    /// de-duplicated, so one surface is never owned twice. Light sources, other material slots,
    /// shader properties, culling flags and switch state are never touched.
    /// </summary>
    private static List<OwnedSurface> CollectSurfaces(GrowLight growLight, IReadOnlyCollection<long> alreadyOwned)
    {
        ToggleableLight? toggle;
        try { toggle = growLight.Light; }
        catch (Exception ex) { LogWarnOnce("read the grow-lamp toggle", ex); return new(); }
        if (!NetworkGuard.IsAlive(toggle)) return new();
        return CollectSurfaces(toggle!, alreadyOwned);
    }

    private static List<OwnedSurface> CollectSurfaces(ToggleableLight toggle, IReadOnlyCollection<long> alreadyOwned)
    {
        var candidates = new List<OwnedSurface>();
        var pointers = new List<long>();
        if (!NetworkGuard.IsAlive(toggle)) return candidates;

        // Family gate first: an unverified grow-lamp family is never registered, so its panel,
        // housing and materials stay exactly as the game built them.
        if (!IsVerifiedFamily(toggle!))
        {
            if (!_familySkipLogged)
            {
                _familySkipLogged = true;
                Log.Info("A grow lamp is outside the verified LED and grow-tent halogen families; it is left completely untouched.");
            }
            return candidates;
        }

        Il2CppReferenceArray<MeshRenderer>? surfaces;
        int slot;
        try
        {
            surfaces = toggle!.lightSurfacesMeshes;
            slot = toggle.MaterialIndex;
        }
        catch (Exception ex) { LogWarnOnce("read the grow-lamp light surfaces", ex); return candidates; }
        if (surfaces is null) return candidates;

        for (int index = 0; index < surfaces.Length; index++)
        {
            MeshRenderer? renderer;
            try { renderer = surfaces[index]; } catch { continue; }
            if (!NetworkGuard.IsAlive(renderer)) continue;

            Il2CppReferenceArray<Material>? materials;
            try { materials = renderer!.sharedMaterials; } catch { continue; }
            if (materials is null || slot < 0 || slot >= materials.Length) continue;

            Material? material;
            try { material = materials[slot]; } catch { continue; }
            if (!IsSupportedPanel(material)) continue;

            long pointer;
            try { pointer = renderer!.Pointer.ToInt64(); } catch { continue; }
            candidates.Add(new OwnedSurface { Renderer = renderer });
            pointers.Add(pointer);
        }

        var keep = LightPointerDedupe.SelectUnowned(pointers, alreadyOwned);
        var result = new List<OwnedSurface>(keep.Count);
        foreach (int index in keep) result.Add(candidates[index]);
        return result;
    }

    /// <summary>
    /// Collects only the lamp's own proven native light sources: <c>lightSources[i]._Light</c> of the
    /// lamp's verified <c>ToggleableLight</c>. The serialized <c>_Light</c> reference is null in the
    /// shipped prefab and resolves at runtime, so a lamp that is off may legitimately expose no source -
    /// that is reported to the coupling policy as "not resolvable", never worked around. Zero, unreadable
    /// and dead pointers are skipped, and native pointers are de-duplicated against the lamp's own list
    /// and every other active lamp, so one Unity light is never driven twice. Nothing but
    /// <c>color</c>/<c>intensity</c> is ever written on a collected source.
    /// </summary>
    private static List<OwnedLightSource> CollectLightSources(GrowLight growLight, IReadOnlyCollection<long> alreadyOwned)
    {
        ToggleableLight? maybeToggle;
        try { maybeToggle = growLight.Light; }
        catch (Exception ex) { LogWarnOnce("read the grow-lamp toggle", ex); return new(); }
        if (!NetworkGuard.IsAlive(maybeToggle)) return new();
        return CollectLightSources(maybeToggle!, alreadyOwned);
    }

    private static List<OwnedLightSource> CollectLightSources(ToggleableLight toggle, IReadOnlyCollection<long> alreadyOwned)
    {
        var candidates = new List<OwnedLightSource>();
        var pointers = new List<long>();
        if (!NetworkGuard.IsAlive(toggle)) return candidates;

        Il2CppReferenceArray<OptimizedLight>? sources;
        try { sources = toggle.lightSources; }
        catch (Exception ex) { LogWarnOnce("read the grow-lamp light sources", ex); return candidates; }
        if (sources is null) return candidates;

        for (int index = 0; index < sources.Length; index++)
        {
            OptimizedLight? source;
            try { source = sources[index]; } catch { continue; }
            if (!NetworkGuard.IsAlive(source)) continue;

            Light? light;
            try { light = source!._Light; } catch { continue; }
            if (!NetworkGuard.IsAlive(light)) continue;

            long pointer;
            try { pointer = light!.Pointer.ToInt64(); } catch { continue; }
            if (pointer == 0) continue;

            candidates.Add(new OwnedLightSource { Handle = light });
            pointers.Add(pointer);
        }

        var keep = LightPointerDedupe.SelectUnowned(pointers, alreadyOwned);
        var result = new List<OwnedLightSource>(keep.Count);
        foreach (int index in keep) result.Add(candidates[index]);
        return result;
    }

    // ---------------------------------------------------------------------------------------
    // Persistence handoff
    // ---------------------------------------------------------------------------------------

    private static void Autosave(double now)
    {
        if (_store is null) return;
        if (_store.Revision == _savedRevision) return;
        if (now < _nextAutosave) return;
        _nextAutosave = now + AutosaveIntervalSeconds;
        Persist();
    }

    /// <summary>
    /// Writes once and advances the saved revision only after a successful write, so a failed I/O is
    /// retried at the next bounded autosave instead of being silently skipped.
    /// </summary>
    private static void Persist()
    {
        if (_store is null || _persistence is null || _storage is null) return;
        if (_persistence.Gate != PersistenceGate.Ready) return;
        long revision = _store.Revision;
        if (revision == _savedRevision) return;
        if (_storage.Save(_persistence))
        {
            _savedRevision = revision;
            LastSaveFailed = false;
            SaveConfirmed = true;
        }
        else
        {
            LastSaveFailed = true;
            Log.Warn("Lamp data was not written; existing storage is retained and the next autosave retries.");
        }
    }

    // ---------------------------------------------------------------------------------------
    // Phone-app surface
    // ---------------------------------------------------------------------------------------

    /// <summary>A read-only view of one registered lamp for the phone app.</summary>
    public readonly record struct LampView(
        int Index,
        int Count,
        string Label,
        ColorMode Color,
        IntensityLevel Intensity,
        string Kind,
        string PropertyCode,
        string PropertyName,
        string Key);

    public static bool TryGetLamp(int index, out LampView view)
    {
        view = default;
        if (!Ready || _store is null || index < 0 || index >= Order.Count) return false;
        var lamp = Order[index];
        if (lamp.Identity is null) return false;
        LampBaseState state;
        try { state = _store.GetBaseState(lamp.Identity); }
        catch { return false; }
        string code = lamp.PropertyCode ?? string.Empty;
        string name = ReadPropertyName(lamp, code);
        view = new LampView(index, Order.Count, $"{lamp.Kind} {index + 1}", state.Color, state.Intensity, lamp.Kind, code, name, lamp.Identity.Key);
        return true;
    }

    /// <summary>Copies the selected lamp onto every lamp of one owned property. Other properties stay put.</summary>
    public static bool ApplyToProperty(int index, string? propertyCode)
    {
        if (!TryGetLamp(index, out var view)) return false;
        if (string.IsNullOrWhiteSpace(propertyCode)) return false;
        return Execute(ControlCommand.Set(
            LampSelection.Property(propertyCode.Trim()),
            new LampChange(view.Color, view.Intensity))).Succeeded;
    }

    private static string ReadPropertyName(OwnedLamp lamp, string code)
    {
        var anchor = LiveAnchor(lamp);
        if (NetworkGuard.IsAlive(anchor))
        {
            try
            {
                var property = anchor!.ParentProperty;
                if (NetworkGuard.IsAlive(property))
                {
                    string name = property!.PropertyName;
                    if (!string.IsNullOrWhiteSpace(name)) return name.Trim();
                }
            }
            catch { /* the code is still a usable label */ }
        }
        return string.IsNullOrWhiteSpace(code) ? "Property" : code.Trim();
    }

    public readonly record struct LampActionResult(CommandStatus Status, int AppliedCount)
    {
        public bool Succeeded => Status is CommandStatus.Applied or CommandStatus.Unchanged;
        public static LampActionResult Rejected => new(CommandStatus.UnknownTarget, 0);
    }

    public readonly record struct CopyPreview(Guid SessionId, string SourceKey, string? PropertyCode, ColorMode Color, IntensityLevel Intensity, string[] TargetKeys);

    public static bool TryBuildPreview(string sourceKey, bool world, out CopyPreview preview)
    {
        preview = default;
        if (!Ready || _store is null) return false;
        if (Find(sourceKey) is not OwnedLamp lamp || lamp.Identity is null) return false;
        string code = (lamp.PropertyCode ?? string.Empty).Trim();
        if (!world && code.Length == 0) return false;
        LampBaseState state;
        try { state = _store.GetBaseState(lamp.Identity); }
        catch { return false; }
        string[] keys;
        try
        {
            keys = _store.AvailableTargetKeys(world ? LampSelection.World() : LampSelection.Property(code));
        }
        catch { return false; }
        preview = new CopyPreview(_store.SessionId, lamp.Identity.Key, world ? null : code, state.Color, state.Intensity, keys);
        return true;
    }

    public static LampActionResult ApplyPreview(CopyPreview shown, bool world)
    {
        if (!TryBuildPreview(shown.SourceKey, world, out var current)) return LampActionResult.Rejected;
        if (!UiSelectionPolicy.PreviewMatches(
            shown.SessionId, shown.SourceKey, shown.PropertyCode, shown.Color, shown.Intensity, shown.TargetKeys,
            current.SessionId, current.SourceKey, current.PropertyCode, current.Color, current.Intensity, current.TargetKeys))
            return new LampActionResult(CommandStatus.StaleRevision, 0);
        var selection = world ? LampSelection.World() : LampSelection.Property(current.PropertyCode!);
        return Execute(ControlCommand.Set(selection, new LampChange(current.Color, current.Intensity)));
    }

    public static LampActionResult SetColor(string key, ColorMode color)
    {
        if (!Enum.IsDefined(typeof(ColorMode), color)) return LampActionResult.Rejected;
        if (!TryState(key, out var lamp, out var state)) return LampActionResult.Rejected;
        if (state.Color == color) return new LampActionResult(CommandStatus.Unchanged, 1);
        return Execute(ControlCommand.Set(LampSelection.Lamp(lamp.Identity!), new LampChange(color: color)));
    }

    public static LampActionResult SetIntensity(string key, IntensityLevel level)
    {
        if (!Enum.IsDefined(typeof(IntensityLevel), level)) return LampActionResult.Rejected;
        if (!TryState(key, out var lamp, out var state)) return LampActionResult.Rejected;
        if (state.Intensity == level) return new LampActionResult(CommandStatus.Unchanged, 1);
        return Execute(ControlCommand.Set(LampSelection.Lamp(lamp.Identity!), new LampChange(intensity: level)));
    }

    public static LampActionResult ResetLamp(string key)
    {
        if (!TryState(key, out var lamp, out _)) return LampActionResult.Rejected;
        var reset = UiSelectionPolicy.OriginalReset();
        return Execute(ControlCommand.Set(LampSelection.Lamp(lamp.Identity!), new LampChange(reset.Color, reset.Intensity)));
    }

    private static OwnedLamp? Find(string key)
    {
        for (int i = 0; i < Order.Count; i++)
        {
            if (string.Equals(Order[i].Key, key, StringComparison.Ordinal) || Order[i].Identity?.Key == key)
                return Order[i];
        }
        return null;
    }

    private static bool TryState(string key, out OwnedLamp lamp, out LampBaseState state)
    {
        state = LampBaseState.Default;
        var found = Find(key);
        if (found?.Identity is not LampIdentity identity || !Ready || _store is null)
        {
            lamp = found ?? new OwnedLamp();
            return false;
        }
        lamp = found;
        try { state = _store.GetBaseState(identity); return true; }
        catch { return false; }
    }

    /// <summary>Sets the selected lamp to one palette color without cycling. Intensity stays.</summary>
    public static bool SetColor(int index, ColorMode color)
    {
        if (!TryGetLamp(index, out var view)) return false;
        return SetColor(view.Key, color).Succeeded;
    }

    /// <summary>Advances the selected lamp's color through all eleven core modes.</summary>
    public static bool CycleColor(int index)
    {
        if (!TryGetLamp(index, out var view)) return false;
        return Execute(ControlCommand.Set(LampSelection.Lamp(Order[view.Index].Identity!), new LampChange(color: UiSelectionPolicy.NextColor(view.Color)))).Succeeded;
    }

    /// <summary>Advances the selected lamp's intensity through all seven core levels.</summary>
    public static bool CycleIntensity(int index)
    {
        if (!TryGetLamp(index, out var view)) return false;
        return Execute(ControlCommand.Set(LampSelection.Lamp(Order[view.Index].Identity!), new LampChange(intensity: UiSelectionPolicy.NextIntensity(view.Intensity)))).Succeeded;
    }

    /// <summary>Restores the selected lamp to the model default: original color, 100%.</summary>
    public static bool ResetOriginal(int index)
    {
        if (!TryGetLamp(index, out var view)) return false;
        var reset = UiSelectionPolicy.OriginalReset();
        return Execute(ControlCommand.Set(LampSelection.Lamp(Order[view.Index].Identity!), new LampChange(reset.Color, reset.Intensity))).Succeeded;
    }

    /// <summary>Copies the selected lamp's state onto every supported owned lamp.</summary>
    public static bool ApplyToAll(int index)
    {
        if (!TryGetLamp(index, out var view)) return false;
        return Execute(ControlCommand.Set(LampSelection.World(), new LampChange(view.Color, view.Intensity))).Succeeded;
    }

    /// <summary>
    /// Rechecks ownership of every active lamp, then applies the command. The local host is its own
    /// verified sender: the authorization callback only ever sees the frozen target set of the pure
    /// model and always runs outside a mutation.
    /// </summary>
    private static LampActionResult Execute(ControlCommand command)
    {
        if (!Ready || _store is null) return LampActionResult.Rejected;
        RecheckOwnership();
        try
        {
            var request = new ClientCommand(LightLimits.ProtocolVersion, _store.SessionId, Guid.NewGuid(), _store.Revision, command);
            var result = _store.ExecuteHost(request, LocalSenderId, _ => true, Now);
            if (result.Status is not (CommandStatus.Applied or CommandStatus.Unchanged))
            {
                Log.Warn($"Local command was not applied: {result.Status}.");
                return new LampActionResult(result.Status, 0);
            }
            int count = result.Status == CommandStatus.Applied ? result.Delta?.Upserts.Count ?? 0 : 0;
            return new LampActionResult(result.Status, count);
        }
        catch (Exception ex) { Log.Warn("Local host command failed", ex); return LampActionResult.Rejected; }
    }

    /// <summary>Demotes any active lamp whose property is no longer owned, before a command is applied.</summary>
    private static void RecheckOwnership()
    {
        List<OwnedLamp>? demoted = null;
        foreach (var lamp in Order)
            if (!IsControllable(lamp)) (demoted ??= new()).Add(lamp);
        if (demoted is null) return;
        foreach (var lamp in demoted)
            Demote(lamp, "its parent property is no longer owned");
    }
}
