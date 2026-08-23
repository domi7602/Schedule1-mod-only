# Heat/Pursuit/Law (Schedule I)

## Important: No Separate "Heat" System
There is **no** dedicated heat gauge. Instead:
- `LE_Intensity` (1-10) = Police Activity Level (global)
- `EPursuitLevel` = individual pursuit/wanted level (per player)

## LE_Intensity (Law Enforcement Intensity)
- Start: `LE_Intensity = 1`
- Increases daily by `+0.15` (at midnight)
- `DAILY_INTENSITY_DRAIN = 0.05f` is **defined but never called**
- Has **no** connection to player crimes (purely time-based)

## Pursuit Levels (EPursuitLevel)
| Level | Search Time | Escalation | Police Speed | Weapon |
|-------|-------------|------------|--------------|--------|
| Investigating | 60s | Immediate upon visual contact | 0.35x | None |
| Arresting | 25s | 25s → NonLethal | 0.7x | Baton |
| NonLethal | 30s | 120s → Lethal | 0.7–0.9x | Taser |
| Lethal | 40s | — | 0.7–0.9x | Pistol |

## Crimes (16 Crimes)
| Crime | Fine |
|-------|------|
| PossessingControlledSubstances | $5/unit |
| PossessingLowSeverityDrug | $10/unit |
| PossessingModerateSeverityDrug | $20/unit |
| PossessingHighSeverityDrug | $30/unit |
| AttemptingToSell | $150 |
| Evading | $50 |
| FailureToComply | $50 |
| ViolatingCurfew | $100 |
| Assault | $75 |
| DeadlyAssault | $150 |
| Vandalism | $50 |
| Theft | $50 |
| BrandishingWeapon | $50 |
| DischargeFirearm | $50 |
| DrugTrafficking | — |
| TransportingIllicitItems | — |
| VehicularAssault | — |

## Body Search
- Minigame: Search indicator over 8 slots
- Player can "hide" items (click/hold)
- Jumping boosts indicator (2.5x)
- On hit: CAUGHT → Escalation to Arresting
- On miss: CLEAR, vehicle search follows
- Cooldown: 30s

## Arrest
- `ARREST_RANGE = 2.75m`, `ARREST_TIME = 1.75s`
- Red circle on floor, leaving 3x → Escalation
- ArrestScreen: Crimes + fines + item confiscation
- Release: Teleported to police station

## Police Dispatch
- Max 4 officers per station, max 5 vehicles
- OfficerPool: NPCs in police station
- Vehicle dispatch if distance > 25m or target in vehicle

## Police Activities (Dependent on LE_Intensity)
| Type | Condition |
|------|-----------|
| PatrolInstance | LE >= IntensityReq |
| VehiclePatrolInstance | LE >= IntensityReq |
| CheckpointInstance | LE >= IntensityReq + Distance > 50m |
| SentryInstance | LE >= IntensityReq |
| CurfewInstance | LE >= IntensityReq + Sleep phase |

## Not Implemented
- **Bounty System** (completely missing)
- **Regional Wanted Level** (global per player)
- **Investigation** (stub class, incomplete)
