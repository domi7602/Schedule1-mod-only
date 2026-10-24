# Latest.log Triage and Diagnostic Capture

> verified: PowerShell one-liners re-tested 2026-08-03; §4.3 lifecycle advice re-verified 2026-09-29 (OnSaveInfoLoaded = 0 firings in the instrumented session).
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 19 of 19 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

> Evidence scope: this is a diagnostic procedure revised from the uploaded notes.
> Historical PowerShell/lifecycle checks are not proof that this session has run them.

## Capture first

1. Locate the actual game directory and the current loader log; the historical default is
   `MelonLoader/Latest.log` below that directory.
2. Record file identity, timestamp, relevant mod versions, scene, and reproduction time.
3. Copy the log before restarting or launching another session that may replace it.
4. Preserve the complete exception block and neighboring initialization or state lines.
5. Use a bounded tail for immediate triage, then widen only for a concrete missing clue.

Do not delete the only log before preserving it. A new capture should be clearly separated
from the earlier session so timestamps and process identities are not mixed.

## Search efficiently

Use available tools such as `rg` or PowerShell `Select-String` on the preserved file.
Search relevant mod prefixes, `Exception`, error/warning markers, `Harmony`, and collected
object messages; the exact prefix format depends on the logger and build.
Retrieve context around a match. Filtering only matching lines can hide the actual caller.
For large files, distinguish a bounded tail from a full-file search in the report.

A first exception is a useful lead. It can be unrelated or handled, while the actual fault
occurs later; establish its causal connection instead of automatically ignoring later errors.
An absence of matches does not prove successful initialization or healthy gameplay.

## Classify the evidence

| Observed record | What it establishes | What remains open |
|---|---|---|
| Patch installation message | A registration path reported success | Target metadata, invocation, intended effect |
| No callback logs | No matching captured log line | Logging enabled, callback reached, filter coverage |
| Empty list after scene entry | The observed read was empty | Valid empty state, readiness, authority, correct world |
| Native access violation | A native failure occurred | Cause, faulting module, lifetime/thread/ABI conditions |
| Missing-member exception | A particular resolution or call failed | Which binary differs and why |
| Clean recent tail | No detected failure in that bounded tail | Earlier failures or unlogged faults |

## Runtime capture boundaries

Use the bridge only if it is available and its current schema has been discovered.
The inherited S1MCP schema uses `last_n_lines`, `first_n_lines`, and timestamp filters;
see [bridge signatures](../../schedule1-mcp/references/tool-signatures.md).
A source note that mentions a tail cap is not a guarantee of the installed handler's cap.
Record truncation, dropped multiline content, and any failed reads.

Keep a live follow operation bounded or cancellable. Stop it when the reproduction ends;
a long-running tail is not itself a test result.
Avoid logging every frame or every object. Aggregate counts/durations over a defined window
and preserve the first distinct error with a repeat count.

## Follow the relevant diagnosis

- For native crashes, preserve the crash report/dump when available and compare dependencies.
- For lifecycle state, capture scene, world identity, role, callback time, and readiness.
- For silent hooks, inspect owner metadata and instrument the actual hook/caller.
- For performance, pair log evidence with measured frame/callback durations.
- For persistence, retain the failed path, operation, exception, and write-gate state.

See [common errors](common-errors.md), [save/load timing](save-load-timing.md), and
[the troubleshooting workflow](../SKILL.md).

## Deliver the finding

State the capture interval, exact symptom, relevant log lines, confirmed versus possible
cause, one next diagnostic step, and what the capture could not observe.
Do not describe a static identifier search as an in-game log or behavior test.
