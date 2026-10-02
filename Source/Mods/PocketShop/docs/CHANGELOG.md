# Changelog

## 0.3.9 (2026-10-02) — Directory fits on the FIRST open (no reopen workaround)

User report: "PocketShop braucht einen 2× Reload, damit die Icons gestreckt werden" —
the first directory build of a session measured the content viewport as ~100 units
(stale layout of the freshly activated app; live log 2026-10-02) instead of the real
~546, so every row fell to the 90dp floor and cards/avatars looked wrong until the
app was reopened.

- `StoreCatalogPane.ComputeFittedRowHeight` now forces the layout from the **topmost**
  rect under the canvas (app container → main vertical layout → content area) via
  `Canvas.ForceUpdateCanvases()` + `LayoutRebuilder.ForceRebuildLayoutImmediate`,
  instead of rebuilding only a mid-level rect (that was the v0.3.8 attempt).
- A viewport below `MinPlausibleViewportUnits` (300u) counts as stale: the immediate
  build uses a canvas-derived estimate (log: `directory viewport stale …`) and the app
  rebuilds the directory exactly on the next frame (`TickRetry`, bounded) — log:
  `directory rebuilt with the live viewport (546px) — no reopen needed`.
- The row log keeps its v0.3.8 shape (`directory rows=N rowH=Xpx viewport=Ypx`, plus
  `(estimated)` while stale), so the playtest evidence stays comparable.

## 0.3.8 (2026-10-01) — Row measurement fixed (rebuild root layout, not the panel)
- v0.3.6 measured the content panel without rebuilding its parent root layout, so the rect was stale/mini and the 90dp floor always won (small rows + black void again). Now the root panel is rebuilt before measuring; the canvas fallback uses the short side (`ActualWidth`) on the landscape phone. One log line per directory build (`directory rows=N rowH=Xpx viewport=Ypx`) proves the fit.

## 0.3.7 (2026-10-01) — Arms card shows Manny's portrait again (user choice)
- Stan Carney is the correct weapons dealer but ships no `MugshotSprite` in vanilla data, so the arms card fell back to the crosshair. Per user decision the card now uses Manny the Fixer's portrait as visible fallback (wrong person, real face): Stan stays first in line (shopkeeper-field + keyword) and wins automatically once TVGS ships his mugshot; Manny is second; crosshair last.

## 0.3.6 (2026-10-01) — Exact-fit store rows, arms fallback confirmed intentional
- Store rows now measure the live viewport and divide it exactly (`max(90dp, (viewport − padding − gaps) / rows)`): 12 stores fit on screen with no clipping, no scroll and no black void; more stores fall back to 90dp rows + scroll. Banner 48dp, avatar bay min 65dp (avatars 66dp, fonts 13/10sp unchanged from v0.3.5).
- Arms Dealer: the v0.3.4 diagnostic proved Stan Carney's `MugshotSprite` is `null` in vanilla game data (his `ShopInterface` correctly points at `armsdealer`). The crosshair fallback is intentional until TVGS ships a mugshot — resolution picks it up automatically, no code change needed. Temporary DIAG removed.

## 0.3.5 (2026-10-01) — Version footer removed, store cards fill the screen
- The `PocketShop vX.Y.Z` footer bar is gone (user request) — the version now lives only in the startup log, `mod.json` and the assembly metadata. Store rows grow from a 105dp floor to 150dp and stretch over the freed area (`flexibleHeight`), so no black void remains below the grid; avatars grow 50→66dp (ring 52→68dp), banner 42→50dp, fonts 11→13 / 9→10sp. More than 12 stores still scroll as before.

## 0.3.4 (2026-10-01) — Arms Dealer diagnostic (temporary)
- The Warehouse weapons card falls back to the crosshair symbol: Stan Carney resolves through no path (no log line at all). One-shot diagnostic logs the arms shop's catalog code/name, whether `stan_carney` is in the NPC registry, his mugshot state, his `ShopInterface` code, and every shopkeeper-coded NPC — the next playtest log shows exactly why. No behavior change otherwise.
- Removes again once the cause is known and fixed.

## 0.3.3 (2026-10-01) — Shop portrait resolution fixed (Fiona, Herbert, Gas-Mart clerks; Stan corrected)
- Four store cards (Thrifty Threads, Bleuball's Boutique, Gas-Mart West, Gas-Mart Central) showed the procedural fallback symbol instead of the shopkeeper's portrait. Avatars now resolve through the game's own data first: the six NPC classes carrying a `ShopInterface` reference (Dan, Fiona, Herbert, Oscar, Stan, Steve) are matched by that reference against the shop — no more name-guessing for these.
- **Gas-Marts show the clerk on duty:** West = Chloe Bowers (day) / Charles Rowland (night); Central = Meg Cooley (day) / Javier Pérez (night); day = 06:00–18:00 in-game time; day and night portraits are cached separately.
- Keyword fallback extended/cleaned: `thrifty` → `fiona_hancock`, `bleuball` → `herbert_bleuball`; `arm`/`weapon` now maps to the actual weapons dealer **Stan Carney** (previously showed Manny, the fixer — wrong person).
- Portrait resolution logs one line per shop (`[PocketShop] portrait <shop> -> <npc> (<source>)`) for verification.

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