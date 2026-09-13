# Procedural 3D Room & Binary Mesh Construction — Reference Guide

This reference details how to generate interior rooms, buildings, and collision shells at runtime without requiring compiled Unity scene bundles.

---

## 1. Binary Mesh Format (`runtime_meshes.bin`)

To load complex models (arcade cabinets, furniture, structural walls) without AssetBundles:

```csharp
public static Mesh ReadBinaryMesh(BinaryReader reader)
{
    int vertexCount = reader.ReadInt32();
    Vector3[] vertices = new Vector3[vertexCount];
    for (int i = 0; i < vertexCount; i++)
        vertices[i] = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

    int normalCount = reader.ReadInt32();
    Vector3[] normals = new Vector3[normalCount];
    for (int i = 0; i < normalCount; i++)
        normals[i] = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

    int uvCount = reader.ReadInt32();
    Vector2[] uvs = new Vector2[uvCount];
    for (int i = 0; i < uvCount; i++)
        uvs[i] = new Vector2(reader.ReadSingle(), reader.ReadSingle());

    int triangleCount = reader.ReadInt32();
    int[] triangles = new int[triangleCount];
    for (int i = 0; i < triangleCount; i++)
        triangles[i] = reader.ReadInt32();

    var mesh = new Mesh
    {
        vertices = vertices,
        normals = normals,
        uv = uvs,
        triangles = triangles
    };
    mesh.RecalculateBounds();
    return mesh;
}
```

---

## 2. Generating the Collision Shell

A custom interior needs solid boundaries to prevent the player from falling into the void:

```csharp
public static void CreateCollisionShell(Transform parent, Vector3 roomSize, Vector3 roomCenter)
{
    // Floor collider
    CreateBoxCollider(parent, "FloorCollider", 
        new Vector3(roomCenter.x, roomCenter.y - 0.1f, roomCenter.z), 
        new Vector3(roomSize.x + 2f, 0.2f, roomSize.z + 2f));

    // Ceiling collider
    CreateBoxCollider(parent, "CeilingCollider", 
        new Vector3(roomCenter.x, roomCenter.y + roomSize.y + 0.1f, roomCenter.z), 
        new Vector3(roomSize.x + 2f, 0.2f, roomSize.z + 2f));

    // 4 Walls
    float halfX = roomSize.x * 0.5f;
    float halfZ = roomSize.z * 0.5f;
    float wallThick = 0.3f;

    CreateBoxCollider(parent, "WallNorth", new Vector3(roomCenter.x, roomCenter.y + roomSize.y * 0.5f, roomCenter.z + halfZ), new Vector3(roomSize.x, roomSize.y, wallThick));
    CreateBoxCollider(parent, "WallSouth", new Vector3(roomCenter.x, roomCenter.y + roomSize.y * 0.5f, roomCenter.z - halfZ), new Vector3(roomSize.x, roomSize.y, wallThick));
    CreateBoxCollider(parent, "WallEast", new Vector3(roomCenter.x + halfX, roomCenter.y + roomSize.y * 0.5f, roomCenter.z), new Vector3(wallThick, roomSize.y, roomSize.z));
    CreateBoxCollider(parent, "WallWest", new Vector3(roomCenter.x - halfX, roomCenter.y + roomSize.y * 0.5f, roomCenter.z), new Vector3(wallThick, roomSize.y, roomSize.z));
}

private static void CreateBoxCollider(Transform parent, string name, Vector3 pos, Vector3 size)
{
    var go = new GameObject(name);
    go.transform.SetParent(parent, false);
    go.transform.position = pos;
    var col = go.AddComponent<BoxCollider>();
    col.size = size;
}
```

---

## 3. Floor & Ceiling Materials with Tiling

```csharp
public static Material CreateTiledFloorMaterial(Texture2D diffuseTex, Texture2D normalMap, float repeatX, float repeatZ)
{
    var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    mat.mainTexture = diffuseTex;
    mat.mainTextureScale = new Vector2(repeatX, repeatZ);
    if (normalMap != null)
    {
        mat.EnableKeyword("_NORMALMAP");
        mat.SetTexture("_BumpMap", normalMap);
        mat.SetTextureScale("_BumpMap", new Vector2(repeatX, repeatZ));
    }
    mat.SetFloat("_Smoothness", 0.35f);
    mat.SetFloat("_Metallic", 0.05f);
    return mat;
}
```
