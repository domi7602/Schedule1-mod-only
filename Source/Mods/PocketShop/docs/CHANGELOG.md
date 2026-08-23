# Changelog

## 0.2.1 (2026-08-17)

- **Banner & Animation Removal**: Removed `ToastOverlay` and the green purchase-confirmation banner entirely for an interruption-free, clean shopping experience.

## 0.2.0 (2026-08-16)

- **Multi-Payment Switcher**: Interactive payment chips in the sub-header (`[💵 Cash]`, `[💳 Bank Card]`, `[⚡ Auto]`). Supports physical cash and online bank transfers (`MoneyManager.CreateOnlineTransaction`).
- **Auto-Fallback Payment**: In `Auto` mode cash is preferred; if it isn't enough, the purchase seamlessly switches to the bank account.
- **In-App Toast Notifications (`ToastOverlay`)**: Animated status banner for purchase confirmations (🟢), insufficient-funds or full-inventory errors (🔴), and payment switching (🔵).
- **Sound Effects (`SoundService`)**: Native cash register chime (`MoneyManager.Instance.PlayCashSound()`), procedural chimes and rejection tones.
- **Item Detail & Inspection Modal (`ItemDetailModal`)**: Tapping an item icon or name opens a detailed full view with price breakdown, quick-quantity chips (`[+1]`, `[+5]`, `[+10]`, `[MAX]`) and direct buy.
- **Extended Configuration (`PocketShopConfig`)**: `PaymentMode` (`Cash`, `Bank`, `Auto`), `EnableSoundEffects` and `DeliveryFeeFlat`.

## 0.1.0 (2026-08-16)

- **Initial release**: PocketShop in-game phone app.
- **Drill-Down Navigation**: Instead of a flat item list, a shop list is shown first (`Shops` header). Each shop row shows name + item-count badge + `›` chevron. Tap → the shop's item list with a `[← Back]` header. Tap an item → detail pane with markup breakdown + buy button.
- **Live Aggregation**: Reads `ShopInterface.AllShops` at runtime, collecting all `ShopListing[]` into a searchable catalog.
- **Instant Purchase**: `[ Buy ]` in the item detail deducts cash via `MoneyManager.ChangeCashBalance(-total, true, true)` (animation + sound ON) and pushes the `ItemInstance` directly via `PlayerInventory.Instance.AddItemToInventory`.
- **Stock Decrement**: `ShopListing.SetStock(stock-1, true)` decrements the vanilla stock after a successful purchase — the catalog reflects the current count.
- **Markup System**: `PocketShopConfig.ServiceFeePercent` (default 10%) is broken down in the UI (`$120 + $12 (10%) = $132`).
- **Retry Loop**: 20×1.5s = ~30s retry mechanism when `AllShops` is initially empty (save-load-timing protection).
- **YAGNI v0.1.0**: No DarkMarket, no quantity selector, no multiplayer sync, no property delivery, no more category pills (drill-down replaces them).
- **Stack**: MelonLoader 0.7.3 + S1API 3.1.15 (PhoneApp + Money) + UnityEngine.UI.
- **Verified**: Build clean, `s1interop` clean (only the `global_usings_require_langversion` false positive).

## 0.1.0
- Initial version.
