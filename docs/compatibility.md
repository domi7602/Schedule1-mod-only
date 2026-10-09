# Compatibility

This page records which game and framework versions each mod was built against and verified on. It is derived from the inventory in [`AGENTS.md`](../AGENTS.md) §2 and the per-mod changelogs; when they disagree, the mod's changelog (`Source/Mods/<Mod>/docs/CHANGELOG.md`) wins.

## Toolchain

| Component | Version | Notes |
|---|---|---|
| Schedule I (Steam default) | v0.4.6f13 | IL2CPP, Unity 2022.3. Baseline for the September 2026 verification pass. |
| Schedule I (installed runtime) | v0.4.7f12, Steam build 25796331 | `Latest.log` on 2026-10-08; Unity 2022.3.62f2. Live `Assembly-CSharp.dll` SHA-256 `441720a60c7729945773e050e4a92bf0ffbf062124626348407fef4c59471a6d`. The local `GameReferences/` decompile was regenerated from that same assembly on 2026-10-08 (2 234 files) and matches it, but it is gitignored — re-run `Tools/bootstrap-game-references.ps1` after every game update before quoting it as evidence. |
| Schedule I (previous Open Beta baseline) | v0.4.7f9 | buildid 25698382, verified 2026-10-05. Spot-check set (EClothingSlot/EClothingColor, MoneyManager, EEmployeeType, EConversationCategory) passed on the f9 decompiles. |
| MelonLoader | 0.7.3 | `net6` runtime. |
| S1API | 3.2.1-beta.8 (git submodule `ThirdParty/S1API`, commit `f65ae40afd1e081750705d810a39efda9c5e74eb`) | Build reference is the beta.8 source tag. |
| S1API (deployed runtime) | 3.2.1-beta.8; assembly version 3.2.1.0 | `Mods\S1API.Il2Cpp.MelonLoader.dll`, SHA-256 `f3fb5a6c509f0f2a4d5379dde21d4a1dd915f6ba6548d8ac8fb2c6befa84b2e7`. It is the local PR #353 compatibility build documented for f11; f12 `Latest.log` shows it loaded, but this is not proof that every patch or TaxiDriver feature works on f12. |
| S1MAPI (deployed runtime) | 2.0.1; assembly version 2.0.1.0 | `UserLibs\S1MAPI_Il2Cpp.dll`, SHA-256 `ef612a9055ff0b3c4ccaec4e3288fd7f54118cb4764a981233443ed44cc27998`; checked-in source commit `89b8d0173946324bb21b3eb1fa584ee9c5f3e618`. |
| Target framework | `net6.0`, C# 12, nullable enabled | Shared via `Source/Mods/Directory.Build.props`. |

## TaxiDriver local Beta-8 build evidence (2026-10-08)

- No-deploy command: `dotnet build Source/Mods/TaxiDriver/src/TaxiDriver.csproj -c Release -p:S1NoDeploy=true`. MSBuild resolved `GameDir` to the installed Schedule I directory and `Il2CppAssemblies`/`MelonNet6` to that game's live MelonLoader folders. Result: **Build succeeded, 0 warnings, 0 errors**. `S1NoDeploy=true` disables the deployment targets; no mod was installed or deployed. No in-game TaxiDriver run was performed.
- Reference identities used by that build: S1API `3.2.1.0` (runtime log says `3.2.1-beta.8`), S1MAPI `2.0.1.0`, `Assembly-CSharp` `0.0.0.0`, `UnityEngine.CoreModule` `0.0.0.0`, `Il2CppInterop.Runtime` `1.5.1.0`, `0Harmony` `2.10.2.0`, and `Il2CppAstarPathfindingProject` `0.0.0.0`. Game/Unity versions are listed above; IL2CPP-generated game proxies carry `0.0.0.0` assembly versions, so their SHA-256 values identify the exact files.
- SHA-256: S1API `f3fb5a6c509f0f2a4d5379dde21d4a1dd915f6ba6548d8ac8fb2c6befa84b2e7`; S1MAPI `ef612a9055ff0b3c4ccaec4e3288fd7f54118cb4764a981233443ed44cc27998`; `Assembly-CSharp.dll` `441720a60c7729945773e050e4a92bf0ffbf062124626348407fef4c59471a6d`; `UnityEngine.CoreModule.dll` `0aa474bc0e139c03953db8f22f1b38a1d3948d50394534681d0cc80661060829`.
- The deployed S1API DLL is the local PR #353 compatibility build whose provenance targets f11; it loaded in the f12 log, but that does not prove all runtime patches work on f12. Source base commits: S1API `f65ae40afd1e081750705d810a39efda9c5e74eb`; S1MAPI `89b8d0173946324bb21b3eb1fa584ee9c5f3e618`.
- Local decompile signatures checked: `GameLifecycle.OnPreLoad` and `OnPreSceneChange` are `Action` events; `PhoneApp` exposes `OnCreated`, `OnCreatedUI(GameObject)`, and `OnDestroyed`; `S1MAPI.Gltf.GltfLoader.LoadFromFile(string, Shader?)` exists. f12 game targets also include `LandVehicle.UpdateThrottle()`, `LandVehicle.UpdateSteerAngle()`, `GameInput.OnVehicleHandbrake()`, `StorageDoorAnimation.Open()`, and `StorageDoorAnimation.SetIsOpen(bool)`.
- The installed S1MAPI `GltfImporter.AttachMesh` builds GLB mesh nodes with `MeshFilter` + `MeshRenderer` and assigns per-submesh materials. Unity 2022.3.62f2 proxies expose `MeshFilter.sharedMesh`, `SkinnedMeshRenderer.sharedMesh`, `Mesh.vertexCount`, `Mesh.subMeshCount`, and `Mesh.GetIndexCount(int) -> uint`; TaxiDriver uses these to reject renderers without vertices or indexed geometry. This verifies available API surface, not what renders in-game.
- **Build compatibility only:** these signatures and a successful compile do not prove patch invocation order, save/scene behavior, GLB appearance, vehicle lifecycle, fares, or passenger safety in-game. Do not claim complete Beta-8 compatibility without runtime evidence.

## Verification matrix

"Verified" means a functional check in a real game session, recorded in the mod's changelog or the inventory in `AGENTS.md`. No automated in-game test exists; GitHub-hosted CI cannot run the game.

The rows below record the game version each check ran on. The installed runtime is v0.4.7f12. Older live checks remain historical; no TaxiDriver gameplay check has been run against the current f12 source changes.

| Mod | Version | Verified on | Date | Needs S1API | Needs S1MAPI | Notes |
|---|---|---|---|---|---|---|
| NotesApp | 1.0.3 | v0.4.6f13 | 2026-09-15 | yes | no | |
| CalculatorApp | 0.2.3 | v0.4.6f13 | 2026-09-15 | yes | no | |
| PotScanner | 0.7.0 | v0.4.6f13 | 2026-09-15 | yes | no | Core dashboard verified 2026-09-15; v0.7.0 is a view rewrite on the shared palette — verification open. |
| BankApp | 0.4.4 | v0.4.6f13 | 2026-09-15 | yes | no | |
| CustomSkateboard | 1.1.5 | v0.4.6f13 | 2026-09-15 | yes | no | |
| MoreSaveSlots | 1.0.12 | v0.4.6f13 | 2026-09-15 | yes (`S1API.Utils.EventHelper`) | no | Harmony patches on the save menus. |
| BusinessIncome | 0.1.6 | v0.4.6f13 | 2026-09-19 | yes | no | |
| PocketShop | 0.3.2 | v0.4.6f13 | 2026-09-20 | yes | no | |
| StackLimitMod | 0.1.7 | v0.4.6f13 | 2026-09-20 | yes | no | v0.1.6 verified 2026-09-20; v0.1.7 adds ingredient stacking — verification open. |
| AutoPackagingStation | 0.3.3 | v0.4.6f13 | 2026-09-20 | yes | yes (`S1MAPI.Gltf.GltfLoader`) | |
| HitmanPhone | 0.2.9 | v0.4.7f6 (beta) | 2026-09-24 | yes | no | v0.2.9 adds a `NPCDeathPatch` for the 0.4.7 `NPCHealth` change; v0.2.8 was verified on v0.4.6f13 (2026-09-15). |
| Weather | 0.4.0 | — | — | yes | no | In-game verification open (0.4.0 = rebuild to the approved mockup; no version of this mod has been verified in-game yet). |
| MessagesPlus | 0.4.1 | v0.4.7f6 (beta) | 2026-09-30 | yes (`S1API.Lifecycle`, `S1API.UI`) | no | v0.4.0 verified in-game 2026-09-30; v0.4.1 (permanent whole-app dark mode + instant deal-window theming) verification open. |
| TaxiDriver | 0.8.2 | v0.4.7f6 (beta; historical) | 2026-09-25 | yes | yes (`S1MAPI.Gltf.GltfLoader`) | Earlier drive/visual checks only. No-deploy Release build against installed f12/Beta.8 references passed 2026-10-08 (0 warnings/errors); no current in-game test. |
| StorageScanner | 0.1.1 | — | — | yes | no | v0.1.0 never ran a scan (readiness gate blocked on `IsContentCulled`); v0.1.1 fix deployed 2026-10-04, in-game verification open. |

## Known version-drift risks

Collected during the v0.4.7 Open Beta impact analysis (2026-09-19). These are expectations, not confirmed breakages:

- Avatar refactor (`AvatarSettings` → `AvatarObjects` / `Outfit`): may affect `PocketShop` (NPC portraits) and `CustomSkateboard` (visual patches).
- NPC ragdoll / `NPCHealth` changes: handled for `HitmanPhone` in v0.2.9.
- Appearance save conversion in the beta is one-way — back up saves before switching branches.

`Tools/bootstrap-game-references.ps1` regenerates local decompiles after a game update; a Release build (`dotnet build -c Release -p:S1NoDeploy=true`) is the quickest API-drift detector.

## Updating this page

Run `pwsh Tools/check-version-sync.ps1` after a version bump; it checks code, `mod.json`, `README.md` and `AGENTS.md`. The verification dates above are maintained by hand — update them together with the mod's `CHANGELOG.md` entry.
