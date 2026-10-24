---
name: schedule1-mod-reversing
description: "Inspect Schedule I binaries, symbols and compatibility. Keywords: decompile, ilspycmd, reverse engineering, third-party mod, foreign DLL,."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — Third-Party Mod Reversing

## Scope and prerequisites

Follow the [working contract](../schedule1-modding/references/working-contract.md).
Analysis does not authorize installing or executing a supplied DLL.
Use the archive and dependencies the user supplied or otherwise authorized.
Identify available decompiler tooling, backend, game references, and source provenance.
Keep extracted material within the analysis workspace and respect its license and scope.

## Workflow

1. Inventory the archive before extraction; check destination paths, sizes, duplicate
   entries, and required companion assets/libraries.
2. Preserve the original artifact and record its hash, metadata, and assembly references.
3. Decompile to an isolated output directory using a locally verified tool invocation.
4. Map entry points, lifecycle hooks, configuration, persistence, and network/file effects.
5. Inventory fixed and dynamic patch targets; read their gates and affected instances.
6. Inspect the relevant implementation bodies, including error and teardown paths.
7. Compare each required type/member/signature with matched deployed references.
8. Produce a report with concrete scenarios and the tests needed for remaining claims.

## Compatibility evidence levels

| Evidence | Supported statement |
|---|---|
| Name occurs in a decompile | A candidate symbol was found |
| Correct declaring type and full signature match | This static API anchor matches |
| Dependency and bootstrap inspection | No inspected static blocker found, with coverage stated |
| Controlled start and patch readback | Initialization/installation succeeded in that session |
| Reproduced gameplay scenario | The tested behavior worked under the recorded conditions |

Symbol presence alone cannot prove that a mod initializes or that a native hook runs.
A global search for `Update` does not verify `ListingUI.Update`; a class declaration does
not verify one of its properties. Treat missing and unsupported anchors separately.

## Interpretation rules

- `ScheduleOne.*` versus `Il2CppScheduleOne.*` does not by itself identify an S1API facade.
  Inspect the referenced assemblies and backend instead.
- A target framework constrains compatibility; it does not prove networking support.
- A generated/native proxy exposes bindings, not the underlying native algorithm.
- Dynamic patch lookup may throw, skip, or partially succeed; read the actual failure path.
- A shared target is a conflict candidate. Check instance gates, priorities, side effects,
  and state restoration before claiming a conflict.
- Direct RPC implementation calls require authority and replication analysis. A single
  connected player does not prove an offline singleplayer session.
- Security pattern scans are bounded evidence, not a proof that a binary is safe.

## Failure handling

Preserve decompiler warnings and original output; a cleaned reading copy is supplementary.
Report unresolved dependencies, incomplete bodies, and obfuscation as coverage limits.
Do not treat file placement as proof that a process currently loaded the DLL.
Do not copy private protocol constants, hard-coded coordinates, or unverified source into
another mod merely because the inspected binary used them.

## Verification and delivery

Report artifact identity, inspection method, useful architecture, mechanics with evidence,
patch inventory, compatibility findings, file/network effects, and open runtime checks.
Distinguish assembly metadata, loader-declared version, and release label if they disagree.
Record the exact file/member for each high-impact finding; avoid unsupported verdicts.

## Read triggers

| Need | Reference |
|---|---|
| Extraction, decompilation, and precise compatibility workflow | [Runbook](references/reversing-runbook.md) |
| Interpreting risky patterns without overclaiming | [Review signals](references/red-flags.md) |
| Inherited examples from prior analyses | [Case notes](references/case-notes.md) |
| Compare overlapping patches with local mods | [Third-party triage](../schedule1-knowledge/references/third-party-mod-triage.md) |
