using GraphicsProbe;
using Xunit;

public class FairTraversalTests
{
    private sealed class Node
    {
        public int Id;
        public List<Node> Children = new();
        public Node(int id) => Id = id;
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static (FairTraversal<Node> Walker, WeakReference Handle) PendingGraph()
    {
        var root = new Node(1); root.Children.Add(new Node(2));
        var walker = new FairTraversal<Node>(n => n.Id, n => n.Children.Count,
            (n, i) => n.Children[i], _ => { }, (_, _) => { });
        walker.AddRoot(0, root); walker.FinishSeeding(); walker.Advance();
        return (walker, new WeakReference(root));
    }

    [Fact]
    public void DisposedWalkerReleasesPendingGraphHandleProxies()
    {
        var (walker, handle) = PendingGraph();
        GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        Assert.True(handle.IsAlive);
        walker.Dispose();
        GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        Assert.False(handle.IsAlive);
        Assert.Equal(0, walker.PendingCursors);
    }

    [Fact]
    public void ExactNodeLimitAtNaturalCompletionDoesNotInventOmittedWork()
    {
        using var complete = new FairTraversal<Node>(n => n.Id, n => n.Children.Count,
            (n, i) => n.Children[i], _ => { }, (_, _) => { }, maxNodes: 1);
        complete.AddRoot(0, new Node(1)); complete.FinishSeeding();
        Assert.True(complete.Advance()); Assert.False(complete.Advance());
        Assert.Null(complete.StopReason);
        using var limited = new FairTraversal<Node>(n => n.Id, n => n.Children.Count,
            (n, i) => n.Children[i], _ => { }, (_, _) => { }, maxNodes: 1);
        var root = new Node(2); root.Children.Add(new Node(3));
        limited.AddRoot(0, root); limited.FinishSeeding();
        Assert.True(limited.Advance()); Assert.False(limited.Advance());
        Assert.Equal("node-limit", limited.StopReason);
        Assert.Equal(1, limited.PendingCursors);
    }

    [Fact]
    public void ParentsRotateAfterOneChildAndGrandchildrenWaitForTheWholeLevel()
    {
        var a = new Node(1); var b = new Node(2);
        a.Children.AddRange(new[] { new Node(3), new Node(5), new Node(7) });
        b.Children.AddRange(new[] { new Node(4), new Node(6) });
        a.Children[0].Children.Add(new Node(8));
        var visited = new List<int>(); int childCalls = 0;
        using var walker = new FairTraversal<Node>(n => n.Id, n => n.Children.Count,
            (n, i) => { childCalls++; return n.Children[i]; }, n => visited.Add(n.Id), (_, _) => { });
        walker.AddRoot(0, a); walker.AddRoot(1, b); walker.FinishSeeding();
        int previous = 0;
        while (walker.Advance()) { Assert.InRange(childCalls - previous, 0, 1); previous = childCalls; }
        Assert.Equal(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, visited);
        Assert.Equal(8, walker.Attempts);
    }

    [Fact]
    public void RootFrontierDepthChildAndNodeBoundsAreExplicit()
    {
        var a = new Node(1); var b = new Node(2);
        a.Children.AddRange(new[] { new Node(3), new Node(4), new Node(5) });
        b.Children.AddRange(new[] { new Node(6), new Node(7) });
        a.Children[0].Children.Add(new Node(8));
        using var walker = new FairTraversal<Node>(n => n.Id, n => n.Children.Count,
            (n, i) => n.Children[i], _ => { }, (_, _) => { }, maxNodes: 4,
            maxDepth: 2, maxChildren: 2, maxFrontier: 1, maxRoots: 2);
        Assert.True(walker.AddRoot(0, a)); Assert.True(walker.AddRoot(1, b));
        Assert.False(walker.AddRoot(2, new Node(99)));
        Assert.Equal(2, walker.PendingRoots);
        walker.FinishSeeding();
        for (int i = 0; i < 100 && walker.Advance(); i++) Assert.InRange(walker.PendingCursors, 0, 1);
        Assert.Equal(4, walker.NodesVisited);
        Assert.Equal(1, walker.RootsRejected);
        Assert.Equal(1, walker.PeakFrontier);
        Assert.Equal(2, walker.FrontierOmissions);
        Assert.Equal(2, walker.ChildEdgesOmitted); // child cap + known child at depth boundary
        Assert.Equal(1, walker.DepthBoundaryNodes);
        Assert.Equal(2, walker.RootsVisited);
    }

    [Fact]
    public void StaleIdentityVisitChildCountAndChildGetterFailuresConsumeAttemptsAndDoNotAbortSiblings()
    {
        var nodes = Enumerable.Range(1, 6).Select(i => new Node(i)).ToArray();
        nodes[4].Children.Add(new Node(7));
        var visited = new List<int>(); var errors = new List<string>();
        using var walker = new FairTraversal<Node>(
            n => n.Id == 2 ? throw new InvalidOperationException("identity") : n.Id,
            n => n.Id == 4 ? throw new InvalidOperationException("childCount") : n.Children.Count,
            (n, i) => throw new InvalidOperationException("GetChild"),
            n => { if (n.Id == 3) throw new InvalidOperationException("visit"); visited.Add(n.Id); },
            (section, _) => errors.Add(section));
        foreach (var n in nodes) walker.AddRoot(n.Id % 2, n);
        walker.FinishSeeding();
        while (walker.Advance()) { }
        Assert.Contains(6, visited);
        Assert.Equal(7, walker.Attempts);
        Assert.Equal(7, walker.NodesVisited);
        Assert.Equal(4, walker.Errors);
        Assert.Equal(4, errors.Count);
        Assert.Equal(6, walker.RootVisitsByScene.Values.Sum());
    }

    [Fact]
    public void DuplicateIdentitiesDoNotExpandTwiceAndDisposeDropsAllPendingReferences()
    {
        var root = new Node(1); root.Children.Add(root);
        int visits = 0;
        using var walker = new FairTraversal<Node>(n => n.Id, n => n.Children.Count,
            (n, i) => n.Children[i], _ => visits++, (_, _) => { });
        walker.AddRoot(0, root); walker.AddRoot(1, root); walker.FinishSeeding();
        while (walker.Advance()) { }
        Assert.Equal(1, visits); Assert.Equal(2, walker.Duplicates);
        walker.Dispose();
        Assert.Equal(0, walker.PendingRoots); Assert.Equal(0, walker.PendingCursors);
        Assert.False(walker.Advance());
    }

    [Fact]
    public void GiantFirstNpcRootCannotStarveLateCameraWaterOrSecondSceneRoots()
    {
        var npc = new Node(1);
        var tail = npc;
        for (int i = 2; i < 40; i++) { var child = new Node(i); tail.Children.Add(child); tail = child; }
        var visited = new List<int>();
        using var walker = new FairTraversal<Node>(n => n.Id, n => n.Children.Count,
            (n, i) => n.Children[i], n => visited.Add(n.Id), (_, _) => { }, maxNodes: 8);
        walker.AddRoot(0, npc);
        walker.AddRoot(0, new Node(100)); // late camera/water root
        walker.AddRoot(1, new Node(200)); // second loaded scene
        walker.FinishSeeding();
        for (int i = 0; i < 3; i++) walker.Advance();
        Assert.Equal(new[] { 1, 200, 100 }, visited);
    }
}
