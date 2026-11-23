# ConsoleHotkey

Opens the vanilla in-game developer console with **F1**.

On the current Linux/Proton setup the physical backtick key (the `^°` key next to `1` on a German keyboard) already toggles the console, but the German XKB layout sends a dead key that the game does not recognise as the backtick. This mod adds an explicit `F1` toggle so a layout change is not needed.

## Usage

1. Start the game with the mod installed.
2. Load a save and press `F1` in gameplay.
3. Press `F1` again to close the console.

The console UI component (`UI/Console`) is not present in the main menu, so F1 has no effect there. The mod does not add new console commands and does not change game state. It only invokes the game's own `ConsoleUI.SetIsOpen` toggle. If `ConsoleEnabled` is disabled in the game settings, the UI still opens but commands may be rejected by the game.

## Tests

Executable tests can be added in a matching test project and the solution regenerated. This mod currently has no test project.
