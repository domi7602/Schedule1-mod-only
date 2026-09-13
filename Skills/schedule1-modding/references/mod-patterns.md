# Mod Patterns — 7 Established Architectures

The active workspace mods (NotesApp, PotScanner, CalculatorApp, CustomSkateboard, MoreSaveSlots, PocketShop, BankApp, HomelessMod, BusinessIncome, Minimap, MoreDrugs) cluster into **7 architectural patterns**. Pick the pattern that matches your use-case before designing.

---

## Pattern 1: PhoneApp (S1API)

**Used by:** NotesApp, PotScanner, CalculatorApp, PocketShop, BankApp (and TVBrowser-archived).

**When:** A persistent in-game app reachable from the phone UI. Touch data the player keeps across sessions.

### Skeleton
```csharp
using S1API.PhoneApp;
using S1API.UI;
using UnityEngine;

public sealed class MyApp : PhoneApp
{
    protected override string AppName      => "MyMod";      // unique id
    protected override string AppTitle     => "My Mod";     // display
    protected override string IconLabel    => "My";         // under icon
    protected override string IconFileName => "icon.png";   // in Mods/

    // Optional sprite (programmatic) — use ONLY when you don't ship an icon.png
    protected override Sprite IconSprite { get; } = null;

    protected override void OnCreatedUI(GameObject container)
    {
        UITheme.Initialize(container.GetComponent<RectTransform>());
        var panel   = UIFactory.Panel("Root",   container.transform, new Color(0.1f, 0.1f, 0.1f), fullAnchor: true);
        // build scroll, list, buttons, input fields ...
        ButtonUtils.AddListener(button, OnClick);
    }
}
```

**Auto-Discovery:** S1API's `HomeScreen_Start_Patch` reflects over `PhoneApp` subclasses and instantiates them. You don't need to register the app manually — just place your class in the assembly and the game finds it.

**Key constraints:**
- Container is a *cloned template* of an in-game app; treat it as opaque.
- Use `UITheme.Sp/Dp` (Methode 3) for all sizes — see `references/ui-and-s1api.md`. **Delegate to `S1Mods.Shared.UITheme`** (Shared/UITheme.cs) — `InitializeForTextApp` (750f) or `InitializeForDashboard` (900f); never duplicate a local UITheme class.
- For text input: register an `InputFocus` MonoBehaviour (NotesAppInputFocus pattern, see AGENTS.md §5).
- For persistence: `SafeStorage.SaveAtomic(...)` on `OnApplicationQuit`.
- **Subscription lifetime (Rule 10, 2026-08-20):** `OnCreated` fires ONCE per scene. Never `Unsubscribe(MelonEvents.OnUpdate)` or `-=` static events inside `OnPhoneClosed` — app goes blank after first close. Defensive `-=`-before-`+=` only in `OnCreated`. Details: `schedule1-phoneapp` SKILL Rule 10 + `schedule1-s1api/references/lifecycle.md` §7.

### Save-Load Integration
Use `S1API.Saveables.Saveable` subclass for cross-session data:
```csharp
public class MyState : Saveable
{
    [SaveableField("data")] private List<Item> _items = new();
    public override SaveableLoadOrder LoadOrder => SaveableLoadOrder.AfterBaseGame;
}
```

**Reference impl:** NotesApp — clean PhoneApp + Saveable + InputFocus + SafeStorage combo.

---

## Pattern 2: HUD Overlay (uGUI / TextMeshPro / IMGUI)

**Used by:** Minimap, DayCounter (archived), ProfitTracker (archived), PotScanner.

**When:** Constantly visible status display (clock, money, weather, distance, etc.). Drawn over the world without a screen.

### Skeleton (uGUI / TextMeshPro)
```csharp
public override void OnInitializeMelon()
{
    GameObject hudGo = new GameObject("MyModHUD");
    Object.DontDestroyOnLoad(hudGo);
    var canvas = hudGo.AddComponent<Canvas>();
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    canvas.sortingOrder = 100;
    _text = hudGo.AddComponent<TextMeshProUGUI>();
    _text.text = "Initial";
    // anchors: top-left by default, draggable via input handlers
}
```

### Skeleton (IMGUI for debug)
```csharp
public override void OnUpdate()
{
    if (Input.GetKeyDown(KeyCode.F8)) _show = !_show;
}
public override void OnGUI()
{
    if (!_show) return;
    GUI.Box(new Rect(10, 10, 200, 60), $"Score: {_score:F0}");
}
```

### HotkeyManager + InputField-Focus-Guard
```csharp
using S1Mods.Shared;
HotkeyManager.Bind(KeyCode.F7, () => _enabled = !_enabled);
if (HotkeyManager.IsInputFieldFocused()) return; // skip toggle when user types
```

**Reference impl:** DayCounter — IMGUI HUD + drag/drop + 4 anchor presets + persistence via MelonPreferences.

---

## Pattern 3: Polling Service (Periodic Tick)

**Used by:** PotScanner, MoreDrugs ProductScanner.

**When:** Need to refresh data regularly from the game world (pot status, NPC list, station state). Tick via `OnUpdate` with cooldown timer; avoid per-frame scans.

### Skeleton
```csharp
private float _elapsed;
private const float IntervalSec = 2f;

public override void OnUpdate()
{
    _elapsed += Time.deltaTime;
    if (_elapsed < IntervalSec) return;
    _elapsed = 0f;
    Refresh();
}

private void Refresh()
{
    // use FindObjectsByType (Unity 2022.3+) ONCE per tick, not per frame
    // cache IntPtr lookups, avoid LINQ on Il2CppSystem.Collections
    for (int i = 0; i < _containers.Count; i++)
    {
        var c = _containers[i];
        // zero-alloc reads: UseStatic(c)->Foo instead of c.GetComponent<…>().Foo
    }
}
```

**Pitfalls:** IL2CPP marshalling costs; LINQ/foreach on `Il2CppSystem.Collections.Generic.List<T>` allocates → GC spikes. Use indexed `for` + cached `IntPtr` lookups (see `il2cpp-harmony-guide.md` §3).

**Reference impl:** PotScanner v0.1.0 — 2 s cooldown, FindObjectsByType, dry/all-moist badges, dictionary lookups for O(1) property assignment.

---

## Pattern 4: Saveable / Persistence Layer

**Used by:** MoreDrugs (custom-product provider), NotesApp.

**When:** Persisting custom data structures into the Schedule I save game.

### Skeleton (S1API's `Saveable` base class)
```csharp
public class MySave : Saveable
{
    [SaveableField("version")] public int Version { get; set; } = 1;
    [SaveableField("payload")] public List<MyItem> Items { get; set; } = new();

    protected override void OnLoaded()  { /* re-attach to runtime */ }
    protected override void OnSaved()   { /* optional cleanup */ }
    public override SaveableLoadOrder LoadOrder => SaveableLoadOrder.AfterBaseGame;
}
```

**Key rules:**
- Class must be `public`, non-abstract, with parameterless constructor.
- All persistent fields need `[SaveableField("name")]`.
- `GameLifecycle.OnSaveInfoLoaded` is the proper hook for save-data refresh — fire earlier than `OnGameplaySceneLoaded`.
- `Saveable.RequestGameSave()` triggers a save after significant changes.

### Standalone JSON Persist (no Save System)
For config / history / non-game-state: use `SafeStorage.SaveAtomic` from `S1Mods.Shared`:
```csharp
SafeStorage.SaveAtomic(filePath, JsonSerializer.Serialize(state));
if (SafeStorage.TryLoad(filePath, out string json))
    state = JsonSerializer.Deserialize<MyState>(json);
```
**Reference impl:** CalculatorApp — decimal-arithmetic + JSON history + clipboard + atomic save.

---

## Pattern 5: Harmony Patch (Cautious)

**Used by:** CustomSkateboard (GetSurfaceSmoothness, IsOnTerrain), MoreSaveSlots (SaveDisplay, ContinueScreen), TVBrowser, MikuPlayerModel (archived).

**When:** Modifying existing game behavior without re-implementing it from scratch.

### Skeleton
```csharp
using HarmonyLib;
using Il2CppScheduleOne.Skating;

[HarmonyPatch(typeof(Skateboard), nameof(Skateboard.OnMount))]
internal static class Patch_Skateboard_OnMount
{
    [HarmonyPostfix]
    private static void Postfix(Skateboard __instance)
    {
        // safe to modify __instance (Postfix guarantees run-after)
        // never modify ref params or struct fields in Prefix without planning
        if (__instance == null || __instance.WasCollected) return;
        // ...
    }
}
```

**CRITICAL Pitfalls (full list in `il2cpp-harmony-guide.md`):**
- ❌ **Never use Transpilers** on game code (IL is only marshalling glue).
- ⚠ **Small methods/getters may be inlined** → your Prefix-Postfix never fires. Patch the caller instead.
- ❌ **Don't pin methods with `MissingMethodException` IL body** → Harmony fails before your code runs.
- ✅ **Use `S1Mods.Shared.PatchGuard.TryPatch(...)`** for graceful-degradation on game updates.
- ✅ **Always** `if (__instance == null || __instance.WasCollected) return;` first.

**Reference impl:** MoreSaveSlots v1.1.1 — patches SaveDisplay, ContinueScreen, NewGameScreen, MenuScreen, SaveManager — with Harmony, no scene-desync.

---

## Pattern 6: ModConfig (MelonPreferences Wrapper)

**Used by:** PotScanner (`PotScannerConfig`), CalculatorApp, DayCounter, CustomSkateboard (skateboard stats), BusinessIncome (with JSON sidecar).

**When:** Storing user-tunable settings across sessions (toggle bools, scale floats, hotkey strings).

### Skeleton (`S1Mods.Shared.ModConfig<T>`)
```csharp
public class MyModConfig
{
    public bool Enabled       { get; set; } = true;
    public float Scale        { get; set; } = 1.0f;
    public string  HotkeyStr  { get; set; } = "F8";
}

// In Mod.cs OnInitializeMelon:
ModConfig<MyModConfig>.Initialize("MyMod");   // registers under MelonPreferences
var cfg = ModConfig<MyModConfig>.Instance;
cfg.OnChanged += (_, _) => Refresh();
```

**Backed by:**
- `UserData/MelonPreferences.cfg` (global), OR
- `<ModName>/config.json` (per-mod file)

### ⚠️ TOML Limit (2026-08-20)
`ModConfig<T>` **cannot persist `Dictionary<K,V>` / `List<T>` properties** ("not TOML-mappable" → silently dropped). If your config has dictionaries (multipliers, overrides, category lists), add a SafeStorage JSON sidecar:

```csharp
// Mod.cs
ModConfig<MyModConfig>.Initialize("MyMod");
ConfigJsonStore.ApplyToConfig(ModConfig<MyModConfig>.Instance);  // merge dicts

// ConfigJsonStore (see BusinessIncome/Services/ConfigJsonStore.cs)
public static void ApplyToConfig(MyModConfig cfg) { /* LoadSafe + merge */ }
public static void Save(MyModConfig cfg)          { /* SaveAtomic */ }
```

**Reference impl:** PotScannerConfig (pot-scanner/src/Services/PotScannerConfig.cs) — AutoWaterEnabled, plus per-throttle. BusinessIncomeConfig + ConfigJsonStore for dictionaries.

---

## Pattern 7: Console / Hash Terminal Command

**Used by:** PotScanner (`pot` command), DayCounter (`day` + `daycounter`), CustomSkateboard (`skate`).

**When:** Power-user diagnostics, in-game tweaking, hotkeys for QA, runtime state queries.

### S1API Console Command
```csharp
using S1API.Console;

[ConsoleCommand("mymod", "Short description")]
public sealed class MyModCommand : BaseConsoleCommand
{
    public override void Execute(string[] args)
    {
        // args[0] is subcommand, etc.
        switch (args[0])
        {
            case "toggle":  _enabled = !_enabled; break;
            case "stats":   Log($"Watered: {_wateredCount}"); break;
            default:        Log("Usage: mymod [toggle|stats]"); break;
        }
    }
}
```

### Hash Terminal (DooDesch) Plugin
```csharp
using Hash;
HashCommands.Add("mymod", "mymod help — show commands", args => {
    // args is List<string>
});
```

**Reference impl:** PotScanner — registers `pot give/stats/help` in both S1API and Hash.

---

## Choosing a Pattern (Decision Flow)

```
Want user interaction via screen?
├─ YES → PhoneApp (Pattern 1)
└─ NO  → Want persistent HUD overlay?
        ├─ YES → HUD (Pattern 2)
        └─ NO  → Want periodic game-state refresh?
                ├─ YES → Polling (Pattern 3)
                └─ NO  → Want to modify game behavior?
                        ├─ YES → Harmony Patch (Pattern 5)
                        └─ NO  → Want cross-session persistence?
                                ├─ YES → Saveable (Pattern 4)
                                └─ NO  → Want user-tunable settings?
                                        ├─ YES → ModConfig (Pattern 6)
                                        └─ NO  → Want console trigger?
                                                └─ YES → Console Command (Pattern 7)
```

A real mod typically combines 2-4 patterns:
- **NotesApp**: 1 (PhoneApp) + 4 (Saveable) + InputFocus + SafeStorage
- **PotScanner**: 1 (PhoneApp) + 2 (HUD) + 3 (Polling) + 6 (ModConfig) + 7 (Console) + 5 (Harmony)
- **CalculatorApp**: 1 (PhoneApp) + 4 (SafeStorage) + 6 (ModConfig) + InputFocus
- **PocketShop**: 1 (PhoneApp) + Multi-Payment + SFX + ItemDetailModal + Atomic Purchase (§5 architecture-and-shared)
- **BankApp**: 1 (PhoneApp) + SafeStorage Slot-Isolation + Weekly Limits + SFX + InputFocus
- **CustomSkateboard**: 5 (Harmony) + 6 (ModConfig) + 7 (Console)
- **HomelessMod**: 5 (Harmony / Build-Everywhere) + Procedural 3D Mesh + SafeStorage + 7 (Console)
- **BusinessIncome**: Multiplayer Host Guard + 4 (SafeStorage Idempotency) + 6 (ModConfig + JSON sidecar) + 7 (Console/Hash)
- **Minimap**: 2 (HUD Overlay / Dual-Shape) + 3 (0-Alloc Pooled Scan) + Drag&Drop + 6 (ModConfig) + 7 (Console/Hash) + Parameter-Keyed Sprite Cache
- **MoreSaveSlots**: 5 (Harmony, multiple patches) + Modal UI
- **DayCounter (archived)**: 2 (HUD) + 6 (ModConfig) + 7 (Console)

---

## Combined-Pattern Recommendations

* **Always** add Pattern 6 (ModConfig) for anything user-configurable. Free fall-back persistence.
* **Always** add Pattern 7 (Console Command) for diagnostic mods. Testers + future you will thank you.
* **Add Pattern 4 (Saveable)** if your mod needs to persist game-related data across save/load.
* **Add Pattern 5 (Harmony)** only if no S1API wrapper exists for the behavior you want to change.
