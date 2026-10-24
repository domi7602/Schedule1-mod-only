# S1MAPI — Terrain, Navigation, and Prefab Integration

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 7 of 7 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence boundary

Public S1MAPI documentation was consulted on 2026-10-21. It places structural terrain tools under `S1MAPI.Building.Structural` and building navigation under `S1MAPI.Building`. Verify the deployed source/assembly before using members; the old universal `S1MAPI.World` recipes and static navigation calls are not retained.

## Terrain preparation

1. Inspect the target footprint, existing world objects, terrain data, ownership, and dependent gameplay systems.
2. Plan the smallest required change and record what must be restored or regenerated.
3. Resolve the installed `TerrainClearer` / `TerrainFlattener` contracts, including deferred retries.
4. Position the building before operations whose footprint comes from its transform.
5. Restrict removal to approved object categories and bounds; do not clear arbitrary nearby interactables or networked entities.
6. Validate the resulting surface, grass/detail state, collision, and neighbors.
7. Make repeated preparation idempotent and cancel stale deferred work when the owning world changes.

The public `TerrainFlattener.FlattenUnder` documentation describes lowering terrain and an outcome that can indicate a queued retry. Therefore a return without an immediate result is not necessarily final failure or readiness. Inspect the actual completion mechanism before advancing construction.

Do not claim terrain operations are O(1). Cost depends on affected data, scene queries, synchronization, and implementation. Measure frame cost and completion time on the intended footprint.

## Navigation ownership

The public `NavigationBuilder` is a building-specific system that can combine carving with interior pathfinding. It is not a universal static region rebake call.

- Build after final placement and valid collision/openings.
- Keep the returned owner and its supported build/rebuild/remove lifecycle.
- Rebuild only after relevant layout changes, with debounce/batching where needed.
- Coordinate release of NPCs before removing navigation they still use.
- Validate each required NPC agent, entry, destination, exit, and blocked route.

For wider world NavMesh changes, follow the [world-expansion pipeline](../../schedule1-world-expansion/references/world-expansion-pipeline.md) and verify the native agent/build-data API separately.

## Functional prefabs

1. Resolve an actual registered prefab and inspect its required components and serialized links.
2. Separate mesh-only decoration from a functional/native entity.
3. Confirm spawn authority and position/parenting before the network spawn when the transport contract requires it.
4. On clients, handle deferred arrival/linking rather than creating a competing local entity.
5. Verify which customization callbacks run on the server versus each client.
6. Track stable identity, teardown, and restore responsibility independently of network identity.
7. Treat absent prefab or failed registration as a failed phase; do not repair arbitrary network graphs by attaching components.

Network replication does not prove durable saving. Read [persistence](persistence.md) before promising restoration. If the native save system might restore the same entity, resolve that question before adding an unconditional respawn path.

## Example expansion plan

For a hotel extension: validate footprint → prepare owned shell → position it → clear only approved obstructing scenery → finish collision → build navigation → add verified functional prefabs → attach gameplay → publish readiness. Each phase has an owner and rollback boundary. Keep definitions independent of these live objects so the process is repeatable.

## Verification

Check absent terrain, deferred completion after scene exit, removal outside the footprint, player collision, unreachable doors, failed network spawn, late join, native save restoration, repeated build, and cleanup. Compare frame-time and resource measurements for the smallest and largest supported layouts.

## Sources and anchors

- [Official structural namespace](https://ifbars.github.io/S1MAPI/api/S1MAPI.Building.Structural.html).
- [Official TerrainFlattener API](https://ifbars.github.io/S1MAPI/api/S1MAPI.Building.Structural.TerrainFlattener.html).
- [Official NavigationBuilder API](https://ifbars.github.io/S1MAPI/api/S1MAPI.Building.NavigationBuilder.html).
- [Official PrefabRef API](https://ifbars.github.io/S1MAPI/api/S1MAPI.Core.PrefabRef.html) — confirm exact spawn semantics from the installed implementation when prose is ambiguous.
- Local source search: `ThirdParty/S1MAPI/` for terrain tools, `NavigationBuilder`, prefab placement and client linker types.
