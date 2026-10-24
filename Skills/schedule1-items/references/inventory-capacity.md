# Inventory and storage capacity

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 5 of 5 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Inherited BankApp, PocketShop, and native-storage notes; not revalidated against
the installed game in this package. Query the actual target inventory layout.

## Distinguish storage models

| Store | Capacity concerns |
|---|---|
| Player inventory/hotbar | Eligible slots, existing compatible stacks, item restrictions |
| Dedicated cash slot and overflow | Cash-specific limits plus eligible remaining space |
| World storage grid | Footprint dimensions, locked cells, rotation, item-specific rules |
| Shop stock | Availability; independent from the player's ability to receive items |

The inherited layout notes used eight hotbar slots (0–7), clipboard slot 8, cash
slot 9, and a 1,000 cash-stack limit. These are historical observations to verify,
not universal constants or a reason to count the clipboard as a ninth hotbar slot.

## Fit and transfer

1. Obtain a valid representative instance from the exact definition.
2. Query the supported capacity/fit API for the intended quantity and metadata.
3. Ensure the fit query and transfer use the same item identity, packaging, quality,
   and quantity interpretation.
4. Revalidate if inventory could change before transfer, or use the provider's
   supported reservation/serialization mechanism.
5. Observe actual accepted quantity. Preflight success does not guarantee that
   every later mutation succeeds or that a failed call had no partial effect.

For cash withdrawal include fees in bank affordability and actual cash capacity in
receivable amount. For grid storage, inspect the target footprint-fit method; do
not invent a generic `CanFit` member or apply hotbar arithmetic to tile storage.

## Partial failure

Keep evidence of confirmed delivery separately from the attempted quantity.
A multi-item transfer may fail after earlier items succeeded. Follow the
[economy settlement workflow](../../schedule1-economy/references/atomic-purchase.md)
when money is involved. Do not refund all money while retained items remain, and
do not remove arbitrary pre-existing matching items as rollback.

## Verification

Test an empty inventory, partially filled compatible stacks, incompatible metadata,
non-stackable items, restricted slots, full inventory, exact-fit and one-over-fit
quantities, cash overflow, and a transfer failure after partial acceptance.
For world storage, include rotated footprints and locked-cell interference.
Verify the resulting inventory, not merely the fit-query Boolean or absence of
exceptions.
