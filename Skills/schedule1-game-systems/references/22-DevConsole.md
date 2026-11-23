# DevConsole (Schedule I)

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 7 of 7 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

> Inherited, not rerun; [evidence scope](README.md#source-history).

## Activation
Resolve the target build’s supported console setting or activation path first. The old note proposed editing `Game.json` and also named an unrelated movement multiplier; neither establishes a current console activation contract. Do not alter a save or memory merely to inspect available commands.

> **2026-11-20 addendum (runtime evidence):** On f12 the console UI is `Il2CppScheduleOne.UI.ConsoleUI` at `UI/Console`. Its input action `ToggleConsoleReference` is an Input System binding (the backtick key); the public toggle is `ConsoleUI.SetIsOpen(bool)`. The gating setting is `GameSettings.ConsoleEnabled` (in the save `Game.json`), exposed by `GameSettingsWindow.ConsoleToggle`. There is no `Toggle()` method and no static `Instance`; resolve the component by path or `FindObjectOfType`. `GameManager.IS_BETA` is unrelated to console activation. The `ConsoleHotkey` mod calls `SetIsOpen` on F1 and was live-verified under Linux/Proton after a save was loaded; the UI does not exist in the main menu.

## Known Console Commands
| Command | Effect |
|---------|--------|
| `cleartrash` | Immediately deletes all trash |
| `addxp <amount>` | Adds XP |
| `changecash <amount>` | Modifies cash balance |
| `give <itemID> <amount>` | Grants items |
| `setowned <propertyCode>` | Sets property state to owned |
| `setqueststate <guid> <state>` | Sets quest state |
| `teleport <x> <y> <z>` | Teleports player |
| `growplants` | Causes plants to grow instantly |

## Additional DevUtilities
- The original notes reported roughly 50 dev-command candidates; registration, arguments and availability must be inspected in the target build.
- `GameManager.IS_BETA` blocks achievements (via `-beta` flag)
- `UNLOCK_ALL_REGIONS` (Editor only)

## Risk
- Achievement behavior is unverified for the target build. A command name in a registry is not proof of its effect.
- For requested state changes, use a reversible test context, inspect the exact argument contract and verify the resulting state; a success string alone is insufficient.
