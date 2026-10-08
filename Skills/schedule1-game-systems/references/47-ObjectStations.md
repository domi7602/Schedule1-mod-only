# Object Stations (Schedule I)
> UNVERIFIED for runtime 0.4.7f11 — carried-over knowledge; re-verify API details against the current decompiles before patching. Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.


## Core Classes

Stations are processing objects placed in properties. Each station has dedicated functionality.

## Production Stations

| Station | Class | Function |
|---------|-------|----------|
| Mixing Station | `MixingStation` | Base mixing station |
| Mixing Station Mk2 | `MixingStationMk2` | Advanced mixing station |
| Chemistry Station | `ChemistryStation` | Chemical processing |
| Cauldron | `Cauldron` | Cauldron for meth cooking |
| Lab Oven | `LabOven` | Lab oven for drying |
| Brick Press | `BrickPress` | Press machine for drug bricks |

## Processing Stations

| Station | Class | Function |
|---------|-------|----------|
| Drying Rack | `DryingRack` | Drying rack |
| Packaging Station | `PackagingStation` | Packaging station |
| Packaging Station Mk2 | `PackagingStationMk2` | Advanced packaging station |
| Recycler | `Recycler` | Recycling machine |
| Soil Pourer | `SoilPourer` | Soil dispenser |

## Plant Stations

| Station | Class | Function |
|---------|-------|----------|
| Pot | `Pot` | Plant pot |
| Mushroom Bed | `MushroomBed` | Mushroom bed |
| Grow Light | `GrowLight` | Grow light |
| Sprinkler | `Sprinkler` | Irrigation sprinkler |

## Storage Stations

| Station | Class | Function |
|---------|-------|----------|
| Floor Rack | `FloorRack` | Floor rack |
| PlaceableStorageEntity | `PlaceableStorageEntity` | Placeable storage |
| SurfaceStorageEntity | `SurfaceStorageEntity` | Surface storage |

## Miscellaneous

| Station | Class | Function |
|---------|-------|----------|
| Vending Machine | `VendingMachine` | Vending machine |
| Jukebox | `Jukebox` | Jukebox |
| Toilet | `Toilet` | Toilet |
| Bed | `Bed` | Bed (sleep) |
| Cash Counter | `CashCounter` | Cash counter |
| Laundering Station | `LaunderingStation` | Money laundering station |

## Station Operations

- `ChemistryCookOperation`: Cooking process
- `DryingOperation`: Drying process
- `MixOperation`: Mixing process
- `OvenCookOperation`: Oven cooking process
- `MixChute`: Discharge chute for mixes
