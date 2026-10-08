# DevConsole (Schedule I)
> UNVERIFIED for runtime 0.4.7f11 — carried-over knowledge; re-verify API details against the 0.4.7f9 decompiles before patching. Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.


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
