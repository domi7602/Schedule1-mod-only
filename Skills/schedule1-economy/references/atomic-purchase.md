# Purchase settlement and partial failure

> verified: sequence from PocketShop source; MoneyManager APIs re-verified against the installed runtime decompiles 2026-10-05 (ChangeCashBalance/CreateOnlineTransaction, see SKILL section 7).
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 5 of 5 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Inherited PocketShop source notes; not revalidated against the installed game in
this package. This filename is retained for compatibility. A payment followed by
several inventory calls is not automatically an atomic transaction.

## Validate before mutation

- Confirm operation identity, world context, authoritative peer, item definition,
  integral positive quantity, finite price/fees, and supported payment mode.
- Keep stock states distinct: zero is sold out; a positive value is finite stock;
  only the provider's explicit unlimited sentinel means unlimited. Reject unknown
  negative states instead of converting every nonpositive value to unlimited.
- Bound quantity and total calculation; reject overflow, NaN, infinity, and an
  unexpected negative total. Freeze the approved price for the operation.
- Check affordability and actual inventory fit. In Auto mode, use the documented
  payment policy; do not silently combine balances when the UI promises fallback.
- Pre-create and validate all deliverable instances when supported. Dispose only
  unused resources owned by the operation. Creation success is not delivery.

## Execute with explicit outcomes

1. Latch the operation before the first fallible provider call so duplicate
   callbacks cannot start another payment.
2. Attempt payment and establish its outcome from the provider contract and
   appropriate state evidence. A thrown call may already have deducted funds.
3. Transfer items while tracking confirmed delivery per instance/quantity.
   If a call has an unknown result, mark that part unresolved.
4. Reconcile stock for the actual retained delivery, following authority and
   concurrency rules. Reserve or revalidate stock where competing purchases can occur.
5. Complete the receipt only when money, retained items, stock, and context agree.
   Treat notification/logging failure separately from settlement failure.

## Settlement decisions

| Established state | Action |
|---|---|
| Payment definitely not applied | No refund; no delivery |
| Payment applied, nothing delivered | Refund the confirmed refundable amount or retry only known-undelivered work |
| Payment applied, some items delivered | Reverse exactly those transfers if supported, or settle/refund only the undelivered portion |
| Payment or last delivery effect unknown | Hold for reconciliation; no blind refund or duplicate transfer |
| Refund effect unknown | Retain unresolved compensation; do not run it again automatically |
| All intended effects confirmed | Record one completed receipt and the matching stock change |

A catch around the entire delivery loop must not refund the entire order while
earlier items remain with the player. Removing arbitrary matching item IDs is not
a safe rollback if they could belong to the player's pre-existing inventory.
Define fee treatment for cancellation and partial delivery explicitly.

## Save boundary

A receipt or journal must describe the same world checkpoint as money, inventory,
and stock. A sidecar-only completed marker can suppress a legitimate payment after
native rollback; a lost marker can permit duplication. Use the project's verified
checkpoint/recovery design and admit when an exact-once guarantee is unavailable.

## Verification

Cover stock 0, finite stock, unlimited stock, oversized quantity, insufficient
funds, full inventory, failure before payment, failure after payment mutation,
first/middle/last delivery failures, partial native acceptance, failed refund,
double click, host/client race, stock mutation failure, save rollback, and reload.
Assert all four outcomes: money, retained items, stock, and operation state.

The general [compensating transaction pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/compensating-transaction)
requires accounting for completed work and compensation progress. Applying it to
these native game calls is a design inference, not proof of their behavior.
