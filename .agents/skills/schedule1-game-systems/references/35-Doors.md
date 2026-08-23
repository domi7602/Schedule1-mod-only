# Doors (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `DoorController` | Base door controller |
| `EDoorAccess` | Access level (Public, Private, Keycard) |
| `EDoorSide` | Door side (Front, Back) |

## Door Types

| Type | Description |
|------|-------------|
| `PivotDoor` | Standard hinged door |
| `SlidingDoor` | Sliding door |
| `RollerDoor` | Roller door |
| `StaticDoor` | Non-interactable door |
| `SensorRollerDoors` | Sensor-controlled roller door |
| `DarkMarketRollerDoors` | Dark Market roller door |

## Special Doors

- `ManholeCoverMovement`: Manhole cover
- `Peephole`: Peephole
- `SewerDoorController`: Sewer door
- `DealerStaticDoor`: Dealer door

## Access Control

- `EDoorAccess`: Public, PropertyOnly, Keycard, Locked
- `PropertyDoorController`: Access via property ownership
- `DoorSensor`: Automatic door opening

## Interaction

- Doors are `InteractableObject` subclasses
- Open/Close animation
- Network sync via RPCs
