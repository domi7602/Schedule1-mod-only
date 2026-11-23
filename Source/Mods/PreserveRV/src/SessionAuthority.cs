using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.PlayerScripts;
using S1Mods.Shared;

namespace PreserveRV;

internal static class SessionAuthority
{
    // Restoration callbacks can precede LoadComplete and the creation of Player.Local.
    internal static bool Supported()
    {
        try
        {
            if (!NetworkGuard.IsInMainScene || !NetworkGuard.IsHostOrSingleplayer()) return false;
            var lobby = Singleton<Lobby>.Instance;
            if (!NetworkGuard.IsAlive(lobby) || lobby!.IsInLobby) return false;
            var players = Player.PlayerList;
            return players == null || players.Count <= 1;
        }
        catch { return false; } // Unknown authority never authorizes a world mutation.
    }
}
