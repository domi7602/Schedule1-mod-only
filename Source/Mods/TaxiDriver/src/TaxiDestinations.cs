using System;
using System.Collections.Generic;
using System.Globalization;
using Il2CppScheduleOne.Economy;
using Il2CppScheduleOne.Map;
using Il2CppScheduleOne.Vehicles.AI;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// Stage 3d — the destination catalog. Dominik: "wir können Deal locations als
/// Wegpunkte setzen".
///
/// The catalog replaces the two hand-read magic coordinates (ROAD A / ROAD B)
/// with places the GAME itself owns:
/// <list type="bullet">
///   <item>
///     <b>Deal locations</b> — every live <see cref="DeliveryLocation"/>. The
///     arrival anchor is <c>TeleportPoint</c>: the point the game itself
///     teleports the player to on arrival, so it is a guaranteed walkable,
///     valid spot. <c>CustomerStandPoint</c> and the transform are the fallbacks.
///   </item>
///   <item>
///     <b>Parking lot entries</b> — every live <see cref="ParkingLot"/>'s
///     <c>EntryPoint</c> as a logical anchor, not proof of a drivable endpoint.
///   </item>
/// </list>
///
/// <see cref="ResolveArrival"/> carries the "a taxi does not have to stop at the
/// doorstep" rule by selecting a nearby strict street candidate. Native full
/// calculation and a confirmed physical arrival remain separate requirements.
/// </summary>
internal static class TaxiDestinations
{
    /// <summary>Existing 3D goal-to-drop-off selection budget, not the physical arrival radius.</summary>
    internal const float LotDropOffRadius = 60f;

    internal const string KindDeal = "deal";
    internal const string KindLot = "lot";

    /// <summary>
    /// Properties (Motel, Barn, ...) — Dominik's actual A→B travel targets ("ich bin
    /// im Motel und möchte zur Barn"). Owned ones are the `HOME` rows.
    /// </summary>
    internal const string KindProperty = "property";

    /// <summary>One selectable place. <see cref="Goal"/> is where the player wants to go.</summary>
    internal sealed class Destination
    {
        internal string Name = "<unnamed>";
        internal string Kind = KindDeal;
        internal Vector3 Goal;
        internal bool HasTeleportAnchor;
        internal bool IsOwned;
        internal int Index = -1;

        internal string Label => Kind == KindProperty
            ? $"{(IsOwned ? "HOME" : "PROP")} * {Name}"
            : Kind == KindDeal ? $"DEAL * {Name}" : $"PARK * {Name}";

        /// <summary>
        /// Short right-aligned tag for the app row (the row carries the name as its
        /// primary text now — a "DEAL * " prefix used to eat the readable width).
        /// </summary>
        internal string Tag => Kind == KindProperty ? (IsOwned ? "HOME" : "PROP") : Kind == KindDeal ? "DEAL" : "PARK";
    }

    /// <summary>Selected drop-off candidate and reason; not full-route or physical-arrival evidence.</summary>
    internal sealed class Arrival
    {
        internal bool Ok;
        internal Vector3 Point;
        internal string Kind = "<none>";
        internal float SnapDelta;
        internal string LotName = string.Empty;
        internal string Reason = string.Empty;
    }

    // ----------------------------------------------------------------- catalog

    /// <summary>
    /// Enumerates every live deal location and parking lot into one list (deal
    /// locations first, each group sorted by name). One-shot — never call this
    /// per frame (both enumerations walk the scene).
    /// </summary>
    internal static bool BuildCatalog(out List<Destination> catalog, string reason)
    {
        catalog = new List<Destination>();

        // Properties = the actual A→B travel targets (Dominik: "ich bin im Motel und
        // möchte zur Barn"). Owned properties first (HOME rows), then the rest
        // (PROP) — the drop-off anchor is the property's exterior spawn point.
        var props = new List<Destination>();
        try
        {
            foreach (S1API.Property.PropertyWrapper property in S1API.Property.PropertyManager.GetAllProperties())
            {
                if (property == null || string.IsNullOrWhiteSpace(property.PropertyName))
                    continue;

                Vector3 goal;
                try
                {
                    goal = property.ExteriorSpawnPosition;
                }
                catch (Exception)
                {
                    continue;
                }

                bool owned;
                try
                {
                    owned = property.IsOwned;
                }
                catch (Exception)
                {
                    owned = false;
                }

                props.Add(new Destination
                {
                    Kind = KindProperty,
                    Name = property.PropertyName.Trim(),
                    Goal = goal,
                    IsOwned = owned,
                });
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[pois] PropertyManager.GetAllProperties() failed ({reason}): {ex.Message} — no property rows.");
        }

        props.Sort((a, b) =>
        {
            int owned = b.IsOwned.CompareTo(a.IsOwned);
            return owned != 0 ? owned : string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
        });

        try
        {
            DeliveryLocation[]? locations = UnityEngine.Object.FindObjectsByType<DeliveryLocation>(FindObjectsSortMode.None);
            if (locations != null)
            {
                foreach (DeliveryLocation? location in locations)
                {
                    if (!Usable(location))
                        continue;

                    Destination? entry = Read(location);
                    if (entry != null)
                        catalog.Add(entry);
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"[pois] FindObjectsByType<DeliveryLocation> failed ({reason}): {ex.Message}");
        }

        catalog.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

        var lots = new List<Destination>();
        try
        {
            ParkingLot[]? found = UnityEngine.Object.FindObjectsByType<ParkingLot>(FindObjectsSortMode.None);
            if (found != null)
            {
                foreach (ParkingLot? lot in found)
                {
                    if (!Usable(lot))
                        continue;

                    Destination? entry = Read(lot);
                    if (entry != null)
                        lots.Add(entry);
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"[pois] FindObjectsByType<ParkingLot> failed ({reason}): {ex.Message}");
        }

        lots.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

        // Cleanup pass (Dominik 2026-09-29: "wenn 5x Parking gelistet wird, weiß man
        // am Ende nicht wo man rauskommt"): the list carries only MAIN checkpoints —
        // properties, then deal locations, then uniquely named places.
        // Generically named lots ("Parking", "Parking lot") and duplicate names are
        // hidden (they still serve internally as drop-off entries — only the LIST
        // is filtered).
        var final = new List<Destination>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        int propKept = 0;
        foreach (Destination d in props)
        {
            if (d.Name.Length == 0 || !seen.Add(d.Name))
                continue;
            final.Add(d);
            propKept++;
        }

        int dealKept = 0;
        foreach (Destination d in catalog)
        {
            d.Name = d.Name.Trim();
            if (!seen.Add(d.Name))
                continue;
            final.Add(d);
            dealKept++;
        }

        int lotKept = 0;
        int lotHidden = 0;
        foreach (Destination d in lots)
        {
            d.Name = d.Name.Trim();
            if (IsGenericLotName(d.Name) || !seen.Add(d.Name))
            {
                lotHidden++;
                continue;
            }

            final.Add(d);
            lotKept++;
        }

        catalog = final;

        for (int i = 0; i < catalog.Count; i++)
            catalog[i].Index = i;

        Mod.Log.Info(
            $"[pois] {reason}: {propKept} property(ies) + {dealKept} deal location(s) + {lotKept} named place(s) = " +
            $"{catalog.Count} destination(s) ({lotHidden} generic/duplicate lot(s) hidden).");
        return catalog.Count > 0;
    }

    /// <summary>
    /// The noise Dominik complained about: lots whose object name says nothing
    /// about where they are ("Parking" ×7, "Parking lot" ×2). Hidden from the
    /// destination list — renamed/uniquely named lots stay.
    /// </summary>
    private static bool IsGenericLotName(string name)
    {
        return name.Length == 0 ||
               string.Equals(name, "Parking", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(name, "Parking lot", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>`taxi pois` — logs the whole catalog so the list can be checked against the map.</summary>
    internal static bool DumpPois(string reason)
    {
        if (!BuildCatalog(out List<Destination> catalog, reason))
        {
            Mod.Log.Error("[pois] no destination found — is the Main scene loaded?");
            return false;
        }

        foreach (Destination d in catalog)
        {
            string detail = d.Kind == KindDeal
                ? $" teleportAnchor={(d.HasTeleportAnchor ? "yes" : "no (fallback)")}"
                : d.Kind == KindProperty ? $" (property exterior{(d.IsOwned ? ", owned" : string.Empty)})"
                : " (lot entry)";
            TaxiLog.Verbose($"[pois]  [{d.Index,2}] {d.Tag,-4} '{d.Name}' goal={Fmt(d.Goal)}{detail}");
        }

        Mod.Log.Info($"[pois] {catalog.Count} destination(s) listed — `taxi to <name|index>` picks one.");
        return true;
    }

    // -------------------------------------------------------------------- find

    /// <summary>
    /// Resolves a catalog entry by index or name (exact first, then substring;
    /// a deal location wins the tie against a parking lot of the same name).
    /// </summary>
    internal static bool TryFind(string query, out Destination destination)
    {
        destination = new Destination();
        string wanted = (query ?? string.Empty).Trim();
        if (wanted.Length == 0)
        {
            Mod.Log.Warn("[pois] `taxi to` needs a destination — use a name or an index (`taxi pois` lists them).");
            return false;
        }

        if (!BuildCatalog(out List<Destination> catalog, $"resolve '{wanted}'"))
            return false;

        if (int.TryParse(wanted, NumberStyles.Integer, CultureInfo.InvariantCulture, out int index))
        {
            if (index >= 0 && index < catalog.Count)
            {
                destination = catalog[index];
                return true;
            }

            Mod.Log.Warn($"[pois] index {index} is out of range (0..{catalog.Count - 1}).");
            return false;
        }

        string flat = wanted.Replace(" ", string.Empty);

        foreach (Destination d in catalog)
        {
            if (Equals(d.Name, wanted) || Equals(d.Name.Replace(" ", string.Empty), flat))
            {
                destination = d;
                return true;
            }
        }

        var hits = new List<Destination>();
        foreach (Destination d in catalog)
        {
            if (d.Name.IndexOf(wanted, StringComparison.OrdinalIgnoreCase) >= 0)
                hits.Add(d);
        }

        if (hits.Count == 1)
        {
            destination = hits[0];
            return true;
        }

        if (hits.Count > 1)
        {
            // Tie priority: an owned property beats a property beats a deal beats a
            // lot (properties are the A→B travel targets).
            Destination? pick = hits.Find(h => h.Kind == KindProperty && h.IsOwned)
                                ?? hits.Find(h => h.Kind == KindProperty)
                                ?? hits.Find(h => h.Kind == KindDeal);
            if (pick != null)
            {
                Mod.Log.Info($"[pois] '{wanted}' matched {hits.Count} places — using the {pick.Kind} place '{pick.Name}'.");
                destination = pick;
                return true;
            }

            Mod.Log.Warn($"[pois] '{wanted}' matched {hits.Count} places — be more specific:");
            foreach (Destination h in hits)
                Mod.Log.Warn($"[pois]    [{h.Index,2}] {h.Label} goal={Fmt(h.Goal)}");
            return false;
        }

        Mod.Log.Warn($"[pois] no destination matches '{wanted}' — `taxi pois` dumps the list.");
        return false;
    }

    // ----------------------------------------------------------------- arrival

    /// <summary>
    /// Selects a roadside candidate for the logical goal without a General-graph
    /// rebound or an assumption that raw parking entries are drivable.
    /// </summary>
    internal static bool ResolveArrival(Destination destination, out Arrival arrival)
    {
        arrival = new Arrival();

        if (RoadTarget.TryStreetDropOff(destination.Goal, out Vector3 candidate, out string reason))
        {
            arrival.Ok = true;
            arrival.Point = candidate;
            arrival.Kind = "roadside candidate";
            arrival.SnapDelta = Vector3.Distance(destination.Goal, candidate);
            arrival.Reason = reason;
            return true;
        }

        arrival.Reason = reason;
        Mod.Log.Error($"[arrival] '{destination.Name}': {arrival.Reason} — ride refused (no blind dispatch).");
        return false;
    }

    /// <summary>
    /// Watchdog fallback: a lot-derived street candidate near the passenger's
    /// logical goal, distinct from the physical target already failing.
    /// </summary>
    internal static bool TryFallbackDropOff(Vector3 goal, Vector3 currentTarget, out Vector3 point, out string lotName)
    {
        point = Vector3.zero;
        lotName = string.Empty;
        if (!float.IsFinite(goal.x) || !float.IsFinite(goal.y) || !float.IsFinite(goal.z) ||
            !float.IsFinite(currentTarget.x) || !float.IsFinite(currentTarget.y) || !float.IsFinite(currentTarget.z)) return false;
        bool found = false;
        float best = float.MaxValue;

        try
        {
            ParkingLot[]? lots = UnityEngine.Object.FindObjectsByType<ParkingLot>(FindObjectsSortMode.None);
            if (lots == null)
                return false;

            foreach (ParkingLot? lot in lots)
            {
                if (!Usable(lot))
                    continue;

                Vector3 candidate;
                string name;
                try
                {
                    Transform? door = lot.EntryPoint ?? lot.ExitPoint;
                    if (door == null || door.Pointer == IntPtr.Zero) continue;
                    candidate = door.position;
                    name = SafeName(lot);
                }
                catch
                {
                    continue;
                }

                // Raw lot entries are only samples. Separation, ranking and the logical
                // goal budget all apply AFTER strict street projection, never before it.
                if (!RoadTarget.TryStreetDropOff(candidate, out candidate, out _)) continue;
                float separation = Vector3.Distance(candidate, currentTarget);
                if (!float.IsFinite(separation) || separation < 5f)
                    continue;

                float d = Vector3.Distance(goal, candidate);
                if (float.IsFinite(d) && d <= LotDropOffRadius && d < best)
                {
                    best = d;
                    point = candidate;
                    lotName = name;
                    found = true;
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[arrival] fallback lot scan failed: {ex.Message}");
            return false;
        }

        return found;
    }

    // ------------------------------------------------------------------ reader

    private static Destination? Read(DeliveryLocation location)
    {
        try
        {
            var entry = new Destination { Kind = KindDeal };

            try
            {
                string? name = location.LocationName;
                entry.Name = string.IsNullOrWhiteSpace(name) ? SafeName(location.gameObject) : name!;
            }
            catch (Exception)
            {
                entry.Name = SafeName(location.gameObject);
            }

            // TeleportPoint = where the game puts the PLAYER on arrival, so it is
            // the safest anchor; the customer stand point and the transform follow.
            try
            {
                Transform? teleport = location.TeleportPoint;
                if (teleport != null && teleport.Pointer != IntPtr.Zero)
                {
                    entry.Goal = teleport.position;
                    entry.HasTeleportAnchor = true;
                    return entry;
                }
            }
            catch (Exception)
            {
                // fall through to the stand point
            }

            try
            {
                Transform? stand = location.CustomerStandPoint;
                if (stand != null && stand.Pointer != IntPtr.Zero)
                {
                    entry.Goal = stand.position;
                    return entry;
                }
            }
            catch (Exception)
            {
                // fall through to the transform
            }

            entry.Goal = location.transform == null ? Vector3.zero : location.transform.position;
            return entry;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[pois] reading a DeliveryLocation failed: {ex.Message}");
            return null;
        }
    }

    private static Destination? Read(ParkingLot lot)
    {
        try
        {
            var entry = new Destination { Kind = KindLot, Name = SafeName(lot.gameObject) };

            try
            {
                Transform? door = lot.EntryPoint ?? lot.ExitPoint;
                if (door != null && door.Pointer != IntPtr.Zero)
                {
                    entry.Goal = door.position;
                    return entry;
                }
            }
            catch (Exception)
            {
                // fall through to the transform
            }

            entry.Goal = lot.transform == null ? Vector3.zero : lot.transform.position;
            return entry;
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[pois] reading a ParkingLot failed: {ex.Message}");
            return null;
        }
    }

    // ----------------------------------------------------------------- helpers

    /// <summary>
    /// IL2CPP guard: a scene unload can leave a collected handle that still looks
    /// non-null to C#.
    /// </summary>
    private static bool Usable(UnityEngine.Object? candidate)
    {
        try
        {
            return candidate != null && candidate.Pointer != IntPtr.Zero;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Name of any Unity object (GameObject, ParkingLot, DeliveryLocation) — guarded.</summary>
    private static string SafeName(UnityEngine.Object? o)
    {
        try
        {
            return o == null ? "<no object>" : o.name;
        }
        catch (Exception ex)
        {
            return $"<name threw: {ex.Message}>";
        }
    }

    internal static string Num(float value) => value.ToString("F1", CultureInfo.InvariantCulture);

    internal static string Fmt(Vector3 v) => $"({Num(v.x)}, {Num(v.y)}, {Num(v.z)})";
}
