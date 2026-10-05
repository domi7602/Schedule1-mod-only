---
name: schedule1-persistence
description: >-
  Persistence runbook for Schedule I v0.4.7f9 (SafeStorage atomic .bak, slot-isolated saves, save-load timing, ModConfig TOML limits). Use when saving configs, player data, placed-world objects, or fixing save-load desyncs, slot leaks, or .bak corruption. Covers NotesApp/CalculatorApp/BankApp/BusinessIncome/HomelessMod/MoreSaveSlots patterns.
  Keywords: SafeStorage, SaveAtomic, SaveTextAtomic, LoadSafe, slot, SaveSlotNumber, TryMigrateLegacy, GameLifecycle, OnSaveInfoLoaded, OnLoadComplete, OnSaveComplete, TOML, ConfigJsonStore.
---

> Version anchor: Game v0.4.7f9 / S1API 3.2.1-beta.8 (deployed 2026-10-05; in-repo ThirdParty/S1API source = beta.8 tag (checked out 2026-10-05, commit f65ae40 = deployed build)) / MelonLoader 0.7.3 (versions verified 2026-10-05 against live install: Latest.log Game Version 0.4.7f9 + MelonLoader v0.7.3 Open-Beta + S1API product 3.2.1-beta.8, Steam buildid 25698382; content NOT re-verified after the 0.4.7f9 update - verify API details against live Il2CppAssemblies). Re-check after any game or S1API update.

# Schedule I — Persistence Skill (Save, Config & Slot Isolation)

This skill is the **single source for every file write** — configs, player notes, economy history, placed-world state. It codifies atomic I/O (`Shared/SafeStorage.cs`), slot-isolated naming, lifecycle-timed serialization, and TOML workarounds verified across active workspace mods.

> **Version check (verified 2026-09-11):** `S1Mods.Shared.SafeStorage` (SaveAtomic/SaveTextAtomic + .bak), `S1API.Lifecycle.GameLifecycle`. IL2CPP lifecycle and slot safety rules incorporate the 2026-09-11 bugfix audit.

---

## 1. The Rule: Atomic + Slot-Isolated + Lifecycle-Timed

| Aspect | Rule | Violation consequence |
|---|---|---|
| **Atomic** | `SafeStorage.SaveAtomic/SaveTextAtomic` only — never `File.WriteAllText` / `File.Copy+Replace` | Crash mid-write → `Game.json`/`config.json` corrupted → save loss (MoreSaveSlots) |
| **Slot-Isolated** | `…_slot_{info.SaveSlotNumber}.json` per save (guard `SaveSlotNumber >= 0`) | Slot-A state leaks into Slot-B, or `slot_-1.json` is generated |
| **Timed** | In-memory during gameplay, serialize only on `GameLifecycle.OnSaveComplete`; load on `OnLoadComplete` (**not** `OnSaveInfoLoaded` — fires 0× on 0.4.7f6+, verified 2026-09-29) | Alt+F4 dupe (HomelessMod placed item kept on disk but inventory reverted), empty lists at `OnGameplaySceneLoaded` |

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

* **Never** use `File.WriteAllText` or `File.Copy(file, bak) + File.WriteAllText(tmp) + File.Replace`.
* **Always** pass non-null fallback to `LoadSafe<T>` where `T:class`.
* Corruption test: corrupt `UserData/MyMod/state_slot_1.json` → restart → `.bak` auto-loads.

---

## 4. Slot-Isolation (Save-Slot Awareness)

**When:** Every mod that persists *per-save* state (notes, calculator history, bank transactions, payout markers, street items). Global files = slot leak.

**Robust Derivation (canonical code in [`references/slot-isolation.md`](references/slot-isolation.md)):**

Triple IL2CPP Guard (`loadMgr != null && Pointer != IntPtr.Zero && !WasCollected` × 2 Levels) + `SaveSlotNumber >= 0` (menu/transition can report `-1` — never write `slot_-1.json`) + `_lastKnownSlot` fallback + `TryMigrateLegacy` (legacy global file → slot file, once).

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
// BROKEN on 0.4.7f6+ (verified 2026-09-29): OnSaveInfoLoaded fires 0× — do NOT subscribe to it.
GameLifecycle.OnPreLoad        += OnPreLoad;        // before save data loads → clear caches / reset state
GameLifecycle.OnLoadComplete   += OnLoadComplete;   // after scene build → refresh Property/Item caches, spawn UI / managers / world objects
GameLifecycle.OnSaveComplete   += OnSaveComplete;   // user pressed Save → flush to disk
// Verified order (schedule1-lifecycle-verify §7): OnSceneWasLoaded("Main") → OnPreLoad → OnLoadComplete
```

* Reference fix: PotScanner v0.2.1 (game 0.4.6f13-era) dropped its 25s retry via the lifecycle hook; on 0.4.7f9 the equivalent is `OnLoadComplete` (`schedule1-troubleshooting §5`).
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

**Suppress the startup warning (2026-09-01, ModConfig API):** undecleared Dictionary/List fields log a `[WARNING] not TOML-mappable` on every startup. Declare sidecar-managed fields so they log a neutral Info instead (`ModConfig.cs` `Initialize`):

```csharp
ModConfig<MyConfig>.Initialize("MyMod", Log,
    sidecarManagedProperties: new[] { "MyDict", "MyList" }); // before ConfigJsonStore.ApplyToConfig
```

---

## 8. Verification Checklist

- [ ] Every `File.WriteAllText / Copy / Replace` replaced by `SafeStorage`?
- [ ] Every per-save file uses `slot_{n}.json` + `TryMigrateLegacy`?
- [ ] Placed objects: in-memory during play, SaveAtomic only on `OnSaveComplete`, Reset on `OnPreLoad`?
- [ ] Static lists read in `OnLoadComplete` (not at `OnGameplaySceneLoaded`) — `OnSaveInfoLoaded` fires 0× on 0.4.7f6+, not used for refresh?
- [ ] `LoadSafe` fallback is non-null? `.bak` recovery tested by corrupting one file?
- [ ] Dictionary/List config handled via `ConfigJsonStore` sidecar, not ModConfig alone?
- [ ] `Reset()` clears cached state + snapshot on scene unload?

---

## 9. References

* `references/safestorage-atomic.md` — `.tmp/.bak` flow + atomic write pattern
* `references/slot-isolation.md` — suffix derivation + migration + snapshot revert + triple guard
* `references/buildable-restore.md` — BuildableItem duplicate-spawn rule (unique content, restored 2026-10-05)
* Timing (canonical): `schedule1-troubleshooting/references/save-load-timing.md` — lifecycle order + PotScanner history (the old `references/save-load-timing.md` copy here was removed 2026-10-05 to end the double-file drift)
* In-Repo Architecture: `Skills/schedule1-game-systems/references/02-Save-Persistence.md`
* Framework Source: `ThirdParty/S1API/S1API/Lifecycle/`
* Live Mod Implementations:
  - `Source/Mods/Shared/src/SafeStorage.cs`
  - `Source/Mods/CalculatorApp/src/CalculatorState.cs`
  - `Source/Mods/NotesApp/src/NotesApp.cs`
  - `Source/Mods/BankApp/src/TransactionHistoryService.cs`
  - `Source/Mods/BusinessIncome/src/PayoutStateStore.cs`
  - `Source/Archive/HomelessMod/src/StreetPropertyManager.cs`
  - `Source/Mods/MoreSaveSlots/src/SaveRenameService.cs`
