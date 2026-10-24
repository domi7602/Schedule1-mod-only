# Bounded Sewer-Key and World-Identity Observation

> UNVERIFIED as an end-to-end gameplay workflow; individual native observations below have limited scope and do not establish a playable search feature.

> Evidence scope: inherited notes from prior workspace observations. The original traces,
> screenshots, game build, and bridge implementation are not included. Named members are
> investigation leads to verify against the actual session, not current API declarations.

## Test-world boundaries

Use a disposable world only within the task's existing authorization and preserve its
required backup before mutation. Creating a new world does not imply overwriting an old one.
Resolve whether an API index is zero-based and how it maps to the visible slot; inspect the
actual slot and identity before invoking a create/load action.

## Separate collection state from object lifetime

The earlier notes compared `SewerManager.IsRandomWorldKeyCollected` with
`RandomWorldSewerKeyPickup`. A surviving managed/native wrapper is not sufficient evidence
that a pickup remains usable, and wrapper invalidity is not sufficient proof it was collected.
Read the authoritative collection state and object presence independently when available.
Do not infer a playable search feature or area accessibility from a candidate coordinate list.

## Bound discovery and reads

Inspect a known component through specific verified member access rather than enumerating
all getters. Some getters allocate or mutate state, including token-producing properties.
Enumerate candidate arrays or a known subtree in bounded batches, persist the batches,
deduplicate, and record coverage. One matching index does not prove all hierarchy ordering.
A capped search with zero matches does not establish absence.

## Observe network role and transitions

A host alone and offline play can share player count and host flags. The earlier notes used
lobby state as additional evidence; its current meaning and transition timing require checking.
Do not create a lobby merely to inspect an unrelated state. If a lobby-transition test is
within scope, record before/during/after states and verify return to the original session.
Pending create/join/leave states remain unknown until resolved.

## Validate world identity

A creation timestamp is a candidate input, not a verified unique world identifier.
Compare same-world reload, a different world, and replacement in the same slot.
Include independently verified generation data only with a versioned identity policy.
A `.Ticks` property does not prove subsecond entropy in the stored timestamp.
Static and instance member endpoints can differ; silent null from the wrong access path
must not become a valid default. Compare against the exact saved world's data where allowed.

## Preserve observation continuity

Start a bounded observer before the event, record elapsed time and each read's duration,
and distinguish continuous coverage from before/after snapshots.
A timeout, failed read, or missed interval breaks continuity. Do not infer sub-frame order
from coarse polling, and do not claim a transition was watched when it happened after expiry.

## Native UI and method invocation

Resolve the exact component and method with the installed bridge's schema.
A matching bare control name can refer to an unrelated UI; check the actual hierarchy,
active/interactable state, and resulting state after invoking an action.
Navigation methods may not enforce the visible UI's bounds; inspect indices/counts first.
The earlier bridge could return silent null on argument-conversion failure. Do not treat
that as success or retry blindly. A save method returning null is not evidence of completed
persistence; inspect relevant busy/error state and the exact saved target.

## Retain useful evidence

Keep raw traces and screenshots with explicit session identity. Required backups and final
verification notes need durable task storage rather than a prunable cache alone.
Process an existing spilled/large result locally where available instead of repeatedly
re-requesting broad reflection dumps.

## Acceptance boundary

This workflow can establish limited native observations. A search feature additionally
needs its own identity model, accessible-area validation, lifecycle/role tests, and user-facing
behavior acceptance. See [live debugging](live-debugging.md) for reporting requirements.
