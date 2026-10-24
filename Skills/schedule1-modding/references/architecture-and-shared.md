# Architecture and project helpers

> verified: API rules 2026-08-20/21 + 2026-09-11 audit; §1/§5 deduplicated to canonical homes 2026-10-05.
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 44/45 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Scope

The supplied workspace uses a Shared library and named helpers such as SafeStorage, ModConfig, UITheme, PatchGuard and HotkeyManager. Their current implementations are not part of this package. Treat names as lookup targets, not guaranteed signatures.

## Inspect before reuse

For each helper determine its actual owner, accessibility, arguments/results, threading rules, mutation behavior and cleanup contract. Check its project reference and deployed dependency. Do not add a new helper with a similar name to avoid understanding the existing one.

## Persistence

Use the existing atomic writer when its actual contract meets the requirement. Confirm whether failure is returned or thrown, how backups are rotated, which reads perform deserialization, and whether schema validation is separate. “Atomic file replacement” does not make multiple files or native save plus sidecar one transaction.

Read [persistence](../../schedule1-persistence/SKILL.md) and its relevant reference. Bind destination and snapshot before asynchronous work or state clearing. Keep failed and unsupported loads distinguishable from new data.

## Configuration

Separate global preferences from world/player state. Validate configuration and retain last-known-good values after a failed reload. The inherited collection reported mapping failures for some ModConfig collection properties; test the current serializer before choosing scalar TOML versus a JSON sidecar. TOML as a format is not universally unable to represent collections.

Give each field one persistence owner. Avoid serializing the same scalar in two stores or applying a stale sidecar over a deliberate user change. Confirm an empty collection is a valid user choice rather than ignoring it.

## Input and UI

Route input focus through the actual project's coordinator. A single component must not unconditionally clear a shared typing flag owned by another active input. Tie event handlers to the concrete UI instance lifetime and include final disposal. Removing the new instance's handler does not remove an old instance's handler.

Read [PhoneApp](../../schedule1-phoneapp/SKILL.md) for creation, rebuild, close/reopen and final teardown.

## Domain and integration boundaries

Keep pricing, identity and state transitions independently understandable; wrap game calls at a small boundary whose outcomes can be observed. A thin boundary is not permission to invent success semantics. Keep Unity object access on its required thread and isolate only operations that are truly thread-safe.

## Economy and network

Use [economy](../../schedule1-economy/SKILL.md) for confirmed effects and compensation. Distinguish local player's resources from host-authoritative shared world state. A transaction spans its real participants; a try/catch is not an atomicity guarantee.

## Acceptance

A reader can identify every persistent field's owner, every runtime object's lifetime, each external call's failure semantics and the source evidence for reused helpers. Missing contracts remain explicit rather than hidden behind helper names.
