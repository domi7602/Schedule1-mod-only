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

            SceneManager.add_sceneLoaded(new Action<Scene, LoadSceneMode>((scene, mode) =>
            {
                HandleSceneLoaded(scene.name, scene.isLoaded);
            }));

            SceneManager.add_sceneUnloaded(new Action<Scene>(scene =>
            {
                HandleSceneUnloaded(scene.name);
            }));

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

    private static void HandleSceneLoaded(string sceneName, bool isLoaded)
    {
        bool wasMain = IsInMainScene;
        CurrentSceneName = sceneName ?? "";
        IsLoaded = isLoaded;
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

    private static void HandleSceneUnloaded(string sceneName)
    {
        bool wasMain = IsInMainScene;
        CurrentSceneName = sceneName ?? "";
        IsLoaded = false;
        IsChangingScenes = true;

        GameObjectResolver.InvalidateCache();

        SafeInvoker.Execute(() => OnSceneChanged?.Invoke(CurrentSceneName), null, "SceneGate.OnSceneChanged");

        if (wasMain && !IsInMainScene)
        {
            SafeInvoker.Execute(() => OnMainSceneUnloaded?.Invoke(), null, "SceneGate.OnMainSceneUnloaded");
        }
    }
}
