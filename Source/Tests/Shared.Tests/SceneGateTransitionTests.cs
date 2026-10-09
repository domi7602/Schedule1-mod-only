using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

public class SceneGateTransitionTests
{
    [Fact]
    public void Evaluate_SameActiveScene_DoesNotReportAChange()
    {
        var transition = SceneGateTransition.Evaluate("Main", true, 1, "Main", true, 1, "Main");

        Assert.False(transition.Changed);
        Assert.True(transition.WasMain);
        Assert.True(transition.IsMain);
    }

    [Fact]
    public void Evaluate_AdditiveLoadThatLeavesActiveSceneAlone_DoesNotReportAChange()
    {
        // The newly loaded scene is not active; SceneGate should compare active state only.
        var transition = SceneGateTransition.Evaluate("Main", true, 1, "Main", true, 1, "Main");

        Assert.False(transition.Changed);
    }

    [Fact]
    public void Evaluate_EnteringMain_ReportsMainLoadedTransition()
    {
        var transition = SceneGateTransition.Evaluate("Menu", true, 1, "Main", true, 2, "Main");

        Assert.True(transition.Changed);
        Assert.False(transition.WasMain);
        Assert.True(transition.IsMain);
    }

    [Fact]
    public void Evaluate_LeavingMain_ReportsMainUnloadedTransition()
    {
        var transition = SceneGateTransition.Evaluate("Main", true, 1, "Menu", true, 2, "Main");

        Assert.True(transition.Changed);
        Assert.True(transition.WasMain);
        Assert.False(transition.IsMain);
    }

    [Fact]
    public void Evaluate_LoadStateChangeWithSameName_IsAChange()
    {
        var transition = SceneGateTransition.Evaluate("Main", true, 1, "Main", false, 1, "Main");

        Assert.True(transition.Changed);
        Assert.True(transition.WasMain);
        Assert.False(transition.IsMain);
    }

    [Fact]
    public void Evaluate_SameNameDifferentSceneHandle_IsAChange()
    {
        var transition = SceneGateTransition.Evaluate("Main", true, 1, "Main", true, 2, "Main");

        Assert.True(transition.Changed);
        Assert.True(transition.WasMain);
        Assert.True(transition.IsMain);
    }
}
