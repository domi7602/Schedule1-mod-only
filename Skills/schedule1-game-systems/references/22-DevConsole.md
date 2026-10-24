# DevConsole (Schedule I)

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 7 of 7 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).

## Activation
Resolve the target build’s supported console setting or activation path first. The old note proposed editing `Game.json` and also named an unrelated movement multiplier; neither establishes a current console activation contract. Do not alter a save or memory merely to inspect available commands.

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
