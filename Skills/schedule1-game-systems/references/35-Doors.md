# Doors (Schedule I)
> verified: classes + methods + enum values + UnityEvents re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: game v0.4.7f9 / S1API 3.2.1-beta.8.

## Core Classes (`ScheduleOne.Doors`)

| Class | Verified base type | Purpose |
|-------|--------------------|---------|
| `DoorController` | `NetworkBehaviour` | The main networked door: open/close state, access, sensors, handles |
| `DoorSensor` | `MonoBehaviour` | Trigger volume detecting players/NPCs (`ActivationDistanceSqr`, `DetectorSide`, `Door`, `OnTriggerEnter/Exit`, `UpdateCollider()`) |
| `StaticDoor` | `MonoBehaviour` | "Fake" doors used by NPC buildings: knock/enter (teleport) |
| `PropertyDoorController` | `DoorController` (in `ScheduleOne.Building.Doors`) | Property-locked door: `Property`, `IsUnlocked`, `Unlock()`, `CheckClose()`, `WANTED_PLAYER_CLOSE_DISTANCE` |
| `DoorKnocker` | `MonoBehaviour` (in `ScheduleOne.Building.Doors`) | `Knock()`, `PlayKnockingSound()` |

### Enums (values verified — old list was wrong)
- `EDoorAccess`: `Open, Locked, ExitOnly, EnterOnly` (NOT "Public/PropertyOnly/Keycard")
- `EDoorSide`: `Interior, Exterior` (NOT "Front/Back")

## Motion Types (no shared base class!)
Each door type is an independent `MonoBehaviour` — they do **not** inherit from `DoorController`:
| Class | Mechanism | Key members |
|-------|-----------|-------------|
| `PivotDoor` | Hinged | `DoorTransform`, `FlipSide`, `OpenInwardsAngle`, `OpenOutwardsAngle`, `SwingSpeed`, `UpdateDoor()`, coroutine-driven |
| `SlidingDoor` | Slides | `ClosedPosition`, `OpenPosition`, `SlideDuration`, `SlideCurve`, `IsOpen`, `Move()` |
| `RollerDoor` | Rolls up | `IsOpen`, `LocalPos_Open`/`LocalPos_Closed`, `LerpTime`, `Blocker`, `Open()`, `Close()`, `LateUpdate()` |
| `SensorRollerDoors` | `RollerDoor` | Auto via `Detector`/`ClipDetector`, `DetectPlayerOccupiedVehiclesOnly` |
| `DarkMarketRollerDoors` | `SensorRollerDoors` | Dark Market variant |
| `ManholeCoverMovement` | Anim-driven | `Open()`, `Close()` (uses `Anim`) |
| `Peephole` | Anim-driven | `Open()`, `Close()`, `OpenSound`, `CloseSound` |
| `DealerStaticDoor` | `StaticDoor` | `Dealer` field |
| `SewerDoorController` | `DoorController` | overrides `CanPlayerAccess(EDoorSide, out string)`, `ExteriorHandleInteracted()` |

Also related (not in `ScheduleOne.Doors`): `Map.DarkMarketMainDoor`, `ObjectScripts.LabOvenDoor`, `Storage.StorageDoorAnimation`.

## DoorController (verified API)
- Config fields: `PlayerAccess` (EDoorAccess), `AutoOpenForPlayer`, `OpenableByNPCs`, `AutoCloseOnSleep`, `AutoCloseOnDistantPlayer` (+ static `DISTANT_PLAYER_THRESHOLD`), `ReturnToOriginalTime`, `PlayerBlocker` (BoxCollider), `InteriorIntObjs` / `ExteriorIntObjs` (`InteractableObject[]` — the actual interact handles), `InteriorDoorHandleAnimation` / `ExteriorDoorHandleAnimation`, `noAccessErrorMessage`
- Runtime state: `IsOpen` (virtual property), `lastOpenSide`, `openedByNPC`, `detectedNPCCount`, `detectedPlayerCount`, `timeSincePlayerSensed`, `timeInCurrentState`
- Access logic: `CanPlayerAccess(EDoorSide)` (non-virtual bool) and `virtual CanPlayerAccess(EDoorSide, out string reason)`
- Interaction callbacks (all virtual): `InteriorHandleHovered/Interacted()`, `ExteriorHandleHovered/Interacted()`, `NPCVicinityEnter/Exit(EDoorSide)`, `PlayerVicinityEnter/Exit(EDoorSide)`
- Open/close: `SetIsOpen_Server(bool open, EDoorSide accessSide, bool openedForPlayer)` → client `SetIsOpen(NetworkConnection, bool, EDoorSide)` → `virtual SetIsOpen(bool, EDoorSide)` (FishNet: Server RPC `1319291243`, Observers + Target broadcasts)

## Events / UnityEvents
| Event | Type | Raised by |
|-------|------|-----------|
| `DoorController.onDoorOpened` | `UnityEvent<EDoorSide>` | on door open (invoke site inferred; `.Invoke()` calls live in native code, not visible in interop dump) |
| `DoorController.onDoorClosed` | `UnityEvent` | on door close (same caveat) |

## Save Participation
- No `ISaveable` / `SaveData` in `ScheduleOne.Doors` (grep verified). Door open state is transient runtime state (with `AutoCloseOnSleep` cleanup); **not** persisted in the save file.

## Hook Points (Harmony)
1. `DoorController.CanPlayerAccess(EDoorSide side, out string reason)` (virtual) — prefix to bypass/lock access per side (locks, keycards, bounty-based doors).
2. `DoorController.SetIsOpen(bool, EDoorSide)` (virtual, runs on every client after RPC) — postfix for open/close reactions (lighting, sound, counters). Patch this instead of the RPC plumbing.
3. `StaticDoor.Interacted()` / `Knock()` (virtual) — custom NPC summons / knock-based triggers.
- S1API: `S1API.Doors` exists — wrapper `DoorController` (`PlayerAccess`, `IsOpen`, `AutoOpenForPlayer`, `OpenableByNPCs`, events `OnDoorOpened`/`OnDoorOpenedAny`...) with enums `S1API.Doors.DoorAccess` and `DoorSide` mirroring the native ones.

## Not Implemented / Notes
- There is **no** keycard subsystem class — access is purely `EDoorAccess` + `CanPlayerAccess` overrides per subclass.
- `RollerDoor.LateUpdate()` performs the actual lerp between open/closed positions — per-frame logic, avoid patching.
