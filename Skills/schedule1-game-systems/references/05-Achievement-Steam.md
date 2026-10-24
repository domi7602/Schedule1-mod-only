# Achievement/Steam (Schedule I)

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 10/16 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Steam Integration
- **Steamworks.NET** (official Valve C# binding)
- `SteamManager` (MonoBehaviour, DontDestroyOnLoad)
- `SteamAPI.Init()` + `SteamAPI.RunCallbacks()` per frame
- AppID: 3164500

## AchievementManager
- **Static Class** (not a Singleton, not a MonoBehaviour)
- `Init()` via `[RuntimeInitializeOnLoadMethod]`
- Dictionary `achievementUnlocked` (cache, loaded via `TryPullAchievements()` — there is no plain `PullAchievements`)
- **100% Event-based** – no polling, no update loop

## Achievement List

| # | Name | Condition |
|---|------|-----------|
| 0 | COMPLETE_PROLOGUE | Complete tutorial |
| 1 | RV_DESTROYED | **Never triggered** (enum exists, no call) |
| 2 | DEALER_RECRUITED | **Never triggered** (enum exists, no call) |
| 3 | MASTER_CHEF | Discover product with market value >= $100 |
| 4 | BUSINESSMAN | Networth >= $100,000 |
| 5 | BIGWIG | Networth >= $1,000,000 |
| 6 | MAGNATE | Networth >= $10,000,000 |
| 7 | UPSTANDING_CITIZEN | Recycle 500 trash items |
| 8 | ROLLING_IN_STYLE | Purchase golden skateboard |
| 9 | LONG_ARM_OF_THE_LAW | Get arrested |
| 10 | INDIAN_DEALER | Pickpocket a dealer (within 60s after deal) |
| 11 | URBAN_ARTIST | Spray 25 graffiti spots |
| 12 | FINISHING_THE_JOB | Defeat cartel |

## Storage
- **Steam Cloud only** (`SteamUserStats.SetAchievement()` + `StoreStats()`)
- **No** Steam Stats API – progress tracked via custom `VariableDatabase`

## VariableDatabase
- `NetworkSingleton<VariableDatabase>` (ISaveable)
- Tracks: `"TrashRecycled"`, `"highestValueProduct" (field, lower camel)`, `"LifetimeEarnings"`
- Network-replicated (ObserversRpc/TargetRpc)

## Beta Blockade
- `-beta` command line flag → `GameManager.IS_BETA = true`
- Blocks all `UnlockAchievement()` calls (silently)

---

## Unresolved identifiers in the imported 2026-10-08 record

The imported scan did not resolve the following tokens within its recorded search scope. These may be placeholders, attributes from another assembly or drift candidates. Search the actual declaring assembly and version before concluding that an API was removed.

- `RuntimeInitializeOnLoadMethod`
- `StoreStats`
- `PullAchievements`
- `SetAchievement`
- `RunCallbacks`
- `TrashRecycled`
