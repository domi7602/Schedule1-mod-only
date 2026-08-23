# S1API — Quests

The `S1API.Quests` namespace wraps the vanilla `Il2CppScheduleOne.Quests.Quest` and provides a discoverable, saveable quest system.

---

## 1. Quickstart

```csharp
using S1API.Quests;

public class MyFirstQuest : Quest
{
    protected override string Title => "My First Quest";
    protected override string Description => "Complete this quest to learn the basics.";
    protected override bool AutoBegin => true;   // active immediately upon instantiation

    private QuestEntry _findLocation;

    protected override void OnCreated()
    {
        _findLocation = AddEntry("Find the secret location");
        AddEntry(text: "Meet at the docks",
                 poi: new Vector3(100f, 0f, 50f),
                 poiObjectName: "Docks Meeting Point");
    }

    public void PlayerFoundSecretLocation()
    {
        if (!_findLocation.IsCompleted)
            _findLocation.Complete();
        // Or: CompleteEntry(0); or CompleteAllEntries();
    }

    protected override void OnComplete() { /* Reward */ }
    protected override void OnFail()     { /* Cleanup/Penalty */ }
    protected override void OnLoaded()   { /* Restore UI state from save */ }
}
```

### Instantiation

```csharp
S1API.Quests.QuestManager.CreateQuest<MyFirstQuest>();
S1API.Quests.QuestManager.CreateQuest<MyFirstQuest>("custom_id"); // with custom GUID
```

> **When to instantiate?** Most mods do this in `GameLifecycle.OnSaveLoaded` (after the world is ready). Auto-Discovery isn't turned on for quests because you typically want to control timing.

---

## 2. Class Hierarchy

```
Quest (abstract, S1API.Quests)
  ├─ Title (string, required)
  ├─ Description (string, required)
  ├─ AutoBegin (bool, default true)
  ├─ QuestIcon (Sprite?, optional)
  ├─ Entries (List<QuestEntry>)
  ├─ IsCompleted (bool, RO)
  ├─ IsBegun (bool, RO)
  └─ AddEntry(text, poi?, poiObjectName?) → QuestEntry

QuestEntry
  ├─ Title (RO)
  ├─ State (EQuestState: NotStarted/InProgress/Active/Completed)
  ├─ PoILocation (Transform?, RO)
  ├─ IsCompleted (bool)
  ├─ IsActive (bool)
  ├─ Complete() / Fail() / Uncomplete()
  └─ PointOfInterest (Transform, RO)
```

---

## 3. Saveable Quest Data

Quests inherit from `Saveable`, so you can attach `[SaveableField]` data:

```csharp
public class MyQuest : Quest
{
    [SaveableField("my_quest_data")]
    private QuestData _data = new();

    protected override void OnLoaded()
    {
        // Restore UI state from saved data
    }
}

public class QuestData : Saveable
{
    [SaveableField("objective_progress")] public int Progress { get; set; }
    [SaveableField("bonus_done")] public bool BonusComplete { get; set; }
}
```

---

## 4. Two Critical Gotchas

### Gotcha A: Stale References Across Save Loads

```csharp
// ❌ WRONG — cached reference becomes stale after save restore
private Quest _coldConcrete;

// ✅ RIGHT — fresh lookup every time
var quest = QuestManager.GetQuestByName("Cold Concrete");
if (quest != null)
{
    foreach (var entry in quest.Entries)
    {
        if (entry.Title == "Sleep 1 night")   // match by title, not index
        {
            if (entry.State == EQuestState.Completed) { /* ... */ }
        }
    }
}
```

**Why?** S1API restores vanilla quest state by replacing vanilla `Quest`/`QuestEntry` objects. Your cached wrapper references still point to the destroyed originals.

### Gotcha B: Completed Quests Don't Persist via S1API

```csharp
// After save + restore, QuestManager.Quests no longer contains the completed quest.
QuestManager.GetQuestByName("Cold Concrete")  // returns null!
```

**Solution:** Maintain your own completion store:

```csharp
private static HashSet<string> _completed = new();

// Load on save load
protected override void OnLoaded()
{
    _completed = new HashSet<string>(SavedNameList ?? new List<string>());
}

// In your quest state check
if (QuestManager.GetQuestByName("Cold Concrete") == null)
{
    if (_completed.Contains("Cold Concrete"))
    {
        // Already done — don't recreate
        return;
    }
    // Re-create it
    QuestManager.CreateQuest<MyFirstQuest>();
}
```

---

## 5. QuestManager Static API

```csharp
QuestManager.Quests                          // List<Quest> — all currently active
QuestManager.GetQuestByName(string name)    // Quest? — null if completed
QuestManager.GetQuestByID(string id)        // Quest?
QuestManager.Quests.Any(q => q.IsCompleted) // LINQ-style
QuestManager.CreateQuest<T>() where T : Quest
QuestManager.CreateQuest<T>(string customId)
```

`QuestManager.Quests` does **not** contain completed quests — that's why your "is X done?" check needs a separate store.

---

## 6. EQuestState

```csharp
public enum EQuestState
{
    NotStarted,  // pre-OnCreated
    InProgress,  // any entry active
    Active,      // (alias — same as InProgress in some versions)
    Completed,   // all entries complete
}
```

`entry.State.Active` is the most useful for "show this entry on the HUD" checks.

---

## 7. Workspace Reference

This workspace uses `S1API.Quests` in `HomelessMod` (3-quest Street Nomad storyline: Cold Concrete, Alley Operations, Street Sovereign). See `Source/Mods/HomelessMod/src/Quests/HomelessQuestline.cs` for a production implementation.

Sub-namespaces:
- `S1API.Quests.Constants` — quest constants
- `S1API.Quests.Identifiers` — pre-defined quest IDs
- `S1API.Internal.Quests` — internal (don't use from mods)
