using System;
using System.Collections.Generic;
using BusinessIncome.Config;
using Xunit;

namespace BusinessIncome.Tests;

// Vertical slice 2: BusinessIncomeConfig.Sanitize guarantees non-null, case-insensitive
// runtime collections and honors deliberately-empty ones (no silent refill with defaults).
public class ConfigCollectionSanitizeTests
{
    [Fact]
    public void Sanitize_replaces_null_collections_with_empty_case_insensitive_containers()
    {
        var cfg = new BusinessIncomeConfig
        {
            PropertyMultipliers = null!,
            DisplayNameOverrides = null!,
            WeekendBonusCategories = null!,
        };
        var warnings = new List<string>();

        cfg.Sanitize(warnings.Add);

        Assert.NotNull(cfg.PropertyMultipliers);
        Assert.Empty(cfg.PropertyMultipliers);
        Assert.Equal(StringComparer.OrdinalIgnoreCase, cfg.PropertyMultipliers.Comparer);

        Assert.NotNull(cfg.DisplayNameOverrides);
        Assert.Empty(cfg.DisplayNameOverrides);
        Assert.Equal(StringComparer.OrdinalIgnoreCase, cfg.DisplayNameOverrides.Comparer);

        Assert.NotNull(cfg.WeekendBonusCategories);
        Assert.Empty(cfg.WeekendBonusCategories);

        Assert.Contains(warnings, w => w.Contains("PropertyMultipliers"));
        Assert.Contains(warnings, w => w.Contains("DisplayNameOverrides"));
        Assert.Contains(warnings, w => w.Contains("WeekendBonusCategories"));
    }

    [Fact]
    public void Sanitize_preserves_empty_collections_instead_of_refilling_defaults()
    {
        var cfg = new BusinessIncomeConfig
        {
            PropertyMultipliers = new Dictionary<string, float>(),
            DisplayNameOverrides = new Dictionary<string, string>(),
            WeekendBonusCategories = new List<string>(),
        };

        cfg.Sanitize();

        Assert.Empty(cfg.PropertyMultipliers);
        Assert.Empty(cfg.DisplayNameOverrides);
        Assert.Empty(cfg.WeekendBonusCategories);
    }

    [Fact]
    public void Sanitize_rebuilds_case_sensitive_dictionaries_as_ordinal_ignore_case()
    {
        var cfg = new BusinessIncomeConfig
        {
            PropertyMultipliers = new Dictionary<string, float>(StringComparer.Ordinal) { ["Car_Wash"] = 1.5f },
            DisplayNameOverrides = new Dictionary<string, string>(StringComparer.Ordinal) { ["NightClub"] = "Club" },
        };

        cfg.Sanitize();

        Assert.True(cfg.PropertyMultipliers.TryGetValue("car_WASH", out float v));
        Assert.Equal(1.5f, v);
        Assert.True(cfg.DisplayNameOverrides.TryGetValue("NIGHTclub", out string? n));
        Assert.Equal("Club", n);
    }
}
