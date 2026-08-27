---
name: schedule1-knowledge
description: >-
  Research guide for the Schedule I Modding Knowledge Base at C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge (304+ files).
  Use this skill whenever you need to find a vanilla game class/method/API, understand a game's system architecture, learn an IL2CPP/S1API/3rd-party-framework rule, study an existing mod's pattern, or navigate the workspace's decompiles + analysis + frameworks folders efficiently.
  Covers: Decompiles (Assembly-CSharp 41MB + verify-snippets), Analysis (Systems 65 / Source-Export 82 / Learnings 138 / APIs 10 / Guides / IL2CPP / Reference), Frameworks, Maps, Templates, Tools.
---

# Schedule I — Knowledge Navigation Skill

> **Knowledge guard (mod-only):** `Knowledge/` is absent in this workspace. Before using `Knowledge/...` paths, `Test-Path Knowledge/` — fallback is `D:\Backup\game source` (`bundleVersion 0.4.5f2 Alternate`, ~1 version behind `v0.4.6f13`, 66k files, structure-only). Verify any decompile hit against live `Assembly-CSharp.dll` via `ilspycmd` / S1MCP before patching.

The workspace ships an extensive knowledge base (**~304 curated analyses + 41 MB vanilla decompile + 962-file S1API decompile + 4804 total files 67 MB**) carefully indexed. Use this guide to find the right file in seconds instead of grepping blind.

---

## 1. Knowledge-Base Map (Quick Orientation)

```
Knowledge/
├── Frameworks/                          (3rd-party libs we use/depend on)
│   ├── S1API/{3.2.0, 3.1.15, Decompiles/3.2.0/3.1.0/3.1.6/3.1.9/, VERSIONS.md}
│   └── MelonLoader/                     (single-file decompile 0.7.3)
│
├── Game-Reference/                      (everything about Schedule I itself)
│   ├── Decompiles/
│   │   ├── Assembly-CSharp/             (★ MAIN: ~2100 files, Il2CppScheduleOne.*)
│   │   ├── PotScanner-WaterAll/         (sub-decompiles + verification-report.md)
│   │   ├── verify-snippets/             (gc-full.txt, GrowContainer.txt, s1api-full.txt)
│   │   ├── DrugExpansion/               (1× decompiled mod)
│   │   └── ConstructionSiteProperty/    (1× decompiled mod)
│   │
│   ├── Maps/                            (Hyland-Point per-district map material)
│   └── Analysis/                        (300+ curated analysis files)
│       ├── Systems/                     (65 files: 01-64 + _index.md)
│       ├── Source-Export/               (82 files: 00-80 + README)
│       ├── Learnings/
│       │   ├── Concepts/                (19 thematic pattern docs)
│       │   └── ThirdParty/              (121 per-mod analyses)
│       ├── APIs/                        (10 framework docs + 4 IL2CPP rules)
│       ├── Guides/                      (4 step-by-step guides incl. 04-Mod-Template)
│       ├── IL2CPP/                      (4 IL2CPP technical files — duplicate of APIs/; prefer APIs/)
│       └── Reference/                   (ScheduleOne_CodeTree.md — namespace index)
│
└── Tools/                               (external tool sources: S1Interop, s1-error-analyzer, S1NotesApp-Ref)
```

> **Total searchable surface: ~304 curated analyses + ~2100 vanilla decompiled files + 962 S1API 3.2.0 files + 4804 total files (67 MB) indexed 2026-08-21.**

---

## 2. The First 60 Seconds: Decision Tree

When you get a question, run this mental check:

| Question | File to open first | Then |
|----------|--------------------|------|
| What's the API signature of `GrowContainer.SetMoistureAmount`? | `Decompiles/verify-snippets/GrowContainer.txt` | `Decompiles/Assembly-CSharp/Il2CppScheduleOne/Growing/GrowContainer*.cs` |
| What does S1API 3.2.0 expose for Phone Apps? | `Analysis/APIs/S1API.md` §5.4 | `Frameworks/S1API/Decompiles/3.2.0/` |
| How does ModX achieve Y? | `Analysis/Learnings/ThirdParty/<ModName>.md` (⚠️ 121 files — own mods not yet mirrored; use `Source/Mods/<Mod>/src/*.cs` directly for NotesApp/PotScanner/CalculatorApp etc.) | `Analysis/Learnings/README.md` (catalog index) |
| What are IL2CPP pitfalls? | `Analysis/APIs/Il2CppInterop.md` + `Analysis/IL2CPP/IL2CPP-Interop-Rules.md` | `Analysis/Learnings/Concepts/01-IL2CPP-MelonLoader-Patterns.md` |
| Which class is responsible for Save/Load? | `Analysis/Systems/02-Save-Persistence.md` | `Decompiles/Assembly-CSharp/Il2CppScheduleOne/Persistence/` |
| What is the screen layout / phone / HUD? | `Analysis/Systems/06-UI-Phone-HUD.md` | `Analysis/APIs/S1API.md` (UIFactory + PhoneApp) |
| What does a "PhoneApp Mod" look like architecturally? | `Analysis/Learnings/Concepts/12-PhoneApp-Mod-Patterns.md` | `Sources/Mods/<own-mod>/` (e.g. `PocketShop`, `BankApp`) |
| Which namespace contains `<ClassName>`? | `Analysis/Reference/ScheduleOne_CodeTree.md` | `Analysis/Source-Export/07-ScheduleOne-Namespaces.md` |
| Build a system involving quests / NPCs / dealers? | `Analysis/Systems/14-NPC-Behaviour.md` + `03-Dialogue-Quest.md` | S1API examples in `Frameworks/S1API/Decompiles/3.2.0/` |
| How to handle multi-payment (Cash/Bank) or economy? | `Sources/Mods/PocketShop/` | `Sources/Mods/BankApp/` |
| How to build a Minimap / Radar / Blips? | `Sources/Mods/Minimap/` | `Analysis/Learnings/ThirdParty/Minimap.md` |
| How to do Multiplayer Host Authority or passive income? | `Sources/Mods/BusinessIncome/` | `Analysis/Systems/18-Multiplayer.md` |
| How to implement Everywhere Building / Procedural 3D? | `Sources/Mods/HomelessMod/` | `Analysis/Learnings/ThirdParty/HomelessMod.md` |

For **unfamiliar / unusual topics** without a direct System file, start at `Analysis/Learnings/Concepts/01-19-…` (19 thematic key learnings), then look up specific mod examples in `Learnings/ThirdParty/`.

For an **external framework** (Sideload, hash, MeshVault, …) start at `Analysis/APIs/<FrameworkName>.md`.

---

## 3. Top Sources by Frequency

| Ranking | Source | Files | Speed | Use case |
|---|---|---|---|---|
| **#1** | `Analysis/Systems/` | 65 | ⚡ instant | Game architecture, systems, events, UI patterns |
| **#2** | `Decompiles/Assembly-CSharp/` | ~2100 | 🔍 grep/targeted | Method signatures, exact field types, enum values |
| **#3** | `Analysis/Learnings/Concepts/` | 17 | ⚡ instant | Cross-cutting modding patterns (IL2CPP, Polling, Save) |
| **#4** | `Analysis/Learnings/ThirdParty/` | 121 | ⚡ instant | How other modders solved specific problems |
| **#5** | `Frameworks/S1API/Decompiles/3.2.0/` | 962 | 🔍 grep | S1API wrapper methods, event hooks, UI builders |
| **#6** | `Analysis/Source-Export/` | 82 | ⚡ instant | High-level namespace & class overviews (Mono-branch AssetRipper export) |
| **#7** | `Analysis/Reference/ScheduleOne_CodeTree.md` | 1 (large) | ⚡ instant | Namespace index for `Il2CppScheduleOne.*` (~2000+ lines, class hierarchy) |
| **#8** | `Analysis/APIs/` | 10 | ⚡ instant | Framework & external library reference (Il2CppInterop, S1API, Sideload, hash, …) |
| **#9** | `Frameworks/MelonLoader/` | 1 (single-file) | 🔍 grep | MelonLoader 0.7.3 internals (Lifecycle, Patches, Reflection) |
| **#10** | `Frameworks/S1API/VERSIONS.md` | 1 | ⚡ instant | Which S1API version to use when (3.2.0 deployed, 3.1.15 backup) |

---

## 4. Safe Research Recipes

A. **Find a method signature in vanilla game.**
   ```pwsh
   # Fast pattern: targeted file search
   grep -rn "class GrowContainer" "Knowledge/Game-Reference/Decompiles/Assembly-CSharp"
   # Or read the curated snippet first:
   cat "Knowledge/Game-Reference/Decompiles/verify-snippets/GrowContainer.txt"
   ```

B. **Find how another mod implemented a feature.**
   ```pwsh
   grep -rn "Saveable" "Knowledge/Game-Reference/Analysis/Learnings/ThirdParty/"
   ```

C. **Check an IL2CPP rule.**
   - Open `Knowledge/Game-Reference/Analysis/APIs/Il2CppInterop.md`
   - Key topics: `[RegisterTypeInIl2Cpp]`, `Il2CppSystem.Collections.Generic.List<T>`, delegate lifecycles

D. **Cross-reference.** Check whether S1API already wraps the API (`Analysis/APIs/S1API.md`). If yes → prefer the wrapper; if no → use direct IL2CPP access via the patterns in `il2cpp-harmony-guide.md`.

E. **Check the testing history.** Search `Analysis/Learnings/ThirdParty/` for similar mods — someone probably already cracked the same problem (see catalog in `references/inventory.md` §3).

F. **Decompile a third-party mod for pattern mining** (when no `ThirdParty/<ModName>.md` exists yet).
   ```pwsh
   # Triage first: du -h <mod>.dll. >50 MB = asset-bomb, skip. <1 MB with 5–30 files = high-yield.
   mkdir -p .scratch/mod-decompile/_decompiled/<ModName>
   ilspycmd "C:/Users/.../<ModName>.dll" -o ".scratch/mod-decompile/_decompiled/<ModName>" -p
   # Read the entry-point file first (Core.cs / ModEntry.cs / <Name>Mod.cs), then
   # follow namespace trails into the utility/integration folders.
   ```
   Output lives under `.scratch/mod-decompile/_decompiled/` — local scratch, **not committed**. The "update available" nag at the top of `ilspycmd` output is advertising; ignore. For workflow + triage heuristics see `schedule1-modding` §2.F.

---

## 5. Anti-Patterns

* **Grepping recursively without index-lookup first.** Wasteful — start at `_index.md`.
* **Reading the raw 41 MB Assembly-CSharp dump.** Use targeted `ilspycmd -t <TypeName>` instead. Reserve grep for unknown namespaces.
* **Re-decoding S1API from `Decompiles/3.1.0/` every time.** Use `Frameworks/S1API/Decompiles/3.2.0/` + `VERSIONS.md` for installed-version reality.
* **Reading the full `Decompiles/Analysis/Systems/64-Weather.md` if you only need the `Weather` class API.** Read header, jump to method list, decompile only the needed type.
* **Creating a mod without checking `Learnings/ThirdParty/` for a similar project.** Reinventing the wheel.

---

## 6. Knowledge State & Update Hygiene

* As of: **2026-08-21** (per `Knowledge/README.md`; verified via `Get-ChildItem -Recurse` 4804 files / 67 MB).
* Game: v0.4.6f13 (Unity 2022.3.62f2, IL2CPP).
* S1API: 3.2.0 deployed; full decompile in `Decompiles/3.2.0/` (962 files); historical 3.1.0 / 3.1.6 / 3.1.9 / 3.1.15 also indexed.
* S1MAPI: 2.0.0 deployed as `UserLibs\S1MAPI_Il2Cpp.dll`; see `Knowledge/Analysis/APIs/S1MAPI.md` and `Knowledge/Analysis/Learnings/ThirdParty/S1MAPI_Il2Cpp.md`.
* **Own mods not yet in ThirdParty/ (gap 2026-08-21):** 121 ThirdParty files cover Fremdmods; own mods (`NotesApp`, `PotScanner`, `CalculatorApp`, `CustomSkateboard`, `MoreSaveSlots`, `PocketShop`, `BankApp`, `HomelessMod`, `Minimap`, `StackLimitMod`, `BusinessIncome`, `Shared`) live only in `Source/Mods/*` — mirror them as `<Mod>.md` after each release.
* When you find a contradiction between docs and reality: **fix the doc, not the code.** Update the entry in MEMORY.md first, then AGENTS.md, then the knowledge file. See `~/.config/opencode/rules/active_learning.md`.
* **Lifecycle ground-truth (2026-08-20):** `S1API.Internal.Patches/HomeScreen_Start_Patch.cs` instantiates each `PhoneApp` ONCE per scene → `OnCreated` fires once, `OnPhoneClosed` fires every close. Do NOT unsubscribe `MelonEvents.OnUpdate` in `OnPhoneClosed`. (Bug-Hunt Round 2 root cause for all 5 phone apps.)
* **Slot-Isolation ground-truth (2026-08-21):** `CalculatorState.cs:66` global file → slot leak → fixed `slot_{n}.json` + `TryMigrateLegacy` (`CalculatorState.cs:65-93`). Same for `NotesApp`, `BankApp`, `BusinessIncome`, `HomelessMod`.
* **Build hygiene (2026-08-21):** Field accessor `BaseItemDefinition.get_DefaultStackLimit` not patchable (`Latest.log:17:43:04.438`); `SafeStorage.SaveTextAtomic` mandatory for `Game.json` renames; `Dictionary<string,Sprite>` keyed caches for `size/radius/thickness`.

---

## 7. Framework Skills (Cross-Reference)

Two of the 6 workspace skills are framework-specific reference catalogs. Use them when you need the **author's curated API map** instead of digging through the decompile:

| Skill | Use when |
|---|---|
| `schedule1-s1api` | Need a S1API namespace/class for a task (Saveables, PhoneApp, Quests, NPCs, Items, Money, GameTime, Lifecycle, cross-branch compatibility) |
| `schedule1-s1mapi` | Need procedural meshes, building construction, GLTF/GLB loading, terrain clearing, NPC navigation, or prefab placement |

Both ship 6-8 reference files for deep dives. They're the right starting point when the decompile is overwhelming.
