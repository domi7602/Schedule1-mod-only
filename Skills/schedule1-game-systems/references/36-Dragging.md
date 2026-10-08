# Dragging (Schedule I)
> verified: classes + methods + RPCs + save-participation re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

Namespace: `Il2CppScheduleOne.Dragging`.

## Core Classes
| Class | Base | Purpose (decompile-verified) |
|-------|------|------------------------------|
| `DragManager` | `NetworkSingleton<DragManager>` | Global drag system: registration, physics, replication |
| `Draggable` | `MonoBehaviour` | Single draggable rigidbody with GUID |
| `SceneDraggable` | `Draggable` | Scene-placed draggable with baked GUID / replication mode |
| `PlayerTasks.Draggable` | `Clickable` | Separate class for physics-based station tasks (Pour/Mist etc.) |
| `PlayerTasks.DraggableConstraint` | `MonoBehaviour` | Constrains task-draggable movement |

## DragManager API (verified)
- Tuning floats: `DragForce`, `DampingFactor`, `TorqueForce`, `TorqueDampingFactor`, `ThrowForce`, `MassInfluence`; static `DefaultDraggableOffset`; `ThrowSound` (`AudioSourceController`)
- State: `CurrentDraggable` (prop, set protected), `IsDragging` (read-only prop), `AllDraggables`, `CurrentlyUpdating`, `lastThrownDraggable`, `lastHeldDraggable`, `_dragStartedThisFrame`
- Flow: `IsDraggingAllowed()` → `RegisterDraggable`/`Deregister` → `StartDragging(Draggable)` → `StopDragging(Vector3 velocity)` → `GetTargetPosition()`; input poll in `UpdateInput()` (private), physics in `FixedUpdate()`
- Replication via FishNet RPCs: `SetDragger_Server(guid, NetworkObject, pos)` / `SetDragger_Client(...)`; `SetTransformData_Server(guid, pos, rot, velocity)` / `SetTransformData_Client(...)` (observers + target variants) — replication keyed by GUID string

## Draggable API (verified)
- Props: `GUID` (Il2CppSystem.Guid), `InitialPosition`, `Velocity`, `Mass`, `IsBeingDragged`, `CurrentDragger` (Player)
- Per-object multipliers: `HoldDistanceMultiplier`, `DragForceMultiplier`, `ThrowForceMultiplier`; static `DraggableInteractionRange`, `PositionDeltaToDropDraggable`
- Companion fields: `_interactable` (`Interaction.InteractableObject` — drag is offered through the interaction system), `DragOrigin`, `_timeOnLastDragStop`, `_defaultCollisionDetectionMode`
- Methods: `UpdateDraggable()`, `ApplyDragForces(Vector3)`, `StartDragging(Player)`, `StopDragging()`, `SetTransformData(pos, rot)`, `SetVelocity(Vector3)`, `SetRigidbody(Rigidbody)`, `SetGUID(Guid)`
- Virtuals for subclassing: `Hovered()`, `Interacted()`, `CanStartDrag()`, `ShouldReplicateInitialTransformForClient(NetworkConnection)`
- `SceneDraggable` adds: static `MovedThreshold`, `BakedGUID` (string), `InitialReplicationMode` (`EInitialReplicationMode`, nested enum), `RegenerateGUID()`

## Applications
- Drag trash bags to containment; move crates/containers
- Drag unconscious/bodied NPCs (via attached `Draggable`)
- `Equippable_TrashGrabber` (Equipping): `TrashItemDraggable.IsTrashGrabberEquipped(...)` gates grabber pickup (verified cross-ref)
- PlayerTasks variant: pulling pots/pourables into station slots

## Events
- `DragManager.onThrowEvent` (`Il2CppSystem.Action`) — subscribe with `SubscribeToThrowEvent(Action)`, remove with `UnsubscribeFromThrowEvent(Action)` (verified; `lastThrownDraggable` set on throw)
- No UnityEvents declared in this namespace (grep)

## Save Participation
- **No `ISaveable` / `SaveData` in any Dragging class** (grep over namespace) — persisted positions flow through other systems; `GUID` is the key used by replication. Which system persists dragged transforms: `unverified`.

## Hook Points
1. `DragManager.IsDraggingAllowed()` (public instance, returns bool) — prefix patch to block/allow all dragging, e.g. freeze props during events; plain method, no field accessors, safe patch target
2. `DragManager.SubscribeToThrowEvent(Action)` — direct runtime subscribe for throw detection (no Harmony needed)
3. `Draggable.CanStartDrag()` (protected virtual) — prefix patch for per-object drag rules (locked doors, quest items); virtual dispatch keeps it patchable
- No S1API wrapper exists for Dragging (grep over S1API source: no `DragManager`/`Draggable` references)

## Not Implemented / Unverified
- Keybind for drag: existing note "Default: G" kept from prior doc — `unverified` in decompiles (binding lives in input settings, not this namespace)
- No drag-limit system, no weight/stamina cost for dragging (nothing in decompile)
- `Draggable.GUID` is a `virtual final new` property — likely an interface impl (e.g. persistence/network GUID contract), exact interface `unverified`

## Cross-links
09-Inventory-ItemFramework · 40-Interaction · 57-Storage · 47-ObjectStations · 01-FishNet-Networking
