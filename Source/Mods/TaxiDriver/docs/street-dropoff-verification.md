# Street-side drop-off candidate selection — 2026-10-31

## Scope and artifact boundary

This continuation investigates the **destination point selected for passenger rides**,
not a new pathfinding engine. During source verification the installed and last observed loaded DLL was
`fb31c4e7881fce86b6e93e0de8a3e8986d9bbe449732065eae495c7b6a46d278`, version
**1.0.5**. That phase did not authorize installation or new game execution. The owner
subsequently approved installing the exact reviewed artifact; see the installation
record below. No new loader/gameplay result is implied by the copy.

The initial working tree is dirty on `main`, based on
`94a42c5b41abc9e403793de238a75688726681db`. Before edits, 117 TaxiDriver source,
test and local-documentation files were copied and hash-checked under
`.opencode/taxi-endpoints-20261031-042454/baseline/`; the adjacent
`baseline-manifest.json` identifies them. Parent baseline: **795/795 tests**, no
failures/skips, focused Release build with `-p:S1NoDeploy=true` **0 warnings/errors**,
SDK 8.0.426, game closed. Baseline Quick is **5 steps passed / 2 failed**, only
pre-existing global `format` and `versions` drift. This is not a whole-tree green claim.

## Reproduction evidence, not a new acceptance

The [settings verification record](navigation-settings-verification.md#owner-run-session-inspected--2026-10-31)
identifies the owner-run 04:06–04:14 session and complete log hash
`d17e5f0d6207a39a3997a7eb48a4fbfa0da48d79cde48a7b791d4837274d0b5e`.

- **Barn:** General Vehicle Graph sampling selected `(172.7, 0.0, -13.8)`, while
  the successful smoothed path ended at `(154.0, 0.0, 18.6)`. Native `Complete`
  did not establish arrival; the taxi halted 37.1 m from its supervised target.
- **Bungalow:** the second attempt's lot fallback selected `(-166.1, -4.0, 109.3)`,
  while its successful path ended at `(-153.0, -4.0, 109.3)`. The final 12.2 m
  residual refers to that fallback, not the original direct destination.
- **Docks Warehouse:** the general-graph sample `(-85.9, -2.2, -56.3)` failed
  complete calculation after successful road segments. A sampled point, parking
  entry or road-seeker success is not a full-route proof.

The nearest Road Nodes point for each current world destination has **not** been
measured in a new game session. Captured coordinates are regression examples, not
hard-coded replacements or evidence that a particular new curb point is reachable.

## Why no independent asynchronous preflight was added

Read-only inspection of the installed IL2CPP assembly confirmed this callable API:

```text
NavigationUtility.CalculatePath(startPosition, destination, navSettings, flags,
    generalSeeker, roadSeeker, callback) -> UnityEngine.Coroutine
callback(result: Success|Failed, path: PathSmoothingUtility.SmoothedPath)
```

Input `Assembly-CSharp.dll` SHA-256:
`346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8`.
The independent research report is retained locally at
`.opencode/taxi-endpoints-native-20261031/findings.md`, SHA-256
`92e102384c9b0a1b1c2f4668bc9bfce673b164ee8a5a7d26715d797549ba1863`.
Its generated type dumps identify the installed signatures, not native method bodies.

- The helper exposes no cancellation/owner contract. Invalidating a managed token
  does not prove an old native query stopped borrowing the same stateful seekers.
- `Navigate` recalculates; it does not accept a precomputed path. The native-private
  calculation callback must not be treated as a public path-installation API.
- The native calculation callback has no request/order ID. Diagnostics label it
  with the current order; that does not independently establish its originating order.
- Existing `RequestPark` does not hold an active navigation order: the actuator
  consumer releases park intent when the order is active. A held-calculation design
  would require additional actuator ownership, supervision gates and native-flight
  draining evidence, not merely another observer.

Consequently the first candidate is deliberately **selection-only**: one normal
native navigation calculation, no extra preflight coroutine, borrowed-seeker lease,
new actuator mode, native path injection or automatic acceptance of arbitrary path ends.
The exact native `endAtRoad` trimming and cancellation implementations remain unknown.

## Bounded change contract

- Select a finite, walkable point from the **exact, unambiguous Road Nodes graph**
  within the existing **60 m destination-selection envelope**. No name-contains-road
  fallback, General Vehicle Graph projection or raw parking-entry reachability claim.
- Preserve that selected point on direct passenger dispatch instead of applying a
  second general-graph transformation. Keep ordinary pickup selection separate.
- Project alternate passenger lot candidates to strict street points before checking
  distance and duplicate identity; a selected passenger goal must not steer a pickup.
- Keep logical goal, resolver point and active physical point distinct. A refused
  selection must not publish a new destination label over an older active route.
- Retain fresh `true/true/false` navigation settings, native driving, the **10 m plus
  confirmed-halt/current-operation arrival rule**, and all finite recovery limits.
  Failed or mismatched native routes remain failures, not post-hoc endpoint rebinding.
- Preserve successful registered-patrol ownership/release behavior. A change to its
  selected candidate is not evidence that the native multi-waypoint route is repaired.

## First candidate and initial verification — superseded

- `RoadTarget.TryStreetDropOff` checks the exact unique graph, returned graph membership
  and walkability, finite values and the inclusive 60 m **3D** budget. Pickup's existing
  `FindNearPlayer`/diagnostic lookup is unchanged. No real nearest point was measured.
- `ResolveArrival` and STAND/legacy/direct-arrival suggestions use that strict selection.
  Direct passenger `Go` explicitly preserves the resolver point; it cannot opt into
  target preservation for pickup or a different resolver identity. Native-patrol
  rebounds/handoffs retain their prior behavior outside this strict-direct claim.
- Alternate lot entries are projected before goal-distance, separation and ranking
  checks. Pickup and an unknown/cleared passenger goal cannot authorize a passenger
  alternative. A failed selection leaves previous picker/trip state intact.
- Recovery prepares guarded native reads and settings before publishing replacement
  `NavTarget`, passenger `RideDestination`/`ActiveTripPoint` and fallback metadata.
  Logical goal/resolver/name identities, the operation's fixed progress goal and its
  budgets are not replaced by that physical point. Completed-trip skipping now requires
  matching logical goal **and** the corresponding completed physical arrival.
- No new production file: **19 C# files plus the existing project**, unchanged version
  1.0.5. `DrivingPolicies.cs`, `SpikeState.cs`, project, native diagnostics, arrival rules,
  fares, reverse, road correction, traffic flags and assets remain baseline-identical.
- New production-extraction tests execute resolver/graph adapter, direct ride selection,
  dispatch, fallback and guarded target adoption against substituted native boundaries.
  Parent repeated the same saved-baseline replay: **90 assertion failures / 13 passing
  controls**, no compiler failures; that candidate's full suite **898/898**, no failures/skips.
  The coder's earlier prerequisite RED/GREEN and final **131/131** focused run are
  preserved separately. These are managed replay tests, not native A* or vehicle physics.
- Parent focused Release build with `S1NoDeploy=true`: **0 warnings/errors**. Parent
  scoped production/test formatting checks passed without edits. Candidate DLL SHA-256:
  `697b37e352818f65880d77a0fb8f3eb788bb783fb17a8150c3c0cadda0d02ddf`.
  It is **built, not installed or gameplay-accepted**; installed DLL remains `fb31c4e7…`.

The initial independent code review returned **CHANGES REQUESTED** for this `697b37e3…`
candidate despite those passing tests. Its five additional executable probes reproduce:

1. A metadata-only pick can retain a fallback beyond the **new** logical goal's 60 m
   envelope. Retained physical points need the new goal's budget check before publication.
2. A strict fallback loses its street provenance on a later ride and can undergo General
   Vehicle Graph resampling. Original resolver identity and reusable street provenance
   must remain separate; native-patrol rebounds must not acquire that provenance.
3. An inherited initial-dispatch continuation can navigate after STOP/teardown during
   native preparation, and publishes the active trip point too late for synchronous
   navigation. Initial preparation needs the same request fencing as recovery.

That candidate was **not ready for installation**. The review's source findings and five
assertion failures are preserved under `verifier/`. Passing its earlier 898 tests did not
close those additional cases.

Evidence and exact commands are retained under
`.opencode/taxi-endpoints-20261031-042454/`; whole-tree and isolated checks remain separate
from the focused results above. The coder's earlier whole Pure snapshot predates the last
three progress-read regressions; its 20 passed / 2 failed steps are not final-source
isolation proof. Its later scoped isolated TaxiDriver run passed 898/898, with all eight
owned source/test file hashes matched. A complete final snapshot must be assessed separately.

## Review corrections and current candidate — source closure PASS

The first three findings were corrected in the subsequent `815931f9…` candidate:
retained physical targets must fit the new logical goal's envelope before relabeling;
`RideStreetCandidate` preserves strict fallback provenance separately from original
resolver identity; initial passenger/pickup preparation and its callers share request
fences and publish coherent active points before native navigation. Completed-trip
skipping additionally requires the exact completed order/operation identity.

Parent verification of that intermediate build passed **1,217/1,217**. Its independent
review nevertheless found two ordinary acquisition-failure paths: a persistently missing
agent could leave automatic step 3 busy indefinitely, or leave a phantom active passenger
ride. **Six failing assertions / four passing cancellation controls** reproduced those
cases. The intermediate source, DLL and evidence are preserved under
`coder-review-fixes/` and `verifier-review-fixes/`; that build was not installed.

The current candidate captures a managed receipt **before** agent acquisition. Ordinary
same-request failure visibly refuses and clears its own busy/ride state; cancellation or
replacement during acquisition cannot authorize stale cleanup. Native rollback requires
fresh positive authority/ownership/pointer/no-player-driver checks. With uncertain
ownership only managed ride state is cleared; native lock/vehicle restoration is deferred
to a later positively owned STOP. An automatic-request serial and retained receipt prevent
later ticks from reacquiring cancelled work. Ordinary pickup selection remains independent
of the passenger destination being selected while the taxi approaches.

- Current DLL SHA-256, still version **1.0.5**, subsequently installed with owner approval:
  `7ec8773abf897bd29a3e85d6d995cdae9dbec44a079bee5a519970cd98d44aa7`.
- Parent full TaxiDriver run: **1,269/1,269**, no failures/skips; Release no-deploy build:
  **0 warnings/errors**. Parent scoped formatting checks passed for the five changed
  production files and seven affected test files, without edits.
- Parent and coder reproduced acquisition-failure RED against preserved `8159…` source:
  **6 assertion failures / 4 controls**, no compilation failures; then **10/10 GREEN**
  and **502/502** focused cases. Earlier parent review ports reproduced **7 assertion
  failures / 1 control** against `697b37e3…` with no compilation failures.
- Final changes still use **19 C# files plus one project**, no new production file.
  `SpikeState` now contains provenance/completion/automatic-request identity state;
  `DrivingPolicies`, project, arrival rules, native calculation/seeker hooks, actuator,
  fares, reverse/road correction/traffic settings and assets remain baseline-identical.
  The optional selection snapshot is attempted before publication; current-order
  diagnostic labels do not prove native callback origin.
- These are production-method replays with substituted native boundaries and compiler
  checks, **not** live road-node measurements, IL2CPP physics, native cancellation or
  gameplay acceptance. No extra CalculatePath query, actor mode or radius/budget change.

Independent bounded closure review is **PASS**, with no remaining actionable blocker
found in the reviewed changes. The verifier independently ran **502/502** focused cases
and an overlapping **18/18** original review-port set, then reproduced the preserved
`8159…` RED (6 assertions / 4 controls, no compiler failures). Its identity report checks
all **20 production files and 84 test files**. Detailed scope, commands and hashes:
`.opencode/taxi-endpoints-20261031-042454/verifier-capture-closure/review.md` and the
adjacent `review-identity.json`.

Complete isolated verification and repository-wide outcomes are retained separately
with the check artifacts/shared vault; they must not be inferred from older intermediate
snapshots. Known global format/version drift is not repaired by this candidate. The
source-verification phase performed no deployment, game execution/control, native-save
access, version bump, commit or push. The later installation is recorded separately;
native gameplay remains unverified.

The subsequent owner-requested CUA-assisted session below supplies a new loaded-artifact
observation and a **failed Barn route**, rather than extending the earlier source-only verdict.

## Owner-authorized installation — 2026-10-31T07:15:38Z

After the owner's explicit **"ja"**, the exact closure-reviewed `7ec8773a…` DLL was
installed, still version **1.0.5**, without rebuilding or changing source code. Before
staging, all **134 selected files** matched the final source-verification manifest;
installed Shared matched the build reference. The game was closed.

The five conventional payload files were staged and hash-checked, then passed once
to `Tools/deploy-mod.ps1`. It copied **DLL/PDB only**; metadata, icon and GLB were
identical and left unchanged. Parent readback matched all five target hashes and
confirmed Shared unchanged. New PDB SHA-256:
`53b723ae069013afc708283f1aacebe7b541f2a2a4b95ffcc6bef75610ad2c3d`.

- Attempt: `20261031T071538741Z-836d41e6cbc4`.
- Receipt: `UserData/.deploy/receipts/TaxiDriver.20261031T071538741Z-836d41e6cbc4.json`.
- Backup: `UserData/.deploy/backups/20261031T071538783Z-TaxiDriver-0657f1f2/`, preserving
  the previous installed `fb31c4e7…` DLL and `10078685…` PDB.
- Local pre-state, staged payload, receipt and parent readback:
  `.opencode/taxi-endpoints-20261031-042454/install-20261031-081511/`.

The installed file is **not** a new loaded-artifact or route-acceptance observation.
The agent did not launch/control the game or access native saves; no version bump,
commit or push occurred. Earlier source/review/isolation manifests remain historical
and unchanged; installation-documentation identity and checks are separate evidence.

## Owner-requested CUA-assisted Barn test — 2026-10-31

**Result: pickup and ordinary boarding observed; Barn route FAILED.** The owner asked
the agent to test through CUA. Steam launched the game, and the visible Continue menu
selected **Dominik, slot 1**. The fresh loader log identifies version **1.0.5** and exact
DLL `7ec8773abf897bd29a3e85d6d995cdae9dbec44a079bee5a519970cd98d44aa7`
(12:08:26 local / UTC+01:00). Basis revision remains `94a42c5…`, dirty `main`.

Evidence directory: `.opencode/taxi-cua-20261031-120809/`. Preserved log
`Latest-after-barn.log` SHA-256:
`4d22ad69bf887ef807fee17edd5d5f1fad44fe0fcb16324bd9b1111b8939a645`.
Line numbers below refer to that immutable copy.

| Observation | Evidence |
|---|---|
| CALL TAXI from outside Town Hall; pickup target `(52.5, 0.0, 12.1)` | Log 505; `09-ready.png`, `21-pickup-start.png` |
| Successful complete pickup calculation, endpoint `(52.5, 0.0, 9.5)` | Log 530–531; partial seeker results are separate |
| Pickup arrival confirmed at 12:13:38: 3.3 m, readable halt 1.0 km/h; subsequent parked readback 0.0 km/h | Log 537 onward |
| Player walked to the stopped taxi and used the native entry interaction; UI showed 0 km/h and then ON BOARD | `41-entry.png`, `44-boarded-scancode.png`, `46-ready-for-destination.png`; no early boarding |
| Barn logical goal `(180.1, 1.0, -11.0)` selects street point `(165.3, 0.0, 25.1)`; direct dispatch preserves it | Log 941–949; approximately 39.0 m from the logical goal, within the 60 m budget |
| Four complete native calculations FAILED, operation 2, dispatch orders 4/6/7/8 | Log 967, 1004, 1033, 1056; all four requested/effective settings remain `true/true/false` |
| Road-seeker subpaths complete, but general-seeker errors precede the failed whole calculations | The generic `Searched all reachable nodes…` error does not establish a particular blocking tag or root cause |
| Four recovery episodes and two existing checked free-spot rescues end in GAVE UP | Log 1068: 108.4 m from the street point, vehicle `(58.0, 0.6, 9.5)`; not arrival |
| Visible terminal result and halt | `49-barn-result.png`: COULDN'T GET THERE, FARE $0; `50-final-halt.png`: 0 km/h |

CUA Driver **0.21.0** provided window-bound screenshots, menu clicks, scrolling/dragging
and Tab/Escape input. Unity had no actionable UIA tree; background input was ineffective.
The driver's E input and virtual-key walking also did not produce the intended effect.
Bounded, foreground-checked **Win32 SendInput** relative mouse movement and scan-code
W/D/E input supplemented CUA for walking, aiming and native entry. This was a hybrid UI
test, not a pure CUA or MCP-driven route. An initial supplemental-input logging error
occurred after the mouse event; that event was observed before further input, not blindly
repeated. No input/teleport/path command was injected into the mod. The two relocations
above came from its existing recovery logic.

The native clock reached its **04:00 cap** during setup; the Barn failure was observed in
that context. This is not a daytime comparison or proof that time caused the failure.
There was no pause during the Barn operation. After terminal capture, Escape left the
vehicle; a second Escape produced the visible pause menu (`53-paused.png`). The earlier
`51-final-pause.png` filename is misleading: its pixels show the on-foot transition.
FARE $0 is a UI observation, not an independent payment audit. Native save files were
not inspected, edited or hash-compared; no explicit save/sleep was requested by the agent.

After preserving the paused result, the agent closed its test session through the
visible Quit confirmation (unsaved-progress warning), returned to the main menu and
selected Quit. Game process absence was read back at **12:30:24 local**; the CUA daemon
started for the test was stopped. Final installed DLL hash still matched `7ec8773a…`.
`Latest-after-exit.log` is retained separately, SHA-256
`de2625e66da4854d4c38b1cc7fbf1b1ea1892bad75c7e2cd1e8bb7eaf493e1f7`.
This is UI/process shutdown evidence, not a byte-identical save-preservation claim.

Bungalow, Docks, the chained route and early boarding were not tested in this session.
The next investigation is why complete native calculation rejects this selected road
point despite successful road subpaths. Selection-only source tests do not close that
runtime failure. Prior owner acceptance of v1.0.0 stays historical and closed.

## Remaining runtime check

The later [before-04:00 comparison and native-shortcut investigation](native-shortcut-verification.md)
reproduced the same Barn calculation failure at 21:39 and found a missing native
direct-path error guard. That continuation owns the next repair and its evidence.

After resolving the observed Barn failure, test each destination from the same safe
starting position with a freshly called taxi. Compare selected street point, actual
complete calculation endpoint and confirmed final halt; only then test the chained
Barn → Bungalow → Docks trip. Early boarding remains a separate check. Selection
on a road graph does not prove full reachability, physical clearance or gameplay success.
