using MelonLoader;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(TaxiDriver.Mod), "TaxiDriver", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace TaxiDriver;

public class Mod : MelonMod
{
    /// <summary>Shared logger — prefixes every line with [TaxiDriver].</summary>
    internal static ModLogger Log { get; } = new("TaxiDriver");

    public override void OnInitializeMelon()
    {
        Log.Info("initialised (v0.1.0) — Stage 1 spike (autonomous drive / NPC seat / player enter-exit) + Stage 2 visual swap (taxi.glb via S1MAPI.Gltf.GltfLoader) + Stage 3 driver ride (vanilla NPC drives, player rides along; seat proof = OccupantNPCs slot + root-to-seat distance) + Stage 3b taxi stand (every ParkingLot is dumped on scene load, the taxi spawns on the fixed stand and drives TO the player via F17).");

        // Idempotent: unsubscribe first so a re-initialised mod never double-hooks.
        MelonEvents.OnUpdate.Unsubscribe(SpikeRunner.Update);
        MelonEvents.OnUpdate.Subscribe(SpikeRunner.Update);
        Log.Info("SpikeRunner.Update subscribed to MelonEvents.OnUpdate.");

        Log.Info("Console: `taxi help` (both `taxi` and `/taxi` are registered) — but the MelonLoader console is log-only, so every taxi command is output-only. Hotkeys are the control surface: F6 full run, F7 probe, F8 trace, F9 ride/out, F10 go2 (settings=null), F11 run without NPC, F12 Navigate on a vanilla vehicle, F13 road target A, F14 road target B, F15 full run to road target, F16 visual swap on/off toggle (applies to the next spawn), F17 call-taxi (spawn at the fixed taxi stand -> npc -> navigate to a road point near the player). F13-F17 only reach the game through real key input (SendInput, focus_key.py — VK_F16 = 0x7F, VK_F17 = 0x80).");
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
        Teardown("deinit");
    }

    private static void Teardown(string reason)
    {
        MelonEvents.OnUpdate.Unsubscribe(SpikeRunner.Update);
        SpikeRunner.Shutdown();
        Log.Info($"SpikeRunner.Update unsubscribed from MelonEvents.OnUpdate ({reason}).");
    }
}
