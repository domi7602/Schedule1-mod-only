using System.Reflection;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.PlayerScripts;
using S1Mods.Shared;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace PreserveRV.Tests;

public class AuthorityTests
{
    [Theory]
    [InlineData(true, true, true, false, 0, false, true)]
    [InlineData(true, true, true, false, -1, false, true)]
    [InlineData(true, true, true, false, 1, false, true)]
    [InlineData(true, true, true, false, 2, false, false)]
    [InlineData(false, true, true, false, 0, false, false)]
    [InlineData(true, false, true, false, 0, false, false)]
    [InlineData(true, true, false, false, 0, false, false)]
    [InlineData(true, true, true, true, 0, false, false)]
    [InlineData(true, true, true, false, 0, true, false)]
    public void Early_authority_does_not_require_local_player_but_refuses_lobby_peers_and_unknown(bool main, bool host, bool alive, bool lobby, int players, bool unknown, bool expected)
    {
        NetworkGuard.Main = main; NetworkGuard.Host = host; NetworkGuard.Throw = unknown;
        Singleton<Lobby>.Instance = new() { Alive = alive, IsInLobby = lobby };
        Player.PlayerList = players < 0 ? null : Enumerable.Range(0, players).Select(_ => new Player()).ToList();
        var type = typeof(AuthorityTests).Assembly.GetType("PreserveRV.SessionAuthority");
        Assert.NotNull(type);
        var result = type!.GetMethod("Supported", BindingFlags.Static | BindingFlags.NonPublic)!.Invoke(null, null);
        Assert.Equal(expected, result);
        NetworkGuard.Throw = false;
    }
}
