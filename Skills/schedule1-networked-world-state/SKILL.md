---
name: schedule1-networked-world-state
description: "Design Schedule I host authority and replicated world state."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Schedule I — Networked World State

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Use this workflow only for explicitly requested multiplayer work; existing authority/refusal guards remain required.
Prefer the verified FishNet/S1API transport and workspace infrastructure over a third-party protocol.

## Establish prerequisites

1. Identify the actual transport, authenticated sender contract, session identity, and host authority.
2. Define object identity, ownership, permissions, resource costs, and what state must be durable.
3. Identify native-save and mod-save success signals separately; inspect their actual failure contracts.
4. Set payload, pending-operation, fragment, retry, and retention limits from the intended game scale.
5. Decide whether prediction is needed. Start without it when latency and UX permit.

## Commit and replicate

1. Let clients submit intent; the host computes and validates resulting state and costs.
2. Include protocol/schema version, session, stable operation ID, actor, target, action, and expected base revision.
3. Bind authorization to the transport's sender identity. Reject mismatched, absent, or stale session identities before mutation.
4. Deduplicate operation IDs within a bounded session-aware policy. Check permissions, inventory, geometry, and invariants.
5. Stage a candidate state and the resource/side-effect plan. Validation alone is not rollback; provide compensation or a recoverable pending state for commit failures.
6. Publish acceptance/revision only after required commit work succeeds. Never acknowledge a transaction whose resource mutation failed.
7. Apply deltas only to the expected revision and validated schema. Reconstruct in a copy and validate before replacing live state.
8. On a gap or mismatch, request a full authoritative snapshot; do not guess a merge.
9. Send a joining client a bounded snapshot before deltas and preserve ordering while the join baseline is installed.
10. Bound/expire fragment assemblies and reject conflicting duplicate chunks or oversized reconstruction.

## Prediction and ambiguous outcomes

- Track each pending operation, its owned provisional effects, and the last authoritative state.
- On rejection, undo only that operation's provisional effects, then rebase remaining predictions.
- A timeout is an unknown result, not proof the server rejected the request. Query/reconcile with the same operation ID before a final resource refund.
- Ignore duplicate resolutions. A late accepted response must not apply or refund twice.
- Do not restore an old whole-world snapshot over newer accepted changes.

## Coupled-save recovery

1. Bind pending intent to the exact normalized save identity, schema, operation/revision, and recovery snapshot.
2. Durably write intent before the native save when the two stores must agree.
3. Mark native completion only at a success point verified for the target build.
4. Recover automatically only when native and mod-file evidence unambiguously selects one consistent result.
5. If identity, completion, or content is ambiguous, freeze dependent writes and preserve recoverable copies.
6. Clear/archive recovery data only after the durable result is verified.

## Verify and instrument

Count accepted/rejected/duplicate requests, gaps, snapshot fallback, pending operations, timeouts,
and recovery outcomes. Keep the first cause; rate-limit repeats.
Test host-only, two clients, late join, concurrent edits, disconnect, retry, lost/delayed ACK,
stale base, bad identity, malformed/oversized fragments, partial commit failure, and save interruption.
Confirm one authoritative result, correct resource accounting, and no cross-save recovery.

## Read on demand

| Need | Reference |
|---|---|
| Protocol, reconciliation, journal, failure matrix | [Network state reference](references/networked-world-state.md) |
| File state and write gates | [Persistence](../schedule1-persistence/SKILL.md) |
| Placement transaction and native object choice | [Grid](../schedule1-grid/SKILL.md) |
| Money mutation and transaction semantics | [Economy](../schedule1-economy/SKILL.md) |

## Evidence and output

Third-party decompiles provide architectural leads. Report authority, commit/recovery boundaries
and actual multiplayer tests under the shared evidence policy. Session IDs or atomic sidecars
alone do not establish safety.
