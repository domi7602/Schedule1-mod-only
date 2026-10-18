using System;

namespace GraphicsProbe;

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
