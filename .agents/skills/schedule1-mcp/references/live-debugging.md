# Live Debugging Runbook with S1MCP

This guide provides step-by-step procedures for AI agents to diagnose runtime issues, inspect state, and test mod features using S1MCP.

---

## 1. Quick Health Check Workflow

When a player reports a bug in-game (e.g. "My plants aren't watering" or "Item won't spawn"):

1. **Verify Connection & Scene:**
   - Call `s1_get_game_state`
   - Ensure `scene_name` is `"Main"` (active gameplay) and not `"Menu"`.

2. **Capture Recent Errors:**
   - Call `s1_capture_logs(keyword="Exception", max_lines=20)`
   - Call `s1_capture_logs(keyword="Error", max_lines=20)`
   - Identify the mod throwing exceptions from the stack traces.

3. **Inspect Player & Target Entity:**
   - For item issues: call `s1_get_player_inventory()` to check slot counts and item definitions.
   - For NPC issues: call `s1_get_npc(npc_id=...)` to check position and health.
   - For custom GameObjects: call `s1_inspect_object(game_object_name=...)`.

---

## 2. Automated Test Scenario Injection

To quickly verify a new mod feature without requiring manual player grind:

- **Money Test:** `s1_add_item_to_player(item_id="cash_stack", quantity=5)` or manipulate cash directly.
- **Item Interaction Test:** `s1_spawn_item(item_id="custom_skateboard", quantity=1)` directly at player position.
- **NPC Quest Test:** `s1_teleport_npc(npc_id="jeff_gilmore", x=100.0, y=1.0, z=50.0)` to bring the NPC directly to the player for dialogue testing.
