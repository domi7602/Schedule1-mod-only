using System;
using System.IO;
using PocketShop.Services;
using Xunit;

namespace PocketShop.Tests;

public sealed class ShopDirectoryPolicyTests
{
    [Theory]
    [InlineData("s1api_supplier_hyland_smoke_wholesaler_two_6eecf33a", "s1api_supplier_hyland_smoke_wholesaler_two_6eecf33a")]
    [InlineData("S1API_Supplier_Hyland", "Gas-Mart (West)")]
    [InlineData("Oscar's Store", " s1api_supplier_hidden ")]
    public void HidesS1ApiSupplierShops(string shopName, string shopCode)
    {
        Assert.True(ShopDirectoryPolicy.IsHiddenS1ApiSupplier(shopName, shopCode));
    }

    [Theory]
    [InlineData("Oscar's Store", "oscars")]
    [InlineData("Gas-Mart (West)", "gas_mart_west")]
    [InlineData("Dan's Hardware", "dans_hardware")]
    [InlineData("not_s1api_supplier_in_the_middle", "shop")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void KeepsOrdinaryShops(string? shopName, string? shopCode)
    {
        Assert.False(ShopDirectoryPolicy.IsHiddenS1ApiSupplier(shopName, shopCode));
    }

    [Fact]
    public void PrefixMatchesTheS1ApiSupplierContract()
    {
        string source = RepoFile("ThirdParty/S1API/S1API/Internal/Entities/Suppliers/SupplierRuntimeIds.cs");
        Assert.Contains($"ShopNamePrefix = \"{ShopDirectoryPolicy.S1ApiSupplierPrefix}\"", source);
    }

    [Fact]
    public void CatalogSkipsSupplierShopsBeforeReadingTheirListings()
    {
        string source = RepoFile("Source/Mods/PocketShop/src/ShopCatalog.cs");
        int refresh = source.IndexOf("public static void Refresh()", StringComparison.Ordinal);
        int listings = source.IndexOf("var listings = shop!.Listings;", refresh, StringComparison.Ordinal);
        int gate = source.IndexOf("IsHiddenS1ApiSupplier(", refresh, StringComparison.Ordinal);
        Assert.True(refresh >= 0 && listings > refresh && gate > refresh && gate < listings,
            "supplier shops must be dropped before their listings are read");
    }

    private static string RepoFile(string relativePath)
    {
        for (DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null; dir = dir.Parent)
        {
            string candidate = Path.Combine(dir.FullName, relativePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(candidate)) return File.ReadAllText(candidate);
        }
        throw new FileNotFoundException(relativePath);
    }
}
