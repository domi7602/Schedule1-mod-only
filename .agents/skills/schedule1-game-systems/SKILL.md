---
name: schedule1-game-systems
description: "Reference guide for all 64 core game systems in Schedule I (v0.4.6f13). Use this to understand game mechanics, find core classes, and plan hooks."
---

# Schedule I Game Systems

> **Knowledge guard (mod-only):** `Knowledge/` is absent in this workspace. Before using `Knowledge/...` paths, `Test-Path Knowledge/` — fallback is `D:\Backup\game source` (`bundleVersion 0.4.5f2 Alternate`, ~1 version behind `v0.4.6f13`, 66k files, structure-only). Verify any decompile hit against live `Assembly-CSharp.dll` via `ilspycmd` / S1MCP before patching.

This skill is the **map to the 64 game systems** — bypasses raw `Assembly-CSharp` (41 MB / ~2100 files). Every system is already curated in `Knowledge/Game-Reference/Analysis/Systems/` with core classes, events, and hook points.

> **Version check (last verified: 2026-08-21):** Game v0.4.6f13, S1API 3.2.0, 4804 total Knowledge files. If the game patched, re-verify `Systems/` against `Decompiles/Assembly-CSharp/`.

---

## 1. Quick Navigation

### Workflow (30 sec to answer)

1. Open `Knowledge/Game-Reference/Analysis/Systems/_index.md:1` → find System # (e.g. Growing = 08)
2. Read `Knowledge/Game-Reference/Analysis/Systems/08-Plant-Growing.md` → core classes + events + variables
3. Locate class in `Knowledge/Game-Reference/Analysis/Reference/ScheduleOne_CodeTree.md` (namespace index, 1855 files)
4. Open vanilla signature in `Knowledge/Game-Reference/Decompiles/Assembly-CSharp/Il2CppScheduleOne.<System>/` or `verify-snippets/*.txt`
5. Check if S1API wraps it: `Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.<Area>/` — prefer wrapper, fallback to `Il2CppScheduleOne.*` + `PatchGuard`.

> **Do NOT** guess game logic. Always consult the System file before planning a Harmony patch or S1API call.

### System Catalog (see `_index.md:74` categories)

| Category | Systems | Use when |
|---|---|---|
| **Core Gameplay** | 08 Growing, 25 Mixing, 53 ProductSystem, 37 DrugEffects | Pots / soil / additives, mixer recipes, quality thresholds |
| **Economy** | 23 Economy-Money, 11 Business-Laundering, 19 Customer-Budget, 20 XP-Leveling | Cash/bank/balance, business revenue, customer spend |
| **World** | 13 Regions, 43 MapAndLocations, 54 Property, 10 Building-Construction, 35 Doors, 59 TilesGrid | Property ownership, building placement, map POIs |
| **NPCs** | 14 NPC-Behaviour, 38 Employees, 03 Dialogue-Quest, 34 Delivery, 30 Cartel | Daily routines, NavMesh A*, dialogue/quest graphs |
| **Law & Order** | 12 Heat-Pursuit-Law, 51 Police, 30 Cartel, 46 Noise | LE_Intensity, 4 pursuit levels, 16 crimes |
| **Player** | 49 Player, 24 Avatar-CharacterCreator, 09 Inventory-ItemFramework, 39 Equipping, 55 Skateboard, 07 Vehicle, 31 Clothing | Inventory slots/stack, item hierarchy, skateboard physics |
| **UI & Phone** | 06 UI-Phone-HUD, 42 ManagementUI, 22 DevConsole, 40 Interaction | Phone app container, `App<T>`, `UIScreenManager` |
| **Infrastructure** | 01 FishNet-Networking, 02 Save-Persistence, 17 TimeManager, 32 Configuration, 61 Variables, 45 Multiplayer | SyncVars/RPCs, ISaveable, TimeManager.CurTime/ElapsedDays |
| **Environment** | 64 Weather, 41 Lighting, 28 Audio, 58 Temperature, 18 Trash-Recycling | Time-of-day, trash/recycler, temp effects |
| **Misc** | 21 Casino-Gambling, 15 Sewer, 16 Graffiti, 60 TVSystem, 33 Cutscenes, 27 Tutorial-Prolog, 53 ProductSystem | Slots 131.9% RTP, TV apps, sewer goblin |

**Full 64 list:** read `Systems/_index.md:8-72`.

---

## 2. Decision Tree — Which System File?

| Question | Open first | Then |
|---|---|---|
| How does growing work (water/soil/growth)? | `Systems/08-Plant-Growing.md` + `Source-Export/34-Growing-Detail.md` | `Decompiles/verify-snippets/GrowContainer.txt` → `Assembly-CSharp/Il2CppScheduleOne/Growing/` |
| Item stacking / Registry / slots? | `Systems/09-Inventory-ItemFramework.md` + `Source-Export/13-ItemFramework-Detail.md` | `StackLimitMod/src/StackLimitEngine.cs` (live pattern) + `Decompiles/.../ItemFramework/` |
| Property/Business ownership? | `Systems/54-Property.md` + `Source-Export/52-Property-Detail.md` + `11-Business-Laundering.md` | `BusinessIncome/src/Services/BusinessResolver.cs` |
| Building / Grid / outdoor placement? | `Systems/10-Building-Construction.md` + `59-TilesGrid.md` | `schedule1-grid` skill + `HomelessMod/src/Building/` (7 Golden Rules) |
| NPC daily routine / NavMesh? | `Systems/14-NPC-Behaviour.md` + `Source-Export/09-NPCs-Detail.md` | `Concepts/03-NPC-CityLife.md` |
| Save/load when lists are empty? | `Systems/02-Save-Persistence.md` | `schedule1-troubleshooting §5` Save-Load-Timing + `S1API.Lifecycle` |
| Money types & laundering? | `Systems/23-Economy-Money.md` + `11-Business-Laundering.md` | `BankApp/src/Services/BankService.cs` + `S1API.Money` |
| Police / pursuit / heat? | `Systems/12-Heat-Pursuit-Law.md` + `51-Police.md` + `46-Noise.md` | `Decompiles/.../Law/` |
| Phone / HUD layout? | `Systems/06-UI-Phone-HUD.md` | `schedule1-phoneapp` + `Concepts/12-PhoneApp-Mod-Patterns.md` |
| Multiplayer authority? | `Systems/45-Multiplayer.md` + `01-FishNet-Networking.md` | `BusinessIncome/src/Services/IncomeEngine.cs:24 IsHostOrSingleplayer` |
| Time & sleep / curfew / energy? | `Systems/17-TimeManager.md` + `Source-Export/32-GameTime-Detail.md` | `S1API.GameTime` |
| Skateboard physics? | `Systems/55-Skateboard.md` + `07-Vehicle.md` | `CustomSkateboard/src/Items/SkateboardItemFactory.cs` |

For unknown topic: `Concepts/01-19-*.md` → `ThirdParty/<Mod>.md` → `Source-Export/07-ScheduleOne-Namespaces.md`.

---

## 3. Recipes

### A. Find a method signature
```pwsh
# 1. Curated snippet (fast)
Get-Content "Knowledge/Game-Reference/Decompiles/verify-snippets/GrowContainer.txt"
# 2. Full decompile (exact signature)
Select-String -Pattern "class GrowContainer" -Path "Knowledge/Game-Reference/Decompiles/Assembly-CSharp/Il2CppScheduleOne/Growing/*"
# 3. Live verify
& 'C:\Users\pc\.dotnet\tools\ilspycmd.exe' -t Il2CppScheduleOne.Growing.GrowContainer "$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\*.dll"
```

### B. Plan a Harmony patch
```
Systems/08-Plant-Growing.md → core method (e.g. GrowContainer.Water)
→ check S1API wrapper (Frameworks/S1API/Decompiles/3.2.0/S1API.Growing/)
→ if wrapped: use wrapper; else PatchGuard.TryPatch on Il2CppScheduleOne.*
→ verify not field accessor (ilspycmd) → test Latest.log PatchGuard report
```

### C. Add a new gameplay feature (e.g. business income, stacking, phone app)
```
Systems/11 or 09 or 06 → cross-ref Source-Export/<Detail>.md → check ThirdParty/<Mod>.md for pattern
→ study live mod in Source/Mods/<SimilarMod>/ → scaffold via Tools/new-mod.ps1
```

### D. Understand save fields
```
Source-Export/75-Save-Fields.md → 80-Save-JSON-Schema.md → Systems/02-Save-Persistence.md
→ persist via SafeStorage slot_{n}.json + GameLifecycle.OnSaveComplete (see schedule1-modding §3)
```

---

## 4. Source-Export Index (82 files)

| File | Covers | When |
|---|---|---|
| `07-ScheduleOne-Namespaces.md` | 69 namespaces + top classes | Find which namespace owns a class |
| `10-Namespaces-Full-FileList.md` | 1855 .cs files | Exhaustive file list |
| `12-Persistence-Detail.md` | Save hierarchy (227 files) | Save/Load plumbing |
| `13-ItemFramework-Detail.md` | Definitions/Instances/Filters | Stack/inventory mods |
| `14-UI-Detail.md` | 285 UI files, 18 sub-namespaces | Phone/HUD/management UI |
| `34-Growing-Detail.md` | Growing system | Plant/pot mods |
| `52-Property-Detail.md` | Properties | Ownership/business mods |
| `75-Save-Fields.md` | Per-system save fields | Persistence mapping |
| `77-Gameplay-Flows.md` | 12 end-to-end flows | Deal/police/rank/save flows |
| `78-Modding-Workflow.md` | 7 tutorials (NPC/drug/station/save/MP/build) | Onboarding |

See `Source-Export/README.md:20-107` for full 00-80 list.

---

## 5. Cross-Ref to Skills

| Need | Load |
|---|---|
| `Il2CppScheduleOne.*` pitfalls, Harmony, PatchGuard | `schedule1-modding` + `schedule1-troubleshooting` |
| Phone UI / HUD / canvas / lifecycle | `schedule1-phoneapp` |
| S1API namespace for wrapper | `schedule1-s1api` |
| Procedural mesh / building / GLTF / terrain | `schedule1-s1mapi` |
| Find the right KB file | `schedule1-knowledge` |
| Grid/building outdoor placement (7 Golden Rules) | `schedule1-grid` |

---

## 6. Anti-Patterns

* Reading 41 MB `Assembly-CSharp` without first opening `_index.md` + `ScheduleOne_CodeTree.md`.
* Using `Source-Export/01-Scripts.md` 27k file list as primary search — use `07-Namespaces.md` or `_index.md` instead.
* Planning a patch without checking S1API wrapper in `Frameworks/S1API/Decompiles/3.2.0/` — wrapper is update-safe.
* Copying a ThirdParty mod verbatim without checking its `Cross-References` → stale pattern.

---

## 7. Update Hygiene

* After game patch: refresh `Game-Reference/Decompiles/Assembly-CSharp/` + re-run `Source-Export/10-Namespaces-Full-FileList.md` timing.
* If a System file contradicts decompile: fix the System file, update `MEMORY.md`, then decompile.
* Own mods (`PotScanner` growing, `StackLimitMod` stacking) are live references but not yet in `ThirdParty/` — use `Source/Mods/*` directly (gap noted 2026-08-21 in `schedule1-knowledge`).
