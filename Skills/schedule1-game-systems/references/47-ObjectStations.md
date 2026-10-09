# Object Stations (Schedule I)
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

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `MixChute`
