# Tutorial/Prologue (Schedule I)

## Overview
- Tutorial takes place in a **separate Unity scene `"Tutorial"`**
- Main game world is named `"Main"`
- `GameManager.IS_TUTORIAL` = `SceneManager.GetActiveScene().name == "Tutorial"`
- On launch: If `PlayTutorial = true` → load Tutorial scene, otherwise Main

## Intro Cutscene (`IntroManager.cs`)
1. **Animation**: Bus ride into town, RV is hidden
2. **Skip**: Hold jump/input/click for 0.5s → advances animation frame
3. Next step: **Character Creator** opens
4. After creation: Teleport to `PlayerInitialPosition` (or `_AfterRVExplosion` if RV was destroyed)
5. Equip clothing, enable player movement controls, save state written

## Tutorial Flow (3 Quests)

### Quest 1: "Getting Started"
- NPC: **Uncle Nelson** (first contact)
- CashDrop containing **$375** cash
- Find payphone, retrieve cash

### Quest 2: "Welcome to Hyland Point"
- Open phone + read messages
- **RV Explosion**:
  - Upon approaching RV (`ExplosionMaxDist = 25m`)
  - `RV.BlowUp()`: Explosion (20m radius, 200 damage, 1000 force impulse)
  - Camera shake, RV is destroyed
  - If between 25-50m: must point camera at RV for >0.4s

### Quest 3: "Down to Business"
- Tutorial completion tracking
- Upon completion: `Days_Since_Tutorial_Completed` increments daily

## RV (`RV.cs`)
- Subclass of Property, central location during the tutorial
- `BlowUp()`: Spawns an actual explosion via `CombatManager.CreateExplosion()`
- Tracks state via VariableDatabase: `RV_Soil_Pots`, `RV_Watered_Pots`, `RV_Seed_Pots`, `RV_SpeedGrow_Pots`
- After destruction: `ShouldSave() = false`

## Skipping the Tutorial
- `SetupScreen.cs`: **SkipIntroToggle** visible in UI
- When ON → `PlayTutorial = false` → tutorial is skipped
- Starts directly in the Main world

## Tutorial End
- `GameManager.EndTutorial(natural)`:
  1. `SaveManager.DisablePlayTutorial()` → Metadata `PlayTutorial = false`
  2. If `natural`: unlock achievement **COMPLETE_PROLOGUE**
  3. `ExitToMenu()` → player must manually load save (which loads Main scene)

## Tutorial-Specific UI Prompts
- `BodySearchTutorialDone`, `ItemAmountSelectionTutorialDone`, `InputHintsTutorialDone`
- `"Show on Map"` button disabled during tutorial
- `TutorialTimeController`: Controls time speed via keyframes

## Important Variables
| Variable | Type | Purpose |
|----------|------|---------|
| PlayTutorial | bool | Whether tutorial will be played on next load |
| Days_Since_Tutorial_Completed | float | Post-tutorial tracking (Loan Sharks) |
| BodySearchTutorialDone | bool | Body search tutorial shown |
| ItemAmountSelectionTutorialDone | bool | Quantity selection tutorial shown |
| InputHintsTutorialDone | bool | Input hints shown |
| Sleep_Count | float | Total sleep count |

## Tutorial Quests (Full Main Quest Chain)
1. GettingStarted → $375 from Nelson
2. WelcomeToHylandPoint → Phone/Messages + RV explosion
3. DownToBusiness → Tutorial completion
4. NeedingTheGreen → $10,000 lifetime earnings
5. OnTheGrind → Complete 3 deals
6. SinkOrSwim → $1,000 in 4 days for Loan Sharks
7. Connections → Build NPC relationships
8. GearingUp → Collect first dead drop
9. WeNeedToCook → Cook Meth
10. MovingUp → 10 customers
11. TheDeepEnd → Thomas meeting + kidnapping
12. ExpandingOperations → Grow tents
13. UnfavourableAgreements → Cartel meeting
14. Botanists/Chemists/Packagers/Cleaners → Employees
15. DealForCartel → Cartel deal
16. DefeatCartel → Endgame

## Files
- `Cutscenes/IntroManager.cs`
- `Property/RV.cs`
- `Quests/Quest_GettingStarted.cs`, `Quest_WelcomeToHylandPoint.cs`, `Quest_DownToBusiness.cs`
- `GameTime/TutorialTimeController.cs`
- `DevUtilities/GameManager.cs`
- `UI/MainMenu/SetupScreen.cs`
- `UI/TaskManagerUI.cs`, `BodySearchScreen.cs`, `HandoverScreen.cs`
- `NPCs/CharacterClasses/UncleNelson.cs`
- `Combat/Explosion.cs`, `Combat/ExplosionData.cs`
