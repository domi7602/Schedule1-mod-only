using System.Text.RegularExpressions;
using Xunit;

namespace StorageScanner.Tests;

/// <summary>Source configuration guards, not a substitute for Unity rendering tests.</summary>
public sealed class PhoneLayoutContractTests
{
    private static string AppSource()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null)
        {
            string candidate = Path.Combine(directory.FullName, "Source", "Mods", "StorageScanner", "src", "StorageScannerApp.cs");
            if (File.Exists(candidate)) return File.ReadAllText(candidate);
            directory = directory.Parent;
        }
        throw new FileNotFoundException("Run these repository layout guards from a repository build output.");
    }

    [Fact]
    public void FixedHeight_OverridesLayoutGroupFlexibleHeight()
    {
        string body = Regex.Match(AppSource(), @"private static void SetHeight\([^)]*\)\s*\{([^}]+)\}").Groups[1].Value;
        Assert.Matches(@"element\.flexibleHeight\s*=\s*0f\s*;", body);
        Assert.Matches(@"element\.layoutPriority\s*=\s*1\s*;", body);
    }

    [Fact]
    public void HorizontalGroups_DoNotForceVerticalExpansion()
    {
        string body = Regex.Match(AppSource(), @"private static void Horizontal\([^)]*\)\s*\{([^}]+)\}").Groups[1].Value;
        Assert.Matches(@"layout\.childForceExpandHeight\s*=\s*false\s*;", body);
    }
}
