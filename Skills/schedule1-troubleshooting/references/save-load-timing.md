# Save-Load Timing — Static-Lists-Pitfalls & Solution

> Verified: instrumented run 2026-09-29; lifecycle order re-checked against local decompiles on 2026-10-05. `OnSaveInfoLoaded` fired 0 times in that run.
> Placement-spezifisch (BuildableItem restore): [`../../schedule1-persistence/references/buildable-restore.md`](../../schedule1-persistence/references/buildable-restore.md).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 28/30 identifier-shaped tokens resolve (1 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

The Schedule I save-load pipeline is multi-phase. Most "static lists" (e.g. `Property.OwnedProperties`, `NPCManager.Registered`, product/mixing state (`MixingStation`, `MixerMap`, `EffectMixCalculator` —there is no `MixManager` type on the installed runtime)) are populated **AFTER** the gameplay scene loads, not before. The naive `OnGameplaySceneLoaded` hook fires too early.

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
  - OnSaveInfoLoaded <- not observed to fire in the instrumented session; do not rely on it without re-verification
  - OnLoadComplete              ← ★ after scene build complete
  - OnSceneWasLoaded (general)
  [VERIFIED 2026-09-29 (instrumented run): Scene Main loaded -> OnPreLoad -> OnLoadComplete (7.9 s later). OnSaveInfoLoaded fired 0 times, both at save-menu open and during the load, despite two subscribed logging handlers. Use OnPreLoad + OnSceneWasLoaded + OnLoadComplete for the observed flow.]
  [STATIC 2026-10-08 (f12 decompile): `Il2CppScheduleOne.Persistence.LoadManager` does declare `public unsafe UnityEvent onSaveInfoLoaded`, but a full-text scan of the 2 234 decompiled game files finds only 5 occurrences — the native field pointer, the property getter and the property setter, all inside LoadManager.cs itself. There is no managed invocation site anywhere in the assembly. So the 0-firing observation is consistent with the event never being raised from game code; it is not a S1API wiring bug. See schedule1-lifecycle-verify section 7.]
  ↓
Game loop running
```

**Key observation:** Static lists populate after `OnGameplaySceneLoaded`, but that callback is too early for refresh. S1API documentation describes `OnSaveInfoLoaded` as a refresh hook; in the instrumented session on 2026-09-29 it fired 0 times, and the f12 decompile shows no managed call site that raises it. Use `OnPreLoad` to reset and `OnLoadComplete` to refresh, then verify the order against the installed runtime.

---

## 2. The Symptom

```
[<time>] [ModName] Info: OnGameplaySceneLoaded fired
[<time>] [ModName] Info: Found 0 owned properties       ← WRONG: should be 5
[<time>] [ModName] Info: Property.OwnedProperties.Count = 0
```

vs.

```
[<time>] [ModName] Info: OnLoadComplete fired (refresh after scene build in the observed run)
[<time>] [ModName] Info: Found 5 owned properties
[<time>] [ModName] Info: Property.OwnedProperties.Count = 5
```

---

## 3. The Solution (`OnPreLoad` + `OnLoadComplete`; the older `OnSaveInfoLoaded` recipe is not relied on)

> **Observed in the 2026-09-29 instrumented run:** `OnSaveInfoLoaded` fired 0 times. The older recipe below is retained only as historical context; use `OnPreLoad` for reset, `OnSceneWasLoaded` for scene entry, and `OnLoadComplete` for final refresh, then re-check current runtime behavior.

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

A previously-used PotScanner fallback was a retry loop with delays 3s/6s/12s/25s:

```csharp
// PRE-FIX RETRY LOOP:
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

The lifecycle hook is **cleaner** (about 60 fewer lines in PotTracker.cs), **faster** (instant), and **more reliable** (deterministic, no timing guess). In the observed run, `OnLoadComplete` was the clean hook (see section 3; `OnSaveInfoLoaded` was not observed to fire).

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

## 6. Reference Implementation: PotScanner Lifecycle Fix (2026-08-04)

> An earlier PotScanner implementation used `OnSaveInfoLoaded`. A later instrumented session observed 0 firings, so do not rely on that result without retesting. The transferable lesson is to prefer deterministic lifecycle hooks over retry loops; use `OnLoadComplete` for the final refresh when confirmed in the installed runtime.

Before the lifecycle-hook fix:
- `OnGameplaySceneLoaded` fires, OwnedProperties empty.
- 25-second retry loop in `PotTracker.Tick()` eventually catches the populated list.
- Used 60+ lines for retry logic.

After the lifecycle-hook fix:
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

1. **Subscribe unsymmetrically.** If you subscribe lifecycle events in init but forget to unsubscribe in `OnApplicationQuit`, you can accumulate handlers across restarts.
2. **Use only the first phase.** `OnPreLoad` is for reset; `OnLoadComplete` is for cache refresh + UI init — pick the right one based on need.
3. **Forget hot-reload.** If your mod supports MelonLoader's hot-reload, subscribe/unsubscribe in `OnInitializeMelon`/`OnDeinitializeMelon` symmetrically.
4. **Race with FishNet SyncVars.** Multiplayer sync may overwrite your local cache — defer UI updates until `OnLoadComplete`. Diagnostic pattern: [`../fishnet-syncvar-diagnosis.md`](fishnet-syncvar-diagnosis.md).
5. **S1API docs describe `OnSaveInfoLoaded` as multi-fire, but the 2026-09-29 instrumented session observed 0 firings.** This is a runtime observation, not a universal guarantee. Keep handlers idempotent and re-check the installed build; `OnPreLoad` was observed for both slot switches and same-slot scene reloads (see `schedule1-modding` Rule 18).

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

Always log the timing order to validate against the installed runtime (the 2026-09-29 observation was Scene Main -> OnPreLoad -> OnLoadComplete):
```csharp
private void OnGameplaySceneLoaded()   => Log.Info($"OnSceneLoaded: Owned={Property.OwnedProperties.Count}");
private void OnPreLoad()               => Log.Info($"OnPreLoad: Owned={Property.OwnedProperties.Count}");
private void OnLoadComplete()          => Log.Info($"OnLoadComplete: Owned={Property.OwnedProperties.Count}");
```

If `OnLoadComplete.Count > 0` but `OnGameplaySceneLoaded.Count == 0`, **the lists populate between the two events** — expected. If `OnLoadComplete.Count == 0` too, statics populate even later (or are player-state dependent) — log the first successful refresh tick instead.

<!-- RESOLVED 2026-10-05: FishNet/SyncVar diagnostic pattern now exists → fishnet-syncvar-diagnosis.md (same folder). Host-side runtime behaviors there remain partially `unverified` until an instrumented multiplayer session. -->

---

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `MixManager`
