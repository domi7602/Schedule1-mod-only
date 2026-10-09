# TaxiDriver 0.8.2 — TaxiApp service, no keyboard controls

TaxiDriver is a Schedule I taxi service controlled through the in-game phone app.
The current local target is Schedule I **0.4.7f12**, S1API **3.2.1-beta.8**,
and S1MAPI **2.0.1**. The no-deploy Release build passed on 2026-10-08 against
the installed references (0 warnings, 0 errors); no TaxiDriver in-game test was
run. The app displays the calculated fare. A clean return from S1API's void Money
methods does not confirm a balance change; the settlement summary labels returned,
unknown and unpaid amounts separately. **Keine Behauptung vollständiger Beta-8-Kompatibilität ohne passenden Build
beziehungsweise Laufzeittest.**

Current service path:

1. **CALL TAXI** calls `SpikeCommands.CallTaxi`, which still drives the existing
   `SpikeRunner.TickAutoRun` workflow: spawn at the fixed stand, board the
   registered `taxi_driver`, then navigate to the player.
2. **Choose a destination** in the app using its list, search and filters. The
   passenger ride uses the game's `VehicleAgent`/patrol AI and existing recovery.
3. **Live status and fare** remain visible in the app. **STOP** cancels and uses
   ownership-checked cleanup; uncertain destruction retains the tracked handle.
4. The taxi's `taxi.glb` remains the visual. If import fails or has no usable
   renderer, vanilla vehicle visuals stay visible; no functional GameObject is
   deactivated for the visual swap.
5. Vanilla **E** entry/exit and **Escape** phone-close remain unchanged. TaxiDriver
   itself installs no keyboard bindings; F1–F12 belong to the game and other mods.

**F12 drücken | Kein Vanilla-Fahrzeug wird übernommen.**

## TaxiApp service flow

Press **CALL TAXI** in the phone app. It uses the existing
`SpikeCommands.CallTaxi` → `SpikeRunner.TickAutoRun` path: spawn at the fixed
stand, seat the registered `taxi_driver`, then navigate to the player. Destination
selection, search, filters, live status, fare display and STOP remain in the app.

STOP may ask for a second press during an active ride. Cleanup only destroys the
tracked TaxiDriver vehicle; if destruction is pending or uncertain, the app says
so and retains the reference. Vanilla **E** entry/exit and **Escape** phone-close
remain unchanged. No TaxiDriver keyboard controls are installed.

## Current Console Diagnostics (read-only)

The `taxi` console entry remains for diagnostics only: `help`, `codes`, `status`,
`diag`, `probe`, `trace [on|off|status]`, `lots`, `pois`, `fare`, and `ai`. It does
not spawn vehicles, seat drivers, start rides, change destinations, stop rides,
clean up vehicles, or toggle visuals. Those actions are available only through
the TaxiApp. Vanilla console slash handling may vary; this does not affect the app.

The older implementation notes below describe previous versions only; their
operational console commands and keyboard bindings are not available in v0.8.2.

## Historical Console Operations — unavailable in v0.8.2

The following command table is retained as an implementation-history record only.
Its operational commands are no longer dispatched; use the read-only list above
for the current console surface.

| Command | What it does |
| --- | --- |
| `taxi help` | Command list (`h`, `?` work as well). |
| `taxi codes` | Iterates `VehicleManager.Instance.VehiclePrefabs` and prints `VehicleCode`, `VehicleName`, `VehiclePrice` per prefab — the vehicle code catalog for later stages. |
| `taxi spawn [code]` | Spawns 6 m in front of the player (`forward * 6`), **`playerOwned=true`** (required so `ShouldBePhysicallySimulated` can pass — a spawned vehicle otherwise never becomes physically simulated and `Navigate` gives up before `CalculatePath`). Y is snapped with a `Physics.RaycastAll` that starts **above the spawn point** (not the player) and only accepts non-trigger surfaces without a rigidbody — the hit object and layer are logged, so a tilted spawn can be explained. Without an argument the first prefab entry *with* a `VehicleCode` is used (no blind `prefabs[0]`). Guards on `InstanceFinder.IsServer`, validates through `VehicleManager.GetVehiclePrefab(code)`, logs code/name/price/position and the typed `LandVehicle.GUID` (an all-zero GUID is reported as "not network-registered"). **Freeze guard:** when a spike vehicle already exists, cleanup is verified first (NPC exit proven, no NPC occupants) and the respawn is **deferred by 0.5 s to a later tick** — destroy and spawn never happen in the same frame. |
| `taxi npc` | Picks the nearest living NPC from `NPCManager.NPCRegistry` — dead NPCs **and** NPCs whose `Health` is null are skipped, as is the NPC that left a vehicle less than 15 s ago (re-enter cooldown; it is only reused when there is no alternative, with a warning). Calls `npc.EnterVehicle(null, veh)`, then dumps `npc.IsInVehicle`, `npc.CurrentVehicle != null`, `LandVehicle.GetFirstFreeSeat()`, every `LandVehicle.Seats` entry (`gameObject.name`, `isDriverSeat`, `isOccupied`, player occupant) and `LandVehicle.OccupantNPCs` — reported as **capacity vs. filled slots**, never as occupancy. Whenever `IsInVehicle`/`CurrentVehicle`/the occupant slot disagree, `LandVehicle.AddNPCOccupant(npc)` runs as the fallback, and a one-line **seat proof** reports the driver-seat **index** (only printed for a real index ≥ 0), the `OccupantNPCs` slot (a claim is only printed for slot ≥ 0) and the **measured root-to-seat distance — NPC root transform → every seat transform** — the physical seat is measured, never inferred from `VehicleSeat.isOccupied` (which is Player-only, see [Driver seat semantics](#driver-seat-semantics-live-verified-2026-09-25)). |
| `taxi ride` | `LandVehicle.EnterVehicle()` — local player gets in; logs `LocalPlayerIsInVehicle`, `IsOccupied`, `DriverPlayer`, then dumps the whole seat state again as `[after ride]` (which seat took the Player occupant, which `isOccupied` flag flipped, and the NPC root-to-seat distances). |
| `taxi out` | `LandVehicle.ExitVehicle()` — local player gets out; logs `LocalPlayerIsInVehicle`. |
| `taxi go [distance=40]` | Target = player position + player forward × distance (Y = player Y). **Pre-dispatch vehicle release:** `LandVehicle.UpdatePhysicallySimulated(true)` when `IsPhysicallySimulated` is false, `ExitPark(false)` while parked, `BrakesApplied=false`, `HandbrakeApplied=false` and `agent.enabled=true` (a fresh spawn sits parked with brakes on, which keeps `AutoDriving` silently false). **Target preparation:** `NavigationUtility.SampleVehicleGraph(target)` snaps the destination onto the graph and the snap is only accepted when the delta is **≤ 25 m** (larger deltas keep the raw target); the vehicle is then force-turned with `Quaternion.LookRotation` toward the target, because the spawn forward can point away from it and burn the run on reverse maneuvers. Then it builds `NavigationSettings { endAtRoad = true, ensureProximityToGraph = true, teleportToGraphIfCalculationFails = true }` and calls `agent.Navigate(target, settings, callback)`. **The `ENavigationResult` callback is passed** through the implicit `NavigationCallback` operator; if the callback dispatch is rejected at runtime, the command retries once without it and says so. Completion is therefore authoritative, with 0.5 s polling as the fallback/telemetry. |
| `taxi go2 [distance=40]` | Same as `taxi go`, but dispatches with `NavigationSettings = null` (A/B against the settings run). The 3 s settings=null retry is skipped (`NavRetried` starts true), so this is the pure null-settings measurement. |
| `taxi stop` | `VehicleAgent.StopNavigating()` and ends the polling. |
| `taxi status` | Dumps every spike state field (vehicle ref, NPC ref, distances, polling, F6 automation incl. **`AutoTarget`** — `null` = blind `forward * 40` — and **`SkipNpcStep`**) plus the four Stage-2 visual fields: **`VisualSwapEnabled`**, **`VisualAutoAlign`**, **`VisualRoot`** (name or `<none>`) and **`VisualSwaps`**. |
| `taxi cleanup` | `NPC.ExitVehicle()` → **verified** (`npc.IsInVehicle == false`, otherwise `LandVehicle.RemoveNPCOccupant(npc)` and a refusal) → `LandVehicle.ExitVehicle()` (if the player is in) → `VehicleAgent.StopNavigating()` (if auto-driving) → `LandVehicle.DestroyVehicle()` **only when `OccupantNPCs` is empty**. State is cleared only after a successful destroy, so a failed cleanup stays retryable instead of stranding the vehicle. |
| `taxi probe` | Dumps the silent-failure preconditions of `taxi go` for the spike vehicle **and every other vehicle** in `VehicleManager.AllVehicles`: `DriveFlags` (null?), `roadSeeker`/`generalSeeker` (null?), `NavigationCalculationInProgress`, `IsOnVehicleGraph()`, `IsPhysicallySimulated`, `IsOwner`/`IsPlayerOwned`, `State`, plus `NavigationUtility.SampleVehicleGraph()` deltas for start **and** target — the differential that shows whether the spike vehicle differs from a vanilla one that drives. |
| `taxi trace [on\|off\|status]` | `on` patches **four** methods through `S1Mods.Shared.PatchGuard` (a missing/renamed method degrades to a warning): `VehicleAgent.Navigate`, `NavigationUtility.CalculatePath`, `VehicleAgent.NavigationCalculationCallback` and `VehicleAgent.StopNavigating` — one log line per dispatch, per path calculation, per async result and per stop call (with the managed stack). `off` silences the lines; the prefixes stay installed. A third argument (or none) reports `patched=` / `logging=` — `taxi trace status`. |
| `taxi visual [on\|off]` | Stage 2 switchboard for the GLB visual swap: no argument prints the status (swap/align flags, resolved GLB path, attached visual root), `on`/`off` arms or disarms the swap for the **next** spawn, `taxi visual align [on\|off]` does the same for the bounds auto-alignment. Unknown modes **and unknown align values** answer with a warning and change nothing (an unrecognised `align` value used to be silently coerced to `off`). The swap itself runs inside `taxi spawn`; the `on`/`off` switch is additionally on hotkey **F4**. |

**Subcommand aliases:** `taxi list` = `codes`, `taxi driver` = `npc`, `taxi in` = `ride`,
`taxi out` = `exit`, `taxi reset` = `cleanup`.

### Historical keyboard controls — removed in v0.8.2

The table below records the retired diagnostic bindings only. They are not
registered or handled by the current TaxiDriver build; F1–F12 remain available
to the base game and other mods.

Historical behavior in the 2026-09-25 build: the MelonLoader console accepted no
input and the bindings below were used for diagnostics. They were removed in
v0.8.2; use the TaxiApp service flow above. This table is not a current control map.

| Key | What it runs | Purpose |
| --- | --- | --- |
| **F1** | `taxi go` → `(-131.4, -4.0, 51.9)` | Dispatch against the first vanilla-proven road target. |
| **F2** | `taxi go` → `(-17.1, 0.0, 13.4)` | Dispatch against the second vanilla-proven road target. |
| **F3** | `spawn → npc → go` → `(-131.4, -4.0, 51.9)` | The proven full configuration: NPC at the wheel against a road target. |
| **F4** | `taxi visual on/off` | Toggles `SpikeState.VisualSwapEnabled` for the **next** spawn (same single-source-of-truth pattern as F8). No `TryBeginGo` gate: the key never starts a run, so it works at any time — an existing vehicle keeps the visuals it spawned with. |
| **F5** | `spawn → npc → go` to a road point near the player | Stage 3b call-taxi: the taxi spawns at the fixed taxi stand, the nearest NPC boards and drives to the player (`F9` boards afterwards). |
| **F6** | `spawn → +1 s npc → +1 s go 40` | The full spike run against a blind `forward * 40` target. |
| **F7** | `taxi probe` | Navigate preconditions for the spike vehicle and every vanilla vehicle. |
| **F8** | `taxi trace on/off` | Toggles the four Harmony prefixes (state comes from `SpikeTrace.Logging`, so key and command can never drift). |
| **F9** | `taxi ride` / `taxi out` | Toggles the local player in/out of the spike vehicle. |
| **F10** | `taxi go2 40` | `Navigate` with `settings=null` (A/B against `taxi go`). |
| **F11** | `spawn → go` (no npc step) | Occupant hypothesis: the run skips the NPC so `Navigate` can be measured without a driver. |
| **F12** | `taxi go 40` on a **vanilla** vehicle | Vehicle-vs-caller A/B: `Complete` implicates our spawn, `Failed` implicates the caller context. |

While an automatic run is in progress or a deferred respawn is pending, every
run-starting key (F1–F3, F5, F6, F10–F12) is **ignored with a logged reason**
(`F<n> ignored — run in progress / respawn pending`) instead of interrupting the run.
F4/F7/F8/F9 are state toggles with no such gate — F4 in particular must stay
pressable *during* a run, because it only decides what the next spawn looks like.

> **Keys remapped 2026-09-26 (F13-F17 -> F1-F5):** the keyboard has only F1-F12,
> so the physically unreachable F13-F17 keys moved to F1-F5. Historical session
> records in this file from 2026-09-25 still use the old names.

## Automatic pickup flow (current)

CALL TAXI sets the existing automatic-run state in `SpikeCommands.CallTaxi`.
`SpikeRunner.Update` continues to call `TickAutoRun` independently of keyboard
input: spawn at the stand → board `taxi_driver` → navigate to the player. Each
step remains bounded and reports failures; removing keyboard dispatch did not
remove this phone-app workflow.

## Historical navigation polling (2026-09-25 build)

The CALL TAXI pickup's navigation step switches polling on. `SpikeRunner.Update`
then logs a heartbeat **every 0.5 s**:

```
[nav t=4.0s] AutoDriving=True target=(...) vehicle=(...) distToTarget=12.3m
             onVehicleGraph=True stuck=False speed=31.5 km/h
```

* The `NavigationCallback` fires → `[nav] callback result=Failed|Complete|Stopped`
  and polling stops (authoritative).
* `AutoDriving == false` → final log with target/vehicle position, the final
  distance vs. the start distance, the elapsed time, the callback result and whether
  `AutoDriving` was ever observed true. The verdict distinguishes **ARRIVED**
  (within **10 m** of the target — `ArrivalThresholdMeters`, raised from 5 m because
  the proven `Complete` run still ends ~8.3 m from the raw target with
  `endAtRoad = true`) from **STOPPED SHORT**.
* The automatic `settings = null` retry at t = 3 s now gets its own **6 s grace
  window** — it used to be cut off by the terminal branch half a second later, so
  the A/B comparison measured nothing.
* 45 s elapsed → `[nav] timeout ...` warning and `VehicleAgent.StopNavigating()`.
  The timeout applies to **every** shape of hang, including a
  `NavigationCalculationInProgress` that never returns.
* Per-frame exceptions are logged **once per episode** (a clean tick re-arms the
  report), never spammed.

## Visual model swap (Stage 2 — visible model)

The spawned spike vehicle keeps **100 % vanilla physics**: body, `Main Collider`,
`Roof Collider`, the four `Wheel*` objects with their wheel colliders, rigidbody,
`VehicleAgent` and every damage/repaint hook are untouched. Only the *pixels* change:
the vanilla `Shitbox` visuals are switched off and `taxi.glb` hangs in as a pure
visual child of `LandVehicle.vehicleModel`.

**Loader:** `S1MAPI.Gltf.GltfLoader.LoadFromFile(path)` from
`UserLibs\S1MAPI_Il2cpp.dll` — the runtime GLB pipeline that
AutoPackagingStation/SnackVendor already used. No AssetBundle, no Unity round-trip,
URP materials are configured by S1MAPI itself (`TaxiVisual` only forces the
renderers on and never touches `renderer.material.shader`).

**GLB path resolution** (`TaxiVisual.CandidatePaths`, first hit wins and is logged):

1. `<GameDir>\UserData\TaxiDriver\assets\taxi.glb` — documented hand-install location,
2. `<GameDir>\Mods\TaxiDriver\taxi.glb` — what `Directory.Build.targets` deploys from
   `Source/Mods/TaxiDriver/assets/*.glb`,
3. `<GameDir>\Mods\TaxiDriver\assets\taxi.glb` — loose install.

No file found → one warning **per lookup** (a miss is not cached, so every spawn
and every `taxi visual` prints it again — review M3-11), the vanilla visuals stay
visible, the spawn/physics are unaffected.

**What the swap does per spawn** (`TaxiVisual.SwapAfterSpawn`, called at the end of
`SpikeCommands.SpawnVehicle`, all exceptions caught):

| Vanilla child of `vehicleModel` | Action |
| --- | --- |
| has renderers, no colliders (`Main_LOD0..2`, lights, indicators, `SmokeParticles`) | `SetActive(false)` — never destroyed, so repair/repaint keep finding the nodes |
| has renderers **and** colliders (`TrunkLid`, `Wheel FL/FR/RR/RL`) | object stays active, renderers `enabled = false` + `forceRenderingOff = true`, `LODGroup` off — the colliders behind them must keep working |
| no renderers at all (`Main Collider`, `Roof Collider`, `TrunkInteraction`, `TrunkGrid`, `OwnedVehiclePoI`, `VehicleSound`) | untouched — functional nodes, not visuals |
| the GLB itself | excluded from every hide pass (`Transform.IsChildOf`), colliders stripped (they would become compound colliders of the vanilla rigidbody) |

The GLB is parented with `localPosition = 0`, `localRotation = identity`,
`localScale = 1`, i.e. it inherits the vehicle transform for free. Right after the
parenting the swap also:

* copies `vehicleModel`'s **layer** onto the GLB root and every descendant
  (Unity layers are per GameObject — a GLB imported on its own layer can be culled),
* runs a **`SetActive(true)` normalisation over the whole GLB subtree** before the
  renderer pass (S1MAPI may hand back an inactive hierarchy), and
* **warns when the enable pass ends with 0 renderers**
  (`[visual] WARNING: 0 renderers on GLB — model will be invisible`) instead of
  logging an unqualified `swap DONE`.

**Failure rollback (review M2-4):** `SpikeState.VisualRoot` is registered the moment
the GLB is parented, and a vanilla visibility snapshot (child `activeSelf`,
renderer `enabled`/`forceRenderingOff`, `LODGroup.enabled`) is taken *before* the
first mutation. If anything after that point throws, `RollbackPartialSwap()` destroys
the half-attached GLB, clears `VisualRoot` and writes the snapshot back — so the
reported `vanilla visuals left as-is` is literally true for the pixels as well, not
only for the physics. Nothing is left half-hidden.

**Stale-root guard (review M2-5):** "already attached" is only trusted when the
tracked root really hangs under the *current* vehicle's `vehicleModel`; otherwise the
stale reference is cleared (with a log line) and the new vehicle is swapped.

**Auto-alignment (`SpikeState.VisualAutoAlign`, default on):** the GLB is translated
so its bounds sit where the vanilla visual sat — same ground height (bounds *min* y)
and same centre in x/z. Two guards make it safe:

* vanilla renderers larger than 30 m per edge are ignored (a POI marker or particle
  emitter otherwise stretches the union across the whole map), and
* an offset larger than **5 m** is rejected as *stale bounds*: Unity only refreshes
  `Renderer.bounds` during culling, so on the spawn frame the freshly teleported
  pooled instance still reports the pre-spawn bounds. Measured live:
  `(57.2, -0.6, -33.3) m` — accepting it would strand the taxi in a field, so the
  GLB keeps its local-zero placement (which is the verified correct one).

**Live proof (2026-09-25):** `F15` full run logs
`[visual] swap DONE: GLB 'TaxiDriver_TaxiVisual' parented under 'Shitbox'
(renderers=21, colliders stripped=0); vanilla children deactivated=10,
renderer-hidden(kept active)=5, untouched(no renderers)=6, errors=0, …`
(the quote is abbreviated — the line continues with the final-sweep count and the
align verdict; since review M3-9 that field is `final-sweep-only renderers hidden=N`,
i.e. it no longer repeats renderers the per-child pass already counted),
and a chase-camera screenshot from that live session shows the low-poly **yellow
boxy taxi with the roof sign** on the road instead of the vanilla Shitbox
(**screenshot from the live session — Hermes cache, since pruned**; no file path is
quoted because the capture is no longer on disk). Two swaps in one session
(cleanup → respawn) both succeeded.

**A/B proof of the F16 toggle (live, 2026-09-25 session 5):** two F15 runs against
the *identical* spawn point `(-116.4, -3.9, 68.6)`, 6 m ahead of the player, shot
~0.8 s after each spawn — swap **OFF** shows the vanilla sedan at that spot (log:
`[visual] swap disabled ... vanilla visuals kept`), swap **ON** shows the yellow
taxi GLB at the same spot (log: `swap DONE ...`). A subsequent `F9 ride` →
`F9 out` cycle on the swapped vehicle left the soft-hidden vanilla children
(wheels, `TrunkLid`) hidden: no vanilla panel and no second/floating wheel showed
up next to or through the GLB after exiting.

## Logging

All output goes through the shared `S1Mods.Shared.ModLogger`, i.e. every line is
prefixed `[TaxiDriver]`. Every error names the exact native method that failed
plus `Exception.Message`, e.g.
`VehicleManager.SpawnAndReturnVehicle('sedan', ...) threw: ...`.

## Native signatures used (verified against the installed 0.4.7f6 interop assembly)

* `Il2CppScheduleOne.Vehicles.VehicleManager.Instance` — `List<LandVehicle> VehiclePrefabs`,
  `List<LandVehicle> AllVehicles`, `GetVehiclePrefab(string)`,
  `SpawnAndReturnVehicle(string, Vector3, Quaternion, bool)`.
* `Il2CppScheduleOne.Vehicles.LandVehicle` — `Agent` (public property),
  `Seats` (`Il2CppReferenceArray<VehicleSeat>`), `OccupantNPCs`,
  `driverEntryPoint`, `IsOccupied`, `DriverPlayer`, `LocalPlayerIsInVehicle`,
  `GetFirstFreeSeat()`, `AddNPCOccupant(NPC)`, `RemoveNPCOccupant(NPC)`,
  `EnterVehicle()`, `ExitVehicle()`, `DestroyVehicle()`, `VehicleCode`,
  `VehicleName`, `VehiclePrice`, `Speed_Kmh`, `IsPlayerOwned`.
* `Il2CppScheduleOne.Vehicles.VehicleSeat` — `isDriverSeat`, `isOccupied`,
  `Occupant` (typed **`Player`** — NPC occupants are tracked through
  `LandVehicle.OccupantNPCs`, not through the seat occupant).
* `Il2CppScheduleOne.Vehicles.AI.VehicleAgent` —
  `Navigate(Vector3 location, NavigationSettings settings = null, NavigationCallback callback = null)`,
  `StopNavigating()`, `AutoDriving`, `NavigationCalculationInProgress`,
  `TargetLocation`, `IsOnVehicleGraph()`, `GetIsStuck()`, `Flags` (`DriveFlags`),
  `roadSeeker`/`generalSeeker` (`Il2CppPathfinding.Seeker`, assembly
  `Il2CppAstarPathfindingProject`), nested `ENavigationResult { Failed, Complete, Stopped }`
  and nested `NavigationCallback` with the implicit `Action<ENavigationResult>` operator.
* `Il2CppScheduleOne.Vehicles.AI.NavigationUtility` — static
  `SampleVehicleGraph(Vector3)` and
  `CalculatePath(Vector3 startPosition, Vector3 destination, NavigationSettings, DriveFlags, Seeker generalSeeker, Seeker roadSeeker, NavigationCalculationCallback)`.
* `Il2CppScheduleOne.Vehicles.AI.NavigationSettings` — public parameterless ctor
  plus the settable bool properties `endAtRoad`, `ensureProximityToGraph`,
  `teleportToGraphIfCalculationFails`.
* `Il2CppScheduleOne.Vehicles.LandVehicle.vehicleModel` — public `GameObject` field,
  the visual root the swap hangs the GLB under (fallback when it is null: the
  vehicle `gameObject` itself).
* `S1MAPI.Gltf.GltfLoader` (assembly `UserLibs/S1MAPI_Il2cpp.dll`, namespace
  `S1MAPI.Gltf`) — `LoadFromFile(string, Shader)`, `LoadGlb(byte[], Shader)`,
  `Load(byte[], Shader)`, `LoadFromJson(string, string, Shader)`.
* `Il2CppScheduleOne.NPCs.NPCManager` — `public static List<NPC> NPCRegistry`.
* `Il2CppScheduleOne.NPCs.NPC` — `EnterVehicle(NetworkConnection, LandVehicle)`,
  `ExitVehicle()`, `CurrentVehicle`, `IsInVehicle`, `Health` (`NPCHealth.IsDead`),
  `ID`.
* `Il2CppScheduleOne.PlayerScripts.Player.Local`, FishNet `InstanceFinder.IsServer`,
  `S1API.Input.Controls.IsTyping`, `MelonEvents.OnUpdate.Subscribe/Unsubscribe`.

## Deviations from the original spike plan

* **No `[ConsoleCommand]` attribute and no `Aliases` property** exist in the
  deployed S1API 3.x assembly. Registration is done by S1API's own auto-discovery
  (`ConsolePatches.AddCommands` → `ReflectionUtils.GetDerivedClasses<BaseConsoleCommand>()`
  → `Activator.CreateInstance`), which requires a *public* parameterless
  constructor — `TaxiConsoleCommand` and `TaxiSlashCommand` are `public sealed`.
  The slash is **not** assumed to be stripped: `taxi` and `/taxi` are registered as
  two separate command words that resolve to one handler.
* **The navigation callback is usable after all.** An earlier note claimed an
  "IL2CPP delegate-ctor pitfall"; the deployed interop assembly exposes
  `public static implicit operator NavigationCallback(System.Action<ENavigationResult>)`,
  which is the supported conversion. `taxi go` passes a callback and only falls back
  to `callback = null` when that dispatch is rejected at runtime.
* **`S1API.Console.ConsoleHelper.Print` does not exist** — answers are printed
  through the `[TaxiDriver]`-prefixed MelonLoader logger instead.
* **`NPC.WasCollected` and `NPC.CurrentVehicleSeat` do not exist** in 0.4.7f6
  (`CurrentVehicleSeat` lives on `Player`). Dead NPCs are filtered through
  `NPC.Health.IsDead`, and the seat result is proven with the `LandVehicle.Seats`
  dump plus `GetFirstFreeSeat()`.

## Build & deploy

```
dotnet build Source/Mods/TaxiDriver/src/TaxiDriver.csproj -c Release
```

`Directory.Build.targets` copies the DLL to `<GameDir>\Mods\TaxiDriver.dll` and
`mod.json` + `TaxiDriver.pdb` to `<GameDir>\UserData\TaxiDriver\` on every build
(`-p:S1NoDeploy=true` skips the deploy).

## Review fix round (same version 0.1.0, 2026-09-25)

Implemented from the completed code review, in priority order:

1. **Freeze guard (blocker):** `taxi spawn` no longer destroys and respawns in the
   same frame — cleanup is verified (NPC exit proven, `OccupantNPCs` empty) and the
   respawn is deferred by 0.5 s; `taxi cleanup` refuses `DestroyVehicle()` while NPC
   occupants remain and keeps the state retryable on failure; `taxi npc` applies a
   15 s re-enter cooldown for the last boarded NPC; F6 logs the start of step 2 with
   a frame number so a repeat freeze is pin-pointable.
2. **Retry measurement window:** the `settings = null` retry gets a 6 s grace window
   before the terminal branch may fire; the 45 s timeout now also covers a hung
   path calculation; the final verdict distinguishes ARRIVED from STOPPED SHORT.
3. **NPC seat proof:** success is judged by `IsInVehicle` + `CurrentVehicle` +
   `OccupantNPCs` slot (making the `AddNPCOccupant` fallback reachable), the occupant
   array is reported as capacity vs. filled, and a one-line seat proof states whether
   the driver seat is proven. NPCs with `Health == null` are no longer selectable or
   shown as alive.
4. **Spawn snap:** ray starts above the spawn point, filters triggers and rigidbody
   holders, and logs the hit object/layer.
5. **Navigate callback:** `ENavigationResult` callback is passed (implicit operator),
   with a `callback = null` fallback and a state-before-dispatch ordering.
6. **Error reporting:** the one-shot exception suppression re-arms after a clean tick
   and no longer depends on the hotkey path.
7. **Instrumentation:** `taxi probe` (preconditions for every vehicle) and
   `taxi trace on|off|status` (four Harmony prefixes via `PatchGuard`:
   `VehicleAgent.Navigate`, `NavigationUtility.CalculatePath`,
   `VehicleAgent.NavigationCalculationCallback`, `VehicleAgent.StopNavigating`).
8. **Minor fixes:** typed `LandVehicle.GUID` (was reflection printing the zero GUID),
   `State` logged as its runtime type name, no blind `prefabs[0]`, teardown in
   `OnDeinitializeMelon` as well, stale-state clearing on scene load, `/taxi` alias,
   heartbeat documented as 0.5 s (was 2 s).

## Historical live verification (real-game session 2026-09-25)

This records the older v0.1.0 feasibility build on Schedule I 0.4.7f6. It is not
evidence that the current v0.8.2 source has been run in-game.

Stage 1 is **verified live** after 13 test rounds — spawn → NPC → path → drive →
arrival all observed in one session:

* **Result:** `[nav] callback result=Complete after 20.2 s` (F15 full run to the
  road target `(-131.4, -4.0, 51.9)`), NPC at the wheel, heartbeat polling active
  the whole way.
* The freeze guard, `taxi probe`, `taxi trace`, the ARRIVED/STOPPED SHORT verdict
  and the settings=null retry were all exercised in the same session; no game
  freeze occurred.
* The previously open question "does `VehicleAgent.Navigate` reach the vanilla path
  calculation at all" is answered: **yes**, and it completes.

**Limitations of that historical build:**

* **`VehicleSeat.isOccupied` stays `False`** for the NPC driver — that flag is
  Player-only by construction (`VehicleSeat.Occupant` is typed `Player`), so NPC
  proof comes from `LandVehicle.OccupantNPCs` (plus `IsInVehicle` /
  `CurrentVehicle`) **and** from the measured root-to-seat distance. See
  [Driver seat semantics](#driver-seat-semantics-live-verified-2026-09-25).
* **End tolerance ≈ 8.3 m** with `endAtRoad = true`: the vehicle stops at the road
  projection of the target, not on the raw coordinates. `ArrivalThresholdMeters` is
  therefore 10 m so the non-callback verdict does not report a complete run as
  STOPPED SHORT.
* That build used a log-only MelonLoader console and keyboard bindings for
  diagnostics; v0.8.2 removes those bindings and uses the phone app.
* **Its hotkeys fit F1–F12 and were scene-gated:** keys were remapped on
  2026-09-26 (F13-F17 -> F1-F5) because the keyboard has only F1-F12, and the
  dispatcher ignores keys outside the gameplay scene (menu scenes keep their own
  keys, e.g. MoreSaveSlots binds F2/R on the save screens).

## Driver seat semantics (live-verified 2026-09-25)

**How the driver seat is claimed by an NPC** — three measured facts, all printed
by the same `[after enter] seat proof` line:

1. **`VehicleSeat.Occupant` is typed `Player`** (`ilspycmd -t
   Il2CppScheduleOne.Vehicles.VehicleSeat` on the installed
   `MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll` →
   `public unsafe Player Occupant`, plus `public unsafe bool isDriverSeat` and the
   `isOccupied` getter). An NPC can never be stored on a seat, so
   **`VehicleSeat.isOccupied` reports Player occupancy only** — it stays `False`
   for an NPC driver *by design*, not because the seat is free.
2. **NPC occupancy lives in `LandVehicle.OccupantNPCs`** — the NPC proof is
   `OccupantNPCs[i] == npc` together with `npc.IsInVehicle` / `npc.CurrentVehicle`.
3. **Which physical seat the NPC sits on is measured, not assumed.** The seat proof
   prints the root-to-seat distance (NPC root transform → every seat transform).
   Live (`F15`, two different NPCs — `chloe_bowers` and `benji_coleman`, three
   runs):

   ```
   [after enter] seat proof: seat[0] isDriverSeat=True name='GameObject' isOccupied=False;
   NPC 'benji_coleman' at OccupantNPCs[0] (filled=1/4);
   body-to-seat [0]=0.0m [1]=0.8m [2]=0.8m [3]=1.1m -> closest seat[0] (0.0m) ...
   -> driver seat claimed. VehicleSeat.isOccupied=False is expected: VehicleSeat.Occupant
   is typed Player, so an NPC can never be stored on the seat (NPC occupancy lives in
   LandVehicle.OccupantNPCs).
   ```

   The NPC root coincides with the driver-seat transform (**0.0 m**, the other seats
   are 0.8/0.8/1.1 m away), i.e. the NPC sits *on* the driver seat — not floating
   next to it, not in a passenger seat. The old wording
   (`driver-seat occupancy NOT proven`) is gone: `isOccupied` can never prove an NPC,
   so the proof is now the root-to-seat measurement.

   > **Label note:** every quoted log/output line in this README keeps the label the
   > build printed at the time (`npc body-to-seat`); the spike now prints the same
   > measurement as `npc root-to-seat` / `root-to-seat`. Only the label changed —
   > the measurement (`npc.transform.position` → seat transform) is identical.

**Corollaries measured in the same session:**

* `LandVehicle.IsOccupied` (the *vehicle* flag) **is `True` with only the NPC
  aboard** (`veh.IsOccupied=True veh.LocalPlayerIsInVehicle=False` right after
  `NPC.EnterVehicle`), while every *seat* flag on that vehicle stays `False` — the
  two flags answer different questions.
* `LandVehicle.GetFirstFreeSeat()` still returns `seat[0]` (the driver seat) while
  the NPC sits there, and returns `seat[1]` after the local player takes `seat[0]` —
  free-seat selection is Player-based as well.
* After `taxi ride` the dump reads
  `seat[0] ... isOccupied=True playerOccupant='Dominik (...)'` — the flag flips for
  the Player. That is the live confirmation of point 1.

**Index mapping `OccupantNPCs[i] ↔ Seats[i]`:** consistent with every measurement
(the single occupant sits on `Seats[0]`, the only `isDriverSeat=True` seat, and
fills `OccupantNPCs[0]`), but **not directly observable** from managed code — 0.4.7f6
has no per-NPC seat index (`NPC.CurrentVehicleSeat` does not exist; `VehicleSeat`
only stores a `Player`), so the mapping is verified for the **single-driver case
only**. Seating a *second* NPC to check `OccupantNPCs[1] ↔ Seats[1]` is an open
follow-up, not a claim.

**Consequence for "the player rides along":** because seat choice is Player-based,
`LandVehicle.EnterVehicle()` puts the local player on `seat[0]` as well — while both
are aboard, player and NPC share the driver seat (see Known limitations).

## Ride with player (live-verified 2026-09-25)

The core feature — **the vanilla NPC drives while the player rides along** — is
verified in one session. Sequence: `F15` first, `F9` (ride) while steps 1/2 are
still running, then let it drive:

> **⚠ Pruned source (run 1) — the table below is a transcript, not greppable
> evidence.** It was written from the first session's log (16:39–16:43), which
> MelonLoader has since pruned; that log no longer exists on disk. The re-greppable
> evidence is run 2 (*Second run — still on disk*, below).

| Time | Log line |
| --- | --- |
| 16:39:16.499 | `F15 pressed — spawn → npc → go to vanilla road target (-131.4, -4.0, 51.9)` |
| 16:39:17.013 | `Spawned vehicle: code='shitbox' ... position=((-74.4, 4.0, 84.9)) forward=((-1.0, 0.0, 0.3)) groundSnapped=True` |
| 16:39:17.502 | `Nearest usable NPC: id='benji_coleman' distance=5.5m` → `[after enter] seat proof: ... body-to-seat [0]=0.0m ... -> driver seat claimed` |
| 16:39:18.173 | `F9 → taxi ride` — **1.7 s after F15**, i.e. after the npc step (+1 s) and before `go` (+2 s) |
| 16:39:18.218 | `[primitive c] PROOF: LocalPlayerIsInVehicle=true — the local player is in the spike vehicle.` + `[after ride] seat[0] ... isOccupied=True playerOccupant='Dominik (...)'` |
| 16:39:18.510 | `VehicleAgent.Navigate dispatched — polling every 0.5 s plus an ENavigationResult callback` |
| 16:39:19 → 16:40:04 | `[nav t=…] … onVehicleGraph=True … speed=2.5 → 20.6 km/h`, vehicle position advancing across the run (≈150 m of road covered with the player aboard) |
| 16:40:04.010 | `[nav] timeout after 45.5s … callback result=Stopped after 45.5s` (this first dispatch drove *away* from the target first) |
| 16:41:16 / 16:42:31 | two further `F13` dispatches — the player stays aboard the whole time |
| 16:42:45.720 | **`[nav] callback result=Complete after 14.5s`**, last poll `distToTarget=5.8m` |
| 16:43:21.310 | `F9 → taxi out` → `[primitive c] PROOF: LocalPlayerIsInVehicle=false` — the player steps out **at the destination** (≤ 8 m from the raw target) |

* **What is proven:** the vehicle covers road distance (`speed > 0` and a position
  that advances across the run in the `[nav t=]` polls) **and** the player stays
  aboard the whole trip — proven as an **absence**: the log contains **no
  `ExitVehicle` / `LocalPlayerIsInVehicle=false` line between the ride (run 2:
  17:32:54) and the `F9 out` (run 2: 17:34:39)**; the run ends with
  `callback result=Complete` and `F9 out` leaves the player standing at the target.
* **Ordering matters (F9 vs. F15):** `F15` runs `Cleanup` first, and `Cleanup` calls
  `LandVehicle.ExitVehicle()` whenever `LocalPlayerIsInVehicle` is true — pressing
  `F9` *before* `F15` would unload the player again (and during cleanup/the 0.5 s
  deferred respawn `SpikeState.Vehicle` still points at the vehicle that is about to
  be destroyed, so an `F9` inside that window would board the dying vehicle). The
  working order is therefore **F15 → F9 after step 1/2** (the ride landed at
  F15 + 1.7 s in the verified run).
* **Long trips need more than one dispatch:** `SpikeRunner.NavigationTimeoutSeconds`
  is 45 s, so a ~100 m trip was cut off twice (`callback result=Stopped`) and
  completed on the third dispatch. This is the same cap with or without a passenger.

**Second run — still on disk (`MelonLoader/Logs/26-9-25_17-23-11.log`, the driver is
the vanilla NPC `chloe_bowers`):** MelonLoader prunes old logs at every start (the
first session's log no longer exists), so this run is the one anyone can re-grep:

```
[17:32:51.896] F15 pressed — spawn → npc → go to vanilla road target (-131.4, -4.0, 51.9).
[17:32:52.915] Nearest usable NPC: id='chloe_bowers' distance=3.6m (registry=107, skipped=1)
[17:32:52.930] [after NPC.EnterVehicle] npc body-to-seat [0]=0.0m [1]=0.8m [2]=0.8m [3]=1.1m -> closest seat[0] (0.0m)
[17:32:52.933] [after enter] seat proof: seat[0] isDriverSeat=True ... NPC 'chloe_bowers' at OccupantNPCs[0] (filled=1/4);
               ... -> driver seat claimed. VehicleSeat.isOccupied=False is expected: VehicleSeat.Occupant is typed Player ...
[17:32:53.921] VehicleAgent.Navigate dispatched — polling every 0.5 s plus an ENavigationResult callback …
[17:32:53.981] F9 → taxi ride.                              (F15 + 2.09 s, i.e. inside step 1/2)
[17:32:54.031] [primitive c] PROOF: LocalPlayerIsInVehicle=true — the local player is in the spike vehicle.
[17:32:54.032] [after ride] npc.IsInVehicle=True ... veh.LocalPlayerIsInVehicle=True
[17:32:54.032] [after ride] GetFirstFreeSeat() -> 'GameObject (1)' isDriverSeat=False   (seat[0] is now taken by the Player)
[17:32:54.032] [after ride] seat[0] ... isOccupied=True playerOccupant='Dominik (76561199109519851)'
[17:32:54.033] [after ride] npc body-to-seat [0]=0.0m ... -> closest seat[0] (0.0m)     (NPC still on the driver seat)
[17:32:54.033] [after ride]   occupantNpc[0] id='chloe_bowers' isInVehicle=True
[17:32:54.034] [after ride] LandVehicle.OccupantNPCs capacity=4 (array length = seat count, NOT occupancy) filled=1 thisNpcAtSlot=0
[17:32:54.421 → 17:33:25.225] [nav t=0.5s … t=31.3s] position advances across the run
               (stationary for consecutive polls while reversing at ≈0 km/h — identical positions at
               t=5.6/6.1/6.6s and t=14.2/14.7s), speed 2.0 → 20.6 km/h,
               distToTarget 26.9m (first poll) → 5.3m (last poll), peaking at 33.1m around t=24s,
               onVehicleGraph=True from t=16.7s
[17:33:25.476] [nav] callback result=Complete after 31.6s (frame=51317) — authoritative completion signal, navigation polling stops.
[17:34:39.578] F9 → taxi out.
[17:34:39.624] [primitive c] PROOF: LocalPlayerIsInVehicle=false — the local player left the spike vehicle.
```

Two chase-camera screenshots from that live session — one mid-ride, one right after
`F9 out` — are described here rather than linked: **screenshots from the live
session (Hermes cache, since pruned; no file path, not part of this repo)**. The
mid-ride one shows the taxi driving on the road with the HUD prompt
**`E Exit Vehicle`** (i.e. the local player is aboard, first person would not show
that prompt) at 9 km/h; the one after `F9 out` shows the player on foot beside the
taxi on the road at the target (no vehicle prompt). The claims above rest on the
log lines, not on the captures.

## Taxi stand + F17 order flow (Stage 3b — live-verified 2026-09-25)

Version stays **0.1.0**. No W/S self-drive claim is made here.

* **Taxi stand `ParkingGarage` at `(-3.2, 0.0, 82.0)`** — the parking-lot dump
  lists 33 lots; `ParkingGarage` is the only garage candidate. Stand resolution
  logs the rule `configured coordinate within 10m (0.1m)` and spawns at
  `(-13.0, 0.0, 84.2)` with forward `(0, 0, 1)`. The `Shitbox` spawns at
  `(-13.0, 0.2, 84.2)` with a Foundation ground snap (2.9 m). Visual swap DONE:
  21 renderers, 10 deactivated.
* **F17 flow (live):** NPC `chloe_bowers` (3.6 m) boards — `IsInVehicle`,
  `OccupantNPCs[0]`, root-to-seat 0.0 m. Road target CHOSEN label `player`,
  destination `(-122.4, -3.9, 64.6)`, snapDelta 1.8 m, distanceToPlayer 1.8 m,
  22 candidates, `endAtRoad` / `ensureProximity` / `teleportIfFail`. Polls show
  `distToTarget` 20.9 → 6.9 m with `onVehicleGraph=True`, `stuck=False`,
  `reversing=False`, speed 11–20 km/h. Arrival:
  `taxi arrived at player (callback=Complete after 46.9s, 6.3m from resolved
  target (-122.4,-3.9,64.6))`.
* **Timeout 45 s → 90 s** (`SpikeRunner.cs`, `NavigationTimeoutSeconds`):
  two earlier dispatches stopped at 13.2/13.7 m on the 45 s budget; 90 s
  covered the run with 46.9 s.
* **Arrival threshold unchanged at 10 m** — 6.3 m < 10 m, i.e. ARRIVED.
* **Side finding:** the mod menu did not load on the first beta click and
  loaded on the second.

## Known limitations (Stages 1–3)

* Server-authoritative only (`InstanceFinder.IsServer`); in singleplayer the
  local client is the host, so this fails only when no save is loaded yet.
* The NPC is seated with `NPC.EnterVehicle(null, veh)`; the seat it lands on is now
  **measured** (root-to-seat distance in the seat proof) instead of inferred —
  in every run so far the NPC root sat exactly on `Seats[0]`, the only
  `isDriverSeat=True` seat. `VehicleSeat.isOccupied` stays `False` for it by design,
  so `OccupantNPCs` plus the distance remain the proof.
* **`OccupantNPCs[1] ↔ Seats[1]` is still open:** the index mapping is consistent
  with every measurement but only verified for the **single-driver case** (0.4.7f6
  has no per-NPC seat index), so seating a second NPC to check it stays an open
  follow-up, not a claim — see
  [Driver seat semantics](#driver-seat-semantics-live-verified-2026-09-25).
* **The player takes the driver seat too.** Seat choice (`GetFirstFreeSeat()` /
  `LandVehicle.EnterVehicle()`) only considers *Player* occupants, so with the NPC
  already at `seat[0]` the player lands on `seat[0]` as well and the two bodies
  overlap while both are aboard (`[after ride] seat[0] isOccupied=True
  playerOccupant='Dominik ...'`). No public API selects a specific seat
  (`LandVehicle.SetSeatOccupant` is private), so "player in a passenger seat" is
  not expressible with the current native surface — a later stage would need a
  seat-index entry point or a passenger-only variant.
* **A spawn can wedge the vehicle.** `taxi spawn` always places the car `forward * 6`
  in front of the player; at the gas-station forecourt that spot sits in a wall
  corner (next to the shop's ATM), and after the in-place turn toward the target the
  vehicle was pressed against the wall (`forward` read as `(0.1, -1.0, 0.1)` before
  the orientation force, `speed=0.0 km/h` for the whole run, `callback result=Stopped
  after 45.4 s` — **twice: once with the player aboard and once without**, so the
  passenger is not the cause). Re-orienting the player so the spawn lands on the open
  road fixed it (same code, same run sequence, driving normally afterwards).
* **90 s navigation budget per dispatch** (`SpikeRunner.NavigationTimeoutSeconds`,
  raised 45 s → 90 s in Stage 3b: two dispatches stopped at 13.2/13.7 m on the
  45 s budget, 90 s covered the F17 run with 46.9 s):
  trips longer than roughly 100 m are cut off with `callback result=Stopped` and need
  another `F1`/`F3` dispatch to finish. Applies with and without a passenger.
* **The driver cannot be seen from outside when the Stage-2 swap is on:**
  `taxi.glb`'s `Glass` material has no `alphaMode` (→ OPAQUE) and
  `baseColorFactor = [0.05, 0.08, 0.10, 1.0]`, so the windows are solid — a
  positioning problem cannot be observed through them. With the vanilla body
  (`F4` off) the glass is dark-tinted and the unlit cabin at 04:00 in rain is pitch
  black; the occupant was only discernible when the door stood open right after
  exiting. The seat proof's numeric distance is therefore the authoritative
  "who sits where" evidence.
* No route planning, no fares, no payment, no NPC-to-door pathing — those are
  later stages.

## Known limitations (visual model swap)

* **Visual only — vanilla damage/repaint state is invisible:** the GLB is a plain
  low-poly model with fixed materials. Vanilla colour changes, dirt, dents and the
  damage smoke are rendered on the vanilla renderers, which are switched off, so
  they no longer show (the nodes themselves still exist and still work).
* **The GLB wheels do not rotate.** The four wheel meshes are static children of the
  GLB; the vanilla wheel objects are the ones that are animated by the wheel
  colliders, and those are hidden. Same for headlight/indicator/brake-light logic —
  the GLB has static emissive-less meshes for them.
* **`VehicleSound`, `OwnedVehiclePoI`, `TrunkGrid`, `TrunkInteraction` stay active**
  on purpose (they carry no renderer). Their interaction volumes still match the
  vanilla `Shitbox` body, so trunk access lines up with where the vanilla car would
  be — the GLB body occupies the same footprint, but the trunk opening itself is
  only visually approximated.
* **Auto-alignment is a no-op on the current build** (see the stale-bounds guard
  above): on the spawn frame `Renderer.bounds` still describe the pooled instance's
  previous position, so the measured offset is rejected and the GLB keeps
  `localPosition = 0`. For the `Shitbox` that placement is verified correct;
  another vehicle model with a different `vehicleModel` pivot would need the
  alignment re-measured on a frame *after* culling.
* **`taxi visual off` only affects the next spawn** — a vehicle that already exists
  keeps its swapped visuals until it is cleaned up and respawned.
* The GLB is loaded fresh on every spawn (45 KB, one `LoadFromFile` call); there is
  no template/instantiate cache yet.
