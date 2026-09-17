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
Source/Tests/         xUnit-Tests (Shared.Tests, AutoPackagingStation.Tests, CalculatorApp.Tests)
GameReferences/       Lokal generierte Decompiles; siehe GameReferences/README.md
Skills/               AI-Skills (schedule1-modding, -phoneapp, -s1api, ...; Index: Skills/README.md)
ThirdParty/           Gepinnte externe Abhaengigkeiten & Archive; siehe ThirdParty/README.md
Tools/                build-all.ps1, gen-sln.ps1, new-mod.ps1, bump-version.ps1, package-release.ps1, check-version-sync.ps1, check-doc-paths.ps1, deploy-thirdparty.ps1, backup-to-d.ps1
Release/              Release-Pakete (.gitkeep)
AGENTS.md             Inventar & Konventionen (Single Source of Truth)
docs/                 Architektur- und Release-Dokumentation
```

## Skills laden

Lade **immer** zuerst den relevanten Skill via `skill`-Tool:

- `schedule1-modding` — für jeden Mod-Task (Pflicht)
- `schedule1-phoneapp` — für PhoneApps
- `schedule1-s1api` / `schedule1-s1mapi` — für Framework-APIs
- `schedule1-knowledge` — für Recherche (`GameReferences/`, `ThirdParty/S1API/`, `Skills/schedule1-game-systems/references/`)
- `schedule1-troubleshooting` — für Crashes/Logs

Skill-Pfade: `Skills/<skill-name>/SKILL.md` (+ `references/`-Unterdateien). Vollständiger Index: `Skills/README.md`.

## Abhaengigkeiten und Referenzen

```pwsh
git submodule update --init --recursive
pwsh Tools/bootstrap-game-references.ps1
```

Die Decompiles sind lokaler Recherche-Output und duerfen nicht committed werden. Die Regeln fuer Abhaengigkeitsrichtung und Mod-Struktur stehen in [`docs/architecture.md`](docs/architecture.md).

## Neuen Mod erstellen

```pwsh
pwsh Tools/new-mod.ps1 -Name "MyNewMod" -Author "Dominik" -Version "0.1.0"
pwsh Tools/gen-sln.ps1
dotnet build Source/Mods/MyNewMod/src/MyNewMod.csproj -c Release
```

`Directory.Build.props/targets` deployt DLLs/PNGs automatisch nach `<GameDir>\Mods\`; `mod.json` + `.pdb` nach `<GameDir>\UserData\<ModName>\` (seit 2026-09 — json/pdb gehören nie nach `Mods\`).

Siehe `Skills/schedule1-modding/references/architecture-and-shared.md` für Pflicht-Patterns (SafeStorage, UITheme, PatchGuard, InputFocus).

## Build & Test

```pwsh
# Alle Mods
pwsh Tools/build-all.ps1
dotnet build Source/Mods/S1Mods.sln -c Release

# Einzelner Mod
dotnet build Source/Mods/NotesApp/src/NotesApp.csproj -c Release

# Alle Tests (110 Tests ueber Solution)
dotnet test Source/Mods/S1Mods.sln -c Release

# Einzelne Test-Suiten
dotnet test Source/Tests/Shared.Tests/Shared.Tests.csproj -c Release               # braucht Spiel-Assemblies
dotnet test Source/Tests/AutoPackagingStation.Tests/AutoPackagingStation.Tests.csproj -c Release # reine Math-Logik
dotnet test Source/Tests/CalculatorApp.Tests/CalculatorApp.Tests.csproj -c Release # reine Decimal-Logik

# Format prüfen (CI)
dotnet format Source/Mods/S1Mods.sln --verify-no-changes

# Versions-Drift prüfen (CI + Pre-Commit): Code <-> mod.json <-> README/AGENTS (Exit 1 bei Drift)
pwsh Tools/check-version-sync.ps1

# Doku-Pfade prüfen (CI + Pre-Commit): referenzierte Repo-Pfade müssen existieren
pwsh Tools/check-doc-paths.ps1
```

## Version Bump (Single Source of Truth = Code)

Niemals manuell — nutze den Bump-Helper:

```pwsh
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.3
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.3 -DryRun

# Verifizieren (Exit 1 bei Drift):
pwsh Tools/check-version-sync.ps1
```

Aktualisiert 5 Stellen: MelonInfo im Code (Datei mit `[assembly: MelonInfo(...)]`, sonst `Constants.ModVersion`) + `Source/Mods/<Mod>/docs/mod.json` + `Source/Mods/<Mod>/docs/CHANGELOG.md` (`## x.y.z`-Header) + `AGENTS.md` (Matrix-Zeile **und** Detail-Header in §2) + `README.md` (Featured-Zeile).

`Tools/check-version-sync.ps1` prüft genau diese Orte gegen den Code; es läuft in CI und im Pre-Commit-Hook.

## IL2CPP Pflichten

- Jeder `[RegisterTypeInIl2Cpp]` MonoBehaviour braucht `public Foo(IntPtr ptr) : base(ptr) { }`
- Keine `foreach`/LINQ auf `Il2CppSystem.Collections.Generic.List<T>` — nur `for`
- Keine `button.onClick.AddListener(new UnityAction(...))` — nutze `S1API.Utils.EventHelper.AddListener` / `ButtonUtils.AddListener`
- Optional, falls installiert: `s1interop analyze <csproj>` (fängt `missing_intptr_constructor`) — `s1interop` ist ein externes Tool ohne Repo-Bezug; ohne Installation sind Build + Tests der verpflichtende Ersatz

## Definition of Done

- [ ] Build 0 Errors / 0 Warnings (`-c Release`)
- [ ] DLL deployed und im `MelonLoader/Latest.log` ohne Exception
- [ ] In-Game verifiziert (inkl. Scene-Wechsel Main Menu → Game → Main Menu)
- [ ] Persistenz Round-Trip OK (Save → Restart → Load)
- [ ] `s1interop analyze` clean (falls installiert)
- [ ] `AGENTS.md`, `CHANGELOG.md`, `mod.json`, `Mod.cs`/`Constants.ModVersion`, `README.md` synchron (via `bump-version.ps1`) — `pwsh Tools/check-version-sync.ps1` muss grün sein
- [ ] `pwsh Tools/check-doc-paths.ps1` grün (keine toten Pfad-Referenzen in der Doku)

## Commit & PR

- Commits: `feat(mod): ...`, `fix(mod): ...`, `chore(tools): ...` (siehe `git log`)
- Solution ist deterministisch (`gen-sln.ps1` nutzt MD5-GUIDs) — keine unnötigen GUID-Diffs
- `dotnet format` vor jedem Push
- PR-Template ausfüllen (siehe `.github/pull_request_template.md`)

## Fragen?

Siehe `AGENTS.md §6` (Workflows), `docs/pitfalls.md` (Gotchas & IL2CPP-Fallen), `Skills/README.md` (Skill-Index) oder öffne ein Issue.
