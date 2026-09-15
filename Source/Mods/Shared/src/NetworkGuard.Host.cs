using System;
using Il2CppFishNet;

namespace S1Mods.Shared;

/// <summary>
/// Host-Authority-Teil von <see cref="NetworkGuard"/>. Liegt in einer eigenen
/// Partial-Datei, weil hier als einziger Shared-Bestandteil Il2CppFishNet
/// (Game-Type) referenziert wird — Signature-Drift fällt zur Compile-Zeit auf,
/// nicht zur Laufzeit.
/// </summary>
public static partial class NetworkGuard
{
    /// <summary>
    /// True, wenn diese Instanz Host/Server ist oder Singleplayer läuft (kein
    /// NetworkManager). IL2CPP-safe: prüft Pointer und WasCollected vor dem
    /// Unity-Object-null-Vergleich.
    /// Fail-closed bei Exceptions (Bug-Audit 2026-09-12, Konsolidierung 2026-09-15):
    /// der Singleplayer-Pfad ist über den NetworkManager-null-Zweig abgedeckt;
    /// wirft das IsServer-Marshalling auf einem echten MP-Client, würde ein
    /// fail-open Doppel-Auszahlungen/Desyncs erlauben. Refuse statt riskieren.
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
