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

    [Fact]
    public void View_HasOnlyOverviewAndTransactionWithoutActivityOrTabStrip()
    {
        string source = AppSource();

        Assert.Contains("BankTab.Overview", source, StringComparison.Ordinal);
        Assert.Contains("BankTab.Transaction", source, StringComparison.Ordinal);
        Assert.DoesNotContain("BankTab.Activity", source, StringComparison.Ordinal);
        Assert.DoesNotContain("BuildActivity", source, StringComparison.Ordinal);
        Assert.DoesNotContain("RenderActivity", source, StringComparison.Ordinal);
        Assert.DoesNotContain("TabStrip", source, StringComparison.Ordinal);
    }

    [Fact]
    public void View_ShowsOverviewBalanceCashAndWeeklyLimit()
    {
        string source = AppSource();

        Assert.Contains("ONLINE BALANCE", source, StringComparison.Ordinal);
        Assert.Contains("CASH ON HAND", source, StringComparison.Ordinal);
        Assert.Contains("WEEKLY DEPOSIT LIMIT", source, StringComparison.Ordinal);
        Assert.Contains("remaining", source, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("reset", source, StringComparison.OrdinalIgnoreCase);
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
    public void View_RendersFeeNetDebitBalancePreview()
    {
        string source = AppSource();

        Assert.Contains("TRANSACTION PREVIEW", source, StringComparison.Ordinal);
        Assert.Contains("Balance after", source, StringComparison.Ordinal);
        Assert.Contains("quote.Fee", source, StringComparison.Ordinal);
        Assert.Contains("quote.Net", source, StringComparison.Ordinal);
        Assert.Contains("quote.Debit", source, StringComparison.Ordinal);
        Assert.Contains("quote.BalanceAfter", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ActionButton_IsDisabledWithInlineReason()
    {
        string source = AppSource();

        Assert.Contains(".interactable =", source, StringComparison.Ordinal);
        Assert.Contains("ErrorMessage", source, StringComparison.Ordinal);
    }

    [Fact]
    public void TransactionPane_HasBackNavigation()
    {
        string source = AppSource();

        Assert.Contains("BankNavigation.Back", source, StringComparison.Ordinal);
        Assert.Contains("BankNavigation.DefaultTab", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ScrollContent_StretchesToFullWidth()
    {
        string source = AppSource();

        Assert.Contains("childForceExpandWidth = true", source, StringComparison.Ordinal);
        Assert.Contains("sizeDelta = Vector2.zero", source, StringComparison.Ordinal);
    }

    // ---- redesign guards (separate transaction pane) ----

    [Fact]
    public void View_ExposesSeparateTransactionPane()
    {
        string source = AppSource();

        Assert.Contains("BankTab.Transaction", source, StringComparison.Ordinal);
        Assert.Contains("BuildTransactionPane", source, StringComparison.Ordinal);
        Assert.Contains("_transactionRoot", source, StringComparison.Ordinal);
        Assert.Contains("TRANSACTION PREVIEW", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Overview_ActionsOpenTheTransactionPane()
    {
        string source = AppSource();

        Assert.Contains("BuildActionsCard", source, StringComparison.Ordinal);
        Assert.Contains("OpenTransaction", source, StringComparison.Ordinal);
    }

    [Fact]
    public void View_DoesNotBuildUnusedNavigationButtons()
    {
        string source = AppSource();

        Assert.DoesNotContain("BuildTabButton", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Escape_Hierarchy_UsesTheS1ApiExitOverride()
    {
        string source = AppSource();

        Assert.Contains("public override void Exit(", source, StringComparison.Ordinal);
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
    public void WeeklyReset_UsesTheDayCountdown()
    {
        string source = AppSource();

        Assert.Contains("TransferMath.DaysUntilWeeklyReset", source, StringComparison.Ordinal);
        Assert.DoesNotContain("Resets weekly (Week", source, StringComparison.Ordinal);
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

        Assert.Contains("TransactionHistoryService.GetWeeklyDeposits", source, StringComparison.Ordinal);
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
