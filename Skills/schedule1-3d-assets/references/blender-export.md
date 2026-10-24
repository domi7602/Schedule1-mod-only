# Blender Export and Geometry Preparation

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 4 of 4 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use this reference for units, pivots, export orientation, normals, and wearable scale.
The inherited game-specific examples came from a 2026-10-08 identifier check, not a measurement of
all game assets. Blender documentation supports the transform guidance; the actual loader and
exporter version determine the final conversion. Record both versions with the exported artifact.

## Preserve the authoring contract

Keep the source `.blend` and export from a duplicate or a controlled export collection.
Decide which point must remain the runtime pivot before applying transforms: a hand grip, the contact
surface of a backpack, the base of furniture, or the centre used by a placement system.

Applying location, rotation, and scale is not a universally safe preparation step. Applying transforms
changes their representation in object data. For an armature, pose transforms, animation curves, and
constraints are not automatically adjusted to preserve every rig relationship. Normalize necessary
scale/rotation before rigging where practical; otherwise test the existing rig and export conversion.
Do not apply location merely to obtain zeroes when that would change the required pivot.

## Choose orientation with the actual loader

| Route | Starting point | Required round-trip check |
|---|---|---|
| OBJ | Exporter axis controls; `-Z Forward`, `Y Up` is a candidate used by the old workspace | Determine whether the OBJ loader already converts axes or winding |
| GLTF/GLB | Exporter's glTF coordinate conversion, normally its `+Y Up` option | Verify the installed GLTF loader's handedness conversion exactly once |
| Unity asset pipeline | Compatible Unity project and importer settings | Verify final runtime transform, rig, and material assignments |

Blender's authoring world is Z-up and Unity uses Y-up, but an object's intended forward direction is
an asset convention. Do not apply an additional negative scale or rotation from a remembered recipe.
A loader may already have performed the conversion.

Export an asymmetric calibration model with a labelled front, top, and right side, a known measured
edge, and a visible pivot marker. Inspect its world bounds, facing direction, winding, UVs, and root
transform after import. Correct one conversion stage and document it before exporting a batch.

## Geometry checks

- Inspect face orientation from both sides. Blue/front and red/back indicate the viewed face side;
  they do not prove that an entire model is a closed manifold with consistently outward faces.
- Recalculate outside normals for appropriate closed surfaces, then inspect open cloth, thin sheets,
  and intentionally two-sided geometry separately.
- Distinguish shading normals from triangle winding. Reversing a normal does not always repair a
  backface-culling problem caused by mirrored transforms or incorrectly wound triangles.
- Inspect negative object/parent scales and tangent handedness when normal maps appear inverted.
- Include UVs and normals required by the loader. Decide where triangulation happens and check the
  exported triangles, especially concave faces and deformed meshes.
- Export modifiers only when their evaluated result is intended. Check skin weights and shape keys
  before applying destructive modifiers.
- Use a collision mesh suited to the role; the render mesh is not automatically a good collider.

## Measure game scale

Measure the target runtime object in world space and record parent scale. Use renderer bounds or
known scene references, then compare the imported model against that measurement.
The previous torso/backpack dimensions were local asset measurements without a documented transform
chain; they must not be treated as universal metre dimensions for Schedule I avatars.

For wearables, compare shoulder width, upper-back depth, and the movement envelope in idle, walk,
crouch, turn, and interaction poses. A static fit on one mannequin is insufficient. For furniture,
check floor contact, door clearance, placement-grid dimensions, and player reach.

## Export acceptance

Record the source file, export selection, exporter/version, unit convention, pivot, transform policy,
axis conversion owner, mesh counts, material slots, texture paths, and representative screenshot.
A small successful round trip is the gate for replacing a production asset.

## Primary sources

- [Blender: Apply transforms](https://docs.blender.org/manual/en/latest/scene_layout/object/editing/apply.html).
- [Blender: glTF 2.0](https://docs.blender.org/manual/en/latest/addons/scene_gltf2.html).
- [Blender export operators](https://docs.blender.org/api/current/bpy.ops.export_scene.html).

These are current documentation references, not proof of the user's installed Blender or game loader.
