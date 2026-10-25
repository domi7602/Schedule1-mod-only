---
name: schedule1-game-systems
description: "Find Schedule I game systems, classes and hook candidates."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Schedule I game-system map

## When to use

Locate game subsystems, inherited research, native owners and hook candidates. Use `schedule1-s1api` for wrapper calls and domain specialists for implementation; never load all 64 references for one question.

## Prerequisites and evidence

The [reference scope](references/README.md) documents the imported 2026-10-08 scan, not reproduced here. Names, constants, formulas and hooks remain lookup candidates. Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.

## Workflow

1. Identify the subsystem and needed answer: class lookup, contract, behavior or implementation.
2. Use the [topic index](references/_index.md) for the matching reference, redirects and coverage; its depth rating is not evidence confidence.
3. Resolve the actual repository, backend and dependencies. Locate the named class in its declaring assembly or matching source; recorded paths are search candidates.
4. Resolve the exact owner, signature, accessibility and units. Check a supported wrapper before adding a native patch; wrappers still require version-specific verification.
5. For behavior or a hook, inspect readable implementation or design a bounded runtime check. Interop stubs do not reveal a native method body. Check host/client authority and pre-/post-mutation timing.
6. Return the source/member, supported conclusion and any missing artifact needed for the next check.

## Hand off to the implementation owner

| Work | Skill |
|---|---|
| Save identity, corrupt input, restore or migration | [Persistence](../schedule1-persistence/SKILL.md) |
| Debit, delivery, stock, compensation | [Economy](../schedule1-economy/SKILL.md) |
| Supported wrapper signature | [S1API](../schedule1-s1api/SKILL.md) |
| Native compatibility and patch feasibility | [Reversing](../schedule1-mod-reversing/SKILL.md), [Harmony bootstrap](../schedule1-harmony-bootstrap/SKILL.md) |
| UI, focus and app lifetime | [Phone apps](../schedule1-phoneapp/SKILL.md) |
| Building placement and scene additions | [Grid](../schedule1-grid/SKILL.md), [world expansion](../schedule1-world-expansion/SKILL.md) |
| Evidence from a connected game | [MCP](../schedule1-mcp/SKILL.md) |

## Completion check

Do not equate “not found” with removal without search scope, a candidate hook with a tested patch, or numeric tables with current balance. Propagate changed members through the [maintenance workflow](../schedule1-modding/references/skill-maintenance.md).
