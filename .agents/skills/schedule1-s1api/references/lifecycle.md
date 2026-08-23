# S1API — Lifecycle Hooks & Save-Load Timing

The most important module for **not chasing ghost bugs**. Static game lists (`Property.OwnedProperties`, `NPCManager.Registered`, `Business.OwnedBusinesses`) are populated at different times than scene callbacks fire. Use `GameLifecycle` hooks; never poll.

---

## 1. The Hook Set

```csharp
public static class GameLifecycle
{
    public static event Action OnPreLoad;            // very early
    public static event Action OnSaveInfoLoaded;     // save info parsed, lists populated
    public static event Action OnLoadComplete;       // after scene build
    public static event Action OnPreSceneChange;     // before scene change
    public static event Action OnSaveStart;
    public static event Action OnSaveComplete;
}
```

| Hook | When | Use for |
|---|---|---|
| `OnSaveInfoLoaded` | Save-info parsed, BEFORE scene build | Property/NPC/Business cache refresh |
| `OnLoadComplete` | AFTER scene build complete | UI rebuild, attach to runtime, instantiate managers |
| `OnPreSceneChange` | Before scene change | Cache cleanup, unsubscribe |
| `OnSaveStart` | When player hits save | Optional pre-save state mutations |
| `OnSaveComplete` | After save | Diagnostic, post-save UI updates |

> **Naming:** `S1API` 3.2.0 exposes both `OnSaveInfoLoaded` AND `OnSaveLoaded`. The newer `OnSaveLoaded` is the recommended hook for new code; `OnSaveInfoLoaded` is kept for backward compatibility.

---

## 2. Subscribe / Unsubscribe Pattern

```csharp
public override void OnInitializeMelon()
{
    GameLifecycle.OnSaveLoaded   += OnSaveLoaded;
    GameLifecycle.OnLoadComplete += OnLoadComplete;
}

public override void OnApplicationQuit()
{
    GameLifecycle.OnSaveLoaded   -= OnSaveLoaded;   // defensive — avoids double-fire across multi-loads
    GameLifecycle.OnLoadComplete -= OnLoadComplete;
}
```

> **Gotcha:** If your mod doesn't unsubscribe, the handler fires **multiple times** on MelonLoader reload (e.g. dev mode). Always unsubscribe.

---

## 3. The Big Three Save-Load Patterns

### Pattern A: Property cache (typical for mods that show owned property lists)

```csharp
private HashSet<string> _ownedPropertyCodes = new();

private void OnSaveLoaded()
{
    _ownedPropertyCodes = new HashSet<string>(
        PropertyManager.GetOwnedProperties()
            .Select(p => p.PropertyCode)
    );
}

private void OnLoadComplete()
{
    // Now safe to query S1API's PropertyManager — it's populated.
    RefreshUI();
}
```

### Pattern B: NPC discovery (typical for mods that track NPCs)

```csharp
private List<NPC> _npcs = new();

private void OnSaveLoaded()
{
    _npcs = NPCManager.GetAllNPCs();   // returns wrappers, not vanilla
}

private void OnLoadComplete()
{
    foreach (var npc in _npcs)
    {
        // Subscribe to NPC events HERE, not in OnSaveLoaded
        npc.HealthChanged += OnNpcHealthChanged;
    }
}
```

### Pattern C: Quest state (typical for quest mods)

```csharp
private void OnSaveLoaded()
{
    // ALWAYS fresh lookup — never cache Quest/QuestEntry references
    var quest = QuestManager.GetQuestByName("Cold Concrete");
    if (quest != null)
    {
        foreach (var entry in quest.Entries)
        {
            // iterate by Title/String match, not by index
            if (entry.Title == "Sleep 1 night" && entry.State == EQuestState.Completed)
            {
                // ...
            }
        }
    }
}
```

---

## 4. Why `OnGameplaySceneLoaded` Is the Wrong Hook

```csharp
// ❌ DO NOT — lists are typically empty here
public override void OnSceneWasLoaded(int buildIndex, string sceneName)
{
    if (sceneName != "Main") return;
    var owned = PropertyManager.GetOwnedProperties();   // EMPTY!
    // ...
}
```

`OnGameplaySceneLoaded` fires **before** the save parser runs. Property/NPC/Business lists are empty for ~3 seconds.

```csharp
// ✅ DO — use S1API lifecycle hooks
public override void OnInitializeMelon()
{
    GameLifecycle.OnSaveLoaded += () =>
    {
        // Property.OwnedProperties, NPCManager.Registered, Business.OwnedBusinesses all populated
        var owned = PropertyManager.GetOwnedProperties();
        // ...
    };
}
```

---

## 5. Multiplayer Caveats

In multiplayer:
- `OnSaveLoaded` fires on **host + clients** but the world state may differ.
- Use `NetworkGuard` (from `S1Mods.Shared`) or `InstanceFinder.IsServer` (Il2CppFishNet) to gate host-only logic.
- Subscribe to `GameLifecycle.OnPreSceneChange` to clean up state before scene transitions.

---

## 6. Lifecycle Order Cheat Sheet

```
Game Launch
  ↓
OnPreLoad (very early)
  ↓
OnSaveInfoLoaded (after save-info, lists populated) ← BEST for Property cache
  ↓
OnLoadComplete (after scene build) ← BEST for UI init
  ↓
Scene playing
  ↓
OnPreSceneChange (before transition)
  ↓
OnSaveStart (player hits save)
  ↓
OnSaveComplete (after save)
```

For deep flow diagrams, see the [`S1API.Lifecycle.GameLifecycle` decompile](file:///Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Lifecycle/GameLifecycle.cs).

---

## 7. PhoneApp Lifecycle — `OnCreated` Fires ONCE per Scene (empirical, 2026-08-20)

Verified against `S1API.Internal.Patches.HomeScreen_Start_Patch` (decompile 3.2.0):

- `HomeScreen.Start()` → S1API reflects over all `PhoneApp` subclasses → `Activator.CreateInstance` → `(IRegisterable).CreateInternal()` → **`OnCreated()`**.
- `OnCreatedUI(container)` is called once per app creation (`SpawnUI` → `CreateAppContainer` → `OnCreatedUI`).
- `OnPhoneClosed()` fires on **every** phone close (subscribed to `Phone.onPhoneClosed`).

**Consequences (bug-hit 2026-08-20, all 5 PhoneApp mods):**
1. `OnCreated()` is NOT the right place to expect re-invocation on every phone open.
2. **Never unsubscribe your `MelonEvents.OnUpdate` handler (or static event handlers) inside `OnPhoneClosed()`** — the subscription dies forever until scene reload → app stays blank.
3. Correct pattern: defensive `Unsubscribe`-before-`Subscribe` in `OnCreated()` only; hide/show via `_mainBG.SetActive(open)` in `Update()`; use `OnDestroyed()` (not `OnPhoneClosed`) for real teardown.
4. S1API re-creates the app instance on **scene reload** (new `HomeScreen.Start`), so `OnCreated` may fire again after Main-Menu→Game transitions — the idempotent `-=`-then-`+=` in `OnCreated` handles that.

```csharp
protected override void OnCreated()
{
    base.OnCreated();
    MelonEvents.OnUpdate.Unsubscribe(Update);   // idempotent (scene reload)
    MelonEvents.OnUpdate.Subscribe(Update);
}

protected override void OnPhoneClosed()
{
    base.OnPhoneClosed();
    if (_mainBG != null) _mainBG.SetActive(false);
    // NEVER unsubscribe here — OnCreated fires only once per scene.
}
```
