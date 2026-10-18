using System;
using Xunit;

namespace TaxiDriver.Tests;

/// <summary>
/// Wiring contracts for the settlement balance readback (no Unity execution):
/// net worth is read before the money calls and the readback is logged after.
/// </summary>
public sealed class BalanceReadbackWiringTests
{
    [Fact]
    public void Charge_ReadsNetWorthBeforeTheMoneyCallsAndLogsTheReadback()
    {
        string fareMeter = DriverSafetySourceContractTests.ReadSource("FareMeter.cs");
        string charge = DriverSafetySourceContractTests.Section(fareMeter, "private static void Charge(int dollars)", "private static void WarnClientOnce(");
        int read = charge.IndexOf("ReadNetWorthSafe(", StringComparison.Ordinal);
        int execute = charge.IndexOf("FarePayment.Execute(", StringComparison.Ordinal);
        Assert.True(read >= 0 && read < execute, "Net worth must be read before the money calls run.");
        int logged = charge.IndexOf("LogBalanceReadback(", StringComparison.Ordinal);
        Assert.True(logged > execute, "The readback must be logged after the money calls.");
        Assert.Contains("BalanceReadbackPolicy.Describe(", fareMeter);
    }
}
