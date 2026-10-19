// ---------------------------------------------------------------------------
// TaxiLifecycle.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract tests can still read that
// file's exact content.
//
//   Runtime/TaxiLifecycle.cs       -> section TaxiLifecycle.cs
//   Lifecycle/CallbackLease.cs     -> section CallbackLease.cs
//   Lifecycle/OwnedSubscriptions.cs -> section OwnedSubscriptions.cs
//   Lifecycle/CleanupGuard.cs      -> section CleanupGuard.cs
//   Lifecycle/DeadTaxiCleanup.cs   -> section DeadTaxiCleanup.cs
//   Lifecycle/VehicleOwnershipLedger.cs -> section VehicleOwnershipLedger.cs
//   Occupancy/PassengerCleanupPolicy.cs -> section PassengerCleanupPolicy.cs
//
// ---------------------------------------------------------------------------

namespace TaxiDriver
{
    // ==== TaxiDriver section: TaxiLifecycle.cs ====
    using System;


    /// <summary>Unity-free lifetime gate shared by updates, service and deferred callbacks.</summary>
    internal sealed class TaxiLifecycle
    {
        private bool _stopping;
        private bool _stopped;
        private long _generation;
        internal bool AcceptingWork { get; private set; }

        internal bool Initialize()
        {
            if (_stopping) return false;
            if (AcceptingWork) return true;
            _generation++;
            _stopped = false;
            AcceptingWork = true;
            return true;
        }

        internal Action<T> Guard<T>(Action<T> callback)
        {
            long generation = _generation;
            return value =>
            {
                if (AcceptingWork && generation == _generation) callback(value);
            };
        }

        internal void Shutdown((string Label, Action Run)[] cleanup,
            (string Label, Action Run)[] finalization, Action<string, Exception> report)
        {
            if (_stopping || _stopped) return;
            _stopping = true; // claim once before invoking any externally supplied callback
            AcceptingWork = false;
            try
            {
                foreach (var step in cleanup) Run(step, report);
            }
            finally
            {
                try
                {
                    foreach (var step in finalization) Run(step, report);
                }
                finally { _stopping = false; _stopped = true; }
            }
        }

        private static void Run((string Label, Action Run) step, Action<string, Exception> report)
        {
            try { step.Run(); }
            catch (Exception ex)
            {
                try { report(step.Label, ex); }
                catch { /* Logging cannot interrupt cleanup or hook finalization. */ }
            }
        }
    }
    // ==== end TaxiDriver section: TaxiLifecycle.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: CallbackLease.cs ====
    using System;

    /// <summary>Breaks the native callback -> app graph even when native unsubscribe
    /// fails. The forwarding delegate remains inert and no longer owns the app.</summary>
    internal sealed class CallbackLease<T>
    {
        private Action<T>? _callback;
        internal CallbackLease(Action<T> callback) => _callback = callback;
        internal void Invoke(T value) => _callback?.Invoke(value);
        internal void Invoke() => Invoke(default!);
        internal void Release() => _callback = null;
    }
    // ==== end TaxiDriver section: CallbackLease.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: OwnedSubscriptions.cs ====
    using System;
    using System.Collections.Generic;

    /// <summary>Owns exact delegate/event pairs, including managed wrappers whose native
    /// owner has died. Clear before destroying UI; never consult Unity overloaded null.</summary>
    internal sealed class OwnedSubscriptions
    {
        private readonly List<Action> _remove = new();
        internal int Count => _remove.Count;

        internal void Add<TCallback, TEvent>(TCallback callback, TEvent evt,
            Action<TCallback, TEvent> subscribe, Action<TCallback, TEvent> unsubscribe,
            Action<TCallback> forget)
        {
            Action remove = () =>
            {
                try { unsubscribe(callback, evt); }
                catch (Exception) { /* Native owner may already be dead. Still release the helper root. */ }
                finally
                {
                    try { forget(callback); }
                    catch (Exception) { /* One broken helper must not retain this registry or block others. */ }
                }
            };
            try { subscribe(callback, evt); }
            catch
            {
                remove();
                throw;
            }
            _remove.Add(remove);
        }

        internal void Clear()
        {
            var entries = _remove.ToArray();
            _remove.Clear();
            foreach (var remove in entries) remove();
        }
    }
    // ==== end TaxiDriver section: OwnedSubscriptions.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: CleanupGuard.cs ====
    using System;


    /// <summary>
    /// Cleanup-safety helper (Cleanup package, 2026-10-03): runs ordered cleanup
    /// steps so a throwing step can never abort the steps after it (review rule:
    /// "Fehler protokollieren, aber UnlockTrunk und StopDriving weiterhin
    /// versuchen"). The step order stays the caller's; every failure is reported
    /// through the injected handler and swallowed. Unity-free so the contract is
    /// unit-tested (CleanupGuardTests).
    /// </summary>
    internal static class CleanupGuard
    {
        /// <summary>
        /// Runs <paramref name="step"/>; an exception is reported via
        /// <paramref name="onFailure"/> (label + exception) and swallowed so the
        /// remaining steps still run. Returns true when the step completed.
        /// </summary>
        internal static bool Run(string label, Action step, Action<string, Exception> onFailure)
        {
            try
            {
                step();
                return true;
            }
            catch (Exception ex)
            {
                onFailure(label, ex);
                return false;
            }
        }
    }
    // ==== end TaxiDriver section: CleanupGuard.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: DeadTaxiCleanup.cs ====
    using System;


    /// <summary>Confirmed-death cleanup ordering; no native vehicle access or payment retry.</summary>
    internal static class DeadTaxiCleanup
    {
        internal static bool Run(Action invalidate, Action stopMeter, Action unlockTrunk,
            Action stopPatrol, Action clearRuntime, Action<string, Exception> report)
        {
            // Reporting is also best-effort: a logging failure must not skip teardown.
            void Report(string label, Exception ex)
            {
                try { report(label, ex); }
                catch { /* preserve cleanup continuation */ }
            }

            bool complete = CleanupGuard.Run("invalidate navigation", invalidate, Report);
            complete &= CleanupGuard.Run("FareMeter.Stop", stopMeter, Report);
            complete &= CleanupGuard.Run("RideLocks.UnlockTrunk", unlockTrunk, Report);
            complete &= CleanupGuard.Run("TaxiAI.StopPatrol", stopPatrol, Report);
            complete &= CleanupGuard.Run("clear dead-taxi runtime", clearRuntime, Report);
            return complete;
        }
    }
    // ==== end TaxiDriver section: DeadTaxiCleanup.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: VehicleOwnershipLedger.cs ====
    using System;


    /// <summary>Ownership classification for the one vehicle TaxiDriver spawned.</summary>
    internal enum VehicleOwnershipKind
    {
        None,
        Owned,
        Foreign,
        Invalid,
        DestroyPending,
        DestroyUnknown,
    }

    /// <summary>
    /// Pure ownership/destruction state for the vehicle returned by TaxiDriver's own
    /// spawn call. A native pointer is never treated as owned unless it was registered
    /// at spawn time. Ambiguous destruction keeps the token and permits at most one
    /// explicitly requested retry.
    /// </summary>
    internal sealed class VehicleOwnershipLedger
    {
        internal const int MaxDestroyAttempts = 2;

        private bool _destroyPending;
        private bool _destroyUnknown;

        internal IntPtr TrackedPointer { get; private set; }

        internal int DestroyAttempts { get; private set; }

        internal bool DestroyPending => _destroyPending;

        internal bool DestroyUnknown => _destroyUnknown;

        internal bool TryRegister(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero || TrackedPointer != IntPtr.Zero)
                return false;

            TrackedPointer = pointer;
            _destroyPending = false;
            _destroyUnknown = false;
            DestroyAttempts = 0;
            return true;
        }

        internal VehicleOwnershipKind Classify(IntPtr candidatePointer, bool isAlive)
        {
            if (candidatePointer == IntPtr.Zero || !isAlive)
                return TrackedPointer == IntPtr.Zero && candidatePointer == IntPtr.Zero
                    ? VehicleOwnershipKind.None
                    : VehicleOwnershipKind.Invalid;

            if (TrackedPointer == IntPtr.Zero || candidatePointer != TrackedPointer)
                return VehicleOwnershipKind.Foreign;

            if (_destroyPending)
                return VehicleOwnershipKind.DestroyPending;
            if (_destroyUnknown)
                return VehicleOwnershipKind.DestroyUnknown;
            return VehicleOwnershipKind.Owned;
        }

        internal bool CanMutate(IntPtr candidatePointer, bool isAlive) =>
            Classify(candidatePointer, isAlive) == VehicleOwnershipKind.Owned;

        internal bool TryBeginDestroy(IntPtr candidatePointer, bool isAlive, bool explicitRetry)
        {
            VehicleOwnershipKind kind = Classify(candidatePointer, isAlive);
            bool firstAttempt = kind == VehicleOwnershipKind.Owned && DestroyAttempts == 0;
            bool boundedManualRetry = kind == VehicleOwnershipKind.DestroyUnknown
                && explicitRetry
                && DestroyAttempts < MaxDestroyAttempts;

            if (!firstAttempt && !boundedManualRetry)
                return false;

            DestroyAttempts++;
            _destroyPending = true;
            _destroyUnknown = false;
            return true;
        }

        /// <summary>
        /// Resolves one pending destroy call. A live object means the result is
        /// ambiguous, not success; its ownership token is deliberately retained.
        /// </summary>
        internal bool ResolveDestroy(IntPtr candidatePointer, bool isAlive)
        {
            if (!_destroyPending || candidatePointer == IntPtr.Zero || candidatePointer != TrackedPointer)
                return false;

            if (isAlive)
            {
                _destroyPending = false;
                _destroyUnknown = true;
                return false;
            }

            Clear();
            return true;
        }

        /// <summary>Turns a timed-out pending request into an ambiguous result without retrying it.</summary>
        internal bool MarkDestroyUnknown(IntPtr candidatePointer)
        {
            if (!_destroyPending || candidatePointer == IntPtr.Zero || candidatePointer != TrackedPointer)
                return false;

            _destroyPending = false;
            _destroyUnknown = true;
            return true;
        }

        /// <summary>Clear the token only after a caller has independently confirmed native death.</summary>
        internal bool ClearAfterConfirmedDeath(IntPtr candidatePointer, bool isAlive)
        {
            if (isAlive || candidatePointer == IntPtr.Zero || candidatePointer != TrackedPointer)
                return false;

            Clear();
            return true;
        }

        /// <summary>Used only by the owning lifecycle after it has confirmed destruction or scene death.</summary>
        internal void ResetAfterConfirmedDeath()
        {
            Clear();
        }

        private void Clear()
        {
            TrackedPointer = IntPtr.Zero;
            _destroyPending = false;
            _destroyUnknown = false;
            DestroyAttempts = 0;
        }
    }
    // ==== end TaxiDriver section: VehicleOwnershipLedger.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: PassengerCleanupPolicy.cs ====
    using System;

    internal enum PlayerOccupancy { Unknown, Occupied, Empty }

    /// <summary>Only a fresh, complete empty-player observation may consume the existing ownership budget.</summary>
    internal static class PassengerCleanupPolicy
    {
        // Called only after seats/registered players and independent NPC teardown are
        // verified. The native aggregate bit may lag NPC exit; do not label it remote.
        internal static PlayerOccupancy FinalizeEmptyPlayerProof(bool localInside, bool aggregateOccupied) =>
            localInside || aggregateOccupied ? PlayerOccupancy.Unknown : PlayerOccupancy.Empty;

        internal static PlayerOccupancy ObserveCurrentVehicle(bool readable, bool present,
            IntPtr currentPointer, IntPtr targetNetworkPointer)
        {
            if (!readable || targetNetworkPointer == IntPtr.Zero || (present && currentPointer == IntPtr.Zero))
                return PlayerOccupancy.Unknown;
            return present && currentPointer == targetNetworkPointer ? PlayerOccupancy.Occupied : PlayerOccupancy.Empty;
        }

        internal static bool TryBeginDestroy(VehicleOwnershipLedger ownership, IntPtr pointer,
            bool alive, bool explicitRetry, PlayerOccupancy occupancy) =>
            occupancy == PlayerOccupancy.Empty && ownership.TryBeginDestroy(pointer, alive, explicitRetry);
    }
    // ==== end TaxiDriver section: PassengerCleanupPolicy.cs ====
}
