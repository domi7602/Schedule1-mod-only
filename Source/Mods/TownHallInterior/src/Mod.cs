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
#if TOWNHALL_BUSINESS_PROBE
        BusinessBindingProbe.Start();
#endif
#if TOWNHALL_BUSINESS_ACTIVATION
        BusinessActivationRuntime.Start(() => _hall != null && _staged is UnityRoomSink room &&
            S1Mods.Shared.NetworkGuard.IsAlive(room.Root) && room.Root!.gameObject.activeInHierarchy);
#elif TOWNHALL_BUSINESS_SUBTYPE
        BusinessSubtypeRuntime.Start(() => _hall != null && _staged is UnityRoomSink room &&
            S1Mods.Shared.NetworkGuard.IsAlive(room.Root) && room.Root!.gameObject.activeInHierarchy);
#elif TOWNHALL_BUSINESS_CONSTRUCTION
        BusinessConstructionRuntime.Start(() => _hall != null && _staged is UnityRoomSink room &&
            S1Mods.Shared.NetworkGuard.IsAlive(room.Root) && room.Root!.gameObject.activeInHierarchy);
#endif
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
#if TOWNHALL_BUSINESS_PROBE
        BusinessBindingProbe.SceneLoaded(sceneName);
#endif
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
#if TOWNHALL_BUSINESS_PROBE
        BusinessBindingProbe.Tick();
#endif
#if TOWNHALL_BUSINESS_ACTIVATION
        BusinessActivationRuntime.Tick();
#elif TOWNHALL_BUSINESS_SUBTYPE
        BusinessSubtypeRuntime.Tick();
#elif TOWNHALL_BUSINESS_CONSTRUCTION
        BusinessConstructionRuntime.Tick();
#endif
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
#if TOWNHALL_BUSINESS_ACTIVATION
        if (sceneName == "Main") BusinessActivationRuntime.SceneUnloaded();
#elif TOWNHALL_BUSINESS_SUBTYPE
        if (sceneName == "Main") BusinessSubtypeRuntime.Leaving();
#endif
#if TOWNHALL_BUSINESS_PROBE
        BusinessBindingProbe.SceneUnloaded(sceneName);
#endif
        if (sceneName == "Main") { DoorHook.NextGeneration(); Cleanup(); }
    }

    public override void OnDeinitializeMelon()
    {
#if TOWNHALL_BUSINESS_ACTIVATION
        BusinessActivationRuntime.Stop();
#elif TOWNHALL_BUSINESS_SUBTYPE
        BusinessSubtypeRuntime.Stop();
#elif TOWNHALL_BUSINESS_CONSTRUCTION
        BusinessConstructionRuntime.Stop();
#endif
#if TOWNHALL_BUSINESS_PROBE
        BusinessBindingProbe.Stop();
#endif
        DoorHook.Uninstall(); Cleanup();
    }

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
