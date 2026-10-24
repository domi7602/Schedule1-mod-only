# S1API cross-runtime compatibility

> verified: loader mechanism checked against S1APIsource 2026-10-05; §6 runtime check corrected 2026-10-05 (was an OS check that proved nothing).
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 32 of 32 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Source scope: the official repository and release history were reviewed on
2026-10-21. Installed assemblies and cross-runtime execution were not tested by
this package. Read [S1API's repository](https://github.com/ifBars/S1API) for the
current distribution contract and match it to the selected release.

## State the supported target

Distinguish the IL2CPP game branch from the Mono branch and identify the actual
runtime used by the project. Do not infer it from Windows versus Linux, player
share percentages, folder names, or whether one optional assembly happens to be
loaded early during startup.

Decide whether the product promises one runtime-neutral build, separate builds,
or one intentionally supported branch. Do not silently broaden that promise just
because the framework supports multiple runtimes.

## Dependency boundary

A public API expressed in wrapper/.NET types is useful but not sufficient for
portability. A private direct reference to an IL2CPP-only assembly still creates a
runtime dependency and can fail before a runtime branch check executes.

Inspect the whole dependency graph: native types in fields, attributes, generic
arguments, method signatures, static initialization, transitive references, and
delegate conversions. Keep direct native integration in separate verified branch
adapters where required. Conditional compilation produces different builds; it
does not by itself create a single universal DLL.

Use the package/reference scheme documented by the target S1API release and the
actual project. Do not adopt a package ID, loader DLL filename, output path, or
configuration such as `MonoRelease` merely because an old example used it.

## Runtime detection and optional dependencies

Use the loader/framework's supported runtime identity when available and verify
when it becomes reliable. An assembly-name probe can be a diagnostic signal but
is timing-dependent; failure to find an assembly is not proof of the other runtime.
Keep optional native code from being eagerly loaded before its dependency is
established. Test the unavailable path as well as the supported path.

## Branch matrix

| Boundary | Verify on each promised runtime |
|---|---|
| Compile references | Correct framework, native/Unity bindings, target framework |
| Loader and distribution | Correct dependency resolution and actual loaded versions |
| Events | Member kind, delegate conversion, exact unsubscribe identity |
| Native access | Equivalent owner/member semantics, not just similar names |
| Save/load | Same intended data behavior and migration compatibility |
| UI/assets | Required shaders, assets, input, and resource lifetime |

Build with the project's real configurations. A successful compile on one branch
is not evidence for the other. Avoid changing the live Steam branch solely to
validate documentation; use the user's established test environment and authorized
workflow.

## Verification and release statement

Test startup, first load, repeated reload, core interactions, and shutdown on each
promised runtime. Record concrete build artifacts and test environments. State
unsupported or untested branches honestly instead of claiming automatic portability.

For a branch-specific implementation, make that restriction explicit in project
metadata and user-facing compatibility information. For a shared implementation,
ensure no native-only dependency leaks across its adapter boundary.
