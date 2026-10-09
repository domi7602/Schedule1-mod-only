# Release Process

Releases are produced **per mod** and **manually**. GitHub-hosted runners do not have Schedule I installed, and every mod compiles against the game's IL2CPP assemblies, so packaging only works on a Windows machine (or self-hosted runner) with the game present.

## Prerequisites

- Schedule I installed; `$env:SCHEDULE1_PATH` set if it is not in the default Steam location.
- MelonLoader and S1API deployed into the game directory (the build references `Mods\S1API.Il2Cpp.MelonLoader.dll`); required versions: [`docs/compatibility.md`](compatibility.md).
- An SDK that can build the mod target framework (see `Source/Mods/Directory.Build.props`) and PowerShell 7.
- Working tree clean, `main` up to date.

## 1. Bump the version

The code is the single source of truth (`[assembly: MelonInfo(...)]`, or `Constants.ModVersion` for PotScanner). Never edit version numbers by hand:

```pwsh
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.4
```

This updates five places atomically: the `MelonInfo` attribute, the mod's `mod.json`, a new `## 1.0.4 (YYYY-MM-DD)` header in the mod's `CHANGELOG.md` (both under `Source/Mods/<Mod>/docs/`), the inventory row (and detail header) in `AGENTS.md`, and the mod table in `README.md`. Fill in the changelog entry afterwards.

## 2. Run the quality gates

```pwsh
dotnet format Source/Mods/S1Mods.sln --verify-no-changes
pwsh Tools/check-version-sync.ps1
pwsh Tools/check-doc-paths.ps1
dotnet build Source/Mods/S1Mods.sln -c Release -p:S1NoDeploy=true
dotnet test  Source/Mods/S1Mods.sln -c Release
```

Then verify the mod in-game (Main Menu → load save → use the mod → Main Menu) and record the result in the changelog and in `docs/compatibility.md`.

## 3. Package

```pwsh
pwsh Tools/package-release.ps1 -Mod NotesApp      # one mod
pwsh Tools/package-release.ps1                    # every active mod
```

The script builds the mod in Release, fails fast if the game assemblies are missing, and writes `Release/<ModName>-v<version>.zip` (the `Release/` folder is git-ignored). The ZIP mirrors the game directory:

```text
NotesApp-v1.0.4.zip
├── Mods/
│   ├── NotesApp.dll
│   ├── Shared.dll                 S1Mods.Shared — required by every mod
│   └── notiz_app_lowpoly_fancy.png
├── UserData/NotesApp/
│   ├── mod.json
│   └── NotesApp.pdb
├── README.md                      copied from docs/README.md
└── CHANGELOG.md                   copied from docs/CHANGELOG.md
```

`Shared` and the archived `_DiagPerfCounter` tool are never packaged on their own. `Shared.dll` is not versioned separately — when releasing several mods, package them from the same commit so they ship an identical `Shared.dll`.

Verify the archive contains `Mods/<Mod>.dll`, `Mods/Shared.dll` and `UserData/<Mod>/mod.json` before publishing.

## 4. Publish on GitHub

1. Commit the version bump and push.
2. Create a tag `<ModName>-v<version>` (one mod per tag keeps release notes focused) and push it.
3. Create a GitHub release from the tag, attach the ZIP from `Release/`, and state in the notes:
   - the game version the package was verified on,
   - the required frameworks, quoted from [`docs/compatibility.md`](compatibility.md) (MelonLoader, S1API, S1MAPI if applicable),
   - the changelog excerpt for this version.
4. Update `docs/compatibility.md` if the verified game version changed.

> **Automated:** steps 2–3 of this section are implemented by `Tools/release-mod.ps1` — it runs the version-sync gate, verifies a clean, pushed branch, packages the ZIP if missing, pushes the tag and creates the GitHub release with the matching CHANGELOG section as notes. Preview with `-DryRun`, review the notes with `-Draft`.

Tags do **not** trigger packaging automatically (an earlier tag-push trigger failed on every run because the runner has no game). The `Release` workflow (`.github/workflows/release.yml`) can be started manually via *Run workflow* on a self-hosted Windows runner with the game installed; it runs `package-release.ps1` and uploads the ZIPs as workflow artifacts.

## Local-only distribution

Packaging can also be used purely locally (for example to copy a mod to a second PC): run `package-release.ps1` and extract the ZIP into the game directory.

Release ZIPs are for **local distribution only** — no upload to NexusMods or other public hosts (Dominik's decision, 2026-09-19).
