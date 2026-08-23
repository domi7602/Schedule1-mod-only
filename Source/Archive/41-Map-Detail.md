# Map/ — Regions + Access

**Path:** `Source/Game/Scripts/scripts/Assembly-CSharp/ScheduleOne/Map/` · **34 files**

Regions, access zones, DarkMarket, NPC regions.

> Detailed documentation: `../systems/13-Regions.md`

---

## Class Inventory

### Region Base

| File | Purpose |
|-------|-------|
| `Region.cs` | **Base region** |
| `RegionManager.cs` (?) | Global region manager |
| `EMapRegion.cs` (Enum) | Map regions (6 items) |

### Access Zones

- `AccessZone.cs` — Base access zone
- `AutoshopAccessZone.cs` — Autoshop access
- `DarkMarketAccessZone.cs` — DarkMarket access
- `NPCEnterableRegion.cs` — Enterable by NPC
- `RestrictedAccessZone.cs` (?) — Restricted access zone
- `PropertyAccessZone.cs` (?) — Property access

### Special Zones

- `DarkMarket.cs` — DarkMarket main building
- `DarkMarketMainDoor.cs` — DarkMarket main door
- `DarkMarketInterior.cs` (?) — Interior

### Map Features

- `NPCRegion.cs` (?) — NPC region
- `MapRegion.cs` (?) — Map region
- `RegionCoordinate.cs` (?) — Region coordinate
- `CartelInfluenceRegion.cs` (?) — Cartel influence
- `InfluenceMap.cs` (?) — Influence map

### Miscellaneous

- `MapHeightSampler.cs` (in DevUtilities/) — Height sampling
- `MapNavMeshBuilder.cs` (?) — NavMesh builder
- `POI.cs` (?) — Point of Interest
- `Waypoint.cs` (?) — Waypoint

---

## Important Fields (Region)

```csharp
public string RegionName;
public Bounds RegionBounds;
public EMapRegion RegionType;
public float CartelInfluence;
public bool IsUnlocked;

public bool IsPositionInRegion(Vector3 worldPos);
public void UnlockRegion(Player player);
public float GetInfluence();
```

### EMapRegion

```csharp
public enum EMapRegion
{
    Downtown,        // Main business district
    Docks,           // Harbor / Docks
    Suburb,          // Suburbs
    Industrial,      // Industrial area
    Residential,     // Residential area
    Sewer,           // Sewer system
}
```

### AccessZone

```csharp
public Bounds ZoneBounds;
public EAccessRequirement Requirement;
public string RequiredRank;
public bool IsAccessibleTo(Player player);
```

---

## Modding Hooks

```csharp
// Custom Region:
public class MyRegion : Region
{
    public override bool IsPositionInRegion(Vector3 pos) => /* ... */;
}

// Unlock-Hook:
[HarmonyPatch(typeof(RegionManager), "UnlockRegion")]
class Patch_Unlock { /* ... */ }
```