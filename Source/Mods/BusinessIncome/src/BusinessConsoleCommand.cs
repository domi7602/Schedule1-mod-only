using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BusinessIncome.Config;
using BusinessIncome.Core;
using BusinessIncome.Services;
using MelonLoader;
using S1API.Console;
using S1Mods.Shared;

namespace BusinessIncome.Console;

public sealed class BusinessConsoleCommand : BaseConsoleCommand
{
    public override string CommandWord => "biz";

    public override string CommandDescription =>
        "BusinessIncome: stats, trigger [--commit], config, set <key> <val>, pending, catchup, help";

    public override string ExampleUsage => "biz stats";

    public override void ExecuteCommand(List<string> args)
    {
        try
        {
            if (args == null || args.Count == 0 || Is(args, 0, "stats", "info", "s"))
            {
                PrintStats();
                return;
            }

            string sub = args[0].ToLowerInvariant();
            switch (sub)
            {
                case "trigger":
                case "pay":
                case "run":
                    ExecuteTrigger(args);
                    return;

                case "config":
                case "cfg":
                    PrintConfig();
                    return;

                case "set":
                    ExecuteSet(args);
                    return;

                case "help":
                case "?":
                case "h":
                    PrintHelp();
                    return;

                case "pending":
                    ExecutePending(args);
                    return;

                case "catchup":
                    ExecuteCatchup();
                    return;

                default:
                    Print($"<color=#ff6060>Unknown command: '{sub}'. Use 'biz help'.</color>");
                    return;
            }
        }
        catch (Exception ex)
        {
            Print($"<color=#ff6060>Error: {ex.Message}</color>");
            Mod.Log.Error($"Error in biz command: {ex}");
        }
    }

    /// <summary>
    /// Reads the in-game day. Returns false when the game clock is unavailable — callers must
    /// ABORT rather than silently operate on day 0.
    /// </summary>
    private static bool TryGetElapsedDays(out int elapsedDays)
    {
        try
        {
            elapsedDays = S1API.GameTime.TimeManager.ElapsedDays;
            return true;
        }
        catch
        {
            elapsedDays = -1;
            return false;
        }
    }

    private void PrintStats()
    {
        if (!TryGetElapsedDays(out int elapsedDays))
        {
            Print("<color=#ff6060>Cannot read the in-game day — aborting to avoid reporting/paying against day 0.</color>");
            return;
        }

        var config = ModConfig<BusinessIncomeConfig>.Instance;
        if (!IncomeEngine.TryGetDailyRevenuePreview(elapsedDays, config, out var preview))
        {
            Print("<color=#ff6060>Cannot read the owned-business list (unreadable or partial) — failing closed. This is NOT 'no businesses'.</color>");
            return;
        }
        var (lines, totalGross, totalCosts, totalNet) = preview;

        bool isPaid = PayoutStateStore.IsDayPaid(elapsedDays);
        bool isInitialization = PayoutStateStore.IsInitializationOnly(elapsedDays);
        string slotSuffix = PayoutStateStore.GetActiveSlotSuffix();
        bool isHost = IncomeEngine.IsHostOrSingleplayer();

        string paidStr = isPaid
            ? (isInitialization ? "<color=#ffaa00>INITIALIZED</color>" : "<color=#60f080>YES</color>")
            : "<color=#ffaa00>NO</color>";

        string stateStr;
        if (!PayoutStateStore.IsAuthoritative) stateStr = "<color=#ffaa00>slot unresolved (payouts blocked)</color>";
        else if (PayoutStateStore.StateLoad.BlocksMutation) stateStr = $"<color=#ff6060>{PayoutStateStore.StateLoad.Status} (blocked)</color>";
        else stateStr = PayoutStateStore.StateLoad.Status.ToString();

        var sb = new StringBuilder();
        sb.AppendLine("<color=#60f080>==================================================</color>");
        sb.AppendLine($"<color=#60f080>★ Business Income Dashboard</color> (Day {elapsedDays})");
        sb.AppendLine($"<color=#aaaaaa>Save Slot:</color> {slotSuffix}  |  <color=#aaaaaa>Host:</color> {(isHost ? "Yes" : "No")}  |  <color=#aaaaaa>Paid Today:</color> {paidStr}");
        sb.AppendLine($"<color=#aaaaaa>State:</color> {stateStr}  |  <color=#aaaaaa>Ready:</color> {(PayoutStateStore.IsReady ? "Yes" : "No")}");

        if (PayoutStateStore.PendingStatus != PendingStatus.Missing)
        {
            string how = PayoutStateStore.PendingStatus == PendingStatus.Corrupt ? "CORRUPT" : "unresolved";
            sb.AppendLine($"<color=#ff6060>Pending payout marker is {how} — new automatic AND forced payouts are BLOCKED. Run 'biz pending'.</color>");
        }

        sb.AppendLine("<color=#60f080>--------------------------------------------------</color>");

        if (lines.Count == 0)
        {
            sb.AppendLine("<color=#ffaa00>No owned businesses found.</color>");
            sb.AppendLine("<color=#888888>Buy businesses like Laundromat or Car Wash for passive income.</color>");
        }
        else
        {
            sb.AppendLine(string.Format("{0,-18} {1,5} {2,6} {3,7} {4,8} {5,8}", "Business", "Mult", "Staff", "Var", "Gross", "Net"));
            sb.AppendLine("<color=#444444>--------------------------------------------------</color>");

            foreach (var l in lines)
            {
                string staffStr = l.EmployeeCount > 0 ? $"+{l.EmployeeBonusPercent * 100:0}%" : "-";
                string varStr = $"{l.VarianceFactor * 100:0}%";
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0,-18} {1,5:0.00} {2,6} {3,7} {4,8} {5,8}",
                    ConfigCollectionSanitizer.EscapeForConsole(Truncate(l.DisplayName, 18)),
                    l.Multiplier,
                    staffStr,
                    varStr,
                    "$" + l.GrossRevenue.ToString("N0", CultureInfo.InvariantCulture),
                    "$" + l.NetRevenue.ToString("N0", CultureInfo.InvariantCulture)));
            }

            sb.AppendLine("<color=#444444>--------------------------------------------------</color>");
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0,-18} {1,5} {2,6} {3,7} {4,8} {5,8}",
                "TOTAL (" + lines.Count + ")",
                "-",
                "-",
                "-",
                "$" + totalGross.ToString("N0", CultureInfo.InvariantCulture),
                "$" + totalNet.ToString("N0", CultureInfo.InvariantCulture)));
            sb.AppendLine($"<color=#aaaaaa>Operating Costs:</color> -${totalCosts.ToString("N2", CultureInfo.InvariantCulture)} ({config.OperatingCostRate * 100:0}%)");
        }

        sb.AppendLine("<color=#60f080>==================================================</color>");
        Print(sb.ToString());
    }

    private void ExecuteTrigger(List<string> args)
    {
        bool commit = args.Exists(a => a.Equals("--commit", StringComparison.OrdinalIgnoreCase) || a.Equals("-c", StringComparison.OrdinalIgnoreCase));

        if (!TryGetElapsedDays(out int elapsedDays))
        {
            Print("<color=#ff6060>Cannot read the in-game day — aborting trigger.</color>");
            return;
        }

        var config = ModConfig<BusinessIncomeConfig>.Instance;

        if (!commit)
        {
            Print($"<color=#ffaa00>Running simulation (dry-run) for day {elapsedDays}...</color>");

            // Single source of truth: use the dry-run's OWN result. It already carries the
            // preview (Net / BusinessCount) and returns NotReady - never DryRun - when the
            // owned-business read is untrustworthy. A separate GetDailyRevenuePreview call would
            // return the silent $0 fallback and falsely report a completed simulation.
            var dry = IncomeEngine.ExecuteDailyPayout(elapsedDays, config, force: true, commit: false, isDryRun: true);

            if (dry.Outcome == PayoutOutcome.DryRun)
            {
                Print($"<color=#60f080>Simulation complete: net revenue would be +${dry.Net.ToString("N2", CultureInfo.InvariantCulture)} ({dry.BusinessCount} {(dry.BusinessCount == 1 ? "business" : "businesses")}). Nothing was booked or saved.</color>");
                Print("<color=#888888>Tip: Use 'biz trigger --commit' for a real bank transfer.</color>");
            }
            else
            {
                Print($"<color=#ff6060>Dry-run could not be completed ({dry.Outcome}: {dry.Detail}). No figures are shown because the owned-business read was not trustworthy - this is NOT a $0 payout.</color>");
            }
            return;
        }

        // Audit 2026-09-13 (BIZ-02): force:true skips the Idempotency gate — without a
        // confirmation this command was a repeatable money printer on the same day.
        if (PayoutStateStore.IsDayPaid(elapsedDays) && !args.Exists(a => a.Equals("--force", StringComparison.OrdinalIgnoreCase)))
        {
            Print($"<color=#ffaa00>Day {elapsedDays} is already paid. Add --force to book AGAIN (double payout!).</color>");
            return;
        }

        Print($"<color=#60f080>Executing authoritative bank booking for day {elapsedDays}...</color>");
        var r = IncomeEngine.ExecuteDailyPayout(elapsedDays, config, force: true, commit: true, isDryRun: false);

        switch (r.Outcome)
        {
            case PayoutOutcome.Requested:
                Print("<color=#60f080>Bank request ACCEPTED and payout state saved. (A clean return means the request was accepted — it is not independent confirmation of settlement.)</color>");
                break;

            case PayoutOutcome.BookedStateNotSaved:
                Print("<color=#ffaa00>Bank request accepted but the payout state was NOT saved. Pending marker kept — run 'biz pending confirm' or 'biz pending resolve'.</color>");
                break;

            case PayoutOutcome.UnknownOutcome:
                Print("<color=#ff6060>The bank call raised AFTER being invoked — outcome UNKNOWN. Marker kept and retry blocked; check the bank app, then run 'biz pending confirm' or 'biz pending resolve'.</color>");
                break;

            case PayoutOutcome.NoBusinesses:
                Print("<color=#ffaa00>No owned businesses — day marked terminal, nothing booked.</color>");
                break;

            case PayoutOutcome.ZeroNet:
                Print("<color=#ffaa00>Net revenue is $0 — day marked terminal, nothing booked.</color>");
                break;

            case PayoutOutcome.AlreadyPaid:
                Print($"<color=#ffaa00>Day {elapsedDays} is already paid.</color>");
                break;

            case PayoutOutcome.NotReady:
            case PayoutOutcome.NotAuthoritative:
                Print($"<color=#ff6060>Payout blocked — runtime not ready or active slot unresolved ({r.Detail}). No write was made.</color>");
                break;

            case PayoutOutcome.BlockedPending:
                Print($"<color=#ff6060>Payout blocked: an unresolved pending payout exists ({r.Detail}). Run 'biz pending confirm' or 'biz pending resolve'.</color>");
                break;

            case PayoutOutcome.BlockedStateCorrupt:
                Print($"<color=#ff6060>Payout blocked: payout state is invalid ({r.Detail}). Refusing to seed over corrupt data — fix or remove the state file.</color>");
                break;

            case PayoutOutcome.BlockedMarkerWrite:
                Print("<color=#ff6060>Payout blocked: the write-ahead marker could not be written durably. No bank call was made.</color>");
                break;

            case PayoutOutcome.StateNotPersisted:
                Print("<color=#ff6060>Payout could not be persisted — no bank call was made.</color>");
                break;

            default:
                Print($"<color=#ffaa00>Payout not executed ({r.Outcome}: {r.Detail}).</color>");
                break;
        }
    }

    private void PrintConfig()
    {
        var cfg = ModConfig<BusinessIncomeConfig>.Instance;
        var sb = new StringBuilder();
        sb.AppendLine("<color=#60f080>[BusinessIncome Configuration]</color>");
        sb.AppendLine($"  PayoutHour:              {cfg.PayoutHour} (0 = Midnight / DayPass)");
        sb.AppendLine($"  DefaultBaseIncome:       ${cfg.DefaultBaseIncome.ToString("N2", CultureInfo.InvariantCulture)}");
        sb.AppendLine($"  OperatingCostRate:       {cfg.OperatingCostRate * 100:0}%");
        sb.AppendLine($"  EmployeeBonusPerWorker:  +{cfg.EmployeeBonusPerWorker * 100:0}% (Max: +{cfg.MaxEmployeeBonus * 100:0}%)");
        sb.AppendLine($"  WeekendBonusRate:        +{cfg.WeekendBonusRate * 100:0}%");
        sb.AppendLine($"  EnableNotifications:     {cfg.EnableNotifications}");
        sb.AppendLine($"  PlayCashSound:           {cfg.PlayCashSound}");
        sb.AppendLine($"  MaxCatchupDays:          {cfg.MaxCatchupDays}");
        sb.AppendLine("  Multipliers:");
        foreach (var kvp in cfg.PropertyMultipliers)
        {
            sb.AppendLine($"    - {kvp.Key}: {kvp.Value.ToString("0.00", CultureInfo.InvariantCulture)}x");
        }
        Print(sb.ToString());
    }

    private void ExecuteSet(List<string> args)
    {
        if (args.Count < 3)
        {
            Print("<color=#ff6060>Usage:</color> biz set <base|hour|costs|notif|sound|maxcatchup> <value>");
            return;
        }

        string key = args[1].ToLowerInvariant();
        string val = args[2];
        var cfg = ModConfig<BusinessIncomeConfig>.Instance;

        switch (key)
        {
            case "base":
            case "baseincome":
                if (float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out float baseVal)
                    && baseVal >= 0f && !float.IsNaN(baseVal) && !float.IsInfinity(baseVal))
                {
                    Print(ApplySetting(() => ModConfig<BusinessIncomeConfig>.SetAndSave("DefaultBaseIncome", baseVal),
                        $"DefaultBaseIncome set to ${baseVal.ToString("N2", CultureInfo.InvariantCulture)}."));
                }
                else Print("<color=#ff6060>Invalid number value (must be finite and >= 0).</color>");
                return;

            case "hour":
            case "payouthour":
                if (int.TryParse(val, out int hourVal) && hourVal >= 0 && hourVal <= 23)
                {
                    Print(ApplySetting(() => ModConfig<BusinessIncomeConfig>.SetAndSave("PayoutHour", hourVal),
                        $"PayoutHour set to {hourVal}:00."));
                }
                else Print("<color=#ff6060>Invalid hour (0-23).</color>");
                return;

            case "costs":
            case "costrate":
                if (float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out float costVal)
                    && costVal >= 0f && costVal <= 1f && !float.IsNaN(costVal))
                {
                    Print(ApplySetting(() => ModConfig<BusinessIncomeConfig>.SetAndSave("OperatingCostRate", costVal),
                        $"OperatingCostRate set to {costVal * 100:0}%."));
                }
                else Print("<color=#ff6060>Invalid rate (0.00 - 1.00).</color>");
                return;

            case "notif":
            case "notifications":
                if (bool.TryParse(val, out bool notifVal))
                {
                    Print(ApplySetting(() => ModConfig<BusinessIncomeConfig>.SetAndSave("EnableNotifications", notifVal),
                        $"EnableNotifications set to {notifVal}."));
                }
                else Print("<color=#ff6060>Invalid bool value (true/false).</color>");
                return;

            case "sound":
                if (bool.TryParse(val, out bool soundVal))
                {
                    Print(ApplySetting(() => ModConfig<BusinessIncomeConfig>.SetAndSave("PlayCashSound", soundVal),
                        $"PlayCashSound set to {soundVal}."));
                }
                else Print("<color=#ff6060>Invalid bool value (true/false).</color>");
                return;

            case "maxcatchup":
            case "catchupdays":
                if (int.TryParse(val, out int catchupVal)
                    && catchupVal >= PayoutScheduler.MinCatchupDays && catchupVal <= PayoutScheduler.MaxCatchupDays)
                {
                    Print(ApplySetting(() => ModConfig<BusinessIncomeConfig>.SetAndSave("MaxCatchupDays", catchupVal),
                        $"MaxCatchupDays set to {catchupVal}."));
                }
                else Print($"<color=#ff6060>Invalid day count ({PayoutScheduler.MinCatchupDays}-{PayoutScheduler.MaxCatchupDays}).</color>");
                return;

            default:
                Print($"<color=#ff6060>Unknown key '{key}'.</color> Allowed: base, hour, costs, notif, sound, maxcatchup");
                return;
        }
    }

    /// <summary>
    /// Applies a scalar setting and reports the TWO independent persistence channels separately.
    /// The scalar lives in the MelonPreferences TOML (written by ModConfig.SetAndSave); the JSON
    /// sidecar holds ONLY the collections (multipliers / display names / weekend categories).
    /// A sidecar write failure must therefore never be reported as "the setting will not persist".
    ///
    /// NOTE: ModConfig.SetAndSave returns void, so TOML persistence cannot be confirmed from here;
    /// the scalar is reported as applied-and-saved only in the sense that SetAndSave did not throw.
    /// </summary>
    private static string ApplySetting(Action apply, string successMessage)
    {
        try
        {
            apply();
            var cfg = ModConfig<BusinessIncomeConfig>.Instance;
            cfg.Sanitize();
            bool sidecarSaved = ConfigJsonStore.Save(cfg);
            if (sidecarSaved)
                return $"<color=#60f080>{successMessage}</color>";

            return $"<color=#ffaa00>{successMessage} The scalar setting was applied and saved to the mod\'s TOML config " +
                   "(ModConfig.SetAndSave returns no success flag, so this build cannot independently confirm the TOML write); " +
                   "only the SEPARATE JSON sidecar (business multipliers / display names / weekend categories) failed to write, " +
                   "so those collection values will not persist across restarts.</color>";
        }
        catch (Exception ex)
        {
            return $"<color=#ff6060>Failed to apply setting: {ex.Message}</color>";
        }
    }

    private void PrintHelp()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<color=#60f080>[BusinessIncome Help]</color>");
        sb.AppendLine("  biz stats                  - Shows revenue preview of all owned businesses");
        sb.AppendLine("  biz trigger                - Simulates a payout (dry-run without booking)");
        sb.AppendLine("  biz trigger --commit       - Executes real payout and saves marker");
        sb.AppendLine("  biz config                 - Shows current configuration");
        sb.AppendLine("  biz set <key> <val>        - Configures settings at runtime");
        sb.AppendLine("  biz pending [confirm|resolve] - Resolve an unchecked payout after a crash");
        sb.AppendLine("  biz catchup                - Shows catch-up backlog and MaxCatchupDays cap");
        sb.AppendLine("  biz help                   - Shows this help");
        sb.AppendLine("<color=#888888>  New automatic AND forced payouts are blocked while an unresolved pending marker exists.</color>");
        Print(sb.ToString());
    }

    private static void Print(string message)
    {
        MelonLogger.Msg(message);
    }

    /// <summary>
    /// F1 follow-up: resolve a leftover pending-payout marker after a crash between
    /// transaction booking and state commit. Only an explicit user choice resolves it.
    /// </summary>
    private void ExecutePending(List<string> args)
    {
        var status = PayoutStateStore.PendingStatus;
        var pending = PayoutStateStore.ReadPendingMarker();

        if (status == PendingStatus.Missing)
        {
            Print("<color=#60f080>No pending payout. Everything settled.</color>");
            return;
        }

        if (status == PendingStatus.Corrupt || pending == null || pending.Day < 0)
        {
            Print("<color=#ff6060>The pending marker for this slot is CORRUPT/unreadable.</color>");
            Print("<color=#ffaa00>New automatic AND forced payouts stay BLOCKED until it is repaired.</color>");

            // A corrupt marker decodes to no readable payload, so confirm/resolve have nothing to
            // act on (the core returns Skipped). Do NOT present them as the fix, and never suggest
            // deleting the marker as "accepting" the payout - clearing it leaves the day unbooked,
            // so the next payout would pay it AGAIN.
            if (args.Exists(a => a.Equals("confirm", StringComparison.OrdinalIgnoreCase))
                || args.Exists(a => a.Equals("resolve", StringComparison.OrdinalIgnoreCase)))
            {
                Print("<color=#ff6060>confirm/resolve cannot act while the marker is unreadable - they need a readable marker. Nothing was written.</color>");
            }

            Print("Repair by hand. FIRST keep a backup of the mod's user-data folder (the marker file AND its .bak, plus the payout-state file AND its .bak):");
            Print("  1. Copy the whole BusinessIncome user-data folder somewhere safe.");
            Print("  2. Fix the marker file's JSON so it parses again, or - ONLY if you verified the payout WAS received - advance the payout-state file past that day.");
            Print("<color=#ffaa00>Do NOT simply delete the marker to \"accept\" it as booked: deleting/clearing it leaves the day unbooked, so the next payout can pay it AGAIN.</color>");
            return;
        }

        if (args.Exists(a => a.Equals("confirm", StringComparison.OrdinalIgnoreCase)))
        {
            ResolveAndReport(PayoutStateStore.ConfirmPending(), "confirm");
        }
        else if (args.Exists(a => a.Equals("resolve", StringComparison.OrdinalIgnoreCase)))
        {
            ResolveAndReport(PayoutStateStore.ResolvePending(), "resolve");
        }
        else
        {
            Print($"<color=#ffaa00>UNCHECKED PAYOUT:</color> day {pending.Day}, +${pending.Amount.ToString("N2", CultureInfo.InvariantCulture)}, {pending.BusinessCount} businesses, started {pending.StartedUtc}.");
            Print("Check the in-game bank app for a 'Business Revenue' entry of that day, then run:");
            Print("  biz pending confirm  - money received, mark day as paid (no second booking)");
            Print("  biz pending resolve  - money missing, clear marker so it pays again");
        }
    }

    private void ResolveAndReport(PayoutResult r, string verb)
    {
        // The settlement core returns PendingConfirmed / PendingResolved even when the marker
        // CLEAR FAILED (the marker is kept and still blocks payouts). Re-read the ACTUAL marker
        // status instead of trusting the outcome/Detail so a failed clear is never reported as a
        // clean resolution. PendingStatus re-reads the engine's in-memory marker state, which the
        // core updates to Valid (kept) on a failed clear and Missing on success.
        bool markerStillPresent = PayoutStateStore.PendingStatus != PendingStatus.Missing;

        switch (r.Outcome)
        {
            case PayoutOutcome.PendingConfirmed:
                if (markerStillPresent)
                    Print("<color=#ffaa00>Payout confirmed received and the payout state was advanced, BUT the pending marker could NOT be cleared and is STILL present. New payouts remain BLOCKED - fix the disk/write issue and run 'biz pending confirm' again. Do NOT delete the marker to \"accept\" it as booked (that would let the day be paid again).</color>");
                else
                    Print("<color=#60f080>Pending payout confirmed received - payout state advanced and the marker was cleared.</color>");
                break;

            case PayoutOutcome.PendingResolved:
                if (markerStillPresent)
                    Print("<color=#ffaa00>Pending payout marked resolved, BUT the marker could NOT be cleared and is STILL present, so the day can NOT be paid again yet. Fix the disk/write issue and run 'biz pending resolve' again; keep a backup before editing files by hand.</color>");
                else
                    Print("<color=#ffaa00>Pending payout resolved - pre-call state restored (when available) and the marker was cleared. The day can be paid again.</color>");
                break;

            case PayoutOutcome.StateNotPersisted:
                Print("<color=#ff6060>The state could not be saved — the marker was KEPT. Nothing was cleared; retry after fixing the disk/write issue.</color>");
                break;

            case PayoutOutcome.Skipped:
                Print("<color=#ffaa00>Nothing resolved: no readable marker. The marker is never auto-cleared.</color>");
                break;

            case PayoutOutcome.NotAuthoritative:
            case PayoutOutcome.NotReady:
                Print($"<color=#ff6060>Cannot {verb}: the runtime is not ready or the active slot is unresolved ({r.Detail}).</color>");
                break;

            default:
                Print($"<color=#ffaa00>Pending {verb} did not complete ({r.Outcome}: {r.Detail}).</color>");
                break;
        }
    }

    /// <summary>
    /// F2 follow-up: show the current catch-up backlog and the configured cap.
    /// </summary>
    private void ExecuteCatchup()
    {
        if (!TryGetElapsedDays(out int elapsedDays))
        {
            Print("<color=#ff6060>Cannot read the in-game day — aborting catch-up report.</color>");
            return;
        }

        var cfg = ModConfig<BusinessIncomeConfig>.Instance;
        var state = PayoutStateStore.GetState();
        long backlog = (long)elapsedDays - state.LastPaidElapsedDay;
        int cap = PayoutScheduler.ClampCap(cfg.MaxCatchupDays);

        Print("<color=#60f080>[Catch-up Status]</color>");
        Print($"  Current day:          {elapsedDays}");
        Print($"  Last paid day:        {state.LastPaidElapsedDay}");
        Print($"  Backlog:              {backlog} day(s)");
        Print($"  MaxCatchupDays:       {cap}");

        if (!PayoutStateStore.IsAuthoritative)
            Print("<color=#ffaa00>  Active slot unresolved — payouts are blocked (fail closed).</color>");
        else if (PayoutStateStore.StateLoad.BlocksMutation)
            Print($"<color=#ff6060>  Payout state is {PayoutStateStore.StateLoad.Status} — catch-up is blocked; no fresh seed.</color>");
        else if (PayoutStateStore.PendingStatus != PendingStatus.Missing)
            Print("<color=#ff6060>  An unresolved pending payout marker exists — catch-up is blocked. Run 'biz pending'.</color>");
        else if (backlog > cap)
            Print($"<color=#ffaa00>  Backlog exceeds cap — the next catch-up pays days {elapsedDays - cap + 1}..{elapsedDays} and skips the rest.</color>");
        else if (backlog > 0)
            Print($"<color=#60f080>  The next day-pass will pay days {state.LastPaidElapsedDay + 1}..{elapsedDays}.</color>");
        else
            Print("<color=#60f080>  Nothing to catch up.</color>");
    }

    private static bool Is(List<string> args, int index, params string[] values)
    {
        if (args.Count <= index) return false;
        string actual = args[index].ToLowerInvariant();
        foreach (var v in values)
        {
            if (actual == v) return true;
        }
        return false;
    }

    private static string Truncate(string val, int maxLen)
    {
        if (string.IsNullOrEmpty(val)) return "";
        return val.Length <= maxLen ? val : val[..(maxLen - 2)] + "..";
    }
}
