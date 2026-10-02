# Compatibility

This page records which game and framework versions each mod was built against and verified on. It is derived from the inventory in [`AGENTS.md`](../AGENTS.md) §2 and the per-mod changelogs; when they disagree, the mod's changelog (`Source/Mods/<Mod>/docs/CHANGELOG.md`) wins.

## Toolchain

| Component | Version | Notes |
|---|---|---|
| Schedule I (Steam default) | v0.4.6f13 | IL2CPP, Unity 2022.3. Baseline for the September 2026 verification pass. |
| Schedule I (Open Beta) | v0.4.7f7 | Development target; running install verified 0.4.7f7 on 2026-10-02 (`Latest.log`). Local decompiles under `GameReferences/` may still be 0.4.6f13. |
| MelonLoader | 0.7.3 | `net6` runtime. |
| S1API | 3.2.1-beta.7 (git submodule `ThirdParty/S1API`) | Build reference; stay on the 3.2.1-beta line for the 0.4.7f6 renames. |
| S1API (deployed runtime) | 3.2.1-beta.7 | `Mods\S1API.Il2Cpp.MelonLoader.dll` + `Plugins\S1APILoader.MelonLoader.dll`. The 3.2.0 stable build does not know the 0.4.7f6 renames. |
| S1MAPI | 2.0.1 (git submodule `ThirdParty/S1MAPI`) | Deployed as `UserLibs\S1MAPI_Il2Cpp.dll`. |
| Target framework | `net6.0`, C# 12, nullable enabled | Shared via `Source/Mods/Directory.Build.props`. |

## Verification matrix

"Verified" means a functional check in a real game session, recorded in the mod's changelog or the inventory in `AGENTS.md`. No automated in-game test exists; GitHub-hosted CI cannot run the game.

| Mod | Version | Verified on | Date | Needs S1API | Needs S1MAPI | Notes |
|---|---|---|---|---|---|---|
| NotesApp | 1.0.4 | v0.4.6f13 | 2026-09-15 | yes | no | 1.0.4 = internal cleanup (search debounce, 2026-10-02); startup smoke clean. |
| CalculatorApp | 0.2.4 | v0.4.6f13 | 2026-09-15 | yes | no | 0.2.4 = internal cleanup (merge, input gates, 2026-10-02); tests 18/18; startup smoke clean. |
| PotScanner | 0.7.1 | v0.4.6f13 | 2026-09-15 | yes | no | Core dashboard verified 2026-09-15; 0.7.0 view rewrite + 0.7.1 cleanup (2026-10-02): startup smoke clean, view verification open. |
| BankApp | 0.4.5 | v0.4.6f13 | 2026-09-15 | yes | no | 0.4.5 = BankTheme -> shared GamePalette (bit-identical values) + cached slot state (2026-10-02); startup smoke clean. |
| CustomSkateboard | 1.1.6 | v0.4.6f13 | 2026-09-15 | yes | no | 1.1.6 = internal cleanup (ObjLoader merge, FixedUpdate gate, 2026-10-02); startup smoke clean. |
| MoreSaveSlots | 1.0.13 | v0.4.6f13 | 2026-09-15 | yes (`S1API.Utils.EventHelper`) | no | Harmony patches on the save menus. 1.0.13 = internal cleanup (2026-10-02); startup smoke clean. |
| BusinessIncome | 0.1.8 | — | — | yes | no | v0.1.6 verified 2026-09-19; v0.1.7 (payout icon) + v0.1.8 (cleanup, 2026-10-02) verification open; startup smoke clean. |
| PocketShop | 0.3.10 | v0.4.6f13 | 2026-09-20 | yes | no | Layout playtest open. 0.3.10 = internal cleanup (2026-10-02); first-open viewport live-verified in the 2026-10-02 smoke (`viewport=546px`). |
| StackLimitMod | 0.1.8 | v0.4.6f13 | 2026-09-20 | yes | no | v0.1.6 verified 2026-09-20; v0.1.7 (ingredients) + v0.1.8 (cleanup, 2026-10-02) verification open; smoke: 20/20 patches applied. |
| AutoPackagingStation | 0.3.4 | v0.4.6f13 | 2026-09-20 | yes | yes (`S1MAPI.Gltf.GltfLoader`) | v0.3.3 verified 2026-09-20; v0.3.4 = dead legacy buffer UI removed + perf (2026-10-02); tests 28/28; startup smoke clean. |
| HitmanPhone | 0.3.0 | v0.4.7f6 (beta) | 2026-09-24 | yes | no | v0.2.9 verified 2026-09-24 (offer -> receipt -> payout); v0.3.0 = internal cleanup, no intended behaviour change (2026-10-02); startup smoke clean. |
| Weather | 0.4.2 | — | — | yes | no | In-game verification open (no version verified in-game yet). 0.4.2 = poll throttled to 0.5 s (2026-10-02); startup smoke clean. |
| MessagesPlus | 0.5.0 | v0.4.7f6 (beta) | 2026-09-30 | yes (`S1API.Lifecycle`, `S1API.UI`) | no | v0.4.0 verified 2026-09-30. **v0.5.0 removes the one-time legacy restore** (behaviour removal) + file merges + sweep latch (2026-10-02); smoke: 18/18 patches applied. |
| TaxiDriver | 0.8.1 | v0.4.7f6 (beta) | 2026-09-25 | yes | yes (`S1MAPI.Gltf.GltfLoader`) | Feasibility spike / developer tool. Stages 1-3 live-verified 2026-09-25; v0.8.0 landscape redesign + v0.8.1 file merges: verification open; Pakete C-G round open. |

## 2026-10-02 compaction session (build evidence)

All 15 mods were restructured for size and performance (file merges, dead-code removal, hot-path fixes; no intended behaviour changes except MessagesPlus 0.5.0, which removes the legacy restore). Evidence from the same day:

- Full solution build: **0 errors** (Release, `net6.0`); `dotnet format` and `check-doc-paths` green.
- Test suites: **156/156** (Shared 64, TaxiDriver 46, AutoPackagingStation 28, CalculatorApp 18).
- In-game startup smoke on v0.4.7f7: all mods load; `PatchGuard` reports fully applied (APS 1/1, CustomSkateboard 7/7, MessagesPlus 18/18, StackLimitMod 20/20); no exceptions in `Latest.log`.
- Interactive flows were only partially re-exercised (PocketShop directory open log-verified); per-mod verification status stays as noted above.

## Known version-drift risks

Collected during the v0.4.7 Open Beta impact analysis (2026-09-19). These are expectations, not confirmed breakages:

- Avatar refactor (`AvatarSettings` → `AvatarObjects` / `Outfit`): may affect `PocketShop` (NPC portraits) and `CustomSkateboard` (visual patches).
- NPC ragdoll / `NPCHealth` changes: handled for `HitmanPhone` in v0.2.9.
- Appearance save conversion in the beta is one-way — back up saves before switching branches.

`Tools/bootstrap-game-references.ps1` regenerates local decompiles after a game update; a Release build (`dotnet build -c Release -p:S1NoDeploy=true`) is the quickest API-drift detector.

## Updating this page

Run `pwsh Tools/check-version-sync.ps1` after a version bump; it checks code, `mod.json`, `README.md` and `AGENTS.md`. The verification dates above are maintained by hand — update them together with the mod's `CHANGELOG.md` entry.
