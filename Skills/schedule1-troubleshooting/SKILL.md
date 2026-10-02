---
name: schedule1-troubleshooting
description: >-
  Diagnostic runbook for Schedule I MelonLoader IL2CPP mods (v0.4.7f7, S1API 3.2.1-beta.7).
  Use this skill whenever: the game crashes on start, a mod throws on first frame, a Harmony patch silently no-ops,
  Latest.log shows error spikes, save-load desyncs, property/owner-lists stay empty,
  an [RegisterTypeInIl2Cpp] crash report appears, S1MCP find_gameobjects freezes the game,
  or any symptom suggests IL2CPP marshalling issues.
  Tools covered: native PowerShell log triage (see references/logscan-and-logs.md), s1interop analyze, s1interop doctor, ilspycmd, MelonPreferences.cfg.
---
<!-- Version anchor: Game v0.4.7f7 / S1API 3.2.1-beta.7 / MelonLoader 0.7.3 (install verified 2026-10-02: Latest.log game-version line 0.4.7f7 + MelonLoader v0.7.3 Open-Beta, S1API.Il2Cpp.MelonLoader.dll file 3.2.1.0 / product 3.2.1-beta.7; content deep-verified against 0.4.7f6 / 2026-09-28 - most crash patterns are version-agnostic, but re-check against 0.4.7f7 when they trigger). Re-check after game updates. -->

# Schedule I — Troubleshooting & Crash Diagnostics

When a mod breaks the game, or a game update breaks a mod, work this skill before writing a fix. Most common issues already have a documented cause in **`AGENTS.md §5`** under one of the category headers — check first.

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

### Direct `Latest.log` Parsing (DEFAULT — zero-dependency triage)
`<GameDir>\MelonLoader\Latest.log` is the canonical live log feed. Use native PowerShell commands to inspect:
```pwsh
$log = "$env:SCHEDULE1_PATH\MelonLoader\Latest.log"

# Scan last 200 lines for errors and warnings
Get-Content $log -Tail 200 | Select-String -Pattern '\[ERROR\]|\[WARNING\]|Exception|WasCollected'

# Filter to a single mod
Get-Content $log -Tail 500 | Select-String -Pattern '\[NotesApp\]'

# Find the very first error (root cause)
Get-Content $log | Select-String -Pattern '\[ERROR\]|Exception' | Select-Object -First 1
```
See **[`references/logscan-and-logs.md`](references/logscan-and-logs.md)** for more PowerShell recipes and real-time streaming options.

### `s1interop` (PRE-FLIGHT + STATIC ANALYSIS)
```pwsh
s1interop doctor --il2cpp-game-path $env:SCHEDULE1_PATH
s1interop analyze "Source\Mods\NotesApp\src\NotesApp.csproj"
```
Catches `injected_type_missing_intptr_constructor`, `[HideFromIl2Cpp]` gaps, `[RegisterTypeInIl2Cpp]` issues.

### `ilspycmd` (LIVE ASSEMBLY VERIFICATION)
```pwsh
& (Get-Command ilspycmd -ErrorAction Stop).Source -t <Full.Type.Name> "$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll"
# Install if missing: dotnet tool install -g ilspycmd
```
Always verify API signatures exist before patching — see **[`../schedule1-knowledge/SKILL.md`](../schedule1-knowledge/SKILL.md)** §4.

---

## 3. Patch Silent No-Op (Game Runs, Mod Does Nothing)

The classic symptom: `OnInitializeMelon` logs "Ready" but `OnUpdate` work never appears, or "Watered!" never logs. Common causes, in order of frequency:

1. **Method-Inlining in IL2CPP.** Small methods (getters, one-liners) are inlined in the native build — Harmony patch installs but never fires. **Fix: patch the caller, not the inline target.**
2. **`HarmonyPatch` attribute without explicit target.** `[HarmonyPatch(typeof(X))]` without `(nameof(X.Method))` fails with "Undefined target method". **Fix: `harmony.Patch(methodInfo, new HarmonyMethod(...))` manually without marker attribute**, or supply full target spec.
3. **Scene is wrong.** The patched object's scene is not active. Verify with `[Mod].Logger.Msg($"Scene: {SceneManager.GetActiveScene().name}")`.
4. **Static-List empty at scene load.** E.g. `Property.OwnedProperties.Count == 0` even though player owns properties. **Fix: use `GameLifecycle.OnPreLoad` + `OnSceneWasLoaded("Main")` / `OnLoadComplete` instead of `OnGameplaySceneLoaded`** (do NOT subscribe `OnSaveInfoLoaded` - it fired 0 times on 0.4.7f6, see `schedule1-lifecycle-verify`).
5. **Mod dependency missing.** Optional dep via `[assembly: MelonOptionalDependencies("S1API")]` requires you to try-catch every S1API code path. Otherwise the mod throws before reaching your logic.

---

## 4. Common Error Patterns (Latest.log Decoder)

| Error string | Meaning | First fix to try |
|---|---|---|
| `Object was collected by IL2CPP GC` | Unity object destroyed or scene unloaded while mod holds reference | Guard with `obj != null && obj.Pointer != IntPtr.Zero && !obj.WasCollected` |
| `state_slot_-1.json` generated | SaveSlotNumber accessed before save info initialized | Check `info.SaveSlotNumber >= 0` before building slot path |
| Handler called N× per event / leak | Event subscribed in OnCreated/SceneLoad without unsubscribe | Use static event dispatcher or defensive unsubscribe/subscribe pattern |
| `MissingMethodException` on a game API | Game update changed or removed method signature | Inspect method in live assembly via `ilspycmd` |
| `[RegisterTypeInIl2Cpp]` failure: "Class not initialized" | Your `MonoBehaviour : Il2CppObjectBase` is missing the `IntPtr` ctor | Add `public Foo(IntPtr ptr) : base(ptr) { }` |
| `MelonLoader\Il2CppAssemblies\…` null reference | Game's IL2CPP proxy is exposing a stale handle | Check `Pointer != IntPtr.Zero && !WasCollected` |
| `HarmonyException: Cannot patch … final method` | IL2CPP strips method bodies → methods are final / sealed | Prefer Prefix/Postfix over Transpiler |
| `Class::Init signatures exhausted, using a substitute!` | IL2CPPInterop can't initialize all classes (>5–10 MB mod) | Reduce mod class count or split mod |
| 0xc0000005 (native AV) with no MelonLoader trace | Native crash after init | Mod binary size or corrupt interop bindings |
| `MissingFieldException` on a private field | IL2CPP field not registered for reflection | Use Harmony prefix/postfix on accessor method |

For an expanded reference (18+ error patterns + IL2CPP exception translations) see **[`references/common-errors.md`](references/common-errors.md)**.

---

## 5. Save-Load-Timing Failures

The Schedule I save-load pipeline is multi-phase and most static lists (`Property.OwnedProperties`, `NPCManager.Registered`, ...) are empty at `OnGameplaySceneLoaded`. The fix is to use the S1API lifecycle hooks (`OnPreLoad` / `OnSceneWasLoaded("Main")` / `OnLoadComplete`):

```csharp
// In Mod.cs OnInitializeMelon:
GameLifecycle.OnPreLoad            += OnPreLoad;
GameLifecycle.OnLoadComplete   += OnLoadComplete;

// Lifecycle order:
//   OnPreLoad / OnSceneWasLoaded("Main") -> reset caches / early refresh (verified order: scene 'Main' -> OnPreLoad -> OnLoadComplete).
//   OnSaveInfoLoaded does NOT fire on 0.4.7f6 - do not subscribe to it (see schedule1-lifecycle-verify).
//   OnLoadComplete    → after scene build complete
//                        (good time to attach UI / instantiate managers)
//   [VERIFIED 2026-09-29 (0.4.7f6): Scene 'Main' loaded -> OnPreLoad -> OnLoadComplete; OnSaveInfoLoaded fired 0 times]
```

Symptom: HUD looks empty, PhoneApp shows "(unknown)" properties, Owner-count = 0 even though save has 5 owned.

Historical reference pattern: **PotScanner v0.2.1** (2026-08-04) used the save-info hook to drop the 25-second retry - that hook later stopped firing (0.4.7f6, 2026-09-29); refresh on `OnSceneWasLoaded("Main")` instead.

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
| **PhoneApp blank/empty on 2nd open (after 1st close)** | `MelonEvents.OnUpdate.Unsubscribe(Update)` in `OnPhoneClosed()` — `OnCreated` fires only once per scene | Remove; defensive `-=`-before-`+=` only in `OnCreated` (see schedule1-phoneapp Rule 10). Same pattern for `-=` on static events (`OnPotsScanned`, `OnBalanceChanged`, `OnStateChanged`) |
| PhoneApp History/Notes wrong after slot switch | Global `state.json` instead of `slot_{n}.json` | Slot isolation via `LoadManager.ActiveSaveInfo` + `TryMigrateLegacy` — see schedule1-phoneapp Rule 11 (`CalculatorState.cs:65` Fix 2026-08-21) |
| Local UITheme shows wrong scale on 4K | Local `UITheme` duplicates `S1Mods.Shared.UITheme` | Let wrappers delegate (`InitializeForTextApp/Dashboard`), no local clamp math (`NotesApp.cs:33` Fix 2026-08-21) |
| Layout broken on player's screen but test OK on dev screen | DPI / Screen-Scale mismatch | Use `UITheme.Sp/Dp` (Method 3) — see `[schedule1-modding/ui-and-s1api.md]` |
| InputField keys trigger WASD movement | InputFocus not hooked | Register `NotesAppInputFocus`-style `MonoBehaviour` per `AGENTS.md §5` |
| UIButton.onClick silently fails | `new UnityAction(...)` IntPtr issue | Use `S1API.Utils.ButtonUtils.AddListener(...)` |
| HUD text disappears intermittently | Component destroyed (scene reload) | Re-resolve via `GameObjectResolver.FindComponentDeep<T>()` after `OnSceneWasLoaded` |
| PhoneApp icon: "Icon file not found" | `IconFileName = ""` or wrong path | Override `IconSprite` (return a Sprite directly, see PotScanner v0.2.0 fix) |
| Minigame/HUD-Sprite wrong after shape change | Cache ignores parameters (one `Sprite?` field for 2 radii) | Key cache by parameters (`Dictionary<string,Sprite>` `"{size}_{radius}"`) - see the parameter-keyed cache rule in `schedule1-modding/references/il2cpp-harmony-guide.md` (archived Minimap fix 2026-08-21) |
| Circle-mask/border wrong on size change | Single `_circleMaskSprite` instead of dict | `_circleMaskCache` keyed `"{size}"` / `"{size}_{thickness}"` — see Fix 2026-08-21 |

---

## 8. Known Fragile Areas (Empirical History)

These **WILL** bite you if you don't read first:

* **`Harmony.Patch` on a method with `MissingMethodException` in IL body** → fails at pin time, BEFORE your code runs. (Documented in `docs/pitfalls.md`.)
* **`DialogueHandler.get_activeDialogue()` removed in 0.4.6f11** → any mod touching this crashes at JIT. (Documented in `docs/pitfalls.md`.)
* **10.5 MB+ Mod class volume** → IL2CPPInterop `Class::Init signatures exhausted` warning → native AV 0xc0000005 → game dies. (DrugExpansion 1.0.0.)
* **Static-List Empty at Scene-Load** (see §5).
* **Reflection-Field-Set in IL2CPP** → unreliable, can write to wrong address — use Harmony-Patch instead.
* **`MelonLogger.Instance.Error` vs `MelonLogger.Error`** — both must be patched for full coverage. (See il2cpp_modding_rules.md rule #3.)
* **Multiplayer Host Authority** → Mod logic running on both host and client causes double payments/executions. Always wrap with `IncomeEngine.IsHostOrSingleplayer()` or equivalent.
* **Procedural Audio Crashing** → Native methods like `MoneyManager.Instance.PlayCashSound()` will throw if the `MoneyManager` instance isn't ready or if called off main thread. Always null check `MoneyManager.Instance` or catch exceptions.
* **PhoneApp-Update-Death (2026-08-20, 5 mods)** → `Unsubscribe` in `OnPhoneClosed` permanently killed `MelonEvents.OnUpdate`/event handler (OnCreated fires only once). Apps blank after 1st close. Fix + diagnosis: see §7 UI debugging.
* **Sprite cache without parameter key (2026-08-20)** - `private static Sprite? _x` cached, but callers with different parameters (radius/thickness) mean the first caller wins (wrong mask/border). Fix: `Dictionary<string, Sprite>` cache keyed by parameter (archived Minimap `MinimapTextures.cs`, fixed 2026-08-21).
* **ModConfig Dictionary loss (2026-08-20)** → `ModConfig<T>` does NOT persist `Dictionary/List` properties (TOML limit) → after restart defaults. Fix: SafeStorage JSON sidecar (ConfigJsonStore pattern).
* **Global State-File Leak (2026-08-21)** → `CalculatorState.cs:66` `calculator_state.json` global instead of `slot_{n}` → Slot-A leaked into Slot-B. Fix: `GetActiveSlotSuffix()` + `TryMigrateLegacy` (`CalculatorState.cs:65-93`, Rule 11).
* **Local UITheme Duplication (2026-08-21)** → `NotesApp.cs:33`/`PotScannerApp.cs:19`/`CalculatorApp.cs:22` local `UITheme` with deviating `RefHeight/Clamp` (750/2.5, 900/1.20, 850/1.30) vs Shared `750/2.0` / `900/1.20` → DPI drift. Fix: wrapper delegates to `S1Mods.Shared.UITheme`.
* **MoreSaveSlots Raw File-Write (2026-08-21)** → `MoreSaveSlotsConfig.cs:57` `File.WriteAllText` + `SaveRenameService.cs:71` `Copy/Write/Replace` without atomic/.bak protection → crash corrupts `Game.json`. Fix: `SafeStorage.SaveTextAtomic`.
* **BusinessIncome Snapshot-Loss (2026-08-21)** → `PayoutStateStore.Revert` set `LastPaid=-1` instead of snapshot → History Day 5 lost → double-payout. Fix: `_pendingPrevLastPaid/_pendingPrevPerBusiness` (`PayoutStateStore.cs:20-21`).
* **Field-Accessor Not Patchable (2026-08-21)** → `BaseItemDefinition.get_DefaultStackLimit` field accessor → `Il2CppInterop can't be patched` (`Latest.log:17:43:04.438`). Fix: remove patch (`Mod.cs:109`), resolve via engine scan.

---

## 9. When to Roll Back vs. Fix Forward

* **Game-version mismatch confirmed** → roll back via `.bak`. Quick, low-risk.
* **One-method API drift** → refresh the method signature in your patch, verify with `ilspycmd`.
* **Wholesale API refactor in S1API** → re-run `s1interop analyze` to map new pattern, refactor conservatively.
* **More than ~3 mods at fault** → roll back to last-known-good state, re-introduce mods one by one with a log check between each.

### Performance bug: which mod is blocking the main thread?

When the symptom is "FPS dropped to 1, but `Latest.log` shows no exceptions, no errors, no warnings", the block is somewhere below the level of normal logging — a hot Harmony prefix, a per-frame `FindObjectOfType`, an unbounded MonoBehaviour Update loop, a save-Load hook that re-registers every frame, or the game engine itself. Use binary search to isolate it.

**Step 1 — Framework-only baseline.** Archive every DLL in `Game/Mods/` *except* `S1API.Il2Cpp.MelonLoader.dll` and `Shared.dll` (the framework loaders — see the `schedule1-modding` "Never archive Shared.dll" rule). Keep `Plugins/S1APILoader.MelonLoader.dll` and `UserLibs/S1MAPI_Il2cpp.dll` as-is. Start the game, load the save, check FPS.

| Baseline FPS | Conclusion |
|---|---|
| 100+ in main menu, 100+ in save | Mods are at fault. Go to Step 2. |
| 100+ in main menu, 1 in save | Save file is corrupt — one mod wrote garbage on last save. Delete the save and start fresh. |
| 1 in main menu | Not a mod problem. Driver, hardware, Windows, or game. Check GPU driver, VSync, fullscreen mode, Game Bar overlay. |

**Step 2 — Binary-search the mod set.** Restore half of the archived mods into `Game/Mods/`, retest.

| Subset FPS | Where the bug lives |
|---|---|
| 100+ | In the *still-archived* half |
| 1 | In the *now-restored* half |

Each round halves the candidate set. With N mods it takes at most ⌈log₂(N)⌉ rounds (5 rounds for ~30 mods). Always run the binary search, never reintroduce all mods at once — the binary pattern is the entire point.

**Step 3 — When one mod is left, audit its hot paths.** Look in order:
1. `FindObjectOfType` / `GetComponent` calls inside `OnUpdate` (cache in `Awake` instead).
2. `Resources.FindObjectsOfTypeAll<T>()` calls inside `OnUpdate` (never do this).
3. Per-frame Harmony prefixes that touch IL2CPP properties (`__instance.SomeProp.OtherProp.ID` chains marshal on every call).
4. `OnSceneWasLoaded` or save hooks that re-register every frame instead of using `[HarmonyPrepare]` / lifecycle flags.

**Step 4 — Audit your own diagnostic mods.** A counter/logging MelonMod that iterates GameObject hierarchies per frame *is itself* the bottleneck. See [`../schedule1-modding/references/diag-mods-tick-counter.md`](../schedule1-modding/references/diag-mods-tick-counter.md) for the correct pattern (verified 2026-08-28: the wrong pattern froze the game at 1 FPS; the right pattern ships clean).

**Sandbox path.** Keep archived mods under `<Workspace>/.scratch/archived-mods/<ModName>.dll` (not in a temp folder) so the path round-trips survive and you don't have to re-download.

---

## 10. Escalation

If a diagnostic step exceeds 30 minutes without resolution:
1. Capture the full `Latest.log` and the failing call stack.
2. Identify the **first** exception (not the last — IL2CPP cascades).
3. Add a minimal repro: scaffold a new empty mod via `pwsh Tools/new-mod.ps1 -Name Repro` and isolate.
4. Check `Source/Archive/` or active workspace mods for similar patterns — fixes are often already documented in `AGENTS.md §5`.

---

## 11. Related Skills

* `schedule1-modding` — mod runbook (build, deploy, architecture patterns) — load for any modding task.
* `schedule1-s1api` — S1API reference (Saveables, Quests, NPCs, lifecycle) — load when the issue is around S1API behavior.
* `schedule1-s1mapi` — S1MAPI reference (procedural meshes, buildings, GLTF) — load when the issue is around procedural geometry.
* `schedule1-knowledge` — KB navigation — load when you need to find the right doc file.

## 12. Open Questions (review backlog)

* **Multiplayer / FishNet SyncVar timing** — `references/save-load-timing.md:196` mentions SyncVar timing briefly but lacks a diagnostic pattern. When this gets a real diagnostic, cross-link here and remove this entry. Tracked since 2026-09-02 (skill-health-check).

---

<!-- TODO next-review: When the open-question entry above is resolved, remove this comment. -->
