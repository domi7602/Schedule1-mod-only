using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

using Il2CppFishNet.Connection;
using Il2CppFishNet.Object;
using Il2CppScheduleOne.Law;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Police;
using Il2CppScheduleOne.Vehicles;
using Il2CppScheduleOne.Vision;

namespace PoliceTestMode;

/// <summary>Explicit, all-required patch group. No generic NPC/vehicle owners or object-return prefixes.</summary>
internal sealed class PoliceHooks
{
    private const string Owner = "Dominik.PoliceTestMode";
    private readonly HarmonyLib.Harmony _harmony = new(Owner);
    private readonly List<MethodBase> _targets = new();
    internal readonly Dictionary<string, long> Blocked = new();
    internal bool Ready { get; private set; }
    internal int Installed => _targets.Count(t => HarmonyLib.Harmony.GetPatchInfo(t)?.Owners.Contains(Owner) == true);

    internal void Install()
    {
        if (Ready) return;
        try
        {
            Add(typeof(LawActivitySettings), "Evaluate");
            Add(typeof(PatrolInstance), "StartPatrol");
            Add(typeof(VehiclePatrolInstance), "StartPatrol");
            Add(typeof(CheckpointInstance), "EnableCheckpoint");
            Add(typeof(SentryInstance), "StartEntry");
            Add(typeof(CurfewInstance), "Enable");
            Add(typeof(CurfewManager), "Enable", typeof(NetworkConnection));
            Add(typeof(CurfewManager), "RpcLogic___Enable_328543758", typeof(NetworkConnection));
            Add(typeof(CurfewManager), "OnUncappedMinPass");
            Add(typeof(CurfewManager), "get_IsEnabled");
            Add(typeof(CurfewManager), "get_IsCurrentlyActive");
            Add(typeof(CurfewManager), "get_IsHardCurfewActive");
            Add(typeof(LawManager), "PoliceCalled", typeof(Player), typeof(Crime));
            Add(typeof(PoliceStation), "Dispatch", typeof(int), typeof(Player), typeof(PoliceStation.EDispatchType), typeof(bool));
            Add(typeof(PoliceOfficer), "Activate");
            Add(typeof(PoliceOfficer), "OnTick");
            Add(typeof(PoliceOfficer), "UpdateVision");
            Add(typeof(PoliceOfficer), "ProcessVisionEvent", typeof(VisionEventReceipt));
            Add(typeof(PoliceOfficer), "BeginFootPursuit", typeof(string));
            Add(typeof(PoliceOfficer), "BeginFootPursuit_Networked", typeof(string), typeof(bool));
            Add(typeof(PoliceOfficer), "RpcLogic___BeginFootPursuit_3615296227", typeof(string));
            Add(typeof(PoliceOfficer), "BeginVehiclePursuit", typeof(string), typeof(NetworkObject), typeof(bool));
            Add(typeof(PoliceOfficer), "BeginVehiclePursuit_Networked", typeof(string), typeof(NetworkObject), typeof(bool));
            Add(typeof(PoliceOfficer), "RpcLogic___BeginVehiclePursuit_1834136777", typeof(string), typeof(NetworkObject), typeof(bool));
            Add(typeof(PoliceOfficer), "BeginBodySearch", typeof(string));
            Add(typeof(PoliceOfficer), "BeginBodySearch_Networked", typeof(string));
            Add(typeof(PoliceOfficer), "ConductBodySearch", typeof(Player));
            Add(typeof(PoliceOfficer), "UpdateBodySearch");
            Add(typeof(PoliceOfficer), "ShouldNoticeGeneralCrime", typeof(Player));
            Add(typeof(PoliceOfficer), "CanInvestigatePlayer", typeof(Player));
            Add(typeof(PoliceOfficer), "CanInvestigate");
            foreach (string name in new[] { "NoticedDrugDeal", "NoticedPettyCrime", "NoticedVandalism", "SawPickpocketing", "PlayerFailedPickpocket", "NoticePlayerBrandishingWeapon", "NoticePlayerDischargingWeapon", "NoticedWantedPlayer", "NoticedSuspiciousPlayer", "NoticedViolatingCurfew", "RespondToAimedAt" })
                Add(typeof(NPCResponses_Police), name, typeof(Player));
            Add(typeof(NPCResponses_Police), "RespondToHitByCar", typeof(LandVehicle));
            Ready = Installed == _targets.Count;
            if (!Ready) throw new InvalidOperationException("Patch owner metadata incomplete.");
            Mod.Log($"hooks ready registered={Installed}/{_targets.Count}; invocation NOT yet verified");
        }
        catch (Exception ex)
        {
            Ready = false;
            Mod.Log("ERROR hooks unavailable: " + ex);
            Remove();
        }
    }

    private void Add(Type owner, string name, params Type[] parameters)
    {
        MethodInfo method = owner.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, null, parameters, null)
            ?? throw new MissingMethodException(owner.FullName, name);
        if (method.ReturnType != typeof(void) && method.ReturnType != typeof(bool))
            throw new InvalidOperationException("Unsupported prefix ABI: " + method);
        _targets.Add(method); // Keep failures visible for rollback verification.
        string prefix = method.ReturnType == typeof(bool) ? nameof(FalsePrefix) : nameof(VoidPrefix);
        if (HarmonyLib.Harmony.GetPatchInfo(method)?.Owners.Contains(Owner) != true)
            _harmony.Patch(method, prefix: new HarmonyMethod(typeof(PoliceHooks), prefix));
        if (HarmonyLib.Harmony.GetPatchInfo(method)?.Owners.Contains(Owner) != true)
            throw new InvalidOperationException("Owner registration absent: " + method);
    }

    internal void Remove()
    {
        Ready = false;
        try
        {
            _harmony.UnpatchSelf();
            Mod.Log($"hooks removed remaining={Installed}");
        }
        catch (Exception ex) { Mod.Log("ERROR removing hooks: " + ex); }
    }

    private static bool VoidPrefix(MethodBase __originalMethod) => !Block(__originalMethod);
    private static bool FalsePrefix(MethodBase __originalMethod, ref bool __result)
    {
        if (!Block(__originalMethod)) return true;
        __result = false;
        return false;
    }

    private static bool Block(MethodBase method)
    {
        var mod = Mod.Instance;
        if (mod == null || !mod.ShouldSuppress()) return false;
        string key = method.DeclaringType!.Name + "." + method.Name;
        mod.Hooks.Blocked.TryGetValue(key, out long count);
        mod.Hooks.Blocked[key] = count + 1;
        if (count == 0) Mod.Log("gate invoked " + key);
        return true;
    }
}
