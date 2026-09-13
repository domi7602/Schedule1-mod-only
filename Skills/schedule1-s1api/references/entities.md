# S1API — Entities (NPCs)

`S1API.Entities` and its sub-namespaces wrap the vanilla `Il2CppScheduleOne.NPCs` system with a builder-pattern, region-aware, saveable NPC creation API.

---

## 1. Quickstart

```csharp
using S1API.Entities;
using S1API.Entities.NPCs;
using S1API.Entities.Appearances;

public sealed class MyFirstNPC : NPC
{
    protected override bool IsPhysical => true;   // if true, 3D model in world

    protected override void ConfigurePrefab(NPCPrefabBuilder builder)
    {
        builder.WithIdentity(id: "my_first_npc", firstName: "John", lastName: "Doe")
               .WithSpawnPosition(new Vector3(0, 0, 0))
               .EnsureCustomer()   // adds customer behaviour
               .WithCustomerDefaults(cd => {
                   cd.WithSpending(100f, 500f)        // budget per order
                     .WithOrdersPerWeek(1, 3);
               });
    }

    protected override void OnCreated()
    {
        base.OnCreated();
        Appearance.Set<CustomizationFields.Gender>(0.5f)
                  .Set<CustomizationFields.Height>(1.0f)
                  .Build();
        Schedule.Enable();               // daily routine
        Schedule.InitializeActions();
    }
}
```

### Recommended Learning Path

1. Basic NPC (no `IsPhysical`)
2. Physical NPC with default appearance
3. Physical NPC with custom appearance
4. Dialogue, Schedule, Customer, Relationship

---

## 2. Region System

Each physical NPC must be assigned to a region:

```csharp
public class MyDocksNPC : NPC
{
    protected override Region[] SpawnRegions => new[] { Region.Docks };
}
```

Available regions:
- `Region.Docks`
- `Region.Downtown`
- `Region.Northtown`
- `Region.Suburbia`
- `Region.Uptown`
- `Region.Westville`
- `Region.Sewers` (Cartel)

NPCs spawn based on region availability (after world load).

---

## 3. NPCPrefabBuilder

```csharp
builder.WithIdentity(id: "alice", firstName: "Alice", lastName: "Smith", middleName: "J", suffix: "PhD")
       .WithSpawnPosition(new Vector3(10, 0, 5))
       .WithSpawnRotation(Quaternion.Euler(0, 90, 0))
       .WithDefaultAppearance(AppearancePresets.Casual)   // optional preset
       .WithCustomerDefaults(cd => cd.WithSpending(200f, 800f).WithOrdersPerWeek(2, 5))
       .WithRelationshipDefaults(rd => rd.WithStartingTier(RelationshipTier.Acquaintance))
       .EnsureCustomer()                  // adds customer behaviour to the NPC
       .EnsureDealer()                    // adds dealer behaviour
       .EnsureEmployee()                  // adds employee behaviour
```

---

## 4. Modular Subsystems

Each subsystem is a property of the NPC wrapper:

| Subsystem | Access | Configure |
|---|---|---|
| `Appearance` | `Appearance.Set<...>(v).Build()` | Hair, body, gender, height |
| `Schedule` | `Schedule.Enable()` + `InitializeActions()` | Daily routine, A* pathfinding |
| `Dialogue` | `Dialogue.AddLine("...", condition)` | Voice line + trigger |
| `Relationship` | `Relationship.SetTier(RelationshipTier.X)` | Tier (Stranger → Family) |
| `Customer` | (via `EnsureCustomer`) | Spending, order frequency, preferences |
| `Dealer` | (via `EnsureDealer`) | Stock, prices |

```csharp
Appearance.Set<CustomizationFields.Gender>(0f)         // 0 = male, 1 = female
          .Set<CustomizationFields.Height>(1.1f)
          .Set<CustomizationFields.SkinTone>(0.3f)
          .Set<CustomizationFields.HairStyle>(3)
          .Set<CustomizationFields.HairColor>(Color.black)
          .Build();
```

---

## 5. Physical vs Non-Physical

```csharp
public class PhoneContact : NPC
{
    protected override bool IsPhysical => false;   // not in world, only via phone
}
```

Non-physical NPCs are phone contacts: no 3D model, only messaging/story. Useful for quest givers who only appear in dialogue.

---

## 6. Lifecycle

```csharp
protected override void ConfigurePrefab(NPCPrefabBuilder builder)  // REQUIRED
protected override void OnCreated()                              // called after instantiation
protected override void OnLoaded()                              // called after save load
protected override void OnSaveStarted()                         // pre-save
protected override void OnSaveCompleted()                       // post-save
protected override void OnDestroyed()                            // final cleanup
```

**Important:** `OnCreated` runs AFTER `ConfigurePrefab`. Use `OnCreated` to register event handlers, `OnLoaded` to restore state from saved `[SaveableField]` data.

---

## 7. Gotchas

* **NPC not appearing in world?** Region is missing or `SpawnRegions` is empty. Check `S1API.Entities.NPCs.Region` enum.
* **NPC appearing in wrong gender?** `Appearance.Set<CustomizationFields.Gender>(...)` — `0 = male`, `1 = female`.
* **NPC not moving?** `Schedule.Enable()` + `Schedule.InitializeActions()` required for A* pathfinding.
* **Customer not buying?** `EnsureCustomer()` + `WithCustomerDefaults(...)` both required.

---

## 8. Workspace Reference

`HomelessMod` does NOT use `S1API.Entities` NPCs (it uses `Il2CppScheduleOne.EntityFramework` directly for its procedural CampingBag). However, `S1API.Entities.NPCs.*` is the recommended path for adding new NPCs that interact with the world's economy.

For the decompile: `Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Entities/` and sub-namespaces.
