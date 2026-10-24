# Player Tasks (Schedule I)

> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 80 of 80 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session. Decompiles are IL2CPP interop stubs — hierarchy/signatures verified, method bodies not readable.

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).

## Core Classes (previously reported)

| Class | Hierarchy | Purpose |
|-------|-----------|---------|
| `Task` | `Il2CppScheduleOne.State.State` | **Tasks are state-machine states**, not mere minigames |
| `TaskManager` | `Singleton<TaskManager>` | Runs the current task |
| `TaskManagerUI` | `Singleton<TaskManagerUI>` | Canvas UI (`inputPromptUI`, `multiGrabIndicator`) |
| `ProgressSlider` | — (`ScheduleOne.UI`) | Progress bar for staged tasks |

## Task base (previously reported members)

- Properties: `TaskName` (protected setter → subclasses override), `CurrentInstruction`, `TaskActive`, `Outcome` (`EOutcome`: `Cancelled`, `Success`, `Fail`).
- Terminal methods: `Success()`, `Fail()`, `CancelTask()`, `StopTask()`.
- Loop hooks: `Update`, `LateUpdate`, `FixedUpdate`, `UpdateCursor`.
- Events (delegates on `Task`): `onTaskSuccess`, `onTaskFail`, `onTaskStop`.
- Drag/click plumbing: `ClickDetectionRange`, `ClickDetectionRadius`, `ClickDetectionEnabled`, `clickablesLayerMask`, `forcedClickables`, `MultiGrabRadius`, `MultiGrabForceMultiplier`, `multiDragTargets`, `isMultiDragging`, `EnableMultiDragging(Transform, float)`, `ForceStartClick`/`ForceEndClick(Clickable)`, `UpdateDraggablePhysics`.

## TaskManager (previously reported)

`IsTaskActive`, `CurrentTask`, `TimeOnLastTaskEnd`, `StartTask(Task)`, `EndTask()`, `PlayTaskCompleteSound()` + `TaskCompleteSound` clip. Event: `OnTaskStarted` (`Action<Task>`).

## Concrete Tasks (hierarchy verified)

| Task | Extends |
|------|---------|
| `GrowContainerPourTask` | `Task` — pour-into-grow-container base |
| `PourOntoTargetTask` | `GrowContainerPourTask` |
| `PourWaterTask`, `PourSoilTask` | `PourOntoTargetTask` |
| `ApplyAdditiveToPot` | `GrowContainerPourTask` |
| `SowSeedTask`, `HarvestPlant`, `FillWaterContainer` | `Task` |
| `UseMixingStationTask` | `Task` |
| `UseChemistryStationTask`, `FinishChemistryStationTask` | `Task` |
| `StartLabOvenTask`, `FinalizeLabOven`, `LabOvenSolidTask` | `Task` |
| `UseBrickPress`, `CauldronTask` | `Task` |
| `PackageProductTask`, `PackageProductTaskMk2` | `Task` (fields: `station`, `Packaging`, `Products`, `FullyPacked`, `ReachedOutput`, `Sealed`) |
| `HarvestMushroomBedTask`, `ApplyShroomSpawnTask`, `InocculateGrainBagTask`, `MistMushroomBedTask` | `Task` |

> `UseMixingStationTask` and the `GrowContainerPourTask → PourOntoTargetTask → PourWaterTask/PourSoilTask` chain were missing from the old list — added.

## Interaction Component Kinds (previously reported)

| Component | Extends | Interaction |
|-----------|---------|-------------|
| `Clickable` | `MonoBehaviour` | Click to execute |
| `Draggable` | `Clickable` | Drag physics |
| `Moveable` | `Clickable` | Move object |
| `Pourable` | `Draggable` | Pour content |
| `Sprayable` | `Draggable` | Spray content |
| `RotateRigidbodyToTarget` | `MonoBehaviour` | Rotate-to-target |
| `DraggableConstraint` | `MonoBehaviour` | Constrains drags |
| `SpawnChunk` | `Clickable` | Spawnable chunk pieces |

Example in `PlayerScripts`: `WaterContainerPourable : Pourable` (refill target for `FillWaterContainer`).

## Save participation

None — tasks are runtime state; no `ISaveable` member anywhere in `PlayerTasks`.

## Candidate Hook Points

1. **Prefix `TaskManager.StartTask` / postfix `TaskManager.EndTask`** — intercept any minigame start/end; alternatively subscribe `TaskManager.OnTaskStarted` (`Action<Task>`) via interop (event field, not patchable).
2. **Prefix `Task.Success` / `Task.Fail`** — candidate boundaries for task completion/failure changes. Check overrides and actual call paths; patching a base method does not prove coverage of every subclass, and reward side effects must occur once.
3. **S1API:** no wrapper exists for tasks — patch native classes directly.
