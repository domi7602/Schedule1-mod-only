using System;
using System.Collections.Generic;
using BusinessIncome.Services;
using Xunit;

namespace BusinessIncome.Tests;

/// <summary>
/// Trust-contract tests for the Unity-free core of BusinessResolver's
/// TryGetOwnedBusinesses: the indexed read is fail-closed. A single invalid
/// native item or unreadable field fails the WHOLE read and must never yield a
/// partial list; an empty-but-real list (count 0) is a success; an unreadable
/// list/count fails before this core is even reached (covered by the runtime
/// adapter, out of this pure suite's scope).
/// </summary>
public class RevenueResolverTrustTests
{
    private static BusinessData Item(string id, int employees = 0) =>
        new BusinessData { Id = id, DisplayName = id, EmployeeCount = employees };

    [Fact]
    public void EmptyRealList_IsSuccess_WithEmptyResult()
    {
        bool ok = OwnedBusinessReader.TryReadAll(0, (int _, out BusinessData _) =>
        {
            throw new InvalidOperationException("reader must not be called for an empty list");
        }, out var businesses);

        Assert.True(ok);
        Assert.Empty(businesses);
    }

    [Fact]
    public void AllValidItems_Succeeds_AndPreservesIndexOrder()
    {
        var source = new[] { Item("car_wash", 1), Item("nightclub", 5), Item("bar", 0) };

        bool ok = OwnedBusinessReader.TryReadAll(source.Length, (int i, out BusinessData data) =>
        {
            data = source[i];
            return true;
        }, out var businesses);

        Assert.True(ok);
        Assert.Equal(new[] { "car_wash", "nightclub", "bar" },
            System.Linq.Enumerable.Select(businesses, b => b.Id));
        Assert.Equal(5, businesses[1].EmployeeCount);
    }

    [Fact]
    public void SingleInvalidNativeItem_FailsWholeRead_NoPartialList()
    {
        var source = new[] { Item("car_wash"), Item("nightclub"), Item("bar") };

        bool ok = OwnedBusinessReader.TryReadAll(source.Length, (int i, out BusinessData data) =>
        {
            if (i == 1)
            {
                data = null!; // invalid native item / missing field
                return false;
            }
            data = source[i];
            return true;
        }, out var businesses);

        Assert.False(ok);
        Assert.Empty(businesses); // never a partial ("complete") list
    }

    [Fact]
    public void ReadThrows_FailsClosed_NoPartialList()
    {
        var source = new[] { Item("car_wash"), Item("nightclub") };

        bool ok = OwnedBusinessReader.TryReadAll(source.Length, (int i, out BusinessData data) =>
        {
            if (i == 1) throw new InvalidOperationException("native read threw");
            data = source[i];
            return true;
        }, out var businesses);

        Assert.False(ok);
        Assert.Empty(businesses);
    }

    [Fact]
    public void NullItemFromReader_IsTreatedAsFailure()
    {
        bool ok = OwnedBusinessReader.TryReadAll(1, (int _, out BusinessData data) =>
        {
            data = null!;
            return true; // reader claims success but yields nothing
        }, out var businesses);

        Assert.False(ok);
        Assert.Empty(businesses);
    }

    [Fact]
    public void NegativeCount_Fails()
    {
        bool ok = OwnedBusinessReader.TryReadAll(-1, (int _, out BusinessData data) =>
        {
            data = Item("x");
            return true;
        }, out var businesses);

        Assert.False(ok);
        Assert.Empty(businesses);
    }
}
