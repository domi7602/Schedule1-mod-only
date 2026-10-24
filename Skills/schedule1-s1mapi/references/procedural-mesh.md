# S1MAPI — Procedural and Custom Meshes

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 29 of 35 identifier-shaped tokens resolve (6 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence boundary

Public API documentation was inspected on 2026-10-21; it describes its published assembly, not the user's deployed DLL. The original skill also recorded a local source inspection on 2026-10-08. Use member names below as lookup targets and verify signatures, output components, and ownership in the target assembly before implementation.

## Pick the builder

| Need | Documented entry point | Integration decision |
|---|---|---|
| Composite primitive geometry | `S1MAPI.ProceduralMesh.ProceduralMeshBuilder` | One builder for one intended composite result |
| Explicit vertices, triangles, UVs | `S1MAPI.ProceduralMesh.CustomMeshBuilder` | Validate topology and attribute counts before build |
| Repeated copies of the same model | Cached mesh/material plus owned instances | Share only resources whose mutation/lifetime contract allows it |

The public primitive API lists `AddBox`, `AddSphere`, `AddCylinder`, `AddCapsule`, `ApplyFlatShading`, `SetColor`, `SetMaterial`, `Build`, and `BuildMesh`. Cylinder/capsule shapes are endpoint-based. The custom builder owns vertex/triangle/quad and UV assembly. Check overloads in the linked API rather than deriving them from a method name.

## Construction contract

1. Define local geometry, material groups, intended pivot, and collision role before creating objects.
2. Validate finite coordinates, positive dimensions/radii, segment limits, and index/vertex budgets.
3. Resolve a supported material. Decide whether it is borrowed, shared by this mod, or owned exclusively by the new object.
4. Add geometry in one coordinate space; avoid embedding world offsets into vertices and then applying the same offset to the root.
5. Build the intended result once. Inspect returned root, mesh, renderer, material assignment, and collider behavior.
6. Add or configure collision explicitly if the verified builder output does not supply the required collider.
7. Register all created resources under an owner before activation so failure cleanup is complete.
8. Position/parent the root, then verify bounds, normals, seams, and collision in the target game.

## Builder accumulation

Do not use repeated append-and-build calls as an independent-object factory unless the actual implementation documents a reset. If the builder retains vertices, each later result can include all prior shapes. Choose either:

- one composite: append all desired shapes and build once;
- independent results: use a fresh builder for each result;
- repeated identical results: build/cache a prototype and instantiate under an explicit sharing contract.

Verify material reuse separately. Reusing a builder does not itself establish ownership or memory efficiency.

## Practical compositions

### Pallet and crates

Define a base and crate transforms as data. Use one composite only when shared collision/material/visibility fits the design; otherwise parent separate results under one owned root. Reserve the root ID before creating children so retries cannot duplicate crates. Supply colliders on the objects that must participate in physics.

### Table with legs

Build the top with its intended dimensions. Derive each leg's lower and upper endpoints from the table-local frame; do not substitute center/radius/height arguments for an endpoint-based cylinder API. Parent all parts before world placement. Verify the leg ends meet the top and the floor.

### Emissive lamp

Create the emissive material through a verified material helper or a supported Unity material configuration, then pass that material to the mesh builder. Add a separate light only if the scene should actually be illuminated. Limit its range/shadows and verify the day/night result.

## Retired recipes

The original package's calls to `SetMaterialPreset`, `SetEmission`, `SetTransparency`, `SetCollider`, and `SetStatic` on `ProceduralMeshBuilder` contradicted its own source inspection. Those recipes are removed. Do not restore them or invent substitutes with similar names. `AddQuad` and `AddTriangle` belong to the custom geometry workflow, not the primitive builder.

## Verification and performance

Check empty/invalid geometry, index limits, negative scale, duplicate builds, missing shader, material lifetime, and destruction while another instance still shares the mesh. Measure actual vertices, draw calls, collider cost, and construction time. Do not claim baked lighting for runtime-created geometry merely because a static flag is set.

## Sources and local anchors

- [Official ProceduralMeshBuilder API](https://ifbars.github.io/S1MAPI/api/S1MAPI.ProceduralMesh.ProceduralMeshBuilder.html)
- [Official CustomMeshBuilder API](https://ifbars.github.io/S1MAPI/api/S1MAPI.ProceduralMesh.CustomMeshBuilder.html)
- Historical source anchor: `ThirdParty/S1MAPI/ProceduralMesh/ProceduralMeshBuilder.cs`; resolve the actual path in the current checkout.
- [Material ownership](materials.md) and [persistence](persistence.md).
