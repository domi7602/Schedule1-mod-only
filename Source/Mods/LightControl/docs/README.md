# LightControl

Grow-lamp **LED panel + emitted-light** control from the in-game phone, for Schedule I. Color and
brightness drive the lamp's own emitting LED panel *and* the light that lamp emits, always from the one
resolved color, so the panel and the light it casts match. The pure model is game-independent; the
Unity/MelonLoader adapter is thin, guarded and only writes values it can prove and restore.

Source version `0.1.0-dev.5` — **first in-game test slice**, now coupled panel + emitted-light output.
Reviewed and tested; deployment is blocked while the game is running. The last installed build remains
`0.1.0-dev.4` (surface only). No combined-output gameplay verification yet. Accidental solution deployment
remains disabled by default.

## What is here

| Path | Content |
|---|---|
| `src/LightControl.csproj` | The single project. Builds `LightControl.dll` (`S1NoDeploy=true` by default). |
| `src/Mod.cs` | The one MelonLoader entry point: lifecycle wiring and a guarded callback for every hook. |
| `src/LightRuntime.cs` | Host runtime: cooperative frame-budgeted discovery, the single coupled visual phase (LED panel + the lamp's own native light sources), ownership gate, unload/rebind and teardown/restore of every owned material clone and native light output. |
| `src/LightPersistence.cs` | Slot-scoped atomic storage with a read-failure write gate. |
| `src/LightControlApp.cs` | The phone app (vertical, dark palette, fixed controls). |
| `src/Core/` | Game-independent model: state, identity, protocol, effects, JSON persistence and the runtime-used policies. No Unity, MelonLoader, S1API or Steam dependency. |
| `src/Diagnostics/DiagnosticMod.cs.txt` | Archived, **not compiled**: the previous read-only diagnostics helper. |
| `assets/` | Eight language files (42 keys each) and `config.defaults.json`. Not yet bound to any UI or runtime configuration. |
| `docs/upstream/` | Archived legacy build configs, scripts, packaging and documentation from the imported archive. Reference only, not active. |

The five cohesive native-light policy files are consolidated into one compilation unit
(`src/Core/NativePolicies.cs`): `NativeLightPolicy`, `NativeColorPolicy`,
`NativeCouplingPolicy`, `NativeLightBinding` and `NativeMaterialIdentity` (with their
step/state types). Each original keeps its own block namespace and using directives, so the
types and their binding are unchanged; the file carries a machine-readable section marker per
original so an independent reconstruction proof can recover each source byte-for-byte.

## Scope of this test slice

Implemented and compiling against the real game assemblies:

- Placed, initialized **grow lamps** with a nonempty native GUID are discovered by a **cooperative
  bounded traversal**: owned-property roots plus a rotating snapshot of the scene roots are queued
  once, then at most 64 nodes and 2 ms are processed per frame up to a hard node cap, with a single
  `GetComponent<GrowLight>()` per visited node and no recursive whole-subtree query. New placements
  arrive through S1API `BuildEvents`; a fresh bounded walk starts at most every 20 s. The scan logs
  its real completion count, elapsed work and truncation status and never claims a synchronous
  complete scan.
- **Ownership gate**: a lamp is only registered, commanded or driven while its live parent property is
  one the player owns (live `IsOwned`, or the code appearing in the native owned-property list).
  Ownership is rechecked before every command and at most once per second, and a lamp whose property
  is no longer owned loses its visual influence immediately.
- **A missing reference is never a deletion**: a lamp whose renderer, native light or parent property is
  temporarily unavailable is marked *temporarily unloaded* — removed from the active list while keeping
  its state — and rebound to the new native outputs of the same GUID when it reappears. Only a live
  `IsDestroyed` counts as permanent removal. Native surface renderer pointers and native light pointers
  are both de-duplicated, so one lamp surface or one Unity light is never owned twice.
- **Coupled output: one color, two writes.** For every lamp the pure model resolves a single
  `VisualInstruction`, and that exact instruction drives both:
  1. the lamp's own emitting LED panel — an owned clone of the native LED material installed into the
     exact `ToggleableLight.MaterialIndex` slot of `ToggleableLight.lightSurfacesMeshes`, with its HDR
     `_EmissionColor` set; and
  2. the lamp's own native light sources — `ToggleableLight.lightSources[i]._Light`, where **only**
     `color` and `intensity` are ever written.

  So the panel and the light it emits always show the same chosen color. **Emitted intensity is
  relative**: the chosen factor scales that source's own freshly sampled native intensity, never the
  panel's HDR magnitude, so it cannot compound and a native intensity of `0` stays `0`. **Emitted color
  is absolute**: the chosen hue replaces the native color, because a color factor would be meaningless.
  Which material the panel slot holds is decided by the **nonzero native pointer**
  (`NativeMaterialIdentity`), never by managed reference equality or a material name: IL2CPP hands out a
  fresh managed wrapper per read, so `ReferenceEquals` alone misread our own clone as a foreign material
  and stopped applying colours after the first one.
- **A real light colours what it illuminates.** Because the lamp's own Unity light is now driven, the
  plants that a lit lamp shines on receive *colored illumination*. That is the requested behaviour and
  it changes no game state: no plant or world material is accessed, cloned or mutated, and no shader,
  shader keyword or global lighting property is touched. Plant materials themselves are never modified.
- **Only the verified LEDLight panel family is touched.** Both the native ON (`ledlight_light_on`) and
  OFF (`ledlight_light_off`) materials were read from the shipped `sharedassets0.assets` and both carry
  an HDR `_EmissionColor` (off: `2.505, 0.1749, 12.358` — the violet panel). A lamp whose native
  `ToggleableLight` does not carry exactly that pair (FullSpectrum, halogen) is left completely
  untouched and reported once, instead of guessing at an unverified material.
- **Nothing else about a native light is changed.** No transform, position, rotation or parenting; no
  range, cone/spot angle, cookie or render mode; no shadows, culling mask or `enabled` flag; and never
  the lamp's switch state. `ToggleableLight`'s own writer is handed the value back in the update phase
  before it runs, so our output can never be re-scaled by us or compound, and the single visual write is
  the late-update phase.
- **Native switch state wins.** A switched-off lamp is never recoloured and never emits our
  illumination: its native light output is handed back to its own baseline, and the mod never switches a
  lamp on, off or otherwise forces its state. An unsupported/foreign panel slot is never replaced and an
  empty slot is never created. The native on/off material swap always beats the mod, and both outputs
  always scale a freshly sampled native baseline.
- **Coupled mode fails closed.** A lamp is only registered once *both* outputs are resolvable — a
  verified emitting panel slot and at least one readable own native light source. The shipped
  `LEDLight` prefab's `_Light` reference is null in the asset and resolves at runtime, so a lamp that is
  off may legitimately expose no source on that frame: it is retained as **pending** and retried at a
  bounded cadence (never per frame), and it is **never** resolved by switching the lamp on. A lamp that
  is on but still exposes no readable source is reported as **unsupported** and left completely
  untouched — the mod never pretends to control both outputs when only the panel is available, and the
  phone app shows that count.
- **Original** restores the *latest* sampled native baseline of both outputs (never a stale activation
  snapshot); at 100% it removes every owned influence, the panel slot is handed back to the game's own
  material and the native light color and intensity are handed back to their own values.
- **Ownership is only claimed for a write that happened.** Each of the two native properties is released
  on its own when its setter throws, so a failure on one property never blocks the other, the rest of the
  lamp or another lamp's cleanup. A native value that no longer equals our own last write is treated as
  an external change and preserved — both in the update-phase undo and on teardown.
- **Storage** is slot-scoped (`SaveSlots.GetActiveSlotNumber()`), written atomically through
  `SafeStorage.SaveTextAtomic` and strictly gated: a read that cannot produce text blocks writes
  instead of clobbering the file, and a write requires the captured slot to still equal the game's
  active slot (including the no-active-slot `-1` case, which never authorizes a write). The saved
  revision advances only after a successful write, so a failed I/O is retried at the next bounded
  autosave rather than silently skipped.
- Teardown restores every owned panel slot and native light output and destroys every owned material
  clone on pre-load, pre-scene-change, scene-unload, quit and mod deinitialization paths.
  `OnDeinitializeMelon` is symmetrical with `OnInitializeMelon` (release subscriptions, then restore and
  drop the session) behind a one-shot gate, so a repeated shutdown cannot write again.
- The phone app reports a failed action instead of refreshing as success, and releases the exact wired
  `UnityEvent`/handler pair on destroy without a Unity fake-null liveness gate.

Deliberately **not** in this slice (see `verification.md` for the honest limitation list):

- Only the **verified LEDLight panel family** is supported. FullSpectrum and halogen grow lamps are
  discovered but left completely untouched until their native panel materials are verified the same
  way, because an unverifiable material clone could visibly break the lamp.
- No group/room/property selection, no disco effects, no configuration or language binding.
- **Host/singleplayer only.** A multiplayer client is detected and stays read-only; it shows an
  unsupported/read-only status and writes nothing.

## Build and test (no deploy)

```text
# Pure model console checks (game-free; links the Core sources):
dotnet run -c Release --project Source/Tests/LightControl.Tests/LightControl.Tests.csproj

# Compile-only mod gate (S1NoDeploy is the project default):
dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false
```

Deploy is off by default (`S1NoDeploy=true` is set in the project). A build with
`-p:S1NoDeploy=false` installs `LightControl.dll` and is intended only when explicitly authorized by
the owner.

## Documentation

- `docs/CHANGELOG.md` — import, adaptation and test-slice record.
- `docs/mod.json` — machine-readable status.
- `docs/verification.md` — build, test and limitation evidence.
- `docs/implementation-plan.md` — the original staged plan; later stages are still open.
- `docs/upstream/` — the original archive's build/packaging tooling, old project files and reports,
  kept as history.
