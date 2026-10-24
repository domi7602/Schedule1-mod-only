# Build integration verification — 2026-10-22

Scope: canonical build scripts, Editor-only water builder, project version, embedded-resource project integration and owned bundle retirement. This record does not cover the independently edited production lighting/water code.

## Executed

- PowerShell parser checks for both build scripts and XML parsing of `src/WorldLook.csproj`: pass.
- `Build.ps1 -CheckOnly`: Unity 2022.3.62f3, .NET SDK 8.0.425 and installed S1API FileVersion 3.2.1.0 found. No restore/production compilation in this check.
- Final `Build.ps1 -ShadersOnly`: pass. Run: `build/logs/20261022-175014-057-0d41c4bc/`.
- Newly built bundle loaded in Unity: exactly ReferenceWater plus its variant collection; one supported shader/pass, all 17 required properties and all 8 requested SVC entries present. No custom sky or pipeline fixture asset in the bundle. No shader errors/warnings or C# compiler errors/warnings were found by the targeted final-log scan.
- Standalone verifier against the published nested bundle: pass.
- One accepted valid pair plus seven rejected invalid copies: corrupt bundle, stale source hash, wrong variant count, custom sky entry, missing property, missing provenance entry and duplicate provenance entry. Evidence: `build/logs/validator-4251ff78d2f841c5bde16ee1f5d2b173/validator-checks.json`.
- A real intervening edit to `build/Build.ps1` was rejected as stale before the final regeneration.
- Scoped `git diff --check`: pass.

## Artifact identity

- `Source/Mods/WorldLook/assets/embedded/worldlook-shaders.bundle`: SHA-256 `596f9f989ac5ba78e6d478dbbbad094f42ac5bc0561a6a35cef4ff3a1e3bd6bf`.
- `Source/Mods/WorldLook/assets/embedded/shader-build.json`: SHA-256 `d2b6d369ec5858e7fcbc0978ae2eebf8d24443ef69a955c92ff8da07f79e2830`.
- Approved `unity/Assets/S1Mods/ReferenceWater.shader` remains byte-identical: SHA-256 `2ce7e343c100f2f9d6f93aef3380039d5ee34d2787b283dc1dde6d46ac952a47`.
- Seven retired files were copied byte-for-byte and hash-checked under `Source/Archive/WorldLook/LegacyUnity/`; details in that folder's `retirement-hashes.json`. Original active editor scripts/metadata and the former top-level bundle (assets/s1reference.bundle) were then removed. The top-level deployable bundle scan is empty.

The final shader receipt records 28 current source/settings/build input hashes, resolved URP/Core 14.0.12, D3D11 and Linear color space. Manifest package pins remain 14.0.11. Canonical settings/package files were not rewritten by Unity: builds used isolated source copies under the run logs.

## Findings and remaining checks

The first run built the bundle but failed the added collection readback because constructing a ShaderVariant from a loaded player shader requires unavailable Editor snippets. The final code validates descriptors against the source shader and uses their public shader identity field only for membership readback. The subsequent builds passed; this introduces no runtime pipeline/variant mutation.

The compiler log shows four vertex and four fragment variants after settings filtering, matching the prior validated standalone build (`20261022-133617-494`). Eight requested/present SVC entries do not prove execution of eight native GPU combinations; no stripping policy or shader was retuned.

Production C# compilation, compiled-DLL embedded-resource readback, tests, independent review and deployment belong to the parent workflow and were not executed here. No game files were installed/changed, and no native GPU/game visual, performance or lifecycle acceptance is claimed. A later full build regenerates the bundle and must be identified by its own receipt/hashes.
