# DarkMarket (Schedule I)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 9 of 9 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.


## Location
- **Warehouse**
- NPCs: **Igor** (Bouncer) + **Oscar** (Shopkeeper)

## Operating Hours
- **18:00 – 06:00** (Night, during curfew)
- Closes IMMEDIATELY during police pursuit (any player)

## Unlock Requirement
1. Knock on Igor's door
2. Requires sufficient `FullRank` (`UnlockRank`)
3. Once unlocked: permanently open (except during police pursuit lockouts)

## Inventory
- Fixed listings in the Inspector (`ShopListing[]`)
- Rank-locked items (`RequiresLevelToPurchase`)
- Daily/weekly restock (`RestockRate`)
- `ConditionalVisibility` (quest-dependent)

## Purchase
- Fullscreen shop UI via Oscar (NPC dialogue)
- **No negotiation** – fixed prices
- Payment via cash or online
- After unlock: deliveries via Oscar (`EnableDeliveries()`)

## Access Control
- Igor checks rank through peephole (`Peephole`)
- After unlock: Igor disappears
- Roller doors (`DarkMarketRollerDoors`) open upon vehicle approach

## Police Risk
- During pursuit → market closes IMMEDIATELY
- During operating hours (18:00–06:00), curfew is active
- Increased police presence at night
