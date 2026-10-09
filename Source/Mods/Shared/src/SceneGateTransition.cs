using System;

namespace S1Mods.Shared;

internal readonly struct SceneGateTransitionResult
{
    public SceneGateTransitionResult(bool changed, bool wasMain, bool isMain)
    {
        Changed = changed;
        WasMain = wasMain;
        IsMain = isMain;
    }

    public bool Changed { get; }
    public bool WasMain { get; }
    public bool IsMain { get; }
}

/// <summary>Pure active-scene transition logic used by SceneGate and its tests.</summary>
internal static class SceneGateTransition
{
    public static SceneGateTransitionResult Evaluate(
        string? currentName,
        bool currentLoaded,
        int currentHandle,
        string? nextName,
        bool nextLoaded,
        int nextHandle,
        string mainSceneName)
    {
        currentName ??= string.Empty;
        nextName ??= string.Empty;
        mainSceneName ??= string.Empty;

        bool wasMain = string.Equals(currentName, mainSceneName, StringComparison.Ordinal) && currentLoaded;
        bool isMain = string.Equals(nextName, mainSceneName, StringComparison.Ordinal) && nextLoaded;
        bool changed = !string.Equals(currentName, nextName, StringComparison.Ordinal) ||
                       currentLoaded != nextLoaded ||
                       currentHandle != nextHandle;
        return new SceneGateTransitionResult(changed, wasMain, isMain);
    }
}
