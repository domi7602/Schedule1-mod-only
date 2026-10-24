# Repository maintenance

Current work and remaining native checks, updated 2026-10-24. This page is a work
tracker; [compatibility](compatibility.md) owns gameplay acceptance and its limits.
[The preceding snapshot](history/repository-maintenance-20261024.md) preserves earlier
checklists, deployment fingerprints, warnings and test totals.

## Current repository cleanup

Scope: clear documentation ownership, consistent verification/setup, predictable local
storage and guarded cache cleanup. Existing mod edits are retained. Mod versions,
gameplay code, installed files and current Unity/backups are outside this pass.

| Package | Status | Result / remaining check |
|---|---|---|
| Documentation and historical evidence | Implemented, reviewed | Current source/test entries; original MessagesPlus and TaxiDriver texts preserved with hashes in their mod histories. |
| Shared SDK and verification workflow | Implemented, fixture-tested | Shared host selection and active SDK checks; PR/developer commands use the no-deploy verifier. |
| New-device setup | Implemented, fixture-tested | Recursive submodules, checked failure paths, noninteractive options and accurate remote guidance. |
| Local layout and scaffold | Implemented, smoke-tested | Empty Analysis/Notes removed after archiving the handoff; new scaffolds point to Source/Tests without placeholder test folders. |
| Documentation paths | Implemented, fixture-tested | 289 maintained documents checked; file-relative links repaired, historical literal paths narrowly annotated. |
| Unity cache helper | Implemented, independently reviewed | 53 synthetic checks passed; real workspace preview found zero remaining allowed candidates and changed nothing. |
| Independent review | Passed | 701 baseline C# / project / mod-metadata files unchanged; archived originals preserved and documentation agrees with the tools. |

### Verification evidence for this pass

The local no-deploy Full run `verify-6ed568455baf` passed all 20 blocks: Release
compilation, all 18 mod test projects (3,144 xUnit tests plus 265 LightControl checks)
and 627 tool assertions. Two existing deployment symlink cases were skipped because
the host lacks the required privilege. The known net6 support and skills-version
warnings remain visible. This is build/test evidence, not a new gameplay acceptance.

The final default-scratch adjustment avoids AppData virtualization; it has dedicated
path-selection fixtures. The final isolated Pure verdict, snapshot identity and report
paths are recorded in the local completion receipt at
`.scratch/repo-cleanup-17374603/completion.json`. That receipt is finalized after the
isolated run and source-preservation checks, outside the captured source tree. Local
receipts are optional evidence on this device, never prerequisites for a fresh clone.

The current Unity Library and all backups were separately compared with the pre-cleanup
manifest: 29,216 files matched SHA-256, length and modification time. The embedded shader
bundle verifier still identifies `29557b87…`. No real cache deletion was needed in this
pass; only the helper's synthetic fixtures exercise deletion and failure paths.

No commit, push or installation is implied by this tracker. Consult `git status` and
`git log` for the actual committed state. Git hooks remain opt-in.

## Completed foundation

- Shared `Tools/verify.ps1` entry point, isolated working-file snapshot verification and
  release-package source/payload provenance were implemented before this pass. The
  [preceding record](history/repository-maintenance-20261024.md#current-repository-improvement--2026-10-24)
  preserves the successful Full snapshot and its exact scope.
- AGENTS.md and compatibility.md were reduced from 88,656 to 34,174 bytes combined;
  historical details remain in their linked archives. The isolated Pure check passed
  for that documentation snapshot; it does not validate subsequent edits.
- Old Unity caches: 96,807 files / 11,613,326,434 bytes removed from seven old runs on
  2026-10-24. All 34,217 protected files matched their prior SHA-256, size and modification
  time. Current Unity Library, backups, buildreports and Bee traces were retained.
  The local receipt is under `.scratch/unity-cache-cleanup-20261024/receipt.json`;
  it is machine-local, not a clone prerequisite.
- Known retained limits: no crash/solution-wide deployment rollback, no automatic backup
  pruning or stale installed-file deletion. Keep the game closed throughout deployment.
  The net6 console-host support warning is retained for runtime parity; skills evidence
  warnings are classified rather than hidden. See [Tools](../Tools/README.md).

## Documentation ownership

| Location | Current responsibility |
|---|---|
| [AGENTS.md](../AGENTS.md) | Agent rules and machine-checked mod inventory rows |
| [compatibility.md](compatibility.md) | Current gameplay evidence, attribution and limits |
| This page | Repository work and links to the remaining WorldLook work |
| Mod docs/README.md and CHANGELOG.md | Behaviour, usage and dated release changes |
| docs/history and mod-local docs/history | Superseded narratives and preserved evidence, not an active backlog |

Keep required inventory/version rows aligned with the version tools. Prefer links for
repeated narratives. Accepted work stays closed; historical unexecuted checks are not
silently upgraded to individually verified actions.

## Local workspace storage

- `.scratch/`: task-owned scratch inputs, exports and local verification receipts. Use a
  descriptive task directory; do not mix disposable caches with irreplaceable evidence.
- `.cache/`: local tool/index caches, including the decompiler installation. Delete only
  identified regenerable data, not the whole directory by default.
- `.backups/`: retained recovery artifacts and manifests; outside automatic cleanup.
- `DisabledMods/`: local inactive third-party DLLs, excluded from Git by the binary rules.
  Their presence does not make them active solution projects or release inputs.
- `Release/`: generated ZIP output. Source belongs in Source/Mods; real tests in Source/Tests.
- Mod-local `tests/` can retain specific notes/fixtures. New scaffolds do not create a
  competing executable-test location. [Developer workflow](../DEVELOPERS.md#test).

## Remaining native work: WorldLook

WorldLook 0.3.0-dev.2 is the only active development mod. Its
[canonical water-surface record](../Source/Mods/WorldLook/docs/water-surface-verification.md)
identifies DLL `410fecf7…` and bundle `29557b87…`, the inherited 1,167 water tests,
GPU/build reviews and owner-confirmed calm morning motion. No new native check is
claimed by repository maintenance.

- [ ] Overall/direct-sun appearance.
- [ ] Positive warmth near 06:00 or 18:00; zero strength after 07:30 is expected.
- [ ] ON after OFF, then night and menu/save/scene reload.
- [ ] Comparable ON/OFF game frame-time measurements.

Owner-accepted mods remain closed, including the version-only Minimap/TaxiDriver releases.
Cancelled Minimap search is not a backlog. Multiplayer is out of scope; existing guards
and their unverified limits remain. GoldenHour/WaterShader ship inside WorldLook and
GraphicsProbe is archived; they are not separate pending releases.
