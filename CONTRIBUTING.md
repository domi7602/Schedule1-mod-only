# Contributing Guide

Thanks for your interest in contributing to the Schedule I mods in this repository. This guide summarizes the workflow and the quality gates; the full build/test/debug reference is [DEVELOPERS.md](DEVELOPERS.md), the documentation index is [`docs/README.md`](docs/README.md).

## Requirements

- **Game:** Schedule I (IL2CPP, Unity 2022.3) — v0.4.7f11 current runtime; older verifications on v0.4.6f13 / v0.4.7f6 / v0.4.7f9 (see [`docs/compatibility.md`](docs/compatibility.md)); installed under `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` (override via `$env:SCHEDULE1_PATH`)
- **SDK:** .NET 6 SDK, PowerShell 7+
- **Loader:** MelonLoader 0.7.3
- **S1API:** 3.2.x (deployed as `Mods/S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.dll`; source pinned as submodule `ThirdParty/S1API/`)
- **S1MAPI:** 2.0.1 (`UserLibs/S1MAPI_Il2Cpp.dll`; submodule `ThirdParty/S1MAPI/`) — needed by AutoPackagingStation and TaxiDriver

## Workspace Layout

```
Source/Mods/          Active mods + Shared lib + S1Mods.sln          → Source/Mods/README.md
Source/Archive/       Archived mods (not in SLN, not built)
Source/Tests/         xUnit tests (Shared.Tests, AutoPackagingStation.Tests, CalculatorApp.Tests)
GameReferences/       Locally generated decompiles (gitignored)      → GameReferences/README.md
Skills/               AI-agent skills                                → Skills/README.md
ThirdParty/           Pinned dependencies & archives                 → ThirdParty/README.md
Tools/                PowerShell helper scripts                      → Tools/README.md
Release/              Release ZIPs (gitignored, .gitkeep only)
docs/                 architecture, pitfalls, compatibility, release-process, troubleshooting
AGENTS.md             Agent bootstrap + authoritative mod inventory
DEVELOPERS.md         Developer guide (build, test, debug)
```

## Loading Skills

Always load the relevant skill first via the `skill` tool:

- `schedule1-modding` — for every mod task (mandatory)
- `schedule1-phoneapp` — for PhoneApps
- `schedule1-s1api` / `schedule1-s1mapi` — for framework APIs
- `schedule1-knowledge` — for research (`GameReferences/`, `ThirdParty/S1API/`, `Skills/schedule1-game-systems/references/`)
- `schedule1-troubleshooting` — for crashes/logs

Skill paths: `Skills/<skill-name>/SKILL.md` (+ `references/` subfiles). Full index: `Skills/README.md`.

## Dependencies and References

```pwsh
git submodule update --init --recursive
pwsh Tools/bootstrap-game-references.ps1
```

The decompiles are local research output and must not be committed. The rules for dependency direction and mod structure are in [`docs/architecture.md`](docs/architecture.md).

## Creating a New Mod

```pwsh
pwsh Tools/new-mod.ps1 -Name "MyNewMod" -Author "Dominik" -Version "0.1.0"
pwsh Tools/gen-sln.ps1
dotnet build Source/Mods/MyNewMod/src/MyNewMod.csproj -c Release
```

`Directory.Build.props/targets` automatically deploys DLLs/PNGs to `<GameDir>\Mods\`; `mod.json` + `.pdb` to `<GameDir>\UserData\<ModName>\` (since 2026-09 — json/pdb never belong in `Mods\`).

See `Skills/schedule1-modding/references/architecture-and-shared.md` for mandatory patterns (SafeStorage, UITheme, PatchGuard, InputFocus).

## Build & Test

```pwsh
# All mods
pwsh Tools/build-all.ps1
dotnet build Source/Mods/S1Mods.sln -c Release

# Single mod
dotnet build Source/Mods/NotesApp/src/NotesApp.csproj -c Release

# All tests (Shared.Tests needs the game assemblies)
dotnet test Source/Mods/S1Mods.sln -c Release

# Single test suites
dotnet test Source/Tests/Shared.Tests/Shared.Tests.csproj -c Release               # requires game assemblies
dotnet test Source/Tests/AutoPackagingStation.Tests/AutoPackagingStation.Tests.csproj -c Release # pure math logic
dotnet test Source/Tests/CalculatorApp.Tests/CalculatorApp.Tests.csproj -c Release # pure decimal logic

# Format check (CI)
dotnet format Source/Mods/S1Mods.sln --verify-no-changes

# Version drift check (CI + pre-commit): code <-> mod.json <-> README/AGENTS (exit 1 on drift)
pwsh Tools/check-version-sync.ps1

# Doc paths check (CI + pre-commit): referenced repo paths must exist
pwsh Tools/check-doc-paths.ps1
```

## Version Bump (Single Source of Truth = Code)

Never manually — use the bump helper:

```pwsh
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.3
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.3 -DryRun

# Verify (exit 1 on drift):
pwsh Tools/check-version-sync.ps1
```

Updates 5 places: MelonInfo in code (file with `[assembly: MelonInfo(...)]`, otherwise `Constants.ModVersion`) + `Source/Mods/<Mod>/docs/mod.json` + `Source/Mods/<Mod>/docs/CHANGELOG.md` (`## x.y.z` header) + `AGENTS.md` (matrix row **and** detail header in §2) + `README.md` (mod table row and featured card). After an in-game verification also update the matrix in `docs/compatibility.md`.

`Tools/check-version-sync.ps1` checks exactly these locations against the code; it runs in CI and in the pre-commit hook.

## IL2CPP Obligations

- Every `[RegisterTypeInIl2Cpp]` MonoBehaviour needs `public Foo(IntPtr ptr) : base(ptr) { }`
- No `foreach`/LINQ on `Il2CppSystem.Collections.Generic.List<T>` — use `for` only
- No `button.onClick.AddListener(new UnityAction(...))` — use `S1API.Utils.EventHelper.AddListener` / `ButtonUtils.AddListener`
- Optional, if installed: `s1interop analyze <csproj>` (reports e.g. missing `IntPtr` constructors)
  - Installation: `dotnet tool install --global S1Interop --version 0.1.0-alpha.1` — external tool (`ifBars/S1Interop`, GPL-3.0), no repo relation, requires .NET SDK 8+
  - Pre-flight check of game references: `s1interop doctor <csproj>` (read-only; in the repo the Mono branch reports `[missing]` — expected, we build IL2CPP)
  - **`analyze` is a report, not a gate:** The tool always returns **exit 0**, even with findings. `0 Errors` in the exit code means nothing — the output must be read.
  - **Known false positives (as of 0.1.0-alpha.1):** `wrong_target_framework` + `global_usings_require_langversion` for all projects, because TFM (`net6.0`) and `LangVersion` come from `Directory.Build.props` and the alpha tool only reads the `.csproj`. Also expected: `ManagedCollectionSignatureInterop` in BusinessIncome (mod-internal calculation API, no game callback) and the reflection findings in HitmanPhone (`S1Quest` is internal in S1API, defensively secured with fallback).
  - Without installation, build + tests are the mandatory substitute. **Local only** — the s1interop CI job was removed on 2026-09-20 (never runnable on GitHub-hosted runners, analyze permanently skipped).

## Definition of Done

- [ ] Build 0 errors / 0 warnings (`-c Release`)
- [ ] DLL deployed and in `MelonLoader/Latest.log` without exception
- [ ] In-game verified (incl. scene change Main Menu → Game → Main Menu)
- [ ] Persistence round-trip OK (Save → Restart → Load)
- [ ] `s1interop analyze` report read, no **unexpected** entries (if installed; exit code is always 0 — see "IL2CPP Obligations")
- [ ] `AGENTS.md`, `CHANGELOG.md`, `mod.json`, `Mod.cs`/`Constants.ModVersion`, `README.md` synchronized (via `bump-version.ps1`) — `pwsh Tools/check-version-sync.ps1` must be green
- [ ] `pwsh Tools/check-doc-paths.ps1` green (no dead path references in the docs)

## Commit & PR

- Commits: `feat(mod): ...`, `fix(mod): ...`, `chore(tools): ...` (see `git log`)
- Solution is deterministic (`gen-sln.ps1` uses MD5 GUIDs) — no unnecessary GUID diffs
- `dotnet format` before every push
- Fill out PR template (see `.github/pull_request_template.md`)

## Documentation changes

- Player-facing text belongs in `README.md` or the mod's `docs/README.md`; developer details in `DEVELOPERS.md` / `docs/`; agent rules in `AGENTS.md`. Do not duplicate content across these files — link instead.
- `pwsh Tools/check-doc-paths.ps1` must stay green: every repository path mentioned in Markdown has to exist.
- Public naming: *Schedule I* (not "Schedule 1"), mod names exactly as their folder/assembly names.

## Questions?

See [`DEVELOPERS.md`](DEVELOPERS.md), `AGENTS.md §6` (workflows), [`docs/pitfalls.md`](docs/pitfalls.md) (gotchas & IL2CPP traps), [`Skills/README.md`](Skills/README.md) (skill index) or open an issue.
