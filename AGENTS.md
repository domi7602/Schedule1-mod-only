# AGENTS.md

Workspace: the `Schedule1-mod-only` repository root (any path/user, e.g. `C:\Users\<you>\Schedule1-mod-only`) — MelonLoader IL2CPP modding workspace for *Schedule I* (TVGS, Steam Open Beta; installed runtime **v0.4.7f12** — see the runtime note below; older verification baselines are listed separately). The repo lives **outside** the game install; builds and deploys resolve the game via the `SCHEDULE1_PATH` env var or the default `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`.

> **Current local runtime (2026-10-08):** `Latest.log` reports Schedule I **v0.4.7f12** and Unity **2022.3.62f2**; `appmanifest_3164500.acf` reports Steam build **25796331**. The S1API **3.2.1-beta.8** figure in that log is **historical**: it predates the 2026-10-09 stable install and refers to the superseded local PR #353 f11 compatibility build. It is evidence that the beta build loaded on f12, not that current S1API patches work. The installed `S1API.Il2Cpp.MelonLoader.dll` today reports ProductVersion `3.2.1+a4cbdd9e…` (§1); no new runtime test is claimed here. S1MAPI **2.0.1** is unchanged. TaxiDriver 0.8.2's no-deploy Release build passed against the live f12 references with 0 warnings/errors; the driver-safety update (0.8.3) was verified in-game on this runtime on 2026-10-11. `GameReferences/` was regenerated for f12 on 2026-10-08 (2 234 decompiled files from `Assembly-CSharp.dll`); for anything newer, use the live f12 assemblies directly. The old `UserData\S1API\LocalBuilds\...` provenance folders are no longer present in the current game directory.

Quick links: [`DEVELOPERS.md`](DEVELOPERS.md) — human build/test/debug workflow · [`docs/README.md`](docs/README.md) — documentation index · [`docs/architecture.md`](docs/architecture.md) — dependency boundaries · [`docs/pitfalls.md`](docs/pitfalls.md) — battle-tested gotchas · [`docs/compatibility.md`](docs/compatibility.md) — per-mod verification matrix · [`ThirdParty/README.md`](ThirdParty/README.md) — pinned dependencies · [`GameReferences/README.md`](GameReferences/README.md) — local decompiles · [`Skills/README.md`](Skills/README.md) — skill index.

> **For AI agents — read this file completely before touching the mods. Hard rules that have bitten us:**
> - Launch the game with `Start-Process "steam://rungameid/3164500"` (Steam App ID — keeps the Open Beta branch active and sets the correct working directory). Never start `Schedule I.exe` directly. **Close the game before building** — loaded DLLs are file-locked and the auto-deploy step fails.
> - Agent shells may not have the .NET SDK on PATH. **On the current device (since the 2026-10-24 change) SDK 8.0.426 and the .NET 6.0.36 runtime are installed globally in `C:\Program Files\dotnet` — do NOT export `DOTNET_ROOT`.** The old user-local override (`$env:DOTNET_ROOT="$env:USERPROFILE\.dotnet"`) breaks `dotnet test` here: testhost then cannot find `hostfxr.dll`, because `%USERPROFILE%\.dotnet` holds only global tools and sentinels (no `dotnet.exe`, no `host\fxr`). Only add the override on a machine where `dotnet --list-sdks` comes back empty. **C# 12 (`LangVersion` 12, §1) needs the .NET 8 SDK** — the 6.0 SDK cannot compile it, whatever the `net6.0` target framework says.
> - Shells vary: Windows execution policy can block `.ps1` scripts (Windows PowerShell 5.1 and `pwsh` both honor it; the effective policy depends on machine/user/host scope), and bash/zsh cannot run `.ps1` at all — so never rely on the ambient shell. Invoke repo scripts explicitly as `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/<script>.ps1`, and prefer forward slashes in paths (`Source/Mods/...`) so commands also work outside PowerShell.
> - Versions are machine-checked. Never hand-edit a version; use `pwsh Tools/bump-version.ps1 -Mod <Name> -Version x.y.z`.
> - Do not commit or push on your own — wait for Dominik's explicit go.
> - If you find something here wrong or outdated, update it.

---

## 0. AI-Agent Skills (`Skills/`)

Twenty-three skills under `Skills/<skill-name>/SKILL.md` (+ `references/`). Load them via the `skill` tool — **always start with `schedule1-modding`**, then the matching specialty. Index: [`Skills/README.md`](Skills/README.md).

- **Core runbooks:** `schedule1-modding` (scaffold/build/deploy, architecture — primary) · `schedule1-phoneapp` (Method-3 responsive UI, input focus, lifecycle rules) · `schedule1-troubleshooting` (log triage, crash patterns, IL2CPP pitfalls, slot recovery) · `schedule1-knowledge` (research: decompiles, S1API source, 64 curated systems).
- **Framework references:** `schedule1-s1api` · `schedule1-s1mapi` · `schedule1-game-systems` (64 systems, hook points) · `schedule1-mcp` (live game introspection, TCP :8765).
- **Domain:** `schedule1-economy` · `schedule1-persistence` · `schedule1-items` · `schedule1-grid` · `schedule1-interiors` · `schedule1-world-expansion` · `schedule1-custom-npcs` · `schedule1-3d-assets`.
- **Reusable patterns:** `schedule1-networked-world-state` · `schedule1-harmony-bootstrap` · `schedule1-il2cpp-reflection` · `schedule1-debounced-reload` · `schedule1-runtime-unity-cache` · `schedule1-lifecycle-verify` · `schedule1-mod-reversing` (third-party DLL analysis: decompile, hook inventory, compatibility check).

The skill list is anchored in three files (this one, `Skills/README.md`, `README.md`) — update all three when adding a skill.

---

## 1. Setup

- **Game:** `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` — override with `$env:SCHEDULE1_PATH` **before** `dotnet build` (MSBuild reads it once at startup).
- **Installed versions:** runtime game v0.4.7f12 (Steam build 25796331; Unity 2022.3.62f2; verified 2026-10-08 via Latest.log/appmanifest) · MelonLoader 0.7.3 (net6) · S1API 3.2.1 stable · S1MAPI 2.0.1. The compatibility matrix distinguishes assembly load/build from gameplay verification. TFM `net6.0`, `LangVersion` 12, nullable enabled.
  - **S1API 3.2.1 (2026-10-09):** official stable release installed, replacing the unpublished local beta.8+PR#353 compatibility build. It carries 0.4.7 (Special Customers) support on Mono + IL2CPP. Verified installed hashes (2026-10-24): IL2CPP `79d7af28…`, loader `811de671…` — both identical to the local `ThirdParty/S1API` Release builds. The `ThirdParty/S1API` submodule is at commit `a4cbdd9` (tag `v3.2.1`; annotated describe `v3.2.1-beta.9-13-ga4cbdd9`), and `git ls-tree HEAD ThirdParty/S1API` plus `git submodule status` both report `a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b` — the superproject pointer matches the checkout, so no pointer bump is pending. The installed DLL's ProductVersion `3.2.1+a4cbdd9e…` matches that commit. Full solution rebuild against 3.2.1: 0 errors / 0 warnings; all mod DLLs redeployed.
- **Install state (current device, changed 2026-10-24):** .NET SDK **8.0.426** and runtimes 6.0.36 / 8.0.32 global in `C:\Program Files\dotnet` — plain `dotnet` works in a fresh shell, no `DOTNET_ROOT` needed. `%USERPROFILE%\.dotnet` holds only global tools (`ilspycmd`) and sentinels; pointing `DOTNET_ROOT` there breaks `dotnet test` (testhost: `hostfxr.dll could not be found`). Python 3.13 with MCP dependencies isolated in `%LOCALAPPDATA%\Schedule1Tools\s1mcp-venv`; official nuget.org package source configured. Older repo copies (in the game dir / on the Desktop) are obsolete — this path is the only truth.
- **Runtime dependencies in the game dir:** S1API at `Mods\S1API.Il2Cpp.MelonLoader.dll` — the build probes exactly that filename (a wrong name breaks every mod with `CS0103: The name 'S1API' does not exist`); S1MAPI at `UserLibs\S1MAPI_Il2Cpp.dll`. `MelonLoader\Il2CppAssemblies\` only exists after the game has run once — until then, mods cannot reference Unity/IL2CPP types.
- **Launch, logs, live debugging:** `Start-Process "steam://rungameid/3164500"`; gameplay log `MelonLoader\Latest.log`; S1MCP bridge at `ThirdParty/S1MCPServer-master/` (TCP `127.0.0.1:8765`) — see `schedule1-mcp`.

**Layout:**

```text
Source/Mods/       Active mods + Shared lib, Directory.Build.props/targets, S1Mods.sln
Source/Archive/    Archived mods & tests (not in the solution, never built)
Source/Tests/      xUnit suites + LightControl console host; Tools/test-all.ps1 runs both
GameReferences/    Locally generated decompiles (gitignored; Tools/bootstrap-game-references.ps1)
Skills/            23 AI-agent skills (index: Skills/README.md)
ThirdParty/        Pinned deps & archives: S1API, S1MAPI, S1MCPServer-master, Archive/
Tools/             Build/test/release helpers: build-all, test-all, gen-sln, new-mod, bump-version, package-release, release-mod,
                   check-version-sync, check-doc-paths, deploy-thirdparty, bootstrap-game-references,
                   setup-workspace, new-laptop-workspace, backup-to-d, mods-cleanup-inventory,
                   check-skills-health, build-upload-sets
assets/            Screenshot convention (assets/README.md)
docs/              architecture.md · pitfalls.md · release-process.md
Release/           Release ZIP output
.githooks/         Git hooks (opt-in: git config core.hooksPath .githooks): pre-commit + pre-push
```

---

## 2. Mod Inventory

> **Status convention:** `Verified <date>` = confirmed in a real game session (assertion only, no artifact in the repo). `In-Game-Verify open` = code complete, not yet confirmed in-game. Keep the two apart — never claim a verify that did not happen. **Multiplayer is out of scope by owner decision (2026-10-24):** Dominik does not play MP, so no MP host/client check is an acceptance criterion. Every mod stays singleplayer-first; the host-authority and MP-refusal guards remain in the code and fail closed, but they are documented as unverified rather than open work. Latest recorded owner confirmations: 2026-10-24 (TaxiDriver v0.8.22 owner-confirmed finished, actions not itemized; PocketShop v0.3.9 directory/item-grid fix). 2026-10-15 (MessagesPlus v0.4.6 popup/response repair and StackLimitMod v0.1.7 ingredient stacking; original test dates not supplied; AutoPackagingStation v0.4.7 tested session). Previous verification round: 2026-10-11 (PhoneScrollFix v2.0.0; Minimap settings app; PotScanner v0.7.0, BankApp v0.5.0, Weather v0.4.0, StorageScanner v0.2.0, TaxiDriver v0.8.3); previous rounds 2026-10-10 (MessagesPlus v0.4.5 dark-mode polish + ProductAppQoL v0.1.0), 2026-09-30 (MessagesPlus v0.4.0), 2026-09-20.

| Mod | Status | Path in `Source/` | Path in `<Game>\Mods\` | Key bits |
|---|---|---|---|---|
| **NotesApp** | ✅ active (v1.0.3, **Verified 2026-09-15**) | `Mods/NotesApp/` | `NotesApp.dll` + icon | PhoneApp, SafeStorage, UITheme, InputFocus |
| **PotScanner** | ✅ active (v0.7.0, **Verified 2026-10-11**; core Verified 2026-09-15) | `Mods/PotScanner/` | `PotScanner.dll` | PhoneApp, Property/Growing/Money, GamePalette rewrite |
| **CalculatorApp** | ✅ active (v0.2.3, **Verified 2026-09-15**) | `Mods/CalculatorApp/` | `CalculatorApp.dll` | PhoneApp, Money, SafeStorage |
| **CustomSkateboard** | ✅ active (v1.1.5, **Verified 2026-09-15**) | `Mods/CustomSkateboard/` | `CustomSkateboard.dll` + icon | Harmony, visuals, avatar safety |
| **MoreSaveSlots** | ✅ active (v1.0.12, **Verified 2026-09-15**) | `Mods/MoreSaveSlots/` | `MoreSaveSlots.dll` | no S1API (MelonMod + Harmony) |
| **PocketShop** | ✅ active (v0.3.9, **Verified 2026-10-24** — owner-confirmed cast fix; prior v0.3.8 directory **owner-confirmed 2026-10-20**; suppliers hidden; last itemized purchase check v0.3.2 on 2026-09-20) | `Mods/PocketShop/` | `PocketShop.dll` | PhoneApp, vanilla PaymentType, level locks |
| **BankApp** | ✅ active (v0.5.0, **Verified 2026-10-11**; v0.4.4 **Verified 2026-09-15**) | `Mods/BankApp/` | `BankApp.dll` + icon | PhoneApp, banking, slot isolation |
| **Weather** | ✅ active (v0.4.0, **Verified 2026-10-11**) | `Mods/Weather/` | `Weather.dll` + icon | PhoneApp, read-only dashboard |
| **BusinessIncome** | ✅ active (v0.1.6, **Verified 2026-09-19**) | `Mods/BusinessIncome/` | `BusinessIncome.dll` | host authority, slot isolation, console |
| **StackLimitMod** | ✅ active (v0.1.7, **Verified, owner-confirmed 2026-10-15**; original test date not supplied; v0.1.6 **Verified 2026-09-20**) | `Mods/StackLimitMod/` | `StackLimitMod.dll` | Harmony, Registry, agriculture-only |
| **Minimap** | 🧪 development (v2.3.5-dev.1; waypoint safety, versioned search store and opt-in Phase-0 observer, 215 tests; **In-Game-Verify open**; historical HUD/settings acceptance below) | `Mods/Minimap/` | `Minimap.dll` | Existing HUD/settings; waypoint persistence repair; prepared unwired search core + search store; no playable search mode yet |
| **AutoPackagingStation** | ✅ active (v0.4.7, **Verified 2026-10-15, owner-confirmed tested session**; 66 tests; Release deployed, retirement-scoped review PASS; multiplayer out of scope (owner does not play MP), legacy migration unverified) | `Mods/AutoPackagingStation/` | `AutoPackagingStation.dll` | Buildable, SafeStorage, GLB |
| **HitmanPhone** | 🧪 audit repair (v0.3.0-dev.1; 148 tests, Release/Debug compile clean; **In-Game-Verify open**; historical 0.2.9 acceptance below) | `Mods/HitmanPhone/` | `HitmanPhone.dll` | Singleplayer only; world checkpoints, evidence delivery/receipts, journal/offer repair |
| **MessagesPlus** | ✅ active (v0.4.6, **Verified, owner-confirmed 2026-10-15**; popup/response repair accepted; original test date not supplied; 25 tests passed) | `Mods/MessagesPlus/` | `MessagesPlus.dll` | vanilla-Messages patches + permanent whole-app dark mode |
| **ProductAppQoL** | ✅ active (v0.1.0, **Verified 2026-10-10**) | `Mods/ProductAppQoL/` | `ProductAppQoL.dll` | Product app: asking-price −/+ buttons respond to mouse/touch (tap = ±$1, hold = repeat) — vanilla wires them gamepad-only via `InputValueRamp` (dead buttons for PC players) |
| **TaxiDriver** | ✅ active (v1.0.0, version-only milestone of the owner-accepted 0.8.22 code; **Verified, owner-confirmed 2026-10-24**; actions not itemized; rebuilt 1.0.0 DLL not yet loaded in-game; 683 tests; known road/jitter issues remain) | `Mods/TaxiDriver/` | `TaxiDriver.dll` + GLB data folder | Own NPC/controller registration, retained release, safe direct navigation fallback, S1MAPI |
| **StorageScanner** | 🆕 new (v0.2.0, **Verified 2026-10-11**) | `Mods/StorageScanner/` | `StorageScanner.dll` + icon | PhoneApp, read-only storage overview |
| **S1MCP** | ✅ active (v1.0.5, **HHMM clock fix deployed 2026-10-24, in-game verify open**; v1.0.4 was verified 2026-10-17 — 10/10 integration checks in the main menu, fast path proven via `inline_requests`; quick-win endpoints `set/get_time_of_day`, `get_weather`, `list_mods` added; read paths verified in-game in a loaded world, `set_time_of_day` write path verified 2026-10-17 (`changed:true` + readback; the synchronous readback can lag one apply — `changed:false` with a stale readback is the known async-apply race, not a failed write)). **The 1.0.5 fix supersedes that write result:** `CurrentTime` is HHMM, so the 2026-10-17 `{"time": 12}` write actually landed on 07:20, not 12:00 - routing, host authority and readback were proven, the applied value was not. 1.0.5 returns `time_hhmm` + `time_minutes` and accepts `time` (hours) / `minutes` / `hhmm`; 50 game-free tests in `Source/Tests/S1MCP.Tests` pin both conversions. One fresh in-game read+write check is open. | `ThirdParty/S1MCPServer-master/` | `S1MCPServer-IL2CPP.dll` | MCP over TCP :8765; fast path answers `handshake`/`status`/`list_methods`/`heartbeat` on the server thread without game focus; `list_mods` works from the menu, time/weather degrade to a clean `available:false` until the world loads; `load_save` loaded the owner's "Tests" slot from the menu |
| **LightControl** | 🧪 installed graphite candidate (v0.1.0-dev.19; **installed 2026-10-22**, three targets and dev.18 backups hash-verified; 264 checks, forced Release 0/0, scoped format clean, final independent delta review PASS; native list/picker screenshots checked 2026-10-22; **In-Game-Verify open**) | `Mods/LightControl/` | `LightControl.dll` | approved HTML port on owned 1201x655 canvas; Arial fallback active (Segoe UI array overload fails on a missing downstream constructor), glyphs, four-column table with sticky heading, labelled Color and brightness meters, 5x2 Cancel/Apply palette with Rainbow tenth; seven immediate brightness steps and separate More/global-copy cards; unique per-widget forwarding fixes S1API listener dedup; owned resource cleanup; 19 CS + project, 17 other production inputs hash-identical to pre-task; evidence/receipt/backups in `Source/Mods/LightControl/docs/graphite-ui-verification.md`; exact font parity, interaction/lifecycle checks open |
| **PropertyLEDs** | 🧪 experimental (v0.1.8; first-open exact current-property selection and property-named power status 2026-10-20, 115 tests; In-Game-Verify open; 0.1.4 owner UI acceptance is historical) | `Mods/PropertyLEDs/` | `PropertyLEDs.dll` + icon | independent RGB ceiling LEDs, per-room overrides, slot-isolated settings, segment-beam geometry, measured layout catalog + reference-ported coverage |
| **PhoneScrollFix** | ✅ active (v2.0.0, hover look **Verified 2026-10-11**) | `Mods/PhoneScrollFix/` | `PhoneScrollFix.dll` | home-screen icon hover: clipped slim frame + RectMask2D viewport (white-plate fix) |
| **WorldLook** | 🧪 installed water-surface candidate (v0.3.0-dev.2; **installed 2026-10-23**, 1,167 water tests, full build 0/0, controlled GPU comparison and independent shader/fixture reviews PASS; **In-Game-Verify open**) | `Mods/WorldLook/` | `WorldLook.dll` (embedded water bundle) | Native sky/mild light; fine advected water normals, filtered bounded sun glints, retained refraction/clarity. DLL `410fecf7…`, bundle `29557b87…`; three targets/backups verified. 13 CS + project, C# delta limited to version metadata. Evidence: `Source/Mods/WorldLook/docs/water-surface-verification.md`. |
| **Shared** | ✅ active (workspace lib) | `Mods/Shared/` | `Shared.dll` | see §5 |
| **S1API** | ✅ active (3.2.1 stable, submodule `ThirdParty/S1API/` at tag `v3.2.1`) | — | `S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.MelonLoader.dll` | required for 0.4.7f6+; 3.2.1 adds released Special Customers (0.4.7) support |

> **Archived** (source preserved under `Source/Archive/`, not in `S1Mods.sln`, never built): BackpackMod v1.2.3 · DayCounter v1.0.0 · HomelessMod v0.1.12 · ProfitTracker v1.1.0 · snackVendor v0.0.9 · TVBrowser v0.1.0 · _DiagPerfCounter v0.3.2, plus `Source/Archive/Tests/BackpackMod.Tests`.
> **Removed / retired third-party:** MikuPlayerModel (discontinued) · ConstructionSiteProperty (disabled) — both remain as `*.dll.bak` (MelonLoader never loads `.bak`) · MoreDrugs (removed 2026-09-19) · PhoneScroll (retired 2026-09-16, `ThirdParty/Archive/PhoneScroll/`) · Herer's Minimap (removed 2026-09-17) · Sideload (dropped 2026-09-10) · Hash (`ThirdParty/Archive/ScheduleOne-Hash/`, reference only).
>
> **S1API note:** stay on **3.2.1 stable** (installed; submodule/build commit `a4cbdd9`); the 3.2.1-beta line is historical. Do **not** downgrade to 3.2.0: it predates the 0.4.7f6 renames and spams `MissingMethodException: NPCHealth.set_npc` (kills 3 patch classes).

<a id="worldlook-continuation--installed-standalone-candidate-2026-10-22"></a>

### WorldLook continuation — native-sky baseline and water refinement (2026-10-23)

**Current working source and installed candidate: `Source/Mods/WorldLook/` (0.3.0-dev.2).** The owner-approved finer irregular water normals and smaller bounded sun glints are installed with retained clarity/distance filtering. Active record: `Source/Mods/WorldLook/docs/water-surface-verification.md`. The 87-file pre-change backup is `.backups/WorldLook/water-detail-20261023-035705/`.

**Installed 2026-10-23:** DLL `410fecf7dda5f079e86807942eb18ef98168201c4a9dddea124391e3d5bd1645`, embedded bundle `29557b87609c367e3b568d84d10400d67c980581a1e3491fbd455def9794c8ef`. Parent 1,167 water tests and 104 unchanged-light baseline tests pass, full shader/C# build 0/0, controlled GPU comparison (20 images across baseline/candidate), independent shader and fixture reviews PASS. Version/format/doc/solution gates pass. Three targets/backups hash-verified; 31 of 34 checked installed files unchanged, including profile (global/light/water ON, Warmth 1.0). Dev.1 rollback: `UserData/.deploy/backups/20261023T042253589Z-WorldLook-2e2ba848/` in the game install. Game was closed at final deployment readback; subsequent owner-run evidence follows.

**First dev.2 runtime, 2026-10-23:** fresh loaded DLL/bundle hashes match. ON F9 at 07:15:48 local is preserved in `status.json.bak`: native sky, active reference water/native SkyState, main-camera depth/opaque requests true, HHMM `740` → 460 minutes, five readable lighting channels, strength 0 (expected after 07:30), no module faults/errors/pending restores. OFF at 07:16:20 and later F9 report clean water release. Morning images (07:59/08:17) show retained submerged visibility versus stronger apparent-OFF surface detail, without a strong direct-sun glint. **Owner subsequently confirmed "ruhig" during slow camera movement**, closing the reported morning shimmer check. Game closed before collection; no live MCP call. Evidence: `.backups/WorldLook/water-detail-20261023-035705/runtime-20261023-051741/`. Missing-ON-status item closed for one sample; overall/direct-sun appearance, measured performance, positive warmth, ON-after-OFF, night and reload remain open. The following dev.1 observations remain historical.

The prior native-sky consolidation restored the mild GoldenHour light recipe with corrected HHMM time, kept the clear environment-only water, and retired the full GraphicsProbe/custom sky. Its dev.1 checks, review, installation and partial runtime evidence are recorded in `Source/Mods/WorldLook/docs/native-baseline-verification.md`; full gameplay acceptance is open. Identify every candidate by hash as well as version.

**Installed 2026-10-22:** DLL `79721251dd863862b027d08d4064b47a197dd8a9527c98f84bf43f6457391c27`, embedded bundle `24748b573273914b6a21b170d698d80ace7a6c7e0da153623353bf901a892391`; source/resource/deployed hashes checked. Profile: Enabled/LightingEnabled/WaterEnabled true, Warmth 1.0 of the mild recipe. 1,293 tests and full build 0 warnings/errors; final independent delta review PASS. Only four WorldLook files changed among 34 checked installed files, with verified backups; game closed throughout installation.

**First candidate run, 2026-10-23:** fresh log matches DLL/bundle identities and confirms reference-water application, then one OFF/material-release at 05:33:58 local. F9 at 05:34:29 reports native `SkyMat / Custom/ScheduleOneSky`, both modules OFF, no module faults/pending lighting restores. Owner supplied morning images (07:25/07:44): clearer submerged detail in the first, stronger surface reflections in the second, both dark; AN/AUS order is consistent with the log but not explicitly labelled. Game was closed at collection, so no live MCP readback. Evidence: `.backups/WorldLook/native-baseline-20261022-171619/runtime-20261023-033632/` and the canonical verification record. Full look acceptance, active-light/HHMM status, ON-after-OFF, daylight/night and reload checks remain open. Prototype evidence below remains historical.

**Midday follow-up, 2026-10-23:** owner images at 13:55/14:08 show clearer bottom plus broad soft highlight bands in the first, more distinct surface/glints in the second. Fresh run retains the candidate hashes, applies water, then reports clean OFF at 05:47:58 local. F9 at 05:48:20 is OFF again; active-light/HHMM readback remains open. Game closed before collection; evidence under `.backups/WorldLook/native-baseline-20261022-171619/runtime-20261023-034923/`. A focused water-normal/highlight refinement is proposed pending approval; no new code/build/deployment. The canonical verification record distinguishes source-based cause hypotheses from observed visuals and owner acceptance.

Historical prototype source is preserved at `Source/Archive/WorldLook/Standalone-0.2.0/`; full pre-task sources/artifacts/config are backed up locally under `.backups/WorldLook/native-baseline-20261022-171619/` (216 hash-verified inputs). The original external copy at `C:\Users\pc\AppData\Local\hermes\cache\scratch\worldlook-020\WorldLook` is retained for evidence, not active development. `<Standalone>` in the historical notes below denotes that external root.

- Session recovery rechecked the installed DLL against that project's `dist/WorldLook.dll` and `dist/build-info.json`: SHA-256 `aebd2b81a0afe143f95e8e54c9e8d06caca30fb52f810da048f3d9f910932515`, with all 24 recorded source hashes matching. The current game log loads embedded shader bundle `d31739f1462b0bdaa6a0c4b0da165c89cdee17aa0c07265d00dfb941c606824c`. Multiple candidates share version 0.2.0; identify them by hash.
- Prototype handoff: **water AN/AUS pair shows improved bottom visibility; OFF path logged, full acceptance open** at `<Standalone>/docs/water-on-test-20261022-1510/README.md`. The clock-format defect identified there is addressed in the canonical light module; the separate MCP time-formatting defect remains outside this change. Prototype sky repair summary: `<Standalone>/docs/cloud-restore-20261022-133344/README.md`; first-image record under `runtime-20261022-144019/`, toggle evidence under `<Standalone>/docs/cloud-restore-20261022-133344/runtime-toggle-20261022-150146/README.md`. Its custom sky renderer is archived in the native-sky candidate.
- **Owner-confirmed 2026-10-22:** normal skybox without orange veil, and Ctrl+Shift+F7 OFF/ON without problems. The saved 17:01:29–17:01:39 local-time log shows four full OFF/ON cycles plus final OFF: five `RestoredBackup` releases retaining native skybox #-176514, four reapplications, no WorldLook exception reported. F9 at 17:01:46 confirms `skyFaulted=false`, `skyPendingRestores=0`, valid native skybox and the final OFF state. Camera-motion trails were not separately reported; day/night, scene/save transitions, water and lighting acceptance remain open.
- **Approved water-test preparation, 2026-10-22 15:12 UTC:** game closed; original config backed up and hash-verified under `<Standalone>/docs/water-on-test-20261022-1510/WorldLook.before.json`. Only substantive config change: `WaterEnabled=false` → `true`; startup look/clouds enabled, `Clouds.LightingStrength=0`. JSON and exact single-setting delta verified, installed DLL still `aebd2b81…`. No new build or game run. The last captured OFF status belongs to the previous sky-only session, not this water test. Water appearance, active reference-shader binding and combined F7 restore await the owner's next run.
- **Subsequent owner run, 2026-10-22:** read-only S1MCP 1.0.4 handshake advertises 51 methods; Main/host, process 7916. Ten matched request/response pairs confirm active/enabled `Map/StylizedWater2_Ocean`, `forceRenderingOff=false`, supported `S1Mods/ReferenceWater` on its shared material, queue 3000. Log confirms the reference recipe (surface opacity 0.22, environment strength 0.12), not the native fallback. The owner screenshot (HUD Wednesday 11:37) shows visible dark blue-gray water with fine ripples; assistant finds it dark/matte, not an owner defect report. Shallow-bottom clarity, motion and combined F7 still open. Existing F9 file is stale (15:01 UTC, previous session). Evidence is under the water-test handoff. The documented Python venv is absent and Python 3.14 lacks client dependencies; this capture used the source-verified length-prefixed TCP protocol via stdlib, then closed the connection.
- **Second water image / clock finding:** Thursday 15:25 screenshot shows submerged quay profiles but nearly black foreground; same-view F7-OFF comparison requested, water acceptance open. A later read-only capture at native ~19:54 confirms `CurrentTime` is HHMM: MCP mislabels raw 1954 as minutes (`32h 34m`), and WorldLook's `ReadDayFraction()` incorrectly returns noon `.5` for it. This affects the star-fade/fallback clock; normal palette uses the readable native sun direction, so it is not a proven dark-water cause. No time mutation or code repair. Details and transcript: water-test handoff.
- **Subsequent water comparison:** requested AN/AUS order, Friday 15:08/15:24 images show clearer bottom/plants/quay with WorldLook and stronger surface/reflection detail when OFF; both views are dark. This corrects the earlier suspicion of exclusively mod-caused darkening; no blanket brightening justified from this pair. At 18:27:58 local the saved log reports water `Stopped` and overall OFF, followed by valid native sky backup restore. Game was closed at follow-up, no new OFF-material MCP readback or subsequent ON. Full owner water-look acceptance, movement/night and combined re-enable remain open; evidence in the water-test handoff.
- Existing build/test records: Unity editor 2022.3.62f3, C# 0 warnings/errors; 32 test groups, 8,283 policy/delegate/source/numerical checks, 0 failures. Session recovery read those records and verified hashes; it did not rerun the build/tests. **In-Game-Verify remains open.**

### Mod Details

**NotesApp v1.0.3 (2026-09-13):**
- Phone notepad: real-time search, pinning, edit/copy/clone/delete with confirm modal, in-game quick-stamp; atomic `notes.json` via SafeStorage; `UITheme` scaling + `NotesAppInputFocus` (typing does not move the character).
- v1.0.3 fixes: null-safe JSON load, slot-suffix ≥ 0. History: `Source/Mods/NotesApp/docs/CHANGELOG.md`.

**PotScanner v0.7.0 (2026-09-27, BankApp-palette view rewrite):**
- Grow dashboard: every `GrowContainer` per property (2 s polling, 0-alloc), filter chips (Thirsty/Ready/Empty), quality display, single-property focus, Water All (30 % skip) + persisted Auto-Water (50 g/pot); money paths host-only.
- v0.7.0 = pure view rewrite on the shared `S1Mods.Shared.GamePalette` / `UISprites`; behaviour unchanged. **Verified 2026-10-11.** History: `Source/Mods/PotScanner/docs/CHANGELOG.md`.

**CalculatorApp v0.2.3 (2026-09-12):**
- Exact `decimal` calculator with searchable history, Cash/Bank insert chips, clipboard, repeating equals; `CalculatorAppInputFocus`; SafeStorage state.
- v0.2.3: overflow handling + slot ≥ 0 guard. History: `Source/Mods/CalculatorApp/docs/CHANGELOG.md`.

**CustomSkateboard v1.1.5 (2026-09-13):**
- Skateboard feel overhaul (carving, instant jump, push curve, anti-gravel) plus a deck visual swap with strict renderer whitelisting so the player avatar is never touched; runtime instance tuning at `OnMount` keeps vanilla boards untouched; Jeff store injection + `skate` console.
- History: `Source/Mods/CustomSkateboard/docs/CHANGELOG.md`.

**MoreSaveSlots v1.0.12 (2026-09-13):**
- 25+ save slots with paginated navigation, inline rename (with `Game.json.bak`), hotkeys/wheel; Harmony remaps keep the vanilla menus working; modal canvas/sorting fixes.
- History: `Source/Mods/MoreSaveSlots/docs/CHANGELOG.md`.

**PocketShop v0.3.9 (prior v0.3.8: 2026-10-20):**
- **0.3.9 fixes a crash that fired in-game on 2026-10-24:** `Latest.log` 06:48:34 `ShopCatalog subscriber failed: Unable to cast object of type 'UnityEngine.Transform' to type 'UnityEngine.RectTransform'`. `ItemGridPane.BuildRows` and `PocketShopApp.BuildUI` still resolved their `RectTransform` with `(RectTransform)x.transform`, which throws on IL2CPP interop proxies; both now use `GetComponent<RectTransform>()`. The 0.3.7 cleanup had fixed only `StoreCatalogPane` and pinned it with a file-scoped guard, so the other two call sites survived — the guard now scans the whole `src` tree (`NoSourceFileCastsATransformProxyToRectTransform`, RED observed via a probe file). 64 tests pass; Release built/deployed with 0 warnings/errors, installed DLL/PDB/metadata hashes matched. **Verified in-game 2026-10-24 (owner-confirmed):** the app loaded as `PocketShop v0.3.9`, the directory and the item grid opened, and the 08:21-08:23 log shows two clean `directory layout applied: viewport=100 … cells=12` lines with zero `ShopCatalog subscriber failed`.
- Fixed four-column grid with three visible rows, scroll overflow and subtle trailing placeholder cells; stable tile size across catalogs/search. Two-line name band with font-metric headroom. (52 tests at the time of 0.3.7/0.3.8; current visual acceptance open.)
- Phone shop browser (grid + detail modal, inline quantity). Since v0.3.1 payment follows the vanilla `shopInterface.PaymentType` — cash shops use `ChangeCashBalance`, online shops `CreateOnlineTransaction`; level/rank locks enforced; host-safe refunds.
- **0.3.8 directory owner-confirmed 2026-10-20.** S1API supplier shops are hidden; Dominik called the app finished. **In-Game-Verify open for 0.3.3–0.3.7.** Last real session: v0.3.2 on v0.4.6f13 (2026-09-20, "done, nothing unusual"). The 0.3.3 payment rewrite and the 0.3.4–0.3.6 layout/header polish have no recorded in-game check; the earlier "Verified 2026-09-20" label had been carried onto 0.3.6 by bumps that rewrote only the version cell. History: `Source/Mods/PocketShop/docs/CHANGELOG.md`.

**BankApp v0.5.0 (2026-10-04):**
- Chip-based deposit/withdraw screen: weekly $10k ATM limit, two-column cash/online balances, double-entry booking with rollback, slot-isolated persistence, `NetworkGuard.IsInMainScene` guards.
- v0.4.5 = removes the phantom withdrawal cap ("inventory full"): the old capacity check multiplied free inventory slots by a cash stack limit that is 1 in vanilla, blocking nearly every withdrawal. **Verified 2026-10-11.** History: `Source/Mods/BankApp/docs/CHANGELOG.md`.

**Weather v0.4.0 (2026-10-01):**
- Read-only phone dashboard of the nine weather components: accent-bordered hero card (name, percentage, intensity pill, meta count, ring gauge with a rounded accent arc) over nine live rows — active conditions tint their row, border, icon and bar, inactive rows stay neutral. Empty state, no persistence, no gameplay impact, no input field.
- v0.4.0 = rebuilt to the approved mockup (flat canvas without header chrome, name above the bar in each row, accent-tinted active rows). All anchors measured off the reference design as canvas fractions. **Verified 2026-10-11.** History: `Source/Mods/Weather/docs/CHANGELOG.md`.

**BusinessIncome v0.1.6 (2026-09-17):**
- Daily passive revenue for owned businesses: deterministic variance, employee/weekend bonuses; host-authority fail-closed, slot-isolated idempotent payout marker, bounded catch-up; `biz` console dashboard.
- History: `Source/Mods/BusinessIncome/docs/CHANGELOG.md`.

**StackLimitMod v0.1.7 (2026-09-26, ingredient stacks):**
- Configurable stack limit (default 40) for agricultural + ingredient categories only; weapon/ammo/clothing/cash veto; dual discovery (definition scan + `Registry` hook) with a `BaseItemInstance.get_StackLimit` postfix; restore on unload; `apply_report.json` + `stack` console.
- **Verified, owner-confirmed 2026-10-15:** Dominik confirmed v0.1.7 works in-game, closing the ingredient-stacking acceptance item. Confirmation recorded on this date; original test date and individual actions were not supplied.
- v0.1.6 was the IL2CPP fix (`is` checks are useless on proxies — `TryCast<T>()`), proven via apply_report + the 2026-09-20 session. History: `Source/Mods/StackLimitMod/docs/CHANGELOG.md`.

**AutoPackagingStation v0.4.7 (2026-10-15):**
- Industrial automated packaging station (2×2m footprint, E-interactable, Hustler-I gate): native `PackSingleInstance` pack engine, chance-based freshness bonus, auto-unpack canvas mirror, restore-safe native slot sync, save schema v2 (`UnpackageMode`, `PendingRefunds`), PackUp with refund + pending-refund queue; all mutations host-guarded.
- **Verified 2026-10-15, owner-confirmed tested session.** Native prefab/network graph and owned cauldron footprint retained; no late PackagingStation injection. The v0.4.6 collider A/B restored native E/canvas interaction by retaining native placed collider geometry. Version 0.4.7 retires the temporary read-only Awake observer, interaction/collider dumps and diagnostic bookkeeping, without adding replacement interaction listeners. 66/66 tests, Release 0 warnings/errors, retirement-scoped review PASS; deployed DLL/PDB/metadata matched. The v0.4.7 session log matches the installed DLL hash and confirms fresh setup, native canvas, Pack/Unpack and mode mirroring; the owner subsequently confirmed everything tested in-game worked. Additional actions were not itemized; separate multiplayer and 0.4.1 cauldron-save migration acceptance remain unverified. History: `Source/Mods/AutoPackagingStation/docs/CHANGELOG.md`.

**HitmanPhone v0.3.0-dev.1 (2026-10-19, experimental audit repair):**
- Singleplayer only (MP hosts/clients blocked with tracker status/local notice). World-checkpoint-bound snapshots replace write-through sidecars; confirmed load identity/write destination, strict native JSON fingerprint, exact primary/backup matching and fail-closed unknown checkpoints. Legacy sources retained, but initial legacy world alignment is unproven.
- Recognized elimination/pending photo/delivery are separate; per-lifecycle verified item IDs, wrong-drop scan continuation, shared deadline, conservative legacy-zero/unknown presence, KO-to-death tracker updates, structural quest phases/deterministic GUIDs and truthful event/contract counts. Native payment exceptions preserve an unresolved claim/photo for manual balance review, without automatic retry/expiry.
- Confirmed-send-only slot consumption, bounded retries, one open offer, actual quota/spacing and monotonic cooldown/pursuit. 148 pure/delegate/source tests; Release/Debug compile 0 warnings/errors; final independent delta review PASS. Release candidate deployed and DLL/PDB/metadata hash-matched; Shared unchanged. **In-Game-Verify open**: normal Release chain, native save/restore/slot roundtrip, registry-before-items and GUID persistence still need the backed-up test slot. Record: `Source/Mods/HitmanPhone/docs/audit-repair-verification.md`.
- Historical beta acceptance **2026-09-24 belongs to 0.2.9**, not this repair. History: `Source/Mods/HitmanPhone/docs/CHANGELOG.md`.
- **First native checkpoint evidence, 2026-10-24:** `Latest.log` 08:22:55.675 reports `[checkpoint] restored 56391B779CBE for the confirmed world from …\UserData\HitmanPhone\checkpoints\63F99A08…\56391B779CBE….json` on a real save reload — the world-identity-confirmed restore path executed against a matching checkpoint file. Registry-before-items, deterministic-GUID persistence across that roundtrip and the cash paths remain open.

**MessagesPlus v0.4.6 (2026-10-12, scoped popup/response dark-mode repair):**
- **Verified, owner-confirmed 2026-10-15:** Dominik confirmed v0.4.6 works in-game, closing the popup/response repair acceptance item. Confirmation recorded on this date; original test date and individual actions were not supplied. This is owner-reported acceptance, not a new independent log or visual check.
- Removed contradictory blanket vanilla-color exclusions for deal-time and response menus; selective neutral surface/text policies retain clock/art/accent colors and native interactions. 25 tests pass; Release build clean. Historical verification below is retained separately.
- **Whole-app dark mode regression fixed and verified in-game 2026-10-09.** v0.4.3's rewrite of `AppTheme` had removed the broad sweep that tints every remaining near-white surface AND tightened the near-white detector (`Spread <= 0.08`), so the whole app stayed vanilla-white. The v0.4.0 sweep and its permissive colour gate are restored; the sweep runs as the FINAL pass, after the targeted ones, so it only fills the gaps (`ThemeColorPolicy` makes re-tinting an already-dark surface a no-op). Confirmed dark by Dominik (screenshots 13:39 / 13:43).

**MessagesPlus implementation history (2026-09-29/30):**
- Patch-only mod (no PhoneApp icon) on the vanilla `MessagesApp`: sticky search band under the title (live search, `[All][Customer][Dealer][Supplier]` chips, unread counter), "⋯" menu with Clear Read / Clear All — view-only filtering, host-only + customer-only mutations, one-time legacy restore of v0.1.x trashed threads.
- **Whole-app dark mode** (`AppTheme`): **permanent since v0.4.1** — the "⋯" menu toggle is gone, the config defaults to ON and a stale `false` self-heals at startup (`MessagesPlusConfig.DarkMode` stays for schema stability). Recolours our band + vanilla app surfaces (page backgrounds, inbox rows, chat bubbles + tails, dialogue header, generic near-white sweep) — one-time per graphic with cached originals; dead-drop/order and counter-offer subtrees are restored to vanilla colors and excluded from sweeps; avatars, badges, the unread dot, and product sprites remain untouched.
- **Vanilla order menus (2026-10-08):** `MSGConversation.responseContainer` and the verified `DealWindowSelector.Container` are preserved/restored; the deal-window hook no longer darkens popup content. In-Game-Verify open.
- Round-1 fixes: `(RectTransform)x.transform` casts → `GetComponent<RectTransform>()` (IL2CPP cast made the "⋯" menu dead), search surface keeps `raycastTarget=true` (v0.4.0 **Verified 2026-09-30**). History: `Source/Mods/MessagesPlus/docs/CHANGELOG.md`.

**Minimap v2.3.5-dev.1 (prior v2.3.4-dev.1: 2026-10-18 — waypoint safety and opt-in Phase-0 observation; In-Game-Verify open):**
- Property-based waypoint JSON, bounded/validated rows, original-path pending IO and A -> B -> A retention; corruption/backup recovery is read-only until explicit clear or manual repair. Console save failures are visible. Parent observed RED/GREEN review regressions; 186 tests at that build, Release 0 warnings/errors and scoped format checks pass.
- **2.3.5-dev.1 adds the missing persistence layer:** `SearchStore` is the versioned (`Version: 1`), fail-closed file store for the controller's `SearchRecord` (world/target/area id+version/accepted, never a completed flag). Max 16 world-keyed rows, 64 KB document cap, atomic dirty-retry writes; an unknown version, duplicate/invalid/null row, over-cap or oversized document rejects the whole file, and a corrupt primary is never overwritten (a valid `.bak` is served read-only until an explicit `Clear()`). 29 new game-free tests; full suite 215. Still not runtime-wired.
- `minimap searchdev` is read-only and OFF by default; explicit bounded development observations are separate from gameplay. The prepared search core and the versioned `SearchStore` are not runtime-wired. Hints UI, curated-area overlay, production world/session binding and exact-marker suppression remain unimplemented. This is NOT a finished search feature.
- 2.3.5-dev.1 DLL/PDB/metadata deployed and hash-matched (DLL `5DCB10EB…`). The live-registry and startup evidence (HUD plus four settings pages initialized, in the menu and the disposable test world) belongs to 2.3.4-dev.1. Manual waypoint roundtrip was postponed by the owner; gameplay acceptance remains open. Previous DLL/metadata and native test-save backups are preserved outside scratch.
- Historical HUD/settings evidence below belongs to its recorded older versions, not this development build.
- Minimap & Unified HUD: dealer/police/customer markers, pulsing heat ring, health bar, user waypoints (`minimap wp …`), and a phone settings app with per-blip toggles and hex colours. Fixed 64-blip pool; no runtime allocation/FPS measurement for the latest optimization.
- v2.3.3 removes keyboard visibility toggling and its rebind row; old keys migrate once to None and visibility is recovered. Zoom and explicit settings/console visibility controls remain. 61 tests pass; runtime verification pending.
- v2.3.2: cached snapshot projections with live converter/calibration invalidation, shared per-frame rotation math, and per-slot applied UI state to skip identical native writes. All categories/cadence/pulses, compact settings and visibility diagnostics retained. 58 tests pass; Release built/deployed with 0 warnings/errors; runtime visual/lifecycle verification pending (not a disappearance fix).
- v2.1.0 adds: zoom range 0.1–12 (one shared clamp rule), a MAP APPEARANCE settings section (shape, corner, rotation, size, zoom, opacity, border width + colour), rebindable keys in-app, purple potential-customer highlights, a blue accepted-deal clock, green purchase checks, gray regional locks, customer name labels, and a Hide-in-Menus HUD toggle. `Minimap.Tests` (xUnit, in `S1Mods.sln`) covers the Unity-free decision helpers.
- v2.1.1 replaces the customer text name with the **NPC portrait image** (`NPC.MugshotSprite`) by default; the name stays as a fallback. New `ShowCustomerPortraits` toggle (default on), `ShowCustomerNames` now default off.
- Built (0 warnings/errors); Minimap.Tests green (26 passed) and the deployed DLL matches the build output. In-game visual verification of the portrait badges is still open.
- **Verified in-game 2026-10-11 (owner):** the tabbed settings app (Karte / Marker / Farben / Erweitert) works. Follow-ups from that test, built & deployed the same day: colour rows are preview-swatch + ▼ palette pickers (`MinimapFeatureRules.ColorPalette`) instead of hex entry (picked colours persist immediately); the Zoom value row was removed from the settings (zoom stays on keys and the `minimap zoom` console command). Picker look pending.
- Post-v2.1.1 (unreleased): customer markers use portraits automatically (Customer Names / Portraits toggles removed; config fields kept for old saves); the pre-tab section builders were deleted.
- Restored from the archive, including the 2026-09-17 visibility fixes in `MinimapMod.cs`. History: `Source/Mods/Minimap/docs/CHANGELOG.md`.

**TaxiDriver v1.0.0 (2026-10-24, milestone release of the registered driver-controller repair; Verified, owner-confirmed 2026-10-24):**
- **1.0.0 is a version-only milestone:** `git diff` shows only the `MelonInfo` version string and `mod.json` metadata changed against the accepted 0.8.22 build. No behaviour, patch, prefab, save or S1API/S1MAPI surface was touched. The rebuilt assembly has a new hash (DLL `64CE7256…`, 371,200 B), and it has not itself been loaded in-game yet.
- **Owner confirmation 2026-10-24:** Dominik confirmed the tax driver work is finished. Individual tested actions and the exact session were not itemized, so the per-finding checklist in `driver-binding-verification.md` stays its own evidence record instead of being marked item-by-item from this confirmation. Known and unchanged: road-correction/jitter on some routes; multiplayer is out of scope (owner does not play MP) and the seat/STOP guards stay unverified in code.
- Uses `driver.Behaviour` and already registered, idle patrol components only; validates owner/cache/NetworkBehaviours/BehaviourIndex/server readiness. No runtime-added network components, owner rewiring, network initialization, S1API/prefab/save changes. Missing valid patrol uses existing direct Navigate.
- Retains original route/vehicle/aggression/enabled state and waypoints until native release is confirmed. Pending release fences fallback/redispatch/polling/reverse/relocation without extending deadlines or refilling budgets; permanent invalid ownership can require scene reload. 683 pure-gate/source tests and forced Release 0 warnings/errors; scoped format passes, whole-solution format reports unrelated LightControl whitespace. Independent delta review PASS. Evidence: `Source/Mods/TaxiDriver/docs/driver-binding-verification.md`.

**Prior traffic-light repair (historical, 0.8.21; red-light behavior owner-confirmed 2026-10-20, broader route acceptance open):**
- Private flags at parameterless `UpdateSweep`/`UpdateSpeedReduction` bypass only native TrafficLight hits for the exact owned host autonomous physical taxi; generic obstacles and native turn/destination braking remain. Kinematic/unknown/reverse/player states retain vanilla. The old out-parameter sweep detour is not installed.
- Stationary-only native comparisons identify readable exclusive traffic stops; only observed wait time is excluded from watchdog clocks/spending, without refilling attempts or declaring arrival. 670 tests and forced Release compile (0 warnings/errors); native activation/red crossing/braking acceptance remains open. Evidence: `Source/Mods/TaxiDriver/docs/traffic-light-repair-verification.md`.

**Prior visual wheel animation (historical, 0.8.20; rolling owner-confirmed 2026-10-20, other acceptance open):**
- Matched imported GLB wheel nodes roll using native rigidbody point velocity/collider radius and steer using native membership/angle. No-slip visual approximation; no wheelspin/suspension transfer, no GLB/material/physics/navigation/fare change. Pair in the common model frame; retain initial mounting offsets and rest rotation.
- Cached references belong to the model lifetime, survive parked-taxi reuse, and are cleared on scene change/shutdown/confirmed death/rollback. Only a new swap arms bounded initial retries; visual failures cannot trigger per-frame rebinding or abort driving work.
- Parent reran 649 tests and a forced compile-only Release (0 warnings/errors), scoped format checks pass. Release evidence and the pending in-game checklist live in `Source/Mods/TaxiDriver/docs/wheel-visual-verification.md`. Traffic-light hook remains disabled; existing navigation/road-correction issues are not repaired by this visual update.

**Prior bounded reverse supervision (historical, 0.8.19, 2026-10-18):**
- Exactly 19 production C# files plus unchanged project; mechanical midpoint preserved 498 tests and per-type IL. Final source separates operation lifetime, native orders, bounded reverse phases and queued callback outcomes. Confirmed-halt/proximity gates arrival and pickup; recovery does not finalize the meter merely because polling stops.
- Current net reverse evidence, real request-window consumer deadline, continuous rear/driver/authority/actuator checks, safe takeover cancellation and a guarded native-stop transition. Failed or superseded stop permits no follow-up. Independent fallback steps preserve current authority/ownership and never park against a player driver. Pedal -0.7, windows 1.5/3 s, budgets, fares and disabled traffic-light hook retained.
- Parent reran 605 tests and nonincremental Release compile (0 warnings/errors); final independent delta review PASS. Source-policy, delegate-seam and source-contract evidence only; native reverse/braking/callback/collider behavior and gameplay remain open. Existing coarse rear sphere check retained, no adaptive shorter reverse. Details: `Source/Mods/TaxiDriver/docs/reverse-recovery-verification.md`.

**Prior terminal relocation rescue and balance readback (historical, 0.8.18, 2026-10-15):**
- Terminal rescue no longer skips the guarded relocation when the car is on the vehicle graph. Proven by the 2026-10-15 `Latest.log`: the ride "taxi to Sweatshop" gave up 106.7 m from the target at `(40.1, 0.6, 52.5)` (`car is on the vehicle graph but still stuck ... no teleport needed, giving up`) while the car sat motionless. `TerminalRescuePolicy` decides: graph state never vetoes the attempt, only the spent one-shot rescue budget does; the attempt runs the existing guarded `RoadKeeper.TryRescueStartup` (supported free spot, ground snap, oriented-box clearance, occupancy check, road-aligned rotation) and re-dispatches. The recurrence-branch redispatch is unchanged (same log: recovered 72 m without intervention).
- Settlements log a balance readback: net worth (S1API `Money.GetNetWorth`, cash + online) before the money calls, observed movement after (`[meter] balance readback for $N: net worth X -> Y (observed Z, charged $N) — ...`). `BalanceReadbackPolicy` formats the verdict (tolerance $0.51). Log evidence only — never proof of causation, never retries/corrects a payment, an unreadable read never blocks settlement. The "(balance movement unverified)" wording stays (a void return is still no proof).
- Parent reran 498 tests (0 failed/skipped; 489 baseline + 9 new, RED observed before wiring). Release built/deployed with 0 warnings/errors after game-closed and Shared-match checks; installed DLL hash-matched. Gameplay verification open: rerun `ParkingGarage` → `taxi to Sweatshop`, confirm the terminal stall at `(40.1, 0.6, 52.5)` attempts `attempting teleport recovery` instead of giving up, and `[meter] balance readback` lines show movement matching the charged fares.

**Prior traffic-light hook disable (historical evidence, 0.8.17 on 2026-10-14):**
- The 0.8.11 wheel-sweep traffic-light hook (`TrafficLightPatch.Apply`, private scoped `DriveFlags` copy on `VehicleAgent.GetWheelSweepCollision`) is DISABLED. Root cause, proven by your 2026-10-14 `Latest.log`: the hook threw a native->managed `NullReferenceException` hundreds of times per second from the first pickup navigation onward, starving the `VehicleAgent` of sweep results. Consequences in that session: the taxi never reached its parked pickup state, rolled on without an active order after the give-up (idle guard stopped after 3 attempts), and could be player-driven because the passenger input gate only engages in `RidePassengerMode`. `TrafficLightPolicy`, the hook class and its unit tests remain in the tree for a trampoline-safe rework; `TrafficLightPatch.Remove()` stays in the teardown path as a no-op guard. Native traffic-light stopping behavior is restored (as before 0.8.11). Cruise, actuator, ride locks and packages 1–5 are unchanged.
- Parent reran 489 tests (0 failed/skipped). `TrafficLightWiringTests` now pins the disabled state per active source line. Release built/deployed with 0 warnings/errors after game-closed and Shared-match checks. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `035ddaa65ba814c79529026acf5cd47036d5016b66b77b47932cc6f3701af964`. Gameplay verification open: confirm no `GetWheelSweepCollision` ERROR spam, pickup stops at the player position, and the player cannot drive the taxi during an active ride. The 0.8.16 deinit expectations (`[taxi-lifecycle] teardown complete`, single meter line, no despawn) still apply at quit.

**Prior deinitialization lifecycle (historical evidence):**
- Deinitialization now runs a one-shot, exception-isolated teardown: the shutdown gate closes before work and cannot reopen mid-cleanup; update/deferred/native callbacks quiesce first (nav orders invalidated, pending spawn/auto-run/seat-verify/settle/lot-dump cleared); the fare meter settles exactly once (claim before report IO, no retry on failed/unknown reports); patrol/navigation/actions stop; only owned trunk components are restored; no foreign seat/player mutation, remote exit or blind occupied-taxi despawn. Harmony hooks (actuator, cruise, traffic-light, ride protection, trace) are removed last via the installed `Harmony.UnpatchSelf`, so a later re-init can re-apply them; pre-teardown callback generations stay inert after re-init. Repeat/reentrant deinit is a no-op; partial init is tolerated.
- Parent reran 489 tests (0 failed/skipped); independent focused review PASS. Release built/deployed with 0 warnings/errors after game-closed and Shared-match checks. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `e3752ad1d95f91842aed08e685e2465acd08fa5cc361404fd6b8f135eaa515e7`. In-game verification open and pending the joint test: expect `[taxi-lifecycle] teardown complete (quit/deinit)`, at most one `[meter] ride ended (session teardown …)` line and no taxi despawn/seat changes on quit. Harmony finalizer execution, MelonEvents unsubscribe symmetry and S1API event removal are source-verified only. Minor non-blocking: the teardown-complete log also prints on the second (no-op) teardown call, and the park intent is reset by hook removal in the same frame, making the parked log line optimistic at quit. Fare rules, recovery budgets, road/traffic patches, destination identity, seat/STOP safety and package-4 listener leases unchanged.

**Prior destination identity/app lifecycle (historical evidence):**
- Logical destination identity (name/kind/exact goal) is separate from physical route identity. A different destination sharing the running route updates picker/RideGoal/active-trip metadata without redispatch or order/budget/fare reset; reachable/fallback coordinates remain authoritative. Repeated same picks are no-ops while driving; give-up permits fresh dispatch. Hero rendering uses only its complete snapshot, including drop-off and active-trip state. Owned row, persistent/search and update listener identities are released before row/base-app destruction; forwarding leases sever callback targets and helper removal handles dead events/partial registration.
- Parent reran 464 tests (0 failed/skipped); independent focused review PASS. Release built/deployed with 0 warnings/errors after game-closed and Shared-match checks. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `c7cf35c2c1c3197e5e705576f6988dff0c8ae47be1cef569055fad8a2860cd8c`. IL2CPP/gameplay verification open: shared-drop-off selection, recovery-only hero refresh and actual helper-root counts across search/filter/reopen/scene teardown untested. If native removal and empty-event fallback both throw, helper entries may remain, but released forwarding leases no longer retain the app. Static UI, fares, recovery, seat/STOP safety, road and native patches unchanged.

**Prior multiplayer seats/STOP safety (historical evidence):**
- Passenger seating uses only verified vacant or exact-local native identities. Driver reservation requires raw reference absence (ReferenceEquals), independently of Unity overloaded null/seat occupancy. Failed seating stops navigation and attempts only safe local exit; new, ongoing, idempotent and reroute paths honor failure. STOP removes/verifies NPC occupants, then proves all player/seat/registry occupancy empty before consuming destruction attempts; occupied/unreadable/stale aggregate state reports Waiting for passengers and retains the taxi. CurrentVehicle is compared against the vehicle NetworkObject pointer. Existing ambiguous retry, fares, navigation, road and native patches unchanged.
- Parent reran 436 tests (0 failed/skipped). Independent package review identified a P2 Unity-null reservation gap; observed RED/GREEN repair and focused independent re-review PASS closed it. Release built/deployed with 0 warnings/errors after game-closed and Shared-match checks. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `42b6c91f224b9e71946faf3b5a341ce794c6a79210a74b462c6cd7345959b35d`. Multiplayer/gameplay verification open. Unreadable registry/seat state or stale aggregate occupancy conservatively retains the taxi until a later explicit STOP confirms vacancy. No forced remote-player exit.

**Prior supported road placement (historical evidence):**
- XZ projection now interpolates original 3D route elevation. Hard placement requires filtered static support within +/-0.75 m, eight-corner measured chassis clearance at the proposed heading, root-to-bottom offset plus 0.05 m, and no foreign collider overlap. Offset candidates are reprojected onto the route; unsafe candidates are rejected before placement/progress bookkeeping. Existing bounded recovery, fares, native driving and package1 remain unchanged.
- Parent reran 404 tests (0 failed/skipped); independent read-only review PASS. Release built/deployed with 0 warnings/errors after game-closed/Shared-match checks. Installed DLL/PDB/mod.json read back and hash-matched; DLL SHA-256 `5f38fa1b4e735fcd6bacdf1d51fbc9b2acaa9414de9177ffe2f9fe58517958da`. Gameplay verification open: conservative world-AABB clearance may reject valid sloped/crowded spots; a single support probe is not proof of wheel support or semantic road identity.

**Prior navigation package (historical evidence):**
- Patrol and ride share one 10 m arrival policy (loose 25 m patrol terminal allowance removed; standing = absolute signed speed). The unconfirmed 8 s patrol start self-test now blocks the ordinary stall watchdog for its full grace. New Go/patrol dispatches clear stale terminal state; recovery redispatch preserves the 4-episode cap, 90 s deadline and recovery budgets. Independent read-only review PASS; parent reran 361 tests; Release clean. In-game verification open. Prior evidence below is historical.
**Prior taxi-only native IgnoreTrafficLights (historical evidence):**
- Scoped physically private flags at the proven wheel-sweep consumer enable IgnoreTrafficLights only for the exact owned host autonomous taxi; restore original reference in postfix/finalizer. Other flags, generic obstacles, turn/destination braking, recovery and fares unchanged. Parent reran 324 tests plus native assertions; Release clean; installed DLL/PDB/metadata hash-matched. Forward-raycast branch does not consume this flag: actual signal crossing/hook execution/allocation cost remain unverified. Prior evidence below is historical.
- Recurring stalls within 6 m without 4 m fresh eligible forward goal approach switch to existing checked free-spot rescue. Reverse displacement no longer resets local history; reverse/coasting/artificial placement earn no reset credit. Operation cap, deadline, rescue budgets and fares unchanged. Parent reran 316 tests; Release clean; deployed DLL/PDB/metadata hash-matched. Actual escape/arrival pending; goal-approach metric conservative on detours. Prior UI/actuator evidence below is historical.
- Destination list inertia disabled; cards pinned to 44 Dp with no flexible height. Existing button transitions remain None. 297 tests passed; Release clean; deployed DLL/PDB/metadata hash-matched. Owner-reported scroll flicker resolution requires in-game confirmation. Driving behavior unchanged; actuator evidence below belongs to the prior build.
- Reverse and explicit park requests now supply exact-owned host-only inputs at the native ApplyThrottle physics consumer, with scoped snapshot/restoration and fail-closed hook-health checks. No native output flags or wheel torques are forced. Parent reran 295 tests; Release built/deployed with 0 warnings/errors and installed DLL/PDB/metadata hash-matched. Native hook activation, actual braking and turn escape remain unverified; positive-speed watchdog and arrival-threshold findings remain unchanged.
- Owned active autonomous taxi forward base target is 50 km/h before native turn/obstacle/destination reductions; traffic, player driving, reversing/recovery and explicit native overrides stay unchanged. No shared DriveFlags or static-speed mutations. 281 tests pass; Release built/deployed with 0 warnings/errors and installed DLL/PDB/metadata hash-matched. Hook activation and achieved speed remain open.
- Operation-wide finite recovery budget and a 90-second meaningful-progress timeout survive redispatches; confirmed vehicle death stops the meter and releases dependent runtime state before ownership reset. Reverse-pedal/standstill-hold changes from the previous build remain included but their runtime acceptance is not established by the cruise tests. Prior driver-safety gameplay verification below belongs to 0.8.3.
- Taxi service is controlled only by the phone app: CALL TAXI → `CallTaxi` → preserved `TickAutoRun`; destination list/search/filters, live status/fare and STOP remain in the app. Vanilla E entry/exit and Escape phone-close remain unchanged. The current source installs no TaxiDriver keyboard bindings.
- The safety pass adds exact spawned-vehicle ownership tracking, fail-closed ride locks, renderer-gated GLB rollback, lifecycle cleanup, bounded fare math and no-repeat payment handling. Earlier Stage 1–3 gameplay checks were on 0.4.7f6; they do not verify this update on f12.
- Dedicated driver only: no bystander fallback or replacement spawn from the service. Native invincibility is applied/read back on create/load/service; a host order can restore the existing saved dead driver. Genuine entry attempts use an exact-pointer 15-second guard; rejected re-entry stops the trip safely. STOP approval is tied to the same visible app session and monotonic ride identity.
- Current local target is v0.4.7f12 with installed S1API 3.2.1 stable and S1MAPI 2.0.1. Release built and deployed with 0 warnings/errors; 207 tests pass (pure policies plus limited source-wiring checks). An independent read-only review found one defect (feedback lost on deferred cleanup), now fixed with its own regression tests. Dominik confirmed the prior driver's killability and replacement appearance in-game; the driver-safety update was verified in-game 2026-10-11.

**StorageScanner v0.2.0 (2026-10-04, new):**
- Read-only PhoneApp: totals per item across all storage containers of owned properties; `ALL` + per-property filter chips, live name search, manual + periodic refresh, explicit INCOMPLETE banner (culled containers reported, never silently counted). No Harmony, no background scanning.
- **v0.1.1 fix:** the readiness gate blocked every scan — `IsContentCulled` is true for any property the player is not near, so the app showed "Loading storage..." forever and zero scans ever ran. The gate now waits only for the owned-property list; culled containers are skipped and reported as incomplete. Full source restored to the deployed 0.1.0 behavior + fix. **Verified 2026-10-11.** History: `Source/Mods/StorageScanner/docs/CHANGELOG.md`.

---

## 3. Build & Deploy

### Commands

```pwsh
# PowerShell: on the current device the SDK is global - call dotnet directly.
# Only on a machine where `dotnet --list-sdks` is empty:
#   $env:DOTNET_ROOT="$env:USERPROFILE\.dotnet"; $env:PATH="$env:USERPROFILE\.dotnet;$env:PATH"

# Single mod (builds and auto-deploys)
dotnet build Source/Mods/NotesApp/src/NotesApp.csproj -c Release

# Whole solution / all mods
pwsh Tools/build-all.ps1          # wrapper; or: dotnet build Source/Mods/S1Mods.sln -c Release

# Compile-only, no deploy to the game dir (CI uses this)
dotnet build Source/Mods/S1Mods.sln -c Release -p:S1NoDeploy=true

# Tests
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/test-all.ps1 -Selection all  # xUnit + console; no deployment
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/test-all.ps1 -Selection pure # game-free suites; no deployment

# Regenerate the solution after adding/removing a project
pwsh Tools/gen-sln.ps1

# Scaffold a new mod
pwsh Tools/new-mod.ps1 -Name <ModName> -Author Dominik -Version 0.1.0

# Release ZIP (needs game assemblies; excludes Shared + _DiagPerfCounter)
pwsh Tools/package-release.ps1 [-Mod <Name>]
```

### What a build deploys (automatic, via `Directory.Build.targets`)

- `Mods\` ← DLL + `*.png` icons + `*.bundle`; GLB meshes go to `Mods\<Mod>\` (data subfolder — MelonLoader loads only DLLs, runtime asset paths resolve there, e.g. TaxiDriver).
- `UserData\<Mod>\` ← `mod.json` + `<Mod>.pdb`. **Never put json/pdb into `Mods\`** (reference-only assemblies there also spam `BadImageFormatException` at game start).
- `S1NoDeploy=true` skips all copies. Every deployment verifies planned file hashes via `Tools/deploy-mod.ps1`; identical files are left unchanged. Changed targets have per-project backups and bounded in-process rollback. The game must stay closed; see `Tools/README.md` for receipts and limitations.

### Build caveats & common failures

- `$env:SCHEDULE1_PATH` must be exported **before** MSBuild starts.
- First clone on a new PC: `git submodule update --init --recursive` then `pwsh Tools/setup-workspace.ps1` (writes the gitignored `local.build.props` for S1API/S1MAPI; can activate the git hook).
- `S1Mods.sln` is generated (`gen-sln.ps1`, deterministic MD5 GUIDs, CI-checked) — regenerate, never hand-edit.
- Version bumps: `pwsh Tools/bump-version.ps1 -Mod <Name> -Version x.y.z` — syncs the mod's code (`MelonInfo`; PotScanner uses `Constants.ModVersion`), its `mod.json` + `CHANGELOG.md`, the AGENTS.md matrix row + detail header, and the README row.
- Failure signatures:
  - `CS0103: The name 'S1API' does not exist` → S1API missing or misnamed in `Mods\`.
  - `UnityEngine.* / Assembly-CSharp not found` at build → the game never ran; `MelonLoader\Il2CppAssemblies\` is empty.
  - Copy error `used by another process` → close the game before building.
  - `MissingMethodException: NPCHealth.set_npc` spam → wrong S1API line (use the installed 3.2.1 stable or newer; not 3.2.0).
  - `BadImageFormatException` at game start → a reference assembly sits in `Mods\`.
  - `dotnet: SDK not found` → set `DOTNET_ROOT`/`PATH` (see top) — but only when no global SDK exists.
  - `dotnet test` aborts with `hostfxr.dll could not be found` → a stale `DOTNET_ROOT` points at `%USERPROFILE%\.dotnet`; unset it (the runtime lives in `C:\Program Files\dotnet`).

### Multi-PC workflow

Linear on `main`, no feature branches. On a new PC: clone → `setup-workspace.ps1` → build. For an independent laptop sandbox (no GitHub fork): `pwsh Tools/new-laptop-workspace.ps1` (renames `origin` → `upstream`; sync with `git fetch upstream && git merge upstream/main`). Game saves live in `%USERPROFILE%\AppData\LocalLow\TVGS\Schedule I\Saves` — PC- and Windows-user-local, never synced through git. `<GameDir>\UserData\` is mod/loader state, not game saves: besides deployed `mod.json`/PDB files it holds loader preferences (`MelonPreferences.cfg`), mod configuration, and mod-owned slot persistence sidecars such as `UserData\HitmanPhone\bounties_slot_1.json` with its `.bak`. The game's native save files are never stored there.

---

## 4. IL2CPP vs Mono (alternate branch) — Architecture Decision

**Decision: stay on IL2CPP** (the default Steam release). Mods then run natively for all regular players without switching to the `alternate` beta branch, and all common IL2CPP hurdles (`IntPtr` constructors, canvas DPI scaling, lifecycle timing) are already solved and documented in the mods and skills.

---

## 5. Conventions & Shared Utilities

### New-mod source-file budget (external review)

- Design new mods for **15–20 hand-maintained production files total**, counting C# sources plus the mod's `.csproj`, so the complete source fits the owner's 20-attachment review limit. This is a planning target, not a minimum: small mods should use fewer files.
- Exclude tests, assets, metadata/docs, shared build files, external dependencies and generated `bin/obj` files from this source metric. Explain inherited build/dependency context in the accompanying review message; any extra attached overview also consumes an attachment slot.
- Group related types/policies coherently by domain. Do not create one file per tiny helper by default, but do not meet the budget by hiding code, adding assemblies, deleting tests or creating arbitrary giant files.
- Check the count when scaffolding and when adding production files. Before exceeding 20, explain the architectural reason and agree an exception or a complete generated upload bundle with the owner. Existing mods are not automatically restructured by this rule; their consolidation needs explicit authorization.

### Responsive UI (Method 3: `UITheme`)

S1API instantiates the phone on a high-DPI uGUI canvas, often rotated 90° — fixed font sizes look tiny. Use `S1Mods.Shared.UITheme` (single source of truth; per-mod wrappers were removed):

- `UITheme.InitializeForTextApp(containerRt)` (750f, 0.85–2.0) · `InitializeForDashboard(containerRt)` (900f, 0.75–1.20) · `Initialize(containerRt, refHeight, minScale, maxScale)`.
- In `OnCreatedUI(GameObject container)` initialize first, then express every size via `UITheme.Sp(...)` (fonts) / `UITheme.Dp(...)` (padding).
- Shared colour + sprite kit: `GamePalette` (BankApp-verified dark palette) and `UISprites` (Rounded/Capsule/Circle/Donut) — use them instead of per-mod copies.

### IL2CPP pitfalls (checklist; full list in `docs/pitfalls.md`)

- `[RegisterTypeInIl2Cpp]` MonoBehaviours need a public `IntPtr` constructor — `public X(IntPtr ptr) : base(ptr) { }` — or the bridge crashes on injection.
- Button wiring: `onClick.AddListener(new UnityAction(...))` fails on IntPtr conversion; always `S1API.Utils.EventHelper.AddListener` / `ButtonUtils.AddListener`, and prefer defensive Remove-before-Add when UIs rebuild (EventHelper dedupes by delegate instance).
- Never cast a proxy `Transform`: `(RectTransform)x.transform` throws at runtime — use `GetComponent<RectTransform>()` or `TryCast<T>()`. `is` checks are meaningless on IL2CPP proxies.
- TextMeshPro types live under `Il2CppTMPro`.
- Multiline `InputField`: set `textComponent`/`placeholder.alignment = UpperLeft`, `lineType = MultiLineNewline`, clean `offsetMin/Max`.
- Save-load timing: static lists (e.g. `Property.OwnedProperties`) are often still empty at scene load — refresh on `S1API.Lifecycle.GameLifecycle.OnLoadComplete` (verified 2026-09-29: scene 'Main' → `OnPreLoad` → `OnLoadComplete`; **`OnSaveInfoLoaded` fires 0× on game 0.4.7f6+** — see `schedule1-lifecycle-verify` §7).
- **Never raw object-typed MelonPreferences entries** (quit-save crash `Tomlet: Cannot serialize N as an enum of type UnityEngine.KeyCode`): Tomlet registers composite deserializers under `typeof(object)` (bug), so the last enum type loaded hijacks `MelonPreferences_Entry<object>` loads — ints become nameless KeyCode values (crash at quit), other types silently lose stored values. Use `ModConfig<T>` (typed entries since 2026-10-09). Full analysis: `docs/pitfalls.md` §1.3 + §2.
- `UIFactory.Text` anchors at (0.5, 0.5) — for custom rows set `anchorMin/anchorMax/offsetMin/offsetMax` manually.

### Shared utilities (`S1Mods.Shared`, `Source/Mods/Shared/`)

- **PatchGuard** (drift-tolerant `TryPatch` + `Report()` + `FailedTargets`) · **Diagnostics** (spam-free `ErrorOnce`/`WarnOnce`, `StartupReport` block) · **SafeStorage** (atomic writes, `.bak`, tolerant JSON) · **SaveSlots** (slot suffixes, ≥ 0 guard) · **NetworkGuard**/**SceneGate** (MP authority, scene gating) · **SafeInvoker**, **GameObjectResolver**, **HotkeyManager**, **ModConfig**, **ModLogger**, **TypeResolver**.
- **UITheme** / **GamePalette** / **UISprites** (UI system) · **AudioHelper**, **EconomyHelper**, **ShopListingSync** (misc helpers).
- New reusable code belongs here (`Source/Mods/Shared/src/`), never copied between mods; mods must not reference each other (`docs/architecture.md`).

---

## 6. Workflows

### Quality gates (same set in CI, pre-commit and local)

```pwsh
dotnet format Source/Mods/S1Mods.sln --verify-no-changes
pwsh Tools/gen-sln.ps1                # then: git diff --exit-code -- Source/Mods/S1Mods.sln
pwsh Tools/check-version-sync.ps1     # code <-> mod.json <-> README/AGENTS must match
pwsh Tools/check-doc-paths.ps1        # every repo path referenced in docs must exist
```

- **CI** (`.github/workflows/ci.yml`): always runs restore, format, the game-free tests (xUnit suites plus the `LightControl.Tests` console host), SLN determinism, version-sync and doc-path checks. Build + full test suite run only where game assemblies exist (self-hosted runner) and use `S1NoDeploy=true`. Checkout uses `submodules: recursive`, because some contract tests read pinned S1API sources under `ThirdParty/`.
- **Release** (`.github/workflows/release.yml`): manual `workflow_dispatch` only; fails fast without a runner that has the game installed.
- **Pre-commit hook** (`.githooks/pre-commit`): opt-in — `git config core.hooksPath .githooks`.
- **Green locally is not green in CI.** The working copy carries state that a checkout does not: git-ignored files (`ThirdParty/*/local.build.props`, `GameReferences/`, local `*.zip`), initialised submodules, and old `bin`/`obj` output. Gates that read any of it pass here and fail on the runner. Whenever you touch docs, tests, `.gitignore`, `.github/` or anything under `ThirdParty/`, run the gates once in a fresh clone:

  ```pwsh
  git clone --quiet . "$env:TEMP\ci-repro"; cd "$env:TEMP\ci-repro"
  git submodule update --init --recursive
  pwsh Tools/test-all.ps1 -Selection pure
  pwsh Tools/check-doc-paths.ps1
  pwsh Tools/check-version-sync.ps1
  pwsh Tools/gen-sln.ps1; git diff --exit-code -- Source/Mods/S1Mods.sln
  ```

  Two real hits on 2026-10-24: `PocketShop.Tests.ShopDirectoryPolicyTests` reads `ThirdParty/S1API/S1API/Internal/Entities/Suppliers/SupplierRuntimeIds.cs` and failed without submodules, and `check-doc-paths` reported `ThirdParty/*/local.build.props` as dead links because `git check-ignore` **exits 128 inside a submodule** instead of answering, so the script's gitignored tolerance never fired.

### Mod update cycle

1. Edit code in `Source/Mods/<Name>/src/` (load the relevant skills first).
2. `dotnet build ... -c Release` — deploys automatically (game closed).
3. Launch via Steam, check `MelonLoader\Latest.log`.
4. Version bump via `Tools/bump-version.ps1`; record the in-game result in the mod's `CHANGELOG.md` **and** in [`docs/compatibility.md`](docs/compatibility.md) (the verification matrix), then run the quality gates above before committing.

### Pre-flight with S1Interop (advisory, local only)

```pwsh
dotnet tool install --global S1Interop --version 0.1.0-alpha.1
s1interop analyze "Source\Mods\<Name>\src\<Name>.csproj"
```

Always exits 0 — it is a report, not a gate; read the output. Known false positives: TFM/LangVersion complaints (set centrally in `Directory.Build.props`), mod-internal collection signatures, defensively guarded reflection. (2026-10-05: the tool was not found on PATH or in the workspace — install it as above before relying on `s1interop doctor/analyze`.)

### Maker-checker workflow

1. **Pre-task:** load the matching skills to collect constraints and gotchas.
2. **Execution:** implement against the architectural pillars; use separate coder/verifier subagents for generation vs. audit.
3. **Post-task:** update the affected skill, the mod's `CHANGELOG.md`, and this file whenever inventory, versions or dependencies change.

---

## 7. Reference Material

- **Game decompiles:** `GameReferences/` — regenerate with `pwsh Tools/bootstrap-game-references.ps1` (pinned ilspycmd 9.1.0.7988, project mode; regenerate after every game update). The local decompiles were regenerated for f12 on 2026-10-08 (2 234 files from `Assembly-CSharp.dll`); `GameReferences/` is gitignored, so never rely on it being in sync — for current ground truth use `ilspycmd` against `<GameDir>\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll` (scoped `DOTNET_ROOT`). The TaxiDriver build uses the live f12 assemblies.
- **S1API source:** `ThirdParty/S1API/` (submodule, commit `a4cbdd9` = tag `v3.2.1`; §1).
- **64 system analyses:** `Skills/schedule1-game-systems/references/`.
- **Archived mod sources:** `Source/Archive/`. The old `Knowledge/` folder from the pre-reinstall workspace no longer exists.
