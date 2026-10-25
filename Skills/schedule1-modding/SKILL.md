---
name: schedule1-modding
description: "Plan, build and verify Schedule I mods."
---

# Schedule I modding

## Scope and prerequisites

Use for new mods, cross-domain changes, builds/deployment or repository reviews. Narrow lookups can load the matching specialty directly.

Before mutation or compatibility claims, read the [workspace contract](references/working-contract.md) once. It owns environment resolution, AGENTS.md, authorization and the new-mod file budget.

## Workflow

1. Establish the task mode, affected mod, outcome and acceptance criteria; inspect existing code and actual Shared contracts.
2. Select the relevant specialty below. Resolve APIs against target assemblies/matching source under the [evidence policy](references/evidence-policy.md).
3. Make the smallest coherent change, separating domain rules, runtime integration and UI where useful. For lifecycle, persistence, payments or network changes, state transitions and partial-failure behavior using [change planning](references/spec-first-workflow.md).
4. Retain diagnostics and restoration paths. Build through the repository's no-deploy mechanism; run checks that distinguish the fix from the old failure.
5. If deployment is authorized, verify installed identity/hash, startup and affected gameplay. Report actual results and limits; accepted work stays closed.

## Route to the specialty

| Task | Load |
|---|---|
| API/source lookup | [S1API](../schedule1-s1api/SKILL.md) for wrappers; [game systems](../schedule1-game-systems/SKILL.md) for native systems; [Knowledge](../schedule1-knowledge/SKILL.md) when source ownership is unclear |
| Save/load, migration, backup or slot switching | [Persistence](../schedule1-persistence/SKILL.md) |
| Purchases, transfers or income | [Economy](../schedule1-economy/SKILL.md) |
| Phone UI, input or app lifecycle | [PhoneApp](../schedule1-phoneapp/SKILL.md) |
| Runtime errors, stalls or regressions | [Troubleshooting](../schedule1-troubleshooting/SKILL.md) |
| Native patching or reflection | [Harmony bootstrap](../schedule1-harmony-bootstrap/SKILL.md) for patches; [IL2CPP reflection](../schedule1-il2cpp-reflection/SKILL.md) for member access |
| Live game inspection | [MCP](../schedule1-mcp/SKILL.md) |
| Custom NPCs | [Custom NPCs](../schedule1-custom-npcs/SKILL.md) |
| New items or inventory changes | [Items](../schedule1-items/SKILL.md) |
| World geometry | [Grid](../schedule1-grid/SKILL.md) for placement; [Interiors](../schedule1-interiors/SKILL.md) for rooms/travel; [3D assets](../schedule1-3d-assets/SKILL.md) for models/materials; [World expansion](../schedule1-world-expansion/SKILL.md) for staged additions |
| Multiplayer world edits | [Networked world state](../schedule1-networked-world-state/SKILL.md) |

## Failure handling

- Unknown save identity, unknown payment outcome or uncertain resource ownership must block only the affected mutation, while preserving diagnosis and recoverable state.
- A failed optional observer must not silently disable unrelated gameplay. A failed safety condition must not be reported as a successful feature.
- Preserve foreign state. Do not clear another mod's listeners, destroy borrowed materials, strip networking or overwrite unknown data to make a test pass.
- Follow the workspace contract's bounded-retry rule; a timeout does not prove no effect.

## Verification and delivery

Use the chosen specialty's checks for changed behavior. The workspace contract governs scope: documentation needs no gameplay acceptance; multiplayer tests need explicit scope. Unrun checks are never passed results.

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
