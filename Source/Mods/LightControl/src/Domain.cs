using System.Collections.ObjectModel;
using System.Text;

namespace LightControl.Core;

public enum ColorMode
{
    Original = 0, White = 1, Red = 2, Orange = 3, Yellow = 4, Green = 5,
    Cyan = 6, Blue = 7, Purple = 8, Pink = 9, Rainbow = 10
}

public enum IntensityLevel
{
    Percent10 = 10, Percent25 = 25, Percent50 = 50, Percent75 = 75,
    Percent100 = 100, Percent150 = 150, Percent200 = 200
}

public enum SessionMode { Host = 1, Client = 2, Local = 3 }
public enum PersistentIdentityKind { VerifiedNativeInstance = 1, VerifiedWorldLocator = 2 }
public enum LampPresence { Available = 1, TemporarilyUnloaded = 2 }
public enum SelectionKind { Lamp = 1, Group = 2, Property = 3, World = 4 }
public enum CommandKind { SetState = 1, CycleColor = 2, StartDisco = 3, StopDisco = 4 }

public static class LightLimits
{
    public const int ProtocolVersion = 1;
    public const int SaveSchemaVersion = 1;
    public const int MaxLamps = 10000;
    public const int MaxPropertyEffects = 1000;
    public const int MaxIdentifierLength = 256;
    public const int MaxJsonBytes = 4 * 1024 * 1024;
    public const int MaxRememberedCommands = 2048;
    public const double MinDiscoStepSeconds = 0.1;
    public const double MaxDiscoStepSeconds = 60;
    public const double RainbowPeriodSeconds = 10;
    public const double MinRainbowPeriodSeconds = 1;
    public const double MaxRainbowPeriodSeconds = 120;
    // Roughly 31 years of session-relative time; additionally limits tick arithmetic.
    public const double MaxSessionTimeSeconds = 1_000_000_000;

    internal static string Identifier(string value, string parameter)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > MaxIdentifierLength ||
            value.Any(char.IsControl) || !string.Equals(value, value.Trim(), StringComparison.Ordinal))
            throw new ArgumentException("Identifier must be nonblank, trimmed, bounded and free of control characters.", parameter);
        for (int index = 0; index < value.Length; index++)
        {
            if (!char.IsSurrogate(value[index])) continue;
            if (!char.IsHighSurrogate(value[index]) || index + 1 >= value.Length || !char.IsLowSurrogate(value[index + 1]))
                throw new ArgumentException("Identifier contains invalid Unicode.", parameter);
            index++;
        }
        return value;
    }

    internal static string? OptionalIdentifier(string? value, string parameter) =>
        value is null ? null : Identifier(value, parameter);

    internal static void Session(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("An explicit, nonempty session identity is required.", nameof(id));
    }

    internal static void EnumValue<T>(T value, string parameter) where T : struct, Enum
    {
        if (!Enum.IsDefined(typeof(T), value)) throw new ArgumentOutOfRangeException(parameter);
    }

    internal static double Time(double value, string parameter)
    {
        if (!double.IsFinite(value) || value < 0 || value > MaxSessionTimeSeconds)
            throw new ArgumentOutOfRangeException(parameter, "Time must be finite, nonnegative and session-relative.");
        return value;
    }

    internal static IReadOnlyList<T> Copy<T>(IEnumerable<T> values, int max, string parameter)
    {
        if (values is null) throw new ArgumentNullException(parameter);
        // Take one extra element, never materialize an unbounded external enumeration.
        var array = values.Take(max + 1).ToArray();
        if (array.Length > max || array.Any(item => item is null))
            throw new ArgumentException("Collection is too large or contains null entries.", parameter);
        return Array.AsReadOnly(array);
    }
}

public sealed record LampBaseState
{
    public static LampBaseState Default { get; } = new(ColorMode.Original, IntensityLevel.Percent100);
    public ColorMode Color { get; }
    public IntensityLevel Intensity { get; }
    public double IntensityFactor => (int)Intensity / 100.0;

    public LampBaseState(ColorMode color, IntensityLevel intensity)
    {
        LightLimits.EnumValue(color, nameof(color));
        LightLimits.EnumValue(intensity, nameof(intensity));
        Color = color;
        Intensity = intensity;
    }

    public LampBaseState NextColor() => new((ColorMode)(((int)Color + 1) % 11), Intensity);
}

/// <summary>
/// Caller-supplied, independently verified identity. Neither an item definition nor a
/// guessed object path is a persistent instance ID. This type does not discover game IDs.
/// </summary>
public sealed record PersistentLampId
{
    public PersistentIdentityKind Kind { get; }
    public string Scheme { get; }
    public int SchemeVersion { get; }
    public string Value { get; }
    public string CanonicalKey { get; }

    public PersistentLampId(PersistentIdentityKind kind, string scheme, int schemeVersion, string value)
    {
        LightLimits.EnumValue(kind, nameof(kind));
        if (schemeVersion < 1 || schemeVersion > 10000) throw new ArgumentOutOfRangeException(nameof(schemeVersion));
        Kind = kind;
        Scheme = LightLimits.Identifier(scheme, nameof(scheme));
        SchemeVersion = schemeVersion;
        Value = LightLimits.Identifier(value, nameof(value));
        // Encoding each textual field keeps distinct identities distinct even with separators.
        CanonicalKey = FormattableString.Invariant($"p:{(int)kind}:{Convert.ToBase64String(Encoding.UTF8.GetBytes(Scheme))}:{schemeVersion}:{Convert.ToBase64String(Encoding.UTF8.GetBytes(Value))}");
    }
}

public sealed record LampIdentity
{
    public string Key { get; }
    public PersistentLampId? PersistentId { get; }
    public Guid? SessionId { get; }

    private LampIdentity(string key, PersistentLampId? persistentId, Guid? sessionId)
    {
        Key = key;
        PersistentId = persistentId;
        SessionId = sessionId;
    }

    public static LampIdentity Persistent(PersistentLampId id)
    {
        if (id is null) throw new ArgumentNullException(nameof(id));
        return new(id.CanonicalKey, id, null);
    }

    /// <summary>The host/integration must assign the same unique token to matching mod clients.</summary>
    public static LampIdentity Ephemeral(Guid sessionId, string sharedInstanceToken)
    {
        LightLimits.Session(sessionId);
        LightLimits.Identifier(sharedInstanceToken, nameof(sharedInstanceToken));
        return new($"s:{sessionId:N}:{Convert.ToBase64String(Encoding.UTF8.GetBytes(sharedInstanceToken))}", null, sessionId);
    }

    internal void ValidateSession(Guid sessionId)
    {
        if (SessionId is Guid ownSession && ownSession != sessionId)
            throw new ArgumentException("Ephemeral lamp identity belongs to another session.");
    }
}

/// <summary>Pure metadata. A game adapter owns actual Unity references and capability checks.</summary>
public sealed record LampRegistration
{
    public LampIdentity Identity { get; }
    public string RuntimeInstanceId { get; }
    public string? PropertyId { get; }
    public string? GroupId { get; }
    public LampPresence Presence { get; }

    public LampRegistration(LampIdentity identity, string runtimeInstanceId, string? propertyId = null,
        string? groupId = null, LampPresence presence = LampPresence.Available)
    {
        Identity = identity ?? throw new ArgumentNullException(nameof(identity));
        RuntimeInstanceId = LightLimits.Identifier(runtimeInstanceId, nameof(runtimeInstanceId));
        PropertyId = LightLimits.OptionalIdentifier(propertyId, nameof(propertyId));
        GroupId = LightLimits.OptionalIdentifier(groupId, nameof(groupId));
        if (groupId is not null && propertyId is null)
            throw new ArgumentException("A switch/room group must be scoped to a known property.", nameof(groupId));
        LightLimits.EnumValue(presence, nameof(presence));
        Presence = presence;
    }

    internal LampRegistration Relocate(string? propertyId, string? groupId) =>
        new(Identity, RuntimeInstanceId, propertyId, groupId, Presence);

    internal LampRegistration SetPresence(LampPresence presence) =>
        new(Identity, RuntimeInstanceId, PropertyId, GroupId, presence);
}

public sealed record LampSelection
{
    public SelectionKind Kind { get; }
    public string? LampKey { get; }
    public string? PropertyId { get; }
    public string? GroupId { get; }

    private LampSelection(SelectionKind kind, string? key, string? propertyId, string? groupId)
    {
        Kind = kind;
        LampKey = key;
        PropertyId = propertyId;
        GroupId = groupId;
    }

    public static LampSelection Lamp(LampIdentity identity) =>
        new(SelectionKind.Lamp, (identity ?? throw new ArgumentNullException(nameof(identity))).Key, null, null);
    public static LampSelection Group(string propertyId, string groupId) =>
        new(SelectionKind.Group, null, LightLimits.Identifier(propertyId, nameof(propertyId)), LightLimits.Identifier(groupId, nameof(groupId)));
    public static LampSelection Property(string propertyId) =>
        new(SelectionKind.Property, null, LightLimits.Identifier(propertyId, nameof(propertyId)), null);
    public static LampSelection World() => new(SelectionKind.World, null, null, null);

    internal bool Includes(LampRegistration lamp) => Kind switch
    {
        SelectionKind.Lamp => lamp.Identity.Key == LampKey,
        SelectionKind.Group => lamp.PropertyId == PropertyId && lamp.GroupId == GroupId,
        SelectionKind.Property => lamp.PropertyId == PropertyId,
        SelectionKind.World => true,
        _ => false
    };
}

public sealed record LampChange
{
    public ColorMode? Color { get; }
    public IntensityLevel? Intensity { get; }

    public LampChange(ColorMode? color = null, IntensityLevel? intensity = null)
    {
        if (color is null && intensity is null) throw new ArgumentException("A state change needs at least one value.");
        if (color is ColorMode c) LightLimits.EnumValue(c, nameof(color));
        if (intensity is IntensityLevel i) LightLimits.EnumValue(i, nameof(intensity));
        Color = color;
        Intensity = intensity;
    }

    internal LampBaseState Apply(LampBaseState previous) => new(Color ?? previous.Color, Intensity ?? previous.Intensity);
}

public sealed record DiscoEffect
{
    public double StartTimeSeconds { get; }
    public double StepSeconds { get; }
    public ulong Seed { get; }

    public DiscoEffect(double startTimeSeconds, double stepSeconds, ulong seed)
    {
        StartTimeSeconds = LightLimits.Time(startTimeSeconds, nameof(startTimeSeconds));
        if (!double.IsFinite(stepSeconds) || stepSeconds < LightLimits.MinDiscoStepSeconds || stepSeconds > LightLimits.MaxDiscoStepSeconds)
            throw new ArgumentOutOfRangeException(nameof(stepSeconds));
        StepSeconds = stepSeconds;
        Seed = seed;
    }
}

public sealed class EffectState
{
    public DiscoEffect? World { get; }
    public IReadOnlyDictionary<string, DiscoEffect> Properties { get; }
    public static EffectState Empty { get; } = new(null, Array.Empty<KeyValuePair<string, DiscoEffect>>());

    public EffectState(DiscoEffect? world, IEnumerable<KeyValuePair<string, DiscoEffect>> properties)
    {
        var entries = LightLimits.Copy(properties, LightLimits.MaxPropertyEffects, nameof(properties));
        var values = new Dictionary<string, DiscoEffect>(StringComparer.Ordinal);
        foreach (var entry in entries)
        {
            LightLimits.Identifier(entry.Key, nameof(properties));
            if (entry.Value is null || !values.TryAdd(entry.Key, entry.Value))
                throw new ArgumentException("Duplicate property or null effect.", nameof(properties));
        }
        World = world;
        Properties = new ReadOnlyDictionary<string, DiscoEffect>(values);
    }

    internal EffectState Change(LampSelection selection, DiscoEffect? effect)
    {
        if (selection.Kind == SelectionKind.World) return new(effect, Properties);
        if (selection.Kind != SelectionKind.Property) throw new ArgumentException("Disco requires a world or property scope.");
        var properties = Properties.ToDictionary(item => item.Key, item => item.Value, StringComparer.Ordinal);
        if (effect is null) properties.Remove(selection.PropertyId!);
        else properties[selection.PropertyId!] = effect;
        return new(World, properties);
    }

    public DiscoEffect? Resolve(string? propertyId) =>
        propertyId is not null && Properties.TryGetValue(propertyId, out var property) ? property : World;
}
