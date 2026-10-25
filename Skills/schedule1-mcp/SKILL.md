---
name: schedule1-mcp
description: "Discover S1MCP tools and verify their actual game effects."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Schedule I — S1MCP Live Inspection

## Scope and prerequisites

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Discover the tools exposed in this session and inspect their actual input schemas first.
Record bridge identity, game process/session, scene, network role, and available methods.
Historical names are lookup hints; this package supplies no running bridge.

## Inspect safely

1. Confirm the intended process/session; do not launch or attach elsewhere because an old
   example does so. Use the installed connector and a small read-only readiness request.
   A connected socket or scene name does not establish target singleton/world readiness.
2. Resolve the exact object/component and selected members. Getters can mutate or allocate.
   Bound depth, count, duration and retries; avoid scene-wide hot-loop scans. Empty capped
   results do not prove absence.
3. Correlate request/response IDs. Stop on mismatches and reconnect after a confirmed protocol
   phase error. Distinguish unavailable tools, unsupported parameters, unready state and timeouts.
   Do not kill a process solely because a port is occupied; identify its owner/session first.

## Mutations require observed effects

Stay within the authorized task and test world. Confirm world identity, authority, implemented
handler and restoration plan. Capture before-state, invoke with the discovered schema, then
read the changed state independently. Stub success, silent null and lack of error do not prove
an effect. **Never retry a non-idempotent operation while its outcome is unknown.** Restore
temporary state and report cleanup failure.

## Load only for the relevant question

| Trigger | Reference |
|---|---|
| Reproduction, controlled mutation, uncertainty or reporting | [Live debugging](references/live-debugging.md) |
| Schema drift, placeholders, absent tools or stub handlers | [Historical tool status](references/tool-signatures.md#2-historical-tool-status-and-parameters) |
| Connector cannot perform an authorized operation; transport failure or connection configuration | [Transport contract and incidents](references/tool-signatures.md#7-historical-transport-contract) |
| World identity, sewer-key state, scan coverage or transition observation | [Bounded observations](references/sewer-key-observation.md) |
| Startup crash or unavailable bridge | [Preserved-log triage](../schedule1-troubleshooting/references/logscan-and-logs.md) |

Use raw transport only when the connector cannot perform the authorized operation and its actual
framing/configuration contract is known. Historical bind address, authentication and single-client
behavior are not current guarantees. Report schema, target,
readback, duration, scan limits and cleanup. One session does not establish host/client safety.
