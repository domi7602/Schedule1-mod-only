# Targeted Search Recipes

> verified: recipes tested against workspace 2026-10-05 (ilspycmd needs scoped `DOTNET_ROOT="$env:USERPROFILE\.dotnet"` — Pitfall 6 in `../schedule1-lifecycle-verify/SKILL.md`).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 24/26 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

> Evidence scope: these are research procedures, not commands already executed against
> the user's repository. Resolve the actual root, tool version, and target assembly first.

## 1. Find a class or member

Use `rg --files` under the relevant namespace or source tree to locate a candidate file.
Search the full declaring type, then inspect the member declaration and neighboring body.
Record static/instance status, overloads, parameter direction/types, return type, and access.
If only a generated IL2CPP proxy is available, limit behavior conclusions accordingly.

## 2. Inspect a deployed assembly

Use the installed ILSpy CLI's verified type-selection option for a full type name.
For a project decompile, the official CLI combines `-p` with `-o`; nested namespace output
uses the separate `--nested-directories` option. Confirm local help before relying on them.
Capture warnings and dependency-resolution failures as part of the result.
Official reference: [ILSpyCmd](https://github.com/icsharpcode/ILSpy/blob/master/ICSharpCode.ILSpyCmd/README.md).

## 3. Compare source and deployment

Record the source commit and deployed DLL hash/assembly identity. Compare the member in
both; a source checkout with a matching marketing version can still differ from the binary.
Locate the actual framework assembly instead of assuming a particular filename or directory.
Use the [S1API catalog](../../schedule1-s1api/SKILL.md) for target-version context.

## 4. Find an existing implementation

Search `Source/Mods/` for the relevant concept, then read its caller and failure/teardown paths.
Check whether the helper is source-linked, shared at runtime, or private to that mod.
Inspect its contract and tests before reusing it in another world, role, or lifecycle.
If the example moved to `Source/Archive/`, treat it as historical evidence.

## 5. Verify a lifecycle claim

Inspect the actual event declarations, subscription sites, and framework invocation path.
When native game code controls invocation, managed proxy searches may not reveal its order.
Plan a bounded trace with scene/world/role identity and relevant readiness checks.
Use [lifecycle verification](../../schedule1-lifecycle-verify/SKILL.md); never turn an event
name's existence or a zero-hit text search into a universal timing guarantee.

## 6. Interpret a search result

| Result | Next step |
|---|---|
| Exact type/member/signature found | Verify semantics and deployment identity |
| Same name in a different type | Treat as a different candidate |
| No result in generated output | Check scope, completeness, dependencies, and native boundary |
| Capped live search returns zero | Inspect exact known paths or expand bounded coverage |
| Example-only identifier found | Do not count it as a framework/game API |

## 7. Build a feature research note

Record goal, exact unknowns, available sources, confirmed surfaces, unverified assumptions,
existing helper contracts, and the smallest implementation/verification sequence.
Research itself does not require deployment. When implementation is authorized, follow the
[current build contract](../../schedule1-modding/references/build-and-deploy.md) and separate
build-only checks from installation and gameplay acceptance.

## 8. Log diagnosis

Preserve the original session log and retrieve relevant context around a match.
Use [log triage](../../schedule1-troubleshooting/references/logscan-and-logs.md) rather than
assuming the first matching error is necessarily the root cause.
