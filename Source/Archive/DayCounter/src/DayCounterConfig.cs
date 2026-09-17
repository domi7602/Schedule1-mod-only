using System;
using UnityEngine;

namespace DayCounter;

public class DayCounterConfig
{
    public bool Visible { get; set; } = true;
    public KeyCode ToggleHotkey { get; set; } = KeyCode.F7;
    public string Anchor { get; set; } = "TopLeft"; // "TopLeft", "TopRight", "BottomLeft", "BottomRight"
    public float PositionX { get; set; } = 24f;
    public float PositionY { get; set; } = -24f;
    public float Scale { get; set; } = 1.0f;
    public float BackgroundOpacity { get; set; } = 0.75f;
    public bool CompactMode { get; set; } = true; // true = 1-row compact text
    public bool TimeFormat24H { get; set; } = true; // true = 15:30, false = 3:30 PM
    public bool ShortDayNames { get; set; } = false; // false = Wednesday, true = Wed
    public bool ShowDayNumber { get; set; } = true;
    public bool ShowDayName { get; set; } = true;
    public bool ShowTime { get; set; } = false; // default off — compact text only
    public bool ShowAccentStrip { get; set; } = true;
    public bool AllowDragging { get; set; } = true;
    public bool AutoHideInMenus { get; set; } = false;
}
