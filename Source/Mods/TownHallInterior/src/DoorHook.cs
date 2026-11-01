using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Il2CppScheduleOne.Doors;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Networking;
using Il2CppScheduleOne.PlayerScripts;
using MelonLoader;
using S1Mods.Shared;

namespace TownHallInterior;

// The selector remains deliberately unavailable until its focus lease, cancellation,
// interaction availability and a safe room entry are proven in the installed game.
internal sealed class UnavailableDoorSelector : IDoorSelector
{
    public bool TryShow(Action<DoorAction> chosen) => false;
    public void Close() { }
}

internal static class DoorHook
{
    private const string Owner = "TownHallInterior.DoorHook";
    private const string BuildingGuid = "c747a750-98cb-47fc-9c40-cf5912acc390";
    private static readonly DoorAdapter Adapter = new(new UnavailableDoorSelector());
    private static HarmonyLib.Harmony? _harmony;
    private static MethodInfo? _target;
    private static int _generation;
    private static bool _loggedFailure;

    internal static void Install()
    {
        try
        {
            var method = typeof(StaticDoor).GetMethod("Interacted", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
            var prefix = typeof(DoorHook).GetMethod(nameof(Prefix), BindingFlags.Static | BindingFlags.NonPublic);
            if (method == null || prefix == null || method.ReturnType != typeof(void) || !method.IsVirtual)
                throw new InvalidOperationException("StaticDoor.Interacted() signature unavailable");
            _target = method;
            _harmony = new HarmonyLib.Harmony(Owner);
            if (HarmonyLib.Harmony.GetPatchInfo(method)?.Owners.Contains(Owner) != true)
                _harmony.Patch(method, prefix: new HarmonyMethod(prefix));
            if (HarmonyLib.Harmony.GetPatchInfo(method)?.Owners.Contains(Owner) != true)
                throw new InvalidOperationException("Harmony owner not present after patch");
            MelonLogger.Warning("TownHallInterior: door hook installed; selector and entry unavailable, native interaction retained.");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning("TownHallInterior door hook unavailable; native interaction retained: " + ex);
            RemovePatches();
        }
    }

    internal static void NextGeneration() => Adapter.Reset(++_generation);

    internal static void Uninstall()
    {
        NextGeneration();
        RemovePatches();
    }

    private static void RemovePatches()
    {
        try
        {
            _harmony?.UnpatchSelf();
            if (_target != null && HarmonyLib.Harmony.GetPatchInfo(_target)?.Owners.Contains(Owner) == true)
            {
                MelonLogger.Warning("TownHallInterior: owner still present after unpatch.");
                return;
            }
            _harmony = null;
            _target = null;
        }
        catch (Exception ex) { MelonLogger.Warning("TownHallInterior unpatch failed: " + ex); }
    }

    private static bool Prefix(StaticDoor __instance)
    {
        try
        {
            if (!IsTarget(__instance)) return true;
            int generation = _generation;
            return Adapter.Prefix(true, IsSingleplayer(), generation,
                // No selector can currently succeed, and room publication is not proven.
                () => throw new InvalidOperationException("Room entry is unavailable"),
                () => __instance.Interacted(),
                () => generation == _generation && IsTarget(__instance) && IsSingleplayer(), __instance.Pointer,
                // The staging build reports the selector as unready so the adapter
                // passes native before any choice decision is made.
                selectorReady: false);
        }
        catch (Exception ex)
        {
            Adapter.Reset(_generation);
            if (!_loggedFailure)
            {
                _loggedFailure = true;
                MelonLogger.Warning("TownHallInterior door inspection failed; passing native: " + ex);
            }
            return true;
        }
    }

    private static bool IsTarget(StaticDoor door)
    {
        if (!NetworkGuard.IsInMainScene || !NetworkGuard.IsAlive(door)) return false;
        var building = door.Building;
        var interaction = door.IntObj;
        if (!NetworkGuard.IsAlive(building) || !NetworkGuard.IsAlive(interaction)) return false;
        if (!string.Equals(building.BakedGUID, BuildingGuid, StringComparison.OrdinalIgnoreCase)) return false;
        var t = door.transform;
        var it = interaction.transform;
        return t != null && t.name == "StaticDoor" && t.parent != null && t.parent.name == "TownHall"
            && it != null && it.name == "IntObj" && it.parent != null && it.parent.Pointer == t.Pointer;
    }

    private static bool IsSingleplayer()
    {
        try
        {
            if (!NetworkGuard.IsHostOrSingleplayer()) return false;
            var lobby = Singleton<Lobby>.Instance;
            if (!NetworkGuard.IsAlive(lobby) || lobby.IsInLobby) return false;
            return Player.PlayerList != null && Player.PlayerList.Count == 1;
        }
        catch { return false; }
    }
}
