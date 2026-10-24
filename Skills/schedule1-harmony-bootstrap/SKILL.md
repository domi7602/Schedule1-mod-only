---
name: schedule1-harmony-bootstrap
description: "Resolve and install Harmony patches with clear outcomes. Keywords: PatchClassProcessor, HarmonyBootstrap, PatchTargetGuard, IsAllowed, applied/skipped/failed counter, auto-discovery, LogHealth, UnpatchSelf, HarmonyId."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — Harmony Bootstrap

## Scope and prerequisites

Follow the [working contract](../schedule1-modding/references/working-contract.md).
Inspect the installed Harmony/HarmonyX and MelonLoader integration before choosing APIs.
Check whether the loader already patches this assembly; choose one owner for installation.
Use the existing workspace patch helper if its actual contract meets the requirements.
This document is an implementation contract, not a compiled drop-in implementation.

## Select the simplest suitable approach

| Situation | Approach |
|---|---|
| A small independent patch set | Explicit registration with visible results |
| Many independent annotated classes | Per-class processing with bounded discovery |
| Patches depend on each other | Explicit feature groups and dependency order |
| Native IL2CPP ABI is uncertain | Verify the bridge and target before installing |

Automatic discovery does not make incompatible patches safe. Alphabetical ordering is
useful for deterministic logs, but it is not a dependency policy.

## Bootstrap workflow

1. Inventory patch classes, intended original methods, feature ownership, and required
   versus optional status. Resolve overloads by full signature.
2. Choose a stable mod-specific Harmony owner ID. Record pre-existing patches for that
   owner so a repeated initialization does not silently duplicate work.
3. Discover only the intended assembly. Handle type-load failures explicitly, recording
   loader exceptions and whether discovery was partial.
4. Resolve targets through the installed Harmony contract. A target method is method
   metadata; the `HarmonyTargetMethod` attribute is not a target-method array.
5. Let Harmony invoke its auxiliary methods. Do not manually invoke an arbitrary
   zero-argument `Prepare` before asking Harmony to invoke it again.
6. Apply each independent class or feature group under its own failure boundary.
7. Inspect both the processing result and original-method patch metadata for this owner.
8. Disable a dependent feature if a required target is missing or installation failed.
9. Publish a startup report, then verify invocation and gameplay in a controlled session.

## Outcome model

Keep separate counts for classes discovered, intentionally skipped, failed, and incomplete;
record original methods with confirmed owner registrations separately from class counts.
A processor returning without an exception is not sufficient evidence of installation.
One class can patch several original methods, and a hook can install without ever running.
Retain target identities, skip reasons, and failure details after rollback or teardown.

## Native call and restoration boundaries

- Check the actual IL2CPP converter for `ref`, `out`, and native object-return paths.
  Omitting a prefix parameter does not prove the converter avoids the original argument.
- Constrain any per-call override by object identity, authority, and lifecycle generation.
- Capture the exact prior state and restore it on normal and exceptional exits.
- Diagnostic read failure must not accidentally authorize an override.
- If restoration cannot be established, stop the affected feature and report the failure.
- Inspect the repository's TaxiDriver traffic-light evidence when that exact mechanism
  is relevant; a static native finding is not a gameplay acceptance test.

## Teardown and retry

Remove only this mod's registrations using the installed version's owner-scoped API.
Read back remaining registrations. Preserve a failed-removal state instead of marking the
bootstrap clean; never remove other owners' patches to make a counter reach zero.
Retry only from an explicit known state. Failed discovery must not leave an initialization
flag that permanently prevents recovery, and partial installation requires reconciliation.

## Verification and delivery

Test missing targets, an intentional skip, a failing class, partial discovery, repeated
initialization, a required-feature failure, and owner-scoped removal.
Confirm counts against actual target metadata, then instrument a representative invocation.
Report what compiled, what installed, what ran, and what gameplay behavior was observed.

## References and evidence

Official Harmony 2 documentation was consulted for the auxiliary-method and result model;
match it to the deployed fork/version before implementation. No game build was run here.

- [Auxiliary methods](https://harmony.pardeike.net/v2/articles/patching-auxiliary.html): `Prepare`, `TargetMethod`, and `TargetMethods` contracts.
- [PatchClassProcessor](https://harmony.pardeike.net/v2/api/HarmonyLib.PatchClassProcessor.html): processing return value.
- [Harmony API](https://harmony.pardeike.net/v2/api/HarmonyLib.Harmony.html): owner metadata and patch queries.
- [IL2CPP reflection](../schedule1-il2cpp-reflection/SKILL.md): runtime member resolution.
- [Troubleshooting](../schedule1-troubleshooting/SKILL.md): silent hooks and crash evidence.
