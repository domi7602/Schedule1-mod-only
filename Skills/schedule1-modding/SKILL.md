---
name: schedule1-modding
description: "Plan, build and verify Schedule I mods. Keywords: Schedule I, MelonLoader, IL2CPP, Harmony, HarmonyPatch, PhoneApp, S1API, Unity, mod scaffolding, SafeStorage, UITheme."
---

# Schedule I modding

## Scope and prerequisites

Use this entry point for a new mod, a cross-domain change, build/deployment work or a repository-wide review. For a narrow API lookup, load the matching specialty directly.

Before changing code, read the [workspace contract](references/working-contract.md) once for this task. Locate the real repository, its current AGENTS.md, compatibility record and build configuration. Distinguish the skill installation path from the mod repository and the game installation.

## Workflow

1. Establish the requested outcome, mode (review, plan, implement, test or deploy), affected mod and acceptance criteria. Preserve existing unrelated work.
2. Inspect the current implementation and the smallest relevant reference. Read actual helper contracts before using a named Shared service.
3. Resolve relevant APIs against the target assemblies and matching source. A name in a reference is a search lead, not a usable signature. Use the [evidence policy](references/evidence-policy.md).
4. Choose the smallest coherent change. Separate domain rules, runtime integration and presentation where that makes failures easier to contain. For a new mod, target at most 20 hand-maintained production source/project files including the csproj; keep smaller mods smaller. Tests/assets/docs do not count. Do not consolidate an existing mod unless requested.
5. For lifecycle, persistence, financial or network changes, state the relevant transitions and partial-failure behavior before editing. Follow any concrete, already configured review process; do not invent a reviewer or request authorization already given.
6. Implement within scope, retaining observability and restoration paths. Reuse verified project helpers rather than copying a similarly named example.
7. Build with deployment disabled by the actual repository mechanism. Inspect targets first; a plain build may otherwise copy files into the game. Do not invent a property name for disabling it.
8. Run focused tests that distinguish the intended behavior from the reported failure. Treat unavailable gameplay testing as pending, not passed.
9. Deploy only when covered by the current task. Verify the installed artifact by identity/hash, then inspect its actual startup and affected gameplay paths.
10. Report the change, evidence, checks run, remaining limitations and any local manual test that is still needed.

## Route to the specialty

| Task | Load |
|---|---|
| Find an API, source owner or decompile | [Knowledge](../schedule1-knowledge/SKILL.md), then [S1API](../schedule1-s1api/SKILL.md) or [game systems](../schedule1-game-systems/SKILL.md) |
| Save/load, migration, backup or slot switching | [Persistence](../schedule1-persistence/SKILL.md) |
| Purchases, transfers or income | [Economy](../schedule1-economy/SKILL.md) |
| Phone UI, input or app lifecycle | [PhoneApp](../schedule1-phoneapp/SKILL.md) |
| Runtime errors, stalls or regressions | [Troubleshooting](../schedule1-troubleshooting/SKILL.md) |
| Native patching or reflection | [Harmony bootstrap](../schedule1-harmony-bootstrap/SKILL.md), [IL2CPP reflection](../schedule1-il2cpp-reflection/SKILL.md) |
| Live game inspection | [MCP](../schedule1-mcp/SKILL.md) |
| Custom NPCs | [Custom NPCs](../schedule1-custom-npcs/SKILL.md) |
| New items or inventory changes | [Items](../schedule1-items/SKILL.md) |
| World geometry, interiors or materials | [World expansion](../schedule1-world-expansion/SKILL.md), then the relevant domain |
| Multiplayer world edits | [Networked world state](../schedule1-networked-world-state/SKILL.md) |

## Failure handling

- Unknown save identity, unknown payment outcome or uncertain resource ownership must block only the affected mutation, while preserving diagnosis and recoverable state.
- A failed optional observer must not silently disable unrelated gameplay. A failed safety condition must not be reported as a successful feature.
- Preserve foreign state. Do not clear another mod's listeners, destroy borrowed materials, strip networking or overwrite unknown data to make a test pass.
- Keep retries bounded and idempotent. A timeout does not prove an operation had no effect.

## Verification and delivery

Choose applicable checks: baseline build, API/compile contracts, unit-level state transitions, save round-trip, menu/scene/slot transitions, UI reopen/rebuild, host/client behavior and cleanup. Publish observed results with their environment; do not demand irrelevant tests for a text-only change.

## Load when needed

- [Architecture and Shared](references/architecture-and-shared.md): helper contracts and ownership.
- [Pattern selection](references/mod-patterns.md): select a mod architecture.
- [Build and deploy](references/build-and-deploy.md): artifacts, deployment boundaries and rollback.
- [IL2CPP and Harmony](references/il2cpp-harmony-guide.md): wrapper lifetime and native hooks.
- [UI and S1API](references/ui-and-s1api.md): presentation integration.
- [Change planning](references/spec-first-workflow.md): acceptance criteria and concrete review modes.
- [Version synchronization](references/version-sync.md): releases and metadata.
- [Performance diagnosis](references/diag-mods-tick-counter.md): what a counter can and cannot measure.
- [Skill maintenance](references/skill-maintenance.md): update this collection without reintroducing drift.
