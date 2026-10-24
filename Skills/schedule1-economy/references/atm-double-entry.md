# Cash and bank transfers

> verified: flow from BankApp source; MoneyManager APIs re-verified against the installed runtime decompiles 2026-10-05.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 5 of 5 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Inherited BankApp source notes; not revalidated against the installed game in this
package. Verify the installed cash, bank, inventory, fee, and authority contracts.

## Define both legs

| Transfer | First intended effect | Second intended effect |
|---|---|---|
| Deposit | Remove the accepted cash amount | Credit bank with amount minus deposit fee |
| Withdrawal | Debit bank with amount plus withdrawal fee | Deliver the accepted cash amount |

This is a compensated workflow across two representations, not an engine-provided
atomic transfer. Use a stable operation identity and record the result of each leg.

## Preflight

- Reject nonpositive/nonfinite amounts and invalid fee settings. Define rounding
  and minimum/maximum rules once; keep the UI preview and execution identical.
- Check cash availability for deposit and bank affordability including fees for
  withdrawal. Do not bound withdrawal by the displayed amount while ignoring fees.
- Query actual cash capacity. Account for existing cash stacks and eligible
  inventory slots according to the target build; do not assume a fixed slot layout.
- Resolve the weekly deposit policy and its native/mod ownership. The inherited
  BankApp used a 10,000 cap and day-based week buckets; these are historical policy
  values, not universal API guarantees. Check day origin and boundary behavior.
- Serialize conflicting transfers and establish the authoritative peer before
  relying on a balance observation.

## Failure handling

Track whether the first leg was attempted and what it actually did. If it failed
with confirmed no effect, stop without refund. If it succeeded and the second leg
failed with confirmed no effect, compensate the first proven effect once.
If either effect is partial or unknown, reconcile before retry or compensation.
An exception is not proof that a provider did nothing.

Do not wrap both calls in one catch that unconditionally adds cash. Do not restore
an old absolute balance over unrelated transactions. Track a failed compensation
as its own unfinished operation and keep the original cause.

Record weekly deposits, transaction history, and UI success only for established
settlement. Preserve sufficient pending state for recovery without counting the
same deposit twice. Do not mark money delivery successful solely because a void
method returned normally.

## Verification

Test deposit and withdrawal with and without fees, exact capacity, no capacity,
partial stacks, boundary amounts, invalid configuration, weekly rollover, and
insufficient bank funds after fees. Inject failure before and after each native
mutation, then during compensation and receipt persistence.
Also test duplicate input, concurrent money activity, host/client execution,
world switch, older native checkpoint, and restart during an unresolved transfer.

Read [purchase failure states](atomic-purchase.md) for the shared settlement model
and [persistence](../../schedule1-persistence/SKILL.md) for checkpoint alignment.
Historical anchors: `BankService.cs` and `TransactionHistoryService.cs` in BankApp.
