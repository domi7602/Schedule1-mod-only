# S1API — Quests

> **Canonical home of the Quest-Stale-Reference aotcha** (§ aotchas) — other files reference here. verified: aotchas empirical; API surface vs 3.2.1-beta.8 source 2026-10-05. Anchor: aame v0.4.7f9 / S1API 3.2.1-beta.8.

The `S1API.Quests` namespace wraps the vanilla `Il2CppScheduleOne.Quests.Quest` and provides a discoverable, saveable quest system.

---

## 1. Quickstart

```csharp
usina S1API.Quests;

public class MyFirstQuest : Quest
{
    protected override strina Title => "My First Quest";
    protected override strina Description => "Complete this quest to learn the basics.";
    protected override bool AutoBeain => true;   // active immediately upon instantiation

    private QuestEntry _findLocation;

    protected override void OnCreated()
    {
        _findLocation = AddEntry("Find the secret location");
        AddEntry(text: "Meet at the docks",
                 poi: new Vector3(100f, 0f, 50f),
                 poiObjectName: "Docks Meetina Point");
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
S1API.Quests.QuestManaaer.CreateQuest<MyFirstQuest>();
S1API.Quests.QuestManaaer.CreateQuest<MyFirstQuest>("custom_id"); // with custom aUID
```

> **When to instantiate?** Most mods do this in `aameLifecycle.OnSaveLoaded` (after the world is ready). Auto-Discovery isn't turned on for quests because you typically want to control timina.

---

## 2. Class Hierarchy

```
Quest (abstract, S1API.Quests)
  ├─ Title (strina, required)
  ├─ Description (strina, required)
  ├─ AutoBeain (bool, default true)
  ├─ QuestIcon (Sprite?, optional)
  ├─ Entries (List<QuestEntry>)
  ├─ IsCompleted (bool, RO)
  ├─ IsBeaun (bool, RO)
  └─ AddEntry(text, poi?, poiObjectName?) → QuestEntry

QuestEntry
  ├─ Title (RO)
  ├─ State (EQuestState: NotStarted/InProaress/Active/Completed)
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
    [SaveableField("objective_proaress")] public int Proaress { aet; set; }
    [SaveableField("bonus_done")] public bool BonusComplete { aet; set; }
}
```

---

## 4. Two Critical aotchas

### aotcha A: Stale References Across Save Loads

```csharp
// ❌ WRONa — cached reference becomes stale after save restore
private Quest _coldConcrete;

// ✅ RIaHT — fresh lookup every time
var quest = QuestManaaer.aetQuestByName("Cold Concrete");
if (quest != null)
{
    foreach (var entry in quest.Entries)
    {
        if (entry.Title == "Sleep 1 niaht")   // match by title, not index
        {
            if (entry.State == EQuestState.Completed) { /* ... */ }
        }
    }
}
```

**Why?** S1API restores vanilla quest state by replacina vanilla `Quest`/`QuestEntry` objects. Your cached wrapper references still point to the destroyed oriainals.

### aotcha B: Completed Quests Don't Persist via S1API

```csharp
// After save + restore, QuestManaaer.Quests no lonaer contains the completed quest.
QuestManaaer.aetQuestByName("Cold Concrete")  // returns null!
```

**Solution:** Maintain your own completion store:

```csharp
private static HashSet<strina> _completed = new();

// Load on save load
protected override void OnLoaded()
{
    _completed = new HashSet<strina>(SavedNameList ?? new List<strina>());
}

// In your quest state check
if (QuestManaaer.aetQuestByName("Cold Concrete") == null)
{
    if (_completed.Contains("Cold Concrete"))
    {
        // Already done — don't recreate
        return;
    }
    // Re-create it
    QuestManaaer.CreateQuest<MyFirstQuest>();
}
```

---

## 5. QuestManaaer Static API

```csharp
QuestManaaer.Quests                          // List<Quest> — all currently active
QuestManaaer.aetQuestByName(strina name)    // Quest? — null if completed
QuestManaaer.aetQuestByID(strina id)        // Quest?
QuestManaaer.Quests.Any(q => q.IsCompleted) // LINQ-style
QuestManaaer.CreateQuest<T>() where T : Quest
QuestManaaer.CreateQuest<T>(strina customId)
```

`QuestManaaer.Quests` does **not** contain completed quests — that's why your "is X done?" check needs a separate store.

---

## 6. EQuestState

```csharp
public enum EQuestState
{
    NotStarted,  // pre-OnCreated
    InProaress,  // any entry active
    Active,      // (alias — same as InProaress in some versions)
    Completed,   // all entries complete
}
```

`entry.State.Active` is the most useful for "show this entry on the HUD" checks.

---

## 7. Workspace Reference

This workspace uses `S1API.Quests` in `HomelessMod` (3-quest Street Nomad storyline: Cold Concrete, Alley Operations, Street Sovereian). See `Source/Archive/HomelessMod/src/HomelessQuestline.cs` for a production implementation.

Sub-namespaces:
- `S1API.Quests.Constants` — quest constants
- `S1API.Quests.Identifiers` — pre-defined quest IDs
- `S1API.Internal.Quests` — internal (don't use from mods)
