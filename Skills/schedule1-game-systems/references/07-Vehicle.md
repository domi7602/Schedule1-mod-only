# Vehicle (Schedule I)
> UNVERIFIED for runtime 0.4.7f11 — carried-over knowledge; re-verify API details against the current decompiles before patching. Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.


## Vehicle Types
| Type | Base | Physics |
|------|------|---------|
| Car (`LandVehicle`) | `NetworkBehaviour` | Unity WheelCollider |
| Skateboard | `NetworkBehaviour` | Custom hover system (PID) |

## Cars (LandVehicle)
- `Shitbox` as the only concrete subclass
- **No fuel system**
- `Wheel[]`: WheelCollider + visuals + drift effects
- Physics optimization: kinematic when >30m away or not visible

### AI Navigation (VehicleAgent)
- Unity NavMesh (no A* for vehicles)
- 2 NavMesh graphs: `"General Vehicle Graph"` + `"Road Nodes"`
- PID steering (P=40, I=5, D=10), PID throttle (P=0.08)
- 5 sensors (FL, FM, FR, RR, RL) for obstacle detection
- Stuck detection: If <1m in 10s → teleport to road network
- Speed zones: BoxCollider with speed limits (default 25 km/h)

### Parking
- `ParkingLot` → `ParkingSpot[]` with `EntryPoint` + `ExitPoint`
- `Park(conn, ParkData)`: Positions vehicle, can hide visual model
- `ExitPark()`: Moves vehicle to ExitPoint, unhides visual model

### Purchase / Sale
- Dealership (NPC Jeremy): `BUY_CASH` or `BUY_ONLINE`
- Paint job: `VehicleModStation` with 16 colors (100 online)
- **No vehicle selling system**

### Police Pursuit
- `VehiclePursuitBehaviour`: 2 modes
  - Aggressive (visual contact): 80 km/h, ignores roads
  - Non-Aggressive (to last known position): 1.5x speed limit
- VisionCone + `LastKnownPosition`
- 5s after lost line of sight: `IsTargetRecentlyVisible = false`

## Live-Verified API (v0.4.7f6, disassembled 2026-09-25 from `MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll`)

Signatures confirmed via ilspycmd against the INSTALLED build (the sections above may still describe 0.4.6f13):

- `VehicleManager.Instance` → `SpawnAndReturnVehicle(string vehicleCode, Vector3 pos, Quaternion rot, bool playerOwned)`, `VehiclePrefabs` (`List<LandVehicle>`), `GetVehiclePrefab(string)`. Vehicle codes come from `VehiclePrefabs` entries (`VehicleCode` property) — do not guess them.
- `LandVehicle` → `public VehicleAgent Agent {get;set}`, `Seats` (`Il2CppReferenceArray<VehicleSeat>`), `driverEntryPoint`, `OccupantNPCs`, `DriverPlayer`, `IsOccupied`, `GetFirstFreeSeat()`, `AddNPCOccupant(NPC)`, `RemoveNPCOccupant(NPC)`, `EnterVehicle()` (local player), `ExitVehicle()`, `DestroyVehicle()`, `VehicleCode/VehicleName/VehiclePrice`, `Speed_Kmh`, `LocalPlayerIsInVehicle`.
- `VehicleAgent` → `Navigate(Vector3 location, NavigationSettings settings = null, NavigationCallback callback = null)`, `StopNavigating()`, `AutoDriving`, `TargetLocation`, `IsOnVehicleGraph()`, `GetIsStuck()`. `ENavigationResult { Failed, Complete, Stopped }` (names verified live 2026-09-25 — Failed=0.2s on bad targets, Complete on arrival, Stopped on our own StopNavigating/timeout).
- `NavigationSettings` (class, parameterless ctor) → bool fields `endAtRoad`, `ensureProximityToGraph`, `teleportToGraphIfCalculationFails`.
- `NavigationCallback` → has `public static implicit operator NavigationCallback(System.Action<ENavigationResult>)` (DelegateSupport.ConvertDelegate) — usable from managed code: pass a lambda cast through the operator; `ENavigationResult { Failed, Complete, Stopped }` gives the definitive Navigate outcome (verified in 0.4.7f6 interop dump 2026-09-25; earlier notes claiming an unreachable ctor were wrong).
- `VehicleSeat` → `isDriverSeat` (get/set), `Occupant` (Player only — NPC occupancy lives on `LandVehicle.OccupantNPCs`; index mapping `OccupantNPCs[i]↔Seats[i]` is only live-verified for the single-driver case, NOT directly observable in 0.4.7f6), `isOccupied` (stays False for NPC occupants by design).
- `NPC` → `EnterVehicle(NetworkConnection connection, LandVehicle veh)` (public virtual, RPC-shaped — singleplayer: try `null` connection), `ExitVehicle()`, `CurrentVehicle`, `IsInVehicle`, events `onEnterVehicle`/`onExitVehicle`. **There is NO `NPC.CurrentVehicleSeat` in the live 0.4.7f6 dump** (older notes claimed it — wrong); NPC seat proof = `LandVehicle.OccupantNPCs[i] == npc` plus root-to-seat distance (closest seat == `isDriverSeat` seat, measured 0.0 m — the NPC root lands on the seat anchor); `VehicleSeat.Occupant` is typed `Player`, so `isOccupied` never reflects NPCs. `NPCManager.NPCRegistry` is `public static List<NPC>`.
- Reference pattern for NPC-drives-vehicle: schedule spec `S1API.Entities.Schedule.DriveToCarParkSpec` wires native `NPCSignal_DriveToCarPark` via reflection (sets `action.Vehicle`/`action.ParkingLot`). Vanilla behaviour class: `VehiclePatrolBehaviour` (`DriveTo(Vector3)`, `SetRoute`, `Vehicle`, `Agent`).
- S1API wrappers expose native objects only as `internal` (`LandVehicle.S1LandVehicle`, `NPC.S1NPC`) — mods that need `Agent`/native calls must use the `Il2CppScheduleOne.*` types directly (Assembly-CSharp reference comes from `Directory.Build.props`).

### Map markers for routes (no custom HUD needed)
- S1API `S1API.Map.MapPOIBuilder(id).WithLabel(...).WithPosition(Vector3 world).Build()` places a marker on the VANILLA MapApp; `MapPOIManager.Get(id)` / `MapPOI.Dispose()`.
- Vanilla `MapApp` (`Il2CppScheduleOne.UI.Phone.Map.MapApp`): `PoIContainer` (RectTransform parent for map items), `SetupMapItem(GameObject)`, `BackgroundImage`, `MainMapSprite`, `FocusPosition(Vector2)`, `opened`.
- `MapPositionUtility.Instance.GetMapPosition(Vector3)→Vector2` (world→map) is ONE-WAY; no inverse exists. Map-click→world must be derived at runtime (calibrate affine inverse from `OriginPoint`/`EdgePoint`/`conversionFactor` + sample calls) — spike-pending, see TaxiDriver.

### Autonomous navigation — live findings (TaxiDriver spike, 13 rounds 2026-09-25)
- **Navigate succeeds or fails by TARGET LOCATION, not by settings/owner/spawn.** Blind `playerPos + forward*40` targets that sit off the reachable road graph return `NavigationCalculationCallback(result=Failed, path=null)` in ~0.2 s, while the same dispatch with a road coordinate returns `Success + SmoothedPath` in ~0.35 s. Verified in the same run: blind `(-95.1,-3.9,101.3)` → Failed; road points `(-131.4,-4,51.9)` and `(-17.1,0,13.4)` (both vanilla Navigate locations) → Success. `NavigationSettings` (endAtRoad/ensureProximityToGraph/teleportToGraphIfCalculationFails) vs `settings=null` changes NOTHING (both Fail on bad targets, both Succeed on road targets).
- **`NavigationUtility.SampleVehicleGraph(target)` snaps to the General Vehicle Graph** — a 9.6 m snap delta is accepted by the sampler but the road path search still fails; proven road targets show ~0.1 m delta. Snap is a diagnostic, NOT a guarantee.
- **The only observable path-search boundary from managed code**: `VehicleAgent.NavigationCalculationCallback(NavigationUtility.ENavigationCalculationResult result, PathSmoothingUtility.SmoothedPath path)` — public, Harmony-prefixable, `ENavigationCalculationResult { Success, Failed }` (nested in `NavigationUtility`). Fires for EVERY dispatch. The managed `NavigationUtility.CalculatePath` wrapper is NEVER called by the native Navigate body (0 trace hits despite a valid patch) — do not instrument it expecting traffic.
- **Before dispatching Navigate**: turn the vehicle toward the target (`transform.rotation = Quaternion.LookRotation(flatDir)`) — otherwise the agent burns its first ~8 s in reverse maneuvers (54/89 reverse polls, no arrival) and clears `BrakesApplied` AND `HandbrakeApplied`.
- **Heartbeat polling that works**: `agent.AutoDriving`, `veh.Speed_Kmh`, `agent.IsOnVehicleGraph()`, `agent.GetIsStuck()`, `agent.IsReversing` at 0.5 s intervals; arrival = `NavigationCallback(Complete)` (~8 m residual with `endAtRoad=true`, e.g. 25.6 m trip → Complete after 20.2 s, max 20.6 km/h).
- **Test hotkeys (TaxiDriver spike)**: F1 road target A · F2 road target B · F3 full run to road target · F4 visual toggle · F5 call-taxi · F6 full run · F7 probe · F8 trace (logs `native ptr` of Navigate: `0x327431F8` this session) · F9 ride/out · F10 Navigate settings=null · F11 spawn without NPC · F12 Navigate on a vanilla vehicle. MelonLoader console has NO input field — in-game `taxi ...` commands are unreachable; use hotkeys (all fit F1-F12, scene-gated to the gameplay scene). Since 0.2.0 the taxi can also be ordered from the in-game phone (Taxi app); the hotkeys remain for diagnostics.
- **Input pitfall**: F13+ VKs (≥ 0x7C) are still dropped by synthetic key events, but since the 2026-09-26 remap no taxi hotkey needs them — all fit F1-F12 (VK < 0x7C).
- `LandVehicle` exposes BOTH `BrakesApplied` and `HandbrakeApplied`; `VehicleAgent.KinematicMode` and `IsPhysicallySimulated`/`ShouldBePhysicallySimulated()` explain drive capability (spawned player-owned vehicles are simulated; vanilla traffic drives non-simulated/kinematic).

## Skateboard
- Custom hover system: 4 HoverPoints with PID controller
- Push mechanics with stamina (12.5 per push, 1s cooldown)
- Jumping: force charge (0.5s), `JumpDuration` 0.2–0.5s
- Terrain slowdown: 40% speed penalty on Terrain tag
- Weather: rain influence via `SkateboardSettings.Blend()`
- Equippable (`Skateboard_Equippable`)
