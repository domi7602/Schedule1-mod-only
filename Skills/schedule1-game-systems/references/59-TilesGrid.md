# Tiles & Grid (Schedule I)

> **Redirect stub** (consolidated 2026-10-05): mechanics (2D grid 0.5 units, BuildManager flow, ghost validation) live in **[`10-Building-Construction.md`](10-Building-Construction.md)**; outdoor-placement working knowledge in the `schedule1-grid` skill. This file keeps only the class register. Class-list only — not yet re-verified against 0.4.7f9.

## Core Classes

| Class | Purpose |
|-------|---------|
| `Grid` | Main grid system (manages all tiles of a property) |
| `Tile` / `IndoorTile` / `FootprintTile` / `ProceduralTile` | Tile types (base / under roof / building footprint / random) |
| `Coordinate` (+ Pair/TilePair/FootprintPair/ProceduralPair) | Grid coordinate system |
| `TileDetector` / `ETileDetectionMode` | Detection (raycast, trigger, physics) |
| `TileAppearance` / `ETileColor` | Visual appearance / color |
| `GridItem` / `ProceduralGridItem` / `BuildableItem` / `SurfaceItem` | Entity-framework grid entities |

## Related

- Mechanics + hook points: [`10-Building-Construction.md`](10-Building-Construction.md)
- Outdoor placement (disable GridItem, never destroy): `schedule1-grid` skill
- Decompiles: `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/Tiles/`
