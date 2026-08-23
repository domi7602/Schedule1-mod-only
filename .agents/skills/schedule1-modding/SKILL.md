---
name: schedule1-modding
description: >-
  Expert guide and runbook for developing, building, testing, and maintaining MelonLoader IL2CPP C# mods for Schedule I v0.4.6f13 (TVGS) in the Schedule I Modding Workspace (C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule 1 Modding).
  Use this skill whenever creating new mods, modifying existing mods, writing Harmony patches, building PhoneApps via S1API, troubleshooting IL2CPP/Unity/MelonLoader issues, building solutions, or implementing UI features for Schedule I.
  Keywords: Schedule I, MelonLoader, IL2CPP, Harmony, HarmonyPatch, PhoneApp, S1API, Unity 2022.3, mod scaffolding, SafeStorage, UITheme.
---

# Schedule I — Modding Skill & Runbook

This skill provides full procedural knowledge, conventions, and architectural guidelines for developing MelonLoader IL2CPP C# mods for **Schedule I v0.4.6f13** (TVGS) in `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule 1 Modding`.

> **Version check (last verified: 2026-08-21):** Before writing patches or building, confirm the installed game version and S1API version still match this skill. If the game was updated, follow the Update Runbook (§6) first.

---

## 1. Toolchain & Workspace Setup

* **Workspace Root**: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule 1 Modding`
* **Game Path**: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` (Override via `$env:SCHEDULE1_PATH`)
* **Runtime**: MelonLoader 0.7.3 (IL2CPP, Unity 2022.3)
* **Target Framework**: `net6.0`, C# 12, `Nullable` enabled

### Layout
```text
Source/Mods/        Mod projects + shared lib + Directory.Build.props/targets + S1Mods.sln
ThirdParty/         External frameworks & sources (Sideload, hash, MoreDrugs)
Tools/              PowerShell automation scripts: build-all.ps1, new-mod.ps1, gen-sln.ps1
Knowledge/          Decompiles, system analyses, framework references, maps
AGENTS.md           Single source of truth for mod inventory & current status
```

**Sync rule:** After creating, renaming, removing, or changing the status of any mod, update `AGENTS.md` and the mod's `CHANGELOG.md` in the same change. `AGENTS.md` must never drift from reality.

---

## 2. Core Workflows

### A. Creating a New Mod
Run the scaffolding script from the workspace root:
```pwsh
pwsh Tools/new-mod.ps1 -Name "MyNewMod" -Author "Dominik" -Version "0.1.0"
pwsh Tools/gen-sln.ps1
```
This generates:
* `Source/Mods/MyNewMod/src/MyNewMod.csproj` & `Mod.cs`
* `Source/Mods/MyNewMod/docs/mod.json`, `README.md`, `CHANGELOG.md`
* Updates `Source/Mods/S1Mods.sln`

### B. Building & Deploying Mods
```pwsh
# Build the entire solution (auto-deploys DLLs to <GameDir>\Mods\)
pwsh Tools/build-all.ps1

# Or build via dotnet CLI directly:
dotnet build Source/Mods/S1Mods.sln -c Release

# Or build a single mod:
dotnet build Source/Mods/NotesApp/src/NotesApp.csproj -c Release
```
*Output artifacts are deployed automatically to `<GameDir>\Mods\<ModName>.dll` via `Directory.Build.targets`.*

### C. Testing a Mod In-Game
1. Build in `Release` and confirm the DLL landed in `<GameDir>\Mods\` (check timestamp).
2. Launch the game via Steam (MelonLoader console window opens alongside).
3. Watch the MelonLoader console for `[ModName]` init lines and any red ERROR/exception output.
4. Check the log file after the session: `<GameDir>\MelonLoader\Latest.log` (previous session: `MelonLoader\Logs\`).
5. Verify in-game: mod loads without exceptions, UI opens/closes, hotkeys work, config persists across restart (check `UserData\` for the written config/save file).
6. Test the failure path too: corrupt the mod's JSON save once and confirm the `.bak` recovery via `SafeStorage.TryLoad` kicks in.

**Definition of Done (new mod or feature):**
- [ ] Builds with 0 warnings/errors in Release
- [ ] DLL deployed and loaded (visible in `Latest.log`)
- [ ] Feature verified in-game, including scene transitions (Main Menu → Game → Main Menu)
- [ ] No exceptions in `Latest.log` during a full play session
- [ ] Persistence round-trip works (save → restart → load)
- [ ] `AGENTS.md`, `README.md`, `CHANGELOG.md` updated

---

## 3. Architecture & Best Practices

Read the sub-guides **before** the matching task — not just "for reference":
* **Writing or changing Harmony patches / touching IL2CPP types** → read [IL2CPP & Harmony Patching Guide](./references/il2cpp-harmony-guide.md) first.
* **Persisting data, configs, logging, or input handling** → read [Architecture & S1Mods.Shared](./references/architecture-and-shared.md) first.
* **Building any UI (PhoneApp, HUD, menus) or console commands** → read [S1API & UI Frameworks Guide](./references/ui-and-s1api.md) first.

### Key Rules Checklist:
1. **Safe Persistence (Atomic + .bak)**: Always use `S1Mods.Shared.SafeStorage.SaveAtomic` / `SaveTextAtomic` for JSON/config/save writes — never `File.WriteAllText` / `File.Copy+Replace` manually. Provides `.tmp` staging + `.bak` rotation + crash recovery. Verified 2026-08-21: `MoreSaveSlots` lost `.bak` by using raw `File.WriteAllText` (`MoreSaveSlotsConfig.cs:57`, `SaveRenameService.cs:71`) → migrated to `SafeStorage`. Test `.bak` recovery by corrupting one save file.
2. **Slot-Isolation (Save-Slot Awareness)**: Every mod that persists per-save data **must** suffix files with `slot_{SaveSlotNumber}` (e.g. `calculator_state_slot_{n}.json`, `notes_slot_{n}.json`, `payout_state_slot_{n}.json`, `street_items_slot_{n}.json`). Source: `LoadManager.Instance.ActiveSaveInfo.SaveSlotNumber` (`PayoutStateStore.cs`, `NotesApp.cs`, `StreetPropertyManager.cs`). Add `TryMigrateLegacy()` moving legacy global file once — verified 2026-08-21: `CalculatorApp` leaked Slot-A history without isolation.
3. **InputFocus Hook**: When creating UI text inputs, register an `InputFocus` hook (canonical API: `HotkeyManager.IsInputFieldFocused()`; `NotesAppInputFocus` is the reference implementation) to disable game WASD / movement controls while typing.
4. **0-Allocation Polling**: In periodic update/polling routines (e.g. 2s container scan), avoid heap allocations inside loops to prevent IL2CPP GC spikes. Use pre-allocated pools for dynamic UI elements (e.g. Minimap blips 64-pool).
5. **Responsive UI ("Method 3")**: The workspace's established scaling approach — dynamic `UITheme.Sp/Dp` scaling with a clamped screen height factor (`Mathf.Clamp(ActualHeight / 900f, 0.75f, 1.20f)`). Details in the UI guide. **Single source of truth: `S1Mods.Shared.UITheme` (Shared/UITheme.cs)** — mod wrappers delegate; never duplicate. Verified 2026-08-21: 3 local duplicates (`NotesApp`, `PotScanner` 900/0.75-1.20, `CalculatorApp` 850/0.75-1.30) → migrated to delegates `InitializeForTextApp` / `InitializeForDashboard`.
6. **PhoneApp Subscription Lifetime**: `OnCreated()` fires ONCE per scene (S1API auto-discovery). **Never** `Unsubscribe(MelonEvents.OnUpdate)` or `-=` static event handlers inside `OnPhoneClosed()` — the app goes blank after the first close. Defensive `-=`-before-`+=` in `OnCreated` only. See `schedule1-phoneapp` Rule 10.
7. **Money/Transaction Ordering + Snapshot Revert**: Mark paid **in memory BEFORE** the bank transaction, revert on **tx failure** restoring the **snapshot** (not `-1`), persist AFTER success. `BusinessIncome` 2026-08-21: `Revert` set `LastPaid=-1` → lost history → re-payout Day 5. Fix: snapshot `prevLastPaid + per-business Dict` in `MarkInMemoryPaid`, restore in `Revert`. For purchases: pre-create item instances, pay, transfer with refund-rollback. Never refund if payment never executed. See `architecture-and-shared.md` §5.
8. **ModConfig TOML Limit**: `ModConfig<T>` cannot persist `Dictionary<K,V>`/`List<T>` properties — use a SafeStorage JSON sidecar (`ConfigJsonStore` pattern). See `architecture-and-shared.md` §3.
9. **Culture-Safe Number Parsing**: Always `float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, ...)` + `ToString("0.##", CultureInfo.InvariantCulture)` — DE/other locales use `,` as decimal separator.
10. **Sprite/Texture Cache Keying**: Dictionary-key every procedural `Sprite`/`Texture2D` by its constructor params (`"{size}_{radius}_{thickness}"`). Single-field `Sprite? _x` returns first caller's asset for all callers — `MinimapTextures.cs:9` circle-mask bug 2026-08-21 fixed via `_circleMaskCache/_circleBorderCache`.
11. **Field-Accessor Not Patchable**: `BaseItemDefinition.get_DefaultStackLimit` is a field accessor → Il2CppInterop `can't be patched`. Don't Harmony-patch it — `StackLimitMod` 2026-08-21 removed dead postfix; engine uses direct scan/field write instead. Check `ilspycmd` before patching.
12. **Knowledge Base Discovery** (6 KB areas — see `schedule1-knowledge` for the detailed index):
   - `Knowledge/Game-Reference/Decompiles/Assembly-CSharp/` (~2100 files): Vanilla symbols & exact method signatures — **mandatory before every patch**.
   - `Knowledge/Analysis/Systems/` (65 files): System architecture, events, UI patterns — **quick-lookup for hooks**.
   - `Knowledge/Frameworks/S1API/Decompiles/3.2.0/` (962 files): S1API wrapper methods — **always check before accessing vanilla directly** (prefer the wrapper).
   - `Knowledge/Analysis/Learnings/Concepts/` (17 files): IL2CPP pitfalls, save-loads, PhoneApp patterns — **cross-cutting knowledge**.
   - `Knowledge/Analysis/Learnings/ThirdParty/<Mod>.md` (121 mods, 4804 total files 67 MB): How did others solve this? — **mandatory before reinventing**. ⚠️ Own mods not yet mirrored there (see `Knowledge/README.md`).
   - `Knowledge/Frameworks/MelonLoader/`: MelonLoader 0.7.3 internals (lifecycle, patches).
13. **Multiplayer Host Authority**: Always guard passive income/state changes with `IncomeEngine.IsHostOrSingleplayer()` to prevent double execution on clients.
14. **Idempotency**: Use checks like `LastPaidElapsedDay` to ensure scene reloads or save reloads don't trigger duplicate transactions (see `BusinessIncome` snapshot fix above).

### Never do (hard guardrails):
* Never use `foreach` or LINQ on `Il2CppSystem.Collections.Generic.List<T>` — indexed `for` only.
* Never call `FindObjectOfType` / `GetComponent` inside `OnUpdate` or tick loops — cache in `Awake`/`Start`/scene-load handlers.
* Never access cached IL2CPP objects after scene unload without a null/`WasCollected` check.
* Never write save/config files with plain `File.WriteAllText` — always `SafeStorage.SaveAtomic`.
* Never let a Harmony patch throw — wrap risky patches with `PatchGuard` so a game update degrades gracefully instead of crashing.

---

## 4. Troubleshooting

Symptom → likely cause → fix. Log file is always `<GameDir>\MelonLoader\Latest.log`.

### Mod does not load at all
* **DLL missing in `<GameDir>\Mods\`** → build ran but deploy failed → check `Directory.Build.targets` output path and `$env:SCHEDULE1_PATH`; rebuild with `pwsh Tools/build-all.ps1`.
* **`TypeLoadException` / `MissingMethodException` on startup** → compiled against stale interop assemblies after a game update → run the Update Runbook (§6), rebuild all mods.

### Harmony patch not firing
* **Target method not found warning in log** → game update renamed/changed the signature → look up the current symbol in `Knowledge/Game-Reference/Decompiles/Assembly-CSharp/` and update the patch; `PatchGuard` should have caught this — if it didn't, extend it.
* **Patch applies but never triggers** → wrong method overload or the patched code path isn't the one the game actually uses → verify against the decompile; add a temporary `ModLogger` line in the prefix to confirm.

### Crash / black screen on startup
* **Hard crash with no managed exception** → unguarded patch throwing inside IL2CPP code, or accessing a destroyed object → wrap in `PatchGuard`, add null/`WasCollected` checks, check scene-transition handling.
* **Crash only after returning to Main Menu** → stale cached gameplay objects → clear caches in `OnSceneWasUnloaded`.

### Performance / stutter
* **Periodic GC spikes** → allocations in polling loops (LINQ, `foreach` on IL2CPP lists, boxing) → apply the 0-allocation rules from the IL2CPP guide.
* **UI looks broken on some resolutions** → raw pixel values instead of `UITheme.Dp/Sp` → migrate to the scaling engine (UI guide §2).

### Input conflicts
* **Character walks while typing in a text field** → missing InputFocus hook → implement per Architecture guide §2.

---

## 5. Update Runbook (after a game patch)

1. Note the new game version; compare with the version in this skill's header.
2. Refresh decompiles/interop in `Knowledge/Game-Reference/Decompiles/Assembly-CSharp/`.
3. Rebuild the full solution — collect all `TypeLoadException`-relevant compile errors.
4. Grep `Latest.log` after first launch for `PatchGuard` warnings; fix affected patches against the new decompile.
5. Run the Definition-of-Done test pass for every affected mod.
6. Bump version references in this skill, `AGENTS.md`, and affected mods' `CHANGELOG.md`.

---

## 6. Maintenance Notes

* This skill is a living document: when a recurring mistake or a new convention emerges, add it here (guardrail, checklist item, or troubleshooting row) instead of relying on memory.
* Keep `SKILL.md` under ~500 lines; move growing detail into the matching file under `references/` and link it with a read-trigger.
* **Related skills** (load via the `skill` tool):
  * `schedule1-phoneapp` — for PhoneApp-focused work (UI, lifecycle, InputFocus)
  * `schedule1-s1api` — for S1API-specific API questions (Saveables, Quests, NPCs, Items, ...)
  * `schedule1-s1mapi` — for procedural geometry, buildings, GLTF, world tools
  * `schedule1-knowledge` — for research and KB navigation
  * `schedule1-troubleshooting` — for crash diagnostics and IL2CPP pitfalls
