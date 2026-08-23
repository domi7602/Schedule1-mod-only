# Vehicle (Schedule I)

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
  - **Aggressive** (visual contact): 80 km/h, ignores roads
  - **Non-Aggressive** (to last known position): 1.5x speed limit
- VisionCone + `LastKnownPosition`
- 5s after lost line of sight: `IsTargetRecentlyVisible = false`

## Skateboard
- Custom hover system: 4 HoverPoints with PID controller
- Push mechanics with stamina (12.5 per push, 1s cooldown)
- Jumping: force charge (0.5s), `JumpDuration` 0.2–0.5s
- Terrain slowdown: 40% speed penalty on Terrain tag
- Weather: rain influence via `SkateboardSettings.Blend()`
- Equippable (`Skateboard_Equippable`)
