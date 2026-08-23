---
name: schedule1-mcp
description: >-
  Live game introspection, remote debugging, and agentic state manipulation for Schedule I via S1MCPServer (MCP Protocol over TCP port 8765). Use when inspecting live player data, inventories, NPC positions, spawning items, querying game scenes, capturing runtime MelonLoader logs, or diagnosing live mod behavior.
  Keywords: S1MCP, S1MCPServer, S1MCPClient, Model Context Protocol, s1_get_player, s1_get_player_inventory, s1_capture_logs, s1_list_npcs, s1_spawn_item, s1_inspect_object, s1_get_game_state, live debugging, TCP 8765.
---

# Schedule I — S1MCP Live Game Introspection & Debugging Skill

This skill is the runbook for **live, real-time agentic interaction with the running *Schedule I* game** using the Model Context Protocol (MCP) bridge (`ifBars/S1MCPServer`).

> **Stack Status (verified: 2026-08-22):** Game v0.4.6f13 (IL2CPP net6), MelonLoader 0.7.3, Python 3.11, TCP localhost:8765, JSON-RPC 2.0 stdio MCP server.

---

## 1. Architecture & Mental Model

```
┌─────────────────────────────────────────────────────────────┐
│                    AI Agent (Antigravity / Flash)            │
│                    (Evaluates tools, queries live state)     │
└────────────────────────────┬────────────────────────────────┘
                             │ MCP Protocol (JSON-RPC over stdio)
┌────────────────────────────▼────────────────────────────────┐
│              S1MCPClient (Python MCP Server)                │
│  - Python 3.11 (`mcp>=0.9.0`, `pydantic`, `httpx`)          │
│  - Translates MCP tools → JSON-RPC TCP packets              │
│  - Working Directory: `ThirdParty/S1MCPServer-master/...`   │
└────────────────────────────┬────────────────────────────────┘
                             │ TCP (localhost:8765, 4-byte length prefix)
┌────────────────────────────▼────────────────────────────────┐
│              S1MCPServer (MelonLoader Mod .NET 6)           │
│  - Background Thread: TCP Server listening on port 8765     │
│  - Main Thread Dispatcher: Executes Unity API calls safely   │
│  - Registered in `<Game>\Mods\S1MCPServer-IL2CPP.dll`       │
└────────────────────────────┬────────────────────────────────┘
                             │
┌────────────────────────────▼────────────────────────────────┐
│              Schedule I Game Engine (v0.4.6f13)              │
│  - PlayerInventory, PlayerClothing, NPCs, Growing, Buildings│
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Decision Tree — When to Use S1MCP Tools

```
Need live game interaction?
│
├─ Diagnosing an in-game error / crash / warning?
│  → Call `s1_capture_logs(keyword="error", max_lines=50)` or `keyword="Exception"` (see §4)
│
├─ Checking player status or money / inventory?
│  → Call `s1_get_player()` and `s1_get_player_inventory()`
│
├─ Testing item drop / spawning / item definition?
│  → Call `s1_list_items(category=...)`, `s1_get_item(item_id=...)`, or `s1_spawn_item(...)`
│
├─ Checking NPC AI / position / health / dialogue?
│  → Call `s1_list_npcs()`, `s1_get_npc(npc_id=...)`, `s1_teleport_npc(...)`
│
├─ Verifying building state / property ownership?
│  → Call `s1_list_buildings()` or `s1_get_building(building_id=...)`
│
└─ Inspecting native Unity GameObject hierarchy / components?
   → Call `s1_inspect_object(game_object_name=...)`
```

---

## 3. Core S1MCP Tool Reference

| Tool Name | Purpose | Parameters |
|---|---|---|
| **`s1_get_game_state`** | Current scene, version, player active, network state | None |
| **`s1_capture_logs`** | Capture & filter MelonLoader logs directly from the game | `keyword`, `max_lines`, `log_level`, `start_time` |
| **`s1_get_player`** | Live player position, health, cash, stamina, state | None |
| **`s1_get_player_inventory`** | Full inventory dump (slots, quantities, item IDs) | None |
| **`s1_teleport_player`** | Move player to coordinates `(x, y, z)` | `x`, `y`, `z` |
| **`s1_add_item_to_player`** | Add item directly to player's inventory | `item_id`, `quantity`, `quality` |
| **`s1_list_npcs`** | List active NPCs in the loaded scene | `filter`, `include_inactive` |
| **`s1_get_npc`** | Get details for specific NPC (health, position, job) | `npc_id` |
| **`s1_teleport_npc`** | Teleport NPC to coordinates | `npc_id`, `x`, `y`, `z` |
| **`s1_set_npc_health`** | Set NPC health / conscious state | `npc_id`, `health` |
| **`s1_list_items`** | List registered item IDs in `Registry` | `category`, `search` |
| **`s1_get_item`** | Inspect definition (price, stack limit, category) | `item_id` |
| **`s1_spawn_item`** | Spawn physical item in front of player or at `(x,y,z)` | `item_id`, `quantity`, `x`, `y`, `z` |
| **`s1_list_buildings`** | List properties & business states | None |
| **`s1_inspect_object`** | Reflectively inspect components & fields of a GameObject | `game_object_name`, `component_name` |

---

## 4. The 4 Non-Negotiable S1MCP Rules

1. **Scene Gate:** S1MCP is active only once the `Main` (gameplay) scene or `Menu` scene is loaded. Startup crashes (0–2 seconds after launch) happen before the TCP server can accept connections — use log file parsing / process isolation for startup crashes.
2. **Never Freeze IL2CPP:** Never call `Resources.FindObjectsOfTypeAll<GameObject>()` in queries. Use `SceneManager.GetActiveScene().GetRootGameObjects()` and bounded tree traversal.
3. **Localhost Only Security:** S1MCPServer binds strictly to `127.0.0.1:8765`. It never exposes ports externally.
4. **Main-Thread Marshaling:** All Unity API and IL2CPP calls MUST execute on the Main Thread via `CommandRouter` and the mod's `OnUpdate` loop. Background threads only handle TCP socket I/O.

---

## 5. References

- [Tool Signatures & JSON-RPC Schema](references/tool-signatures.md)
- [Live Debugging Runbook](references/live-debugging.md)
