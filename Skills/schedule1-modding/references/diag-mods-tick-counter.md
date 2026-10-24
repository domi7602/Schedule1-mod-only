# Performance counters: measurement contract

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 7 of 7 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Corrected limitation of the old example

The previous example incremented a counter for every registered MelonMod from the diagnostic mod's own OnUpdate. It observed the diagnostic loop and registration list; it did not intercept other mods' updates or measure their duration. Equal counts are expected by construction. Different registration times or multiple Melons sharing an assembly can also affect totals.

Do not use that example to identify a bottleneck, to rule out another OnUpdate, or to conclude that the engine is responsible. The misleading implementation has been removed from the active guide.

## Choose the actual question

| Question | Evidence needed |
|---|---|
| Is the main thread making progress? | Timestamped progress samples from a known observer, with its own limits |
| Which callback consumes time? | Duration measurement around the actual callback or profiler samples attributable to it |
| Is a scan responsible? | Controlled comparison with scan timing/count/size and the feature enabled/disabled |
| Is a particular mod involved? | Repeatable comparison across controlled mod sets and identical scene/save conditions |
| Does a save-specific condition matter? | Preserved original save plus a separate test save and comparable conditions |

A callback counter is not a duration measurement. Low observer frequency does not identify what ran before it.

## Practical investigation

1. Preserve the original save and mod configuration.
2. Record scene, save, loaded artifacts, frame-time symptom and reproduction.
3. Separate menu, load and steady gameplay. Compare one change at a time; use bounded mod-set bisection when dependencies permit.
4. Measure suspected callbacks/scans with low-overhead instrumentation or an available profiler. Track count, total duration, maximum and relevant workload size.
5. Keep instrumentation bounded; avoid per-frame whole-world scans and synchronous log writes.
6. Reproduce the slowdown with instrumentation disabled to estimate observer overhead.
7. Report correlation and causation separately. A framework-only baseline still contains executable framework code.

## Tests and removal

Verify that the instrumented path actually executes and that a known deliberate test delay is attributed to that path in an isolated fixture. Restore original timing/instrumentation afterwards. If no suitable instrumentation is available, report the current hypothesis and next measurement rather than manufacturing per-mod timing from registration counts.

See [troubleshooting](../../schedule1-troubleshooting/SKILL.md) for controlled isolation and [evidence policy](evidence-policy.md) for claim scope.
