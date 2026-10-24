---
name: schedule1-knowledge
description: "Find and maintain Schedule I knowledge and evidence. Keywords: Decompiles, Assembly-CSharp, Il2CppScheduleOne, GameReferences, Game Systems, S1API, S1MAPI, ilspycmd, research."
---

# Schedule I — Knowledge Navigation

## Scope and prerequisites

Follow the [working contract](../schedule1-modding/references/working-contract.md).
Establish the actual repository root and available files before using historical paths.
The reference directories below are expected workspace locations, not bundled sources.
Do not treat a curated note or a symbol sweep as current-runtime verification.

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

1. Write the exact unknown: existence, signature, ownership, ordering, or behavior.
2. Locate the relevant index entry, then open only the matching reference.
3. Locate the named source/decompile and record assembly/source identity.
4. Search within the declaring type and inspect parameter/return types and callers.
5. Compare the source checkout with the deployed binary before relying on implementation.
6. When proxies hide the answer, state the limitation and design a bounded runtime probe.
7. Record the result and evidence level; update contradictory examples together.

## Typical workspace map

| Location | Purpose and limit |
|---|---|
| `GameReferences/decompiled/Assembly-CSharp/` | Generated game/proxy surface; provenance must match the binary |
| `ThirdParty/S1API/` | Framework source; not proof of the deployed DLL's implementation |
| `ThirdParty/S1MAPI/` | Mapping/building source if available |
| `ThirdParty/S1MCPServer-master/` | Bridge source; compare with the running bridge |
| `Source/Mods/` | Current local implementations and tests |
| `Source/Archive/` | Historical patterns, not automatically supported examples |
| `AGENTS.md` and `docs/compatibility.md` | Workspace-specific environment record if supplied |

## Search and interpretation rules

- Prefer `rg`/`rg --files` with a narrow path, then inspect matching bodies.
- Inspect actual tool help and backend before assuming a decompiler option or shell path.
- A renamed generated proxy namespace is not proof of changed gameplay semantics.
- A zero-result search can reflect an incomplete decompile, scan cap, or wrong source.
- Do not promote identifiers in examples to verified APIs because they appear elsewhere.
- IL2CPP proxy method bodies cannot generally establish native event invocation order.
- Missing evidence remains unknown; do useful independent work and report the missing input.

## Verification and delivery

Return the exact type/member, source or assembly identity, evidence scope, and the next
smallest check needed. Cite local file/member or official documentation for material claims.
Separate proposed implementation from inspected behavior and tested acceptance.
Do not start a build, deploy, launch, or mutation solely as part of source lookup.

## Read triggers

- [Inventory](references/inventory.md): locate framework, game, and workspace evidence.
- [Search recipes](references/search-recipes.md): targeted signature and lifecycle lookup.
- [Third-party triage](references/third-party-mod-triage.md): compare another mod's gates and effects.
- [Third-party patterns](references/third-party-mod-patterns.md): inherited architectural observations, not ready-made APIs.
