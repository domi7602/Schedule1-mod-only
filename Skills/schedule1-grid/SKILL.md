---
name: schedule1-grid
description: "Schedule I: Grid placement system, outdoor/unrestricted building, BuildUpdate_Grid patching, ghost positioning, and IL2CPP lifecycle stability"
---

> Version anchor: Game v0.4.7f7 / S1API 3.2.1-beta.7 / MelonLoader 0.7.3 (install verified 2026-10-02 against the live Steam Open Beta: Latest.log "Game Version: 0.4.7f7"; content deep-verified against 0.4.7f6 / 2026-09-28 - anything not explicitly marked as re-verified must be checked against the live Il2CppAssemblies). Re-check after any game or S1API update.

# Schedule I - Grid & Building System Skill

This skill documents how to work with the building, placement, and grid systems in *Schedule I* (IL2CPP / MelonLoader), particularly when creating outdoor/unrestricted placement systems (such as the archived `HomelessMod` - `Source/Archive/HomelessMod/`) or extending vanilla building mechanics.

> **Reference split (A):** Details moved to `references/` — load on demand. This file is the decision tree + quick refs.

---

## Reference Architecture & Key Classes

| Class / Component | Type | Responsibility & Behavior |
|---|---|---|
| `BuildUpdate_Grid` | Vanilla (`Il2CppScheduleOne.Building`) | Manages build mode ghost positioning, rotation, raycasting, and placement execution. |
| `GridItem` | Vanilla (`Il2CppScheduleOne.EntityFramework`) | Attached to vanilla buildables. Triggers auto-deletion if placed outside a purchased property grid. |
| `BuildableItem` | Vanilla (`Il2CppScheduleOne.EntityFramework`) | Manages property attachment, network registration, and culling states. |
| `GridManager` | Vanilla (`Il2CppScheduleOne.Tiles`) | Tracks valid purchased properties via invisible "Grid" layer trigger colliders. |
| `BuildManager` | Vanilla (`Il2CppScheduleOne.Building`) | Singleton (`NetworkSingleton<BuildManager>.Instance`) providing ghost materials, sound effects, and networking/navigation strippers (`DisableNetworking`, `DisableNavigation`). |
| `FootprintTile` / `TileAppearance` | Vanilla (`Il2CppScheduleOne.Tiles`) | Visual indicator tiles instantiated below grid objects. Must be suppressed for outdoor placement. |
| `Property` | Vanilla (`Il2CppScheduleOne.Property`) | Core property entity. **Never attach directly to virtual world roots.** |
| `GroundPlacementAssistant` | Custom Helper (`HomelessMod.Building`, archived) | Zero-allocation helper for smart 5-point ground sampling, slope checks (≤ 45°), box collision non-alloc checks, and dual-grid snapping. |
| `StreetPropertyManager` | Custom Manager (`HomelessMod.Building`, archived) | Virtual world root container (`StreetNomad_WorldRoot`), in-memory registration, save-slot isolated persistence, and GUID deduplication. |
| `OutdoorItemInteractable` | Custom Component (`HomelessMod.Building`, archived) | IL2CPP-registered MonoBehaviour for holding RMB or pressing [F] to dismantle/pack up placed world objects. |

---

## The 7 Golden Rules — Summary

Full code & rationale in `references/`:

1. **NEVER Destroy GridItem** — disable (`gi.enabled=false`) + `SetFootprintTileVisiblity(false)` → `references/outdoor-placement.md`
2. **Strip FishNet Networking** — `DisableNetworking/DisableNavigation` on clone → `references/build-update-patching.md`
3. **Harmony Guards on BuildableItem.Start & SetCulled** — skip on outdoor items → `references/build-update-patching.md`
4. **Never attach Property to _streetRoot** — keep `StreetNomad_WorldRoot` clean + `DontDestroyOnLoad` → `references/outdoor-placement.md`
5. **Footprint & Grid Tile Suppression** — deactivate `FootprintTile`, filter renderers → `references/outdoor-placement.md`
6. **Save Synchronicity (Anti-Dupe)** — in-memory only, serialize only on `GameLifecycle.OnSaveComplete`, slot `street_items_slot_{id}.json` → `references/build-update-patching.md`
7. **IL2CPP Pointer Safety** — `Pointer != IntPtr.Zero && !WasCollected` + parent walk in `IsOutdoorItem` → `references/build-update-patching.md`

---

## Placement & Load Lifecycle — Overview

- **Ghost evaluation** (`CheckIntersections` postfix): camera raycast → fallback down ray → 4-corner sampling (slope ≤45°, reach 0.3–7m) → `OverlapBoxNonAlloc` clearance → ghost white/red. Details: `references/collision-and-ghosts.md`
- **Placement** (`Place` prefix): if `IsCustomPlacementValid` → deactivate source prefab, `Instantiate`, parent to `StreetRoot`, strip networking, attach `OutdoorItemInteractable`, suppress footprints, register, deduct inventory, `Stop()` and `return false`. Details: `references/build-update-patching.md`
- **Save/Load:** `OnPreLoad->ResetState`, `OnSceneWasLoaded("Main")->register defs`, `OnLoadComplete->LoadAndSpawn`, `OnSaveComplete->SaveStreetItems` (atomic; `OnSaveInfoLoaded` is dead on 0.4.7f6). Diagram in `references/build-update-patching.md`

---

## Pack-Up & Verification

- **Dismantle:** distance ≤3m, `[F]` or hold RMB 0.40s, IMGUI cached, `AddItemToInventory` → `Unregister` → `Destroy` (persist on save only). Details: `references/collision-and-ghosts.md`
- **Checklist:** 12-point grid verification in `references/collision-and-ghosts.md` (raycast mask, GridItem, networking, Harmony, virtual root, footprint, kinematic, anti-dupe, slot isolation, pointer, parent walk, state reset)

Load the relevant `references/*.md` when implementing that subsystem — do not inline all rules at once.

## Decompile Reference Note

Decompiled references for grid and building classes (`BuildUpdate_Grid`, `GridManager`, `BuildableItem`) are located directly in the repo at:
- `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/Building/`
- `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/Tiles/`
- Architecture guide: `Skills/schedule1-game-systems/references/10-Building-Construction.md` and `59-TilesGrid.md`
