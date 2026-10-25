# S1MAPI — Geometry Persistence and Restoration Ownership

> verified: research against S1MAPI source + workspace mods 2026-10-05. S1MAPI.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 59/62 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Evidence boundary

Inherited workspace notes inspected S1MAPI and mod sources on 2026-10-05. They observed runtime geometry without an automatic save contract. The actual deployed implementation and native restoration paths are not supplied here. Treat persistence ownership as an explicit integration decision, not a property of a builder or network flag.

## Decide who restores each object

| Object kind | Durable source | Restore owner |
|---|---|---|
| Static mod-authored decoration | Versioned mod definition/assets | Deterministic mod rebuild |
| User-placed mod-owned geometry | Validated mod placement state | Mod factory keyed by stable ID |
| Native registered buildable | Native save plus optional mod side data | Native loader; mod reattaches extra state |
| Networked prefab with uncertain saving | Unknown until inspected/tested | Resolve before adding a respawn path |

A network spawn is not evidence of disk persistence. Conversely, lack of S1MAPI save hooks does not prove that a spawned native component cannot be restored by the game. Unconditional registry respawn does **not** safely cover both possibilities.

## Placement registry

Keep durable definitions separate from live Unity references. Store stable placement ID, definition/prefab identity, schema version, local/world transform convention, and feature-specific state. Generate identity once at committed placement; preserve it on reload. Validate duplicate IDs, finite coordinates, supported definitions, and bounded collection sizes.

Runtime references belong to a separate owner/generation-bound registry and are rebuilt or rebound. Do not serialize raw Unity/IL2CPP references as durable identity.

## Save identity and load gate

Use [slot identity and migration](../../schedule1-persistence/references/slot-isolation.md)
and [validation/recovery gates](../../schedule1-persistence/references/frozen-writes.md#validation-and-recovery).
Resolve authority before loading or writing; unresolved identity or load failure
keeps affected writes closed.

## Commit at a verified save boundary

Placement and pack-up mutate the in-memory committed registry and inventory through one transaction contract. Persist at a boundary whose actual completion/failure meaning was verified for the current game.

Writing a sidecar atomically after a native save can still leave a crash window between the two stores. If resource ownership depends on their agreement, use a recovery journal or a verified native-integrated persistence route. Refer to [networked world-state recovery](../../schedule1-networked-world-state/references/networked-world-state.md) for journal invariants; the same coupled-save concern can exist in single player.

On native save failure, retain dirty state and diagnostics; do not report the sidecar as synchronized. Mark data clean only after the intended durable write succeeds.

## Restoration sequence

After the load gate, follow [native restoration and duplicate prevention](../../schedule1-persistence/references/buildable-restore.md#restore-metadata-without-spawning-duplicates).
For mod-owned geometry, create one root and reconcile missing owned placements
from saved definitions. Reapply owned terrain/navigation changes only after
geometry and native prerequisites are ready. Network linking/late-join
synchronization is separate from disk restoration. Publish readiness only after
invariants and mandatory dependencies pass.

## Runtime teardown versus durable state

Dispose roots, navigation owners, listeners, and runtime caches on the relevant world teardown, including same-save reload. A persistent root is optional and must not leak children across worlds.

Follow [cleanup/save coordination](../../schedule1-persistence/references/buildable-restore.md#cleanup-and-save-coordination)
and [retained-write ownership](../../schedule1-persistence/references/frozen-writes.md#preserve-the-datadestination-pair).
Keep recovery records until the save outcome is known; retaining durable data
does not require retaining dead Unity objects.

## Failure and regression matrix

Use the canonical [restore checks](../../schedule1-persistence/references/buildable-restore.md#verification),
[identity/migration checks](../../schedule1-persistence/references/slot-isolation.md#verification)
and [write-failure checks](../../schedule1-persistence/references/frozen-writes.md#verification).
Add missing geometry definitions, repeated mod-owned rebuilds, corrupt-primary
recovery with valid/invalid backups, access denied, client writes, interrupted native/sidecar saves
and late network links after scene exit where applicable.

Acceptance requires one restored object per ID, no lost/duplicated inventory, no cross-save state, preserved recoverable data, and bounded cleanup. Historical reports that a particular lifecycle event fired zero times are a reason to measure the current event path, not a universal API prohibition.

## Source anchors

- Historical examples: `Source/Mods/AutoPackagingStation/src/AutoPackStore.cs`, `Source/Archive/HomelessMod/src/StreetPropertyManager.cs`, and deterministic TaxiDriver geometry rebuilds.
- S1MAPI source search: prefab placement, network linking, terrain changes, navigation lifecycle, and resource tracking under `ThirdParty/S1MAPI/`.
