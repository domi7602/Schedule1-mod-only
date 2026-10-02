using System;
using System.IO;
using MelonLoader;
using MelonLoader.Utils;
using PocketShop.Config;
using PocketShop.Services;
using PocketShop.UI;
using S1Mods.Shared;
using S1API.PhoneApp;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;
using UITheme = S1Mods.Shared.UITheme;

namespace PocketShop;

public enum ViewMode
{
    Directory,
    ShopDetail
}

/// <summary>
/// PocketShop PhoneApp for Schedule I (v0.3.0).
/// Features:
///   - 2-Level Navigation (Store Directory & Item Grid)
///   - Card-Only Payment (v0.3.0: Schedule I charges legal shops by card — no Cash/Auto switcher)
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
    private Text _statsLabel = null!;

    // Balance chips (v0.3.1: Cash + Card — black-market shops charge cash, clean shops charge card)
    private Image _cardChipBg = null!;
    private Text _cardChipText = null!;
    private Text _cashChipText = null!;

    private ViewMode _viewMode = ViewMode.Directory;
    private int _activeShopIndex;
    private float _lastStatsRefreshTime;

    // Fix (Bug-Audit 2026-09-10): the phone re-instantiates this app per scene load and the old
    // per-instance handler stayed subscribed to MelonEvents.OnUpdate forever, while ItemGridPane's
    // ShopCatalog.OnCatalogChanged subscription leaked because its Dispose() was never called.
    // OnUpdate now dispatches through _active (subscribed exactly once); Mod.OnSceneWasUnloaded
    // clears _active and disposes the grid pane + detail modal when the gameplay scene tears down.
    private static PocketShopApp? _active;
    private static bool _staticSubscribed;

    protected override void OnCreated()
    {
        _active = this;
        if (!_staticSubscribed)
        {
            _staticSubscribed = true;
            MelonEvents.OnUpdate.Subscribe(DispatchUpdate);
        }
        base.OnCreated();
        MelonLogger.Msg("Registered with S1API PhoneApp system (v0.3.9).");
    }

    internal static void TearDownForSceneUnload()
    {
        var app = _active;
        _active = null;
        if (app == null) return;
        try { app._gridPane?.Dispose(); } catch { }
        // DirectoryPane holds scene GameObjects too — destroy them as well and drop
        // the instance-level OnShopSelected handler so nothing survives the unload.
        try
        {
            if (app._directoryPane != null)
                app._directoryPane.OnShopSelected -= app.OnStoreCardSelected;
        }
        catch { }
        try { app._directoryPane?.Dispose(); } catch { }

        if (_cachedIcon != null)
        {
            try
            {
                if (_cachedIcon.Pointer != IntPtr.Zero && !_cachedIcon.WasCollected)
                {
                    if (_cachedIcon.texture != null && _cachedIcon.texture.Pointer != IntPtr.Zero && !_cachedIcon.texture.WasCollected)
                        UnityEngine.Object.Destroy(_cachedIcon.texture);
                    UnityEngine.Object.Destroy(_cachedIcon);
                }
            }
            catch { }
            _cachedIcon = null;
        }
    }

    private static void DispatchUpdate()
    {
        var a = _active;
        if (a != null)
            a.Update();
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (_mainBG != null) _mainBG.SetActive(false);
        _viewMode = ViewMode.Directory;
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
            // v0.3.9: the very first directory build can hit a stale viewport (app just
            // became visible) — rebuild once with the real rect instead of making the
            // user reopen the app twice.
            if (_viewMode == ViewMode.Directory)
                _directoryPane?.TickRetry();

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
        // Bug-Audit 2026-09-13 (Round 5): wire the catalog-change callback
        // so the directory pane's store-count badge updates when the catalog refreshes.
        _gridPane.OnCatalogChanged += () => _directoryPane?.RefreshShopCount();
    }

    /// <summary>
    /// v0.3.1: two balance chips (non-interactive). Black-market shops charge Cash,
    /// clean/legal shops charge by card — so both balances are shown side by side.
    /// </summary>
    private void BuildPaymentChips(Transform parent)
    {
        // Cash chip
        var cashPanel = S1API.UI.UIFactory.Panel("CashChip", parent, new Color(0.10f, 0.20f, 0.14f, 1f));
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

        // Card chip
        var cardPanel = S1API.UI.UIFactory.Panel("CardChip", parent, new Color(0.10f, 0.16f, 0.24f, 1f));
        _cardChipBg = cardPanel.GetComponent<Image>() ?? cardPanel.AddComponent<Image>();
        var cardLE = cardPanel.AddComponent<LayoutElement>();
        cardLE.preferredWidth = UITheme.Dp(120f);
        cardLE.minWidth = UITheme.Dp(100f);
        cardLE.preferredHeight = UITheme.Dp(24f);
        var cardVlg = cardPanel.AddComponent<VerticalLayoutGroup>();
        cardVlg.childControlWidth = true;
        cardVlg.childControlHeight = true;
        cardVlg.childForceExpandWidth = true;
        cardVlg.childForceExpandHeight = true;
        cardVlg.childAlignment = TextAnchor.MiddleCenter;
        _cardChipText = S1API.UI.UIFactory.Text("Txt", "💳 $—", cardPanel.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _cardChipText.color = new Color(0.30f, 0.85f, 0.95f, 1f);
        _cardChipText.raycastTarget = false;
    }

    private void HandlePurchaseResult(PurchaseResultData result)
    {
        RefreshStats();
        if (result.IsSuccess && result.Item != null)
        {
            // Bug-Audit 2026-09-12: only refresh BUY-state left the visible card stale
            // (badge + QuantitySelector max). Notify the matching grid card so it
            // re-clamps its selector and redraws the stock badge in sync with the
            // live listing that PurchaseService just decremented.
            _gridPane?.NotifyStockChanged(result.Item.ItemId);
        }
        else
        {
            _gridPane?.RefreshAllBuyStates();
        }
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
            // v0.3.1: both balances — black-market = Cash, clean shops = Card.
            if (_cashChipText != null) _cashChipText.text = $"💵 ${mm.cashBalance:F0}";
            if (_cardChipText != null) _cardChipText.text = $"💳 ${mm.onlineBalance:F0}";
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
