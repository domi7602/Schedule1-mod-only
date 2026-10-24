# S1MCP Historical Signatures and Transport Notes

> verified: tool schemas extracted from S1MCPServer source 2026-10-05.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 108 of 110 identifier-shaped tokens resolve (2 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

> Evidence scope: this file preserves interface/handler observations from the uploaded
> workspace notes, which attributed them to local source inspections on 2026-10-05 and
> later live observations. The local source, deployed DLL, Python client, and traces were
> not provided in this package. None of these schemas was live-discovered or tested here.

## 1. Use the live schema first

Discover available tools and their actual input schemas through the installed connector.
Record bridge/build identity, target process, and network/session state.
Use this reference to recognize drift, stubs, and transport failures, not to manufacture a
missing tool or invoke a historical schema against an unidentified bridge.
A public upstream repository is not proof of the local fork's implementation.

The inherited source paths were `ThirdParty/S1MCPServer-master/S1MCPServer/` for the mod
and `ThirdParty/S1MCPServer-master/S1MCPClient/` for the Python bridge. Verify those paths
and their revision if they exist in the actual workspace.

## 2. Historical tool status and parameters

All rows describe the inherited revision only. A current connector may differ.

| Historical tool | Wire method | Parameters described in the notes | Important limit |
|---|---|---|---|
| `s1_get_game_state` | `get_game_state` | None | `game_time` was a stub |
| `s1_capture_logs` | `capture_logs` | Line, keyword, timestamp, regex filters below | Old `max_lines`/`log_level`/`start_time` names were absent |
| `s1_get_player` | `get_player` | None | Player must be ready |
| `s1_get_player_inventory` | `get_player_inventory` | None | Hotbar only; per-item serialized fields unverified |
| `s1_teleport_player` | `teleport_player` | Required `position` object with numeric `x/y/z` | Notes report direct transform assignment; gameplay/network acceptance unverified |
| `s1_add_item_to_player` | `add_item_to_player` | Required `item_id`; optional `quantity` | Non-mutating TODO stub; `quality` absent |
| `s1_list_npcs` | `list_npcs` | Optional `filter` | Several filter values were accepted but not implemented |
| `s1_get_npc` | `get_npc` | Required `npc_id` | Reflected fields could be missing |
| `s1_teleport_npc` | `teleport_npc` | Required `npc_id` and `position` | Non-mutating TODO stub |
| `s1_set_npc_health` | `set_npc_health` | Required `npc_id` and `health` | Non-mutating TODO stub |
| `s1_list_items` | `list_items` | Optional `category` | Category filter and some result fields were placeholders |
| `s1_get_item` | `get_item` | Required `item_id` | Price/category/legal-status fields were placeholders |
| `s1_spawn_item` | `spawn_item` | Required `item_id` and `position`; optional `quantity` | Non-mutating TODO stub |
| `s1_inspect_object` | `inspect_object` | Required `object_name`; optional `object_type` | `object_type` ignored by described handler |
| `s1_list_buildings` | — | — | Not found in the described client/server source |
| `s1_list_properties` | `list_properties` | None | Empty-list TODO stub |
| `s1_get_property` | `get_property` | `property_id` or `property_name` | Placeholder TODO response |

A success response from a listed stub is not evidence of a state change. Confirm the actual
handler implementation and independent readback before using a tool for a test mutation.

## 3. Log filtering details

Historical source: `src/tools/log_tools.py` and `Handlers/LogCommandHandler.cs`.

| Parameter | Described type/constraint |
|---|---|
| `last_n_lines` | Positive integer; exclusive with `first_n_lines` |
| `first_n_lines` | Positive integer; exclusive with `last_n_lines` |
| `keyword` | Case-insensitive substring |
| `from_timestamp`, `to_timestamp` | Time strings such as `HH:mm:ss` or `HH:mm:ss.fff` |
| `include_pattern`, `exclude_pattern` | Case-insensitive regular expressions |

The notes report timestamp, keyword, regex, then line-count filtering. Timestamp filtering
could discard non-timestamped stack lines. Preserve full exception context separately.
They also report a final-256-KB tail optimization for large files unless beginning/time-based
filters required broader reading. Record actual truncation rather than assuming this cap.

Reported response fields: `lines` with `line_number/timestamp/content`, `total_lines_in_file`,
`filtered_count`, `filters_applied`, and optional `warning` when the file was absent.
An empty success response with a warning is not the same as an error-free game session.

## 4. State and entity response details

### Game state

The historical response used `scene_name`, `game_time`, `network_status`, and `loaded_mods`.
`game_time` was always zero in that implementation. Previously documented `build_index`,
`is_multiplayer`, `is_host`, and `game_version` were not established by those source notes.
Reported network labels included host, singleplayer, client, server, and unknown; their
meaning during transitions still requires observation.

### Player and inventory

Reported player fields: position, Euler-angle rotation, health, money containing cash/bank/
networth, and network status. Readiness depended on a local player instance.
The inventory handler enumerated non-null instances in hotbar slots and returned items/count.
It did not establish complete inventory coverage or stable fields for raw item serialization.

### Position-bearing operations

The described schema used a nested `position` object, not independent top-level coordinates.
Historical validation rejected positions below -10 or above 1000 on the vertical axis, or
outside 10000 on horizontal axes. These bounds belong to that handler and do not prove
collision safety, reachability, controller synchronization, or network acceptance.

### NPCs

Historical `filter` values were conscious, unconscious, in_building, and in_vehicle, but
only the first two were described as implemented. Other accepted values returned unfiltered
results. Reported fields included ID, name, position, health, consciousness, and object name;
reflection failures could omit fields.
NPC lookup reportedly tried several ID/name properties. Treat the resolved identity as
something to verify, especially where names can collide.
Teleport and health mutation handlers were documented as TODOs. The reported old-health
value was a constant, so it cannot serve as an independent before-state measurement.

### Items and properties

Item definitions reportedly provided real ID/name/description/stack-limit fields while
category, price, and legal status could be fixed placeholders. Do not use those placeholder
fields for economy analysis. The category filter was accepted without filtering.
Item addition and physical spawning were documented as TODOs that returned success.
Property enumeration returned an empty placeholder list; property lookup returned placeholder
position and occupants. Empty output cannot prove there are no owned properties.

## 5. Object/component inspection

Historical source: `src/tools/debug_tools.py` and `Handlers/Debug/DebugObjectInspectionHandler.cs`.
The object tool used `object_name`; old `game_object_name` and `component_name` parameters
were not present in the described schema. Component inspection was a separate tool.
Reported object fields included name/type, full type, position, quaternion rotation, scale,
component descriptions, child names/paths, and counts.

The described handler read up to 20 public properties and 20 public fields per component,
filtering some backing fields/pointers. That still permits expensive or stateful getters.
Prefer a named member on a known component. An ignored parameter or reflected property
exception must not be interpreted as a successful read of the intended target.

The notes mention tools such as `s1_inspect_component`, `s1_find_gameobjects`,
`s1_get_npc_position`, and lifecycle commands beyond the summarized subset. Discover their
actual schema rather than deriving arguments from the name.

## 6. Additional wire-only methods reported later

A later local release note mentioned methods without corresponding Python `s1_*` wrappers.
These are historical local capabilities to check, not automatically exposed connector tools.
The clock rows incorporate the correction and bounded live evidence recorded on 2026-10-24 in the [bridge changelog](../../../ThirdParty/S1MCPServer-master/S1MCPServer/CHANGELOG.md); the [compatibility matrix](../../../docs/compatibility.md#verification-matrix) owns current acceptance and limits.

| Reported method | Described behavior | Verification boundary |
|---|---|---|
| `get_time_of_day` | No arguments; `time_hhmm` (native HHMM, e.g. 1954 = 19:54), `time_minutes` (0-1439), `elapsed_days`, 24-hour `display`, or unavailable in menu | Owner-confirmed 2026-10-24; later live read returned `812` → 08:12 / 492 min. A no-op HHMM write/readback was also observed, not every write form. |
| `set_time_of_day` | Exactly one of `time` (decimal hours), `minutes` (0-1439) or `hhmm` (0-2359 or `"19:54"`); host-owned time change with `readback_hhmm`/`changed` | Inspect authority and compare the exact resulting clock |
| `get_weather` | No arguments; current weather-weight fields or unavailable | Reading weights does not prove visual weather effects |
| `list_mods` | Managed mod metadata and total count | A list alone is not patch-health evidence |

Do not prepend `s1_` and call a tool that the connector did not register. Use an inspected raw
protocol only when needed, within scope, and compatible with the installed implementation.

## 7. Historical transport contract

The uploaded notes describe two layers: MCP over stdio between agent and Python client,
and a custom TCP exchange between that client and the game mod.
The reported game endpoint was loopback port 8765. Verify the actual bind/configuration;
this is not a current security or availability guarantee.

| Element | Described behavior |
|---|---|
| Frame | Four-byte little-endian unsigned length plus UTF-8 JSON |
| Client frame limit | 10 MB |
| Request fields | `id`, `method`, `params` |
| JSON-RPC marker | The custom TCP serializer omitted `jsonrpc`; do not assume a standard JSON-RPC envelope |
| Response | Matching `id` with result or error |
| Acknowledgment | Client acknowledgment containing ID and received status after response |
| Heartbeats | Client and server heartbeat paths at roughly 60-second intervals |
| Client ownership | Described server accepted one client at a time |
| Execution | Socket work on a background thread; queued game work handled through the main-thread pump |

Reported errors included parse/invalid-request/method-not-found/invalid-params/internal-error,
plus game and validation errors. Preserve the actual code, details, and target context.
Do not collapse a game-not-ready response into an unsupported-API finding.

## 8. Transport and discovery incidents

These are inherited observations, not newly reproduced defects.

- **Connection before readiness:** the notes describe a listener started during mod init,
  while player/world objects were not yet ready. A successful handshake is not readiness.
- **Occupied port:** a stale or different process can own the endpoint. Identify it before
  any shutdown action; reconnecting may otherwise attach to the wrong game.
- **Second client rejected:** coordinate the current connection instead of repeatedly
  opening a competing one.
- **Large reflection queries:** main-thread traversal and serialization can stall frames.
  Bound depth, count, duration, and output; prefer exact targets.
- **Read/response timeout differences:** the notes described separate server/client waits
  and retries. A delayed response can outlive the caller's timeout; mutation outcome is unknown.
- **Acknowledgment phase drift:** an October 2026 report observed stale response IDs after
  an error path. Reject mismatched IDs, close the failed session, and reconnect deliberately.
- **Scan caps:** one recorded search stopped after 2000 objects. A zero match under that cap
  did not prove absence. Inspect a known exact path or broaden bounded coverage.
- **Argument conversion:** `call_method` reportedly returned silent null on some component
  or argument-conversion failures. Null alone does not establish invocation or success.
- **Volume inspection:** a profile getter can instantiate data. Do not read it to prove a
  profile has never been instantiated; inspect a non-mutating verified field path instead.

Debug-logging configuration and truncation limits also depend on the installed fork.
Preserve necessary evidence before toggling or restarting it, and disable noisy capture when
the bounded diagnostic interval ends.

## 9. Connection configuration and reporting

Locate the actual Python entry point, dependencies, working-directory requirements, and
configuration before launching a client. Historical notes mention `python -m src.main` from
`S1MCPClient/`, but the current package may have another supported entry point.
Do not overwrite existing MCP configuration with a guessed registration snippet.

A test report identifies the connector/schema, bridge and process, world/role/object,
request/result correlation, independent readback, observation duration, cleanup, and limits.
See [live debugging](live-debugging.md) and [bounded observations](sewer-key-observation.md).
