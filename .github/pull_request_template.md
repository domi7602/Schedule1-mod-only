# Pull Request

## Summary
<!-- What does this PR change? Link issues if applicable. -->

## Checklist

- [ ] `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/verify.ps1 -Mode Auto` passed; selected mode and report path recorded below. Full requires game references and a closed game; Pure includes game-free xUnit suites, the LightControl console host and tool tests. All modes disable deployment.
- [ ] For documentation, tests, tooling, ignore rules, CI or dependency changes: `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/verify-clean-checkout.ps1 -Mode Pure` passed against the final working files.
- [ ] Relevant build/test warnings reviewed; known framework warnings are recorded rather than hidden. Missing game references or unexecuted checks are stated explicitly.
- [ ] If runtime behaviour changed, the affected gameplay checks and artifact identity are recorded in the mod's changelog and compatibility record, or clearly marked pending. Documentation-only changes need no new gameplay acceptance.
- [ ] Documentation reflects the resulting behaviour. Release version changes used `Tools/bump-version.ps1`; source/history evidence was preserved.
- [ ] Optional `s1interop analyze` findings reviewed when relevant; its exit code is not a verification gate.

## Testing
<!-- Selected mode, report paths, focused tests, warnings and limits. For runtime changes, add scenario/version/log evidence. Builds alone do not prove gameplay acceptance. -->

## Notes
<!-- Breaking changes, follow-ups, TODOs -->
