using System;
using System.IO;
using BankApp.Services;
using BankApp.UI;
using BankApp.UI.Panes;
using MelonLoader;
using MelonLoader.Utils;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BankApp;

public enum BankTab
{
    Dashboard,
    Transfer,
    History
}

/// <summary>
/// Mobile Banking Smartphone App for Schedule I (v0.1.0).
/// Provides checking account balances, slot-aware deposits/withdrawals,
/// weekly ATM limit tracking, and save-slot isolated statement logs.
/// </summary>
public sealed class BankApp : PhoneApp
{
    protected override string AppName => "BankApp";
    protected override string AppTitle => "National Bank";
    protected override string IconLabel => "Bank";
    protected override string IconFileName => "bank_icon.png";
    protected override EOrientation Orientation => EOrientation.Vertical;

    private static Sprite? _cachedIcon;
    protected override Sprite IconSprite => BuildIconSprite();

    private GameObject _mainBG = null!;
    private BankAppInputFocus _focusHook = null!;

    // Panes
    private DashboardPane _dashboardPane = null!;
    private TransferPane _transferPane = null!;
    private HistoryPane _historyPane = null!;

    // Navigation Tabs
    private Image _tabDashboardBg = null!;
    private Text _tabDashboardText = null!;
    private Image _tabTransferBg = null!;
    private Text _tabTransferText = null!;
    private Image _tabHistoryBg = null!;
    private Text _tabHistoryText = null!;

    private BankTab _currentTab = BankTab.Dashboard;
    private float _lastStatsRefreshTime;

    protected override void OnCreated()
    {
        base.OnCreated();
        MelonEvents.OnUpdate.Unsubscribe(Update);
        MelonEvents.OnUpdate.Subscribe(Update);
        S1API.Money.Money.OnBalanceChanged -= OnExternalBalanceChanged;
        S1API.Money.Money.OnBalanceChanged += OnExternalBalanceChanged;
        TransactionHistoryService.OnHistoryChanged -= OnExternalBalanceChanged;
        TransactionHistoryService.OnHistoryChanged += OnExternalBalanceChanged;
        MelonLogger.Msg("Registered with S1API PhoneApp system (v0.1.0).");
    }

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.Initialize(containerRt);
        }

        _focusHook = container.AddComponent<BankAppInputFocus>();

        // 1. Root Isolated Background Panel
        _mainBG = UIFactory.Panel("BankApp_MainBG", container.transform, UITheme.BgDark, fullAnchor: true);
        _mainBG.SetActive(false);

        var vlg = _mainBG.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 0f;
        vlg.padding = new RectOffset(0, 0, 0, 0);
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // 2. Top App Header Bar
        BuildHeader(_mainBG.transform);

        // 3. Tab Switcher Navigation Bar
        BuildTabBar(_mainBG.transform);

        // 4. Content Area Hosting the 3 Panes
        var contentArea = UIFactory.Panel("ContentArea", _mainBG.transform, Color.clear);
        var contentLe = contentArea.GetComponent<LayoutElement>();
        if (contentLe == null) contentLe = contentArea.AddComponent<LayoutElement>();
        contentLe.flexibleHeight = 1f;
        var contentRt = contentArea.GetComponent<RectTransform>();

        _dashboardPane = new DashboardPane(contentRt);
        _dashboardPane.Build();
        _dashboardPane.OnDepositClicked += () =>
        {
            _transferPane.SetMode(TransferMode.Deposit);
            SwitchTab(BankTab.Transfer);
        };
        _dashboardPane.OnWithdrawClicked += () =>
        {
            _transferPane.SetMode(TransferMode.Withdraw);
            SwitchTab(BankTab.Transfer);
        };
        _dashboardPane.OnViewAllHistoryClicked += () => SwitchTab(BankTab.History);

        _transferPane = new TransferPane(contentRt, _focusHook);
        _transferPane.Build();
        _transferPane.OnTransactionCompleted += () =>
        {
            _dashboardPane.Refresh();
            _historyPane.Refresh();
        };

        _historyPane = new HistoryPane(contentRt);
        _historyPane.Build();

        SwitchTab(BankTab.Dashboard);
    }

    private void BuildHeader(Transform parent)
    {
        var header = UIFactory.Panel("Header", parent, UITheme.HeaderBg);
        var le = header.GetComponent<LayoutElement>();
        if (le == null) le = header.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(38f);
        le.minHeight = UITheme.Dp(38f);
        le.flexibleHeight = 0f;

        var hlg = header.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), 0, 0);
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;

        // Left bank icon + title
        var title = UIFactory.Text("Title", "🏛  PALMETTO BANK", header.transform, UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold);
        title.color = UITheme.TextPrimary;

        // Right secure badge
        var badge = UIFactory.Text("Badge", "🔒 256-BIT SECURE", header.transform, UITheme.Sp(8), TextAnchor.MiddleRight, FontStyle.Bold);
        badge.color = UITheme.AccentGreen;
    }

    private void BuildTabBar(Transform parent)
    {
        var bar = UIFactory.Panel("TabBar", parent, UITheme.HeaderBg);
        var le = bar.GetComponent<LayoutElement>();
        if (le == null) le = bar.AddComponent<LayoutElement>();
        le.preferredHeight = UITheme.Dp(32f);
        le.minHeight = UITheme.Dp(32f);
        le.flexibleHeight = 0f;

        var hlg = bar.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(2f), (int)UITheme.Dp(4f));
        hlg.spacing = UITheme.Dp(4f);
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        // Tab 1: Dashboard
        var t1 = UIFactory.Panel("Tab_Dash", bar.transform, UITheme.CardBg);
        _tabDashboardBg = t1.GetComponent<Image>();
        var t1Vlg = t1.AddComponent<VerticalLayoutGroup>();
        t1Vlg.childAlignment = TextAnchor.MiddleCenter;
        _tabDashboardText = UIFactory.Text("Lbl", "📊 Accounts", t1.transform, UITheme.Sp(9), TextAnchor.MiddleCenter, FontStyle.Bold);
        var b1 = t1.AddComponent<Button>();
        b1.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(b1, () =>
        {
            BankSoundService.PlayClick();
            SwitchTab(BankTab.Dashboard);
        });

        // Tab 2: Transfer
        var t2 = UIFactory.Panel("Tab_Transfer", bar.transform, UITheme.CardBg);
        _tabTransferBg = t2.GetComponent<Image>();
        var t2Vlg = t2.AddComponent<VerticalLayoutGroup>();
        t2Vlg.childAlignment = TextAnchor.MiddleCenter;
        _tabTransferText = UIFactory.Text("Lbl", "⇄ Transfer", t2.transform, UITheme.Sp(9), TextAnchor.MiddleCenter, FontStyle.Bold);
        var b2 = t2.AddComponent<Button>();
        b2.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(b2, () =>
        {
            BankSoundService.PlayClick();
            SwitchTab(BankTab.Transfer);
        });

        // Tab 3: History
        var t3 = UIFactory.Panel("Tab_Hist", bar.transform, UITheme.CardBg);
        _tabHistoryBg = t3.GetComponent<Image>();
        var t3Vlg = t3.AddComponent<VerticalLayoutGroup>();
        t3Vlg.childAlignment = TextAnchor.MiddleCenter;
        _tabHistoryText = UIFactory.Text("Lbl", "📜 Statement", t3.transform, UITheme.Sp(9), TextAnchor.MiddleCenter, FontStyle.Bold);
        var b3 = t3.AddComponent<Button>();
        b3.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(b3, () =>
        {
            BankSoundService.PlayClick();
            SwitchTab(BankTab.History);
        });
    }

    private void SwitchTab(BankTab tab)
    {
        _currentTab = tab;

        _dashboardPane?.SetActive(tab == BankTab.Dashboard);
        _transferPane?.SetActive(tab == BankTab.Transfer);
        _historyPane?.SetActive(tab == BankTab.History);

        // Update tab pill highlights
        if (_tabDashboardBg != null) _tabDashboardBg.color = tab == BankTab.Dashboard ? UITheme.AccentBlue : UITheme.CardBg;
        if (_tabDashboardText != null) _tabDashboardText.color = tab == BankTab.Dashboard ? Color.white : UITheme.TextMuted;

        if (_tabTransferBg != null) _tabTransferBg.color = tab == BankTab.Transfer ? UITheme.AccentBlue : UITheme.CardBg;
        if (_tabTransferText != null) _tabTransferText.color = tab == BankTab.Transfer ? Color.white : UITheme.TextMuted;

        if (_tabHistoryBg != null) _tabHistoryBg.color = tab == BankTab.History ? UITheme.AccentBlue : UITheme.CardBg;
        if (_tabHistoryText != null) _tabHistoryText.color = tab == BankTab.History ? Color.white : UITheme.TextMuted;
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
                if (_currentTab != BankTab.Dashboard)
                {
                    BankSoundService.PlayClick();
                    SwitchTab(BankTab.Dashboard);
                }
                else
                {
                    CloseApp();
                }
            }

            if (Time.unscaledTime - _lastStatsRefreshTime > 1.5f)
            {
                _lastStatsRefreshTime = Time.unscaledTime;
                RefreshActivePane();
            }
        }
    }

    private void OnAppOpened()
    {
        _dashboardPane?.Refresh();
        _transferPane?.Refresh();
        _historyPane?.Refresh();
    }

    private void OnExternalBalanceChanged()
    {
        if (IsOpen())
        {
            RefreshActivePane();
        }
    }

    private void RefreshActivePane()
    {
        switch (_currentTab)
        {
            case BankTab.Dashboard:
                _dashboardPane?.Refresh();
                break;
            case BankTab.Transfer:
                _transferPane?.Refresh();
                break;
            case BankTab.History:
                _historyPane?.Refresh();
                break;
        }
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (_mainBG != null) _mainBG.SetActive(false);
        if (_focusHook != null) _focusHook.amountInput = null;
        _currentTab = BankTab.Dashboard;
        // Update bleibt lebenslang subscribed (defensives Unsubscribe-Subscribe in OnCreated).
        // Balance/History-Handler bleiben ebenfalls subscribed (Events sind statisch, kein Leak-Risiko durch Phone-Cycle).
    }

    private static Sprite BuildIconSprite()
    {
        if (_cachedIcon != null) return _cachedIcon;

        // 1. Try to load from disk
        try
        {
            string iconPath = Path.Combine(MelonEnvironment.ModsDirectory, "bank_icon.png");
            if (File.Exists(iconPath))
            {
                byte[] data = File.ReadAllBytes(iconPath);
                var tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
                if (ImageConversion.LoadImage(tex, data))
                {
                    _cachedIcon = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                    _cachedIcon.name = "BankApp_AppIcon";
                    return _cachedIcon;
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"Failed to load icon file: {ex.Message}");
        }

        // 2. Procedural Fallback (Sapphire blue background with bank vault emblem)
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
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);

                    if (dist <= radius)
                    {
                        // Gradient sapphire blue circle
                        float t = (float)y / 128f;
                        pixels[y * 128 + x] = Color.Lerp(new Color(0.12f, 0.35f, 0.85f, 1f), new Color(0.25f, 0.60f, 1.0f, 1f), t);
                    }
                    else
                    {
                        pixels[y * 128 + x] = new Color(0, 0, 0, 0);
                    }
                }
            }
            tex.SetPixels32(pixels);
            tex.Apply(false, false);
            _cachedIcon = Sprite.Create(tex, new Rect(0, 0, 128, 128), new Vector2(0.5f, 0.5f), 100f);
            _cachedIcon.name = "BankApp_AppIcon_Procedural";
            return _cachedIcon;
        }
        catch
        {
            var fallback = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            _cachedIcon = Sprite.Create(fallback, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 100f);
            return _cachedIcon;
        }
    }
}
