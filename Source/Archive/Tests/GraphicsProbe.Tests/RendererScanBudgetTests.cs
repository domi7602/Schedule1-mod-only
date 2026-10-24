using GraphicsProbe;
using Xunit;

public class RendererScanBudgetTests
{
    [Fact]
    public void ParticleFloodBeforeLaterMeshDoesNotSpendSurfaceScanOrRetentionCapacity()
    {
        var budget = new RendererScanBudget(surfaceLimit: 1, effectLimit: 1);
        var surfaces = new BoundedSection<string>(1);
        var effects = new BoundedSection<string>(1);
        foreach (string type in Enumerable.Repeat("UnityEngine.ParticleSystemRenderer", 50).Append("UnityEngine.MeshRenderer"))
        {
            if (!budget.TryScan(type, out bool effect)) continue;
            (effect ? effects : surfaces).Consider(true, () => type);
        }
        Assert.Equal("UnityEngine.MeshRenderer", Assert.Single(surfaces.Items));
        Assert.Single(effects.Items);
        Assert.Equal(49, budget.EffectScanOmissions);
        Assert.Equal(0, budget.SurfaceScanOmissions);
        Assert.Equal(50, budget.CountsByNativeType["UnityEngine.ParticleSystemRenderer"]);
    }

    [Theory]
    [InlineData("UnityEngine.ParticleSystemRenderer", true)]
    [InlineData("UnityEngine.LineRenderer", true)]
    [InlineData("UnityEngine.TrailRenderer", true)]
    [InlineData("UnityEngine.MeshRenderer", false)]
    [InlineData("UnityEngine.SkinnedMeshRenderer", false)]
    public void EffectsHaveSeparateDiagnosticCategory(string type, bool expected)
        => Assert.Equal(expected, RendererScanBudget.IsEffect(type));
}
