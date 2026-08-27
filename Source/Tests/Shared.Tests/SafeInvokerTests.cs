using System;
using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Tests for S1Mods.Shared.SafeInvoker — exception isolation in event/lifecycle
/// callbacks. The static class swallows all exceptions and falls back via
/// Func&lt;T&gt;. TResult overloads. No Unity runtime needed.
/// </summary>
public class SafeInvokerTests
{
    [Fact]
    public void Execute_NullAction_ReturnsFalse()
    {
        Action? action = null;
        Assert.False(SafeInvoker.Execute(action));
    }

    [Fact]
    public void Execute_Success_ReturnsTrue()
    {
        bool ran = false;
        bool ok = SafeInvoker.Execute(() => ran = true);
        Assert.True(ok);
        Assert.True(ran);
    }

    [Fact]
    public void Execute_ThrowingAction_ReturnsFalseAndDoesNotRethrow()
    {
        bool ok = SafeInvoker.Execute(() => throw new InvalidOperationException("boom"));
        Assert.False(ok);
    }

    [Fact]
    public void ExecuteT_Success_PassesParameter()
    {
        int received = 0;
        Action<int> action = x => received = x;
        bool ok = SafeInvoker.Execute(action, 42);
        Assert.True(ok);
        Assert.Equal(42, received);
    }

    [Fact]
    public void ExecuteT_ThrowingAction_ReturnsFalse()
    {
        Action<int> action = _ => throw new Exception("nope");
        bool ok = SafeInvoker.Execute(action, 1);
        Assert.False(ok);
    }

    [Fact]
    public void ExecuteT_NullAction_ReturnsFalse()
    {
        Action<int>? action = null;
        Assert.False(SafeInvoker.Execute(action, 1));
    }

    [Fact]
    public void ExecuteFunc_NullFunc_ReturnsFallback()
    {
        int result = SafeInvoker.Execute<int>(null!, fallback: 99);
        Assert.Equal(99, result);
    }

    [Fact]
    public void ExecuteFunc_Success_ReturnsValue()
    {
        int result = SafeInvoker.Execute<int>(() => 7, fallback: -1);
        Assert.Equal(7, result);
    }

    [Fact]
    public void ExecuteFunc_ThrowingFunc_ReturnsFallback()
    {
        int result = SafeInvoker.Execute<int>(() => throw new Exception("nope"), fallback: -1);
        Assert.Equal(-1, result);
    }

    [Fact]
    public void Execute_AfterFailure_ContinuesToWork()
    {
        // Verify the SafeInvoker instance is reusable across failures
        int counter = 0;
        for (int i = 0; i < 5; i++)
        {
            SafeInvoker.Execute(() =>
            {
                if (i % 2 == 0) throw new Exception("even");
                counter++;
            });
        }
        Assert.Equal(2, counter); // i=1 and i=3 succeeded
    }
}
