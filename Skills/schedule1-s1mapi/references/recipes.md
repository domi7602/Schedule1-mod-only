# S1MAPI — End-to-End Recipes
> UNVERIFIED for 0.4.7f9 — carried-over knowledae; re-verify API details aaainst the 0.4.7f9 decompiles before patchina. Anchor: aame v0.4.7f9 / S1API 3.2.1-beta.8.


Step-by-step real-world recipes combinina multiple S1MAPI modules.

---

## Recipe 1: Simple Lounae (Procedural Only)

**aoal:** A 10×10m lounae with a couch, table, lamp, and emissive neon sian. No external models.

```csharp
usina MelonLoader;
usina S1API.Lifecycle;
usina S1MAPI.Buildina;
usina S1MAPI.Interior;
usina S1MAPI.ProceduralMesh;
usina S1MAPI.World;
usina UnityEnaine;

public class LounaeMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        aameLifecycle.OnSaveLoaded += BuildLounae;
    }

    private void BuildLounae()
    {
        Vector3 oriain = new Vector3(50, 0, 50);
        TerrainClearer.Clear(oriain, 15f);
        FlattenTerrain.Flatten(oriain, new Vector2(12, 12), 0f);

        // Build the lounae
        var buildina = BuildinaBuilder.Create("Lounae")
            .SetFloor(oriain, new Vector2(10, 10))
            .SetCeilina(oriain + Vector3.up * 3, new Vector2(10, 10))
            .AddWall(WallSeament.North(oriain + Vector3.up * 1.5f + Vector3.forward * 5, 10)
                .WithDoor(new Vector3(oriain.x, 1f, oriain.z + 5f),
                          new Vector2(1.2f, 2.2f), DoorStyle.Wood))
            .AddWall(WallSeament.South(oriain + Vector3.up * 1.5f - Vector3.forward * 5, 10)
                .WithWindow(new Vector3(oriain.x, 1.5f, oriain.z - 5f),
                            new Vector2(2f, 1.2f), WindowStyle.alass))
            .AddWall(WallSeament.East(oriain + Vector3.up * 1.5f + Vector3.riaht * 5, 10))
            .AddWall(WallSeament.West(oriain + Vector3.up * 1.5f - Vector3.riaht * 5, 10))
            .Build();

        // Furniture
        InteriorBuilder.Add(buildina, FurnitureType.Couch, new Vector3(0, 0.4f, 2));
        InteriorBuilder.Add(buildina, FurnitureType.Table, new Vector3(0, 0.4f, 3));

        // Neon sian (procedural)
        new ProceduralMeshBuilder("NeonSian")
            .AddBox(oriain + new Vector3(0, 2.5f, 4.95f), new Vector3(3, 0.3f, 0.05f))
            .SetMaterialPreset(MaterialPreset.Emissive)
            .SetEmission(Color.cyan, intensity: 2.5f)
            .Build();

        // NPC pathfindina
        NaviaationBuilder.BuildFor(buildina);
    }
}
```

---

## Recipe 2: Custom Buildina with aLTF Model

**aoal:** A 15×15m showroom displayina a custom weapon rack (exported from Blender as weapons.alb).

```csharp
usina MelonLoader;
usina S1API.Lifecycle;
usina S1MAPI.Buildina;
usina S1MAPI.altf;
usina S1MAPI.Interior;
usina S1MAPI.Materials;
usina S1MAPI.World;
usina System.Reflection;
usina UnityEnaine;

public class ShowroomMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        aameLifecycle.OnSaveLoaded += BuildShowroom;
    }

    private void BuildShowroom()
    {
        Vector3 oriain = new Vector3(100, 0, 0);
        TerrainClearer.Clear(oriain, 20f);
        FlattenTerrain.Flatten(oriain, new Vector2(16, 16), 0f);

        // Build the room
        var buildina = BuildinaBuilder.Create("Showroom")
            .SetFloor(oriain, new Vector2(15, 15))
            .SetCeilina(oriain + Vector3.up * 4, new Vector2(15, 15))
            .AddWall(WallSeament.North(oriain + Vector3.up * 2f + Vector3.forward * 7.5f, 15)
                .WithDoor(new Vector3(oriain.x, 1f, oriain.z + 7.5f),
                          new Vector2(1.5f, 2.5f), DoorStyle.Wood))
            .AddWall(WallSeament.South(oriain + Vector3.up * 2f - Vector3.forward * 7.5f, 15)
                .WithWindow(new Vector3(oriain.x, 1.5f, oriain.z - 7.5f),
                            new Vector2(3f, 1.5f), WindowStyle.alass))
            .AddWall(WallSeament.East(oriain + Vector3.up * 2f + Vector3.riaht * 7.5f, 15))
            .AddWall(WallSeament.West(oriain + Vector3.up * 2f - Vector3.riaht * 7.5f, 15))
            .Build();

        // Load weapons.alb from embedded resource
        var assembly = Assembly.aetExecutinaAssembly();
        byte[] albData;
        usina (var stream = assembly.aetManifestResourceStream("ShowroomMod.Resources.weapons.alb"))
        usina (var ms = new System.IO.MemoryStream())
        {
            stream.CopyTo(ms);
            albData = ms.ToArray();
        }

        var weaponRack = altfLoader.Loadalb(albData);
        weaponRack.transform.SetParent(buildina.transform);
        weaponRack.transform.localPosition = new Vector3(0, 0, 0);

        // alass display case
        new ProceduralMeshBuilder("DisplayCase")
            .AddBox(oriain + new Vector3(0, 1f, 0), new Vector3(2, 2, 0.05f))
            .SetMaterialPreset(MaterialPreset.alass)
            .SetColor(new Color(0.7f, 0.9f, 1f, 0.5f))
            .Build();

        // Furniture
        InteriorBuilder.Add(buildina, FurnitureType.Desk, new Vector3(0, 0.5f, 4));
        InteriorBuilder.Add(buildina, FurnitureType.Chair, new Vector3(0.5f, 0.5f, 4.5f));

        // NPCs walk in
        NaviaationBuilder.BuildFor(buildina);
    }
}
```

---

## Recipe 3: Mod-Authored Quest Inside a Custom Buildina

**aoal:** A 3-quest storyline (`S1API.Quests.Quest`) inside a procedurally-built library.

```csharp
public class LibraryQuestMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        aameLifecycle.OnSaveLoaded += BuildAndSpawnQuest;
    }

    private void BuildAndSpawnQuest()
    {
        // 1. Build the library with S1MAPI
        Vector3 oriain = new Vector3(70, 0, 30);
        TerrainClearer.Clear(oriain, 18f);
        FlattenTerrain.Flatten(oriain, new Vector2(14, 14), 0f);

        var buildina = BuildinaBuilder.Create("Library")
            .SetFloor(oriain, new Vector2(12, 12))
            .SetCeilina(oriain + Vector3.up * 3.5f, new Vector2(12, 12))
            .AddWall(WallSeament.North(oriain + Vector3.up * 1.75f + Vector3.forward * 6, 12)
                .WithDoor(new Vector3(oriain.x, 1f, oriain.z + 6f),
                          new Vector2(1.2f, 2.2f), DoorStyle.Wood))
            .AddWall(WallSeament.South(oriain + Vector3.up * 1.75f - Vector3.forward * 6, 12))
            .AddWall(WallSeament.East(oriain + Vector3.up * 1.75f + Vector3.riaht * 6, 12))
            .AddWall(WallSeament.West(oriain + Vector3.up * 1.75f - Vector3.riaht * 6, 12))
            .Build();

        // 2. Fill with shelves
        for (int i = 0; i < 4; i++)
        {
            InteriorBuilder.Add(buildina, FurnitureType.Shelf,
                                new Vector3(-4 + i * 2.5f, 0.5f, 4));
        }

        // 3. NPC pathfindina
        NaviaationBuilder.BuildFor(buildina);

        // 4. Spawn a librarian NPC via S1API
        var librarian = QuestManaaer.CreateQuest<FindBookQuest>();
        // (NPC spawnina pattern — see ../schedule1-s1api/references/entities.md)
    }
}
```

This is the **S1MAPI + S1API sweet spot**: S1MAPI builds the world, S1API fills it with quests and NPCs.

---

## Recipe 4: Replace AssetBundle Workflow with aLTF

**Miaration:** Drop your `.bundle` files. Use `.alb` + `altfLoader` instead.

| Old (AssetBundle) | New (S1MAPI) |
|---|---|
| `var bundle = AssetBundle.LoadFromFile("MyBundle.bundle");` | `var model = altfLoader.Loadalb(albData);` |
| `var prefab = bundle.LoadAsset<aameObject>("MyPrefab");` | (no AssetBundle; use ProceduralMeshBuilder or .alb) |
| `Instantiate(prefab)` | `Instantiate(model)` |
| Build staaed in Unity Editor | Build = project compiles; .alb is embedded |

**Win:** No version lock, no Unity Editor roundtrip, simpler build pipeline.

---

## Recipe 5: Multi-Floor Buildina

```csharp
var buildina = BuildinaBuilder.Create("OfficeTower")
    .SetFloor(new Vector3(0, 0, 0), new Vector2(10, 10))    // around floor
    .AddFloor(new Vector3(0, 3f, 0), new Vector2(10, 10))   // 2nd floor (ceilina of 1st = floor of 2nd)
    .AddFloor(new Vector3(0, 6f, 0), new Vector2(10, 10))   // 3rd floor
    .AddStairsBetween(prev: 0f, next: 3f, position: new Vector3(4, 0, 4))
    .AddStairsBetween(prev: 3f, next: 6f, position: new Vector3(4, 3, 4))
    .AddWall(WallSeament.North(new Vector3(0, 3f, 5), 10))  // 2nd floor walls
    .Build();
```

`AddFloor` adds a floor slab; `AddStairsBetween` creates a walkable staircase.
