using Xunit;

namespace TaxiDriver.Tests;

/// <summary>Pure arbitration between a short-lived local override and the persistent service feedback.</summary>
public class ServiceFeedbackTests
{
    [Fact]
    public void Override_ShowsUntilItExpires_ThenStatusShows()
    {
        var feedback = new ServiceFeedback();
        feedback.ArmOverride("Taxi ordered", 100f, 4f);
        Assert.Equal("Taxi ordered", feedback.Resolve(101f, "", 0, false));
        Assert.Equal("service down", feedback.Resolve(106f, "service down", 0, false));
    }

    [Fact]
    public void NewStatus_ImmediatelySupersedesAnActiveOverride()
    {
        var feedback = new ServiceFeedback();
        feedback.ArmOverride("Taxi ordered", 100f, 4f);
        Assert.Equal("Taxi ordered", feedback.Resolve(101f, "", 0, false));
        Assert.Equal("Taxi Driver is unavailable", feedback.Resolve(102f, "Taxi Driver is unavailable", 7, false));
        Assert.False(feedback.HasOverride);
    }

    [Fact]
    public void SameRevision_DoesNotClearANewlyArmedOverride()
    {
        var feedback = new ServiceFeedback();
        Assert.Equal("failed", feedback.Resolve(100f, "failed", 3, false));
        feedback.ArmOverride("Taxi ordered", 101f, 4f);
        Assert.Equal("Taxi ordered", feedback.Resolve(102f, "failed", 3, false));
    }

    [Fact]
    public void ProtectedStopPrompt_SurvivesANewStatus_ThenStatusShows()
    {
        var feedback = new ServiceFeedback();
        feedback.ArmOverride("press STOP again", 100f, 3f);
        Assert.Equal("press STOP again", feedback.Resolve(101f, "driver down", 9, true));
        Assert.Equal("driver down", feedback.Resolve(104f, "driver down", 9, true));
    }

    [Fact]
    public void EmptyStatus_NeverClearsAnOverride()
    {
        var feedback = new ServiceFeedback();
        feedback.ArmOverride("ok", 100f, 4f);
        Assert.Equal("ok", feedback.Resolve(101f, "", 5, false));
        Assert.True(feedback.HasOverride);
    }

    [Fact]
    public void ClearOverride_DropsTheArmedText()
    {
        var feedback = new ServiceFeedback();
        feedback.ArmOverride("ok", 100f, 4f);
        feedback.ClearOverride();
        Assert.False(feedback.HasOverride);
        Assert.Equal("status", feedback.Resolve(101f, "status", 1, false));
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(-1f)]
    public void InvalidTime_FailsClosedToTheServiceStatus(float now)
    {
        var feedback = new ServiceFeedback();
        feedback.ArmOverride("ok", 100f, 4f);
        Assert.Equal("status", feedback.Resolve(now, "status", 0, false));
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    public void InvalidArmTime_NeverShowsTheOverride(float now)
    {
        var feedback = new ServiceFeedback();
        feedback.ArmOverride("ok", now, 4f);
        Assert.Equal("status", feedback.Resolve(101f, "status", 0, false));
    }
}
