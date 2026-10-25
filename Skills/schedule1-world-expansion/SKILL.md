---
name: schedule1-world-expansion
description: "Add Schedule I world geometry, travel and spawn behavior."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Schedule I — World Expansion

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Coordinate geometry, property integration, navigation, population, persistence, and teardown as separate stages.

## Establish prerequisites

1. Map the source objects/assets, exact hierarchy evidence, world readiness, and required native components.
2. Identify the target game/framework builds and compare source with deployed assemblies.
3. Specify stable location identity, ownership, save responsibility, and host/client construction roles.
4. List required navigation agent types, route endpoints, door links, and fallback destinations.
5. Define the playable footprint, budget, and acceptance tests before optimizing meshes.

## Procedure

1. Keep layout definitions separate from construction: identity, transform, template, bounds, doors, docks, and optional features.
2. Validate definitions, finite coordinates, resource existence, and native registration requirements before scene mutation.
3. Build in retry-safe phases: prerequisites → owned geometry root → property preparation → authoritative spawn → dependent decor → navigation → population.
4. Mark a phase complete only after success. A repeated callback must reuse or reconcile existing owned objects.
5. Clone inactive where possible. Remove only inventoried child systems; preserve required network, interaction, save, and physics components.
6. Give cloned locations stable IDs and rebind all dependent native references. A Property component alone is not a registered property.
7. Position geometry before building navigation; preserve every created navigation/link handle and remove it on replacement.
8. Validate each required agent and both directions through door/bridge links. Coordinate any asynchronous release of native navigation.
9. Derive residents' homes/routes from validated layout data. Handle removed, occupied, or missing locations with a tested fallback.
10. Choose one restoration owner for every object. Do not combine native restoration with unconditionally spawning another copy.
11. Teardown runtime roots, subscriptions, navigation, and caches for the relevant world lifetime. Keep pending save/recovery data until its outcome is resolved.

## Optimize only after correctness

Group static render meshes only when material, layer, shadow behavior, and spatial chunk match.
Exclude LOD-managed or behavior-sensitive renderers and preserve colliders/gameplay components.
Compare visibility, shadows, collision, renderer counts, construction latency, and memory before/after.
Do not assume a smaller renderer count improves every target scene.

## Failure handling

- Missing required template/registration data aborts that definition before activation.
- A partial phase failure removes only that phase's owned resources and leaves a bounded diagnostic.
- Navigation failures may disable a dependent route; do not silently spawn residents into unreachable locations.
- Reject duplicate IDs and conflicting existing objects instead of silently adopting foreign state.
- Do not persist an incomplete construction as the authoritative saved world.

## Verify

Select relevant checks: clean installation, scene re-entry, reload, slot switch, each navigation agent,
entrances/exits, optional features, absent assets, interrupted construction/save and cleanup.
Host/client and late-join tests require explicit scope. Record construction time and support limits.

## Read on demand

| Need | Reference |
|---|---|
| Phases, navigation, mesh grouping, layout-driven residents | [Expansion pipeline](references/world-expansion-pipeline.md) |
| Detailed historical custom-property and native-asset case | [Hyland Smoke Shop notes](references/hyland-smoke-shop-property.md) |
| Build-mode placements | [Grid](../schedule1-grid/SKILL.md) |
| Room and entrance implementation | [Interiors](../schedule1-interiors/SKILL.md) |
| Population definitions and schedules | [Custom NPCs](../schedule1-custom-npcs/SKILL.md) |
| Procedural construction library | [S1MAPI](../schedule1-s1mapi/SKILL.md) |

## Evidence and output

Case references are inherited binary observations, not current API/coordinate/constant guarantees.
Report dependencies, ownership/restore decisions, phases and rollback boundaries under the shared evidence policy.
