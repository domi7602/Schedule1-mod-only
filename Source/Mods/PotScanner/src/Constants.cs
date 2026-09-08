namespace PotScanner.Utils;

public static class Constants
{
    public const string ModName = "PotScanner";
    public const string ModVersion = "0.5.0";
    public const string ModAuthor = "Dominik";
    public const float HudRangeMeters = 30f;
    public const float PotRefreshIntervalSec = 2f;
    public const string GameplaySceneName = "Main";
    public const string UnknownPropertyCode = "unknown";
    /// <summary>Cost per pot for the Water-All feature (v0.2.0), in cash. Matches MoneyManager.cashBalance (float).</summary>
    public const float WaterAllCostPerPot = 50f;
    /// <summary>v0.2.2: pots with WaterPercent &gt;= this are skipped (not watered, not charged). 0.30 = 30% (very dry threshold).</summary>
    public const float WaterAllSkipThreshold = 0.30f;
}
