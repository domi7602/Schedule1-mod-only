using System;
using System.Collections.Generic;
using System.Linq;
using BusinessIncome.Core;
using BusinessIncome.Models;
using S1Mods.Shared;

namespace BusinessIncome.Services;

/// <summary>
/// Runtime facade over the pure A-D financial-safety core
/// (<see cref="PayoutSettlementEngine"/>, <see cref="PayoutCodec"/>, <see cref="PayoutLedger"/>,
/// <see cref="PayoutScheduler"/>, <see cref="PayoutWindowPlanner"/>, <see cref="LegacyMigration"/>).
///
/// Hard rules enforced here:
/// <list type="bullet">
/// <item>every mutation needs a KNOWN active slot and resolved load readiness;</item>
/// <item>the active slot is NEVER a default or a cached last-known value - it is cleared on every
/// PreLoad / scene-unload transition and only re-resolved on LoadComplete;</item>
/// <item>corrupt / schema / identity-invalid state fails closed - no silent fresh seed;</item>
/// <item>legacy state is never deleted and is only auto-adopted on an exact identity match.</item>
/// </list>
/// </summary>
public static class PayoutStateStore
{
    private static readonly PayoutRuntimeStorage Storage = new();
    private static readonly PayoutSettlementEngine Engine =
        new(Storage, new MoneyPayoutBank(), m => Mod.Log?.Debug(m));
    private static readonly Dictionary<string, bool> LegacyMigrationBlocked = new(StringComparer.OrdinalIgnoreCase);

    public static SlotAuthority Authority => Engine.Authority;
    public static LoadReadiness Readiness => Engine.Readiness;
    public static bool IsAuthoritative => Engine.Authority == SlotAuthority.Known;
    public static bool IsReady =>
        IsAuthoritative && Engine.Readiness == LoadReadiness.Resolved && !Engine.StateLoad.BlocksMutation;
    public static PayoutState? StateOrNull => Engine.State;
    public static StateLoadResult StateLoad => Engine.StateLoad;
    public static PendingLoadResult PendingLoad => Engine.PendingLoad;

    // --- lifecycle ------------------------------------------------------------------

    /// <summary>
    /// Readiness AND authority reset at PreLoad / scene unload. The slot is deliberately
    /// cleared (the keepSlot parameter exists only for call-site compatibility and is
    /// ignored): carrying a slot across a transition could write into the previous save.
    /// </summary>
    public static void Reset(bool keepSlot = false) => Engine.ResetForLoad();

    public static void ResetForSceneUnload() => Engine.ResetForLoad();

    /// <summary>Resolves the active slot from the S1 save info, or returns null (fail closed).</summary>
    public static string? TryResolveActiveSlot()
    {
        try
        {
            var info = SaveSlots.TryGetActiveSaveInfo();
            if (info is { SlotNumber: >= 0 } slot)
            {
                Engine.SetActiveSlot($"slot_{slot.SlotNumber}");
                return Engine.ActiveSlot;
            }
            if (SaveSlots.TryExtractSlotTokenFromSavePath(info?.SavePath) is { } token)
            {
                Engine.SetActiveSlot($"slot_{token}");
                return Engine.ActiveSlot;
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"[Payout] active-slot resolution failed: {ex.Message}");
        }
        return null;
    }

    /// <summary>
    /// Back-compat accessor. Returns the resolved slot, or "(unresolved)" - it NEVER invents
    /// "default" and NEVER caches a last-known slot.
    /// </summary>
    public static string GetActiveSlotSuffix() => Engine.ActiveSlot ?? "(unresolved)";

    public static string GetStateFilePath()
    {
        string? slot = Engine.ActiveSlot;
        return string.IsNullOrEmpty(slot) ? "" : PayoutPaths.State(slot);
    }

    public static string GetPendingFilePath()
    {
        string? slot = Engine.ActiveSlot;
        return string.IsNullOrEmpty(slot) ? "" : PayoutPaths.Pending(slot);
    }

    /// <summary>
    /// Called on LoadComplete: resolves readiness (regardless of SaveInfoLoaded), re-reads the
    /// pending marker here, and loads state. If no slot can be resolved the engine stays
    /// NotAuthoritative and every mutation is blocked for the session.
    /// </summary>
    public static void LoadForActiveSlot()
    {
        string? slot = TryResolveActiveSlot();
        Engine.ResolveOnLoadComplete(slot);
        if (slot == null)
        {
            Mod.Log?.Warn("[Payout] active slot unresolved at LoadComplete - payouts are blocked for this session (fail closed).");
            return;
        }

        bool legacyBlocked = RunLegacyMigrationOnce(slot);
        string statePath = PayoutPaths.State(slot);
        MigratePreSchemaState(statePath, slot);
        Engine.LoadState(statePath);
        Engine.LoadPending(PayoutPaths.Pending(slot));

        if (legacyBlocked)
        {
            // Ambiguous/failed legacy migration fails closed and stays blocked for the whole
            // session; the cached result survives every later LoadComplete.
            Engine.BlockMutations("ambiguous/failed legacy state migration - payouts blocked this session");
            Mod.Log?.Warn($"[Payout] legacy migration blocked for {slot} - automatic AND forced payouts are BLOCKED; no fresh seed.");
            return;
        }

        if (Engine.StateLoad.BlocksMutation)
            Mod.Log?.Warn($"[Payout] state for {slot} is {Engine.StateLoad.Status} ({Engine.StateLoad.Detail}) - automatic AND forced payouts are BLOCKED; no fresh seed.");
    }

    /// <summary>
    /// Runs legacy migration at most once per slot and CACHES the blocked result so a repeat
    /// LoadComplete cannot bypass a failed or ambiguous adoption. Returns true when the slot
    /// must stay fail-closed.
    /// </summary>
    private static bool RunLegacyMigrationOnce(string slot)
    {
        if (LegacyMigrationBlocked.TryGetValue(slot, out bool cached)) return cached;

        bool blocked = RunLegacyMigration(slot);
        LegacyMigrationBlocked[slot] = blocked;
        return blocked;
    }

    private static bool RunLegacyMigration(string slot)
    {
        bool blocked = false;
        string target = PayoutPaths.State(slot);
        foreach (string legacy in PayoutPaths.LegacyStateCandidates)
        {
            try
            {
                if (string.Equals(legacy, target, StringComparison.OrdinalIgnoreCase)) continue;
                if (!Storage.Exists(legacy)) continue;

                // Parse+validate first: only the parsed identity drives the decision, and only
                // VALIDATED contents are ever copied.
                var decoded = PayoutCodec.DecodeState(Storage, legacy, expectedIdentity: "");
                string legacyIdentity = decoded.Usable ? decoded.State?.SaveIdentity ?? "" : "";

                switch (LegacyMigration.Decide(legacyIdentity, slot, Storage.Exists(target) || Storage.BackupExists(target) || Storage.Exists(target + ".tmp"), legacyExists: true))
                {
                    case LegacyMigrationDecision.AdoptMatching:
                        // Re-encode the validated state - NEVER write the raw (possibly
                        // structurally invalid) legacy text into the live target.
                        if (decoded.State != null && Storage.WriteAtomic(target, PayoutCodec.EncodeState(decoded.State)))
                            Mod.Log?.Info($"[Payout] adopted matching legacy state {legacy} -> {target}; legacy file left in place (never deleted).");
                        else
                        {
                            blocked = true;
                            Mod.Log?.Warn($"[Payout] legacy adoption of {legacy} FAILED - payouts stay BLOCKED this session (no fresh seed).");
                        }
                        break;

                    case LegacyMigrationDecision.BlockAmbiguous:
                        blocked = true;
                        Mod.Log?.Warn($"[Payout] AMBIGUOUS legacy state {legacy} (identity '{legacyIdentity}' != slot '{slot}'). NOT adopted and NOT deleted. " +
                                      "Rename or remove it manually if it belongs to another save - payouts are BLOCKED this session.");
                        break;

                    case LegacyMigrationDecision.TargetExists:
                        Mod.Log?.Debug($"[Payout] legacy {legacy} present but {target} already exists - legacy left untouched.");
                        break;
                }
            }
            catch (Exception ex)
            {
                blocked = true;
                Mod.Log?.Warn($"[Payout] legacy migration inspection failed for {legacy}: {ex.Message} - payouts BLOCKED this session (fail closed).");
            }
        }
        return blocked;
    }

    /// <summary>
    /// Upgrades a pre-schema (v0) payout state in place, once per load, right before the state
    /// is read. The codec already ACCEPTS such a file, so a failed upgrade never blocks payouts:
    /// the state stays valid and the upgrade is simply retried on the next load. Structurally
    /// invalid or foreign-identity files are never rewritten (the load result below still fails
    /// closed for them).
    /// </summary>
    private static void MigratePreSchemaState(string statePath, string slot)
    {
        try
        {
            var result = PayoutCodec.MigrateLegacySchema(Storage, statePath, slot);
            switch (result.Status)
            {
                case SchemaMigrationStatus.Migrated:
                    Mod.Log?.Info($"[Payout] upgraded pre-schema payout state {statePath} to schema {PayoutCodec.CurrentSchemaVersion} ({result.Detail}).");
                    break;
                case SchemaMigrationStatus.WriteFailed:
                    Mod.Log?.Warn($"[Payout] pre-schema payout state upgrade of {statePath} FAILED ({result.Detail}) - the state stays valid and is retried next load.");
                    break;
                case SchemaMigrationStatus.NotLegacy:
                case SchemaMigrationStatus.IdentityMismatch:
                    // The load below reports the blocking status with its own warning.
                    Mod.Log?.Debug($"[Payout] payout state {statePath} not upgraded ({slot}): {result.Detail}.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Mod.Log?.Warn($"[Payout] pre-schema payout state upgrade of {statePath} threw: {ex.Message} - state loaded as-is.");
        }
    }

    // --- reads ----------------------------------------------------------------------

    /// <summary>In-memory state, or a transient default when nothing is loaded (never written).</summary>
    public static PayoutState GetState() =>
        Engine.State ?? new PayoutState { SaveIdentity = Engine.ActiveSlot ?? "", LastPaidElapsedDay = -1 };

    public static bool IsDayPaid(int elapsedDay)
    {
        var s = Engine.State;
        return s != null && s.LastPaidElapsedDay >= elapsedDay;
    }

    /// <summary>True when the day is only covered by the fresh-install seed, not a real payout.</summary>
    public static bool IsInitializationOnly(int elapsedDay)
    {
        var s = Engine.State;
        return s != null
            && s.InitializationDay >= 0
            && s.LastPaidElapsedDay == s.InitializationDay
            && elapsedDay <= s.InitializationDay;
    }

    public static bool IsBusinessPaid(string businessId, int elapsedDay)
    {
        var s = Engine.State;
        return s != null && s.LastPaidDayByBusiness.TryGetValue(businessId, out int lastDay) && lastDay >= elapsedDay;
    }

    public static PendingPayoutState? ReadPendingMarker() => Engine.PendingLoad.Marker;

    public static PendingStatus PendingStatus => Engine.PendingLoad.Status;

    // --- mutations (explicit PayoutResult, never a bare bool) ------------------------

    public static PayoutResult TryBook(int day, float net, IReadOnlyList<string> businessIds, bool force)
    {
        if (!IsAuthoritative)
            return new PayoutResult(PayoutOutcome.NotAuthoritative, day, net, businessIds.Count, "active slot unknown - no write");
        return Engine.TryBook(day, net, businessIds, GetStateFilePath(), GetPendingFilePath(), force);
    }

    /// <summary>Dry-run preview: no bank call, no storage write, no host requirement.</summary>
    public static PayoutResult Preview(int day, float net, int businessCount) => Engine.Preview(day, net, businessCount);

    /// <summary>Forward-only commit with no bank call (seed / cap / terminal day).</summary>
    public static PayoutResult CommitOnly(int day, IReadOnlyList<string> businessIds)
    {
        if (!IsAuthoritative)
            return new PayoutResult(PayoutOutcome.NotAuthoritative, day, 0f, businessIds.Count, "active slot unknown - no write");
        return Engine.CommitOnly(day, businessIds, GetStateFilePath());
    }

    /// <summary>Back-compat bool commit; prefer <see cref="CommitOnly"/> for the explicit outcome.</summary>
    public static bool CommitPayout(int elapsedDay, IEnumerable<string> paidBusinessIds)
        => CommitOnly(elapsedDay, paidBusinessIds.ToList()).Succeeded;

    /// <summary>
    /// Explicit, fail-closed fresh-install initialization. Seeds GENUINELY missing state only,
    /// and only after a durable write (a failed write never advances memory). A pending marker
    /// or any blocking/non-missing state is never overwritten.
    /// </summary>
    public static PayoutResult Initialize(int day)
    {
        if (!IsAuthoritative)
            return new PayoutResult(PayoutOutcome.NotAuthoritative, day, 0f, 0, "active slot unknown - no write");
        return Engine.Initialize(day, GetStateFilePath());
    }

    /// <summary>Back-compat alias for <see cref="Initialize"/> (fresh-install seed).</summary>
    public static PayoutResult MarkInitialization(int day) => Initialize(day);

    public static PayoutResult ConfirmPending()
    {
        if (!IsAuthoritative) return new PayoutResult(PayoutOutcome.NotAuthoritative, -1, 0f, 0, "active slot unknown");
        return Engine.ConfirmPending(GetStateFilePath(), GetPendingFilePath());
    }

    public static PayoutResult ResolvePending()
    {
        if (!IsAuthoritative) return new PayoutResult(PayoutOutcome.NotAuthoritative, -1, 0f, 0, "active slot unknown");
        return Engine.ResolvePending(GetPendingFilePath());
    }

    public static void ClearPendingMarker()
    {
        string p = GetPendingFilePath();
        if (!string.IsNullOrEmpty(p)) PayoutCodec.ClearMarker(Storage, p);
        Engine.LoadPending(p);
    }
}
