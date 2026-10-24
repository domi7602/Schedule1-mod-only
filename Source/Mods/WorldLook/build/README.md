# Canonical WorldLook build

Run from the repository root using PowerShell 7 (`pwsh`). Unity 2022.3.62f3 and, for a full build, .NET SDK 8+ are required. The mod still targets `net6.0` / C# 12. Close the game before building.

## Commands

- Preflight: `pwsh -NoProfile -ExecutionPolicy Bypass -File Source/Mods/WorldLook/build/Build.ps1 -CheckOnly`
- Rebuild only the shader bundle: `pwsh -NoProfile -ExecutionPolicy Bypass -File Source/Mods/WorldLook/build/Build.ps1 -ShadersOnly`
- Full shader + C# build: `pwsh -NoProfile -ExecutionPolicy Bypass -File Source/Mods/WorldLook/build/Build.ps1`
- Validate a previously generated bundle: `pwsh -NoProfile -ExecutionPolicy Bypass -File Source/Mods/WorldLook/build/Verify-ShaderBundle.ps1`
- C# compile using an already verified bundle: `dotnet build Source/Mods/WorldLook/src/WorldLook.csproj -c Release -p:S1NoDeploy=true`

`-GamePath`, `-UnityPath`, `-DotNetPath` and `-Configuration Debug` are optional. Game resolution is explicit path, then `SCHEDULE1_PATH`, legacy `SCHEDULE_I_GAME_DIR`, then the repository's default Steam path. Unity resolution also accepts `UNITY_EDITOR_PATH`. The SDK lookup prefers the user-local `.dotnet/dotnet.exe` when present. The script does not install tools.

## Inputs and output

The full script always regenerates the shader bundle. It copies the pinned package/project settings, the byte-preserved `ReferenceWater.shader` and the single current Editor builder into a fresh isolated project under ignored `build/logs/<run>/unity`. Unity package restore and generated build fixtures stay there. The source manifest retains the original `14.0.11` URP/Core pins; this editor must resolve both packages to `14.0.12`.

The validated D3D11 / Linear / URP 14 fixture keeps main-light, cascaded, screen-space and soft-shadow branches eligible. Its explicit bundle map includes only ReferenceWater and its eight-entry shader variant collection. The newly rebuilt bundle is loaded in the Editor and checked for the sole shader, the requested collection entries, pass availability and all 17 material properties. No custom sky or pipeline asset enters the bundle.

Only after those checks does the script publish `assets/embedded/worldlook-shaders.bundle` and `assets/embedded/shader-build.json`. The nested directory intentionally avoids the repository deploy helper's nonrecursive `assets/*.bundle` scan. The logical embedded resource remains `WorldLook.Shaders.bundle`.

The `.csproj` validates bundle SHA-256/size, editor/package/variant/property contracts and the exact current shader/build-source provenance before every normal C# build. A missing or stale input fails the build and requires regeneration. Original settings/lock hashes and generated fixture/resolved-lock hashes are separately recorded.

Full builds use the repository's inherited props/targets and Shared reference; **both restore and build explicitly pass `S1NoDeploy=true`**. Output is `src/bin/<Configuration>/net6.0/WorldLook.dll`. No game files are copied by this script. A plain `dotnet build` without the no-deploy property still follows the normal repository deployment rules.

Each run keeps logs and source snapshots. `shader-check.json` hashes the bundle and shader receipt. Full builds additionally write `build-info.json`, hash the actual embedded resource read back from the compiled DLL, and record production input hashes excluding `bin/obj` and archives. Failed shader regeneration leaves the previous published pair intact; a publication-copy exception attempts to restore that pair from the run's backup. This is not a cross-process transactional publication.

Editor validation and collection membership do not establish execution of every native GPU variant, game appearance, performance or lifecycle acceptance. Those checks remain a separate in-game step.
