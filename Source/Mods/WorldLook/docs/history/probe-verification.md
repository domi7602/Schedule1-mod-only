# Initial implementation verification — GraphicsProbe 0.1.0 (historical)

This section records the isolated implementation gate, before parent integration and the
review-driven correction. It is not evidence that 0.1.0 was deployed or run in the game.
The final release gate is recorded separately below.

## Scope and environment

Implementation changes are confined to `Source/Mods/GraphicsProbe/**` and `Source/Tests/GraphicsProbe.Tests/**`. The initial version/author came from the required scaffold command:

```bash
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/new-mod.ps1 -Name GraphicsProbe -Author Dominik -Version 0.1.0
```

No solution regeneration, global docs/Shared/tooling/other-mod edits, commit, push, game launch/close or final deployment were performed. The Release gate below explicitly disables both deployment and project-reference builds. Existing framework references/Shared wiring are inherited unchanged.

All dotnet commands ran from `C:/Users/pc/Schedule1-mod-only` with:

```bash
export DOTNET_ROOT='C:/Users/pc/.dotnet'
export PATH="$DOTNET_ROOT:$PATH"
```

`dotnet --version` returned **8.0.425**. Mod and tests target **net6.0**.

## Observed RED → GREEN sequence

The following commands were actually executed before the corresponding production behavior/wiring was supplied. Initial absent-type REDs are explicitly compilation failures, not claims that Unity/native code ran. Later REDs include real policy assertion failures. Tests link actual runtime-used pure source files; no test references the mod/Unity assembly.

| Slice | Observed RED command/result | GREEN |
|---|---|---|
| Water evidence | `dotnet test Source/Tests/GraphicsProbe.Tests/GraphicsProbe.Tests.csproj -c Release --nologo` → exit 1 with water policy absent | Same command exit 0 after implementing evidence policy |
| Scheduling | Same full test command → exit 1 with `CaptureSchedule` absent | Same command exit 0 after delayed/coalesced/cooldown/shutdown/retry policy |
| Bounded report/finite numbers | Same full test command → exit 1 with bounded section/report policy absent | Same command exit 0 after implementation; a test initially queried PascalCase keys despite the camelCase schema and was corrected to camelCase before GREEN |
| Unicode text cap | `dotnet test Source/Tests/GraphicsProbe.Tests/GraphicsProbe.Tests.csproj -c Release --nologo --filter TextCapDoesNotSplitUnicodeSurrogatePairs` → exit 1, assertion failure against the existing cut behavior | Full test command exit 0 after surrogate-safe truncation |
| Field isolation | Full test command → exit 1 with `ObservedRecord` absent | Full test command exit 0 after guarded per-field reads |
| Runtime wiring (supplementary only) | `dotnet test Source/Tests/GraphicsProbe.Tests/GraphicsProbe.Tests.csproj -c Release --nologo --filter RuntimeWiringTests` → exit 1 before scene/object/environment readers and lifecycle wiring existed | Full test command exit 0 after implementation |
| Explicit partial-discovery/material-limit reporting (supplementary only) | `dotnet test Source/Tests/GraphicsProbe.Tests/GraphicsProbe.Tests.csproj -c Release --nologo --filter PartialDiscoveryAndMaterialCapsAreExplicitlyMarked` → exit 1, assertion failure | Full test command exit 0 after marking these incomplete paths |
| Reject seat/research false positives | `dotnet test Source/Tests/GraphicsProbe.Tests/GraphicsProbe.Tests.csproj -c Release --nologo --filter SeaClueDoesNotMatchSeatOrResearch` → exit 1, assertion failures for `sea` inside ordinary names | Full test command exit 0 after making the `sea` clue token-bounded; standalone/path `sea` still qualifies |
| Existing runtime pipeline type (supplementary only) | `dotnet test Source/Tests/GraphicsProbe.Tests/GraphicsProbe.Tests.csproj -c Release --nologo --filter PipelineTypeReadsExistingRuntimeFieldWithoutRendererResolution` → exit 1, missing `RenderPipelineManager.s_CurrentPipeline` assertion | Full test command exit 0 after using the verified existing native static field |

Build feedback also caught an overly general native-type helper and then the repository's explicit `il2cpp` mscorlib alias (`CS0234` when naming `Il2CppSystem.Object` without the alias). The helper now uses `extern alias il2cpp`; the final live-interop compile has no warnings/errors. Neither build issue was worked around by changing Shared/global build files.

## Final executed gates

```bash
dotnet test Source/Tests/GraphicsProbe.Tests/GraphicsProbe.Tests.csproj -c Release --nologo --logger 'trx;LogFileName=GraphicsProbe-final.trx' --results-directory Source/Tests/GraphicsProbe.Tests/TestResults
```

**Exit 0. Passed 31, failed 0, skipped 0.** The TRX was parsed independently: 31 enumerated result records, all `Passed`; counters total/executed/passed are all 31, failed/error/notExecuted are 0.

- 25 linked runtime-policy tests: WaterHeuristics 13, CaptureSchedule 5, ReportPolicy 6, ObservedRecord 1.
- 6 supplementary RuntimeWiring source-contract tests. These check known forbidden API/wiring patterns, not semantic/native execution safety.
- Local generated evidence: `Source/Tests/GraphicsProbe.Tests/TestResults/GraphicsProbe-final.trx` (ignored build/test artifact, not a fake runtime snapshot).

```bash
dotnet build Source/Mods/GraphicsProbe/src/GraphicsProbe.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false --nologo
```

**Exit 0. Build succeeded; 0 warnings, 0 errors.** Output:

```text
Source/Mods/GraphicsProbe/src/bin/Release/net6.0/GraphicsProbe.dll
```

Metadata JSON was parsed successfully with version `0.1.0`; owned-tree `git diff --check` returned exit 0. New files are untracked; no commit was created.

The final DLL was also inspected with `ilspycmd.exe -il`: exit 0; a direct-call pattern scan found **0** Unity setters/object creation/destruction/global object searches/material-instancing getters/lazy profile or renderer accessors/renderer-resolution calls/RenderTexture constructors. IL evidence is in Hermes scratch (`graphicsprobe-api/GraphicsProbe.final.il`), not mod assets. This supplementary pattern scan is not proof that every native getter is side-effect-free.

## Review targets / remaining acceptance

- `RenderingSnapshot.cs`: bounded scene-root traversal, inactive descendants, explicit special-scene limitation, cooperative deadline and node/depth/component caps. Counts are of a discovered subset. No global Resources or whole-world component search.
- `ObjectReader.cs` / `EnvironmentReader.cs`: stale-getter isolation, shared references, finite scalar serialization, native type identity and serialized URP fields. Audit against the installed interop DLLs; no lazy renderer construction/resolution, instanced Volume profile, material instantiation or GPU allocations.
- `Mod.cs`: selected-input focus guard, low-collision chord, finally-completed scheduler and SafeStorage/readback behavior. No per-frame discovery unless a coalesced capture is due.
- Parent still owns independent review, inventory/solution/global docs wiring and any final build/deployment/hash checks.
- **In-Game-Verify OPEN:** no actual `latest.json` was generated, no Unity integration or scene traversal executed, and no timing/IO failure/lifecycle acceptance was claimed. A synchronous capture can hitch; managed catches cannot contain process-fatal native access violations. Persistent DontDestroyOnLoad objects may be out of scope. Candidate names do not prove water, gameplay camera selection or GPU depth-buffer availability.

## Review correction — 0.1.1

The initial independent review rejected 0.1.0 before deployment: an exception in the
outer scene discovery provider retained `fieldErrors.scenes` but bypassed discovery
error accounting. An empty report could therefore be labeled untruncated. A separate
fix context added an error callback to the production-used `ObservedRecord.Read`
boundary and wired top-level scene failures into the existing bounded accounting.
The startup/report version is now derived from Melon identity, not duplicate literals.
The version was changed with `Tools/bump-version.ps1`, not by editing MelonInfo.

Observed correction REDs (fix context):

- `dotnet test Source/Tests/GraphicsProbe.Tests/GraphicsProbe.Tests.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false --no-restore --filter FullyQualifiedName~ObservedRecordTests`:
  exit 1 with two CS1501 missing-overload compilation errors, **not assertion RED**;
  then 3/3 pass after the linked helper change.
- The same base command with
  `--filter 'FullyQualifiedName~TopLevelSceneFailureFeedsBoundedDiscoveryErrors|FullyQualifiedName~LogAndReportVersionComeFromMelonIdentity'`:
  two assertion failures before wiring; 2/2 pass after wiring.

Parent independently reran the complete suite with deployment and dependency builds
disabled and parsed its TRX: **35 passed, 0 failed, 0 skipped** (27 linked policy
cases, 8 supplementary source contracts). The Release compile-only gate returned
**0 warnings/errors**. Full scoped `dotnet format --verify-no-changes --no-restore`
passed for both projects from their own directories. Initial scaffold CRLF in Mod.cs
was normalized to LF; byte reconstruction against the original hash confirmed that
normalization had no other content change.

The nonblocking suggestion to replace source `Contains` checks with syntax/IL
infrastructure is deferred. Those checks are explicitly supplementary; direct IL
inspection is a separate static gate and never a claim of native-runtime safety.
A delta re-review and final deployment are recorded in the release gate below;
these tests do not establish that the real Unity exception path was exercised.

## Final reviewed build and deployment — 2026-10-15

- Independent five-file delta review: **PASS**, no security concerns, logic errors or
  remaining suggestions in that delta. The reviewer separately reproduced 35 passing
  tests and the clean deployment-disabled Release build.
- Parent confirmed the game was closed, then built with:
  `dotnet build Source/Mods/GraphicsProbe/src/GraphicsProbe.csproj -c Release -p:S1NoDeploy=false -p:BuildProjectReferences=false --no-restore --nologo`.
  Actual output included `Deployed GraphicsProbe` and **0 warnings/errors**. Dependency
  rebuilding was disabled; the existing built/installed Shared hash already matched.
- Built and installed DLL SHA-256 match:
  `03bfc79e90891f455f947676cbd7253b9be892e3598d7f575213856d74ae66a2`.
- Built and installed PDB SHA-256 match:
  `78510ad96f1a8457bb8a95dc4c5d07f6e721901e73d799d92d7a82c681a8f4ca`.
- Source and installed metadata SHA-256 match:
  `d8bd5a8bb2261d9e65703f20828d2d14f27d95f43e16849de8a5fdc726c1d105`.
  Installed metadata reads version `0.1.1`, verification `open`.
- Exact installed locations: `Mods/GraphicsProbe.dll`,
  `UserData/GraphicsProbe/GraphicsProbe.pdb`, `UserData/GraphicsProbe/mod.json` beneath
  the game directory. Installed assembly metadata contains Mod, RenderingSnapshot,
  ObjectReader, EnvironmentReader, CaptureSchedule and ObservedRecord types.
- All **41** previously hashed installed DLL/PDB/metadata files are unchanged relative
  to the pre-deployment baseline. The Shared baseline suite passed **72/72** separately.
- Parent inspected the final DLL IL: zero known forbidden direct Unity setter,
  creation/destruction, instancing/lazy-renderer/profile or render-target constructor
  call patterns matched. This static pattern audit is **not native-execution evidence**.
- Solution regeneration is deterministic and adds only GraphicsProbe and its test
  project. The pure suite is included in CI without requiring game assemblies.

**Runtime gate remains OPEN.** The game was not started and no actual `latest.json`
exists at this gate. Load a save via Steam and request the manual capture near water;
inspect fresh timestamps, pipeline/camera observations, errors/truncation and backup
behavior before treating any candidate or native lifecycle path as verified. No
commit, push, tag or rendering change was performed.

## First real-game report — 2026-10-16 (partial acceptance)

Owner launched the game; the assistant inspected the actual log and both persisted
reports. This supersedes the absence of runtime evidence at the historical deployment
gate, but does **not** close overall In-Game-Verify.

- Captured runtime: Schedule I **0.4.7f12**, Unity **2022.3.62f2**, MelonLoader
  **0.7.3**. Latest.log records GraphicsProbe **0.1.1**, and its loaded DLL SHA-256
  equals the installed/reviewed DLL hash above.
- Startup logged at 04:15:33 local; successful captures logged at 04:15:40 and
  04:15:45 (one camera each, discovery not truncated). Gameplay capture at
  04:16:16 and subsequent manual captures at 04:16:30 and 04:17:34 were truncated.
  Earlier report contents were already replaced; their logged counts do not establish
  gameplay-camera identity or actual water.
- `latest.json`: `capturedAtUtc=2026-10-16T02:17:33.3572193Z`, reason `manual`,
  version `0.1.1`, SHA-256
  `8c0d6bb4341c9dbed4de06d4553dd6a8477d7e8acf1abca5a37d19aaf55d8616`.
  Backup: `2026-10-16T02:16:29.7770666Z`, reason `manual`, SHA-256
  `3b07731e44070da0727e64426560aa1fd43dfd1a4d2e84db6261daf378124c03`.
  Both parse as JSON; successful capture breadcrumbs occur after the production
  exact-file readback. Repeated manual capture and preservation of the prior report
  are observed; input suppression and exceptional IO paths remain untested.
- Active native pipeline is **UniversalRenderPipeline** and current/quality asset is
  **URP_3Ultra** (default asset URP_2High), Direct3D11, Linear color space, Ultra,
  1920x1080. The asset requests depth/opaque textures and supports HDR. These are
  configuration observations, **not proof of actual camera buffers or executed passes**.
  A renderer-data asset named Deferred Renderer is not an executed-mode observation.
- **Blocking coverage limitation:** both retained gameplay reports stop at 50,000
  visited nodes (`node-limit`, `discoveryTruncated=true`), with only 2/31 roots of
  Main scanned and 1/2 scenes scanned. Zero cameras were discovered in this subset;
  this does not mean the game has no cameras. Renderer heuristic scans reach 20,000.
- The 128 retained water candidates in each report are ParticleSystemRenderers:
  64 Vomit particles and 64 Blood Mist. A retained sample matches
  `particles_water_mat` under the NPC hierarchy, not a water surface. No actual
  water renderer/material or gameplay camera is confirmed by these reports.
- Latest collection took **1041.301 ms**, backup **1081.7957 ms**; serialization/IO
  are excluded. Main-thread collection duration is a hitch risk, not an observed FPS
  or owner-confirmed perceptual result. Both reports contain zero discovery errors
  and no `fieldErrors`; that does not repair missing coverage.

Next diagnostic gate: prevent a large early hierarchy from exhausting all discovery
budget before other roots/cameras, and prevent particle-name false positives from
crowding out surface candidates. Preserve bounded work, inactive-object coverage and
read-only guarantees; do not merely raise caps or treat another identical capture as
proof. No source/DLL/game/render state was changed during this runtime inspection.
Camera/water coverage, focus/input suppression, failure retry, shutdown cancellation
and performance acceptance remain open; no water prototype is authorized by these
partial findings alone.

## Coverage repair — 0.1.2 (2026-10-16)

This gate is separate from the native **0.1.1** evidence above. The repair changes
only diagnostic discovery/reporting; no water prototype or rendering mutation is
included. The game was confirmed closed via `tasklist.exe`. Before replacement,
0.1.1 DLL/PDB/metadata and both existing reports were copied and readback-hash-checked
under `UserData/GraphicsProbe/BuildBackups/0.1.1-20261016-051748/`.

### Executable regression evidence

The implementation worker ran an assertion regression against an extracted unchanged
old depth-first policy: a deep first NPC root starved the late camera/water root and
second-scene root. It failed before the fair traversal, then passed. An exact-node-cap
natural-completion assertion also failed before correction (a finished leaf must not
invent a `node-limit` omission). The worker's initial cooperative/controller and
category-budget failures were **missing-type compilation failures**, not behavior
assertion REDs. Parent inspected the completed worker trace and independently reran
all current tests; native Unity execution is not represented by these pure graphs.

The final runtime-used helpers are linked directly into the test assembly:

- `FairTraversal<T>`: admitted roots first, scene-round-robin visits and one-child
  parent rotation within breadth levels; node/depth/child/root/frontier caps,
  identity deduplication, stale-getter isolation/counting, release of pending graph
  references and exact node-cap natural completion.
- `CooperativeCapture`: 2 ms cooperative slices / 128 steps per frame, active and wall
  budgets, a slow native-like callback overrun without a second call in that slice,
  completion/failure/cancellation/release, no idle work, queued request coalescing and
  cancel/reschedule without overlap. Synthetic callbacks are not native timing tests.
- `RendererScanBudget`: a particle flood before a later mesh cannot consume surface
  scan/retention allowance; explicit renderer categories, encountered counts and
  independent scan omissions. Scan allowances 19,488 + 512 retain the prior total
  maximum 20,000; retained matches are 128 surface-like and 32 particle/line/trail.
- Source contracts additionally constrain capture/lifecycle wiring, tagged-camera
  provenance/deduplication, safe read-only APIs, release and timing/report fields.
  They are supplementary, not execution of the Unity adapter or MelonLoader hooks.

Parent independently observed:

```bash
dotnet test Source/Tests/GraphicsProbe.Tests/GraphicsProbe.Tests.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false --no-restore --nologo
# Passed 56, Failed 0, Skipped 0

dotnet build Source/Mods/GraphicsProbe/src/GraphicsProbe.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false --no-restore --nologo
# Build succeeded; 0 warnings, 0 errors

# Each command executed from its respective project directory:
dotnet format GraphicsProbe.csproj --verify-no-changes --no-restore --verbosity minimal
dotnet format GraphicsProbe.Tests.csproj --verify-no-changes --no-restore --verbosity minimal
# Both exited 0
```

The installed Camera.main getter was inspected with ILSpy; installed interop compile
checks required `Il2CppArrayBase<Component>` for the per-object component array.
Initialization/environment/camera reads occur inside timed steps, not constructors.
Collection reports schema 2 start/end times, active/wall/max-step/frame/step measures,
root admission/visits/known omissions and pending work at release. Synchronous
serialization/write/readback cost and total wall time are logged separately. Native
calls may exceed targets; neither unit tests nor a budget guarantee absence of hitches.

A scoped static security-pattern pass over all 20 current `.cs`/`.csproj` source/test
files found no matches in the hardcoded-secret, shell-execution, eval/exec, pickle or
SQL-interpolation checks. This is a narrow pattern pass, not a comprehensive security
proof. Exact actual-source hashes were supplied for independent review, including
untracked files (a Git diff alone would omit them).

Version was advanced with `Tools/bump-version.ps1 -Mod GraphicsProbe -Version 0.1.2`;
no hand-edited assembly version. An attempted `check-version-sync.ps1 -Mod` invocation
was rejected because that guard has no Mod parameter; it made no changes. The corrected
no-switch invocation passed for all 20 active mods with zero drift. Parent doc-path
and scoped Git whitespace checks passed. A fresh post-bump test run again passed
56/56 and a post-bump compile-only build returned zero warnings/errors. Independent
review and final deployment/readback gate are recorded below.

### Deployment record — 0.1.2 (2026-10-16, deploy verified, In-Game-Verify open)

Delta re-review verdict **PASS**: both P2 defects resolved in the final source, no new
P1/P2 introduced, no security findings; all 22 reviewed file hashes recomputed and
matched the on-disk sources. Deployed build (0 warnings/0 errors) verified by readback:
installed DLL SHA-256 `390237feefc3b93016601430a991a52a6254d6344b2e6715944aac0d70cf3a15`
(104,448 bytes) matches the built artifact; PDB and mod.json hashes match their build
outputs; installed identity reads `[assembly: MelonInfo(typeof(Mod), "GraphicsProbe",
"0.1.2", "Dominik", null)]`. Rollback backup 0.1.1-20261016-051748 hash-verified
unchanged; prior latest.json/.bak untouched; 61 unowned installed files and 142 unrelated
dirty files unchanged. Post-deploy suite: **59/59**. Two non-blocking review suggestions
are deferred to a later revision: assert the node-visit error-routing prefix in the
component-length regression, and separate failed scene-handle rows from the SeedScene
"kept" count (pre-existing, outside the fix delta).

### Independent review checkpoint — rejected before deployment

The independent reviewer reproduced both P2 findings against the actual collector
method bodies compiled with managed adapters (not native Unity):

1. A failed component-array `Length` getter left the previous node's component bound
   installed. A three-component predecessor caused three spurious index reads/errors
   after the next array failed acquisition. Publish a component batch only after its
   bounds are read; clear all cursor state before acquisition.
2. A failed scene-count read retained top-level null/error evidence but incorrectly
   rendered the scene subsection as known `totalObserved=0, truncated=false`. Preserve
   unavailable/unknown scene count separately from a successful zero-scene observation.

The first review verdict was **FAIL** despite 56 green policy/source tests and a clean
compile. No security issues were identified. The two corrections and three executable
managed-adapter tests now pass; the final independent delta review passed (see the
deployment record below). Parent read
the actual saved RED logs: component index reads expected 0, actual 3; unknown scene
total expected Null, actual Number. After correction, the first regression verifies
zero failed-batch indices, one error and later-root camera discovery. The second
verifies unavailable/null/truncated scene reporting; the third distinguishes successful
zero-scene enumeration (available/0/not truncated). Full suite independently rerun:
**59 passed, 0 failed, 0 skipped**; production compile-only Release: **0 warnings,
0 errors**; both scoped format checks: exit 0. These execute the actual linked
RenderingSnapshot/controller/report code against reset, nonparallel TEST-ONLY managed
Unity/array adapters, not real Unity or the real ObjectReader field getters.
A final 22-file actual-source/hash snapshot and narrow security scan (zero pattern
matches) supersede the earlier 20-file review input. The reviewer also
noted that scene admission itself is sequential: per-scene quotas protect capacity,
not admission-time fairness. Only **visits of already-admitted roots** are scene-round-
robin. Early budget expiration during seeding must remain explicitly partial.

### Native acceptance still open for 0.1.2

No repaired DLL has been exercised in a game session. Native root/camera/surface
coverage, cancellation callbacks, focus/input handling, report/backup/failure behavior
and actual active/slice/finalization timings require a fresh 0.1.2 session. A Camera.main
tagged-enabled hint does not prove gameplay-camera identity; a non-particle lexical
match does not prove water, and depth/opaque requests do not prove usable buffers.
Launch normally via Steam, load the save, settle near water, trigger Ctrl+Shift+F8,
and allow up to 35 seconds. Inspect fresh version/schema/times and final log status,
not merely existence of the old JSON. Overall **In-Game-Verify remains OPEN**.

## First 0.1.2 game session — 2026-10-16 (core acceptance observed)

This supersedes "no repaired DLL has been exercised" above. Owner launched via Steam
(07:56 local startup) and later closed the process via Task Manager **End Task** (an
ungraceful kill; no capture was in flight). Pre-flight: game process absent
(`tasklist`), built and installed DLL SHA-256 matched at
`390237feefc3b93016601430a991a52a6254d6344b2e6715944aac0d70cf3a15`, installed Shared
hash matched its build output. Latest.log records **GraphicsProbe v0.1.2** initialization
at 07:56:27 local.

- Capture lifecycle observed: two scene-transition captures logged
  `Capture cancelled/discarded; previous report retained` (07:56:38, 07:57:17),
  one automatic `completed` capture (07:56:45), one automatic capture completed under
  `wall-deadline` (07:57:53), and the manual Ctrl+Shift+F8 capture `completed`
  (07:58:18). No error loop or stale-object repetition followed until the kill.
- Fresh manual report `latest.json`: SHA-256
  `c92f464319dfcd504c0d4b0741196141f6c370beaea84bb671293f55339cb82e`, modVersion
  `0.1.2`, schema 2, reason `manual`, `captureStartedAtUtc=2026-10-16T05:57:58.4383711Z`,
  `captureEndedAtUtc=2026-10-16T05:58:18.9583329Z`. Backup `latest.json.bak` (SHA-256
  `6408d1704dd99f088c9919ca0316f3be62561c297862155bacc5d9852b2e2832`) preserves the
  earlier `scene-initialized` capture (05:57:23–05:57:53Z). Prior-report backup is
  therefore observed in this session; the 4 MiB oversize refusal and IO-failure retry
  were not exercised. No `latest.json.tmp` survived the hard kill; atomic writes held.
- Coverage repair is effective in native execution: **8 cameras discovered**
  (0 for 0.1.1), including the `Player_Local/CameraContainer/Camera` object via the
  labeled Camera.main hint (deduplicated: `cameraDuplicateIdentities=1`) and the
  disabled `@EnvironmentFX/Volumetric Fog Volume/SurfaceCam` holding a 512x512,
  32-depth-bit `Format=Depth` RenderTexture. This is a target-texture observation,
  not proof of a usable scene depth buffer. Cameras also included ThumbnailCamera,
  PerformanceCapture, Intro and two disabled cutscene cameras.
- Both scenes enumerated (`Main`, `MovedObjectsHolder`); manual report admitted and
  visited 32/32 Main roots with 11,995 explicit frontier child-edge omissions and
  `pendingCursorsAtRelease=3642` after `node-limit` at 50,000 nodes. One bounded
  discovery error sample: an IL2CPP `NullReferenceException` during child enumeration
  (`errors=1`, not truncated). The earlier `scene-initialized` report admitted 67 roots.
- Surface/effect separation works: manual report scanned 15,184 surface-like and 395
  particle/line/trail renderers with zero scan-limit omissions; 214 lexical matches,
  128 kept (cap) with 86 limit-omissions. 125 of 128 retained candidates are
  `WaterBottle_Trash` name false positives. The substantive candidate is
  `Map[0]/StylizedWater2_Ocean[2]`: MeshRenderer, active, layer 4, material
  `StylizedWater2_Ocean - Copy`, shader **`Stylized Water 2/Standard (Tessellation)`**,
  renderQueue 2955, components `StylizedWater2.WaterObject` and
  `ScheduleOne.Map.WaterCollider`, combined mesh 44,770 vertices / 24 submeshes,
  3114x3114 units. Remaining matches: `SM_Bld_WaterTower_01` (name FP) and a Sewer
  hallway (`canal` material-name FP). Lexical identity remains unconfirmed water by
  policy; parent `Map` carries `FishNet.Object.NetworkObject` (network-safety relevant
  for any future slice).
- Timing recorded honestly: manual capture active work 1074.9 ms across 20.5 s wall,
  205,264 steps / 1,613 frames, max observed step 16.3 ms, finalization 7.8 ms. The
  `wall-deadline` capture logged 27.4 ms max step. Owner observation: rendering and
  controls unchanged; gameplay remained smooth during captures (owner judgment, not a
  GPU measurement).

Remaining open for 0.1.2: graceful shutdown with a capture in flight (the kill had
none pending), the second post-cooldown capture and uGUI/TMP input-suppression check,
the menu round-trip (return + reload coalescing/cancellation), and retry after IO
failure. These are edge lifecycle paths; the core discovery/report acceptance above
is observed on the actual 0.1.2 build. Overall In-Game-Verify for 0.1.2 core: closed;
edge checklist: open. No water prototype is authorized by this diagnostic alone; the
next slice is selected from this report per graphics-probe-plan.md.

## 0.1.3 pre-deploy independent review — 2026-10-16

Reviewer: independent delegated review agent (scoped review of the uncommitted 0.1.3
delta, verification-gated: it ran the test suite and a fresh Release build against the
real interop DLLs itself). Verdict: **PASS-with-nits**, no P1. Categories cleared with
zero findings: Unity 2022 IL2CPP API usage (cross-checked against GameReferences,
including TimeManager's static `GetMinutesToDisplayTime`), closure capture/iteration,
JSON validity, render/scene/save mutation risk, schema-2 report consistency.

Findings fixed before deployment:

- P2 — report-critical wiring was pinned by zero tests: two source-contract Facts added
  (`MaterialPropertyIntrospectionIsCappedGatedAndErrorIsolated`,
  `SunAndInGameTimeEvidenceIsGuardedBoundedAndCoherent`), including guard-before-read
  ordering for the TimeManager block.
- P3 — per-entry error isolation degraded inside shader-property enumeration (one bad
  property would null the whole list): name/kind now go through `ObservedRecord.Read`,
  so a bad entry keeps its `fieldErrors` and the list survives.
- P3 — `NormalizeComponents` doc overstated JSON responsibility (the report's finite
  converters own JSON validity; tuple rejection is a semantic choice): reworded.
- P3 — the 64-property cap was unpinned: `Assert.Equal(64, ...)` plus a bounded range.
- Nits — `displayTime` wrapped in `ReportPolicy.Text`; `sun`/`currentTime` captured
  into locals for read coherence; degenerate `PlanIndices` inputs and
  `NormalizeComponents(null)` pinned by tests.

Residual (documented, unchanged): `LightDetails` intensity/range/spotAngle stay raw
floats guarded at the converter layer — pre-existing convention shared with the whole
report. Suites after fixes: GraphicsProbe.Tests 80/80 green; Release build 0 warnings /
0 errors. Deploy gate: PASS delta re-review required next (repo rule).

## 0.1.3 delta re-review — 2026-10-16 (deploy gate cleared)

Reviewer: independent delegated review agent (fix-delta scope; read all touched files in
full, ran both gates itself). Verdict: **PASS**. All five prior findings confirmed FIXED
with line-level evidence; the lambda-capture restructure was analyzed for compile
validity, per-iteration closure state, synchronous Read invocation and both failure paths
(name-read fails / kind-read fails — entry survives with `fieldErrors`, list survives).
No new issues in the previously cleared categories. Suites at the final stand:
GraphicsProbe.Tests **80/80** green; Release build **0 warnings / 0 errors**; deployed
build carries the fixes.

New non-blocking nits (documented, deferred):
- N1 — when the property-name read fails, the value getter still runs with an empty
  property name and can record a Unity default next to `name: null`; `fieldErrors`
  flags it. Skipping the value read on empty name would be cleaner.
- N2 — default-branch `valueNote` says "unknown property kind" even when
  `GetPropertyType` threw (kind unreadable rather than unknown); `fieldErrors`
  disambiguates.
- N3 — the `Assert.InRange` upper bound is decorative; `Assert.Equal(64, ...)` is the
  real pin.

Evidence session next: launch the game, two manual captures (Ctrl+Shift+F8, text fields
closed, ~35 s wait) at different in-game times, then verify SkyMat shader properties,
sun details and the time anchor in `UserData/GraphicsProbe/latest.json`. In-Game-Verify
for 0.1.3 remains OPEN until that session.

## 0.1.3 in-game evidence session — 2026-10-16 (evidence slice verified)

Two manual captures on the deployed 0.1.3 build (hash `7ad7d181…`), ~2 minutes apart,
reported via `UserData/GraphicsProbe/latest.json` (capture 2) and its `.bak` (capture 1):

- **Time anchor**: capture 1 at in-game `15h 34m` (currentTime 934, Tuesday,
  elapsedDays 1), capture 2 at `23h 41m` (currentTime 1421) — two clearly different
  in-game times, the intended A/B evidence pair. `NetworkGuard.IsAlive` gate held
  (no null/garbage time fields).
- **SkyMat shader properties**: both captures report exactly 12 properties,
  `truncated: false` — `_SunSizeModifier` 0.15, `_SunTexture` Sky_Sun (Tex2D),
  `_SunShine` HDR ~16.95 white, `_MoonSizeModifier` 0.15, `_MoonTexture` Sky_moon,
  `_MoonShine` 8/8/8/1, `_StarShine` HDR ~44.7, `_StarFadeOutStart/End` 0.074/0.277,
  `_StarFadeInStart/End` 0.770/0.961, `_FogInfluence` 1. Identical across both
  captures — consistent with material properties being static while the day/night
  cycle is driven by sun/volume state.
- **Sun details**: Directional, shadows Soft, `shadowStrength` 0.95, color constant
  white (1,1,1) in both captures; intensity moved 2.45 → 2.58 between the times.
  Observation for the golden-hour plan: the sun color does NOT track time of day in
  these samples — golden tint must come from volume grading (ColorAdjustments /
  ShadowsMidtonesHighlights / SplitToning) and/or SkyMat `_SunShine`, exactly as the
  plan's "grading before shader rebuild" route assumed.
- Capture quality: capture 1 7/7 cameras, capture 2 8/8 cameras; node limit reached
  (50,000, `discoveryTruncated: true`) as documented — expected behavior, not a defect.

Verdict: all three 0.1.3 evidence features work on the live runtime. **0.1.3
In-Game-Verify (evidence slice): CLOSED 2026-10-16.** Remaining 0.1.2 edge checklist
(graceful shutdown with pending capture, post-cooldown repeat + input suppression,
menu round-trip, IO-failure retry) stays open.
