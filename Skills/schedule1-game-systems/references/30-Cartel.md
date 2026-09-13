# Cartel (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `Cartel` | Cartel main manager class (NetworkSingleton) |
| `CartelActivities` | Regional cartel activities manager |
| `CartelActivity` | Base class for individual activities |
| `CartelDealManager` | Manages cartel transactions/deals |
| `CartelInfluence` | Cartel influence tracking system |

## Cartel Influence

- `CartelInfluence`: Per-region influence level (0–100%)
- Increased or lowered through activities
- Lower player influence → higher cartel presence
- `CartelRegionActivities`: Activities assigned per region

## Cartel Activities

| Activity | Description |
|----------|-------------|
| `SprayGraffiti` | Spraying cartel graffiti (territory marking) |
| `CartelCustomerDeal` | Intercepting customer transactions |
| `CartelDealer` | Attacks on dealers |
| `RobDealer` | Robbing player dealers |
| `StealDeadDrop` | Stealing dead drop contents |
| `Ambush` | Cartel ambush events |

## Cartel Goons

- `CartelGoon`: Cartel enforcer / thug NPC
- `CartelGoonAppearance`: Randomized visual appearance
- `GoonPool`: Goon spawn pool manager
- `NPCResponses_CartelGoon`: Cartel dialogue responses

## Cartel Meetings

- `CartelMeetingController`: Meeting sequence controller
- `CartelDealInfo`: Deal parameters and information

## Dead Drops

- `DeadDrop`: Hidden dropoff / pickup points
- `StealDeadDrop`: Dead drop theft event
