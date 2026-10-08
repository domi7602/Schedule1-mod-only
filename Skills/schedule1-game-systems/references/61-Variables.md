# Variables & Conditions (Schedule I)
> verified: VariableDatabase, Variable hierarchy, Condition/Conditions, enums and events re-checked 2026-10-05 against decompiles (generation 2026-10-02; game v0.4.7f9). Anchor: 0.4.7f9-era evidence; runtime 0.4.7f11 per workspace AGENTS.md.

Namespace: `Il2CppScheduleOne.Variables` — 13 types in the decompile dump.

## Core Classes

| Class | Base | Purpose |
|-------|------|---------|
| `VariableDatabase` | `NetworkSingleton<VariableDatabase>` | Global variable store + replication |
| `BaseVariable` | Il2CppSystem.Object | Abstract base: `Name`, `Persistent`, `VariableMode`, `ReplicationMode`, `Owner` (Player) |
| `Variable<T>` | BaseVariable | Typed value: `GetValue()`/`SetValue(object, bool)`, `TryDeserialize(string)`, `ReplicateValue(...)` |
| `BoolVariable` | `Variable<bool>` | Bool variable |
| `NumberVariable` | `Variable<float>` | Numeric variable |
| `VariableCreator` | Il2CppSystem.Object | Serializable factory: `Name`, `Type`, `InitialValue`, `Persistent`, `Mode` |

`EVariableMode` = `{ Global, Player }` — variables are either global or per-player (owned via `Owner`; created by `CreatePlayerVariables(Player)` on spawn).
`EVariableReplicationMode` = `{ Local, Networked }`.
`EVariableType` = `{ Bool, Number }`.

## VariableDatabase API
- Setup: `Awake()` → `CreateVariables()` (from `Creators`); `CreatePlayerVariables(Player)` for player-scoped ones
- CRUD: `CreateVariable(name, EVariableType, initialValue, persistent, EVariableMode, Player, EVariableReplicationMode)`, `AddVariable(BaseVariable)`, `GetVariable(name)` → BaseVariable, `GetValue<T>(name)`
- `SetVariableValue(name, value, bool)` — string-typed setter used by serialization/console
- Networking: `SendValue(conn, name, value)` (server RPC) → `ReceiveValue` observers/target RPCs; `OnSpawnServer(conn)` pushes state
- Items: `NotifyItemAcquired(itemName, count)` + `ItemsToTrackAcquire` — auto-updates tracking variables
- DevConsole helpers: `PrintAllVariables()`, `PrintVariableValue(name)`
- ISaveable: `InitializeSaveable`, `GetSaveString()`, `LoadVariable(VariableData)` (data type in `Persistence\Datas`) — see 02-Save-Persistence

## Conditions
| Class | Fields | Method |
|-------|--------|--------|
| `Condition` | `VariableName`, `Operator` (`EConditionType`), `Value` | `Evaluate()` |
| `Conditions` | `EvaluationType` (`EEvaluationType`), `ConditionList`, `QuestConditionList` | `Evaluate()` |
| `QuestCondition` | `CheckQuestState`, `QuestName`, `QuestState`, `CheckQuestEntryState`, `QuestEntryIndex`, `QuestEntryState` | `Evaluate()` |

- `EConditionType` = `{ GreaterThan, LessThan, EqualTo, NotEqualTo, GreaterThanOrEqualTo, LessThanOrEqualTo }`
- `EEvaluationType` = `{ And, Or }`
- `Condition.Evaluate()` reads the variable from `VariableDatabase`; mixed variable+quest conditions are evaluated together in `Conditions`.

## Setters
- `VariableSetter { VariableName, NewValue }` with `Execute()` — sets a variable value; consumed by `Quests\SystemTrigger` (quest/dialogue side effects fire setters).
- Usable standalone: construct one and call `Execute()` to write through the same path as quest triggers.

## Events / UnityEvents
| Event | Type | Raised by |
|-------|------|-----------|
| `OnValueChanged` (Variable<T>) | **`UnityEvent<T>`** | `SetValue(object, bool)` |

No UnityEvents on the database itself; value replication uses FishNet RPCs (01-FishNet-Networking).

## Save Participation
- **Yes** — `VariableDatabase` is ISaveable (own save file via `SaveFolderName`/`GetSaveString`/`LoadVariable`); `Persistent` flag per variable decides what gets written.
- Player variables follow the player's save entry (per-owner persistence; exact layout unverified).

## Hook Points
1. **Postfix `VariableDatabase.SetVariableValue(string, string, bool)`** — react to any variable write (flags, counters) without patching each consumer.
2. **Prefix `Conditions.Evaluate()`** — override/extend condition checks for custom dialogue/quest gating.
3. **Postfix `VariableDatabase.CreateVariable(...)`** — inject variables at creation time (guarantees they exist on both server and clients).
- **No S1API wrapper namespace for Variables found in 3.2.1-beta.8** (S1API has no `S1API.Variables`) — use direct interop or `VariableSetter`.

## Applications (verified usage paths)
- Quest flags + quest-entry state via `QuestCondition` (03-Dialogue-Quest)
- Item-acquisition tracking via `NotifyItemAcquired`
- `SystemTrigger`-driven story state (27-Tutorial-Prolog)

## Not Implemented / Unverified
- Literal save-file names / folder layout: unverified (02-Save-Persistence).
- Old claim "ReadOnly / ReadWrite / ServerOnly modes" removed — actual `EVariableMode` is `{ Global, Player }`.

## Cross-links
03-Dialogue-Quest (QuestCondition usage) · 02-Save-Persistence (ISaveable) · 01-FishNet-Networking (replication) · 50-PlayerTasks · 27-Tutorial-Prolog
