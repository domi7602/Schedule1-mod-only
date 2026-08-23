# Player Tasks (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `TaskManager` | Global task manager |
| `Task` | Base task class |

## Task Types

| Task | Function |
|------|----------|
| `HarvestPlant` | Harvest plant |
| `SowSeedTask` | Plant seeds |
| `ApplyAdditiveToPot` | Apply additive/fertilizer |
| `FillWaterContainer` | Refill water container |
| `PourWaterTask` | Pour water |
| `PourSoilTask` | Pour soil |
| `CauldronTask` | Operate cauldron |
| `StartLabOvenTask` | Start oven |
| `FinalizeLabOven` | Finalize oven |
| `LabOvenSolidTask` | Oven solid processing |
| `UseBrickPress` | Use brick press |
| `UseChemistryStationTask` | Use chemistry station |
| `FinishChemistryStationTask` | Finish chemistry station |
| `PackageProductTask` | Package product |
| `PackageProductTaskMk2` | Package product Mk2 |
| `HarvestMushroomBedTask` | Harvest mushroom bed |
| `ApplyShroomSpawnTask` | Apply mushroom spawn |
| `InocculateGrainBagTask` | Inoculate grain bag |
| `MistMushroomBedTask` | Mist mushroom bed |
| `GrowContainerPourTask` | Fill grow container |

## Task System

- Tasks are interaction minigames
- `Task`: Base class, defines start/end behavior
- Requires specific equipped items
- `Clickable`, `Draggable`, `Moveable`, `Pourable`, `Sprayable`, `RotateRigidbodyToTarget`

## Interaction Types

- **Clickable**: Click to execute
- **Draggable**: Drag to execute
- **Pourable**: Pour to execute
- **Sprayable**: Spray to execute
- **RotateRigidbodyToTarget**: Rotate to target

## Task UI

- `TaskManagerUI`: Task manager UI
- `ProgressSlider`: Progress bar
- Interaction prompts for current task
