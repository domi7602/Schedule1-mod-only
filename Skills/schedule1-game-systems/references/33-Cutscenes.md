# Cutscenes (Schedule I)
> verified: classes + methods + UnityEvents + event add/remove pairs re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: game v0.4.7f9 / S1API 3.2.1-beta.8.

## Core Classes (`ScheduleOne.Cutscenes`)

| Class | Verified base type | Purpose |
|-------|--------------------|---------|
| `CutsceneManager` | `Singleton<CutsceneManager>` (client-side, NOT networked) | Registry + playback control |
| `Cutscene` | `MonoBehaviour` | One cutscene: timeline-driven animation with camera control |
| `CutsceneCamera` | `MonoBehaviour` | Camera takeover point; `ControlFoV`, `FoV` |
| `IntroManager` | `Singleton<IntroManager>` | New-game intro (RV explosion, character creator handoff) |
| `EndCutscene` | `Cutscene` | Ending / credits sequence |

The whole namespace is exactly these 5 classes (no other cutscene classes in the dump). A previously listed `DemoIntro` class **does not exist** in the decompiles — removed.

## Cutscene lifecycle (verified)
- Fields: `Name`, `UseCinematicBars`, `IsPlaying` (property), `_animation`, `_state`, `_activeCameraController`
- Flow: `Play()` (virtual) → `LateUpdate()`/`UpdateCamera()` each frame → `InvokeEnd()` → `End()` (virtual)
- Camera: `SetActiveCameraControl(CutsceneCamera)`, `ClearActiveCameraControl(CutsceneCamera)`, `DisableActiveCameraControl()` — cutscene takes over from the FPS camera controller while active
- Cinematic bars: `CutsceneManager.SetCinematicBars(bool)`

## CutsceneManager API
- `Play(string name)` / `Play(Cutscene cutscene)` — the single entry point for triggering
- `RegisterCutscene(Cutscene)` / `UnregisterCutscene(Cutscene)` — cutscene components register themselves (`_cutscenes` list, `_activeCutscene` field)
- `OnActiveCutsceneEnded()` (internal teardown)

## Events
| Event | Type | Raised by |
|-------|------|-----------|
| `CutsceneManager.OnCutsceneStarted` | `Action<Cutscene>` (add/remove verified) | `Play(...)` |
| `CutsceneManager.OnCutsceneEnded` | `Action<Cutscene>` (add/remove verified) | `OnActiveCutsceneEnded()` |
| `Cutscene.onPlay` | `UnityEvent` | on playback start (invoke site inferred; native code not visible in interop dump) |
| `Cutscene.onEnd` | `UnityEvent` | `InvokeEnd()` is the end funnel (invoke site inferred) |
| `EndCutscene.onStandUp` / `onRunStart` / `onEngineStart` / `onLightsOn` | `UnityEvent` | associated with `StandUp()`, `RunStart()`, `EngineStart()`, `LightsOn()` timeline steps |

## Intro flow (IntroManager, verified fields)
- `Play()`, `PlayMusic()`, `CharacterCreationDone(CharacterCreatorState)`, `PassedStep(int stepIndex)`, `Update()`; skip support via `SkipInputTime`, `SkipContainer`, `SkipDial`, `currentSkipTime`
- Scene scaffolding: `CurrentStep`, `TimeOfDayOverride`, `Container`, `PlayerInitialPosition`, `PlayerInitialPosition_AfterRVExplosion`, `CameraContainer`, `Anim`, `DisableDuringIntro`, `rv` (the exploding RV), `MusicName`
- Completion callbacks: `onIntroDone`, `onIntroDoneAsServer` (Il2Cpp delegates) — this is where the tutorial/first quest chain kicks in

## Save Participation
- No `ISaveable` / `SaveData` in `ScheduleOne.Cutscenes` (grep verified). Cutscene state is transient; game progression is saved via the save system, not via cutscenes.

## Hook Points (Harmony)
1. `CutsceneManager.Play(string name)` — prefix/postfix to observe, block, or replace cutscenes by name (non-trivial instance method, safe patch).
2. `Cutscene.InvokeEnd()` — postfix to fire logic exactly when any cutscene finishes (e.g., unlock after credits; `InvokeEnd` is the single funnel into `End()`).
3. `IntroManager.PassedStep(int)` — hook tutorial step progression (used by the intro state machine).
- S1API: `S1API.Cutscenes` exists but is a **separate mod-side cutscene system** (static `CutsceneManager`, `CutsceneHandle`, `CutsceneFrame`, `CutsceneCamera`, `CutsceneBuilder`, `CutsceneEndReason` + `Internal/Cutscenes` adapter), not a wrapper over the game's `CutsceneManager`. Use it to run your own local cutscenes; it plays one cutscene at a time.

## Not Implemented / Notes
- Triggering is quest/scripted-flow driven — there is no public "trigger by id" dispatcher beyond `CutsceneManager.Play(name)`.
- Camera behavior lives in `Cutscene.UpdateCamera()`; overriding individual shots requires scene-level work (Animation + `CutsceneCamera` components), not just code.
