# S1Mods.Shared & Architecture Guidelines

> verified: API rules 2026-08-20/21 + 2026-09-11 audit; §1/§5 deduplicated to canonical homes 2026-10-05. Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

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
* **Slot-Isolation & Slot -1 Guard (2026-09-11 Standard):** every per-save file must be `slot_{SaveSlotNumber}.json`, guarded by the Triple IL2CPP Guard + `SaveSlotNumber >= 0` (never `slot_-1.json`), plus `TryMigrateLegacy`. **Canonical implementation + full code:** [`../../schedule1-persistence/references/slot-isolation.md`](../../schedule1-persistence/references/slot-isolation.md) — edit there, not here.

## 2. InputFocus Protection (`NotesAppInputFocus` / `HotkeyManager`)
When user focuses on an `InputField` or `TMP_InputField`, vanilla player movement (WASD, hotbars, phone toggles) must be suspended.

* Implement an `IInputFocus` or subscribe to `HotkeyManager.IsInputFieldFocused()`.
* On focus enter: Block character input & pause hotkey routing.
* On focus exit / submit / cancel: Restore normal game input.

---

## 3. ModConfig & MelonPreferences
Use `ModConfig<T>` to bind strongly-typed C# models to `UserData/MelonPreferences.cfg` or per-mod `config.json`.

**Backend guidance:** `MelonPreferences.cfg` = the global TOML backend `ModConfig<T>` binds to (user-tunable scalars, hot-reloadable via `schedule1-debounced-reload`). The per-mod `config.json`/JSON sidecar (`ConfigJsonStore`) is for data TOML cannot hold (`Dictionary`/`List`) — don't mix: one source of truth per data kind, scalars → TOML, collections → JSON sidecar.

* Store user configurable keys (e.g., toggle HUD, custom hotkeys, scale, opacity).
* Validate values upon loading (clamp numeric ranges, fallback to defaults on null).

### ⚠️ TOML Limitation — canonical (empirical, 2026-08-20)
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

## 5. Transaction Ordering for Money/Economy — see canonical

**Canonical source:** [`../../schedule1-economy/references/atomic-purchase.md`](../../schedule1-economy/references/atomic-purchase.md) (PocketShop purchase flow) + [`../../schedule1-economy/references/passive-revenue.md`](../../schedule1-economy/references/passive-revenue.md) (BusinessIncome snapshot ordering). Summary of the rules:

1. **Memory-first + snapshot revert**: mark paid in memory (snapshotting previous state), execute the transaction in try/catch, revert to the **snapshot** on failure (never `-1`), persist to disk only after success.
2. **Atomic purchase**: validate & price → pre-create ALL instances → pay in its own try/catch (**never refund if payment never executed**) → transfer in its own try/catch (refund via the opposite ledger on failure) → decrement stock last.
3. **Host authority**: passive income / balance writes wrapped in `NetworkGuard.IsHostOrSingleplayer()` — never book on clients (2–4× payout in co-op).

Why-snapshot evidence (2026-08-21 `PayoutStateStore.cs:20-21/:149-160`) and full code live in the canonical files — edit there, not here.

---

## 6. IL2CPP Native Lifecycle & WasCollected Guards (2026-09-11 Standard)

In IL2CPP modding, C# object instances act as proxies over native C++ objects. When Unity destroys an object in C++, the managed proxy may still exist in memory!
* Checking `myComponent != null` can return `true` even if the C++ object was collected.
* Accessing fields on a dead proxy triggers native unhandled exceptions (`0xc0000005` access violation).
* **The Golden Guard:**
  ```csharp
  if (obj == null || obj.Pointer == IntPtr.Zero || obj.WasCollected) return;
  ```
Apply this guard in `OnUpdate()`, button listeners, polling loops, and async callbacks.

---

## 7. Static Event Dispatchers & Leak Prevention (2026-09-11 Standard)

**The Trap:** Ephemeral UI classes (e.g., PhoneApp panels, item cards, list rows) subscribing instance methods to static game events:
```csharp
Money.OnBalanceChanged += this.UpdateBalance; // LEAK! Keeps old app instances in memory across scene transitions
```
When the scene unloads or the phone closes, the static event still holds the delegate. On next trigger, it invokes code on a collected C++ object → crash or zombie updates.

**The Solution:**
1. Use static event dispatchers in your mod root / service layer.
2. Maintain a weak or managed list of active listeners that clears on `OnSceneWasUnloaded` or `OnDestroy()`.
3. Defensive unsubscribe: always `-=` before `+=` in `OnCreated()`.
4. Wrap listener execution in individual `try/catch` blocks so one failing subscriber does not break others.
