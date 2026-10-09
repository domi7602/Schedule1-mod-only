# Management UI (Schedule I)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 47 of 47 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.


## Core Classes

| Class | Purpose |
|-------|---------|
| `ManagementInterface` | Main management interface |
| `EntityConfiguration` | Base entity configuration |
| `ClipboardScreen` | Management clipboard UI |
| `ConfigPanel` | Configuration panel |

## Management Clipboard

- `ManagementClipboard_Equippable`: Equippable clipboard
- `ManagementClipboard`: Clipboard component
- Displays configurable entities on the property

## Configurable Entities

| Entity | Config Panel |
|--------|-------------|
| `PotConfiguration` | `PotConfigPanel` |
| `MushroomBedConfiguration` | `MushroomBedConfigPanel` |
| `MixingStationConfiguration` | `MixingStationConfigPanel` |
| `ChemistryStationConfiguration` | `ChemistryStationConfigPanel` |
| `CauldronConfiguration` | `CauldronConfigPanel` |
| `LabOvenConfiguration` | `LabOvenConfigPanel` |
| `BrickPressConfiguration` | `BrickPressConfigPanel` |
| `DryingRackConfiguration` | `DryingRackConfigPanel` |
| `PackagingStationConfiguration` | `PackagingStationConfigPanel` |
| `SpawnStationConfiguration` | `SpawnStationConfigPanel` |

## Employee Configuration

- `BotanistConfiguration` / `BotanistConfigPanel`
- `ChemistConfiguration` / `ChemistConfigPanel`
- `PackagerConfiguration` / `PackagerConfigPanel`
- `CleanerConfiguration` / `CleanerConfigPanel`

## UI Fields

- `ItemFieldUI`: Item selection
- `NPCFieldUI`: NPC selection
- `NumberFieldUI`: Number input
- `StringFieldUI`: Text input
- `QualityFieldUI`: Quality selection
- `ObjectFieldUI`: Object selection
- `ObjectListFieldUI`: Object list
- `RouteListFieldUI`: Route list
- `StationRecipeFieldUI`: Recipe selection

## Transit Routes

- `TransitRoute`: Route between stations
- `TransitRouteMaterial`: Visual route representation
- `AdvancedTransitRoute`: Advanced routes
- `TransitLineVisuals`: Line visualization
