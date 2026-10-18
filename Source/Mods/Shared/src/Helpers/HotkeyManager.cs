using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace S1Mods.Shared;

/// <summary>
/// Unique hotkey binding consisting of a key (KeyCode) and modifier keys (Ctrl/Alt/Shift).
/// </summary>
public readonly record struct HotkeyBinding(KeyCode Key, HotkeyManager.Modifiers Modifiers = HotkeyManager.Modifiers.None);

/// <summary>
/// Hotkey router based on the Unity legacy input system (KeyCode).
/// Register() starts a self-update loop (MelonCoroutines), optionally with
/// modifier combination and cooldown. Hotkeys do not fire when a text field
/// is focused (chat/config conflicts).
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
    /// Registers a hotkey with optional modifiers (Ctrl, Alt, Shift).
    /// </summary>
    /// <param name="key">The main key.</param>
    /// <param name="modifiers">Required modifier keys.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="cooldownSeconds">Cooldown in seconds between triggers.</param>
    /// <param name="strictModifiers">When true (default), the hotkey does not fire if additional non-required modifiers are pressed (e.g. shift blocks a pure F5 hotkey).</param>
    public void Register(KeyCode key, Modifiers modifiers, Action action, float cooldownSeconds = 0f, bool strictModifiers = true)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(HotkeyManager));
        if (action == null)
        {
            _log.Warn($"Register({key}, {modifiers}) ignored with null action.");
            return;
        }

        var binding = new HotkeyBinding(key, modifiers);
        if (_entries.ContainsKey(binding))
            _log.Warn($"{key} (+{modifiers}) already registered — will be overwritten.");

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

    /// <summary>Removes a specific key + modifier binding.</summary>
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

    /// <summary>Removes all bindings for this KeyCode (all modifier variants).</summary>
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
    /// Manual entry point for mods that drive their own OnUpdate.
    /// Zero-allocation in the frame loop.
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
                _log.Error($"Hotkey action '{entry.Key}' (+{entry.Modifiers}) failed", ex);
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
            _log.Warn($"Self-update hook failed: {ex.Message} — call Update() manually.");
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
                _log.Warn($"Self-update failed: {ex.Message}");
            }
            yield return null;
        }
    }

    /// <summary>
    /// Checks whether the pressed modifiers match the required ones.
    /// With strictModifiers = true (default), no additional modifier may be pressed (e.g. F5 does not fire while shift is held).
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
