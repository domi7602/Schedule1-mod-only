# Changelog

## 0.3.2 (2026-09-20) — Cash-HUD-Feedback
- **Cash-Zahlungen zeigen jetzt das Vanilla-HUD-Popup:** `ChangeCashBalance` läuft mit `visualizeChange: true` (war `false` — Cash wurde stimmungslos abgezogen). Black-Market-Käufe zeigen jetzt "-$X" im HUD wie In-World-Dealer-Käufe. Alle Cash-Refund-Pfade (Partial-Delivery, Outer-Gap) ebenfalls visualisiert ("+$X").
- Karte-Zahlungen unverändert (`CreateOnlineTransaction` hat kein Visualize-Flag — Banking-Feed im Phone ist dort das Feedback).


## 0.3.1 (2026-09-19) — Shop-Payment-Regeln (Black Market = Cash, Clean = Card)
- **Korrektur der v0.3.0-Pauschalumstellung (User-Feedback):** Nicht alle Shops sind legal — PocketShop listingt auch Black-Market-Supplier, die vanilla **Cash** verlangen. v0.3.1 folgt jetzt der **Vanilla-Zahlungsregel jedes Shops** via `ShopInterface.PaymentType` (`EPaymentType`): `Cash` → `ChangeCashBalance`, `Online` → `CreateOnlineTransaction`, `PreferCash`/`PreferOnline` → beide Mittel mit entsprechender Präferenz.
- `ShopCatalog.Refresh` cacht `PaymentType` pro Shop/Item (`ShopPOCO.PaymentType`, `ItemPOCO.ShopPaymentType`), `PurchaseService.CanAfford` + Payment + Refund-Pfade arbeiten damit. Keine hardcoded Shop-Namen — mod-injizierte Shops erben ihre Vanilla-Regel automatisch.
- UI: Zwei nicht-interaktive Balance-Chips (💵 Cash + 💳 Card) statt einem — beide Konten sind relevant. Buy-Button zeigt 💵-Badge nur bei Cash-Shops (Karte bleibt der stille Default).
- Success-Toast nennt den Zahlungsweg ("Cash"/"Card"); Not-enough-funds-Meldung matcht den tatsächlich geprüften Kontotyp.
- **Verifikations-Befehl:** `pshop shops` (In-Game-Konsole) listet jeden registrierten Shop mit seiner Vanilla-Zahlungsregel — damit lässt sich die Cash-vs-Karte-Zuordnung live gegen externe Quellen prüfen. Hinweis: `PaymentType` ist ein serialized Inspector-Feld (TVGS setzt es pro Shop-Instanz in den Szenen-Daten); es existiert keine statische Code-Tabelle, nur der Live-Wert ist verlässlich.

## 0.3.0 (2026-09-19) — Card-Only Payment (Realismus: legale Shops zahlen per Karte)
- **Payment-Überarbeitung auf User-Wunsch:** PocketShop war als legales Geschäft modelliert, zog aber bisher Cash vom Spieler ab. In Schedule I zahlen legale Shops ausschließlich per Karte (`onlineBalance` / `CreateOnlineTransaction`) — Cash ist nur für illegale Transaktionen. Alle Käufe laufen jetzt über `CreateOnlineTransaction`; `ChangeCashBalance` wird nicht mehr berührt.
- **Auto-Modus entfernt:** Der Cash-first/Bank-fallback-Switcher (💵/💳/⚡) ist obsolet — Items landen ohnehin sofort im Inventar, ein Zahlungsmodus-Switcher ist ohne Nutzen. Drei interaktive Chips → ein einziger nicht-interaktiver 💳-Kontostand-Chip (schlankeres SubHeader-Layout).
- `PaymentModeStatic` resolviert immer `PaymentMode.Bank` (Setter = No-Op, Source-kompatibel); `SelectedPaymentMode` bleibt als Legacy-Feld in der Config (Bestand: Bestehende Configs laden weiter, Default jetzt `Bank`).
- Not-enough-funds-Fehlermeldung auf Kartenkontext verschlankt ("Not enough card funds"); BUY-Badge "[CARD]" entfernt (selbstverständlich). Refund-Pfade (Partial-Delivery, Outer-Gap) auf Online-Transaktionen umgestellt.


## 0.2.7 (2026-09-17)
- Version bump.



## 0.2.6 (2026-09-17) — Level-Lock Enforce & Inline Quantity Input
- **Vanilla Level & Rank Lock Enforcement**: PocketShop respects `StorableItemDefinition.RequiresLevelToPurchase` and `IsUnlocked`. Items requiring higher player level/rank are displayed with `🔒 LOCKED (REQUIRES [RANK])`, stock indicator shows `🔒 LOCKED ([RANK])`, and buy interactions are disabled both in ItemCard and ItemDetailModal (`BuyResult.LevelLocked`). Configurable via `EnforceLevelRequirements` (default: `true`).
- **Direct Inline Quantity Input (Dan's Hardware Style)**: Clicking the quantity number directly focuses an inline `InputField` right inside the selector between `[-]` and `[+]`. Players can immediately type any amount (e.g. 20 or 40) directly on the card or detail view without any popup HUD. Supports live pricing recalculation while typing, and automatic clamping to stock limits upon completion (`onEndEdit`).
- **Input Focus Protection (`PocketShopInputFocus`)**: While editing the inline quantity number, `S1API.Input.Controls.IsTyping` is active to protect against accidental WASD player movement or shortcut triggers. Unfocusing or pressing <kbd>Enter</kbd> restores game controls.

## 0.2.5 (2026-09-13) — Bug-Audit-Fixes Runde 5 (Audit 2026-09-13)
- UI-Refresh: `StoreCatalogPane.RefreshShopCount()` + `ItemGridPane.OnCatalogChanged`-Callback — Store-Count-Badge im Directory-Header aktualisiert sich live bei Catalog-Refresh.
- Cache-Invalidation: `ShopCatalog.Refresh()` invalidiert `_itemCache`/`_shopCache` aggressiv vor dem Lesen von `ShopInterface.AllShops` → keine "Ghost-Shops" mehr.
- QuantitySelector Edge-Case: `ChangeBySafe()` — clamped nach jeder Änderung gegen `EffectiveMax()` um Stock-Drops während +/−-Druck abzufangen.
- Versionsangabe in Assembly/JSON/UI-Footer auf „0.2.5" aktualisiert.

## 0.2.4 (2026-09-12) — Bug-Audit-Fixes (Audit 2026-09-12)
- `PurchaseService.BuyWithQuantity` liest jetzt den Live-Bestand (`item.SourceListing.CurrentStock` + `IsInStock`) statt nur den POCO-Snapshot. Schliesst Koop-Oversell: Mit-Spieler koennen nicht mehr die POCO-Stale-Stock-Luecke ausnutzen.
- `QuantitySelector.ClampTo` synchronisiert jetzt `_maxStockOrSentinel` (Feld nicht mehr readonly). MAX-Chip und OUT-Zustand folgen echten Bestand. Sentinel (-1) wird zuerst abgefangen (vorher: <=0-First-Branch hat unbegrenzte Items auf qty=1 zurueckgesetzt).
- `HandlePurchaseResult` ruft jetzt `_gridPane.NotifyStockChanged(itemId)` nach erfolgreichem Kauf — Grid-Karten zeigen neuen Stock-Badge + geclamptes QuantitySelector-Max.
- Neuer `ItemPOCO.ItemId` + `ItemCard.GetItemIdPublic` fuer den Cross-Card-Dispatch.
- Versionsangabe in Assembly/JSON/UI-Footer ist jetzt konsistent auf „0.2.4“.

## 0.2.3 (2026-09-11)
- ShopCatalog: Per-Handler-Invoke (ein toter Subscriber killt den Katalog nicht mehr) + CurrentStock-Fallback.
- ItemGridPane: defensives Re-Subscribe + WasCollected-Guard; TearDown entsorgt DirectoryPane.

## 0.2.2 (2026-09-10)

- Statischer Event-Dispatcher; ItemGridPane.Dispose() wird beim Teardown aufgerufen.
- Toter NPC-Mugshot-Scan entfernt (direkt Themed-Icon).
- Partial-Delivery refundet nur den ungelieferten Rest (fee-gerecht, Stock/Menge korrigiert).
- ShopCatalog.Refresh() guarded plus Reset bei Scene-Unload.

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
