using System;
using System.Collections.Generic;

namespace BusinessIncome.Config;

/// <summary>
/// Configuration for the BusinessIncome mod (persisted via MelonPreferences / JSON).
/// </summary>
public sealed class BusinessIncomeConfig
{
    /// <summary>
    /// Time of payout (0 = midnight / day transition; 1-23 = configured in-game hour).
    /// </summary>
    public int PayoutHour { get; set; } = 0;

    /// <summary>
    /// Base income per business and day in dollars before multipliers.
    /// </summary>
    public float DefaultBaseIncome { get; set; } = 500.0f;

    /// <summary>
    /// Operating cost rate of gross revenue (e.g. 0.10 = 10%).
    /// </summary>
    public float OperatingCostRate { get; set; } = 0.10f;

    /// <summary>
    /// Bonus per assigned employee (e.g. 0.05 = +5%).
    /// </summary>
    public float EmployeeBonusPerWorker { get; set; } = 0.05f;

    /// <summary>
    /// Maximum cumulative employee bonus (e.g. 0.25 = +25%).
    /// </summary>
    public float MaxEmployeeBonus { get; set; } = 0.25f;

    /// <summary>
    /// Weekend bonus for weekend-relevant businesses (e.g. 0.25 = +25%).
    /// </summary>
    public float WeekendBonusRate { get; set; } = 0.25f;

    /// <summary>
    /// Enables or disables in-game HUD notifications on payout.
    /// </summary>
    public bool EnableNotifications { get; set; } = true;

    /// <summary>
    /// Plays the register chime sound on payout.
    /// </summary>
    public bool PlayCashSound { get; set; } = true;

    /// <summary>
    /// Multipliers per business ID / key.
    /// </summary>
    public Dictionary<string, float> PropertyMultipliers { get; set; } = new(StringComparer.OrdinalIgnoreCase)
    {
        ["laundromat"] = 1.00f,
        ["car_wash"] = 1.25f,
        ["taco_ticklers"] = 0.90f,
        ["dispensary"] = 1.50f,
        ["nightclub"] = 2.00f,
        ["bar"] = 1.20f,
        ["coffee_shop"] = 0.85f,
        ["convenience_store"] = 0.95f
    };

    /// <summary>
    /// Custom display names for business IDs.
    /// </summary>
    public Dictionary<string, string> DisplayNameOverrides { get; set; } = new(StringComparer.OrdinalIgnoreCase)
    {
        ["laundromat"] = "Laundromat",
        ["car_wash"] = "Car Wash",
        ["taco_ticklers"] = "Taco Ticklers",
        ["dispensary"] = "Dispensary",
        ["nightclub"] = "Nightclub",
        ["bar"] = "Downtown Bar",
        ["coffee_shop"] = "Coffee Shop",
        ["convenience_store"] = "Convenience Store"
    };

    /// <summary>
    /// List of business IDs that benefit from the weekend bonus.
    /// </summary>
    public List<string> WeekendBonusCategories { get; set; } = new()
    {
        "nightclub",
        "bar",
        "taco_ticklers",
        "coffee_shop"
    };

    /// <summary>
    /// Cleans and validates inputs to safe bounds.
    /// </summary>
    public void Sanitize()
    {
        if (PayoutHour < 0 || PayoutHour > 23)
            PayoutHour = 0;

        if (DefaultBaseIncome < 0f)
            DefaultBaseIncome = 500f;

        if (OperatingCostRate < 0f || OperatingCostRate > 1f)
            OperatingCostRate = 0.10f;

        if (EmployeeBonusPerWorker < 0f)
            EmployeeBonusPerWorker = 0.05f;

        if (MaxEmployeeBonus < 0f)
            MaxEmployeeBonus = 0.25f;

        if (WeekendBonusRate < 0f)
            WeekendBonusRate = 0.25f;
    }
}
