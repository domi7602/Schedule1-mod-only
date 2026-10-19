using System;
using System.IO;
using System.Text.Json;
using PropertyLEDs.Core;
using S1Mods.Shared;

namespace PropertyLEDs;

/// <summary>
/// I/O adapter only: every decision (slot identity, debounce, schema tri-state) lives in the
/// Unity-free, unit-tested types under <c>PropertyLEDs.Core</c>. The destination path is captured
/// once at load time and may only be written while the active slot still equals the loaded slot.
/// No file is created or overwritten in a menu, and an unreadable or future-schema file is kept.
/// </summary>
internal sealed class LEDSettingsStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private readonly Func<int> _activeSlot;
    private readonly Func<double> _clock;
    private readonly Func<int, string> _pathForSlot;
    private readonly Func<string, string?> _readText;
    private readonly Func<string, string, bool> _writeText;
    private readonly Action<string> _warn;
    private const double DebounceSeconds = 1.0;

    private SlotSession? _session;

    internal LEDSettingsStore(Func<int> activeSlot, Func<double> clock, Func<int, string> pathForSlot,
        Func<string, string?> readText, Func<string, string, bool> writeText, Action<string> warn)
    {
        _activeSlot = activeSlot;
        _clock = clock;
        _pathForSlot = pathForSlot;
        _readText = readText;
        _writeText = writeText;
        _warn = warn;
    }

    internal static LEDSettingsStore Create(Func<double> clock)
        => new LEDSettingsStore(
            SaveSlots.GetActiveSlotNumber,
            clock,
            slot => SafeStorage.GetUserDataPath("PropertyLEDs", $"settings_slot_{slot}.json"),
            // Missing main AND .bak => null, which stays distinguishable from an empty/corrupt file.
            // .bak recovery stays in SafeStorage and is never re-implemented here.
            path => File.Exists(path) || File.Exists(path + ".bak")
                ? SafeStorage.LoadTextSafe(path, string.Empty, Mod.Log) : null,
            (path, json) => SafeStorage.SaveTextAtomic(path, json, Mod.Log),
            Mod.Log.Warn);

    internal int LoadedSlot => _session?.SlotNumber ?? -1;
    internal bool ReadOnly => _session == null || !_session.CanWrite;
    internal bool SlotMatches => _session != null && LoadedSlot >= 0 && _activeSlot() == LoadedSlot;
    internal bool CanEdit => _session?.CanWrite == true && SlotMatches;
    internal bool Dirty => _session?.IsDirty ?? false;
    internal string? CapturedPath { get; private set; }

    /// <summary>Deep snapshot; callers never receive the live session state.</summary>
    internal LEDState State => _session?.Snapshot() ?? new LEDState();

    internal LEDSettings GetEffective(string propertyId, string? roomId = null)
        => State.GetEffective(propertyId, roomId);

    internal bool HasOverride(string propertyId, string roomId)
    {
        var state = State;
        return state.Properties != null && state.Properties.TryGetValue(propertyId, out var property) &&
            property?.Rooms != null && property.Rooms.ContainsKey(roomId);
    }

    internal bool SetGlobal(string propertyId, LEDSettings settings)
        => _session?.SetGlobal(propertyId, settings, _clock()) == true;

    internal bool SetRoom(string propertyId, string roomId, LEDSettings settings)
        => _session?.SetRoom(propertyId, roomId, settings, _clock()) == true;

    internal bool UseProperty(string propertyId, string roomId)
        => _session?.UseProperty(propertyId, roomId, _clock()) == true;

    /// <summary>
    /// Loads the current save slot. Returns false without touching any file when no slot is active
    /// yet or when the slot changed while reading.
    /// </summary>
    internal bool Load()
    {
        Reset();
        int slot = _activeSlot();
        if (slot < 0) return false;
        string path = _pathForSlot(slot);
        StateLoadResult load;
        try { load = StateCodec.Read(_readText(path)); }
        catch (Exception ex)
        {
            _warn($"LED settings could not be read for slot {slot}: {ex.Message}");
            return false;
        }
        // The read may cross a transition: never publish (or later write) data for another slot.
        if (_activeSlot() != slot) return false;
        // 1 s debounce: a slider drag or any burst of edits produces at most one write per second.
        _session = new SlotSession(slot, load, DebounceSeconds);
        CapturedPath = path;
        switch (load.Status)
        {
            case StateLoadStatus.Missing:
                Mod.Log.Info($"No LED settings for slot {slot} yet; defaults active, file is created on the first change.");
                break;
            case StateLoadStatus.UnsupportedVersion:
                _warn($"LED settings schema for slot {slot} is newer than this build; file preserved read-only, LEDs stay off.");
                break;
            case StateLoadStatus.Invalid:
                _warn($"LED settings for slot {slot} are invalid/corrupt; file preserved read-only, defaults shown in memory.");
                break;
        }
        return true;
    }

    internal bool Flush(bool force = true)
    {
        if (_session == null || CapturedPath == null) return false;
        // Pure debounce decision first; the slot guard is only probed for an actually due write.
        if (!_session.ShouldSave(_clock(), force)) return false;
        if (!CanEdit) return false;
        long revision = _session.Revision;
        bool saved;
        try { saved = _writeText(CapturedPath, JsonSerializer.Serialize(_session.Snapshot(), JsonOptions)); }
        catch (Exception ex)
        {
            _warn($"LED settings write failed: {ex.Message}");
            saved = false;
        }
        if (saved && _session.MarkSaved(revision)) return true;
        if (!saved) _warn("LED settings write failed; pending state is retained until the next attempt or teardown.");
        return false;
    }

    internal void Reset()
    {
        // Flush BEFORE dropping the captured path. A different/invalid active slot never authorizes a write.
        Flush();
        if (_session?.IsDirty == true)
            _warn($"Discarding unsaved LED settings for slot {_session.SlotNumber}; the slot guard blocked the final flush.");
        _session = null;
        CapturedPath = null;
    }
}
