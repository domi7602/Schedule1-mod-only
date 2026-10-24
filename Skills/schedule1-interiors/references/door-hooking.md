# Door Integration and Interior Transitions

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 13 of 13 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.

## Evidence and use

Use for adding an entrance/menu action and returning safely from a custom interior. Historical
lookup anchors were `StaticDoor`, `DoorKnocker`, and a summon-menu type named `NPCSummonMenu` or
`NpcSummonMenu` in different examples. Verify exact namespace, case, overload, and lifecycle in the
installed assemblies before writing a Harmony target. These are not interchangeable declared types.

## Resolve the target and hook

Identify the intended building/door through a stable native identity, supported reference, or verified
hierarchy. Inspect what its interaction method does before choosing a prefix or postfix. A postfix
runs after the native action; it cannot undo an already-opened unrelated menu or a completed transition
merely by returning a value.

Scope every hook to the resolved target and current scene generation. Unrelated doors and summon
menus must retain native behavior. Detect missing target paths early and give one diagnostic rather
than repeatedly scanning the scene or patching all menus by name.

## Owned menu option

1. Resolve the live menu generation and its actual button template/layout owner.
2. Check whether this feature's option already exists. Reuse/rebind or remove the old owned option
   instead of adding another on each open.
3. Clone only the intended template. Inspect copied components/listeners and replace the inherited
   action only on the owned clone. Do not clear listeners from the native template.
4. Set text through the actual uGUI/TMP component and connect interaction using the verified
   IL2CPP-safe event adapter used by the repository.
5. Bind the current entrance/context rather than a stale captured door from a previous menu open.
6. Disable the action while a transition is pending or its prerequisites are unavailable.
7. Detach handlers and release the clone on menu replacement or final teardown.

The previous direct cast to a guessed `UnityAction` was removed. A managed-looking listener can require
a different bridge on the deployed IL2CPP runtime. Verify one click invokes one supported handler.

## Transition transaction

Prefer the game's supported player movement/teleport path. Determine local versus host authority and
any network transform, controller, camera, or velocity state it coordinates. Toggling a
`CharacterController` and writing a transform is not a universal multiplayer teleport implementation.

Before entry, validate room readiness, destination clearance, safe floor support, and a reachable exit.
Capture the return pose plus the relevant controller/camera/input state. Acquire transition input
ownership through the verified mechanism and prevent overlapping requests.

Perform movement using the chosen supported path. If temporary controller disablement is actually
required, capture its previous enabled state and restore that state in `finally`, while respecting
any newer owner. Never force enabled merely because the old sample did so. Reset velocity or camera
state only where the installed movement contract requires it.

Close the native menu and transfer focus in the order its lifecycle supports. Do not force a globally
locked/hidden cursor if an active UI still needs it. On failure, release this transition's resources
and restore a verified safe pose/state; report when recovery cannot be confirmed.

## Return and lifecycle

Store a return destination tied to the active scene/session. Validate it again before exit; geometry,
player instance, or map readiness may have changed. Maintain a tested exterior fallback and a defined
load-inside behavior. A raw saved transform alone does not prove that the destination is safe.

Cancel pending transitions on scene change, player replacement, room teardown, or application focus
loss where appropriate. Releasing one interior's input ownership must not clear another UI's gate.
Do not destroy a room while a player is inside without a supported evacuation/transition policy.

## Verify

Test repeated menu opens, double clicks, cancellation, blocked/missing destination, room-build
failure, enter/exit, load while inside, scene change, and each supported network role. Check original
door behavior, one injected option, restored input/cursor/camera, and absence of stale delegates.

Read [Phone input ownership](../../schedule1-phoneapp/references/input-focus-and-controls.md) and
[Room geometry](procedural-room.md) for dependent contracts.
