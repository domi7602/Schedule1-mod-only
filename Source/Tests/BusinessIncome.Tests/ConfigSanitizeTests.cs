using System.Collections.Generic;
using BusinessIncome.Config;
using Xunit;

namespace BusinessIncome.Tests;

// Vertical slice 1: BusinessIncomeConfig.Sanitize(warn) reports per-scalar warnings
// while the no-arg contract stays source-compatible.
public class ConfigSanitizeTests
{
    [Fact]
    public void Sanitize_reports_warning_for_out_of_range_payout_hour_and_resets_default()
    {
        var cfg = new BusinessIncomeConfig { PayoutHour = 99 };
        var warnings = new List<string>();

        cfg.Sanitize(warnings.Add);

        Assert.Equal(0, cfg.PayoutHour);
        Assert.Contains(warnings, w => w.Contains("PayoutHour"));
    }

    [Fact]
    public void Sanitize_reports_warning_for_nan_base_income_and_restores_default()
    {
        var cfg = new BusinessIncomeConfig { DefaultBaseIncome = float.NaN };
        var warnings = new List<string>();

        cfg.Sanitize(warnings.Add);

        Assert.Equal(500f, cfg.DefaultBaseIncome);
        Assert.Contains(warnings, w => w.Contains("DefaultBaseIncome"));
    }

    [Fact]
    public void Sanitize_without_callback_still_applies_existing_default_contract()
    {
        var cfg = new BusinessIncomeConfig
        {
            DefaultBaseIncome = float.PositiveInfinity,
            OperatingCostRate = -1f,
            EmployeeBonusPerWorker = float.NaN,
            MaxEmployeeBonus = 9f,
            WeekendBonusRate = -3f,
            MaxCatchupDays = 0,
        };

        cfg.Sanitize();

        Assert.Equal(500f, cfg.DefaultBaseIncome);
        Assert.Equal(0.10f, cfg.OperatingCostRate);
        Assert.Equal(0.05f, cfg.EmployeeBonusPerWorker);
        Assert.Equal(0.25f, cfg.MaxEmployeeBonus);
        Assert.Equal(0.25f, cfg.WeekendBonusRate);
        Assert.Equal(7, cfg.MaxCatchupDays);
    }
}
