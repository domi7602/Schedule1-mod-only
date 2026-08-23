# Plant/Growing (Schedule I)

## Architecture
```
GrowContainer (abstract, GridItem)
  +-- Pot
  |     +-- Plant (abstract)
  |     |     +-- WeedPlant
  |     |     +-- CocaPlant
  |     +-- SoilChunk[]
  |     +-- PotConfiguration
  |
  +-- MushroomBed
        +-- ShroomColony (NetworkBehaviour)
        |     +-- GrowingMushroom[]
        +-- MushroomBedConfiguration
```

## Growth (MinPass Tick)
- Every game minute: `Plant.MinPass(1)`
- On TimeSkip: `Plant.MinPass(minsSkipped)`
- Formula:
  ```
  growth = (1 / (GrowthTime * 60)) * mins
        * TemperatureMultiplier
        * AverageLightExposure
        * GrowSpeedMultiplier
        * (0 if Moisture <= 0)
  ```
- `GrowthTime` = 48min (Weed), 18min (Shrooms)

## Stages
- `PlantGrowthStage[]`: One active GameObject per stage
- Final stage = `FinalGrowthStage` with `GrowthSites` (Transforms for buds)
- Collider scales starting at 50% growth

## Soil
- `SoilDefinition`: Basic / Premium, `Uses` (default 1)
- `SoilCapacity` = 30, consumed after harvest (`_remainingSoilUses - 1`)
- At 0: soil + water + additives are cleared
- ESoilState: Flat → Parted → Packed

## Water
- `MoistureCapacity` = 5, `_moistureDrainPerHour` = 1
- Automatic watering: `Sprinkler` (range, 5 segments)
- Manual: `PourableWaterContainerEquipped`
- **No water = no growth** (multiplier = 0)

## Additives (max 3)
| Effect | Description |
|--------|-------------|
| `QualityChange` | Changes quality (+/-) |
| `YieldMultiplier` | Multiplies yield (min 0) |
| `InstantGrowth` | Instant growth in % (0-1) |

## Quality
- Starting value: `BaseQualityLevel = 0.5f`
- **No other influencing factors** (temperature/light/water only affect growth speed)

| Threshold | Quality | Color |
|-----------|---------|-------|
| > 0.9 | Heavenly | Gold |
| > 0.75 | Premium | Pink |
| > 0.4 | Standard | Blue |
| > 0.25 | Poor | Green |
| <= 0.25 | Trash | Brown |

## Harvest
- `BaseYieldQuantity` = 12 (default), multiplied by `YieldMultiplier`
- Harvestables distributed randomly across `GrowthSites`
- Harvestable only with **Trimmers** (Equippable)
- After full harvest: +5 XP, soil uses -1, additives reset
- When `_remainingSoilUses <= 0`: soil completely cleared

## MushroomBed (vs. Pot)
| Aspect | Pot | MushroomBed |
|--------|-----|-------------|
| Object | Plant (single) | ShroomColony (multiple) |
| Seed | SeedDefinition | ShroomSpawnDefinition |
| Temperature | +20°C = faster | max 15°C, otherwise no growth |
| Growth time | 48min | 18min |
| Harvest | Buds | Individual mushrooms (`GrowingMushroom`) |

## No System For:
- Diseases/pests
- Plant protection products / pesticides
- Random damage events
