# Interaction (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `InteractionManager` | Global interaction manager |
| `InteractableObject` | Base for interactable objects |
| `WorldSpaceLabel` | Interaction label |

## Interaction System

- `InteractableObject`: MonoBehaviour component on interactable objects
- `InteractableToggleable`: Toggleable (On/Off) interaction
- `NetworkedInteractableToggleable`: Network-synchronized version
- `IUsableInteractableObject`: Usable interface

## Interaction Types

- `EInteractionSearchType`: Search method (Raycast, OverlapSphere)
- Context-sensitive interactions based on equipped item
- Right-click for action, left-click for primary action

## Interaction UI

- `WorldSpaceLabel`: 3D label above objects
- `InteractionCanvas`: Canvas with action text
- `CrosshairText`: Crosshair text
- `InputPromptsCanvas`: Input prompts

## Network

- Interactions are validated via ServerRpc
- `NetworkedInteractableToggleable` for synchronized states
- Ownership check before action
