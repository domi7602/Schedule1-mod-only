# Networked World-State Protocol and Recovery

> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 5/14 identifier-shaped tokens resolve (0 documented as absent). External-reference symbols are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Evidence and use

This reference retains architecture observations from the user-provided `BaseBuilder.dll` decompile.
The binary and its runtime behavior were not revalidated during this revision. `LobbySync`,
`WorldDelta`, `BuildRequest`, `BuildReply`, and `SaveSafety` are that mod's external-reference names,
not Schedule I/FishNet APIs. Its lobby-chat transport is not a recommendation to copy.

Bind the following contract to the installed transport, authenticated-sender callback, game
permissions, and save lifecycle. Use existing verified workspace infrastructure where available.

## Bounded message envelope

A request carries protocol/schema version, session identity, stable operation ID, actor, action,
target, and expected base revision. A result identifies the same operation and gives an authoritative
accepted/rejected/pending outcome with a reason and resulting revision. A delta carries base/target
revisions plus changed/removed entities and ordering data when it affects behavior.

Before dispatching mutation code:

- Match the active session and protocol/schema.
- Derive the sender from the transport's authenticated callback and compare it with the actor being
  authorized. A client-supplied player ID is not authentication.
- Enforce permissions, ownership, valid target/entity IDs, action constraints, and resource limits.
- Bound bytes, nesting/collections, fragments, pending groups, reconstructed size, age, and retry rate.
- Reject inconsistent duplicate fragments, malformed fields, non-finite values, and stale generations.
- Expire incomplete groups without treating an expired request as proof that an earlier commit failed.

Inherited evidence: `AuthenticatedBuildingMessagePatch.Prefix` checked sender/routing and `LobbySync`
used message prefixes and fragment limits. The exact callbacks and limits are transport-specific.

## Host commit

Clients submit intent. The host computes the candidate state and resource effects from authoritative
information, checks the expected revision, and validates all resulting invariants. Deduplicate the
operation ID before applying effects; retain the result so an identical retry receives the same answer.
Reject a reused ID with a different request payload.

Stage state changes and a side-effect plan together. Inventory, currency, native object registration,
and world records may not share an atomic API; define reservation, compensation, or recoverable pending
state for each partial-failure boundary. Validating a copied world state does not roll back an already
applied inventory change.

Publish the accepted revision/result only after the required commit effects have succeeded. If a
commit becomes ambiguous, retain the pending operation for reconciliation and prevent conflicting
mutations; do not report rejection and grant a refund while the authoritative effect may still exist.
Persist operation outcome where crash/restart recovery requires deduplication beyond process memory.

Bound the deduplication store using the real retry/session/recovery horizon. Expiring an ID while its
request can still be retried can permit a second mutation; coordinate expiry with session closure,
acknowledged progress, or another verified replay policy.

## Snapshot and delta reconciliation

Apply a delta only to its expected base revision and supported schema. Reconstruct a candidate copy,
validate entity/order invariants, and verify any canonical fingerprint before replacing live state.
Canonicalization must include every behavior-relevant field and deterministic ordering; a content
hash detects mismatched content but does not authenticate its sender.

On a gap, corrupt delta, or incompatible base, request a full authoritative snapshot. Do not partially
apply the delta or guess a merge. Bound snapshot size and transfer work. A joining client's baseline
and subsequent deltas need ordering or buffering so updates are not lost during snapshot installation.

Inherited evidence: `WorldDelta.Create`, `Fingerprint`, and `TryApply` reconstructed/validated copies;
`LobbySync` tracked revision and snapshot fallback behavior. None of that establishes this game's
current transport semantics or makes its file/message format a standard.

## Prediction and unknown outcomes

Track each pending operation's provisional effects and last authoritative baseline. Predicted visuals
can be discarded independently; predicted inventory/currency requires explicit ownership and accounting.

On an authoritative rejection, undo only that operation's provisional effects once, then rebase
remaining predictions onto the current authoritative state. Do not restore an old whole-world snapshot
over later accepted changes. A duplicate response must not apply another mutation or refund.

A timeout or disconnect means the outcome can be unknown. Query/retry with the **same operation ID**
or reconcile against authoritative state before a final resource refund. Keep late accepted responses
resolvable and test out-of-order responses. If the session cannot provide proof, retain a recoverable
pending state and surface that limit instead of guessing success or rejection.

## Interrupted-save recovery

Coupled native and mod files need an explicit recovery transaction even in single player. An atomic
sidecar replacement alone does not make the two stores atomic.

1. Bind an intent to the exact normalized save identity, schema, transaction/operation ID, expected
   prior generation/hash, and proposed mod-state snapshot.
2. Write and durably publish the intent using the repository's verified storage policy before native
   save work begins. Preserve a validated prior copy.
3. Mark native completion only at a success signal verified for the target build. Inspect what that
   signal proves; a callback name or elapsed time is not sufficient evidence.
4. At load, compare identity and native/mod content or generation evidence with the intent. Timestamps
   can support diagnosis but should not alone establish which logical save committed.
5. If native completion is proven and the mod file is exactly the expected prior copy, complete the
   recorded mod snapshot through the verified write path.
6. If the final mod data already matches the committed transaction, resolve the journal without
   applying resource effects or writing the transaction a second time.
7. If identity, completion, prior state, or resulting content is ambiguous, freeze dependent writes,
   preserve all recoverable copies, and request a matching backup or further evidence.
8. Archive/clear the intent only after the final durable state is verified. Runtime teardown must not
   delete the sole recovery snapshot.

Inherited evidence: `SaveSafety.Begin`, `MarkNativeComplete`, `Resolve`, and `Finish` bracketed native
save work with pending intent and a snapshot. Adapt the principle to the canonical persistence skill;
do not copy that third-party format or assume its marker matches the installed lifecycle.

## Diagnostics and meaningful tests

Record operation/session/revision IDs, first rejection reason, duplicate count, stale revisions,
fragment expiry, snapshot fallback, pending outcomes, and recovery decision. Rate-limit repeated
messages and avoid unnecessary sensitive player data.

| Scenario | Required property |
|---|---|
| Concurrent clients and stale base | One ordered authoritative result with valid invariants |
| Identical retry / changed payload with same ID | No duplicate mutation / explicit conflict |
| Lost or delayed ACK | No guessed final refund; same-ID reconciliation |
| Rejection after another accepted edit | Only rejected provisional effects undone |
| Sender/session mismatch | Refused before mutation |
| Oversized/incomplete/conflicting fragments | Bounded memory/time and clean rejection/expiry |
| Side effect fails during commit | Exact compensation or recoverable pending state; no false acceptance |
| Late join during edits | Coherent baseline followed by ordered updates |
| Crash between native and mod save | Content-backed recovery or write freeze |
| Same-slot reload and unrelated save path | Correct identity; no cross-save recovery |

Static decompiles and compile checks cannot establish these outcomes. Report controlled multiplayer
and interrupted-save test results separately from source inspection.
