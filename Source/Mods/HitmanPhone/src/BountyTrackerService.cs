using System;
using System.Collections.Generic;
using System.Globalization;
using HitmanPhone.Persistence;

namespace HitmanPhone.Bounty;

/// <summary>
/// R07 — pure render gate for <see cref="BountyTrackerApp"/>.
///
/// The app only re-renders when the value it would display changes. The key is
/// committed with <see cref="Commit"/> ONLY after a rebuild actually succeeded:
/// a transient UI failure with unchanged data therefore stays pending and is
/// retried on the next tick instead of being masked until the app is reopened.
/// Kept Unity-free so the pure suite covers the invariant.
/// </summary>
public sealed class TrackerRenderGate
{
    private string _committed = string.Empty;

    /// <summary>True when the key differs from the last successfully rendered key.</summary>
    public bool NeedsRebuild(string renderedKey) =>
        !string.Equals(renderedKey, _committed, StringComparison.Ordinal);

    /// <summary>Record a successfully rendered key. Call only after the rebuild completed.</summary>
    public void Commit(string renderedKey) => _committed = renderedKey ?? string.Empty;

    /// <summary>Force the next check to rebuild (e.g. the app was just opened).</summary>
    public void Reset() => _committed = string.Empty;
}

/// <summary>
/// Phase E — save-scoped kill/contract tracker queries.
///
/// Pure aggregation over <see cref="BountySaveData"/> (no Unity, no game types)
/// so Source/Tests/HitmanPhone.Tests can cover it. The UI lives in
/// <see cref="BountyTrackerApp"/>.
///
/// Scope rule: ONLY hitman-contract targets are tracked (records are written by
/// <see cref="BountyService.RecordKillEvent"/>, which only ever fires for a
/// matched contract). Multiple contracts on the same NPC produce exactly ONE
/// historical record (the record is unique per NPC id).
///
/// State rule (HP-08 — the three-way split the old UI blurred):
///   • no elimination recorded — a row that exists only because a contract
///     references the target (accepted, expired, or completed without an event).
///     This is NOT proof of a knockout: the old build counted every such row as
///     "out of action" and invented an elimination on mere contract acceptance.
///   • knockout/unconscious only — a recorded event with Died == false.
///   • confirmed dead — a recorded event with Died == true.
/// Contract completion stays a separate fact: a completed contract can be paid
/// on a knockout polaroid and is never evidence of a kill.
/// </summary>
public static class BountyTrackerService
{
    /// <summary>One tracked target.</summary>
    public sealed class Row
    {
        public string TargetNpcId = string.Empty;
        public string TargetName = string.Empty;
        /// <summary>
        /// True when at least one elimination EVENT (knockout or death) was
        /// recorded for this target. A row created only from a contract has no
        /// event and must never be rendered or counted as an elimination.
        /// </summary>
        public bool HasElimination;
        /// <summary>True when game-confirmed dead. Only meaningful while <see cref="HasElimination"/>.</summary>
        public bool Died;
        /// <summary>Day of the recorded elimination event (first, upgraded on death).</summary>
        public int Day;
        /// <summary>Minute-of-day-sum of the recorded elimination event.</summary>
        public long MinuteSum;
        /// <summary>True when at least one contract on this target completed.</summary>
        public bool ContractCompleted;
        /// <summary>True when a contract on this target is still active.</summary>
        public bool ContractActive;
    }

    /// <summary>Aggregated snapshot for the tracker screen.</summary>
    public sealed class Snapshot
    {
        public List<Row> Rows = new();
        /// <summary>Unique targets with a recorded, game-confirmed death event.</summary>
        public int ConfirmedDead;
        /// <summary>Unique targets with a recorded knockout-only event.</summary>
        public int OnlyOut;
        /// <summary>
        /// Completed CONTRACTS (not rows). Two completed contracts on the same
        /// NPC are two completed contracts but one unique target row (HP-08).
        /// </summary>
        public int ContractsCompleted;
    }

    /// <summary>
    /// Build the tracker view from the current save. Deterministic order: name
    /// ascending, then id.
    /// </summary>
    public static Snapshot Build(BountySaveData save)
    {
        var snap = new Snapshot();
        var byId = new Dictionary<string, Row>(StringComparer.OrdinalIgnoreCase);

        if (save.KillEvents != null)
        {
            for (int i = 0; i < save.KillEvents.Count; i++)
            {
                var e = save.KillEvents[i];
                if (e == null || string.IsNullOrEmpty(e.TargetNpcId)) continue;
                if (!byId.TryGetValue(e.TargetNpcId, out var row))
                {
                    row = new Row { TargetNpcId = e.TargetNpcId };
                    byId[e.TargetNpcId] = row;
                }
                // A recorded event — knockout or death — is the only thing that
                // makes this row an elimination.
                row.HasElimination = true;
                // First record wins for naming/time unless the death upgrade moved it.
                if (string.IsNullOrEmpty(row.TargetName)) row.TargetName = e.TargetName ?? string.Empty;
                if (!row.Died && e.Died)
                {
                    row.Died = true;
                    row.Day = e.Day;
                    row.MinuteSum = e.MinuteSum;
                }
                else if (!row.Died && row.Day == 0 && row.MinuteSum == 0)
                {
                    row.Day = e.Day;
                    row.MinuteSum = e.MinuteSum;
                }
            }
        }

        ApplyContractFlags(save.Active, EBountyStatus.Active, byId);
        // Completed/failed/expired contracts live in the single History list;
        // row flags are derived from each contract's own Status.
        ApplyContractFlags(save.History, null, byId);

        // HP-08: the contract total comes from the contract records, never from a
        // per-row boolean (which collapsed same-target contracts into one).
        snap.ContractsCompleted = CountCompleted(save.Active) + CountCompleted(save.History);

        snap.Rows.AddRange(byId.Values);
        snap.Rows.Sort((a, b) =>
        {
            int byName = string.Compare(a.TargetName, b.TargetName, StringComparison.OrdinalIgnoreCase);
            return byName != 0 ? byName : string.Compare(a.TargetNpcId, b.TargetNpcId, StringComparison.OrdinalIgnoreCase);
        });
        for (int i = 0; i < snap.Rows.Count; i++)
        {
            var r = snap.Rows[i];
            if (!r.HasElimination) continue;   // contract-only row: no invented elimination
            if (r.Died) snap.ConfirmedDead++;
            else snap.OnlyOut++;
        }
        return snap;
    }

    private static int CountCompleted(List<BountyContract> contracts)
    {
        if (contracts == null) return 0;
        int count = 0;
        for (int i = 0; i < contracts.Count; i++)
        {
            var c = contracts[i];
            if (c != null && c.Status == EBountyStatus.Completed) count++;
        }
        return count;
    }

    private static void ApplyContractFlags(List<BountyContract> contracts, EBountyStatus? status, Dictionary<string, Row> byId)
    {
        if (contracts == null) return;
        for (int i = 0; i < contracts.Count; i++)
        {
            var c = contracts[i];
            if (c == null || string.IsNullOrEmpty(c.TargetNpcId)) continue;
            if (!byId.TryGetValue(c.TargetNpcId, out var row))
            {
                row = new Row { TargetNpcId = c.TargetNpcId, TargetName = c.TargetNpcName ?? string.Empty };
                byId[c.TargetNpcId] = row;
            }
            var effective = status ?? c.Status;
            if (effective == EBountyStatus.Completed) row.ContractCompleted = true;
            if (effective == EBountyStatus.Active) row.ContractActive = true;
        }
    }

    /// <summary>In-game timestamp for display: "Day 3 · 13:45".</summary>
    public static string FormatEventTime(int day, long minuteSum)
    {
        int minuteOfDay = (int)(minuteSum % 1440);
        if (minuteOfDay < 0) minuteOfDay += 1440;
        int hh = minuteOfDay / 60;
        int mm = minuteOfDay % 60;
        return string.Format(CultureInfo.InvariantCulture,
            "Day {0} \u00b7 {1:D2}:{2:D2}", day, hh, mm);
    }
}
