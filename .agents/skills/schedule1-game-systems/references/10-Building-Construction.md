# Building/Construction (Schedule I)

## Grid System
- 2D grid (x/z), **Tile size = 0.5 units**
- `Grid` with `Coordinate(x, y)` → `Tile`
- No 3D grid (height = floor level)

## Entity Hierarchy
```
BuildableItem (abstract, NetworkBehaviour)
  +-- GridItem (on grid tiles)
  |     +-- ToggleableItem (on/off)
  |     +-- GrowContainer, ChemistryStation, Pot, etc.
  +-- ProceduralGridItem (on ProceduralTile, e.g. shelving)
  +-- SurfaceItem (on walls/ceilings)
        +-- ToggleableSurfaceItem
        +-- LabelledSurfaceItem
```

## BuildManager (NetworkSingleton)
1. **`StartBuilding(item)`** → Instantiates `BuildHandler` (ghost)
2. **`BuildUpdate`** → Positioning + validation
3. **`Place()`** → `CreateGridItem()` → FishNet spawn + remove item from inventory

## Ghost Preview
- **White** (`ghostMaterial_White`): valid placement position
- **Red** (`ghostMaterial_Red`): invalid (occupied, wrong property, corner obstacle, offset mismatch)
- Ghost disables: Colliders, Navigation, Networking, Canvases, Lights
- `ActivateDuringBuild` objects are visible in ghost mode

## Validation (Grid)
1. `CheckIntersections()` → find nearest tile
2. Snap footprint to tile
3. For each footprint: `Grid.IsTileValidAtCoordinate()` checks:
   - Does coordinate exist in grid?
   - Is tile free?
   - Is `AvailableOffset >= RequiredOffset`?
   - Is property owned?
4. `AreCornerObstaclesBlocked()` → corner obstruction check

## Rotation
- Grid: 90-degree steps (0/90/180/270)
- ProceduralGrid: 90-degree steps
- Surface: `RotationIncrement` (default 45°, can be 0 = none)

## Walls / Doors
- **NO player-buildable walls** (static level geometry)
- Walls have `Surface` component for `SurfaceItem` placement
- `PropertyDoorController`: Opens upon property purchase, closes during police activity (20m)

## Properties
| Class | Special Feature |
|-------|-----------------|
| `Bungalow` | Ground floor |
| `MotelRoom` | Motel room |
| `RV` | Can explode (`BlowUp()`) |
| `SewerOffice` | Passcode door |
| `Business` | Commercial property (laundering) |

## Removal
- `CanBeDestroyed()` / `CanBePickedUp()`
- `PickupItem()` → item returned to inventory + `Destroy_Server()`
- `Destroy()` → `ClearPositionData()` (clears tile occupants)
