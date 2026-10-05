using System.Collections.Generic;
using BusinessIncome.Config;
using Xunit;

namespace BusinessIncome.Tests;

// Vertical slice 6: the ID validator's charset targets NORMALIZED runtime ids
// (the snake_case keys the resolver produces, e.g. "car_wash"), never the human
// display name ("Car Wash"). These tests pin that every real runtime id is
// accepted and that the human-readable names - which are VALUES, not keys - are
// handled by the display-name validator. They also pin that untrusted strings are
// escaped before they reach a rich-text console/log line.
public class ConfigIdValidationTests
{
    // The real default keys shipped in BusinessIncomeConfig (normalized runtime ids).
    [Theory]
    [InlineData("laundromat")]
    [InlineData("car_wash")]
    [InlineData("taco_ticklers")]
    [InlineData("dispensary")]
    [InlineData("nightclub")]
    [InlineData("bar")]
    [InlineData("coffee_shop")]
    [InlineData("convenience_store")]
    public void Every_real_default_business_id_is_accepted(string id)
        => Assert.True(ConfigCollectionSanitizer.IsSafeId(id));

    // The human-readable names contain spaces - they are display-name VALUES, never ids.
    // Widening IsSafeId to accept spaces/hyphens would be wrong: such a key could never match
    // a normalized lookup and would silently do nothing.
    [Theory]
    [InlineData("Car Wash")]
    [InlineData("Taco Ticklers")]
    [InlineData("Convenience Store")]
    public void Human_readable_business_names_are_not_ids(string name)
        => Assert.False(ConfigCollectionSanitizer.IsSafeId(name));

    [Theory]
    [InlineData("Car Wash")]
    [InlineData("Taco Ticklers")]
    [InlineData("Downtown Bar")]
    [InlineData("Convenience Store")]
    public void Human_readable_business_names_are_valid_display_names(string name)
        => Assert.True(ConfigCollectionSanitizer.IsSafeDisplayName(name));

    [Fact]
    public void Default_config_collections_survive_sanitize_with_no_warnings()
    {
        var cfg = new BusinessIncomeConfig();
        var warnings = new List<string>();

        cfg.Sanitize(warnings.Add);

        Assert.Equal(8, cfg.PropertyMultipliers.Count);
        Assert.Equal(8, cfg.DisplayNameOverrides.Count);
        Assert.Equal(4, cfg.WeekendBonusCategories.Count);
        Assert.Empty(warnings); // no legitimate default id/name is dropped or clamped
    }

    [Fact]
    public void EscapeForConsole_neutralizes_tmp_rich_text_characters()
    {
        Assert.Equal("&lt;color=#ff0000&gt;Red&lt;/color&gt;",
            ConfigCollectionSanitizer.EscapeForConsole("<color=#ff0000>Red</color>"));
        Assert.Equal("A &amp; B", ConfigCollectionSanitizer.EscapeForConsole("A & B"));
        Assert.Equal(string.Empty, ConfigCollectionSanitizer.EscapeForConsole(null));
        Assert.Equal(string.Empty, ConfigCollectionSanitizer.EscapeForConsole(""));
    }

    [Fact]
    public void Multiplier_warning_for_an_unsafe_id_does_not_emit_raw_rich_text()
    {
        // A hand-edited sidecar key can carry markup. The warning must escape it so it cannot
        // break/inject the surrounding <color=...> console line.
        var cfg = new BusinessIncomeConfig { PropertyMultipliers = new() { ["<b>x</b>"] = 1.5f } };
        var warnings = new List<string>();

        cfg.Sanitize(warnings.Add);

        var w = Assert.Single(warnings, s => s.Contains("unsafe business id"));
        Assert.Contains("&lt;b&gt;x&lt;/b&gt;", w);
        Assert.DoesNotContain("<b>", w);
    }
}
