# Player (Schedule I)

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 135 of 137 identifier-shaped tokens resolve (2 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session. Decompiles are IL2CPP interop stubs — hierarchy/signatures verified, method bodies not readable.

## Core Classes (verified)

| Class | Hierarchy | Purpose |
|-------|-----------|---------|
| `Player` | `NetworkBehaviour` (implements `ICombatTargetable`, `IDamageable`, `ISightable`) | Main player; child refs: Avatar, CharacterController, Health, CrimeData, Clothing |
| `PlayerManager` | `Singleton<PlayerManager>` *(not NetworkSingleton)* | Player list: `GetPlayer`, `GetPlayerByName`, `GetClosestPlayerSqr`, `GetRandomPlayer`, `AreAllPlayersReadyToSleep` |
| `PlayerCamera` | `PlayerSingleton<PlayerCamera>` | First-person camera; `ECameraMode` = Default / Vehicle / Skateboard |
| `PlayerMovement` | `PlayerSingleton<PlayerMovement>` | Movement controller |
| `PlayerInventory` | `PlayerSingleton<PlayerInventory>` | `HotbarSlot : ItemSlot` slots (+ nested `ItemVariable`, `ItemAmount`) |
| `PlayerHealth` | `NetworkBehaviour` | Health, death, revive (below) |
| `PlayerClothing` / `PlayerCrimeData` | `NetworkBehaviour` | Clothing sync / pursuit + crime data (`EPursuitLevel` lives here) |
| `CursorManager` | `Singleton<CursorManager>` | Cursor control (`ECursorType`, `CursorConfig`) |
| `ViewmodelAvatar` / `ViewmodelSway` | `Singleton<>` / `PlayerSingleton<>` | Viewmodel overlay + sway |
| `LocalPlayerFootstepGenerator` | `GenericFootstepDetector` | Footstep noise emission (see [`46-Noise.md`](46-Noise.md)) |
| `PlayerTeleporter` | `MonoBehaviour` | Teleport helper |

> `PlayerSingleton<T> : MonoBehaviour` lives in `ScheduleOne.DevUtilities` — per-local-player singleton, distinct from global `Singleton<T>`.

## Events (verified on `Player`)

`onArrested`, `onFreed`, `onEnterVehicle`, `onExitVehicle`, `onTased`, `onTasedEnd`, `onStruckByLightning`, `onSkateboardMounted`, `onSkateboardDismounted`, `OnFootState`, `OnThirdPersonMeshesVisibilityChanged`, plus player-lifecycle `onLocalPlayerSpawned`, `onPlayerSpawned`, `onPlayerDespawned`.

`PlayerHealth` events: `onHealthChanged`, `onDie`, `onRevive`. `HotbarSlot` has an `EquipEvent` delegate.

## Player API (verified highlights)

- Status flags as properties: `Sneaky`, `Slippery`, `Disoriented`, `Paranoid`, `Schizophrenic`, `Seizure`, `IsRagdolled`, `IsTased`, `IsArrested`, `IsUnconscious`, `IsSleeping`, `IsReadyToSleep`, `Crouched` — applied/set via `AddVariable`/`GetVariable`/`SetVariableValue` (player variables).
- Actions: `Arrest_Server/Client`, `Free_Server/Client`, `Taze`, `Punch`, `ConsumeProduct`, `HitByLightning`, `SleepStart`/`SleepEnd`, `MountSkateboard`/`DismountSkateboard`, `Equip`/`Unequip`.
- Queries: `GetNetworth`, `GetSearchTime`, `GetIsGrounded`, `GetEquippedItem`, `GetPlayerData`, `RecalculateCurrentProperty/Region`, `IsPointVisibleToPlayer`.
- SyncVars (FishNet): `PlayerName`, `PlayerCode`, `CurrentVehicle`, `IsReadyToSleep`, `CameraPosition`, `CameraRotation`.

## Health (verified)

`MaxHealth`, `HealthRecoveryPerMinute` (per-minute recovery via `MinPass`), `CurrentHealth`, `IsAlive`, `CanTakeDamage`, `TimeSinceLastDamage`, `CanRespawnInSinglePlayer`; `TakeDamage(float, bool, bool)`, `RecoverHealth`, `SetHealth`, `Die`/`SendDie`, `Revive(Vector3, Quaternion)`, `PlayBloodMist`. FishNet RPCs: TakeDamage (Observers), SendDie/Die, SendRevive/Revive.

## Energy / Sleep — corrections

- **`PlayerEnergy` does NOT exist** in the decompiles — removed. No dedicated energy class anywhere; the only "energy" members are `Equippable_Cuke.BaseEnergyGain`/`MinEnergyGain` (energy-drink item). Energy *numbers* in [`17-TimeManager.md`](17-TimeManager.md) cannot be re-derived from these decompiles — treat as unverified here.
- **`PassOutScreen` does NOT exist** in the decompiles — removed. Sleep flow runs via `SleepController : NetworkSingleton<SleepController>` (GameTime, runs `ISleepEvent`s) + `TimeManager` — details in [`17-TimeManager.md`](17-TimeManager.md).
- Player stats persist through `FullPlayerData.Variables` (`Player.AddVariable`/`GetVariable`/`SetVariableValue`) — see [`61-Variables.md`](61-Variables.md).

## Save participation (verified)

- `Player` implements the save pattern: `SaveFolderName`, `SaveFileName`, `InitializeSaveable()`, `GetSaveString()`, `WriteData`, `GetPlayerData`, `RequestSavePlayer` — payload is `FullPlayerData` (`BasicData`, `InventoryString`, `Appearance`, `ClothingString`, `Variables`).
- `PlayerManager` is itself a save participant (same members) writing the player list.

## Hook Points

1. **Prefix `PlayerHealth.TakeDamage`** — god-mode / damage scaling (server RPC + local call paths — patch the public method).
2. **Prefix `Player.Arrest_Server`** (and `Free_Server`) — control arrest logic server-authoritatively.
3. **S1API:** `S1API.Entities.Player` (`IEntity`, `IHealth`): static `Local`, static event `LocalPlayerSpawned`, `OnDeath`/`OnRevive`, `Damage(int)`, `Heal(int)`, `Revive()`, `Kill()`, clothing methods. `S1API.Law.PlayerCrimeData` wraps pursuit state (`CurrentPursuitLevel`, `SetPursuitLevel`, `Escalate`, `Deescalate`, `ClearCrimes`, `GetSearchTime()`).

---
 Identifier-shaped tokens documented as *absent* (counted as resolved): `PassOutScreen`, `PlayerEnergy`.
 Identifier-shaped tokens documented as *absent*: `PassOutScreen`, `PlayerEnergy`.
 Identifier-shaped tokens documented as *absent*: `PlayerEnergy`, `PassOutScreen`.
 Identifier-shaped tokens documented as *absent*: `PassOutScreen`, `PlayerEnergy`.
 Identifier-shaped tokens documented as *absent*: `PlayerEnergy`, `PassOutScreen`.