# TaxiDriver findings — research pack for Perplexity (2026-10-03)

NOTE FOR THE RESEARCHER (Perplexity): you cannot read the source repository.
All code you need is quoted verbatim below. Anything game-specific that you
cannot verify on the web is marked DO-NOT-GUESS. Answer only the numbered
research questions; do not invent game API behavior.

## Shared context (applies to every item)

- Language: C# 12, nullable enabled, default (unchecked) arithmetic.
- Runtime: Unity 2022.3 with IL2CPP scripting backend, modded via MelonLoader
  0.7.3 on .NET 6. Managed code calls into ahead-of-time compiled game code
  through generated interop wrappers.
- Game: "Schedule I" v0.4.7f6 (Steam). The following are GAME classes the mod
  only calls and cannot change — DO-NOT-GUESS their internals:
  `LandVehicle` (with `Agent`, `Speed_Kmh`, `EnterVehicle`, `ExitVehicle`,
  `BrakesApplied`, `HandbrakeApplied`, `Seats`, `OccupantNPCs`,
  `DriverPlayer`, `localPlayerSeat`, `LocalPlayerIsDriver`,
  `LocalPlayerIsInVehicle`, `IsPhysicallySimulated`, `boundingBox`),
  `VehicleAgent` (with `Navigate`, `StopNavigating`, `StartReverse`,
  `StopReversing`, `AutoDriving`, `NavigationCalculationInProgress`,
  `IsReversing`, `GetIsStuck`, `IsOnVehicleGraph`, `StuckTimeThreshold`),
  `NavigationSettings`, `NavigationUtility` (with `SampleVehicleGraph`,
  `CalculatePath`), `VehiclePatrolBehaviour` (with `SetRoute`, `Activate`,
  `StartPatrol`, `Deactivate`, `IsAsCloseAsPossible`, `CurrentWaypoint`,
  `isDriving`), `VehiclePatrolRoute`, `NPC` (with `EnterVehicle`,
  `ExitVehicle`, `IsInVehicle`, `CurrentVehicle`), `ParkingLot`,
  `DeliveryLocation`, `StorageDoorAnimation`.
- `S1API` is the community modding API; `SpikeState` is the mod's own static
  state holder; `Time.*` is standard Unity (`Time.time` scaled,
  `Time.unscaledTime`, `Time.timeScale`, `Time.frameCount`,
  `Time.unscaledDeltaTime`).
- Quoted code is verbatim from the repo. Line numbers cited are informational.

---

## 1. Recovery re-dispatch never refreshes the retry timestamp (HIGH)

Finding: after a recovery re-dispatch the 6-second retry grace is governed
by a stale timestamp, so a freshly dispatched car can be declared
STOPPED SHORT after ~3 s instead of ~9 s.

Code A — `ReDispatch` (SpikeRunner.cs:1441):

```csharp
internal static void ReDispatch(VehicleAgent agent, Vector3 target, string why)
{
    SpikeState.PollingActive = true;
    SpikeState.NavTarget = target;
    SpikeState.NavStartTime = Time.unscaledTime;
    SpikeState.NavEverAutoDriving = false;
    SpikeState.NavCallbackResult = null;
    SpikeState.NavRetried = true;
    int order = SpikeState.NextNavOrder();
    // ... progress window reset, brake release ...
    agent.Navigate(target, null, NavigationCallbackFor(order));
}
```

Code B — first-dispatch retry + grace (SpikeRunner.cs:1803-1831):

```csharp
if (!SpikeState.NavRetried && !SpikeState.NavEverAutoDriving && elapsed >= 3f)
{
    SpikeState.NavRetried = true;
    SpikeState.NavRetryAt = now;
    int retryOrder = SpikeState.NextNavOrder();
    agent.Navigate(SpikeState.NavTarget, null, NavigationCallbackFor(retryOrder));
    return;
}

if (!SpikeState.NavEverAutoDriving && elapsed < 3f)
    return; // give the first dispatch its 3 s before deciding.

if (SpikeState.NavRetried && SpikeState.NavRetryAt > 0f && now - SpikeState.NavRetryAt < RetryGraceSeconds)
    return;
```

DO-NOT-GUESS: whether the game's `Navigate(target, null, ...)` behaves
differently on re-dispatch. Assume it does not.

Research questions:
1a. Confirm by reading Code A + B only: is there any path on which
`NavRetryAt` is set between `ReDispatch` and the grace check? (Expected: no.)
1b. Which fix is more robust: (i) set `NavRetryAt = now` inside `ReDispatch`,
or (ii) derive the grace from `NavStartTime`/`elapsed` instead of a second
timestamp? Discuss stale-timestamp failure modes of each.

---

## 2. Manually boarded rides never start (HIGH)

Finding: seating the player (`Ride()`) never starts a ride; the ride starts
only via a pickup/exit flag the diagnostic flow never sets. Boarding + `taxi
to <place>` stores a destination "for the NEXT ride" that has no trigger.

Code — `Ride()` seats but never starts (SpikeCommands.cs:845); the only
`StartRide` caller is the board detector gated on `RideAwaitingBoard`
(SpikeRunner.cs:970):

```csharp
if (SpikeState.RideAwaitingBoard && veh != null && veh.LocalPlayerIsInVehicle &&
    Time.unscaledTime - SpikeState.LastExitAt >= SpikeCommands.ExitDebounceSeconds)
{
    SpikeCommands.StartRide("ride-kernel");
    return;
}
```

DO-NOT-GUESS: game seat semantics (`DriverPlayer`, `localPlayerSeat`).

Research questions:
2a. From a state-machine design view: is a single "passenger aboard with NPC
at wheel" event (fired by both manual boarding and pickup boarding) cleaner
than the current flag (`RideAwaitingBoard`) that only two of three boarding
paths set? Sketch the minimal transition table.
2b. What guard prevents double-start if both the manual path and the flag
path fire on the same frame (the mod already has per-frame `Update` + a
monotonic `NavOrder` token)?

---

## 3. `TaxiTuning.Apply` is never called (MEDIUM)

Finding: a repo-wide search for `TaxiTuning` returns only its own file. The
config loads and logs but no caller applies it.

Code — the dead applier (TaxiTuning.cs:65):

```csharp
internal static void Apply(LandVehicle veh)
{
    float want = Config.StuckTimeThresholdSeconds;
    if (want <= 0f) { /* keep game default */ return; }
    VehicleAgent? agent = veh.Agent;
    if (agent == null) { /* warn */ return; }
    float before = agent.StuckTimeThreshold;
    if (Math.Abs(before - want) < 0.01f) return;
    agent.StuckTimeThreshold = want;
}
```

Research questions:
3a. No game knowledge needed — this is a wiring omission. What cheap static
safeguard catches "public method with zero call sites" in a .NET 6 project
(compiler warning, analyzer rule, or test) without restructuring the mod?
3b. The setter targets an instance field on a per-vehicle agent object that
dies with the vehicle. Is there any Unity/IL2CPP reason an instance-field
write like this would NOT stick (e.g. native-side re-init per frame), and
how would you verify it sticks using only in-game observable behavior?

---

## 4. Trunk lock skipped for one ride after a scene load (MEDIUM)

Finding: the static disabled-interactable list survives a scene load, so the
next ride's lock is skipped by its own idempotency guard.

Code — guard + only unlock call site:

```csharp
// RideLocks.cs:172
internal static void LockTrunk(LandVehicle veh)
{
    if (TrunkInteractables.Count > 0)
        return;
    // ... disable trunk/Storage InteractableObjects ...
}

// only caller of UnlockTrunk is EndRide (SpikeCommands.cs:1518);
// scene-load reset (SpikeState.Reset) never calls it.
```

DO-NOT-GUESS: `InteractableObject`, `StorageDoorAnimation`, `Trunk` shapes.

Research questions:
4a. In Unity (2022.3, IL2CPP backend), what is the reliable way to detect a
destroyed object held in a static `List<T>` where `T : UnityEngine.Object` —
`== null`, `ReferenceEquals`, `Pointer == IntPtr.Zero`, or
`WasCollected` — and what are the failure modes of each under IL2CPP
interop wrappers?
4b. Which pattern is standard for mod/static state across scene unloads:
subscribe to `SceneManager.sceneUnloaded`, prune lazily with an aliveness
check at the guard, or both? What goes wrong if cleanup runs during the
unload callback itself?

---

## 5. Destination rows keyed by unstable catalog index (MEDIUM)

Finding: list rows use the catalog index as click key, but the catalog is
rebuilt from a live scene walk on every open — indexes shift when world
objects appear/disappear.

Code (TaxiApp.cs:483-516):

```csharp
MakeDestinationRow("STAND", $"Taxi-Stand ({TaxiStand.StandName})", "STAND", "Taxi-Stand");
// ...
foreach (TaxiDestinations.Destination destination in catalog)
{
    if (!MatchesFilter(destination))
        continue;
    MakeDestinationRow(destination.Index.ToString(), destination.Name, destination.Tag, destination.Name);
}
```

with index assignment after filtering/dedup in `BuildCatalog`, and lookup by
`int.TryParse` in `TryFind`.

Research question:
5. For a Unity phone-UI list rebuilt from live scene objects, what is the
recommended stable row identity when names can repeat and objects can
despawn — and how do you resolve it back to a world position at tap time
without re-walking the scene into a different order?

---

## 6. STOP/cleanup player exit skips the ground snap (MEDIUM)

Finding: two exit paths snap the player to safe ground; the cancel path
exits the player with a bare `ExitVehicle()` and destroys the car.

Code — cancel path without snap (SpikeCommands.cs:2343-2371):

```csharp
LandVehicle? veh = SpikeState.Vehicle;
if (veh != null)
{
    if (veh.LocalPlayerIsInVehicle)
    {
        veh.ExitVehicle();
        Print("LandVehicle.ExitVehicle() called (local player).");
    }
    if (SafePlayerInVehicle(veh)) { /* refuse destroy */ }
}
```

vs the normal path ending with (SpikeCommands.cs:961-964):

```csharp
EndRide("player exited", stopNavigation: false, rearm: true);
ReportExitDeltas(veh, beforeAll, afterNavStop);
SafeExitGroundSnap(veh);
```

DO-NOT-GUESS: what `ExitVehicle` does to the player transform.

Research questions:
6a. Is "exit, verify ground under player via downward raycast ignoring
dynamic/trigger colliders, else move player beside the vehicle" the standard
Unity approach for modded vehicle exits, and what layer/collider filters
does it need so the probe itself never lands on the vehicle being exited?
6b. Any reason to run the ground snap BEFORE destroying the vehicle rather
than after (collider lifetime during `DestroyVehicle`)?

---

## 7. Change-signature bit packing overflows at $64 (MEDIUM)

Finding: display-only staleness from 32-bit shift wrapping.

Code (TaxiApp.cs:741-754):

```csharp
int signature = (SpikeState.RideActive ? 1 : 0)
                | (SpikeState.RideAwaitingDestination ? 2 : 0)
                // ... flags up to bit 10 ...
                | ((destination.GetHashCode() & 0xFFFF) << 11)
                | (FareMeter.ChargedTotal << 27);
if (signature == _heroSignature && overrideText == _heroOverrideShown)
    return;
```

Research questions:
7a. Confirm in C# (unchecked default): `64 << 27` as `int` equals 0, and
`$128`, `$192` collide likewise. Show the arithmetic.
7b. What is the idiomatic replacement for a multi-field UI change signature
in C# 12 (`HashCode.Combine`, tuple compare, or plain field compare), and
why is `string.GetHashCode` additionally unsuitable for anything persisted
or compared across sessions?

---

## 8. Clear-while-driving keeps a stale target under a cleared label (LOW)

Finding: `ClearDestination` drops picker state while the dispatched drive
continues; the hero shows fare accumulating toward "none selected".

Code (SpikeState.cs:321):

```csharp
internal static void ClearDestination()
{
    bool keepWaiting = RideActive && !RideDestinationPicked;
    ResetPicker();
    if (keepWaiting)
        RideAwaitingDestination = true;
}
```

Research question:
8. UI-state vs simulation-state separation: when the user clears a
destination mid-drive, which of these is least surprising — (a) keep driving
to the dispatched point and keep showing it as "previous destination", (b)
re-target to a safe stop, (c) end the ride? Argue from standard navigation-
UI conventions, no game knowledge needed.

---

## 9. Exited taxi left unbraked (LOW)

Finding: after player exit only `StopNavigating` runs; brakes/handbrake are
never set, so the car can roll until the idle guard catches it.

Code (SpikeCommands.cs:924-961): pre-exit `agent.StopNavigating()` plus
`PollingActive = false`, then `veh.ExitVehicle()`, then
`EndRide("player exited", stopNavigation: false, rearm: true)` — whose
`stopNavigation: false` branch releases only the patrol behavior, never
calling `ParkCar`.

Research question:
9. For Unity wheeled-vehicle mods, is "brakes + handbrake before player exit,
release on next dispatch" the correct order, and does setting brake flags
while the player object is being teleported out risk a physics pop? What
ordering avoids it?

---

## 10. Pause freezes only part of the deadlines (LOW)

Finding: `ShiftPauseDeadlines` extends some timestamps each paused frame but
not `AutoNextAt`, `PendingSpawnAt`, `SettleCheckAt`, `LastExitAt`,
`RideHeartbeatAt` — so automation can advance while paused.

Code (SpikeRunner.cs:268-289):

```csharp
private static void ShiftPauseDeadlines()
{
    float dt = Time.unscaledDeltaTime;
    if (SpikeState.NavReDispatchAt > 0f)
        SpikeState.NavReDispatchAt += dt;
    if (SpikeState.NavStartTime > 0f)
        SpikeState.NavStartTime += dt;
    if (_startupAt > 0f)
        _startupAt += dt;
    if (_selfTestAt > 0f)
        _selfTestAt += dt;
    if (_navCalcSince > 0f)
        _navCalcSince += dt;
    if (_proofSpeedSince > 0f)
        _proofSpeedSince += dt;
    if (_playerOutSince > 0f)
        _playerOutSince += dt;
    if (_idleMovingSince > 0f)
        _idleMovingSince += dt;
    if (SpikeState.ProgressWindowStart > 0f)
        SpikeState.ProgressWindowStart = Time.unscaledTime;
}
```

Research questions:
10a. Shifting every deadline by `unscaledDeltaTime` each paused frame vs
early-returning the whole `Update` on `timeScale == 0`: which is less error-
prone as new timers are added, and what breaks if a timer is added to one
scheme but not the other?
10b. Is there a Unity pause edge where `unscaledDeltaTime` spikes (e.g.
alt-tab hitches) that would over-shift these deadlines, and should the shift
be clamped?

---

## 11-14. Small items (no deep research needed, include for completeness)

11. `BoardDriver` success check (SpikeCommands.cs:771)
`if (!best.IsInVehicle || best.CurrentVehicle == null) return false;`
ignores the occupant-slot proof the same function requires elsewhere.
Question: none structural — confirm that requiring the slot too cannot
produce false negatives if the game fills slots a frame later (i.e. is a
one-frame-delayed recheck needed?).

12. `taxi status` prints `(default ROAD A)` (SpikeCommands.cs:2243) although
no default exists since Paket A. Question: none — text fix to
`(none selected)`.

13. Fare threshold migration (FareMeter.cs:98-112): values below 0.1 are
fixed in memory without rewriting the file, so the file stays dirty.
Question: none structural — confirm check order (legacy-then-invalid) is the
minimal fix.

14. Auto-run pending-spawn guard covers step 1 only
(SpikeRunner.cs:864: `if (SpikeState.AutoStep == 1 && ...)`).
Steps 2/3 can run before the deferred spawn fires on slow frames.
Question: is extending the guard to `AutoStep >= 1` sufficient, or does the
step-2 delay (1 s) vs spawn delay (0.5 s) already constitute the intended
margin — i.e. under what frame-time distribution does the race actually
fire?
