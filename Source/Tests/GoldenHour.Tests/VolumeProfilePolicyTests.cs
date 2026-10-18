using Xunit;

namespace GoldenHour.Tests;

/// <summary>
/// URP instancing rule for the volume profile. `Volume.profile` creates a runtime clone
/// (`m_InternalProfile`) on first access, and the VolumeManager then blends from the clone
/// instead of the shared asset. If the game itself never instantiated the profile, that
/// clone would silently decouple the game's own per-frame colorFilter writes (day/night
/// grading) from what is rendered. So: only write into the profile when the game already
/// instantiated it — otherwise leave the volume completely alone.
/// </summary>
public class VolumeProfilePolicyTests
{
    [Fact]
    public void UsesProfile_when_game_already_instantiated_it()
        => Assert.True(VolumeProfilePolicy.ShouldUseProfile(gameInstantiatedProfile: true));

    [Fact]
    public void Skips_when_game_did_not_instantiate_it()
        => Assert.False(VolumeProfilePolicy.ShouldUseProfile(gameInstantiatedProfile: false));
}
