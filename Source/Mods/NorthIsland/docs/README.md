# NorthIsland

**NorthIsland** connects the unfinished Northtown Overpass (`z = 136..276`) straight across the northern water gap onto North Island, removes the base-game construction barriers and blocking invisible walls along the route, repairs the native southern Overpass Ramp collider, and constructs the coastal settlement **Northshore Town** plus the **8m West Plateau Hideout Warehouse**.

## Current Status & Verification Limits

- **Version:** `0.1.0` (**Phase 1 owner-confirmed 2026-11-16**, DLL `EA91537C…`; Phase 2 implemented, in-game verification open; Phase 3 „Northshore Nights & Waterfront" implemented 2026-11-16, in-game verification open)
- **Offline verification:** Pure C# geometry, broadphase coverage, instancing proxy filtering, terrain-clearance, Highland Drive / Off-Ramp seam continuity, `CoastalHouse_West` road clearance, `BarrierFilterRules`, and Hideout purchase/grid/save rules (`15/15` passed) live under [`Source/Tests/NorthIsland.Tests/`](../../../Tests/NorthIsland.Tests/).
- **Live gameplay status:** Phase 1 in-game verification owner-confirmed by Dominik (`2026-11-16`): overpass continuation, off-ramp terrain clearance, vehicle & player collisions across the expanded broadphase region (`Physics.RebuildBroadphaseRegions`), grounded pine trees, clean `HideoutWarehouse` floor perimeter, and `CoastalHouse_West` clearance beside Highland Drive verified in singleplayer. Phase 2 registers that warehouse as the singleplayer property `northshore_hideout` ($20,000, bank then cash) with a door-side buy prompt and an interior build grid. Ownership and placements are stored in the slot's `Modded/NorthIsland/hideout.json`, not in native Saveables. Purchase, placement, pickup and reload are not yet owner-confirmed. No delivery dock, no realtor listing and no employee routing. Multiplayer remains out of scope.

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
4. **Hideout property (`HideoutPropertyHost`):**
   - `northshore_hideout`, $20,000, Barn for-sale sign on its pole in front of the east door, bought at Ray's Realty, interior 0.5 m build grid after purchase.
   - Slot file `Modded/NorthIsland/hideout.json`. Not a native Saveable. No dock, realtor listing or employee routes yet.
5. **Console Commands & Debug Raycast (`IslandConsoleCommand`, `Mod`):**
   - `island help | ramp | bridge | town | tp | hideout | plateau | rebuild | scan | status`
   - `island raycast [on|off]` — toggles plain Middle-Click raycast inspector (disabled by default; `Ctrl + MiddleClick` always remains available without interfering with gameplay camera controls).
   - Teleports check `PlayerMovement.Instance.CurrentVehicle` and ask to exit any vehicle first before warping.
6. **Northshore Nights & Waterfront (`NorthshoreNeon`, `NorthshoreWaterfront`, Phase 3, in-game verification open):**
   - Illuminated TMP lettering on the welcome/store/diner signs (font borrowed read-only from the Barn for-sale sign), pink/cyan diner neon tubes, warm unlit window glow, 8 sagging string-light spans between the existing boardwalk lamp poles, and guardrail-post lanterns — no extra realtime lights.
   - Walkable wooden pier through a clipped guardrail gap at `x = 14..20` out to a head platform (`z = 476.5`) with bench, one lamp light, mooring bollards and car-blocking entrance bollards; static rowboat; lighthouse on a stone breakwater mound in open water at `(-35, 462)` with one landmark point light.
   - Deck/mound elevations adapt at runtime to the discovered native ocean plane `Main/Map/StylizedWater2_Ocean` (read-only, WorldLook-evidenced path); if unresolved, the waterfront skips itself with a bounded warning. The guardrail gap is the only change to accepted Phase 1 geometry and is pure-test covered.

## Tests

Run executable game-free unit tests via:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/test-all.ps1
```
