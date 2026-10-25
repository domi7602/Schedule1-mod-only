---
name: schedule1-grid
description: "Build Schedule I grids and validate placement."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Schedule I — Grid and Placement

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Treat historical HomelessMod recipes as one implementation model, not universal native behavior.

## Choose the ownership model first

| Model | Network policy | Restore policy |
|---|---|---|
| Native registered buildable | Preserve required native registration | Native save restoration; mod attaches extra state |
| Mod-owned persistent world object | Host-authoritative mod protocol when shared | Mod registry and explicit rebuild |
| Local visual preview or cosmetic | No gameplay authority | Recreate/dispose locally |

Do not strip FishNet or navigation from a shared native object to make a local placement recipe work.
Load [networked world state](../schedule1-networked-world-state/SKILL.md) when mod-owned placements must replicate.

## Establish prerequisites

- Inspect current `BuildUpdate_Grid`, `GridItem`, `BuildableItem`, `BuildManager`, and grid/property contracts.
- Resolve scene readiness, host/client role, exact save identity, item definition, and object ownership.
- Find the selected mod's placement validator, registry, and inventory transaction interface.
- Confirm which native callbacks must remain and which may be narrowly intercepted.

## Workflow

1. Keep native valid placement as the normal path unless the feature explicitly changes it.
2. Evaluate custom ghosts using bounded camera/ground queries, footprint sampling, slope and reach limits, and collision clearance.
3. Treat a saturated non-alloc collision buffer as incomplete evidence; retry with a bounded larger buffer or reject placement.
4. Revalidate on commit; a previously valid ghost is not permission to place after the scene or inventory changes.
5. Stage the clone inactive and configure identity, ownership, collider policy, and interaction before activation.
6. Preserve source-prefab state and restore it on every exit path if temporary source changes are unavoidable.
7. Commit registry and inventory changes through an exact-once transaction; failure leaves no unpaid object or consumed item without an object.
8. Reset placement state on completion, cancellation, exception, or ownership change.
9. Pack up only after confirming the destination inventory accepted the full return. Never destroy the source merely because an add call was attempted.
10. Persist at verified save boundaries, with load-result gates and exact save identity. A separate atomic JSON write does not make two stores one transaction.

## Lifecycle and failure handling

- For the historical local outdoor model, disabling a required `GridItem` may be safer than destroying a component still referenced by native code; verify the actual callbacks.
- Scope Harmony guards to proven owned instances. Unrelated buildables must retain vanilla behavior.
- Keep a virtual mod root separate from a real property. A real property requires its full native registration contract.
- Clear runtime objects and caches at relevant world teardown; retain durable state needed by an in-flight save until its outcome is known.
- Reject invalid/stale pointers and owner generations before dereferencing, while recognizing that pointer checks are not proof of safe native lifetime.

## Verify

Select relevant checks: native/custom placement, blocked/full inventory, duplicate requests,
cancellation, curbs/slopes, masked layers, footprint visibility, culling, pack-up failure,
reload, slot switch, interrupted saves and cleanup. Host/client and late-join tests require explicit scope.

## Read on demand

| Trigger | Reference |
|---|---|
| Native interception, transaction, save timing | [Build-update patching](references/build-update-patching.md) |
| Virtual root, GridItem, footprint policy | [Outdoor placement](references/outdoor-placement.md) |
| Raycasts, collision, ghost and pack-up tests | [Collision and ghosts](references/collision-and-ghosts.md) |

## Evidence and output

Report ownership, intercepted targets and transaction/restore responsibilities under the shared
evidence policy. A singleplayer example does not prove multiplayer or anti-duplication support.
