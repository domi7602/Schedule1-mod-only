# S1MAPI — Furnishings and Functional Prefabs

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 10/12 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Evidence boundary

The public API consulted on 2026-10-21 places `InteriorBuilder` in `S1MAPI.Building.Interior`. It is an instance builder with furniture-specific methods and a separate prefab path. The earlier static `InteriorBuilder.Add(... FurnitureType ..., networked: ...)` signature was not established and has been removed.

## Separate three kinds of content

| Content | Responsibility | Required evidence |
|---|---|---|
| Decorative furniture mesh | Visual hierarchy and collision as needed | Mesh/material availability and ownership |
| Procedural furniture from a building helper | Geometry placed in building-local coordinates | Actual enum/helper members and generated components |
| Functional native prefab | Gameplay, interaction, network and persistence contracts | Registered prefab, authority, initialization and restore owner |

The published `FurnitureType` enum is not a catalog of every game furnishing. At the consulted page it lists Table, Chair, Desk, Bookshelf, Counter, and CoffeeTable. Do not infer beds, doors, or appliances from an imagined 30-item enum.

## API lookup guidance

The public interior builder lists methods such as `AddBed`, `AddChair`, `AddDesk`, `AddCustomMesh`, `AddCustomObject`, and `AddPrefab`. Resolve exact signatures and result semantics in the target assembly. Mesh-based furnishings do not automatically become functional native storage, usable computers, or networked doors.

Source anchors to locate: `ThirdParty/S1MAPI/Building/Interior/InteriorBuilder.cs`, furniture builders, and the prefab placement implementation. Prefer searching the type name if the checkout uses another directory structure.

## Placement workflow

1. Define each furnishing with a stable ID, role, parent, local position, rotation, bounds, and optional interaction.
2. Confirm the root's coordinate convention. A local Y value is not automatically the desktop height or floor contact point.
3. Resolve all required mesh/material/prefab resources before activating the item.
4. Create under one owned interior root and apply the intended local transform exactly once.
5. Inspect collision and clearance around chairs, work surfaces, storage fronts, and entrances.
6. Add native functionality only through a verified initialization/registration path.
7. If networked, choose server spawn and client linking explicitly; test late join and callbacks.
8. Record restoration ownership and extra state before introducing saving.

## Custom buildable furniture

An inventory item instance and a placed GameObject are different responsibilities. Do not access an assumed `transform` on an item created by a definition and treat it as a spawned furnishing. Follow the current item/buildable registration and placement path; let native restoration own registered buildables when applicable.

For mod-owned furniture, record the definition ID and placement data, then use a deterministic factory during rebuild. Avoid manually spawning another copy of something the native save system already restores.

## Practical room layouts

- Office: desk, chair, light, storage; verify chair clearance and interaction reach.
- Lounge: seating, low table, wall display; verify line of sight and walking space.
- Functional shop: separate decorative shell from registered storage, checkout, doors, and worker stations.

Keep these as declarative definitions, not a fixed sequence of unexplained coordinates. Validate the layout after transforms and scale are applied.

## Failure and verification

Missing resources must not produce partially registered functional entities. On failure remove only owned partial output and keep the definition available for diagnosis/retry. Test duplicate IDs, repeated build, absent prefab, blocked placement, multiplayer authority, save/reload, and cleanup while shared assets remain in use.

## Sources

- [Official InteriorBuilder API](https://ifbars.github.io/S1MAPI/api/S1MAPI.Building.Interior.InteriorBuilder.html).
- [Official FurnitureType enum](https://ifbars.github.io/S1MAPI/api/S1MAPI.Building.Interior.FurnitureType.html).
- [Item/buildable guidance](../../schedule1-items/SKILL.md) and [geometry persistence](persistence.md).
