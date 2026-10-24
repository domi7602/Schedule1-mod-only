# Common Error Patterns — Evidence and Next Checks

> verified: patterns collected 2026-08-03 → 2026-08-22; §9 lifecycle advice re-verified 2026-09-29 (OnSaveInfoLoaded = 0 firings in the instrumented session); §11 guard idiom unified to the Golden Guard 2026-10-05.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 105/109 identifier-shaped tokens resolve (1 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

> Evidence scope: revised from the uploaded workspace notes. Earlier incident dates and
> project names identify inherited reports, not tests performed for this package.
> The referenced game, framework, and mod sources are not bundled here.

## How to use this decoder

Treat the symptom as a search key. Confirm the caller, relevant assembly identity, and
failure path before selecting a repair. A matching error string is not a unique diagnosis.
See [log triage](logscan-and-logs.md) for preservation and capture boundaries.

## Startup and interop

| Symptom | Candidate causes | Verification and bounded response |
|---|---|---|
| `MissingMethodException` during patch installation | Build/deployment mismatch, changed signature, dependency resolution, removed API in patch code | Inspect the full exception and exact declaring type in both build references and deployed DLLs. Repair the mismatch before choosing another hook. |
| Injected type cannot register | Unsupported member types, missing required constructor, registration order, duplicate registration | Compare the installed Il2CppInterop class-injection contract with the actual class. The historical NotesApp case involved its `IntPtr` constructor; that is not the only possible cause. |
| Private member lookup fails | Wrong declaring type, member became a property, renamed member, wrong flags, missing proxy surface | Resolve the exact member and its type. Do not turn a failed write into success or infer arbitrary-address writes from a missing member alone. |
| Native access violation `0xc0000005` | Invalid lifetime/thread/ABI, unsafe native patch, incompatible dependencies, or unrelated native fault | Preserve crash evidence, inspect recent changes, and isolate a minimal reproduction. DLL size is not a proven universal threshold. |
| CLR fatal error `0x80131506` | Runtime failure with several possible causes | Inspect crash diagnostics and source/deployment identities. Old S1API recursion notes are a lead only when the same call path is demonstrated. |
| Field-accessor hook rejected | Proxy accessor may lack a patchable native method | Confirm the actual installed bridge's target classification; use a verified caller or a supported data path if appropriate. |

Historical DrugExpansion notes associated a class-initialization warning and a large DLL
with a native crash. They do not establish that 5 MB, 10 MB, or a given class count causes
all such crashes, or that splitting a DLL is sufficient to fix one.

## Patching and callbacks

| Symptom | What to establish | Response |
|---|---|---|
| Installed hook never fires | Exact owner/target, native call path, scene, object gates, reproduction coverage | Instrument the hook and its relevant caller with bounded counters. Inlining is one candidate, not the default proof. |
| Several mods patch the same target | Which instances and state each patch changes; priorities and skip-original behavior | Write a concrete conflict scenario and test both together. Patch count alone does not establish conflict. |
| Prefix changes a native object return and crashes | Actual parameter/return ABI and HarmonySupport converter | Do not reinstall the same failing detour blindly. Verify a minimal alternative and preserve restoration. Managed Harmony support does not prove native bridge safety. |
| Error capture is incomplete | Which logger overloads and sinks the actual build uses | Inspect the logger surface and capture paths. Avoid recursive logging inside a logging patch. |
| Repeated event callbacks | Where subscription happens, who owns the delegate, when the instance dies | Make subscription and teardown symmetric and idempotent for the intended lifetime. |

Use [Harmony bootstrap](../../schedule1-harmony-bootstrap/SKILL.md) for outcome accounting.
A caller patch is an alternative only after checking its scope, invocation rate, and semantics;
patching every caller indiscriminately can multiply behavior and cost.

## Unity lifetime and UI

| Symptom | What to establish | Response |
|---|---|---|
| Collected/stale IL2CPP wrapper | Managed reference, wrapper validity, Unity lifetime, scene generation | Use the installed project's verified validity helper and re-resolve at the correct lifecycle boundary. A check does not grant indefinite validity. |
| Works before scene change, fails after it | Ownership of cached singleton and object lists | Invalidate scene-owned references. A singleton label does not prove a native object survives a reload. |
| Button listener fails | Exact delegate type, constructor/adapter, and target lifetime | Prefer a verified S1API listener helper when present; otherwise implement the actual installed delegate contract. |
| Phone app blank on second open | Close versus destruction, update subscription, active root | Keep the reopen mechanism alive; tear down on the actual destruction boundary. Read the current PhoneApp implementation before naming callbacks. |
| Gameplay keys leak through UI | Focus owner, modal state, input map, cursor behavior | Use the project's verified input arbitration. Cursor lock alone is a heuristic, not proof that no blocking UI is open. |
| Wrong sprite after a size/style change | Cache key and dependency ownership | Include all rendering-relevant parameters and lifecycle generation; clean up unused owned resources. |

See [PhoneApp lifecycle](../../schedule1-phoneapp/references/lifecycle-and-canvas.md) and
[runtime cache ownership](../../schedule1-runtime-unity-cache/SKILL.md).

## Save/load and configuration

| Symptom | What to establish | Response |
|---|---|---|
| Empty owned-property/NPC list | Correct world, lifecycle phase, local authority, legitimate empty state | Record readiness and refresh after the relevant verified event; do not use nonzero count as the only readiness signal. |
| Save appears lost after scene exit | Which hook cleared persistent state; whether a later save wrote it | Separate transient cache invalidation from persistent-state reset; bind writes to the active world. |
| Stations multiply after each load | Who owns restoration; persistent GUID acquisition timing | Choose one restoration owner. Match native objects by verified persistent identity before applying sidecar data. |
| JSON parse failure | Actual bytes, encoding, expected schema, truncation or wrong file | Preserve the input and error. Validate backup recovery without overwriting the failed primary from default state. |
| Dictionary/list config disappears | Installed serializer and workspace configuration wrapper | Inspect the actual supported types. Use a separate versioned store when required, not a universal claim that TOML cannot represent collections. |
| Enum conversion on preferences save | Actual generic entry type and serializer behavior | Trace the stored value and type; inherited workspace notes recommend typed entries but the deployed helper must be checked. |
| Data leaks across saves | World binding, save root, fallback paths, migration ownership | Follow the canonical persistence identity contract; a slot number alone may be insufficient. |
| Items disappear during dismantle | Capacity, actual transfer result, mutation ordering | Validate the transfer and only remove the source after confirmed acceptance, with a defined failure path. |

See [save/load timing](save-load-timing.md) and
[persistence](../../schedule1-persistence/SKILL.md).

## Performance and tooling

- Measure callback duration and allocation cost, not how often an observer enumerates mods.
- Separate initial asset/type warm-up from steady-state cost with repeated controlled runs.
- A native stack offset requires matched binary/load-address/symbol or native-analysis
  evidence; opening an unrelated proxy type in ILSpy does not resolve that offset.
- Generic AOT constraints require an actual supported type and API. `GetComponent` needs
  a component type; a struct example cannot validate a general AOT workaround.
- Discover optional tools before using their advertised diagnostics. If unavailable, report
  the limitation and use the checks that can actually run.

## Acceptance evidence

For each repair record the original reproduction, the specific cause confirmed, changed
behavior, tested failure paths, and remaining runtime/host/client coverage gaps.
A clean build or clean log alone does not close an in-game bug.
