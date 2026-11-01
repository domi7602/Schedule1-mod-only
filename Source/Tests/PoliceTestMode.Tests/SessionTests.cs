using System.Reflection;
using Xunit;
namespace PoliceTestMode.Tests;
public class SessionTests
{
    [Theory]
    [InlineData(true, true, true, true, true)]
    [InlineData(false, true, true, true, false)]
    [InlineData(true, false, true, true, false)]
    [InlineData(true, true, false, true, false)]
    [InlineData(true, true, true, false, false)]
    public void Suppression_requires_request_hooks_world_and_singleplayer(bool request, bool hooks, bool world, bool singleplayer, bool expected)
    {
        var type = typeof(SessionTests).Assembly.GetType("PoliceTestMode.SessionPolicy");
        Assert.NotNull(type);
        var method = type.GetMethod("ShouldSuppress", BindingFlags.Public | BindingFlags.Static);
        Assert.NotNull(method);
        Assert.Equal(expected, method.Invoke(null, new object[] { request, hooks, world, singleplayer }));
    }
}
