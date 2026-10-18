using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Actual final throttle requests only; native gear remains a native output.</summary>
public class ReversePedalPolicyTests
{
    [Fact]
    public void SupervisedReverse_RequestsNegativePointSeven()
        => Assert.Equal(-0.7f, ReversePedalPolicy.CurrentThrottleFor(true));

    [Fact]
    public void Park_RequestsZeroPropulsion()
        => Assert.Equal(0f, ReversePedalPolicy.CurrentThrottleFor(false));
}
