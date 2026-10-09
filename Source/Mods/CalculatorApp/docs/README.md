# CalculatorApp

A feature-rich, tactile **Calculator PhoneApp** for *Schedule I* (v0.4.7, IL2CPP) powered by the **S1API PhoneApp** framework and **S1Mods.Shared**.

## Features

- **Exact Decimal Arithmetic**:
  - High-precision `decimal` math engine eliminates floating-point rounding errors on financial, drug yield, and batch calculations.
  - Operations `+`, `-`, `×`, `÷`, `%`, sign negation `±`, square root `√`, dynamic `C` / `AC`, backspace `⌫`, and repeated equals (`=`).
- **Schedule I In-Game Money Integration**:
  - `[ 💵 Cash ]`: Instantly pulls live cash balance via `S1API.Money.Money.GetCashBalance()` into the calculator.
  - `[ 💳 Bank ]`: Pulls live online bank account balance via `S1API.Money.Money.GetOnlineBalance()`.
- **Clipboard Integration**:
  - `[ 📋 Copy ]` button & <kbd>Ctrl+C</kbd> copies displayed numbers directly to the Windows clipboard.
  - <kbd>Ctrl+V</kbd> pastes clipboard numbers into the calculator.
- **Searchable Calculation History ("Suchverlauf")**:
  - Dedicated **History** screen with real-time query search.
  - Tap any card to restore the result to the calculator.
  - Per-item `[ 📋 ]` (Copy result) and `[ 🗑️ ]` (Delete single record) action buttons.
  - "Clear All History" action button.
- **Input Focus Protection**:
  - `CalculatorAppInputFocus` prevents character movement (WASD) and game shortcut triggers while searching history.
- **SafeStorage Persistence**:
  - Atomic JSON persistence with `.bak` backup protection — one file per save slot (`UserData/CalculatorApp/calculator_state_slot_<N>.json`); a legacy `calculator_state.json` is migrated automatically.
- **Physical Keyboard & Numpad Support**:
  - Keypad tab: `0-9` (numpad or number row), `+`, `-`, `*`, `/`, `.`/`,`, `Enter`/`=`, `Backspace`, `Delete`/`C` (clear).
  - Both tabs: <kbd>Ctrl+C</kbd>, <kbd>Ctrl+V</kbd>, <kbd>Tab</kbd>/<kbd>Ctrl+H</kbd> (switch between keypad and history).

## Installation & Build

Build the mod with dotnet:
```pwsh
dotnet build Source/Mods/CalculatorApp/src/CalculatorApp.csproj -c Release
```
The compiled `CalculatorApp.dll` and metadata are automatically deployed to your game's `Mods/` folder.
