// ---------------------------------------------------------------------------
// Probe/ProbeCapturePolicies.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract checks and an independent
// reconstruction proof can still read that file's exact content.
//
//   Probe/CaptureSchedule.cs           -> section CaptureSchedule.cs
//   Probe/CooperativeCapture.cs        -> section CooperativeCapture.cs
//   Probe/FairTraversal.cs             -> section FairTraversal.cs
//   Probe/RendererScanBudget.cs        -> section RendererScanBudget.cs
// ---------------------------------------------------------------------------

namespace GraphicsProbe
{
    // ==== WorldLook section: CaptureSchedule.cs ====
    using System;
    using System.Collections.Generic;


    // Main-thread only. No retained Unity objects, coroutines or background callbacks.
    internal sealed class CaptureSchedule
    {
        private readonly double _delay;
        private readonly double _cooldown;
        private readonly List<string> _reasons = new();
        private double _due;
        private double _notBefore;
        private bool _running;
        private bool _closed;

        public CaptureSchedule(double delay, double cooldown)
        {
            _delay = Math.Max(0, delay);
            _cooldown = Math.Max(0, cooldown);
        }

        public void Request(double now, string reason)
        {
            if (_closed) return;
            if (_reasons.Count == 0) _due = now + _delay;
            // Callers supply fixed reason labels, not unbounded scene/object names.
            if (!_reasons.Contains(reason) && _reasons.Count < 3) _reasons.Add(reason);
        }

        public bool TryBegin(double now, out string reason)
        {
            reason = "";
            if (_closed || _running || _reasons.Count == 0 || now < Math.Max(_due, _notBefore)) return false;
            reason = string.Join(",", _reasons);
            _reasons.Clear();
            _running = true;
            return true;
        }

        public void Complete(double now)
        {
            if (!_running) return;
            _running = false;
            _notBefore = now + _cooldown;
        }

        public void Shutdown()
        {
            _closed = true;
            _running = false;
            _reasons.Clear();
        }
    }
    // ==== end WorldLook section: CaptureSchedule.cs ====
}

namespace GraphicsProbe
{
    // ==== WorldLook section: CooperativeCapture.cs ====
    using System;


    internal interface ISteppedCapture : IDisposable
    {
        bool Advance();
    }

    // Pure, runtime-used slice controller. Clock units are milliseconds. Native work is
    // cooperative: one call may exceed a slice; no subsequent call starts in that slice.
    internal sealed class CooperativeCapture : IDisposable
    {
        private ISteppedCapture? _work;
        private readonly Func<double> _clock;
        private readonly double _start, _slice, _activeBudget, _wallDeadline;
        private readonly int _maxSteps;
        private double _end;
        public string Status { get; private set; } = "running";
        public Exception? Failure { get; private set; }
        public double ActiveMilliseconds { get; private set; }
        public double WallMilliseconds => (Status == "running" ? _clock() : _end) - _start;
        public double MaxObservedStepMilliseconds { get; private set; }
        public int Steps { get; private set; }
        public int Frames { get; private set; }

        public CooperativeCapture(ISteppedCapture work, Func<double> clock, int maxSteps = 128,
            double slice = 2, double activeBudget = 1500, double wallDeadline = 30000)
        {
            _work = work; _clock = clock; _start = clock();
            _maxSteps = Math.Max(1, maxSteps); _slice = Math.Max(0.001, slice);
            _activeBudget = Math.Max(0, activeBudget); _wallDeadline = Math.Max(0, wallDeadline);
        }

        public void StepFrame()
        {
            if (Status != "running") return;
            Frames++;
            double frameStart = _clock();
            for (int i = 0; i < _maxSteps && Status == "running"; i++)
            {
                if (WallMilliseconds >= _wallDeadline) { Finish("wall-deadline"); break; }
                if (ActiveMilliseconds >= _activeBudget) { Finish("active-budget"); break; }
                if (_clock() - frameStart >= _slice) break;
                double start = _clock();
                bool more = false;
                try { more = _work!.Advance(); }
                catch (Exception ex) { Failure = ex; }
                finally
                {
                    double elapsed = Math.Max(0, _clock() - start);
                    ActiveMilliseconds += elapsed;
                    MaxObservedStepMilliseconds = Math.Max(MaxObservedStepMilliseconds, elapsed);
                    Steps++;
                }
                if (Failure != null) Finish("failed");
                else if (!more) Finish("completed");
                else if (ActiveMilliseconds >= _activeBudget) Finish("active-budget");
                else if (WallMilliseconds >= _wallDeadline) Finish("wall-deadline");
            }
        }

        private void Finish(string status)
        {
            if (Status != "running") return;
            Status = status;
            var work = _work; _work = null;
            try { work?.Dispose(); }
            catch (Exception ex) { Failure = ex; Status = "failed"; }
            finally { _end = _clock(); }
        }

        public void Cancel() => Finish("cancelled");
        public void Dispose() => Cancel();
    }
    // ==== end WorldLook section: CooperativeCapture.cs ====
}

namespace GraphicsProbe
{
    // ==== WorldLook section: FairTraversal.cs ====
    using System;
    using System.Collections.Generic;


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
    // ==== end WorldLook section: FairTraversal.cs ====
}

namespace GraphicsProbe
{
    // ==== WorldLook section: RendererScanBudget.cs ====
    using System;
    using System.Collections.Generic;


    internal sealed class RendererScanBudget
    {
        private readonly int _surfaceLimit, _effectLimit;
        public int SurfaceScanned { get; private set; }
        public int EffectsScanned { get; private set; }
        public int SurfaceScanOmissions { get; private set; }
        public int EffectScanOmissions { get; private set; }
        public Dictionary<string, int> CountsByNativeType { get; } = new();

        public RendererScanBudget(int surfaceLimit, int effectLimit)
        { _surfaceLimit = Math.Max(0, surfaceLimit); _effectLimit = Math.Max(0, effectLimit); }

        public static bool IsEffect(string type) => type == "UnityEngine.ParticleSystemRenderer"
            || type == "UnityEngine.LineRenderer" || type == "UnityEngine.TrailRenderer";

        public bool TryScan(string type, out bool effect)
        {
            effect = IsEffect(type);
            if (!CountsByNativeType.ContainsKey(type) && CountsByNativeType.Count >= 32) type = "<other-native-types: key cap>";
            CountsByNativeType.TryGetValue(type, out int count);
            CountsByNativeType[type] = count + 1;
            if (effect)
            {
                if (EffectsScanned >= _effectLimit) { EffectScanOmissions++; return false; }
                EffectsScanned++;
            }
            else
            {
                if (SurfaceScanned >= _surfaceLimit) { SurfaceScanOmissions++; return false; }
                SurfaceScanned++;
            }
            return true;
        }
    }
    // ==== end WorldLook section: RendererScanBudget.cs ====
}
