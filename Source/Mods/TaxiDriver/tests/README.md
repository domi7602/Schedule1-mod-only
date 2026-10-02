# TaxiDriver verification

## Build check, 2026-10-01

- Release build against installed game references: 0 errors, 2 existing CS8604
  nullable warnings in SpikeCommands.
- Syntax checks and offline meter policy/model tests passed.
- This is not an in-game verification.

## Gameplay checklist

- Spawn parallel to a wall: oriented vehicle-sized box must reject overlaps.
  If no safe candidate exists, expect `[patrol] spawn cancelled`.
- Spawn facing a wall with rear clear: expect the logged 180-degree spawn heading.
- Start without moving: about 3 s after path calculation finishes, expect startup
  reverse, or grounded/free-point rescue if reverse cannot free the vehicle.
- Drive into a lamp/tree: expect a static-obstacle stall at about 2.5 s, rear
  clearance check, 1.5 s reverse, then redispatch to the same destination.
- Block the rear: expect no reverse. Never relocate into an occupied point.
- Traffic/pedestrians: retain the normal 5 s stationary window; a path
  calculation is not treated as a collision.
- Reverse while paused: no elapsed manoeuvre time, no RoadKeeper repositioning,
  no meter charge. Resume should not charge paused time.
- Reroute twice in the same frame: no reuse of a behaviour already scheduled for
  Destroy. Prefab behaviours must be preserved when stopped.
- Check driver remains seated during recovery.
- Continuous normal-speed motion with the current config: $1 per full movement
  unit, approximately $10 for 10 real seconds; stationary intervals are free.
- Sleep/fast-forward: inspect `[meter] TimeManager` rate and sleep/clock-stop logs.
  Sleep skips must not bill hours of time instantaneously.
- Exit, STOP, reload a save, return to menu: no stale recovery or fare state.

## Evidence to capture

Keep `MelonLoader/Latest.log` immediately after a test. Include `[patrol]`,
`[meter]`, `[driver]` and `[nav]` lines plus the relevant obstacle, route and
pause/time-speed settings. Do not claim a gameplay pass from the build alone.

## Paket 1 v2 checklist (2026-10-02)

| Test | Success criterion |
|---|---|
| Bungalow six times | One dispatch; five `duplicate pick ignored`; no repeated "releasing the game's patrol driver". |
| `navCalc` stays active | Documented fallback (`start self-test failed` → mod dispatch) no later than 8 s after the dispatch. |
| Crawl only, no drive start | Startup protection stays armed; no fare (`standing (FREE)`, never `meter armed`). |
| Pickup reached, do not board | Taxi holds (parked); a drift logs `[guard]` + is stopped — none of the unexplained 12 m drift. |
| Exit during a calculation | One ride end (`[drive] stopped`); a later callback logs `stale callback … ignored`. |
| Arrival at the destination | Route ends, drive stops, taxi stays parked (`[park]`), no ghost motion. |
| Speed spike > 2.5 km/h, then standstill | No start confirmation from the impulse (needs 0.5 s held) — protection stays effective. |
| New ride right after a ride end, then an old callback | The new ride is unaffected (order tokens). |
| Pause during startup/calculation | No recovery in the pause; after resume only the remaining time runs. |
| Spawn | `[tune]` log shows `StuckTimeThreshold 15 -> 6` (per-agent); `UserData/TaxiDriver/tuning.json` exists. |

The `[probe]` double-read (`autoDriving1/2`, `navCalc1/2` + pointers in one line) is the
evidence for the old `[nav]`/`[hb]` flag mismatch — include it if a stall fires.
