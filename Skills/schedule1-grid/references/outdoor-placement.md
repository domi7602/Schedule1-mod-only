# Outdoor Object Ownership, Native Components, and Footprints

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 14 of 14 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use for items outside purchased properties, virtual outdoor roots, footprint suppression, and
native-query errors. The inherited StreetNomad/HomelessMod rules describe a specific mod-owned object
architecture. Re-evaluate them for native registered buildables, functional equipment, and multiplayer.

## Root model

For a mod-owned virtual collection, use a clearly owned root and registry with a defined scene/session
lifetime. Do not attach a partially configured native `Property` merely to satisfy one parent lookup:
real-estate UI, business logic, policing, save loaders, and other systems may enumerate it.

When the feature really is a native property, implement the complete supported property contract and
registration order; the world-expansion skill covers that architecture. A native property is not
universally forbidden, and a `DontDestroyOnLoad` root is not automatically the right lifetime. If a
persistent root is needed, tear down its old scene content and subscriptions at each session boundary.

## Retain or suppress components by contract

The old local/mod-owned architecture retained disabled `GridItem`/`BuildableItem` components because
immediate destruction could leave native code with stale references. Confirm current native query and
lifecycle behavior before choosing suppression. Do not use `DestroyImmediate` on live components as a
shortcut around an incomplete ownership design.

A retained disabled component may still be referenced directly by native code; disabling it does not
prove every callback or save path is neutralized. Inspect the relevant calls and scope any Harmony
guard to the registered owned clone. Never globally block `GridItem.Destroy` or native cleanup.

For a fully native buildable, keep the components needed for registration, interaction, networking,
saving, and placement. For a visual ghost, disable only the behavior that must not run while previewing.
The same prefab can require different policies in those roles.

## Footprint suppression

Historical lookup candidates include `FootprintTile`, `TileAppearance`, and the spelling
`SetFootprintTileVisiblity`. Verify those types/members in the target build before using them.

For an owned clone whose footprint indicators must be hidden:

1. Identify footprint/placement-indicator children by actual component ownership, including inactive
   children. Do not use a vague object-name substring for a whole scene.
2. Deactivate those indicator objects and call the verified footprint-visibility helper if required.
3. Disable only their relevant renderers. Preserve all other renderers' original enabled state.
4. Inspect whether native updates recreate/reactivate indicators; add a narrowly scoped lifecycle
   reconciliation only when that behavior is observed.
5. On reuse or return to native placement, restore only states this owner changed and still controls.

Turning every non-footprint renderer on can reveal hidden LODs, duplicate meshes, or intentionally
culled functional parts. Store a small ownership record for deliberate overrides instead.

## Root resolution and cache validity

Resolve from the actual hit/buildable instance to a registered root marker or stable runtime record.
A parent walk and `GetComponentInParent` are both possible implementation strategies after verifying
interop behavior. Guard against nested owned roots and stale scene references. Use managed owner
identity/session generation as well as native validity checks.

Register the world object only after required setup succeeds. Unregister it once during pack-up,
replacement, or teardown; an old instance's cleanup must not erase a replacement with the same stable
ID. Durable removal and runtime teardown are distinct operations.

## Verification

Check first placement, scene culling transitions, pack-up, replacement, scene reload, same-slot reload,
and host/client visibility. Verify native queries do not throw, indicators remain hidden only where
intended, functional renderers preserve their state, and vanilla buildables still place and despawn.

Read [Placement lifecycle](build-update-patching.md) and
[Ghost collision and pack-up](collision-and-ghosts.md) for the surrounding transaction.
