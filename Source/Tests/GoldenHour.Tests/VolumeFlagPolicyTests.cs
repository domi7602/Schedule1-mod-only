using Xunit;

namespace GoldenHour.Tests;

/// <summary>
/// The URP volume component must be active AND the colorFilter override must be set,
/// otherwise writing colorFilter.value has no effect on rendering. 0.1.1 forces both
/// when needed and restores the captured originals on the way out.
/// </summary>
public class VolumeFlagPolicyTests
{
    [Fact]
    public void NeedsEnable_when_component_inactive_true()
        => Assert.True(VolumeFlagPolicy.NeedsEnable(1.0, componentActive: false, colorFilterOverride: true));

    [Fact]
    public void NeedsEnable_when_override_state_false_true()
        => Assert.True(VolumeFlagPolicy.NeedsEnable(1.0, componentActive: true, colorFilterOverride: false));

    [Fact]
    public void NeedsEnable_when_both_already_set_false()
        => Assert.False(VolumeFlagPolicy.NeedsEnable(1.0, componentActive: true, colorFilterOverride: true));

    [Fact]
    public void NeedsEnable_when_strength_is_zero_false()
        => Assert.False(VolumeFlagPolicy.NeedsEnable(0.0, componentActive: false, colorFilterOverride: false));

    [Theory]
    [InlineData(1.0, false, false)]
    [InlineData(0.5, true, false)]
    [InlineData(0.25, false, true)]
    public void NeedsEnable_any_positive_strength_counts(double strength, bool active, bool overrideState)
        => Assert.True(VolumeFlagPolicy.NeedsEnable(strength, active, overrideState));
}
