using System;
using System.Collections.Generic;

namespace TownHallInterior;

// Shared entrance geometry and motion policy; native mutation remains in the runtime host.
public static class PublicEntrance
{
    public const float SlideSeconds = 0.75f;
    public const float SensorRadius = 1.2f;
    // Shared visual/collision opening height; a design coordinate, not a player measurement.
    public const float PortalTop = 4.55f;
    public const float LeafHeight = 3.2f;
    public const float LeafCenterY = PortalTop - LeafHeight / 2;
    // The visible head lining overlaps the leaf top by one centimetre, so no upward sight line
    // survives between the lintel and the leaf plane; the clear height stays ~2.95 m.
    public const float HeadLiningDrop = 0.01f;
    public const float HeadLiningBottom = PortalTop - HeadLiningDrop;
    public const float LintelHeight = RoomPlan.Upper - HeadLiningBottom;
    // One centimetre in front of the leaf plane (54.17): the linings bury the leaf edges.
    public const float LiningFrontX = 54.16f;
    // These are owned geometry coordinates, NOT measured native floor/player heights.
    public const float FloorCenterY = 1.48f, FloorThickness = 0.1f;
    public const float FloorTop = FloorCenterY + FloorThickness / 2;

    // Sliding leaves. The reveal linings close the opening at z 30.02 / 31.98
    // (FurniturePlan "reveal-south"/"reveal-north"); each panel reaches LeafOverlap past its
    // lining and meets its partner exactly at the portal centre. Both edges therefore end on
    // solid geometry and no straight sight line into the exterior survives.
    public const float LeafCenterX = 54.12f;
    public const float LeafThickness = 0.06f;
    public const float RevealSouthZ = 30.02f, RevealNorthZ = 31.98f;
    public const float LeafOverlap = 0.02f;
    public const float LeafWidth = (RevealNorthZ - RevealSouthZ) / 2f + LeafOverlap;
    public const float LeafTravel = 1.05f;
    // Closed centres place the panel edges at 30.00/31.00 and 31.00/32.00.
    public const float LeafClosedSouthZ = RevealSouthZ - LeafOverlap + LeafWidth / 2f;
    public const float LeafClosedNorthZ = RevealNorthZ + LeafOverlap - LeafWidth / 2f;

    public static float LeafSouthZ(float open) => LeafClosedSouthZ - open * LeafTravel;
    public static float LeafNorthZ(float open) => LeafClosedNorthZ + open * LeafTravel;
    public static float LeafMinZ(float open) => LeafSouthZ(open) - LeafWidth / 2f;
    public static float LeafMaxZ(float open) => LeafNorthZ(open) + LeafWidth / 2f;

    // Closed leaves must cover the reveal on both sides; open leaves must leave it clear.
    public static bool ClosedLeavesSealReveal() =>
        LeafMinZ(0f) <= RevealSouthZ && LeafMaxZ(0f) >= RevealNorthZ &&
        LeafSouthZ(0f) + LeafWidth / 2f >= LeafNorthZ(0f) - LeafWidth / 2f;

    public static bool OpenLeavesClearReveal() =>
        LeafSouthZ(1f) + LeafWidth / 2f <= RevealSouthZ &&
        LeafNorthZ(1f) - LeafWidth / 2f >= RevealNorthZ;

    /// One visible part of a sliding leaf, relative to the leaf transform. Rails are 1 mm
    /// thinner than the stiles, and stiles/panel stop below the rail top, so no two visible
    /// faces share a plane; coplanar faces flickered on the black frame.
    /// No part is solid: the leaves are the visible replacement of the hidden native door, and
    /// the proximity sensor only knows the local player, so an NPC resident would otherwise be
    /// blocked by a closed leaf (owner report 2026-10-29: Jen's StayInBuilding tick threw).
    public sealed record LeafPart(string Name, float OffsetY, float OffsetZ,
        float ThicknessX, float Height, float WidthZ, bool Solid);

    public static readonly IReadOnlyList<LeafPart> LeafParts = new[]
    {
        new LeafPart("panel", 0f, 0f,
            0.06f, LeafHeight - 0.04f, LeafWidth - 0.14f + 0.02f, false),
        new LeafPart("stile-south", 0f, -(LeafWidth - 0.07f) / 2f,
            0.10f, LeafHeight - 0.02f, 0.07f, false),
        new LeafPart("stile-north", 0f, (LeafWidth - 0.07f) / 2f,
            0.10f, LeafHeight - 0.02f, 0.07f, false),
        new LeafPart("rail-top", (LeafHeight - 0.07f) / 2f, 0f, 0.09f, 0.07f, LeafWidth - 0.004f, false),
        new LeafPart("rail-bottom", -(LeafHeight - 0.07f) / 2f, 0f, 0.09f, 0.07f, LeafWidth - 0.004f, false),
    };

    public static IReadOnlyList<RoomBox> CreateInterior()
    {
        var boxes = new List<RoomBox>();
        boxes.Add(new RoomBox("public-floor", 58.7f, FloorCenterY, 31f, 7.4f, FloorThickness, 18.8f, true, 0));
        // Finishes the unfloored wall recess under the doorway: same footprint as the reveal
        // linings, top exactly at FloorTop, underside buried below them. Leaves stop at x 54.16.
        boxes.Add(new RoomBox("public-threshold", 54.6455f, 1.445f, 31f, 0.611f, 0.17f, 1.96f, true, 0));
        boxes.Add(new RoomBox("inner-west-south", 54.85f, 2.85f, 25.6f, 0.2f, 2.7f, 8.8f, true, 1));
        boxes.Add(new RoomBox("inner-west-north", 54.85f, 2.85f, 36.4f, 0.2f, 2.7f, 8.8f, true, 1));
        // Runs from the facade plane to the inner skin: a native facade cut is rasterised
        // along triangle edges and overshoots, so a lining that stops short leaves an
        // upward sight line over the leaves.
        boxes.Add(new RoomBox("inner-lintel", (LiningFrontX + 54.95f) / 2f, (HeadLiningBottom + RoomPlan.Upper) / 2f, 31f,
            54.95f - LiningFrontX, LintelHeight, 1.92f, true, 1));
        boxes.Add(new RoomBox("inner-east", 62.55f, 2.85f, 31f, 0.2f, 2.7f, 18.8f, true, 1));
        boxes.Add(new RoomBox("inner-south", 58.7f, 2.85f, 21.5f, 7.4f, 2.7f, 0.2f, true, 1));
        boxes.Add(new RoomBox("inner-north", 58.7f, 2.85f, 40.5f, 7.4f, 2.7f, 0.2f, true, 1));
        return boxes;
    }
    public static readonly string[] DisableColliders =
    {
        NativeSceneSwap.BuildingPath + "/townhall/Walls",
        NativeSceneSwap.BuildingPath + "/Cube",
        NativeSceneSwap.BuildingPath + "/townhall/Door/Quad",
        NativeSceneSwap.BuildingPath + "/townhall/UpperBrickTrim",
    };
    public static readonly string[] PreserveColliders =
    {
        NativeSceneSwap.BuildingPath + "/townhall/UpperBrickTrim",
        NativeSceneSwap.BuildingPath + "/townhall/LowerBrickTrim",
        NativeSceneSwap.BuildingPath + "/StaticDoor",
    };

    public static bool ColliderSwapAllowed(NativeSceneInspection inspection) =>
        inspection.IdentityAndGeometryMatch && !inspection.CanPublish;

    public static bool WantsDoorOpen(float x, float y, float z)
    {
        // Keep both approaches and the occupied threshold, not the entire public hall.
        return x >= 51.6f && x <= 56.4f && z >= 29.2f && z <= 32.8f && y >= 0.8f && y <= 4.2f;
    }

    public static float MoveLeaf(float current, bool occupied, float deltaSeconds)
    {
        if (float.IsNaN(current) || float.IsInfinity(current)) current = 0f;
        var target = occupied ? 1f : 0f;
        var step = Math.Max(0f, deltaSeconds) / SlideSeconds;
        if (current < target) return Math.Min(target, current + step);
        return Math.Max(target, current - step);
    }
}
