---
name: schedule1-world-expansion
description: >-
  Runbook for staged Schedule I world expansions, cloned locations, custom properties,
  navigation, and population. Use when a mod adds connected areas, buildings, or residents.
  Keywords: world expansion, custom property, map layout, NavMesh, clone, world build, NPC population.
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). This skill was informed by static inspection of user-provided third-party mod assemblies on 2026-10-08; none of the patterns were runtime-verified. Re-check live game APIs before use.

# Schedule I — World Expansion

Use this runbook to coordinate world geometry, properties, navigation, NPCs, and cleanup as separate stages. It complements `schedule1-grid`, `schedule1-interiors`, `schedule1-custom-npcs`, and `schedule1-s1mapi`; it does not replace their API-specific rules.

## When to Use

- Adding a district, road, pier, building cluster, or other persistent world feature.
- Cloning vanilla properties or props to create new world content.
- Adding NPCs whose homes or routes depend on a changed map layout.
- Rebuilding navigation or optimizing a large static scene.

**Don't use for:** a single furniture placement, isolated room, or standalone NPC; load the matching domain skill instead.

## Procedure

1. **Map dependencies.** Identify source objects, lifecycle readiness, scene roots, property registration requirements, navigation agents, NPC routes, and which side owns networked spawning. Verify source names and required components in the current game.
2. **Keep definitions separate from construction.** Represent location identity, transform, source template, footprint, doors, docks, and optional features as data. Validate every definition before touching Unity objects.
3. **Build in ordered, retry-safe phases.** Wait for map/terrain readiness; create one owned root; build geometry; prepare/register properties; spawn them only on the authoritative server; then add dependent decor and navigation. A repeated call must not duplicate the world.
4. **Clone defensively.** Find and validate the source hierarchy before cloning. Clone inactive where practical, remove only explicitly identified child systems, reset or remove nested `NetworkObject` state deliberately, assign stable IDs, and log missing or changed source paths. Do not assume a hierarchy path survives a game update.
5. **Treat navigation as a separate product.** Collect valid collision/build sources, build for each required agent type, add explicit door/bridge links where needed, record the returned data/link instances, and remove them on scene unload. Measure build time and report partial failures.
6. **Preserve gameplay while optimizing visuals.** Group static meshes only when material, layer, shadow behavior, and spatial chunk match. Exclude LOD-managed renderers and special assets; leave colliders and gameplay components intact. Compare renderer counts and verify visibility, shadows, and collision in-game.
7. **Make population layout-aware.** Keep NPC archetypes reusable and derive homes/routes from an explicit layout source. Parse numbers invariantly, represent removed/occupied locations, and provide a safe fallback when the external layout is missing or invalid.
8. **Own cleanup.** On scene unload, clear navigation instances, event subscriptions, spawned managers, static caches, and owned roots. Re-entering the scene must build one clean copy.
9. **Verify in-game.** Test clean install, repeated scene entry, host and client, each NPC agent type, door/bridge traversal, missing source assets, disabled optional features, and cleanup after leaving the scene. Record logs and measured build time.

## Pitfalls

- A visible mesh does not imply a usable collider or navigable path.
- Removing child network objects can fix nested clone identity, but can also remove required networked behavior; inventory and test every removed component.
- Agent-specific voxel settings, links, and no-go areas are layout-specific, not universal constants.
- Static batching can break LOD, shadow, layer, material, or renderer visibility behavior.
- Embedded coordinates and fallback positions drift as the world changes; validate them against the current map.
- Decompiled third-party output may contain unresolved IL and is not proof of runtime behavior. Never copy a third-party implementation wholesale.

## References

- `references/world-expansion-pipeline.md` — staged architecture and evidence-backed checks.
- `../schedule1-grid/SKILL.md` — build mode, placement, and grid safety.
- `../schedule1-interiors/SKILL.md` — interior construction and doors.
- `../schedule1-custom-npcs/SKILL.md` — S1API NPC definitions, schedules, and lifecycle.
- `../schedule1-s1mapi/SKILL.md` — procedural assets and world tools.
