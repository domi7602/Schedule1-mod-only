using System.Collections.Generic;
using System.Linq;
using BusinessIncome.Config;
using Xunit;

namespace BusinessIncome.Tests;

// Vertical slice 3: per-entry validation of the runtime collections.
// Bad entries are dropped/clamped with a warning; the rest of the collection survives.
public class ConfigEntryValidationTests
{
    [Fact]
    public void Sanitize_clamps_finite_multiplier_above_cap_instead_of_dropping_it()
    {
        var cfg = new BusinessIncomeConfig { PropertyMultipliers = new() { ["nightclub"] = 1000f } };
        var warnings = new List<string>();

        cfg.Sanitize(warnings.Add);

        Assert.Equal(100f, cfg.PropertyMultipliers["nightclub"]);
        Assert.Contains(warnings, w => w.Contains("nightclub"));
    }

    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    [InlineData(0f)]
    [InlineData(-2f)]
    public void Sanitize_drops_non_finite_or_non_positive_multiplier_entry(float bad)
    {
        var cfg = new BusinessIncomeConfig { PropertyMultipliers = new() { ["bar"] = bad, ["cafe"] = 2f } };
        var warnings = new List<string>();

        cfg.Sanitize(warnings.Add);

        Assert.False(cfg.PropertyMultipliers.ContainsKey("bar"));
        Assert.Equal(2f, cfg.PropertyMultipliers["cafe"]);
        Assert.Contains(warnings, w => w.Contains("bar"));
    }

    [Fact]
    public void Sanitize_keeps_valid_multiplier_entries_untouched()
    {
        var cfg = new BusinessIncomeConfig
        {
            PropertyMultipliers = new() { ["car_wash"] = 1.25f, ["dispensary"] = 100f },
        };

        cfg.Sanitize();

        Assert.Equal(1.25f, cfg.PropertyMultipliers["car_wash"]);
        Assert.Equal(100f, cfg.PropertyMultipliers["dispensary"]);
    }

    [Fact]
    public void Sanitize_drops_entry_with_unsafe_business_id_key()
    {
        var cfg = new BusinessIncomeConfig
        {
            PropertyMultipliers = new() { ["bad id!"] = 1.5f, ["ok_id"] = 1.5f },
            DisplayNameOverrides = new() { ["also bad<>"] = "X", ["ok_id"] = "Okay" },
        };
        var warnings = new List<string>();

        cfg.Sanitize(warnings.Add);

        Assert.False(cfg.PropertyMultipliers.ContainsKey("bad id!"));
        Assert.True(cfg.PropertyMultipliers.ContainsKey("ok_id"));
        Assert.False(cfg.DisplayNameOverrides.ContainsKey("also bad<>"));
        Assert.True(cfg.DisplayNameOverrides.ContainsKey("ok_id"));
    }

    [Fact]
    public void Sanitize_drops_display_name_with_rich_text_or_control_characters()
    {
        var cfg = new BusinessIncomeConfig
        {
            DisplayNameOverrides = new()
            {
                ["a"] = "<color=#ff0000>Red</color>",
                ["b"] = "Line\u0007bell",
                ["c"] = "  Trimmed Name  ",
            },
        };
        var warnings = new List<string>();

        cfg.Sanitize(warnings.Add);

        Assert.False(cfg.DisplayNameOverrides.ContainsKey("a"));
        Assert.False(cfg.DisplayNameOverrides.ContainsKey("b"));
        Assert.Equal("Trimmed Name", cfg.DisplayNameOverrides["c"]);
        Assert.Contains(warnings, w => w.Contains("a"));
    }

    [Fact]
    public void Sanitize_dedupes_weekend_categories_case_insensitively_and_normalizes_to_lowercase()
    {
        var cfg = new BusinessIncomeConfig
        {
            WeekendBonusCategories = new List<string> { "NightClub", "nightclub", " bar ", "bad key!", "BAR" },
        };
        var warnings = new List<string>();

        cfg.Sanitize(warnings.Add);

        Assert.Equal(new[] { "nightclub", "bar" }, cfg.WeekendBonusCategories);
        Assert.Contains(warnings, w => w.Contains("bad key!"));
    }
}
