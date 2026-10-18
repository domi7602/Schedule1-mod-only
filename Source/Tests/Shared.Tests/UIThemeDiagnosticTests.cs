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
            string candidate = Path.Combine(directory.FullName, "Source", "Mods", "Shared", "src", "UITheme.cs");
            if (File.Exists(candidate)) { source = File.ReadAllText(candidate); break; }
            directory = directory.Parent;
        }
        Assert.NotNull(source);
        Assert.DoesNotContain("Container Rect looks landscape", source);
        Assert.Contains("float h = Mathf.Max(r.width, r.height);", source);
        Assert.Contains("float w = Mathf.Min(r.width, r.height);", source);
        Assert.Contains("Scale = Mathf.Clamp(ActualHeight / refHeight, minScale, maxScale);", source);
    }
}
