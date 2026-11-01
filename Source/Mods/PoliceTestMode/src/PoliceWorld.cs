using System;
using System.Collections.Generic;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.Law;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.Police;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Vehicles;
using S1Mods.Shared;
using UnityEngine;

namespace PoliceTestMode;

/// <summary>Temporary Unity activeSelf leases, never NPC.SetActive, death, despawn, save, or identity changes.</summary>
internal sealed class PoliceWorld
{
    internal const int MaxEntries = 512;
    private readonly Dictionary<int, (GameObject Target, BoolLease Lease)> _leases = new();
    private readonly List<BoolLease> _curfewLeases = new();
    private CurfewManager? _curfew;
    internal int Count => _leases.Count + _curfewLeases.Count;
    internal int Officers { get; private set; }
    internal int Vehicles { get; private set; }
    internal int OccupiedVehicles { get; private set; }
    internal int UnknownVehicles { get; private set; }
    internal int Checkpoints { get; private set; }

    internal bool Ready()
        => PoliceOfficer.Officers != null && PoliceOfficer.Officers.Count > 0
        && PoliceStation.PoliceStations != null && PoliceStation.PoliceStations.Count > 0
        && NetworkGuard.IsAlive(NetworkSingleton<CheckpointManager>.Instance)
        && NetworkGuard.IsAlive(NetworkSingleton<CurfewManager>.Instance);

    internal void Apply()
    {
        var curfew = NetworkSingleton<CurfewManager>.Instance;
        if (!NetworkGuard.IsAlive(curfew)) throw new InvalidOperationException("Curfew manager lost.");
        if (_curfewLeases.Count == 0)
        {
            _curfew = curfew;
            _curfewLeases.Add(new BoolLease(() => NetworkGuard.IsAlive(curfew), () => curfew._IsEnabled_k__BackingField, v => curfew._IsEnabled_k__BackingField = v, false));
            _curfewLeases.Add(new BoolLease(() => NetworkGuard.IsAlive(curfew), () => curfew._IsCurrentlyActive_k__BackingField, v => curfew._IsCurrentlyActive_k__BackingField = v, false));
            _curfewLeases.Add(new BoolLease(() => NetworkGuard.IsAlive(curfew), () => curfew._IsHardCurfewActive_k__BackingField, v => curfew._IsHardCurfewActive_k__BackingField = v, false));
        }
        if (_curfew?.Pointer != curfew.Pointer) throw new InvalidOperationException("Curfew manager replaced without lifecycle release.");
        foreach (var lease in _curfewLeases) lease.Apply();
        var targets = new List<GameObject>();
        Officers = Vehicles = OccupiedVehicles = UnknownVehicles = Checkpoints = 0;
        // Copy registries before SetActive: native OnDisable callbacks may change lists.
        var officers = PoliceOfficer.Officers;
        CheckBound(officers.Count);
        for (int i = 0; i < officers.Count; i++)
        {
            var officer = officers[i];
            if (!NetworkGuard.IsAlive(officer)) continue;
            targets.Add(officer.gameObject);
            Officers++;
        }
        var stations = PoliceStation.PoliceStations;
        CheckBound(stations.Count);
        for (int i = 0; i < stations.Count; i++)
        {
            var station = stations[i];
            if (!NetworkGuard.IsAlive(station)) continue;
            var vehicles = station.deployedVehicles;
            if (vehicles == null) continue;
            CheckBound(vehicles.Count);
            for (int j = 0; j < vehicles.Count; j++)
            {
                var vehicle = vehicles[j];
                if (!NetworkGuard.IsAlive(vehicle)) continue;
                // Vehicle-wide IsOccupied includes NPCs; only player seats exempt patrol cars.
                var occupancy = ReadPlayerOccupancy(vehicle);
                if (occupancy != VehicleOccupancy.Clear)
                {
                    if (occupancy == VehicleOccupancy.Player) OccupiedVehicles++;
                    else UnknownVehicles++;
                    int occupiedId = vehicle.gameObject.GetInstanceID();
                    if (_leases.TryGetValue(occupiedId, out var existing))
                    {
                        if (!existing.Lease.Restore()) throw new InvalidOperationException("Occupied vehicle restore failed.");
                        _leases.Remove(occupiedId);
                    }
                    continue;
                }
                targets.Add(vehicle.gameObject);
                Vehicles++;
            }
        }
        var manager = NetworkSingleton<CheckpointManager>.Instance;
        if (NetworkGuard.IsAlive(manager))
        {
            foreach (var checkpoint in new[] { manager.WesternCheckpoint, manager.DocksCheckpoint, manager.NorthResidentialCheckpoint, manager.WestResidentialCheckpoint })
            {
                if (!NetworkGuard.IsAlive(checkpoint)) continue;
                targets.Add(checkpoint.gameObject);
                Checkpoints++;
            }
        }
        CheckBound(targets.Count);
        foreach (var target in targets)
        {
            int id = target.GetInstanceID();
            if (!_leases.ContainsKey(id))
            {
                CheckBound(_leases.Count + 1);
                _leases.Add(id, (target, new BoolLease(() => NetworkGuard.IsAlive(target), () => target.activeSelf, target.SetActive, false)));
            }
        }
        foreach (var entry in new List<KeyValuePair<int, (GameObject Target, BoolLease Lease)>>(_leases))
        {
            if (!NetworkGuard.IsAlive(entry.Value.Target)) { _leases.Remove(entry.Key); continue; }
            entry.Value.Lease.Apply();
        }
    }

    internal bool Restore()
    {
        foreach (var entry in new List<KeyValuePair<int, (GameObject Target, BoolLease Lease)>>(_leases))
        {
            try
            {
                if (entry.Value.Lease.Restore()) _leases.Remove(entry.Key);
                else Mod.Log("ERROR restore readback mismatch id=" + entry.Key);
            }
            catch (Exception ex) { Mod.Log($"ERROR restore id={entry.Key}: {ex.Message}"); }
        }
        foreach (var lease in new List<BoolLease>(_curfewLeases))
        {
            try { if (lease.Restore()) _curfewLeases.Remove(lease); }
            catch (Exception ex) { Mod.Log("ERROR curfew restore: " + ex.Message); }
        }
        if (_curfewLeases.Count == 0) _curfew = null;
        return Count == 0;
    }

    internal string Readback()
    {
        try
        {
            int active = 0;
            foreach (var entry in _leases.Values)
                if (NetworkGuard.IsAlive(entry.Target) && entry.Target.activeSelf) active++;
            var curfew = NetworkSingleton<CurfewManager>.Instance;
            string flags = NetworkGuard.IsAlive(curfew)
                ? $"{curfew._IsEnabled_k__BackingField}/{curfew._IsCurrentlyActive_k__BackingField}/{curfew._IsHardCurfewActive_k__BackingField}"
                : "unavailable";
            return $"activeTargets={active}/{_leases.Count} curfewRaw={flags}";
        }
        catch (Exception ex) { return "readback ERROR: " + ex.Message; }
    }

    private static void CheckBound(int count)
    {
        if (count > MaxEntries) throw new InvalidOperationException("Police registry safety limit exceeded: " + count);
    }

    private static VehicleOccupancy ReadPlayerOccupancy(LandVehicle vehicle)
    {
        try
        {
            var seats = vehicle.Seats;
            return SessionPolicy.ReadVehicleOccupancy(
                () => seats == null ? -1 : seats.Length,
                i =>
                {
                    var seat = seats![i];
                    if (!NetworkGuard.IsAlive(seat)) return null;
                    Player? occupant = seat.Occupant;
                    if (ReferenceEquals(occupant, null)) return false;
                    return NetworkGuard.IsAlive(occupant) ? true : null;
                });
        }
        catch { return VehicleOccupancy.Unknown; }
    }
}
