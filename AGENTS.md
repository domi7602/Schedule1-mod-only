# AGENTS.md

Workspace: `C:\Users\pc\Schedule1-mod-only` — MelonLoader modding workspace for *Schedule I* v0.4.6f13 (TVGS). The repo lives **outside** the game install dir (under the user profile); build/deploy resolves the game path via the `SCHEDULE1_PATH` env var or the default fallback `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`.

Quick links: [`docs/architecture.md`](docs/architecture.md) defines dependency boundaries, [`ThirdParty/README.md`](ThirdParty/README.md) documents pinned dependencies, and [`GameReferences/README.md`](GameReferences/README.md) explains local decompile generation. This file remains the compact operational index and version inventory.

> **Pfad-Historie (3 Umzüge):**
> - **2026-09 Setup-Wiederherstellung (aktueller Stand):** Repo unter `C:\Users\pc\Schedule1-mod-only`. .NET SDK 8.0.424 installiert, S1API 3.2.0 (Fork-Build aus `ThirdParty/S1API/`) deployed, NotesApp + Shared als Verifikation gebaut. Deploy-Konvention seit 2026-09: DLL/PNG/bundle → `Mods\`; `mod.json` + `.pdb` → `UserData\<Mod>\`.
> - **Vorheriger Stand (vor Wiederherstellung):** Repo lag im Spiel-Ordner unter `<GameDir>\Schedule1-mod-only-main`. Obsolet durch die Neuinstallation.
> - **Ältester Stand:** `C:\Users\pc\Desktop\Schedule1-mod-only`. Bereits mit dem Wechsel ins Game-Dir obsolet.

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
- **Game Version:** v0.4.6f13 (Unity 2022.3, IL2CPP — default Steam branch)
- **Mod Loader:** MelonLoader 0.7.3 (net6)
- **TFM:** `net6.0`, `LangVersion` 12, `Nullable` enabled
- **Layout:**
  ```
  Source/Mods/        Mods + Shared lib (inkl. Shared/UITheme) + Directory.Build.props/targets + S1Mods.sln
  Source/Archive/     Archived mods (DayCounter, ProfitTracker, TVBrowser, BackpackMod, Minimap)
  Source/Tests/       Unit-Tests (Shared.Tests, AutoPackagingStation.Tests, CalculatorApp.Tests)
  GameReferences/     Local generated decompiles (Assembly-CSharp, firstpass)
  Skills/             20 AI-Agent Skills & References (modding, phoneapp, economy, systems, etc.)
  ThirdParty/         Pinned external frameworks & reference sources; see ThirdParty/README.md
  Tools/              (reaktiviert 2026-09-10, 11 Skripte: build-all, gen-sln, new-mod, bump-version, package-release, backup-to-d, check-version-sync, check-doc-paths, deploy-thirdparty, bootstrap-game-references, mods-cleanup-inventory)
  Release/            Release packages (.gitkeep)
  .githooks/          Pre-commit hook (dotnet format + gen-sln determinism)
  docs/               Architektur, Release-Prozess & IL2CPP-Fallen (docs/pitfalls.md)
  AGENTS.md           Agent bootstrap & inventory
  CONTRIBUTING.md     Contributor guide
  DEVELOPERS.md       Developer documentation
  LICENSE             MIT (workspace) + Third-Party notices
  ```

---

## 2. Mod Inventory (Stand 2026-09-17 — Deploy-Status siehe Hinweis)

> **Status-Konvention (2026-09-16):** `Verified <Datum>` = Funktions-Verify in einer Echtspiel-Session, nachvollziehbar über den jeweiligen Fix-/Audit-Commit und CHANGELOG-Eintrag (Assertion, **kein** Artefakt im Repo). `In-Game-Verify offen` = Code vollständig, aber noch nicht im Spiel bestätigt. Beide Stände bewusst getrennt führen.

> **Status nach Neuinstallation (2026-09-04):** Setup wiederhergestellt — .NET SDK 8.0.424 installiert, S1API 3.2.0 aus `ThirdParty/S1API/` gebaut und deployed (`Mods\S1API.Il2Cpp.MelonLoader.dll` + `Plugins\S1APILoader.dll`), NotesApp + Shared als Verifikation gebaut. Die übrigen Mods sind **noch nicht** redeployed (Stand 2026-09-04) — einfach `dotnet build` je Mod laufen lassen. S1API-Rebuild: `local.build.props` (Kopie von `example.build.props`) liegt bereit; der Multi-TFM-Loader-Build wirft einen harmlosen MSB3030-Copy-Fehler beim net6.0-Durchlauf — die korrekte netstandard2.1-DLL wird trotzdem deployt. **Deploy-Konvention seit 2026-09:** DLL + Icons/Bundles → `Mods\`; `mod.json` + `<Mod>.pdb` → `UserData\<Mod>\` — json/pdb gehören **nicht** nach `Mods\`.

| Mod              | Status          | Path in `Source/`      | Path in `<Game>\Mods\`              | S1API? |
|------------------|-----------------|------------------------|-------------------------------------|--------|
| **NotesApp**     | ✅ active (v1.0.3, **Verified 2026-09-15**) | `Mods/NotesApp/` | `NotesApp.dll` + Icon | yes (PhoneApp + UIFactory + InputFocus hook + SafeStorage) |
| **PotScanner**   | ✅ active (v0.5.4, **Verified 2026-09-15**) | `Mods/PotScanner/` | `PotScanner.dll` | yes (PhoneApp + Property + Growing + Money + Lifecycle APIs + ModConfig + Console/BaseConsoleCommand) |
| **CalculatorApp**| ✅ active (v0.2.3, **Verified 2026-09-15**) | `Mods/CalculatorApp/` | `CalculatorApp.dll` | yes (PhoneApp + Money + UIFactory + InputFocus hook + SafeStorage) |
| **CustomSkateboard**| ✅ active (v1.1.5, **Verified 2026-09-15**) | `Mods/CustomSkateboard/` | `CustomSkateboard.dll` + Icon | yes (Skating/Skateboard + Ultra Carving + Instant Jump + Anti-Gravel + Jeff Dialogue + Nexus Ready) |
| **MoreSaveSlots**| ✅ active (v1.0.12, **Verified 2026-09-15**) | `Mods/MoreSaveSlots/` | `MoreSaveSlots.dll` | no (MelonMod + Harmony) |
| **DayCounter**      | ⏸ archived (v1.0.0) | `Source/Archive/DayCounter/` | — (removed from MelonLoader) | no (MelonMod + uGUI Screen HUD + TextMeshPro + S1API/Hash Console + ModConfig) |
| **PocketShop** | ✅ active (v0.2.7, **Level-Lock & Inline-Qty 2026-09-17 / v0.4.6f13, Verified 2026-09-19**) | `Mods/PocketShop/` | `PocketShop.dll` | yes (PhoneApp + Multi-Payment (Cash/Bank/Auto) + ItemDetailModal + Inline-Quantity-Input + Level-Lock + SFX) |
| **BankApp**    | ✅ active (v0.4.4, **Verified 2026-09-15**) | `Mods/BankApp/`    | `BankApp.dll` + Icon | yes (PhoneApp + Chip-Based Single-Screen UI + Weekly Limit Progress + Double-Entry Booking + Slot-Awareness + Save-Slot Isolation) |
| **HomelessMod** | ⏸ archived (v0.1.12) | `Source/Archive/HomelessMod/` | — (removed from MelonLoader) | yes (Street Nomad + Everywhere Building + 3D Procedural Sleeping Bag + Quests + Console + Save-Slot Isolation; archiviert 2026-09-17 auf User-Wunsch) |
| **BusinessIncome** | ✅ active (v0.1.6, **0-Business Backlog-Fix 2026-09-17 / v0.4.6f13, Verified 2026-09-19**) | `Mods/BusinessIncome/` | `BusinessIncome.dll` | yes (Daily Passive Revenue + Multiplayer Host Authority + Slot Idempotency + Deterministic Variance + Console Dashboard) |
| **Minimap**        | ⏸ archived (v2.0.2) | `Source/Archive/Minimap/` | — (removed from MelonLoader) | yes (Minimap & Unified HUD + Dual-Shape Circle/Square + Pooled Blips + DayCounter Merged + Drag-and-Drop + **v2: Dealer-Marker + Heat-Ring (EPursuitLevel) + Waypoints (slot-isoliert, Console `minimap wp`) + Health-Bar + Minimap-Settings-PhoneApp (Toggles + Hex-Farben)**; archiviert 2026-09-17; User bevorzugt Spiel ohne Minimap) |
| **ProfitTracker**   | ⏸ archived (v1.1.0) | `Source/Archive/ProfitTracker/` | — (removed from MelonLoader) | no (MelonMod + uGUI Screen HUD + TextMeshPro + S1API/Hash Console + SafeStorage) |
| **TVBrowser**       | ⏸ archived (v0.1.0) | `Source/Archive/TVBrowser/`   | — (removed from MelonLoader) | yes (TVApp + BrowserNavBar + InputFocus + UwbBridge + ModConfig) |
| **MikuPlayerModel** | ⏸ disabled (discontinued) | — (Quelle nicht im Repo; gelöscht vor Initial-Commit) | `MikuPlayerModel.dll.bak` | yes (Player Avatar Injection + Verlet Hair Spring Physics + Console) |
| **S1MCP**        | ✅ active (v1.0.1, IL2CPP net6, freeze fix, debug-logging opt-in, **verified 2026-08-22**) | `ThirdParty/S1MCPServer-master/` | `S1MCPServer-IL2CPP.dll` | yes (MCP Protocol / TCP Server :8765 / Live Game State / Log Inspection) |
| Construction Site | ⏸ disabled     | —                      | `ConstructionSiteProperty.dll.bak`   | no   |
| **MoreDrugs 1.0.2** | ❌ removed (2026-09-19, **nicht redeployed nach Neuinstallation** — 3rd-party by ifBars, Quelle nicht im Repo) | — (nur deployte DLL, nicht Teil dieses Repos) | — (DLL nicht wiederhergestellt, User-Entscheidung: bleibt weg) | yes (war: S1API 3.1.7+ Save-Provider APIs) |
| **StackLimitMod** | ✅ active (v0.1.4, **Agriculture-Only Mode & Weapon-Shield 2026-09-15 / v0.4.6f13, Verified 2026-09-19**) | `Mods/StackLimitMod/` | `StackLimitMod.dll` | yes (BaseItemDefinition + Registry + BaseItemInstance Harmony Patches + Console + SafeStorage) |
| **BackpackMod**   | ⏸ archived (v1.2.3) | `Source/Archive/BackpackMod/` | — (removed from MelonLoader) | yes (3D Wearable Backpacks + Spine Rig Alignment + Realistic Harness & Straps + ObjLoader + Storage + Mannequin 360 Rotation + **B1 Sort: Button-only Sortierung (Backpack/Inventory/Container) mit Stack-Merge nach ID+Qualität+Packaging, Sort-Button im StorageMenu + „Sort Inventory" im GameplayMenu, atomarer Plan-then-Commit (v1.2.0: GetCopy statt GetDefaultInstance — Quality/Packaging preserved; Clipboard-Slot-Referenz-Filter; Overflow-Sidecar bei vollem Inventar; OnPreLoad-Cross-Save-Protect; v1.2.1: ObjLoader 50MB/250k-Vertex-Cap, ShopDump #if DEBUG; v1.2.2: HUD-Sort-Button Raycast-Schutz gegen Grid-Click-Schlucken; v1.2.3: StorageMenu-Sort-Overlay-Fix (kleinstes-Button-Template + erzwungene Geometrie via SortButtonLayout + BackpackMod.Tests)**) |
| **AutoPackagingStation** | ✅ active (v0.2.8, **Bugfix-Runde 7 2026-09-19 / v0.4.6f13, Verified 2026-09-19**) | `Mods/AutoPackagingStation/` | `AutoPackagingStation.dll` | yes (4x4 Industrial Packaging Line + UV Scroll Conveyor + Native Slot Sync + SafeStorage + Shop Injection + 2x2 Footprint + InteractableObject + Hustler-I Rank-Gate) |
| **HitmanPhone** | ✅ active (v0.2.9, **KnockOut-Hook + Single-Drop-Latch + Orphan-Cleanup, Verified 2026-09-15**) | `Mods/HitmanPhone/` | `HitmanPhone.dll` | yes (MessagesApp Contracts + NPC + Items + Quests + SafeStorage) |
| **SnackVendor** | ⏸ archived (v0.0.9) | `Source/Archive/SnackVendor/` | — (removed from MelonLoader) | yes (S1API Buildable + Vanilla-VendingMachine-Klon + marker-guarded Harmony + Sidecar-Persistenz + S1MAPI.GltfLoader-Mesh + NPC-Inventory-Credit + Deposit/Extract-Panel; archiviert 2026-09-17 auf User-Wunsch, In-Game-Verify nie erfolgt) |
| **_DiagPerfCounter** | ✅ active (v0.3.2, **Dev-Tool, dok. 2026-09-10**) | `Mods/_DiagPerfCounter/` | `_DiagPerfCounter.dll` | no (Reflection-Dump via SafeStorage-Pfad) |
| **PhoneScroll** | ❌ removed (v1.4, **retired 2026-09-16**, DLL in `UserData\_RemovedMods\`) | `ThirdParty/Archive/PhoneScroll/` | — | no (vanilla Phone-HomeScreen-Hook, 3rd-party by V4LEXL) |
| **Shared**       | ✅ active (workspace lib, **Verified 2026-09-15**) | `Mods/Shared/` | `Shared.dll` | no (PatchGuard, SafeStorage, GameObjectResolver, SafeInvoker, HotkeyManager, ModConfig, ModLogger, NetworkGuard, SceneGate, TypeResolver, UITheme) |
| S1API 3.2.0      | ✅ active        | —                      | `S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.MelonLoader.dll` | (itself) |

### Frameworks & Tooling (not self-built, but deployed in the workspace)

| Framework       | Status     | Source                                                                  | Deploy Path (Runtime)                  | Notes |
|-----------------|------------|-------------------------------------------------------------------------|----------------------------------------|-------|
| **Sideload 1.7.0** | ❌ entfernt 2026-09-10 (deprecated, ungenutzt) | — | — | DooDesch HTML/CSS/JS-UI-Framework; kein Mod im Workspace hatte es je eingebunden. |
| **hash 1.0.5**  | 📦 lokal vorhanden (undeployed, deprecated 2026-09-10) | `ThirdParty/Archive/ScheduleOne-Hash/` (ZIP + `Hash_extracted/`)              | `Hash.dll` → `<Game>\Mods\`            | DooDesch terminal replacement for the dev console. Nicht mehr genutzt — bleibt im Repo als Referenz (2026-09-16 nach `Archive/` verschoben; PotScanner/BusinessIncome Source-Includes folgen dem neuen Pfad). |
| **Herer's Minimap 2.0.1** | ❌ removed (2026-09-17) | NexusMods #899 by JackHerer1820 | — (DLL in `UserData\_RemovedMods\`) | JackHerer1820 Minimap mit integrierter MinimApp auf dem In-Game-Phone. Entfernt auf Wunsch (User spielt lieber ohne Minimap). |
| S1API 3.2.0      | ✅ active (Fork-Build, deployed 2026-09-04) | `ThirdParty/S1API/` (Source + `local.build.props`)               | `S1API.Il2Cpp.MelonLoader.dll` + `Plugins\S1APILoader.dll` | (itself — see §4) |

---

### Mod Details (current status)

**NotesApp v1.0.3 (2026-09-13, Bug-Report-Runde 6):**
- **v1.0.3 (Bug-Report-Runde 6 2026-09-13):** N1 - JSON `Title`/`Text`: `null` wird beim Laden zu `string.Empty` normalisiert (RefreshList-Crash bei hand-editierten/korrupten `notes`-Dateien behoben).
- **v1.0.2:** RefreshList Null-/WasCollected-Guard; Slot-Suffix >=0-Check (kein slot_-1-File).
- **v1.0.1:** Statischer Event-Dispatcher (kein Subscriber-Leak pro Szeneload); tote `MigrateLegacyGlobalNotes()` entfernt.
- **SafeStorage Persistence:** Protects `notes.json` via `S1Mods.Shared.SafeStorage.SaveAtomic` (atomic write + `.bak` backup) with automatic crash recovery.
- **Real-Time Search Bar:** Search field (`🔍 Search notes...`) above the list for fast title and full-text filtering.
- **Pinning / Favorites (📌):** Notes can be pinned (`IsPinned`), receive a prominent amber accent stripe, and are always sorted to the top.
- **Detail Actions (4 buttons):** `[Edit]`, `[Copy]` (copies the note content to the Windows clipboard via `GUIUtility.systemCopyBuffer`), `[Clone]` (duplicates the note), and `[Delete]` (with confirmation modal).
- **In-Game Quick-Stamp:** Header button `[ 🕒 Stamp ]` in the editor inserts the in-game day tag, weekday, and time (e.g. `[Day 14 (Wednesday), 15:30]`) with a single click.
- **Power-User Shortcuts:** <kbd>Ctrl+S</kbd> (save), <kbd>Escape</kbd> (back), <kbd>Tab</kbd> (title/text focus swap), <kbd>Ctrl+N</kbd> (new note), <kbd>Ctrl+F</kbd> (search).
- **Method 3 Responsive UI & Input Focus:** Dynamic canvas scaling via `UITheme.Sp/Dp` and the focus hook `NotesAppInputFocus` for typing without character movement.

**PotScanner v0.5.4 (2026-09-12, Audit-Patch Runde 2026-09-12):**
- **v0.5.3 (Bug-Audit 2026-09-12):** WaterAll Threshold-Vergleich nutzt jetzt den Live-Wert `c.NormalizedMoistureAmount` (vorher: gecachter `info.WaterPercent`, bis 2s alt — Skip/Charge-Mismatch bei stale Cache). Per-Charge-`cashBalance`-Re-Check vor jeder `ChangeCashBalance`-Abbuchung (break statt negativem Cash, wenn parallele Spender zwischen den Iterationen den Kontostand senken). `Constants.ModVersion` auf „0.5.3“ korrigiert.
- **v0.5.2:** WasCollected-Guards (Update/Handler/PhoneClosed); Row-Caches werden bei Unload geleert.
- **v0.5.1:** Statischer Event-Dispatcher + Per-Handler-try/catch; WaterAll/WaterSinglePot host-only (MP-Geldschutz).
- **Quick Filter Tabs Toolbar:** 4 interactive filter pills (`[All]`, `[💧 Thirsty]`, `[★ Ready]`, `[⬡ Empty]`) above the list for filtering by thirst, ripeness, or emptiness, with dynamic group badges and auto-hide for empty groups.
- **Plant Quality Rating Display:** Shows the plant quality (`Q:{pct}%`) directly in the pot row header for growing and ready plants.
- **Single-Property Focus Mode (Accordion/Focus View):** Tapping a property (e.g. "Barn") hides all other properties so only that property's pots occupy the phone screen. Tapping the header again restores the full overview.
- **Compact Responsive UI (Method 3):** Dynamic `UITheme` engine with a damped scale factor (`Mathf.Clamp(ActualHeight / 900f, 0.75f, 1.20f)`), prevents oversized tiles on high-DPI screens.
- **Slim Dimensions & 0-Allocation:** Slim top buttons (`Dp(36f)`), filter pills (`Dp(26f)`), compact property headers (`Dp(30f)`), space-saving pot rows (`Dp(46f)`). Non-allocating in-place polling loop (0 GC allocs every 2s).
- **Dynamic Single-Water Click:** Clicking the water bar triggers `WaterSinglePot` at any time, even for a pot that started wet and later dried out.
- **Status Indicators & Features:** "All Moist" indicator for watered pots, 2s polling of all `GrowContainer`, "Water All" action button with 30% skip threshold, "Auto-Water" polling service (50g/pot, persisted via `ModConfig<PotScannerConfig>`), `pot` console commands (S1API + hash bridge).

**CalculatorApp v0.2.3 (2026-09-12, Audit-Patch Runde 2026-09-12):**
- **v0.2.3 (Bug-Audit 2026-09-12):** `GetActiveSlotSuffix` jetzt mit `SaveSlotNumber >= 0`-Guard (Schliesst `calculator_state_slot_-1.json`-Pfad, den NotesApp schon hatte). `InputSquare` und `InputReciprocal` fangen `decimal.OverflowException` und setzen „Overflow“ statt unkontrolliert in den Button-Callback zu werfen (vorher: Overflow wirft aus dem Button-Handler). `HasError` erkennt `DisplayText == "Overflow"` (vorher: Eingaben hingen an `"Overflow5"`).
- **v0.2.2:** WasCollected-Guards in UpdateDisplayUI/RefreshHistoryList.
- **v0.2.1:** Statischer Event-Dispatcher; Pointer-/WasCollected-Guards im Slot-Suffix.
- **Exact Decimal Arithmetic:** High-precision `decimal` math engine eliminates floating-point rounding errors for financial, drug yield, and batch calculations.
- **In-Game Money Integration:** `[ 💵 Cash ]` & `[ 💳 Bank ]` quick-insert chips pull live player balances into the calculator via `S1API.Money.Money`.
- **Clipboard Integration:** `[ 📋 Copy ]` chip and <kbd>Ctrl+C</kbd> copy the displayed number to the Windows clipboard; <kbd>Ctrl+V</kbd> parses and pastes numbers into the calculator.
- **Scientific Utilities & Repeating Equals:** Square root (`√`), dynamic `C` (Clear Entry) / `AC` (All Clear), and repeated `=` execution.
- **Searchable History with Per-Item Actions:** Real-time search filter, tap-to-restore, per-item `[ 📋 ]` (Copy) and `[ 🗑️ ]` (Delete) buttons, and "Clear All History".
- **IL2CPP Input-Focus Protection:** `CalculatorAppInputFocus` prevents character movement (WASD) and game shortcut triggers while searching history.
- **SafeStorage Persistence:** Atomic JSON persistence (`UserData/CalculatorApp/calculator_state.json`) with `.bak` backup protection.
- **Physical Keyboard Support:** Direct numpad/keyboard keys, <kbd>Ctrl+C</kbd>, <kbd>Ctrl+V</kbd>, <kbd>Tab</kbd> / <kbd>Ctrl+H</kbd>.

**CustomSkateboard v1.1.5 (2026-09-13, Audit-Patch Runde 5 2026-09-13):**
- **v1.1.5 (Bug-Audit 2026-09-13):** Toter State entfernt — `s_disableTerrainSlowdownCached` (nur geschrieben, nie gelesen) und `ClearStyledCache()` aus `SkateboardVisualPatches`/`CyberSkateboardVisualizer` gelöscht. Kein Funktionsverlust, nur Wartung.
- **v1.1.4:** Validate/Tune-Clamp-Divergenz aufgelöst.
- **v1.1.3:** Pass-2-Deck-Heuristik `IsDeckMeshSane` ergänzt.
- **v1.1.2:** Settings-Sharing-Detektor (Error-Log bei geteiltem _settings-Objekt).
- **v1.1.0/v1.1.1:** Tuning-Release (TopSpeed 140 km/h, Push/Lenkung); `IsGameplayScene` strikt `"Main"`.
- **Bugfix-Round (20 issues, 6 HIGH / 9 MEDIUM / 5 LOW)**: scene-callback symmetry, per-renderer Material leak (`sharedMaterial`), cached `AnimationCurve`/`Gradient` (no GC churn), `InitializeAssets` re-init fix, candidate-base-ID logging + override config, TOCTOU collapse, exact mesh-name match, light shadows/renderMode fix, `EnableKeyword` gating, log-spam gating via `logStats`, lazy `Mod.Log`, `OrdinalIgnoreCase` dialogue compare, `BoardLeanRate` config-exposed, config-fail → `Log.Error`.
- **New configs**: `BaseItemIdOverride` (default empty), `BoardLeanRate` (default 60f).
- **Avatar Protection & Whitelist Visuals:** Strict renderer filtering and targeted MeshFilter swap on the deck prevent any visual impairment of the player avatar (hair, eyes, clothing stay untouched).
- **Ultra-Responsive Carving & Instant-Jump:** Scalar values (`TurnForce: 15.0`, `TurnChangeRate: 64.0`, `TurnReturnToRestRate: 56.0`, `LateralFriction: 1.60`), clean jump duration (`JumpDuration_Min: 0.38s`, `JumpDuration_Max: 0.58s`) and native gravity landing.
- **High-Speed Push:** `AnimationCurve` cached as `static readonly` for progressive acceleration up to 100+ km/h without speed decay.
- **Anti-Gravel & Suspension:** Prevents slowdown on grass/gravel and uneven terrain via Harmony patches (`GetSurfaceSmoothness`, `IsOnTerrain`).
- **Store Injection & Console:** Dynamic dialog injection for Jeff Gilmore plus console commands `skate` (give, stats, help).
- **Strict Vanilla Isolation & Camera:** Pure runtime instance tuning at `OnMount` protects all vanilla boards (Golden Skateboard, Cruiser, etc.) from overwrites and preserves the native `SkateboardCamera` follow.

**MoreSaveSlots v1.0.12 (2026-09-13, Bug-Report-Runde 6):**
- **v1.0.12 (Bug-Report-Runde 6 2026-09-13):** S-05 „tote DEL/EDIT-Buttons" — Rename-/Delete-Modals waehlten per `FindObjectsByType<Canvas>()[0]` einen unsortierten Canvas und landeten hinter dem Menue-Canvas (unsichtbar, Vollbild-Dimmer frass alle Klicks). Fix: `UIHelper.FindDialogCanvas()` (SaveDisplay-UI-Stack, Fallback hoechster `sortingOrder`), eigenes Sorting-Overlay (`overrideSorting`, order 1000) + GraphicRaycaster pro Modal, `SetAsLastSibling` beim Re-Open.
- **v1.0.11 (2026-09-13):** Savegame-Dupe-Fix (Geisterkarten).
- **v1.0.10 (Bug-Audit 2026-09-13):** `CleanupOwnedTriggersForSlot(IntPtr)` (PaginationController.cs:39-68) räumt owned EventTrigger-Pointer explizit ab; `RefreshActiveScreen()` (Z. 104-118) entfernt stale Keys pro Slot — schliesst das Rest-Leak aus dem Runde-3-Audit (F18-Verifikation).
- **v1.0.9 (Bug-Audit 2026-09-12):** EventTrigger-Cleanup (`PaginationController.AttachHoverTracker`) entfernt nur noch **eigene** Entries (per `_ownedTriggers`-Dictionary getrackt) statt alle Pointer-Eintraege. Vorher wurden Vanilla-Hover/Click-Handler der Save-Slot-Cards mit-entfernt, wodurch der Hover-Highlight der Slots verloren ging.
- **v1.0.3:** NewGame Bounds-Guard; EventHelper-Hover ohne Clear(); Font-Liveness; Rename ohne Regex-Fallback.
- **v1.0.2:** 1-based Slot-Nummern im Scan; toter Dialog-Block + SlotsPerPage-Config entfernt.
- **Expand Save Slots:** Expands game save slots from vanilla 5 up to 25+ slots (default 25, configurable in `UserData/MoreSaveSlots/config.json`).
- **Paginated Navigation:** Native `[ ◄ PREV ]` / `[ NEXT ► ]` navigation bar in `ContinueScreen`, `NewGameScreen`, and `ImportScreen`.
- **Save Game Renaming:** Inline save rename modal dialog with backdrop dismiss, canvas parenting, and `Game.json.bak` auto-backup before writing.
- **Configurable Hotkeys & Wheel:** Custom string keybindings in `config.json` (`PrevPageKey`, `NextPageKey`, `RenameKey`), plus mouse wheel scrolling for flipping pages.
- **Fast Caching & Focus UX:** `SlotCardCache` UI hierarchy caching, direct screen references, auto-jump to `LastPlayedGame` page on `ContinueScreen` open, and active slot highlight.
- **Harmony Patches & Scene Return Resilience:** Transparent remapping on `SaveDisplay.Refresh`, `SaveDisplay.Awake`, `SaveDisplay.SetDisplayedSave`, `ContinueScreen.LoadGame`, `NewGameScreen.SlotSelected`, `MenuScreen.OnOpen`/`OnClose`, and `SaveManager.Awake`. Skips native out-of-bounds loops when returning to Main Menu.


**PocketShop v0.2.7 (2026-09-17, Level-Lock & Inline Quantity Input):**
- **v0.2.6 (Feature-Update 2026-09-17):** Vanilla Level- & Rank-Requirements werden jetzt erzwungen (`StorableItemDefinition.RequiresLevelToPurchase` + `IsUnlocked`). Gesperrte Items zeigen `🔒 LOCKED (REQUIRES [RANK])`, Stock zeigt `🔒 LOCKED ([RANK])`, und Kauf-Buttons sind deaktiviert (`BuyResult.LevelLocked`, konfigurierbar via `EnforceLevelRequirements`). Direktes Inline-Zahlenfeld in `QuantitySelector` wie in Dan's Hardware: Ein Klick auf die Zahl zwischen `[-]` und `[+]` aktiviert ein editierbares Zahlenfeld (`InputField`) direkt auf der Karte/im Detailview (z.B. Eingabe von 20 oder 40 ohne extra HUD/Popup). Mit automatischer Stock-Begrenzung bei Bestätigung und `PocketShopInputFocus` (WASD-Sperre via `Controls.IsTyping` während des Tippens).
- **v0.2.5 (Bug-Audit 2026-09-13):** `RefreshShopCount()` (StoreCatalogPane.cs:211-218) selektiver Count-Refresh; aggressive Cache-Invalidation in `ShopCatalog.cs:95-96`; `ChangeBySafe(int)` (QuantitySelector.cs:136-151) Edge-Case-Guard am Min/Max-Anschlag (LOW-Findings F25-F28).
- **v0.2.4 (Bug-Audit 2026-09-12):** `PurchaseService.BuyWithQuantity` liest jetzt den Live-Bestand (`item.SourceListing.CurrentStock` + `IsInStock`) statt nur den POCO-Snapshot — schliesst Koop-Oversell. `QuantitySelector.ClampTo` synct `_maxStockOrSentinel` (Feld nicht mehr readonly); Sentinel (-1) wird zuerst abgefangen (vorher: <=0-First-Branch hat unbegrenzte Items auf qty=1 zurueckgesetzt). `HandlePurchaseResult` ruft nach erfolgreichem Kauf `_gridPane.NotifyStockChanged(itemId)` auf → Grid-Karten zeigen frischen Badge + geclamptes QuantitySelector-Max. Neuer `ItemPOCO.ItemId` + `ItemCard.GetItemIdPublic()` fuer Cross-Card-Dispatch.
- **v0.2.3:** Catalog Per-Handler-Invoke; CurrentStock-Fallback; GridPane Re-Subscribe-Guard; TearDown mit DirectoryPane.
- **v0.2.2:** Statischer Event-Dispatcher + Dispose; toter Mugshot-Scan raus; Partial-Refund nur Rest; Catalog-Refresh guarded.
- **Multi-Payment Switcher & Bank Integration:** Interactive chips in the sub-header (`[💵 Cash]`, `[💳 Bank Card]`, `[⚡ Auto]`). Supports physical cash (`ChangeCashBalance`) and online bank transfers (`CreateOnlineTransaction`). The `Auto` mode prefers cash and seamlessly falls back to the bank account when cash is insufficient.
- **Item Detail & Inspection Modal (`ItemDetailModal`):** Click on an item icon or name opens a high-resolution detail view with full price breakdown (base price, service fee, total), merchant source, stock count, and quick-quantity chips (`[+1]`, `[+5]`, `[+10]`, `[MAX]`) for fast bulk purchase.
- **Clean Shopping (No Disruptive Overlays):** Purchase confirmations complete without blocking banner overlays or fullscreen animations.
- **Native & Procedural Audio Feedback (`SoundService`):** Plays the game's real cash register chime (`MoneyManager.Instance.PlayCashSound()`) on successful purchases, soft clicks on quantity changes, and alarm tones on rejected transactions. Enable/disable via `PocketShopConfig.EnableSoundEffects`.
- **2-Level Navigation:** 4×3 grid with themed vector icons and merchant portraits (`StoreCatalogPane`) plus a 5×N item grid (`ItemGridPane`) with synchronized stock count.

**BankApp v0.4.4 (2026-09-12, Audit-Patch Runde 3 2026-09-12):**
- **v0.4.4 (Bug-Audit 2026-09-12):** `DepositCash`/`WithdrawCash` haben jetzt einen `NetworkGuard.IsInMainScene`-Guard vor jedem Geld-Op. Defense in depth: bei Scene-Wechsel-Mid-Call oder Hotkey-Aufruf außerhalb von Main bricht der Service sauber mit Fehlermeldung ab, statt eine Teiltransaktion zu riskieren.
- **v0.4.3 (Bug-Audit 2026-09-12):** `UpdateAmountDisplay`/`UpdateModeVisuals`/`SetFeedback` nutzen jetzt `IsAlive(...)` (vorher nur `!= null`-Check) — konsistent zu v0.4.2-RefreshAll. Verhindert MissingReference-Spam bei Phone-Close oder Szenenwechsel.
- **v0.4.2:** WasCollected-Guards (RefreshAll/Update/PhoneClosed); PreLoad ohne Slot-Reset.
- **v0.4.1:** Statischer Event-Dispatcher (OnUpdate/Balance/History) gegen Subscriber-Leaks.
- **Mockup-Based Redesign** (`Source/Mods/BankApp/docs/mockup-target-v0.3.0.png`): Weekly progress bar ($10k ATM limit), two-column balances (Cash | Online, teal), chip grid 2×5 ($1–$1000 + ✕ CLEAR + MAX accent), ⬇DEPOSIT/⬆WITHDRAW mode tabs, single full-width confirm button (green/orange by mode).
- **Chip Interaction:** Chips ADD to the running amount; MAX fills mode-dependent maximum (max depositable cash vs. max withdrawable); CLEAR resets.
- **Removed:** Free-text amount input + `BankAppInputFocus` (no typing in UI → WASD protection obsolete; IL2Cpp registration removed).
- **Unchanged Backend:** Double-entry transactions (rollback on failure), slot capacity, weekly limit, slot-isolated persistence, audio.

**BusinessIncome v0.1.6 (2026-09-17, 0-Business Backlog-Fix):**
- **v0.1.6 (2026-09-17):** Fix fuer 0-Business-Backlog-Spam beim Laden eines neuen Spielstands.
- **v0.1.5 (Bug-Report-Runde 6 2026-09-13):** BIZ-01 Catch-up-Loop nach Cap neu eingelesen + hart auf `MaxCatchupDays` gedeckelt + LastPaid-Validierung beim Laden (Freeze bei korruptem State); BIZ-02 `biz trigger --commit` verlangt `--force` bei bereits bezahltem Tag (Money-Printer geschlossen); BIZ-03 `float.IsFinite`-/Upper-Bound-Guards in `Sanitize` inkl. `MaxCatchupDays`-Clamp (NaN-Economy-Brick verhindert); BIZ-04 `biz pending confirm` committet nur vorwaerts; BIZ-05 `OnPreLoad` behaelt den Slot (`keepSlot:true`, kein `*_default.json` im Lade-Fenster).
- **v0.1.4 (Bug-Audit 2026-09-12):** `IsHostOrSingleplayer` jetzt fail-**closed** bei Exceptions (`return false` statt `return true` — der Singleplayer-Pfad ist bereits ueber den `NetworkManager == null`-Branch abgedeckt; ein Authority-Marshalling-Fehler auf einem MP-Client erlaubt nun nicht mehr Doppelbuchungen). Bei `commit && !committed` (Geld gebucht aber Save fehlgeschlagen) zeigt die HUD-Notification jetzt einen separaten „booked — save FAILED, run `biz pending confirm|resolve`"-Hinweis statt der irrefuehrenden Erfolgsmeldung; Pending-Marker bleibt auf Disk, der `biz pending`-Konsolen-Resolve-Pfad war bereits vorhanden.
- **v0.1.2:** Windfall-Seed wird persistiert (CommitPayout).
- **v0.1.1:** Windfall-Seed bei Erstinstallation; Catch-up via OnDayPass; invariantes Display-Format.
- **Daily Passive Revenue:** Generates daily passive income for all owned businesses (`Business.OwnedBusinesses`) via online bank transfer through `S1API.Money.Money.CreateOnlineTransaction`.
- **Multiplayer Host Authority:** Strict server/host guard (`IncomeEngine.IsHostOrSingleplayer()`) prevents duplicate payouts and money duplication on client instances.
- **Savegame Slot Idempotency & SafeStorage:** Slot-isolated persistence (`payout_state_slot_{slotId}.json`) with atomic write and `.bak` crash protection. Idempotent `LastPaidElapsedDay` marker guards against double-booking across scene reloads and save reloads.
- **Deterministic Revenue Model:** Pseudo-random variance (±15%) computed via hash from `(ElapsedDays, BusinessId)`. Employee bonus (+5%/employee, cap 25%), weekend bonus (+25% for bars/nightclubs), and configurable operating costs (10%).
- **HUD & Audio Feedback:** In-game HUD notification via `NotificationsManager.Instance.SendNotification` with cash register chime.
- **Console & Terminal Dashboard:** Full dev-console (`~` / `F1`) and DooDesch `hash` terminal (`#`) integration (`biz stats`, `biz trigger [--commit]`, `biz config`, `biz set <key> <val>`, `biz help`).

**Minimap v2.0.2 (2026-09-13, Bug-Report-Runde 6):**
- **v2.0.2 (Bug-Report-Runde 6 2026-09-13):** M-05 - `ContentSizeFitter` (`PreferredSize`) am Settings-Scroll-Content; Save&Apply-Footer ist jetzt bei langem Inhalt erreichbar (Scroll funktionierte vorher nie korrekt).
- **v2.0.1 (Bug-Audit 2026-09-12):** `ResolveSlotSuffix → Save()`-Rekursion gefixt (Crash): `_dirty` wird jetzt vor `FlushToPath(oldPath)` konsumiert → keine unkatchbare `StackOverflowException` mehr beim Slot-Switch nach fehlgeschlagenem Waypoint-Save (AV/Readonly); neue `FlushToPath(string)` schreibt an expliziten Pfad ohne erneute `ResolveSlotSuffix`-Auflösung. Health-Bar bekommt `sizeDelta = (mapSize*0.92f, 6f)` (vorher Unity-Default 100x100 → ueberlappte die Karte).
- **v1.0.3:** Critical-First-Blip-Partition (Pool-Overflow); Raycast-Sync in ApplyLayout.
- **v1.0.2:** Config-Save entprellt (Toggle/Unload statt pro Zoom-Taste).
- **Dual-Shape Viewport (Radar vs Tactical):** Dynamic uGUI mask switch between **Circular Radar** (with rotating compass ring N/E/S/W) and **Rounded-Square Tactical GPS** via procedural anti-aliasing.
- **Integrated DayCounter HUD:** Seamless integration of day (`DAY 14`), weekday (`Wednesday`), and clock (`15:30`) as a compact header or footer bar with emerald accent stripe and 0-allocation state caching.
- **Dynamic Rotation & Zoom:** Two rotation modes (`FollowPlayer` with rotating map vs. `NorthUp` with rotating player arrow) plus smooth live zoom (0.75×–4.0× via <kbd>[</kbd> / <kbd>]</kbd> / numpad).
- **0-Allocation Pooled Blip Engine:** Pre-allocated pool for 64 blips with fast scene scan (police, dealers/shops, customers, properties/trailers, quests) and shape-aware edge clamping at the minimap border.
- **Drag & Drop & Screen Bounds:** Free mouse drag while menus/pause/phone are open with automatic screen clamping and SafeStorage persistence (`UserData/Minimap/config.json`).
- **Console & Terminal Integration:** Dev console and DooDesch `hash` terminal (`minimap toggle`, `minimap shape`, `minimap zoom`, `minimap pos`, `minimap blips`, `minimap day`, `minimap help`).



**StackLimitMod v0.1.4 (2026-09-15, Agriculture-Only Mode & Weapon/Ammo Shield):**
- **v0.1.4 (Bug-Fix 2026-09-15):** Agriculture-Only Modus (`AgricultureOnly: true` als Standard): Beschränkt das Überschreiben von Stack-Limits strikt auf landwirtschaftliche Gegenstände (Erde/Soil, Samen/Seeds, Verpackungen/Baggies/Jars, Dünger/Additives, Pilzsporen/Spawn, Ernte-/Drogenprodukte). Hard-Guard gegen Waffen (Nahkampf/Schusswaffen), Munition, Kleidung und Bargeld: Waffen & Munition behalten immer ihr natives Limit (1 bzw. Vanilla) und werden im Postfix auf `BaseItemInstance.get_StackLimit` strikt abgefangen. Verhindert chinesische Schriftzeichen / Glyphen-Korruption im UI und Ammo-Unterlauf (`-1 Munition`) beim Nachladen. Automatische Wiederherstellung von Nicht-Agrar-Items beim Start. Neuer Console-Befehl `stack set ag <true|false>`.
- **v0.1.3 (Bug-Audit 2026-09-12):** `StackLimitEngine.RestoreAll()` neu: stellt jedes getrackte Original-`StackLimit` wieder her. Wird in `Mod.OnDeinitializeMelon` aufgerufen, sodass Mod-Disable/Unload die Definitionen nicht mit ueberschriebenen Limits zuruecklaesst. `OverrideNonStackable`-Default bleibt `true` (Backcompat), aber im Config-Kommentar dokumentiert: kann Quest-/Unique-Items stapelbar machen.
- **v0.1.2:** Exclude-Restore; Decision-Cache mit Cap + Known-Check.
- **v0.1.1:** Decision-Cache im Hot-Path; Toggle-off stellt Original-Limits wieder her.
- **Configurable Global Stack Limit:** Dynamically overrides item stack limits (1–9999, default 40) across player inventories, storage, shelves, and trunks.
- **Dual-Layer Item Discovery:** Automatically scans in-memory `BaseItemDefinition` assets via `Resources.FindObjectsOfTypeAll` and dynamic registry items via `Registry.Instance.GetAllItems()`.
- **Runtime Hook & Safety Patches:** Harmony postfix on `Registry.AddToRegistry` to automatically apply rules to newly registered items, plus instance-level patch on `BaseItemInstance.get_StackLimit` secured via `PatchGuard`.
- **Exclusions & Non-Stackable Control:** Configurable `OverrideNonStackable` toggle and `ExcludedItemIds` whitelist/blacklist.
- **In-Game Console & Terminal Integration:** Dev console (`~`) and DooDesch `hash` terminal (`#`) commands: `stack stats`, `stack set <amount>`, `stack reload`, `stack help` (with `stacklimit` alias).
- **SafeStorage Persistence:** Atomic JSON persistence (`UserData/StackLimitMod/config.json`) with automatic `.bak` recovery.


**AutoPackagingStation v0.2.8 (2026-09-13, Bug-Report-Runde 6):**
- **v0.2.7 (Bug-Report-Runde 6 2026-09-13):** Host-Guards fuer PackUp, OnDestroy-Refund, alle `TryExtract*`/`TryDeposit*`-Pfade und OnSaveComplete (9 neue `IsHostOrSingleplayer`-Guards). MP-Clients koennen Stationen nicht mehr lokal auszahlen/zerstoeren (Dupe/Desync) und ueberschreiben die Slot-Datei des Hosts nicht mehr. Supersedes 0.2.4.
- **v0.2.6 (Bug-Audit 2026-09-12):** F-Taste-PackUp Reentrancy-Schutz: `_packingUp`-Flag plus `try/finally`-Reset in `PackUpStation` verhindert Doppel-Auszahlung, falls `Destroy_Server` ein zweites Dispatch im selben Frame ausloest.
- **v0.2.5:** Quality-Mixing nativ (HIGH) + ObjLoader 50MB/250k-Vertex-Cap.
- **v0.2.4:** PackUpStation CRITICAL-Fix (BuildableItem.Destroy_Server statt GameObject.Destroy), Output-Pre-Flight.
- **v0.2.1/v0.2.2:** Output-Extraktion + Native-Sync + GUID-Sync (v0.2.1); rData-Dupe-Fix, Snapshot/Revert, PackUp-Fit-Check, Slot--1-Guard (v0.2.2).
- **4x4 Industrial Automated Packaging:** Instantiates a massive 4x4 animated machine with conveyor belt, pneumatic press, and status LEDs.
- **Atomic 2-Phase Engine:** Deducts inputs (raw product + packaging material) in the exact same frame as output insertion, preventing TOCTOU duplication exploits.
- **Weighted Quality Mixing:** Automatically blends input product quality using weighted average based on item quantities.
- **SafeStorage Slot-Isolation:** Stores station states separately per save slot under `autopack_slot_{slotId}.json` with `.bak` safety backups.
- **Strict Raycast & Key Guards:** Fixes AOE dismantling using strict raycast target validation and blocks key input whenever cursor is unlocked (UIs open).
- **Multiplayer & IL2CPP Stability:** Uses `IsServer` checks to prevent client desyncs and implements native `Il2CppType.Of` component setups to avoid crashes.

**HitmanPhone v0.2.9 (2026-09-15, KnockOut & Quest-Lifecycle In-Game-Verified):**
- **v0.2.8 (In-Game-Verified 2026-09-15):** Patch auf `S1NPCHealth.KnockOut` fängt nicht-tödliche K.o.-Schläge (Faust/Nahkampf) ab, sodass Ziele nicht mehr ins Wasser geworfen werden müssen. 5-Sekunden-Latch (`_recentlyHandledNpcIds`) in `BountyService` verhindert Doppel-Drops von Polaroids und schwebende Ragdoll-Klone. `CleanupOrphanBountyQuests()` und sauberer 1:1-Rebind in `BountyJournalBridge` bereinigen verwaiste/geduplizierte Quests im HUD beim Laden. Live verifiziert: 3 Verträge (Kyle Cooley, Chloe Bowers, Sam Thompson) nacheinander durchgespielt, jeweils 1 Polaroid gespawnt, Dead-Drop validiert, Cash ausgezahlt, Quests sauber auf 0 aktive / 9 historische abgeschlossen.
- **v0.2.7 (2026-09-14):** Restore-BountyQuests rendern im Journal als „Hitman Contract" statt „Hitman Contract: <NPC>" — der Vanilla-UI-Snapshot im `QuestComponent` wird im ctor einmalig gesetzt und durch den v0.2.6-Reflection-Setter auf `s1q.title` nicht neu getriggert; v0.2.7 synct den Vanilla-UI-Titel.
- **v0.2.5 (bug-ludwig-double 2026-09-13):** Cross-Session-Fallback in `TryValidateAndPay` lehnte jeden Polaroid-Drop ab sobald >1 Vertrag `AwaitingDrop=true` war. Crow bietet aber tagelang neue Kopfgelder auf denselben NPC an (ludwig_meyer Day 5+6). Nach Save-Reload war der polaroid `Value=0` (Unity InstanceID futsch) → Refuse. Fix: neue `GroupSameTargetAwaiting`-Helper gruppiert wartende Vertraege nach `TargetNpcId`; bei einheitlichem TargetNpcId wird die ganze Gruppe gemeinsam ausgezahlt (ein Kill = ein Beweisstueck = mehrere Kopfgelder), heterogene NPCs bleiben geschuetzt. `OnStorageContentsChanged` macht Side-Effects (Reward/Journal/Heat/Cooldown) pro Vertrag in der Liste und konsumiert EIN Polaroid fuer die ganze Gruppe. Persist einmal pro Batch, ausser eine Payout schlug fehl (Retry-Pfad).
- **v0.2.4 (Bug-Audit 2026-09-12):** `OnStorageContentsChanged` laesst jetzt den Client-Spieler mit eigenem Save validieren + zahlen (vorher: Host-only-Gate blockierte Client-Payout komplett, weil das host-lokale Save nie den `AwaitingDrop`-Vertrag des Clients sah); `ChangeCashBalance` synct Player-Bilanz ueber FishNet. `BountyTargetWatchdog` prueft `npc.Health.IsDead` statt `!npc.IsConscious` — verhindert, dass ein K.o. (nicht-tödliche Betaeubung) als Kill mit voller Belohnung + Lethal-Pursuit zaehlt.
- **v0.2.3:** Deadline >=; Single-Slot-Consume; OnDecline-Guard; Tod-Log auf Debug.
- **Bounty/Contract-Gameplay:** Anonyme Auftraggeber ("Ghost", "Jackal", …) bieten Hits via Phone-Messages-App; Ziele aus dem Dealer-Kundennetzwerk; "Polaroid Evidence"-Item (InstanceID-codiert) wird im Dead Drop gegen Dirty Cash eingelöst.
- **Systeme:** Police-Heat-Integration, Journal-Quests, 3-Tage-Ablauf, Death-Forfeit, slot-isolierte Persistenz (`slot_{n}` + TryMigrateLegacy).
- **v0.2.2 Audit-Fixes:** Payout host-only, PatchGuard statt PatchAll, Day-Throttle, Cooldown-Persist, Deadline-Boundary, Test-Commands nur DEBUG.

**_DiagPerfCounter v0.3.2 (2026-09-11, Dev-Tool):**
- **v0.3.2:** Nur noch DEBUG-Builds; Dump via SaveTextAtomic mit Return-Types.
- **Zweck:** Diagnose-Tool — dumpt `StorageEntity`-Methodensignaturen per Reflection nach `UserData\_DiagPerfCounter\dump.txt` (Hook-Target-Discovery). Kein Gameplay-Effekt.
- **v0.3.1:** `Source/Mods/_DiagPerfCounter/docs/mod.json` ergänzt; Dump-Pfad aus UserData-Root verlegt.

---


### Archived Mods (`Source/Archive/`)

**BackpackMod v1.2.3 (archived):**
- **Description:** 3D wearable backpacks with realistic harness straps, tier-based storage scaling (+4/+8/+16 slots), runtime `.obj` model loading, 360° mannequin rotation, and button-only inventory sorting with stack-merge.
- **Archive path:** `Source/Archive/BackpackMod/`
- **History:** v1.2.3 (StorageMenu-Sort-Overlay-Fix), v1.2.2 (Raycast-Schutz), v1.2.0 (GetCopy atomar + Overflow-Sidecar).

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
- **Description:** Street Nomad lifestyle — Everywhere Building (unrestricted placement via `BuildUpdate_Grid` patches), procedural 3D sleeping bag, outdoor item dismantling, Street Nomad questline (3 quests), slot-isolierte Persistenz (`street_items_slot_{n}.json`). DLL/Deploy nach `Mods\_archived\` (Save-Daten in `UserData\HomelessMod\` bleiben erhalten).
- **Archive path:** `Source/Archive/HomelessMod/`
- **History:** v0.1.12, **Verified 2026-09-15**; Detail-Verlauf siehe `Source/Archive/HomelessMod/docs/CHANGELOG.md`.

**SnackVendor v0.0.9 (archived 2026-09-17):**
- **Description:** Player-stocked vending machine (S1API Buildable + Vanilla-VendingMachine-Klon, GLB-Mesh via `S1MAPI.GltfLoader`, NPC-Inventory-Credit, Deposit/Extract-Panel, Hardware-Shop-Listing). Build OK, In-Game-Verify nie erfolgt. DLL/Assets nach `Mods\_archived\` (incl. `Mods\_archived\SnackVendor\` GLB-Ordner).
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

> **ThirdParty-Deploy (2026-09-16 optimiert):** `Tools/deploy-thirdparty.ps1` läuft nur noch einmalig gebunden an `Shared.dll`, statt redundant bei jedem einzelnen Mod 16x parallel ausgeführt zu werden. Mutex auf lokale Sitzungsebene umgestellt.
>
> **Hinweis:** `Tools\` wurde am 2026-09-10 aus der Git-Historie reaktiviert (alle Helper ausser `fix-knowledge-paths.ps1` — Knowledge/ existiert nicht mehr; Reaktivierung: `git show 891c330^:Tools/fix-knowledge-paths.ps1`).
>
> **Drift-Guard (2026-09-14, neu):** `Tools/check-version-sync.ps1` vergleicht die Code-Version (MelonInfo-Attribut bzw. `Constants.ModVersion`) mit `Source/Mods/<Mod>/docs/mod.json`, `README.md`, der AGENTS.md-Matrix-Zeile und dem AGENTS.md-Detail-Header. Exit 1 bei Drift; laeuft in `.github/workflows/ci.yml` und im Pre-Commit-Hook. `Tools/bump-version.ps1` schreibt die Detail-Header jetzt ebenfalls mit. **Dazu (2026-09-16):** `Tools/check-doc-paths.ps1` prueft, dass alle in Markdown referenzierten Repo-Pfade existieren (gleiche Gates) — Doku-Pfade sind damit die zweite bewachte Drift-Klasse.
>
> **Cleanup-Hinweis (2026-09-10):** In `<GameDir>\Mods\` lagen pre-existing zwei Reference-Assemblies (`S1API.dll` 415 KB, `Hash.dll` 188 KB) aus frueheren manuellen Deploys. Beide riefen beim Spielstart `BadImageFormatException` (Reference assemblies should not be loaded for execution) und floodeten den Log mit ~50 ERROR-Zeilen, ohne funktional etwas zu brechen. Geloescht — die echten Versionen sind: `S1API.Il2Cpp.MelonLoader.dll` (Mods\), `Plugins\S1APILoader.dll`, `UserLibs\S1MAPI_Il2cpp.dll` (fuer AutoPackagingStation.GltfLoader). Hash.dll bleibt nur noch als Referenz im Repo (`ThirdParty/Archive/ScheduleOne-Hash/`), nicht deployed — PotScanner nutzt den S1API-Hash-Bridge ohne DLL via Reflection-Queue.

Deployment läuft **automatisch** via `Directory.Build.targets` — mit getrennten Zielen (Konvention seit Reinstall 2026-09):

- `<GameDir>\Mods\`: DLL + PNGs + Bundles (nur von MelonLoader ladbare Dateien)
- `<GameDir>\UserData\<Mod>\`: `mod.json` (Metadaten) + `<Mod>.pdb` (portable Debug-Symbole via `<DebugType>portable</DebugType>`)

**JSON und PDB gehören niemals nach `Mods\`.**

### ⚠️ Important Build Caveats

- **SkipUnchangedFiles:** Fixed to `false` (2026-08-20) — every `dotnet build` now force-deploys to `<GameDir>\Mods\`, eliminating stale-DLL traps.
- **GameDir Override:** `$env:SCHEDULE1_PATH` must be set **before** running `dotnet build`, because MSBuild evaluates the property once at startup.
- **Solution Determinism:** `Tools/gen-sln.ps1` now uses deterministic MD5-GUIDs (`Get-DeterministicGuid "Project:<rel>"`) — no more random GUID diffs on every regeneration. CI vergleicht Zeilenenden-normalisiert (Checkout-unabhaengig).
- **Version Bumps:** Use `pwsh Tools/bump-version.ps1 -Mod <Name> -Version x.y.z` to sync `Mod.cs` + `mod.json` + `CHANGELOG.md` + `AGENTS.md` atomically.

### Sideload + hash (ThirdParty, NICHT in der Solution)

Sideload (DooDesch) und hash (DooDesch) liegen nach dem Umzug 2026-09 als **fertige Release-Pakete** unter `ThirdParty/` vor (ZIP + extrahiert, kein Source/Build mehr): `ScheduleOne-Sideload/Sideload_extracted/` (Sideload 1.7.0, inkl. `UserLibs/`-Abhängigkeiten) und `ScheduleOne-Hash/Hash_extracted/` (hash 1.0.5). Beide sind **nicht** Teil von `S1Mods.sln`.

**Deploy (manuell, bei Bedarf):**
```pwsh
# hash 1.0.5 (deprecated, undeployed — nur Referenz im Repo):
# DLL liegt unter ThirdParty/Archive/ScheduleOne-Hash/Hash_extracted/Hash.dll.
# Sideload 1.7.0 ist komplett entfernt (deprecated 2026-09-10, kein Archive-Rest);
# Hash.dll benoetigte Sideload zur Laufzeit und ist daher nicht mehr lauffaehig
# deployment-wuerdig — bei echtem Bedarf Sideload neu beschaffen (DooDesch, Nexus).
```

### Multi-PC-Workflow (Laptop ↔ Wohnungs-PC)

Linearer Workflow auf `main`, keine Feature-Branches. Beim ersten Klon auf einem neuen PC:

```pwsh
# 1. Repo klonen
git clone https://github.com/domi7602/Schedule1-mod-only.git
cd Schedule1-mod-only

# 2. Submodule + Build-Props generieren (idempotent, fragt nach Game-Pfad falls nicht Standard)
pwsh Tools/setup-workspace.ps1

# 3. Bauen
dotnet build Source/Mods/S1Mods.sln -c Release
```

`local.build.props` (S1API + S1MAPI) sind gitignored und werden vom Skript pro PC generiert. Spiel-Saves (`<GameDir>\UserData\<Mod>\*.json`) sind ebenfalls PC-lokal — beim Wechsel sieht jeder PC nur seine eigenen Saves.

Vor jeder Session auf einem anderen PC: `git pull`. Nach jeder Session: `git add && git commit && git push`.

**Laptop-Sandbox-Workspace (Variante A):** Wenn du auf einem zweiten PC (Laptop, Test-Maschine) unabhängig vom Wohnungs-PC arbeiten willst, ohne das stabile Repo zu gefährden: `pwsh Tools/new-laptop-workspace.ps1`. Das Skript klont das Repo, benennt `origin` zu `upstream` um (damit versehentliche Pushes physikalisch ausgeschlossen sind), konfiguriert optional ein privates Backup-Remote als neuen `origin`, und läuft `setup-workspace.ps1` direkt mit. **Kein GitHub-Fork** — eine Code-Wahrheit auf GitHub, lokale Sandbox daneben. Merge vom stabilen Repo: `git fetch upstream && git merge upstream/main`.

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
s1interop doctor --mono-game-path $env:SCHEDULE1_PATH --il2cpp-game-path $env:SCHEDULE1_PATH
s1interop analyze "$env:SCHEDULE1_PATH\..\..\Users\pc\Schedule1-mod-only\Source\Mods\<Name>\src\<Name>.csproj"
# or simply, from the workspace root:
s1interop analyze "Source\Mods\<Name>\src\<Name>.csproj"
```

### Mod Update Cycle

1. Edit code in `Source/Mods/<Name>/src/`
2. `dotnet build Source\Mods\<Name>\src\<Name>.csproj -c Release` (or `pwsh Tools/build-all.ps1`)
3. DLL is automatically deployed to `<Game>\Mods\` (force, no stale-DLL)
4. Launch the game & check `Latest.log`
5. For version bumps: `pwsh Tools/bump-version.ps1 -Mod <Name> -Version x.y.z` (syncs 5 files incl. README.md)

### CI & Quality Gates

- **Local:** `dotnet format --verify-no-changes`, `pwsh Tools/gen-sln.ps1` (determinism check), `pwsh Tools/check-version-sync.ps1` (Versions-Drift-Guard), `pwsh Tools/check-doc-paths.ps1` (Doku-Pfad-Guard), `dotnet test Source/Tests/Shared.Tests/Shared.Tests.csproj` + `dotnet test Source/Tests/AutoPackagingStation.Tests/AutoPackagingStation.Tests.csproj` + `dotnet test Source/Tests/CalculatorApp.Tests/CalculatorApp.Tests.csproj` (Tests brauchen Spiel-Assemblies)
- **CI:** `.github/workflows/ci.yml` runs on push/PR (format + version-sync check + game-gated build/tests + gen-sln check + s1interop advisory analysis). Der s1interop-Job (aktiviert 2026-09-19) installiert `S1Interop` 0.1.0-alpha.1 (ifBars/S1Interop, dotnet tool) und analysiert alle Mod-csproj — **advisory, niemals blockierend** (Exit immer 0). Bekannte Alpha-False-Positives: TFM/LangVersion aus `Directory.Build.props` werden nicht ausgewertet; akzeptierte Reflection in HitmanPhone; mod-internes `List<T>` in BusinessIncome.
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

> **Note:** This mod-only workspace does not include the `Knowledge/` folder (decompiles, analyses, maps, tools). Der frühere Haupt-Workspace `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule 1 Modding` existiert nach der Neuinstallation (2026-09) **nicht mehr** — die Knowledge-Base (4804 Dateien / 67 MB) muss bei Bedarf neu generiert (AssetRipper/ilspycmd) oder aus einem Backup wiederhergestellt werden.

Reference decompiles and analysis snippets for archived mods are available in `Source/Archive/`.
