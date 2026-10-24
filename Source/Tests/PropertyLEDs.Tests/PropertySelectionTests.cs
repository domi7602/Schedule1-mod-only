using PropertyLEDs.Core;
using Xunit;

namespace PropertyLEDs.Tests;

public sealed class PropertySelectionTests
{
    private static readonly string[] Owned = { "barn", "motelroom", "rv" };

    [Fact]
    public void FirstResolveAdoptsTheExactCurrentProperty()
    {
        var selection = new PropertySelection();
        Assert.True(selection.CanAdoptCurrent);

        string? shown = selection.Resolve(Owned, "motelroom");

        Assert.Equal("motelroom", shown);
        Assert.Equal("motelroom", selection.ShownId);
        Assert.False(selection.CanAdoptCurrent);
    }

    [Fact]
    public void FirstResolveWithoutACurrentPropertyFallsBackToTheFirstOwnedProperty()
    {
        var selection = new PropertySelection();

        Assert.Equal("barn", selection.Resolve(Owned, null));
        Assert.Equal("barn", selection.Resolve(Owned, ""));
    }

    [Fact]
    public void CurrentPropertyOutsideTheOwnedListIsNeverAdopted()
    {
        var selection = new PropertySelection();

        // Greenhouse Island is a real property code but not owned in this save.
        Assert.Equal("barn", selection.Resolve(Owned, "greenhouseisland"));
        Assert.Equal("barn", selection.ShownId);
    }

    [Fact]
    public void CurrentPropertyMatchIsOrdinal()
    {
        var selection = new PropertySelection();

        Assert.Equal("barn", selection.Resolve(Owned, "MotelRoom"));
    }

    [Fact]
    public void ManualSelectionPersistsAcrossReopenAndLaterCurrentPropertyChanges()
    {
        var selection = new PropertySelection();
        selection.Resolve(Owned, "motelroom");

        selection.Select("rv");
        // Reopen: RefreshView runs again, now with a different current property.
        Assert.Equal("rv", selection.Resolve(Owned, "motelroom"));
        Assert.Equal("rv", selection.Resolve(Owned, null));
        Assert.Equal("rv", selection.Resolve(Owned, "barn"));
        Assert.Equal("rv", selection.ShownId);
    }

    [Fact]
    public void ManualSelectionBeforeTheFirstResolveAlsoWins()
    {
        var selection = new PropertySelection();

        selection.Select("rv");

        Assert.False(selection.CanAdoptCurrent);
        Assert.Equal("rv", selection.Resolve(Owned, "motelroom"));
    }

    [Fact]
    public void InvalidateReArmsCurrentAdoptionForTheNextLoad()
    {
        var selection = new PropertySelection();
        selection.Resolve(Owned, "motelroom");
        selection.Select("rv");

        selection.Invalidate();

        Assert.Null(selection.ShownId);
        Assert.True(selection.CanAdoptCurrent);
        Assert.Equal("barn", selection.Resolve(Owned, "barn"));
    }

    [Fact]
    public void InvalidatedSelectionWithoutACurrentPropertyFallsBackToTheFirstOwnedProperty()
    {
        var selection = new PropertySelection();
        selection.Select("rv");

        selection.Invalidate();

        Assert.Equal("barn", selection.Resolve(Owned, null));
    }

    [Fact]
    public void AnEmptyOwnedListKeepsAdoptionArmedUntilTheListArrives()
    {
        var selection = new PropertySelection();

        Assert.Null(selection.Resolve(Array.Empty<string>(), "motelroom"));
        Assert.Null(selection.Resolve(null, "motelroom"));
        Assert.True(selection.CanAdoptCurrent);
        Assert.Equal("motelroom", selection.Resolve(Owned, "motelroom"));
    }

    [Fact]
    public void APropertyThatDisappearsFromTheOwnedListFallsBack()
    {
        var selection = new PropertySelection();
        selection.Resolve(Owned, "motelroom");

        Assert.Equal("barn", selection.Resolve(new[] { "barn", "rv" }, null));
    }

    [Fact]
    public void SelectIgnoresBlankIds()
    {
        var selection = new PropertySelection();
        selection.Resolve(Owned, "motelroom");

        selection.Select("  ");

        Assert.Equal("motelroom", selection.ShownId);
    }
}

public sealed class PropertyStatusTests
{
    [Fact]
    public void EnabledStatusNamesTheSelectedProperty()
    {
        Assert.Equal("LEDs enabled: Motel Room", PropertyStatus.Label("Motel Room", true));
    }

    [Fact]
    public void DisabledStatusNamesTheSelectedProperty()
    {
        Assert.Equal("LEDs disabled: Greenhouse Island", PropertyStatus.Label("Greenhouse Island", false));
    }

    [Fact]
    public void MissingDisplayNameKeepsTheStateOnly()
    {
        Assert.Equal("LEDs enabled", PropertyStatus.Label(null, true));
        Assert.Equal("LEDs disabled", PropertyStatus.Label("   ", false));
    }

    [Fact]
    public void DisplayNameIsTrimmed()
    {
        Assert.Equal("LEDs enabled: Motel Room", PropertyStatus.Label(" Motel Room ", true));
    }

    [Fact]
    public void DetailMirrorsTheSameState()
    {
        Assert.Equal("Property LEDs are currently on", PropertyStatus.Detail(true));
        Assert.Equal("Property LEDs are currently off", PropertyStatus.Detail(false));
    }
}
