# AGENTS.md

Workspace: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule1-mod-only-main` — MelonLoader modding workspace for *Schedule I* v0.4.6f13 (TVGS).

> **Umzug 2026-09 (Neuinstallation):** Das Repo liegt jetzt **im Spiel-Ordner** (`<GameDir>\Schedule1-mod-only-main`); der alte Pfad `C:\Users\pc\Desktop\Schedule1-mod-only` ist obsolet. Setup 2026-09-04 wiederhergestellt: .NET SDK 8.0.424 installiert, S1API 3.2.0 (Fork-Build aus `ThirdParty/S1API/`) deployed, NotesApp + Shared als Verifikation gebaut. Deploy-Konvention seit 2026-09: DLL/PNG/bundle → `Mods\`; `mod.json` + `.pdb` → `UserData\<Mod>\`.

> **For AI Agents:** This is the bootstrap file. Read it completely before doing anything to the mods. If you make assumptions not stated here, ask first. If you find something here that is *wrong* or outdated, please update it.

---

## 0. AI-Agent Skills (`.agents/skills/`)

> **AI-Agent Skills:** Historisch 14 Skills unter `.agents/skills/`. Der Ordner wurde beim Umzug 2026-09 aus dem Repo entfernt (gewollt) — die folgende Tabelle dient als Referenz; bei Bedarf aus der Git-Historie reaktivieren (`git checkout HEAD -- .agents/skills/`).

Fourteen skills orchestrate mod work (8 + 5 on 2026-08-21 + 1 S1MCP on 2026-08-22). **Load them via the `skill` tool** when the task matches:

| Skill | When to use |
|---|---|
| **`schedule1-modding`** | Mod runbook: scaffold, build, deploy, architecture patterns, S1API/UI/Harmony. **Primary skill, always first.** |
| **`schedule1-phoneapp`** | PhoneApp runbook: S1API PhoneApp development, Method 3 responsive UI, input focus protection, lifecycle stability (Rule 10/11). |
| **`schedule1-grid`** | Grid & building: outdoor/unrestricted placement, BuildUpdate_Grid patching, ghost positioning, 7 Golden Rules, custom building. |
| **`schedule1-s1api`** | S1API framework reference: Saveables, PhoneApp base, Quests, NPCs, Items, Money, GameTime, Lifecycle, cross-branch compatibility. |
| **`schedule1-s1mapi`** | S1MAPI framework reference: ProceduralMesh, BuildingBuilder, GltfLoader, InteriorBuilder, World tools (Terrain/Nav/Prefab). |
| `schedule1-knowledge` | Research: efficiently use 4804 files / 304+ analyses, read decompiles, understand system architecture. |
| `schedule1-troubleshooting` | Diagnostics: `logscan.py`, crash patterns, save-load timing, IL2CPP pitfalls (11 fragile areas). |
| `schedule1-game-systems` | Game systems: 64 systems (Growing 08, Inventory 09, Property 54, etc.), decision tree, recipes — bypass raw decompile. |
| **`schedule1-economy`** | Economy: Money (cash/bank), Business revenue, Shop multi-payment (Cash/Bank/Auto), Customers, Laundering — host authority + snapshot revert. |
| **`schedule1-persistence`** | Persistence: SafeStorage atomic .bak, slot_{n}.json + TryMigrateLegacy, GameLifecycle timing, ModConfig TOML sidecar. |
| **`schedule1-items`** | Items: BaseItemDefinition/Registry/StackLimit (dual-scan + not patchable accessor), Inventory slots (CashSlot 1k), Buildable injection. |
| **`schedule1-interiors`** | Interiors & Minigames: Door hooking (StaticDoor/NpcSummonMenu), Procedural room shells (binary/layout mesh reader), In-world screens (Texture2D.SetPixels32), 3D spatial ambience. |
| **`schedule1-3d-assets`** | 3D Assets & Blender: Blender pipeline (Z-Up to Y-Up, Apply Transforms, Recalculate Normals), URP shader resolution (Lit/Unlit pink shader fix), PBR materials, bone rigging (Spine2/Head/Hands), zero-collider rule. |
| **`schedule1-mcp`** | S1MCP & Live Debugging: Live game introspection, TCP :8765 bridge, in-game log capturing, player/NPC state inspection, object reflection, item spawning. |

Skill paths: `.agents/skills/<skill-name>/SKILL.md` (plus `references/` sub-files). Standard content: SKILL.md (YAML frontmatter + decision tree + references), `references/*.md` for sub-topics. **Do not open manually — load via the `skill` tool.**

---

## 1. Setup

- **Game:** `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` (override via `$env:SCHEDULE1_PATH`)
- **Game Version:** v0.4.6f13 (Unity 2022.3, IL2CPP — default Steam branch)
- **Mod Loader:** MelonLoader 0.7.3 (net6)
- **TFM:** `net6.0`, `LangVersion` 12, `Nullable` enabled
- **Layout:**
  ```
  Source/Mods/        Mods + Shared lib (inkl. Shared/UITheme) + Directory.Build.props/targets + S1Mods.sln
  Source/Archive/     Archived mods (DayCounter, ProfitTracker, TVBrowser), Tests & reference decompiles
  ThirdParty/         External frameworks & mod sources (Sideload, hash, MoreDrugs, S1MCP)
  Tools/              (leer nach Umzug 2026-09 — PowerShell-Helper in der Git-Historie)
  Release/            Release packages (.gitkeep)
  .agents/skills/     (entfernt beim Umzug 2026-09 — in der Git-Historie)
  .githooks/          Pre-commit hook (dotnet format + gen-sln determinism)
  .github/            CI (workflows/ci.yml, workflows/release.yml) + Issue/PR templates
   memory/             MEMORY.md (Daily-Logs + memory-protocol.md in der Git-Historie)
   AGENTS.md           Agent bootstrap & inventory
   CONTRIBUTING.md     Contributor guide
   DEVELOPERS.md       Developer documentation
   LICENSE             MIT (workspace) + Third-Party notices
  ```

---

## 2. Mod Inventory (Stand 2026-08-23 — Deploy-Status siehe Hinweis)

> **Status nach Neuinstallation (2026-09-04):** Setup wiederhergestellt — .NET SDK 8.0.424 installiert, S1API 3.2.0 aus `ThirdParty/S1API/` gebaut und deployed (`Mods\S1API.Il2Cpp.MelonLoader.dll` + `Plugins\S1APILoader.dll`), NotesApp + Shared als Verifikation gebaut. Die übrigen Mods sind **noch nicht** redeployed (Stand 2026-09-04) — einfach `dotnet build` je Mod laufen lassen. S1API-Rebuild: `local.build.props` (Kopie von `example.build.props`) liegt bereit; der Multi-TFM-Loader-Build wirft einen harmlosen MSB3030-Copy-Fehler beim net6.0-Durchlauf — die korrekte netstandard2.1-DLL wird trotzdem deployt. **Deploy-Konvention seit 2026-09:** DLL + Icons/Bundles → `Mods\`; `mod.json` + `<Mod>.pdb` → `UserData\<Mod>\` — json/pdb gehören **nicht** nach `Mods\`.

| Mod              | Status          | Path in `Source/`      | Path in `<Game>\Mods\`              | S1API? |
|------------------|-----------------|------------------------|-------------------------------------|--------|
| **NotesApp**     | ✅ active (v1.0.0, **verified 2026-08-14 / v0.4.6f13**) | `Mods/NotesApp/` | `NotesApp.dll` + Icon | yes (PhoneApp + UIFactory + InputFocus hook + SafeStorage) |
| **PotScanner**   | ✅ active (v0.5.0, **verified 2026-08-14 / v0.4.6f13**) | `Mods/PotScanner/` | `PotScanner.dll` | yes (PhoneApp + Property + Growing + Money + Lifecycle APIs + ModConfig + Console/BaseConsoleCommand) |
| **CalculatorApp**| ✅ active (v0.2.0, **verified 2026-08-14 / v0.4.6f13**) | `Mods/CalculatorApp/` | `CalculatorApp.dll` | yes (PhoneApp + Money + UIFactory + InputFocus hook + SafeStorage) |
| **CustomSkateboard**| ✅ active (v1.0.2, **verified 2026-08-20 / v0.4.6f13**) | `Mods/CustomSkateboard/` | `CustomSkateboard.dll` + Icon | yes (Skating/Skateboard + Ultra Carving + Instant Jump + Anti-Gravel + Jeff Dialogue + Nexus Ready) |
| **MoreSaveSlots**| ✅ active (v1.0.1, **verified 2026-08-14 / v0.4.6f13**) | `Mods/MoreSaveSlots/` | `MoreSaveSlots.dll` | no (MelonMod + Harmony) |
| **DayCounter**      | ⏸ archived (v1.0.0) | `Archive/DayCounter/` | — (removed from MelonLoader) | no (MelonMod + uGUI Screen HUD + TextMeshPro + S1API/Hash Console + ModConfig) |
| **PocketShop** | ✅ active (v0.2.1, **verified 2026-08-17 / v0.4.6f13**) | `Mods/PocketShop/` | `PocketShop.dll` | yes (PhoneApp + Multi-Payment (Cash/Bank/Auto) + ItemDetailModal + SFX) |
| **BankApp**    | ✅ active (v0.3.0, **verified 2026-09-09 / v0.4.6f13**) | `Mods/BankApp/`    | `BankApp.dll` + Icon | yes (PhoneApp + Chip-Based Single-Screen UI + Weekly Limit Progress + Double-Entry Booking + Slot-Awareness + Save-Slot Isolation) |
| **HomelessMod** | ✅ active (v0.1.1, **verified 2026-08-17 / v0.4.6f13**) | `Mods/HomelessMod/` | `HomelessMod.dll` | yes (Street Nomad + Everywhere Building + 3D Procedural Sleeping Bag + Quests + Console + Save-Slot Isolation) |
| **BusinessIncome** | ✅ active (v0.1.0, **verified 2026-08-17 / v0.4.6f13**) | `Mods/BusinessIncome/` | `BusinessIncome.dll` | yes (Daily Passive Revenue + Multiplayer Host Authority + Slot Idempotency + Deterministic Variance + Console Dashboard) |
| **Minimap**        | ✅ active (v1.0.1, **verified 2026-08-18 / v0.4.6f13**) | `Mods/Minimap/` | `Minimap.dll` | yes (Minimap & Unified HUD + Dual-Shape Circle/Square + Pooled Blips + DayCounter Merged + Drag-and-Drop) |
| **ProfitTracker**   | ⏸ archived (v1.1.0) | `Archive/ProfitTracker/` | — (removed from MelonLoader) | no (MelonMod + uGUI Screen HUD + TextMeshPro + S1API/Hash Console + SafeStorage) |
| **TVBrowser**       | ⏸ archived (v0.1.0) | `Archive/TVBrowser/`   | — (removed from MelonLoader) | yes (TVApp + BrowserNavBar + InputFocus + UwbBridge + ModConfig) |
| **MikuPlayerModel** | ⏸ disabled (discontinued) | `Archive/MikuPlayerModel/` | `MikuPlayerModel.dll.bak` | yes (Player Avatar Injection + Verlet Hair Spring Physics + Console) |
| **S1MCP**        | ✅ active (v1.0.1, IL2CPP net6, freeze fix, debug-logging opt-in, **verified 2026-08-22**) | `ThirdParty/S1MCPServer-master/` | `S1MCPServer-IL2CPP.dll` | yes (MCP Protocol / TCP Server :8765 / Live Game State / Log Inspection) |
| Construction Site | ⏸ disabled     | —                      | `ConstructionSiteProperty.dll.bak`   | no   |
| **MoreDrugs 1.0.2** | ✅ active (ifBars rewrite, S1API Save-Provider, **verified 2026-08-04**) | `ThirdParty/MoreDrugs/` | `DrugExpansion_Il2cpp.dll` (10.5 MB) + .bak | yes (S1API 3.1.7+ Save-Provider APIs verified) |
| **StackLimitMod** | ✅ active (v0.1.0, **verified 2026-08-20 / v0.4.6f13**) | `Mods/StackLimitMod/` | `StackLimitMod.dll` | yes (BaseItemDefinition + Registry + BaseItemInstance Harmony Patches + Console + SafeStorage) |
| **BackpackMod**   | ✅ active (v1.0.0, **verified 2026-08-21 / v1.0.0f13**) | `Mods/BackpackMod/` | `BackpackMod.dll` + .bundle | yes (3D Wearable Backpacks + Spine Rig Alignment + Realistic Harness & Straps + ObjLoader + Storage + Mannequin 360 Rotation) |
| **AutoPackagingStation** | ✅ active (v0.2.0, **verified 2026-08-23 / v0.2.0f13**) | `Mods/AutoPackagingStation/` | `AutoPackagingStation.dll` | yes (4x4 Industrial Packaging Line + UV Scroll Conveyor + Native Slot Sync + SafeStorage + Shop Injection) |
| **Shared**       | ✅ active (workspace lib, **verified 2026-08-20**) | `Mods/Shared/` | `Shared.dll` | no (PatchGuard, SafeStorage, GameObjectResolver, SafeInvoker, HotkeyManager, ModConfig, ModLogger, NetworkGuard, SceneGate, TypeResolver, UITheme) |
| S1API 3.2.0      | ✅ active        | —                      | `S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.MelonLoader.dll` | (itself) |

### Frameworks & Tooling (not self-built, but deployed in the workspace)

| Framework       | Status     | Source                                                                  | Deploy Path (Runtime)                  | Notes |
|-----------------|------------|-------------------------------------------------------------------------|----------------------------------------|-------|
| **Sideload 1.7.0** | 📦 lokal vorhanden (undeployed) | `ThirdParty/ScheduleOne-Sideload/` (ZIP + `Sideload_extracted/` inkl. `UserLibs/`) | `Sideload.dll` → `<Game>\Mods\`, AngleSharp + Jint + Esprima → `<Game>\UserLibs\` | DooDesch phone UI framework (HTML/CSS/JS on uGUI). Deploy siehe §3. |
| **hash 1.0.5**  | 📦 lokal vorhanden (undeployed) | `ThirdParty/ScheduleOne-Hash/` (ZIP + `Hash_extracted/`)              | `Hash.dll` → `<Game>\Mods\`            | DooDesch terminal replacement for the dev console: tab-completion, `#` shorthand, history, help, plugin API via `HashCommands.Add(...)`. Benötigt Sideload. |
| S1API 3.2.0      | ✅ active (Fork-Build, deployed 2026-09-04) | `ThirdParty/S1API/` (Source + `local.build.props`)               | `S1API.Il2Cpp.MelonLoader.dll` + `Plugins\S1APILoader.dll` | (itself — see §4) |

---

### Mod Details (current status)

**NotesApp v1.0.0 (2026-08-14, verified):**
- **SafeStorage Persistence:** Protects `notes.json` via `S1Mods.Shared.SafeStorage.SaveAtomic` (atomic write + `.bak` backup) with automatic crash recovery.
- **Real-Time Search Bar:** Search field (`🔍 Search notes...`) above the list for fast title and full-text filtering.
- **Pinning / Favorites (📌):** Notes can be pinned (`IsPinned`), receive a prominent amber accent stripe, and are always sorted to the top.
- **Detail Actions (4 buttons):** `[Edit]`, `[Copy]` (copies the note content to the Windows clipboard via `GUIUtility.systemCopyBuffer`), `[Clone]` (duplicates the note), and `[Delete]` (with confirmation modal).
- **In-Game Quick-Stamp:** Header button `[ 🕒 Stamp ]` in the editor inserts the in-game day tag, weekday, and time (e.g. `[Day 14 (Wednesday), 15:30]`) with a single click.
- **Power-User Shortcuts:** <kbd>Ctrl+S</kbd> (save), <kbd>Escape</kbd> (back), <kbd>Tab</kbd> (title/text focus swap), <kbd>Ctrl+N</kbd> (new note), <kbd>Ctrl+F</kbd> (search).
- **Method 3 Responsive UI & Input Focus:** Dynamic canvas scaling via `UITheme.Sp/Dp` and the focus hook `NotesAppInputFocus` for typing without character movement.

**PotScanner v0.5.0 (2026-08-14, verified):**
- **Quick Filter Tabs Toolbar:** 4 interactive filter pills (`[All]`, `[💧 Thirsty]`, `[★ Ready]`, `[⬡ Empty]`) above the list for filtering by thirst, ripeness, or emptiness, with dynamic group badges and auto-hide for empty groups.
- **Plant Quality Rating Display:** Shows the plant quality (`Q:{pct}%`) directly in the pot row header for growing and ready plants.
- **Single-Property Focus Mode (Accordion/Focus View):** Tapping a property (e.g. "Barn") hides all other properties so only that property's pots occupy the phone screen. Tapping the header again restores the full overview.
- **Compact Responsive UI (Method 3):** Dynamic `UITheme` engine with a damped scale factor (`Mathf.Clamp(ActualHeight / 900f, 0.75f, 1.20f)`), prevents oversized tiles on high-DPI screens.
- **Slim Dimensions & 0-Allocation:** Slim top buttons (`Dp(36f)`), filter pills (`Dp(26f)`), compact property headers (`Dp(30f)`), space-saving pot rows (`Dp(46f)`). Non-allocating in-place polling loop (0 GC allocs every 2s).
- **Dynamic Single-Water Click:** Clicking the water bar triggers `WaterSinglePot` at any time, even for a pot that started wet and later dried out.
- **Status Indicators & Features:** "All Moist" indicator for watered pots, 2s polling of all `GrowContainer`, "Water All" action button with 30% skip threshold, "Auto-Water" polling service (50g/pot, persisted via `ModConfig<PotScannerConfig>`), `pot` console commands (S1API + hash bridge).

**CalculatorApp v0.2.0 (2026-08-14, verified):**
- **Exact Decimal Arithmetic:** High-precision `decimal` math engine eliminates floating-point rounding errors for financial, drug yield, and batch calculations.
- **In-Game Money Integration:** `[ 💵 Cash ]` & `[ 💳 Bank ]` quick-insert chips pull live player balances into the calculator via `S1API.Money.Money`.
- **Clipboard Integration:** `[ 📋 Copy ]` chip and <kbd>Ctrl+C</kbd> copy the displayed number to the Windows clipboard; <kbd>Ctrl+V</kbd> parses and pastes numbers into the calculator.
- **Scientific Utilities & Repeating Equals:** Square root (`√`), dynamic `C` (Clear Entry) / `AC` (All Clear), and repeated `=` execution.
- **Searchable History with Per-Item Actions:** Real-time search filter, tap-to-restore, per-item `[ 📋 ]` (Copy) and `[ 🗑️ ]` (Delete) buttons, and "Clear All History".
- **IL2CPP Input-Focus Protection:** `CalculatorAppInputFocus` prevents character movement (WASD) and game shortcut triggers while searching history.
- **SafeStorage Persistence:** Atomic JSON persistence (`UserData/CalculatorApp/calculator_state.json`) with `.bak` backup protection.
- **Physical Keyboard Support:** Direct numpad/keyboard keys, <kbd>Ctrl+C</kbd>, <kbd>Ctrl+V</kbd>, <kbd>Tab</kbd> / <kbd>Ctrl+H</kbd>.

**CustomSkateboard v1.0.2 (2026-08-20, verified):**
- **Bugfix-Round (20 issues, 6 HIGH / 9 MEDIUM / 5 LOW)**: scene-callback symmetry, per-renderer Material leak (`sharedMaterial`), cached `AnimationCurve`/`Gradient` (no GC churn), `InitializeAssets` re-init fix, candidate-base-ID logging + override config, TOCTOU collapse, exact mesh-name match, light shadows/renderMode fix, `EnableKeyword` gating, log-spam gating via `logStats`, lazy `Mod.Log`, `OrdinalIgnoreCase` dialogue compare, `BoardLeanRate` config-exposed, config-fail → `Log.Error`.
- **New configs**: `BaseItemIdOverride` (default empty), `BoardLeanRate` (default 60f).
- **Avatar Protection & Whitelist Visuals:** Strict renderer filtering and targeted MeshFilter swap on the deck prevent any visual impairment of the player avatar (hair, eyes, clothing stay untouched).
- **Ultra-Responsive Carving & Instant-Jump:** Scalar values (`TurnForce: 15.0`, `TurnChangeRate: 64.0`, `TurnReturnToRestRate: 56.0`, `LateralFriction: 1.60`), clean jump duration (`JumpDuration_Min: 0.38s`, `JumpDuration_Max: 0.58s`) and native gravity landing.
- **High-Speed Push:** `AnimationCurve` cached as `static readonly` for progressive acceleration up to 100+ km/h without speed decay.
- **Anti-Gravel & Suspension:** Prevents slowdown on grass/gravel and uneven terrain via Harmony patches (`GetSurfaceSmoothness`, `IsOnTerrain`).
- **Store Injection & Console:** Dynamic dialog injection for Jeff Gilmore plus console commands `skate` (give, stats, help).
- **Strict Vanilla Isolation & Camera:** Pure runtime instance tuning at `OnMount` protects all vanilla boards (Golden Skateboard, Cruiser, etc.) from overwrites and preserves the native `SkateboardCamera` follow.

**MoreSaveSlots v1.0.1 (2026-08-14, verified):**
- **Expand Save Slots:** Expands game save slots from vanilla 5 up to 25+ slots (default 25, configurable in `UserData/MoreSaveSlots/config.json`).
- **Paginated Navigation:** Native `[ ◄ PREV ]` / `[ NEXT ► ]` navigation bar in `ContinueScreen`, `NewGameScreen`, and `ImportScreen`.
- **Save Game Renaming:** Inline save rename modal dialog with backdrop dismiss, canvas parenting, and `Game.json.bak` auto-backup before writing.
- **Configurable Hotkeys & Wheel:** Custom string keybindings in `config.json` (`PrevPageKey`, `NextPageKey`, `RenameKey`), plus mouse wheel scrolling for flipping pages.
- **Fast Caching & Focus UX:** `SlotCardCache` UI hierarchy caching, direct screen references, auto-jump to `LastPlayedGame` page on `ContinueScreen` open, and active slot highlight.
- **Harmony Patches & Scene Return Resilience:** Transparent remapping on `SaveDisplay.Refresh`, `SaveDisplay.Awake`, `SaveDisplay.SetDisplayedSave`, `ContinueScreen.LoadGame`, `NewGameScreen.SlotSelected`, `MenuScreen.OnOpen`/`OnClose`, and `SaveManager.Awake`. Skips native out-of-bounds loops when returning to Main Menu.


**PocketShop v0.2.1 (2026-08-17, verified):**
- **Multi-Payment Switcher & Bank Integration:** Interactive chips in the sub-header (`[💵 Cash]`, `[💳 Bank Card]`, `[⚡ Auto]`). Supports physical cash (`ChangeCashBalance`) and online bank transfers (`CreateOnlineTransaction`). The `Auto` mode prefers cash and seamlessly falls back to the bank account when cash is insufficient.
- **Item Detail & Inspection Modal (`ItemDetailModal`):** Click on an item icon or name opens a high-resolution detail view with full price breakdown (base price, service fee, total), merchant source, stock count, and quick-quantity chips (`[+1]`, `[+5]`, `[+10]`, `[MAX]`) for fast bulk purchase.
- **Clean Shopping (No Disruptive Overlays):** Purchase confirmations complete without blocking banner overlays or fullscreen animations.
- **Native & Procedural Audio Feedback (`SoundService`):** Plays the game's real cash register chime (`MoneyManager.Instance.PlayCashSound()`) on successful purchases, soft clicks on quantity changes, and alarm tones on rejected transactions. Enable/disable via `PocketShopConfig.EnableSoundEffects`.
- **2-Level Navigation:** 4×3 grid with themed vector icons and merchant portraits (`StoreCatalogPane`) plus a 5×N item grid (`ItemGridPane`) with synchronized stock count.

**HomelessMod v0.1.1 (2026-08-17, verified):**
- **Everywhere Building & Collision Protection:** Unrestricted placement of objects across the entire game area (streets, parks, alleys) via Harmony patches on `BuildUpdate_Grid`. Bounding-box collision check against walls and 4-corner floor stability check. Synchronized ghost rotation.
- **Procedural 3D Sleeping Bag:** Self-contained procedural mesh & texture/icon without external asset bundles. Interactions: [E] Sleep (18:00–06:00 or anytime per config), [F] / [Hold RMB] Pack-Up back into inventory.
- **Outdoor Item Dismantling:** Generic outdoor objects (pots, tables, racks) can be dismantled via [F]/Hold RMB using `OutdoorItemInteractable` (IL2CPP-registered).
- **Savegame Slot Isolation & SafeStorage:** Slot-specific persistence (`street_items_slot_{slotId}.json`) with `.bak` crash protection and automatic migration from legacy saves.
- **Zero-Allocation OnGUI:** Statically cached `GUIStyle` instances and dynamic screen scaling (`scale = Mathf.Clamp(Screen.height / 900f, 0.80f, 1.25f)`) prevent GC allocations in IMGUI.
- **Street Nomad Questline:** 3 staged quests (*Cold Concrete*, *Alley Operations*, *Street Sovereign*) via S1API Quests.
- **Console Integration:** `homeless bag`, `homeless start`, `homeless stats`, `homeless help`.

**BankApp v0.3.0 (2026-09-09):**
- **Mockup-Based Redesign** (`docs/mockup-target-v0.3.0.png`): Weekly progress bar ($10k ATM limit), two-column balances (Cash | Online, teal), chip grid 2×5 ($1–$1000 + ✕ CLEAR + MAX accent), ⬇DEPOSIT/⬆WITHDRAW mode tabs, single full-width confirm button (green/orange by mode).
- **Chip Interaction:** Chips ADD to the running amount; MAX fills mode-dependent maximum (max depositable cash vs. max withdrawable); CLEAR resets.
- **Removed:** Free-text amount input + `BankAppInputFocus` (no typing in UI → WASD protection obsolete; IL2Cpp registration removed).
- **Unchanged Backend:** Double-entry transactions (rollback on failure), slot capacity, weekly limit, slot-isolated persistence, audio.

**BusinessIncome v0.1.0 (2026-08-17, verified):**
- **Daily Passive Revenue:** Generates daily passive income for all owned businesses (`Business.OwnedBusinesses`) via online bank transfer through `S1API.Money.Money.CreateOnlineTransaction`.
- **Multiplayer Host Authority:** Strict server/host guard (`IncomeEngine.IsHostOrSingleplayer()`) prevents duplicate payouts and money duplication on client instances.
- **Savegame Slot Idempotency & SafeStorage:** Slot-isolated persistence (`payout_state_slot_{slotId}.json`) with atomic write and `.bak` crash protection. Idempotent `LastPaidElapsedDay` marker guards against double-booking across scene reloads and save reloads.
- **Deterministic Revenue Model:** Pseudo-random variance (±15%) computed via hash from `(ElapsedDays, BusinessId)`. Employee bonus (+5%/employee, cap 25%), weekend bonus (+25% for bars/nightclubs), and configurable operating costs (10%).
- **HUD & Audio Feedback:** In-game HUD notification via `NotificationsManager.Instance.SendNotification` with cash register chime.
- **Console & Terminal Dashboard:** Full dev-console (`~` / `F1`) and DooDesch `hash` terminal (`#`) integration (`biz stats`, `biz trigger [--commit]`, `biz config`, `biz set <key> <val>`, `biz help`).

**Minimap v1.0.0 (2026-08-18, verified):**
- **Dual-Shape Viewport (Radar vs Tactical):** Dynamic uGUI mask switch between **Circular Radar** (with rotating compass ring N/E/S/W) and **Rounded-Square Tactical GPS** via procedural anti-aliasing.
- **Integrated DayCounter HUD:** Seamless integration of day (`DAY 14`), weekday (`Wednesday`), and clock (`15:30`) as a compact header or footer bar with emerald accent stripe and 0-allocation state caching.
- **Dynamic Rotation & Zoom:** Two rotation modes (`FollowPlayer` with rotating map vs. `NorthUp` with rotating player arrow) plus smooth live zoom (0.75×–4.0× via <kbd>[</kbd> / <kbd>]</kbd> / numpad).
- **0-Allocation Pooled Blip Engine:** Pre-allocated pool for 64 blips with fast scene scan (police, dealers/shops, customers, properties/trailers, quests) and shape-aware edge clamping at the minimap border.
- **Drag & Drop & Screen Bounds:** Free mouse drag while menus/pause/phone are open with automatic screen clamping and SafeStorage persistence (`UserData/Minimap/config.json`).
- **Console & Terminal Integration:** Dev console and DooDesch `hash` terminal (`minimap toggle`, `minimap shape`, `minimap zoom`, `minimap pos`, `minimap blips`, `minimap day`, `minimap help`).



**StackLimitMod v0.1.0 (2026-08-20, verified):**
- **Configurable Global Stack Limit:** Dynamically overrides item stack limits (1–9999, default 40) across player inventories, storage, shelves, and trunks.
- **Dual-Layer Item Discovery:** Automatically scans in-memory `BaseItemDefinition` assets via `Resources.FindObjectsOfTypeAll` and dynamic registry items via `Registry.Instance.GetAllItems()`.
- **Runtime Hook & Safety Patches:** Harmony postfix on `Registry.AddToRegistry` to automatically apply rules to newly registered items, plus instance-level patch on `BaseItemInstance.get_StackLimit` secured via `PatchGuard`.
- **Exclusions & Non-Stackable Control:** Configurable `OverrideNonStackable` toggle and `ExcludedItemIds` whitelist/blacklist.
- **In-Game Console & Terminal Integration:** Dev console (`~`) and DooDesch `hash` terminal (`#`) commands: `stack stats`, `stack set <amount>`, `stack reload`, `stack help` (with `stacklimit` alias).
- **SafeStorage Persistence:** Atomic JSON persistence (`UserData/StackLimitMod/config.json`) with automatic `.bak` recovery.

**BackpackMod v1.0.0 (2026-08-21, verified):**
- **3D Wearable Backpacks & Realistic Harness:** Custom 3D backpack mesh system with main body, front zipper pocket, side utility pockets, top grab handle, shoulder straps over the shoulders, chest sternum cross-strap, and metallic buckles.
- **Tailored Proportions:** Specifically scaled and fitted to the stylized, slender Schedule I player avatar between the shoulder blades without rising above the hair or clipping arms.
- **Strict Slot-10 Equipment Binding:** Only renders on the player back and Character menu mannequin when actively equipped in `ClothingSlot 10` (`BackpackSlot`); no inventory leak when items are in hotbar/pockets.
- **Direct Blender `.obj` Runtime Support (`ObjLoader`):** Zero-dependency Wavefront OBJ parser dynamically loads custom `.obj` files from `UserData/BackpackMod/models/` (e.g. `backpack.obj`, `tier1.obj`, `tier2.obj`, `tier3.obj`).
- **Tier-Based Storage Scaling:** Opens dedicated backpack storage via hotkey <kbd>B</kbd> scaling by equipped tier (Tier 1 Small: +4 slots, Tier 2 Hiking: +8 slots, Tier 3 Tactical: +16 slots).
- **360° Mannequin Rotation:** Hold Right Mouse Button and drag (or <kbd>Q</kbd> / <kbd>E</kbd>) in Character menu (<kbd>Tab</kbd>) to freely inspect front straps and rear backpack.
- **URP Shading & Zero-Collider Safety:** Universal Render Pipeline Lit shader integration with metallic/smoothness parameters; auto-strips colliders to prevent UI click-blocking.

**AutoPackagingStation v0.2.0 (2026-08-22, verified):**
- **4x4 Industrial Automated Packaging:** Instantiates a massive 4x4 animated machine with conveyor belt, pneumatic press, and status LEDs.
- **Atomic 2-Phase Engine:** Deducts inputs (raw product + packaging material) in the exact same frame as output insertion, preventing TOCTOU duplication exploits.
- **Weighted Quality Mixing:** Automatically blends input product quality using weighted average based on item quantities.
- **SafeStorage Slot-Isolation:** Stores station states separately per save slot under `autopack_slot_{slotId}.json` with `.bak` safety backups.
- **Strict Raycast & Key Guards:** Fixes AOE dismantling using strict raycast target validation and blocks key input whenever cursor is unlocked (UIs open).
- **Multiplayer & IL2CPP Stability:** Uses `IsServer` checks to prevent client desyncs and implements native `Il2CppType.Of` component setups to avoid crashes.

---


### Archived Mods (`Source/Archive/`)

**DayCounter v1.0.0 (archived):**
- **Description:** On-screen HUD & clock overlay with 2 layout modes, real-time drag & drop, 4 anchor presets, and full dev-console & hash-terminal integration.
- **Archive path:** `Source/Archive/DayCounter/`

**ProfitTracker v1.1.0 (archived):**
- **Description:** Compact top-right HUD with net-worth tracking (cash, bank, business assets), rolling 7-day history, and in-game commands.
- **Archive path:** `Source/Archive/ProfitTracker/`

**TVBrowser v0.1.0 (archived):**
- **Description:** In-game TV web & YouTube app for property TVs with viewport rendering and navigation bar.
- **Archive path:** `Source/Archive/TVBrowser/`

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

# Regenerate the solution (after adding a new mod!)
pwsh Tools\gen-sln.ps1

# Scaffold a new mod
pwsh Tools\new-mod.ps1 -Name <ModName>
```

> **Hinweis:** `Tools\` ist nach dem Umzug 2026-09 leer — die PowerShell-Helper (build-all, gen-sln, new-mod, bump-version, package-release) liegen in der Git-Historie (`git checkout HEAD -- Tools/`). Direkt nutzbar bleibt `dotnet build` (siehe oben).

Deployment läuft **automatisch** via `Directory.Build.targets` — mit getrennten Zielen (Konvention seit Reinstall 2026-09):

- `<GameDir>\Mods\`: DLL + PNGs + Bundles (nur von MelonLoader ladbare Dateien)
- `<GameDir>\UserData\<Mod>\`: `mod.json` (Metadaten) + `<Mod>.pdb` (portable Debug-Symbole via `<DebugType>portable</DebugType>`)

**JSON und PDB gehören niemals nach `Mods\`.**

### ⚠️ Important Build Caveats

- **SkipUnchangedFiles:** Fixed to `false` (2026-08-20) — every `dotnet build` now force-deploys to `<GameDir>\Mods\`, eliminating stale-DLL traps.
- **GameDir Override:** `$env:SCHEDULE1_PATH` must be set **before** running `dotnet build`, because MSBuild evaluates the property once at startup.
- **Solution Determinism:** `Tools/gen-sln.ps1` now uses deterministic MD5-GUIDs (`Get-DeterministicGuid "Project:<rel>"`) — no more random GUID diffs on every regeneration.
- **Version Bumps:** Use `pwsh Tools/bump-version.ps1 -Mod <Name> -Version x.y.z` to sync `Mod.cs` + `mod.json` + `CHANGELOG.md` + `AGENTS.md` atomically.

### Sideload + hash (ThirdParty, NICHT in der Solution)

Sideload (DooDesch) und hash (DooDesch) liegen nach dem Umzug 2026-09 als **fertige Release-Pakete** unter `ThirdParty/` vor (ZIP + extrahiert, kein Source/Build mehr): `ScheduleOne-Sideload/Sideload_extracted/` (Sideload 1.7.0, inkl. `UserLibs/`-Abhängigkeiten) und `ScheduleOne-Hash/Hash_extracted/` (hash 1.0.5). Beide sind **nicht** Teil von `S1Mods.sln`.

**Deploy (manuell, bei Bedarf):**
```pwsh
# Sideload: DLL nach Mods\, Abhängigkeiten nach UserLibs\
Copy-Item "ThirdParty\ScheduleOne-Sideload\Sideload_extracted\Sideload.dll" "..\Mods\"
Copy-Item "ThirdParty\ScheduleOne-Sideload\Sideload_extracted\UserLibs\*" "..\UserLibs\" -Recurse -Force

# hash: DLL nach Mods\ (benötigt Sideload)
Copy-Item "ThirdParty\ScheduleOne-Hash\Hash_extracted\Hash.dll" "..\Mods\"
```

---

## 4. IL2CPP vs Mono (Alternate Branch) — Architecture Decision

**Decision: We stay firmly on IL2CPP (default Steam release).**

- **Why?**
  1. The workspace is already fully set up and stable for IL2CPP with MelonLoader 0.7.3, S1API 3.2.0, `Directory.Build.props`, and `UITheme`.
  2. Mods run natively for all regular Steam players (95%+), without requiring users to manually switch to the `alternate` beta branch.
  3. All common IL2CPP hurdles (`IntPtr` constructors, canvas DPI scaling, lifecycle timing) are solved in our mods and documented as reusable patterns.

---

## 5. Mod Conventions & Best Practices

### Responsive UI Design (Method 3: `UITheme`)

In Schedule I, S1API instantiates the phone container on a high-DPI uGUI canvas that is often rotated by 90° (`Quaternion.Euler(0, 0, 90)`). Fixed integer font sizes (e.g. 14pt–18pt) appear extremely tiny on some screens.

**Central implementation:** `Source/Mods/Shared/src/UITheme.cs` (`S1Mods.Shared.UITheme`) is the single source of truth. Mod-specific `UI/UITheme.cs` wrappers (e.g. `BankApp.UI.UITheme`, `PocketShop.UI.UITheme`) delegate to it for `Scale/Sp/Dp` while keeping their own color palettes. New mods should directly use `S1Mods.Shared.UITheme`.

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
s1interop doctor --mono-game-path $env:SCHEDULE1_PATH --il2cpp-game-path $env:SCHEDULE1_PATH
s1interop analyze "C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule1-mod-only-main\Source\Mods\<Name>\src\<Name>.csproj"
```

### Mod Update Cycle

1. Edit code in `Source/Mods/<Name>/src/`
2. `dotnet build Source\Mods\<Name>\src\<Name>.csproj -c Release` (or `pwsh Tools/build-all.ps1`)
3. DLL is automatically deployed to `<Game>\Mods\` (force, no stale-DLL)
4. Launch the game & check `Latest.log`
5. For version bumps: `pwsh Tools/bump-version.ps1 -Mod <Name> -Version x.y.z` (syncs 4 files)

### CI & Quality Gates

- **Local:** `dotnet format --verify-no-changes`, `pwsh Tools/gen-sln.ps1` (determinism check), `dotnet test Source/Archive/Tests/Shared.Tests/Shared.Tests.csproj`
- **CI:** `.github/workflows/ci.yml` runs on push/PR (format + build + tests + gen-sln check + Knowledge path check)
- **Pre-commit:** `git config core.hooksPath .githooks` enables `.githooks/pre-commit` (format + gen-sln + XRef)
- **License:** `LICENSE` (MIT workspace + Third-Party notices), `CONTRIBUTING.md` for contributors

### Maker-Checker Workflow

1. **Pre-Task:** Load relevant `.agents/skills/` to identify architectural constraints and known gotchas.
2. **Execution (Maker-Checker):** Strictly adhere to architectural pillars and 7 Golden Rules. Use separated coder/verifier subagents for code generation and audit.
3. **Post-Task (Proactive Learning):**
   - Proactively update relevant skills in `.agents/skills/`.
   - Update `AGENTS.md` and mod `CHANGELOG.md` if inventory, versions, or dependencies change.

---

## 7. Reference Material

> **Note:** This mod-only workspace does not include the `Knowledge/` folder (decompiles, analyses, maps, tools). Der frühere Haupt-Workspace `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule 1 Modding` existiert nach der Neuinstallation (2026-09) **nicht mehr** — die Knowledge-Base (4804 Dateien / 67 MB) muss bei Bedarf neu generiert (AssetRipper/ilspycmd) oder aus einem Backup wiederhergestellt werden.

Reference decompiles and analysis snippets for archived mods are available in `Source/Archive/`.

