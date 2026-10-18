using System.Text.RegularExpressions;
using Xunit;

namespace Minimap.Tests;

public sealed class SettingsInteropRegressionTests
{
    // Source guard: the pure test host cannot execute Unity's IL2CPP runtime.
    // This bans the exact implicit cast observed in Latest.log, not a layout simulation.
    [Fact]
    public void PageStretchDoesNotEnumerateIl2CppTransformAsManagedTransform()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md")))
            dir = dir.Parent;
        Assert.NotNull(dir);
        var source = File.ReadAllText(Path.Combine(dir!.FullName,
            "Source", "Mods", "Minimap", "src", "MinimapSettingsApp.cs"));
        var start = source.IndexOf("private void ApplyPageStretch()", StringComparison.Ordinal);
        var end = source.IndexOf("private void AddSectionLabel", start, StringComparison.Ordinal);
        var method = source.Substring(start, end - start);
        Assert.False(Regex.IsMatch(method, @"foreach\s*\(\s*Transform\s+\w+\s+in"),
            "Transform enumeration implicitly casts Il2CppSystem.Object; use childCount/GetChild(index).");
        Assert.Contains("pageRt.GetChild(childIndex)", method);
    }
}
