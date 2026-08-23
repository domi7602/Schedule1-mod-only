# Multiplayer & Lobby (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `Lobby` | Lobby management |
| `AutoNetworkStart` | Automatic network start |
| `TransportInitializer` | Transport initialization |
| `LocalMultiplayerTool` | Splitscreen tool |

## Lobby System

- Steamworks-based networking
- `Lobby`: Create / Join / Manage
- Invitations via Steam Overlay
- `NetworkConditionalObject`: Network-dependent objects

## Splitscreen

- `LocalMultiplayerTool`: Local co-op via splitscreen
- Two players on one screen
- Shared inventory and progress
- Player 2 uses controller

## Network Start

- `AutoNetworkStart`: Automatic start as Host/Client
- `TransportInitializer`: Selects transport (Steam, Local)
- `IStaggeredReplicator`: Staggered replication
- `ReplicationQueue`: Replication queue

## UI

- `LobbyInterface`: Lobby UI
- `JoinLocal`: Join local game
- Steam Overlay for invitations

## Sync

- FishNet 3.x as networking layer
- NetworkSingleton<T> for global managers
- Server-authoritative model
- SyncVars + RPCs for state sync
