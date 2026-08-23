---
name: schedule1-persistence
description: >-
  Persistence runbook for Schedule I v0.4.6f13 (SafeStorage atomic .bak, slot-isolated saves, save-load timing, ModConfig TOML limits). Use when saving configs, player data, placed-world objects, or fixing save-load desyncs, slot leaks, or .bak corruption. Covers NotesApp/CalculatorApp/BankApp/BusinessIncome/HomelessMod/MoreSaveSlots patterns.
  Keywords: SafeStorage, SaveAtomic, SaveTextAtomic, LoadSafe, slot, SaveSlotNumber, TryMigrateLegacy, GameLifecycle, OnSaveInfoLoaded, OnLoadComplete, OnSaveComplete, TOML, ConfigJsonStore.
---

# Schedule I — Persistence Skill (Save, Config & Slot Isolation)

This skill is the **single source for every file write** — configs, player notes, economy history, placed-world state. It codifies atomic I/O (`Shared/SafeStorage.cs`), slot-isolated naming, lifecycle-timed serialization, and TOML workarounds verified across 8/12 active mods.

> **Version check (last verified: 2026-08-21):** `S1Mods.Shared.SafeStorage` (SaveAtomic/SaveTextAtomic + .bak), `S1API.Lifecycle.GameLifecycle`. If the game patched, re-verify `LoadManager.ActiveSaveInfo` and `GameLifecycle` hooks.

---

## 1. The Rule: Atomic + Slot-Isolated + Lifecycle-Timed

| Aspect | Rule | Violation consequence |
|---|---|---|
| **Atomic** | `SafeStorage.SaveAtomic/SaveTextAtomic` only — never `File.WriteAllText` / `File.Copy+Replace` | Crash mid-write → `Game.json`/`config.json` corrupted → save loss (MoreSaveSlots 2026-08-21) |
| **Slot-Isolated** | `…_slot_{LoadManager.ActiveSaveInfo.SaveSlotNumber}.json` per save | Slot-A state leaks into Slot-B (`CalculatorApp` global leak 2026-08-21) |
| **Timed** | In-memory during gameplay, serialize only on `GameLifecycle.OnSaveComplete`; load on `OnSaveInfoLoaded`/`OnLoadComplete` | Alt+F4 dupe (HomelessMod placed item kept on disk but inventory reverted), empty lists at `OnGameplaySceneLoaded` |

---

## 2. Decision Tree

```
Need to persist?
│
├─ Config / generic JSON (settings, history, state)?
│  → SafeStorage atomic (see §3) + slot suffix if per-save (see §4)
│
├─ Placed world objects that affect save integrity (HomelessMod street items)?
│  → In-memory `_activeStreetObjects` + slot-isolated JSON + flush ONLY on OnSaveComplete (§5)
│
├─ Data loaded from static lists (Property.OwnedProperties, Business.OwnedBusinesses)?
│  → Don't read at OnGameplaySceneLoaded — use GameLifecycle hooks (§6)
│
└─ Dictionary<List> config property resets after restart?
   → ModConfig TOML limit → JSON sidecar ConfigJsonStore (§7)
```

---

## 3. SafeStorage Atomic I/O (Shared/SafeStorage.cs)

```csharp
using S1Mods.Shared;

// Write — JSON object
bool ok = SafeStorage.SaveAtomic(filePath, myObject, Mod.Log);

// Write — raw text (Game.json, config.json)
bool ok = SafeStorage.SaveTextAtomic(filePath, jsonString, Mod.Log);
// intern: EnsureDirectoryForFile → Write .tmp → Copy .bak → Move .tmp→target

// Read — with .bak fallback
var data = SafeStorage.LoadSafe<MyData>(filePath, fallback: new MyData(), log: Mod.Log);
string txt = SafeStorage.LoadTextSafe(filePath, fallback: "", log: Mod.Log);

// Path helper
string path = SafeStorage.GetUserDataPath("MyMod", $"state_slot_{slot}.json");
```

* **Never** `File.WriteAllText` / `File.Copy(file, bak) + File.WriteAllText(tmp) + File.Replace`. MoreSaveSlots `Config/Services` migrated 2026-08-21 (`MoreSaveSlotsConfig.cs:54`, `SaveRenameService.cs:69`).
* **Always** pass non-null fallback to `LoadSafe<T>` where `T:class`.
* Corruption test: corrupt `UserData/MyMod/state_slot_1.json` → restart → `.bak` auto-loads.

---

## 4. Slot-Isolation (Save-Slot Awareness)

**When:** Every mod that persists *per-save* state (notes, calculator history, bank transactions, payout markers, street items). Global files = slot leak.

**Pattern (reference: `CalculatorState.cs:65-93`, `NotesApp.cs:111`, `PayoutStateStore.cs:23`, `StreetPropertyManager.cs:148`, `BankState`):**

```csharp
using Il2CppScheduleOne.Persistence;

private static string _lastKnownSlot = "default";
public static string GetActiveSlotSuffix() {
    try {
        var info = LoadManager.Instance?.ActiveSaveInfo;
        if (info != null) { _lastKnownSlot = info.SaveSlotNumber.ToString(); return _lastKnownSlot; }
    } catch {}
    return _lastKnownSlot;
}
public static string GetStateFilePath() {
    string s = GetActiveSlotSuffix();
    string path = SafeStorage.GetUserDataPath("MyMod", $"state_slot_{s}.json");
    TryMigrateLegacy(path);
    return path;
}
private static void TryMigrateLegacy(string slotPath) {
    string legacy = SafeStorage.GetUserDataPath("MyMod", "state.json"); // pre-slot file
    if (!File.Exists(legacy)) return;
    if (File.Exists(slotPath)) { File.Delete(legacy); return; }
    File.Move(legacy, slotPath);
}
```

`Load()` / `Save()` / `GetStateFilePath()` must all use the slot suffix. `Reset()` clears `_cachedState` + snapshot fields on `OnPreLoad` / `OnSceneWasUnloaded`.

---

## 5. World-Object Anti-Dupe (HomelessMod Pattern)

```csharp
// Runtime — in-memory only
StreetPropertyManager.RegisterStreetItem(placedObj, itemId, guid);   // go → _activeStreetObjects
StreetPropertyManager.UnregisterStreetItem(go);                      // pack-up

// Persist — ONLY on save
GameLifecycle.OnSaveComplete += () => StreetPropertyManager.SaveStreetItems(); // slot_{n}.json SaveAtomic
GameLifecycle.OnPreLoad      += () => StreetPropertyManager.ResetState();     // clear lists, destroy clones
GameLifecycle.OnLoadComplete += () => StreetPropertyManager.LoadAndSpawnStreetItems(); // slot file → Instantiate
```

* No `Save()` in `Place()` / `PackUp()` — else Alt+F4 reverts inventory but disk keeps item → infinite dupe.
* Save file: `UserData/HomelessMod/street_items_slot_{n}.json` (`StreetPropertyManager.cs:148`).

---

## 6. Save-Load Timing (Static Lists Empty)

Static lists `Property.OwnedProperties`, `Business.OwnedBusinesses`, `NPCManager.Registered` are **empty** at `OnGameplaySceneLoaded`. Use:

```csharp
// Mod.cs OnInitializeMelon:
GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded; // after save-info parsed, BEFORE scene build → refresh Property/Item caches
GameLifecycle.OnLoadComplete   += OnLoadComplete;   // after scene build → spawn UI / managers / world objects
GameLifecycle.OnSaveComplete   += OnSaveComplete;   // user pressed Save → flush to disk
GameLifecycle.OnPreLoad        += OnPreLoad;        // before new save loads → clear caches
```

* Reference fix: PotScanner v0.2.1 dropped 25s retry by switching to `OnSaveInfoLoaded` (`schedule1-troubleshooting §5`).
* Also cache in `OnSceneWasLoaded("Main")` for in-session `ApplyStackLimits` retries.

---

## 7. ModConfig TOML Limit → JSON Sidecar

`ModConfig<T>` (MelonPreferences) **cannot** persist `Dictionary<K,V>` / `List<T>` — `CreateEntry` silently drops them. `BusinessIncome` `PropertyMultipliers` reset every restart until 2026-08-20.

**Fix — ConfigJsonStore sidecar:**

```csharp
// Services/ConfigJsonStore.cs
public static class ConfigJsonStore {
    private static readonly string Path = SafeStorage.GetUserDataPath("MyMod", "config_extra.json");
    public static void ApplyToConfig(MyConfig cfg) {
        var stored = SafeStorage.LoadSafe<MyConfig>(Path, null, Mod.Log);
        if (stored?.MyDict != null && stored.MyDict.Count>0) cfg.MyDict = stored.MyDict;
    }
    public static void Save(MyConfig cfg) => SafeStorage.SaveAtomic(Path, cfg, Mod.Log);
}
// Mod.cs:
Config = ModConfig<MyConfig>.Instance; ConfigJsonStore.ApplyToConfig(Config);
// on biz set: ModConfig.SetAndSave(key,val); ConfigJsonStore.Save(Config);
```

---

## 8. Verification Checklist

- [ ] Every `File.WriteAllText / Copy / Replace` replaced by `SafeStorage`?
- [ ] Every per-save file uses `slot_{n}.json` + `TryMigrateLegacy`?
- [ ] Placed objects: in-memory during play, SaveAtomic only on `OnSaveComplete`, Reset on `OnPreLoad`?
- [ ] Static lists read only in `OnSaveInfoLoaded` / `OnLoadComplete`, not at `OnGameplaySceneLoaded`?
- [ ] `LoadSafe` fallback is non-null? `.bak` recovery tested by corrupting one file?
- [ ] Dictionary/List config handled via `ConfigJsonStore` sidecar, not ModConfig alone?
- [ ] `Reset()` clears cached state + snapshot on scene unload?

---

## 9. References

* `references/safestorage-atomic.md` — `.tmp/.bak` flow + `SaveRenameService` rename
* `references/slot-isolation.md` — suffix derivation + migration + `PayoutStateStore` snapshot revert
* `references/save-load-timing.md` — lifecycle diagram + PotScanner fix
* External: `Knowledge/Game-Reference/Analysis/Systems/02-Save-Persistence.md`, `Source-Export/75-Save-Fields.md` + `80-Save-JSON-Schema.md`, `S1API.Lifecycle` `Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Lifecycle/`
* Live code: `Shared/src/SafeStorage.cs`, `CalculatorApp/src/CalculatorState.cs:65`, `BankApp/Services/TransactionHistoryService.cs`, `BusinessIncome/Services/PayoutStateStore.cs`, `HomelessMod/src/Building/StreetPropertyManager.cs`, `MoreSaveSlots/src/Services/SaveRenameService.cs`
