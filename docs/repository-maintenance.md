# Repository maintenance

Current work and remaining native checks, updated 2026-10-25. This page is a work
tracker; [compatibility](compatibility.md) owns gameplay acceptance and its limits.
[The preceding snapshot](history/repository-maintenance-20261024.md) preserves earlier
checklists, deployment fingerprints, warnings and test totals.

## Completed repository cleanup

Completed scope: clear documentation ownership, consistent verification/setup, predictable
local storage and guarded cache cleanup. Existing mod edits were preserved. Mod versions,
gameplay code, installed files and current Unity/backups were outside the cleanup pass.

| Package | Status | Result / remaining check |
|---|---|---|
| Documentation and historical evidence | Implemented, reviewed | Current source/test entries; original MessagesPlus and TaxiDriver texts preserved with hashes in their mod histories. |
| Shared SDK and verification workflow | Implemented, fixture-tested | Shared host selection and active SDK checks; PR/developer commands use the no-deploy verifier. |
| New-device setup | Implemented, fixture-tested | Recursive submodules, checked failure paths, noninteractive options and accurate remote guidance. |
| Local layout and scaffold | Implemented, smoke-tested | Empty Analysis/Notes removed after archiving the handoff; new scaffolds point to Source/Tests without placeholder test folders. |
| Documentation paths | Implemented, fixture-tested | 289 maintained documents checked; file-relative links repaired, historical literal paths narrowly annotated. |
| Unity cache helper | Implemented, independently reviewed | 53 synthetic checks passed; real workspace preview found zero remaining allowed candidates and changed nothing. |
| Independent review | Passed | 701 baseline C# / project / mod-metadata files unchanged; archived originals preserved and documentation agrees with the tools. |
| Warning follow-up | Committed and pushed as `cf9baae` | Historical skill evidence retained; scoped LightControl test-host EOL exception documented; mod/test target frameworks unchanged. |

### Latest committed verification — 2026-10-24

Commit `cf9baaef858dff52ea07dc8216b37ced3ec19852` is on `main`.
[CI #128](https://github.com/domi7602/Schedule1-mod-only/actions/runs/40117028773)
completed successfully: Auto selected Pure, all 19 verification blocks and all 17 selected
test projects passed, including 265 LightControl checks. The uploaded report and logs
confirmed zero skill warnings and zero NETSDK1138 warnings; the downloaded artifact's
SHA-256 matched GitHub's digest. The isolated local Pure snapshot also passed all 19 blocks.
These results cover that revision, not subsequent edits or new gameplay acceptance.

### Earlier Full verification and preservation evidence

The local no-deploy Full run `verify-6ed568455baf` passed all 20 blocks: Release
compilation, all 18 mod test projects (3,144 xUnit tests plus 265 LightControl checks)
and 627 tool assertions. Two existing deployment symlink cases were skipped because
the host lacks the required privilege. The known net6 support and skills-version
warnings were visible in that run; the later warning follow-up above does not rewrite
this historical result. This is build/test evidence, not a new gameplay acceptance.

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

Only the revision explicitly identified above carries a commit/push claim. Consult
`git status` and `git log` for later edits; no installation is implied. Git hooks remain opt-in.

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
  .NET 6 remains out of support; the LightControl test host retains runtime parity through
  the narrowly scoped [EOL diagnostic exception](../DEVELOPERS.md#test). The later skill
  follow-up preserves dated evidence with explicit annotations and current source links.
  See [Tools](../Tools/README.md) for the deployment limits.

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

WorldLook is the only active development mod; its **water work and ON/OFF are owner-accepted
and closed as of 2026-10-24**. The
[canonical water-surface record](../Source/Mods/WorldLook/docs/water-surface-verification.md)
identifies the accepted dev.2 DLL `410fecf7…` and bundle `29557b87…`, prior GPU/build reviews,
calm morning motion, the fresh ON→OFF reports and Dominik's acceptance. The dev.3 diagnostic
cleanup leaves the shader/recipe unchanged and was installed/hash-verified on 2026-10-24.
Dev.4 MCP control was installed and tested on 2026-10-25; scope and retained defects follow below.

The [acceptance record and protocol](../Source/Mods/WorldLook/docs/in-game-acceptance.md)
separates closed water work from remaining WorldLook evidence. Positive numeric warmth was
observed at 17:42. Quantitative frame-time work still needs a verified recorder and an
owner-agreed performance budget. F9 is removed in dev.3; existing reports remain historical.

- [x] Water appearance, including direct sun; owner accepted 2026-10-24.
- [x] ON/OFF including re-enable; owner confirmed without errors 2026-10-24.
- [ ] Visible positive warmth near 06:00 or 18:00; positive numeric strength already observed.
- [x] Native night; dev.3 owner-confirmed normal on 2026-10-25.
- [x] Automated ON/OFF menu/reload state and measured-light checks, dev.4 on 2026-10-25 in Dominik; seven agent-captured screenshots. Reference-water shader reload still fails separately.
- [ ] Repair and recheck the reference-water shader reload path; prior appearance acceptance remains closed.
- [ ] Comparable ON/OFF game frame-time measurements.

Owner-accepted mods remain closed, including the version-only Minimap/TaxiDriver releases.
Cancelled Minimap search is not a backlog. Multiplayer is out of scope; existing guards
and their unverified limits remain. GoldenHour/WaterShader ship inside WorldLook and
GraphicsProbe is archived; they are not separate pending releases.
