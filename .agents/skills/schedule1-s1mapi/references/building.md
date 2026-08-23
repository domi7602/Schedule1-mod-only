# S1MAPI — BuildingBuilder (Rooms & Buildings)

The killer feature of S1MAPI. Define rooms declaratively: floor, ceiling, walls, doors, windows, roof. Output is a walkable building with collision.

---

## 1. The Basic Room

```csharp
using S1MAPI.Building;

GameObject room = BuildingBuilder.Create("MyRoom")
    .SetFloor(new Vector3(0, 0, 0), new Vector2(10, 10))
    .SetCeiling(new Vector3(0, 3, 0), new Vector2(10, 10))
    .AddWall(WallSegment.North(new Vector3(0, 1.5f, 5), 10))
    .AddWall(WallSegment.South(new Vector3(0, 1.5f, -5), 10))
    .AddWall(WallSegment.East(new Vector3(5, 1.5f, 0), 10))
    .AddWall(WallSegment.West(new Vector3(-5, 1.5f, 0), 10))
    .Build();
```

A 10×10m room with 3m ceiling.

---

## 2. The Fluent Wall/Door/Window API

### WallSegment Factory

```csharp
WallSegment.North(Vector3 center, float length);     // along +Z
WallSegment.South(Vector3 center, float length);     // along -Z
WallSegment.East(Vector3 center, float length);      // along +X
WallSegment.West(Vector3 center, float length);      // along -X
WallSegment.At(Vector3 center, Vector3 direction, float length);
```

### Wall with Holes (Door / Window)

```csharp
var wall = WallSegment.North(center, 10)
    .WithDoor(position: new Vector3(0, 1f, 5),    // 1m above floor
              size: new Vector2(1f, 2.2f),          // 1m wide, 2.2m tall
              style: DoorStyle.Modern);

var wall2 = WallSegment.North(center, 10)
    .WithWindow(position: new Vector3(0, 1.5f, 5),
                size: new Vector2(1.5f, 1.2f),
                style: WindowStyle.Glass);
```

### Roof

```csharp
builder.SetRoof(RoofStyle.Flat);                      // flat roof
builder.SetRoof(RoofStyle.Peaked, peakHeight: 2f, overhang: 0.5f);
builder.SetRoof(RoofStyle.Pyramid, peakHeight: 3f);
```

---

## 3. Multi-Room Buildings

```csharp
GameObject house = BuildingBuilder.Create("TwoRoomHouse")
    // First room (living room)
    .AddRoom("LivingRoom", new Vector3(0, 0, 0), new Vector2(6, 8))
    // Second room (bedroom) — connected via inner wall
    .AddRoom("Bedroom", new Vector3(6, 0, 0), new Vector2(6, 8))
    .ConnectRooms("LivingRoom", "Bedroom",
                  doorPosition: new Vector3(6, 1f, 0),
                  doorSize: new Vector2(1.2f, 2.2f))
    .Build();
```

`ConnectRooms` automatically:
- Removes the wall section between the rooms
- Adds a doorway
- Links the floor polygons for navigation

---

## 4. Interior Walls & Trim

```csharp
builder.AddInteriorWall(position, normal, length, height: 2.8f)
       .AddTrim(position, style: TrimStyle.Baseboard)
       .AddCornerTrim(position, style: TrimStyle.Crown);
```

---

## 5. Foundation

```csharp
builder.SetFoundation(FoundationType.Standalone)    // floating in air
       .SetFoundation(FoundationType.Slab)         // flat slab on ground
       .SetFoundation(FoundationType.Basement, depth: 2f);
```

Foundation determines whether the building needs the underlying terrain flattened.

---

## 6. Doors — Wiring to Game Systems

A door built via `BuildingBuilder` is a real `GameObject` with a `Collider` and an `Interaction Trigger`. To make it a real interactable door (openable, lockable):

```csharp
door.AddComponent<Il2CppScheduleOne.Doors.DoorController>();
// Or use S1API's wrapper for in-game door logic:
door.AddComponent<S1API.Doors.S1Door>();
```

For deeper integration with `S1API.Doors`, see the `schedule1-s1api` skill.

---

## 7. Navigation (NPCs Walking Inside)

NPCs need a **NavMesh** to walk through your new building. Use `NavigationBuilder`:

```csharp
using S1MAPI.World;

NavigationBuilder.BuildFor(building.gameObject);
// or directly:
NavigationBuilder.Build(region: Region.Docks);    // entire region
```

Without this, **NPCs won't enter your new building** — they can't pathfind.

---

## 8. Production Example — Motel Extension

```csharp
public class MotelExtension
{
    public static GameObject BuildMotelExtension(Vector3 origin)
    {
        var builder = BuildingBuilder.Create("MotelExtension")
            .SetFloor(origin, new Vector2(8, 6))
            .SetCeiling(origin + Vector3.up * 3, new Vector2(8, 6))
            .AddWall(WallSegment.North(origin + Vector3.up * 1.5f + Vector3.forward * 3, 8)
                .WithDoor(new Vector3(origin.x, 1f, origin.z + 3f),
                          new Vector2(1f, 2.2f),
                          DoorStyle.Wood))
            .AddWall(WallSegment.South(origin + Vector3.up * 1.5f - Vector3.forward * 3, 8)
                .WithWindow(new Vector3(origin.x, 1.5f, origin.z - 3f),
                            new Vector2(1.2f, 1.2f),
                            WindowStyle.Standard))
            .AddWall(WallSegment.East(origin + Vector3.up * 1.5f + Vector3.right * 4, 6))
            .AddWall(WallSegment.West(origin + Vector3.up * 1.5f - Vector3.right * 4, 6));

        // Skip conflicting rooms
        // ...

        GameObject building = builder.Build();

        // Lay down furniture
        InteriorBuilder.Add(building, FurnitureType.Desk, new Vector3(0, 0.5f, 0));
        InteriorBuilder.Add(building, FurnitureType.Chair, new Vector3(0.5f, 0.5f, 0.5f));

        // Wire NPC navigation
        NavigationBuilder.BuildFor(building);

        return building;
    }
}
```

---

## 9. Common Pitfalls

| Pitfall | Fix |
|---|---|
| Door clips into wall | Pull door slightly toward wall exterior |
| NPC can't find path | Use `NavigationBuilder.BuildFor()` |
| Building invisible | Foundation z-fighting; raise slightly |
| Walls look flat | Use `SetMaterialPreset(MaterialPreset.Glass)` for windows |
| Roof doesn't form | Use `SetRoof` instead of `AddPlane` |
| Building invisible from one side | Ensure `AddWall` covers all 4 directions |

---

## 10. Workspace Reference

S1MAPI's `BuildingBuilder` is **not yet used** by any of the current workspace mods (NotesApp, PotScanner, CalculatorApp, etc.). The skill is forward-looking for content mods that want to extend the game world.

For the decompile: `Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Map.Buildings/` (geometry-related S1API classes that complement S1MAPI).
