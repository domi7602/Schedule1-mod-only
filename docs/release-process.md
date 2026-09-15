# Release Process

1. Update a mod version only with `pwsh Tools/bump-version.ps1 -Mod <Name> -Version <Version>`.
2. Run `pwsh Tools/check-version-sync.ps1`, tests, and a Release build.
3. Package with `pwsh Tools/package-release.ps1 -Mod <Name>`.
4. Verify the generated ZIP contains `Mods/<Mod>.dll` and `UserData/<Mod>/mod.json` plus the PDB when available.
5. Create and push a version tag only after in-game verification. The release workflow uploads generated ZIPs as workflow artifacts.

GitHub-hosted runners do not contain proprietary game assemblies. Release packaging therefore requires a local or self-hosted Windows runner with Schedule I installed.
