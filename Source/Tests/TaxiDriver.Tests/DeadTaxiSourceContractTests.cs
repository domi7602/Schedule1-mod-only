using System;
using Xunit;

namespace TaxiDriver.Tests;

public class DeadTaxiSourceContractTests
{
    [Fact]
    public void ConfirmedDeath_ReleasesRuntimeStateBeforeClearingOwnership()
    {
        string source = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string reconcile = DriverSafetySourceContractTests.Section(source,
            "internal static bool ReconcileDeadTaxi()", "internal static void TickDestroyVerification()");
        Assert.Contains("DeadTaxiCleanup.Run(", reconcile);
        Assert.Contains("FareMeter.Stop(\"vehicle gone\")", reconcile);
        Assert.Contains("RideLocks.UnlockTrunk", reconcile);
        Assert.Contains("TaxiAI.StopPatrol(\"vehicle gone\")", reconcile);
        Assert.Contains("SpikeState.NextNavOrder()", reconcile);
        Assert.Contains("SpikeRunner.ResetDrivingState()", reconcile);
        Assert.DoesNotContain("ownership.ClearAfterConfirmedDeath", reconcile);
        int cleanup = reconcile.IndexOf("DeadTaxiCleanup.Run(", StringComparison.Ordinal);
        int reset = reconcile.IndexOf("SpikeState.Reset()", StringComparison.Ordinal);
        Assert.True(reset > cleanup, "Confirmed-death teardown must precede state/ownership reset.");
        Assert.DoesNotContain("Charge(", reconcile);
        Assert.DoesNotContain("FareMeter.Start(", reconcile);
    }

    [Fact]
    public void Cleanup_DoesNotBypassFailedDeadTaxiTeardownViaAnInvalidHandleReset()
    {
        string source = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string cleanup = DriverSafetySourceContractTests.Section(source,
            "if (initialOwnership == VehicleOwnershipKind.None || initialOwnership == VehicleOwnershipKind.Invalid)",
            "if (initialOwnership == VehicleOwnershipKind.Foreign)");
        int retained = cleanup.IndexOf("if (SpikeState.VehicleOwnership.TrackedPointer != IntPtr.Zero)", StringComparison.Ordinal);
        int reset = cleanup.IndexOf("SpikeState.Reset()", StringComparison.Ordinal);
        Assert.True(retained >= 0 && retained < reset, "Failed confirmed-death cleanup must retain its ownership token rather than fall through to Reset.");
    }

    [Fact]
    public void DeferredDestroyVerification_UsesTheSameDependentTeardown()
    {
        string source = DriverSafetySourceContractTests.ReadSource("SpikeCommands.cs");
        string verification = DriverSafetySourceContractTests.Section(source,
            "internal static void TickDestroyVerification()", "private static bool ClearNpcOccupants");
        Assert.Contains("ReconcileDeadTaxi()", verification);
        Assert.DoesNotContain("ownership.ResolveDestroy(tracked, isAlive: false)", verification);
        Assert.DoesNotContain("SpikeState.Reset()", verification);
    }

    [Fact]
    public void StopMeter_ConsumesActiveFlagWithoutAnAdditionalCharge()
    {
        string meter = DriverSafetySourceContractTests.ReadSource("FareMeter.cs");
        string stop = DriverSafetySourceContractTests.Section(meter,
            "internal static void Stop(string reason)", "internal static void PrintStatus()");
        Assert.True(stop.IndexOf("_active = false", StringComparison.Ordinal) < stop.IndexOf("FareConfig cfg = Config", StringComparison.Ordinal));
        Assert.DoesNotContain("Charge(", stop);
        Assert.DoesNotContain("_paymentStatus.Reset", stop);
    }
}
