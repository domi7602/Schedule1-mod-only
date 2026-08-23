# Sewer (Schedule I)

## Status: FULLY IMPLEMENTED (Not Cut Content)

## Access
- Locked main door (`SewerDoorController`)
- Requires **Sewer Key** (obtainable via 3 ways):
  1. **Random NPC** (`KeyPossessor[]`): Carrying key, dialogue: "Do you have a sewer key?"
  2. **Jen**: Purchase available at relationship level 3.0 ("Friend")
  3. **World Spawn Key**: Oscar gives region hint, key spawns at random location
- After unlocking: permanently open (for all MP players)
- Entrance via manhole cover (`ManholeCover`) + ladder (`Ladder`)

## Sewer Goblin
- **10% chance per hour** (12h cooldown between checks)
- 3 states: Inactive → Attacking → Retrieving → Retreating
- Retreat trigger: HP < 50%, or 30% chance per hit taken
- Pacification: Hold `PacifyItem` in hand (<10m)
- Retrieval: Goblin steals item from player inventory
- Respawn: full HP regeneration, `HoursSinceLastDeploy` is persisted

## Sewer King (Boss)
- Guards the `SewerOffice`
- Attacks when player is in `CurrentProperty == sewerOffice`
- `CombatBehaviour` (standard combat system)
- After defeat: permanently deactivated (`HasSewerKingBeenDefeated`)
- **No special phases or mechanics in code**

## Sewer Office
- Full-fledged `Property` (purchasable via passcode)
- `OnPasscodeCorrect()` → `SetOwned_Server()`
- Default save under `DefaultSave/Properties/Sewer Office.json`

## Sewer Mushrooms (Mushroom Farming)
- 15 spawn locations across 3 areas
- Max 12 mushrooms simultaneously
- 20% daily respawn rate
- Persisted in `SewerData.ActiveMushroomLocationIndices`

## Visual System
- Custom `SewerCameraPresense`: Sky profile + post-processing volume
- `SewerAmbientSound`: Background ambiance, volume managed via CameraPresence
- `SewerSkyProfileOverride` and `SewerPPVolume` blended via lerp

## Persistence
- `SewerData`: IsSewerUnlocked, HasSewerKingBeenDefeated, HoursSinceLastSewerGoblinAppearance, RandomKeyPossessorIndex, ActiveMushroomLocationIndices
- `SewerLoader` restores state upon loading
