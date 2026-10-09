using System;
using UnityEngine.SceneManagement;

namespace S1Mods.Shared;

/// <summary>
/// Central scene monitor and cache.
/// Monitors scene changes, caches the main-scene state and invalidates UI caches on scene changes.
/// </summary>
public static class SceneGate
{
    private static bool _eventsSubscribed;
    private static int _currentSceneHandle = -1;

    // IL2CPP GC safety: delegates passed to the native SceneManager must stay rooted
    // on the managed side for the process lifetime — an inline `new Action(...)`
    // could otherwise be collected and scene callbacks would silently stop (or crash).
    private static readonly Action<Scene, LoadSceneMode> _onSceneLoaded = HandleSceneLoaded;
    private static readonly Action<Scene> _onSceneUnloaded = HandleSceneUnloaded;
    private static readonly Action<Scene, Scene> _onActiveSceneChanged = HandleActiveSceneChanged;

    public static string MainSceneName { get; set; } = "Main";

    public static string CurrentSceneName { get; private set; } = "";

    public static bool IsLoaded { get; private set; }

    public static bool IsChangingScenes { get; private set; }

    public static bool IsInMainScene =>
        string.Equals(CurrentSceneName, MainSceneName, StringComparison.Ordinal) && IsLoaded;

    public static event Action<string>? OnSceneChanged;
    public static event Action? OnMainSceneLoaded;
    public static event Action? OnMainSceneUnloaded;

    static SceneGate()
    {
        EnsureInitialized();
    }

    /// <summary>
    /// Initializes the SceneManager listeners. Called automatically from the static constructor.
    /// </summary>
    public static void EnsureInitialized()
    {
        if (_eventsSubscribed)
        {
            UpdateActiveSceneState();
            return;
        }

        try
        {
            UpdateActiveSceneState();

            SceneManager.add_sceneLoaded(_onSceneLoaded);
            SceneManager.add_sceneUnloaded(_onSceneUnloaded);
            SceneManager.add_activeSceneChanged(_onActiveSceneChanged);

            _eventsSubscribed = true;
        }
        catch
        {
            _eventsSubscribed = false;
        }
    }

    private static void UpdateActiveSceneState()
    {
        try
        {
            var active = SceneManager.GetActiveScene();
            ApplyActiveSceneState(active.name, active.isLoaded, active.handle);
        }
        catch
        {
        }
    }

    private static void ApplyActiveSceneState(string? name, bool loaded, int handle)
    {
        try
        {
            name ??= string.Empty;
            var transition = SceneGateTransition.Evaluate(
                CurrentSceneName, IsLoaded, _currentSceneHandle, name, loaded, handle, MainSceneName);
            if (!transition.Changed) return;

            CurrentSceneName = name;
            IsLoaded = loaded;
            _currentSceneHandle = handle;
            IsChangingScenes = !loaded;

            if (!string.IsNullOrEmpty(name))
                GameObjectResolver.InvalidateCache();

            if (!transition.WasMain && transition.IsMain)
            {
                SafeInvoker.Execute(() => OnMainSceneLoaded?.Invoke(), null, "SceneGate.OnMainSceneLoaded");
            }
            else if (transition.WasMain && !transition.IsMain)
            {
                SafeInvoker.Execute(() => OnMainSceneUnloaded?.Invoke(), null, "SceneGate.OnMainSceneUnloaded");
            }

            SafeInvoker.Execute(() => OnSceneChanged?.Invoke(CurrentSceneName), null, "SceneGate.OnSceneChanged");
        }
        catch
        {
        }
    }

    private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // A load event may be additive and leave the active scene unchanged.
        UpdateActiveSceneState();
    }

    private static void HandleActiveSceneChanged(Scene previous, Scene next)
    {
        ApplyActiveSceneState(next.name, next.isLoaded, next.handle);
    }

    private static void HandleSceneUnloaded(Scene scene)
    {
        UpdateActiveSceneState();
    }
}
