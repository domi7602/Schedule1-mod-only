# IL2CPP objects and Harmony integration

> **Canonical home of the WasCollected/Golden-Guard idiom (§2) and parameter-keyed sprite caches (§4)** — other files reference here. verified: 2026-08-20/21; re-checked 2026-10-05.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 35/37 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Preconditions

Identify the actual backend and target artifact. A managed proxy method can expose a native entry point without exposing native implementation logic. Confirm exact overloads, patchability and bridge behavior before installing a detour.

## Object lifetime

Separate managed reference existence, IL2CPP wrapper validity and Unity engine object lifetime. Their checks are not interchangeable. In particular, do not assert that every non-null wrapper proves a live native object, or that an instance ID alone proves lifetime.

Use the checks supported by the actual declared type and interop version. Inspect Pointer/WasCollected behavior before reading those members in a guard; they are not available on every object type. Where Unity equality operators apply, distinguish their semantics from System.Object/reference comparisons. Avoid retaining scene objects across unload; retain stable domain identities and resolve fresh runtime objects at the appropriate lifecycle point.

## Native patch contract

1. Identify the exact target type, method, overload and backend.
2. Inspect how the current Harmony/interop bridge marshals arguments, including ref/out arguments even if a prefix omits them.
3. Define instance, authority and lifecycle gates; keep unrelated calls unchanged.
4. Choose a prefix, postfix or finalizer for the actual effect. A postfix normally observes an already executed original; it cannot retroactively veto it.
5. Publish restoration state before temporary mutation. Restore exactly the prior owned/borrowed value on all supported exits, including exceptions.
6. Confirm installation and exercise the actual hook. An “applied” counter is not proof that it executed.
7. Remove only this mod's patches and event bindings at final teardown.

Use [Harmony bootstrap](../../schedule1-harmony-bootstrap/SKILL.md) for discovery/results and [reflection](../../schedule1-il2cpp-reflection/SKILL.md) for missing or moving bindings.

## Hot paths

Cache appropriate lookups with an invalidation owner and scene/session generation. A static set of instance IDs without cleanup is unsafe when identities are reused. Bound scans by measured workload and time; avoid per-frame global enumeration. For IL2CPP collections, inspect marshalling/allocation behavior and prefer indexed traversal where appropriate. Do not label all managed foreach loops as inherently unsafe.

## Resource caches

Key generated assets by all parameters affecting their result. Define ownership, bounded growth, eviction and consumer replacement. Use culture-independent representations for numeric keys where strings are used. A correct dictionary key does not by itself provide cleanup or lifetime safety.

## Historical field-accessor issue

The uploaded notes reported an unpatchable generated stack-limit accessor. Treat that report as a reason to inspect the installed accessor and call path; do not assume every getter is patchable or every getter is a direct field accessor.

## Verification

Test valid target, absent target, changed overload, exception path, repeated initialization, unload/reload and unrelated instances. Record actual bridge/version evidence for native ABI-sensitive changes. Do not claim that a managed try/catch can recover every native access violation.
