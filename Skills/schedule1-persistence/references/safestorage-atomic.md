# Atomic file replacement and recovery

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 1 of 1 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Inherited source notes; not revalidated against the installed game in this package.
`SafeStorage` is a repository helper, not a standard S1API service. Inspect its
current source, callsites, return contract, and serializer options before use.

## Establish the actual guarantee

The inherited implementation description writes a temporary file, copies an
existing target to a backup, and replaces the target. Verify the real sequence,
filesystem assumptions, cleanup, concurrency handling, and error propagation.
Separate complete-file publication, backup validity, and crash/power-loss durability.
Do not claim a database transaction or native-save transaction from a helper name.

Use the established helper for production writes when it satisfies the required
contract. Do not reimplement ad hoc file replacement at every callsite. If it lacks
needed behavior, repair the shared boundary or isolate an adapter with tests.

## Writer contract

1. Require a confirmed destination and validated immutable snapshot.
2. Serialize with the same schema/options as the reader. Validate useful content,
   not merely whether an empty object can be serialized.
3. Publish using the helper's verified atomic-replacement path. Serialize writers
   per target; a shared fixed temporary name is unsafe for overlapping writes.
4. Treat false or exception as a failed write. Preserve dirty state, pending data,
   destination, and error details. Do not print success or advance saved generation.
5. Release only temporary files owned by this operation. Preserve recoverable
   primary/backup data when recovery is incomplete.

## Reader contract

Return a meaningful outcome at the persistence boundary: valid, missing,
corrupt, unsupported, or unknown due to IO failure. A fallback object by itself
cannot communicate that distinction. If the shared helper hides it, add an
adapter or use a more expressive supported path before authorizing writes.

A text read that succeeds does not establish valid JSON. Validate parsing, schema,
bounds, and identity for the primary; validate the backup independently when
needed. Do not promise a backup attempt merely because `LoadTextSafe` was called.

A valid backup with a corrupt primary is a recovery state. A normal write may
rotate the corrupt primary over the only good backup. Preserve recovery material
and keep writes closed until a separately verified repair sequence is complete.

## Tests

Cover absent target, valid target, corrupt primary, corrupt backup, schema mismatch,
permission failure, failed temporary write, failed replacement, competing writes,
and restart with leftover temporary files. Assert that reported success matches
published content and that failure preserves both dirty state and recovery data.
Use fault injection for policy; identify real disk/native tests separately.

Related: [slot identity](slot-isolation.md) and [frozen writes](frozen-writes.md).
Historical source anchor: `Source/Mods/Shared/src/SafeStorage.cs`.
