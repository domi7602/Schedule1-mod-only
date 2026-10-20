# Tools

PowerShell 7 helper scripts for building, checking, releasing and setting up the workspace. Run them from the repository root, e.g. `pwsh Tools/build-all.ps1`. Scripts that touch the game directory resolve it from `$env:SCHEDULE1_PATH` with the Steam default as fallback.

## Build and solution

| Script | Purpose |
|---|---|
| `build-all.ps1` | Builds `Source/Mods/S1Mods.sln` (`-Config Release` by default); regenerates the SLN first if it is missing. Deploys into the game directory unless `S1NoDeploy=true`. |
| `gen-sln.ps1` | Regenerates `S1Mods.sln` from all `*.csproj` under `Source/Mods/` and `Source/Tests/` with deterministic MD5-based project GUIDs. Run after adding or removing a project; CI verifies the committed SLN matches. |
| `new-mod.ps1` | Scaffolds `Source/Mods/<Name>/` with `src/` (csproj + `Mod.cs`), `docs/` (`mod.json`, `README.md`, `CHANGELOG.md`), `assets/` and `tests/`. |
| `deploy-thirdparty.ps1` | Called by `Directory.Build.targets` when `Shared` builds: copies whitelisted third-party DLLs from `ThirdParty/` into `<GameDir>\Mods\` according to `ThirdParty/.deployallow`. Uses a mutex so parallel MSBuild invocations do not race. |

`build-all.ps1 -NoDeploy` disables deployment; an existing `S1NoDeploy=true` environment setting is also respected. `-GameDir` overrides the installation path consistently for mod and third-party copies, and `-DotnetPath` selects the SDK host. The active SDK must support C# 12.

`check-deploy-preflight.ps1 [-GameDir <path>] [-Json]` is a read-only gate: rejects a missing/unsafe installation, a running game, or an unreadable process state. MSBuild deployment targets and the standalone third-party deploy call it before copying. The third-party deploy requires explicit allowlist entries, shares the per-install mutex with normal mod deployment, and checks SHA-256. Every nonempty pattern must match. Existing differing DLLs and workspace-mod name collisions are refused before copying; upgrades require explicit owner-managed handling, not deleting an installed DLL as an automatic workaround. Emptying the allowlist is not an uninstall.

`deploy-mod.ps1` deploys one project at a time and records absolute source/destination paths,
SHA256 hashes, configuration, Git revision and dirty state. Latest receipts live under
`<GameDir>/UserData/.deploy/receipts/`; overwritten-file backups live under
`<GameDir>/UserData/.deploy/backups/`. Backups never recursively include saves/configuration.
Identical artifacts are checked but not rewritten. In-process copy/verification failure
attempts to restore only this project's touched targets; failed restores are reported, not
called success. Keep backups until the corresponding build is accepted; there is no automatic
retention cleanup.

This is **not** an all-solution transaction, crash recovery or protection from arbitrary
external writers. A process kill/power loss needs manual recovery from verified backups;
already-deployed projects are not rolled back if a later project fails. A game launched after
preflight remains a race: keep it closed until deployment completes. Stale artifacts need
explicit review; no automatic deletion of previous mod assets or third-party DLLs is authorized.

## Tests

`pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/test-all.ps1 -Selection pure` runs game-free xUnit suites and the LightControl console host. Use `-Selection all` to include game-required suites; missing game references then fail rather than silently skipping them. `-ListOnly` prints classification, `-Configuration Debug|Release` selects the configuration, and `-DotnetPath` overrides SDK resolution. Every test build disables deployment. CI and the pre-push hook use this same runner; a successful console run is separate from xUnit test discovery.

## Tooling regression tests

Run `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/Tests/bump-version.Tests.ps1`
and `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/Tests/deploy.Tests.ps1`,
plus `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/Tests/test-all.Tests.ps1`,
`pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/Tests/deploy-mod.Tests.ps1` and
`pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/Tests/skills-health.Tests.ps1`.
CI discovers and runs all of these suites.
They use disposable fixtures outside the repository, not real mod versions or the game
installation. The version tool preflights metadata before writes and preserves prior
acceptance as history; an already-open row keeps its evidence unchanged. Ambiguous plain
`Verified` text outside supported labels requires manual status reconciliation and aborts
without writing (for example PotScanner's mixed current/core acceptance label).

## Repository checks (CI, pre-commit and pre-push)

| Script | Purpose |
|---|---|
| `check-version-sync.ps1` | Verifies that the code version (`MelonInfo` attribute or `Constants.ModVersion`) matches `docs/mod.json`, the mod table in `README.md` and the inventory row (plus detail header) in `AGENTS.md`. Exit 1 on drift. |
| `check-doc-paths.ps1` | Verifies that every repository path referenced in Markdown (root docs, `docs/`, `Skills/`, `ThirdParty/README.md`, `GameReferences/README.md`) exists. Exit 1 on dead references. |
| `check-skills-health.ps1` | Static health scan of `Skills/`: frontmatter, reference verification headers, duplicate reference filenames, stale pinned versions, broken relative links, CRLF line endings. Exit 1 only on FAIL-level findings; warnings exit 0. |
| `bump-version.ps1` | `-Mod <Name> -Version x.y.z [-DryRun]` — updates code, `mod.json`, `CHANGELOG.md` header, `AGENTS.md` and `README.md` in one step. The only supported way to change a version. |

## Release

| Script | Purpose |
|---|---|
| `package-release.ps1` | `[-Mod <Name>|All] [-OutputDir Release]` — builds the mod(s) in Release and writes `Release/<Mod>-v<version>.zip` mirroring the game layout (`Mods/` with the mod DLL, `Shared.dll` and icons; `UserData/<Mod>/` with `mod.json` and PDB; README and CHANGELOG at the root). Fails fast without game assemblies. See [`docs/release-process.md`](../docs/release-process.md). |
| `release-mod.ps1` | `-Mod <Name> [-Draft] [-DryRun]` — automates steps 2–3 of [`docs/release-process.md`](../docs/release-process.md): version-sync gate, clean/up-to-date checks, packages the ZIP if missing, pushes the annotated tag `<Mod>-v<version>`, and creates the GitHub release with the matching CHANGELOG section as notes (requires `gh`). |

## Workspace setup and maintenance

| Script | Purpose |
|---|---|
| `setup-workspace.ps1` | Generates the git-ignored `local.build.props` for the S1API and S1MAPI submodules on a new machine; prompts for the game path if it is not the Steam default; optionally enables the pre-commit hook. Idempotent. |
| `new-laptop-workspace.ps1` | Clones the repository into a second, sandboxed workspace whose remote is renamed to `upstream` (so it cannot push to the main repo by accident) and runs `setup-workspace.ps1` there. |
| `bootstrap-game-references.ps1` | Decompiles `<GameDir>\MelonLoader\Il2CppAssemblies` with `ilspycmd` (installed into `.cache/tools/`) into the git-ignored `GameReferences/decompiled/`. Research aid only. |
| `mods-cleanup-inventory.ps1` | Inventories `<GameDir>\Mods\` and, with `-Apply`, moves runtime-DLL duplicates into `MelonLoader\_archived\` (reversible move, `-WhatIf` by default). |
| `backup-to-d.ps1` | Robocopy mirror of the game folder, saves and this workspace to a backup drive (`-TargetRoot`, default `D:\Schedule 1\Backups`). Maintainer convenience, machine-specific. |
| `build-upload-sets.ps1` | `[-Mod <Name>] [-OutDir <dir>] [-MaxFiles 20]` — writes per-mod review/upload **copies** to `.scratch/gpt-uploads/<Mod>/`, capped at `MaxFiles` files each, so a whole mod fits an external reviewer's attachment limit. Mods above the limit are emitted as one block-bundle per `src/` domain folder (each original file verbatim in its own block, `// from:` marker, file-scoped `namespace` converted to block form, `extern alias` hoisted). Never touches the repo sources; idempotent; writes `UEBERSICHT.md` with the bundle map. |

## Conventions

- Scripts are UTF-8, LF line endings (enforced by `.gitattributes`), 4-space indentation (`.editorconfig`).
- Scripts that can break CI (`check-*.ps1`, `gen-sln.ps1`) must be side-effect free apart from their documented output and must exit non-zero on failure.
- When adding a script, document it here and, if it becomes a gate, in `.github/workflows/ci.yml` and the git hooks (`.githooks/pre-commit`, `.githooks/pre-push`).
