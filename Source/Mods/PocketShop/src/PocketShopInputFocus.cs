using System;
using MelonLoader;
using S1API.Input;
using UnityEngine;
using UnityEngine.UI;

namespace PocketShop;

/// <summary>
/// Reports "the player is typing" to the game's global input gate while any PocketShop input
/// field is focused. Several fields can exist at once (search + quantity), so the flag is
/// ref-counted across focused fields instead of being written by each field independently —
/// otherwise one field blurring would clear the global typing state while another field is
/// still being edited.
/// </summary>
[RegisterTypeInIl2Cpp]
public sealed class PocketShopInputFocus : MonoBehaviour
{
    public PocketShopInputFocus(IntPtr ptr) : base(ptr) { }

    public InputField? inputField;

    private static int _focusedCount;
    private static bool _globalTyping;

    private bool _registered;

    private void Update()
    {
        bool focused = inputField != null && inputField.isFocused && isActiveAndEnabled;
        if (focused == _registered) return;

        if (focused)
        {
            _registered = true;
            _focusedCount++;
        }
        else
        {
            _registered = false;
            _focusedCount = Math.Max(0, _focusedCount - 1);
        }
        ApplyGlobalTyping();
    }

    private void OnDisable()
    {
        Unregister();
    }

    private void OnDestroy()
    {
        Unregister();
    }

    private void Unregister()
    {
        if (!_registered) return;
        _registered = false;
        _focusedCount = Math.Max(0, _focusedCount - 1);
        ApplyGlobalTyping();
    }

    private static void ApplyGlobalTyping()
    {
        bool typing = _focusedCount > 0;
        if (typing == _globalTyping) return;
        _globalTyping = typing;
        try { Controls.IsTyping = typing; }
        catch (Exception ex) { MelonLogger.Warning($"[PocketShop] Could not update the global typing state: {ex.Message}"); }
    }

    /// <summary>
    /// Scene boundary: the fields are destroyed with the app, so the count and the global
    /// flag are dropped deliberately — never per app open.
    /// </summary>
    public static void ResetGlobalTypingState()
    {
        _focusedCount = 0;
        _globalTyping = false;
        try { Controls.IsTyping = false; }
        catch (Exception ex) { MelonLogger.Warning($"[PocketShop] Could not reset the global typing state: {ex.Message}"); }
    }
}
