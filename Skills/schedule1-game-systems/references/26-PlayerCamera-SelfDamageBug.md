# Self-Damage Bug (Baseball Bat + V Key) (Schedule I)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 15 of 15 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

> **Static-check limitation (2026-10-08).** `Il2CppScheduleOne.Equipping.Equippable_MeleeWeapon`
> exists on the installed runtime and `ExecuteHit(float power)` is still declared, but the
> decompiled body is only an `il2cpp_runtime_invoke` stub — the method logic is not present in
> the shipped metadata. Therefore the missing self-damage guard, the local variable name and
> the "Line 236" reference **cannot be confirmed or refuted** from the decompiles. Everything
> below is carried-over analysis plus an in-game report, not decompile-verified code. Re-derive
> it from a runtime dump (S1MCP / dnSpy-on-dump) before relying on it.


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

---
