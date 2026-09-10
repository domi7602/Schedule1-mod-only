using System;
using System.Linq;
using System.Reflection;
using S1Mods.Shared;
using Xunit;

namespace Shared.Tests;

/// <summary>
/// Tests for S1Mods.Shared.TypeResolver — resolves types by full name or simple
/// class name across all loaded assemblies, with optional assembly hint and
/// in-memory caching.
/// </summary>
public class TypeResolverTests
{
    public TypeResolverTests()
    {
        // Ensure a clean cache for every test
        TypeResolver.ClearCache();
    }

    [Fact]
    public void Find_EmptyOrNull_ReturnsNull()
    {
        Assert.Null(TypeResolver.Find(""));
        Assert.Null(TypeResolver.Find("   "));
        Assert.Null(TypeResolver.Find(null!));
    }

    [Fact]
    public void Find_FullName_FindsSystemType()
    {
        // The test process has System.String loaded; find it by full name
        Type? t = TypeResolver.Find("System.String");
        Assert.NotNull(t);
        Assert.Equal(typeof(string), t);
    }

    [Fact]
    public void Find_SimpleName_FindsSystemType()
    {
        // Simple-name lookup ("String") across all assemblies
        Type? t = TypeResolver.Find("String");
        Assert.NotNull(t);
        Assert.Equal(typeof(string), t);
    }

    [Fact]
    public void Find_AssemblyHint_PrioritizesHint()
    {
        Type? t = TypeResolver.Find("String", "System.Runtime");
        Assert.NotNull(t);
        Assert.Equal(typeof(string), t);
    }

    [Fact]
    public void Find_UnknownType_ReturnsNull()
    {
        Type? t = TypeResolver.Find("ThisTypeShouldNotExist_42B27B5A");
        Assert.Null(t);
    }

    [Fact]
    public void Find_CachesResult_SecondCallReturnsSame()
    {
        // The internal cache should make a second call return the same instance
        Type? first = TypeResolver.Find("System.Int32");
        Type? second = TypeResolver.Find("System.Int32");
        Assert.NotNull(first);
        Assert.Same(first, second);
    }

    [Fact]
    public void ClearCache_ForcesRescan()
    {
        Type? first = TypeResolver.Find("System.Int32");
        Assert.NotNull(first);

        TypeResolver.ClearCache();

        Type? second = TypeResolver.Find("System.Int32");
        Assert.NotNull(second);
        // After clearing, the second resolution may return a new Type reference
        // (CLR re-load is not expected here; the cache just gets re-populated).
        // Same identity is the more likely outcome, but at minimum NotNull.
    }

    [Fact]
    public void Find_KnownSharedType_Resolves()
    {
        // The current test assembly references Shared; the type must be findable
        Type? t = TypeResolver.Find("S1Mods.Shared.SafeStorage");
        Assert.NotNull(t);
        Assert.Equal(typeof(SafeStorage), t);
    }

    [Fact]
    public void Find_TypeFromTestAssembly_Resolves()
    {
        // Find a type that lives in the test assembly itself (not the hint)
        Type? t = TypeResolver.Find("Shared.Tests.TypeResolverTests");
        Assert.NotNull(t);
        Assert.Equal(typeof(TypeResolverTests), t);
    }

    [Fact]
    public void Find_GenericType_ResolvesByFullName()
    {
        // Generic types in System.Collections.Generic should be reachable
        Type? t = TypeResolver.Find("System.Collections.Generic.Dictionary`2");
        Assert.NotNull(t);
        Assert.True(t!.IsGenericTypeDefinition);
    }
}
