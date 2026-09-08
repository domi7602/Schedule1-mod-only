using System;

namespace HomelessMod.Config;

/// <summary>
/// Configuration for the Homeless Mod ("Street Nomad").
/// </summary>
public class HomelessConfig
{
    /// <summary>
    /// Item ID for the sleeping bag item in the registry.
    /// </summary>
    public string SleepingBagItemId { get; set; } = "sleepingbag";

    /// <summary>
    /// If true, allows building/placing items anywhere in the world (streets, parks, alleys).
    /// </summary>
    public bool EnableEverywhereBuilding { get; set; } = true;

    /// <summary>
    /// If true, allows the player to sleep at any hour of the day in the sleeping bag (not just 18:00-06:00).
    /// </summary>
    public bool AllowAnytimeSleep { get; set; } = false;

    /// <summary>
    /// Grid snapping step in meters when placing items outdoors.
    /// </summary>
    public float OutdoorGridSnapSize { get; set; } = 0.5f;

    /// <summary>
    /// If true, automatically registers the "From Gutter to Kingpin" homeless quests on save load.
    /// </summary>
    public bool EnableHomelessQuests { get; set; } = true;

    /// <summary>
    /// If true, shows the compact interaction tooltip (Sleep / Pack Up) when hovering street items.
    /// </summary>
    public bool ShowInteractionHud { get; set; } = true;
}
