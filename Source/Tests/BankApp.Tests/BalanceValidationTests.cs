using BankApp.Logic;
using Xunit;

namespace BankApp.Tests;

public class BalanceValidationTests
{
    [Theory]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    [InlineData(float.NegativeInfinity)]
    public void DepositRejectsUnavailableDestinationBalance(float balance)
    {
        var quote = TransferMath.ComputeQuote(TransferDirection.Deposit, 100f, 1000f, balance, 2f, false, 0f);
        Assert.False(quote.IsValid);
    }

    [Fact]
    public void DepositRejectsOverflowingResult()
    {
        var quote = TransferMath.ComputeQuote(TransferDirection.Deposit, float.MaxValue, float.MaxValue, float.MaxValue, 0f, false, 0f);
        Assert.False(quote.IsValid);
    }
}
