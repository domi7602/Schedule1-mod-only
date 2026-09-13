# S1MAPI — MaterialPresets (URP-Compatible Materials)

Creating URP materials at runtime is error-prone — shader names, property keys, and render pipelines vary. `MaterialPresets` wraps the URP shader suite into 5 ready-to-use presets.

---

## 1. The 5 Presets

```csharp
public enum MaterialPreset
{
    Opaque,        // solid surface, no transparency
    Transparent,   // partial transparency
    Glass,         // refractive-style, glass-like
    Metallic,      // metal (high specular, low roughness)
    Emissive       // glow (emission color + intensity)
}
```

---

## 2. Using via `ProceduralMeshBuilder`

```csharp
var box = new ProceduralMeshBuilder("ReflectiveFloor")
    .AddBox(Vector3.zero, new Vector3(5, 0.1f, 5))
    .SetMaterialPreset(MaterialPreset.Metallic)
    .SetColor(Color.gray)
    .Build();
```

---

## 3. Using Directly

```csharp
using S1MAPI.Materials;

var mat = MaterialPresets.Create(MaterialPreset.Glass);
mat.SetColor("_BaseColor", new Color(0.7f, 0.9f, 1f, 0.3f));
mat.SetFloat("_Surface", 1f);   // URP "Transparent" surface type

var renderer = box.GetComponent<Renderer>();
renderer.material = mat;
```

### Property Setters (Common)

| Method | Property | Notes |
|---|---|---|
| `SetColor(string, Color)` | `_BaseColor` | URP main color |
| `SetFloat(string, float)` | various | numeric properties |
| `SetTexture(string, Texture)` | various | textures |
| `SetVector(string, Vector4)` | various | vector properties |

---

## 4. Per-Preset Recipes

### Opaque

```csharp
new ProceduralMeshBuilder("PaintedWall")
    .AddBox(Vector3.zero, size)
    .SetColor(Color.cream)
    .Build();
// default: opaque, no emission, no transparency
```

### Transparent

```csharp
new ProceduralMeshBuilder("GlassPane")
    .AddBox(Vector3.zero, new Vector3(2, 2, 0.05f))
    .SetMaterialPreset(MaterialPreset.Transparent)
    .SetColor(new Color(0.7f, 0.9f, 1f, 0.3f))
    .Build();
```

### Glass

```csharp
new ProceduralMeshBuilder("GlassWindow")
    .AddBox(Vector3.zero, size)
    .SetMaterialPreset(MaterialPreset.Glass)
    .SetColor(new Color(0.6f, 0.85f, 1f, 0.5f))
    .Build();
```

`Glass` is heavier than `Transparent` — uses shader-based transparency, refracts/diffuses light. Use for windows, display cases, science equipment.

### Metallic

```csharp
new ProceduralMeshBuilder("SteelLocker")
    .AddBox(Vector3.zero, new Vector3(1f, 2f, 0.6f))
    .SetMaterialPreset(MaterialPreset.Metallic)
    .SetColor(new Color(0.7f, 0.7f, 0.7f))
    .Build();
```

### Emissive

```csharp
new ProceduralMeshBuilder("NeonSign")
    .AddBox(Vector3.zero, new Vector3(2, 0.5f, 0.05f))
    .SetMaterialPreset(MaterialPreset.Emissive)
    .SetEmission(Color.cyan, intensity: 3.0f)
    .Build();
```

This is the equivalent of a glowing neon sign. Use for lights, screens, magic items, status indicators.

---

## 5. Limitations

* **No shader customization beyond presets.** If you need a specific shader (e.g., outline shader, custom fragment), use `MaterialPresets.Create(preset)` and modify the result, or write a custom shader.
* **No MaterialPropertyBlock reuse across presets.** Each preset creates a new Material instance.
* **No GPU instancing by default.** For 100+ identical objects, consider `MaterialPropertyBlock` for variation.

---

## 6. Direct Material API Beyond Presets

```csharp
using S1MAPI.Materials;

// Create a material from a base preset
var mat = MaterialPresets.Create(MaterialPreset.Opaque);

// Apply additional effects
mat.SetTexture("_BaseMap", myTexture);
mat.SetColor("_EmissionColor", Color.cyan);

// Use directly on a renderer
renderer.material = mat;

// Or use a shared material (multiple objects, one material instance)
renderer.sharedMaterial = mat;
```

> **Gotcha:** `renderer.material` returns a clone, `renderer.sharedMaterial` is the actual instance. Use `sharedMaterial` for performance when many objects share the same look.
