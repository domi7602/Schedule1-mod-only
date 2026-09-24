# AGENTS.md

Workspace: `C:\Users\pc\Schedule1-mod-only` — MelonLoader modding workspace for *Schedule I* v0.4.6f13 (TVGS). The repo lives **outside** the game install dir (under the user profile); build/deploy resolves the game path via the `SCHEDULE1_PATH` env var or the default fallback `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`.

Quick links: [`docs/architecture.md`](docs/architecture.md) defines dependency boundaries, [`ThirdParty/README.md`](ThirdParty/README.md) documents pinned dependencies, and [`GameReferences/README.md`](GameReferences/README.md) explains local decompile generation. This file remains the compact operational index and version inventory.

> **Path history (3 moves):**
> - **2026-09 setup restoration (current state):** Repo at `C:\Users\pc\Schedule1-mod-only`. .NET SDK 8.0.424 installed, S1API 3.2.0 (fork build from `ThirdParty/S1API/`) deployed, NotesApp + Shared built as verification. Deploy convention since 2026-09: DLL/PNG/bundle → `Mods\`; `mod.json` + `.pdb` → `UserData\<Mod>\`.
> - **Previous state (before restoration):** Repo was in the game folder under `<GameDir>\Schedule1-mod-only-main`. Obsolete due to the new installation.
> - **Oldest state:** `C:\Users\pc\Desktop\Schedule1-mod-only`. Already obsolete when moving to the game dir.

> **For AI Agents:** This is the bootstrap file. Read it completely before doing anything to the mods. If you make assumptions not stated here, ask first. If you find something here that is *wrong* or outdated, please update it.

---

## 0. AI-Agent Skills (`Skills/`)

Twenty skills orchestrate mod work (located directly in `Skills/<skill-name>/SKILL.md`). **Load them via the `skill` tool** when the task matches:

| Skill | When to use |
|---|---|
| **`schedule1-modding`** | Mod runbook: scaffold, build, deploy, architecture patterns, S1API/UI/Harmony. **Primary skill, always first.** |
| **`schedule1-phoneapp`** | PhoneApp runbook: S1API PhoneApp development, Method 3 responsive UI, input focus protection, lifecycle stability (Rule 10/11), WasCollected guards. |
| **`schedule1-grid`** | Grid & building: outdoor/unrestricted placement, BuildUpdate_Grid patching, ghost positioning, 7 Golden Rules, custom building. |
| **`schedule1-s1api`** | S1API framework reference: Saveables, PhoneApp base, Quests, NPCs, Items, Money, GameTime, Lifecycle, cross-branch compatibility. |
| **`schedule1-s1mapi`** | S1MAPI framework reference: ProceduralMesh, BuildingBuilder, GltfLoader, InteriorBuilder, World tools (Terrain/Nav/Prefab). |
| `schedule1-knowledge` | Research: locally generated decompiles (`GameReferences/`), S1API source (`ThirdParty/S1API/`), 64 curated systems (`Skills/schedule1-game-systems/references/`). |
| `schedule1-troubleshooting` | Diagnostics: native PowerShell `Latest.log` triage, crash patterns, save-load timing, IL2CPP pitfalls, WasCollected, slot_-1.json recovery. |
| `schedule1-game-systems` | Game systems: 64 systems (Growing 08, Inventory 09, Property 54, etc.), decision tree, recipes — bypass raw decompile. |
| **`schedule1-economy`** | Economy: Money (cash/bank), Business revenue, Shop multi-payment (Cash/Bank/Auto), Customers, Laundering — host authority + snapshot revert. |
| **`schedule1-persistence`** | Persistence: SafeStorage atomic .bak, slot_{n}.json (triple-guard + slot >= 0), GameLifecycle timing, ModConfig TOML sidecar, BuildableItem restore rule. |
| **`schedule1-items`** | Items: BaseItemDefinition/Registry/StackLimit (dual-scan + not patchable accessor), Inventory slots (CashSlot 1k), Buildable injection. |
| **`schedule1-interiors`** | Interiors & Minigames: Door hooking (StaticDoor/NpcSummonMenu), Procedural room shells (binary/layout mesh reader), In-world screens (Texture2D.SetPixels32), 3D spatial ambience. |
| **`schedule1-3d-assets`** | 3D Assets & Blender: Blender pipeline (Z-Up to Y-Up, Apply Transforms, Recalculate Normals), URP shader resolution (Lit/Unlit pink shader fix), PBR materials, bone rigging (Spine2/Head/Hands), zero-collider rule. |
| **`schedule1-mcp`** | S1MCP & Live Debugging: Live game introspection, TCP :8765 bridge, in-game log capturing, player/NPC state inspection, object reflection, item spawning. |
| **`schedule1-custom-npcs`** | Custom NPCs: NPCPrefabBuilder, appearances, dialogue node graphs, daily schedules, and custom clothing. |
| **`schedule1-debounced-reload`** | Debounced Live-Reload: FileSystemWatcher debouncer (150–250ms), main-thread pump via OnUpdate, config hot-reloading. |
| **`schedule1-harmony-bootstrap`** | Harmony Bootstrap: Assembly-wide patch discovery, PatchTargetGuard pre-flight checks, applied/skipped/failed counters, clean unpatching. |
| **`schedule1-il2cpp-reflection`** | IL2CPP Runtime Reflection: Array bridging (Il2CppStructArray vs T[]), missing overloads, namespace fallback, dynamic member access. |
| **`schedule1-lifecycle-verify`** | Lifecycle Verification: ILSpycmd runbook for verifying S1API and native lifecycle event ordering against game assemblies. |
| **`schedule1-runtime-unity-cache`** | Runtime Unity Cache: Memory leak prevention for runtime Texture2D, Sprite, AudioClip, and Material objects. |

Skill paths: `Skills/<skill-name>/SKILL.md` (plus `references/` sub-files). Index: `Skills/README.md`. Standard content: SKILL.md (YAML frontmatter + decision tree + references), `references/*.md` for sub-topics.

---

## 1. Setup

- **Game:** `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` (override via `$env:SCHEDULE1_PATH`)
- **Game Version:** v0.4.7f6 (Open Beta, Unity 2022.3, IL2CPP — `GameReferences/` decompiles are still 0.4.6f13 and may drift; verify against the live Il2CppAssemblies)
- **Mod Loader:** MelonLoader 0.7.3 (net6)
- **TFM:** `net6.0`, `LangVersion` 12, `Nullable` enabled
- **Layout:**
  ```
  Source/Mods/        Mods + Shared lib (incl. Shared/UITheme) + Directory.Build.props/targets + S1Mods.sln
  Source/Archive/     Archived mods (DayCounter, ProfitTracker, TVBrowser, BackpackMod, Minimap)
  Source/Tests/       Unit tests (Shared.Tests, AutoPackagingStation.Tests, CalculatorApp.Tests)
  GameReferences/     Locally generated decompiles (Assembly-CSharp, firstpass)
  Skills/             20 AI-Agent Skills & References (modding, phoneapp, economy, systems, etc.)
  ThirdParty/         Pinned external frameworks & reference sources; see ThirdParty/README.md
  Tools/              (reactivated 2026-09-10, 11 scripts: build-all, gen-sln, new-mod, bump-version, package-release, backup-to-d, check-version-sync, check-doc-paths, deploy-thirdparty, bootstrap-game-references, mods-cleanup-inventory)
  Release/            Release packages (.gitkeep)
  .githooks/          Pre-commit hook (dotnet format + gen-sln determinism)
  docs/               Architecture, Release process & IL2CPP pitfalls (docs/pitfalls.md)
  AGENTS.md           Agent bootstrap & inventory
  CONTRIBUTING.md     Contributor guide
  DEVELOPERS.md       Developer documentation
  LICENSE             MIT (workspace) + Third-Party notices
  ```

---

## 2. Mod Inventory (Status 2026-09-17 — Deploy-Status see Note)

> **Status convention (2026-09-16):** `Verified <date>` = functional verification in a real-game session, traceable via the respective fix/audit commit and CHANGELOG entry (assertion, **no** artifact in the repo). `In-Game-Verify open` = code complete, but not yet confirmed in-game. Both states are intentionally kept separate.

> **Verified (real-game session 2026-09-20, 13:42):** `StackLimitMod v0.1.6` ✅ (User: weed=40, packaged products stack; apply_report proves ogkush/sourdiesel/meth/cocaine → modified, cash/mushroomhat → weapon guard, 42 items), `PocketShop v0.3.2` ✅ (Payment rules + cash HUD feedback; User: "done, nothing unusual"), `AutoPackagingStation v0.3.3` ✅ (E-prompt, Unpackage-mode mirror, 10-unit unpack batching, alignment visual hide; User: "functional without errors"). Evidence: apply_report.json 2026-09-20T11:42:59Z, Latest.log 13:42:59 (Applied to 42 items), User session feedback 2026-09-20.

> **Status after reinstallation (2026-09-04):** Setup restored — .NET SDK 8.0.424 installed, S1API 3.2.0 from `ThirdParty/S1API/` built and deployed (`Mods\S1API.Il2Cpp.MelonLoader.dll` + `Plugins\S1APILoader.dll`), NotesApp + Shared built as verification. The remaining mods have **not yet** been redeployed (state 2026-09-04) — just run `dotnet build` for each mod. S1API rebuild: `local.build.props` (copy of `example.build.props`) is ready; the multi-TFM loader build throws a harmless MSB3030 copy error during the net6.0 pass — the correct netstandard2.1 DLL is still deployed. **Deploy convention since 2026-09:** DLL + icons/bundles → `Mods\`; `mod.json` + `<Mod>.pdb` → `UserData\<Mod>\` — json/pdb **never** belong in `Mods\`.

| Mod              | Status          | Path in `Source/`      | Path in `<Game>\Mods\`              | S1API? |
|------------------|-----------------|------------------------|-------------------------------------|--------|
| **NotesApp**     | ✅ active (v1.0.3, **Verified 2026-09-15**) | `Mods/NotesApp/` | `NotesApp.dll` + Icon | yes (PhoneApp + UIFactory + InputFocus hook + SafeStorage) |
| **PotScanner**   | ✅ active (v0.5.4, **Verified 2026-09-15**) | `Mods/PotScanner/` | `PotScanner.dll` | yes (PhoneApp + Property + Growing + Money + Lifecycle APIs + ModConfig + Console/BaseConsoleCommand) |
| **CalculatorApp**| ✅ active (v0.2.3, **Verified 2026-09-15**) | `Mods/CalculatorApp/` | `CalculatorApp.dll` | yes (PhoneApp + Money + UIFactory + InputFocus hook + SafeStorage) |
| **CustomSkateboard**| ✅ active (v1.1.5, **Verified 2026-09-15**) | `Mods/CustomSkateboard/` | `CustomSkateboard.dll` + Icon | yes (Skating/Skateboard + Ultra Carving + Instant Jump + Anti-Gravel + Jeff Dialogue + Nexus Ready) |
| **MoreSaveSlots**| ✅ active (v1.0.12, **Verified 2026-09-15**) | `Mods/MoreSaveSlots/` | `MoreSaveSlots.dll` | no (MelonMod + Harmony) |
| **DayCounter**      | ⏸ archived (v1.0.0) | `Source/Archive/DayCounter/` | — (removed from MelonLoader) | no (MelonMod + uGUI Screen HUD + TextMeshPro + S1API/Hash Console + ModConfig) |
| **PocketShop** | ✅ active (v0.3.2, **Level-Lock & Inline-Qty 2026-09-17 / Vanilla-Shop-PaymentType + Cash-HUD-Feedback 2026-09-20 (Verified 2026-09-20), v0.4.6f13**) | `Mods/PocketShop/` | `PocketShop.dll` | yes (PhoneApp + Vanilla-PaymentType per shop (Cash/Card) + Cash-HUD-Popup (visualizeChange) + ItemDetailModal + Inline-Quantity-Input + Level-Lock + SFX + Console `pshop shops`) |
| **BankApp**    | ✅ active (v0.4.4, **Verified 2026-09-15**) | `Mods/BankApp/`    | `BankApp.dll` + Icon | yes (PhoneApp + Chip-Based Single-Screen UI + Weekly Limit Progress + Double-Entry Booking + Slot-Awareness + Save-Slot Isolation) |
| **Weather**    | ✅ active (v0.1.0, In-Game-Verify open) | `Mods/Weather/` | `Weather.dll` + Icon | yes (PhoneApp + S1API Weather API (`WeatherManager.Current` / `OnWeatherChanged`) + UIFactory + UITheme; no persistence, no gameplay influence) |
| **HomelessMod** | ⏸ archived (v0.1.12) | `Source/Archive/HomelessMod/` | — (removed from MelonLoader) | yes (Street Nomad + Everywhere Building + 3D Procedural Sleeping Bag + Quests + Console + Save-Slot Isolation; archived 2026-09-17 at user's request) |
| **BusinessIncome** | ✅ active (v0.1.6, **0-Business Backlog-Fix 2026-09-17 / v0.4.6f13, Verified 2026-09-19**) | `Mods/BusinessIncome/` | `BusinessIncome.dll` | yes (Daily Passive Revenue + Multiplayer Host Authority + Slot Idempotency + Deterministic Variance + Console Dashboard) |
| **Minimap**        | ⏸ archived (v2.0.2) | `Source/Archive/Minimap/` | — (removed from MelonLoader) | yes (Minimap & Unified HUD + Dual-Shape Circle/Square + Pooled Blips + DayCounter Merged + Drag-and-Drop + **v2: Dealer-Marker + Heat-Ring (EPursuitLevel) + Waypoints (slot-isolated, Console `minimap wp`) + Health-Bar + Minimap-Settings-PhoneApp (Toggles + Hex-Colors)**; archived 2026-09-17; user prefers game without Minimap) |
| **ProfitTracker**   | ⏸ archived (v1.1.0) | `Source/Archive/ProfitTracker/` | — (removed from MelonLoader) | no (MelonMod + uGUI Screen HUD + TextMeshPro + S1API/Hash Console + SafeStorage) |
| **TVBrowser**       | ⏸ archived (v0.1.0) | `Source/Archive/TVBrowser/`   | — (removed from MelonLoader) | yes (TVApp + BrowserNavBar + InputFocus + UwbBridge + ModConfig) |
| **MikuPlayerModel** | ⏸ disabled (discontinued) | — (source not in repo; deleted before initial commit) | `MikuPlayerModel.dll.bak` | yes (Player Avatar Injection + Verlet Hair Spring Physics + Console) |
| **S1MCP**        | ✅ active (v1.0.1, IL2CPP net6, freeze fix, debug-logging opt-in, **verified 2026-08-22**) | `ThirdParty/S1MCPServer-master/` | `S1MCPServer-IL2CPP.dll` | yes (MCP Protocol / TCP Server :8765 / Live Game State / Log Inspection) |
| Construction Site | ⏸ disabled     | —                      | `ConstructionSiteProperty.dll.bak`   | no   |
| **MoreDrugs 1.0.2** | ❌ removed (2026-09-19, **not redeployed after reinstallation** — 3rd-party by ifBars, source not in repo) | — (only deployed DLL, not part of this repo) | — (DLL not restored, user decision: stays away) | yes (was: S1API 3.1.7+ Save-Provider APIs) |
| **StackLimitMod** | ✅ active (v0.1.6, **Agriculture-Only & Weapon-Shield 2026-09-15 (Verified 2026-09-15) / IL2CPP-TryCast-Fix 2026-09-19 (Verified 2026-09-20: 42 items, drugs modified, cash/clothing protected), v0.4.6f13**) | `Mods/StackLimitMod/` | `StackLimitMod.dll` | yes (BaseItemDefinition + Registry + BaseItemInstance Harmony Patches + Console + SafeStorage) |
| **BackpackMod**   | ⏸ archived (v1.2.3) | `Source/Archive/BackpackMod/` | — (removed from MelonLoader) | yes (3D Wearable Backpacks + Spine Rig Alignment + Realistic Harness & Straps + ObjLoader + Storage + Mannequin 360 Rotation + **B1 Sort: Button-only sorting (backpack/inventory/container) with stack-merge by ID+quality+packaging, sort button in StorageMenu + "Sort Inventory" in GameplayMenu, atomic plan-then-commit (v1.2.0: GetCopy instead of GetDefaultInstance — quality/packaging preserved; clipboard-slot reference filter; overflow sidecar on full inventory; OnPreLoad cross-save protect; v1.2.1: ObjLoader 50MB/250k vertex cap, ShopDump #if DEBUG; v1.2.2: HUD sort button raycast protection against grid click swallowing; v1.2.3: StorageMenu sort overlay fix (smallest button template + enforced geometry via SortButtonLayout + BackpackMod.Tests)**) |
| **AutoPackagingStation** | ✅ active (v0.3.3, **Bugfix Round 7 2026-09-19 / v0.4.6f13; E-Prompt + Auto-UNPACK + Unpack-Batching + Alignment-Visual-Hide 2026-09-20 (Verified 2026-09-20 through user session: "functional without errors")**) | `Mods/AutoPackagingStation/` | `AutoPackagingStation.dll` | yes (4x4 Industrial Packaging Line + UV Scroll Conveyor + Native Slot Sync + Auto-Unpack via Canvas-Mode-Mirror + GetState-Gate + Batched Unpack + Alignment-Visual-Hide + SafeStorage + Shop Injection + 2x2 Footprint + InteractableObject + Hustler-I Rank-Gate) |
| **HitmanPhone** | ✅ active (v0.2.9, **0.4.7f6 NPCDeathPatch + DEBUG offer host fallback, Beta verified 2026-09-24: offer→accept→receipt→payout** | `Mods/HitmanPhone/` | `HitmanPhone.dll` | yes (MessagesApp Contracts + NPC + Items + Quests + SafeStorage) |
| **MessagesPlus** | ✅ active (v0.1.0, In-Game-Verify open) | `Mods/MessagesPlus/` | `MessagesPlus.dll` | no (MelonMod + Harmony patches on vanilla MessagesApp + SafeStorage) |
| **SnackVendor** | ⏸ archived (v0.0.9) | `Source/Archive/SnackVendor/` | — (removed from MelonLoader) | yes (S1API Buildable + Vanilla-VendingMachine-clone + marker-guarded Harmony + Sidecar-persistence + S1MAPI.GltfLoader-mesh + NPC-inventory-credit + Deposit/Extract-panel; archived 2026-09-17 at user's request, in-game-verify never performed) |
| **_DiagPerfCounter** | ⏸ archived (v0.3.2, **archived 2026-09-20 at user's request — obsolete: reflection dump makes ilspycmd/S1MCP better; release build was a no-op anyway (`#if !DEBUG`)**) | `Source/Archive/_DiagPerfCounter/` | `_DiagPerfCounter.dll.bak` (does not load) | no (MelonMod + reflection dump via SafeStorage path) |
| **PhoneScroll** | ❌ removed (v1.4, **retired 2026-09-16**, DLL in `UserData\_RemovedMods\`) | `ThirdParty/Archive/PhoneScroll/` | — | no (vanilla Phone-HomeScreen-Hook, 3rd-party by V4LEXL) |
| **Shared**       | ✅ active (workspace lib, **Verified 2026-09-15**) | `Mods/Shared/` | `Shared.dll` | no (PatchGuard, SafeStorage, GameObjectResolver, SafeInvoker, HotkeyManager, ModConfig, ModLogger, NetworkGuard, SceneGate, TypeResolver, UITheme) |
| S1API 3.2.1-beta.5 | ✅ active      | —                      | `S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.MelonLoader.dll` | (itself) |

### Frameworks & Tooling (not self-built, but deployed in the workspace)

| Framework       | Status     | Source                                                                  | Deploy Path (Runtime)                  | Notes |
|-----------------|------------|-------------------------------------------------------------------------|----------------------------------------|-------|
| **Sideload 1.7.0** | ❌ removed 2026-09-10 (deprecated, unused) | — | — | DooDesch HTML/CSS/JS UI framework; no mod in the workspace had ever bound it. |
| **hash 1.0.5**  | 📦 locally present (undeployed, deprecated 2026-09-10) | `ThirdParty/Archive/ScheduleOne-Hash/` (ZIP + `Hash_extracted/`)              | `Hash.dll` → `<Game>\Mods\`            | DooDesch terminal replacement for the dev console. No longer used — stays in the repo as reference (2026-09-16 moved to `Archive/`; PotScanner/BusinessIncome source includes follow the new path). |
| **Herer's Minimap 2.0.1** | ❌ removed (2026-09-17) | NexusMods #899 by JackHerer1820 | — (DLL in `UserData\_RemovedMods\`) | JackHerer1820 Minimap with integrated MinimApp on the in-game phone. Removed on request (user prefers to play without Minimap). |
| S1API 3.2.1-beta.5 | ✅ active (prebuilt beta zip, deployed 2026-09-23; source submodule still v3.2.0+4) | `ThirdParty/S1API/` (Source + `local.build.props`)               | `S1API.Il2Cpp.MelonLoader.dll` + `Plugins\S1APILoader.dll` | (itself — see §4) |

---

### Mod Details (current status)

**NotesApp v1.0.3 (2026-09-13, Bug Report Round 6):**
- **v1.0.3 (Bug Report Round 6 2026-09-13):** N1 - JSON `Title`/`Text`: `null` is normalized to `string.Empty` on load (fixed RefreshList crash on hand-edited/corrupted `notes` files).
- **v1.0.2:** RefreshList Null/WasCollected guard; Slot-Suffix >=0 check (no slot_-1 file).
- **v1.0.1:** Static event dispatcher (no subscriber leak per scene load); dead `MigrateLegacyGlobalNotes()` removed.
- **SafeStorage Persistence:** Protects `notes.json` via `S1Mods.Shared.SafeStorage.SaveAtomic` (atomic write + `.bak` backup) with automatic crash recovery.
- **Real-Time Search Bar:** Search field (`🔍 Search notes...`) above the list for fast title and full-text filtering.
- **Pinning / Favorites (📌):** Notes can be pinned (`IsPinned`), receive a prominent amber accent stripe, and are always sorted to the top.
- **Detail Actions (4 buttons):** `[Edit]`, `[Copy]` (copies the note content to the Windows clipboard via `GUIUtility.systemCopyBuffer`), `[Clone]` (duplicates the note), and `[Delete]` (with confirmation modal).
- **In-Game Quick-Stamp:** Header button `[ 🕒 Stamp ]` in the editor inserts the in-game day tag, weekday, and time (e.g. `[Day 14 (Wednesday), 15:30]`) with a single click.
- **Power-User Shortcuts:** <kbd>Ctrl+S</kbd> (save), <kbd>Escape</kbd> (back), <kbd>Tab</kbd> (title/text focus swap), <kbd>Ctrl+N</kbd> (new note), <kbd>Ctrl+F</kbd> (search).
- **Method 3 Responsive UI & Input Focus:** Dynamic canvas scaling via `UITheme.Sp/Dp` and the focus hook `NotesAppInputFocus` for typing without character movement.

**PotScanner v0.5.4 (2026-09-12, Audit Patch Round 2026-09-12):**
- **v0.5.3 (Bug Audit 2026-09-12):** WaterAll threshold comparison now uses the live value `c.NormalizedMoistureAmount` (previously: cached `info.WaterPercent`, up to 2s old — Skip/Charge mismatch on stale cache). Per-charge `cashBalance` re-check before every `ChangeCashBalance` deduction (break instead of negative cash, if parallel spenders lower the balance between iterations). `Constants.ModVersion` corrected to "0.5.3".
- **v0.5.2:** WasCollected guards (Update/Handler/PhoneClosed); row caches are cleared on unload.
- **v0.5.1:** Static event dispatcher + per-handler try/catch; WaterAll/WaterSinglePot host-only (MP money protection).
- **Quick Filter Tabs Toolbar:** 4 interactive filter pills (`[All]`, `[💧 Thirsty]`, `[★ Ready]`, `[⬡ Empty]`) above the list for filtering by thirst, ripeness, or emptiness, with dynamic group badges and auto-hide for empty groups.
- **Plant Quality Rating Display:** Shows the plant quality (`Q:{pct}%`) directly in the pot row header for growing and ready plants.
- **Single-Property Focus Mode (Accordion/Focus View):** Tapping a property (e.g. "Barn") hides all other properties so only that property's pots occupy the phone screen. Tapping the header again restores the full overview.
- **Compact Responsive UI (Method 3):** Dynamic `UITheme` engine with a damped scale factor (`Mathf.Clamp(ActualHeight / 900f, 0.75f, 1.20f)`), prevents oversized tiles on high-DPI screens.
- **Slim Dimensions & 0-Allocation:** Slim top buttons (`Dp(36f)`), filter pills (`Dp(26f)`), compact property headers (`Dp(30f)`), space-saving pot rows (`Dp(46f)`). Non-allocating in-place polling loop (0 GC allocs every 2s).
- **Dynamic Single-Water Click:** Clicking the water bar triggers `WaterSinglePot` at any time, even for a pot that started wet and later dried out.
- **Status Indicators & Features:** "All Moist" indicator for watered pots, 2s polling of all `GrowContainer`, "Water All" action button with 30% skip threshold, "Auto-Water" polling service (50g/pot, persisted via `ModConfig<PotScannerConfig>`), `pot` console commands (S1API + hash bridge).

**CalculatorApp v0.2.3 (2026-09-12, Audit Patch Round 2026-09-12):**
- **v0.2.3 (Bug Audit 2026-09-12):** `GetActiveSlotSuffix` now with `SaveSlotNumber >= 0` guard (closes the `calculator_state_slot_-1.json` path that NotesApp already had). `InputSquare` and `InputReciprocal` catch `decimal.OverflowException` and set "Overflow" instead of throwing uncontrolled into the button callback (previously: overflow threw from the button handler). `HasError` recognizes `DisplayText == "Overflow"` (previously: inputs were stuck on `"Overflow5"`).
- **v0.2.2:** WasCollected guards in UpdateDisplayUI/RefreshHistoryList.
- **v0.2.1:** Static event dispatcher; pointer/WasCollected guards in the slot suffix.
- **Exact Decimal Arithmetic:** High-precision `decimal` math engine eliminates floating-point rounding errors for financial, drug yield, and batch calculations.
- **In-Game Money Integration:** `[ 💵 Cash ]` & `[ 💳 Bank ]` quick-insert chips pull live player balances into the calculator via `S1API.Money.Money`.
- **Clipboard Integration:** `[ 📋 Copy ]` chip and <kbd>Ctrl+C</kbd> copy the displayed number to the Windows clipboard; <kbd>Ctrl+V</kbd> parses and pastes numbers into the calculator.
- **Scientific Utilities & Repeating Equals:** Square root (`√`), dynamic `C` (Clear Entry) / `AC` (All Clear), and repeated `=` execution.
- **Searchable History with Per-Item Actions:** Real-time search filter, tap-to-restore, per-item `[ 📋 ]` (Copy) and `[ 🗑️ ]` (Delete) buttons, and "Clear All History".
- **IL2CPP Input-Focus Protection:** `CalculatorAppInputFocus` prevents character movement (WASD) and game shortcut triggers while searching history.
- **SafeStorage Persistence:** Atomic JSON persistence (`UserData/CalculatorApp/calculator_state.json`) with `.bak` backup protection.
- **Physical Keyboard Support:** Direct numpad/keyboard keys, <kbd>Ctrl+C</kbd>, <kbd>Ctrl+V</kbd>, <kbd>Tab</kbd> / <kbd>Ctrl+H</kbd>.

**CustomSkateboard v1.1.5 (2026-09-13, Audit Patch Round 5 2026-09-13):**
- **v1.1.5 (Bug Audit 2026-09-13):** Removed dead state — `s_disableTerrainSlowdownCached` (only written, never read) and `ClearStyledCache()` deleted from `SkateboardVisualPatches`/`CyberSkateboardVisualizer`. No functional loss, just maintenance.
- **v1.1.4:** Resolve/tune-clamp divergence resolved.
- **v1.1.3:** Pass-2 deck heuristic `IsDeckMeshSane` added.
- **v1.1.2:** Settings-sharing detector (error log on shared _settings object).
- **v1.1.0/v1.1.1:** Tuning release (TopSpeed 140 km/h, Push/Steering); `IsGameplayScene` strictly `"Main"`.
- **Bugfix-Round (20 issues, 6 HIGH / 9 MEDIUM / 5 LOW)**: scene-callback symmetry, per-renderer Material leak (`sharedMaterial`), cached `AnimationCurve`/`Gradient` (no GC churn), `InitializeAssets` re-init fix, candidate-base-ID logging + override config, TOCTOU collapse, exact mesh-name match, light shadows/renderMode fix, `EnableKeyword` gating, log-spam gating via `logStats`, lazy `Mod.Log`, `OrdinalIgnoreCase` dialogue compare, `BoardLeanRate` config-exposed, config-fail → `Log.Error`.
- **New configs**: `BaseItemIdOverride` (default empty), `BoardLeanRate` (default 60f).
- **Avatar Protection & Whitelist Visuals:** Strict renderer filtering and targeted MeshFilter swap on the deck prevent any visual impairment of the player avatar (hair, eyes, clothing stay untouched).
- **Ultra-Responsive Carving & Instant-Jump:** Scalar values (`TurnForce: 15.0`, `TurnChangeRate: 64.0`, `TurnReturnToRestRate: 56.0`, `LateralFriction: 1.60`), clean jump duration (`JumpDuration_Min: 0.38s`, `JumpDuration_Max: 0.58s`) and native gravity landing.
- **High-Speed Push:** `AnimationCurve` cached as `static readonly` for progressive acceleration up to 100+ km/h without speed decay.
- **Anti-Gravel & Suspension:** Prevents slowdown on grass/gravel and uneven terrain via Harmony patches (`GetSurfaceSmoothness`, `IsOnTerrain`).
- **Store Injection & Console:** Dynamic dialog injection for Jeff Gilmore plus console commands `skate` (give, stats, help).
- **Strict Vanilla Isolation & Camera:** Pure runtime instance tuning at `OnMount` protects all vanilla boards (Golden Skateboard, Cruiser, etc.) from overwrites and preserves the native `SkateboardCamera` follow.

**MoreSaveSlots v1.0.12 (2026-09-13, Bug Report Round 6):**
- **v1.0.12 (Bug Report Round 6 2026-09-13):** S-05 "dead DEL/EDIT buttons" — Rename/Delete modals selected an unsorted canvas via `FindObjectsByType<Canvas>()[0]` and ended up behind the menu canvas (invisible, fullscreen dimmer ate all clicks). Fix: `UIHelper.FindDialogCanvas()` (SaveDisplay UI stack, fallback highest `sortingOrder`), own sorting overlay (`overrideSorting`, order 1000) + GraphicRaycaster per modal, `SetAsLastSibling` on re-open.
- **v1.0.11 (2026-09-13):** Savegame dupe fix (ghost cards).
- **v1.0.10 (Bug Audit 2026-09-13):** `CleanupOwnedTriggersForSlot(IntPtr)` (PaginationController.cs:39-68) explicitly cleans up owned EventTrigger pointers; `RefreshActiveScreen()` (lines 104-118) removes stale keys per slot — closes the residual leak from round-3 audit (F18 verification).
- **v1.0.9 (Bug Audit 2026-09-12):** EventTrigger cleanup (`PaginationController.AttachHoverTracker`) now only removes **own** entries (tracked via `_ownedTriggers` dictionary) instead of all pointer entries. Previously, vanilla hover/click handlers of the save slot cards were also removed, causing the hover highlight of the slots to disappear.
- **v1.0.3:** NewGame bounds guard; EventHelper hover without Clear(); font liveness; rename without regex fallback.
- **v1.0.2:** 1-based slot numbers in the scan; dead dialog block + SlotsPerPage config removed.
- **Expand Save Slots:** Expands game save slots from vanilla 5 up to 25+ slots (default 25, configurable in `UserData/MoreSaveSlots/config.json`).
- **Paginated Navigation:** Native `[ ◄ PREV ]` / `[ NEXT ► ]` navigation bar in `ContinueScreen`, `NewGameScreen`, and `ImportScreen`.
- **Save Game Renaming:** Inline save rename modal dialog with backdrop dismiss, canvas parenting, and `Game.json.bak` auto-backup before writing.
- **Configurable Hotkeys & Wheel:** Custom string keybindings in `config.json` (`PrevPageKey`, `NextPageKey`, `RenameKey`), plus mouse wheel scrolling for flipping pages.
- **Fast Caching & Focus UX:** `SlotCardCache` UI hierarchy caching, direct screen references, auto-jump to `LastPlayedGame` page on `ContinueScreen` open, and active slot highlight.
- **Harmony Patches & Scene Return Resilience:** Transparent remapping on `SaveDisplay.Refresh`, `SaveDisplay.Awake`, `SaveDisplay.SetDisplayedSave`, `ContinueScreen.LoadGame`, `NewGameScreen.SlotSelected`, `MenuScreen.OnOpen`/`OnClose`, and `SaveManager.Awake`. Skips native out-of-bounds loops when returning to Main Menu.


**PocketShop v0.3.2 (2026-09-19, Shop Payment Rules):**
- **v0.3.1 (Payment Rules 2026-09-19):** Correction of the v0.3.0 blanket switch — PocketShop also lists Black-Market suppliers (vanilla cash). Payment now follows `ShopInterface.PaymentType` (`EPaymentType`: Cash/Online/PreferCash/PreferOnline): Cash → `ChangeCashBalance`, Online → `CreateOnlineTransaction`, Prefer\* → both with preference. PaymentType is cached in `ShopCatalog.Refresh` per shop/item; no hardcoded names. UI: 💵+💳 balance chips, 💵 badge on BUY only for cash shops. In-game verification open.
- **v0.3.0 (Payment Overhaul 2026-09-19):** PocketShop is a legal business — Schedule I pays legal shops exclusively by card. All purchases now go through `CreateOnlineTransaction` (`onlineBalance`); `ChangeCashBalance` is no longer touched. Multi-Payment switcher (💵 Cash / 💳 Bank / ⚡ Auto) removed — three interactive chips → one non-interactive 💳 balance chip. `PaymentModeStatic` always resolves to `Bank`; `SelectedPaymentMode` remains as a legacy config field (default now `Bank`). Refund paths (Partial-Delivery, Outer-Gap) converted to online transactions. In-game verification of the card flow open (next session).
- **v0.2.6 (Feature Update 2026-09-17):** Vanilla level & rank requirements are now enforced (`StorableItemDefinition.RequiresLevelToPurchase` + `IsUnlocked`). Locked items show `🔒 LOCKED (REQUIRES [RANK])`, stock shows `🔒 LOCKED ([RANK])`, and buy buttons are disabled (`BuyResult.LevelLocked`, configurable via `EnforceLevelRequirements`). Direct inline number field in `QuantitySelector` like in Dan's Hardware: A click on the number between `[-]` and `[+]` activates an editable number field (`InputField`) directly on the card/in the detail view (e.g. entering 20 or 40 without an extra HUD/popup). With automatic stock clamping on confirmation and `PocketShopInputFocus` (WASD lock via `Controls.IsTyping` while typing).
- **v0.2.5 (Bug Audit 2026-09-13):** `RefreshShopCount()` (StoreCatalogPane.cs:211-218) selective count refresh; aggressive cache invalidation in `ShopCatalog.cs:95-96`; `ChangeBySafe(int)` (QuantitySelector.cs:136-151) edge-case guard at min/max endpoint (LOW findings F25-F28).
- **v0.2.4 (Bug Audit 2026-09-12):** `PurchaseService.BuyWithQuantity` now reads the live stock (`item.SourceListing.CurrentStock` + `IsInStock`) instead of just the POCO snapshot — closes coop oversell. `QuantitySelector.ClampTo` syncs `_maxStockOrSentinel` (field no longer readonly); sentinel (-1) is caught first (previously: <=0-first-branch reset unlimited items to qty=1). `HandlePurchaseResult` calls `_gridPane.NotifyStockChanged(itemId)` after a successful purchase → grid cards show fresh badge + clamped QuantitySelector max. New `ItemPOCO.ItemId` + `ItemCard.GetItemIdPublic()` for cross-card dispatch.
- **v0.2.3:** Catalog per-handler invoke; CurrentStock fallback; GridPane re-subscribe guard; TearDown with DirectoryPane.
- **v0.2.2:** Static event dispatcher + Dispose; dead mugshot scan out; partial refund only remaining; catalog refresh guarded.
- **Multi-Payment Switcher & Bank Integration:** Interactive chips in the sub-header (`[💵 Cash]`, `[💳 Bank Card]`, `[⚡ Auto]`). Supports physical cash (`ChangeCashBalance`) and online bank transfers (`CreateOnlineTransaction`). The `Auto` mode prefers cash and seamlessly falls back to the bank account when cash is insufficient.
- **Item Detail & Inspection Modal (`ItemDetailModal`):** Click on an item icon or name opens a high-resolution detail view with full price breakdown (base price, service fee, total), merchant source, stock count, and quick-quantity chips (`[+1]`, `[+5]`, `[+10]`, `[MAX]`) for fast bulk purchase.
- **Clean Shopping (No Disruptive Overlays):** Purchase confirmations complete without blocking banner overlays or fullscreen animations.
- **Native & Procedural Audio Feedback (`SoundService`):** Plays the game's real cash register chime (`MoneyManager.Instance.PlayCashSound()`) on successful purchases, soft clicks on quantity changes, and alarm tones on rejected transactions. Enable/disable via `PocketShopConfig.EnableSoundEffects`.
- **2-Level Navigation:** 4×3 grid with themed vector icons and merchant portraits (`StoreCatalogPane`) plus a 5×N item grid (`ItemGridPane`) with synchronized stock count.

**BankApp v0.4.4 (2026-09-12, Audit Patch Round 3 2026-09-12):**
- **v0.4.4 (Bug Audit 2026-09-12):** `DepositCash`/`WithdrawCash` now have a `NetworkGuard.IsInMainScene` guard before every money operation. Defense in depth: on scene-change-mid-call or hotkey invocation outside of Main, the service cleanly aborts with an error message instead of risking a partial transaction.
- **v0.4.3 (Bug Audit 2026-09-12):** `UpdateAmountDisplay`/`UpdateModeVisuals`/`SetFeedback` now use `IsAlive(...)` (previously only `!= null` check) — consistent with v0.4.2 RefreshAll. Prevents MissingReference spam on phone close or scene change.
- **v0.4.2:** WasCollected guards (RefreshAll/Update/PhoneClosed); PreLoad without slot reset.
- **v0.4.1:** Static event dispatcher (OnUpdate/Balance/History) against subscriber leaks.
- **Mockup-Based Redesign** (`Source/Mods/BankApp/docs/mockup-target-v0.3.0.png`): Weekly progress bar ($10k ATM limit), two-column balances (Cash | Online, teal), chip grid 2×5 ($1–$1000 + ✕ CLEAR + MAX accent), ⬇DEPOSIT/⬆WITHDRAW mode tabs, single full-width confirm button (green/orange by mode).
- **Chip Interaction:** Chips ADD to the running amount; MAX fills mode-dependent maximum (max depositable cash vs. max withdrawable); CLEAR resets.
- **Removed:** Free-text amount input + `BankAppInputFocus` (no typing in UI → WASD protection obsolete; IL2Cpp registration removed).
- **Unchanged Backend:** Double-entry transactions (rollback on failure), slot capacity, weekly limit, slot-isolated persistence, audio.

**Weather v0.1.0 (2026-09-24, initial release):**
- **Read-Only Weather Dashboard:** PhoneApp (`WeatherApp : PhoneApp`, `EOrientation.Vertical`) that displays the live state of the game's nine weather components (Sunny, Cloudy, Rainy, Stormy, Snowy, Foggy, Windy, Hail, Sleet) from `S1API.Weather.WeatherManager`.
- **Dominant-Condition Hero Readout:** Highest-weighted component large (`RAINY  62%`) in its own accent colour, with intensity line (`Heavy` > 0.66 / `Moderate` 0.33–0.66 / `Light` > 0 / `Clear` at 0) and a wide progress bar.
- **All Nine Components Listed:** Name + horizontal progress bar (0–1) + percentage per condition; zero-weight rows are dimmed and their bar stays empty so active conditions stand out.
- **Live Rendering:** `WeatherManager.OnWeatherChanged` re-renders immediately; while closed only the snapshot is cached and the view is rendered on the next open. A cheap per-frame `WeatherManager.Current` comparison covers changes that happened before the instance subscribed.
- **Graceful Empty State:** `Current == null` (outside gameplay / native services initialising) shows "No weather data available" plus an in-game hint instead of stale numbers.
- **Responsive Layout (Method 3):** `S1Mods.Shared.UITheme.Sp/Dp` for every font/dimension; the nine rows share the available height via `VerticalLayoutGroup` (`childForceExpandHeight`) — all conditions visible without scrolling at any resolution. <kbd>Escape</kbd> closes the app.
- **Lifecycle Per Runbook:** `_mainBG` full-anchored and starts inactive (Rule 3); `MelonEvents.OnUpdate` subscribed exactly once with defensive `Unsubscribe`-before-`Subscribe` in `OnCreated` (Rule 10); nothing destroyed or unsubscribed in `OnPhoneClosed` (Rule 2); the static `OnWeatherChanged` handler dispatches through a single `_active` instance (no subscriber stacking across scene reloads).
- **No Persistence / No Gameplay Impact:** Pure status display — no save files, no money or gameplay interaction, no input fields (therefore no `InputFocus` hook needed).

**BusinessIncome v0.1.6 (2026-09-17, 0-Business Backlog Fix):**
- **v0.1.6 (2026-09-17):** Fix for 0-business backlog spam when loading a new save.
- **v0.1.5 (Bug Report Round 6 2026-09-13):** BIZ-01 Catch-up loop after cap re-read + hard-capped to `MaxCatchupDays` + LastPaid validation on load (freeze on corrupted state); BIZ-02 `biz trigger --commit` requires `--force` on already paid day (money printer closed); BIZ-03 `float.IsFinite`/UpperBound guards in `Sanitize` incl. `MaxCatchupDays` clamp (prevents NaN economy brick); BIZ-04 `biz pending confirm` only commits forward; BIZ-05 `OnPreLoad` keeps the slot (`keepSlot:true`, no `*_default.json` in the load window).
- **v0.1.4 (Bug Audit 2026-09-12):** `IsHostOrSingleplayer` now fail-**closed** on exceptions (`return false` instead of `return true` — the singleplayer path is already covered via the `NetworkManager == null` branch; an authority marshalling error on an MP client no longer allows duplicate bookings). On `commit && !committed` (money booked but save failed) the HUD notification now shows a separate "booked — save FAILED, run `biz pending confirm|resolve`" hint instead of the misleading success message; pending marker stays on disk, the `biz pending` console resolve path was already present.
- **v0.1.2:** Windfall seed is persisted (CommitPayout).
- **v0.1.1:** Windfall seed on first install; catch-up via OnDayPass; invariant display format.
- **Daily Passive Revenue:** Generates daily passive income for all owned businesses (`Business.OwnedBusinesses`) via online bank transfer through `S1API.Money.Money.CreateOnlineTransaction`.
- **Multiplayer Host Authority:** Strict server/host guard (`IncomeEngine.IsHostOrSingleplayer()`) prevents duplicate payouts and money duplication on client instances.
- **Savegame Slot Idempotency & SafeStorage:** Slot-isolated persistence (`payout_state_slot_{slotId}.json`) with atomic write and `.bak` crash protection. Idempotent `LastPaidElapsedDay` marker guards against double-booking across scene reloads and save reloads.
- **Deterministic Revenue Model:** Pseudo-random variance (±15%) computed via hash from `(ElapsedDays, BusinessId)`. Employee bonus (+5%/employee, cap 25%), weekend bonus (+25% for bars/nightclubs), and configurable operating costs (10%).
- **HUD & Audio Feedback:** In-game HUD notification via `NotificationsManager.Instance.SendNotification` with cash register chime.
- **Console & Terminal Dashboard:** Full dev-console (`~` / `F1`) and DooDesch `hash` terminal (`#`) integration (`biz stats`, `biz trigger [--commit]`, `biz config`, `biz set <key> <val>`, `biz help`).

**Minimap v2.0.2 (2026-09-13, Bug Report Round 6):**
- **v2.0.2 (Bug Report Round 6 2026-09-13):** M-05 - `ContentSizeFitter` (`PreferredSize`) on settings scroll content; Save&Apply footer is now reachable on long content (scroll never worked correctly before).
- **v2.0.1 (Bug Audit 2026-09-12):** `ResolveSlotSuffix → Save()` recursion fixed (crash): `_dirty` is now consumed before `FlushToPath(oldPath)` → no more uncatchable `StackOverflowException` on slot switch after a failed waypoint save (AV/read-only); new `FlushToPath(string)` writes to an explicit path without re-resolving `ResolveSlotSuffix`. Health bar gets `sizeDelta = (mapSize*0.92f, 6f)` (previously Unity default 100x100 → overlapped the map).
- **v1.0.3:** Critical-first blip partition (pool overflow); raycast sync in ApplyLayout.
- **v1.0.2:** Config save debounced (toggle/unload instead of per zoom key).
- **Dual-Shape Viewport (Radar vs Tactical):** Dynamic uGUI mask switch between **Circular Radar** (with rotating compass ring N/E/S/W) and **Rounded-Square Tactical GPS** via procedural anti-aliasing.
- **Integrated DayCounter HUD:** Seamless integration of day (`DAY 14`), weekday (`Wednesday`), and clock (`15:30`) as a compact header or footer bar with emerald accent stripe and 0-allocation state caching.
- **Dynamic Rotation & Zoom:** Two rotation modes (`FollowPlayer` with rotating map vs. `NorthUp` with rotating player arrow) plus smooth live zoom (0.75×–4.0× via <kbd>[</kbd> / <kbd>]</kbd> / numpad).
- **0-Allocation Pooled Blip Engine:** Pre-allocated pool for 64 blips with fast scene scan (police, dealers/shops, customers, properties/trailers, quests) and shape-aware edge clamping at the minimap border.
- **Drag & Drop & Screen Bounds:** Free mouse drag while menus/pause/phone are open with automatic screen clamping and SafeStorage persistence (`UserData/Minimap/config.json`).
- **Console & Terminal Integration:** Dev console and DooDesch `hash` terminal (`minimap toggle`, `minimap shape`, `minimap zoom`, `minimap pos`, `minimap blips`, `minimap day`, `minimap help`).



**StackLimitMod v0.1.6 (2026-09-19, IL2CPP-Typcheck-Fix):**
- **v0.1.6 (CRITICAL FIX 2026-09-19):** `is` type checks do not work on IL2CPP proxy objects (managed wrapper is always of the declared type). `IsAgricultureItem`/`IsWeaponOrAmmo` therefore misclassified real product definitions (ogkush/meth/cocaine → WeedDefinition/MethDefinition/CocaineDefinition) as "not-agriculture" — they were never raised to the stack limit; cash/clothing protection only ran via ID keywords. Fix: `TryCast<T>()` instead of `is` (real IL2CPP class hierarchy). Proven via apply_report.json (v0.1.5 diagnosis).
- **v0.1.5 (Timing Fix 2026-09-19):** Fixes "packaged products don't stack": An apply that ran too early (`Registry:0`) was preserved by the 1500ms dedupe in `OnLoadComplete` — the canonical post-load apply never happened. OnLoadComplete now re-applies when the last apply had `LastRegistryCount==0` with `ModifiedItemCount>0`. New: `apply_report.json` (per item id/type/original/decision/source), console `stack check <itemId>` + `stack report`, config flag `LogDecisions` (default false) for hot-path decision logging. Statically verified (interop decompiles): packaged products retain the product definition ID, packaging only in instance field `ProductItemInstance.PackagingID`; no `get_StackLimit` override in derived instance classes.
- **v0.1.4 (Bug Fix 2026-09-15):** Agriculture-only mode (`AgricultureOnly: true` as default): Restricts stack-limit overrides strictly to agricultural items (soil, seeds, packaging like baggies/jars, fertilizer/additives, mushroom spores/spawn, harvest/drug products). Hard guard against weapons (melee/firearms), ammo, clothing, and cash: weapons & ammo always retain their native limit (1 or vanilla) and are strictly blocked in the postfix on `BaseItemInstance.get_StackLimit`. Prevents Chinese characters / glyph corruption in the UI and ammo underflow (`-1 ammo`) on reload. Automatic restoration of non-agricultural items on startup. New console command `stack set ag <true|false>`.
- **v0.1.3 (Bug Audit 2026-09-12):** `StackLimitEngine.RestoreAll()` new: restores every tracked original `StackLimit`. Called in `Mod.OnDeinitializeMelon`, so that mod disable/unload doesn't leave the definitions with overwritten limits. `OverrideNonStackable` default stays `true` (backcompat), but documented in config comment: can make quest/unique items stackable.
- **v0.1.2:** Exclude-Restore; Decision-Cache with cap + known-check.
- **v0.1.1:** Decision-Cache in hot path; toggle-off restores original limits.
- **Configurable Global Stack Limit:** Dynamically overrides item stack limits (1–9999, default 40) across player inventories, storage, shelves, and trunks.
- **Dual-Layer Item Discovery:** Automatically scans in-memory `BaseItemDefinition` assets via `Resources.FindObjectsOfTypeAll` and dynamic registry items via `Registry.Instance.GetAllItems()`.
- **Runtime Hook & Safety Patches:** Harmony postfix on `Registry.AddToRegistry` to automatically apply rules to newly registered items, plus instance-level patch on `BaseItemInstance.get_StackLimit` secured via `PatchGuard`.
- **Exclusions & Non-Stackable Control:** Configurable `OverrideNonStackable` toggle and `ExcludedItemIds` whitelist/blacklist.
- **In-Game Console & Terminal Integration:** Dev console (`~`) and DooDesch `hash` terminal (`#`) commands: `stack stats`, `stack set <amount>`, `stack reload`, `stack help` (with `stacklimit` alias).
- **SafeStorage Persistence:** Atomic JSON persistence (`UserData/StackLimitMod/config.json`) with automatic `.bak` recovery.


**AutoPackagingStation v0.3.3 (2026-09-13, Bug Report Round 6):**
- **v0.2.7 (Bug Report Round 6 2026-09-13):** Host guards for PackUp, OnDestroy refund, all `TryExtract*`/`TryDeposit*` paths and OnSaveComplete (9 new `IsHostOrSingleplayer` guards). MP clients can no longer locally pay out/destroy stations (dupe/desync) and no longer overwrite the host's slot file. Supersedes 0.2.4.
- **v0.2.6 (Bug Audit 2026-09-12):** F-key PackUp reentrancy protection: `_packingUp` flag plus `try/finally` reset in `PackUpStation` prevents double payout, if `Destroy_Server` triggers a second dispatch in the same frame.
- **v0.2.5:** Quality-Mixing native (HIGH) + ObjLoader 50MB/250k vertex cap.
- **v0.2.4:** PackUpStation CRITICAL-Fix (BuildableItem.Destroy_Server instead of GameObject.Destroy), Output-Pre-Flight.
- **v0.2.1/v0.2.2:** Output-Extraction + Native-Sync + GUID-Sync (v0.2.1); rData-Dupe-Fix, Snapshot/Revert, PackUp-Fit-Check, Slot--1-Guard (v0.2.2).
- **4x4 Industrial Automated Packaging:** Instantiates a massive 4x4 animated machine with conveyor belt, pneumatic press, and status LEDs.
- **Atomic 2-Phase Engine:** Deducts inputs (raw product + packaging material) in the exact same frame as output insertion, preventing TOCTOU duplication exploits.
- **Weighted Quality Mixing:** Automatically blends input product quality using weighted average based on item quantities.
- **SafeStorage Slot-Isolation:** Stores station states separately per save slot under `autopack_slot_{slotId}.json` with `.bak` safety backups.
- **Strict Raycast & Key Guards:** Fixes AOE dismantling using strict raycast target validation and blocks key input whenever cursor is unlocked (UIs open).
- **Multiplayer & IL2CPP Stability:** Uses `IsServer` checks to prevent client desyncs and implements native `Il2CppType.Of` component setups to avoid crashes.

**HitmanPhone v0.2.9 (2026-09-15, KnockOut & Quest-Lifecycle In-Game-Verified):**
- **v0.2.8 (In-Game-Verified 2026-09-15):** Patch on `S1NPCHealth.KnockOut` intercepts non-lethal knockouts (fists/melee), so targets no longer need to be thrown into water. 5-second latch (`_recentlyHandledNpcIds`) in `BountyService` prevents double drops of Polaroids and floating ragdoll clones. `CleanupOrphanBountyQuests()` and clean 1:1 rebind in `BountyJournalBridge` clean up orphaned/duplicated quests in the HUD on load. Live verified: 3 contracts (Kyle Cooley, Chloe Bowers, Sam Thompson) played back-to-back, each spawned 1 Polaroid, dead-drop validated, cash paid out, quests cleanly at 0 active / 9 historical completed.
- **v0.2.7 (2026-09-14):** Restore-BountyQuests render in the journal as "Hitman Contract" instead of "Hitman Contract: <NPC>" — the vanilla UI snapshot in `QuestComponent` is set once in the ctor and not re-triggered by the v0.2.6 reflection setter on `s1q.title`; v0.2.7 syncs the vanilla UI title.
- **v0.2.5 (bug-ludwig-double 2026-09-13):** Cross-session fallback in `TryValidateAndPay` rejected every polaroid drop as soon as >1 contract `AwaitingDrop=true`. But Crow offers new bounties on the same NPC for days (ludwig_meyer Day 5+6). After save reload, the polaroid `Value=0` (Unity InstanceID gone) → refuse. Fix: new `GroupSameTargetAwaiting` helper groups waiting contracts by `TargetNpcId`; with uniform `TargetNpcId`, the entire group is paid out together (one kill = one evidence = multiple bounties), heterogeneous NPCs remain protected. `OnStorageContentsChanged` does side effects (Reward/Journal/Heat/Cooldown) per contract in the list and consumes ONE polaroid for the entire group. Persist once per batch, unless a payout failed (retry path).
- **v0.2.4 (Bug Audit 2026-09-12):** `OnStorageContentsChanged` now lets the client player validate and pay with their own save (previously: host-only gate blocked client payout completely because the host-local save never saw the client's `AwaitingDrop` contract); `ChangeCashBalance` syncs player balance via FishNet. `BountyTargetWatchdog` checks `npc.Health.IsDead` instead of `!npc.IsConscious` — prevents a K.O. (non-lethal stun) from counting as a kill with full reward + lethal pursuit.
- **v0.2.3:** Deadline >=; single-slot-consume; OnDecline guard; death log on Debug.
- **Bounty/Contract Gameplay:** Anonymous clients ("Ghost", "Jackal", …) offer hits via Phone Messages app; targets from the dealer customer network; "Polaroid Evidence" item (InstanceID-encoded) is redeemed in the dead drop for dirty cash.
- **Systems:** Police-Heat-Integration, Journal-Quests, 3-day expiry, Death-Forfeit, slot-isolated persistence (`slot_{n}` + TryMigrateLegacy).
- **v0.2.2 Audit Fixes:** Payout host-only, PatchGuard instead of PatchAll, day-throttle, cooldown-persist, deadline-boundary, test-commands only DEBUG.

**MessagesPlus v0.1.0 (2026-09-24, initial release):**
- **Patch-only mod (no PhoneApp, no homescreen icon):** Harmony patches on the vanilla `MessagesApp` (`Start`, `SetOpen`, `Loaded`) via `PatchGuard.TryPatch` inject a `[Clear All]` button into the existing Messages toolbar and a collapsible `[Papierkorb (N)]` trash section below the conversation list (`TrashUI` into `MessagesApp.homePage`).
- **Clear All + Trash/Restore:** Clear All (with confirmation popup) hides all visible threads via `MSGConversation.SetEntryVisibility(false)`; per-thread `[↩]` restores with `SetEntryVisibility(true)` + `MoveToTop()` + `RepositionEntries()`; `[Trash leeren]` permanently removes trashed threads behind a second confirmation (removed from `MessagesApp.Conversations`/`ActiveConversations`, re-removed after every save load via a purge list — native saveables/UI objects are never destroyed).
- **Slot-Isolated Persistence:** `trash_slot_{n}.json` via `SafeStorage.SaveAtomic` (triple-guarded slot suffix via `SaveSlots.GetActiveSlotNumber()`, never `slot_-1`), loaded on `GameLifecycle.OnSaveInfoLoaded` and re-applied after vanilla `MessagesApp.Loaded`.
- **Multiplayer:** all trash mutations host-only (`NetworkGuard.IsHostOrSingleplayer`); clients see a read-only trash.
- **Phase 1 of 3:** toast/sound (Phase 2) and configurable background (Phase 3) are pre-defined in `MessagesPlusConfig` but not active yet.

**_DiagPerfCounter v0.3.2 (2026-09-11, Dev-Tool):**
- **v0.3.2:** Only DEBUG builds; dump via SaveTextAtomic with return types.
- **Purpose:** Diagnostic tool — dumps `StorageEntity` method signatures via reflection to `UserData\_DiagPerfCounter\dump.txt` (hook-target discovery). No gameplay effect.
- **v0.3.1:** `Source/Archive/_DiagPerfCounter/docs/mod.json` added; dump path moved from UserData root. *(2026-09-20: mod moved to `Source/Archive/`, obsolete.)*

---


### Archived Mods (`Source/Archive/`)

**BackpackMod v1.2.3 (archived):**
- **Description:** 3D wearable backpacks with realistic harness straps, tier-based storage scaling (+4/+8/+16 slots), runtime `.obj` model loading, 360° mannequin rotation, and button-only inventory sorting with stack-merge.
- **Archive path:** `Source/Archive/BackpackMod/`
- **History:** v1.2.3 (StorageMenu sort overlay fix), v1.2.2 (raycast protection), v1.2.0 (GetCopy atomic + overflow sidecar).

**DayCounter v1.0.0 (archived):**
- **Description:** On-screen HUD & clock overlay with 2 layout modes, real-time drag & drop, 4 anchor presets, and full dev-console & hash-terminal integration.
- **Archive path:** `Source/Archive/DayCounter/`

**ProfitTracker v1.1.0 (archived):**
- **Description:** Compact top-right HUD with net-worth tracking (cash, bank, business assets), rolling 7-day history, and in-game commands.
- **Archive path:** `Source/Archive/ProfitTracker/`

**TVBrowser v0.1.0 (archived):**
- **Description:** In-game TV web & YouTube app for property TVs with viewport rendering and navigation bar.
- **Archive path:** `Source/Archive/TVBrowser/`

**HomelessMod v0.1.12 (archived 2026-09-17):**
- **Description:** Street Nomad lifestyle — Everywhere Building (unrestricted placement via `BuildUpdate_Grid` patches), procedural 3D sleeping bag, outdoor item dismantling, Street Nomad questline (3 quests), slot-isolated persistence (`street_items_slot_{n}.json`). DLL/deploy to `Mods\_archived\` (save data in `UserData\HomelessMod\` preserved).
- **Archive path:** `Source/Archive/HomelessMod/`
- **History:** v0.1.12, **Verified 2026-09-15**; detail history see `Source/Archive/HomelessMod/docs/CHANGELOG.md`.

**SnackVendor v0.0.9 (archived 2026-09-17):**
- **Description:** Player-stocked vending machine (S1API Buildable + Vanilla-VendingMachine-clone, GLB-mesh via `S1MAPI.GltfLoader`, NPC-inventory-credit, Deposit/Extract-panel, hardware-shop-listing). Build OK, in-game verify never performed. DLL/assets to `Mods\_archived\` (incl. `Mods\_archived\SnackVendor\` GLB folder).
- **Archive path:** `Source/Archive/SnackVendor/`

**Convention Archive & `.bak`:** Archived mods live in `Source/Archive/`, are not part of `S1Mods.sln`, and are not built. MelonLoader does not load `*.dll.bak` files. The source code is fully preserved.

---

## 3. Build & Deploy

### Commands

```pwsh
# Single mod
dotnet build Source\Mods\NotesApp\src\NotesApp.csproj -c Release

# All mods in the solution
pwsh Tools\build-all.ps1
# or directly via dotnet:
dotnet build Source\Mods\S1Mods.sln -c Release

# Run all unit tests across the solution (Shared, AutoPackagingStation, CalculatorApp)
dotnet test Source\Mods\S1Mods.sln -c Release

# Regenerate the solution (after adding a new mod or test project!)
pwsh Tools\gen-sln.ps1

# Scaffold a new mod
pwsh Tools\new-mod.ps1 -Name <ModName>
```

> **ThirdParty deploy (optimized 2026-09-16):** `Tools/deploy-thirdparty.ps1` now runs only once tied to `Shared.dll`, instead of redundantly 16x in parallel per mod. Mutex switched to local session level.
>
> **Note:** `Tools\` was reactivated from the git history on 2026-09-10 (all helpers except `fix-knowledge-paths.ps1` — Knowledge/ no longer exists; reactivation: `git show 891c330^:Tools/fix-knowledge-paths.ps1`).
>
> **Drift guard (2026-09-14, new):** `Tools/check-version-sync.ps1` compares the code version (MelonInfo attribute or `Constants.ModVersion`) with `Source/Mods/<Mod>/docs/mod.json`, `README.md`, the AGENTS.md matrix row, and the AGENTS.md detail header. Exit 1 on drift; runs in `.github/workflows/ci.yml` and in the pre-commit hook. `Tools/bump-version.ps1` now also writes the detail headers. **Additionally (2026-09-16):** `Tools/check-doc-paths.ps1` verifies that all repo paths referenced in markdown exist (same gates) — doc paths are thus the second guarded drift class.
>
> **Cleanup note (2026-09-10):** In `<GameDir>\Mods\` were pre-existing two reference assemblies (`S1API.dll` 415 KB, `Hash.dll` 188 KB) from earlier manual deploys. Both raised `BadImageFormatException` on game start (Reference assemblies should not be loaded for execution) and flooded the log with ~50 ERROR lines, without breaking anything functionally. Deleted — the real versions are: `S1API.Il2Cpp.MelonLoader.dll` (Mods\), `Plugins\S1APILoader.dll`, `UserLibs\S1MAPI_Il2cpp.dll` (for AutoPackagingStation.GltfLoader). Hash.dll stays only as reference in the repo (`ThirdParty/Archive/ScheduleOne-Hash/`), not deployed — PotScanner uses the S1API hash bridge without DLL via reflection queue.

Deployment runs **automatically** via `Directory.Build.targets` — with split targets (convention since the 2026-09 reinstall):

- `<GameDir>\Mods\`: DLL + PNGs + Bundles (only files loadable by MelonLoader)
- `<GameDir>\UserData\<Mod>\`: `mod.json` (metadata) + `<Mod>.pdb` (portable debug symbols via `<DebugType>portable</DebugType>`)

**JSON and PDB never belong in `Mods\`.**

### ⚠️ Important Build Caveats

- **SkipUnchangedFiles:** Fixed to `false` (2026-08-20) — every `dotnet build` now force-deploys to `<GameDir>\Mods\`, eliminating stale-DLL traps.
- **GameDir Override:** `$env:SCHEDULE1_PATH` must be set **before** running `dotnet build`, because MSBuild evaluates the property once at startup.
- **Solution Determinism:** `Tools/gen-sln.ps1` now uses deterministic MD5-GUIDs (`Get-DeterministicGuid "Project:<rel>"`) — no more random GUID diffs on every regeneration. CI compares line endings normalized (checkout-independent).
- **Version Bumps:** Use `pwsh Tools/bump-version.ps1 -Mod <Name> -Version x.y.z` to sync `Mod.cs` + `mod.json` + `CHANGELOG.md` + `AGENTS.md` atomically.

### Sideload + hash (ThirdParty, NOT in the Solution)

Sideload (DooDesch) and hash (DooDesch) live after the 2026-09 move as **finished release packages** under `ThirdParty/` (ZIP + extracted, no source/build anymore): `ScheduleOne-Sideload/Sideload_extracted/` (Sideload 1.7.0, incl. `UserLibs/` dependencies) and `ScheduleOne-Hash/Hash_extracted/` (hash 1.0.5). Both are **not** part of `S1Mods.sln`.

**Deploy (manual, if needed):**
```pwsh
# hash 1.0.5 (deprecated, undeployed — only reference in repo):
# DLL is under ThirdParty/Archive/ScheduleOne-Hash/Hash_extracted/Hash.dll.
# Sideload 1.7.0 is completely removed (deprecated 2026-09-10, no archive rest);
# Hash.dll required Sideload at runtime and is therefore no longer runnable
# deployment-worthy — if really needed, procure Sideload anew (DooDesch, Nexus).
```

### Multi-PC Workflow (Laptop ↔ Home PC)

Linear workflow on `main`, no feature branches. On first clone on a new PC:

```pwsh
# 1. Clone repo
git clone https://github.com/domi7602/Schedule1-mod-only.git
cd Schedule1-mod-only

# 2. Generate submodule + build props (idempotent, asks for game path if not standard)
pwsh Tools/setup-workspace.ps1

# 3. Build
dotnet build Source/Mods/S1Mods.sln -c Release
```

`local.build.props` (S1API + S1MAPI) are gitignored and generated by the script per PC. Game saves (`<GameDir>\UserData\<Mod>\*.json`) are also PC-local — on switch each PC only sees its own saves.

Before each session on a different PC: `git pull`. After each session: `git add && git commit && git push`.

**Laptop sandbox workspace (variant A):** If you want to work on a second PC (laptop, test machine) independently of the home PC without endangering the stable repo: `pwsh Tools/new-laptop-workspace.ps1`. The script clones the repo, renames `origin` to `upstream` (so accidental pushes are physically excluded), optionally configures a private backup remote as the new `origin`, and runs `setup-workspace.ps1` directly. **No GitHub fork** — one code truth on GitHub, local sandbox alongside. Merge from the stable repo: `git fetch upstream && git merge upstream/main`.

---

## 4. IL2CPP vs Mono (Alternate Branch) — Architecture Decision

**Decision: We stay firmly on IL2CPP (default Steam release).**

- **Why?**
  1. The workspace is already fully set up and stable for IL2CPP with MelonLoader 0.7.3, S1API 3.2.1-beta.5, `Directory.Build.props`, and `UITheme`.
  2. Mods run natively for all regular Steam players (95%+), without requiring users to manually switch to the `alternate` beta branch.
  3. All common IL2CPP hurdles (`IntPtr` constructors, canvas DPI scaling, lifecycle timing) are solved in our mods and documented as reusable patterns.

---

## 5. Mod Conventions & Best Practices

### Responsive UI Design (Method 3: `UITheme`)

In Schedule I, S1API instantiates the phone container on a high-DPI uGUI canvas that is often rotated by 90° (`Quaternion.Euler(0, 0, 90)`). Fixed integer font sizes (e.g. 14pt–18pt) appear extremely tiny on some screens.

**Central implementation:** `Source/Mods/Shared/src/UITheme.cs` (`S1Mods.Shared.UITheme`) is the single source of truth. Per-mod pass-through wrappers (e.g. `BankApp.UI.UITheme`) were removed 2026-09 (commit 8c490fc) — every mod uses `S1Mods.Shared.UITheme` directly and keeps only its own color palette constants.

```csharp
// Text-heavy app:
S1Mods.Shared.UITheme.InitializeForTextApp(containerRt); // 750f, 0.85-2.0
// Dense dashboard (PotScanner):
S1Mods.Shared.UITheme.InitializeForDashboard(containerRt); // 900f, 0.75-1.2
// Custom:
S1Mods.Shared.UITheme.Initialize(containerRt, refHeight: 750f, minScale: 0.85f, maxScale: 2.0f);
```

In `OnCreatedUI(GameObject container)` always call `UITheme.Initialize...` first, then compute all fonts with `UITheme.Sp(...)` and paddings with `UITheme.Dp(...)`.

### Known IL2CPP Pitfalls (Checklist)

- **`[RegisterTypeInIl2Cpp]` MonoBehaviours require a public `IntPtr` constructor:**
  ```csharp
  public class NotesAppInputFocus : MonoBehaviour
  {
      public NotesAppInputFocus(IntPtr ptr) : base(ptr) { }
  }
  ```
  Without this constructor the IL2CPP bridge crashes during injection.
- **UnityEvent Listeners:** `button.onClick.AddListener(new UnityEngine.Events.UnityAction(...))` fails because of IntPtr conversion. **Always** use `S1API.Utils.EventHelper.AddListener(...)` or `ButtonUtils.AddListener(...)`.
- **Multiline InputField Alignment:** For large note fields, uGUI defaults to centered text. Always set `textComponent.alignment = TextAnchor.UpperLeft`, `placeholder.alignment = TextAnchor.UpperLeft` and `lineType = InputField.LineType.MultiLineNewline`, plus clean `offsetMin/Max` margins.
- **Save-Load Timing:** Static lists (like `Property.OwnedProperties`) are often still empty at `OnGameplaySceneLoaded`. Subscribe to the `S1API.Lifecycle.GameLifecycle.OnSaveInfoLoaded` hook, which fires after save parsing but before scene build.
- **`UIFactory.CreateTextBlock` vs. Layout:** `UIFactory.Text` sets anchors to `(0.5, 0.5)` by default. For custom rows, always set positions manually via `rectTransform.anchorMin/anchorMax/offsetMin/offsetMax`.

### Patch Resilience & Shared Utilities (`S1Mods.Shared`)

- **`PatchGuard`:** Catches changed method signatures for Harmony patches. Prevents game crashes on updates via graceful degradation (`PatchGuard.TryPatch(...)`), no guessing on overloads, transpiler/finalizer support, and status reporting (`PatchGuard.Report()`).
- **`SafeStorage`:** Protects against file corruption and load crashes through atomic write operations (`SaveAtomic`), auto-backup (`.bak`), fault-tolerant JSON parsing (`LoadSafe<T>`), and safe directory creation (`EnsureDirectoryForFile`).
- **`GameObjectResolver`:** Recursive search for UI components (`FindComponentDeep<T>`), cleanly returns `null` when `hintName` is missing instead of blindly taking the first child, and supports cache invalidation (`InvalidateCache()`).
- **`SafeInvoker`:** Wraps event and lifecycle callbacks in try-catch blocks (`Execute`) so single failures after patches do not stop the Unity main loop.
- **`HotkeyManager`:** 0-allocation KeyCode router with `(KeyCode, Modifiers)` keys (`HotkeyBinding`), `strictModifiers` filter, cooldowns, and InputField focus protection.
- **`ModConfig<T>`:** Type-safe MelonPreferences integration with boxing/enum/type casting, auto-save, change events, and `ModLogger` propagation.
- **`ModLogger`:** Centralized mod logging with auto `[ModName]` prefix for all levels (`Info`, `Warn`, `Error`, `Debug`) and exception overloads.
- **`NetworkGuard` & `SceneGate`:** Safe execution in game scenes (`InGame(action)` via `SafeInvoker`), configurable main scene, automatic scene tracking, and cache invalidation.
- **`TypeResolver`:** Resilient type resolution across all loaded assemblies (`TypeResolver.Find(...)`) with internal reflection cache.

---

## 6. Workflows

### Pre-Flight Check with S1Interop

```pwsh
# One-time installation (external tool, no repo reference; requires .NET SDK 8+):
dotnet tool install --global S1Interop --version 0.1.0-alpha.1

s1interop doctor --mono-game-path $env:SCHEDULE1_PATH --il2cpp-game-path $env:SCHEDULE1_PATH
s1interop analyze "$env:SCHEDULE1_PATH\..\..\Users\pc\Schedule1-mod-only\Source\Mods\<Name>\src\<Name>.csproj"
# or simply, from the workspace root:
s1interop analyze "Source\Mods\<Name>\src\<Name>.csproj"
```

> **s1interop is advisory (2026-09-19):** `analyze` always returns **exit 0** — even with findings. It is a report, not a gate: the output must be read. Known false positives of alpha 0.1.0-alpha.1: `wrong_target_framework`/`global_usings_require_langversion` (TFM + LangVersion are set in `Directory.Build.props`, the csproj alone does not know them), `ManagedCollectionSignatureInterop` in BusinessIncome (mod-internal API), reflection findings in HitmanPhone (internal S1API type, defensively secured). Full assessment see `memory-bank/decisionLog.md`.

### Mod Update Cycle

1. Edit code in `Source/Mods/<Name>/src/`
2. `dotnet build Source\Mods\<Name>\src\<Name>.csproj -c Release` (or `pwsh Tools/build-all.ps1`)
3. DLL is automatically deployed to `<Game>\Mods\` (force, no stale-DLL)
4. Launch the game & check `Latest.log`
5. For version bumps: `pwsh Tools/bump-version.ps1 -Mod <Name> -Version x.y.z` (syncs 5 files incl. README.md)

### CI & Quality Gates

- **Local:** `dotnet format --verify-no-changes`, `pwsh Tools/gen-sln.ps1` (determinism check), `pwsh Tools/check-version-sync.ps1` (version drift guard), `pwsh Tools/check-doc-paths.ps1` (doc path guard), `dotnet test Source/Tests/Shared.Tests/Shared.Tests.csproj` + `dotnet test Source/Tests/AutoPackagingStation.Tests/AutoPackagingStation.Tests.csproj` + `dotnet test Source/Tests/CalculatorApp.Tests/CalculatorApp.Tests.csproj` (tests need game assemblies)
- **CI:** `.github/workflows/ci.yml` runs on push/PR (format + version-sync check + game-gated build/tests + gen-sln check + doc-paths check). The s1interop CI job was removed on 2026-09-20: on GitHub-hosted runners, Schedule I is never installed, the analyze step was permanently skipped (no-op). s1interop stays as **local pre-flight** (see above) — advisory, never blocking (exit always 0).
- **Pre-commit:** `git config core.hooksPath .githooks` enables `.githooks/pre-commit` (format + gen-sln determinism + version-sync)
- **License:** `LICENSE` (MIT workspace + Third-Party notices), `CONTRIBUTING.md` for contributors

### Maker-Checker Workflow

1. **Pre-Task:** Load relevant `Skills/` to identify architectural constraints and known gotchas.
2. **Execution (Maker-Checker):** Strictly adhere to architectural pillars and 7 Golden Rules. Use separated coder/verifier subagents for code generation and audit.
3. **Post-Task (Proactive Learning):**
   - Proactively update relevant skills in `Skills/`.
   - Update `AGENTS.md` and mod `CHANGELOG.md` if inventory, versions, or dependencies change.

---

## 7. Reference Material

> **Note:** This mod-only workspace does not include the `Knowledge/` folder (decompiles, analyses, maps, tools). The earlier main workspace `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule 1 Modding` **no longer exists** after the reinstallation (2026-09) — the knowledge base (4804 files / 67 MB) must be regenerated if needed (AssetRipper/ilspycmd) or restored from a backup.

Reference decompiles and analysis snippets for archived mods are available in `Source/Archive/`.
