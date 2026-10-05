# Multiplayer & Lobby (Schedule I)

> **Redirect stub** (consolidated 2026-10-05): FishNet fundamentals (NetworkSingleton, SyncVar patterns, RPC naming, server authority, PredictedSpawn) live in **[`01-FishNet-Networking.md`](01-FishNet-Networking.md)**. This file keeps only the lobby/splitscreen class register. Class-list only — not yet re-verified against 0.4.7f9.

## Core Classes

| Class | Purpose |
|-------|---------|
| `Lobby` | Create / Join / Manage (Steamworks-based, invitations via Steam Overlay) |
| `LobbyInterface` / `JoinLocal` | Lobby UI / local join |
| `AutoNetworkStart` | Automatic start as Host/Client |
| `TransportInitializer` | Selects transport (Steam, Local) |
| `LocalMultiplayerTool` | Splitscreen co-op (2 players, controller for P2) |
| `NetworkConditionalObject` | Network-dependent objects |
| `IStaggeredReplicator` / `ReplicationQueue` | Replication plumbing |

## Sync model (details in 01)

FishNet 3.x · `NetworkSingleton<T>` for global managers · server-authoritative · SyncVars + RPCs.

**Mod-side rule:** host authority for economy/world writes via `NetworkGuard.IsHostOrSingleplayer()` — see `schedule1-modding` Rule 15 + `schedule1-economy` §6.
