using System;
using System.Linq;
using CalculatorApp;
using Xunit;

namespace CalculatorApp.Tests;

public class CalculatorEngineTests
{
    private (CalculatorEngine engine, CalculatorState state) CreateEngine()
    {
        var state = new CalculatorState();
        var engine = new CalculatorEngine(state);
        return (engine, state);
    }

    [Fact]
    public void InputDigit_BuildsMultiDigitNumbers()
    {
        var (engine, _) = CreateEngine();

        engine.InputDigit(1);
        engine.InputDigit(2);
        engine.InputDigit(3);

        Assert.Equal("123", engine.DisplayText);
    }

    [Fact]
    public void InputDecimal_AllowsSingleDecimalPoint()
    {
        var (engine, _) = CreateEngine();

        engine.InputDigit(3);
        engine.InputDecimal();
        engine.InputDigit(1);
        engine.InputDigit(4);
        engine.InputDecimal(); // Duplicate decimal point should be ignored

        Assert.Equal("3.14", engine.DisplayText);
    }

    [Fact]
    public void BasicArithmetic_ExactDecimalAddition()
    {
        var (engine, _) = CreateEngine();

        // 0.1 + 0.2 must equal 0.3 without IEEE 754 floating point artifacts
        engine.InputDigit(0);
        engine.InputDecimal();
        engine.InputDigit(1);

        engine.InputOperator("+");

        engine.InputDigit(0);
        engine.InputDecimal();
        engine.InputDigit(2);

        engine.InputEquals();

        Assert.Equal("0.3", engine.DisplayText);
    }

    [Theory]
    [InlineData("+", 15, 27, "42")]
    [InlineData("-", 100, 42, "58")]
    [InlineData("×", 7, 8, "56")]
    [InlineData("÷", 144, 12, "12")]
    public void BasicOperations_EvaluateCorrectly(string op, int a, int b, string expected)
    {
        var (engine, _) = CreateEngine();

        foreach (char c in a.ToString()) engine.InputDigit(c - '0');
        engine.InputOperator(op);
        foreach (char c in b.ToString()) engine.InputDigit(c - '0');
        engine.InputEquals();

        Assert.Equal(expected, engine.DisplayText);
    }

    [Fact]
    public void DivisionByZero_SetsErrorMessageAndHasError()
    {
        var (engine, _) = CreateEngine();

        engine.InputDigit(5);
        engine.InputOperator("÷");
        engine.InputDigit(0);
        engine.InputEquals();

        Assert.True(engine.HasError);
        Assert.Contains("Cannot divide by 0", engine.DisplayText, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SquareRoot_CalculatesValidAndRejectsNegative()
    {
        var (engine, _) = CreateEngine();

        // sqrt(25) = 5
        engine.InputDigit(2);
        engine.InputDigit(5);
        engine.InputSquareRoot();
        Assert.Equal("5", engine.DisplayText);

        // sqrt(-9) = Invalid input
        engine.ClearAll();
        engine.InputDigit(9);
        engine.InputNegate();
        engine.InputSquareRoot();

        Assert.True(engine.HasError);
    }

    [Fact]
    public void SquareAndReciprocal_EvaluateCorrectly()
    {
        var (engine, _) = CreateEngine();

        // 5^2 = 25
        engine.InputDigit(5);
        engine.InputSquare();
        Assert.Equal("25", engine.DisplayText);

        // 1 / 4 = 0.25
        engine.ClearAll();
        engine.InputDigit(4);
        engine.InputReciprocal();
        Assert.Equal("0.25", engine.DisplayText);
    }

    [Fact]
    public void Overflow_SetsOverflowMessage()
    {
        var (engine, _) = CreateEngine();

        // Type a huge number then square it to provoke decimal overflow
        engine.SetDisplayValue(100_000_000_000_000m);
        engine.InputSquare();
        engine.InputSquare(); // 10^28 -> overflow

        Assert.True(engine.HasError);
        Assert.Equal("Overflow", engine.DisplayText);
    }

    [Fact]
    public void Negate_TogglesSign()
    {
        var (engine, _) = CreateEngine();

        engine.InputDigit(4);
        engine.InputDigit(2);
        engine.InputNegate();
        Assert.Equal("-42", engine.DisplayText);

        engine.InputNegate();
        Assert.Equal("42", engine.DisplayText);
    }

    [Fact]
    public void RepeatingEquals_AppliesLastOperationSuccessively()
    {
        var (engine, _) = CreateEngine();

        // 10 + 3 = 13 = 16 = 19
        engine.InputDigit(1);
        engine.InputDigit(0);
        engine.InputOperator("+");
        engine.InputDigit(3);
        engine.InputEquals();
        Assert.Equal("13", engine.DisplayText);

        engine.InputEquals();
        Assert.Equal("16", engine.DisplayText);

        engine.InputEquals();
        Assert.Equal("19", engine.DisplayText);
    }

    [Fact]
    public void History_RecordsCalculationsAndAllowsDeletion()
    {
        var (engine, state) = CreateEngine();

        engine.InputDigit(1);
        engine.InputOperator("+");
        engine.InputDigit(2);
        engine.InputEquals();

        engine.InputOperator("×");
        engine.InputDigit(3);
        engine.InputEquals();

        Assert.Equal(2, engine.History.Count);

        string firstId = engine.History[0].Id;
        engine.DeleteHistoryRecord(firstId);

        Assert.Single(engine.History);
        Assert.NotEqual(firstId, engine.History[0].Id);

        engine.ClearHistory();
        Assert.Empty(engine.History);
    }

    [Fact]
    public void ClearAndAllClear_BehaveCorrectly()
    {
        var (engine, _) = CreateEngine();

        engine.InputDigit(1);
        engine.InputDigit(0);
        engine.InputOperator("+");
        engine.InputDigit(9);
        engine.InputClear(); // Clears current entry (9 -> 0)

        Assert.Equal("0", engine.DisplayText);
        Assert.True(engine.HasPendingOperation);

        engine.InputDigit(5);
        engine.InputEquals();
        Assert.Equal("15", engine.DisplayText);

        engine.ClearAll();
        Assert.Equal("0", engine.DisplayText);
        Assert.False(engine.HasPendingOperation);
    }

    [Theory]
    [InlineData("1234.56", "1234.56")]
    [InlineData("$500", "500")]
    [InlineData("1.234,50", "1234.5")]
    public void TryPaste_ParsesVariousFormats(string input, string expectedDisplay)
    {
        var (engine, _) = CreateEngine();

        bool success = engine.TryPaste(input);

        Assert.True(success);
        Assert.Equal(expectedDisplay, engine.DisplayText);
    }
}
