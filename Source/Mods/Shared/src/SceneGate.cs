using System;
using UnityEngine.SceneManagement;

namespace S1Mods.Shared;

/// <summary>
/// Zentraler Szenen-Monitor und Cache.
/// Überwacht Szenenwechsel, cacht den Main-Scene-Status und invalidiert UI-Caches bei Szenenwechseln.
/// </summary>
public static class SceneGate
{
    private static bool _eventsSubscribed;

    // IL2CPP GC safety: delegates passed to the native SceneManager must stay rooted
    // on the managed side for the process lifetime — an inline `new Action(...)`
    // could otherwise be collected and scene callbacks would silently stop (or crash).
    private static readonly Action<Scene, LoadSceneMode> _onSceneLoaded = HandleSceneLoaded;
    private static readonly Action<Scene> _onSceneUnloaded = HandleSceneUnloaded;

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
    /// Initialisiert die SceneManager-Listener. Wird beim statischen Konstruktor automatisch aufgerufen.
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
            string name = active.name ?? "";
            bool loaded = active.isLoaded;

            if (name != CurrentSceneName || loaded != IsLoaded)
            {
                bool wasMain = IsInMainScene;
                CurrentSceneName = name;
                IsLoaded = loaded;
                IsChangingScenes = !loaded;

                if (!string.IsNullOrEmpty(name))
                {
                    GameObjectResolver.InvalidateCache();
                }

                bool isNowMain = string.Equals(CurrentSceneName, MainSceneName, StringComparison.Ordinal) && IsLoaded;
                if (!wasMain && isNowMain)
                {
                    SafeInvoker.Execute(() => OnMainSceneLoaded?.Invoke(), null, "SceneGate.OnMainSceneLoaded");
                }
                else if (wasMain && !isNowMain)
                {
                    SafeInvoker.Execute(() => OnMainSceneUnloaded?.Invoke(), null, "SceneGate.OnMainSceneUnloaded");
                }

                SafeInvoker.Execute(() => OnSceneChanged?.Invoke(CurrentSceneName), null, "SceneGate.OnSceneChanged");
            }
        }
        catch
        {
        }
    }

    private static void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var active = SceneManager.GetActiveScene();

        bool wasMain = IsInMainScene;
        CurrentSceneName = active.name ?? "";
        IsLoaded = active.isLoaded;
        IsChangingScenes = false;

        GameObjectResolver.InvalidateCache();

        SafeInvoker.Execute(() => OnSceneChanged?.Invoke(CurrentSceneName), null, "SceneGate.OnSceneChanged");

        bool isNowMain = string.Equals(CurrentSceneName, MainSceneName, StringComparison.Ordinal) && IsLoaded;
        if (!wasMain && isNowMain)
        {
            SafeInvoker.Execute(() => OnMainSceneLoaded?.Invoke(), null, "SceneGate.OnMainSceneLoaded");
        }
        else if (wasMain && !isNowMain)
        {
            SafeInvoker.Execute(() => OnMainSceneUnloaded?.Invoke(), null, "SceneGate.OnMainSceneUnloaded");
        }
    }

    private static void HandleSceneUnloaded(Scene scene)
    {
        var active = SceneManager.GetActiveScene();

        bool wasMain = IsInMainScene;
        CurrentSceneName = active.name ?? "";
        IsLoaded = active.isLoaded;
        IsChangingScenes = !active.isLoaded;

        GameObjectResolver.InvalidateCache();

        SafeInvoker.Execute(() => OnSceneChanged?.Invoke(CurrentSceneName), null, "SceneGate.OnSceneChanged");

        if (wasMain && !IsInMainScene)
        {
            SafeInvoker.Execute(() => OnMainSceneUnloaded?.Invoke(), null, "SceneGate.OnMainSceneUnloaded");
        }
    }
}
