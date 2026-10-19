// ---------------------------------------------------------------------------
// FareDomain.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract tests can still read that
// file's exact content.
//
//   Fares/FareLedger.cs            -> section FareLedger.cs
//   Fares/FarePayment.cs           -> section FarePayment.cs
//   Fares/BalanceReadbackPolicy.cs -> section BalanceReadbackPolicy.cs
//   Fares/MeterSettlement.cs       -> section MeterSettlement.cs
//
// ---------------------------------------------------------------------------

namespace TaxiDriver
{
    // ==== TaxiDriver section: FareLedger.cs ====
    using System;


    /// <summary>
    /// Pure, Unity-free fare accrual state machine, extracted from
    /// <c>FareMeter.Tick</c> on 2026-10-02 so the billing rules are unit-testable
    /// without the game (<c>Source/Tests/TaxiDriver.Tests</c>). Behavior-preserving:
    /// all time values are inputs, all Unity/game access stays in FareMeter, and the
    /// returned <see cref="TickOutcome"/> drives the same log lines and payment.
    ///
    /// Rules (unchanged from FareMeter):
    /// <list type="bullet">
    /// <item>Standing is free; only whole moving in-game minutes are billed.</item>
    /// <item>The first moving frame after entering motion does not accrue.</item>
    /// <item>Pauses, clock stops and dead speed handles reset the motion latch and accrue nothing.</item>
    /// <item>Raw frame deltas are capped at <see cref="MaxDeltaSeconds"/> (hitch protection).</item>
    /// <item>Billing is committed before payment so a failed payment is never retried.</item>
    /// </list>
    /// </summary>
    internal sealed class FareLedger
    {
        /// <summary>Hitch cap: a freeze (e.g. 2 s) must never bill more than half a second of motion.</summary>
        public const float MaxDeltaSeconds = 0.5f;

        private double _movingMinutes;
        private int _billedMinutes;
        private bool _wasMoving;
        private bool _armed;
        private bool _paused;

        /// <summary>True while the meter is in a paused (timeScale 0) state.</summary>
        public bool Paused => _paused;

        /// <summary>Accumulated moving in-game minutes (not yet floored).</summary>
        public double MovingMinutes => _movingMinutes;

        /// <summary>Whole minutes already billed; a re-tick can never double-charge below this.</summary>
        public int BilledMinutes => _billedMinutes;

        /// <summary>Motion latch from the previous tick (first moving frame is not counted).</summary>
        public bool WasMoving => _wasMoving;

        /// <summary>True once the first confirmed motion of the ride was seen.</summary>
        public bool Armed => _armed;

        private static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);

        /// <summary>Clears all accrual state for a new ride (FareMeter.Start).</summary>
        public void Reset()
        {
            _movingMinutes = 0;
            _billedMinutes = 0;
            _wasMoving = false;
            _armed = false;
            _paused = false;
        }

        /// <summary>
        /// Advances the accrual state machine by one frame. Mirrors the former
        /// <c>FareMeter.Tick</c> branch order exactly; the caller maps the outcome
        /// to logging and payment.
        /// </summary>
        public TickOutcome Tick(in TickInput input)
        {
            var outcome = new TickOutcome();

            if (input.TimePaused)
            {
                outcome.JustPaused = !_paused;
                _paused = true;
                _wasMoving = false;
                return outcome;
            }

            outcome.JustResumed = _paused;
            _paused = false;

            // Same early-return set as the old code: ride gone, clock unavailable,
            // meter disabled/zero rate, or a dead speed handle are all free and
            // reset the motion latch (the first frame after recovery never accrues).
            if (!input.RideValid || !input.ClockAvailable || !input.ConfigEnabled ||
                input.DollarsPerMinute <= 0 || input.DollarsPerMinute > FareConfigRules.MaximumDollarsPerMinute ||
                input.SpeedReadFailed)
            {
                _wasMoving = false;
                return outcome;
            }

            // Numeric hardening (2026-10-03): reject non-finite tick input BEFORE any
            // state mutation - a poisoned accumulator can never be repaired (the old
            // behavior let NaN pass through Math.Clamp and corrupted the ride total).
            if (!IsFinite(input.RawDeltaSeconds) || !IsFinite(input.ClockRate))
                return outcome;

            float dt = Math.Clamp(input.RawDeltaSeconds, 0f, MaxDeltaSeconds) * input.ClockRate;
            if (!IsFinite(dt))
                return outcome;

            bool moving = !float.IsNaN(input.SpeedKmh) && !float.IsInfinity(input.SpeedKmh) &&
                          input.SpeedKmh > 0f && input.SpeedKmh >= input.MovingThresholdKmh;

            if (moving && !_armed)
            {
                _armed = true;
                outcome.ArmedNow = true;
            }

            if (moving != _wasMoving)
                outcome.MovingChangedTo = moving;

            bool countInterval = moving && _wasMoving;
            _wasMoving = moving;
            if (!countInterval)
                return outcome; // standing is free

            long maxBillableMinutes = FareConfigRules.MaximumFareDollars / (long)input.DollarsPerMinute;
            _movingMinutes = Math.Min(_movingMinutes + (double)dt, maxBillableMinutes);
            outcome.Counted = true;

            // Floor TIME first (rate=2 must not charge $1 after 0.5 s). Cap the
            // lifetime total in minutes before the cast and multiplication so even a
            // corrupt clock-rate value cannot wrap the int billed-minute counter or
            // repeat the payment every frame.
            int wholeMinutes = (int)Math.Floor(_movingMinutes);
            long units = wholeMinutes - (long)_billedMinutes;
            int due = 0;
            if (units > 0)
            {
                long rawDue = units * input.DollarsPerMinute;
                due = (int)Math.Min(rawDue, int.MaxValue);
            }
            if (due > 0)
            {
                // Mark consumed before the caller touches money: a failed/partially
                // applied payment must never be retried every subsequent frame.
                _billedMinutes = wholeMinutes;
                outcome.DueDollars = due;
            }

            outcome.WholeMinutes = wholeMinutes;
            return outcome;
        }

        /// <summary>Inputs for one frame; everything is derived by the caller from Unity/game state.</summary>
        internal readonly struct TickInput
        {
            /// <summary><c>Time.timeScale == 0f</c>.</summary>
            public bool TimePaused { get; init; }

            /// <summary><c>SpikeState.RideActive &amp;&amp; ReferenceEquals(veh, SpikeState.Vehicle)</c>.</summary>
            public bool RideValid { get; init; }

            /// <summary>Raw frame delta in scaled seconds; capped internally.</summary>
            public float RawDeltaSeconds { get; init; }

            /// <summary>True when the game clock provided a valid rate this frame.</summary>
            public bool ClockAvailable { get; init; }

            /// <summary>Game minutes per scaled second (see <see cref="FareClock.TryComputeRate"/>).</summary>
            public float ClockRate { get; init; }

            /// <summary>fare.json <c>Enabled</c>.</summary>
            public bool ConfigEnabled { get; init; }

            /// <summary>fare.json <c>DollarsPerInGameMinute</c>.</summary>
            public int DollarsPerMinute { get; init; }

            /// <summary>True when reading <c>Speed_Kmh</c> threw (dead handle).</summary>
            public bool SpeedReadFailed { get; init; }

            /// <summary>Absolute speed in km/h; ignored when <see cref="SpeedReadFailed"/>.</summary>
            public float SpeedKmh { get; init; }

            /// <summary>fare.json <c>MovingSpeedThresholdKmh</c>.</summary>
            public float MovingThresholdKmh { get; init; }
        }

        /// <summary>What one tick did; the caller turns this into the same log lines as before.</summary>
        internal struct TickOutcome
        {
            /// <summary>True on the first paused frame only.</summary>
            public bool JustPaused { get; set; }

            /// <summary>True on the first frame after a pause.</summary>
            public bool JustResumed { get; set; }

            /// <summary>True on the first confirmed motion of the ride.</summary>
            public bool ArmedNow { get; set; }

            /// <summary>Set when the moving/standing state flipped; the value is the new state.</summary>
            public bool? MovingChangedTo { get; set; }

            /// <summary>True when the interval was counted (moving follow-up frame).</summary>
            public bool Counted { get; set; }

            /// <summary>Whole dollars due now (0 = nothing to charge).</summary>
            public int DueDollars { get; set; }

            /// <summary>Floored moving minutes after this tick.</summary>
            public int WholeMinutes { get; set; }
        }
    }

    /// <summary>
    /// Threshold validation/migration rules from the fare.json loader
    /// (FareMeter.Config). Pure so the invalid/legacy branches stay under test.
    /// </summary>
    internal static class FareConfigRules
    {
        /// <summary>Default whole-dollar rate per fully driven in-game minute.</summary>
        public const int DefaultDollarsPerMinute = 1;

        /// <summary>Bounds the hand-edited rate so fare math and float money APIs stay finite.</summary>
        public const int MaximumDollarsPerMinute = 1_000_000;

        /// <summary>Largest exact whole-dollar amount accepted by the game's float money API.</summary>
        public const int MaximumFareDollars = 16_777_216;

        /// <summary>Repairs zero, negative, and excessively large configured rates.</summary>
        public static int NormalizeDollarsPerMinute(int value, out bool changed)
        {
            changed = value < 1 || value > MaximumDollarsPerMinute;
            return changed ? DefaultDollarsPerMinute : value;
        }

        /// <summary>Default moving threshold in km/h.</summary>
        public const float DefaultThresholdKmh = 3f;

        /// <summary>Values at or below this are the legacy creep threshold (&lt;= 0.5 km/h).</summary>
        public const float LegacyThresholdMaxKmh = 0.5f;

        /// <summary>
        /// True for NaN/Infinity or anything below 0.1 km/h — reset to the default
        /// WITHOUT rewriting the file (the legacy migration below handles and logs
        /// real rewrites).
        /// </summary>
        public static bool IsInvalidThreshold(float value) =>
            float.IsNaN(value) || float.IsInfinity(value) || value < 0.1f;

        /// <summary>True for the legacy creep threshold that counted parking-speed crawl as motion.</summary>
        public static bool IsLegacyThreshold(float value) => value <= LegacyThresholdMaxKmh;

        /// <summary>
        /// Package 8: migration decision (legacy first, invalid second), pure so the
        /// matrix stays under test. Legacy (even below 0.1) and invalid values both
        /// repair to the default; <paramref name="changed"/> tells the caller to
        /// rewrite the file so a second load is migration-free.
        /// </summary>
        public static float Migrate(float value, out bool changed, out bool wasLegacy)
        {
            wasLegacy = IsLegacyThreshold(value);
            if (wasLegacy || IsInvalidThreshold(value))
            {
                changed = true;
                return DefaultThresholdKmh;
            }

            changed = false;
            return value;
        }
    }

    /// <summary>
    /// Pure game-clock conversion (TimeManager CycleDuration/TimeSpeedMultiplier →
    /// in-game minutes per scaled second). Validation mirrors the former inline
    /// checks; a false return makes the caller throw/log exactly as before.
    /// </summary>
    internal static class FareClock
    {
        /// <summary>
        /// <c>1440 / (cycle * 60) * speed</c>; false when either input is
        /// NaN/Infinity/&lt;= 0.
        /// </summary>
        public static bool TryComputeRate(float cycleRealMinutesPerDay, float speedMultiplier, out float rate)
        {
            rate = 0f;
            if (!IsFinitePositive(cycleRealMinutesPerDay) || !IsFinitePositive(speedMultiplier))
                return false;

            rate = 1440f / (cycleRealMinutesPerDay * 60f) * speedMultiplier;
            if (!IsFinitePositive(rate))
            {
                // Numeric hardening (2026-10-03): finite positive inputs can still
                // overflow the computed rate - a useless result is rejected, not used.
                rate = 0f;
                return false;
            }

            return true;
        }

        private static bool IsFinitePositive(float value) =>
            !float.IsNaN(value) && !float.IsInfinity(value) && value > 0f;
    }
    // ==== end TaxiDriver section: FareLedger.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: FarePayment.cs ====
    using System;


    /// <summary>
    /// Production settlement seam used by <see cref="FareMeter"/>. The money calls
    /// remain behind a small Unity-free wallet interface so cash/bank splitting,
    /// payment status, exception handling and no-retry behavior are unit-testable.
    ///
    /// Preserved semantics (characterized in FarePaymentCharacterizationTests):
    /// <list type="bullet">
    /// <item>Billing before payment, no retry: the ledger consumes the minutes
    /// first (FareLedger.cs:109-119); one Execute call makes at most one attempt
    /// per payment part.</item>
    /// <item>A clean wallet return is NOT a payment confirmation: S1API.Money
    /// silently no-ops when MoneyManager.Instance is null (Money.cs:43,57). The
    /// outcome reports "call returned", never "money moved".</item>
    /// <item>A thrown wallet call is NOT "uncharged": its outcome is UNKNOWN
    /// (S1API fires OnBalanceChanged after the manager call, Money.cs:44,58; the
    /// native MoneyManager methods are IL2CPP stubs, so mutate-then-throw is not
    /// disproven). A thrown part is never auto-classified and never retried.</item>
    /// <item>Since the TD-01/TD-02/TD-04 fixes (2026-10-03): a failed cash read
    /// aborts WITHOUT any money call (fail closed, the amount is unpaid), only
    /// clean-returned parts are counted as payment calls returned (not balance-
    /// verified), a thrown part is UNKNOWN and never retried, and a client makes
    /// no payment call (calculated fare only). The outer catch boundary still covers the wallet calls AND the
    /// caller's record/log hooks, exactly like the original try.</item>
    /// </list>
    /// </summary>
    internal static class FarePayment
    {
        /// <summary>
        /// Cash/bank split of FareMeter.cs:258-259: clamp cash at 0, cap at the due
        /// amount, truncate to whole dollars. Production (IL2CPPMELON, the mod build)
        /// runs the EXACT original expression, so the special-value/Unity-Math quirks
        /// cannot change; the test build (no IL2CPPMELON) runs a comparison-operator
        /// mirror whose semantics are pinned by the P09 characterization. Mirror
        /// equivalence to Unity is therefore not load-bearing for production
        /// (seam-mapping.md section 4).
        /// </summary>
        internal static int SplitCash(float cash, int dollars)
        {
#if IL2CPPMELON
            return (int)UnityEngine.Mathf.Min(UnityEngine.Mathf.Max(cash, 0f), dollars);
#else
            float clamped = cash > 0f ? cash : 0f; // mirrors Mathf.Max(cash, 0f)
            float capped = clamped < dollars ? clamped : dollars; // mirrors Mathf.Min(x, dollars)
            return (int)capped;
#endif
        }

        /// <summary>
        /// One settlement attempt for <paramref name="dollars"/> whole dollars: host
        /// gate, cash read with its inner guard, split, cash call, bank call, counter,
        /// summary log - one outer catch around all of it (same boundary as the
        /// original FareMeter.Charge). The outcome reports returned/unknown/unpaid
        /// parts instead of a blanket skip: the hooks
        /// run at the exact positions of the original statements, and a thrown part is
        /// never classified as "not charged" and never retried.
        /// </summary>
        internal static PaymentOutcome Execute(
            int dollars,
            IGameWallet wallet,
            bool isHost,
            Action<int>? recordCharge = null,
            Action? clientWarnOnce = null,
            Action? chargeSummaryLog = null)
        {
            var outcome = new PaymentOutcome
            {
                DueDollars = dollars,
                CalculatedDollars = dollars,
            };

            try
            {
                if (!isHost)
                {
                    // FareMeter.cs:238-242 (the warn-once state stays in the adapter).
                    clientWarnOnce?.Invoke();
                    // TD-04: a client makes no payment call; only the calculated fare
                    // may be shown without host authority.
                    outcome.NotHost = true;
                    return outcome;
                }

                float cash;
                try
                {
                    cash = wallet.GetCashBalance(); // FareMeter.cs:251
                }
                catch (Exception ex)
                {
                    // TD-02: fail closed - an unreadable balance selects NO payment
                    // method; nothing is touched and the amount stays unpaid.
                    outcome.CashReadFailed = true;
                    outcome.CashReadError = $"{ex.GetType().Name}: {ex.Message}";
                    outcome.UnpaidDollars = dollars;
                    return outcome;
                }

                int fromCash = SplitCash(cash, dollars); // FareMeter.cs:258
                int fromBank = dollars - fromCash; // FareMeter.cs:259
                outcome.CashPart = fromCash;
                outcome.BankPart = fromBank;

                if (fromCash > 0)
                {
                    outcome.CashStatus = PaymentPartStatus.Attempted;
                    wallet.ChangeCashBalance(-fromCash, true, false); // FareMeter.cs:262
                    outcome.CashStatus = PaymentPartStatus.CallReturned;
                }

                if (fromBank > 0)
                {
                    outcome.BankStatus = PaymentPartStatus.Attempted;
                    wallet.CreateOnlineTransaction("Taxi fare", -fromBank, 1, "Taxi"); // FareMeter.cs:265
                    outcome.BankStatus = PaymentPartStatus.CallReturned;
                }

                recordCharge?.Invoke(dollars); // FareMeter.cs:267 (full amount: every part returned)
                outcome.CountedDollars = dollars;
                outcome.ChargeRecorded = true;
                chargeSummaryLog?.Invoke(); // FareMeter.cs:270-276
            }
            catch (Exception ex)
            {
                outcome.Caught = true;
                outcome.CaughtType = ex.GetType().Name;
                outcome.CaughtMessage = ex.Message;

                // A part whose call was in flight when the exception escaped did not
                // report back: outcome UNKNOWN (mutate-then-throw is not disproven),
                // never "not charged".
                if (outcome.CashStatus == PaymentPartStatus.Attempted)
                    outcome.CashStatus = PaymentPartStatus.CallThrewUnknown;
                if (outcome.BankStatus == PaymentPartStatus.Attempted)
                    outcome.BankStatus = PaymentPartStatus.CallThrewUnknown;

                if (!outcome.ChargeRecorded)
                {
                    // TD-01 partial accounting: record only clean-returned parts as
                    // API returns (never balance-confirmed); a thrown part is UNKNOWN,
                    // a never-attempted part unpaid.
                    int returned =
                        (outcome.CashStatus == PaymentPartStatus.CallReturned ? outcome.CashPart : 0) +
                        (outcome.BankStatus == PaymentPartStatus.CallReturned ? outcome.BankPart : 0);
                    outcome.CountedDollars = returned;
                    if (returned > 0)
                        recordCharge?.Invoke(returned);
                    outcome.UnknownDollars =
                        (outcome.CashStatus == PaymentPartStatus.CallThrewUnknown ? outcome.CashPart : 0) +
                        (outcome.BankStatus == PaymentPartStatus.CallThrewUnknown ? outcome.BankPart : 0);
                    outcome.UnpaidDollars = dollars - returned - outcome.UnknownDollars;
                }
            }

            return outcome;
        }
    }

    /// <summary>Per-part state of one settlement attempt. There is deliberately no
    /// "Confirmed" and no "Unpaid" state for a thrown call: a clean return is only
    /// "call returned", a throw is "outcome unknown".</summary>
    internal enum PaymentPartStatus
    {
        /// <summary>No call was made (part not due, or the flow stopped earlier).</summary>
        NotAttempted,

        /// <summary>The call is in flight (transient, only observed mid-Execute).</summary>
        Attempted,

        /// <summary>The call returned cleanly. NOT proof that money moved.</summary>
        CallReturned,

        /// <summary>The call threw. NOT proof that money did not move; never retried.</summary>
        CallThrewUnknown,
    }

    /// <summary>
    /// The three money effects FareMeter uses (S1API.Money.Money signatures
    /// mirrored 1:1, Money.cs:41/54/74). The interface makes NO stronger success
    /// promise than S1API: a clean return means "call returned", not "money
    /// moved", and a thrown call leaves the outcome unknown.
    /// </summary>
    internal interface IGameWallet
    {
        /// <summary>Mirrors S1API Money.GetCashBalance (Money.cs:74-77).</summary>
        float GetCashBalance();

        /// <summary>Mirrors S1API Money.ChangeCashBalance (Money.cs:41-45).</summary>
        void ChangeCashBalance(float delta, bool visualizeChange, bool playCashSound);

        /// <summary>Mirrors S1API Money.CreateOnlineTransaction (Money.cs:54-59).</summary>
        void CreateOnlineTransaction(string transactionName, float unitAmount, float quantity, string transactionNote);
    }

    /// <summary>
    /// Observable result of one <see cref="FarePayment.Execute"/> attempt. Records
    /// what happened (calls returned or threw, parts attempted, counter hook value)
    /// and never classifies a thrown call as "uncharged" or a clean call as
    /// "confirmed".
    /// </summary>
    internal sealed class PaymentOutcome
    {
        /// <summary>Dollars requested for this attempt.</summary>
        public int DueDollars;

        /// <summary>Calculated fare shown to the player; separate from payment-call outcomes.</summary>
        public int CalculatedDollars;

        /// <summary>Split part attempted via ChangeCashBalance.</summary>
        public int CashPart;

        /// <summary>Split part attempted via CreateOnlineTransaction.</summary>
        public int BankPart;

        /// <summary>Payment-call-returned dollars passed to the record callback; not proof of balance movement.</summary>
        public int CountedDollars;

        /// <summary>The client path was taken (no wallet call).</summary>
        public bool NotHost;

        /// <summary>GetCashBalance threw; settlement stopped before any money call and the due amount remains unpaid.</summary>
        public bool CashReadFailed;

        /// <summary>The outer catch engaged; a wallet call or hook threw.</summary>
        public bool Caught;

        /// <summary>Exception type name for the caller's log (FareMeter.cs:280 shape).</summary>
        public string? CaughtType;

        /// <summary>Exception message for the caller's log.</summary>
        public string? CaughtMessage;

        /// <summary>Exception of a failed cash read ("Type: Message") for the caller's log.</summary>
        public string? CashReadError;

        /// <summary>Parts whose money call threw: outcome UNKNOWN, never "not charged", never retried.</summary>
        public int UnknownDollars;

        /// <summary>Parts never attempted (fail-closed abort): not charged.</summary>
        public int UnpaidDollars;

        /// <summary>True once the payment-call-returned amount was handed to recordCharge (guards double counting).</summary>
        public bool ChargeRecorded;

        /// <summary>State of the cash part (see <see cref="PaymentPartStatus"/>).</summary>
        public PaymentPartStatus CashStatus;

        /// <summary>State of the bank part (see <see cref="PaymentPartStatus"/>).</summary>
        public PaymentPartStatus BankStatus;
    }

    /// <summary>
    /// Per-ride summary of settlement outcomes. A clean void API return is tracked
    /// separately from a thrown/unknown call and is never described as verified money
    /// movement. Totals are bounded by the fare ledger's lifetime cap.
    /// </summary>
    internal sealed class FarePaymentStatusLedger
    {
        internal int ReturnedDollars { get; private set; }
        internal int UnknownDollars { get; private set; }
        internal int UnpaidDollars { get; private set; }
        internal bool ClientOnly { get; private set; }

        internal void Reset()
        {
            ReturnedDollars = 0;
            UnknownDollars = 0;
            UnpaidDollars = 0;
            ClientOnly = false;
        }

        /// <summary>Records the amount whose payment calls returned cleanly, without claiming a balance change.</summary>
        internal void RecordReturned(int dollars) =>
            ReturnedDollars = SaturatingAdd(ReturnedDollars, dollars);

        /// <summary>Records one completed payment outcome, including returned parts for unit-test callers.</summary>
        internal void Record(PaymentOutcome outcome)
        {
            RecordReturned(outcome.CountedDollars);
            RecordNonReturned(outcome);
        }

        /// <summary>Records only outcome metadata when returned parts were already recorded by the production callback.</summary>
        internal void RecordNonReturned(PaymentOutcome outcome)
        {
            UnknownDollars = SaturatingAdd(UnknownDollars, outcome.UnknownDollars);
            UnpaidDollars = SaturatingAdd(UnpaidDollars, outcome.UnpaidDollars);
            ClientOnly |= outcome.NotHost;
        }

        internal void RecordUnknown(int dollars) =>
            UnknownDollars = SaturatingAdd(UnknownDollars, dollars);

        internal void RecordUnpaid(int dollars) =>
            UnpaidDollars = SaturatingAdd(UnpaidDollars, dollars);

        internal string SummaryText()
        {
            if (ClientOnly && ReturnedDollars == 0 && UnknownDollars == 0 && UnpaidDollars == 0)
                return "no payment attempted on this client; host authority required";

            string settlement =
                $"payment calls returned ${ReturnedDollars} (balance movement unverified), " +
                $"unknown ${UnknownDollars}, unpaid ${UnpaidDollars}";
            return ClientOnly
                ? $"payment skipped on this client (host authority); {settlement}"
                : settlement;
        }

        private static int SaturatingAdd(int current, int amount)
        {
            if (amount <= 0)
                return current;
            return (int)Math.Min((long)current + amount, FareConfigRules.MaximumFareDollars);
        }
    }
    // ==== end TaxiDriver section: FarePayment.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: BalanceReadbackPolicy.cs ====
    using System;
    using System.Globalization;


    /// <summary>
    /// Formats the observed balance movement around one settlement attempt. The
    /// readback is log evidence only: it compares net worth before and after the
    /// money calls and never proves causation (other game systems can move money
    /// concurrently).
    /// </summary>
    internal static class BalanceReadbackPolicy
    {
        /// <summary>Whole-dollar fares, plus float noise from S1API reads.</summary>
        internal const float ToleranceDollars = 0.51f;

        /// <summary>Describes before/after net worth and the observed delta.</summary>
        internal static string Describe(float netWorthBefore, float netWorthAfter, int chargedDollars)
        {
            float observed = netWorthBefore - netWorthAfter;
            string verdict = Math.Abs(observed - chargedDollars) <= ToleranceDollars
                ? "movement matches the charged amount"
                : "movement does not match the charged amount (concurrent money changes possible; verify manually)";
            return $"net worth {Num(netWorthBefore)} -> {Num(netWorthAfter)} (observed {Num(observed)}, charged ${chargedDollars}) — {verdict}";
        }

        private static string Num(float value) => value.ToString("0.##", CultureInfo.InvariantCulture);
    }
    // ==== end TaxiDriver section: BalanceReadbackPolicy.cs ====
}

namespace TaxiDriver
{
    // ==== TaxiDriver section: MeterSettlement.cs ====
    using System;


    /// <summary>Consumes the meter end before IO; reporting failure never resubmits fares.</summary>
    internal static class MeterSettlement
    {
        internal static void Stop(ref bool running, Action report)
        {
            if (!running) return;
            running = false;
            report();
        }
    }
    // ==== end TaxiDriver section: MeterSettlement.cs ====
}
