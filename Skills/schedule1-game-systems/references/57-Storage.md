# Storage (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `StorageManager` | Global storage manager (NetworkSingleton) |
| `StorageEntity` | Base storage entity |
| `StorageGrid` | Storage grid |
| `StorageTile` | Individual grid tile |

## Storage Entities

- `StorageEntity`: Container for stored items
- `WorldStorageEntity`: Placeable in the world
- `PlaceableStorageEntity`: Placeable in the building grid
- `SurfaceStorageEntity`: Placeable on surfaces

## Storage Grid

- `StorageGrid`: Grid system for item placement
- `StorageTile`: Individual tile in the grid
- Items occupy 1 to n tiles
- Visual feedback during placement

## Stored Items

- `StoredItem`: Base for stored items
- `StoredItem_GenericBox`: Generic box
- `StoredItemRandomRotation`: Random rotation
- `LiquidMeth_Stored`: Liquid meth storage
- `StorableItemInstance`: Storable item instance

## UI

- `StorageMenu`: Storage UI
- `StorageVisualizationUtility`: Visualization utility
- `StorageVisualizer`: Visualizer
- `StorageEntityVisualizer`: Entity visualizer
- `StorageEntityInteractable`: Interaction component

## Animation

- `StorageDoorAnimation`: Door animation for storage entities
- `CoordinateStorageFootprintTilePair`: Coordinate mapping

## Features

- Store / retrieve items
- Automatic sorting
- Categorization by item type
- Multiplayer sync of contents
