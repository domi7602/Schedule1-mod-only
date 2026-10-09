# FishNet-Networking (Schedule I)
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 19/24 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.


## Version
FishNet (Steamworks-Transport)

## Base Classes

| Class | Purpose |
|---|---|
| `NetworkSingleton<T>` | Custom base for all global managers (MoneyManager, TimeManager, LevelManager, ProductManager) |
| `Singleton<T>` | Pure MonoBehaviour (local, non-networked) |
| `PersistentSingleton<T>` | Cross-scene (DontDestroyOnLoad) |

## SyncVar Pattern
```csharp
[SyncVar(WritePermissions = WritePermission.ClientUnsynchronized)]
public NetworkObject NPCUserObject;
```
- `SyncVar<T>` with manual SyncAccessor + `sync___set_value`
- No SyncLists/SyncDicts – instead manual RPCs for arrays

## RPC Naming Convention
| Type | Server Function | Client Function |
|---|---|---|
| `[ServerRpc]` | `Action_Server(params)` | — |
| `[ObserversRpc]` | — | `Action_Client(params)` |
| `[TargetRpc]` + `[ObserversRpc]` | `Action_Client(conn, params)` | Both |

Dual attributes (`[ObserversRpc(RunLocally=true)]` + `[TargetRpc]`) on the same method:
- `conn == null` → ObserversRpc (all clients)
- `conn != null` → TargetRpc (single client only)

## Server Authority
- Strictly authoritative: Clients send requests via ServerRpc, server validates + distributes
- `RequireOwnership = false` for most player actions
- `WritePermission.ClientUnsynchronized` for some SyncVars (client can change locally, server maintains its own state)

## Time Loop
- Server: `TimeLoop()` → waits `MinuteDuration` → `PassMinute()` → `PassMinute_Client()` (ObserversRpc)
- Client: `TickLoop()` → every 0.5s `onTick`
- Plants grow server-side, propagated to clients via RPC

## Object Spawning
- `BuildableItem` uses `[RequireComponent(typeof(PredictedSpawn))]` (FishNet PredictedSpawn)
- Client builds locally → `SendInitializationToServer()` → server validates + ObserversRpc to all
- Late-Join: `OnSpawnServer(conn)` sends complete state

## Lobby System
- Steamworks Matchmaking (max 4 players)
- `Lobby.cs` (PersistentSingleton, not a NetworkBehaviour)
- Chat messages trigger game start

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `TargetRpc`
- `ClientUnsynchronized`
- `WritePermission`
- `Action_Server`
- `Action_Client`
