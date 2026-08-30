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
        // Unity's standard pause pattern: the vanilla Pause-menu (and any other
        // world-freezing modal) sets Time.timeScale to 0. Reading the static
        // property is essentially free, so we no longer need a 0.25s scan over
        // every GameObject in the scene — that scan was the 1-FPS-killer.
        _cachedPauseOpen = Time.timeScale == 0f;
    }

    private void OnDestroy()
    {
        if (_instance != null && _instance.Pointer == this.Pointer) _instance = null;
    }
}