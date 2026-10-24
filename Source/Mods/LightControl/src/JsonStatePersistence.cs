using System.Text;
using System.Text.Json;

namespace LightControl.Core;

public sealed record PersistedLampState
{
    public PersistentLampId Identity { get; }
    public LampBaseState State { get; }

    public PersistedLampState(PersistentLampId identity, LampBaseState state)
    {
        Identity = identity ?? throw new ArgumentNullException(nameof(identity));
        State = state ?? throw new ArgumentNullException(nameof(state));
    }
}

public enum PersistenceGate { NotLoaded, Ready, Blocked }
public enum PersistenceError { None, InvalidJson, UnsupportedSchema, WrongSave, InvalidState, AlreadyLoaded, WrongSession }
public sealed record PersistenceLoadResult(bool Success, PersistenceError Error, string Detail);

/// <summary>
/// Classification of one JSON document against this save, produced without touching the store
/// or the write gate. The load policy needs this to tell "corrupt" (backup recovery may be
/// attempted) apart from "unknown schema" or "foreign save" (the data is intact and must be
/// retained, so no backup is ever substituted).
/// </summary>
public enum PersistenceDataStatus { Valid = 0, CorruptJson = 1, UnsupportedSchema = 2, WrongSave = 3, InvalidState = 4 }

/// <summary>Pure inspection result: validated states only when <see cref="Status"/> is Valid.</summary>
public sealed record PersistenceDataCheck(PersistenceDataStatus Status, string Detail, IReadOnlyList<PersistedLampState> States)
{
    public static PersistenceDataCheck Invalid(PersistenceDataStatus status, string detail) =>
        new(status, detail, Array.Empty<PersistedLampState>());

    /// <summary>The gate error this classification maps to; <see cref="PersistenceError.None"/> only when valid.</summary>
    public PersistenceError Error => Status switch
    {
        PersistenceDataStatus.Valid => PersistenceError.None,
        PersistenceDataStatus.UnsupportedSchema => PersistenceError.UnsupportedSchema,
        PersistenceDataStatus.WrongSave => PersistenceError.WrongSave,
        PersistenceDataStatus.CorruptJson => PersistenceError.InvalidJson,
        _ => PersistenceError.InvalidState
    };
}

/// <summary>
/// Versioned JSON DATA API. It deliberately does not perform disk I/O or use a guessed
/// S1API hook. A verified save integration must read the exact current save, distinguish
/// missing data from read failure, then persist the complete generated string with its
/// own confirmed atomic-write/backup/recovery contract. Do not pass null on read errors.
/// </summary>
public sealed class JsonStatePersistence
{
    private readonly object _gate = new();
    private readonly LightStateStore _store;
    private readonly Guid _sessionId;
    private readonly string _saveId;
    private PersistenceGate _state;

    public PersistenceGate Gate { get { lock (_gate) return _state; } }

    public JsonStatePersistence(LightStateStore store)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        if (store.Mode != SessionMode.Host)
            throw new InvalidOperationException("Local and client sessions have no persistent lamp database.");
        _sessionId = store.SessionId;
        _saveId = store.SaveId;
    }

    /// <summary>Call only after the storage provider positively confirmed absence, never after an I/O error.</summary>
    public PersistenceLoadResult ConfirmDataMissing()
    {
        lock (_gate)
        {
            if (_state == PersistenceGate.Ready)
                return new(false, PersistenceError.AlreadyLoaded, "Initial load already completed; the valid write gate is retained.");
            if (_state != PersistenceGate.NotLoaded)
                return Fail(PersistenceError.AlreadyLoaded, "A failed or completed load cannot be replaced by declaring missing data.");
            return Restore(Array.Empty<PersistedLampState>());
        }
    }

    /// <summary>Use when the provider cannot read data. No serialization is permitted afterwards.</summary>
    public PersistenceLoadResult ReportReadFailure()
        => ReportLoadFailure(PersistenceError.InvalidState, "Storage read failed. Lamp-data writes are blocked.");

    /// <summary>
    /// Blocks the write gate with a caller-supplied classification and detail, so the adapter can
    /// preserve the exact reason (unsupported schema, foreign save, unreadable backup) instead of
    /// collapsing every refusal into one message. An explicit read failure always blocks, even
    /// after a successful load: continuing to write over storage that just failed a read would
    /// risk losing the only good copy.
    /// </summary>
    public PersistenceLoadResult ReportLoadFailure(PersistenceError error, string detail)
    {
        lock (_gate)
            return Fail(error == PersistenceError.None ? PersistenceError.InvalidState : error, detail);
    }

    /// <summary>A failed initial load may be retried with actually recovered valid JSON.</summary>
    public PersistenceLoadResult LoadJson(string json)
    {
        lock (_gate)
        {
            if (_state == PersistenceGate.Ready)
                return new(false, PersistenceError.AlreadyLoaded, "Initial load already completed; no state was replaced and the valid write gate is retained.");
            var check = Inspect(json, _saveId);
            return check.Status == PersistenceDataStatus.Valid
                ? Restore(check.States)
                : Fail(check.Error, check.Detail);
        }
    }

    /// <summary>
    /// Classifies one JSON document against <paramref name="expectedSaveId"/> without touching the
    /// store or the write gate. The three "intact but not ours" outcomes (unsupported schema,
    /// foreign save) are reported separately from corruption so the load policy can refuse to
    /// substitute a backup for data it must retain.
    /// </summary>
    public static PersistenceDataCheck Inspect(string json, string expectedSaveId)
    {
        if (expectedSaveId is null) throw new ArgumentNullException(nameof(expectedSaveId));
        if (json is null || json.Length > LightLimits.MaxJsonBytes || Encoding.UTF8.GetByteCount(json) > LightLimits.MaxJsonBytes)
            return PersistenceDataCheck.Invalid(PersistenceDataStatus.CorruptJson, "JSON is absent or exceeds the size limit.");
        try
        {
            using var document = JsonDocument.Parse(json, new JsonDocumentOptions
            {
                AllowTrailingCommas = false,
                CommentHandling = JsonCommentHandling.Disallow,
                MaxDepth = 16
            });
            var root = document.RootElement;
            RequireUniqueObject(root);
            int schema = RequiredInt(root, "schemaVersion");
            if (schema != LightLimits.SaveSchemaVersion)
                return PersistenceDataCheck.Invalid(PersistenceDataStatus.UnsupportedSchema,
                    $"Unsupported save schema {schema}. Existing data must be retained.");
            string saveId = RequiredString(root, "saveId");
            LightLimits.Identifier(saveId, "saveId");
            if (!string.Equals(saveId, expectedSaveId, StringComparison.Ordinal))
                return PersistenceDataCheck.Invalid(PersistenceDataStatus.WrongSave, "Saved lamp data belongs to another game save.");
            RequireProperties(root, "schemaVersion", "saveId", "lamps");
            var lamps = root.GetProperty("lamps");
            if (lamps.ValueKind != JsonValueKind.Array || lamps.GetArrayLength() > LightLimits.MaxLamps)
                return PersistenceDataCheck.Invalid(PersistenceDataStatus.InvalidState, "Lamp collection is invalid or exceeds the capacity limit.");
            var states = new List<PersistedLampState>();
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var lamp in lamps.EnumerateArray())
            {
                RequireProperties(lamp, "identity", "colorMode", "intensityPercent");
                var identity = lamp.GetProperty("identity");
                RequireProperties(identity, "kind", "scheme", "schemeVersion", "value");
                var persistent = new PersistentLampId((PersistentIdentityKind)RequiredInt(identity, "kind"),
                    RequiredString(identity, "scheme"), RequiredInt(identity, "schemeVersion"), RequiredString(identity, "value"));
                var state = new LampBaseState((ColorMode)RequiredInt(lamp, "colorMode"),
                    (IntensityLevel)RequiredInt(lamp, "intensityPercent"));
                if (!seen.Add(persistent.CanonicalKey))
                    return PersistenceDataCheck.Invalid(PersistenceDataStatus.InvalidState, "Duplicate persistent lamp identity.");
                states.Add(new(persistent, state));
            }
            return new(PersistenceDataStatus.Valid, "Lamp data passed schema, save-id and content validation.", states);
        }
        catch (JsonException)
        {
            return PersistenceDataCheck.Invalid(PersistenceDataStatus.CorruptJson, "Malformed JSON. Lamp-data writes are blocked.");
        }
        catch (Exception exception) when (exception is ArgumentException or InvalidOperationException or KeyNotFoundException or FormatException or OverflowException)
        {
            return PersistenceDataCheck.Invalid(PersistenceDataStatus.InvalidState, "Invalid or incomplete save data. Lamp-data writes are blocked.");
        }
    }

    private PersistenceLoadResult Restore(IReadOnlyList<PersistedLampState> states)
    {
        try
        {
            _store.RestorePersisted(_sessionId, _saveId, states);
            _state = PersistenceGate.Ready;
            return new(true, PersistenceError.None, "Lamp data is initialized for this host session.");
        }
        catch (InvalidOperationException)
        {
            return Fail(PersistenceError.WrongSession, "Host/session/save does not match or lamp registration already started.");
        }
        catch (ArgumentException)
        {
            return Fail(PersistenceError.InvalidState, "Persistent data failed final validation.");
        }
    }

    private PersistenceLoadResult Fail(PersistenceError error, string detail)
    {
        _state = PersistenceGate.Blocked;
        return new(false, error, detail);
    }

    /// <summary>
    /// Returns complete JSON only; no write has occurred. The caller must retain the old
    /// valid file if this method or the subsequent verified storage operation fails.
    /// </summary>
    public string CreateJsonForSave()
    {
        lock (_gate)
        {
            if (_state != PersistenceGate.Ready)
                throw new InvalidOperationException("Lamp-data writes are blocked until initial data was loaded successfully or positively confirmed absent.");
            var values = _store.ExportPersisted(_sessionId, _saveId);
            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
            {
                writer.WriteStartObject();
                writer.WriteNumber("schemaVersion", LightLimits.SaveSchemaVersion);
                writer.WriteString("saveId", _saveId);
                writer.WriteStartArray("lamps");
                foreach (var item in values)
                {
                    writer.WriteStartObject();
                    writer.WriteStartObject("identity");
                    writer.WriteNumber("kind", (int)item.Identity.Kind);
                    writer.WriteString("scheme", item.Identity.Scheme);
                    writer.WriteNumber("schemeVersion", item.Identity.SchemeVersion);
                    writer.WriteString("value", item.Identity.Value);
                    writer.WriteEndObject();
                    writer.WriteNumber("colorMode", (int)item.State.Color);
                    writer.WriteNumber("intensityPercent", (int)item.State.Intensity);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            if (stream.Length > LightLimits.MaxJsonBytes)
                throw new InvalidOperationException("Serialized lamp data exceeds the supported size. Existing storage must remain untouched.");
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }

    private static int RequiredInt(JsonElement value, string name)
    {
        var field = value.GetProperty(name);
        if (field.ValueKind != JsonValueKind.Number || !field.TryGetInt32(out int result))
            throw new FormatException("Integer field required.");
        return result;
    }

    private static string RequiredString(JsonElement value, string name)
    {
        var field = value.GetProperty(name);
        if (field.ValueKind != JsonValueKind.String) throw new FormatException("String field required.");
        return field.GetString() ?? throw new FormatException("String field is null.");
    }

    private static void RequireUniqueObject(JsonElement value)
    {
        if (value.ValueKind != JsonValueKind.Object) throw new FormatException("Object required.");
        var properties = new HashSet<string>(StringComparer.Ordinal);
        foreach (var property in value.EnumerateObject())
            if (!properties.Add(property.Name)) throw new FormatException("Duplicate JSON property.");
    }

    private static void RequireProperties(JsonElement value, params string[] expected)
    {
        RequireUniqueObject(value);
        var actual = value.EnumerateObject().Select(property => property.Name).ToHashSet(StringComparer.Ordinal);
        if (!actual.SetEquals(expected)) throw new FormatException("Unexpected or missing JSON property.");
    }
}
