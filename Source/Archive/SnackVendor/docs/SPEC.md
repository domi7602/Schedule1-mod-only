# SnackVendor — Spec v0.1 (Draft, 2026-09-14)

> Custom vending machine like the Cuke machine, but the player fills it
> themselves with ingredients from the gas mart. NPCs buy from it; the sale
> price is the vanilla market price of the ingredient and is credited to the
> player as cash.

## User Decisions (2026-09-14, binding)

| Question | Decision |
|---|---|
| Placement | **Placeable** like AutoPackagingStation (buildable item, anywhere) |
| Consumption | **No player consumption.** Only NPCs buy; item moves into NPC inventory |
| Price | **Vanilla gas-mart price** of the ingredient (like a snack machine) |
| User | NPCs buy → **passive income** for the player |
| Self-extract | **Free** (own stock) |

## Asset Pipeline: 3D Model (2026-09-14, built)

- **Blender headless:** `"C:\Program Files\Blender Foundation\Blender 5.2\blender.exe"
  --background --python assets/build_snackvendor_model.py` produces
  `assets/SnackVendor_model.glb` (+ preview render).
- **Model:** low-poly vending machine in Cuke style, 0.95×1.85×0.72 m:
  red body, glass front with 4 shelves + colorful snack boxes/cans
  (deterministic seed), chrome door frame, control panel (display, 3×3 keypad,
  coin slot), output bin, base/feet, yellow logo stripes.
- **Reference empties in the GLB** (for path-A integration): `ItemSpawnPoint`,
  `AccessPoint`, `CashSpawnPoint` — the transforms the vanilla
  `VendingMachine` component expects.
- **Runtime load (planned, AutoPack pattern):** `Mods/SnackVendor/model.glb` via
  `S1MAPI.Gltf.GltfLoader.LoadGlb(bytes)` → strip colliders → URP-Lit shader
  on all renderers (otherwise pink) → parent under visual root. GLB is already
  at the target location; C# loader follows with feature step 2.
- **Pitfall:** `bpy.path.abspath("//...")` resolves in `--background` mode
  without a .blend file to `C:\` — give absolute output paths or move the
  file back after the run.

## Verified Vanilla Building Blocks (game v0.4.6f13, decompiled proxies)

- `Il2CppScheduleOne.ObjectScripts.VendingMachine` — reference for purchase flow
  (`Interacted → SendPurchase` ServerRpc → `PurchaseRoutine` ObserversRpc →
  `DropItem`), static `AllMachines` list, GUID persistence (`GetSaveData`/`Load`),
  break/repair, static `COST`. **Has NO stock system** — Cuke stock is infinite.
- `NPCSignal_UseVendingMachine` (`Il2CppScheduleOne.NPCs.Schedules`) — vanilla NPC AI
  buys at machines: `GetTargetMachine()`, `MachineOverride`, `TargetMachine`,
  `WalkCallback`. Finds machines via `VendingMachine.AllMachines`.
- `NPC.Inventory` (`NPCInventory`, `Il2CppScheduleOne.NPCs.Framework.Inventory`) —
  NPC inventory exists; purchased items can move into it.
- `AdditiveDefinition` (ingredients): only carries mixing stats (`QualityChange`,
  `YieldMultiplier`, `InstantGrowth`) + `BasePurchasePrice` via
  `StorableItemDefinition`. **No consumption effects in vanilla** — for player
  consumption a new build would be needed (intentionally NOT in scope).
- S1API: `S1API.Entities.NPC`, `S1API.Money`, `BuildableItemDefinitionBuilder`
  (AutoPack uses the latter for the placeable station).

## Architecture (Layers like AutoPackagingStation)

```
Mod.cs                     Melon entry, config init, Harmony patches
SnackItemFilter.cs         HardFilter: only EItemCategory.Ingredient in stock slots
SnackVendorController.cs   MonoBehaviour on the placed GO:
                           native ItemSlots (stock), GUID, interaction hooks
SnackVendorStore.cs        Sidecar persistence snacks_slot_{n}.json (slot_-1 guard,
                           .bak protection) — pattern: AutoPackStore/BusinessIncome
SnackPurchaseEngine.cs     NPC purchase: capture signal/attraction, price =
                           BasePurchasePrice, item → NPC.Inventory, cash → player
SnackVendorFactory.cs      BuildableItemDefinitionBuilder + mesh (AutoPack clone)
```

### NPC Attraction — Decision: Cuke Clone Architecture (2026-09-14, replaces path A/B)

Vanilla scheduling analysis (decompiled, verified):
- Every NPC has `NPCScheduleManager` (via `ScheduleBehaviour`): actions list
  (data-driven from NPC prefab assets!), `OnMinPass` tick, `ShouldStart()` per
  action, `priority`/`StartTime`, curfew lists (`EnabledDuringCurfew`/`NoCurfew`).
- `NPCSignal_UseVendingMachine` selects target AT RUNTIME: `MachineOverride`
  field + `GetTargetMachine()`; NPCs find machines via the static
  `VendingMachine.AllMachines`.
- `NPCManager.NPCRegistry` = static `List<NPC>` of all NPCs (fallback, if the
  clone path still fails: radius scan, BusinessIncome pattern).

**Rejected: `AddComponent<VendingMachine>` on the placed GridItem.** FishNet
collects NetworkBehaviours on spawn — components added afterwards don't get
RPC registration; `SendPurchase` (ServerRpc) would silently fail. No spike
value: architecturally a dead end.

**Cuke clone architecture (new, binding):**
1. **GridItem 1×1 stays the anchor** (build menu, placement, overlap prevention,
   vanilla position persistence) — AutoPack pattern (`CloneFrom(pot|storage)`
   instead of `packagingstation`, if footprint correction needed:
   `CoordinateFootprintTilePairs`/`OriginFootprint` set to 1×1).
2. On placement (`BuildableItem.Start` postfix): **clone a real Cuke machine
   from the scene** (`Instantiate` a vanilla instance from
   `VendingMachine.AllMachines` as template) → FishNet object with fully wired
   RPCs, `InteractableObject`, colliders, lights, sounds,
   `PhysicsDamageable` — `Awake()` of the clone registers itself in
   `AllMachines`.
3. **Mesh swap:** disable original renderers on the clone, parent our GLB
   (`Mods/SnackVendor/model.glb`, with `ItemSpawnPoint`/`AccessPoint`/
   `CashSpawnPoint` empties) underneath, redirect component references
   (`ItemSpawnPoint`, `AccessPoint`, `CashSpawnPoint`, possibly `DoorMesh`) to
   our empties. GLB load: S1MAPI.Gltf + URP-shader fix.
4. Align the clone to the grid position; keep the GridItem's visual invisible
   (AutoPack `HideBaseRenderers` pattern).
5. Host spawns the clone via FishNet (runtime spawn proven — vanilla
   `DropItem` spawns NetworkObject pickups on every purchase).
6. Harmony patch on `SendPurchase`/`PurchaseRoutine`/`DropItem`: only on
   instances with our marker (`SnackVendorMarker` component) → purchase
   against our stock (ingredient from slot → `NPC.Inventory`, cash =
   `BasePurchasePrice` to player). Vanilla machines untouched.
7. Save/Load: position = GridItem (vanilla). Stock sidecar
   (`snacks_slot_{n}.json`) attaches to the clone's GUID; on load re-instantiate
   the clone + re-link (GUID stability via clone's `BakedGUID`).

**Spike gate (S1MCP) stays, but tighter scope:** (a) does cloning a scene
vending machine at runtime produce a working, purchasable machine?
(b) How does `OnDestroy` of the clone (AllMachines deregistration) behave on
destroy/dismantle of the GridItem? (c) BakedGUID stability across save/load.

### Money-Flow Rules (from game-mod-persistence skill)

- Book sale: first decrement stock slot + snapshot, then credit cash;
  flag persistence sidecar dirty; on load check marker (crash window).
- Price source: `StorableItemDefinition.BasePurchasePrice` (vanilla gas mart),
  NO custom price — user decision.
- Multiplayer: host guards on every mutating path (deposit/extract/purchase/
  save-flush), pattern from schedule1-il2cpp-sorting-patterns skill.

## Non-Scope (Explicit)

- Player consumption of ingredients (no vanilla path, user declined)
- Fixed spawn points in the city
- Co-op purchase by other players (host world only)

## Open Items / Build Order

1. [x] Scaffold builds green (Mod.cs skeleton, csproj, mod.json) ← done 2026-09-14
2. [x] 3D model (Blender headless → GLB + ref empties, in Mods/SnackVendor/) ← done
3. [ ] Register + place GridItem 1×1 (AutoPack clone: CloneFrom, ghost, icon)
4. [ ] Cuke clone spawn on placement: BuildableItem.Start postfix → VendingMachine
       clone + mesh swap + ref redirect + marker — **S1MCP spike (a)/(b)/(c)**
5. [ ] Stock slots (HardFilter Ingredient) + deposit/extract UI + sidecar persistence
6. [ ] Purchase interception (Harmony on SendPurchase) + cash payout (money-flow
       marker) + NPC.Inventory transfer
7. [ ] UI polish, multiplayer host guards, version bump (4 places + AGENTS.md approval)