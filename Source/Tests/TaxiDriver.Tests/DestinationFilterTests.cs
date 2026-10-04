using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Rules for the TaxiApp destination search (DestinationFilter): an empty
/// query matches everything, otherwise a case-insensitive substring must occur
/// in the name or the type tag. Pure logic, linked-source tested.
/// </summary>
public class DestinationFilterTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void EmptyQuery_MatchesEverything(string? query)
    {
        Assert.True(DestinationFilter.Matches(query, "Barn", "DEAL"));
        Assert.True(DestinationFilter.Matches(query, "<unnamed>", "PARK"));
    }

    [Fact]
    public void NameSubstring_Matches_IgnoringCase()
    {
        Assert.True(DestinationFilter.Matches("bar", "Barn", "DEAL"));
        Assert.True(DestinationFilter.Matches("BARN", "Barn", "DEAL"));
        Assert.True(DestinationFilter.Matches("Ba", "Barn", "DEAL"));
    }

    [Fact]
    public void TagSubstring_Matches_IgnoringCase()
    {
        Assert.True(DestinationFilter.Matches("deal", "Barn", "DEAL"));
        Assert.True(DestinationFilter.Matches("home", "Motel", "HOME"));
        Assert.True(DestinationFilter.Matches("prop", "Motel", "PROP"));
        Assert.True(DestinationFilter.Matches("stand", "Taxi-Stand (ParkingGarage)", "STAND"));
    }

    [Fact]
    public void QueryMatchingNeither_NameNorTag_ShowsNoRow()
    {
        Assert.False(DestinationFilter.Matches("airfield", "Barn", "DEAL"));
    }

    [Fact]
    public void UmlautCase_IsFolded()
    {
        Assert.True(DestinationFilter.Matches("MOWE", "Möwe", "PARK") == false); // umlaut is not "o"
        Assert.True(DestinationFilter.Matches("MÖWE", "Möwe", "PARK"));
    }

    [Fact]
    public void SurroundingWhitespace_IsTrimmed()
    {
        Assert.True(DestinationFilter.Matches("  barn  ", "Barn", "DEAL"));
    }

    [Fact]
    public void NullFields_NeverMatchANonEmptyQuery()
    {
        Assert.False(DestinationFilter.Matches("x", null, null));
    }

    [Fact]
    public void MultiWordQuery_MustOccurAsTyped()
    {
        // The real stand row reads "Taxi-Stand (ParkingGarage)" - a multi-word
        // query must occur verbatim (order and punctuation included).
        Assert.True(DestinationFilter.Matches("xi-Stand (Park", "Taxi-Stand (ParkingGarage)", "STAND"));
        Assert.False(DestinationFilter.Matches("stand taxi", "Taxi-Stand (ParkingGarage)", "STAND"));
    }
}
