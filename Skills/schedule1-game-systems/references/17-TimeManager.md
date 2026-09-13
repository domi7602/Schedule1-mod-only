# TimeManager (Schedule I)

## Architecture
- `NetworkSingleton<TimeManager>` (Server-authoritative)
- Format: `int CurrentTime` in 24h format (HHMM, e.g. 900 = 09:00)
- `CycleDuration` = 24 minutes real-time per game day

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
| Event | Trigger |
|-------|---------|
| `onMinutePass` | Every minute (except 04:00–06:00) |
| `onUncappedMinutePass` | Every minute (including 04:00–06:00) |
| `onTick` | Every 0.5s (Client) |
| `onHourPass` | Every top of the hour |
| `onDayPass` | Day change (00:00) |
| `onWeekPass` | Week change (Monday) |
| `onTimeSkip(mins)` | On time skip |
| `onSleepStart/End` | Sleep start/end |

## Sleeping (TimeSkip)
1. Interact with bed (from 18:00 onwards)
2. `SleepCanvas`: select hours (4–12h)
3. All players must be `IsReadyToSleep`
4. Server: `SkipForwardToTime(0700)` (Wake-up)
5. TimeSkip difference recalculates plant growth + drug processing catch-up
6. **Automatic save** upon sleep completion

## Curfew
| Phase | Time |
|-------|------|
| Warning | 20:30 |
| Curfew active | 21:00 – 05:00 |
| Hard Curfew | 21:15 – 05:00 |

- Active only when `LE_Intensity >= IntensityRequirement` (default 5)
- Violation: `ViolatingCurfew` = $100 fine

## Energy
- `PlayerEnergy.MaxEnergy = 100`
- Drain: ~0.076/min (= full depletion in 22h)
- Sprinting: 1.3x multiplier
- Below 20: eyelid overlay begins closing
- At 0: fainting (`PassOut`), $50–$500 cash loss, teleported

## Days of the Week (EDay)
`Monday(0) → Tuesday → Wednesday → Thursday → Friday → Saturday → Sunday(6)`
- `ElapsedDays` = total days elapsed
- `DayIndex = ElapsedDays % 7`
