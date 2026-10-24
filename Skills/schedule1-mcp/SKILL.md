---
name: schedule1-mcp
description: "Discover S1MCP tools and verify their actual game effects. Keywords: S1MCP, S1MCPServer, S1MCPClient, Model Context Protocol, s1_get_player, s1_get_player_inventory, s1_capture_logs, s1_list_npcs, s1_spawn_item, s1_inspect_object, s1_get_game_state, live debugging, TCP 8765."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — S1MCP Live Inspection

## Scope and prerequisites

Follow the [working contract](../schedule1-modding/references/working-contract.md).
Discover the tools exposed in this session and inspect their actual input schemas first.
Record bridge identity, game process/session, scene, network role, and available methods.
This package does not include or connect a running bridge; historical names are lookup hints.

## Connection and schema workflow

1. Confirm that the intended bridge and game process are available. Do not launch a game
   or attach to an unrelated session merely because an old example contains that step.
2. Prefer the installed connector. Use raw transport only with its actual framing contract
   and only when the configured connector cannot perform the authorized operation.
3. Compare the live tool schema with [historical signatures](references/tool-signatures.md).
4. Use a small read-only request to establish the target process and readiness.
5. Resolve an exact object/component before reading selected members.
6. Bound depth, result count, elapsed duration, and retries; preserve partial/unknown results.

## Historical names and known caveats

These entries summarize the uploaded source notes, not a fresh live discovery.

| Historical capability | Required caution |
|---|---|
| `s1_get_game_state` | Fields can be placeholders; source notes mark `game_time` as a stub |
| `s1_capture_logs` | Notes use `last_n_lines`, not `max_lines`; inspect the current schema |
| `s1_inspect_object` | Notes use `object_name`; component inspection is separate |
| `s1_get_player_inventory` | Notes describe hotbar enumeration, not a proven full inventory |
| `s1_add_item_to_player`, `s1_spawn_item` | Described revision returns success from unimplemented handlers |
| `s1_teleport_npc`, `s1_set_npc_health` | Described revision contains non-mutating stubs |
| `s1_list_properties` | Described property enumeration is a stub |
| `s1_list_buildings` | Absent from the described source revision; do not invent the tool |

## Read-only diagnosis

Start with connection/readiness, recent logs, and an exact target state.
Do not interpret an empty search as absence when discovery is scan-capped or incomplete.
Prefer explicit known members over broad reflection: a property getter may allocate or
change state. Avoid scene-wide native enumeration in hot loops.
A connected socket does not prove gameplay readiness; a valid scene name does not prove
that the target singleton or world data is ready.

## Test mutations

Use a mutation only within the user's authorized task and an appropriate test world.
Confirm the intended world, authority, restoration plan, and actual implemented handler.
Capture before-state, call with the discovered schema, then read the exact changed state.
A `success` field, silent null, or lack of error is not acceptance evidence.
Do not use documented stubs to claim spawning, money changes, NPC movement, or health tests.
A timeout can leave the result unknown; read back before retrying a non-idempotent action.
Restore temporary changes and record cleanup failure explicitly.

## Transport and failure handling

- Do not infer current bind address, authentication, or message framing from this archive.
- Check response/request identity. A mismatched response is not evidence for the command.
- Reconnect after a confirmed protocol phase error; do not continue on stale IDs.
- Distinguish unsupported parameters, unavailable tools, unready game state, and timeouts.
- A second connection can be rejected by the historical single-client bridge; coordinate
  ownership rather than repeatedly opening sockets.
- Keep startup-crash triage available through preserved logs when the bridge cannot help.
- Do not kill a process solely because a port is occupied; identify the process and session.

## Verification and delivery

Report tool/schema identity, target world/object, request outcome, independent readback,
observation duration, scan limits, and any retained unknowns.
Native observations from one role or session do not establish host/client transition safety.

## Read triggers

- [Tool signatures](references/tool-signatures.md): inherited schemas, stubs, wire protocol, transport incidents.
- [Live debugging](references/live-debugging.md): bounded diagnostic sequence and test readbacks.
- [Sewer-key observations](references/sewer-key-observation.md): identity, scan bounds, and observation continuity.
- [Log triage](../schedule1-troubleshooting/references/logscan-and-logs.md): bridge-independent evidence.
