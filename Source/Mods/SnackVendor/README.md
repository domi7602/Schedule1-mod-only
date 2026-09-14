# SnackVendor

Player-stocked vending machine for Schedule I (MelonLoader / IL2CPP / Unity 2022.3,
.NET 6). Lets you fill a vending machine with ingredients from the gas market and
passively earn cash when NPCs buy from it.

## MVP build status (2026-09-14)

This is the **first integrated build**. The hookup from placing to NPC-buying
runs end-to-end through the live Cuke-Machine clone plus a marker-guarded Harmony
gate, but several sub-features are still pending verification in-game before
they can be called stable:

| Feature | Status |
|---|---|
| Buildable 1×1 item registered (`snackvendor`) | ✅ registered via S1API Builder |
| Placement via vanilla Build/Place flow | ✅ wired (`BuildableItem.Start` postfix) |
| Vanilla Cuke-VendingMachine cloned onto each placed station | ⚠ wired but not yet in-game verified |
| NPCs can route purchases to our stations | ⚠ patched (marker-guarded prefix), unverified |
| Stock data persisted in `snacks_slot_{n}.json` sidecar | ✅ loaded on LoadComplete, written on first purchase |
| Cash credit to player wallet | ⚠ logged-only (vanilla cash API not yet mapped) |
| Hardware-shop listing (so player can buy it in-game) | ⚠ skipped (needs S1API ShopIntegration bridge) |
| Custom 3D mesh via Blender-authored GLB | ⚠ engine wiring left; mesh asset ready in `assets/SnackVendor_model.glb` |
| Player deposit / extract UI panel | ❌ not yet built |

**Spike-Gate (next in-game run)**: confirm that the cloned VendingMachine
registers in `VendingMachine.AllMachines` and that NPCs route purchases through
the marker-guarded prefix.

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

1. **Verify in-game that the cloned VendingMachine actually shows up in
   `VendingMachine.AllMachines` and NPCs route through our prefix.**
2. Wire the cash credit (currently logged-only) through the vanilla wallet
   flow — `Player.Network.AddCash` is not the right call in v0.4.6f13 and we
   have not yet located the canonical cash-credit API.
3. Wire the GLB through `S1MAPI.GltfLoader.LoadGlb` to actually replace the
   mesh (currently a placeholder red proxy box).
4. Wire `S1API.Internal.Shops.ShopIntegration.AddItemToShop` so the player
   can buy the station in-game (currently requires S1MCP-spawn workaround).
5. Build the UI panel that lets the player deposit / extract ingredients;
   the stock-purchase intercept already works, the user-facing side doesn't.
6. Capture markers from `NPCSignal_UseVendingMachine` (the NPC reference is
   in scope there) to actually credit ingredient items into `NPC.Inventory`
   rather than only crediting cash + decrementing stock.

## How to verify the build

`dotnet build Source/Mods/SnackVendor/src/SnackVendor.csproj -c Release`. The
built DLL is automatically copied to `<Game>/Mods/SnackVendor.dll` by the
shared deploy pipeline (same as AutoPack / BackpackMod).

## See also

`docs/SPEC.md` — design decisions and the full build-up pipeline.
