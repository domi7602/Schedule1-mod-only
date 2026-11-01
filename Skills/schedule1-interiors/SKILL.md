---
name: schedule1-interiors
description: "Build Schedule I interiors, exits and load lifecycles."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Schedule I — Interiors and In-World Screens

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Separate entrance routing, room ownership, input focus, rendering, and audio lifetimes.

## Establish prerequisites

1. Inspect the target door/menu APIs, local-player movement controller, camera/input ownership, and runtime branch.
2. Decide whether the room is local presentation, a shared world space, or a native registered property.
3. Specify entrance, safe interior spawn, verified exit destination, and behavior when loading already inside.
4. Select procedural geometry, imported mesh, or bundle based on required assets; inspect the actual loader.
5. Define resource limits for mesh counts, textures, audio, and per-frame work.

## Build the interior

1. Resolve and validate the intended entrance by stable identity or verified hierarchy, not a generic object name.
2. Add an idempotent custom menu choice using supported event adapters; remove inherited listeners only on owned clones.
3. Build one inactive room root with local-space geometry, explicit wall/floor collision, and usable openings.
4. Validate all external mesh counts/indices and bounds before allocating Unity resources.
5. Add lighting/materials with deliberate ownership and target-pipeline compatibility.
6. Prepare navigation separately when NPCs must enter; test both directions through each doorway.
7. Activate only completed phases; remove partial owned resources if construction fails.

### Facade openings: mesh evidence before renderer changes

Inspect the exact renderer-owned triangles and LOD membership, not collider bounds or a
door-sized name/bounds heuristic. The f12 Town Hall entrance is covered by three independent
contributions: `townhall/Walls` (mesh41/sub84), root `Cube` (mesh125/sub127), and distinct LOD
`townhall/Cube` (mesh95/sub84..86). Hiding Door/Glass/Quad does not remove those wall faces.
Do not infer that `Renderer.enabled=false` fails for static batches from such a result.
The trim colliders span the footprint, but their visual meshes are perimeter rings, not floor caps.

For unreadable native meshes, extract only the attributed contributors offline from hash-pinned
assets. Preserve UV0/UV1, normals, tangents, material-slot order and world-coordinate conventions;
clip crossing polygons rather than deleting whole triangles by midpoint or bounding box.
Build owned replacements inactive and preserve LOD membership. Never alter global combined
mesh buffers or borrowed materials; journal exact renderer/LOD references for restoration.
The current local extraction embeds proprietary game geometry and is not for redistribution.
See `Source/Mods/TownHallInterior/docs/facade-portal-verification.md` for the bounded evidence.

Check the complete generated collision shell, including render-hidden perimeter lintels and
slabs, against the actual owned floor geometry. Do not promote player/camera/teleport Y to
a measured ground height. Share the opening height between facade, perimeter and inset skin;
sweep finite capsules through all generated fixed boxes in both directions. These offline
tests do not establish native controller stepping or in-game traversal.

## Transition and focus contract

- Use a verified movement/teleport path for the actual player controller. Do not assume toggling one CharacterController is sufficient in every game build or network role.
- Capture a safe return position and relevant state before entry; maintain a tested exterior fallback.
- Restore prior controller/input/camera state only when still owned by this transition, including exception paths.
- Finish native menu cleanup before giving control to the interior interface.
- On cabinet focus, acquire one owned input lease and route controls to the selected game.
- On exit, scene change, component disable, or player replacement, release the same lease exactly once.
- Do not force global movement enabled if another UI or gameplay system still owns the restriction.

## Verify

Select relevant checks: enter/exit, menu cancellation, repeated interactions, load inside, missing entrance/assets,
blocked destination, collision seams, navigation, input restoration, pause/time scale, audio failures,
scene unload and rebuilds. Multiplayer checks require explicit scope; headless geometry/image tests
do not establish in-game interaction or rendering acceptance.

## Read on demand

| Trigger | Reference |
|---|---|
| Door/menu integration and safe return | [Door hooking](references/door-hooking.md) |
| Mesh validation, room coordinates, collision and lighting budget | [Procedural room](references/procedural-room.md) |
| Screens/cabinets: simulation, pixel/upload budget, focus, audio and cleanup | [Minigame screens](references/minigame-screens.md) |

## Evidence and output

ScheduleIArcade/HomelessMod observations remain historical design context. Report subsystem
responsibilities and recovery under the shared evidence policy.
