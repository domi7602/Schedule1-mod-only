# Station Framework (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `StationItem` | Base station item |
| `StationRecipe` | Recipe definition |
| `BoilingFlask` | Boiling flask |
| `LiquidContainer` | Liquid container |

## Base Classes

- `StationItem`: MonoBehaviour base for all stations
- `StationRecipe`: Defines input/output for stations
- `ItemModule`: Item integration
- `IngredientModule`: Ingredient integration
- `PourableModule`: Pouring integration

## Liquids

- `LiquidContainer`: Liquid physics
- `LiquidLevelVisuals`: Visual liquid level display
- `LiquidVolumeCollider`: Collision volume
- `BoilingFlask`: Boiling flask with boiling behavior

## Station Types

| Station | Class |
|---------|-------|
| Mushroom Spawn | `MushroomSpawnStation` |
| Spore Syringe | `SporeSyringeStationItem` |
| Liquid Meth | `LiquidMeth_StationItem` |
| Product | `ProductStationItem` |

## Cookables

- `CookableModule`: Cooking module
- `Fillable`: Fillable stations
- `IngredientPiece`: Ingredient pieces
- `PourableAngleLimit`: Pouring angle limit

## Integration

- Stations interact with the management system (Clipboard)
- Each station has a UI configuration
- Networked for multiplayer sync
- Integration with EntityFramework (GridItem, SurfaceItem)
