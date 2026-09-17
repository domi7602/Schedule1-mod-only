using System;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.UI;
using Il2CppScheduleOne.UI.Phone;
using Il2CppScheduleOne.UI.Phone.Map;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using S1Mods.Shared;

namespace DayCounter;

public sealed class DayCounterHUD
{
    private readonly ModLogger _log;

    private GameObject? _rootObj;
    private Canvas? _canvas;
    private UnityEngine.UI.CanvasScaler? _scaler;
    private CanvasGroup? _group;
    private RectTransform? _panelRt;
    private Image? _panelBg;

    // Accent line
    private GameObject? _accentObj;
    private RectTransform? _accentRt;
    private Image? _accentImg;

    // Standard 2-row container & TMPs
    private GameObject? _standardContainerObj;
    private TextMeshProUGUI? _headerTmp;
    private TextMeshProUGUI? _subTmp;

    // Compact 1-row container & TMP
    private GameObject? _compactContainerObj;
    private TextMeshProUGUI? _compactTmp;

    // Colors
    private static readonly Color ColorBg = new Color(0.07f, 0.08f, 0.10f, 0.75f);
    private static readonly Color ColorAccent = new Color(0.23f, 0.70f, 0.44f, 0.95f); // Emerald
    private static readonly Color ColorHeader = new Color(1f, 1f, 1f, 1f);
    private static readonly Color ColorSub = new Color(0.72f, 0.76f, 0.82f, 1f);

    // Cache state to avoid per-frame allocations & string formatting
    private int _lastElapsedDays = -999;
    private EDay _lastDayEnum = (EDay)(-1);
    private int _lastCurrentTime = -999;
    private bool _last24H = true;
    private bool _lastShortDay = false;
    private bool _lastCompact = false;
    private bool _lastShowDayNum = true;
    private bool _lastShowDayName = true;
    private bool _lastShowTime = true;

    // Dragging state
    private bool _isDragging;
    private Vector2 _dragPointerOffset;

    public DayCounterHUD(ModLogger log)
    {
        _log = log;
    }

    public bool IsCreated => _rootObj != null && (UnityEngine.Object)_rootObj != null;

    public void EnsureHUD(DayCounterConfig config)
    {
        if (IsCreated)
            return;

        try
        {
            // 1. Root Canvas
            _rootObj = new GameObject("DayCounter_HUD_Root");
            UnityEngine.Object.DontDestroyOnLoad(_rootObj);

            _canvas = _rootObj.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 4400;

            _scaler = _rootObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            _scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _scaler.referenceResolution = new Vector2(1920f, 1080f);
            _scaler.matchWidthOrHeight = 0.5f;

            _group = _rootObj.AddComponent<CanvasGroup>();
            _group.blocksRaycasts = config.AllowDragging;
            _group.interactable = false;

            // 2. HUD Panel
            GameObject panelObj = new GameObject("Card_Panel", Il2CppType.Of<RectTransform>());
            panelObj.transform.SetParent(_rootObj.transform, false);

            _panelRt = panelObj.GetComponent<RectTransform>();

            _panelBg = panelObj.AddComponent<Image>();
            _panelBg.color = new Color(ColorBg.r, ColorBg.g, ColorBg.b, Mathf.Clamp01(config.BackgroundOpacity));

            // 3. Left Accent Strip
            _accentObj = new GameObject("AccentStrip", Il2CppType.Of<RectTransform>());
            _accentObj.transform.SetParent(panelObj.transform, false);
            _accentRt = _accentObj.GetComponent<RectTransform>();
            _accentRt.anchorMin = new Vector2(0f, 0f);
            _accentRt.anchorMax = new Vector2(0f, 1f);
            _accentRt.pivot = new Vector2(0f, 0.5f);
            _accentRt.anchoredPosition = Vector2.zero;
            _accentRt.sizeDelta = new Vector2(3.5f, 0f);

            _accentImg = _accentObj.AddComponent<Image>();
            _accentImg.color = ColorAccent;
            _accentObj.SetActive(config.ShowAccentStrip);

            // 4. Standard 2-Row Container
            _standardContainerObj = new GameObject("StandardContainer", Il2CppType.Of<RectTransform>());
            _standardContainerObj.transform.SetParent(panelObj.transform, false);
            RectTransform stdRt = _standardContainerObj.GetComponent<RectTransform>();
            stdRt.anchorMin = Vector2.zero;
            stdRt.anchorMax = Vector2.one;
            stdRt.offsetMin = new Vector2(12f, 4f);
            stdRt.offsetMax = new Vector2(-10f, -4f);

            // Row 1: Header (e.g. DAY 14)
            _headerTmp = CreateTMP(_standardContainerObj.transform, "Header_TMP", "DAY 1", 15.5f, FontStyles.Bold, TextAlignmentOptions.Left, ColorHeader);
            RectTransform hdrRt = _headerTmp.GetComponent<RectTransform>();
            hdrRt.anchorMin = new Vector2(0f, 0.5f);
            hdrRt.anchorMax = new Vector2(1f, 1f);
            hdrRt.pivot = new Vector2(0f, 1f);
            hdrRt.anchoredPosition = Vector2.zero;
            hdrRt.sizeDelta = Vector2.zero;

            // Row 2: Subtitle (e.g. Wednesday • 15:30)
            _subTmp = CreateTMP(_standardContainerObj.transform, "Sub_TMP", "Monday • 08:00", 11.5f, FontStyles.Normal, TextAlignmentOptions.Left, ColorSub);
            RectTransform subRt = _subTmp.GetComponent<RectTransform>();
            subRt.anchorMin = new Vector2(0f, 0f);
            subRt.anchorMax = new Vector2(1f, 0.5f);
            subRt.pivot = new Vector2(0f, 0f);
            subRt.anchoredPosition = Vector2.zero;
            subRt.sizeDelta = Vector2.zero;

            // 5. Compact 1-Row Container
            _compactContainerObj = new GameObject("CompactContainer", Il2CppType.Of<RectTransform>());
            _compactContainerObj.transform.SetParent(panelObj.transform, false);
            RectTransform cptRt = _compactContainerObj.GetComponent<RectTransform>();
            cptRt.anchorMin = Vector2.zero;
            cptRt.anchorMax = Vector2.one;
            cptRt.offsetMin = new Vector2(12f, 2f);
            cptRt.offsetMax = new Vector2(-10f, -2f);

            _compactTmp = CreateTMP(_compactContainerObj.transform, "Compact_TMP", "DAY 1  │  Monday  08:00", 12f, FontStyles.Bold, TextAlignmentOptions.Left, ColorHeader);
            RectTransform cmpRt = _compactTmp.GetComponent<RectTransform>();
            cmpRt.anchorMin = Vector2.zero;
            cmpRt.anchorMax = Vector2.one;
            cmpRt.pivot = new Vector2(0f, 0.5f);
            cmpRt.anchoredPosition = Vector2.zero;
            cmpRt.sizeDelta = Vector2.zero;

            // Apply layout settings
            ApplyAnchorAndPosition(config);

            _rootObj.SetActive(false);
            _log.Info("DayCounter HUD created successfully.");
        }
        catch (Exception ex)
        {
            _log.Error("Failed to build DayCounter HUD", ex);
        }
    }

    public void ApplyAnchorAndPosition(DayCounterConfig config)
    {
        if (_panelRt == null || (UnityEngine.Object)_panelRt == null)
            return;

        Vector2 anchor;
        Vector2 pivot;
        switch (config.Anchor.ToLowerInvariant())
        {
            case "topright":
                anchor = new Vector2(1f, 1f);
                pivot = new Vector2(1f, 1f);
                break;
            case "bottomleft":
                anchor = new Vector2(0f, 0f);
                pivot = new Vector2(0f, 0f);
                break;
            case "bottomright":
                anchor = new Vector2(1f, 0f);
                pivot = new Vector2(1f, 0f);
                break;
            case "topleft":
            default:
                anchor = new Vector2(0f, 1f);
                pivot = new Vector2(0f, 1f);
                break;
        }

        _panelRt.anchorMin = anchor;
        _panelRt.anchorMax = anchor;
        _panelRt.pivot = pivot;
        _panelRt.anchoredPosition = new Vector2(config.PositionX, config.PositionY);

        // Fixed base card dimensions, scale via localScale
        float width = config.CompactMode ? 240f : 180f;
        float height = config.CompactMode ? 30f : 48f;

        _panelRt.sizeDelta = new Vector2(width, height);
        _panelRt.localScale = new Vector3(config.Scale, config.Scale, 1f);

        if (_panelBg != null && (UnityEngine.Object)_panelBg != null)
        {
            _panelBg.color = new Color(ColorBg.r, ColorBg.g, ColorBg.b, Mathf.Clamp01(config.BackgroundOpacity));
        }

        if (_accentObj != null && (UnityEngine.Object)_accentObj != null)
        {
            _accentObj.SetActive(config.ShowAccentStrip);
        }

        if (_standardContainerObj != null && (UnityEngine.Object)_standardContainerObj != null)
        {
            _standardContainerObj.SetActive(!config.CompactMode);
        }

        if (_compactContainerObj != null && (UnityEngine.Object)_compactContainerObj != null)
        {
            _compactContainerObj.SetActive(config.CompactMode);
        }

        if (_group != null && (UnityEngine.Object)_group != null)
        {
            _group.blocksRaycasts = config.AllowDragging;
        }

        // Force text refresh
        ForceRefreshValues();
    }

    public void RefreshFont()
    {
        var (font, mat) = DayCounterFont.ResolveFont();
        if (font != null && (UnityEngine.Object)font != null)
        {
            if (_headerTmp != null && (UnityEngine.Object)_headerTmp != null)
            {
                _headerTmp.font = font;
                if (mat != null) _headerTmp.fontSharedMaterial = mat;
            }

            if (_subTmp != null && (UnityEngine.Object)_subTmp != null)
            {
                _subTmp.font = font;
                if (mat != null) _subTmp.fontSharedMaterial = mat;
            }

            if (_compactTmp != null && (UnityEngine.Object)_compactTmp != null)
            {
                _compactTmp.font = font;
                if (mat != null) _compactTmp.fontSharedMaterial = mat;
            }

            _log.Info($"Refreshed DayCounter TMP fonts to '{font.name}'.");
        }
    }

    public void ForceRefreshValues()
    {
        _lastElapsedDays = -999;
        _lastCurrentTime = -999;
        _lastDayEnum = (EDay)(-1);
    }

    public void UpdateValues(int elapsedDays, EDay dayOfWeek, int currentTime, DayCounterConfig config)
    {
        if (!IsCreated)
            return;

        // Check if anything meaningful changed to avoid string allocations
        if (elapsedDays == _lastElapsedDays &&
            dayOfWeek == _lastDayEnum &&
            currentTime == _lastCurrentTime &&
            config.TimeFormat24H == _last24H &&
            config.ShortDayNames == _lastShortDay &&
            config.CompactMode == _lastCompact &&
            config.ShowDayNumber == _lastShowDayNum &&
            config.ShowDayName == _lastShowDayName &&
            config.ShowTime == _lastShowTime)
        {
            return;
        }

        _lastElapsedDays = elapsedDays;
        _lastDayEnum = dayOfWeek;
        _lastCurrentTime = currentTime;
        _last24H = config.TimeFormat24H;
        _lastShortDay = config.ShortDayNames;
        _lastCompact = config.CompactMode;
        _lastShowDayNum = config.ShowDayNumber;
        _lastShowDayName = config.ShowDayName;
        _lastShowTime = config.ShowTime;

        // 1. Format Day String
        string dayText = config.ShowDayNumber ? $"DAY {elapsedDays}" : "";

        // 2. Format Day of Week
        string dayNameText = "";
        if (config.ShowDayName)
        {
            string fullDay = dayOfWeek.ToString();
            dayNameText = config.ShortDayNames && fullDay.Length >= 3 ? fullDay.Substring(0, 3) : fullDay;
        }

        // 3. Format Time
        string timeText = "";
        if (config.ShowTime)
        {
            int rawHours = currentTime / 100;
            int rawMins = currentTime % 100;

            if (config.TimeFormat24H)
            {
                timeText = $"{rawHours:D2}:{rawMins:D2}";
            }
            else
            {
                string amPm = rawHours >= 12 ? "PM" : "AM";
                int h12 = rawHours % 12;
                if (h12 == 0) h12 = 12;
                timeText = $"{h12}:{rawMins:D2} {amPm}";
            }
        }

        // 4. Update UI Elements
        if (config.CompactMode)
        {
            if (_compactTmp != null && (UnityEngine.Object)_compactTmp != null)
            {
                string combined = "";
                if (!string.IsNullOrEmpty(dayText)) combined += $"<color=white>{dayText}</color>";
                if (!string.IsNullOrEmpty(dayNameText))
                {
                    if (combined.Length > 0) combined += " ";
                    combined += $"<color=#B0B8C0>{dayNameText}</color>";
                }
                if (!string.IsNullOrEmpty(timeText))
                {
                    if (combined.Length > 0) combined += " ";
                    combined += $"<color=#E0E4E8>{timeText}</color>";
                }

                _compactTmp.text = combined;
            }
        }
        else
        {
            if (_headerTmp != null && (UnityEngine.Object)_headerTmp != null)
            {
                _headerTmp.text = dayText;
            }

            if (_subTmp != null && (UnityEngine.Object)_subTmp != null)
            {
                string subCombined = "";
                if (!string.IsNullOrEmpty(dayNameText)) subCombined += dayNameText;
                if (!string.IsNullOrEmpty(timeText))
                {
                    if (subCombined.Length > 0) subCombined += " ";
                    subCombined += timeText;
                }
                _subTmp.text = subCombined;
            }
        }
    }

    public void SetVisible(bool visible)
    {
        if (_rootObj != null && (UnityEngine.Object)_rootObj != null)
        {
            if (_rootObj.activeSelf != visible)
                _rootObj.SetActive(visible);
        }
    }

    public void HandleMouseDrag(DayCounterConfig config, Action<float, float> onPositionChanged)
    {
        if (!config.AllowDragging || _panelRt == null || (UnityEngine.Object)_panelRt == null)
            return;

        if (_rootObj == null || !_rootObj.activeSelf)
        {
            _isDragging = false;
            return;
        }

        if (_panelRt.parent is not RectTransform parentRt)
            return;

        Vector2 mousePos = Input.mousePosition;

        // When mouse clicked down
        if (Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_panelRt, mousePos, null))
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRt, mousePos, null, out Vector2 parentStartPoint))
                {
                    _isDragging = true;
                    _dragPointerOffset = parentStartPoint - _panelRt.anchoredPosition;
                }
            }
        }

        if (_isDragging && Input.GetMouseButton(0))
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRt, mousePos, null, out Vector2 currentParentPoint))
            {
                Vector2 rawTargetPos = currentParentPoint - _dragPointerOffset;

                // Scale & Pivot-aware boundaries
                float scaleX = Mathf.Abs(_panelRt.localScale.x);
                float scaleY = Mathf.Abs(_panelRt.localScale.y);
                float effW = _panelRt.rect.width * scaleX;
                float effH = _panelRt.rect.height * scaleY;
                float parentW = parentRt.rect.width;
                float parentH = parentRt.rect.height;

                float minX, maxX, minY, maxY;
                string anchorLower = config.Anchor.ToLowerInvariant();
                switch (anchorLower)
                {
                    case "topright":
                        minX = -(parentW - effW);
                        maxX = 0f;
                        minY = -(parentH - effH);
                        maxY = 0f;
                        break;
                    case "bottomleft":
                        minX = 0f;
                        maxX = parentW - effW;
                        minY = 0f;
                        maxY = parentH - effH;
                        break;
                    case "bottomright":
                        minX = -(parentW - effW);
                        maxX = 0f;
                        minY = 0f;
                        maxY = parentH - effH;
                        break;
                    case "topleft":
                    default:
                        minX = 0f;
                        maxX = parentW - effW;
                        minY = -(parentH - effH);
                        maxY = 0f;
                        break;
                }

                float clampedX = Mathf.Clamp(rawTargetPos.x, minX, maxX);
                float clampedY = Mathf.Clamp(rawTargetPos.y, minY, maxY);
                _panelRt.anchoredPosition = new Vector2(clampedX, clampedY);
            }
        }

        if (_isDragging && Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            onPositionChanged?.Invoke(_panelRt.anchoredPosition.x, _panelRt.anchoredPosition.y);
        }
    }

    public bool CheckShouldHide(DayCounterConfig config)
    {
        if (!config.AutoHideInMenus)
            return false;

        try
        {
            if (Phone.Instance != null && (UnityEngine.Object)Phone.Instance != null && Phone.Instance.IsOpen)
                return true;

            if (PauseMenu.Instance != null && (UnityEngine.Object)PauseMenu.Instance != null && PauseMenu.Instance.IsPaused)
                return true;

            if (MapApp.Instance != null && (UnityEngine.Object)MapApp.Instance != null && MapApp.Instance.opened)
                return true;
        }
        catch (Exception ex)
        {
            _log.Warn($"Error during HUD visibility check: {ex.Message}");
        }

        return false;
    }

    private static TextMeshProUGUI CreateTMP(Transform parent, string name, string text, float fontSize, FontStyles style, TextAlignmentOptions align, Color color)
    {
        GameObject obj = new GameObject(name, Il2CppType.Of<RectTransform>());
        obj.transform.SetParent(parent, false);

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        var (font, mat) = DayCounterFont.ResolveFont();
        if (font != null && (UnityEngine.Object)font != null)
        {
            tmp.font = font;
            if (mat != null)
                tmp.fontSharedMaterial = mat;
        }

        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.fontStyle = style;
        tmp.alignment = align;
        tmp.color = color;
        tmp.raycastTarget = false;
        tmp.richText = true;
        return tmp;
    }

    public void Destroy()
    {
        if (_rootObj != null && (UnityEngine.Object)_rootObj != null)
        {
            UnityEngine.Object.Destroy(_rootObj);
            _rootObj = null;
        }
    }
}
