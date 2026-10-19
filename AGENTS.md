# AGENTS.md

Workspace: the `Schedule1-mod-only` repository root (any path/user, e.g. `C:\Users\<you>\Schedule1-mod-only`) — MelonLoader IL2CPP modding workspace for *Schedule I* (TVGS, Steam Open Beta; installed runtime **v0.4.7f12** — see the override above; older verification baselines are listed separately). The repo lives **outside** the game install; builds and deploys resolve the game via the `SCHEDULE1_PATH` env var or the default `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`.

> **Current local runtime (2026-10-08):** `Latest.log` reports Schedule I **v0.4.7f12** and Unity **2022.3.62f2**; `appmanifest_3164500.acf` reports Steam build **25796331**. MelonLoader 0.7.3 loaded S1API **3.2.1-beta.8** and S1MAPI **2.0.1**. The deployed S1API DLL is the local PR #353 compatibility build documented for f11; its known hash still matches the preserved f11 build, and the DLL loads in the f12 log. This does **not** prove all S1API patches work on f12. TaxiDriver 0.8.2's no-deploy Release build passed against the live f12 references with 0 warnings/errors; the driver-safety update (0.8.3) was verified in-game on this runtime on 2026-10-11. `GameReferences/` was regenerated for f12 on 2026-10-08 (2 234 decompiled files from `Assembly-CSharp.dll`); for anything newer, use the live f12 assemblies directly. Provenance and the official S1API backup remain under `<GameDir>\UserData\S1API\LocalBuilds\2026-10-07-f11\README.md`.

Quick links: [`DEVELOPERS.md`](DEVELOPERS.md) — human build/test/debug workflow · [`docs/README.md`](docs/README.md) — documentation index · [`docs/architecture.md`](docs/architecture.md) — dependency boundaries · [`docs/pitfalls.md`](docs/pitfalls.md) — battle-tested gotchas · [`docs/compatibility.md`](docs/compatibility.md) — per-mod verification matrix · [`ThirdParty/README.md`](ThirdParty/README.md) — pinned dependencies · [`GameReferences/README.md`](GameReferences/README.md) — local decompiles · [`Skills/README.md`](Skills/README.md) — skill index.

> **For AI agents — read this file completely before touching the mods. Hard rules that have bitten us:**
> - Launch the game with `Start-Process "steam://rungameid/3164500"` (Steam App ID — keeps the Open Beta branch active and sets the correct working directory). Never start `Schedule I.exe` directly. **Close the game before building** — loaded DLLs are file-locked and the auto-deploy step fails.
> - Agent shells may not have the .NET SDK on PATH (`dotnet --list-sdks` is empty; only the runtime host exists). The SDK 8.0.425 is user-local: `$env:DOTNET_ROOT="$env:USERPROFILE\.dotnet"; $env:PATH="$env:USERPROFILE\.dotnet;$env:PATH"`.
> - The default shell is Windows PowerShell 5.1 with script execution blocked — run repo scripts as `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools\<script>.ps1`.
> - Versions are machine-checked. Never hand-edit a version; use `pwsh Tools/bump-version.ps1 -Mod <Name> -Version x.y.z`.
> - Do not commit or push on your own — wait for Dominik's explicit go.
> - If you find something here wrong or outdated, update it.

---

## 0. AI-Agent Skills (`Skills/`)

Twenty-two skills under `Skills/<skill-name>/SKILL.md` (+ `references/`). Load them via the `skill` tool — **always start with `schedule1-modding`**, then the matching specialty. Index: [`Skills/README.md`](Skills/README.md).

- **Core runbooks:** `schedule1-modding` (scaffold/build/deploy, architecture — primary) · `schedule1-phoneapp` (Method-3 responsive UI, input focus, lifecycle rules) · `schedule1-troubleshooting` (log triage, crash patterns, IL2CPP pitfalls, slot recovery) · `schedule1-knowledge` (research: decompiles, S1API source, 64 curated systems).
- **Framework references:** `schedule1-s1api` · `schedule1-s1mapi` · `schedule1-game-systems` (64 systems, hook points) · `schedule1-mcp` (live game introspection, TCP :8765).
- **Domain:** `schedule1-economy` · `schedule1-persistence` · `schedule1-items` · `schedule1-grid` · `schedule1-interiors` · `schedule1-world-expansion` · `schedule1-custom-npcs` · `schedule1-3d-assets`.
- **Reusable patterns:** `schedule1-networked-world-state` · `schedule1-harmony-bootstrap` · `schedule1-il2cpp-reflection` · `schedule1-debounced-reload` · `schedule1-runtime-unity-cache` · `schedule1-lifecycle-verify`.

The skill list is anchored in three files (this one, `Skills/README.md`, `README.md`) — update all three when adding a skill.

---

## 1. Setup

- **Game:** `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` — override with `$env:SCHEDULE1_PATH` **before** `dotnet build` (MSBuild reads it once at startup).
- **Installed versions:** runtime game v0.4.7f12 (Steam build 25796331; Unity 2022.3.62f2; verified 2026-10-08 via Latest.log/appmanifest) · MelonLoader 0.7.3 (net6) · S1API 3.2.1 stable · S1MAPI 2.0.1. The compatibility matrix distinguishes assembly load/build from gameplay verification. TFM `net6.0`, `LangVersion` 12, nullable enabled.
  - **S1API 3.2.1 (2026-10-09):** official stable release installed, replacing the unpublished local beta.8+PR#353 compatibility build. It carries 0.4.7 (Special Customers) support on Mono + IL2CPP. Installed DLL hashes: IL2CPP `85b218ff…` / loader `d58b1d3c…`; the replaced DLLs and the source hash are preserved in `<GameDir>\UserData\S1API\LocalBuilds\2026-10-09-3.2.1`. The `ThirdParty/S1API` submodule is checked out at tag `v3.2.1` (commit `a4cbdd9`); the superproject pointer still records `v3.2.1-beta.8` and must be bumped deliberately per the sync rule. Full solution rebuild against 3.2.1: 0 errors / 0 warnings; all mod DLLs redeployed.
- **Install state (after the 2026-10-07 Windows reinstall):** .NET 6.0.36 runtime global in `C:\Program Files\dotnet` and user-local for tests; .NET SDK 8.0.425 user-local in `$env:USERPROFILE\.dotnet`; Python 3.13 with MCP dependencies isolated in `%LOCALAPPDATA%\Schedule1Tools\s1mcp-venv`; official nuget.org package source configured. Older repo copies (in the game dir / on the Desktop) are obsolete — this path is the only truth.
- **Runtime dependencies in the game dir:** S1API at `Mods\S1API.Il2Cpp.MelonLoader.dll` — the build probes exactly that filename (a wrong name breaks every mod with `CS0103: The name 'S1API' does not exist`); S1MAPI at `UserLibs\S1MAPI_Il2Cpp.dll`. `MelonLoader\Il2CppAssemblies\` only exists after the game has run once — until then, mods cannot reference Unity/IL2CPP types.
- **Launch, logs, live debugging:** `Start-Process "steam://rungameid/3164500"`; gameplay log `MelonLoader\Latest.log`; S1MCP bridge at `ThirdParty/S1MCPServer-master/` (TCP `127.0.0.1:8765`) — see `schedule1-mcp`.

**Layout:**

```text
Source/Mods/       Active mods + Shared lib, Directory.Build.props/targets, S1Mods.sln
Source/Archive/    Archived mods & tests (not in the solution, never built)
Source/Tests/      xUnit suites: Shared.Tests, AutoPackagingStation.Tests, CalculatorApp.Tests
GameReferences/    Locally generated decompiles (gitignored; Tools/bootstrap-game-references.ps1)
Skills/            22 AI-agent skills (index: Skills/README.md)
ThirdParty/        Pinned deps & archives: S1API, S1MAPI, S1MCPServer-master, Archive/
Tools/             16 helpers: build-all, gen-sln, new-mod, bump-version, package-release, release-mod,
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

> **Status convention:** `Verified <date>` = confirmed in a real game session (assertion only, no artifact in the repo). `In-Game-Verify open` = code complete, not yet confirmed in-game. Keep the two apart — never claim a verify that did not happen. Latest recorded owner confirmations: 2026-10-15 (MessagesPlus v0.4.6 popup/response repair and StackLimitMod v0.1.7 ingredient stacking; original test dates not supplied; AutoPackagingStation v0.4.7 tested session). Previous verification round: 2026-10-11 (PhoneScrollFix v2.0.0; Minimap settings app; PotScanner v0.7.0, BankApp v0.5.0, Weather v0.4.0, StorageScanner v0.2.0, TaxiDriver v0.8.3); previous rounds 2026-10-10 (MessagesPlus v0.4.5 dark-mode polish + ProductAppQoL v0.1.0), 2026-09-30 (MessagesPlus v0.4.0), 2026-09-20.

| Mod | Status | Path in `Source/` | Path in `<Game>\Mods\` | Key bits |
|---|---|---|---|---|
| **NotesApp** | ✅ active (v1.0.3, **Verified 2026-09-15**) | `Mods/NotesApp/` | `NotesApp.dll` + icon | PhoneApp, SafeStorage, UITheme, InputFocus |
| **PotScanner** | ✅ active (v0.7.0, **Verified 2026-10-11**; core Verified 2026-09-15) | `Mods/PotScanner/` | `PotScanner.dll` | PhoneApp, Property/Growing/Money, GamePalette rewrite |
| **CalculatorApp** | ✅ active (v0.2.3, **Verified 2026-09-15**) | `Mods/CalculatorApp/` | `CalculatorApp.dll` | PhoneApp, Money, SafeStorage |
| **CustomSkateboard** | ✅ active (v1.1.5, **Verified 2026-09-15**) | `Mods/CustomSkateboard/` | `CustomSkateboard.dll` + icon | Harmony, visuals, avatar safety |
| **MoreSaveSlots** | ✅ active (v1.0.12, **Verified 2026-09-15**) | `Mods/MoreSaveSlots/` | `MoreSaveSlots.dll` | no S1API (MelonMod + Harmony) |
| **PocketShop** | ✅ active (v0.3.7, **In-Game-Verify open**; last real check v0.3.2 on v0.4.6f13, 2026-09-20) | `Mods/PocketShop/` | `PocketShop.dll` | PhoneApp, vanilla PaymentType, level locks |
| **BankApp** | ✅ active (v0.5.0, **Verified 2026-10-11**; v0.4.4 **Verified 2026-09-15**) | `Mods/BankApp/` | `BankApp.dll` + icon | PhoneApp, banking, slot isolation |
| **Weather** | ✅ active (v0.4.0, **Verified 2026-10-11**) | `Mods/Weather/` | `Weather.dll` + icon | PhoneApp, read-only dashboard |
| **BusinessIncome** | ✅ active (v0.1.6, **Verified 2026-09-19**) | `Mods/BusinessIncome/` | `BusinessIncome.dll` | host authority, slot isolation, console |
| **StackLimitMod** | ✅ active (v0.1.7, **Verified, owner-confirmed 2026-10-15**; original test date not supplied; v0.1.6 **Verified 2026-09-20**) | `Mods/StackLimitMod/` | `StackLimitMod.dll` | Harmony, Registry, agriculture-only |
| **Minimap** | 🧪 development (v2.3.4-dev.1; waypoint safety + opt-in Phase-0 observer, 186 tests; **In-Game-Verify open**; historical HUD/settings acceptance below) | `Mods/Minimap/` | `Minimap.dll` | Existing HUD/settings; waypoint persistence repair; no playable search mode yet |
| **AutoPackagingStation** | ✅ active (v0.4.7, **Verified 2026-10-15, owner-confirmed tested session**; 66 tests; Release deployed, retirement-scoped review PASS; multiplayer/legacy migration unverified) | `Mods/AutoPackagingStation/` | `AutoPackagingStation.dll` | Buildable, SafeStorage, GLB |
| **HitmanPhone** | ✅ active (v0.2.9, beta **Verified 2026-09-24**) | `Mods/HitmanPhone/` | `HitmanPhone.dll` | Messages/NPC/Items/Quests |
| **MessagesPlus** | ✅ active (v0.4.6, **Verified, owner-confirmed 2026-10-15**; popup/response repair accepted; original test date not supplied; 25 tests passed) | `Mods/MessagesPlus/` | `MessagesPlus.dll` | vanilla-Messages patches + permanent whole-app dark mode |
| **ProductAppQoL** | ✅ active (v0.1.0, **Verified 2026-10-10**) | `Mods/ProductAppQoL/` | `ProductAppQoL.dll` | Product app: asking-price −/+ buttons respond to mouse/touch (tap = ±$1, hold = repeat) — vanilla wires them gamepad-only via `InputValueRamp` (dead buttons for PC players) |
| **TaxiDriver** | 🧪 bounded reverse supervision (v0.8.19; 19 CS + project; 605 tests; final delta review PASS; In-Game-Verify open; prior driver-safety **Verified 2026-10-11** on 0.8.3) | `Mods/TaxiDriver/` | `TaxiDriver.dll` + GLB data folder | TaxiApp-only service, own NPC driver, vehicle ownership guards, S1MAPI |
| **StorageScanner** | 🆕 new (v0.2.0, **Verified 2026-10-11**) | `Mods/StorageScanner/` | `StorageScanner.dll` + icon | PhoneApp, read-only storage overview |
| **S1MCP** | ✅ active (v1.0.4, **verified 2026-10-17** — 10/10 integration checks in the main menu, fast path proven via `inline_requests`; quick-win endpoints `set/get_time_of_day`, `get_weather`, `list_mods` added; read paths verified in-game in a loaded world, `set_time_of_day` write path verified 2026-10-17 (`changed:true` + readback; the synchronous readback can lag one apply — `changed:false` with a stale readback is the known async-apply race, not a failed write)) | `ThirdParty/S1MCPServer-master/` | `S1MCPServer-IL2CPP.dll` | MCP over TCP :8765; fast path answers `handshake`/`status`/`list_methods`/`heartbeat` on the server thread without game focus; `list_mods` works from the menu, time/weather degrade to a clean `available:false` until the world loads; `load_save` loaded the owner's "Tests" slot from the menu |
| **LightControl** | 🧪 coupled LED + light test build (v0.1.0-dev.5; 143 tests, targeted review PASS; In-Game-Verify open) | `Mods/LightControl/` | `LightControl.dll` | one instruction colors LEDLight panel and emitted light; relative native intensity, guarded Original/teardown; dev.4 native-material identity retained; plant materials untouched, colored illumination intended; FullSpectrum/halogen/world-lamp/MP transport unsupported |
| **PropertyLEDs** | 🧪 experimental (v0.1.7; larger phone typography and fitting text bands 2026-10-18, 94 tests, scoped review PASS; In-Game-Verify open; 0.1.4 owner UI acceptance is historical) | `Mods/PropertyLEDs/` | `PropertyLEDs.dll` + icon | independent RGB ceiling LEDs, per-room overrides, slot-isolated settings, segment-beam geometry, measured layout catalog + reference-ported coverage |
| **PhoneScrollFix** | ✅ active (v2.0.0, hover look **Verified 2026-10-11**) | `Mods/PhoneScrollFix/` | `PhoneScrollFix.dll` | home-screen icon hover: clipped slim frame + RectMask2D viewport (white-plate fix) |
| **WorldLook** | 🧪 experimental (v0.1.2; GoldenHour + WaterShader + GraphicsProbe merged into one DLL, 2026-10-17; milder warm bias **light look owner-confirmed 2026-10-18**; probe is manual-only; review PASS-with-nits, both nits fixed; DLL SHA-256 `0a44aa0a435e8d272810018c4f2c2488e66251268237831c2a7575a529063681`; manual probe capture **verified 2026-10-18**; 0.1.2 water clarity step (`_Absorption` 0.38 + `_ShallowTint` red 0.50, foam reverted) **In-Game-Verify open**) | `Mods/WorldLook/` | `WorldLook.dll` + `s1reference.bundle` | one world look: clock-weighted light (Ctrl+Shift+F7), reference water on `StylizedWater2_Ocean`, manual probe (Ctrl+Shift+F8). Old GoldenHour/WaterShader/GraphicsProbe DLLs must not stay loaded |
| **Shared** | ✅ active (workspace lib) | `Mods/Shared/` | `Shared.dll` | see §5 |
| **S1API** | ✅ active (3.2.1 stable, submodule `ThirdParty/S1API/` at tag `v3.2.1`) | — | `S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.MelonLoader.dll` | required for 0.4.7f6+; 3.2.1 adds released Special Customers (0.4.7) support |

> **Archived** (source preserved under `Source/Archive/`, not in `S1Mods.sln`, never built): BackpackMod v1.2.3 · DayCounter v1.0.0 · HomelessMod v0.1.12 · ProfitTracker v1.1.0 · snackVendor v0.0.9 · TVBrowser v0.1.0 · _DiagPerfCounter v0.3.2, plus `Source/Archive/Tests/BackpackMod.Tests`.
> **Removed / retired third-party:** MikuPlayerModel (discontinued) · ConstructionSiteProperty (disabled) — both remain as `*.dll.bak` (MelonLoader never loads `.bak`) · MoreDrugs (removed 2026-09-19) · PhoneScroll (retired 2026-09-16, `ThirdParty/Archive/PhoneScroll/`) · Herer's Minimap (removed 2026-09-17) · Sideload (dropped 2026-09-10) · Hash (`ThirdParty/Archive/ScheduleOne-Hash/`, reference only).
>
> **S1API note:** stay on the 3.2.1-beta line. The 3.2.0 stable build does not know the 0.4.7f6 renames and spams `MissingMethodException: NPCHealth.set_npc` (kills 3 patch classes).

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

**PocketShop v0.3.7 (2026-10-18):**
- Fixed four-column grid with three visible rows, scroll overflow and subtle trailing placeholder cells; stable tile size across catalogs/search. Two-line name band with font-metric headroom. 52 tests pass; current visual acceptance open.
- Phone shop browser (grid + detail modal, inline quantity). Since v0.3.1 payment follows the vanilla `shopInterface.PaymentType` — cash shops use `ChangeCashBalance`, online shops `CreateOnlineTransaction`; level/rank locks enforced; host-safe refunds.
- **In-Game-Verify open for 0.3.3–0.3.7.** Last real session: v0.3.2 on v0.4.6f13 (2026-09-20, "done, nothing unusual"). The 0.3.3 payment rewrite and the 0.3.4–0.3.6 layout/header polish have no recorded in-game check; the earlier "Verified 2026-09-20" label had been carried onto 0.3.6 by bumps that rewrote only the version cell. History: `Source/Mods/PocketShop/docs/CHANGELOG.md`.

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

**HitmanPhone v0.2.9 (2026-09-15):**
- Bounty contracts via phone messages (anonymous callers): kill → Polaroid evidence → dead-drop payout; police heat, journal quests, 3-day expiry, slot-isolated save; `KnockOut` patch + double-drop latch + orphan-quest cleanup.
- Beta verified 2026-09-24 (offer → accept → receipt → payout). History: `Source/Mods/HitmanPhone/docs/CHANGELOG.md`.

**MessagesPlus v0.4.6 (2026-10-12, scoped popup/response dark-mode repair):**
- **Verified, owner-confirmed 2026-10-15:** Dominik confirmed v0.4.6 works in-game, closing the popup/response repair acceptance item. Confirmation recorded on this date; original test date and individual actions were not supplied. This is owner-reported acceptance, not a new independent log or visual check.
- Removed contradictory blanket vanilla-color exclusions for deal-time and response menus; selective neutral surface/text policies retain clock/art/accent colors and native interactions. 25 tests pass; Release build clean. Historical verification below is retained separately.
- **Whole-app dark mode regression fixed and verified in-game 2026-10-09.** v0.4.3's rewrite of `AppTheme` had removed the broad sweep that tints every remaining near-white surface AND tightened the near-white detector (`Spread <= 0.08`), so the whole app stayed vanilla-white. The v0.4.0 sweep and its permissive colour gate are restored; the sweep runs as the FINAL pass, after the targeted ones, so it only fills the gaps (`ThemeColorPolicy` makes re-tinting an already-dark surface a no-op). Confirmed dark by Dominik (screenshots 13:39 / 13:43).

**MessagesPlus implementation history (2026-09-29/30):**
- Patch-only mod (no PhoneApp icon) on the vanilla `MessagesApp`: sticky search band under the title (live search, `[All][Customer][Dealer][Supplier]` chips, unread counter), "⋯" menu with Clear Read / Clear All — view-only filtering, host-only + customer-only mutations, one-time legacy restore of v0.1.x trashed threads.
- **Whole-app dark mode** (`AppTheme`): **permanent since v0.4.1** — the "⋯" menu toggle is gone, the config defaults to ON and a stale `false` self-heals at startup (`MessagesPlusConfig.DarkMode` stays for schema stability). Recolours our band + vanilla app surfaces (page backgrounds, inbox rows, chat bubbles + tails, dialogue header, generic near-white sweep) — one-time per graphic with cached originals; dead-drop/order and counter-offer subtrees are restored to vanilla colors and excluded from sweeps; avatars, badges, the unread dot, and product sprites remain untouched.
- **Vanilla order menus (2026-10-08):** `MSGConversation.responseContainer` and the verified `DealWindowSelector.Container` are preserved/restored; the deal-window hook no longer darkens popup content. In-Game-Verify open.
- Round-1 fixes: `(RectTransform)x.transform` casts → `GetComponent<RectTransform>()` (IL2CPP cast made the "⋯" menu dead), search surface keeps `raycastTarget=true` (v0.4.0 **Verified 2026-09-30**). History: `Source/Mods/MessagesPlus/docs/CHANGELOG.md`.

**Minimap v2.3.4-dev.1 (2026-10-18 — waypoint safety and opt-in Phase-0 observation; In-Game-Verify open):**
- Property-based waypoint JSON, bounded/validated rows, original-path pending IO and A -> B -> A retention; corruption/backup recovery is read-only until explicit clear or manual repair. Console save failures are visible. Parent observed RED/GREEN review regressions; 186 tests, Release 0 warnings/errors and scoped format checks pass.
- `minimap searchdev` is read-only and OFF by default; explicit bounded development observations are separate from gameplay. Prepared search core is not runtime-wired. Hints UI, curated area overlay, production binding and search-store IO remain unimplemented. This is NOT a finished search feature.
- Development DLL/PDB/metadata deployed and hash-matched; live registry and startup in the menu/disposable test world confirm loading plus HUD/four settings pages initialized. Manual waypoint roundtrip was postponed by the owner; gameplay acceptance remains open. Previous DLL/metadata and native test-save backups are preserved outside scratch.
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

**TaxiDriver v0.8.19 (2026-10-18, bounded reverse supervision; In-Game-Verify open):**
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
# Ensure the user-local SDK first (agent shells; see hard rules at the top)
$env:DOTNET_ROOT="$env:USERPROFILE\.dotnet"; $env:PATH="$env:USERPROFILE\.dotnet;$env:PATH"

# Single mod (builds and auto-deploys)
dotnet build Source\Mods\NotesApp\src\NotesApp.csproj -c Release

# Whole solution / all mods
pwsh Tools\build-all.ps1          # wrapper; or: dotnet build Source\Mods\S1Mods.sln -c Release

# Compile-only, no deploy to the game dir (CI uses this)
dotnet build Source\Mods\S1Mods.sln -c Release -p:S1NoDeploy=true

# Tests
dotnet test Source\Mods\S1Mods.sln -c Release            # all three suites (needs game assemblies)
dotnet test Source\Tests\AutoPackagingStation.Tests\AutoPackagingStation.Tests.csproj -c Release  # pure logic, no game

# Regenerate the solution after adding/removing a project
pwsh Tools\gen-sln.ps1

# Scaffold a new mod
pwsh Tools\new-mod.ps1 -Name <ModName> -Author Dominik -Version 0.1.0

# Release ZIP (needs game assemblies; excludes Shared + _DiagPerfCounter)
pwsh Tools\package-release.ps1 [-Mod <Name>]
```

### What a build deploys (automatic, via `Directory.Build.targets`)

- `Mods\` ← DLL + `*.png` icons + `*.bundle`; GLB meshes go to `Mods\<Mod>\` (data subfolder — MelonLoader loads only DLLs, runtime asset paths resolve there, e.g. TaxiDriver).
- `UserData\<Mod>\` ← `mod.json` + `<Mod>.pdb`. **Never put json/pdb into `Mods\`** (reference-only assemblies there also spam `BadImageFormatException` at game start).
- `S1NoDeploy=true` skips all copies. Every build force-deploys (`skipUnchangedFiles=false`) — good against stale DLLs, but the game must be closed.

### Build caveats & common failures

- `$env:SCHEDULE1_PATH` must be exported **before** MSBuild starts.
- First clone on a new PC: `git submodule update --init --recursive` then `pwsh Tools/setup-workspace.ps1` (writes the gitignored `local.build.props` for S1API/S1MAPI; can activate the git hook).
- `S1Mods.sln` is generated (`gen-sln.ps1`, deterministic MD5 GUIDs, CI-checked) — regenerate, never hand-edit.
- Version bumps: `pwsh Tools/bump-version.ps1 -Mod <Name> -Version x.y.z` — syncs the mod's code (`MelonInfo`; PotScanner uses `Constants.ModVersion`), its `mod.json` + `CHANGELOG.md`, the AGENTS.md matrix row + detail header, and the README row.
- Failure signatures:
  - `CS0103: The name 'S1API' does not exist` → S1API missing or misnamed in `Mods\`.
  - `UnityEngine.* / Assembly-CSharp not found` at build → the game never ran; `MelonLoader\Il2CppAssemblies\` is empty.
  - Copy error `used by another process` → close the game before building.
  - `MissingMethodException: NPCHealth.set_npc` spam → wrong S1API line (use 3.2.1-beta.x).
  - `BadImageFormatException` at game start → a reference assembly sits in `Mods\`.
  - `dotnet: SDK not found` → set `DOTNET_ROOT`/`PATH` (see top).

### Multi-PC workflow

Linear on `main`, no feature branches. On a new PC: clone → `setup-workspace.ps1` → build. For an independent laptop sandbox (no GitHub fork): `pwsh Tools/new-laptop-workspace.ps1` (renames `origin` → `upstream`; sync with `git fetch upstream && git merge upstream/main`). Game saves in `<GameDir>\UserData\` are PC-local and never leave it.

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

- **PatchGuard** (drift-tolerant `TryPatch` + `Report()`) · **SafeStorage** (atomic writes, `.bak`, tolerant JSON) · **SaveSlots** (slot suffixes, ≥ 0 guard) · **NetworkGuard**/**SceneGate** (MP authority, scene gating) · **SafeInvoker**, **GameObjectResolver**, **HotkeyManager**, **ModConfig**, **ModLogger**, **TypeResolver**.
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

- **CI** (`.github/workflows/ci.yml`): always runs restore, format, pure-logic tests (AutoPackagingStation.Tests + CalculatorApp.Tests), SLN determinism, version-sync and doc-path checks. Build + full test suite run only where game assemblies exist (self-hosted runner) and use `S1NoDeploy=true`.
- **Release** (`.github/workflows/release.yml`): manual `workflow_dispatch` only; fails fast without a runner that has the game installed.
- **Pre-commit hook** (`.githooks/pre-commit`): opt-in — `git config core.hooksPath .githooks`.

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
- **S1API source:** `ThirdParty/S1API/` (submodule, pinned beta line — currently the beta.8 tag).
- **64 system analyses:** `Skills/schedule1-game-systems/references/`.
- **Archived mod sources:** `Source/Archive/`. The old `Knowledge/` folder from the pre-reinstall workspace no longer exists.
