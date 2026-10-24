# Historical snapshot — 2026-10-24

These dated test counts, beta API references and unexecuted-at-the-time checklists are historical. TaxiDriver 1.0.0 was owner-accepted on 2026-10-24 without itemized actions; that does not turn each old item into a separate observed pass. See the [current test entry](../../tests/README.md) and [compatibility](../../../../../docs/compatibility.md#verification-matrix).

Original: `Source/Mods/TaxiDriver/tests/README.md`. Original SHA-256: `49c0d527088ce631d457674ef11054453e98226f890ab073e54dd07043cc80fd`.
The original document bytes are retained after the marker below.

<!-- preserved-document:start -->
# TaxiDriver verification

## Driver-safety and STOP regression pass, 2026-10-10

The current suite has **207 passing tests** (baseline before this fix: 165). `DriverBoardingGateTests` exercises exact native identity, strict re-entry timing, failed-attempt reservation, invalid clocks and non-extending refusal. `StopConfirmationTests` exercises same-ride approval, three-second expiry, close/hidden synchronization, new rides, invalid/backwards clocks and immediate non-ride STOP. `ServiceFeedbackTests` exercises override expiry, immediate supersession by a new failure, protection of the STOP prompt, empty-status and same-revision no-ops, and invalid-clock fail-closed behaviour.

`DriverSafetySourceContractTests`, `StopConfirmationSourceContractTests` and `ServiceFeedbackSourceContractTests` are structural wiring checks, not native runtime reproductions. They verify that the app and native helpers invoke the tested seams, that bystander selection is absent and that cleanup no longer erases the service feedback. They cannot prove invincibility, revival, seat timing, safe world exits or real NPC appearance in Unity.

### In-game acceptance (new fixes: not yet run)

- Attack the dedicated driver: protection should prevent death/knockout; no replacement or regular NPC should take the wheel.
- Use the prior test save where the driver was killed: CALL TAXI should restore that same registered driver or refuse clearly, never borrow someone else.
- CALL TAXI → STOP → CALL TAXI within 15 seconds of the last genuine entry: expect a remaining-wait message, no new taxi from the refused order, and no cooldown bypass warning.
- Reorder after the window expires: normal pickup and a single driver should work.
- During a ride, STOP once → close/reopen phone quickly → STOP once: expect a fresh confirmation. Repeat by closing only the app.
- Confirm STOP twice without closing: cancellation/ownership-checked cleanup should still work.
- Force a missing driver during the cooldown: expect cancelled navigation and parked taxi; native E exit and STOP remain available. No old reverse/redispatch may restart it.
- Save/menu/load cycle: only the registered driver receives protection and no old confirmation can cancel the next ride.

## Unit tests (pure logic)

The extracted Unity-free logic is covered by `Source/Tests/TaxiDriver.Tests`
(no game assemblies required):

```pwsh
dotnet test Source/Tests/TaxiDriver.Tests/TaxiDriver.Tests.csproj -c Release
```

Current pure-logic coverage includes `VehicleOwnershipLedger` (owned/foreign/
invalid state, pending/uncertain destruction, confirmed death and bounded retry),
`RideProtectionPolicy` (complete patch set, fail-closed input/trunk gates),
`NavigationCallbackPolicy` (only current callbacks during active polling),
`TaxiVisualPolicy` (nonempty indexed-mesh and zero-renderer fallback), `TaxiConsolePolicy` (read-only
allow-list), `FareLedger`/`FarePayment`/`FarePaymentStatusLedger` (accrual, cash-first split,
API-returned versus balance-unverified status, unknown/unpaid totals, exceptions
and no repeated attempt), and `FareConfigRules`/`FareClock`
(configuration and numeric bounds). The in-game checklist remains the source of
truth for Unity-bound behavior.

## Beta 8 source verification, 2026-10-08

- `dotnet test Source/Tests/TaxiDriver.Tests/TaxiDriver.Tests.csproj -c Release`:
  **165 passed, 0 failed, 0 skipped**.
- Test-first checks for the ride input, handbrake and trunk fail-closed policies
  first failed because the new pure policy methods were absent; after implementing
  those methods, the focused `RideProtectionPolicyTests` passed (11/11).
- Payment-status tests went red for the missing status ledger/formatter and for
  a client-only status hiding an earlier result; after the fix, the focused
  `FarePaymentStatusLedgerTests` passed (5/5).
- Navigation callback tests went red before the callback policy existed, then
  passed (3/3); visual geometry tests went red before the mesh-data gate existed,
  then passed in the focused suite (8/8).
- No-deploy Release build against installed Schedule I 0.4.7f12 / S1API
  3.2.1-beta.8 / S1MAPI 2.0.1 references: **0 warnings, 0 errors**.
  `-p:S1NoDeploy=true` was set; the mod was not installed or deployed.
- This verifies source compilation and pure logic only. No current TaxiDriver
  in-game test was run; do not claim full Beta-8 gameplay compatibility.

## Historical build check, 2026-10-01

Older result: Release build against the then-installed references had 0 errors
and 2 CS8604 nullable warnings in SpikeCommands. This is not the current f12
build result.

## Beta 8 in-game acceptance checklist (not yet run)

| Test | Status |
|---|---|
| Press the base game's F1–F12 controls | Not run in-game; static scan found no TaxiDriver F-key binding. |
| CALL TAXI button | Not run in-game; source still routes `CallTaxi` → `TickAutoRun`. |
| Taxi arrives and registered `taxi_driver` boards | Not run in-game. |
| Vanilla E entry/exit | Not run in-game. |
| Destination selection, search, filters and app navigation | Not run in-game. |
| STOP exits occupants and removes the owned taxi safely | Not run in-game. |
| Order taxi twice without an unintended duplicate | Not run in-game. |
| F12 does not adopt a vanilla vehicle | Not run in-game; foreign-vehicle takeover path and `FindVanillaVehicle` are absent. |
| Missing/empty GLB keeps vanilla visuals visible | Policy test passes; visual fallback not run in-game. |
| Close and reopen TaxiApp | Not run in-game; `OnDestroyed` unsubscribes its update handler. |
| Switch saves/scenes without stale ride state | Not run in-game; local lifecycle signatures compile. |
| Money API throws or returns an unknown result | Pure payment tests pass; native Money API behavior not run in-game. |
| Schedule I 0.4.7f12 / local Beta-8 API build | Passed, no deploy; see `docs/compatibility.md`. |

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
