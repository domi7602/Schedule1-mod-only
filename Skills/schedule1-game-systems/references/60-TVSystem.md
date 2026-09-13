# TV System (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `TVApp` | Base TV app |
| `TVHomeScreen` | TV home screen |
| `TVInteractable` | TV interaction component |
| `TVInterface` | TV UI |

## TV Devices

- `TVInteractable`: Interactable TV device
- `TVHomeScreen`: Home screen with app selection
- `TVInterface`: TV screen UI
- Placeable in the property

## TV Games

| Game | Class | Description |
|------|-------|-------------|
| Pong | `Pong` | Classic Pong |
| Snake | `Snake` | Snake game |
| Runner | `RunnerGame` | Endless runner |

## Game Classes

- `Pong`: Main Pong class, `PongBall`, `PongPaddle`
- `Snake`: Main Snake class, `SnakeTile`
- `RunnerGame`: Main Runner class, `RunnerGameCharacter`
- `TVPauseScreen`: Pause screen

## Interaction

- Turn on TV → HomeScreen
- Select app → Start game
- Escape/Remote to go back
- Keyboard control (P1: WASD, P2: Arrow keys)
