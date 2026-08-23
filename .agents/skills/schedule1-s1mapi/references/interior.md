# S1MAPI — InteriorBuilder & FurnitureType

`InteriorBuilder` places in-game furniture (desks, chairs, beds, …) inside custom buildings or anywhere in the world. This bridges geometry back to the game world — your procedural building can host real, interactable game objects.

---

## 1. Quickstart

```csharp
using S1MAPI.Interior;

InteriorBuilder.Add(parent: room, type: FurnitureType.Desk, position: new Vector3(0, 0.5f, 0));
InteriorBuilder.Add(parent: room, type: FurnitureType.Chair, position: new Vector3(0.5f, 0.5f, 0.5f));
```

A desk and chair appear in the room.

---

## 2. FurnitureType Enum

```csharp
public enum FurnitureType
{
    Desk,
    Chair,
    Bed,
    Table,
    Shelf,
    Lamp,
    Couch,
    Stove,
    Fridge,
    Washer,
    Dryer,
    // ... 30+ types
}
```

> The exact list varies by S1MAPI version. See `Knowledge/Analysis/Learnings/ThirdParty/S1MAPI_Il2Cpp.md` for the deployed version's list.

---

## 3. The `Add` Method

```csharp
public static GameObject Add(
    GameObject parent,
    FurnitureType type,
    Vector3 position,
    Quaternion rotation = default,
    bool interactable = true,
    bool networked = false
);
```

| Param | Default | Notes |
|---|---|---|
| `parent` | required | The room or scene root |
| `type` | required | From `FurnitureType` enum |
| `position` | required | Local pos relative to parent |
| `rotation` | `default` (identity) | Local rotation |
| `interactable` | `true` | Adds an interaction trigger |
| `networked` | `false` | Sources a `NetworkPrefab` for multiplayer sync |

---

## 4. Production Patterns

### Pattern A: Office Room

```csharp
public void SetupOffice(GameObject room)
{
    InteriorBuilder.Add(room, FurnitureType.Desk, new Vector3(0, 0.5f, 0));
    InteriorBuilder.Add(room, FurnitureType.Chair, new Vector3(0, 0.5f, -0.5f));
    InteriorBuilder.Add(room, FurnitureType.Lamp, new Vector3(0, 1.5f, -0.5f),
                        Quaternion.Euler(0, 90, 0));
    InteriorBuilder.Add(room, FurnitureType.Shelf, new Vector3(2, 0.5f, 0));
}
```

### Pattern B: Living Room with Rotation

```csharp
InteriorBuilder.Add(room, FurnitureType.Couch, new Vector3(0, 0.4f, 2),
                    Quaternion.Euler(0, 180, 0));   // facing -Z

InteriorBuilder.Add(room, FurnitureType.Table, new Vector3(0, 0.4f, 3));

InteriorBuilder.Add(room, FurnitureType.TV, new Vector3(0, 1.2f, 4.8f),
                    Quaternion.Euler(0, 180, 0));   // facing -Z
```

### Pattern C: Networked Interactable

```csharp
var door = InteriorBuilder.Add(parent: room, type: FurnitureType.Door,
                              position: new Vector3(0, 1f, 5),
                              networked: true);
// Now the door's state syncs across multiplayer clients
```

---

## 5. Common Pitfalls

| Pitfall | Fix |
|---|---|
| Furniture floats | Use proper y-position (e.g., 0.5f for a desk surface) |
| Door clips into wall | Position carefully, use `DoorBuilder` for precise doors |
| Furniture doesn't interact | Ensure `interactable: true` |
| Multiplayer desync | Use `networked: true` for state-synced objects |
| Furniture not aligned | Set `rotation` explicitly |

---

## 6. Custom Items (Not in `FurnitureType`)

For custom furniture not in the enum, use a different builder:

```csharp
using S1API.Items.Buildable;

var myCustomFurniture = new MyCustomFurniture();   // extends BuildableItemDefinition
var instance = myCustomFurniture.GetDefaultInstance();
instance.transform.SetParent(room.transform);
instance.transform.localPosition = new Vector3(0, 0, 0);
```

See `schedule1-s1api/references/items-products.md` for the `BuildableItemDefinition` pattern.

---

## 7. Workspace Reference

`HomelessMod`'s procedural SleepingBag is **not** an `InteriorBuilder.Add(...)` — it uses S1API's `BuildableItemDefinition` because it's a custom (not vanilla) furniture. For vanilla furniture in custom buildings, use `InteriorBuilder`.

For the decompile: `Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Items.Buildable/`.
