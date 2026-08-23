using System;
using System.IO;
using MelonLoader;
using MelonLoader.Utils;
using PocketShop.Config;
using PocketShop.Services;
using PocketShop.UI;
using S1Mods.Shared;
using UITheme = PocketShop.UI.UITheme;
using S1API.PhoneApp;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PocketShop;

public enum ViewMode
{
    Directory,
    ShopDetail
}

/// <summary>
/// PocketShop PhoneApp for Schedule I (v0.2.0).
/// Features:
///   - 2-Level Navigation (Store Directory & Item Grid)
///   - Multi-Payment Switcher (Cash, Bank Card, Auto)
///   - Rich Item Inspection Modal with Quick Quantity Chips (+1, +5, +10, MAX)
///   - In-App Animated Toast Notifications
///   - Native & Procedural Audio Feedback
/// </summary>
public sealed class PocketShopApp : PhoneApp
{
    protected override string AppName => "PocketShop";
    protected override string AppTitle => "Pocket Shop";
    protected override string IconLabel => "Shop";
    protected override string IconFileName => "pocketshop_icon.png";
    protected override EOrientation Orientation => EOrientation.Horizontal;

    private static Sprite? _cachedIcon;

    protected override Sprite IconSprite => BuildIconSprite();

    private GameObject _mainBG = null!;
    private StoreCatalogPane _directoryPane = null!;
    private ItemGridPane _gridPane = null!;
    private ItemDetailModal _detailModal = null!;
    private Text _statsLabel = null!;

    // Payment Switcher Chips
    private Image _cashChipBg = null!;
    private Text _cashChipText = null!;
    private Image _bankChipBg = null!;
    private Text _bankChipText = null!;
    private Image _autoChipBg = null!;
    private Text _autoChipText = null!;

    private ViewMode _viewMode = ViewMode.Directory;
    private int _activeShopIndex;
    private float _lastStatsRefreshTime;

    protected override void OnCreated()
    {
        MelonEvents.OnUpdate.Unsubscribe(Update);
        MelonEvents.OnUpdate.Subscribe(Update);
        base.OnCreated();
        MelonLogger.Msg("Registered with S1API PhoneApp system (v0.2.1).");
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (_mainBG != null) _mainBG.SetActive(false);
        _detailModal?.Hide();
        _viewMode = ViewMode.Directory;
        ShopCatalog.ResetForSceneReload();
        // Update bleibt lebenslang subscribed (defensives Unsubscribe-Subscribe in OnCreated).
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
        }

        if (open)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackClicked();
            }

            if (Time.unscaledTime - _lastStatsRefreshTime > 1.0f)
            {
                _lastStatsRefreshTime = Time.unscaledTime;
                RefreshStats();
            }
        }
    }

    private void OnAppOpened()
    {
        ShopCatalog.Refresh();
        ShopCatalog.RetryIfEmpty();
        SetViewMode(ViewMode.Directory);
        RefreshStats();
        UpdatePaymentChipVisuals();
    }

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.Initialize(containerRt);
        }

        // Isolated background panel covering phone screen (inactive until opened)
        _mainBG = S1API.UI.UIFactory.Panel("PocketShopBG", container.transform, new Color(0.06f, 0.07f, 0.09f, 1f), fullAnchor: true);
        _mainBG.SetActive(false);

        var rootVlg = _mainBG.AddComponent<VerticalLayoutGroup>();
        rootVlg.childControlWidth = true;
        rootVlg.childControlHeight = true;
        rootVlg.childForceExpandWidth = true;
        rootVlg.childForceExpandHeight = false;
        rootVlg.spacing = 0f;
        rootVlg.padding = new RectOffset(0, 0, 0, 0);

        // 1. Header: Back | PocketShop Title | X
        AppHeaderBuilder.Build(_mainBG.transform, OnBackClicked, () => CloseApp());

        // 2. SubHeader Bar: (Left: Store/Item Count) | (Right: Interactive Payment Chips)
        var subHeaderPanel = S1API.UI.UIFactory.Panel("SubHeaderBar", _mainBG.transform, new Color(0.08f, 0.10f, 0.14f, 1f));
        var shLE = subHeaderPanel.AddComponent<LayoutElement>();
        shLE.minHeight = UITheme.Dp(30f);
        shLE.preferredHeight = UITheme.Dp(30f);
        shLE.flexibleHeight = 0f;

        var shHlg = subHeaderPanel.AddComponent<HorizontalLayoutGroup>();
        shHlg.spacing = UITheme.Dp(6f);
        shHlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), (int)UITheme.Dp(2f), (int)UITheme.Dp(2f));
        shHlg.childControlWidth = true;
        shHlg.childControlHeight = true;
        shHlg.childForceExpandWidth = false;
        shHlg.childForceExpandHeight = true;
        shHlg.childAlignment = TextAnchor.MiddleLeft;

        // SubHeader Left Title (Directory title or Shop Name)
        _statsLabel = S1API.UI.UIFactory.Text("SubHeaderTitle", "CHOOSE A SHOP · 12 STORES", subHeaderPanel.transform, UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold | FontStyle.Italic);
        _statsLabel.color = new Color(0.55f, 0.70f, 0.85f, 1f);
        _statsLabel.raycastTarget = false;
        _statsLabel.horizontalOverflow = HorizontalWrapMode.Overflow;
        var statsLE = _statsLabel.gameObject.AddComponent<LayoutElement>();
        statsLE.minWidth = UITheme.Dp(160f);
        statsLE.preferredWidth = UITheme.Dp(210f);

        // Flexible Spacer
        var shSpacer = S1API.UI.UIFactory.Panel("Spacer", subHeaderPanel.transform, Color.clear);
        shSpacer.AddComponent<LayoutElement>().flexibleWidth = 1f;

        // Payment Chips: [💵 Cash $X] [💳 Bank $Y] [⚡ Auto]
        BuildPaymentChips(subHeaderPanel.transform);

        // 3. Main Content Area
        var contentPanel = S1API.UI.UIFactory.Panel("ContentArea", _mainBG.transform, new Color(0.06f, 0.07f, 0.09f, 0.95f));
        var contentLE = contentPanel.AddComponent<LayoutElement>();
        contentLE.flexibleHeight = 1f;
        contentLE.flexibleWidth = 1f;

        var contentRt = (RectTransform)contentPanel.transform;

        // Level 1: Directory Pane (4x3 Grid)
        _directoryPane = new StoreCatalogPane(contentRt);
        _directoryPane.OnShopSelected += OnStoreCardSelected;

        // Level 2: Item Grid Pane (5xN Grid)
        _gridPane = new ItemGridPane(contentRt);
        _gridPane.OnPurchaseResult += HandlePurchaseResult;
        _gridPane.OnInspectRequested += itm => _detailModal?.Show(itm);

        // Item Detail Inspection Modal (Overlay)
        _detailModal = new ItemDetailModal((RectTransform)_mainBG.transform);
        _detailModal.OnPurchaseResult += HandlePurchaseResult;

        // Footer: Version
        FooterBuilder.Build(_mainBG.transform);
    }

    private void BuildPaymentChips(Transform parent)
    {
        // 1. Cash Chip
        var cashPanel = S1API.UI.UIFactory.Panel("CashChip", parent, new Color(0.12f, 0.16f, 0.22f, 1f));
        _cashChipBg = cashPanel.GetComponent<Image>() ?? cashPanel.AddComponent<Image>();
        var cashLE = cashPanel.AddComponent<LayoutElement>();
        cashLE.preferredWidth = UITheme.Dp(105f);
        cashLE.minWidth = UITheme.Dp(90f);
        cashLE.preferredHeight = UITheme.Dp(24f);
        var cashVlg = cashPanel.AddComponent<VerticalLayoutGroup>();
        cashVlg.childControlWidth = true;
        cashVlg.childControlHeight = true;
        cashVlg.childForceExpandWidth = true;
        cashVlg.childForceExpandHeight = true;
        cashVlg.childAlignment = TextAnchor.MiddleCenter;
        _cashChipText = S1API.UI.UIFactory.Text("Txt", "💵 $—", cashPanel.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _cashChipText.color = new Color(0.35f, 0.92f, 0.58f, 1f);
        _cashChipText.raycastTarget = false;
        var cashBtn = cashPanel.AddComponent<Button>();
        cashBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(cashBtn, () => SetPaymentMode(PaymentMode.Cash));

        // 2. Bank Chip
        var bankPanel = S1API.UI.UIFactory.Panel("BankChip", parent, new Color(0.12f, 0.16f, 0.22f, 1f));
        _bankChipBg = bankPanel.GetComponent<Image>() ?? bankPanel.AddComponent<Image>();
        var bankLE = bankPanel.AddComponent<LayoutElement>();
        bankLE.preferredWidth = UITheme.Dp(110f);
        bankLE.minWidth = UITheme.Dp(95f);
        bankLE.preferredHeight = UITheme.Dp(24f);
        var bankVlg = bankPanel.AddComponent<VerticalLayoutGroup>();
        bankVlg.childControlWidth = true;
        bankVlg.childControlHeight = true;
        bankVlg.childForceExpandWidth = true;
        bankVlg.childForceExpandHeight = true;
        bankVlg.childAlignment = TextAnchor.MiddleCenter;
        _bankChipText = S1API.UI.UIFactory.Text("Txt", "💳 $—", bankPanel.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _bankChipText.color = new Color(0.30f, 0.85f, 0.95f, 1f);
        _bankChipText.raycastTarget = false;
        var bankBtn = bankPanel.AddComponent<Button>();
        bankBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(bankBtn, () => SetPaymentMode(PaymentMode.Bank));

        // 3. Auto Chip
        var autoPanel = S1API.UI.UIFactory.Panel("AutoChip", parent, new Color(0.12f, 0.16f, 0.22f, 1f));
        _autoChipBg = autoPanel.GetComponent<Image>() ?? autoPanel.AddComponent<Image>();
        var autoLE = autoPanel.AddComponent<LayoutElement>();
        autoLE.preferredWidth = UITheme.Dp(55f);
        autoLE.preferredHeight = UITheme.Dp(24f);
        var autoVlg = autoPanel.AddComponent<VerticalLayoutGroup>();
        autoVlg.childControlWidth = true;
        autoVlg.childControlHeight = true;
        autoVlg.childForceExpandWidth = true;
        autoVlg.childForceExpandHeight = true;
        autoVlg.childAlignment = TextAnchor.MiddleCenter;
        _autoChipText = S1API.UI.UIFactory.Text("Txt", "⚡ AUTO", autoPanel.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _autoChipText.color = new Color(0.96f, 0.77f, 0.26f, 1f);
        _autoChipText.raycastTarget = false;
        var autoBtn = autoPanel.AddComponent<Button>();
        autoBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(autoBtn, () => SetPaymentMode(PaymentMode.Auto));

        UpdatePaymentChipVisuals();
    }

    private void SetPaymentMode(PaymentMode mode)
    {
        SoundService.PlayButtonClick();
        PocketShopConfig.PaymentModeStatic = mode;
        UpdatePaymentChipVisuals();

        string label = mode switch
        {
            PaymentMode.Cash => "💵 Cash (Physical)",
            PaymentMode.Bank => "💳 Bank Card (Online)",
            PaymentMode.Auto => "⚡ Auto (Cash first, Bank fallback)",
            _ => mode.ToString()
        };

        _gridPane?.RefreshAllBuyStates();
        _detailModal?.RefreshDisplay();
    }

    private void UpdatePaymentChipVisuals()
    {
        if (_cashChipBg == null || _bankChipBg == null || _autoChipBg == null) return;

        var activeMode = PocketShopConfig.PaymentModeStatic;

        // Active style: brighter background & vivid text
        _cashChipBg.color = activeMode == PaymentMode.Cash
            ? new Color(0.15f, 0.35f, 0.22f, 1f)
            : new Color(0.10f, 0.12f, 0.16f, 0.85f);

        _bankChipBg.color = activeMode == PaymentMode.Bank
            ? new Color(0.12f, 0.30f, 0.45f, 1f)
            : new Color(0.10f, 0.12f, 0.16f, 0.85f);

        _autoChipBg.color = activeMode == PaymentMode.Auto
            ? new Color(0.38f, 0.28f, 0.10f, 1f)
            : new Color(0.10f, 0.12f, 0.16f, 0.85f);
    }

    private void HandlePurchaseResult(PurchaseResultData result)
    {
        RefreshStats();
        _gridPane?.RefreshAllBuyStates();
        _detailModal?.RefreshDisplay();
    }

    private void SetViewMode(ViewMode mode)
    {
        _viewMode = mode;
        if (mode == ViewMode.Directory)
        {
            _directoryPane.SetActive(true);
            _gridPane.SetActive(false);
            _statsLabel.text = $"CHOOSE A SHOP  ·  {ShopCatalog.Shops.Count} STORES";
            _statsLabel.color = new Color(0.55f, 0.70f, 0.85f, 1f);
            _directoryPane.Build();
        }
        else
        {
            _directoryPane.SetActive(false);
            _gridPane.SetActive(true);
            ShowShop(_activeShopIndex);
        }
    }

    private void OnStoreCardSelected(ShopPOCO shop)
    {
        var shops = ShopCatalog.Shops;
        int idx = 0;
        for (int i = 0; i < shops.Count; i++)
        {
            if (shops[i].ShopCode == shop.ShopCode)
            {
                idx = i;
                break;
            }
        }
        _activeShopIndex = idx;
        SetViewMode(ViewMode.ShopDetail);
    }

    private void ShowShop(int index)
    {
        var shops = ShopCatalog.Shops;
        if (shops.Count == 0)
        {
            _statsLabel.text = "NO SHOPS";
            _gridPane.RefreshActiveShop();
            return;
        }
        _activeShopIndex = Mathf.Clamp(index, 0, shops.Count - 1);
        var shop = shops[_activeShopIndex];
        _statsLabel.text = shop.Name.ToUpperInvariant();
        _statsLabel.color = new Color(0.78f, 0.95f, 0.25f, 1f);
        _gridPane.ActiveShopCode = shop.ShopCode;
        _gridPane.RefreshActiveShop();
        ModConfig<PocketShopConfig>.SetAndSave("LastShopIndex", _activeShopIndex);
    }

    private void OnBackClicked()
    {
        if (_detailModal != null && _detailModal.IsOpen)
        {
            _detailModal.Hide();
            return;
        }

        if (_viewMode == ViewMode.ShopDetail)
        {
            SetViewMode(ViewMode.Directory);
        }
        else
        {
            CloseApp();
        }
    }

    private void RefreshStats()
    {
        var mm = Il2CppScheduleOne.Money.MoneyManager.Instance;
        if (mm != null)
        {
            if (_cashChipText != null) _cashChipText.text = $"💵 ${mm.cashBalance:F0}";
            if (_bankChipText != null) _bankChipText.text = $"💳 ${mm.onlineBalance:F0}";
        }

        if (_viewMode == ViewMode.Directory && _statsLabel != null)
        {
            _statsLabel.text = $"CHOOSE A SHOP  ·  {ShopCatalog.Shops.Count} STORES";
        }

        _gridPane?.RefreshAllBuyStates();
    }

    private static Sprite BuildIconSprite()
    {
        if (_cachedIcon != null) return _cachedIcon;

        // 1. Try loading custom PNG from Mods directory
        try
        {
            string iconPath = Path.Combine(MelonEnvironment.ModsDirectory, "pocketshop_icon.png");
            if (File.Exists(iconPath))
            {
                byte[] data = File.ReadAllBytes(iconPath);
                var tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
                if (ImageConversion.LoadImage(tex, data))
                {
                    _cachedIcon = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                    _cachedIcon.name = "PocketShop_AppIcon";
                    return _cachedIcon;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLoader.MelonLogger.Warning($"Failed to load icon file: {ex.Message}");
        }

        // 2. Procedural Fallback
        try
        {
            var tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            var pixels = new Color32[128 * 128];
            int center = 64;
            float radius = 56;
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                {
                    float dx = x - center;
                    float dy = y - center;
                    pixels[y * 128 + x] = Mathf.Sqrt(dx * dx + dy * dy) <= radius
                        ? new Color(0.30f, 0.85f, 0.95f, 1f)
                        : new Color(0, 0, 0, 0);
                }
            }
            tex.SetPixels32(pixels);
            tex.Apply(false, false);
            _cachedIcon = Sprite.Create(tex, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f), 100f);
            _cachedIcon.name = "PocketShop_AppIcon";
            return _cachedIcon;
        }
        catch (Exception ex)
        {
            MelonLoader.MelonLogger.Warning($"Icon build failed ({ex.Message}), using minimal fallback");
            var fallback = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            _cachedIcon = Sprite.Create(fallback, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 100f);
            _cachedIcon.name = "PocketShop_AppIcon_Fallback";
            return _cachedIcon;
        }
    }
}
