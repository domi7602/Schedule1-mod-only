using System;
using System.Reflection;
using Xunit;

namespace TaxiDriver.Tests;

public class TrafficLightPolicyTests
{
    [Fact]
    public void OnlyHostExactOwnedActiveAutonomousNonPlayerTaxiIgnoresLights()
    {
        Assert.True(TrafficLightPolicy.ShouldIgnore(true, true, true, true, false));
        for (int i = 0; i < 5; i++)
        {
            bool[] state = { true, true, true, true, false };
            state[i] = !state[i];
            Assert.False(TrafficLightPolicy.ShouldIgnore(state[0], state[1], state[2], state[3], state[4]));
        }
    }
}
