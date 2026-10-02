using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace S1Mods.Shared;

/// <summary>
/// Text-input guard shared by the mods (Key Rule 5): true while a UI text field
/// is focused, so hotkeys / WASD movement can be suppressed while typing.
/// The former instance-based hotkey router was removed 2026-10-02 — no mod used
/// it (TaxiDriver and MoreSaveSlots drive their own key loops).
/// </summary>
public static class HotkeyManager
{
    /// <summary>True if a UI text field is focused (hotkeys should not fire in that case).</summary>
    public static bool IsInputFieldFocused()
    {
        try
        {
            var es = EventSystem.current;
            // Bug-Audit 2026-09-12: EventSystem==null happens during scene transitions /
            // loading screens. Previously returned false (fail-open) so hotkeys would fire
            // during load and trigger gameplay logic — fail-CLOSED instead.
            if (es == null) return true;
            GameObject? selected = es.currentSelectedGameObject;
            if (selected == null) return false;
            if (selected.TryGetComponent<InputField>(out _))
                return true;
            if (selected.TryGetComponent<Il2CppTMPro.TMP_InputField>(out _))
                return true;
            return false;
        }
        catch
        {
            return true;
        }
    }
}
