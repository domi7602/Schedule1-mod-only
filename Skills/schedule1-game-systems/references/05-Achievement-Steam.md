# Achievement/Steam (Schedule I)
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

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `RuntimeInitializeOnLoadMethod`
- `StoreStats`
- `PullAchievements`
- `SetAchievement`
- `RunCallbacks`
- `TrashRecycled`
