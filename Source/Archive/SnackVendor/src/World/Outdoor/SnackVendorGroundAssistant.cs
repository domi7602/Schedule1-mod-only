using System;
using Il2CppInterop.Runtime;
using UnityEngine;

namespace SnackVendor.World.Outdoor;

public struct SnackVendorPlacementResult
{
    public bool IsValid;
    public Vector3 Position;
    public Quaternion Rotation;
    public float SlopeAngle;
    public string InvalidReason;
}

/// <summary>
/// Dedicated zero-allocation ground detection and placement assistant for the SnackVendor.
/// Evaluates ground slope, player reach, multi-corner elevation, and obstacle collisions.
/// </summary>
public static class SnackVendorGroundAssistant
{
    private static readonly Vector3[] CornerOffsets = new Vector3[]
    {
        new Vector3(+1f, 0f, +1f),
        new Vector3(+1f, 0f, -1f),
        new Vector3(-1f, 0f, +1f),
        new Vector3(-1f, 0f, -1f)
    };

    private static readonly Collider[] OverlapResults = new Collider[32];

    // Obstacle layer mask
    private static readonly int ObstacleLayerMask = BuildObstacleMask();
    private static readonly int RayMask = BuildRayMask();

    private static int BuildObstacleMask()
    {
        int mask = 0;
        string[] layers = new[] {
            "Default", "Terrain", "Building", "Props",
            "Vehicle", "NPC", "Player", "Item",
            "Interactable", "Navigation", "NavigationRegion"
        };
        foreach (var layer in layers)
        {
            int idx = LayerMask.NameToLayer(layer);
            if (idx >= 0)
                mask |= 1 << idx;
        }
        return mask != 0 ? mask : ~0;
    }

    public static int BuildRayMask()
    {
        int excludeMask = 0;
        string[] layers = new[] { "Ignore Raycast", "Player" };
        foreach (var layer in layers)
        {
            int idx = LayerMask.NameToLayer(layer);
            if (idx >= 0)
                excludeMask |= 1 << idx;
        }
        return ~excludeMask;
    }

    /// <summary>
    /// Evaluates whether a SnackVendor can be placed at the given ground hit point.
    /// </summary>
    public static SnackVendorPlacementResult EvaluatePlacement(
        Vector3 rawHitPoint,
        Vector3 surfaceNormal,
        float rotationAngle,
        GameObject? ghostModel,
        Vector3 playerPos)
    {
        // 1. Grid Snapping (0.10m step)
        float snapStep = 0.10f;
        Vector3 targetPos = rawHitPoint;

        targetPos.x = Mathf.Round(rawHitPoint.x / snapStep) * snapStep;
        targetPos.z = Mathf.Round(rawHitPoint.z / snapStep) * snapStep;

        // Sample ground height at snapped coordinate
        Vector3 probe = new Vector3(targetPos.x, rawHitPoint.y + 1.8f, targetPos.z);
        if (Physics.Raycast(probe, Vector3.down, out RaycastHit gHit, 4.0f, RayMask, QueryTriggerInteraction.Ignore))
        {
            targetPos.y = gHit.point.y;
            surfaceNormal = gHit.normal;
        }
        else
        {
            targetPos.y = rawHitPoint.y;
        }

        // 2. Synchronized Rotation
        Quaternion targetRot = Quaternion.Euler(0f, rotationAngle, 0f);

        // 3. Multi-Corner Terrain Elevation (prevents machine from sinking into sloping curbs/terrain)
        // Machine footprint is ~ 0.95m wide x 0.72m deep x 1.85m tall
        Vector3 boxExtents = new Vector3(0.475f, 0.925f, 0.36f);
        Vector3 ext = new Vector3(boxExtents.x * 0.70f, 0f, boxExtents.z * 0.70f);

        float highestGroundY = targetPos.y;
        for (int c = 0; c < 4; c++)
        {
            Vector3 cPos = targetPos + targetRot * new Vector3(ext.x * CornerOffsets[c].x, 0f, ext.z * CornerOffsets[c].z);
            Vector3 cProbe = new Vector3(cPos.x, targetPos.y + 1.5f, cPos.z);

            if (Physics.Raycast(cProbe, Vector3.down, out RaycastHit cHit, 3.0f, RayMask, QueryTriggerInteraction.Ignore))
            {
                if (cHit.point.y > highestGroundY && (cHit.point.y - targetPos.y) <= 0.60f)
                {
                    highestGroundY = cHit.point.y;
                }
            }
        }

        targetPos.y = highestGroundY + 0.005f;

        // 4. Slope Validation (Walkable ground up to ~45 degrees)
        float slopeAngle = Vector3.Angle(surfaceNormal, Vector3.up);
        bool isWalkableSlope = slopeAngle <= 45f && Vector3.Dot(surfaceNormal, Vector3.up) >= 0.40f;

        // 5. Reach Distance Check (0.4m - 6.0m)
        float playerDist = Vector3.Distance(targetPos, playerPos);
        bool isWithinReach = playerDist >= 0.4f && playerDist <= 6.0f;

        // 6. Collision / Clearance Check
        bool isSpaceClear = true;
        Vector3 overlapCenter = targetPos + Vector3.up * (boxExtents.y + 0.10f);
        Vector3 overlapExtents = new Vector3(boxExtents.x * 0.85f, boxExtents.y * 0.80f, boxExtents.z * 0.85f);

        int hits = Physics.OverlapBoxNonAlloc(overlapCenter, overlapExtents, OverlapResults, targetRot, ObstacleLayerMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < hits; i++)
        {
            var col = OverlapResults[i];
            if (col == null || col.Pointer == IntPtr.Zero || col.WasCollected) continue;

            try
            {
                if (col.gameObject == null || col.gameObject.Pointer == IntPtr.Zero) continue;

                // Ignore ghost model's own colliders
                if (ghostModel != null && col.transform.IsChildOf(ghostModel.transform))
                    continue;

                // Ignore player collider
                if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
                    continue;

                // Ignore the ground surface we are standing on (threshold: 0.03m above feet)
                if (col.bounds.max.y <= targetPos.y + 0.03f)
                    continue;
            }
            catch { continue; }

            isSpaceClear = false;
            break;
        }

        bool isValid = isWalkableSlope && isWithinReach && isSpaceClear;
        string invalidReason = string.Empty;
        if (!isValid)
        {
            if (!isWalkableSlope) invalidReason = $"Slope too steep ({slopeAngle:F1}°)";
            else if (!isWithinReach) invalidReason = $"Out of reach ({playerDist:F1}m)";
            else if (!isSpaceClear) invalidReason = "Space blocked";
        }

        return new SnackVendorPlacementResult
        {
            IsValid = isValid,
            Position = targetPos,
            Rotation = targetRot,
            SlopeAngle = slopeAngle,
            InvalidReason = invalidReason
        };
    }
}
