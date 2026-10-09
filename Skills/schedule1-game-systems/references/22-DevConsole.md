# DevConsole (Schedule I)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 7 of 7 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.


## Activation
- **Not visible in normal gameplay**
- Activatable via save edit: `Game.json` → `"ConsoleEnabled": true`
- Or via Cheat Engine on `PlayerMovement.StaticMoveSpeedMultiplier`

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
- **~50 functional dev commands** present in the release build
- `GameManager.IS_BETA` blocks achievements (via `-beta` flag)
- `UNLOCK_ALL_REGIONS` (Editor only)

## Risk
- Using the console likely does not disable achievements (except when `-beta` flag is used)
- May corrupt save state if used improperly
