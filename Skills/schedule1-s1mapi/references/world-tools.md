# S1MAPI — World Tools (Terrain, Navigation, Prefab Placement)

The "advanced" world-modification API. Use these when you need to integrate procedural geometry with the existing game world: clear surroundings, flatten terrain, build NPC navigation, place in-game prefabs.

---

## 1. TerrainClearer

Remove vegetation, trees, rocks, and other objects around a build site.

```csharp
using S1MAPI.World;

TerrainClearer.Clear(origin: new Vector3(0, 0, 0),
                     radius: 20f);   // 20m radius
```

Use it before placing a new building to ensure a clean construction site.

---

## 2. FlattenTerrain

Flatten terrain around a build site as a foundation.

```csharp
FlattenTerrain.Flatten(origin: new Vector3(0, 0, 0),
                       size: new Vector2(20, 20),   // 20×20m flat area
                       targetHeight: 0f);            // absolute y
```

The terrain is reduced to a flat plane at `targetHeight` over the specified area.

---

## 3. NavigationBuilder

Build NPC pathfinding (A* navmesh) for new areas. **Without this, NPCs won't walk into your custom area.**

```csharp
NavigationBuilder.BuildFor(myBuilding.gameObject);
```

Or for an entire region:

```csharp
NavigationBuilder.Build(Region.Docks);
```

### When to Use

| Situation | Required? |
|---|---|
| Procedural building with NPCs inside | ✅ Yes |
| Static decorative structure | ❌ No |
| Procedural building with no NPCs | ❌ No |
| New area on the map (extension) | ✅ Yes |

---

## 4. PrefabPlacer

Place in-game prefabs (real game objects, e.g., cars, doors, NPCs) at runtime. With `networked: true`, the state syncs across multiplayer clients.

```csharp
using S1MAPI.World;

PrefabPlacer.Place(prefab: myCustomBuildingPrefab,
                  position: new Vector3(0, 0, 0),
                  rotation: Quaternion.identity,
                  networked: true);
```

### Common Use Cases

| Prefab Type | Use for |
|---|---|
| Doors | Real interactable doors |
| Lights | Functional in-game lighting |
| NPCs | Real NPCs in custom positions |
| Vehicles | Park spawned vehicles |
| Containers | Interactable storage |

---

## 5. Complete Workflow — Adding a New Region

```csharp
public class NewRegionBuilder
{
    public static GameObject BuildHotelExtension()
    {
        // 1. Clear existing terrain
        TerrainClearer.Clear(origin: new Vector3(50, 0, 50), radius: 30f);

        // 2. Flatten the build site
        FlattenTerrain.Flatten(origin: new Vector3(50, 0, 50),
                               size: new Vector2(20, 20),
                               targetHeight: 0f);

        // 3. Build the building
        var building = BuildingBuilder.Create("HotelExtension")
            .SetFloor(new Vector3(50, 0, 50), new Vector2(20, 20))
            .SetCeiling(new Vector3(50, 3, 50), new Vector2(20, 20))
            .AddWall(WallSegment.North(new Vector3(50, 1.5f, 60), 20)
                .WithDoor(new Vector3(50, 1f, 60),
                          new Vector2(1.5f, 2.2f),
                          DoorStyle.Wood))
            .AddWall(WallSegment.South(new Vector3(50, 1.5f, 40), 20))
            .AddWall(WallSegment.East(new Vector3(60, 1.5f, 50), 20))
            .AddWall(WallSegment.West(new Vector3(40, 1.5f, 50), 20))
            .Build();

        // 4. Add interior
        InteriorBuilder.Add(building, FurnitureType.Desk, new Vector3(50, 0.5f, 50));
        InteriorBuilder.Add(building, FurnitureType.Chair, new Vector3(50.5f, 0.5f, 50.5f));

        // 5. Build navigation so NPCs can walk in
        NavigationBuilder.BuildFor(building);

        // 6. (Optional) Place networked prefabs
        // PrefabPlacer.Place(doorPrefab, new Vector3(50, 1f, 60), networked: true);

        return building;
    }
}
```

---

## 6. Edge Cases & Caveats

| Caveat | Why | Fix |
|---|---|---|
| NPC walks into wall | NavMesh says "yes" but geometry blocks | Re-run `NavigationBuilder` after geometry changes |
| Cleared terrain suddenly respawns vegetation | Game spawns new vegetation periodically | Use `FlattenTerrain` regularly, or pre-claim area with `Marker` |
| Placed prefab disappears on save load | Game restores its own state | Use `PrefabPlacer` with `networked: true` + S1API `Saveable` integration |
| NavigationBuilder takes a long time | Large area | Build per-room, not per-building |

---

## 7. Performance Notes

| Op | Cost | Notes |
|---|---|---|
| `TerrainClearer` | O(n) for n objects in radius | Use sparingly |
| `FlattenTerrain` | O(1) | Cheap |
| `NavigationBuilder` | O(area) | Heavy for large areas, prefer per-room |
| `PrefabPlacer` | O(1) per placement | Cheap |

---

## 8. Workspace Reference

None of the current workspace mods use `World` tools yet. The skill is forward-looking for content mods that want to extend the game world.

For the official S1MAPI docs: https://github.com/ifBars/S1MAPI/tree/stable/docs/examples.md
