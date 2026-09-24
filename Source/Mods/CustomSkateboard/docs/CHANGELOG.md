# Changelog



## 1.1.5 (2026-09-13) — Bug-audit fixes round 5 (audit 2026-09-13)
- Dead state removed: `s_disableTerrainSlowdownCached` / `SetDisableTerrainSlowdownCached` in `SkateboardVisualPatches` and `ClearStyledCache()` in `CyberSkateboardVisualizer` — fields were written, never read. Call site in `SkateboardItemFactory.TuneSkateboard()` removed.
- Memory waste eliminated (~2 static bool + 1 static method + 1 call per Tune).

## 1.1.4 (2026-09-12) — Bug-audit fixes round 4 (audit 2026-09-12)
- **Validate/Tune clamp divergence resolved:** `TuneSkateboard` now calls `config.Validate()` first and only clamps the three additional fields (`PushForceDuration`, `BrakeForce`, `AirMovementForce`). The `Validate()` call can no longer be overwritten by a divergent `Tune` clamp pair, and the central source now contains all 11 fields.

## 1.1.3 (2026-09-12) — Bug-audit fixes round 3 (audit 2026-09-12)
- Pass-2 deck heuristic (`TrySwapDeckMesh`) now applies the same `IsDeckMeshSane` as pass 1. Without the sanity check a combined/root/collision mesh with just `Contains("board")` would have matched and overwritten the custom geometry.

## 1.1.2 (2026-09-11)
- Settings-sharing detector: warns via error log if a tuned board shares its _settings object with other boards (vanilla-pollution proof).

## 1.1.1 (2026-09-10)

- IsGameplayScene strictly 'Main' (no tuning-state loss in the menu).

## 1.1.0 (2026-09-09)

- **Tuning: higher speed + better steering** (user request; values apply to code defaults AND live config `UserData/MelonPreferences.cfg [CustomSkateboard]`, since stored TOML values override code defaults):
  - Speed: `TopSpeed_Kmh` 100 → **140**, `PushForceMultiplier` 5.2 → **6.5**, `PushCooldown` 0.22 s → **0.18 s**, `LongitudinalFrictionMultiplier` 0.16 → **0.13** (less rolling resistance, speed holds longer).
  - Steering: `TurnForce` 15 → **20**, `TurnChangeRate` 64 → **85**, `TurnReturnToRestRate` 56 → **75**, `LateralFrictionForceMultiplier` 1.60 → **1.85** (high-speed grip), `MaxBoardLean` 28° → **33°**, `BoardLeanRate` 60 → **75**.
  - Curve coverage verified: turn/push curves are defined up to 160/150 km/h — 140 km/h stays well within keyframes.
  - Unchanged: jump physics (JumpForce 18), anti-gravel, price, visuals.

## 1.0.3 (2026-09-03)
- **Fix: Ollie flattening (flat jump)**: custom board jumps first raised the nose (vanilla ollie animation). Root cause: the axle jump curves (`FrontAxleJumpCurve`/`RearAxleJumpCurve`) exist twice — on the board instance AND in the `SkateboardSettings` ScriptableObject; vanilla natively reads the settings copy. The 2026-09-02 fix only wrote to the board copy. Now `TuneSettingsObject` additionally clones the front curve to the rear curve of the settings object (`board._settings` + `CurentSettings`) → flat jump like standard boards (in-game verified 2026-09-03).
- Note: ollie behavior is vanilla design for ALL boards — the curve smoothing is an intentional custom-board style, not a bugfix on the game.

## 1.0.2 (2026-08-20)

- **Bugfix round (20 issues, 6 HIGH / 9 MEDIUM / 5 LOW)**:
  - **HIGH**: Scene-callback symmetry (`OnSceneWasUnloaded` now uses `IsGameplayScene` guard like `OnSceneWasLoaded`).
  - **HIGH**: Per-renderer material allocation fixed (`r.material` → `r.sharedMaterial`) preventing material leaks on zero-slot renderers.
  - **HIGH**: `AnimationCurve` allocations cached as `static readonly` instead of per-board re-creation.
  - **HIGH**: `Gradient` allocated on every mount → cached as `static readonly`.
  - **HIGH**: `InitializeAssets` re-init no-op fixed: base template now found BEFORE first init.
  - **HIGH**: Candidate base ID chain now logged with override support (`BaseItemIdOverride` config) for future-proofing.
  - **MEDIUM**: TOCTOU in `Registry.ItemExists`+`GetItem` collapsed into single `GetItem` call.
  - **MEDIUM**: Custom mesh filter match tightened from `Contains("board")` to exact `"deck"`/`"board"`.
  - **MEDIUM**: Underglow light explicitly sets `shadows=None` and `renderMode=ForcePixel`.
  - **MEDIUM**: `EnableKeyword("_EMISSION")` only when `_EmissionColor` property exists.
  - **MEDIUM**: `DumpSkateboardStats` log gated by `logStats` parameter — weather-change re-tunes no longer spam logs.
  - **MEDIUM**: Misleading jump-duration comment corrected.
  - **LOW**: `SkateboardConfig.Default()` now used in `_fallbackConfig`.
  - **LOW**: Dialogue option duplicate-check uses `OrdinalIgnoreCase`.
  - **LOW**: `BoardLeanRate` added to `SkateboardConfig` (was hardcoded 60f).
  - **LOW**: Config init failure now logs `Error` instead of `Warn`.
  - **LOW**: `Mod.Log` lazy-initialized to defer MelonLogger init until needed.
- **New config**: `BaseItemIdOverride` (empty by default, falls back to candidate chain).
- **New config**: `BoardLeanRate` (default 60f).

## 1.0.1 (2026-08-16)

- **Architecture refactor (Patches)**:
  - Removed dead code: `CaptureVanillaDefaults()`, no-op `RestoreVanillaPhysics()`, no-op `ApplyPhysicsTuning()` (method + 3 internal call sites in `CreateAndRegister`). Eliminated confusing placeholder methods.
  - Added universal custom-board filter: `SkateboardItemFactory.IsCustomItem(ItemInstance?)` and `IsCustomSkateboard(Skateboard)`. All 6 inline `Definition.ID == SkateboardId` checks now use the helper (single source of truth for "is this the custom board?").
  - Local `IsCustomSkateboard` helper in `SkateboardVisualPatches` deleted — moved to factory for unification.
- **Architecture refactor (Idempotent tuning)**:
  - Added `_tunedBoards: HashSet<IntPtr>` in `SkateboardItemFactory` to prevent double-tuning. Previously, both `OnSkateboardAwakePostfix` and `OnMountPostfix` could call `TuneSkateboard`, allocating ~2 `AnimationCurve` per call. Now the second call short-circuits.
  - `ClearTuningState()` is called on every `OnSceneWasLoaded` to allow re-tuning across scene transitions.
- **Architecture refactor (Save-recovery logging)**:
  - `OnSaveInfoLoaded` now logs the registration outcome: `[SaveRecovery] Custom board '...' ready` or `[SaveRecovery] Custom board registration deferred — base board prefab not in Registry yet.` Operators no longer need to grep `MelonLoader\Latest.log` blindly.
- **Architecture refactor (Default factory)**:
  - Added `SkateboardConfig.Default()` static factory as a canonical entry point for defaults (single source of truth for tests / programmatic resets).

## 0.1.2 (2026-08-14)
- **Top speed boosted**: Increased default top speed to 80 km/h (was 55 km/h) and push force multiplier to x2.8 for rapid acceleration.
- **Anti-gravel & terrain slowdown removal**: Fully eliminated gravel, grass, and dirt slowdown (`SlowOnTerrain = false`, `GetSurfaceSmoothness = 1.0f`, `IsOnTerrain = false`).
- **Enhanced air time & jumping**: Increased jump force multiplier to x2.2 and turn force to x1.8 for sharper carving.
- **Reduced friction glide**: Reduced longitudinal friction multiplier to 0.35 for longer coasting distance after pushes.

## 0.1.1 (2026-08-14)
- **Material caching & memory-leak fix**: Materials are now properly cached in `CyberSkateboardVisualizer` instead of being reallocated on every equip/mount.
- **Shader safe fallback**: Added multi-tier fallback lookup (`FindSafeShader`) preventing NRE crashes when `Standard` or URP shaders are stripped/missing in IL2CPP.
- **PatchGuard integration**: Replaced raw Harmony attributes with resilient `PatchGuard.TryPatch` calls from `S1Mods.Shared`.
- **Lifecycle reliability**: Added `GameLifecycle.OnSaveInfoLoaded` subscription and proper unsubscription in `OnDeinitializeMelon`.
- **Underglow light dedup**: Prevents duplicate point lights when repeatedly equipping or remounting boards.
- **ObjLoader triangulation & custom deck hook**: Corrected CCW winding order `(0, i-1, i)` for front-facing polygon normals and added `TryGetOrLoadDeckMesh()` hook.
- **Documentation & logging**: Unified logging via `ModLogger` and aligned `README.md` physics specifications with config defaults.

## 0.1.0 (2026-08-13)
- Initial version.