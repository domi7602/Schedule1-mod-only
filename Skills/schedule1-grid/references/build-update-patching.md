# Placement Interception, Lifecycle, and Ownership Guards

> verified: guards + anti-dupe live-verified via HomelessMod (older game build); lifecycle section re-verified 2026-09-29 against the instrumented run.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 40 of 40 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use when intercepting `BuildUpdate_Grid`, native buildable lifecycle, or save/load transitions.
The inherited HomelessMod patterns were exercised on an older build and instrumented on 2026-09-29.
The later 2026-10-08 identifier sweep was static. Reconfirm current method signatures and call order
against the installed game; old patch names are search anchors, not a runtime compatibility claim.

## Select one placement architecture

| Ownership model | Spawn/network behavior | Restore owner |
|---|---|---|
| Native registered buildable/property | Preserve required native components and authoritative spawn | Native save/restore |
| Mod-owned world entity | Explicit custom registration, authority, and replication contract | Mod sidecar/custom state |
| Local visual or placement ghost | No gameplay authority; disable unwanted behavior on that clone | Rebuilt from current state |

Do not call `DisableNetworking` and `DisableNavigation` on every outdoor clone. Those historical
BuildManager helpers are candidates only when the selected model deliberately replaces those systems
or the clone is a local visual. Removing FishNet identity from a functional native buildable can break
replication and restore. Navigation also has its own role and cleanup requirements.

## Patch only the owned path

Inspect `BuildUpdate_Grid.Place`, intersection checking, cancellation, and native completion signals.
If a placement is outside this feature's scope, allow vanilla behavior untouched. Once the mod owns a
custom placement attempt, an invalid result must cancel/refuse that attempt; do not blindly fall
through into a different native placement path unless that handoff is explicitly designed.

Historical `BuildableItem.Start` and `SetCulled` guards avoided property lookups or unwanted interior
culling for a mod-owned outdoor model. Skipping an entire method also skips other initialization.
Inspect its current responsibilities and replace all required work, or use a narrower hook. Scope the
guard through a verified owner/registration marker and current instance generation, not a global name.

## Placement transaction

1. Resolve the selected item instance, ownership model, session, authority, and current ghost result.
2. Revalidate distance, geometry, inventory availability, and feature state at commit time.
3. Stage an inactive owned clone using a verified cloning path. If a borrowed source prefab must be
   temporarily deactivated, capture its exact state and restore it in `finally`; avoid modifying a
   live world instance or shared prefab when a supported inactive clone path exists.
4. Configure the owned clone's components, collision, materials, IDs, and chosen network behavior.
5. Prepare required registration and inventory reservation/consumption with explicit compensation for
   any failure. Do not expose a usable free object before consumption is secured.
6. Commit one authoritative placement operation. Track an operation ID when retry/network delivery or
   coupled persistence can repeat it.
7. Publish the final state, notify native/UI systems through verified calls, and clear ghost state.
8. On failure, undo only this operation's provisional changes and release the staged clone/resources.

The old sequence `RegisterStreetItem` then `RemoveAmountOfItem` was not an atomicity guarantee. Verify
actual return values and partial effects. A sound/notification is not proof that inventory or network
registration succeeded.

`isKinematic` is a **Rigidbody** setting, not a collider setting. Set body physics deliberately and
inspect colliders separately. Never enable every renderer indiscriminately; preserve intended LOD,
culling, hidden functional children, and role-specific states.

## Managed/native lifetime

Use the installed interop's valid-object checks at native access boundaries and invalidate caches on
scene/session changes. A nonzero `Pointer` alone does not prove that the native object is live or that
a wrapper belongs to the current generation. Avoid broad exception swallowing as a lifetime strategy.

A verified parent walk or supported `GetComponentInParent` can resolve a root marker. Choose the one
that works with the deployed interop and inspect ambiguity; neither is universally safe merely by
name. The result must belong to this feature's registered object, not an unrelated ancestor.

## Save/load timing

Clear runtime caches and cancel pending placement when a new session/load begins, including a same-slot
menu-to-game reload. Resolve the real save identity before reading sidecars. Restore only entities
owned by the mod's restore model after their dependencies are ready; never duplicate native-restored
registered buildables in a generic load-complete callback.

The historical `OnSaveInfoLoaded` event fired zero times in one instrumented session. That observation
is a reason to inspect current event behavior, not a universal ban on the event. Select registration
and restore hooks from the target lifecycle and record their observed order.

Saving a sidecar after native save completion can align ordinary saves but does not remove the crash
window between native inventory and mod world state. For coupled item/world mutations, use the
canonical persistence transaction/recovery policy and test interrupted commits. A filename containing
only a slot number is not sufficient identity across different saves or storage roots.

Read [Persistence](../../schedule1-persistence/SKILL.md),
[Outdoor ownership](outdoor-placement.md), and [Collision and pack-up](collision-and-ghosts.md).
