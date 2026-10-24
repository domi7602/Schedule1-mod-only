# FishNet SyncVar Diagnosis — Authority and Cache Staleness

> verified: 2026-10-05 — decompile facts in §3 are grep-verified against `GameReferences/decompiled/Assembly-CSharp/` (older game build proxies, regenerated 2026-10-03). The onChangeCallback surface (§4 Option A) and ALL host/client runtime behaviors are **unverified** — no in-game multiplayer test was run for this document. FishNet / S1API.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 23/30 identifier-shaped tokens resolve (5 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

> Evidence scope: inherited notes describe proxy members inspected in October 2026.
> The underlying assemblies and multiplayer traces are not bundled. Member names below
> are search leads; current field permissions and replication behavior require verification.

## Start with the concrete symptom

A client view can become stale because of a local cache, missed notification, unready world,
wrong object identity, rejected request, prediction/reconciliation, or later replicated state.
A value snapping back is not by itself proof of a particular SyncVar permission setting.

## Inspect the current field path

1. Identify the exact object, field/property, world generation, and network role.
2. Read its declaration and generated accessors in matched game and FishNet references.
3. Determine who may write, what requests are accepted, and how clients receive changes.
4. Locate the available change notification or read path in the installed wrapper.
5. Verify which path actually fires for local and replicated changes before relying on it.

Historical proxy names include `syncVar____<field>`, `sync___get_value__<field>`,
`sync___set_value__<field>`, and per-class `ReadSyncVar` variants. They do not prove that
every field uses the same route or that a patch on a setter sees replicated writes.
Do not invent `onChangeCallback`, `WritePermission`, or a particular permission enum value
if the installed source/schema does not expose it.

## Bounded host/client observation

Label each trace with process/session, role, object identity, and elapsed timestamp.
Do not label a host alone as offline singleplayer based only on player count.
Record the request, host acceptance/rejection, host state, client received state, and UI cache.
Correlate using the actual object's persistent/network identity and a request identifier
where the protocol provides one. Clock alignment uncertainty must remain visible.

Read-only observation comes first. If an authorized mutation test is necessary, use a
controlled world, capture before-state, and restore it through the appropriate authority.
A missing callback or timed-out read leaves a gap; it does not prove the state was unchanged.

## Cache and UI policy

- Cache values with an explicit invalidation/update contract, not a blanket ban on caching.
- Reconcile after verified change notifications or bounded read-through where suitable.
- `OnLoadComplete` may initiate readiness checks; it is not a universal replication barrier.
- Ignore late results from a previous world/session/object generation.
- Keep client UI rendering local, while authoritative game mutations use the intended owner.
- A local write that does not reach the host requires contract inspection; do not infer its
  exact permission enum from that symptom alone.

## Patch choice

Prefer a supported notification or domain hook over a generated/private accessor.
If a native accessor must be patched, verify exact signature, call route, and patchability
for this runtime. Do not patch a broad serialization method without scoping objects and cost.
Instrumentation failure must remain distinguishable from game-state mutation failure.

## Required tests

Test host, connected client, late join, leave/rejoin, same-world reload, rejected request,
rapid successive updates, and world transition while a read or request is pending.
Observe both accepted changes and intended nonchanges. Record trace gaps and scan limits.

## References

- [Multiplayer system notes](../../schedule1-game-systems/references/45-Multiplayer.md)
- [Networked world-state contract](../../schedule1-networked-world-state/SKILL.md)
- [Save/load timing](save-load-timing.md)
- [Harmony bootstrap](../../schedule1-harmony-bootstrap/SKILL.md)
