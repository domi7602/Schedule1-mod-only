using Xunit;

namespace WaterShader.Tests;

public sealed class WaterSurfaceMotionWiringTests
{
    [Fact]
    public void AnimatedWaveStructureModulatesWaterBodyWithoutAddingLight()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "AGENTS.md"))) directory = directory.Parent;
        string shader = File.ReadAllText(Path.Combine(directory!.FullName, "Source/Mods/WorldLook/unity/Assets/S1Mods/ReferenceWater.shader"));
        Assert.Contains("float WaterSurfaceShade(", shader);
        Assert.Contains("WaterSurfaceShade(IN.positionWS.xz, _Time.y)", shader);
        Assert.Contains("transmitted *= surfaceShade", shader);
    }
}
