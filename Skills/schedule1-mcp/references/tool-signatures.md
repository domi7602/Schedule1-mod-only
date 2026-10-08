# S1MCP Tool Signatures & JSON-RPC Reference
> verified: tool schemas extracted from S1MCPServer source 2026-10-05. Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

Source of truth: `ThirdParty/S1MCPServer-master/` (C# mod: `S1MCPServer/`, Python MCP client: `S1MCPClient/`). Every schema below is backed by the named source file. Fields that exist only as stubs/TODOs in the source are marked. Nothing in this file is guessed.

The SKILL.md §3 table lists a **15-tool subset**. The Python client actually registers more tools (e.g. `s1_get_npc_position`, `s1_inspect_component`, `s1_find_gameobjects`, `s1_launch_game`). This file documents the 15 listed there; all 14 that exist in source are below, plus one entry documenting a name mismatch (`s1_list_buildings`).

---

## 1. JSON-RPC Envelope (Wire Protocol)

**Transport:** TCP socket. Server binds `127.0.0.1:8765` (`IPAddress.Loopback`, port fixed in `MainMod.cs` → `new TcpServer(..., port: 8765)`; client default `localhost:8765` in `S1MCPClient/src/tcp_client.py` and `src/utils/config.py`).

**Framing** (`S1MCPClient/src/protocol.py`, `src/tcp_client.py`): 4-byte **little-endian unsigned int32** length prefix (`struct.pack('<I', len)`) followed by a UTF-8 JSON payload. The client rejects frames larger than 10 MB (`message_length > 10 * 1024 * 1024`).

**Request envelope** — serialized by `protocol.py: serialize_request()` and parsed by the C# `Models/Request.cs` (`id`, `method`, `params` only):

```json
{"id": 1, "method": "get_player", "params": {}}
```

**Correction vs. older revision of this file:** the wire format is JSON-RPC-2.0-*style* (id/method/params, standard error codes) but the `"jsonrpc": "2.0"` member is **not** sent. Neither the C# request/response models nor the Python serializer contains it. Method names on the wire are the MCP tool names **without** the `s1_` prefix (mapping table in §2).

**Response envelope** — C# `Models/Response.cs` + `Models/ErrorResponse.cs` (`result`/`error` are omitted when null via `JsonIgnoreCondition.WhenWritingNull`):

```json
{"id": 1, "result": {"success": true}}
{"id": 2, "error": {"code": -32602, "message": "item_id parameter is required", "data": null}}
```

**Acknowledgment step** (`TcpServer.HandleClient` + `tcp_client.call()`): after every response the client sends `{"id": <request id>, "status": "received"}`; the server waits for this ack before reading the next request.

**Heartbeats:**
- Client → server: `heartbeat` request every 60 s (`tcp_client._heartbeat_loop`).
- Server → client: server-initiated response every 60 s with its own ID counter, payload `{"type": "server_heartbeat", "status": "alive", "timestamp": <unix ms>}` (`TcpServer.HeartbeatLoop`). The client detects it via `result.type == "server_heartbeat"` and keeps waiting for the actual response.

**Error codes** (observed in handlers; `ProtocolHandler.CreateErrorResponse`, `ValidationHelper`):

| Code | Meaning | Example |
|---|---|---|
| `-32700` | Parse error | invalid JSON payload |
| `-32600` | Invalid request | empty `method` |
| `-32601` | Method not found | unknown method; `data.availableMethods` lists valid ones |
| `-32602` | Invalid params | missing/incorrect `position`, mutually exclusive `capture_logs` params, invalid regex |
| `-32603` | Internal error | handler exception (`data.details`) |
| `-32000` | Game error | `Player not ready`, `NPC not found`, `Item not found`, `GameObject not found` |
| `-32002` | Validation error | `data.reason`, e.g. `"Position is too far underground"`, `"NPC '<id>' not found"` |

**Execution model** (`MainMod.cs`, `Core/CommandQueue.cs`, `Core/ResponseQueue.cs`): the TCP background thread enqueues requests into `CommandQueue`; the mod's `OnUpdate` (main thread) dequeues and routes them; responses flow back through `ResponseQueue` and a background `ResponseLoop`. All Unity/IL2CPP access happens on the main thread.

**Single-client rule** (`TcpServer.ServerLoop`): if a client is already connected, a second incoming connection is closed immediately (`"Rejecting new client connection ... - already have a connected client"`).

---

## 2. Tool Schemas (15)

Each block: MCP tool name → wire `method`, params (from the Python `inputSchema` in `S1MCPClient/src/tools/*.py`, cross-checked against C# param handling in `S1MCPServer/Handlers/*.cs`), response structure, error cases.

### `s1_get_game_state` → `get_game_state`
Source: `src/tools/game_state_tools.py`, `Handlers/GameStateCommandHandler.cs`.
**Params:** none.
**Response:**
```json
{
  "scene_name": "Main",
  "game_time": 0,
  "network_status": "singleplayer",
  "loaded_mods": ["S1MCPServer", "..."]
}
```
- `scene_name`: `SceneManager.GetActiveScene().name`.
- `game_time`: **stub, always `0`** (`// TODO: Get actual game time`).
- `network_status`: `host` | `singleplayer` | `client` | `server` | `unknown` (FishNet `InstanceFinder.NetworkManager`).
- `loaded_mods`: `MelonMod.RegisteredMelons` names.
- **Outdated fields removed from this doc:** the previous revision showed `build_index`, `is_multiplayer`, `is_host`, `game_version` — none of these exist in the current source (`game_version` is commented out as TODO).
**Errors:** `-32000` "Failed to get game state" (`data.details`).

### `s1_capture_logs` → `capture_logs`
Source: `src/tools/log_tools.py`, `Handlers/LogCommandHandler.cs`. Reads `MelonEnvironment.MelonLoaderDirectory/Latest.log` with `FileShare.ReadWrite`. If the file is > 256 KB and neither `first_n_lines` nor `from_timestamp` is set, only the **last 256 KB** are read (tail seek).
**Params:**

| Param | Type | Default | Notes |
|---|---|---|---|
| `last_n_lines` | integer | — | min 1; mutually exclusive with `first_n_lines` |
| `first_n_lines` | integer | — | min 1; mutually exclusive with `last_n_lines` |
| `keyword` | string | — | case-insensitive substring |
| `from_timestamp` | string | — | `HH:mm:ss` or `HH:mm:ss.fff` (`TimeSpan.TryParse`) |
| `to_timestamp` | string | — | same format |
| `include_pattern` | string | — | case-insensitive regex |
| `exclude_pattern` | string | — | case-insensitive regex |

Filter order: timestamp → keyword → regex → line count. Lines without a parseable timestamp are dropped during timestamp filtering. **Outdated params removed:** `max_lines`, `log_level`, `start_time` from the old SKILL.md table do not exist.
**Response:**
```json
{
  "lines": [{"line_number": 120, "timestamp": "[12:30:45.123]", "content": "..."}],
  "total_lines_in_file": 4821,
  "filtered_count": 37,
  "filters_applied": ["keyword: error"],
  "warning": "Log file not found"
}
```
- `timestamp` is the extracted `[HH:mm:ss.fff]` prefix (empty string if none). `warning` only appears when `Latest.log` does not exist (then `lines` is empty, success response).
**Errors:** `-32602` "Cannot specify both last_n_lines and first_n_lines"; `-32602` "Invalid include_pattern regex" / "Invalid exclude_pattern regex" (`data.details`); `-32000` "Failed to read log file" / "Failed to capture logs" (with `data.stack_trace`).

### `s1_get_player` → `get_player`
Source: `src/tools/player_tools.py`, `Handlers/PlayerCommandHandler.cs (HandleGetPlayer)`.
**Params:** none.
**Response:**
```json
{
  "position": {"x": 124.5, "y": 2.1, "z": -45.0},
  "rotation": {"x": 0.0, "y": 180.0, "z": 0.0},
  "health": 100.0,
  "money": {"cash": 4500.0, "bank": 12000.0, "networth": 30000.0},
  "network_status": "singleplayer"
}
```
- `rotation`: `transform.eulerAngles` (degrees).
- `money.cash` = `MoneyManager.cashBalance`, `money.bank` = `MoneyManager.onlineBalance`, `money.networth` = `MoneyManager.LastCalculatedNetworth`.
**Errors:** `-32000` "Player not ready" (no `Player.Local`); `-32000` "Failed to get player information" (`data.details`).

### `s1_get_player_inventory` → `get_player_inventory`
Source: `src/tools/player_tools.py`, `Handlers/PlayerCommandHandler.cs (HandleGetPlayerInventory)`.
**Params:** none.
**Response:**
```json
{"items": [ {"...ItemInstance serialization..."} ], "count": 2}
```
- Reads only `PlayerInventory.Instance.hotbarSlots`, collecting slots with a non-null `ItemInstance`. Items are the raw serialized `ItemInstance` objects — **the exact per-item fields are `unverified`** (the C# source does not enumerate them; it hands the IL2CPP objects to the serializer).
**Errors:** `-32000` "Player not ready"; `-32000` "Failed to get player inventory" (`data.details`).

### `s1_teleport_player` → `teleport_player`
Source: `src/tools/player_tools.py`, `Handlers/PlayerCommandHandler.cs (HandleTeleportPlayer)`, `Core/ValidationHelper.cs`.
**Params:**

| Param | Type | Required | Notes |
|---|---|---|---|
| `position` | object `{x, y, z}` numbers | yes | all three required by schema; C# defaults missing keys to `0.0` |

**Response:**
```json
{"success": true, "new_position": {"x": 10.0, "y": 2.0, "z": 20.0}}
```
- Sets `player.transform.position` directly.
**Validation (`ValidationHelper.ValidatePosition`, error code `-32002`, `data.reason`):**
- `y < -10` → "Position is too far underground"
- `y > 1000` → "Position is too high"
- `|x| > 10000` or `|z| > 10000` → "Position is outside world bounds"
**Errors:** `-32602` "position parameter is required" / "Invalid position format" / "Failed to parse position"; `-32002`; `-32000` "Player not ready" / "Failed to teleport player".

### `s1_add_item_to_player` → `add_item_to_player`
Source: `src/tools/player_tools.py`, `Handlers/PlayerCommandHandler.cs (HandleAddItemToPlayer)`.
**Params:**

| Param | Type | Default | Notes |
|---|---|---|---|
| `item_id` | string | — | required; validated against the item registry |
| `quantity` | number | 1 | clamped to ≥ 1; unparsable → default 1 |

**Response:**
```json
{"success": true, "item_id": "weed_baggy", "quantity_added": 5, "new_total": 5}
```
- **Stub warning:** the handler body is `// TODO: Implement item addition using native game classes` — it returns success **without actually modifying the inventory**. `new_total` is just the passed quantity. Do not rely on this tool to change game state.
- **Outdated param removed:** `quality` (SKILL.md §3) does not exist in the source.
**Errors:** `-32602` "item_id parameter is required"; `-32002` "Item '<id>' not found in registry" (`data.reason`); `-32000` "Player not ready" / "Failed to add item to player".

### `s1_list_npcs` → `list_npcs`
Source: `src/tools/npc_tools.py`, `Handlers/NPCCommandHandler.cs (HandleListNPCs)`. Iterates `NPCManager.NPCRegistry`.
**Params:**

| Param | Type | Default | Notes |
|---|---|---|---|
| `filter` | string enum | — | schema enum: `conscious`, `unconscious`, `in_building`, `in_vehicle`. C# only implements `conscious`/`unconscious`; other values match nothing and **include all** NPCs |

**Response:**
```json
{
  "npcs": [
    {"npc_id": "kyle_cooley", "name": "Kyle Cooley", "position": {"x": 1.0, "y": 0.0, "z": 2.0},
     "health": 100.0, "is_conscious": true, "game_object_name": "Kyle Cooley"}
  ],
  "count": 1
}
```
- Per-NPC fields (`name`, `position`, `health`, `is_conscious`, `game_object_name`) are extracted via reflection on common property names and are **optional** — an NPC entry may omit any of them if reflection fails. `game_object_name` is included specifically so it can be fed into `s1_inspect_object` / `s1_inspect_component`.
**Errors:** `-32000` "Failed to list NPCs" (`data.details`).

### `s1_get_npc` → `get_npc`
Source: `src/tools/npc_tools.py`, `Handlers/NPCCommandHandler.cs (HandleGetNPC)`.
**Params:** `npc_id` (string, required). Lookup is case-insensitive over `NPCRegistry`, resolving the NPC's ID via reflection on properties `ID`, `Id`, `NPCID`, `NpcId`, `Identifier`, `Name` (fallback: `ToString()`).
**Response:** same shape as one `s1_list_npcs` entry (see above).
**Errors:** `-32602` "npc_id parameter is required"; `-32002` "NPC '<id>' not found" (`ValidationHelper.ValidateNPCID` → `NPCManager.GetNPC`); `-32000` "NPC not found" (`data.npc_id`) / "Failed to extract NPC data" / "Failed to get NPC".

### `s1_teleport_npc` → `teleport_npc`
Source: `src/tools/npc_tools.py`, `Handlers/NPCCommandHandler.cs (HandleTeleportNPC)`.
**Params:** `npc_id` (string, required), `position` (`{x, y, z}`, required; same parsing/validation as `s1_teleport_player`).
**Response:**
```json
{"success": true, "npc_id": "kyle_cooley", "new_position": {"x": 1.0, "y": 0.0, "z": 2.0}}
```
- **Stub warning:** handler body is `// TODO: Implement NPC teleportation` — returns success without moving the NPC.
**Errors:** `-32602` "npc_id parameter is required" / "position parameter is required" / "Invalid position format"; `-32002` (NPC not found / position bounds); `-32000` "Failed to teleport NPC".

### `s1_set_npc_health` → `set_npc_health`
Source: `src/tools/npc_tools.py`, `Handlers/NPCCommandHandler.cs (HandleSetNPCHealth)`.
**Params:** `npc_id` (string, required), `health` (number, required; must be 0–1000).
**Response:**
```json
{"success": true, "npc_id": "kyle_cooley", "old_health": 100.0, "new_health": 0.0}
```
- **Stub warning:** handler body is `// TODO: Implement NPC health modification`; `old_health` is a hard-coded `100.0f`. The health value is **not** applied to the game.
**Errors:** `-32602` "npc_id parameter is required" / "health parameter is required" / "Invalid health value"; `-32002` "Health must be between 0 and 1000" or "NPC '<id>' not found"; `-32000` "Failed to set NPC health".

### `s1_list_items` → `list_items`
Source: `src/tools/item_tools.py`, `Handlers/ItemCommandHandler.cs (HandleListItems)`. Enumerates `Helpers.Utils.GetAllStorableItemDefinitions()`.
**Params:** `category` (string, optional — accepted but **not applied**; C# TODO: "include all items").
**Response:**
```json
{
  "items": [
    {"item_id": "weed_baggy", "name": "Bag of Weed", "description": "...", "category": "Unknown",
     "base_price": 0.0, "stack_limit": 5}
  ],
  "count": 1
}
```
- `category` and `base_price` are **stubs** (`"Unknown"` / `0.0` — TODO in source). `item_id`, `name`, `description`, `stack_limit` are real (`StorableItemDefinition.ID/Name/Description/StackLimit`).
**Errors:** `-32000` "Failed to list items" (`data.details`).

### `s1_get_item` → `get_item`
Source: `src/tools/item_tools.py`, `Handlers/ItemCommandHandler.cs (HandleGetItem)`.
**Params:** `item_id` (string, required).
**Response:**
```json
{"item_id": "weed_baggy", "name": "Bag of Weed", "description": "...", "category": "Unknown",
 "base_price": 0.0, "stack_limit": 5, "legal_status": "unknown"}
```
- `category`, `base_price`, `legal_status` are stubs (TODO in source).
**Errors:** `-32602` "item_id parameter is required"; `-32000` "Item not found" (`data.item_id`) / "Failed to get item".

### `s1_spawn_item` → `spawn_item`
Source: `src/tools/item_tools.py`, `Handlers/ItemCommandHandler.cs (HandleSpawnItem)`.
**Params:**

| Param | Type | Default | Notes |
|---|---|---|---|
| `item_id` | string | — | required; registry-validated |
| `position` | object `{x, y, z}` | — | required; same bounds validation as teleport |
| `quantity` | number | 1 | only sent by client when ≠ 1; C# clamps < 1 → 1 |

**Response:**
```json
{"success": true, "item_id": "weed_baggy", "quantity": 1, "position": {"x": 1.0, "y": 2.0, "z": 3.0}}
```
- **Stub warning:** handler body is `// TODO: Implement item spawning using native game classes` — returns success **without spawning anything** in the world.
**Errors:** `-32602` "item_id parameter is required" / "position parameter is required" / "Invalid position format"; `-32002` (item registry / position bounds); `-32000` "Failed to spawn item".

### `s1_list_buildings` — **not present in source** (`unverified` / renamed)
There is **no** `s1_list_buildings` tool and no `list_buildings` method in this source revision (`grep` over `S1MCPClient/src` and `S1MCPServer` returns nothing). The SKILL.md §3 row is stale. The building/property equivalent that *does* exist:
- **`s1_list_properties`** → `list_properties` (`src/tools/property_tools.py`; `Handlers/PropertyCommandHandler.cs`). Params: none. Response: `{"properties": [], "count": 0}` — **stub** (`// TODO: Implement property enumeration`, reference pattern `S1API Property.GetAll()`).
- **`s1_get_property`** → `get_property`. Params: `property_id` **or** `property_name` (string, at least one required). Response: `{"property_id": "...", "name": "...", "position": {"x":0.0,"y":0.0,"z":0.0}, "npcs_inside": [], "npc_count": 0}` — **stub** (TODO; reference pattern `S1API Property.GetByName()`).
**Errors (get_property):** `-32602` "property_id or property_name parameter is required"; `-32000` "Failed to get property".

### `s1_inspect_object` → `inspect_object`
Source: `src/tools/debug_tools.py (handle_s1_inspect_object)`, `Handlers/Debug/DebugObjectInspectionHandler.cs`. GameObject lookup: `ReflectionHelper.FindGameObject(name)` — `GameObject.Find(name)` for active objects, otherwise an active-scene root traversal comparing `obj.name == name` (exact match; inactive objects only when `includeDisabled`, which the tool does not expose).
**Params:**

| Param | Type | Default | Notes |
|---|---|---|---|
| `object_name` | string | — | required; exact GameObject name |
| `object_type` | string | `"GameObject"` | accepted by the client schema but **ignored** by the C# handler — it always inspects as GameObject |

**Outdated params removed:** `game_object_name`, `component_name` (old SKILL.md table) do not exist in the source; component-level reflection is a separate tool (`s1_inspect_component`).
**Response:**
```json
{
  "object_name": "Kyle Cooley",
  "object_type": "GameObject",
  "full_type": "UnityEngine.GameObject",
  "position": {"x": 1.0, "y": 0.0, "z": 2.0},
  "rotation": {"x": 0.0, "y": 0.0, "z": 0.0, "w": 1.0},
  "scale": {"x": 1.0, "y": 1.0, "z": 1.0},
  "components": [
    {"type": "Transform", "full_type": "UnityEngine.Transform",
     "properties": {"position": "..."}, "fields": {}}
  ],
  "component_count": 1,
  "component_types": ["Transform"],
  "children": [{"name": "Model", "path": "Kyle Cooley/Model"}],
  "child_count": 1
}
```
- Per component: up to **20 public properties** and **20 public fields** (reflection, `Take(20)`); backing fields (`<Name>k__BackingField`) and `IntPtr`-typed values are filtered out. A component that throws during inspection is returned as `{"type": ..., "full_type": ..., "error": "<message>"}`.
**Errors:** `-32602` "object_name parameter is required"; `-32000` "GameObject not found" (`data.object_name`) / "Failed to inspect object" (`data.details`).

---

## 3. Bridge Troubleshooting

### 3.1 Connection refused / "Game is not connected"
- **Game not running or mod not loaded:** `TcpConnectionError` ("Failed to connect to TCP server: ...") after 3 `call_with_retry` attempts (1 s apart); the MCP tool surfaces it as `"Error: Connection failed - ... Please ensure the game is running with the mod loaded."`
- **Client-side connection guard** (`src/main.py: can_call_tool`): every tool except the lifecycle set (`s1_launch_game`, `s1_close_game`, `s1_get_game_process_info`, `s1_search_s1api_docs`) is refused until the startup `handshake` succeeded: `"Error: Game is not connected. Please launch the game first using s1_launch_game."`
- **Scene-gate window:** the TCP listener is started in `OnInitializeMelon`, i.e. *before* the Main scene loads (`MainMod.cs` logs `"S1MCPServer ready - waiting for Main scene"`). In that window a connection + handshake can succeed while game data is not ready — data tools then fail with `-32000`, e.g. `s1_get_player` → `"Player not ready"` until `Player.Local` exists. Source marker that the game is usable: `"Main scene loaded - S1MCPServer is active"`. (Note: this source revision contains no explicit per-command scene gate beyond these errors; the stricter gate wording in SKILL.md §4.1 is `unverified` against the code.)

### 3.2 Port 8765 occupied (stale game process)
- If a previous `Schedule I.exe` is still alive, the new instance cannot bind: `TcpListener.Start()` throws, `TcpServer.ServerLoop` logs `"TCP server error: ..."` and retries the bind **every 1 s** (so Latest.log fills with bind errors). Meanwhile a client may successfully connect to the **old** process and read stale state.
- Fix: kill the stale `Schedule I.exe` before relaunching (check `netstat -ano | findstr 8765`).
- Also remember the single-client rule: while one MCP client holds the connection, a second one is silently closed by the server (`"Rejecting new client connection ..."`).

### 3.3 Timeouts on large inspect queries
- Client timing (`tcp_client.py`): connect timeout 5 s; after connect the **read timeout is 90 s** (covers the 60 s heartbeat plus buffer); `call_with_retry` makes up to **3 attempts** with 1 s reconnect delay.
- Server timing (`TcpServer.HandleClient`): after enqueueing a command it waits max **10 s** (200 × 50 ms) for the response to be sent, then logs a warning and continues — the response may still arrive later.
- Cost drivers: `s1_inspect_object` reflects up to 40 members per component on every component of the target; scene-wide debug queries (`s1_find_gameobjects`, up to 1000 results) run on the main thread and can stall the frame. Serialization has `MaxDepth = 32` (`ProtocolHandler.JsonOptions`) — deeply nested object graphs can fail.
- Mitigation (source-supported): prefer targeted tools (`s1_get_npc` → `game_object_name` → `s1_inspect_component` with `max_depth`), use `keyword`/`last_n_lines` on `s1_capture_logs` (the handler already tails only the last 256 KB of large logs unless `first_n_lines`/`from_timestamp` forces a full read).

### 3.4 DebugLogging config
- `MainMod.cs` registers MelonPreferences category `S1MCPServer`, file **`UserData/S1MCPServer.cfg`** (relative to the game install), entry `DebugLogging` (bool, **default `false`**, description "Enable verbose [DEBUG] log output (includes truncated payload dumps)").
- When enabled, `ModLogger.Debug` / `ModLogger.DebugPayload` write `[DEBUG]` lines including raw request/response JSON, **truncated to 500 chars** (`ModLogger.MaxPayloadLogChars = 500`). Info/Warn lifecycle lines (TCP server start, client connect/disconnect, Main scene loaded) are always logged.
- Prefs are read once in `OnInitializeMelon` (`MelonPreferences.Load()`) — changing the cfg requires a game restart.
- Use it when diagnosing the bridge itself (payload dumps show the exact wire JSON); turn it off afterwards to avoid Latest.log spam.

---

## 4. Client Setup (S1MCPClient)

- **Location / working directory:** `ThirdParty/S1MCPServer-master/S1MCPClient/` — run the server **from this directory** (`src` package imports depend on the cwd).
- **Dependencies:** Python 3.10+ (`pip install -r requirements.txt`; MCP SDK `mcp>=0.9.0` per README).
- **Launch:** `python -m src.main` (or the console script `s1mcpclient` if pip-installed — `pyproject.toml` maps `s1mcpclient = "src.main:main"`).
- **Config:** `config.json` in the S1MCPClient root (`src/utils/config.py: Config.from_file`). Only `game_config.json.example` is committed — without `config.json` the code defaults apply:

| Key | Default | Purpose |
|---|---|---|
| `host` / `port` | `localhost` / `8765` | TCP bridge endpoint (must match the mod's fixed 8765) |
| `log_level` | `DEBUG` | client log verbosity |
| `connection_timeout` | `5.0` | TCP connect timeout (s) |
| `reconnect_delay` | `1.0` | delay between retry attempts (s) |
| `game_il2cpp_path` | null | game dir for lifecycle tools (IL2CPP marker: `MelonLoader/Il2CppAssemblies`) |
| `game_mono_path` | null | game dir for lifecycle tools (Mono) |
| `game_executable` | `"Schedule I.exe"` | process name used by lifecycle tools |
| `game_startup_timeout` | `60.0` | wait for game start (s) |
| `game_connection_poll_interval` | `2.0` | connection polling interval (s) |

- **MCP client registration** (README pattern, e.g. Claude Desktop):
```json
{
  "mcpServers": {
    "s1mcpclient": {
      "command": "python",
      "args": ["-m", "src.main"],
      "cwd": "C:\\path\\to\\ThirdParty\\S1MCPServer-master\\S1MCPClient"
    }
  }
}
```
- **Startup behavior** (`src/main.py`): optional initial `tcp_client.connect()` + `handshake` (records `available_methods`); game tools stay gated on `is_connected` until handshake succeeds. A PID file (`%TEMP%/s1mcpclient.pid`) warns when another instance is running.
