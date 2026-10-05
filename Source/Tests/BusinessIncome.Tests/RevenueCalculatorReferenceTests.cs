using System;
using BusinessIncome.Config;
using BusinessIncome.Models;
using BusinessIncome.Services;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>
/// Reference/characterization tests for the pure RevenueCalculator.
///
/// Every expected value below was captured from the ACTUAL pre-extraction
/// calculator (current RevenueCalculator.cs linked against a pure stand-in
/// ResolvedBusiness, 2026-10-05) and is asserted at the BIT level
/// (BitConverter.SingleToInt32Bits) so the refactor to the pure BusinessData
/// model must reproduce the old float rounding and the seeded Random
/// (hash -> new Random(hash) -> NextDouble) EXACTLY. Nothing here depends on
/// Unity, IL2CPP, S1API or the runtime resolver.
///
/// The calculator consumes isWeekend as a plain bool; the elapsedDays -> day
/// parity ("negative day weekend helper") stays in the finance planner and is
/// deliberately NOT exercised here.
/// </summary>
public class RevenueCalculatorReferenceTests
{
    private static BusinessData Biz(string id, string name, int employees) =>
        new BusinessData { Id = id, DisplayName = name, EmployeeCount = employees };

    private static BusinessIncomeConfig Default()
    {
        var cfg = new BusinessIncomeConfig();
        cfg.Sanitize(); // boundary caller duty (engine), not the calculator's
        return cfg;
    }

    /// <summary>Bit-exact float assert: catches any 1-ULP drift from float rounding.</summary>
    private static void AssertBits(float expected, float actual, string because)
    {
        int e = BitConverter.SingleToInt32Bits(expected);
        int a = BitConverter.SingleToInt32Bits(actual);
        Assert.True(e == a,
            $"{because}: expected {expected:R} (bits {e}) but got {actual:R} (bits {a})");
    }

    [Fact]
    public void A_CarWash_Day10_Weekday_ThreeEmployees()
    {
        var line = RevenueCalculator.Calculate(Biz("car_wash", "Car Wash", 3), 10, false, Default());

        AssertBits(500f, line.BaseIncome, "base");
        AssertBits(1.25f, line.Multiplier, "multiplier");
        AssertBits(1.1277f, line.VarianceFactor, "variance");
        Assert.Equal(3, line.EmployeeCount);
        AssertBits(0.15f, line.EmployeeBonusPercent, "employee bonus");
        Assert.False(line.IsWeekendBonusApplied);
        AssertBits(0f, line.WeekendBonusPercent, "weekend pct");
        AssertBits(810.53f, line.GrossRevenue, "gross");
        AssertBits(81.05f, line.OperatingCosts, "costs");
        AssertBits(729.48f, line.NetRevenue, "net");
    }

    [Fact]
    public void B_Nightclub_Day12_Weekend_FiveEmployees()
    {
        var line = RevenueCalculator.Calculate(Biz("nightclub", "Nightclub", 5), 12, true, Default());

        AssertBits(2f, line.Multiplier, "multiplier");
        AssertBits(1.1346f, line.VarianceFactor, "variance");
        AssertBits(0.25f, line.EmployeeBonusPercent, "employee bonus (5 * 0.05 = cap)");
        Assert.True(line.IsWeekendBonusApplied);
        AssertBits(0.25f, line.WeekendBonusPercent, "weekend pct");
        AssertBits(1772.81f, line.GrossRevenue, "gross");
        AssertBits(177.28f, line.OperatingCosts, "costs");
        AssertBits(1595.53f, line.NetRevenue, "net");
    }

    [Fact]
    public void C_Bar_Day7_Weekend_TenEmployees_BonusCapped()
    {
        var line = RevenueCalculator.Calculate(Biz("bar", "Downtown Bar", 10), 7, true, Default());

        AssertBits(1.2f, line.Multiplier, "multiplier");
        AssertBits(1.1262f, line.VarianceFactor, "variance");
        Assert.Equal(10, line.EmployeeCount);
        AssertBits(0.25f, line.EmployeeBonusPercent, "employee bonus clamped to MaxEmployeeBonus");
        AssertBits(1055.81f, line.GrossRevenue, "gross");
        AssertBits(105.58f, line.OperatingCosts, "costs");
        AssertBits(950.23f, line.NetRevenue, "net");
    }

    [Fact]
    public void D_ZeroBaseIncome_AllMoneyZero_ButBonusesStillComputed()
    {
        var cfg = new BusinessIncomeConfig { DefaultBaseIncome = 0f };
        cfg.Sanitize();
        var line = RevenueCalculator.Calculate(Biz("car_wash", "Car Wash", 3), 10, false, cfg);

        AssertBits(0f, line.BaseIncome, "base");
        AssertBits(1.1277f, line.VarianceFactor, "variance");
        AssertBits(0.15f, line.EmployeeBonusPercent, "employee bonus");
        AssertBits(0f, line.GrossRevenue, "gross");
        AssertBits(0f, line.OperatingCosts, "costs");
        AssertBits(0f, line.NetRevenue, "net");
    }

    [Fact]
    public void E_ZeroOperatingCost_CostsZero_NetEqualsGross()
    {
        var cfg = new BusinessIncomeConfig { OperatingCostRate = 0f };
        cfg.Sanitize();
        var line = RevenueCalculator.Calculate(Biz("dispensary", "Dispensary", 2), 3, false, cfg);

        AssertBits(1.5f, line.Multiplier, "multiplier");
        AssertBits(0.9443f, line.VarianceFactor, "variance");
        AssertBits(0.1f, line.EmployeeBonusPercent, "employee bonus");
        AssertBits(779.05f, line.GrossRevenue, "gross");
        AssertBits(0f, line.OperatingCosts, "costs");
        AssertBits(779.05f, line.NetRevenue, "net == gross");
    }

    [Fact]
    public void F_NegativeElapsedDay_DeterministicVariance_ReusedExactly()
    {
        // The calculator must not reject/repair negative days: variance is a pure
        // function of (day, id) and stays deterministic for negative input.
        var line = RevenueCalculator.Calculate(Biz("laundromat", "Laundromat", 0), -3, false, Default());

        AssertBits(1.0551f, line.VarianceFactor, "variance");
        AssertBits(1f, line.Multiplier, "multiplier");
        AssertBits(527.55f, line.GrossRevenue, "gross");
        AssertBits(52.76f, line.OperatingCosts, "costs");
        AssertBits(474.79f, line.NetRevenue, "net");
    }

    [Fact]
    public void G_UnknownId_DefaultMultiplier_NoWeekendCategory()
    {
        var line = RevenueCalculator.Calculate(Biz("unknown_biz", "Mystery", 1), 1, true, Default());

        AssertBits(1f, line.Multiplier, "unknown id -> multiplier 1.0");
        AssertBits(0.8787f, line.VarianceFactor, "variance");
        AssertBits(0.05f, line.EmployeeBonusPercent, "employee bonus");
        Assert.False(line.IsWeekendBonusApplied); // not in WeekendBonusCategories
        AssertBits(0f, line.WeekendBonusPercent, "weekend pct");
        AssertBits(461.32f, line.GrossRevenue, "gross");
        AssertBits(46.13f, line.OperatingCosts, "costs");
        AssertBits(415.19f, line.NetRevenue, "net");
    }

    [Fact]
    public void H_EmptyId_NoCrash_DeterministicVariance()
    {
        var line = RevenueCalculator.Calculate(Biz("", "", 0), 0, false, Default());

        AssertBits(1f, line.Multiplier, "multiplier");
        AssertBits(0.8633f, line.VarianceFactor, "empty id variance");
        AssertBits(431.65f, line.GrossRevenue, "gross");
        AssertBits(43.16f, line.OperatingCosts, "costs");
        AssertBits(388.49f, line.NetRevenue, "net");
    }

    [Fact]
    public void I_ExtremeBase100k_FullCostRate_NetFloorsAtZero()
    {
        var cfg = new BusinessIncomeConfig { DefaultBaseIncome = 100000f, OperatingCostRate = 1f };
        cfg.Sanitize();
        var line = RevenueCalculator.Calculate(Biz("nightclub", "Nightclub", 10), 100, true, cfg);

        AssertBits(100000f, line.BaseIncome, "base");
        AssertBits(2f, line.Multiplier, "multiplier");
        AssertBits(1.1104f, line.VarianceFactor, "variance");
        AssertBits(0.25f, line.EmployeeBonusPercent, "employee bonus");
        AssertBits(347000f, line.GrossRevenue, "gross");
        AssertBits(347000f, line.OperatingCosts, "costs");
        AssertBits(0f, line.NetRevenue, "net floored at 0");
    }

    [Fact]
    public void J_CostEqualsGross_NetFloorsAtZero()
    {
        var cfg = new BusinessIncomeConfig { OperatingCostRate = 1f };
        cfg.Sanitize();
        var line = RevenueCalculator.Calculate(Biz("coffee_shop", "Coffee Shop", 4), 5, true, cfg);

        AssertBits(0.85f, line.Multiplier, "multiplier");
        AssertBits(1.0741f, line.VarianceFactor, "variance");
        AssertBits(0.2f, line.EmployeeBonusPercent, "employee bonus");
        Assert.True(line.IsWeekendBonusApplied);
        AssertBits(684.74f, line.GrossRevenue, "gross");
        AssertBits(684.74f, line.OperatingCosts, "costs");
        AssertBits(0f, line.NetRevenue, "net floored at 0");
    }
}
