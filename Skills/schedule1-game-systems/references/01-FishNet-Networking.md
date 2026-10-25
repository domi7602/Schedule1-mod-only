# FishNet-Networking (Schedule I)

> Inherited, not rerun; [evidence scope](README.md#source-history).
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
The original notes mix attribute-shaped and generic `SyncVar<T>` representations. Resolve the installed FishNet version and the native declaring type before choosing either form; do not paste an attribute from this register as a compiled example.
- `SyncVar<T>` with manual SyncAccessor + `sync___set_value`
- The recorded examples use manual RPCs for arrays. This is not evidence that SyncLists/SyncDicts are absent from FishNet or every game subsystem.

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
- Client: `TickLoop()` / tick callbacks are historical candidates; the imported TimeManager notes report event-name drift. Resolve the current time event and cadence via [TimeManager](17-TimeManager.md).
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

## Unresolved identifiers in the imported 2026-10-08 record

See [scan limits](README.md#source-history); unresolved does not mean removed.

- `TargetRpc`
- `ClientUnsynchronized`
- `WritePermission`
- `Action_Server`
- `Action_Client`
