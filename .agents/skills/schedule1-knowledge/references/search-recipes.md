# Search Recipes — Concrete Patterns

Concrete patterns to use the workspace's grep/glob/ilspycmd tools efficiently. Generic advice ("search the codebase") is not actionable; these are tested recipes.

---

## Recipe 1: Find a Vanilla Class

**Goal:** Locate `<Full.Type.Name>` (e.g. `Il2CppScheduleOne.Growing.GrowContainer`) and inspect members.

```pwsh
# Step 1: Open the 1st-party decompiled file
Get-ChildItem 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Decompiles\Assembly-CSharp\' -Recurse -Filter 'GrowContainer*.cs' | Select-Object FullName

# Step 2: Read the decompile
Read-File '<chosen path>'
```

Or use the pre-dumped verify-snippet:
```pwsh
Read-File 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Decompiles\verify-snippets\GrowContainer.txt'
```

Or the live-game DLL via `ilspycmd`:
```pwsh
& 'C:\Users\pc\.dotnet\tools\ilspycmd.exe' -t 'Il2CppScheduleOne.Growing.GrowContainer' `
    "$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\Il2CppScheduleOne.dll"
```

---

## Recipe 2: Find a Specific Method

```pwsh
# Search within decompile folder
grep -rn 'SetMoistureAmount' 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Decompiles\Assembly-CSharp\Il2CppScheduleOne\Growing\'

# Cross-reference: which systems use this method?
grep -rn 'SetMoistureAmount' 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Analysis\'
```

---

## Recipe 3: Explore a Namespace

```pwsh
# List all types under Il2CppScheduleOne.Growing
Get-ChildItem 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Decompiles\Assembly-CSharp\Il2CppScheduleOne\Growing\' | Select-Object Name

# Or read the CodeTree index
Read-File 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Analysis\Reference\ScheduleOne_CodeTree.md' -Limit 200
```

---

## Recipe 4: Understand a System

```pwsh
# 1. Open the System doc
Read-File 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Analysis\Systems\08-Plant-Growing.md'

# 2. Drill into the Source-Export detail
Read-File 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Analysis\Source-Export\34-Growing-Detail.md'

# 3. Look at the canonical concept
Read-File 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Analysis\Learnings\Concepts\12-PhoneApp-Mod-Patterns.md'  # or similar concept
```

---

## Recipe 5: Find a 3rd-Party Mod That Already Did It

```pwsh
# Search the catalog
grep -rn 'GrowContainer' 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Analysis\Learnings\ThirdParty\'

# Or browse the README
Read-File 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Analysis\Learnings\README.md'
```

---

## Recipe 6: Verify an S1API API

```pwsh
# Step 1: Check the version guide
Read-File 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Frameworks\S1API\VERSIONS.md'

# Step 2: Check the S1API analysis doc
Read-File 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Analysis\APIs\S1API.md'

# Step 3: Verify the deployed version's signature
& 'C:\Users\pc\.dotnet\tools\ilspycmd.exe' -t 'S1API.Money.Money' `
    "$env:SCHEDULE1_PATH\Mods\S1API.Il2Cpp.MelonLoader.dll"
```

---

## Recipe 7: Check a Lifecycle Hook

```pwsh
# Find which event exists for "after save loaded"
grep -rn 'OnSaveInfoLoaded\|OnLoadComplete\|OnGameplaySceneLoaded' `
    'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Decompiles\verify-snippets\s1api-full.txt'

# Or read S1API's analysis doc
Read-File 'C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\Game-Reference\Analysis\APIs\S1API.md' -Offset 50 -Limit 80
```

---

## Recipe 8: Pre-Flight Before Writing a Mod

```pwsh
# Check the mod does not violate IL2CPP rules
& 'C:\Users\pc\Desktop\Schedule1-mod-only\Tools\s1-error-analyzer' doctor `
    --il2cpp-game-path "$env:SCHEDULE1_PATH"

# Or s1interop analyze
s1interop analyze "$workspace_root\Source\Mods\MyMod\src\MyMod.csproj"
```

---

## Recipe 9: Quick Log Diagnosis

```pwsh
# Already covered in schedule1-troubleshooting; see references/logscan-and-logs.md
& "C:\Users\pc\Desktop\Schedule1\External\S1MCPServer\logscan.py" --error-summary --tail 200
```

---

## Anti-Patterns (Don't Do This)

| Anti-pattern | Why | Do instead |
|--------------|-----|-----------|
| `grep -r "GrowContainer"` over all of `Decompiles/` | Grepping 41 MB, slow, noisy | Use `grep -rl` first to find the namespace folder, then targeted search |
| `Recurse-Get-ChildItem` on `Decompiles/Assembly-CSharp` and read the first 100 | Massive unnecessary context | Step 1: get the namespace → Step 2: get the file |
| Reading the entire `Systems/01-FishNet-Networking.md` when you need 1 method | Wasteful | Open it, Cmd-F the method name |
| Skipping `_index.md` of an Analysis folder | Misses the navigation table | Always open `_index.md` first |
| Reading S1API from raw decompile every time | Slow + 962 files | Use `Frameworks/S1API/Decompiles/3.2.0/` + `S1API.md` overview first |
| Reading a full 4 MB third-party mod source via grep | Slow | Use `Analysis/Learnings/ThirdParty/<ModName>.md` summary first |

---

## When to Read vs. Grep

| Situation | Use |
|-----------|-----|
| I have a concrete class name | `ilspycmd -t` or grep-then-read |
| I need the canonical flow of a feature | Read the System doc (01-64) |
| I need to compare two implementations | Read the ThirdParty analysis files |
| I want a one-screen cheatsheet | Read the index `_index.md` files |
| I want to verify an API signature against actual code | `ilspycmd -t` |

---

## Tools Reference (Paths)

| Tool | Path | Used For |
|------|------|----------|
| `ilspycmd` | `C:\Users\pc\.dotnet\tools\ilspycmd.exe` | C# decompile |
| `logscan.py` | `C:\Users\pc\Desktop\Schedule1\External\S1MCPServer\logscan.py` | Latest.log triage |
| `s1interop` | `s1interop` (CLI global tool) | Mod static analysis |
| `rg` | `Get-Command rg` (ripgrep) | Fast text search |

---

## Sample Session (End-to-End)

**Task:** Add a HUD that displays current cash balance + property count in real time.

1. `Read-File 'Analysis/APIs/S1API.md' -Offset 100 -Limit 50` — confirm `S1API.Money.Money` API.
2. `ilspycmd -t 'S1API.Money.Money' "<GameDir>\Mods\S1API.Il2Cpp.MelonLoader.dll"` — get exact members.
3. `Read-File 'Analysis/Systems/54-Property.md'` — confirm `PropertyManager.GetOwnedProperties()` shape.
4. `Read-File 'Analysis/Learnings/Concepts/12-PhoneApp-Mod-Patterns.md'` — see established patterns (or directly `references/mod-patterns.md` Pattern 2).
5. `Read-File '.agents/skills/schedule1-modding/references/mod-patterns.md'` — Pattern 2 + 6 combo.
6. Scaffold: `pwsh Tools/new-mod.ps1 -Name "CashHUD" -Author "Dominik"`.
7. Implement following the HUD pattern; register `ModConfig<CashHudConfig>` for toggle.
8. `dotnet build Source\Mods\CashHUD\src\CashHUD.csproj -c Release` → auto-deploy.
9. `s1interop analyze` → 0 errors.
10. Launch game, validate.
