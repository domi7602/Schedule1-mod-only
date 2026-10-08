# Compatibility

This page records which game and framework versions each mod was built against and verified on. It is derived from the inventory in [`AGENTS.md`](../AGENTS.md) §2 and the per-mod changelogs; when they disagree, the mod's changelog (`Source/Mods/<Mod>/docs/CHANGELOG.md`) wins.

## Toolchain

| Component | Version | Notes |
|---|---|---|
| Schedule I (Steam default) | v0.4.6f13 | IL2CPP, Unity 2022.3. Baseline for the September 2026 verification pass. |
| Schedule I (installed runtime) | v0.4.7f11 | Verified 2026-10-07 via `MelonLoader\Latest.log`. Local decompiles under `GameReferences/` regenerated 2026-10-07 against f11 (ilspycmd 9.1.0.7988, project mode); `ProductIconManager.GenerateIcons` is absent, `GenerateRuntimeIcons(string)` present. |
| Schedule I (previous Open Beta baseline) | v0.4.7f9 | buildid 25698382, verified 2026-10-05. Spot-check set (EClothingSlot/EClothingColor, MoneyManager, EEmployeeType, EConversationCategory) passed on the f9 decompiles. |
| MelonLoader | 0.7.3 | `net6` runtime. |
| S1API | 3.2.1-beta.8 (git submodule `ThirdParty/S1API`) | Build reference — submodule checked out at the beta.8 tag 2026-10-05 (commit f65ae40 = deployed build). Stay on the 3.2.1-beta line for the 0.4.7f6+ renames. |
| S1API (deployed runtime) | 3.2.1-beta.8 (2026-10-05) | `Mods\S1API.Il2Cpp.MelonLoader.dll` + `Plugins\S1APILoader.MelonLoader.dll`. The 3.2.0 stable build does not know the 0.4.7f6 renames. |
| S1MAPI | 2.0.1 (git submodule `ThirdParty/S1MAPI`) | Deployed as `UserLibs\S1MAPI_Il2Cpp.dll`. |
| Target framework | `net6.0`, C# 12, nullable enabled | Shared via `Source/Mods/Directory.Build.props`. |

## Verification matrix

"Verified" means a functional check in a real game session, recorded in the mod's changelog or the inventory in `AGENTS.md`. No automated in-game test exists; GitHub-hosted CI cannot run the game.

The rows below record the game version each check ran on. The installed runtime is now v0.4.7f11, and none of the rows has been re-run on it yet. Treat f11 behaviour as unverified until a session is logged.

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
| TaxiDriver | 0.7.0 | v0.4.7f6 (beta) | 2026-09-25 | yes | yes (`S1MAPI.Gltf.GltfLoader`) | Feasibility spike / developer tool. Stages 1-3 live-verified 2026-09-25; the current Pakete C-G test round is open. |
| StorageScanner | 0.1.1 | — | — | yes | no | v0.1.0 never ran a scan (readiness gate blocked on `IsContentCulled`); v0.1.1 fix deployed 2026-10-04, in-game verification open. |

## Known version-drift risks

Collected during the v0.4.7 Open Beta impact analysis (2026-09-19). These are expectations, not confirmed breakages:

- Avatar refactor (`AvatarSettings` → `AvatarObjects` / `Outfit`): may affect `PocketShop` (NPC portraits) and `CustomSkateboard` (visual patches).
- NPC ragdoll / `NPCHealth` changes: handled for `HitmanPhone` in v0.2.9.
- Appearance save conversion in the beta is one-way — back up saves before switching branches.

`Tools/bootstrap-game-references.ps1` regenerates local decompiles after a game update; a Release build (`dotnet build -c Release -p:S1NoDeploy=true`) is the quickest API-drift detector.

## Updating this page

Run `pwsh Tools/check-version-sync.ps1` after a version bump; it checks code, `mod.json`, `README.md` and `AGENTS.md`. The verification dates above are maintained by hand — update them together with the mod's `CHANGELOG.md` entry.
