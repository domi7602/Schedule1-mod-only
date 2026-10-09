# TaxiDriver fixes — work plan (2026-10-03)

> **Historical plan:** superseded by the 2026-10-08 Beta-8 safety pass. The remaining text records an earlier implementation plan; do not treat its F9/console instructions as current behavior.

Basis: Sonnet review of the 2026-10-03 code-only bug report, plus 7
repo-grounded corrections (greetings from the working tree: every line
below was grepped, not guessed). F9 removal added per Dominik.

Scope: 14 findings + F9 hotkey removal. Game APIs stay untouched; all new
state lives in the mod. No full rewrite of the ride kernel.

## Package 0 — F9 hotkey removal (new request)

Fact: F9 (`SpikeRunner.cs:645-663`) is the ride/out TOGGLE, not a spawn.
Boarding in game also works via the game's own E-enter; the Taxi app orders
via CALL TAXI. Console `taxi ride` / `taxi out` stay as diagnostics.

Changes:
- Delete the F9 block in `SpikeRunner.HandleHotkey` (lines 645-663).
  `SpikeCommands.Ride()` / `Out()` and the console cases
  (`TaxiConsoleCommand.cs:59-64`) stay — still used by console.
- Update texts: `SpikeCommands.cs:127` help line (drop the F9 row),
  `Mod.cs:40` hotkey log line, `SpikeCommands.cs:1539` rearm note
  ("E or F9" becomes "E"), `SpikeRunner.cs:954/965/1624/1633` and
  `SpikeState.cs:161` doc comments mentioning F9.
- Consequence, covered by packages below: native E becomes the only
  in-game board/exit path, so the unified board handler (package 2) MUST
  detect native-E boarding (it does — `TickRide` reads
  `LocalPlayerIsInVehicle`), and the native-E exit path MUST keep the
  nav-stop + snap order (it does via `EndRide` → `StopDriving` + existing
  `SafeExitGroundSnap` at `SpikeRunner.cs:1005`).

Acceptance: F9 does nothing in game; E-board after CALL TAXI starts the
ride (with package 2); E-exit snaps to ground; console `taxi ride/out`
still work.

## Package 1 — Timers: retry stamp + pause shift (findings 1, 10)

File `SpikeRunner.cs`.

1a. `ReDispatch` (~line 1448): set both stamps at dispatch:
```csharp
float now = Time.unscaledTime;
SpikeState.NavStartTime = now;
SpikeState.NavRetryAt = now;
SpikeState.NavRetried = true;
```
(`Go` at `SpikeCommands.cs:1895` already pairs `NavRetried` with
`NavRetryAt = 0f`; reset sites `ResetNavigation`, `SpikeState.cs:248`
already clear it. Verified complete writer list: 1895, 1808, 248.)

1b. `ShiftPauseDeadlines` (lines 268-289): add the missing fields with the
same `+= dt` pattern: `SpikeState.NavRetryAt`, `SpikeState.AutoNextAt`,
`SpikeState.PendingSpawnCode != null ? SpikeState.PendingSpawnAt` (guard
`> 0f` like the rest), `SpikeState.SettleCheckAt`,
`SpikeState.LastExitAt`, `SpikeState.RideHeartbeatAt`.
No early-return of the whole `Update` (would disable hotkeys in pause —
`HandleHotkey` has no pause guard today, keep that behavior). No clamping
of `dt` (a real pause must be excluded fully; clamping would count part of
it as drive time).

Acceptance: recovery dispatch gets the full 6 s grace even with stale or
zero `NavRetryAt`; pausing during retry grace / auto-run / exit debounce
preserves remaining time after resume (log: retryAge frozen across pause).

## Package 2 — Unified boarding (finding 2)

Files `SpikeRunner.cs` (~line 970 `TickRide`), `SpikeCommands.cs:1447`
(`StartRide`).

2a. Extend the board detector so native-E boarding starts a ride without
`RideAwaitingBoard`: fire when `veh.LocalPlayerIsInVehicle &&
!SpikeState.RideActive && DriverAtWheel(veh)` (NPC-at-wheel proof reuses
`DriverAtWheel`, same check `EndRide` rearm uses). Keep the existing
`RideAwaitingBoard` condition as an OR — do not delete the flag yet; both
paths converge on `StartRide("ride-kernel")`.

2b. Double-start guard inside `StartRide`, placed AFTER the preconditions
(correction: setting `RideActive = true` as the first line would corrupt
the no-vehicle / no-NPC failure paths, which call `EndRide` and would then
see phantom `wasRiding`). Order: null/NPC checks first (unchanged), then
`if (RideActive) return true;` then set `RideActive = true` before locks
and dispatch. Same-thread `Update` makes this sufficient; `NavOrder` keeps
protecting only callbacks.

Acceptance: manual E-board with NPC at wheel + `taxi to <place>` drives;
CALL TAXI pickup flow unchanged; double board event in one frame yields one
ride (log shows one `StartRide`); failed board (no NPC) still refuses with
the old message and no phantom ride-end line.

## Package 3 — Exit order: brake, exit, snap, destroy (findings 9, 6)

File `SpikeCommands.cs`.

3a. `Out()` (lines 902-966): after the pre-exit `StopNavigating`, set the
holding brakes before `ExitVehicle` (use existing `ParkCar(veh, …)`, line
1568 — it stops navigation AND sets brakes+handbrake; drop the inline
stop to avoid two writers). Rest unchanged: exit, verify, `EndRide`
rearm, forensics, `SafeExitGroundSnap`.

3b. `Cleanup()` player exit (lines 2348-2371): after the verified
`ExitVehicle`, call `SafeExitGroundSnap(veh)` before the
`StopNavigating`/destroy section — same helper as `Out()` and `TickRide`,
no new logic. Destroy order otherwise unchanged (freeze guards stay).

Do NOT assume game `DestroyVehicle` is end-of-frame (unverified); the
verified-exit-then-destroy order already in the code is the safety, keep it.

Acceptance: STOP on flat road, exit on slope (car holds, no idle-guard
warning), exit beside wall (snap finds side spot), cancel path leaves the
player on ground with the car gone.

## Package 4 — Trunk lifecycle (finding 4)

File `RideLocks.cs` (`LockTrunk` line 172, guard line 174;
`UnlockTrunk` line 251).

4a. At `LockTrunk` entry, prune dead entries before the count check:
drop handles whose object is gone (Unity `== null` on the wrapper type in
try/catch — the `==` overload on IL2CPP wrappers is the one in-game
verified open point; log the prune count once). Non-empty list of LIVE
entries from the SAME vehicle stays a no-op; entries of a FOREIGN vehicle
are restored-then-cleared, never written blindly.

4b. Call `RideLocks.UnlockTrunk()` on the scene-load reset path
(`SpikeRunner.OnSceneLoaded`, before/after `SpikeState.Reset()`).
`UnlockTrunk` is already per-entry exception-safe and ends with `Clear`,
so half-destroyed objects are forgotten, not touched. Do NOT put the
clear inside `SpikeState.Reset` (keeps state and side effects separate).

Acceptance: scene change mid-ride, next ride locks the trunk again
(log: `trunk locked: N …`); normal ride end still restores; double reset
is a no-op.

## Package 5 — Tuning wiring (finding 3)

File `TaxiTuning.cs:65`, caller `SpikeCommands.SpawnVehicle`.

Call `TaxiTuning.Apply(veh)` once per successful spawn (after the vehicle
exists and before any dispatch — end of `SpawnVehicle`, next to the
`TaxiVisual.SwapAfterSpawn` call). Extend the existing log line with
`before`/`want` plus a readback. Do not call per frame (would mask a game
overwrite — which stays an explicit in-game check: set 2 s, wedge the car,
stuck must come early; read back after ~1 s).

Acceptance: `tuning.json` edit changes the agent value in the log;
`0` keeps the game default with the existing message.

## Package 6 — Hero snapshot (finding 7)

File `TaxiApp.cs:741-756`.

Replace the bit-packed `int signature` with a typed snapshot compare, e.g.
```csharp
internal readonly record struct HeroSnapshot(
    bool RideActive, bool AwaitingDestination, bool Arrived, bool GaveUp,
    bool Picked, bool AwaitingBoard, bool ToPlayer, bool AutoRunning,
    bool PendingSpawn, bool Vehicle, string Destination, int Fare, string Override);
```
Compare with `==`, render on inequality, store. (Confirmed: `ChargedTotal`
is `int`, `FareMeter.cs:61/73`; collision period is 32, not 64 —
`32 << 27 == 0` mod 2^32. Sign bit from $16-$31 is harmless under `==`.)

Acceptance (pure test, no game): fares
0,1,15,16,31,32,33,63,64,128,192 each produce a distinct snapshot;
same fare + changed destination re-renders; changed fare + same
destination re-renders.

## Package 7 — Destinations: stable keys + selected vs active (findings 5, 8, 12)

Files `TaxiApp.cs:483-545`, `TaxiDestinations.cs:293`, `SpikeState.cs:321`.

7a. Build ONE catalog snapshot per app open (`Dictionary<string, Entry>`
held by the app). Row key: stable identity — kind + name + rounded goal
(1 m grid); keep the `STAND` key as its own kind. Tap resolves against the
HELD snapshot only: entry alive → use its live position; despawned → error
message + stored position note, never a fresh scene walk. Index becomes
display-only.

7b. Split the state: `SelectedDestination` (picker, next dispatch) vs the
running drive's target (`NavTarget` + kept display name). `ClearDestination`
clears only the picker while `RideActive`; hero keeps showing the active
trip destination with fare ticking. STOP/exit stays the only ride end.

7c. One-liner: `SpikeCommands.cs:2243` `(default ROAD A)` → `(none selected)`
(`SpikeState.NoDestinationName`).

Acceptance: duplicate names resolve without cross-talk; object spawned
after open does not shift taps; despawn before tap errors cleanly; clear
mid-ride keeps showing the active target; status and hero agree.

## Package 8 — Small items (findings 11, 13, 14)

- 11 (`SpikeCommands.cs:771`): require the occupant slot in the
  `BoardDriver` success check; on slot-miss, one delayed recheck next
  frame (bounded, no `EnterVehicle` repeat), then fail loudly. In-game log
  proves whether slots fill late.
- 13 (`FareMeter.cs:98-112`): legacy check (`<= 0.5`) FIRST with file
  rewrite, invalid check (`< 0.1`, NaN/Infinity) second WITH rewrite too
  (shared `changed` flag → single `SaveAtomic`). Second load must be
  migration-free.
- 14 (`SpikeRunner.cs:864`): guard becomes `AutoStep >= 1` while
  `PendingSpawnCode != null` (time margins 0.5 s/1 s are not readiness
  proofs on hitching frames).
- 12 is covered in 7c.

## Tests (pure, `Source/Tests/TaxiDriver.Tests`, no game assemblies)

- Hero snapshot inequality across the fare boundary values (package 6).
- Grace predicate with simulated timestamps incl. pause-shifted values
  (extract the `elapsed`/`NavRetryAt` decision to a pure function first —
  precondition for testing package 1).
- Double `StartRide` same frame → single ride (guard, package 2).
- Config migration matrix: valid untouched/no-write; legacy migrated+saved;
  invalid/NaN repaired+saved; reload clean (package 8/13).
- Picker snapshot: tap after catalog shift resolves the held entry;
  despawned entry errors (package 7).

## In-game regression run (after each package)

1. Spawn + NPC board confirmed. 2. Manual E-board. 3. `taxi to` → drive.
4. Clear picker mid-ride. 5. Force a stall → recovery → grace respected.
6. Pause during grace → resume with remaining time. 7. STOP → safe ground,
car gone. 8. Next ride. 9. Scene change → next ride (trunk locked).
10. Slow frame at auto-spawn. 11. Config repair + restart persistence.

## Commit split

1. Package 0 (F9 removal; texts + hotkey block).
2. Package 1 (timers) + pure grace test.
3. Package 2 (boarding) + double-start test.
4. Package 3 (exit order).
5. Package 4 (trunk) + package 5 (tuning + log).
6. Package 6 (hero) + snapshot test.
7. Package 7 (picker keys + selected/active + status text).
8. Package 8 (11, 13, 14) + migration/picker tests.
9. Full regression run results (no code).

## Done criteria

- Every finding has a pure test or a logged in-game reproduction.
- No start/exit/dispatch path bypasses the shared helpers.
- Late callbacks cannot touch a new vehicle generation (order token;
  generation check optional follow-up, not in this plan).
- Scene change never blocks the next ride's locks.
- STOP leaves the player on safe ground before any destroy.
- UI and sim show the same active trip target.
- Pause preserves remaining time on every timer.
- Config repairs survive a restart.
