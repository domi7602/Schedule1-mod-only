# Cartel (Schedule I)

> **Redirect stub** (consolidated 2026-10-05): mechanics, influence values (−0.075/Deal, Westville-Cap 0.5) and region unlocks live in **[`13-Regions.md`](13-Regions.md)**. This file keeps only the class register. Class-list only — not yet re-verified.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 20 of 20 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Core Classes

| Class | Purpose |
|-------|---------|
| `Cartel` | Cartel main manager class (NetworkSingleton) |
| `CartelActivities` / `CartelActivity` | Regional activities manager / base class |
| `CartelDealManager` | Manages cartel transactions/deals |
| `CartelInfluence` | Per-region influence tracking (0–100%) |
| `CartelRegionActivities` | Activities assigned per region |
| `CartelGoon` / `CartelGoonAppearance` / `GoonPool` | Enforcer NPC + appearance + spawn pool |
| `NPCResponses_CartelGoon` | Goon dialogue responses |
| `CartelMeetingController` / `CartelDealInfo` | Meeting sequence / deal parameters |
| `DeadDrop` / `StealDeadDrop` | Hidden dropoff points / theft event |

## Activity types (details in 13-Regions.md)

`SprayGraffiti` · `CartelCustomerDeal` · `CartelDealer` · `RobDealer` · `StealDeadDrop` · `Ambush`

## Related

- Graffiti system: [`16-Graffiti.md`](16-Graffiti.md)
- Dead drops (S1API): `schedule1-s1api/references/game-systems.md`
