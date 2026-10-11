# Diagnostics and room measurement

PropertyLEDs ships no room coordinates. A room becomes visible only after it has been measured in
game, exported as a layout candidate, visually checked, and marked `Verified: true` by hand in
`<UserData>\PropertyLEDs\layouts.json`.

Everything below runs on the main thread, is bounded (node/depth/point budgets), never scans the
whole scene and never touches vanilla geometry, colliders or materials.

## Console command

The `led` command works from the MelonLoader console. Tokens containing spaces must be quoted.

| Command | Purpose |
|---|---|
| `led help` | Command overview (same text as below) |
| `led properties` | List owned properties/businesses with their stable IDs |
| `led rooms <propertyId>` | List known rooms of one property |
| `led status` | Runtime state (slot, readiness, shader, geometry counts, layer budgets) |
| `led dump <propertyId> [anchorPath\|.] [nodes=256,max512] [depth=6,max12]` | Bounded walk of one property's transform hierarchy: node path, local + world position, renderer bounds, truncated flags |
| `led begin <propertyId> <roomId> [anchorPath\|.] [displayName]` | Start a new capture relative to a property-anchored path |
| `led local <x> <y> <z>` | Append an anchor-local point (metres) |
| `led world <x> <y> <z>` | Append a point given in world space (converted at capture time) |
| `led aim [metres=10,max30]` | Append the point the crosshair currently points at |
| `led arm [metres]` | Close the console and capture the next click inside the game world (30 s window, Escape cancels) |
| `led path [open]` | Start a new path (closed by default, `open` for an open run) |
| `led closed <true\|false>` | Change the current path's closure |
| `led undo` | Remove the last captured point |
| `led width <0.005..0.25>` | Beam width in metres for the current capture |
| `led preview` | Show the capture as temporary markers + LED polyline |
| `led hide` | Remove markers/preview (capture is kept) |
| `led clear` | Discard the current capture |
| `led export [safeName]` | Write a layout candidate into `UserData/PropertyLEDs/diagnostics/` |
| `led reload` | Re-read `layouts.json` and rebuild geometry (no recompile) |
| `led refresh` | Re-read the owned property/business list and rebuild |
| `led set <propertyId> <roomId\|.> <enabled\|hue\|saturation\|intensity\|brightness> <value>` | Manual colour control (same code path as the app) |
| `led white <propertyId> <roomId\|.>` | Apply the white preset to a scope |
| `led inherit <propertyId> <roomId>` | Drop a room override so the room follows the property |

## Measurement workflow

1. Stand in the room, open the console and run `led properties` to get the property ID.
2. `led dump <propertyId> . 256 6` to understand the anchor hierarchy, then inspect the written dump
   under `UserData/PropertyLEDs/diagnostics/`.
3. `led begin <propertyId> <roomId> <anchorPath>` — pick the deepest stable path that belongs to the
   room (`. ` = property root). Coordinates are stored relative to that anchor.
4. Collect points with `led local` (measured values) or `led aim` / `led arm` (look at a corner and
   capture). Walk in one direction per edge of the ceiling outline; a rectangle is four points in one
   closed path, an L-shaped room is one six-point closed path.
5. `led preview` and look at the result in the world. Adjust with `led undo`, `led width`, or by
   re-capturing a point.
6. `led export myRoom` writes the candidate (always with `Verified: false`).
7. Move the candidate into `UserData/PropertyLEDs/layouts.json`, check it visually, and only then set
   `"Verified": true`. `led reload` applies it immediately.
8. Repeat for sloped ceilings: the points simply follow the roof geometry — include the slope's
   start and end points in the path.

## Calling the bridge over S1MCP

```text
call_method  object_name="PropertyLEDs_Diagnostics"  component_type="LEDDiagnosticBridge"  method_name="Status"
```

- `component_type` is required: without it the call targets the GameObject and returns a silent
  `null` instead of invoking the component method.
- Installed S1MCP cannot pass a string parameter to an injected IL2CPP method — `Execute` with an
  argument returns `null` without an error. Parameterless methods such as `Status` are reliable.
- Taking a screenshot is not part of the installed S1MCP surface; visual checks are done from inside
  the game (or an external desktop capture).

## Rules the tooling enforces

- A capture is **never** promoted to verified automatically — not by the console, not by the phone
  app, and not by the MCP bridge.
- Exported candidates never overwrite `layouts.json`.
- Unverified geometry is only shown through `led preview` (a temporary, non-persistent object).
- Markers, preview objects, dumps and captures are removed on scene unload and quit.
- No scene-wide searches: dumps are scoped to one property, with node and depth budgets.

## Offline asset cross-check (UnityPy, no game needed)

Live dumps give renderer bounds (boxes), not wall planes. When a strip looks wrong and the game
is closed, the vanilla meshes can be read directly from
`<GameDir>\Schedule I_Data\level1` + `sharedassets{0,1}.assets` (Unity 2022.3; `py` = Python 3.13
already has UnityPy 1.25.4). Proven 2026-10-10 for the motel room:

- `UnityPy.load(path)` takes **one** file (varargs for several: `load(*files)`), never a list.
- Scene units repeat, so key everything by area (player motel room: world x −76…−65, z 80…88);
  bare names (`Cube`, `Interior`) exist a dozen times across the map.
- Link Transform→GameObject via `Transform.m_GameObject.path_id`, never via component order.
- `MeshFilter.m_Mesh` gives `(file_id, path_id)`; `file_id 0` = same file, otherwise resolve through
  the raw external names (`grep -a -o -E "[\w./-]+\.assets" level1`, order = file order).
- `Mesh.m_Vertices` may come back empty while `m_VertexData` is complete: parse
  `m_DataSize` manually (stride 48 here: float32 pos/normal @0/12, tangent @24, uv @40).
- Static combined meshes (`Combined Mesh (root: scene) N`) store **world-space** vertices — do NOT
  apply the node TRS on top, or the room lands kilometres off. Sanity anchor: interior shell
  corners must equal the live dump bounds (motel: x −73.89…−67.79, ceiling 3.515, z 81.66…86.66).
- Orientation trap: the property anchor is yawed 90°, so property-local X runs along world Z.
  Derive the mapping from two known nodes before converting anything.
- Scratch scripts: `asset_tree3.py` (hierarchy + world TRS), `asset_meshscan.py` (mesh names per
  file), `asset_room.py` (room-box vert scan), `asset_ceiling.py` (ceiling-zone dump).

## Phone app

The app is the normal control surface: property/room selection, the four sliders, white preset,
on/off, "Use property color" and a status line that states whether the selected room's layout is
verified. Selecting a scope never changes settings; only deliberate edits do.
