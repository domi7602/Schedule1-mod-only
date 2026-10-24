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

Follow the current [persistence skill](../../schedule1-persistence/SKILL.md) and its canonical file contracts.

1. Resolve the exact active save identity and authority before loading or writing.
2. Distinguish a confirmed absent file from unreadable, malformed, incompatible, or wrong-save data.
3. Validate a recovered backup against the same schema, identity, and domain rules.
4. Keep dependent writes frozen after an unresolved load error; a default empty state is not permission to overwrite.
5. Do not reuse the previous slot identity during a transition or infer world identity from a slot number alone.
6. Perform legacy migration only after proving source/destination identity and preserving recoverable copies.

## Commit at a verified save boundary

Placement and pack-up mutate the in-memory committed registry and inventory through one transaction contract. Persist at a boundary whose actual completion/failure meaning was verified for the current game.

Writing a sidecar atomically after a native save can still leave a crash window between the two stores. If resource ownership depends on their agreement, use a recovery journal or a verified native-integrated persistence route. Refer to [networked world-state recovery](../../schedule1-networked-world-state/references/networked-world-state.md) for journal invariants; the same coupled-save concern can exist in single player.

On native save failure, retain dirty state and diagnostics; do not report the sidecar as synchronized. Mark data clean only after the intended durable write succeeds.

## Restoration sequence

1. Establish save identity and load/validate state before enabling mutations.
2. Discover which native objects the game will or did restore.
3. For native-owned objects, stage extra data by their persistent identity and attach at a verified readiness point.
4. For mod-owned geometry, create one root and rebuild each missing owned placement from its saved definition.
5. For already-live owned IDs, reconcile instead of spawning another instance.
6. Reapply owned terrain/navigation changes only after geometry and native prerequisites are ready.
7. Complete network linking/late-join synchronization separately from disk restoration.
8. Publish readiness only after invariants and mandatory dependencies pass.

## Runtime teardown versus durable state

Dispose roots, navigation owners, listeners, and runtime caches on the relevant world teardown, including same-save reload. A persistent root is optional and must not leak children across worlds.

Keep pending durable snapshots, dirty state, and recovery records until their save outcome is known. Do not clear all state on every additive scene unload, and do not retain dead Unity objects merely to protect durable data. Filter by the actual gameplay-world lifetime and save transaction.

## Failure and regression matrix

Test: native-restored object plus mod side data; mod-owned object; duplicate IDs; missing definition; corrupt primary with valid/invalid backup; access denied; exact-save mismatch; transition with unknown save identity; repeated rebuild; client attempt to write; interrupted native save; interrupted sidecar save; late network link after scene exit.

Acceptance requires one restored object per ID, no lost/duplicated inventory, no cross-save state, preserved recoverable data, and bounded cleanup. Historical reports that a particular lifecycle event fired zero times are a reason to measure the current event path, not a universal API prohibition.

## Source anchors

- Historical examples: `Source/Mods/AutoPackagingStation/src/AutoPackStore.cs`, `Source/Archive/HomelessMod/src/StreetPropertyManager.cs`, and deterministic TaxiDriver geometry rebuilds.
- S1MAPI source search: prefab placement, network linking, terrain changes, navigation lifecycle, and resource tracking under `ThirdParty/S1MAPI/`.
- [Canonical buildable restoration](../../schedule1-persistence/references/buildable-restore.md).
- [Canonical write freezing](../../schedule1-persistence/references/frozen-writes.md).
