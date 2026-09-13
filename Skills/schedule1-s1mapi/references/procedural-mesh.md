# S1MAPI — ProceduralMesh (Primitive Shapes)

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
| `AddSphere(Vector3 center, float radius, int segments = 16)` | Sphere |
| `AddCylinder(Vector3 center, float radius, float height, int segments = 16)` | Cylinder |
| `AddCapsule(Vector3 center, float radius, float height, int direction = 1)` | Capsule |
| `AddPlane(Vector3 center, Vector2 size, Vector3 normal)` | Plane (default normal up) |
| `AddQuad(Vector3 center, Vector2 size)` | Single-sided quad |

---

## 3. The Full Fluent API

```csharp
var box = new ProceduralMeshBuilder("MyBox")
    .AddBox(Vector3.zero, Vector3.one)                        // geometry
    .SetColor(Color.red)                                       // color
    .SetMaterialPreset(MaterialPreset.Metallic)                // material type
    .SetEmission(Color.cyan, intensity: 1.5f)                   // emissive
    .SetCollider(enabled: true, isTrigger: false)               // collision
    .SetLayer(LayerMask.NameToLayer("Interactable"))           // layer
    .SetStatic(true)                                           // static
    .Build();
```

### Common Methods

| Method | Purpose |
|---|---|
| `SetColor(Color)` | Single tint |
| `SetMaterialPreset(MaterialPreset)` | URP type (opaque/glass/metallic/emissive) |
| `SetEmission(Color, intensity)` | Emissive glow |
| `SetTransparency(float alpha)` | 0–1 alpha |
| `SetCollider(bool enabled, bool isTrigger = false)` | Box collider fit |
| `SetLayer(int layer)` | Unity layer |
| `SetStatic(bool isStatic)` | Static flag for baking |
| `Build()` | Finalize and return `GameObject` |

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
    .SetColor(Color.brown)
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
