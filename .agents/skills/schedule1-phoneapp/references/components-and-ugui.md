# uGUI Components, UIFactory & IL2CPP Rules

This reference documents the component building blocks, 0-allocation list patterns, modal dialogs, and IL2CPP event wiring for Schedule I Phone Apps.

---

## 1. S1API UIFactory Building Blocks

S1API provides `S1API.UI.UIFactory` with helper methods for constructing uGUI elements:

| Helper | Signature | Description |
|---|---|---|
| `UIFactory.Panel` | `(string name, Transform parent, Color color, bool fullAnchor = false)` | Creates a raw `GameObject` with `Image` and optional full stretch anchors. |
| `UIFactory.Text` | `(string name, string text, Transform parent, int fontSize, TextAnchor alignment, FontStyle style = FontStyle.Normal)` | Creates a standard uGUI `Text` element. |
| `UIFactory.Button` | `(string name, Transform parent, Color color)` | Creates a basic clickable button panel. |
| `UIFactory.RoundedButtonWithLabel` | `(string name, string label, Transform parent, Color btnColor, Color txtColor, int fontSize)` | Creates a pre-styled rounded button with child label. |

---

## 2. Event Listener Binding (IntPtr-Safe)

> [!CAUTION]
> In IL2CPP, calling `button.onClick.AddListener(new UnityEngine.Events.UnityAction(...))` directly often throws Marshalling / IntPtr Conversion Exceptions.

### Always Use `ButtonUtils` or `EventHelper`:
```csharp
using S1API.Utils; // Provides EventHelper

// Method A: S1API EventHelper
EventHelper.AddListener(() => OnButtonClicked(), button.onClick);

// Method B: Workspace ButtonUtils helper
public static class ButtonUtils
{
    public static void AddListener(Button btn, Action action)
    {
        EventHelper.AddListener(action, btn.onClick);
    }
}

ButtonUtils.AddListener(myButton, () => DoSomething());
```

---

## 3. Building High-Performance Scrollable Lists

### Structure of a ScrollRect:
```text
ScrollGO (ScrollRect)
  └── Viewport (RectMask2D + Image[alpha=0] + RectTransform[0,0,1,1])
        └── Content (RectTransform[pivot=0.5,1; anchors=0,1,1,1] + VerticalLayoutGroup + ContentSizeFitter)
              ├── ItemRow 0
              ├── ItemRow 1
              └── ...
```

### Complete Scrollable Container Code:
```csharp
public static GameObject CreateScrollView(Transform parent, out RectTransform contentRt)
{
    var scrollGO = new GameObject("ScrollView");
    scrollGO.transform.SetParent(parent, false);
    var sRt = scrollGO.AddComponent<RectTransform>();
    sRt.anchorMin = Vector2.zero;
    sRt.anchorMax = Vector2.one;
    sRt.offsetMin = Vector2.zero;
    sRt.offsetMax = Vector2.zero;

    var scrollRect = scrollGO.AddComponent<ScrollRect>();
    scrollRect.scrollSensitivity = 30f;
    scrollRect.horizontal = false;
    scrollRect.vertical = true;

    // Viewport
    var viewportGO = new GameObject("Viewport");
    viewportGO.transform.SetParent(scrollGO.transform, false);
    var vRt = viewportGO.AddComponent<RectTransform>();
    vRt.anchorMin = Vector2.zero;
    vRt.anchorMax = Vector2.one;
    vRt.offsetMin = Vector2.zero;
    vRt.offsetMax = Vector2.zero;
    var vImg = viewportGO.AddComponent<Image>();
    vImg.color = new Color(0, 0, 0, 0);
    viewportGO.AddComponent<RectMask2D>();
    scrollRect.viewport = vRt;

    // Content
    var contentGO = new GameObject("Content");
    contentGO.transform.SetParent(viewportGO.transform, false);
    contentRt = contentGO.AddComponent<RectTransform>();
    contentRt.anchorMin = new Vector2(0, 1);
    contentRt.anchorMax = new Vector2(1, 1);
    contentRt.pivot = new Vector2(0.5f, 1f);
    contentRt.offsetMin = Vector2.zero;
    contentRt.offsetMax = Vector2.zero;

    var vlg = contentGO.AddComponent<VerticalLayoutGroup>();
    vlg.childControlWidth = true;
    vlg.childControlHeight = true;
    vlg.childForceExpandWidth = true;
    vlg.childForceExpandHeight = false;
    vlg.spacing = UITheme.Dp(4f);

    var csf = contentGO.AddComponent<ContentSizeFitter>();
    csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

    scrollRect.content = contentRt;
    return scrollGO;
}
```

---

## 4. 0-Allocation UI Updates (Non-Allocating In-Place Polling)

Repeatedly destroying and re-instantiating GameObjects every 2 seconds causes severe GC frame drops and visual stutter.

### Recommended Pattern: In-Place Row Cache
1. Keep a `Dictionary<Id, RowUIRef>` storing references to the `Text`, `Image`, and `Button` components.
2. In your update loop, update `.text` and `.fillAmount` directly on existing objects.
3. Only instantiate rows when new items appear, and deactivate rows when items are removed.

```csharp
private readonly Dictionary<string, RowRef> _rowCache = new();

public void RefreshData(List<DataModel> items)
{
    // Update existing rows or spawn new ones
    foreach (var item in items)
    {
        if (!_rowCache.TryGetValue(item.Id, out var row))
        {
            row = SpawnRow(item);
            _rowCache[item.Id] = row;
        }

        // 0-allocation updates:
        if (row.TitleText.text != item.Title) row.TitleText.text = item.Title;
        if (row.ValueText.text != item.FormattedValue) row.ValueText.text = item.FormattedValue;
        row.RootObject.SetActive(true);
    }
}
```

---

## 5. Modal Dialogs & Backdrops

To display confirmation dialogs, inspection cards, or prompts:
1. Parent the modal directly to `_mainBG.transform` as a sibling to the content panel (so it renders on top).
2. Set full-stretch anchors (`Vector2.zero` to `Vector2.one`) with a semi-transparent dark backdrop (`new Color(0, 0, 0, 0.75f)`).
3. Add an empty `Button` or click listener on the backdrop to dismiss the modal when tapping outside.

```csharp
public class ConfirmModal
{
    private readonly GameObject _root;

    public ConfirmModal(Transform parent, Action onConfirmed)
    {
        // 1. Semi-transparent Fullscreen Backdrop
        _root = UIFactory.Panel("ModalBackdrop", parent, new Color(0, 0, 0, 0.75f), fullAnchor: true);
        _root.SetActive(false);

        // Backdrop Dismiss Click
        var dismissBtn = _root.AddComponent<Button>();
        dismissBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(dismissBtn, () => Hide());

        // 2. Centered Modal Card
        var card = UIFactory.Panel("ModalCard", _root.transform, new Color(0.12f, 0.15f, 0.20f, 1f));
        var cardRt = (RectTransform)card.transform;
        cardRt.anchorMin = new Vector2(0.5f, 0.5f);
        cardRt.anchorMax = new Vector2(0.5f, 0.5f);
        cardRt.sizeDelta = new Vector2(UITheme.Dp(320f), UITheme.Dp(180f));
        
        // Block raycasts from falling through to backdrop
        card.AddComponent<GraphicRaycaster>();
    }

    public void Show() => _root.SetActive(true);
    public void Hide() => _root.SetActive(false);
    public bool IsOpen => _root.activeSelf;
}
```
