# AGENTS.md

This repository is the **only active mod workspace**, outside the Schedule I installation.
Read this file completely before editing mods; load `Skills/schedule1-modding/SKILL.md` first,
then the matching specialty. The rules below apply on any machine; local provenance is dated.

- [Developer workflow](DEVELOPERS.md) · [Tools](Tools/README.md) · [Architecture](docs/architecture.md)
- [Current compatibility and evidence](docs/compatibility.md) · [Pitfalls](docs/pitfalls.md)
- [Skills index](Skills/README.md) · [Documentation index](docs/README.md)

**Hard rules**

- Launch through Steam: `Start-Process "steam://rungameid/3164500"`. Never run
  `Schedule I.exe` directly. **Close the game before building** and keep it closed through
  deployment; loaded DLLs are locked.
- Run PowerShell helpers explicitly as
  `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/<script>.ps1`; prefer forward-slash paths.
  Do not rely on the ambient shell or execution policy.
- Use the **.NET 8 SDK** for C# 12, even though mods target `net6.0`. On this device
  **do not set `DOTNET_ROOT`**: `%USERPROFILE%\.dotnet` contains tools, not a runtime, and
  breaks testhost (`hostfxr.dll` missing). Override locally only when `dotnet --list-sdks` is
  empty and that directory really contains an SDK/runtime.
- Versions change only through `Tools/bump-version.ps1 -Mod <Name> -Version x.y.z`.
  Never hand-edit versions or `S1Mods.sln`; use `Tools/gen-sln.ps1` for the solution.
- Preserve unrelated working changes. **No commit or push without Dominik's explicit go.**
  Work linearly on `main`; do not create feature branches.
- Verification builds use `-p:S1NoDeploy=true` or a helper's `-NoDeploy`. Plain builds
  auto-deploy. Install only when the task includes deployment.
- Build/load evidence does not establish gameplay acceptance. Preserve dates, versions,
  hashes, attribution and known limits when summarizing evidence. Correct outdated guidance.
- Unity `Library/` folders can contain build reports. For old WorldLook run caches use
  `Tools/clean-unity-caches.ps1` (preview first); preserve current caches, backups and evidence.

## 0. AI-Agent Skills (`Skills/`)

Load the primary skill, then only the specialty/references needed for the task (use the skill
loader when available, otherwise read the files). [Skills/README.md](Skills/README.md) indexes
all 23 skills and their references:

- Core: `schedule1-modding`, `schedule1-phoneapp`, `schedule1-troubleshooting`, `schedule1-knowledge`.
- Frameworks: `schedule1-s1api`, `schedule1-s1mapi`, `schedule1-game-systems`, `schedule1-mcp`.
- Domains: `schedule1-economy`, `schedule1-persistence`, `schedule1-items`, `schedule1-grid`,
  `schedule1-interiors`, `schedule1-world-expansion`, `schedule1-custom-npcs`, `schedule1-3d-assets`.
- Patterns: `schedule1-networked-world-state`, `schedule1-harmony-bootstrap`,
  `schedule1-il2cpp-reflection`, `schedule1-debounced-reload`, `schedule1-runtime-unity-cache`,
  `schedule1-lifecycle-verify`, `schedule1-mod-reversing`.

Keep this list, `Skills/README.md` and `README.md` aligned when adding a skill.

## 1. Setup

**Recorded local baseline:** Schedule I Open Beta **v0.4.7f12**, Unity **2022.3.62f2**,
Steam build **25796331** (log/manifest, 2026-10-08); MelonLoader **0.7.3**, S1API **3.2.1 stable**,
S1MAPI **2.0.1**. Mods use `net6.0`, C# 12 and nullable references. SDK **8.0.426** and
runtimes **6.0.36 / 8.0.32** are global in `C:\Program Files\dotnet` (2026-10-24).

- Game path: `SCHEDULE1_PATH`, otherwise
  `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`. Set the variable **before**
  MSBuild starts. Initialize submodules and run `Tools/setup-workspace.ps1` on a new clone;
  it writes machine-local dependency props.
- Required files: `Mods/S1API.Il2Cpp.MelonLoader.dll` (exact name),
  `UserLibs/S1MAPI_Il2Cpp.dll` and `MelonLoader/Il2CppAssemblies/`. The latter is generated
  by the game's first run; absent/wrong references break compilation.
- S1API is pinned at `a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b` / `v3.2.1`; superproject
  pointer and checkout matched on 2026-10-24. Installed IL2CPP `79d7af28…` and loader
  `811de671…` hashes matched local Release builds, ProductVersion `3.2.1+a4cbdd9e…`.
  The old log's beta.8 is historical. **Do not downgrade to 3.2.0**: pre-f6 NPC renames
  cause `MissingMethodException: NPCHealth.set_npc`. See [dependency pins](ThirdParty/README.md)
  and [full prior provenance](docs/history/agent-mod-history-20261024.md).
- Gameplay log: `MelonLoader/Latest.log`. S1MCP source: `ThirdParty/S1MCPServer-master/`;
  bridge `127.0.0.1:8765`. Use the MCP skill for client setup and live checks.

| Location | Purpose |
|---|---|
| `Source/Mods/` | Active mods, Shared, central build props/targets and `S1Mods.sln` |
| `Source/Tests/` | xUnit suites and LightControl console host; `Tools/test-all.ps1` runs both |
| `Source/Archive/` | Preserved inactive mods/tests; never included in the solution/build |
| `Skills/`, `docs/`, `Tools/` | Runbooks, evidence/reference documents, build/test/release helpers |
| `ThirdParty/`, `GameReferences/` | Pinned dependencies and local decompiles; see section 7 |
| `assets/`, `Release/`, `.githooks/` | Screenshot convention, ZIP output, opt-in Git hooks |

## 2. Mod Inventory

**No mod is currently under active development:** WorldLook was confirmed finished by Dominik
on 2026-10-25, referring to the last session. ✅/🆕 mean feature-complete;
🧪 means development, independently of gameplay verification. `Verified` is a recorded
real-session or attributed owner-confirmation claim, not an assertion that a test artifact
exists here. Owner acceptance without itemized actions does not prove each earlier checklist
item; accepted work stays closed.

**Multiplayer is out of scope by owner decision:** singleplayer first; retain host-authority
and MP-refusal guards, document them as unverified, and do not reopen MP acceptance.

These short rows support version tools. [Compatibility](docs/compatibility.md#verification-matrix)
owns current evidence limits and links to detailed records; per-mod changelogs retain history.
Keep exactly one inventory row per mod and supported bold acceptance labels.

| Mod | Status | Path in `Source/` | Path in `<Game>/Mods/` | Key bits |
|---|---|---|---|---|
| **NotesApp** | ✅ (v1.0.3, **Verified 2026-09-15**) | `Mods/NotesApp/` | `NotesApp.dll` + icon | Notes, SafeStorage, input focus |
| **PotScanner** | ✅ (v0.7.0, **Verified 2026-10-11**) | `Mods/PotScanner/` | `PotScanner.dll` | Grow dashboard, watering |
| **CalculatorApp** | ✅ (v0.2.3, **Verified 2026-09-15**) | `Mods/CalculatorApp/` | `CalculatorApp.dll` | Decimal calculator, history |
| **CustomSkateboard** | ✅ (v1.1.5, **Verified 2026-09-15**) | `Mods/CustomSkateboard/` | `CustomSkateboard.dll` + icon | Handling, avatar-safe visuals |
| **MoreSaveSlots** | ✅ (v1.0.12, **Verified 2026-09-15**) | `Mods/MoreSaveSlots/` | `MoreSaveSlots.dll` | MelonMod/Harmony; S1API EventHelper |
| **PocketShop** | ✅ (v0.3.9, **Verified 2026-10-24**) | `Mods/PocketShop/` | `PocketShop.dll` | Owner-confirmed cast fix; suppliers hidden |
| **BankApp** | ✅ (v0.5.0, **Verified 2026-10-11**) | `Mods/BankApp/` | `BankApp.dll` + icon | Banking, slot isolation |
| **Weather** | ✅ (v0.4.0, **Verified 2026-10-11**) | `Mods/Weather/` | `Weather.dll` + icon | Read-only dashboard |
| **BusinessIncome** | ✅ (v0.1.6, **Verified 2026-09-19**) | `Mods/BusinessIncome/` | `BusinessIncome.dll` | Host-only income, slot isolation |
| **StackLimitMod** | ✅ (v0.1.7, **Verified, owner-confirmed 2026-10-15**) | `Mods/StackLimitMod/` | `StackLimitMod.dll` | Agriculture only; original test date not supplied |
| **Minimap** | ✅ (v2.3.6, **Verified, owner-confirmed 2026-10-25**) | `Mods/Minimap/` | `Minimap.dll` | Clock AM/PM stays on one line; search cancelled, dormant core retained |
| **AutoPackagingStation** | ✅ (v0.4.7, **Verified 2026-10-15, owner-confirmed tested session**) | `Mods/AutoPackagingStation/` | `AutoPackagingStation.dll` | Buildable; legacy migration unverified |
| **HitmanPhone** | ✅ (v0.3.2, **Verified, owner-confirmed 2026-10-25**) | `Mods/HitmanPhone/` | `HitmanPhone.dll` | Singleplayer only; offers use own customers; tracker app removed |
| **MessagesPlus** | ✅ (v0.4.6, **Verified, owner-confirmed 2026-10-15**) | `Mods/MessagesPlus/` | `MessagesPlus.dll` | Popup/response repair; original test date not supplied |
| **ProductAppQoL** | ✅ (v0.1.0, **Verified 2026-10-10**) | `Mods/ProductAppQoL/` | `ProductAppQoL.dll` | Mouse/touch asking-price buttons |
| **TaxiDriver** | ✅ (v1.0.0, **Verified, owner-confirmed 2026-10-24**) | `Mods/TaxiDriver/` | `TaxiDriver.dll` + GLB folder | Version-only 0.8.22 milestone; known road/jitter issues |
| **StorageScanner** | 🆕 (v0.2.0, **Verified 2026-10-11**) | `Mods/StorageScanner/` | `StorageScanner.dll` + icon | Read-only storage overview |
| **S1MCP** | ✅ (v1.0.5, **Verified, owner-confirmed 2026-10-24**) | `ThirdParty/S1MCPServer-master/` | `S1MCPServer-IL2CPP.dll` | HHMM fix; read/no-op write observed, not every write form |
| **LightControl** | ✅ (v0.1.1; 0.1.0 **Verified, owner-confirmed 2026-10-24**) | `Mods/LightControl/` | `LightControl.dll` | 0.1.1 deployed log-only Rainbow fix; prior acceptance not itemized |
| **PropertyLEDs** | ✅ (v0.1.10, **Verified, owner-confirmed 2026-10-25**) | `Mods/PropertyLEDs/` | `PropertyLEDs.dll` + icon | Landscape phone, tighter insets; prior 0.1.8 actions not itemized |
| **PhoneScrollFix** | ✅ (v2.0.0, hover look **Verified 2026-10-11**) | `Mods/PhoneScrollFix/` | `PhoneScrollFix.dll` | Clipped home-icon hover |
| **WorldLook** | ✅ (v0.3.0-dev.5, **Verified, owner-confirmed 2026-10-25**) | `Mods/WorldLook/` | `WorldLook.dll` + embedded bundle | Owner-confirmed finished in the last session; actions not itemized; prior shader-reload evidence retained |
| **Shared** | ✅ workspace library | `Mods/Shared/` | `Shared.dll` | Reusable helpers; section 5 |
| **S1API** | ✅ 3.2.1 stable | — | `S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.MelonLoader.dll` | Pinned dependency; section 1 |

Archived: BackpackMod, DayCounter, HomelessMod, ProfitTracker, SnackVendor, TVBrowser and
_DiagPerfCounter plus archived tests. Retired dependencies and their old versions remain in
[the dated history](docs/history/agent-mod-history-20261024.md); they are not active work.

<a id="worldlook-continuation--installed-standalone-candidate-2026-10-22"></a>

### WorldLook completion and detailed history

**WorldLook is finished and owner-accepted** (Dominik, 2026-10-25, referring to the last
session). Version stays `0.3.0-dev.5`; no version-only release is implied. The confirmation
itemized no further actions and supplied no frame-time measurements and no separate
visible-warmth verdict. That is an evidence limit, **not an open backlog**: do not propose
warmth or performance acceptance work unless the owner explicitly reopens it.

Project path: `Source/Mods/WorldLook/`. Preserved evidence and history:

- [Water-surface record](Source/Mods/WorldLook/docs/water-surface-verification.md): the accepted
  dev.2 session, DLL `410fecf7…`, bundle `29557b87…`. **Water and ON/OFF are owner-accepted and
  closed (2026-10-24)**; do not reopen them from older checklists. Positive numeric warmth at 17:42.
- dev.3 (DLL `cc9a206a…`, installed 2026-10-24) removes F9/debug chatter only; shader and recipe
  unchanged. Its 2026-10-25 session measured repeatable Sun tint near 18:00, and Dominik accepted
  normal night appearance. ON lighting survived a same-process Dominik menu/reload, but the water
  renderer fell back to its native shader — a historical reload defect, repaired in dev.5.
- dev.4 (`5522f394…`, installed 2026-10-25): explicit session-only MCP control. Automated OFF/ON
  menu roundtrips preserved requested state and measured lighting (seven screenshots); it
  reproduced the reference-water fallback. [Automation contract](Source/Mods/WorldLook/docs/mcp-automation.md).
- dev.5 (`3f46ae88…`, installed 2026-10-25): shader-cache lifetime repair, bundle/recipe unchanged.
  Four automated Dominik reloads (two ON, two OFF followed by ON) passed actual shader/support
  readbacks; ten scene/menu screenshots are retained. Profile and all 44 native-save files were
  unchanged. [Current acceptance record](Source/Mods/WorldLook/docs/in-game-acceptance.md).

Expected look: mild light peaks at 06:00/18:00; zero strength from 07:30 through 16:30.
[Prior narrative](docs/history/agent-mod-history-20261024.md) is historical, not a current backlog.

## 3. Build & Deploy

Use [DEVELOPERS.md](DEVELOPERS.md#build) for the full workflow and [Tools/README.md](Tools/README.md)
for helper parameters. A compile-only solution build is:

```pwsh
dotnet build Source/Mods/S1Mods.sln -c Release -p:S1NoDeploy=true
```

- Plain builds deploy DLL/PNG/bundle files into `Mods/`; GLB files go into `Mods/<Mod>/`.
  Metadata/PDB go into `UserData/<Mod>/`, **never `Mods/`**. Do not put reference-only
  assemblies in `Mods/` (`BadImageFormatException`).
- `Tools/deploy-mod.ps1` checks planned hashes, leaves identical files unchanged, and backs up
  changed targets with bounded in-process rollback. Receipts/limits are in the Tools guide.
  `S1NoDeploy=true` skips every deployment copy.
- Use `Tools/new-mod.ps1` to scaffold, `Tools/gen-sln.ps1` after project changes (deterministic
  GUIDs; commit the regenerated SLN), `Tools/bump-version.ps1` for versions and
  `Tools/package-release.ps1` for ZIPs. Packaging needs game references and does not deploy;
  Shared and archived _DiagPerfCounter are not standalone release selections.
- A new laptop sandbox uses `Tools/new-laptop-workspace.ps1`, not a GitHub fork; it renames
  `origin` to `upstream`. Sync with `git fetch upstream` then `git merge upstream/main`.
- Native saves are PC/user-local at `%USERPROFILE%\AppData\LocalLow\TVGS\Schedule I\Saves`,
  never synced through Git. Game `UserData/` contains loader preferences, mod configuration,
  deployed metadata/PDB and slot sidecars (for example HitmanPhone JSON plus `.bak`), **not
  native saves**.

## 4. IL2CPP vs Mono (alternate branch) — Architecture Decision

Stay on **IL2CPP**, the default Steam release. Do not require the `alternate` Mono branch.
Known bridge/UI/lifecycle constraints have established solutions in the mods and skills.

## 5. Conventions & Shared Utilities

### New-mod source-file budget (external review)

- Target **15–20 hand-maintained production files including the csproj** so a new mod fits
  the owner's 20-attachment review limit; smaller mods need fewer files.
- Exclude tests, assets, docs/metadata, shared build files, dependencies and generated outputs.
  Explain inherited build/dependency context; an extra attached overview consumes a slot.
- Group related types by domain. Do not hide code, add assemblies, delete tests or create
  arbitrary giant files to meet the budget. Check at scaffold and when adding source files.
- Before exceeding 20, explain the architectural need and agree an exception or complete
  generated upload bundle. **Do not consolidate existing mods without explicit authorization.**

### Responsive UI (Method 3: `UITheme`)

Initialize in `OnCreatedUI(GameObject container)` before sizing:
`UITheme.InitializeForTextApp(containerRt)` (750f, 0.85–2.0),
`InitializeForDashboard(containerRt)` (900f, 0.75–1.20), or
`Initialize(containerRt, refHeight, minScale, maxScale)`.
Use `Sp` for fonts, `Dp` for spacing, and shared `GamePalette`/`UISprites`.
The high-DPI, often rotated phone canvas makes fixed sizes unreliable.

### IL2CPP pitfalls (checklist; full list in `docs/pitfalls.md`)

- Registered MonoBehaviours need `public X(IntPtr ptr) : base(ptr) { }`.
- Wire buttons with `S1API.Utils.EventHelper.AddListener` / `ButtonUtils.AddListener`,
  not raw `onClick.AddListener(new UnityAction(...))`. Prefer Remove-before-Add on rebuild;
  EventHelper deduplicates by delegate instance.
- Proxy transforms require `GetComponent<RectTransform>()` / `TryCast<T>()`;
  direct casts throw and `is` is not a reliable IL2CPP type check.
- TextMeshPro lives in `Il2CppTMPro`. Multiline InputFields need text/placeholder
  `UpperLeft` alignment, `MultiLineNewline` and clean offsets.
- Refresh static world lists at `OnLoadComplete`; they may be empty at scene load.
  `OnSaveInfoLoaded` fired zero times on the recorded f6+ runtime (2026-09-29);
  see `schedule1-lifecycle-verify`.
- Never use `MelonPreferences_Entry<object>`: Tomlet can deserialize values as
  `KeyCode` and crash on quit. Use typed `ModConfig<T>`.
- `UIFactory.Text` defaults to centered anchors; set anchors/offsets explicitly for custom rows.

### Shared utilities (`S1Mods.Shared`, `Source/Mods/Shared/`)

Reusable code belongs in `Source/Mods/Shared/src/`. **Mods must not reference one another**
or copy shared helpers; follow [dependency boundaries](docs/architecture.md).
Use `PatchGuard` for tolerant patching; `Diagnostics` for once-only errors/startup reports;
`SafeStorage` and `SaveSlots` for atomic slot persistence; `NetworkGuard` / `SceneGate`
for authority/lifecycle gates; typed `ModConfig` for preferences. Other helpers include
`SafeInvoker`, `GameObjectResolver`, `HotkeyManager`, `ModLogger`, `TypeResolver`,
`AudioHelper`, `EconomyHelper` and `ShopListingSync`. UI helpers are listed above;
inspect actual contracts before use.

## 6. Workflows

### Quality gates

Run `Tools/verify.ps1` with `-Mode Quick`, `Pure`, `Full` or `Auto`.
All modes disable deployment and retain reports; [the Tools guide](Tools/README.md#repository-checks-ci-pre-commit-and-pre-push)
defines the checks:

- **Quick:** format, solution determinism, versions, documentation paths, skills and tracked MSBuild XML.
- **Pure:** Quick + game-free tests (including LightControl console) + tool suites.
- **Full:** Quick + Release build + all tests/tool suites; requires game references and a closed game.
- **Auto:** Full with game references, otherwise Pure. CI/pre-push use Auto; pre-commit uses Quick.
  Hooks are opt-in (`git config core.hooksPath .githooks`); CI initializes submodules recursively.
  Release is manual `workflow_dispatch` and requires a configured runner with game assemblies.

For changes to docs, tests, `.gitignore`, `.github/` or `ThirdParty/`, also verify an isolated
checkout of the current working files (including new files and pinned clean submodules):

```pwsh
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/verify-clean-checkout.ps1 -Mode Pure
```

Local success alone is insufficient: missing submodule sources and generated files previously
hid checkout-dependent failures. This check does not commit, change the source index or prove
remote CI. See [snapshot limits](Tools/README.md#verify-uncommitted-work-in-isolation).

### Mod update cycle

1. Load skills, inspect implementation and preserve unrelated changes.
2. Edit `Source/Mods/<Name>/src/`; use the version tool when changing a mod version.
3. Verify without deployment. Use separate coder/verifier subagents for implementation/audit.
4. When installation is authorized, deploy with the game closed, launch through Steam, then
   inspect `MelonLoader/Latest.log` and the affected gameplay path.
5. Record actual evidence in the mod CHANGELOG and compatibility matrix. Update affected
   skills and this file when inventory, versions or dependencies change; commit/push only on request.

### Pre-flight with S1Interop (advisory, local only)

Installed 2026-10-25 as `S1Interop 0.1.0-alpha.1` (install with
`dotnet tool install --global S1Interop --version 0.1.0-alpha.1`), then run
`s1interop analyze "Source/Mods/<Name>/src/<Name>.csproj"`. It always exits 0: read the report,
never treat exit status as a gate. Known false positives: centrally defined TFM/LangVersion
(reports `tfm=<unknown>`), mod-internal collections and guarded reflection.

### Maker-checker workflow

Load constraints first, implement against the architectural boundaries with a separate verifier,
then update affected runbooks/CHANGELOG/inventory when their facts change.

## 7. Reference Material

- **Live assemblies are API truth.** `GameReferences/` decompiles are local/generated and can
  be stale. The recorded f12 regeneration (2026-10-08) produced 2,234 files; regenerate after
  game updates with `Tools/bootstrap-game-references.ps1` (ilspycmd `9.1.0.7988`, project mode).
  For current signatures inspect `<GameDir>/MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll`
  with ilspycmd and only a valid scoped `DOTNET_ROOT` if needed. TaxiDriver uses live f12 references.
- **Framework source:** `ThirdParty/S1API/` at the pin in section 1.
- **System analyses:** `Skills/schedule1-game-systems/references/` (64 systems).
- **Archives:** `Source/Archive/`. The old `Knowledge/` directory no longer exists.
