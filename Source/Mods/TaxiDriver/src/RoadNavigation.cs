// ---------------------------------------------------------------------------
// RoadNavigation.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract tests can still read that
// file's exact content.
//
//   Roads/RoadKeeper.cs            -> section RoadKeeper.cs
//   Roads/RoadTarget.cs            -> section RoadTarget.cs
//
// ---------------------------------------------------------------------------

namespace TaxiDriver
{
    // ==== TaxiDriver section: RoadKeeper.cs ====
    using System;
    using Il2CppScheduleOne.Vehicles;
    using Il2CppScheduleOne.Vehicles.AI;
    using UnityEngine;


    /// <summary>
    /// Paket B (2026-09-29) — the road keeper: "eine Fahrhilfe ... sodass das Taxi
    /// erst die Straße nicht verlassen darf" (Dominik).
    ///
    /// The road is not guessed: the GAME already knows the exact route of every
    /// dispatch — <c>VehicleAgent.path</c> is the live <c>SmoothedPath</c> polyline the
    /// agent is following (built by the game's own path pipeline over the Road Nodes
    /// graph). This class turns that polyline into a corridor the taxi cannot leave:
    ///
    /// <list type="bullet">
    /// <item><b>Soft (lane assist):</b> drifts past the corridor margin → a small
    /// lateral nudge pulls the car back toward the route line each tick. Natural
    /// driving, no teleport.</item>
    /// <item><b>Hard (enforcement):</b> outside the hard margin, sunk into a ditch
    /// (car far below the route line) or on a steep off-road slope → the car is
    /// snapped back onto the route line, aligned with the road direction. This is
    /// the HomelessMod ground-probe technique (down-ray + slope check) doing the
    /// opposite job: not placing an object on the ground, but recognising when the
    /// car has left drivable ground.</item>
    /// </list>
    ///
    /// Deliberate scope: <see cref="SpikeState.NavTarget"/> is NEVER touched (the
    /// arrival verdict keeps its honest target), and while the game's own patrol
    /// driver (<see cref="TaxiAI"/>) owns the wheel only the hard rule applies — the
    /// keeper yanks a car out of a ditch but does not micro-steer the game's AI.
    /// Every correction is logged with a <c>[road]</c> prefix.
    /// </summary>
    internal static class RoadKeeper
    {
        /// <summary>Corridor half-width before the soft lane assist starts nudging (metres).</summary>
        private const float SoftMarginMeters = 3f;


        /// <summary>
        /// How far the car may sit BELOW the route line before it counts as "in a
        /// ditch" (metres). The polyline follows the road surface, so a real Grube
        /// shows up as a Y gap even while the lateral distance is still small.
        /// </summary>
        private const float MaxDropMeters = 2f;

        /// <summary>Ground slope above this = embankment/ditch wall (degrees, HomelessMod slope validation).</summary>
        private const float MaxSlopeDegrees = 40f;

        /// <summary>Tick interval of the keeper (seconds) — 5 Hz is plenty for lane keeping.</summary>
        private const float TickIntervalSeconds = 0.2f;

        /// <summary>Maximum lateral nudge per tick (metres) — small steps, no pops.</summary>
        private const float MaxNudgeMeters = 0.35f;

        /// <summary>Half extents of the box that must be free at a snap target (car footprint, metres).</summary>
        private static readonly Vector3 SnapClearanceHalfExtents = new Vector3(1.0f, 0.6f, 2.2f);

        /// <summary>Lateral offsets along the road (metres) tried when the exact snap point is occupied.</summary>
        private static readonly float[] SnapAlongOffsets = { 4f, -4f, 8f, -8f };

        /// <summary>Seconds between "no free spot" warnings, so a blocked snap cannot spam the log.</summary>
        private const float BlockedLogIntervalSeconds = 5f;

        private static float _nextBlockedLogAt;

        private static float _nextTickAt;

        /// <summary>True while the car is outside the soft margin (one-shot excursion logs).</summary>
        private static bool _excursionActive;

        /// <summary>Telemetry for <c>taxi status</c>: soft nudges / hard snaps this drive.</summary>
        private static int _softCorrections;

        /// <summary>Telemetry for <c>taxi status</c>: hard snap-backs this drive.</summary>
        internal static int HardSnaps { get; private set; }

        /// <summary>Clears the excursion state + telemetry (called per dispatch).</summary>
        internal static void Reset()
        {
            _excursionActive = false;
            _softCorrections = 0;
            HardSnaps = 0;
            _nextTickAt = 0f;
        }

        /// <summary>
        /// Per-frame entry (from <see cref="SpikeRunner.Update"/>): no-ops unless a
        /// dispatch is in flight. All native reads are guarded — a dead handle just
        /// means "keeper idle this tick".
        /// </summary>
        internal static void Tick()
        {
            if (Time.timeScale == 0f || !SpikeState.PollingActive || SpikeState.Reverse.Active)
                return;

            if (Time.unscaledTime < _nextTickAt)
                return;

            _nextTickAt = Time.unscaledTime + TickIntervalSeconds;

            try
            {
                LandVehicle? veh = SpikeState.Vehicle;
                if (veh == null || !SpikeCommands.HasAuthority() || !SpikeCommands.IsOwnedTaxi(veh))
                    return;

                VehicleAgent? agent = veh.Agent;
                if (agent == null)
                    return;

                Vector3 position = veh.transform.position;
                if (!TryGetCorridor(agent, position, out Vector3 closest, out Vector3 direction, out float lateral))
                    return;

                float drop = closest.y - position.y;
                bool inDitch = drop > MaxDropMeters;
                bool hard = RoadSnapSafetyPolicy.NeedsHardCorrection(lateral, drop,
                    lateral > SoftMarginMeters && SteepGroundBelow(position));

                if (hard)
                {
                    HardSnap(veh, closest, direction, lateral, drop, inDitch);
                    return;
                }

                if (lateral > SoftMarginMeters)
                {
                    SoftNudge(veh, closest, lateral);
                    return;
                }

                if (_excursionActive)
                {
                    _excursionActive = false;
                    Mod.Log.Info(
                        $"[road] back on the road (lateral {TaxiDestinations.Num(lateral)} m within the " +
                        $"{TaxiDestinations.Num(SoftMarginMeters)} m corridor) after {_softCorrections} nudge(s) / {HardSnaps} snap(s).");
                }
            }
            catch (Exception ex)
            {
                // The keeper is supervision — it must never break the ride it supervises.
                Mod.Log.Warn($"[road] keeper tick failed ({ex.GetType().Name}: {ex.Message}) — ride continues unguarded this tick.");
            }
        }

        /// <summary>
        /// Closest point of the live route polyline to the car (XZ projection per
        /// segment), plus the road direction of that segment and the lateral distance.
        /// False while the agent has no route yet (dispatch just started).
        /// </summary>
        private static bool TryGetCorridor(VehicleAgent agent, Vector3 position, out Vector3 closest, out Vector3 direction, out float lateral)
        {
            closest = default;
            direction = Vector3.forward;
            lateral = float.MaxValue;

            var path = agent.path;
            if (path == null)
                return false;

            var points = path.vectorPath;
            if (points == null || points.Count < 2)
                return false;

            float bestSqr = float.MaxValue;
            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector3 a = points[i];
                Vector3 b = points[i + 1];

                if (!RoadProjectionPolicy.TryProject(
                        new(a.x, a.y, a.z), new(b.x, b.y, b.z), new(position.x, position.y, position.z),
                        out var projected, out var tangent, out float distance))
                    continue;
                float dSqr = distance * distance;
                if (dSqr < bestSqr)
                {
                    bestSqr = dSqr;
                    closest = new Vector3(projected.X, projected.Y, projected.Z);
                    direction = new Vector3(tangent.X, 0f, tangent.Z);
                }
            }

            if (bestSqr == float.MaxValue)
                return false;

            lateral = Mathf.Sqrt(bestSqr);
            return true;
        }

        /// <summary>
        /// Soft lane assist: a small lateral nudge toward the route line (one log line
        /// per excursion, not per tick). While the game's patrol driver owns the wheel
        /// this is skipped — the keeper only enforces the hard rule there.
        /// </summary>
        private static void SoftNudge(LandVehicle veh, Vector3 closest, float lateral)
        {
            if (!_excursionActive)
            {
                _excursionActive = true;
                Mod.Log.Warn(
                    $"[road] drifting off the road (lateral {TaxiDestinations.Num(lateral)} m > " +
                    $"{TaxiDestinations.Num(SoftMarginMeters)} m) — lane assist pulls back toward the route.");
            }

            if (TaxiAI.Active)
                return;

            Vector3 position = veh.transform.position;
            Vector3 pull = closest - position;
            pull.y = 0f;
            if (pull.sqrMagnitude < 1e-4f)
                return;

            float step = Mathf.Min((lateral - SoftMarginMeters) * 0.4f, MaxNudgeMeters);
            Vector3 target = position + pull.normalized * step;
            target.y = position.y;
            ApplyPosition(veh, target, veh.transform.rotation, zeroVelocity: false);
            _softCorrections++;
        }

        /// <summary>
        /// Hard enforcement: the taxi may not leave the road — snap it back onto the
        /// route line, aligned with the road direction. This is also the last-resort
        /// "road-snap" that replaces a GAVE-UP for cars stuck in terrain.
        /// </summary>
        private static void HardSnap(LandVehicle veh, Vector3 closest, Vector3 direction, float lateral, float drop, bool inDitch)
        {
            _excursionActive = true;

            // A snap onto an occupied spot (parked car, lamp, other NPC car) wedges the taxi
            // INTO that object - the opposite of the job. Look for free road first; with no
            // free spot, leave the car to the game's own driving (and its stuck recovery).
            if (!SpikeCommands.HasAuthority() || !SpikeCommands.IsOwnedTaxi(veh))
                return;
            if (!TryFindFreeSpot(veh, closest, direction, out Vector3 target, out Quaternion rotation))
            {
                if (Time.unscaledTime >= _nextBlockedLogAt)
                {
                    _nextBlockedLogAt = Time.unscaledTime + BlockedLogIntervalSeconds;
                    Mod.Log.Warn(
                        $"[road] snap-back skipped: no free road spot near {SpikeCommands.Fmt(closest)} " +
                        $"(lateral {TaxiDestinations.Num(lateral)} m) - leaving the car to the AI's own recovery.");
                }

                return;
            }

            HardSnaps++;
            Mod.Log.Warn(
                $"[road] HARD SNAP #{HardSnaps} back onto the road (lateral {TaxiDestinations.Num(lateral)} m" +
                $"{(inDitch ? $", {TaxiDestinations.Num(drop)} m below the road = ditch" : string.Empty)}) — " +
                $"the taxi may not leave the road; placed at {SpikeCommands.Fmt(target)}.");

            ApplyPosition(veh, target, rotation, zeroVelocity: true);
        }

        /// <summary>
        /// First collision-free spot for the car on the route line: the exact closest
        /// point, then a few points further along / back along the road direction.
        /// Static and dynamic colliders count; triggers and the taxi itself do not.
        /// </summary>
        internal static bool TryFindFreeSpot(LandVehicle veh, Vector3 closest, Vector3 direction, out Vector3 spot,
            out Quaternion rotation, bool skipOrigin = false, bool useCorridor = true)
        {
            spot = default;
            rotation = Quaternion.identity;
            if (!SpikeCommands.HasAuthority() || !SpikeCommands.IsOwnedTaxi(veh))
                return false;
            Vector3 flatDir = new Vector3(direction.x, 0f, direction.z);
            if (!Finite(closest) || !Finite(flatDir) || flatDir.sqrMagnitude < 1e-4f)
                return false;
            flatDir.Normalize();

            // Preserve the existing bounded origin / +/-4 / +/-8 search. Each offset is
            // reprojected onto the real 3D route, not extrapolated at the origin's height.
            for (int i = -1; i < SnapAlongOffsets.Length; i++)
            {
                if (i == -1 && skipOrigin)
                    continue;
                Vector3 candidate = closest + flatDir * (i == -1 ? 0f : SnapAlongOffsets[i]);
                Vector3 tangent = flatDir;
                if (useCorridor)
                {
                    var agent = veh.Agent;
                    if (agent == null || !TryGetCorridor(agent, candidate, out candidate, out tangent, out _))
                        continue;
                }
                // Clamped end projections must not defeat a rescue's skip-origin request.
                if (skipOrigin && Vector3.Distance(candidate, closest) < 0.5f)
                    continue;
                Quaternion candidateRotation = Quaternion.LookRotation(tangent, Vector3.up);
                if (TrySupportedSpot(veh, candidate, candidateRotation, out spot))
                {
                    rotation = candidateRotation;
                    return true;
                }
            }
            return false;
        }

        private static bool TrySupportedSpot(LandVehicle veh, Vector3 projected, Quaternion rotation, out Vector3 spot)
        {
            spot = default;
            try
            {
                if (!SpikeCommands.HasAuthority() || !SpikeCommands.IsOwnedTaxi(veh) || !Finite(projected) ||
                    !TryGetPlacementBounds(veh, rotation, out Vector3 minOffset, out Vector3 maxOffset))
                    return false;
                float proximity = RoadSnapSafetyPolicy.SupportProximityMeters;
                RaycastHit[] hits = Physics.RaycastAll(projected + Vector3.up * proximity, Vector3.down,
                    proximity * 2f, ~0, QueryTriggerInteraction.Ignore);
                // RaycastAll has no ordering guarantee; prefer support closest to road
                // elevation, not the highest nearby unrelated surface.
                Array.Sort(hits, (a, b) => Mathf.Abs(a.point.y - projected.y).CompareTo(Mathf.Abs(b.point.y - projected.y)));
                foreach (var hit in hits)
                {
                    var collider = hit.collider;
                    if (collider == null || collider.Pointer == IntPtr.Zero || collider.WasCollected ||
                        hit.transform == null || !Finite(hit.point) || !Finite(hit.normal) || hit.normal.sqrMagnitude < 1e-6f)
                        continue;
                    if (!RoadSnapSafetyPolicy.IsSafeSupport(collider.isTrigger, collider.attachedRigidbody != null,
                            collider.GetComponentInParent<Il2CppScheduleOne.NPCs.NPC>() != null ||
                            collider.GetComponentInParent<Il2CppScheduleOne.PlayerScripts.Player>() != null,
                            collider.GetComponentInParent<Il2CppScheduleOne.Vehicles.LandVehicle>() != null,
                            hit.transform.IsChildOf(veh.transform), Vector3.Angle(hit.normal, Vector3.up)))
                        continue;
                    float rootY = veh.transform.position.y;
                    if (!RoadSnapSafetyPolicy.TryRootHeight(projected.y, hit.point.y, rootY, rootY + minOffset.y,
                            supported: true, free: true, out float targetY))
                        continue;
                    Vector3 candidate = new Vector3(projected.x, targetY, projected.z);
                    bool free = IsPlacementFree(veh, candidate, minOffset, maxOffset);
                    if (!RoadSnapSafetyPolicy.TryRootHeight(projected.y, hit.point.y, rootY, rootY + minOffset.y,
                            supported: true, free: free, out _))
                        continue;
                    spot = candidate;
                    return true;
                }
            }
            catch (Exception)
            {
                // Unknown support, bounds or ancestry: never relocate blind.
            }
            return false;
        }

        private static bool Finite(Vector3 value) => RoadProjectionPolicy.Finite(new(value.x, value.y, value.z));

        private static bool TryGetPlacementBounds(LandVehicle veh, Quaternion rotation, out Vector3 min, out Vector3 max)
        {
            min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            // The existing spawn/rescue uses the deterministic boundingBox, not renderer
            // bounds. Measure ALL eight corners at the proposed heading: two opposite
            // world corners can understate a rotated box, and current roll/pitch must not
            // become clearance for a proposed upright placement.
            var box = veh.boundingBox;
            if (box == null || box.Pointer == IntPtr.Zero || box.WasCollected || box.transform == null)
                return false;
            Vector3 half = box.size * 0.5f;
            if (!Finite(half) || half.x <= 0f || half.y <= 0f || half.z <= 0f)
                return false;
            var root = veh.transform;
            Span<System.Numerics.Vector3> offsets = stackalloc System.Numerics.Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                Vector3 corner = box.center + new Vector3((i & 1) == 0 ? -half.x : half.x,
                    (i & 2) == 0 ? -half.y : half.y, (i & 4) == 0 ? -half.z : half.z);
                Vector3 offset = box.transform.TransformPoint(corner) - root.position;
                offsets[i] = new(offset.x, offset.y, offset.z);
            }
            Quaternion current = root.rotation;
            if (!RoadSnapSafetyPolicy.TryPlacementBounds(offsets,
                    new(current.x, current.y, current.z, current.w),
                    new(rotation.x, rotation.y, rotation.z, rotation.w), out var measuredMin, out var measuredMax))
                return false;
            min = new Vector3(measuredMin.X, measuredMin.Y, measuredMin.Z);
            max = new Vector3(measuredMax.X, measuredMax.Y, measuredMax.Z);
            return true;
        }

        private static bool IsPlacementFree(LandVehicle veh, Vector3 position, Vector3 minOffset, Vector3 maxOffset)
        {
            // Conservative world AABB of the measured chassis at the candidate rotation.
            // Unlike the legacy spawn probe this has no guessed footprint/root offset.
            var hits = Physics.OverlapBox(position + (minOffset + maxOffset) * 0.5f,
                (maxOffset - minOffset) * 0.5f, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);
            foreach (var hit in hits)
            {
                if (hit == null || hit.Pointer == IntPtr.Zero || hit.WasCollected || hit.transform == null)
                    return false;
                if (!hit.transform.IsChildOf(veh.transform))
                    return false;
            }
            return true;
        }

        internal static bool IsFree(LandVehicle? veh, Vector3 position, Quaternion rotation)
        {
            try
            {
                Vector3 center = position + Vector3.up * (SnapClearanceHalfExtents.y + 0.35f);
                var hits = Physics.OverlapBox(center, SnapClearanceHalfExtents, rotation, ~0, QueryTriggerInteraction.Ignore);
                foreach (var hit in hits)
                {
                    if (hit == null)
                        continue;
                    // Ignore the taxi's own colliders (and its driver's, which live under it).
                    if (veh != null && hit.transform != null && hit.transform.IsChildOf(veh.transform))
                        continue;
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                // Unknown -> treat as occupied (never snap blind).
                return false;
            }
        }

        /// <summary>
        /// Places the vehicle (rigidbody-aware: position/rotation + velocity kill when
        /// a Rigidbody is present, transform fallback otherwise).
        /// </summary>
        internal static void ApplyPosition(LandVehicle veh, Vector3 position, Quaternion rotation, bool zeroVelocity)
        {
            if (!SpikeCommands.HasAuthority() || !SpikeCommands.IsOwnedTaxi(veh))
            {
                Mod.Log.Warn("[taxi-recovery] road correction refused: vehicle is not a live TaxiDriver-owned taxi on the host.");
                return;
            }

            // Record before touching native state, including partial/fallback placement.
            // Corrections never count as driving progress or renew the operation deadline.
            SpikeRunner.ObserveArtificialPlacement(position);

            try
            {
                Rigidbody? rb = veh.GetComponent<Rigidbody>();
                if (rb != null && rb.Pointer != IntPtr.Zero)
                {
                    if (zeroVelocity)
                    {
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }

                    rb.position = position;
                    rb.rotation = rotation;
                    return;
                }
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[road] rigidbody placement failed ({ex.Message}) — transform fallback.");
            }

            veh.transform.position = position;
            veh.transform.rotation = rotation;
            Physics.SyncTransforms();
        }

        /// <summary>
        /// Ground probe below the car (the HomelessMod slope validation, reused for
        /// driving): a steep surface under the wheels means embankment or ditch wall —
        /// not drivable road. False when no ground is found (mid-air over a bridge gap
        /// is not an excuse to snap).
        /// </summary>
        private static bool SteepGroundBelow(Vector3 position)
        {
            Vector3 origin = position + Vector3.up * 2f;
            if (!Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 6f, ~0, QueryTriggerInteraction.Ignore))
                return false;

            float slope = Vector3.Angle(hit.normal, Vector3.up);
            return slope > MaxSlopeDegrees;
        }

        /// <summary>Bumper probe shared by spawn orientation and stationary supervision.</summary>
        internal static bool HasObstacle(LandVehicle? veh, Vector3 position, Quaternion rotation,
            bool rear = false, bool staticOnly = false, float distance = 2f)
        {
            try
            {
                Vector3 direction = rotation * (rear ? Vector3.back : Vector3.forward);
                Vector3 origin = position + Vector3.up * 0.8f + direction * 2.4f;
                // Include initial overlaps: SphereCast alone does not report these.
                var overlaps = Physics.OverlapSphere(origin, 0.4f, ~0, QueryTriggerInteraction.Ignore);
                foreach (var collider in overlaps)
                    if (IsObstacle(collider, veh, staticOnly))
                        return true;
                var hits = Physics.SphereCastAll(origin, 0.4f, direction, distance, ~0, QueryTriggerInteraction.Ignore);
                foreach (var hit in hits)
                    if (IsObstacle(hit.collider, veh, staticOnly))
                        return true;
                return false;
            }
            catch (Exception ex)
            {
                Mod.Log.Warn($"[patrol] clearance probe failed: {ex.Message}");
                // Fail closed for movement; never shorten the watchdog on an unknown hit.
                return !staticOnly;
            }
        }

        private static bool IsObstacle(Collider collider, LandVehicle? veh, bool staticOnly)
        {
            if (collider == null || collider.isTrigger)
                return false;
            if (veh != null && collider.transform != null && collider.transform.IsChildOf(veh.transform))
                return false;
            if (staticOnly)
            {
                if (collider.attachedRigidbody != null)
                    return false;
                // Some character colliders have no Rigidbody; they are not static props.
                if (collider.GetComponentInParent<Il2CppScheduleOne.NPCs.NPC>() != null ||
                    collider.GetComponentInParent<Il2CppScheduleOne.PlayerScripts.Player>() != null)
                    return false;
            }
            return true;
        }

        /// <summary>Last-resort rescue. Prefer the live road corridor, never an occupied point.</summary>
        /// <param name="context">Who triggered the rescue (used verbatim in the log lines).</param>
        internal static bool TryRescueStartup(LandVehicle veh, VehicleAgent agent, string context = "startup")
        {
            if (!SpikeCommands.HasAuthority() || !SpikeCommands.IsOwnedTaxi(veh))
            {
                Mod.Log.Warn($"[taxi-recovery] {context} rescue refused: vehicle is not a live TaxiDriver-owned taxi on the host.");
                return false;
            }

            Vector3 position = veh.transform.position;
            Vector3 closest = position;
            Vector3 direction = veh.transform.forward;
            bool hasCorridor = TryGetCorridor(agent, position, out Vector3 road, out Vector3 tangent, out _);
            if (hasCorridor)
            {
                closest = road;
                direction = tangent;
            }
            else
            {
                // Retain the bounded local fallback when the route is not available;
                // anchor its support search to measured chassis bottom, never a tall ray.
                if (!TryGetPlacementBounds(veh, veh.transform.rotation, out Vector3 bottom, out _))
                    return false;
                closest.y = position.y + bottom.y;
            }
            float dx = closest.x - position.x;
            float dz = closest.z - position.z;
            if (!TryFindFreeSpot(veh, closest, direction, out Vector3 spot, out Quaternion rotation,
                    skipOrigin: dx * dx + dz * dz < 0.25f, useCorridor: hasCorridor))
            {
                Mod.Log.Warn($"[patrol] {context} rescue: TryFindFreeSpot found no supported free point; no blind teleport.");
                return false;
            }
            if (HasObstacle(veh, spot, rotation))
            {
                if (!HasObstacle(veh, spot, rotation, rear: true))
                    rotation = rotation * Quaternion.Euler(0f, 180f, 0f);
                else
                {
                    Mod.Log.Warn($"[patrol] {context} rescue: free point blocked on both ends; not relocating.");
                    return false;
                }
            }
            // Recheck the measured footprint at the final (possibly reversed) heading.
            if (!TryGetPlacementBounds(veh, rotation, out Vector3 minOffset, out Vector3 maxOffset) ||
                !IsPlacementFree(veh, spot, minOffset, maxOffset) ||
                !SpikeCommands.HasAuthority() || !SpikeCommands.IsOwnedTaxi(veh))
                return false;
            ApplyPosition(veh, spot, rotation, zeroVelocity: true);
            Physics.SyncTransforms();
            Mod.Log.Warn($"[patrol] {context} rescue: placed at {SpikeCommands.Fmt(spot)} (TryFindFreeSpot, oriented box clear).");
            return true;
        }
    }
    // ==== end TaxiDriver section: RoadKeeper.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: RoadTarget.cs ====
    using System;
    using System.Collections.Generic;
    using Il2CppPathfinding;
    using Il2CppScheduleOne.Vehicles.AI;
    using UnityEngine;


    /// <summary>
    /// Stage 3b — finding a DRIVEABLE target near the player for the TaxiApp pickup
    /// (the taxi drives from the stand to the player).
    ///
    /// The player is not guaranteed to stand on a road, so a raw
    /// <c>playerPos</c> dispatch is exactly the pattern that silently failed in
    /// round 5/11 (target off the graph). The resolver therefore builds a pool of
    /// candidate points, projects every one through
    /// <c>NavigationUtility.SampleVehicleGraph</c> and picks the projected point
    /// that is both (a) on the graph — snap delta ≤ <see cref="PreferredSnapDelta"/>
    /// — and (b) nearest to the player. The pool contains:
    /// <list type="bullet">
    ///   <item>the nearest node of the ROAD A* graph (<c>AstarPath.GetNearest</c>
    ///         masked to <c>VehicleAgent.RoadNodesGraphName</c>) — the "closest road
    ///         point" the game itself exposes, and</li>
    ///   <item>the proven road-sampling pattern: <c>SampleVehicleGraph</c> on the player
    ///         position plus forward/back/left/right offsets at 3/5/6/10 m.</item>
    /// </list>
    /// Every candidate and the winning rule are logged, so a failed run can be
    /// diagnosed (and the pool widened) from the log alone.
    /// </summary>
    internal static class RoadTarget
    {
        /// <summary>Snap delta (candidate → vehicle graph) that counts as "on the graph". Matches the proven road targets (0.1 m).</summary>
        private const float PreferredSnapDelta = 3f;

        /// <summary>Largest distance from the player that still counts as "arrived near the player" for the road-node candidate.</summary>
        private const float MaxRoadNodeDistance = 30f;

        private static bool _graphsDumped;

        // ------------------------------------------------------------------ pool

        /// <summary>
        /// Resolves the best navigation target near <paramref name="playerPos"/>.
        /// Returns <c>null</c> when no candidate could be projected onto the graph.
        /// </summary>
        internal static Vector3? FindNearPlayer(Vector3 playerPos, Vector3 playerForward)
        {
            TaxiLog.Verbose($"[target] resolving a road point near the player: player={SpikeCommands.Fmt(playerPos)} forward={SpikeCommands.Fmt(playerForward)}");

            if (playerForward.sqrMagnitude < 1e-4f)
                playerForward = Vector3.forward;
            Vector3 right = Vector3.Cross(Vector3.up, playerForward);
            if (right.sqrMagnitude < 1e-4f)
                right = Vector3.right;

            var candidates = new List<Candidate>();

            // (1) closest point on the ROAD graph the game exposes.
            Vector3? roadNode = NearestRoadNode(playerPos, out string roadInfo);
            float roadDistance = roadNode.HasValue ? Vector3.Distance(roadNode.Value, playerPos) : -1f;
            TaxiLog.Verbose($"[target] road-graph nearest: {(roadNode.HasValue ? SpikeCommands.Fmt(roadNode.Value) : "<none>")} distanceToPlayer={(roadDistance < 0f ? "-" : roadDistance.ToString("F1") + "m")} ({roadInfo})");
            if (roadNode.HasValue && roadDistance <= MaxRoadNodeDistance)
                candidates.Add(new Candidate("roadNode", roadNode.Value, fromRoadGraph: true));
            else if (roadNode.HasValue)
                Mod.Log.Warn($"[target] road node is {roadDistance:F1} m away from the player (> {MaxRoadNodeDistance:F0} m) — kept only as a logged fallback, not as a candidate.");

            // (2) the proven candidate sweep around the player.
            AddOffset(candidates, "player", playerPos);
            foreach (float d in new[] { 3f, 5f, 10f })
            {
                AddOffset(candidates, $"fwd+{d:F0}", playerPos + playerForward * d);
                AddOffset(candidates, $"fwd-{d:F0}", playerPos - playerForward * d);
                AddOffset(candidates, $"right+{d:F0}", playerPos + right * d);
                AddOffset(candidates, $"right-{d:F0}", playerPos - right * d);
            }

            // (3) a 6 m ring, so a sideways street still gets a near candidate.
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f * Mathf.Deg2Rad;
                Vector3 dir = new Vector3(
                    playerForward.x * Mathf.Cos(angle) + right.x * Mathf.Sin(angle),
                    0f,
                    playerForward.z * Mathf.Cos(angle) + right.z * Mathf.Sin(angle));
                AddOffset(candidates, $"ring{i * 45}", playerPos + dir * 6f);
            }

            // Project every candidate onto the vehicle graph.
            int projected = 0;
            foreach (Candidate c in candidates)
            {
                try
                {
                    c.Destination = NavigationUtility.SampleVehicleGraph(c.SamplePoint);
                    c.SnapDelta = Vector3.Distance(c.SamplePoint, c.Destination);
                    c.DistanceToPlayer = Vector3.Distance(c.Destination, playerPos);
                    c.Projected = true;
                    projected++;
                }
                catch (Exception ex)
                {
                    c.Error = ex.Message;
                }
            }

            TaxiLog.Verbose($"[target] {candidates.Count} candidate(s), {projected} projected onto the vehicle graph (eligible = snap delta ≤ {PreferredSnapDelta:F0} m):");
            foreach (Candidate c in candidates)
                TaxiLog.Verbose($"[target]   {c}");

            if (projected == 0)
            {
                Mod.Log.Error("[target] every SampleVehicleGraph call threw — no target available (see the errors above).");
                return null;
            }

            // Rule A: eligible (on-graph) candidate closest to the player; the road
            // graph node wins a tie because it is a real road point.
            Candidate? best = null;
            foreach (Candidate c in candidates)
            {
                if (!c.Projected || c.SnapDelta > PreferredSnapDelta)
                    continue;
                if (best == null || IsBetter(c, best))
                    best = c;
            }

            string rule = $"on-graph (delta ≤ {PreferredSnapDelta:F0} m) and closest to the player";

            // Rule B: nothing eligible → smallest snap delta, i.e. the point the
            // graph can actually serve, even if it sits further away.
            if (best == null)
            {
                foreach (Candidate c in candidates)
                {
                    if (!c.Projected)
                        continue;
                    if (best == null || c.SnapDelta < best.SnapDelta - 0.001f)
                        best = c;
                }

                rule = $"NO candidate within the {PreferredSnapDelta:F0} m delta budget → smallest snap delta";
                Mod.Log.Warn($"[target] {rule}.");
            }

            if (best == null)
            {
                Mod.Log.Error("[target] no usable candidate survived the rules.");
                return null;
            }

            TaxiLog.Verbose(
                $"[target] CHOSEN rule='{rule}' label='{best.Label}' fromRoadGraph={best.FromRoadGraph} " +
                $"sample={SpikeCommands.Fmt(best.SamplePoint)} destination={SpikeCommands.Fmt(best.Destination)} " +
                $"snapDelta={best.SnapDelta:F1}m distanceToPlayer={best.DistanceToPlayer:F1}m");
            return best.Destination;
        }

        private static bool IsBetter(Candidate c, Candidate current)
        {
            if (Math.Abs(c.DistanceToPlayer - current.DistanceToPlayer) > 0.5f)
                return c.DistanceToPlayer < current.DistanceToPlayer;

            // Within 0.5 m: prefer the real road node, then the smaller snap delta.
            if (c.FromRoadGraph != current.FromRoadGraph)
                return c.FromRoadGraph;
            return c.SnapDelta < current.SnapDelta;
        }

        private static void AddOffset(List<Candidate> candidates, string label, Vector3 point)
            => candidates.Add(new Candidate(label, point, fromRoadGraph: false));

        // ------------------------------------------------------------- road graph

        /// <summary>
        /// Closest node of the road A* graph — the vanilla "closest road point"
        /// (<c>AstarPath.GetNearest</c> masked to
        /// <c>VehicleAgent.RoadNodesGraphName</c>, with a "name contains road"
        /// fallback). Returns <c>null</c> (with the reason in <paramref name="info"/>)
        /// when no road graph or no node can be found.
        /// </summary>
        internal static Vector3? NearestRoadNode(Vector3 point, out string info)
        {
            try
            {
                global::Il2Cpp.AstarPath? astar = global::Il2Cpp.AstarPath.active;
                if (astar == null)
                {
                    info = "AstarPath.active is null";
                    return null;
                }

                var graphs = astar.graphs;
                if (graphs == null || graphs.Length == 0)
                {
                    info = "AstarPath.graphs is null/empty";
                    return null;
                }

                string wanted;
                try
                {
                    wanted = VehicleAgent.RoadNodesGraphName ?? "<null>";
                }
                catch (Exception ex)
                {
                    wanted = $"<threw: {ex.Message}>";
                }

                int index = -1;
                for (int i = 0; i < graphs.Length; i++)
                {
                    if (SafeGraphName(graphs[i], out string n).Equals(wanted, StringComparison.OrdinalIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }

                if (index < 0)
                {
                    for (int i = 0; i < graphs.Length; i++)
                    {
                        if (SafeGraphName(graphs[i], out string n).IndexOf("road", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            index = i;
                            break;
                        }
                    }
                }

                if (index < 0)
                {
                    var names = new List<string>();
                    for (int i = 0; i < graphs.Length; i++)
                        names.Add($"[{i}]{DescribeGraph(graphs[i])}");
                    info = $"no graph named '{wanted}' (or containing 'road') among {graphs.Length}: {string.Join(" ", names)}";
                    return null;
                }

                string graphName = SafeGraphName(graphs[index], out _);
                var constraint = new NNConstraint();
                constraint.graphMask = GraphMask.FromGraph(graphs[index]);

                NNInfo? nearest = astar.GetNearest(point, constraint);
                if (nearest == null || nearest.node == null)
                {
                    info = $"graph[{index}]='{graphName}' returned no node";
                    return null;
                }

                Vector3 position = nearest.position;
                info = $"graph[{index}]='{graphName}' (wanted='{wanted}') node={SpikeCommands.Fmt(position)}";
                return position;
            }
            catch (Exception ex)
            {
                info = $"threw: {ex.Message}";
                return null;
            }
        }

        /// <summary>One-shot A* graph inventory — printed with the parking-lot dump so a target failure can be diagnosed offline.</summary>
        internal static void DumpGraphs()
        {
            if (_graphsDumped)
                return;

            try
            {
                string wanted;
                try
                {
                    wanted = VehicleAgent.RoadNodesGraphName ?? "<null>";
                }
                catch (Exception ex)
                {
                    wanted = $"<threw: {ex.Message}>";
                }

                string vehicleGraph;
                try
                {
                    vehicleGraph = VehicleAgent.VehicleGraphName ?? "<null>";
                }
                catch (Exception ex)
                {
                    vehicleGraph = $"<threw: {ex.Message}>";
                }

                Mod.Log.Info($"[graphs] VehicleAgent.RoadNodesGraphName='{wanted}' VehicleGraphName='{vehicleGraph}'");

                global::Il2Cpp.AstarPath? astar = global::Il2Cpp.AstarPath.active;
                if (astar == null)
                {
                    Mod.Log.Info("[graphs] AstarPath.active is null (A* not initialised yet).");
                    return;
                }

                var graphs = astar.graphs;
                if (graphs == null || graphs.Length == 0)
                {
                    Mod.Log.Info("[graphs] AstarPath.graphs is null/empty.");
                    return;
                }

                Mod.Log.Info($"[graphs] AstarPath.graphs: {graphs.Length}");
                for (int i = 0; i < graphs.Length; i++)
                    Mod.Log.Info($"  [{i}] {DescribeGraph(graphs[i])}");

                _graphsDumped = true;
            }
            catch (Exception ex)
            {
                Mod.Log.Error($"[graphs] dump failed: {ex.Message}");
            }
        }

        private static string SafeGraphName(NavGraph graph, out string name)
        {
            name = "<null graph>";
            if (graph == null)
                return name;
            try
            {
                name = string.IsNullOrEmpty(graph.name) ? "<unnamed>" : graph.name;
            }
            catch (Exception ex)
            {
                name = $"<threw: {ex.Message}>";
            }

            return name;
        }

        private static string DescribeGraph(NavGraph graph)
        {
            if (graph == null)
                return "<null>";
            try
            {
                return $"name='{(string.IsNullOrEmpty(graph.name) ? "<unnamed>" : graph.name)}' type={graph.GetType().Name}";
            }
            catch (Exception ex)
            {
                return $"<threw: {ex.Message}>";
            }
        }

        /// <summary>One candidate point, its projection and the numbers the rule compares.</summary>
        private sealed class Candidate
        {
            internal readonly string Label;
            internal readonly Vector3 SamplePoint;
            internal readonly bool FromRoadGraph;

            internal bool Projected;
            internal Vector3 Destination;
            internal float SnapDelta = float.MaxValue;
            internal float DistanceToPlayer = float.MaxValue;
            internal string? Error;

            internal Candidate(string label, Vector3 samplePoint, bool fromRoadGraph)
            {
                Label = label;
                SamplePoint = samplePoint;
                FromRoadGraph = fromRoadGraph;
            }

            public override string ToString()
            {
                if (!Projected)
                    return $"[{Label}] sample={SpikeCommands.Fmt(SamplePoint)} SampleVehicleGraph FAILED: {Error}";

                string eligible = SnapDelta <= PreferredSnapDelta ? "ELIGIBLE" : "off-graph";
                return $"[{Label}] sample={SpikeCommands.Fmt(SamplePoint)} dest={SpikeCommands.Fmt(Destination)} " +
                       $"delta={SnapDelta:F1}m destDist={DistanceToPlayer:F1}m {eligible}{(FromRoadGraph ? " (road graph)" : string.Empty)}";
            }
        }
    }
    // ==== end TaxiDriver section: RoadTarget.cs ====
}
