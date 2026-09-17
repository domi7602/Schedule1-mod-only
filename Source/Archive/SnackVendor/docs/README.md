# SnackVendor (Schedule I)

Player-stocked vending machine: buy a machine, fill it with ingredients from the
gas market, and let NPCs buy from it for passive cash.

**Version:** `0.0.2-mvp` · **Status:** MVP spike — build + placement are wired,
in-game verification of the NPC purchase routing and payout is still pending.

## Feature status

| Feature | Status |
|---|---|
| Buildable 1×1 item registered (`snackvendor`) | registered via S1API Builder |
| Placement via vanilla Build/Place flow | wired (`BuildableItem.Start` postfix) |
| Vanilla Cuke-VendingMachine cloned onto each placed station | wired, not yet in-game verified |
| NPCs can route purchases to our stations | patched (marker-guarded prefix), unverified |
| Stock data persisted in `snacks_slot_{n}.json` sidecar | loaded on LoadComplete, written on first purchase |
| Cash credit to player wallet | logged-only (vanilla cash API not yet mapped) |
| Hardware-shop listing | skipped (needs the S1API ShopIntegration bridge) |
| Custom 3D mesh via Blender-authored GLB | engine wiring left; asset ready in `assets/SnackVendor_model.glb` |
| Player deposit / extract UI panel | not yet built |

## Installation

1. Install **MelonLoader 0.7.3+** and **S1API 3.2.0** (see the main repository README).
2. Copy `SnackVendor.dll` into `<GameDir>\Mods\`.
3. Copy `mod.json` (and, if present, `SnackVendor.pdb`) into `<GameDir>\UserData\SnackVendor\` — never into `Mods\`.
4. Launch the game. Configuration lives in `UserData/SnackVendor/` (price, slot count, hardware-shop name, payout clamps).

## Notes

- Vanilla Cuke vending machines are never modified — Harmony patches only act on
  instances that carry our `SnackVendorMarker`.
- Persistence is save-slot isolated (`snacks_slot_{n}.json`) with a sentinel-based
  "slot not resolved" guard.
- Developer status table, build instructions and the open TODO list:
  `README.md` in the mod folder (`Source/Mods/SnackVendor/README.md`).