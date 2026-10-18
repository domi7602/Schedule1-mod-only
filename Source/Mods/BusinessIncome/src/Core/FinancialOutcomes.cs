using System;
using System.Collections.Generic;
using BusinessIncome.Models;

namespace BusinessIncome.Core;

/// <summary>
/// Explicit payout outcome. Never a bare bool: a clean return from the bank seam means
/// "request accepted", NOT "settlement confirmed" (see references/money-api-semantics.md).
/// </summary>
public enum PayoutOutcome
{
    /// <summary>Nothing was attempted (e.g. nothing to pay, feature disabled).</summary>
    Skipped,
    /// <summary>This instance is not authoritative for mutations (MP client).</summary>
    NotAuthoritative,
    /// <summary>Runtime/load readiness or live money manager not ready.</summary>
    NotReady,
    /// <summary>The day was already paid and force was not requested.</summary>
    AlreadyPaid,
    /// <summary>No owned businesses; day is still terminal-marked so catch-up cannot loop.</summary>
    NoBusinesses,
    /// <summary>Net revenue is zero/negative; day terminal-marked, no bank call.</summary>
    ZeroNet,
    /// <summary>Dry-run preview only: no bank call, no storage write, no host requirement.</summary>
    DryRun,
    /// <summary>An unresolved pending marker exists; ALL new booking for the slot is blocked.</summary>
    BlockedPending,
    /// <summary>The persisted state is corrupt/schema/identity invalid; fail closed, no seed.</summary>
    BlockedStateCorrupt,
    /// <summary>The write-ahead marker could not be written durably; the bank call is blocked.</summary>
    BlockedMarkerWrite,
    /// <summary>The request was accepted by the bank seam (NOT confirmed settlement).</summary>
    Requested,
    /// <summary>The request was accepted but the forward state commit failed; marker kept.</summary>
    BookedStateNotSaved,
    /// <summary>An exception escaped after the bank API was invoked: outcome UNKNOWN, never retry.</summary>
    UnknownOutcome,
    /// <summary>A re-entrant booking attempt was rejected by the guard.</summary>
    Reentrant,
    /// <summary>A terminal (zero/no-business) day could not be persisted; catch-up must stop.</summary>
    StateNotPersisted,
    /// <summary>User confirmed the pending marker was actually received; forward state committed.</summary>
    PendingConfirmed,
    /// <summary>User chose to resolve (re-pay): pre-call snapshot restored and marker cleared.</summary>
    PendingResolved,
    /// <summary>Confirm/resolve committed but the write-ahead marker could not be cleared; the slot stays blocked.</summary>
    MarkerClearFailed,
    /// <summary>A caller-supplied input (day/net) was invalid: fail closed — no write, no bank call.</summary>
    InvalidInput
}

/// <summary>Rich result of a settlement attempt.</summary>
public readonly record struct PayoutResult(
    PayoutOutcome Outcome,
    int Day,
    float Net,
    int BusinessCount,
    string Detail)
{
    /// <summary>True once the bank seam has been invoked (outcome no longer controllable).</summary>
    public bool BankInvoked =>
        Outcome is PayoutOutcome.Requested
               or PayoutOutcome.BookedStateNotSaved
               or PayoutOutcome.UnknownOutcome;

    public bool Succeeded => Outcome == PayoutOutcome.Requested;
}

/// <summary>Validation status of a persisted <see cref="PayoutState"/>.</summary>
public enum StateStatus
{
    Missing,
    Ok,
    /// <summary>Main file was unreadable/corrupt; a valid backup was recovered.</summary>
    RecoveredFromBackup,
    Corrupt,
    SchemaMismatch,
    IdentityMismatch
}

/// <summary>Validation status of the write-ahead pending marker.</summary>
public enum PendingStatus
{
    Missing,
    Valid,
    /// <summary>Main marker was unreadable/corrupt; a valid backup marker was recovered.</summary>
    Backup,
    Corrupt
}

/// <summary>Load-readiness of the runtime, reset at PreLoad / scene unload, resolved on LoadComplete.</summary>
public enum LoadReadiness
{
    NotResolved,
    Resolved
}

/// <summary>Authority of the active save slot: mutations require a known slot.</summary>
public enum SlotAuthority
{
    Unknown,
    Known
}

/// <summary>Result of loading/validating the payout state.</summary>
public readonly record struct StateLoadResult(StateStatus Status, PayoutState? State, string Detail)
{
    // A recovered-from-backup state is NOT usable for a payout decision: it is an older
    // snapshot of the truth and must not authorise a bank call from stale data.
    public bool Usable => Status is StateStatus.Ok;
    public bool BlocksMutation => Status is StateStatus.Corrupt or StateStatus.SchemaMismatch
        or StateStatus.IdentityMismatch or StateStatus.RecoveredFromBackup;
}

/// <summary>Result of loading/validating the write-ahead pending marker.</summary>
public readonly record struct PendingLoadResult(PendingStatus Status, PendingPayoutState? Marker, string Detail)
{
    /// <summary>A valid, recovered-from-backup or corrupt marker blocks ALL new automatic and manual/forced booking.</summary>
    public bool BlocksBooking => Status is PendingStatus.Valid or PendingStatus.Backup or PendingStatus.Corrupt;
}

/// <summary>Decision of the centralized payout-window planner.</summary>
public readonly record struct WindowDecision(bool InWindow, string Reason);

/// <summary>Pure catch-up plan: candidate days oldest-to-newest plus terminal information.</summary>
public sealed class CatchupPlan
{
    public IReadOnlyList<int> Days { get; init; } = Array.Empty<int>();
    public bool StoppedEarly { get; init; }
    public string Reason { get; init; } = "";
    public bool TerminalSkip { get; init; }
}

/// <summary>Pure legacy-migration decision. Never deletes legacy; only adopts matching identity.</summary>
public enum LegacyMigrationDecision
{
    None,
    TargetExists,
    AdoptMatching,
    BlockAmbiguous
}

/// <summary>Outcome of the pre-schema (v0) payout-state upgrade.</summary>
public enum SchemaMigrationStatus
{
    /// <summary>No state file at the path - nothing to upgrade.</summary>
    MissingFile,
    /// <summary>The file already carries a schema (whatever its value) - left untouched, so a
    /// schema this build does not understand is never rewritten either.</summary>
    NotNeeded,
    /// <summary>Pre-schema state rewritten in place; the original bytes were saved first.</summary>
    Migrated,
    /// <summary>Unreadable or structurally invalid file - never rewritten.</summary>
    NotLegacy,
    /// <summary>The legacy file belongs to another slot - never rewritten.</summary>
    IdentityMismatch,
    /// <summary>Backup or migrated write failed - the original file was left in place.</summary>
    WriteFailed
}

/// <summary>Result of <see cref="PayoutCodec.MigrateLegacySchema"/>.</summary>
public readonly record struct SchemaMigrationResult(SchemaMigrationStatus Status, string Detail);
