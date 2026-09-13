# S1API — Saveables (Custom Persistence in Save Games)

Saveables let your mod's custom data classes **persist in the schedule-i save game** with JSON serialization, GUID references, and automatic save-game restart safety.

---

## 1. The Saveable Pattern

```csharp
using S1API.Internal.Abstraction;
using S1API.Saveables;

public class NotesSave : Saveable
{
    [SaveableField("notes")] private List<Note> _notes = new();

    protected override void OnLoaded()
    {
        // Apply loaded data to your UI / managers
    }

    protected override void OnSaved()
    {
        // Optional cleanup of transient caches
    }

    // Load order relative to the base game:
    // AfterBaseGame (default) — Game entities (NPCs, buildings, ...) already exist
    // BeforeBaseGame          — for early hooks / global setup
    public override SaveableLoadOrder LoadOrder => SaveableLoadOrder.AfterBaseGame;
}
```

### Auto-Discovery

- Class must be `public`, **non-abstract**, with a **parameterless constructor**.
- S1API auto-discovers via reflection and instantiates **one instance per type** at save load.
- No manual registration needed.

---

## 2. `[SaveableField]` Attribute

```csharp
[SaveableField("version")] public int Version { get; set; } = 1;
[SaveableField("payload")] public List<MyItem> Items { get; set; } = new();
[SaveableField("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
```

- **`name` is required** — it's the JSON key in the save file.
- **Private fields work** — S1API uses reflection to read private state.
- **Initialize defaults** in the declaration or constructor.

---

## 3. Lifecycle of a Saveable

```
OnInitializeMelon()        your mod runs
  ↓
GameLifecycle.OnSaveLoaded   ← S1API instantiates your Saveable
  ↓
Saveable.OnLoaded()         ← your initializer runs
  ↓
[Game running, you read/write fields freely]
  ↓
Saveable.RequestGameSave()   ← trigger save (see below)
  ↓
Saveable.OnSaved()          ← cleanup hook
```

### Triggering a Save

```csharp
Saveable.RequestGameSave();   // asks S1API to save the game now
```

You don't call this on every change — it's expensive. Call it after significant state changes (e.g., user added a new note, player completed a quest).

---

## 4. Common Patterns

### Pattern A: Single Saveable Per Mod

```csharp
public class ModStateSave : Saveable
{
    [SaveableField("version")] public int Version { get; set; } = 1;
    [SaveableField("high_scores")] public List<int> HighScores { get; set; } = new();
    [SaveableField("settings")] public Dictionary<string, string> Settings { get; set; } = new();
}
```

### Pattern B: Saveable as a Sub-State of a Domain Object

```csharp
public class QuestData : Saveable
{
    [SaveableField("objective_progress")] public int ObjectiveProgress { get; set; }
    [SaveableField("optional_done")] public bool BonusComplete { get; set; }
}

public class MyQuest : Quest  // S1API.Quests
{
    [SaveableField("my_quest_data")]
    private QuestData _data = new();
}
```

Saveables can be nested: when their parent is saved, they're saved along.

### Pattern C: ID Lookup Pattern

```csharp
// Useful when you want to reference a vanilla or modded object in save data
[SaveableField("npc_id")]
private string _npcId = "kyle_cooley";

private NPC _npc;  // resolved at runtime

protected override void OnLoaded()
{
    _npc = NPCManager.GetNPCByID(_npcId);  // safe — runs after save load
}
```

---

## 5. Constraint Table

| Allowed | Not Allowed |
|---|---|
| `int`, `float`, `bool`, `string` | `Il2CppScheduleOne.*` references in fields |
| `List<T>` / `Dictionary<K,V>` of primitives | `[Serializable]` will not save to schedule-i save |
| Other `[SaveableField]`-tagged classes | `HashSet<T>` (use `List<T>` and `.Distinct()`) |
| `DateTime` | `Func` / `Action` / `delegate` |
| `[SaveableField]` tagged nested Saveables | Unity primitives — use `[SaveableField]` on the wrapper |

---

## 6. Drag-and-Drop Refresh Pattern

If your UI needs to react to save-load (e.g., re-display saved data after restore):

```csharp
public class NotesSave : Saveable
{
    [SaveableField("notes")] private List<Note> _notes = new();

    public event Action<List<Note>> NotesChanged;

    protected override void OnLoaded()
    {
        NotesChanged?.Invoke(_notes);
    }

    public void AddNote(Note n)
    {
        _notes.Add(n);
        NotesChanged?.Invoke(_notes);
        RequestGameSave();
    }
}
```

The UI subscribes to `NotesChanged` and rebuilds.

---

## 7. Debugging Tips

* **My data isn't saved → am I getting auto-discovered?** Check the MelonLoader console for `S1API.Saveables` log lines. The class must be `public`, not nested private, and have a parameterless constructor.
* **My collection shows up empty after save-load** → field needs `[SaveableField]`. S1API does NOT automatically serialize public fields.
* **Reference loops throw** → S1API uses `JsonIgnoreHandling` per cycle. For circular refs, use `[SaveableField]` on the inner objects too.

Deep reference: `Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Saveables/Saveable.cs`
