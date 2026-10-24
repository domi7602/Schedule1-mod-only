using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Pure wheel-visual math for the imported taxi.glb: node↔native-wheel matching by
/// FINAL position (GLB labels FL/FR/RL/RR and the legacy orientation comment are not
/// trusted), rolling direction from native ground velocity, steering sign, and
/// bounded angle accumulation.
/// </summary>
public sealed class TaxiWheelKinematicsTests
{
    // ---------------------------------------------------------------- slots/matching

    [Theory]
    [InlineData(1f, 1f, 3)]   // +x, +z
    [InlineData(-1f, 1f, 2)]  // -x, +z
    [InlineData(1f, -1f, 1)]  // +x, -z
    [InlineData(-1f, -1f, 0)] // -x, -z
    public void AWheelIsClassifiedByItsQuarter(float x, float z, int expected)
    {
        Assert.Equal(expected, TaxiWheelKinematics.SlotOf(new WheelPoint(x, z), 0.05f));
    }

    [Theory]
    [InlineData(0f, 1f)]
    [InlineData(1f, 0f)]
    [InlineData(0.01f, -1f)]
    [InlineData(float.NaN, 1f)]
    [InlineData(1f, float.NaN)]
    public void AWheelOnTheCentrePlaneOrWithUnreadableCoordinatesHasNoSlot(float x, float z)
    {
        Assert.Equal(-1, TaxiWheelKinematics.SlotOf(new WheelPoint(x, z), 0.05f));
    }

    [Fact]
    public void MatchingUsesTheSameQuarterAndTheNearestNode()
    {
        // Native front-left wheel at (0.9, 1.4); the imported candidates sit in the same
        // frame after the root's 180° rotation, so the label order is deliberately wrong.
        var candidates = new[]
        {
            new WheelPoint(-0.9f, -1.4f), // quarter 0
            new WheelPoint(0.9f, -1.4f),  // quarter 1
            new WheelPoint(-0.9f, 1.4f),  // quarter 2
            new WheelPoint(0.88f, 1.35f), // quarter 3, nearest to the native point
            new WheelPoint(0.95f, 1.45f), // quarter 3, second nearest
        };

        Assert.Equal(
            3,
            TaxiWheelKinematics.FindCounterpart(
                candidates, new WheelPoint(0.9f, 1.4f), TaxiWheelKinematics.MaxMatchDistance));
    }

    [Fact]
    public void ANativeWheelWithNoImportedNodeInItsQuarterStaysUnmatched()
    {
        var candidates = new[]
        {
            new WheelPoint(-0.9f, -1.4f),
            new WheelPoint(0.9f, -1.4f),
            new WheelPoint(-0.9f, 1.4f),
        };

        Assert.Equal(
            -1,
            TaxiWheelKinematics.FindCounterpart(
                candidates, new WheelPoint(0.9f, 1.4f), TaxiWheelKinematics.MaxMatchDistance));
    }

    [Fact]
    public void ANativeWheelTooFarFromItsNearestNodeStaysUnmatched()
    {
        var candidates = new[] { new WheelPoint(0.9f, 1.4f) };

        Assert.Equal(
            -1,
            TaxiWheelKinematics.FindCounterpart(
                candidates, new WheelPoint(0.9f, 3.1f), TaxiWheelKinematics.MaxMatchDistance));
    }

    [Fact]
    public void AMirroredOrRotatedModelStillMapsNativeWheelsToTheNodeAtTheSamePlace()
    {
        // The whole imported root is rotated 180° about Y in TaxiVisual: every node's
        // vehicle-frame position flips sign in x and z. The native front-left wheel must
        // therefore bind to the node that ends up front-left IN THE VEHICLE FRAME,
        // whatever its GLB name says.
        var importedVehicleFrame = new[]
        {
            new WheelPoint(0.86f, -1.4f),  // was GLB "+x +z" before the 180° root flip
            new WheelPoint(-0.86f, -1.4f),
            new WheelPoint(0.86f, 1.4f),
            new WheelPoint(-0.86f, 1.4f),
        };

        int frontLeft = TaxiWheelKinematics.FindCounterpart(
            importedVehicleFrame, new WheelPoint(0.88f, 1.44f), TaxiWheelKinematics.MaxMatchDistance);
        int rearRight = TaxiWheelKinematics.FindCounterpart(
            importedVehicleFrame, new WheelPoint(-0.88f, -1.44f), TaxiWheelKinematics.MaxMatchDistance);

        Assert.Equal(2, frontLeft);
        Assert.Equal(1, rearRight);
    }

    [Fact]
    public void AnEmptyOrUnreadableCandidateListMatchesNothing()
    {
        Assert.Equal(-1, TaxiWheelKinematics.FindCounterpart(
            Array.Empty<WheelPoint>(), new WheelPoint(1f, 1f), 2f));
        Assert.Equal(-1, TaxiWheelKinematics.FindCounterpart(
            new[] { new WheelPoint(float.NaN, float.NaN) }, new WheelPoint(1f, 1f), 2f));
    }

    // ---------------------------------------------------------------- rolling

    [Fact]
    public void ForwardMotionRollsAPositiveAxleWheelForward()
    {
        // v = +Z (vehicle forward), up = +Y, axle = +X, r = 0.5 m at 5 m/s:
        // omega = v / r = 10 rad/s = 572.96 deg/s about the +X axle.
        float degPerSecond = TaxiWheelKinematics.SpinDegreesPerSecond(
            new WheelVector(0f, 0f, 5f),
            WheelVector.Up,
            new WheelVector(1f, 0f, 0f),
            0.5f);

        Assert.Equal(572.9578f, degPerSecond, 2);
    }

    [Fact]
    public void ReverseMotionRollsTheSameWheelBackwards()
    {
        float forward = TaxiWheelKinematics.SpinDegreesPerSecond(
            new WheelVector(0f, 0f, 5f), WheelVector.Up, new WheelVector(1f, 0f, 0f), 0.5f);
        float reverse = TaxiWheelKinematics.SpinDegreesPerSecond(
            new WheelVector(0f, 0f, -5f), WheelVector.Up, new WheelVector(1f, 0f, 0f), 0.5f);

        Assert.True(reverse < 0f);
        Assert.Equal(-forward, reverse, 2);
    }

    [Fact]
    public void AnAxlePointingTheOtherWayFlipsTheRenderedRollDirection()
    {
        // Mirroring flips local axes, so the spin about the imported node's own axle must
        // follow the axle it actually has — not the model file's handedness.
        float positiveAxle = TaxiWheelKinematics.SpinDegreesPerSecond(
            new WheelVector(0f, 0f, 5f), WheelVector.Up, new WheelVector(1f, 0f, 0f), 0.5f);
        float negativeAxle = TaxiWheelKinematics.SpinDegreesPerSecond(
            new WheelVector(0f, 0f, 5f), WheelVector.Up, new WheelVector(-1f, 0f, 0f), 0.5f);

        Assert.Equal(-positiveAxle, negativeAxle, 2);
    }

    [Fact]
    public void SidewaysSlidingDoesNotRollTheWheel()
    {
        Assert.Equal(0f, TaxiWheelKinematics.SpinDegreesPerSecond(
            new WheelVector(4f, 0f, 0f), WheelVector.Up, new WheelVector(1f, 0f, 0f), 0.5f), 3);
    }

    [Fact]
    public void AnUnusableRadiusOrVelocityProducesNoSpin()
    {
        Assert.Equal(0f, TaxiWheelKinematics.SpinDegreesPerSecond(
            new WheelVector(0f, 0f, 5f), WheelVector.Up, new WheelVector(1f, 0f, 0f), 0f));
        Assert.Equal(0f, TaxiWheelKinematics.SpinDegreesPerSecond(
            new WheelVector(0f, 0f, 5f), WheelVector.Up, new WheelVector(1f, 0f, 0f), float.NaN));
        Assert.Equal(0f, TaxiWheelKinematics.SpinDegreesPerSecond(
            new WheelVector(float.NaN, 0f, 0f), WheelVector.Up, new WheelVector(1f, 0f, 0f), 0.5f));
    }

    // ---------------------------------------------------------------- integration

    [Fact]
    public void TheAccumulatedAngleAdvancesByTheRateTimesDeltaTime()
    {
        Assert.Equal(25f, TaxiWheelKinematics.AdvanceSpin(0f, 500f, 0.05f, 180f), 3);
    }

    [Fact]
    public void ASingleFrameStepIsBoundedSoAHitchCannotSpinTheWheelWildly()
    {
        // ±180° is the same pose after wrapping; the bound itself is what matters.
        Assert.Equal(180f, Math.Abs(TaxiWheelKinematics.AdvanceSpin(0f, 50000f, 0.1f, 180f)), 3);
        Assert.Equal(180f, Math.Abs(TaxiWheelKinematics.AdvanceSpin(0f, -50000f, 0.1f, 180f)), 3);
    }

    [Fact]
    public void TheAccumulatedAngleWrapsInsteadOfGrowingForever()
    {
        Assert.Equal(-160f, TaxiWheelKinematics.AdvanceSpin(170f, 300f, 0.1f, 180f), 3);
        Assert.Equal(179f, TaxiWheelKinematics.AdvanceSpin(-181f, 0f, 0.1f, 180f), 3);
    }

    [Theory]
    [InlineData(0f)]
    [InlineData(-1f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    public void ANonPositiveOrUnreadableDeltaTimeHoldsTheAngle(float dt)
    {
        Assert.Equal(42f, TaxiWheelKinematics.AdvanceSpin(42f, 500f, dt, 180f), 3);
    }

    // ---------------------------------------------------------------- steering

    [Fact]
    public void TheNativeSteerAngleIsAppliedInTheNodesOwnUpDirection()
    {
        Assert.Equal(12f, TaxiWheelKinematics.SteerDegrees(12f, 1f), 3);
        Assert.Equal(-12f, TaxiWheelKinematics.SteerDegrees(12f, -1f), 3);
    }

    [Fact]
    public void AnUnreadableSteeringAxisOrAngleSteersNothing()
    {
        Assert.Equal(0f, TaxiWheelKinematics.SteerDegrees(12f, 0f), 3);
        Assert.Equal(0f, TaxiWheelKinematics.SteerDegrees(float.NaN, 1f), 3);
    }

    [Theory]
    [InlineData(0.99f, 1f)]
    [InlineData(-0.99f, -1f)]
    [InlineData(0.2f, 0f)]
    [InlineData(-0.2f, 0f)]
    [InlineData(float.NaN, 0f)]
    public void AnAxisIsOnlyTrustedWhenItReallyPointsAlongTheReference(float dot, float expected)
    {
        Assert.Equal(expected, TaxiWheelKinematics.AxisSign(dot, 0.5f));
    }
}
