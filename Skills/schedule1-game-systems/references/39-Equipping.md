# Equipping (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `Equippable` | Base for all equippable items |
| `EquippedItemHandler` | Controller for current equipment |
| `EquippableDataRegistry` | Registry of all equippable data |
| `NetworkedEquipper` | Network sync |

## Equippable Types

| Type | Description |
|------|-------------|
| `Equippable_MeleeWeapon` | Melee weapon |
| `Equippable_RangedWeapon` | Ranged weapon |
| `Equippable_Revolver` | Revolver |
| `Equippable_PumpShotgun` | Pump shotgun |
| `Equippable_Additive` | Plant additive / fertilizer |
| `Equippable_Seed` | Seed |
| `Equippable_Pourable` | Pourable items |
| `Equippable_SprayBottle` | Spray bottle |
| `Equippable_TrashGrabber` | Trash grabber |
| `Equippable_Trimmers` | Trimmers / Shears |
| `Equippable_BuildableItem` | Placeable items |
| `Equippable_SurfaceItem` | Surface items |
| `Equippable_Cuke` | Cucumber (Meme) |
| `Equippable_AvatarViewmodel` | Viewmodel items |
| `Equippable_Viewmodel` | Viewmodel base |

## Equipping Framework

- `EquippableItemDefinition`: Definition in item framework
- `EquippableHandlerService`: Service for equip handling
- `EquipConfiguration`: Equip configuration
- `INetworkedEquippableUser`: Interface for networked user

## Avatar Equipment

- `AvatarEquippable`: Base for avatar equipment
- `AvatarGun`: Avatar firearm
- `AvatarMeleeWeapon`: Avatar melee weapon
- `AvatarRangedWeapon`: Avatar ranged weapon
- `FlashlightAvatarEquippable`: Flashlight

## Viewmodel

- Dedicated camera model for weapons/items
- `ViewmodelAvatar`: Avatar viewmodel
- `ViewmodelSway`: Movement sway simulation
