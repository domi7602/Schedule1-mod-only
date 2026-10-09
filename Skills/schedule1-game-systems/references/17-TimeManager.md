# TimeManager (Schedule I)
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 36/40 identifier-shaped tokens resolve (3 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.


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
Event surface re-checked against the f12 decompile on 2026-10-08. `TimeManager` exposes
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

> **Removed/renamed:** earlier revisions of this file listed `onMinutePass`,
> `onUncappedMinutePass` and `onTick`. None exist in the f12 assemblies — re-derive
> minute-level hooks from `ShouldMinutePass()` / `onTimeChanged` before using them.

## Sleeping (TimeSkip)
1. Interact with bed (from 18:00 onwards)
2. `SleepMenu` (UI): select hours; `SleepController` drives the session
3. All players must be ready to sleep
4. Server advances time (no public `SkipForwardToTime` in the f12 assemblies — use the
   controller surface or the `onTimeSkip` callback instead)
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

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `SkipForwardToTime`
