using Xunit;

namespace Minimap.Tests;

public sealed class VisibilityDiagnosticsTests
{
    /// <summary>
    /// Resolves a mod source file by name from the src root, searching the domain
    /// subfolders (src/&lt;Domain&gt;/&lt;File&gt;.cs) as well as the root itself.
    /// </summary>
    private static string ReadSource(string name)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md"))) dir = dir.Parent;
        string root = Path.Combine(dir!.FullName, "Source", "Mods", "Minimap", "src");

        string direct = Path.Combine(root, name);
        if (File.Exists(direct)) return File.ReadAllText(direct);

        string[] matches = Directory.GetFiles(root, name, SearchOption.AllDirectories);
        if (matches.Length == 1) return File.ReadAllText(matches[0]);
        if (matches.Length > 1)
            throw new IOException($"Ambiguous Minimap source name '{name}': {string.Join(", ", matches)}");
        throw new FileNotFoundException("Minimap source file not found", name);
    }

    // Wiring guards: the test host cannot execute Unity input or IL2CPP Phone state.
    [Fact]
    public void PersistentVisibilityChangesAreTracedBeforeSaving()
    {
        var s = ReadSource("MinimapMod.cs");
        Assert.Contains("TraceVisibility(\"save:\" + caller)", s);
        Assert.Contains("[CallerMemberName] string caller", s);
        Assert.Contains("TraceVisibility(\"update:unattributed\")", s);
    }

    [Fact]
    public void KeyboardCannotToggleVisibility()
    {
        var s = ReadSource("MinimapMod.cs");
        Assert.Contains("[MinimapVisibility] Map key M pressed", s);
        Assert.DoesNotContain("Input.GetKeyDown(_toggleKeyCode)", s);
        Assert.DoesNotContain("_toggleKeyCode", s);
        Assert.Contains("TraceVisibility(\"startup\")", s);
        Assert.Contains("Input.GetKeyDown(_zoomInKeyCode)", s);
        Assert.Contains("Input.GetKeyDown(_zoomOutKeyCode)", s);
    }

    [Fact]
    public void RetiredVisibilityHotkeyCannotBeReboundInSettings()
    {
        Assert.DoesNotContain("AddKeybindRow(keys, \"Toggle\"", ReadSource("MinimapSettingsApp.cs"));
        Assert.Contains("public string ToggleKey { get; set; } = \"None\";", ReadSource("MinimapConfig.cs"));
    }

    [Fact]
    public void LegacyHotkeyMigrationRecoversVisibilityOnlyOnce()
    {
        var s = ReadSource("MinimapMod.cs");
        Assert.Contains("if (!string.Equals(Config.ToggleKey, \"None\", StringComparison.OrdinalIgnoreCase))", s);
        Assert.Contains("Config.ToggleKey = \"None\";", s);
        Assert.Contains("Config.MinimapVisible = true;", s);
        Assert.Contains("[MinimapVisibility] Keyboard visibility toggle removed; legacy off state recovered.", s);
    }

    [Fact]
    public void ConsoleHelpDoesNotAdvertiseRemovedVisibilityHotkey()
    {
        var s = ReadSource("MinimapCommands.cs");
        Assert.DoesNotContain("[M] Toggle HUD", s);
        Assert.Contains("No keyboard visibility toggle", s);
    }

    [Fact]
    public void SettingsSwitchIdentifiesTheChangedControl()
    {
        var s = ReadSource("MinimapSettingsApp.cs");
        Assert.Contains("TraceVisibility(\"settings:\" + _label)", s);
    }
}
