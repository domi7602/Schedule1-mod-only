using Xunit;

namespace PropertyLEDs.Tests;

// Source contracts only: these do not exercise IL2CPP player state or Unity rendering.
public sealed class PropertySelectionWiringTests
{
    private static string Source(string fileName)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md"))) dir = dir.Parent;
        string root = Path.Combine(dir!.FullName, "Source", "Mods", "PropertyLEDs", "src");
        string path = Path.Combine(root, fileName);
        if (!File.Exists(path)) throw new FileNotFoundException("PropertyLEDs source file not found", fileName);
        return File.ReadAllText(path);
    }

    [Fact]
    public void AppUsesTheTestedSelectionPolicyAndNamesTheSelectedProperty()
    {
        string app = Source("PropertyLEDsApp.cs");
        Assert.Contains("private readonly PropertySelection _selection = new();", app);
        Assert.Contains("LEDRuntime.CurrentPropertyId()", app);
        Assert.Contains("_selection.Resolve(ids, currentId)", app);
        Assert.Contains("_selection.Select(next);", app);
        Assert.Contains("PropertyStatus.Label(property.DisplayName, _shown.Enabled)", app);
        Assert.Contains("PropertyStatus.Detail(_shown.Enabled)", app);
        Assert.DoesNotContain("_propertyId = properties[0].Id;", app);
    }

    [Fact]
    public void HiddenUiCreationCannotConsumeTheFirstOpenSelection()
    {
        string app = Source("PropertyLEDsApp.cs");
        Assert.Contains("if (ready && IsOpen() && propertyIndex < 0 && properties.Count > 0)", app);
    }

    [Fact]
    public void RefreshErrorsDoNotDiscardManualSelection()
    {
        string app = Source("PropertyLEDsApp.cs");
        int start = app.IndexOf("private void Update()", StringComparison.Ordinal);
        int end = app.IndexOf("private void ToggleAdvanced()", start, StringComparison.Ordinal);
        string update = app.Substring(start, end - start);
        int handler = update.IndexOf("catch (Exception ex)", StringComparison.Ordinal);
        string recovery = update.Substring(handler);
        Assert.DoesNotContain("Invalidate();", recovery);
        Assert.DoesNotContain("_selection.Invalidate()", recovery);
        Assert.Contains("_shownRevision = -1;", recovery);
        Assert.Contains("SetEnabled(false);", recovery);
    }

    [Fact]
    public void ManualSelectionSurvivesAppCloseAndPhoneCloseDoesNotResetIt()
    {
        string app = Source("PropertyLEDsApp.cs");
        int close = app.IndexOf("protected override void OnPhoneClosed()", StringComparison.Ordinal);
        int destroy = app.IndexOf("protected override void OnDestroyed()", close, StringComparison.Ordinal);
        Assert.True(close >= 0 && destroy > close);
        string closed = app.Substring(close, destroy - close);
        Assert.DoesNotContain("_selection", closed);
        Assert.DoesNotContain("_propertyId = null", closed);

        int invalidate = app.IndexOf("private void Invalidate()", StringComparison.Ordinal);
        int selected = app.IndexOf("private PropertyEntry? SelectedProperty()", invalidate, StringComparison.Ordinal);
        Assert.True(invalidate >= 0 && selected > invalidate);
        Assert.Contains("_selection.Invalidate();", app.Substring(invalidate, selected - invalidate));
    }

    [Fact]
    public void SelectionUsesOnlyExactNativePropertyIdentityNotPositions()
    {
        string runtime = Source("LEDRuntime.cs");
        Assert.Contains("using NativePlayer = Il2CppScheduleOne.PlayerScripts.Player;", runtime);
        Assert.Contains("var player = NativePlayer.Local;", runtime);
        Assert.Contains("player!.CurrentProperty", runtime);
        Assert.Contains("player.CurrentBusiness", runtime);
        Assert.Contains("return property.PropertyCode;", runtime);
        Assert.Contains("return business.PropertyCode;", runtime);
        Assert.Contains("if (!Ready || !NetworkGuard.IsInMainScene) return null;", runtime);
        Assert.DoesNotContain("Distance(", runtime);
        Assert.DoesNotContain(".position", runtime);

        string app = Source("PropertyLEDsApp.cs");
        Assert.DoesNotContain("Vector3.Distance", app);
        Assert.DoesNotContain("sqrMagnitude", app);
        Assert.DoesNotContain("Anchor.position", app);
    }
}
