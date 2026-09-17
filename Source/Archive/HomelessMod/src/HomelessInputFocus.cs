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
        // Gatekeeper-fix L4: Time.timeScale==0f is unreliable in Schedule I (Netcode/Netcode-host does not globally freeze
        // timeScale on pause; pause is a UI/menu state). Kept as best-effort cheap check, but the real guard is
        // Cursor.lockState != Locked and Controls.IsTyping in IsBlockingInput / interactables. Consider also checking
        // an explicit menu-open flag (e.g. PauseMenu.Instance.IsOpen or ESC menu active) if a stable API is available.
        // Previous 0.25s GameObject scan was removed as the 1-FPS-killer; this static property read is essentially free.
        _cachedPauseOpen = Time.timeScale == 0f;
    }

    private void OnDestroy()
    {
        if (_instance != null && _instance.Pointer == this.Pointer) _instance = null;
    }
}
