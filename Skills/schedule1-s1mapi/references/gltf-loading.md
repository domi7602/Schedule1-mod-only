# S1MAPI — GltfLoader (External 3D Models)

`GltfLoader` loads `.glb` / `.gltf` models exported from Blender (or similar) at runtime. Replaces AssetBundles — no Unity Editor roundtrip, no version lock.

---

## 1. Quickstart

```csharp
using S1MAPI.Gltf;

GameObject model = GltfLoader.LoadGlb(glbData);
```

That's it. The `glbData` is a `byte[]` (the .glb file contents).

---

## 2. Loading From Different Sources

### From Embedded Resource

```csharp
private byte[] LoadEmbeddedGlb(string resourceName)
{
    var assembly = Assembly.GetExecutingAssembly();
    using var stream = assembly.GetManifestResourceStream(resourceName);
    using var ms = new MemoryStream();
    stream.CopyTo(ms);
    return ms.ToArray();
}

// Usage
byte[] data = LoadEmbeddedGlb("MyMod.Models.mything.glb");
GameObject model = GltfLoader.LoadGlb(data);
```

### From Disk

```csharp
byte[] data = File.ReadAllBytes(Path.Combine(modFolder, "mything.glb"));
GameObject model = GltfLoader.LoadGlb(data);
```

### From URL (rare, avoid in production)

```csharp
byte[] data = new WebClient().DownloadData(url);
GameObject model = GltfLoader.LoadGlb(data);
```

---

## 3. Loading `.gltf` + Binaries

```csharp
// For .gltf with separate .bin files (rare):
GameObject model = GltfLoader.LoadGltf(textData, additionalBinaries);
```

For most use cases, prefer `.glb` (single-file) format over `.gltf` (multi-file).

---

## 4. Post-Load Adjustments

### Reposition / Rescale

```csharp
model.transform.position = new Vector3(10, 0, 5);
model.transform.rotation = Quaternion.Euler(0, 90, 0);
model.transform.localScale = Vector3.one * 0.5f;   // halve the size
```

### Material Override

```csharp
var renderer = model.GetComponentInChildren<Renderer>();
renderer.material = MaterialPresets.Create(MaterialPreset.Metallic);
```

### Shader Adjustment

```csharp
// S1MAPI auto-applies shader/emission fixes during LoadGlb.
// Override only if your model has unique shader requirements.
```

S1MAPI handles URP compatibility automatically. Custom shaders (e.g., outline effects) require post-processing.

---

## 5. Production Example — Lounge Building

```csharp
public class LoungeImport
{
    public static GameObject CreateLounge(string modelBundlePath, Vector3 origin)
    {
        // Load .glb from embedded resource
        var data = Resources.Load<TextAsset>(modelBundlePath).bytes;
        var lounge = GltfLoader.LoadGlb(data);

        // Reposition
        lounge.transform.position = origin;
        lounge.transform.rotation = Quaternion.Euler(0, 180, 0);

        // Replace glass walls with URP glass material
        foreach (var renderer in lounge.GetComponentsInChildren<Renderer>())
        {
            if (renderer.name.Contains("Glass"))
            {
                renderer.material = MaterialPresets.Create(MaterialPreset.Glass);
            }
        }

        // Add collider if the model lacks one
        if (lounge.GetComponentInChildren<Collider>() == null)
        {
            lounge.AddComponent<BoxCollider>();
        }

        return lounge;
    }
}
```

---

## 6. Bundle Workflow

Embed `.glb` files in your mod DLL:

1. Add the `.glb` to your mod's `Resources/` folder
2. **Set the Build Action to "Embedded Resource"** in the `.csproj`:
   ```xml
   <ItemGroup>
     <EmbeddedResource Include="Resources\*.glb" />
   </ItemGroup>
   ```
3. Load with `Assembly.GetManifestResourceStream("MyMod.Resources.mything.glb")`

> **Alternative:** Use `Resources.Load<TextAsset>("mything")` if you keep the .glb as a `TextAsset` (Unity-managed asset, requires `Resources\` folder).

---

## 7. Common Pitfalls

| Pitfall | Fix |
|---|---|
| Player textures look glitchy | S1MAPI auto-fixes; ensure your model wasn't built with legacy shaders |
| Model is invisible | Check `Scale` — Blender meters ≠ Unity meters |
| Model clips into ground | Adjust `transform.position.y` |
| Model has no collider | Add `BoxCollider` manually |
| Model too big/small | Set `transform.localScale = Vector3.one * 0.5f` (or `2f`) |
| Texture colors swap | GLB spec UV index mismatch — re-export from Blender with default config |

---

## 8. Authoring for S1MAPI's `.glb` Loader

When exporting `.glb` from Blender:

| Setting | Recommended |
|---|---|
| Format | glTF Binary (`.glb`) |
| Axis | Y-up (Unity default) |
| Units | Meters (Unity default) |
| Textures | PNG, embedded in .glb |
| Compression | Draco (optional, runtime decompression) |
| Geometry | Triangulated |
| Materials | PBR (will be converted to URP at runtime) |

---

## 9. Workspace Reference

GltfLoader is **not yet used** by any of the current workspace mods. The skill is forward-looking for content mods that ship custom 3D models.

For the official S1MAPI glTF-loading docs: https://github.com/ifBars/S1MAPI/tree/stable/docs/gltf-loading.md
