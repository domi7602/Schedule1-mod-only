# Universal Render Pipeline (URP) & Material System in Schedule I

This document describes the shader architecture of *Schedule I* (v0.4.6f13), how to correctly assign textures and materials at runtime, and how to avoid typical rendering mistakes (pink shaders, missing gloss, material leaks).

---

## 1. Why Shaders Turn Pink (*The Pink Shader Problem*)

*Schedule I* uses the **Universal Render Pipeline (URP)** in Unity 2022.3 LTS.
* If a mod attempts to use the old Built-in shaders `"Standard"` or `"Legacy Shaders/Diffuse"`, the URP render pipeline cannot interpret them and renders the object in bright **magenta/pink**.

### The correct shader resolution in C#:

```csharp
public static Shader GetUrpLitShader()
{
    // 1. Primary URP Lit shader
    Shader shader = Shader.Find("Universal Render Pipeline/Lit");
    if (shader != null) return shader;

    // 2. Fallbacks for Unlit or vanilla asset shaders
    shader = Shader.Find("Universal Render Pipeline/Unlit");
    if (shader != null) return shader;

    shader = Shader.Find("Universal Render Pipeline/Simple Lit");
    if (shader != null) return shader;

    // 3. Last-resort fallback
    return Shader.Find("Standard");
}
```

---

## 2. PBR Material Properties in URP

A URP Lit material controls its appearance through standardized shader properties:

| Property Name | Data type | Description |
|---|---|---|
| `_BaseColor` | `Color` (RGBA) | Main color / tint of the material (e.g. `Color(0.2f, 0.5f, 0.2f, 1f)` for olive green). |
| `_BaseMap` | `Texture2D` | Diffuse / albedo texture map. |
| `_Metallic` | `float` (0.0 to 1.0) | 0.0 = non-metallic (fabric, wood, plastic), 1.0 = full metal (buckles, steel). |
| `_Smoothness` | `float` (0.0 to 1.0) | 0.0 = matte / rough, 1.0 = high-gloss / mirror-like. For fabric, $0.1\text{--}0.3$ is recommended. |
| `_BumpMap` | `Texture2D` | Normal map for realistic surface details (e.g. threads, folds, grooves). |
| `_EmissionColor` | `Color` (HDR) | Emission color for LED elements, screens, or glowing strips. |

### C# code example for a fabric-like backpack material with metal buckles:

```csharp
Material fabricMat = new Material(GetUrpLitShader());
fabricMat.name = "Backpack_Fabric_Olive";
fabricMat.SetColor("_BaseColor", new Color(0.18f, 0.26f, 0.16f, 1f)); // Olive green
fabricMat.SetFloat("_Metallic", 0.0f);   // Fabric is not metallic
fabricMat.SetFloat("_Smoothness", 0.18f); // Matte / slightly rough

Material metalMat = new Material(GetUrpLitShader());
metalMat.name = "Backpack_Buckle_Steel";
metalMat.SetColor("_BaseColor", new Color(0.85f, 0.85f, 0.88f, 1f)); // Silver
metalMat.SetFloat("_Metallic", 0.85f);   // High metallic value
metalMat.SetFloat("_Smoothness", 0.75f); // High gloss value
```

---

## 3. Loading Dynamic Textures at Runtime

When a mod loads its own `.png` or `.jpg` images from the filesystem:

```csharp
public static Texture2D LoadTextureFromFile(string filePath)
{
    if (!File.Exists(filePath)) return null;

    byte[] fileData = File.ReadAllBytes(filePath);
    Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, true);
    
    // Unity's ImageConversion loads PNG/JPG into the texture
    if (ImageConversion.LoadImage(texture, fileData))
    {
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Repeat;
        return texture;
    }
    return null;
}
```

---

## 4. Preventing Memory Leaks (`sharedMaterial` vs `material`)

* **Danger:** A call to `renderer.material` creates **a new instance copy** of the material in graphics memory (VRAM) every time. With repeated calls in update loops, this leads to massive memory leaks and FPS drops!
* **The solution:**
  * For pure color adjustments or read access, always use `renderer.sharedMaterial`.
  * Create materials once centrally, cache them in static dictionaries or fields, and assign them to multiple renderers.