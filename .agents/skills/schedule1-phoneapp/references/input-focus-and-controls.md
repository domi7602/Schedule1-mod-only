# Input-Focus Protection & Keyboard Navigation

When building Phone Apps in *Schedule I*, two critical input problems occur:
1. **The WASD & Game-Hotkey Leak**: When typing text into an `InputField` (e.g. search bar, notes editor, rename field), the base game's input system still listens for <kbd>W</kbd>, <kbd>A</kbd>, <kbd>S</kbd>, <kbd>D</kbd>, <kbd>Tab</kbd>, and number keys, causing the player character to walk around or trigger inventory hotkeys while typing.
2. **Missing Navigation Controls**: Players expect standard desktop shortcuts (<kbd>Escape</kbd> to go back/close, <kbd>Tab</kbd> to switch focus, <kbd>Ctrl+S</kbd> to save).

---

## 1. The Input-Focus Protection Hook

To block game controls while typing, create a dedicated `MonoBehaviour` decorated with `[RegisterTypeInIl2Cpp]` that sets `S1API.Input.Controls.IsTyping = true` while any `InputField` is focused.

### Crucial IL2CPP Requirement:
> [!CAUTION]
> Every `[RegisterTypeInIl2Cpp]` MonoBehaviour **MUST** have a public constructor taking `IntPtr ptr`:
> ```csharp
> public MyAppInputFocus(IntPtr ptr) : base(ptr) { }
> ```
> Missing this constructor will crash the IL2CPP interop bridge when instantiating the component!

### Complete InputFocus Implementation:
```csharp
using System;
using MelonLoader;
using S1API.Input;
using UnityEngine;
using UnityEngine.UI;

namespace MyMod.UI;

[RegisterTypeInIl2Cpp]
internal sealed class MyAppInputFocus : MonoBehaviour
{
    public MyAppInputFocus(IntPtr ptr) : base(ptr) { }

    public InputField? SearchInput;
    public InputField? EditorInput;

    private bool _lastTyping;

    private void Update()
    {
        bool typing = (SearchInput != null && SearchInput.isFocused)
                   || (EditorInput != null && EditorInput.isFocused);

        if (typing != _lastTyping)
        {
            _lastTyping = typing;
            Controls.IsTyping = typing; // Blocks game WASD/hotkeys when true
        }

        // Tab Navigation between fields
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (SearchInput != null && SearchInput.isFocused && EditorInput != null)
            {
                EditorInput.Select();
                EditorInput.ActivateInputField();
            }
            else if (EditorInput != null && EditorInput.isFocused && SearchInput != null)
            {
                SearchInput.Select();
                SearchInput.ActivateInputField();
            }
        }
    }

    private void OnDisable()
    {
        _lastTyping = false;
        Controls.IsTyping = false; // Always release game input on close
    }
}
```

### Attaching the Hook in `OnCreatedUI`:
```csharp
protected override void OnCreatedUI(GameObject container)
{
    // ... build UI ...
    var focusHook = container.AddComponent<MyAppInputFocus>();
    focusHook.SearchInput = _searchInput;
    focusHook.EditorInput = _noteEditorInput;
}
```

---

## 2. Desktop Keyboard Navigation

Inside your app's `Update()` loop (when `IsOpen()` is true), wire standard keyboard shortcuts:

```csharp
private void Update()
{
    if (!IsOpen()) return;

    bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

    // 1. Escape: Back / Dismiss Modal / Close
    if (Input.GetKeyDown(KeyCode.Escape))
    {
        if (_activeModal != null && _activeModal.IsOpen)
        {
            _activeModal.Hide();
        }
        else if (_currentView != ViewMode.Main)
        {
            SetViewMode(ViewMode.Main);
        }
        else
        {
            CloseApp();
        }
    }

    // 2. Ctrl + F: Quick Focus Search
    if (ctrl && Input.GetKeyDown(KeyCode.F))
    {
        if (_searchInput != null && _searchInput.gameObject.activeInHierarchy)
        {
            _searchInput.Select();
            _searchInput.ActivateInputField();
        }
    }

    // 3. Ctrl + S: Quick Save
    if (ctrl && Input.GetKeyDown(KeyCode.S))
    {
        SaveCurrentState();
    }
}
```

---

## 3. Multiline InputField Alignment Pitfall

By default, Unity uGUI centres multiline input fields vertically and horizontally, causing large text areas to render in the middle rather than starting from the top-left.

**Always apply this fix when creating multiline text areas:**
```csharp
var inputField = inputGO.AddComponent<InputField>();
inputField.lineType = InputField.LineType.MultiLineNewline;

// Fix alignment on both textComponent and placeholder!
if (inputField.textComponent != null)
{
    inputField.textComponent.alignment = TextAnchor.UpperLeft;
}
if (inputField.placeholder is Text placeholderText)
{
    placeholderText.alignment = TextAnchor.UpperLeft;
}
```
