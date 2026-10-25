---
name: schedule1-s1mapi
description: "Build Schedule I geometry with verified S1MAPI contracts."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Schedule I — S1MAPI Integration

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Treat this skill as a module-selection and integration guide, not a replacement for the installed assembly.

## Establish prerequisites

1. Identify the deployed S1MAPI assembly, runtime branch, project reference, and source revision.
2. Compare local source with the deployed DLL. Public documentation can describe a different release.
3. Verify the fully qualified type, visibility, parameters, return type, and side effects for every selected member.
4. Decide whether the output is decorative geometry, an interactive game entity, or a native networked prefab.
5. Establish world readiness, spawn authority, save ownership, and a deterministic teardown path.

Do not switch this repository to Mono references merely because public documentation is generated
from a Mono assembly. Cross-runtime distribution requires the actual dependency contract and validation
on each supported branch; a single DLL is not guaranteed by a documentation label.

## Select the module

| Need | Read |
|---|---|
| Primitive or custom geometry | [Procedural mesh](references/procedural-mesh.md) |
| Material properties and ownership | [Materials](references/materials.md) |
| Building shell, walls, roof, doors | [Building](references/building.md) |
| Furniture and functional prefabs | [Interior](references/interior.md) |
| GLB/GLTF loading and asset packaging | [GLTF loading](references/gltf-loading.md) |
| Terrain, navigation, prefab integration | [World tools](references/world-tools.md) |
| End-to-end construction plan | [Recipes](references/recipes.md) |
| Save/reload or native restoration | [Persistence](references/persistence.md) |

## Integration workflow

1. Keep geometry definitions separate from runtime objects. Use stable IDs and explicit coordinate spaces.
2. Resolve required assets and supported API members before modifying the scene.
3. Create one inactive owned root after the relevant world is ready.
4. Build geometry with an explicit material owner; inspect generated colliders instead of assuming they exist.
5. Position the final hierarchy before building navigation. Test door widths, stairs, and entry/exit reachability.
6. Add gameplay integration through verified S1API or native components. A visible mesh does not imply inventory, interaction, networking, or saving.
7. Spawn authoritative entities on the host. Treat client links/callbacks and late join as distinct readiness phases.
8. Activate the completed phase and publish its registry only after its prerequisites succeed.
9. Rebuild idempotently from definitions. On partial failure, destroy only owned partial objects and retain diagnostics.
10. Release navigation, listeners, roots, and owned runtime resources at their actual end of life.

## Guardrails

- Do not revive old recipes using methods this package has explicitly retired.
- Do not assume repeated `Build()` clears accumulated builder geometry.
- Do not equate network spawning with save persistence.
- Never respawn an entity until its native-versus-mod restoration owner is known.
- Never assume one added component fully initializes a native networked door or property.
- Treat documentation claims about update resilience, compression, auto cleanup, and performance as claims to test in the target integration.

## Verification

Select relevant checks: bounds, normals, materials, collision, navigation, missing assets, repeated
builds, reload, slot switch and teardown. Host/client and late-join tests require explicit scope.
Measure construction latency and resources; do not assume constant-time behavior.

## Evidence and output

Official S1MAPI documentation was consulted on 2026-10-21; references distinguish it from inherited
observations. The deployed S1MAPI DLL and S1API 3.2.1 runtime were not supplied for that revision. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->
Report integration assumptions and selected API evidence under the shared policy.
