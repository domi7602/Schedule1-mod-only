# Trash/Recycling (Schedule I)

> Inherited, not rerun; [evidence scope](README.md#source-history).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 6/7 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Trash Generation
- `TrashGenerator`: Per property, spawns daily trash (up to 20% of max capacity)
- `TrashSpawnVolume`: Public areas
- Global limit: 2000 active trash items (oldest overwritten)

## Trash Types
- `TrashItem` (individual items, `SellValue` $0–$10)
- `TrashBag` (trash bag with contents, multiple items)

## Collection
- **TrashGrabber**: 20-slot tool
- **TrashBag** (equippable): Empty containers or collect floor trash in 0.45m radius

## Cleaner (NPCs)
- Priority chain: Bags → loose items → empty containers
- Work automatically within designated properties

## Recycler (Machine)
1. Open lid → drop trash inside (Physics.OverlapBox)
2. Press button → 1.5s processing time
3. Collect cash

### Calculation
```
Normal Items:  value = trashItem.SellValue              ($0–$10 per piece)
TrashBags:      value = Sum(entry.UnitValue × Quantity) (contents individually evaluated)
```

### Achievement
- `UPSTANDING_CITIZEN` upon recycling 500 items (Variable `"TrashRecycled"`)

## Console Command
- `cleartrash`: Immediately clears all trash in the world
- Recommended by TVGS to reduce CPU load (each piece of trash has a collider + networking)

## Important: Trash Has NO Impact on Heat/Police
Purely an aesthetics/management system. No littering crime, no traces left behind.

---

## Unresolved identifiers in the imported 2026-10-08 record

See [scan limits](README.md#source-history); unresolved does not mean removed.

- `TrashRecycled`
