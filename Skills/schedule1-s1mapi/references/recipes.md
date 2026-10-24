# S1MAPI — End-to-End Construction Plans

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 17/18 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Scope

These are implementation contracts, not copy-and-paste code or tested API signatures. Select members from the current target assembly using [building](building.md), [meshes](procedural-mesh.md), [interior](interior.md), and [GLTF](gltf-loading.md). The earlier recipes used members their own reference marked absent; those contradictory chains are removed.

## Shared sequence

1. Define the feature's stable identity, layout data, world prerequisites, authority, and persistence owner.
2. Resolve actual assets and API contracts before mutation.
3. Build one owned inactive root in idempotent phases.
4. Validate geometry/materials/collision before dependent interaction or navigation.
5. Position the root before footprint-dependent terrain and navigation work.
6. Add gameplay only through supported initialization/registration paths.
7. Publish readiness after all mandatory phases succeed; optional failures remain explicit.
8. Teardown runtime resources without losing a pending durable save/recovery record.

## Plan A — Procedural lounge

**Input:** footprint, room height, door/window layout, furniture definitions, lighting budget.

- Generate a shell through verified building methods and openings.
- Place seating and tables in local coordinates with clear walking paths.
- Build a sign mesh and assign an owned emissive material; emission and room illumination are separate decisions.
- Build required navigation after collision and furniture are final.
- Register save data only if the layout is user-modifiable; deterministic static content can rebuild from versioned definitions.

**Acceptance:** doorway traversal, furniture clearance, readable sign in day/night, repeated load without duplicates, and complete owned-resource cleanup.

## Plan B — Showroom with an imported rack

**Input:** shell definition, packaged GLB resource, expected bounds and orientation, material requirements.

- Build the shell and validate the resource exists in the compiled package.
- Import with a supported loader/shader; handle an absent result before accessing transforms.
- Parent and place the rack once in the building's local frame.
- Create collision appropriate to the rack; keep a walkable display aisle.
- Separate visual glass from physical barriers and check transparent sorting.
- Add functional interactions only if required, using verified game contracts.

**Acceptance:** clean installation, missing/corrupt asset, exact rack scale, texture/normal appearance, collision, and repeated reload memory.

## Plan C — Quest in a library

**Input:** static library geometry, stable quest identity, NPC role, destinations, progression state.

- Build geometry and navigation first.
- Register the NPC through S1API's supported discovery/prefab path.
- Create or locate the quest through the verified quest lifecycle; quest creation is not NPC spawning.
- Bind dialogue and progression to stable IDs, and reattach saved callbacks after load.
- Gate readiness so the NPC does not enter a location still under construction.

**Acceptance:** only one NPC and quest instance, reachable destinations, dialogue exit, progression restoration, and no repeat rewards after reload.

## Plan D — Replace one suitable AssetBundle model

**Input:** asset inventory listing mesh, texture, shader, animation, controller, particles, and components.

- Identify which content the deployed GLTF loader actually supports.
- Prototype the static model import and compare appearance, bounds, collision, and runtime cost.
- Keep Unity-specific or unsupported features on a compatible asset-build path.
- Change packaging/deployment and validate a clean install before deleting the original dependency.

**Acceptance:** equivalent required behavior and appearance, measured resource use, documented unsupported features, and reversible asset migration.

## Plan E — Multi-floor building

**Input:** per-floor elevations/bounds, stair and landing geometry, opening definitions, agent dimensions.

- Define every floor/ceiling and connecting stair explicitly.
- Avoid assuming an unverified convenience helper creates valid stairs or navigation.
- Verify headroom, slope, railings/collision, and entry/exit at each level.
- Build and test navigation links or the selected interior path system after final transforms.

**Acceptance:** player and every required NPC can traverse all levels in both directions; removed links or blocked stairs fail visibly without teleporting or trapping actors.

## Failure exercise

For the chosen plan, inject one failure at asset resolution, construction, navigation, network spawn, and save. Confirm the completed prior phase remains consistent or the feature rolls back to a defined safe state. Do not mark a feature tested because the builder method names were found.

## Evidence

The recipes are engineering recommendations composed from this package's corrected ownership/lifecycle contracts. Concrete signatures must come from the linked specialist references and deployed assemblies. No recipe in this revision was compiled or executed in the user's game.
