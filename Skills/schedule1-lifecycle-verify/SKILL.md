---
name: schedule1-lifecycle-verify
description: "Verify Schedule I lifecycle timing and subscription cleanup. Keywords: lifecycle, verify, ilspycmd, GameLifecycle, OnPreLoad, OnLoadComplete, OnSaveInfoLoaded, event order, UNVERIFIED, verification log, DOTNET_ROOT, LoadManager, SaveManager."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Lifecycle verification

## Scope and prerequisites

Use the [working contract](../schedule1-modding/references/working-contract.md).
Record the game build, runtime branch, S1API assembly identity, and scenario.
Use the actual project/deployed assembly paths; do not assume a particular Steam
location, assembly count, DLL size, or globally configured environment variable.

The package contains inherited observations, not the live DLLs or their logs.
Use `ilspycmd` only after checking the installed tool's help and runtime support.
Do not install or replace the project's toolchain merely to inspect one member.

## Choose evidence for the claim

| Claim | Sufficient starting evidence | Limitation |
|---|---|---|
| Member exists with a signature | Exact target assembly or matching source | Does not prove the callback fires |
| Wrapper subscribes to a native event | Managed wrapper implementation | Does not prove native event order |
| Managed call path order | Readable implementation plus relevant control flow | Only proves the inspected path |
| IL2CPP native event order/readiness | Instrumented runtime traces or verified native implementation | Interop proxy thunks cannot establish it |
| Handler survives reload without duplication | Repeated load/teardown trace with instance identity | One startup trace is insufficient |

## Verification workflow

1. State one testable claim and its required scenario: menu scan, first load,
   same-slot reload, different-slot load, save, quit, host, or client.
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

Return a compact evidence record: claim, environment, observed sequence, affected
implementation, completed checks, and open runtime tests. Do not mark a claim
verified solely because a symbol search succeeded.
