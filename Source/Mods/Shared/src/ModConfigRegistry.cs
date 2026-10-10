using System;
using System.Collections.Generic;

namespace S1Mods.Shared;

/// <summary>
/// Central registry of pending <see cref="ModConfig{T}"/> flushes.
/// <para>
/// ModConfig debounces disk writes, so a value changed via SetAndSave lives only
/// in RAM for up to <see cref="ModConfig{T}.SaveDebounceMs"/>. Without a drain,
/// quitting or changing scenes inside that window writes MelonPreferences' stale
/// cache (the game re-saves the category itself) and the new value is lost.
/// </para>
/// <para>
/// Each ModConfig&lt;T&gt; registers its Flush on Initialize; SceneGate drains the
/// registry on main-scene unload and Unity's wantsToQuit drains it on quit, so no
/// consuming mod has to remember the flush call sites.
/// </para>
/// </summary>
public static class ModConfigRegistry
{
    private static readonly List<WeakReference<Action>> _flushers = new();
    private static readonly object _lock = new();
    private static bool _quitHooked;

    /// <summary>Registers a flush delegate (called by ModConfig&lt;T&gt;.Initialize).</summary>
    public static void Register(Action flush)
    {
        lock (_lock)
        {
            // Drop dead references from unloaded mod domains first.
            _flushers.RemoveAll(wr => !wr.TryGetTarget(out _));
            _flushers.Add(new WeakReference<Action>(flush));
        }
        EnsureQuitHook();
    }

    /// <summary>Runs every registered flush that is still alive. Safe to call anytime (no-op when empty).</summary>
    public static void FlushAll()
    {
        lock (_lock)
        {
            for (int i = _flushers.Count - 1; i >= 0; i--)
            {
                if (_flushers[i].TryGetTarget(out Action? flush))
                {
                    try { flush(); }
                    catch { /* one mod's flush failure must not block the others */ }
                }
                else
                {
                    _flushers.RemoveAt(i);
                }
            }
        }
    }

    private static readonly Func<bool> _onWantsToQuit = () =>
    {
        FlushAll();
        return true; // never block the quit
    };

    private static void EnsureQuitHook()
    {
        if (_quitHooked) return;
        _quitHooked = true;
        try
        {
            // Rooted delegate: Unity keeps it alive for the process lifetime.
            UnityEngine.Application.wantsToQuit += _onWantsToQuit;
        }
        catch
        {
            // Unity runtime unavailable (e.g. unit tests) — quit drain is skipped there.
            _quitHooked = false;
        }
    }
}
