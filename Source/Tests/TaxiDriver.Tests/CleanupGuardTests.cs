using System;
using System.Collections.Generic;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Contract tests for the Cleanup package (2026-10-03): cleanup steps run in
/// the caller's order, a throwing step is reported and swallowed, and every
/// step after it still runs.
/// </summary>
public class CleanupGuardTests
{
    [Fact]
    public void AllSteps_RunInOrder_WhenNoneThrow()
    {
        var order = new List<string>();

        bool a = CleanupGuard.Run("a", () => order.Add("a"), Fail);
        bool b = CleanupGuard.Run("b", () => order.Add("b"), Fail);
        bool c = CleanupGuard.Run("c", () => order.Add("c"), Fail);

        Assert.True(a);
        Assert.True(b);
        Assert.True(c);
        Assert.Equal(new[] { "a", "b", "c" }, order);
    }

    [Fact]
    public void ThrowingStep_DoesNotAbortTheLaterSteps()
    {
        var order = new List<string>();
        var failures = new List<string>();

        bool a = CleanupGuard.Run("a", () => order.Add("a"), (label, ex) => failures.Add(label));
        bool b = CleanupGuard.Run("b", () => throw new InvalidOperationException("boom"), (label, ex) => failures.Add(label));
        bool c = CleanupGuard.Run("c", () => order.Add("c"), (label, ex) => failures.Add(label));

        Assert.True(a);
        Assert.False(b);
        Assert.True(c);
        Assert.Equal(new[] { "a", "c" }, order);
        Assert.Equal(new[] { "b" }, failures);
    }

    [Fact]
    public void Failure_ReceivesTheLabelAndTheOriginalException()
    {
        string? seenLabel = null;
        Exception? seenEx = null;

        CleanupGuard.Run(
            "FareMeter.Stop",
            () => throw new TimeoutException("disk slow"),
            (label, ex) =>
            {
                seenLabel = label;
                seenEx = ex;
            });

        Assert.Equal("FareMeter.Stop", seenLabel);
        Assert.IsType<TimeoutException>(seenEx);
        Assert.Equal("disk slow", seenEx!.Message);
    }

    [Fact]
    public void EveryThrowingStep_IsReported_AllStepsAreAttempted()
    {
        var failures = new List<string>();

        for (int i = 0; i < 3; i++)
        {
            int captured = i;
            CleanupGuard.Run($"step{captured}", () => throw new Exception($"e{captured}"), (label, ex) => failures.Add(label));
        }

        Assert.Equal(new[] { "step0", "step1", "step2" }, failures);
    }

    private static void Fail(string label, Exception ex) =>
        throw new Xunit.Sdk.XunitException($"unexpected cleanup failure in '{label}': {ex}");
}
