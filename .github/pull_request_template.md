# Pull Request

## Summary
<!-- What does this PR change? Link issues if applicable. -->

## Checklist
- [ ] `dotnet build Source/Mods/S1Mods.sln -c Release` — 0 Errors / 0 Warnings
- [ ] `dotnet format --verify-no-changes` clean
- [ ] `pwsh Tools/gen-sln.ps1` — no diff (deterministic GUIDs)
- [ ] In-game verified (Main Menu → Game → Main Menu)
- [ ] `AGENTS.md` + `CHANGELOG.md` + `mod.json` + `Mod.cs` in sync (`Tools/bump-version.ps1`)
- [ ] `s1interop analyze` clean (if applicable)
- [ ] Knowledge links OK (`Tools/fix-knowledge-paths.ps1 -WhatIf`)

## Testing
<!-- How did you test? Steps, save file, screenshots, Latest.log excerpt -->

## Notes
<!-- Breaking changes, follow-ups, TODOs -->
