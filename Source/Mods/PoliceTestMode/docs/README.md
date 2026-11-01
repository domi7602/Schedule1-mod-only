# PoliceTestMode 0.1.0 — standalone singleplayer TEST MOD

**Default ON whenever installed. Installed; scoped live activation/OFF/ON and lifecycle restoration verified.**

Live check: 43/43 hooks registered; 10 officers and 4 checkpoints suppressed (`activeTargets=0/14`), raw curfew flags false; actual activity/officer/curfew hook invocations observed. OFF restored all leases (`pending=0`), ON returned ACTIVE; menu/shutdown restored cleanly and removed all hooks. TaxiApp and taxi driver initialized. Parent independently verified the evidence log SHA-256 `5a2cfdf3ddd5d5e43e1ef7e7c8b39cd7ff78c3527b7d77920190b360eae088c2` under `.scratch/police-test-mode/live/final.log`; all 40 native save files unchanged. 36 tests pass, Release 0 warnings/errors; installed DLL SHA-256 `d44e9921202ba6e25dcd3bdcc6a910090f52806291ee46ffe8e0bac470d6a956`.

Not exercised: actual nighttime arrest/pursuit, deployed police vehicles, taxi travel, repeated reloads or second-process restart. OFF restores captured booleans, not exact AI coroutine history. Global AGENTS inventory update was blocked by an approval timeout and was not retried.
This is separate from TaxiDriver. Requires the workspace MelonLoader IL2CPP, Shared and S1API dependencies.

## Controls / visible state

Open the game's developer console and enter (without a slash):

```text
policetest status
policetest off
policetest on
```

ON means **police suppressed**, OFF means native police permitted. The HUD overlay was removed
at the owner's request. Use `policetest status` for WAITING, ACTIVE, PARTIAL and ERROR states;
startup still prints the default-ON notice. Overlay removal: observed RED/GREEN source contract,
36 tests passed and clean Release build; installed no-overlay DLL hash
`033de3df31a6c8fa21f917a0e2a04b70980b99da5a476658096307c5dc05e740` matched build, with no OnGUI member.
Suppression behavior and console controls are unchanged. Version remains 0.1.0 pending protected
inventory synchronization; historical HUD evidence below describes the earlier build.
The preference is process-memory only: OFF survives menu/save reloads in this process;
a new process defaults ON again. There are no preferences or sidecars. Remove the DLL
with the game closed to remove the test mode.

`on` rearms the 30-check readiness window (one check/second after load complete) and retries
restoration before reapplying. Required-hook failure is not retried; inspect logs and restart
with a corrected build. Unknown authority, clients and any Steam lobby are refused.

## What it changes

- Explicit police-only Harmony gates: activity evaluation/start, station dispatch, police calls,
  curfew activation/tick/getters, officer pursuit/body search/investigation/vision, and police
  response callbacks (including curfew and vehicle pursuit).
- Once/second, bounded registry access (512-entry safety cap), **not scene scans**:
  `PoliceOfficer.Officers`, `PoliceStation.PoliceStations[].deployedVehicles`, and the four
  dedicated CheckpointManager checkpoints. Snapshots `GameObject.activeSelf`, temporarily
  makes only those police objects inactive, and verifies the write. New/re-enabled police
  objects are covered on the next pass. Player-occupied police vehicles are not hidden;
  NPC-only patrol cars remain eligible. Every seat's Player-typed `Occupant` is checked,
  not the vehicle-wide `IsOccupied` flag (which includes NPCs). Missing/empty/unreadable
  seat data protects the vehicle too. Both protections produce visible PARTIAL status,
  with separate `playerOccupiedSkipped` and `unknownSkipped` counters, not a full claim.
- Snapshots the three CurfewManager backing flags (enabled/current/hard), forces false and
  verifies them. This also covers callers reading native fields rather than patched getters.
- OFF, world-leaving, loss of singleplayer authority and shutdown restore exact captured
  boolean values, including originally inactive objects. Three restoration attempts per
  transition; failures retain their snapshots and report ERROR rather than silently clearing.
- No global NPCBehaviour/VehicleAgent hooks or shutdown, no taxi mutations, no NPC deletion,
  no death/despawn or identity substitution, no LE-intensity changes and no save-file writes.

## Important limits

This pauses/hides existing police objects; it does not serialize an AI simulation. Unity
SetActive(false) can terminate coroutines. OFF restores boolean state and permits future native
activity, **not the exact interrupted pursuit/patrol coroutine or elapsed-time history**. Normal
native scheduling resumes; a menu/reload after OFF is the clean baseline if an interrupted
activity does not resume. Do not use this as a production gameplay mod.

Existing player wanted/crime records are deliberately untouched. OFF can therefore resume a
response immediately. Already-open search/arrest UI is not dismissed. Enable by installation
before loading a test session rather than in the middle of an arrest. A save initiated by the
base game can still record ordinary gameplay progression; this mod does not claim all native
save bytes are invariant. In-game save/reload behavior remains unverified.

## GUI acceptance procedure (reviewer/parent; not executed here)

1. With game closed, install through the repository deploy helper after review. Launch through
   Steam only. Load a disposable **singleplayer** session. Wait for ACTIVE, not just startup.
2. Run `policetest status`. In `MelonLoader/Latest.log`, require `effective=True`, no ERROR,
   nonzero officers/leases, `playerOccupiedSkipped=0`, `unknownSkipped=0`, `activeTargets=0/<nonzero>` and
   `curfewRaw=False/False/False`. `hooks ready registered=N/N` proves registrations only.
3. Let a native activity tick occur and exercise nighttime/curfew, police call/crime and taxi
   driver/passenger travel through a former patrol/checkpoint area. Check for no response,
   no police roadblocks, and normal taxi/NPC traffic. First hook invocations log
   `gate invoked <Owner>.<Method>`; `policetest status` prints cumulative `blocked ...=N`.
   Examples to look for: `LawActivitySettings.Evaluate`, `PoliceStation.Dispatch`,
   `PoliceOfficer.BeginVehiclePursuit`, `NPCResponses_Police.NoticedViolatingCurfew`.
   A counter absent/zero means **not observed**, not a passing test. Inactive officers may
   legitimately never receive vision callbacks; do not manufacture crime calls to inflate counts.
4. `policetest off`: require `effective=False`, `restore reason=command pending=0`, HUD OFF.
   Observe normal patrol/response on a subsequent activity cycle. Confirm taxi remains normal.
5. `policetest on`, wait one second, re-read status; repeat OFF/ON twice. Menu/reload once ON
   and once OFF. Confirm no stale objects, duplicate hooks or restoration errors. Restart
   process to confirm default ON. Never claim this checklist passed from build results.

## Local verification / review footprint

```pwsh
dotnet test Source/Tests/PoliceTestMode.Tests/PoliceTestMode.Tests.csproj -c Release -p:S1NoDeploy=true
dotnet build Source/Mods/PoliceTestMode/src/PoliceTestMode.csproj -c Release -p:S1NoDeploy=true
```

Six hand-maintained production files including the csproj: Mod, PoliceHooks, PoliceWorld,
BoolLease, SessionPolicy, project. Shared build props/targets supply game/S1API/Harmony references
and Shared project dependency; no other mod references. Tests are game-free policy/lease tests
plus source wiring checks and execution of the extracted production seat adapter with
substituted native getters, not native simulation. See [evidence](evidence.md).
