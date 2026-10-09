# Map & Locations (Schedule I)

> **Redirect stub** (consolidated 2026-10-05): region mechanics/unlocks live in **[`13-Regions.md`](13-Regions.md)**; the live-verified map-POI/S1API `MapPOIBuilder` analysis lives in the map section of **[`07-Vehicle.md`](07-Vehicle.md)** (verified 2026-09-25). This file keeps only the class register. Class-list only — not yet re-verified.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 20 of 20 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Core Classes

| Class | Purpose |
|-------|---------|
| `Map` / `MapPositionUtility` / `EMapRegion` | Main map / positions / regions enum |
| `MapApp` / `UIMapPanel` / `UIMapItem` | Phone map app + UI |
| `POI` / `NPCPoI` / `MapRegionData` | Points of interest |
| `AccessZone` / `TimedAccessZone` / `NPCPresenceAccessZone` | Access control zones |
| `Gate` / `ManorGate` / `Ladder` | Traversal |
| `ParkingLot` / `ParkingSpot` | Parking |
| `SewerCameraPresense` | Camera presence |

## Named locations

DarkMarket · Dealership · MedicalCentre · PoliceStation · SewerManager · AutoshopAccessZone · ManorGate

## Related

- Regions/unlocks: [`13-Regions.md`](13-Regions.md) · Manor constants: [`54-Property.md`](54-Property.md)
- S1API map hooks: `schedule1-s1api/references/game-systems.md` + 07 map section
