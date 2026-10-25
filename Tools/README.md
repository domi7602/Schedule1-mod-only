# Tools

PowerShell 7 helper scripts for building, checking, releasing and setting up the workspace.
Run from the repository root with `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/<script>.ps1`.
Scripts that touch the game directory resolve it from `SCHEDULE1_PATH` with the Steam default as fallback.
Start routine checks with `verify.ps1`; use the individual tools for focused work.

## Build and solution

| Script | Purpose |
|---|---|
| `build-all.ps1` | Builds `Source/Mods/S1Mods.sln` (`-Config Release` by default); regenerates the SLN first if it is missing. Deploys into the game directory unless `S1NoDeploy=true`. |
| `gen-sln.ps1` | Regenerates `S1Mods.sln` from all `*.csproj` under `Source/Mods/` and `Source/Tests/` with deterministic MD5-based project GUIDs. Run after adding or removing a project; CI verifies the committed SLN matches. |
| `new-mod.ps1` | Scaffolds `Source/Mods/<Name>/` with `src/` (csproj + `Mod.cs`), `docs/` (`mod.json`, `README.md`, `CHANGELOG.md`) and `assets/`. The README directs executable tests to `Source/Tests/<Name>.Tests/`; no placeholder test folder/project is created. |
| `deploy-thirdparty.ps1` | Called by `Directory.Build.targets` when `Shared` builds: copies whitelisted third-party DLLs from `ThirdParty/` into `<GameDir>\Mods\` according to `ThirdParty/.deployallow`. Uses a mutex so parallel MSBuild invocations do not race. |

`build-all.ps1 -NoDeploy` disables deployment; an existing `S1NoDeploy=true` environment setting is also respected. `-GameDir` overrides the installation path consistently for mod and third-party copies, and `-DotnetPath` selects the SDK host. The active SDK must support C# 12.

`build-all.ps1`, `test-all.ps1` and `verify.ps1` share `dotnet-sdk.ps1`: explicit
`-DotnetPath`, then the executable on PATH, then an existing user-local host if PATH
has none. The selected active SDK must be 8+; merely having SDK 8 installed beside
an active older SDK is insufficient. An invalid explicit path or selected SDK fails
with a diagnostic. No helper changes `DOTNET_ROOT`, PATH or machine configuration.
The dot-sourced helper invokes nothing until called; test/verifier `-ListOnly` remains
an inspection operation without SDK execution or report writes.

`gen-sln.ps1 -Check` compares the expected solution in memory, tolerates LF/CRLF
differences, and fails on a missing or stale solution without changing it.

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

`verify.ps1 -Mode Pure` discovers every `Tools/Tests/*.Tests.ps1` suite; CI uses the
same discovery. For focused work invoke the affected suite directly with
`pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/Tests/<suite>.Tests.ps1`.
Coverage includes version/deploy contracts, SDK selection, setup/clone failures,
Markdown paths, cache preservation, orchestration, snapshots and release provenance.
The suites never publish or modify the installed game.
They use disposable fixtures outside the repository, not real mod versions or the game
installation. The version tool preflights metadata before writes and preserves prior
acceptance as history; an already-open row keeps its evidence unchanged. Ambiguous plain
`Verified` text outside supported labels requires manual status reconciliation and aborts
without writing (for example PotScanner's mixed current/core acceptance label).

## Repository checks (CI, pre-commit and pre-push)

`pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/verify.ps1` is the common
entry point for local checks, CI and the Git hooks. It calls the existing tools;
it does not replace their individual commands.

| Mode | Checks |
|---|---|
| `Quick` | Formatting, read-only solution determinism, version sync, documentation paths, skills health and tracked MSBuild XML. |
| `Pure` | Quick checks, game-free tests and every discovered `Tools/Tests/*.Tests.ps1` suite. |
| `Full` | Quick checks, a Release build with deployment disabled, all test projects and tool suites. Requires game references. |
| `Auto` (default) | Full when game references exist; otherwise Pure. |

Use `-ListOnly` to inspect the selected steps without executing them, `-DotnetPath`
to choose an SDK host, and `-ScratchRoot` for a runner-owned output directory.
Every executed run retains step logs and `report.json` in a unique `verify-*`
folder. The default is `%USERPROFILE%/.cache/Schedule1Tools/verification` (with
LOCALAPPDATA fallback only when USERPROFILE is unavailable). A failing
step makes the command fail; a successful Pure run does not claim a game build
or gameplay verification. Pre-commit uses Quick, pre-push uses Auto, and hosted
CI uses the same entry point. Keep the game closed for any build as required by
the workspace contract.

### Verify uncommitted work in isolation

`pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/verify-clean-checkout.ps1 -Mode Pure`
captures current tracked files, deletions, nonignored new files and clean submodule
commits into a fresh checkout. It then runs the same verifier there. This covers
the working files, including changes made after staging; it is not a staged-only
commit check. Ignored build output, local configuration and generated decompiles
are excluded. Modified or untracked files inside submodules, unresolved merges
and symlinks/reparse points are rejected explicitly rather than silently omitted.

The source worktree/index stays untouched. The checkout, `snapshot.json` (HEAD,
source digest and per-file hashes) and logs remain under a unique external scratch
directory, by default `%USERPROFILE%/.cache/Schedule1Tools/clean-checkouts`.
Both verifier defaults avoid the packaged Windows app's AppData path redirection;
an explicit `-ScratchRoot` still takes precedence. Source changes during
capture/verification invalidate the result.
`-SnapshotOnly` prepares the artifact without running checks and reports Captured,
never Passed. Submodule initialization may need network access; no credentials or
machine-local Git configuration are copied. Use this command after changes to
documentation, tests, tooling or CI to catch hidden local prerequisites before a
commit. A plain `git clone .` alone checks only committed content.

| Script | Purpose |
|---|---|
| `check-version-sync.ps1` | Verifies that the code version (`MelonInfo` attribute or `Constants.ModVersion`) matches `<Mod>/docs/mod.json`, the mod table in `README.md` and the inventory row (plus detail header) in `AGENTS.md`. Exit 1 on drift. |
| `check-doc-paths.ps1` | Checks maintained root, docs, Skills, Tools and Source/Mods Markdown plus the ThirdParty/GameReferences READMEs. Actual Markdown links resolve from their source document, including parent paths and plain filenames. Exit 1 on missing nonoptional file targets. |
| `check-skills-health.ps1` | Static health scan of `Skills/`: frontmatter, reference verification headers, duplicate reference filenames, stale pinned versions, broken relative links, CRLF line endings. Exit 1 only on FAIL-level findings; warnings exit 0. |
| `check-xml.ps1` | Parses Git-tracked `.csproj`, `.props` and `.targets`, including archived projects. Does not read untracked or generated files. Exit 1 on invalid XML; the message names the file. |
| `bump-version.ps1` | `-Mod <Name> -Version x.y.z [-DryRun]` — updates code, `mod.json`, `CHANGELOG.md` header, `AGENTS.md` and `README.md` in one step. The only supported way to change a version. |

## Release

| Script | Purpose |
|---|---|
| `package-release.ps1` | `[-Mod <Name>\|All] [-OutputDir Release]` — rebuilds without deployment and creates ZIPs matching the game layout, including required mod/Shared DLLs, metadata and a source/payload manifest. Rejects missing inputs and preserves an existing ZIP on verification failure. See [`docs/release-process.md`](../docs/release-process.md). |
| `release-mod.ps1` | `-Mod <Name> [-Draft] [-DryRun]` — checks versions, source state and ZIP provenance before publishing an annotated tag and GitHub release. Existing ZIPs require a matching manifest; a matching filename alone is insufficient. DryRun stays local/read-only. Publishing still requires explicit owner authorization. |
| `release-manifest.ps1` | Dot-sourced helpers for source identity, required payloads, safe staging cleanup and complete ZIP hash validation; does not build or publish by itself. |

Each ZIP contains `release-manifest.json` with its mod/version, source HEAD,
dirty state/content fingerprint and payload hashes. Local dirty packages remain
possible for inspection; publication requires a matching clean source identity.
This evidence does not establish gameplay compatibility or bit-for-bit rebuilds
across different compilers/game references. The manual workflow requires an
available self-hosted Windows runner labelled `schedule1`; without one it queues.

## Workspace setup and maintenance

| Script | Purpose |
|---|---|
| `setup-workspace.ps1` | Validates both pinned dependency templates and local game references before generating git-ignored props. `-GameDir`, `-NonInteractive` and opt-in `-EnableHooks` control unattended setup; existing hook choices remain. Does not build/deploy. |
| `new-laptop-workspace.ps1` | Recursively clones into a new target, renames the source remote to `upstream`, optionally adds a backup remote and runs setup. The name is a convention, not a push restriction. Supports `-NonInteractive`, `-GameDir`, `-EnableHooks`; existing targets are refused and failed new clones retained for diagnosis. |
| `clean-unity-caches.ps1` | Previews known cache children of old WorldLook build/preview runs. `-Apply` performs guarded cleanup; `-Apply -WhatIf` previews the action. Current Unity Library, backups and evidence stay protected. Details below. |
| `bootstrap-game-references.ps1` | Decompiles `<GameDir>\MelonLoader\Il2CppAssemblies` with `ilspycmd` (installed into `.cache/tools/`) into the git-ignored `GameReferences/decompiled/`. Research aid only. |
| `mods-cleanup-inventory.ps1` | Inventories `<GameDir>\Mods\` and, with `-Apply`, moves runtime-DLL duplicates into `MelonLoader\_archived\` (reversible move, `-WhatIf` by default). |
| `backup-to-d.ps1` | Robocopy mirror of the game folder, saves and this workspace to a backup drive (`-TargetRoot`, default `D:\Schedule 1\Backups`). Maintainer convenience, machine-specific. |
| `build-upload-sets.ps1` | `[-Mod <Name>] [-OutDir <dir>] [-MaxFiles 20]` — writes per-mod review/upload **copies** to `.scratch/gpt-uploads/<Mod>/`, capped at `MaxFiles` files each, so a whole mod fits an external reviewer's attachment limit. Mods above the limit are emitted as one block-bundle per `src/` domain folder (each original file verbatim in its own block, `// from:` marker, file-scoped `namespace` converted to block form, `extern alias` hoisted). Never touches the repo sources; idempotent; writes `UEBERSICHT.md` with the bundle map. |

### Documentation-path scope

The guard uses tracked and nonignored new Markdown files, excluding generated outputs,
dependency source trees and imported `**/docs/upstream/` material. Markdown links and image
targets are checked from their source directory; a same-named file elsewhere cannot
mask a broken link. Inline-code paths keep a separate, conservative repository-path
heuristic. Historical path evidence can use the same-line annotation
`<!-- doc-paths: historical-reference (specific reason) -->`; this exempts only inline
code on that line, never real Markdown links. Use it only for actual historical evidence.
Missing ignored outputs and documented local dependency paths are reported as local
prerequisites. This is a file-path guard, not an anchor/HTML/full Markdown validator.

### Old Unity run caches

Run `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/clean-unity-caches.ps1`
for a read-only list and byte/file totals. The tool has no arbitrary path/root argument:
it recognizes only timestamp/id build runs and preview runs under WorldLook's build logs.
It removes only named package/import/shader caches and cache databases, never a whole
Library or run folder. Unexpected report/diagnostic files inside a candidate stop preflight.
Bee, BuildPlayerData, ScriptAssemblies, reports, source snapshots and all unlisted paths
remain. Keep Unity and the game closed throughout an explicit `-Apply`.

Apply validates paths/reparse points, rechecks candidate state, and hashes protected
WorldLook/backup files before and after. Receipts and preservation manifests remain under
`.scratch/unity-cache-cleanup/`. A failure can leave partial cache removal; the receipt
distinguishes completed targets from a possibly partial target. There is no rollback or
automatic background cleanup. An empty selection is a successful no-op.

## WorldLook live acceptance

`invoke-worldlook-mcp.ps1` provides explicit `Status`, `On`, `Off`, `Menu`, `Load` and `Time`
actions against the local S1MCP bridge. Mutations bind to an owner-selected native save and
game process, use deterministic WorldLook commands and independently verify completion.
Each action writes a fresh JSONL evidence file; ambiguous mutation outcomes are not retried.
See the [WorldLook automation contract](../Source/Mods/WorldLook/docs/mcp-automation.md)
for parameters, screenshot orchestration and the distinction between requested state and
rendering acceptance. This helper is not a CI gate and never launches/builds/deploys the game.

## Conventions

- Scripts are UTF-8, LF line endings (enforced by `.gitattributes`), 4-space indentation (`.editorconfig`).
- Checks (`check-*.ps1`, `gen-sln.ps1 -Check`) must be read-only apart from documented output and exit non-zero on failure. Run generators without their check flag only for intentional updates.
- When adding a script, document it here and, if it becomes a gate, in `.github/workflows/ci.yml` and the git hooks (`.githooks/pre-commit`, `.githooks/pre-push`).
