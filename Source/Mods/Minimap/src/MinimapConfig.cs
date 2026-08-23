using System;
using UnityEngine;

namespace Minimap;

public enum MinimapShape
{
    Circle = 0,
    RoundedSquare = 1,
    Square = 2
}

public enum MinimapRotation
{
    FollowPlayer = 0,
    NorthUp = 1
}

public enum DayCounterMode
{
    Header = 0,
    Footer = 1,
    Standalone = 2,
    Hidden = 3
}

public enum MinimapAnchor
{
    TopRight = 0,
    TopLeft = 1,
    BottomRight = 2,
    BottomLeft = 3
}

[Serializable]
public sealed class MinimapConfig
{
    // Layout & Appearance (Default: RoundedSquare Tactical GPS)
    public MinimapShape Shape { get; set; } = MinimapShape.RoundedSquare;
    public MinimapRotation RotationMode { get; set; } = MinimapRotation.FollowPlayer;
    public DayCounterMode DayMode { get; set; } = DayCounterMode.Header;
    public MinimapAnchor AnchorPosition { get; set; } = MinimapAnchor.TopRight;

    public float MapSize { get; set; } = 220f;
    public float ZoomLevel { get; set; } = 1.75f;
    public float BackgroundOpacity { get; set; } = 0.85f;
    public string BorderColorHex { get; set; } = "#3BD882"; // Emerald accent
    public bool ShowCompassRing { get; set; } = false; // Only on Circle
    public bool ShowPlayerArrow { get; set; } = true;

    // Performance & Range Filtering
    public float MaxEntityRange { get; set; } = 140f; // Max detection range in meters
    public bool ClampOnlyCritical { get; set; } = true; // Only clamp Deals, Quests, Properties
    public bool EnableEdgeClamping { get; set; } = true;

    // Blip Category Filters
    public bool ShowPoliceBlips { get; set; } = true;
    public bool ShowActiveDeals { get; set; } = true;
    public bool ShowPotentialCustomers { get; set; } = true;
    public bool ShowKnownCustomers { get; set; } = true;
    public bool ShowPropertyBlips { get; set; } = true;
    public bool ShowShopBlips { get; set; } = true;
    public bool ShowQuestBlips { get; set; } = true;
    public bool ShowVehicleBlips { get; set; } = true;

    // DayCounter Options
    public bool Time24H { get; set; } = true;
    public bool ShowDayNumber { get; set; } = true;
    public bool ShowDayName { get; set; } = true;
    public bool ShowTime { get; set; } = true;
    public bool ShortDayName { get; set; } = false;

    // Drag & Drop
    public bool AllowDragging { get; set; } = true;
    public bool HasCustomPosition { get; set; } = false;
    public float CustomPosX { get; set; } = 0f;
    public float CustomPosY { get; set; } = 0f;

    // Hotkeys & General
    public bool MinimapVisible { get; set; } = true;
    public string ToggleKey { get; set; } = "M";
    public string ZoomInKey { get; set; } = "RightBracket";
    public string ZoomOutKey { get; set; } = "LeftBracket";
}
