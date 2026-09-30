using System;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vehicles.AI;
using UnityEngine;

namespace TaxiDriver;

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

    /// <summary>Corridor half-width after which the hard snap-back fires (metres).</summary>
    private const float HardMarginMeters = 5f;

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
        if (!SpikeState.PollingActive || SpikeState.NavReDispatchAt > 0f)
            return;

        if (Time.unscaledTime < _nextTickAt)
            return;

        _nextTickAt = Time.unscaledTime + TickIntervalSeconds;

        try
        {
            LandVehicle? veh = SpikeState.Vehicle;
            if (veh == null || veh.Pointer == IntPtr.Zero)
                return;

            VehicleAgent? agent = veh.Agent;
            if (agent == null)
                return;

            Vector3 position = veh.transform.position;
            if (!TryGetCorridor(agent, position, out Vector3 closest, out Vector3 direction, out float lateral))
                return;

            float drop = closest.y - position.y;
            bool inDitch = drop > MaxDropMeters;
            bool hard =
                lateral > HardMarginMeters ||
                (inDitch && lateral > SoftMarginMeters) ||
                (lateral > SoftMarginMeters && SteepGroundBelow(position));

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

            Vector3 ab = b - a;
            ab.y = 0f;
            float abSqr = ab.sqrMagnitude;
            if (abSqr < 1e-6f)
                continue;

            Vector3 ap = position - a;
            ap.y = 0f;
            float t = Mathf.Clamp01(Vector3.Dot(ap, ab) / abSqr);

            Vector3 p = a + ab * t;
            float dx = position.x - p.x;
            float dz = position.z - p.z;
            float dSqr = dx * dx + dz * dz;
            if (dSqr < bestSqr)
            {
                bestSqr = dSqr;
                closest = p;
                direction = ab.normalized;
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
        HardSnaps++;
        _excursionActive = true;

        Vector3 target = closest;
        Quaternion rotation = direction.sqrMagnitude > 1e-4f
            ? Quaternion.LookRotation(direction, Vector3.up)
            : veh.transform.rotation;

        Mod.Log.Warn(
            $"[road] HARD SNAP #{HardSnaps} back onto the road (lateral {TaxiDestinations.Num(lateral)} m" +
            $"{(inDitch ? $", {TaxiDestinations.Num(drop)} m below the road = ditch" : string.Empty)}) — " +
            $"the taxi may not leave the road; placed at {SpikeCommands.Fmt(target)}.");

        ApplyPosition(veh, target, rotation, zeroVelocity: true);
    }

    /// <summary>
    /// Places the vehicle (rigidbody-aware: position/rotation + velocity kill when
    /// a Rigidbody is present, transform fallback otherwise).
    /// </summary>
    private static void ApplyPosition(LandVehicle veh, Vector3 position, Quaternion rotation, bool zeroVelocity)
    {
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
}
