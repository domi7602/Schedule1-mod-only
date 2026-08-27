# Changelog

## 1.0.1 (2026-08-24)
- **Fixed: MelonLoader log spam on agent connect.** `ModLogger.Debug` wrote unconditionally to Latest.log; every MCP message dumped its full raw JSON (handshake + `tools/list` schemas → MBs of spam on client login).
  - Debug logging is now **opt-in** via MelonPreferences `S1MCPServer/DebugLogging` (`UserData/S1MCPServer.cfg`, default `false`).
  - Payload dumps (raw request JSON, serialized response JSON, request params) are truncated to 500 chars via `ModLogger.DebugPayload` — even when debug is enabled.
  - Info/Warn lifecycle messages (connect/disconnect/listening) unchanged.

## 1.0.0
- added stuff
