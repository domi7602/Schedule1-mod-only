# Procedural Rooms, Binary Meshes, and Collision Shells

> UNVERIFIED against the installed runtime. This file contains no backticked identifier claims, so the f12 static identifier sweep 2026-10-08 had nothing to check: its prose, tables and numbers are **not covered** by the static check and must be re-derived from the decompiles before being relied on.

## Evidence and use

Use for a room built from primitives, generated meshes, or custom embedded binary meshes. The original
`runtime_meshes.bin` example was an illustrative format with no verified reader bounds or schema.
This reference defines the checks needed before implementing the reader against an actual exporter.
It does not claim an existing game-native binary format.

## Choose the representation

Simple room shells can use primitive/render meshes and separate colliders. Imported GLB/OBJ or an
appropriate Unity asset build may be better for complex geometry. A custom binary format is reasonable
only when the producer, schema, validation, and maintenance cost are deliberate.

A mesh interchange format does not supply every animation controller, Unity component, shader, or
native gameplay contract. Read the 3D assets and S1MAPI references before replacing an asset pipeline.

## Binary reader contract

Define a magic identifier, format version, byte order, part count, per-part fields, optional attributes,
and expected end-of-file behavior. Record exporter/reader compatibility. Reject unsupported versions
instead of interpreting them using the current layout.

Validate before allocating or publishing Unity resources:

- Bound file bytes, part count, string length, vertex count, index count, and total allocation budget.
- Use checked arithmetic for count × element-size and cumulative offsets; confirm remaining stream
  bytes cover every array before allocation/read.
- Require finite vertex, normal, and UV values. Validate bounds against the intended room budget.
- Require matching per-vertex attribute counts when an attribute is present; define a supported policy
  for absent normals/UVs instead of accepting mismatched arrays.
- Require triangle index count divisible by three, indices within the vertex range, and the selected
  mesh index format to support the largest index. Verify 32-bit index support where needed.
- Validate material/submesh mapping and reject invalid or overlapping ranges where the format forbids
  them. Bound pathological geometry even if each individual index is technically valid.
- Treat truncation, unexpected trailing data, invalid names, and duplicate part IDs according to an
  explicit schema policy. Preserve the failing asset identifier and first useful diagnostic.

Decode into bounded plain data first. Build owned Unity meshes only after validation, publish the
completed root atomically, and release all partial meshes/materials/objects on any failure. Use the
installed managed/IL2CPP array APIs; no unchecked old reader should be copied as a working recipe.

## Local-space room construction

Choose one root transform and define room dimensions/anchors in its local space. Distinguish a floor
origin from a geometric center in the data model. Parent generated children with the intended local
transform semantics, then position the whole room once. Do not pass local coordinates into world
`position` assignments merely because the root happened to be at the origin in a test.

Build floor, ceiling, and individual wall collision with deliberate thickness and overlap at seams.
Split walls around doors/windows; a single box enclosing the whole room blocks its usable interior.
Check floor height, headroom, step clearance, wall thickness, and the actual player controller shape.
Keep the render shell and collision shell aligned but independently tunable.

Create the room inactive, finish geometry and collision, then validate destination points before
activation. If physics queries immediately follow transform changes, inspect whether the target
runtime requires an explicit transform synchronization point. Build NPC navigation only after the
final geometry pose and keep all mod-created navigation handles for teardown.

## Tiled materials and lighting

Resolve a compatible shader or borrowed working material before creating a new material. Check the
actual texture/property names and normal-map keyword/format requirements. Derive repeat amounts from
surface dimensions and UVs so floor/wall density stays coherent; normal/albedo tiling must agree.

Choose material ownership explicitly. A shared native material is borrowed read-only; a mod-owned
instance or tested property override isolates changed tiling/tint. A missing shader/image must leave
a recoverable partial failure, not a pink active room with no exit.

Lighting, exposure, shadows, emission, and audio are distinct systems. A visually bright emissive
material does not establish illumination of the room. Bound shadow-casting lights and validate the
intended performance with multiple rooms active.

## Verification

Test rotated/translated roots, seams, open doorways, floor contact, blocked spawn, corrupt/truncated
mesh input, allocation limits, unsupported version, missing material, repeated rebuild, and teardown.
Measure mesh/material counts and build time. Verify navigation and player transitions in the game;
static bounds checks cannot establish playable access.

Read [Rendering and materials](../../schedule1-3d-assets/references/urp-rendering-materials.md),
[Asset import](../../schedule1-s1mapi/references/gltf-loading.md), and [Door transitions](door-hooking.md).
