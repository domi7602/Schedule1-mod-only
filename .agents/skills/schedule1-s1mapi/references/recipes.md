# S1MAPI — End-to-End Recipes

Step-by-step real-world recipes combining multiple S1MAPI modules.

---

## Recipe 1: Simple Lounge (Procedural Only)

**Goal:** A 10×10m lounge with a couch, table, lamp, and emissive neon sign. No external models.

```csharp
using MelonLoader;
using S1API.Lifecycle;
using S1MAPI.Building;
using S1MAPI.Interior;
using S1MAPI.ProceduralMesh;
using S1MAPI.World;
using UnityEngine;

public class LoungeMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        GameLifecycle.OnSaveLoaded += BuildLounge;
    }

    private void BuildLounge()
    {
        Vector3 origin = new Vector3(50, 0, 50);
        TerrainClearer.Clear(origin, 15f);
        FlattenTerrain.Flatten(origin, new Vector2(12, 12), 0f);

        // Build the lounge
        var building = BuildingBuilder.Create("Lounge")
            .SetFloor(origin, new Vector2(10, 10))
            .SetCeiling(origin + Vector3.up * 3, new Vector2(10, 10))
            .AddWall(WallSegment.North(origin + Vector3.up * 1.5f + Vector3.forward * 5, 10)
                .WithDoor(new Vector3(origin.x, 1f, origin.z + 5f),
                          new Vector2(1.2f, 2.2f), DoorStyle.Wood))
            .AddWall(WallSegment.South(origin + Vector3.up * 1.5f - Vector3.forward * 5, 10)
                .WithWindow(new Vector3(origin.x, 1.5f, origin.z - 5f),
                            new Vector2(2f, 1.2f), WindowStyle.Glass))
            .AddWall(WallSegment.East(origin + Vector3.up * 1.5f + Vector3.right * 5, 10))
            .AddWall(WallSegment.West(origin + Vector3.up * 1.5f - Vector3.right * 5, 10))
            .Build();

        // Furniture
        InteriorBuilder.Add(building, FurnitureType.Couch, new Vector3(0, 0.4f, 2));
        InteriorBuilder.Add(building, FurnitureType.Table, new Vector3(0, 0.4f, 3));

        // Neon sign (procedural)
        new ProceduralMeshBuilder("NeonSign")
            .AddBox(origin + new Vector3(0, 2.5f, 4.95f), new Vector3(3, 0.3f, 0.05f))
            .SetMaterialPreset(MaterialPreset.Emissive)
            .SetEmission(Color.cyan, intensity: 2.5f)
            .Build();

        // NPC pathfinding
        NavigationBuilder.BuildFor(building);
    }
}
```

---

## Recipe 2: Custom Building with GLTF Model

**Goal:** A 15×15m showroom displaying a custom weapon rack (exported from Blender as weapons.glb).

```csharp
using MelonLoader;
using S1API.Lifecycle;
using S1MAPI.Building;
using S1MAPI.Gltf;
using S1MAPI.Interior;
using S1MAPI.Materials;
using S1MAPI.World;
using System.Reflection;
using UnityEngine;

public class ShowroomMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        GameLifecycle.OnSaveLoaded += BuildShowroom;
    }

    private void BuildShowroom()
    {
        Vector3 origin = new Vector3(100, 0, 0);
        TerrainClearer.Clear(origin, 20f);
        FlattenTerrain.Flatten(origin, new Vector2(16, 16), 0f);

        // Build the room
        var building = BuildingBuilder.Create("Showroom")
            .SetFloor(origin, new Vector2(15, 15))
            .SetCeiling(origin + Vector3.up * 4, new Vector2(15, 15))
            .AddWall(WallSegment.North(origin + Vector3.up * 2f + Vector3.forward * 7.5f, 15)
                .WithDoor(new Vector3(origin.x, 1f, origin.z + 7.5f),
                          new Vector2(1.5f, 2.5f), DoorStyle.Wood))
            .AddWall(WallSegment.South(origin + Vector3.up * 2f - Vector3.forward * 7.5f, 15)
                .WithWindow(new Vector3(origin.x, 1.5f, origin.z - 7.5f),
                            new Vector2(3f, 1.5f), WindowStyle.Glass))
            .AddWall(WallSegment.East(origin + Vector3.up * 2f + Vector3.right * 7.5f, 15))
            .AddWall(WallSegment.West(origin + Vector3.up * 2f - Vector3.right * 7.5f, 15))
            .Build();

        // Load weapons.glb from embedded resource
        var assembly = Assembly.GetExecutingAssembly();
        byte[] glbData;
        using (var stream = assembly.GetManifestResourceStream("ShowroomMod.Resources.weapons.glb"))
        using (var ms = new System.IO.MemoryStream())
        {
            stream.CopyTo(ms);
            glbData = ms.ToArray();
        }

        var weaponRack = GltfLoader.LoadGlb(glbData);
        weaponRack.transform.SetParent(building.transform);
        weaponRack.transform.localPosition = new Vector3(0, 0, 0);

        // Glass display case
        new ProceduralMeshBuilder("DisplayCase")
            .AddBox(origin + new Vector3(0, 1f, 0), new Vector3(2, 2, 0.05f))
            .SetMaterialPreset(MaterialPreset.Glass)
            .SetColor(new Color(0.7f, 0.9f, 1f, 0.5f))
            .Build();

        // Furniture
        InteriorBuilder.Add(building, FurnitureType.Desk, new Vector3(0, 0.5f, 4));
        InteriorBuilder.Add(building, FurnitureType.Chair, new Vector3(0.5f, 0.5f, 4.5f));

        // NPCs walk in
        NavigationBuilder.BuildFor(building);
    }
}
```

---

## Recipe 3: Mod-Authored Quest Inside a Custom Building

**Goal:** A 3-quest storyline (`S1API.Quests.Quest`) inside a procedurally-built library.

```csharp
public class LibraryQuestMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        GameLifecycle.OnSaveLoaded += BuildAndSpawnQuest;
    }

    private void BuildAndSpawnQuest()
    {
        // 1. Build the library with S1MAPI
        Vector3 origin = new Vector3(70, 0, 30);
        TerrainClearer.Clear(origin, 18f);
        FlattenTerrain.Flatten(origin, new Vector2(14, 14), 0f);

        var building = BuildingBuilder.Create("Library")
            .SetFloor(origin, new Vector2(12, 12))
            .SetCeiling(origin + Vector3.up * 3.5f, new Vector2(12, 12))
            .AddWall(WallSegment.North(origin + Vector3.up * 1.75f + Vector3.forward * 6, 12)
                .WithDoor(new Vector3(origin.x, 1f, origin.z + 6f),
                          new Vector2(1.2f, 2.2f), DoorStyle.Wood))
            .AddWall(WallSegment.South(origin + Vector3.up * 1.75f - Vector3.forward * 6, 12))
            .AddWall(WallSegment.East(origin + Vector3.up * 1.75f + Vector3.right * 6, 12))
            .AddWall(WallSegment.West(origin + Vector3.up * 1.75f - Vector3.right * 6, 12))
            .Build();

        // 2. Fill with shelves
        for (int i = 0; i < 4; i++)
        {
            InteriorBuilder.Add(building, FurnitureType.Shelf,
                                new Vector3(-4 + i * 2.5f, 0.5f, 4));
        }

        // 3. NPC pathfinding
        NavigationBuilder.BuildFor(building);

        // 4. Spawn a librarian NPC via S1API
        var librarian = QuestManager.CreateQuest<FindBookQuest>();
        // (NPC spawning pattern — see schedule1-s1api/references/entities.md)
    }
}
```

This is the **S1MAPI + S1API sweet spot**: S1MAPI builds the world, S1API fills it with quests and NPCs.

---

## Recipe 4: Replace AssetBundle Workflow with GLTF

**Migration:** Drop your `.bundle` files. Use `.glb` + `GltfLoader` instead.

| Old (AssetBundle) | New (S1MAPI) |
|---|---|
| `var bundle = AssetBundle.LoadFromFile("MyBundle.bundle");` | `var model = GltfLoader.LoadGlb(glbData);` |
| `var prefab = bundle.LoadAsset<GameObject>("MyPrefab");` | (no AssetBundle; use ProceduralMeshBuilder or .glb) |
| `Instantiate(prefab)` | `Instantiate(model)` |
| Build staged in Unity Editor | Build = project compiles; .glb is embedded |

**Win:** No version lock, no Unity Editor roundtrip, simpler build pipeline.

---

## Recipe 5: Multi-Floor Building

```csharp
var building = BuildingBuilder.Create("OfficeTower")
    .SetFloor(new Vector3(0, 0, 0), new Vector2(10, 10))    // ground floor
    .AddFloor(new Vector3(0, 3f, 0), new Vector2(10, 10))   // 2nd floor (ceiling of 1st = floor of 2nd)
    .AddFloor(new Vector3(0, 6f, 0), new Vector2(10, 10))   // 3rd floor
    .AddStairsBetween(prev: 0f, next: 3f, position: new Vector3(4, 0, 4))
    .AddStairsBetween(prev: 3f, next: 6f, position: new Vector3(4, 3, 4))
    .AddWall(WallSegment.North(new Vector3(0, 3f, 5), 10))  // 2nd floor walls
    .Build();
```

`AddFloor` adds a floor slab; `AddStairsBetween` creates a walkable staircase.
