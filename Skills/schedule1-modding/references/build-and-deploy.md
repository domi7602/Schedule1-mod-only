# Build & Deploy — Detailed Runbook

This reference documents the **complete build/deploy pipeline** for the Schedule I Modding Workspace, including the conventions enforced by `Source/Mods/Directory.Build.props` + `Directory.Build.targets` and the handling of ThirdParty frameworks.

---

## 1. Command Matrix

| Build | Command (PowerShell) | When |
|---|---|---|
| Single workspace mod | `dotnet build Source/Mods/<Name>/src/<Name>.csproj -c Release` | Fast iteration on one mod |
| All workspace mods (Solution) | `dotnet build Source/Mods/S1Mods.sln -c Release` or `pwsh Tools/build-all.ps1` | Full solution builds, baseline checks |
| Regenerate solution after new mod | `pwsh Tools/gen-sln.ps1` | After `Tools/new-mod.ps1` |
| ThirdParty deploy | Auto-executed via MSBuild `Directory.Build.targets` or `pwsh Tools/deploy-thirdparty.ps1` | Syncs whitelisted 3rd-party DLLs to `<GameDir>\Mods\` |
| S1API rebuild (if modified) | `dotnet build ThirdParty/S1API/S1API.sln -c Release` | Framework upgrades or internal API fixes |
| Version bump across 4 sources | `pwsh Tools/bump-version.ps1 -Mod <Name> -Version <X.Y.Z>` | Release preparation |

---

## 2. The Auto-Deploy Mechanism (2026-09 Standard)

Every workspace mod's `.csproj` is implicitly wrapped by `Source/Mods/Directory.Build.props` (precedence) and `Source/Mods/Directory.Build.targets` (action) — both live in `Source/Mods/`. The targets file auto-deploys:

1. **Loadables for MelonLoader** $\rightarrow$ `<GameDir>\Mods\`:
   - `<ModName>.dll`
   - `*.png` (app/HUD icons)
   - `*.bundle` (embedded AssetBundles)
2. **Metadata & Debug Symbols** $\rightarrow$ `<GameDir>\UserData\<ModName>\`:
   - `Source/Mods/<ModName>/docs/mod.json` $\rightarrow$ `<GameDir>\UserData\<ModName>\mod.json`
   - `<ModName>.pdb` $\rightarrow$ `<GameDir>\UserData\<ModName>\<ModName>.pdb`
3. **ThirdParty DLLs**:
   - `Directory.Build.targets` invokes `Tools/deploy-thirdparty.ps1`.
   - Copies whitelisted DLLs under `ThirdParty/` to `<GameDir>\Mods\` based on `ThirdParty/.deployignore`.

The deploy step uses `<Copy ... SkipUnchangedFiles="false" />` to force deploy on every build.

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

**Version-Bump (seit 2026-08-20):** Nie manuell 4 Files ändern — `Tools/bump-version.ps1` synchronisiert `Mod.cs` (MelonInfo) + `Source/Mods/<Mod>/docs/mod.json` + `Source/Mods/<Mod>/docs/CHANGELOG.md` + `AGENTS.md` atomar:
```pwsh
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.1        # echte Änderung
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.1 -DryRun # Vorschau
```

---

## 5. ThirdParty Libraries & Frameworks

| Framework | Status | Origin / Deploy | Notes |
|---|---|---|---|
| **S1API 3.2.0** | ✅ active | Fork-build in `ThirdParty/S1API/` $\rightarrow$ `Mods\S1API.Il2Cpp.MelonLoader.dll` + `Plugins\S1APILoader.dll` | Core modding API |
| **PhoneScroll 1.4** | ❌ retired 2026-09-16 | `ThirdParty/Archive/PhoneScroll/` (nur README, DLL zurückgezogen) $\rightarrow$ `Mods\PhoneScroll.dll` | Closed-source vanilla phone scroll hook by V4LEXL |
| **S1MAPI 2.0.0** | ✅ active | `ThirdParty/S1MAPI/` | Procedural meshes, GLTF loader, interior tools |
| **S1MCPServer** | ✅ active | `ThirdParty/S1MCPServer-master/` $\rightarrow$ `Mods\S1MCPServer-IL2CPP.dll` | Live TCP introspection (:8765) |
| **Sideload** | ❌ removed | Removed on 2026-09-10 (commit `37e7a5d`) | Deprecated, unused |
| **hash 1.0.5** | 📦 deprecated | `ThirdParty/Archive/ScheduleOne-Hash/` (undeployed) | Kept for reference only |

---

## 6. Build Errors & Their First-Try Fixes

| Error | Likely cause | First try |
|---|---|---|
| `CS0246: type 'S1API' not found` | S1API DLL missing or broken HintPath | Rebuild S1API (`ThirdParty/S1API/`) or check `Directory.Build.props` |
| `CS0118: 'Harmony' is both namespace and type` | `using HarmonyLib;` + `private static Harmony? _harmony;` | Qualify: `private static HarmonyLib.Harmony? _harmony;` |
| `MSB3026 / MSB3027: Access denied / already exists` | Schedule I still running, file locked | Close game; build auto-retries |
| `IL2CPP0019: method body invalid` | Mod generates IL that IL2CPP can't process | Reduce complexity or use `[HideFromIl2Cpp]` on the offending type |
| `WasCollected / NullReferenceException in IL2CPP` | Native object was destroyed | Guard with `obj != null && obj.Pointer != IntPtr.Zero && !obj.WasCollected` |
| `Implicit conversion from float to int` warnings | Money fields are float now, not int | Use `float cashBalance` and `onlineBalance` everywhere |

---

## 7. Deploy Verification Checklist

After every successful build, verify before launching the game:

1. **DLL exists** in `<GameDir>\Mods\<Name>.dll`
2. **Timestamp** of DLL ≥ timestamp of `bin/Release/net6.0/<Name>.dll` (deploy happened)
3. **`mod.json` exists** in `<GameDir>\UserData\<Name>\mod.json`
4. **`<Name>.pdb` exists** in `<GameDir>\UserData\<Name>\<Name>.pdb`
5. **Icon PNG exists** in `<GameDir>\Mods\` if the mod ships one
6. **`MelonPreferences` cfg** updates correctly (check UserData)

---

## 8. Distributable Outputs (for Nexus/Thunderstore)

When shipping a mod externally:

```pwsh
# Output files:
- <GameDir>\Mods\<Name>.dll
- <GameDir>\Mods\<icon>.png                           (if applicable)
- <GameDir>\UserData\<Name>\mod.json
- README.md + CHANGELOG.md
```

Solution-wide packaging: use `pwsh Tools/package-release.ps1 -Mod All` (creates `Release/<Mod>-v<Version>.zip` per mod).

---

## 9. Common Pipelines (End-to-End)

**Iterate quickly on one mod:**
```pwsh
$env:WORKSPACE_ROOT = "C:\Users\pc\Schedule1-mod-only"   # set once per session
dotnet build "$env:WORKSPACE_ROOT\Source\Mods\NotesApp\src\NotesApp.csproj" -c Release
```

**Clean release for all mods:**
```pwsh
$env:WORKSPACE_ROOT = "C:\Users\pc\Schedule1-mod-only"
pwsh "$env:WORKSPACE_ROOT\Tools\build-all.ps1"
```

> The workspace lives outside the game directory (`C:\Users\pc\Schedule1-mod-only`); only the game path needs `$env:SCHEDULE1_PATH`. Use `$env:WORKSPACE_ROOT` for cross-machine portability.

**Onboard a new mod:**
```pwsh
pwsh Tools/new-mod.ps1 -Name "MyMod" -Author "Dominik" -Version "0.1.0"
pwsh Tools/gen-sln.ps1
dotnet build Source\Mods\MyMod\src\MyMod.csproj -c Release
```

---

## 10. Multi-PC-Workflow (Laptop ↔ Desktop)

Linearer Workflow auf `main`, keine Feature-Branches. Spielstände und Save-Daten (`<GameDir>\UserData\<Mod>\*.json`) bleiben PC-lokal — nur Source-Code und Docs werden synchronisiert.

**Erstmaliges Setup auf einem neuen PC** (Repo-Klon, dann):
```pwsh
git clone https://github.com/domi7602/Schedule1-mod-only.git
cd Schedule1-mod-only
pwsh Tools/setup-workspace.ps1        # generiert local.build.props, fragt ggf. nach Game-Pfad
dotnet build Source/Mods/S1Mods.sln -c Release
```

**Täglicher Wechsel:**
- Session-Start: `git pull`
- Session-Ende: `git add -A && git commit && git push`

**`local.build.props`** sind gitignored (`ThirdParty/S1API/local.build.props`, `ThirdParty/S1MAPI/local.build.props`) und werden vom `setup-workspace.ps1` aus den `*.example`-Vorlagen generiert. Bei Steam-Standardpfad `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` reicht der Default; bei anderer Library-Lokation interaktiver Prompt oder `-GameDir "<Pfad>"`.

**Was NICHT synchronisiert wird:** `<GameDir>\Mods\*.dll` (Build-Artefakte, nach `dotnet build` lokal erzeugt), `<GameDir>\UserData\*` (Saves), `bin/`/`obj/`.
