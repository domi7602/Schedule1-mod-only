using System;
using System.Globalization;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.UI.Phone;
using Il2CppScheduleOne.UI.Phone.Map;
using Il2CppScheduleOne.UI;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using S1Mods.Shared;

namespace ProfitTracker;

public sealed class ProfitTrackerHUD
{
    private readonly ModLogger _log;

    private GameObject? _rootObj;
    private Canvas? _canvas;
    private RectTransform? _panelRt;
    private Image? _panelBg;
    private TextMeshProUGUI? _todayLabelTmp;
    private TextMeshProUGUI? _todayValueTmp;
    private TextMeshProUGUI? _weekLabelTmp;
    private TextMeshProUGUI? _weekValueTmp;
    private TextMeshProUGUI? _sessionLabelTmp;
    private TextMeshProUGUI? _sessionValueTmp;
    private GameObject? _sessionRowObj;

    private static readonly Color ColorProfit = new Color(0.38f, 0.95f, 0.48f, 1f);
    private static readonly Color ColorLoss = new Color(1f, 0.38f, 0.38f, 1f);
    private static readonly Color ColorNeutral = new Color(0.70f, 0.74f, 0.78f, 1f);
    private static readonly Color ColorLabel = new Color(0.72f, 0.76f, 0.80f, 1f);

    // Dragging state
    private bool _isDragging;
    private Vector2 _dragPointerOffset;

    public ProfitTrackerHUD(ModLogger log)
    {
        _log = log;
    }

    public bool IsCreated => _rootObj != null && (UnityEngine.Object)_rootObj != null;

    public void EnsureHUD(ProfitTrackerConfig config)
    {
        if (IsCreated)
            return;

        try
        {
            // 1. Root Canvas
            _rootObj = new GameObject("ProfitTracker_HUD_Root");
            UnityEngine.Object.DontDestroyOnLoad(_rootObj);

            _canvas = _rootObj.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 4500;

            var scaler = _rootObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            var group = _rootObj.AddComponent<CanvasGroup>();
            group.blocksRaycasts = config.AllowDragging;
            group.interactable = false;

            // 2. HUD Panel
            GameObject panelObj = new GameObject("Card_Panel", Il2CppType.Of<RectTransform>());
            panelObj.transform.SetParent(_rootObj.transform, false);

            _panelRt = panelObj.GetComponent<RectTransform>();
            ApplyAnchorAndPosition(config);

            _panelBg = panelObj.AddComponent<Image>();
            _panelBg.color = new Color(0.06f, 0.07f, 0.09f, Mathf.Clamp01(config.BackgroundOpacity));

            // Top accent indicator strip (sleek 2px emerald line)
            GameObject stripObj = new GameObject("AccentStrip", Il2CppType.Of<RectTransform>());
            stripObj.transform.SetParent(panelObj.transform, false);
            RectTransform stripRt = stripObj.GetComponent<RectTransform>();
            stripRt.anchorMin = new Vector2(0f, 1f);
            stripRt.anchorMax = new Vector2(1f, 1f);
            stripRt.pivot = new Vector2(0.5f, 1f);
            stripRt.anchoredPosition = Vector2.zero;
            stripRt.sizeDelta = new Vector2(0f, 2f);

            Image stripImg = stripObj.AddComponent<Image>();
            stripImg.color = new Color(0.25f, 0.85f, 0.55f, 0.90f);

            // 3. Rows Container
            // Height is 72px for 2 rows, or 92px for 3 rows
            float row1Y = -12f;
            float row2Y = -38f;
            float row3Y = -64f;

            // Row 1: TODAY
            CreateRow(panelObj.transform, "Row_Today", row1Y, "TODAY", out _todayLabelTmp, out _todayValueTmp);

            // Row 2: WEEK
            CreateRow(panelObj.transform, "Row_Week", row2Y, "WEEK", out _weekLabelTmp, out _weekValueTmp);

            // Row 3: SESSION (optional)
            _sessionRowObj = CreateRow(panelObj.transform, "Row_Session", row3Y, "SESSION", out _sessionLabelTmp, out _sessionValueTmp);
            _sessionRowObj.SetActive(config.ShowSessionRow);

            _rootObj.SetActive(false);
        }
        catch (Exception ex)
        {
            _log.Error("Failed to build ProfitTracker HUD", ex);
        }
    }

    public void ApplyAnchorAndPosition(ProfitTrackerConfig config)
    {
        if (_panelRt == null || (UnityEngine.Object)_panelRt == null)
            return;

        Vector2 anchor;
        Vector2 pivot;
        switch (config.Anchor.ToLowerInvariant())
        {
            case "topleft":
                anchor = new Vector2(0f, 1f);
                pivot = new Vector2(0f, 1f);
                break;
            case "bottomleft":
                anchor = new Vector2(0f, 0f);
                pivot = new Vector2(0f, 0f);
                break;
            case "bottomright":
                anchor = new Vector2(1f, 0f);
                pivot = new Vector2(1f, 0f);
                break;
            case "topright":
            default:
                anchor = new Vector2(1f, 1f);
                pivot = new Vector2(1f, 1f);
                break;
        }

        _panelRt.anchorMin = anchor;
        _panelRt.anchorMax = anchor;
        _panelRt.pivot = pivot;
        _panelRt.anchoredPosition = new Vector2(config.PositionX, config.PositionY);

        float height = config.ShowSessionRow ? 92f : 72f;
        _panelRt.sizeDelta = new Vector2(200f * config.Scale, height * config.Scale);
        _panelRt.localScale = Vector3.one;

        if (_panelBg != null && (UnityEngine.Object)_panelBg != null)
        {
            _panelBg.color = new Color(0.06f, 0.07f, 0.09f, Mathf.Clamp01(config.BackgroundOpacity));
        }

        if (_sessionRowObj != null && (UnityEngine.Object)_sessionRowObj != null)
        {
            _sessionRowObj.SetActive(config.ShowSessionRow);
        }
    }

    public void UpdateValues(float todayDelta, float weekDelta, float sessionDelta, bool useNetWorth, ProfitTrackerConfig config)
    {
        if (!IsCreated)
            return;

        string prefix = useNetWorth ? "" : "CASH ";

        if (_todayLabelTmp != null && (UnityEngine.Object)_todayLabelTmp != null)
            _todayLabelTmp.text = prefix + "TODAY";

        if (_todayValueTmp != null && (UnityEngine.Object)_todayValueTmp != null)
        {
            _todayValueTmp.text = FormatDelta(todayDelta);
            _todayValueTmp.color = todayDelta > 0.01f ? ColorProfit : (todayDelta < -0.01f ? ColorLoss : ColorNeutral);
        }

        if (_weekLabelTmp != null && (UnityEngine.Object)_weekLabelTmp != null)
            _weekLabelTmp.text = prefix + "WEEK";

        if (_weekValueTmp != null && (UnityEngine.Object)_weekValueTmp != null)
        {
            _weekValueTmp.text = FormatDelta(weekDelta);
            _weekValueTmp.color = weekDelta > 0.01f ? ColorProfit : (weekDelta < -0.01f ? ColorLoss : ColorNeutral);
        }

        if (config.ShowSessionRow && _sessionValueTmp != null && (UnityEngine.Object)_sessionValueTmp != null)
        {
            _sessionValueTmp.text = FormatDelta(sessionDelta);
            _sessionValueTmp.color = sessionDelta > 0.01f ? ColorProfit : (sessionDelta < -0.01f ? ColorLoss : ColorNeutral);
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

    public void HandleMouseDrag(ProfitTrackerConfig config, Action<float, float> onPositionChanged)
    {
        if (!config.AllowDragging || _panelRt == null || (UnityEngine.Object)_panelRt == null)
            return;

        if (_rootObj == null || !_rootObj.activeSelf)
        {
            _isDragging = false;
            return;
        }

        // When mouse clicked
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(_panelRt, mousePos, null))
            {
                _isDragging = true;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_panelRt, mousePos, null, out _dragPointerOffset);
            }
        }

        if (_isDragging && Input.GetMouseButton(0))
        {
            if (_panelRt.parent is RectTransform parentRt)
            {
                Vector2 mousePos = Input.mousePosition;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRt, mousePos, null, out Vector2 localPoint))
                {
                    Vector2 newPos = localPoint - _dragPointerOffset;
                    // Clamp to canvas bounds
                    float halfW = _panelRt.rect.width * 0.5f;
                    float halfH = _panelRt.rect.height * 0.5f;
                    float clampedX = Mathf.Clamp(newPos.x, -parentRt.rect.width * 0.5f + halfW, parentRt.rect.width * 0.5f - halfW);
                    float clampedY = Mathf.Clamp(newPos.y, -parentRt.rect.height * 0.5f + halfH, parentRt.rect.height * 0.5f - halfH);
                    _panelRt.anchoredPosition = new Vector2(clampedX, clampedY);
                }
            }
        }

        if (_isDragging && Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            onPositionChanged?.Invoke(_panelRt.anchoredPosition.x, _panelRt.anchoredPosition.y);
        }
    }

    public bool CheckShouldHide()
    {
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

    private GameObject CreateRow(Transform parent, string name, float yOffset, string label, out TextMeshProUGUI labelTmp, out TextMeshProUGUI valueTmp)
    {
        GameObject rowObj = new GameObject(name, Il2CppType.Of<RectTransform>());
        rowObj.transform.SetParent(parent, false);

        RectTransform rowRt = rowObj.GetComponent<RectTransform>();
        rowRt.anchorMin = new Vector2(0f, 1f);
        rowRt.anchorMax = new Vector2(1f, 1f);
        rowRt.pivot = new Vector2(0.5f, 1f);
        rowRt.anchoredPosition = new Vector2(0f, yOffset);
        rowRt.sizeDelta = new Vector2(0f, 22f);

        // Label (left)
        labelTmp = CreateTMP(rowObj.transform, "Label", label, 11f, FontStyles.Bold, TextAlignmentOptions.Left, ColorLabel);
        RectTransform lblRt = labelTmp.GetComponent<RectTransform>();
        lblRt.anchorMin = Vector2.zero;
        lblRt.anchorMax = Vector2.one;
        lblRt.offsetMin = new Vector2(10f, 0f);
        lblRt.offsetMax = new Vector2(-100f, 0f);

        // Value (right)
        valueTmp = CreateTMP(rowObj.transform, "Value", "— $0", 12f, FontStyles.Bold, TextAlignmentOptions.Right, ColorNeutral);
        RectTransform valRt = valueTmp.GetComponent<RectTransform>();
        valRt.anchorMin = Vector2.zero;
        valRt.anchorMax = Vector2.one;
        valRt.offsetMin = new Vector2(60f, 0f);
        valRt.offsetMax = new Vector2(-10f, 0f);

        return rowObj;
    }

    private static TextMeshProUGUI CreateTMP(Transform parent, string name, string text, float fontSize, FontStyles style, TextAlignmentOptions align, Color color)
    {
        GameObject obj = new GameObject(name, Il2CppType.Of<RectTransform>());
        obj.transform.SetParent(parent, false);

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        var (font, mat) = ProfitTrackerFont.ResolveFont();
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
        return tmp;
    }

    public static string FormatDelta(float delta)
    {
        if (Math.Abs(delta) < 0.01f)
            return "—  $0";

        bool up = delta > 0f;
        string arrow = up ? "▲ +" : "▼ -";
        float abs = Math.Abs(delta);
        return arrow + "$" + abs.ToString("#,0", CultureInfo.InvariantCulture);
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
