# NPC Behaviour (Schedule I)

## Navigation
- **Unity NavMesh** (no A*)
- `NPCMovement`: `NavMeshAgent` with `WalkSpeed=1.8`, `RunSpeed=7.0`
- Three agent types: Humanoid, BigHumanoid, IgnoreCosts
- NPCPathCache: cached paths (`USE_PATH_CACHE = true`)
- `NPCSpeedController`: priority-stack based (Combat Prio 5, Ragdoll Prio 100)

## Schedules (NPCScheduleManager)
- `NPCAction[]` with `StartTime`, `EndTime`, `Priority`
- Tick-based: evaluated every minute
- Signal actions (short, non-blocking): `NPCSignal_WalkToLocation`, `NPCSignal_DriveToCarPark`, `NPCSignal_UseVendingMachine`, `NPCSignal_UseATM`
- Discrete actions (randomized): smoking, drinking, graffiti, umbrella

## Emotions (AvatarEmotionManager)
- Priority-based override stack, highest priority wins
- Default: "Neutral" (Priority -1)
- Lerping over 0.2s, evaluated every 0.25s, active only within <30m
- Available: Neutral, Cheery, Happy, Concerned, Annoyed, Angry, Surprised, Scared, Sleeping, Meth, Cocaine, Zombie, Shroom

## Relationships (NPCRelationData)
- Scale: 0.0 (Enemy) to 5.0 (Best Friend), default 2.0
- Changes: Attack (-0.25/-1.0), Aiming weapon (-0.5), Pickpocket caught (-0.25)
- Unlock: `Recommendation` (from NPC) or `DirectApproach`
- Connection network: Unlocking an NPC also unlocks connections

## NPC Types
| Type | Special Feature |
|------|-----------------|
| `PoliceOfficer` | Deactivation/Activation system, VisionCone 1.8x |
| `CartelGoon` | GoonPool (randomized skins), group combat, despawn |
| `Employee` | Botanist/Chemist/Cleaner/Packager, daily wage 100 |
| `Customer` | Purchasing behavior, region-bound |
| `Supplier` | Dead drop deliveries |
| `Dealer` | Drug distribution, managed customers |

## Combat
- Weapons: VirtualPunch (always available), DefaultWeapon (Baton/Taser/Gun)
- `CombatBehaviour`: `StartCombat()` → `ReadyToAttack()` → `Attack()`
- `PursuitBehaviour : CombatBehaviour`: 4 escalation levels
- `FleeBehaviour`: 20-40m distance, "Scared" emotion
- Disengages after `GiveUpAfterSuccessfulHits` landed hits

## Voiceover
- `VOEmitter` (on head bone)
- `EVOLineType`: Hurt, Scared, Concerned, Angry, Die, Think, PoliceChatter
- NPC-specific responses: `NPCResponses_Civilian`, `NPCResponses_CartelGoon`, `NPCResponses_Police`, `NPCResponses_Employee`
