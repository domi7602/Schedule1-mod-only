using System;
using System.Collections.Generic;
using System.Linq;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using S1API.Console;
using S1API.Lifecycle;
using S1Mods.Shared;
using UnityEngine;

[assembly: MelonInfo(typeof(PoliceTestMode.Mod), "PoliceTestMode", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PoliceTestMode;

public sealed class Mod : MelonMod
{
    internal static Mod? Instance;
    internal readonly PoliceHooks Hooks = new();
    private readonly PoliceWorld _world = new();
    internal bool Requested = true;
    private const int ReadinessLimit = 30;
    private bool _loaded;
    private bool _worldReady;
    private bool _fault;
    private int _attempts;
    private float _nextPoll;
    private string _state = "WAITING for loaded singleplayer world";

    internal static void Log(string text) => MelonLogger.Msg("[PoliceTestMode] " + text);

    public override void OnInitializeMelon()
    {
        Instance = this;
        Hooks.Install();
        GameLifecycle.OnLoadComplete += Loaded;
        GameLifecycle.OnPreLoad += Leaving;
        GameLifecycle.OnPreSceneChange += Leaving;
        Log("TEST MOD default requested=ON; session-only. policetest on|off|status. No LE/save edits.");
    }

    internal bool ShouldSuppress()
        => SessionPolicy.ShouldSuppress(Requested && !_fault, Hooks.Ready, _loaded && _worldReady, Singleplayer());

    private static bool Singleplayer()
    {
        try
        {
            if (!NetworkGuard.IsInMainScene || !NetworkGuard.IsHostOrSingleplayer()) return false;
            var lobby = Singleton<Lobby>.Instance;
            if (!NetworkGuard.IsAlive(lobby) || lobby.IsInLobby) return false;
            return Player.PlayerList != null && Player.PlayerList.Count == 1 && NetworkGuard.IsAlive(Player.Local);
        }
        catch { return false; } // Unknown authority never authorizes mutation.
    }

    private void Loaded()
    {
        Restore("load boundary");
        _fault = _world.Count > 0;
        _loaded = true;
        _worldReady = false;
        _attempts = 0;
        _nextPoll = 0;
        Log("lifecycle load-complete; readiness window armed");
    }

    private void Leaving()
    {
        _loaded = _worldReady = false; // Disable gates before restoring objects.
        Restore("lifecycle leaving");
        _state = _world.Count > 0 ? "ERROR - unresolved restoration" : "WAITING for loaded singleplayer world";
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (sceneName == "Main") Leaving();
    }

    public override void OnUpdate()
    {
        if (Time.unscaledTime < _nextPoll) return;
        _nextPoll = Time.unscaledTime + 1f;
        if (!Requested || !Hooks.Ready || !_loaded || !Singleplayer())
        {
            _worldReady = false;
            if (_world.Count > 0 && !_fault) Restore("inactive/authority refusal");
            if (_fault) return; // Failed restore exhausted its three attempts; explicit command/lifecycle only.
            _state = !Requested ? "OFF - native police permitted" : !Hooks.Ready ? "ERROR - required hooks missing" : "WAITING / multiplayer refused";
            return;
        }
        if (_fault) return;
        try
        {
            if (!_worldReady)
            {
                if (++_attempts > ReadinessLimit)
                {
                    _fault = true;
                    _state = "ERROR - readiness timeout; policetest on to retry";
                    Log(_state);
                    return;
                }
                if (!_world.Ready()) return;
                _worldReady = true;
            }
            _world.Apply(); // Fixed police registries only, max 512 entries; once/second.
            string state = _world.UnknownVehicles > 0
                            ? "PARTIAL - vehicle occupancy unreadable; see status"
                            : _world.OccupiedVehicles > 0
                            ? "PARTIAL - player-occupied police vehicle protected; see status"
                : "ACTIVE - police suppressed (TEST)";
            if (_state != state)
            {
                _state = state;
                Status();
            }
        }
        catch (Exception ex)
        {
            _fault = true;
            _worldReady = false;
            _state = "ERROR - suppression failed; native police permitted";
            Log(_state + ": " + ex);
            Restore("apply failure");
        }
    }

    private void Restore(string reason)
    {
        // Idempotent bool restores only. Retain unresolved entries, never mark clean on exception.
        for (int retry = 0; retry < 3 && _world.Count > 0; retry++) _world.Restore();
        if (_world.Count > 0) { _fault = true; _state = "ERROR - unresolved restoration"; }
        Log($"restore reason={reason} pending={_world.Count}");
    }

    internal void SetRequested(bool enabled)
    {
        _worldReady = false;
        Restore("command");
        Requested = enabled;
        _fault = _world.Count > 0;
        _attempts = 0;
        _nextPoll = 0;
        _state = _fault ? "ERROR - unresolved restoration" : enabled ? "WAITING for suppression readback" : "OFF - native police permitted";
        Status();
    }

    internal void Status()
    {
        Log($"status requested={Requested} effective={ShouldSuppress()} state='{_state}' hooks={Hooks.Installed} loaded={_loaded} singleplayer={Singleplayer()} attempts={_attempts}/{ReadinessLimit} leases={_world.Count} officers={_world.Officers} vehicles={_world.Vehicles} playerOccupiedSkipped={_world.OccupiedVehicles} unknownSkipped={_world.UnknownVehicles} checkpoints={_world.Checkpoints}");
        Log(_world.Readback());
        foreach (var counter in Hooks.Blocked.OrderBy(x => x.Key)) Log($"blocked {counter.Key}={counter.Value}");
    }

    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnLoadComplete -= Loaded;
        GameLifecycle.OnPreLoad -= Leaving;
        GameLifecycle.OnPreSceneChange -= Leaving;
        Requested = false;
        Leaving();
        Hooks.Remove();
        Instance = null;
    }
}

// S1API discovers public parameterless BaseConsoleCommand subclasses on console Awake.
public sealed class PoliceTestCommand : BaseConsoleCommand
{
    public override string CommandWord => "policetest";
    public override string CommandDescription => "Session-only police test suppression: on, off, status.";
    public override string ExampleUsage => "policetest status";
    public override void ExecuteCommand(List<string> args)
    {
        var mod = Mod.Instance;
        if (mod == null) { Mod.Log("mod not initialized"); return; }
        if (args.Count == 0 || (args.Count == 1 && args[0].Equals("status", StringComparison.OrdinalIgnoreCase))) mod.Status();
        else if (args.Count == 1 && args[0].Equals("on", StringComparison.OrdinalIgnoreCase)) mod.SetRequested(true);
        else if (args.Count == 1 && args[0].Equals("off", StringComparison.OrdinalIgnoreCase)) mod.SetRequested(false);
        else Mod.Log("Usage: policetest on|off|status (no state changed)");
    }
}
