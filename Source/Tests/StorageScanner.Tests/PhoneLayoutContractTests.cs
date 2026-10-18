using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace StorageScanner.Tests;

/// <summary>Source configuration guards, not a substitute for Unity rendering tests.</summary>
public sealed class PhoneLayoutContractTests
{
    /// <summary>
    /// Resolves a mod source file by name from the src root, searching the domain
    /// subfolders (src/&lt;Domain&gt;/&lt;File&gt;.cs) as well as the root itself. Keeping the
    /// lookup layout-independent means a source reorganization does not invalidate
    /// every contract test that reads a file.
    /// </summary>
    private static string ReadSource(string name)
    {
        for (DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
        {
            string root = Path.Combine(dir.FullName, "Source", "Mods", "StorageScanner", "src");
            if (!Directory.Exists(root)) continue;

            string direct = Path.Combine(root, name);
            if (File.Exists(direct)) return File.ReadAllText(direct);

            string[] matches = Directory.GetFiles(root, name, SearchOption.AllDirectories);
            if (matches.Length == 1) return File.ReadAllText(matches[0]);
            if (matches.Length > 1)
                throw new IOException($"Ambiguous StorageScanner source name '{name}': {string.Join(", ", matches)}");
            throw new FileNotFoundException("StorageScanner source file not found", name);
        }
        throw new FileNotFoundException("StorageScanner source root not found", name);
    }

    private static string AppSource() => ReadSource("StorageScannerApp.cs");

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
