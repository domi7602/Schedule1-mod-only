# Slot Isolation

Per-save files: `slot_{SaveSlotNumber}.json`. See `CalculatorState.cs`, `NotesApp.cs`, `PayoutStateStore.cs`, `BankState.cs`.

### Robust Slot Suffix Derivation (IL2CPP Triple Guard + >= 0 Check)

```csharp
using System;
using System.IO;
using Il2CppScheduleOne.Persistence;
using S1Mods.Shared;

private static string _lastKnownSlot = "default";

public static string GetActiveSlotSuffix()
{
    try
    {
        var loadMgr = LoadManager.Instance;
        if (loadMgr != null && loadMgr.Pointer != IntPtr.Zero && !loadMgr.WasCollected)
        {
            var info = loadMgr.ActiveSaveInfo;
            if (info != null && info.Pointer != IntPtr.Zero && !info.WasCollected && info.SaveSlotNumber >= 0)
            {
                _lastKnownSlot = info.SaveSlotNumber.ToString();
                return _lastKnownSlot;
            }
        }
    }
    catch { }
    return _lastKnownSlot;
}

public static string GetStateFilePath() => SafeStorage.GetUserDataPath("MyMod", $"state_slot_{GetActiveSlotSuffix()}.json");

// Legacy once migration
void TryMigrateLegacy(string slotPath)
{
    string legacy = SafeStorage.GetUserDataPath("MyMod", "state.json");
    if (!File.Exists(legacy)) return;
    if (File.Exists(slotPath)) { File.Delete(legacy); return; }
    File.Move(legacy, slotPath);
}
```

> **CRITICAL Guard Rule:**
> 1. Check `loadMgr != null && loadMgr.Pointer != IntPtr.Zero && !loadMgr.WasCollected`.
> 2. Check `info != null && info.Pointer != IntPtr.Zero && !info.WasCollected`.
> 3. Check `info.SaveSlotNumber >= 0` — in menu or transition states, `SaveSlotNumber` can be `-1`. Without this check, files like `state_slot_-1.json` are created and slot isolation breaks.

### State Snapshot & Rollback
`PayoutStateStore.cs` additionally snapshots `_pendingPrevLastPaid` + `_pendingPrevPerBusiness` in `MarkInMemoryPaid` and restores in `Revert` — preventing double-payments or lost state on crash/failure.

### Placed World Objects
`HomelessMod`: `street_items_slot_{n}.json` is modified in-memory only and flushed to disk on `GameLifecycle.OnSaveComplete` to prevent Alt+F4 duplication exploits.

