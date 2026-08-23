# PhoneApp Lifecycle & Canvas Architecture

This reference covers the complete lifecycle of an S1API `PhoneApp` inside Schedule I, the container hierarchy, orientation mechanics, and critical stability rules to prevent UI destruction bugs.

---

## 1. S1API Discovery & Instantiation

* **Auto-Discovery**: Any public non-abstract class inheriting from `S1API.PhoneApp.PhoneApp` is automatically discovered by S1API via reflection on startup.
* **HomeScreen Binding**: When the in-game phone initializes, S1API invokes `SpawnUI(HomeScreen)` and `SpawnIcon(HomeScreen)` for each registered app.
* **One Instance Per App**: S1API instantiates one singleton instance of your app class. Do not manually `new MyPhoneApp()` in your mod initialize logic.

```text
Game Launch / Scene Load
   │
   ▼
PhoneApp.OnCreated()          ◄── Register hooks, subscribe MelonEvents.OnUpdate
   │
   ▼
PhoneApp.SpawnUI()            ◄── S1API clones AppPanel from template inside AppsCanvas
   │
   ▼
PhoneApp.OnCreatedUI(container)◄── Build your entire UI hierarchy here (ONCE!)
```

---

## 2. Container Hierarchy & The Root Isolation Pattern

Inside `OnCreatedUI(GameObject container)`:
- `container` is a child GameObject inside the game's `AppsCanvas`.
- **Always build an isolated root panel (`_mainBG`)** as a direct child of `container.transform` with `fullAnchor: true`.
- **Start with `_mainBG.SetActive(false)`**: The app panel must remain invisible until `IsOpen()` becomes true.

```csharp
protected override void OnCreatedUI(GameObject container)
{
    var containerRt = container.GetComponent<RectTransform>();
    if (containerRt != null)
    {
        UITheme.Initialize(containerRt);
    }

    // 1. Isolated Full-Screen Background Panel
    _mainBG = UIFactory.Panel("MyApp_MainBG", container.transform, new Color(0.08f, 0.09f, 0.12f, 1f), fullAnchor: true);
    _mainBG.SetActive(false);

    var vlg = _mainBG.AddComponent<VerticalLayoutGroup>();
    vlg.childControlWidth = true;
    vlg.childControlHeight = true;
    vlg.childForceExpandWidth = true;
    vlg.childForceExpandHeight = false;

    // 2. Build Sub-Panels (Header, Content, Modals)
    BuildHeader(_mainBG.transform);
    BuildContentArea(_mainBG.transform);
    BuildModals(_mainBG.transform);
}
```

---

## 3. Orientation: Vertical vs. Horizontal

S1API allows two screen orientations via the `Orientation` property:

```csharp
public enum EOrientation
{
    Horizontal, // Wide landscape (tablet style, multi-column catalogs)
    Vertical    // Standard portrait (default for smartphone apps like Notes, Calculator, PotScanner)
}
```

### Key Differences:
| Setting | Use Case | Behavior |
|---|---|---|
| `EOrientation.Vertical` | Text, Tools, Standard Apps (NotesApp, CalculatorApp, PotScanner) | Keeps standard phone rotation, `LookOffsetMultiplier = 1.0f`. |
| `EOrientation.Horizontal` | Wide Dashboards, 5-Column Stores (PocketShop, TVBrowser) | Rotates phone model by 90°, adjusts camera look offset to `0.6f`. |

> [!IMPORTANT]
> **Always explicitly declare `Orientation` in your app class.**
> If omitted, S1API defaults to `EOrientation.Horizontal` in some versions, which triggers 90° phone rotation and camera offset changes!

```csharp
protected override EOrientation Orientation => EOrientation.Vertical; // or Horizontal
```

---

## 4. The Golden Rule: Never Destroy UI Hierarchies in `OnPhoneClosed()`

### ⚠️ The "Transparent Phone" (Empty Housing) Bug
In *Schedule I*, pressing <kbd>Tab</kbd> or clicking outside the phone lowers the phone and fires `Phone.onPhoneClosed`.

* **The Anti-Pattern**: Calling `Dispose()`, `Object.Destroy()`, or wiping data caches in `OnPhoneClosed()`.
  ```csharp
  // ❌ FATAL ANTI-PATTERN: DESTROYS THE APP FOREVER
  protected override void OnPhoneClosed()
  {
      base.OnPhoneClosed();
      _gridPane?.Dispose();       // Destroys child GameObjects!
      _directoryPane?.Dispose();  // Destroys root containers!
      _mainBG.SetActive(false);
  }
  ```
  When the player raises the phone again or re-enters the app, all child UI GameObjects have been destroyed. The 3D phone model renders with a completely empty, transparent screen.

* **The Correct Pattern**: Only hide modals or reset navigation state. Keep GameObjects alive:
  ```csharp
  // ✅ CLEAN & SAFE PATTERN
  protected override void OnPhoneClosed()
  {
      base.OnPhoneClosed();
      if (_mainBG != null) _mainBG.SetActive(false);
      _detailModal?.Hide();
      _viewMode = ViewMode.Directory; // Reset to default view without destroying anything
  }
  ```

---

## 5. Lifecycle Sync in `Update()`

Subscribe to `MelonEvents.OnUpdate` in `OnCreated()` to track `IsOpen()` transitions and handle periodic refreshes:

```csharp
protected override void OnCreated()
{
    base.OnCreated();
    // Idempotent: defensive Unsubscribe-before-Subscribe (re-creation safe).
    MelonEvents.OnUpdate.Unsubscribe(Update);
    MelonEvents.OnUpdate.Subscribe(Update);
}

private void Update()
{
    bool open = IsOpen();
    if (_mainBG != null && _mainBG.activeSelf != open)
    {
        _mainBG.SetActive(open);
        if (open)
        {
            OnAppOpened();
        }
        else
        {
            OnAppClosed();
        }
    }

    if (!open) return;

    // Physical Keyboard / Navigation
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        HandleBackOrClose();
    }

    // Periodic Refresh (e.g. 1s interval)
    if (Time.unscaledTime - _lastRefreshTime > 1.0f)
    {
        _lastRefreshTime = Time.unscaledTime;
        RefreshDynamicData();
    }
}
```

### ⚠️ Subscription Lifetime (empirical, 2026-08-20)

- `OnCreated()` fires **ONCE** per scene (S1API auto-discovery via `HomeScreen_Start_Patch` — verify in `Knowledge/Frameworks/S1API/Decompiles/3.2.0/S1API.Internal.Patches/HomeScreen_Start_Patch.cs`).
- `OnPhoneClosed()` fires on **every** phone close.
- **Therefore: NEVER `MelonEvents.OnUpdate.Unsubscribe(Update)` inside `OnPhoneClosed()`** — the Update loop would stay dead until the next scene reload, and `_mainBG` would never re-show → blank app on the 2nd open.
- Same for **static event handlers** (`OnPotsScanned`, `Money.OnBalanceChanged`, `TransactionHistoryService.OnHistoryChanged`, `_engine.OnStateChanged`): unsubscribing them in `OnPhoneClosed` kills live-refresh after the first close. The `-=`-before-`+=` in `OnCreated` is the correct idempotency pattern.
- If a handler really must be torn down, override `OnDestroyed()` instead of `OnPhoneClosed()` (S1API calls it once when the app is destroyed).
```
