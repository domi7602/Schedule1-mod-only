using MelonLoader;

[assembly: MelonInfo(typeof(WorldLook.WorldLookMod), "WorldLook", "0.1.1", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace WorldLook;

/// <summary>
/// One look mod: clock-weighted light, the reference water surface, and a manual
/// rendering probe. The three used to be separate MelonMods; loading all three
/// from this assembly would double-apply the look, so only this type is a MelonMod.
/// </summary>
public sealed class WorldLookMod : MelonMod
{
    private readonly GoldenHour.Mod _lighting = new();
    private readonly GraphicsProbe.Mod _probe = new();
    private readonly WaterShader.WaterShaderMod _water = new();

    public override void OnInitializeMelon()
    {
        _lighting.OnInitializeMelon();
        _probe.OnInitializeMelon(Info.Version);
        MelonLogger.Msg($"[WorldLook] Initialized (v{Info.Version}). Light Ctrl+Shift+F7, probe Ctrl+Shift+F8. Water applies on Main. In-Game-Verify open.");
    }

    public override void OnLateInitializeMelon() => _water.OnLateInitializeMelon();

    public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        => _probe.OnSceneWasInitialized(buildIndex, sceneName);

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        _probe.OnSceneWasLoaded(buildIndex, sceneName);
        _water.OnSceneWasLoaded(buildIndex, sceneName);
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        _probe.OnSceneWasUnloaded(buildIndex, sceneName);
        _water.OnSceneWasUnloaded(buildIndex, sceneName);
    }

    public override void OnUpdate()
    {
        _lighting.OnUpdate();
        _probe.OnUpdate();
        _water.OnUpdate();
    }

    public override void OnLateUpdate() => _lighting.OnLateUpdate();

    public override void OnApplicationQuit() => _lighting.OnApplicationQuit();

    public override void OnDeinitializeMelon()
    {
        _probe.OnDeinitializeMelon();
        _water.OnDeinitializeMelon();
        _lighting.OnDeinitializeMelon();
    }
}
