extern alias il2cpp;

using System;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.Persistence;
using Il2CppScheduleOne.PlayerScripts;
using S1Mods.Shared;
using UnityEngine;

namespace TownHallInterior;

/// <summary>
/// Native engine implementation of <see cref="IBusinessWorldPort"/>, maintaining an isolated,
/// inactive marker hierarchy without mutating native SaveManager.Saveables or unspawned Business lists.
/// </summary>
public sealed class NativeWorldPort : IBusinessWorldPort
{
    private GameObject? _root;
    private bool _isRegistered;
    private bool _isOwned;

    public GameObject? Root => _root;
    public bool IsRegistered => _isRegistered;
    public bool IsOwned => _isOwned;

    public static ConstructionContext? CaptureCurrentContext(bool hallReady, bool allowSaving = false)
    {
        try
        {
            var load = PersistentSingleton<LoadManager>.Instance;
            var save = PersistentSingleton<SaveManager>.Instance;
            var info = load?.ActiveSaveInfo;
            bool singleplayer = IsLocalSingleplayer();

            bool loadReady = S1Mods.Shared.NetworkGuard.IsAlive(load) && load!.IsGameLoaded && !load.IsLoading;

            bool ready = S1Mods.Shared.NetworkGuard.IsInMainScene &&
                loadReady &&
                S1Mods.Shared.NetworkGuard.IsAlive(save) &&
                (allowSaving || !save.IsSaving);

            return new ConstructionContext(
                ready,
                singleplayer,
                hallReady,
                true,
                info?.SavePath,
                load?.LoadedGameFolderPath,
                info?.OrganisationName);
        }
        catch
        {
            return null;
        }
    }

    internal static bool IsLocalSingleplayer()
    {
        var lobby = Singleton<Lobby>.Instance;
        var player = Player.Local;
        return NetworkGuard.IsHostOrSingleplayer() &&
            (!NetworkGuard.IsAlive(lobby) || !lobby.IsInLobby) &&
            Player.PlayerList?.Count == 1 &&
            NetworkGuard.IsAlive(player) && player!.IsOwner && player.PlayerLoaded;
    }

    public WorldRegistrationSnapshot CaptureSnapshot()
    {
        bool reg = _isRegistered;
        bool owned = _isRegistered && _isOwned;
        bool unowned = _isRegistered && !_isOwned;

        // NativeSaveWriteAllowed=false: we never inject an incomplete Business into SaveManager.Saveables.
        // TownHallSaveSession binds the separate JSON to the native checkpoint.
        return new WorldRegistrationSnapshot(
            InBusinessList: reg,
            InUnownedBusinessList: unowned,
            InOwnedBusinessList: owned,
            InPropertyList: reg,
            InUnownedPropertyList: unowned,
            InOwnedPropertyList: owned,
            InSaveablesList: false);
    }

    public bool RegisterInWorld(WorldRegistrationIdentity identity, bool isOwned)
    {
        ArgumentNullException.ThrowIfNull(identity);

        try
        {
            var save = PersistentSingleton<SaveManager>.Instance;
            if (!S1Mods.Shared.NetworkGuard.IsAlive(save) || save.IsSaving)
                return false;

            GameObject? parent = GameObject.Find("@Businesses");
            if (parent == null)
            {
                parent = new GameObject("@Businesses");
            }

            if (_root == null || !S1Mods.Shared.NetworkGuard.IsAlive(_root))
            {
                _root = new GameObject(identity.PropertyName);
                _root.SetActive(false); // Keep strictly inactive to prevent blind native Awake
                _root.transform.SetParent(parent.transform, false);
            }

            _isRegistered = true;
            _isOwned = isOwned;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool DeregisterFromWorld(WorldRegistrationIdentity identity)
    {
        try
        {
            if (_root != null && S1Mods.Shared.NetworkGuard.IsAlive(_root))
            {
                UnityEngine.Object.DestroyImmediate(_root);
                _root = null;
            }

            _isRegistered = false;
            _isOwned = false;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
