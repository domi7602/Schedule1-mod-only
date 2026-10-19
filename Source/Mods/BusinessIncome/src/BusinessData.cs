using System;
using System.Collections.Generic;

namespace BusinessIncome.Services;

/// <summary>
/// Pure, Unity/IL2CPP-free projection of a validated owned business.
///
/// Fixed contract: exactly the fields the pure RevenueCalculator consumes,
/// mirroring the Id / DisplayName / EmployeeCount surface of the runtime
/// <c>ResolvedBusiness</c> (same <c>init</c> accessors) but WITHOUT the native
/// <c>Business</c> handle. This is what lets the calculator and its tests build
/// without Unity, IL2CPP or S1API references.
/// </summary>
public sealed class BusinessData
{
    public string Id { get; init; } = "";
    public string DisplayName { get; init; } = "";
    public int EmployeeCount { get; init; }
}

/// <summary>
/// Unity-free trust core for reading the runtime owned-business list.
///
/// The runtime adapter (<c>BusinessResolver.TryGetOwnedBusinesses</c>) supplies
/// one reader per index that performs the native/IL2CPP access and returns a
/// fully materialized <see cref="BusinessData"/> or <c>false</c> on any problem
/// (invalid native item, missing field, unreadable employee list). This core
/// enforces the fail-closed contract:
///   - a single unreadable or invalid entry fails the WHOLE read;
///   - the result is never a partial list presented as complete;
///   - an empty, real list (count 0) is a SUCCESS with an empty result;
///   - a negative count or a throwing reader is a failure with an empty result.
/// The runtime adapter handles the "list null / list read threw" (unready) case
/// before delegating here.
/// </summary>
public static class OwnedBusinessReader
{
    /// <summary>Reads one entry; returns false when the entry cannot be trusted.</summary>
    public delegate bool TryReadEntry(int index, out BusinessData data);

    public static bool TryReadAll(int count, TryReadEntry readEntry, out List<BusinessData> businesses)
    {
        if (readEntry == null) throw new ArgumentNullException(nameof(readEntry));

        if (count < 0)
        {
            businesses = new List<BusinessData>();
            return false;
        }

        var result = new List<BusinessData>(count);
        for (int i = 0; i < count; i++)
        {
            BusinessData data;
            try
            {
                if (!readEntry(i, out data) || data == null)
                {
                    businesses = new List<BusinessData>();
                    return false;
                }
            }
            catch
            {
                businesses = new List<BusinessData>();
                return false;
            }

            result.Add(data);
        }

        businesses = result;
        return true;
    }
}
