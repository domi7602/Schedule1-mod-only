# Live Debugging with S1MCP

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 3 of 3 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

> Evidence scope: procedure revised from inherited bridge notes. No bridge connection,
> tool call, game-state mutation, or gameplay test was performed for this package.

## 1. Discover the actual session

Confirm the task's game/session and available connector before using tools.
Inspect the exposed tool names and input schemas; preserve the bridge/build identity.
Use a small read-only game-state request if available and check the actual response fields.
Do not assume a field exists because a previous example used it.
A connected bridge can precede player/world readiness.

## 2. Capture the relevant failure

Prefer a bounded log read around the reproduction time using the live schema.
For changed parameter names, consult [historical log filtering](tool-signatures.md#3-log-filtering-details).
Preserve complete exception context through a file capture when a filtered response omits it.
Record time bounds, truncation, process/session identity, and failed reads.
See [log triage](../../schedule1-troubleshooting/references/logscan-and-logs.md).

## 3. Inspect an exact target

Resolve player/NPC/object identity, then read only the members needed for the hypothesis.
Object and component inspection are separate; resolve schema drift through the
[historical inspection notes](tool-signatures.md#5-objectcomponent-inspection).
Prefer a known path/member over a broad reflective sweep because getters can have effects.
Record scan caps and incomplete coverage; an empty capped search is not proof of absence.

## 4. Establish implemented mutation capability

Before changing state, inspect the actual tool schema and handler/capability evidence.
The [historical tool table](tool-signatures.md#2-historical-tool-status-and-parameters)
identifies inherited stubs; their success responses cannot establish gameplay changes.
If only a stub is available, use an independently verified native/UI path within the task's
scope or report that the test cannot be performed through this bridge.

## 5. Run an authorized controlled scenario

1. Confirm the test world, target object, network role, and prior state.
2. Establish a restoration/cleanup path and preserve any required save copy.
3. Invoke the implemented operation using its discovered parameter schema.
4. Read the exact affected state through an independent verified path.
5. Observe the gameplay behavior required by the test, not just the tool's response.
6. Restore temporary state and verify cleanup.

Never retry a timed-out non-idempotent operation before resolving whether it already applied.

## 6. Handle uncertainty

| Observation | Interpretation |
|---|---|
| Silent null | Return value, missing target, conversion failure, or swallowed error remain possible |
| Success from documented stub | No verified state mutation |
| Stale response ID | Protocol/session failure; result is not attributable to the request |
| Timeout | Outcome unknown until readback or other evidence |
| No matching object in capped scan | Search incomplete or target unresolved |
| Read succeeds before/after an event | Two observations, not continuous transition coverage |

## 7. Deliver the result

Report the request/schema, exact target, before/after readback, relevant logs, observation
window, cleanup outcome, and unsupported tests. Separate source inspection from live evidence.
See [historical signatures](tool-signatures.md) and
[bounded observation notes](sewer-key-observation.md).
