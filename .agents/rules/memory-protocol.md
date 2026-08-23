---
trigger: always_on
description: "Proactive Memory & Knowledge Update Protocol for Schedule 1 Modding"
---

# Memory & Knowledge Protocol — Schedule 1 Modding

This rule governs the proactive knowledge lifecycle and cross-session memory management for all AI agents operating within the `Schedule 1 Modding` workspace.

---

## 1. Pre-Task Phase (Knowledge Retrieval & Guardrail Check)

Before formulating plans, designing features, or writing code:
1. **Consult `MEMORY.md`**: Read the Core Architecture Standards (§2) and the Battle-Tested Gotchas Matrix (§3) — now 2026-08-21 with snapshot-revert, slot-leak, UITheme-duplication, field-accessor, and SafeStorage-atomic entries.
2. **Load Matching Skills**: Activate relevant skills in `.agents/skills/` — core roster is now **11 skills**:
   - Always: `schedule1-modding` (runbook) + `schedule1-knowledge` (KB nav) + `schedule1-troubleshooting` (if bug/crash)
   - Domain: `schedule1-phoneapp` (PhoneApps — Rule 10/11 + UITheme delegation), `schedule1-grid` (outdoor building 7 Golden Rules), `schedule1-s1api` ( Saveables/Quests/NPCs/PhoneApp), `schedule1-s1mapi` (ProceduralMesh/Building/GLTF), `schedule1-game-systems` (64 systems, decision tree)
   - **New since 2026-08-21:** `schedule1-economy` (Money/Business/Shop — atomic purchase, double-entry, host authority, snapshot), `schedule1-persistence` (SafeStorage atomic + slot_{n} + GameLifecycle timing + TOML sidecar), `schedule1-items` (Registry dual-scan, StackLimit, inventory CashSlot)
   - Load **by task**, not all — see each `SKILL.md` description for "When to use".
3. **Verify Constraints**: Identify any known game version quirks (v0.4.6f13 / Unity 2022.3 / IL2CPP 2022.3.62f2), lifecycle timing constraints (`OnCreated` once per scene, static lists empty at `OnGameplaySceneLoaded`), or IL2CPP pointer hazards (`Pointer != IntPtr.Zero`, field accessor not patchable).

---

## 2. Execution Phase (Pillars & Maker-Checker Standard)

During code modification and feature development:
1. **Strict Architectural Pillars** (5 pillars in `MEMORY.md:§2`, enforced 2026-08-21):
   - Validate IL2CPP pointers (`Pointer != IntPtr.Zero`) + `IntPtr` ctor for `[RegisterTypeInIl2Cpp]` + `for` not `foreach` on `Il2CppSystem.Collections.Generic.List<T>`.
   - Method 3 via `S1Mods.Shared.UITheme` only — `InitializeForTextApp(750f,0.85-2.0)` / `InitializeForDashboard(900f,0.75-1.20)` delegated from mod wrappers; never local duplicate (see `schedule1-modding:§3 Rule 5` + `schedule1-phoneapp:Rule 11`).
   - `SafeStorage.SaveAtomic/SaveTextAtomic` + `slot_{n}.json` via `LoadManager.ActiveSaveInfo` + `TryMigrateLegacy` + serialize only on `GameLifecycle.OnSaveComplete` (see `schedule1-persistence`).
   - `PatchGuard.TryPatch` for Harmony; field accessors (`BaseItemDefinition.get_DefaultStackLimit`) not patchable → scan + instance postfix (`schedule1-items`); dictionary-key Sprite caches (`"{size}_{radius}"`).
   - Multiplayer host authority (`IncomeEngine.IsHostOrSingleplayer()`) + idempotent `LastPaidElapsedDay` + snapshot revert (not `-1`) (`schedule1-economy`).
   - **7 Golden Rules of Outdoor Building** when touching grid/world placement (`schedule1-grid`).
2. **Maker-Checker Workflow**:
   - Utilize a separated `coder` and `verifier` subagent workflow where all modifications are independently audited against compilation, IL2CPP memory safety, and log outputs. Verifier checks slot-isolation, UITheme delegation, PatchGuard report, and `Latest.log` warnings.

---

## 3. Post-Task / Proactive Learning Phase (Continuous Memory Sync)

Whenever a task resolves a non-trivial bug, reverse-engineers a game system, or establishes a new architectural pattern:
1. **Update `MEMORY.md`**:
   - Document new gotchas, failure modes, and permanent solutions in the Gotchas Matrix (§3) — e.g. 2026-08-21 additions: global leak, UITheme duplication, raw file-write, snapshot-loss, field-accessor, circle-cache.
   - Update the Active Mod Matrix (§4) and append a milestone entry to the Decision Log (§5) with verifier result (`dotnet build` + `Shared.Tests` 12/12 + `Latest.log` PatchGuard).
   - Bump `Last Synchronized` to current date and Knowledge snapshot (4804 files / 67 MB).
2. **Update Associated Skills** (now 11 — update the **matching** domain skill, not just generic):
   - Phone app / UI / lifecycle / slot-leak → `schedule1-phoneapp` (Rule 10/11) + `schedule1-modding` `ui-and-s1api.md` + `schedule1-troubleshooting` §7/§8
   - Building / grid / placement → `schedule1-grid` (7 Golden Rules)
   - Economy (Money/Business/Shop) → `schedule1-economy` (`atomic-purchase.md`, `atm-double-entry.md`, `passive-revenue.md`)
   - Persistence (SafeStorage/slot/timing/TOML) → `schedule1-persistence` (`safestorage-atomic.md`, `slot-isolation.md`, `save-load-timing.md`)
   - Items / Registry / StackLimit / Inventory → `schedule1-items` (`registry-and-scan.md`, `stacklimit-engine.md`, `inventory-capacity.md`)
   - Grid only if placement-specific; S1API/S1MAPI for framework reference bumps (version 2026-08-21)
   - Knowledge nav → `schedule1-knowledge` §6 counts + eigenmods gap; Game systems → `schedule1-game-systems` decision tree
3. **Sync `AGENTS.md`**:
   - Update the Mod Inventory table and mod details if versions, dependencies, or statuses changed.
   - Ensure `AGENTS.md` and `MEMORY.md` remain perfectly synchronized.
   - Update `AGENTS.md:§0` skill table to list all 11 skills (economy/persistence/items + game-systems).
