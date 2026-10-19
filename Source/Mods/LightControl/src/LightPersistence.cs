using System;
using System.IO;
using LightControl.Core;
using S1Mods.Shared;

namespace LightControl;

/// <summary>
/// Slot-scoped disk adapter for the pure JSON data API. The file path is derived from the active
/// slot number captured at load time only; a negative slot never yields a path, a read that cannot
/// produce text is reported as a failure (never as "missing data"), and a write is only attempted
/// while the captured slot still matches the game's active slot.
/// </summary>
internal sealed class LightPersistence
{
    private const string ModName = "LightControl";

    private readonly int _slot;
    private readonly string _path;
    private readonly ModLogger _log;

    public LightPersistence(int slot, string saveId, ModLogger log)
    {
        _slot = slot;
        _log = log;
        _path = SafeStorage.GetUserDataPath(ModName, $"lamps_{saveId}.json");
    }

    /// <summary>Loads the captured slot's lamp data, distinguishing missing from unreadable.</summary>
    public bool Load(JsonStatePersistence persistence)
    {
        bool mainExists = File.Exists(_path);
        bool backupExists = File.Exists(_path + ".bak");
        string? text = null;
        if (mainExists || backupExists) text = SafeStorage.LoadTextSafe(_path, string.Empty, _log);

        switch (PersistenceSlotPolicy.DecideLoad(mainExists, backupExists, text))
        {
            case PersistenceLoadAction.ConfirmMissing:
                return persistence.ConfirmDataMissing().Success;
            case PersistenceLoadAction.ReadFailure:
                _log.Warn("Lamp data could not be read; writes stay blocked and the existing file is kept.");
                return persistence.ReportReadFailure().Success;
            default:
                return persistence.LoadJson(text!).Success;
        }
    }

    /// <summary>
    /// Writes the complete JSON atomically. Refuses when the game reports a different active slot
    /// (a stale slot must never be written), when the write gate is not ready, or on any I/O error.
    /// </summary>
    public bool Save(JsonStatePersistence persistence)
    {
        int active = SaveSlots.GetActiveSlotNumber();
        // Strict guard: the captured slot must always match the active slot, including when the game
        // reports no active slot (-1). A negative slot never authorizes a write.
        if (!PersistenceSlotPolicy.CanWrite(_slot, active))
        {
            _log.Warn($"Active save slot {active} no longer matches the captured slot {_slot}; lamp data was not written.");
            return false;
        }

        string json;
        try { json = persistence.CreateJsonForSave(); }
        catch (Exception ex) { _log.Warn("Lamp data could not be serialized; existing storage is kept", ex); return false; }
        return SafeStorage.SaveTextAtomic(_path, json, _log);
    }
}
