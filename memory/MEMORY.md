# MEMORY.md — Schedule 1 Modding Knowledge Hub

> **Central Memory Bank for AI Agents & Developers**
> Workspace: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule1-mod-only-main` (seit Neuinstallation 2026-09 im Spiel-Ordner)
> Last Synchronized: 2026-09-10 (Bereinigung: komplette Datei-Duplikate entfernt, Gotchas zusammengeführt, Versionen an `AGENTS.md` synchronisiert)
> Toolchain: .NET SDK 8.0.424 · MelonLoader 0.7.3 · S1API 3.2.0 (Fork-Build aus `ThirdParty/S1API/`)
> `.agents/skills/` (14 Skills) und `Tools/`-Helper wurden beim Umzug 2026-09 aus dem Repo entfernt — Reaktivierung via Git-Historie (`git checkout HEAD -- .agents/skills/` bzw. `Tools/`).
> Knowledge-Base (4804 files / 67 MB, Stand 2026-08-22): alter Workspace existiert nicht mehr — bei Bedarf neu generieren (AssetRipper/ilspycmd) oder aus Backup wiederherstellen.
> **Single Source of Truth für Mod-Inventar & Versionen: `AGENTS.md` §2.** Dieses File hält nur noch artefact-freies Wissen: Architektur-Regeln, Gotchas, Historie.

---

## 1. Workspace Metadata & Toolchain

| Parameter | Specification |
|---|---|
| Game | *Schedule I* (TVGS) `v0.4.6f13` (Unity 2022.3.62f2, IL2CPP), Default Steam Release |
| Mod Loader | MelonLoader `0.7.3` (net6) |
| TFM | `net6.0`, `LangVersion 12`, `Nullable enable`, `<DebugType>portable</DebugType>` |
| Branch Policy | **Strictly IL2CPP** — niemals User zum `alternate`-Mono-Branch zwingen |
| Deploy | `Directory.Build.targets`: DLL/Icons/Bundles → `<GameDir>\Mods\`; `mod.json` + portable `.pdb` → `<GameDir>\UserData\<Mod>\` (seit 2026-09). **Niemals** json/pdb nach `Mods\`. |
| Build | `dotnet build Source\Mods\<Name>\src\<Name>.csproj -c Release` (Deploy automatisch; `SkipUnchangedFiles=false` → kein stale-DLL) |
| Solution | `Source/Mods/S1Mods.sln` — deterministische MD5-GUIDs via `gen-sln.ps1` (in Git-Historie) |

---

## 2. Core Architecture Standards (Non-Negotiable Guardrails)

### 2.1 IL2CPP Pointer Validation & Lifecycle Safety
- **Pointer Validation:** `obj != null` ist unzureichend — immer `obj != null && obj.Pointer != IntPtr.Zero` prüfen (Managed-Proxies outliven die nativen C++-Objekte).
- **Public `IntPtr` Constructors:** Jede `[RegisterTypeInIl2Cpp]`-MonoBehaviour **MUSS** `public X(IntPtr ptr) : base(ptr) { }` deklarieren — sonst nativer Bridge-Crash beim Attachment.
- **Kein `foreach`/LINQ auf `Il2CppSystem.Collections.Generic.List<T>`** — nur indizierte `for`-Loops (0-Allocation).
- **Scene-Transition Cleanup:** Statische UI-/GameObject-Caches in `OnSceneWasUnloaded`/`OnPreLoad` explizit leeren (dangling Native-Pointer-Zugriffe verhindern).
- **`UnityEngine.TextMesh` ist in IL2CPP gestript** — `AddComponent(Il2CppType.Of<TextMesh>())` wirft `ArgumentException`. Ersatz: 0-Allocation-`OnGUI()`-HUD mit statisch gecachten `GUIStyle`s + `Cursor.lockState`-Check + `Mathf.Clamp(Screen.height / 900f, 0.80f, 1.25f)`.

### 2.2 Responsive UI ("Method 3": `UITheme`)
- S1API-Phone-Canvases sind high-DPI und 90° rotiert — feste Pixelgrößen sind unlesbar/riesig. **Single Source of Truth:** `S1Mods.Shared.UITheme` (`Source/Mods/Shared/src/UITheme.cs`); `InitializeForTextApp` (750f, 0.85–2.0) für Text-Apps, `InitializeForDashboard` (900f, 0.75–1.20) für Dashboards. Mod-Wrapper delegieren nur (`Sp/Dp/Scale`), niemals lokal `Mathf.Clamp` reimplementieren (3 Divergenz-Duplikate wurden 2026-08-21 entfernt).
- **Non-Destructive PhoneApp-Lifecycle:** Nie `Object.Destroy()`/UI-Clears in `OnPhoneClosed()` — nur `_mainBG.SetActive(false)`. Sonst **"Transparent Phone"**-Bug beim erneuten Öffnen.
- **Rule 10/11:** `OnCreated()` feuert **1× pro Scene** (S1API `HomeScreen_Start_Patch`), `OnPhoneClosed()` bei jedem Close. **Niemals** `MelonEvents.OnUpdate`-Unsubscribe in `OnPhoneClosed` → App beim 2. Öffnen blank (C1-Regression 2026-08-20, 5 PhoneApps). Persistence: `slot_{n}.json` + `TryMigrateLegacy()`.

### 2.3 Safe Persistence & Savegame-Synchronisation
- **Atomic Writes:** Nur `SafeStorage.SaveAtomic`/`SaveTextAtomic` (`.tmp` → atomic Move → `.bak`), nie rohes `File.WriteAllText`/`File.Copy+Replace` (`Shared/SafeStorage.cs`).
- **Slot Isolation:** Mod-Saves suffixen mit `LoadManager.Instance.ActiveSaveInfo.SaveSlotNumber` → `<name>_slot_{n}.json` + einmalige `TryMigrateLegacy()`-Migration (Rule 11).
- **Transactionale Save-Integrity:** Runtime-Operationen bleiben strikt in RAM; Serialisierung **nur** auf `S1API.Lifecycle.GameLifecycle.OnSaveComplete`; State-Clear auf `OnPreLoad`/Scene-Unload (Anti-Dupe-Regel). Economics brauchen **Snapshot-Revert**: `MarkInMemoryPaid` snapshottet `prevLastPaid + perBusiness-Dict`, `Revert` stellt restore her (nie `-1`), `CommitPayout` cleared (`PayoutStateStore.cs`).
- **Sprite/Texture-Caches:** Immer `Dictionary<string,Sprite>` keyed `"{size}_{radius}_{thickness}"`, nie Single-Field-Cache (Minimap-Mask-Bug 2026-08-21).
- **Culture-Safe Parsing:** `float/decimal` immer mit `NumberStyles.Float + CultureInfo.InvariantCulture` parsen **und** formatieren (DE-Locale `,` bricht Bank-Transfers).

### 2.4 Resilient Harmony Patching (`PatchGuard`)
- **Graceful Degradation:** Patch-Registrierung nur via `S1Mods.Shared.PatchGuard.TryPatch` — Signature-Drift nach Game-Update → Warnung + Skip statt Hard-Crash. **Nie blind `harmony.PatchAll()`.**
- **Nie `ref <Il2CppType> __result` in Prefixes mit `return false`** — verwaltetes Wrapper-Objekt auf native Register → `0xc0000005` in `UnityPlayer.dll` (BackpackMod 2026-08-22). Stattdessen UI/List-Mutation direkt.
- Field-Accessors (`BaseItemDefinition.get_DefaultStackLimit`) sind **nicht patchbar** (`Il2CppInterop can't be patched`) — Instance-Methoden (`BaseItemInstance.get_StackLimit`) postfixen statt dessen.
- Prefixes mit Pointer-Validierung guarden; keine ungefangenen Exceptions in den Unity-Main-Loop escapen lassen.

### 2.5 Multiplayer Host Authority
- Alle global-economischen/weltverändernden State-Changes strikt mit `NetworkGuard.IsHostOrSingleplayer()` / `IncomeEngine.IsHostOrSingleplayer()` guarden.
- Achtung: In Singleplayer ist FishNet `InstanceFinder.IsServer == false` (NetworkManager inaktiv) — Helper muss `NetworkManager == null || (Object)NetworkManager == null || InstanceFinder.IsServer` checken, sonst Freeze in `Blocked`.
- Clients führen niemals lokale Payouts/State-Mods aus; alles läuft über den Host.

### 2.6 IL2CPP-Component-Architecture (Decoupled Runtime Pattern)
- Custom Managed-Types (`SlotItemData`, `SaveData`, `List<string>`) in Signaturen von `[RegisterTypeInIl2Cpp]`-MonoBehaviours → `Il2CppInterop` rejected die **gesamte Assembly**. MonoBehaviours bleiben rein primitiv (Unity-Lifecycle + `string Guid`); DTOs laufen über pure C#-Manager (`AutoPackStore.GetRuntimeData(guid)`).

---

## 3. Battle-Tested Gotchas & Solutions Matrix

Kompakte Referenz aller verifizierten Edge-Cases. Formale Quellen: Mod-`CHANGELOG.md`, `AGENTS.md` §5, Git-Historie der Bug-Hunt-Logs.

| Kontext | Gotcha | Lösung |
|---|---|---|
| **HomelessMod** (Outdoor) | `DestroyImmediate(GridItem)` → Hard Crash (Native-Component-Tables korrupt) | **Nie `GridItem` destroyen:** `gi.enabled=false; gi.SetFootprintTileVisiblity(false);` + `GridItem.Destroy`-Prefix `return false` bei `IsOutdoorItem` |
| **HomelessMod** | FishNet-Clone-Desync (Vanilla-Prefabs erwarten Server-Authority) | Sofort `BuildManager.Instance.DisableNetworking(obj)` + `DisableNavigation(obj)` |
| **HomelessMod** | Unsichtbare Objekte nach Interieur-Exit (`BuildableItem.SetCulled`/`Start` erwartet Vanilla-`Property`-Parent) | Prefix-Guards auf `BuildableItem.Start` + `SetCulled` → `return false` wenn Outdoor; `b.enabled=false` |
| **HomelessMod** | `Property`-Komponente auf `_streetRoot` kaputt Real-Estate-Apps/Dashboards/Raids | **Nie `Property` auf `_streetRoot`** — clean halten (`DontDestroyOnLoad`), Verwaltung via `StreetPropertyManager`-Singleton |
| **HomelessMod** | Floating Grid-Indicator-Tiles auf Asphalt | `FootprintTile`-Child-GameObjects deaktivieren + Child-Renderer filtern (Footprint aus, Mesh an) |
| **HomelessMod** | Dupe-Exploit bei Alt+F4 nach Platzieren (Disk-Save sofort, Inventory erst bei Game-Save) | Runtime strikt in RAM (`_activeStreetObjects`); Serialisierung nur auf `OnSaveComplete`; Clear auf `OnPreLoad` |
| **HomelessMod** | Raycast fällt durch Terrain (Street-Meshes liegen auf `Grid`-Layer) | Mask `~LayerMask.GetMask("Ignore Raycast","Player")` inkl. `Grid`, `QueryTriggerInteraction.Ignore` |
| **HomelessMod** | Falsche Pickup-Toast "doesn't fit" bei Outdoor-Right-Click (Buildable deaktiviert + aus Inventory entfernt) | Alternatives Pickup-Component (`OutdoorItemInteractable` via `TypeResolver` + `AddComponent(Il2CppType.From(...))`) mit `PackUp()` → `GetDefaultInstance(1)` → `AddItemToInventory` |
| **CustomSkateboard** | Material-Leaks via `r.material` | `r.sharedMaterial` + `static readonly`-Material-Caches |
| **CustomSkateboard** | Base-Item-ID-Drift zwischen Game-Versionen | Candidate-ID-Resolution-Chain + Diag-Logging + `BaseItemIdOverride`-Config |
| **CustomSkateboard** | TOCTOU `Registry.ItemExists` → `GetItem` | Ein atomares `GetItem(id)` mit Null/Pointer-Check |
| **CustomSkateboard** | Duplicate Dialogue Injection bei Jeff | `StringComparison.OrdinalIgnoreCase` vor Injektion |
| **CustomSkateboard** | Double-Tuning Allocation Churn (Awake+Mount-Hooks) | `_tunedBoards: HashSet<IntPtr>`-Idempotenzfilter, Clear in `OnSceneWasLoaded` |
| **CustomSkateboard** | Avatar-Visual-Corruption (Clothes/Eyes/Hair) | Renderer strikt nach Mesh-Name (`"deck"`/`"board"`) auf Child-Transforms filtern |
| **MoreSaveSlots** | Slot-Allocation-Timing-Crash (OOB nach `SaveManager`-Init) | `SaveManager.Awake`-Prefix injiziert erweiterten `SAVE_SLOT_COUNT` vor Array-Allokation |
| **MoreSaveSlots** | "Skeleton"-Buttons (Custom-Handler instanziiert keine Default-Graphics) | Standard `UnityEngine.UI.Button` + TMP-Font-Auto-Resolution vom aktiven Canvas |
| **MoreSaveSlots** | Main-Menu-Return-Crash (native 5-Slot-Loops) | Harmony-Guards in `SaveDisplay`/`MenuScreen` für OOB-Slot-Mappings |
| **MoreSaveSlots** | Szene-Guard verkehrt herum: `IsInMainScene()=="MainMenu"` deaktivierte Input im Menu, aktivierte Hijack im Gameplay | `IsInGameplayScene()` prüft exakt `"Main"` (Workspace-Konvention) |
| **MoreSaveSlots** | Rohe `File.WriteAllText`/`Copy+Replace` korruptiert `Game.json` bei Crash | `SafeStorage.SaveTextAtomic` (`.tmp`/`.bak`) |
| **PhoneApps** | WASD-Movement während Tippen (InputField blockt kein Player-Movement) | `[RegisterTypeInIl2Cpp]`-Component (IntPtr-ctor!) toggelt `S1API.Input.Controls.IsTyping` on focus/blur |
| **PhoneApps** | `UnityEvent.AddListener(new UnityAction(...))` IL2CPP-Crash | Immer `S1API.Utils.EventHelper.AddListener(...)` / `ButtonUtils.AddListener(...)` |
| **PhoneApps** | Multiline-Text zentriert (uGUI-Default) | `textComponent.alignment=UpperLeft`, `placeholder.alignment=UpperLeft`, `lineType=MultiLineNewline`, saubere `offsetMin/Max` |
| **PhoneApps** | Globaler State-File-Leak: `calculator_state.json` global → Slot-A leakte in Slot-B | `calculator_state_slot_{n}.json` via `GetActiveSlotSuffix()` + `TryMigrateLegacy()` (Rule 11) |
| **PhoneApps** | Lokale `UITheme`-Duplikate drifften (2.5/850:1.30 vs Shared 750/2.0) | Thin Wrapper → `S1Mods.Shared.UITheme`, niemals lokales Clamp |
| **PhoneApps** | ModConfig-Dictionaries nicht TOML-mappable → Config-Reset nach Restart | JSON-Sidecar via `SafeStorage` (`business_config.json`), Apply on Init + Save on every set |
| **PocketShop** | Leerer Shop-Katalog bei Fast-Load (`ShopInterface.AllShops` noch leer) | Resilient Retry-Loop (20×1.5s) auf Registry-Population |
| **PocketShop** | Nicht-atomarer Kauf: Zahlung vor Item-Übergabe, kein Rollback → Geldverlust | Alle Instanzen vor Zahlung vor Erstellen, Payment in try/catch, Inventory-Fail → Refund, `paymentExecuted`-Tracking |
| **PocketShop** | Shop mit 0 Items verschwand; `ShopCode ?? ShopName` null → Dictionary-NRE | Fallback-Kette (Code→Name→gameObject.name→Index) + Tile immer anzeigen |
| **BankApp** | Inventory-Overflow Cash-Loss beim Abheben | Kapazität über `CashSlot` + leere Hotbar-Slots ($1.000/slot) vor Withdrawal prüfen |
| **BusinessIncome** | Duplicate Payouts im Co-op (Host+Client rechnen beide) | Strikt `IsHostOrSingleplayer()` |
| **BusinessIncome** | Double-Booking bei Save/Reload im selben Day | `LastPaidElapsedDay` in slot-isoliertem JSON, assert `current > LastPaid` |
| **BusinessIncome** | Snapshot-Loss bei Tx-Fail (`Revert` setzte `-1` → Day-5-Re-Payment) | Snapshot `prevLastPaid + perBusiness` in `MarkInMemoryPaid`, restore in `Revert`, `CommitPayout` cleared |
| **Minimap** | GC-Stutter durch Per-Frame-Blip-Instantiate/Destroy | Pre-allocated 64-Blip-Pool + Shape-aware Edge-Clamping, 0 Allocations/Frame |
| **Minimap** | Single-Sprite-Cache für Maske/Border → falsche Maske nach Size/Radius-Wechsel | `Dictionary<string,Sprite>` keyed `"{size}"`/`"{size}_{radius}_{thickness}"` |
| **PotScanner** | Wide-Data-Tables squeezed im Portrait-Default | `Orientation => EOrientation.Horizontal` für Tabellen-Apps |
| **PotScanner** | `WaterSinglePot` ohne Ownership/Threshold-Guard; `OnSceneUnloaded` clearte `_ownedPropertyCodes` nicht | `PotTracker.FindByPtr` public + `IsOwnedProperty` + `WaterPercent >= SkipThreshold`; `.Clear()` ergänzen |
| **StackLimitMod** | Field-Accessor-Patch dead (16/16 false-positive, `Latest.log`) | Patch entfernen; Resources+Registry-Scan + `BaseItemInstance.get_StackLimit`-Postfix |
| **BackpackMod** | `ref <Il2CppType> __result` Prefix-Crash (0xc0000005) | Nie Objekt-Returns via Prefix überschreiben; nie blind `PatchAll()`; `PatchGuard.TryPatch` + Slot-Data via UI/List-Mutation |
| **BackpackMod** | `ClothingItemUI.UpdateUI`-NRE bei geklonten Clothing (geerbt `Colorable=true` ohne Palette) | `.WithColorable(false)` an `ClothingItemCreator.CloneFrom` wenn keine Color-Processing nötig |
| **AutoPackagingStation** | `ClassInjector` rejected Assembly bei Managed-Types in MB-Signaturen | Decoupled Runtime Pattern (§2.6): MBs rein primitiv, DTOs via pure C#-Manager |
| **AutoPackagingStation** | Outdoor-Cauldron-Visual-Bug (Base-Prefab-Renderer an, `BuildableItem.Start` bypassed, Duplicate-HUDs) | Custom-Station-IDs in `BuildUpdate_Grid_Place_Patch`+`SpawnSavedStreetItem` intercepten → `SetupPlacedStation` via `TypeResolver`, `StationGuid` sync, `HideBaseRenderers()` |
| **AutoPackagingStation** | Stuck `Blocked`/`NoPackaging` nach Load (Update-Loop deckt States nicht ab) | `ApplySaveData` erzwingt `Idle` wenn Blocked/NoPackaging **und** beide Inputs > 0. Lesson: State-Enums in Save/Load müssen unter der Recovery-Update-Loop abgeschlossen sein |
| **AutoPackagingStation** | Multi-Quantity-Probe-Deny: `GetDefaultInstance(qty)` in `CanItemFitInInventory(instance, qty)` testet qty² | Immer 1-Unit-Probe (`GetDefaultInstance(1)`), Add in Loop mit 1-Unit-Instanzen + Quality-Tier-Erhalt |
| **AutoPackagingStation** | FishNet `IsServer==false` in Singleplayer → Pack-Freeeze in `Blocked` | `IsHostOrSingleplayer()`-Helper (§2.5) |
| **AutoPackagingStation** | Packaged-Product-Identity-Desync: Packaged Drugs teilen Base-ID, Packaging liegt auf `ProductItemInstance` | `PackagingID` + `SetPackaging(pkgDef)` auf Probe **und** allen Outputs setzen; `CanStartPackaging`/`ExecutePackagingTransaction` auf `PackagingId` synchronisieren |
| **AutoPackagingStation** | HUD "folgt der Maus": Visibility an Aim gebunden + Screen-Center-Position | Visibility = Distance-only (`_isPlayerNear`), Aim-Gate nur für Input (`_isPlayerAiming`); `WorldToScreenPoint` + Screen-Clamp + `guiY = Screen.height - screenPos.y`; Camera.main-Null-Guard |
| **S1MCP** | Sync-Log-Tail-Freeze: 700k-Zeilen-`Latest.log` synchron geparst → 10s+ ANR | `Stream.Seek(-Math.Min(Length, 256*1024), SeekOrigin.End)` — nur letztes 256-KB-Fenster lesen |
| **Workspace-Tools** | S1MCP-Object-Scan via `Resources.FindObjectsOfTypeAll` → 20s GC-Freeze | `SceneManager.GetActiveScene().GetRootGameObjects()`-Baum-Traversierung |

---

## 4. Chronological Milestone Timeline

Kompakte Historie; Details stehen in den Mod-`CHANGELOG.md`-Dateien und der Git-Historie (Bug-Hunt-Runden 1–3 vollständig dort archiviert).

| Datum | Meilenstein |
|---|---|
| 2026-08-04 | MoreDrugs 1.0.2 — S1API Save-Provider-Verifikation |
| 2026-08-13 | Workspace-Scaffold & IL2CPP-Baseline (S1Mods.sln, Deploy-Targets) |
| 2026-08-14 | Foundation: NotesApp 1.0.0, PotScanner 0.5.0, CalculatorApp 0.2.0, MoreSaveSlots 1.0.1, Shared-Lib, UITheme-Standardisierung |
| 2026-08-16 | PocketShop 0.1.0/0.2.0, CustomSkateboard 1.0.1-Refactor |
| 2026-08-17 | HomelessMod 0.1.0/0.1.1 (7 Golden Rules Outdoor Building), BankApp 0.1.0, BusinessIncome 0.1.0, PocketShop 0.2.1 |
| 2026-08-18 | Minimap 1.0.0/1.0.1 (DayCounter-HUD-Merge, 64-Blip-Pool) |
| 2026-08-19 | Knowledge-Base kuratiert (304+ Analysen), erste 7 AI-Skills |
| 2026-08-20 | CustomSkateboard 1.0.2 (20-Issue-Audit), StackLimitMod 0.1.0, Hybrid-Memory-System, Track-C-Upgrade (zentrales UITheme, deterministische SLN, bump-version, CI, Tests, LICENSE), Bug-Hunt Round 2 (2C+9H+M), Skills-Upgrade Round 2 (Rule 10) |
| 2026-08-21 | Bug-Hunt Round 3 (6 Bugs: UITheme×3, atomic writes, Payout-Snapshot, Slot-Isolation, dead Patch, Sprite-Cache), BackpackMod 1.0.0, Skills Runde 3 (11→14 Skills inkl. economy/persistence/items/interiors/3d-assets/mcp) |
| 2026-08-22 | AutoPackagingStation 0.1.0→0.2.0 (16+ Bugfixes, TextMesh/ClassInjector/IsServer-Gotchas), Outdoor Pack-Up + Dan-Shop-Injection, Kessel-Realism + Save/Load-State-Recovery, 3D-anchored HUD, S1MCP 1.0.1 (Freeze-Fix) |
| 2026-08-23 | AutoPackagingStation 0.2.1 (Output-Extraktion N-Instanzen, Recipe-Multiplier, Native-Slot-Sync, Dismantle-Double-Refund-Schutz, GUID-Sync) |
| 2026-09-04 | **Neuinstallation/Umzug:** Repo in Spiel-Ordner, .NET SDK 8.0.424, S1API 3.2.0-Fork-Build deployed, NotesApp+Shared als Verifikation; Deploy-Konvention json/pdb → `UserData\<Mod>\` |
| 2026-09-09 | BankApp 0.3.0 (Mockup-Redesign: Chip-Grid, Weekly-Limit-Progress, Double-Entry, kein Free-Text-Input mehr → InputFocus entfernt) |
| 2026-09-10 | MEMORY.md-Bereinigung: Duplikate entfernt, Gotchas zusammengeführt, ~94 KB → kompakte Referenz |
| 2026-09-10 | Audit-Fix-Patches (4 HIGH / 19 MEDIUM / 30 LOW verifiziert + gefixt), Patch-Versionen (Mod.cs + mod.json + CHANGELOG), HitmanPhone/_DiagPerfCounter in AGENTS.md + README + DEVELOPERS.md dokumentiert |
| 2026-09-10 | CI-Reparatur: Tools/ (6 Skripte) + Source/Tests/ (82 Tests) aus Historie reaktiviert, Build/Tests game-gated (hosted Runner grün), XRef-Step entfernt, bump-version Regex-Bug (v0.4.6f13-Korruption) + Package-Staging (UserData-Konvention) gefixt |
| 2026-09-10 | PhoneScroll als 3rd-Party: DLL von NexusMods (V4LEXL v1.4) übernommen, auto-Deploy via `Tools/deploy-thirdparty.ps1` + `.deployignore`-Whitelist, Sideload/Hash/S1API/S1MAPI/S1MCP/MoreDrugs explizit ausgeschlossen, hash als deprecated markiert |
| 2026-09-10 | Log-Cleanup: pre-existing `S1API.dll` (415 KB) und `Hash.dll` (188 KB) aus `<GameDir>\Mods\` entfernt (Reference Assemblies, lösten BadImageFormatException-Flood aus). S1MAPI in `UserLibs\` bleibt (AutoPackagingStation.GltfLoader aktiv). |

---

## 5. Pointers

- **Mod-Inventar, Deploy-Status, Versionen:** `AGENTS.md` §2 (Single Source of Truth — hier bewusst NICHT dupliziert).
- **Build/Deploy/CI/Conventions:** `AGENTS.md` §3, §5, §6; `CONTRIBUTING.md`; `DEVELOPERS.md`.
- **Archivierte Bug-Hunt-Detaillogs (Round 1–3):** Git-Historie dieser Datei (`git log -p -- memory/MEMORY.md`).
- **Memory-Protokoll:** lag in `.agents/rules/memory-protocol.md` (Git-Historie, beim Umzug 2026-09 entfernt).
