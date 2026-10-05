# Player Tasks (Schedule I)

> verified: classes, hierarchy, events re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Decompiles are IL2CPP interop stubs — hierarchy/signatures verified, method bodies not readable. Anchor: game v0.4.7f9 / S1API 3.2.1-beta.8.

## Core Classes (verified)

| Class | Hierarchy | Purpose |
|-------|-----------|---------|
| `Task` | `Il2CppScheduleOne.State.State` | **Tasks are state-machine states**, not mere minigames |
| `TaskManager` | `Singleton<TaskManager>` | Runs the current task |
| `TaskManagerUI` | `Singleton<TaskManagerUI>` | Canvas UI (`inputPromptUI`, `multiGrabIndicator`) |
| `ProgressSlider` | — (`ScheduleOne.UI`) | Progress bar for staged tasks |

## Task base (verified members)

- Properties: `TaskName` (protected setter → subclasses override), `CurrentInstruction`, `TaskActive`, `Outcome` (`EOutcome`: `Cancelled`, `Success`, `Fail`).
- Terminal methods: `Success()`, `Fail()`, `CancelTask()`, `StopTask()`.
- Loop hooks: `Update`, `LateUpdate`, `FixedUpdate`, `UpdateCursor`.
- Events (delegates on `Task`): `onTaskSuccess`, `onTaskFail`, `onTaskStop`.
- Drag/click plumbing: `ClickDetectionRange`, `ClickDetectionRadius`, `ClickDetectionEnabled`, `clickablesLayerMask`, `forcedClickables`, `MultiGrabRadius`, `MultiGrabForceMultiplier`, `multiDragTargets`, `isMultiDragging`, `EnableMultiDragging(Transform, float)`, `ForceStartClick`/`ForceEndClick(Clickable)`, `UpdateDraggablePhysics`.

## TaskManager (verified)

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

## Interaction Component Kinds (verified)

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

## Hook Points

1. **Prefix `TaskManager.StartTask` / postfix `TaskManager.EndTask`** — intercept any minigame start/end; alternatively subscribe `TaskManager.OnTaskStarted` (`Action<Task>`) via interop (event field, not patchable).
2. **Prefix `Task.Success` / `Task.Fail`** — auto-complete, force-fail or reward-modify tasks globally (fires for every task subclass).
3. **S1API:** no wrapper exists for tasks — patch native classes directly.
