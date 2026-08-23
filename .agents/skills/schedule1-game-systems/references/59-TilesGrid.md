# Tiles & Grid (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `Grid` | Main grid system |
| `Tile` | Individual tile |
| `Coordinate` | Coordinate in the grid |
| `TileDetector` | Tile detection |

## Grid System

- 2D grid for property building
- `Grid`: Manages all tiles of a property
- `IndoorTile`: Indoor tile (under roof)
- `FootprintTile`: Footprint tile
- `ProceduralTile`: Procedurally generated tile

## Coordinates

- `Coordinate`: Grid coordinate (x, y, z)
- `CoordinatePair`: Coordinate pair
- `CoordinateTilePair`: Tile-to-tile connection
- `CoordinateFootprintTilePair`: Footprint connection
- `CoordinateProceduralTilePair`: Procedural connection

## Tile Types

| Type | Properties |
|------|------------|
| `Tile` | Base tile |
| `IndoorTile` | Under roof (temperature regulated) |
| `FootprintTile` | Building footprint |
| `ProceduralTile` | Randomly generated |

## Visuals

- `TileAppearance`: Visual tile appearance
- `ETileColor`: Tile color
- `ETileDetectionMode`: Detection mode
- Category system for material assignment

## Detection

- `TileDetector`: Detects tiles nearby
- `ETileDetectionMode`: Raycast, Trigger, Physics
- Integration with building system

## Entity Framework

- `GridItem`: Grid-based entity
- `ProceduralGridItem`: Procedural grid entity
- `BuildableItem`: Placeable entity
- `SurfaceItem`: Surface entity
