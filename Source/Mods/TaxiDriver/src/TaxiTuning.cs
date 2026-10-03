using System;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vehicles.AI;
using S1Mods.Shared;

namespace TaxiDriver;

/// <summary>
/// Driving-agent tuning (Paket 1, review 2026-10-02). <c>VehicleAgent.StuckTimeThreshold</c>
/// is an INSTANCE field on the agent (verified in the installed interop assembly:
/// <c>Il2CppScheduleOne.Vehicles.AI.VehicleAgent.StuckTimeThreshold</c> with a public
/// setter) — so this changes only the taxi's own agent, never any other vehicle in the
/// game, and needs no global restore (the agent dies with the vehicle).
///
/// Config: <c>UserData/TaxiDriver/tuning.json</c> (SafeStorage, hand-editable).
/// </summary>
internal static class TaxiTuning
{
    /// <summary>Hand-editable driving tuning (public properties: System.Text.Json round-trip).</summary>
    public sealed class TuningConfig
    {
        /// <summary>
        /// Seconds the game's own agent tolerates a stuck vehicle before it un-sticks it
        /// (vanilla default 15). 0 = leave the game's value untouched.
        /// </summary>
        public float StuckTimeThresholdSeconds { get; set; } = 6f;
    }

    private static TuningConfig? _config;
    private static bool _configLoaded;

    /// <summary>The sidecar path (also logged on first load).</summary>
    internal static string FilePath => SafeStorage.GetUserDataPath("TaxiDriver", "tuning.json");

    private static TuningConfig Config
    {
        get
        {
            if (_configLoaded)
                return _config!;

            _configLoaded = true;
            _config = SafeStorage.LoadSafe(FilePath, new TuningConfig(), Mod.Log);
            if (!System.IO.File.Exists(FilePath))
            {
                SafeStorage.SaveAtomic(FilePath, _config, Mod.Log);
                Mod.Log.Info(
                    $"[tune] first run — default tuning written to {FilePath} " +
                    $"(StuckTimeThresholdSeconds={_config!.StuckTimeThresholdSeconds:0.###}, 0 = game default).");
            }

            if (float.IsNaN(_config.StuckTimeThresholdSeconds) || float.IsInfinity(_config.StuckTimeThresholdSeconds) ||
                _config.StuckTimeThresholdSeconds < 0f)
                _config.StuckTimeThresholdSeconds = 6f;
            Mod.Log.Info(
                $"[tune] loaded {FilePath}: StuckTimeThresholdSeconds={_config.StuckTimeThresholdSeconds:0.###} (0 = game default).");
            return _config;
        }
    }

    /// <summary>
    /// Applies the tuning to a freshly spawned taxi agent. Instance fields only — call
    /// once per spawn (the agent belongs to the vehicle instance).
    /// </summary>
    internal static void Apply(LandVehicle veh)
    {
        float want = Config.StuckTimeThresholdSeconds;
        if (want <= 0f)
        {
            Mod.Log.Info("[tune] StuckTimeThreshold left at the game default (config = 0).");
            return;
        }

        try
        {
            VehicleAgent? agent = veh.Agent;
            if (agent == null)
            {
                Mod.Log.Warn("[tune] no VehicleAgent on the spawned taxi — tuning skipped.");
                return;
            }

            float before = agent.StuckTimeThreshold;
            if (Math.Abs(before - want) < 0.01f)
            {
                Mod.Log.Info($"[tune] StuckTimeThreshold already {before:0.###} s.");
                return;
            }

            agent.StuckTimeThreshold = want;
            float applied = agent.StuckTimeThreshold;
            Mod.Log.Info(
                $"[tune] taxi agent StuckTimeThreshold {before:0.###} -> {want:0.###} s " +
                $"(readback {applied:0.###}; this vehicle's agent only — the game un-sticks earlier).");
            if (Math.Abs(applied - want) >= 0.01f)
                Mod.Log.Warn("[tune] readback differs — the game may overwrite StuckTimeThreshold (in-game check open).");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[tune] applying StuckTimeThreshold failed ({ex.GetType().Name}: {ex.Message}) — game default stays.");
        }
    }
}
