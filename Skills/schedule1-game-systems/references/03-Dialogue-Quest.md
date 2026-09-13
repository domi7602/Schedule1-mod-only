# Dialogue/Quest (Schedule I)

## Dialogue System (Custom Built)
- **NO** YarnSpinner, Ink, or external framework
- Visual node graph editor in Unity (ScriptableObject-based)
- `DialogueContainer : ScriptableObject` with:
  - `NodeLinkData[]` (edges)
  - `DialogueNodeData[]` (speech bubble nodes: text, choices, voice line)
  - `BranchNodeData[]` (branches, evaluated programmatically)

## Dialogue Workflow
1. `DialogueController.Interacted()` (via InteractableObject)
2. `handler.InitializeDialogue(container)` at `ENTRY` node
3. `ShowNode()` → text + choices → `ChoiceSelected()` → next node
4. `EvaluateBranch()` on BranchNode → `CheckBranch(label)` → index

## Dialogue Events
- `DialogueNodeEvent` (UnityEvent on specific node)
- `DialogueChoice.onChoosen` (UnityEvent on choice)
- `ChoiceCallback()` in NPC-specific controller

## Quest System (C# Subclasses)
- Each quest = custom `MonoBehaviour` subclass of `Quest`
- **NO** visual quest scripting (everything in C#)

### EQuestState
`Inactive → Active → Completed / Failed / Expired / Cancelled`

### Quest State Tracking
- `StateMachine` (static): Bulk evaluation on state change
- `StateMachine.OnStateChange` triggers `AutoCompleteConditions`
- UnityEvents: `onQuestBegin`, `onQuestEnd`, `onComplete`, `onTrackChange`

### Quest Chains
- `PrerequisiteQuests[]` + `OnUncappedMinPass()` polling
- `SystemTrigger` with `Conditions[]` + `QuestStateSetter[]`
- `VariableDatabase` for time-based quests (e.g. `"Days_Since_Tutorial_Completed"`)

### Quest Journal (Phone App)
- `JournalApp : App<JournalApp>` with quest list + detail panel
- `QuestHUDUI` + `QuestEntryHUDUI` for tracked quests in HUD

### Quest Rewards
- `CompletionXP` → `LevelManager.AddXP()`
- Items → via dialogue callback or `onComplete` event
- Money → via `MoneyManager.ChangeCashBalance()`
- Unlocks → `VariableDatabase.SetVariableValue("Unlock", "true")`
