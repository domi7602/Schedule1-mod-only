# Retired WorldLook Unity build inputs

Retired during the approved native-sky/canonical-repository integration on 2026-10-22.

- `Editor/` contains byte-identical archival copies of `BuildBundles.cs`, `BuildReference.cs`, `ShaderDiagnostics.cs` and their original Unity metadata. They are outside active Unity `Assets`.
- `s1reference.bundle` is the previous external 0.1.5 bundle, moved out of the top-level deployable asset directory. SHA-256: `04a2afb6321e06321869740541d19ea71d017b3148592a7971beb609ad4c3a6f`.
- `retirement-hashes.json` records each original path, preserved size and verified archival SHA-256. The pre-task backup remains under `.backups/WorldLook/native-baseline-20261022-171619`.

The active builder is `Source/Mods/WorldLook/build/Build.ps1`. Only its water-only embedded bundle is an active build input. This archive operation did not change installed game files.
