using System;
using BankApp.Models;
using BankApp.Services;
using S1API.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BankApp.UI.Panes;

/// <summary>
/// Displays the full save-slot isolated bank transaction statements and history.
/// </summary>
public sealed class HistoryPane
{
    private readonly RectTransform _parent;
    private GameObject? _rootPanel;
    private Transform? _listContent;
    private Text? _slotBadgeText;
    private string _listStamp = "";

    public HistoryPane(RectTransform parent)
    {
        _parent = parent;
    }

    public void Build()
    {
        if (_rootPanel != null)
        {
            UnityEngine.Object.Destroy(_rootPanel);
        }

        _rootPanel = new GameObject("HistoryRoot");
        _rootPanel.transform.SetParent(_parent, false);
        var rRt = _rootPanel.AddComponent<RectTransform>();
        rRt.anchorMin = Vector2.zero;
        rRt.anchorMax = Vector2.one;
        rRt.offsetMin = Vector2.zero;
        rRt.offsetMax = Vector2.zero;

        var vlg = _rootPanel.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(6f);
        vlg.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), (int)UITheme.Dp(10f), (int)UITheme.Dp(12f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // 1. Top Header Banner
        BuildHeaderBanner(_rootPanel.transform);

        // 2. Scrollable Transaction List
        BuildScrollList(_rootPanel.transform);

        Refresh();
    }

    private void BuildHeaderBanner(Transform parent)
    {
        var header = UIFactory.Panel("HeaderBanner", parent, UITheme.CardBg);
        header.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(36f);

        var hlg = header.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), 0, 0);
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;

        var title = UIFactory.Text("Title", "ACCOUNT STATEMENT", header.transform, UITheme.Sp(10), TextAnchor.MiddleLeft, FontStyle.Bold);
        title.color = UITheme.TextPrimary;

        _slotBadgeText = UIFactory.Text("SlotBadge", "Save Slot", header.transform, UITheme.Sp(9), TextAnchor.MiddleRight, FontStyle.Bold);
        _slotBadgeText.color = UITheme.AccentBlue;
    }

    private void BuildScrollList(Transform parent)
    {
        var scrollGO = new GameObject("ScrollArea");
        scrollGO.transform.SetParent(parent, false);
        var sRt = scrollGO.AddComponent<RectTransform>();
        sRt.anchorMin = Vector2.zero;
        sRt.anchorMax = Vector2.one;
        scrollGO.AddComponent<LayoutElement>().flexibleHeight = 1f;

        var scrollRect = scrollGO.AddComponent<ScrollRect>();
        scrollRect.scrollSensitivity = 25f;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;

        var viewportGO = new GameObject("Viewport");
        viewportGO.transform.SetParent(scrollGO.transform, false);
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
        _listContent = contentGO.transform;
        var contentRt = contentGO.AddComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0, 1);
        contentRt.anchorMax = new Vector2(1, 1);
        contentRt.pivot = new Vector2(0.5f, 1f);
        contentRt.offsetMin = Vector2.zero;
        contentRt.offsetMax = Vector2.zero;

        var vlg = contentGO.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(4f);
        vlg.padding = new RectOffset(0, 0, (int)UITheme.Dp(4f), (int)UITheme.Dp(10f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        var csf = contentGO.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        scrollRect.content = contentRt;
    }

    public void Refresh()
    {
        if (_slotBadgeText != null)
        {
            string slot = TransactionHistoryService.GetSaveSlotSuffix();
            _slotBadgeText.text = $"Slot: {slot.Replace('_', ' ').ToUpper()}";
        }

        if (_listContent == null) return;

        var list = TransactionHistoryService.GetTransactions();
        if (list == null) return;

        int count = list.Count;
        string stamp = count == 0 ? "" : $"{list[0].Id}|{count}";
        if (stamp == _listStamp) return;
        _listStamp = stamp;

        // Clear children
        for (int i = _listContent.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(_listContent.GetChild(i).gameObject);
        }

        if (count == 0)
        {
            var emptyTxt = UIFactory.Text("Empty", "No transaction history recorded for this save slot.", _listContent, UITheme.Sp(11), TextAnchor.MiddleCenter, FontStyle.Italic);
            emptyTxt.color = UITheme.TextDim;
            return;
        }

        for (int i = 0; i < count; i++)
        {
            BuildTransactionItem(_listContent, list[i]);
        }
    }

    private void BuildTransactionItem(Transform parent, BankTransaction tx)
    {
        var card = UIFactory.Panel($"Tx_{tx.Id}", parent, UITheme.CardBg);
        var cLE = card.AddComponent<LayoutElement>();
        cLE.preferredHeight = UITheme.Dp(40f);
        cLE.minHeight = UITheme.Dp(40f);

        var hlg = card.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), 0, 0);
        hlg.childAlignment = TextAnchor.MiddleCenter;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;

        // Type Icon Pill
        string iconStr = tx.Type switch
        {
            TransactionType.Deposit => "⬇",
            TransactionType.Withdrawal => "⬆",
            TransactionType.Fee => "🧾",
            _ => "⇄"
        };
        Color iconColor = tx.Type == TransactionType.Deposit ? UITheme.AccentGreen : UITheme.AccentOrange;

        var iconTxt = UIFactory.Text("Icon", iconStr, card.transform, UITheme.Sp(12), TextAnchor.MiddleCenter, FontStyle.Bold);
        iconTxt.color = iconColor;
        iconTxt.GetComponent<LayoutElement>().preferredWidth = UITheme.Dp(18f);

        // Left Col (Description & Date)
        var leftCol = UIFactory.Panel("LeftCol", card.transform, Color.clear);
        var lVlg = leftCol.AddComponent<VerticalLayoutGroup>();
        lVlg.spacing = 0f;
        lVlg.childControlWidth = true;
        lVlg.childControlHeight = true;
        lVlg.childForceExpandWidth = true;
        lVlg.childForceExpandHeight = false;

        var desc = UIFactory.Text("Desc", tx.Description, leftCol.transform, UITheme.Sp(10), TextAnchor.MiddleLeft, FontStyle.Bold);
        desc.color = UITheme.TextPrimary;

        var time = UIFactory.Text("Time", $"Day {tx.InGameDay} • {tx.InGameTime}", leftCol.transform, UITheme.Sp(8), TextAnchor.MiddleLeft);
        time.color = UITheme.TextDim;

        // Right Col (Amount & Balance)
        var rightCol = UIFactory.Panel("RightCol", card.transform, Color.clear);
        var rVlg = rightCol.AddComponent<VerticalLayoutGroup>();
        rVlg.spacing = 0f;
        rVlg.childControlWidth = true;
        rVlg.childControlHeight = true;
        rVlg.childForceExpandWidth = true;
        rVlg.childForceExpandHeight = false;

        bool isPos = tx.Amount >= 0f;
        string sign = isPos ? "+" : "-";
        float absAmt = Mathf.Abs(tx.Amount);

        var amt = UIFactory.Text("Amt", $"{sign}$ {absAmt:N2}", rightCol.transform, UITheme.Sp(10), TextAnchor.MiddleRight, FontStyle.Bold);
        amt.color = isPos ? UITheme.AccentGreen : UITheme.AccentOrange;

        var bal = UIFactory.Text("Bal", $"Bal: $ {tx.BalanceAfter:N2}", rightCol.transform, UITheme.Sp(8), TextAnchor.MiddleRight);
        bal.color = UITheme.TextDim;
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
