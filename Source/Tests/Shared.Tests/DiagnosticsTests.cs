using System;
using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Tests for S1Mods.Shared.Diagnostics — the fail-visible helper that keeps a
/// per-frame exception from flooding MelonLoader/Latest.log and reports at startup
/// which patches did not apply. Pure logic, no Unity/Harmony runtime needed.
/// </summary>
public class DiagnosticsTests
{
    public DiagnosticsTests()
    {
        Diagnostics.Reset();
        PatchGuard.ResetStats();
    }

    [Fact]
    public void ErrorOnce_FirstFailureLogs_RepeatsAreCounted()
    {
        Exception ex = new InvalidOperationException("boom");

        Diagnostics.ErrorOnce("Hook.Tick", ex);

        Assert.Equal(0, Diagnostics.SuppressedCount("Hook.Tick"));

        for (int i = 0; i < 4; i++) Diagnostics.ErrorOnce("Hook.Tick", ex);

        // 5 hits total: 1 logged, 4 suppressed.
        Assert.Equal(4, Diagnostics.SuppressedCount("Hook.Tick"));
        Assert.Equal(4, Diagnostics.SuppressedTotal);
    }

    [Fact]
    public void ErrorOnce_KeysAreIndependent()
    {
        Exception ex = new InvalidOperationException("boom");

        Diagnostics.ErrorOnce("A", ex);
        Diagnostics.ErrorOnce("A", ex);
        Diagnostics.ErrorOnce("B", ex);

        Assert.Equal(1, Diagnostics.SuppressedCount("A"));
        Assert.Equal(0, Diagnostics.SuppressedCount("B"));
        Assert.Equal(1, Diagnostics.SuppressedTotal);
    }

    [Fact]
    public void ErrorOnce_NullOrEmptyKey_FallsBackToUnknown()
    {
        Diagnostics.ErrorOnce(null!, new Exception("x"));
        Diagnostics.ErrorOnce("", new Exception("x"));

        // Both calls share the "unknown" bucket -> second one is suppressed.
        Assert.Equal(1, Diagnostics.SuppressedCount("unknown"));
    }

    [Fact]
    public void SuppressedCount_UnknownKey_IsZero()
    {
        Assert.Equal(0, Diagnostics.SuppressedCount("never.seen"));
        Assert.Equal(0, Diagnostics.SuppressedCount(""));
    }

    [Fact]
    public void WarnOnce_DoesNotCollideWithErrors()
    {
        Diagnostics.ErrorOnce("Same.Key", new Exception("x"));
        Diagnostics.WarnOnce("Same.Key", "warned");

        // Separate buckets: the warning is its own first hit, the error keeps its count.
        Assert.Equal(0, Diagnostics.SuppressedCount("Same.Key"));
    }

    [Fact]
    public void Reset_ClearsCounters()
    {
        Diagnostics.ErrorOnce("A", new Exception("x"));
        Diagnostics.ErrorOnce("A", new Exception("x"));
        Assert.Equal(1, Diagnostics.SuppressedTotal);

        Diagnostics.Reset();

        Assert.Equal(0, Diagnostics.SuppressedTotal);
        Assert.Equal(0, Diagnostics.SuppressedCount("A"));
    }

    [Fact]
    public void FlushSummary_DoesNotThrow_WithAndWithoutLogger()
    {
        Diagnostics.ErrorOnce("A", new Exception("x"));
        Diagnostics.ErrorOnce("A", new Exception("x"));

        // No logger: must fall through to MelonLogger without throwing.
        Diagnostics.FlushSummary();
    }

    [Fact]
    public void FailedTargets_StartsEmpty_AndSurvivesReset()
    {
        PatchGuard.ResetStats();
        Assert.Empty(PatchGuard.FailedTargets);

        PatchGuard.ResetStats();
        Assert.Empty(PatchGuard.FailedTargets);
    }

    [Fact]
    public void StartupReport_DoesNotThrow_WithoutLogger()
    {
        Diagnostics.StartupReport("TestMod", "1.2.3");
        Diagnostics.StartupReport("TestMod", "1.2.3", extraLines: new[] { "designs 3", "" });
        Diagnostics.StartupReport("", "");
    }
}
