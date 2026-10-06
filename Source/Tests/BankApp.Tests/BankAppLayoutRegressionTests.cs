using System.Globalization;
using System.Text.RegularExpressions;
using Xunit;

namespace BankApp.Tests;

/// <summary>
/// Unity-free numerical harness for uGUI <c>HorizontalOrVerticalLayoutGroup</c> height distribution.
/// <para>
/// This is a MODEL, not a uGUI render test: it reproduces the sizing rule the real layout system
/// uses — a child's effective <c>preferred</c> is clamped up to its <c>min</c>, spare space is split
/// between children proportional to their reported <c>flexible</c> size, and a child only reports a
/// positive flexible size if a LayoutElement says so (priority wins) or a layout group with
/// force-expansion promotes it. It exists so the "band swallows the screen" regression can be
/// demonstrated at 400x750 without booting the game.
/// </para>
/// </summary>
public static class VerticalLayoutModel
{
    public sealed class Band
    {
        public string Name = string.Empty;
        public float Min;
        public float Preferred;
        public float Flexible;

        /// <summary>A fixed band: min == preferred, never grows (LayoutElement flexibleHeight = 0, priority 1).</summary>
        public static Band Fixed(string name, float height) => new() { Name = name, Min = height, Preferred = height, Flexible = 0f };

        /// <summary>The intended viewport: the only band allowed positive height flexibility.</summary>
        public static Band Grow(string name, float min, float preferred) => new() { Name = name, Min = min, Preferred = preferred, Flexible = 1f };

        /// <summary>A band whose LayoutElement leaves flexibleHeight unset, so a sibling HLG with force-expansion reports flexible &gt; 0.</summary>
        public static Band Unpinned(string name, float min, float preferred, float reportedFlexible) =>
            new() { Name = name, Min = min, Preferred = preferred, Flexible = reportedFlexible };
    }

    /// <summary>Height assigned to each child band when the group fills <paramref name="available"/>.</summary>
    public static float[] Distribute(IReadOnlyList<Band> bands, float available, float spacing, float paddingVertical)
    {
        int n = bands.Count;
        float gaps = (spacing * MathF.Max(0, n - 1)) + paddingVertical;
        float sumPref = 0f, sumMin = 0f, totalFlex = 0f;
        for (int i = 0; i < n; i++)
        {
            sumPref += bands[i].Preferred;
            sumMin += bands[i].Min;
            totalFlex += bands[i].Flexible;
        }

        float free = available - gaps;
        var result = new float[n];

        if (totalFlex > 0f && free > sumPref)
        {
            float extra = free - sumPref;
            for (int i = 0; i < n; i++)
                result[i] = bands[i].Preferred + (extra * bands[i].Flexible / totalFlex);
        }
        else if (free >= sumMin)
        {
            // Squeeze: clamp every band up to its minimum (preferred clamped to min happens earlier).
            for (int i = 0; i < n; i++) result[i] = MathF.Max(bands[i].Min, MathF.Min(bands[i].Preferred, free * (bands[i].Min / MathF.Max(1f, sumMin))));
            for (int i = 0; i < n; i++) result[i] = MathF.Max(bands[i].Min, result[i]);
        }
        else
        {
            for (int i = 0; i < n; i++) result[i] = bands[i].Min;
        }

        return result;
    }
}

/// <summary>
/// Design-budget checks at the reference canvas (400x750) and the installed phone rect (~300x560 →
/// UITheme scale clamps to 0.85). Together with the source contracts below this is the honest
/// substitute for a Unity render regression.
/// </summary>
public sealed class LayoutBudgetHarnessTests
{
    private const float HeaderDp = 38f;
    private const float TabsDp = 36f;
    private const float ViewportMinDp = 200f;
    private const float SpacingDp = 6f;
    private const float PaddingDp = 18f; // top 8 + bottom 10

    [Fact]
    public void Overview_AtReferenceCanvas_KeepsTopBandsCompactAndViewportDominant()
    {
        var bands = new[]
        {
            VerticalLayoutModel.Band.Fixed("Header", HeaderDp),
            VerticalLayoutModel.Band.Fixed("Tabs", TabsDp),
            VerticalLayoutModel.Band.Grow("Viewport", ViewportMinDp, ViewportMinDp),
        };

        float[] h = VerticalLayoutModel.Distribute(bands, 750f, SpacingDp, PaddingDp);

        Assert.Equal(HeaderDp, h[0], 3);          // header never absorbs slack
        Assert.Equal(TabsDp, h[1], 3);            // tabs never absorb slack
        Assert.True(h[2] >= ViewportMinDp);       // viewport keeps its floor
        Assert.True(h[2] > 600f, $"viewport should dominate: got {h[2]}");
        Assert.True(h[0] <= 0.08f * 750f, $"header should be <= 8% of 750: got {h[0]}");
        Assert.True(h[1] <= 0.08f * 750f, $"tabs should be <= 8% of 750: got {h[1]}");
    }

    [Fact]
    public void Overview_AtInstalledScale_StillLeavesViewportDominant()
    {
        // Installed phone rect ~300x560 → UITheme.Scale = clamp(560/750, 0.85, 2.0) = 0.85.
        const float scale = 0.85f;
        float available = 560f;
        float dp(float v) => v * scale;

        var bands = new[]
        {
            VerticalLayoutModel.Band.Fixed("Header", dp(HeaderDp)),
            VerticalLayoutModel.Band.Fixed("Tabs", dp(TabsDp)),
            VerticalLayoutModel.Band.Grow("Viewport", dp(ViewportMinDp), dp(ViewportMinDp)),
        };

        float[] h = VerticalLayoutModel.Distribute(bands, available, dp(SpacingDp), dp(PaddingDp));

        Assert.True(h[0] <= 0.10f * available, $"header should be <= 10% of the phone: got {h[0]}");
        Assert.True(h[1] <= 0.10f * available, $"tabs should be <= 10% of the phone: got {h[1]}");
        Assert.True(h[2] >= 0.70f * available, $"viewport should dominate the phone: got {h[2]}");
    }

    [Fact]
    public void UnpinnedBand_ReproducesTheBlowup_AndPinningReturnsSlackToTheViewport()
    {
        // Pre-fix: Header's LayoutElement left flexibleHeight unset (-1); its HorizontalLayoutGroup
        // with childForceExpandHeight=true reports flexibleHeight = childCount (2), so the parent
        // vertical group split the spare space 2:1 and the header ballooned to most of the screen.
        var buggy = new[]
        {
            VerticalLayoutModel.Band.Unpinned("Header", HeaderDp, HeaderDp, reportedFlexible: 2f),
            VerticalLayoutModel.Band.Unpinned("Tabs", TabsDp, TabsDp, reportedFlexible: 2f),
            VerticalLayoutModel.Band.Grow("Viewport", ViewportMinDp, ViewportMinDp),
        };
        float[] before = VerticalLayoutModel.Distribute(buggy, 560f, SpacingDp, PaddingDp);
        Assert.True(before[0] > 120f, $"model must reproduce the header blowup (~140 on a 560 screen): got {before[0]}");
        Assert.True(before[1] > 120f, $"model must reproduce the tab blowup: got {before[1]}");

        var fixedBands = new[]
        {
            VerticalLayoutModel.Band.Fixed("Header", HeaderDp),
            VerticalLayoutModel.Band.Fixed("Tabs", TabsDp),
            VerticalLayoutModel.Band.Grow("Viewport", ViewportMinDp, ViewportMinDp),
        };
        float[] after = VerticalLayoutModel.Distribute(fixedBands, 560f, SpacingDp, PaddingDp);
        Assert.Equal(HeaderDp, after[0], 3);
        Assert.Equal(TabsDp, after[1], 3);
        Assert.True(after[2] > before[2], "pinning flexibleHeight=0 must hand the slack back to the viewport");
    }
}

/// <summary>
/// Source guards for the shipped view. These assert the actual code shape that fixes the layout /
/// implements the mockup — they do NOT prove the runtime render. The numeric harness above carries
/// the sizing budget; these carry the "the fix is actually in the file" contract.
/// </summary>
public sealed class BankAppLayoutSourceContractTests
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

    private static string MethodBody(string source, string signature)
    {
        int start = source.IndexOf(signature, StringComparison.Ordinal);
        Assert.True(start >= 0, $"missing method: {signature}");
        int brace = source.IndexOf('{', start);
        Assert.True(brace >= 0, $"missing body for: {signature}");
        int depth = 0;
        for (int i = brace; i < source.Length; i++)
        {
            if (source[i] == '{') depth++;
            else if (source[i] == '}')
            {
                depth--;
                if (depth == 0) return source.Substring(brace, i - brace + 1);
            }
        }
        throw new InvalidOperationException($"unbalanced body for {signature}");
    }

    private static int Count(string haystack, string needle)
    {
        int count = 0, i = 0;
        while ((i = haystack.IndexOf(needle, i, StringComparison.Ordinal)) >= 0) { count++; i += needle.Length; }
        return count;
    }

    private static float DpValue(string body, string nameToken)
    {
        var m = Regex.Match(body, Regex.Escape("\"" + nameToken + "\"") + @"[^)]*UITheme\.Dp\((\d+(?:\.\d+)?)f\)");
        Assert.True(m.Success, $"no UITheme.Dp value near \"{nameToken}\"");
        return float.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
    }

    private static int SpValue(string body, string token)
    {
        var m = Regex.Match(body, Regex.Escape(token) + @"[^;]*?UITheme\.Sp\((\d+)\)");
        Assert.True(m.Success, $"no UITheme.Sp value after {token}");
        return int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
    }

    // ---- root cause: fixed bands must not report height flexibility ----

    [Fact]
    public void SetHeight_PinsFlexibleHeightToZeroAndRaisesPriority()
    {
        string body = MethodBody(AppSource(), "private static void SetHeight(GameObject go, float height)");

        Assert.Contains("le.flexibleHeight = 0f;", body, StringComparison.Ordinal);
        Assert.Contains("le.layoutPriority = 1;", body, StringComparison.Ordinal);
    }

    [Fact]
    public void FixedTopBands_DoNotForceExpandVertically()
    {
        string src = AppSource();

        Assert.Contains("childForceExpandHeight = false", MethodBody(src, "private void BuildHeader(Transform parent)"), StringComparison.Ordinal);
        Assert.DoesNotContain("BuildTabStrip", src, StringComparison.Ordinal);
    }

    // ---- Overview matches the mockup ordering / prominence ----

    [Fact]
    public void Overview_OrderIsBalanceThenActionsThenWeekly()
    {
        string body = MethodBody(AppSource(), "private void BuildOverview(Transform content)");

        int balance = body.IndexOf("BuildBalanceCard", StringComparison.Ordinal);
        int actions = body.IndexOf("BuildActionsCard", StringComparison.Ordinal);
        int weekly = body.IndexOf("BuildWeeklyCard", StringComparison.Ordinal);

        Assert.True(balance >= 0 && actions > balance && weekly > actions,
            "Overview must be balance hero → large action tiles → weekly card");
    }

    [Fact]
    public void BalanceHero_IsLargeWithDominantValue()
    {
        string body = MethodBody(AppSource(), "private void BuildBalanceCard(Transform parent)");

        Assert.True(DpValue(body, "BalanceCard") >= 140f, "balance hero band should be ~150dp");
        Assert.True(SpValue(body, "\"OnlineBalance\"") >= 30, "balance value must dominate");
    }

    [Fact]
    public void ActionTiles_AreLargeAndEqualWidth()
    {
        string body = MethodBody(AppSource(), "private void BuildActionsCard(Transform parent)");

        Assert.True(DpValue(body, "ActionsCard") >= 100f, "action band should be ~110dp");
        Assert.Contains("childForceExpandWidth = true", body, StringComparison.Ordinal); // equal-width tiles
        Assert.Contains("BuildActionButton(row.transform, \"Deposit\"", body, StringComparison.Ordinal);
        Assert.Contains("BuildActionButton(row.transform, \"Withdraw\"", body, StringComparison.Ordinal);
    }

    // ---- Transaction pane: 3x2 preset grid + pinned primary button ----

    [Fact]
    public void ChipGrid_IsThreeColumnsByTwoRowsThenRelativeRow()
    {
        string body = MethodBody(AppSource(), "private void BuildChipGrid(Transform parent)");

        Assert.Equal(3, Count(body, "BuildChipTriple(parent,"));
        Assert.DoesNotContain("BuildChipRow(", body, StringComparison.Ordinal);

        int[] order =
        {
            body.IndexOf("\"$100\"", StringComparison.Ordinal),
            body.IndexOf("\"$500\"", StringComparison.Ordinal),
            body.IndexOf("\"$1,000\"", StringComparison.Ordinal),
            body.IndexOf("\"$2,500\"", StringComparison.Ordinal),
            body.IndexOf("\"$5,000\"", StringComparison.Ordinal),
            body.IndexOf("\"$10,000\"", StringComparison.Ordinal),
        };
        for (int i = 0; i < order.Length; i++) Assert.True(order[i] >= 0, "missing preset label");
        for (int i = 1; i < order.Length; i++) Assert.True(order[i] > order[i - 1], "presets must stay in order");

        Assert.Contains("\"25%\"", body, StringComparison.Ordinal);
        Assert.Contains("\"50%\"", body, StringComparison.Ordinal);
        Assert.Contains("\"MAX\"", body, StringComparison.Ordinal);
    }

    [Fact]
    public void ChipTriple_BuildsEqualWidthChipsInOneRow()
    {
        string body = MethodBody(AppSource(), "private void BuildChipTriple(Transform parent, params");

        Assert.Contains("foreach", body, StringComparison.Ordinal);              // one row, N chips
        Assert.Equal(1, Count(body, "BuildChip(row.transform"));                  // single call site per chip
        Assert.Contains("childForceExpandWidth = true", body, StringComparison.Ordinal); // equal width
    }

    [Fact]
    public void ChipGrid_SourcesSixFixedPresetsTwoRelativeAndMax()
    {
        string body = MethodBody(AppSource(), "private void BuildChipGrid(Transform parent)");

        Assert.Equal(6, Count(body, "TransferMath.AmountPresets["));
        Assert.Equal(2, Count(body, "TransferMath.RelativePresets["));
        Assert.Equal(3, Count(body, "CurrentAllowedMax()")); // 25%, 50% ceiling + the MAX chip
    }

    [Fact]
    public void TransactionPane_PinsPrimaryButtonAndUsesACompactHead()
    {
        string body = MethodBody(AppSource(), "private GameObject BuildTransactionPane(Transform parent");

        Assert.Contains("BuildScroll(", body, StringComparison.Ordinal);                // the form scrolls
        Assert.Contains("BuildConfirmButton(pane.transform)", body, StringComparison.Ordinal); // primary pinned in the footer
        Assert.Contains("BuildPreviewCard(scrollContent)", body, StringComparison.Ordinal);   // preview lives in the body

        var m = Regex.Match(body, @"Row\(pane\.transform,\s*UITheme\.Dp\((\d+)f\)\)");
        Assert.True(m.Success, "drill-in head band not found");
        Assert.True(int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture) <= 44, "drill-in head must be compact");
    }

    [Fact]
    public void TransactionAmount_IsDominant()
    {
        string body = MethodBody(AppSource(), "private void BuildAmountCard(Transform parent)");

        Assert.True(SpValue(body, "\"Text\"") >= 24, "amount field text must dominate the pane");
    }

}
