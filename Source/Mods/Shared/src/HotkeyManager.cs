using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace S1Mods.Shared;

/// <summary>
/// Eindeutiges Hotkey-Binding bestehend aus Taste (KeyCode) und Modifier-Tasten (Ctrl/Alt/Shift).
/// </summary>
public readonly record struct HotkeyBinding(KeyCode Key, HotkeyManager.Modifiers Modifiers = HotkeyManager.Modifiers.None);

/// <summary>
/// Hotkey-Router auf Basis des Unity-Legacy-Input-Systems (KeyCode).
/// Register() startet einen Selbst-Update-Loop (MelonCoroutines), optional mit
/// Modifier-Kombination und Cooldown. Hotkeys feuern nicht, wenn ein Textfeld
/// fokussiert ist (Chat/Config-Konflikte).
/// </summary>
public sealed class HotkeyManager : IDisposable
{
    [Flags]
    public enum Modifiers
    {
        None = 0,
        Ctrl = 1,
        Alt = 2,
        Shift = 4
    }

    private sealed class Entry
    {
        public KeyCode Key;
        public Modifiers Modifiers;
        public Action Action = null!;
        public float Cooldown;
        public float LastFire = float.NegativeInfinity;
        public bool StrictModifiers = true;
    }

    private readonly Dictionary<HotkeyBinding, Entry> _entries = new();
    private readonly ModLogger _log;
    private Entry[] _cachedEntries = Array.Empty<Entry>();
    private bool _selfUpdating;
    private bool _disposed;

    public HotkeyManager(ModLogger log)
    {
        _log = log;
    }

    public void Register(KeyCode key, Action action, float cooldownSeconds = 0f)
        => Register(key, Modifiers.None, action, cooldownSeconds, strictModifiers: true);

    /// <summary>
    /// Registriert einen Hotkey mit optionalen Modifiern (Ctrl, Alt, Shift).
    /// </summary>
    /// <param name="key">Die Haupttaste.</param>
    /// <param name="modifiers">Geforderte Modifier-Tasten.</param>
    /// <param name="action">Die auszuführende Action.</param>
    /// <param name="cooldownSeconds">Cooldown in Sekunden zwischen Auslösungen.</param>
    /// <param name="strictModifiers">Wenn true (Standard), feuert der Hotkey nicht, wenn zusätzliche nicht geforderte Modifier gedrückt sind (z.B. blockiert Shift einen reinen F5-Hotkey).</param>
    public void Register(KeyCode key, Modifiers modifiers, Action action, float cooldownSeconds = 0f, bool strictModifiers = true)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(HotkeyManager));
        if (action == null)
        {
            _log.Warn($"Register({key}, {modifiers}) mit null-Action ignoriert.");
            return;
        }

        var binding = new HotkeyBinding(key, modifiers);
        if (_entries.ContainsKey(binding))
            _log.Warn($"{key} (+{modifiers}) bereits registriert — wird überschrieben.");

        _entries[binding] = new Entry
        {
            Key = key,
            Modifiers = modifiers,
            Action = action,
            Cooldown = cooldownSeconds,
            StrictModifiers = strictModifiers
        };

        RebuildCache();
        EnsureSelfUpdate();
    }

    /// <summary>Entfernt ein spezifisches Key + Modifier Binding.</summary>
    public void Unregister(KeyCode key, Modifiers modifiers)
    {
        var binding = new HotkeyBinding(key, modifiers);
        if (_entries.Remove(binding))
        {
            RebuildCache();
            if (_entries.Count == 0)
                StopSelfUpdate();
        }
    }

    /// <summary>Entfernt alle Bindings für diesen KeyCode (alle Modifier-Varianten).</summary>
    public void Unregister(KeyCode key)
    {
        List<HotkeyBinding>? toRemove = null;
        foreach (var k in _entries.Keys)
        {
            if (k.Key == key)
            {
                toRemove ??= new List<HotkeyBinding>();
                toRemove.Add(k);
            }
        }

        if (toRemove != null)
        {
            foreach (var k in toRemove)
                _entries.Remove(k);

            RebuildCache();
            if (_entries.Count == 0)
                StopSelfUpdate();
        }
    }

    public void UnregisterAll()
    {
        _entries.Clear();
        RebuildCache();
        StopSelfUpdate();
    }

    public bool IsRegistered(KeyCode key, Modifiers modifiers) => _entries.ContainsKey(new HotkeyBinding(key, modifiers));

    public bool IsRegistered(KeyCode key)
    {
        foreach (var k in _entries.Keys)
        {
            if (k.Key == key)
                return true;
        }
        return false;
    }

    private void RebuildCache()
    {
        if (_entries.Count == 0)
        {
            _cachedEntries = Array.Empty<Entry>();
            return;
        }

        var arr = new Entry[_entries.Count];
        _entries.Values.CopyTo(arr, 0);
        _cachedEntries = arr;
    }

    /// <summary>
    /// Manueller Entrypoint für Mods, die ihr eigenes OnUpdate treiben.
    /// 0-Allocation im Frame-Loop.
    /// </summary>
    public void Update()
    {
        if (_disposed || _cachedEntries.Length == 0 || IsInputFieldFocused())
            return;

        float now = Time.realtimeSinceStartup;
        Entry[] entries = _cachedEntries;
        int count = entries.Length;

        for (int i = 0; i < count; i++)
        {
            Entry entry = entries[i];

            if (!Input.GetKeyDown(entry.Key))
                continue;

            if (!ModifiersMatch(entry.Modifiers, entry.StrictModifiers))
                continue;

            if (entry.Cooldown > 0f && now - entry.LastFire < entry.Cooldown)
                continue;

            entry.LastFire = now;
            try
            {
                entry.Action();
            }
            catch (Exception ex)
            {
                _log.Error($"Hotkey-Action '{entry.Key}' (+{entry.Modifiers}) fehlgeschlagen", ex);
            }
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        StopSelfUpdate();
        UnregisterAll();
    }

    private void EnsureSelfUpdate()
    {
        if (_selfUpdating)
            return;
        _selfUpdating = true;
        try
        {
            MelonCoroutines.Start(SelfUpdateLoop());
        }
        catch (Exception ex)
        {
            _selfUpdating = false;
            _log.Warn($"Self-Update-Hook fehlgeschlagen: {ex.Message} — Update() manuell aufrufen.");
        }
    }

    private void StopSelfUpdate()
    {
        _selfUpdating = false;
    }

    private System.Collections.IEnumerator SelfUpdateLoop()
    {
        while (_selfUpdating && !_disposed)
        {
            try
            {
                Update();
            }
            catch (Exception ex)
            {
                _log.Warn($"Self-Update fehlgeschlagen: {ex.Message}");
            }
            yield return null;
        }
    }

    /// <summary>
    /// Prüft, ob die gedrückten Modifiers mit den geforderten übereinstimmen.
    /// Bei strictModifiers = true (Standard) darf kein zusätzlicher Modifier gedrückt sein (z.B. feuert F5 nicht bei gedrücktem Shift).
    /// </summary>
    public static bool ModifiersMatch(Modifiers required, bool strictModifiers = true)
    {
        bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        bool alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        bool reqCtrl = (required & Modifiers.Ctrl) != 0;
        bool reqAlt = (required & Modifiers.Alt) != 0;
        bool reqShift = (required & Modifiers.Shift) != 0;

        if (strictModifiers)
        {
            return (ctrl == reqCtrl) && (alt == reqAlt) && (shift == reqShift);
        }
        else
        {
            if (reqCtrl && !ctrl) return false;
            if (reqAlt && !alt) return false;
            if (reqShift && !shift) return false;
            return true;
        }
    }

    /// <summary>True, wenn ein UI-Textfeld fokussiert ist (Hotkeys sollen dann nicht feuern).</summary>
    public static bool IsInputFieldFocused()
    {
        try
        {
            var es = EventSystem.current;
            GameObject? selected = es?.currentSelectedGameObject;
            if (selected == null)
                return false;
            if (selected.TryGetComponent<InputField>(out _))
                return true;
            if (selected.TryGetComponent<Il2CppTMPro.TMP_InputField>(out _))
                return true;
            return false;
        }
        catch
        {
            return false;
        }
    }
}
