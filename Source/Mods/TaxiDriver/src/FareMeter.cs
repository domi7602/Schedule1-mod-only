using System;
using Il2CppScheduleOne.UI;
using S1API.Money;
using S1Mods.Shared;
using UnityEngine;
using GameClock = Il2CppScheduleOne.GameTime.TimeManager;

namespace TaxiDriver;

/// <summary>
/// The taxi's meter (Dominik, 2026-09-29): "ein kostenloses Taxi ist leider nicht
/// Realität ... wenn das Auto fährt, jede Sekunde −1 cash, wenn 0 km/h wird nichts
/// abgezogen — der Trigger beginnt wenn die Destination ausgewählt ist".
///
/// Standard time base: at a 24-real-minute game day, 1 real second = 1 in-game minute.
/// Motion intervals are converted using TimeManager's CycleDuration and
/// TimeSpeedMultiplier. Pauses, sleep and the end-of-day clock stop are free;
/// skipped clock minutes are never retroactively charged.
/// The meter therefore charges whole dollars per FULL moving in-game minute
/// (= per full real second of motion at normal speed; paused Unity time is free)
/// — no cents, because the game has none
/// (Dominik: "$1 da es keine Cent-Beträge gibt").
///
/// Rules:
/// <list type="bullet">
/// <item>Start: the moment the ride drive is dispatched (destination picked).</item>
/// <item>0 km/h (standing, jams, waiting for the exit) is FREE.</item>
/// <item>Payment: cash first (clamped at 0), the remainder goes to the bank
/// account which MAY go negative (Dominik: "Konto darf ins Minus").</item>
/// <item>Ride end: one HUD notification with the total.</item>
/// <item>Multiplayer: money is only touched host/singleplayer-side
/// (<see cref="NetworkGuard.IsHostOrSingleplayer"/>, fail-closed).</item>
/// </list>
///
/// Config: <c>UserData/TaxiDriver/fare.json</c> (SafeStorage, hand-editable):
/// <c>Enabled</c>, <c>DollarsPerInGameMinute</c>, <c>MovingSpeedThresholdKmh</c>.
/// </summary>
internal static class FareMeter
{
    /// <summary>Hand-editable fare rules (public properties: System.Text.Json round-trip).</summary>
    public sealed class FareConfig
    {
        /// <summary>Master switch — false = free rides (debugging).</summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Whole dollars per full moving in-game minute (= per full real second of
        /// motion; 1 real s = 1 in-game min at `CycleDuration` = 24 real min/day).
        /// </summary>
        public int DollarsPerInGameMinute { get; set; } = 1;

        /// <summary>Below this speed the car counts as standing (meter pauses).</summary>
        public float MovingSpeedThresholdKmh { get; set; } = 3f;
    }

    private static FareConfig? _config;
    private static bool _configLoaded;
    private static bool _mpWarned;

    private static bool _active;
    private static readonly FarePaymentStatusLedger _paymentStatus = new();

    /// <summary>
    /// Total fare calculated for the current/last ride. This is the fare shown in
    /// TaxiApp; Money API calls are tracked separately and are not balance-verified.
    /// </summary>
    private static int _calculatedTotal;
    private static float _lastTickAt;
    private static int _lastTickFrame = -1;
    private static int _nextChargeLogAt = 10;
    private static float _lastClockRate = -1f;
    private static bool _clockUnavailableWarned;
    private static bool _clockStopped;

    // Pure accrual state machine (Unity-free; unit-tested in Source/Tests/TaxiDriver.Tests).
    private static readonly FareLedger _ledger = new();

    /// <summary>Fare due for the current/last ride (dollars, whole).</summary>
    internal static int CalculatedTotal => _calculatedTotal;

    /// <summary>Amount whose S1API payment calls returned; not proof that balances changed.</summary>
    internal static int PaymentCallsReturnedTotal => _paymentStatus.ReturnedDollars;

    /// <summary>True while a ride is metered.</summary>
    internal static bool Running => _active;

    /// <summary>The sidecar path (also logged on first load).</summary>
    internal static string FilePath => SafeStorage.GetUserDataPath("TaxiDriver", "fare.json");

    private static FareConfig Config
    {
        get
        {
            if (_configLoaded)
                return _config!;

            _configLoaded = true;
            _config = SafeStorage.LoadSafe(FilePath, new FareConfig(), Mod.Log);
            if (!System.IO.File.Exists(FilePath))
            {
                SafeStorage.SaveAtomic(FilePath, _config, Mod.Log);
                Mod.Log.Info(
                    $"[meter] first run — default fare written to {FilePath} " +
                    $"(${_config!.DollarsPerInGameMinute}/In-Game-min moving, {(_config.Enabled ? "enabled" : "disabled")}).");
            }

            // Keep configuration inputs in the meter's safe numeric range. The
            // explicit Enabled switch is the supported way to make rides free.
            int normalizedRate = FareConfigRules.NormalizeDollarsPerMinute(
                _config.DollarsPerInGameMinute, out bool rateChanged);
            if (rateChanged)
            {
                Mod.Log.Warn(
                    $"[meter] invalid DollarsPerInGameMinute={_config.DollarsPerInGameMinute}; " +
                    $"reset to ${FareConfigRules.DefaultDollarsPerMinute} (valid range 1..{FareConfigRules.MaximumDollarsPerMinute}).");
                _config.DollarsPerInGameMinute = normalizedRate;
            }

            // Package 8: legacy FIRST (it carries the rewrite), invalid second —
            // both persist, so a second load is migration-free. The 0.5 km/h
            // legacy threshold counted physics creep as motion (the 2026-10-01
            // test ride billed $9 for ~2 m of crawl).
            float migrated = FareConfigRules.Migrate(
                _config.MovingSpeedThresholdKmh, out bool thresholdChanged, out bool wasLegacy);
            if (thresholdChanged)
            {
                Mod.Log.Warn(
                    wasLegacy
                        ? $"[meter] legacy threshold migrated: {_config.MovingSpeedThresholdKmh:0.###} -> 3 km/h (creep no longer billed); {FilePath} updated."
                        : $"[meter] invalid threshold {_config.MovingSpeedThresholdKmh:0.###} reset to 3 km/h; {FilePath} updated.");
                _config.MovingSpeedThresholdKmh = migrated;
            }
            if (rateChanged || thresholdChanged)
                SafeStorage.SaveAtomic(FilePath, _config, Mod.Log);

            TaxiLog.Verbose($"[meter] loaded {FilePath}: Enabled={_config.Enabled}, DollarsPerInGameMinute={_config.DollarsPerInGameMinute}, MovingSpeedThresholdKmh={_config.MovingSpeedThresholdKmh:0.###}.");
            if (_config.DollarsPerInGameMinute != FareConfigRules.DefaultDollarsPerMinute)
                Mod.Log.Warn("[meter] fare.json differs from requested $1 rate; keeping the explicit config. Set DollarsPerInGameMinute=1 for $1 per full moving second at normal speed.");
            return _config;
        }
    }

    /// <summary>Starts the meter (destination picked → the ride drives). Idempotent.</summary>
    internal static void Start()
    {
        if (_active)
            return;

        // Double guard (Dominik 2026-09-29: "Taxi = LandVehicle — zählt der Zähler
        // auch bei MEINEN Autos?"): the meter is bound to the RIDE state, never to
        // "a LandVehicle exists". No active passenger ride = no meter, period.
        if (!SpikeState.RideActive)
        {
            Mod.Log.Warn("[meter] Start() refused — no active passenger ride (the meter only runs for taxi rides, never for your own cars).");
            return;
        }
        if (!SpikeCommands.HasAuthority() || !SpikeCommands.IsOwnedTaxi(SpikeState.Vehicle))
        {
            Mod.Log.Warn("[meter] Start() refused — the active vehicle is not a live host-owned TaxiDriver taxi.");
            return;
        }

        FareConfig cfg = Config;
        _active = true;
        _ledger.Reset();
        _paymentStatus.Reset();
        _calculatedTotal = 0;
        _lastTickAt = Time.time;
        _lastTickFrame = -1;
        _nextChargeLogAt = 10;
        _lastClockRate = -1f;
        _clockUnavailableWarned = false;
        _clockStopped = false;

        if (!cfg.Enabled)
        {
            Mod.Log.Info("[meter] ride started — meter OFF via fare.json (free ride).");
            return;
        }

        Mod.Log.Info(
            $"[meter] meter started: ${cfg.DollarsPerInGameMinute} per FULL moving in-game minute " +
            $"(= per real second in motion at normal speed; 0 km/h is free) — cash first, bank may go negative.");
        TaxiLog.Verbose("[meter] clock=TimeManager CycleDuration/TimeSpeedMultiplier; 24 min/day at speed 1 = 1 game min per real second. Pause/sleep/clock-stop free; no time-skip catch-up; one tick per frame.");
    }

    /// <summary>
    /// Per-frame tick while the ride runs (from <see cref="SpikeRunner.TickRide"/>):
    /// accumulates MOTION time only and charges whole dollars per full unit.
    /// </summary>
    internal static void Tick(Il2CppScheduleOne.Vehicles.LandVehicle veh)
    {
        // Double guard: only THE taxi (SpikeState.Vehicle) during an active
        // passenger ride is metered — your own cars are never charged.
        if (!_active || veh == null)
            return;

        if (_lastTickFrame == Time.frameCount)
            return; // protect against a second caller/hook within the same frame
        _lastTickFrame = Time.frameCount;
        float now = Time.time;
        float rawDelta = now - _lastTickAt;
        _lastTickAt = now; // always consume pauses/standing; never catch up on resume

        if (Time.timeScale == 0f)
        {
            FareLedger.TickOutcome pausedOutcome = _ledger.Tick(new FareLedger.TickInput { TimePaused = true });
            if (pausedOutcome.JustPaused)
                TaxiLog.Verbose("[meter] paused: no fare accrues.");
            return;
        }

        bool rideValid = SpikeState.RideActive && ReferenceEquals(veh, SpikeState.Vehicle) &&
                         SpikeCommands.HasAuthority() && SpikeCommands.IsOwnedTaxi(veh);
        bool clockAvailable = false;
        float clockRate = 0f;
        if (rideValid)
            clockAvailable = TryGetGameMinutesPerSecond(out clockRate);

        FareConfig cfg = Config;
        bool speedReadFailed = false;
        float speed = 0f;
        if (rideValid && clockAvailable && cfg.Enabled && cfg.DollarsPerInGameMinute > 0)
        {
            try
            {
                speed = Mathf.Abs(veh.Speed_Kmh);
            }
            catch (Exception)
            {
                speedReadFailed = true; // dead handle — no motion, no charge
            }
        }

        FareLedger.TickOutcome outcome = _ledger.Tick(new FareLedger.TickInput
        {
            RideValid = rideValid,
            RawDeltaSeconds = rawDelta,
            ClockAvailable = clockAvailable,
            ClockRate = clockRate,
            ConfigEnabled = cfg.Enabled,
            DollarsPerMinute = cfg.DollarsPerInGameMinute,
            SpeedReadFailed = speedReadFailed,
            SpeedKmh = speed,
            MovingThresholdKmh = cfg.MovingSpeedThresholdKmh,
        });

        if (outcome.JustResumed)
            TaxiLog.Verbose("[meter] resumed: paused time discarded.");
        if (outcome.ArmedNow)
            TaxiLog.Verbose(
                $"[meter] armed — first confirmed motion above {cfg.MovingSpeedThresholdKmh:0.###} km/h; the time before this moment is discarded.");
        if (outcome.MovingChangedTo is bool movingNow)
            TaxiLog.Verbose($"[meter] {(movingNow ? "moving" : "standing (FREE)")}: speed={speed:0.###} km/h, accumulated={_ledger.MovingMinutes:0.###} moving in-game minutes, paymentCallsReturned=${_paymentStatus.ReturnedDollars}, frame={Time.frameCount}.");
        if (outcome.DueDollars > 0)
            Charge(outcome.DueDollars);
    }

    /// <summary>Submits whole-dollar payment calls: cash first (never below 0), then the bank.</summary>
    /// <remarks>
    /// Records the fare before calling the authority and wallet APIs, then keeps
    /// returned, unknown and unpaid amounts separate. A clean return from a void
    /// Money API is not proof that a balance changed; consumed amounts are never
    /// retried automatically.
    /// </remarks>
    private static void Charge(int dollars)
    {
        if (dollars <= 0)
            return;

        _calculatedTotal = (int)Math.Min(
            (long)_calculatedTotal + dollars,
            FareConfigRules.MaximumFareDollars);

        bool isHost;
        try
        {
            isHost = NetworkGuard.IsHostOrSingleplayer();
        }
        catch (Exception ex)
        {
            _paymentStatus.RecordUnpaid(dollars);
            Mod.Log.Warn($"[meter] payment was not attempted for ${dollars}: host authority check failed ({ex.GetType().Name}: {ex.Message}); amount stays unpaid and will not be retried.");
            return;
        }

        int returnedBefore = _paymentStatus.ReturnedDollars;
        PaymentOutcome outcome;
        try
        {
            outcome = FarePayment.Execute(
                dollars,
                Wallet,
                isHost,
                recordCharge: _paymentStatus.RecordReturned,
                clientWarnOnce: WarnClientOnce,
                chargeSummaryLog: LogChargeSummary);
        }
        catch (Exception ex)
        {
            // A wallet/settlement exception escaping the seam cannot prove that
            // none of the money operations took effect. Consume it as UNKNOWN.
            int returnedThisAttempt = _paymentStatus.ReturnedDollars - returnedBefore;
            int unknown = Math.Max(0, dollars - returnedThisAttempt);
            _paymentStatus.RecordUnknown(unknown);
            Mod.Log.Warn($"[meter] payment result is unknown for ${dollars} ({ex.GetType().Name}: {ex.Message}); the fare was consumed and no retry will be made.");
            return;
        }

        _paymentStatus.RecordNonReturned(outcome);

        if (outcome.CashReadFailed)
        {
            // TD-02 (fixed 2026-10-03): fail closed - explicit report, nothing touched.
            Mod.Log.Warn(
                $"[meter] charging ${dollars} aborted — cash balance unreadable ({outcome.CashReadError}); " +
                $"no money call was made; ${outcome.UnpaidDollars} remains unpaid.");
        }
        else if (outcome.Caught)
        {
            if (outcome.UnknownDollars > 0 || outcome.UnpaidDollars > 0)
            {
                // A clean void-method return is only "returned", never proof of
                // changed balances. A thrown part is UNKNOWN and is never retried.
                Mod.Log.Warn(
                    $"[meter] settlement for ${dollars}: payment calls returned for ${outcome.CountedDollars} (balance movement unverified), " +
                    $"unknown ${outcome.UnknownDollars} (a money call threw; outcome unknown, never retried), " +
                    $"unpaid ${outcome.UnpaidDollars} ({outcome.CaughtType}: {outcome.CaughtMessage}).");
            }
            else
            {
                Mod.Log.Warn(
                    $"[meter] payment calls returned for ${outcome.CountedDollars} (balance movement unverified); a post-payment step failed ({outcome.CaughtType}: {outcome.CaughtMessage}).");
            }
        }
    }

    /// <summary>Warns once when this process does not have host authority.</summary>
    private static void WarnClientOnce()
    {
        if (!_mpWarned)
        {
            _mpWarned = true;
            Mod.Log.Warn("[meter] multiplayer client — fare is calculated here, but no payment is attempted on this client (host authority).");
        }
    }

    /// <summary>
    /// Logs every $10 of returned payment API calls. Split values are the exact
    /// payload parts dispatched to S1API, not verified balance changes.
    /// </summary>
    private static void LogChargeSummary()
    {
        if (_paymentStatus.ReturnedDollars >= _nextChargeLogAt)
        {
            _nextChargeLogAt = (_paymentStatus.ReturnedDollars / 10 + 1) * 10;
            int fromCash = Wallet.LastCashPart;
            int fromBank = Wallet.LastBankPart;
            TaxiLog.Verbose(
                $"[meter] payment calls returned for ${_paymentStatus.ReturnedDollars} so far (balance movement unverified) " +
                $"({_ledger.MovingMinutes:0.###} moving in-game minutes; rate=${Config.DollarsPerInGameMinute}/full minute, clock=TimeManager; last API payload: ${fromCash} cash / ${fromBank} bank, frame={Time.frameCount}).");
        }
    }

    private static readonly S1ApiWallet Wallet = new();

    /// <summary>
    /// Production <see cref="IGameWallet"/>: forwards 1:1 to S1API.Money.Money
    /// (same arguments as the original direct calls) and records the dispatched
    /// split parts from the call arguments so <see cref="LogChargeSummary"/> can
    /// print the same values as before the extraction.
    /// </summary>
    private sealed class S1ApiWallet : IGameWallet
    {
        /// <summary>Cash part (fromCash) of the current settlement; 0 when no call ran.</summary>
        internal int LastCashPart;

        /// <summary>Bank part (fromBank) of the current settlement; 0 when no call ran.</summary>
        internal int LastBankPart;

        public float GetCashBalance()
        {
            LastCashPart = 0;
            LastBankPart = 0;
            return Money.GetCashBalance();
        }

        public void ChangeCashBalance(float delta, bool visualizeChange, bool playCashSound)
        {
            LastCashPart = (int)-delta;
            Money.ChangeCashBalance(delta, visualizeChange, playCashSound);
        }

        public void CreateOnlineTransaction(string transactionName, float unitAmount, float quantity, string transactionNote)
        {
            LastBankPart = (int)-unitAmount;
            Money.CreateOnlineTransaction(transactionName, unitAmount, quantity, transactionNote);
        }
    }

    private static bool TryGetGameMinutesPerSecond(out float rate)
    {
        rate = 0f;
        try
        {
            GameClock? clock = GameClock.Instance;
            if (clock == null || clock.Pointer == IntPtr.Zero || clock.WasCollected)
                throw new InvalidOperationException("no live TimeManager");
            bool stopped = S1API.GameTime.TimeManager.SleepInProgress || clock.IsEndOfDay || clock.TimeSpeedMultiplier <= 0f;
            if (stopped)
            {
                if (!_clockStopped)
                    TaxiLog.Verbose("[meter] TimeManager stopped/sleeping: FREE, no skipped-time billing.");
                _clockStopped = true;
                return false;
            }
            if (_clockStopped)
                TaxiLog.Verbose("[meter] TimeManager resumed: skipped time discarded.");
            _clockStopped = false;

            float cycle = GameClock.CycleDuration; // real minutes per game day
            float speed = clock.TimeSpeedMultiplier;
            if (!FareClock.TryComputeRate(cycle, speed, out rate))
                throw new InvalidOperationException("invalid TimeManager cycle/speed");
            if (Mathf.Abs(rate - _lastClockRate) > 0.001f)
            {
                _lastClockRate = rate;
                TaxiLog.Verbose($"[meter] TimeManager: CycleDuration={cycle:0.###} real min/day, TimeSpeedMultiplier={speed:0.###}, rate={rate:0.###} game min/scaled second, HHMM={clock.CurrentTime}.");
            }
            _clockUnavailableWarned = false;
            return true;
        }
        catch (Exception ex)
        {
            if (!_clockUnavailableWarned)
                Mod.Log.Warn($"[meter] game clock unavailable ({ex.Message}); FREE until clock recovers.");
            _clockUnavailableWarned = true;
            return false;
        }
    }

    /// <summary>Stops the meter and reports the total (arrival, exit, STOP). Idempotent.</summary>
    internal static void Stop(string reason)
    {
        if (!_active)
            return;

        _active = false;
        int calculated = _calculatedTotal;
        int minutes = (int)_ledger.MovingMinutes;
        FareConfig cfg = Config;

        string paymentSummary = _paymentStatus.SummaryText();
        Mod.Log.Info(
            $"[meter] ride ended ({reason}) — fare ${calculated} calculated; {paymentSummary}; " +
            $"{minutes} moving in-game minute(s).");

        string fareText =
            $"Fare ${calculated} calculated — {_paymentStatus.SummaryText()}; {minutes} moving in-game min (${cfg.DollarsPerInGameMinute}/min).";

        if (!cfg.Enabled || calculated <= 0)
            return;

        try
        {
            NotificationsManager? notifMgr = NotificationsManager.Instance;
            if (notifMgr != null && (UnityEngine.Object)notifMgr != null)
            {
                notifMgr.SendNotification(
                    "Taxi",
                    fareText,
                    TaxiIcon.Get(),
                    5f,
                    true);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"[meter] fare notification failed: {ex.Message}");
        }
    }

    /// <summary><c>taxi fare</c> — config + live meter state into the log.</summary>
    internal static void PrintStatus()
    {
        FareConfig cfg = Config;
        SpikeCommands.Print(
            $"[meter] {(cfg.Enabled ? "ON" : "OFF")} — ${cfg.DollarsPerInGameMinute} per moving in-game minute " +
            $"(0 km/h free, threshold {cfg.MovingSpeedThresholdKmh:0.#} km/h), payment cash→bank (bank may go negative).");
        SpikeCommands.Print(
            $"[meter] current ride: {(_active ? "RUNNING" : "idle")} — ${_calculatedTotal} fare calculated; " +
            $"payment calls returned ${_paymentStatus.ReturnedDollars} (balance unverified), unknown ${_paymentStatus.UnknownDollars}, unpaid ${_paymentStatus.UnpaidDollars}, " +
            $"{(int)_ledger.MovingMinutes} moving in-game min. Config: {FilePath}");
    }
}
