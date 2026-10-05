using System;
using System.Collections.Generic;
using System.Linq;
using BusinessIncome.Config;
using BusinessIncome.Models;
using BusinessIncome.Services;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>
/// Behavior tests for the pure RevenueCalculator on the extracted BusinessData
/// model: staff-cap boundary, weekend gating, extremes, determinism and the
/// non-mutation contract. These target the DESIRED (new) behavior; the
/// pre-extraction quirk of the old calculator mutating the caller's config via
/// an internal Sanitize() call is pinned by
/// <see cref="Calculate_DoesNotSanitizeOrMutateCallerConfig"/>.
/// </summary>
public class RevenueCalculatorTests
{
    private static BusinessData Biz(string id, int employees = 0) =>
        new BusinessData { Id = id, DisplayName = id, EmployeeCount = employees };

    private static BusinessIncomeConfig Sanitized(BusinessIncomeConfig? cfg = null)
    {
        cfg ??= new BusinessIncomeConfig();
        cfg.Sanitize();
        return cfg;
    }

    [Theory]
    [InlineData(0, 0f)]
    [InlineData(1, 0.05f)]
    [InlineData(3, 0.15f)]
    [InlineData(4, 0.20f)]
    [InlineData(5, 0.25f)]   // 5 * 0.05 == cap exactly
    [InlineData(10, 0.25f)]  // 10 * 0.05 = 0.50 -> clamped to cap
    [InlineData(1000, 0.25f)]
    public void EmployeeBonus_ClampsAtMaxEmployeeBonus(int employees, float expected)
    {
        var line = RevenueCalculator.Calculate(Biz("car_wash", employees), 4, false, Sanitized());
        Assert.Equal(expected, line.EmployeeBonusPercent);
    }

    [Fact]
    public void EmployeeBonus_DisabledRate_NoBonusEvenWithStaff()
    {
        var cfg = Sanitized(new BusinessIncomeConfig { EmployeeBonusPerWorker = 0f });
        var line = RevenueCalculator.Calculate(Biz("car_wash", 5), 4, false, cfg);
        Assert.Equal(0f, line.EmployeeBonusPercent);
    }

    [Theory]
    [InlineData("nightclub")]
    [InlineData("bar")]
    [InlineData("taco_ticklers")]
    [InlineData("coffee_shop")]
    public void Weekend_ConfiguredCategory_AppliesRate(string id)
    {
        var line = RevenueCalculator.Calculate(Biz(id), 9, true, Sanitized());
        Assert.True(line.IsWeekendBonusApplied);
        Assert.Equal(0.25f, line.WeekendBonusPercent);
    }

    [Theory]
    [InlineData("car_wash")]
    [InlineData("dispensary")]
    [InlineData("unknown_biz")]
    public void Weekend_UnlistedCategory_NotApplied(string id)
    {
        var line = RevenueCalculator.Calculate(Biz(id), 9, true, Sanitized());
        Assert.False(line.IsWeekendBonusApplied);
        Assert.Equal(0f, line.WeekendBonusPercent);
    }

    [Fact]
    public void Weekend_Weekday_NeverApplied()
    {
        var line = RevenueCalculator.Calculate(Biz("nightclub"), 9, false, Sanitized());
        Assert.False(line.IsWeekendBonusApplied);
        Assert.Equal(0f, line.WeekendBonusPercent);
    }

    [Fact]
    public void Determinism_SameInputs_ProduceBitIdenticalLines()
    {
        var a = RevenueCalculator.Calculate(Biz("dispensary", 3), 42, true, Sanitized());
        var b = RevenueCalculator.Calculate(Biz("dispensary", 3), 42, true, Sanitized());

        Assert.Equal(BitConverter.SingleToInt32Bits(a.VarianceFactor), BitConverter.SingleToInt32Bits(b.VarianceFactor));
        Assert.Equal(BitConverter.SingleToInt32Bits(a.GrossRevenue), BitConverter.SingleToInt32Bits(b.GrossRevenue));
        Assert.Equal(BitConverter.SingleToInt32Bits(a.NetRevenue), BitConverter.SingleToInt32Bits(b.NetRevenue));
    }

    [Fact]
    public void DeterministicVariance_StaysInDocumentedRange()
    {
        for (int day = -10; day <= 100; day++)
        {
            float v = RevenueCalculator.CalculateDeterministicVariance(day, "car_wash");
            Assert.InRange(v, 0.85f, 1.15f);
        }
    }

    [Fact]
    public void DeterministicVariance_IsCaseInsensitive_OnBusinessId()
    {
        // The hash lowercases each char, so casing must not change the variance.
        Assert.Equal(
            BitConverter.SingleToInt32Bits(RevenueCalculator.CalculateDeterministicVariance(10, "Car_Wash")),
            BitConverter.SingleToInt32Bits(RevenueCalculator.CalculateDeterministicVariance(10, "car_wash")));
    }

    [Fact]
    public void DeterministicVariance_DifferentDay_Differs()
    {
        Assert.NotEqual(
            RevenueCalculator.CalculateDeterministicVariance(10, "car_wash"),
            RevenueCalculator.CalculateDeterministicVariance(11, "car_wash"));
    }

    [Fact]
    public void CalculateAll_MapsEveryBusinessInOrder()
    {
        var businesses = new List<BusinessData>
        {
            new() { Id = "car_wash", DisplayName = "Car Wash", EmployeeCount = 1 },
            new() { Id = "nightclub", DisplayName = "Nightclub", EmployeeCount = 5 },
            new() { Id = "bar", DisplayName = "Downtown Bar", EmployeeCount = 0 },
        };

        var lines = RevenueCalculator.CalculateAll(businesses, 6, true, Sanitized());

        Assert.Equal(3, lines.Count);
        Assert.Equal(new[] { "car_wash", "nightclub", "bar" }, lines.Select(l => l.BusinessId));
        Assert.Equal("Car Wash", lines[0].DisplayName);
    }

    [Fact]
    public void CalculateAll_EmptyInput_ReturnsEmpty()
    {
        var lines = RevenueCalculator.CalculateAll(Array.Empty<BusinessData>(), 6, true, Sanitized());
        Assert.Empty(lines);
    }

    [Fact]
    public void Calculate_DoesNotSanitizeOrMutateCallerConfig()
    {
        // TARGET (new contract): the calculator is a pure function of its inputs.
        // Sanitize() is the BOUNDARY caller's duty (IncomeEngine before the call).
        // The pre-extraction calculator called config.Sanitize() internally and
        // mutated the caller's config (e.g. -5 -> 500); that quirk is gone.
        var cfg = new BusinessIncomeConfig
        {
            DefaultBaseIncome = -5f,
            OperatingCostRate = 2f,
            EmployeeBonusPerWorker = -1f,
            MaxEmployeeBonus = 99f,
            WeekendBonusRate = 99f,
            PayoutHour = 99,
            MaxCatchupDays = -7,
        };

        var line = RevenueCalculator.Calculate(Biz("car_wash", 3), 10, false, cfg);

        Assert.Equal(-5f, cfg.DefaultBaseIncome);
        Assert.Equal(2f, cfg.OperatingCostRate);
        Assert.Equal(-1f, cfg.EmployeeBonusPerWorker);
        Assert.Equal(99f, cfg.MaxEmployeeBonus);
        Assert.Equal(99f, cfg.WeekendBonusRate);
        Assert.Equal(99, cfg.PayoutHour);
        Assert.Equal(-7, cfg.MaxCatchupDays);

        // With the raw (unsanitized) inputs the money floors at 0 instead of the
        // sanitized 500-base line.
        Assert.Equal(0f, line.GrossRevenue);
        Assert.Equal(0f, line.NetRevenue);
    }
}
