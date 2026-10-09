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
    private const float ViewportMinDp = 200f;
    private const float SpacingDp = 6f;
    private const float PaddingDp = 18f; // top 8 + bottom 10

    [Fact]
    public void SingleScreen_AtReferenceCanvas_KeepsHeaderCompactAndViewportDominant()
    {
        var bands = new[]
        {
            VerticalLayoutModel.Band.Fixed("Header", HeaderDp),
            VerticalLayoutModel.Band.Grow("Viewport", ViewportMinDp, ViewportMinDp),
        };

        float[] h = VerticalLayoutModel.Distribute(bands, 750f, SpacingDp, PaddingDp);

        Assert.Equal(HeaderDp, h[0], 3);          // header never absorbs slack
        Assert.True(h[1] >= ViewportMinDp);       // viewport keeps its floor
        Assert.True(h[1] > 600f, $"viewport should dominate: got {h[1]}");
        Assert.True(h[0] <= 0.08f * 750f, $"header should be <= 8% of 750: got {h[0]}");
    }

    [Fact]
    public void SingleScreen_AtInstalledScale_StillLeavesViewportDominant()
    {
        // Installed phone rect ~300x560 → UITheme.Scale = clamp(560/750, 0.85, 2.0) = 0.85.
        const float scale = 0.85f;
        float available = 560f;
        float dp(float v) => v * scale;

        var bands = new[]
        {
            VerticalLayoutModel.Band.Fixed("Header", dp(HeaderDp)),
            VerticalLayoutModel.Band.Grow("Viewport", dp(ViewportMinDp), dp(ViewportMinDp)),
        };

        float[] h = VerticalLayoutModel.Distribute(bands, available, dp(SpacingDp), dp(PaddingDp));
        Assert.True(h[0] <= 0.10f * available, $"header should be <= 10% of the phone: got {h[0]}");
        Assert.True(h[1] >= 0.70f * available, $"viewport should dominate the phone: got {h[1]}");
    }

    [Fact]
    public void UnpinnedBand_ReproducesTheBlowup_AndPinningReturnsSlackToTheViewport()
    {
        // Pre-fix: Header's LayoutElement left flexibleHeight unset (-1); its HorizontalLayoutGroup
        // with childForceExpandHeight=true reports flexibleHeight = childCount (2), so the parent
        // vertical group split the spare space and the header ballooned to most of the screen.
        var buggy = new[]
        {
            VerticalLayoutModel.Band.Unpinned("Header", HeaderDp, HeaderDp, reportedFlexible: 2f),
            VerticalLayoutModel.Band.Grow("Viewport", ViewportMinDp, ViewportMinDp),
        };
        float[] before = VerticalLayoutModel.Distribute(buggy, 560f, SpacingDp, PaddingDp);
        Assert.True(before[0] > 120f, $"model must reproduce the header blowup (~140 on a 560 screen): got {before[0]}");

        var fixedBands = new[]
        {
            VerticalLayoutModel.Band.Fixed("Header", HeaderDp),
            VerticalLayoutModel.Band.Grow("Viewport", ViewportMinDp, ViewportMinDp),
        };
        float[] after = VerticalLayoutModel.Distribute(fixedBands, 560f, SpacingDp, PaddingDp);
        Assert.Equal(HeaderDp, after[0], 3);
        Assert.True(after[1] > before[1], "pinning flexibleHeight=0 must hand the slack back to the viewport");
    }
}

/// <summary>
/// Source guards for the shipped view. These assert the actual code shape that fixes the layout /
/// implements the single screen — they do NOT prove the runtime render. The numeric harness above
/// carries the sizing budget; these carry the "the fix is actually in the file" contract.
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

    /// <summary>Height token anywhere after the named element (tolerates nested calls between them).</summary>
    private static float DpValue(string body, string nameToken)
    {
        var m = Regex.Match(body, Regex.Escape("\"" + nameToken + "\"") + @"[\s\S]{0,240}?UITheme\.Dp\((\d+(?:\.\d+)?)f\)");
        Assert.True(m.Success, $"no UITheme.Dp value after \"{nameToken}\"");
        return float.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
    }

    /// <summary>Height of a <c>Row(parent, UITheme.Dp(nn))</c> band.</summary>
    private static float RowDp(string body)
    {
        var m = Regex.Match(body, @"Row\([^)]*UITheme\.Dp\((\d+(?:\.\d+)?)f\)");
        Assert.True(m.Success, "no Row(...UITheme.Dp(nn)) band in this method");
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
    }

    // ---- single screen: required ordering and prominence ----

    [Fact]
    public void SingleScreen_OrderIsBalanceModeAmountPreviewActionRecent()
    {
        string body = MethodBody(AppSource(), "private void BuildSingleScreen(Transform content)");

        int balance = body.IndexOf("BuildBalanceCard", StringComparison.Ordinal);
        int mode = body.IndexOf("BuildModeSwitcher", StringComparison.Ordinal);
        int amount = body.IndexOf("BuildAmountCard", StringComparison.Ordinal);
        int preview = body.IndexOf("BuildPreviewCard", StringComparison.Ordinal);
        int action = body.IndexOf("BuildConfirmButton", StringComparison.Ordinal);
        int recent = body.IndexOf("BuildRecentCard", StringComparison.Ordinal);

        Assert.True(balance >= 0, "balance hero must be built");
        Assert.True(mode > balance, "mode switcher comes after the balance hero");
        Assert.True(amount > mode, "amount comes after the mode switcher");
        Assert.True(preview > amount, "preview comes after the amount");
        Assert.True(action > preview, "primary action comes after the preview");
        Assert.True(recent > action, "recent activity comes last");
    }

    [Fact]
    public void BalanceHero_IsLargeWithDominantValue()
    {
        string body = MethodBody(AppSource(), "private void BuildBalanceCard(Transform parent)");

        Assert.True(DpValue(body, "BalanceCard") >= 140f, "balance hero band should be ~150dp");
        Assert.True(SpValue(body, "\"OnlineBalance\"") >= 30, "balance value must dominate");
    }

    [Fact]
    public void QuickChips_AreOneRowOfPresetsPlusMax()
    {
        string body = MethodBody(AppSource(), "private void BuildQuickChips(Transform parent)");

        Assert.Contains("TransferMath.AmountPresets[0]", body, StringComparison.Ordinal);
        Assert.Contains("TransferMath.AmountPresets[1]", body, StringComparison.Ordinal);
        Assert.Contains("TransferMath.AmountPresets[2]", body, StringComparison.Ordinal);
        Assert.Contains("\"MAX\"", body, StringComparison.Ordinal);
        Assert.Equal(1, Count(body, "BuildChipRow(parent,"));

        string rowBody = MethodBody(AppSource(), "private void BuildChipRow(Transform parent, params");
        Assert.True(RowDp(rowBody) >= 44f, "chips must keep a touch target of at least 44dp");
    }

    [Fact]
    public void ModeButtons_KeepATouchTarget()
    {
        string body = MethodBody(AppSource(), "private void BuildModeSwitcher(Transform parent)");

        Assert.True(RowDp(body) >= 44f, "mode switcher buttons must be at least 44dp tall");
    }

    [Fact]
    public void PrimaryButton_IsTallEnoughToTap()
    {
        string body = MethodBody(AppSource(), "private void BuildConfirmButton(Transform parent)");

        Assert.True(DpValue(body, "ConfirmButton") >= 44f, "primary action must be at least 44dp tall");
    }

    [Fact]
    public void AmountField_IsDominant()
    {
        string body = MethodBody(AppSource(), "private void BuildAmountCard(Transform parent)");

        Assert.True(SpValue(body, "\"Text\"") >= 24, "amount field text must dominate the screen");
    }
}
