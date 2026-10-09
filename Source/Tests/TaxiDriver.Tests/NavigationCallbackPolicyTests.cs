using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public sealed class NavigationCallbackPolicyTests
{
    [Fact]
    public void CurrentOrder_IsAcceptedWhileNavigationIsPolling()
    {
        Assert.True(NavigationCallbackPolicy.ShouldAcceptResult(
            callbackOrder: 7, currentOrder: 7, pollingActive: true));
    }

    [Fact]
    public void OldOrder_IsRejectedAfterStopInvalidatesNavigation()
    {
        Assert.False(NavigationCallbackPolicy.ShouldAcceptResult(
            callbackOrder: 7, currentOrder: 8, pollingActive: true));
    }

    [Fact]
    public void MatchingOrder_IsRejectedAfterCleanupStopsPolling()
    {
        Assert.False(NavigationCallbackPolicy.ShouldAcceptResult(
            callbackOrder: 7, currentOrder: 7, pollingActive: false));
    }
}
