# Combat & Weapons (Schedule I)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 137 of 139 identifier-shaped tokens resolve (2 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Core Classes

| Class | Verified base type | Purpose |
|-------|--------------------|---------|
| `CombatManager` | `NetworkSingleton<CombatManager>` (`ScheduleOne.Combat`) | Explosion routing (server-authoritative via FishNet RPCs) |
| `CombatBehaviour` | `Il2CppScheduleOne.NPCs.Behaviour.Behaviour` | NPC combat AI: targeting, searching, shooting, melee |
| `PunchController` | `MonoBehaviour` | Player unarmed combat: `StartLoad()`, `Release()`, `Punch(float power)`, `ExecuteHit(float power)` |
| `Explosion` | `MonoBehaviour` | `Initialize(Vector3 origin, ExplosionData data)` applies radial damage/push |
| `Impact` | plain object (NOT MonoBehaviour) | Payload struct-like class: `HitPoint`, `ImpactForceDirection`, `ImpactForce`, `ImpactDamage`, `ImpactType`, `ImpactSource`, `ImpactID`, `ExplosionType`; `IsPlayerImpact(out Player)` |
| `ExplosionData` | data class | `DamageRadius`, `MaxDamage`, `PushForceRadius`, `MaxPushForce`, `CheckLoS`, `ExplosionType`, statics `DefaultSmall`, `LightningStrike` |

## Interfaces (interop wrappers, real C# interfaces in game)
- `IDamageable`: `gameObject`, `SendImpact(Impact)`, `ReceiveImpact(Impact)`
- `ICombatTargetable`: `NetworkObject`, `CenterPoint`, `CenterPointTransform`, `LookAtPoint`, `IsCurrentlyTargetable`, `RangedHitChanceMultiplier`, `Velocity`, `RecordLastKnownPosition(bool)`, `GetSearchTime()`, `IsPlayer`, `AsPlayer`
- `IPhysicsDamageable` / `PhysicsDamageable` (MonoBehaviour) / `NetworkedPhysicsDamageable` (NetworkBehaviour, RPCs `SendImpact`/`ReceiveImpact`) — physics-prop damage chain

## Enums (values verified)
- `EImpactType`: `Punch, BluntMetal, SharpMetal, Bullet, PhysicsProp, Explosion`
- `EExplosionType`: `Default, Lightning`
- `ERangedWeaponAction`: `None, Shoot, Reposition, RepositionAndShoot` (drives `CombatBehaviour` ranged decision-making / prompts)

## Weapons
| Class | Verified inheritance | Notes |
|-------|---------------------|-------|
| `AvatarWeapon` | `AvatarEquippable` (`ScheduleOne.AvatarFramework.Equipping`) | Base for all avatar-held weapons |
| `AvatarMeleeWeapon` | `AvatarWeapon` | Nested `MeleeAttack` data class |
| `AvatarRangedWeapon` | `AvatarWeapon` | `CanShoot()`, `IsTargetInLoS(ICombatTargetable)`, reload routine |
| `AvatarGun` | `AvatarRangedWeapon` | Muzzle flash routine |
| `Taser` | `AvatarRangedWeapon` | Police taser (NOT `AvatarEquippable` directly as previously listed) |
| `Equippable_MeleeWeapon` | `Equippable_AvatarViewmodel` | `UpdateInput()`, `StartLoad()`, `Release()`, `Hit(float)`, `ExecuteHit(float)` |
| `Equippable_RangedWeapon` | `Equippable_AvatarViewmodel` | Full gun framework: `MagazineSize`, `Magazine`, `CanFire(bool checkAmmo = true)`, `CanCock()`, `Cock()`, `GetSpreadAngle()`, `CheckAimingAtNPC()`, `MinSpread`/`MaxSpread`, `ReloadType` (`EReloadType`) |
| `Equippable_Revolver` | `Equippable_RangedWeapon` | `SetDisplayedBullets(int)` cylinder display |
| `Equippable_PumpShotgun` | `Equippable_RangedWeapon` | No unique public members in dump — pump behavior comes from base fields (`MustBeCocked`, `CockedByDefault`, reload/cock config) |

## Events / UnityEvents
On `Equippable_RangedWeapon` (verified `UnityEvent` properties; invocation sites are inside the fire/reload/cock method flow — exact `.Invoke()` call sites not visible in the interop dump):
| UnityEvent | Associated flow |
|-----------|-----------|
| `onFire` | firing (`Fire` path, single + spread) |
| `onReloadStart` / `onReloadIndividual` / `onReloadEnd` | reload coroutine (`EReloadType` magazine vs. individual) |
| `onCockStart` | cock routine |

No `UnityEvent`/`event Action`/`PreallocatedAction` members exist anywhere in `ScheduleOne.Combat` itself — explosion and impact flow is method-based, not event-based.

## CombatBehaviour AI (verified fields — moddable tuning knobs)
- Constants: `RECENT_VISIBILITY_THRESHOLD`, `REPOSITION_TIME`, `SEARCH_RADIUS_MIN/MAX`, `SEARCH_SPEED`, `CONSECUTIVE_MISS_ACCURACY_BOOST`, `REACHED_DESTINATION_DISTANCE`, `DelayBeforeFirstAttack`
- Inspector/config: `GiveUpRange`, `GiveUpAfterSuccessfulHits`, `DefaultMovementSpeed`, `VirtualPunchWeapon`, `DefaultSearchTime`, `CombatOnStart`
- Key methods: `SetTargetAndEnable_Server(NetworkObject)`, `SetDefaultWeapon(AvatarWeapon)`, `SetWeapon(string weaponPath)` (protected virtual), `ClearWeapon()`, `IsCurrentWeaponMelee()`, `Shoot()`, `SucessfulHit()` (sic), `StartSearching()`/`StopSearching()`, `CheckTargetVisibility()`, `MarkPlayerVisible()`

## Save Participation
- None of the Combat/Equipping classes implement `ISaveable` (grep verified). Ammo state is item-instance data; equipped items persist via item instances, not via this namespace.

## Hook Points (Harmony)
1. `CombatManager.CreateExplosion(Vector3, ExplosionData)` (public, CallerCount>0) and its private id-overload — prefix to modify/custom-block explosions; networked flow is `RpcWriter___Server_CreateExplosion_*` → server → observers, so patch the public client entry, not the RPC reader.
2. `CombatBehaviour.Shoot()` — bool-returning, non-trivial instance method; prefix to force hit/miss (roll accuracy mods) without touching raycast internals.
3. `Equippable_RangedWeapon.CanFire(bool)` or `PunchController.Punch(float)` — gate/disable weapons or re-route damage.
- S1API: **no** `S1API.Combat`/weapon namespace exists. `S1API.Entities.NPC` exposes `.CombatBehaviour` (wrapper class `S1API.Entities.Behaviour.CombatBehaviour`) with `GiveUpRange`, `DefaultSearchTime`, `DefaultWeapon`, `SetTargetAndEnable_Server`, `SetWeapon(string path)` — use that instead of raw Harmony where possible.

## Not Implemented / Dead Code
- No `WeaponRegistry` or ammo-reserve manager class found in this dump; magazine/ammo logic lives entirely on `Equippable_RangedWeapon` fields.
- `ReticleController` (`ShowReticle(float duration = -1)`, `HideReticle`, `SetReticle(float spreadAngle)`), `ReticleUI`, `CrosshairText` live in `ScheduleOne.UI` — UI-only, no damage involvement.
- Old claim "Taser (AvatarEquippable)" corrected: `Taser : AvatarRangedWeapon`.

---

---

---

---

---
 Identifier-shaped tokens documented as *absent*: `WeaponRegistry`, `RpcWriter___Server_CreateExplosion_`.
 Identifier-shaped tokens documented as *absent*: `RpcWriter___Server_CreateExplosion_`, `WeaponRegistry`.
 Identifier-shaped tokens documented as *absent*: `WeaponRegistry`, `RpcWriter___Server_CreateExplosion_`.