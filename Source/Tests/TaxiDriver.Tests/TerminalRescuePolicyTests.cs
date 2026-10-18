using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Terminal-rescue decision: the 2026-10-15 run gave up 106.7 m from the target
/// while the taxi was ON the vehicle graph ("no teleport needed, giving up"), so
/// graph membership must never skip the guarded relocation.
/// </summary>
public sealed class TerminalRescuePolicyTests
{
    [Fact]
    public void OnGraphWithRescueAvailable_StillAttemptsTheGuardedRelocation()
    {
        Assert.Equal(
            TerminalRescuePolicy.Action.AttemptGuardedRelocation,
            TerminalRescuePolicy.Decide(onGraph: true, lastRescueAvailable: true));
    }

    [Fact]
    public void OffGraphWithRescueAvailable_AttemptsTheGuardedRelocation()
    {
        Assert.Equal(
            TerminalRescuePolicy.Action.AttemptGuardedRelocation,
            TerminalRescuePolicy.Decide(onGraph: false, lastRescueAvailable: true));
    }

    [Fact]
    public void SpentRescueBudget_GivesUpRegardlessOfGraphState()
    {
        Assert.Equal(
            TerminalRescuePolicy.Action.GiveUp,
            TerminalRescuePolicy.Decide(onGraph: true, lastRescueAvailable: false));
        Assert.Equal(
            TerminalRescuePolicy.Action.GiveUp,
            TerminalRescuePolicy.Decide(onGraph: false, lastRescueAvailable: false));
    }
}
