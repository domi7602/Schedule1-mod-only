using Xunit;

namespace MessagesPlus.Tests;

// Source contracts cover only managed wiring, NOT Harmony execution or Unity rendering.
public sealed class OrderThemeWiringTests
{
    private static string Source(string name)
    {
        DirectoryInfo? root = new(AppContext.BaseDirectory);
        while (root != null && !File.Exists(Path.Combine(root.FullName, "AGENTS.md"))) root = root.Parent;
        Assert.NotNull(root);
        // Optional read-only snapshot root lets the same contracts be exercised
        // against git's original source without replacing anyone's working files.
        string sourceRoot = Environment.GetEnvironmentVariable("MESSAGESPLUS_SOURCE_ROOT") ?? root!.FullName;
        string src = Path.Combine(sourceRoot, "Source", "Mods", "MessagesPlus", "src");

        // Resolve from the src root, searching the domain subfolders
        // (src/<Domain>/<File>.cs) as well as the root itself. Keeping the lookup
        // layout-independent means a source reorganization does not invalidate the
        // contract tests that read a file.
        string direct = Path.Combine(src, name);
        if (File.Exists(direct)) return File.ReadAllText(direct);

        string[] matches = Directory.GetFiles(src, name, SearchOption.AllDirectories);
        if (matches.Length == 1) return File.ReadAllText(matches[0]);
        if (matches.Length > 1)
            throw new IOException($"Ambiguous MessagesPlus source name '{name}': {string.Join(", ", matches)}");
        throw new FileNotFoundException("MessagesPlus source file not found", name);
    }

    private static string Method(string source, string signature)
    {
        int start = source.IndexOf(signature, StringComparison.Ordinal);
        Assert.True(start >= 0, $"Missing managed wiring: {signature}");
        int brace = source.IndexOf('{', start);
        int depth = 1, end = brace + 1;
        while (depth > 0 && end < source.Length)
        {
            if (source[end] == '{') depth++;
            if (source[end] == '}') depth--;
            end++;
        }
        return source[brace..end];
    }

    [Fact]
    public void ResponseCreationAndLateFrame_ThemeOnlyKnownActiveRoots()
    {
        string theme = Source("AppTheme.cs");
        Assert.DoesNotContain("PreserveResponseArea", theme);
        Assert.DoesNotContain("PreserveConversationResponseArea", Source("MessagesAppPatch.cs"));
        Assert.Contains("AppTheme.ThemeConversationResponseArea(__instance)", Source("MessagesAppPatch.cs"));
        string fast = Method(theme, "public static void FastPopupPass(");
        Assert.Contains("app.DealWindowSelector", fast);
        Assert.Contains("RefreshDealWindow", fast);
        Assert.Contains("ThemeConversationResponseArea", fast);
        Assert.Contains("app.currentConversation", fast);
        Assert.Contains("activeInHierarchy", fast);
        Assert.DoesNotContain("GetComponentsInChildren", fast);
        Assert.DoesNotContain("Apply(app)", fast);
        Assert.Contains("InboxUI.ThemeFastFrame()", Method(Source("Mod.cs"), "public override void OnLateUpdate("));
    }

    [Fact]
    public void PopupSurfacePass_ProtectsArtAndUsesDedicatedColorPolicy()
    {
        string theme = Source("AppTheme.cs");
        string popup = Method(theme, "private static void ThemePopupSubtree(");
        Assert.Contains("ProtectPopupArt", popup);
        Assert.Contains("TintPopupGraphic", popup);
        string tint = Method(theme, "private static void TintPopupGraphic(");
        Assert.Contains("ThemeRole.PopupSurface", tint);
        Assert.Contains("ThemeSelectable", tint);
        Assert.Contains("IsNeutralColorBlock", tint);
        Assert.Contains("TintDarkTextsIn", popup);
    }

    [Fact]
    public void FinalAppSweep_CannotBypassPopupSurfaceOrTextGuards()
    {
        string theme = Source("AppTheme.cs");
        Assert.Contains("IsPopupSubtree", Method(theme, "private static void TintGraphic("));
        Assert.Contains("ThemeRole.PopupSurface", Method(theme, "private static void TintGraphic("));
        Assert.Contains("IsPopupSubtree", Method(theme, "private static void TintIfDarkText("));
        Assert.Contains("ThemeRole.PopupText", Method(theme, "private static void TintIfDarkText("));
        Assert.Contains("_popupRoots.Clear()", Method(theme, "private static void ClearTracking("));
    }

    [Fact]
    public void PopupColorTint_UsesPaletteOnceAndRejectsColoredBaseBeforeStates()
    {
        string tint = Method(Source("AppTheme.cs"), "private static void TintPopupGraphic(");
        int eligibility = tint.IndexOf("ThemeColorPolicy.IsPopupNeutralSurface", StringComparison.Ordinal);
        int states = tint.IndexOf("GetComponentsInParent<Selectable>", StringComparison.Ordinal);
        Assert.True(eligibility >= 0 && eligibility < states, "Reject coloured Image multipliers before touching ColorTint states.");
        Assert.Contains("ThemeSelectable(selectable, ThemeRole.PopupSurface);\n            return;", tint);
    }

    [Fact]
    public void RuntimeEvidence_IsOneTimePerScopeAndReportsTrackedTargets()
    {
        string theme = Source("AppTheme.cs");
        Assert.Contains("LogScopedPass(selector.Container", Method(theme, "public static void RefreshDealWindow("));
        Assert.Contains("LogScopedPass(container.gameObject", Method(theme, "private static void ThemeResponseArea("));
        string log = Method(theme, "private static void LogScopedPass(");
        Assert.Contains("if (logged", log);
        Assert.Contains("activeInHierarchy", log);
        Assert.Contains("logged = true", log);
        Assert.Contains("[AppTheme] scoped-dark", log);
        Assert.Contains("_graphics", log);
        Assert.Contains("_selectables", log);
    }

    [Fact]
    public void LateFrame_DoesNotDiscoverSceneObjectsOrRecolorRelationshipArt()
    {
        string theme = Source("AppTheme.cs");
        string fast = Method(theme, "public static void FastPopupPass(");
        Assert.DoesNotContain("ResolveOrderPopupRoot", fast);
        Assert.Contains("app.CounterofferInterface", fast);
        Assert.Contains("app.PhoneShopInterface", fast);
        Assert.DoesNotContain("ProtectConversationStatus", Method(theme, "public static void ThemeConversationResponseArea("));
    }

    [Fact]
    public void FullPass_RegistersNativePopupsBeforeSweepAndThemesNeutralHintLabels()
    {
        string theme = Source("AppTheme.cs");
        string full = Method(theme, "private static void ThemeOrderPopups(");
        Assert.Contains("app.PhoneShopInterface", full);
        Assert.Contains("app.CounterofferInterface", full);
        Assert.Contains("(!popup && IsProtectedText(text))", Method(theme, "private static void TintIfDarkText("));
    }

    [Fact]
    public void TrackedPopupImage_CannotBypassTheAccentGuard()
    {
        string tint = Method(Source("AppTheme.cs"), "private static void TintPopupGraphic(");
        Assert.DoesNotContain("!IsTrackedGraphic(image)", tint);
        Assert.Contains("SameRgb(image.color, state!.LastTheme)", tint);
    }

    [Fact]
    public void DealOpen_UsesSelectiveThemeInsteadOfPreservingEntirePopup()
    {
        string hook = Source("DealWindowSelectorPatch.cs");
        Assert.DoesNotContain("PreserveVanillaSubtree(selector.Container)", hook);
        Assert.Contains("AppTheme.RefreshDealWindow(selector)", hook);
        string theme = Source("AppTheme.cs");
        Assert.Contains("selector.CurrentTimeArm", theme);
        Assert.Contains("selector.MorningButton", theme);
        Assert.Contains("selector.AfternoonButton", theme);
        Assert.Contains("selector.NightButton", theme);
        Assert.Contains("selector.LateNightButton", theme);
    }
}
