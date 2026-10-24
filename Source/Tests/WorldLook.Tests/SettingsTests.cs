using System.Text.Json;
using WorldLook;
using Xunit;

namespace WorldLook.Tests;

public sealed class SettingsTests
{
    [Fact]
    public void LegacyCloudTestProfileKeepsItsWaterAndGlobalChoice()
    {
        const string json = """
            {"Enabled":false,"WaterEnabled":true,"Clouds":{"Enabled":false,"LightingStrength":0}}
            """;
        int writes = 0;
        var warnings = new List<string>();
        var profile = WorldLookSettings.Load(() => json, _ => { writes++; return true; }, warnings.Add);

        Assert.False(profile.Enabled);
        Assert.True(profile.WaterEnabled);
        Assert.True(profile.LightingEnabled);
        Assert.Equal(1.0, profile.Warmth);
        Assert.Equal(0, writes);
        Assert.Empty(warnings);
    }

    [Fact]
    public void ExplicitNewSettingsTakePrecedenceOverRetiredCloudSettings()
    {
        var profile = WorldLookSettings.Parse("""
            {"Enabled":true,"LightingEnabled":false,"WaterEnabled":false,"Warmth":0.25,
             "Clouds":{"Enabled":true,"LightingStrength":1}}
            """);
        Assert.True(profile.Enabled);
        Assert.False(profile.LightingEnabled);
        Assert.False(profile.WaterEnabled);
        Assert.Equal(.25, profile.Warmth);
    }

    [Fact]
    public void ExistingUnknownSettingsAreNotRewrittenOnRead()
    {
        int writes = 0;
        var profile = WorldLookSettings.Load(() => """{"FutureOption":{"value":7},"Warmth":0.4}""",
            _ => { writes++; return true; }, _ => { });
        Assert.Equal(.4, profile.Warmth);
        Assert.Equal(0, writes);
    }

    [Theory]
    [InlineData("{\"Warmth\":-1}", 0)]
    [InlineData("{\"Warmth\":0}", 0)]
    [InlineData("{\"Warmth\":0.5}", .5)]
    [InlineData("{\"Warmth\":1}", 1)]
    [InlineData("{\"Warmth\":8}", 1)]
    [InlineData("{\"Warmth\":1e400}", 0)]
    public void WarmthHasAFiniteBoundedRuntimeValue(string json, double expected)
    {
        Assert.Equal(expected, WorldLookSettings.Parse(json).Warmth);
    }

    [Fact]
    public void PreservesCaseInsensitiveCommentedProfiles()
    {
        var profile = WorldLookSettings.Parse("""
            { // User-owned config
              "enabled": false, "waterenabled": false, "warmth": 0.6,
            }
            """);
        Assert.False(profile.Enabled);
        Assert.False(profile.WaterEnabled);
        Assert.Equal(.6, profile.Warmth);
    }

    [Theory]
    [InlineData("")]
    [InlineData("null")]
    [InlineData("[]")]
    [InlineData("{")]
    [InlineData("{\"Enabled\":\"false\"}")]
    [InlineData("{\"Warmth\":\"NaN\"}")]
    [InlineData("{\"LightingEnabled\":null}")]
    public void InvalidExistingProfileDisablesOnlyTheSessionAndNeverWrites(string json)
    {
        int writes = 0;
        var warnings = new List<string>();
        var profile = WorldLookSettings.Load(() => json, _ => { writes++; return true; }, warnings.Add);
        Assert.False(profile.Enabled);
        Assert.Equal(0, writes);
        Assert.Single(warnings);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ReadFailuresDoNotBecomeDefaultFileWrites(bool accessDenied)
    {
        int writes = 0;
        var warnings = new List<string>();
        var profile = WorldLookSettings.Load(
            () => throw (accessDenied ? new UnauthorizedAccessException("denied") : new IOException("unavailable")),
            _ => { writes++; return true; }, warnings.Add);
        Assert.False(profile.Enabled);
        Assert.Equal(0, writes);
        Assert.Single(warnings);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void OnlyConfirmedAbsenceCreatesOneDefaultProfile(bool directoryAbsent)
    {
        var writes = new List<string>();
        var warnings = new List<string>();
        var profile = WorldLookSettings.Load(
            () => throw (directoryAbsent ? new DirectoryNotFoundException() : new FileNotFoundException()),
            json => { writes.Add(json); return true; }, warnings.Add);
        Assert.True(profile.Enabled);
        Assert.True(profile.LightingEnabled);
        Assert.True(profile.WaterEnabled);
        Assert.Equal(1.0, profile.Warmth);
        using var saved = JsonDocument.Parse(Assert.Single(writes));
        Assert.Equal(4, saved.RootElement.EnumerateObject().Count());
        Assert.True(saved.RootElement.GetProperty("LightingEnabled").GetBoolean());
        Assert.Empty(warnings);
    }

    [Fact]
    public void FailedDefaultSaveIsReportedWithoutRepeatedWrites()
    {
        int writes = 0;
        var warnings = new List<string>();
        var profile = WorldLookSettings.Load(() => throw new FileNotFoundException(),
            _ => { writes++; return false; }, warnings.Add);
        Assert.True(profile.Enabled);
        Assert.Equal(1, writes);
        Assert.Single(warnings);
    }

    [Fact]
    public void ThrowingDefaultSaveIsReportedAndKeepsValidSessionDefaults()
    {
        var warnings = new List<string>();
        var profile = WorldLookSettings.Load(() => throw new FileNotFoundException(),
            _ => throw new IOException("disk full"), warnings.Add);
        Assert.True(profile.Enabled);
        Assert.Single(warnings);
        Assert.Contains("disk full", warnings[0]);
    }
}
