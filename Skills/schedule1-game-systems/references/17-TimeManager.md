# TimeManager (Schedule I)

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 36/40 identifier-shaped tokens resolve (3 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Architecture
- `NetworkSingleton<TimeManager>` (Server-authoritative)
- Format: `int CurrentTime` in 24h format (HHMM, e.g. 900 = 09:00)
- `CycleDuration` = 24 minutes real-time per game day

**Recorded local clock-unit observation, 2026-10-22 (0.4.7f12):** a read-only observation returned `CurrentTime=1954` while the native DayNightController reported about `19.90776` hours / `.82951415` day fraction. <!-- skills-health: version-evidence (game build of the dated clock-unit observation, not a current target pin) -->
Convert HHMM to minutes before minute-based policy calculations; `1954` is 19:54, not 1,954 elapsed minutes.

**Bridge correction recorded 2026-10-24:** S1MCP 1.0.5 returns `time_hhmm` (native), `time_minutes` (true 0-1439) and a 24-hour `display`; `set_time_of_day` accepts exactly one of `time` (hours), `minutes` or `hhmm`. <!-- skills-health: version-evidence (historical bridge release that introduced the corrected clock contract) -->
S1MCP 1.0.4 mislabelled the raw value as `time_minutes` (`32h 34m`) and its write path landed on 07:20 for a requested 12:00; that old write acceptance is superseded. <!-- skills-health: version-evidence (affected historical bridge release retained for diagnosing the HHMM defect) -->
The [bridge changelog](../../../ThirdParty/S1MCPServer-master/S1MCPServer/CHANGELOG.md) records owner acceptance on 2026-10-24 and a later live read (`812` → 08:12 / 492 min) plus a no-op `hhmm: 836` write with matching 08:36 / 516 min readback. Those observations do not exercise every write form. The [compatibility matrix](../../../docs/compatibility.md#verification-matrix) owns current acceptance and limits; the earlier request for a fresh check is closed by that record.
WorldLook's native-baseline repair corrects its own conversion. The clock-unit evidence does not verify the event/cycle claims below.

## Important Constants
| Constant | Value | Description |
|----------|-------|-------------|
| DefaultTime | 0900 | Start time |
| WakeTime | 0700 | Wake-up time |
| EndOfDay | 0400 | Time stand-still start |
| MIN_SLEEP_TIME | 1800 | Earliest sleep time |
| MaxSleepTime | 12h | Max sleep duration |
| MinSleepTime | 4h | Min sleep duration |
| TickDuration | 0.5s | Client tick rate |

## Day / Night Cycle
- **Day**: 06:00 – 17:59
- **Night**: 18:00 – 05:59
- Time stands still between **04:00 – 06:00** (`EndOfDay`)

## Events
The original notes report an event-surface check against the f12 decompile on 2026-10-08; this package has not reproduced that check. `TimeManager` exposes
`onDayPass`, `onHourPass`, `onWeekPass`, `onTimeChanged`, `onTimeSet`, `onUpdate`,
`onFixedUpdate` and `onTimeSkip(int)`.

| Event | Trigger |
|-------|---------|
| `onTimeChanged` | Current time value changed |
| `onTimeSet` | Time was explicitly set |
| `onHourPass` | Every top of the hour |
| `onDayPass` | Day change |
| `onWeekPass` | Week change |
| `onTimeSkip(mins)` | On a time skip |
| `onUpdate` / `onFixedUpdate` | Per-frame / fixed-step callbacks |

Related public helpers observed on f12: `GetDateTime()`, `GetTotalMinSum()`,
`IsCurrentTimeWithinRange(min, max)`, `IsCurrentDateWithinRange(start, end)`,
`ShouldMinutePass()`, `TickLoop()`, `TimeLoop()`.

> **Not located in the recorded snapshot:** earlier revisions of this file listed `onMinutePass`,
> `onUncappedMinutePass` and `onTick`. None exist in the f12 assemblies — re-derive
> minute-level hooks from `ShouldMinutePass()` / `onTimeChanged` before using them. Do not replace an event with a similarly named method without checking timing, units and subscription semantics.

## Sleeping (TimeSkip)
1. Interact with bed (from 18:00 onwards)
2. `SleepMenu` (UI): select hours; `SleepController` drives the session
3. All players must be ready to sleep
4. Server advances time (no public `SkipForwardToTime` in the f12 assemblies — use the
   verified controller surface; observing `onTimeSkip` is not a command to advance time)
5. The skip difference recalculates plant growth + drug processing catch-up
6. **Automatic save** upon sleep completion

## Curfew
| Phase | Time |
|-------|------|
| Warning | 20:30 |
| Curfew active | 21:00 – 05:00 |
| Hard Curfew | 21:15 – 05:00 |

- Active only when `LE_Intensity >= IntensityRequirement` (default 5)
- Violation: `ViolatingCurfew` = $100 fine

## Stamina (formerly documented as "Energy")
**Corrected 2026-10-08.** The f12 assemblies contain **no** `Energy`, `MaxEnergy`,
`PlayerEnergy` or `PassOut` identifiers (0 hits across all 2 234 decompiled files).
The player vital is stamina-based instead:

- `Il2CppScheduleOne.UI.StaminaBar` — HUD element (`MonoBehaviour`) with the static
  `UpdateStaminaBar(float change)` entry point plus `StaminaFadeTime` and `StaminaShowTime`
- Condition state lives in `ECondition` / `EConditions` / `ConditionFlags`

The previous energy numbers (max value, drain rate, sprint multiplier, fainting penalty)
are **not verified on f12** — re-derive them from the current stamina code before use.

## Days of the Week (EDay)
`Monday(0) → Tuesday → Wednesday → Thursday → Friday → Saturday → Sunday(6)`
- `ElapsedDays` = total days elapsed
- `DayIndex = ElapsedDays % 7`

---

## Unresolved identifiers in the imported 2026-10-08 record

The imported scan did not resolve the following tokens within its recorded search scope. These may be placeholders, attributes from another assembly or drift candidates. Search the actual declaring assembly and version before concluding that an API was removed.

- `SkipForwardToTime`
