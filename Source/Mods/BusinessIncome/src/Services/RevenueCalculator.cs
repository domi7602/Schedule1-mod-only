using System;
using System.Collections.Generic;
using BusinessIncome.Config;
using BusinessIncome.Models;

namespace BusinessIncome.Services;

/// <summary>
/// Pure, testable calculation logic without Unity, network, or game dependencies.
/// Uses deterministic pseudo-random numbers so revenue preview and payout match exactly.
/// </summary>
public static class RevenueCalculator
{
    /// <summary>
    /// Calculates the billing line for a specific business on a given in-game day.
    /// </summary>
    public static BusinessRevenueLine Calculate(
        ResolvedBusiness business,
        int elapsedDays,
        bool isWeekend,
        BusinessIncomeConfig config)
    {
        config.Sanitize();

        // 1. Resolve multiplier
        float multiplier = 1.0f;
        if (config.PropertyMultipliers.TryGetValue(business.Id, out float customMult) && customMult > 0f)
        {
            multiplier = customMult;
        }

        // 2. Deterministic daily variance (+/- 15%)
        float varianceFactor = CalculateDeterministicVariance(elapsedDays, business.Id);

        // 3. Compute employee bonus
        float employeeBonusPercent = 0f;
        if (business.EmployeeCount > 0 && config.EmployeeBonusPerWorker > 0f)
        {
            employeeBonusPercent = MathF.Min(
                business.EmployeeCount * config.EmployeeBonusPerWorker,
                config.MaxEmployeeBonus
            );
        }

        // 4. Check weekend bonus
        bool isWeekendBonusApplied = false;
        float weekendBonusPercent = 0f;
        if (isWeekend && config.WeekendBonusCategories.Contains(business.Id))
        {
            isWeekendBonusApplied = true;
            weekendBonusPercent = config.WeekendBonusRate;
        }

        // 5. Compute gross revenue
        float baseIncome = config.DefaultBaseIncome;
        float grossRevenue = baseIncome * multiplier * (1f + employeeBonusPercent) * (1f + weekendBonusPercent) * varianceFactor;
        grossRevenue = MathF.Max(0f, MathF.Round(grossRevenue, 2));

        // 6. Compute operating costs
        float operatingCosts = 0f;
        if (config.OperatingCostRate > 0f)
        {
            operatingCosts = MathF.Round(grossRevenue * config.OperatingCostRate, 2);
        }

        // 7. Compute net revenue (minimum 0)
        float netRevenue = MathF.Max(0f, MathF.Round(grossRevenue - operatingCosts, 2));

        return new BusinessRevenueLine
        {
            BusinessId = business.Id,
            DisplayName = business.DisplayName,
            BaseIncome = baseIncome,
            Multiplier = multiplier,
            VarianceFactor = varianceFactor,
            EmployeeCount = business.EmployeeCount,
            EmployeeBonusPercent = employeeBonusPercent,
            IsWeekendBonusApplied = isWeekendBonusApplied,
            WeekendBonusPercent = weekendBonusPercent,
            GrossRevenue = grossRevenue,
            OperatingCosts = operatingCosts,
            NetRevenue = netRevenue
        };
    }

    /// <summary>
    /// Calculates all billing lines for a list of businesses.
    /// </summary>
    public static List<BusinessRevenueLine> CalculateAll(
        IEnumerable<ResolvedBusiness> businesses,
        int elapsedDays,
        bool isWeekend,
        BusinessIncomeConfig config)
    {
        var list = new List<BusinessRevenueLine>();
        foreach (var biz in businesses)
        {
            list.Add(Calculate(biz, elapsedDays, isWeekend, config));
        }
        return list;
    }

    /// <summary>
    /// Generates a deterministic variance factor in the range [0.85, 1.15] based on in-game day and business ID.
    /// </summary>
    public static float CalculateDeterministicVariance(int elapsedDays, string businessId)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + elapsedDays;
            if (!string.IsNullOrEmpty(businessId))
            {
                foreach (char c in businessId)
                {
                    hash = hash * 31 + char.ToLowerInvariant(c);
                }
            }

            var rng = new Random(hash);
            // Uniformly distributed between 0.85 and 1.15
            double val = 0.85 + (rng.NextDouble() * 0.30);
            return (float)Math.Round(val, 4);
        }
    }
}
