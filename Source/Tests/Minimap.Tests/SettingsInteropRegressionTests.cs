using System.Text.RegularExpressions;
using Xunit;

namespace Minimap.Tests;

public sealed class SettingsInteropRegressionTests
{
    /// <summary>
    /// Resolves a mod source file by name from the src root, searching the domain
    /// subfolders (src/&lt;Domain&gt;/&lt;File&gt;.cs) as well as the root itself.
    /// </summary>
    private static string ReadSource(DirectoryInfo repoRoot, string name)
    {
        string root = Path.Combine(repoRoot.FullName, "Source", "Mods", "Minimap", "src");

        string direct = Path.Combine(root, name);
        if (File.Exists(direct)) return File.ReadAllText(direct);

        string[] matches = Directory.GetFiles(root, name, SearchOption.AllDirectories);
        if (matches.Length == 1) return File.ReadAllText(matches[0]);
        if (matches.Length > 1)
            throw new IOException($"Ambiguous Minimap source name '{name}': {string.Join(", ", matches)}");
        throw new FileNotFoundException("Minimap source file not found", name);
    }

    // Source guard: the pure test host cannot execute Unity's IL2CPP runtime.
    // This bans the exact implicit cast observed in Latest.log, not a layout simulation.
    [Fact]
    public void PageStretchDoesNotEnumerateIl2CppTransformAsManagedTransform()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md")))
            dir = dir.Parent;
        Assert.NotNull(dir);
        var source = ReadSource(dir!, "MinimapSettingsApp.cs");
        var start = source.IndexOf("private void ApplyPageStretch()", StringComparison.Ordinal);
        var end = source.IndexOf("private void AddSectionLabel", start, StringComparison.Ordinal);
        var method = source.Substring(start, end - start);
        Assert.False(Regex.IsMatch(method, @"foreach\s*\(\s*Transform\s+\w+\s+in"),
            "Transform enumeration implicitly casts Il2CppSystem.Object; use childCount/GetChild(index).");
        Assert.Contains("pageRt.GetChild(childIndex)", method);
    }
}
