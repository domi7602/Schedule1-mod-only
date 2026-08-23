using System;
using System.Globalization;
using BankApp.Config;
using BankApp.Services;
using S1API.UI;
using S1API.Utils;
using S1Mods.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace BankApp.UI.Panes;

public enum TransferMode
{
    Deposit,
    Withdraw
}

/// <summary>
/// Slot-aware and limit-checked mobile transfer terminal for Deposits and Withdrawals.
/// </summary>
public sealed class TransferPane
{
    public event Action? OnTransactionCompleted;

    private readonly RectTransform _parent;
    private readonly BankAppInputFocus _focusHook;
    private GameObject? _rootPanel;

    private TransferMode _mode = TransferMode.Deposit;
    private float _enteredAmount = 0f;

    private Image _depositTabBg = null!;
    private Text _depositTabText = null!;
    private Image _withdrawTabBg = null!;
    private Text _withdrawTabText = null!;

    private Text _sourceBalanceLabel = null!;
    private Text _sourceBalanceValue = null!;
    private Text _limitLabel = null!;
    private Text _limitValue = null!;

    private InputField _amountInput = null!;
    private Text _feedbackText = null!;
    private GameObject _confirmBtnGO = null!;
    private Text _confirmBtnText = null!;
    private Image _confirmBtnBg = null!;

    public TransferPane(RectTransform parent, BankAppInputFocus focusHook)
    {
        _parent = parent;
        _focusHook = focusHook;
    }

    public void SetMode(TransferMode mode)
    {
        _mode = mode;
        _enteredAmount = 0f;
        if (_amountInput != null) _amountInput.text = string.Empty;
        if (_feedbackText != null) _feedbackText.text = string.Empty;
        UpdateModeVisuals();
        Refresh();
    }

    public void Build()
    {
        if (_rootPanel != null)
        {
            UnityEngine.Object.Destroy(_rootPanel);
        }

        _rootPanel = new GameObject("TransferRoot");
        _rootPanel.transform.SetParent(_parent, false);
        var rRt = _rootPanel.AddComponent<RectTransform>();
        rRt.anchorMin = Vector2.zero;
        rRt.anchorMax = Vector2.one;
        rRt.offsetMin = Vector2.zero;
        rRt.offsetMax = Vector2.zero;

        var vlg = _rootPanel.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(10f);
        vlg.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), (int)UITheme.Dp(10f), (int)UITheme.Dp(16f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // 1. Mode Switcher Tabs
        BuildModeSwitcher(_rootPanel.transform);

        // 2. Info & Limits Card
        BuildInfoCard(_rootPanel.transform);

        // 3. Amount Input & Quick Chips Card
        BuildInputSection(_rootPanel.transform);

        // 4. Feedback / Error Status Label
        _feedbackText = UIFactory.Text("FeedbackText", "", _rootPanel.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        _feedbackText.color = UITheme.AccentGreen;
        _feedbackText.horizontalOverflow = HorizontalWrapMode.Wrap;

        // 5. Confirm Action Button
        BuildConfirmButton(_rootPanel.transform);

        UpdateModeVisuals();
        Refresh();
    }

    private void BuildModeSwitcher(Transform parent)
    {
        var row = UIFactory.Panel("ModeRow", parent, Color.clear);
        var rowLE = row.AddComponent<LayoutElement>();
        rowLE.minHeight = UITheme.Dp(36f);
        rowLE.preferredHeight = UITheme.Dp(36f);

        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(6f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        // Deposit Tab
        var depTab = UIFactory.Panel("DepositTab", row.transform, UITheme.CardBg);
        _depositTabBg = depTab.GetComponent<Image>();
        var dVlg = depTab.AddComponent<VerticalLayoutGroup>();
        dVlg.childAlignment = TextAnchor.MiddleCenter;
        _depositTabText = UIFactory.Text("Label", "⬇  DEPOSIT (Cash → Bank)", depTab.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        var depBtn = depTab.AddComponent<Button>();
        depBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(depBtn, () =>
        {
            BankSoundService.PlayClick();
            SetMode(TransferMode.Deposit);
        });

        // Withdraw Tab
        var withTab = UIFactory.Panel("WithdrawTab", row.transform, UITheme.CardBg);
        _withdrawTabBg = withTab.GetComponent<Image>();
        var wVlg = withTab.AddComponent<VerticalLayoutGroup>();
        wVlg.childAlignment = TextAnchor.MiddleCenter;
        _withdrawTabText = UIFactory.Text("Label", "⬆  WITHDRAW (Bank → Cash)", withTab.transform, UITheme.Sp(10), TextAnchor.MiddleCenter, FontStyle.Bold);
        var withBtn = withTab.AddComponent<Button>();
        withBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(withBtn, () =>
        {
            BankSoundService.PlayClick();
            SetMode(TransferMode.Withdraw);
        });
    }

    private void BuildInfoCard(Transform parent)
    {
        var card = UIFactory.Panel("InfoCard", parent, UITheme.CardBg);
        var cardLE = card.AddComponent<LayoutElement>();
        cardLE.minHeight = UITheme.Dp(72f);
        cardLE.preferredHeight = UITheme.Dp(72f);

        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), (int)UITheme.Dp(8f), (int)UITheme.Dp(8f));
        vlg.spacing = UITheme.Dp(3f);
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // Line 1: Source balance
        var row1 = UIFactory.Panel("Row1", card.transform, Color.clear);
        var r1Hlg = row1.AddComponent<HorizontalLayoutGroup>();
        r1Hlg.childControlWidth = true;
        r1Hlg.childControlHeight = true;
        r1Hlg.childForceExpandWidth = true;
        r1Hlg.childForceExpandHeight = false;

        _sourceBalanceLabel = UIFactory.Text("Label1", "Available Cash:", row1.transform, UITheme.Sp(9), TextAnchor.MiddleLeft, FontStyle.Bold);
        _sourceBalanceLabel.color = UITheme.TextMuted;
        _sourceBalanceValue = UIFactory.Text("Val1", "$ 0.00", row1.transform, UITheme.Sp(10), TextAnchor.MiddleRight, FontStyle.Bold);
        _sourceBalanceValue.color = UITheme.TextPrimary;

        // Line 2: Constraints / Limits
        var row2 = UIFactory.Panel("Row2", card.transform, Color.clear);
        var r2Hlg = row2.AddComponent<HorizontalLayoutGroup>();
        r2Hlg.childControlWidth = true;
        r2Hlg.childControlHeight = true;
        r2Hlg.childForceExpandWidth = true;
        r2Hlg.childForceExpandHeight = false;

        _limitLabel = UIFactory.Text("Label2", "Remaining Weekly ATM Limit:", row2.transform, UITheme.Sp(9), TextAnchor.MiddleLeft, FontStyle.Bold);
        _limitLabel.color = UITheme.TextMuted;
        _limitValue = UIFactory.Text("Val2", "$ 10,000 / $ 10,000", row2.transform, UITheme.Sp(10), TextAnchor.MiddleRight, FontStyle.Bold);
        _limitValue.color = UITheme.AccentBlue;
    }

    private void BuildInputSection(Transform parent)
    {
        var card = UIFactory.Panel("InputCard", parent, UITheme.CardBg);
        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), (int)UITheme.Dp(10f), (int)UITheme.Dp(10f));
        vlg.spacing = UITheme.Dp(8f);
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // Input Field Box
        var inputContainer = UIFactory.Panel("InputContainer", card.transform, UITheme.CardBgSecondary);
        inputContainer.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(38f);
        var inHlg = inputContainer.AddComponent<HorizontalLayoutGroup>();
        inHlg.padding = new RectOffset((int)UITheme.Dp(10f), (int)UITheme.Dp(10f), 0, 0);
        inHlg.childAlignment = TextAnchor.MiddleCenter;

        var dollarSign = UIFactory.Text("DollarSign", "$", inputContainer.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        dollarSign.color = UITheme.TextMuted;

        _amountInput = CreateAmountInputField("AmountInputField", "0.00", inputContainer.transform);
        _focusHook.amountInput = _amountInput;

        EventHelper.AddListener<string>(val =>
        {
            // InvariantCulture: Spieler können "100.50" tippen, unabhängig vom OS-Locale (DE: Komma).
            if (float.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsed))
            {
                _enteredAmount = parsed;
            }
            else
            {
                _enteredAmount = 0f;
            }
        }, _amountInput.onValueChanged);

        // Quick Chips Row
        var chipsRow = UIFactory.Panel("ChipsRow", card.transform, Color.clear);
        chipsRow.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(30f);
        var cHlg = chipsRow.AddComponent<HorizontalLayoutGroup>();
        cHlg.spacing = UITheme.Dp(4f);
        cHlg.childControlWidth = true;
        cHlg.childControlHeight = true;
        cHlg.childForceExpandWidth = true;
        cHlg.childForceExpandHeight = true;

        BuildChip(chipsRow.transform, "+$100", () => AddAmount(100f));
        BuildChip(chipsRow.transform, "+$500", () => AddAmount(500f));
        BuildChip(chipsRow.transform, "+$1,000", () => AddAmount(1000f));
        BuildChip(chipsRow.transform, "MAX", ApplyMaxAmount, isAccent: true);
        BuildChip(chipsRow.transform, "CLEAR", ClearAmount);
    }

    private InputField CreateAmountInputField(string name, string placeholderText, Transform parent)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        go.AddComponent<LayoutElement>().flexibleWidth = 1f;

        var inputField = go.AddComponent<InputField>();

        var text = UIFactory.Text("Text", "", go.transform, UITheme.Sp(13), TextAnchor.MiddleLeft);
        var textRt = text.rectTransform;
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = new Vector2(6f, 0f);
        textRt.offsetMax = new Vector2(-6f, 0f);
        text.color = UITheme.TextPrimary;
        text.supportRichText = false;
        text.raycastTarget = false;

        var placeholder = UIFactory.Text("Placeholder", placeholderText, go.transform, UITheme.Sp(13), TextAnchor.MiddleLeft);
        var phRt = placeholder.rectTransform;
        phRt.anchorMin = Vector2.zero;
        phRt.anchorMax = Vector2.one;
        phRt.offsetMin = new Vector2(6f, 0f);
        phRt.offsetMax = new Vector2(-6f, 0f);
        placeholder.color = UITheme.TextDim;
        placeholder.supportRichText = false;
        placeholder.raycastTarget = false;

        inputField.textComponent = text;
        inputField.placeholder = placeholder;
        inputField.contentType = InputField.ContentType.DecimalNumber;
        inputField.lineType = InputField.LineType.SingleLine;

        return inputField;
    }

    private void BuildChip(Transform parent, string label, Action onClick, bool isAccent = false)
    {
        var chipGO = UIFactory.Panel($"Chip_{label}", parent, isAccent ? UITheme.AccentBlue : UITheme.CardBgSecondary);
        var chipVlg = chipGO.AddComponent<VerticalLayoutGroup>();
        chipVlg.childAlignment = TextAnchor.MiddleCenter;

        var txt = UIFactory.Text("Txt", label, chipGO.transform, UITheme.Sp(9), TextAnchor.MiddleCenter, FontStyle.Bold);
        txt.color = isAccent ? Color.white : UITheme.TextPrimary;

        var btn = chipGO.AddComponent<Button>();
        btn.transition = Selectable.Transition.ColorTint;
        ButtonUtils.AddListener(btn, () =>
        {
            BankSoundService.PlayClick();
            onClick();
        });
    }

    private void BuildConfirmButton(Transform parent)
    {
        _confirmBtnGO = UIFactory.Panel("ConfirmBtn", parent, UITheme.AccentGreen);
        _confirmBtnBg = _confirmBtnGO.GetComponent<Image>();
        _confirmBtnGO.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(40f);

        var vlg = _confirmBtnGO.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.MiddleCenter;

        _confirmBtnText = UIFactory.Text("Label", "CONFIRM DEPOSIT", _confirmBtnGO.transform, UITheme.Sp(12), TextAnchor.MiddleCenter, FontStyle.Bold);
        _confirmBtnText.color = Color.white;

        var btn = _confirmBtnGO.AddComponent<Button>();
        btn.transition = Selectable.Transition.ColorTint;
        ButtonUtils.AddListener(btn, ExecuteTransaction);
    }

    private void AddAmount(float delta)
    {
        _enteredAmount += delta;
        _amountInput.text = _enteredAmount.ToString("0.##", CultureInfo.InvariantCulture);
    }

    private void ClearAmount()
    {
        _enteredAmount = 0f;
        _amountInput.text = "";
    }

    private void ApplyMaxAmount()
    {
        if (_mode == TransferMode.Deposit)
        {
            _enteredAmount = BankService.GetMaxDepositableCash();
        }
        else
        {
            _enteredAmount = BankService.GetMaxWithdrawableCash();
        }
        _amountInput.text = _enteredAmount.ToString("0.##", CultureInfo.InvariantCulture);
    }

    private void ExecuteTransaction()
    {
        if (_enteredAmount <= 0f)
        {
            SetFeedback("Please enter a valid amount greater than $0.", isError: true);
            BankSoundService.PlayError();
            return;
        }

        bool success;
        string errorMsg;

        if (_mode == TransferMode.Deposit)
        {
            success = BankService.DepositCash(_enteredAmount, out errorMsg);
            if (success)
            {
                SetFeedback($"Successfully deposited $ {_enteredAmount:N2} into Checking Account.", isError: false);
                _enteredAmount = 0f;
                _amountInput.text = "";
                OnTransactionCompleted?.Invoke();
            }
            else
            {
                SetFeedback(errorMsg, isError: true);
            }
        }
        else
        {
            success = BankService.WithdrawCash(_enteredAmount, out errorMsg);
            if (success)
            {
                SetFeedback($"Successfully withdrew $ {_enteredAmount:N2} into pockets.", isError: false);
                _enteredAmount = 0f;
                _amountInput.text = "";
                OnTransactionCompleted?.Invoke();
            }
            else
            {
                SetFeedback(errorMsg, isError: true);
            }
        }

        Refresh();
    }

    private void SetFeedback(string text, bool isError)
    {
        if (_feedbackText != null)
        {
            _feedbackText.text = text;
            _feedbackText.color = isError ? UITheme.AccentRed : UITheme.AccentGreen;
        }
    }

    private void UpdateModeVisuals()
    {
        bool isDep = _mode == TransferMode.Deposit;

        if (_depositTabBg != null) _depositTabBg.color = isDep ? UITheme.AccentGreen : UITheme.CardBg;
        if (_depositTabText != null) _depositTabText.color = isDep ? Color.white : UITheme.TextMuted;

        if (_withdrawTabBg != null) _withdrawTabBg.color = !isDep ? UITheme.AccentOrange : UITheme.CardBg;
        if (_withdrawTabText != null) _withdrawTabText.color = !isDep ? Color.white : UITheme.TextMuted;

        if (_confirmBtnBg != null) _confirmBtnBg.color = isDep ? UITheme.AccentGreen : UITheme.AccentOrange;
        if (_confirmBtnText != null) _confirmBtnText.text = isDep ? "CONFIRM DEPOSIT" : "CONFIRM WITHDRAWAL";
    }

    public void Refresh()
    {
        bool isDep = _mode == TransferMode.Deposit;

        if (isDep)
        {
            float cash = BankService.GetCashBalance();
            _sourceBalanceLabel.text = "Available Cash in Pockets:";
            _sourceBalanceValue.text = $"$ {cash:N2}";

            var config = ModConfig<BankAppConfig>.Instance;
            if (config.RespectVanillaAtmLimit)
            {
                float remLimit = BankService.GetRemainingWeeklyAtmLimit();
                _limitLabel.text = "Remaining Weekly ATM Limit:";
                _limitValue.text = $"$ {remLimit:N0} / $ {BankService.VanillaWeeklyAtmLimit:N0}";
                _limitValue.color = remLimit > 0f ? UITheme.AccentBlue : UITheme.AccentRed;
            }
            else
            {
                _limitLabel.text = "Weekly Deposit Limit:";
                _limitValue.text = "UNLIMITED (Config Override)";
                _limitValue.color = UITheme.AccentGreen;
            }
        }
        else
        {
            float online = BankService.GetOnlineBalance();
            _sourceBalanceLabel.text = "Digital Checking Balance:";
            _sourceBalanceValue.text = $"$ {online:N2}";

            int freeSlots = BankService.GetFreeInventorySlotsCount();
            float maxWithdrawable = BankService.GetMaxWithdrawableCash();
            _limitLabel.text = "Max Cash Carry Capacity:";
            _limitValue.text = $"$ {maxWithdrawable:N0} ({freeSlots} free slot{(freeSlots == 1 ? "" : "s")})";
            _limitValue.color = maxWithdrawable > 0f ? UITheme.AccentOrange : UITheme.AccentRed;
        }
    }

    public void SetActive(bool active)
    {
        if (_rootPanel != null)
        {
            _rootPanel.SetActive(active);
            if (active)
            {
                Refresh();
            }
            else
            {
                if (_focusHook != null) _focusHook.amountInput = null;
            }
        }
    }
}
