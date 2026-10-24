# Frozen writes and personal-intent persistence

> UNVERIFIED for native save/load roundtrips; policy and fault-injection tests are distinct from runtime acceptance. See the mod-specific verification record before applying these rules.

Implementation contract; not verified in a native save/load session by this package.
Use for notes, bookmarks, search acceptance, and other state whose visible context
changes independently of pending disk writes.

## Preserve the data/destination pair

- Capture immutable data, confirmed identity, destination, and generation together.
  Do this before clearing the view or resolving a different world.
- Keep failed writes independently of the current UI. Retry the exact bound pair;
  never resolve a new current path for retained old data.
- Only a successful writer result may clear dirty state, advance saved generation,
  discard the pending write, or emit a successful-save status.
- Coalesce pending writes by confirmed destination. A newer pending generation
  supersedes an older snapshot for that destination, not a newer completed write.
- Bound retained work and retry cadence. On capacity exhaustion, surface the storage
  fault and keep the last recoverable snapshot; do not silently drop unsaved edits.
- Advance visible context revision even when retaining a write fails, so a cached
  Unity projection cannot continue displaying another world's rows.

## Load precedence and explicit clear

An older disk snapshot must not replace newer retained pending data for the same
identity. Resolve precedence before publishing the candidate to the UI.
An absent current-view record during transition is not a deletion command.
Represent an explicit clear as an intentional empty generation; retain and retry
that generation on failure instead of resurrecting older rows.

Keep personal acceptance/cancellation distinct from observed native completion.
For example, cancelled acceptance must not turn back into an accepted search just
because an older native save makes its target appear uncollected again. Apply the
project's documented rollback policy to each kind of state independently.

## Validation and recovery

1. Distinguish missing files, unreadable text, invalid JSON, incompatible identity,
   and unsupported schema. Do not convert all failures into empty defaults.
2. Parse and validate a candidate before replacing committed visible state.
3. Validate backup contents explicitly. Preserve valid recovery data before any
   write that could rotate a corrupt primary over it.
4. Keep writes closed after an unresolved IO failure until a complete reconciliation
   re-establishes identity, valid content, and the recovery/write policy.
5. Bound row counts and text length at both load and edit boundaries. Reject nonfinite
   coordinates, normalize duplicate keys, and escape labels for their rendering use.
6. Verify serialization options on the actual DTO. A field-only DTO needs field
   inclusion when using System.Text.Json; properties are a separate serializer choice.

Microsoft's [System.Text.Json field guidance](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/fields)
explains opt-in field handling. Do not infer the S1API SaveableField contract from
System.Text.Json attributes; those are different persistence mechanisms.

## Verification

Exercise false-write -> retained snapshot -> context change -> original-target
retry, then inspect both worlds' files. Assert failure does not advance success
counters. Test clear-to-empty, failed clear, cancellation during load, pending data
versus older disk state, bounded queue failure, invalid backup, oversized input,
and useful production-serializer roundtrips.

For native-world-dependent records also verify the precise native checkpoint.
A void return, stale save metadata, or an immediate non-saving flag does not prove
asynchronous IO completed. Use [slot identity](slot-isolation.md) and the native
save coordination rules in [the persistence skill](../SKILL.md).
