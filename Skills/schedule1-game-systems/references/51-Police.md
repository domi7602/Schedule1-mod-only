# Police (Schedule I)

> **Redirect stub** (consolidated 2026-10-05): the deep law-enforcement analysis (LE_Intensity mechanics +0.15/Tag, 4 pursuit levels, 16-crime fines table, arrest constants) lives in **[`12-Heat-Pursuit-Law.md`](12-Heat-Pursuit-Law.md)**. This file keeps only the class register. Class-list only — not yet re-verified.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 16 of 16 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Core Classes

| Class | Purpose |
|-------|---------|
| `PoliceOfficer` | Officer NPC (patrol/pursue/search, reacts to noise + sightings) |
| `NPCResponses_Police` | Police dialogues |
| `Investigation` | Ongoing investigation (crime discovery → pursuit escalation, evidence incl. footprints) |
| `Offense` / `OffenceNoticeUI` | Individual offense (severity, combinable) / notification |
| `RoadCheckpoint` / `CheckpointInstance` / `CheckpointManager` | Vehicle checkpoints |
| `ArrestScreen` / `ArrestNoticeScreen` / `BodySearchScreen` / `PickpocketScreen` | Arrest/search UI |
| `FootprintMatchData` | Evidence matching |

## Integration (details in 12)

Works with `LawManager`/Crime classes; pursuit combines Vision ([`62-Vision.md`](62-Vision.md)) + Noise ([`46-Noise.md`](46-Noise.md)) + Law.
