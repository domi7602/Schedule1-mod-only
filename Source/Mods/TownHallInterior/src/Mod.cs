using System;
using MelonLoader;
[assembly: MelonInfo(typeof(TownHallInterior.Mod), "TownHallInterior", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]
namespace TownHallInterior;

/// <summary>Experimental local hall; only a fully prepared, identity-matched facade is published.</summary>
public sealed class Mod : MelonMod
{
    private IRoomSink? _staged;
    private PublicHallHost? _hall;

    public override void OnInitializeMelon()
    {
        MelonLogger.Warning("TownHallInterior: experimental local hall; hash-pinned geometric portal, not a business.");
        DoorHook.Install();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        if (sceneName != "Main" || _staged != null) return;
        DoorHook.NextGeneration();
        try
        {
            _staged = RoomBuilder.Build(() => new UnityRoomSink());
            _hall = PublicHallHost.TryOpen(_staged);
            if (_hall == null)
                MelonLogger.Warning("TownHallInterior: hall stayed inactive; any pending native restore is logged separately.");
        }
        catch (Exception ex) { MelonLogger.Warning("TownHallInterior staging failed: " + ex); }
    }

    public override void OnUpdate()
    {
        _hall?.Tick(UnityEngine.Time.deltaTime);
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (sceneName == "Main") { DoorHook.NextGeneration(); Cleanup(); }
    }

    public override void OnDeinitializeMelon() { DoorHook.Uninstall(); Cleanup(); }

    private void Cleanup()
    {
        var hall = _hall;
        var old = _staged;
        _hall = null;
        _staged = null;
        for (int attempt = 0; attempt < 2; attempt++)
        {
            try { hall?.Dispose(); }
            catch (Exception ex) { MelonLogger.Warning("TownHallInterior hall restore failed: " + ex); }
            if (hall == null || hall.CleanupComplete) break;
        }
        if (hall != null && !hall.CleanupComplete)
            MelonLogger.Warning("TownHallInterior: native restoration remains incomplete after two passes; inspect the pending-step log.");
        try { old?.Dispose(); }
        catch (Exception ex) { MelonLogger.Warning("TownHallInterior cleanup failed: " + ex); }
    }
}
