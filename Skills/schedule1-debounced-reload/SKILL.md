---
name: schedule1-debounced-reload
description: "Debounce config reloads with retries and clean shutdown. Keywords: FileSystemWatcher, debounce, TryConsumeScheduledReload, OnUpdate, main-thread pump, config hot-reload, ImageFolderWatcher, PreferencesFileWatcher."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — Debounced Reload

## Scope and prerequisites

Follow the [working contract](../schedule1-modding/references/working-contract.md).
Identify the watched paths, their owner, valid input schema, reload cost, and scene lifetime.
Reuse a tested workspace watcher if its current contract meets these requirements.
This is a design and validation contract; no drop-in watcher is claimed to be compiled.

## Responsibilities

| Component | Responsibility |
|---|---|
| Watcher callback | Record that input may have changed; never touch Unity objects |
| Scheduler | Coalesce events, retain generations, bound retries, and handle shutdown |
| Loader | Read and validate a candidate without replacing the active state |
| Main-thread publisher | Check generation, publish the candidate, release old owned resources |

File events are notifications, not proof of a complete, readable, valid file.
A delay reduces duplicate work but cannot guarantee that an editor has finished writing.

## Workflow

1. Validate the directory and filters; create a mod-owned directory only when that is
   part of the task. Otherwise report an inactive watcher and define recovery.
2. Register change, create, delete, rename, and error handlers before enabling events.
3. On notification, update a generation and an elapsed-time deadline under one consistent
   synchronization policy. A small lock is acceptable; do not describe it as lock-free.
4. Consume due work through the main-thread pump without holding the scheduler lock
   during file I/O, parsing, or Unity operations.
5. Read and validate a candidate. Keep the last valid configuration or asset batch intact.
6. If a newer event arrives during loading, retain that pending generation; do not let
   completion of an older reload clear it.
7. Publish only if the request still belongs to the current scene/session and generation.
8. Log a bounded success/failure summary and expose the actual watcher/reload state.

## Timing and recovery

- Use an elapsed-time source for deadlines; wall-clock changes must not delay a reload
  indefinitely or trigger it early. Validate negative/overflowing configuration values.
- Treat a locked or temporarily incomplete file as retryable with a bounded backoff.
- Treat persistent validation failure as visible rejected input; do not retry every frame.
- A retry must preserve newer pending work and terminate at the configured limit.
- A continuous stream of edits may postpone trailing-edge debounce. Define a maximum
  latency only when needed and still validate input before accepting it.
- A watcher `Error` can mean lost events or stopped monitoring. Mark the snapshot suspect,
  schedule a bounded rescan, and recreate the watcher if required by the observed error.
- Do not infer full-folder content changes solely from the directory modification time.

## Shutdown and ownership

Set the terminal state under the same synchronization policy used by callbacks.
Ignore late callbacks, cancel pending generations, disable events, detach handlers, and
release the watcher. Report inactive after disposal; a non-null disposed reference is not
an active watcher. Repeated disposal must be harmless.
Main-thread operations remain on the main thread; background work handles bytes and pure
parsing only unless a particular API is explicitly documented as thread safe.

## Verification and delivery

Exercise duplicate events, atomic rename saves, partial writes, locked and invalid input,
continuous edits, an event during loading, watcher error/rescan, and disposal races.
Verify that a failed reload retains the last valid state and does not flood logs or frames.
Measure reload cost and allocation behavior before making performance claims.

## References

- [.NET FileSystemWatcher](https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher): events, filtering, and notification limitations.
- [.NET Error event](https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher.error): lost-event and monitoring-failure handling.
- [.NET Stopwatch](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.stopwatch): elapsed-time measurement; select a target-runtime-supported API.
- [Runtime asset cache](../schedule1-runtime-unity-cache/SKILL.md): candidate publication and resource cleanup.
- [Persistence](../schedule1-persistence/SKILL.md): durable configuration writes and recovery.
