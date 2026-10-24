---
name: schedule1-3d-assets
description: "Create Schedule I meshes, materials and texture assets. Keywords: Blender, 3D, Mesh, URP, Universal Render Pipeline, Shader, Material, Lit, Unlit, Pink Texture, Flipped Normals, Backface Culling, ObjLoader, GLTF, GLB, AssetBundle, Bone Attachment, Spine2, Humanoid, Zero Collider, Scale."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — 3D Assets

Apply the [shared working contract](../schedule1-modding/references/working-contract.md).
Choose the asset pipeline from the required content and the actual loader capabilities.

## Establish prerequisites

- Inspect the supplied asset, its source authoring file, intended pivot, units, and animation needs.
- Identify the installed runtime, render pipeline, supported shader variants, and loader.
- Determine who owns meshes, materials, textures, colliders, and instantiated roots.
- Measure the target avatar or prop in world space; do not promote one historical local-space measurement to a universal game scale.
- Record visual and performance acceptance criteria before changing export settings.

## Choose a pipeline

| Asset need | Candidate | Required check |
|---|---|---|
| Simple geometry | Unity mesh or S1MAPI builder | Topology, collider, material ownership |
| Static simple prop | Existing OBJ loader | UV/normal/material support and axis conversion |
| Textured static model | S1MAPI GLB/GLTF loader | Supported extensions, texture formats, failure behavior |
| Complex animation, particle system, custom shader | Unity AssetBundle | Game/editor compatibility and retained shader variants |

Do not replace AssetBundles wholesale with GLB. Mesh interchange does not carry every Unity-specific
component, shader, animation controller, or effect. Code can use existing shaders and retained variants;
a new shader needs a compatible build/asset route. A C# DLL is not a general Unity shader compiler.

## Workflow

1. Preserve the original authoring asset. Normalize only the transforms required by the selected pipeline.
2. Preserve the chosen pivot and rig contract; applying all transforms blindly can change attachment or animation behavior.
3. Verify normals, triangle winding, UVs, negative scale, and mesh bounds.
4. Export a small representative asset and validate scale, orientation, and materials before a full batch.
5. Resolve a shader supported by the running pipeline. If none is available, fail the visual feature cleanly or preserve a verified existing material.
6. Reuse borrowed materials read-only. For isolated changes use a deliberately owned material or a tested property override.
7. Build collider policy by role: cosmetic wearable, decorative prop, or interactive physical object.
8. Attach to an unambiguous target instance/bone and keep ownership records for replacement and cleanup.
9. Load new resources before replacing old ones; on failure destroy partial owned resources and retain the last usable asset.
10. Release owned runtime resources when their last consumer is gone; do not destroy borrowed game assets.

## Failure and performance checks

- Missing shader, corrupt image, unsupported GLTF extension, missing bone, and invalid scale must give an actionable bounded diagnostic.
- Disable cosmetic colliders before deferred destruction when immediate collision/raycast exclusion is required.
- Avoid full-scene searches and uncached material instantiation in per-frame paths.
- Profile upload size, draw calls, material count, and repeated reload memory. A property block is not automatically faster under URP.
- Test the visible result under daylight and artificial light, from all relevant distances and animation poses.

## Reference routing

| Trigger | Reference |
|---|---|
| Axis, pivot, units, normals, export settings | [Blender export](references/blender-export.md) |
| Pink model, PBR, material ownership, image failure | [Rendering and materials](references/urp-rendering-materials.md) |
| Clothing, backpacks, held props, mannequin | [Rigging and attachment](references/rigging-and-attachment.md) |

## Evidence and output

Unity ownership and resource-lifetime guidance is linked to official documentation in the references.
Game-specific paths, bones, dimensions, and loader behaviors require the installed asset/runtime.
Return the chosen pipeline, assumptions, changed assets/settings, measured checks, and remaining in-game
verification. Do not label static identifier matches as visual or runtime validation.
