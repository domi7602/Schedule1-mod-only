using System;
using System.Collections.Generic;

namespace GraphicsProbe;

// Resumable breadth traversal. All admitted roots precede descendants. A parent
// receives ONE GetChild attempt per turn at its level, including failed attempts.
internal sealed class FairTraversal<T> : IDisposable
{
    private sealed class Cursor
    {
        public T Node;
        public int Depth, Next, Count;
        public Cursor(T node, int depth, int count) { Node = node; Depth = depth; Count = count; }
    }

    private readonly Func<T, int> _identity;
    private readonly Func<T, int> _children;
    private readonly Func<T, int, T> _child;
    private readonly Action<T> _visit;
    private readonly Action<string, Exception> _error;
    private readonly int _maxNodes, _maxDepth, _maxChildren, _maxFrontier, _maxRoots;
    private readonly Dictionary<int, Queue<T>> _roots = new();
    private readonly Queue<int> _scenes = new();
    private Queue<Cursor> _level = new();
    private Queue<Cursor> _nextLevel = new();
    private readonly HashSet<int> _seen = new();
    private bool _seeded, _disposed;
    public int NodesVisited { get; private set; }
    public int Attempts { get; private set; }
    public int RootsAdmitted { get; private set; }
    public Dictionary<int, int> RootVisitsByScene { get; } = new();
    public int RootsVisited { get; private set; }
    public int RootsRejected { get; private set; }
    public int Duplicates { get; private set; }
    public int Errors { get; private set; }
    public int ChildEdgesOmitted { get; private set; }
    public int DepthBoundaryNodes { get; private set; }
    public int FrontierOmissions { get; private set; }
    public int PeakFrontier { get; private set; }
    public int PendingRoots { get; private set; }
    public int PendingCursors => _level.Count + _nextLevel.Count;
    public string? StopReason { get; private set; }

    public FairTraversal(Func<T, int> identity, Func<T, int> children, Func<T, int, T> child,
        Action<T> visit, Action<string, Exception> error, int maxNodes = 50000,
        int maxDepth = 64, int maxChildren = 4096, int maxFrontier = 50000, int maxRoots = 4096)
    {
        _identity = identity; _children = children; _child = child; _visit = visit; _error = error;
        _maxNodes = Math.Max(1, maxNodes); _maxDepth = Math.Max(1, maxDepth);
        _maxChildren = Math.Max(0, maxChildren); _maxFrontier = Math.Max(0, maxFrontier); _maxRoots = Math.Max(0, maxRoots);
    }

    public bool AddRoot(int scene, T node)
    {
        if (_disposed || _seeded) throw new InvalidOperationException("Root admission closed.");
        if (RootsAdmitted >= _maxRoots) { RootsRejected++; return false; }
        if (!_roots.TryGetValue(scene, out var roots)) { roots = new Queue<T>(); _roots.Add(scene, roots); _scenes.Enqueue(scene); }
        roots.Enqueue(node); PendingRoots++; RootsAdmitted++;
        return true;
    }

    public void FinishSeeding() => _seeded = true;

    public bool Advance()
    {
        if (_disposed || StopReason != null) return false;
        if (!_seeded) throw new InvalidOperationException("Seed roots before traversal.");
        if (NodesVisited >= _maxNodes)
        {
            if (PendingRoots > 0 || PendingCursors > 0) StopReason = "node-limit";
            return false;
        }
        if (_scenes.Count > 0)
        {
            int scene = _scenes.Dequeue();
            var roots = _roots[scene];
            var node = roots.Dequeue(); PendingRoots--; RootsVisited++; Attempts++;
            RootVisitsByScene.TryGetValue(scene, out int visits);
            RootVisitsByScene[scene] = visits + 1;
            if (roots.Count > 0) _scenes.Enqueue(scene); else _roots.Remove(scene);
            Visit(node, 0);
            return true;
        }
        if (_level.Count == 0) { (_level, _nextLevel) = (_nextLevel, _level); }
        if (_level.Count == 0) return false;
        var cursor = _level.Dequeue();
        int index = cursor.Next++;
        if (cursor.Next < cursor.Count) _level.Enqueue(cursor);
        Attempts++;
        try { Visit(_child(cursor.Node, index), cursor.Depth + 1); }
        catch (Exception ex) { NodesVisited++; Fail("child", ex); }
        return true;
    }

    private void Visit(T node, int depth)
    {
        NodesVisited++;
        try
        {
            if (!_seen.Add(_identity(node))) { Duplicates++; return; }
            try { _visit(node); } catch (Exception ex) { Fail("node-visit", ex); }
            int count = Math.Max(0, _children(node));
            if (depth + 1 >= _maxDepth) { if (count > 0) { DepthBoundaryNodes++; ChildEdgesOmitted += count; } return; }
            if (count > _maxChildren) { ChildEdgesOmitted += count - _maxChildren; count = _maxChildren; }
            if (count == 0) return;
            if (PendingCursors >= _maxFrontier) { FrontierOmissions += count; return; }
            _nextLevel.Enqueue(new Cursor(node, depth, count));
            PeakFrontier = Math.Max(PeakFrontier, PendingCursors);
        }
        catch (Exception ex) { Fail("hierarchy-node", ex); }
    }

    private void Fail(string section, Exception ex) { Errors++; _error(section, ex); }

    public void Dispose()
    {
        _disposed = true;
        _roots.Clear(); _scenes.Clear(); _level.Clear(); _nextLevel.Clear(); _seen.Clear(); PendingRoots = 0;
    }
}
