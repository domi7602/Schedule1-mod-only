---
name: schedule1-runtime-unity-cache
description: "Cache Unity resources with ownership and staged replacement. Keywords: Object.Destroy, SpriteCache, dirty flag, MarkDirty, EnsureLoaded, IDisposable, Texture2D cache, AudioClip pool, GC.Alloc-free reload."
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — Runtime Unity Asset Cache

## Scope and prerequisites

Follow the [working contract](../schedule1-modding/references/working-contract.md).
Identify the cache owner, consumers, scene/session lifetime, and each resource's origin.
Distinguish owned runtime creations from borrowed game assets and shared dependencies.
This is an ownership contract, not a generic compiled cache implementation.

## Required ownership model

Track a batch's assets and dependencies together: for example, sprites plus textures,
materials plus owned textures, or audio clips plus active consumers.
Record which resources may be destroyed, which are shared, and who detaches consumers.
A managed reference disappearing does not itself express a Unity asset-release policy.
Never destroy a borrowed game material or texture merely because a cache used it.

## Reload workflow

1. Mark the source generation dirty and capture the current owner/session identity.
2. Read bytes and perform pure parsing off-thread when useful and supported.
3. Create Unity objects on the main thread, registering each newly owned object in a
   candidate batch immediately so partial construction can be cleaned up.
4. Validate the complete candidate, including required assets and format/size limits.
5. Recheck generation and session before publication; discard stale candidates safely.
6. Rebind consumers to the candidate and publish the new batch as one logical operation.
7. Detach consumers from the previous batch, then release its owned resources in
   dependency-safe order, after its last consumer has stopped using them.
8. On failure, release the candidate and retain or restore the last valid active batch.

Building first can temporarily increase peak memory. If the budget cannot support that,
choose an explicit staged/loading-state design and document the resulting availability
trade-off. Destroying the old batch first is not a universal leak-prevention rule.

## Failure and concurrency rules

- A loader failure must not silently replace working assets with an empty array.
- Do not retry every frame on an unchanged broken file; use bounded retry or a new event.
- An event arriving during a reload remains pending after the older request completes.
- A thread-safe reference assignment is not a transaction across consumers and resources.
- If publication partially fails, restore prior consumer references where valid and report
  incomplete restoration. Do not dispose a batch that still has known consumers.
- Keep byte reading separate from Unity creation/destruction; do not move all loading
  to a worker thread as a performance fix.
- Stop or detach audio consumers before releasing their owned clips; await completion
  through the supported runtime mechanism instead of blocking the main thread.

## Lifetime and cleanup

Use runtime-appropriate Unity destruction for owned assets; it is deferred, so measure
memory after destruction has had an opportunity to complete.
Do not introduce a blanket `DestroyImmediate` exception during application shutdown.
Invalidate scene-bound consumers on scene exit and reject late asynchronous completion.
Make cleanup idempotent and preserve useful diagnostics if native release fails.
If a public clear operation leaves the cache reusable, it must invalidate its loaded key
and dirty/generation state; disposal instead marks the cache terminal.

## Verification and delivery

Verify initial load, repeated reloads, partial creation failure, invalid input, shared
texture ownership, multiple consumers, scene exit during load, and repeated cleanup.
Measure peak and retained native/GPU resources over repeated cycles after settling;
managed GC statistics alone do not establish that Unity resources were released.
Report the ownership graph, last-good behavior, main-thread boundary, and measured limits.

## References

- [Unity Object.Destroy](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Object.Destroy.html): deferred destruction semantics; match the installed engine documentation.
- [Unity Object.DestroyImmediate](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Object.DestroyImmediate.html): editor-oriented API and runtime guidance.
- [Unity thread context](https://docs.unity3d.com/6000.2/Documentation/Manual/async-awaitable-continuations.html): most Unity APIs require the main thread; this does not establish Awaitable availability in the game.
- [Debounced reload](../schedule1-debounced-reload/SKILL.md): generation and retry scheduling.
- [IL2CPP reflection](../schedule1-il2cpp-reflection/SKILL.md): verified array/member conversions.
