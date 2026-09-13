# Vision System (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `VisionCone` | Vision cone for NPCs |
| `EntityVisibility` | Entity visibility |
| `PlayerVisibility` | Player visibility |
| `VisionEvent` | Vision event |

## Vision Cone

- `VisionCone`: NPC vision area (cone shape)
- `VisionEventReceipt`: Vision event receipt
- `VisionObscurer`: Obstacle blocking line of sight
- `LightVisibilityAffector`: Light affects visibility

## Visibility Types

- `EVisualState`: Invisible, Hidden, Visible, FullyVisible
- `EntityVisualState`: Visual state of an entity
- `ISightable`: Interface for sightable entities
- `UniqueVisibilityAttribute`: Unique visibility attribute
- `VisibilityAttribute`: General visibility attribute

## Player Visibility

- `PlayerVisibility`: How visible the player is
- Influenced by:
  - Light (night = less visible)
  - Movement speed (sprinting = easier to spot)
  - Hiding/crouching
  - Camouflage clothing

## Integration

- Police detection combines Vision + Noise + Law
- NPCs react based on visibility
- Cartel goons scout using vision
- Stealth mechanics (sneaking, hiding)
