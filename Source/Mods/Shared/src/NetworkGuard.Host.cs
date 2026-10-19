using System;
using Il2CppFishNet;

namespace S1Mods.Shared;

/// <summary>
/// Host-authority part of <see cref="NetworkGuard"/>. Lives in its own
/// partial file because this is the only Shared component that references
/// Il2CppFishNet (a game type) — signature drift surfaces at compile time,
/// not at runtime.
/// </summary>
public static partial class NetworkGuard
{
    /// <summary>
    /// True if this instance is host/server or running singleplayer (no
    /// NetworkManager). IL2CPP-safe: checks Pointer and WasCollected before
    /// the Unity-Object null comparison.
    /// Fail-closed on exceptions (bug audit 2026-09-12, consolidation 2026-09-15):
    /// the singleplayer path is covered by the NetworkManager-null branch;
    /// if the IsServer marshalling throws on a real MP client, a fail-open
    /// would allow double payouts/desyncs. Refuse rather than risk it.
    /// </summary>
    public static bool IsHostOrSingleplayer()
    {
        try
        {
            var nm = InstanceFinder.NetworkManager;
            if (nm == null || nm.Pointer == IntPtr.Zero || nm.WasCollected || (UnityEngine.Object)nm == null)
                return true;

            return InstanceFinder.IsServer;
        }
        catch
        {
            return false;
        }
    }
}
