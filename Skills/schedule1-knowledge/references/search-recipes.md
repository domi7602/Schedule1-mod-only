# Search Recipes — Concrete Patterns

Concrete patterns to use the workspace's grep/glob/ilspycmd tools efficiently. Generic advice ("search the codebase") is not actionable; these are tested recipes.

---

## Recipe 1: Find a Vanilla Class

**Goal:** Locate `<Full.Type.Name>` (e.g. `Il2CppScheduleOne.Growing.GrowContainer`) and inspect members.

```pwsh
# Step 1: Search in-repo decompiles
Get-ChildItem 'GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne' -Recurse -Filter 'GrowContainer*.cs' | Select-Object FullName

# Step 2: Or verify live against runtime proxies:
& (Get-Command ilspycmd -ErrorAction Stop).Source -t 'Il2CppScheduleOne.Growing.GrowContainer' `
    "$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll"
```

---

## Recipe 2: Find a Specific Method

```pwsh
# Search within local decompile folder
Select-String -Pattern 'SetMoistureAmount' -Path 'GameReferences\decompiled\Assembly-CSharp\Il2CppScheduleOne\Growing\*'

# Cross-reference: which systems use this method?
Select-String -Pattern 'SetMoistureAmount' -Path 'Skills\schedule1-game-systems\references\*'
```

---

## Recipe 3: Explore a Namespace

```pwsh
# List all types under Il2CppScheduleOne.Growing
Get-ChildItem 'GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/Growing/' | Select-Object Name
```

---

## Recipe 4: Understand a System

```pwsh
# 1. Open the System index
# Skills/schedule1-game-systems/references/_index.md

# 2. Read the specific System doc
# Skills/schedule1-game-systems/references/08-Plant-Growing.md
```

---

## Recipe 5: Find a Workspace Mod That Already Solved It

```pwsh
# Search active mod implementations in the workspace
Select-String -Pattern 'CanItemFitInInventory' -Path 'Source\Mods\*\src\*.cs'
```

---

## Recipe 6: Verify S1API Source & Methods

```pwsh
# Step 1: Inspect S1API C# source directly in the repo
Get-ChildItem 'ThirdParty/S1API/S1API' -Recurse -Filter '*Money*.cs' | Select-Object FullName

# Step 2: Verify the deployed version's signature
& (Get-Command ilspycmd -ErrorAction Stop).Source -t 'S1API.Money.Money' `
    "$env:SCHEDULE1_PATH\Mods\S1API.Il2Cpp.MelonLoader.dll"
```

---

## Recipe 7: Check a Lifecycle Hook

```pwsh
# Find lifecycle event definitions in S1API
Select-String -Pattern 'OnSaveInfoLoaded|OnLoadComplete|OnGameplaySceneLoaded' `
    -Path 'ThirdParty\S1API\S1API\Lifecycle\*.cs'
```

---

## Recipe 8: Quick Log Diagnosis (PowerShell)

```pwsh
# Scan Latest.log for errors and exceptions (last 200 lines)
Get-Content "$env:SCHEDULE1_PATH\MelonLoader\Latest.log" -Tail 200 |
    Select-String -Pattern '\[ERROR\]|\[WARNING\]|Exception|WasCollected'
```

---

## Anti-Patterns (Don't Do This)

| Anti-pattern | Why | Do instead |
|---|---|---|
| Grepping 41 MB blindly | Slow, noisy | Check `Skills/schedule1-game-systems/references/_index.md` first |
| Missing `WasCollected` check | IL2CPP proxy stays non-null after C++ destruction | Check `obj != null && obj.Pointer != IntPtr.Zero && !obj.WasCollected` |
| Using `slot_-1.json` | Corrupts save data before load completes | Check `info.SaveSlotNumber >= 0` |
| Calling `Resources.FindObjectsOfTypeAll` in loops | Freezes game engine at 1 FPS | Scope to active scene root game objects |

---

## When to Read vs. Grep

| Situation | Use |
|---|---|
| I have a concrete class name | `GameReferences/decompiled/Assembly-CSharp/...` or `ilspycmd -t` |
| I need the canonical flow of a feature | Read the System doc in `Skills/schedule1-game-systems/references/` (01-64) |
| I want to see a working mod example | Inspect `Source/Mods/<ModName>/src/` |
| I want to verify an API signature against actual code | `ilspycmd -t` on live proxy DLL |

---

## Sample Session (End-to-End)

**Task:** Add a HUD that displays current cash balance + property count in real time.

1. Inspect S1API Money: `ThirdParty/S1API/S1API/Money/Money.cs`.
2. Inspect Property system: `Skills/schedule1-game-systems/references/54-Property.md`.
3. Check established mod pattern: `Skills/schedule1-modding/references/mod-patterns.md` (Pattern 2: HUD Overlay).
4. Scaffold: `pwsh Tools/new-mod.ps1 -Name "CashHUD" -Author "Dominik"`.
5. Implement following the HUD pattern; register `ModConfig<CashHudConfig>` for toggle.
6. `dotnet build Source\Mods\CashHUD\src\CashHUD.csproj -c Release` $\rightarrow$ auto-deploy.
7. Launch game, validate.
