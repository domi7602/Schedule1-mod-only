# Map & Locations (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `Map` | Main map |
| `MapPositionUtility` | Map positions |
| `EMapRegion` | Regions enum |
| `POI` | Points of Interest |

## Map System

- `MapApp`: Map app on the phone
- `UIMapPanel`: Map UI panel
- `UIMapItem`: Map UI item
- Interactive 2D map of the game world

## Points of Interest

- `POI`: Base POI class
- `NPCPoI`: NPC-specific POIs
- `MapRegionData`: Region data on map
- POIs display NPCs, shops, properties

## Important Locations

| Location | Class | Description |
|----------|-------|-------------|
| DarkMarket | `DarkMarket` | Dark market |
| Dealership | `Dealership` | Car dealership |
| MedicalCentre | `MedicalCentre` | Hospital / Medical center |
| PoliceStation | `PoliceStation` | Police station |
| SewerManager | `SewerManager` | Sewer system |
| AutoshopAccessZone | `AutoshopAccessZone` | Auto repair shop |
| ManorGate | `ManorGate` | Manor gate |

## Regions

- `EMapRegion`: 6 regions + Sewer
- `AccessZone`: Access zones
- `TimedAccessZone`: Timer-controlled access
- `NPCPresenceAccessZone`: NPC presence zone

## Access Control

- `Gate`, `ManorGate`: Gates
- `Ladder`: Ladders
- `SewerCameraPresense`: Camera presence
- `ParkingLot`, `ParkingSpot`: Parking lots and spots
