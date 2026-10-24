using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Source contracts for the imported-wheel animation: it is visual-only, driven by
/// native wheel state, bound by position (never by model node names), cached once, and
/// severed on every teardown path.
/// </summary>
public sealed class TaxiWheelWiringTests
{
    [Fact]
    public void TheRunnerTicksTheVisualWheelsEveryFrameAfterTheRideKernel()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        string update = DriverSafetySourceContractTests.Section(
            runner, "internal static void Update()", "private static void ShiftPauseDeadlines()");

        int ride = update.IndexOf("TickRide();", StringComparison.Ordinal);
        int wheels = update.IndexOf("TaxiVisual.TickWheels();", StringComparison.Ordinal);
        Assert.True(ride >= 0 && wheels > ride, "The visual wheel tick must run in the normal Update path.");
        Assert.DoesNotContain("GetComponentsInChildren", update);
    }

    [Fact]
    public void RideEndKeepsWheelRestPoseButShutdownReleasesLinks()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        string reset = DriverSafetySourceContractTests.Section(
            runner, "internal static void ResetDrivingState()", "private const float ReverseClearanceMeters");
        Assert.DoesNotContain("TaxiVisual.ReleaseWheels();", reset);
        string shutdown = DriverSafetySourceContractTests.Section(
            runner, "internal static void Shutdown()", "private static void TickReverseSupervision()");
        Assert.Contains("TaxiVisual.ReleaseWheels();", shutdown);
        Assert.DoesNotContain("Destroy(", reset);
        Assert.DoesNotContain("GetComponents", reset);
    }

    [Fact]
    public void OnlyANewSwapCanArmBindingAndFailuresCannotRebindAnimatedNodes()
    {
        string visual = DriverSafetySourceContractTests.ReadSource("TaxiVisual.cs");
        string deferred = DriverSafetySourceContractTests.Section(
            visual, "private static void TryDeferredWheelBind()", "private static void ScheduleWheelRetry(");
        Assert.Contains("if (!_wheelBindRequested)", deferred);
        string release = DriverSafetySourceContractTests.Section(
            visual, "internal static void ReleaseWheels()", "private static bool IsSteeredWheel");
        Assert.Contains("_wheelBindRequested = false;", release);
        Assert.Contains("_wheelRoot = null;", release);
        string swap = DriverSafetySourceContractTests.Section(
            visual, "private static void SwapInternal(", "private static void RollbackPartialSwap()");
        Assert.Contains("ReleaseWheels();", swap);
        Assert.Contains("_wheelBindRequested = true;", swap);
        string bind = DriverSafetySourceContractTests.Section(
            visual, "private static void BindWheels(", "private static void TryDeferredWheelBind()");
        Assert.Contains("_wheelBindRequested = false;", bind);
        Assert.Contains("Mod.Log.Info($\"[visual] visual wheels bound:", bind);
    }

    [Fact]
    public void ConfirmedVehicleDeathDropsTheWheelLinksRightAfterTheVisualRoot()
    {
        string state = DriverSafetySourceContractTests.ReadSource("SpikeState.cs");
        string reset = DriverSafetySourceContractTests.Section(
            state, "internal static bool Reset()", "CancelPendingSpawn();");

        int root = reset.IndexOf("VisualRoot = null;", StringComparison.Ordinal);
        int wheels = reset.IndexOf("TaxiVisual.ReleaseWheels();", StringComparison.Ordinal);
        Assert.True(root >= 0 && wheels > root, "Wheel links must be severed with the GLB root.");
    }

    [Fact]
    public void ASceneChangeDropsTheWheelLinksBeforeTheSceneIsUnloaded()
    {
        string runner = DriverSafetySourceContractTests.ReadSource("SpikeRunner.cs");
        string sceneChange = DriverSafetySourceContractTests.Section(
            runner, "internal static void OnPreSceneChange()", "internal static void Shutdown()");

        int release = sceneChange.IndexOf("TaxiVisual.ReleaseWheels();", StringComparison.Ordinal);
        int cancel = sceneChange.IndexOf("CancelPendingSpawn();", StringComparison.Ordinal);
        Assert.True(release >= 0 && release < cancel, "The cache must be dropped as the first scene-change action.");
    }

    [Fact]
    public void APartialSwapRollbackSeverstheWheelLinksBeforeTouchingAnythingElse()
    {
        string visual = DriverSafetySourceContractTests.ReadSource("TaxiVisual.cs");
        string rollback = DriverSafetySourceContractTests.Section(
            visual, "private static void RollbackPartialSwap()", "private static void ApplyLayerRecursively");

        int release = rollback.IndexOf("ReleaseWheels();", StringComparison.Ordinal);
        int destroy = rollback.IndexOf("Object.Destroy(rb.Glb)", StringComparison.Ordinal);
        Assert.True(release >= 0 && destroy > release, "The cache must be dropped before the GLB is destroyed.");
    }

    [Fact]
    public void ReleaseWheelsTouchesNoNativeObject()
    {
        string visual = DriverSafetySourceContractTests.ReadSource("TaxiVisual.cs");
        string release = DriverSafetySourceContractTests.Section(
            visual, "internal static void ReleaseWheels()", "private static bool IsSteeredWheel");

        Assert.Contains("_wheelLinks = Array.Empty<WheelLink>();", release);
        Assert.Contains("_wheelVehicle = null;", release);
        Assert.Contains("_wheelBody = null;", release);
        foreach (string forbidden in new[] { "Destroy(", "SetActive(", "enabled =", "localRotation", "Pointer", "try {", "catch" })
            Assert.DoesNotContain(forbidden, release);
    }

    [Fact]
    public void PerFrameWheelWorkWritesOnlyTheImportedNodeRotation()
    {
        string visual = DriverSafetySourceContractTests.ReadSource("TaxiVisual.cs");
        string tick = DriverSafetySourceContractTests.Section(
            visual, "internal static void TickWheels()", "internal static void ReleaseWheels()");

        // Reads only: native wheel state is never written.
        Assert.Contains("GetPointVelocity", tick);
        Assert.Contains("collider.radius", tick);
        Assert.Contains("collider.steerAngle", tick);
        Assert.Contains("node.localRotation =", tick);
        foreach (string forbidden in new[]
        {
            "motorTorque", "brakeTorque", "steerAngle =", "body.velocity =", "angularVelocity",
            "transform.position =", "transform.rotation =", "GetComponentsInChildren", "FindObjectsOfType",
            "Resources.FindObjectsOfTypeAll", "GetComponent<", "SetActive(",
        })
            Assert.DoesNotContain(forbidden, tick);
    }

    [Fact]
    public void WheelNodesAreCollectedByPrefixAndPairedByPositionNotByName()
    {
        string visual = DriverSafetySourceContractTests.ReadSource("TaxiVisual.cs");
        string bind = DriverSafetySourceContractTests.Section(
            visual, "private static void BindWheels(", "private static void TryDeferredWheelBind()");

        Assert.Contains("StartsWith(GlbWheelNodePrefix, StringComparison.OrdinalIgnoreCase)", bind);
        Assert.Contains("TaxiWheelKinematics.FindCounterpart(", bind);
        Assert.Contains("InverseTransformPoint", bind);
        Assert.Contains("vehicleFrame.InverseTransformDirection(node.right)", bind);
        Assert.Contains("vehicleFrame.InverseTransformDirection(node.up)", bind);

        // The asset labels must not decide anything.
        foreach (string label in new[] { "\"Wheel_FL\"", "\"Wheel_FR\"", "\"Wheel_RL\"", "\"Wheel_RR\"", "_Hub" })
            Assert.DoesNotContain(label, bind);
    }

    [Fact]
    public void BindingIsBoundedAndNeverSearchesEveryFrame()
    {
        string visual = DriverSafetySourceContractTests.ReadSource("TaxiVisual.cs");

        Assert.Contains("private const int WheelResolveAttempts = 3;", visual);
        Assert.Contains("private const float WheelResolveRetrySeconds = 1f;", visual);

        string deferred = DriverSafetySourceContractTests.Section(
            visual, "private static void TryDeferredWheelBind()", "private static void ScheduleWheelRetry(");
        Assert.Contains("if (_wheelResolveAttempts >= WheelResolveAttempts)", deferred);
        Assert.Contains("if (now < _wheelRetryAt)", deferred);
        Assert.DoesNotContain("GetComponentsInChildren", deferred);
        Assert.DoesNotContain("new List<", deferred);

        string tick = DriverSafetySourceContractTests.Section(
            visual, "internal static void TickWheels()", "internal static void ReleaseWheels()");
        string early = tick.Substring(0, tick.IndexOf("ReleaseWheels();", StringComparison.Ordinal));
        Assert.Contains("TryDeferredWheelBind();", early);
    }

    [Fact]
    public void ADestroyedLinkOrForeignVehicleFailsSoftInsteadOfThrowing()
    {
        string visual = DriverSafetySourceContractTests.ReadSource("TaxiVisual.cs");
        string tick = DriverSafetySourceContractTests.Section(
            visual, "internal static void TickWheels()", "internal static void ReleaseWheels()");

        Assert.Contains("!ReferenceEquals(veh, SpikeState.Vehicle)", tick);
        Assert.Contains("catch", tick);
        Assert.Contains("ReleaseWheels();", tick);
        Assert.DoesNotContain("Mod.Log.Error", tick);
    }
}
