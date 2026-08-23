# QoL_BiggerTrees.Il2CPP v1.1.1f

> **Directory:** `QoL_BiggerTrees.Il2CPP`
> **Author:** FannsoNetti
> **Category:** Quality of Life & Game Mechanics
> **Entry Class:** MapSceneHook extends `MelonMod`
> **Target Framework:** netstandard2.1
> **Files:** 4 (2 .cs)

## Purpose

Increases `Terrain.treeLODBiasMultiplier` to **6.7** in Hyland Point, rendering trees at full detail level at significantly greater distances before swapping to lower LOD tiers. Visual effect: denser, opaque forests with less abrupt LOD pop-in when approaching.

## Entry Point & Architecture

`BiggerTrees\MapSceneHook.cs` — Lifecycle:
- `OnSceneWasLoaded`: On scene `"Main"`, launches the `ApplyWithRetry()` coroutine. All other scenes are ignored.

Architecture: Minimally invasive single-purpose mod. One MelonMod class + two iterator state machines (`ApplyWithRetry` for retry logic, `NudgeMapOneFrame` for terrain tick triggers). Both decompiled as `CompilerGenerated` IEnumerator classes (IL2CPP decompiler output).

## Key Files

- `BiggerTrees/MapSceneHook.cs` — Main logic: Scene detection, `ApplyWithRetry()` coroutine with up to 20 attempts, `NudgeMapOneFrame` coroutine for terrain wake-up.
- `Properties/AssemblyInfo.cs` — Assembly metadata.

## Patterns & Lessons Learned

- **Coroutine with Initial Delay + Retry**: `WaitForSecondsRealtime(5f)` → followed by up to 20 retries every 2s using `WaitForSecondsRealtime(2f)` — highly resilient against slow scene initialization.
- **`WaitForSecondsRealtime` instead of `WaitForSeconds`**: Realtime ticks even when TimeScale=0, essential for consistent retry intervals.
- **Map Nudge Trick**: `NudgeMapOneFrame` temporarily sets Transform.position to `(0, 0.01, 0)` and back to `(0, 0, 0)` — triggering a terrain update tick to activate `treeLODBiasMultiplier`.
- **Hardcoded LOD Multiplier**: `treeLODBiasMultiplier = 6.7f` — empirically determined value generating noticeably denser forests without severe performance costs.
- **`GameObject.Find("Map")` Lookup**: Direct scene graph lookup instead of singleton API — effective because "Map" is a stable root node name.
- **`Find("Hyland Point/Main Terrain")`**: Expected path to the terrain component — hardcoded to a specific game version, susceptible to map updates.
- **MaxAttempts Pattern**: 20 attempts × 2 s = 40 s timeout — highly defensive, covering extreme SSD loading times.

## Dependencies

**MelonLoader References:** MelonLoader, Harmony, Assembly-CSharp, UnityEngine
**Additional Libraries:** None

## Cross-References

- See `Analysis/Systems/43-MapAndLocations.md` (Hyland Point, Terrain, world geometry)
- See `Analysis/Systems/59-TilesGrid.md` (LOD bias, terrain tree)
- See `Analysis/Systems/64-Weather.md` (Terrain weather effects)
- See `Analysis/Learnings/18-Visual-Enhancement-Patterns.md` (LOD multiplier pattern)
- See `Analysis/Learnings/04-Embedded-Assets-Build-Placement.md` (GameObject.Find vs. Singleton)

---
*Created from source code analysis*