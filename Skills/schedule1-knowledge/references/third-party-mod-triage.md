# Third-Party Mod Triage and Compatibility Comparison

> UNVERIFIED against the installed runtime. Static method note: it records the order of work that
> proved reliable, not runtime behaviour. Proven on two downloaded mods inspected 2026-10-19 (see
> `third-party-mod-patterns.md` for the findings). Re-verify every identifier against the current
> game before relying on it.

> Evidence scope: adapted from inherited October 2026 static-review notes. The cited
> foreign binaries are not bundled, and no current installation or gameplay was tested.

## Scope

Use this reference to compare a third-party mod with a workspace mod or understand a supplied
archive without installing it. Use the [reversing runbook](../../schedule1-mod-reversing/references/reversing-runbook.md)
for package handling, artifact identity, and decompilation; it is the single procedural source.

## Triage order

1. Inventory the package and dependencies.
2. Identify assembly/loader metadata and backend expectations.
3. Map entry points, domain rules, persistence, and patch targets.
4. Read targeted implementation bodies and failure paths.
5. Inspect file/network/native effects and unresolved decompilation warnings.
6. Compare exact API anchors and relevance gates with local mods.

Names, UI strings, and type counts narrow the reading scope; they do not replace the body
when explaining mechanics. Keep raw decompiler warnings visible even if a second reading
copy removes annotation noise.

## Compatibility comparison

| Question | Evidence to collect |
|---|---|
| Do both mods reach the same original? | Declaring type plus exact signature and backend |
| Do they affect the same instances? | Item IDs, ownership, feature flags, tags, and clone behavior |
| Does either skip/replace original behavior? | Prefix return path, result changes, and priorities |
| Do they share mutable state? | Fields, registries, property blocks, input, time scale, resource ownership |
| Can failure leave state changed? | Snapshot, restoration, teardown, and exception paths |
| Is the behavior role dependent? | Host/client gates, request validation, replication ownership |
| Is initialization repeatable? | Duplicate registration, scene reload, late join, and cleanup |

Write a concrete test scenario for each credible collision. An overlapping class name is
only a candidate; a read-only consumer can still cause cost or lifecycle problems, and an
additive patch can still change an invariant.

## External-effect review

Inspect HTTP/socket destinations, process launches, registry access, writes/deletes, native
imports, and their data/validation/error paths. Explain the observed effect and scope.
Pattern scanning cannot prove absence of hidden effects. P/Invoke into a familiar system
DLL is not automatically benign; a user-provided URL still needs bounded input and timeouts.

## Package-specific pitfalls

- Preserve `UserLibs` and asset-directory requirements when describing installation;
  do not install as a side effect of analysis.
- Inspect embedded resources and disk fallback paths before declaring a package incomplete.
- Resolve shell/native paths based on the actual environment, not a fixed Windows username.
- Report assembly metadata, loader-declared version, and file version separately when different.
- Identify a deployed artifact before claiming it is installed; identify a loaded module
  before claiming it is running.

## Reporting

Return what was inspected, what the relevant bodies establish, static compatibility findings,
concrete risk scenarios, and what requires a controlled game session.
Keep new code or third-party modifications out of a read-only analysis unless requested.
See [pattern notes](third-party-mod-patterns.md) for inherited design ideas to verify anew.
