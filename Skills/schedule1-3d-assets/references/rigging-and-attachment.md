# Rigging, Wearable Attachment, and Preview Ownership

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 7 of 7 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use for backpacks, hats, clothing, held props, and character-menu previews. Bone names below are
inherited lookup candidates from the workspace; they are not a guarantee for every avatar, prefab,
customization, or game version. Confirm the current rig and the specific actor instance first.

## Resolve the correct rig

Common historical candidates include `Hips`, `Spine`, `Spine1`, `Spine2`, `Neck`, `Head`,
`LeftShoulder`, `RightShoulder`, `LeftArm`, `RightArm`, `LeftForeArm`, `RightForeArm`, `LeftHand`,
`RightHand`, and leg/foot counterparts. Backpacks historically used an upper-spine bone; headwear
used `Head`; held props used the relevant hand. Treat these as search hints.

1. Resolve the intended player/NPC/avatar or mannequin through its owner, not a global first match.
2. Prefer the rig's supported bone mapping if exposed by the installed runtime.
3. Otherwise search only that rig, collect matches, and reject ambiguous/missing results.
4. Cache a binding for that avatar instance/generation. Invalidate it when customization rebuilds the
   model or the scene/owner changes.
5. Keep world-avatar, first-person, remote-player, and UI-preview bindings distinct.

A recursive name search returning the first match can select a nested prop or another rig. Log the
resolved relative path and model identity during development, without making the path a permanent
cross-version API claim.

## Attachment contract

The asset carries its intended pivot and local axes. Instantiate an owned root, parent it to the
verified anchor using the selected local/world transform semantics, and apply a measured local pose.
Do not reuse a historical `Spine2` offset as a universal backpack fit.

Rigid attachments and skinned clothing are different pipelines. Rigid props follow one bone;
skinned clothing needs a compatible mesh, bind poses, bone mapping, and weights. Parenting a skinned
mesh under one bone does not repair an incompatible skeleton.

Validate idle, walking, crouching, turning, interaction poses, and all supported body customization
ranges. Check bounds/culling as well as clipping. Record the original pose for temporary previews;
restore that pose instead of assuming `Quaternion.identity` was the original state.

## Collider policy by role

Cosmetic-only wearables normally should not intercept interaction raycasts or collide with the
wearer's controller. Inspect every collider on the owned wearable clone. Disable unwanted colliders
immediately before scheduling their destruction; Unity destruction is deferred until later in the
frame. Review any rigidbodies or interactive components that would recreate or rely on them.

This policy applies to the owned cosmetic clone. Do not remove colliders from the avatar, borrowed
prefab, gameplay equipment, dropped physical item, or unrelated child object. Interactive equipment
needs a deliberate collision/layer/trigger policy instead of a blanket stripping function.

## Preview controls

Add rotation only inside the active preview region and only when it owns input. Resolve the installed
input system and user bindings before assigning mouse drag or keyboard shortcuts. Q/E and raw mouse
axes from the old example are design candidates, not guaranteed free controls.

Cancel dragging on pointer release, focus loss, modal interruption, menu close, and teardown.
Restore the saved preview pose when the interaction ends. A preview must not rotate or change the
world actor unless that is the explicit feature. Create preview resources with their own ownership
record and reuse shared borrowed assets read-only.

## Lifetime and checks

Replace attachments atomically: validate the new rig/asset, stage the new clone, swap the owned
binding, then release the old clone and its uniquely owned materials/meshes/textures. On failure,
keep the last valid attachment or hide only the affected cosmetic.

Test equip/unequip, avatar customization, menu reopening, player respawn, remote player arrival,
scene reload, and missing bone. Verify there is one attachment per intended owner and that interaction
rays, movement, and menu focus still work.

## Primary source

- [Unity Object.Destroy](https://docs.unity3d.com/ScriptReference/Object.Destroy.html) documents delayed destruction.

For material cleanup, also read [Rendering and materials](urp-rendering-materials.md).
