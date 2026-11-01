# Implementation evidence and limits

## API truth used

Read repository AGENTS.md fully, modding working contract, police system references 12 and
51, lifecycle and Harmony contracts, existing mod integration, and `Tools/new-mod.ps1`.
Used the helper to scaffold and `Tools/gen-sln.ps1` to add mod/tests to S1Mods.sln.

Inspected current installed Assembly-CSharp with repository-local ilspycmd 9.1.0.7988,
scoped to LawActivitySettings, CurfewManager, PatrolInstance, VehiclePatrolInstance,
CheckpointInstance, SentryInstance, CurfewInstance, LawManager, PoliceOfficer,
NPCResponses_Police and PoliceStation. Those are **interop thunks: signatures, not native
behavior evidence**. Compile also validates directly accessed checkpoint fields,
LandVehicle.Seats / VehicleSeat.Occupant, officer/station registries, Unity activeSelf/SetActive, lifecycle
and console base signatures. Runtime reflection resolves full parameter signatures with
DeclaredOnly and rejects anything except void/bool before installing a required hook.
All hooks share one explicit owner; no Harmony annotations/automatic second installation.

Observed installed SHA256:

| File | SHA256 |
|---|---|
| MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll | `346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8` |
| Mods/S1API.Il2Cpp.MelonLoader.dll | `85b218ff0ac3d18ec03e2a959f13ce13c596d38d46ab78f7908ec9fa7533c6e3` |
| MelonLoader/net6/0Harmony.dll | `6c898933b52149e8bcf6722305c7e4d94add47c9e53b364096f91721624c0877` |

The S1API hash differs from the historical AGENTS.md baseline; no downgrade, replacement or
claim of matching the historical artifact was made. Console discovery was inspected in local
S1API source; its execution still needs the live acceptance check.

Verified CurfewManager backing flags: `_IsEnabled_k__BackingField`,
`_IsCurrentlyActive_k__BackingField`, `_IsHardCurfewActive_k__BackingField`.
There is no invented StopActivity call, no native object-result prefix and no global AI hook.

## Executed checks

- Game process query before build returned no Schedule process.
- TDD RED: five policy cases failed on absent policy; GREEN five passed.
- RED: two lease cases failed on absent lease; GREEN seven total passed.
- RED: three runtime wiring checks failed against scaffold/missing integration; GREEN ten passed.
- RED: added raw-curfew lease wiring check failed; implemented fields; GREEN fourteen passed
  including additional lease exception/mismatch/dead-object/idempotence regression tests.
- RED: live readback wiring requirement failed; implemented readback; GREEN fourteen passed.
- Final targeted Release build with S1NoDeploy=true: zero warnings/errors.
- Installed Mods/PoliceTestMode.dll was absent after the no-deploy build.
- No launch, deployment, commit or push. No native hook installation/invocation, GUI, taxi,
  curfew, OFF restoration or save/reload behavior has been declared passed.

## Repository-wide verification

Ran `Tools/verify-clean-checkout.ps1 -Mode Pure` in an isolated snapshot. Its game-free
mod test step passed, as did solution determinism, docs, skills, XML and the other tool
suites except workspace setup. Overall **19 steps passed, 3 failed**:

- Format: new Mod.cs retained scaffold CRLF. Corrected only PoliceTestMode source/tests
  using scoped dotnet format; subsequent scoped full format verification passed.
- Version sync: PoliceTestMode is intentionally missing global README/AGENTS/compatibility
  rows pending parent review. Existing HitmanPhone/Minimap version drift also reported.
- workspace-setup tool fixtures: recursive Git clone/submodule failed with an MSYS path
  `not a git repository` error in the isolated long scratch path. No mod-runtime failure.

The entire Pure gate has **not** subsequently passed. Report retained at
`C:/Users/pc/AppData/Local/hermes/cache/scratch/police-clean-check/clean-checkout-359138f229fb/checks/verify-4f0845ea3ee7/report.json`.
Final targeted tests (14), no-deploy build (zero warnings/errors), and scoped formatting
were rerun after the formatting correction and passed.

## Changed scope

Only new PoliceTestMode mod/tests plus regenerated Source/Mods/S1Mods.sln. Existing dirty
TaxiDriver, PropertyLEDs, HitmanPhone, Minimap and global documentation were preserved.
Global inventory/version rows intentionally left to parent review as requested.

See README.md for exact GUI commands, expected log evidence and reversal limitations.

## Player occupancy reviewer blocker (no deployment)

- Re-inspected installed `Assembly-CSharp.dll` using ilspycmd 9.1.0.7988. SHA256 remains
  `346205306273805a78b01a917f14b94518498af1361595c4c417baa0b32b4bd8`.
- Exact public signatures: `LandVehicle.Seats: Il2CppReferenceArray<VehicleSeat>`,
  `VehicleSeat.Occupant: Il2CppScheduleOne.PlayerScripts.Player`,
  `LandVehicle.OccupantPlayers: Il2CppSystem.Collections.Generic.List<Player>`,
  `LandVehicle.OccupantNPCs: Il2CppReferenceArray<NPC>`, `LandVehicle.IsOccupied: bool`.
  Interop thunks establish signatures only. TaxiDriver CHANGELOG driver-seat evidence
  records NPC-only `IsOccupied=True` with null Player occupants; its current
  `SpikeCommands.ReadPassengerSeat` also reads Player-typed occupants fail-closed.
- Replacement uses `Seats` and explicit `Player? occupant = seat.Occupant`, not the
  combined vehicle flag or local-player-only test. All readable null occupants => Clear
  (including NPC-only). Any Player => protect. Missing/empty/oversized seat collection,
  dead/null seat, invalid player wrapper, or read exception => Unknown/protect.
  Existing leases on protected vehicles restore before removal. Unknowns visibly show
  `PARTIAL - vehicle occupancy unreadable`; status reports `playerOccupiedSkipped` and
  `unknownSkipped` separately, alongside existing active-target and raw-curfew readbacks.
- RED: ten pure occupancy cases plus replacement wiring check failed on absent reader;
  extracted adapter NPC-only regression failed on absent adapter. GREEN: 35 passed;
  new unknown-status wiring failed as expected. After status wiring, **36/36 passed**.
  Eleven adapter cases compile/execute the unchanged extracted method against native-getter
  substitutes and the real policy. This tests adapter logic, not actual IL2CPP execution.
- Full PoliceTestMode Release build with `-p:S1NoDeploy=true`: zero warnings/errors.
  Six production/project files retained; no version, game, deployment or other-mod edits.
  No live occupancy or Unity activation readback claimed; gameplay acceptance remains open.
