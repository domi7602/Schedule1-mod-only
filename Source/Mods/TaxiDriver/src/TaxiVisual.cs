using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Il2CppScheduleOne.Vehicles;
using MelonLoader.Utils;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// The taxi keeps the vanilla <c>Shitbox</c> body, colliders, wheel colliders
/// and physics. Its GLB (<c>assets/taxi.glb</c>) is attached as a visual child of
/// <see cref="LandVehicle.vehicleModel"/>; vanilla renderers/LOD visibility are
/// hidden only after a usable GLB renderer is confirmed, and vanilla GameObjects
/// remain active.
///
/// Loader: <c>S1MAPI.Gltf.GltfLoader.LoadFromFile</c> (UserLibs\S1MAPI_Il2cpp.dll) —
/// the same runtime GLB pipeline that AutoPackagingStation / SnackVendor used,
/// no AssetBundle, no Unity round-trip.
///
/// No vanilla GameObject is deactivated: only renderer and LOD visibility are
/// changed, so physics and functional scripts remain active.
/// </summary>
internal static class TaxiVisual
{
    /// <summary>Diagnostic trace: printed only when log.json enables verbose logging.</summary>
    private static void Trace(string msg) => TaxiLog.Verbose(msg);

    /// <summary>MelonLoader mod folder name (also the UserData subfolder).</summary>
    internal const string ModFolderName = "TaxiDriver";

    /// <summary>Asset file name under the mod folder.</summary>
    internal const string AssetFileName = "taxi.glb";

    /// <summary>Name given to the loaded GLB root so it is identifiable in dumps.</summary>
    private const string VisualRootName = "TaxiDriver_TaxiVisual";

    /// <summary>Smallest bounds edge that may drive the auto-alignment (guards against garbage bounds).</summary>
    private const float MinBoundsEdgeMeters = 0.1f;

    /// <summary>
    /// A vanilla renderer wider than this is not part of the car body (POI marker,
    /// particle emitter, projector) and must not influence the alignment.
    /// </summary>
    private const float MaxBoundsEdgeMeters = 30f;

    /// <summary>
    /// Largest offset the auto-alignment may apply. Anything bigger is not a pivot
    /// difference but a stale-bounds artefact of the spawn frame (see the guard in
    /// <see cref="SwapInternal"/>), so the GLB keeps its local-zero placement.
    /// </summary>
    private const float MaxAlignOffsetMeters = 5f;

    /// <summary>Cached result of <see cref="ResolveAssetPath"/> — the file does not move during a session.</summary>
    private static string? _resolvedPath;

    /// <summary>
    /// GLB world min Y measured right before the auto-alignment moved it (NaN when
    /// the swap did not run) — one half of the 0.3.0 spawn diagnostic line in
    /// <see cref="SpikeCommands.SpawnVehicle"/>.
    /// </summary>
    internal static float LastGlbMinYBefore = float.NaN;

    /// <summary>GLB world min Y after the auto-alignment (NaN when the swap did not run).</summary>
    internal static float LastGlbMinYAfter = float.NaN;

    /// <summary>
    /// Vanilla visibility snapshot taken at the start of a swap; non-null while a
    /// swap is in flight. Cleared on success, consumed by
    /// <see cref="RollbackPartialSwap"/> when <see cref="SwapInternal"/> throws
    /// (review M2-4 — a half-applied swap must not leave the vehicle half-hidden
    /// behind a GLB that never arrived).
    /// </summary>
    private static Rollback? _rollback;

    /// <summary>
    /// Everything <see cref="RollbackPartialSwap"/> needs to undo a partial swap:
    /// the GLB that was parented plus the vanilla visibility state captured before
    /// the first mutation.
    /// </summary>
    private sealed class Rollback
    {
        internal GameObject? Glb;
        internal Transform?[] Children = Array.Empty<Transform?>();
        internal bool[] ChildActive = Array.Empty<bool>();
        internal Renderer[] Renderers = Array.Empty<Renderer>();
        internal bool[] RenderersEnabled = Array.Empty<bool>();
        internal bool[] RenderersForceOff = Array.Empty<bool>();
        internal LODGroup[] LodGroups = Array.Empty<LODGroup>();
        internal bool[] LodEnabled = Array.Empty<bool>();
    }

    // ------------------------------------------------------------------
    // Visual wheels (GLB nodes driven by native wheel state)
    // ------------------------------------------------------------------

    /// <summary>
    /// Name prefix of the imported wheel nodes. Only the prefix is used to *collect*
    /// candidates; which node belongs to which native wheel is decided by position,
    /// because the asset's FL/FR/RL/RR names and the old orientation comment disagree
    /// about which end of the car is the front.
    /// </summary>
    private const string GlbWheelNodePrefix = "Wheel";

    /// <summary>Binding attempts before a vehicle's wheels stay static for the session (no per-frame searches).</summary>
    private const int WheelResolveAttempts = 3;

    /// <summary>Seconds between the bounded binding attempts (the native wheel list may be empty on the spawn frame).</summary>
    private const float WheelResolveRetrySeconds = 1f;

    /// <summary>
    /// One imported wheel node bound to the native wheel it sits on. <see cref="RestRotation"/>
    /// is the imported mounting rotation (captured once), so every frame only composes a
    /// delta on top of it — the GLB pivot/offset is never re-guessed.
    /// </summary>
    private sealed class WheelLink
    {
        internal Transform Node = null!;
        internal Quaternion RestRotation;
        internal WheelCollider Collider = null!;
        internal Wheel Native = null!;
        internal float UpSign;

        /// <summary>Sign the steering axis would need if this wheel turns out to be steered.</summary>
        internal float SteerAxisSign;

        /// <summary>Remaining bounded probes for the native steering flag (the spawn frame may precede it).</summary>
        internal int SteerProbeTicks;

        internal float SpinDegrees;
    }

    /// <summary>Frames a freshly bound wheel may spend re-reading the native steering flag (≈1 s at 60 fps).</summary>
    private const int SteerProbeFrames = 60;

    /// <summary>Cached wheel links for the current GLB swap (empty = not bound).</summary>
    private static WheelLink[] _wheelLinks = Array.Empty<WheelLink>();

    /// <summary>The vehicle the cached links belong to (identity check only, never dereferenced for state).</summary>
    private static LandVehicle? _wheelVehicle;

    /// <summary>Rigidbody used for the native point velocity (cached once — no per-frame search).</summary>
    private static Rigidbody? _wheelBody;
    private static GameObject? _wheelRoot;
    private static bool _wheelBindRequested;

    private static float _wheelRetryAt;
    private static int _wheelResolveAttempts;
    private static bool _wheelUnavailableLogged;

    /// <summary>
    /// GLB lookup order. First hit wins and is logged once:
    /// 1. <c>UserData\TaxiDriver\assets\taxi.glb</c> — hand-install location (documented),
    /// 2. <c>Mods\TaxiDriver\taxi.glb</c> — where <c>Directory.Build.targets</c> deploys
    ///    <c>Source/Mods/TaxiDriver/assets/*.glb</c>,
    /// 3. <c>Mods\TaxiDriver\assets\taxi.glb</c> — loose install.
    /// </summary>
    internal static string[] CandidatePaths() => new[]
    {
        Path.Combine(MelonEnvironment.UserDataDirectory, ModFolderName, "assets", AssetFileName),
        Path.Combine(MelonEnvironment.ModsDirectory, ModFolderName, AssetFileName),
        Path.Combine(MelonEnvironment.ModsDirectory, ModFolderName, "assets", AssetFileName),
    };

    /// <summary>
    /// Returns the first existing GLB path (and remembers it), or null with one
    /// warning per call — a missing file is NOT cached, so every attempt warns again
    /// instead of failing silently later on (review M3-11).
    /// </summary>
    internal static string? ResolveAssetPath()
    {
        if (_resolvedPath != null && File.Exists(_resolvedPath))
            return _resolvedPath;

        string[] candidates = CandidatePaths();
        for (int i = 0; i < candidates.Length; i++)
        {
            if (!File.Exists(candidates[i]))
                continue;

            _resolvedPath = candidates[i];
            Trace($"[visual] GLB resolved: {candidates[i]}");
            return _resolvedPath;
        }

        Mod.Log.Warn(
            $"taxi.glb not found — tried {candidates.Length} locations: " +
            string.Join(" | ", candidates) +
            ". Vanilla visuals stay visible (spawn/physics unaffected).");
        return null;
    }

    /// <summary>
    /// Called once per successful <c>SpawnAndReturnVehicle</c>. Failures are logged
    /// and never abort the spawn — a missing GLB must not cost us the vehicle.
    /// Review M2-4/M2-5: a failure after the GLB was parented is rolled back (the
    /// GLB is destroyed and every vanilla visibility change is restored), so the
    /// "vanilla visuals left as-is" message below stays literally true; and the
    /// "already attached" check compares against the CURRENT vehicle instead of
    /// being session-global.
    /// </summary>
    internal static void SwapAfterSpawn(LandVehicle veh)
    {
        // 0.3.0 spawn diagnostic: fresh "GLB min Y before/after" per call.
        LastGlbMinYBefore = float.NaN;
        LastGlbMinYAfter = float.NaN;

        if (!SpikeState.VisualSwapEnabled)
        {
            Trace("[visual] swap disabled (SpikeState.VisualSwapEnabled=false) — vanilla visuals kept.");
            return;
        }

        GameObject? attached = SpikeState.VisualRoot;
        bool attachedAlive = attached != null && attached.Pointer != IntPtr.Zero;
        Transform? modelT = ModelTransform(veh);

        // M2-5: skip only when the tracked root really hangs under THIS vehicle.
        // Anything else (older vehicle, destroyed vehicle, cleared state) is stale
        // telemetry, not proof that this spawn is already swapped.
        if (attachedAlive && modelT != null && attached!.transform.IsChildOf(modelT))
        {
            Trace(
                $"[visual] a swap root is already attached under '{modelT.name}' — skipping a second swap on this vehicle.");
            return;
        }

        if (attachedAlive)
        {
            Trace(
                $"[visual] stale swap root '{attached!.name}' under '{attached.transform.parent?.name}' does not belong to " +
                $"'{modelT?.name ?? "<no model>"}' — clearing it and swapping this vehicle.");
            SpikeState.VisualRoot = null;
        }
        else if (attached != null)
        {
            Trace("[visual] swap root reference points at a destroyed object — clearing it.");
            SpikeState.VisualRoot = null;
        }

        try
        {
            SwapInternal(veh);
        }
        catch (Exception ex)
        {
            // M2-4: if the GLB already hung under the vehicle (or vanilla pixels were
            // already switched off), undo it before reporting — see RollbackPartialSwap.
            RollbackPartialSwap();
            Mod.Log.Error($"visual model swap failed ({ex.Message}) — vanilla visuals left as-is, spawn is unaffected.");
        }
    }

    /// <summary>
    /// The transform the swap hangs the GLB under: <c>veh.vehicleModel</c> when it
    /// exists, otherwise the vehicle root itself (the same fallback
    /// <see cref="SwapInternal"/> uses). Null when neither can be read.
    /// </summary>
    private static Transform? ModelTransform(LandVehicle veh)
    {
        try
        {
            GameObject? model = veh.vehicleModel;
            if (model != null && model.Pointer != IntPtr.Zero)
                return model.transform;
            return veh.transform;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// World min Y of the attached GLB right now (NaN when no GLB is attached or
    /// its bounds cannot be read) — consumed by the 0.3.0 "after settle" diagnostic
    /// in <see cref="SpikeRunner.TickRide"/>-adjacent logging.
    /// </summary>
    internal static float CurrentGlbMinY()
    {
        GameObject? root = SpikeState.VisualRoot;
        if (root == null || root.Pointer == IntPtr.Zero)
            return float.NaN;
        return TryGetWorldBounds(root, out Vector3 c, out Vector3 s)
            ? c.y - s.y * 0.5f
            : float.NaN;
    }

    /// <summary>
    /// One visual swap. Order: resolve the GLB → measure the vanilla bounds →
    /// load the GLB → snapshot the vanilla children *and their visibility* →
    /// parent the GLB (registers <see cref="SpikeState.VisualRoot"/> immediately,
    /// review M2-4) → strip its colliders / normalise layer + active state →
    /// switch the vanilla pixels off → auto-align → DONE log.
    /// Everything after the snapshot is protected by <see cref="_rollback"/>, so a
    /// failure anywhere in there leaves the vehicle exactly as it was found.
    /// </summary>
    /// <param name="veh">The freshly spawned spike vehicle.</param>
    private static void SwapInternal(LandVehicle veh)
    {
        string? path = ResolveAssetPath();
        if (path == null)
            return;

        // ---- 1. capture the alignment reference BEFORE anything is switched off ----
        GameObject? vehicleModel = veh.vehicleModel;
        GameObject parentGo = vehicleModel != null && vehicleModel.Pointer != IntPtr.Zero
            ? vehicleModel
            : veh.gameObject;

        // 0.3.0 float fix: the alignment reference is LandVehicle.boundingBox (the
        // deterministic prefab BoxCollider — world corners via TransformPoint(center
        // ± size/2), never culling-stale), NOT the vanilla Renderer.bounds: those
        // are refreshed only during culling and are STALE on the spawn frame (the
        // live run measured a −0.63 m y delta and a ~41 m xz offset pointing at the
        // pre-teleport pool location → "alignment rejected, GLB stays at local
        // zero" on EVERY spawn, which is what left the taxi floating).
        bool haveVanillaBounds = SpikeCommands.TryGetVehicleBoxWorldBounds(veh, out Vector3 boxMin, out Vector3 boxMax);
        Vector3 vanillaCenter = (boxMin + boxMax) * 0.5f;
        Vector3 vanillaSize = boxMax - boxMin;
        // Stale-bounds diagnostic only — never used for the alignment itself.
        bool haveRendererBounds = TryGetWorldBounds(parentGo, out Vector3 staleCenter, out Vector3 staleSize);
        Trace(
            $"[visual] parent = '{parentGo.name}' (vehicleModel {(vehicleModel == null ? "NULL — falling back to the vehicle root" : "present")}), " +
            (haveVanillaBounds
                ? $"boundingBox min=({Vec(boxMin)}) max=({Vec(boxMax)}) — alignment reference"
                : "boundingBox unavailable (no BoxCollider) — alignment skipped"));
        Trace(
            haveRendererBounds
                ? $"[visual] vanilla Renderer.bounds (diagnostic only, culling-stale on the spawn frame) center=({Vec(staleCenter)}) size=({Vec(staleSize)})"
                : "[visual] vanilla Renderer.bounds unavailable (no renderer)");

        // ---- 2. load the GLB through S1MAPI's runtime loader ----
        GameObject? glb;
        try
        {
            glb = S1MAPI.Gltf.GltfLoader.LoadFromFile(path);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"S1MAPI.Gltf.GltfLoader.LoadFromFile('{path}') threw: {ex.Message} — vanilla visuals kept.");
            return;
        }

        if (glb == null || glb.Pointer == IntPtr.Zero)
        {
            Mod.Log.Warn($"S1MAPI.Gltf.GltfLoader.LoadFromFile('{path}') returned no GameObject — vanilla visuals kept.");
            return;
        }

        // ---- 3. snapshot the vanilla children (the GLB joins them in a moment) ----
        Transform parentT = parentGo.transform;
        int childCount = parentT.childCount;
        var vanillaChildren = new Transform[childCount];
        bool[] childActive = new bool[childCount];
        for (int i = 0; i < childCount; i++)
        {
            vanillaChildren[i] = parentT.GetChild(i);
            childActive[i] = vanillaChildren[i] != null && vanillaChildren[i].Pointer != IntPtr.Zero
                && vanillaChildren[i].gameObject.activeSelf;
        }

        // Rollback snapshot (review M2-4), taken BEFORE the first mutation and before
        // the GLB is parented — so it can never contain one of our own renderers.
        Renderer[] snapRs = parentT.GetComponentsInChildren<Renderer>(true);
        var snapRsOn = new bool[snapRs.Length];
        var snapRsForceOff = new bool[snapRs.Length];
        for (int i = 0; i < snapRs.Length; i++)
        {
            snapRsOn[i] = snapRs[i] != null && snapRs[i].Pointer != IntPtr.Zero && snapRs[i].enabled;
            snapRsForceOff[i] = snapRs[i] != null && snapRs[i].Pointer != IntPtr.Zero && snapRs[i].forceRenderingOff;
        }

        LODGroup[] snapLods = parentT.GetComponentsInChildren<LODGroup>(true);
        var snapLodOn = new bool[snapLods.Length];
        for (int i = 0; i < snapLods.Length; i++)
            snapLodOn[i] = snapLods[i] != null && snapLods[i].Pointer != IntPtr.Zero && snapLods[i].enabled;

        _rollback = new Rollback
        {
            Glb = glb,
            Children = vanillaChildren,
            ChildActive = childActive,
            Renderers = snapRs,
            RenderersEnabled = snapRsOn,
            RenderersForceOff = snapRsForceOff,
            LodGroups = snapLods,
            LodEnabled = snapLodOn,
        };

        // ---- 4. hang the GLB in as a pure visual child ----
        glb.name = VisualRootName;
        glb.transform.SetParent(parentT, false);
        // M2-4: from the moment the GLB hangs under the vehicle a failure must be
        // able to find it again — this is the rollback handle.
        SpikeState.VisualRoot = glb;
        glb.transform.localPosition = Vector3.zero;
        // Stage 3b orientation fix (Dominik drove it live: W = reverse, S =
        // forward): the taxi.glb nose lands on Unity -Z while the vehicle
        // forward is +Z — the build script put the bumper on Blender -Y
        // assuming "-Y becomes +Z", but it becomes -Z, so the model stands
        // 180° against the physics. One flip HERE (not in the asset) turns
        // the nose onto +Z. The step-6 auto-align measures world bounds AFTER
        // this rotation, so it keeps compensating correctly.
        glb.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        glb.transform.localScale = Vector3.one;

        // Same layer as the vehicle: the game's culling/visibility code filters by
        // layer, and a GLB imported on its own layer can be culled (or drawn when the
        // vehicle is hidden). M2-3(a): whole subtree, not just the root.
        ApplyLayerRecursively(glb.transform, parentGo.layer);

        // M2-3(b): S1MAPI may hand back an inactive hierarchy — normalise it before
        // the renderer pass, otherwise "force the renderers on" would report success
        // on objects that stay invisible.
        SetActiveRecursively(glb.transform, true);

        // Visual-only: colliders on the GLB would become compound colliders of the
        // vanilla rigidbody and change the physics we must not touch.
        int strippedColliders = 0;
        var glbColliders = glb.GetComponentsInChildren<Collider>(true);
        for (int i = 0; i < glbColliders.Length; i++)
        {
            Collider col = glbColliders[i];
            if (col == null || col.Pointer == IntPtr.Zero)
                continue;
            try { UnityEngine.Object.Destroy(col); strippedColliders++; }
            catch { /* non-fatal */ }
        }

        // S1MAPI creates MeshFilter + MeshRenderer pairs for GLB nodes. Verify
        // actual mesh vertices and indexed submeshes before any vanilla pixels
        // are hidden; an empty Renderer component is not a valid visual.
        int glbRenderers = 0;
        var glbRs = glb.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < glbRs.Length; i++)
        {
            Renderer r = glbRs[i];
            if (r == null || r.Pointer == IntPtr.Zero || !HasRenderableMeshGeometry(r))
                continue;
            try
            {
                r.enabled = true;
                r.forceRenderingOff = false;
                glbRenderers++;
            }
            catch { /* non-fatal */ }
        }

        // Do not hide any vanilla pixels unless the imported GLB has at least one
        // renderer backed by nonempty mesh geometry. Rollback destroys the bad GLB.
        if (!TaxiVisualPolicy.CanCommitSwap(glbRenderers))
        {
            Mod.Log.Warn("[visual] GLB has 0 renderers with usable mesh geometry — rolling back; vanilla visuals stay visible.");
            RollbackPartialSwap();
            return;
        }

        // ---- 5. switch the vanilla visuals off (never destroy) ----
        int softHidden = 0;
        int untouched = 0;
        int errors = 0;
        for (int i = 0; i < vanillaChildren.Length; i++)
        {
            Transform child = vanillaChildren[i];
            if (child == null || child.Pointer == IntPtr.Zero)
                continue;

            try
            {
                Collider[] cols = child.GetComponentsInChildren<Collider>(true);
                Renderer[] rs = child.GetComponentsInChildren<Renderer>(true);
                int lodgs = child.GetComponentsInChildren<LODGroup>(true).Length;

                if (rs.Length == 0)
                {
                    // No pixels here (VehicleSound, TrunkGrid, OwnedVehiclePoI, collider
                    // dummies): a functional node — leaving it alone is the safe default.
                    untouched++;
                    Trace($"[visual]   '{Path2(child)}' -> untouched (colliders={cols.Length} renderers=0 lodGroups={lodgs})");
                }
                else
                {
                    // Keep every vanilla object active so scripts, audio and any
                    // non-collider gameplay components continue running. Hide pixels only.
                    HideRenderers(child, glb.transform);
                    softHidden++;
                    Trace($"[visual]   '{Path2(child)}' -> renderer-only hide, object KEPT ACTIVE (colliders={cols.Length} renderers={rs.Length} lodGroups={lodgs})");
                }
            }
            catch (Exception ex)
            {
                errors++;
                Mod.Log.Warn($"[visual] could not process child '{child.name}': {ex.Message} — left untouched.");
            }
        }

        // Final sweep (review M3-9): catch every renderer that is still visible under
        // the parent — the vehicleModel object itself, or a child branch the loop
        // could not process. Children the loop already handled are NOT counted again:
        // HideRenderers skips renderers that are already off or on an inactive object,
        // so this number is the *additional* set, not a second count of softHidden.
        // The GLB subtree is excluded, otherwise we would switch off our own taxi.
        int parentRs = HideRenderers(parentT, glb.transform);

        // ---- 6. auto-align: put the GLB where the SHITBOX BODY sits ----
        // Rule: same ground height (boundingBox MIN y — both cars stand on the road)
        // and the same lateral/longitudinal centre (boundingBox centre x/z). A plain
        // centre-minus-centre would sink the taxi whenever the two pivots differ
        // vertically, because a car's bounding box sits above its pivot. The
        // reference is the deterministic LandVehicle.boundingBox (see step 1) — with
        // the pre-0.3.0 Renderer.bounds reference this step rejected every spawn as
        // stale and left the GLB ~0.55 m in the air after the chassis pop-up.
        string alignNote = "skipped (boundingBox unavailable)";
        if (haveVanillaBounds && TryGetWorldBounds(glb, out Vector3 glbCenter, out Vector3 glbSize))
        {
            if (vanillaSize.x >= MinBoundsEdgeMeters && vanillaSize.y >= MinBoundsEdgeMeters && vanillaSize.z >= MinBoundsEdgeMeters
                && glbSize.x >= MinBoundsEdgeMeters && glbSize.y >= MinBoundsEdgeMeters && glbSize.z >= MinBoundsEdgeMeters)
            {
                Vector3 vanillaMin = vanillaCenter - vanillaSize * 0.5f;
                Vector3 glbMin = glbCenter - glbSize * 0.5f;
                LastGlbMinYBefore = glbMin.y;
                Vector3 delta = new Vector3(
                    vanillaCenter.x - glbCenter.x,
                    vanillaMin.y - glbMin.y,
                    vanillaCenter.z - glbCenter.z);

                // Plausibility guard (kept): the boundingBox is deterministic prefab
                // data, so a delta beyond the guard means the GLB itself sits
                // somewhere odd — never move it across the map.
                float flat = (float)Math.Sqrt(delta.x * delta.x + delta.z * delta.z);
                if (flat > MaxAlignOffsetMeters || Math.Abs(delta.y) > MaxAlignOffsetMeters)
                {
                    alignNote = $"rejected as implausible ({Vec(delta)} > {MaxAlignOffsetMeters:F0} m) — GLB stays at local zero";
                    Mod.Log.Warn($"[visual] {alignNote}.");
                }
                else if (SpikeState.VisualAutoAlign)
                {
                    glb.transform.position += delta;
                    alignNote = $"applied ({Vec(delta)})";
                }
                else
                {
                    alignNote = $"disabled — would be ({Vec(delta)})";
                }

                Trace($"[visual] GLB bounds   center=({Vec(glbCenter)}) size=({Vec(glbSize)})");
                Trace($"[visual] delta(xz centre + y ground) = ({Vec(delta)}) -> align {alignNote}");
            }
            else
            {
                alignNote = "skipped (bounds below the 0.1 m guard)";
                Trace($"[visual] bounds too small to align — boundingBox size=({Vec(vanillaSize)}) GLB size=({Vec(glbSize)})");
            }
        }

        // GLB min Y before/after for the 0.3.0 spawn diagnostic line: the "before"
        // half was captured above when the alignment measured it; skipped paths fall
        // back to the current value (nothing moved the GLB there).
        if (TryGetWorldBounds(glb, out Vector3 glbFinal, out Vector3 glbFinalSize))
        {
            float glbFinalMin = glbFinal.y - glbFinalSize.y * 0.5f;
            if (float.IsNaN(LastGlbMinYBefore))
                LastGlbMinYBefore = glbFinalMin;
            LastGlbMinYAfter = glbFinalMin;
        }

        // Success — drop the rollback snapshot (the swap may now be reported).
        _rollback = null;
        SpikeState.VisualSwaps++;

        // Visual wheel animation: bind each imported wheel node once to the native wheel
        // that sits at the same place in the vehicle frame. Visual-only; a failure leaves
        // the wheels static and never affects the drive.
        ReleaseWheels();
        _wheelBindRequested = true;
        BindWheels(veh, parentT, glb);

        Trace(
            $"[visual] swap DONE: GLB '{VisualRootName}' parented under '{parentGo.name}' (renderers={glbRenderers}, colliders stripped={strippedColliders}); " +
            $"vanilla children deactivated=0, renderer-hidden(kept active)={softHidden}, untouched(no renderers)={untouched}, errors={errors}, " +
            $"final-sweep-only renderers hidden={parentRs}; align {alignNote}; model rotated 180° about Y; visual wheels bound={_wheelLinks.Length}.");
    }

    /// <summary>
    /// Undoes a partially applied swap (review M2-4): the half-attached GLB is
    /// destroyed, <see cref="SpikeState.VisualRoot"/> is cleared and every vanilla
    /// child/renderer/LODGroup state captured before the first mutation is written
    /// back. This is what makes the reported failure text
    /// "vanilla visuals left as-is" true — without it the message would only be
    /// true for the physics, while the pixels were already switched off.
    /// Never throws: a rollback failure is reported, not propagated (the original
    /// exception is the one that matters).
    /// </summary>
    private static void RollbackPartialSwap()
    {
        Rollback? rb = _rollback;
        _rollback = null;
        SpikeState.VisualRoot = null;

        // Sever the cached wheel links FIRST and without touching any native object: the
        // GLB (and possibly the vehicle) is on its way out, so this must not dereference
        // a dying transform or collider.
        ReleaseWheels();

        if (rb == null)
            return; // nothing was mutated yet (the failure happened before the snapshot)

        if (rb.Glb != null && rb.Glb.Pointer != IntPtr.Zero)
        {
            try { UnityEngine.Object.Destroy(rb.Glb); }
            catch (Exception ex) { Mod.Log.Warn($"[visual] rollback could not destroy the GLB: {ex.Message}"); }
        }

        int children = 0;
        for (int i = 0; i < rb.Children.Length && i < rb.ChildActive.Length; i++)
        {
            Transform? child = rb.Children[i];
            if (child == null || child.Pointer == IntPtr.Zero)
                continue;
            try
            {
                if (child.gameObject.activeSelf != rb.ChildActive[i])
                {
                    child.gameObject.SetActive(rb.ChildActive[i]);
                    children++;
                }
            }
            catch { /* best effort */ }
        }

        int renderers = 0;
        for (int i = 0; i < rb.Renderers.Length && i < rb.RenderersEnabled.Length; i++)
        {
            Renderer? r = rb.Renderers[i];
            if (r == null || r.Pointer == IntPtr.Zero)
                continue;
            try
            {
                if (r.enabled != rb.RenderersEnabled[i] || r.forceRenderingOff != rb.RenderersForceOff[i])
                {
                    r.enabled = rb.RenderersEnabled[i];
                    r.forceRenderingOff = rb.RenderersForceOff[i];
                    renderers++;
                }
            }
            catch { /* best effort */ }
        }

        int lodGroups = 0;
        for (int i = 0; i < rb.LodGroups.Length && i < rb.LodEnabled.Length; i++)
        {
            LODGroup? lod = rb.LodGroups[i];
            if (lod == null || lod.Pointer == IntPtr.Zero)
                continue;
            try
            {
                if (lod.enabled != rb.LodEnabled[i])
                {
                    lod.enabled = rb.LodEnabled[i];
                    lodGroups++;
                }
            }
            catch { /* best effort */ }
        }

        Trace(
            $"[visual] rollback: GLB removed, vanilla visibility restored " +
            $"(children={children}, renderers={renderers}, lodGroups={lodGroups} written back).");
    }

    /// <summary>
    /// Copies <paramref name="layer"/> onto <paramref name="root"/> and every
    /// descendant (review M2-3a) — Unity layers are per GameObject, so setting only
    /// the GLB root would leave its meshes on whatever layer the loader produced.
    /// </summary>
    private static void ApplyLayerRecursively(Transform root, int layer)
    {
        if (root == null || root.Pointer == IntPtr.Zero)
            return;

        try { root.gameObject.layer = layer; }
        catch { /* non-fatal */ }

        for (int i = 0; i < root.childCount; i++)
        {
            Transform child = root.GetChild(i);
            if (child != null && child.Pointer != IntPtr.Zero)
                ApplyLayerRecursively(child, layer);
        }
    }

    /// <summary>
    /// Sets <c>activeSelf</c> on <paramref name="root"/> and every descendant
    /// (review M2-3b). Runs before the renderer pass so "renderers forced on" cannot
    /// report success for objects inside an inactive subtree.
    /// </summary>
    private static void SetActiveRecursively(Transform root, bool active)
    {
        if (root == null || root.Pointer == IntPtr.Zero)
            return;

        try { root.gameObject.SetActive(active); }
        catch { /* non-fatal */ }

        for (int i = 0; i < root.childCount; i++)
        {
            Transform child = root.GetChild(i);
            if (child != null && child.Pointer != IntPtr.Zero)
                SetActiveRecursively(child, active);
        }
    }

    /// <summary>
    /// Hides every renderer + LOD group under <paramref name="t"/> without
    /// touching GameObjects or colliders. The subtree rooted at
    /// <paramref name="excludeRoot"/> is skipped (that is our own GLB), and so is
    /// everything that is already invisible (renderer switched off, or its GameObject
    /// inactive in the hierarchy) — without that second rule the final sweep would
    /// count the very renderers the per-child pass has already handled (review M3-9).
    /// Returns the number of renderers this call actually switched off.
    /// </summary>
    private static int HideRenderers(Transform t, Transform? excludeRoot)
    {
        int hidden = 0;
        Renderer[] rs = t.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < rs.Length; i++)
        {
            Renderer r = rs[i];
            if (r == null || r.Pointer == IntPtr.Zero)
                continue;
            if (excludeRoot != null && excludeRoot.Pointer != IntPtr.Zero
                && r.transform != null && r.transform.Pointer != IntPtr.Zero
                && r.transform.IsChildOf(excludeRoot))
                continue;
            try
            {
                // Already invisible (inactive parent or a previous hide pass) — nothing
                // to do, and counting it would be a double count (M3-9).
                if (!r.enabled || r.forceRenderingOff)
                    continue;
                if (r.gameObject != null && r.gameObject.Pointer != IntPtr.Zero && !r.gameObject.activeInHierarchy)
                    continue;

                r.enabled = false;
                r.forceRenderingOff = true;
                hidden++;
            }
            catch { /* non-fatal */ }
        }

        var lodgs = t.GetComponentsInChildren<LODGroup>(true);
        for (int i = 0; i < lodgs.Length; i++)
        {
            LODGroup lod = lodgs[i];
            if (lod == null || lod.Pointer == IntPtr.Zero)
                continue;
            if (excludeRoot != null && excludeRoot.Pointer != IntPtr.Zero
                && lod.transform != null && lod.transform.Pointer != IntPtr.Zero
                && lod.transform.IsChildOf(excludeRoot))
                continue;
            try { lod.enabled = false; }
            catch { /* non-fatal */ }
        }

        return hidden;
    }

    /// <summary>
    /// True only when the renderer is backed by a nonempty MeshFilter or
    /// SkinnedMeshRenderer mesh with vertices and at least one indexed submesh.
    /// Any interop/API error fails closed so vanilla visuals remain visible.
    /// </summary>
    private static bool HasRenderableMeshGeometry(Renderer renderer)
    {
        try
        {
            MeshFilter? filter = renderer.GetComponent<MeshFilter>();
            if (filter != null && filter.Pointer != IntPtr.Zero && HasUsableMesh(filter.sharedMesh))
                return true;

            SkinnedMeshRenderer? skinned = renderer.GetComponent<SkinnedMeshRenderer>();
            return skinned != null && skinned.Pointer != IntPtr.Zero && HasUsableMesh(skinned.sharedMesh);
        }
        catch (Exception ex)
        {
            Trace($"[visual] GLB renderer geometry check failed: {ex.Message} — renderer ignored.");
            return false;
        }
    }

    private static bool HasUsableMesh(Mesh? mesh)
    {
        if (mesh == null || mesh.Pointer == IntPtr.Zero)
            return false;

        try
        {
            int vertices = mesh.vertexCount;
            int subMeshes = mesh.subMeshCount;
            if (vertices <= 0 || subMeshes <= 0)
                return false;

            int indexedSubMeshes = 0;
            for (int i = 0; i < subMeshes; i++)
            {
                if (mesh.GetIndexCount(i) > 0)
                {
                    indexedSubMeshes++;
                    break;
                }
            }

            return TaxiVisualPolicy.HasUsableGeometry(vertices, subMeshes, indexedSubMeshes);
        }
        catch (Exception ex)
        {
            Trace($"[visual] GLB mesh geometry read failed: {ex.Message} — mesh ignored.");
            return false;
        }
    }

    // ------------------------------------------------------------------
    // Visual wheels: binding, per-frame pose, release
    // ------------------------------------------------------------------

    /// <summary>
    /// Binds every imported wheel node once to the native wheel that sits at the same
    /// place in the vehicle frame. Positions decide the pairing — never the node names,
    /// never a forward-axis assumption — so a mirrored import or a 180°-rotated root is
    /// handled by construction. The native wheel list can still be empty on the spawn
    /// frame, so binding is retried a bounded number of times (no per-frame searches).
    /// Any unreadable state leaves the wheels static; the drive is never affected.
    /// </summary>
    private static void BindWheels(LandVehicle veh, Transform vehicleFrame, GameObject glb)
    {
        _wheelLinks = Array.Empty<WheelLink>();
        _wheelBody = null;
        _wheelVehicle = veh;
        _wheelRoot = glb;
        _wheelResolveAttempts++;

        try
        {
            var nodes = new List<Transform>();
            var nodePoints = new List<WheelPoint>();
            Transform glbT = glb.transform;
            for (int i = 0; i < glbT.childCount; i++)
            {
                Transform child = glbT.GetChild(i);
                if (child == null || child.Pointer == IntPtr.Zero)
                    continue;

                string name = child.name;
                if (name == null || !name.StartsWith(GlbWheelNodePrefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                Vector3 local = vehicleFrame.InverseTransformPoint(child.position);
                nodes.Add(child);
                nodePoints.Add(new WheelPoint(local.x, local.z));
            }

            if (nodes.Count == 0)
            {
                WheelUnavailable("the imported GLB has no 'Wheel*' node");
                return;
            }

            var nativeWheels = veh.wheels;
            if (nativeWheels == null || nativeWheels.Count == 0)
            {
                ScheduleWheelRetry("the native wheel list is still empty");
                return;
            }

            var links = new List<WheelLink>();
            var nodeTaken = new bool[nodes.Count];
            for (int i = 0; i < nativeWheels.Count; i++)
            {
                Wheel? wheel = nativeWheels[i];
                if (wheel == null)
                    continue;

                WheelCollider? collider = wheel.wheelCollider;
                if (collider == null || collider.Pointer == IntPtr.Zero)
                    continue;

                Vector3 nativeLocal = vehicleFrame.InverseTransformPoint(collider.transform.position);
                int match = TaxiWheelKinematics.FindCounterpart(
                    nodePoints,
                    new WheelPoint(nativeLocal.x, nativeLocal.z),
                    TaxiWheelKinematics.MaxMatchDistance);
                if (match < 0)
                {
                    Trace(
                        $"[visual] wheel {i}: no imported node near ({Vec(nativeLocal)}) within " +
                        $"{TaxiWheelKinematics.MaxMatchDistance:0.0} m — this wheel stays static.");
                    continue;
                }

                Transform node = nodes[match];
                if (nodeTaken[match])
                {
                    // Two native wheels landed on the same imported node (bad/ambiguous
                    // placement): driving one transform from two wheels would fight, so
                    // the second wheel stays static instead.
                    Trace($"[visual] wheel {i}: node '{node.name}' is already bound — this wheel stays static.");
                    continue;
                }
                nodeTaken[match] = true;

                bool steers = IsSteeredWheel(veh, wheel, collider);

                // The steering axis is the node's own up axis; a mirrored/flipped node
                // would steer the wrong way, so the sign comes from the real direction.
                Vector3 upInFrame = vehicleFrame.InverseTransformDirection(node.up);
                float steerAxisSign = TaxiWheelKinematics.AxisSign(upInFrame.y, 0.5f);
                float upSign = steers ? steerAxisSign : 0f;

                links.Add(new WheelLink
                {
                    Node = node,
                    RestRotation = node.localRotation,
                    Collider = collider,
                    Native = wheel,
                    UpSign = upSign,
                    SteerAxisSign = steerAxisSign,
                    // A wheel whose steering flag is still unset gets a short bounded
                    // re-latch instead of a permanent "never steers".
                    SteerProbeTicks = steers ? 0 : SteerProbeFrames,
                });
                Trace(
                    $"[visual] wheel {i} bound to '{node.name}' at ({Vec(nativeLocal)}) — steers={steers}, " +
                    $"steerAxisSign={upSign:0.#}, axleInFrame=({Vec(vehicleFrame.InverseTransformDirection(node.right))}).");
            }

            if (links.Count == 0)
            {
                WheelUnavailable("no imported wheel node matched a native wheel position");
                return;
            }

            _wheelBindRequested = false;
            _wheelLinks = links.ToArray();
            _wheelBody = TryReadBody(veh, links[0].Collider);
            if (_wheelBody == null)
                Trace("[visual] no rigidbody for the wheel point velocity — visual wheels stay static (drive unaffected).");

            _wheelUnavailableLogged = false;
            Mod.Log.Info($"[visual] visual wheels bound: {links.Count} of {nativeWheels.Count} native wheels ({nodes.Count} imported nodes).");
        }
        catch (Exception ex)
        {
            ScheduleWheelRetry($"binding threw: {ex.Message}");
        }
    }

    /// <summary>
    /// One deferred binding attempt. Time-gated and attempt-capped: the spawn frame may
    /// precede the native wheel list, and a permanently unusable vehicle must not cause
    /// a search every frame.
    /// </summary>
    private static void TryDeferredWheelBind()
    {
        if (!_wheelBindRequested)
            return;
        if (_wheelResolveAttempts >= WheelResolveAttempts)
            return;

        float now = Time.unscaledTime;
        if (now < _wheelRetryAt)
            return;
        _wheelRetryAt = now + WheelResolveRetrySeconds;

        LandVehicle? veh = SpikeState.Vehicle;
        GameObject? glb = SpikeState.VisualRoot;
        if (!ReferenceEquals(veh, _wheelVehicle) || !ReferenceEquals(glb, _wheelRoot) ||
            ReferenceEquals(veh, null) || glb == null || !S1Mods.Shared.NetworkGuard.IsAlive(glb))
        {
            ReleaseWheels();
            return;
        }

        Transform? frame = ModelTransform(veh);
        if (frame == null)
        {
            ReleaseWheels();
            return;
        }

        try
        {
            if (!glb.transform.IsChildOf(frame))
            {
                ReleaseWheels();
                return;
            }
        }
        catch
        {
            ReleaseWheels();
            return;
        }

        BindWheels(veh, frame, glb);
    }

    private static void ScheduleWheelRetry(string note)
    {
        if (_wheelResolveAttempts >= WheelResolveAttempts)
        {
            WheelUnavailable(note);
            return;
        }

        _wheelRetryAt = Time.unscaledTime + WheelResolveRetrySeconds;
        Trace(
            $"[visual] visual wheel binding deferred ({note}) — attempt " +
            $"{_wheelResolveAttempts}/{WheelResolveAttempts}.");
    }

    private static void WheelUnavailable(string reason)
    {
        ReleaseWheels();
        _wheelLinks = Array.Empty<WheelLink>();
        _wheelBody = null;
        _wheelResolveAttempts = WheelResolveAttempts;

        if (_wheelUnavailableLogged)
            return;
        _wheelUnavailableLogged = true;
        Mod.Log.Warn(
            $"[visual] imported wheels stay static: {reason}. " +
            "The taxi still drives, steers and settles fares exactly as before.");
    }

    /// <summary>
    /// Per-frame visual wheel pose from native wheel state. Read-only towards the game:
    /// no native wheel, collider, rigidbody or vehicle transform is written. Rolling is
    /// the native ground velocity at the wheel centre projected onto the node's own axle
    /// (correct in both directions and at any speed); steering is the native
    /// <c>WheelCollider.steerAngle</c> on the wheels the game marks as steered.
    /// The mounting rotation captured at bind time is kept, so only the delta is composed.
    /// </summary>
    internal static void TickWheels()
    {
        try { TickWheelState(); }
        catch (Exception ex) { WheelUnavailable("native visual state unreadable: " + ex.Message); }
    }

    private static void TickWheelState()
    {
        if (_wheelLinks.Length == 0)
        {
            TryDeferredWheelBind();
            return;
        }

        LandVehicle? veh = _wheelVehicle;
        GameObject? root = SpikeState.VisualRoot;
        if (ReferenceEquals(veh, null) || !ReferenceEquals(veh, SpikeState.Vehicle) ||
            root == null || root.Pointer == IntPtr.Zero)
        {
            ReleaseWheels();
            return;
        }

        // Paused/unreadable time must hold the wheels exactly where they are.
        float dt = Time.deltaTime;
        if (!(dt > 0f) || float.IsInfinity(dt))
            return;

        Rigidbody? body = _wheelBody;
        bool haveBody = body != null && body.Pointer != IntPtr.Zero;

        // The car's own up axis (not a world constant) keeps the roll sign right on slopes.
        WheelVector up = WheelVector.Up;
        if (haveBody)
        {
            try
            {
                Vector3 bodyUp = body!.transform.up;
                up = ToWheelVector(bodyUp);
            }
            catch
            {
                ReleaseWheels();
                return;
            }
        }

        WheelLink[] links = _wheelLinks;
        for (int i = 0; i < links.Length; i++)
        {
            WheelLink link = links[i];
            try
            {
                Transform? node = link.Node;
                WheelCollider? collider = link.Collider;
                if (node == null || node.Pointer == IntPtr.Zero || collider == null || collider.Pointer == IntPtr.Zero)
                {
                    ReleaseWheels();
                    return;
                }

                if (haveBody)
                {
                    Vector3 velocity = body!.GetPointVelocity(node.position);
                    float rate = TaxiWheelKinematics.SpinDegreesPerSecond(
                        ToWheelVector(velocity),
                        up,
                        ToWheelVector(node.right),
                        collider.radius);
                    link.SpinDegrees = TaxiWheelKinematics.AdvanceSpin(
                        link.SpinDegrees, rate, dt, TaxiWheelKinematics.MaxStepDegrees);
                }

                if (link.SteerProbeTicks > 0)
                {
                    // Bounded re-latch of the native steering flag (4 cheap reads for at
                    // most ~1 s) — never a permanent search.
                    link.SteerProbeTicks--;
                    if (IsSteeredWheel(veh!, link.Native, collider))
                    {
                        link.UpSign = link.SteerAxisSign;
                        link.SteerProbeTicks = 0;
                    }
                }

                float steer = link.UpSign == 0f
                    ? 0f
                    : TaxiWheelKinematics.SteerDegrees(collider.steerAngle, link.UpSign);

                node.localRotation = link.RestRotation
                    * Quaternion.AngleAxis(steer, Vector3.up)
                    * Quaternion.AngleAxis(link.SpinDegrees, Vector3.right);
            }
            catch
            {
                // A dying wrapper (scene reset, vehicle destroyed) must not keep the tick
                // alive with dead proxies — drop the cache and stay quiet.
                ReleaseWheels();
                return;
            }
        }
    }

    /// <summary>
    /// Severs the cached wheel links. Deliberately touches NO Unity object: it runs on
    /// rollback, scene reset and deinit, where the native vehicle and the GLB may already
    /// be destroyed. After this the wheels are static until the next swap binds again.
    /// </summary>
    internal static void ReleaseWheels()
    {
        _wheelLinks = Array.Empty<WheelLink>();
        _wheelVehicle = null;
        _wheelBody = null;
        _wheelRoot = null;
        _wheelBindRequested = false;
        _wheelRetryAt = 0f;
        _wheelResolveAttempts = 0;
    }

    /// <summary>
    /// Front wheels are the ones the game marks as steered. <c>Wheel.IsSteerWheel</c> is
    /// the native flag; <c>LandVehicle.steerWheels</c> membership is the fallback when
    /// that property is unreadable or still false on the spawn frame.
    /// </summary>
    private static bool IsSteeredWheel(LandVehicle veh, Wheel wheel, WheelCollider collider)
    {
        try
        {
            if (wheel.IsSteerWheel)
                return true;
        }
        catch { /* fall through to the collider list */ }

        try
        {
            var steered = veh.steerWheels;
            if (steered == null)
                return false;

            for (int i = 0; i < steered.Length; i++)
            {
                WheelCollider candidate = steered[i];
                if (candidate != null && candidate.Pointer == collider.Pointer)
                    return true;
            }
        }
        catch { /* unknown membership: this wheel simply does not steer */ }

        return false;
    }

    private static Rigidbody? TryReadBody(LandVehicle veh, WheelCollider collider)
    {
        try
        {
            Rigidbody body = veh.Rb;
            if (body != null && body.Pointer != IntPtr.Zero)
                return body;
        }
        catch { /* fall through to the collider's body */ }

        try
        {
            Rigidbody attached = collider.attachedRigidbody;
            if (attached != null && attached.Pointer != IntPtr.Zero)
                return attached;
        }
        catch { /* no readable body — wheels stay static */ }

        return null;
    }

    private static WheelVector ToWheelVector(Vector3 v) => new WheelVector(v.x, v.y, v.z);

    /// <summary>
    /// World-space union of the *car body* renderers under <paramref name="root"/>:
    /// any renderer larger than <see cref="MaxBoundsEdgeMeters"/> is ignored — a
    /// point-of-interest marker or a particle emitter otherwise stretches the union
    /// across the whole map (the first live run aligned the taxi 66 m away).
    /// NOTE: no `is MeshRenderer` filter — under IL2CPP interop
    /// GetComponentsInChildren&lt;Renderer&gt; returns wrappers of the *declared*
    /// type, so every element types as Renderer and such a check drops the union.
    /// </summary>
    private static bool TryGetWorldBounds(GameObject root, out Vector3 center, out Vector3 size)
    {
        center = Vector3.zero;
        size = Vector3.zero;
        if (root == null || root.Pointer == IntPtr.Zero)
            return false;

        Renderer[] rs = root.GetComponentsInChildren<Renderer>(true);
        bool any = false;
        Bounds union = default;
        for (int i = 0; i < rs.Length; i++)
        {
            Renderer r = rs[i];
            if (r == null || r.Pointer == IntPtr.Zero)
                continue;
            try
            {
                Bounds b = r.bounds;
                if (b.size.x > MaxBoundsEdgeMeters || b.size.y > MaxBoundsEdgeMeters || b.size.z > MaxBoundsEdgeMeters)
                    continue;
                if (!any)
                {
                    union = b;
                    any = true;
                }
                else
                {
                    union.Encapsulate(b);
                }
            }
            catch { /* non-fatal */ }
        }

        if (!any)
            return false;

        center = union.center;
        size = union.size;
        return true;
    }

    /// <summary>Path from the swap parent down to <paramref name="t"/> for readable log lines.</summary>
    private static string Path2(Transform t)
    {
        string name = t.name;
        Transform? p = t.parent;
        int guard = 0;
        while (p != null && p.Pointer != IntPtr.Zero && guard++ < 4)
        {
            name = p.name + "/" + name;
            p = p.parent;
        }

        return name;
    }

    /// <summary>
    /// Formats a <see cref="Vector3"/> as <c>"x, y, z"</c> with two decimals using
    /// the invariant culture (log lines must not change with the OS locale).
    /// </summary>
    /// <param name="v">The vector to format.</param>
    /// <returns>The invariant-culture representation.</returns>
    private static string Vec(Vector3 v) =>
        string.Format(CultureInfo.InvariantCulture, "{0:F2}, {1:F2}, {2:F2}", v.x, v.y, v.z);
}
