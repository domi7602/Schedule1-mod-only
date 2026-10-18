using GraphicsProbe;
using Xunit;

public class IntrospectionPolicyTests
{
    [Theory]
    [InlineData("Color", "color")]
    [InlineData("Vector", "vector")]
    [InlineData("Float", "float")]
    [InlineData("Range", "float")]
    [InlineData("Texture", "texture")]
    [InlineData(null, "unknown")]
    [InlineData("SomeFutureKind", "unknown")]
    public void ShapeOfMapsUnityShaderPropertyTypesToReadStrategies(string? kind, string expected)
        => Assert.Equal(expected, IntrospectionPolicy.ShapeOf(kind));

    [Fact]
    public void PlanIndicesKeepsPrefixWithinCap()
    {
        Assert.Equal(new[] { 0, 1, 2 }, IntrospectionPolicy.PlanIndices(5, 3));
        Assert.Equal(new[] { 0, 1 }, IntrospectionPolicy.PlanIndices(2, 3));
        Assert.Empty(IntrospectionPolicy.PlanIndices(5, 0));
        Assert.Empty(IntrospectionPolicy.PlanIndices(0, 3));
    }

    [Theory]
    [InlineData(1.5, 1.5)]
    [InlineData(0.0, 0.0)]
    [InlineData(double.NaN, null)]
    [InlineData(double.PositiveInfinity, null)]
    [InlineData(double.NegativeInfinity, null)]
    public void NormalizeKeepsFiniteValuesAndDropsNonFinite(double input, double? expected)
        => Assert.Equal(expected, IntrospectionPolicy.Normalize(input));

    [Fact]
    public void NormalizeComponentsPassesFiniteTuplesThrough()
    {
        var result = IntrospectionPolicy.NormalizeComponents(new[] { 1.0, 2.0, -3.25, 0.0 });
        Assert.NotNull(result);
        Assert.Equal(new[] { 1.0, 2.0, -3.25, 0.0 }, result);
    }

    [Fact]
    public void NormalizeComponentsRejectsWholeTupleWhenAnyComponentIsNonFinite()
    {
        // JSON has no NaN/Infinity representation: one bad component poisons the
        // tuple so the report records a clean null instead of invalid JSON numbers.
        Assert.Null(IntrospectionPolicy.NormalizeComponents(new[] { 1.0, double.NaN }));
        Assert.Null(IntrospectionPolicy.NormalizeComponents(new[] { double.PositiveInfinity, 1.0 }));
        Assert.Null(IntrospectionPolicy.NormalizeComponents(new[] { 0.0, double.NegativeInfinity }));
    }

    [Fact]
    public void NormalizeComponentsRejectsEmptyTuples()
        => Assert.Null(IntrospectionPolicy.NormalizeComponents(new double[0]));

    [Fact]
    public void NormalizeComponentsRejectsMissingTuples()
        => Assert.Null(IntrospectionPolicy.NormalizeComponents(null));

    [Fact]
    public void PlanIndicesClampsDegenerateInputs()
    {
        Assert.Empty(IntrospectionPolicy.PlanIndices(-3, 5));
        Assert.Empty(IntrospectionPolicy.PlanIndices(5, -1));
    }

    [Fact]
    public void ShaderPropertyCapIsPinnedToItsDocumentedValue()
    {
        // 64 per materials/golden-hour evidence scope; a silent bump must fail here.
        Assert.Equal(64, IntrospectionPolicy.MaxShaderProperties);
        Assert.InRange(IntrospectionPolicy.MaxShaderProperties, 1, ReportPolicy.MaxBytes / 16);
    }
}
