# Compatibility

This page records which game and framework versions each mod was built against and verified on. It is derived from the inventory in [`AGENTS.md`](../AGENTS.md) §2 and the per-mod changelogs; when they disagree, the mod's changelog (`Source/Mods/<Mod>/docs/CHANGELOG.md`) wins.

## Toolchain

| Component | Version | Notes |
|---|---|---|
| Schedule I (Steam default) | v0.4.6f13 | IL2CPP, Unity 2022.3. Baseline for the September 2026 verification pass. |
| Schedule I (Open Beta) | v0.4.7f6 | Development target since 2026-09-23. Local decompiles under `GameReferences/` may still be 0.4.6f13. |
| MelonLoader | 0.7.3 | `net6` runtime. |
| S1API | 3.2.0 (git submodule `ThirdParty/S1API`, tag `v3.2.0` + 4 commits) | Build reference. |
| S1API (deployed runtime) | 3.2.1-beta.5 | Prebuilt beta used for the 2026-09-23/24 verifications (HitmanPhone). |
| S1MAPI | 2.0.1 (git submodule `ThirdParty/S1MAPI`) | Deployed as `UserLibs\S1MAPI_Il2Cpp.dll`. |
| Target framework | `net6.0`, C# 12, nullable enabled | Shared via `Source/Mods/Directory.Build.props`. |

## Verification matrix

"Verified" means a functional check in a real game session, recorded in the mod's changelog or the inventory in `AGENTS.md`. No automated in-game test exists; GitHub-hosted CI cannot run the game.

| Mod | Version | Verified on | Date | Needs S1API | Needs S1MAPI | Notes |
|---|---|---|---|---|---|---|
| NotesApp | 1.0.3 | v0.4.6f13 | 2026-09-15 | yes | no | |
| CalculatorApp | 0.2.3 | v0.4.6f13 | 2026-09-15 | yes | no | |
| PotScanner | 0.5.4 | v0.4.6f13 | 2026-09-15 | yes | no | |
| BankApp | 0.4.4 | v0.4.6f13 | 2026-09-15 | yes | no | |
| CustomSkateboard | 1.1.5 | v0.4.6f13 | 2026-09-15 | yes | no | |
| MoreSaveSlots | 1.0.12 | v0.4.6f13 | 2026-09-15 | yes (`S1API.Utils.EventHelper`) | no | Harmony patches on the save menus. |
| BusinessIncome | 0.1.6 | v0.4.6f13 | 2026-09-19 | yes | no | |
| PocketShop | 0.3.2 | v0.4.6f13 | 2026-09-20 | yes | no | |
| StackLimitMod | 0.1.6 | v0.4.6f13 | 2026-09-20 | yes | no | |
| AutoPackagingStation | 0.3.3 | v0.4.6f13 | 2026-09-20 | yes | yes (`S1MAPI.Gltf.GltfLoader`) | |
| HitmanPhone | 0.2.9 | v0.4.7f6 (beta) | 2026-09-24 | yes | no | v0.2.9 adds a `NPCDeathPatch` for the 0.4.7 `NPCHealth` change; v0.2.8 was verified on v0.4.6f13 (2026-09-15). |
| Weather | 0.1.0 | — | — | yes | no | Code complete (2026-09-24); in-game verification open. |
| MessagesPlus | 0.1.1 | — | — | yes (`S1API.Lifecycle`, `S1API.UI`) | no | Code complete (2026-09-24); in-game verification open. Written against the 0.4.7f6 `MSGConversation` API with fallbacks. |
| TaxiDriver | 0.1.0 | v0.4.7f6 (beta) | 2026-09-25 | yes | yes (`S1MAPI.Gltf.GltfLoader`) | Feasibility spike / developer tool. |

## Known version-drift risks

Collected during the v0.4.7 Open Beta impact analysis (see `memory-bank/decisionLog.md`, 2026-09-19). These are expectations, not confirmed breakages:

- Avatar refactor (`AvatarSettings` → `AvatarObjects` / `Outfit`): may affect `PocketShop` (NPC portraits) and `CustomSkateboard` (visual patches).
- NPC ragdoll / `NPCHealth` changes: handled for `HitmanPhone` in v0.2.9.
- Appearance save conversion in the beta is one-way — back up saves before switching branches.

`Tools/bootstrap-game-references.ps1` regenerates local decompiles after a game update; a Release build (`dotnet build -c Release -p:S1NoDeploy=true`) is the quickest API-drift detector.

## Updating this page

Run `pwsh Tools/check-version-sync.ps1` after a version bump; it checks code, `mod.json`, `README.md` and `AGENTS.md`. The verification dates above are maintained by hand — update them together with the mod's `CHANGELOG.md` entry.
