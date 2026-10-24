# S1MAPI — Buildings, Openings, and Navigation

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 50/65 identifier-shaped tokens resolve (13 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Evidence boundary

The official `S1MAPI.Building.BuildingBuilder` API was inspected on 2026-10-21. The published API uses an instance constructor and staged building methods; it is not a certification for the user's assembly. Read the deployed signatures before implementing a chain.

## Public member map

| Stage | Public documentation lookup targets |
|---|---|
| Define dimensions/appearance | `WithConfig`, `DefineRoom`, `WithPalette` |
| Shell | `AddFloor`, `AddCeiling`, `AddWalls` |
| Internal partitions/openings | `AddInteriorWall`, `AddDoorFrames`, `AddInteriorDoorFrames` |
| Roof, foundation, trim | `AddHipRoof`, `AddParapetRoof`, `AddFoundation`, `AddBaseMolding` |
| Dependent content | `AddFurniture`, `AddPrefab`, `AddSlidingDoors`, `AddLights` |
| Navigation | `CreateNavigationBuilder` and the navigation object's own lifecycle |

This map identifies responsibilities, not overloads or interchangeable fluent return types. In particular, inspect the result of navigation creation before chaining another method.

## Shell construction

1. Define dimensions, floor elevation, wall thickness, local origin/pivot, entrances, and intended materials.
2. Validate the footprint against existing collision and native ownership boundaries.
3. Create the builder/root under an owner and configure dimensions before generating the shell.
4. Add floor, ceiling, and exterior walls with required openings. Add door frames after the corresponding walls.
5. Add internal partitions and their openings; verify the connections between rooms rather than merely their appearance.
6. Select a roof construction. Avoid stacking decorative roof trims that duplicate an existing roof's own geometry.
7. Build/finalize the geometry, set its final world transform, and inspect collision.
8. Prepare terrain/navigation only after their coordinate dependencies are satisfied.
9. Add gameplay integration after the geometry is usable and its owner is registered.

## Doors are an integration boundary

A visible opening or door mesh does not establish native interaction, physics, network identity, or save support. Choose a supported functional prefab or a verified native integration path. Do not attach a lone native DoorController and assume its serialized references, sensors, networking, and registration are initialized.

For a replicated door, verify who spawns it, when clients receive/link it, and where customization callbacks run. Host-only callbacks cannot initialize client-only presentation. Test manual interaction and proximity opening separately; they can have different authority paths.

## Navigation sequence

The public navigation documentation describes a building-specific navigation object with build, rebuild, and remove responsibilities. Position the building first. Build against final collision/opening data, retain the navigation owner, rebuild after relevant geometry changes, and release it when the building is removed.

Inspect whether the installed implementation uses native NavMesh data, carving, a custom interior grid, or a combination. Do not equate interior pathfinding with a general world NavMesh rebake. Validate the player, each required NPC agent, and both entry and exit paths.

## Multi-room and multi-floor plans

Express each floor, internal wall, opening, stair, landing, and link as layout data. Derive all transforms in the building frame. A series of stacked floors is not a usable multi-floor building until collision, headroom, stair traversal, and navigation between floors are verified.

The previous `AddRoom`/`ConnectRooms` and `AddStairsBetween` recipes were not established by the supplied evidence. Use only currently verified construction primitives and make the connecting geometry explicit.

## Failure handling and acceptance

- Missing required material/prefab: abort the affected phase before activation.
- Partial build: release owned children, meshes, materials, and navigation; leave borrowed assets intact.
- Repeated build: reconcile the root's stable identity instead of adding another building.
- Native restoration: attach to the restored entity rather than creating another copy.
- Test narrow doors, wall corners, roof/floor overlap, foundation height, all floor connections, save/reload, host/client roles, and scene teardown.

## Retired API recipes

The original `SetFloor`, `SetCeiling`, `SetRoof`, singular `AddWall` with `WallSegment`, `WithDoor`/`WithWindow`, and static `NavigationBuilder.BuildFor` examples contradicted the package's source corrections. They are not implementation instructions. Look up the actual builder and opening types rather than reviving those recipes.

## Sources and local anchors

- [Official BuildingBuilder API](https://ifbars.github.io/S1MAPI/api/S1MAPI.Building.BuildingBuilder.html).
- [Official NavigationBuilder API](https://ifbars.github.io/S1MAPI/api/S1MAPI.Building.NavigationBuilder.html).
- Local source anchors: `ThirdParty/S1MAPI/Building/BuildingBuilder.cs` and `ThirdParty/S1MAPI/Building/NavigationBuilder.cs`; paths may move.
- [Furnishings](interior.md), [world tools](world-tools.md), and [persistence](persistence.md).
