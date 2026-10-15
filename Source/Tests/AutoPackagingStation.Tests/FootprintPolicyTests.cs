using System;
using System.Collections.Generic;
using Xunit;

namespace AutoPackagingStation.Tests;

public sealed class FootprintPolicyTests
{
    [Fact]
    public void CompleteOffsetFourByFourMappingIsAccepted()
    {
        var cells = Grid(7, -3);
        Assert.True(FootprintPolicy.IsCompleteFourByFour(cells));
    }

    [Fact]
    public void DuplicateCellCannotMasqueradeAsSixteenTiles()
    {
        var cells = Grid(0, 0);
        cells[15] = cells[0];
        Assert.False(FootprintPolicy.IsCompleteFourByFour(cells));
    }

    [Fact]
    public void SparseOrWrongShapeMappingIsRejected()
    {
        var cells = Grid(0, 0);
        cells.RemoveAt(0);
        Assert.False(FootprintPolicy.IsCompleteFourByFour(cells));
        cells.Add((8, 8));
        Assert.False(FootprintPolicy.IsCompleteFourByFour(cells));
    }

    private static List<(int X, int Y)> Grid(int x0, int y0)
    {
        var cells = new List<(int X, int Y)>();
        for (int y = 0; y < 4; y++)
            for (int x = 0; x < 4; x++) cells.Add((x0 + x, y0 + y));
        return cells;
    }
}
