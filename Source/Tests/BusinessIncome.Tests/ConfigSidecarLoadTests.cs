using BusinessIncome.Config;
using Xunit;

namespace BusinessIncome.Tests;

// Vertical slice 5a: the PURE load classifier that distinguishes
// missing / valid / backup / unsupported-schema / unreadable before any file I/O.
public class ConfigSidecarLoadTests
{
    private const string Valid = """{ "SchemaVersion": 1, "PropertyMultipliers": { "nightclub": 2.5 } }""";
    private const string Unsupported = """{ "SchemaVersion": 99, "PropertyMultipliers": { "nightclub": 9 } }""";
    private const string Garbage = "{ this is not json";

    [Fact]
    public void Neither_file_present_is_missing()
    {
        var status = SidecarLoad.Classify(false, null, false, null, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.Missing, status);
        Assert.Null(data);
    }

    [Fact]
    public void Valid_main_is_valid_and_preferred_over_backup()
    {
        var status = SidecarLoad.Classify(true, Valid, true, Valid, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.Valid, status);
        Assert.Equal(2.5f, data!.PropertyMultipliers!["nightclub"]);
    }

    [Fact]
    public void Missing_main_with_valid_backup_is_backup()
    {
        var status = SidecarLoad.Classify(false, null, true, Valid, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.Backup, status);
        Assert.NotNull(data);
    }

    [Fact]
    public void Corrupt_main_with_valid_backup_is_backup()
    {
        var status = SidecarLoad.Classify(true, Garbage, true, Valid, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.Backup, status);
        Assert.NotNull(data);
    }

    [Fact]
    public void Corrupt_main_without_backup_is_unreadable()
    {
        var status = SidecarLoad.Classify(true, Garbage, false, null, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.Unreadable, status);
        Assert.Null(data);
    }

    [Fact]
    public void Existing_but_unreadable_files_are_unreadable_not_missing()
    {
        var status = SidecarLoad.Classify(true, null, true, null, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.Unreadable, status);
        Assert.Null(data);
    }

    [Fact]
    public void Corrupt_main_and_corrupt_backup_is_unreadable()
    {
        var status = SidecarLoad.Classify(true, Garbage, true, Garbage, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.Unreadable, status);
        Assert.Null(data);
    }

    [Fact]
    public void Unsupported_schema_is_reported_and_never_treated_as_valid()
    {
        var status = SidecarLoad.Classify(true, Unsupported, false, null, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.UnsupportedSchema, status);
        Assert.Equal(99, data!.SchemaVersion);
    }

    [Fact]
    public void Unsupported_main_falls_back_to_a_supported_backup()
    {
        var status = SidecarLoad.Classify(true, Unsupported, true, Valid, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.Backup, status);
        Assert.Equal(2.5f, data!.PropertyMultipliers!["nightclub"]);
    }
}
