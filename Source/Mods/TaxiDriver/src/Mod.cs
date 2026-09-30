using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(TaxiDriver.Mod), "TaxiDriver", "0.7.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace TaxiDriver;

public class Mod : MelonMod
{
    /// <summary>Shared logger — prefixes every line with [TaxiDriver].</summary>
    internal static ModLogger Log { get; } = new("TaxiDriver");

    public override void OnInitializeMelon()
    {
        Log.Info("initialised — Stage 1 spike (autonomous drive / NPC seat / player enter-exit) + Stage 2 visual swap (taxi.glb via S1MAPI.Gltf.GltfLoader) + Stage 3 driver ride (vanilla NPC drives, player rides along; seat proof = OccupantNPCs slot + root-to-seat distance) + Stage 3b taxi stand (every ParkingLot is dumped on scene load, the taxi spawns on the fixed stand and drives TO the player via F5) + Stage 3c passenger ride (ride locks + destination picker) + Stage 4 (the GAME's own patrol driver cloned onto the taxi: runtime VehiclePatrolRoute + VehiclePatrolBehaviour, with the mod's Navigate dispatch as fallback) + Stage 3d (destination catalog from the game's own deal locations / lot entries, drop-off rule, STOP despawns the taxi, progress watchdog, ride heartbeat).");

        // Idempotent: unsubscribe first so a re-initialised mod never double-hooks.
        MelonEvents.OnUpdate.Unsubscribe(SpikeRunner.Update);
        MelonEvents.OnUpdate.Subscribe(SpikeRunner.Update);
        Log.Info("SpikeRunner.Update subscribed to MelonEvents.OnUpdate.");

        // Stage 3c: the passenger-ride input + trunk locks (PatchGuard — a failed
        // patch is non-fatal and named in the log; the ride still works without it).
        RideLocks.Apply();

        // Close experiment (transparent frame): armed once, reverts cleanly.
        TaxiCloseExperiment.Enable();

        // Paket A (2026-09-29): the destination picker must NOT survive a save
        // reload (Dominik: "das Taxi merkt sich die letzten punkte, nachdem man das
        // game reloaded"). OnSaveInfoLoaded is the workspace's canonical save-load
        // boundary (fires after save parsing, before scene build).
        GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
        Log.Info("GameLifecycle.OnSaveInfoLoaded hooked — the destination picker is cleared on every save load.");

        Log.Info("Console: `taxi help` (both `taxi` and `/taxi` are registered) — but the MelonLoader console is log-only, so every taxi command is output-only. Hotkeys are the control surface (F1-F12): F1 road target A, F2 road target B, F3 full run to road target, F4 visual swap on/off toggle (applies to the next spawn), F5 call-taxi (spawn at the fixed taxi stand -> npc -> navigate to a road point near the player), F6 full run, F7 probe, F8 trace, F9 ride/out, F10 go2 (settings=null), F11 run without NPC, F12 Navigate on a vanilla vehicle. Hotkeys only fire in the gameplay scene (menu scenes keep their own keys).");
    }

    /// <summary>
    /// Stale-state guard for save switches: navigation polling, the F6 automation and
    /// a pending respawn are cleared when a scene loads and the spike vehicle is gone.
    /// </summary>
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        SpikeRunner.OnSceneLoaded(sceneName);
    }

    public override void OnApplicationQuit()
    {
        Teardown("quit");
    }

    /// <summary>Review minor: teardown must not live only in OnApplicationQuit.</summary>
    public override void OnDeinitializeMelon()
    {
        GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
        Teardown("deinit");
    }

    /// <summary>
    /// Paket A: every save load starts with a clean destination picker — the stale
    /// static picker state was the "taxi drives to the last checkpoint after reload" bug.
    /// </summary>
    private static void OnSaveInfoLoaded()
    {
        SpikeState.ResetPicker();
        Log.Info("[picker] save loaded — destination selection cleared (pick a place before the next ride).");
    }

    private static void Teardown(string reason)
    {
        MelonEvents.OnUpdate.Unsubscribe(SpikeRunner.Update);
        SpikeRunner.Shutdown();
        Log.Info($"SpikeRunner.Update unsubscribed from MelonEvents.OnUpdate ({reason}).");
    }
}
