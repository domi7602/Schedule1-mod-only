# Changelog

## 0.1.0 (2026-09-25) — Stage 1 spike + Stage 2 visual swap

- **Initial version.** Dev tool that proves the three primitives of the future
  taxi mod on Schedule I 0.4.7f6 (IL2CPP, MelonLoader):
  (a) autonomous A→B driving via `VehicleAgent.Navigate`,
  (b) an NPC boarding the spike vehicle (`NPC.EnterVehicle(null, veh)` with a
  `LandVehicle.AddNPCOccupant(npc)` fallback), and
  (c) local player in/out via `LandVehicle.EnterVehicle()` /
  `LandVehicle.ExitVehicle()`.
- **Console command `taxi`** (S1API `BaseConsoleCommand`, auto-discovered):
  `help`, `codes`, `spawn [code]`, `npc`, `ride`, `out`, `go [distance=40]`,
  `stop`, `status`, `cleanup`. A leading `/` resolves to the same handler.
- **`taxi codes`** enumerates `VehicleManager.Instance.VehiclePrefabs`
  (`VehicleCode` / `VehicleName` / `VehiclePrice`) — the vehicle code catalog
  used by later stages.
- **`taxi spawn`** places the vehicle 6 m in front of the player with a downward
  `Physics.Raycast` ground snap, **`playerOwned=true`** (required so
  `ShouldBePhysicallySimulated` passes and the vehicle can drive), guarded by
  `InstanceFinder.IsServer` and `VehicleManager.GetVehiclePrefab(code)`.
- **`taxi npc`** selects the nearest living NPC from `NPCManager.NPCRegistry`
  (skips dead and already-seated NPCs) and dumps the full seat state
  (`LandVehicle.Seats` → `gameObject.name`, `isDriverSeat`, `isOccupied`,
  `OccupantNPCs`, `GetFirstFreeSeat()`).
- **`taxi go`** builds
  `NavigationSettings { endAtRoad, ensureProximityToGraph, teleportToGraphIfCalculationFails }`,
  releases the vehicle first (physical simulation, `ExitPark`, brakes, agent),
  snaps the target to the vehicle graph (25 m cut-off), turns the vehicle toward
  the target and calls `agent.Navigate(target, settings, callback)` through the
  interop's implicit `NavigationCallback` operator (a rejected callback dispatch
  falls back to `callback = null`); `taxi go2` is the same dispatch with
  `settings = null`.
- **Navigation polling** in `SpikeRunner.Update` (hooked idempotently into
  `MelonEvents.OnUpdate`): 0.5 s heartbeat with `AutoDriving`, target/actual
  distance, `IsOnVehicleGraph()`, `GetIsStuck()`, `Speed_Kmh` and the
  `ENavigationResult` callback value; final log when `AutoDriving` turns false
  (final vs. start distance + duration, ARRIVED vs. STOPPED SHORT) and a warning
  plus `StopNavigating()` after a 45 s timeout that also covers a path calculation
  which never finishes. Per-frame exceptions are logged once per episode and the
  report re-arms after a clean tick.
- **Console is log-only (no input field):** every `taxi` command above is
  output-only — the MelonLoader window cannot be typed into, so the spike is
  controlled with hotkeys (ignored while `S1API.Input.Controls.IsTyping` is true):

  | Key | What it runs |
  | --- | --- |
  | **F6** | full run: `spawn → +1 s npc → +1 s go 40` (blind `forward * 40`) |
  | **F7** | `taxi probe` |
  | **F8** | `taxi trace` on/off toggle (four Harmony prefixes) |
  | **F9** | `taxi ride` / `taxi out` toggle |
  | **F10** | `taxi go2` — `Navigate` with `settings=null` |
  | **F11** | full run without the npc step (occupant hypothesis) |
  | **F12** | `Navigate` on a vanilla (not spawned) vehicle — vehicle-vs-caller A/B |
  | **F13** | `taxi go` to the proven road target `(-131.4, -4.0, 51.9)` |
  | **F14** | `taxi go` to the second proven road target `(-17.1, 0.0, 13.4)` |
  | **F15** | full run (spawn → npc) to the road target `(-131.4, -4.0, 51.9)` |

  While an automatic run or a deferred respawn is pending, the keys are ignored
  with a logged reason. F13–F15 need real key input (`SendInput`); the generic
  computer-use key path drops those virtual-key codes.
- **F6 run kernel** drives the sequence on a `Time.unscaledTime` step kernel with
  a per-step `try/catch` and abort messages that name the failing step.
- **Deploy:** `Directory.Build.targets` copies `TaxiDriver.dll` to
  `<GameDir>\Mods\` and `mod.json` + `TaxiDriver.pdb` to
  `<GameDir>\UserData\TaxiDriver\` on every Release build.
- **Documented deviations:** the deployed S1API 3.x assembly has no
  `[ConsoleCommand]` attribute, no `Aliases` property and no
  `ConsoleHelper.Print`; `NPC.WasCollected` and `NPC.CurrentVehicleSeat` do not
  exist in 0.4.7f6 (`CurrentVehicleSeat` lives on `Player`). Registration relies
  on S1API's `BaseConsoleCommand` auto-discovery (public parameterless
  constructor), answers go through the `[TaxiDriver]`-prefixed MelonLoader
  logger, dead NPCs are filtered with `NPC.Health.IsDead`, and the seat result
  is proven with the `LandVehicle.Seats` dump.
- **Code-review fix round (2026-09-25, same version):**
  - *Freeze guard (blocker):* `taxi spawn` never destroys and respawns in the same
    frame — the respawn is deferred by 0.5 s behind a verified cleanup
    (`NPC.ExitVehicle()` proven via `IsInVehicle`, `OccupantNPCs` emptied through
    `RemoveNPCOccupant`); `taxi cleanup` refuses `DestroyVehicle()` while NPC
    occupants remain and only clears the state after a successful destroy; `taxi npc`
    holds a 15 s re-enter cooldown for the last boarded NPC; F6 logs the start of
    step 2 with `Time.frameCount`.
  - *Retry measurement window:* the `settings = null` retry gets a 6 s grace window
    before the terminal branch may fire (it used to be cut off after 0.5 s).
  - *NPC seat proof:* boarding counts as proven only when `IsInVehicle`,
    `CurrentVehicle` and an `OccupantNPCs` slot agree — which also makes the
    `AddNPCOccupant` fallback reachable; the occupant array is logged as
    capacity vs. filled (the length is the seat count, not the occupancy); a
    one-line seat proof reports whether the driver seat is proven; NPCs with
    `Health == null` are skipped and no longer shown as alive.
  - *Spawn snap:* the ground ray starts above the spawn point (was: above the
    player), skips triggers and rigidbody holders, and logs the hit object + layer.
  - *Navigate callback:* `agent.Navigate(target, settings, callback)` through the
    interop implicit `NavigationCallback` operator (the documented "delegate-ctor
    pitfall" does not apply), with a `callback = null` fallback; polling state is
    written before the dispatch.
  - *Diagnostics:* new `taxi probe` (Flags/Seekers/graph-sample deltas/ownership for
    every vehicle) and `taxi trace on|off|status` (four Harmony prefixes via
    `PatchGuard`: `VehicleAgent.Navigate`, `NavigationUtility.CalculatePath`,
    `VehicleAgent.NavigationCalculationCallback`, `VehicleAgent.StopNavigating`).
  - *Minor:* typed `LandVehicle.GUID` instead of reflection, `State` logged by
    runtime type name, no blind `prefabs[0]`, teardown in `OnDeinitializeMelon`,
    stale-state clearing on scene load, `/taxi` alias registered next to `taxi`,
    help/docs corrected to a 0.5 s heartbeat (was 2 s).
- **Live verification (real-game session 2026-09-25):** after 13 test rounds the
  stage-1 spike is proven end to end — spawn + NPC + path + drive + arrival,
  `[nav] callback result=Complete after 20.2 s` for the F15 full run to the road
  target `(-131.4, -4.0, 51.9)`, no game freeze (freeze guard exercised).
- **Known limitations of the verified build:**
  - `driverSeat.isOccupied` stays `False` for an NPC driver — the NPC proof comes
    from `LandVehicle.OccupantNPCs` (plus `IsInVehicle` / `CurrentVehicle`).
  - End tolerance ≈ 8.3 m with `endAtRoad = true`; `ArrivalThresholdMeters` was
    raised 5 m → 10 m so the non-callback verdict does not report a complete run
    as STOPPED SHORT.
  - The MelonLoader console is log-only (no input) — all `taxi` commands are
    output-only; F6–F15 are the control surface.
  - F13–F15 only via real key input (`SendInput`); the computer-use key path drops
    those virtual-key codes.
- **Second fix round (2026-09-25, same version):** `SpikeState.AutoTarget` is now
  cleared on every reset (an F11 run no longer silently inherits F15's road target);
  the shared `TryBeginGo` gate makes F6/F10–F15 ignore keys while a run or a
  deferred respawn is pending (previously only F6/F11/F12/F15 were guarded);
  hotkeys are handled in ascending F6→F15 order; F8 toggles through
  `SpikeTrace.Logging` instead of a second private flag; `taxi status` prints
  `AutoTarget` + `SkipNpcStep`; `taxi help` and the init log document the log-only
  console and all ten hotkeys.
- Build verified: Release, 0 warnings / 0 errors.

### Visual model swap (Stage 2, same version 0.1.0, 2026-09-25)

- **The spawned vehicle now shows `taxi.glb` instead of the vanilla `Shitbox`
  visuals** — vanilla physics, `Main Collider`, `Roof Collider`, the four wheel
  objects with their wheel colliders, rigidbody and `VehicleAgent` are untouched
  (V1 principle: swap pixels only).
- **New file `Source/Mods/TaxiDriver/src/TaxiVisual.cs`:** `SwapAfterSpawn` runs at
  the end of `SpikeCommands.SpawnVehicle` (no extra hotkey) and
  - loads the GLB with `S1MAPI.Gltf.GltfLoader.LoadFromFile` from
    `UserLibs\S1MAPI_Il2cpp.dll` — resolver order `UserData\TaxiDriver\assets\taxi.glb`
    → `Mods\TaxiDriver\taxi.glb` (build deploy of `Source/Mods/TaxiDriver/assets/*.glb`)
    → `Mods\TaxiDriver\assets\taxi.glb`, first hit logged;
  - parents it under `LandVehicle.vehicleModel` (fallback: the vehicle `gameObject`)
    with `localPosition = 0` / identity / scale 1, forces its 21 renderers on and
    strips its colliders (they would become compound colliders of the vanilla
    rigidbody);
  - switches the vanilla visuals off by rule: children **with renderers and no
    colliders** get `SetActive(false)` (`Main_LOD0..2`, lights, indicators,
    `SmokeParticles`), children **with renderers and colliders** stay active and only
    lose renderer/`LODGroup` visibility (`TrunkLid`, `Wheel FL/FR/RR/RL`), children
    **without renderers** (`Main Collider`, `Roof Collider`, `TrunkGrid`,
    `TrunkInteraction`, `OwnedVehiclePoI`, `VehicleSound`) are untouched; nothing is
    destroyed, so repair/repaint keep working;
  - excludes the GLB from every hide pass via `Transform.IsChildOf`.
- **Auto-alignment** (`SpikeState.VisualAutoAlign`, default on): bounds-based
  ground/xz alignment with a 30 m per-renderer size guard and a **5 m plausibility
  guard** — Unity refreshes `Renderer.bounds` only during culling, so on the spawn
  frame the pooled instance still reports its old bounds (measured `(57.2, -0.6,
  -33.3) m`); such a delta is rejected and the GLB keeps `localPosition = 0`.
- **New console command `taxi visual [on|off]` / `taxi visual align [on|off]`**
  (also `/taxi visual`) — status plus the two Stage-2 switches; `taxi help` and the
  command description list it.
- **Asset:** `taxi.glb` (45 KB, 21 nodes, 8 materials, ~4.4 m × 1.8 m) is now checked
  in under `Source/Mods/TaxiDriver/assets/` (build deploy) *and* installed at
  `UserData\TaxiDriver\assets\taxi.glb`.
- **Live verification (2026-09-25, session 4):** F15 full run logged
  `[visual] swap DONE ... renderers=21, colliders stripped=0, vanilla children
  deactivated=10, renderer-hidden(kept active)=5, untouched(no renderers)=6, errors=0,
  align rejected as stale — GLB stays at local zero`, and a chase-camera screenshot
  from that live session (Hermes cache, since pruned — no path to quote) shows the
  low-poly yellow boxy taxi with roof sign on the road instead of the vanilla Shitbox. A second swap in the same session (cleanup → respawn) also
  succeeded.
- **Known limitations:** GLB wheels do not rotate (the animated vanilla wheels are
  hidden), vanilla paint/damage/smoke is no longer visible, trunk/POI/sound nodes
  still follow the vanilla body footprint, auto-alignment is inert on this build
  (stale bounds), `taxi visual off` only applies to the next spawn, the GLB is
  re-loaded on every spawn.

### Stage-2 review fix round (2026-09-25, same version 0.1.0)

- **`F16` hotkey toggles the visual swap** (`SpikeState.VisualSwapEnabled`, same
  single-source-of-truth pattern as F8/trace) — logs `F16 → visual swap ON|OFF
  (applies to next spawn)`. No `TryBeginGo` gate: it never starts a run. F16 is
  documented in `taxi help`, the init log, the README hotkey table and here;
  F13–F16 remain reachable only through real key input (`SendInput`,
  `focus_key.py`, F16 = VK `0x7F`).
- **`taxi status` prints the four Stage-2 fields** (`VisualSwapEnabled`,
  `VisualAutoAlign`, `VisualRoot` name or `<none>`, `VisualSwaps`).
- **GLB normalisation:** the vehicle's layer is copied onto the whole GLB subtree,
  `SetActive(true)` is normalised across it before the renderer pass, and a swap
  that ends with 0 renderers warns
  `[visual] WARNING: 0 renderers on GLB — model will be invisible`.
- **Failure rollback:** `VisualRoot` is registered as soon as the GLB is parented
  and a vanilla visibility snapshot is taken before the first mutation; a later
  exception destroys the half-attached GLB and restores child `activeSelf`,
  renderer and `LODGroup` state, so `vanilla visuals left as-is` is literally true
  (previously only the physics was untouched — the pixels could stay hidden).
- **Stale-root guard:** "already attached" only skips when the tracked root hangs
  under the *current* vehicle's `vehicleModel`; otherwise the stale reference is
  cleared and the new vehicle is swapped.
- **`taxi visual align <unknown>`** now answers with a warning and changes nothing
  (an unrecognised value used to be silently coerced to `off`).
- **Counting fix:** the final hide sweep no longer re-counts renderers the
  per-child pass already handled — the DONE field is now
  `final-sweep-only renderers hidden=N` (was: `other renderers hidden=N`, which
  summed the same renderers twice).
- **Docs:** `SwapInternal`/`Vec` XML docs, help head + init log now say
  "Stage 1 spike + Stage 2 visual swap", `ResolveAssetPath` documents its
  warning as *per call* (a miss is not cached), README/AGENTS/mod.json describe
  Stage 2 (Visual-Swap via `S1MAPI.Gltf.GltfLoader`, `assets/taxi.glb`, F16).
  Version stays **0.1.0**.
- **Live verification (real-game session 2026-09-25, session 5, PID 13692):**
  F16 → `F16 → visual swap OFF (applies to next spawn)` → F15 logged
  `[visual] swap disabled ... vanilla visuals kept` and the capture shows the
  vanilla sedan at the spawn spot; F16 → `ON` → F15 logged `swap DONE ... final-sweep-only
  renderers hidden=0` and the same spot shows the yellow taxi GLB (A/B capture pair —
  both screenshots live in the Hermes session cache, since pruned; no file paths).
  Enter/exit with the swapped vehicle (F9 ride → F9 out, screenshot taken while
  inside, same pruned cache): the soft-hidden vanilla children (wheels, `TrunkLid`) stayed hidden — no
  vanilla panel or extra wheel appeared next to or through the GLB after exiting.

### Stage 3 — NPC drives, player rides along (same version 0.1.0, 2026-09-25)

Scope V1: **vanilla NPC as driver** (no NPC model swap — that stays a later
extension).

- **Driver-seat proof replaced with a measurement.** The seat proof used to end in
  `driver-seat occupancy NOT proven` because it judged the NPC with
  `VehicleSeat.isOccupied`, a flag that can only ever be set by a *Player*
  (`VehicleSeat.Occupant` is typed `Player`, confirmed with `ilspycmd` against the
  installed `Assembly-CSharp.dll`). `PrintSeatProof` now reports the driver-seat
  **index**, the `OccupantNPCs` slot and the **root-to-seat distance (NPC root
  transform → every seat transform)** (`ClosestSeatIndex`, `SafeDriverSeatIndex`),
  and states the result honestly, e.g.
  `[after enter] seat proof: seat[0] isDriverSeat=True ... root-to-seat [0]=0.0m
  [1]=0.8m [2]=0.8m [3]=1.1m -> closest seat[0] (0.0m) -> NPC root sits on the
  DRIVER seat ... -> driver seat claimed. VehicleSeat.isOccupied=False is expected:
  VehicleSeat.Occupant is typed Player ...`.
  `DumpSeats` prints the same root-to-seat distances, and **`taxi ride` now dumps
  the seat state after boarding** (`[after ride]`) so the Player occupancy flip is
  part of the record.
- **Driver seat semantics documented** (`docs/README.md` → *Driver seat
  semantics*): `VehicleSeat.Occupant` is `Player`-typed ⇒ `isOccupied` is
  Player-only; NPC occupancy lives in `LandVehicle.OccupantNPCs`;
  `LandVehicle.IsOccupied` (vehicle flag) *is* `True` with only the NPC aboard
  while every seat flag stays `False`; `GetFirstFreeSeat()` still returns
  `seat[0]` with the NPC seated and `seat[1]` once the player holds `seat[0]`;
  `OccupantNPCs[i] ↔ Seats[i]` is consistent with all measurements but only
  verified for the single-driver case (0.4.7f6 has no per-NPC seat index).
- **Ride with player aboard — live verified (run 1, 16:39–16:43):** ⚠ **Pruned
  source: this run's log has since been deleted by MelonLoader — the lines below are
  a transcript, not re-greppable evidence** (run 2, further down, is). `F15` at 16:39:16.499 → spawn at
  `(-74.4, 4.0, 84.9)` → NPC `benji_coleman` seated (proof: body-to-seat
  `[0]=0.0m`) → `F9` ride at 16:39:18.173 (F15 + 1.7 s, after the npc step) →
  `[primitive c] PROOF: LocalPlayerIsInVehicle=true` → `Navigate` dispatched →
  polls show `onVehicleGraph=True` and `speed=2.5 → 20.6 km/h` with the vehicle
  position advancing across the run (≈150 m of road with the player aboard) →
  after two further `F13` dispatches (45 s budget each) **`[nav] callback result=
  Complete after 14.5s`, last poll `distToTarget=5.8m`** → `F9 out` at
  16:43:21.310 → `[primitive c] PROOF: LocalPlayerIsInVehicle=false` — the player
  is standing at the destination (≤ 8 m from the raw target).
- **Ordering F15 vs. F9 proven in code and in the run:** `Cleanup()` (first thing
  `F15` does) calls `LandVehicle.ExitVehicle()` whenever `LocalPlayerIsInVehicle`
  is true, and during cleanup + the 0.5 s deferred respawn `SpikeState.Vehicle`
  still references the vehicle being destroyed — so `F9` must come **after**
  `F15` (after step 1/2), never before it. Documented in the README instead of a
  code change: nothing needed fixing, the existing guards behave as designed.
- **New known limitations (README):** the player takes `seat[0]` as well and
  overlaps the NPC (no public seat-selection API); a `forward * 6` spawn can wedge
  the car in a wall corner (`speed=0.0` for 45 s, `callback result=Stopped` twice —
  with *and* without a passenger, so the passenger is not the cause; re-orienting
  the player so the spawn lands on open road fixed it); the 45 s navigation budget
  needs a re-dispatch for trips ≳ 100 m; and the driver is not observable from
  outside while the Stage-2 GLB is on (`taxi.glb` `Glass` material is OPAQUE,
  `alpha = 1.0`) — at 04:00 in rain the vanilla cabin is pitch black too, so the
  numeric seat distance stays the authoritative "who sits where" evidence.
- **Second run re-verified from a log that is still on disk**
  (`MelonLoader/Logs/26-9-25_17-23-11.log`; MelonLoader prunes older logs at each
  start; quoted lines keep the label as logged — `body-to-seat`, which the spike now
  prints as `root-to-seat`): `F15` 17:32:51.896 → NPC `chloe_bowers` seated (body-to-seat `[0]=0.0m`)
  → `F9` ride 17:32:53.981 (`[primitive c] PROOF: LocalPlayerIsInVehicle=true`,
  `[after ride] seat[0] ... playerOccupant='Dominik'`, NPC still
  `OccupantNPCs[0]` at 0.0 m) → polls `[nav t=0.5s … 31.3s]` in which the position
  advances across the run (stationary for consecutive polls while reversing at
  ≈0 km/h; 2.0 → 20.6 km/h; `distToTarget` 26.9 m at the first poll → 5.3 m at the
  last, peaking at 33.1 m around t=24 s) →
  `[nav] callback result=Complete after 31.6s (frame=51317)` → `F9 out`
  17:34:39.578 → `[primitive c] PROOF: LocalPlayerIsInVehicle=false` with the
  player on foot beside the car at the target. The full block is quoted in the
  README (*Ride with player*, "Second run").
- **Build:** Release, 0 warnings / 0 errors. Version stays **0.1.0**.

### Etappe-3 review fix round (2026-09-25, same version 0.1.0)

- **B1 — the seat proof no longer states unproven facts** (`PrintSeatProof`):
  `driver seat claimed` additionally requires an `OccupantNPCs` slot (`slot >= 0`) —
  on `slot == -1` the line now reads `driver seat NOT proven (seating incomplete)`
  instead of `OccupantNPCs[-1] holds it`; `seat[i] isDriverSeat=True` is only printed
  for `driverIdx >= 0` (otherwise `driver seat NOT identified (driverIdx=-1, ...)`);
  the passenger case (`closest != driver`) uses neutral wording and implies no
  `OccupantNPCs[i] <-> Seats[i]` mapping; the XML docs now say *measures* vs
  *assumes* and spell out the claim guard.
- **Label unification:** `body-to-seat` → `root-to-seat` (measured from
  `npc.transform.position`) in the spike output, `taxi help`, XML docs, command
  table and docs prose; quoted live-log lines keep the label the build printed at
  the time (noted in the README *Driver seat semantics* and at run 2).
- **Help / command description cover Stage 3:** the help head, `taxi npc` (seat
  proof), `taxi ride` (`[after ride]` dump) and the `BaseConsoleCommand`
  `CommandDescription` now mention Stage 3.
- **Evidence corrections in the docs:** run 1 (16:39–16:43, `Complete after 14.5 s`,
  log since pruned by MelonLoader) carries an inline pruned-source marker at the
  start of its table and of the CHANGELOG bullet, and run 2 (`Complete after 31.6 s`,
  log on disk) is the headline number in `README.md`, `AGENTS.md` and `mod.json`;
  `position changing on every poll` was replaced with `position advances across the
  run (stationary for consecutive polls while reversing at ≈0 km/h, identical
  positions at t=5.6/6.1/6.6 s and t=14.2/14.7 s)` and the `distToTarget 26.9 → 5.3
  m` endpoints now carry their 33.1 m peak around t=24 s; `LocalPlayerIsInVehicle=
  true for the whole trip` is stated as an absence (no `ExitVehicle` logged between
  the ride at 17:32:54 and the out at 17:34:39); chase-camera captures are labelled
  as pruned Hermes-cache screenshots; the run-2 quote's two joined log lines are
  split into their own timestamps, the missing full stop on the `F15 pressed` line
  is restored and the two truncations carry an explicit `…`.
- **README structure:** the intro list gains the Stage-3 point, `Known limitations`
  is scoped `Stages 1–3` and references the still-open `OccupantNPCs[1] <-> Seats[1]`
  follow-up; `mod.json` describes Stage 3 (driver ride, root-to-seat 0.0 m,
  `Complete after 31.6s`) and refines the known limitations (NPC occupancy = slot +
  measured distance, index mapping not observable / single-driver only). Version
  stays **0.1.0**.
