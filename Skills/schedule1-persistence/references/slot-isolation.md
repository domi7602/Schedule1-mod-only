# Slot identity and legacy migration

> **Canonical home of the Triple-Guard / slot-suffix pattern** — SKILL.md and other skills reference here instead of duplicating the code. verified: 2026-09-11 standard; re-checked 2026-10-05.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 14/17 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

Inherited source notes; not revalidated against the installed game in this package.
This is the canonical identity policy for external per-save files. The earlier
last-known-slot helper and destructive migration sample have been replaced.

## Identity is more than a suffix

Use a confirmed completed-load identity from the current project's save contract.
Determine whether it includes the native save root, world identifier, owner/host,
slot, and load generation. Use only fields actually exposed by the target API.
A recycled slot number can refer to another world; a different load of the same
world can refer to an older checkpoint. Account for both distinctions.

A filename such as `state_slot_<number>.json` may remain a compatible storage
layout, but bind the file to validated identity metadata. Do not let its name
alone authorize access. Resolve and normalize paths only under the configured
mod/save root and only for path forms the native contract actually uses.

## State and write gate

| Context | Read/view behavior | Write behavior |
|---|---|---|
| Menu or no completed load | Show no active-world state | Closed |
| Load in progress | Retain old pending writes independently; hide old-world view | Closed for new context |
| Confirmed world, valid loaded state | Publish candidate state | Open under the confirmed destination |
| Confirmed world, safely confirmed missing file | Initialize a new state intentionally | Open after initialization policy |
| IO failure or uncertain identity | Preserve previous committed data and diagnostics | Closed pending reconciliation |
| Corrupt or newer-schema file | Preserve source; optionally show validated recovery read-only | Closed until verified repair |

For IL2CPP-native objects, use the target interop's appropriate validity checks
before dereference. Null, native pointer, and collected-wrapper checks are not
substitutes for completed-load identity. Never use `_lastKnownSlot`, a menu save
selection, or a default filename as authority to save unknown-context data.

## Context transition

1. Capture the old confirmed identity, destination, immutable snapshot, and dirty
   generation before resolving the new context.
2. Retain that bound write even when its first IO attempt fails.
3. Advance the visible context revision and clear the old presentation.
4. Resolve and validate the new context and its candidate state independently.
5. Retry old writes only against their original destination. Do not carry an old
   snapshot into the newly active world's filename.

Use [frozen writes](frozen-writes.md) for bounded queues, cancellation, and pending
state precedence. An auxiliary scene unload is not necessarily a world transition.

## Non-destructive legacy migration

- Require a confirmed target world and an explicit policy for assigning the
  legacy data. A global legacy file may not contain enough evidence to infer this.
- Read and validate the legacy source without changing it. Distinguish absence,
  read failure, malformed content, and unsupported schema.
- Inspect and validate the target independently. If both contain useful data,
  apply a documented merge/conflict policy; do not delete either based on existence.
- Write the validated candidate to the confirmed target with the storage helper.
  Check the Boolean result and read the target back using the production parser.
- Record migration completion only after verification. Retain the legacy source
  or a recoverable archive until cleanup is explicitly part of the migration policy.
- Repeat safely after interruption. Do not treat a partial target or marker as a
  completed migration, and do not move the only source before successful publish.

## Verification

Test A-to-B switching after an A write failure; both resulting destinations must
contain their own data. Test same-slot replacement by another world, reload of an
older checkpoint, unknown context, and negative/unavailable slot information.
For migration, cover valid legacy plus corrupt target, both valid, failed read,
failed publish, interrupted migration, and a second identical migration attempt.

Historical research anchors: `CalculatorState.cs`, `NotesApp.cs`,
`PayoutStateStore.cs`, and `BankState.cs` in the owning repository. Locate their
current versions instead of trusting inherited line numbers.
