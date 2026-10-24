# Third-Party Review Signals — Interpretation and Evidence

> verified 2026-10-20 — every flag below was observed in one of three analysed third-party mods; file references are to the local decompiles under `Documents/Schedule1-Mod-Reversing/`.

> Evidence scope: signals distilled from the uploaded notes. Earlier mod examples are
> inherited static observations. A signal prompts inspection; it is not a verdict by itself.

## Diagnostics and lifecycle

| Signal | Inspect | Defensible finding |
|---|---|---|
| Empty catch around a whole tick | Logging, recovery, retained invalid state | The inspected failures are swallowed and may remain invisible |
| Conditional diagnostic methods | Build symbols and actual emitted call sites | Diagnostic coverage depends on whether these calls are present in this artifact |
| Repeated initialization/subscription | Ownership, deduplication, teardown | The demonstrated path duplicates work or retains stale instances |
| Hook counters | What is counted and actual owner metadata | Counts represent classes/targets/invocations only if that is what is measured |

A conditional attribute alone does not prove that every release log disappeared; inspect
the actual build. A deduplicated error log can be appropriate when repeats remain countable.

## Patch surface

| Signal | Inspect | Defensible finding |
|---|---|---|
| Compiler-generated target | Exact identity and current call path | Fragile across relevant compiler/source changes; future breakage is not certain |
| Private field-name reflection | Missing-field branch and default behavior | State the actual fail-open/fail-closed behavior, not a guessed one |
| Hash-suffixed RPC target | Full signature, installation gate, failure handling | Name the feature affected by a missing/changed target |
| Broad dynamic patch scan | Matching criteria and every selected target | Quantify scope and unintended compatible instances |
| Direct `RpcLogic` call | Authority, owner, network transmission, caller context | Explain the local/replicated effect actually established |
| Prefix skips original or replaces a native result | Restoration and installed native converter | Treat ABI and coexistence as explicit acceptance checks |

A single-player-count check cannot reliably distinguish offline play from a host alone.
Direct local implementation calls can be intentional in some paths; classify them by the
actual owner/replication contract rather than a blanket singleplayer-only label.

## Performance

Per-frame hooks, per-item getters, scans, allocations, and disk I/O are measurement targets.
Record invocation rate and measured cost, including diagnostic overhead.
A dirty flag can help when invalidation is correct; it is not a replacement for measurement.
Two postfixes can conflict if both mutate the same value, but overlapping target names alone
do not prove double-scaling or a performance fault.

## Persistence

Inspect write atomicity, durability assumptions, backup policy, schema validation, world
binding, and interrupted-write recovery separately. Atomic replacement and a backup solve
different problems. The absence of `.bak` alone does not prove that rename is non-atomic.
A periodic sidecar snapshot can lag game state; test the actual save/drop/switch sequence.
A shared fallback such as `unknown_slot` can merge worlds and should reject ambiguous writes.
Do not claim all custom items are invisible to native saves; inspect their registration and
serialization path. Native and mod restoration must have one clear owner.

## External effects

For HTTP, sockets, process creation, registry access, file writes/deletes, or P/Invoke, record:

1. Trigger and whether it is user initiated.
2. Destination and data sent/read/written.
3. Scope, validation, timeout, size limits, and cancellation.
4. Error handling and retained side effects.

A user-provided URL is not automatically harmless; a hard-coded host is not automatically
malicious. Report the specific operation and its effect. A bounded static scan is not a
security guarantee and cannot replace an executable's provenance or runtime evidence.

## Positive signals

Useful patterns include explicit owner IDs, tested state restoration, structured failure
results, schema migrations, validated world identity, targeted diagnostics, and resource
ownership. Runtime-generated assets or lazy registration can be appropriate, but neither
is inherently superior to shipped assets or explicit initialization.

For each adopted idea, verify the current API and failure path, then implement the behavior
needed by the receiving mod rather than copying unverified code or protocol constants.
