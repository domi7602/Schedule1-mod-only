---
name: schedule1-lifecycle-verify
description: >-
  Verification runbook for Schedule I lifecycle hooks (Game v0.4.6f13, S1API 3.2.0).
  Use whenever a lifecycle claim is marked [UNVERIFIED], before patching lifecycle code,
  after a game update shifts event order, or when deciding between OnSaveInfoLoaded /
  OnLoadComplete / OnGameplaySceneLoaded. Decision tree + ilspycmd command list.
  Code examples are backfilled by the GLM live-verification task.
---

> Version anchor: Game v0.4.6f13 / S1API 3.2.0 / MelonLoader 0.7.3 / ilspycmd 11.0.0.9375 (verified 2026-09-02). Re-check after any game or S1API update.

> **Knowledge guard (mod-only):** Always verify against the live DLLs under `$env:SCHEDULE1_PATH\` — never against memory or the legacy `D:\Backup\game source\Knowledge\` mirror (bundle 0.4.5f2, one version behind).

# Schedule I — Lifecycle Verification with ilspycmd

This skill turns "[UNVERIFIED] lifecycle-order" findings into verified facts. It does NOT contain gameplay code examples (those live in `schedule1-s1api` / `schedule1-troubleshooting`); it contains the decision tree, the exact commands, and the interpretation rules. Source of the current open finding: Skill-Audit 2026-09-01, Finding 3 — lifecycle event order in `schedule1-troubleshooting/references/save-load-timing.md:27` is marked `[UNVERIFIED]`.

---

## 1. When to Use

- A skill file, comment, or mod claims an event order (e.g. "OnSaveInfoLoaded fires before OnLoadComplete") and carries an `[UNVERIFIED]` marker or has no verification date.
- You are about to write or patch a Harmony patch or event subscription that depends on WHEN static lists populate.
- A game update or S1API update landed — anchor dates are older than the update.
- You must pick between competing hooks and cannot test in-game right now.

**Don't use for:** general decompile browsing (→ `schedule1-knowledge/references/search-recipes.md`), crash diagnostics (→ `schedule1-troubleshooting`), or save-file structure (→ `schedule1-persistence`).

## 2. Prerequisites

- `ilspycmd` on PATH (dotnet tool). If missing: `dotnet tool install -g ilspycmd`, then re-open the shell. Resolve the binary explicitly: `(Get-Command ilspycmd -ErrorAction Stop).Source`.
- `$env:SCHEDULE1_PATH` pointing to the game root. Fallback literal: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`.
- The two target assembly groups:
  - S1API wrapper: `$env:SCHEDULE1_PATH\Mods\S1API.Il2Cpp.MelonLoader.dll` (defines `S1API.Lifecycle.GameLifecycle` events)
  - Native proxies: `$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\` (136 files). **Most native game classes live in `Assembly-CSharp.dll`** (~41 MB) — e.g. `Il2CppScheduleOne.Persistence.LoadManager`, `Il2CppScheduleOne.Persistence.SaveManager` (verified 2026-09-02). `Il2CppScheduleOne.Core.dll` holds only a small subset.

## 3. Decision Tree

```
Lifecycle claim to verify
│
├─ Claim is about S1API hook names / event order?
│  └─ YES → Decompile S1API wrapper (Command A), then check
│           which native events it bridges (Command B)
│
├─ Claim is about when statics populate (Property.OwnedProperties etc.)?
│  ├─ Need the population CALL SITE → decompile the native owner class
│  │   (Command C), find where the list is filled relative to event invoke
│  └─ Only need ORDER of two events → Command B, read the invoking method body
│
├─ Type/namespace unknown?
│  └─ List candidates first (Command D), grep for the event name
│
├─ Result ambiguous or empty?
│  ├─ ilspycmd printed nothing → wrong full type name → Command D first
│  ├─ Method body is opaque IL → retry with -il (Command E)
│  └─ DLL older than current game build → assemblies are stale,
│      launch game once so MelonLoader regenerates Il2CppAssemblies, retry
│
└─ Verified?
   └─ YES → backfill: replace [UNVERIFIED] marker in
       schedule1-troubleshooting/references/save-load-timing.md:27,
       record date + evidence line in this skill's section 7
```

## 4. Command List (PowerShell)

All commands run in `terminal` with `pwsh`/PowerShell semantics. `$ils` resolves the real binary path (skills must not assume PATH order).

**A. Decompile the S1API lifecycle class** (hook names, event signatures):

```powershell
$ils = (Get-Command ilspycmd -ErrorAction Stop).Source
& $ils -t S1API.Lifecycle.GameLifecycle "$env:SCHEDULE1_PATH\Mods\S1API.Il2Cpp.MelonLoader.dll"
```

**B. Find WHO invokes an event and in WHAT order** — decompile the class, then read the raising method bodies. If the body decompiles unreadably, use IL (Command E).

**C. Decompile a native class** (e.g. the class that RAISES the lifecycle events):

```powershell
& $ils -t Il2CppScheduleOne.Persistence.LoadManager "$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll"
```

Adjust namespace/type by listing first (Command D). Targeted `-t` only — never dump the whole 41 MB Assembly-CSharp.

**D. List candidate types when the namespace is unknown:**

```powershell
& $ils -l c "$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll" | Select-String -Pattern "LoadManager|Lifecycle"
```

Verified 2026-09-02: `-l c` on Assembly-CSharp.dll finds `Il2CppScheduleOne.Persistence.LoadManager` and `Il2CppScheduleOne.Persistence.SaveManager`.

**E. IL fallback for opaque bodies:**

```powershell
& $ils -t S1API.Lifecycle.GameLifecycle -il "$env:SCHEDULE1_PATH\Mods\S1API.Il2Cpp.MelonLoader.dll"
```

**F. Structured output for scripting:** append `--json`.

## 5. Interpretation Rules

| Observation in decompiled output | Meaning |
|---|---|
| Event A's raising method is called from a method that event B's raising method also calls, earlier in the body | A fires before B — order claim VERIFIED for that path |
| Events are raised from independent call sites | Order is NOT statically provable from one class — mark VERIFIED-PARTIAL, note the call sites found, and verify at runtime with the logging pattern (save-load-timing.md §10) |
| Event name missing entirely from the class | Hook name is wrong — search the real name via Command D before patching |
| Member is a field accessor (`get_X` with no body) | Never Harmony-patch it — documented trap, see schedule1-items SKILL.md Trap 2026-08-21 |

## 6. Pitfalls

1. **Stale `Il2CppAssemblies`.** The proxy DLLs are generated by MelonLoader at game launch. After a game update they regenerate — a verification against old files proves nothing. Compare DLL timestamp against the game build date before trusting output.
2. **Wrapper vs. native namespace.** S1API events (`S1API.Lifecycle.GameLifecycle`) are wrappers around native events (`Il2CppScheduleOne.*`). Verify BOTH layers when the claim spans them; the wrapper can reorder or coalesce.
3. **Whole-assembly dumps.** `ilspycmd <dll>` without `-t` on Assembly-CSharp produces a 41 MB dump. Use `-t`; fall back to `-p -o <tempdir>` + `search_files` only for unknown-namespace hunts.
4. **PATH assumptions.** Always resolve via `(Get-Command ilspycmd -ErrorAction Stop).Source` — hotfix convention from the 2026-09-02 skill health check.
5. **`$env:SCHEDULE1_PATH` may be empty in a fresh shell.** It was set User-level on 2026-09-02 (`[Environment]::SetEnvironmentVariable(...,'User')`). If `$env:SCHEDULE1_PATH` prints nothing, set it inline for the session: `$env:SCHEDULE1_PATH = 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I'`.

## 7. Verification Log

| Date | Claim | Result | Evidence |
|---|---|---|---|
| 2026-09-02 | Wrapper bridges native events 1:1 | **VERIFIED (partial):** `S1API.Lifecycle.GameLifecycle.Initialize()` wires `OnPreLoad/OnLoadComplete/OnPreSceneChange/OnSaveInfoLoaded` to `LoadManager.onPreLoad/onLoadComplete/onPreSceneChange/onSaveInfoLoaded` and `OnSaveStart/OnSaveComplete` to `SaveManager.onSaveStart/onSaveComplete`. Consequence: the ORDER question lives in the NATIVE `LoadManager`, not in the wrapper. | Command A output, live `S1API.Il2Cpp.MelonLoader.dll` |
| — | OnSaveInfoLoaded fires after save parse, before scene build; OnLoadComplete after scene build (`save-load-timing.md:22-27`) | **PENDING** — assigned to GLM live-verification task (kanban t_40ac7ce3). Next step for GLM: decompile `Il2CppScheduleOne.Persistence.LoadManager` from `Assembly-CSharp.dll` (Command C) and read which UnityEvent fires first. | — |

Backfill rule: when the GLM task returns, move the row here with a real date, the command used, and the decisive decompiled line. Only then delete the `[UNVERIFIED]` marker in `save-load-timing.md`.

## 8. Open Items

- Code examples (a worked end-to-end verification transcript) are backfilled by GLM after the live check against `Assembly-CSharp.dll` — do not write them here by hand.
