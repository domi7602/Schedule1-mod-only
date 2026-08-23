# Employees (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `EmployeeManager` | Global employee manager (NetworkSingleton) |
| `Employee` | Base employee class |
| `EEmployeeType` | Employee types |

## Employee Types

| Type | Task |
|------|------|
| `Botanist` | Water, harvest, and fertilize plants |
| `Chemist` | Operate chemistry stations |
| `Packager` | Package products |
| `Cleaner` | Dispose of trash, cleaning |

## Hiring

- Employees are recruited via the Phone app
- Costs a weekly salary
- Works according to a schedule (NPC schedule)
- `EmployeeHome`: Employee residence

## Management

- `BotanistConfiguration`: Work configuration
- `ChemistConfiguration`: Work configuration
- `PackagerConfiguration`: Work configuration
- `CleanerConfiguration`: Work configuration
- Configuration via `ManagementClipboard`

## NPC Responses

- `NPCResponses_Employee`: Employee dialogues
- Different responses depending on the situation

## Behavior

- Dedicated behavior classes for each job
- Automatic navigation to stations
- Work quality depends on traits
