using Il2CppScheduleOne.UI;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(ConsoleHotkey.ConsoleHotkeyMod), "ConsoleHotkey", "0.1.0", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace ConsoleHotkey;

/// <summary>
/// Adds an explicit F1 hotkey for the vanilla developer console.
///
/// The game binds its console to the Input System backtick action. On a German
/// XKB layout that physical key emits a dead key, so Proton never sees the
/// backtick the game expects. This mod bypasses the Input System entirely and
/// drives the same ConsoleUI component that the backtick action drives.
/// </summary>
public class ConsoleHotkeyMod : MelonMod
{
    private const string ConsolePath = "UI/Console";

    private bool _missingLogged;
    private bool _disabledWarningLogged;

    public override void OnInitializeMelon()
    {
        MelonLogger.Msg("[ConsoleHotkey] initialisiert (v0.1.0) - F1 toggelt die Entwicklerkonsole.");
    }

    public override void OnUpdate()
    {
        if (!Input.GetKeyDown(KeyCode.F1))
        {
            return;
        }

        // Deliberately not using HotkeyManager.IsInputFieldFocused(): while the
        // console is open its TMP input field has focus, and F1 must still close it.
        ConsoleUI console = FindConsole();
        if (console == null)
        {
            if (!_missingLogged)
            {
                MelonLogger.Warning($"[ConsoleHotkey] ConsoleUI not found at '{ConsolePath}'. Is the dev console enabled in the game settings?");
                _missingLogged = true;
            }
            return;
        }

        bool isOpen = console.canvas != null && console.canvas.enabled;
        console.SetIsOpen(!isOpen);

        if (!console.IsConsoleEnabled && !_disabledWarningLogged)
        {
            MelonLogger.Warning("[ConsoleHotkey] ConsoleEnabled is false in GameSettings; the UI was toggled but commands may be rejected.");
            _disabledWarningLogged = true;
        }
    }

    private static ConsoleUI FindConsole()
    {
        GameObject root = GameObject.Find(ConsolePath);
        if (root != null)
        {
            ConsoleUI component = root.GetComponent<ConsoleUI>();
            if (component != null)
            {
                return component;
            }
        }

        return UnityEngine.Object.FindObjectOfType<ConsoleUI>();
    }
}
