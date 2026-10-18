using Xunit;

namespace Minimap.Tests;

public sealed class BlipPerformanceWiringTests
{
    private static string Source()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "AGENTS.md"))) dir = dir.Parent;
        return File.ReadAllText(Path.Combine(dir!.FullName, "Source", "Mods", "Minimap", "src", "MinimapBlips.cs"));
    }

    [Fact]
    public void SnapshotProjectionIsInvalidatedOnRebuildAndCalibrationChanges()
    {
        var s = Source();
        Assert.Contains("_snapshotGeneration++;", s);
        Assert.Contains("_projectionKeys[i].NeedsUpdate(projectionKey)", s);
        Assert.Contains("_projectionKeys[i].Commit(projectionKey)", s);
        Assert.Contains("converter.GetInstanceID()", s);
        Assert.Contains("converter.OriginPoint", s);
        Assert.Contains("converter.EdgePoint", s);
        Assert.Contains("converter.MapDimensions", s);
        Assert.Contains("converter.conversionFactor", s);
    }

    [Fact]
    public void SharedYawAndConverterLookupAreOutsidePoolLoop()
    {
        var s = Source();
        var render = s[s.IndexOf("public void RenderBlips", StringComparison.Ordinal)..];
        int loop = render.IndexOf("for (int i = 0; i < poolCount; i++)", StringComparison.Ordinal);
        Assert.Contains("Mathf.Cos(rad)", render[..loop]);
        Assert.Contains("MapPositionUtility.Instance", render[..loop]);
        Assert.DoesNotContain("Mathf.Cos", render[loop..]);
        Assert.DoesNotContain("MapPositionUtility.Instance", render[loop..]);
    }

    [Fact]
    public void RenderUsesCachedComponentsAndConditionalNativeWrites()
    {
        var s = Source();
        var render = s[s.IndexOf("public void RenderBlips", StringComparison.Ordinal)..];
        Assert.DoesNotContain("as RectTransform", s);
        Assert.DoesNotContain(".GetComponent<", render);
        Assert.DoesNotContain(".rectTransform", render);
        Assert.Contains("pooled.IconColor.NeedsUpdate(colorKey)", render);
        Assert.Contains("pooled.Dimension.NeedsUpdate(finalDimension)", render);
        Assert.Contains("pooled.PortraitSprite.NeedsUpdate(portraitPointer)", render);
        Assert.Contains("pooled.Alpha.NeedsUpdate(alpha)", render);
    }

    [Fact]
    public void UnityProxyStructsAreNotComparedWithManagedValueTypeEquality()
    {
        // Live IL2CPP Vector2/Color proxies do not implement managed IEquatable<T>.
        // EqualityComparer<T>.Default on them boxes; use their scalar components.
        var s = Source();
        Assert.DoesNotContain("AppliedValue<Vector2>", s);
        Assert.DoesNotContain("AppliedValue<Color>", s);
        Assert.Contains("AppliedValue<(float, float)>", s);
        Assert.Contains("AppliedValue<(float, float, float, float)>", s);
    }

    [Fact]
    public void PoolSizeScanCadenceAndPerFrameEffectsRemainUnchanged()
    {
        var s = Source();
        Assert.Contains("PoolSize = 64", s);
        Assert.Contains("LandmarkScanInterval = 10.0f", s);
        Assert.Contains("DynamicScanInterval = 1.0f", s);
        Assert.Contains("Mathf.Sin(Time.time * 6f) * 0.15f", s);
        Assert.Contains("(itemMapPos - playerMapPos) * zoomFactor", s);
        Assert.Contains("StableCriticalFirst(_activeBlips)", s);
        Assert.Contains("config.Shape == MinimapShape.Circle", s);
        Assert.Contains("config.ClampOnlyCritical && !info.IsCritical", s);
    }
}
