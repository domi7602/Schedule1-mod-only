# Save-Load Timing

Static lists empty at OnGameplaySceneLoaded → use GameLifecycle.

```
Game → S1API OnPreLoad → Mod ResetState() (clear caches, destroy clones)
Game → S1API OnSaveInfoLoaded → Mod refresh Property/Item caches
Game → S1API OnLoadComplete → Mod LoadAndSpawn / Attach UI
Gameplay → in-memory Register/Unregister only
Game → S1API OnSaveComplete → Mod SaveAtomic(slot_{n}.json)
```

```csharp
GameLifecycle.OnPreLoad       += ResetState;
GameLifecycle.OnSaveInfoLoaded+= OnSaveInfoLoaded; // PotScanner v0.2.1 dropped 25s retry
GameLifecycle.OnLoadComplete  += OnLoadComplete;
GameLifecycle.OnSaveComplete  += OnSaveComplete;
```

Also `OnSceneWasLoaded("Main")` for in-session retries (StackLimitEngine.ApplyStackLimits).

---

## Buildable Restore Rule (verified 2026-08-22)

**⚠️ CRITICAL: Never call `SpawnStationAt` / `new GameObject` in `OnLoadComplete` for items that are registered `BuildableItem`s.**

The game restores all placed `BuildableItem`s itself on scene load via its own persistence system. If you also spawn a GameObject in `OnLoadComplete`, you get a duplicate at the same position. After a second save→load: 3 stations, then 4, etc.

**Correct pattern:** In `OnLoadComplete` only pre-stage runtime data in a Dictionary keyed by GUID. The game's `BuildableItem.Start()` (Harmony-postfixed) calls your setup, which then reads the pre-staged data via `GetRuntimeData(guid)`.

```csharp
// OnLoadComplete — only pre-stage, never spawn
foreach (var saved in data.Stations)
{
    // Check if controller already active (timing: game may have restored before OnLoadComplete)
    var existing = _activeStations.Find(s => s.StationGuid == saved.Guid);
    if (existing != null)
        ApplySaveData(saved.Guid, saved);  // controller already up
    else
        ApplySaveData(saved.Guid, saved);  // pre-stage; BuildableItem.Start() picks it up lazily
    // ← NEVER: SpawnStationAt(pos, rot, saved)  ← DUPLICATE BUG
}

// Also: NEVER hook OnSceneWasUnloaded to clear _activeStations.
// It fires for ANY scene unload (menus, etc.) and wipes state before OnSaveComplete.
// Only clear in OnPreLoad.
```

**GUID**: Use `BuildableItem.GUID` (persistent). Never `Guid.NewGuid()` in `Awake()` — it regenerates every session and breaks GUID matching.

