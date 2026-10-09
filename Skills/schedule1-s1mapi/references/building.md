# S1MAPI — BuildingBuilder (Rooms & Buildings)
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 50/65 identifier-shaped tokens resolve (13 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.


The killer feature of S1MAPI. Define rooms declaratively: floor, ceiling, walls, doors, windows, roof. Output is a walkable building with collision.

---

## 1. The Basic Room

> **Corrected 2026-10-08 against the checked-in S1MAPI source.** `BuildingBuilder` uses
> `AddFloor` / `AddCeiling` / `AddWalls` / `AddInteriorWall` / `AddStairs` / `AddHipRoof` /
> `AddParapetRoof` / `AddDoorFrames` / `AddInteriorDoorFrames` / `AddSlidingDoors` /
> `AddFurniture` / `AddPrefab` / `AddLights` / `AddAmbientLighting` / `DefineRoom` /
> `WithConfig` / `WithPalette` / `WithInteriorWallLayer` / `CreateNavigationBuilder` /
> `FlattenTerrain` / `Build`.
> There is **no** `WallSegment` type, no `SetFloor`/`SetCeiling`/`SetRoof`/`AddWall`
> (singular) and no `ConnectRooms`. Wall geometry is handled by the dedicated
> `WallBuilder` (`BuildWall`, `BuildWalls`), `InteriorWallBuilder` (`BuildInteriorWall`),
> `RoofBuilder`, `DecorBuilder` and `InteriorBuilder`.

```csharp
using S1MAPI.Building;

var room = BuildingBuilder.Create("MyRoom")
    .AddFloor(...)          // floor slab
    .AddCeiling(...)        // ceiling
    .AddWalls(...)          // exterior walls (WallBuilder: BuildWalls)
    .Build();
```

Re-derive exact overloads from `ThirdParty/S1MAPI/Building/BuildingBuilder.cs` —
the signatures above are verified as *member names*, not as parameter lists.

---

## 2. Walls, doors and windows

Walls are built through `WallBuilder`, not a `WallSegment` factory:

```csharp
WallBuilder.BuildWall(...);   // single wall
WallBuilder.BuildWalls(...);  // wall set
InteriorWallBuilder.BuildInteriorWall(...);
```

### Doors, windows and openings

Openings are expressed through the door-frame / sliding-door members, not through a
`WithDoor`/`WithWindow` wall modifier (those do not exist in the checked-in source):

```csharp
BuildingBuilder.Create("MyRoom")
    .AddDoorFrames(...)            // door frames
    .AddInteriorDoorFrames(...)    // interior door frames
    .AddSlidingDoors(...)          // sliding doors
    .Build();
```

Interior furnishing (walls, furniture, decor) is composed via `InteriorBuilder`
(`AddWall`, `AddBed`, `AddDesk`, `AddLocker`, `AddCustomMesh`, `AddPrefab`, …) and
`DecorBuilder` (`AddBaseMolding`, `AddCornerTrim`, `AddRoofTrim`, `AddStairs`, …).

> **Removed:** `WallSegment`, `WithDoor`, `WithWindow`, `DoorStyle`, `WindowStyle`
> are not present. Check `DoorwayInfo` / `InteriorWallAxis` / `InteriorWallDefinition`
> in `ThirdParty/S1MAPI/Building/` for the current opening model.

### Roof

```csharp
builder.AddHipRoof(...);        // hipped roof (RoofBuilder)
builder.AddParapetRoof(...);    // flat roof with parapet (RoofBuilder)
builder.AddRoofTrim(...);       // roof trim (DecorBuilder)
builder.AddSecondaryRoofTrim(...);
```

> **Removed:** `SetRoof` and `RoofStyle` do not exist in the checked-in source — roof
> shapes are explicit members (`AddHipRoof`, `AddParapetRoof`).

---

## 3. Multi-Room Buildings

Rooms are declared with `DefineRoom` and connected through the interior-wall /
door-frame members:

```csharp
var house = BuildingBuilder.Create("TwoRoomHouse")
    .DefineRoom(...)                  // room definition
    .AddInteriorWall(...)             // interior wall
    .AddInteriorDoorFrames(...)       // openings between rooms
    .WithInteriorWallLayer(...)
    .CreateNavigationBuilder()        // navigation for the finished shell
    .Build();
```

> **Removed:** `AddRoom(name, pos, size)` and `ConnectRooms(a, b, doorPosition, doorSize)`
> are not members of the checked-in `BuildingBuilder`. Room definition and room linking go
> through `DefineRoom` plus the wall/door-frame members; navigation is set up explicitly via
> `CreateNavigationBuilder()`.

---

## 4. Interior Walls & Trim

```csharp
builder.AddInteriorWall(...)          // interior wall
       .AddBaseMolding(...)           // base trim   (DecorBuilder)
       .AddCornerTrim(...)            // corner trim
       .AddRoofTrim(...)              // roof trim
       .AddSecondaryRoofTrim(...);
```

> **Removed:** `AddTrim(..., TrimStyle.…)` does not exist; trim is added through the
> explicit `AddBaseMolding` / `AddCornerTrim` / `AddRoofTrim` members. `TrimStyle`
> is not present in the checked-in source.

---

## 5. Foundation

```csharp
builder.AddFoundation(...);           // foundation (DecorBuilder)
```

Foundation determines whether the building needs the underlying terrain flattened.

---

## 6. Doors — Wiring to Game Systems

A door built via `BuildingBuilder` is a real `GameObject` with a `Collider` and an
interaction trigger. To make it a real interactable door (openable, lockable) attach the
vanilla door component, or use an S1API wrapper if one exists for your branch:

```csharp
door.AddComponent<Il2CppScheduleOne.Doors.DoorController>();
```

> **Corrected 2026-10-08:** `S1API.Doors.S1Door` does not exist in the checked-in S1API
> source. Verify the available door wrapper (or use the vanilla component) before relying
> on it; see `Skills/schedule1-s1api/references/entities.md` for the current wrapper list.

---

## 7. Navigation (NPCs Walking Inside)

NPCs need a **NavMesh** to walk through your new building. From inside a build chain use
`CreateNavigationBuilder()`; standalone use `NavigationBuilder`:

```csharp
using S1MAPI.Building;

BuildingBuilder.Create("MyRoom")
    .AddFloor(...)
    .AddWalls(...)
    .CreateNavigationBuilder()
    .Build();
```

> **Corrected 2026-10-08:** `NavigationBuilder.BuildFor(building.gameObject)` and
> `NavigationBuilder.Build(region)` are not members of the checked-in
> `NavigationBuilder`. Derive the actual entry points from
> `ThirdParty/S1MAPI/Building/NavigationBuilder.cs`.

Without this, **NPCs won't enter your new building** — they can't pathfind.

---

## 8. Production Example — Motel Extension

```csharp
public class MotelExtension
{
    public static GameObject BuildMotelExtension(Vector3 origin)
    {
        // Shapes below are member names verified against ThirdParty/S1MAPI on 2026-08.
        // Parameter lists are NOT verified — read the builder source before shipping.
        var builder = BuildingBuilder.Create("MotelExtension")
            .AddFloor(...)                       // floor slab
            .AddCeiling(...)                     // ceiling
            .AddWalls(...)                       // exterior walls (WallBuilder.BuildWalls)
            .AddDoorFrames(...)                  // door openings
            .AddSlidingDoors(...)                // sliding doors
            .AddParapetRoof(...)                 // roof (RoofBuilder)
            .AddBaseMolding(...)                 // trim
            .FlattenTerrain(...)                 // terrain under the footprint
            .CreateNavigationBuilder();          // NPC pathing

        GameObject building = builder.Build();

        // Lay down furniture
        // InteriorBuilder: AddDesk / AddChair / AddBed / AddLocker / …

        return building;
    }
}
```

---

## 9. Common Pitfalls

| Pitfall | Fix |
|---|---|
| Door clips into wall | Pull the door frame slightly toward the wall exterior |
| NPC can't find path | Call `CreateNavigationBuilder()` before `Build()` |
| Building invisible | Foundation z-fighting; raise slightly |
| Walls look flat | Assign a material via the material helpers / `SetMaterial` |
| Roof doesn't form | Use `AddHipRoof` / `AddParapetRoof` explicitly |
| Building invisible from one side | Ensure the wall set covers all sides |

---

## 10. Workspace Reference

S1MAPI's `BuildingBuilder` is **not yet used** by any of the current workspace mods (NotesApp, PotScanner, CalculatorApp, etc.). The skill is forward-looking for content mods that want to extend the game world.

For the source: `ThirdParty/S1API/` (S1API.Map.Buildings - geometry-related S1API classes that complement S1MAPI; initialize the submodule first if needed).

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `SetCeiling`
- `SetFloor`
