// ---------------------------------------------------------------------------
// DrivingPolicies.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract tests can still read that
// file's exact content.
//
//   Navigation/NavigationArrivalPolicy.cs -> section NavigationArrivalPolicy.cs
//   Navigation/NavigationCallbackPolicy.cs -> section NavigationCallbackPolicy.cs
//   Navigation/NavigationEndReason.cs -> section NavigationEndReason.cs
//   Navigation/NavigationRecoveryBudget.cs -> section NavigationRecoveryBudget.cs
//   Navigation/NavigationTerminalPolicy.cs -> section NavigationTerminalPolicy.cs
//   Navigation/PatrolStartPolicy.cs -> section PatrolStartPolicy.cs
//   Navigation/RecoveryRecurrencePolicy.cs -> section RecoveryRecurrencePolicy.cs
//   Navigation/TerminalRescuePolicy.cs -> section TerminalRescuePolicy.cs
//   Roads/RoadProjectionPolicy.cs  -> section RoadProjectionPolicy.cs
//   Roads/RoadSnapSafetyPolicy.cs  -> section RoadSnapSafetyPolicy.cs
//   Driving/BrakeHoldPolicy.cs     -> section BrakeHoldPolicy.cs
//   Driving/CruiseSpeedPolicy.cs   -> section CruiseSpeedPolicy.cs
//   Driving/NativeActuatorPolicy.cs -> section NativeActuatorPolicy.cs
//   Driving/NativeActuatorScope.cs -> section NativeActuatorScope.cs
//   Driving/ReversePedalPolicy.cs  -> section ReversePedalPolicy.cs
//   Traffic/PrivateFlagsScope.cs   -> section PrivateFlagsScope.cs
//   Traffic/TrafficLightPolicy.cs  -> section TrafficLightPolicy.cs
//
// ---------------------------------------------------------------------------

namespace TaxiDriver
{
    // ==== TaxiDriver section: NavigationArrivalPolicy.cs ====
    using System;


    /// <summary>The resolved destination has one arrival radius for patrol and ride verdicts.</summary>
    internal static class NavigationArrivalPolicy
    {
        internal const float ArrivalMeters = 10f;
        internal const float StandingKmh = 1f;

        internal static bool IsWithinArrival(float distance) =>
            float.IsFinite(distance) && distance >= 0f && distance <= ArrivalMeters;

        internal static bool IsStandingAtArrival(bool atLastWaypoint, float distance, float signedSpeed) =>
            atLastWaypoint && IsWithinArrival(distance) && Math.Abs(signedSpeed) <= StandingKmh;

        /// <summary>
        /// Readability is separate from value (brief §8.2): it only says whether the sample is a
        /// usable finite number. The reader reports its own success separately, so a valid
        /// negative speed (including -1) is a real speed and must not be rejected as a read
        /// failure. The -1 substitute a failed reader leaves behind is excluded by the separate
        /// readability flag, never by its value.
        /// </summary>
        internal static bool IsReadableSpeed(float signedSpeed) => float.IsFinite(signedSpeed);

        /// <summary>
        /// Confirmed halt: a readable, finite speed inside the standing band while the car
        /// is not actively reversing. A valid negative speed inside the band is a halt; a
        /// magnitude above the band (either sign) is motion; a speed-read failure is never a
        /// halt (brief §8.2).
        /// </summary>
        internal static bool IsConfirmedHalt(bool speedReadable, float signedSpeed, bool reversing) =>
            speedReadable && IsReadableSpeed(signedSpeed) && !reversing &&
            Math.Abs(signedSpeed) <= StandingKmh;

        /// <summary>
        /// Final arrival verdict: target proximity AND a readable, confirmed halt. Distance
        /// alone (or a distance inside the radius while still moving/reversing) never
        /// completes a taxi (brief §8.2).
        /// </summary>
        internal static bool IsConfirmedArrival(float distance, bool speedReadable, float signedSpeed, bool reversing) =>
            IsWithinArrival(distance) && IsConfirmedHalt(speedReadable, signedSpeed, reversing);
    }
    // ==== end TaxiDriver section: NavigationArrivalPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: SupervisedArrivalPolicy.cs ====

    /// <summary>
    /// The single arrival decision every path shares (patrol supervision, ride finalization,
    /// pickup callback and the polling terminal). Proximity alone, a moving/reversing car, an
    /// unreadable speed, or a parked state carried over from a previous order never complete an
    /// order (brief §8.2). A car that merely STARTED inside the radius needs current-operation
    /// evidence — a native current <c>Complete</c> result or real drive progress in THIS
    /// operation — before proximity may complete it.
    /// </summary>
    internal static class SupervisedArrivalPolicy
    {
        internal static bool IsConfirmedArrival(bool currentRunComplete, bool operationProgress,
            float distance, bool speedReadable, float signedSpeed, bool reversing) =>
            NavigationArrivalPolicy.IsConfirmedArrival(distance, speedReadable, signedSpeed, reversing) &&
            (currentRunComplete || operationProgress);
    }
    // ==== end TaxiDriver section: SupervisedArrivalPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: NavigationCallbackPolicy.cs ====

    /// <summary>Pure gate that accepts only a current, still-active navigation callback.</summary>
    internal static class NavigationCallbackPolicy
    {
        internal static bool ShouldAcceptResult(int callbackOrder, int currentOrder, bool pollingActive) =>
            pollingActive && callbackOrder == currentOrder;
    }
    // ==== end TaxiDriver section: NavigationCallbackPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: NavigationEndReason.cs ====
    using System;


    /// <summary>One truthful terminal verdict shared by ride feedback and fare-end reporting.</summary>
    internal static class NavigationEndReason
    {
        internal static string ForRide(bool gaveUp, float distance, float arrivalRadius, string? callback)
        {
            if (gaveUp) return "navigation gave up";
            if (string.Equals(callback, "Failed", StringComparison.OrdinalIgnoreCase)) return "navigation failed";
            if (string.Equals(callback, "Stopped", StringComparison.OrdinalIgnoreCase)) return "navigation stopped";
            if (!float.IsFinite(distance) || distance > arrivalRadius) return "stopped short";
            return "arrived";
        }
    }
    // ==== end TaxiDriver section: NavigationEndReason.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: NavigationRecoveryBudget.cs ====

    /// <summary>Operation-wide recovery cap and meaningful-approach deadline, independent of native dispatch orders.</summary>
    internal sealed class NavigationRecoveryBudget
    {
        private bool _active;
        private float _deadline;
        private int _attempts;
        private bool _lastRescueUsed;
        private float _timeout;
        private float _bestDistance = float.PositiveInfinity;
        private float _progressMark = float.PositiveInfinity;

        // A persistent high-water mark, never a sum of jitter or dispatch-local motion.
        internal void ObserveProgress(float now, float distance, bool eligible)
        {
            if (!_active || HasExpired(now) || !float.IsFinite(distance) || distance < 0f) return;
            if (float.IsPositiveInfinity(_bestDistance))
            {
                _bestDistance = _progressMark = distance;
                return;
            }
            float previousBestDistance = _bestDistance;
            _bestDistance = System.Math.Min(_bestDistance, distance);
            if (!eligible)
            {
                // Consume only new artificial ground; preserve pending genuine progress.
                _progressMark -= previousBestDistance - _bestDistance;
                return;
            }
            if (_progressMark - _bestDistance < 2f) return;
            _progressMark = _bestDistance;
            _deadline = now + _timeout;
        }

        internal int Attempts => _attempts;

        internal void Begin(float now, float timeout)
        {
            _active = true;
            _deadline = now + timeout;
            _timeout = timeout;
            _bestDistance = _progressMark = float.PositiveInfinity;
            _attempts = 0;
            _lastRescueUsed = false;
        }

        internal void Reset()
        {
            _active = false;
            _deadline = 0f;
            _attempts = 0;
            _lastRescueUsed = false;
        }

        internal void ShiftPause(float seconds)
        {
            if (_active) _deadline += seconds;
        }

        internal bool HasExpired(float now) => _active && now >= _deadline;

        internal bool TrySpend(float now, int cap)
        {
            if (!_active || HasExpired(now) || _lastRescueUsed || _attempts >= System.Math.Max(2, cap))
                return false;
            _attempts++;
            return true;
        }

        internal bool TryLastRescue(float now)
        {
            if (!_active || HasExpired(now) || _lastRescueUsed)
                return false;
            _lastRescueUsed = true; // consume before the native attempt, even when it fails
            return true;
        }
    }
    // ==== end TaxiDriver section: NavigationRecoveryBudget.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: NavigationTerminalPolicy.cs ====

    /// <summary>Clear the previous verdict only at a genuinely new operation boundary, never on recovery.</summary>
    internal static class NavigationTerminalPolicy
    {
        internal static void Begin(ref bool gaveUp, ref bool arrived, ref string? callback)
        {
            gaveUp = false;
            arrived = false;
            callback = null;
        }
    }
    // ==== end TaxiDriver section: NavigationTerminalPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: PatrolStartPolicy.cs ====

    /// <summary>Order-bound start ownership; ordinary stalls must not preempt the fixed self-test.</summary>
    internal static class PatrolStartPolicy
    {
        internal static bool ShouldWait(bool active, int selfTestOrder, int currentOrder,
            bool driveStartConfirmed, float elapsed, float selfTestSeconds) =>
            active && selfTestOrder == currentOrder && !driveStartConfirmed && elapsed < selfTestSeconds;
    }
    // ==== end TaxiDriver section: PatrolStartPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: RecoveryRecurrencePolicy.cs ====

    /// <summary>Operation-local stall history; independent of native navigation orders.</summary>
    internal sealed class RecoveryRecurrencePolicy
    {
        // A reverse can move several metres while leaving the same obstruction unresolved.
        internal const float StallRadiusMeters = 6f;
        internal const float ForwardApproachMeters = 4f;
        private float _bestDistance, _progressMark;
        private bool _hasStall;
        private float _stallX, _stallZ;

        internal void Begin(float distance)
        {
            _hasStall = false;
            _bestDistance = _progressMark = float.IsFinite(distance) && distance >= 0f
                ? distance : float.PositiveInfinity;
        }

        // Fixed operation target, not a redispatch-local route/goal. Ineligible movement
        // consumes only newly acquired ground, so returning forward cannot earn it again.
        internal bool ObserveProgress(float distance, bool eligibleForward)
        {
            if (!float.IsFinite(distance) || distance < 0f) return false;
            if (float.IsPositiveInfinity(_bestDistance))
            {
                _bestDistance = _progressMark = distance;
                return false;
            }
            float previousBest = _bestDistance;
            _bestDistance = System.Math.Min(_bestDistance, distance);
            if (!eligibleForward)
            {
                _progressMark -= previousBest - _bestDistance;
                return false;
            }
            if (_progressMark - _bestDistance < ForwardApproachMeters) return false;
            _progressMark = _bestDistance;
            _hasStall = false;
            return true;
        }

        internal bool RegisterStall(float x, float z)
        {
            if (!float.IsFinite(x) || !float.IsFinite(z)) return false;
            float dx = x - _stallX, dz = z - _stallZ;
            bool recurring = _hasStall && dx * dx + dz * dz <= StallRadiusMeters * StallRadiusMeters;
            // Escape credit must be earned after this stall, not borrowed from the
            // preceding ordinary drive. Keep closest-ever ground across redispatch.
            _progressMark = _bestDistance;
            if (!recurring)
            {
                _stallX = x;
                _stallZ = z;
                _hasStall = true;
            }
            return recurring;
        }
    }
    // ==== end TaxiDriver section: RecoveryRecurrencePolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: TerminalRescuePolicy.cs ====

    /// <summary>
    /// Terminal-rescue decision: after the operation recovery budget is spent, one
    /// guarded relocation may still be attempted before the ride is given up.
    /// </summary>
    /// <remarks>
    /// 2026-10-15 gameplay log: the old wiring gave up whenever the taxi was still
    /// on the vehicle graph ("no teleport needed, giving up") and abandoned a ride
    /// 106.7 m from its destination while the car sat stuck at (40.1, 0.6, 52.5).
    /// Graph membership only says a route exists — not that the car can reach it.
    /// The relocation itself stays guarded (RoadKeeper.TryRescueStartup: supported
    /// free spot, ground snap, oriented-box clearance, occupancy check).
    /// </remarks>
    internal static class TerminalRescuePolicy
    {
        internal enum Action
        {
            GiveUp,
            AttemptGuardedRelocation,
        }

        /// <summary>
        /// Decides whether the terminal rescue attempts the guarded relocation.
        /// The graph state never vetoes it; only a spent one-shot rescue budget does.
        /// </summary>
        internal static Action Decide(bool onGraph, bool lastRescueAvailable)
        {
            _ = onGraph; // documented no-veto parameter, see remarks
            return lastRescueAvailable ? Action.AttemptGuardedRelocation : Action.GiveUp;
        }
    }
    // ==== end TaxiDriver section: TerminalRescuePolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: RoadProjectionPolicy.cs ====
    using System;
    using System.Numerics;


    /// <summary>XZ parameter/distance, original 3D segment height; no Unity dependency.</summary>
    internal static class RoadProjectionPolicy
    {
        internal static bool TryProject(Vector3 a, Vector3 b, Vector3 position,
            out Vector3 point, out Vector3 direction, out float lateral)
        {
            point = default;
            direction = default;
            lateral = 0;
            if (!Finite(a) || !Finite(b) || !Finite(position))
                return false;
            Vector3 segment = b - a;
            Vector3 flat = new(segment.X, 0, segment.Z);
            float lengthSquared = flat.LengthSquared();
            if (!float.IsFinite(lengthSquared) || lengthSquared < 1e-6f)
                return false;
            float dot = Vector3.Dot(new(position.X - a.X, 0, position.Z - a.Z), flat);
            if (!float.IsFinite(dot))
                return false;
            float t = Math.Clamp(dot / lengthSquared, 0f, 1f);
            point = a + segment * t;
            direction = Vector3.Normalize(flat);
            lateral = new Vector2(position.X - point.X, position.Z - point.Z).Length();
            return Finite(point) && Finite(direction) && float.IsFinite(lateral);
        }

        internal static bool Finite(Vector3 value) =>
            float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z);
    }
    // ==== end TaxiDriver section: RoadProjectionPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: RoadSnapSafetyPolicy.cs ====
    using System;
    using System.Numerics;


    /// <summary>Fail-closed vertical support and measured chassis/root placement policy.</summary>
    internal static class RoadSnapSafetyPolicy
    {
        // A short probe around the interpolated road elevation, not a roof-to-basement search.
        internal const float SupportProximityMeters = 0.75f;
        internal const float BottomClearanceMeters = 0.05f;

        internal static bool NeedsHardCorrection(float lateral, float drop, bool steep) =>
            float.IsFinite(lateral) && float.IsFinite(drop) &&
            (lateral > 5f || (lateral > 3f && (drop > 2f || steep)));

        internal static bool IsSafeSupport(bool trigger, bool dynamic, bool character, bool vehicle,
            bool self, float slope) =>
            !trigger && !dynamic && !character && !vehicle && !self &&
            float.IsFinite(slope) && slope >= 0f && slope <= 40f;

        internal static bool TryPlacementBounds(ReadOnlySpan<Vector3> worldRootOffsets, Quaternion currentRotation,
            Quaternion targetRotation, out Vector3 min, out Vector3 max)
        {
            min = new(float.MaxValue);
            max = new(float.MinValue);
            float currentLength = currentRotation.LengthSquared();
            float targetLength = targetRotation.LengthSquared();
            if (worldRootOffsets.Length != 8 || !float.IsFinite(currentLength) || !float.IsFinite(targetLength) ||
                Math.Abs(currentLength - 1f) > 0.001f || Math.Abs(targetLength - 1f) > 0.001f)
                return false;
            Quaternion delta = targetRotation * Quaternion.Inverse(currentRotation);
            foreach (Vector3 corner in worldRootOffsets)
            {
                if (!RoadProjectionPolicy.Finite(corner))
                    return false;
                Vector3 offset = Vector3.Transform(corner, delta);
                if (!RoadProjectionPolicy.Finite(offset))
                    return false;
                min = Vector3.Min(min, offset);
                max = Vector3.Max(max, offset);
            }
            Vector3 size = max - min;
            return RoadProjectionPolicy.Finite(size) && size.X > 0 && size.Y > 0 && size.Z > 0;
        }

        internal static bool TryRootHeight(float projectedY, float groundY, float rootY, float bottomY,
            bool supported, bool free, out float targetY)
        {
            targetY = 0f;
            if (!supported || !free || !float.IsFinite(projectedY) || !float.IsFinite(groundY) ||
                !float.IsFinite(rootY) || !float.IsFinite(bottomY) ||
                Math.Abs(groundY - projectedY) > SupportProximityMeters)
                return false;
            targetY = groundY + (rootY - bottomY) + BottomClearanceMeters;
            return float.IsFinite(targetY);
        }
    }
    // ==== end TaxiDriver section: RoadSnapSafetyPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: BrakeHoldPolicy.cs ====

    /// <summary>Park intent requests native handbrake on every consumption.
    /// Previous HandbrakeApplied is an output, never proof the next request is held.</summary>
    internal static class BrakeHoldPolicy
    {
        internal static bool ShouldRequestPark(bool activeOrder, bool playerDriver, bool parkIntent)
            => !activeOrder && !playerDriver && parkIntent;
    }
    // ==== end TaxiDriver section: BrakeHoldPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: CruiseSpeedPolicy.cs ====

    /// <summary>Unity-free selection of the pre-reduction forward cruise target, in km/h.</summary>
    internal static class CruiseSpeedPolicy
    {
        internal const float CruiseKmh = 50f;

        internal static float BaseTargetKmh(float nativeBaseKmh, bool ownedTaxi, bool activeOrder,
            bool autoDriving, bool playerDriving, bool reversing, bool nativeOverride)
        {
            if (!ownedTaxi || !activeOrder || !autoDriving || playerDriving || reversing || nativeOverride ||
                !float.IsFinite(nativeBaseKmh) || nativeBaseKmh <= 0f)
                return nativeBaseKmh;
            return CruiseKmh;
        }
    }
    // ==== end TaxiDriver section: CruiseSpeedPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: NativeActuatorPolicy.cs ====

    internal enum NativeActuatorMode { None, Reverse, Park }

    /// <summary>Pure selection at the consumer; ownership is the exact live tracked
    /// pointer, not vehicle type. A pending supervised window takes precedence over park.</summary>
    internal static class NativeActuatorPolicy
    {
        internal static NativeActuatorMode Choose(bool host, bool exactOwnedVehicle,
            bool paused, bool playerDriver, bool reverseWindow, bool activeOrder, bool parkIntent)
        {
            if (!host || !exactOwnedVehicle || paused || playerDriver)
                return NativeActuatorMode.None;
            if (reverseWindow) return NativeActuatorMode.Reverse;
            return BrakeHoldPolicy.ShouldRequestPark(activeOrder, playerDriver, parkIntent)
                ? NativeActuatorMode.Park : NativeActuatorMode.None;
        }
    }
    // ==== end TaxiDriver section: NativeActuatorPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: NativeActuatorScope.cs ====
    using System;


    /// <summary>Call-scoped final native inputs. Does not write derived output flags.</summary>
    internal sealed class NativeActuatorScope : IDisposable
    {
        private readonly (float Throttle, bool Handbrake, bool Controls) _saved;
        private readonly Action<float> _throttle;
        private readonly Action<bool> _handbrake;
        private readonly Action<bool> _controls;
        private bool _restored;

        private NativeActuatorScope((float, bool, bool) saved, Action<float> throttle,
            Action<bool> handbrake, Action<bool> controls)
        {
            _saved = saved;
            _throttle = throttle;
            _handbrake = handbrake;
            _controls = controls;
        }

        internal static NativeActuatorScope Begin(Func<(float, bool, bool)> read,
            Action<float> throttle, Action<bool> handbrake, Action<bool> controls, bool reverse)
        {
            var scope = new NativeActuatorScope(read(), throttle, handbrake, controls);
            try
            {
                throttle(ReversePedalPolicy.CurrentThrottleFor(reverse));
                handbrake(!reverse);
                controls(true);
                return scope;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            if (_restored) return;
            _restored = true;
            // Attempt every saved input even if a destroyed native handle rejects one.
            try { _throttle(_saved.Throttle); }
            finally
            {
                try { _handbrake(_saved.Handbrake); }
                finally { _controls(_saved.Controls); }
            }
        }
    }
    // ==== end TaxiDriver section: NativeActuatorScope.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: ReversePedalPolicy.cs ====

    /// <summary>Final currentThrottle request at ApplyThrottle, not a gear output.
    /// Ordinary AI/player inputs are restored after the call; no render Update hold.</summary>
    internal static class ReversePedalPolicy
    {
        internal const float PedalInput = -0.7f;
        internal static float CurrentThrottleFor(bool reverse) => reverse ? PedalInput : 0f;
    }
    // ==== end TaxiDriver section: ReversePedalPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: PrivateFlagsScope.cs ====
    using System;


    /// <summary>Swap a private reference only for synchronous native consumption.</summary>
    internal sealed class PrivateFlagsScope<T> : IDisposable where T : class
    {
        private readonly T _saved;
        private readonly Action<T> _write;
        private bool _restored;

        private PrivateFlagsScope(T saved, Action<T> write) => (_saved, _write) = (saved, write);

        internal static PrivateFlagsScope<T> Begin(Func<T> read, Action<T> write,
            Func<T, T> copy, Func<T, T, bool> sameNativeObject,
            Action<PrivateFlagsScope<T>>? publish = null)
        {
            T original = read();
            T privateCopy = copy(original);
            if (privateCopy == null || sameNativeObject(original, privateCopy))
                throw new InvalidOperationException("Flags copy must be a physically distinct native object.");
            var scope = new PrivateFlagsScope<T>(original, write);
            publish?.Invoke(scope); // Harmony state exists before any reference write.
            try
            {
                write(privateCopy);
                return scope;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            if (_restored) return;
            _write(_saved);
            _restored = true;
        }
    }
    // ==== end TaxiDriver section: PrivateFlagsScope.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: TrafficLightPolicy.cs ====

    /// <summary>Authority/control gate only. No obstacle classification or recovery timers.</summary>
    internal static class TrafficLightPolicy
    {
        internal static bool ShouldIgnore(bool host, bool exactOwnedTaxi, bool activeOrder,
            bool autoDriving, bool playerDriver)
            => host && exactOwnedTaxi && activeOrder && autoDriving && !playerDriver;

        internal static bool CanBypass(bool eligible, bool physical, bool reversing, bool paused)
            => eligible && physical && !reversing && !paused;

        internal static bool IsTrafficWait(bool readable, bool stopped, bool trafficHit,
            bool genericHit, bool filteredHit)
            => readable && stopped && trafficHit && !genericHit && !filteredHit;
    }

    internal sealed class PatrolReleaseGate
    {
        internal bool Requested { get; private set; }
        internal bool Busy { get; private set; }
        internal bool Completed { get; private set; }
        internal bool TryBegin()
        {
            if (Busy || Completed) return false;
            Requested = true;
            Busy = true;
            return true;
        }
        internal void End(bool completed)
        {
            if (!Busy) return;
            Completed = completed;
            Busy = false;
        }
    }

    internal static class PatrolBindingPolicy
    {
        internal static bool CanUse(bool owner, bool network, bool registered, bool serverReady, bool idle)
            => owner && network && registered && serverReady && idle;
    }

    internal sealed class TrafficWaitBudget
    {
        internal const float MaxSeconds = 30f;
        internal float HeldSeconds { get; private set; }
        internal float Observe(bool waiting, float seconds, bool withinArrival)
        {
            if (!waiting || withinArrival || !float.IsFinite(seconds) || seconds <= 0f) return 0f;
            float held = System.Math.Min(seconds, System.Math.Max(0f, MaxSeconds - HeldSeconds));
            HeldSeconds = System.Math.Min(MaxSeconds, HeldSeconds + held);
            return held;
        }
        internal void Reset() => HeldSeconds = 0f;
    }
    // ==== end TaxiDriver section: TrafficLightPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: ReverseManoeuvrePolicy.cs ====
    using System;
    using System.Numerics;


    /// <summary>
    /// Distinguishable phases of one bounded reverse manoeuvre (brief §6.1). The phase
    /// is the single managed truth for "does a reverse request own the controls"; a bare
    /// native request flag or a stale timestamp never is (brief §7.3).
    /// </summary>
    internal enum ReversePhase
    {
        /// <summary>No reverse request owns the controls.</summary>
        Inactive,

        /// <summary>Native request issued; actual reverse motion is still unconfirmed.</summary>
        Requested,

        /// <summary>Sustained directed reverse motion confirmed; clearance still supervised.</summary>
        Moving,

        /// <summary>Reverse intent ended; controlled re-check before at most one forward order.</summary>
        Transition,
    }

    /// <summary>
    /// One bounded reverse manoeuvre inside a navigation operation (brief §6/§7).
    /// Unity-free: it consumes measured positions/speeds and decides the phase, the
    /// directed horizontal net reverse displacement, the sustained-movement proof and
    /// the bounded deadline. The overall cap is never restarted by a phase change or a
    /// repeated movement impulse (brief §7.2).
    /// </summary>
    internal sealed class ReverseManoeuvrePolicy
    {
        /// <summary>Directed reverse displacement (metres) that opens the movement proof.</summary>
        internal const float MoveConfirmMeters = 0.25f;

        /// <summary>Sustained time (seconds) the directed displacement must hold to confirm motion.</summary>
        internal const float MoveConfirmSeconds = 0.3f;

        /// <summary>Directed reverse (metres) that counts as a meaningful, forward-enabling escape.</summary>
        internal const float ForwardEscapeMeters = 0.5f;

        /// <summary>Whole-manoeuvre cap in seconds, independent of any phase change (brief §7.2).</summary>
        internal const float OverallCapSeconds = 6f;

        /// <summary>
        /// Minimum dot product of the current horizontal forward and the captured forward
        /// (brief §7.1): below this the car has yawed far enough that the simple straight
        /// reverse the policy measured no longer describes the manoeuvre, so it must not
        /// confirm movement.
        /// </summary>
        internal const float MinForwardDot = 0.5f;

        private long _operationId;
        private int _navOrder;
        private ReversePhase _phase;
        private Vector3 _origin;
        private Vector3 _direction;
        private float _requestedAt;
        private float _requestDeadline;
        private float _overallDeadline;
        private float _sustainSince;
        private float _bestReverse;
        private float _currentReverse;
        private bool _sustaining;
        private bool _confirmed;

        internal ReversePhase Phase => _phase;
        internal bool Active => _phase != ReversePhase.Inactive;
        internal bool MovedConfirmed => _confirmed;
        internal long OperationId => _operationId;
        internal int NavOrder => _navOrder;
        internal float RequestedAt => _requestedAt;
        internal float CurrentReverseMeters => _currentReverse;

        /// <summary>
        /// Starts a manoeuvre for one operation/order. Captures the origin and the
        /// horizontal reverse direction, so later motion is measured along that axis and
        /// not as arbitrary 3D displacement (brief §7.1). <paramref name="requestWindowSeconds"/>
        /// is the native request window (1.5 s / 3 s); the consumer enforces the SMALLER of the
        /// request window and the overall cap, so an expired request never keeps authorizing
        /// input (brief §7.3).
        /// </summary>
        internal void Begin(long operationId, int navOrder, Vector3 origin, Vector3 reverseDirection, float now,
            float requestWindowSeconds = OverallCapSeconds)
        {
            _operationId = operationId;
            _navOrder = navOrder;
            _origin = origin;
            _direction = HorizontalUnit(reverseDirection);
            _requestedAt = now;
            float window = float.IsFinite(requestWindowSeconds) && requestWindowSeconds > 0f
                ? requestWindowSeconds : OverallCapSeconds;
            _requestDeadline = float.IsFinite(now) ? now + window : window;
            _overallDeadline = float.IsFinite(now) ? now + OverallCapSeconds : OverallCapSeconds;
            _sustainSince = 0f;
            _bestReverse = 0f;
            _currentReverse = 0f;
            _sustaining = false;
            _confirmed = false;
            _phase = ReversePhase.Requested;
        }

        /// <summary>True while this manoeuvre is the active request for exactly this operation/order.</summary>
        internal bool Matches(long operationId, int navOrder) =>
            Active && _operationId == operationId && _navOrder == navOrder;

        /// <summary>
        /// Consumer-side validity (brief §7.3): active, current operation/order, not in the
        /// forward transition and inside BOTH the request window and the overall cap. A bare
        /// positive timestamp is not a grant; an unreadable time fails closed (no input).
        /// </summary>
        internal bool IsCurrentFor(long operationId, int navOrder, float now) =>
            Active && _phase != ReversePhase.Transition && _operationId == operationId &&
            _navOrder == navOrder && !Expired(now);

        internal bool Expired(float now) =>
            Active && (!float.IsFinite(now) || now >= _requestDeadline || now >= _overallDeadline);

        /// <summary>
        /// Moves every deadline — request window, overall cap and the sustain/request stamps —
        /// by the paused time exactly once (brief §7.2): paused time never counts as movement
        /// proof and never shortens the remaining time after resuming.
        /// </summary>
        internal void ShiftPause(float seconds)
        {
            if (!Active || !float.IsFinite(seconds) || seconds <= 0f) return;
            _overallDeadline += seconds;
            _requestDeadline += seconds;
            if (_requestedAt > 0f) _requestedAt += seconds;
            if (_sustaining && _sustainSince > 0f) _sustainSince += seconds;
        }

        /// <summary>
        /// Net horizontal displacement along the reverse axis; positive means the car
        /// backed up. Lateral slip and height changes are excluded (brief §7.1).
        /// </summary>
        internal float DirectedNetReverseMeters(Vector3 position)
        {
            if (!Finite(position) || !Finite(_origin)) return 0f;
            return (position.X - _origin.X) * _direction.X + (position.Z - _origin.Z) * _direction.Z;
        }

        /// <summary>
        /// Samples the car. Returns true only once sustained directed reverse motion is
        /// confirmed. Confirmation needs a readable, finite speed sample and a finite
        /// position/time; an unreadable/NaN sample, a degenerate direction or a yaw drift
        /// beyond <see cref="MinForwardDot"/> never confirm movement. The native speed SIGN is
        /// deliberately not interpreted — the positional directed displacement is the proof.
        /// </summary>
        internal bool Observe(Vector3 position, bool speedReadable, float signedSpeed, float now,
            float forwardDot = 1f)
        {
            if (!Active || _phase == ReversePhase.Transition) return _confirmed;
            if (!speedReadable || !float.IsFinite(signedSpeed) || !Finite(position) || !float.IsFinite(now) ||
                _direction == Vector3.Zero || !float.IsFinite(forwardDot) || forwardDot < MinForwardDot)
            {
                _sustaining = false;
                return _confirmed;
            }
            float net = DirectedNetReverseMeters(position);
            _currentReverse = net;
            if (net > _bestReverse) _bestReverse = net;
            bool backing = net >= MoveConfirmMeters;
            if (!backing)
            {
                _sustaining = false;
                return _confirmed;
            }
            if (!_sustaining)
            {
                _sustaining = true;
                _sustainSince = now;
            }
            else if (now - _sustainSince >= MoveConfirmSeconds)
            {
                _confirmed = true;
                _phase = ReversePhase.Moving;
            }
            return _confirmed;
        }

        /// <summary>
        /// Front gate for the forward transition: a confirmed, meaningful reverse — measured by
        /// the CURRENT directed net, not a best-ever high-water mark a retrace could fake — plus
        /// a front that is no longer blocked. 0.6 m with a still-blocked front is not an escape
        /// (brief §7.1/§7.5).
        /// </summary>
        internal bool CanDispatchForward(float currentReverseMeters, bool frontBlocked) =>
            _confirmed && float.IsFinite(currentReverseMeters) &&
            currentReverseMeters >= ForwardEscapeMeters && !frontBlocked;

        /// <summary>Revokes the reverse intent and enters the bounded transition (no more
        /// reverse samples accepted). Returns true when an active manoeuvre was ended.</summary>
        internal bool BeginTransition()
        {
            if (_phase == ReversePhase.Inactive) return false;
            _phase = ReversePhase.Transition;
            return true;
        }

        /// <summary>Clears the manoeuvre completely (idempotent).</summary>
        internal bool End()
        {
            if (_phase == ReversePhase.Inactive) return false;
            _phase = ReversePhase.Inactive;
            return true;
        }

        private static Vector3 HorizontalUnit(Vector3 direction)
        {
            var flat = new Vector3(direction.X, 0f, direction.Z);
            float length = flat.Length();
            if (!float.IsFinite(length) || length < 1e-4f) return Vector3.Zero;
            return flat / length;
        }

        private static bool Finite(Vector3 value) =>
            float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z);
    }
    // ==== end TaxiDriver section: ReverseManoeuvrePolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: RideSatisfactionPolicy.cs ====

    /// <summary>
    /// A dispatch may be skipped only for the exact same completed logical target with a
    /// readable, confirmed stop. A different nearby target is still a current order and
    /// must drive (brief §8.2).
    /// </summary>
    internal static class RideSatisfactionPolicy
    {
        internal const float SameTargetMeters = 1f;

        internal static bool IsAlreadySatisfied(bool sameCompletedTarget, bool callbackComplete, bool gaveUp,
            float distance, bool speedReadable, float signedSpeed, bool reversing) =>
            sameCompletedTarget && callbackComplete && !gaveUp &&
            NavigationArrivalPolicy.IsConfirmedArrival(distance, speedReadable, signedSpeed, reversing);
    }
    // ==== end TaxiDriver section: RideSatisfactionPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: NavigationCallbackOutcomePolicy.cs ====
    using System;


    /// <summary>What an accepted navigation callback hands to the supervisor (brief §8.1).</summary>
    internal enum NavigationOutcomeAction
    {
        /// <summary>Not an accepted current result — no state changes.</summary>
        Ignore,

        /// <summary>Terminal completion the caller may act on (a pickup Complete).</summary>
        Finalize,

        /// <summary>One bounded recovery decision; the ride and meter stay alive.</summary>
        BoundedRecovery,

        /// <summary>A current Complete for a ride: confirm a halt before declaring arrival.</summary>
        ConfirmArrival,

        /// <summary>An unexpected Stopped: a distinct, safely explicit failure — never a restart.</summary>
        ExplicitFailure,
    }

    /// <summary>
    /// A single failed navigation run is not automatically the end of the operation (brief
    /// §8.1): a current <c>Failed</c> result hands ONE bounded recovery decision to supervision
    /// instead of finalizing — for a running ride AND for a pickup on a valid operation. A
    /// current <c>Complete</c> asks the ride to confirm a halt; an unexpected <c>Stopped</c> is a
    /// distinct explicit failure, never a universal restart, and a <c>Stopped</c> from our own
    /// controlled transition is ignored (brief §8.1/§7.4).
    /// </summary>
    internal static class NavigationCallbackOutcomePolicy
    {
        internal static NavigationOutcomeAction Decide(string? result, bool navToPlayer, bool operationValid,
            bool controlledStop)
        {
            if (!operationValid) return NavigationOutcomeAction.Ignore;
            // Review D16: while OUR OWN controlled transition owns navigation, no callback may act
            // — a stop/dispatch we issued must never re-enter the state machine.
            if (controlledStop) return NavigationOutcomeAction.Ignore;
            bool complete = string.Equals(result, "Complete", StringComparison.OrdinalIgnoreCase);
            bool failed = string.Equals(result, "Failed", StringComparison.OrdinalIgnoreCase);
            bool stopped = string.Equals(result, "Stopped", StringComparison.OrdinalIgnoreCase);

            if (stopped) return NavigationOutcomeAction.ExplicitFailure;
            if (failed) return NavigationOutcomeAction.BoundedRecovery;
            if (complete)
                return navToPlayer ? NavigationOutcomeAction.Finalize : NavigationOutcomeAction.ConfirmArrival;
            return NavigationOutcomeAction.Finalize;
        }
    }
    // ==== end TaxiDriver section: NavigationCallbackOutcomePolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: DestinationIdentityPolicy.cs ====
    using System;
    using System.Numerics;


    /// <summary>
    /// Logical destination identity (brief §8.2): a name+kind match plus the exact goal point.
    /// A near-coincidence of two points (the old "&lt;= 1 m from the previous NavTarget" ghost) is
    /// NOT identity — a fresh nearby target is a current order and must drive. The route is
    /// derived deterministically from the exact goal, so matching the goal matches the route.
    /// </summary>
    internal static class DestinationIdentityPolicy
    {
        internal const float PointEpsilonMeters = 0.05f;

        internal static bool SameLogicalDestination(string? nameA, string? kindA, Vector3 goalA,
            string? nameB, string? kindB, Vector3 goalB) =>
            string.Equals(nameA ?? string.Empty, nameB ?? string.Empty, StringComparison.Ordinal) &&
            string.Equals(kindA ?? string.Empty, kindB ?? string.Empty, StringComparison.Ordinal) &&
            SamePoint(goalA, goalB);

        internal static bool SamePoint(Vector3 a, Vector3 b) =>
            float.IsFinite(a.X) && float.IsFinite(a.Y) && float.IsFinite(a.Z) &&
            float.IsFinite(b.X) && float.IsFinite(b.Y) && float.IsFinite(b.Z) &&
            Math.Abs(a.X - b.X) <= PointEpsilonMeters &&
            Math.Abs(a.Y - b.Y) <= PointEpsilonMeters &&
            Math.Abs(a.Z - b.Z) <= PointEpsilonMeters;
    }
    // ==== end TaxiDriver section: DestinationIdentityPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: ReverseSupervisionPolicy.cs ====

    /// <summary>
    /// Runtime supervision decisions for the bounded reverse manoeuvre (brief §7.3/§7.4/§7.5).
    /// Unity-free: it consumes measured rear-clearance samples, the ownership/authority facts and
    /// the pause/terminal state and decides whether new native input is permitted and how the
    /// supervisor must react. Every unknown fails closed.
    /// </summary>
    internal static class ReverseSupervisionPolicy
    {
        /// <summary>Rear-clearance re-check cadence during an active reverse (brief §7.5).</summary>
        internal const float RearCheckSeconds = 0.15f;

        /// <summary>True when the rear-clearance sample must be re-taken at this time.</summary>
        internal static bool ShouldCheckRear(float lastCheckAt, float now) =>
            !float.IsFinite(lastCheckAt) || !float.IsFinite(now) || now - lastCheckAt >= RearCheckSeconds ||
            now < lastCheckAt;

        /// <summary>
        /// New native stop/park input is permitted only for the exact own live taxi, with current
        /// host authority, and no ACTUAL player driver (brief §7.4). A local PASSENGER must never
        /// suppress the native cleanup of an NPC-driven reverse; only a real player at the wheel
        /// does. An unreadable driver reference fails closed (treated as a player driver).
        /// </summary>
        internal static bool NativeCleanupPermitted(bool host, bool exactOwnedAlive, bool playerDriver) =>
            host && exactOwnedAlive && !playerDriver;
    }
    // ==== end TaxiDriver section: ReverseSupervisionPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: NavigationOperationPolicy.cs ====

    /// <summary>Explicit lifecycle of one navigation operation, separate from the native PollingActive flag.</summary>
    internal enum OperationPhase
    {
        /// <summary>No operation owns navigation.</summary>
        Inactive,

        /// <summary>The operation drives; a redispatch/reverse keeps this phase.</summary>
        Active,

        /// <summary>Temporarily in a bounded recovery; the operation, meter and UI stay alive.</summary>
        Recovery,

        /// <summary>Explicitly finished (arrived or failed) — never inferred from a native flag.</summary>
        Terminal,
    }

    /// <summary>
    /// Explicit operation/terminal model (brief §6.1/§8.1), separate from the native
    /// <c>PollingActive</c> flag: an operation can be Active, temporarily in Recovery, or
    /// explicitly Terminal. A single native <c>PollingActive</c> flip must never publish an
    /// intermediate terminal state; only a genuine new order opens a new generation.
    /// </summary>
    internal sealed class NavigationOperationPolicy
    {
        private OperationPhase _phase = OperationPhase.Inactive;
        private string _terminalReason = string.Empty;
        private bool _failed;
        private long _generation;

        internal OperationPhase Phase => _phase;
        internal bool Active => _phase == OperationPhase.Active || _phase == OperationPhase.Recovery;
        internal bool InRecovery => _phase == OperationPhase.Recovery;
        internal bool Terminal => _phase == OperationPhase.Terminal;
        internal bool Failed => _failed;
        internal string TerminalReason => _terminalReason;
        internal long Generation => _generation;

        internal void Begin()
        {
            _generation++;
            _phase = OperationPhase.Active;
            _terminalReason = string.Empty;
            _failed = false;
        }

        /// <summary>Enters bounded recovery without publishing a terminal state (brief §8.1).</summary>
        internal void EnterRecovery()
        {
            if (_phase == OperationPhase.Active) _phase = OperationPhase.Recovery;
        }

        /// <summary>Returns to Active after a bounded recovery continued the same operation.</summary>
        internal void Recover()
        {
            if (_phase == OperationPhase.Recovery) _phase = OperationPhase.Active;
        }

        /// <summary>Explicit terminal verdict; idempotent, never reopened by a native flag.</summary>
        internal void Terminate(string reason, bool failed)
        {
            if (_phase == OperationPhase.Terminal) return;
            _phase = OperationPhase.Terminal;
            _terminalReason = reason ?? string.Empty;
            _failed = failed;
        }

        internal void Reset()
        {
            _phase = OperationPhase.Inactive;
            _terminalReason = string.Empty;
            _failed = false;
        }
    }
    // ==== end TaxiDriver section: NavigationOperationPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: ReverseStopTransitionPolicy.cs ====
    using System;

    /// <summary>
    /// Outcome of the controlled native end of a reverse manoeuvre (brief §7.4/§7.5). A returned
    /// native <c>StopReversing</c> is NOT a proven physical halt — it only authorizes the next
    /// checked transition. A stop that was not permitted, threw, or was superseded mid-frame
    /// authorizes NO follow-up (no retry, no rescue, no forward dispatch).
    /// </summary>
    internal sealed class ReverseStopTransitionResult
    {
        /// <summary>The exact-own-alive / host / no-player stop precondition held at the check.</summary>
        internal bool StopPermitted { get; set; }

        /// <summary>The native stop call was actually issued.</summary>
        internal bool StopIssued { get; set; }

        /// <summary>The native stop call threw.</summary>
        internal bool StopThrew { get; set; }

        /// <summary>The captured operation / authority was no longer current after the stop.</summary>
        internal bool Superseded { get; set; }

        internal bool FallbackNavigatingRan { get; set; }
        internal bool FallbackNavigatingThrew { get; set; }
        internal bool FallbackParkRan { get; set; }
        internal bool FallbackParkThrew { get; set; }

        /// <summary>
        /// A checked follow-up (retry / rescue / forward dispatch) is authorized ONLY when the
        /// native stop was permitted, actually issued, did not throw, and the captured operation
        /// stayed current. This is authorization for the next checked transition, never proof of a
        /// physical halt (brief §7.4).
        /// </summary>
        internal bool FollowupAllowed =>
            StopPermitted && StopIssued && !StopThrew && !Superseded;
    }

    /// <summary>
    /// Runtime-used, Unity-free helper for the controlled end of a reverse manoeuvre (brief §7.4):
    /// the caller revokes the managed intent FIRST, then this runs the native stop and the
    /// independent fallback cleanup as isolated, re-validated steps. Every step is guarded against
    /// its own exception so one failure never skips the rest, and every new native input step
    /// re-checks that the captured operation is still current and permitted. A returned
    /// <c>StopReversing</c> is authorization for the next checked transition, never proof of a
    /// physical halt.
    /// </summary>
    internal static class ReverseStopTransitionPolicy
    {
        internal static ReverseStopTransitionResult Run(
            Func<bool> stopPermitted,
            Action stopNative,
            Func<bool> stillCurrentAndPermitted,
            Action fallbackStopNavigating,
            Action fallbackPark,
            Action<Exception> onStepError)
        {
            var result = new ReverseStopTransitionResult();

            // Step 1: the native stop — isolated, and only under the exact-own-alive precondition.
            try
            {
                if (stopPermitted())
                {
                    result.StopPermitted = true;
                    stopNative();
                    result.StopIssued = true;
                }
            }
            catch (Exception ex)
            {
                result.StopThrew = true;
                Report(onStepError, ex);
            }

            // Re-validate AFTER the stop: a STOP/reroute or an authority/player change mid-frame
            // means the captured operation is no longer current and no follow-up may run.
            bool current;
            try
            {
                current = stillCurrentAndPermitted();
            }
            catch (Exception ex)
            {
                current = false;
                Report(onStepError, ex);
            }
            if (!current) result.Superseded = true;

            // Steps 2 + 3: independent fallback cleanup — each step isolated AND re-validated.
            RunFallback(current, stillCurrentAndPermitted, fallbackStopNavigating, onStepError,
                () => result.FallbackNavigatingRan = true, () => result.FallbackNavigatingThrew = true);
            RunFallback(current, stillCurrentAndPermitted, fallbackPark, onStepError,
                () => result.FallbackParkRan = true, () => result.FallbackParkThrew = true);

            return result;
        }

        private static void RunFallback(bool currentAfterStop, Func<bool> stillCurrentAndPermitted,
            Action step, Action<Exception> onStepError, Action markRan, Action markThrew)
        {
            try
            {
                bool permitted = currentAfterStop;
                if (permitted)
                {
                    // Re-validate on the step itself: authority/player/operation may have changed.
                    permitted = stillCurrentAndPermitted();
                }
                if (!permitted) return;
                step();
                markRan();
            }
            catch (Exception ex)
            {
                markThrew();
                Report(onStepError, ex);
            }
        }

        private static void Report(Action<Exception> onStepError, Exception ex)
        {
            try { onStepError?.Invoke(ex); } catch { /* reporting must never mask the outcome */ }
        }
    }
    // ==== end TaxiDriver section: ReverseStopTransitionPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: ArrivalConfirmationPolicy.cs ====
    using System;

    /// <summary>What the pending arrival confirmation gate decides for this frame (brief §8.2).</summary>
    internal enum ArrivalGateOutcome
    {
        /// <summary>No pending arrival evidence for the current operation.</summary>
        NotPending,

        /// <summary>Evidence is current, but the halt/proximity is not yet proven — keep supervising.</summary>
        Pending,

        /// <summary>A readable, confirmed halt inside the target radius — accept the arrival.</summary>
        Accept,

        /// <summary>The evidence is superseded or its bounded settle window elapsed — drop it.</summary>
        Expire,
    }

    /// <summary>
    /// The bounded arrival confirmation gate (brief §8.2). A current navigation <c>Complete</c> is
    /// EVIDENCE for this operation's arrival, not the arrival itself: the operation may only
    /// complete once supervision proves a readable, confirmed halt inside the exact target radius
    /// with no native or managed reverse. The evidence is scoped to one order AND operation
    /// generation, so a stale Complete can never complete a newer target, and it is bounded so it
    /// can never hang the operation.
    /// </summary>
    internal static class ArrivalConfirmationPolicy
    {
        /// <summary>How long a current Complete may await a confirmed halt before it is dropped.</summary>
        internal const float SettleWindowSeconds = 3f;

        /// <summary>True only for the exact current order AND operation generation (review D12).</summary>
        internal static bool IsCurrent(bool pending, int pendingOrder, long pendingOperation,
            int order, long operation) =>
            pending && pendingOrder == order && pendingOperation == operation;

        internal static ArrivalGateOutcome Evaluate(bool pending, bool currentOrder, bool currentOperation,
            bool operationActive, float pendingSeconds, float distance, bool speedReadable,
            float signedSpeed, bool reversing)
        {
            if (!pending) return ArrivalGateOutcome.NotPending;
            // A superseded order/operation, or a finished operation, is never current evidence.
            if (!currentOrder || !currentOperation || !operationActive) return ArrivalGateOutcome.Expire;
            // Accept only a readable, confirmed halt inside the radius with no reverse.
            if (SupervisedArrivalPolicy.IsConfirmedArrival(
                    currentRunComplete: true, operationProgress: false,
                    distance, speedReadable, signedSpeed, reversing))
                return ArrivalGateOutcome.Accept;
            // Bounded: the evidence may not hang forever without a confirmed halt.
            if (!float.IsFinite(pendingSeconds) || pendingSeconds >= SettleWindowSeconds)
                return ArrivalGateOutcome.Expire;
            return ArrivalGateOutcome.Pending;
        }
    }
    // ==== end TaxiDriver section: ArrivalConfirmationPolicy.cs ====
}
