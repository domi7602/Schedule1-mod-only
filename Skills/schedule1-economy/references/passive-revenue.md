# Passive revenue and payout idempotency

> verified: flow from BusinessIncome source (snapshot fix 2026-08-21); Money APIs re-verified against the installed runtime decompiles 2026-10-05.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 9/13 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

Inherited BusinessIncome source notes; not revalidated against the installed game
in this package. The named helpers and old source line numbers are not API
contracts. Find the current implementation before applying a repair.

## Model a payout

Define a payout key from confirmed world identity, the eligible day/period, and
business identity or batch membership. Keep the calculation deterministic for the
same key. A process-local re-entry guard does not provide persistence or rollback
safety. Only the authoritative peer may perform the financial mutation.

The historical business model combined base income, property multiplier,
deterministic variation of approximately plus/minus 15 percent, an employee bonus
of 5 percent per employee capped at 25 percent, an eligible-weekend factor of 1.25,
and operating cost of 10 percent. Treat these as configurable mod policy; preserve
the project's current formula and test rounding and bounds. Do not present them as
vanilla business rules or mix additive and multiplicative versions of the formula.

## Execution and outcome

1. Resolve confirmed identity, eligible businesses, period, and unpaid status.
2. Calculate a bounded preview without mutations; distinguish dry run from payout.
3. Latch an in-flight payout before calling the money provider.
4. Observe whether payment had no effect, its expected effect, a partial effect,
   or an unresolved effect. Preserve pre-attempt metadata for reconciliation.
5. Mark settled only after the financial outcome is established. Persist against
   the compatible native checkpoint, checking the actual writer result.
6. Send optional notifications after settlement without reopening payment on a
   notification, sound, or logging exception.

On confirmed no-effect failure, release or restore only this attempt's pending
state. On unknown effect, retain an unresolved payout and suppress blind repetition.
Never simply reset the paid marker after every exception: the bank may already
have been credited. Never substitute `-1` for a prior paid-day snapshot.

## Checkpoint and recovery

A successfully written payout marker plus an unsaved native bank balance can lose
income after rollback. A saved bank balance plus a missing marker can duplicate it.
Choose and verify a native-coupled persistence design; atomic JSON alone cannot
settle this mismatch. Compare world checkpoint as well as identity during recovery.

A journal may retain evidence and facilitate reconciliation, but it cannot invent
an atomic boundary across an uninstrumented provider and separate native save.
If the provider cannot establish outcome or idempotency, expose the unresolved
state and its recovery procedure rather than claiming exact-once delivery.

For per-business payouts, define whether eligibility changes during a period
create additional payable work. Avoid a single day-paid flag accidentally hiding
unpaid businesses or allowing already-paid ones to repeat.

## Verification

Test no eligible businesses, zero/negative calculated totals, dry run, repeated
same-day callbacks, multiple businesses, late business acquisition, client calls,
and concurrent host triggers. Inject failure before/after money mutation, during
marker publication, and after settlement notifications. Reload the same checkpoint
and an older one; verify balances and payout evidence together.

Related: [purchase outcome states](atomic-purchase.md),
[frozen writes](../../schedule1-persistence/references/frozen-writes.md), and
[slot identity](../../schedule1-persistence/references/slot-isolation.md).
