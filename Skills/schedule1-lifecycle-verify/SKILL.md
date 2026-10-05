---
name: schedule1-lifecycle-verify
description: >-
  Verification runbook for Schedule I lifecycle hooks (Game v0.4.7f9, S1API 3.2.1-beta.8).
  Use whenever a lifecycle claim is marked [UNVERIFIED], before patching lifecycle code,
  after a game update shifts event order, or when deciding between OnSaveInfoLoaded /
  OnLoadComplete / OnGameplaySceneLoaded. Decision tree + ilspycmd command list.
  Code examples are backfilled by the GLM live-verification task.
  Keywords: lifecycle, verify, ilspycmd, GameLifecycle, OnPreLoad, OnLoadComplete, OnSaveInfoLoaded, event order, UNVERIFIED, verification log, DOTNET_ROOT, LoadManager, SaveManager.
---

> Version anchor: Game v0.4.7f9 / S1API 3.2.1-beta.8 (deployed 2026-10-05; in-repo ThirdParty/S1API source = beta.8 tag) / MelonLoader 0.7.3 / ilspycmd 9.1.0.7988 (versions verified 2026-10-05 against live install; ilspycmd 9.1.0.7988 re-installed 2026-10-05 - the global tools dir had gone missing; start it with the scoped DOTNET_ROOT from section 2; content NOT re-verified after the 0.4.7f9 update except the lifecycle type surface, see section 7). Re-check after any game or S1API update.

> **Live Verification Rule:** Always verify against the live DLLs under `$env:SCHEDULE1_PATH\` or in-repo decompiles under `GameReferences/decompiled/Assembly-CSharp/`.

# Schedule I — Lifecycle Verification with ilspycmd

This skill turns "[UNVERIFIED] lifecycle-order" findings into verified facts. It does NOT contain gameplay code examples (those live in `schedule1-s1api` / `schedule1-troubleshooting`); it contains the decision tree, the exact commands, and the interpretation rules. Source of the finding: Skill-Audit 2026-09-01, Finding 3 — lifecycle event order in `../schedule1-troubleshooting/references/save-load-timing.md:27`. Status 2026-09-28: **VERIFIED-PARTIAL** (see section 7) — the marker in `save-load-timing.md` now names exactly what is proven and what stays open.

---

## 1. When to Use

- A skill file, comment, or mod claims an event order (e.g. "OnSaveInfoLoaded fires before OnLoadComplete") and carries an `[UNVERIFIED]` marker or has no verification date.
- You are about to write or patch a Harmony patch or event subscription that depends on WHEN static lists populate.
- A game update or S1API update landed — anchor dates are older than the update.
- You must pick between competing hooks and cannot test in-game right now.

**Don't use for:** general decompile browsing (→ `schedule1-knowledge/references/search-recipes.md`), crash diagnostics (→ `schedule1-troubleshooting`), or save-file structure (→ `schedule1-persistence`).

## 2. Prerequisites

- `ilspycmd` on PATH (dotnet tool). **Status 2026-09-28: `ilspycmd` 9.1.0.7988 installed** (`%USERPROFILE%\.dotnet\tools`). **Every invocation needs the user-local .NET 8 runtime** - PowerShell: `$env:DOTNET_ROOT="$env:USERPROFILE\.dotnet"` first, bash: `DOTNET_ROOT="$USERPROFILE/.dotnet" ilspycmd ...` (why: Pitfall 6). If missing: `dotnet tool install --global ilspycmd --version 9.1.0.7988` (a bare `install -g ilspycmd` fails on SDK 8 - see Pitfall 6). Resolve the binary explicitly: `(Get-Command ilspycmd -ErrorAction Stop).Source`.
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
       ../schedule1-troubleshooting/references/save-load-timing.md:27,
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
6. **ilspycmd install & runtime traps (verified 2026-09-28).** (a) `dotnet tool install -g ilspycmd` pulls the 10.x/11.x line and fails on SDK 8.0.425 with `The settings file in the tool's NuGet package is invalid: DotnetToolSettings.xml was not found` - pin `--version 9.1.0.7988` (last line that both installs and runs on net8). (b) A bare `ilspycmd` then fails with `You must install or update .NET`: the apphost probes `C:\Program Files\dotnet` (only 6.0.36, kept there for MelonLoader) and misses the user-local 8.0.31 runtime - prefix every call with `$env:DOTNET_ROOT="$env:USERPROFILE\.dotnet"` (bash: `DOTNET_ROOT="$USERPROFILE/.dotnet" ...`). Do **NOT** set `DOTNET_ROOT` user-level as a shortcut: MelonLoader resolves its hostfxr from `C:\Program Files\dotnet\host\fxr\6.0.36` and must keep seeing net6 (precaution, not re-verified).
7. **Command C cannot show event-RAISING order for IL2CPP (verified 2026-09-28).** The `Il2CppAssemblies` proxies are thunk-only - every method body is just `il2cpp_runtime_invoke(...)`, and the code that invokes `onSaveInfoLoaded.Invoke()` and friends lives natively in `GameAssembly.dll`, invisible to ilspycmd on the proxy. Use Command C / `-l c` for EXISTENCE and signatures only. For ORDER use runtime evidence (the `save-load-timing.md` section 10 logging pattern) or lift `GameAssembly.dll` (Cpp2IL / Il2CppInspector) as a last resort.

## 7. Verification Log

| Date | Claim | Result | Evidence |
|---|---|---|---|
| 2026-09-02 | Wrapper bridges native events 1:1 | **VERIFIED (partial):** `S1API.Lifecycle.GameLifecycle.Initialize()` wires `OnPreLoad/OnLoadComplete/OnPreSceneChange/OnSaveInfoLoaded` to `LoadManager.onPreLoad/onLoadComplete/onPreSceneChange/onSaveInfoLoaded` and `OnSaveStart/OnSaveComplete` to `SaveManager.onSaveStart/onSaveComplete`. Consequence: the ORDER question lives in the NATIVE `LoadManager`, not in the wrapper. | Command A output, live `S1API.Il2Cpp.MelonLoader.dll` |
| 2026-09-28 | Toolchain + lifecycle type surface still valid after the 0.4.7f6 / S1API 3.2.1-beta.7 update | **VERIFIED:** `ilspycmd` 9.1.0.7988 (scoped `DOTNET_ROOT`) decompiles `S1API.Lifecycle.GameLifecycle` from the deployed DLL (still holds `LoadManager`/`SaveManager` fields) and `Il2CppScheduleOne.Persistence.LoadManager` + `SaveManager` both list in the live `Assembly-CSharp.dll`. Event-ORDER question stays PENDING. | Commands A + D against live install, 2026-09-28 |
| 2026-09-28 | Order claim: OnSaveInfoLoaded after save parse / before scene build, OnLoadComplete after scene build (`save-load-timing.md:22-27`) | **VERIFIED-PARTIAL.** (a) Wrapper wiring re-confirmed: `GameLifecycle.Initialize` attaches `InvokeOnPreLoad/InvokeOnLoadComplete/InvokeOnPreSceneChange/InvokeOnSaveInfoLoaded` to `LoadManager.onPreLoad/onLoadComplete/onPreSceneChange/onSaveInfoLoaded` (S1API 3.2.1-beta.7 source + live DLL). (b) S1API author docs (`ThirdParty/S1API/S1API/Lifecycle/GameLifecycle.cs`): OnPreLoad = 'before the game begins loading saved data'; OnLoadComplete = 'after all save data has been loaded and the loading screen is about to close'; OnSaveInfoLoaded = 'when save game information has been loaded and refreshed … when the save menu is opened or when save data is re-scanned' -> MULTI-FIRE, also outside a load. (c) Runtime (Latest.log, real load 2026-09-28 19:52): scene 'Main' active 34.249 -> OnPreLoad 34.638 -> OnLoadComplete 39.469: OnPreLoad < OnLoadComplete PROVEN; the scene-active marker BEFORE OnPreLoad contradicts the old phase diagram in `save-load-timing.md` (that diagram keeps its marker). (d) Exact OnSaveInfoLoaded position NOT statically provable (Pitfall 7) and not logged in that run. | Commands A + C + S1API source + Latest.log 2026-09-28 |
| 2026-09-29 | Final runtime check: does OnSaveInfoLoaded fire at all, and in what order? (instrumented run) | **VERIFIED — NEGATIVE for OnSaveInfoLoaded.** Four explicit `[VERIFY]` markers in PotScanner's handlers, real session 05:58-06:02 (boot -> save menu -> Continue -> gameplay -> quit). Observed order: `OnSceneWasLoaded 'Menu'` 05:58:33 -> `OnSceneWasLoaded 'Main'` 05:59:02.534 -> `OnPreLoad` 05:59:02.957 -> `OnLoadComplete` 05:59:10.822. **`OnSaveInfoLoaded` fired 0 times** — not at save-menu open, not during the load — although two mods (PotScanner, CustomSkateboard) were subscribed with first-line logging. CONSEQUENCE: the 'refresh on OnSaveInfoLoaded' pattern is DEAD on game 0.4.7f6 + S1API 3.2.1-beta.7; use `OnPreLoad` / `OnSceneWasLoaded` / `OnLoadComplete`. Finding 3's claim: OnLoadComplete-after-scene-build CONFIRMED; 'OnSaveInfoLoaded after parse / before scene build' OBSERVED FALSE (the event never fires). Instrumentation removed after the run (PotScanner rebuilt clean). | PotScanner `[VERIFY]` markers + Latest.log 2026-09-29 05:58 |
| 2026-10-05 | Event set of S1API 3.2.1-beta.8 (does `OnSaveLoaded` exist?) | **VERIFIED — NEGATIVE for OnSaveLoaded.** `GameLifecycle` source (`ThirdParty/S1API/S1API/Lifecycle/GameLifecycle.cs`) exposes EXACTLY 6 events: `OnPreLoad` (:50), `OnLoadComplete` (:69), `OnPreSceneChange` (:88), `OnSaveInfoLoaded` (:107), `OnSaveStart` (:126), `OnSaveComplete` (:145). No `OnSaveLoaded` anywhere in the S1API tree (rg over `ThirdParty/S1API/S1API`). CONSEQUENCE: the `OnSaveLoaded` recommendations in s1api SKILL §5/§7 + `lifecycle.md` were dead code — corrected 2026-10-05 to `OnPreLoad`/`OnLoadComplete`. | rg + GameLifecycle.cs line refs, 2026-10-05 |
| 2026-10-05 | Toolchain validity after the 0.4.7f9 update (buildid 25698382, appmanifest written 2026-10-05) | **VERIFIED:** Latest.log game-version line `0.4.7f9` + MelonLoader `v0.7.3 Open-Beta` (session 2026-10-05 18:51); `S1API.Il2Cpp.MelonLoader.dll` product `3.2.1-beta.8` (deployed 2026-10-05 08:51 — one beta line above the old anchor beta.7); `S1MAPI_Il2cpp.dll` `2.0.1.0` (deployed 2026-10-02). ilspycmd had gone missing (global dotnet tools dir gone, `dotnet tool list` empty) — re-installed `9.1.0.7988` 2026-10-05, runs with scoped `DOTNET_ROOT`. `Il2CppAssemblies/Assembly-CSharp.dll` regenerated 2026-10-03 (0.4.7f9-era, 13.5 MB, thunk-only — Pitfall 7 unchanged). In-repo `ThirdParty/S1API` submodule still beta.7 (db1de39) ≠ deployed beta.8 (f65ae40) — submodule update pending. | Latest.log + VersionInfo probes + `dotnet tool install` output, 2026-10-05 |

Backfill rule: SATISFIED 2026-09-29 — the 2026-09-29 row above carries date + evidence, and the `[UNVERIFIED]` marker in `save-load-timing.md` was replaced by the verified marker. New backfills follow the same pattern (row here first, then un-mark the claim).

## 8. Open Items

- Code examples (a worked end-to-end verification transcript) are backfilled by GLM after the live check against `Assembly-CSharp.dll` — do not write them here by hand.
