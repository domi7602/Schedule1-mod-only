// ---------------------------------------------------------------------------
// Core/PayoutPolicies.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so a source-contract reader can recover that
// file's exact content.
//
//   Core/LegacyMigration.cs          -> section Core/LegacyMigration.cs
//   Core/PayoutScheduler.cs          -> section Core/PayoutScheduler.cs
//   Core/PayoutWindowPlanner.cs      -> section Core/PayoutWindowPlanner.cs
//   Core/Seams.cs                    -> section Core/Seams.cs
//
// ---------------------------------------------------------------------------

namespace BusinessIncome.Core
{
    // ==== BusinessIncome section: Core/LegacyMigration.cs ====
    using System;


    /// <summary>
    /// Pure legacy-migration decision. The legacy file is NEVER deleted, and it is only
    /// auto-adopted when its SaveIdentity matches the explicit migration target exactly.
    /// Anything ambiguous is surfaced loudly (BlockAmbiguous) instead of speculatively
    /// counting saves.
    /// </summary>
    public static class LegacyMigration
    {
        public static LegacyMigrationDecision Decide(
            string? legacyIdentity,
            string targetIdentity,
            bool targetExists,
            bool legacyExists)
        {
            if (!legacyExists) return LegacyMigrationDecision.None;
            if (targetExists) return LegacyMigrationDecision.TargetExists;

            if (!string.IsNullOrEmpty(legacyIdentity)
                && !string.IsNullOrEmpty(targetIdentity)
                && string.Equals(legacyIdentity, targetIdentity, StringComparison.OrdinalIgnoreCase))
                return LegacyMigrationDecision.AdoptMatching;

            return LegacyMigrationDecision.BlockAmbiguous;
        }
    }
    // ==== end BusinessIncome section: Core/LegacyMigration.cs ====
}

namespace BusinessIncome.Core
{
    // ==== BusinessIncome section: Core/PayoutScheduler.cs ====
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// Pure catch-up planner: produces the candidate days oldest-to-newest and stops the
    /// enumeration at the first day that is not durable (the executor must also stop there).
    /// Loop size is bounded by the cap and all arithmetic is done in long to be overflow-safe.
    /// </summary>
    public static class PayoutScheduler
    {
        public const int MinCatchupDays = 1;
        public const int MaxCatchupDays = 365;

        public static CatchupPlan PlanCatchup(
            long lastPaidDay,
            long currentDay,
            int maxCatchupDays,
            bool hasBusinesses,
            bool stateDurable)
        {
            if (!stateDurable)
                return new CatchupPlan { Days = Array.Empty<int>(), StoppedEarly = true, Reason = "state not durable" };

            if (lastPaidDay < -1)
                return new CatchupPlan { Days = Array.Empty<int>(), StoppedEarly = true, Reason = "invalid last-paid day" };

            int cap = ClampCap(maxCatchupDays);

            if (currentDay <= lastPaidDay)
                return new CatchupPlan { Days = Array.Empty<int>(), Reason = "nothing to catch up" };

            long windowStart = currentDay - cap + 1;
            if (windowStart < lastPaidDay + 1) windowStart = lastPaidDay + 1;
            if (windowStart < 1) windowStart = 1;

            var days = new List<int>();
            for (long d = windowStart; d <= currentDay; d++)
            {
                if (d > int.MaxValue) break; // overflow-safe guard
                days.Add((int)d);
                if (days.Count > MaxCatchupDays) break; // hard loop cap
            }

            bool capped = (currentDay - lastPaidDay) > cap;

            // "zero/no business terminal if persisted": a saved day with no businesses is
            // terminal. It must STILL return the days so the executor can durably commit them —
            // otherwise the backlog survives and later pays as a windfall once a business is
            // bought. Same oldest-to-newest, capped enumeration as a normal backlog.
            if (!hasBusinesses)
                return new CatchupPlan
                {
                    Days = days,
                    StoppedEarly = capped,
                    // These are executable terminal commits, not an instruction to skip the pass.
                    TerminalSkip = false,
                    Reason = capped
                        ? $"no owned businesses — terminal days {days[0]}..{days[^1]} (cap {cap})"
                        : "no owned businesses — days treated as terminal"
                };

            return new CatchupPlan
            {
                Days = days,
                StoppedEarly = capped,
                Reason = capped
                    ? $"backlog exceeds cap {cap}; paying only the last {days.Count} day(s)"
                    : "full backlog within cap"
            };
        }

        public static int ClampCap(int maxCatchupDays)
        {
            if (maxCatchupDays < MinCatchupDays) return MinCatchupDays;
            if (maxCatchupDays > MaxCatchupDays) return MaxCatchupDays;
            return maxCatchupDays;
        }
    }
    // ==== end BusinessIncome section: Core/PayoutScheduler.cs ====
}

namespace BusinessIncome.Core
{
    // ==== BusinessIncome section: Core/PayoutWindowPlanner.cs ====
    using System;


    /// <summary>
    /// Centralized, pure payout-window planner. Fails closed: an unknown current time for the
    /// CURRENT day keeps the day out of the window (never pay on a bad time read). Past days
    /// (backlog) bypass the window. Hour math is overflow-safe because the hour is clamped.
    /// </summary>
    public static class PayoutWindowPlanner
    {
        public static WindowDecision Plan(int payoutHour, int currentTime24h, bool timeKnown, bool isPastDay)
        {
            if (isPastDay)
                return new WindowDecision(true, "past-day backlog bypasses the window");

            if (payoutHour == 0)
                return new WindowDecision(true, "midnight/day-pass window");

            if (payoutHour < 0 || payoutHour > 23)
                return new WindowDecision(false, "fail-closed: invalid payout hour");

            if (!timeKnown)
                return new WindowDecision(false, "fail-closed: time probe failed");

            int start = payoutHour * 100; // <= 2300, no overflow
            int end = start + 59;
            bool inWindow = currentTime24h >= start && currentTime24h <= end;
            return inWindow
                ? new WindowDecision(true, $"in window {start:D4}-{end:D4}")
                : new WindowDecision(false, $"outside window {start:D4}-{end:D4}");
        }
    }
    // ==== end BusinessIncome section: Core/PayoutWindowPlanner.cs ====
}

namespace BusinessIncome.Core
{
    // ==== BusinessIncome section: Core/Seams.cs ====
    using System;


    /// <summary>
    /// Pure storage seam. The production adapter forwards to <c>S1Mods.Shared.SafeStorage</c>
    /// (atomic write + .bak copy + .tmp staging); unit tests inject fakes that simulate
    /// per-path write/delete failures without touching disk or Unity.
    /// </summary>
    public interface IPayoutStorage
    {
        /// <summary>True when the main file at <paramref name="path"/> exists.</summary>
        bool Exists(string path);

        /// <summary>True when the <c>.bak</c> sidecar at <paramref name="path"/> exists.</summary>
        bool BackupExists(string path);

        /// <summary>Raw text of the file, or null when missing/unreadable. Never throws.</summary>
        string? ReadText(string path);

        /// <summary>Atomically writes text. Returns false when the durable write failed.</summary>
        bool WriteAtomic(string path, string content);

        /// <summary>Deletes a single file. Returns true when the file is gone afterwards (or never existed). Never throws.</summary>
        bool DeleteFile(string path);
    }

    /// <summary>
    /// Pure bank seam. Mirrors the <c>IGameWallet</c> guidance in money-api-semantics.md:
    /// a clean return means "request accepted", not "settlement confirmed"; the call may
    /// mutate-then-throw, in which case the outcome is UNKNOWN and must never be retried.
    /// </summary>
    public interface IPayoutBank
    {
        /// <summary>Live money-manager readiness probe (not cached).</summary>
        bool IsReady();

        /// <summary>
        /// Requests an online transaction. Returns true when the request was handed off cleanly
        /// (request accepted, NOT confirmed settlement). May throw after mutating.
        /// </summary>
        bool RequestTransfer(string transactionName, float unitAmount, string note);
    }

    /// <summary>Logging seam so the pure core stays free of MelonLoader/Unity.</summary>
    public delegate void PayoutLog(string message);
    // ==== end BusinessIncome section: Core/Seams.cs ====
}
