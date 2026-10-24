# S1MAPI — GLB/GLTF Loading

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 10 of 10 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence boundary

Official S1MAPI loader documentation was consulted on 2026-10-21. Published guides and snippets can drift, including descriptions of external images and coordinate conversion. The deployed loader/source and a representative imported asset determine the supported behavior. The earlier unverified `LoadGltf(textData, additionalBinaries)` recipe is removed.

## Select the loading path

The public `S1MAPI.Gltf.GltfLoader` API lists `Load`, `LoadGlb`, `LoadFromFile`, and `LoadFromJson`; return values can be absent. Match the actual target signatures and preserve the failure result instead of immediately dereferencing the root.

| Source | Contract |
|---|---|
| Assembly resource | Confirm exact resource name, open the managed assembly resource stream, check absence, read within limits, dispose the stream |
| User asset file | Resolve the approved path, bound size, read with error handling, and preserve the last usable asset on failure |
| GLTF JSON with dependencies | Resolve relative dependencies against a controlled base path and confirm importer support for each resource type |
| Unity Resources asset | Use only when the asset actually exists in the game's or bundle's Unity asset database |

A `.csproj` EmbeddedResource is not made available to `UnityEngine.Resources.Load` merely by placing it in a folder named Resources.

## Import workflow

1. Inspect the file header/format, external dependencies, textures, extensions, geometry count, and expected dimensions.
2. Prefer a representative uncompressed GLB with embedded supported textures for the first compatibility test.
3. Do not enable Draco or another extension until the deployed importer explicitly supports it.
4. Verify animation/skinning requirements separately. GLTF interchange is not a complete replacement for Unity AssetBundles or controllers.
5. Read/decode file data separately from Unity object creation where supported; perform Unity operations on the required main thread.
6. Select a shader known to work in the target pipeline. A generic importer fallback is not proof of compatibility.
7. Handle null/exception and partial-import cleanup before publishing the new root.
8. Parent under an owned root, apply the selected local/world transform, and inspect scale and orientation with an asymmetric calibration model.
9. Check materials, submeshes, normals, tangents, UVs, alpha, emission, and packed-map conventions in-game.
10. Add collision from the intended physical shape; a single root BoxCollider is rarely a valid room interior.

## Coordinates and scale

Blender, GLTF, the importer, and Unity can each apply conversion. Identify which stage performs each conversion and do it once. Do not repair an unexplained orientation error with a negative scale without testing triangle winding, normals, tangents, and physics.

Use a measured unit cube and an asymmetric direction marker. Preserve both expected local bounds and resulting world bounds. Replace historical guesses such as "Blender meters differ from Unity meters" with measured import/export settings.

## Cache and ownership

Cache immutable imported resources only when the loader's sharing contract is known. Distinguish the prototype root, live instances, shared meshes/materials/textures, and generated conversion textures. Repeated import can allocate additional runtime resources; destroying a root is not sufficient evidence that all owned resources are released.

Build and validate a replacement before swapping it in. On failure release only the new partial resources and retain the previous asset. Cancel or discard stale results after a scene/owner change.

## Acceptance tests

Test missing/empty/truncated files, unsupported extensions, absent external dependency, material fallback, transparent/emissive surfaces, multiple submeshes, repeated reload, large files, and teardown. Package the exact runtime assets and verify a clean build/deploy without manual files in the game folder.

## Sources and anchors

- [Official GltfLoader API](https://ifbars.github.io/S1MAPI/api/S1MAPI.Gltf.GltfLoader.html) — public entry points and nullable result.
- [Official GLTF guide](https://ifbars.github.io/S1MAPI/docs/gltf-loading.html) — inspect feature limits against the deployed importer; guide is mutable.
- Source search: `ThirdParty/S1MAPI/Gltf/` for `GltfLoader` / `GltfImporter`.
- [Blender export](../../schedule1-3d-assets/references/blender-export.md) and [material ownership](materials.md).
