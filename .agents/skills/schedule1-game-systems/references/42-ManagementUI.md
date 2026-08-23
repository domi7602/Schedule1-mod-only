# Management UI (Schedule I)

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
