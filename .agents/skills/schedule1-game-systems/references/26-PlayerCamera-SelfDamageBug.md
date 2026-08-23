# Self-Damage Bug (Baseball Bat + V Key) (Schedule I)

## Root Cause
**Missing self-damage check** in `Equippable_MeleeWeapon.ExecuteHit()` (Line 236).

## Workflow / Sequence
1. **V Key** (`ViewAvatar`): Camera is positioned behind the player, `LookAt(LowestSpine)`
2. `Camera.forward` points directly at the player
3. **Swing**: `LookRaycast()` fires from **camera position** in **camera direction**
4. Hits player's own collider (CapsuleCollider / Avatar)
5. `hit.collider.GetComponentInParent<IDamageable>()` resolves to **the player itself**
6. **NO** `componentInParent != Player.Local` check → hit impact is registered
7. Fully charged swing: 60 damage → death (at 100 HP)

## Comparison Across Weapon Systems
| Weapon System | Self-Damage Check |
|---------------|-------------------|
| PunchController | ✅ `componentInParent != player` |
| RangedWeapon | ✅ `componentInParent == Player.Local => continue` |
| Explosion | ✅ (ImpactSource = null, never own grenade) |
| **Equippable_MeleeWeapon** | **❌ Completely missing** |

## Why Only Baseball Bat?
- **Prefab configuration difference, not code difference**
- All melee weapons share identical code logic
- Difference: **Prefab values** (Range + HitRadius)

| Weapon | Range | HitRadius | Effect |
|--------|-------|-----------|--------|
| Baseball Bat | ~2.5 | ~0.4 | SphereCast reaches player collider |
| Frying Pan | ~1.0 | ~0.0 | Raycast too short/narrow |

## Fix (1 Line)
```csharp
// In Equippable_MeleeWeapon.ExecuteHit(), Line 236:
// OLD:
if (componentInParent != null)
// NEW:
if (componentInParent != null && componentInParent != Player.Local)
```

## Mod Idea
Set `Range` and `HitRadius` via mod on other melee weapons → bug occurs with frying pan etc. as well.
