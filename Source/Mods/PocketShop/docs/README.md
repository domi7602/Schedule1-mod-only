# PocketShop

**PocketShop** is a polished in-game phone app for *Schedule I* (v0.4.7, IL2CPP) that lets the player buy directly from the city's merchants via the smartphone, anywhere on the map.

---

## 🌟 Features (v0.3.2)

- **2-Level Navigation**:
  - **Level 1 (Store Directory)**: 4×3 grid with all city merchants, themed vector icons and merchant portraits.
  - **Level 2 (Item Grid)**: 5-column item grid with prices, stock, image and quantity selector.
- **🔒 Level & Rank Requirement Enforcement**:
  - Automatically respects vanilla item lock requirements (`RequiresLevelToPurchase` / `RequiredRank`).
  - Locked items clearly display `🔒 LOCKED` with the BUY button disabled (the rejection notice names the required rank).
  - Configurable via `EnforceLevelRequirements` (default: `true`).
- **🔢 Direct Inline Quantity Input (Dan's Hardware Style)**:
  - Click the quantity number directly between `[-]` and `[+]` on any item card.
  - Type any number (e.g. `20` or `40`) inline with automatic stock clamping upon completion.
  - Full WASD-movement protection via `PocketShopInputFocus` (`Controls.IsTyping`) while editing.
- **💳 Payment Follows the Shop (vanilla `PaymentType`, since v0.3.1)**:
  - No payment switcher: each purchase uses the shop's own vanilla payment rule.
  - Cash shops debit your wallet, card shops your bank account; `PreferCash` / `PreferOnline` shops accept both and try the preferred source first.
- **🧾 Purchase Feedback**:
  - Purchase results appear as a status line under the item grid (`ItemGridPane.ShowStatus`).
  - The BUY label shows the fee-inclusive total (e.g. `BUY $132 CASH` — the `CASH` badge only marks payment that deviates from card).
  - Closed or level-locked shops show `CLOSED` / `SHOP LOCKED` on the BUY button, which is disabled.
- **🔊 Sound Effects (SFX)**:
  - Real cash register chime on successful purchase, subtle clicks on key interactions, and alarm tone on rejected purchase.
- **⚙️ Configuration & Persistence**:
  - Configurable service markup (`ServiceFeePercent`, default 10%).
  - Adjustable flat delivery fee (`DeliveryFeeFlat`).
  - Enforce vanilla level locks (`EnforceLevelRequirements`, default `true`).
  - Enable/disable sound effects (`EnableSoundEffects`).

---

## ⚙️ Configuration

The settings are stored in `UserData/MelonPreferences.cfg` (MelonPreferences category `PocketShop`, via `Shared.ModConfig`). Settings schema:

```json
{
  "ServiceFeePercent": 10.0,
  "DeliveryFeeFlat": 0.0,
  "SelectedPaymentMode": "Bank",
  "EnableSoundEffects": true,
  "EnforceLevelRequirements": true,
  "LastShopIndex": 0
}
```

`SelectedPaymentMode` is a legacy v0.2.x key kept for config compatibility — it no longer affects payment behaviour (the shop's vanilla `PaymentType` decides since v0.3.1).
