---
name: schedule1-knowledge
description: "Find and maintain Schedule I knowledge and evidence."
---

# Schedule I — Knowledge Navigation

## Scope and prerequisites

Follow [AGENTS.md](../../AGENTS.md); read the [working contract](../schedule1-modding/references/working-contract.md) once before mutation or compatibility claims. Read-only lookup can use the relevant source directly.

## Choose the evidence needed

| Question | Start with | Verify against |
|---|---|---|
| Which subsystem owns a feature? | [Systems index](../schedule1-game-systems/references/_index.md) | Matching source/type |
| Does an S1API method exist? | [S1API catalog](../schedule1-s1api/SKILL.md) | Exact target-version DLL/source |
| Does S1MAPI provide a building/model helper? | [S1MAPI catalog](../schedule1-s1mapi/SKILL.md) | Deployed dependency |
| How does a local mod implement it? | Its source and tests | Current commit and runtime evidence |
| What does native code do? | Available native/Mono evidence | Instrumented behavior or matched implementation |
| Why does a loaded mod fail? | Log and reproduction | [Troubleshooting](../schedule1-troubleshooting/SKILL.md) |
| What does a foreign DLL change? | Package and patch inventory | [Reversing](../schedule1-mod-reversing/SKILL.md) |

## Research workflow

1. Define the unknown: existence, signature, ownership, ordering or behavior. Select one relevant source/reference.
2. Record source/assembly identity; inspect the declaring type, parameters, return type and callers. Match source to deployed bytes before relying on implementation.
3. If proxies hide behavior, state the limit and design a bounded probe. Record supported conclusions and update contradictory examples together.

## Typical workspace map

If the source location is unknown, use the [workspace inventory](references/inventory.md#locate-before-reading). Known paths can be inspected directly; the inventory records external workspace candidates, not bundled or currently verified artifacts.

## Search and interpretation rules

- Prefer `rg`/`rg --files` with a narrow path, then inspect matching bodies.
- Inspect actual tool help and backend before assuming a decompiler option or shell path.
- A renamed generated proxy namespace is not proof of changed gameplay semantics.
- A zero-result search can reflect an incomplete decompile, scan cap, or wrong source.
- Do not promote identifiers in examples to verified APIs because they appear elsewhere.
- IL2CPP proxy method bodies cannot generally establish native event invocation order.
- Missing evidence remains unknown; do useful independent work and report the missing input.

## Verification and delivery

Return the exact member/file, artifact identity and supported conclusion under the
[evidence policy](../schedule1-modding/references/evidence-policy.md).
Source lookup alone does not call for a build, deploy, launch or mutation.

## Read triggers

- [Inventory](references/inventory.md): locate framework, game, and workspace evidence.
- [Search recipes](references/search-recipes.md): targeted signature and lifecycle lookup.
- [Third-party triage](references/third-party-mod-triage.md): compare another mod's gates and effects.
- [Third-party patterns](references/third-party-mod-patterns.md): inherited architectural observations, not ready-made APIs.
