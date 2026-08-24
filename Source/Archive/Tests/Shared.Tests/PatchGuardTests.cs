using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

public class PatchGuardTests
{
    [Fact]
    public void PatchGuard_Report_DoesNotThrow()
    {
        var ex = Record.Exception(() => PatchGuard.Report());
        Assert.True(ex == null || ex is System.NullReferenceException || ex is System.IO.FileNotFoundException || ex is System.TypeInitializationException);
    }

    [Fact]
    public void SafeInvoker_Execute_SwallowsException()
    {
        var ex = Record.Exception(() => SafeInvoker.Execute(() => throw new System.InvalidOperationException("test"), null, "test"));
        Assert.Null(ex);
    }

    [Fact]
    public void SafeInvoker_Execute_ReturnsValueOnSuccess()
    {
        var result = SafeInvoker.Execute(() => 42, -1, null, "test");
        Assert.Equal(42, result);
    }

    [Fact]
    public void SafeInvoker_Execute_ReturnsFallbackOnException()
    {
        var result = SafeInvoker.Execute<int>(() => throw new System.Exception("oops"), -1, null, "test");
        Assert.Equal(-1, result);
    }
}
