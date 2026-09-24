# Changelog

## 0.0.9 (2026-09-17)
- **Station BoxCollider & [E] interaction fixed:**
  - **Physical station BoxCollider:** `SnackVendorController.EnsureStationCollider()` configures a `BoxCollider` (`0.95m × 1.85m × 0.72m`, center `Y = 0.925m`) directly on the station GameObject (`gameObject`). Fixes the problem where, after the GLB colliders were removed, the station only had the 10 cm flat ground collider of the cloned `dryingrack` and player raycasts at eye height passed straight through the machine into the wall.
  - **Native [E] interaction & look-at detection:** `SnackVendorController.Update()` checks for the `[E]` interaction key within a 3m look-at radius (raycast + viewing-angle fallback) and toggles the `SnackVendorPanel`.
  - **HUD interaction prompt:** On look contact, `<color=#f39c12>[E]</color>  <b>Snack-Automat</b>` appears centered at the bottom of the screen (perfectly stacked above the RMB dismantle bar).
  - **Panel [E] & [ESC] close:** The panel now closes either via the `[ESC]` key or by pressing `[E]` again.
  - **Central dismantle & remaining-snack refund:** `RefundStockToPlayer()` and `RefundStockAndDismantle()` place remaining ingredients and the station safely into the player inventory, unregister the machine from all managers, and clean up the sidecar entry.



## 0.0.8 (2026-09-17)
- **Standalone outdoor placement (independent of HomelessMod):**
  - **Strictly isolated to SnackVendor:** Outdoor placement outside of bought properties is hard-gated to `snackvendor`. Other furniture/items stay restricted to properties in the vanilla build system.
  - **Standalone build subsystem (`SnackVendor.World.Outdoor`):**
    - `SnackVendorBuildPatches`: intercepts `BuildUpdate_Grid.CheckIntersections` (postfix) & `Place` (prefix).
    - `SnackVendorGroundAssistant`: 0-allocation raycast and ground detection (slope ≤ 45°, range 0.4–6.0 m, multi-corner terrain height, `OverlapBoxNonAlloc` obstacle check).
    - `SnackVendorOutdoorManager`: own virtual world root (`SnackVendor_OutdoorRoot`, `DontDestroyOnLoad`) and persistent save-slot files `UserData/SnackVendor/outdoor_stations_slot_{n}.json` via `SafeStorage.SaveAtomic` (.bak-secured).
    - `SnackVendorGuardPatches`: protects placed street machines from `BuildableItem.Start` property crashes, interior culling (`SetCulled`), and GridManager auto-destruction (`GridItem.Destroy`).
  - **Pack up (dismantle):**
    - `SnackVendorOutdoorInteractable`: hold RMB for 0.4s with a visual HUD progress bar packs the machine up.
    - `SnackVendorPanel`: additional `[ 📦 Pack Up Station ]` button in the UI panel for convenient click-to-dismantle.
    - **Automatic stock refund:** Remaining snacks in the machine are automatically placed back into the player inventory alongside the machine (1x `snackvendor`) when dismantling.
  - **Seamless coexistence with HomelessMod:** `HomelessModInterop` checks at runtime whether HomelessMod is active; no double placements or file conflicts occur.


## 0.0.7 (2026-09-17)
- **Blueprint ghost Y-offset & GLB preview fixed:** In blueprint/ghost mode the machine sank halfway (0.925 m) into the ground. Cause: `PrimitiveType.Cube` centers its pivot at `(0, 0, 0)` (Y ranges from -0.925 m to +0.925 m), so the lower half went below the grid plane.
  - **GLB ghost preview:** `BuildOrLoadGhostPrefab` now primarily loads the real GLB model (`SnackVendor_model.glb`) via `S1MAPI.GltfLoader` (colliders removed, base at Y = 0) — in the blueprint the real vending machine model is now visible flush on the ground.
  - **Cube fallback corrected:** Should the GLB be missing/fail, the fallback proxy cube is shifted upward by `+0.925 m`, so its lower edge sits flush on the ground.
  - **Fresh instances:** The ghost factory now creates a fresh instance each time, to prevent destroyed GameObject references after build abort / placement.


## 0.0.6 (2026-09-16)
- **Repo fix (discovered with it):** `NPCSignalPatches.cs` and `SnackVendorPanel.cs` were never committed due to a `.gitignore` bug (`Mods/` without a root anchor also matched `Source/Mods/`) — a fresh clone could not build SnackVendor. Both files are now in the repo.
- **Interacted_Prefix fail-closed:** The silent `catch { return true; }` allowed internal errors to open the vanilla pay UI on our own machines (player could "buy back" their own stock). Now: warning log + vanilla UI stays blocked for our machines; vanilla machines unchanged.
- **Persist double backup removed:** `SnackVendorStore.Save` did its own `.bak` copy + dead `.tmp` cleanup, even though `SafeStorage.SaveAtomic` handles both internally — duplicate backup write per NPC purchase (hot path) eliminated.
- **Panel log flood throttled:** `SnackVendorPanel.Draw` logged a persistent error per frame; now max 1 warning per panel-open (reset on toggle). Duplicate `_open == null` checks in `Update`/`Draw` merged.
- **KNOWN GAP (MP) documented:** Deposit/Extract run client-local (local stock + local sidecar file), while NPC purchase only decrements host-side → stock desync possible in multiplayer. Documented as a deliberate spike gap in `SnackVendorPanel`/`VendingMachinePatches`; singleplayer is unaffected.
- **Invisible station fixed (GLB mesh hiding fix 2026-09-17):** The placed station was completely invisible. Causes: 1) `SwapMesh` attached the GLB mesh to `Clone.transform` (the animated vanilla clone). 2) The `HideBaseRenderers` loop called `IsChildOf` on unmapped IL2CPP GLTF nodes, failed, and set `forceRenderingOff=true` on all 59 GLB meshes. 3) `sm.shader = sh` overwrote the URP-Lit-PBR materials already correctly initialized by `S1MAPI.GltfLoader`. Fix: GLB is attached directly to `transform` as `SnackVendor_VisualRoot`, all child nodes get the `SnackVendor_` prefix, the `sm.shader` mutation is removed, clone renderers are selectively switched off on the clone, and the root loop spares all `SnackVendor` renderers.
- **RMB pack-up via HomelessMod (in-game report 2026-09-17):** The station on the street couldn't be packed up with RMB ("won't fit in inventory") — as a custom station it got no `OutdoorItemInteractable`, so the vanilla pack-up of the rack prefab fired into the void. Coordinated fix in HomelessMod v0.1.12: `snackvendor` gets the generic `OutdoorItemInteractable` in the place and restore paths (refund 1x via `GetDefaultInstance` → `AddItemToInventory` + `UnregisterStreetItem` + Destroy); `autopackagingstation` stays excluded (own F-key pack-up).
- **Sidecar orphan prune:** `PersistAllStationSlots` removes sidecar entries without a live controller on SaveComplete — packed-up stations no longer leave a reawakenable stock entry (GUID changes on new placement).



## 0.0.5 (2026-09-16) — Spike completion (GLB, NPC inventory, panel)
- **GLB mesh via S1MAPI:** `SwapMesh` loads `S1MAPI.Gltf.GltfLoader.LoadGlb(bytes)` (same pipeline as AutoPackagingStation): colliders removed, URP shader fix via `sharedMaterial` (H12 pattern, no material clones), vanilla clone renderers disabled only after a successful load. Fallback on load failure: vanilla mesh stays visible. GLB is now auto-deployed (`assets/*.glb` → `Mods\SnackVendor\` via `Directory.Build.targets`); runtime accepts both `SnackVendor_model.glb` and the legacy `model.glb`.
- **NPC inventory credit:** New `NPCSignalPatches.Purchase_Prefix` (PatchGuard-registered) captures the purchasing NPC (`GetComponentInParent<NPC>()`, host-gated) in a 60s pending map; `SendPurchase_Prefix` transfers `GetDefaultInstance(1)` → `NPC.Inventory.InsertItem`. Errors never block the cash path. Config: `CreditNpcInventory` (default on).
- **Deposit/Extract panel:** `VendingMachine.Interacted` prefix opens an IMGUI panel on our machines (HomelessMod pattern: cached GUIStyles, damped scaling, cursor-free while open) instead of the vanilla pay UI. Stock in (+1/+5/All) with `GetCopy` rollback on full station slot; extract with 1-unit capacity probe and stock refund on add fail. Auto-close on distance/ESC/scene change/destroyed station. Config: `PanelRange` (3.5 m).
- **Stock identity switched to registry IDs:** The old numeric scan (`GetItem("1".."1023")`) never matched real item IDs (IDs are strings like `"cuke"`) — stock slots, price/name resolution, and allowed set now run via string IDs; the allowed set is derived from shop listings (gas mart preferred, `AdditiveDefinition` filter, 30s cache). Legacy sidecar rows with numeric IDs are discarded on restore (logged). `MaxSlots` now attaches to `MaxIngredientSlots` (config).
- **Side-effect fix:** `ThirdParty/Archive/**` was missing in `ThirdParty/.deployignore` — every build deployed the deprecated reference assembly `Hash.dll` to `Mods\` (BadImageFormatException source, cleanup 2026-09-10). Ignore rule added, DLL removed from `Mods\`.
- Note: In-game verification (spike gate + new features) is still pending — see verification table in the mod's README.

## 0.0.4 (2026-09-16) — Street placement integration
- **`SetupPlacedStation`:** New entry point in `SnackVendorItemFactory` for placement paths that instantiate the BuildItem prefab directly and skip vanilla `BuildableItem.Start` (HomelessMod street placement; same contract as AutoPack's `SetupPlacedStation`). Attaches the controller or reuses an existing one.
- **Placement split in the controller:** `SetupAfterPlacement` (vanilla path) and `SetupAfterPlacementExternal` (street path) both run into `RunPlacementSetup` with a `_placementDone` guard — double-setup (start postfix + external handoff) is excluded.
- **Restore GUID:** On street-place handoff, the persisted street-item GUID moves into the controller, so the sidecar (`snacks_slot_{n}.json`) resolves to the same slot entry across save restores.
- **Keep clone mesh visible:** `SwapMesh` previously disabled clone renderers even though the GLB→mesh conversion was still a stub — the result would have been an invisible machine. Renderers stay on until the real GLB routing lands (follow-up).
- **Diagnostics:** The start postfix now logs (debug) for non-SnackVendor items — "postfix never fired" vs. "wrong item" is distinguishable in the log. Cosmetics: hardcoded "0.0.1" in the init log string removed.
- Note: In-game verification of the spike gate (clone in `VendingMachine.AllMachines`, NPC routing, cash credit) is still pending.

## 0.0.3 (2026-09-15) — Audit patch (before 0.1.0)
Security and cleanup audit from the repo review 2026-09-15. Inserted before
the open 0.1.0 TODOs (GLB, shop listing, UI, NPC inventory) so that MP
security and memory leaks are in place before the player-facing release.

- **H1 Host authority for `SendPurchase`:** `NetworkGuard.IsHostOrSingleplayer()`
  as pre-gate in `VendingMachinePatches.SendPurchase_Prefix` — prevents
  cash-credit duplication on MP clients. The other three prefixes
  (`PurchaseRoutine`/`DropItem`/`DropCash`) stay pure "block vanilla" prefixes
  without money movement and need no gate.
- **H2 PatchGuard migration:** `PatchClassProcessor` replaced by five
  explicit `PatchGuard.TryPatch` calls (BuildableItem.Start postfix + 4×
  VendingMachine prefixes). PatchGuard statistics (`PatchGuard.Report(Log)`)
  now show signature drift instead of silent dead station.
- **H3 `SnackVendorController.OnDestroy`:** Now tracks the material
  instances allocated in `SwapMesh` + the GLB mesh GameObject in
  `_ownedMaterials`/`_glbMeshGo` and destroys them explicitly on station
  dismantle. Idempotent via `_destroyed` guard.
- **M8 `BuildOrLoadGhostPrefab.Dispose()`:** Destroys icon sprite +
  texture, ghost material and ghost GameObject; called in
  `Mod.OnDeinitializeMelon` (hot reload / app quit safety).


# Changelog - SnackVendor

## 0.0.2-mvp (2026-09-14) — MVP spike
First integrated build (build OK, in-game verify pending):

- **Wired:** Buildable 1x1 (`snackvendor`) via S1API `BuildableItemDefinitionBuilder`; placement via `BuildableItem.Start` postfix; vanilla Cuke-vending-machine clone per station; marker-guarded Harmony prefixes (`SendPurchase`, `PurchaseRoutine`, `DropItem`, `DropCash`); sidecar persistence `snacks_slot_{n}.json` (sentinel guard) via `SnackVendorStore`; procedural proxy ghost + 64x80 icon.
- **Open (TODO before 0.1.0):** In-game verification of NPC purchase routing, cash credit to player (currently only log), GLB mesh via `S1MAPI.GltfLoader`, hardware shop listing (`S1API.Internal.Shops.ShopIntegration`), deposit/extract UI panel, ingredient credit into `NPC.Inventory` via `NPCSignal_UseVendingMachine`.
- Details + verification table: `README.md` in mod folder; design decisions: `docs/SPEC.md`.