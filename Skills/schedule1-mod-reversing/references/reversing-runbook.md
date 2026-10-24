# Reversing Runbook — Reproducible Static Analysis

> verified 2026-10-20 — procedure executed end-to-end on three third-party DLLs (see `case-notes.md` for the per-mod evidence). Decompile, hook inventory and compatibility checks are confirmed; no in-game behaviour was tested.

> Evidence scope: workflow revised from the uploaded notes. No foreign DLL was executed,
> installed, compiled, or retested for this package. Paths and prior artifact names are
> examples to resolve in the actual environment.

## 1. Preserve and inventory

Record the supplied archive's name, size, SHA-256, and origin. List its entries first.
Check path traversal, absolute paths, links, duplicate destination names, and unreasonable
expanded size before extracting into an isolated directory.
Preserve companion DLLs, model/texture folders, and embedded-resource information.
A one-file archive can still depend on frameworks, configuration, or external assets.
A large DLL can contain resources; byte size does not establish code complexity or safety.

## 2. Establish tooling and paths

Locate `ilspycmd` and inspect its local version/help. Resolve paths for the shell and the
actual executable; convert MSYS-style paths where required by the observed environment.
Use a simple staged filename only if a concrete path/tool issue requires it, while keeping
original identity and hash. Unicode and spaces are not universally unsupported.
Do not install or upgrade global tools as an unexamined side effect of reading a DLL.

## 3. Decompile with evidence retained

The official CLI documents `-p` for project output, `-o` for its destination, and `-t` for
a specific fully qualified type. `--nested-directories` changes namespace layout; it does
not replace `-p`. Check these options in the installed tool before use.
Capture command/version, exit status, warnings, unresolved dependencies, and output count.
Use matching reference paths when available. Preserve raw output; any cleaned reading copy
must retain a traceable link to it and must not hide unresolved-decompilation warnings.

Official CLI source: [ILSpyCmd documentation](https://github.com/icsharpcode/ILSpy/blob/master/ICSharpCode.ILSpyCmd/README.md).
A generated project is a navigation artifact, not evidence that it builds or is licensed
for redistribution. Do not automatically build, load, or install it.

## 4. Map behavior before reading bulk output

Inspect loader metadata and entry points first, then configuration, domain rules, lifecycle,
persistence, network/file effects, and the methods that implement the requested feature.
Use `rg` to inventory patch attributes, explicit registration, dynamic target discovery,
subscription sites, disk writes, external requests, and native calls.
Read implementation bodies for every high-impact claim; names and UI strings are leads.
Skip unrelated geometry/UI bulk initially, but include it when it affects the user's question.

The reference set and target framework help identify backend/dependency expectations.
They do not establish singleplayer support, current compatibility, or successful loading.

## 5. Build an exact hook inventory

For each hook record declaring assembly/type, original member, parameter and return types,
patch kind, owner, installation gate, instance gate, authority gate, and failure behavior.
Include dynamically selected targets and helper calls, not only `HarmonyPatch` attributes.
Record whether the hook can skip original behavior, replace outputs, or retain modified state.
A patch on a class also reaches compatible cloned instances unless its own gate excludes them.

## 6. Compatibility checks

Compare against references with recorded provenance for the intended game/backend.
Search inside the exact declaring type. A class-name hit cannot verify a property, and a
method-name hit in another type cannot verify the original target.
Check dependencies, overloads, generic constraints, native patchability, and initialization
conditions separately. Distinguish missing, ambiguous, unavailable-to-inspect, and matched.

| Check result | Report |
|---|---|
| Names found | Candidate symbols located |
| Full signatures matched | Inspected static anchors match these reference identities |
| No static blocker found | No blocker in the stated inspection scope; initialization untested |
| Actual controlled initialization passed | Started under the recorded dependency set |
| Gameplay scenario passed | Only the recorded behavior/role/scenario is accepted |

Never turn all-green symbol searches into a claim that the mod will initialize.
A generated RPC hash or private field is a drift risk, not proof it breaks at the next build.

## 7. Footprint and risk review

Inspect actual destination, data, trigger, timeout, and failure paths for network/file/native
operations. Pattern scanning cannot prove the absence of hidden behavior.
If the task includes installed-state review, identify the exact deployed file and, where
possible, the loaded process/module. A DLL in `Mods` is not proof it is currently running.
Keep install/uninstall and save migration separate from read-only analysis.

## 8. Report and retain

Deliver artifact identity, analysis scope, mechanics with file/member evidence, exact hook
inventory, compatibility limits, concrete risk scenarios, and useful design lessons.
Record assembly version, loader-declared version, file version, and release label separately
when they differ; none should silently overwrite the others as the universal release truth.
Keep necessary analysis artifacts in the user-approved durable destination.

Read [review signals](red-flags.md) for interpretation and [case notes](case-notes.md) for
inherited examples. Do not claim runtime coverage for those inherited cases.
