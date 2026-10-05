using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BusinessIncome.Config;
using BusinessIncome.Services;
using Xunit;

namespace BusinessIncome.ConfigIo.Tests;

/// <summary>
/// Real-IO coverage for the production ConfigJsonStore persistence layer: the shipped
/// file and Shared SafeStorage are linked in; every test drives ACTUAL files on disk. This closes the gap left by the pure in-memory
/// config suite, which never exercised the write/backup/replace/failure path.
/// </summary>
public sealed class ConfigJsonStoreIoTests : IDisposable
{
    private readonly string _dir;
    private readonly string _sidecar;

    public ConfigJsonStoreIoTests()
    {
        _dir = Path.Combine(Environment.GetEnvironmentVariable("TMPDIR") ?? Path.GetTempPath(), "bi-io-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_dir);
        _sidecar = Path.Combine(_dir, "business_config.json");
    }

    public void Dispose()
    {
        try { if (Directory.Exists(_dir)) Directory.Delete(_dir, true); } catch { /* best effort */ }
    }

    private static string Json(ConfigSidecarData d) => JsonSerializer.Serialize(d, ConfigSidecarData.JsonOptions);

    [Fact]
    public void Save_then_apply_round_trips_the_collections()
    {
        var cfg = new BusinessIncomeConfig();
        cfg.PropertyMultipliers["nightclub"] = 3.5f;
        cfg.DisplayNameOverrides["car_wash"] = "My Wash";
        cfg.WeekendBonusCategories.Add("laundromat");

        Assert.True(ConfigJsonStore.Save(cfg, _sidecar));
        Assert.True(File.Exists(_sidecar));

        var loaded = new BusinessIncomeConfig();
        Assert.Equal(SidecarLoadStatus.Valid, ConfigJsonStore.ApplyToConfigFrom(loaded, _sidecar));

        Assert.Equal(3.5f, loaded.PropertyMultipliers["nightclub"]);
        Assert.Equal("My Wash", loaded.DisplayNameOverrides["car_wash"]);
        Assert.Contains("laundromat", loaded.WeekendBonusCategories);
    }

    [Fact]
    public void Save_replaces_atomically_and_backs_up_the_previous_file()
    {
        var first = new BusinessIncomeConfig();
        first.PropertyMultipliers["bar"] = 1.1f;
        Assert.True(ConfigJsonStore.Save(first, _sidecar));
        string firstJson = File.ReadAllText(_sidecar);

        var second = new BusinessIncomeConfig();
        second.PropertyMultipliers["nightclub"] = 2.2f;
        Assert.True(ConfigJsonStore.Save(second, _sidecar));

        Assert.True(File.Exists(_sidecar + ".bak"));
        Assert.Equal(firstJson, File.ReadAllText(_sidecar + ".bak")); // .bak holds the PREVIOUS file
        Assert.False(File.Exists(_sidecar + ".tmp"));                 // no temp file left behind

        var reload = new BusinessIncomeConfig();
        Assert.Equal(SidecarLoadStatus.Valid, ConfigJsonStore.ApplyToConfigFrom(reload, _sidecar));
        Assert.Equal(2.2f, reload.PropertyMultipliers["nightclub"]);
    }

    [Fact]
    public void Save_refuses_to_clobber_a_newer_schema_unless_forced()
    {
        File.WriteAllText(_sidecar, Json(new ConfigSidecarData
        {
            SchemaVersion = 99,
            PropertyMultipliers = new() { ["nightclub"] = 9f }
        }));
        string before = File.ReadAllText(_sidecar);
        var warnings = new List<string>();

        Assert.False(ConfigJsonStore.Save(new BusinessIncomeConfig(), _sidecar, false, warnings.Add));
        Assert.Equal(before, File.ReadAllText(_sidecar));             // byte-identical, untouched
        Assert.Contains(warnings, w => w.Contains("Refusing to overwrite"));

        Assert.True(ConfigJsonStore.Save(new BusinessIncomeConfig(), _sidecar, true, warnings.Add));
    }

    [Fact]
    public void Apply_unsupported_schema_preserves_the_file_and_applies_nothing()
    {
        File.WriteAllText(_sidecar, Json(new ConfigSidecarData
        {
            SchemaVersion = 42,
            PropertyMultipliers = new() { ["nightclub"] = 9f }
        }));
        string before = File.ReadAllText(_sidecar);
        var warnings = new List<string>();
        var baseline = new BusinessIncomeConfig();
        var cfg = new BusinessIncomeConfig();

        Assert.Equal(SidecarLoadStatus.UnsupportedSchema, ConfigJsonStore.ApplyToConfigFrom(cfg, _sidecar, warnings.Add));

        Assert.Equal(before, File.ReadAllText(_sidecar));             // preserved, never rewritten
        Assert.Equal(8, cfg.PropertyMultipliers.Count);               // defaults intact
        Assert.Equal(baseline.PropertyMultipliers["nightclub"], cfg.PropertyMultipliers["nightclub"]);
        Assert.Contains(warnings, w => w.Contains("unsupported schema"));
    }

    [Fact]
    public void Apply_uses_a_readable_backup_when_the_main_is_corrupt()
    {
        File.WriteAllText(_sidecar, "{ this is not valid json");
        File.WriteAllText(_sidecar + ".bak", Json(new ConfigSidecarData
        {
            SchemaVersion = 1,
            PropertyMultipliers = new() { ["nightclub"] = 4.5f }
        }));
        var warnings = new List<string>();
        var cfg = new BusinessIncomeConfig();

        Assert.Equal(SidecarLoadStatus.Backup, ConfigJsonStore.ApplyToConfigFrom(cfg, _sidecar, warnings.Add));

        Assert.Single(cfg.PropertyMultipliers);                       // replaced by the backup snapshot
        Assert.Equal(4.5f, cfg.PropertyMultipliers["nightclub"]);
    }

    [Fact]
    public void Apply_reports_unreadable_and_preserves_both_files()
    {
        File.WriteAllText(_sidecar, "{ bad main");
        File.WriteAllText(_sidecar + ".bak", "also bad");
        var warnings = new List<string>();
        var cfg = new BusinessIncomeConfig();

        Assert.Equal(SidecarLoadStatus.Unreadable, ConfigJsonStore.ApplyToConfigFrom(cfg, _sidecar, warnings.Add));

        Assert.Equal(8, cfg.PropertyMultipliers.Count);               // defaults kept, nothing applied
        Assert.True(File.Exists(_sidecar));                           // files preserved, never auto-deleted
        Assert.True(File.Exists(_sidecar + ".bak"));
        Assert.Contains(warnings, w => w.Contains("could not be read"));
    }

    [Fact]
    public void Apply_is_silent_and_keeps_defaults_when_no_file_exists()
    {
        var warnings = new List<string>();
        var cfg = new BusinessIncomeConfig();

        Assert.Equal(SidecarLoadStatus.Missing, ConfigJsonStore.ApplyToConfigFrom(cfg, _sidecar, warnings.Add));

        Assert.Equal(8, cfg.PropertyMultipliers.Count);
        Assert.False(File.Exists(_sidecar));
        Assert.Empty(warnings);
    }

    [Fact]
    public void Unsupported_schema_backup_is_preserved_and_not_applied()
    {
        File.WriteAllText(_sidecar + ".bak", Json(new ConfigSidecarData
        {
            SchemaVersion = 7,
            PropertyMultipliers = new() { ["bar"] = 9f }
        }));
        string beforeBak = File.ReadAllText(_sidecar + ".bak");
        var warnings = new List<string>();
        var cfg = new BusinessIncomeConfig();

        Assert.Equal(SidecarLoadStatus.UnsupportedSchema, ConfigJsonStore.ApplyToConfigFrom(cfg, _sidecar, warnings.Add));

        Assert.Equal(beforeBak, File.ReadAllText(_sidecar + ".bak")); // preserved
        Assert.Equal(8, cfg.PropertyMultipliers.Count);               // not applied
    }

    [Fact]
    public void Save_reports_failure_instead_of_false_success_when_the_write_is_impossible()
    {
        // Make the sidecar's parent path an ordinary FILE so directory creation fails on disk.
        string blocker = Path.Combine(_dir, "blocker");
        File.WriteAllText(blocker, "x");
        string impossible = Path.Combine(blocker, "business_config.json");
        var warnings = new List<string>();

        Assert.False(ConfigJsonStore.Save(new BusinessIncomeConfig(), impossible, false, warnings.Add));

        Assert.False(File.Exists(impossible));
        Assert.Contains(warnings, w => w.Contains("Failed to write"));
    }

    [Fact]
    public void Saved_sidecar_holds_only_the_collections_not_scalar_settings()
    {
        Assert.True(ConfigJsonStore.Save(new BusinessIncomeConfig(), _sidecar));

        using var doc = JsonDocument.Parse(File.ReadAllText(_sidecar));
        var names = doc.RootElement.EnumerateObject().Select(p => p.Name)
            .OrderBy(n => n, StringComparer.Ordinal).ToArray();

        // Scalars persist via MelonPreferences TOML, never in this file - which is why a
        // sidecar write failure must not be reported as "the setting will not persist".
        // HasUnsupportedSchema is a computed, get-only property that System.Text.Json also
        // emits; it is ignored on read and is not a config value.
        Assert.Equal(
            new[] { "DisplayNameOverrides", "HasUnsupportedSchema", "PropertyMultipliers", "SchemaVersion", "WeekendBonusCategories" },
            names);
    }
}
