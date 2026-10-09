using System;
using Il2CppScheduleOne.Map;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// Stage 3b — the fixed taxi stand (Dominik: "das Auto nur via den Taxi-Fahrer
/// spawnen … einen fixen Punkt, wo er losfährt").
///
/// The taxi is never spawned at the player any more: it starts on a vanilla
/// <see cref="ParkingLot"/> spot and drives TO the player (TaxiApp CALL TAXI).
///
/// Two configuration values define the stand and are finalised from the live
/// ParkingLot dump (see <see cref="DumpLots"/>):
/// <list type="bullet">
///   <item><see cref="StandName"/> — the lot name as it appears on the map ("Parking Garage" = the downtown public-service parking garage).</item>
///   <item><see cref="StandCoordinate"/> — the world position of that lot's street-side ENTRY (the drive-in point, not a parking spot: spots inside the garage structure are not routable).</item>
/// </list>
/// The resolution never trusts the constants blindly: <see cref="TryResolve"/>
/// matches the live lots by coordinate first, then by name, and always logs the
/// rule that won plus every candidate, so a wrong guess is visible in the log.
/// </summary>
internal static class TaxiStand
{
    // ---------------------------------------------------------------- config

    /// <summary>
    /// Expected lot name (case-insensitive substring). Dominik confirmed on the
    /// map that the stand is the <b>downtown "Parking Garage"</b> (the
    /// public-service marker, Brad Crosby's dealer tent sits there too).
    /// </summary>
    internal const string StandName = "Parking Garage";

    /// <summary>
    /// Fallback name filter when no lot carries <see cref="StandName"/> verbatim
    /// (lots may be named differently in the scene hierarchy).
    /// </summary>
    internal const string SecondaryNameFilter = "garage";

    /// <summary>
    /// Third filter: the central downtown lot, used when neither "Parking Garage"
    /// nor "garage" matches any live lot.
    /// </summary>
    internal const string TertiaryNameFilter = "downtown";

    /// <summary>
    /// World position of the stand spawn — finalised from the live ParkingLot
    /// dump of 2026-09-25 (scene 'Main', 33 lots): lot [22] 'ParkingGarage'
    /// at (-3.2, 0.0, 82.0), 26/26 spots free, first spot (2.0, 0.2, 76.5),
    /// ENTRY (-13.0, 0.0, 84.2), EXIT (-13.0, 0.2, 86.0).
    ///
    /// The constant anchors the ENTRY (street side), not a parking spot:
    /// spots inside the garage structure are not routable — two pickup runs
    /// spawned on spot[0] failed path calculation in 0.1 s
    /// (NavigationCalculationCallback(result=Failed, path=null)) for BOTH
    /// NavigationSettings and settings=null, including the vanilla-proven
    /// target (-131.4,-4,51.9), while a diagnostic spawn near the
    /// player completed in 17.6 s. The entry is the lot's drive-in point and
    /// therefore open street space.
    /// </summary>
    internal static readonly Vector3 StandCoordinate = new Vector3(-13.0f, 0.0f, 84.2f);

    /// <summary>Tolerance for the "lot at the configured coordinate" rule (metres).</summary>
    private const float CoordinateMatchTolerance = 10f;

    // ------------------------------------------------------------------ dump

    /// <summary>
    /// Logs every live <see cref="ParkingLot"/> instance: GameObject name, world
    /// position, spot count, free/usable spots, the FIRST spot position (the true
    /// spawn position), its forward, the alignment point, entry/exit points and
    /// the baked GUID. Also prints the A* graph names once, because the pickup
    /// target resolver needs the road graph.
    /// </summary>
    /// <param name="reason">Why the dump runs (attempt counter / command name).</param>
    /// <returns><c>true</c> when at least one lot was found.</returns>
    internal static bool DumpLots(string reason)
    {
        ParkingLot[]? lots;
        try
        {
            lots = UnityEngine.Object.FindObjectsByType<ParkingLot>(FindObjectsSortMode.None);
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"[lots] FindObjectsByType<ParkingLot> failed ({reason}): {ex.Message}");
            return false;
        }

        int count = lots == null ? 0 : lots.Length;
        TaxiLog.Verbose($"[lots] {reason}: {count} ParkingLot instance(s).");
        if (lots == null || count == 0)
            return false;

        int reported = 0;
        for (int i = 0; i < count; i++)
        {
            ParkingLot? lot = lots[i];
            if (lot == null)
            {
                TaxiLog.Verbose($"  [{i}] <null entry>");
                continue;
            }

            try
            {
                string name = SafeName(lot);
                Vector3 pos = lot.transform == null ? Vector3.zero : lot.transform.position;
                Vector3 lotForward = lot.transform == null ? Vector3.forward : Flat(lot.transform.forward);

                int spotCount = 0;
                int freeCount = 0;
                string firstSpot = "<no spots>";
                string firstSpotForward = "-";
                string alignmentPoint = "-";
                string firstFree = "<none>";

                var spots = lot.ParkingSpots;
                if (spots != null)
                {
                    spotCount = spots.Count;
                    for (int s = 0; s < spotCount; s++)
                    {
                        ParkingSpot? spot = spots[s];
                        if (spot == null)
                            continue;

                        bool usable = false;
                        bool occupied = false;
                        try
                        {
                            usable = spot.IsUsable;
                            occupied = spot.OccupantVehicle != null;
                        }
                        catch (Exception ex)
                        {
                            TaxiLog.Verbose($"      spot[{s}] native getter failed: {ex.Message}");
                        }

                        if (usable && !occupied)
                        {
                            freeCount++;
                            if (firstFree == "<none>")
                                firstFree = $"[{s}] {Fmt(spot.transform == null ? Vector3.zero : spot.transform.position)}";
                        }

                        if (s == 0)
                        {
                            Vector3 spotPos = spot.transform == null ? Vector3.zero : spot.transform.position;
                            Vector3 spotFwd = Flat(spot.transform == null ? Vector3.forward : spot.transform.forward);
                            firstSpot = $"[0] {Fmt(spotPos)} usable={usable} occupied={occupied}";
                            firstSpotForward = Fmt(spotFwd);
                            try
                            {
                                Transform? ap = spot.AlignmentPoint;
                                alignmentPoint = ap == null ? "<null>" : $"{Fmt(ap.position)} fwd={Fmt(Flat(ap.forward))}";
                            }
                            catch (Exception ex)
                            {
                                alignmentPoint = $"<threw: {ex.Message}>";
                            }
                        }
                    }
                }

                string entry = "<null>";
                string exit = "<null>";
                try
                {
                    Transform? ep = lot.EntryPoint;
                    entry = ep == null ? "<null>" : Fmt(ep.position);
                }
                catch (Exception ex)
                {
                    entry = $"<threw: {ex.Message}>";
                }

                try
                {
                    Transform? xp = lot.ExitPoint;
                    exit = xp == null ? "<null>" : Fmt(xp.position);
                }
                catch (Exception ex)
                {
                    exit = $"<threw: {ex.Message}>";
                }

                string guid;
                try
                {
                    guid = lot.BakedGUID ?? "<null>";
                }
                catch (Exception ex)
                {
                    guid = $"<threw: {ex.Message}>";
                }

                TaxiLog.Verbose(
                    $"  [{i}] name='{name}' worldPos={Fmt(pos)} lotForward={Fmt(lotForward)} " +
                    $"spots={spotCount} freeUsable={freeCount}");
                TaxiLog.Verbose(
                    $"      firstSpot={firstSpot} firstSpotForward={firstSpotForward} alignmentPoint={alignmentPoint}");
                TaxiLog.Verbose(
                    $"      firstFreeSpot={firstFree} entryPoint={entry} exitPoint={exit} guid='{guid}'");
                reported++;
            }
            catch (Exception ex)
            {
                TaxiLog.Verbose($"  [{i}] dump failed for this lot: {ex.Message}");
            }
        }

        TaxiLog.Verbose($"[lots] {reason}: {reported}/{count} lot(s) reported — stand candidates above.");
        RoadTarget.DumpGraphs();
        return reported > 0;
    }

    // --------------------------------------------------------------- resolve

    /// <summary>
    /// Picks the stand from the live lots. Rules, in order (the first that fires
    /// wins and every rule is logged):
    /// <ol>
    ///   <li>lot whose first spot or entry sits within <see cref="CoordinateMatchTolerance"/> of <see cref="StandCoordinate"/> (the configured coordinate is authoritative),</li>
    ///   <li>name contains <see cref="StandName"/> ("Parking Garage"),</li>
    ///   <li>name contains <see cref="SecondaryNameFilter"/> ("garage"),</li>
    ///   <li>name contains <see cref="TertiaryNameFilter"/> ("downtown"),</li>
    ///   <li>most central lot (closest to the centroid of all lots),</li>
    ///   <li>first lot found.</li>
    /// </ol>
    /// </summary>
    internal static bool TryResolve(out StandSelection selection)
    {
        selection = new StandSelection();

        ParkingLot[]? lots;
        try
        {
            lots = UnityEngine.Object.FindObjectsByType<ParkingLot>(FindObjectsSortMode.None);
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"[stand] FindObjectsByType<ParkingLot> failed: {ex.Message}");
            return false;
        }

        if (lots == null || lots.Length == 0)
        {
            Mod.Log.Error("[stand] no ParkingLot instance in the scene — falling back to the configured coordinate.");
            selection.LotName = "<no ParkingLot>";
            selection.Rule = "constant (no lot found)";
            selection.Position = StandCoordinate;
            selection.Forward = Vector3.forward;
            return StandCoordinate != Vector3.zero;
        }

        // Snapshot the lots so every rule compares the same data.
        var entries = new System.Collections.Generic.List<LotEntry>(lots.Length);
        foreach (ParkingLot? lot in lots)
        {
            if (lot == null)
                continue;
            LotEntry? e = Snapshot(lot);
            if (e != null)
                entries.Add(e);
        }

        if (entries.Count == 0)
        {
            Mod.Log.Error("[stand] every ParkingLot entry was unreadable — cannot resolve the stand.");
            return false;
        }

        foreach (LotEntry e in entries)
            TaxiLog.Verbose($"[stand] candidate '{e.Name}' spot0={Fmt(e.SpotPosition)} {(e.HasEntry ? $"entry={Fmt(e.EntryPosition)} " : string.Empty)}spots={e.SpotCount} distanceToConstant={(StandCoordinate == Vector3.zero ? "n/a" : StandDistance(e, StandCoordinate).ToString("F1") + "m")}");

        LotEntry? chosen = null;
        string rule = "<none>";

        // 1) configured coordinate (authoritative once finalised) — nearest of
        // the lot's spot and its street-side entry.
        if (StandCoordinate != Vector3.zero)
        {
            float best = float.MaxValue;
            foreach (LotEntry e in entries)
            {
                float d = StandDistance(e, StandCoordinate);
                if (d < best)
                {
                    best = d;
                    chosen = e;
                }
            }

            if (chosen != null && best <= CoordinateMatchTolerance)
            {
                rule = $"configured coordinate within {CoordinateMatchTolerance:F0}m ({best:F1}m)";
            }
            else
            {
                Mod.Log.Warn($"[stand] NO lot within {CoordinateMatchTolerance:F0}m of the configured coordinate {Fmt(StandCoordinate)} (nearest '{chosen?.Name}' at {best:F1}m) — falling back to the name rules.");
                chosen = null;
            }
        }
        else
        {
            TaxiLog.Verbose("[stand] configured coordinate is the (0,0,0) placeholder — skipping the coordinate rule.");
        }

        // 2..4) name filters
        if (chosen == null)
            chosen = MatchName(entries, StandName, rule2: "name contains 'Parking Garage'", ref rule);
        if (chosen == null)
            chosen = MatchName(entries, SecondaryNameFilter, rule2: "name contains 'garage'", ref rule);
        if (chosen == null)
            chosen = MatchName(entries, TertiaryNameFilter, rule2: "name contains 'downtown'", ref rule);

        // 5) most central lot
        if (chosen == null)
        {
            Vector3 centroid = Vector3.zero;
            foreach (LotEntry e in entries)
                centroid += e.SpotPosition;
            centroid /= entries.Count;

            float best = float.MaxValue;
            foreach (LotEntry e in entries)
            {
                float d = Vector3.Distance(e.SpotPosition, centroid);
                if (d < best)
                {
                    best = d;
                    chosen = e;
                }
            }

            rule = $"most central lot (centroid {Fmt(centroid)}, {best:F1}m)";
            Mod.Log.Warn("[stand] no name filter matched — using the most central lot. Compare this one against the map.");
        }

        if (chosen == null)
        {
            Mod.Log.Error("[stand] stand resolution failed (no candidate survived the rules).");
            return false;
        }

        selection.LotName = chosen.Name;
        selection.Rule = rule;
        // The taxi must be able to DRIVE AWAY: spawn on the street-side entry,
        // never inside the garage structure (spots there are not routable —
        // path calculation fails in 0.1 s from a spot spawn).
        selection.Position = chosen.HasEntry ? chosen.EntryPosition : chosen.SpotPosition;
        selection.SpawnKind = chosen.HasEntry ? "entry" : "spot";
        // Spawn position and heading must match: the entry is street-side open
        // space, but SpotForward points along the indoor parking slot (often at
        // a wall). Use the entry's own forward when we spawn on the entry.
        // Live test 2026-10-01: EntryPoint.forward (+X, east) points INTO the
        // garage — the taxi drove east into the pillars, then reversed west for
        // 8+ s toward the target. Negated: street/target side is -X (west).
        selection.Forward = chosen.HasEntry ? -chosen.EntryForward : chosen.SpotForward;
        selection.SpotIndex = chosen.SpotIndex;
        selection.SpotCount = chosen.SpotCount;
        selection.DistanceToConfiguredCoordinate =
            StandCoordinate == Vector3.zero ? -1f : StandDistance(chosen, StandCoordinate);

        Mod.Log.Info(
            $"[stand] RESOLVED lot='{selection.LotName}' rule='{selection.Rule}' " +
            $"spawnPosition={Fmt(selection.Position)} ({selection.SpawnKind}) forward={Fmt(selection.Forward)} " +
            $"entryForward={Fmt(chosen.EntryForward)} spotForward={Fmt(chosen.SpotForward)} " +
            $"spot[{selection.SpotIndex}]/{selection.SpotCount} " +
            $"distanceToConstant={(selection.DistanceToConfiguredCoordinate < 0f ? "n/a" : selection.DistanceToConfiguredCoordinate.ToString("F1") + "m")}");
        Mod.Log.Info(
            $"[stand] NOTE entry polarity: EntryPoint.forward points INTO the garage " +
            $"(live test 2026-10-01), so the spawn heading is negated (-EntryForward, street side).");
        return true;
    }

    /// <summary>
    /// Distance from a lot to the configured <see cref="StandCoordinate"/> —
    /// the nearer of its first spot and its street-side entry.
    /// </summary>
    private static float StandDistance(LotEntry e, Vector3 point)
    {
        float d = Vector3.Distance(e.SpotPosition, point);
        if (e.HasEntry)
            d = Math.Min(d, Vector3.Distance(e.EntryPosition, point));
        return d;
    }

    private static LotEntry? MatchName(System.Collections.Generic.List<LotEntry> entries, string filter, string rule2, ref string rule)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return null;

        // Normalise both sides (strip spaces): the map calls the downtown stand
        // "Parking Garage" but the live GameObject is named "ParkingGarage".
        string flat = filter.Replace(" ", string.Empty);
        foreach (LotEntry e in entries)
        {
            if (e.Name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                e.Name.Replace(" ", string.Empty).IndexOf(flat, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                rule = rule2;
                return e;
            }
        }

        return null;
    }

    /// <summary>Reads the values one rule needs; returns null when the lot is unreadable.</summary>
    private static LotEntry? Snapshot(ParkingLot lot)
    {
        try
        {
            var entry = new LotEntry
            {
                Name = SafeName(lot),
                SpotCount = 0,
                SpotIndex = -1,
                SpotPosition = lot.transform == null ? Vector3.zero : lot.transform.position,
                SpotForward = lot.transform == null ? Vector3.forward : Flat(lot.transform.forward),
            };

            // Street-side spawn anchor: the lot's drive-in point (EntryPoint,
            // ExitPoint as the fallback). Spots inside a garage structure are
            // not routable, so the taxi always starts here, never on a spot.
            try
            {
                Transform? door = lot.EntryPoint ?? lot.ExitPoint;
                if (door != null)
                {
                    entry.EntryPosition = door.position;
                    entry.EntryForward = Flat(door.forward);
                    entry.HasEntry = true;
                }
            }
            catch (Exception)
            {
                // An unreadable entry must not stop the scan — the spot stays the pick.
            }

            var spots = lot.ParkingSpots;
            if (spots == null || spots.Count == 0)
                return entry;

            entry.SpotCount = spots.Count;

            // Prefer the first usable + unoccupied spot; otherwise the first spot
            // (the task's "erste Spot-Position = wahre Spawn-Position").
            int pick = 0;
            for (int s = 0; s < spots.Count; s++)
            {
                ParkingSpot? spot = spots[s];
                if (spot == null)
                    continue;
                if (s == 0)
                    pick = 0;
                try
                {
                    if (spot.IsUsable && spot.OccupantVehicle == null)
                    {
                        pick = s;
                        break;
                    }
                }
                catch (Exception)
                {
                    // Unreadable flags must not stop the scan — index 0 stays the pick.
                }
            }

            ParkingSpot? chosen = spots[pick];
            if (chosen == null)
                return entry;

            entry.SpotIndex = pick;
            if (chosen.transform != null)
            {
                entry.SpotPosition = chosen.transform.position;
                entry.SpotForward = Flat(chosen.transform.forward);
            }

            return entry;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"[stand] Snapshot of lot '{SafeName(lot)}' failed: {ex.Message}");
            return null;
        }
    }

    private static string SafeName(ParkingLot lot)
    {
        try
        {
            return lot.gameObject == null ? "<no gameObject>" : lot.gameObject.name;
        }
        catch (Exception ex)
        {
            return $"<name threw: {ex.Message}>";
        }
    }

    private static Vector3 Flat(Vector3 v)
    {
        v.y = 0f;
        return v.sqrMagnitude < 1e-6f ? Vector3.forward : v.normalized;
    }

    internal static string Fmt(Vector3 v) => $"({v.x:F1}, {v.y:F1}, {v.z:F1})";

    /// <summary>Everything the TaxiApp pickup needs to place the taxi on the stand.</summary>
    internal sealed class StandSelection
    {
        internal string LotName = "<none>";
        internal string Rule = "<none>";
        internal Vector3 Position;
        internal string SpawnKind = "spot";
        internal Vector3 Forward = Vector3.forward;
        internal int SpotIndex = -1;
        internal int SpotCount;
        internal float DistanceToConfiguredCoordinate = -1f;
    }

    private sealed class LotEntry
    {
        internal string Name = "<unknown>";
        internal int SpotCount;
        internal int SpotIndex = -1;
        internal Vector3 SpotPosition;
        internal Vector3 SpotForward = Vector3.forward;
        internal Vector3 EntryPosition;
        internal Vector3 EntryForward = Vector3.forward;
        internal bool HasEntry;
    }
}
