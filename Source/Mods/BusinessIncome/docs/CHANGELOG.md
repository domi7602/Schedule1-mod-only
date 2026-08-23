# Changelog

## 0.1.0 (2026-08-17)

- **Initial version**:
  - Daily passive income for all owned `Business.OwnedBusinesses`.
  - Multiplayer host-authority protection via `NetworkGuard.IsHostOrSingleplayer()`.
  - Save-slot-isolated `PayoutStateStore` with atomic writes (`SafeStorage`) and crash protection.
  - Deterministic revenue model with hash variance (±15%), employee bonus and weekend factors.
  - Automatic online transactions via S1API `Money.CreateOnlineTransaction`.
  - In-game HUD notifications via `NotificationsManager` with sound effect.
  - Extensive dev console and DooDesch `hash` terminal integration (`biz`).
