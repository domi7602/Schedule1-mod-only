---
name: schedule1-modding
description: >-
  Expert guide and runbook for developing, building, testing, and maintaining MelonLoader IL2CPP C# mods for Schedule I v0.4.6f13 (TVGS) in the Schedule I Modding Workspace (Schedule1-mod-only-main).
  Use this skill whenever creating new mods, modifying existing mods, writing Harmony patches, building PhoneApps via S1API, troubleshooting IL2CPP/Unity/MelonLoader issues, building solutions, or implementing UI features for Schedule I.
  Keywords: Schedule I, MelonLoader, IL2CPP, Harmony, HarmonyPatch, PhoneApp, S1API, Unity 2022.3, mod scaffolding, SafeStorage, UITheme.
---

> Version anchor: Game v0.4.6f13 / S1API 3.2.0 / MelonLoader 0.7.3 (verified 2026-09-12). Re-check after any game or S1API update.

# Schedule I — Modding Skill & Runbook

This skill provides full procedural knowledge, conventions, and architectural guidelines for developing MelonLoader IL2CPP C# mods for **Schedule I v0.4.6f13** (TVGS) in `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule1-mod-only-main`.

> **Version check (last verified: 2026-09-12):** Before writing patches or building, confirm the installed game version and S1API version still match this skill. If the game was updated, follow the Update Runbook (§5) first.

---

## 1. Toolchain & Workspace Setup

* **Workspace Root**: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule1-mod-only-main` (located inside the game directory)
* **Game Path**: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` (Override via `$env:SCHEDULE1_PATH`)
* **Runtime**: MelonLoader 0.7.3 (IL2CPP, Unity 2022.3)
* **Target Framework**: `net6.0`, C# 12, `Nullable` enabled

### Layout
```text
Source/Mods/        Mod projects + Shared lib + Directory.Build.props/targets + S1Mods.sln
GameReferences/     Local decompiles: decompiled/Assembly-CSharp/ (Il2CppScheduleOne.*)
ThirdParty/         External frameworks & sources (S1API, S1MAPI, PhoneScroll, S1MCPServer)
Tools/              PowerShell automation: build-all.ps1, new-mod.ps1, gen-sln.ps1, deploy-thirdparty.ps1, bump-version.ps1
Skills/             20 modular modding & game system skills (including all 64 system analyses)
AGENTS.md           Single source of truth for mod inventory & current status
```

**Deploy Convention (since 2026-09):**
- `<GameDir>\Mods\` $\rightarrow$ Loadables for MelonLoader (`*.dll`, `*.png`, `*.bundle`)
- `<GameDir>\UserData\<ModName>\` $\rightarrow$ Metadata and debug symbols (`mod.json`, `<ModName>.pdb`)
- ThirdParty DLLs auto-deployed via `Tools/deploy-thirdparty.ps1` with whitelist in `ThirdParty/.deployignore`.

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
* `Source/Mods/<ModName>/src/<ModName>.csproj` & `Mod.cs`
* `Source/Mods/<ModName>/docs/mod.json`, `README.md`, `CHANGELOG.md`
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

### C. Version Bump & Sync (AGENTS.md ↔ Source)

The four source-of-truth files that MUST agree on every version:
1. `Source/Mods/<Mod>/src/Mod.cs` → `[assembly: MelonInfo(..., "X.Y.Z", "Dominik")]`
2. `Source/Mods/<Mod>/docs/mod.json` → `"version": "X.Y.Z"`
3. `Source/Mods/<Mod>/docs/CHANGELOG.md` → top entry `## X.Y.Z (YYYY-MM-DD)`
4. `AGENTS.md` → mod-matrix row `| **<Mod>** | ✅ active (vX.Y.Z, ...)`

**The script**:
```pwsh
pwsh Tools/bump-version.ps1 -Mod <Name> -Version <X.Y.Z> -DryRun   # always preview first
pwsh Tools/bump-version.ps1 -Mod <Name> -Version <X.Y.Z>           # then apply
```

**Feature-Diff Gate (MANDATORY before any bump)** — verified 2026-08-24 on BackpackMod + AutoPackagingStation:
Before running the script, build a feature table that compares AGENTS.md claims against actual source. For each claim, grep for the keyword (e.g. `Mannequin`, `Rotation`, `360`, `ObjLoader`, `Spine`, `Tier`) in `Source/Mods/<Mod>/src/`. If the claim is **real**, the bump is honest. If a claim is **aspirational** (in AGENTS.md but not in source), AGENTS.md is wrong — either fix AGENTS.md, or write the missing feature first. Never bump to a version number whose features you cannot prove exist.

Two valid outcomes from the diff:
- **Bump + CHANGELOG with real features** → standard release path.
- **Bump + CHANGELOG that explicitly says "documentation sync, no code changes since 0.X.0"** → honest for mods that drifted. Better than lying about features.

**Three known `bump-version.ps1` pitfalls** (verified 2026-08-24, may already be fixed in newer revisions):
1. The CHANGELOG header regex `(?m)^# Changelog\s*\r?\n` does NOT match `# Changelog - ModName` (e.g. AutoPackagingStation). When this fails, the script prepends a duplicate `# Changelog` + `## X.Y.Z` block ABOVE the existing file. Always read the CHANGELOG after the bump and clean up duplicates manually.
2. The version-match regex `## $NewVersion\b` does NOT match `## [X.Y.Z]` (brackets). Mods that use the `[X.Y.Z]` convention get a duplicate entry prepended.
3. The auto-prepend body `- Version bump.` is dangerous when the prior CHANGELOG was empty or near-empty (e.g. BackpackMod had `## 0.1.0 - Initiale Version.`). It produces a release note that implies a stable prior build. Workaround: pre-populate the CHANGELOG with a real `## X.Y.Z (date) - <real-feature-list>` BEFORE running the script. The script's dedup check then skips prepending.

Mitigation: always read the post-bump CHANGELOG.md immediately after the script run and clean up duplicates/placeholder text.

### D. Spec-First Workflow for Fixes and Changes (mandatory in the Hermes group chat)

**Rule (2026-08-27, group-chat convention with `@gatekeeper` + `@designer`):** Never start a code change just because a user reported a problem. The maker-checker chain (`@coder` writes, `@gatekeeper` reviews) requires an explicit green light after a Spec, BEFORE any `dotnet build`.

**When this applies:**
- Bug fixes, refactors, new features touching >1 file
- Any change to a hot path (Harmony prefix/postfix, Update loops, polling)
- Any change to save/load lifecycle hooks (`OnPreLoad`, `OnSaveInfoLoaded`, `OnLoadComplete`, `OnSaveComplete`)

**Spec template (paste into the chat before any code touches a file):**
1. **Trigger / problem** — what's broken, where, repro
2. **Spec items as a numbered checklist** — concrete acceptance criteria, NOT aspirational
3. **Pitfalls / edge cases** — list each one with the chosen handling (e.g. "What if X happens during a scene reload? — ResetState() before spawn cycle")
4. **Logging / observability** — what counters or log lines prove the fix fires
5. **Build order** — "first A (critical), then B/C/D"
6. **Open questions** — anything ambiguous; ask before coding, not during

**Workflow:**
1. User reports problem → coder reads code, finds root cause
2. Coder drafts Spec (template above) in the chat → gatekeeper reviews, pokes holes, suggests edge cases
3. Spec gets explicit "grün" / "spec ok, bau" → coder builds in the order listed
4. Build green → coder reports what changed, where, what to test → gatekeeper does the review

**Don't do:**
- Don't ask "darf ich bauen?" — write the Spec, wait for green light, then build
- Don't skip the Spec "just because it's a small fix" — small fixes have the biggest hidden regressions
- Don't list build-order bullets that aren't in the chat (e.g. "I'll also fix X while I'm in there" — out of scope; surface it as a separate Spec if needed)

**Live reference:** CustomSkateboard `IsInstanceTuned` early-out fix (Spec → grün → build green) and HomelessMod F-key + slot-switch fix (Spec → 3 answer round → grün with edge-case list → build green) both ran this way in 2026-08-27 and shipped without a revert.

### E. Testing a Mod In-Game
1. Build in `Release` and confirm the DLL landed in `<GameDir>\Mods\` (check timestamp).
2. Launch the game via Steam (MelonLoader console window opens alongside).
3. Watch the MelonLoader console for `[ModName]` init lines and any red ERROR/exception output.
4. Check the log file after the session: `<GameDir>\MelonLoader\Latest.log` (previous session: `MelonLoader\Logs\`).
5. Verify in-game: mod loads without exceptions, UI opens/closes, hotkeys work, config persists across restart (check `UserData\` for the written config/save file).
6. Test the failure path too: corrupt the mod's JSON save once and confirm the `.bak` recovery via `SafeStorage.TryLoad` kicks in.

### F. Definition of Done (new mod or feature):

- [ ] Builds with 0 warnings/errors in Release
- [ ] DLL deployed and loaded (visible in `Latest.log`)
- [ ] Feature verified in-game, including scene transitions (Main Menu → Game → Main Menu)
- [ ] No exceptions in `Latest.log` during a full play session
- [ ] Persistence round-trip works (save → restart → load)
- [ ] `AGENTS.md`, `README.md`, `CHANGELOG.md` updated

### G. Bulk-Decompile Triage (when user drops a folder of Nexus/third-party mods)

When asked to "decompile these N mods and write skills from the learnings" — **decompile nothing first, triage first.** Verified workflow 2026-08-26 with 12 Nexus/DooDesch mods (4 skills extracted, 8 skipped):

1. **Sort into 4 buckets** before any decompile:
   - **Already in workspace** (e.g. Sideload repo) → skip, just version-compare.
   - **Asset-bombs** (DLL >50 MB, mostly bundles/textures) → skip, no skill value.
   - **Single-purpose feature mods** (custom NPC, mini-game, specific shop) → decompile but expect "no new pattern."
   - **Generic infrastructure mods** (config systems, save engines, reflection, hot-reload, harmony bootstrap) → high-skill-yield; decompile these first.
2. **Estimate pattern-yield by size:** <50 KB core DLLs with 5–30 files are almost always high-yield. 1–10 MB with 100+ files are usually feature-heavy and low-yield. >50 MB is asset-bomb.
3. **Triage before extraction** keeps skill count realistic — without triage, 12 mods → 12 skills (8 of which would be empty wrappers around a single feature). With triage, 12 mods → 4–5 class-level skills that actually generalize.
4. **Ethically-flavored mod?** (cheat menus, duplication exploits, anything that breaks the game for other players in MP) → document as "intentionally skipped" in the session log but do **not** create a skill from it.
5. **Decompile command:** `ilspycmd <dll> -o <outdir> -p` — silently ignore the "update available" nag at the top of output. Expect 1–30 sec per small DLL, 5–15 min per >10 MB DLL.

---

## 3. Architecture & Best Practices

Read the sub-guides **before** the matching task — not just "for reference":
* **Writing or changing Harmony patches / touching IL2CPP types** → read [IL2CPP & Harmony Patching Guide](./references/il2cpp-harmony-guide.md) first.
* **Persisting data, configs, logging, or input handling** → read [Architecture & S1Mods.Shared](./references/architecture-and-shared.md) first.
* **Building any UI (PhoneApp, HUD, menus) or console commands** → read [S1API & UI Frameworks Guide](./references/ui-and-s1api.md) first.

### Key Rules Checklist:
1. **Safe Persistence (Atomic + .bak)**: Always use `S1Mods.Shared.SafeStorage.SaveAtomic` / `SaveTextAtomic` for JSON/config/save writes — never `File.WriteAllText` / `File.Copy+Replace` manually. Provides `.tmp` staging + `.bak` rotation + crash recovery. Verified 2026-08-21: `MoreSaveSlots` lost `.bak` by using raw `File.WriteAllText` (`MoreSaveSlotsConfig.cs:57`, `SaveRenameService.cs:71`) → migrated to `SafeStorage`. Test `.bak` recovery by corrupting one save file.
2. **Slot-Isolation & Slot -1 Guard**: Every mod that persists per-save data **must** suffix files with `slot_{SaveSlotNumber}` (e.g. `calculator_state_slot_{n}.json`, `notes_slot_{n}.json`, `payout_state_slot_{n}.json`, `street_items_slot_{n}.json`). **Critical Guard:** In IL2CPP, always check `info != null && info.Pointer != IntPtr.Zero && !info.WasCollected && info.SaveSlotNumber >= 0`. Never write or generate `slot_-1.json`! Add `TryMigrateLegacy()` moving legacy global file once.
3. **IL2CPP Native Lifecycle & WasCollected Safety (2026-09-11)**: When checking Unity IL2CPP objects, standard C# `!= null` can evaluate to true even when the native C++ object is destroyed. Always check `obj != null && obj.Pointer != IntPtr.Zero && !obj.WasCollected`. In UI loops or event handlers, return early if collected.
4. **Static Event Dispatchers & Leak Prevention (2026-09-11)**: Never subscribe instance methods of ephemeral UI components (like PhoneApp panels or screen rows) directly to static events (`Money.OnBalanceChanged`, `GameLifecycle.*`, `PotTracker.OnPotsScanned`) without a clean unsubscribe. Prefer static dispatchers or unhook on unload/dispose to prevent subscriber leaks and `WasCollected` exceptions across scene transitions.
5. **InputFocus Hook**: When creating UI text inputs, register an `InputFocus` hook (canonical API: `HotkeyManager.IsInputFieldFocused()`; `NotesAppInputFocus` is the reference implementation) to disable game WASD / movement controls while typing.
6. **0-Allocation Polling**: In periodic update/polling routines (e.g. 2s container scan), avoid heap allocations inside loops to prevent IL2CPP GC spikes. Use pre-allocated pools for dynamic UI elements (e.g. Minimap blips 64-pool).
7. **Responsive UI ("Method 3")**: The workspace's established scaling approach — dynamic `UITheme.Sp/Dp` scaling with a clamped screen height factor (`Mathf.Clamp(ActualHeight / 900f, 0.75f, 1.20f)`). Details in the UI guide. **Single source of truth: `S1Mods.Shared.UITheme` (Shared/UITheme.cs)** — mod wrappers delegate; never duplicate.
8. **PhoneApp Subscription Lifetime**: `OnCreated()` fires ONCE per scene (S1API auto-discovery). **Never** `Unsubscribe(MelonEvents.OnUpdate)` inside `OnPhoneClosed()` — the app goes blank after the first close. Defensive `-=`-before-`+=` in `OnCreated` only. See `schedule1-phoneapp` Rule 10.
9. **Money/Transaction Ordering + Snapshot Revert**: Mark paid **in memory BEFORE** the bank transaction, revert on **tx failure** restoring the **snapshot** (not `-1`), persist AFTER success. For purchases: pre-create item instances, pay, transfer with refund-rollback. Never refund if payment never executed. See `architecture-and-shared.md` §5.
10. **ModConfig TOML Limit**: `ModConfig<T>` cannot persist `Dictionary<K,V>`/`List<T>` properties — use a SafeStorage JSON sidecar (`ConfigJsonStore` pattern). See `architecture-and-shared.md` §3.
11. **Culture-Safe Number Parsing**: Always `float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, ...)` + `ToString("0.##", CultureInfo.InvariantCulture)` — DE/other locales use `,` as decimal separator.
12. **Sprite/Texture Cache Keying**: Dictionary-key every procedural `Sprite`/`Texture2D` by its constructor params (`"{size}_{radius}_{thickness}"`). Single-field `Sprite? _x` returns first caller's asset for all callers.
13. **Field-Accessor Not Patchable**: `BaseItemDefinition.get_DefaultStackLimit` is a field accessor → Il2CppInterop `can't be patched`. Don't Harmony-patch it — engine uses direct scan/field write instead. Check `ilspycmd` before patching.
14. **Knowledge Base Discovery** (Curated in-repo sources):
    - `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/`: Vanilla symbols & exact method signatures — **mandatory before every patch**.
    - `Skills/schedule1-game-systems/references/`: All 64 system analyses (01–64 + `_index.md`) with core classes, events, hook points.
    - `ThirdParty/S1API/S1API/`: S1API full C# source & wrapper methods — **always check before accessing vanilla directly** (prefer the wrapper).
    - `Source/Mods/`: Live mod implementations (NotesApp, BankApp, PotScanner, HomelessMod, etc.).
    - Live game verification: `$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll` via `ilspycmd`.
15. **Multiplayer Host Authority**: Always guard passive income, financial balance updates, and world object placement with `NetworkGuard.IsHostOrSingleplayer()` to prevent duplicate client execution and desyncs in co-op.
16. **Idempotency**: Use checks like `LastPaidElapsedDay` to ensure scene reloads or save reloads don't trigger duplicate transactions.
17. **Hot-Path Harmony Early-Out via Cached Set**: When a Harmony prefix/postfix fires every physics step for every instance in the scene, cache target IDs into a `static readonly HashSet<int> _tunedInstanceIds` in `Awake`/`Start`, and early-out with `if (instanceId == 0 || !_tunedInstanceIds.Contains(instanceId)) return true;`.
18. **Save-Slot-Change vs Same-Slot-Scene-Reload Detection**: `GameLifecycle.OnPreLoad` fires for both real save-slot switches AND same-slot Menu→Game scene reloads. Distinguish slot switches (`oldSlot != newSlot`) from scene reload to avoid destroying placed objects on menu return.

### Never do (hard guardrails):
* Never use `foreach` or LINQ on `Il2CppSystem.Collections.Generic.List<T>` — indexed `for` only.
* Never call `FindObjectOfType` / `GetComponent` inside `OnUpdate` or tick loops — cache in `Awake`/`Start`/scene-load handlers.
* Never call `Resources.FindObjectsOfTypeAll<T>()` inside `OnUpdate`, diagnostic counters, or any per-frame path — freezes the game at 1 FPS. Scope to active scene via `SceneManager.GetActiveScene().GetRootGameObjects()` with bounded tree traversal.
* Never archive `Game/Mods/Shared.dll` together with the mods that depend on it. `Shared` is a shared library (`S1Mods.Shared`), not a mod.
* Never access cached IL2CPP objects after scene unload without a null/`Pointer`/`WasCollected` check.
* Never write save/config files with plain `File.WriteAllText` — always `SafeStorage.SaveAtomic`.
* Never let a Harmony patch throw — wrap risky patches with `PatchGuard` so a game update degrades gracefully instead of crashing.
* Never run `bump-version.ps1` without first doing the Feature-Diff Gate (§2.C).
* Never leave the auto-prepend `- Version bump.` placeholder in a CHANGELOG. Either write real release notes before the script runs, or clean up afterward.

---

## 4. Troubleshooting

Symptom → likely cause → fix. Log file is always `<GameDir>\MelonLoader\Latest.log`.

### Mod does not load at all
* **DLL missing in `<GameDir>\Mods\`** → build ran but deploy failed → check `Directory.Build.targets` output path and `$env:SCHEDULE1_PATH`; rebuild with `pwsh Tools/build-all.ps1`.
* **`TypeLoadException` / `MissingMethodException` on startup** → compiled against stale interop assemblies after a game update → run the Update Runbook (§5), rebuild all mods.

### Harmony patch not firing
* **Target method not found warning in log** → game update renamed/changed the signature → look up current symbol in `GameReferences/decompiled/Assembly-CSharp/` and update patch; verify against live DLL with `ilspycmd`.
* **Patch applies but never triggers** → wrong method overload or the patched code path isn't the one the game actually uses → verify against the decompile; add a temporary `ModLogger` line in the prefix to confirm.

### Crash / black screen on startup
* **Hard crash with no managed exception** → unguarded patch throwing inside IL2CPP code, or accessing a destroyed object → wrap in `PatchGuard`, add null/`Pointer`/`WasCollected` checks, check scene-transition handling.
* **Crash only after returning to Main Menu** → stale cached gameplay objects → clear caches in `OnSceneWasUnloaded`.

### Performance / stutter
* **Periodic GC spikes** → allocations in polling loops (LINQ, `foreach` on IL2CPP lists, boxing) → apply 0-allocation rules from the IL2CPP guide.
* **UI looks broken on some resolutions** → raw pixel values instead of `UITheme.Dp/Sp` → migrate to the scaling engine (UI guide §2).

### Input conflicts
* **Character walks while typing in a text field** → missing InputFocus hook → implement per Architecture guide §2.

### Build / namespace errors
* **`PersistentSingleton<LoadManager>` not found** → `PersistentSingleton<T>` lives in `Il2CppScheduleOne.DevUtilities`, but `LoadManager` itself lives in `Il2CppScheduleOne.Persistence`. Both `using`s are needed: `using Il2CppScheduleOne.DevUtilities; using Il2CppScheduleOne.Persistence;`.
* **`MelonLogger` has no `.Debug()`** → use `.Msg(...)` for chatty logs, `.Warning(...)` for fixable issues, `.Error(...)` for blockers.
* **`OnPreLoad` fires for both slot switch AND Menu→Game scene reload** → see Key Rule §3.18.

---

## 5. Update Runbook (after a game patch)

1. Note the new game version; compare with the version in this skill's header.
2. Refresh decompiles in `GameReferences/decompiled/Assembly-CSharp/`.
3. Rebuild the full solution — collect all `TypeLoadException`-relevant compile errors.
4. Grep `Latest.log` after first launch for `PatchGuard` warnings; fix affected patches against the new decompile.
5. Run the Definition-of-Done test pass for every affected mod.
6. Bump version references in this skill, `AGENTS.md`, and affected mods' `CHANGELOG.md`.

---

## 6. Maintenance Notes

* This skill is a living document: when a recurring mistake or a new convention emerges, add it here (guardrail, checklist item, or troubleshooting row) instead of relying on memory.
* Keep `SKILL.md` under ~500 lines; move growing detail into the matching file under `references/` and link it with a read-trigger.
* Skills live in `Skills/<skill-name>/SKILL.md` with `references/` subdirectories.

### Related skills (load via the `skill` tool):
  * `schedule1-phoneapp` — for PhoneApp-focused work (UI, lifecycle, InputFocus)
  * `schedule1-s1api` — for S1API-specific API questions (Saveables, Quests, NPCs, Items, ...)
  * `schedule1-s1mapi` — for procedural geometry, buildings, GLTF, world tools
  * `schedule1-knowledge` — for research and KB navigation
  * `schedule1-troubleshooting` — for crash diagnostics and IL2CPP pitfalls
  * `schedule1-persistence` — for atomic SafeStorage, slot isolation, and serialization
  * `schedule1-economy` — for money transactions, banking, and business revenue
  * `schedule1-items` — for registry scanning, item definitions, and stack limits
