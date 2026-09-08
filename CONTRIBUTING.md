# Contributing Guide

Danke für dein Interesse am `Schedule I Modding Workspace`! Diese Anleitung fasst den effizienten Workflow für Beiträge zusammen.

## Anforderungen

- **Game:** Schedule I v0.4.6f13 (IL2CPP, Unity 2022.3) unter `C:\Program Files (x86)\Steam\steamapps\common\Schedule I` (override via `$env:SCHEDULE1_PATH`)
- **SDK:** .NET 6 SDK, PowerShell 7+
- **Loader:** MelonLoader 0.7.3
- **S1API:** 3.2.0 (deployed als `Mods/S1API.Il2Cpp.MelonLoader.dll` + `Plugins/S1APILoader.dll`, Fork-Build aus `ThirdParty/S1API/`)

## Workspace Layout

```
Source/Mods/          Mods + Shared lib + S1Mods.sln
Source/Archive/       Archivierte Mods (nicht in SLN)
Tools/                build-all.ps1, gen-sln.ps1, bump-version.ps1, package-release.ps1
Knowledge/            Decompiles, Analysen, Framework-Referenzen
.agents/skills/       AI-Skills (schedule1-modding, -phoneapp, -s1api, ...)
AGENTS.md             Inventar & Konventionen (Single Source of Truth)
```

## Skills laden

Lade **immer** zuerst den relevanten Skill via `skill`-Tool:

- `schedule1-modding` — für jeden Mod-Task (Pflicht)
- `schedule1-phoneapp` — für PhoneApps
- `schedule1-s1api` / `schedule1-s1mapi` — für Framework-APIs
- `schedule1-knowledge` — für Recherche in `Knowledge/`
- `schedule1-troubleshooting` — für Crashes/Logs

## Neuen Mod erstellen

```pwsh
pwsh Tools/new-mod.ps1 -Name "MyNewMod" -Author "Dominik" -Version "0.1.0"
pwsh Tools/gen-sln.ps1
dotnet build Source/Mods/MyNewMod/src/MyNewMod.csproj -c Release
```

`Directory.Build.props/targets` deployt DLLs/PNGs automatisch nach `<GameDir>\Mods\`; `mod.json` + `.pdb` nach `<GameDir>\UserData\<ModName>\` (seit 2026-09 — json/pdb gehören nie nach `Mods\`).

Siehe `.agents/skills/schedule1-modding/references/architecture-and-shared.md` für Pflicht-Patterns (SafeStorage, UITheme, PatchGuard, InputFocus).

## Build & Test

```pwsh
# Alle Mods
pwsh Tools/build-all.ps1
dotnet build Source/Mods/S1Mods.sln -c Release

# Einzelner Mod
dotnet build Source/Mods/NotesApp/src/NotesApp.csproj -c Release

# Tests (Shared)
dotnet test Source/Tests/Shared.Tests/Shared.Tests.csproj -c Release

# Format prüfen (CI)
dotnet format Source/Mods/S1Mods.sln --verify-no-changes

# Knowledge-Links prüfen
pwsh Tools/fix-knowledge-paths.ps1 -WhatIf -Scope XRef
```

## Version Bump (4-File Sync)

Niemals manuell — nutze den Bump-Helper:

```pwsh
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.1
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.1 -DryRun
```

Aktualisiert: `Mod.cs` MelonInfo + `docs/mod.json` + `docs/CHANGELOG.md` + `AGENTS.md`.

## IL2CPP Pflichten

- Jeder `[RegisterTypeInIl2Cpp]` MonoBehaviour braucht `public Foo(IntPtr ptr) : base(ptr) { }`
- Keine `foreach`/LINQ auf `Il2CppSystem.Collections.Generic.List<T>` — nur `for`
- Keine `button.onClick.AddListener(new UnityAction(...))` — nutze `S1API.Utils.EventHelper.AddListener` / `ButtonUtils.AddListener`
- Vor jedem Build: `s1interop analyze <csproj>` (fängt `missing_intptr_constructor`)

## Definition of Done

- [ ] Build 0 Errors / 0 Warnings (`-c Release`)
- [ ] DLL deployed und im `MelonLoader/Latest.log` ohne Exception
- [ ] In-Game verifiziert (inkl. Scene-Wechsel Main Menu → Game → Main Menu)
- [ ] Persistenz Round-Trip OK (Save → Restart → Load)
- [ ] `s1interop analyze` clean
- [ ] `AGENTS.md`, `CHANGELOG.md`, `mod.json`, `Mod.cs` synchron (via `bump-version.ps1`)

## Commit & PR

- Commits: `feat(mod): ...`, `fix(mod): ...`, `chore(knowledge): ...` (siehe `git log`)
- Solution ist deterministisch (`gen-sln.ps1` nutzt MD5-GUIDs) — keine unnötigen GUID-Diffs
- `dotnet format` vor jedem Push
- PR-Template ausfüllen (siehe `.github/pull_request_template.md`)

## Fragen?

Siehe `AGENTS.md §6` (Workflows), `MEMORY.md` (Gotchas), `Knowledge/README.md` (Index) oder öffne ein Issue.
