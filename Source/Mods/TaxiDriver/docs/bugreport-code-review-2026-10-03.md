# TaxiDriver Bug Report — code-only review (2026-10-03)

Scope: static review of `Source/Mods/TaxiDriver/src` (18 files) plus the
existing tests under `Source/Tests/TaxiDriver.Tests`. No game session, no
`Latest.log`, no in-game verify. Every finding cites the mechanism in the
code; severity is judged by player-visible effect.

Status convention: none of this is in-game-verified. Each HIGH/MEDIUM item
needs one live round before a fix ships.

## Summary

| # | Finding | Severity |
|---|---------|----------|
| 1 | Recovery re-dispatch never refreshes `NavRetryAt`, so the 6 s retry grace does not apply after a patrol handover | HIGH |
| 2 | A manually boarded passenger ride never starts (no pickup, no rearm) | HIGH |
| 3 | `TaxiTuning.Apply` is never called — `tuning.json` is dead | MEDIUM |
| 4 | Trunk lock is skipped for exactly one ride after a scene load mid-ride | MEDIUM |
| 5 | Destination list uses catalog indexes as click keys — a shifted catalog misroutes the tap | MEDIUM |
| 6 | `taxi stop` / `taxi cleanup` player exit skips the ground snap (Paket E path bypassed) | MEDIUM |
| 7 | Hero change signature overflows: fare totals that are multiples of $64 do not refresh | MEDIUM |
| 8 | `ClearDestination` while driving keeps the old dispatched target under a cleared label | LOW |
| 9 | Exited taxi is left unbraked (rollaway on slopes) | LOW |
| 10 | Pause freezes only part of the deadlines | LOW |
| 11 | `BoardDriver` success check ignores the occupant slot it otherwise requires | LOW |
| 12 | `taxi status` prints `(default ROAD A)` although there is no default since Paket A | LOW |
| 13 | Fare thresholds below 0.1 km/h are fixed in memory but never rewritten to disk | LOW |
| 14 | Auto-run pending-spawn guard covers step 1 only | LOW |

---

## 1. Recovery re-dispatch skips the retry grace (HIGH)

Location: `src/SpikeRunner.cs` — `ReDispatch` (sets `NavRetried = true`,
verified near line 1448) vs the settings=null retry
(`NavRetried = true; NavRetryAt = now`, verified lines 1807-1808) and the
grace check (`now - NavRetryAt < RetryGraceSeconds`, verified line 1830).

Mechanism: the first dispatch gets 3 s (`elapsed < 3`) plus, after the
settings=null retry, 6 s of grace — about 9 s total before the terminal
branch can fire. `ReDispatch` (patrol-stall handover, reverse recovery,
free-spot rescue) sets `NavRetried = true` but never sets `NavRetryAt`, so
the 6 s grace is governed by a stale timestamp from an earlier run (or 0
when no retry ever fired). With a stale old timestamp the handover gets
only the 3 s `elapsed < 3` window before `STOPPED SHORT` fires — the mod
dispatch barely gets rolling before the ride is declared dead. With a
recent stale timestamp the opposite happens: undeserved extra grace.

Fix: set `SpikeState.NavRetryAt = Time.unscaledTime` inside `ReDispatch`
(the dispatch it sends IS settings=null, so the grace must start now), or
reset it to 0 and treat 0 as "grace active" — either way, one writer owns it.

## 2. Manually boarded rides never start (HIGH)

Location: `src/SpikeCommands.cs:1447` (`StartRide`), sole caller
`src/SpikeRunner.cs:970` (board detection, requires `RideAwaitingBoard`).

Mechanism: `Ride()` only seats the player; it never calls `StartRide`.
`TickRide` starts a ride only when `RideAwaitingBoard` is true, which is set
only by pickup arrival (`CompletePickup` path / `OnNavigationResult`) or by
exit rearm. Diagnostic flow (spawn, npc, F9 board, `taxi to <place>`) therefore
stores the destination ("takes effect on the NEXT ride") but no ride ever
starts — the player sits in the car with a stored destination and nothing
happens, with no message explaining the dead end.

Fix (design choice): either route manual boarding into `StartRide` when an
NPC is at the wheel, or print an actionable hint on board without
`RideAwaitingBoard` ("order via CALL TAXI / F5 first, or pick a destination
after the pickup arrived"). At minimum `ApplyDestinationToRunningRide`
should not promise a "NEXT ride" that has no trigger.

## 3. `TaxiTuning.Apply` is never called (MEDIUM)

Location: `src/TaxiTuning.cs:65`. A repo-wide grep for `TaxiTuning` returns
only the defining file — no caller in `SpawnVehicle`, `StartRideDrive`,
`Go`, or anywhere else.

Mechanism: `tuning.json` loads, logs, validates — and changes nothing,
because nothing invokes `Apply(veh)`. A player editing
`StuckTimeThresholdSeconds` gets a log line suggesting it loaded, but the
taxi agent keeps the game default. Silent dead config.

Fix: call `TaxiTuning.Apply(veh)` once per spawn (e.g. in `SpawnVehicle`
after the agent exists), or delete the file + docs if the tuning is
unwanted.

## 4. Trunk lock skipped for one ride after a scene load (MEDIUM)

Location: `src/RideLocks.cs` (`LockTrunk` early-returns when
`TrunkInteractables.Count > 0`; `UnlockTrunk`, line 251, is called only from
`EndRide`, verified `src/SpikeCommands.cs:1518`).

Mechanism: `OnSceneLoaded` → `SpikeState.Reset()` never calls
`UnlockTrunk`, so a scene load mid-ride leaves destroyed wrappers in the
static list. The next ride's `LockTrunk` sees `Count > 0` and returns
without locking the new vehicle's trunk; the ride after that clears the
list via its own `EndRide`. Exactly one ride per mid-ride scene load runs
with an unlocked trunk (input gate unaffected — it reads `SpikeState`).

Fix: call `RideLocks.UnlockTrunk()` (or a dead-entry prune) on the
scene-load reset path; alternatively make `LockTrunk` prune collected
handles instead of early-returning.

## 5. Destination rows keyed by catalog index (MEDIUM)

Location: `src/TaxiApp.cs` (`RebuildDestinationList`: row key =
`destination.Index.ToString()`).

Mechanism: the catalog is rebuilt on every open and every resolve
(`BuildCatalog` walks the scene). If the world changes between list build
and tap (a deal location spawns/despawns, a lot appears), indexes shift and
the tap resolves to a different place than the row showed — or out of range
(refused, safe). Names are shown but indexes are trusted.

Fix: key rows by a stable identity (name + kind, or the resolved goal
vector) and resolve by that; keep the index as display only.

## 6. STOP/cleanup player exit skips the ground snap (MEDIUM)

Location: `src/SpikeCommands.cs` — `Out()` snaps (line 964),
`TickRide` exit snaps (`src/SpikeRunner.cs:1005`), `Cleanup()` player exit
does neither (verified: no other `SafeExitGroundSnap` call sites).

Mechanism: Paket E ("fell under the map on exit") was fixed for the
`Out()` and native-E paths, but `taxi stop` / `taxi cleanup` exits the
player with a bare `veh.ExitVehicle()` and destroys the car. Same exit,
same missing ground — the exact reported bug can recur precisely on the
cancel path, where the player is most likely to be somewhere unfamiliar.

Fix: call `SafeExitGroundSnap(veh)` after the player exit in `Cleanup()`
(the vehicle reference is still valid there, before destroy).

## 7. Hero fare display freezes at $64 multiples (MEDIUM)

Location: `src/TaxiApp.cs:752-753`:
`(destination.GetHashCode() & 0xFFFF) << 11 | (FareMeter.ChargedTotal << 27)`.

Mechanism: `ChargedTotal << 27` overflows 32-bit int wrapping (unchecked by
default): 64 << 27 == 0 mod 2^32. A $64 fare produces the same signature
bits as $0, so `RefreshHero` early-returns and the hero keeps showing the
old fare until the next dollar. Same for $128, $192, … — reachable on long
rides at $1/minute. No money bug (the ledger is exact); display only.

Fix: stop bit-packing; compare the fields directly
(e.g. keep last destination string + last total + flags) or use
`HashCode.Combine`.

## 8. Clear-while-driving keeps a stale drive under a cleared label (LOW)

Location: `src/SpikeState.cs:321` (`ClearDestination`) + `src/TaxiApp.cs`
hero (`RIDING` without destination once unpicked).

Mechanism: documented behavior — a running drive keeps its dispatched
target while picker/marker state is cleared. The hero then shows a fare
accumulating toward "none selected". It is the mirror image of the Paket A
stale-target bug (there: driving to a stale pick; here: driving to a pick
the UI says is gone). Arguably correct (don't yank a moving car), but the
UI should keep showing WHERE it is still driving to.

Fix: keep `RideDestinationName` for display on a running drive after clear
(mark it e.g. "(cleared — still driving)"), or end/re-route the drive.

## 9. Exited taxi left unbraked (LOW)

Location: `src/SpikeCommands.cs` (`Out()`: pre-exit `StopNavigating`, then
`EndRide("player exited", stopNavigation: false, rearm: true)` → else branch
releases only the patrol; `ParkCar` is never called).

Mechanism: after every player exit the car sits with brakes off and no
driver input. On a slope it rolls; the idle guard then reports "moving
without an order" and brakes it after ~1 s with a warning — noise at best,
a rolled-away taxi at worst.

Fix: `ParkCar(veh, …)` on the exit path (or set brakes before the exit).

## 10. Pause freezes only part of the deadlines (LOW)

Location: `src/SpikeRunner.cs` (`ShiftPauseDeadlines`).

Mechanism: nav/retry/self-test/progress/idle timestamps are extended each
paused frame, but `AutoNextAt`, `PendingSpawnAt`, `SettleCheckAt`,
`LastExitAt` (exit debounce), and `RideHeartbeatAt` are not. A long pause
can fire a deferred spawn, advance an F6 run, expire the exit debounce, or
emit the settle diagnostic while the game stands still. The F6-during-pause
case is the sharpest (spawn + npc + go while paused).

Fix: extend the same way, or early-return the auto-run/pending-spawn ticks
on `timeScale == 0` like the polling does.

## 11. `BoardDriver` success ignores the occupant slot (LOW)

Location: `src/SpikeCommands.cs:771`:
`if (!best.IsInVehicle || best.CurrentVehicle == null) return false;`

Mechanism: the file's own review history (M3) states
`IsInVehicle + CurrentVehicle` never prove a seat — the slot list does —
and the skip path honors that. But the final success verdict after
`EnterVehicle` + `AddNPCOccupant` fallback checks only the two weak facts,
never `OccupantIndexOf >= 0`. A half-seated NPC (in-vehicle flag set, no
slot) returns true and becomes `DriverNpc`.

Fix: require `slot >= 0` in the success check, or downgrade to a warning
("seated without slot — seat proof weak").

## 12. Status text claims a default that no longer exists (LOW)

Location: `src/SpikeCommands.cs:2243`: `(default ROAD A)`.

Mechanism: since Paket A there is no default — a ride without a pick waits.
The status line still tells the player the fallback is ROAD A.

Fix: print `(none selected)` (`SpikeState.NoDestinationName`).

## 13. Sub-0.1 thresholds never migrate on disk (LOW)

Location: `src/FareMeter.cs:98-112`.

Mechanism: `IsInvalidThreshold (< 0.1)` resets in memory WITHOUT rewrite;
`IsLegacyThreshold (<= 0.5)` rewrites. A stored value below 0.1 (also
legacy by the second rule) is fixed in memory on every load but the file
stays dirty forever.

Fix: run the legacy check first, or rewrite on the invalid path too.

## 14. Pending-spawn guard covers step 1 only (LOW)

Location: `src/SpikeRunner.cs` (`TickAutoRun` top:
`if (AutoStep == 1 && PendingSpawnCode != null) return;`).

Mechanism: after a deferred spawn, step 1 advances to step 2 immediately
while the vehicle still does not exist (fires 0.5 s later). Step 2 is due
1 s later, so it usually wins the race — but on a slow frame the npc step
runs against `Vehicle == null` and aborts a healthy run. Steps 2 and 3
should wait for the pending spawn the same way step 1 does.

---

## Explicitly checked, no finding

- Meter double guard (ride state + vehicle identity) is consistent in
  `Start`, `Tick`, and `Charge` (host-only, fail-closed).
- Destroy/spawn separation (freeze guard) and the re-enter cooldown are
  wired on every path that creates or boards.
- Navigation order tokens are monotonic and never reset; stale callbacks
  and recovery-owned callbacks are ignored.
- `FareLedger` math (floor-then-rate, commit-before-payment, hitch cap,
  latch resets) matches its documented rules; `WholeMinutes` being unset on
  non-counted ticks is dead but harmless (no reader).
- Stand forward negation is Garage-specific but `FindClearSpawn` tries four
  headings, so a wrong polarity on a fallback lot is recovered, not fatal.
- `RoadKeeper` idling while the patrol owns the wheel without an
  `agent.path` is a supervision gap, not a bug (no false snaps — the
  conservative direction).

## Method note

File-only review: all "verified" line numbers above come from grep against
the working tree with uncommitted WIP present (MessagesPlus files
modified; TaxiDriver tree untouched per `git status`). No behavior was
executed. Suggested next step: confirm #1 and #2 in one live round
(patrol-stall handover log + manual board/`taxi to` sequence), then fix in
priority order 1, 2, 6, 3, 5, 7.
