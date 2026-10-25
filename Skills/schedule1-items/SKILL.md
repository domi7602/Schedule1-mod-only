---
name: schedule1-items
description: "Create Schedule I items and verify inventory delivery."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Item and inventory integration

## Scope and prerequisites

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Resolve the runtime and exact owning assembly before using a vanilla item type.
S1API wrappers and native definitions/instances are different contracts.

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
| Stack limits: lifetime baseline, owned restoration, retry and patchability | [Stack-limit engine](references/stacklimit-engine.md) |
| Cash, hotbar, storage grid, partial transfers | [Capacity](references/inventory-capacity.md) |
| S1API creation and product integration | [Items and products](../schedule1-s1api/references/items-products.md) |
| Native buildable reload | [Buildable restore](../schedule1-persistence/references/buildable-restore.md) |

## Failure handling

Reject invalid definitions and stale native wrappers. Preserve a reason for skipped
items and rate-limit diagnostics. Avoid swallowing exceptions as successful
registration, successful transfer, or completed scan coverage.
When payment and delivery are coupled, follow the
[economy workflow](../schedule1-economy/SKILL.md); do not refund the full amount
merely because a later item failed to transfer.

## Verification

Select relevant checks: a vanilla item, a custom item registered late, duplicate IDs,
excluded and non-stackable items, settings changes, reload, full inventory,
partial stacks, cash overflow, and partial transfer. For world storage, test
actual footprint fit and rotation. For placed objects, test native save/restore
without duplicated instances. Verify only the runtime branches promised by the
project and describe any untested branch explicitly.
