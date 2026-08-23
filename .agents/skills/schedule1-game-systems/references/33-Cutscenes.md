# Cutscenes (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `CutsceneManager` | Global cutscene manager |
| `Cutscene` | Base cutscene class |
| `IntroManager` | Tutorial intro cutscene manager |
| `EndCutscene` | Ending / credits cutscene |

## Cutscene System

- Sequences of animated scenes
- Camera positions and dialogue overlays
- NPC positioning and animation control
- Triggered via the quest system

## Intro

- `IntroManager`: RV explosion, tutorial introduction
- `DemoIntro`: Demo version intro
- Triggered upon starting a new game

## Ending Cutscene

- `EndCutscene`: Main story conclusion
- Credits rollout
- Narrative resolution

## Integration

- Cutscenes are triggered by quest events
- `Cutscene` provides Start/End callbacks
- Camera overrides active during cutscenes
