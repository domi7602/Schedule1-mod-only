using System;
using System.Collections.Generic;

namespace LightControl.Core;

/// <summary>
/// Unity-free ownership gate for one placed grow lamp. A lamp may only be registered, commanded or
/// driven while its live parent property is one the player actually owns. The adapter reads the
/// native values (property code, the live <c>IsOwned</c> flag and the native owned-property list)
/// and passes them in; this type never touches Unity or the game.
/// </summary>
public static class PropertyOwnershipPolicy
{
    /// <summary>
    /// True when the property code is nonblank and the property is owned: either the live native
    /// <c>IsOwned</c> flag is set, or the code appears in the native owned-property/business list.
    /// An unreadable (null/empty) code is never owned, so an arbitrary or unowned grow lamp cannot
    /// be controlled.
    /// </summary>
    public static bool IsOwnedProperty(string? propertyCode, bool isOwned, IReadOnlyCollection<string>? ownedPropertyCodes)
    {
        if (string.IsNullOrEmpty(propertyCode)) return false;
        if (isOwned) return true;
        if (ownedPropertyCodes is null) return false;
        foreach (string code in ownedPropertyCodes)
            if (string.Equals(code, propertyCode, StringComparison.Ordinal)) return true;
        return false;
    }
}
