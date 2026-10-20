# HitmanPhone — audit repair candidate

Anonymous bounty contracts: accept an offer, eliminate the assigned customer (KO counts),
receive the evidence photo and use the **assigned** dead drop for a cash reward.
Existing reward tiers and delivery windows are retained.

**Experimental audit repair; not gameplay-accepted.** Target: Schedule I 0.4.7f12,
S1API 3.2.1, MelonLoader IL2CPP. Historical 0.2.9 gameplay evidence does not verify this
repair. Builds, model tests and source checks do not establish the native gameplay chain.

## Session and gameplay rules

- **Singleplayer only.** Multiplayer hosts and clients are unsupported; the tracker shows
  that status and contract mutation/receipt paths are gated. This is not an MP protocol.
- Targets are customers assigned to recruited player dealers. Acceptance re-resolves and
  revalidates the target; an already dead/KO target cannot be accepted normally.
- Five caller identities share one message thread, with at most one open offer. Information
  and notices preserve its responses. Stale session callbacks are rejected. Failed delivery
  is deferred with a bounded retry budget and is not counted as a successful call.
- Planned and successful calls respect at most two per game day and 210 game minutes
  minimum spacing. Longer caller cooldowns are never shortened by a later short cooldown.
- Elimination recognition and photo delivery are separate. A full inventory leaves pending
  delivery, serviceable without another attack or renewed kill heat.
- Missing evidence is repaired only after a complete, unambiguous scan. An unreadable
  container/value or ambiguous legacy-zero photo defers repair instead of creating a copy.
- New photos use positive persistent tokens. Legacy item IDs and constrained legacy matching
  remain. A wrong-drop photo stays in place and cannot block a later valid deposit.
- One deadline rule applies at recognition and payout: eligible **before** the due point,
  expired **at or after** it. The short drop window starts at actual photo delivery.
- Acceptance does not intentionally lower stronger pursuit. Recognition retains lethal heat;
  photo repair is not a fresh elimination. The reward remains cash.
- Journal objectives are structural. Quest binding uses a contract-derived persistent GUID,
  not display text or the first unbound quest. Unknown old quests are not fabricated successes.
- Tracker evidence, KO, confirmed death and contract status are distinct. Two completed
  contracts on one target count as two contracts, not two distinct eliminated targets.
- A native cash-call exception has an unknown effect. The claim/photo is retained with
  `RewardPaymentUncertain` and a tracker warning; automatic payment retry, expiry and death
  forfeiture are disabled for that claim. Review balances/logs manually; do not blindly clear
  the flag or repay. A normal void API return is not independent proof that money moved.

## Saving and migration

Gameplay changes remain in memory until a normal native world save. Save-start freezes
the bounty snapshot; save-complete verifies identity, native save result and destination
before writing the matching checkpoint. No extra unconfirmed save request is substituted
for a transaction.

- Checkpoints: `UserData/HitmanPhone/checkpoints/<identity-hash>/<world-hash>.json`.
- Compatibility copy: `UserData/HitmanPhone/bounties_slot_{n}.json`.
- Native world JSON content determines the fingerprint; `Metadata.json`, `Modded/` and
  `Backups/` are excluded. A primary/backup fallback requires both exact identity and hash.
- Unknown/corrupt checkpoints pause HitmanPhone rather than guessing or losing a claim.
  Loading never moves, deletes or immediately rewrites original legacy data. Null lists
  and rows are normalized before use. Tokens/legacy IDs and `IncludeFields=true` remain.

An old write-through file cannot retrospectively prove alignment with the native world:
its first import is a legacy assumption, not verified rollback recovery. No atomic
transaction spans engine and mod filesystem; failure between writes may require manual
checkpoint recovery. Root-level JSON sidecars from other mods or load-time game upgrades
can change the fingerprint and deliberately pause HitmanPhone.

## Verification and developer commands

See [audit-repair-verification.md](audit-repair-verification.md) for per-report-ID mapping,
test evidence, environment and the backed-up singleplayer acceptance checklist. Native
save paths/event ordering, fingerprint reload stability, pre-restore registration, message
responses and real cash/item changes remain unverified. Long-save performance is unmeasured;
no history truncation or broad optimization is part of this repair.

`/hitman_force_offer`, `/hitman_status`, `/hitman_kill`, `/hitman_reset` and
`/hitman_cleanup` are **DEBUG-only**, not the Release acceptance route. DEBUG fallbacks
cannot prove normal offer eligibility. Reset retains identity and the token high-water
mark; use a normal world save to commit it. Do not use reset as save recovery.
