# Historical snapshot — 2026-10-24

Snapshot before the second repository cleanup. Completed work, prior test totals, deployment fingerprints and then-open checklists retain their original scope. Use the [current maintenance page](../repository-maintenance.md) for active work and [compatibility](../compatibility.md) for gameplay acceptance.

Original: `docs/repository-maintenance.md`. Original SHA-256: `4ef60146a10445c2cf5dfe251e847640ceb255ac4eb3d3835934ba8e08362324`.
Only relative Markdown link destinations were relocated; the original text is retained below.

<!-- preserved-document:start -->
# Repository maintenance checklist

Durable tracker for repository clean-up and remaining native (in-game) checks. This is not a
gameplay acceptance record and it does not replace `docs/compatibility.md`.

**Snapshot:** 2026-10-24. Status labels used below:

- **Observed** — verified by reading the working tree, files, hashes or `Latest.log` during this snapshot.
- **Parent-reported** — reported by the coordinating agent's run; not independently repeated here.
- **Proposed** — a test to run; it has **not** been executed.
- **Open** — no evidence yet.

## Current repository improvement — 2026-10-24

Scope: make routine verification reproducible and the current documentation consistent.
The existing LightControl and acceptance-record edits are retained. Mod feature development,
installation, commits and pushes are outside this maintenance pass.

| Work item | Current status | Completion evidence |
| --- | --- | --- |
| Reconcile SDK/runtime/S1API guidance | Implemented in the working tree | README, DEVELOPERS and CONTRIBUTING agree on SDK 8, runtime 6, game f12 and S1API 3.2.1 stable. |
| Shorten the agent entry point | Implemented in the working tree | Current inventory and hard rules remain in AGENTS; original detailed history is preserved in the dated archive. |
| Remove superseded open-work labels | Implemented in the working tree | Owner-accepted mods are listed below; former item-level checklists remain historical and are not individually relabeled passed. |
| Add one no-deploy verification command | Implemented and fixture-tested | `Tools/verify.ps1` with Quick, Pure, Full and Auto modes; CI and hooks share it. 36 orchestration checks pass, including failure propagation and no regeneration of a missing solution. |
| Add isolated snapshot verification | Implemented and fixture-tested | `Tools/verify-clean-checkout.ps1` captures working files, Git modes and submodule commits. 35 checks pass, including dirty files, deletions, ignored-but-tracked files, nested junctions and source/index preservation. |
| Bind release packages to their inputs | Implemented and fixture-tested | 24 release checks pass; ignored production inputs are rejected. A real no-deploy NotesApp rebuild produced a verified seven-file ZIP payload; installed NotesApp/Shared artifacts stayed hash-identical. |
| Run the final integrated checks | Passed in an isolated Full run | All 16 verification blocks passed: Release build, 18 mod test projects (3,144 xUnit tests and 265 LightControl checks), and 440 tool checks. Two existing symlink fixtures were skipped because this host lacks symlink permission. Inherited NoDeploy and nested Windows path defects found during integration were repaired and retested. |

The completed Full snapshot is `clean-checkout-37b76a207dd9`, source digest
`a59e82ebe327f37e736b95fa04d574b8566da7af495415ce68e5e3ea03b78826`.
Its report and snapshot manifest both record success; the source and snapshot were
unchanged throughout verification. This results annotation was added afterward and
does not alter production or tooling inputs. The build retains the two expected
`NETSDK1138` warnings; the skills scan retains nine version-reference warnings.
Remote GitHub CI was not triggered. The release workflow requires a configured
self-hosted Windows runner with the `schedule1` label and game references.

The [dated history](agent-mod-history-20261024.md) retains the original AGENTS mod
narrative and superseded maintenance checklist. The [compatibility matrix](../compatibility.md)
and each mod's own record remain authoritative for gameplay acceptance.

## 1. Repository clean-up

### Earlier maintenance results (historical)

- **Existing source layout:** mod sources live under `Source/Mods/<Mod>/src/`; tests live
  under `Source/Tests/`. This maintenance pass does not reorganize mod sources.
- **Single test runner:** `Tools/test-all.ps1` runs xUnit suites and the LightControl console
  host. CI and pre-push use it, with deployment disabled. Reviewed zero-discovery and
  environment-override holes are fixed; xUnit TRX execution counters and the console result
  contract are checked. Final parent run: 2,956 xUnit tests plus 151 console checks passed.
- **Observed parent validation:** all 17 projects passed, none skipped; full solution
  `dotnet format --verify-no-changes --no-restore` passed. A forced Release rebuild with
  `S1NoDeploy=true` completed with 0 errors and 0 C# warnings; only the framework support
  warning below was emitted twice (restore/build). This is not gameplay acceptance.
- **Framework constraint (accepted, not suppressed):** `LightControl.Tests` deliberately targets
  `net6.0`, matching the MelonLoader runtime and the linked production core. The SDK's
  `NETSDK1138` end-of-support warning is retained. Moving just the test host to a newer
  runtime to silence this warning would weaken runtime parity; changing the game's mod
  runtime is outside repository maintenance. This warning is not a gameplay verification.
- [x] Nullable warnings reviewed and corrected in MoreSaveSlots, BusinessIncome and the
  Hash command shim shared with PotScanner. PotScanner's blanket suppression removed.
  BusinessIncome retains only a two-line CS8601 suppression documenting S1API's no-op
  delegate invariant; no replacement delegates or new lifecycle behavior. Invalid-null
  test inputs are explicitly marked. MoreSaveSlots removes a no-card call whose exception
  was swallowed locally; live-card behavior stays unchanged.
- **Documentation:** SDK/runtime/submodule guidance and native-save versus mod-sidecar paths
  reconciled; test commands now use the no-deploy runner.

### Earlier tooling results and retained limits

- [x] Deploy preflight and explicit third-party allowlist implemented and reviewed.
  Destination-override finding fixed and exercised through the actual MSBuild target against
  a fake installation; final live-install deployment/readback is recorded below.
- [x] Version-bump acceptance safety implemented; prior evidence stays historical, already-open
  rows retain their original evidence, and ambiguous labels abort before writes.
- [x] Durable tool regression suites: version 72, per-mod deploy 131, third-party/preflight
  deploy 64, skills health 21, runner 62: 350 assertions passed in the parent integrated
  run. CI discovers these suites; remote GitHub execution was not triggered.
- [x] Version sync: 21 mods, 0 drift; documentation paths, solution determinism, format and
  diff whitespace gates passed. Skills health: clean; historical version evidence is retained with narrow reason-bearing annotations.
- [x] Packaging disables deployment and includes GLB models in the mod data subfolder. Actual
  TaxiDriver package entries matched built DLLs, model and metadata; installed artifacts stayed
  unchanged. The package is a scratch verification artifact, not a published release.
- [x] Per-project hash receipts, attempt history, verified overwritten-file backups and bounded
  in-process rollback implemented. Corrupt backups, reparse targets, copy/receipt failures,
  partial rollback, stale-file reporting and no-op reruns are fixture-tested. Normal and
  third-party deployment share a per-install mutex.
- **Explicit limits:** no solution-wide/crash rollback, backup pruning or automatic stale-file
  deletion. Third-party deployment installs new DLLs or accepts identical content; differing
  installed DLLs require an explicit owner-managed upgrade and are never silently overwritten.
- [ ] Game-launch race after preflight remains: keep the game closed until deployment finishes.
- [ ] Complete the remaining WorldLook checks below with the owner. Other owner-accepted mods are not reopened by this tracker.
- **Working tree state:** `git status` and `git log` are the source of truth for what is committed; this record is not. Never assume the tree is clean or synced because an older line here said so, and never commit, push or rewrite history without explicit authorization.

### Guardrails

- Distinguish artifact identity (hash equality) from native correctness. A matching DLL hash
  proves what loaded, not that a gameplay path works.
- Read existing logs and save sidecars without modifying the game or saves.
- One session in an empty checkpoint state cannot close the full gameplay acceptance.

## Earlier technical handoff (historical)

The parent ran `Tools/build-all.ps1` against the closed game after the focused delta review
returned PASS. Release succeeded: 0 errors, only the retained NETSDK1138 warning (two SDK
emissions). Readback verified **22 projects / 78 artifacts**: 40 copied, 38 content-identical;
source SHA256, installed SHA256 and receipt SHA256 agreed for every artifact. Attempt
receipts matched latest receipts, retained backup hashes matched their manifests, and no
stale entries were reported. Third-party allowlist was empty: no third-party DLL changed.

Durable receipts and backups are in `<GameDir>/UserData/.deploy/`. This was an actual deployment,
not a native gameplay test. Native acceptance was deferred at that checkpoint; later owner
confirmations are reconciled in the current status section below.
No commit, staging, push, hook activation or remote CI run was performed. New tooling files,
including `ThirdParty/.deployallow`, must be included when a commit is explicitly authorized.
The pre-push hook remains opt-in; its syntax was checked, not activated.

Installed DLL fingerprints for the consolidated test round (**2026-10-19 snapshot — historical;
current versions live in the per-mod CHANGELOGs and the compatibility matrix**):

| Mod | Installed source version | SHA256 prefix |
|---|---|---|
| HitmanPhone | 0.3.0-dev.1 | `aad5679242acd335` |
| TaxiDriver | 0.8.19 | `608c8221a30fe362` |
| LightControl | 0.1.0-dev.13 | `329a34ae72cd330f` |
| PropertyLEDs | 0.1.7 | `5efe7f06dc0d8ad5` |
| PocketShop | 0.3.7 | `1d98bde22f2c2b9a` |
| Minimap | 2.3.4-dev.1 | `359c28f6ce0551a5` |
| WorldLook | 0.1.2 | `f68fe25784f12602` |
| MoreSaveSlots | 1.0.12 | `47be5ec9d7f52515` |
| BusinessIncome | 0.1.6 | `dca7673ae99044ba` |
| PotScanner | 0.7.0 | `6af0d6eee73df298` |

The earlier handoff suggested smoke tests for save-slot pagination/labels and BusinessIncome's
normal day rollover. Those maintenance edits had compile/review evidence; this historical
suggestion does not reopen the owner's feature-complete mod inventory.

## 2. Current native status

WorldLook is the only mod under active development. Owner confirmations close the other
listed work even when individual actions were not itemized. A broad owner confirmation is
recorded as such; it does not manufacture a log, artifact identity or a pass for each old test.

Multiplayer is outside the owner's scope (2026-10-24). Host-authority and MP-refusal guards
remain in code and remain unverified where noted; they are not open acceptance tasks.

### WorldLook 0.3.0-dev.2 — still open

- **Recorded evidence:** 1,167 water tests, full build 0/0, controlled GPU comparison and
  independent shader/fixture reviews PASS; DLL `410fecf7…` + bundle `29557b87…` match the
  recorded owner run. Morning motion was owner-confirmed calm ("ruhig"). These are inherited
  results, not tests rerun by this documentation change.
- **Canonical record:** [water-surface verification](../../Source/Mods/WorldLook/docs/water-surface-verification.md).
- [ ] Overall/direct-sun appearance (the morning pair showed no strong direct-sun glint).
- [ ] Positive warmth near the 06:00 or 18:00 peak; strength 0 after 07:30 is expected.
- [ ] ON after OFF, confirming water re-applies cleanly.
- [ ] Night look and menu/save/scene reload.
- [ ] Measured performance; no game frame-time/FPS measurement is recorded for this build.

Capture the tested artifact, state and scenario with each new result. The existing active F9
sample already proves one HHMM conversion and readable light channels; repeating it alone
would not resolve the remaining appearance, transition or performance questions.

### Owner-accepted — no current action

| Mod | Current status and limits | Detailed record |
| --- | --- | --- |
| S1MCP 1.0.5 | Clock contract owner-confirmed 2026-10-24. A later live sample read `812` as 08:12 / 492 min; a no-op `hhmm: 836` write returned 08:36 / 516 min with matching readback. This does not independently exercise every write form. | [Compatibility](../compatibility.md), `ThirdParty/S1MCPServer-master/S1MCPServer/CHANGELOG.md` |
| HitmanPhone 0.3.0 | Owner-confirmed 2026-10-24; individual actions not itemized. The 08:22:55 log additionally records a matching world checkpoint restore. Legacy alignment and unknown payment outcomes retain their documented limits. | [Audit repair](../../Source/Mods/HitmanPhone/docs/audit-repair-verification.md) |
| LightControl | 0.1.0 owner-confirmed 2026-10-24, including the previously missing interaction/lifecycle and font-parity acceptance; actions not itemized. 0.1.1 is the subsequent deployed log-only Rainbow-tick fix; no fresh native acceptance is inferred from that deployment. | [Graphite verification](../../Source/Mods/LightControl/docs/graphite-ui-verification.md), [changelog](../../Source/Mods/LightControl/docs/CHANGELOG.md) |
| PropertyLEDs 0.1.8 | Owner-confirmed 2026-10-24; location/status fitting, color/toggle and save roundtrip were not itemized. | [Implementation status](../../Source/Mods/PropertyLEDs/docs/implementation-status.md) |
| MessagesPlus 0.4.6 | Popup/response repair owner-confirmed 2026-10-15; the older order-menu checklist is historical. Original test date was not supplied. | [Changelog](../../Source/Mods/MessagesPlus/docs/CHANGELOG.md) |
| AutoPackagingStation 0.4.7 | Owner-confirmed tested session 2026-10-15. Legacy 0.4.1 cauldron-save migration remains unverified and is relevant only if that save format is actually needed. | [Interaction evidence](../../Source/Mods/AutoPackagingStation/docs/interaction-diagnostics.md) |
| TaxiDriver 1.0.0 | Version-only milestone of the owner-accepted 0.8.22 code; confirmed finished 2026-10-24 and subsequently observed loaded. Known road correction/jitter remains a limitation, not reopened work. | [Driver binding](../../Source/Mods/TaxiDriver/docs/driver-binding-verification.md) |
| Minimap 2.3.5 | Owner-confirmed finished 2026-10-24. Optional sewer-key search was cancelled; dormant controller/store code and its former experiments are not a backlog. | [Changelog](../../Source/Mods/Minimap/docs/CHANGELOG.md) |
| PocketShop 0.3.9 | Owner-confirmed directory/item-grid repair 2026-10-24; original purchase-path itemization remains historical. | [Changelog](../../Source/Mods/PocketShop/docs/CHANGELOG.md) |

Former per-action checklists are preserved in the [dated archive](agent-mod-history-20261024.md#preserved-maintenance-checklist-before-reconciliation).
They are evidence of earlier plans, not new requirements. The full inventory also records
accepted mods not repeated in this table.

## 3. Merged and retired graphics mods are not separate releases

- `GoldenHour` and `WaterShader` were merged into **WorldLook**; their code now lives in
  `Source/Mods/WorldLook/src/` and the modules ship inside `WorldLook.dll`. They are **not**
  separate active releases - do not package, deploy or list them independently.
- `GraphicsProbe` is **retired**: its source is archived under
  `Source/Archive/WorldLook/GraphicsProbe/`, its tests under `Source/Archive/Tests/GraphicsProbe.Tests/`,
  and its empty mirrored build-output folder under `Source/Tests/` was deleted on 2026-10-24.
  The "still open" probe items in older notes are not outstanding work.
- Only `WorldLook.dll` is installed; no standalone `WaterShader.dll`, `GoldenHour.dll` or
  `GraphicsProbe.dll` belongs in `Mods\`.
- Release notes and the compatibility matrix describe the merged WorldLook artifact only.

## 4. Next verification order

1. Finish and review the repository verification commands, then record their real local and
   isolated-snapshot results in the current work table above.
2. For WorldLook, use the existing candidate for one focused session covering strong sun,
   warm light, OFF/ON, night and reload. Preserve the scenario and artifact identity.
3. Measure game frame times under comparable ON/OFF conditions before proposing shader
   optimization; existing static renders do not establish game performance.

The owner-accepted mods and cancelled Minimap search feature are not prerequisites for this
work. No new gameplay pass or performance measurement is claimed by this tracker update.
