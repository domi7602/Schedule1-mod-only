# Changelog

All notable changes to `ProductAppQoL` are documented here. Format: [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), versioning: [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.1.1 (2026-11-12)
- Version bump.

## 0.1.1 (2026-11-12)
- **Fixed search bar and sort controls above the product list.** Added `ProductToolbarController` (pinned above the vanilla `ScrollRect` via `Transform.parent` walk) and `ProductListLogic`:
  - **Search (`Search name or effect...`) + Clear (`×`):** Case-insensitive multi-token filter matching both product names and effect names (`ProductDefinition.Properties`). Matching categories auto-expand and empty categories hide while searching; clearing restores the previous expanded state.
  - **Typing focus guard (`ProductSearchInputFocus`):** Acquires/releases `Controls.IsTyping` only while the search input is focused and active, and releases on disable/app close.
  - **Sorting:** Cycle button + direction toggle (`↓` / `↑`) supporting `Default`, `Price` (first after `Default`, descending by default), `Name`, `Value` (`MarketValue`), `Addict` (`GetAddictiveness()`), and `Listed` (`ProductManager.ListedProducts`) with deterministic original-order tie-breaking.
  - **Mix Rename & Delete (`ProductDetailActions`):** For player-created mixes (`ProductManager.createdProducts`) in `ProductAppDetailPanel`, added `Rename` (inline `InputField` with `Controls.IsTyping` focus guard, updates `Name` while preserving `ID` and sets `HasChanged = true`) and `Delete` (2-step inline confirmation warning that owned items vanish; removes the mix from `DiscoveredProducts`, `ListedProducts`, `FavouritedProducts`, `createdProducts`, `AllProducts`, `ProductPrices`, `mixRecipes`, and the app UI). Base products stay protected.
  - **Live refresh:** Hooks `ProductManagerApp.SetOpen`, `CreateEntry`, `CreateFavouriteEntry`, and `OnProductListedEvent`.
- Release build 0 warnings/errors; deployed. Earlier in-game verification (`SaveGame_1`, `diffCount=0` across all 38 save files) confirmed toolbar layout, name search (`purple`), effect search (`sedating`), clear (`×`), `Sort: Name` (`↑`), and detail-panel selection; in-game verification of `Rename`, `Delete`, and the updated `Price`-first sort order is open.

## 0.1.0 (2026-10-10)
- **Mouse/touch support for the asking-price −/+ buttons.** Vanilla wires `Products > AskingPrice/Container/{Add,Remove}` through `ScheduleOne.GamepadInput.InputValueRamp` (hold-to-accelerate gamepad adjustment), so plain mouse clicks do nothing and mouse users can only type the price (`PriceSubmitted`). `PriceButtonMouseInput` is added alongside the untouched vanilla Button and handles pointer input: tap = ±$1 (`ProductAppDetailPanel.AdjustPrice(float change)` + `UpdatePrice()`), hold = repeat that accelerates from 0.13 s to 0.05 s after a 0.4 s delay — mirroring the gamepad ramp feel. Gamepad and typed input stay unchanged (listeners only, no vanilla state replaced).
