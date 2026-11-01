using Xunit;

namespace WaterShader.Tests;

public sealed class WaterBroadWaveWiringTests
{
    [Fact]
    public void BroadAnimatedWavesAffectSurfaceAndGuardedRefraction()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md"))) directory = directory.Parent;
        string shader = File.ReadAllText(Path.Combine(directory!.FullName, "Source/Mods/WorldLook/unity/Assets/S1Mods/ReferenceWater.shader"));
        Assert.Contains("float2 BroadWaveGradient(", shader);
        Assert.Contains("BroadWaveGradient(IN.positionWS.xz, _Time.y)", shader);
        Assert.Contains("refractionGrad + broadWave", shader);
        Assert.Contains("surfaceGrad + broadWave", shader);
        Assert.Contains("refractUV = lerp(screenUV, refractUV, refractionValid)", shader);
    }
}
