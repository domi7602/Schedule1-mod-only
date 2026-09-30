using System;
using System.Collections.Generic;
using Il2CppPathfinding;
using Il2CppScheduleOne.Vehicles.AI;
using UnityEngine;

namespace TaxiDriver;

/// <summary>
/// Stage 3b — finding a DRIVEABLE target near the player (F5: the taxi drives
/// from the stand to the player).
///
/// The player is not guaranteed to stand on a road, so a raw
/// <c>playerPos</c> dispatch is exactly the pattern that silently failed in
/// round 5/11 (target off the graph). The resolver therefore builds a pool of
/// candidate points, projects every one through
/// <c>NavigationUtility.SampleVehicleGraph</c> and picks the projected point
/// that is both (a) on the graph — snap delta ≤ <see cref="PreferredSnapDelta"/>
/// — and (b) nearest to the player. The pool contains:
/// <list type="bullet">
///   <item>the nearest node of the ROAD A* graph (<c>AstarPath.GetNearest</c>
///         masked to <c>VehicleAgent.RoadNodesGraphName</c>) — the "closest road
///         point" the game itself exposes, and</li>
///   <item>the proven F1/F2 pattern: <c>SampleVehicleGraph</c> on the player
///         position plus forward/back/left/right offsets at 3/5/6/10 m.</item>
/// </list>
/// Every candidate and the winning rule are logged, so a failed run can be
/// diagnosed (and the pool widened) from the log alone.
/// </summary>
internal static class RoadTarget
{
    /// <summary>Snap delta (candidate → vehicle graph) that counts as "on the graph". Matches the proven road targets (0.1 m).</summary>
    private const float PreferredSnapDelta = 3f;

    /// <summary>Largest distance from the player that still counts as "arrived near the player" for the road-node candidate.</summary>
    private const float MaxRoadNodeDistance = 30f;

    private static bool _graphsDumped;

    // ------------------------------------------------------------------ pool

    /// <summary>
    /// Resolves the best navigation target near <paramref name="playerPos"/>.
    /// Returns <c>null</c> when no candidate could be projected onto the graph.
    /// </summary>
    internal static Vector3? FindNearPlayer(Vector3 playerPos, Vector3 playerForward)
    {
        Mod.Log.Info($"[target] resolving a road point near the player: player={SpikeCommands.Fmt(playerPos)} forward={SpikeCommands.Fmt(playerForward)}");

        if (playerForward.sqrMagnitude < 1e-4f)
            playerForward = Vector3.forward;
        Vector3 right = Vector3.Cross(Vector3.up, playerForward);
        if (right.sqrMagnitude < 1e-4f)
            right = Vector3.right;

        var candidates = new List<Candidate>();

        // (1) closest point on the ROAD graph the game exposes.
        Vector3? roadNode = NearestRoadNode(playerPos, out string roadInfo);
        float roadDistance = roadNode.HasValue ? Vector3.Distance(roadNode.Value, playerPos) : -1f;
        Mod.Log.Info($"[target] road-graph nearest: {(roadNode.HasValue ? SpikeCommands.Fmt(roadNode.Value) : "<none>")} distanceToPlayer={(roadDistance < 0f ? "-" : roadDistance.ToString("F1") + "m")} ({roadInfo})");
        if (roadNode.HasValue && roadDistance <= MaxRoadNodeDistance)
            candidates.Add(new Candidate("roadNode", roadNode.Value, fromRoadGraph: true));
        else if (roadNode.HasValue)
            Mod.Log.Warn($"[target] road node is {roadDistance:F1} m away from the player (> {MaxRoadNodeDistance:F0} m) — kept only as a logged fallback, not as a candidate.");

        // (2) the proven F1/F2 candidate sweep around the player.
        AddOffset(candidates, "player", playerPos);
        foreach (float d in new[] { 3f, 5f, 10f })
        {
            AddOffset(candidates, $"fwd+{d:F0}", playerPos + playerForward * d);
            AddOffset(candidates, $"fwd-{d:F0}", playerPos - playerForward * d);
            AddOffset(candidates, $"right+{d:F0}", playerPos + right * d);
            AddOffset(candidates, $"right-{d:F0}", playerPos - right * d);
        }

        // (3) a 6 m ring, so a sideways street still gets a near candidate.
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(
                playerForward.x * Mathf.Cos(angle) + right.x * Mathf.Sin(angle),
                0f,
                playerForward.z * Mathf.Cos(angle) + right.z * Mathf.Sin(angle));
            AddOffset(candidates, $"ring{i * 45}", playerPos + dir * 6f);
        }

        // Project every candidate onto the vehicle graph.
        int projected = 0;
        foreach (Candidate c in candidates)
        {
            try
            {
                c.Destination = NavigationUtility.SampleVehicleGraph(c.SamplePoint);
                c.SnapDelta = Vector3.Distance(c.SamplePoint, c.Destination);
                c.DistanceToPlayer = Vector3.Distance(c.Destination, playerPos);
                c.Projected = true;
                projected++;
            }
            catch (Exception ex)
            {
                c.Error = ex.Message;
            }
        }

        Mod.Log.Info($"[target] {candidates.Count} candidate(s), {projected} projected onto the vehicle graph (eligible = snap delta ≤ {PreferredSnapDelta:F0} m):");
        foreach (Candidate c in candidates)
            Mod.Log.Info($"[target]   {c}");

        if (projected == 0)
        {
            Mod.Log.Error("[target] every SampleVehicleGraph call threw — no target available (see the errors above).");
            return null;
        }

        // Rule A: eligible (on-graph) candidate closest to the player; the road
        // graph node wins a tie because it is a real road point.
        Candidate? best = null;
        foreach (Candidate c in candidates)
        {
            if (!c.Projected || c.SnapDelta > PreferredSnapDelta)
                continue;
            if (best == null || IsBetter(c, best))
                best = c;
        }

        string rule = $"on-graph (delta ≤ {PreferredSnapDelta:F0} m) and closest to the player";

        // Rule B: nothing eligible → smallest snap delta, i.e. the point the
        // graph can actually serve, even if it sits further away.
        if (best == null)
        {
            foreach (Candidate c in candidates)
            {
                if (!c.Projected)
                    continue;
                if (best == null || c.SnapDelta < best.SnapDelta - 0.001f)
                    best = c;
            }

            rule = $"NO candidate within the {PreferredSnapDelta:F0} m delta budget → smallest snap delta";
            Mod.Log.Warn($"[target] {rule}.");
        }

        if (best == null)
        {
            Mod.Log.Error("[target] no usable candidate survived the rules.");
            return null;
        }

        Mod.Log.Info(
            $"[target] CHOSEN rule='{rule}' label='{best.Label}' fromRoadGraph={best.FromRoadGraph} " +
            $"sample={SpikeCommands.Fmt(best.SamplePoint)} destination={SpikeCommands.Fmt(best.Destination)} " +
            $"snapDelta={best.SnapDelta:F1}m distanceToPlayer={best.DistanceToPlayer:F1}m");
        return best.Destination;
    }

    private static bool IsBetter(Candidate c, Candidate current)
    {
        if (Math.Abs(c.DistanceToPlayer - current.DistanceToPlayer) > 0.5f)
            return c.DistanceToPlayer < current.DistanceToPlayer;

        // Within 0.5 m: prefer the real road node, then the smaller snap delta.
        if (c.FromRoadGraph != current.FromRoadGraph)
            return c.FromRoadGraph;
        return c.SnapDelta < current.SnapDelta;
    }

    private static void AddOffset(List<Candidate> candidates, string label, Vector3 point)
        => candidates.Add(new Candidate(label, point, fromRoadGraph: false));

    // ------------------------------------------------------------- road graph

    /// <summary>
    /// Closest node of the road A* graph — the vanilla "closest road point"
    /// (<c>AstarPath.GetNearest</c> masked to
    /// <c>VehicleAgent.RoadNodesGraphName</c>, with a "name contains road"
    /// fallback). Returns <c>null</c> (with the reason in <paramref name="info"/>)
    /// when no road graph or no node can be found.
    /// </summary>
    internal static Vector3? NearestRoadNode(Vector3 point, out string info)
    {
        try
        {
            global::Il2Cpp.AstarPath? astar = global::Il2Cpp.AstarPath.active;
            if (astar == null)
            {
                info = "AstarPath.active is null";
                return null;
            }

            var graphs = astar.graphs;
            if (graphs == null || graphs.Length == 0)
            {
                info = "AstarPath.graphs is null/empty";
                return null;
            }

            string wanted;
            try
            {
                wanted = VehicleAgent.RoadNodesGraphName ?? "<null>";
            }
            catch (Exception ex)
            {
                wanted = $"<threw: {ex.Message}>";
            }

            int index = -1;
            for (int i = 0; i < graphs.Length; i++)
            {
                if (SafeGraphName(graphs[i], out string n).Equals(wanted, StringComparison.OrdinalIgnoreCase))
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
            {
                for (int i = 0; i < graphs.Length; i++)
                {
                    if (SafeGraphName(graphs[i], out string n).IndexOf("road", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index < 0)
            {
                var names = new List<string>();
                for (int i = 0; i < graphs.Length; i++)
                    names.Add($"[{i}]{DescribeGraph(graphs[i])}");
                info = $"no graph named '{wanted}' (or containing 'road') among {graphs.Length}: {string.Join(" ", names)}";
                return null;
            }

            string graphName = SafeGraphName(graphs[index], out _);
            var constraint = new NNConstraint();
            constraint.graphMask = GraphMask.FromGraph(graphs[index]);

            NNInfo? nearest = astar.GetNearest(point, constraint);
            if (nearest == null || nearest.node == null)
            {
                info = $"graph[{index}]='{graphName}' returned no node";
                return null;
            }

            Vector3 position = nearest.position;
            info = $"graph[{index}]='{graphName}' (wanted='{wanted}') node={SpikeCommands.Fmt(position)}";
            return position;
        }
        catch (Exception ex)
        {
            info = $"threw: {ex.Message}";
            return null;
        }
    }

    /// <summary>One-shot A* graph inventory — printed with the parking-lot dump so a target failure can be diagnosed offline.</summary>
    internal static void DumpGraphs()
    {
        if (_graphsDumped)
            return;

        try
        {
            string wanted;
            try
            {
                wanted = VehicleAgent.RoadNodesGraphName ?? "<null>";
            }
            catch (Exception ex)
            {
                wanted = $"<threw: {ex.Message}>";
            }

            string vehicleGraph;
            try
            {
                vehicleGraph = VehicleAgent.VehicleGraphName ?? "<null>";
            }
            catch (Exception ex)
            {
                vehicleGraph = $"<threw: {ex.Message}>";
            }

            Mod.Log.Info($"[graphs] VehicleAgent.RoadNodesGraphName='{wanted}' VehicleGraphName='{vehicleGraph}'");

            global::Il2Cpp.AstarPath? astar = global::Il2Cpp.AstarPath.active;
            if (astar == null)
            {
                Mod.Log.Info("[graphs] AstarPath.active is null (A* not initialised yet).");
                return;
            }

            var graphs = astar.graphs;
            if (graphs == null || graphs.Length == 0)
            {
                Mod.Log.Info("[graphs] AstarPath.graphs is null/empty.");
                return;
            }

            Mod.Log.Info($"[graphs] AstarPath.graphs: {graphs.Length}");
            for (int i = 0; i < graphs.Length; i++)
                Mod.Log.Info($"  [{i}] {DescribeGraph(graphs[i])}");

            _graphsDumped = true;
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"[graphs] dump failed: {ex.Message}");
        }
    }

    private static string SafeGraphName(NavGraph graph, out string name)
    {
        name = "<null graph>";
        if (graph == null)
            return name;
        try
        {
            name = string.IsNullOrEmpty(graph.name) ? "<unnamed>" : graph.name;
        }
        catch (Exception ex)
        {
            name = $"<threw: {ex.Message}>";
        }

        return name;
    }

    private static string DescribeGraph(NavGraph graph)
    {
        if (graph == null)
            return "<null>";
        try
        {
            return $"name='{(string.IsNullOrEmpty(graph.name) ? "<unnamed>" : graph.name)}' type={graph.GetType().Name}";
        }
        catch (Exception ex)
        {
            return $"<threw: {ex.Message}>";
        }
    }

    /// <summary>One candidate point, its projection and the numbers the rule compares.</summary>
    private sealed class Candidate
    {
        internal readonly string Label;
        internal readonly Vector3 SamplePoint;
        internal readonly bool FromRoadGraph;

        internal bool Projected;
        internal Vector3 Destination;
        internal float SnapDelta = float.MaxValue;
        internal float DistanceToPlayer = float.MaxValue;
        internal string? Error;

        internal Candidate(string label, Vector3 samplePoint, bool fromRoadGraph)
        {
            Label = label;
            SamplePoint = samplePoint;
            FromRoadGraph = fromRoadGraph;
        }

        public override string ToString()
        {
            if (!Projected)
                return $"[{Label}] sample={SpikeCommands.Fmt(SamplePoint)} SampleVehicleGraph FAILED: {Error}";

            string eligible = SnapDelta <= PreferredSnapDelta ? "ELIGIBLE" : "off-graph";
            return $"[{Label}] sample={SpikeCommands.Fmt(SamplePoint)} dest={SpikeCommands.Fmt(Destination)} " +
                   $"delta={SnapDelta:F1}m destDist={DistanceToPlayer:F1}m {eligible}{(FromRoadGraph ? " (road graph)" : string.Empty)}";
        }
    }
}
