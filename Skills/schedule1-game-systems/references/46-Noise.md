# Noise System (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `NoiseUtility` | Noise utilities |
| `NoiseEvent` | Individual noise event |
| `Listener` | Noise listener |

## Noise System

- Players and NPCs generate noise events
- `ENoiseType`: Types (Footstep, Gunshot, Breaking, etc.)
- NPCs with `Listener` component react to noise
- Distance-based detection

## Applications

- Police hear gunshots → Pursuit
- NPCs hear breaking glass → React
- Cartel goons hear fights
- Sneaking reduces noise

## Integration

- `NoiseEvent` has position, radius, type
- `Listener` on NPCs checks events within range
- Combined with vision system for complete detection

## Stealth

- Sneaky effect eliminates footstep noise
- Movement speed affects noise level
- Underground/Sewer reduces noise range
