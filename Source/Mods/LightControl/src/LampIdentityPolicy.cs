namespace LightControl.Core;

public enum NativeIdentityRejection { None = 0, Missing = 1, Empty = 2, Malformed = 3 }

public readonly record struct NativeIdentityResult(bool Accepted, NativeIdentityRejection Rejection,
    LampIdentity? Identity, PersistentLampId? PersistentId);

/// <summary>
/// Turns a native placed-item GUID into a persistent logical identity. Only a live, initialized
/// placed object with a nonempty GUID is accepted; an item definition or a runtime instance id is
/// never an identity. Pure, no game dependency.
/// </summary>
public static class LampIdentityPolicy
{
    public const string Scheme = "schedule1.buildableitem.guid";
    public const int SchemeVersion = 1;
    public const string EmptyGuidText = "00000000-0000-0000-0000-000000000000";

    /// <summary>
    /// Validates the native GUID text and normalizes every textual form to one canonical identity,
    /// so upper case, brace and parenthesis spellings of the same GUID never become two lamps.
    /// </summary>
    public static NativeIdentityResult FromNativeGuid(string? guidText)
    {
        if (string.IsNullOrWhiteSpace(guidText))
            return new(false, NativeIdentityRejection.Missing, null, null);
        string trimmed = guidText.Trim();
        if (!Guid.TryParse(trimmed, out Guid parsed))
            return new(false, NativeIdentityRejection.Malformed, null, null);
        if (parsed == Guid.Empty)
            return new(false, NativeIdentityRejection.Empty, null, null);
        var persistent = new PersistentLampId(PersistentIdentityKind.VerifiedNativeInstance, Scheme, SchemeVersion,
            parsed.ToString("D"));
        return new(true, NativeIdentityRejection.None, LampIdentity.Persistent(persistent), persistent);
    }
}
