# Outdoor Placement — Golden Rules 1, 4, 5

Applies when placing buildables outside purchased properties (e.g. HomelessMod street items). See `SKILL.md` for class map.

## 1. NEVER Destroy GridItem — disable instead

Calling `DestroyImmediate(gridItem)` leaves dangling native pointers and throws NREs in vanilla queries.

```csharp
var gridItems = placedObj.GetComponentsInChildren<GridItem>(true);
foreach (var gi in gridItems)
{
    if (gi != null && gi.Pointer != IntPtr.Zero)
    {
        try { gi.SetFootprintTileVisiblity(false); } catch { }
        gi.enabled = false;
    }
}
```

Harmony: prefix `GridItem.Destroy` → `return false` if `StreetPropertyManager.IsOutdoorItem(__instance.gameObject)`.

## 4. Never attach Property to virtual root

`StreetNomad_WorldRoot` must be a clean `GameObject` with `DontDestroyOnLoad`. Attaching `Property` poisons phone real-estate, business dashboards, police raids (they enumerate `Property` instances). Manage via `StreetPropertyManager` singleton.

## 5. Footprint & Grid Tile Suppression

Vanilla prefabs spawn `FootprintTile`/`TileAppearance` indicators. Suppress in 3 steps:

1. `ft.gameObject.SetActive(false)` for all `FootprintTile` children
2. `gi.SetFootprintTileVisiblity(false)` on all GridItems
3. Filter Renderers — disable footprint renderers, keep mesh renderers:

```csharp
var renderers = placedObj.GetComponentsInChildren<Renderer>(true);
foreach (var r in renderers)
{
    if (r == null || r.Pointer == IntPtr.Zero) continue;
    if (r.GetComponentInParent<FootprintTile>() != null ||
        r.GetComponentInParent<TileAppearance>() != null)
        r.enabled = false;
    else
        r.enabled = true;
}
```

Related: `collision-and-ghosts.md` for ground sampling, `build-update-patching.md` for save timing.
