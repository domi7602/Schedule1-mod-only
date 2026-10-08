# Interaction (Schedule I)
> verified: classes + fields + event types + RPCs + save-participation re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

Namespace: `Il2CppScheduleOne.Interaction`.

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `InteractionManager` | `Singleton<InteractionManager>` | Local player ray/sphere-cast search, hover + interact dispatch, hold-to-destroy |
| `InteractableObject` | `MonoBehaviour` | Base interactable: message, state, range, UnityEvents |
| `InteractableToggleable` | `MonoBehaviour` | On/Off interactable wired to an `InteractableObject` |
| `NetworkedInteractableToggleable` | `NetworkBehaviour` | Toggleable with FishNet RPC replication |
| `InteractablePurchaseableItem` | `MonoBehaviour` | Buy-through-interaction component |
| `IUsableInteractableObject` | `InteractableObject` | Adapter for `IIUsable` implementers |
| `WorldSpaceLabel` | `Il2CppSystem.Object` | **Not a MonoBehaviour** — world-space text label helper (`ctor(string, Vector3)`, `RefreshDisplay()`, `Destroy()`) |

## InteractionManager (verified)
- Search: `DoCasts()` (private), `_ray`, `_sphereCastHits`, `_rayCastHits`, `interaction_SearchMask` (`LayerMask`), `RayRadius`, `MaxInteractionRange`, `rightClickRange`
- Selection state: `HoveredInteractableObject`, `HoveredValidInteractableObject`, `InteractedObject` (props, set protected), `CanDestroy` prop + `SetCanDestroy(bool)`
- Hold-to-destroy: `itemBeingDestroyed`, `destroyTime`, `timeToDestroy`
- UI/colors: `InteractKeyStr` prop + `LoadInteractKey()`, `InteractInput`, `messageColor_Default/Invalid`, `iconColor_Default/Invalid/Default_Key`, `icon_Key/LeftMouse/Cross`, `interactCooldown`, `timeSinceLastInteractStart`
- Methods: `Update()`, `LateUpdate()`, `CheckHover()`, `CheckInteraction()`, `CheckRightClick()`, `GetHoveredBuildableItem()`, `IsAnythingBlockingInteraction()`
- **Correction:** old note "EInteractionSearchType (Raycast, OverlapSphere)" — no such enum exists in the decompiles; search is a LayerMask-driven sphere cast. `InputPromptsCanvas` also not found (`unverified`/removed). `InteractionCanvas`/`CrosshairText` live in `Il2CppScheduleOne.UI`.

## InteractableObject (verified)
- Nested enums: `EInteractionType { Key_Press, LeftMouse_Click }`, `EInteractableState { Default, Invalid, Disabled, Label }`
- Fields: `message`, `interactionType`, `interactionState`, `MaxInteractionRange`, `RequiresUniqueClick`, `Priority`, `displayLocationCollider`, `displayLocationPoint`, `LimitInteractionAngle`, `AngleLimit`, `_isMessageActive`, `_currentBindingData`, `_descriptorData`
- Methods: `SetInteractionType(EInteractionType)`, `SetInteractableState(EInteractableState)`, `SetMessage(string)`, virtual `Hovered()` / `Exited()` / `StartInteract()` / `EndInteract()` / `ShowMessage()`, `CheckAngleLimit(Vector3) → bool`, input hooks `SetInputData`/`OnInputChange(InputDeviceType)`

## Toggleables (verified)
- `InteractableToggleable`: `IsActivated` prop, `ActivateMessage`/`DeactivateMessage`, `CoolDown`, `IntObj` (InteractableObject ref), `lastActivated`; `Toggle()`, `SetState(bool)`, `Hovered()`, `Interacted()`, `PoliceDetected()` — auto-deactivate when police are near (cross-link 12)
- `NetworkedInteractableToggleable`: same fields + RPCs — client `RpcWriter___Server_SendToggle_2166136261`, replication `RpcWriter___Observers_SetState_214505783` / target variant
- `InteractablePurchaseableItem`: `_paymentType`, `_useItemDefaultPrice`, `_setPrice`, `_item`, `_itemQuantity`, `_showNameInMessage`, `_playCashSoundOnPurchase`, `_purchaseCooldown`/`_lastPurchaseTime`, `_interactableObject`; `Buy()`, `CanBuy(out string reason)` (virtual), `GetItemToGive(int) → ItemInstance`, `HasSpaceInInventory()`, `HasEnoughFunds()`, `IsOnPurchaseCooldown()`, `OnHovered()`/`OnInteracted()`

## Events / UnityEvents
| Event | Type | Raised by |
|-------|------|-----------|
| `InteractableObject.onHovered` / `onInteractStart` / `onInteractEnd` | UnityEvent | `Hovered()` / `StartInteract()` / `EndInteract()` |
| `InteractableToggleable.onToggle` / `onActivate` / `onDeactivate` | UnityEvent | `Toggle()` / `SetState(bool)` |
| `NetworkedInteractableToggleable.onToggle` / `onActivate` / `onDeactivate` | UnityEvent | same as above (networked) |
| `EmployeeHome`-style label events — none here | — | — |

No C#-delegate event fields in the namespace (grep).

## Save Participation
- **No `ISaveable` / `SaveData` in the Interaction namespace** (grep). Toggle state persistence is owned by the underlying objects (e.g. `ToggleableLight` state, doors) — not by these components.

## Hook Points
1. **Prefix `InteractableObject.StartInteract()`** — global per-object interact gate (lock doors, quest items, cooldowns); virtual, non-inline, called for every interaction.
2. **Postfix `InteractionManager.CheckInteraction()`** — observe every player interaction per frame (analytics, achievement-style triggers).
3. **Postfix `InteractionManager.IsAnythingBlockingInteraction()`** — inject custom blocking conditions (cutscenes, UI modals).
- **S1API (3.2.1-beta.8) wrapper (verified in source):** `S1API.Interaction.InteractionPrompt` + `InteractionPromptBuilder` — managed handle that adds/configures the native `InteractableObject` on a mod GameObject (`SetMessage`, maps `EInteractionType`/`EInteractableState`, exposes `Hovered`/`InteractionStarted`/`InteractionEnded` events, `Message`/`Range`/`Priority`/`AngleLimit` properties).
- `S1API.Entities.NPC` also reuses native `InteractableObject` for talk/pickpocket interactables (verified in `NPC.cs`).

## Not Implemented / Unverified
- No manager-level "interaction happened" event — only per-object UnityEvents.
- `IUsableInteractableObject` wraps `_iUsable` (`IIUsable`); the interface wrapper source is not in this decompile generation (`unverified` members).
- Old claim "interactions validated via ServerRpc" — only `NetworkedInteractableToggleable` has RPCs; generic `InteractableObject` is purely client-side (`unverified` for other networked subclasses).

## Cross-links
36-Dragging · 39-Equipping · 12-Heat-Pursuit-Law · 47-ObjectStations · 57-Storage
