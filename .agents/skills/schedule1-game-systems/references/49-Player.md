# Player (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `Player` | Main player class |
| `PlayerManager` | Player manager (NetworkSingleton) |
| `PlayerMovement` | Movement |
| `PlayerCamera` | Camera |
| `PlayerInventory` | Inventory |
| `PlayerHealth` | Health |

## Player Controller

- `Player`: Network-synced player character
- `PlayerManager`: Manages all players
- `CursorManager`: Cursor control
- `LocalPlayerFootstepGenerator`: Footstep noise

## Movement

- `PlayerMovement`: Walking, sprinting, sneaking
- Movement speed affects noise level
- Sprint stamina system
- `PlayerEnergy`: Energy system (sleep required)

## Camera

- `PlayerCamera`: First-person camera
- Camera bob, FOV changes
- Viewmodel overlay for weapons/items
- `ViewmodelSway`: Movement sway simulation

## Inventory

- `PlayerInventory`: 8 hotbar slots + backpack
- `HotbarSlot`: Quick-key slot (1-8)
- Item management (pick up, drop, use)
- `PlayerClothing`: Clothing management

## Health

- `PlayerHealth`: Health points
- Damage intake (weapons, falls, poison)
- Healing (hospital, items)
- `PlayerHealthVisuals`: Visual feedback

## Energy

- `PlayerEnergy`: Energy system
- Sleep required (Bed interaction)
- Low energy → Exhaustion → Pass out
- `PassOutScreen`: Pass out screen

## Crime Data

- `PlayerCrimeData`: Crime tracking
- Current charges, penalties
- Police interactions
