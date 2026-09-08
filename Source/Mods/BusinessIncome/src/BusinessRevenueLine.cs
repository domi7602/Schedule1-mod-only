namespace BusinessIncome.Models;

/// <summary>
/// Detailed billing line for a single business on a specific day.
/// </summary>
public sealed class BusinessRevenueLine
{
    public string BusinessId { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public float BaseIncome { get; set; }
    public float Multiplier { get; set; } = 1.0f;
    public float VarianceFactor { get; set; } = 1.0f;
    public int EmployeeCount { get; set; }
    public float EmployeeBonusPercent { get; set; }
    public bool IsWeekendBonusApplied { get; set; }
    public float WeekendBonusPercent { get; set; }
    public float GrossRevenue { get; set; }
    public float OperatingCosts { get; set; }
    public float NetRevenue { get; set; }
}
