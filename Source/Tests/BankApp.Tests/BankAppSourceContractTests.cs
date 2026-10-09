using System.Text.RegularExpressions;
using Xunit;

namespace BankApp.Tests;

/// <summary>
/// Practical UI guards: assert the shipped view wires the required sections against the shared
/// theme/sprites and keeps the required behaviours. Not a substitute for Unity rendering tests.
/// </summary>
public sealed class BankAppSourceContractTests
{
    private static string AppSource()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null)
        {
            string candidate = Path.Combine(directory.FullName, "Source", "Mods", "BankApp", "src", "BankApp.cs");
            if (File.Exists(candidate)) return File.ReadAllText(candidate);
            directory = directory.Parent;
        }
        throw new FileNotFoundException("Run these repository layout guards from a repository build output.");
    }

    [Fact]
    public void View_ReusesSharedThemeAndSprites()
    {
        string source = AppSource();

        Assert.Contains("GamePalette.", source, StringComparison.Ordinal);
        Assert.Contains("UISprites.", source, StringComparison.Ordinal);
    }

    // ---- single screen: no second pane, no tab navigation ----

    [Fact]
    public void View_IsASingleScreen_WithoutTabsOrBackNavigation()
    {
        string source = AppSource();

        Assert.Contains("BuildSingleScreen", source, StringComparison.Ordinal);
        Assert.DoesNotContain("BankTab", source, StringComparison.Ordinal);
        Assert.DoesNotContain("BankNavigation", source, StringComparison.Ordinal);
        Assert.DoesNotContain("SetTab", source, StringComparison.Ordinal);
        Assert.DoesNotContain("BuildTransactionPane", source, StringComparison.Ordinal);
        Assert.DoesNotContain("_transactionRoot", source, StringComparison.Ordinal);
        Assert.DoesNotContain("_overviewRoot", source, StringComparison.Ordinal);
        Assert.DoesNotContain("OpenTransaction", source, StringComparison.Ordinal);
        Assert.DoesNotContain("BuildActionsCard", source, StringComparison.Ordinal);
        Assert.DoesNotContain("BuildActivity", source, StringComparison.Ordinal);
        Assert.DoesNotContain("< Back", source, StringComparison.Ordinal);
        Assert.DoesNotContain("BuildTabButton", source, StringComparison.Ordinal);
        Assert.DoesNotContain("BuildTabStrip", source, StringComparison.Ordinal);
    }

    [Fact]
    public void View_KeepsOneScrollHelper_ForTheWholePage()
    {
        string source = AppSource();

        // Exactly one ScrollRect-backed body: the page itself.
        Assert.Contains("BuildScroll(\"BankAppScroll\"", source, StringComparison.Ordinal);
        Assert.Single(Regex.Matches(source, Regex.Escape("BuildScroll(\"")));
    }

    // ---- required content ----

    [Fact]
    public void View_ShowsOnlineBalanceCashOnHandAndHeaderMeta()
    {
        string source = AppSource();

        Assert.Contains("Online balance", source, StringComparison.Ordinal);
        Assert.Contains("Cash on hand", source, StringComparison.Ordinal);
        Assert.Contains("BankApp", source, StringComparison.Ordinal);
        Assert.Contains("BankService.GetCurrentInGameDay", source, StringComparison.Ordinal);
        Assert.Contains("BankService.GetCurrentInGameTimeString", source, StringComparison.Ordinal);
    }

    [Fact]
    public void View_HasModeSwitcherThatNeverNavigates()
    {
        string source = AppSource();

        Assert.Contains("BuildModeSwitcher", source, StringComparison.Ordinal);
        Assert.Contains("BuildModeButton(row.transform, \"Deposit\"", source, StringComparison.Ordinal);
        Assert.Contains("BuildModeButton(row.transform, \"Withdraw\"", source, StringComparison.Ordinal);
        Assert.Contains("SetMode(mode)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void View_UsesSharedPresetAndMaxSources()
    {
        string source = AppSource();

        Assert.Contains("TransferMath.AmountPresets", source, StringComparison.Ordinal);
        Assert.Contains("GetMaxDepositableCash", source, StringComparison.Ordinal);
        Assert.Contains("GetMaxWithdrawableCash", source, StringComparison.Ordinal);
        Assert.Contains("TryParseAmount", source, StringComparison.Ordinal);
        Assert.Contains("BankAppInputFocus", source, StringComparison.Ordinal);
    }

    [Fact]
    public void AmountField_IsGuardedByTheInputFocusComponent()
    {
        string source = AppSource();

        Assert.Contains("_amountInput = field.AddComponent<InputField>()", source, StringComparison.Ordinal);
        Assert.Contains("_inputFocus.amountInput = _amountInput", source, StringComparison.Ordinal);
    }

    [Fact]
    public void View_RendersFeeAndBalanceAfterPreview()
    {
        string source = AppSource();

        Assert.Contains("PREVIEW", source, StringComparison.Ordinal);
        Assert.Contains("Balance after", source, StringComparison.Ordinal);
        Assert.Contains("quote.Fee", source, StringComparison.Ordinal);
        Assert.Contains("quote.BalanceAfter", source, StringComparison.Ordinal);
        Assert.Contains("TransferMath.ComputeQuote", source, StringComparison.Ordinal);
    }

    [Fact]
    public void WeeklyLine_UsesRemainingLimitAndResetCountdown()
    {
        string source = AppSource();

        Assert.Contains("Weekly limit left", source, StringComparison.Ordinal);
        Assert.Contains("GetRemainingWeeklyAtmLimit", source, StringComparison.Ordinal);
        Assert.Contains("TransferMath.DaysUntilWeeklyReset", source, StringComparison.Ordinal);
        Assert.Contains("Resets in", source, StringComparison.Ordinal);
    }

    [Fact]
    public void PrimaryButton_IsDisabledWithInlineReason()
    {
        string source = AppSource();

        Assert.Contains(".interactable =", source, StringComparison.Ordinal);
        Assert.Contains("ErrorMessage", source, StringComparison.Ordinal);
    }

    [Fact]
    public void RecentActivity_KeepsHistoryGrouping()
    {
        string source = AppSource();

        Assert.Contains("HistoryGrouping.GroupByDay", source, StringComparison.Ordinal);
        Assert.Contains("HistoryDayGroup", source, StringComparison.Ordinal);
        Assert.Contains("group.Label", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ScrollContent_StretchesToFullWidth()
    {
        string source = AppSource();

        Assert.Contains("childForceExpandWidth = true", source, StringComparison.Ordinal);
        Assert.Contains("sizeDelta = Vector2.zero", source, StringComparison.Ordinal);
    }

    [Fact]
    public void CloseAndEscape_StayWithThePhone()
    {
        string source = AppSource();

        // No in-app Exit override: the app is one screen, so the phone's own exit chain handles it.
        Assert.DoesNotContain("public override void Exit(", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Input.GetKeyDown(KeyCode.Escape)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void AmountInput_UsesRoundTripFormatting()
    {
        string source = AppSource();

        Assert.Contains("TransferMath.FormatAmountInput", source, StringComparison.Ordinal);
        Assert.DoesNotContain(".ToString(\"N0\", CultureInfo.InvariantCulture)", source, StringComparison.Ordinal);
    }

    [Fact]
    public void FeeRow_IsHiddenAtZero()
    {
        string source = AppSource();

        Assert.Contains("_previewFeeRow", source, StringComparison.Ordinal);
    }

    [Fact]
    public void SuccessFeedback_Expires()
    {
        string source = AppSource();

        Assert.Contains("_feedbackExpiry", source, StringComparison.Ordinal);
    }

    [Fact]
    public void WeeklyLimit_StillRefreshesFromPersistedHistory()
    {
        string source = AppSource();

        Assert.Contains("GetRemainingWeeklyAtmLimit", source, StringComparison.Ordinal);
        Assert.Contains("DispatchHistoryChanged", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Cards_UseContentFittedMinimumHeights()
    {
        string source = AppSource();

        Assert.DoesNotContain("SetHeight(card,", source, StringComparison.Ordinal);
        Assert.Contains("le.minHeight = minHeight", source, StringComparison.Ordinal);
    }
}
