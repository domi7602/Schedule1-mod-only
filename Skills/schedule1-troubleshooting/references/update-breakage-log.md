# Update Breakage Log — What did each game update break?

> verified: 2026-10-05 — rows compiled from the cited evidence files (evidence dates per row); no new in-game tests were run for this log. Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

Per-update record of **proven** mod breakage, split from expectations. Every "confirmed" row cites an evidence file with a date and a repro/log artifact. "Expected" rows come from `docs/compatibility.md` (Known version-drift risks, 2026-09-19 impact analysis) and are explicitly **not yet confirmed** — treat them as review triggers, not facts.

## Confirmed breakages

| Game/API-Version | Breakage | Evidence-Datei | Severity |
|---|---|---|---|
| v0.4.6f11 | `DialogueHandler.get_activeDialogue()` removed → any mod whose pinned IL body references it fails at patch install (`MissingMethodException` before user code runs) | `references/common-errors.md` §1 (2026-08 era) | high |
| v0.4.7f6+ | `GameLifecycle.OnSaveInfoLoaded` fires **0×** in a full load (instrumented run 2026-09-29: two mods subscribed with first-line logging, 0 firings; event still exists in S1API 3.2.1-beta.8 but is never invoked). "Refresh on OnSaveInfoLoaded" recipes are dead — use `OnPreLoad` / `OnLoadComplete` | `../schedule1-lifecycle-verify/SKILL.md` §7, row 2026-09-29 | high |
| S1API 3.2.1-beta.8 | `OnSaveLoaded` does not exist at all — `GameLifecycle` exposes exactly 6 events (`OnPreLoad`, `OnLoadComplete`, `OnPreSceneChange`, `OnSaveInfoLoaded`, `OnSaveStart`, `OnSaveComplete`); recipes recommending `OnSaveLoaded` are dead code | `../schedule1-lifecycle-verify/SKILL.md` §7, row 2026-10-05 | medium |
| S1API 3.2.0 stable vs game 0.4.7f6 | 3.2.0 stable does not know the 0.4.7f6 renames — deployed runtime must stay on the 3.2.1-beta line | `docs/compatibility.md` (Toolchain table) | medium |
| any (mod-size, empirical 2026-08) | 10.5 MB+ mod class volume → IL2CPPInterop `Class::Init signatures exhausted, using a substitute!` → native AV 0xc0000005 → game dies (DrugExpansion 1.0.0) | `SKILL.md` §8; `references/common-errors.md` §4 | critical |
| any (0.4.6-era, 2026-08-21) | Field-accessor trap: `BaseItemDefinition.get_DefaultStackLimit` field accessor cannot be Harmony-patched (`Il2CppInterop can't be patched`, `Latest.log:17:43:04.438`) → patch silently removed (`Mod.cs:109`), limit resolved via engine scan instead | `../schedule1-items/references/stacklimit-engine.md`; `SKILL.md` §8 | medium |

## Expected (from compatibility.md, not yet confirmed)

| Game/API-Version | Expected breakage | Evidence-Datei | Severity (if real) |
|---|---|---|---|
| v0.4.7 Open Beta | Avatar refactor (`AvatarSettings` → `AvatarObjects` / `Outfit`): may affect `PocketShop` (NPC portraits) and `CustomSkateboard` (visual patches) | `docs/compatibility.md` (Known version-drift risks, 2026-09-19) | medium |
| v0.4.7 Open Beta | NPC ragdoll / `NPCHealth` changes: `HitmanPhone` v0.2.9 already mitigates (`NPCDeathPatch`, verified on 0.4.7f6 beta 2026-09-24); other mods touching NPC death/ragdoll unverified | `docs/compatibility.md`; verification matrix row HitmanPhone | medium |
| v0.4.7 Open Beta | Appearance save conversion is one-way — back up saves before switching branches (data-loss risk, not a code breakage) | `docs/compatibility.md` (Known version-drift risks) | high (data loss) |

## Appendix — Update-Response-Runbook (short form)

Full runbook: **`schedule1-modding` SKILL.md §5 "Update Runbook (after a game patch)"**. Short form:

1. Let the game launch once so MelonLoader regenerates `MelonLoader/Il2CppAssemblies/` (stale proxies prove nothing — lifecycle-verify Pitfall 1).
2. `pwsh Tools/bootstrap-game-references.ps1` — regenerate local decompiles.
3. `dotnet build -c Release -p:S1NoDeploy=true` — quickest API-drift detector.
4. Pre-flight / static analysis — `s1interop doctor` + `s1interop analyze <csproj>`. **Note 2026-10-05: the tool was not found on PATH or in the workspace** (see warning in `SKILL.md` §2); if still missing, build + tests are the documented substitute (`CONTRIBUTING.md`).
5. For each patched class: `ilspycmd -t <Full.Type.Name>` against the regenerated proxies; compare signatures.
6. Check the mod inventory's "verified" dates (`AGENTS.md` §2 / `docs/compatibility.md` verification matrix) — anything older than the new game version is at risk; re-verify before trusting.
7. Wrap risky patches in `PatchGuard` (`S1Mods.Shared`) for graceful degradation.

After the response pass, add a row to the table above — confirmed breakages need an evidence file + date, expected ones stay flagged until proven.
