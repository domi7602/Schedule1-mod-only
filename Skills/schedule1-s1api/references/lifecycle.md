# S1API lifecycle and readiness

> verified: instrumented run 2026-09-29 (order + OnSaveInfoLoaded = 0 firings); event set re-checked against checked-in S1API source on 2026-10-05.
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 42 of 42 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

Source scope: official online documentation reviewed on 2026-10-21; installed
S1API 3.2.1 and native game behavior were not tested by this package. Inherited <!-- skills-health: version-evidence (S1API target requested by the owner; installed build state lives in AGENTS.md) -->
session observations are identified separately below.

## Hook selection

The online [GameLifecycle reference](https://ifbars.github.io/S1API/api/S1API.Lifecycle.GameLifecycle.html)
describes these events:

| Event | Documented purpose | Design implication |
|---|---|---|
| `OnPreLoad` | Before saved game data loads | Reset load-scoped state; prepare required definitions |
| `OnLoadComplete` | After world data loads | Initialize systems needing the loaded world |
| `OnPreSceneChange` | Before scene transition | Release transition-owned resources |
| `OnSaveInfoLoaded` | Save metadata/menu refresh | Refresh save selection metadata, not world readiness |
| `OnSaveStart` | Host/server starts saving | Capture the intended save boundary |
| `OnSaveComplete` | Host/server completes saving | Observe completion under the verified save contract |

Do not invent `OnSaveLoaded` or `OnPreSave` aliases. Match event availability and
actual forwarding to the target DLL before implementation. Save callbacks are not
a universal host-and-client synchronization channel.

## Own subscriptions explicitly

Assign every handler an owning lifetime: mod, loaded world, app instance, or native
object. Register once for that lifetime and remove the exact delegate on teardown.
Repeated subtract-then-add is idempotent only for the same delegate target; a new
instance cannot remove an old instance's method subscription that way.

On a new world generation, detach handlers from old NPCs/objects before replacing
collections. Avoid untracked anonymous handlers when later unsubscription is
required. Guard late callbacks with the owning generation and object validity.
Keep lifecycle registration reachable during initialization instead of placing it
behind readiness that the callback itself is supposed to establish.

## Readiness and identity

A callback is a starting signal, not proof of every optional subsystem's readiness.
Check the specific manager, collection, identity, and authority the feature needs.
If dependencies finish later, use a bounded targeted retry with one deadline and
cancellation on world change. Avoid a permanent whole-scene poll.

Cache persistent IDs and reacquire scene-owned wrappers after restore. Do not reuse
an old QuestEntry, NPC wrapper, or Unity object simply because the same slot opened.
Filter scene cleanup to the owning world; auxiliary scenes may unload independently.

## Save semantics

Distinguish a save request, a save-start snapshot, and a completed native checkpoint.
For sidecars, confirm target identity and compatibility before publishing pending
state. A completion notification alone does not make multiple files transactional.
Use [persistence](../../schedule1-persistence/SKILL.md) for failed writes and recovery.

## Inherited evidence and required tests

The original notes describe a 2026-09-29 trace with Main scene notification before
`OnPreLoad`, then `OnLoadComplete`, and zero observed `OnSaveInfoLoaded` callbacks.
The original logs and assembly identities are not bundled. Do not promote that one
trace into a universal event order or a claim that the event never fires.

Verify first load, same-world reload, different-world load, menu metadata refresh,
normal save, failed save, and teardown as relevant. On multiplayer features include
host, client, and late join. Instrument handler entry and the required state, not
only a success message at the end of the handler.
Use [lifecycle verification](../../schedule1-lifecycle-verify/SKILL.md) when order
or native completion is still uncertain.
