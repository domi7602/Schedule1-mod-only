# Build, deployment and artifact verification

> verified: pipeline re-checked 2026-10-05 against the current workspace state (deploy rows + bump caveats).
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 42 of 42 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Before the first build

Locate the actual repository instructions and project/solution files. Read imported props/targets and scripts that may run during build. The original workspace used Source/Mods/Directory.Build.props, Directory.Build.targets and Tools/build-all.ps1; verify these paths and current behavior.

Resolve the game directory from existing configuration or an explicit user path. Never write to a guessed Steam path. Record whether the game is running and whether files could be locked.

## Separate four phases

| Phase | Work | Evidence |
|---|---|---|
| Inspect | Confirm target, dependency paths, generated vs owned files and side effects | Identified files/configuration |
| Build | Compile a focused target in an isolated output using the repository's supported deployment-disable mechanism | Command, exit code, warnings and output artifact |
| Deploy | Copy only approved artifacts through the established helper | Exact destination, backup/receipt when provided, content identity |
| Runtime check | Start/load under the intended setup and exercise the changed behavior | Actual loaded artifact and observed gameplay result |

Do not assume a plain Release build is read-only: the uploaded runbook describes auto-deployment. Do not invent an MSBuild switch. If the supported isolated route cannot be established, complete static review and identify that blocker before mutating the installation.

## Baseline and scope

Inspect existing changes first. Build the smallest affected project and required dependencies. Broaden to the solution when shared APIs, imported targets or an explicit gate justify it. Report baseline failures separately from new regressions; do not edit unrelated code just to obtain a clean total.

For repository maintenance, use `Tools/verify.ps1` as the shared gate entry point:
Quick runs formatting/solution/version/documentation/skill/tracked-XML guards; Pure also runs
game-free and tooling tests; Full also builds without deployment and runs all test
projects. Auto selects Full only when game references are present. Existing focused
tools remain available, and every actual verifier run records its steps and results.

The build, test and verifier wrappers share `Tools/dotnet-sdk.ps1`: explicit host,
then PATH, then a complete user-local host if PATH has none. Validate the active SDK
(8+ for this repository), not just the installed SDK list; never set `DOTNET_ROOT`
to a tools-only directory. `gen-sln.ps1 -Check` checks determinism without regenerating
the solution. Test/verifier `-ListOnly` discovers work without invoking the SDK.

Use `Tools/verify-clean-checkout.ps1 -Mode Pure` for the fresh-checkout gate after
documentation, tests, tooling or CI changes. It captures the current working files,
including nonignored new files and deletions, and records their hashes before
running the same verifier in isolation. A plain local clone contains only committed
HEAD and cannot validate uncommitted edits. Dirty submodule contents are rejected;
ignored machine-local files are intentionally absent. Capturing a snapshot alone
does not constitute passing verification. See `Tools/README.md` for profiles and
evidence locations.

For old WorldLook Unity runs, `Tools/clean-unity-caches.ps1` previews a narrow cache
allowlist; only explicit `-Apply` removes it. A Library also holds build evidence:
keep the current project Library, backups, reports and unlisted diagnostics. Never
replace the helper with a recursive deletion of Library or whole run directories.

## Cross-platform build gates

A gate wired as `<Exec ... BeforeTargets="BeforeBuild">` runs on every build and is **not**
covered by `-p:S1NoDeploy=true`; only deploy targets are. When such a gate calls a
PowerShell script on a host without `pwsh`, the whole solution fails at exit 127 before any
project compiles. Before blaming the SDK, grep `Source/Mods/**/*.csproj` and
`Directory.Build.targets` for `Exec` plus `BeforeTargets`, and separate build gates from
deploy targets.

Keep Windows authoritative and add a non-Windows route instead of deleting the script:

- Port the check to a stdlib-only Python script beside the PowerShell one. Mirror every
  validation rule (contract fields, hashes/sizes, inventory, duplicate and stale checks,
  exit code) and keep the same success line so logs stay comparable across hosts.
- Select it in MSBuild with `Condition="'$(OS)' == 'Windows_NT'"` for `pwsh` and
  `Condition="'$(OS)' != 'Windows_NT'"` for the port, and expose
  `<S1Python Condition="'$(S1Python)' == ''">python3</S1Python>` so
  `-p:S1Python=/path/to/python3` overrides a missing PATH entry. CI runs on
  `windows-latest`, so the PowerShell branch stays the authoritative gate.
- Do not install PowerShell merely to unblock a Linux build; port the one blocking step.

Frozen provenance: when the PowerShell script hashes a glob such as `build/*.ps1` into a
receipt produced by the Windows editor toolchain, the port must stay OUTSIDE that glob —
no `*.py` in the inventory — or every later build on both hosts fails as "input changed".
Record that constraint next to the script.

PowerShell and Python do not share comparison or filesystem semantics; a port that reads
correctly can still accept or reject the wrong inputs:

- `-match`, `-eq` and `-in` compare case-insensitively, while `-ceq` and .NET `EndsWith`
  are ordinal. Casefold names and regexes for exclusion lists, or `.ENV`, `THUMBS.DB` and
  `PRIVATE.LOCAL.JSON` pass a filter that blocks their lowercase twins.
- `Get-ChildItem -Filter` matches case-insensitively and skips hidden entries; `pathlib`
  `glob` is case-sensitive and includes dotfiles. Match with `name.casefold().endswith(...)`
  and skip leading dots so the hashed inventory is identical on both hosts.
- `os.walk` swallows unreadable directories unless `onerror` raises; PowerShell's
  `$ErrorActionPreference='Stop'` aborts. A silent skip lets a backup or scan report success
  over missing data.
- `Copy-Item` keeps timestamps, `shutil.copyfile` does not; use `shutil.copy2` for backups.
- Catch `UnicodeDecodeError` next to `JSONDecodeError`; otherwise a corrupt input prints a
  traceback instead of the gate's own `error:` line.
- PowerShell coerces `1.0`/`"1"` when comparing numbers; a strict Python `isinstance(int)`
  check false-rejects hand-written receipts. Accept integral floats and numeric strings.

Verify the port by behavior, not by reading it: one positive run against the real artifact,
then negative probes on scratch copies — flip one byte in the hashed artifact, edit a hashed
input, delete an inventory file, rewrite one receipt field. Each must fail with its own
message and exit 1. Finish with
`dotnet build Source/Mods/S1Mods.sln -c Release -p:S1NoDeploy=true` at 0 warnings/0 errors.

Remaining Linux gap: deploy targets still invoke `pwsh Tools/deploy-mod.ps1`, so a plain
build without the no-deploy property breaks outside Windows. Use the no-deploy property for
every verification build; port the deploy helper (plan, preflight, mutex, backups, receipts,
rollback) only when installation on that host is actually required.

## Deployment

Check owner authorization already present in the task. Inspect the actual helper and allowlist, enumerate planned outputs, and preserve rollback data for replaced artifacts. Do not remove Shared or another dependency while retaining mods that need it.

Compare SHA256 of produced and installed files. A byte-identical file can correctly keep an older timestamp. A matching filename/version string alone is insufficient.

After deployment, verify which artifact the loader actually loaded. A file copy does not prove successful initialization.

## Failure cases

- Locked destination: preserve built artifact and the previous installed one; retry only after the lock condition is resolved.
- Partial copy: use the helper's actual receipt/rollback rules; do not claim all-or-nothing deployment unless implemented.
- Dependency mismatch: align the requested target and references; do not silently deploy newest framework binaries.
- Stale DLL suspicion: compare identity and load paths before destructive cleanup.
- Build warnings: classify existing and new warnings; meet actual project gates without hiding diagnostics.

## Packaging and release

Use the current packaging script when verified. Include required assets/metadata and explicit dependency requirements. Do not bundle third-party binaries by default; inspect project distribution rules. Version changes follow [version synchronization](version-sync.md).

`Tools/package-release.ps1` rebuilds with deployment disabled and validates the
ZIP's `release-manifest.json`. `Tools/release-mod.ps1` checks source identity and
all declared payload hashes before publishing; an existing versioned filename is
not proof of a current package. Dirty local packages are review artifacts, not
publishable releases. Follow `docs/release-process.md` for the exact contract and
limits. Running a verifier or package builder does not authorize tags or publication.

## Delivery

Report built, deployed and runtime-tested as separate statuses. Include only applicable commands/results and concrete remaining manual checks. A build-only task ends with a build artifact and its evidence, not an unsolicited deployment.
