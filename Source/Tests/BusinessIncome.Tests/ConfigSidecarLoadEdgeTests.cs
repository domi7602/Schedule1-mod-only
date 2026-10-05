using BusinessIncome.Config;
using Xunit;

namespace BusinessIncome.Tests;

// Vertical slice 5b: the classifier's edge cases around an UNREADABLE or
// UNSUPPORTED backup - the cases that decide whether a hand-edited/corrupt
// sidecar is preserved and reported instead of being silently defaulted.
public class ConfigSidecarLoadEdgeTests
{
    private const string Valid = """{ "SchemaVersion": 1, "PropertyMultipliers": { "nightclub": 2.5 } }""";
    private const string Unsupported = """{ "SchemaVersion": 99, "PropertyMultipliers": { "nightclub": 9 } }""";

    [Fact]
    public void Missing_main_with_unreadable_backup_is_unreadable_not_missing()
    {
        // The backup exists but cannot be parsed. This must NOT look like "nothing on disk"
        // (which silently keeps defaults) - it is Unreadable, and the loader never touches files.
        var status = SidecarLoad.Classify(false, null, true, null, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.Unreadable, status);
        Assert.Null(data);
    }

    [Fact]
    public void Missing_main_with_unsupported_backup_reports_unsupported()
    {
        // The only readable file declares a newer schema: report UnsupportedSchema and expose the
        // declared version (the caller preserves the file and applies nothing).
        var status = SidecarLoad.Classify(false, null, true, Unsupported, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.UnsupportedSchema, status);
        Assert.Equal(99, data!.SchemaVersion);
    }

    [Fact]
    public void Unsupported_main_and_unsupported_backup_reports_unsupported()
    {
        var status = SidecarLoad.Classify(true, Unsupported, true, Unsupported, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.UnsupportedSchema, status);
        Assert.Equal(99, data!.SchemaVersion);
    }

    [Fact]
    public void Unsupported_main_with_unreadable_backup_is_unsupported_not_unreadable()
    {
        // A readable-but-newer main wins over an unreadable backup: the schema can still be named,
        // so the caller can tell the user exactly which version is unsupported.
        var status = SidecarLoad.Classify(true, Unsupported, true, null, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.UnsupportedSchema, status);
        Assert.Equal(99, data!.SchemaVersion);
    }

    [Fact]
    public void Unsupported_main_with_valid_backup_still_prefers_the_backup()
    {
        // A newer main must never shadow a readable, current-schema backup.
        var status = SidecarLoad.Classify(true, Unsupported, true, Valid, out ConfigSidecarData? data);
        Assert.Equal(SidecarLoadStatus.Backup, status);
        Assert.Equal(2.5f, data!.PropertyMultipliers!["nightclub"]);
    }
}
