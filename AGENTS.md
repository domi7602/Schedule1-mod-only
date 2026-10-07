# AGENTS.md

Workspace: the `Schedule1-mod-only` repository root (any path/user, e.g. `C:\Users\<you>\Schedule1-mod-only`) — MelonLoader IL2CPP modding workspace for *Schedule I* (TVGS, Steam Open Beta; documented baseline **v0.4.7f9**). The repo lives **outside** the game install; builds and deploys resolve the game via the `SCHEDULE1_PATH` env var or the default `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`.

> **Current local runtime override (2026-10-07):** The fresh Windows installation runs **game v0.4.7f11**, Steam build **25770926**, with MelonLoader 0.7.3 and **S1API beta.8 + the unpublished compatibility fix from [PR #353](https://github.com/ifBars/S1API/pull/353)**. Official beta.8 fails to patch the removed `ProductIconManager.GenerateIcons` on f11. The local DLL was built and deployed after 749 IL2CPP contract tests passed; startup/gameplay with it remain unverified. The submodule stays unmodified at the beta.8 tag. Build provenance, modified source, and the official DLL backup are preserved in `<GameDir>\UserData\S1API\LocalBuilds\2026-10-07-f11\README.md`. Older f9 compatibility claims below are historical, not f11 gameplay verification.

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

Twenty skills under `Skills/<skill-name>/SKILL.md` (+ `references/`). Load them via the `skill` tool — **always start with `schedule1-modding`**, then the matching specialty. Index: [`Skills/README.md`](Skills/README.md).

- **Core runbooks:** `schedule1-modding` (scaffold/build/deploy, architecture — primary) · `schedule1-phoneapp` (Method-3 responsive UI, input focus, lifecycle rules) · `schedule1-troubleshooting` (log triage, crash patterns, IL2CPP pitfalls, slot recovery) · `schedule1-knowledge` (research: decompiles, S1API source, 64 curated systems).
- **Framework references:** `schedule1-s1api` · `schedule1-s1mapi` · `schedule1-game-systems` (64 systems, hook points) · `schedule1-mcp` (live game introspection, TCP :8765).
- **Domain:** `schedule1-economy` · `schedule1-persistence` · `schedule1-items` · `schedule1-grid` · `schedule1-interiors` · `schedule1-custom-npcs` · `schedule1-3d-assets`.
- **Reusable patterns:** `schedule1-harmony-bootstrap` · `schedule1-il2cpp-reflection` · `schedule1-debounced-reload` · `schedule1-runtime-unity-cache` · `schedule1-lifecycle-verify`.

The skill list is anchored in three files (this one, `Skills/README.md`, `README.md`) — update all three when adding a skill.

---

## 1. Setup

- **Game:** `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` — override with `$env:SCHEDULE1_PATH` **before** `dotnet build` (MSBuild reads it once at startup).
- **Baseline versions:** game v0.4.7f9 (Unity 2022.3, IL2CPP; verified 2026-10-05) · MelonLoader 0.7.3 (net6) · S1API 3.2.1-beta.8; submodule at beta.8 tag. See the current local runtime override above for the installed f11 compatibility build. TFM `net6.0`, `LangVersion` 12, nullable enabled.
- **Install state (after the 2026-10-07 Windows reinstall):** .NET 6.0.36 runtime global in `C:\Program Files\dotnet` and user-local for tests; .NET SDK 8.0.425 user-local in `$env:USERPROFILE\.dotnet`; Python 3.13 with MCP dependencies isolated in `%LOCALAPPDATA%\Schedule1Tools\s1mcp-venv`; official nuget.org package source configured. Older repo copies (in the game dir / on the Desktop) are obsolete — this path is the only truth.
- **Runtime dependencies in the game dir:** S1API at `Mods\S1API.Il2Cpp.MelonLoader.dll` — the build probes exactly that filename (a wrong name breaks every mod with `CS0103: The name 'S1API' does not exist`); S1MAPI at `UserLibs\S1MAPI_Il2Cpp.dll`. `MelonLoader\Il2CppAssemblies\` only exists after the game has run once — until then, mods cannot reference Unity/IL2CPP types.
- **Launch, logs, live debugging:** `Start-Process "steam://rungameid/3164500"`; gameplay log `MelonLoader\Latest.log`; S1MCP bridge at `ThirdParty/S1MCPServer-master/` (TCP `127.0.0.1:8765`) — see `schedule1-mcp`.

**Layout:**

```text
Source/Mods/       Active mods + Shared lib, Directory.Build.props/targets, S1Mods.sln
Source/Archive/    Archived mods & tests (not in the solution, never built)
Source/Tests/      xUnit suites: Shared.Tests, AutoPackagingStation.Tests, CalculatorApp.Tests
GameReferences/    Locally generated decompiles (gitignored; Tools/bootstrap-game-references.ps1)
Skills/            20 AI-agent skills (index: Skills/README.md)
ThirdParty/        Pinned deps & archives: S1API, S1MAPI, S1MCPServer-master, Archive/
Tools/             15 helpers: build-all, gen-sln, new-mod, bump-version, package-release, release-mod,
                   check-version-sync, check-doc-paths, deploy-thirdparty, bootstrap-game-references,
                   setup-workspace, new-laptop-workspace, backup-to-d, mods-cleanup-inventory,
                   check-skills-health
assets/            Screenshot convention (assets/README.md)
docs/              architecture.md · pitfalls.md · release-process.md
Release/           Release ZIP output
.githooks/         Git hooks (opt-in: git config core.hooksPath .githooks): pre-commit + pre-push
```

---

## 2. Mod Inventory

> **Status convention:** `Verified <date>` = confirmed in a real game session (assertion only, no artifact in the repo). `In-Game-Verify open` = code complete, not yet confirmed in-game. Keep the two apart — never claim a verify that did not happen. Latest completed verify: 2026-09-30 (MessagesPlus); previous round 2026-09-20.

| Mod | Status | Path in `Source/` | Path in `<Game>\Mods\` | Key bits |
|---|---|---|---|---|
| **NotesApp** | ✅ active (v1.0.3, **Verified 2026-09-15**) | `Mods/NotesApp/` | `NotesApp.dll` + icon | PhoneApp, SafeStorage, UITheme, InputFocus |
| **PotScanner** | ✅ active (v0.7.0, **In-Game-Verify open**; core Verified 2026-09-15) | `Mods/PotScanner/` | `PotScanner.dll` | PhoneApp, Property/Growing/Money, GamePalette rewrite |
| **CalculatorApp** | ✅ active (v0.2.3, **Verified 2026-09-15**) | `Mods/CalculatorApp/` | `CalculatorApp.dll` | PhoneApp, Money, SafeStorage |
| **CustomSkateboard** | ✅ active (v1.1.5, **Verified 2026-09-15**) | `Mods/CustomSkateboard/` | `CustomSkateboard.dll` + icon | Harmony, visuals, avatar safety |
| **MoreSaveSlots** | ✅ active (v1.0.12, **Verified 2026-09-15**) | `Mods/MoreSaveSlots/` | `MoreSaveSlots.dll` | no S1API (MelonMod + Harmony) |
| **PocketShop** | ✅ active (v0.3.2, **Verified 2026-09-20**) | `Mods/PocketShop/` | `PocketShop.dll` | PhoneApp, vanilla PaymentType, level locks |
| **BankApp** | ✅ active (v0.4.5, **In-Game-Verify open**; v0.4.4 **Verified 2026-09-15**) | `Mods/BankApp/` | `BankApp.dll` + icon | PhoneApp, banking, slot isolation |
| **Weather** | ✅ active (v0.4.0, In-Game-Verify open) | `Mods/Weather/` | `Weather.dll` + icon | PhoneApp, read-only dashboard |
| **BusinessIncome** | ✅ active (v0.1.6, **Verified 2026-09-19**) | `Mods/BusinessIncome/` | `BusinessIncome.dll` | host authority, slot isolation, console |
| **StackLimitMod** | ✅ active (v0.1.7; v0.1.6 **Verified 2026-09-20**) | `Mods/StackLimitMod/` | `StackLimitMod.dll` | Harmony, Registry, agriculture-only |
| **AutoPackagingStation** | ✅ active (v0.3.3, **Verified 2026-09-20**) | `Mods/AutoPackagingStation/` | `AutoPackagingStation.dll` | Buildable, SafeStorage, GLB |
| **HitmanPhone** | ✅ active (v0.2.9, beta **Verified 2026-09-24**) | `Mods/HitmanPhone/` | `HitmanPhone.dll` | Messages/NPC/Items/Quests |
| **MessagesPlus** | ✅ active (v0.4.1, **In-Game-Verify open**; v0.4.0 **Verified 2026-09-30**) | `Mods/MessagesPlus/` | `MessagesPlus.dll` | vanilla-Messages patches + permanent whole-app dark mode |
| **TaxiDriver** | 🧪 spike (v0.8.2; Stages 1–3 live-verified 2026-09-25, Pakete C–G test round open) | `Mods/TaxiDriver/` | `TaxiDriver.dll` + GLB data folder | vehicles, own NPC driver, S1MAPI |
| **TabToHome** | ⏸️ shelved (v0.1.0, **not deployed**; game copy removed 2026-10-04, waiting for upstream S1API fix) | `Mods/TabToHome/` | `TabToHome.dll` (not deployed) | no S1API (MelonMod + Harmony): Tab → HomeScreen |
| **StorageScanner** | 🆕 new (v0.2.0, **In-Game-Verify open**) | `Mods/StorageScanner/` | `StorageScanner.dll` + icon | PhoneApp, read-only storage overview |
| **S1MCP** | ✅ active (v1.0.1) | `ThirdParty/S1MCPServer-master/` | `S1MCPServer-IL2CPP.dll` | MCP over TCP :8765 |
| **Shared** | ✅ active (workspace lib) | `Mods/Shared/` | `Shared.dll` | see §5 |
| **S1API** | ✅ active (3.2.1-beta.8, submodule `ThirdParty/S1API/` at the beta.8 tag) | — | `S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.MelonLoader.dll` | required for 0.4.7f6+ |

> **Archived** (source preserved under `Source/Archive/`, not in `S1Mods.sln`, never built): BackpackMod v1.2.3 · DayCounter v1.0.0 · HomelessMod v0.1.12 · Minimap v2.0.2 · ProfitTracker v1.1.0 · snackVendor v0.0.9 · TVBrowser v0.1.0 · _DiagPerfCounter v0.3.2, plus `Source/Archive/Tests/BackpackMod.Tests`.
> **Removed / retired third-party:** MikuPlayerModel (discontinued) · ConstructionSiteProperty (disabled) — both remain as `*.dll.bak` (MelonLoader never loads `.bak`) · MoreDrugs (removed 2026-09-19) · PhoneScroll (retired 2026-09-16, `ThirdParty/Archive/PhoneScroll/`) · Herer's Minimap (removed 2026-09-17) · Sideload (dropped 2026-09-10) · Hash (`ThirdParty/Archive/ScheduleOne-Hash/`, reference only).
>
> **S1API note:** stay on the 3.2.1-beta line. The 3.2.0 stable build does not know the 0.4.7f6 renames and spams `MissingMethodException: NPCHealth.set_npc` (kills 3 patch classes).

### Mod Details

**NotesApp v1.0.3 (2026-09-13):**
- Phone notepad: real-time search, pinning, edit/copy/clone/delete with confirm modal, in-game quick-stamp; atomic `notes.json` via SafeStorage; `UITheme` scaling + `NotesAppInputFocus` (typing does not move the character).
- v1.0.3 fixes: null-safe JSON load, slot-suffix ≥ 0. History: `Source/Mods/NotesApp/docs/CHANGELOG.md`.

**PotScanner v0.7.0 (2026-09-27, BankApp-palette view rewrite):**
- Grow dashboard: every `GrowContainer` per property (2 s polling, 0-alloc), filter chips (Thirsty/Ready/Empty), quality display, single-property focus, Water All (30 % skip) + persisted Auto-Water (50 g/pot); money paths host-only.
- v0.7.0 = pure view rewrite on the shared `S1Mods.Shared.GamePalette` / `UISprites`; behaviour unchanged. **In-Game-Verify open.** History: `Source/Mods/PotScanner/docs/CHANGELOG.md`.

**CalculatorApp v0.2.3 (2026-09-12):**
- Exact `decimal` calculator with searchable history, Cash/Bank insert chips, clipboard, repeating equals; `CalculatorAppInputFocus`; SafeStorage state.
- v0.2.3: overflow handling + slot ≥ 0 guard. History: `Source/Mods/CalculatorApp/docs/CHANGELOG.md`.

**CustomSkateboard v1.1.5 (2026-09-13):**
- Skateboard feel overhaul (carving, instant jump, push curve, anti-gravel) plus a deck visual swap with strict renderer whitelisting so the player avatar is never touched; runtime instance tuning at `OnMount` keeps vanilla boards untouched; Jeff store injection + `skate` console.
- History: `Source/Mods/CustomSkateboard/docs/CHANGELOG.md`.

**MoreSaveSlots v1.0.12 (2026-09-13):**
- 25+ save slots with paginated navigation, inline rename (with `Game.json.bak`), hotkeys/wheel; Harmony remaps keep the vanilla menus working; modal canvas/sorting fixes.
- History: `Source/Mods/MoreSaveSlots/docs/CHANGELOG.md`.

**PocketShop v0.3.2 (2026-09-20):**
- Phone shop browser (grid + detail modal, inline quantity). Since v0.3.1 payment follows the vanilla `shopInterface.PaymentType` — cash shops use `ChangeCashBalance`, online shops `CreateOnlineTransaction`; level/rank locks enforced; host-safe refunds.
- **Verified 2026-09-20.** History: `Source/Mods/PocketShop/docs/CHANGELOG.md`.

**BankApp v0.4.5 (2026-10-04):**
- Chip-based deposit/withdraw screen: weekly $10k ATM limit, two-column cash/online balances, double-entry booking with rollback, slot-isolated persistence, `NetworkGuard.IsInMainScene` guards.
- v0.4.5 = removes the phantom withdrawal cap ("inventory full"): the old capacity check multiplied free inventory slots by a cash stack limit that is 1 in vanilla, blocking nearly every withdrawal. **In-Game-Verify open.** History: `Source/Mods/BankApp/docs/CHANGELOG.md`.

**Weather v0.4.0 (2026-10-01):**
- Read-only phone dashboard of the nine weather components: accent-bordered hero card (name, percentage, intensity pill, meta count, ring gauge with a rounded accent arc) over nine live rows — active conditions tint their row, border, icon and bar, inactive rows stay neutral. Empty state, no persistence, no gameplay impact, no input field.
- v0.4.0 = rebuilt to the approved mockup (flat canvas without header chrome, name above the bar in each row, accent-tinted active rows). All anchors measured off the reference design as canvas fractions. **In-Game-Verify open.** History: `Source/Mods/Weather/docs/CHANGELOG.md`.

**BusinessIncome v0.1.6 (2026-09-17):**
- Daily passive revenue for owned businesses: deterministic variance, employee/weekend bonuses; host-authority fail-closed, slot-isolated idempotent payout marker, bounded catch-up; `biz` console dashboard.
- History: `Source/Mods/BusinessIncome/docs/CHANGELOG.md`.

**StackLimitMod v0.1.7 (2026-09-26, ingredient stacks):**
- Configurable stack limit (default 40) for agricultural + ingredient categories only; weapon/ammo/clothing/cash veto; dual discovery (definition scan + `Registry` hook) with a `BaseItemInstance.get_StackLimit` postfix; restore on unload; `apply_report.json` + `stack` console.
- v0.1.6 was the IL2CPP fix (`is` checks are useless on proxies — `TryCast<T>()`), proven via apply_report + the 2026-09-20 session. History: `Source/Mods/StackLimitMod/docs/CHANGELOG.md`.

**AutoPackagingStation v0.3.3 (2026-09-13):**
- 4×4 animated packaging line (2×2 footprint, E-interactable, Hustler-I gate): atomic 2-phase pack engine, weighted quality mixing, auto-unpack canvas mirror, native slot sync, PackUp with refund; all mutations host-guarded.
- **Verified 2026-09-20.** History: `Source/Mods/AutoPackagingStation/docs/CHANGELOG.md`.

**HitmanPhone v0.2.9 (2026-09-15):**
- Bounty contracts via phone messages (anonymous callers): kill → Polaroid evidence → dead-drop payout; police heat, journal quests, 3-day expiry, slot-isolated save; `KnockOut` patch + double-drop latch + orphan-quest cleanup.
- Beta verified 2026-09-24 (offer → accept → receipt → payout). History: `Source/Mods/HitmanPhone/docs/CHANGELOG.md`.

**MessagesPlus v0.4.1 (2026-09-29/30):**
- Patch-only mod (no PhoneApp icon) on the vanilla `MessagesApp`: sticky search band under the title (live search, `[All][Customer][Dealer][Supplier]` chips, unread counter), "⋯" menu with Clear Read / Clear All — view-only filtering, host-only + customer-only mutations, one-time legacy restore of v0.1.x trashed threads.
- **Whole-app dark mode** (`AppTheme`): **permanent since v0.4.1** — the "⋯" menu toggle is gone, the config defaults to ON and a stale `false` self-heals at startup (`MessagesPlusConfig.DarkMode` stays for schema stability). Recolours our band + vanilla surfaces (page backgrounds, inbox rows, chat bubbles + tails, dialogue header + response panel, generic near-white sweep) — one-time per graphic with cached originals; avatars, badges and the unread dot stay untouched.
- **v0.4.1 fix:** the deal-window popup themes **instantly** (`DealWindowSelectorPatch` — same-frame subtree refresh on `SetIsOpen`, forced re-tint for vanilla re-colours); previously freshly shown surfaces stayed light until the 1-2 tick. **In-Game-Verify open.**
- Round-1 fixes: `(RectTransform)x.transform` casts → `GetComponent<RectTransform>()` (IL2CPP cast made the "⋯" menu dead), search surface keeps `raycastTarget=true` (v0.4.0 **Verified 2026-09-30**). History: `Source/Mods/MessagesPlus/docs/CHANGELOG.md`.

**TaxiDriver v0.8.2 (2026-10-02, spike):**
- Orderable taxi from the in-game phone ("Taxi" app) with an own NPC driver: vehicle spawn + `VehicleAgent.Navigate` A→B + player ride, `taxi.glb` visual swap via S1MAPI GltfLoader, `RoadKeeper` road-corridor assistance, destination picker (properties/custom checkpoints), fare meter (moving in-game minutes; cash → bank; host-only fail-closed), F9–F12 diagnostic hotkeys + output-only `taxi` console.
- Stages 1–3 live-verified 2026-09-25. **Pakete C–G in-game test round open** (exit hardening, driver retention, spawn clearance, clear-selection, patrol NRE). History: `Source/Mods/TaxiDriver/docs/CHANGELOG.md`.

**TabToHome v0.1.0 (2026-10-04, new — shelved same day):**
- Harmony prefix on `Phone.SetIsOpen`: Tab with an app open fires the game's own `closeApps` event and skips the put-away, so the phone stays up on the HomeScreen (Escape-equivalent state); second Tab puts the phone away as usual. Any patch failure falls through to vanilla behavior.
- **Shelved 2026-10-04 (decision by Dominik):** in-game the phone still closed — S1API's own `Phone.SetIsOpen(false)` calls (3+ per Tab, alternating) run after the patch's skip, so the redirect cannot win. Game copy removed (not deployed); stock Tab behavior until upstream S1API is fixed. History: `Source/Mods/TabToHome/docs/CHANGELOG.md`.

**StorageScanner v0.2.0 (2026-10-04, new):**
- Read-only PhoneApp: totals per item across all storage containers of owned properties; `ALL` + per-property filter chips, live name search, manual + periodic refresh, explicit INCOMPLETE banner (culled containers reported, never silently counted). No Harmony, no background scanning.
- **v0.1.1 fix:** the readiness gate blocked every scan — `IsContentCulled` is true for any property the player is not near, so the app showed "Loading storage..." forever and zero scans ever ran. The gate now waits only for the owned-property list; culled containers are skipped and reported as incomplete. Full source restored to the deployed 0.1.0 behavior + fix. **In-Game-Verify open.** History: `Source/Mods/StorageScanner/docs/CHANGELOG.md`.

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

- **Game decompiles:** `GameReferences/` — regenerate with `pwsh Tools/bootstrap-game-references.ps1` (pinned ilspycmd 9.1.0.7988, project mode; regenerate after every game update). Current generation: 2026-10-07 against the freshly generated 0.4.7f11 proxies; `ProductIconManager` was inspected to confirm `GenerateIcons` is absent and `GenerateRuntimeIcons(string)` exists. For ground truth use `ilspycmd` against `<GameDir>\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll` (scoped `DOTNET_ROOT`).
- **S1API source:** `ThirdParty/S1API/` (submodule, pinned beta line — currently the beta.8 tag).
- **64 system analyses:** `Skills/schedule1-game-systems/references/`.
- **Archived mod sources:** `Source/Archive/`. The old `Knowledge/` folder from the pre-reinstall workspace no longer exists.
