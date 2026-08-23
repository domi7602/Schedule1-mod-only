# Combat & Weapons (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `CombatManager` | Global combat manager (NetworkSingleton) |
| `CombatBehaviour` | NPC combat AI behavior |
| `PunchController` | Unarmed melee attacks |
| `IDamageable` | Damage interface |
| `Explosion` | Explosion damage handling |

## Weapons

- **Melee**: `Equippable_MeleeWeapon` (Baseball bat, knife)
- **Ranged**: `Equippable_RangedWeapon`, `Equippable_Revolver`, `Equippable_PumpShotgun`
- **Taser**: `Taser` (AvatarEquippable)
- **Handgun / Firearms**: `AvatarGun`, `AvatarRangedWeapon`

## Damage System

- `PhysicsDamageable`: Damage caused by physics objects
- `ICombatTargetable`: Combat targeting interface
- `EImpactType`: Damage types (Blunt, Pierce, Explosion)
- `ExplosionData`: Explosion parameters

## Combat UI

- `ReticleController`: Crosshair animation controller
- `ReticleUI`: Crosshair display
- `CrosshairText`: Interaction prompt text inside crosshair

## Weapon Classes

- `AvatarWeapon`: Base class for all avatar weapons
- `AvatarMeleeWeapon`: Melee weapons
- `AvatarRangedWeapon`: Firearms
- `Equippable_RangedWeapon`: Player equippable component for firearms

## Ammunition & Reloading

- Weapons consume item system ammunition
- `Equippable_Revolver`: 6-shot capacity, manual reload
- `Equippable_PumpShotgun`: Pump-action mechanics
