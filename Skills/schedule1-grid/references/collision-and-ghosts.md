# Placement Ghost Evaluation, Collision, and Pack-Up

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 33 of 33 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use for ground placement, slope/reach checks, collision clearance, ghost feedback, and reclaiming
outdoor items. The inherited helpers and thresholds came from one older HomelessMod implementation.
Treat them as design leads and verify the current game physics layers, camera, input, and item sizes.

## Evaluate a coherent candidate

Resolve the active placement owner, valid selected item, current session, camera, ghost root, and
footprint. Reset the custom-valid result on selection changes, cancellation, stop, exceptions, and
session changes. A valid result from a previous item or frame must not authorize a new placement.

Use a camera ray with deliberate layer/trigger rules. The old scene had street surfaces on a `Grid`
layer, so excluding that layer removed legitimate ground. Inspect current collider layers rather than
copying a broad mask. Ignore the player's and ghost's own colliders by verified ownership, not a global
name. A fallback down-ray must still satisfy reach, slope, and obstruction rules.

Sample the center and meaningful footprint corners at the candidate rotation. Reject gaps, excessive
height variation, unsupported overhangs, or slopes unsuitable for the item. Use the resulting support
plane and the asset's pivot/footprint to derive a final pose; a ray hitting one corner is not enough.
The old 45-degree slope and 0.3–7.0 distance limits were tuning choices, not established game rules.

## Clearance checks

Test a volume derived from the actual occupied shape, with a deliberate margin. The old 0.85 extents
factor can miss real collisions if applied without considering the asset's clearance requirements.
Separate allowed support contact from obstructing walls, actors, other items, and trigger volumes.

For `OverlapBoxNonAlloc` or another fixed-buffer query, handle saturation explicitly. A result count
at the buffer's capacity may be incomplete; grow/retry within a bound or conservatively reject with
an actionable diagnostic. Never infer clear space from only a truncated subset of colliders.

Keep evaluation and visuals separate: one candidate result contains pose, validity, and reason. Render
the ghost from that result using owned materials/overrides. Preserve vanilla placement when it owns
the attempt, and revalidate a custom result at commit rather than trusting a stale green ghost.

## Pack-up is a transaction

Resolve a registered owned object from the actual hit, confirm the player/session/authority and
interaction range, and ensure the item is not concurrently reserved, moving, or already being removed.
Use the game's binding/input policy; the old F and 0.40-second right-button hold were one design.
Cancel hold progress on release, focus loss, target change, menu/modal interruption, or teardown.

Before removing the world object, prove that the inventory operation can accept the complete return
item or use a verified reservation/transaction mechanism. Check actual accepted quantity and failure
behavior. A console fallback that silently creates an item elsewhere is not equivalent to successful
inventory delivery.

Commit one operation with a stable ID where retries are possible: secure item transfer, remove the
authoritative world record, then publish the new state and release the runtime object. Define
compensation for every partial failure. Do not simply call `AddItemToInventory`, unregister, and
`Destroy` without confirming acceptance and guarding reentry. Pending/unknown network outcomes need
reconciliation, not an automatic second item grant.

## Persistence and runtime cleanup

Read [Placement lifecycle](build-update-patching.md) and the persistence skill for native/sidecar
coordination. Saving only at `OnSaveComplete` is not a crash-proof anti-duplication mechanism. Test
interruption between inventory transfer and world-state save, and distinguish an intentional pack-up
from scene teardown that should preserve the durable object record.

For a custom HUD, cache reusable styles/resources and update only changed content. String formatting
and layout can still allocate; do not promise a zero-allocation IMGUI path without measurement.

## Verification matrix

| Case | Expected result |
|---|---|
| Curb, rotated footprint, slope, gap | Consistent valid/invalid reason and correct support pose |
| Player/ghost collider | Excluded by ownership without hiding other obstructions |
| Full overlap buffer | Retry or conservative refusal, never a false clear result |
| Item changed/cancelled | Old custom-valid flag cannot commit |
| Full inventory/partial acceptance | World item preserved or exact compensation; no duplication |
| Double click/repeated message | One authoritative pack/placement operation |
| Disconnect/crash/save interruption | Defined reconciliation with no guessed success/refund |
| Scene/slot reload | One restored instance according to its designated restore owner |
