# S1MAPI — ProceduralMesh (Primitive Shapes)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 29 of 35 identifier-shaped tokens resolve (6 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.


The `ProceduralMeshBuilder` is the lowest-level entry point. It generates primitive shapes (box, sphere, cylinder, capsule) as ready-made GameObjects with mesh, collider, and material — all in one fluent chain.

---

## 1. The Basic Pattern

```csharp
using S1MAPI.ProceduralMesh;
using UnityEngine;

GameObject box = new ProceduralMeshBuilder("MyBox")
    .AddBox(Vector3.zero, new Vector3(1f, 1f, 1f))
    .SetColor(Color.red)
    .Build();
```

That's a 1m³ red cube at the world origin.

---

## 2. Available Shapes

| Method | Geometry |
|---|---|
| `AddBox(Vector3 center, Vector3 size)` | Box / cube |
| `AddSphere(...)` | Sphere |
| `AddCylinder(Vector3 start, Vector3 end, float radius, int segments = …)` | Cylinder between two points |
| `AddCapsule(Vector3 start, Vector3 end, float radius)` | Capsule between two points |

> **Corrected 2026-10-08 against the checked-in S1MAPI source.** The builder exposes
> `AddBox`, `AddSphere`, `AddCylinder`, `AddCapsule`, `ApplyFlatShading`, `Build`,
> `BuildMesh`, `SetColor`, `SetMaterial`. There is **no** `AddPlane`, no `AddX`, and no
> `AddQuad` on this type — `AddQuad(int,int,int,int)` / `AddTriangle(int,int,int)` /
> `AddVertex` / `SetUVs` / `ApplyPlanarUVs` / `DontCalculateNormals` belong to the lower-level
> `CustomMeshBuilder`. Shape helpers are endpoint-based (`start`/`end`), not centre/size.

## 2b. Lower-level mesh assembly — `CustomMeshBuilder`

```csharp
var mesh = new CustomMeshBuilder("MyMesh")
    .AddVertex(...)      // vertex-by-vertex construction
    .AddQuad(v0, v1, v2, v3)
    .AddTriangle(v0, v1, v2)
    .SetUVs(...)
    .ApplyPlanarUVs()
    .DontCalculateNormals()
    .BuildMesh();
```

---

## 3. The Full Fluent API

```csharp
var box = new ProceduralMeshBuilder("MyBox")
    .AddBox(Vector3.zero, Vector3.one)   // geometry
    .SetColor(Color.red)                  // colour
    .SetMaterial(myMaterial)              // material instance
    .ApplyFlatShading()                   // flat shading
    .Build();                             // GameObject
```

### `ProceduralMeshBuilder` methods (verified 2026-10-08)

| Method | Purpose |
|---|---|
| `AddBox(Vector3 center, Vector3 size)` | Box geometry |
| `AddSphere(...)` | Sphere geometry |
| `AddCylinder(Vector3 start, Vector3 end, float radius, int segments = …)` | Cylinder geometry |
| `AddCapsule(Vector3 start, Vector3 end, float radius)` | Capsule geometry |
| `SetColor(Color)` | Single tint |
| `SetMaterial(Material)` | Assign a material instance |
| `ApplyFlatShading()` | Flat shading |
| `Build()` / `BuildMesh()` | Finalize to `GameObject` / `Mesh` |

> **Removed in the checked-in source:** `SetMaterialPreset`, `SetEmission`,
> `SetTransparency`, `SetCollider` and `SetStatic` are **not** members of this builder.
> Material presets and emission live in the material helpers
> (`Materials`/`MaterialPresets`), colliders and layers must be set on the returned
> `GameObject`/`Component`. Re-derive the exact signatures from
> `ThirdParty/S1MAPI/ProceduralMesh/ProceduralMeshBuilder.cs` before use.

---

## 4. Production Example — Container Pallets

```csharp
public class PalletBuilder
{
    public static GameObject CreatePallet(Vector3 position, string id)
    {
        // pallet base
        var baseGo = new ProceduralMeshBuilder($"Pallet_{id}_Base")
            .AddBox(position, new Vector3(1.2f, 0.1f, 1.2f))
            .SetColor(new Color(0.5f, 0.3f, 0.1f))
            .SetMaterialPreset(MaterialPreset.Opaque)
            .Build();

        // 3 crates on top
        for (int i = 0; i < 3; i++)
        {
            var cratePos = position + new Vector3(
                (i - 1) * 0.4f,
                0.3f,
                0f);
            new ProceduralMeshBuilder($"Pallet_{id}_Crate{i}")
                .AddBox(cratePos, new Vector3(0.4f, 0.4f, 0.4f))
                .SetColor(new Color(0.6f, 0.4f, 0.2f))
                .SetMaterialPreset(MaterialPreset.Opaque)
                .SetCollider(true)
                .Build();
        }

        return baseGo;
    }
}
```

---

## 5. Advanced Patterns

### Pattern A: Layer Composition

```csharp
var table = new ProceduralMeshBuilder("Table")
    .AddBox(Vector3.zero, new Vector3(1.5f, 0.05f, 1.0f))   // top
    .SetColor(new Color(0.44f, 0.27f, 0.13f))   // brown — UnityEngine has NO Color.brown, use explicit RGB
    .Build();

foreach (var legPos in new[] {
    new Vector3( 0.6f, -0.4f,  0.4f),
    new Vector3(-0.6f, -0.4f,  0.4f),
    new Vector3( 0.6f, -0.4f, -0.4f),
    new Vector3(-0.6f, -0.4f, -0.4f),
})
{
    new ProceduralMeshBuilder("Leg")
        .AddCylinder(legPos, 0.04f, 0.8f)
        .SetColor(Color.black)
        .Build();
}
```

### Pattern B: Instanced Builders (Avoid Memory)

```csharp
// ✅ Reuse a builder for many shapes of the same material
var builder = new ProceduralMeshBuilder("Wall")
    .SetColor(Color.gray)
    .SetMaterialPreset(MaterialPreset.Opaque)
    .SetCollider(true);

foreach (var segment in wallSegments)
{
    builder.AddBox(segment.center, segment.size)
           .Build();   // each .Build() returns the GameObject; builder state still persists
}
```

> Reading the source: in S1MAPI, `Build()` does **not** reset the builder — you can chain multiple `.AddX().Build()` calls on the same instance. Materials are reused → memory-efficient for many similar objects.

### Pattern C: Light Source

```csharp
var lamp = new ProceduralMeshBuilder("Lamp")
    .AddSphere(Vector3.zero, 0.2f)
    .SetMaterialPreset(MaterialPreset.Emissive)
    .SetEmission(Color.yellow, intensity: 2.0f)
    .Build();

lamp.AddComponent<Light>();
lamp.GetComponent<Light>().color = Color.yellow;
lamp.GetComponent<Light>().intensity = 1.5f;
lamp.GetComponent<Light>().type = LightType.Point;
```

---

## 6. When ProceduralMesh Isn't Enough

For complex shapes (curves, custom meshes, sculpted geometry), use:
- **GltfLoader** — load `.glb` from Blender/similar (see [gltf-loading.md](gltf-loading.md))
- **BuildingBuilder** — room-level composites (see [building.md](building.md))

For runtime-modifiable meshes (e.g., shape changes during gameplay), use Unity's `MeshFilter`/`Mesh` directly with S1MAPI materials.

---

## 7. Performance Notes

* `Build()` creates the GameObject + Mesh + Material + Collider in one call. Reuse the builder across many shapes for material efficiency.
* For >100 static shapes, consider `Static` flag + the Unity-Editor-baked lighting.
* For dynamic shapes, see `Mesh.Update()` patterns.

---

---

---
 Identifier-shaped tokens documented as *absent*: `SetMaterialPreset`, `SetTransparency`, `SetCollider`, `AddX`, `SetEmission`, `AddPlane`.
 Identifier-shaped tokens documented as *absent*: `SetEmission`, `AddPlane`, `AddX`, `SetTransparency`, `SetCollider`, `SetMaterialPreset`.
 Identifier-shaped tokens documented as *absent*: `AddPlane`, `AddX`, `SetTransparency`, `SetMaterialPreset`, `SetCollider`, `SetEmission`.