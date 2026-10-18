using GraphicsProbe;
using Xunit;

public class WaterHeuristicsTests
{
    [Theory]
    [InlineData("OceanSurface", "", "", "renderer-name:ocean")]
    [InlineData("dock", "Water (Instance)", "", "material-name:water")]
    [InlineData("plane", "blue", "Custom/WaterURP", "shader-name:water")]
    [InlineData("RIVER", null, null, "renderer-name:river")]
    public void RetainsSourceAndMatchedTerm(string renderer, string? material, string? shader, string evidence)
        => Assert.Contains(evidence, WaterHeuristics.Evidence(renderer, material, shader));

    [Theory]
    [InlineData(null, null, null)]
    [InlineData("building", "Concrete", "Universal Render Pipeline/Lit")]
    [InlineData("plane", "blue", "Transparent")]
    [InlineData("waveform", "", "")]
    public void DoesNotTreatAppearanceOrGenericTransparencyAsProof(string? renderer, string? material, string? shader)
        => Assert.Empty(WaterHeuristics.Evidence(renderer, material, shader));

    [Theory]
    [InlineData("Seat", "LeatherSeat", "Universal Render Pipeline/Lit")]
    [InlineData("Research", "ResearchDesk", "")]
    public void SeaClueDoesNotMatchSeatOrResearch(string renderer, string material, string shader)
        => Assert.Empty(WaterHeuristics.Evidence(renderer, material, shader));

    [Theory]
    [InlineData("Sea", "renderer-name:sea")]
    [InlineData("World/sea/surface", "renderer-name:sea")]
    public void StandaloneSeaTokenIsStillEvidence(string renderer, string expected)
        => Assert.Contains(expected, WaterHeuristics.Evidence(renderer, null, null));

    [Fact]
    public void MultipleCluesRemainUnconfirmedEvidenceNotASemanticVerdict()
    {
        var clues = WaterHeuristics.Evidence("WaterPlane", "Ocean", "Custom/River");
        Assert.Equal(3, clues.Count);
        Assert.All(clues, clue => Assert.Contains(":", clue));
    }
}
