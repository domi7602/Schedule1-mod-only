using System;

namespace TaxiDriver;

/// <summary>
/// Pure search matching for the TaxiApp destination list (search feature,
/// 2026-10-03): a query matches when it occurs (case-insensitive substring) in
/// the destination name OR its type tag ("DEAL"/"PROP"/"HOME"/"PARK"/"STAND").
/// An empty or whitespace-only query matches everything; surrounding
/// whitespace in the query is trimmed first. Unity-free so the rules stay
/// unit-tested (DestinationFilterTests).
/// </summary>
internal static class DestinationFilter
{
    /// <summary>True when the destination row should be visible for <paramref name="query"/>.</summary>
    internal static bool Matches(string? query, string? name, string? tag)
    {
        string trimmed = (query ?? string.Empty).Trim();
        if (trimmed.Length == 0)
            return true;

        return Contains(name, trimmed) || Contains(tag, trimmed);
    }

    private static bool Contains(string? field, string query) =>
        !string.IsNullOrEmpty(field) &&
        field.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
}
