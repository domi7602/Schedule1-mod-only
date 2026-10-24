using System;
using MelonLoader;
using S1API.Building;
using S1API.Lifecycle;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(LightControl.Mod), "LightControl", "0.1.0-dev.19", "LightControl Project")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace LightControl;

/// <summary>
/// Production LightControl host. The pure, game-free model lives in <c>LightControl.Core</c>; this
/// type only wires lifecycle events to <see cref="LightRuntime"/> and keeps every callback guarded
/// so one bad frame cannot kill the mod or the game.
/// </summary>
public sealed class Mod : MelonMod
{
    public static ModLogger Log { get; } = new("LightControl");

    /// <summary>One-shot shutdown gate: deinitialization and quit must tear down at most once.</summary>
    private static bool _deinitialized;

    public override void OnInitializeMelon()
    {
        _deinitialized = false;
        LightRuntime.Initialize();
        Unsubscribe();
        GameLifecycle.OnPreLoad += BeforeLoad;
        GameLifecycle.OnLoadComplete += AfterLoad;
        GameLifecycle.OnPreSceneChange += BeforeSceneChange;
        GameLifecycle.OnSaveStart += SaveStarted;
        // Newly placed buildables: filtered inside the handler (GetComponent<GrowLight>()), so a
        // vanilla or donor object never gets a controller.
        BuildEvents.OnGridItemCreated += LightRuntime.OnBuildablePlaced;
        BuildEvents.OnBuildableItemInitialized += LightRuntime.OnBuildablePlaced;
        Log.Info("Initialized. Host/singleplayer grow-lamp tint and brightness from the phone app, slot-scoped storage, original restoration. The lamp's own LED panel and the light it emits are always driven together from the same color, so both match. Plant materials are never modified.");
    }

    // Update restores owned native outputs, steps bounded discovery and retries persistence.
    // LateUpdate applies the shared instruction to the lamp's own panel and native light sources.
    public override void OnUpdate() => Guard("Update phase (owned restore, bounded scan and autosave)", LightRuntime.UpdatePhase);

    public override void OnLateUpdate() => Guard("Late-update phase (apply coupled lamp output)", LightRuntime.LateUpdatePhase);

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (string.Equals(sceneName, "Main", StringComparison.Ordinal))
            Guard("Gameplay scene unloaded", LightRuntime.Teardown);
    }

    /// <summary>
    /// Symmetrical loader teardown: release every event subscription first, then restore and drop the
    /// runtime session. Guarded by a one-shot gate so a re-init can re-apply the hooks cleanly and a
    /// repeated deinit is a no-op.
    /// </summary>
    public override void OnDeinitializeMelon()
    {
        if (_deinitialized) return;
        _deinitialized = true;
        Unsubscribe();
        Guard("Deinitialization teardown", LightRuntime.Teardown);
    }

    public override void OnApplicationQuit()
    {
        if (_deinitialized) return;
        _deinitialized = true;
        Unsubscribe();
        Guard("Application quit", LightRuntime.Teardown);
    }

    private static void BeforeLoad() => Guard("Pre-load teardown", LightRuntime.Teardown);

    private static void AfterLoad() => Guard("Load-complete initialization", LightRuntime.LoadComplete);

    private static void BeforeSceneChange() => Guard("Pre-scene-change teardown", LightRuntime.Teardown);

    private static void SaveStarted() => Guard("Save-start persist", LightRuntime.SaveNow);

    private static void Unsubscribe()
    {
        GameLifecycle.OnPreLoad -= BeforeLoad;
        GameLifecycle.OnLoadComplete -= AfterLoad;
        GameLifecycle.OnPreSceneChange -= BeforeSceneChange;
        GameLifecycle.OnSaveStart -= SaveStarted;
        BuildEvents.OnGridItemCreated -= LightRuntime.OnBuildablePlaced;
        BuildEvents.OnBuildableItemInitialized -= LightRuntime.OnBuildablePlaced;
    }

    private static void Guard(string context, Action action)
    {
        try { action(); }
        catch (Exception ex) { Log.Warn(context, ex); }
    }
}
