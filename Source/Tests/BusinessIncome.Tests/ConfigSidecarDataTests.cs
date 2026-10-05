using System.Collections.Generic;
using System.Text.Json;
using BusinessIncome.Config;
using Xunit;

namespace BusinessIncome.Tests;

// Vertical slice 4: pure ConfigSidecarData semantics - collections-only payload,
// null (missing) vs empty distinction, tolerant parsing, schema marker.
public class ConfigSidecarDataTests
{
    [Fact]
    public void FromConfig_stamps_current_schema_and_carries_only_collections()
    {
        var cfg = new BusinessIncomeConfig();

        ConfigSidecarData data = ConfigSidecarData.FromConfig(cfg);
        string json = JsonSerializer.Serialize(data, ConfigSidecarData.JsonOptions);

        Assert.Equal(ConfigSidecarData.CurrentSchemaVersion, data.SchemaVersion);
        Assert.Equal(cfg.PropertyMultipliers.Count, data.PropertyMultipliers!.Count);
        Assert.Equal(cfg.DisplayNameOverrides.Count, data.DisplayNameOverrides!.Count);
        Assert.Equal(cfg.WeekendBonusCategories.Count, data.WeekendBonusCategories!.Count);

        // No scalar duplicates may ever leave the sidecar.
        Assert.DoesNotContain("PayoutHour", json);
        Assert.DoesNotContain("DefaultBaseIncome", json);
        Assert.DoesNotContain("OperatingCostRate", json);
        Assert.DoesNotContain("MaxCatchupDays", json);
    }

    [Fact]
    public void ApplyTo_honors_empty_collections_but_preserves_defaults_for_null_fields()
    {
        var cfg = new BusinessIncomeConfig();
        int defaultNames = cfg.DisplayNameOverrides.Count;
        int defaultWeekend = cfg.WeekendBonusCategories.Count;

        var data = new ConfigSidecarData
        {
            PropertyMultipliers = new Dictionary<string, float>(), // explicitly cleared
            DisplayNameOverrides = null,                           // missing -> keep default
            WeekendBonusCategories = null,                         // missing -> keep default
        };

        data.ApplyTo(cfg);

        Assert.Empty(cfg.PropertyMultipliers);
        Assert.Equal(defaultNames, cfg.DisplayNameOverrides.Count);
        Assert.Equal(defaultWeekend, cfg.WeekendBonusCategories.Count);
    }

    [Fact]
    public void ApplyTo_reconstructs_case_insensitive_dictionaries_and_copies_not_aliases()
    {
        var cfg = new BusinessIncomeConfig();
        var data = new ConfigSidecarData
        {
            PropertyMultipliers = new Dictionary<string, float>(System.StringComparer.Ordinal) { ["NightClub"] = 3f },
            DisplayNameOverrides = new Dictionary<string, string>(System.StringComparer.Ordinal) { ["NightClub"] = "The Club" },
            WeekendBonusCategories = new List<string> { "nightclub" },
        };

        data.ApplyTo(cfg);

        Assert.True(cfg.PropertyMultipliers.TryGetValue("nightclub", out float m));
        Assert.Equal(3f, m);
        Assert.True(cfg.DisplayNameOverrides.TryGetValue("nightclub", out string? n));
        Assert.Equal("The Club", n);

        data.PropertyMultipliers["nightclub"] = 9f; // mutating the DTO must not leak into the config
        Assert.Equal(3f, cfg.PropertyMultipliers["nightclub"]);
    }

    [Fact]
    public void Roundtrip_from_config_through_json_preserves_collections()
    {
        var cfg = new BusinessIncomeConfig
        {
            PropertyMultipliers = new Dictionary<string, float> { ["nightclub"] = 3f },
            DisplayNameOverrides = new Dictionary<string, string> { ["nightclub"] = "The Club" },
            WeekendBonusCategories = new List<string> { "car_wash" },
        };

        string json = JsonSerializer.Serialize(ConfigSidecarData.FromConfig(cfg), ConfigSidecarData.JsonOptions);

        Assert.True(ConfigSidecarData.TryParse(json, out ConfigSidecarData? parsed, out string? error));
        Assert.Null(error);

        var fresh = new BusinessIncomeConfig();
        parsed!.ApplyTo(fresh);

        Assert.Single(fresh.PropertyMultipliers);
        Assert.Equal(3f, fresh.PropertyMultipliers["nightclub"]);
        Assert.Single(fresh.DisplayNameOverrides);
        Assert.Equal("The Club", fresh.DisplayNameOverrides["nightclub"]);
        Assert.Equal(new[] { "car_wash" }, fresh.WeekendBonusCategories);
    }

    [Fact]
    public void Parse_ignores_legacy_scalar_fields_but_honors_loaded_collections()
    {
        const string legacy = """
        {
          "PayoutHour": 13,
          "MaxCatchupDays": 30,
          "DefaultBaseIncome": 9999,
          "OperatingCostRate": 0.5,
          "PropertyMultipliers": { "nightclub": 2.5 },
          "DisplayNameOverrides": { "nightclub": "Club" },
          "WeekendBonusCategories": [ "nightclub" ]
        }
        """;

        Assert.True(ConfigSidecarData.TryParse(legacy, out ConfigSidecarData? parsed, out string? error));
        Assert.Null(error);
        Assert.Null(parsed!.SchemaVersion); // legacy file has no explicit schema
        Assert.Equal(2.5f, parsed.PropertyMultipliers!["nightclub"]);

        var cfg = new BusinessIncomeConfig();
        parsed.ApplyTo(cfg);

        // scalars stay at their TOML/default value - legacy duplicates are ignored
        Assert.Equal(0, cfg.PayoutHour);
        Assert.Equal(7, cfg.MaxCatchupDays);
        Assert.Equal(500f, cfg.DefaultBaseIncome);
        Assert.Equal(2.5f, cfg.PropertyMultipliers["nightclub"]);
    }

    [Fact]
    public void Parse_keeps_representable_entries_and_only_drops_unrepresentable_ones()
    {
        const string json = """
        {
          "SchemaVersion": 1,
          "PropertyMultipliers": { "nightclub": 2.5, "bar": 0, "cafe": -1, "big": 1e39, "bad": "x" },
          "WeekendBonusCategories": [ "nightclub", 42, null, "bar" ]
        }
        """;
        var warnings = new List<string>();

        Assert.True(ConfigSidecarData.TryParse(json, out ConfigSidecarData? parsed, out string? error, warnings.Add));
        Assert.Null(error);
        // 2.5 kept; 0 and -1 are representable -> kept for the sanitizer to reject per-entry
        Assert.Equal(2.5f, parsed!.PropertyMultipliers!["nightclub"]);
        Assert.Equal(0f, parsed.PropertyMultipliers["bar"]);
        Assert.Equal(-1f, parsed.PropertyMultipliers["cafe"]);
        // 1e39 overflows float and "x" is not a number -> dropped per entry, sidecar survives
        Assert.False(parsed.PropertyMultipliers.ContainsKey("big"));
        Assert.False(parsed.PropertyMultipliers.ContainsKey("bad"));
        // wrong-typed array elements dropped per entry
        Assert.Equal(new[] { "nightclub", "bar" }, parsed.WeekendBonusCategories);
        Assert.NotEmpty(warnings);
    }

    [Fact]
    public void Parse_fails_only_on_structurally_invalid_json()
    {
        Assert.False(ConfigSidecarData.TryParse("{ not json", out _, out string? error));
        Assert.NotNull(error);

        Assert.False(ConfigSidecarData.TryParse("null", out _, out error));
        Assert.NotNull(error);

        Assert.False(ConfigSidecarData.TryParse("   ", out _, out error));
        Assert.NotNull(error);
    }

    [Fact]
    public void Future_schema_is_parsed_but_flagged_as_unsupported()
    {
        const string json = """{ "SchemaVersion": 99, "PropertyMultipliers": { "nightclub": 3 } }""";

        Assert.True(ConfigSidecarData.TryParse(json, out ConfigSidecarData? parsed, out string? error));
        Assert.Null(error);
        Assert.True(parsed!.HasUnsupportedSchema);
    }

    [Fact]
    public void Current_and_legacy_schemas_are_supported()
    {
        Assert.False(new ConfigSidecarData().HasUnsupportedSchema);
        Assert.False(new ConfigSidecarData { SchemaVersion = ConfigSidecarData.CurrentSchemaVersion }.HasUnsupportedSchema);
    }
}
