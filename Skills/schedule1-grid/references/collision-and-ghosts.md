# Ghost Positioning & Collision — Raycast, Sampling, Pack-Up

Ghost evaluation runs every frame in `BuildUpdate_Grid.CheckIntersections` postfix. Uses `GroundPlacementAssistant` for 5-point sampling.

## Real-Time Ghost Evaluation

1. Guards: `EnableEverywhereBuilding`, `Pointer != 0`, `GhostModel`, `BuildManager`
2. Vanilla pass-through: if `__instance._validPosition == true` → white material, return
3. Camera: `PlayerSingleton<PlayerCamera>.Instance.Camera.transform` fallback `Camera.main`
4. Raycast: `Physics.Raycast` along camera forward, mask `~GetMask("Ignore Raycast","Player")`, `QueryTriggerInteraction.Ignore`. **Do not exclude `Grid` layer** — street meshes live on `Grid`. Fallback: down ray 2m in front, `y+1.5f`
5. Ground sampling (`GroundPlacementAssistant`):
   - 4-corner elevations around footprint → prevents curb sinking
   - Slope ≤45°, reach 0.3–7.0m
   - Clearance: `Physics.OverlapBoxNonAlloc` extents×0.85, center raised, ignore player/ghost colliders
6. Feedback: set `IsCustomPlacementValid`, `_validPosition`, `ghostMaterial_White/Red`; position+rotate ghost

## Pack-Up & Dismantling (OutdoorItemInteractable)

- Detection: distance ≤3.0m, camera dot + raycast vs colliders
- Inputs: `[F]` instant, Hold RMB 0.40s progress
- HUD: cached `GUIStyle`, clamped scaling `Screen.height/900f 0.80–1.25` (0-alloc IMGUI)
- Execution: `AddItemToInventory` (fallback `ConsoleHelper`), `UnregisterStreetItem`, `Destroy(gameObject)` — persistence only on `OnSaveComplete`

## Verification Checklist (grid mods)

- [ ] Raycast mask allows `Grid` layer with `Ignore` trigger
- [ ] GridItem disabled not destroyed
- [ ] DisableNetworking/DisableNavigation called
- [ ] BuildableItem.Start/SetCulled Harmony guards
- [ ] Virtual root clean (no Property)
- [ ] FootprintTile deactivated & renderers filtered
- [ ] Rigidbodies `isKinematic = true`
- [ ] Anti-dupe: runtime in-memory only, save on OnSaveComplete
- [ ] Slot isolation `street_items_slot_{slotId}.json`
- [ ] Pointer checks `Pointer != IntPtr.Zero`
- [ ] `IsOutdoorItem` uses parent walk
- [ ] `IsCustomPlacementValid` reset in Place/Stop/exception

See also `outdoor-placement.md` and `build-update-patching.md` for companion rules.
