# Variables & Conditions (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `VariableDatabase` | Global variables database |
| `Variable` | Base variable |
| `Condition` | Condition check |
| `VariableSetter` | Variable setter |

## Variable Types

| Type | Class | Value |
|------|-------|-------|
| Boolean | `BoolVariable` | true/false |
| Number | `NumberVariable` | Numeric value |
| Base | `BaseVariable` | Base class |

## Variables System

- `Variable`: Stores named value
- `EVariableMode`: ReadOnly, ReadWrite, ServerOnly
- `EVariableReplicationMode`: Network replication
- `VariableCreator`: Factory for variables

## Conditions

- `Condition`: Checks variable state
- `Conditions`: Collection of conditions (AND/OR)
- `QuestCondition`: Quest-specific conditions
- Used for quest progress

## Applications

- Quest flags (Quest XYZ completed)
- Game states (DarkMarket visited)
- NPC relationship values
- Achievements and story progress

## Setters

- `VariableSetter`: Sets variable on event
- Automated variable manipulation
- Quest events trigger setters
