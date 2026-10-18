# GoldenHour

Golden-hour look bias for Schedule I — graphics-overhaul slice 2 ([plan](../../../docs/graphics-golden-hour-plan.md)).

## What it does

Ctrl+Shift+F7 toggles a client-side warm bias. The bias is full around 06:00 and 18:00 and released at noon and at night, so a noon session stays the game's own light.

| Key | Target | Bias tint |
|---|---|---|
| `sky.sunShine` | SkyMat `_SunShine` HDR glow | (1.00, 0.72, 0.42) — strongest gold |
| `sun.color` | `RenderSettings.sun` color | (1.15, 0.97, 0.72) |
| `ambient.*` | Trilight sky/equator/ground | (1.08, 0.98, 0.82) — gentlest |
| `fog.color` | `RenderSettings.fogColor` | (1.22, 0.95, 0.62) — amber haze |
| `volume.colorFilter` | GlobalVolume `ColorAdjustments.colorFilter` | (1.10, 0.96, 0.76) |

**Design rule:** every write is a *multiplicative* bias (`original × mix(white, tint, strength)`) — the game's day/night cycle keeps control of magnitude, the mod only shifts hue. Night stays readable; no absolute overrides anywhere.

## Safety

- Original values are snapshotted (first-wins) on the first apply of an activation.
- Restore happens on toggle-off, Main-scene exit and game shutdown.
- Fail-soft: unreadable parameters are skipped and reported; an unreadable clock releases the bias instead of painting gold. Errors disable the feature and restore what was recorded.
- While active, the bias reapplies every frame in `OnLateUpdate` (the game rewrites sun color continuously). The bounded cadence remains only if `ApplyEveryLateUpdate` is turned off.
- Volume writes go into the volume's runtime profile instance (`volume.profile`), never the
  shared asset, and force `active`/`colorFilter.overrideState` first — a colorFilter write
  with either flag off is a silent no-op. Both flags are restored on the way out. The profile
  instance is only used when the game itself instantiated it (`Volume.HasInstantiatedProfile()`);
  otherwise the volume key is skipped so the game's own day/night grading keeps driving it.
- Every activation logs a `DIAG` dump (camera post-processing, volume layer, whether the
  game instantiated its profile, volume flags, sky `_SunShine` with `HasProperty` check,
  `ambientMode`, `fog`, sun color) to `MelonLoader/Latest.log`.

## Config

MelonPreferences via S1Mods.Shared ModConfig (global `UserData/MelonPreferences.cfg`, category `GoldenHour`): `Strength` (0–1, default 1.0, then scaled by the in-game clock), `ReapplyIntervalSeconds` (default 1.0, minimum 0.5, used only when per-frame apply is off), `ApplyEveryLateUpdate` (default `true`), `DiagnosticStrongTint` (default `false` — ignores the clock).

## Status

v0.1.5 experimental, In-Game-Verify open. The warm bias is weighted by the in-game clock (full at 06:00 and 18:00, zero at noon and at night). 82 unit tests. The 0.1.4 in-game sun-color measurement is historical and does not cover this clock. Fog is not enabled.
