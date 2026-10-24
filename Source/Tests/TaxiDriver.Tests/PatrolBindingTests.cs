using Xunit;

namespace TaxiDriver.Tests;

public class PatrolBindingTests
{
    [Theory]
    [InlineData(true, true, true, true, true, true)]
    [InlineData(false, true, true, true, true, false)]
    [InlineData(true, false, true, true, true, false)]
    [InlineData(true, true, false, true, true, false)]
    [InlineData(true, true, true, false, true, false)]
    [InlineData(true, true, true, true, false, false)]
    public void BindingRequiresExactOwnerNetworkRegistrationAndIdleState(bool owner, bool network,
        bool registered, bool serverReady, bool idle, bool expected)
        => Assert.Equal(expected, PatrolBindingPolicy.CanUse(owner, network, registered, serverReady, idle));

    [Fact]
    public void PatrolUsesRegisteredDriverControllerWithoutRuntimeNetworkComponents()
    {
        string ai = DriverSafetySourceContractTests.ReadSource("TaxiAI.cs");
        Assert.Contains("driver.Behaviour", ai);
        Assert.DoesNotContain("AddComponent<NPCBehaviour>", ai);
        Assert.DoesNotContain("AddComponent<VehiclePatrolBehaviour>", ai);
        Assert.Contains("_networkObjectCache", ai);
        Assert.Contains("NetworkBehaviours", ai);
        Assert.Contains("BehaviourIndex", ai);
        Assert.DoesNotContain("Patrol.beh =", ai);
    }

    [Fact]
    public void ExistingPatrolIsLeasedAndNeverDestroyed()
    {
        string ai = DriverSafetySourceContractTests.ReadSource("TaxiAI.cs");
        Assert.Contains("OriginalRoute = patrol.Route", ai);
        Assert.Contains("OriginalVehicle = patrol.Vehicle", ai);
        Assert.Contains("OriginalEnabled = patrol.enabled", ai);
        Assert.Contains("OriginalAggressive = patrol.aggressiveDrivingEnabled", ai);
        Assert.DoesNotContain("Object.Destroy(Patrol)", ai);
        Assert.Contains("patrol.Route = lease.OriginalRoute", ai);
        Assert.Contains("patrol.Vehicle = lease.OriginalVehicle", ai);
        Assert.Contains("patrol.enabled = lease.OriginalEnabled", ai);
        Assert.Contains("patrol.aggressiveDrivingEnabled = lease.OriginalAggressive", ai);
    }

    [Fact]
    public void FailedStartAndRepeatedStopCannotReleaseAnotherLease()
    {
        string ai = DriverSafetySourceContractTests.ReadSource("TaxiAI.cs");
        string stop = ai[ai.IndexOf("internal static void StopPatrol(", System.StringComparison.Ordinal)..];
        Assert.Contains("lease.Release.TryBegin()", stop);
        Assert.True(stop.IndexOf("ReleaseLease(lease)", System.StringComparison.Ordinal) <
            stop.IndexOf("Lease = null;", System.StringComparison.Ordinal));
        Assert.Contains("if (ReferenceEquals(Lease, lease)) StopPatrol(\"start failed\");", ai);
        Assert.Contains("ReferenceEquals(Lease, lease)", ai);
        Assert.Contains("BindingValid(lease.Driver, lease.Manager, patrol)", ai);
        Assert.Contains("if (Lease != null) return false;", ai);
        string caller = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        Assert.Contains("TaxiAI.NavigationBlocked", caller);
    }

    [Fact]
    public void IncompleteReleaseRetainsOwnershipAndPermitsOnlyAConfirmedRetry()
    {
        var gate = new PatrolReleaseGate();
        Assert.True(gate.TryBegin());
        Assert.True(gate.Requested);
        Assert.False(gate.TryBegin());
        gate.End(false);
        Assert.False(gate.Completed);
        Assert.True(gate.Requested);
        Assert.False(gate.Busy);
        Assert.True(gate.TryBegin());
        gate.End(true);
        Assert.True(gate.Completed);
        Assert.False(gate.TryBegin());
    }

    [Fact]
    public void UnclaimedCompletionCannotReleaseTheOwner()
    {
        var gate = new PatrolReleaseGate();
        gate.End(true);
        Assert.False(gate.Completed);
        Assert.False(gate.Requested);
    }

    [Fact]
    public void PendingReleaseBlocksEveryRecoveryActuationBoundary()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        foreach (string signature in new[] {
            "private static bool TryFreeSpotRescue(LandVehicle veh, VehicleAgent agent, string context)",
            "private static bool TryStartReverse(VehicleAgent agent, LandVehicle veh, float now, string reason)",
            "private static bool TryTeleportRecovery(LandVehicle veh, VehicleAgent agent, Vector3 position, float distance)" })
        {
            int start = runner.IndexOf(signature, System.StringComparison.Ordinal);
            Assert.True(start >= 0);
            Assert.Contains("if (TaxiAI.NavigationBlocked) return false;", runner.Substring(start, signature.Length + 85));
        }
    }

    [Fact]
    public void PendingReleaseCannotBypassTheFenceThroughPollingRetryOrSuspendTheDeadline()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        string polling = DriverSafetySourceContractTests.Section(runner,
            "private static void TickNavigationPolling()", "private static void TickArrivalConfirmation(");
        int fence = polling.IndexOf("if (TaxiAI.NavigationBlocked)", System.StringComparison.Ordinal);
        int deadline = polling.IndexOf("StopIfNavigationExpired(", System.StringComparison.Ordinal);
        int retry = polling.IndexOf("agent.Navigate(", System.StringComparison.Ordinal);
        Assert.True(deadline >= 0 && fence > deadline && retry > fence);
    }
}
