# SnackVendor

Player-stocked vending machine for Schedule I (MelonLoader / IL2CPP / Unity 2022.3,
.NET 6). Lets you fill a vending machine with ingredients from the gas market and
passively earn cash when NPCs buy from it.

## MVP build status (2026-09-16, v0.0.5 Spike-Completion)

The hookup from placing to NPC-buying runs end-to-end through the live Cuke-Machine
clone plus a marker-guarded Harmony gate. v0.0.5 wired the remaining spike TODOs
(GLB mesh, NPC inventory credit, deposit/extract panel); everything below still
awaits in-game verification before 0.1.0:

| Feature | Status |
|---|---|
| Buildable 1×1 item registered (`snackvendor`) | ✅ registered via S1API Builder |
| Placement via vanilla Build/Place flow (+ HomelessMod street path) | ✅ wired (`BuildableItem.Start` postfix + `SetupPlacedStation`) |
| Vanilla Cuke-VendingMachine cloned onto each placed station | ⚠ wired but not yet in-game verified |
| NPCs can route purchases to our stations | ⚠ patched (marker-guarded prefix), unverified |
| Stock data persisted in `snacks_slot_{n}.json` sidecar | ✅ loaded on LoadComplete, written on first purchase; **v0.0.5:** stock identity switched to registry string IDs (legacy numeric rows dropped on restore) |
| Cash credit to player wallet | ⚠ wired (`EconomyHelper.ChangeCashBalance` → `S1API.Money.Money`, same MP-verified path as HitmanPhone since v0.1.9); in-game verify pending |
| Hardware-shop listing | ⚠ wired via `InjectHardwareStoreListing` + `Shared.ShopListingSync` (Add → Initialize → UI-row, equivalent of S1API ShopIntegration); in-game verify pending |
| Custom 3D mesh via Blender-authored GLB | ⚠ **v0.0.5 wired** via `S1MAPI.Gltf.GltfLoader.LoadGlb` (URP shader fix, vanilla renderers hidden on success, fallback keeps clone mesh); auto-deployed to `Mods\SnackVendor\`; in-game verify pending |
| NPC ingredient credit (`NPC.Inventory`) | ⚠ **v0.0.5 wired** via `NPCSignalPatches.Purchase_Prefix` capture + `SendPurchase` transfer (`GetDefaultInstance(1)` → `InsertItem`); host-gated; in-game verify pending |
| Player deposit / extract UI panel | ⚠ **v0.0.5 wired** (IMGUI panel on `Interacted`, +1/+5/All deposit with `GetCopy` rollback, Take 1/All extract with capacity probe); in-game verify pending |

**Spike-Gate (next in-game run)**: confirm that the cloned VendingMachine
registers in `VendingMachine.AllMachines`, that NPCs route purchases through
the marker-guarded prefix, and that the full v0.0.5 chain holds end-to-end:
GLB mesh visible → NPC buys → stock decrements → cash credited → ingredient
lands in `NPC.Inventory` → panel deposit/extract round-trips → save/reload
restores stock.

## What's actually wired

- `Mod.cs` — Melon entry, config init, IL2CPP-injected MonoBehaviour types,
  Harmony patches, S1API lifecycle subscriptions.
- `World/SnackVendorController.cs` — owns stock, swaps the cloned vanilla mesh
  for our GLB (proxy until the GLB is wired through `S1MAPI.GltfLoader`), calls
  stock decrement on purchase, persists slots.
- `World/SnackVendorMarker.cs` — gate marker on each cloned VendingMachine;
  Harmony patches only act on instances that carry this marker.
- `World/VendingMachinePatches.cs` — Harmony prefixes on `SendPurchase`,
  `PurchaseRoutine`, `DropItem`, `DropCash`. Vanilla Cuke-machines are untouched.
- `World/BuildableItem_Start_Patch.cs` — attaches the controller when a station
  finishes being placed.
- `World/BuildOrLoadGhostPrefab.cs` — procedural proxy ghost (red snack-bar
  box) used as place-mode preview and a procedural 64×80 icon sprite for the
  shop UI.
- `Items/SnackVendorItemFactory.cs` — registers the `snackvendor` Buildable
  Item using `BuildableItemDefinitionBuilder` cloning a vanilla base item
  (`storage`/`pot`/`packagingstation`).
- `Persistence/SnackSlotData.cs`, `Persistence/SnackVendorStore.cs` — slot-
  specific sidecar JSON with sentinel-based "slot not resolved" guard.
- `Config/SnackVendorConfig.cs` — every tunable (price, slot count, hardware
  shop name, payout clamps).
- `assets/build_snackvendor_model.py` + `SnackVendor_model.glb` — Blender
  headless build of the visual model.

## Known TODO before the 0.1.0 release

All five spike TODOs are **wired as of v0.0.5** — what remains is the
in-game verification pass itself (no code left on this list):

1. Verify in-game that the cloned VendingMachine shows up in
   `VendingMachine.AllMachines`, NPCs route through the prefix, and the
   cash credit lands in the wallet (`EconomyHelper.ChangeCashBalance` →
   `S1API.Money.Money` — the same payout path HitmanPhone uses, MP-verified
   since v0.1.9; only the vending context itself is unverified).
2. Verify the GLB mesh renders via `S1MAPI.Gltf.GltfLoader.LoadGlb`
   (vanilla clone renderers hide only after a successful load; fallback
   keeps the clone mesh visible).
3. Verify the hardware-shop listing (buy the station at Handy Hank's —
   `InjectHardwareStoreListing` + `Shared.ShopListingSync` already run the
   Add → Initialize → UI-row sequence that `ShopIntegration.AddItemToShop`
   would perform).
4. Verify the deposit/extract panel (`Interacted` on our machines):
   deposit +1/+5/All with quality-preserving rollback, Take 1/All extract,
   auto-close on distance/ESC/scene change.
5. Verify the NPC ingredient credit: `NPCSignalPatches` captures the buyer,
   `SendPurchase_Prefix` transfers `GetDefaultInstance(1)` into
   `NPC.Inventory` (`CreditNpcInventory` config, default on).

## How to verify the build

`dotnet build Source/Mods/SnackVendor/src/SnackVendor.csproj -c Release`. The
built DLL is automatically copied to `<Game>/Mods/SnackVendor.dll` by the
shared deploy pipeline (same as AutoPack / BackpackMod).

## See also

`docs/SPEC.md` — design decisions and the full build-up pipeline.
