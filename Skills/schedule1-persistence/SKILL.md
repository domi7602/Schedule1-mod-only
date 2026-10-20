---
name: schedule1-persistence
description: >-
  Persistence runbook for Schedule I (SafeStorage atomic .bak, slot-isolated saves, save-load timing, ModConfig TOML limits). Use when saving configs, player data, placed-world objects, or fixing save-load desyncs, slot leaks, or .bak corruption. Covers NotesApp/CalculatorApp/BankApp/BusinessIncome/HomelessMod/MoreSaveSlots patterns.
  Keywords: SafeStorage, SaveAtomic, SaveTextAtomic, LoadSafe, slot, SaveSlotNumber, TryMigrateLegacy, GameLifecycle, OnSaveInfoLoaded, OnLoadComplete, OnSaveComplete, TOML, ConfigJsonStore.
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — Persistence Skill (Save, Config & Slot Isolation)

This skill is the **single source for every file write** — configs, player notes, economy history, placed-world state. It codifies atomic I/O (`Shared/SafeStorage.cs`), slot-isolated naming, lifecycle-timed serialization, and TOML workarounds verified across active workspace mods.

> API check (2026-09-11): `S1Mods.Shared.SafeStorage` (`SaveAtomic` / `SaveTextAtomic` + `.bak`) and `S1API.Lifecycle.GameLifecycle` were inspected. IL2CPP lifecycle and slot-safety rules incorporate the 2026-09-11 bug-fix audit.

---

## 1. The Rule: Atomic + Slot-Isolated + Lifecycle-Timed

| Aspect | Rule | Violation consequence |
|---|---|---|
| **Atomic** | `SafeStorage.SaveAtomic/SaveTextAtomic` only — never `File.WriteAllText` / `File.Copy+Replace` | Crash mid-write → `Game.json`/`config.json` corrupted → save loss (MoreSaveSlots) |
| **Slot-Isolated** | `…_slot_{info.SaveSlotNumber}.json` per save (guard `SaveSlotNumber >= 0`) | Slot-A state leaks into Slot-B, or `slot_-1.json` is generated |
| **Timed** | Keep state in memory during gameplay; serialize on `GameLifecycle.OnSaveComplete` and load on `OnLoadComplete`. The instrumented session recorded 0 firings for `OnSaveInfoLoaded`; verify before relying on it. | Alt+F4 dupe (HomelessMod placed item kept on disk but inventory reverted), empty lists at `OnGameplaySceneLoaded` |

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
* Clear dirty/saved markers only when the atomic writer returns `true`. Freeze both snapshot and destination before clearing a view or resolving another slot; failed old-slot IO must never be retried to the new slot. See [frozen writes and personal-intent persistence](references/frozen-writes.md).
* `LoadTextSafe` recovers from text-read failures, not a later JSON/schema parse failure. Explicitly validate a backup before using it; preserve/quarantine unrecoverable or newer-schema sources instead of silently overwriting defaults.
* Corruption test: corrupt `UserData/MyMod/state_slot_1.json` → restart → `.bak` auto-loads.

---

## 4. Slot-Isolation (Save-Slot Awareness)

**When:** Every mod that persists *per-save* state (notes, calculator history, bank transactions, payout markers, street items). Global files = slot leak.

**Robust Derivation (canonical code in [`references/slot-isolation.md`](references/slot-isolation.md)):**

Use triple IL2CPP guards (`loadMgr != null && Pointer != IntPtr.Zero && !WasCollected` × 2 levels), `SaveSlotNumber >= 0`, a completed-load identity and a destination bound to that identity. Never use `_lastKnownSlot` to authorize an unknown-context write: it may describe another world. Treat the older suffix helper reference as historical naming mechanics, not authoritative identity binding. Read legacy data without moving/deleting/rewriting its source until context and validation are confirmed; a menu/save-info scan is not a completed world load.

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

### 5.1 Economy/evidence sidecars: checkpoint and unknown-effect boundaries

- Do not equate an atomic JSON write, a matching save identity or `OnSaveComplete` alone
  with an engine/sidecar transaction. Freeze both state and bound destination at save-start;
  verify native success/destination before commit. Compare the exact world checkpoint when
  recovering a primary/backup file: same identity can still be a different rolled-back world.
- Verify both native `Save()` overload capture and actual property/event ordering in-game.
  Normalize only proven path forms; do not weaken identity/path gates after a transient read
  error. Start a bounded load deadline once, and retry installation without resetting it.
- Treat content fingerprints as a provisional consistency design until real load/save
  roundtrips prove stability. Exclude metadata timestamp churn, but account explicitly for
  foreign root sidecars and load-time native rewrites; unknown content must not silently
  choose the latest mod state. Initial legacy alignment cannot be reconstructed from nothing.
- Separate Missing, Present and Unknown evidence. A failed read is not an integer zero or
  proof of absence; ambiguous legacy photos and incomplete scans must defer repair rather
  than duplicate it. Make receipt, repair and cleanup use the same compatibility policy.
- Latch a payment attempt before calling a fallible native provider. An exception may be
  after cash mutation: retain an explicit unresolved claim/photo and disable blind retry,
  expiry and forfeiture pending balance review. A normal void return still is not an
  independently observed balance change. Keep post-payment logging outside the retry decision.
- Trace lifecycle/retry/status helpers to real callsites, including before readiness gates.
  Green pure helpers do not prove that startup registration or unsupported-state UI runs.
  The HitmanPhone audit verification record separates source/delegate evidence from still-open
  native acceptance; do not promote its candidate checkpoint design as gameplay-proven.

---

## 6. Save-Load Timing (Static Lists Empty)

Static lists `Property.OwnedProperties`, `Business.OwnedBusinesses`, `NPCManager.Registered` are **empty** at `OnGameplaySceneLoaded`. Use:

```csharp
// Mod.cs OnInitializeMelon:
// OBSERVED IN THE 2026-09-29 INSTRUMENTED RUN: `OnSaveInfoLoaded` fired 0 times; do not rely on it without re-verification.
GameLifecycle.OnPreLoad        += OnPreLoad;        // before save data loads → clear caches / reset state
GameLifecycle.OnLoadComplete   += OnLoadComplete;   // after scene build → refresh Property/Item caches, spawn UI / managers / world objects
GameLifecycle.OnSaveComplete   += OnSaveComplete;   // user pressed Save → flush to disk
// Verified order (schedule1-lifecycle-verify §7): OnSceneWasLoaded("Main") → OnPreLoad → OnLoadComplete
```

* Reference fix: PotScanner replaced a long retry loop with a lifecycle hook; use `OnLoadComplete` for the final refresh when confirmed in the installed runtime (`schedule1-troubleshooting` section 5).
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
- [ ] Static lists read in `OnLoadComplete` (not at `OnGameplaySceneLoaded`); the instrumented session observed 0 `OnSaveInfoLoaded` firings, so do not use it for refresh without re-verification.
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
