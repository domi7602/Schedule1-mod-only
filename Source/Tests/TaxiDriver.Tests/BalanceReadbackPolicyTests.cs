using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Balance-readback wording: evidence for the log, never proof of causation.
/// Formatting is culture-invariant so the log stays greppable.
/// </summary>
public sealed class BalanceReadbackPolicyTests
{
    [Fact]
    public void MatchingMovement_IsDescribedAsMatching()
    {
        Assert.Equal(
            "net worth 100 -> 59 (observed 41, charged $41) — movement matches the charged amount",
            BalanceReadbackPolicy.Describe(100f, 59f, 41));
    }

    [Fact]
    public void NoMovement_IsDescribedAsMismatch()
    {
        Assert.Equal(
            "net worth 100 -> 100 (observed 0, charged $41) — movement does not match the charged amount (concurrent money changes possible; verify manually)",
            BalanceReadbackPolicy.Describe(100f, 100f, 41));
    }

    [Fact]
    public void PartialMovement_IsDescribedAsMismatch()
    {
        Assert.Contains(
            "movement does not match the charged amount",
            BalanceReadbackPolicy.Describe(100f, 80f, 41));
    }

    [Fact]
    public void FloatNoiseWithinTolerance_StillMatches()
    {
        Assert.Contains(
            "movement matches the charged amount",
            BalanceReadbackPolicy.Describe(100f, 59.3f, 41));
    }
}
