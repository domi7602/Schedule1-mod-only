# S1MCP Tool Signatures & JSON-RPC Reference

This reference documents the detailed request/response schemas for the S1MCP bridge.

---

## 1. Protocol Architecture
- **Transport:** TCP Socket (`127.0.0.1:8765`)
- **Framing:** 4-byte Little-Endian integer specifying payload length, followed by UTF-8 encoded JSON.
- **Specification:** JSON-RPC 2.0 (`{"jsonrpc": "2.0", "id": 1, "method": "...", "params": {...}}`)

---

## 2. Core Tool Details

### `s1_get_game_state`
Returns engine status, active scene, multiplayer state, and loaded mods.
```json
{
  "scene_name": "Main",
  "build_index": 1,
  "is_multiplayer": false,
  "is_host": true,
  "game_version": "0.4.7f6",
  "active_mods": ["NotesApp", "PotScanner", "CustomSkateboard", "S1MCPServer", "Shared"]
}
```

### `s1_capture_logs`
Filters in-memory MelonLoader log entries without needing to read locked file handles.
- **Parameters:**
  - `keyword` (string, optional): Search string (e.g. `"error"`, `"NullReferenceException"`).
  - `max_lines` (int, default 50): Number of lines to return.
  - `log_level` (string, optional): `"INFO"`, `"WARN"`, `"ERROR"`.
- **Response:** Array of timestamped log strings.

### `s1_get_player` & `s1_get_player_inventory`
Retrieves live avatar coordinates, health, cash on hand, and slot-by-slot inventory contents.
```json
{
  "position": {"x": 124.5, "y": 2.1, "z": -45.0},
  "rotation": 180.0,
  "cash": 4500,
  "health": 100.0,
  "inventory": [
    {"slot_index": 0, "item_id": "custom_skateboard", "quantity": 1},
    {"slot_index": 1, "item_id": "weed_baggy", "quantity": 20, "quality": 0.95}
  ]
}
```

### `s1_inspect_object`
Performs reflection on any active GameObject.
- **Parameters:**
  - `game_object_name` (string): Exact name or path of GameObject.
  - `component_name` (string, optional): Specific component to inspect.
- **Response:** Component properties, field values, child names, and world transform.

---

## 3. Wire-protocol quick facts (verified 2026-10-03 against v1.0.1)

- Framing: TCP `127.0.0.1:8765`, 4-byte LITTLE-endian length prefix + UTF-8 JSON.
  Request shape `{"id": 1, "method": "<name>", "params": {...}}` - no `jsonrpc`
  envelope field is needed (unknown fields are ignored).
- Method names on the wire have NO `s1_` prefix: `get_game_state`,
  `capture_logs`, `inspect_object`, `find_gameobjects`, `inspect_ui_image`,
  `read_sprite_pixels`, ... (48 handlers). The `s1_*` names belong to the
  Python MCP client's tool surface only. `handshake` lists all methods.
- Use ONE request per connection and retry with a fresh connection on a
  timeout: pipelined requests and rapid reconnects can drop responses.

## 4. UI / theming tools (added 2026-10-03)

### `inspect_ui_image`
Render-relevant Image state for dark-mode / theming triage. Resolves targets by
name or full path as case-insensitive substrings, inactive objects INCLUDED
(bounded scene traversal, never Resources.FindObjectsOfTypeAll).
- **Parameters:**
  - `object_name` (string, required): name or path substring.
  - `max_results` (int, default 10), `max_depth` (int, default 20),
    `include_children` (bool, default true).
- **Response per match:** full `path`, active flags, and per Image:
  `field_m_Sprite` / `field_m_OverrideSprite` (the true slots) vs
  `prop_sprite` / `prop_overrideSprite_active` (Unity: the overrideSprite
  GETTER returns the ACTIVE sprite and falls back to the plain one),
  `color` vs `canvas_renderer.color` (a dark crColor with white color means a
  Selectable state multiply), type/fill/raycast flags, sprite rect/pivot/
  border/packing + texture info, rect, and the nearest Selectable
  (transition, normal_color, target_is_this_image).

### `read_sprite_pixels`
Samples actual sprite texels through a RenderTexture copy (works for
non-readable and atlas textures) - decides "is the art dark or is it a tint"
without screenshots.
- **Parameters:**
  - `object_name` (string, required), `which` (active | plain | override,
    default active), `samples` (list of `[u, v]` normalized to the sprite's
    textureRect; default 7 points across the middle), `max_depth`.
- **Response:** texture size/name, texture_rect, and per sample: u/v, pixel
  x/y, `rgba_hex`, `rgba`, `max_channel`. Refuses textures above 4096 px.

### `capture_logs` (extended)
- **New parameter:** `source` (`file` | `ring` | `both`, default `file`).
  `ring`/`both` include the in-memory capture buffer, which holds ALL
  MelonLoader log lines including `[DEBUG]` (those never reach Latest.log).
- `find_gameobjects` was also fixed (2026-10-03): `pattern` works as an alias
  of `name_pattern`, new `path_pattern`, `max_results`, `max_depth` (default
  20), and every result now carries its full hierarchy `path` (previously
  filters silently missed anything deeper than level 10 / past 2000 objects).
