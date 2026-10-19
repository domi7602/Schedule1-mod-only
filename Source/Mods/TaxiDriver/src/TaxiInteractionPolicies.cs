// ---------------------------------------------------------------------------
// TaxiInteractionPolicies.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract tests can still read that
// file's exact content.
//
//   App/HeroSnapshot.cs            -> section HeroSnapshot.cs
//   App/HeroView.cs                -> section HeroView.cs
//   App/ServiceFeedback.cs         -> section ServiceFeedback.cs
//   App/StopConfirmation.cs        -> section StopConfirmation.cs
//   Dispatch/DestinationFilter.cs  -> section DestinationFilter.cs
//   Dispatch/DestinationSelectionPolicy.cs -> section DestinationSelectionPolicy.cs
//   Console/TaxiConsolePolicy.cs   -> section TaxiConsolePolicy.cs
//   Occupancy/DriverBoardingGate.cs -> section DriverBoardingGate.cs
//   Occupancy/PassengerSeatPolicy.cs -> section PassengerSeatPolicy.cs
//   Occupancy/RideProtectionPolicy.cs -> section RideProtectionPolicy.cs
//   Visuals/TaxiVisualPolicy.cs    -> section TaxiVisualPolicy.cs
//
// ---------------------------------------------------------------------------

namespace TaxiDriver
{
    // ==== TaxiDriver section: HeroSnapshot.cs ====

    /// <summary>
    /// Typed hero change snapshot (finding 7). Replaces the bit-packed int
    /// signature, whose 5 fare bits wrapped every $32
    /// (<c>32 &lt;&lt; 27 == 0</c> mod 2^32) and froze the fare display.
    /// Exact field comparison — no hashes. Pure (no Unity), unit-tested;
    /// the builder lives in <c>TaxiApp.RefreshHero</c> (game state).
    /// </summary>
    internal readonly record struct HeroSnapshot(
        bool RideActive,
        bool AwaitingDestination,
        bool Arrived,
        bool GaveUp,
        bool Picked,
        bool AwaitingBoard,
        bool ToPlayer,
        bool AutoRunning,
        bool PendingSpawn,
        bool Vehicle,
        bool ToPlayerPolling,
        string Destination,
        int Fare,
        string Override,
        string DropOff = "",
        string ActiveTripName = "",
        bool HasActiveTrip = false);
    // ==== end TaxiDriver section: HeroSnapshot.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: HeroView.cs ====
    using System;

    internal readonly record struct HeroText(string Title, string Meta, string Value, bool IsFare);

    /// <summary>All displayed hero state comes from the same change-guarded snapshot.</summary>
    internal static class HeroView
    {
        internal static HeroText Build(HeroSnapshot s)
        {
            string title;
            string meta;
            string value = string.Empty;
            bool isFare = false;

            if (s.RideActive)
            {
                if (s.AwaitingDestination)
                {
                    title = "ON BOARD";
                    meta = "Pick a destination below";
                }
                else if (s.Arrived)
                {
                    title = s.GaveUp ? "COULDN'T GET THERE" : "ARRIVED";
                    meta = s.GaveUp
                        ? "Press E to get out — or STOP to despawn"
                        : "Press E to exit the taxi";
                    value = $"FARE ${s.Fare}";
                    isFare = true;
                }
                else
                {
                    // Package 7: while driving, the hero names the ACTIVE trip —
                    // the picker may have been cleared or re-picked since dispatch.
                    string trip = s.HasActiveTrip && s.ActiveTripName.Length > 0
                        ? s.ActiveTripName
                        : s.Destination;
                    bool showing = s.Picked || trip != s.Destination;
                    title = showing ? $"RIDING TO {trip.ToUpperInvariant()}" : "RIDING";
                    string dropOff = ShortDropOff(s.DropOff);
                    meta = showing
                        ? (string.IsNullOrEmpty(dropOff) ? "On the way" : $"Drop-off: {dropOff}")
                        : "On the way";
                    value = $"FARE ${s.Fare}";
                    isFare = true;
                }
            }
            else if (s.AwaitingBoard)
            {
                title = "BOARD NOW";
                meta = "Press E to get in";
                value = s.Picked ? $"TO {s.Destination}" : "Or pick a destination below";
            }
            else if (s.ToPlayer || s.ToPlayerPolling)
            {
                title = "TAXI ON THE WAY";
                meta = "Driving to your position — wait outside";
                if (s.Picked)
                    value = $"NEXT: {s.Destination}";
            }
            else if (s.AutoRunning || s.PendingSpawn)
            {
                title = "SPAWNING…";
                meta = "The taxi is being dispatched";
            }
            else if (s.Vehicle)
            {
                title = "TAXI WAITING";
                meta = "Press STOP to despawn it";
                value = s.Picked ? $"TO {s.Destination}" : "No destination picked";
            }
            else
            {
                title = "CALL A TAXI";
                meta = "Orders a cab to your position";
                value = s.Picked ? $"TO {s.Destination}" : "No destination picked";
            }

            if (s.Override.Length > 0)
                meta = s.Override;

            return new(title, meta, value, isFare);
        }

        private static string ShortDropOff(string? dropOff)
        {
            if (string.IsNullOrEmpty(dropOff))
                return string.Empty;
            int paren = dropOff.IndexOf('(');
            return paren > 0 ? dropOff.Substring(0, paren).Trim() : dropOff;
        }

    }
    // ==== end TaxiDriver section: HeroView.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: ServiceFeedback.cs ====
    using System;


    /// <summary>
    /// Pure display arbitration between a short-lived local override (a button outcome for a
    /// few seconds) and the persistent <see cref="SpikeState.ServiceStatus"/> feedback.
    ///
    /// A newer, non-empty service status supersedes an active override immediately, so an
    /// asynchronous failure can never be masked by a stale success message — while an
    /// override that is armed afterwards (deliberate UI feedback, or the protected STOP
    /// prompt) still shows normally. Empty statuses and unchanged revisions never discard an
    /// override. All time inputs are validated fail-closed, so an invalid clock shows the
    /// persistent status instead of an override.
    /// </summary>
    internal sealed class ServiceFeedback
    {
        private string _overrideText = string.Empty;
        private float _overrideUntil;
        private long _appliedRevision;

        /// <summary>Arms a local override. An invalid or negative <paramref name="now"/> arms nothing.</summary>
        internal void ArmOverride(string text, float now, float seconds)
        {
            _overrideText = text ?? string.Empty;
            _overrideUntil = _overrideText.Length > 0 && IsValidTime(now)
                ? now + Math.Max(0f, seconds)
                : 0f;
        }

        internal void ClearOverride()
        {
            _overrideText = string.Empty;
            _overrideUntil = 0f;
        }

        /// <summary>True while a local override is armed (regardless of its expiry).</summary>
        internal bool HasOverride => _overrideText.Length > 0;

        /// <summary>
        /// The text to display: the active override while it is armed and unexpired, otherwise
        /// the service status. A new non-empty status (new revision) supersedes an unprotected
        /// override once, marking that revision as consumed.
        /// </summary>
        internal string Resolve(float now, string serviceStatus, long statusRevision, bool overrideProtected)
        {
            if (statusRevision != _appliedRevision)
            {
                _appliedRevision = statusRevision;
                if (!overrideProtected && serviceStatus.Length > 0)
                    ClearOverride();
            }

            if (_overrideText.Length > 0 && IsValidTime(now) && now < _overrideUntil)
                return _overrideText;

            return serviceStatus;
        }

        private static bool IsValidTime(float value) => !float.IsNaN(value) && !float.IsInfinity(value) && value >= 0f;
    }
    // ==== end TaxiDriver section: ServiceFeedback.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: StopConfirmation.cs ====
    using System;


    /// <summary>Two-press destructive action gate; no Unity objects or wall-clock dependencies.</summary>
    internal sealed class StopConfirmation
    {
        internal const float ConfirmSeconds = 3f;
        private bool _armed;
        private float _until;
        private float _armedAt;
        private long _rideId;

        internal void Reset()
        {
            _armed = false;
            _until = 0f;
            _armedAt = 0f;
            _rideId = 0;
        }

        internal bool Synchronize(bool appOpen, bool rideActive, long rideId)
        {
            if (!_armed || (appOpen && rideActive && rideId == _rideId))
                return false;
            Reset();
            return true;
        }

        internal bool Press(bool rideActive, long rideId, float now)
        {
            if (!float.IsFinite(now) || now < 0f)
            {
                Reset();
                return false;
            }
            if (!rideActive)
            {
                Reset();
                return true;
            }
            if (_armed && rideId == _rideId && now >= _armedAt && now < _until)
            {
                Reset();
                return true;
            }
            _armed = true;
            _rideId = rideId;
            _armedAt = now;
            _until = now + ConfirmSeconds;
            return false;
        }
    }
    // ==== end TaxiDriver section: StopConfirmation.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: DestinationFilter.cs ====
    using System;


    /// <summary>
    /// Pure search matching for the TaxiApp destination list (search feature,
    /// 2026-10-03): a query matches when it occurs (case-insensitive substring) in
    /// the destination name OR its type tag ("DEAL"/"PROP"/"HOME"/"PARK"/"STAND").
    /// An empty or whitespace-only query matches everything; surrounding
    /// whitespace in the query is trimmed first. Unity-free so the rules stay
    /// unit-tested (DestinationFilterTests).
    /// </summary>
    internal static class DestinationFilter
    {
        /// <summary>True when the destination row should be visible for <paramref name="query"/>.</summary>
        internal static bool Matches(string? query, string? name, string? tag)
        {
            string trimmed = (query ?? string.Empty).Trim();
            if (trimmed.Length == 0)
                return true;

            return Contains(name, trimmed) || Contains(tag, trimmed);
        }

        private static bool Contains(string? field, string query) =>
            !string.IsNullOrEmpty(field) &&
            field.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
    }
    // ==== end TaxiDriver section: DestinationFilter.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: DestinationSelectionPolicy.cs ====

    internal readonly record struct DestinationPoint(float X, float Y, float Z);
    internal readonly record struct DestinationSelection(string Name, string Kind, DestinationPoint Goal);
    internal enum DestinationPickMode { NoOp, MetadataOnly, Dispatch }
    internal readonly record struct DestinationPickResult(DestinationPickMode Mode,
        DestinationSelection Selection, DestinationPoint RoutePoint, string DropOff, string Lot, string ActiveName);

    /// <summary>Logical picks and physical route identity are independent. Exact points,
    /// not a proximity radius: a nearby different route must still be dispatched.</summary>
    internal static class DestinationSelectionPolicy
    {
        internal static bool IsSelected(DestinationSelection? current, DestinationSelection candidate) =>
            current.HasValue && current.Value == candidate;

        internal static DestinationPickResult Select(DestinationSelection? current, DestinationPoint? resolved,
            DestinationPoint? activePoint, string activeDropOff, string activeLot,
            DestinationSelection next, DestinationPoint arrival, string dropOff, string lot,
            bool riding, bool driving, bool gaveUp)
        {
            bool keepRoute = riding && driving && !gaveUp && activePoint.HasValue &&
                (arrival == activePoint.Value || (resolved.HasValue && arrival == resolved.Value));
            var mode = keepRoute ? (current == next ? DestinationPickMode.NoOp : DestinationPickMode.MetadataOnly)
                : DestinationPickMode.Dispatch;
            // A reachable/fallback rebound remains the truth while retaining the route.
            bool rebound = keepRoute && activePoint!.Value != arrival;
            return new(mode, next, keepRoute ? activePoint!.Value : arrival,
                rebound ? activeDropOff : dropOff, rebound ? activeLot : lot, riding ? next.Name : string.Empty);
        }
    }
    // ==== end TaxiDriver section: DestinationSelectionPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: TaxiConsolePolicy.cs ====
    using System;
    using System.Collections.Generic;


    /// <summary>Allow-list for console diagnostics; service actions belong to TaxiApp.</summary>
    internal static class TaxiConsolePolicy
    {
        private static readonly HashSet<string> ReadOnlyCommands = new(StringComparer.OrdinalIgnoreCase)
        {
            "help", "h", "?", "codes", "list", "status", "diag", "probe", "trace",
            "lots", "pois", "places", "destinations", "fare", "meter", "ai", "ki",
        };

        internal static bool IsReadOnlyDiagnostic(string? command) =>
            command != null && ReadOnlyCommands.Contains(command.Trim());
    }
    // ==== end TaxiDriver section: TaxiConsolePolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: DriverBoardingGate.cs ====
    using System;


    /// <summary>Native identity gate for a genuine EnterVehicle attempt; Unity-free for tests.</summary>
    internal sealed class DriverBoardingGate
    {
        internal const float CooldownSeconds = 15f;
        private IntPtr _lastAttemptedNpc;
        private float _lastAttemptAt;

        internal static bool IsDedicatedDriver(IntPtr candidate, IntPtr dedicated) =>
            candidate != IntPtr.Zero && dedicated != IntPtr.Zero && candidate == dedicated;

        internal float RemainingSeconds(IntPtr candidate, float now) =>
            candidate == _lastAttemptedNpc && candidate != IntPtr.Zero
                ? Math.Max(0f, CooldownSeconds - (now - _lastAttemptAt))
                : 0f;

        /// <summary>Commit BEFORE the native call: even a throwing/failed attempt starts the cooldown.</summary>
        internal bool TryBeginEntry(IntPtr candidate, IntPtr dedicated, float now)
        {
            if (!float.IsFinite(now) || now < 0f || !IsDedicatedDriver(candidate, dedicated) || RemainingSeconds(candidate, now) > 0f)
                return false;
            _lastAttemptedNpc = candidate;
            _lastAttemptAt = now;
            return true;
        }
    }
    // ==== end TaxiDriver section: DriverBoardingGate.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: PassengerSeatPolicy.cs ====
    using System;

    /// <summary>Player-only seat observation. Native null and native player identity are distinct from an unreadable proxy.</summary>
    internal readonly record struct PassengerSeatSnapshot(
        bool Driver, bool Readable, bool OccupantPresent, IntPtr OccupantPointer, bool IsOccupied);

    internal static class PassengerSeatPolicy
    {
        internal static bool CanReserveDriverSeat(bool occupantPresent, bool isOccupied) =>
            !occupantPresent && !isOccupied;

        internal static bool CanUse(PassengerSeatSnapshot seat, IntPtr localPointer) =>
            localPointer != IntPtr.Zero && seat.Readable && !seat.Driver &&
            (seat.OccupantPresent
                ? seat.OccupantPointer != IntPtr.Zero && seat.OccupantPointer == localPointer
                : !seat.IsOccupied);

        internal static int Select(int count, Func<int, PassengerSeatSnapshot> read, IntPtr localPointer)
        {
            int vacant = -1;
            for (int i = 0; i < count; i++)
            {
                PassengerSeatSnapshot seat = read(i);
                if (!CanUse(seat, localPointer)) continue;
                if (seat.OccupantPresent) return i;
                if (vacant < 0) vacant = i;
            }
            return vacant;
        }
    }
    // ==== end TaxiDriver section: PassengerSeatPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: RideProtectionPolicy.cs ====
    using System;


    [Flags]
    internal enum RideProtectionPatch
    {
        None = 0,
        Throttle = 1 << 0,
        Steering = 1 << 1,
        Handbrake = 1 << 2,
        TrunkOpen = 1 << 3,
        TrunkSetOpen = 1 << 4,
    }

    /// <summary>Pure fail-closed policy for the patches required before a passenger ride.</summary>
    internal static class RideProtectionPolicy
    {
        internal const RideProtectionPatch Required =
            RideProtectionPatch.Throttle |
            RideProtectionPatch.Steering |
            RideProtectionPatch.Handbrake |
            RideProtectionPatch.TrunkOpen |
            RideProtectionPatch.TrunkSetOpen;

        internal static bool IsComplete(RideProtectionPatch installed) =>
            (installed & Required) == Required;

        /// <summary>
        /// Fail closed while a passenger ride is active: if the currently controlled
        /// vehicle cannot be proven to be the owned taxi, do not pass player input
        /// through. When the owned taxi's agent override is active, leave that
        /// separate AI-control path untouched.
        /// </summary>
        internal static bool ShouldBlockPlayerInput(
            bool passengerRideMode,
            bool ownedRideVehicleVerified,
            bool overrideControls) =>
            passengerRideMode && (!ownedRideVehicleVerified || !overrideControls);

        /// <summary>The handbrake is a player input path, never an agent-override path.</summary>
        internal static bool ShouldBlockHandbrake(bool passengerRideMode) => passengerRideMode;

        /// <summary>
        /// During a passenger ride, block the known taxi trunk and fail closed if
        /// either the taxi's trunk or the candidate door cannot be identified.
        /// Other storage doors remain usable only when both identities are valid.
        /// </summary>
        internal static bool ShouldBlockTrunkOpen(
            bool passengerRideMode,
            bool ownedTrunkVerified,
            bool doorIdentityVerified,
            bool isRideTrunk) =>
            passengerRideMode && (!ownedTrunkVerified || !doorIdentityVerified || isRideTrunk);

        internal static RideProtectionPatch Missing(RideProtectionPatch installed) =>
            Required & ~installed;
    }
    // ==== end TaxiDriver section: RideProtectionPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: TaxiVisualPolicy.cs ====

    /// <summary>Pure gates for refusing empty mesh data and preserving vanilla visuals on failure.</summary>
    internal static class TaxiVisualPolicy
    {
        internal static bool HasUsableGeometry(int vertexCount, int subMeshCount, int indexedSubMeshCount) =>
            vertexCount > 0 && subMeshCount > 0 && indexedSubMeshCount > 0;

        internal static bool CanCommitSwap(int usableRendererCount) => usableRendererCount > 0;
    }
    // ==== end TaxiDriver section: TaxiVisualPolicy.cs ====
}
