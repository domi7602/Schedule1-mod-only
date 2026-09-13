# Police (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `PoliceOfficer` | Police officer character |
| `Investigation` | Current investigation |
| `Offense` | Offense |
| `RoadCheckpoint` | Road checkpoint |

## Police Behavior

- `PoliceOfficer`: NPC with specialized AI behavior
- Patrolling, pursuing, searching
- React to noise events and sightings
- `NPCResponses_Police`: Police dialogues

## Investigations

- `Investigation`: Ongoing investigation
- Starts upon crime discovery
- Escalation to pursuit
- Evidence system (footprints, etc.)

## Offenses

- `Offense`: Individual offense
- Severity level affects reaction
- Can be combined into charges
- `OffenceNoticeUI`: Display notification upon offense

## Road Checkpoints

- `RoadCheckpoint`: Random checkpoints
- `CheckpointInstance`: Active checkpoint
- Vehicle search
- `CheckpointManager`: Checkpoint management

## Arrest

- `ArrestScreen`: Arrest screen
- `ArrestNoticeScreen`: Charges list
- `BodySearchScreen`: Body search
- `PickpocketScreen`: Pickpocketing

## Integration with Law System

- Works together with LawManager/Crime classes
- `FootprintMatchData`: Evidence
- Pursuit combines Vision + Noise + Law
