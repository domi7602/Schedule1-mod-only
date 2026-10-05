# S1API — Lifecycle Hooks & Save-Load Timing

> verified: instrumented run 2026-09-29 (order + OnSaveInfoLoaded = 0 firings); event set re-verified against 3.2.1-beta.8 source 2026-10-05. Anchor: game v0.4.7f9 / S1API 3.2.1-beta.8.

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
| `OnPreLoad` | Before save data loads; also fires for same-slot Menu→Game scene reloads (modding Rule 18) | Reset caches, clear state |
| `OnLoadComplete` | AFTER scene build | **Cache refresh, UI rebuild, attach to runtime, instantiate managers** |
| `OnPreSceneChange` | Before scene change | Cache cleanup, unsubscribe |
| `OnSaveStart` | When player hits save | Optional pre-save state mutations |
| `OnSaveComplete` | After save | Diagnostic, post-save UI updates |
| `OnSaveInfoLoaded` | **DEAD on game 0.4.7f6+: fires 0×** (verified 2026-09-29 — `schedule1-lifecycle-verify` §7) | Do not use for refresh |

> **Naming (verified 2026-10-05 against 3.2.1-beta.8 source + deployed DLL):** `GameLifecycle` exposes EXACTLY these 6 events — `OnPreLoad`, `OnLoadComplete`, `OnPreSceneChange`, `OnSaveInfoLoaded`, `OnSaveStart`, `OnSaveComplete`. **`OnSaveLoaded` does NOT exist** (earlier editions of this file recommended it — that was wrong). Refresh work goes into `OnLoadComplete`. Verified order: Scene 'Main' loaded → `OnPreLoad` → `OnLoadComplete` (instrumented run 2026-09-29).

---

## 2. Subscribe / Unsubscribe Pattern

```csharp
public override void OnInitializeMelon()
{
    GameLifecycle.OnPreLoad      += OnPreLoad;        // reset caches
    GameLifecycle.OnLoadComplete += OnLoadComplete;
}

public override void OnApplicationQuit()
{
    GameLifecycle.OnPreLoad      -= OnPreLoad;   // defensive — avoids double-fire across multi-loads
    GameLifecycle.OnLoadComplete -= OnLoadComplete;
}
```

> **Gotcha:** If your mod doesn't unsubscribe, the handler fires **multiple times** on MelonLoader reload (e.g. dev mode). Always unsubscribe.

---

## 3. The Big Three Save-Load Patterns

### Pattern A: Property cache (typical for mods that show owned property lists)

```csharp
private HashSet<string> _ownedPropertyCodes = new();

private void OnLoadComplete()
{
    // Lists are populated by now (verified order: Scene 'Main' → OnPreLoad → OnLoadComplete).
    _ownedPropertyCodes = new HashSet<string>(
        PropertyManager.GetOwnedProperties()
            .Select(p => p.PropertyCode)
    );
    RefreshUI();
}
```

### Pattern B: NPC discovery (typical for mods that track NPCs)

```csharp
private List<NPC> _npcs = new();

private void OnLoadComplete()
{
    _npcs = NPCManager.GetAllNPCs();   // returns wrappers, not vanilla — populated by now
    foreach (var npc in _npcs)
    {
        // Subscribe to NPC events HERE, not in OnPreLoad
        npc.HealthChanged += OnNpcHealthChanged;
    }
}
```

### Pattern C: Quest state (typical for quest mods)

```csharp
private void OnLoadComplete()
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
    GameLifecycle.OnLoadComplete += () =>
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
- Lifecycle events fire on **host + clients** but the world state may differ.
- Use `NetworkGuard` (from `S1Mods.Shared`) or `InstanceFinder.IsServer` (Il2CppFishNet) to gate host-only logic.
- Subscribe to `GameLifecycle.OnPreSceneChange` to clean up state before scene transitions.

---

## 6. Lifecycle Order Cheat Sheet

```
Game Launch
  ↓
Scene 'Main' loads (OnSceneWasLoaded)
  ↓
OnPreLoad  (reset caches; ALSO fires for same-slot Menu→Game reloads)
  ↓
OnLoadComplete (after scene build) ← BEST for Property cache + UI init  [verified 2026-09-29]
  ↓
Scene playing
  ↓
OnPreSceneChange (before transition)
  ↓
OnSaveStart (player hits save)
  ↓
OnSaveComplete (after save)

DEAD: OnSaveInfoLoaded — 0 firings on game 0.4.7f6+ (2026-09-29 instrumented run).
The ≤ 0.4.6f13 pipeline had an 'OnSaveInfoLoaded after parse' stage between the two — that stage no longer exists on 0.4.7f6+.
```

For deep flow diagrams, see the [`S1API.Lifecycle.GameLifecycle` source](../../../ThirdParty/S1API/S1API/Lifecycle/GameLifecycle.cs).

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
