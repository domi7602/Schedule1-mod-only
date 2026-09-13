# Regions (Schedule I)

## EMapRegion (6 Regions)
| Index | Region | Unlock Requirement |
|-------|--------|-------------------|
| 0 | Northtown | Default (always) |
| 1 | Westville | Rank Requirement (FullRank) |
| 2 | Downtown | Cartel Influence < 30% in Westville |
| 3 | Docks | Cartel Influence < 30% in Downtown |
| 4 | Suburbia | Cartel Influence < 30% in Docks |
| 5 | Uptown | Cartel Influence < 30% in Suburbia |

## Unlock Mechanics
1. **Default**: `UnlockedByDefault = true` (Northtown)
2. **Rank**: `LevelManager.onRankUp` → Westville (e.g. Street_Rat Tier 3)
3. **Cartel Influence**: `CartelInfluence.ChangeInfluence()` → unlocks next region when `Influence <= 0.3f`

## Cartel Influence
- Per region: `RegionInfluenceData.Influence` (0.0-1.0)
- Westville Cap: `WESTVILLE_MAX_INFLUENCE = 0.5f`
- **Decreases** per successful deal: `-0.075f`
- Primary ways to lower (besides dealers): Overwriting graffiti (-0.05), interrupting cartel graffiti (-0.1)

## Cartel Status (ECartelStatus)
`Unknown → Truced → Hostile → Defeated`

## Cartel Activities Per Region
`CartelRegionActivities` with:
- `CartelCustomerDeal` (cartel dealer makes transactions)
- `RobDealer` (ambush/rob player dealer)
- `StealDeadDrop` (theft from dead drops)
- `SprayGraffiti` (spray-paint cartel graffiti)

Cooldown: 12-48h (based on influence + player contract activity)

## Gameplay Impact
- Customers: Only contactable in unlocked regions
- Dealers: Only assignable in unlocked regions
- Deliveries: DeliveryLocations are region-bound
- NPCs: `NPC.Region` affects visibility in ContactsApp
