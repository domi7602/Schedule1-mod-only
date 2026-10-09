using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public sealed class TaxiVisualPolicyTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(3, true)]
    public void AVisualSwapRequiresAtLeastOneUsableRenderer(int rendererCount, bool expected)
    {
        Assert.Equal(expected, TaxiVisualPolicy.CanCommitSwap(rendererCount));
    }

    [Theory]
    [InlineData(0, 1, 1, false)]
    [InlineData(3, 0, 0, false)]
    [InlineData(3, 1, 0, false)]
    [InlineData(3, 1, 1, true)]
    [InlineData(3, 2, 1, true)]
    public void MeshNeedsVerticesAndAtLeastOneIndexedSubmesh(
        int vertexCount, int subMeshCount, int indexedSubMeshCount, bool expected)
    {
        Assert.Equal(
            expected,
            TaxiVisualPolicy.HasUsableGeometry(vertexCount, subMeshCount, indexedSubMeshCount));
    }
}
