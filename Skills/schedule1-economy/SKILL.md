---
name: schedule1-economy
description: "Handle Schedule I payments, stock and partial failures."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Economy operations

## Scope and prerequisites

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Inspect the target money, inventory, stock, and save contracts before mutation.
`BankApp`, `PocketShop` and `BusinessIncome` are external workspace examples to inspect.

Identify whether the operation affects local cash, shared bank balance, stock,
world ownership, or several stores. Determine the authoritative peer for each.
Cash storage and capacity are inventory concerns, not merely float arithmetic.

## Workflow

1. Define a stable operation identity and confirmed world context. Reject duplicate
   or concurrent execution of the same operation without repeating its effects.
2. Validate finite positive amounts, quantity, price, fees, stock state, authority,
   and actual inventory capacity. Resolve prices before taking payment.
3. Pre-create and validate required instances before payment where the API permits.
   Keep finite stock, empty stock, and explicit unlimited stock distinct.
4. Track each attempted mutation and its observed outcome: not attempted,
   confirmed no effect, confirmed effect, partial effect, or unresolved effect.
5. Execute through the supported provider. Check the actual return contract and
   independent state evidence where needed; exceptions may follow mutation.
6. Finalize stock, receipts, and payout markers only for the established result.
   Persist them at a checkpoint compatible with the native state they describe.
7. Keep logging, notifications, and UI errors outside the payment retry decision.

## Failure and compensation

- Refund only confirmed deducted value that is not retained as delivered value.
  A failed payment before deduction is not a reason to credit money.
- A failed item loop may have delivered earlier items. Either reverse the exact
  confirmed transfers or settle only the undelivered part. Never refund the whole
  order while keeping its successfully delivered items.
- Record compensation progress. A failed refund can itself have partial or unknown
  effects; do not retry it from an unqualified catch block.
- On an unresolved native outcome, hold the operation for reconciliation. Do not
  reset a payout marker or automatically issue a replacement payment.
- Do not restore a stale total balance over unrelated player activity. Compensation
  concerns this operation's proven effect, not a blind snapshot of all money.

## Select a pattern

| Operation | Reference |
|---|---|
| Purchase with stock and inventory delivery | [Purchase workflow](references/atomic-purchase.md) |
| Supplier debt, split payments and relationship rewards | [Supplier settlement](references/atomic-purchase.md#supplier-payments) |
| Cash to bank or bank to cash | [ATM transfer workflow](references/atm-double-entry.md) |
| Daily business revenue and rollback recovery | [Passive revenue](references/passive-revenue.md) |
| Wrapper names and time representation | [Money and game time](../schedule1-s1api/references/money-economy.md) |
| Capacity and stack behavior | [Inventory capacity](../schedule1-items/references/inventory-capacity.md) |
| Checkpoints, IO failures, identity | [Persistence](../schedule1-persistence/SKILL.md) |

## Verification

Select relevant checks: out-of-stock zero, explicit unlimited stock, invalid/overflowing totals,
insufficient capacity, failure before deduction, failure after mutation, partial
item transfer, failed compensation, duplicate action, and slot/world change.
For recurring revenue test duplicate authoritative callbacks, refused unauthorized calls,
reload, rollback and failed marker persistence. Assert ledger, inventory, stock and receipt
consistency together. Multiplayer sessions require explicit scope.
