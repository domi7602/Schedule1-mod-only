using System;
using System.IO;
using Xunit;

namespace Shared.Tests;

/// <summary>Source contract only; no Unity layout execution.</summary>
public class UIThemeDiagnosticTests
{
    [Fact]
    public void PortraitNormalization_DoesNotWarnForAnExpectedRotatedPhoneRect()
    {
        DirectoryInfo? directory = new DirectoryInfo(AppContext.BaseDirectory);
        string? source = null;
        while (directory != null)
        {
            string root = Path.Combine(directory.FullName, "Source", "Mods", "Shared", "src");
            if (Directory.Exists(root))
            {
                // Layout-independent: UITheme.cs lives in a domain subfolder (src/UI/UITheme.cs).
                string path = Assert.Single(Directory.GetFiles(root, "UITheme.cs", SearchOption.AllDirectories));
                source = File.ReadAllText(path);
                break;
            }
            directory = directory.Parent;
        }
        Assert.NotNull(source);
        Assert.DoesNotContain("Container Rect looks landscape", source);
        Assert.Contains("float h = Mathf.Max(r.width, r.height);", source);
        Assert.Contains("float w = Mathf.Min(r.width, r.height);", source);
        Assert.Contains("Scale = Mathf.Clamp(ActualHeight / refHeight, minScale, maxScale);", source);
    }
}
