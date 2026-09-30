using System;
using Il2CppScheduleOne.UI;
using S1API.Money;
using S1Mods.Shared;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// The taxi's meter (Dominik, 2026-09-29): "ein kostenloses Taxi ist leider nicht
/// Realität ... wenn das Auto fährt, jede Sekunde −1 cash, wenn 0 km/h wird nichts
/// abgezogen — der Trigger beginnt wenn die Destination ausgewählt ist".
///
/// Time base (verified against <c>TimeManager</c>): the game day runs on
/// `CycleDuration` = 24 REAL minutes, so **1 real second = 1 in-game minute**.
/// The meter therefore charges whole dollars per FULL moving in-game minute
/// (= per full real second of motion) — no cents, because the game has none
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
        public float MovingSpeedThresholdKmh { get; set; } = 0.5f;
    }

    private static FareConfig? _config;
    private static bool _configLoaded;
    private static bool _mpWarned;

    private static bool _active;
    private static double _movingSeconds;
    private static int _chargedTotal;
    private static float _lastTickAt;

    /// <summary>Total charged for the current/last ride (dollars, whole).</summary>
    internal static int ChargedTotal => _chargedTotal;

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

        FareConfig cfg = Config;
        _active = true;
        _movingSeconds = 0;
        _chargedTotal = 0;
        _lastTickAt = Time.unscaledTime;

        if (!cfg.Enabled)
        {
            Mod.Log.Info("[meter] ride started — meter OFF via fare.json (free ride).");
            return;
        }

        Mod.Log.Info(
            $"[meter] meter started: ${cfg.DollarsPerInGameMinute} per FULL moving in-game minute " +
            $"(= per real second in motion; 0 km/h is free) — cash first, bank may go negative.");
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

        if (!SpikeState.RideActive || !ReferenceEquals(veh, SpikeState.Vehicle))
        {
            _lastTickAt = Time.unscaledTime;
            return;
        }

        float now = Time.unscaledTime;
        // Hitch-safe: a 2 s freeze must not bill 2 s of standing around as motion,
        // and never more than one unit per frame pair.
        float dt = Mathf.Clamp(now - _lastTickAt, 0f, 0.5f);
        _lastTickAt = now;

        FareConfig cfg = Config;
        if (!cfg.Enabled || cfg.DollarsPerInGameMinute <= 0)
            return;

        float speed;
        try
        {
            speed = Mathf.Abs(veh.Speed_Kmh);
        }
        catch (Exception)
        {
            return; // dead handle — no motion, no charge
        }

        if (speed < cfg.MovingSpeedThresholdKmh)
            return; // standing is free

        _movingSeconds += dt;

        int due = (int)(_movingSeconds * cfg.DollarsPerInGameMinute) - _chargedTotal;
        if (due > 0)
            Charge(due);
    }

    /// <summary>Charges whole dollars: cash first (never below 0), the rest to the bank.</summary>
    private static void Charge(int dollars)
    {
        try
        {
            if (!NetworkGuard.IsHostOrSingleplayer())
            {
                // Fail-closed like BusinessIncome: a client never touches money, but
                // the meter display stays honest (the fare still "ran").
                if (!_mpWarned)
                {
                    _mpWarned = true;
                    Mod.Log.Warn("[meter] multiplayer client — the meter counts, but no money is charged here (host authority).");
                }

                _chargedTotal += dollars;
                return;
            }

            float cash;
            try
            {
                cash = Money.GetCashBalance();
            }
            catch (Exception)
            {
                cash = 0f;
            }

            int fromCash = (int)Mathf.Min(Mathf.Max(cash, 0f), dollars);
            int fromBank = dollars - fromCash;

            if (fromCash > 0)
                Money.ChangeCashBalance(-fromCash, true, false);

            if (fromBank > 0)
                Money.CreateOnlineTransaction("Taxi fare", -fromBank, 1, "Taxi");

            _chargedTotal += dollars;

            // One quiet line per $10 — the per-second tick must never spam the log.
            if (_chargedTotal % 10 == 0)
            {
                Mod.Log.Info(
                    $"[meter] ${_chargedTotal} charged so far " +
                    $"({(int)_movingSeconds} moving in-game min; last charge: ${fromCash} cash / ${fromBank} bank).");
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[meter] charging ${dollars} failed ({ex.GetType().Name}: {ex.Message}) — ride continues, fare skipped.");
        }
    }

    /// <summary>Stops the meter and reports the total (arrival, exit, STOP). Idempotent.</summary>
    internal static void Stop(string reason)
    {
        if (!_active)
            return;

        _active = false;
        int total = _chargedTotal;
        int minutes = (int)_movingSeconds;
        FareConfig cfg = Config;

        Mod.Log.Info($"[meter] ride ended ({reason}) — fare ${total} for {minutes} moving in-game minute(s).");

        if (!cfg.Enabled || total <= 0)
            return;

        try
        {
            NotificationsManager? notifMgr = NotificationsManager.Instance;
            if (notifMgr != null && (UnityEngine.Object)notifMgr != null)
            {
                notifMgr.SendNotification(
                    "Taxi",
                    $"Fare ${total} — {minutes} moving in-game min (${cfg.DollarsPerInGameMinute}/min)",
                    null!,
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
            $"[meter] current ride: {(_active ? "RUNNING" : "idle")} — ${_chargedTotal} charged, " +
            $"{(int)_movingSeconds} moving in-game min. Config: {FilePath}");
    }
}
