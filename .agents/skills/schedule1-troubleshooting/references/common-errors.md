# Common Error Patterns — Detailed Decoder

A deeper dive than the master `SKILL.md` table. Includes cause, workarounds, and reference impls for the most common Schedule I mod failures.

---

## 1. `MissingMethodException` During Patch Install

**Symptom:**
```
[<time>] [ModName] Error: MissingMethodException: void ScheduleOne.A.B.C()
[<time>] [Harmony] failed to patch <FullName>
```

**Cause (verified):** Harmony tries to pin the method's IL body. When the body references an obsolete/removed API, the pin fails BEFORE user code runs.

**Fix:** Patch the **caller** instead. E.g. instead of patching `DialogueHandler.MarkDirty` which references `get_activeDialogue()` (removed in 0.4.6f11), patch the caller of `MarkDirty`.

**Lessons:**
- The `AGENTS.md §5` patch-exception rule.
- The "Harmony-Patch Ohne explizite Target-Spec" rule (use `harmony.Patch(methodInfo, ...)` without marker-attribute).

---

## 2. `[RegisterTypeInIl2Cpp]` Failure: Class Not Initialized

**Symptom:**
```
[<time>] [ModName] Error: Cannot register Il2Cpp type
[<time>] [ModName] Error: Object reference not set…
```
or
```
[<time>] [ModName] Info: Adding component NotesAppInputFocus
[<time>] Exception: NullReferenceException: MonoBehaviour …
```

**Cause (verified 2026-08-04, NotesApp fix):** A `MonoBehaviour` subclass is marked `[RegisterTypeInIl2Cpp]` but **missing the public `IntPtr` constructor**:
```csharp
public MyBehaviour(IntPtr ptr) : base(ptr) { }
```
Without it, the IL2CPP-Bridge cannot initialize the wrapper when `AddComponent<MyBehaviour>()` is called.

**Detection:** `s1interop analyze <csproj>` reports `injected_type_missing_intptr_constructor`.

**Fix:** Add the constructor (the only mandatory thing). Verify: `s1interop analyze` after fix returns no warning.

---

## 3. `MissingFieldException` on Private Field

**Symptom:** Mod reads `Game.GetType().GetField("privateField").GetValue(...)` → fails with `MissingFieldException`.

**Cause (verified 2026-08-04):** IL2CPP's reflection layer sometimes strips fields or returns a stale handle. Reflection-based set may write to the wrong address.

**Fix:**
- Prefer Harmony-Prefix on the method that uses the field (read inside the patched method, valid IL).
- Or use S1API wrapper if available.

---

## 4. Native Access Violation (0xc0000005)

**Symptom:**
```
Windows Error Dialog: "Exception Processing Message 0xc0000005"
Game closes without managed stack trace in Latest.log.
```

**Cause (verified 2026-08-04, DrugExpansion rollback):** IL2CPPInterop fails to init all classes in the mod because:
- Mod class count > ~5000 / Mod .dll size > 10 MB
- Warning just before the crash:
  ```
  [<time>] [Il2CppInterop] Warning: Class::Init signatures have been exhausted, using a substitute!
  ```
- Substituted class init → memory-mismatch → native AV.

**Fix:**
- Split the mod into multiple `.csproj` files / DLLs.
- Reduce `[RegisterTypeInIl2Cpp]` usage.
- Roll back to an older version of the third-party mod.

---

## 5. CLR Fatal Error (0x80131506)

**Symptom:**
```
Windows Error Dialog: "0x80131506" (Fatal CLR Error)
[<time>] [S1API] Fatal: <…>
```

**Cause (verified 2026-08-03, S1API 3.1.3/3.1.6):** S1API's `PreRegisterAllNpcPrefabs()` called `Assembly.GetTypes()` → infinite recursion → CLR crash.

**Fix:** Upgrade to S1API 3.1.7+ (ifBars-Fork has the deferred-building-resolve fix). For older 3.1.x forks, roll back.

---

## 6. Patch Installs But Never Fires (Inlining)

**Symptom:** Method is patched but `OnUpdate` work never appears.

**Cause:** IL2CPP inlines small methods (getters, setters, one-liners) in the native build. The Harmony patch installs on the IL proxy, but the inlined native call site bypasses it.

**Fix:** Patch the **caller** instead. For example, instead of patching `MoneyManager.GetCashBalance()`, patch every call site that uses cash balance. Or compute the value yourself.

---

## 7. UIButton.onClick Crash

**Symptom:**
```
[<time>] [ModName] Exception: Cannot convert from method group to UnityAction
[<time>]   at System.Reflection…
```

**Cause:** `button.onClick.AddListener(new UnityAction(MyMethod))` — IntPtr cast fails on IL2CPP delegates.

**Fix:** Always use `S1API.Utils.ButtonUtils.AddListener(button, () => MyMethod())` or `S1API.Utils.EventHelper.AddListener(...)`.

---

## 8. Scene-Unload Saga

**Symptom:** Mod works fine in scene A, then breaks on scene transition.

**Cause:** Static-List retention or stale cache after scene unload. Common cases:
- Cached `MoneyManager.Instance` is fine (singleton).
- Cached `List<T>` of gameplay objects is **not** — gets destroyed.

**Fix (verified 2026-08-04, PotScanner v0.1.1):**
- Clear all caches in `OnSceneWasUnloaded`.
- Single helper `IsGameplayScene(name)` predicate, reused by Load + Unload handlers (load/unload asymmetry is the classic bug source).
- Re-resolve via `S1Mods.Shared.GameObjectResolver.FindComponentDeep<T>()` after scene load.

---

## 9. Save-Load Timing (Static List Empty)

**Symptom:** `Property.OwnedProperties.Count == 0` even though save has owned properties. `Player.Stats == null`.

**Cause (verified 2026-08-04, PotScanner v0.2.0):** `OnGameplaySceneLoaded` fires before S1API/internal save-load completes. Static lists are uninitialized at that point.

**Fix:** Subscribe to `GameLifecycle.OnSaveInfoLoaded` (fires after save-info parse, before scene build).
```csharp
GameLifecycle.OnSaveInfoLoaded += () => RefreshPropertyCache();
GameLifecycle.OnLoadComplete   += () => ForceRefreshUI();
```

---

## 10. `MelonLogger.Instance.Error` vs `MelonLogger.Error`

**Symptom:** Patching the error-logger fails to catch IL2CPP-internal errors.

**Cause:** Both static `MelonLogger.Error` and instance `MelonLogger.Instance.Error` are used by MelonLoader. Patch **both** for full coverage.

**Fix:**
```csharp
harmony.Patch(typeof(MelonLoader.MelonLogger).GetMethod("Error", new[] { typeof(string) }), postfix: new HarmonyMethod(() => {}));
harmony.Patch(typeof(MelonLoader.MelonLogger).GetProperty("Instance").Getter, /* hook error */);
```

---

## 11. `IL2CPP.Il2CppObjectBaseToPtr` Null Return

**Symptom:**
```
[<time>] Exception: NullReferenceException at IL2CPP.Il2CppObjectBaseToPtrNotNull
```

**Cause:** Object was destroyed (Unity `==` overload reports null) but C# wrapper survived; passing it to a native method fails.

**Fix:**
```csharp
if (obj == null || obj.GetInstanceID() == 0) return;
```
Never `is null` for `UnityEngine.Object` (operator overload is bypassed).

---

## 12. Stack Trace from `GameAssembly`

**Symptom:** Stack trace mentions `<GameAssembly.dll!SomeMethod+0x...>` offsets only.

**Cause:** IL2CPP compiles to native; native offsets don't map back to method names.

**Fix:** Translate the offset → native method by:
1. Opening `Assembly-CSharp.dll` or `Il2CppScheduleOne.dll` in `ilspycmd -t` and matching the method name.
2. Or searching the decompile for the nearest method that contains the offset's surrounding stack frames.

---

## 13. `FormatException` from JSON Persistence

**Symptom:**
```
[<time>] [ModName] Error: FormatException: '<', hexadecimal value 0x3C, …
```

**Cause:** The persisted JSON file is empty or truncated (crash during write).

**Fix:**
- Always use `S1Mods.Shared.SafeStorage.SaveAtomic` — atomic write + `.bak`.
- On corrupt load, fall back to `.bak` content.

---

## 14. Generic Loading-Screen Stutter

**Symptom:** Mod's first scene-load is 1-2 s slower than expected.

**Cause:** First `il2cpp_runtime_invoke` for each type marshals + warms caches. With many types, this adds up.

**Fix:** Lazy-init per-type on first use. Avoid eager scene-wide scans during `OnGameplaySceneLoaded`.

---

## 15. Type Mismatch on Generic Method

**Symptom:** `MissingMethodException` on a `GetComponent<T>` for a struct type.

**Cause (Rule 10 in `IL2CPP-Interop-Rules.md`):** AOT doesn't generate generic instances for struct types the game never used.

**Fix:**
- Use non-generic overloads: `GetComponent(Il2CppType.Of<T>())`.
- Or use the version known to work in vanilla.

---

## 16. PhoneApp Blank After 1st Close (Unsubscribe in OnPhoneClosed)

**Symptom (verified 2026-08-20, ALL 5 PhoneApp mods):** App renders on first open, but is blank/empty on the second phone open. No crash, no log error.

**Cause:** `OnPhoneClosed()` contains `MelonEvents.OnUpdate.Unsubscribe(Update)` (or `-=` on static events like `OnPotsScanned`/`OnBalanceChanged`/`OnStateChanged`). S1API instantiates each `PhoneApp` **ONCE per scene** (`HomeScreen_Start_Patch` + `Activator.CreateInstance`), so `OnCreated` (which re-subscribes) never fires again until a scene reload. After the 1st close the Update loop is dead → `_mainBG.SetActive(open)` never runs → app stays hidden.

**Fix:**
```csharp
protected override void OnCreated()
{
    base.OnCreated();
    MelonEvents.OnUpdate.Unsubscribe(Update);  // idempotent (scene reload)
    MelonEvents.OnUpdate.Subscribe(Update);
}
protected override void OnPhoneClosed()
{
    base.OnPhoneClosed();
    if (_mainBG != null) _mainBG.SetActive(false);
    // NO Unsubscribe here!
}
```

**Detection:** Grep all PhoneApp mods for `Unsubscribe` inside `OnPhoneClosed`. Regression source: e24371b (2026-08-19).

**Reference:** schedule1-phoneapp Rule 10 + `schedule1-s1api/references/lifecycle.md` §7.

---

## 17. Config Values Reset on Restart (ModConfig TOML Limit)

**Symptom (verified 2026-08-20, BusinessIncome):** Dictionary/List config values (`PropertyMultipliers`, `DisplayNameOverrides`, `WeekendBonusCategories`) return to defaults after every restart, though scalars persist.

**Cause:** `ModConfig<T>` (MelonPreferences/TOML) cannot serialize `Dictionary<K,V>`/`List<T>` properties — `CreateEntry` throws "not TOML-mappable", property silently dropped (logged once).

**Fix:** Add a SafeStorage JSON sidecar (`ConfigJsonStore` pattern):
```csharp
ConfigJsonStore.ApplyToConfig(ModConfig<T>.Instance);  // after Initialize
ConfigJsonStore.Save(ModConfig<T>.Instance);           // after every SetAndSave
```
---

## 18. Fatal Startup Crash (0xc0000005) on IL2CPP Prefix `ref __result`

**Symptom (verified 2026-08-22, BackpackMod):**
MelonLoader closes after ~2 seconds with a Windows Application Error:
`Ausnahmecode: 0xc0000005` (Access Violation) in `UnityPlayer.dll` at `0x0000000000646d5e`. `Latest.log` abruptly ends after mod initialization or during first scene load without an exception trace.

**Cause:**
In Unity IL2CPP, writing a managed IL2CPP wrapper object to `ref <Il2CppType> __result` in a `[HarmonyPrefix]` with `return false;` corrupts the native C++ return register / trampoline. When the engine calls the hooked function, it reads an invalid native pointer and crashes instantly.
Additionally, using blind `harmony.PatchAll()` will indiscriminately arm such dangerous patches.

**Fix:**
1. **Never override native object returns via Prefix `ref __result`.** Instead, let the native method run and mutate the object in a `[HarmonyPostfix]`, or inject data into static registries/lists directly during initialization.
2. **Never use blind `harmony.PatchAll()`.** Register all patches explicitly through `PatchGuard.TryPatch` with target method reflection and pointer validation (`obj != null && obj.Pointer != IntPtr.Zero && !obj.WasCollected`).

---

## Quick-Reference: First-Try Fixes

| Error | First Try |
|-------|-----------|
| `MissingMethodException` on patch | Patch the caller, not the dead method |
| `NullReferenceException` in patch | Add `GetInstanceID() == 0` null-check |
| Mod silent | Check inlining (small method patched → patch caller) |
| Native AV / 0xc0000005 | Remove `ref <Il2CppType> __result` in Prefix (§18), split DLL |
| Save data empty | Use `OnSaveInfoLoaded` hook, not `OnGameplaySceneLoaded` |
| UIButton crash | Use `ButtonUtils.AddListener` |
| JSON corrupt | Use `SafeStorage.SaveAtomic` |
| `[RegisterTypeInIl2Cpp]` crash | Add `IntPtr` ctor |
| Stack offsets only | `ilspycmd` the offset's enclosing method |
| PhoneApp blank after 1st close | Remove `Unsubscribe` from `OnPhoneClosed` (§16) |
| Dict/List config resets | JSON sidecar via SafeStorage (§17) |
| Startup AV in UnityPlayer.dll | Check Harmony Prefixes with `ref __result` (§18) |
| Stations multiply on each load | Never `SpawnStationAt` in `OnLoadComplete` for BuildableItems — game restores them; only `ApplySaveData` |
| Save data wiped on scene change | Never `OnSceneWasUnloaded` to clear state — use `OnPreLoad` only |
| Input keys fire through game UI | Guard with `if (Cursor.lockState != CursorLockMode.Locked) return;` |
| Items lost on Dismantle | Check `CanItemFitInInventory` before each `AddItemToInventory` in dismantle flow |

---

## §19 — Buildable Duplicate-Spawn Bug (verified 2026-08-22, AutoPackagingStation)

**Symptom:** Each save→load adds one more copy of the station at the same position.

**Cause:** `OnLoadComplete` calls `SpawnStationAt(new GameObject(...))` in the `else`-branch when GUID matching fails. But the game also restores `BuildableItem`s itself. On first load: 2 stations. On second: 3. Etc.

**Why GUID matching fails:** `Awake()` uses `Guid.NewGuid()` instead of `BuildableItem.GUID`. Every session generates a new random GUID → saved GUIDs never match → `else`-branch always fires.

**Fix:**
```csharp
// In Awake() — use BuildableItem's own persistent GUID
var buildable = gameObject.GetComponent<BuildableItem>();
if (buildable != null) _stationGuid = buildable.GUID.ToString();
// Fallback only if truly no buildable (e.g. procedural spawn)
if (string.IsNullOrEmpty(_stationGuid)) _stationGuid = Guid.NewGuid().ToString();

// In OnLoadComplete — NEVER spawn, only pre-stage data
ApplySaveData(saved.Guid, saved); // Controller picks up lazily via GetRuntimeData(guid)
// ← REMOVE: SpawnStationAt(pos, rot, saved);
```

**Also:** Remove any `OnSceneWasUnloaded` hook that clears `_activeStations` — it fires for menu/loading scene unloads and wipes state before `OnSaveComplete`, causing a blank save.

---

## §20 — Key Input Conflict With Game UIs (verified 2026-08-22)

**Symptom:** Station interaction keys (E/R/F/Space) fire while talking to NPCs, in menus, or shopping.

**Cause:** `HandlePlayerInput()` only checks `_isPlayerNear` (raycast), not whether any game UI is blocking input.

**Fix:** Add at top of `HandlePlayerInput()`:
```csharp
if (Cursor.lockState != CursorLockMode.Locked) return; // UI is open, cursor is free
```
`CursorLockMode.Locked` = gameplay mode, cursor hidden. `None`/`Confined` = UI open.
