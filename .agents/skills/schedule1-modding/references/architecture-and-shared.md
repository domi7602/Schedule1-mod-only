# S1Mods.Shared & Architecture Guidelines

`S1Mods.Shared` is the central core library shared across all mods in the workspace (`Source/Mods/Shared/`).

---

## 1. SafeStorage (`SafeStorage.cs`)
Handles fault-tolerant atomic JSON persistence with automatic backup and crash recovery.

```csharp
using S1Mods.Shared;

// Atomic save with .bak rotation (JSON object)
SafeStorage.SaveAtomic(filePath, myObject, Mod.Log);

// Text atomic (e.g. Game.json, config.json)
SafeStorage.SaveTextAtomic(filePath, jsonString, Mod.Log);

// Safe load with fallback to .bak if primary JSON is corrupted
var data = SafeStorage.LoadSafe<MyData>(filePath, fallback: new MyData(), log: Mod.Log);
string txt = SafeStorage.LoadTextSafe(filePath, fallback: "", log: Mod.Log);
```

* Prevents data corruption during sudden game crashes or alt-F4 — writes `.tmp` → `Copy .bak` → `Move .tmp → target`.
* **Mandatory for:** mod configs, player notes, calculator history, persistent states, **and** `Game.json` renames (`MoreSaveSlots/SaveRenameService.cs:69` migrated 2026-08-21 from `File.Copy/Write/Replace` to `SaveTextAtomic`), `MoreSaveSlotsConfig.cs:54`.
* **Gotcha:** `LoadSafe<T>(path, fallback)` requires a non-null fallback for `T : class` — pass `new T()` / `new List<T>()`, never `null`.
* **Slot-Isolation (2026-08-21):** Every per-save file must be `slot_{SaveSlotNumber}.json` (`LoadManager.Instance.ActiveSaveInfo.SaveSlotNumber`). Reference patterns: `NotesApp.cs:111 GetSaveSlotSuffix`, `BusinessIncome/Services/PayoutStateStore.cs:23 GetActiveSlotSuffix`, `HomelessMod/StreetPropertyManager.cs:148`. Add `TryMigrateLegacy(slotPath)` moving `legacy.json → slot_{n}.json` once (`CalculatorState.cs:93`, `NotesApp.cs:260`). Non-slot file = slot-leak bug (`CalculatorState.cs:66` fixed 2026-08-21).

---

## 2. InputFocus Protection (`NotesAppInputFocus` / `HotkeyManager`)
When user focuses on an `InputField` or `TMP_InputField`, vanilla player movement (WASD, hotbars, phone toggles) must be suspended.

* Implement an `IInputFocus` or subscribe to `HotkeyManager.IsInputFieldFocused()`.
* On focus enter: Block character input & pause hotkey routing.
* On focus exit / submit / cancel: Restore normal game input.

---

## 3. ModConfig & MelonPreferences
Use `ModConfig<T>` to bind strongly-typed C# models to `UserData/MelonPreferences.cfg` or per-mod `config.json`.

* Store user configurable keys (e.g., toggle HUD, custom hotkeys, scale, opacity).
* Validate values upon loading (clamp numeric ranges, fallback to defaults on null).

### ⚠️ TOML Limitation (empirical, 2026-08-20)
`ModConfig<T>` (MelonPreferences) **cannot persist `Dictionary<K,V>` / `List<T>` properties** — `CreateEntry` throws "not TOML-mappable" and the property is silently dropped (logged once). Example: `BusinessIncomeConfig.PropertyMultipliers` was reset every restart.

**Fix pattern — JSON sidecar via SafeStorage:**
```csharp
// Services/ConfigJsonStore.cs
public static class ConfigJsonStore
{
    private static readonly string SavePath = SafeStorage.GetUserDataPath("MyMod", "config_extra.json");

    public static void ApplyToConfig(MyConfig cfg)  // merge dictionaries after ModConfig.Initialize
    {
        var stored = SafeStorage.LoadSafe<MyConfig>(SavePath, null, Mod.Log);
        if (stored?.MyDict != null && stored.MyDict.Count > 0) cfg.MyDict = stored.MyDict;
    }

    public static void Save(MyConfig cfg) => SafeStorage.SaveAtomic(SavePath, cfg, Mod.Log);
}
```
Call `ApplyToConfig(ModConfig<T>.Instance)` right after `ModConfig<T>.Initialize(...)` and `ConfigJsonStore.Save(...)` after every `SetAndSave`.

---

## 4. ModLogger & PatchGuard
* **`ModLogger`**: Namespaced, structured logging (`[ModName] [INFO/WARN/ERROR]`).
* **`PatchGuard`**: Wraps Harmony patches to handle IL2CPP version shifts safely without hard crashing the game runtime if a target method signature changed in a minor game patch.

---

## 5. Transaction Ordering for Money/Economy (empirical, 2026-08-20/21)

**Problem:** Executing a bank transaction *before* persisting the "paid" marker means a disk-write failure after a successful transfer → the next day-pass/retry pays again = money duplication.

**Safe ordering (Memory-First + Snapshot Revert):**
```csharp
// 1. Mark paid IN MEMORY first (snapshots prevLastPaid + per-business Dict)
PayoutStateStore.MarkInMemoryPaid(day, businessIds);

// 2. Execute the real transaction
try { Money.CreateOnlineTransaction(...); }
catch { PayoutStateStore.RevertInMemoryPaid(day, businessIds); return false; } // restores snapshot, NOT -1

// 3. Persist to disk (failure here only loses the marker on disk; memory keeps it set)
PayoutStateStore.CommitPayout(day, businessIds); // clears pending snapshot on success
```

**Why snapshot (2026-08-21):** `Revert` set `LastPaid=-1` → lost history Day 5 → Day 5 re-paid after Day 6 fail. Fix: `Mark` snapshots `_pendingPrevLastPaid` + `_pendingPrevPerBusiness` (`PayoutStateStore.cs:20-21`), `Revert` restores them (`PayoutStateStore.cs:149-160`), `Reset` clears snapshot.

**Corollary — Atomic Purchase (PocketShop):**
1. Validate & price first (`CanAfford`, `CanItemFitInInventory`).
2. **Pre-create ALL item instances** BEFORE payment.
3. Execute payment in its own `try/catch` — **never refund if payment never executed** (would create money).
4. Inventory transfer in its own `try/catch` — on failure, refund (cash `ChangeCashBalance(+total)` / bank `CreateOnlineTransaction(+total)`), return failure.
5. Decrement stock only after successful transfer.
