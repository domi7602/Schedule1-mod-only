# Polling (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `PollManager` | Global poll manager |
| `PollData` | Poll data |
| `PollAnswer` | Response option |
| `PollResponse` | Player response |

## Poll System

- Community polls / surveys on the phone
- NPCs can express opinions
- Poll results affect game world?
- Time-limited polls

## UI

- `PollPanel`: Poll UI
- Response options as buttons
- Current results displayable
- Integration with NotificationsManager

## Data

- `PollResponseWrapper`: Response wrapper for network
- Poll data persistable via JSON
- NetworkSingleton for sync
