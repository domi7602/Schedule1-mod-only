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
    /// Trust-aware variant of <see cref="GetOwnedBusinesses"/> used by the engine.
    /// Returns the owned businesses as PURE <see cref="BusinessData"/> (no native
    /// handle) and fails closed instead of returning a partial/optimistic list:
    ///
    ///   false  - the owned list is unreadable (static getter threw) or null/unready;
    ///   false  - any entry is an invalid native item (null proxy, collected wrapper,
    ///            dead pointer), has no usable name field, or its employee list cannot
    ///            be read (threw / not ready);
    ///   true   - every entry validated; an EMPTY, real list yields an empty result.
    ///
    /// A single bad entry discards the WHOLE read (never a partial list presented as
    /// complete). The indexed IL2CPP iteration is guarded on every hop.
    /// </summary>
    public static bool TryGetOwnedBusinesses(
        Dictionary<string, string>? displayNameOverrides,
        out List<BusinessData> businesses)
    {
        businesses = new List<BusinessData>();
        var overrides = displayNameOverrides;

        try
        {
            // Business.OwnedBusinesses is an IL2CPP interop list, not System.Collections.Generic.
            var owned = Business.OwnedBusinesses;

            if (owned == null)
            {
                // Not ready yet (e.g. before the save/property manager is built) - not an
                // empty list. The caller must not treat this as "no businesses".
                Mod.Log.Debug("[BusinessResolver] Owned business list not ready (null).");
                return false;
            }

            int count = owned.Count;
            return OwnedBusinessReader.TryReadAll(count, (int i, out BusinessData data) =>
            {
                data = null!;

                // Indexed IL2CPP iteration: every hop can throw on a torn-down proxy.
                Business biz;
                try
                {
                    biz = owned[i];
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"[BusinessResolver] Owned[{i}] indexer threw: {ex.Message}");
                    return false;
                }

                // Invalid native item: null proxy, GC-collected wrapper, or dead pointer.
                if (biz == null || biz.WasCollected || biz.Pointer == IntPtr.Zero)
                {
                    Mod.Log.Warn($"[BusinessResolver] Owned[{i}] is an invalid native item (null/collected/dead pointer).");
                    return false;
                }

                // Business name field: PropertyName, falling back to the Unity object name.
                string rawName;
                try
                {
                    rawName = biz.PropertyName;
                }
                catch
                {
                    try
                    {
                        rawName = biz.name;
                    }
                    catch (Exception ex)
                    {
                        Mod.Log.Warn($"[BusinessResolver] Owned[{i}] name fields unreadable: {ex.Message}");
                        return false;
                    }
                }

                if (string.IsNullOrWhiteSpace(rawName))
                {
                    Mod.Log.Warn($"[BusinessResolver] Owned[{i}] has no usable name field.");
                    return false;
                }

                // Employee read must succeed AND be ready - a null list is "not ready",
                // not a legitimate zero, so the whole read fails closed.
                int employeeCount;
                try
                {
                    var employees = biz.Employees;
                    if (employees == null)
                    {
                        Mod.Log.Warn($"[BusinessResolver] Owned[{i}] employee list not ready (null).");
                        return false;
                    }
                    employeeCount = employees.Count;
                }
                catch (Exception ex)
                {
                    Mod.Log.Warn($"[BusinessResolver] Owned[{i}] employee read failed: {ex.Message}");
                    return false;
                }

                string id = NormalizeId(rawName);
                string displayName = rawName;
                if (overrides != null && overrides.TryGetValue(id, out var overrideName) && !string.IsNullOrWhiteSpace(overrideName))
                {
                    displayName = overrideName;
                }

                data = new BusinessData
                {
                    Id = id,
                    DisplayName = displayName,
                    EmployeeCount = employeeCount
                };
                return true;
            }, out businesses);
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"[BusinessResolver] Owned business list unreadable: {ex.Message}");
            businesses = new List<BusinessData>();
            return false;
        }
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
