# Pull Request

## Summary
<!-- What does this PR change? Link issues if applicable. -->

## Checklist
- [ ] `dotnet build Source/Mods/S1Mods.sln -c Release -p:S1NoDeploy=true` — 0 errors / 0 warnings
- [ ] `dotnet format Source/Mods/S1Mods.sln --verify-no-changes` clean
- [ ] `pwsh Tools/gen-sln.ps1` — no diff (deterministic GUIDs)
- [ ] `pwsh Tools/check-version-sync.ps1` green (code ↔ `mod.json` ↔ `README.md` ↔ `AGENTS.md`, via `Tools/bump-version.ps1`)
- [ ] `pwsh Tools/check-doc-paths.ps1` green (no dead repository paths in Markdown)
- [ ] Tests pass (`dotnet test Source/Mods/S1Mods.sln -c Release`; at least the pure-logic suites if the game is not installed)
- [ ] In-game verified (Main Menu → Game → Main Menu); result noted in the mod's `CHANGELOG.md` and `docs/compatibility.md`
- [ ] `s1interop analyze` report read, no unexpected findings (optional, advisory)
- [ ] Documentation updated where behaviour changed (mod `docs/README.md`, root `README.md` table, `AGENTS.md` §2)

## Testing
<!-- How did you test? Steps, save file, screenshots, Latest.log excerpt -->

## Notes
<!-- Breaking changes, follow-ups, TODOs -->
