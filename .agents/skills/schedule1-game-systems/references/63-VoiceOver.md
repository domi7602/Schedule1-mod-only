# VoiceOver (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `VODatabase` | VoiceOver database |
| `VODatabaseEntry` | Individual VO entry |
| `VOEmitter` | VO playback component |

## VoiceOver System

- Spoken dialogue lines for NPCs
- `EVOLineType`: Line types (Response, Idle, Alert, etc.)
- NPCs play VO during specific events
- `PoliceChatterVO`: Police radio chatter

## Database

- `VODatabase`: Contains all VO entries
- `VODatabaseEntry`: Individual line with audio clip + text
- Organized by NPC/category
- Random selection from multiple lines

## Emitter

- `VOEmitter`: Plays VO on NPC
- Positions audio in 3D world
- Distance-based volume
- Priority system (higher priority interrupts lower)

## Integration

- VO is triggered during dialogue events
- Police radio chatter during pursuit
- NPC responses during interaction
- Cartel goon threats
