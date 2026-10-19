using System;
using System.Collections.Generic;

namespace LightControl.Core;

/// <summary>
/// Unity-free accounting for a cooperative, frame-budgeted scene traversal. A scan may visit at
/// most <see cref="MaxNodesPerFrame"/> nodes and spend at most <see cref="MaxMillisecondsPerFrame"/>
/// per frame, and stops permanently at <see cref="TotalNodeCap"/> nodes with a truncation status.
/// <see cref="FairRootOrder"/> rotates the starting root so successive scans do not always starve
/// the same end of the scene.
/// </summary>
public sealed class SceneScanBudget
{
    public SceneScanBudget(int maxNodesPerFrame, double maxMillisecondsPerFrame, int totalNodeCap)
    {
        MaxNodesPerFrame = maxNodesPerFrame;
        MaxMillisecondsPerFrame = maxMillisecondsPerFrame;
        TotalNodeCap = totalNodeCap;
    }

    public int MaxNodesPerFrame { get; }
    public double MaxMillisecondsPerFrame { get; }
    public int TotalNodeCap { get; }

    /// <summary>True while this frame may visit another node.</summary>
    public bool CanVisit(int nodesVisitedThisFrame, double elapsedMillisecondsThisFrame) =>
        nodesVisitedThisFrame >= 0 && nodesVisitedThisFrame < MaxNodesPerFrame &&
        double.IsFinite(elapsedMillisecondsThisFrame) && elapsedMillisecondsThisFrame < MaxMillisecondsPerFrame;

    /// <summary>True once the traversal reached the hard node cap and must be reported as truncated.</summary>
    public bool IsTruncated(int totalNodesVisited) => totalNodesVisited >= TotalNodeCap;

    /// <summary>
    /// Root indices for one scan cycle, starting at <paramref name="startIndex"/> and wrapping so
    /// every root is visited exactly once and the start point rotates across cycles.
    /// </summary>
    public static int[] FairRootOrder(int rootCount, int startIndex)
    {
        if (rootCount <= 0) return Array.Empty<int>();
        int start = ((startIndex % rootCount) + rootCount) % rootCount;
        var order = new int[rootCount];
        for (int i = 0; i < rootCount; i++) order[i] = (start + i) % rootCount;
        return order;
    }
}
