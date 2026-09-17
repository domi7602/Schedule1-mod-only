# Schedule I — Modding Workspace (Developers Guide)

*Note: This is the technical documentation for building and contributing to the mods in this repository. For the player-facing mod list and installation guide, please see [README.md](README.md).*

MelonLoader modding workspace for *Schedule I* v0.4.6f13 (TVGS). Fully built on IL2CPP + S1API 3.2.0 + MelonLoader 0.7.3.

> **Single Source of Truth:** [`AGENTS.md`](AGENTS.md) §2 ist die verbindliche Mod-Inventar-Liste (Status, Versionen, Pfade). Dieser Guide dupliziert sie bewusst **nicht** mehr (frühere Kopien drifteten — Repo-Audit 2026-09-16). Der Skill-Index lebt ausschließlich in [`Skills/README.md`](Skills/README.md).

## What This Is

- MelonLoader / IL2CPP mods (TFM `net6.0`) targeting Schedule I **v0.4.6f13**
- Mod source plus locally generated decompiled assemblies under `GameReferences/` (Assembly-CSharp, firstpass)
- AI agent skills under `Skills/` (index: `Skills/README.md`)

## Layout

```
Source/Mods/                  Mods + shared lib + Directory.Build.props/targets
Source/Mods/S1Mods.sln        Solution (regeneratable via Tools/gen-sln.ps1)
Source/Archive/               Archived mods (DayCounter, ProfitTracker, TVBrowser, BackpackMod) — not in SLN
Source/Tests/                 xUnit tests (Shared.Tests, AutoPackagingStation.Tests, CalculatorApp.Tests)
GameReferences/               Locally generated decompiles; see GameReferences/README.md
Skills/                       20 AI-agent skills (`Skills/<name>/SKILL.md` + `references/`; index: Skills/README.md)
ThirdParty/                   Pinned external dependencies & archives; see ThirdParty/README.md
Tools/                        11 helper scripts (build-all, gen-sln, new-mod, bump-version, check-version-sync, check-doc-paths, package-release, deploy-thirdparty, backup-to-d, bootstrap-game-references, mods-cleanup-inventory)
Release/                      Release packages
.github/                      CI (workflows/ci.yml, workflows/release.yml) + issue/PR templates
docs/                         Architecture, release process, and pitfalls (docs/pitfalls.md)
AGENTS.md                     Workspace conventions & mod inventory (single source of truth)
README.md                     Player-facing mod overview
DEVELOPERS.md                 This file
```

## Build & Deploy

```pwsh
# Build the full solution
pwsh Tools/build-all.ps1

# Or directly via dotnet
dotnet build Source/Mods/S1Mods.sln -c Release

# Build WITHOUT touching the game install (CI, pure compile checks):
$env:S1NoDeploy = "true"        # or: dotnet build ... -p:S1NoDeploy=true

# GameDir override:
$env:SCHEDULE1_PATH = "D:\path\to\Schedule I"

# Quality gates (identical in CI + pre-commit)
dotnet format Source/Mods/S1Mods.sln --verify-no-changes   # formatting
pwsh Tools/gen-sln.ps1                                     # SLN determinism
pwsh Tools/check-version-sync.ps1                          # code <-> mod.json <-> README/AGENTS
pwsh Tools/check-doc-paths.ps1                             # referenced repo paths must exist
dotnet test Source/Mods/S1Mods.sln -c Release              # all three test suites (game assemblies needed)
```

A successful build deploys automatically (via `Directory.Build.targets`), with split targets since the 2026-09 reinstall:

- `<GameDir>\Mods\` — DLLs, icons (PNG), bundles (only MelonLoader-loadable files)
- `<GameDir>\UserData\<ModName>\` — `mod.json` (metadata) + `<ModName>.pdb` (portable debug symbols, `<DebugType>portable</DebugType>`)

**Never place json/pdb files into `Mods\`.** Default GameDir: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` (override via `SCHEDULE1_PATH`). With `S1NoDeploy=true` no copy happens at all.

`SkipUnchangedFiles` is set to `false` (since 2026-08-20) — every `dotnet build` force-deploys, eliminating stale-DLL traps. Release packaging (`Tools/package-release.ps1`) excludes the `Shared` library and the `_DiagPerfCounter` dev tool; the Release workflow is manual-only (`workflow_dispatch`) because it needs a runner with Schedule I installed.

## Dependencies & References

Initialize the pinned API dependencies after cloning:

```pwsh
git submodule update --init --recursive
```

Generate local game decompiles only when researching game internals:

```pwsh
pwsh Tools/bootstrap-game-references.ps1
```

See [`docs/architecture.md`](docs/architecture.md), [`ThirdParty/README.md`](ThirdParty/README.md), and [`GameReferences/README.md`](GameReferences/README.md) for ownership and dependency rules.

## Active Mods

Siehe [`AGENTS.md`](AGENTS.md) §2 (Matrix + Mod-Details) — inklusive archivierter Mods unter `Source/Archive/` und entfernter Third-Party-Tools. Aktuell: **19 Projekte** in `S1Mods.sln` (15 Mods + `Shared` + 3 Testprojekte), archivierte Mods werden nicht gebaut.

`Tools/check-version-sync.ps1` erzwingt, dass die Code-Version mit `mod.json`, `README.md` und `AGENTS.md` synchron bleibt; `Tools/check-doc-paths.ps1` bewacht die referenzierten Pfade.

## Status

- [x] Phase 0 — Git baseline (empty workspace committed)
- [x] Phase 1 — Project scaffold + build pipeline
- [x] Phase 2 — UI framework analysis (v0.4.6)
- [x] Phase 3 — Mod selection & architecture
- [x] Phase 4 — Development & Verification (active)
