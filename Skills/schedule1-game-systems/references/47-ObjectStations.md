# Object Stations (Schedule I)

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 28/29 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

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

---

## Unresolved identifiers in the imported 2026-10-08 record

The imported scan did not resolve the following tokens within its recorded search scope. These may be placeholders, attributes from another assembly or drift candidates. Search the actual declaring assembly and version before concluding that an API was removed.

- `MixChute`
