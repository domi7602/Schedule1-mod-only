# Version and release metadata synchronization

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 33 of 34 identifier-shaped tokens resolve (1 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Establish actual release identities

Inspect the current project and packaging mechanism. The uploaded workspace used MelonInfo in Mod.cs, docs/mod.json, docs/CHANGELOG.md and the AGENTS.md mod inventory. Confirm those are still the relevant files and distinguish source metadata from the artifact actually loaded in game.

A version string is not a content hash. Two different binaries can carry the same version.

## Procedure

1. Read the diff and affected feature implementation. Separate implemented behavior, planned work and observed tests.
2. Choose the version according to the existing repository policy and requested release scope. A documentation correction does not automatically require a code release.
3. Inspect the actual Tools/bump-version.ps1 or equivalent. Use its supported preview mode before mutation; do not assume the script is atomic or regex-safe.
4. Update relevant metadata consistently and preserve unrelated entries.
5. Inspect the resulting diff for duplicate headers, bracketed version formatting, placeholder release text and accidental unrelated changes.
6. Build and package through the verified isolated route; deploy/publish only within authorization.
7. Record exact artifacts and checks. Commit/push only as the current task/repository authorization permits.

## Historical regression cases to check

The supplied notes reported older bump-script failures with custom changelog headings, bracketed versions and generic “Version bump” entries. These are useful regression fixtures, not proof the current script still fails. Check the script's behavior on copies before modifying production metadata.

## Feature evidence

A keyword occurrence is a starting point. Inspect reachable behavior, integration and tests before describing a feature as implemented. “Verified in game” requires an actual observation with the relevant setup.

## Acceptance

The intended version is consistent across the actual consumers, release notes describe the real diff, and generated artifacts correspond to that source state. Unexecuted gameplay checks remain explicit.
