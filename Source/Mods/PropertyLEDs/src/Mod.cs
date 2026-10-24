using System;
using MelonLoader;
using S1API.Lifecycle;
using S1Mods.Shared;

[assembly: MelonInfo(typeof(PropertyLEDs.Mod), "PropertyLEDs", "0.1.8", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace PropertyLEDs;

public sealed class Mod : MelonMod
{
    public static ModLogger Log { get; } = new("PropertyLEDs");

    public override void OnInitializeMelon()
    {
        LEDRuntime.Initialize();
        Unsubscribe();
        GameLifecycle.OnPreLoad += BeforeLoad;
        GameLifecycle.OnLoadComplete += AfterLoad;
        GameLifecycle.OnPreSceneChange += BeforeSceneChange;
        GameLifecycle.OnSaveComplete += AfterSave;
        Log.Info("Initialized (v0.1.8). External verified layouts only; realtime lighting disabled.");
    }

    public override void OnUpdate() => Guard("Debounce tick", LEDRuntime.Tick);

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        if (string.Equals(sceneName, "Main", StringComparison.Ordinal))
            Guard("Gameplay scene unloaded", LEDRuntime.Reset);
    }

    public override void OnApplicationQuit()
    {
        Unsubscribe();
        Guard("Application quit", LEDRuntime.Reset);
    }

    private static void BeforeLoad() => Guard("Pre-load reset", LEDRuntime.Reset);
    private static void AfterLoad() => Guard("Load-complete initialization", LEDRuntime.LoadComplete);
    private static void BeforeSceneChange() => Guard("Pre-scene-change flush/cleanup", LEDRuntime.Reset);
    private static void AfterSave() => Guard("Save-complete flush", LEDRuntime.Flush);

    private static void Unsubscribe()
    {
        GameLifecycle.OnPreLoad -= BeforeLoad;
        GameLifecycle.OnLoadComplete -= AfterLoad;
        GameLifecycle.OnPreSceneChange -= BeforeSceneChange;
        GameLifecycle.OnSaveComplete -= AfterSave;
    }

    private static void Guard(string context, Action action)
    {
        try { action(); }
        catch (Exception ex) { Log.Warn(context, ex); }
    }
}
