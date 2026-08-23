# PocketShop

**PocketShop** is a polished in-game phone app for *Schedule I* (v0.4.6f13) that lets the player buy directly from the city's merchants via the smartphone, anywhere on the map.

---

## 🌟 Features (v0.2.0)

- **2-Level Navigation**:
  - **Level 1 (Store Directory)**: 4×3 grid with all city merchants, themed vector icons and merchant portraits.
  - **Level 2 (Item Grid)**: 5-column item grid with prices, stock, image and quantity selector.
- **💳 Multi-Payment & Bank Payment**:
  - Interactive chips in the sub-header bar: `[ 💵 Cash ]`, `[ 💳 Bank Card ]`, `[ ⚡ Auto ]`.
  - Direct debit from the bank account via online transaction or cash payment.
  - Smart `Auto` mode (pays with cash, switches seamlessly to the bank account when needed).
- **🔍 Item Detail & Inspection Modal**:
  - Tap any item icon or name to open a high-resolution detail view.
  - Includes price breakdown, merchant source and quick-quantity chips (`[+1]`, `[+5]`, `[+10]`, `[MAX]`).
- **🔔 In-App Toast Notifications**:
  - Elegant animated status banner for purchase confirmations (🟢), insufficient-funds or full-inventory errors (🔴), and payment switching (🔵).
- **🔊 Sound Effects (SFX)**:
  - Real cash register chime on successful purchase, subtle clicks on key interactions, and alarm tone on rejected purchase.
- **⚙️ Configuration & Persistence**:
  - Configurable service markup (`ServiceFeePercent`, default 10%).
  - Adjustable flat delivery fee (`DeliveryFeeFlat`).
  - Enable/disable sound effects (`EnableSoundEffects`).

---

## ⚙️ Configuration

The settings are stored in `UserData/PocketShop/config.json` or `UserData/MelonPreferences.cfg`:

```json
{
  "ServiceFeePercent": 10.0,
  "DeliveryFeeFlat": 0.0,
  "SelectedPaymentMode": "Auto",
  "EnableSoundEffects": true,
  "LastShopIndex": 0
}
```
