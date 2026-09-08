using System;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.GameTime;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI;
using Il2CppScheduleOne.UI.Phone.Map;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using S1Mods.Shared;

namespace Minimap;

public sealed class MinimapHUD
{
    private readonly ModLogger _log;
    private readonly MinimapBlips _blips = new();

    // GameObjects & UI Hierarchy
    private GameObject? _rootCanvasObj;
    private Canvas? _canvas;
    private UnityEngine.UI.CanvasScaler? _scaler;
    private CanvasGroup? _canvasGroup;

    private GameObject? _cardObj;
    private RectTransform? _cardRt;

    // DayCounter Container & TMPs
    private GameObject? _dayHeaderObj;
    private RectTransform? _dayHeaderRt;
    private TextMeshProUGUI? _dayTmp;
    private Image? _dayAccentStrip;

    // Viewport & Masking
    private GameObject? _maskObj;
    private RectTransform? _maskRt;
    private Image? _maskImg;
    private Mask? _maskComp;
    private Image? _bgImg;

    // Map Content
    private GameObject? _mapContainerObj;
    private RectTransform? _mapContainerRt;
    private GameObject? _mapImageObj;
    private RectTransform? _mapImageRt;
    private Image? _mapImg;

    // Compass & Border Overlay
    private GameObject? _compassRingObj;
    private RectTransform? _compassRingRt;
    private Image? _borderImg;
    private TextMeshProUGUI? _northLabel;
    private TextMeshProUGUI? _eastLabel;
    private TextMeshProUGUI? _southLabel;
    private TextMeshProUGUI? _westLabel;

    // Player Marker
    private GameObject? _playerMarkerObj;
    private RectTransform? _playerMarkerRt;
    private Image? _playerMarkerImg;

    // Blip Container
    private GameObject? _blipContainerObj;

    // Cache state to eliminate per-frame allocations
    private int _lastElapsedDays = -999;
    private EDay _lastDayEnum = (EDay)(-1);
    private int _lastCurrentTime = -999;
    private bool _last24H = true;
    private bool _lastShortDay = false;

    // Dragging state
    private bool _isDragging;
    private Vector2 _dragPointerOffset;

    // Cached vanilla sprite
    private Sprite? _vanillaMapSprite;
    private bool _spriteResourcesSearchDone;

    public MinimapHUD(ModLogger log)
    {
        _log = log;
    }

    public bool IsCreated => _rootCanvasObj != null && (UnityEngine.Object)_rootCanvasObj != null;

    public void InvalidateBlipCache()
    {
        _blips.InvalidateLandmarkCache();
    }

    public void InvalidateMapSprite()
    {
        _vanillaMapSprite = null;
    }

    public void EnsureHUD(MinimapConfig config)
    {
        if (IsCreated)
            return;

        try
        {
            MinimapFont.ResetCache();

            // 1. Root Canvas
            _rootCanvasObj = new GameObject("Minimap_HUD_Root");
            UnityEngine.Object.DontDestroyOnLoad(_rootCanvasObj);

            _canvas = _rootCanvasObj.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 32000;

            _scaler = _rootCanvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            _scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _scaler.referenceResolution = new Vector2(1920f, 1080f);
            _scaler.matchWidthOrHeight = 0.5f;

            _canvasGroup = _rootCanvasObj.AddComponent<CanvasGroup>();
            _canvasGroup.blocksRaycasts = config.AllowDragging;
            _canvasGroup.interactable = false;

            // 2. Main Card Root (contains Map + DayCounter)
            _cardObj = new GameObject("Minimap_Card", Il2CppType.Of<RectTransform>());
            _cardObj.transform.SetParent(_rootCanvasObj.transform, false);
            _cardRt = _cardObj.GetComponent<RectTransform>();

            // 3. DayCounter Header Bar
            BuildDayHeader(config);

            // 4. Map Viewport with Mask
            BuildMapView(config);

            // 5. Compass Ring & Border
            BuildCompassAndBorder(config);

            // 6. Center Player Marker
            BuildPlayerMarker();

            // 7. Initialize Blips
            if (_blipContainerObj != null)
            {
                _blips.Initialize(_blipContainerObj.transform);
            }

            // 8. Apply layout positioning
            ApplyLayout(config);
            _rootCanvasObj.SetActive(config.MinimapVisible);

            _log.Info("Minimap HUD created successfully.");
        }
        catch (Exception ex)
        {
            _log.Error($"Error initializing Minimap HUD: {ex}");
        }
    }

    private void BuildDayHeader(MinimapConfig config)
    {
        if (_cardObj == null) return;

        _dayHeaderObj = new GameObject("DayHeader_Bar", Il2CppType.Of<RectTransform>());
        _dayHeaderObj.transform.SetParent(_cardObj.transform, false);
        _dayHeaderRt = _dayHeaderObj.GetComponent<RectTransform>();

        Image headerBg = _dayHeaderObj.AddComponent<Image>();
        headerBg.color = new Color(0.08f, 0.10f, 0.13f, 0.92f);

        // Accent strip on left
        GameObject accentObj = new GameObject("AccentStrip", Il2CppType.Of<RectTransform>());
        accentObj.transform.SetParent(_dayHeaderObj.transform, false);
        RectTransform accentRt = accentObj.GetComponent<RectTransform>();
        accentRt.anchorMin = new Vector2(0f, 0f);
        accentRt.anchorMax = new Vector2(0f, 1f);
        accentRt.pivot = new Vector2(0f, 0.5f);
        accentRt.sizeDelta = new Vector2(3.5f, 0f);
        accentRt.anchoredPosition = Vector2.zero;

        _dayAccentStrip = accentObj.AddComponent<Image>();
        ColorUtility.TryParseHtmlString(config.BorderColorHex, out Color accentCol);
        _dayAccentStrip.color = accentCol;

        // TMP Text label
        var (font, mat) = MinimapFont.ResolveFont();
        GameObject textObj = new GameObject("Day_TMP", Il2CppType.Of<RectTransform>());
        textObj.transform.SetParent(_dayHeaderObj.transform, false);
        RectTransform textRt = textObj.GetComponent<RectTransform>();
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(12f, 1f);
        textRt.offsetMax = new Vector2(-6f, -1f);

        _dayTmp = textObj.AddComponent<TextMeshProUGUI>();
        if (font != null)
        {
            _dayTmp.font = font;
            if (mat != null) _dayTmp.fontSharedMaterial = mat;
        }
        _dayTmp.fontSize = 13.5f;
        _dayTmp.fontStyle = FontStyles.Bold;
        _dayTmp.alignment = TextAlignmentOptions.Center;
        _dayTmp.color = Color.white;
        _dayTmp.text = "DAY 1 │ Tuesday 08:00";
        _dayTmp.raycastTarget = false;
    }

    private void BuildMapView(MinimapConfig config)
    {
        if (_cardObj == null) return;

        // Viewport Mask Object
        _maskObj = new GameObject("Map_Mask", Il2CppType.Of<RectTransform>());
        _maskObj.transform.SetParent(_cardObj.transform, false);
        _maskRt = _maskObj.GetComponent<RectTransform>();

        _maskImg = _maskObj.AddComponent<Image>();
        _maskImg.sprite = GetShapeMaskSprite(config.Shape);
        _maskImg.color = Color.white;
        _maskImg.raycastTarget = true;

        _maskComp = _maskObj.AddComponent<Mask>();
        _maskComp.showMaskGraphic = false;

        // Background Dark Filler
        GameObject bgObj = new GameObject("Map_Background", Il2CppType.Of<RectTransform>());
        bgObj.transform.SetParent(_maskObj.transform, false);
        RectTransform bgRt = bgObj.GetComponent<RectTransform>();
        bgRt.anchorMin = Vector2.zero;
        bgRt.anchorMax = Vector2.one;
        bgRt.sizeDelta = Vector2.zero;

        _bgImg = bgObj.AddComponent<Image>();
        _bgImg.color = new Color(0.06f, 0.08f, 0.10f, config.BackgroundOpacity);
        _bgImg.raycastTarget = false;

        // Map Transform Container (handles rotation & scaling)
        _mapContainerObj = new GameObject("Map_Container", Il2CppType.Of<RectTransform>());
        _mapContainerObj.transform.SetParent(_maskObj.transform, false);
        _mapContainerRt = _mapContainerObj.GetComponent<RectTransform>();
        _mapContainerRt.anchorMin = new Vector2(0.5f, 0.5f);
        _mapContainerRt.anchorMax = new Vector2(0.5f, 0.5f);
        _mapContainerRt.pivot = new Vector2(0.5f, 0.5f);
        _mapContainerRt.anchoredPosition = Vector2.zero;

        // Map Image (actual map texture)
        _mapImageObj = new GameObject("Map_Image", Il2CppType.Of<RectTransform>());
        _mapImageObj.transform.SetParent(_mapContainerObj.transform, false);
        _mapImageRt = _mapImageObj.GetComponent<RectTransform>();
        _mapImageRt.anchorMin = new Vector2(0.5f, 0.5f);
        _mapImageRt.anchorMax = new Vector2(0.5f, 0.5f);
        _mapImageRt.pivot = new Vector2(0.5f, 0.5f);

        _mapImg = _mapImageObj.AddComponent<Image>();
        _mapImg.raycastTarget = false;

        TryResolveMapSprite();

        // Blip Container (sits above map texture)
        _blipContainerObj = new GameObject("Blips_Container", Il2CppType.Of<RectTransform>());
        _blipContainerObj.transform.SetParent(_maskObj.transform, false);
        RectTransform blipRt = _blipContainerObj.GetComponent<RectTransform>();
        blipRt.anchorMin = new Vector2(0.5f, 0.5f);
        blipRt.anchorMax = new Vector2(0.5f, 0.5f);
        blipRt.pivot = new Vector2(0.5f, 0.5f);
        blipRt.anchoredPosition = Vector2.zero;
    }

    private void BuildCompassAndBorder(MinimapConfig config)
    {
        if (_cardObj == null) return;

        // Outer Border & Rim
        GameObject borderObj = new GameObject("Map_Border", Il2CppType.Of<RectTransform>());
        borderObj.transform.SetParent(_cardObj.transform, false);
        RectTransform borderRt = borderObj.GetComponent<RectTransform>();
        borderRt.anchorMin = new Vector2(0.5f, 0.5f);
        borderRt.anchorMax = new Vector2(0.5f, 0.5f);
        borderRt.pivot = new Vector2(0.5f, 0.5f);

        _borderImg = borderObj.AddComponent<Image>();
        _borderImg.sprite = GetShapeBorderSprite(config.Shape);
        ColorUtility.TryParseHtmlString(config.BorderColorHex, out Color borderCol);
        _borderImg.color = borderCol;
        _borderImg.raycastTarget = false;

        // Compass Ring with Cardinal Points (only active in Circle mode)
        _compassRingObj = new GameObject("Compass_Ring", Il2CppType.Of<RectTransform>());
        _compassRingObj.transform.SetParent(_cardObj.transform, false);
        _compassRingRt = _compassRingObj.GetComponent<RectTransform>();
        _compassRingRt.anchorMin = new Vector2(0.5f, 0.5f);
        _compassRingRt.anchorMax = new Vector2(0.5f, 0.5f);
        _compassRingRt.pivot = new Vector2(0.5f, 0.5f);

        var (font, mat) = MinimapFont.ResolveFont();

        _northLabel = CreateCardinalLabel(_compassRingObj.transform, "N", new Vector2(0.5f, 1f), new Vector2(0f, -12f), new Color(0.95f, 0.30f, 0.30f, 1f), font, mat);
        _eastLabel = CreateCardinalLabel(_compassRingObj.transform, "E", new Vector2(1f, 0.5f), new Vector2(-12f, 0f), Color.white, font, mat);
        _southLabel = CreateCardinalLabel(_compassRingObj.transform, "S", new Vector2(0.5f, 0f), new Vector2(0f, 12f), Color.white, font, mat);
        _westLabel = CreateCardinalLabel(_compassRingObj.transform, "W", new Vector2(0f, 0.5f), new Vector2(12f, 0f), Color.white, font, mat);

        _compassRingObj.SetActive(config.ShowCompassRing && config.Shape == MinimapShape.Circle);
    }

    private TextMeshProUGUI CreateCardinalLabel(Transform parent, string text, Vector2 anchor, Vector2 offset, Color color, TMP_FontAsset? font, Material? mat)
    {
        GameObject obj = new GameObject($"Label_{text}", Il2CppType.Of<RectTransform>());
        obj.transform.SetParent(parent, false);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = offset;
        rt.sizeDelta = new Vector2(18f, 18f);

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        if (font != null)
        {
            tmp.font = font;
            if (mat != null) tmp.fontSharedMaterial = mat;
        }
        tmp.fontSize = 11f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = color;
        tmp.text = text;
        tmp.raycastTarget = false;
        return tmp;
    }

    private void BuildPlayerMarker()
    {
        if (_cardObj == null) return;

        _playerMarkerObj = new GameObject("Player_Marker", Il2CppType.Of<RectTransform>());
        _playerMarkerObj.transform.SetParent(_cardObj.transform, false);
        _playerMarkerRt = _playerMarkerObj.GetComponent<RectTransform>();
        _playerMarkerRt.anchorMin = new Vector2(0.5f, 0.5f);
        _playerMarkerRt.anchorMax = new Vector2(0.5f, 0.5f);
        _playerMarkerRt.pivot = new Vector2(0.5f, 0.5f);
        _playerMarkerRt.anchoredPosition = Vector2.zero;
        _playerMarkerRt.sizeDelta = new Vector2(22f, 22f);

        _playerMarkerImg = _playerMarkerObj.AddComponent<Image>();
        _playerMarkerImg.sprite = MinimapTextures.GetPlayerArrow();
        _playerMarkerImg.raycastTarget = false;
    }

    private Sprite GetShapeMaskSprite(MinimapShape shape) => shape switch
    {
        MinimapShape.Circle => MinimapTextures.GetCircleMask(),
        MinimapShape.RoundedSquare => MinimapTextures.GetRoundedSquareMask(256, 24f),
        MinimapShape.Square => MinimapTextures.GetRoundedSquareMask(256, 4f),
        _ => MinimapTextures.GetRoundedSquareMask(256, 24f)
    };

    private Sprite GetShapeBorderSprite(MinimapShape shape) => shape switch
    {
        MinimapShape.Circle => MinimapTextures.GetCircleBorder(),
        MinimapShape.RoundedSquare => MinimapTextures.GetRoundedSquareBorder(256, 24f, 4f),
        MinimapShape.Square => MinimapTextures.GetRoundedSquareBorder(256, 4f, 4f),
        _ => MinimapTextures.GetRoundedSquareBorder(256, 24f, 4f)
    };

    private void TryResolveMapSprite()
    {
        if (_mapImg == null) return;

        if (_vanillaMapSprite != null && _vanillaMapSprite.Pointer != IntPtr.Zero && !_vanillaMapSprite.WasCollected)
        {
            _mapImg.sprite = _vanillaMapSprite;
            return;
        }

        // Try extracting from MapApp if instantiated
        try
        {
            if (MapApp.Instance != null && (UnityEngine.Object)MapApp.Instance != null)
            {
                var sprite = MapApp.Instance.MainMapSprite;
                if (sprite != null && sprite.Pointer != IntPtr.Zero)
                {
                    _vanillaMapSprite = sprite;
                    _mapImg.sprite = _vanillaMapSprite;
                    if (_mapImageRt != null)
                    {
                        float mapDim = sprite.rect.width;
                        if (MapPositionUtility.Instance != null && (UnityEngine.Object)MapPositionUtility.Instance != null)
                        {
                            if (MapPositionUtility.Instance.MapDimensions > 0)
                            {
                                mapDim = MapPositionUtility.Instance.MapDimensions;
                            }
                        }
                        _mapImageRt.sizeDelta = new Vector2(mapDim, mapDim);
                        _log.Info($"Resolved vanilla map sprite from MapApp. MapDim={mapDim}");
                    }
                    return;
                }
            }
        }
        catch { }

        // Fallback: search loaded Sprite assets (IL2CPP-safe for loop) — one-shot guard.
        // This is the EXPECTED startup path while the vanilla MapApp is not instantiated yet,
        // so progression notes are Debug; only a failed scan (below) stays a Warning.
        if (_spriteResourcesSearchDone)
        {
            _log.Debug("TryResolveMapSprite: skipping Resources.FindObjectsOfTypeAll<Sprite> — previous scan already failed (one-shot guard).");
            return;
        }
        _log.Debug("TryResolveMapSprite: MapApp sprite not available — falling back to Resources.FindObjectsOfTypeAll<Sprite> scan (one-shot).");
        try
        {
            var sprites = Resources.FindObjectsOfTypeAll<Sprite>();
            if (sprites != null)
            {
                for (int si = 0; si < sprites.Length; si++)
                {
                    var s = sprites[si];
                    if (s == null || s.Pointer == IntPtr.Zero || s.WasCollected) continue;
                    if (s.name.Contains("MainMap", StringComparison.OrdinalIgnoreCase) || s.name.Contains("Map_Full", StringComparison.OrdinalIgnoreCase) || s.name.Contains("MapSprite", StringComparison.OrdinalIgnoreCase))
                    {
                        _vanillaMapSprite = s;
                        _mapImg.sprite = _vanillaMapSprite;
                        if (_mapImageRt != null)
                        {
                            float mapDim = s.rect.width;
                            if (MapPositionUtility.Instance != null && (UnityEngine.Object)MapPositionUtility.Instance != null)
                            {
                                if (MapPositionUtility.Instance.MapDimensions > 0)
                                {
                                    mapDim = MapPositionUtility.Instance.MapDimensions;
                                }
                            }
                            _mapImageRt.sizeDelta = new Vector2(mapDim, mapDim);
                            _log.Info($"Resolved map sprite from Resources: '{s.name}'. MapDim={mapDim}");
                        }
                        _spriteResourcesSearchDone = true;
                        return;
                    }
                }
            }
        }
        catch { }
        _spriteResourcesSearchDone = true;
        _log.Warn("TryResolveMapSprite: Resources fallback found no matching sprite — future calls will skip Resources scan (one-shot guard).");
    }

    public void ApplyLayout(MinimapConfig config)
    {
        if (_cardRt == null || _maskRt == null) return;

        float size = config.MapSize;
        float headerHeight = 26f;
        bool hasHeader = config.DayMode == DayCounterMode.Header;
        bool hasFooter = config.DayMode == DayCounterMode.Footer;
        bool isStandalone = config.DayMode == DayCounterMode.Standalone;
        float totalHeight = size + (hasHeader || hasFooter ? headerHeight : 0f);

        if (isStandalone)
            _cardRt.sizeDelta = new Vector2(size, headerHeight);
        else
            _cardRt.sizeDelta = new Vector2(size, totalHeight);

        // Position Anchors
        if (config.HasCustomPosition)
        {
            _cardRt.anchorMin = new Vector2(0.5f, 0.5f);
            _cardRt.anchorMax = new Vector2(0.5f, 0.5f);
            _cardRt.pivot = new Vector2(0.5f, 0.5f);
            _cardRt.anchoredPosition = new Vector2(config.CustomPosX, config.CustomPosY);
        }
        else
        {
            switch (config.AnchorPosition)
            {
                case MinimapAnchor.TopRight:
                    _cardRt.anchorMin = new Vector2(1f, 1f);
                    _cardRt.anchorMax = new Vector2(1f, 1f);
                    _cardRt.pivot = new Vector2(1f, 1f);
                    _cardRt.anchoredPosition = new Vector2(-25f, -25f);
                    break;
                case MinimapAnchor.TopLeft:
                    _cardRt.anchorMin = new Vector2(0f, 1f);
                    _cardRt.anchorMax = new Vector2(0f, 1f);
                    _cardRt.pivot = new Vector2(0f, 1f);
                    _cardRt.anchoredPosition = new Vector2(25f, -25f);
                    break;
                case MinimapAnchor.BottomRight:
                    _cardRt.anchorMin = new Vector2(1f, 0f);
                    _cardRt.anchorMax = new Vector2(1f, 0f);
                    _cardRt.pivot = new Vector2(1f, 0f);
                    _cardRt.anchoredPosition = new Vector2(-25f, 25f);
                    break;
                case MinimapAnchor.BottomLeft:
                    _cardRt.anchorMin = new Vector2(0f, 0f);
                    _cardRt.anchorMax = new Vector2(0f, 0f);
                    _cardRt.pivot = new Vector2(0f, 0f);
                    _cardRt.anchoredPosition = new Vector2(25f, 25f);
                    break;
            }
        }

        // Viewport layout for Square / RoundedSquare
        if (config.Shape != MinimapShape.Circle)
        {
            if (isStandalone)
            {
                if (_dayHeaderObj != null && _dayHeaderRt != null)
                {
                    _dayHeaderObj.SetActive(true);
                    _dayHeaderRt.anchorMin = new Vector2(0f, 0f);
                    _dayHeaderRt.anchorMax = new Vector2(1f, 1f);
                    _dayHeaderRt.pivot = new Vector2(0.5f, 0.5f);
                    _dayHeaderRt.anchoredPosition = Vector2.zero;
                    _dayHeaderRt.sizeDelta = Vector2.zero;
                }
                if (_maskObj != null) _maskObj.SetActive(false);
                if (_borderImg != null) _borderImg.gameObject.SetActive(false);
            }
            else if (hasHeader)
            {
                if (_dayHeaderObj != null && _dayHeaderRt != null)
                {
                    _dayHeaderObj.SetActive(true);
                    _dayHeaderRt.anchorMin = new Vector2(0f, 1f);
                    _dayHeaderRt.anchorMax = new Vector2(1f, 1f);
                    _dayHeaderRt.pivot = new Vector2(0.5f, 1f);
                    _dayHeaderRt.anchoredPosition = Vector2.zero;
                    _dayHeaderRt.sizeDelta = new Vector2(0f, headerHeight);
                }
                if (_maskObj != null) _maskObj.SetActive(true);
                if (_borderImg != null) _borderImg.gameObject.SetActive(true);

                _maskRt.anchorMin = new Vector2(0f, 0f);
                _maskRt.anchorMax = new Vector2(1f, 1f);
                _maskRt.offsetMin = new Vector2(0f, 0f);
                _maskRt.offsetMax = new Vector2(0f, -headerHeight);
            }
            else if (hasFooter)
            {
                if (_dayHeaderObj != null && _dayHeaderRt != null)
                {
                    _dayHeaderObj.SetActive(true);
                    _dayHeaderRt.anchorMin = new Vector2(0f, 0f);
                    _dayHeaderRt.anchorMax = new Vector2(1f, 0f);
                    _dayHeaderRt.pivot = new Vector2(0.5f, 0f);
                    _dayHeaderRt.anchoredPosition = Vector2.zero;
                    _dayHeaderRt.sizeDelta = new Vector2(0f, headerHeight);
                }
                if (_maskObj != null) _maskObj.SetActive(true);
                if (_borderImg != null) _borderImg.gameObject.SetActive(true);

                _maskRt.anchorMin = new Vector2(0f, 0f);
                _maskRt.anchorMax = new Vector2(1f, 1f);
                _maskRt.offsetMin = new Vector2(0f, headerHeight);
                _maskRt.offsetMax = new Vector2(0f, 0f);
            }
            else
            {
                if (_dayHeaderObj != null) _dayHeaderObj.SetActive(false);
                if (_maskObj != null) _maskObj.SetActive(true);
                if (_borderImg != null) _borderImg.gameObject.SetActive(true);
                _maskRt.anchorMin = Vector2.zero;
                _maskRt.anchorMax = Vector2.one;
                _maskRt.offsetMin = Vector2.zero;
                _maskRt.offsetMax = Vector2.zero;
            }

            if (_borderImg != null)
            {
                _borderImg.rectTransform.anchorMin = _maskRt.anchorMin;
                _borderImg.rectTransform.anchorMax = _maskRt.anchorMax;
                _borderImg.rectTransform.offsetMin = _maskRt.offsetMin;
                _borderImg.rectTransform.offsetMax = _maskRt.offsetMax;
            }

            if (_compassRingObj != null) _compassRingObj.SetActive(false);
        }
        else // Circular Mode
        {
            _maskRt.sizeDelta = new Vector2(size, size);
            _maskRt.anchorMin = new Vector2(0.5f, 0.5f);
            _maskRt.anchorMax = new Vector2(0.5f, 0.5f);
            _maskRt.pivot = new Vector2(0.5f, 0.5f);
            _maskRt.anchoredPosition = Vector2.zero;

            if (_dayHeaderObj != null && _dayHeaderRt != null)
            {
                _dayHeaderObj.SetActive(hasHeader || hasFooter);
                _dayHeaderRt.anchorMin = new Vector2(0.5f, hasHeader ? 1f : 0f);
                _dayHeaderRt.anchorMax = new Vector2(0.5f, hasHeader ? 1f : 0f);
                _dayHeaderRt.pivot = new Vector2(0.5f, hasHeader ? 0f : 1f);
                _dayHeaderRt.anchoredPosition = new Vector2(0f, hasHeader ? 8f : -8f);
                _dayHeaderRt.sizeDelta = new Vector2(size * 0.90f, headerHeight);
            }

            if (_borderImg != null)
            {
                _borderImg.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                _borderImg.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                _borderImg.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                _borderImg.rectTransform.sizeDelta = new Vector2(size, size);
                _borderImg.rectTransform.anchoredPosition = Vector2.zero;
            }

            if (_compassRingObj != null)
            {
                _compassRingObj.SetActive(config.ShowCompassRing);
                if (_compassRingRt != null) _compassRingRt.sizeDelta = new Vector2(size, size);
            }
        }

        // Apply Shape Sprites
        if (_maskImg != null) _maskImg.sprite = GetShapeMaskSprite(config.Shape);
        if (_borderImg != null) _borderImg.sprite = GetShapeBorderSprite(config.Shape);

        if (_bgImg != null)
            _bgImg.color = new Color(0.06f, 0.08f, 0.10f, config.BackgroundOpacity);

        ColorUtility.TryParseHtmlString(config.BorderColorHex, out Color accentCol);
        if (_borderImg != null) _borderImg.color = accentCol;
        if (_dayAccentStrip != null) _dayAccentStrip.color = accentCol;
    }

    public void Update(MinimapConfig config, float now)
    {
        if (!IsCreated || !_rootCanvasObj!.activeSelf)
            return;

        // 1. Update DayCounter text
        UpdateDayCounterText(config);

        // 2. Resolve Player position & rotation
        Player localPlayer = Player.Local;
        if (localPlayer == null || (UnityEngine.Object)localPlayer == null)
            return;

        Vector3 playerPos = localPlayer.transform.position;
        float playerYaw = localPlayer.transform.eulerAngles.y;

        // 3. Resolve Map coordinate
        Vector2 playerMapPos = Vector2.zero;
        if (MapPositionUtility.Instance != null && (UnityEngine.Object)MapPositionUtility.Instance != null)
        {
            playerMapPos = MapPositionUtility.Instance.GetMapPosition(playerPos);
        }
        else
        {
            playerMapPos = new Vector2(playerPos.x * 2.5f, playerPos.z * 2.5f);
        }

        // Lazy sprite check
        if (_vanillaMapSprite == null || (UnityEngine.Object)_vanillaMapSprite == null)
        {
            TryResolveMapSprite();
        }

        // 4. Update Map Position & Rotation
        float zoom = config.ZoomLevel;
        if (_mapImageRt != null)
        {
            _mapImageRt.anchoredPosition = -playerMapPos * zoom;
            _mapImageRt.localScale = new Vector3(zoom, zoom, 1f);
        }

        if (config.RotationMode == MinimapRotation.FollowPlayer)
        {
            if (_mapContainerRt != null)
                _mapContainerRt.localRotation = Quaternion.Euler(0f, 0f, playerYaw);

            if (_compassRingRt != null)
                _compassRingRt.localRotation = Quaternion.Euler(0f, 0f, playerYaw);

            if (_playerMarkerRt != null)
                _playerMarkerRt.localRotation = Quaternion.identity; // Points up
        }
        else // North-Up
        {
            if (_mapContainerRt != null)
                _mapContainerRt.localRotation = Quaternion.identity;

            if (_compassRingRt != null)
                _compassRingRt.localRotation = Quaternion.identity;

            if (_playerMarkerRt != null)
                _playerMarkerRt.localRotation = Quaternion.Euler(0f, 0f, -playerYaw);
        }

        // 5. Update & Render Blips (Optimized Scan)
        _blips.UpdateEntities(config, now, playerPos);
        _blips.RenderBlips(playerPos, playerYaw, playerMapPos, config, config.MapSize * 0.5f, zoom);

        // 6. Handle Drag & Drop
        HandleDragging(config);
    }

    private void UpdateDayCounterText(MinimapConfig config)
    {
        if (_dayTmp == null || config.DayMode == DayCounterMode.Hidden)
            return;

        int elapsedDays = 0;
        EDay dayEnum = EDay.Monday;
        int currentTime = 0;

        if (TimeManager.Instance != null && (UnityEngine.Object)TimeManager.Instance != null)
        {
            try
            {
                elapsedDays = TimeManager.Instance.ElapsedDays;
                dayEnum = TimeManager.Instance.CurrentDay;
                currentTime = TimeManager.Instance.CurrentTime;
            }
            catch { }
        }

        if (elapsedDays == _lastElapsedDays &&
            dayEnum == _lastDayEnum &&
            currentTime == _lastCurrentTime &&
            config.Time24H == _last24H &&
            config.ShortDayName == _lastShortDay)
        {
            return; // 0-allocation skip
        }

        _lastElapsedDays = elapsedDays;
        _lastDayEnum = dayEnum;
        _lastCurrentTime = currentTime;
        _last24H = config.Time24H;
        _lastShortDay = config.ShortDayName;

        string dayName = config.ShortDayName ? GetShortDayName(dayEnum) : dayEnum.ToString();
        string timeStr = FormatTime(currentTime, config.Time24H);

        if (config.ShowDayNumber && config.ShowDayName && config.ShowTime)
            _dayTmp.text = $"DAY {elapsedDays} │ {dayName} {timeStr}";
        else if (config.ShowDayNumber && config.ShowTime)
            _dayTmp.text = $"DAY {elapsedDays} │ {timeStr}";
        else if (config.ShowDayName && config.ShowTime)
            _dayTmp.text = $"{dayName} {timeStr}";
        else if (config.ShowDayNumber)
            _dayTmp.text = $"DAY {elapsedDays}";
        else
            _dayTmp.text = timeStr;
    }

    private static string FormatTime(int currentTime, bool use24H)
    {
        if (use24H)
        {
            float f = currentTime;
            int hours = Mathf.FloorToInt(f / 100f);
            int mins = Mathf.FloorToInt(f - hours * 100f);
            return $"{hours:D2}:{mins:D2}";
        }
        try
        {
            return TimeManager.Get12HourTime(currentTime, true);
        }
        catch
        {
            int hours = (currentTime / 100) % 24;
            int mins = currentTime % 100;
            string ampm = hours >= 12 ? "PM" : "AM";
            int h12 = hours % 12;
            if (h12 == 0) h12 = 12;
            return $"{h12}:{mins:D2} {ampm}";
        }
    }

    private static string GetShortDayName(EDay day) => day switch
    {
        EDay.Monday => "Mon",
        EDay.Tuesday => "Tue",
        EDay.Wednesday => "Wed",
        EDay.Thursday => "Thu",
        EDay.Friday => "Fri",
        EDay.Saturday => "Sat",
        EDay.Sunday => "Sun",
        _ => "Day"
    };

    private void HandleDragging(MinimapConfig config)
    {
        if (!config.AllowDragging || _cardRt == null)
            return;

        bool cursorUnlocked = Cursor.visible && Cursor.lockState != CursorLockMode.Locked;
        if (!cursorUnlocked)
        {
            _isDragging = false;
            return;
        }

        Vector2 mousePos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_cardRt, mousePos, null))
            {
                _isDragging = true;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_cardRt, mousePos, null, out _dragPointerOffset);
            }
        }
        else if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            _isDragging = false;
            config.HasCustomPosition = true;
            config.CustomPosX = _cardRt.anchoredPosition.x;
            config.CustomPosY = _cardRt.anchoredPosition.y;
            MinimapMod.Instance?.SaveConfig();
        }

        if (_isDragging && _rootCanvasObj != null)
        {
            RectTransform canvasRt = _rootCanvasObj.GetComponent<RectTransform>();
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRt, mousePos, null, out Vector2 canvasLocalPos))
            {
                _cardRt.anchorMin = new Vector2(0.5f, 0.5f);
                _cardRt.anchorMax = new Vector2(0.5f, 0.5f);
                _cardRt.pivot = new Vector2(0.5f, 0.5f);

                Vector2 targetPos = canvasLocalPos - _dragPointerOffset;

                // Screen Boundary Clamping
                float halfW = canvasRt.rect.width * 0.5f - _cardRt.rect.width * 0.5f;
                float halfH = canvasRt.rect.height * 0.5f - _cardRt.rect.height * 0.5f;

                targetPos.x = Mathf.Clamp(targetPos.x, -halfW, halfW);
                targetPos.y = Mathf.Clamp(targetPos.y, -halfH, halfH);

                _cardRt.anchoredPosition = targetPos;
            }
        }
    }

    public void SetHUDActive(bool active)
    {
        if (_rootCanvasObj != null)
        {
            _rootCanvasObj.SetActive(active);
        }
    }

    public void ToggleVisibility(MinimapConfig config)
    {
        config.MinimapVisible = !config.MinimapVisible;
        SetHUDActive(config.MinimapVisible);
    }

    public void AdjustZoom(MinimapConfig config, float delta)
    {
        config.ZoomLevel = Mathf.Clamp(config.ZoomLevel + delta, 0.75f, 4.0f);
    }

    public void DestroyHUD()
    {
        if (_rootCanvasObj != null)
        {
            UnityEngine.Object.Destroy(_rootCanvasObj);
            _rootCanvasObj = null;
        }
    }
}
