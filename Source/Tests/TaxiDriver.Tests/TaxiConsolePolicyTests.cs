using TaxiDriver;
using Xunit;

namespace TaxiDriver.Tests;

public sealed class TaxiConsolePolicyTests
{
    [Theory]
    [InlineData("help")]
    [InlineData("codes")]
    [InlineData("status")]
    [InlineData("diag")]
    [InlineData("probe")]
    [InlineData("trace")]
    [InlineData("pois")]
    [InlineData("fare")]
    [InlineData("ai")]
    public void ReadOnlyDiagnostics_AreAllowed(string command)
    {
        Assert.True(TaxiConsolePolicy.IsReadOnlyDiagnostic(command));
    }

    [Theory]
    [InlineData("spawn")]
    [InlineData("npc")]
    [InlineData("ride")]
    [InlineData("out")]
    [InlineData("go")]
    [InlineData("go2")]
    [InlineData("stop")]
    [InlineData("cleanup")]
    [InlineData("stand")]
    [InlineData("to")]
    [InlineData("clear")]
    [InlineData("visual")]
    public void OperationalOrStateChangingCommands_AreNotAllowed(string command)
    {
        Assert.False(TaxiConsolePolicy.IsReadOnlyDiagnostic(command));
    }

    [Fact]
    public void DiagnosticNames_AreCaseInsensitiveAndNullIsRejected()
    {
        Assert.True(TaxiConsolePolicy.IsReadOnlyDiagnostic("STATUS"));
        Assert.False(TaxiConsolePolicy.IsReadOnlyDiagnostic(null));
    }
}
