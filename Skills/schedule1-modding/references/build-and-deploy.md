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

## Delivery

Report built, deployed and runtime-tested as separate statuses. Include only applicable commands/results and concrete remaining manual checks. A build-only task ends with a build artifact and its evidence, not an unsolicited deployment.
