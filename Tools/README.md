# Tools

PowerShell 7 helper scripts for building, checking, releasing and setting up the workspace. Run them from the repository root, e.g. `pwsh Tools/build-all.ps1`. Scripts that touch the game directory resolve it from `$env:SCHEDULE1_PATH` with the Steam default as fallback.

## Build and solution

| Script | Purpose |
|---|---|
| `build-all.ps1` | Builds `Source/Mods/S1Mods.sln` (`-Config Release` by default); regenerates the SLN first if it is missing. Deploys into the game directory unless `S1NoDeploy=true`. |
| `gen-sln.ps1` | Regenerates `S1Mods.sln` from all `*.csproj` under `Source/Mods/` and `Source/Tests/` with deterministic MD5-based project GUIDs. Run after adding or removing a project; CI verifies the committed SLN matches. |
| `new-mod.ps1` | Scaffolds `Source/Mods/<Name>/` with `src/` (csproj + `Mod.cs`), `docs/` (`mod.json`, `README.md`, `CHANGELOG.md`), `assets/` and `tests/`. |
| `deploy-thirdparty.ps1` | Called by `Directory.Build.targets` when `Shared` builds: copies whitelisted third-party DLLs from `ThirdParty/` into `<GameDir>\Mods\` according to `ThirdParty/.deployignore`. Uses a mutex so parallel MSBuild invocations do not race. |

## Repository checks (CI, pre-commit and pre-push)

| Script | Purpose |
|---|---|
| `check-version-sync.ps1` | Verifies that the code version (`MelonInfo` attribute or `Constants.ModVersion`) matches `docs/mod.json`, the mod table in `README.md` and the inventory row (plus detail header) in `AGENTS.md`. Exit 1 on drift. |
| `check-doc-paths.ps1` | Verifies that every repository path referenced in Markdown (root docs, `docs/`, `Skills/`, `ThirdParty/README.md`, `GameReferences/README.md`) exists. Exit 1 on dead references. |
| `bump-version.ps1` | `-Mod <Name> -Version x.y.z [-DryRun]` — updates code, `mod.json`, `CHANGELOG.md` header, `AGENTS.md` and `README.md` in one step. The only supported way to change a version. |

## Release

| Script | Purpose |
|---|---|
| `package-release.ps1` | `[-Mod <Name>|All] [-OutputDir Release]` — builds the mod(s) in Release and writes `Release/<Mod>-v<version>.zip` mirroring the game layout (`Mods/` with the mod DLL, `Shared.dll` and icons; `UserData/<Mod>/` with `mod.json` and PDB; README and CHANGELOG at the root). Fails fast without game assemblies. See [`docs/release-process.md`](../docs/release-process.md). |
| `release-mod.ps1` | `-Mod <Name> [-Draft] [-DryRun]` — automates steps 2–3 of [`docs/release-process.md`](../docs/release-process.md): version-sync gate, clean/up-to-date checks, packages the ZIP if missing, pushes the annotated tag `<Mod>-v<version>`, and creates the GitHub release with the matching CHANGELOG section as notes (requires `gh`). |

## Workspace setup and maintenance

| Script | Purpose |
|---|---|
| `setup-workspace.ps1` | Generates the git-ignored `local.build.props` for the S1API and S1MAPI submodules on a new machine; prompts for the game path if it is not the Steam default; optionally enables the pre-commit hook; `-BootstrapGameReferences` additionally generates the local game decompiles under `GameReferences/`. Idempotent. |
| `new-laptop-workspace.ps1` | Clones the repository into a second, sandboxed workspace whose remote is renamed to `upstream` (so it cannot push to the main repo by accident) and runs `setup-workspace.ps1` there. |
| `bootstrap-game-references.ps1` | Decompiles `<GameDir>\MelonLoader\Il2CppAssemblies` with `ilspycmd` (pinned net6-compatible version, installed into `.cache/tools/`) into the git-ignored `GameReferences/decompiled/`. A failing tool never destroys existing output (staging + atomic replace). Research aid only; also reachable via `setup-workspace.ps1 -BootstrapGameReferences`. |
| `mods-cleanup-inventory.ps1` | Inventories `<GameDir>\Mods\` and, with `-Apply`, moves runtime-DLL duplicates into `MelonLoader\_archived\` (reversible move, `-WhatIf` by default). |
| `backup-to-d.ps1` | Robocopy mirror of the game folder, saves and this workspace to a backup drive (`-TargetRoot`, default `D:\Schedule 1\Backups`). Maintainer convenience, machine-specific. |

## Conventions

- Scripts are UTF-8, LF line endings (enforced by `.gitattributes`), 4-space indentation (`.editorconfig`).
- Scripts that can break CI (`check-*.ps1`, `gen-sln.ps1`) must be side-effect free apart from their documented output and must exit non-zero on failure.
- When adding a script, document it here and, if it becomes a gate, in `.github/workflows/ci.yml` and the git hooks (`.githooks/pre-commit`, `.githooks/pre-push`).
