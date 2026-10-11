using System;
using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes;
using S1API.Property;
using UnityEngine;
using NativeProperty = Il2CppScheduleOne.Property.Property;
using NativeBusiness = Il2CppScheduleOne.Property.Business;

namespace PropertyLEDs;

public sealed class PropertyEntry
{
    internal PropertyEntry(string id, string displayName, Transform anchor, bool isBusiness)
    {
        Id = id;
        DisplayName = displayName;
        Anchor = anchor;
        IsBusiness = isBusiness;
    }

    public string Id { get; }
    public string DisplayName { get; }
    public Transform Anchor { get; }
    public bool IsBusiness { get; }
}

/// <summary>Owned, live native property roots. Rebuilt only on load complete or explicit refresh.</summary>
internal sealed class PropertyRegistry
{
    private readonly Dictionary<string, PropertyEntry> _byId = new(StringComparer.Ordinal);
    private IReadOnlyList<PropertyEntry> _entries = Array.Empty<PropertyEntry>();
    internal IReadOnlyList<PropertyEntry> Entries => _entries;

    // Check the managed proxy BEFORE invoking Unity's destroyed-object equality operator.
    internal static bool NativeAlive(Il2CppObjectBase? value)
        => value != null && value.Pointer != IntPtr.Zero && !value.WasCollected;

    internal static bool UnityAlive(UnityEngine.Object? value)
    {
        try { return !ReferenceEquals(value, null) && NativeAlive(value) && value != null; }
        catch { return false; }
    }

    internal PropertyEntry? Find(string? id)
        => id != null && _byId.TryGetValue(id, out var entry) && UnityAlive(entry.Anchor) ? entry : null;

    internal void Refresh()
    {
        Clear();
        var entries = new List<PropertyEntry>();
        var ambiguousIds = new HashSet<string>(StringComparer.Ordinal);
        // S1API's wrappers expose metadata, but NOT Transform/InnerProperty publicly. Its manager
        // allocates an IL2CPP enumerator. Use indexed native owned lists to obtain guarded roots,
        // then the confirmed public wrapper constructors for metadata. Never reflect internals.
        try
        {
            var native = NativeProperty.OwnedProperties;
            if (NativeAlive(native))
            {
                for (int i = 0; i < Math.Min(native.Count, 512); i++)
                {
                    try
                    {
                        var property = native[i];
                        if (!UnityAlive(property)) continue;
                        var business = property.TryCast<NativeBusiness>();
                        PropertyWrapper wrapper = UnityAlive(business)
                            ? new BusinessWrapper(business!) : new PropertyWrapper(property);
                        Add(wrapper, property.transform, UnityAlive(business), entries, ambiguousIds);
                    }
                    catch (Exception ex) { Mod.Log.Warn($"Skipped owned property at index {i}: {ex.Message}"); }
                }
            }
        }
        catch (Exception ex) { Mod.Log.Warn($"Property discovery unavailable: {ex.Message}"); }

        // Businesses have their own owned list and are not assumed to be in OwnedProperties.
        try
        {
            var native = NativeBusiness.OwnedBusinesses;
            if (NativeAlive(native))
            {
                for (int i = 0; i < Math.Min(native.Count, 512); i++)
                {
                    try
                    {
                        var business = native[i];
                        if (!UnityAlive(business)) continue;
                        Add(new BusinessWrapper(business), business.transform, true, entries, ambiguousIds);
                    }
                    catch (Exception ex) { Mod.Log.Warn($"Skipped owned business at index {i}: {ex.Message}"); }
                }
            }
        }
        catch (Exception ex) { Mod.Log.Warn($"Business discovery unavailable: {ex.Message}"); }

        entries.Sort((left, right) => StringComparer.Ordinal.Compare(left.Id, right.Id));
        _entries = entries.AsReadOnly();
    }

    private void Add(PropertyWrapper wrapper, Transform anchor, bool business, List<PropertyEntry> entries,
        HashSet<string> ambiguousIds)
    {
        if (!UnityAlive(anchor) || !wrapper.IsOwned) return;
        string id = wrapper.PropertyCode;
        if (string.IsNullOrWhiteSpace(id) || ambiguousIds.Contains(id)) return;
        if (_byId.TryGetValue(id, out var existing))
        {
            if (existing.Anchor.Pointer == anchor.Pointer) return; // same native root in both lists
            // A duplicate code for DIFFERENT native roots is unsafe. Do not pick one arbitrarily.
            _byId.Remove(id);
            entries.Remove(existing);
            ambiguousIds.Add(id);
            Mod.Log.Warn($"Skipped ambiguous property code '{id}' (different native roots).");
            return;
        }
        string name = wrapper.PropertyName;
        var entry = new PropertyEntry(id, string.IsNullOrWhiteSpace(name) ? id : name, anchor, business);
        _byId.Add(id, entry);
        entries.Add(entry);
    }

    internal void Clear()
    {
        _byId.Clear();
        _entries = Array.Empty<PropertyEntry>();
    }
}
