---
name: schedule1-lifecycle-verify
description: "Verify Schedule I lifecycle timing and subscription cleanup."
---

> Workspace and runtime rules: [AGENTS.md](../../AGENTS.md).

# Lifecycle verification

## Scope and prerequisites

Before mutation or compatibility claims, read the [working contract](../schedule1-modding/references/working-contract.md) once.
Record build, backend, S1API artifact and scenario. Resolve actual assembly paths;
do not assume a Steam location, assembly count, DLL size or environment override.

The package contains inherited observations, not the live DLLs or their logs.
Use `ilspycmd` only after checking the installed tool's help and runtime support.
Do not install or replace the project's toolchain merely to inspect one member.

## Choose evidence for the claim

Use the shared [evidence policy](../schedule1-modding/references/evidence-policy.md).
Signatures and wrapper subscriptions do not establish callback execution or native event order;
managed control flow proves only its inspected path. Native readiness/order needs runtime traces
or native implementation evidence. Reload claims need repeated load/teardown with instance identity.

## Verification workflow

1. State one claim and relevant scenario: menu scan, first load, reload, slot change,
   save or quit. Host/client scenarios require explicit scope.
2. Locate the exact fully qualified owner. List candidate types when unsure;
   decompile the smallest relevant managed type instead of dumping every assembly.
3. Determine whether the body is managed behavior or an IL2CPP interop thunk.
   Use proxy output for existence/signatures only. An IL view of the same thunk
   does not reveal the native event-raising sequence.
4. Inspect wrapper registration, forwarding, exception handling, and teardown.
   Trace actual subscription callsites, not only helper definitions.
5. Instrument handler entry before its readiness gates. Record a monotonic
   timestamp, event name, world/load generation, owner instance, thread, authority,
   and only the state relevant to the claim.
6. Execute the specific scenario and repeat the load or action where duplicates
   matter. Observe the state the feature needs, not merely the callback name.
7. Record result, exact environment, raw evidence location, and remaining limits.
   Update the canonical reference and remove obsolete active examples together.

## Interpret results conservatively

- No callback in one traced session means not observed in that session. Check
  installation and instrumentation before concluding that an event is unusable.
- An event description or name does not authorize save writes or prove lists are
  ready in every branch, peer, or update.
- A save request accepted is not a completed durable save. Check the target,
  authoritative peer, completion path, and failure result relevant to the design.
- A scene unload may be unrelated to the active world. Filter cleanup by the
  owning world/lifetime rather than clearing all state for every unload event.

## Inherited observations to recheck

The original notes record `Main` scene notification before `OnPreLoad`, followed by
`OnLoadComplete`, in a 2026-09-29 session. They record zero `OnSaveInfoLoaded`
callbacks in that instrumentation. The underlying logs and DLL identities are
not included, so retain these as historical observations only.

Current online [GameLifecycle documentation](https://ifbars.github.io/S1API/api/S1API.Lifecycle.GameLifecycle.html)
was reviewed on 2026-10-21. It describes `OnSaveInfoLoaded` as metadata refresh,
and `OnSaveStart`/`OnSaveComplete` as host/server save events. Match those contracts
to the installed version before relying on them.

## References and output

- Hook selection and resource ownership: [S1API lifecycle](../schedule1-s1api/references/lifecycle.md).
- Existing diagnosis cases: [Save/load timing](../schedule1-troubleshooting/references/save-load-timing.md).
- Data/write boundary: [Persistence](../schedule1-persistence/SKILL.md).

Report the observed sequence and instance lifetime under the shared evidence policy.
