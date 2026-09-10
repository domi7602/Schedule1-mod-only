# Changelog


## 1.1.0 (2026-09-09)

- **Tuning: Höhere Geschwindigkeit + bessere Lenkung** (Benutzerwunsch; Werte gelten für Code-Defaults UND Live-Config `UserData/MelonPreferences.cfg [CustomSkateboard]`, da gespeicherte TOML-Werte Code-Defaults überschreiben):
  - Geschwindigkeit: `TopSpeed_Kmh` 100 → **140**, `PushForceMultiplier` 5.2 → **6.5**, `PushCooldown` 0.22 s → **0.18 s**, `LongitudinalFrictionMultiplier` 0.16 → **0.13** (weniger Rollwiderstand, Speed hält länger).
  - Lenkung: `TurnForce` 15 → **20**, `TurnChangeRate` 64 → **85**, `TurnReturnToRestRate` 56 → **75**, `LateralFrictionForceMultiplier` 1.60 → **1.85** (Highspeed-Grip), `MaxBoardLean` 28° → **33°**, `BoardLeanRate` 60 → **75**.
  - Kurven-Abdeckung geprüft: Turn-/Push-Curves sind bis 160/150 km/h definiert — 140 km/h bleibt voll innerhalb der Keyframes.
  - Unverändert: Sprungphysik (JumpForce 18), Anti-Gravel, Preis, Visuals.

## 1.0.3 (2026-09-03)
- **Fix: Ollie-Flattening (flacher Sprung)**: Custom-Board-Sprünge hoben zuerst die Nase (Vanilla-Ollie-Animation). Root Cause: Die Achsen-Sprungkurven (`FrontAxleJumpCurve`/`RearAxleJumpCurve`) existieren doppelt — auf der Board-Instanz UND im `SkateboardSettings`-ScriptableObject; Vanilla liest nativ die Settings-Kopie. Der Fix vom 02.09. schrieb nur die Board-Kopie. Jetzt klont `TuneSettingsObject` die Front-Kurve zusätzlich auf die Rear-Kurve des Settings-Objekts (`board._settings` + `CurentSettings`) → flacher Sprung wie Standard-Boards (in-game verifiziert 2026-09-03).
- Hinweis: Ollie-Verhalten ist Vanilla-Design für ALLE Boards — die Kurven-Glättung ist ein bewusster Custom-Board-Stil, kein Bugfix am Spiel.

## 1.0.2 (2026-08-20)

- **Bugfix Round (20 issues, 6 HIGH / 9 MEDIUM / 5 LOW)**:
  - **HIGH**: Scene-callback symmetry (`OnSceneWasUnloaded` now uses `IsGameplayScene` guard like `OnSceneWasLoaded`).
  - **HIGH**: Per-renderer Material allocation fixed (`r.material` → `r.sharedMaterial`) preventing material leaks on zero-slot renderers.
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

- **Architecture Refactor (Patches)**:
  - Removed dead code: `CaptureVanillaDefaults()`, no-op `RestoreVanillaPhysics()`, no-op `ApplyPhysicsTuning()` (method + 3 internal call sites in `CreateAndRegister`). Eliminated confusing placeholder methods.
  - Added universal custom-board filter: `SkateboardItemFactory.IsCustomItem(ItemInstance?)` and `IsCustomSkateboard(Skateboard)`. All 6 inline `Definition.ID == SkateboardId` checks now use the helper (single source of truth for "is this the custom board?").
  - Local `IsCustomSkateboard` helper in `SkateboardVisualPatches` deleted — moved to factory for unification.
- **Architecture Refactor (Idempotent Tuning)**:
  - Added `_tunedBoards: HashSet<IntPtr>` in `SkateboardItemFactory` to prevent double-tuning. Previously, both `OnSkateboardAwakePostfix` and `OnMountPostfix` could call `TuneSkateboard`, allocating ~2 `AnimationCurve` per call. Now the second call short-circuits.
  - `ClearTuningState()` is called on every `OnSceneWasLoaded` to allow re-tuning across scene transitions.
- **Architecture Refactor (Save-Recovery Logging)**:
  - `OnSaveInfoLoaded` now logs the registration outcome: `[SaveRecovery] Custom board '...' ready` or `[SaveRecovery] Custom board registration deferred — base board prefab not in Registry yet.` Operators no longer need to grep `MelonLoader\Latest.log` blindly.
- **Architecture Refactor (Default Factory)**:
  - Added `SkateboardConfig.Default()` static factory as a canonical entry point for defaults (single source of truth for tests / programmatic resets).

## 0.1.2 (2026-08-14)
- **Top Speed Boosted**: Increased default top speed to 80 km/h (was 55 km/h) and push force multiplier to x2.8 for rapid acceleration.
- **Anti-Gravel & Terrain Slowdown Removal**: Fully eliminated gravel, grass, and dirt slowdown (`SlowOnTerrain = false`, `GetSurfaceSmoothness = 1.0f`, `IsOnTerrain = false`).
- **Enhanced Air Time & Jumping**: Increased jump force multiplier to x2.2 and turn force to x1.8 for sharper carving.
- **Reduced Friction Glide**: Reduced longitudinal friction multiplier to 0.35 for longer coasting distance after pushes.

## 0.1.1 (2026-08-14)
- **Material Caching & Memory-Leak Fix**: Materials are now properly cached in `CyberSkateboardVisualizer` instead of being reallocated on every equip/mount.
- **Shader Safe Fallback**: Added multi-tier fallback lookup (`FindSafeShader`) preventing NRE crashes when `Standard` or URP shaders are stripped/missing in IL2CPP.
- **PatchGuard Integration**: Replaced raw Harmony attributes with resilient `PatchGuard.TryPatch` calls from `S1Mods.Shared`.
- **Lifecycle Reliability**: Added `GameLifecycle.OnSaveInfoLoaded` subscription and proper unsubscription in `OnDeinitializeMelon`.
- **Underglow Light Dedup**: Prevents duplicate point lights when repeatedly equipping or remounting boards.
- **ObjLoader Triangulation & Custom Deck Hook**: Corrected CCW winding order `(0, i-1, i)` for front-facing polygon normals and added `TryGetOrLoadDeckMesh()` hook.
- **Documentation & Logging**: Unified logging via `ModLogger` and aligned `README.md` physics specifications with config defaults.

## 0.1.0 (2026-08-13)
- Initiale Version.
