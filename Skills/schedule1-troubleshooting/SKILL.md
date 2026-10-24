---
name: schedule1-troubleshooting
description: "Diagnose Schedule I crashes, slowdowns and load failures. Keywords: troubleshooting, crash, Latest.log, log triage, 0xc0000005, 0x80131506, WasCollected, Harmony patch silent, no-op, save-load timing, IL2CPP pitfalls, slot_-1, FishNet, SyncVar, breakage log, update resilience, s1interop, ilspycmd."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — Troubleshooting

## Scope and prerequisites

Use this skill to isolate a fault before choosing a repair.
Follow the [working contract](../schedule1-modding/references/working-contract.md).
Identify the installed game build, backend, loader, framework DLLs, affected mod,
network role, save identity, and last known working configuration.
Repository sources and decompiles are optional inputs; establish their availability first.
A historical incident is a hypothesis for this session, not its diagnosis.

## Workflow

1. Write a minimal reproduction with expected and observed behavior.
2. Preserve the current log and relevant configuration before restarting.
3. Capture the earliest relevant failure and enough surrounding context to see its caller.
4. Classify the fault using the table below; keep plausible alternatives open.
5. Change one variable, collect the same evidence again, and compare.
6. Repair the confirmed path and exercise its failure/cleanup behavior.

| Symptom | First evidence | Next check |
|---|---|---|
| Startup crash | Full loader log, process exit, crash report | Dependency identity, initialization order, interop boundary |
| Patch appears installed but does nothing | Exact target and owner metadata | Actual invocation, instance/scene gates, native call path |
| Missing method/member | Declaring type and exact signature | Deployed DLL versus build reference and source commit |
| Empty state after load | World identity and lifecycle trace | Readiness, load result, authority, stale cache generation |
| Phone app fails after reopening | Subscription and visibility lifecycle | Close versus destruction, current root and input ownership |
| Frame-time spike | Repeatable scene and measured duration | Actual call costs, allocations, I/O, scans, GPU work |

## Safe performance isolation

- Preserve the save and installed-mod manifest; use a copy for destructive test scenarios.
- Record resolution, graphics settings, scene, camera, time, and warm-up interval.
- Compare the affected save with a new disposable save. A difference narrows the search;
  it does not prove corruption or justify deleting the original.
- Test a documented dependency-respecting subset. A baseline that retains S1API,
  Shared, or S1MAPI is a framework baseline, not a vanilla-game baseline.
- Restore subsets to isolate a candidate, then confirm by removing and reintroducing it.
  Dependencies, multiple faults, and interactions can invalidate simple binary splitting.
- Measure time inside the actual callback or use a compatible profiler. Counting loaded
  mods once per observer frame cannot measure their callbacks or identify a slow mod.
- Keep instrumentation bounded and compare its own overhead with instrumentation off.

## Failure handling

- Do not infer a specific root cause from an exception code, DLL size, or FPS threshold.
- A clean log proves only that the captured logging paths reported no failure.
- Preserve unreadable save/config input; do not replace it with defaults and save.
- Distinguish scene-owned caches from persistent state. Invalidate the former when their
  scene ends; clear/rebind the latter only through the verified world lifecycle.
- Optional tools such as `s1interop` must be discovered before use. Missing tooling is
  a reported limitation; use available builds, source inspection, and targeted runtime probes.
- A missing optional patch may degrade one feature. A failed invariant-critical patch
  must disable the dependent feature visibly rather than continue with corrupt state.

## Verification and delivery

Verify the original reproduction, same-world reload, relevant scene transitions,
shutdown, and host/client behavior when the repair changes shared state.
Do not deploy or launch the game merely because a diagnostic document contains commands.
Report the confirmed cause, changed behavior, evidence, tests performed, and open checks.
Separate build success, patch installation, callback invocation, and gameplay acceptance.

## Read the relevant reference

| Need | Reference |
|---|---|
| Logs and capture boundaries | [Log triage](references/logscan-and-logs.md) |
| Error families and alternative causes | [Common errors](references/common-errors.md) |
| Load order, empty caches, persistence | [Save/load timing](references/save-load-timing.md) |
| Replication and stale client state | [FishNet diagnosis](references/fishnet-syncvar-diagnosis.md) |
| Historical update incidents | [Breakage notes](references/update-breakage-log.md) |
| Patch outcome accounting | [Harmony bootstrap](../schedule1-harmony-bootstrap/SKILL.md) |
| Exact source lookup | [Knowledge navigation](../schedule1-knowledge/SKILL.md) |
