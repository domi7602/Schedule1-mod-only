---
name: schedule1-interiors
description: "Build Schedule I interiors, exits and load lifecycles. Keywords: Interior, Building, StaticDoor, DoorKnocker, NpcSummonMenu, RawArcadeSceneService, ArcadeEntranceService, IArcadeGame, Texture2D, SetPixels32, CRT, PacMan, Pinball, CabinetInteraction, AudioSource, Ambience, TeleportPlayerWithController."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — Interiors and In-World Screens

Apply the [shared working contract](../schedule1-modding/references/working-contract.md).
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

## Transition and focus contract

- Use a verified movement/teleport path for the actual player controller. Do not assume toggling one CharacterController is sufficient in every game build or network role.
- Capture a safe return position and relevant state before entry; maintain a tested exterior fallback.
- Restore prior controller/input/camera state only when still owned by this transition, including exception paths.
- Finish native menu cleanup before giving control to the interior interface.
- On cabinet focus, acquire one owned input lease and route controls to the selected game.
- On exit, scene change, component disable, or player replacement, release the same lease exactly once.
- Do not force global movement enabled if another UI or gameplay system still owns the restriction.

## Screen and audio workflow

1. Keep simulation separate from pixels and Unity resource access.
2. Reuse a bounded pixel buffer; upload only changed/visible content at a measured cadence.
3. Use an appropriate unlit/emissive presentation; skipping mipmap generation does not remove the GPU upload cost.
4. Validate local audio paths and decode results; cancel stale loads and destroy owned clips on replacement.
5. Limit spatial sources and shadow-casting lights; measure performance with the intended number of rooms/cabinets.

## Verify

Test enter/exit, menu cancellation, repeated interactions, load inside, missing entrance/assets,
blocked destination, collision seams, navigation, input restoration, pause/time scale, audio failures,
scene unload, repeated rebuilds, and supported multiplayer roles. A headless geometry or image check
can assist development; final interaction and rendering acceptance requires the running game.

## Read on demand

| Trigger | Reference |
|---|---|
| Door/menu integration and safe return | [Door hooking](references/door-hooking.md) |
| Mesh validation, room coordinates, collision | [Procedural room](references/procedural-room.md) |
| Pixel buffer, upload budget, focus and cleanup | [Minigame screens](references/minigame-screens.md) |

## Evidence and output

Historical ScheduleIArcade/HomelessMod observations are retained as design context, not verified APIs.
Return subsystem responsibilities, source evidence for concrete calls, changed files/assets, failure
recovery, and playtest results. Report unperformed runtime checks explicitly.
