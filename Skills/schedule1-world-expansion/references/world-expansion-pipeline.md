# World Expansion Pipeline

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 5/20 identifier-shaped tokens resolve (9 documented as absent). External-reference symbols are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Evidence and use

These patterns are inherited static observations from user-provided `PierExpansion.dll` and
`DaffysHills.dll` decompiles. Those binaries and their current source were not revalidated during this
revision. Their internal class names below are external-reference anchors, not game APIs. The old
2026-10-08 identifier sweep did not prove runtime compatibility, phase ordering, or numerical settings.

Use the architecture after checking the installed game, framework, asset paths, and native contracts.
The [Hyland case notes](hyland-smoke-shop-property.md) preserve a separate, more detailed evidence record.

## Staged ownership

Keep one world owner and explicit phase states: prerequisites, geometry, property preparation and
registration, authoritative spawn, dependent decor, navigation, population, and live operation.
Record each phase's created roots, registrations, event handlers, material/mesh resources, and
navigation handles. Mark completion only after all required effects succeed.

A repeated callback or readiness retry must reconcile an existing owned phase rather than duplicate
it. Prevent overlapping asynchronous builds and bind callbacks to a session/generation. On failure,
roll back that phase and its dependent phases in reverse dependency order; retain completed unrelated
definitions only when they remain internally valid.

On scene unload, dispose runtime roots, subscriptions, nav data, and caches. Runtime teardown must
not delete durable location/progress records or unresolved save recovery data. Rebuild deterministic
geometry from definitions, restore native-owned entities through native restore, and restore mod-owned
entities only through their designated mod path.

Inherited example: `PierExpansion.Core` separated `Build`, `Prepare`/`SpawnAll`, decor, and navigation;
`ClifftopAccess.Core` delayed a worker until world data was ready. These names identify observed source
roles, not methods available to a new mod.

## Data-driven location definitions

Represent each location with a stable code, display name, source template/asset, world transform,
dimensions, ownership model, capacity, docks, price, and explicit children/systems to remove.
Validate unique IDs, finite values, valid bounds, referenced definitions, and expected hierarchy before
scene mutation. Resolve required source objects first; abort a definition cleanly when they are absent.

A clone retains copied identity, listeners, components, and references until deliberately rebound.
Inventory them and preserve required networking, interactions, save registration, and physics. Do not
remove all unfamiliar components to make a visual shell work. Creating a `Property` component alone
does not establish a functioning registered property.

Inherited example: `PierExpansion.World.PierProperties.MakeDefs`, `Prepare`, and `MakeTemplate` used
property definitions and sanitized clones. Its paths, grids, save points, docks, and network state
must be inspected in the actual target build.

## Authority and restoration

Specify whether clients build local deterministic geometry, receive native network spawns, or apply
custom snapshots. The host owns authoritative world mutations. A client must not both instantiate a
functional native entity locally and later receive another replicated copy.

Assign one restore owner per durable entity before implementing load callbacks. Stable property/entity
IDs and dependent registration must exist at the time the native loader requires them. Do not
unconditionally respawn native-restored buildables at load completion. Pending client requests,
recovery journals, and late-join snapshots belong to the world-state/persistence contract.

## Navigation build and teardown

1. Identify the actual navigation system and every required agent type. Do not substitute a Unity
   NavMesh example for an installed A* or framework-specific path without an adapter contract.
2. Stabilize final geometry transforms and inspect collision sources; exclude irrelevant triggers and
   disabled/invalid geometry according to the chosen builder's semantics.
3. Derive bounds from the actual playable colliders with deliberate padding and cost limits.
4. Build each required agent representation and report failures separately; a successful pedestrian
   path does not establish every worker/vehicle route.
5. Add explicit links where supported for doors, gaps, bridges, or transitions; test both directions.
6. Retain every created data/link handle, including `NavMeshDataInstance`/`NavMeshLinkInstance` when
   those are the verified API, and remove them safely on rebuild/unload.
7. Record source counts, agent types, link failures, build latency, and unreachable destinations.

Inherited example: `PierExpansion.World.NavBuilder.Build`, `JoinEnds`, `AddDoorLinks`, and `Clear`
kept per-agent data and teardown handles. Reuse the ownership principle, not its unverified constants.

## Rendering optimization boundaries

Before combining static meshes, exclude LOD-managed, animated, skinned, or behavior-sensitive renderers.
Group compatible meshes by material, layer, shadow behavior, and spatial cell; preserve transforms,
submeshes, bounds, and other relevant renderer state. Cap chunk/index sizes and skip unreadable or
missing meshes. Keep colliders, interactions, and native gameplay objects separate.

Record ownership of newly combined meshes/materials and release them on rebuild. Do not destroy borrowed
source meshes. Compare culling, shadows, lightmap/probe behavior where used, memory, build time, and
frame cost before/after. Fewer renderers alone is not proof of a faster scene.

Inherited example: `PierExpansion.World.MeshMerge.Merge` grouped by material/shadow/layer/cell and
skipped LOD renderers. This is a design lead, not proof for a different shader or scene.

## Layout-driven population

Use explicit layout records for homes, transforms, doors, hangouts, shops, and removed locations.
Parse culture-invariant numeric values; bound file/row counts; validate field counts, unique IDs,
finite coordinates, and referenced locations. Sort deterministically where order influences choices.
Do not index an empty fallback list after rejecting malformed rows.

Resolve occupied or removed homes through a deliberate alternative policy, then validate route
reachability and schedule dependencies. If no valid home exists, stop that resident's dependent
initialization with a clear reason instead of spawning it at a guessed coordinate.

Inherited example: `ClifftopAccess.StreetLayout.Load` and `HomeSlot` used embedded/user layout data and
removed-home mapping. Embedded positions and hard-coded fallbacks require current-map verification.

## Exit criteria

- Repeated scene entry and same-save reload produce one owned instance/registration per location.
- Host/client/late-join behavior matches the selected authority and restore models.
- Every required route reaches its destination through the intended doors/links.
- Missing definitions and partial builds yield bounded diagnostics and correct phase rollback.
- Unload removes runtime resources without deleting durable world state or recovery evidence.
- Actual rendering, interaction, save restoration, and performance have been measured in the game.
