# Two ride-logic questions — research pack for Perplexity (2026-10-03)

NOTE FOR THE RESEARCHER: you can read the repository (public, branch
`feat/tests-core-logic`):
https://github.com/domi7602/Schedule1-mod-only
Relevant files (read these first, they are authoritative over any quote below):
- Source/Mods/TaxiDriver/src/SpikeCommands.cs (`StartRideDrive` at line 2131)
- Source/Mods/TaxiDriver/src/SpikeRunner.cs (`TickRide` arrival at line 1044,
  `TickPatrolSupervision` at 1311, `GiveUpNavigation` at 1519,
  `LogAgentProbe` at 423)
- Source/Mods/TaxiDriver/src/SpikeState.cs (all mod state)

Context you do NOT have: the game is "Schedule I" v0.4.7f6 (Unity 2022.3,
IL2CPP). The following are GAME classes the mod only calls — DO NOT GUESS
their internals: `LandVehicle` (`Agent`, `Speed_Kmh`, `BrakesApplied`,
`HandbrakeApplied`, `EnterVehicle`, `ExitVehicle`), `VehicleAgent`
(`Navigate`, `StopNavigating`, `StartReverse`, `StopReversing`,
`AutoDriving`, `IsReversing`, `GetIsStuck`), `VehiclePatrolBehaviour`
(`SetRoute`, `Activate`, `StartPatrol`, `Deactivate`,
`IsAsCloseAsPossible`), `VehiclePatrolRoute`, `NPC`. Answer only the
numbered questions.

## A. Ghost ride when re-boarding at the destination (log-proven)

Log excerpt (verbatim, timestamps trimmed):
```
[ride] ARRIVED at Barn — 3.0 m from the target (within the 10 m arrival threshold, callback=Complete)
[ride] ended (player exited) — input/trunk locks released — next board (E) starts a new ride.
[ride] trunk locked ... [ride] passenger seat fix applied ...
ride-kernel — passenger ride to Barn: input + trunk locks engaged, 'taxi_driver' keeps the wheel.
[meter] meter started ...
ride-kernel: the GAME's patrol driver owns the ride to Barn ...
[patrol] ARRIVED at Barn — waypoint 0/1, 2.3 m to the target, speed=0.1 km/h (within the 10 m threshold).
[patrol] Deactivate() failed (NullReferenceException inside the GAME's Deactivate)
[driver] the driver LEFT the wheel mid-ride — re-boarding him now
[meter] ride ended (arrived) — fare $0 for 0 moving in-game minute(s).
[ride] ARRIVED at Barn — 2.3 m from the target (within the 10 m threshold, callback=-)
```

Mechanism: exit at destination re-arms boarding; instant re-board starts a
full second ride to the same target 2.3 m away. Patrol attaches, reports
arrival without driving, `Deactivate` throws (game-side), the driver is
ejected and re-boarded — all for a $0 ride. Proposed fix (unimplemented):
in `StartRideDrive`, if already within the 10 m arrival threshold of the
destination, skip the dispatch (no patrol attach) and let the existing
`TickRide` arrival branch declare ARRIVED next frame.

Research questions:
A1. Is "already there, declare arrived without dispatching" sound against
the failure mode it guards (a stale/rebound target that LOOKS reached but
is not)? What condition distinguishes "standing at the true target" from
"navigation ended early near it" using only position, target, threshold,
callback result, and give-up flag?
A2. Skipping the patrol attach also skips the game's arrival bookkeeping.
What could plausibly break if a game-owned patrol behaviour is never
attached for a ride that ends immediately (orphaned route objects, driver
AI leftovers), and how would you verify absence of side effects using only
log output?

## B. Car rolls backward after give-up despite brakes (log-proven)

Log excerpt (verbatim, one line per ~1 s for 12 s, positions trimmed):
```
[nav] GAVE UP after 2.6 s ... and 5 recovery attempt(s) — the taxi did not move (213.2 m left ..., stuck at (172.7, 0.6, -14.6)).
[meter] ride ended (arrived) — fare $2 ...
[ride] GAVE UP at Pizzeria parking lot — 213.1 m from the target ...
[guard] the taxi is moving without an active order (speed=-2.6 km/h, attempt 1/3 ...) — stopping it.
... repeats at -2.3, -2.0, -1.9, -2.0, -2.4, -2.9, -3.1, -2.9, -2.7, -2.3, -1.3 km/h ...
[guard] the taxi keeps moving without an order — press STOP to despawn it ... (4x ERROR)
```

Mechanism: after the give-up, the arrival branch sets brakes + handbrake,
yet the car rolls backward at a near-constant -2.5 km/h for 12+ s while the
idle guard fires powerlessly (it sets the same ineffective brakes). No
`StopReversing() failed` line precedes it. Hypotheses: (H1) slope overcomes
the brake force; (H2) the agent is stuck in reverse gear from an earlier
recovery (`StartReverse` without effective `StopReversing`).

Research questions:
B1. For a Unity wheeled vehicle that rolls at near-constant speed with
brake + handbrake flags set: how do you distinguish slope-roll from
stuck-reverse-gear using only logged telemetry (speed sign constancy,
`IsReversing`, `AutoDriving`, `GetIsStuck`, obstacle reading)? Which two
additional values would you log first?
B2. A watchdog whose only action (set brakes) is provably ineffective in
this state spams 10+ warnings. What is the standard pattern for a
supervision ladder whose actuator failed: escalate (which escalation, given
destroying under a seated player is forbidden), back off with decay, or
hand the decision to the user once? Discuss log-noise vs stuck-forever
trade-offs.
