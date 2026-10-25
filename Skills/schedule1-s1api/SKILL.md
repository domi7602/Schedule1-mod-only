---
name: schedule1-s1api
description: "Resolve S1API contracts against the installed version."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# S1API integration

## Scope and prerequisites

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Treat this skill as an API map, not a replacement for the target assembly.
The user's target is S1API 3.2.1 when no newer target is explicitly supplied. <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->
Do not silently substitute current website documentation for that target.

Resolve project/backend, compile reference, deployed assembly and matching source.
Expected external locations: `ThirdParty/S1API/`, `GameReferences/`, `Source/Mods/Shared/`.

## Workflow

1. Define the operation, its authority, affected save state, and required lifecycle.
2. Select one reference below; open only the relevant domain references.
3. Find the fully qualified owning type and member in the exact target source or DLL.
   Check accessibility, overload, parameter meaning, return value, thread, lifetime,
   and whether a wrapper or native type is required.
4. Compare the project's compile reference with the deployed assembly. A correct
   source checkout does not establish what the game actually loads.
5. Implement through a supported wrapper where it meets the requirement. Isolate
   unavoidable native access and document its branch and lifecycle dependency.
6. Compile the actual integration, then verify the behavior in the requested game
   scenarios. Report unavailable checks separately from completed checks.

## Choose a reference

| Need | Read |
|---|---|
| Load/save readiness, subscriptions, cleanup | [Lifecycle](references/lifecycle.md) |
| Saveable fields, serialization, load order | [Saveables](references/saveables.md) |
| PhoneApp base class, icons, discovery | [Phone apps](references/phoneapp.md) |
| Custom or vanilla quests, completion, restore | [Quests](references/quests.md) |
| Physical NPCs, phone contacts, schedules | [Entities](references/entities.md) |
| Custom items, instances, products, registration | [Items and products](references/items-products.md) |
| Money, time, property, law | [Money and game time](references/money-economy.md) |
| Doors, vehicles, stations, weather, messaging | [Other game systems](references/game-systems.md) |
| One-build portability or branch-specific interop | [Cross-runtime compatibility](references/cross-compat.md) |

## API and ownership rules

- Distinguish framework APIs, vanilla APIs, repository helpers, and example-local
  names. A same-named symbol elsewhere is not a verified contract.
- Preserve stable domain IDs; reacquire scene-owned wrappers after a world reload.
- Keep native runtime types out of shared interfaces when portability is required.
  Private direct references still create a runtime dependency.
- Subscribe once per owning lifetime and remove the exact delegate at teardown.
  Unsubscribing a new instance's method does not remove an old instance's handler.
- Check authority before mutations. Client-visible state is not server authority.
- Separate request acceptance, observed mutation, and durable completion.
  A void return or exception cannot establish all three.

## Failure handling

If the required member is absent, verify the owner and version first. Use an
existing supported alternative only after checking its behavior; do not invent
similarly named methods or assume an undocumented overload exists.
If a feature lacks its dependency, expose a bounded unavailable state and keep
unrelated features usable. Retain diagnostic cause without per-frame log spam.
When native effects are uncertain, stop automatic retries and reconcile state
before compensating or issuing the same mutation again.

## Verification and handoff

Select relevant reload, teardown and repeated-action checks; host/client tests require explicit scope.
Report under the shared evidence policy. Match online API documentation to the target assembly.

Official entry points: [API documentation](https://ifbars.github.io/S1API/) and
[release history](https://github.com/ifBars/S1API/releases).
