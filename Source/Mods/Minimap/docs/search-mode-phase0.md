# Optional sewer-key search: Phase 0

Status: source/reference investigation and compile baseline complete; runtime contract and Phase 0 acceptance OPEN. No search implementation, version bump, deployment, game launch or save mutation in this slice.

## Scope and source

Owner brief: `Minimap_Suchmodus_Implementierungsplan.txt` supplied from Downloads. The complete original read was recovered from session history after Downloads disappeared and preserved at [the original owner plan](reference/minimap-search-owner-plan.txt). First delivery is an optional singleplayer sewer-key search inside Minimap, not a separate mission framework. Preserve the original key interaction and world state. A curated area must not derive its center from the pickup position. Unknown locations remain unsupported.

Starting revision: `af33b87d2ec2c78e3bfab551342af8bfd9ae7d9f`, branch `main`, pre-existing dirty workspace. Minimap source and test files had no tracked modifications at baseline. Do not create a branch or overwrite unrelated work. The upload names containing `(2)` are references to existing files, not extra source files.

## Executed baseline

User-local .NET SDK: 8.0.425. Evaluated project: `Source/Mods/Minimap/src/Minimap.csproj`, inherited `net6.0`, C# 12, nullable enabled. `Directory.Build.props` resolves the actual installed game references; `Directory.Build.targets` gates copying on `S1NoDeploy`.

Commands executed from repository root with the user-local SDK on PATH:

```text
dotnet build Source/Mods/Minimap/src/Minimap.csproj -c Release --no-incremental -p:S1NoDeploy=true -p:BuildProjectReferences=false
dotnet test Source/Tests/Minimap.Tests/Minimap.Tests.csproj -c Release -p:S1NoDeploy=true -p:BuildProjectReferences=false
```

Results: Release build succeeded, 0 warnings / 0 errors; 61 tests passed, 0 failed / 0 skipped. A second nonincremental build produced the same Minimap DLL hash. These are existing policy/wiring tests, not execution of a new search mode or Unity gameplay.

Evaluated direct assembly references: 24, all present. Shared's existing built DLL matched its installed DLL; dependency builds/deployment were disabled.

Existing `Latest.log` reports game 0.4.7f12, Unity 2022.3.62f2, MelonLoader 0.7.3 Open-Beta, S1API 3.2.1, Minimap 2.3.3. Steam appmanifest build ID: 25796331. S1API source checkout describes itself as tag `v3.2.1`; source-tag agreement alone is not binary provenance. No Schedule process was found during this investigation. The log is historical evidence, not a fresh test.

Historical log observations:
- Minimap initialization and successful HUD creation.
- Map sprite resolved from Resources, then from vanilla MapApp on a later load.
- Existing CustomSkateboard breadcrumbs show two OnPreLoad/OnLoadComplete pairs.
- This does NOT establish key state semantics or coverage of every reload path.

### Artifact identities (SHA-256)

| Artifact | Hash |
|---|---|
| Installed Assembly-CSharp interop reference | `38f45597a972887abb9219315bce41ac7666854096d76372accac4f9267f8771` |
| Installed S1API reference | `79d7af288b605f8fcd4d2582a59636670b6cafd6a8847bc31c21b2278a9f2bf3` |
| Installed S1MAPI reference | `75662770f7ef80114649b0c54b85c896734001ca2e4ae119241e0b4a75501e71` |
| Built and installed Shared | `7df38520bd509357d0998d71405029870e8b47516f6bebec7de33cfc7de6b912` |
| Existing installed Minimap, NOT replaced | `27bc72062e7c1a3b7bae6c24fd0636c066427d66ccddf529ad6ef7b192a82ced` |
| Fresh compile-only Minimap baseline | `ab584c4b0c85bbaaab0e67d2f65be9e28b58f7e873e3ecb37af1efe2c2456b65` |

Built and installed Minimap differ despite the same version. Cause is not established; do not attribute it to a source regression or silently replace the installed baseline. The S1API hash also differs from the historical abbreviated hash in AGENTS.md; preserve the observed identity rather than assuming the old provenance applies.

Local, temporary evidence is under the Hermes scratch directory `minimap-search-phase0`: evaluated reference paths/hashes (`references.json`), installed-reference type decompiles, preserved `Latest-baseline.log`, and a copy of the installed Minimap DLL. Scratch is prunable and not a durable backup. Do not distribute the game-reference decompiles or dependency binaries.

## Game-access contract: evidence and remaining conditions

The following members were inspected with `ilspycmd -t` against the installed Assembly-CSharp reference, not assumed from old GameReferences. IL2CPP wrappers prove signatures/accessors, not native implementation bodies or event ordering.

| Required fact | Observed access surface | Required validity / unresolved proof |
|---|---|---|
| Current collection state | `Il2CppScheduleOne.Map.SewerManager.IsRandomWorldKeyCollected` | Read only after confirmed load completion, valid current manager/world and contained native reads. Must observe false before pickup, true after normal pickup, and true after reloading a collected save without pickup. |
| Current target | `RandomSewerKeyLocationIndex`, `RandomWorldSewerKeyPickup`, `RandomSewerKeyLocations` | Validate index bounds, finite coordinates and actual index-to-pickup agreement. Presence of the index in the wrapper does not prove stable spawn identity. |
| Serialized key fields | `Persistence.Datas.SewerData.IsRandomWorldKeyCollected`, `RandomSewerKeyLocationIndex`; `SewerManager.Load(SewerData)` | These are candidate persistence connections, not proof of native load order or authoritative live timing. Never call setters or Load from search mode. |
| Readiness | `Persistence.LoadManager.IsLoading`, `IsGameLoaded`, `ActiveSaveInfo`, `LoadedGameFolderPath` | Main scene alone is insufficient. Unknown/missing values suspend observations and acceptance. |
| Load epochs | `S1API.Lifecycle.GameLifecycle.OnPreLoad`, `OnLoadComplete`, `OnPreSceneChange` | Source forwards native LoadManager UnityEvents. Subscribe before first loading boundary and unsubscribe on teardown. Every load, including same-world loads, must discard transition history; first reliable observation is baseline only. Specific data-only reload paths still need observation. |
| Persistent world identity | `SaveInfo.SavePath`, `SaveSlotNumber`, `DateCreated`, `MetaData`; GameData has `Seed`; MetaData has creation dates/versions | No verified durable world identity yet. The inspected SaveInfo/GameData/MetaData surfaces expose no obvious world GUID. Slot/path/name/seed/creation time must not be promoted to a guaranteed unique identity without proof. Persistence remains blocked on a conservative, tested identity contract. |
| Session classification | `Networking.Lobby.IsHost`, `IsInLobby`, `PlayerCount`, `LobbyID`, `GetSessionConnectionIdentifier()` | Members exist, but singleplayer vs a host waiting alone in an MP lobby needs runtime/native semantics. `NetworkGuard.IsHostOrSingleplayer()` deliberately accepts multiplayer hosts and therefore cannot serve as the required singleplayer-only gate. |
| Player position | Existing Minimap HUD already resolves the local player's world position | Extract/reuse only a verified valid local-player access path; search polling must not depend on HUD being created or active. No fallback to zero position. |

No existing S1API sewer-key wrapper was found by targeted symbol search. Direct game access belongs in one future adapter, with all Unity/native reads on the main thread. `WasCollected` is wrapper lifetime, not key collection. Enclose liveness/property access in exception containment because Pointer itself can throw.

## Resolved native facts (read-only assembly/metadata pass, 2026-10-18)

Evidence base: installed interop `Assembly-CSharp.dll` (`<GameDir>\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll`, SHA-256 `38f45597a972887abb9219315bce41ac7666854096d76372accac4f9267f8771`), decompiled with ilspycmd 9.1.0.7988 into scratch `minimap-search-phase0/acsharp/Assembly-CSharp.decompiled.cs` — the bare line numbers below are that file's. String literals come from `Schedule I_Data\il2cpp_data\Metadata\global-metadata.dat`. Wrapper decompiles prove member existence/signatures, not native bodies. No game launch, no save read, no code/build change.

### 1. Sewer key spawn transforms — enumerate the real array, invent nothing

- `Il2CppScheduleOne.Map.SewerManager : NetworkSingleton<SewerManager>` (line 222238) carries the complete scene-serialized candidate list `Il2CppReferenceArray<Transform> RandomSewerKeyLocations` (staged `SewerManager.cs:393-406`; field decl `:107`) and the selected `int RandomSewerKeyLocationIndex` (`SewerManager.cs:613-632`). The live object is `NetworkedItemPickup RandomWorldSewerKeyPickup` (`SewerManager.cs:378-391`). `SewerManager.Instance` is the `NetworkSingleton<T>.Instance` accessor already used at `MinimapBlips.cs:161`.
- Exactly ONE such array exists in the assembly (grep `Il2CppReferenceArray<Transform>` + sewer/key/location → only line 222606). There is no separate curated/area table, so `RandomSewerKeyLocations[i].position` **is** the real spawn set; no coordinate has to be authored or copied.
- The existing `minimap searchdev sites [offset]` path (`MinimapSearchDevelopment.cs:188-226`) already dumps `locations[i].position` under a 16-entry / 20 ms budget with `curated=false`. It logs xyz only — add `site.name` and `site.parent` name in the next approved slice so the later hint text is game-derived rather than invented.

### 2. Singleplayer vs multiplayer-host-alone classification

- `Il2CppScheduleOne.Networking.Lobby : PersistentSingleton<Lobby>` (201362) → `Lobby.Instance`. Public `IsInLobby` (staged `Lobby.cs:211-224`), `IsHost` (`:170-183`), `PlayerCount` (`:226-239`), `LobbyID` ulong (`:185-209`), backing `ILobbyService _lobbyService` (`:155-168`). Implementations `SteamLobbyService` (203282; Steamworks `LobbyCreated_t`/`LobbyEnter_t`/`LobbyChatUpdate_t`, `CSteamID`) and `MockLobbyService` (201856) both expose `IsInLobby`/`IsHost`/`PlayerCount`.
- `Shared/src/Multiplayer/NetworkGuard.Host.cs:23-37` returns `InstanceFinder.IsServer`, which is true for an MP host — confirmed unusable as the singleplayer gate.
- The game's own distinction is server-based only: `NetworkConditionalObject.ECondition { All, HostOnly }` (202215-202222); `ELoadStatus` has client-only states `WaitingForHost`/`WaitingForAuth` (173042-173051). `PlayerHealth.CanRespawnInSinglePlayer` (273095) is a gameplay static, not a session mode.
- Red herrings — do NOT gate on these: `Pong.EGameMode { SinglePlayer, MultiPlayer }` (79940-79946) belongs to the TV minigame; `StorageEntity.EAccessSettings.SinglePlayerOnly` (~457094) is a storage access setting.
- Candidate fail-closed gate: `InstanceFinder.IsServer == true` **and** `Lobby.Instance` alive **and** `Lobby.Instance.IsInLobby == false`; any exception or missing Lobby → not singleplayer. Corroborate with `Lobby.PlayerCount` (host alone = 1) and `LobbyID` (0/absent = no Steam lobby). The observer already logs `lobbyHost`/`inLobby`/`lobbyPlayers` (`MinimapSearchDevelopment.cs:156-163`). The singleplayer case is unproven until experiment R2.

### 3. Strongest available stable world binding

- **No world GUID exists.** Full field enumeration: `SaveInfo` (176952) = SavePath, SaveSlotNumber, OrganisationName, DateCreated, DateLastPlayed, Networth, SaveVersion, MetaData; `GameData` (188337) = OrganisationName, Seed, Settings; `MetaData` (191757) = CreationDate, LastPlayedDate, CreationVersion, LastSaveVersion, PlayTutorial. Nothing GUID-shaped.
- `GUIDManager` (5219-5330) GUIDs are per-saveable-object ids and `GUIDManager.Clear()` runs on every load — not a world identity. `PlayerData.PlayerCode` (195069) is a per-player (Steam-account) code, not world-unique.
- **The slot folder is reused on world replacement, so path/slot alone is insufficient:** `NewGameScreen.SlotSelected(int slotIndex)` (757386), `SetupScreen.ClearFolderContents(string folderPath)` (758022), `CopyDefaultSaveToFolder(string folderPath)` (758034), `SetupScreen.DEFAULT_SAVE_PATH` (757839), and `LoadManager.TryLoadSaveInfo(string saveFolderPath, int saveSlotIndex, out SaveInfo, bool requireGameFile)` (staged `LoadManager.cs:3541`). The folder name is fixed per slot: literal `SaveGame_` in `global-metadata.dat` (plus a `SaveGame.zip` export path), matching `Skills/schedule1-game-systems/references/02-Save-Persistence.md:12` — `<PersistentDataPath>/Saves/<SteamID>/SaveGame_<1-5>/`, with PersistentDataPath `%USERPROFILE%\AppData\LocalLow\TVGS\Schedule I` (from `Schedule I_Data/app.info` = `TVGS` / `Schedule I`).
- The binding must therefore include a regenerated world field. Candidates in order:
  1. `SaveInfo.DateCreated` — `Il2CppSystem.DateTime` (staged `SaveInfo.cs:73-84`), so sub-second `.Ticks` are available; already read as `candidateCreatedTicks` (`MinimapSearchDevelopment.cs:146`).
  2. `SaveInfo.MetaData.CreationDate` — `DateTimeData` (187700+) with Year/Month/Day/Hour/Minute/Second only (1-second resolution); corroboration and fallback when `DateCreated` reads as `default` (Ticks == 0).
  3. `GameManager.Seed` static (342961) / `GameManager.Instance.seed` (342733) — usable only if R1 shows it varies per world; do not assume it is random.
- Recommended: key = SHA-256 over (SaveSlotNumber, `SaveInfo.SavePath`, `SaveInfo.DateCreated.Ticks`, `MetaData.CreationDate`), appending `GameManager.Seed` only after R1 proves it varies; fail closed (suspend the feature, never reuse stored data) when `DateCreated.Ticks == 0`, MetaData is null, or SavePath is empty.

## Exact remaining runtime experiments

R1 — world binding (owner-approved disposable slot, backup first): start a NEW game, log `candidatePathHash`/`slot`/`candidateCreatedTicks` plus (to be added) `MetaData.CreationDate`, `MetaData.CreationVersion`, `GameManager.Seed`; quit to menu; overwrite the SAME slot with another NEW game; compare. The fields that changed are the discriminating binding. Then load the SAME world twice — those fields must be identical. Also log `LoadManager.LoadedGameFolderPath` and `SaveInfo.SavePath` verbatim to settle folder-vs-file and confirm the path is slot-stable.

R2 — session classification: observe singleplayer, MP host alone (nobody joined), and MP client. Do not prescribe or assume the values. Record `IsInLobby`, `IsHost`, `PlayerCount`, lobby identity presence and `InstanceFinder.IsServer`. A proposed gate is acceptable only if it admits the observed singleplayer case and rejects both multiplayer cases. In particular, `inLobby=false` in both solo and MP-host-alone would DISPROVE the proposed `!IsInLobby` gate. Unknown/unverified classification keeps the feature unavailable.

R3 — spawn transforms: on load complete read `locationCount`, `locationIndex`, `pickupAlive`, then run `minimap searchdev sites 0`, `sites 16`, `sites 32`, ... until `count` is covered. Expect exactly one site within ~0.5 m of `RandomWorldSewerKeyPickup.transform.position` and that it is the entry at `locationIndex` — that is the index-to-transform agreement proof. Reload the same world: same count and same positions (a different index is fine). Repeat once on a fresh un-collected world and once after vanilla pickup.

## Existing integration boundaries

- `MinimapMod.cs:124`: `OnUpdate` exits before HUD creation. Search polling must run before that guard and remain active with hidden HUD and closed phone. Use a monotonic timer rather than Time.time for the new poller.
- `MinimapMod.cs:97`: existing HUD/waypoint initialization is scene-based; it is not a verified search load boundary.
- `MinimapBlips.cs:505`: dynamic snapshot rebuild is throttled; `UpdateSewerKeys` runs at line 523. Exact-key suppression needs render-time enforcement as well as scan-time filtering so cached markers cannot flash after enabling the mode.
- Existing Shared host-authority guard accepts MP hosts; add no singleplayer claim based on that helper.
- Search persistence must be separate from config and waypoints, never serialize a completed flag as a second authority. Freeze the resolved world-specific target path with each pending write.

## Independent source cross-check

The read-only cross-check returned the same lifecycle and cached-marker hazards. Parent rechecked these additional findings against current source:

- `MinimapWaypoints.cs:16-20` serializes/deserializes with no field inclusion, while `WaypointFile` at lines 123-130 contains public fields only. Default System.Text.Json therefore omits these fields. This confirms the plan's serialization defect at source level; no owner waypoint files were read and no new executable round-trip test was run in Phase 0. Repair and RED/GREEN coverage belong to the next approved storage package, not this investigation.
- `MinimapSettingsApp.cs:654` defines four tabs; reset-color visibility is tied to numeric index 2 at lines 630 and 690. Adding Hints must preserve or explicitly replace these assumptions and update affected tests.
- `Shared/src/SafeStorage.cs:88-101` returns a boolean from `SaveAtomic`; `SaveTextAtomic` at lines 106-139 stages a temporary file, attempts a backup, then moves it over the target. Backup-copy failure is logged but does not prevent a successful primary write. Future search-store handling must distinguish primary-write success from guaranteed backup availability and retain dirty state on false.

This closes the independent source investigation, not the runtime acceptance below.

## Runtime acceptance still required

Do not mark Phase 0 complete or package a playable release until these pass:

1. Use an owner-approved disposable/test save before key collection, with backup. Do not mutate the owner's normal save to synthesize a test.
2. Observe manager validity, collection state, load readiness, spawn index, selected spawn/pickup agreement and candidate world identity. Exact-position diagnostics must be development-only and off by default.
3. Pick up the actual key through vanilla interaction. Observe collection-state timing relative to pickup disappearance, and later availability without pickup.
4. Reload pre-fund and post-fund saves; reload the same world and exercise a data-only reload. First reliable state in each new epoch must not create a success event.
5. Confirm replacement of a world in the same slot cannot reuse old personal search data.
6. Verify singleplayer classification separately from MP host/client, including a host alone.
7. Inspect possible spawn locations and physically validate one fixed curated area with height limits, interior margin, access route and truthful hint. No invented area coordinates/text.
8. Establish the same-scene performance/resource baseline. No CPU/allocation/FPS measurements have been made here.

Next coherent slice: a separately reviewed, bounded opt-in developer observation path for these facts, then owner-driven in-game collection/reload tests. No diagnostic build was installed by this Phase 0 source investigation.

## Follow-up implementation and live evidence (2026-10-18)

This section updates the earlier investigation status without retroactively claiming its acceptance.

- Waypoint serialization is repaired in the working tree using a property-based DTO in `src/Persistence/MinimapJson.cs`. The child observed behavioral RED (`[{},{}]`) and GREEN; parent independently executed the actual serializer on .NET 6.0.36: complete two-row name/XYZ/color roundtrip, damaged `[{}]` retaining empty defaults (no invented recovery), and legacy lowercase keys passed. This does not prove the deployed game uses the repair: no deployment yet.
- Correction to the child rationale: `JsonSerializerOptions.IncludeFields` **does exist on .NET 6** (parent runtime reflection returned true). Properties are a valid design choice, not a net6 compatibility necessity.
- Owner explicitly authorized loading `Tests`, creating a separate new test world without overwriting existing saves, and temporarily foregrounding the game for tests. `Tests` was backed up outside the repo before loading; it already reported collection=true with an inactive pickup. No collection flag was changed.
- Created and read back `Minimap Search Test` in previously empty slot index 2 (display slot 3); existing slots remained listed. Backed up the new save before collection under the Hermes backup directory, separately from the prunable scratch cache.
- Initial disposable world: `IsLoading=false`, `IsGameLoaded=true`; `IsRandomWorldKeyCollected=false`, selected index 43, active pickup. The pickup position matches hierarchy candidate 43 exactly. All 50 candidate hierarchy transforms were collected and counted programmatically into `live-sites.json`. The later replacement world selected index 35 and also matched its earlier hierarchy position exactly. After a normal reload the native candidate array still contains 50 entries. This verifies two index/position agreements, NOT the entire array ordering or all 50 positions across reloads.
- With explicit permission, opened a normal game lobby without invites and left it in a `finally` cleanup: solo `IsInLobby=false / IsHost=true / PlayerCount=1`; host-alone `true / true / 1`; post-leave readback `false / true / 1`. This supports those signals as a steady-state gate candidate (confirmed server+host, no lobby, exactly one player). Pending create/join/leave intervals were not captured and must remain fail-closed; do not infer transition safety from this test. MP clients remain untested and must be rejected (not a declared multiplayer acceptance).
- The owner subsequently picked up the real key normally in the disposable world. Parent read back `IsRandomWorldKeyCollected=true`; the old pickup wrapper still reports `WasCollected=false` but its Unity `gameObject` is no longer readable. The separate 180-second watch had expired while all 360 samples were still false, BEFORE the owner acted. This is before/after plus owner-confirmed interaction evidence, not continuous transition or exact destruction-order evidence.
- Native `SaveManager.Save()` was invoked only after checking the active organisation was `Minimap Search Test`; observed `IsSaving=true`, later false, then read back the exact disposable save `Sewer.json`: `IsRandomWorldKeyCollected=true`, index 43. Created a separate post-collection backup outside scratch. The original pre-collection backup was retained.
- Normal menu -> same-save reload after the first world's vanilla pickup retained collection=true. Its native creation ticks and six metadata calendar fields matched the pre-save/pre-reload observation exactly. The owner then explicitly authorized replacing ONLY the newly created slot 3 after another backup; the native overwrite dialog and setup both reported zero-based slotIndex=2 before confirmation. Created `Minimap Search Test 2` at the same path: creation ticks changed, metadata creation changed, and the key is now uncollected at index 35. Original slots 0/1 were compared programmatically across the operation (path/name/creation/last-played/networth/version); those measured fields were unchanged.
- World A saved `Game.json` seed=1717417267; world B saved seed=357314953, matching a direct live read of `Il2CppScheduleOne.DevUtilities.GameManager.Seed`. `get_member_value` can read this static property; the generic instance-only property endpoint returning null is not evidence of a null seed. After finalizing the default test avatar, saved world B and read its exact `Sewer.json` (false, index 35). Backed it up, returned to the menu, and reloaded: the same creation fields, native seed 357314953, collection=false, index 35 and native array count 50 were confirmed.
- Still open: an older pre-fund rollback of world A itself, data-only reload coverage, exact pickup timing, a physically validated fixed curated area/access route, and the performance baseline. A test-only relocation enabled the first pickup; this proves neither a walkable route nor a curated area. Raw spawn positions are not curated search-area centers.
- The observer remains opt-in development code, not a playable search mode. The subsequent `2.3.4-dev.1` development deployment/load check is recorded below; earlier native-key investigation used the older DLL.
- The Unity-free search core is now included in the normal test project. Parent observed additional behavioral RED failures and repaired cancellation restore, cross-world completion leakage, stale found/searching states during unknown observations, completion-revision changes, invalid/overflowing area tests, mode-disable intent preservation, and false-with-missing-pickup gaps. At the pre-review checkpoint all 133 Minimap tests passed, with a Release compile-only build at 0 warnings/errors and passing doc-path checks. No Unity runtime execution of this new core is claimed.
- Still unimplemented: production game-access/session/world binding, curated catalogue, Hints page, exact-marker suppression, and projected area overlay. **Versioned search-store IO with corruption/retry handling now exists** as the unwired `SearchStore` (2.3.5-dev.1, 2026-10-24; format `Version: 1`, fail-closed validation, atomic dirty retry, corrupt-primary protection), covered by 29 game-free tests. Do not describe the controller or the store alone as a complete Minimap search feature. The independent source review completed; its subsequent corrections are recorded below.

## Follow-up review disposition

- Search/observer corrections: retained intent is now explicitly readable as `RetainedRecord` while `CurrentRecord` is unbound; null is documented as NOT a deletion request. Pending intent can be cancelled before re-observation. Cancel/restore-false discard an unconsumed completion, and unsuccessful consumption returns a default signal. Vertical `Height` is documented as a half-band, and the `Unavailable` regression asserts that exact phase.
- Observer budget expiry is reportable exactly once even if a lifecycle callback or a permission check detected it before `Tick`. Site authorization no longer consumes cadence; its warning distinguishes an off/expired watch. World candidate validity now requires a nonblank path, nonnegative slot, positive creation ticks, readable metadata and a valid managed creation date; identity-read exceptions prevent key reads. This remains candidate validation, not a claim of a proven GUID.
- These changes had observed RED/GREEN regressions. Parent ran the 75 search-related tests successfully and a Release compile-only build with 0 warnings/errors. The native-wrapper guard has supplementary source-wiring checks, NOT a simulated native fault-injection claim.
- Waypoint review D1-D4/D10 is implemented in the Unity-free `WaypointStore` and the thin Unity adapter. Parent additionally observed ten RED regressions and repaired A -> B -> A stale reload, CRUD-before-context-change, unknown-context edits, faulted edits, good-backup rotation risk, `[{}]` recovery, existence-probe failures, label/control limits, bounded pending capacity and projection invalidation on blocked switches. Original-path pending snapshots are retained on failed writes; recovered backups with corrupt primaries remain read-only until explicit clear/manual repair. All 186 tests pass; this is policy/serializer evidence, not a native waypoint roundtrip.
- Thirteen raw evidence files were copied out of the prunable scratch cache to the local Hermes backup bundle `backups/minimap-search/evidence-20261018-163643/`, with source/target SHA-256 verification and a manifest. Raw save paths/Steam identifiers remain outside the repo. Native save backups before and after collection and before replacement are retained separately.

## Development deployment (2.3.4-dev.1)

Feature-diff gate before the scripted bump: existing HUD/blips/settings have their original implementation; waypoint serialization/retry/validation is implemented; the off-by-default developer observer is implemented; the tested search controller is unwired. Hints UI, production search binding/store, curated catalogue and area overlay are absent and are NOT advertised as features.

- Bumped using the repository helper (preview first), not manual source/metadata edits. Corrected inventory/compatibility status so historical acceptance is not carried onto the new version.
- Exited only the disposable test session through native UI; the game process was confirmed stopped. Preserved the previous DLL/PDB/metadata/log and verified that the existing built and installed Shared hashes match. Built/deployed ONLY Minimap with `BuildProjectReferences=false`, 0 warnings/errors; no Shared/third-party deployment.
- Installed DLL SHA-256: `501c448da2d51ae114d48aff09689f1fa74e47699c946de1933f0f62d96734ae`. DLL/PDB/metadata source/target hashes matched. Version-sync: 21 mods, no drift; doc-path and scoped format checks passed.
- Launched through Steam. The live mod registry reported exactly one Minimap at 2.3.4-dev.1. Loaded only `Minimap Search Test 2`; log confirms successful HUD creation, four settings pages initialized and the developer observer OFF. This is load/initialization smoke evidence, not visual or search acceptance.
- Owner explicitly postponed the manual in-game waypoint roundtrip. No console command was executed and no test waypoint was created. The game process was subsequently found no longer running; the attempted pause could not run because MCP was refused. It was NOT restarted after postponement. Cause of exit was not established. The inspected Minimap-scoped log lines contained no error/exception/failure match; this is not a whole-process crash diagnosis.
- Preserved the new runtime log and hash receipt outside scratch in a `backups/minimap-search/loaded-2.3.4-dev.1-*` bundle. Full waypoint gameplay acceptance remains open, as do the Phase-0 timing/access/performance gates and the unimplemented user-facing search.


