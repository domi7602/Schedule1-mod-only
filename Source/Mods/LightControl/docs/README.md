# LightControl

Grow-lamp **LED panel + emitted-light** control from the in-game phone, for Schedule I. Color and
brightness drive the lamp's own emitting LED panel *and* the light that lamp emits, always from the one
resolved color, so the panel and the light it casts match. The pure model is game-independent; the
Unity/MelonLoader adapter is thin, guarded and only writes values it can prove and restore.

## Graphite native UI — 0.1.0-dev.19

The exact checked Release build was **installed on 2026-10-22**. DLL/PDB/metadata and the three
previous dev.18 files were hash-verified; the backup and receipt are recorded below.

The approved [interactive concept](ui-mockup.html) is ported to the native phone: a 1201 x 655
owned reference canvas, graphite table, large lamp labels, explicit color controls, brightness
meters and ellipsis actions. The color chooser is a **5 x 2** grid with **Rainbow at bottom-right**,
staged **Cancel / Apply**, while all seven brightness steps apply immediately. Real properties,
lamp data and storage/scan notices replace the mockup fixtures.

The view attempts Segoe UI with an Arial fallback; the owner's 2026-10-22 session confirms that a
missing downstream font constructor prevents Segoe UI creation, so the shown native view uses Arial.
It retains cached owned glyph assets and a sticky column heading. Unique forwarding bindings keep every Cancel/Close button independently registered
with S1API and release callback targets during teardown. The existing 19 C# files plus project
remain the production source budget.

Parent verification: **264/264 checks**, forced Release **0 warnings / 0 errors**, scoped formatting
clean and final independent delta review **PASS**. Owner screenshots subsequently establish the
three-lamp list and open chooser rendering, including visible Cancel/Apply and Rainbow tenth.
Exact font parity, click routing and wider gameplay/lifecycle acceptance are owner-accepted
2026-10-24 (not itemized; the confirmation had been missing from the record). Current
evidence: [graphite-ui-verification.md](graphite-ui-verification.md).

## Historical installed V4 professional list — dev.18

The exact retained `0.1.0-dev.18` DLL/PDB/metadata were installed on 2026-10-22 after owner
approval, without rebuilding. All three installed files and previous-file backups were hash-verified;
the game was closed. Receipt and backup paths are in
[phone-color-picker-deployment.md](phone-color-picker-deployment.md). Native UI acceptance remains open.

`0.1.0-dev.18` reworks the list presentation from the owner's in-game screenshots and the accepted
V4 preview: rows are one horizontal layout with a fixed **18 dp square** real-color marker, the lamp
name as the primary label, the color name below it, and right-aligned **Brightness**/**Actions**
columns shared with a single column-heading row. Selection is a restrained background tint instead of
a text `>` prefix, property groups are plain headings, and the standing instruction paragraph became
one compact action-feedback line. The color chooser keeps nine staged true-color tiles with one white
ring and an empty center note.

The reported hint-only footer is addressed structurally: **Back**, **Select** and the single Close
button now declare their own preferred and minimum height, so the child-controlled footer group can
no longer collapse them. A one-shot `[ui-layout]` line after each modal opens records the measured
card and button rects, so the next session can separate a real layout failure from a rendering
misread. 253 core/adapter/source checks, forced Release 0 warnings/errors, scoped format clean, and an
independent review whose one blocking finding (a duplicate column-heading row) was fixed and
re-verified. These checks describe the retained build; they were not rerun during installation.
Native fit/hit testing/rendering remain open. Details:
[phone-color-picker-verification.md](phone-color-picker-verification.md).

## Historical compact picker baseline (dev.17)

The exact retained `0.1.0-dev.17` DLL/PDB/metadata were installed on 2026-10-21 after separate owner
approval, without rebuilding. All three installed hashes and prior-file backups were read back;
76 unrelated files, four LightControl sidecars and 125 native save files were unchanged. The game
was not started. Receipt, backup paths and the installed identity are in
[phone-color-picker-deployment.md](phone-color-picker-deployment.md). Frozen build metadata retains
its original compile-only label; that creation-state label is superseded by the deployment record.

That installed build introduced the full-width property-grouped list and the compact color-only
chooser. In-game observation confirmed both rendered and that the picker stages a color, but the
Back/Select footer did not render, which is exactly what dev.18 corrects.

The current source replaces the permanent selected-lamp editor with a full-width property-grouped
list and a compact color-only chooser. A lamp's main/color area opens the nine true-color tiles;
tile clicks only stage a choice, **Select** applies it to the captured session/lamp, and **Back**,
outside dismissal or phone closure discards it. Original/Rainbow sources never become White merely
by opening. The separate seven-step intensity menu retains its immediate click-to-apply behavior.
Original, Rainbow, Reset and counted property/world copy remain under **More**, outside the chooser.

Tests, actual review outcomes, retained artifacts and the singleplayer acceptance checklist are in
[phone-color-picker-verification.md](phone-color-picker-verification.md). The accepted visual/behavioral
contract is [phone-color-picker-design.md](phone-color-picker-design.md). The earlier implementation
slice was compile-only; the subsequent approved installation used that exact frozen artifact.
No game launch, save mutation, Shared/native-light/persistence change or commit was performed.
Native layout, hit testing and rendering still need the owner's in-game check.

## Historical dev.16 audit baseline

The exact retained `0.1.0-dev.16` DLL/PDB/metadata were installed after separate owner approval on
2026-10-21, without rebuilding. Destination hashes and receipt were read back; the backed-up **Tests**
save, LightControl persistence and all 76 unrelated mod/library/plugin witness files were unchanged.
The game was not started. Runtime loading, rendering, material destruction and actual PhoneApp fit
remain unverified. Prior dev.15 text-clipping acceptance is historical, not acceptance of this build.

The complete backup, receipt and installed hashes are in [audit-repair-verification.md](audit-repair-verification.md).
Frozen build metadata remains byte-identical; its original compile-only label is superseded by the
deployment record. The earlier unintended Shared receipt write is retained separately in that record.

Prior dev.16 scope: landscape property-grouped list and selected-lamp editor; Original/Color/Rainbow;
nine real-color swatches and seven intensity steps (10/25/50/75/100/150/200%). Original preserves
brightness; Reset selects Original and 100%. Property/global copy targets currently available lamps;
counts include the source. A stale preview must not partially apply. Clients remain read-only.

See [audit-repair-verification.md](audit-repair-verification.md) for baseline, per-finding outcomes,
source/artifact identity, tests and remaining native/container acceptance. The managed measurements in
[resolver-microbenchmark.csv](resolver-microbenchmark.csv) are not Unity or FPS measurements.

Numeric slot identity does not distinguish a new world or another player profile using the same slot
and mod path. The schema is unchanged; no world-ID migration is invented. Shared remains unchanged.

## What is here

| Path | Content |
|---|---|
| `src/LightControl.csproj` | The single project. Builds `LightControl.dll` (`S1NoDeploy=true` by default). |
| `src/Mod.cs` | The one MelonLoader entry point: lifecycle wiring and a guarded callback for every hook. |
| `src/LightRuntime.cs` | Host runtime: cooperative frame-budgeted discovery, the single coupled visual phase (LED panel + the lamp's own native light sources), ownership gate, unload/rebind and teardown/restore of every owned material clone and native light output. |
| `src/LightPersistence.cs` | Slot-scoped atomic storage with a read-failure write gate. |
| `src/LightControlApp.cs` | Full-width landscape list, dedicated staged color chooser, immediate seven-step intensity menu and separate More actions; English dark UI. |
| `src/` (remaining policy files) | Game-independent state, identity, protocol, JSON, visual and native policies. 19 C# files plus project total; no new assembly or dependency. |
| `assets/` | Eight language files (42 keys each) and `config.defaults.json`. Not yet bound to any UI or runtime configuration. |
| [upstream archive](upstream/) | Archived legacy build configs, scripts, packaging and documentation from the imported archive. Reference only, not active. |

The five cohesive native-light policy files are consolidated into one compilation unit
(`src/NativePolicies.cs`): `NativeLightPolicy`, `NativeColorPolicy`,
`NativeCouplingPolicy`, `NativeLightBinding` and `NativeMaterialIdentity` (with their
step/state types). Each original keeps its own block namespace and using directives, so the
types and their binding are unchanged; the file carries a machine-readable section marker per
original so an independent reconstruction proof can recover each source byte-for-byte.

## Historical imported test-slice design (dev.5, not current acceptance)

The following design notes describe the older imported implementation. Its unconditional restore,
resource-release and no-compounding claims are specifically under repair in the audit above. They
must not be used as evidence for the candidate. Halogen support now includes the grow-tent pair;
property copy, landscape UI and later fixes supersede the old exclusions below.

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
dotnet run -c Release --project Source/Tests/LightControl.Tests/LightControl.Tests.csproj -p:S1NoDeploy=true -p:BuildProjectReferences=false

# Compile-only mod gate (S1NoDeploy is the project default):
dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false
```

Deploy is off by default (`S1NoDeploy=true` is set in the project). A build with
`-p:S1NoDeploy=false` installs `LightControl.dll` and is intended only when explicitly authorized by
the owner.

## Documentation

- [Interactive UI mockup](ui-mockup.html) — standalone, offline Smart-Home-style design concept at the
  1201 x 655 phone reference size. Open the HTML file in a browser to try color selection, brightness
  and lamp actions with in-memory demo data. This is a browser preview, not an in-game screenshot
  or native UI acceptance evidence.
- [CHANGELOG.md](CHANGELOG.md) — import, adaptation and test-slice record.
- [mod.json](mod.json) — machine-readable metadata.
- [verification.md](verification.md) — dated build, test and limitation evidence.
- [implementation-plan.md](implementation-plan.md) — original staged plan; current owner acceptance and limits are in the [graphite record](graphite-ui-verification.md).
- [upstream archive](upstream/) — the original archive's build/packaging tooling, old project files and reports,
  kept as history.
