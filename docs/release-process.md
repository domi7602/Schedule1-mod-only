# Release Process

Releases are produced **per mod** and **manually**. GitHub-hosted runners do not have Schedule I installed, and every mod compiles against the game's IL2CPP assemblies, so packaging only works on a Windows machine (or self-hosted runner) with the game present.

## Prerequisites

- Schedule I installed; `$env:SCHEDULE1_PATH` set if it is not in the default Steam location.
- MelonLoader and S1API deployed into the game directory (the build references `Mods\S1API.Il2Cpp.MelonLoader.dll`); required versions: [`docs/compatibility.md`](compatibility.md).
- .NET **8 SDK** for C# 12, .NET **6 runtime** for the tests/mod target, and PowerShell 7. On this device use the global installation; do not set `DOTNET_ROOT` to the tools-only user directory.
- Close the game before building. Packaging always passes `-p:S1NoDeploy=true`.
- For publication: clean working tree (including submodules), `main` pushed and up to date. Commits, pushes, tags and publication require the owner's explicit instruction.

## 1. Bump the version

The code is the single source of truth (`[assembly: MelonInfo(...)]`, or `Constants.ModVersion` for PotScanner). Never edit version numbers by hand:

```pwsh
pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.4
```

This updates the version-bearing files sequentially (not as an atomic transaction): the `MelonInfo` attribute, the mod's `mod.json`, a new `## 1.0.4 (YYYY-MM-DD)` header in the mod's `CHANGELOG.md` (both under `Source/Mods/<Mod>/docs/`), the inventory row (and detail header) in `AGENTS.md`, and the mod table in `README.md`, plus the current row in `docs/compatibility.md`. Metadata is preflighted before writes; missing or ambiguous rows abort rather than guessing. A changed version does not inherit old gameplay acceptance: the previous version/date remains historical and the new one is marked open. Review the generated diff and fill in the changelog entry afterwards.

## 2. Run the quality gates

```pwsh
dotnet format Source/Mods/S1Mods.sln --verify-no-changes
pwsh Tools/check-version-sync.ps1
pwsh Tools/check-doc-paths.ps1
dotnet build Source/Mods/S1Mods.sln -c Release -p:S1NoDeploy=true
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/test-all.ps1 -Selection all
```

Then verify the mod in-game (Main Menu → load save → use the mod → Main Menu) and record the result in the changelog and in `docs/compatibility.md`.

## 3. Package

```pwsh
pwsh Tools/package-release.ps1 -Mod NotesApp      # one mod
pwsh Tools/package-release.ps1                    # every active mod
```

The script forces a Release rebuild with deployment disabled and writes `Release/<ModName>-v<version>.zip` (the `Release/` folder is git-ignored). Missing or invalid metadata, an absent/empty mod DLL or `Shared.dll`, missing game assemblies, an uninitialized submodule or source changes during the build/copy abort packaging. A previous ZIP is replaced only after the new archive passes verification. Each attempt uses a unique temporary directory; cleanup is confined to that owned directory.

Every ZIP contains `release-manifest.json` with schema version, exact mod/version, Git HEAD, clean/dirty state, a SHA-256 source fingerprint, and the relative path, size and SHA-256 of every payload file. The source fingerprint covers tracked and nonignored untracked working-tree files, recursively including initialized submodules. The manifest itself is excluded from its payload hash list. The verifier reads the archive without extracting it and rejects missing, extra, duplicate, unsafe or hash-mismatched entries. The ZIP mirrors the game directory:

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
├── CHANGELOG.md                   copied from docs/CHANGELOG.md
└── release-manifest.json          source identity + every payload hash
```

`Shared` and the archived `_DiagPerfCounter` tool are never packaged on their own. `Shared.dll` is not versioned separately — when releasing several mods, package them from the same commit so they ship an identical `Shared.dll`.

Both packaging and publication verify `Mods/<Mod>.dll`, `Mods/Shared.dll` and `UserData/<Mod>/mod.json`, including metadata name/version. Dirty-tree packages remain useful for local distribution, but publication rejects them. Commit/push the intended source only when authorized, then package again from that clean commit. An old ZIP with the right filename is insufficient; legacy ZIPs without a manifest and ZIPs from another source state must be regenerated explicitly.

Git-clean alone does not prove that SDK wildcard inputs are tracked. Packaging and source validation therefore reject ignored files of any extension under active mod `src/` directories (except their generated `bin/` and `obj/` trees), ignored build-control files beside/above those projects, ignored PNG/bundle/GLB runtime assets, and ignored embedded-asset inputs such as those under `Source/Mods/WorldLook/assets/embedded/`. This also covers local exclusions such as `.git/info/exclude`. For example, an ignored `LocalPatch.tmp.cs` or `src/scratch/Hidden.cs` aborts instead of being silently compiled into a supposedly clean release. Intended production inputs must be tracked or moved out of these trees. Existing linked sources/resources outside `src/` are tracked and included in the fingerprint. New custom MSBuild inputs or imports outside this bounded layout need corresponding provenance coverage before release.

The manifest detects stale archives and accidental payload changes; it is not a cryptographic signature or a reproducible-build proof. Installed game/framework assemblies, toolchain versions and ignored machine configuration outside the production-input guard are outside the source fingerprint. Payload hashes identify the produced binaries, including `Shared.dll`; gameplay verification remains a separate requirement.

Run the game-free release fixtures without creating a release or building mods:

```pwsh
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/Tests/release.Tests.ps1
```

## 4. Publish on GitHub

1. With explicit owner authorization, commit the version bump and push; package from that clean commit. Regenerate a ZIP previously built from a dirty tree or another commit.
2. Create a tag `<ModName>-v<version>` (one mod per tag keeps release notes focused) and push it.
3. Create a GitHub release from the tag, attach the ZIP from `Release/`, and state in the notes:
   - the game version the package was verified on,
   - the required frameworks, quoted from [`docs/compatibility.md`](compatibility.md) (MelonLoader, S1API, S1MAPI if applicable),
   - the changelog excerpt for this version.
4. Update `docs/compatibility.md` if the verified game version changed.

> **Automated:** steps 2–3 of this section are implemented by `Tools/release-mod.ps1` — it runs the version-sync gate, verifies a clean, pushed branch, packages the ZIP only if missing, validates the manifest against the current source and all payload hashes, pushes the tag and creates the GitHub release with the matching CHANGELOG section as notes. An existing stale/unverifiable ZIP fails rather than being silently reused or rebuilt. The source and archive are checked again immediately before tagging. `-Draft` still pushes a tag and creates a GitHub draft release, so it is a publication operation. `-DryRun` previews locally without fetching, querying remote tags, building, writing notes, tagging or publishing; its cached remote-branch check is not proof of current remote state.

Tags do **not** trigger packaging automatically. The `Release` workflow (`.github/workflows/release.yml`) is manual-only and targets a self-hosted Windows runner with the custom **`schedule1`** label. That machine must have the game, MelonLoader and framework references installed and the game closed. Checkout initializes recursive submodules; setup provides the .NET 8 SDK plus .NET 6 SDK/runtime. The input mod name is passed through an environment variable rather than interpolated into PowerShell source. Packaging has deployment disabled and uploads ZIPs as workflow artifacts; the workflow does not create GitHub releases.

No available/configured `schedule1` runner is assumed. Without one, a dispatched run waits in the queue. Local `Tools/package-release.ps1` remains the supported path on this device; registering a runner is separate setup work.

## Local-only distribution

Packaging can also be used purely locally (for example to copy a mod to a second PC): run `package-release.ps1` and extract the ZIP into the game directory.

Release ZIPs are for **local distribution only** — no upload to NexusMods or other public hosts (Dominik's decision, 2026-09-19).
