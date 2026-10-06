using System;
using System.IO;
using BankApp.Config;
using BankApp.Logic;
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
/// Overview and Transaction panes built against the shared <see cref="GamePalette"/> +
/// <c>UISprites</c>: balances + weekly limit, Deposit/Withdraw modes, a guarded amount field with
/// fixed/relative presets and a live fee/net/debit/balance-after preview.
/// All money figures come from the shared <see cref="TransferMath"/>
/// seam so the preview, the MAX buttons and <c>BankService</c> agree exactly.
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
    private GameObject _contentRoot = null!;
    private GameObject _overviewRoot = null!;
    private RectTransform _overviewContent = null!;
    private ScrollRect _overviewScroll = null!;
    private GameObject _transactionRoot = null!;
    private RectTransform _transactionContent = null!;

    // Balances
    private Text _cashText = null!;
    private Text _onlineText = null!;

    // Weekly limit
    private Text _weeklyValueText = null!;
    private Text _weeklyRemainingText = null!;
    private Text _weeklyResetText = null!;
    private RectTransform _weeklyBarFill = null!;

    // Mode tabs
    private Image _depositTabBg = null!;
    private Text _depositTabText = null!;
    private Image _withdrawTabBg = null!;
    private Text _withdrawTabText = null!;

    // Amount (guarded direct input)
    private InputField _amountInput = null!;
    private BankAppInputFocus? _inputFocus;
    private bool _suppressInput;

    // Preview
    private Text _previewAmountText = null!;
    private Text _previewFeeText = null!;
    private Text _previewNetLabel = null!;
    private Text _previewNetText = null!;
    private Text _previewDebitLabel = null!;
    private Text _previewDebitText = null!;
    private Text _previewBalanceText = null!;
    private GameObject _previewFeeRow = null!;

    // Transaction pane header
    private Text _transactionTitleText = null!;

    // Action
    private Image _confirmBtnBg = null!;
    private Text _confirmBtnText = null!;
    private Button _confirmButton = null!;

    private Text _reasonText = null!;
    private Text _feedbackText = null!;

    private TransferMode _mode = TransferMode.Deposit;
    private BankTab _tab = BankNavigation.DefaultTab;
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
        MelonLogger.Msg("Registered with S1API PhoneApp system (v0.4.5).");
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

        // Content host: fills the remaining height; Overview and Transaction swap here.
        _contentRoot = UIFactory.Panel("ContentRoot", _mainBG.transform, new Color(0f, 0f, 0f, 0f));
        NoRaycast(_contentRoot);
        var contentLe = _contentRoot.AddComponent<LayoutElement>();
        contentLe.flexibleHeight = 1f;
        contentLe.minHeight = UITheme.Dp(200f);

        _overviewContent = BuildScroll("OverviewScroll", _contentRoot.transform, out _overviewScroll, out _overviewRoot);
        BuildOverview(_overviewContent);

        _transactionRoot = BuildTransactionPane(_contentRoot.transform, out _transactionContent);

        _inputFocus = _mainBG.AddComponent<BankAppInputFocus>();
        _inputFocus.amountInput = _amountInput;

        SetTab(BankNavigation.DefaultTab, playSound: false);
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

        var icon = UIFactory.Text("HeaderIcon", "$", header.transform, UITheme.Sp(18), TextAnchor.MiddleCenter, FontStyle.Bold);
        icon.color = BankTheme.AccentBlue;
        icon.raycastTarget = false;
        SetWidth(icon.gameObject, UITheme.Dp(22f));

        var title = UIFactory.Text("HeaderTitle", "National Bank", header.transform, UITheme.Sp(16), TextAnchor.MiddleLeft, FontStyle.Bold);
        title.color = BankTheme.TextPrimary;
        title.raycastTarget = false;
        var le = title.gameObject.AddComponent<LayoutElement>();
        le.flexibleWidth = 1f;
    }

    // --- Overview ----------------------------------------------------------

    private void BuildOverview(Transform content)
    {
        BuildBalanceCard(content);
        BuildActionsCard(content);
        BuildWeeklyCard(content);
    }

    /// <summary>
    /// Overview quick actions: two large, equal-width Deposit / Withdraw tiles (green / orange),
    /// placed directly under the balance hero and above the weekly card to match the mockup.
    /// Each tile opens the transaction pane pre-set to that direction.
    /// </summary>
    private void BuildActionsCard(Transform parent)
    {
        var card = BuildCard(parent, "ActionsCard", UITheme.Dp(112f));
        Stack(card, UITheme.Dp(8f));
        SectionLabel(card.transform, "QUICK ACTIONS");

        var row = Row(card.transform, UITheme.Dp(84f));
        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;   // both tiles share the row equally
        hlg.childForceExpandHeight = true;

        BuildActionButton(row.transform, "Deposit", TransferMode.Deposit, "CASH \u2192 BANK");
        BuildActionButton(row.transform, "Withdraw", TransferMode.Withdraw, "BANK \u2192 CASH");
    }

    private void BuildActionButton(Transform parent, string label, TransferMode mode, string caption)
    {
        Color accent = mode == TransferMode.Deposit ? BankTheme.AccentGreen : BankTheme.AccentOrange;
        var go = UIFactory.Panel("Action_" + label, parent, accent);
        var img = go.GetComponent<Image>();
        img.sprite = UISprites.Rounded(10f, 32);
        img.type = Image.Type.Sliced;

        var button = go.AddComponent<Button>();
        button.targetGraphic = img;
        ButtonUtils.AddListener(button, () => OpenTransaction(mode));

        var vlg = go.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(2f);
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        var text = UIFactory.Text("Label", label.ToUpperInvariant(), go.transform, UITheme.Sp(16), TextAnchor.MiddleCenter, FontStyle.Bold);
        text.color = Color.white;
        text.raycastTarget = false;
        SetHeight(text.gameObject, UITheme.Dp(20f));

        var sub = UIFactory.Text("Caption", caption, go.transform, UITheme.Sp(10), TextAnchor.MiddleCenter);
        sub.color = new Color(1f, 1f, 1f, 0.85f);
        sub.raycastTarget = false;
        SetHeight(sub.gameObject, UITheme.Dp(14f));
    }

    // --- Transaction pane (Deposit / Withdraw drill-in) --------------------

    /// <summary>
    /// The transaction screen opened from an Overview action. Builds a compact fixed back/title head,
    /// a flexible scrolling body (mode toggle, dominant amount field, 3x2 preset grid, unified
    /// preview) and a fixed footer holding the primary action so it stays at the bottom of the pane.
    /// </summary>
    private GameObject BuildTransactionPane(Transform parent, out RectTransform scrollContent)
    {
        var pane = UIFactory.Panel("TransactionPane", parent, new Color(0f, 0f, 0f, 0f), fullAnchor: true);
        var vlg = pane.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(6f);
        vlg.padding = new RectOffset((int)UITheme.Dp(2f), (int)UITheme.Dp(2f), (int)UITheme.Dp(2f), (int)UITheme.Dp(6f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // Compact drill-in head (fixed): back + title.
        var head = Row(pane.transform, UITheme.Dp(38f));
        var hlg = head.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childAlignment = TextAnchor.MiddleLeft;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight = false;

        var back = UIFactory.Panel("TransBackButton", head.transform, BankTheme.CardBg);
        var bimg = back.GetComponent<Image>();
        bimg.sprite = UISprites.Rounded(8f, 32);
        bimg.type = Image.Type.Sliced;
        var ble = back.AddComponent<LayoutElement>();
        ble.preferredWidth = UITheme.Dp(72f);
        ble.minWidth = UITheme.Dp(72f);
        SetHeight(back, UITheme.Dp(30f));
        var bbtn = back.AddComponent<Button>();
        bbtn.targetGraphic = bimg;
        ButtonUtils.AddListener(bbtn, () => SetTab(BankNavigation.Back(_tab)));
        var blabel = UIFactory.Text("Label", "< Back", back.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        blabel.color = BankTheme.TextPrimary;
        blabel.raycastTarget = false;
        Stretch(blabel.rectTransform, 0f);

        _transactionTitleText = UIFactory.Text("TransTitle", "Deposit", head.transform, UITheme.Sp(16), TextAnchor.MiddleLeft, FontStyle.Bold);
        _transactionTitleText.color = BankTheme.TextPrimary;
        _transactionTitleText.raycastTarget = false;
        var tle = _transactionTitleText.gameObject.AddComponent<LayoutElement>();
        tle.flexibleWidth = 1f;

        // Flexible scrolling body.
        scrollContent = BuildScroll("TransactionScroll", pane.transform, out _, out _);
        BuildModeTabs(scrollContent);
        BuildAmountCard(scrollContent);
        BuildChipGrid(scrollContent);
        BuildPreviewCard(scrollContent);

        // Fixed footer: the primary action and the feedback line stay at the bottom.
        BuildConfirmButton(pane.transform);
        _feedbackText = UIFactory.Text("FeedbackText", "", pane.transform, UITheme.Sp(12), TextAnchor.MiddleCenter, FontStyle.Bold);
        _feedbackText.color = BankTheme.AccentGreen;
        _feedbackText.horizontalOverflow = HorizontalWrapMode.Wrap;
        SetHeight(_feedbackText.gameObject, UITheme.Dp(20f));

        pane.SetActive(false);
        return pane;
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

        var caption = SectionLabel(card.transform, "ONLINE BALANCE");
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

        var caption2 = UIFactory.Text("CashCaption", "CASH ON HAND", row.transform, UITheme.Sp(13), TextAnchor.MiddleLeft);
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

    private void BuildWeeklyCard(Transform parent)
    {
        var card = BuildCard(parent, "WeeklyCard", UITheme.Dp(90f));
        Stack(card, UITheme.Dp(8f));

        var head = Row(card.transform, UITheme.Dp(22f));
        var hhlg = head.AddComponent<HorizontalLayoutGroup>();
        hhlg.childControlWidth = true;
        hhlg.childControlHeight = true;
        hhlg.childForceExpandWidth = false;
        hhlg.childForceExpandHeight = true;

        var caption = UIFactory.Text("WeeklyCaption", "WEEKLY DEPOSIT LIMIT", head.transform, UITheme.Sp(12), TextAnchor.MiddleLeft);
        caption.color = BankTheme.TextMuted;
        caption.raycastTarget = false;
        var cle = caption.gameObject.AddComponent<LayoutElement>();
        cle.flexibleWidth = 1f;

        _weeklyValueText = UIFactory.Text("WeeklyValue", "0 / 0", head.transform, UITheme.Sp(13), TextAnchor.MiddleRight, FontStyle.Bold);
        _weeklyValueText.color = BankTheme.AccentBlue;
        _weeklyValueText.raycastTarget = false;
        var vle = _weeklyValueText.gameObject.AddComponent<LayoutElement>();
        vle.flexibleWidth = 1f;

        var track = UIFactory.Panel("WeeklyBarTrack", card.transform, BankTheme.CardBgSecondary);
        NoRaycast(track);
        SetHeight(track, UITheme.Dp(10f));

        var fill = UIFactory.Panel("WeeklyBarFill", track.transform, BankTheme.AccentBlue);
        NoRaycast(fill);
        _weeklyBarFill = fill.GetComponent<RectTransform>();
        _weeklyBarFill.anchorMin = new Vector2(0f, 0f);
        _weeklyBarFill.anchorMax = new Vector2(0f, 1f);
        _weeklyBarFill.offsetMin = Vector2.zero;
        _weeklyBarFill.offsetMax = Vector2.zero;

        var foot = Row(card.transform, UITheme.Dp(18f));
        var fhlg = foot.AddComponent<HorizontalLayoutGroup>();
        fhlg.childControlWidth = true;
        fhlg.childControlHeight = true;
        fhlg.childForceExpandWidth = false;
        fhlg.childForceExpandHeight = true;

        _weeklyRemainingText = UIFactory.Text("WeeklyRemaining", "$ 0 remaining", foot.transform, UITheme.Sp(12), TextAnchor.MiddleLeft);
        _weeklyRemainingText.color = BankTheme.TextMuted;
        _weeklyRemainingText.raycastTarget = false;
        var rle = _weeklyRemainingText.gameObject.AddComponent<LayoutElement>();
        rle.flexibleWidth = 1f;

        _weeklyResetText = UIFactory.Text("WeeklyReset", "Resets weekly", foot.transform, UITheme.Sp(11), TextAnchor.MiddleRight);
        _weeklyResetText.color = BankTheme.TextDim;
        _weeklyResetText.raycastTarget = false;
        var tle = _weeklyResetText.gameObject.AddComponent<LayoutElement>();
        tle.flexibleWidth = 1f;
    }

    private void BuildModeTabs(Transform parent)
    {
        var row = Row(parent, UITheme.Dp(40f));
        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(6f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        BuildModeButton(row.transform, "Deposit", TransferMode.Deposit, out _depositTabBg, out _depositTabText);
        BuildModeButton(row.transform, "Withdraw", TransferMode.Withdraw, out _withdrawTabBg, out _withdrawTabText);
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
    }

    /// <summary>
    /// Preset grid matching the mockup: two rows of three fixed amounts ($100..$10,000), then a row
    /// of 25% / 50% / MAX. Three equal-width chips per row.
    /// </summary>
    private void BuildChipGrid(Transform parent)
    {
        BuildChipTriple(parent,
            ("$100", (Action)(() => SetAmount(TransferMath.AmountPresets[0]))),
            ("$500", () => SetAmount(TransferMath.AmountPresets[1])),
            ("$1,000", () => SetAmount(TransferMath.AmountPresets[2])));
        BuildChipTriple(parent,
            ("$2,500", () => SetAmount(TransferMath.AmountPresets[3])),
            ("$5,000", () => SetAmount(TransferMath.AmountPresets[4])),
            ("$10,000", () => SetAmount(TransferMath.AmountPresets[5])));
        BuildChipTriple(parent,
            ("25%", () => SetAmount(TransferMath.PercentageOfMax(TransferMath.RelativePresets[0], CurrentAllowedMax()))),
            ("50%", () => SetAmount(TransferMath.PercentageOfMax(TransferMath.RelativePresets[1], CurrentAllowedMax()))),
            ("MAX", () => SetAmount(CurrentAllowedMax())));
    }

    private void BuildChipTriple(Transform parent, params (string label, Action onClick)[] chips)
    {
        var row = Row(parent, UITheme.Dp(40f));
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
        var card = BuildCard(parent, "PreviewCard", UITheme.Dp(166f));
        Stack(card, UITheme.Dp(4f));
        SectionLabel(card.transform, "TRANSACTION PREVIEW");

        BuildStatRow(card.transform, "Amount", BankTheme.TextPrimary, out _previewAmountText);
        _previewFeeRow = BuildStatRow(card.transform, "Fee", BankTheme.AccentOrange, out _previewFeeText, out _);
        BuildStatRow(card.transform, "You receive", BankTheme.AccentGreen, out _previewNetText, out _previewNetLabel);
        BuildStatRow(card.transform, "Bank debit", BankTheme.AccentOrange, out _previewDebitText, out _previewDebitLabel);
        BuildStatRow(card.transform, "Balance after", BankTheme.AccentTeal, out _previewBalanceText);

        _reasonText = UIFactory.Text("Reason", "", card.transform, UITheme.Sp(12), TextAnchor.MiddleLeft);
        _reasonText.color = BankTheme.AccentRed;
        _reasonText.horizontalOverflow = HorizontalWrapMode.Wrap;
        _reasonText.raycastTarget = false;
        SetHeight(_reasonText.gameObject, UITheme.Dp(18f));
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

        valueText = UIFactory.Text("Value", "—", row.transform, UITheme.Sp(13), TextAnchor.MiddleRight, FontStyle.Bold);
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

    // --- State changes -----------------------------------------------------

    private void SetTab(BankTab tab, bool playSound = true)
    {
        _tab = tab;
        bool overview = tab == BankTab.Overview;
        bool transaction = tab == BankTab.Transaction;

        if (IsAlive(_overviewRoot)) _overviewRoot.SetActive(overview);
        if (IsAlive(_transactionRoot)) _transactionRoot.SetActive(transaction);
        if (playSound) BankSoundService.PlayClick();
    }

    /// <summary>Opens the transaction pane pre-set to <paramref name="mode"/> (from an Overview action).</summary>
    private void OpenTransaction(TransferMode mode)
    {
        _mode = mode;
        UpdateModeVisuals();
        UpdatePreviewAndAction(ModConfig<BankAppConfig>.Instance);
        SetTab(BankTab.Transaction);
    }

    private void SetMode(TransferMode mode)
    {
        _mode = mode;
        BankSoundService.PlayClick();
        UpdateModeVisuals();
        UpdatePreviewAndAction(ModConfig<BankAppConfig>.Instance);
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
        if (_depositTabBg != null) _depositTabBg.color = deposit ? BankTheme.AccentGreen : BankTheme.CardBg;
        if (_withdrawTabBg != null) _withdrawTabBg.color = deposit ? BankTheme.CardBg : BankTheme.AccentOrange;
        if (_depositTabText != null) _depositTabText.color = deposit ? Color.white : BankTheme.TextMuted;
        if (_withdrawTabText != null) _withdrawTabText.color = deposit ? BankTheme.TextMuted : Color.white;
        if (_transactionTitleText != null) _transactionTitleText.text = deposit ? "Deposit" : "Withdraw";
    }

    private void UpdatePreviewAndAction(BankAppConfig config)
    {
        _quote = ComputeCurrentQuote(config);
        bool valid = _quote.IsValid;

        if (IsAlive(_previewAmountText))
            _previewAmountText.text = _enteredAmount > 0f ? FormatMoney(_enteredAmount) : "—";
        if (IsAlive(_previewFeeText))
            _previewFeeText.text = valid
                ? $"{FormatMoney(_quote.Fee)} ({TransferMath.SanitizeFeePercent(config.ServiceFeePercent):0.#}%)"
                : "—";
        if (IsAlive(_previewFeeRow)) _previewFeeRow.SetActive(valid && _quote.Fee > 0f);
        if (IsAlive(_previewNetText)) _previewNetText.text = valid ? FormatMoney(_quote.Net) : "—";
        if (IsAlive(_previewDebitText)) _previewDebitText.text = valid ? FormatMoney(_quote.Debit) : "—";
        if (IsAlive(_previewBalanceText)) _previewBalanceText.text = valid ? FormatMoney(_quote.BalanceAfter) : "—";

        if (IsAlive(_previewNetLabel)) _previewNetLabel.text = "You receive";
        if (IsAlive(_previewDebitLabel))
            _previewDebitLabel.text = _mode == TransferMode.Deposit ? "Cash out" : "Bank debit";

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
            _confirmBtnText.text = _enteredAmount > 0f ? $"{verb} {FormatMoney(_enteredAmount)}" : verb;
            _confirmBtnText.color = valid ? Color.white : BankTheme.TextDim;
        }
    }

    private void UpdateWeekly(BankAppConfig config)
    {
        bool limited = config.RespectVanillaAtmLimit;
        float limit = BankService.VanillaWeeklyAtmLimit;
        float used = limited ? TransactionHistoryService.GetWeeklyDeposits(BankService.GetCurrentInGameWeek()) : 0f;
        float remaining = limited ? BankService.GetRemainingWeeklyAtmLimit() : float.MaxValue;

        if (IsAlive(_weeklyValueText))
            _weeklyValueText.text = limited ? $"{used:N0} / {limit:N0}" : "No weekly limit";
        if (IsAlive(_weeklyRemainingText))
            _weeklyRemainingText.text = limited ? $"{FormatMoney(remaining)} remaining" : "Unlimited weekly deposits";
        if (IsAlive(_weeklyResetText))
        {
            int daysUntilReset = TransferMath.DaysUntilWeeklyReset(BankService.GetCurrentInGameDay());
            _weeklyResetText.text = $"Resets in {daysUntilReset}d";
        }
        if (_weeklyBarFill != null)
        {
            float fraction = limited && limit > 0f ? Mathf.Clamp01(used / limit) : 0f;
            _weeklyBarFill.anchorMax = new Vector2(fraction, 1f);
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

        if (IsAlive(_onlineText)) _onlineText.text = FormatMoney(BankService.GetOnlineBalance());
        if (IsAlive(_cashText)) _cashText.text = FormatMoney(BankService.GetCashBalance());

        UpdateWeekly(config);
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
    /// Fixed-height band helper for the top-level chrome (header, tab strip) and every fixed row.
    /// <para>
    /// Root fix for the "band swallows the screen" bug: a LayoutElement that leaves
    /// <c>flexibleHeight</c> unset (-1) stops reporting it, so the layout system falls through to a
    /// sibling layout group on the same GameObject. A HorizontalLayoutGroup with
    /// <c>childForceExpandHeight = true</c> promotes each child to flexible >= 1 and reports a
    /// positive flexible height, so the parent vertical group hands that band the lion's share of
    /// the spare space (header/tab strip grew to ~140px each on a 560px screen). Pinning
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
                SetTab(BankNavigation.DefaultTab, playSound: false);
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

    /// <summary>
    /// Escape follows S1API's native phone exit chain instead of polling the key ourselves: a
    /// Transaction pane is popped first and the exit request consumed;
    /// on Overview the base implementation closes the app exactly like any other native app.
    /// </summary>
    public override void Exit(ExitAction exit)
    {
        if (exit.Used || !IsOpen()) return;

        if (_tab == BankTab.Transaction)
        {
            exit.Used = true;
            SetTab(BankNavigation.Back(_tab));
            return;
        }

        base.Exit(exit);
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
        SetTab(BankNavigation.DefaultTab, playSound: false);
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
