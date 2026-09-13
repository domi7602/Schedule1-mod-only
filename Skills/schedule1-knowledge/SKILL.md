---
name: schedule1-knowledge
description: >-
  Research guide for the Schedule I Modding Knowledge Base, including in-repo decompiles (GameReferences/), all 64 system analyses (Skills/schedule1-game-systems/references/), S1API source (ThirdParty/S1API/), and active mod code.
  Use this skill whenever you need to find a vanilla game class/method/API, understand a game's system architecture, learn an IL2CPP/S1API rule, study an existing mod's pattern, or navigate the workspace's decompiles + analysis efficiently.
  Keywords: Decompiles, Assembly-CSharp, Il2CppScheduleOne, GameReferences, Game Systems, S1API, S1MAPI, ilspycmd, research.
---

> Version anchor: Game v0.4.6f13 / S1API 3.2.0 / MelonLoader 0.7.3 (verified 2026-09-12). Re-check after any game or S1API update.

# Schedule I — Knowledge Navigation Skill

This skill guides you through the curated knowledge resources available directly within the workspace and live game installation. Use this guide to locate symbols, methods, and system architectures in seconds instead of searching blindly.

---

## 1. Knowledge Map (In-Repo Orientation)

```text
Schedule1-mod-only-main/
├── GameReferences/
│   └── decompiled/Assembly-CSharp/
│       ├── Assembly-CSharp.csproj           (IDE navigation)
│       └── Il2CppScheduleOne/               (★ Primary game core: 78 sub-namespaces)
│           ├── Audio/ AvatarFramework/ Building/ Calling/ Cartel/ Casino/ Clothing/
│           ├── Combat/ Configuration/ Dialogue/ Doors/ Dragging/ Economy/ Effects/
│           ├── Employees/ EntityFramework/ Equipping/ Events/ Gamepad/ GameTime/
│           ├── Growing/ Heatmap/ Input/ Interaction/ ItemFramework/ Law/ Levelling/
│           ├── Lighting/ Management/ Map/ Materials/ Math/ Messaging/ Misc/ Money/
│           ├── Networking/ NPCs/ Noise/ ObjectScripts/ Packaging/ Persistence/
│           ├── PlayerScripts/ PlayerTasks/ Police/ Polling/ Product/ Property/
│           ├── Quests/ Reporting/ ScriptableObjects/ Skating/ State/ Storage/
│           ├── StationFramework/ Temperature/ Tiles/ Tools/ Trash/ TV/ UI/
│           └── Variables/ Vehicles/ Vision/ VoiceOver/ Weather/
│
├── Skills/
│   ├── schedule1-game-systems/references/   (★ All 64 system analyses: 01-FishNet to 64-Weather + _index.md)
│   ├── schedule1-modding/references/        (Build, deploy, architecture, mod patterns)
│   ├── schedule1-s1api/references/          (S1API subsystem deep-dives)
│   └── schedule1-phoneapp/references/       (Method 3 responsive UI, input focus, lifecycle)
│
├── ThirdParty/
│   ├── S1API/S1API/                         (Full C# source code of S1API 3.2.0)
│   ├── S1MAPI/                              (S1MAPI 2.0.0 mapping framework)
│   ├── PhoneScroll/                         (Phone scroll hook by V4LEXL)
│   └── S1MCPServer-master/                  (S1MCP protocol bridge source)
│
└── Source/Mods/                             (13 active mod implementations: NotesApp, BankApp, PotScanner, HomelessMod, etc.)
```

> **Live Inspection:** Always verify method signatures against the live game proxies in `$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll` using `ilspycmd`.

---

## 2. The First 60 Seconds: Decision Tree

When you get a question, run this mental check:

| Question | File / Location to open first | Then |
|---|---|---|
| What's the API signature of `GrowContainer.SetMoistureAmount`? | `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/Growing/GrowContainer.cs` | Verify live DLL via `ilspycmd` |
| What does S1API 3.2.0 expose for Phone Apps? | `Skills/schedule1-phoneapp/SKILL.md` + `ThirdParty/S1API/S1API/PhoneApp/` | `Source/Mods/NotesApp/` |
| How does ModX achieve Y? | `Source/Mods/<ModName>/src/*.cs` (active workspace mods) | `AGENTS.md` (mod inventory) |
| What are IL2CPP pitfalls? | `Skills/schedule1-il2cpp-reflection/SKILL.md` + `schedule1-troubleshooting` | `Source/Mods/Shared/` |
| Which class is responsible for Save/Load? | `Skills/schedule1-game-systems/references/02-Save-Persistence.md` | `GameReferences/decompiled/.../Persistence/` |
| What is the screen layout / phone / HUD? | `Skills/schedule1-game-systems/references/06-UI-Phone-HUD.md` | `Skills/schedule1-phoneapp/` |
| How does growing work (water/soil/growth)? | `Skills/schedule1-game-systems/references/08-Plant-Growing.md` | `Source/Mods/PotScanner/` |
| Item stacking / Registry / slots? | `Skills/schedule1-items/SKILL.md` + `references/09-Inventory-ItemFramework.md` | `Source/Mods/StackLimitMod/` |
| How to handle multi-payment (Cash/Bank) or economy? | `Skills/schedule1-economy/SKILL.md` | `Source/Mods/PocketShop/` + `Source/Mods/BankApp/` |
| How to build a Minimap / Radar / Blips? | `Source/Mods/Minimap/` | `Skills/schedule1-modding/references/mod-patterns.md` |
| How to do Multiplayer Host Authority or passive income? | `Source/Mods/BusinessIncome/` | `Skills/schedule1-economy/references/passive-revenue.md` |
| How to implement Everywhere Building / Procedural 3D? | `Source/Mods/HomelessMod/` | `Skills/schedule1-grid/` |

---

## 3. Top Sources by Frequency

| Ranking | Source | Location | Speed | Use case |
|---|---|---|---|---|
| **#1** | **Game Systems (64 systems)** | `Skills/schedule1-game-systems/references/` | ⚡ instant | Game architecture, systems, events, UI patterns |
| **#2** | **Vanilla Decompiled Source** | `GameReferences/decompiled/Assembly-CSharp/` | 🔍 grep/targeted | Exact class fields, signatures, enums, game logic |
| **#3** | **S1API Full Source** | `ThirdParty/S1API/S1API/` | 🔍 targeted | S1API wrapper methods, event hooks, UI builders |
| **#4** | **Active Workspace Mods** | `Source/Mods/<Mod>/` | ⚡ instant | Production-tested patterns, SafeStorage, Method 3 UI |
| **#5** | **Live Proxy Assemblies** | `$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\` | 🔍 `ilspycmd` | Runtime ground truth for all method signatures |

---

## 4. Safe Research Recipes (PowerShell)

A. **Find a method signature in vanilla game:**
   ```pwsh
   # Search decompiles in the repository:
   Select-String -Pattern "class GrowContainer" -Path "GameReferences\decompiled\Assembly-CSharp\Il2CppScheduleOne\Growing\*"
   # Or verify live against runtime proxies:
   & (Get-Command ilspycmd -ErrorAction Stop).Source -t Il2CppScheduleOne.Growing.GrowContainer "$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll"
   ```

B. **Find how an active workspace mod implemented a feature:**
   ```pwsh
   # Search across workspace mods:
   Select-String -Pattern "SaveAtomic" -Path "Source\Mods\*\src\*.cs"
   ```

C. **Check a specific game system:**
   - Open `Skills/schedule1-game-systems/references/_index.md`
   - Read the matching system document (e.g. `references/08-Plant-Growing.md` or `references/23-Economy-Money.md`)

D. **Inspect S1API wrappers:**
   - Check `ThirdParty/S1API/S1API/` for wrappers before using raw `Il2CppScheduleOne.*`.

---

## 5. Anti-Patterns

* **Grepping the entire disk without index-lookup first.** Wasteful — start at `_index.md`.
* **Assuming methods can be patched without checking.** Always verify with `ilspycmd` whether a method is an IL2CPP field accessor (`get_DefaultStackLimit` cannot be patched).
* **Missing WasCollected checks.** IL2CPP objects can be destroyed natively while C# references remain non-null. Always check `obj != null && obj.Pointer != IntPtr.Zero && !obj.WasCollected`.
* **Writing save files without slot isolation.** Always check `SaveSlotNumber >= 0` and suffix per-save files with `slot_{SaveSlotNumber}.json`.

---

## 6. Knowledge State & Anchors

* Workspace: `C:\Program Files (x86)\Steam\steamapps\common\Schedule I\Schedule1-mod-only-main`
* Game Version: `v0.4.6f13` (Unity 2022.3, IL2CPP).
* S1API: `3.2.0` deployed (`Plugins\S1APILoader.dll` + `Mods\S1API.Il2Cpp.MelonLoader.dll`).
* S1MAPI: `2.0.0` in `ThirdParty/S1MAPI/`.
* Active Mods: 13 mods in `Source/Mods/` (NotesApp, PotScanner, CalculatorApp, CustomSkateboard, MoreSaveSlots, PocketShop, BankApp, HomelessMod, BusinessIncome, Minimap, StackLimitMod, BackpackMod, AutoPackagingStation, HitmanPhone).
* All 64 game systems fully documented in `Skills/schedule1-game-systems/references/`.

---

## 7. Framework Skills (Cross-Reference)

| Skill | Use when |
|---|---|
| `schedule1-s1api` | Need a S1API namespace/class for a task (Saveables, PhoneApp, Quests, NPCs, Items, Money, GameTime, Lifecycle) |
| `schedule1-s1mapi` | Need procedural meshes, building construction, GLTF/GLB loading, terrain clearing, NPC navigation, or prefab placement |
| `schedule1-modding` | Primary mod development runbook, build/deploy, architecture rules, SafeStorage |
| `schedule1-troubleshooting` | Diagnostic runbook, Latest.log parsing, crash analysis, IL2CPP pitfalls |
