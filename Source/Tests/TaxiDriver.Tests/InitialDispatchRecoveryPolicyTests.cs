using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public class InitialDispatchRecoveryPolicyTests
{
    [Fact]
    public void NeverDrove_PrefersRedispatchInsteadOfReverse()
    {
        Assert.True(InitialDispatchRecoveryPolicy.PreferRedispatch(forwardDriveConfirmed: false));
    }

    [Fact]
    public void DroveThenStalled_StillReverses()
    {
        Assert.False(InitialDispatchRecoveryPolicy.PreferRedispatch(forwardDriveConfirmed: true));
    }
}
