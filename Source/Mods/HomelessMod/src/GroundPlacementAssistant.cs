using System;
using Il2CppScheduleOne.PlayerScripts;
using UnityEngine;

namespace HomelessMod.Building;

/// <summary>
/// Result structure containing validated position, rotation, and placement status.
/// </summary>
public struct GroundPlacementResult
{
    public bool IsValid;
    public Vector3 Position;
    public Quaternion Rotation;
    public float SlopeAngle;
    public bool IsFreePlacement;
    public string InvalidReason;
}

/// <summary>
/// Bulletproof, Zero-Allocation ground detector and placement assistant.
/// Uses direct stack-allocated Raycasts (100% IL2CPP safe).
/// </summary>
public static class GroundPlacementAssistant
{
    private static readonly Vector3[] CornerOffsets = new Vector3[]
    {
        new Vector3(+1f, 0f, +1f),
        new Vector3(+1f, 0f, -1f),
        new Vector3(-1f, 0f, +1f),
        new Vector3(-1f, 0f, -1f)
    };

    private static readonly Collider[] OverlapResults = new Collider[32];

    // Gatekeeper-fix 2026-08-30 H2: throw-free mask builder — GetMask throws ArgumentException if any layer missing.
    private static readonly int ObstacleLayerMask = BuildObstacleMask();

    private static int BuildObstacleMask()
    {
        int mask = 0;
        string[] layers = new[] { "Default", "Terrain", "Building", "Props" };
        foreach (var layer in layers)
        {
            int idx = LayerMask.NameToLayer(layer);
            if (idx >= 0)
                mask |= 1 << idx;
            else
                MelonLoader.MelonLogger.Warning($"[HomelessMod] Layer '{layer}' not found — skipped from ObstacleLayerMask.");
        }
        return mask != 0 ? mask : ~0;
    }

    // [GroundFix v0.1.2] Ray mask for ground snapping — everything except Ignore Raycast & Player.
    private static readonly int SnapRayMask = BuildSnapRayMask();

    private static int BuildSnapRayMask()
    {
        int excludeMask = 0;
        string[] layers = new[] { "Ignore Raycast", "Player" };
        foreach (var layer in layers)
        {
            int idx = LayerMask.NameToLayer(layer);
            if (idx >= 0)
                excludeMask |= 1 << idx;
            else
                MelonLoader.MelonLogger.Warning($"[HomelessMod] Layer '{layer}' not found — not excluded from SnapRayMask.");
        }
        return ~excludeMask;
    }

    /// <summary>
    /// [GroundFix v0.1.2] Snaps a world position onto the ground surface directly below it.
    /// A short headroom probe (0.5m above the saved Y) means items resting correctly on a
    /// roof/bridge stay there, while items saved floating (old bed-verticalOffset bug, ~1.5m)
    /// drop onto the ground. Returns the original position if no ground is found within range.
    /// </summary>
    public static Vector3 SnapToGround(Vector3 pos, float headroom = 0.5f, float maxDrop = 3.0f)
    {
        Vector3 probe = new Vector3(pos.x, pos.y + headroom, pos.z);
        if (Physics.Raycast(probe, Vector3.down, out RaycastHit hit, headroom + maxDrop, SnapRayMask, QueryTriggerInteraction.Ignore))
        {
            return new Vector3(pos.x, hit.point.y, pos.z);
        }
        return pos;
    }

    public static GroundPlacementResult EvaluatePlacement(
        Vector3 rawHitPoint,
        Vector3 surfaceNormal,
        float rotationAngle,
        Vector3 boxExtents,
        float verticalOffset,
        float snapGridSize,
        bool isFreePlacementHeld,
        GameObject? ghostModel,
        Vector3 playerPos,
        int groundLayerMask)
    {
        // 1. Dual-Grid Snapping: 0.05m with Shift (Free Placement), otherwise configurable snap
        float snapStep = isFreePlacementHeld ? 0.05f : Mathf.Max(0.05f, snapGridSize);
        Vector3 targetPos = rawHitPoint;

        if (snapStep > 0.02f)
        {
            targetPos.x = Mathf.Round(rawHitPoint.x / snapStep) * snapStep;
            targetPos.z = Mathf.Round(rawHitPoint.z / snapStep) * snapStep;

            // Sample ground height at snapped coordinate — fallback to raw hit if snapped probe misses (H7)
            Vector3 probe = new Vector3(targetPos.x, rawHitPoint.y + 1.8f, targetPos.z);
            if (Physics.Raycast(probe, Vector3.down, out RaycastHit gHit, 4.0f, groundLayerMask, QueryTriggerInteraction.Ignore))
            {
                targetPos.y = gHit.point.y;
                surfaceNormal = gHit.normal;
            }
            else
            {
                targetPos.y = rawHitPoint.y;
            }
        }

        // 2. Synchronized Rotation
        Quaternion targetRot = Quaternion.Euler(0f, rotationAngle, 0f);

        // 3. Multi-Corner Terrain Elevation (prevents object corners from sinking on slopes/curbs)
        Vector3 ext = Vector3.Max(boxExtents * 0.70f, new Vector3(0.15f, 0.10f, 0.15f));

        float highestGroundY = targetPos.y;
        for (int c = 0; c < 4; c++)
        {
            Vector3 cPos = targetPos + targetRot * new Vector3(ext.x * CornerOffsets[c].x, 0f, ext.z * CornerOffsets[c].z);
            Vector3 cProbe = new Vector3(cPos.x, targetPos.y + 1.5f, cPos.z);

            if (Physics.Raycast(cProbe, Vector3.down, out RaycastHit cHit, 3.0f, groundLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (cHit.point.y > highestGroundY && (cHit.point.y - targetPos.y) <= 0.60f)
                {
                    highestGroundY = cHit.point.y;
                }
            }
        }

        // Apply auto-elevation and vertical offset
        targetPos.y = highestGroundY + verticalOffset + 0.005f;

        // 4. Slope Validation (Walkable ground up to ~45 degrees)
        float slopeAngle = Vector3.Angle(surfaceNormal, Vector3.up);
        bool isWalkableSlope = slopeAngle <= 45f && Vector3.Dot(surfaceNormal, Vector3.up) >= 0.40f;

        // 5. Reach Distance Check
        // The camera is usually slightly behind or at the player's head.
        // We use playerPos (which is now camTransform.position) for distance.
        float playerDist = Vector3.Distance(targetPos, playerPos);
        bool isWithinReach = playerDist >= 0.3f && playerDist <= 7.0f;

        // 6. Collision / Intersection Check
        bool isSpaceClear = true;
        // Elevate center to ignore the floor, shrink extents slightly for forgiving placement
        Vector3 overlapCenter = targetPos + Vector3.up * (boxExtents.y + 0.15f);
        Vector3 overlapExtents = new Vector3(boxExtents.x * 0.85f, Mathf.Max(0.05f, boxExtents.y * 0.8f), boxExtents.z * 0.85f);

        int hits = Physics.OverlapBoxNonAlloc(overlapCenter, overlapExtents, OverlapResults, targetRot, ObstacleLayerMask, QueryTriggerInteraction.Ignore);

        // Gatekeeper-fix 2026-08-30 L8: warn if buffer overflow (silently capped at 32).
        if (hits == OverlapResults.Length)
            MelonLoader.MelonLogger.Warning($"[HomelessMod] OverlapBoxNonAlloc buffer full ({hits}/{OverlapResults.Length}) — some colliders may be missed. Placement validity may be inaccurate.");

        for (int i = 0; i < hits; i++)
        {
            var col = OverlapResults[i];
            if (col == null || col.Pointer == IntPtr.Zero || col.WasCollected) continue;

            // In Il2Cpp, destroyed objects might still exist as C# wrappers but have Pointer == IntPtr.Zero
            try
            {
                if (col.gameObject == null || col.gameObject.Pointer == IntPtr.Zero) continue;

                // Ignore the ghost model's own colliders
                if (ghostModel != null && col.transform.IsChildOf(ghostModel.transform))
                    continue;

                // Ignore the player's collider (usually on the "Player" layer, but just in case)
                if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
                    continue;

                // Ignore the ground we are placing on (anything that doesn't reach higher than our feet) — 0.02m threshold prevents curb false-positive (H5)
                if (col.bounds.max.y <= targetPos.y + 0.02f)
                    continue;
            }
            catch { continue; } // Skip if accessing native properties throws (e.g. destroyed object)

            // Hit a valid obstacle
            isSpaceClear = false;
            break;
        }

        bool isValid = isWalkableSlope && isWithinReach && isSpaceClear;
        string invalidReason = string.Empty;

        if (!isValid)
        {
            if (!isWalkableSlope)
            {
                invalidReason = $"Slope too steep (Angle: {slopeAngle:F1}°)";
            }
            else if (!isWithinReach)
            {
                invalidReason = $"Out of reach (Distance: {playerDist:F1}m)";
            }
            else if (!isSpaceClear)
            {
                invalidReason = "Blocked by obstacle: ";
                for (int i = 0; i < hits; i++)
                {
                    var col = OverlapResults[i];
                    if (col == null || col.Pointer == IntPtr.Zero || col.WasCollected) continue;

                    try
                    {
                        if (col.gameObject == null || col.gameObject.Pointer == IntPtr.Zero) continue;
                        if (ghostModel != null && col.transform.IsChildOf(ghostModel.transform)) continue;
                        if (col.gameObject.layer == LayerMask.NameToLayer("Player")) continue;
                        if (col.bounds.max.y <= targetPos.y + 0.02f) continue;

                        invalidReason += $"{col.name} (Layer: {LayerMask.LayerToName(col.gameObject.layer)})";
                        break;
                    }
                    catch { continue; }
                }
            }
        }

        // Clear references to avoid memory leaks
        Array.Clear(OverlapResults, 0, hits);

        return new GroundPlacementResult
        {
            IsValid = isValid,
            Position = targetPos,
            Rotation = targetRot,
            SlopeAngle = slopeAngle,
            IsFreePlacement = isFreePlacementHeld,
            InvalidReason = invalidReason
        };
    }
}
