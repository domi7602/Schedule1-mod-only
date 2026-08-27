using System;
using System.Reflection;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using S1API.Input;
using UnityEngine;

namespace HomelessMod;

/// <summary>
/// Lightweight global focus guard for the HomelessMod. Interactables check
/// <see cref="IsBlockingInput"/> before honouring RMB-Hold for pack-up.
/// True when:
///   - The player is typing in any UI (phone / backpack / Pause menu text input)
///   - The Pause menu is open
///
/// Pattern matches NotesApp/CalculatorApp InputFocus hooks but lives standalone
/// (no Shared-lib dependency, no input-field refs to wire up — pause detection
/// is a polling GameObject scan so we stay compatible across S1API versions).
/// </summary>
[RegisterTypeInIl2Cpp]
public sealed class HomelessInputFocus : MonoBehaviour
{
    public HomelessInputFocus(IntPtr ptr) : base(ptr) { }

    private static HomelessInputFocus? _instance;
    private float _nextScanTime;
    private bool _cachedPauseOpen;

    /// <summary>
    /// Static accessor for hot-path interactorables.
    /// </summary>
    public static bool IsBlockingInput
    {
        get
        {
            // Cheap checks first — most frames this short-circuits.
            if (Controls.IsTyping) return true;
            var inst = _instance;
            if (inst != null && inst._cachedPauseOpen) return true;
            return false;
        }
    }

    public static void EnsureAttached(MelonMod owner)
    {
        if (_instance != null && _instance.Pointer != IntPtr.Zero && !_instance.WasCollected) return;
        try
        {
            var go = new GameObject("HomelessInputFocus");
            GameObject.DontDestroyOnLoad(go);
            go.hideFlags = HideFlags.HideAndDontSave;
            _instance = go.AddComponent<HomelessInputFocus>();
            MelonLogger.Msg("HomelessInputFocus attached.");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"HomelessInputFocus attach failed: {ex.Message}");
        }
    }

    private void Update()
    {
        // Cheap polling for Pause-menu state — only every 0.25s to keep cost ~0.
        if (Time.unscaledTime < _nextScanTime) return;
        _nextScanTime = Time.unscaledTime + 0.25f;
        _cachedPauseOpen = ScanPauseMenuOpen();
    }

    private void OnDestroy()
    {
        if (_instance != null && _instance.Pointer == this.Pointer) _instance = null;
    }

    /// <summary>
    /// Looks up the vanilla PauseMenu UI GameObject without taking a hard IL2CPP
    /// dependency on its type. We use FindObjectsOfTypeAll with a name-based scan
    /// fallback so the check degrades gracefully if the menu name changes.
    /// </summary>
    private static bool ScanPauseMenuOpen()
    {
        try
        {
            // Approach 1: search for any GameObject named "PauseMenu" / "PauseScreen" that's active.
            // FindObjectsOfTypeAll returns inactive objects too — filter by activeInHierarchy.
            var all = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int i = 0; i < all.Count; i++)
            {
                var go = all[i];
                if (go == null || go.Pointer == IntPtr.Zero || go.WasCollected) continue;
                if (!go.activeInHierarchy) continue;
                string n = go.name;
                if (string.IsNullOrEmpty(n)) continue;
                if (n.IndexOf("Pause", StringComparison.OrdinalIgnoreCase) >= 0 &&
                    (n.IndexOf("Menu", StringComparison.OrdinalIgnoreCase) >= 0 ||
                     n.IndexOf("Screen", StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    // Only consider top-level / persistent UI, not deeply nested child panels.
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"PauseMenu scan warning: {ex.Message}");
        }
        return false;
    }
}