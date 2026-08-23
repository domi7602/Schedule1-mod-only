using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Il2CppScheduleOne.Property;
using S1Mods.Shared;

namespace BusinessIncome.Services;

/// <summary>
/// Data holder for a resolved and validated vanilla business.
/// </summary>
public sealed class ResolvedBusiness
{
    public string Id { get; init; } = "";
    public string DisplayName { get; init; } = "";
    public int EmployeeCount { get; init; }
    public Business RawBusiness { get; init; } = null!;
}

/// <summary>
/// Encapsulates volatile game and S1API calls for detecting and normalizing businesses.
/// </summary>
public static class BusinessResolver
{
    private static readonly Regex NormalizerRegex = new(@"[^a-z0-9_]+", RegexOptions.Compiled);

    /// <summary>
    /// Returns all businesses currently owned by the player.
    /// </summary>
    public static List<ResolvedBusiness> GetOwnedBusinesses(Dictionary<string, string>? displayNameOverrides = null)
    {
        var result = new List<ResolvedBusiness>();

        try
        {
            var owned = Business.OwnedBusinesses;
            if (owned == null)
                return result;

            int count = owned.Count;
            for (int i = 0; i < count; i++)
            {
                var biz = owned[i];
                if (biz == null || biz.Pointer == IntPtr.Zero)
                    continue;

                string rawName = "";
                try
                {
                    rawName = biz.PropertyName;
                }
                catch
                {
                    rawName = biz.name;
                }

                if (string.IsNullOrWhiteSpace(rawName))
                    rawName = $"business_{i}";

                string id = NormalizeId(rawName);

                string displayName = rawName;
                if (displayNameOverrides != null && displayNameOverrides.TryGetValue(id, out var overrideName) && !string.IsNullOrWhiteSpace(overrideName))
                {
                    displayName = overrideName;
                }

                int employeeCount = 0;
                try
                {
                    if (biz.Employees != null)
                    {
                        employeeCount = biz.Employees.Count;
                    }
                }
                catch
                {
                    employeeCount = 0;
                }

                result.Add(new ResolvedBusiness
                {
                    Id = id,
                    DisplayName = displayName,
                    EmployeeCount = employeeCount,
                    RawBusiness = biz
                });
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to resolve businesses: {ex.Message}");
        }

        return result;
    }

    /// <summary>
    /// Normalizes any property name to a stable identifier (e.g. "Car Wash" -> "car_wash").
    /// </summary>
    public static string NormalizeId(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "unknown";

        string clean = name.Trim().ToLowerInvariant().Replace(" ", "_").Replace("-", "_");
        clean = NormalizerRegex.Replace(clean, "");
        return string.IsNullOrEmpty(clean) ? "unknown" : clean;
    }
}
