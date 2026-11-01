# Vehicle (Schedule I)

> Inherited, not rerun; [evidence scope](README.md#source-history).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 119/122 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

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
- A* Pathfinding Project seekers (`roadSeeker` and `generalSeeker`), not the previously claimed Unity NavMesh-only implementation. Targeted correction inspected 2026-10-30: TaxiDriver compiles direct Seeker access against the installed interop assemblies, and its preserved 2026-10-26 native trace records both seekers. This does not reverify the other inherited vehicle claims.
- Graphs named `"General Vehicle Graph"` and `"Road Nodes"`; these names do not establish a concrete graph implementation type.
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

## previously reported API (inspected 2026-09-25 from `MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll`)

Signatures confirmed via ilspycmd against the installed build; some sections above may reflect older evidence:

- `VehicleManager.Instance` → `SpawnAndReturnVehicle(string vehicleCode, Vector3 pos, Quaternion rot, bool playerOwned)`, `VehiclePrefabs` (`List<LandVehicle>`), `GetVehiclePrefab(string)`. Vehicle codes come from `VehiclePrefabs` entries (`VehicleCode` property) — do not guess them.
- `LandVehicle` → `public VehicleAgent Agent {get;set}`, `Seats` (`Il2CppReferenceArray<VehicleSeat>`), `driverEntryPoint`, `OccupantNPCs`, `DriverPlayer`, `IsOccupied`, `GetFirstFreeSeat()`, `AddNPCOccupant(NPC)`, `RemoveNPCOccupant(NPC)`, `EnterVehicle()` (local player), `ExitVehicle()`, `DestroyVehicle()`, `VehicleCode/VehicleName/VehiclePrice`, `Speed_Kmh`, `LocalPlayerIsInVehicle`.
- `VehicleAgent` → `Navigate(Vector3 location, NavigationSettings settings = null, NavigationCallback callback = null)`, `StopNavigating()`, `AutoDriving`, `TargetLocation`, `IsOnVehicleGraph()`, `GetIsStuck()`. `ENavigationResult { Failed, Complete, Stopped }` (names verified live 2026-09-25 — Failed=0.2s on bad targets, Complete on arrival, Stopped on our own StopNavigating/timeout).
- `NavigationSettings` (class, parameterless ctor) → bool fields `endAtRoad`, `ensureProximityToGraph`, `teleportToGraphIfCalculationFails`.
- `NavigationCallback` -> has `public static implicit operator NavigationCallback(System.Action<ENavigationResult>)` (DelegateSupport.ConvertDelegate) and accepts a managed lambda; `ENavigationResult { Failed, Complete, Stopped }` provides the Navigate outcome (previously reported in the inspected interop dump on 2026-09-25).
- `VehicleSeat` -> `isDriverSeat` (get/set), `Occupant` (Player only; NPC occupancy lives on `LandVehicle.OccupantNPCs`), `isOccupied` (stays False for NPC occupants). The `OccupantNPCs[i]` to `Seats[i]` mapping was previously reported only for the single-driver case; the proxy does not expose it directly.
- `NPC` -> `EnterVehicle(NetworkConnection connection, LandVehicle veh)` (public virtual, RPC-shaped; singleplayer: try `null` connection), `ExitVehicle()`, `CurrentVehicle`, `IsInVehicle`, events `onEnterVehicle` / `onExitVehicle`. The inspected dump had no `NPC.CurrentVehicleSeat`; NPC seat proof used `LandVehicle.OccupantNPCs[i] == npc` plus root-to-seat distance. `VehicleSeat.Occupant` is typed `Player`, so `isOccupied` does not reflect NPC occupants. `NPCManager.NPCRegistry` is `public static List<NPC>`.
- Reference pattern for NPC-drives-vehicle: schedule spec `S1API.Entities.Schedule.DriveToCarParkSpec` wires native `NPCSignal_DriveToCarPark` via reflection (sets `action.Vehicle`/`action.ParkingLot`). Vanilla behaviour class: `VehiclePatrolBehaviour` (`DriveTo(Vector3)`, `SetRoute`, `Vehicle`, `Agent`).
- S1API wrappers expose native objects only as `internal` (`LandVehicle.S1LandVehicle`, `NPC.S1NPC`) — mods that need `Agent`/native calls must use the `Il2CppScheduleOne.*` types directly (Assembly-CSharp reference comes from `Directory.Build.props`).

### Map markers for routes (no custom HUD needed)
- S1API `S1API.Map.MapPOIBuilder(id).WithLabel(...).WithPosition(Vector3 world).Build()` places a marker on the VANILLA MapApp; `MapPOIManager.Get(id)` / `MapPOI.Dispose()`.
- Vanilla `MapApp` (`Il2CppScheduleOne.UI.Phone.Map.MapApp`): `PoIContainer` (RectTransform parent for map items), `SetupMapItem(GameObject)`, `BackgroundImage`, `MainMapSprite`, `FocusPosition(Vector2)`, `opened`.
- `MapPositionUtility.Instance.GetMapPosition(Vector3)→Vector2` (world→map) is ONE-WAY; no inverse exists. Map-click→world must be derived at runtime (calibrate affine inverse from `OriginPoint`/`EdgePoint`/`conversionFactor` + sample calls) — spike-pending, see TaxiDriver.

### Autonomous navigation — live findings (TaxiDriver spike, 13 rounds 2026-09-25)
- **Target-dependent outcomes in those September trials.** Blind `playerPos + forward*40` targets off the reachable road graph returned `NavigationCalculationCallback(result=Failed, path=null)` in ~0.2 s, while the road-coordinate trials returned `Success + SmoothedPath` in ~0.35 s. Reported in the same run: blind `(-95.1,-3.9,101.3)` → Failed; road points `(-131.4,-4,51.9)` and `(-17.1,0,13.4)` (both vanilla Navigate locations) → Success. Explicit `NavigationSettings` versus `settings=null` changed neither success nor failure for those specific targets. This does not establish equivalent effective flags, endpoints or behavior for other routes.
- **`NavigationUtility.SampleVehicleGraph(target)` snaps to the General Vehicle Graph** — a 9.6 m snap delta is accepted by the sampler but the road path search still fails; proven road targets show ~0.1 m delta. Snap is a diagnostic, NOT a guarantee.
- **Path-search boundary observed in the historical spike**: `VehicleAgent.NavigationCalculationCallback(NavigationUtility.ENavigationCalculationResult result, PathSmoothingUtility.SmoothedPath path)` — public, Harmony-prefixable, `ENavigationCalculationResult { Success, Failed }` (nested in `NavigationUtility`). It was observed for each tested dispatch. The managed `NavigationUtility.CalculatePath` wrapper produced zero trace hits despite the installed patch in those trials; that does not prove it is never called in other paths. Later TaxiDriver traces also observe seeker completions, so this is not the only available observation boundary.
- **Historical orientation experiment, not current guidance**: the September spike force-turned the vehicle toward its target to reduce startup reversing (54/89 reverse polls in the reported trial). Current `SpikeCommands.Go` and `SpikeRunner.ReDispatch` deliberately omit road-agnostic forced yaw; their source records its later removal after collision reports. Do not restore that old recipe or directly rewrite braking outputs from this historical note; current owned-taxi park/reverse input handling belongs to `NativeActuatorPatch`.
- **Historical heartbeat observations**: `agent.AutoDriving`, `veh.Speed_Kmh`, `agent.IsOnVehicleGraph()`, `agent.GetIsStuck()` and `agent.IsReversing` were sampled at 0.5 s intervals. A reported 25.6 m trip emitted `Complete` after 20.2 s with ~8 m residual and max 20.6 km/h under `endAtRoad=true`. `Complete` alone is not the current TaxiDriver arrival rule: current-operation evidence and a finite readable non-reversing halt/proximity are required.
- **Historical test hotkeys (TaxiDriver spike)**: F1 road target A · F2 road target B · F3 full run to road target · F4 visual toggle · F5 call-taxi · F6 full run · F7 probe · F8 trace (logs `native ptr` of Navigate: `0x327431F8` this session) · F9 ride/out · F10 Navigate settings=null · F11 spawn without NPC · F12 Navigate on a vanilla vehicle. The spike used scene-gated F1-F12 because the MelonLoader console had no input field for `taxi ...` commands. A later iteration added phone ordering. **The current TaxiDriver installs no keyboard bindings**; use its [current service instructions](../../../Source/Mods/TaxiDriver/docs/README.md).
- **Input pitfall**: F13+ VKs (≥ 0x7C) are still dropped by synthetic key events, but since the 2026-09-26 remap no taxi hotkey needs them — all fit F1-F12 (VK < 0x7C).
- `LandVehicle` exposes BOTH `BrakesApplied` and `HandbrakeApplied`; `VehicleAgent.KinematicMode` and `IsPhysicallySimulated`/`ShouldBePhysicallySimulated()` explain drive capability (spawned player-owned vehicles are simulated; vanilla traffic drives non-simulated/kinematic).

### Retry settings — historical observation inspected 2026-10-30

- Do not treat `Navigate(target, null, callback)` as equivalent to the initial explicit settings. The preserved 2026-10-26 TaxiDriver 1.0.4 trace reads initial `endAtRoad=true / ensureProximityToGraph=true / teleportToGraphIfCalculationFails=false`, but `false / true / true` after a null-settings recovery. That establishes a flag change, not an actual teleport on every failed calculation. <!-- skills-health: version-evidence (historical route trace, not a dependency pin) -->
- The endpoint-policy difference, Bungalow traversal failure and Docks general-graph failure are separate findings. Neither a sampled graph point nor successful road-seeker output establishes whole-route reachability. A generic error suggestion about tags does not identify a blocking tag.
- [Navigation settings evidence and scope](../../../Source/Mods/TaxiDriver/docs/navigation-settings-verification.md) preserves source identities and limits. This inspection did not launch the game or replace the earlier dated gameplay evidence above.

### Street candidates and full calculation — inspection 2026-10-31

- The owner-run TaxiDriver trace reproduces successful full paths ending before the
  supervised Barn/Bungalow-fallback targets, and failed whole Docks calculations despite
  road-seeker successes. A General Vehicle Graph sample or parking entry is not proof of
  full reachability. An exact, walkable Road Nodes sample is also only a **candidate**;
  preserve the logical-goal selection envelope separately from physical arrival radius.
- Installed signature inspection confirms `NavigationUtility.CalculatePath(startPosition,
  destination, navSettings, flags, generalSeeker, roadSeeker, callback)` returns a Unity
  `Coroutine`; the callback supplies Success/Failed plus a `SmoothedPath`. The interop
  bodies do not establish the host, cancellation or borrowed-seeker ownership contract.
  Do not overlap an independent probe with navigation, or assume a timeout/token change
  stopped native work. `Navigate` recalculates instead of accepting that precomputed path.
- The native calculation callback has no request/order ID. A diagnostic using the current
  navigation order does not independently prove the result's originating request. Do not
  install a path through that native-private callback or adopt an arbitrary short endpoint.
- [Scoped evidence and repair limits](../../../Source/Mods/TaxiDriver/docs/street-dropoff-verification.md)
  records installed input hashes and the decision to avoid an additional asynchronous
  preflight in the first selection-only candidate. This is not native route acceptance.

## Skateboard
- Custom hover system: 4 HoverPoints with PID controller
- Push mechanics with stamina (12.5 per push, 1s cooldown)
- Jumping: force charge (0.5s), `JumpDuration` 0.2–0.5s
- Terrain slowdown: 40% speed penalty on Terrain tag
- Weather: rain influence via `SkateboardSettings.Blend()`
- Equippable (`Skateboard_Equippable`)

---

## Unresolved identifiers in the imported 2026-10-08 record

See [scan limits](README.md#source-history); unresolved does not mean removed.

- `VehicleModStation`
- `BUY_ONLINE`
- `BUY_CASH`
