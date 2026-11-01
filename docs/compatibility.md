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
- **WorldLook is owner-confirmed finished as of 2026-10-25.** TownHallInterior is the one mod
  under active development (🧪): a local, non-distributable staging build whose in-game visual
  acceptance is open. Owner-accepted work stays closed; unitemized checks and missing
  quantitative measurements are evidence limits, not an open backlog.
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
| S1API | 3.2.1 stable | Installed 2026-10-09; current pin/installed provenance is in AGENTS §1. Historical beta.8/f11/f12 load records do not establish current patch or gameplay correctness. |
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
| TownHallInterior | 0.1.0 | v0.4.7f12 (candidate installed, never launched since) | In-Game-Verify open | no (native Assembly-CSharp proxies; Shared) | no | Local, non-distributable staging build: public reception hall behind a geometric facade portal, proximity-driven sliding entrance, vanilla-sourced reception furniture and eight local lights. The installed candidate DLL `5efb8650…` (public-threshold candidate, deployed 2026-10-29T10:11Z) matched that day's local build; no game session has been launched or visually accepted since, so installation and hashes are not gameplay evidence. Repository integration on 2026-10-29 added the mod to `S1Mods.sln`: 128 game-free tests, No-Deploy Release build 0 errors/0 warnings and a clean `dotnet format` pass. The staged, never-wired door dialog moved to `Source/Archive/TownHallInterior/DoorDialog/`; its 41 test cases left the active suite, so the earlier 169/169 result and the 39-file source fingerprint belong to the pre-archive candidate. The embedded native slices are local hash-pinned extractions and are **not distributable**; `Tools/package-release.ps1` selects this mod by directory and would package them, so no release archive may be produced from it. [Verification record](../Source/Mods/TownHallInterior/docs/enclosure-verification.md), [mod documentation](../Source/Mods/TownHallInterior/docs/README.md). |
| LightControl | 0.1.1 | v0.4.7f12 | **Verified, owner-confirmed 2026-10-24** for 0.1.0 (confirmation supplied retroactively; actions not itemized); 0.1.1 is a log-only Rainbow-tick fix deployed 2026-10-24; 0.1.0 installed and loaded 2026-10-24 (`list_mods`) as the stable release of dev.19 (dev.19 installed 2026-10-22) / partial visual check | yes (3.2.1 live reference) | no | Graphite UI on the owned 1201x655 canvas; ten Cancel/Apply colors including Rainbow, seven immediate brightness steps and distinct widget callbacks. The dev.19 evidence includes 264 checks, clean Release/review, verified deployment backups and native screenshots. Arial fallback remains active. Exact font parity and interaction/lifecycle acceptance belong to the owner-confirmed 0.1.0, with actions not itemized; no new native pass is claimed for the 0.1.1 logging-only update. [Evidence](../Source/Mods/LightControl/docs/graphite-ui-verification.md), [changelog](../Source/Mods/LightControl/docs/CHANGELOG.md). |
| PropertyLEDs | 0.1.11 | v0.4.7f12 | In-Game-Verify open (prior v0.1.10: **Verified, owner-confirmed 2026-10-25** (prior v0.1.8: **Verified, owner-confirmed 2026-10-24**, actions not itemized)) | yes | no | Dominik explicitly confirmed the LED edges are correct. `Latest.log` 14:53:44–15:11:59 loaded v0.1.10, DLL `31E6E129…`, initialized `PropertyLEDsApp`, reported 35 verified rooms and built 5 LED groups for slot 1. The log does not measure the about-1.2-percent insets; edge clearance is his confirmation, not a log measurement. An earlier same-day screenshot showed landscape only. 0.1.9 opens landscape (`EOrientation.Horizontal`); theme scale follows the short side. Prior 0.1.8 evidence: first-open current-property selection and property-named power status fixed; 115 tests. The earlier screenshot mismatch was a different selected property, not a demonstrated rendering failure. Native fitting, toggle/color and save roundtrip are owner-accepted without itemized actions; 0.1.4 acceptance remains historical. [Record](../Source/Mods/PropertyLEDs/docs/implementation-status.md). Verification evidence is historical for v0.1.10 (**Verified, owner-confirmed 2026-10-25** (prior v0.1.8: **Verified, owner-confirmed 2026-10-24**, actions not itemized)); no acceptance for v0.1.11. |
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
| HitmanPhone | 0.3.2 | v0.4.7f12 (running) | **Verified, owner-confirmed 2026-10-25** (prior v0.3.0: **Verified, owner-confirmed 2026-10-24**, actions not itemized; 0.3.0 loaded 2026-10-24 (`list_mods`); 0.2.9 historical 2026-09-24) | yes (installed 3.2.1) | no | Dominik confirmed the open 0.3.2 behavior. `Latest.log` 14:53:44–15:11:59 loaded DLL `B03899BD…` and recorded day-2 20:02 `donna_martin` offer → accept → kill → $150 cash → journal complete, plus day-3 offers for `geraldine_poon` and `peggy_myers`. No tracker app was registered. The end-of-session save assigns Donna to recruited Benji and does not assign Geraldine or Peggy; it is not a pre-offer empty-network snapshot, and the log does not print eligibility. The already moved day-5 slot was not observed being pulled back. A wrong-drop deposit kept Geraldine's polaroid until expiry. One load-window MP notice did not block later offers; MP remains out of scope. 0.3.1 removed `BountyTrackerApp`. Prior SP audit evidence stays historical: 148 checks and clean Release/Debug. On 2026-10-24 at 08:22:55.675 the log restored matching checkpoint `56391B779CBE`. Registry-before-items and GUID persistence remain covered by the 0.3.0 confirmation, not by a new itemized sample. Legacy import cannot establish world alignment retrospectively; unknown cash outcomes retain the claim without automatic retry. [Audit evidence](../Source/Mods/HitmanPhone/docs/audit-repair-verification.md). |
| Weather | 0.4.0 | v0.4.7f12 | 2026-10-11 | yes | no | The 0.4.0 mockup rebuild was verified 2026-10-11. [Changelog](../Source/Mods/Weather/docs/CHANGELOG.md). |
| MessagesPlus | 0.4.6 | v0.4.7f12 (workspace runtime) | Owner confirmation recorded 2026-10-15 | yes (`S1API.Lifecycle`, `S1API.UI`) | no | Popup/response repair owner-confirmed; original test date, exact runtime and individual actions not separately supplied. The 2026-10-12 implementation had 25 tests, clean Release/review and matched installed files. Dark-mode checks on 2026-10-10 and 2026-09-30 remain historical. No new independent log/visual check is implied. [Changelog](../Source/Mods/MessagesPlus/docs/CHANGELOG.md). |
| ProductAppQoL | 0.1.0 | v0.4.7f12 | 2026-10-10 | no | no | Tap ±$1 and hold-repeat were verified in-game; typed input remained unaffected. Vanilla asking-price buttons were wired only for gamepad via `InputValueRamp`. [Changelog](../Source/Mods/ProductAppQoL/docs/CHANGELOG.md). |
| Minimap | 2.3.6 | v0.4.7f12 | **Verified, owner-confirmed 2026-10-25** (prior v2.3.5: **Verified 2026-10-24 (owner-confirmed, actions not itemized)**; stable release of the accepted 2.3.5-dev.1 code; optional search feature cancelled) | yes | no | Dominik explicitly confirmed the 2.3.6 clock line is correct and stays on one line. `Latest.log` 14:53:44–15:11:59 loaded DLL `676719AD…` and recorded HUD creation plus M-key visibility events. The log does not measure AM/PM wrapping; the one-line appearance is his confirmation, not a log measurement. 2.3.6 formats 12-hour time locally as `h:mm AM`, shows the short weekday beside the clock and disables word wrapping. Prior row: stable, version-only release of owner-accepted 2.3.5-dev.1; 215 tests and clean Release, installed DLL `8C133F11…` matched. The owner cancelled sewer-key search: its core/store/observer stays dormant and unwired; no search UI/overlay is planned, and R1/R2/R3 experiments are not prerequisites. Earlier waypoint postponement is superseded by owner acceptance. The detailed menu/test-world startup, HUD and settings evidence belongs to 2.3.4-dev.1; it is not a new itemized 2.3.5 session. [Changelog](../Source/Mods/Minimap/docs/CHANGELOG.md), [prior matrix](history/compatibility-history-20261024.md#verification-matrix). |
| PoliceTestMode | 0.1.0 | v0.4.7f12 | In-Game-Verify open | yes | no | Standalone singleplayer test suppression. Parent: 14 tests pass, Release 0 warnings/errors. Native suppression/restoration and taxi coexistence pending. See `Source/Mods/PoliceTestMode/docs/README.md`. |
| TaxiDriver | 1.0.5 | v0.4.7f12 | In-Game-Verify open (Barn routing/early boarding; prior v1.0.0: **Verified 2026-10-24 (owner-confirmed, actions not itemized)**) | yes | yes (`S1MAPI.Gltf.GltfLoader`) | **Combined Guard+supervisor DLL `012330b1…` installed and loaded on 2026-11-01**, built locally from the reviewed supervisor sources with `-p:TaxiShortcutGuard=true` (0 warnings/0 errors) and deployed with the Observe DLL/PDB backed up. Barn ride from a fresh Dominik/Town Hall pickup: Guard overrode only the failed empty direct path (`0 → Infinity`, native group unchanged, `mode=Guard`), the new supervisor gate completed on a readable halt at **5.0 m / 1.0 km/h**, and the post-exit player position was **4.8 m** from the selected street point `(165.3, 0.0, 25.1)` — first confirmed Barn arrival in this investigation. A second launch on the same artifact reached **Bungalow** `3.5 m` short (callback=Complete, post-exit 2.8 m, `override=False` pass-through), and a later pickup showed the repaired recovery path (`hold-arrival` at 13.0 m → `grant-maneuver` → completion at 3.3 m). **Docks Warehouse was not ridden**: the first CALL after the Bungalow ride failed at step 1 while the previous taxi's destroy was still unconfirmed (freeze guard; confirmed 87 ms later) and the retried taxi could not be re-boarded, so Docks keeps its 2026-10-31 single observation. Fare `$0` on both rides (no full moving in-game minute). Early boarding and chained travel remain untested; multiplayer out of scope. The previously frozen supervisor artifact `8153b7b6…` is a default **Observe** build (`mode=Observe`), so version `1.0.5` alone never identifies the guard state. 79 native save files byte-identical before/after; no explicit save/sleep. One run on one locally combined build, not owner acceptance or broad reliability. Parent 1,376/1,376 tests, bounded R1 review 24/24; whole repository remains failed on unrelated global format/version drift. [Current native, deployment and route evidence](../Source/Mods/TaxiDriver/docs/native-shortcut-verification.md). Earlier `7ec8773a…` / `fb31c4e7…` failed route histories remain [here](../Source/Mods/TaxiDriver/docs/street-dropoff-verification.md) and [here](../Source/Mods/TaxiDriver/docs/navigation-settings-verification.md). Historical v1.0.0 owner acceptance remains closed. Registered-patrol rebounds, unknown native restoration and road-correction/jitter retain documented limits. [Driver record](../Source/Mods/TaxiDriver/docs/driver-binding-verification.md). |
| StorageScanner | 0.2.0 | v0.4.7f12 | 2026-10-11 | yes | no | The 0.1.0 readiness gate prevented every scan; the 0.1.1 repair was deployed 2026-10-04. Current 0.2.0 was verified 2026-10-11. [Changelog](../Source/Mods/StorageScanner/docs/CHANGELOG.md). |
| PhoneScrollFix | 2.0.0 | v0.4.7f12 | 2026-10-11 | yes | no | Hover appearance owner-verified 2026-10-11 ("jetzt ist das vanilla"): clipped slim highlight and `RectMask2D` viewport. Root cause was observed with S1MCP and the mod watcher; scene-reload re-apply was not separately retested. [Changelog](../Source/Mods/PhoneScrollFix/docs/CHANGELOG.md). |
| S1MCP | 1.0.5 | v0.4.7f12 | 1.0.4 verified 2026-10-17 (main menu, 10/10); 1.0.5 clock fix **Verified, owner-confirmed 2026-10-24** (confirmation supplied retroactively; actions not itemized) | n/a (bridge, no S1API) | no | CurrentTime is HHMM, not minutes; the claimed noon write in 1.0.4 actually landed at 07:20. The 1.0.5 contract returns `time_hhmm`, `time_minutes` and display; writes accept exactly one of hours, minutes or HHMM. 50 game-free tests, clean Release and matched DLL `03DE264F…`. Owner-confirmed; later live read `812` → 08:12 / 492 min and no-op `hhmm:836` write/readback → 08:36 / 516 min. This does not exercise every write form. The earlier 10/10 transport/menu and loaded-world read checks remain historical; stale immediate readback is the known async-apply race. A diagnostics-only status-metric race remains documented. [Changelog](../ThirdParty/S1MCPServer-master/S1MCPServer/CHANGELOG.md). |

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
