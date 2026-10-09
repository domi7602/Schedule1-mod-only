> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 5/14 identifier-shaped tokens resolve (0 documented as absent). External-reference symbols are listed at the end of this file. Runtime behaviour is not covered by this sweep.

# Networked World-State Protocol and Recovery

This reference distills design checks from static inspection of the user-provided `BaseBuilder.dll`. Its custom lobby-chat transport is an example to inspect, not a recommendation to copy. Use the current Schedule I/FishNet APIs and verify the callback contracts against live assemblies.

## Message envelope

Keep message schemas explicit and bounded. A request/reply should carry a protocol version, session, stable operation ID, actor/player identity, action/target, expected revision, and an accept/reject result with a reason. A state delta should carry base/target revisions, schema, changed and removed entities, and optional order data where ordering is meaningful.

Before routing a decoded request:

- Confirm the active lobby/session matches the envelope.
- Compare the transport-provided sender identity with the packet callback and the actor identity being authorized.
- Reject absent, mismatched, stale, or unauthorized senders before calling mutation code.
- Limit serialized bytes, chunk count, age, and retry frequency. Expire incomplete chunk groups.

Static evidence: `BaseBuilder.AuthenticatedBuildingMessagePatch.Prefix` reads the transport sender, checks lobby routing, and rejects malformed packets; `LobbySync` defines request/reply/fragment prefixes and limits. These details are transport-specific and must not be copied verbatim.

## Host transaction and reconciliation

Treat client input as intent. On the host, validate identity, permissions, resource cost, placement rules, and the expected base revision. Build the candidate in a copy or transaction buffer; validate all resulting invariants; publish the state/revision only after validation. Cache completed request IDs so retries do not duplicate mutations.

For deltas, require the protocol and base revision to match. If a canonical fingerprint is used, canonicalize ordering and include every field that affects behavior. Apply to a copy, check the resulting fingerprint and game-specific rules, and replace live state only on success. A failed delta should request a full authoritative snapshot; it should not partially mutate or heuristically merge.

For local prediction, retain the before-state and pending resource changes per operation. Reconcile from the host response; restore/refund exactly once on rejection or expiry; ignore duplicate or late responses already resolved. Test disconnects and delayed ACKs explicitly.

Static evidence: `BaseBuilder.WorldDelta.Create`, `Fingerprint`, and `TryApply` use protocol/revision/hash checks and validate a reconstructed copy. `BaseBuilder.BuildRequest` and `BuildReply` carry operation/session identity and response status. `BaseBuilder.LobbySync` tracks local/late edits, accepted replies, owed kits, revisions, and snapshot/delta fallback counters. The underlying implementation has not been validated in a running session.

## Interrupted-save recovery

When mod state must stay aligned with a native save, record an atomic pending intent before native save work. Bind the intent to the normalized exact save path and a snapshot of mod state. Mark native completion only after a success signal that has been verified for the current game. At load, compare the intent with the target save and relevant native file timestamps/content:

- If the native save definitely completed and the mod file is exactly the prior copy, safely finish from the stored snapshot.
- If the final mod data already matches, archive/clear the intent without writing again.
- If the save path, completion point, or resulting data is ambiguous, fail closed; preserve the intent and ask for a matching full backup rather than overwriting.
- Archive resolved/abandoned records for diagnosis; never delete the only recovery snapshot before the final state is verified.

Static evidence: `BaseBuilder.SaveSafety.Begin`, `MarkNativeComplete`, `Resolve`, and `Finish` maintain a pending intent and snapshot around native save activity. Adapt the principle to workspace `SafeStorage` and verified lifecycle hooks; do not reuse the third-party file format or assume its completion marker maps to the current game.

## Diagnostics and test matrix

Log one clear reason for every rejection class and count stale revisions, duplicate requests, missing fragments, snapshot fallbacks, and pending edits. Include session/revision/operation IDs in diagnostics, but avoid logging credentials or sensitive player data.

Test at minimum: host/client authority, late join, identical retry, duplicate request, stale base, missing delta, corrupted hash, invalid entity/order list, sender mismatch, oversized/incomplete fragments, rejected purchase, late response, disconnect during save, same-slot reload, slot switch, and recovery with an unrelated save path.

## Evidence scope

The observations above come from one user-provided decompiled binary and are not runtime verification. ILSpy output can contain missing-reference or unresolved-IL annotations. Confirm every field, method, transport behavior, save hook, and invariant against the current live game and a controlled multiplayer test before relying on it.

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

The identifiers below come from mod assemblies that are **not part of this repository**. They were read from external decompiles, so they cannot be resolved against the game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as external-reference symbols, not as game-API claims, and re-verify them against those mods before relying on them.

- `AuthenticatedBuildingMessagePatch`
- `MarkNativeComplete`
- `BuildRequest`
- `Fingerprint`
- `BuildReply`
- `LobbySync`
- `WorldDelta`
- `BaseBuilder`
- `SaveSafety`
