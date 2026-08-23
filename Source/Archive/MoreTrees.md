# MoreTrees v1.0.1

> **Directory:** `MoreTrees`
> **Author:** iJamesss
> **Category:** World & Environment
> **Entry Class:** Mod extends `MelonMod`
> **Target Framework:** netcoreapp6.0
> **Files:** 4 (2 .cs)

## Purpose

Densifies vanilla terrain trees and introduces additional trees around properties (Bungalow, Motel, Barn, Manor, RV, etc.) to enhance natural visual screening. Keeps trees away from property core centers, prevents inter-tree collision, and honors keep-out zones.

## Entry Point & Architecture

`MoreTrees\Mod.cs` — Lifecycle:
- `OnSceneWasLoaded`: When loading `Main` and not yet applied, launches coroutine `Apply()`.
- `Apply()`: Polls up to 80 × 0.1s for `Terrain.activeTerrains`, builds property zones from `@properties` colliders, clears trees within keep-out zones, increases tree scaling, executes a 3-second resync loop, then optionally builds overlay geometry.

Architecture: Pure terrain manipulation system — no NPCs, no UI, no save files. Data structures:
- `PropertyZone { Name, BoundsList, Canopy }` — per detected property.
- `Spatial Grid (12 m)` — accelerates distance queries.
- `Baseline` Dictionary per terrain — stores original `TreeInstance` scaling so re-applications remain deterministic.

## Key Files

- `MoreTrees/Mod.cs` — Complete mod (~1080 lines, single file implementation).

## Patterns & Lessons Learned

- Stable Pseudo-Random: `StableVariance(TreeKey, seed)` using XOR-shift hashing on deterministic tree positions — identical property coordinates produce identical results after save reloads.
- Spatial Hashing: `Dictionary<long, List<BoundRef>>` using `(cx<<32) ^ cz` key for O(1) distance lookups.
- Terrain Projection: `Physics.Raycast(Vector3.down, 180f, ...)` cast from 60m above target point — projects candidate tree points onto actual terrain surface.
- Overlap Check: `Physics.OverlapCapsule` with HeightClearance=2.5m prevents trees inside/on top of structures.
- Cell-based occupancy grid (`occ: Dictionary<long, List<Vector2>>`) enforcing minimum spacing between trees.
- Bounding box surrounding property colliders with property-specific padding offsets (`ExtraPad`: RV +4m, Sweatshop -1m).
- Hard cap at 1000 additional trees as a performance safety guard.

## Dependencies

**MelonLoader References:** MelonLoader, Harmony, Assembly-CSharp, UnityEngine

## Cross-References

- See `Analysis/Systems/43-MapAndLocations.md`
- See `Analysis/Learnings/19-Trash-Environment-World.md` (World modification)
- See `Analysis/Learnings/18-Visual-Enhancement-Patterns.md`

---
*Created from source code analysis*