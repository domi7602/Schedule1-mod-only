# Buildable Restore Rule (Guided Spawning)

> verified: 2026-08-22 (AutoPackagingStation duplicate-station bug); cross-checked against `common-errors.md` §20 2026-10-05.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 11 of 11 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Save/load **timing** (which lifecycle hook fires when, and which are dead) lives in the canonical doc:
[`schedule1-troubleshooting/references/save-load-timing.md`](../../schedule1-troubleshooting/references/save-load-timing.md) — verified order 2026-09-29: Scene 'Main' → `OnPreLoad` → `OnLoadComplete`; **`OnSaveInfoLoaded` fires 0× in the instrumented session**. Atomar-Speicherung: `safestorage-atomic.md`.

This file keeps the **placement-specific rule** that pairs with that timing doc.

---

## The Rule

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

---

## Cross-References

- Expanded decoder: `schedule1-troubleshooting/references/common-errors.md` §20 (Buildable Duplicate-Spawn Bug)
- Timing + logging pattern: `schedule1-troubleshooting/references/save-load-timing.md` §1/§10
- Atomic persistence: `safestorage-atomic.md` (same skill)
- Slot isolation: `slot-isolation.md` (same skill)
