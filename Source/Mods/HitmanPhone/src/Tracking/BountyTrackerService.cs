using System;
using System.Collections.Generic;
using System.Globalization;
using HitmanPhone.Persistence;

namespace HitmanPhone.Bounty;

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
/// State rule (the three-way split the old UI blurred):
///   • "Ausgeschaltet"      — knockout/unconscious only (Died = false).
///   • "Bestaetigt tot"     — game-confirmed death (Died = true).
///   • "Auftrag abgeschlossen" — a separate contract flag; a completed contract
///     is NOT proof of death (it can be paid out on a knockout polaroid).
/// </summary>
public static class BountyTrackerService
{
    /// <summary>One tracked target.</summary>
    public sealed class Row
    {
        public string TargetNpcId = string.Empty;
        public string TargetName = string.Empty;
        /// <summary>True when game-confirmed dead.</summary>
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
        public int ConfirmedDead;
        public int OnlyOut;
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

        snap.Rows.AddRange(byId.Values);
        snap.Rows.Sort((a, b) =>
        {
            int byName = string.Compare(a.TargetName, b.TargetName, StringComparison.OrdinalIgnoreCase);
            return byName != 0 ? byName : string.Compare(a.TargetNpcId, b.TargetNpcId, StringComparison.OrdinalIgnoreCase);
        });
        for (int i = 0; i < snap.Rows.Count; i++)
        {
            var r = snap.Rows[i];
            if (r.Died) snap.ConfirmedDead++;
            else snap.OnlyOut++;
            if (r.ContractCompleted) snap.ContractsCompleted++;
        }
        return snap;
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
