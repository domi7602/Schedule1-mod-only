using System;
using BankApp.Models;
using BankApp.Services;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BankApp.UI.Panes;

/// <summary>
/// Main dashboard overview pane:
/// - Digital Checking Account card with live balance
/// - Cash on hand & Net worth indicators
/// - Quick action deposit/withdraw buttons
/// - Mini recent transactions feed
/// </summary>
public sealed class DashboardPane
{
    public event Action? OnDepositClicked;
    public event Action? OnWithdrawClicked;
    public event Action? OnViewAllHistoryClicked;

    private readonly RectTransform _parent;
    private GameObject? _rootPanel;

    private Text? _digitalBalanceText;
    private Text? _cashOnHandText;
    private Text? _cashSlotsSubText;
    private Text? _netWorthText;
    private Transform? _recentFeedContent;
    private string _recentFeedStamp = "";

    public DashboardPane(RectTransform parent)
    {
        _parent = parent;
    }

    public void Build()
    {
        if (_rootPanel != null)
        {
            UnityEngine.Object.Destroy(_rootPanel);
        }

        _rootPanel = new GameObject("DashboardRoot");
        _rootPanel.transform.SetParent(_parent, false);
        var rRt = _rootPanel.AddComponent<RectTransform>();
        rRt.anchorMin = Vector2.zero;
        rRt.anchorMax = Vector2.one;
        rRt.offsetMin = Vector2.zero;
        rRt.offsetMax = Vector2.zero;

        var scrollRect = _rootPanel.AddComponent<ScrollRect>();
        scrollRect.scrollSensitivity = 25f;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;

        var viewportGO = new GameObject("Viewport");
        viewportGO.transform.SetParent(_rootPanel.transform, false);
        var vrt = viewportGO.AddComponent<RectTransform>();
        vrt.anchorMin = Vector2.zero;
        vrt.anchorMax = Vector2.one;
        vrt.offsetMin = Vector2.zero;
        vrt.offsetMax = Vector2.zero;
        viewportGO.AddComponent<Image>().color = new Color(0, 0, 0, 0);
        viewportGO.AddComponent<RectMask2D>();
        scrollRect.viewport = vrt;

        var contentGO = new GameObject("Content");
        contentGO.transform.SetParent(viewportGO.transform, false);
        var contentRt = contentGO.AddComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0, 1);
        contentRt.anchorMax = new Vector2(1, 1);
        contentRt.pivot = new Vector2(0.5f, 1f);
        contentRt.offsetMin = Vector2.zero;
        contentRt.offsetMax = Vector2.zero;

        var vlg = contentGO.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(10f);
        vlg.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), (int)UITheme.Dp(10f), (int)UITheme.Dp(16f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        var csf = contentGO.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        scrollRect.content = contentRt;

        // 1. Digital Bank Account Card
        BuildDigitalCard(contentGO.transform);

        // 2. Quick Action Buttons (Deposit / Withdraw)
        BuildQuickActions(contentGO.transform);

        // 3. Secondary Metrics (Cash on hand & Net worth)
        BuildSecondaryMetrics(contentGO.transform);

        // 4. Recent Transactions Section
        BuildRecentFeed(contentGO.transform);

        Refresh();
    }

    private void BuildDigitalCard(Transform parent)
    {
        var card = UIFactory.Panel("DigitalCard", parent, UITheme.CardBg);
        var cardLE = card.AddComponent<LayoutElement>();
        cardLE.minHeight = UITheme.Dp(125f);
        cardLE.preferredHeight = UITheme.Dp(125f);

        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(4f);
        vlg.padding = new RectOffset((int)UITheme.Dp(14f), (int)UITheme.Dp(14f), (int)UITheme.Dp(12f), (int)UITheme.Dp(12f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // Header Row in Card: Bank Name + Chip tag
        var topRow = UIFactory.Panel("TopRow", card.transform, Color.clear);
        var topHlg = topRow.AddComponent<HorizontalLayoutGroup>();
        topHlg.childAlignment = TextAnchor.MiddleCenter;
        topHlg.childControlWidth = true;
        topHlg.childControlHeight = true;
        topHlg.childForceExpandWidth = true;
        topHlg.childForceExpandHeight = false;
        topRow.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(18f);

        var bankLabel = UIFactory.Text("BankName", "PALMETTO STATE BANK", topRow.transform, UITheme.Sp(10), TextAnchor.MiddleLeft, FontStyle.Bold);
        bankLabel.color = UITheme.AccentBlue;

        var statusLabel = UIFactory.Text("Status", "● ACTIVE CHECKING", topRow.transform, UITheme.Sp(9), TextAnchor.MiddleRight, FontStyle.Bold);
        statusLabel.color = UITheme.AccentGreen;

        // Subtitle: Card / Account number
        var subLabel = UIFactory.Text("AccountSub", "Checking Account •••• 4092", card.transform, UITheme.Sp(9), TextAnchor.MiddleLeft);
        subLabel.color = UITheme.TextMuted;

        // Large Balance Display
        _digitalBalanceText = UIFactory.Text("DigitalBalance", "$ 0.00", card.transform, UITheme.Sp(22), TextAnchor.MiddleLeft, FontStyle.Bold);
        _digitalBalanceText.color = UITheme.TextPrimary;

        // Bottom Card Info
        var bottomInfo = UIFactory.Text("BottomInfo", "Verified Online Account • Instant Mobile Access", card.transform, UITheme.Sp(8), TextAnchor.MiddleLeft, FontStyle.Italic);
        bottomInfo.color = UITheme.TextDim;
    }

    private void BuildQuickActions(Transform parent)
    {
        var row = UIFactory.Panel("QuickActionsRow", parent, Color.clear);
        var rowLE = row.AddComponent<LayoutElement>();
        rowLE.minHeight = UITheme.Dp(42f);
        rowLE.preferredHeight = UITheme.Dp(42f);

        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        // Deposit Button
        var depBtnGO = UIFactory.Panel("DepositBtn", row.transform, UITheme.AccentGreen);
        var depVlg = depBtnGO.AddComponent<VerticalLayoutGroup>();
        depVlg.childAlignment = TextAnchor.MiddleCenter;
        var depTxt = UIFactory.Text("Label", "⬇  DEPOSIT CASH", depBtnGO.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold);
        depTxt.color = Color.white;
        var depBtn = depBtnGO.AddComponent<Button>();
        depBtn.transition = Selectable.Transition.ColorTint;
        ButtonUtils.AddListener(depBtn, () =>
        {
            BankSoundService.PlayClick();
            OnDepositClicked?.Invoke();
        });

        // Withdraw Button
        var withBtnGO = UIFactory.Panel("WithdrawBtn", row.transform, UITheme.AccentOrange);
        var withVlg = withBtnGO.AddComponent<VerticalLayoutGroup>();
        withVlg.childAlignment = TextAnchor.MiddleCenter;
        var withTxt = UIFactory.Text("Label", "⬆  WITHDRAW CASH", withBtnGO.transform, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Bold);
        withTxt.color = Color.white;
        var withBtn = withBtnGO.AddComponent<Button>();
        withBtn.transition = Selectable.Transition.ColorTint;
        ButtonUtils.AddListener(withBtn, () =>
        {
            BankSoundService.PlayClick();
            OnWithdrawClicked?.Invoke();
        });
    }

    private void BuildSecondaryMetrics(Transform parent)
    {
        var row = UIFactory.Panel("MetricsRow", parent, Color.clear);
        var rowLE = row.AddComponent<LayoutElement>();
        rowLE.minHeight = UITheme.Dp(68f);
        rowLE.preferredHeight = UITheme.Dp(68f);

        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        // Card 1: Cash on Hand
        var cashCard = UIFactory.Panel("CashCard", row.transform, UITheme.CardBg);
        var cVlg = cashCard.AddComponent<VerticalLayoutGroup>();
        cVlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f));
        cVlg.spacing = UITheme.Dp(1f);
        cVlg.childControlWidth = true;
        cVlg.childControlHeight = true;
        cVlg.childForceExpandWidth = true;
        cVlg.childForceExpandHeight = false;

        var cHeader = UIFactory.Text("Header", "💵 Cash on Hand", cashCard.transform, UITheme.Sp(9), TextAnchor.MiddleLeft, FontStyle.Bold);
        cHeader.color = UITheme.TextMuted;

        _cashOnHandText = UIFactory.Text("CashVal", "$ 0.00", cashCard.transform, UITheme.Sp(13), TextAnchor.MiddleLeft, FontStyle.Bold);
        _cashOnHandText.color = UITheme.TextPrimary;

        _cashSlotsSubText = UIFactory.Text("SlotsSub", "0 slots free", cashCard.transform, UITheme.Sp(8), TextAnchor.MiddleLeft);
        _cashSlotsSubText.color = UITheme.TextDim;

        // Card 2: Total Net Worth
        var nwCard = UIFactory.Panel("NetWorthCard", row.transform, UITheme.CardBg);
        var nVlg = nwCard.AddComponent<VerticalLayoutGroup>();
        nVlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), (int)UITheme.Dp(6f), (int)UITheme.Dp(6f));
        nVlg.spacing = UITheme.Dp(1f);
        nVlg.childControlWidth = true;
        nVlg.childControlHeight = true;
        nVlg.childForceExpandWidth = true;
        nVlg.childForceExpandHeight = false;

        var nHeader = UIFactory.Text("Header", "🏛 Total Net Worth", nwCard.transform, UITheme.Sp(9), TextAnchor.MiddleLeft, FontStyle.Bold);
        nHeader.color = UITheme.TextMuted;

        _netWorthText = UIFactory.Text("NetWorthVal", "$ 0.00", nwCard.transform, UITheme.Sp(13), TextAnchor.MiddleLeft, FontStyle.Bold);
        _netWorthText.color = UITheme.TextPrimary;

        var nwSub = UIFactory.Text("NwSub", "Liquid & Assets", nwCard.transform, UITheme.Sp(8), TextAnchor.MiddleLeft);
        nwSub.color = UITheme.TextDim;
    }

    private void BuildRecentFeed(Transform parent)
    {
        var section = UIFactory.Panel("RecentSection", parent, UITheme.CardBg);
        var sLE = section.AddComponent<LayoutElement>();
        sLE.preferredHeight = UITheme.Dp(160f);

        var vlg = section.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), (int)UITheme.Dp(8f), (int)UITheme.Dp(8f));
        vlg.spacing = UITheme.Dp(4f);
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // Header Row
        var headerRow = UIFactory.Panel("HeaderRow", section.transform, Color.clear);
        var hHlg = headerRow.AddComponent<HorizontalLayoutGroup>();
        hHlg.childControlWidth = true;
        hHlg.childControlHeight = true;
        hHlg.childForceExpandWidth = true;
        hHlg.childForceExpandHeight = false;
        headerRow.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(16f);

        var title = UIFactory.Text("Title", "RECENT ACTIVITY", headerRow.transform, UITheme.Sp(10), TextAnchor.MiddleLeft, FontStyle.Bold);
        title.color = UITheme.TextMuted;

        var viewAllBtnGO = UIFactory.Panel("ViewAllBtn", headerRow.transform, Color.clear);
        var vaTxt = UIFactory.Text("Label", "View All →", viewAllBtnGO.transform, UITheme.Sp(9), TextAnchor.MiddleRight, FontStyle.Bold);
        vaTxt.color = UITheme.AccentBlue;
        var vaBtn = viewAllBtnGO.AddComponent<Button>();
        vaBtn.transition = Selectable.Transition.ColorTint;
        ButtonUtils.AddListener(vaBtn, () =>
        {
            BankSoundService.PlayClick();
            OnViewAllHistoryClicked?.Invoke();
        });

        // Feed container
        var feedGO = new GameObject("FeedContent");
        feedGO.transform.SetParent(section.transform, false);
        _recentFeedContent = feedGO.transform;
        var fVlg = feedGO.AddComponent<VerticalLayoutGroup>();
        fVlg.spacing = UITheme.Dp(4f);
        fVlg.childControlWidth = true;
        fVlg.childControlHeight = true;
        fVlg.childForceExpandWidth = true;
        fVlg.childForceExpandHeight = false;
    }

    public void Refresh()
    {
        float online = BankService.GetOnlineBalance();
        float cash = BankService.GetCashBalance();
        float netWorth = BankService.GetNetWorth();
        int freeSlots = BankService.GetFreeInventorySlotsCount();

        if (_digitalBalanceText != null) _digitalBalanceText.text = $"$ {online:N2}";
        if (_cashOnHandText != null) _cashOnHandText.text = $"$ {cash:N2}";
        if (_cashSlotsSubText != null) _cashSlotsSubText.text = $"{freeSlots} free slot{(freeSlots == 1 ? "" : "s")}";
        if (_netWorthText != null) _netWorthText.text = $"$ {netWorth:N2}";

        RefreshRecentFeed();
    }

    private void RefreshRecentFeed()
    {
        if (_recentFeedContent == null) return;

        var list = TransactionHistoryService.GetTransactions();
        if (list == null) return;

        int count = Mathf.Min(3, list.Count);
        string stamp = count == 0 ? "" : $"{list[0].Id}|{count}";
        if (stamp == _recentFeedStamp) return;
        _recentFeedStamp = stamp;

        // Clear previous children
        for (int i = _recentFeedContent.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(_recentFeedContent.GetChild(i).gameObject);
        }

        if (count == 0)
        {
            var emptyTxt = UIFactory.Text("Empty", "No recent transactions recorded.", _recentFeedContent, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Italic);
            emptyTxt.color = UITheme.TextDim;
            return;
        }

        for (int i = 0; i < count; i++)
        {
            var tx = list[i];
            BuildRecentRow(_recentFeedContent, tx);
        }
    }

    private void BuildRecentRow(Transform parent, BankTransaction tx)
    {
        var row = UIFactory.Panel($"Row_{tx.Id}", parent, UITheme.CardBgSecondary);
        var rLE = row.AddComponent<LayoutElement>();
        rLE.preferredHeight = UITheme.Dp(32f);
        rLE.minHeight = UITheme.Dp(32f);

        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset((int)UITheme.Dp(8f), (int)UITheme.Dp(8f), 0, 0);
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;

        // Left description & time
        var leftCol = UIFactory.Panel("LeftCol", row.transform, Color.clear);
        var lVlg = leftCol.AddComponent<VerticalLayoutGroup>();
        lVlg.spacing = 0;
        lVlg.childControlWidth = true;
        lVlg.childControlHeight = true;
        lVlg.childForceExpandWidth = true;
        lVlg.childForceExpandHeight = false;

        var desc = UIFactory.Text("Desc", tx.Description, leftCol.transform, UITheme.Sp(9), TextAnchor.MiddleLeft, FontStyle.Bold);
        desc.color = UITheme.TextPrimary;

        var time = UIFactory.Text("Time", $"Day {tx.InGameDay}, {tx.InGameTime}", leftCol.transform, UITheme.Sp(8), TextAnchor.MiddleLeft);
        time.color = UITheme.TextDim;

        // Right amount
        bool isPositive = tx.Amount >= 0f;
        string sign = isPositive ? "+" : "-";
        float absAmt = Mathf.Abs(tx.Amount);

        var amt = UIFactory.Text("Amt", $"{sign}$ {absAmt:N2}", row.transform, UITheme.Sp(10), TextAnchor.MiddleRight, FontStyle.Bold);
        amt.color = isPositive ? UITheme.AccentGreen : UITheme.AccentOrange;
    }

    public void SetActive(bool active)
    {
        if (_rootPanel != null)
        {
            _rootPanel.SetActive(active);
            if (active) Refresh();
        }
    }
}
