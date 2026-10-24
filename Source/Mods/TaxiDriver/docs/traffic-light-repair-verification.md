# TaxiDriver traffic-light repair verification

## Scope — 0.8.21

The owned host-side autonomous taxi supplies a private DriveFlags copy with IgnoreTrafficLights enabled only during the native parameterless UpdateSweep and UpdateSpeedReduction calls. The old GetWheelSweepCollision detour is not installed. All other flag fields are copied unchanged and the exact prior reference is restored in postfix/finalizer, including partial-install failure.

Eligibility requires current exact vehicle/agent ownership, an active order, autonomous control, no player driver, active gameplay/lifecycle, no pause/reverse, and a physically simulated nonkinematic rigidbody. Kinematic vehicles use a different native raycast path that does not consume the ignore-lights flag: they deliberately retain vanilla behavior rather than disabling generic obstacles. No shared flag, light state, collider, speed output or throttle is overridden.

## Native evidence

GameAssembly SHA-256 `61829050e56ca180d67be592e4bfe61ec97d2e9573f720d90a54e318a7e4381d` was recomputed. Installed Cpp2IL metadata and capstone disassembly establish:

- UpdateSpeedReduction RVA 0xAD7D20 calls Rigidbody.get_isKinematic (0x2BF04C0) at 0xAD8023 and branches at 0xAD802A. Physical path calls GetWheelSweepCollision at 0xAD806A/0xAD80AC; kinematic path calls GetForwardObstacle at 0xAD8169.
- InfrequentUpdate invokes UpdateSweep at 0xAD559C and UpdateSpeedReduction at 0xAD55A6. The sweep consumer reads IgnoreTrafficLights at flags offset 0x20 and skips only VehicleObstacle.type=1, continuing native hit filtering. Turn/destination reductions remain separate and unmodified.
- The installed HarmonySupport bridge converts all original native arguments before invoking patched managed code. A prefix that does not declare an original out argument still does not avoid that conversion. This is why the repair moves to zero-argument methods; it is not a claim to have reproduced the precise cause of the historical trampoline NRE.

See traffic-light-repair-investigation.md for the initial evidence gate and tool setup. The last test log plus owner report correlated a halt at (93.1, 0.6, 2.2) with the five-second watchdog/reverse intervention; the prior log did not itself record a signal state.

## Explicit traffic-wait guard

Stationary-only observations run at most every 0.25 seconds, keyed to the current vehicle and navigation order. Normal managed-to-native sweep calls (not detours) compare native left/right results before and during a temporary private ignore-lights scope. Only readable traffic-only baseline hits with no generic baseline hit and no remaining hit after filtering can hold recovery. Unknown, moving, foreign, player-driven, reversing, kinematic or unhealthy state never holds it.

A held interval shifts only the operation no-progress deadline and local startup/self-test/progress-window clocks; it preserves the progress origin and does not create progress or refill attempts. A pure TrafficWaitBudget allows at most 30 seconds total per genuine navigation operation across repeated stops/redispatches. It grants no exemption inside the arrival radius, so normal arrival supervision remains available. At the cap, ordinary bounded supervision resumes even if the signal classification persists. Optional probe failures disable only the probe, not the functional bypass; failed flag restoration still disables the feature. Generic collisions remain native. The guard does not turn a halt into arrival or retry a fare.

## Executed checks

- Parent observed behavioral policy/source-contract RED before implementation (4 failed assertions, 9 passed), not just compile failures.
- Initial full independent review found an unbounded arrival-suppressing hold, probe-failure coupling and a stale progress origin. Parent added regressions, observed 4 additional failing assertions, repaired all three classes, then reran the full suite: 670 passed, 0 failed/skipped. Added tests cover the finite cumulative allowance, arrival, invalid time, shift-not-reset and diagnostic/function separation as well as eligibility/unknown/generic hits and activation wiring. Existing private-scope tests cover restoration, exceptions, alias rejection and nesting. These are pure policies/source contracts, not native gameplay acceptance.
- Forced compile-only Release: 0 warnings/errors. Scoped production/test formatting passed.
- Full independent review first found the hold/arrival, probe and progress-window defects above. Bounded delta re-review passed the runtime repairs; its remaining source-contract coverage requests and clamp nit were applied. Final independent contract closure returned PASS, with no gameplay claim. Parent reran 670 tests, forced Release and whole-solution/test formatting; version sync, doc paths and Skills health are clean.
- Release deployed after a fresh game-closed check. Installed DLL/PDB/mod.json were read back and hash-matched; installed metadata reports 0.8.21. DLL SHA-256 `f25f231b32ec09c85561911f8c928d069a3d37ec1e44c00d4edbb8928bc7d409`; PDB `46d642b9f7189de3e637da7f2b9bf7a237d168df95d25ba1b48e63fda1604fd3`; metadata `b8a32e542897586e94ff459af5904695a9c3805c18f590fe0e4a6152bd83448f`. Shared and the GLB remain byte-identical. Deployment receipt attempt: `20261020T182120443Z-0b4c75b58ab7`; backup ID: `20261020T182120481Z-TaxiDriver-ffdbc279`. Native gameplay acceptance remains open.

## Expected log and in-game acceptance — open

Startup: `[traffic-lights] parameterless physics consumers healthy=True`.
First eligible order: `phase=consume-private-copy` with `ignore=True`, followed by `phase=restored-original` with the original pointer/value.
A positive stationary classification logs `native sweeps identify an exclusive traffic-light stop; bounded recovery allowance evaluated separately`.

Test a red crossing on the previously observed route, a genuine solid/vehicle obstruction, destination braking, pause/resume, STOP/reroute, and normal player-controlled vehicles. No native-to-managed trampoline exception spam is acceptable. If an unsupported/unsafe state is reported, vanilla remains the fallback; do not globally disable colliders or force speed as a substitute.

Wheel animation, fares and road-correction behavior remain from 0.8.20; this release does not claim to fix all navigation/jitter issues.
