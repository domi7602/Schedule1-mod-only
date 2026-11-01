using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TownHallInterior;

// Read-only preflight for the extracted f12 Town Hall. This is intentionally NOT a
// scene adapter or transaction: source observations do not establish runtime identity,
// native static-batch rendering, a safe replacement shell, or a safe exit. No mutation
// and no publication may be added on the strength of this check alone.
public readonly record struct NativeColliderObservation(
    string Path, string Kind,
    float MinX, float MinY, float MinZ, float MaxX, float MaxY, float MaxZ,
    bool Enabled, bool IsTrigger, int ComponentCount);

public sealed record NativeSceneObservation(
    string BuildingPath, string BuildingGuid,
    IReadOnlyList<NativeColliderObservation> Colliders,
    int BuildingCount);

public readonly record struct NativeSceneInspection(bool IdentityAndGeometryMatch, bool CanPublish, string Reason);

public static class NativeSceneSwap
{
    public const string BuildingPath = "Map/Hyland Point/Region_Downtown/TownCenter/TownHall";
    public const string BuildingGuid = "c747a750-98cb-47fc-9c40-cf5912acc390";
    private const float BoundsTolerance = 0.025f;

    // World bounds from townhall_measure_world.out; no global sharedMesh bounds.
    // Enabled is intentionally not a precondition: original enabled states must be
    // observed and eventually restored exactly, not assumed to be true.
    private static readonly ReadOnlyCollection<NativeColliderObservation> Manifest = Array.AsReadOnly(new[]
    {
        new NativeColliderObservation(BuildingPath + "/townhall/Walls", "BoxCollider", 54.35f, 1.25f, 20.50f, 63.35f, 8.25f, 41.50f, true, false, 1),
        new NativeColliderObservation(BuildingPath + "/Cube", "BoxCollider", 54.40f, 0.65f, 20.60f, 63.30f, 9.65f, 41.40f, true, false, 1),
        new NativeColliderObservation(BuildingPath + "/townhall/UpperBrickTrim", "BoxCollider", 54.30f, 4.60f, 20.45f, 63.40f, 4.90f, 41.55f, true, false, 1),
        new NativeColliderObservation(BuildingPath + "/townhall/LowerBrickTrim", "BoxCollider", 54.30f, 1.10f, 20.45f, 63.40f, 1.40f, 41.55f, true, false, 1),
        new NativeColliderObservation(BuildingPath + "/townhall/Door/Quad", "MeshCollider", 54.192f, 1.092f, 30.039f, 54.192f, 3.950f, 31.961f, true, false, 1),
    });

    public static IReadOnlyList<NativeColliderObservation> Candidates => Manifest;

    public static NativeSceneInspection Inspect(NativeSceneObservation? scene)
    {
        if (scene == null || scene.BuildingCount != 1 ||
            !string.Equals(scene.BuildingPath, BuildingPath, StringComparison.Ordinal) ||
            !string.Equals(scene.BuildingGuid, BuildingGuid, StringComparison.OrdinalIgnoreCase) || scene.Colliders == null)
            return Reject("Town Hall path, GUID, unique building, or observation missing");

        foreach (var expected in Manifest)
        {
            int matches = 0;
            NativeColliderObservation found = default;
            foreach (var candidate in scene.Colliders)
                if (string.Equals(candidate.Path, expected.Path, StringComparison.Ordinal))
                {
                    matches++;
                    found = candidate;
                }
            if (matches != 1 || found.ComponentCount != 1 || found.IsTrigger ||
                !string.Equals(found.Kind, expected.Kind, StringComparison.Ordinal) || !BoundsMatch(found, expected))
                return Reject("Missing, ambiguous, trigger, wrong type or bounds: " + expected.Path);
        }

        return new NativeSceneInspection(true, false,
            "Publication blocked: exterior-preserving static-batch replacement, runtime identity, player clearance, and safe teardown are unverified.");
    }

    private static NativeSceneInspection Reject(string reason) => new(false, false, reason);

    private static bool BoundsMatch(NativeColliderObservation a, NativeColliderObservation b) =>
        Near(a.MinX, b.MinX) && Near(a.MinY, b.MinY) && Near(a.MinZ, b.MinZ) &&
        Near(a.MaxX, b.MaxX) && Near(a.MaxY, b.MaxY) && Near(a.MaxZ, b.MaxZ);

    private static bool Near(float a, float b) => !float.IsNaN(a) && !float.IsInfinity(a) && Math.Abs(a - b) <= BoundsTolerance;
}
