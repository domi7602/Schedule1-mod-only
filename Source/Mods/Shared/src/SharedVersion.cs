namespace S1Mods.Shared;

/// <summary>
/// Single source of truth for the Shared library version. Mods can log this at
/// startup to determine the exact Shared build they run against (binary
/// compatibility discipline). Keep in sync with the &lt;Version&gt; in Shared.csproj.
/// </summary>
public static class SharedVersion
{
    /// <summary>Current Shared library version (semver).</summary>
    public const string Current = "1.1.0";
}
