# Changelog

## 1.0.5 (2026-10-24)
- **Fixed: the clock is HHMM, not minutes since midnight.** `TimeManager.CurrentTime` is a
  24-hour integer (1954 = 19:54); 1.0.4 treated it as minutes, so `get_time_of_day` reported
  `time_minutes: 1954` and a `display` of `32h 34m` for a 19:54 clock, and `set_time_of_day`
  sent minutes into `SetTime_Server` - `{"time": 12}` produced 07:20 instead of 12:00.
  Evidence: `CurrentTime` is compared directly against contract/schedule windows that S1API
  documents as `hhmm`, the native helpers are `IsValid24HourTime(int)` /
  `Normalized24HourTime(int)` / `AddMinutesTo24HourTime(int, int)`, `DailyMinSum` is the
  minutes member, and a live read-only capture on 2026-10-23 returned 1954 while the HUD
  showed 19:54.
- `get_time_of_day` now returns `time_hhmm` (native value), `time_minutes` (true minutes since
  midnight, 0-1439) and a 24-hour `display` (`19:54`). A malformed clock reading withholds
  `time_minutes` and adds a `warning` instead of inventing a value.
- `set_time_of_day` accepts exactly one of `time` (decimal hours, 0-24 - unchanged contract,
  now converted correctly), `minutes` (0-1439) or the new `hhmm` (0-2359, or `"19:54"`).
  Responses carry `applied_hhmm`, `applied_minutes`, `readback_hhmm`, `readback_minutes` and
  the honest `changed` verdict. `24:00`/`24` wraps to `00:00`, matching the native clock.
- Validation still happens on the raw double/string before any cast, so an out-of-range value
  cannot reach `SetTime_Server` as garbage. Passing more than one time parameter is an explicit
  error instead of a silent precedence rule.
- New game-free suite `Source/Tests/S1MCP.Tests` (50 tests: both-way conversion, a full
  `0..2359` round-trip, decimal hours, text parsing and every rejection path).
  In-game acceptance of the read/write path on a real clock remains open.

## 1.0.4 (2026-10-17)
- **New: `set_time_of_day` / `get_time_of_day`.** Reads and writes the in-game clock against
  `ScheduleOne.GameTime.TimeManager`: `CurrentTime` is minutes-since-midnight (0-1439),
  `get_time_of_day` returns `time_minutes`, `elapsed_days` and a `display` string;
  `set_time_of_day` accepts `time` (hours, 0-24) or `minutes` (0-1439), calls the
  host-authoritative `SetTime_Server`, and reports an honest `changed` verdict from the
  readback (a non-host client cannot change the clock - the note says so).
- **New: `get_weather`.** Read-only snapshot of the nine native weather-condition weights
  (`sunny`, `cloudy`, `rainy`, `stormy`, `snowy`, `foggy`, `windy`, `hail`, `sleet`) read
  from `ScheduleOne.Weather.EnvironmentManager._currentWeatherConditions` - the same members
  S1API's WeatherRuntime mirrors for the Weather mod. Never selects sequences or triggers
  effects.
- **New: `list_mods`.** Reports every registered Melon (name, version, author, type) from
  the MelonLoader registry, including this server itself. Purely managed; stays on the
  main-thread queue on purpose.
- All three handlers answer a clean, structured `available:false` (with a `reason`) when the
  game world is not loaded - in the main menu TimeManager and EnvironmentManager have not
  spawned, and a request must never crash the connection there.
- The integration test grows to 10 checks: the four feature endpoints are exercised with
  RED-before-GREEN (they failed with method-not-found against 1.0.3). The GREEN run happens
  in the main menu, which proves routing and the clean `available:false` degradation — the
  true feature paths (actual clock write, real weather weights) need an in-game session as
  host and remain in-game-verification-open.

## 1.0.3 (2026-10-16)
- **New: fast path for connection commands.** `handshake`, `list_methods` and `heartbeat` (plus `status`) no longer go through the main-thread command queue. Handlers that never touch the Unity API are marked `IThreadSafeCommandHandler` and are executed directly on the TCP server thread, so a client can connect, handshake and read `status` while the game is paused - for example while it sits in the main menu or in the background with the main-thread queue not draining. Everything else is queued exactly as before.
- **`status` now reports fast-path activity** (`inline_requests`, `last_inline_method`), so a non-zero counter proves the fast path is live rather than merely compiled in.
- **Fixed: the handshake reported a hardcoded `1.0.0`.** It now reports the real assembly version, so a client can tell which build it is talking to.
- **Review delta (independent read-only reviews of 1.0.2 and 1.0.3, both PASS with no blocking findings; fixes applied for the real defects they surfaced):**
  - **Fixed: fast-path send-wait registration race.** The inline handler enqueued its response before the waiter was registered, so the 10 ms-polling `ResponseLoop` could write and confirm the response inside that window - the waiter then stalled for the full 10 s timeout even though the response was already on the socket. The waiter is now registered *before* any producer can enqueue a response (before `TryHandleDirectly` on the fast path, before `EnqueueCommand` on the queue path).
  - **Fixed: `status` could report a healthy client as disconnected.** `DropCurrentClient` now returns whether it actually dropped the installed connection; the response/heartbeat write-failure loops only report `OnClientDisconnected` when their drop was real, so a superseded stream's write failure can no longer clear the healthy replacement client's flag.
  - **Fixed: teardown on clean disconnect.** `HandleClient` now runs `DropCurrentClient` on every exit path (EOF, error, cancellation). Previously the socket and CTS stayed open until the next accept or a heartbeat failure up to ~30 s later, inflating `stale_clients_replaced`.
  - **Fixed: heartbeat ids could collide with client request ids.** Server-initiated heartbeat frames now use negative ids, so `CompleteSend` can never satisfy a client's waiter with a heartbeat.
  - **Hardened: `_connectedClient`/`_clientStream` are `volatile`** so loops reading them outside the client lock cannot act on a stale reference.
  - **Known residual (non-blocking, diagnostics-only):** `OnClientDisconnected` reads `CurrentGeneration` after `DropCurrentClient` has released the lock; if a reconnect lands inside that nanosecond window the status snapshot can briefly misreport the healthy replacement as disconnected until the next accept. No protocol or resource effect.

## 1.0.2 (2026-10-16)
- **Fixed: a dead client could lock the server out permanently.** The read path treated EOF as "no data yet" - guarded by `Stream.CanRead`, which stays `true` after EOF - and the client handler then retried every 200 ms forever, still holding the accept loop. Later connections were refused because `.Connected` still reported `true` for the dead peer. EOF on the length prefix is now `EndOfStreamException`, partial prefixes are read to completion, the listener accepts continuously, and a new connection **replaces** a stale one instead of being rejected.
- **Fixed: an idle read starved the heartbeat.** Read and write shared one semaphore, so while a read blocked waiting for the next request the heartbeat could not be written - the dead-client watchdog was tied up by the very thing it was meant to watch. Reads are no longer serialized against writes.
- **Fixed: blocking acknowledgment read without timeout.** Acks are now ordinary messages consumed at the top of the read loop; a client that sends none no longer hangs the connection.
- **Fixed: response-wait polling.** The old "poll `_responseQueue.Count`" heuristic could not attribute a response to a request and burned 10 s (`Waited 10000ms for response to be sent`) whenever a write failed. Writes are confirmed per response ID.
- **Background loops hardened.** `ServerLoop`/`ResponseLoop`/`HeartbeatLoop` are `async Task` with top-level error handling instead of `async void`; an exception can no longer kill a loop silently.
- **TCP keep-alive** (10 s idle / 3 s interval) so the OS eventually reports a half-open connection.
- **New `status` command:** listener state, client endpoint, connection generation, totals, last request/error, queue depths, uptime.

## 1.0.1 (2026-08-24)
- **Fixed: MelonLoader log spam on agent connect.** `ModLogger.Debug` wrote unconditionally to Latest.log; every MCP message dumped its full raw JSON (handshake + `tools/list` schemas → MBs of spam on client login).
  - Debug logging is now **opt-in** via MelonPreferences `S1MCPServer/DebugLogging` (`UserData/S1MCPServer.cfg`, default `false`).
  - Payload dumps (raw request JSON, serialized response JSON, request params) are truncated to 500 chars via `ModLogger.DebugPayload` — even when debug is enabled.
  - Info/Warn lifecycle messages (connect/disconnect/listening) unchanged.

## 1.0.0
- added stuff
