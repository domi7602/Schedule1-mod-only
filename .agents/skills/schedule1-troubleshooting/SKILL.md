---
name: schedule1-troubleshooting
description: >-
  Diagnostic runbook for Schedule I MelonLoader IL2CPP mods (v0.4.6f13, S1API 3.2.0).
  Use this skill whenever: the game crashes on start, a mod throws on first frame, a Harmony patch silently no-ops,
  Latest.log shows error spikes, save-load desyncs, property/owner-lists stay empty,
  an [RegisterTypeInIl2Cpp] crash report appears, S1MCP find_gameobjects freezes the game,
  or any symptom suggests IL2CPP marshalling issues.
  Tools covered: logscan.py, s1interop analyze, s1interop doctor, ilspycmd, MelonPreferences.cfg.
---

# Schedule I — Troubleshooting & Crash Diagnostics

When a mod breaks the game, or a game update breaks a mod, work this skill before writing a fix. Three out of five reported issues already have a documented cause in **`AGENTS.md §5`** or **`MEMORY.md`** under one of these category headers — check first.

---

## 1. Diagnostic Decision Tree

```
Mod / Game misbehaving
│
├─ Game crashed on start?
│  ├─ Windows Error dialog "0xc0000005" → native AV / IL2CPP
│  ├─ Windows Error dialog "0x80131506" → CLR Fatal Error (S1API pre-init)
│  └─ "Fatal Error in GC" / OutOfMemory → 10MB+ mod class volume exceeded
│
├─ Game running, mod silent (no errors)?
│  └─ See §3 "Patch silent no-op"
│
├─ Game running, mod logs errors?
│  └─ See §4 "Common error patterns"
│
├─ Save loaded, but data is empty/wrong?
│  └─ See §5 "Save-Load-Timing"
│
├─ Mod worked yesterday, broken today (game update)?
│  └─ See §6 "Update resilience"
│
└─ UI / PhoneApp broken?
   └─ See §7 "UI debugging"
```

---

## 2. Tooling

### `logscan.py` (DEFAULT — first tool to invoke)
Path: `C:\Users\pc\Desktop\Schedule1\External\S1MCPServer\logscan.py`. S1MCP-free, no game-freeze risk.
```pwsh
pwsh "C:\Users\pc\Desktop\Schedule1\External\S1MCPServer\logscan.py" --error-summary
pwsh "C:\Users\pc\Desktop\Schedule1\External\S1MCPServer\logscan.py" --mod NotesApp
pwsh "C:\Users\pc\Desktop\Schedule1\External\S1MCPServer\logscan.py" --since 14:00
```
See **[`references/logscan-and-logs.md`](references/logscan-and-logs.md)** for the full flag set, column layout, common spike patterns, and worked examples.

### `s1interop` (PRE-FLIGHT + STATIC ANALYSIS)
```pwsh
s1interop doctor --il2cpp-game-path $env:SCHEDULE1_PATH                              # environment
s1interop analyze "C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule 1 Modding\Source\Mods\NotesApp\src\NotesApp.csproj"   # static
```
Catches `injected_type_missing_intptr_constructor`, `[HideFromIl2Cpp]` gaps, `[RegisterTypeInIl2Cpp]` issues. See **[`../schedule1-knowledge/references/inventory.md`](../schedule1-knowledge/references/inventory.md)** for s1interop tool path.

### `ilspycmd` (LIVE VERIFICATION)
```pwsh
& 'C:\Users\pc\.dotnet\tools\ilspycmd.exe' -t <Full.Type.Name> $env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\*.dll
```
Always verify API signatures exist before patching — see **[`../schedule1-knowledge/SKILL.md`](../schedule1-knowledge/SKILL.md)** §4.

### Direct `Latest.log` parsing
`<GameDir>\MelonLoader\Latest.log` is the canonical live feed. For deep dives, `logscan.py` is preferred over manual `select-string` (more reliable regex, contextual output).

---

## 3. Patch Silent No-Op (Game Runs, Mod Does Nothing)

The classic symptom: `OnInitializeMelon` logs "Ready" but `OnUpdate` work never appears, or "Watered!" never logs. Common causes, in order of frequency:

1. **Method-Inlining in IL2CPP.** Small methods (getters, one-liners) are inlined in the native build — Harmony patch installs but never fires. **Fix: patch the caller, not the inline target.**
2. **`HarmonyPatch` attribute without explicit target.** `[HarmonyPatch(typeof(X))]` without `(nameof(X.Method))` fails with "Undefined target method". **Fix: `harmony.Patch(methodInfo, new HarmonyMethod(...))` manually without marker attribute**, or supply full target spec.
3. **Scene is wrong.** The patched object's scene is not active. Verify with `[Mod].Logger.Msg($"Scene: {SceneManager.GetActiveScene().name}")`.
4. **Static-List empty at scene load.** E.g. `Property.OwnedProperties.Count == 0` even though player owns properties (verified 2026-08-04 with PotScanner v0.2.0). **Fix: subscribe to `GameLifecycle.OnSaveInfoLoaded` instead of `OnGameplaySceneLoaded`** (PotScanner v0.2.1 reference impl).
5. **Mod dependency missing.** Optional dep via `[assembly: MelonOptionalDependencies("S1API")]` requires you to try-catch every S1API code path. Otherwise the mod throws before reaching your logic.

For all five: reproduce in-game, capture `Latest.log`, log the failing scenario's `OnUpdate`/`Tick` invocation count, and trace.

---

## 4. Common Error Patterns (Latest.log Decoder)

| Error string | Meaning | First fix to try |
|---|---|---|
| `MissingMethodException` on a now-removed game API | Game update removed the API; your patch pinned the IL body. | Patch the caller, not the dead method body. |
| `[RegisterTypeInIl2Cpp]` failure: "Class not initialized" | Your `MonoBehaviour : Il2CppObjectBase` is missing the `IntPtr` ctor. | Add `public Foo(IntPtr ptr) : base(ptr) { }`. |
| `MelonLoader\Il2CppAssemblies\…` null reference | The game's IL2CPP proxy is exposing a stale handle after object destruction. | Use `GetInstanceID() == 0` null-check before access. |
| `HarmonyException: Cannot patch … final method` | IL2CPP strips method bodies → methods are final / sealed. | Use Prefix-Postfix, never Transpiler. |
| `Class::Init signatures exhausted, using a substitute!` | IL2CPPInterop can't initialize all classes (>5–10 MB mod). | Reduce mod class count (split) or roll back to older version. |
| 0xc0000005 (native AV) with no MelonLoader trace | Native crash after init. | Backout mods in halves until stable; culprit has 10MB+ class volume. |
| `Object reference not set to an instance of an object` in patch | You tried to access a destroyed IL2CPP object. | Always `if (obj == null || obj.GetInstanceID() == 0) return;` first. |
| `MissingFieldException` on a private field | IL2CPP field not registered for reflection. | Use Harmony-Prefix on the method instead of reading the field via reflection. |

For an expanded reference (18+ error patterns + IL2CPP exception translations) see **[`references/common-errors.md`](references/common-errors.md)**.

---

## 5. Save-Load-Timing Failures

The Schedule I save-load pipeline is multi-phase and most static lists (`Property.OwnedProperties`, `NPCManager.Registered`, …) are empty at `OnGameplaySceneLoaded`. The fix is **always** to use the S1API lifecycle hook:

```csharp
// In Mod.cs OnInitializeMelon:
GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
GameLifecycle.OnLoadComplete   += OnLoadComplete;

// Lifecycle order:
//   OnSaveInfoLoaded  → after Save-Info parsing, BEFORE scene build
//                        (good time to refresh Property / Item caches)
//   OnLoadComplete    → after scene build complete
//                        (good time to attach UI / instantiate managers)
```

Symptom: HUD looks empty, PhoneApp shows "(unknown)" properties, Owner-count = 0 even though save has 5 owned.

Reference pattern: **PotScanner v0.2.1** (2026-08-04) — used exactly this hook to drop the 25-second retry-mechanism it needed in v0.2.0.

For deeper analysis (multi-phase lifecycle, all S1API hooks, FishNet SyncVar timing) see **[`references/save-load-timing.md`](references/save-load-timing.md)**.

---

## 6. Update Resilience (Game-Patch-Breakage)

When TVGS pushes a Schedule I update:

1. **Run `s1interop doctor`** — detect new IL2CPP runtime version, missing method counts.
2. **Check `AGENTS.md §2 Mod-Inventar`** — every verified mod has a "verified YYYY-MM-DD" date; anything older than the latest game version is at risk.
3. **For each mod: `ilspycmd` the patched class** — confirm method signatures match. Anything that uses pinning on IL body containing deprecated APIs is at risk.
4. **`PatchGuard`** in `S1Mods.Shared` wraps Harmony patches with graceful-degradation (transpiler/finalizer forwarding, overload protection, status report). Use it for any patch that could be near a hot game-surface.

If a previously-working patch no-ops:
1. Open `Latest.log`, search for `Harmony` lines
2. Compare `harmony.GetPatchedMethods()` output before/after game restart
3. Most likely culprit: small inlined getter/setter — patch the caller

---

## 7. UI Debugging (PhoneApp / uGUI)

| Symptom | Likely cause | Fix |
|---|---|---|
| PhoneApp icon shows but tap does nothing | Wrong Scene, wrong `container` | Check `OnAppOpen()` order; verify container != null |
| **PhoneApp blank/leer beim 2. Öffnen (nach 1. Close)** | `MelonEvents.OnUpdate.Unsubscribe(Update)` in `OnPhoneClosed()` — `OnCreated` feuert nur einmal pro Scene | Entfernen; defensives `-=`-before-`+=` nur in `OnCreated` (siehe schedule1-phoneapp Rule 10). Selbes Muster bei `-=` auf static Events (`OnPotsScanned`, `OnBalanceChanged`, `OnStateChanged`) |
| PhoneApp History/Notes nach Slot-Wechsel falsch | Global `state.json` statt `slot_{n}.json` | Slot-Isolation via `LoadManager.ActiveSaveInfo` + `TryMigrateLegacy` — siehe schedule1-phoneapp Rule 11 (`CalculatorState.cs:65` Fix 2026-08-21) |
| Local UITheme zeigt falschen Scale auf 4K | Lokaler `UITheme` dupliziert `S1Mods.Shared.UITheme` | Wrapper delegieren lassen (`InitializeForTextApp/Dashboard`), keine lokale Clamp-Math (`NotesApp.cs:33` Fix 2026-08-21) |
| Layout broken on player's screen but test OK on dev screen | DPI / Screen-Scale mismatch | Use `UITheme.Sp/Dp` (Methode 3) — see `[schedule1-modding/ui-and-s1api.md]` |
| InputField keys trigger WASD movement | InputFocus not hooked | Register `NotesAppInputFocus`-style `MonoBehaviour` per `AGENTS.md §5` |
| UIButton.onClick silently fails | `new UnityAction(...)` IntPtr issue | Use `S1API.Utils.ButtonUtils.AddListener(...)` |
| HUD text disappears intermittently | Component destroyed (scene reload) | Re-resolve via `GameObjectResolver.FindComponentDeep<T>()` after `OnSceneWasLoaded` |
| PhoneApp icon: "Icon file not found" | `IconFileName = ""` or wrong path | Override `IconSprite` (return a Sprite directly, see PotScanner v0.2.0 fix) |
| Minigame/HUD-Sprite falsch nach Shape-Wechsel | Cache ignoriert Parameter (eine `Sprite?`-Field für 2 Radien) | Cache nach Parametern keyen (`Dictionary<string,Sprite>` `"{size}_{radius}"`) — siehe MinimapTextures Fix `MinimapTextures.cs:9` 2026-08-21 |
| Circle-Mask/Border falsch bei size-Wechsel | Single `_circleMaskSprite` statt dict | `_circleMaskCache` keyed `"{size}"` / `"{size}_{thickness}"` — siehe Fix 2026-08-21 |

---

## 8. Known Fragile Areas (Empirical History)

These **WILL** bite you if you don't read first:

* **`Harmony.Patch` on a method with `MissingMethodException` in IL body** → fails at pin time, BEFORE your code runs. (Documented in MEMORY.md, "MelonLoader 0.7.3 + IL2CPP Mod-Suppression Lessons".)
* **`DialogueHandler.get_activeDialogue()` removed in 0.4.6f11** → any mod touching this crashes at JIT. (See "ThompsonYard" entry in MEMORY.md.)
* **10.5 MB+ Mod class volume** → IL2CPPInterop `Class::Init signatures exhausted` warning → native AV 0xc0000005 → game dies. (DrugExpansion 1.0.0.)
* **Static-List Empty at Scene-Load** (see §5).
* **Reflection-Field-Set in IL2CPP** → unreliable, can write to wrong address — use Harmony-Patch instead.
* **`MelonLogger.Instance.Error` vs `MelonLogger.Error`** — both must be patched for full coverage. (See il2cpp_modding_rules.md rule #3.)
* **Multiplayer Host Authority** → Mod logic running on both host and client causes double payments/executions. Always wrap with `IncomeEngine.IsHostOrSingleplayer()` or equivalent.
* **Procedural Audio Crashing** → Native methods like `MoneyManager.Instance.PlayCashSound()` will throw if the `MoneyManager` instance isn't ready or if called off main thread. Always null check `MoneyManager.Instance` or catch exceptions.
* **PhoneApp-Update-Tod (2026-08-20, 5 Mods)** → `Unsubscribe` in `OnPhoneClosed` killte `MelonEvents.OnUpdate`/Event-Handler permanent (OnCreated feuert nur einmal). Apps blank nach 1. Close. Fix + Diagnose: siehe §7 UI-Debugging.
* **Sprite-Cache ohne Parameter-Key (2026-08-20)** → `private static Sprite? _x` gecacht, aber Aufrufer mit unterschiedlichen Parametern (Radius/Thickness) → erster Aufrufer gewinnt, falsche Maske/Border. Fix: `Dictionary<string, Sprite>`-Cache (`MinimapTextures.cs:9` circle-mask 2026-08-21 ebenfalls).
* **ModConfig-Dictionary-Verlust (2026-08-20)** → `ModConfig<T>` persistiert `Dictionary/List`-Properties NICHT (TOML-Limit) → nach Restart Defaults. Fix: SafeStorage-JSON-Sidecar (ConfigJsonStore-Pattern).
* **Global State-File Leak (2026-08-21)** → `CalculatorState.cs:66` `calculator_state.json` global statt `slot_{n}` → Slot-A leaked nach Slot-B. Fix: `GetActiveSlotSuffix()` + `TryMigrateLegacy` (`CalculatorState.cs:65-93`, Rule 11).
* **Local UITheme Duplication (2026-08-21)** → `NotesApp.cs:33`/`PotScannerApp.cs:19`/`CalculatorApp.cs:22` lokale `UITheme` mit abweichenden `RefHeight/Clamp` (750/2.5, 900/1.20, 850/1.30) vs Shared `750/2.0` / `900/1.20` → DPI-Drift. Fix: Wrapper delegiert zu `S1Mods.Shared.UITheme`.
* **MoreSaveSlots Raw File-Write (2026-08-21)** → `MoreSaveSlotsConfig.cs:57` `File.WriteAllText` + `SaveRenameService.cs:71` `Copy/Write/Replace` ohne atomar/.bak-Schutz → Crash korrumpiert `Game.json`. Fix: `SafeStorage.SaveTextAtomic`.
* **BusinessIncome Snapshot-Loss (2026-08-21)** → `PayoutStateStore.Revert` setzte `LastPaid=-1` statt Snapshot → History Day 5 verloren → Doppel-Payout. Fix: `_pendingPrevLastPaid/_pendingPrevPerBusiness` (`PayoutStateStore.cs:20-21`).
* **Field-Accessor Not Patchable (2026-08-21)** → `BaseItemDefinition.get_DefaultStackLimit` field accessor → `Il2CppInterop can't be patched` (`Latest.log:17:43:04.438`). Fix: Patch entfernen (`Mod.cs:109`), via Engine-Scan lösen.

---

## 9. When to Roll Back vs. Fix Forward

* **Game-version mismatch confirmed** → roll back via `.bak`. Quick, low-risk.
* **One-method API drift** → refresh the method signature in your patch, verify with `ilspycmd`.
* **Wholesale API refactor in S1API** → re-run `s1interop analyze` to map new pattern, refactor conservatively.
* **More than ~3 mods at fault** → roll back to last-known-good state, re-introduce mods one by one with logscan between each.

---

## 10. Escalation

If a diagnostic step exceeds 30 minutes without resolution:
1. Capture the full `Latest.log` and the failing call stack.
2. Identify the **first** exception (not the last — IL2CPP cascades).
3. Add a minimal repro: scaffold a new empty mod via `pwsh Tools/new-mod.ps1 -Name Repro` and isolate.
4. Check `Knowledge/Game-Reference/Analysis/Learnings/ThirdParty/` for a similar mod — fixes are often already documented.

---

## 11. Related Skills

* `schedule1-modding` — mod runbook (build, deploy, architecture patterns) — load for any modding task.
* `schedule1-s1api` — S1API reference (Saveables, Quests, NPCs, lifecycle) — load when the issue is around S1API behavior.
* `schedule1-s1mapi` — S1MAPI reference (procedural meshes, buildings, GLTF) — load when the issue is around procedural geometry.
* `schedule1-knowledge` — KB navigation — load when you need to find the right doc file.
