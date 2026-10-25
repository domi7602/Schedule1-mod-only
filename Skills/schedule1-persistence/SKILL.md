---
name: schedule1-persistence
description: "Protect Schedule I saves, world identity and migrations."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Persistence and save isolation

## Scope and prerequisites

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Inspect actual storage, lifecycle, serializer and identity contracts. `SafeStorage`,
`SaveSlots` and `ModConfig` are external repository helpers, not bundled implementations.

## Select the persistence model

| Data | Required boundary |
|---|---|
| Global preferences | Global validated config; no accidental slot dependency |
| Personal notes, bookmarks, accepted/cancelled intent | Confirmed world identity where applicable; frozen independent pending writes |
| Native-world-dependent state, rewards, placed items | Same native checkpoint; explicit save coordination and recovery policy |
| Native-restored BuildableItems | Restore mod metadata onto native objects; do not duplicate spawn |

A slot number is a filename component, not proof of world identity or checkpoint.
An atomic sidecar write does not make the engine save and sidecar one transaction.

## Workflow

1. Identify the data owner, lifetime, schema, authoritative writer, and whether
   rollback of the native save must also roll back this state.
2. Bind the completed-load identity to a destination. During menu, transition,
   invalid identity, or ambiguous context, keep new writes closed.
3. Load into a candidate. Distinguish missing, valid, corrupt, unsupported schema,
   and unknown because IO failed. Validate any backup independently.
4. Commit only validated candidate state to the visible model. Never turn an IO
   failure into an empty successful load or permission to overwrite the source.
5. Freeze an immutable snapshot and its confirmed destination before context
   changes. Advance the saved generation only after the writer reports success.
6. For world-dependent sidecars, freeze at the established native save boundary,
   verify destination and native outcome, and commit only the compatible snapshot.
7. Reset scene projections on a confirmed world transition, retain recoverable
   pending writes independently, and retry with bounded frequency and capacity.

## Recovery rules

- Retry an old snapshot only at its original confirmed destination; never combine
  old data with the newly active slot. Coalesce by destination and generation.
- Do not replace newer pending state with an older disk snapshot during reload.
- Keep unreadable or newer-schema state protected. A later successful path probe
  alone must not silently authorize overwriting an earlier unresolved failure.
- Do not rotate a corrupt primary over the only valid backup. Preserve both until
  a verified repair transaction can safely publish and retain recovery data.
- Migrate legacy state by validated, non-destructive copy. A destination merely
  existing does not authorize deletion of the legacy source.
- For unknown native mutation effects, retain an unresolved operation and block
  blind retry. Save completion callbacks alone do not settle a payment outcome.

## Lifecycle and cleanup

Use `OnPreLoad` and `OnLoadComplete` as candidates for world transition handling,
then verify them in the target runtime. `OnSaveInfoLoaded` concerns save metadata;
it does not establish a completed world load. Do not clear gameplay state on every
scene-unload callback: auxiliary scenes can unload independently.
Keep handler installation, retry pumps, and unavailable-state UI reachable before
readiness gates. Pure helper tests do not prove real callsites execute.

## Verification

Select relevant checks: missing files, denied access, corrupt primary with valid backup, both
invalid, unknown schema, failed write, slot switch during pending write, and
explicit clear-to-empty. Verify the actual serializer roundtrip with useful data.
For native-coupled data, include save/reload, rollback and interrupted save;
host/client tests require explicit scope. Distinguish policy tests from gameplay acceptance.

## References

- File replacement or helper migration: [Atomic storage](references/safestorage-atomic.md).
- Identity resolution or legacy migration: [Slot isolation](references/slot-isolation.md).
- Failed writes, changing UI context, personal intent: [Frozen writes](references/frozen-writes.md).
- Placed native objects or duplicate spawns: [Buildable restore](references/buildable-restore.md).
- Unclear event ordering: [Lifecycle verification](../schedule1-lifecycle-verify/SKILL.md).
- Framework-owned save data: [S1API saveables](../schedule1-s1api/references/saveables.md).
