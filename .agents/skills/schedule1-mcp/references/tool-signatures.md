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
  "game_version": "0.4.6f13",
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
