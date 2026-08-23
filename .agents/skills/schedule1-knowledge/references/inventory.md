# Knowledge-Base Inventory

The full inventory of the `Knowledge/` tree. **Use this when you don't know where to start** — pick the category that fits your question; jump to the named file.

---

## 1. Frameworks (3rd-Party Libs We Use)

| Path | Files | When to read |
|------|-------|--------------|
| `Frameworks/S1API/3.2.0/` | IL2CPP DLL + Loader plugin (deployed) | API signatures at runtime |
| `Frameworks/S1API/Decompiles/3.2.0/` | 962 source files | **Most complete & current** S1API 3.2.0 surface |
| `Frameworks/S1API/3.1.15/` | IL2CPP DLL + Loader plugin (backup) | Previous runtime reference |
| `Frameworks/S1API/Decompiles/3.1.0/` | 879 source files | Historical 3.1.0 S1API surface |
| `Frameworks/S1API/Decompiles/3.1.6/` | 1 file, single-file-decompile | 3.1.6 fork snapshot |
| `Frameworks/S1API/Decompiles/3.1.9/` | UI snippets | UIFactory / TopBar / ScrollableVerticalList |
| `Frameworks/S1API/VERSIONS.md` | Index | Which version to use when |
| `Frameworks/MelonLoader/` | Single-file-decompile 0.7.3 | MelonLoader internals |

---

## 2. Game-Reference — Decompiles (Vanilla Schedule I Source)

### 2.1. Assembly-CSharp (★ MAIN DECOMPILE: ~2100 files)

`Knowledge/Game-Reference/Decompiles/Assembly-CSharp/` contains the IL2CPP-aware decompile of Schedule I. Organized by namespace:

```
Decompiles/Assembly-CSharp/
├── Properties/
├── Assembly-CSharp.csproj               (for IDE navigation)
├── Il2Cpp                               (interop primitives)
├── Il2CppScheduleOne/                   (★ ~69 sub-namespaces; game core)
│   ├── Audio/ AvatarFramework/ Building/ Calling/ Cartel/ Casino/ Clothing/
│   ├── Combat/ Configuration/ Dialogue/ Doors/ Dragging/ Economy/ Effects/
│   ├── Employees/ EntityFramework/ Equipping/ Events/ Gamepad/ GameTime/
│   ├── Growing/ Heatmap/ Input/ Interaction/ ItemFramework/ Law/ Levelling/
│   ├── Lighting/ Management/ Map/ Materials/ Math/ Messaging/ Misc/ Money/
│   ├── Networking/ NPCs/ Noise/ ObjectScripts/ Packaging/ Persistence/
│   ├── PlayerScripts/ PlayerTasks/ Police/ Polling/ Product/ Property/
│   ├── Quests/ Reporting/ ScriptableObjects/ Skating/ State/ Storage/
│   ├── StationFramework/ Temperature/ Tiles/ Tools/ Trash/ TV/ UI/
│   ├── Variables/ Vehicles/ Vision/ VoiceOver/ Weather/
├── Il2CppScheduleOne.Audio/ ...  (and 100+ sub-namespace directories)
├── Il2CppFishNet/                       (mirror of FishNet networking)
├── Il2CppFishySteamworks/               (Steam transport)
├── Il2CppLiquidVolumeFX/ Il2CppVLB/ ... (3rd-party libs)
```

**Quick-Lookup tips:**
- Search via Grep (`rg "ScheduleOne.Growing" Decompiles/Assembly-CSharp/`).
- Namespace index in `Analysis/Reference/ScheduleOne_CodeTree.md`.
- Game namespace = `Il2CppScheduleOne.<Area>` → looks like e.g. `Il2CppScheduleOne.Growing`.

### 2.2. Sub-Decompiles (mod-specific)

| Path | What |
|------|------|
| `Decompiles/PotScanner-WaterAll/` | GrowContainer, Property, MoneyManager, GameLifecycle, PhoneApp, CustomProductSaveProvider + `verification-report.md` |
| `Decompiles/DrugExpansion/` | Old DrugExpansion 1× decompile (legacy reference) |
| `Decompiles/ConstructionSiteProperty/` | fishproductions' CSM mod decompile |

### 2.3. Verify-Snippets (Pre-Dumped Fulltext)

| File | Size | Content |
|------|------|---------|
| `Decompiles/verify-snippets/GrowContainer.txt` | 80 KB | Full GrowContainer IL2CPP decompile |
| `Decompiles/verify-snippets/gc-full.txt` | large | Game-wide type listing |
| `Decompiles/verify-snippets/s1api-full.txt` | large | S1API full decompile (grep-friendly) |

---

## 3. Game-Reference — Analysis (300+ Curated Files)

### 3.1. Systems (65 files: 01-64 + `_index.md`)

**Index:** `Analysis/Systems/_index.md` — Quick Reference for all 64 systems.

| Category | Files |
|----------|-------|
| **Core Gameplay** | 08-Growing, 25-Mixing, 53-Product, 37-DrugEffects |
| **Economy** | 23-Economy, 11-Business, 19-Customer, 20-XP |
| **World** | 13-Regions, 43-Map, 54-Property, 10-Building, 35-Doors |
| **NPCs** | 14-NPC-Behaviour, 38-Employees, 03-Dialogue, 34-Delivery |
| **Law/Order** | 12-Heat-Pursuit, 51-Police, 30-Cartel, 46-Noise |
| **Player** | 49-Player, 24-Avatar, 09-Inventory, 39-Equipping, 55-Skateboard |
| **UI/Phone** | 06-UI-Phone-HUD, 42-Management, 22-DevConsole |
| **Infrastructure** | 01-Networking, 02-Save, 17-Time, 32-Config, 61-Variables |
| **Environment** | 64-Weather, 41-Lighting, 28-Audio, 58-Temperature, 18-Trash |
| **Misc** | 21-Casino, 15-Sewer, 16-Graffiti, 60-TV, 33-Cutscenes, 27-Tutorial |

### 3.2. Source-Export (82 files: 00-80 + README)

The Mono-branch AssetRipper export. Files 01-19 describe folders (Scripts, Assets, Assemblies, Cpp2il-out, …). Files 20+ are detailed per-namespace inventories (Growing-Detail, Map-Detail, …) — great for finding exact field names and sub-types.

| File | Scope | Use it for |
|------|-------|------------|
| `00-Folder-Map.md` | Top-level overview | First-stop orientation |
| `01-Scripts.md` | 27k files, 187 DLLs | Find which DLL holds a class |
| `07-ScheduleOne-Namespaces.md` | 69 game namespaces | Namespace → contents |
| `10-Namespaces-Full-FileList.md` | All 1,855 ScheduleOne files | File → namespace mapping |
| `12-Persistence-Detail.md` | Save system | Save/load API |
| `13-ItemFramework-Detail.md` | Items, inventory | Item hierarchy |
| `34-Growing-Detail.md` | Plant growth | GrowContainer, Plant, Soil |
| `75-Save-Fields.md` | Save-Data classes | Which fields persist |
| `80-Save-JSON-Schema.md` | JSON format | Save format reverse-engineering |

### 3.3. Learnings (138 files: 17 Concepts + 121 ThirdParty)

**Index:** `Analysis/Learnings/README.md`

- **`Concepts/01-19-…md`** (17 active, 06/07 removed): Thematic key learnings, e.g.
  - 01-IL2CPP-MelonLoader-Patterns
  - 12-PhoneApp-Mod-Patterns (canonical PhoneApp architecture)
  - 05-Modding-Workflow-Build-Automation
  - 17-Multi-Mod-Integration-Connectors

- **`ThirdParty/<ModName>.md`**: Per-mod analysis with metadata, architecture, key files, patterns, lessons.
  - **121 mods cataloged.** Categories: APIs, Building, Lighting, Character, Crafting, Debug, Vehicles, NPCs, Storage, Police, Economics, Casino, Drugs, Quests.
  - Use the catalog in `Learnings/README.md` to find similar mods.

### 3.4. APIs (10 files)

**Index:** `Analysis/APIs/README.md`

| Framework | Category | Use for |
|-----------|----------|---------|
| **Il2CppInterop** | IL2CPP foundation | `TryCast<T>`, class injection, base class |
| **S1API** | Game systems + Lifecycle | Cross-runtime modding framework |
| **S1MAPI** | Map + Building | Map editor, NPC NavMesh, furniture |
| **MeshVault** | 3D meshes + Assets | Central mesh catalog for mods |
| **TextureSwapper** | Materials + Textures | Runtime texture replacement |
| **Lithium** | Modular framework | 20 submodules + ModuleBase |
| **PhantomLink** | Runtime discovery | WebSocket/HTTP IPC server |
| **KowyxUI** | UI library | Panel/Card/Anchor without MelonMod |
| **OverTheCounter Loader** | Framework loader | Plugin dependency checker |

The `/IL2CPP/` directory **also** has 4 IL2CPP files (`IL2CPP-Interop-Rules.md`, `IL2CPP-Delegates-Events.md`, `IL2CPP-Generics-Collections.md`, `IL2CPP-Debugging-Diagnostics.md`). These are referenced from `APIs/README.md` rule list — use this directory for IL2CPP-specific deep dives.

### 3.5. Guides (5 files)

| File | Purpose |
|------|---------|
| `01-Modding-Fundamentals.md` | Modding quickstart |
| `02-Game-Source-Code-Reference.md` | Where to find code |
| `03-Mod-Project-Workflow.md` | Project setup |
| `04-Mod-Template.md` | Canonical C# class template for Mod.cs & PhoneApp |
| `Definitive-S1-Guide.md` | 2142-line community guide (Scott/Noc). Gameplay mechanics, mixing recipes, employee ratios. NOT modding-specific. |

### 3.6. Reference (1 file)

`Reference/ScheduleOne_CodeTree.md` — Hierarchical listing of all `Il2CppScheduleOne` namespaces and their classes (largest doc in the workspace, ~2000+ lines). Use as a "directory" when grep/jumping.

---

## 4. Maps/ (Hyland-Point Map Material)

Per-district map data. Useful for location-aware mods (POIs, region-locked content, distance-tagging).

| Path | Coverage |
|------|----------|
| `Maps/Hyland_Point_Guide.md` | Overview |
| `Maps/hyland_point_map.json` | Programmable map |
| `Maps/Docks/` | Docks district |
| `Maps/Downtown/` | Downtown district |
| `Maps/Northtown/` | Northtown district |
| `Maps/Suburbia/` | Suburbia district |
| `Maps/The Sewers/` | Sewers (Cartel / fight arena) |
| `Maps/Uptown/` | Uptown district |
| `Maps/Westville/` | Westville district |
| `Maps/rescan.py` | Regenerate map data |

---

## 5. Tools/ (External Tools)

| Path | Purpose |
|------|---------|
| `Tools/s1-error-analyzer/` | TVGS open-source error analyzer (8.8 MB) — game error decoding. |
| `Tools/S1Interop/` | Mod-Interop analysis tool — CLI `s1interop {doctor\|analyze\|lint\|migrate\|setup}`. |
| `Tools/S1NotesApp-Reference/` | Original `S1NotesApp` (pre-namespace refactor) — historical reference. |

---

## Quick-Reference: "I need to …"

- **Find a game class's full path** → `Analysis/Source-Export/07-ScheduleOne-Namespaces.md`
- **Look at the IL2CPP proxy of a vanilla class** → `Decompiles/Assembly-CSharp/Il2CppScheduleOne/<...>`
- **Understand a game's save format** → `Analysis/Source-Export/75-Save-Fields.md` + `80-Save-JSON-Schema.md`
- **Find an existing mod that already does X** → `Analysis/Learnings/ThirdParty/<X>.md`
- **Pick an API for a feature** → `Analysis/APIs/README.md` over the table.
- **Find IL2CPP rule X** → `Analysis/APIs/Il2CppInterop.md` (overview) + `Analysis/IL2CPP/IL2CPP-Interop-Rules.md` (deep)
- **Check a S1API version's API surface** → `Frameworks/S1API/VERSIONS.md` first, then `Decompiles/3.1.0/`.
- **Get a map location** → `Maps/Hyland_Point_Guide.md` + `hyland_point_map.json`.
