# TaxiDriver consolidation and reverse-recovery verification

## Authorized scope

The owner explicitly selected permanent production consolidation to exactly 19 C# files plus the existing TaxiDriver.csproj, followed by the reverse-recovery repair in the supplied implementation brief. Tests, inherited build files, dependencies and assets remain outside this count. No commit or push is authorized.

## Baseline (executed)

- 58 handwritten production C# files, 15,255 lines; entry version 0.8.18.
- Release build: passed, 0 warnings, 0 errors, with deployment and project-reference builds disabled.
- TaxiDriver.Tests: 498 passed, 0 failed, 0 skipped.
- Evaluated configuration: net6.0, C# 12, nullable enabled, IL2CPPMELON; 26 reference HintPaths exist. Project remains at src/TaxiDriver.csproj, preserving inherited Directory.Build.props/targets.
- Built and installed Shared.dll hashes match: b1ce1669a2b6b48461383c1188e63f00dd094326f10ddcd02cdc7e0e55004912.
- Latest.log reports game 0.4.7f12. The captured session contains initialization but no taxi trip reproducing the reported reverse failure. It is not a reproduction of that failure.
- The game was not running during baseline inspection.
- Immutable baseline source/tests/assets archive and SHA-256 manifest: C:/Users/pc/TaxiDriver-work-2026-10-18/baseline.zip and baseline-manifest.json. Build/test logs and evaluated reference hashes are alongside them. These are local evidence, not distributable game assemblies.

## Functional acceptance contract

1. Distinguish operation lifetime, per-dispatch callback identity and bounded reverse phases. A new operation resets budgets; redispatch, reverse, jitter and artificial placement do not.
2. A self-test handoff spends a bounded recovery episode but does not claim a physical reverse failed at that position. Record a real attempt before issuing the native start call.
3. Request reverse only for the exact owned living taxi, current lifecycle/operation, host authority, confirmed intended driver, no player at the wheel and a healthy actuator. Recheck after native calls.
4. Confirm horizontal directed net reverse movement, not arbitrary displacement or native request flags. Preserve existing 1.5/3-second characterization windows and -0.7 input. Require a forward-clearance decision before redispatch; 0.6 m plus a blocked front is not escape.
5. Supervise reverse before meter finalization independently of polling. Independently reject expired/invalid reverse intent at ApplyThrottle. Preserve unconditional restoration of an already-captured actuator scope.
6. Revoke managed intent before native cleanup. STOP, takeover, lifecycle/scene loss, actuator failure and unreadable state cannot schedule future navigation. Native cleanup steps are isolated and require current authority/ownership; never park against a player driver.
7. Queue accepted native navigation outcomes for supervision rather than recursively issuing navigation from callbacks. Current recoverable failures retain the operation and meter; stale and controlled-stop callbacks cannot affect a new order.
8. Final arrival requires readable finite speed and a confirmed stop, target proximity and no reverse. A fresh nearby target is evaluated independently; an ended trip without a new order remains parked.
9. Preserve fare rates, settlement-before-IO claim and no-retry behavior for unknown payment results. Temporary recovery must not finalize or restart a meter.
10. Keep native driving, traffic-light hook disabled, model/identity and existing ownership/passenger safeguards. Do not implement adaptive shorter reverse clearance without measured braking/runout evidence.

## Verification sequence

Baseline -> mechanical consolidation -> separate build/tests and source/type preservation proof -> immutable midpoint snapshot -> targeted RED/GREEN functional regressions -> complete suite and Release compile -> independent review -> fix/re-review -> version/metadata consistency -> game-closed release deployment and installed readback.

## Mechanical midpoint (executed)

- Exactly 19 root-level C# files plus unchanged TaxiDriver.csproj; all 58 original sources mapped exactly once.
- Release build: passed, 0 warnings/errors. All 498 baseline tests passed with 0 failed/skipped. Mod and test format verification passed.
- Reconstructed 46 bundled sections and checked the 12 unchanged files against baseline; 0 preservation failures. Assembly header and per-type IL match except compiler-generated static-data address labels.
- Parent independently checked the SHA-256 of all 19 on-disk sources against the recorded midpoint manifest: 19 matched. Preserved midpoint DLL SHA-256: 6ec4e9507a68c50adb6e20cb3a81fff656c1e19a661444fd60d59e7bc1493bf5.
- Test Compile links were replaced by a deduplicated set of four pure bundles. Source-contract lookup extracts marker-delimited current sections; no assertion was changed during consolidation.
- Local evidence: C:/Users/pc/TaxiDriver-work-2026-10-18/proof/mapping.md, reconstruction-report.txt, il-compare-report.txt, and snapshots/midpoint-dll/TaxiDriver.dll.
- Evidence retention incident: the original work directory disappeared during the mechanical phase for an undetermined reason. The consolidator reconstructed the baseline from HEAD and reported all 136 hashes matching the saved manifest; the preserved baseline DLL is from that isolated rebuild, not the lost first artifact. See mapping.md for the separate path-control comparison. Do not claim the original baseline.zip/build logs remain available.

## First functional candidate (rejected; not deployed)

The first candidate passed 542 tests and compiled cleanly. The parent reran the complete
542-test suite, but source inspection and independent read-only review rejected release.
The green tests did not cover all production transitions and some encoded incorrect assumptions.

Confirmed repair requirements: remove the patrol's surviving distance-only arrival branch;
recheck rear clearance/actuator health/player takeover during reverse; allow native cleanup
with a local passenger (not a player driver); protect the StopReversing transition from its own
callbacks; enforce the real 1.5/3-second input window at the consumer, not only a 6-second outer
cap; forbid blind forward dispatch after ineffective reverse; accept readable finite -1 speeds;
reject unreadable movement evidence and best-ever rather than current escape credit; shift
pause measurement times; invalidate pending outcomes across operations; replace polling-off
inferred settlement with an explicit terminal verdict; queue callback processing and clear
managed intent on early exits. These are release blockers, not documented acceptance exceptions.

## Second functional candidate (rejected; not deployed)

The parent executed the complete suite: 583 passed, 0 failed, 0 skipped. The nonincremental
Release compile passed with 0 warnings/errors. An independent exact-tree review confirmed
the earlier recovery, input-window, pause, takeover and explicit-operation repairs, but
blocked deployment on two remaining transitions:

- TickReDispatch recorded a failed/absent StopReversing call only in logging, then still
  permitted retry/rescue/forward dispatch. A failed stop must prohibit every follow-up and
  perform independently guarded cleanup without touching any replacement operation.
- Current native Complete callbacks could publish terminal arrival (or pickup boarding)
  before readable halt and proximity confirmation. Completion evidence must be supervised,
  bounded and kept separate from terminal publication.

## Final transition candidate (executed; release 0.8.19 deployed)

- F1 now uses the runtime-called ReverseStopTransitionPolicy: failed/unauthorized/superseded
  StopReversing forbids every follow-up; independent fallback steps revalidate the captured
  operation/vehicle/authority/player state. The caller rechecks operation identity after native calls.
- F2 records scoped Complete evidence and supervises both pickup and ride arrival before terminal
  publication/boarding. Evidence waits at most 3 seconds without a confirmed halt; stale evidence
  cannot affect a new operation. A long pause may discard this short settle evidence on resume;
  the operation remains active under the existing polling/recovery bounds rather than restarting
  or prematurely settling the meter. This is a known conservative behavior, not a measured native result.
- Final delta review PASS on the exact runtime wiring (local transcript:
  C:/Users/pc/AppData/Local/hermes/cache/delegation/live/deleg_b6a2d76a/task-0.log).
- Parent reran 605 tests after the version bump: 0 failed/skipped. Final compile and deployment:
  0 warnings/errors. Scoped mod/test format, version-sync (21 mods, 0 drift) and doc-path checks passed.
- The game was closed; built/installed Shared hashes matched and remained unchanged. Two
  nonincremental 0.8.19 builds (compile-only and deployment) produced the same DLL. Installed DLL,
  PDB, metadata and GLB were read back and hash-matched. DLL SHA-256:
  `5c0e889862ee3496fbf0bd6b77aa07687fff1df9ae9676d26c770b14ac9c6b83`.
- Installed assembly metadata confirms ReverseManoeuvrePolicy, NavigationOperationPolicy,
  ReverseStopTransitionPolicy and ArrivalConfirmationPolicy. No game was launched for this release.
- Release logs, artifact readback and old installed DLL/PDB/JSON backup:
  C:/Users/pc/TaxiDriver-work-2026-10-18/final-release/. Original version was 0.8.18.
- Evidence quality: the first repair's deliberate regression injection proves test sensitivity,
  not original test-first ordering. The final transition slice wrote tests before implementation:
  19 assertion failures / 3 trivial negative-case passes with runtime-wiring checks against old
  code and executable tests against signature stubs; final complete suite 605/605. Pure policies,
  delegate seams and source contracts are not a live Unity host.

## Final production inventory (19 C# plus one project)

Mod.cs; TaxiApp.cs; TaxiInteractionPolicies.cs; TaxiConsoleCommand.cs; SpikeCommands.cs;
SpikeRunner.cs; SpikeState.cs; TaxiAI.cs; TaxiDestinations.cs; TaxiStand.cs; RoadNavigation.cs;
DrivingPatches.cs; DrivingPolicies.cs; TaxiDriverNPC.cs; RideLocks.cs; FareMeter.cs; FareDomain.cs;
TaxiLifecycle.cs; TaxiVisual.cs; TaxiDriver.csproj.

The project stayed at its original path. Tests now link deduplicated pure bundles and their
source readers recover original marker sections. No tests, assets or generated files were
deleted to meet the budget; no new assembly or hidden production source was introduced.

## Gameplay acceptance (not executed)

- Native StartReverse effect, signed speed behavior, StopReversing callback ordering and actual colliders at the failing location remain unverified in gameplay.
- Required gameplay checks: blocked-front/new-order with clear rear; blocked rear and newly entering obstacle; short reverse with front still blocked; normal pickup/board/ride/arrival/exit; STOP/reroute/pause/save transition during recovery. A log saying requested or reversing is not movement proof.
- Full gameplay acceptance remains open until those checks are actually observed. No generated or simulated result substitutes for a game test.
