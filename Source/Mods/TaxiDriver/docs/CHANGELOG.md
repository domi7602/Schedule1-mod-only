# Changelog

## 1.0.0 (2026-10-24) — Milestone release of the accepted driver-controller repair
- Version-only milestone: the production code is the owner-accepted 0.8.22 build. `git diff`
  shows only the `MelonInfo` version string and `mod.json` metadata changed; no behaviour,
  patch, prefab, save or S1API/S1MAPI surface was touched.
- **Verified, owner-confirmed 2026-10-24:** Dominik confirmed the tax driver work is finished.
  Individual tested actions and the exact session were not itemized, so the per-finding checks
  in `driver-binding-verification.md` remain their own record.
- The rebuilt 1.0.0 assembly carries the new version string and therefore has a different hash;
  the code path is the accepted one. Known and unchanged: road-correction/jitter on some routes;
  multiplayer is out of scope (owner does not play MP, 2026-10-24).

## 0.8.22 (2026-10-20) — Registered driver controller and retained patrol release
- **Verified in-game 2026-10-24 (owner-confirmed):** Dominik confirmed the tax driver work is
  finished. Individual tested actions and the exact session were not itemized, so the per-finding
  checks in `driver-binding-verification.md` remain their own record rather than being marked
  passed one by one. Known and unchanged: road-correction/jitter on some routes, and no separate
  multiplayer acceptance.
- Use driver.Behaviour and require exact owner, shared NetworkObject, ComponentIndex registration, BehaviourIndex stack membership and server readiness. No runtime-added NPCBehaviour/VehiclePatrolBehaviour, owner rewiring, registration repair or S1API/prefab/save changes. If no registered idle patrol exists, use the existing direct VehicleAgent.Navigate path.
- Preserve borrowed route/vehicle/aggression/enabled state; retain the original lease and waypoint objects until release is confirmed. Reentrant cleanup cannot discard or resnapshot dirty state. Deactivate is requested at most once and need not complete synchronously. Pending release fences fallback, redispatch, polling retries, reverse and relocation without pausing the operation deadline or refilling budgets.
- Permanent ownership/initialization loss can keep replacement navigation blocked until the native instance is destroyed or the scene is reloaded. During player takeover only the leased component is quiesced; no vehicle/seat actuation is issued, and restoration requires safe ownership again. Do not claim every pending condition is automatically recoverable.
- Parent observed RED for the controller contract, positive eligibility case, lease cleanup, retained-release gate and recovery/polling fences before repair. 683 tests pass (670 prior + 13 new pure-gate/source cases); forced Release 0 warnings/errors; scoped production/test formatting pass. Independent review fixes applied; delta re-review PASS. Whole-solution formatting is blocked by out-of-scope LightControl VisualResolver whitespace at lines 99–101, left unchanged.
- The prior 0.8.21 red-light behavior is owner-confirmed; the test log records restored flag references and arrivals at Barn/Sweatshop, but also Docks failures and real wall proximity. Controller/departure gameplay acceptance on 0.8.22 remains open. Wall placement, road correction, fare rules and traffic-light behavior are not changed. See driver-binding-verification.md.
- Release deployed with the game closed; installed DLL/PDB/metadata read back and hash-matched, version 0.8.22. Shared/GLB unchanged; receipt and artifact hashes are in driver-binding-verification.md. No commit/push.

## 0.8.21 (2026-10-20) — Scoped traffic-light bypass and explicit wait protection
- Replace the disabled GetWheelSweepCollision detour with complete prefix/postfix/finalizer scopes on parameterless UpdateSweep and UpdateSpeedReduction. Exact owned host autonomous physical taxi only; no player driver, pause or reverse. Fresh private flags preserve every other setting; restore the exact original reference on exit/error.
- Native f12 disassembly proves physical sweeps consume IgnoreTrafficLights while kinematic raycasts do not. Kinematic/unknown states retain vanilla handling; no global collider/light mutation, generic-obstacle bypass, forced speed/throttle or reduced destination braking.
- Stationary native sweep comparisons can positively classify traffic-only blocking with no remaining generic hit. Only that observation excludes wait time from recovery clocks and prevents recovery spending, capped at 30 seconds total per operation and never inside the arrival radius; it never refills attempts or declares arrival. Unknown/mixed hits retain normal supervision. Optional observation failures do not disable the functional bypass.
- Parent observed 4 initial regression assertions and 4 review-repair assertions failing before their repairs, then reran 670 tests (0 failed/skipped); forced Release compile has 0 warnings/errors, whole-solution and test format checks pass. Independent findings repaired and final contract closure PASS; Release deployed and installed DLL/PDB/metadata hash-matched. Actual red crossing, generic braking and native hook behavior remain In-Game-Verify open. See traffic-light-repair-verification.md.
- Wheel visuals, road correction/jitter and fare behavior remain from 0.8.20.

## 0.8.20 (2026-10-20) — Visual wheel rolling and steering
- Animate only the four matched imported wheel nodes: signed no-slip rolling from native rigidbody point velocity and collider radius, plus native steering membership/angle. Preserve mounting offsets and rotation; pair in the common model frame instead of trusting GLB front/left labels. The GLB and materials are unchanged; no suspension transfer.
- Cache wheel references on swap with three bounded initial binding attempts. Preserve the cache through ride end/park/reuse; sever it on model rollback, confirmed vehicle death, scene change and shutdown. Failed native visual reads stop visual work without throwing into the driving loop or repeatedly rebinding animated poses.
- Emit a normal-log `[visual] visual wheels bound: ...` summary. Missing/unsupported links remain static; visual motion is a no-slip approximation during wheelspin/skids and is capped at 180 degrees per frame during hitches.
- Parent reran 649 tests (0 failed/skipped) and forced compile-only Release (0 warnings/errors); scoped production/test format checks passed. Lifecycle regression contracts were observed failing before repair. Final independent delta re-review PASS. Forced Release build/deploy: 0 warnings/errors; installed DLL/PDB/metadata hash-matched (metadata 0.8.20), both new wheel types confirmed in the installed assembly. Shared and GLB hashes unchanged. In-Game-Verify open; see wheel-visual-verification.md.
- Traffic-light hook stays disabled. Navigation, road correction/jitter, fares, ownership and passenger protection are unchanged.

## 0.8.19 (2026-10-18) — Bounded reverse supervision and source consolidation

- Mechanically consolidated 58 production C# files to 19 plus the unchanged project; preserved namespace/import scopes and all baseline types/IL, with 498 baseline tests passing at the separately verified midpoint. Updated deduplicated test links and marker-aware source lookup.
- Separated operation lifetime from native navigation orders and polling. Queue scoped callback outcomes before ride supervision; recoverable failures preserve the active meter and budget. Arrival and pickup boarding require readable finite halt, proximity and no reverse; a Complete callback supplies bounded evidence, not an immediate arrival.
- Added requested/moving/transition reverse phases, directed current net movement, real request-window and outer-cap checks at the native input consumer, continuous rear/driver/authority/actuator supervision and safe early-exit cancellation. Pause shifts reverse measurement clocks; valid signed -1 speed is not a read-failure sentinel. Self-test handoff no longer invents a failed physical manoeuvre.
- Revoke intent/invalidate old callbacks before native stop. A throwing, unauthorized or superseded reverse stop permits no retry/rescue/forward command; fallback cleanup steps are independently guarded. Preserve unconditional actuator-snapshot restoration, -0.7 pedal, 1.5/3-second windows, recovery budgets, fare rules and the disabled traffic-light hook.
- Parent reran 605 tests (0 failed/skipped) and a nonincremental Release compile (0 warnings/errors). Independent final delta review PASS. Earlier rejected candidates and regression-sensitivity versus genuine test-first evidence are recorded in reverse-recovery-verification.md. Tests cover pure policies, delegate seams and source contracts, not Unity physics.
- In-Game-Verify open: blocked-front departure, dynamic rear obstacle, ineffective short reverse, normal pickup/ride/arrival, STOP/reroute/pause/slot change and native callback ordering. Existing coarse sphere clearance is retained; no adaptive shorter reverse or measured braking/runout guarantee.

## 0.8.18 (2026-10-15) — Terminal relocation rescue and balance readback

- Terminal rescue no longer skips the guarded relocation when the car is on the vehicle graph. The 2026-10-15 `Latest.log` proved the old gate wrong: the ride "taxi to Sweatshop" gave up **106.7 m from the target** at `(40.1, 0.6, 52.5)` with `car is on the vehicle graph but still stuck ... no teleport needed, giving up`, while the car sat motionless (reverse moved 3.1 m, re-dispatch idle for 7.4 s). Graph membership only proves a route exists, not that the car can reach it. `TerminalRescuePolicy` now decides: graph state never vetoes the attempt, only the spent one-shot rescue budget does (`NavigationRecovery.TryLastRescue`, unchanged). The attempt runs the existing guarded `RoadKeeper.TryRescueStartup` (supported free spot, ground snap, oriented-box clearance, occupancy check, road-aligned rotation), then re-dispatches. The recurrence-branch redispatch stays unchanged — the same log showed it recovering 72 m without intervention (`no supported free point` → re-dispatch → progress).
- Settlements now log a balance readback: `FareMeter.Charge` reads net worth (S1API `Money.GetNetWorth`, cash + online) before the money calls and logs the observed movement after (`[meter] balance readback for $N: net worth X -> Y (observed Z, charged $N) — movement matches/does not match the charged amount`). `BalanceReadbackPolicy` formats the verdict (tolerance $0.51 for float noise); the readback is log evidence only — never proof of causation, never retries or corrects a payment, and a skipped/unreadable read never blocks settlement. The `payment calls returned ... (balance movement unverified)` wording is unchanged (a void return is still no proof); the readback line adds the observed numbers next to it.
- 498 tests pass (489 baseline + 7 policy + 2 wiring contracts; RED observed before wiring). Release built/deployed with 0 warnings/errors after game-closed and Shared-match checks; installed DLL hash-matched. Gameplay verification open: rerun `ParkingGarage` → `taxi to Sweatshop` and confirm (a) the terminal stall at `(40.1, 0.6, 52.5)` now attempts `attempting teleport recovery` instead of giving up, and (b) `[meter] balance readback` lines appear with movement matching the charged fares.

## 0.8.17 (2026-10-14) — Traffic-light hook disabled after live NRE

- The 0.8.11 wheel-sweep traffic-light hook (`TrafficLightPatch.Apply`, private scoped `DriveFlags` copy on `VehicleAgent.GetWheelSweepCollision`) is DISABLED. Root cause, proven by your 2026-10-14 `Latest.log`: the hook threw a native->managed `NullReferenceException` hundreds of times per second from the first pickup navigation onward, starving the `VehicleAgent` of sweep results. Consequences in that session: the taxi never reached its parked pickup state, rolled on without an active order after the give-up (idle guard stopped after 3 attempts), and could be player-driven because the passenger input gate only engages in `RidePassengerMode`. `TrafficLightPolicy`, the hook class and its unit tests remain in the tree for a trampoline-safe rework; `TrafficLightPatch.Remove()` stays in the teardown path as a no-op guard. Native traffic-light stopping behavior is restored (as before 0.8.11). Cruise, actuator, ride locks and packages 1–5 are unchanged.
- Parent reran 489 tests (0 failed/skipped). `TrafficLightWiringTests` now pins the disabled state per active source line. Release built/deployed with 0 warnings/errors after game-closed and Shared-match checks. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `035ddaa65ba814c79529026acf5cd47036d5016b66b77b47932cc6f3701af964`.
- Gameplay verification open: confirm no `GetWheelSweepCollision` ERROR spam, pickup stops at the player position, and the player cannot drive the taxi during an active ride. The 0.8.16 deinit expectations (`[taxi-lifecycle] teardown complete`, single meter line, no despawn) still apply at quit.

## 0.8.16 (2026-10-13) — Deinitialization lifecycle

- Deinitialization now runs a one-shot, exception-isolated teardown: the shutdown gate closes before work and cannot reopen mid-cleanup; update/deferred/native callbacks quiesce first (nav orders invalidated, pending spawn/auto-run/seat-verify/settle/lot-dump cleared); the fare meter settles exactly once (claim before report IO, no retry on failed/unknown reports); patrol/navigation/actions stop; only owned trunk components are restored; no foreign seat/player mutation, remote exit or blind occupied-taxi despawn. Harmony hooks (actuator, cruise, traffic-light, ride protection, trace) are removed last via the installed `Harmony.UnpatchSelf`, so a later re-init can re-apply them; pre-teardown callback generations stay inert after re-init. Repeat/reentrant deinit is a no-op; partial init is tolerated.
- Parent reran 489 tests (0 failed/skipped); independent focused review PASS. Release built/deployed with 0 warnings/errors after game-closed and Shared-match checks. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `e3752ad1d95f91842aed08e685e2465acd08fa5cc361404fd6b8f135eaa515e7`.
- In-game verification open and pending the joint test: expect `[taxi-lifecycle] teardown complete (quit/deinit)`, at most one `[meter] ride ended (session teardown …)` line and no taxi despawn/seat changes on quit. Harmony finalizer execution, MelonEvents unsubscribe symmetry and S1API event removal are source-verified only. Minor non-blocking: the teardown-complete log also prints on the second (no-op) teardown call, and the park intent is reset by hook removal in the same frame, making the parked log line optimistic at quit. Fare rules, recovery budgets, road/traffic patches, destination identity, seat/STOP safety and package-4 listener leases unchanged.

## 0.8.15 (2026-10-13) — Destination identity and app subscription lifecycle

- Logical destination identity (name/kind/exact goal) is separate from physical route identity. A different destination sharing the running route updates picker/RideGoal/active-trip metadata without redispatch or order/budget/fare reset; reachable/fallback coordinates remain authoritative. Repeated same picks are no-ops while driving; give-up permits fresh dispatch. Hero rendering uses only its complete snapshot, including drop-off and active-trip state. Owned row, persistent/search and update listener identities are released before row/base-app destruction; forwarding leases sever callback targets and helper removal handles dead events/partial registration.
- Parent reran 464 tests (0 failed/skipped); independent focused review PASS. Release built/deployed with 0 warnings/errors after game-closed and Shared-match checks. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `c7cf35c2c1c3197e5e705576f6988dff0c8ae47be1cef569055fad8a2860cd8c`.
- IL2CPP/gameplay verification open: shared-drop-off selection, recovery-only hero refresh and actual helper-root counts across search/filter/reopen/scene teardown untested. If native removal and empty-event fallback both throw, helper entries may remain, but released forwarding leases no longer retain the app. Static UI, fares, recovery, seat/STOP safety, road and native patches unchanged.

## 0.8.14 (2026-10-13) — Multiplayer seats and STOP occupancy safety

- Passenger seating uses only verified vacant or exact-local native identities. Driver reservation requires raw reference absence (ReferenceEquals), independently of Unity overloaded null/seat occupancy. Failed seating stops navigation and attempts only safe local exit; new, ongoing, idempotent and reroute paths honor failure. STOP removes/verifies NPC occupants, then proves all player/seat/registry occupancy empty before consuming destruction attempts; occupied/unreadable/stale aggregate state reports Waiting for passengers and retains the taxi. CurrentVehicle is compared against the vehicle NetworkObject pointer. Existing ambiguous retry, fares, navigation, road and native patches unchanged.
- Parent reran 436 tests (0 failed/skipped). Independent package review identified a P2 Unity-null reservation gap; observed RED/GREEN repair and focused independent re-review PASS closed it. Release built/deployed with 0 warnings/errors after game-closed and Shared-match checks. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `42b6c91f224b9e71946faf3b5a341ce794c6a79210a74b462c6cd7345959b35d`.
- Multiplayer/gameplay verification open. Unreadable registry/seat state or stale aggregate occupancy conservatively retains the taxi until a later explicit STOP confirms vacancy. No forced remote-player exit.

## 0.8.13 (2026-10-13) — Supported road-snap slope correction

- XZ projection now interpolates original 3D route elevation. Hard placement requires filtered static support within +/-0.75 m, eight-corner measured chassis clearance at the proposed heading, root-to-bottom offset plus 0.05 m, and no foreign collider overlap. Offset candidates are reprojected onto the route; unsafe candidates are rejected before placement/progress bookkeeping. Existing bounded recovery, fares, native driving and package1 remain unchanged.
- Parent reran 404 tests (0 failed/skipped); independent read-only review PASS. Release built/deployed with 0 warnings/errors after game-closed/Shared-match checks. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `5f38fa1b4e735fcd6bacdf1d51fbc9b2acaa9414de9177ffe2f9fe58517958da`.
- Gameplay verification open: conservative world-AABB clearance may reject valid sloped/crowded spots; a single support probe is not proof of wheel support or semantic road identity.

## 0.8.12 (2026-10-13) — Navigation arrival/start-grace/terminal-state fixes

- Patrol and ride verdicts share one arrival policy (`NavigationArrivalPolicy`, 10 m radius). The loose 25 m patrol terminal allowance is removed, so a final-waypoint stop between 10 m and 25 m no longer ends patrol as ARRIVED while the ride verdict reports GAVE UP. Standing uses absolute signed speed: reverse motion at the last waypoint no longer qualifies as standing.
- The unconfirmed, matching-order 8 s patrol start self-test now blocks the ordinary stall watchdog for its full grace (`PatrolStartPolicy.ShouldWait`); static-obstacle (2.5 s) and ordinary (5 s) stall recovery can no longer preempt it. Arrival, the operation deadline and upstream drive-start proof sampling remain ahead of the wait gate.
- `BeginNavigationOperation()` clears stale terminal state (`NavGaveUp`, `RideArrived`, callback verdict) via `NavigationTerminalPolicy` at genuinely new Go/patrol dispatches only; recovery redispatch does not call it, so the 4-episode recovery cap, 90 s meaningful-progress deadline and recovery budgets survive unchanged.
- Independent read-only review PASS (10 m consistency, abs speed, gate ordering, new-op-only reset). Parent reran the full suite: 361 tests passed, 0 failed/skipped; Release compile-only build 0 warnings/errors. Pure/source tests do not exercise Unity orchestration; in-game verification open.

## 0.8.11 (2026-10-13) — Taxi-only native IgnoreTrafficLights

- At the proven GetWheelSweepCollision consumer, supply a physically private DriveFlags copy with IgnoreTrafficLights=true only for the exact owned host-side autonomous taxi with an active order and no player driver. Copy all nine other settings verbatim; restore the original reference in postfix/finalizer, including partial-write failure. Shared flags are never mutated; patrol resets/redispatch settings are copied afresh.
- Native f12 instruction assertions prove only TrafficLight obstacle hits are skipped by this flag in the wheel-sweep branch. Generic hit filtering, native turn/destination reductions, cruise, recovery and fares are unchanged. A separate forward-raycast branch does not consume the flag; actual signal behavior on that branch remains unverified. See `traffic-light-native-evidence.md`.
- Parent reviewed code and reran native instruction assertions and all 324 tests (0 failed/skipped). Release built/deployed with 0 warnings/errors after confirming no game process and matching Shared hashes. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `54598bbbfc3b5c4632fb6323927512653a704993406b3e555c9088e7aa3c36b5`.
- In-game verification open: Harmony dispatch, signal crossing, generic collision handling and per-call allocation cost unverified. Check `[traffic-lights] healthy=True`, `phase=consume-private-copy` and `phase=restored-original`.

## 0.8.10 (2026-10-13) — Recurring-stall recovery quick wins

- Retain recovery history across reverse/redispatch. Reverse displacement proves movement, not escape. A repeated stall within 6 m XZ without 4 m of new eligible approach to the fixed operation goal switches to the existing collision-checked free-spot rescue instead of repeating the reverse ladder.
- Clear local recovery history only after fresh forward approach with active autonomous driving, signed speed >=2.5 km/h and positive sampled throttle. Reverse, unpowered coasting, artificial placement and previously visited ground cannot earn reset credit. These gates are not wheel-actuation proof; closest-goal progress remains conservative on detours.
- Recurring recovery spends the existing operation budget before side effects, releases patrol, re-seats the dedicated driver and honors safety aborts before relocation. Preserve four-episode operation cap, 90-second meaningful-progress deadline, rescue budgets, normal steering/cruise, fares and static destination UI.
- Subagent observed assertion RED-to-GREEN regressions; parent reviewed runtime wiring/policy and reran 316 tests (0 failed/skipped). Release built/deployed with 0 warnings/errors; installed DLL/PDB/mod.json read back and hash-matched. DLL SHA-256 `a726dee56b0ba4b8abd83ebcc4c4c8a0eb1779fe07dbbce80bf3b5521cb9182e`.
- Gameplay verification open. Expected branch breadcrumb: `[nav] recurring stall ... checked free-spot rescue instead of reverse.` Safe-spot availability and successful route escape are not established by pure/source tests.

## 0.8.9 (2026-10-13) — Static destination list

- Disable destination ScrollRect inertia and initialize velocity to zero; retain clamped scrolling and existing instant/no-transition buttons. Pin destination cards to 44 Dp with zero flexible height and an explicit layout priority.
- Owner reported transient animation-like card changes while scrolling. Source inspection found no card tween/Animator; inertia was inherited from ScrollRect defaults and row height was not fixed. These are static-UI safeguards, not a proven root cause or visually verified flicker repair. Search, filters, destination selection, taxi navigation and fares are unchanged.
- Two source regression tests observed assertion RED then GREEN. Full suite: 297 passed, 0 failed/skipped. Release built/deployed with 0 warnings/errors; installed DLL/PDB/mod.json hash-matched. DLL SHA-256: `d9ee62c46abc2ffb4c68a4a69040671db6f1d270d727adc16cb49dc4d8cb8fa7`.
- In-game scroll appearance remains open. Startup UI breadcrumb: `[ui] destination scrolling: inertia=False, movement=Clamped; card transitions=None.` Native phone opening/closing and search caret behavior are not modified.

## 0.8.8 (2026-10-13) — Native physics-consumer reverse and park inputs

### Fixed
- Replace generic render-Update reverse/handbrake output writes with an exact-owned LandVehicle.ApplyThrottle prefix. Installed f12 disassembly proves this is the FixedUpdate physics consumer: currentThrottle is consumed here, while handbrakeOverride with overrideControls selects actual wheel braking. HandbrakeApplied/BrakesApplied/IsReversing are resultant flags, not actuator requests.
- During supervised recovery, supply currentThrottle=-0.7 and no handbrake; during explicit parked intent with no active order, supply zero throttle and handbrakeOverride=true. Snapshot and restore all changed inputs through postfix and idempotent exception finalizer, including partial-write rollback. Leave ordinary AI, native steering, cruise50, recovery durations/budgets and fares unchanged.
- Require exact vehicle ownership, host authority, unpaused state and no player driver. Refuse new service if the complete consumer hook is unhealthy. Park intent is pointer-scoped and released on dispatch/control change/reset.
- Throttled `[actuator]` diagnostics report inputs before/at/after consumption and restoration, wheel torque/ground state, signed speed, yaw and steering. Reverse verdict samples native state before our StopReversing call and explicitly labels displacement as unproven escape.

### Verification
- Owner 0.8.7 traces reproduced zero reverse displacement despite a stored negative throttle, repeated forward/back cycles and rolling despite hand=true. Native-input/timing investigation identifies a control-boundary defect, not a proven complete explanation of turn geometry.
- Assertion RED-to-GREEN regression tests cover command selection, hostile render writes, normal/exception restoration, partial writes and ownership/control exclusions. Parent reran full suite: 295 passed, 0 failed/skipped. Nine installed-native byte contracts passed; pure/source tests do not execute Unity physics.
- Release built/deployed as 0.8.8 after confirming no game process and matching Shared hashes: 0 warnings/errors. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `47c267c9be0681377df0fb1843fc98c5298e6fe36b250424e0be0b7de04018e6`. Version sync: 19 mods, 0 drift.
- Native hook activation, park deceleration and escape from the Barn turn remain unverified. Positive-speed watchdog decisions and the callback-Complete/10.8 m arrival mismatch are separate findings, intentionally unchanged. Check `[actuator] healthy=True` and prefix-consume/postfix-consumed wheel readbacks in a fresh session.

## 0.8.7 (2026-10-13) — Taxi-only 50 km/h cruise target

### Changed
- Set the owned taxi's autonomous forward base cruising target to 50 km/h through a VehicleAgent.UpdateSpeed postfix. Native turn, obstacle and destination reductions still execute afterward; 50 km/h is a base target, not a forced actual speed.
- Keep traffic, player-driven vehicles, reversing/recovery windows, stopped/invalid targets and explicit native speed overrides unchanged. No shared DriveFlags, global speed constants, throttle or velocity are modified.
- Reapply on native base-speed calculation so patrol flag resets and redispatches cannot silently restore the old base. Remove the hook on teardown. Log `[cruise]` initialization and per-order native/base/actual speed values.

### Verification
- Installed f12 GameAssembly disassembly confirms targetSpeed uses the same unconverted km/h units as Speed_Kmh at the throttle PID and that native reductions follow the base-speed calculation.
- Actual assertion RED-to-GREEN coverage for cruise selection, safety exclusions and lifecycle wiring; parent reran the full suite: 281 passed, 0 failed/skipped.
- Release built/deployed as 0.8.7 with 0 warnings/errors after confirming the game was closed and Shared hashes matched. Installed DLL/PDB/metadata read back and hash-matched; TaxiDriver.dll SHA-256: `c45aadddfad1573cd72178fd1c2abf765e67e85bfaf942a01d3167f85fcfe2b4`. Version sync: 19 mods, 0 drift.
- Native gameplay and Harmony-hook activation remain pending; tests do not execute Unity. On a fresh game session, inspect `[cruise]` initialization and `targetBase=50.0 km/h` per-order breadcrumbs, then check actual straight-road speed and native braking.

## 0.8.6 (2026-10-13) — Reverse recovery that moves the car; standstill brake hold

### Fixed
- The recovery reverse now drives the game's own pedal path (`LandVehicle.currentThrottle`, negative throttle + reverse gear flag) while its supervised window is pending. Root cause evidence: the game's `StartReverse()` manoeuvre provably often ends without moving the car — Latest.log 2026-10-13 recorded 4x "reverse attempt N did not move the car" with the rear clear, escalating to free-spot rescues and a give-up 213 m short of "Storage Unit".
- The reverse pedal is keyed on the pending re-dispatch window, not on a call site: post-reverse re-dispatch, give-up and STOP all release throttle and reverse gear on the next tick — a reverse left engaged must never keep driving the car.
- Standstill brake hold: `LandVehicle.HandbrakeApplied` provably reverts to False within ~1 s of every park (probe hand=False one second after "brakes + handbrake set" at 08:19:17 and after the give-up park at 08:21:57; "brake action ineffective — the taxi keeps rolling" at -1.4 km/h with brakes set). A per-frame hold re-asserts the game's handbrake flag while the mod has parked the car — never against an active order (the supervised reverse must roll free) and never against the player at the wheel.
- Reverse decisions log measured drive state (autoDriving/agentRev/vehRev/thr/brakes/hand/speed) and the measured displacement per attempt, so the next triage can prove whether the game kept the mod's reverse throttle.

### Verification
- Regression tests observed RED before the fix (missing policy files, then failing wiring contracts); the suite now reports 261 passing tests covering both policies and their SpikeRunner wiring.
- Compiled and deployed as 0.8.6 (hash-verified). In-game acceptance pending. Expected breadcrumbs: `[patrol] ... reversing ... (autoDriving=... thr=...)`, `[patrol] reverse manoeuvre moved X m ...` / `did not move the car (moved X m ...)`, `[hold] the game cleared the handbrake ...`.
- Open risk: whether the game keeps `currentThrottle` against its own throttle loop. If the reverse breadcrumbs show `thr=0.0`, the pedal is being overwritten and the next iteration must drive the wheels directly.

## 0.8.5 (2026-10-12) — Progress-aware navigation timeout

### Fixed
- Replaced the hard 90-second trip cutoff with a 90-second no-progress timeout. Fresh closest-ever approach of at least 2 m renews the timer; redispatch, reversing, rescues and road corrections cannot buy time. Operation-wide recovery limits and pause accounting remain intact.
- Artificial observations consume only artificial approach, preserving accumulated genuine fractional progress between road corrections.
- Navigation failure logs report the actual timeout and operation recovery episodes without claiming the taxi never moved. Fare-end reporting distinguishes give-up, failed/stopped navigation, stopping short and arrival; payment mechanics are unchanged.

### Verification
- Live 0.8.4 Barn test reproduced premature termination at Ray's Real Estate, 82 m short, with $77 reported and a misleading arrived verdict.
- Regression tests observed RED before fixes; current suite has 249 passing tests. Independent review caught the mixed artificial/genuine-progress defect, now covered by five additional RED-to-GREEN cases.
- A legitimate detour with no closest-ever goal approach for 90 seconds can still time out. Native gameplay acceptance of 0.8.5 remains pending.

## 0.8.4 (2026-10-12) — Bounded recovery and dead-taxi teardown

### Fixed
- Recovery episodes consume an operation-wide attempt budget before reverse/rescue branches. Redispatch cannot renew the absolute 90-second deadline; paused time remains excluded and terminal graph rescue is one-shot. Vanilla steering and navigation settings are unchanged.
- Confirmed taxi death invalidates callbacks and stops the existing fare meter without another charge, then releases trunk/patrol and managed runtime state before clearing ownership. Cleanup steps are exception-isolated; incomplete teardown retains the reference for retry. Invalid-handle and deferred-destroy paths cannot bypass this teardown.

### Verification
- 230 tests passed (0 failed/skipped), including 23 new pure-helper and source-contract regression cases. Missing runtime wiring and helper behavior were observed RED before implementation; source contracts do not prove Unity/native gameplay.
- Independent read-only review found no actionable defects. Release build/deploy passed with 0 warnings/errors against installed f12 references. BuildProjectReferences=false prevented unrelated Shared/third-party redeployment after checking Shared hashes matched. Installed DLL/PDB/JSON were read back and matched; metadata reports 0.8.4. TaxiDriver DLL SHA-256: `76efc091bb362fc30265aec91901cf46189b5e90b1848f1418d657829f7d8e0b`. Gameplay acceptance for these fixes remains open; prior driver-safety verification applies to 0.8.3 only.


## 0.8.3 (2026-10-10) — Dedicated driver safety and STOP confirmation

### Fixed

- Removed the nearest-bystander driver fallback entirely. Only the existing registered `taxi_driver` may board; missing/unready drivers refuse the order with an app status message.
- Centralized the 15-second re-entry guard at the genuine native `EnterVehicle` attempt, using exact native identity instead of managed-wrapper equality. A throwing/failed attempt also starts the guard; already-proven seating and pending seat verification do not renew it.
- Rapid reorders are refused before spawning. A driver-step failure performs ownership-checked cleanup; uncertain destruction stays tracked for explicit STOP recovery.
- Unsafe mid-ride re-entry cancels/parks the trip rather than bypassing the cooldown. Reverse, relocation and redispatch paths honor driver-safety cancellation.
- The dedicated driver receives native `_invincible` protection on creation/load/service preparation, with readback. A host order can revive the existing saved dead/knocked-out driver; no replacement NPC is instantiated and no regular NPC is protected or revived.
- STOP confirmation now belongs to the same passenger-ride identity and visible app session. Phone/app close, ride changes and unrelated app actions discard both the approval and its stale instruction.
- Service feedback (refusal / failure messages) is no longer erased by ordinary owned-taxi cleanup or a deferred destroy confirmation. It survives until the next order or a real session/save teardown. A new asynchronous failure now immediately supersedes a still-visible order-success message instead of being masked for four seconds; the protected STOP prompt is unaffected.

### Verification

- Existing baseline: 165 tests passed before this change. Current suite: **207 passed, 0 failed, 0 skipped**; includes pure boarding/confirmation/feedback tests and explicitly limited source-wiring checks. Relevant missing helpers, cooldown bypass, close/new-ride retention, cleanup wiring, backwards-time approval, feedback supersession and feedback-survives-cleanup checks were observed red before implementation.
- Independent code review (fresh subagent, read-only) found no security concerns and one actionable defect: the deferred-destroy `Reset()` erased the pickup-failure feedback. That defect was fixed in this change with its own RED→GREEN regression tests; the reviewer confirmed dedicated-driver identity, cooldown ordering, recovery cancellation guards and STOP confirmation showed no other demonstrable new defects.
- Release build against installed f12 / S1API 3.2.1 stable / S1MAPI 2.0.1: **0 warnings, 0 errors**, with normal automatic deploy enabled. Existing Shared build/deployed DLL hashes matched, so `BuildProjectReferences=false` prevented unrelated Shared/third-party redeployment. Deployed TaxiDriver DLL SHA-256 `d658f1fd230e92389dab273147eb1d66f4e70f5cf60b74ad1711c7152e5dd8bb`; DLL, PDB and JSON were read back and matched their build/source files; deployed metadata says 0.8.3.
- Supplemental Shared regression suite: **71 passed, 0 failed**. That test project's reference triggered a same-byte Shared DLL/PDB redeploy; both were read back and matched source/build artifacts. The third-party target deployed **0** DLLs; S1API's hash remained unchanged. Version/doc-path/skill-health gates and scoped formatting verification passed.
- New behavior **was verified in-game 2026-10-11** (Dominik): the driver-safety/STOP update passed its gameplay acceptance check in a real game session.

## 0.8.2 (2026-10-08) — Beta 8 safety pass

### Controls and retained behavior

- Removed TaxiDriver's F1–F12 runtime bindings and the old `HandleHotkey`/
  `HandleF6` dispatcher. `SpikeRunner.Update` still calls `TickAutoRun`, and
  `TaxiApp` still uses `SpikeCommands.CallTaxi`; CALL TAXI, destinations/search/
  filters, live status/fare, STOP, and navigation between app views remain phone-only.
- Removed the F12 vanilla-vehicle test/takeover path and `FindVanillaVehicle`;
  TaxiDriver never adopts a vehicle it did not spawn and register itself.
- The console is limited to a read-only diagnostic allow-list. Service, route,
  destination, cleanup, and visual-toggle operations are app-only.
- Vanilla E entry/exit and Escape phone-close remain unchanged.

### Safety changes

- Added an exact-pointer ownership ledger for TaxiDriver's own spawned vehicle.
  Cleanup verifies host authority, ownership, occupant exits, destroy request and
  native death. Pending/unknown outcomes retain the reference; no automatic retry
  occurs, and only one explicit STOP retry is allowed.
- Passenger ride startup requires all five input/trunk patches. Input/trunk gates
  fail closed when identity checks throw or are uncertain; handbrake input remains
  blocked during a ride, while the agent's throttle/steering override path remains
  untouched. A partial patch set prevents taxi service from starting.
- `Npc`, `BoardDriver` and `Go` verify exact host-owned taxi identity before
  boarding, navigation or brake/physics changes. STOP invalidates the navigation
  order and pickup state before teardown; callbacks require a current order and
  active polling, so late pickup callbacks cannot complete after STOP.
- GLB visuals are committed only when a renderer has mesh vertices and at least
  one indexed submesh. Import/geometry failure rolls back and leaves vanilla
  visuals visible; vanilla GameObjects and functional components stay active.
- Save/scene teardown preserves uncertain vehicle state instead of dropping its
  ownership token. `TaxiApp.OnDestroyed` removes its update subscription.
- Fare rate and lifetime fare are bounded. Only moving time accrues; cash is used
  first and the bank may go negative. Payment status distinguishes returned,
  thrown/unknown and unpaid parts; returned void calls are explicitly unverified,
  the app displays calculated fare, and consumed amounts are never automatically retried.

### Verification

- Unity-free test suite: **165 passed, 0 failed, 0 skipped**. New ride-gate,
  payment-status, callback and mesh-geometry tests were observed failing before
  their pure policy helpers existed, then passed after implementation.
- Schedule I 0.4.7f12 / local S1API 3.2.1-beta.8 / S1MAPI 2.0.1 no-deploy Release
  build: **0 warnings, 0 errors**. Assembly identities and hashes are recorded in
  `docs/compatibility.md`.
- No mod deployment or current in-game TaxiDriver test was performed. The build
  proves compilation only; runtime ride, lifecycle, GLB, money and multiplayer
  behavior remain unverified.
- Safety text: **F12 drücken | Kein Vanilla-Fahrzeug wird übernommen**. **Keine
  Behauptung vollständiger Beta-8-Kompatibilität ohne passenden Build
  beziehungsweise Laufzeittest.**

## Earlier v0.8.2 iteration (2026-10-03) — superseded by the Beta 8 safety pass

- F9 ride/out toggle removed (E is the only in-game board/exit path; console
  `taxi ride`/`out` stay as diagnostics); F9 now runs the new `taxi diag`
  one-shot ride scan (verdict, ride flags, picker vs active trip, nav state,
  car + driver slot, fare) into the MelonLoader log.
- Recovery re-dispatch refreshes NavRetryAt (full 6 s grace after every
  handover); pause now freezes all timers (auto-run, pending spawn, settle
  check, exit debounce, heartbeat).
- Manual E-boarding with the NPC at the wheel starts a ride (never during a
  pickup or automation run); double board in one frame opens one ride.
- Exits park the car first (brakes + handbrake); STOP/cleanup snaps the
  player to safe ground after a proven exit.
- Trunk lock survives scene loads (dead entries pruned, unlock on the
  scene-load reset); tuning.json is applied once per spawn with readback.
- Hero uses a typed snapshot (no more $32 fare-display freeze); destination
  rows use stable keys against a held catalog snapshot; hero and `taxi status`
  show the active trip while driving (`(default ROAD A)` label fixed).
- Boarding requires the occupant slot, with a bounded 1 s late verdict
  (no EnterVehicle repeat); config migration repairs and rewrites in both
  branches; auto-run spawn guard covers all steps.
- Tests: 61 green (HeroSnapshot boundaries, migration matrix).

## 0.8.1 (2026-10-02) - internal cleanup (file merges)

- Merged `TaxiIcon` + `TaxiCloseExperiment` into `TaxiApp.cs`, `SpikeTrace` into `SpikeCommands.cs` and `TaxiLog` into `Mod.cs` (21 -> 17 files). Code moved verbatim; class names, console words, Harmony targets and config keys unchanged.
- No behaviour change intended. Build 0 errors; 2026-10-02 session smoke: loads clean (log-verified). Phase-2 perf work stays queued until the Pakete C-G test round is done.

## 0.8.0 (2026-10-02) - landscape redesign (PotScanner idiom) + custom checkpoints removed

- **Landscape dashboard** (`EOrientation.Horizontal`): the game turns the phone sideways
  like PocketShop/PotScanner; `UITheme.InitializeForDashboard`. The portrait
  `ScreenSafeInset` math is replaced by the landscape band layout (Dp8 padding).
- **Banded layout in the PotScanner/BankApp design language** (`GamePalette` surfaces,
  `UISprites` two-layer cards — the ad-hoc blue boxes are gone):
  - `[1] Hero (Dp96)`: live state as the headline (`CALL A TAXI` / `TAXI ON THE WAY` /
    `BOARD NOW` / `RIDING TO X` / `ARRIVED`), context line, **fare as the big teal value**
    while riding, app icon on the right; button feedback appears in the hero for a few
    seconds (the separate status box is gone).
  - `[2] Action row (Dp48)`: **CALL TAXI** (green, primary) + **STOP** (red, destructive;
    disabled while no taxi exists).
  - `[3] Filter toolbar (Dp28)`: **All / Homes / Deals / Places** chips (blue = active),
    place count, `✕ clear` chip (enabled only while a destination is picked).
  - `[4] Destination list (flexible)`: two-layer cards, name (computed fit) + type tag
    (STAND/HOME/PROP/DEAL/PARK in palette hues); the picked destination gets the blue
    selection rim + raised fill. Taxi-Stand stays pinned first in every filter.
- **Custom checkpoints removed** (Dominik: "die custom destination bzw rename … passt
  nicht rein"): `CustomCheckpoints.cs`, the `taxi wp` command/help and the `YOU` row
  ("EXAMPLE – rename me …") are gone. `UserData/TaxiDriver/checkpoints.json` is no longer
  read (the file is left on disk untouched). Destination ties resolve owned property →
  property → deal.
- **Quiet diagnostics:** the `[ui]`/`[close]` chatter now goes through `TaxiLog.Verbose`
  (log.json) instead of Info.
- Lifecycle untouched: `_mainBG` deactivate-on-close, OnUpdate wired once, the deferred
  close experiment (Rule 12) as before.

## 0.7.1 (2026-10-02) — Paket 1 v2 + Fare-Icon + Log-Quieting

Basis: Code-Review der Diffs `abb5fc0`/`c984d9c` (GitHub) + Auswertung des Live-Logs
2026-10-01. Lokal gebaut (**0 Fehler, 2 bekannte CS8604**); User-Feedback 2026-10-02:
„taxi funktioniert jetzt sauber“. Roll-up: enthält zusätzlich den Icon-/Log-Batch vom
selben Tag (unten). Kein Commit (wartet auf Dominik).

### Icon + Log-Quieting (gleicher Tag)

- **Fare-Notification zeigt das Taxi-Icon**: `FareMeter` übergibt nicht mehr `null`
  (weißes Quadrat), sondern `TaxiIcon.Get()` — dieselbe `taxi_icon.png` wie das Phone-App-Icon
  (prozeduraler gelber „T“-Fallback, jetzt in `TaxiIcon.cs`; App und Notification teilen
  ein Sprite).
- **Log-Quieting per `UserData/TaxiDriver/log.json`**: neuer Schalter `VerboseLogging`
  (Default `false`, Datei wird beim ersten Start angelegt) + mod-lokales `TaxiLog.Verbose`.
  Standardmäßig still sind die Dauerläufer: `[nav t=…]`-Poll, `[hb]`, Lot-/Stand-Dumps,
  `[pois]`-Detaildump, `[ai]`-Konstanten/-Drive-Model, `[meter]`-Detailzeilen, Patrol-Wiring,
  `[visual]`, `[close-exp]`, `[target]`-Kandidaten. **Info bleibt** für Meilensteine (gerufen,
  angekommen, Ride-Ende/Fare), **Warn/Error immer**; Hotkey- und `taxi`-Diagnosen bleiben
  unverändert. Gemessen am 2026-10-02-Log: 1133 → erwartet ~100 TaxiDriver-Zeilen/Session.

### Fixed

- **Stall-Watchdog blockiert nicht mehr an `navCalc`** (`TickPatrolSupervision`,
  `TickStartupRecovery`): Die Pfadberechnung bekommt eine feste **8-s-Karenz je
  zusammenhängender Phase** (`NavCalcGraceSeconds`) — eine nie endende Berechnung kann
  keinen Stall mehr maskieren, und die Frist verlängert den Stall-Timer nicht.
- **Feste 8-s-Startfrist für Patrol-Dispatches**: Bis ein Fahrstart **bestätigt** ist —
  ≥ 2,5 km/h **gehalten für 0,5 s** (ein einzelner Physik-/Kollisionsimpuls zählt nicht)
  oder ≥ 4 m Netto-Annäherung (Startdistanz − beste Distanz, nicht Summe von Jitter) —
  gehört der Start dem Selbsttest; ohne Bestätigung nach 8 s wird die Patrol einmalig
  freigegeben und die Fahrt fällt auf den Mod-`Navigate`-Dispatch zurück.
- **Fortschritt zeitbezogen**: Das Stall-Fenster setzt nur noch bei **Ø ≥ 5 km/h über das
  Fenster** oder **≥ 2 m Netto-Annäherung** zurück. Das 0,5-km/h-Kriechen (2026-10-01)
  kann den Schutz nicht mehr aushebeln.
- **Reverse-Eskalation**: Ein Reverse, der das Auto nachweislich nicht bewegt, löst einen
  zweiten, längeren (3 s) Versuch aus, danach die Free-Spot-Rescue
  (`RoadKeeper.TryRescueStartup`, Budget 2 pro Fahrt) — nie ein blinder Teleport.
- **Zentraler Stopp** (`SpikeRunner.StopDriving`): Ausstieg, Give-Up und STOP invalidieren
  den Auftrag, geben die Patrol frei, stoppen Reverse + Navigation, parken das Auto
  (Bremsen + Handbremse) und räumen alle Runner-Flags; `ResetNavigation()` läuft jetzt
  wirklich am Fahrtende (vorher nicht).
- **Bremsen-Freigabe beim nächsten Dispatch**: Ein geparktes Taxi
  (`StopDriving`/`ParkCar` → Bremsen + Handbremse) wird vor jedem neuen Ride-Dispatch
  wieder gelöst (`isParked`/`BrakesApplied`/`HandbrakeApplied`) — auch im Patrol-Pfad,
  der das vorher nicht tat (Go() hatte die Freigabe, StartRideDrive nicht).
- **Auftrags-Tokens**: Jeder Dispatch zählt `SpikeState.NavOrder` monoton hoch (wird nie
  zurückgesetzt) und jede `Navigate()`-Closure trägt ihre Order — späte Callbacks werden
  als `stale callback … ignored` geloggt und ändern nichts.
- **Pickup endgültig**: Der Abschluss invalidiert den Pickup-Auftrag, parkt das Auto und
  öffnet erst dann das Boarding-Gate; späte Pickup-Callbacks können nicht mehr wirken.
- **Pickup-Parken**: Der Pickup-Callback ließ das Auto ungeparkt zurück — Verdacht für die
  unerklärten 12 m Leerlauf-Bewegung zwischen Ankunft und Boarding (2026-10-01).
- **Geisterfahrt-/Idle-Wächter**: Solange ein Taxi existiert und **kein gültiger
  Fahr-/Recovery-Auftrag** läuft (inkl. „Pickup erreicht“ und „wartet auf Ziel“, aber nie
  ein allein fahrender Spieler), wird Bewegung > 0,5 km/h für 1 s geloggt (`[probe]` +
  `[guard]`), der Agent gestoppt und die Bremsen gesetzt.
- **Exit-Entprellung**: Das Seat-Flag flackert nachweislich für Einzelframes — der Ausstieg
  zählt erst nach 0,3 s durchgehender Abwesenheit.
- **Erreichbarkeits-Konsistenz**: Die Game-eigene Prüfung (`IsAsCloseAsPossible`) läuft
  VOR dem Routenbau; **ein** aufgelöster Punkt geht in Route, `NavTarget`, Ride-Ziel und
  Drop-off-Notiz (Rebinding beratend, nur ab ≥ 0,5 m Delta; `LogReachability` schreibt
  keinen State mehr).
- **Duplikat-Picks**: Derselbe Zielpunkt während eines laufenden Dispatches ist ein No-Op
  (`duplicate pick ignored`) — der 2026-10-01-Log zeigte sechs Patrol-Neustarts in 25 s.
- **Fare-Migration (echt)**: `fare.json` mit dem Alt-Schwellwert 0,5 km/h wird auf 3 km/h
  umgeschrieben (alte/neue Werte geloggt); der Zähler wird erst mit bestätigter Bewegung
  scharf (Zeit davor verworfen).
- **Agent-Tuning** (Instanz-Felder, nur der Taxi-Agent — kein Spiel-weites Static):
  neu `UserData/TaxiDriver/tuning.json` (`StuckTimeThresholdSeconds`, Default 6,
  0 = Game-Default), angewendet beim Spawn (`[tune]`-Log).
- **Deactivate()-NRE-Diagnose**: Erstes Auftreten je Fehler loggt vollen Stacktrace +
  Wiring-Snapshot (enabled/activeAndEnabled/vehicle/route/wp/isDriving/beh/Pointer);
  Wiederholungen desselben Fehlers sind auf 1 Bericht pro 30 s gedrosselt (Zähler).

### Forensics

- `[probe]`-Zeilen (Order, Frame, Fahrzeug-/Agent-Pointer, Doppelleseprobe
  `AutoDriving`/`navCalc`, Speed, Position, stuck) bei Selbsttest-Fehler, Startup-Stuck,
  Patrol-Stall und Idle-Guard-Stopps.
- `[nav]`-Pollingzeilen tragen `order=`; neu: `[drive]`-Startbestätigungen,
  `[park]`-Parkzeilen, `[guard]`-Geisterfahrt-Zeilen.

### Open

- In-Game-Verifikation (Checkliste in `tests/README.md`).
- Icon-/Log-Paket: Fare-Notification zeigt das Taxi-Icon; `log.json` Default = still,
  `VerboseLogging=true` = voller Diagnose-Stream.
- Die Patrol-Wiring-Tiefenursache (Deactivate-NRE, Klon fährt nicht) ist **nicht** behoben —
  der 8-s-Fallback macht Fahrten davon unabhängig; Snapshot/Probe-Daten der nächsten
  Testsession sind die Grundlage für den Wiring-Fix.

## Unreleased (2026-10-01) - taxi stand spawn faces the street entry

### Fixed

- Live-test fix 2026-10-01 (spawn glitch forward/back under the garage roof):
  `EntryPoint.forward` (+X, east) points INTO the garage — the taxi drove east
  into the pillars, then reversed west 8+ s toward the target. Spawn heading is
  now `-EntryForward` (street/target side, -X west).
- `SnapToGround` no longer grounds on people: NPC capsules and the player
  controller carry no rigidbody, so the old filter accepted an NPC 'Capsule'
  2.1 m up — the taxi fell 1.9 m and bounced. Character colliders are skipped
  (same rule as `RoadKeeper.IsObstacle`), log shows a `character` skip count.
- Post-spawn overlap guard (`EnsurePostSpawnFree`): after the vehicle exists, its
  real position is re-checked with self-ignoring overlap. If it sits inside
  geometry, it is moved to the first free probe point (same 5-7 m offsets + four
  headings); if nothing is free, the spawn is cancelled and the vehicle removed
  instead of leaving a stuck wall-taxi.
- Stand spawn uses the lot entry's own forward (`EntryForward`) instead of the
  indoor parking-spot forward: position (entry) and heading finally match, so the
  taxi no longer spawns facing the garage wall.
- `FindClearSpawn` probes out to 5-7 m (plus 5/2.5 m diagonals) and tries four
  headings per point (0 / 180 / +90 / -90 deg). A side wall or a reversed
  `EntryPoint.forward` polarity still yields a street-facing spawn.
- Stand resolution log now prints `entryForward` vs `spotForward` plus a note on
  negating `EntryForward` if the entry transform points into the garage.

## Unreleased (2026-10-01) - patrol reverse, safe spawn and fare clock

Built in an isolated .NET SDK 8.0.425 environment against a snapshot of the installed
game's IL2CPP/MelonLoader/S1API/S1MAPI assemblies. The live log identifies the game as
0.4.7f7. Release build: **0 errors, 2 existing CS8604 nullable warnings**.
**In-game verification remains open.** No version bump, commit or push.

### Fixed

- Patrol stalls now release the behaviour, check rear clearance, reverse for 1.5 s,
  then dispatch the same target. A blocked rear or failed reverse falls back to
  redispatch in place. Driver re-boarding runs before reversing if Deactivate
  ejected the NPC.
- A short forward sphere probe distinguishes static props from Rigidbody objects
  and character colliders: stationary recovery after 2.5 s for static obstacles,
  otherwise 5 s. Pending path calculation is still allowed to finish.
- Spawn clearance shares RoadKeeper's oriented box (half-extents 1.0/0.6/2.2 m).
  Candidates also need safe ground. Blocked front/clear rear rotates the heading
  180 degrees; no safe candidate cancels the spawn instead of using a blocked origin.
- Initial under-1-km/h, under-0.5-m movement stalls get a one-shot recovery after
  about 3 s (excluding path-calculation time): reverse first; if unavailable or
  ineffective, try a collision-free, grounded road point via TryFindFreeSpot.
- StopPatrol destroys only behaviours added by the mod. Prefab components remain;
  components scheduled for Unity's deferred Destroy are not reused in that frame.
- Navigation callbacks during controlled recovery no longer prematurely stop its
  polling. Pausing also suspends recovery deadlines and RoadKeeper corrections.
- Fare ticks are guarded against duplicate frames and pause/standing intervals.
  Whole movement minutes are floored before multiplying the dollar rate.
  Failed/partial payments are not blindly retried on every frame.
- Fare time uses TimeManager CycleDuration/TimeSpeedMultiplier rather than a fixed
  unscaled-time assumption. Sleep, stopped game time and absent/dead clock handles
  are free; skipped clock minutes are never retrospectively billed.
- Added `[patrol]` recovery/spawn/ownership logs and `[meter]` configuration, motion,
  pause, clock-rate and periodic-payment logs.

### Inspection and tests

- Exactly one FareMeter.Tick caller, in SpikeRunner.TickRide. Mod initialization
  unsubscribes before subscribing the runner.
- Installed fare.json: Enabled=true, DollarsPerInGameMinute=1,
  MovingSpeedThresholdKmh=0.5; left unchanged.
- Previous log: $10 increments around 10 real seconds during continuous driving;
  no double charge established from that log.
- Offline syntax/policy/model checks passed (whole units at rates 1/2/5,
  duplicate frames, pause/resume, standing, time-rate conversion).
- Real physics, runtime clock-rate behaviour, prefab lifetime and money/UI effects
  still require the gameplay checklist in tests/README.md.

## Unreleased (2026-10-01) - driving AI review: less "verwirrt", fewer wall hits

Review of the stuck/hits-objects reports (code review + build; **not yet live-verified**).

### Fixed

- **`ReDispatch` no longer rotates the car toward the target.** The forced yaw ignored the
  road direction and the geometry around the car - the same bug 0.4.0 removed from `Go()`
  ("faehrt gegen Lampen"), still alive in the recovery path.
- **Watchdog window 1.5 s -> 5 s** (`StuckWindowSeconds`): a normal pedestrian/traffic wait of
  the game's own obstacle braking no longer counts as "stuck" (it used to take the wheel from
  the patrol driver and re-dispatch).
- **Patrol supervision waits for the path calculation** (`NavigationCalculationInProgress`)
  instead of releasing the game's driver mid-calculation on long routes.
- **Recovery #1 only reverses with room behind the car** (sphere-cast 3.5 m); otherwise it
  re-dispatches in place instead of backing into a lamp/wall.
- **RoadKeeper hard snap checks the target is free** (box overlap; tries +-4 m / +-8 m along the
  road) and skips the snap, with a throttled `[road] snap-back skipped` warning, when it is not.
- `TaxiAI.DumpDriveModel` compiled against member names that do not exist in the installed
  game assembly (`OBSTACLE_MIN_RANGE`, `OBSTACLE_MAX_RANGE`, `Steer_Rate`,
  `MAX_STEER_ANGLE_OVERRIDE`) - now `ObstacleMinRange`, `ObstacleMaxRange`,
  `steerTargetFollowRate`, `MaxSteerAngleOverride`.

### Verify in-game (open)

- Rides through traffic/pedestrians: the taxi waits instead of reversing/re-dispatching early.
- `[nav] recovery #1: ... not reversing` / `[road] snap-back skipped` lines only near real obstacles.
- If the taxi still hits objects: send `Latest.log` of that ride.

## 0.7.0 (2026-09-29) - destination picker gate + road keeper + dressed driver

Three fixes from Dominik's bug report ("Taxi merkt sich die letzten punkte nach
dem Reload", "fährt in Gruben/Bäume und bleibt stuck", "der Fahrer ist nackt").

### Fixed — stale destination after save reload (Paket A)

- **Root cause:** `SpikeState.RideDestination` (+ goal/drop-off/name) was
  deliberate "picker state" that survived everything — including save reloads
  (MelonLoader statics live as long as the process). Boarding without a pick
  then drove to the *last* destination (`StartRide`'s `?? RideTargetRoadA`
  fallback masked it as a default).
- `SpikeState.ResetPicker()` clears the picker at every save-load boundary
  (`GameLifecycle.OnSaveInfoLoaded`) and when leaving the `Main` scene.
- **No pick, no drive:** `StartRide` refuses to dispatch without an explicit
  pick — the ride waits (`RideAwaitingDestination`), the app shows
  `On board — pick a destination`, and the drive starts the moment a place is
  tapped (`To`/`SetRideDestination` now re-route a RUNNING/waiting ride instead
  of "the NEXT ride").
- The app header is honest: `DESTINATION: none selected — tap a place`.
- The old silent default (ROAD A) is gone from `StartRide`.

### Added — RoadKeeper, the taxi may not leave the road (Paket B)

- New `RoadKeeper.cs`: supervised from `SpikeRunner.Update`, 5 Hz while a
  dispatch is in flight. The corridor is the GAME's own live route
  (`VehicleAgent.path` → `SmoothedPath.vectorPath` polyline) — no guessed roads.
- **Soft lane assist:** drifting past 3 m lateral → small per-tick nudges back
  to the route line (skipped while the game's patrol driver owns the wheel).
- **Hard enforcement:** beyond 5 m lateral, sunk > 2 m below the route line
  ("Grube" — Y gap against the polyline) or on a > 40° slope (HomelessMod
  ground-probe technique) → snap back onto the route line, aligned with the road
  direction, velocities zeroed. Logged as `[road] HARD SNAP`.
- `SpikeState.NavTarget` is never touched — the arrival verdict stays honest.

### Changed — the driver is dressed (Dominik: "der Fahrer ist komplett nackt")

- `taxi_driver` now wears a proper outfit at prefab time (S1API appearance
  layers): white button-up (`Shirts.Buttonup`), dark jeans (`Pants.Jeans`),
  sneakers (`Feet.Sneakers`). Prefab-time layers survive avatar rebuilds.
- The old render-hide (`HideAvatar`/`HideNow`) is removed — the "invisible"
  design never held anyway (the game rebuilt the renderers, which is exactly
  why the underwear driver showed up). The driver is now visible on purpose.

### Added — custom checkpoints + cleaned destination list (Dominik: "wenn 5x Parking gelistet wird, weiß man am Ende nicht wo man rauskommt")

- The destination list now carries only **main checkpoints**: custom checkpoints
  first (tag `YOU`), then deal locations (tag `DEAL`), then uniquely named places
  (tag `PARK`). Generic lot names ("Parking" ×7, "Parking lot" ×2) and duplicate
  names ("Mick's house parking" ×2) are hidden from the LIST — they still serve
  internally as drop-off entries for the arrival rule.
- New `CustomCheckpoints.cs`: named waypoints in
  `UserData/TaxiDriver/checkpoints.json` (SafeStorage, hand-editable,
  comment-tolerant JSON). In-game management via console:
  `taxi wp add <name>` (captures the player position), `taxi wp remove <name>`,
  `taxi wp list`. `taxi to <name>` resolves them and custom names win ties.
- **Properties are first-class destinations** (Dominik: the taxi is the A→B
  shuttle — "ich bin im Motel und möchte zur Barn"): every `PropertyManager`
  property appears with its `ExteriorSpawnPosition` as the drop-off — owned ones
  as `HOME` rows (list-first), unowned as `PROP`. Tie priority: custom > owned
  property > property > deal > lot.
- The first TaxiApp open per session dumps the complete destination table into
  the log (one-shot `[pois]` dump) so the full list can be read off the log.

### Added — the meter, no free rides (Dominik: "ein kostenloses Taxi ist leider nicht Realität")

- New `FareMeter.cs`: **$1 per FULL moving in-game minute** (= per real second in
  motion — time base verified: `TimeManager.CycleDuration` = 24 real min/day, so
  1 real s = 1 in-game min). Whole dollars only ("$1 da es keine Cent-Beträge im
  Spiel gibt"). **0 km/h is free** (standing, jams, arrival wait).
- Meter start = the moment the picked destination turns into a drive; ride end
  (arrival / exit / STOP) reports the total via one HUD notification. The app
  status shows the running fare (`Riding to Barn — $27`).
- Payment: cash first (clamped at 0), remainder to the bank account which may go
  negative (Dominik: "Konto darf ins Minus") via `Money.CreateOnlineTransaction`.
  Multiplayer: money is only touched host/singleplayer-side (fail-closed).
- Config `UserData/TaxiDriver/fare.json`: `Enabled`, `DollarsPerInGameMinute`,
  `MovingSpeedThresholdKmh`. Console: `taxi fare` shows config + live meter.

### Fixed — test round 2026-09-29 (Dominik's in-game findings)

- **Paket E, exit hardening** ("nach E spammen ein/aus beamt sich das Auto ein paar
  Meter weg … einmal beim Aussteigen unter die Map gefallen"): the navigation now
  stops BEFORE `ExitVehicle()` (a live `VehicleAgent` can snap the car onto its route
  at the exit moment — that was the beaming), rapid E in/out is debounced (0.8 s),
  and `SafeExitGroundSnap()` verifies there is ground under the player after every
  exit (static-surface probe; anything > 2.5 m below / > 1 m above road level or with
  no ground at all is snapped to safe ground beside the taxi). One-shot forensics
  `[exit] forensics: … nav-stop delta / ExitVehicle delta` when the car moves anyway.
- **Paket G, driver retention** ("der Fahrer ist ohne Grund ausgestiegen und die Fahrt
  fuhr nicht weiter"): `EnsureDriverSeat` keeps the NPC at the wheel every ride frame
  and re-boards him instantly if he left (with a forensics snapshot: speed, seat slot,
  patrol state — crash ejection vs. vanilla AI). The stuck watchdog re-boards the
  driver FIRST instead of burning navigation recoveries on a driverless car.
- **Paket C, spawn clearance** ("es bugt manchmal rum wenn es an der Mauer kommt beim
  Parkplatz"): the spawn point is probed for cabin-zone clearance (two static-surface
  spheres) — blocked spots move along/sideways to the first clear candidate (11 probes),
  never worse than before.
- **Paket D, clear destination** ("nach Checkpoint-Auswahl bleibt immer der letzte
  markiert"): `✕ Clear selection` row on top of the app's destination list + console
  `taxi clear`. A waiting ride keeps waiting; a running drive keeps its dispatched
  target; only the marker/state is cleared.
- **Paket F**: `Patrol.Deactivate()` only runs on a live behaviour and its NRE noise
  drops to Debug (was one Warn per ride end).

### Acceptance (in-game, open)

- Reload the save → call the taxi → board WITHOUT picking: the taxi waits,
  status `On board — pick a destination`; picking a place starts the drive.
- Reload → the app shows `none selected`, never the previous session's place.
- Watch a long ride: `[road]` lines only appear near obstacles; the taxi no
  longer ends up in ditches or wedged on trees (snap-backs are logged).
- The driver sits at the wheel in shirt + jeans + sneakers.

## 0.6.0 (2026-09-28) - own invisible driver (no more bystanders)

Dominik: random NPC as driver ends, and after dismiss the taxi must be gone.

### Added

- New `TaxiDriverNPC.cs`: dedicated S1API NPC (`taxi_driver`, physical, spawns
  at the taxi stand, waits there via schedule, no customer/dealer role, no
  badge). Avatar renderers are switched off after creation — the taxi looks
  driverless while the game's patrol behaviour drives it.
- `SpikeCommands.Npc()` boards ours first (`NPC.Get<TaxiDriverNPC>()`); the
  nearest-bystander search is only the fallback when ours is unavailable.
- Boarding extracted into `BoardDriver()`: skips `EnterVehicle` when the NPC
  provably already sits in our vehicle (the repeat pattern behind the 57 s
  freeze), otherwise EnterVehicle + AddNPCOccupant fallback + seat proof.
- `Cleanup`: after a successful dismiss-destroy the driver is warped back to
  the stand (`ReturnToStand`) — dismiss = taxi gone, driver waiting.

### Acceptance (in-game, open)

- Call taxi: log shows "ours is taking the wheel", no bystander disappears.
- The taxi looks empty, drives, arrives; dismiss despawns it, nothing left to
  drive yourself.

## 0.5.3 (2026-09-28) - the app's own layout bug (clipped first letters)

Dominik's second screenshot: text now readable, but the first characters of every row are
missing ("xi-Stand" instead of "Taxi-Stand", "eyway behind Top Tattoo").

### Root cause

The name came from the *app's own* RectTransform hand-math: the text rects were positioned
with stretch anchors + `sizeDelta` + `anchoredPosition`, which put the name box left of the
row's left edge; the ScrollRect viewport's `Mask` then clipped the overflow — the ink has
to start left of the row for the first letters to vanish, and only a mask can cut there.
(The same symptom existed before the size change: the old screenshot showed rows starting
with the middle of "DEAL * ".)

### Changed

- Destination rows, the destination header line and the place counter now use
  **`HorizontalLayoutGroup`** children (BankApp's proven idiom: padding + `childControl*`
  + `flexibleWidth` on the growing text, fixed `preferredWidth` on the tag/counter) instead
  of hand-placed rects. The layout engine places the boxes, so nothing can slide out of a
  row. `SingleLineRect` is gone.
- Row tag (`STAND`/`DEAL`/`PARK`) switches colour with the selection: pale grey on bright
  yellow read as "ST…" (the zoom on Dominik's screenshot). Dark olive on the selected row.
- `[ui]` reporting bug fixed: the theme scale is global and gets re-initialised by whichever
  app opens last (the log shows 1.60 and 1.20 initialized 67 ms apart), so the report was
  dividing sizes built at 1.60 by a scale of 1.20. The app now remembers the scale it was
  **built** with and reports with that; it also prints the name column, both font sizes and
  how many rows would still be cut at the minimum font size.

### Acceptance

A screenshot of the app: every row shows its name from the first letter, and `[ui]` reports
`0 still too wide at the minimum font size`.

## 0.5.1 (2026-09-28) - legible app (measured, not guessed)

Dominik: "die taxi app ist noch unleserlich" (with a screenshot of the running app).

### Diagnosis (from the screenshot's pixels, not from taste)

- The log says the app canvas is `655x1201` units and it reaches the screen at ~275x507
  px, i.e. **0.42 px per unit**. The row text was `Sp(11)` = 18 units = **5 px of ink**.
- The comparison that settles it: Weather runs on the same phone (295x537 screenshot,
  same `InitializeForTextApp` scale) and its *smallest* text measures 8 px, its rows
  13 px — that is the size Dominik accepts.
- Nothing was truncated: `UIFactory.Text` uses `horizontalOverflow = Wrap` and
  `verticalOverflow = Overflow`, so the labels were complete — just too small. The
  `"DEAL * "` / `"PARK * "` prefix additionally ate the readable width.
- The app also left a quarter of the screen empty (content ended at y=394 of 521).

### Changed

| | before | after |
|---|---|---|
| row height | `Dp(30)` | `Dp(48)` |
| row text | `Sp(11)` normal | `Sp(18)` bold |
| row layout | `"DEAL * Name"` prefix | name left, small `DEAL`/`PARK`/`STAND` tag right |
| header | `Sp(16)`, `Dp(46)` | `Sp(22)`, `Dp(56)` |
| CALL TAXI | `Sp(15)`, `Dp(68)` | `Sp(20)`, `Dp(72)` |
| destination line | `Sp(11)`, `Dp(28)` | `Sp(15)`, `Dp(42)` + right-aligned "79 places" |
| list panel | `Dp(236)` | `Dp(370)` (uses the empty lower third) |
| STOP | `Sp(13)`, `Dp(52)` | `Sp(18)`, `Dp(62)` |
| status | `Sp(12)`, `Dp(58)` | `Sp(15)`, `Dp(66)` |

- The destination line no longer prints empty parentheses and only shows the drop-off
  *kind* (`> lot entry`); the full sentence stays in the log.
- `TaxiDestinations.Destination.Tag` carries the short row tag.

### Acceptance

Measured the same way as the diagnosis: row text ink height >= 9 px (was 5) in a
screenshot of the same phone size, names not truncated.

## 0.5.0 (2026-09-27) - Stage 4: the game's own driver (the "police AI" clone)

Dominik: "können wir nicht die KI klonen, vom Spiel z.B. die wo das Polizeifahrzeug
steuert".

### Finding

The taxi already drove with the game's autopilot (`VehicleAgent` is the component every
police car uses) — what was missing was the **supervision the game puts on top of it**.
That supervisor is reachable from a mod:

- `VehiclePatrolRoute` is a plain object with a public constructor: `RouteName`, a
  `Il2CppReferenceArray<Transform>` of `Waypoints`, `StartWaypointIndex`. Buildable at
  runtime — no scene asset needed.
- `SetRoute(route)`, `Activate()`, `StartPatrol()` and
  `IsAsCloseAsPossible(Vector3, out Vector3)` are all public on the IL2CPP proxy.
- The numbers are readable static properties: patrol `MAX_CONSECUTIVE_PATHING_FAILURES`
  + `PROGRESSION_THRESHOLD`; pursuit `RECENT_VISIBILITY_THRESHOLD`,
  `CLOSE_ENOUGH_THRESHOLD`, `EXIT_VEHICLE_MAX_SPEED`, `UPDATE_FREQUENCY`,
  `STATIONARY_THRESHOLD`, `TIME_STATIONARY_TO_EXIT`.
- The game's route into it: `PoliceOfficer.StartVehiclePatrol(VehiclePatrolRoute,
  LandVehicle)` + `VehiclePatrolInstance` (Law manager). The pursuit behaviour is NOT
  usable for us — it hangs off a `Player` target and vision events.

### Added

- **`TaxiAI`** (`src/TaxiAI.cs`): builds a two-waypoint route at runtime (where the car
  is, where it should be), attaches `VehiclePatrolBehaviour` to the taxi driver
  (reusing an existing component when the driver prefab has one), then
  `Vehicle`/`SetRoute`/`Activate`/`StartPatrol` — the game drives, the mod supervises.
  Every step is logged *before* it runs, so a hang names its own step.
- **`TickPatrolSupervision`** in the polling: arrival (at the resolved point or standing
  on the game route's last waypoint near it) and stall handling. A stall does **not**
  fight the game's behaviour — it releases it and hands the ride back to the mod's
  `Navigate()` dispatch, so a broken clone degrades to the proven path.
- **`taxi ai`** and an automatic dump on the first ride: every readable number of the
  game's supervision, read live (the console is log-only, hence the automatic dump).
- The mod's recovery ladder now honours the game's own
  **`MAX_CONSECUTIVE_PATHING_FAILURES`** as its re-dispatch budget.
- `IsAsCloseAsPossible` invites the game's own arrival verdict into the log (warning
  only — an unproven call must not be able to refuse a ride).

### Changed

- Ride dispatch goes through `StartRideDrive` (patrol first, `Navigate()` fallback);
  `StartRide` and `ReRoute` share it.
- `EndRide` releases the game's behaviour with the ride — it is attached to the NPC, so
  `DestroyVehicle` alone would have left it running.

### Open

- **Unproven:** whether `AddComponent<VehiclePatrolBehaviour>` on a plain (non-police)
  NPC is accepted, and whether the driver NPC's own schedule behaviour fights the
  patrol. The fallback covers both; the `[patrol]` log names the failing step.
- The rest of the game's driving numbers are dumped but **not** acted on — their units
  would have to be guessed (e.g. whether `STATIONARY_THRESHOLD` is km/h or m/s), and
  guessing is what produced the bad dispatches in the first place.
- `RepathDistanceThresholdMap` is an instance `AnimationCurve` on the pursuit behaviour
  (no static read).
- Still not used from the game's driving model: `StartReverse`/`StopReversing`,
  `GetForwardObstacle`, `UpdateSpeedReduction`, `RefreshSpeedZone`,
  `OverrideMaxSteerAngle`.

## 0.4.0 (2026-09-27) - Stage 3d: real destinations + no more "stuck" rides

Dominik's report: "Taxi KI ist sehr unklug, fährt in den skatepark (wird stuck) oder
fährt gegen lampen etc. nachdem man taxi kündigt und im taxi ist ist das spiel stuck".

### Added

- **`taxi pois` / `taxi to <name|index>`** and a destination catalog
  (`TaxiDestinations`): every live `DeliveryLocation` (the game's own deal locations —
  `LocationName`, arrival anchor `TeleportPoint`, fallback `CustomerStandPoint`) plus
  every `ParkingLot` street-side `EntryPoint`. No hand-read coordinates any more.
- **Arrival rule** (`TaxiDestinations.ResolveArrival`): goal on the vehicle graph
  (<= 6 m) → drive it directly; otherwise the nearest lot entry within 60 m → drop off
  beside it ("a taxi does not have to stop at the doorstep"); otherwise off-graph
  within 20 m → accept with a warning; otherwise refuse. The ride is never dispatched
  blindly into geometry.
- **Taxi app destination list** (replaces the ROAD A / ROAD B / STAND buttons): a
  scrollable list of the catalog (stand first, then deal locations, then lots), a
  "DESTINATION: <place> (<drop-off rule>)" header, and the selected row highlighted.
  Rows are rebuilt on every app open (the catalog walks the scene).
- **`taxi ride <place>`** — picks the destination and starts the ride in one command.
- **Progress watchdog** (`SpikeRunner.RecoverStuck`): movement is measured, not flags.
  Recovery #1 reverses a wedged car 1.5 s then re-dispatches (or re-dispatches
  immediately when the agent never started / has no path), recovery #2 falls back to
  another lot entry near the goal, then the navigation gives up with an actionable
  message instead of 90 s of silence.
- **Ride heartbeat** (`[hb]`, 1 s) while a ride runs or the player sits in the taxi:
  a broken heartbeat = the main thread is blocked, a continuing heartbeat = only the
  state is wrong. This is what today's "the player was frozen but the game ran"
  report could not be told apart from.

### Changed

- **STOP despawns the taxi** (Dominik: "Kündigen soll das Taxi despawnen lassen").
  It runs the proven cleanup order — ride state + input/trunk locks off, NPC out
  (verified), player out (verified), `StopNavigating()`, `DestroyVehicle()`, state
  cleared. A failed exit refuses the destroy (state stays retryable), and the player
  exit is now verified with the same freeze guard the NPC path already had.
  The chosen destination survives the despawn.
- `Cleanup(reason)` takes the reason so `taxi stop` logs as a stop, not as a cleanup.
- **No graph teleport** on a failed path calculation
  (`teleportToGraphIfCalculationFails = false`): the teleport was the only way the car
  could end up somewhere it never drove to (suspected "suddenly inside the skatepark").
- **No forced pre-dispatch orientation** — the snapshot turned the car to face the
  target regardless of the road direction ("fährt gegen Lampen"). The agent picks the
  heading; reversing now belongs to the watchdog.

### Fixed

- **The 9.6 s standstill**: `if (autoDriving) return;` in the polling swallowed every
  recovery path. Log proof (2026-09-27, ride to ROAD A): `AutoDriving=True
  navCalc=False speed=0.0 km/h distToTarget=93.5m callback=-` unchanged for 9.6 s,
  `stuck=False` throughout — only the 90 s timeout could have ended it.
- **Version drift**: the deployed assembly still reported 0.2.0 while this file already
  documented 0.3.0 (passenger mode). `MelonInfo`, `Source/Mods/TaxiDriver/docs/mod.json` and this changelog are
  now aligned at 0.4.0.

### Open

- Why the game never calculated a path for that ride (`callback=-`, target on the graph
  with a 0.1 m delta) is still unexplained — that lives inside the game. `taxi trace`
  (Harmony on `Navigate` / `CalculatePath` / `NavigationCalculationCallback` /
  `StopNavigating`) is the instrument for a reproduction run.
- The reverse recovery can only be exercised with a genuinely wedged car (crash into
  geometry) — not yet observed in a real session.
- Ebene 3 of the driver work (steering authority / speed reduction in turns) is not
  implemented yet.

## 0.3.0 (2026-09-26) - Passenger mode

### Added

- Passenger ride flow (Stage 3c): order the taxi (CALL TAXI / `taxi call` / F5) — the NPC
  boards and drives to a road point near the player, the status flips to
  "Board to ride (E)" and the game's `E` entry (or F9) starts the ride. The NPC drives to
  the picked destination — ROAD A `(-131.4, -4.0, 51.9)` / ROAD B `(-17.1, 0.0, 13.4)` /
  STAND (the fixed taxi stand coordinate) — brakes + handbrake hold on arrival
  ("Arrived — press E to exit") and the exit re-arms the next board. `taxi stop` aborts a
  ride anywhere.
- Shared ride entry points (single source of truth for the ride kernel and the TaxiApp):
  `SpikeCommands.StartRide` / `EndRide` / `SetRideDestination`, plus `FindSeat` /
  `EnsurePassengerSeat` and the `Ride()` driver-seat pre-reserve boarding fix.
- TaxiApp destination picker row (ROAD A / ROAD B / STAND, the selected one highlighted,
  `ButtonUtils.AddListener` wiring) and the ride-aware live status ("Taxi arriving" →
  "Board to ride (E)" → "Riding to <destination>" → "Arrived — press E to exit").
- Ride locks (`RideLocks`, PatchGuard prefixes): during a ride the player's car inputs are
  gated (`LandVehicle.UpdateThrottle` / `UpdateSteerAngle`, `GameInput.OnVehicleHandbrake`
  — the NPC drive path is untouched) and the trunk stays shut
  (`StorageDoorAnimation.Open` / `SetIsOpen(true)` gate + trunk/Storage interactables
  disabled; the vehicle-entry interactable stays enabled so boarding keeps working).

### Fixed

- Floating taxi at spawn: the ground-snap now places the root via the deterministic
  `LandVehicle.boundingBox` (`y = hitY + (rootY - boxMinY)` — the model bottom, not the
  root, touches the road) instead of the culling-stale `Renderer.bounds`; a `[snap]` line
  per spawn and a one-shot `[settle]` diagnostic ~2 s later report the exact heights.

### Verified

- The TaxiApp visual default (`TaxiVisual.VisualSwapEnabled = true`) is intended — the
  0.2.0 "default-off" wording came from a stale `TaxiDriver.dll` deploy and is retired.

## 0.2.0 (2026-09-26) - Taxi phone app

- New **"Taxi" phone app** (`TaxiApp.cs`, discovered automatically by S1API): order the taxi from the in-game phone — open the phone, tap **Taxi**, press **CALL TAXI**. No more keyboard needed for the normal order flow.
- The call-taxi flow is extracted into `SpikeCommands.CallTaxi(caller)` — the **single source of truth** shared by the F5 hotkey and the phone button (same run-start guard, same one-shot stand arm, same automation state block; the two can never drift apart).
- **STOP** button runs the same code path as the `taxi stop` console command (`SpikeCommands.Stop()`).
- Live status label ("No taxi" / "Run in progress" / "Taxi active — press STOP or ride along"), derived from `SpikeState` and refreshed with a cheap string compare; button outcomes show for a few seconds, and every button is scene-gated ("Only available in gameplay." outside the main scene).
- New app icon `Source/Mods/TaxiDriver/assets/taxi_icon.png` (128×128, deployed to `Mods\`), with a procedural fallback sprite so a missing file never logs errors.
- F1–F12 hotkeys unchanged — they remain the diagnostic control surface.

## 0.1.1 (2026-09-26) - F1-F12 hotkey remap

- **Hotkeys remapped F13-F17 -> F1-F5** (the keyboard only has F1-F12, so the
  old F13-F17 keys were physically unreachable): F1 = `taxi go` to the proven
  road target (-131.4, -4.0, 51.9), F2 = `taxi go` to the second proven road
  target (-17.1, 0.0, 13.4), F3 = full run (spawn -> npc -> go) to the road
  target (-131.4, -4.0, 51.9), F4 = visual-swap on/off toggle (applies to the
  next spawn), F5 = call-taxi (spawn at the fixed taxi stand -> npc -> navigate
  to a road point near the player). F6-F12 keep their old keys and semantics.
- **Scene gate for the hotkeys:** every taxi hotkey now only fires in the
  gameplay scene (`S1Mods.Shared.SceneGate.IsInMainScene`) — menu scenes keep
  their own keys (e.g. MoreSaveSlots binds F2/R on the save screens).
- **Removed the SendInput/focus_key.py note** from the init log and `taxi help`:
  it only existed because F13-F17 needed real key input; all hotkeys now fit
  F1-F12 (VK below 0x7C), which synthetic key events deliver fine.

## 0.1.0 (2026-09-25) — Stage 1 spike + Stage 2 visual swap

- **Initial version.** Dev tool that proves the three primitives of the future
  taxi mod on Schedule I 0.4.7f6 (IL2CPP, MelonLoader):
  (a) autonomous A→B driving via `VehicleAgent.Navigate`,
  (b) an NPC boarding the spike vehicle (`NPC.EnterVehicle(null, veh)` with a
  `LandVehicle.AddNPCOccupant(npc)` fallback), and
  (c) local player in/out via `LandVehicle.EnterVehicle()` /
  `LandVehicle.ExitVehicle()`.
- **Console command `taxi`** (S1API `BaseConsoleCommand`, auto-discovered):
  `help`, `codes`, `spawn [code]`, `npc`, `ride`, `out`, `go [distance=40]`,
  `stop`, `status`, `cleanup`. A leading `/` resolves to the same handler.
- **`taxi codes`** enumerates `VehicleManager.Instance.VehiclePrefabs`
  (`VehicleCode` / `VehicleName` / `VehiclePrice`) — the vehicle code catalog
  used by later stages.
- **`taxi spawn`** places the vehicle 6 m in front of the player with a downward
  `Physics.Raycast` ground snap, **`playerOwned=true`** (required so
  `ShouldBePhysicallySimulated` passes and the vehicle can drive), guarded by
  `InstanceFinder.IsServer` and `VehicleManager.GetVehiclePrefab(code)`.
- **`taxi npc`** selects the nearest living NPC from `NPCManager.NPCRegistry`
  (skips dead and already-seated NPCs) and dumps the full seat state
  (`LandVehicle.Seats` → `gameObject.name`, `isDriverSeat`, `isOccupied`,
  `OccupantNPCs`, `GetFirstFreeSeat()`).
- **`taxi go`** builds
  `NavigationSettings { endAtRoad, ensureProximityToGraph, teleportToGraphIfCalculationFails }`,
  releases the vehicle first (physical simulation, `ExitPark`, brakes, agent),
  snaps the target to the vehicle graph (25 m cut-off), turns the vehicle toward
  the target and calls `agent.Navigate(target, settings, callback)` through the
  interop's implicit `NavigationCallback` operator (a rejected callback dispatch
  falls back to `callback = null`); `taxi go2` is the same dispatch with
  `settings = null`.
- **Navigation polling** in `SpikeRunner.Update` (hooked idempotently into
  `MelonEvents.OnUpdate`): 0.5 s heartbeat with `AutoDriving`, target/actual
  distance, `IsOnVehicleGraph()`, `GetIsStuck()`, `Speed_Kmh` and the
  `ENavigationResult` callback value; final log when `AutoDriving` turns false
  (final vs. start distance + duration, ARRIVED vs. STOPPED SHORT) and a warning
  plus `StopNavigating()` after a 45 s timeout that also covers a path calculation
  which never finishes. Per-frame exceptions are logged once per episode and the
  report re-arms after a clean tick.
- **Console is log-only (no input field):** every `taxi` command above is
  output-only — the MelonLoader window cannot be typed into, so the spike is
  controlled with hotkeys (ignored while `S1API.Input.Controls.IsTyping` is true):

  | Key | What it runs |
  | --- | --- |
  | **F6** | full run: `spawn → +1 s npc → +1 s go 40` (blind `forward * 40`) |
  | **F7** | `taxi probe` |
  | **F8** | `taxi trace` on/off toggle (four Harmony prefixes) |
  | **F9** | `taxi ride` / `taxi out` toggle |
  | **F10** | `taxi go2` — `Navigate` with `settings=null` |
  | **F11** | full run without the npc step (occupant hypothesis) |
  | **F12** | `Navigate` on a vanilla (not spawned) vehicle — vehicle-vs-caller A/B |
  | **F13** | `taxi go` to the proven road target `(-131.4, -4.0, 51.9)` |
  | **F14** | `taxi go` to the second proven road target `(-17.1, 0.0, 13.4)` |
  | **F15** | full run (spawn → npc) to the road target `(-131.4, -4.0, 51.9)` |

  While an automatic run or a deferred respawn is pending, the keys are ignored
  with a logged reason. F13–F15 need real key input (`SendInput`); the generic
  computer-use key path drops those virtual-key codes.
- **F6 run kernel** drives the sequence on a `Time.unscaledTime` step kernel with
  a per-step `try/catch` and abort messages that name the failing step.
- **Deploy:** `Directory.Build.targets` copies `TaxiDriver.dll` to
  `<GameDir>\Mods\` and `mod.json` + `TaxiDriver.pdb` to
  `<GameDir>\UserData\TaxiDriver\` on every Release build.
- **Documented deviations:** the deployed S1API 3.x assembly has no
  `[ConsoleCommand]` attribute, no `Aliases` property and no
  `ConsoleHelper.Print`; `NPC.WasCollected` and `NPC.CurrentVehicleSeat` do not
  exist in 0.4.7f6 (`CurrentVehicleSeat` lives on `Player`). Registration relies
  on S1API's `BaseConsoleCommand` auto-discovery (public parameterless
  constructor), answers go through the `[TaxiDriver]`-prefixed MelonLoader
  logger, dead NPCs are filtered with `NPC.Health.IsDead`, and the seat result
  is proven with the `LandVehicle.Seats` dump.
- **Code-review fix round (2026-09-25, same version):**
  - *Freeze guard (blocker):* `taxi spawn` never destroys and respawns in the same
    frame — the respawn is deferred by 0.5 s behind a verified cleanup
    (`NPC.ExitVehicle()` proven via `IsInVehicle`, `OccupantNPCs` emptied through
    `RemoveNPCOccupant`); `taxi cleanup` refuses `DestroyVehicle()` while NPC
    occupants remain and only clears the state after a successful destroy; `taxi npc`
    holds a 15 s re-enter cooldown for the last boarded NPC; F6 logs the start of
    step 2 with `Time.frameCount`.
  - *Retry measurement window:* the `settings = null` retry gets a 6 s grace window
    before the terminal branch may fire (it used to be cut off after 0.5 s).
  - *NPC seat proof:* boarding counts as proven only when `IsInVehicle`,
    `CurrentVehicle` and an `OccupantNPCs` slot agree — which also makes the
    `AddNPCOccupant` fallback reachable; the occupant array is logged as
    capacity vs. filled (the length is the seat count, not the occupancy); a
    one-line seat proof reports whether the driver seat is proven; NPCs with
    `Health == null` are skipped and no longer shown as alive.
  - *Spawn snap:* the ground ray starts above the spawn point (was: above the
    player), skips triggers and rigidbody holders, and logs the hit object + layer.
  - *Navigate callback:* `agent.Navigate(target, settings, callback)` through the
    interop implicit `NavigationCallback` operator (the documented "delegate-ctor
    pitfall" does not apply), with a `callback = null` fallback; polling state is
    written before the dispatch.
  - *Diagnostics:* new `taxi probe` (Flags/Seekers/graph-sample deltas/ownership for
    every vehicle) and `taxi trace on|off|status` (four Harmony prefixes via
    `PatchGuard`: `VehicleAgent.Navigate`, `NavigationUtility.CalculatePath`,
    `VehicleAgent.NavigationCalculationCallback`, `VehicleAgent.StopNavigating`).
  - *Minor:* typed `LandVehicle.GUID` instead of reflection, `State` logged by
    runtime type name, no blind `prefabs[0]`, teardown in `OnDeinitializeMelon`,
    stale-state clearing on scene load, `/taxi` alias registered next to `taxi`,
    help/docs corrected to a 0.5 s heartbeat (was 2 s).
- **Live verification (real-game session 2026-09-25):** after 13 test rounds the
  stage-1 spike is proven end to end — spawn + NPC + path + drive + arrival,
  `[nav] callback result=Complete after 20.2 s` for the F15 full run to the road
  target `(-131.4, -4.0, 51.9)`, no game freeze (freeze guard exercised).
- **Known limitations of the verified build:**
  - `driverSeat.isOccupied` stays `False` for an NPC driver — the NPC proof comes
    from `LandVehicle.OccupantNPCs` (plus `IsInVehicle` / `CurrentVehicle`).
  - End tolerance ≈ 8.3 m with `endAtRoad = true`; `ArrivalThresholdMeters` was
    raised 5 m → 10 m so the non-callback verdict does not report a complete run
    as STOPPED SHORT.
  - The MelonLoader console is log-only (no input) — all `taxi` commands are
    output-only; F6–F15 are the control surface.
  - F13–F15 only via real key input (`SendInput`); the computer-use key path drops
    those virtual-key codes.
- **Second fix round (2026-09-25, same version):** `SpikeState.AutoTarget` is now
  cleared on every reset (an F11 run no longer silently inherits F15's road target);
  the shared `TryBeginGo` gate makes F6/F10–F15 ignore keys while a run or a
  deferred respawn is pending (previously only F6/F11/F12/F15 were guarded);
  hotkeys are handled in ascending F6→F15 order; F8 toggles through
  `SpikeTrace.Logging` instead of a second private flag; `taxi status` prints
  `AutoTarget` + `SkipNpcStep`; `taxi help` and the init log document the log-only
  console and all ten hotkeys.
- Build verified: Release, 0 warnings / 0 errors.

### Visual model swap (Stage 2, same version 0.1.0, 2026-09-25)

- **The spawned vehicle now shows `taxi.glb` instead of the vanilla `Shitbox`
  visuals** — vanilla physics, `Main Collider`, `Roof Collider`, the four wheel
  objects with their wheel colliders, rigidbody and `VehicleAgent` are untouched
  (V1 principle: swap pixels only).
- **New file `Source/Mods/TaxiDriver/src/TaxiVisual.cs`:** `SwapAfterSpawn` runs at
  the end of `SpikeCommands.SpawnVehicle` (no extra hotkey) and
  - loads the GLB with `S1MAPI.Gltf.GltfLoader.LoadFromFile` from
    `UserLibs\S1MAPI_Il2cpp.dll` — resolver order `UserData\TaxiDriver\assets\taxi.glb`
    → `Mods\TaxiDriver\taxi.glb` (build deploy of `Source/Mods/TaxiDriver/assets/*.glb`)
    → `Mods\TaxiDriver\assets\taxi.glb`, first hit logged;
  - parents it under `LandVehicle.vehicleModel` (fallback: the vehicle `gameObject`)
    with `localPosition = 0` / identity / scale 1, forces its 21 renderers on and
    strips its colliders (they would become compound colliders of the vanilla
    rigidbody);
  - switches the vanilla visuals off by rule: children **with renderers and no
    colliders** get `SetActive(false)` (`Main_LOD0..2`, lights, indicators,
    `SmokeParticles`), children **with renderers and colliders** stay active and only
    lose renderer/`LODGroup` visibility (`TrunkLid`, `Wheel FL/FR/RR/RL`), children
    **without renderers** (`Main Collider`, `Roof Collider`, `TrunkGrid`,
    `TrunkInteraction`, `OwnedVehiclePoI`, `VehicleSound`) are untouched; nothing is
    destroyed, so repair/repaint keep working;
  - excludes the GLB from every hide pass via `Transform.IsChildOf`.
- **Auto-alignment** (`SpikeState.VisualAutoAlign`, default on): bounds-based
  ground/xz alignment with a 30 m per-renderer size guard and a **5 m plausibility
  guard** — Unity refreshes `Renderer.bounds` only during culling, so on the spawn
  frame the pooled instance still reports its old bounds (measured `(57.2, -0.6,
  -33.3) m`); such a delta is rejected and the GLB keeps `localPosition = 0`.
- **New console command `taxi visual [on|off]` / `taxi visual align [on|off]`**
  (also `/taxi visual`) — status plus the two Stage-2 switches; `taxi help` and the
  command description list it.
- **Asset:** `taxi.glb` (45 KB, 21 nodes, 8 materials, ~4.4 m × 1.8 m) is now checked
  in under `Source/Mods/TaxiDriver/assets/` (build deploy) *and* installed at
  `UserData\TaxiDriver\assets\taxi.glb`.
- **Live verification (2026-09-25, session 4):** F15 full run logged
  `[visual] swap DONE ... renderers=21, colliders stripped=0, vanilla children
  deactivated=10, renderer-hidden(kept active)=5, untouched(no renderers)=6, errors=0,
  align rejected as stale — GLB stays at local zero`, and a chase-camera screenshot
  from that live session (Hermes cache, since pruned — no path to quote) shows the
  low-poly yellow boxy taxi with roof sign on the road instead of the vanilla Shitbox. A second swap in the same session (cleanup → respawn) also
  succeeded.
- **Known limitations:** GLB wheels do not rotate (the animated vanilla wheels are
  hidden), vanilla paint/damage/smoke is no longer visible, trunk/POI/sound nodes
  still follow the vanilla body footprint, auto-alignment is inert on this build
  (stale bounds), `taxi visual off` only applies to the next spawn, the GLB is
  re-loaded on every spawn.

### Stage-2 review fix round (2026-09-25, same version 0.1.0)

- **`F16` hotkey toggles the visual swap** (`SpikeState.VisualSwapEnabled`, same
  single-source-of-truth pattern as F8/trace) — logs `F16 → visual swap ON|OFF
  (applies to next spawn)`. No `TryBeginGo` gate: it never starts a run. F16 is
  documented in `taxi help`, the init log, the README hotkey table and here;
  F13–F16 remain reachable only through real key input (`SendInput`,
  `focus_key.py`, F16 = VK `0x7F`).
- **`taxi status` prints the four Stage-2 fields** (`VisualSwapEnabled`,
  `VisualAutoAlign`, `VisualRoot` name or `<none>`, `VisualSwaps`).
- **GLB normalisation:** the vehicle's layer is copied onto the whole GLB subtree,
  `SetActive(true)` is normalised across it before the renderer pass, and a swap
  that ends with 0 renderers warns
  `[visual] WARNING: 0 renderers on GLB — model will be invisible`.
- **Failure rollback:** `VisualRoot` is registered as soon as the GLB is parented
  and a vanilla visibility snapshot is taken before the first mutation; a later
  exception destroys the half-attached GLB and restores child `activeSelf`,
  renderer and `LODGroup` state, so `vanilla visuals left as-is` is literally true
  (previously only the physics was untouched — the pixels could stay hidden).
- **Stale-root guard:** "already attached" only skips when the tracked root hangs
  under the *current* vehicle's `vehicleModel`; otherwise the stale reference is
  cleared and the new vehicle is swapped.
- **`taxi visual align <unknown>`** now answers with a warning and changes nothing
  (an unrecognised value used to be silently coerced to `off`).
- **Counting fix:** the final hide sweep no longer re-counts renderers the
  per-child pass already handled — the DONE field is now
  `final-sweep-only renderers hidden=N` (was: `other renderers hidden=N`, which
  summed the same renderers twice).
- **Docs:** `SwapInternal`/`Vec` XML docs, help head + init log now say
  "Stage 1 spike + Stage 2 visual swap", `ResolveAssetPath` documents its
  warning as *per call* (a miss is not cached), README/AGENTS/mod.json describe
  Stage 2 (Visual-Swap via `S1MAPI.Gltf.GltfLoader`, `Source/Mods/TaxiDriver/assets/taxi.glb`, F16).
  Version stays **0.1.0**.
- **Live verification (real-game session 2026-09-25, session 5, PID 13692):**
  F16 → `F16 → visual swap OFF (applies to next spawn)` → F15 logged
  `[visual] swap disabled ... vanilla visuals kept` and the capture shows the
  vanilla sedan at the spawn spot; F16 → `ON` → F15 logged `swap DONE ... final-sweep-only
  renderers hidden=0` and the same spot shows the yellow taxi GLB (A/B capture pair —
  both screenshots live in the Hermes session cache, since pruned; no file paths).
  Enter/exit with the swapped vehicle (F9 ride → F9 out, screenshot taken while
  inside, same pruned cache): the soft-hidden vanilla children (wheels, `TrunkLid`) stayed hidden — no
  vanilla panel or extra wheel appeared next to or through the GLB after exiting.

### Stage 3 — NPC drives, player rides along (same version 0.1.0, 2026-09-25)

Scope V1: **vanilla NPC as driver** (no NPC model swap — that stays a later
extension).

- **Driver-seat proof replaced with a measurement.** The seat proof used to end in
  `driver-seat occupancy NOT proven` because it judged the NPC with
  `VehicleSeat.isOccupied`, a flag that can only ever be set by a *Player*
  (`VehicleSeat.Occupant` is typed `Player`, confirmed with `ilspycmd` against the
  installed `Assembly-CSharp.dll`). `PrintSeatProof` now reports the driver-seat
  **index**, the `OccupantNPCs` slot and the **root-to-seat distance (NPC root
  transform → every seat transform)** (`ClosestSeatIndex`, `SafeDriverSeatIndex`),
  and states the result honestly, e.g.
  `[after enter] seat proof: seat[0] isDriverSeat=True ... root-to-seat [0]=0.0m
  [1]=0.8m [2]=0.8m [3]=1.1m -> closest seat[0] (0.0m) -> NPC root sits on the
  DRIVER seat ... -> driver seat claimed. VehicleSeat.isOccupied=False is expected:
  VehicleSeat.Occupant is typed Player ...`.
  `DumpSeats` prints the same root-to-seat distances, and **`taxi ride` now dumps
  the seat state after boarding** (`[after ride]`) so the Player occupancy flip is
  part of the record.
- **Driver seat semantics documented** (`docs/README.md` → *Driver seat
  semantics*): `VehicleSeat.Occupant` is `Player`-typed ⇒ `isOccupied` is
  Player-only; NPC occupancy lives in `LandVehicle.OccupantNPCs`;
  `LandVehicle.IsOccupied` (vehicle flag) *is* `True` with only the NPC aboard
  while every seat flag stays `False`; `GetFirstFreeSeat()` still returns
  `seat[0]` with the NPC seated and `seat[1]` once the player holds `seat[0]`;
  `OccupantNPCs[i] ↔ Seats[i]` is consistent with all measurements but only
  verified for the single-driver case (0.4.7f6 has no per-NPC seat index).
- **Ride with player aboard — live verified (run 1, 16:39–16:43):** ⚠ **Pruned
  source: this run's log has since been deleted by MelonLoader — the lines below are
  a transcript, not re-greppable evidence** (run 2, further down, is). `F15` at 16:39:16.499 → spawn at
  `(-74.4, 4.0, 84.9)` → NPC `benji_coleman` seated (proof: body-to-seat
  `[0]=0.0m`) → `F9` ride at 16:39:18.173 (F15 + 1.7 s, after the npc step) →
  `[primitive c] PROOF: LocalPlayerIsInVehicle=true` → `Navigate` dispatched →
  polls show `onVehicleGraph=True` and `speed=2.5 → 20.6 km/h` with the vehicle
  position advancing across the run (≈150 m of road with the player aboard) →
  after two further `F13` dispatches (45 s budget each) **`[nav] callback result=
  Complete after 14.5s`, last poll `distToTarget=5.8m`** → `F9 out` at
  16:43:21.310 → `[primitive c] PROOF: LocalPlayerIsInVehicle=false` — the player
  is standing at the destination (≤ 8 m from the raw target).
- **Ordering F15 vs. F9 proven in code and in the run:** `Cleanup()` (first thing
  `F15` does) calls `LandVehicle.ExitVehicle()` whenever `LocalPlayerIsInVehicle`
  is true, and during cleanup + the 0.5 s deferred respawn `SpikeState.Vehicle`
  still references the vehicle being destroyed — so `F9` must come **after**
  `F15` (after step 1/2), never before it. Documented in the README instead of a
  code change: nothing needed fixing, the existing guards behave as designed.
- **New known limitations (README):** the player takes `seat[0]` as well and
  overlaps the NPC (no public seat-selection API); a `forward * 6` spawn can wedge
  the car in a wall corner (`speed=0.0` for 45 s, `callback result=Stopped` twice —
  with *and* without a passenger, so the passenger is not the cause; re-orienting
  the player so the spawn lands on open road fixed it); the 45 s navigation budget
  needs a re-dispatch for trips ≳ 100 m; and the driver is not observable from
  outside while the Stage-2 GLB is on (`taxi.glb` `Glass` material is OPAQUE,
  `alpha = 1.0`) — at 04:00 in rain the vanilla cabin is pitch black too, so the
  numeric seat distance stays the authoritative "who sits where" evidence.
- **Second run re-verified from a log that is still on disk**
  (`MelonLoader/Logs/26-9-25_17-23-11.log`; MelonLoader prunes older logs at each
  start; quoted lines keep the label as logged — `body-to-seat`, which the spike now
  prints as `root-to-seat`): `F15` 17:32:51.896 → NPC `chloe_bowers` seated (body-to-seat `[0]=0.0m`)
  → `F9` ride 17:32:53.981 (`[primitive c] PROOF: LocalPlayerIsInVehicle=true`,
  `[after ride] seat[0] ... playerOccupant='Dominik'`, NPC still
  `OccupantNPCs[0]` at 0.0 m) → polls `[nav t=0.5s … 31.3s]` in which the position
  advances across the run (stationary for consecutive polls while reversing at
  ≈0 km/h; 2.0 → 20.6 km/h; `distToTarget` 26.9 m at the first poll → 5.3 m at the
  last, peaking at 33.1 m around t=24 s) →
  `[nav] callback result=Complete after 31.6s (frame=51317)` → `F9 out`
  17:34:39.578 → `[primitive c] PROOF: LocalPlayerIsInVehicle=false` with the
  player on foot beside the car at the target. The full block is quoted in the
  README (*Ride with player*, "Second run").
- **Build:** Release, 0 warnings / 0 errors. Version stays **0.1.0**.

### Stage 3b — taxi stand + F17 order flow (same version 0.1.0, 2026-09-25)

- **Taxi stand `ParkingGarage` at `(-3.2, 0.0, 82.0)`** — parking-lot dump lists
  33 lots, `ParkingGarage` is the only garage candidate. Stand resolution logs
  `configured coordinate within 10m (0.1m)`, spawn `(-13.0, 0.0, 84.2)` forward
  `(0, 0, 1)`; `Shitbox` spawned at `(-13.0, 0.2, 84.2)` with a Foundation
  ground snap (2.9 m); visual swap DONE (21 renderers, 10 deactivated).
- **F17 flow live-verified:** NPC `chloe_bowers` (3.6 m) boards (`IsInVehicle`,
  `OccupantNPCs[0]`, root-to-seat 0.0 m); road target CHOSEN label `player`,
  destination `(-122.4, -3.9, 64.6)`, snapDelta 1.8 m, distanceToPlayer 1.8 m,
  22 candidates, `endAtRoad` / `ensureProximity` / `teleportIfFail`; polls
  `distToTarget` 20.9 → 6.9 m, `onVehicleGraph=True`, `stuck=False`,
  `reversing=False`, 11–20 km/h; arrival `taxi arrived at player
  (callback=Complete after 46.9s, 6.3m from resolved target
  (-122.4,-3.9,64.6))`.
- **Timeout 45 s → 90 s** (`SpikeRunner.NavigationTimeoutSeconds`): two earlier
  dispatches stopped at 13.2/13.7 m on the 45 s budget; 90 s covered the run
  with 46.9 s. **Arrival threshold unchanged at 10 m** (6.3 m < 10 m).
- **Side finding:** mod menu did not load on the first beta click, loaded on
  the second. No W/S self-drive claim is made here.

### Etappe-3 review fix round (2026-09-25, same version 0.1.0)

- **B1 — the seat proof no longer states unproven facts** (`PrintSeatProof`):
  `driver seat claimed` additionally requires an `OccupantNPCs` slot (`slot >= 0`) —
  on `slot == -1` the line now reads `driver seat NOT proven (seating incomplete)`
  instead of `OccupantNPCs[-1] holds it`; `seat[i] isDriverSeat=True` is only printed
  for `driverIdx >= 0` (otherwise `driver seat NOT identified (driverIdx=-1, ...)`);
  the passenger case (`closest != driver`) uses neutral wording and implies no
  `OccupantNPCs[i] <-> Seats[i]` mapping; the XML docs now say *measures* vs
  *assumes* and spell out the claim guard.
- **Label unification:** `body-to-seat` → `root-to-seat` (measured from
  `npc.transform.position`) in the spike output, `taxi help`, XML docs, command
  table and docs prose; quoted live-log lines keep the label the build printed at
  the time (noted in the README *Driver seat semantics* and at run 2).
- **Help / command description cover Stage 3:** the help head, `taxi npc` (seat
  proof), `taxi ride` (`[after ride]` dump) and the `BaseConsoleCommand`
  `CommandDescription` now mention Stage 3.
- **Evidence corrections in the docs:** run 1 (16:39–16:43, `Complete after 14.5 s`,
  log since pruned by MelonLoader) carries an inline pruned-source marker at the
  start of its table and of the CHANGELOG bullet, and run 2 (`Complete after 31.6 s`,
  log on disk) is the headline number in `README.md`, `AGENTS.md` and `mod.json`;
  `position changing on every poll` was replaced with `position advances across the
  run (stationary for consecutive polls while reversing at ≈0 km/h, identical
  positions at t=5.6/6.1/6.6 s and t=14.2/14.7 s)` and the `distToTarget 26.9 → 5.3
  m` endpoints now carry their 33.1 m peak around t=24 s; `LocalPlayerIsInVehicle=
  true for the whole trip` is stated as an absence (no `ExitVehicle` logged between
  the ride at 17:32:54 and the out at 17:34:39); chase-camera captures are labelled
  as pruned Hermes-cache screenshots; the run-2 quote's two joined log lines are
  split into their own timestamps, the missing full stop on the `F15 pressed` line
  is restored and the two truncations carry an explicit `…`.
- **README structure:** the intro list gains the Stage-3 point, `Known limitations`
  is scoped `Stages 1–3` and references the still-open `OccupantNPCs[1] <-> Seats[1]`
  follow-up; `mod.json` describes Stage 3 (driver ride, root-to-seat 0.0 m,
  `Complete after 31.6s`) and refines the known limitations (NPC occupancy = slot +
  measured distance, index mapping not observable / single-driver only). Version
  stays **0.1.0**.
