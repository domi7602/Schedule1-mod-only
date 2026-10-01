# Changelog

## 0.3.2 (2026-09-20) — Cash HUD feedback
- **Cash payments now show the vanilla HUD popup:** `ChangeCashBalance` runs with `visualizeChange: true` (was `false` — cash was deducted without feedback). Black-market purchases now show "-$X" in the HUD like in-world dealer purchases. All cash refund paths (Partial Delivery, Outer Gap) are also visualized ("+$X").
- Card payments unchanged (`CreateOnlineTransaction` has no visualize flag — the banking feed in the phone provides feedback there).

## 0.3.1 (2026-09-19) — Shop payment rules (Black Market = Cash, Clean = Card)
- **Correction of the v0.3.0 blanket switch (user feedback):** Not all shops are legal — PocketShop also lists black-market suppliers that vanilla requires **cash** for. v0.3.1 now follows each shop's **vanilla payment rule** via `ShopInterface.PaymentType` (`EPaymentType`): `Cash` → `ChangeCashBalance`, `Online` → `CreateOnlineTransaction`, `PreferCash`/`PreferOnline` → both means with the corresponding preference.
- `ShopCatalog.Refresh` caches `PaymentType` per shop/item (`ShopPOCO.PaymentType`, `ItemPOCO.ShopPaymentType`); `PurchaseService.CanAfford` + payment + refund paths use it. No hardcoded shop names — mod-injected shops automatically inherit their vanilla rule.
- UI: Two non-interactive balance chips (💵 Cash + 💳 Card) instead of one — both accounts are relevant. The buy button shows the 💵 badge only for cash shops (card stays the silent default).
- Success toast names the payment route ("Cash"/"Card"); not-enough-funds message matches the actually checked account type.
- **Verification command:** `pshop shops` (in-game console) lists each registered shop with its vanilla payment rule — so you can live-check the cash-vs-card mapping against external sources. Note: `PaymentType` is a serialized inspector field (TVGS sets it per shop instance in scene data); there is no static code table, only the live value is reliable.

## 0.3.0 (2026-09-19) — Card-only payment (realism: legal shops pay by card)
- **Payment overhaul at user request:** PocketShop was modeled as a legal business, but previously deducted cash from the player. In Schedule I, legal shops only pay by card (`onlineBalance` / `CreateOnlineTransaction`) — cash is only for illegal transactions. All purchases now go through `CreateOnlineTransaction`; `ChangeCashBalance` is no longer touched.
- **Auto mode removed:** The cash-first/bank-fallback switcher (💵/💳/⚡) is obsolete — items land in the inventory immediately anyway, a payment-mode switcher has no use. Three interactive chips → one single non-interactive 💳 balance chip (slimmer subheader layout).
- `PaymentModeStatic` always resolves to `PaymentMode.Bank` (setter = no-op, source-compatible); `SelectedPaymentMode` stays as a legacy field in the config (preserved: existing configs keep loading, default now `Bank`).
- Not-enough-funds error message slimmed for card context ("Not enough card funds"); BUY badge "[CARD]" removed (self-evident). Refund paths (Partial Delivery, Outer Gap) switched to online transactions.

## 0.2.7 (2026-09-17)
- Version bump.

## 0.2.6 (2026-09-17) — Level-lock enforce & inline quantity input
- **Vanilla level & rank lock enforcement**: PocketShop respects `StorableItemDefinition.RequiresLevelToPurchase` and `IsUnlocked`. Items requiring higher player level/rank are displayed with `🔒 LOCKED (REQUIRES [RANK])`, the stock indicator shows `🔒 LOCKED ([RANK])`, and buy interactions are disabled both in ItemCard and ItemDetailModal (`BuyResult.LevelLocked`). Configurable via `EnforceLevelRequirements` (default: `true`).
- **Direct inline quantity input (Dan's Hardware style)**: Clicking the quantity number directly focuses an inline `InputField` inside the selector between `[-]` and `[+]`. Players can immediately type any amount (e.g. 20 or 40) directly on the card or detail view without any popup HUD. Supports live pricing recalculation while typing, and automatic clamping to stock limits upon completion (`onEndEdit`).
- **Input focus protection (`PocketShopInputFocus`)**: While editing the inline quantity number, `S1API.Input.Controls.IsTyping` is active to protect against accidental WASD player movement or shortcut triggers. Unfocusing or pressing <kbd>Enter</kbd> restores game controls.

## 0.2.5 (2026-09-13) — Bug-audit fixes round 5 (audit 2026-09-13)
- UI refresh: `StoreCatalogPane.RefreshShopCount()` + `ItemGridPane.OnCatalogChanged` callback — store-count badge in the directory header updates live on catalog refresh.
- Cache invalidation: `ShopCatalog.Refresh()` aggressively invalidates `_itemCache`/`_shopCache` before reading `ShopInterface.AllShops` → no more "ghost shops".
- QuantitySelector edge case: `ChangeBySafe()` — clamps against `EffectiveMax()` after every change to catch stock drops during +/− presses.
- Version stamp in assembly/JSON/UI footer updated to "0.2.5".

## 0.2.4 (2026-09-12) — Bug-audit fixes (audit 2026-09-12)
- `PurchaseService.BuyWithQuantity` now reads the live stock (`item.SourceListing.CurrentStock` + `IsInStock`) instead of only the POCO snapshot. Closes coop oversell: co-players can no longer exploit the POCO-stale-stock gap.
- `QuantitySelector.ClampTo` now synchronizes `_maxStockOrSentinel` (field no longer readonly). MAX chip and OUT state follow real stock. Sentinel (-1) is caught first (before: <=0-first branch reset unlimited items to qty=1).
- `HandlePurchaseResult` now calls `_gridPane.NotifyStockChanged(itemId)` after a successful purchase — grid cards show fresh stock badge + clamped QuantitySelector max.
- New `ItemPOCO.ItemId` + `ItemCard.GetItemIdPublic` for cross-card dispatch.
- Version stamp in assembly/JSON/UI footer is now consistently "0.2.4".

## 0.2.3 (2026-09-11)
- ShopCatalog: per-handler invoke (one dead subscriber no longer kills the catalog) + CurrentStock fallback.
- ItemGridPane: defensive re-subscribe + WasCollected guard; TearDown disposes DirectoryPane.

## 0.2.2 (2026-09-10)

- Static event dispatcher; ItemGridPane.Dispose() is invoked on teardown.
- Dead NPC mugshot scan removed (themed icon directly).
- Partial Delivery refunds only the undelivered remainder (fee-fair, stock/quantity corrected).
- ShopCatalog.Refresh() guarded plus reset on scene unload.

## 0.2.1 (2026-08-17)

- **Banner & animation removal**: Removed `ToastOverlay` and the green purchase-confirmation banner entirely for an interruption-free, clean shopping experience.

## 0.2.0 (2026-08-16)

- **Multi-payment switcher**: Interactive payment chips in the sub-header (`[💵 Cash]`, `[💳 Bank Card]`, `[⚡ Auto]`). Supports physical cash and online bank transfers (`MoneyManager.CreateOnlineTransaction`).
- **Auto-fallback payment**: In `Auto` mode cash is preferred; if it isn't enough, the purchase seamlessly switches to the bank account.
- **In-app toast notifications (`ToastOverlay`)**: Animated status banner for purchase confirmations (🟢), insufficient-funds or full-inventory errors (🔴), and payment switching (🔵).
- **Sound effects (`SoundService`)**: Native cash register chime (`MoneyManager.Instance.PlayCashSound()`), procedural chimes and rejection tones.
- **Item detail & inspection modal (`ItemDetailModal`)**: Tapping an item icon or name opens a detailed full view with price breakdown, quick-quantity chips (`[+1]`, `[+5]`, `[+10]`, `[MAX]`) and direct buy.
- **Extended configuration (`PocketShopConfig`)**: `PaymentMode` (`Cash`, `Bank`, `Auto`), `EnableSoundEffects` and `DeliveryFeeFlat`.

## 0.1.0 (2026-08-16)

- **Initial release**: PocketShop in-game phone app.
- **Drill-down navigation**: Instead of a flat item list, a shop list is shown first (`Shops` header). Each shop row shows name + item-count badge + `›` chevron. Tap → the shop's item list with a `[← Back]` header. Tap an item → detail pane with markup breakdown + buy button.
- **Live aggregation**: Reads `ShopInterface.AllShops` at runtime, collecting all `ShopListing[]` into a searchable catalog.
- **Instant purchase**: `[ Buy ]` in the item detail deducts cash via `MoneyManager.ChangeCashBalance(-total, true, true)` (animation + sound ON) and pushes the `ItemInstance` directly via `PlayerInventory.Instance.AddItemToInventory`.
- **Stock decrement**: `ShopListing.SetStock(stock-1, true)` decrements the vanilla stock after a successful purchase — the catalog reflects the current count.
- **Markup system**: `PocketShopConfig.ServiceFeePercent` (default 10%) is broken down in the UI (`$120 + $12 (10%) = $132`).
- **Retry loop**: 20×1.5s = ~30s retry mechanism when `AllShops` is initially empty (save-load-timing protection).
- **YAGNI v0.1.0**: No DarkMarket, no quantity selector, no multiplayer sync, no property delivery, no more category pills (drill-down replaces them).
- **Stack**: MelonLoader 0.7.3 + S1API 3.1.15 (PhoneApp + Money) + UnityEngine.UI.
- **Verified**: Build clean, `s1interop` clean (only the `global_usings_require_langversion` false positive).

## 0.1.0
- Initial version.