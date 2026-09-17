using System;
using UnityEngine;

namespace ProfitTracker;

public class ProfitTrackerConfig
{
    public bool Visible { get; set; } = true;
    public KeyCode ToggleHotkey { get; set; } = KeyCode.F8;
    public string Anchor { get; set; } = "TopRight"; // "TopRight", "TopLeft", "BottomLeft", "BottomRight"
    public float PositionX { get; set; } = -20f;
    public float PositionY { get; set; } = -95f;
    public float Scale { get; set; } = 1.0f;
    public float BackgroundOpacity { get; set; } = 0.75f;
    public bool TrackInventoryNetWorth { get; set; } = true; // true = Total Net Worth (Betrieb + Ware), false = Liquid Only
    public bool ShowSessionRow { get; set; } = false; // optional third row for session profit
    public bool AllowDragging { get; set; } = true;
}
