using System;
using System.IO;
using BankApp.Config;
using BankApp.Services;
using BankApp.UI;
using MelonLoader;
using MelonLoader.Utils;
using S1API.PhoneApp;
using S1API.UI;
using S1API.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BankApp;

public enum TransferMode
{
    Deposit,
    Withdraw
}

/// <summary>
/// Mobile Banking Smartphone App for Schedule I (v0.4.0).
/// Single-screen chip-based UI per target mockup (docs/mockup-target-v0.3.0.png):
/// weekly progress bar, two-column balances, chip grid ($1..$1000 + Clear/MAX),
/// deposit/withdraw mode tabs, one confirm button. No text input field.
/// Uniform 38dp rows / 6dp grid gaps.
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

    // Uniform layout metrics — every interactive row is identical
    private float RowHeight => UITheme.Dp(38f);
    private float GridGap => UITheme.Dp(6f);

    // Weekly progress
    private Text _weeklyValueText = null!;
    private RectTransform _weeklyBarFill = null!;

    // Balances
    private Text _cashText = null!;
    private Text _onlineText = null!;

    // Amount display
    private Text _amountText = null!;

    // Mode tabs
    private Image _depositTabBg = null!;
    private Text _depositTabText = null!;
    private Image _withdrawTabBg = null!;
    private Text _withdrawTabText = null!;

    // MAX chip
    private Image _maxChipBg = null!;

    // Confirm button
    private Image _confirmBtnBg = null!;
    private Text _confirmBtnText = null!;

    private Text _feedbackText = null!;

    private TransferMode _mode = TransferMode.Deposit;
    private float _enteredAmount;
    private float _lastRefreshTime;

    private static readonly (string label, float value)[] AmountChips =
    {
        ("$1", 1f), ("$5", 5f),
        ("$10", 10f), ("$25", 25f),
        ("$50", 50f), ("$100", 100f),
        ("$500", 500f), ("$1000", 1000f),
    };

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
        TransactionHistoryService.OnHistoryChanged += DispatchBalanceChanged;
        MelonLogger.Msg("Registered with S1API PhoneApp system (v0.3.0).");
    }

    internal static void TearDownForSceneUnload() => _active = null;

    private static void DispatchUpdate() => _active?.Update();

    private static void DispatchBalanceChanged() => _active?.OnExternalBalanceChanged();

    protected override void OnCreatedUI(GameObject container)
    {
        var containerRt = container.GetComponent<RectTransform>();
        if (containerRt != null)
        {
            UITheme.Initialize(containerRt);
        }

        _mainBG = UIFactory.Panel("BankApp_MainBG", container.transform, UITheme.BgDark, fullAnchor: true);
        _mainBG.SetActive(false);

        var vlg = _mainBG.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(6f);
        vlg.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), (int)UITheme.Dp(12f), (int)UITheme.Dp(14f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        // 1. Weekly Progress (limit usage + bar)
        BuildWeeklyCard(_mainBG.transform);

        // 2. Two-column balances
        BuildBalanceCard(_mainBG.transform);

        // 3. Amount display row
        BuildAmountRow(_mainBG.transform);

        // 4. Deposit / Withdraw mode tabs
        BuildModeTabs(_mainBG.transform);

        // 5. Chip grid (4 rows of amount chips + Clear/MAX row)
        BuildChipGrid(_mainBG.transform);

        // 6. Confirm button
        BuildConfirmButton(_mainBG.transform);

        // 7. Feedback line (fills remaining space)
        _feedbackText = UIFactory.Text("FeedbackText", "", _mainBG.transform, UITheme.Sp(12), TextAnchor.MiddleCenter, FontStyle.Bold);
        _feedbackText.color = UITheme.AccentGreen;
        _feedbackText.horizontalOverflow = HorizontalWrapMode.Wrap;
        var fbLe = _feedbackText.gameObject.AddComponent<LayoutElement>();
        fbLe.minHeight = UITheme.Dp(22f);
        fbLe.flexibleHeight = 1f;

        UpdateModeVisuals();
        RefreshAll();
    }

    private void BuildWeeklyCard(Transform parent)
    {
        var card = UIFactory.Panel("WeeklyCard", parent, UITheme.CardBg);
        var cardLe = card.AddComponent<LayoutElement>();
        cardLe.flexibleHeight = 0.4f;
        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), (int)UITheme.Dp(10f), (int)UITheme.Dp(10f));
        vlg.spacing = UITheme.Dp(8f);
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        var row = UIFactory.Panel("Row", card.transform, Color.clear);
        row.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(24f);
        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;

        var lbl = UIFactory.Text("Label", "WEEKLY PROGRESS", row.transform, UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold);
        lbl.color = UITheme.TextMuted;

        _weeklyValueText = UIFactory.Text("Value", "$ 0 / $ 10,000", row.transform, UITheme.Sp(12), TextAnchor.MiddleRight, FontStyle.Bold);
        _weeklyValueText.color = UITheme.TextPrimary;

        // Progress bar: track + anchored fill (track uses CardBorder so the empty
        // bar stays visible at 0% progress instead of blending into the card)
        var track = UIFactory.Panel("BarTrack", card.transform, UITheme.CardBorder);
        track.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(9f);
        var trackRt = track.GetComponent<RectTransform>();

        var fill = UIFactory.Panel("BarFill", track.transform, UITheme.AccentTeal);
        _weeklyBarFill = fill.GetComponent<RectTransform>();
        _weeklyBarFill.anchorMin = Vector2.zero;
        _weeklyBarFill.anchorMax = new Vector2(0f, 1f);
        _weeklyBarFill.offsetMin = Vector2.zero;
        _weeklyBarFill.offsetMax = Vector2.zero;
        _ = trackRt; // track anchors are controlled by the parent VerticalLayoutGroup
    }

    private void BuildBalanceCard(Transform parent)
    {
        var card = UIFactory.Panel("BalanceCard", parent, UITheme.CardBg);
        var cardLe = card.AddComponent<LayoutElement>();
        cardLe.flexibleHeight = 0.6f;
        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), (int)UITheme.Dp(10f), (int)UITheme.Dp(10f));
        vlg.spacing = UITheme.Dp(2f);
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        var row = UIFactory.Panel("Cols", card.transform, Color.clear);
        row.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(48f);
        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = UITheme.Dp(8f);
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;

        _cashText = BuildBalanceColumn(row.transform, "CashCol", "CASH BALANCE");
        _onlineText = BuildBalanceColumn(row.transform, "OnlineCol", "ONLINE BALANCE");
    }

    private Text BuildBalanceColumn(Transform parent, string name, string label)
    {
        var col = UIFactory.Panel(name, parent, Color.clear);
        col.AddComponent<LayoutElement>().flexibleWidth = 1f;
        var vlg = col.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = UITheme.Dp(2f);
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        var lbl = UIFactory.Text("Label", label, col.transform, UITheme.Sp(11), TextAnchor.MiddleLeft, FontStyle.Bold);
        lbl.color = UITheme.TextMuted;

        var value = UIFactory.Text("Value", "$ 0", col.transform, UITheme.Sp(19), TextAnchor.MiddleLeft, FontStyle.Bold);
        value.color = UITheme.AccentTeal;

        return value;
    }

    private void BuildAmountRow(Transform parent)
    {
        var card = UIFactory.Panel("AmountRow", parent, UITheme.CardBg);
        var cardLe = card.AddComponent<LayoutElement>();
        cardLe.flexibleHeight = 0.4f;
        var vlg = card.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset((int)UITheme.Dp(12f), (int)UITheme.Dp(12f), (int)UITheme.Dp(8f), (int)UITheme.Dp(8f));
        vlg.childControlWidth = true;
        vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        var row = UIFactory.Panel("Row", card.transform, Color.clear);
        row.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(30f);
        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = false;

        var lbl = UIFactory.Text("Label", "AMOUNT", row.transform, UITheme.Sp(12), TextAnchor.MiddleLeft, FontStyle.Bold);
        lbl.color = UITheme.TextMuted;

        _amountText = UIFactory.Text("Value", "$ 0", row.transform, UITheme.Sp(18), TextAnchor.MiddleRight, FontStyle.Bold);
        _amountText.color = UITheme.TextPrimary;
    }

    private void BuildModeTabs(Transform parent)
    {
        var row = UIFactory.Panel("ModeTabs", parent, Color.clear);
        var rowLe = row.AddComponent<LayoutElement>();
        rowLe.preferredHeight = RowHeight; // v0.4.2: identisch zu ChipRow (dazu minHeight)
        rowLe.minHeight = RowHeight;

        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = GridGap; // v0.4.0: matches chip grid gap (was 8dp)
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        // Deposit tab
        var depTab = UIFactory.Panel("DepositTab", row.transform, UITheme.CardBgSecondary);
        _depositTabBg = depTab.GetComponent<Image>();
        var depLe = depTab.AddComponent<LayoutElement>();
        depLe.flexibleWidth = 1f; // v0.4.2: wie Chips — erzwingt exakt 50/50, unabhaengig von Textlaenge
        var dVlg = depTab.AddComponent<VerticalLayoutGroup>();
        dVlg.childAlignment = TextAnchor.MiddleCenter;
        dVlg.childControlWidth = false;
        dVlg.childControlHeight = false;
        dVlg.childForceExpandWidth = false;
        dVlg.childForceExpandHeight = false;
        _depositTabText = UIFactory.Text("Label", "⬇  DEPOSIT", depTab.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        var depBtn = depTab.AddComponent<Button>();
        depBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(depBtn, () => SetMode(TransferMode.Deposit));

        // Withdraw tab
        var withTab = UIFactory.Panel("WithdrawTab", row.transform, UITheme.CardBgSecondary);
        _withdrawTabBg = withTab.GetComponent<Image>();
        var withLe = withTab.AddComponent<LayoutElement>();
        withLe.flexibleWidth = 1f; // v0.4.2: wie Chips — erzwingt exakt 50/50, unabhaengig von Textlaenge
        var wVlg = withTab.AddComponent<VerticalLayoutGroup>();
        wVlg.childAlignment = TextAnchor.MiddleCenter;
        wVlg.childControlWidth = false;
        wVlg.childControlHeight = false;
        wVlg.childForceExpandWidth = false;
        wVlg.childForceExpandHeight = false;
        _withdrawTabText = UIFactory.Text("Label", "⬆  WITHDRAW", withTab.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        var withBtn = withTab.AddComponent<Button>();
        withBtn.transition = Selectable.Transition.None;
        ButtonUtils.AddListener(withBtn, () => SetMode(TransferMode.Withdraw));
    }

    private void BuildChipGrid(Transform parent)
    {
        // v0.4.0: 4 rows with 2 amount chips each (mockup: 2-col grid). The old
        // fixed MAX slot is gone — MAX is now mode-aware (Withdraw only, accent
        // chip) and its slot hosts a neutral placeholder in Deposit mode.
        for (int r = 0; r < AmountChips.Length / 2; r++)
        {
            var (labelA, valueA) = AmountChips[r * 2];
            var (labelB, valueB) = AmountChips[r * 2 + 1];
            BuildChipRow(parent, (labelA, valueA), (labelB, valueB));
        }

        // Last row: Clear + MAX
        BuildChipRow(parent, ("✕ CLEAR", null), ("MAX", float.NaN));
    }

    private (GameObject left, GameObject right) BuildChipRow(Transform parent, (string label, float? value) left, (string label, float? value) right)
    {
        var row = UIFactory.Panel("ChipRow", parent, Color.clear);
        var rowLe = row.AddComponent<LayoutElement>();
        rowLe.preferredHeight = RowHeight; // v0.4.0: fixed height — no flexibleHeight, no uneven rows
        rowLe.minHeight = RowHeight;

        var hlg = row.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = GridGap;
        hlg.childControlWidth = true;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = true;
        hlg.childForceExpandHeight = true;

        var l = BuildChip(row.transform, left.label, left.value);
        var r = BuildChip(row.transform, right.label, right.value);
        return (l, r);
    }

    private GameObject BuildChip(Transform parent, string label, float? value)
    {
        bool isAction = value == null;           // Clear
        bool isAccent = float.IsNaN(value ?? 0f) && value != null; // MAX

        var chipGO = UIFactory.Panel($"Chip_{label.Replace("$", "").Replace("✕ ", "")}",
            parent, isAccent ? UITheme.AccentBlue : UITheme.CardBgSecondary);
        if (isAccent)
        {
            _maxChipBg = chipGO.GetComponent<Image>();
        }
        var chipLe = chipGO.AddComponent<LayoutElement>();
        chipLe.flexibleWidth = 1f;
        var chipVlg = chipGO.AddComponent<VerticalLayoutGroup>();
        chipVlg.childAlignment = TextAnchor.MiddleCenter;
        chipVlg.childControlWidth = false;
        chipVlg.childControlHeight = false;
        chipVlg.childForceExpandWidth = false;
        chipVlg.childForceExpandHeight = false;

        var txt = UIFactory.Text("Txt", label, chipGO.transform, UITheme.Sp(13), TextAnchor.MiddleCenter, FontStyle.Bold);
        txt.color = isAccent ? Color.white : UITheme.TextPrimary;

        var btn = chipGO.AddComponent<Button>();
        btn.transition = Selectable.Transition.ColorTint;
        ButtonUtils.AddListener(btn, () =>
        {
            BankSoundService.PlayClick();
            if (value == null)
            {
                ClearAmount();
            }
            else if (float.IsNaN(value.Value))
            {
                ApplyMaxAmount();
            }
            else
            {
                AddAmount(value.Value);
            }
        });
        return chipGO;
    }

    private void BuildConfirmButton(Transform parent)
    {
        var btnGO = UIFactory.Panel("ConfirmBtn", parent, UITheme.AccentGreen);
        _confirmBtnBg = btnGO.GetComponent<Image>();
        btnGO.AddComponent<LayoutElement>().preferredHeight = UITheme.Dp(52f);

        var vlg = btnGO.AddComponent<VerticalLayoutGroup>();
        vlg.childAlignment = TextAnchor.MiddleCenter;

        _confirmBtnText = UIFactory.Text("Label", "DEPOSIT", btnGO.transform, UITheme.Sp(16), TextAnchor.MiddleCenter, FontStyle.Bold);
        _confirmBtnText.color = Color.white;

        var btn = btnGO.AddComponent<Button>();
        btn.transition = Selectable.Transition.ColorTint;
        ButtonUtils.AddListener(btn, ExecuteTransaction);
    }

    // --- Actions -----------------------------------------------------------

    private void SetMode(TransferMode mode)
    {
        if (_mode == mode) return;
        BankSoundService.PlayClick();
        _mode = mode;
        ClearAmount();
        UpdateModeVisuals();
    }

    private void AddAmount(float delta)
    {
        _enteredAmount += delta;
        UpdateAmountDisplay();
    }

    private void ClearAmount()
    {
        _enteredAmount = 0f;
        UpdateAmountDisplay();
    }

    private void ApplyMaxAmount()
    {
        _enteredAmount = _mode == TransferMode.Deposit
            ? BankService.GetMaxDepositableCash()
            : BankService.GetMaxWithdrawableCash();
        UpdateAmountDisplay();
    }

    private void ExecuteTransaction()
    {
        if (_enteredAmount <= 0f)
        {
            SetFeedback("Select an amount first.", isError: true);
            BankSoundService.PlayError();
            return;
        }

        bool success = _mode == TransferMode.Deposit
            ? BankService.DepositCash(_enteredAmount, out string errorMsg)
            : BankService.WithdrawCash(_enteredAmount, out errorMsg);

        if (success)
        {
            SetFeedback($"{(_mode == TransferMode.Deposit ? "Deposited" : "Withdrew")} $ {_enteredAmount:N0}.", isError: false);
            ClearAmount();
        }
        else
        {
            SetFeedback(errorMsg, isError: true);
        }

        RefreshAll();
    }

    private void UpdateAmountDisplay()
    {
        if (_amountText != null) _amountText.text = $"$ {_enteredAmount:N0}";
    }

    private void UpdateModeVisuals()
    {
        bool isDep = _mode == TransferMode.Deposit;

        if (_depositTabBg != null) _depositTabBg.color = isDep ? UITheme.AccentBlue : UITheme.CardBgSecondary;
        if (_depositTabText != null) _depositTabText.color = isDep ? Color.white : UITheme.TextMuted;

        if (_withdrawTabBg != null) _withdrawTabBg.color = !isDep ? UITheme.AccentBlue : UITheme.CardBgSecondary;
        if (_withdrawTabText != null) _withdrawTabText.color = !isDep ? Color.white : UITheme.TextMuted;

        if (_maxChipBg != null) _maxChipBg.color = isDep ? UITheme.AccentBlue : UITheme.AccentOrange;

        if (_confirmBtnBg != null) _confirmBtnBg.color = isDep ? UITheme.AccentGreen : UITheme.AccentOrange;
        if (_confirmBtnText != null) _confirmBtnText.text = isDep ? "⬇  DEPOSIT" : "⬆  WITHDRAW";
    }

    private void SetFeedback(string text, bool isError)
    {
        if (_feedbackText != null)
        {
            _feedbackText.text = text;
            _feedbackText.color = isError ? UITheme.AccentRed : UITheme.AccentGreen;
        }
    }

    private void RefreshAll()
    {
        float cash = BankService.GetCashBalance();
        float online = BankService.GetOnlineBalance();
        if (IsAlive(_cashText)) _cashText.text = $"$ {cash:N0}";
        if (IsAlive(_onlineText)) _onlineText.text = $"$ {online:N0}";

        // Weekly deposit progress against vanilla ATM limit
        bool limitEnabled = S1Mods.Shared.ModConfig<BankAppConfig>.Instance.RespectVanillaAtmLimit;
        if (limitEnabled)
        {
            float remaining = BankService.GetRemainingWeeklyAtmLimit();
            float deposited = Mathf.Max(0f, BankService.VanillaWeeklyAtmLimit - remaining);
            float fraction = Mathf.Clamp01(deposited / BankService.VanillaWeeklyAtmLimit);

            if (IsAlive(_weeklyValueText))
            {
                _weeklyValueText.text = $"$ {deposited:N0} / $ {BankService.VanillaWeeklyAtmLimit:N0}";
                _weeklyValueText.color = fraction >= 1f ? UITheme.AccentRed : UITheme.TextPrimary;
            }
            if (IsAlive(_weeklyBarFill)) _weeklyBarFill.anchorMax = new Vector2(fraction, 1f);
        }
        else
        {
            if (IsAlive(_weeklyValueText))
            {
                _weeklyValueText.text = "NO LIMIT";
                _weeklyValueText.color = UITheme.AccentGreen;
            }
            if (IsAlive(_weeklyBarFill)) _weeklyBarFill.anchorMax = new Vector2(0f, 1f);
        }
    }

    /// <summary>IL2CPP liveness: managed wrappers survive scene unload while native objects are dead.</summary>
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
            if (open) RefreshAll();
        }

        if (!open) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseApp();
        }

        if (Time.unscaledTime - _lastRefreshTime > 1.5f)
        {
            _lastRefreshTime = Time.unscaledTime;
            RefreshAll();
        }
    }

    private void OnExternalBalanceChanged()
    {
        if (IsOpen()) RefreshAll();
    }

    protected override void OnPhoneClosed()
    {
        base.OnPhoneClosed();
        if (IsAlive(_mainBG)) _mainBG.SetActive(false);
        _enteredAmount = 0f;
        _mode = TransferMode.Deposit;
        UpdateAmountDisplay();
        UpdateModeVisuals();
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
