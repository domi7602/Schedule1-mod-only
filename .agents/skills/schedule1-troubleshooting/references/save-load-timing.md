# Save-Load Timing — Static-Lists-Pitfalls & Solution

The Schedule I save-load pipeline is multi-phase. Most "static lists" (e.g. `Property.OwnedProperties`, `NPCManager.Registered`, `MixManager.Products`) are populated **AFTER** the gameplay scene loads, not before. The naive `OnGameplaySceneLoaded` hook fires too early.

This document explains the timeline, the available hooks, and the proven solution pattern.

---

## 1. The Multi-Phase Pipeline

```
Game boot
  ↓
Scene load (Menu scene or initial scene)
  ↓
Player clicks "Continue"
  ↓
Save file parse    ← statics populated HERE (1)
  ↓
Scene build        ← gameplay scene becomes active
  ↓
Lifecycle event fires:
  - OnGameplaySceneLoaded       ← TOO EARLY for static lists
  - OnSaveInfoLoaded            ← ★ right after parse, before scene build
  - OnLoadComplete              ← ★ after scene build complete
  - OnSceneWasLoaded (general)
  ↓
Game loop running
```

**Key observation:** Phase (1) is where the static lists populate — but `OnGameplaySceneLoaded` is the FIRST hook MelonLoader exposes, and S1API documents `GameLifecycle.OnSaveInfoLoaded` as the right one for refresh.

---

## 2. The Symptom

```
[<time>] [ModName] Info: OnGameplaySceneLoaded fired
[<time>] [ModName] Info: Found 0 owned properties       ← WRONG: should be 5
[<time>] [ModName] Info: Property.OwnedProperties.Count = 0
```

vs.

```
[<time>] [ModName] Info: OnSaveInfoLoaded fired         ← right hook
[<time>] [ModName] Info: Found 5 owned properties
[<time>] [ModName] Info: Property.OwnedProperties.Count = 5
```

---

## 3. The Solution: Hook `GameLifecycle.OnSaveInfoLoaded`

```csharp
// In Mod.cs / Mod class:
public override void OnInitializeMelon()
{
    GameLifecycle.OnSaveInfoLoaded += OnSaveInfoLoaded;
    GameLifecycle.OnLoadComplete   += OnLoadComplete;
    GameLifecycle.OnSaveStart      += OnSaveStart;
    GameLifecycle.OnSaveComplete   += OnSaveComplete;
}

public override void OnApplicationQuit()
{
    GameLifecycle.OnSaveInfoLoaded -= OnSaveInfoLoaded;
    GameLifecycle.OnLoadComplete   -= OnLoadComplete;
    // …
}

private void OnSaveInfoLoaded()
{
    // ★ Refresh property / NPC / item caches HERE
    // static lists are now populated
    RefreshCaches();
    UpdateUI();
}

private void OnLoadComplete()
{
    // Scene build complete; safe to attach UI / instantiate runtime objects
}
```

---

## 4. Why Not Retry / Polling?

A previously-used fallback (PotScanner v0.2.0) was a retry loop with delays 3s/6s/12s/25s:

```csharp
// v0.2.0 PRE-fix:
private void Tick()
{
    if (Property.OwnedProperties.Count > 0 && !_refreshed)
    {
        _refreshed = true;
        RefreshCaches();
    }
    // … no other code
}
```

**Why this is inferior:**
1. Worst-case 25 s wait before button is active.
2. Has flaky logic if scene loads during a retry.
3. Adds 60+ lines of timeout-management code.
4. Confused the log with retry diagnostics.

The S1API hook is **cleaner** (≈ 60 fewer lines in PotTracker.cs), **faster** (instant), and **more reliable** (deterministic, no timing guess).

---

## 5. Save-Side Hooks

For save-triggered logic (UI dirty-state, last-write wins, etc.), the symmetric hooks are:

```csharp
GameLifecycle.OnPreSave       += () => Log("Save starting");
GameLifecycle.OnSaveStart     += () => { /* before write */ };
GameLifecycle.OnSaveComplete  += () => { /* after write */ };
```

For mods persisting custom data into the save:
```csharp
public class MySave : Saveable
{
    [SaveableField("data")] private List<Item> _items = new();
    public override SaveableLoadOrder LoadOrder => SaveableLoadOrder.AfterBaseGame;
}
```

The `AfterBaseGame` order means items are loaded after the base-game state — the static lists are already populated when your `OnLoaded()` runs.

---

## 6. Reference Implementation: PotScanner v0.2.1 (2026-08-04)

Before (v0.2.0):
- `OnGameplaySceneLoaded` fires, OwnedProperties empty.
- 25-second retry loop in `PotTracker.Tick()` eventually catches the populated list.
- Used 60+ lines for retry logic.

After (v0.2.1):
```csharp
// PotTracker.cs
public void OnSaveInfoLoaded()
{
    RefreshPropertyCache();   // reliable, instant
    RefreshNow();              // UI updates in same frame
}
```

Mod.cs subscribes in `OnInitializeMelon`:
```csharp
public override void OnInitializeMelon()
{
    GameLifecycle.OnSaveInfoLoaded += _potTracker.OnSaveInfoLoaded;
    GameLifecycle.OnLoadComplete   += _potTracker.RefreshNow;
}
```

Result: Zero retry code, instant feedback, log is clean (`[PotScanner] [OnSaveInfoLoaded] refreshed 12 owned`).

---

## 7. Alternative: Implicit Refresh on First Access

If hooks aren't available (e.g. mod without S1API dependency), refresh on first access:

```csharp
private List<Property> _cachedProperties;
private bool _firstAccess = true;

public List<Property> GetOwnedProperties()
{
    if (_firstAccess || _cachedProperties == null)
    {
        _cachedProperties = Property.OwnedProperties.ToManagedList();
        _firstAccess = false;
    }
    return _cachedProperties;
}
```

This is **inferior** to the hook but works as a fallback when S1API isn't available.

---

## 8. Common Pitfalls in Save-Load Lifecycle Code

1. **Subscribe unsymmetrically.** If you subscribe `OnSaveInfoLoaded` in init but forget to unsubscribe in `OnApplicationQuit`, you can accumulate handlers across restarts.
2. **Use only the first phase.** `OnLoadComplete` is for UI init; `OnSaveInfoLoaded` is for cache refresh — pick the right one based on need.
3. **Forget hot-reload.** If your mod supports MelonLoader's hot-reload, subscribe/unsubscribe in `OnInitializeMelon`/`OnDeinitializeMelon` symmetrically.
4. **Race with FishNet SyncVars.** Multiplayer sync may overwrite your local cache — defer UI updates until `OnLoadComplete`.

---

## 9. Compatibility: When S1API Is Unavailable

For mods that avoid the S1API dependency (e.g. `MelonOptionalDependencies`):

```csharp
private void TrySubscribeToS1API()
{
    var glType = TypeResolver.Find("S1API.Lifecycle.GameLifecycle");
    if (glType == null) { _usePolling = true; return; }
    var onSaveLoaded = glType.GetEvent("OnSaveInfoLoaded");
    if (onSaveLoaded == null) { _usePolling = true; return; }
    onSaveLoaded.AddEventHandler(/* … */);
}
```

Use `S1Mods.Shared.TypeResolver.Find` to resolve the type across loaded assemblies.

---

## 10. Sanity Checks (Logging Pattern)

Always log the timing-order to validate:
```csharp
private void OnSaveInfoLoaded()        => Log.Info($"OnSaveInfoLoaded: Owned={Property.OwnedProperties.Count}");
private void OnGameplaySceneLoaded()   => Log.Info($"OnSceneLoaded: Owned={Property.OwnedProperties.Count}");
private void OnLoadComplete()          => Log.Info($"OnLoadComplete: Owned={Property.OwnedProperties.Count}");
```

If `OnSaveInfoLoaded.Count > 0` but `OnGameplaySceneLoaded.Count == 0`, **the lists populate between the two events** — perfect.
