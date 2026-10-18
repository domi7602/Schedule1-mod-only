using System;
using System.Collections.Generic;
using System.IO;
using BankApp.Config;
using BankApp.Logic;
using BankApp.Models;
using BankApp.Services;
using BankApp.UI;
using MelonLoader;
using MelonLoader.Utils;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UITheme = S1Mods.Shared.UITheme;
using UnityEngine;
using UnityEngine.UI;

namespace BankApp;

public enum TransferMode
{
    Deposit,
    Withdraw
}

/// <summary>
/// Mobile Banking smartphone app for Schedule I.
/// <para>
/// One screen, top to bottom: header (app name, in-game day and clock) → balance hero
/// (online balance + cash on hand) → Deposit/Withdraw mode switcher → guarded amount field
/// with quick-amount chips → live fee / balance-after preview (plus the weekly-limit line
/// while depositing) → the primary action → the newest five transactions grouped by day.
/// Everything below the header lives in a single scroll helper, so the page scrolls as a
/// whole and no code path ever opens a second screen.
/// </para>
/// All money figures come from the shared <see cref="TransferMath"/>
/// seam so the preview, the MAX chip and <c>BankService</c> agree exactly.
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

    // Balances
    private Text _cashText = null!;
    private Text _onlineText = null!;

    // Header meta (in-game day + clock)
    private Text _headerMetaText = null!;

    // Mode switcher
    private Image _depositModeBg = null!;
    private Text _depositModeText = null!;
    private Image _withdrawModeBg = null!;
    private Text _withdrawModeText = null!;

    // Amount (guarded direct input)
    private InputField _amountInput = null!;
    private BankAppInputFocus? _inputFocus;
    private bool _suppressInput;

    // Preview
    private Text _previewFeeText = null!;
    private Text _previewBalanceText = null!;
    private GameObject _previewFeeRow = null!;
    private GameObject _weeklyRow = null!;
    private Text _weeklyRemainingText = null!;
    private Text _weeklyResetText = null!;

    // Recent transactions: fixed slots, updated in place on every refresh
    private const int RecentRowCount = 5;
    private readonly GameObject[] _recentRows = new GameObject[RecentRowCount];
    private readonly Text[] _recentWhenTexts = new Text[RecentRowCount];
    private readonly Text[] _recentKindTexts = new Text[RecentRowCount];
    private readonly Text[] _recentAmountTexts = new Text[RecentRowCount];
    private Text _recentEmptyText = null!;

    // Action
    private Image _confirmBtnBg = null!;
    private Text _confirmBtnText = null!;
    private Button _confirmButton = null!;

    private Text _reasonText = null!;
    private Text _feedbackText = null!;

    private TransferMode _mode = TransferMode.Deposit;
    private float _enteredAmount;
    private float _lastRefreshTime;
    private TransferQuote _quote;
    private float _feedbackExpiry;

    // Fix (Bug-Audit 2026-09-10): the phone re-instantiates this app per scene load and the old
    // per-instance handlers stayed in the static invocation lists forever. Static events now
    // dispatch through _active, subscribed exactly once; Mod.OnSceneWasUnloaded clears _active
    // when the gameplay scene tears down.
    private static BankApp? _active;
    private static bool _staticSubscribed;

    protected override void OnCreated()
    {
        base.OnCreated();
        _active = this;
        if (_staticSubscribed) return;
        _staticSubscribed = true;
        MelonEvents.OnUpdate.Subscribe(DispatchUpdate);
        S1API.Money.Money.OnBalanceChanged += DispatchBalanceChanged;
        TransactionHistoryService.OnHistoryChanged += DispatchHistoryChanged;
        MelonLogger.Msg("Registered with S1API PhoneApp system (v0.5.0).");
    }

    internal static void TearDownForSceneUnload()
    {
        _active = null;
        _cachedIcon = null;
    }

    private static void DispatchUpdate()
    {
        var a = _active;
        if (a != null)
        {
            try { a.Update(); } catch { }
        }
    }

    private static void DispatchBalanceChanged()
    {
        var a = _active;
        if (a != null)
        {
            try { a.OnExternalBalanceChanged(); } catch { }
        }
    }

    private static void DispatchHistoryChanged()
    {
        var a = _active;
        if (a != null)
        {
            try { a.OnHistoryChanged(); } catch { }
        }
    }

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.Initialize(containerRt);
        }

        _mainBG = UIFactory.Panel("BankApp_MainBG", container.transform, BankTheme.BgDark, fullAnchor: true);
        _mainBG.SetActive(false);

        var vlg = _mainBG.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(6f);
        vlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), (int)UITheme.Dp(8f), (int)UITheme.Dp(10f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        BuildHeader(_mainBG.transform);

        // The single page: one scroll helper holds every section below the header.
        RectTransform content = BuildScroll("BankAppScroll", _mainBG.transform, out _, out _);
        BuildSingleScreen(content);

        _inputFocus = _mainBG.AddComponent<BankAppInputFocus>();
        _inputFocus.amountInput = _amountInput;

        RefreshAll();
    }

    // --- Header -----------------------------------------------------

    private void BuildHeader(Transform parent)
    {
        var header = UIFactory.Panel("Header", parent, BankTheme.HeaderBg);
        SetHeight(header, UITheme.Dp(38f));
        var hlg = header.AddComponent<HorizontalLayoutGroup>();
        hlg.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), 0, 0);
        hlg.spacing = UITheme.Dp(8f);
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;

        var title = UIFactory.Text("HeaderTitle", "BankApp", header.transform, UITheme.Sp(16), TextAnchor.MiddleLeft, FontStyle.Bold);
        title.color = BankTheme.TextPrimary;
        title.raycastTarget = false;
        var le = title.gameObject.AddComponent<LayoutElement>();
        le.flexibleWidth = 1f;

        _headerMetaText = UIFactory.Text("HeaderMeta", string.Empty, header.transform, UITheme.Sp(12), TextAnchor.MiddleRight);
        _headerMetaText.color = BankTheme.TextMuted;
        _headerMetaText.raycastTarget = false;
        var mle = _headerMetaText.gameObject.AddComponent<LayoutElement>();
        mle.flexibleWidth = 1f;
    }

    private void UpdateHeader()
    {
        if (!IsAlive(_headerMetaText)) return;
        _headerMetaText.text = $"Day {BankService.GetCurrentInGameDay()} \u2022 {BankService.GetCurrentInGameTimeString()}";
    }

    // --- Single page ----------------------------------------------------------

    /// <summary>
    /// The whole screen in reading order: balance hero, mode switcher, amount (field + chips),
    /// preview, primary action, then the recent-activity trail. There is no second pane — the
    /// mode switcher and the chips only change state, they never navigate.
    /// </summary>
    private void BuildSingleScreen(Transform content)
    {
        BuildBalanceCard(content);
        BuildModeSwitcher(content);
        BuildAmountCard(content);
        BuildPreviewCard(content);
        BuildConfirmButton(content);
        _feedbackText = UIFactory.Text("FeedbackText", "", content, UITheme.Sp(12), TextAnchor.MiddleCenter, FontStyle.Bold);
        _feedbackText.color = BankTheme.AccentGreen;
        _feedbackText.horizontalOverflow = HorizontalWrapMode.Wrap;
        SetHeight(_feedbackText.gameObject, UITheme.Dp(20f));
        BuildRecentCard(content);
    }

    /// <summary>
    /// Balance hero: a large accent-blue card whose dominant number is the online balance, with the
    /// cash-on-hand line beneath — sized to ~150dp so it reads as the page's focal point.
    /// </summary>
    private void BuildBalanceCard(Transform parent)
    {
        var card = BuildCard(parent, "BalanceCard", UITheme.Dp(150f), Color.Lerp(BankTheme.BgDark, BankTheme.AccentBlue, 0.40f));
        var vlg = Stack(card, UITheme.Dp(8f));
        vlg.childAlignment = TextAnchor.MiddleLeft;

        var caption = SectionLabel(card.transform, "Online balance");
        caption.color = new Color(1f, 1f, 1f, 0.72f);

        _onlineText = UIFactory.Text("OnlineBalance", "$ 0", card.transform, UITheme.Sp(38), TextAnchor.MiddleLeft, FontStyle.Bold);
        _onlineText.color = Color.white;
        _onlineText.raycastTarget = false;
        SetHeight(_onlineText.gameObject, UITheme.Dp(48f));

        var row = Row(card.transform, UITheme.Dp(22f));
        var rowHlg = row.AddComponent<HorizontalLayoutGroup>();
        rowHlg.childControlWidth = true;
        rowHlg.childControlHeight = true;
        rowHlg.childForceExpandWidth = false;
        rowHlg.childForceExpandHeight = true;

        var caption2 = UIFactory.Text("CashCaption", "Cash on hand", row.transform, UITheme.Sp(13), TextAnchor.MiddleLeft);
        caption2.color = new Color(1f, 1f, 1f, 0.70f);
        caption2.raycastTarget = false;
        var cle = caption2.gameObject.AddComponent<LayoutElement>();
        cle.flexibleWidth = 1f;

        _cashText = UIFactory.Text("CashBalance", "$ 0", row.transform, UITheme.Sp(16), TextAnchor.MiddleRight, FontStyle.Bold);
        _cashText.color = Color.white;
        _cashText.raycastTarget = false;
        var vle = _cashText.gameObject.AddComponent<LayoutElement>();
        vle.flexibleWidth = 1f;
    }

    /// <summary>
    /// Mode switcher: two equal-width, 44dp-tall Deposit / Withdraw buttons. Switching only
    /// changes the mode, the action label and the preview — it never navigates anywhere.
    /// </summary>
    private void BuildModeSwitcher(Transform parent)
    {
        var row = Row(parent, UITheme.Dp(44f));
        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(6f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        BuildModeButton(row.transform, "Deposit", TransferMode.Deposit, out _depositModeBg, out _depositModeText);
        BuildModeButton(row.transform, "Withdraw", TransferMode.Withdraw, out _withdrawModeBg, out _withdrawModeText);
    }

    private void BuildModeButton(Transform parent, string label, TransferMode mode, out Image bg, out Text text)
    {
        var go = UIFactory.Panel("Mode_" + label, parent, BankTheme.CardBg);
        var img = go.GetComponent<Image>();
        img.sprite = UISprites.Rounded(8f, 32);
        img.type = Image.Type.Sliced;

        var button = go.AddComponent<Button>();
        button.targetGraphic = img;
        ButtonUtils.AddListener(button, () => SetMode(mode));

        text = UIFactory.Text("Label", label, go.transform, UITheme.Sp(14), TextAnchor.MiddleCenter, FontStyle.Bold);
        text.color = BankTheme.TextPrimary;
        text.raycastTarget = false;
        Stretch(text.rectTransform, 0f);
        bg = img;
    }

    private void BuildAmountCard(Transform parent)
    {
        var card = BuildCard(parent, "AmountCard", UITheme.Dp(92f));
        Stack(card, UITheme.Dp(6f));
        SectionLabel(card.transform, "AMOUNT");

        var fieldRow = Row(card.transform, UITheme.Dp(52f));
        var hlg = fieldRow.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;

        var field = UIFactory.Panel("AmountField", fieldRow.transform, BankTheme.CardBgSecondary);
        var fimg = field.GetComponent<Image>();
        fimg.sprite = UISprites.Rounded(8f, 32);
        fimg.type = Image.Type.Sliced;
        var fle = field.AddComponent<LayoutElement>();
        fle.flexibleWidth = 1f;
        fle.minHeight = UITheme.Dp(52f);

        _amountInput = field.AddComponent<InputField>();
        _amountInput.targetGraphic = fimg;
        _amountInput.lineType = InputField.LineType.SingleLine;
        _amountInput.contentType = InputField.ContentType.DecimalNumber;
        _amountInput.characterValidation = InputField.CharacterValidation.Decimal;
        _amountInput.caretWidth = 2;
        _amountInput.caretColor = Color.white;
        _amountInput.selectionColor = new Color(0.23f, 0.51f, 0.96f, 0.45f);

        var text = UIFactory.Text("Text", "", field.transform, UITheme.Sp(28), TextAnchor.MiddleLeft, FontStyle.Bold);
        text.color = BankTheme.TextPrimary;
        text.raycastTarget = false;
        Stretch(text.rectTransform, UITheme.Dp(12f));

        var placeholder = UIFactory.Text("Placeholder", "0", field.transform, UITheme.Sp(28), TextAnchor.MiddleLeft);
        placeholder.color = BankTheme.TextMuted;
        placeholder.raycastTarget = false;
        Stretch(placeholder.rectTransform, UITheme.Dp(12f));

        _amountInput.textComponent = text;
        _amountInput.placeholder = placeholder;
        EventHelper.AddListener<string>(OnAmountInputChanged, _amountInput.onValueChanged);

        var clear = UIFactory.Panel("ClearButton", fieldRow.transform, BankTheme.CardBg);
        var cimg = clear.GetComponent<Image>();
        cimg.sprite = UISprites.Rounded(8f, 32);
        cimg.type = Image.Type.Sliced;
        var cle = clear.AddComponent<LayoutElement>();
        cle.preferredWidth = UITheme.Dp(64f);
        cle.minWidth = UITheme.Dp(64f);
        var cbtn = clear.AddComponent<Button>();
        cbtn.targetGraphic = cimg;
        ButtonUtils.AddListener(cbtn, ClearAmount);
        var clabel = UIFactory.Text("Label", "CLEAR", clear.transform, UITheme.Sp(12), TextAnchor.MiddleCenter, FontStyle.Bold);
        clabel.color = BankTheme.TextMuted;
        clabel.raycastTarget = false;
        Stretch(clabel.rectTransform, 0f);

        BuildQuickChips(card.transform);
    }

    /// <summary>
    /// Quick-amount chips directly under the amount field: $100 / $500 / $1,000 write the shared
    /// fixed presets into the field, MAX writes the largest amount the current mode allows
    /// (<see cref="BankService.GetMaxDepositableCash"/> / <see cref="BankService.GetMaxWithdrawableCash"/>).
    /// </summary>
    private void BuildQuickChips(Transform parent)
    {
        BuildChipRow(parent,
            ("$100", (Action)(() => SetAmount(TransferMath.AmountPresets[0]))),
            ("$500", () => SetAmount(TransferMath.AmountPresets[1])),
            ("$1,000", () => SetAmount(TransferMath.AmountPresets[2])),
            ("MAX", () => SetAmount(CurrentAllowedMax())));
    }

    private void BuildChipRow(Transform parent, params (string label, Action onClick)[] chips)
    {
        var row = Row(parent, UITheme.Dp(44f));
        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(6f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        foreach (var chip in chips)
        {
            Color? accent = chip.label == "MAX" ? BankTheme.AccentBlue : null;
            BuildChip(row.transform, chip.label, chip.onClick, accent);
        }
    }

    private static void BuildChip(Transform parent, string label, Action onClick, Color? accent = null)
    {
        var go = UIFactory.Panel("Chip_" + label, parent, accent ?? BankTheme.CardBg);
        var img = go.GetComponent<Image>();
        img.sprite = UISprites.Rounded(8f, 32);
        img.type = Image.Type.Sliced;
        var button = go.AddComponent<Button>();
        button.targetGraphic = img;
        ButtonUtils.AddListener(button, onClick);
        var text = UIFactory.Text("Label", label, go.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        text.color = BankTheme.TextPrimary;
        text.raycastTarget = false;
        Stretch(text.rectTransform, 0f);
    }

    private void BuildPreviewCard(Transform parent)
    {
        var card = BuildCard(parent, "PreviewCard", UITheme.Dp(92f));
        Stack(card, UITheme.Dp(4f));
        SectionLabel(card.transform, "PREVIEW");

        _previewFeeRow = BuildStatRow(card.transform, "Fee", BankTheme.AccentOrange, out _previewFeeText, out _);
        BuildStatRow(card.transform, "Balance after", BankTheme.AccentTeal, out _previewBalanceText, out _);
        BuildWeeklyBlock(card.transform);

        _reasonText = UIFactory.Text("Reason", "", card.transform, UITheme.Sp(12), TextAnchor.MiddleLeft);
        _reasonText.color = BankTheme.AccentRed;
        _reasonText.horizontalOverflow = HorizontalWrapMode.Wrap;
        _reasonText.raycastTarget = false;
        SetHeight(_reasonText.gameObject, UITheme.Dp(18f));
    }

    /// <summary>
    /// Weekly ATM limit line, shown only while depositing with the vanilla limit enabled:
    /// the remaining deposit room plus the reset countdown from
    /// <see cref="TransferMath.DaysUntilWeeklyReset"/>.
    /// </summary>
    private void BuildWeeklyBlock(Transform parent)
    {
        var block = UIFactory.Panel("WeeklyBlock", parent, new Color(0f, 0f, 0f, 0f));
        NoRaycast(block);
        var vlg = block.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(2f);
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        _weeklyRow = block;

        var row = Row(block.transform, UITheme.Dp(19f));
        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;

        var label = UIFactory.Text("WeeklyRemainingLabel", "Weekly limit left", row.transform, UITheme.Sp(12), TextAnchor.MiddleLeft);
        label.color = BankTheme.TextMuted;
        label.raycastTarget = false;
        var lle = label.gameObject.AddComponent<LayoutElement>();
        lle.flexibleWidth = 1f;

        _weeklyRemainingText = UIFactory.Text("WeeklyRemaining", "\u2014", row.transform, UITheme.Sp(13), TextAnchor.MiddleRight, FontStyle.Bold);
        _weeklyRemainingText.color = BankTheme.AccentBlue;
        _weeklyRemainingText.raycastTarget = false;
        var vle = _weeklyRemainingText.gameObject.AddComponent<LayoutElement>();
        vle.flexibleWidth = 1f;

        _weeklyResetText = UIFactory.Text("WeeklyReset", "", block.transform, UITheme.Sp(11), TextAnchor.MiddleLeft);
        _weeklyResetText.color = BankTheme.TextDim;
        _weeklyResetText.raycastTarget = false;
        SetHeight(_weeklyResetText.gameObject, UITheme.Dp(16f));
    }

    private static GameObject BuildStatRow(Transform parent, string label, Color valueColor, out Text valueText, out Text labelText)
    {
        var row = Row(parent, UITheme.Dp(19f));
        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = true;

        labelText = UIFactory.Text("Label", label, row.transform, UITheme.Sp(12), TextAnchor.MiddleLeft);
        labelText.color = BankTheme.TextMuted;
        labelText.raycastTarget = false;
        var lle = labelText.gameObject.AddComponent<LayoutElement>();
        lle.flexibleWidth = 1f;

        valueText = UIFactory.Text("Value", "\u2014", row.transform, UITheme.Sp(13), TextAnchor.MiddleRight, FontStyle.Bold);
        valueText.color = valueColor;
        valueText.raycastTarget = false;
        var vle = valueText.gameObject.AddComponent<LayoutElement>();
        vle.flexibleWidth = 1f;

        return row;
    }

    private static void BuildStatRow(Transform parent, string label, Color valueColor, out Text valueText)
        => BuildStatRow(parent, label, valueColor, out valueText, out _);

    private void BuildConfirmButton(Transform parent)
    {
        var go = UIFactory.Panel("ConfirmButton", parent, BankTheme.AccentGreen);
        SetHeight(go, UITheme.Dp(52f));
        _confirmBtnBg = go.GetComponent<Image>();
        _confirmBtnBg.sprite = UISprites.Rounded(10f, 32);
        _confirmBtnBg.type = Image.Type.Sliced;

        _confirmButton = go.AddComponent<Button>();
        _confirmButton.targetGraphic = _confirmBtnBg;
        ButtonUtils.AddListener(_confirmButton, ExecuteTransaction);

        _confirmBtnText = UIFactory.Text("Label", "DEPOSIT", go.transform, UITheme.Sp(16), TextAnchor.MiddleCenter, FontStyle.Bold);
        _confirmBtnText.color = Color.white;
        _confirmBtnText.raycastTarget = false;
        Stretch(_confirmBtnText.rectTransform, 0f);
    }

    /// <summary>
    /// Receipt trail at the bottom of the page: the newest <see cref="RecentRowCount"/> entries,
    /// one row each (day/time, direction, signed amount). Rows are built once and refreshed in place.
    /// </summary>
    private void BuildRecentCard(Transform parent)
    {
        var card = BuildCard(parent, "RecentCard", UITheme.Dp(80f));
        Stack(card, UITheme.Dp(4f));
        SectionLabel(card.transform, "RECENT");

        _recentEmptyText = UIFactory.Text("RecentEmpty", "No transactions yet", card.transform, UITheme.Sp(12), TextAnchor.MiddleLeft);
        _recentEmptyText.color = BankTheme.TextDim;
        _recentEmptyText.raycastTarget = false;
        SetHeight(_recentEmptyText.gameObject, UITheme.Dp(18f));

        for (int i = 0; i < RecentRowCount; i++)
        {
            var row = Row(card.transform, UITheme.Dp(20f));
            var hlg = row.AddComponent<HorizontalLayoutGroup>();
            hlg.spacing = UITheme.Dp(8f);
            hlg.childControlWidth = true;
            hlg.childControlHeight = true;
            hlg.childForceExpandWidth = true;
            hlg.childForceExpandHeight = true;

            _recentWhenTexts[i] = RecentCell(row.transform, "When", TextAnchor.MiddleLeft, BankTheme.TextMuted, 11);
            _recentKindTexts[i] = RecentCell(row.transform, "Kind", TextAnchor.MiddleCenter, BankTheme.TextPrimary, 12);
            _recentAmountTexts[i] = RecentCell(row.transform, "Amount", TextAnchor.MiddleRight, BankTheme.TextPrimary, 12);
            _recentRows[i] = row;
            row.SetActive(false);
        }
    }

    private static Text RecentCell(Transform parent, string name, TextAnchor anchor, Color color, int fontSize)
    {
        var text = UIFactory.Text(name, string.Empty, parent, UITheme.Sp(fontSize), anchor, name == "Amount" ? FontStyle.Bold : FontStyle.Normal);
        text.color = color;
        text.raycastTarget = false;
        var le = text.gameObject.AddComponent<LayoutElement>();
        le.flexibleWidth = 1f;
        return text;
    }

    private void UpdateRecent()
    {
        if (!IsAlive(_recentEmptyText)) return;

        IReadOnlyList<BankTransaction> history = TransactionHistoryService.GetTransactions();
        int count = Math.Min(RecentRowCount, history.Count);

        // Only the newest slice is grouped; the stored list is already newest-first.
        var latest = new List<BankTransaction>(count);
        for (int i = 0; i < count; i++) latest.Add(history[i]);

        int currentDay = BankService.GetCurrentInGameDay();
        int slot = 0;
        foreach (HistoryDayGroup group in HistoryGrouping.GroupByDay(latest, currentDay))
        {
            foreach (BankTransaction tx in group.Items)
            {
                SetRecentRow(slot, tx, group.Label);
                slot++;
            }
        }

        for (int i = 0; i < RecentRowCount; i++)
        {
            if (IsAlive(_recentRows[i])) _recentRows[i].SetActive(i < slot);
        }
        _recentEmptyText.gameObject.SetActive(slot == 0);
    }

    private void SetRecentRow(int slot, BankTransaction tx, string dayLabel)
    {
        _recentWhenTexts[slot].text = $"{dayLabel} {tx.InGameTime}";
        _recentKindTexts[slot].text = DirectionLabel(tx.Type);
        _recentAmountTexts[slot].text = (tx.Amount < 0f ? "-" : "+") + FormatMoney(Math.Abs(tx.Amount));
        _recentAmountTexts[slot].color = tx.Amount < 0f ? BankTheme.AccentOrange : BankTheme.AccentGreen;
    }

    private static string DirectionLabel(TransactionType type) => type switch
    {
        TransactionType.Deposit => "Deposit",
        TransactionType.Withdrawal => "Withdraw",
        TransactionType.TransferIn => "Transfer in",
        TransactionType.TransferOut => "Transfer out",
        TransactionType.Fee => "Fee",
        _ => type.ToString()
    };

    // --- State changes -----------------------------------------------------

    private void SetMode(TransferMode mode)
    {
        _mode = mode;
        BankSoundService.PlayClick();
        UpdateModeVisuals();
        UpdatePreviewAndAction(ModConfig<BankAppConfig>.Instance);
        UpdateWeekly(ModConfig<BankAppConfig>.Instance);
    }

    private void SetAmount(float value)
    {
        _enteredAmount = TransferMath.IsFinite(value) && value > 0f ? value : 0f;
        SetAmountText(TransferMath.FormatAmountInput(_enteredAmount));
        UpdatePreviewAndAction(ModConfig<BankAppConfig>.Instance);
    }

    private void ClearAmount()
    {
        _enteredAmount = 0f;
        SetAmountText(string.Empty);
        SetFeedback(string.Empty, false);
        UpdatePreviewAndAction(ModConfig<BankAppConfig>.Instance);
    }

    private void OnAmountInputChanged(string raw)
    {
        if (_suppressInput) return;

        _enteredAmount = TransferMath.TryParseAmount(raw, out float parsed) ? parsed : 0f;
        if (IsAlive(_feedbackText)) _feedbackText.text = string.Empty;
        UpdatePreviewAndAction(ModConfig<BankAppConfig>.Instance);
    }

    private void ExecuteTransaction()
    {
        var config = ModConfig<BankAppConfig>.Instance;
        TransferQuote quote = ComputeCurrentQuote(config);

        if (!quote.IsValid)
        {
            SetFeedback(quote.ErrorMessage, true);
            BankSoundService.PlayError();
            RefreshAll();
            return;
        }

        bool success;
        string error;
        try
        {
            success = _mode == TransferMode.Deposit
                ? BankService.DepositCash(_enteredAmount, out error)
                : BankService.WithdrawCash(_enteredAmount, out error);
        }
        catch (Exception ex)
        {
            success = false;
            error = $"Transaction failed: {ex.Message}";
        }

        if (success)
        {
            string verb = _mode == TransferMode.Deposit ? "Deposited" : "Withdrew";
            SetFeedback($"{verb} {FormatMoney(_enteredAmount)}.", false);
            BankSoundService.PlayCashSuccess();
            _enteredAmount = 0f;
            SetAmountText(string.Empty);
        }
        else
        {
            SetFeedback(error, true);
            BankSoundService.PlayError();
        }

        RefreshAll();
    }

    // --- Refresh / visuals -------------------------------------------------

    private float CurrentAllowedMax() =>
        _mode == TransferMode.Deposit ? BankService.GetMaxDepositableCash() : BankService.GetMaxWithdrawableCash();

    private TransferQuote ComputeCurrentQuote(BankAppConfig config)
    {
        bool limitEnabled = config.RespectVanillaAtmLimit;
        float remainingLimit = limitEnabled ? BankService.GetRemainingWeeklyAtmLimit() : float.MaxValue;
        return TransferMath.ComputeQuote(
            _mode == TransferMode.Deposit ? TransferDirection.Deposit : TransferDirection.Withdraw,
            _enteredAmount,
            BankService.GetCashBalance(),
            BankService.GetOnlineBalance(),
            config.ServiceFeePercent,
            limitEnabled,
            remainingLimit);
    }

    private void SetAmountText(string text)
    {
        if (!IsAlive(_amountInput)) return;
        _suppressInput = true;
        _amountInput.text = text;
        _suppressInput = false;
    }

    private void UpdateModeVisuals()
    {
        bool deposit = _mode == TransferMode.Deposit;
        if (_depositModeBg != null) _depositModeBg.color = deposit ? BankTheme.AccentGreen : BankTheme.CardBg;
        if (_withdrawModeBg != null) _withdrawModeBg.color = deposit ? BankTheme.CardBg : BankTheme.AccentOrange;
        if (_depositModeText != null) _depositModeText.color = deposit ? Color.white : BankTheme.TextMuted;
        if (_withdrawModeText != null) _withdrawModeText.color = deposit ? BankTheme.TextMuted : Color.white;
    }

    private void UpdatePreviewAndAction(BankAppConfig config)
    {
        _quote = ComputeCurrentQuote(config);
        bool valid = _quote.IsValid;

        if (IsAlive(_previewFeeText))
            _previewFeeText.text = valid
                ? $"{FormatMoney(_quote.Fee)} ({TransferMath.SanitizeFeePercent(config.ServiceFeePercent):0.#}%)"
                : "\u2014";
        if (IsAlive(_previewFeeRow)) _previewFeeRow.SetActive(valid && _quote.Fee > 0f);
        if (IsAlive(_previewBalanceText)) _previewBalanceText.text = valid ? FormatMoney(_quote.BalanceAfter) : "\u2014";

        if (IsAlive(_reasonText))
        {
            bool show = !valid && _enteredAmount > 0f;
            _reasonText.gameObject.SetActive(show);
            if (show) _reasonText.text = _quote.ErrorMessage;
        }

        if (IsAlive(_confirmButton)) _confirmButton.interactable = valid;
        if (IsAlive(_confirmBtnBg))
            _confirmBtnBg.color = valid
                ? (_mode == TransferMode.Deposit ? GamePalette.Green : GamePalette.Orange)
                : GamePalette.CardAlt;
        if (IsAlive(_confirmBtnText))
        {
            string verb = _mode == TransferMode.Deposit ? "DEPOSIT" : "WITHDRAW";
            string label = _enteredAmount > 0f ? $"{verb} {FormatMoney(_enteredAmount)}" : verb;
            _confirmBtnText.text = label;
            _confirmBtnText.color = valid ? Color.white : BankTheme.TextDim;
        }
    }

    /// <summary>
    /// Weekly ATM limit line: remaining deposit room plus the reset countdown. Only relevant
    /// for deposits, and only while the vanilla weekly limit is respected — the line is hidden
    /// entirely otherwise.
    /// </summary>
    private void UpdateWeekly(BankAppConfig config)
    {
        bool show = config.RespectVanillaAtmLimit && _mode == TransferMode.Deposit;
        if (IsAlive(_weeklyRow)) _weeklyRow.SetActive(show);
        if (!show) return;

        float remaining = BankService.GetRemainingWeeklyAtmLimit();
        if (IsAlive(_weeklyRemainingText)) _weeklyRemainingText.text = FormatMoney(remaining);
        if (IsAlive(_weeklyResetText))
        {
            int daysUntilReset = TransferMath.DaysUntilWeeklyReset(BankService.GetCurrentInGameDay());
            _weeklyResetText.text = $"Resets in {daysUntilReset} days";
        }
    }

    private void SetFeedback(string text, bool isError)
    {
        if (!IsAlive(_feedbackText)) return;
        _feedbackText.text = text;
        _feedbackText.color = isError ? BankTheme.AccentRed : BankTheme.AccentGreen;
        // Success confirmations fade after ~2 s; errors persist until the next interaction.
        _feedbackExpiry = !isError && !string.IsNullOrEmpty(text) ? Time.unscaledTime + 2f : 0f;
    }

    private void RefreshAll()
    {
        if (!IsAlive(_mainBG)) return;

        var config = ModConfig<BankAppConfig>.Instance;

        UpdateHeader();
        if (IsAlive(_onlineText)) _onlineText.text = FormatMoney(BankService.GetOnlineBalance());
        if (IsAlive(_cashText)) _cashText.text = FormatMoney(BankService.GetCashBalance());

        UpdateWeekly(config);
        UpdateRecent();
        UpdateModeVisuals();
        UpdatePreviewAndAction(config);
    }

    // --- Shared UI helpers -------------------------------------------------

    private static RectTransform BuildScroll(string name, Transform parent, out ScrollRect scroll, out GameObject scrollRoot)
    {
        RectTransform content = UIFactory.ScrollableVerticalList(name, parent, out scroll);
        scrollRoot = scroll.gameObject;

        var le = scrollRoot.AddComponent<LayoutElement>();
        le.flexibleHeight = 1f;
        le.minHeight = UITheme.Dp(120f);

        // Fill the parent slot in both hosts (ignored where a layout group drives the rect).
        var scrollRect = scrollRoot.GetComponent<RectTransform>();
        scrollRect.anchorMin = Vector2.zero;
        scrollRect.anchorMax = Vector2.one;
        scrollRect.offsetMin = Vector2.zero;
        scrollRect.offsetMax = Vector2.zero;

        scroll.movementType = ScrollRect.MovementType.Clamped;
        scroll.scrollSensitivity = UITheme.Dp(28f);
        scroll.horizontal = false;

        content.anchorMin = new Vector2(0f, 1f);
        content.anchorMax = new Vector2(1f, 1f);
        content.pivot = new Vector2(0.5f, 1f);
        content.anchoredPosition = Vector2.zero;
        content.sizeDelta = Vector2.zero;

        var layout = content.GetComponent<VerticalLayoutGroup>();
        if (layout != null)
        {
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.padding = new RectOffset((int)UITheme.Dp(2f), (int)UITheme.Dp(2f), (int)UITheme.Dp(2f), (int)UITheme.Dp(8f));
            layout.spacing = UITheme.Dp(6f);
        }

        var fitter = content.GetComponent<ContentSizeFitter>();
        if (fitter != null)
        {
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        return content;
    }

    /// <summary>
    /// Card background sized to its contents: the min height is only a floor, so the card's own
    /// VerticalLayoutGroup can report the real preferred height and never clip stacked children.
    /// </summary>
    private static GameObject BuildCard(Transform parent, string name, float minHeight, Color? bg = null)
    {
        var card = UIFactory.Panel(name, parent, bg ?? BankTheme.CardBg);
        var img = card.GetComponent<Image>();
        img.sprite = UISprites.Rounded(10f, 32);
        img.type = Image.Type.Sliced;
        var le = card.AddComponent<LayoutElement>();
        le.minHeight = minHeight;
        // Cards size to content inside a scroll body; they must not report height flexibility.
        le.flexibleHeight = 0f;
        le.layoutPriority = 1;
        return card;
    }

    private static VerticalLayoutGroup Stack(GameObject card, float spacing)
    {
        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), (int)UITheme.Dp(8f), (int)UITheme.Dp(8f));
        vlg.spacing = spacing;
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        return vlg;
    }

    private static GameObject Row(Transform parent, float height)
    {
        var row = UIFactory.Panel("Row", parent, new Color(0f, 0f, 0f, 0f));
        NoRaycast(row);
        SetHeight(row, height);
        return row;
    }

    private static Text SectionLabel(Transform parent, string content)
    {
        var label = UIFactory.Text("Section_" + content, content, parent, UITheme.Sp(11), TextAnchor.MiddleLeft, FontStyle.Bold);
        label.color = BankTheme.TextMuted;
        label.raycastTarget = false;
        SetHeight(label.gameObject, UITheme.Dp(16f));
        return label;
    }

    /// <summary>
    /// Fixed-height band helper for the top-level chrome (header) and every fixed row.
    /// <para>
    /// Root fix for the "band swallows the screen" bug: a LayoutElement that leaves
    /// <c>flexibleHeight</c> unset (-1) stops reporting it, so the layout system falls through to a
    /// sibling layout group on the same GameObject. A HorizontalLayoutGroup with
    /// <c>childForceExpandHeight = true</c> promotes each child to flexible >= 1 and reports a
    /// positive flexible height, so the parent vertical group hands that band the lion's share of
    /// the spare space (header grew to ~140px on a 560px screen). Pinning
    /// <c>flexibleHeight = 0</c> with <c>layoutPriority = 1</c> makes this value win, so only the
    /// intended content viewport keeps positive height flexibility.
    /// </para>
    /// </summary>
    private static void SetHeight(GameObject go, float height)
    {
        var le = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
        le.preferredHeight = height;
        le.minHeight = height;
        le.flexibleHeight = 0f;
        le.layoutPriority = 1;
    }

    private static void SetWidth(GameObject go, float width)
    {
        var le = go.GetComponent<LayoutElement>() ?? go.AddComponent<LayoutElement>();
        le.preferredWidth = width;
        le.minWidth = width;
    }

    private static void NoRaycast(GameObject go)
    {
        var img = go.GetComponent<Image>();
        if (img != null) img.raycastTarget = false;
    }

    private static void Stretch(RectTransform rect, float padding)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(padding, 0f);
        rect.offsetMax = new Vector2(-padding, 0f);
    }

    private static void Stretch(RectTransform rect, float horizontalPadding, float verticalPadding)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(horizontalPadding, verticalPadding);
        rect.offsetMax = new Vector2(-horizontalPadding, -verticalPadding);
    }

    private static string FormatMoney(float value) => TransferMath.FormatMoney(value);

    private static bool IsAlive(UnityEngine.Object? obj)
    {
        if (obj == null) return false;
        try { return obj.Pointer != IntPtr.Zero && !obj.WasCollected && (UnityEngine.Object)obj != null; }
        catch { return false; }
    }

    // --- Lifecycle ---------------------------------------------------------

    private void Update()
    {
        bool open = IsOpen();
        if (IsAlive(_mainBG) && _mainBG.activeSelf != open)
        {
            _mainBG.SetActive(open);
            if (open)
            {
                RefreshAll();
            }
        }

        if (!open) return;

        // Expire success feedback (~2 s); errors clear on the next interaction instead.
        if (_feedbackExpiry > 0f && Time.unscaledTime >= _feedbackExpiry)
        {
            _feedbackExpiry = 0f;
            if (IsAlive(_feedbackText)) _feedbackText.text = string.Empty;
        }

        if (Time.unscaledTime - _lastRefreshTime > 1.5f)
        {
            _lastRefreshTime = Time.unscaledTime;
            RefreshAll();
        }
    }

    private void OnExternalBalanceChanged()
    {
        // Balance ticks refresh the displayed numbers.
        if (IsOpen()) RefreshAll();
    }

    private void OnHistoryChanged()
    {
        // Keep the weekly deposit counter current when persisted history changes.
        if (IsOpen()) RefreshAll();
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (IsAlive(_mainBG)) _mainBG.SetActive(false);
        _enteredAmount = 0f;
        _mode = TransferMode.Deposit;
        _feedbackExpiry = 0f;
        SetAmountText(string.Empty);
        SetFeedback(string.Empty, false);
        // Update bleibt lebenslang subscribed (defensives Unsubscribe-Subscribe in OnCreated).
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

        // 2. Procedural Fallback (Sapphire blue circle)
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
