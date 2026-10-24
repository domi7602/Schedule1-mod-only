---
name: schedule1-items
description: "Create Schedule I items and verify inventory delivery. Keywords: ItemDefinition, BaseItemDefinition, BaseItemInstance, Registry, GetAllItems, StackLimit, Inventory, PlayerInventory, CashSlot, StackLimitMod, HomelessMod, sleepingbag, GetDefaultInstance, BuildableItemDefinition."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Item and inventory integration

## Scope and prerequisites

Use the [working contract](../schedule1-modding/references/working-contract.md).
Resolve the runtime and exact owning assembly before using a vanilla item type.
S1API wrappers and native definitions/instances are different contracts.
Inherited workspace notes are research leads, not installed-game verification.

## Workflow

1. Identify whether the change concerns a definition, an instance, an inventory
   slot, a storage-grid footprint, a shop listing, or a placed world object.
2. Inspect the relevant target API and its ownership. Resolve by stable item ID;
   do not infer packaging, quality, or buildability from name substrings.
3. Register custom definitions before their saved instances are restored. Make
   registration idempotent and verify what survives a scene transition.
4. For modified vanilla definitions, clone when the change is meant to create a
   distinct item; otherwise record the original value and mutation ownership.
5. Discover definitions at the correct load boundary and catch late registration.
   Avoid full global scans in per-frame paths; deduplicate bounded scan work.
6. Evaluate actual capacity before transfer, then observe the transfer result.
   Preflight does not make a multi-item transfer atomic.
7. Reacquire scene-dependent objects after reload; restore or release only state
   and Unity resources that the mod owns.

## Select a reference

| Need | Read |
|---|---|
| Registry lookup, late items, packaging classification | [Registry and discovery](references/registry-and-scan.md) |
| Definition and instance stack limits | [Stack-limit engine](references/stacklimit-engine.md) |
| Cash, hotbar, storage grid, partial transfers | [Capacity](references/inventory-capacity.md) |
| S1API creation and product integration | [Items and products](../schedule1-s1api/references/items-products.md) |
| Native buildable reload | [Buildable restore](../schedule1-persistence/references/buildable-restore.md) |

## Stack-limit policy

Keep a baseline for the confirmed definition lifetime, not a permanent assumption
about all future objects sharing its ID. Respect excluded IDs, original
non-stackability, item-specific restrictions, and bounds before writing.
A failed mutation must remain retryable; discovering an ID is not successful
application. Restore a previous mod override when settings exclude it, where the
ownership policy permits. Do not overwrite a newer change made by another mod.

Treat inherited field-accessor patch failures as a diagnostic lead. Inspect the
actual member and patchability; do not apply a universal rule that all properties
are unpatchable. A definition scan and a real instance-method hook serve different
purposes and each requires an independently verified target.

## Failure handling

Reject invalid definitions and stale native wrappers. Preserve a reason for skipped
items and rate-limit diagnostics. Avoid swallowing exceptions as successful
registration, successful transfer, or completed scan coverage.
When payment and delivery are coupled, follow the
[economy workflow](../schedule1-economy/SKILL.md); do not refund the full amount
merely because a later item failed to transfer.

## Verification

Test an existing vanilla item, a custom item registered late, duplicate IDs,
excluded and non-stackable items, settings changes, reload, full inventory,
partial stacks, cash overflow, and partial transfer. For world storage, test
actual footprint fit and rotation. For placed objects, test native save/restore
without duplicated instances. Verify only the runtime branches promised by the
project and describe any untested branch explicitly.
