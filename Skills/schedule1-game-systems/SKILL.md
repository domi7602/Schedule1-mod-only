---
name: schedule1-game-systems
description: "Reference guide for all 64 core game systems in Schedule I (v0.4.6f13). Use this to understand game mechanics, find core classes, and plan hooks."
---

> Version anchor: Game v0.4.6f13 / S1API 3.2.0 / MelonLoader 0.7.3 (verified 2026-09-03). Re-check after any game or S1API update.

# Schedule I Game Systems

This skill is the **map to the 64 core game systems** in *Schedule I* v0.4.6f13 — bypassing the need to navigate raw decompiled code blind. Every system is curated with core classes, network behavior, variables, and Harmony/S1API hook points directly in `Skills/schedule1-game-systems/references/`.

---

## 1. Quick Navigation

### Workflow (30 sec to answer)

1. Open `Skills/schedule1-game-systems/references/_index.md` → find System # (e.g. Growing = 08)
2. Read `Skills/schedule1-game-systems/references/08-Plant-Growing.md` → core classes + events + variables
3. Open vanilla class in `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/<System>/`
4. Check if S1API wraps it: `ThirdParty/S1API/S1API/<Area>/` — prefer S1API wrapper, fallback to `Il2CppScheduleOne.*` + `PatchGuard`.
5. Verify live signatures using `ilspycmd` or S1MCP (:8765) before patching.

> **Do NOT** guess game logic. Always consult the System reference file before planning a Harmony patch or S1API call.

### System Catalog (see `_index.md` categories)

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

**Full 64 list:** read `Skills/schedule1-game-systems/references/_index.md`.

---

## 2. Decision Tree — Which System File?

| Question | Open first | Then |
|---|---|---|
| How does growing work (water/soil/growth)? | `references/08-Plant-Growing.md` | `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/Growing/` |
| Item stacking / Registry / slots? | `references/09-Inventory-ItemFramework.md` | `Source/Mods/StackLimitMod/` (live pattern) + `GameReferences/.../ItemFramework/` |
| Property/Business ownership? | `references/54-Property.md` + `references/11-Business-Laundering.md` | `Source/Mods/BusinessIncome/src/BusinessResolver.cs` |
| Building / Grid / outdoor placement? | `references/10-Building-Construction.md` + `references/59-TilesGrid.md` | `schedule1-grid` skill + `Source/Mods/HomelessMod/src/` (7 Golden Rules) |
| NPC daily routine / NavMesh? | `references/14-NPC-Behaviour.md` | `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/NPCs/` |
| Save/load & slot isolation? | `references/02-Save-Persistence.md` | `schedule1-persistence` + `schedule1-troubleshooting` |
| Money types & laundering? | `references/23-Economy-Money.md` + `references/11-Business-Laundering.md` | `Source/Mods/BankApp/` + `ThirdParty/S1API/S1API/Money/` |
| Police / pursuit / heat? | `references/12-Heat-Pursuit-Law.md` + `references/51-Police.md` | `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/Law/` |
| Phone / HUD layout? | `references/06-UI-Phone-HUD.md` | `schedule1-phoneapp` skill |
| Multiplayer authority? | `references/45-Multiplayer.md` + `references/01-FishNet-Networking.md` | `Source/Mods/BusinessIncome/` (`NetworkGuard.IsHostOrSingleplayer()`) |
| Time & sleep / curfew / energy? | `references/17-TimeManager.md` | `ThirdParty/S1API/S1API/GameTime/` |
| Skateboard physics? | `references/55-Skateboard.md` + `references/07-Vehicle.md` | `Source/Mods/CustomSkateboard/` |

---

## 3. Recipes

### A. Find a method signature
```pwsh
# 1. Search in-repo decompile
rg "class GrowContainer" "GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/Growing"

# 2. Live verify against game assemblies
& (Get-Command ilspycmd -ErrorAction Stop).Source -t Il2CppScheduleOne.Growing.GrowContainer "$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll"
# If not installed: dotnet tool install -g ilspycmd
```

### B. Plan a Harmony patch
```
references/08-Plant-Growing.md → core method (e.g. GrowContainer.Water)
→ check S1API wrapper in ThirdParty/S1API/S1API/Growing/
→ if wrapped: use wrapper; else PatchGuard.TryPatch on Il2CppScheduleOne.*
→ verify not field accessor (ilspycmd) → test Latest.log PatchGuard report
```

### C. Add a new gameplay feature
```
references/<System>.md → check live mod in Source/Mods/<SimilarMod>/
→ scaffold via Tools/new-mod.ps1
```

### D. Understand save fields
```
references/02-Save-Persistence.md
→ persist via SafeStorage slot_{n}.json + GameLifecycle.OnSaveComplete (see schedule1-persistence §1)
```

---

## 4. Cross-Ref to Skills

| Need | Load |
|---|---|
| `Il2CppScheduleOne.*` pitfalls, Harmony, PatchGuard | `schedule1-modding` + `schedule1-troubleshooting` |
| Phone UI / HUD / canvas / lifecycle | `schedule1-phoneapp` |
| S1API namespace for wrapper | `schedule1-s1api` |
| Procedural mesh / building / GLTF / terrain | `schedule1-s1mapi` |
| Find the right KB file | `schedule1-knowledge` |
| Grid/building outdoor placement (7 Golden Rules) | `schedule1-grid` |
| Economy, Money, Shop multi-payment | `schedule1-economy` |
| Persistence, SafeStorage atomic, Slot isolation | `schedule1-persistence` |

---

## 5. Anti-Patterns

* Navigating the 41 MB `Assembly-CSharp` blindly without checking `_index.md` first.
* Planning a patch without checking S1API wrapper in `ThirdParty/S1API/S1API/` — wrappers are update-safe.
* Calling methods on IL2CPP Unity objects without `WasCollected` guards.
* Writing save data without slot-isolation (`info.SaveSlotNumber >= 0`).

---

## 6. Update Hygiene

* After game patch: verify changed classes via `ilspycmd` against `<GameDir>/MelonLoader/Il2CppAssemblies/`.
* If a System file contradicts decompile: update the respective markdown file in `references/`.
* Own mods in `Source/Mods/` serve as canonical reference implementations.
