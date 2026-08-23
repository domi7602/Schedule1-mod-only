# Dragging (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `DragManager` | Global drag manager |
| `Draggable` | Component for draggable objects |

## Drag System

- `Draggable` is attached to GameObjects
- Player can grab and drag objects
- Physics-based dragging
- Network synchronization

## Applications

- Drag trash bags to containment
- Drag bodies/unconscious NPCs
- Move crates and containers
- `Equippable_TrashGrabber`: Trash grabber for picking up

## Player Tasks

- `Draggable` (PlayerTasks): Task-based dragging
- `DraggableConstraint`: Movement constraints

## Interaction

- Drag key (Default: G)
- Visual feedback during drag
- Pick up / drop via interaction system
