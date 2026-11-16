# NorthIsland

**NorthIsland** connects the unfinished Northtown Overpass (`z = 136..276`) straight across the northern water gap onto North Island, removes the base-game construction barriers and blocking invisible walls along the route, repairs the native southern Overpass Ramp collider, and constructs the coastal settlement **Northshore Town** plus the **8m West Plateau Hideout Warehouse**.

## Current Status & Verification Limits

- **Version:** `0.1.0` (**Phase 1 owner-confirmed 2026-11-16**, DLL `EA91537C…`)
- **Offline verification:** Pure C# geometry, broadphase coverage, instancing proxy filtering, terrain-clearance, Highland Drive / Off-Ramp seam continuity, `CoastalHouse_West` road clearance, and `BarrierFilterRules` unit tests (`11/11` passed) live under [`Source/Tests/NorthIsland.Tests/`](../../../Tests/NorthIsland.Tests/).
- **Live gameplay status:** Phase 1 in-game verification owner-confirmed by Dominik (`2026-11-16`): overpass continuation, off-ramp terrain clearance, vehicle & player collisions across the expanded broadphase region (`Physics.RebuildBroadphaseRegions`), grounded pine trees, clean `HideoutWarehouse` floor perimeter, and `CoastalHouse_West` clearance beside Highland Drive verified in singleplayer. Phase 2 (`HideoutWarehouse` buyable property & build grid) and multiplayer remain out of scope / not yet implemented.

## Key Features

1. **Overpass Continuation & Off-Ramp (`BridgeBuilder`):**
   - Continuous 140m bridge deck (`z = 136..276`, `y = 10.080m`), continuous side guardrails, and collider-equipped support piers.
   - Smoothly blended `IslandOffRamp` (`z = 386..424`) keeping positive clearance above 5m terrain (`>= +0.128m`) and following `NorthIslandGeometry.GetBoulevardSurfaceY(x)` pointwise across the full roadway width.
   - 5m-grid `NorthIsland_TerrainMeshCollider` (`76 x 49` samples from `NorthIslandTerrainData`).
   - Smoothed southern Overpass Ramp `MeshCollider` + top-curve transition collider.
2. **Northshore Town (`TownBuilder`):**
   - **Roads & Parking:** Northshore Boulevard (`x = -70..+105, z = 424..434`), Town Center Parking (`x = +2..+24`), Highland Drive climbing smoothly to the `8.08m` West Plateau Courtyard (`x = -110..-72, z = 318..344`) with curb-free flush end aprons.
   - **Buildings:** `Northshore_HideoutWarehouse` (with sill entry ramp & recessed floor slab), `Northshore_GeneralStore`, `Northshore_BaysideDiner` (central open steps + flanked patio decks), `Northshore_HarborGarage`, `CoastalHouse_West` (`-64.5, 414.2`, clear of Highland Drive), and `CoastalHouse_East`, equipped with solid `BoxCollider` walls and 1.4m flush-exterior anti-tunnel colliders.
   - **Physics Broadphase & Environment:** Expands PhysX MultiboxPruning bounds (`Physics.RebuildBroadphaseRegions`) to cover Hyland Point + North Island (`Z in [-300, +650]`), filters out `@InstancingManager` proxies so only grounded non-instanced tree prefabs spawn with trunk colliders, plus stem-collidered shrubs, perimeter streetlights (`0.42m` pole colliders), benches, trash cans, welcome sign, and 0.50m coastal safety barrier.
3. **Barrier Removal (`BarrierRemover` & `BarrierFilterRules`):**
   - Deactivates construction barriers, invisible boundary walls (including distance-culled `OptimizedColliderGroup` colliders), blocking Overpass Turn fences/dividers, and northern gap obstacles with full tracking/restoration in `RestoreAll()`.
4. **Console Commands & Debug Raycast (`IslandConsoleCommand`, `Mod`):**
   - `island help | ramp | bridge | town | tp | hideout | plateau | rebuild | scan | status`
   - `island raycast [on|off]` — toggles plain Middle-Click raycast inspector (disabled by default; `Ctrl + MiddleClick` always remains available without interfering with gameplay camera controls).
   - Teleports check `PlayerMovement.Instance.CurrentVehicle` and ask to exit any vehicle first before warping.

## Tests

Run executable game-free unit tests via:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/test-all.ps1
```
