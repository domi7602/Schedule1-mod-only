# Equipping (Schedule I)
> verified: classes + hierarchy + event types + save-participation re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

Namespace: `Il2CppScheduleOne.Equipping` (+ `Equipping.Framework`).

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `Equippable` | `MonoBehaviour` | Base equippable component: `itemInstance`, `CanInteractWhenEquipped`, `CanPickUpWhenEquipped`, `Equip(ItemInstance)`, `Unequip()`, `Update()` |
| `EquippedItemHandler` | `NetworkBehaviour` | Spawned handler instance per equipped item (first/third-person setup) |
| `NetworkedEquipper` | `NetworkBehaviour` | Equip/unequip orchestration + FishNet SyncList replication |
| `EquippableDataRegistry` | `PersistentSingleton<EquippableDataRegistry>` | GUID → `EquippableData` lookup (`GetEquippableData(Guid)`, `RegisterEquippableData`, `_equippableDataList`) |

## Class Tree (verified bases)
- `Equippable_Viewmodel : Equippable` — viewmodel placement (`localPosition`, `localEulerAngles`, `localScale`, `AvatarEquippable` ref)
  - `Equippable_AvatarViewmodel` → `Equippable_MeleeWeapon` (`Hit(power)`, `ExecuteHit`, `StartLoad`/`Release`, `UpdateCooldown`), `Equippable_RangedWeapon` → `Equippable_Revolver` (`SetDisplayedBullets`), `Equippable_PumpShotgun` (`GetBulletDirections`)
  - `Equippable_Pourable` (`CanPour(GrowContainer, out reason)`, `StartPourTask`) → `Equippable_Additive`; `PourableWaterContainerEquipped`
  - `Equippable_SprayBottle` (`CanSpray`, `StartSprayTask(MushroomBed)`), `Equippable_Seed` (`StartSowSeedTask(Pot)`), `Equippable_Trimmers`, `Equippable_TrashGrabber` (`PickupTrash`, `EjectTrash`, `GetCapacity`), `Equippable_Cuke` (`Drink`, `ApplyEffects`), `MushroomSpawnEquipped`, `LiquidMeth_Equippable`
- Direct `Equippable` children: `Equippable_BuildableItem`, `Equippable_SurfaceItem`
- The item-definition side of equippables is covered in 09-Inventory-ItemFramework — do not duplicate here.

## Handler & Network Flow (verified)
- `EquippedItemHandler`: `IsEquipped` prop, `Equipped(IEquippableUser, EquippableData)`, `EquippedWithItem(user, data, BaseItemInstance)`, `Unequipped()`, `SetupParent()`/`SetupFirstPerson()`/`SetupThirdPerson()`, `UserUpdate()`, SyncVars `_user`/`_equippableData`
- `NetworkedEquipper`: `Equip(EquippableData|BaseItemInstance, bool networked)`, `Unequip(handler)`, `UnequipAll()`, `CanEquip(EquippableData)`, `IsRightHandOccupied()`/`IsLeftHandOccupied()`, `IsItemEquipped(handler)`, `CreateHandlerForEquippable(EquippableData)`; SyncList `_networkEquippedItems` (`NetworkEquippedItems_OnChange`); RPCs `Unequip_Server`/`Unequip_Client`/`AddNetworkedEquippedItem_Server`/`RemoveNetworkedEquippedItem_Server` (hash 897730888)
- Subclasses: `PlayerNetworkedEquipper` (`_player`), `NPCNetworkedEquipper` — both only override `GetUser()`

## Framework folder (verified)
- `EquippableItemDefinition : GenericEquippableItemDefinition<EquippableData> : StorableItemDefinition` (+ `ValidateDefinition()`)
- `EquippableHandlerService`: static `SetupHandlerKeys()`, `GetHandlerPrefab(EquippableData) → IEquippedItemHandler`; nested `HandlerInfo { DataType, HandlerType }`
- `EquipConfiguration : Configuration<EquipSettings>` — `Handlers`, `TryGetHandlerForData(Type, out IEquippedItemHandler)`
- `EquippableDataSerializer` / `INetworkedEquippableUserSerializer` — FishNet Writer/Reader extensions
- `CustomHandlerEquippableData : EquippableData` (`OnValidate`), `INetworkedEquippableUser` (`Equip_Networked(EquippableData|BaseItemInstance)`)
- `EquipTester` — dev-only MonoBehaviour
- **Core types `EquippableData`, `IEquippableUser`, `IEquippedItemHandler`, `TPEquippedItem` live in `ScheduleOne.Core.Equipping.Framework`** (per using-directives in `EquippableDataRegistry`/`EquippableHandlerService`); wrapper sources are **not present** in decompile generation 2026-10-02 — member details `unverified`.
- **Namespace corrections vs. older docs:** `AvatarEquippable`, `AvatarGun`, `AvatarMeleeWeapon`, `FlashlightAvatarEquippable` are in `Il2CppScheduleOne.AvatarFramework.Equipping`; `ViewmodelAvatar`/`ViewmodelSway` are in `Il2CppScheduleOne.PlayerScripts`. `TPEquippedUmbrella : TPEquippedItem` sits in this namespace.

## Events / UnityEvents
| Event | Type | Raised by |
|-------|------|-----------|
| `Equippable_RangedWeapon.onFire` / `onReloadStart` / `onReloadIndividual` / `onReloadEnd` / `onCockStart` | UnityEvent | `Fire()` / `Reload()` / `NotifyIncrementalReload()` / `Cock()` |
| `Equippable_TrashGrabber.onPickup` | UnityEvent | `PickupTrash(TrashItem)` |
| `EquippedItemHandler.OnUnequipped` | **C#-style event** (`Il2CppSystem.Action`, `add_`/`remove_` accessors) | `Unequipped()` — not a UnityEvent |

## Save Participation
- **No `ISaveable` / `SaveData` in the Equipping namespace** (grep). Equipped state persists indirectly via inventory/item instances; equipped handler replication is FishNet SyncList only. Exact re-equip-on-load path: `unverified`.

## Hook Points
1. **Prefix `NetworkedEquipper.CanEquip(EquippableData)`** — deny/allow equips globally (restrict weapons in safe zones); plain bool method, patchable.
2. **Postfix `EquippedItemHandler.Equipped(...)` / `Unequipped()`** — react to equip state changes (HUD, buffs); virtual, non-inline.
3. **Postfix `EquippableDataRegistry.RegisterEquippableData`** — observe custom/game equippable registrations at load time.
- **S1API (3.2.1-beta.8) wrappers (verified in source):** `S1API.Items.Equippable` (wraps native `Equippable`; `CanInteractWhenEquipped`, `CanPickUpWhenEquipped`, virtual `Equip(ItemInstance)`/`Unequip()`), `S1API.Items.EquippableBuilder`, `S1API.Items.AvatarEquippableRegistry` / `AvatarEquippablePaths`.

## Not Implemented / Unverified
- No equipping events on the manager level (only per-weapon UnityEvents + `OnUnequipped`).
- Ranged weapon ammo details (`GetMagazine(out StorableItemInstance)`, `IsReloadReady`) verified as methods; ammo persistence semantics `unverified`.

## Cross-links
09-Inventory-ItemFramework · 36-Dragging · 40-Interaction · 47-ObjectStations · 01-FishNet-Networking
