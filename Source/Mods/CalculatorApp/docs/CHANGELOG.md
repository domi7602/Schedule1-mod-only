# Changelog

All notable changes to **CalculatorApp** are documented in this file.

## [0.2.1] - 2026-09-10

### Fixed
- Statischer Event-Dispatcher gegen Subscriber-Leaks.
- Pointer-/WasCollected-Guards im Slot-Suffix (GetActiveSlotSuffix).

## [0.2.0] - 2026-08-14

### Added
- **Exact Decimal Arithmetic:** Upgraded math engine from `double` to `decimal`, eliminating IEEE 754 floating-point inaccuracies (e.g. `0.1 + 0.2` or drug unit batch computations).
- **In-Game Balance Quick-Insert:**
  - `[ 💵 Cash ]`: Ingests the player's live cash balance via `S1API.Money.Money.GetCashBalance()` into the calculator with 1 tap.
  - `[ 💳 Bank ]`: Ingests the player's live online bank balance via `S1API.Money.Money.GetOnlineBalance()`.
- **Windows Clipboard Integration:**
  - `[ 📋 Copy ]` action chip & <kbd>Ctrl+C</kbd> copies displayed value to Windows clipboard.
  - <kbd>Ctrl+V</kbd> pastes numbers from clipboard directly into active calculation.
- **Scientific Quick-Actions:**
  - `[ √ Sqrt ]` chip computes square roots directly.
  - Repeated equals (`=`) operation support.
- **Per-Item History Actions:**
  - `[ 📋 ]` button to copy individual past calculation results.
  - `[ 🗑️ ]` button to delete specific historical records without wiping the entire history.
- **Input Focus Guard:**
  - `CalculatorAppInputFocus` component prevents player movement (WASD) and game hotkey triggers when searching in calculation history.
- **Resilient Persistence:**
  - Integrated `S1Mods.Shared.SafeStorage` with atomic writes (`.tmp` -> `.bak` -> replace) to protect `calculator_state.json` from corruption.
- **Dynamic C / AC Display:**
  - Clear button automatically toggles between `C` (Clear Entry) when a number is dirty and `AC` (All Clear) when clear.

### Fixed
- Fixed IL2CPP delegate marshaling crash hazard on `_historySearchInput.onValueChanged` by using S1API `EventHelper.AddListener<string>`.
- Fixed sign negation `±` creating invalid strings like `"-Cannot divide by 0"` when errors are present.

## [0.1.0] - 2026-08-13

### Added
- Initial release of **CalculatorApp** for *Schedule I* using S1API 3.1.9+.
- 4×5 Keypad grid with addition, subtraction, multiplication, division, percentage, and sign negation.
- Dual-line display showing current input and sub-expression formula.
- State persistence saving current digits and expressions to `UserData/CalculatorApp/calculator_state.json`.
- Searchable Calculation History ("Suchverlauf") with real-time text filter and tap-to-restore.
- Physical keyboard and numpad shortcuts while calculator is open.
- High-resolution programmatic icon and asset fallback.
