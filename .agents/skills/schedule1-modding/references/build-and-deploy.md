# Build & Deploy — Detailed Runbook

This reference documents the **complete build/deploy pipeline** for the Schedule I Modding Workspace, including the conventions enforced by `Source/Mods/Directory.Build.props` + `Directory.Build.targets` and the special handling required for the ThirdParty repos (Sideload, hash, MoreDrugs).

---

## 1. Command Matrix

| Build | Command (PowerShell) | When |
|-------|---------------------|------|
| Single workspace mod | `dotnet build Source/Mods/<Name>/src/<Name>.csproj -c Release` | Fast iteration on one mod |
| All workspace mods (Solution) | `dotnet build Source/Mods/S1Mods.sln -c Release` or `pwsh Tools/build-all.ps1` | Releases, baseline checks |
| Regenerate solution after new mod | `pwsh Tools/gen-sln.ps1` | After `Tools/new-mod.ps1` |
| Sideload | `Set-Location ThirdParty\ScheduleOne-Sideload; dotnet build Sideload.csproj -c Release` | Phone-UI framework updates |
| hash | `Set-Location ThirdParty\ScheduleOne-Hash; dotnet build Hash.csproj -c Release` (needs Sideload.dll in `<Game>\Mods\`) | Dev-console replacement updates |
| MoreDrugs | `Set-Location Knowledge\external-repos\MoreDrugs; dotnet build src/DrugExpansion/DrugExpansion.csproj -c Il2cpp` | Drug catalog updates |
| S1Interop static analysis (pre-flight) | `s1interop analyze "<csproj>"` | Before every build |
| S1Interop doctor (env check) | `s1interop doctor --il2cpp-game-path $env:SCHEDULE1_PATH` | After game or Studio updates |

---

## 2. The Auto-Deploy Mechanism

Every workspace mod's `.csproj` is implicitly wrapped by `Source/Mods/Directory.Build.props` (precedence) and `Source/Mods/Directory.Build.targets` (action) — both live in `Source/Mods/`. The targets file auto-deploys:

1. `<ModName>.dll` → `<GameDir>\Mods\<ModName>.dll`
2. `*.png` (icons) → `<GameDir>\Mods\`
3. `docs/mod.json` → `<GameDir>\Mods\`
4. **Embedded PDBs**: `<DebugType>embedded</DebugType>` in props → no separate `.pdb` produced.

The deploy step uses `<CopyToOutputDirectory>...</CopyToOutputDirectory>` with `SkipUnchangedFiles="true"` (see §4).

---

## 3. GameDir Override

The workspace defaults `<GameDir>` to `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`. Override in two ways:

A. **PowerShell env var** (recommended; survives the whole session):
```pwsh
$env:SCHEDULE1_PATH = "D:\path\to\Schedule I"
dotnet build ...
```

B. **`local.build.props`** (persists on disk; should be gitignored):
```xml
<!-- In Source/Mods/local.build.props (gitignored) -->
<Project>
  <PropertyGroup>
    <GameDir>D:\path\to\Schedule I</GameDir>
    <S1ApiIl2CppPath>D:\path\to\Schedule I\MelonLoader\Il2CppAssemblies\S1API.Il2Cpp.MelonLoader.dll</S1ApiIl2CppPath>
  </PropertyGroup>
</Project>
```

> **IMPORTANT (verified gotcha):** MSBuild evaluates `<GameDir>` **once at process start**. Setting `$env:SCHEDULE1_PATH` AFTER `dotnet build` has begun will not change the target. Set it BEFORE the build.

---

## 4. ⚠ SkipUnchangedFiles-Falle (GEFIXT seit 2026-08-20)

**Historisch:** Der Deploy-Target nutzte `SkipUnchangedFiles="true"` — wenn die produzierte DLL byte-identisch zur deployed Version war, wurde nicht neu kopiert → man testete gegen stale Code.

**Aktueller Stand (2026-08-20):** `Source/Mods/Directory.Build.targets` nutzt jetzt **`SkipUnchangedFiles="false"`** für DLL + PNG + mod.json → jeder `dotnet build` force-deployt. Die Falle existiert nicht mehr.

**Wenn trotzdem stale DLL vermutet wird:**
```pwsh
dotnet clean Source\Mods\<Name>\src\<Name>.csproj
dotnet build  Source\Mods\<Name>\src\<Name>.csproj -c Release
# oder deployed DLL löschen und neu bauen
Remove-Item "$env:SCHEDULE1_PATH\Mods\<Name>.dll"
dotnet build  Source\Mods\<Name>\src\<Name>.csproj -c Release
```

**Version-Bump (seit 2026-08-20):** Nie manuell 4 Files ändern — `Tools/bump-version.ps1` synchronisiert `Mod.cs` (MelonInfo) + `docs/mod.json` + `docs/CHANGELOG.md` + `AGENTS.md` atomar:
```pwsh
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.1        # echte Änderung
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.1 -DryRun # Vorschau
```

---

## 5. ThirdParty Builds (Sideload, hash, MoreDrugs) — Special Handling

These are **NOT** part of `S1Mods.sln`. They live in `ThirdParty/` (and `Knowledge/external-repos/` for MoreDrugs), each with its own `*.csproj` that bypasses the workspace's `Directory.Build.props`.

### Sideload (DooDesch Phone-UI framework)
```pwsh
Set-Location "C:\Users\pc\Desktop\Schedule1-mod-only\ThirdParty\ScheduleOne-Sideload"
dotnet build Sideload.csproj -c Release       # auto-deploys to <Game>\Mods\
```
Dep: needs AngleSharp + Jint + Esprima → `<Game>\UserLibs\`.

### hash (DooDesch Dev-Console)
```pwsh
Set-Location "C:\Users\pc\Desktop\Schedule1-mod-only\ThirdParty\ScheduleOne-Hash"
dotnet build Hash.csproj -c Release            # auto-deploys
```
Dep: requires `Sideload.dll` already in `<Game>\Mods\` (build includes `Sideload.cs` via `<Compile Include="..\..\Sideload\Sideload.cs"/>`).

### MoreDrugs (ifBars-Fork)
```pwsh
Set-Location "C:\Users\pc\Desktop\Schedule1-mod-only\Knowledge\external-repos\MoreDrugs"
dotnet build src/DrugExpansion/DrugExpansion.csproj -c Il2cpp
```
Dep: `local.build.props` with `S1ApiIl2CppPath` + `MApiIl2CppPath`. DLL name: `DrugExpansion_Il2cpp.dll`.

---

## 6. Build Errors & Their First-Try Fixes

| Error | Likely cause | First try |
|---|---|---|
| `CS0246: type 'S1API' not found` | S1API DLL is `.bak` (broken HintPath) | Restore S1API DLLs (deployed) or set `<S1ApiIl2CppPath>` in `local.build.props` |
| `CS0118: 'Harmony' is both namespace and type` | `using HarmonyLib;` + `private static Harmony? _harmony;` | Qualify: `private static HarmonyLib.Harmony? _harmony;` |
| `MSB3026: Could not copy ... Access denied` | Schedule I still running, file locked | Close game; build auto-retries |
| `MSB3027: Could not copy ... already exists` | Deployed DLL locked by game | Move game binary out of the way or use `--no-deploy` flag |
| `IL2CPP0019: ...method body invalid` | Mod generates IL that IL2CPP can't process | Reduce complexity or use `[HideFromIl2Cpp]` on the offending type |
| `LogLevel Warning: Class::Init signatures exhausted, using a substitute!` | Mod class volume too high (>5–10 MB) | Split or roll back to a smaller version; reduce `[RegisterTypeInIl2Cpp]` count |
| `Implicit conversion from float to int` warnings everywhere | Money fields are float now, not int | Use `float cashBalance` everywhere; see PotScanner v0.2.0 spec |

---

## 7. Deploy Verification Checklist

After every successful build, verify before launching the game:

1. **DLL exists** in `<GameDir>\Mods\<Name>.dll`
2. **Timestamp** of DLL ≥ timestamp of `bin/Release/net6.0/<Name>.dll` (deploy happened)
3. **`mod.json` exists** in `<GameDir>\Mods\` if the mod ships one
4. **Icon PNG exists** if the mod has an icon (e.g. NotesApp)
5. **`MelonPreferences` cfg** updates correctly (check UserData)

---

## 8. Distributable Outputs (for Nexus/Thunderstore)

When shipping a mod externally:

```pwsh
# Inside each mod's bin/Release/net6.0/ folder
- <Name>.dll                         (embedded PDB)
- icon.png                           (if applicable)
- docs/mod.json
- README.md + CHANGELOG.md
```

Solution-wide packaging: use `pwsh Tools/package-release.ps1 -Mod All` (creates `Release/<Mod>-v<Version>.zip` per mod). CI: `.github/workflows/ci.yml` (format + build + tests + SLN-determinism + Knowledge XRef) and `release.yml` (tag-triggered zips).

---

## 9. Common Pipelines (End-to-End)

**Iterate quickly on one mod:**
```pwsh
$env:SCHEDULE1_PATH = "C:\Program Files (x86)\Steam\steamapps\common\Schedule I"
cd "C:\Users\pc\Desktop\Schedule1-mod-only"
dotnet build "Source\Mods\NotesApp\src\NotesApp.csproj" -c Release
# Game already running? Just alt-tab and reload the phone — most mods pick up changes on scene change
```

**Clean release for all mods:**
```pwsh
$env:SCHEDULE1_PATH = "C:\Program Files (x86)\Steam\steamapps\common\Schedule I"
Set-Location "C:\Users\pc\Desktop\Schedule1-mod-only"
pwsh Tools/build-all.ps1
s1interop doctor --il2cpp-game-path $env:SCHEDULE1_PATH
```

**Onboard a new mod:**
```pwsh
pwsh Tools/new-mod.ps1 -Name "MyMod" -Author "Dominik" -Version "0.1.0"
pwsh Tools/gen-sln.ps1
dotnet build Source\Mods\MyMod\src\MyMod.csproj -c Release
```
