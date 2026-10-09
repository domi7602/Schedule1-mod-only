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
    private GameObject _backButton = null!;

    // Balance chips (v0.3.1: Cash + Card — black-market shops charge cash, clean shops charge card)
    private Image _cardChipBg = null!;
    private Text _cardChipText = null!;
    private Text _cashChipText = null!;

    private ViewMode _viewMode = ViewMode.Directory;
    private int _activeShopIndex;
    private float _lastStatsRefreshTime;
    private InputField _searchField = null!;
    private string _searchQuery = string.Empty;

    // Fix (Bug-Audit 2026-09-10): the phone re-instantiates this app per scene load and the old
    // per-instance handler stayed subscribed to MelonEvents.OnUpdate forever, while ItemGridPane's
    // ShopCatalog.OnCatalogChanged subscription leaked because its Dispose() was never called.
    // OnUpdate now dispatches through _active (subscribed exactly once); Mod.OnSceneWasUnloaded
    // clears _active and disposes the grid pane + detail modal when the gameplay scene tears down.
    private static PocketShopApp? _active;
    private static bool _staticSubscribed;
    private bool _cleanupComplete;

    protected override void OnCreated()
    {
        var previous = _active;
        if (previous != null && !ReferenceEquals(previous, this))
            previous.CleanupAppResources();

        _active = this;
        // Defensive idempotent registration: only one dispatcher survives scene reloads.
        MelonEvents.OnUpdate.Unsubscribe(DispatchUpdate);
        MelonEvents.OnUpdate.Subscribe(DispatchUpdate);
        _staticSubscribed = true;
        base.OnCreated();
        MelonLogger.Msg("Registered with S1API PhoneApp system (v0.3.2).");
    }

    protected override void OnDestroyed()
    {
        try { CleanupAppResources(); }
        finally { base.OnDestroyed(); }
    }

    internal static void TearDownForSceneUnload()
    {
        var app = _active;
        if (app != null)
        {
            app.CleanupAppResources();
            return;
        }

        ShopCatalog.StopRetryLoop();
        if (_staticSubscribed)
        {
            MelonEvents.OnUpdate.Unsubscribe(DispatchUpdate);
            _staticSubscribed = false;
        }
        DestroyCachedIcon();
    }

    private void CleanupAppResources()
    {
        if (_cleanupComplete) return;
        _cleanupComplete = true;
        bool wasActive = ReferenceEquals(_active, this);
        if (wasActive)
        {
            _active = null;
            if (_staticSubscribed)
            {
                MelonEvents.OnUpdate.Unsubscribe(DispatchUpdate);
                _staticSubscribed = false;
            }
        }

        ShopCatalog.StopRetryLoop();
        try
        {
            if (_gridPane != null)
            {
                _gridPane.OnPurchaseResult -= HandlePurchaseResult;
                _gridPane.OnCatalogChanged -= HandleCatalogChanged;
                _gridPane.Dispose();
            }
        }
        catch { }
        try
        {
            if (_directoryPane != null)
            {
                _directoryPane.OnShopSelected -= OnStoreCardSelected;
                _directoryPane.Dispose();
            }
        }
        catch { }

        if (wasActive) DestroyCachedIcon();
        _gridPane = null!;
        _directoryPane = null!;
        _mainBG = null!;
        _statsLabel = null!;
        _backButton = null!;
        _searchField = null!;
        _cashChipText = null!;
        _cardChipText = null!;
        _cardChipBg = null!;
    }

    private static void DestroyCachedIcon()
    {
        var sprite = _cachedIcon;
        _cachedIcon = null;
        if (sprite == null) return;
        try
        {
            if (sprite.Pointer == IntPtr.Zero || sprite.WasCollected) return;
            var texture = sprite.texture;
            if (texture != null && texture.Pointer != IntPtr.Zero && !texture.WasCollected)
                UnityEngine.Object.Destroy(texture);
            UnityEngine.Object.Destroy(sprite);
        }
        catch { }
    }

    private static void DispatchUpdate()
    {
        var app = _active;
        if (app == null || app._cleanupComplete) return;
        try { app.Update(); }
        catch (Exception ex)
        {
            MelonLogger.Error($"[PocketShop] App update failed; cleaning up stale UI: {ex.Message}");
            app.CleanupAppResources();
        }
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (_cleanupComplete) return;
        try
        {
            if (_mainBG != null && _mainBG.Pointer != IntPtr.Zero && !_mainBG.WasCollected)
                _mainBG.SetActive(false);
        }
        catch { }
        // Close-path must stay allocation- and rebuild-free: the next OnAppOpened()
        // rebuilds via SetViewMode(Directory) anyway. In particular, _searchField.text =
        // would fire OnSearchChanged -> ApplySearch -> full Directory.Build() synchronously
        // inside the close handler, stalling the phone-lower animation (glass phone
        // while the device is put away). SetTextWithoutNotify resets the visible text
        // without triggering the event chain.
        _viewMode = ViewMode.Directory;
        _searchQuery = string.Empty;
        if (_searchField != null)
        {
            try { _searchField.SetTextWithoutNotify(string.Empty); } catch { }
        }
        if (_directoryPane != null) _directoryPane.Filter = string.Empty;
        if (_gridPane != null) _gridPane.Filter = string.Empty;
        // Update bleibt lebenslang subscribed (defensives Unsubscribe-Subscribe in OnCreated).
    }

    private void Update()
    {
        if (_cleanupComplete) return;
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
                // The search field owns the first Escape (same behaviour as the Taxi app);
                // a second Escape navigates back / closes.
                if (_searchQuery.Trim().Length > 0)
                {
                    ClearSearch();
                    ApplySearch();
                }
                else
                {
                    OnBackClicked();
                }
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
        if (_cleanupComplete) return;
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

        // 1. Header: Back | Title | live search | X
        _searchField = AppHeaderBuilder.Build(_mainBG.transform, OnBackClicked, () => CloseApp(), OnSearchChanged, out _backButton);
        try
        {
            var focus = _searchField.gameObject.AddComponent<PocketShopInputFocus>();
            focus.inputField = _searchField;
        }
        catch (Exception ex) { MelonLogger.Warning($"[PocketShop] search focus guard failed: {ex.Message}"); }

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

        // Payment Chips: [CASH $X] [CARD $Y]  (no emoji glyphs: Arial renders them blank)
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
        _gridPane.OnCatalogChanged += HandleCatalogChanged;

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
        _cashChipText = S1API.UI.UIFactory.Text("Txt", "CASH $—", cashPanel.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
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
        _cardChipText = S1API.UI.UIFactory.Text("Txt", "CARD $—", cardPanel.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _cardChipText.color = new Color(0.30f, 0.85f, 0.95f, 1f);
        _cardChipText.raycastTarget = false;
    }

    private void HandleCatalogChanged()
    {
        if (_cleanupComplete) return;
        try { _directoryPane?.RefreshShopCount(); }
        catch { }
    }

    private void HandlePurchaseResult(PurchaseResultData result)
    {
        if (_cleanupComplete) return;
        RefreshStats();
        _gridPane?.ShowStatus(result.Message, result.IsSuccess);
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
        _backButton.SetActive(mode != ViewMode.Directory);
        if (mode == ViewMode.Directory)
        {
            _directoryPane.SetActive(true);
            _gridPane.SetActive(false);
            _statsLabel.text = $"CHOOSE A SHOP  ·  {ShopCatalog.Shops.Count} STORES";
            _statsLabel.color = new Color(0.55f, 0.70f, 0.85f, 1f);
            _directoryPane.Filter = _searchQuery;
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
        _gridPane.Filter = _searchQuery;
        _gridPane.RefreshActiveShop();
        ModConfig<PocketShopConfig>.SetAndSave("LastShopIndex", _activeShopIndex);
    }

    /// <summary>Live search: filters the store tiles (Directory) or the item list (ShopDetail).</summary>
    private void OnSearchChanged(string value)
    {
        _searchQuery = value ?? string.Empty;
        ApplySearch();
    }

    private void ApplySearch()
    {
        if (_viewMode == ViewMode.Directory)
        {
            _directoryPane.Filter = _searchQuery;
            _directoryPane.Build();
        }
        else
        {
            _gridPane.Filter = _searchQuery;
            _gridPane.RefreshActiveShop();
        }
    }

    private void ClearSearch()
    {
        _searchQuery = string.Empty;
        if (_searchField != null)
        {
            // No event: callers (Escape handler) run ApplySearch() explicitly right after.
            // .text = would fire OnSearchChanged -> ApplySearch a first time, rebuilding
            // the directory twice per keypress.
            try { _searchField.SetTextWithoutNotify(string.Empty); } catch { }
        }
        if (_directoryPane != null) _directoryPane.Filter = string.Empty;
        if (_gridPane != null) _gridPane.Filter = string.Empty;
    }

    private void OnBackClicked()
    {
        if (_viewMode == ViewMode.ShopDetail)
        {
            ClearSearch();
            SetViewMode(ViewMode.Directory);
        }
        else
        {
            CloseApp();
        }
    }

    private void RefreshStats()
    {
        if (_cleanupComplete) return;
        var mm = Il2CppScheduleOne.Money.MoneyManager.Instance;
        if (mm != null)
        {
            // v0.3.1: both balances — black-market = Cash, clean shops = Card.
            // No emoji glyphs: Arial renders them as blanks, so the chips would show an empty gap.
            if (_cashChipText != null) _cashChipText.text = $"CASH ${mm.cashBalance:F0}";
            if (_cardChipText != null) _cardChipText.text = $"CARD ${mm.onlineBalance:F0}";
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
