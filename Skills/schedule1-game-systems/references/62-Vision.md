# Vision System (Schedule I)
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 97 of 97 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Namespace: `Il2CppScheduleOne.Vision` — 12 types in the decompile dump.

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `VisionCone` | NetworkBehaviour | NPC sight: frustum-based LOS + notice-progress state machine |
| `EntityVisibility` | NetworkBehaviour | Per-entity visibility scoring + named visual states |
| `PlayerVisibility` | EntityVisibility | Player-specific points + curfew flag |
| `VisionEvent` | plain class | One sighting: notice progress towards `FullNoticeTime` |
| `VisionEventReceipt` | plain class | Networkable receipt: `Target` (NetworkObject) + `State` (EVisualState) |
| `VisionObscurer` | MonoBehaviour | Occluder with `ObscuranceAmount` |
| `LightVisibilityAffector` | MonoBehaviour | Lights modify a visibility attribute (`PointLightEffect`/`SpotLightEffect`, `EffectMultiplier`) |
| `VisibilityAttribute` | plain class | `{ name, pointsChange, multiplier }` + `Delete()` |
| `UniqueVisibilityAttribute` | VisibilityAttribute | Adds `uniquenessCode` (dedupe on EntityVisibility) |
| `ISightable` | interface (interop) | `NetworkObject`, `VisibilityComponent` (EntityVisibility), `HighestProgressionEvent`, `IsCurrentlySightable()` |

## VisionCone Detection Model
- Geometry: `HorizontalFOV`, `VerticalFOV`, `Range`, `VisionOrigin`, `VisionFalloff`, `VisibilityBlockingLayers`, `RangeMultiplier`
- Tuning: `VISION_UPDATE_INTERVAL`, `MinVisionDelta`, `NoticeTimeMultiplier`, `UniversalAttentivenessScale`, `UniversalMemoryScale`, per-cone `Attentiveness`, `Memory`
- Flow: `VisionUpdate(dt)` → `UpdateVision(dt)` + `UpdateEvents(dt)`; per sightable `VisionEvent` builds notice progress
- Queries: `IsPointWithinSight(Vector3, bool, LandVehicle)`, `IsPlayerVisible(Player)`, `IsTargetVisible(ISightable)`, `WasSightableVisibleThisFrame(ISightable)`, `GetPlayerVisibility(Player)` → float, `RequiredNoticeTime`
- Actions: `SetNoticePlayerCrimes(Player, bool)`, `AddSightableOfInterest`/`RemoveSightableOfInterest`, `ClearEvents()`, `OnDie()` cleanup
- Reaction hooks wired to popups/sounds: `QuestionMarkPopup`, `ExclamationPointPopup`, `ExclamationSound`, `UseTremoloSound`

## Events
| Event | Type | Raised by |
|-------|------|-----------|
| `onVisionEventStarted` / `onVisionEventHalf` / `onVisionEventFull` / `onVisionEventExpired` | delegate field `VisionCone.EventStateChange : MulticastDelegate` (signature `(VisionEventReceipt)`) — **not UnityEvents**, no add/remove accessors | `EventReachedZero` / `EventHalfNoticed` / `EventFullyNoticed` / drop past `NOTICE_DROP_THRESHOLD` |
| `EEventLevel { Start, Half, Full, Zero }` | enum | `SendEventReceipt(receipt, level)` (server RPC) → `ReceiveEventReceipt` observers RPC |

Networked: sight progress is shared via receipts so all clients see the same NPC noticing state (01-FishNet-Networking).

## Visual States
- `EVisualState` = `{ Visible, Suspicious, DisobeyingCurfew, Vandalizing, PettyCrime, DrugDealing, Wanted, Pickpocketing, DischargingWeapon, Brandishing }` (10 states)
- `EntityVisualState { state, label, stateDestroyed }`; `EntityVisibility.VisualStates` list
- Apply/remove is server-authoritative: `ApplyState(name, EVisualState, float)` / `RemoveState(name, float)` are FishNet server RPCs; `GetState(name)` / `ClearStates()` local
- Visibility scoring: `CalculateVisibility(float)`, `CalculateExposureToPoint(Vector3, float, NPC)`, `CurrentVisibility`, `Suspiciousness`; `MAX_VISIBLITY` (typo is in game code); multiple visibility points (`VisibilityPoints`, `CentralVisibilityPoint`, `VisibilityCheckMask`)

## Player Visibility
- `PlayerVisibility : EntityVisibility` on the player; `Player.Visibility` field + `VisibilityComponent` getter; `OnThirdPersonMeshesVisibilityChanged` event
- Verified influence: **light** via `LightVisibilityAffector`/`UpdateEnvironmentalVisibilityAttribute` (environmental attribute), **curfew** via `AddFlag_DisobeyingCurfew()`/`RemoveFlag_DisobeyingCurfew()` (17-TimeManager)
- Movement speed / crouching / camouflage: gameplay-level claims — **unverified** in these classes (see 31-Clothing)

## Consumers (verified users)
- `NPCs\NPCAwareness` — owns `VisionCone` for NPC perception (14-NPC-Behaviour)
- `NPCs\NPC` + `PlayerScripts\Player` — implement/hold `EntityVisibility` (ISightable)
- `Audio\SpottedTremolo` — "spotted" tremolo SFX driven by visibility state (28-Audio)
- Police pursuit escalation combines vision + noise + law (12-Heat-Pursuit-Law, 46-Noise, 51-Police)

## Save Participation
- **None** — no ISaveable in the namespace; visibility state is transient.

## Hook Points
1. **Postfix `VisionCone.IsPlayerVisible(Player)` / `GetPlayerVisibility(Player)`** — stealth mods (camouflage, shadow blending, disguises).
2. **Postfix `EntityVisibility.CalculateVisibility(float)`** — global or per-entity visibility multiplier.
3. **Prefix `VisionCone.SendEventReceipt(VisionEventReceipt, EEventLevel)`** — intercept/suppress NPC "noticed me" escalations before they reach law enforcement.
- **No S1API wrapper** — S1APIhas no `S1API.Vision` namespace (only `S1API.Law`); use direct interop.

## Not Implemented / Unverified
- Old claim "EVisualState = Invisible/Hidden/Visible/FullyVisible" removed — actual enum has the 10 values above.
- `VisionEvent` is not a component/UnityEvent — plain progress object.

## Cross-links
51-Police (stub → 12-Heat-Pursuit-Law) · 46-Noise · 14-NPC-Behaviour · 28-Audio · 17-TimeManager (curfew) · 01-FishNet-Networking
