---
name: schedule1-networked-world-state
description: >-
  Design authoritative multiplayer world edits, validated state sync, and save recovery for Schedule I.
  Use when clients place, remove, or edit persistent world objects together.
  Keywords: multiplayer, FishNet, authority, world state, delta, revision, snapshot, recovery journal.
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). This guidance was informed by static inspection of user-provided third-party assemblies on 2026-10-08, not runtime verification; re-check live transport/game APIs before implementation.

# Schedule I — Networked World State

Use this skill for multiplayer edits that must remain consistent across host, clients, and save/load. It complements `schedule1-persistence`, `schedule1-grid`, and `schedule1-economy`; always prefer the current FishNet/S1API surfaces and workspace patterns over a third-party transport implementation.

## When to Use

- Replicating persistent building, placement, furniture, or world-layout edits.
- Handling late joiners, out-of-order edits, rejected operations, or client prediction.
- Protecting a native save plus mod-owned world data from interruption.

**Don't use for:** read-only sync or an ordinary single-player save with no network state.

## Procedure

1. **Choose one authority.** The server/host validates and commits gameplay state. Clients submit intent; never trust a client-supplied price, ownership, inventory, or resulting world snapshot without server-side checks.
2. **Define a bounded protocol.** Include protocol/schema version, session/lobby identity, stable request ID, actor identity, action, target identity, and the expected base revision. Set payload and fragment limits, expiry, and rate limits. Reject unknown versions and malformed identities.
3. **Authenticate the transport envelope.** Validate the current lobby/session and sender against the transport callback’s sender, then bind that identity to the decoded request. Reject packets from absent/mismatched senders before changing state; do not rely on a player name or packet field as authentication.
4. **Make edits idempotent and transactional.** Deduplicate request IDs. Validate ownership, permissions, geometry, item costs, and limits on the host. Apply against a copied/staged state, run invariants, and only then publish the new revision and perform side effects. A rejected edit must leave state and resources unchanged.
5. **Prevent divergence.** For a delta, require the expected base revision and—when practical—a canonical base fingerprint. Validate protocol, reconstructed result, ordering, and domain rules before commit. On mismatch, reject and request a full authoritative snapshot rather than guessing or silently merging incompatible state.
6. **Handle client prediction explicitly.** Keep a before-snapshot for each pending local edit. On acceptance, reconcile to the authoritative result; on rejection or timeout, restore the snapshot and refund only a cost that was actually taken. Make late replies idempotent so a delayed ACK cannot apply/refund twice.
7. **Support joining and fragmentation safely.** Send a versioned full snapshot to a joining client before incremental edits. Bound fragment count and total bytes, expire incomplete assemblies, reject duplicate/conflicting chunks, and log/fall back to a fresh snapshot if reconstruction fails.
8. **Protect saves with a journal when native and mod data are coupled.** Before invoking a native save, atomically write an intent containing the exact save identity and a recovery snapshot. Mark native completion only at a verified success point. On next load, compare the save path and persisted files with the intent; complete only an unambiguous recovery, otherwise stop further writes and preserve the evidence for user recovery.
9. **Instrument state transitions.** Track accepted/rejected requests, duplicate IDs, stale revisions, delta/snapshot fallbacks, pending edits, timeouts, and save recovery outcomes. Rate-limit repeated warnings without hiding the first cause.
10. **Verify end-to-end.** Test host-only, two clients, late join, concurrent edits, disconnect during edit/save, stale base, malformed/oversized packet, lost/delayed ACK, rejected purchase, and slot changes. Confirm exactly one durable result and no duplicated/refunded resources.

## Pitfalls

- A session ID is not authentication by itself; validate the actual sender and current lobby.
- Hashes only detect divergence if serialization is canonical and all relevant state is included.
- Never mutate live state before all checks pass. Catching an exception after partial mutation is not rollback.
- A revision counter without gap handling does not solve out-of-order delivery.
- Do not copy a third-party protocol, Harmony patch, or Steam transport wholesale. Its packet framing and sender hooks may be version-specific or unsafe.
- A pending save journal must fail closed when ownership, path, or native completion is ambiguous; do not overwrite the only recoverable copy.

## References

- `references/networked-world-state.md` — protocol, reconciliation, and save-journal checks.
- `../schedule1-persistence/SKILL.md` — `SafeStorage`, save-slot isolation, lifecycle timing.
- `../schedule1-grid/SKILL.md` — placement validity and buildable lifecycle.
- `../schedule1-economy/SKILL.md` — transaction ordering and host-only money mutations.
