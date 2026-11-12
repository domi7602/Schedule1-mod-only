# Compatibility

Current build and gameplay evidence for active mods. Version/acceptance data comes from the
[AGENTS inventory](../AGENTS.md#2-mod-inventory) and per-mod changelogs; the mod's changelog
wins if records disagree. The [complete earlier record](history/compatibility-history-20261024.md)
preserves historical releases, all original matrix notes, hashes and limitations.

## Reading the evidence

- **Verified** means a real-game check or explicitly attributed owner confirmation. Compilation,
  unit tests, installed hashes and a loader entry establish narrower facts.
- Owner confirmations without itemized actions remain broad confirmations; do not manufacture
  individual test passes, exact sessions or additional log evidence from them.
- The matrix retains each check's recorded game version. A September baseline does not become
  an f12 gameplay check just because f12 is installed today.
- **WorldLook is owner-confirmed finished as of 2026-10-25.** Dominik reported TownHallInterior
  locally finished on 2026-11-10; the agreed cabinet and hall-storage repair was also accepted
  after Save/menu reload that day. The local build remains non-distributable. The earlier
  business-probe records below retain their historical scope. Owner-accepted work stays closed;
  unitemized checks and missing quantitative measurements are evidence limits, not an open backlog.
  **Multiplayer is out of scope by owner decision (2026-10-24):** preserve fail-closed authority
  and refusal guards; label untested MP behavior unverified, not open acceptance work.
- This page was shortened on 2026-10-24 without changing gameplay acceptance; repository
  checks do not constitute new gameplay evidence. The archive is evidence history, not the
  current backlog.

## Toolchain

| Component | Current reference | Evidence and limits |
|---|---|---|
| Schedule I | v0.4.7f12, Steam build 25796331; Unity 2022.3.62f2 | 2026-10-08 log/manifest. Local decompiles were regenerated from the matching live assembly (2,234 files); they are gitignored research, not build inputs. |
| MelonLoader | 0.7.3 | `net6` runtime. |
| S1API | 3.2.2 stable | Official IL2CPP + loader installed/hash-verified 2026-11-02, source `6b6d7ba`. No-deploy solution build 0 warnings/errors; 4,157 tests + 265 console checks passed, concrete 3.2.1 → 3.2.2 ApiCompat clean. Matching loader/runtime hashes and TownHallTest startup/load observed 2026-11-05; phone opening/closing subsequently owner-confirmed without problems. Limited startup/load/phone baseline complete, not individual app or broad API/gameplay acceptance; per-mod 3.2.1 acceptance below remains historical. [Upgrade and rollback](s1api-upgrade-3.2.2.md), [bounded live baseline](../Source/Mods/TownHallInterior/docs/launch-preparation.md). |
| S1MAPI | 2.0.1 | Installed IL2CPP reference for the mods that use it; original assembly hash and source pin are archived. |
| Mod compiler contract | `net6.0`, C# 12, nullable enabled | Shared `Source/Mods/Directory.Build.props`; current SDK/setup instructions are in [DEVELOPERS](../DEVELOPERS.md#requirements). |

Exact current installation details belong in [AGENTS §1](../AGENTS.md#1-setup). Previous game
baselines, full reference hashes and the superseded local S1API compatibility build are retained
in the [historical toolchain record](history/compatibility-history-20261024.md#toolchain).

## Verification matrix

The first six columns retain the original current-row values; Notes summarize the evidence and
link to its full record. Local MCP acceptance tooling is separate from GitHub-hosted CI,
which cannot run the game.

| Mod | Version | Verified on | Date | Needs S1API | Needs S1MAPI | Notes |
|---|---|---|---|---|---|---|
| WorldLook | 0.3.0-dev.5 | v0.4.7f12 | **Verified, owner-confirmed 2026-10-25** (finished in the last session; additional actions not itemized) | yes (read-only native weather wrapper; Shared) | no | Completion confirmed by Dominik; prior dev.2 water/ON-OFF and dev.3 night acceptance remain their own evidence. Accepted dev.2 DLL `410fecf7…` / unchanged bundle `29557b87…`; dev.3 `cc9a206a…` and dev.4 `5522f394…` results/failures retained. Installed dev.5 DLL `3f46ae88…` repairs shader-cache lifetime. Four same-process Dominik reloads (two ON, two OFF followed by ON) passed actual shader/support readbacks; eight renderer samples and ten scene/menu screenshots retained. No shader warning; profile and 44 native-save files unchanged. First cold-load status transport failure was recovered by an independent read, without repeating the load. No separate visible-warmth verdict or quantitative frame-time measurements were supplied; these limits do not reopen acceptance. [Current scope and evidence](../Source/Mods/WorldLook/docs/in-game-acceptance.md), [prior water attribution](../Source/Mods/WorldLook/docs/water-surface-verification.md). |
| TownHallInterior | 0.1.0 | v0.4.7f12 | Local completion and agreed storage repair **Verified, owner-confirmed 2026-11-10**; interior accepted 2026-10-30 | yes (diagnostic builds; native proxies and Shared) | no | Local, non-distributable hall and business, reported finished by Dominik on 2026-11-10. Cabinet and hall-storage contents after Save/menu reload are owner-confirmed; the agreed repair is closed. [Acceptance and limits](#townhall-lager--2026-11-10-eigentümerabnahme-abgeschlossen). Entrance, EG and OG accepted; cellar abandoned. **Historical probe evidence through 2026-11-04:** Schema-2 first load/menu reload passed. Inactive candidate 52bb577a passed native construction, reference checks, JSON serialization and cleanup on TownHallTest at 14:04:58 CET; ten registry fingerprints unchanged. Activation candidate 54d37fe5 crashed before slot load on shared native code detours, was withdrawn and compile-blocked. Verified predecessor 52bb577a restored at 14:53:53 CET; 39 Slot-2 and 38 Slot-1 native files unchanged. Subtype candidate 659ee6a2 failed at native type registration on 2026-11-04 before creating an instance; ten registries and 13 foreign VTables unchanged. The scoped NetworkConnection correction 59484a66 passed native own-subtype/Property dispatch at 04:04:16 UTC on 2026-11-04, preserving ten registries and 14 foreign VTables; same-process reload at 04:08:48 UTC confirmed one-shot skip. Native Slot2/Slot1 files unchanged. Separate inactive ISaveable.InitializeSaveable dispatch probe `cb98bc95` failed at 04:53:58 UTC on 2026-11-04 before the interface call (`interface-target-or-counter-invalid`); rejected field not recorded. Property dispatch and own cleanup succeeded; ten registries and 15 foreign VTables unchanged. Follow-up candidate `853a1178` recorded `interface-resolved-flags`: the own injected method matched the pointer but was not flagged virtual. Correction `a8d45f2a` observed one inactive `ISaveable.InitializeSaveable` call at 06:58:38 UTC on 2026-11-04, with counters 0 to 1, property counters remaining 1/1, ten registries and 15 foreign VTables unchanged, and own cleanup verified. Same-process reload at 07:00:49 UTC returned skipped/attempt-already-consumed with no new registration. [Interface contract](../Source/Mods/TownHallInterior/docs/business-interface-probe.md). [Subtype contract](../Source/Mods/TownHallInterior/docs/business-subtype-probe.md). Inactive `SaveManager.RegisterSaveable` route observed at 07:11:05 UTC on the Dominik / SaveGame_1 slot (mode `inactive-saveable-registration`, DLL `473064e0`): one in-memory registration added and immediately removed (Before 404 / Added 1 / Remaining 0); Save/WriteData never called; registries, 14 foreign VTables and native files unchanged; own cleanup verified. [Manager report](../Source/Mods/TownHallInterior/docs/evidence/business-manager-live-register-saveable-20261104.json). At that probe stage, activation, full initialization, early registration before restore, station, purchase and persistence remained open; this historical list does not reopen the later owner-accepted work. [Crash and next step](../Source/Mods/TownHallInterior/docs/business-activation-probe.md), [construction result](../Source/Mods/TownHallInterior/docs/business-construction-probe.md), [interior history](../Source/Mods/TownHallInterior/docs/README.md). No distributable release archive. |
| LightControl | 0.1.1 | v0.4.7f12 | **Verified, owner-confirmed 2026-10-24** for 0.1.0 (confirmation supplied retroactively; actions not itemized); 0.1.1 is a log-only Rainbow-tick fix deployed 2026-10-24; 0.1.0 installed and loaded 2026-10-24 (`list_mods`) as the stable release of dev.19 (dev.19 installed 2026-10-22) / partial visual check | yes (3.2.1 live reference) | no | Graphite UI on the owned 1201x655 canvas; ten Cancel/Apply colors including Rainbow, seven immediate brightness steps and distinct widget callbacks. The dev.19 evidence includes 264 checks, clean Release/review, verified deployment backups and native screenshots. Arial fallback remains active. Exact font parity and interaction/lifecycle acceptance belong to the owner-confirmed 0.1.0, with actions not itemized; no new native pass is claimed for the 0.1.1 logging-only update. [Evidence](../Source/Mods/LightControl/docs/graphite-ui-verification.md), [changelog](../Source/Mods/LightControl/docs/CHANGELOG.md). |
| PropertyLEDs | 0.1.11 | v0.4.7f12 | 35-room catalog/reload **Verified 2026-11-10**; missing-layout UI and unowned roots **Verified 2026-11-11** (prior v0.1.10: **Verified, owner-confirmed 2026-10-25** (prior v0.1.8: **Verified, owner-confirmed 2026-10-24**, actions not itemized)) | yes | no | **Bounded live verification 2026-11-10** (`TownHallTest` / `SaveGame_2`, `PID 10708`, DLL `D5C456E0…`): `Latest.log` verified `Layout catalog: 35 valid rooms, 35 marked verified`, `Built 4 LED room groups; 0 colliders, 0 realtime lights (emissive visuals only)` and `LED runtime ready for slot 2: 2 owned properties/businesses` on initial load and after a `Main -> Menu -> Main` reload; live `LEDDiagnosticBridge` (`PropertyLEDs_Diagnostics`) `Status()` and `CatalogAudit()` readbacks confirmed `rv` (`0` rooms), `seweroffice` (`4` verified rooms: `hallway-1`, `hallway-2`, `office-1`, `office-2`) and all 19 native property/business entries matching `35` verified rooms across 12 properties with zero dangling catalog IDs and `diffCount=0` across all 39 `SaveGame_2` files. **2026-11-11 follow-up:** RV showed `No LED layout available` with a disabled switch, and Sewer Office restored the switch. In a separate run on PID 5540, all 14 unique native building roots were read individually. Sewer Office held four groups, RV and all 12 unowned roots held zero. Site captures are views from the native spawn points. Prior v0.1.10: Dominik explicitly confirmed the LED edges are correct (`Latest.log` 14:53:44–15:11:59, DLL `31E6E129…`, 35 verified rooms, 5 LED groups for slot 1). [Record](../Source/Mods/PropertyLEDs/docs/implementation-status.md), [changelog](../Source/Mods/PropertyLEDs/docs/CHANGELOG.md). |
| NotesApp | 1.0.3 | v0.4.6f13 | 2026-09-15 | yes | no | [Changelog](../Source/Mods/NotesApp/docs/CHANGELOG.md); recorded September game baseline retained. |
| CalculatorApp | 0.2.3 | v0.4.6f13 | 2026-09-15 | yes | no | [Changelog](../Source/Mods/CalculatorApp/docs/CHANGELOG.md); recorded September game baseline retained. |
| PotScanner | 0.7.0 | v0.4.7f12 | 2026-10-11 | yes | no | Core dashboard verified 2026-09-15; the 0.7.0 shared-palette view rewrite was verified 2026-10-11. [Changelog](../Source/Mods/PotScanner/docs/CHANGELOG.md). |
| BankApp | 0.5.0 | v0.4.7f12 | 2026-10-11 | yes | no | The 0.5.0 single-screen UI and 0.4.5 withdrawal-cap fix were verified 2026-10-11; 0.4.4 verification was 2026-09-15. [Changelog](../Source/Mods/BankApp/docs/CHANGELOG.md). |
| CustomSkateboard | 1.1.5 | v0.4.6f13 | 2026-09-15 | yes | no | [Changelog](../Source/Mods/CustomSkateboard/docs/CHANGELOG.md); recorded September game baseline retained. |
| MoreSaveSlots | 1.0.12 | v0.4.6f13 | 2026-09-15 | yes (`S1API.Utils.EventHelper`) | no | Harmony patches on the save menus. [Changelog](../Source/Mods/MoreSaveSlots/docs/CHANGELOG.md). |
| BusinessIncome | 0.1.6 | v0.4.6f13 | 2026-09-19 | yes | no | [Changelog](../Source/Mods/BusinessIncome/docs/CHANGELOG.md); recorded September game baseline retained. |
| PocketShop | 0.3.9 | v0.4.7f12 (running) | **Verified 2026-10-24** (owner-confirmed) | yes | no | The 2026-10-24 Transform→RectTransform proxy failure was fixed in both item-grid and app UI construction; the regression guard now scans all src files. 64 tests and clean Release/deployment. Owner-confirmed 08:21–08:23 run opened the directory and item grid without `ShopCatalog subscriber failed`; supplier shops stayed hidden. Last itemized purchase acceptance remains 0.3.2 on v0.4.6f13 (2026-09-20). [Changelog](../Source/Mods/PocketShop/docs/CHANGELOG.md). |
| StackLimitMod | 0.1.7 | v0.4.7f12 (workspace runtime) | Owner confirmation recorded 2026-10-15 | yes | no | Ingredient-stacking acceptance is owner-confirmed. Original test date, exact runtime and individual actions were not separately supplied; no new independent log check. The 0.1.6 check on v0.4.6f13 (2026-09-20) is historical. [Changelog](../Source/Mods/StackLimitMod/docs/CHANGELOG.md). |
| AutoPackagingStation | 0.4.7 | v0.4.7f12 | 2026-10-15 (owner-confirmed tested session) | yes (installed 3.2.1) | yes (`S1MAPI.Gltf.GltfLoader`) | The 0.4.6 collider A/B repaired native E/canvas selection; 0.4.7 retired diagnostic scaffolding. 66 tests, clean Release/review and matched installed hashes. The separate 0.4.7 log records one station setup, native canvas, Pack/Unpack and mode mirroring; identical GUIDs do not prove two stations, and its empty pre-placement save is not a reload test. Owner accepted the tested session; additional actions were not itemized. Legacy 0.4.1 cauldron-save migration remains unverified. MP guards remain unverified and out of scope. [Evidence](../Source/Mods/AutoPackagingStation/docs/interaction-diagnostics.md). |
| HitmanPhone | 0.3.4 | v0.4.7f12 (running) | SP load-gate/reload **Verified 2026-11-10**; dead-drop progression **Verified 2026-11-11** (prior v0.3.2: **Verified, owner-confirmed 2026-10-25** (prior v0.3.0: **Verified, owner-confirmed 2026-10-24**, actions not itemized; 0.3.0 loaded 2026-10-24 (`list_mods`); 0.2.9 historical 2026-09-24)) | yes (3.2.2 live reference) | no | **Bounded live verification 2026-11-10** (`TownHallTest` / `SaveGame_2`, `PID 10708`, DLL `F5A96146…`): v0.3.4 singleplayer load-gate fix verified with zero `"Multiplayer is not supported"` warnings across startup, initial `SaveGame_2` load (`20:09:45`–`20:10:01`) and `Main -> Menu -> Main` reload (`20:19:58`–`20:20:12`), while polaroid definitions (`current='bounty_evidence_polaroid_v2', legacy='bounty_evidence_polaroid', icon=loaded`), checkpoint restore (`restored 9247B88CF755`), journal rebind (`active=0`), storage write hook (`[Receipt] Storage write hook alive — first storage event observed`) and due bounty call dispatch (`'Viper' offering bounty on 'sam_thompson'`) executed cleanly (`diffCount=0` across all 39 `SaveGame_2` files). **2026-11-11 follow-up** (PID 5540): regular Jackal offer for Kyle Cooley, phone acceptance and native KO via S1MCP advanced the journal to the Dead-Drop objective. The Polaroid spawned with a 180-minute delivery window, and `Behind auto shop` appeared on the Minimap. Prior v0.3.2: Dominik confirmed the open 0.3.2 behavior (`Latest.log` 14:53:44–15:11:59, DLL `B03899BD…`, day-2 20:02 `donna_martin` offer → accept → kill → $150 cash → journal complete). [Audit evidence](../Source/Mods/HitmanPhone/docs/audit-repair-verification.md), [changelog](../Source/Mods/HitmanPhone/docs/CHANGELOG.md). |
| Weather | 0.4.0 | v0.4.7f12 | 2026-10-11 | yes | no | The 0.4.0 mockup rebuild was verified 2026-10-11. [Changelog](../Source/Mods/Weather/docs/CHANGELOG.md). |
| MessagesPlus | 0.4.6 | v0.4.7f12 (workspace runtime) | Owner confirmation recorded 2026-10-15 | yes (`S1API.Lifecycle`, `S1API.UI`) | no | Popup/response repair owner-confirmed; original test date, exact runtime and individual actions not separately supplied. The 2026-10-12 implementation had 25 tests, clean Release/review and matched installed files. Dark-mode checks on 2026-10-10 and 2026-09-30 remain historical. No new independent log/visual check is implied. [Changelog](../Source/Mods/MessagesPlus/docs/CHANGELOG.md). |
| ProductAppQoL | 0.1.0 | v0.4.7f12 | 2026-10-10 | no | no | Tap ±$1 and hold-repeat were verified in-game; typed input remained unaffected. Vanilla asking-price buttons were wired only for gamepad via `InputValueRamp`. [Changelog](../Source/Mods/ProductAppQoL/docs/CHANGELOG.md). |
| Minimap | 2.3.7 | v0.4.7f12 | 12-hour clock/reload **Verified 2026-11-10**; dead-drop marker **Verified 2026-11-11** (prior v2.3.6: **Verified, owner-confirmed 2026-10-25** (prior v2.3.5: **Verified 2026-10-24 (owner-confirmed, actions not itemized)**; stable release of the accepted 2.3.5-dev.1 code; optional search feature cancelled)) | yes | no | **Bounded live verification 2026-11-10** (`TownHallTest` / `SaveGame_2`, `PID 10708`, DLL `208DAEE8…`): v2.3.7 12-hour single-line header (`DAY 3 | Thu 12:42 AM`) visually verified in screenshot `capture-d87e1171d2d44b55ab5619805815f0c7.png` (`382D806F…`, `1920x1080`) and via live `S1MCPServer` readback on `Minimap_HUD_Root/Minimap_Card/DayHeader_Bar/Day_TMP` (`Il2CppTMPro.TextMeshProUGUI`: `text="DAY 3 │ Thu 4:00 AM"`, `enableWordWrapping=False`, `overflowMode="Overflow"`, `enableAutoSizing=True`, `fontSize=13.5`, `fontSizeMin=8`, `fontSizeMax=13.5`); `MinimapSettings` compact layout verified across initial load and `Main -> Menu -> Main` reload (`viewport=469.2, natural=258.2, target=469.2, columns=2`, `diffCount=0` across all 39 `SaveGame_2` files). **2026-11-11 follow-up** (PID 5540): the `Behind auto shop` marker and label were visible during the active Polaroid delivery objective. The screenshot records the marker at one instant. Prior v2.3.6: Dominik explicitly confirmed the clock line stays on one line (`Latest.log` 14:53:44–15:11:59, DLL `676719AD…`). [Changelog](../Source/Mods/Minimap/docs/CHANGELOG.md), [prior matrix](history/compatibility-history-20261024.md#verification-matrix). |
| PoliceTestMode | 0.1.0 | v0.4.7f12 | Default suppression/reload **Verified 2026-11-10**; console toggle/occupancy **Verified 2026-11-11** | yes | no | **Bounded live verification 2026-11-10** (`TownHallTest` / `SaveGame_2`, `PID 10708`, DLL `033DE3DF…`): `43/43` hooks registered at startup, default ON suppression applied on load and after `Main -> Menu -> Main` reload (`state='ACTIVE - police suppressed (TEST)' hooks=43 loaded=True singleplayer=True attempts=1/30 leases=17 officers=10 vehicles=0 playerOccupiedSkipped=0 unknownSkipped=0 checkpoints=4`, `activeTargets=0/14 curfewRaw=False/False/False`), live blocked hook counters observed across 7 native methods (`PoliceOfficer.OnTick=11355`, `PoliceOfficer.CanInvestigate=5710`, `CurfewManager.get_IsCurrentlyActive=568`, `CurfewManager.OnUncappedMinPass=563`, `LawActivitySettings.Evaluate=563`, `CurfewManager.get_IsEnabled=110`, `CurfewManager.RpcLogic___Enable_328543758=1`) and clean lease restoration verified on `Main -> Menu` (`pending=0`) and `Menu -> Main` (`pending=0`, `diffCount=0` across all 39 `SaveGame_2` files). **2026-11-11 follow-up** (PID 5540): console OFF restored all leases, ON restored suppression. While the player sat in a station-deployed car, `playerOccupiedSkipped=1` and the car stayed active. After exit the count returned to zero and the car became inactive. See [evidence](../Source/Mods/PoliceTestMode/docs/evidence.md) and [README](../Source/Mods/PoliceTestMode/docs/README.md). |
| TaxiDriver | 1.0.6 | v0.4.7f12 | Barn → Bungalow **owner-confirmed 2026-11-02**; early entry during pickup braking **log-verified 2026-11-02**; Rathaus Sedan detour **observed twice 2026-11-11** (prior v1.0.0: **Verified 2026-10-24 (owner-confirmed, actions not itemized)**) | yes | yes (`S1MAPI.Gltf.GltfLoader`) | **2026-11-12 follow-up:** pickup braking and both controlled historical departures passed. Guard `7e5bc3e0…` completed the final-leg retreat, 12 m street search and first-via direct selection through Bungalow arrival at 2.8 m after two recovery episodes, with regular RoadKeeper corrections. The normal control without the pose fixture also arrived at 2.8 m with 1 recovery and $49 paid. Natural episode 4 was not observed. [Current bounded status](#taxidriver-rathaus-departure-follow-up--2026-11-12). **Earlier local Guard `c7baa316…`, 2026-11-12.** Two natural Rathaus departures reached the regular Bungalow stop at 2.8 m after one and two recovery episodes. The prior failure poses and the new candidate-search and direct-final-leg-rescue branches were not reproduced; episode 4 was not observed. Both save slots were byte-identical. [Bounded live record](#taxidriver-rathaus-departure-follow-up--2026-11-12). **Earlier local Guard `0411e121…`, 2026-11-11.** Four Docks controls from the original call point reached the regular Bungalow stop at 2.8 m each. One used a checked detour and three used checked road snaps. The closest natural departure was about 0.6 m and 1° from the first archived departure. A separate attempt to place the taxi at an archived curb pose was refused by the physical footprint check. A controlled replay also matched the archived Docks dumpster pose, reproduced the missing via after a 7.3 m retreat, and reached the regular stop at 2.8 m after the new path check. All 1,609 TaxiDriver tests passed. **Earlier local Guards on 2026-11-11.** A controlled fourth recovery reached the regular Bungalow stop at 3.3 m after the fixture spent one prior budget entry. A moving STOP confirmed at approximately 32.9 km/h ended the ride, exited the passenger and removed the taxi. Both native save slots were byte-identical after the recorded runs. **Local Guard `8b8f6406…`, 2026-11-11:** the two original pickup request points led to regular Bungalow arrivals at 3.8 m each. The first detour started on recovery #1. The second used the game's checked direct path after the first via was refused. Actual departure positions varied; the last-budget decision was also replayed offline. **Earlier Guard `1ecd4375…`, 2026-11-11:** the Sedan detour reached the regular Bungalow stop twice (3.4 m and 4.0 m). Two other departures ended before a detour could start. STOP cancelled a pending detour calculation. Cleanup required normal E exit and a second confirmed removal request. The preceding candidate's Docks control reached the regular stop and its Barn control reached the fallback lot. The original Docks snap refusal was still open on those builds. **Earlier Guard `9d897a86…`, 2026-11-11:** a fresh Bungalow ride selected the regular roadside drop, then switched to the fallback parking lot and gave up 242.4 m before it after four recovery episodes. See the [current changelog](../Source/Mods/TaxiDriver/docs/CHANGELOG.md). **Barn → Bungalow owner-confirmed 2026-11-02** (run 12:53–12:56: Barn 5.0 m, first recovery redispatch/no reverse, later confirmed 3.2 m reverse, Bungalow 3.4 m with Complete; no GAVE UP). Requested same-taxi chain closed; not broad route reliability or early-boarding/MP acceptance. Prior offline/deployment checks: (independent delta review passed; physical forward evidence + passive post-consumer diagnostics, 1,402/1,402 local and isolated tests; only DLL/PDB replaced, target/backup hashes checked). **Owner-tested Guard `42b7e219…`, 2026-11-02, chained ride failed**: Barn arrived at 5.0 m ($18), Bungalow whole-route Success did not produce a physical start; first reverse 3.2 m, recurring reverse 2.3 m, front still blocked, GAVE UP 332.4 m short ($7). The initial no-drive policy wrongly consumes AutoDriving as physical proof; extracted-method diagnosis reproduced that decision (2 controls pass / 1 contract fails). No new production edit/deploy in this diagnosis. Earlier explicit Guard build and installed `Observe=false` were independently decompiled; 1,382/1,382 pre-run offline tests, Release 0 warnings/errors. Replaced the mistakenly labeled Observe artifact `75a6ab63…`; same-taxi chain acceptance remains open. **Historical 1.0.5 combined Guard+supervisor DLL `012330b1…` installed and loaded on 2026-11-01**, built locally from the reviewed supervisor sources with `-p:TaxiShortcutGuard=true` (0 warnings/0 errors) and deployed with the Observe DLL/PDB backed up. Barn ride from a fresh Dominik/Town Hall pickup: Guard overrode only the failed empty direct path (`0 → Infinity`, native group unchanged, `mode=Guard`), the new supervisor gate completed on a readable halt at **5.0 m / 1.0 km/h**, and the post-exit player position was **4.8 m** from the selected street point `(165.3, 0.0, 25.1)` — first confirmed Barn arrival in this investigation. A second launch on the same artifact reached **Bungalow** `3.5 m` short (callback=Complete, post-exit 2.8 m, `override=False` pass-through), and a later pickup showed the repaired recovery path (`hold-arrival` at 13.0 m → `grant-maneuver` → completion at 3.3 m). **Docks Warehouse was not ridden**: the first CALL after the Bungalow ride failed at step 1 while the previous taxi's destroy was still unconfirmed (freeze guard; confirmed 87 ms later) and the retried taxi could not be re-boarded, so Docks keeps its 2026-10-31 single observation. Fare `$0` on both rides (no full moving in-game minute). Early boarding and chained travel remain untested; multiplayer out of scope. The previously frozen supervisor artifact `8153b7b6…` is a default **Observe** build (`mode=Observe`), so version `1.0.5` alone never identifies the guard state. 79 native save files byte-identical before/after; no explicit save/sleep. One run on one locally combined build, not broad reliability. Dominik's own 19:35 Slot-1 session on the same `012330b1…` artifact also arrived at Barn (5.0 m, fare $17, normal boarding). A chained Bungalow selected without exiting then GAVE UP 334.1 m short from the Barn street end; that does not reopen the Barn arrival. Docks and early boarding remain untested in his session. Parent 1,376/1,376 tests, bounded R1 review 24/24; whole repository remains failed on unrelated global format/version drift. [Current native, deployment and route evidence](../Source/Mods/TaxiDriver/docs/native-shortcut-verification.md). Earlier `7ec8773a…` / `fb31c4e7…` failed route histories remain [here](../Source/Mods/TaxiDriver/docs/street-dropoff-verification.md) and [here](../Source/Mods/TaxiDriver/docs/navigation-settings-verification.md). Historical v1.0.0 owner acceptance remains closed. Registered-patrol rebounds, unknown native restoration and road-correction/jitter retain documented limits. [Driver record](../Source/Mods/TaxiDriver/docs/driver-binding-verification.md). |
| StorageScanner | 0.2.0 | v0.4.7f12 | 2026-10-11 | yes | no | The 0.1.0 readiness gate prevented every scan; the 0.1.1 repair was deployed 2026-10-04. Current 0.2.0 was verified 2026-10-11. [Changelog](../Source/Mods/StorageScanner/docs/CHANGELOG.md). |
| PhoneScrollFix | 2.0.0 | v0.4.7f12 | 2026-10-11 | yes | no | Hover appearance owner-verified 2026-10-11 ("jetzt ist das vanilla"): clipped slim highlight and `RectMask2D` viewport. Root cause was observed with S1MCP and the mod watcher; scene-reload re-apply was not separately retested. [Changelog](../Source/Mods/PhoneScrollFix/docs/CHANGELOG.md). |
| S1MCP | 1.0.5 | v0.4.7f12 | 1.0.4 verified 2026-10-17 (main menu, 10/10); 1.0.5 clock fix **Verified, owner-confirmed 2026-10-24** (confirmation supplied retroactively; actions not itemized) | n/a (bridge, no S1API) | no | CurrentTime is HHMM, not minutes; the claimed noon write in 1.0.4 actually landed at 07:20. The 1.0.5 contract returns `time_hhmm`, `time_minutes` and display; writes accept exactly one of hours, minutes or HHMM. 50 game-free tests, clean Release and matched DLL `03DE264F…`. Owner-confirmed; later live read `812` → 08:12 / 492 min and no-op `hhmm:836` write/readback → 08:36 / 516 min. This does not exercise every write form. The earlier 10/10 transport/menu and loaded-world read checks remain historical; stale immediate readback is the known async-apply race. A diagnostics-only status-metric race remains documented. [Changelog](../ThirdParty/S1MCPServer-master/S1MCPServer/CHANGELOG.md). |

### TownHall Lager — 2026-11-10 (Eigentümerabnahme abgeschlossen)

Kandidat `9238c44e…` um 19:26 CET bei geschlossenem Spiel lokal installiert:
Hallen-Weltbindung, Cabinet-Checkpoint, vollständige ItemSet-Payloads und dauerhafte
Pending-Vorbereitung vor nativer Save-Freigabe. 1.248 Tests lokal und isoliert,
No-Deploy-Build 0/0; native Grenzen in Replays ersetzt, keine Gameplay-Abnahme daraus abgeleitet.
DLL/PDB ersetzt; Metadaten/Icon, Shared, andere beobachtete Mods und Slot 2 unverändert.
Vier Ziel- und zwei Backuphashes unabhängig geprüft. Danach Cabinet- und Hallenfach-Inhalt
nach Save/Menü-Reload von Dominik bestätigt; vereinbarter Lagerkorrektur-Test abgeschlossen.
Prozessneustart nicht separat geprüft, kein neuer Auftrag. Keine Item-Rückbuchung. Frühere
Kauf-/Geldwäscheabnahme bleibt separat bestehen.
[Befund und Grenzen](../Source/Mods/TownHallInterior/docs/item-loss-fix-2026-11-10.md),
[Installationsnachweis](../Source/Mods/TownHallInterior/docs/evidence/item-loss-20261110/deployment-verification.json).

### TownHall Business — 2026-11-09 (Offline-Prüfung)

Kauf und Geldwäsche bleiben an TownHallTest / Slot 2 gebunden. Der Offline-Stand
speichert den Rathauszustand am verifizierten nativen Checkpoint mit gelesenen
Geld- und Zeitwerten. Tests mit nachgebildeten Spieldateien prüfen Kauf per Bank
und Bargeld, Laden nach einem älteren Save, parallele Aufträge und gestörte Saves.
Der Release-Build verwendet die geprüfte f12-Referenz. Zum damaligen Offline-Stand
stand der Spieltest der neuen Save-, Zeit- und Wallet-Anbindung aus. Der spätere
lokale Abschluss vom 10. November ist im [Changelog](../Source/Mods/TownHallInterior/docs/CHANGELOG.md)
festgehalten; dieser Absatz bleibt der historische Offline-Nachweis.
[Speichervertrag und Altstand-Übernahme](../Source/Mods/TownHallInterior/docs/business-persistence-plan.md).

### TownHall Business-Daten — 2026-11-06 (eigene Daten-/Abfrageprobe bestanden, Normalbuild wiederhergestellt)

Separater Datenmodus mit eigenen Collections, zwei Loadern, Container und
deaktiviertem Bounds-Collider; tatsächliche native SetProperty-/Wrapper-/Raumabfragen
im Adapter implementiert. 89 neue Tests, 1.166/1.166 TownHall-Tests,
sieben No-Deploy-Varianten mit 0 Warnungen/Fehlern und unabhängiger Quellenreview
bestanden; neun inkompatible/zurückgezogene Routen durch CS1029 gesperrt.
Der erste Liveversuch mit Kandidat `2e519ccf…` brach am 6. November um 14:18:16 CET
in Slot 2 mit `quarantined / data-minute-list-over-budget` beim ersten
Listener-Snapshot ab; der tatsächliche Count wurde nicht protokolliert.
Der zweite Lauf mit Kandidat `4ae6509a…` am 6. November um 15:28:53 CET zeigte
`size=889`, `capacity=1024`, `version=891`, `countProperty=889`,
`countPropertyMatchesSize=true`: Der generische Getter las korrekt, der alte
512er-Guard war zu niedrig. Status `quarantined / data-minute-list-over-budget:889`;
keine eigenen Daten oder Abfragen. 39/39 native Slot-2-Dateien nach regulärem
Alt+F4 bytegleich; Normalbuild `614b81fd…` um 15:31:02 CET wiederhergestellt.
Die kapazitätsgebundene Korrektur prüft `size <= capacity`, `capacity <= 4096`
und begrenzt den Graph auf 16.384 Knoten; `nodes`/`maxChildren` werden
diagnostiziert. Kandidat `b449c1c1…` / PDB `943b9176…`: 1.166/1.166 Tests,
sieben No-Deploy-Builds, neun CS1029-Sperren; am 6. November um 15:43:53 CET
(14:43:53 UTC) über den Helper installiert, nur DLL/PDB ersetzt, `mod.json`
bytegleich. Der dritte Slot-2-Lauf um 15:49:12 CET endet **`observed`, keine
Findings**: Listener `size=889`/`capacity=1024`/`nodes=889`/`maxChildren=0`,
`SetProperty` und Datenvorbereitung bestätigt, native Abfragen 0/0/true/false,
Cleanup bestätigt, Root zerstört; Vorher-/Nachher-Snapshots identisch
(32 Register, 19 fremde VTables). 39/39 native Slot-2-Dateien bytegleich;
Normalbuild `614b81fd…` um 15:50:44 CET wiederhergestellt. Das ist die eigene
leere Daten-/Abfrageprobe, keine vollständige Business-Initialisierung; Kauf,
Geldwäsche und Persistenz bleiben offen.
[Live-Beleg](../Source/Mods/TownHallInterior/docs/evidence/business-data-capacity-live-20261106/observation.json),
[Savevergleich](../Source/Mods/TownHallInterior/docs/evidence/business-data-capacity-live-20261106/save-verification.json),
[Rollback](../Source/Mods/TownHallInterior/docs/evidence/business-data-capacity-live-20261106/rollback-verification.json).

### TownHall Business-/NetworkObject-Bindung — 2026-11-06 (begrenzter Live-Test bestanden)

Eigene no-base NetworkObject-Unterklasse mit exakt eigener Business-Bindung,
geprüften Unity-Methodenzielen und getrennten Lifecycle-Zählern. Native Manager-
und bekannte Halteridentitäten werden frisch geprüft; leere Client-Caches,
Verbindungslisten und tatsächliche Nullreferenzen gehören zur Mutationsgrenze.
Unklare Teilwirkungen erlauben weder erneute Aktivierung noch spekulatives Destroy.
104 neue Fälle und 1.077/1.077 TownHall-Tests bestanden; unabhängiger nativer Review
hat Mutations- und Rückbaugrenzen präzisiert. Kandidat `beb088f2…` / PDB `84fa0b0f…`
um 13:05:17 CET in TownHallTest / Slot 2 beobachtet: `observed`, keine Findings,
gegenseitige Bindung, beide Callbackketten und eigener Rückbau bestätigt.
Unity-Frames 1361 → 1362; 28 Register-/Halterfingerprints und 15 fremde VTables
unverändert. 39 native Testdateien nach Spielende bytegleich. Normalbuild `614b81fd…`
seit 13:09:54 CET wiederhergestellt und unabhängig geprüft.
[Live-Nachweis](../Source/Mods/TownHallInterior/docs/evidence/business-network-subtype-live-20261106/observation.json),
[Save-Abgleich](../Source/Mods/TownHallInterior/docs/evidence/business-network-subtype-live-20261106/save-verification.json),
[Rollback](../Source/Mods/TownHallInterior/docs/evidence/business-network-subtype-live-20261106/rollback-verification.json).
Originale native Business-/FishNet-Initialisierung, RPCs, Weltregistrierung und
Persistenz sind nicht nachgewiesen. Genaue Builds, Quellhashes, Audits und isolierter
Gesamtprüfstatus: [Vertrag und Belege](../Source/Mods/TownHallInterior/docs/business-network-subtype-probe.md).

### TownHall aktiver Untertyp — 2026-11-06 (begrenzter Live-Test bestanden)

Neuer separater Buildmodus für den eigenen no-base Business-Untertyp: Aktivierung,
beobachtete Lifecycle-Zähler über echte Unity-Frames und eigener Rückbau mit
frischer Welt-/Instanzbindung. Keine NetworkObject-Komponente, Registerpublikation
oder Save-Aufrufe. 89 neue Fälle und 973/973 TownHall-Offlinetests bestanden,
fünf No-Deploy-Varianten jeweils ohne Warnungen/Fehler. Legacy-Aktivierung und
gemischte ActiveSubtype-/Manager-Defines erwartungsgemäß durch CS1029 gesperrt.
Unabhängiger Review abgeschlossen; Berichte nach Widerruf verwenden gespeicherte
Framewerte. Diese Offline-Prüfungen wurden für den Liveabschluss nicht wiederholt.
Kandidat `c07ada69…` / PDB `711863c0…` lief auf TownHallTest / SaveGame_2 um
11:55:28 CET: `observed`, keine Findings, echte Unity-Frames 2243 → 2244.
Awake, OnEnable, Start und OnDisable jeweils 1; FixedUpdate 10 Aufrufe,
OnDestroy 1. Zehn FixedUpdate-Aufrufe sind keine zehn Unity-Frames. Eigener
Rückbau bestätigt; zehn Register und 14 fremde VTables unverändert. Kein
NetworkObject, keine Weltpublikation und kein Save-Aufruf. Unabhängiger
Live-Review bestätigt die begrenzte eigene no-base Shell, keine originale
native Business-Initialisierung, Persistenz oder Weltwechselgarantie.
39/39 native Slot-2-Dateien nach Spielende bytegleich, keine neuen oder fehlenden
Dateien; leere Baseline-Verzeichnisse waren nicht inventarisiert. Seit
11:57:33 CET ist Normalbuild `614b81fd…` / PDB `8126ae65…` über den Helper
wiederhergestellt, unabhängig geprüft.
[Vertrag und Prüfbelege](../Source/Mods/TownHallInterior/docs/business-active-subtype-probe.md),
[Installationsabgleich](../Source/Mods/TownHallInterior/docs/evidence/business-active-subtype-deployment-20261106/deployment-verification.json),
[Laufzeitbeleg](../Source/Mods/TownHallInterior/docs/evidence/business-active-subtype-live-20261106/observation.json),
[Save-Abgleich](../Source/Mods/TownHallInterior/docs/evidence/business-active-subtype-live-20261106/save-verification.json),
[Rollback](../Source/Mods/TownHallInterior/docs/evidence/business-active-subtype-live-20261106/rollback-verification.json).

### TownHall Save-Integration — 2026-11-06 (begrenzte No-IO-Probe bestanden)

Getter-Diagnose erzeugt keinen falschen Save-Start mehr. Die tatsächliche
Run-Integration unterlässt Cleanup bei möglicher Registrierungs-/Save-Teilwirkung,
fehlender Freigabe, unlesbarer Vorprüfung und Kontextverlust. 884/884 TownHall-
Tests, vier No-Deploy-Buildvarianten und unabhängiger Code-/Testreview bestanden.
Die 24 neuen Fälle nutzen Produktionslogik, aber künstliche Port-/Completiondaten.
Korrigierter Kandidat `38bd7da3…` bestand nach dem Deployment am 6. November
um 09:56:01 CET (08:56:01 UTC) auf TownHallTest / SaveGame_2 genau einen eigenen
synchronen `ISaveable.Save`-Aufruf ohne Dateischreiben: Status `observed`, Findings
leer, Completion und SafeToDestroy bestätigt. Eigene Saveables vorher/nachher 0,
Start/Completion 0/0 → 1/1; Register und fremde VTables unverändert, eigener Rückbau
bestätigt. 39/39 native Testdateien gegen Checkpoint bytegleich, keine neuen oder
fehlenden Dateien; leere Baseline-Verzeichnisse waren nicht inventarisiert.
Nach geschlossenem Spiel Normalbuild `614b81fd…` / PDB `8126ae65…` wiederhergestellt.
[Laufzeitbeleg](../Source/Mods/TownHallInterior/docs/evidence/business-save-live-success-20261106/observation.json),
[Save-Abgleich](../Source/Mods/TownHallInterior/docs/evidence/business-save-live-success-20261106/save-verification.json),
[Rollback](../Source/Mods/TownHallInterior/docs/evidence/business-save-live-success-20261106/rollback-verification.json).

`NativeSaveWriteAllowed=false` bleibt bestehen. Kein vollständiger `SaveManager.Save`,
keine Persistenz-/Reload-, Kauf- oder Geldwäscheabnahme und keine native
Last-Use-/Weltwechselgarantie. Die synthetischen Fortschrittsmarker 2694 → 2698
belegen keine vier vergangenen Unity-Frames. Business-Phase 1 bleibt offen.
[Korrektur, technische Teilabnahme und vollständige Prüfgrenzen](../Source/Mods/TownHallInterior/docs/business-manager-save-probe.md).

### TownHall data-only save-operation owner — 2026-11-05 (offline model only)

The managed synthetic owner is implemented in one new BusinessConstruction block;
all pre-existing source bytes and tests remain unchanged. Facades share the slot
of one explicit process model, not an already wired OS-process singleton. Immutable
binding, ordered intent/outcome/readback/failure, monotonic revocation and bounded
callback/diagnostic records have no native capabilities or lifecycle wiring.
128 new model cases and final 809/809 TownHall tests pass. Independent V2 semantics
accepted; 33 separate probe groups (including 200 model races) pass. Sole required
fix was test whitespace, corrected with unchanged values/assertions and verified
separately; no new independent semantic review of V3 claimed. Isolated Pure remains
failed overall: 20 passed, 2 failed (own formatting now corrected; existing foreign
version drift untouched), despite 4,740 passing xUnit cases, 265 console checks and
14 tool suites. No installation/game/save access or runtime approval. All native
permissions false, three proof goals unknown.
[Implementation identity, review and limits](../Source/Mods/TownHallInterior/docs/business-save-operation-owner.md).

### Prior ownership design — 2026-11-05 (specification-only task)

Immutable operation/world/manager/participant binding, intent-before-effect,
one process slot, monotonic revocation and separate reference/write/sidecar proofs
are specified, not implemented. Constructor callbacks, partial registration,
foreign/autosaves and code-unload lifetime are explicit blockers, not solved by
quarantine or retained roots. The next implementation slice is a managed data-only
owner with synthetic messages, no runtime port or Mod wiring. Native write route
remains blocked. No source/test changes, builds/testsuites, installed-file reads,
deployment or game/save actions; prior tests/baseline/audits remain closed.
[Ownership contract and scope](../Source/Mods/TownHallInterior/docs/business-save-operation-owner.md).

### TownHall world-exit / save-abort follow-up — 2026-11-05 (static only, closed audit)

24 additional native windows byte-checked: Clean is an instance-bound
PreSceneChange listener, not a full holder drain. Loaded exit invalidates active
identity before that event; its waits concern time/player data, not own save
quiescence. SaveManager follows the persistent singleton path. Own Leaving/Stop
provide invalidation, not old-operation ownership or release acknowledgement.
Unity engine cancellation remains outside the inspected wrapper. Decision: do
not convert the synchronous transient probe into an async writer by merely
waiting for an event/frame; design own frozen operation ownership and explicit
participant lifetime first. Not implemented; no write/unregister/destroy approval.
No production/test edits, builds, installation, game/save actions or save-file
reads. Prior baseline/tests/evidence remain closed and unchanged.
[Evidence and architecture boundary](../Source/Mods/TownHallInterior/docs/native-world-exit-audit.md).

### TownHall native save completion audit — 2026-11-05 (static only, still blocked)

Matched GameAssembly/metadata were freshly mapped and 23 selected native windows
checked against actual PE bytes. Queue removal precedes the request's write
dispatch; IsSaving=false and completion precede the final SaveError check. The
restqueue branch clears CompletedSaveables, not the queue; Completed and batch
snapshots are separate reference holders. Reentrancy and world/request identity
remain part of the unproved own-instance last-use/release contract.
No save/destroy/unregister approval, production/test changes, builds, deployment,
game control, save calls or native-save file reads. Earlier tests and the closed
startup/load/phone baseline were not repeated. Native lifetime, write success
and consistency of additional mod data remain separate proof goals.
[Native evidence, independent review and limits](../Source/Mods/TownHallInterior/docs/native-save-completion-audit.md).

### TownHall startup/load/phone baseline complete — 2026-11-05 (no native save probe)

After Dominik's "geladen", the read-only capture at 07:51:25 UTC ties the fresh
process to normal DLL `614b81fd…`, S1API 3.2.2 and its expected loader via actual
loader hashes and installed-file hashes. TownHallTest / SaveGame_2 load, Main,
OnLoadComplete and hall publication are logged. Eleven PhoneApp initialization
entries are present. Phone opening/closing was not confirmed at capture time;
Dominik subsequently explicitly confirmed both worked without problems. This
closes the limited startup/load/phone baseline, not eleven individual app tests.
No ERROR/fatal line or TownHall exception in the captured log prefix. Five
warnings are retained, including a caught LightControl Segoe UI constructor
failure with logged Arial fallback; later app initialization continues.
No agent game control, bridge/console command, save invocation or native-save
file access. No new interior acceptance, business Save/Load test or broad S1API
feature acceptance. No builds/deployment while running; isolated Pure not rerun
for this documentation-only follow-up because the game remains active.
[Runtime evidence, owner confirmation and scope](../Source/Mods/TownHallInterior/docs/launch-preparation.md).

### Earlier TownHall launch preparation — 2026-11-05 (before owner launch)

The separately audited installed `cff67a85…` DLL was the legacy Slot-1/double-Save
probe, with actual startup/update wiring and native Save callsites. After explicit
owner selection, the previously checked **normal build `614b81fd…` without business
probe runtime wiring** replaced it through the regular deploy helper, without a
rebuild. Only TownHall DLL/PDB changed; metadata, Shared, S1API and other mods stayed
unchanged. A fresh private checkpoint covers TownHallTest/SaveGame_2 and local
UserData, and local CloudEnabled=0 was observed without changing Steam settings.
No game launch, loaded-artifact observation or new gameplay/native save acceptance.
[Preparation, backup limits and owner-led baseline](../Source/Mods/TownHallInterior/docs/launch-preparation.md).

### TownHall save safety — 2026-11-05 (earlier offline-only step)

The prepared Slot-1/immediate-removal/double-Save route has been removed.
The manager build targets TownHallTest / SaveGame_2 only, including explicit-target
checks, and blocks before registration or native writes until its completion and
release contract is established. The new pure state machine is not wired to a
native completion certificate. A parameterless native completion event plus idle,
empty queues and a later frame is not yet an established release contract.
No deployment, game launch, native save-file access or new gameplay acceptance.
The installed DLL hash observed on 2026-11-05 differs from the last documented
registration build; it was not replaced or equated with the current source.
[Scope, checks and outstanding native contract](../Source/Mods/TownHallInterior/docs/business-manager-save-probe.md).

### TaxiDriver Rathaus departure follow-up — 2026-11-12

**Both controlled historical departures have reached the regular stop.**
Guard `9db1ae65…` verified the pickup approach correction and the historical
`missing-via` departure (2.8 m). Guard `7e5bc3e0…` passed 1,788 TaxiDriver
tests and the independent source review, then completed the other archived
departure in session 13: the direct final leg reached a second blocker,
the bounded retreat confirmed 10.9 m net motion, and the preserved search
anchor selected the 12 m street candidate. The first-via direct preference
was consumed (`directSelected=True`), followed by the via halt and a final
path checked against both blockers. Bungalow arrival was confirmed at
2.8 m with `Complete`, using exactly two recovery episodes.

This controlled run also used three normal RoadKeeper nudges and one
internal hard snap; the free-spot rescue did not place the car. The $0 fare
at the 04:00 clock limit is no payment test.

The pose fixture was removed before session 14. A normal call from
`(60.0, 0.975, 4.7)` then reached Bungalow at 2.8 m after one recovery,
with a confirmed 2.8 m reverse, one RoadKeeper nudge and no snap. All $49
were supported by individual balance readbacks and the final cash balance
of $459,951. Player positioning and the native boarding interaction were
assisted; the taxi, traffic, recovery budget and game time were not changed
externally during the ride. Natural episode 4 was not observed and remains
an additional coverage limit. All 79 native save files remained unchanged
through session 14. Current fixes, earlier failures and
individually pinned builds are in the
[changelog](../Source/Mods/TaxiDriver/docs/CHANGELOG.md).

**Earlier bounded evidence:**

Guard `c7baa316…` (PDB `c4dd2bbf…`) remained installed after two natural
TownHallTest / SaveGame_2 rides. Calls at `(60.10, 4.83)` and `(60.17, 3.95)`
(x, z) led to actual departure positions `(65.1, -1.5)` and `(59.3, -4.5)`.
Passenger operations 2 and 4 used one and two recovery episodes and checked
vias `(52.3, 12.5)` and `(46.5, 9.5)`; both arrived 2.8 m from the
regular Bungalow stop. The two exact earlier failure poses, the new 12 m
candidate search and direct-final-leg rescue were not exercised by those
rides. A fourth recovery episode was not observed. The game was closed
without saving and both native save slots were byte-identical.

The candidate passed 1,634 TaxiDriver tests, Full 23/23 and isolated Pure
22/22 before this documentation change. This is bounded live evidence,
not broad route acceptance. [Changelog](../Source/Mods/TaxiDriver/docs/CHANGELOG.md).

### TaxiDriver Barn recheck — 2026-10-31

The owner-requested repeat on unchanged Guard `4d195888…` again failed Barn, now
**5.0 m** short after **three** recovery episodes, despite successful native path
calculation. This second Barn observation is separate from the earlier 5.5 m/four-
episode failure and does not repeat Bungalow or Docks. The taxi is visible at the
paved-road end/vegetated edge; no exact blocking collider was measured. Independent
source review identified direct-navigation halt-gate and delayed-probe motion-check
gaps. A later source candidate repaired those two gaps in managed tests; it was then
built with the native guard enabled and arrived at Barn in one run on 2026-11-01
(row above). [Evidence and candidate](../Source/Mods/TaxiDriver/docs/native-shortcut-verification.md#guard--supervisor-live-barn-run--2026-11-01).

### TaxiDriver source-review history — 2026-10-31

The then-installed `7ec8773a…` / 1.0.5 build also failed Barn at **21:39** from a fresh
Dominik/Town Hall pickup, after ordinary halted boarding: four whole calculations
failed, same street point and `true/true/false`, terminal 108.2 m. Thus the 04:00 cap
is not necessary. Hash-pinned native inspection found a missing error guard in the
optional direct-shortcut length comparison; read-only live inspection confirmed the
final failed path's non-null empty vector list. At that stage the discarded chosen
group was an instrumentation gap; the later Observe and Guard runs are in the row above.
[Native evidence and repair contract](../Source/Mods/TaxiDriver/docs/native-shortcut-verification.md).

The initial instrumented candidate had separate Observe (`dbc14e2b…`, default)
and Guard (`5d3a71dc…`, `-p:TaxiShortcutGuard=true`) artifacts, both 1.0.5. Parent full
suite: **1,344/1,344**; coder focused 75/75 and both no-deploy builds 0 warnings/errors.
Independent implementation review **rejected those initial artifacts**: five injected
revocations during final validation still triggered an override. The sources/binaries
were preserved while that boundary was corrected. They were never installed.

The R1-corrected candidate was then Observe `74c8c3fd…` / Guard `4d195888…`,
still 1.0.5 and Observe by default. Exclusive validation, post-callback frame currency
and a final managed-only receipt gate close the reproduced boundary in managed tests:
22 pre-fix assertion failures/7 controls → 29/29; focused 97/97, parent full 1,366/1,366;
both no-deploy builds 0 warnings/errors. Independent bounded R1 closure **PASS**, 24/24
additional probes, including zero override in all five original revocations. These
source checks alone do not establish native hook execution or route acceptance; the
later measured installation and gameplay outcomes are recorded in the row above.

## Retired and historical rows

GoldenHour and WaterShader now ship only inside WorldLook; GraphicsProbe is retired.
The standalone WorldLook 0.2.0 candidate and Minimap 2.3.3 are historical versions, not current
releases. Their original rows remain in the [archived matrix](history/compatibility-history-20261024.md#verification-matrix),
along with earlier native observations and their unresolved-at-the-time limitations. Later owner
acceptance does not retroactively turn each historical sub-check into a separately observed pass.

## Known version-drift risks

The 2026-09-19 Open Beta analysis identified possible avatar/API impact on PocketShop and
CustomSkateboard; these were expectations, not confirmed new breakages. NPCHealth/ragdoll changes
were handled for HitmanPhone in 0.2.9. The beta appearance-save conversion is one-way: retain save
backups before switching branches. The [original risk notes](history/compatibility-history-20261024.md#known-version-drift-risks)
remain preserved with their dates.

After a game update, regenerate local references with `Tools/bootstrap-game-references.ps1` and
compile against the live assemblies with deployment disabled. A successful compile detects some
API drift; it does not verify patch invocation, UI, persistence, visuals or gameplay.

## Updating this page

Use `Tools/bump-version.ps1` for release metadata, then `Tools/check-version-sync.ps1`. Keep the
single `## Verification matrix` heading, current row names and column positions intact. Acceptance
dates change only with actual new evidence, recorded together with the mod's changelog. Preserve
prior evidence as historical; do not rewrite the dated archive during later version bumps.
