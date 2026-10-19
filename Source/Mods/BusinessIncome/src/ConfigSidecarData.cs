using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BusinessIncome.Config;

/// <summary>
/// Pure validation helpers for the collection-shaped config values
/// (multipliers, display-name overrides, weekend categories).
/// Unity-free / MelonLoader-free so it links into the pure test project.
///
/// Rules (chosen deliberately, documented so they are not "magic"):
/// - multiplier: must be finite and &gt; 0; values above <see cref="MaxMultiplier"/> are
///   clamped to the cap (kept, because a multiplier is still representable) while
///   non-finite / non-positive values drop the entry (the runtime ignores &lt;= 0 and
///   NaN poisons the economy math).
/// - business id / keys: non-empty, at most <see cref="MaxIdLength"/> chars,
///   restricted to <c>[A-Za-z0-9_]</c> (the shape produced by BusinessResolver.NormalizeId).
/// - display name: non-empty after trim, at most <see cref="MaxDisplayNameLength"/> chars,
///   no control characters and no '&lt;' / '&gt;' (validated, not escaped: a name that would
///   need TMP rich-text escaping is treated as an authoring error and dropped).
/// - weekend categories: normalized to lower-case and de-duplicated case-insensitively.
/// </summary>
public static class ConfigCollectionSanitizer
{
    public const float MaxMultiplier = 100f;
    public const int MaxIdLength = 64;
    public const int MaxDisplayNameLength = 64;

    public static bool IsSafeId(string? id)
    {
        if (string.IsNullOrEmpty(id) || id.Length > MaxIdLength)
            return false;

        foreach (char c in id)
        {
            bool ok = (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_';
            if (!ok) return false;
        }
        return true;
    }

    public static bool IsSafeDisplayName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > MaxDisplayNameLength)
            return false;

        foreach (char c in name)
        {
            if (char.IsControl(c) || c == '<' || c == '>')
                return false;
        }
        return true;
    }

    /// <summary>
    /// Escapes the TMP rich-text significant characters (&amp;, &lt;, &gt;) so an UNVALIDATED
    /// string - most importantly a source business display name that comes from the game or a
    /// hand-edited sidecar rather than from a validated override - cannot break or inject markup
    /// into a console line that is already wrapped in &lt;color=...&gt; tags. Validated override
    /// names never contain these characters; this is the escape for the rest.
    /// </summary>
    public static string EscapeForConsole(string? value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
    }

    public static Dictionary<string, float> SanitizeMultipliers(Dictionary<string, float>? src, Action<string>? warn)
    {
        var result = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
        bool changed = src == null || src.Comparer != StringComparer.OrdinalIgnoreCase;

        if (src != null)
        {
            foreach (var kv in src)
            {
                if (!IsSafeId(kv.Key))
                {
                    warn?.Invoke($"Dropped multiplier for unsafe business id '{EscapeForConsole(kv.Key)}'.");
                    changed = true;
                    continue;
                }

                float value = kv.Value;
                if (!float.IsFinite(value) || value <= 0f)
                {
                    warn?.Invoke($"Dropped multiplier '{kv.Key}'={value}: must be finite and > 0.");
                    changed = true;
                    continue;
                }

                if (value > MaxMultiplier)
                {
                    warn?.Invoke($"Multiplier '{kv.Key}'={value} exceeds cap {MaxMultiplier} - clamped.");
                    value = MaxMultiplier;
                    changed = true;
                }

                result[kv.Key] = value;
            }

            if (!changed)
                return src;
        }

        return result;
    }

    public static Dictionary<string, string> SanitizeDisplayNames(Dictionary<string, string>? src, Action<string>? warn)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        bool changed = src == null || src.Comparer != StringComparer.OrdinalIgnoreCase;

        if (src != null)
        {
            foreach (var kv in src)
            {
                if (!IsSafeId(kv.Key))
                {
                    warn?.Invoke($"Dropped display name for unsafe business id '{EscapeForConsole(kv.Key)}'.");
                    changed = true;
                    continue;
                }

                string name = (kv.Value ?? string.Empty).Trim();
                if (!IsSafeDisplayName(name))
                {
                    warn?.Invoke($"Dropped display name for '{kv.Key}': empty, over length, or contains control/rich-text characters.");
                    changed = true;
                    continue;
                }

                if (!string.Equals(name, kv.Value, StringComparison.Ordinal))
                    changed = true;

                result[kv.Key] = name;
            }

            if (!changed)
                return src;
        }

        return result;
    }

    public static List<string> SanitizeWeekendCategories(List<string>? src, Action<string>? warn)
    {
        var result = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        bool changed = src == null;

        if (src != null)
        {
            foreach (string? raw in src)
            {
                string id = (raw ?? string.Empty).Trim().ToLowerInvariant();
                if (!IsSafeId(id))
                {
                    warn?.Invoke($"Dropped weekend category '{EscapeForConsole(raw)}': not a safe business id.");
                    changed = true;
                    continue;
                }

                if (!seen.Add(id))
                {
                    warn?.Invoke($"Dropped duplicate weekend category '{EscapeForConsole(raw)}'.");
                    changed = true;
                    continue;
                }

                if (!string.Equals(raw, id, StringComparison.Ordinal))
                    changed = true;

                result.Add(id);
            }

            if (!changed && result.Count == src.Count)
                return src;
        }

        return result;
    }
}

/// <summary>
/// Pure sidecar DTO for the config values that MelonPreferences/TOML cannot map.
/// Carries ONLY the collections plus a schema marker - scalars stay in TOML, so the
/// sidecar can never silently override a scalar (no duplicated scalar fields).
/// Every collection is nullable so "missing in JSON" is distinguishable from "empty in JSON":
/// null means "keep the existing default", empty means "explicitly cleared".
/// </summary>
public sealed class ConfigSidecarData
{
    /// <summary>Schema emitted by this build. Absent schema == legacy full-config sidecar.</summary>
    public const int CurrentSchemaVersion = 1;

    /// <summary>Serializer options for the sidecar (tolerant read, indented, no null noise).</summary>
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public int? SchemaVersion { get; set; }
    public Dictionary<string, float>? PropertyMultipliers { get; set; }
    public Dictionary<string, string>? DisplayNameOverrides { get; set; }
    public List<string>? WeekendBonusCategories { get; set; }

    /// <summary>
    /// True when the file declares a schema this build does not understand. The caller must
    /// warn and must NOT overwrite/apply such a file without an explicit decision.
    /// </summary>
    public bool HasUnsupportedSchema => SchemaVersion.HasValue && SchemaVersion.Value != CurrentSchemaVersion;

    private static readonly JsonDocumentOptions ParseOptions = new()
    {
        AllowTrailingCommas = true,
        CommentHandling = JsonCommentHandling.Skip,
    };

    /// <summary>Snapshot of the config's collection values, stamped with the current schema.</summary>
    public static ConfigSidecarData FromConfig(BusinessIncomeConfig cfg)
    {
        if (cfg == null) throw new ArgumentNullException(nameof(cfg));
        cfg.Sanitize();

        return new ConfigSidecarData
        {
            SchemaVersion = CurrentSchemaVersion,
            PropertyMultipliers = new Dictionary<string, float>(cfg.PropertyMultipliers, StringComparer.OrdinalIgnoreCase),
            DisplayNameOverrides = new Dictionary<string, string>(cfg.DisplayNameOverrides, StringComparer.OrdinalIgnoreCase),
            WeekendBonusCategories = new List<string>(cfg.WeekendBonusCategories),
        };
    }

    /// <summary>Validates/cleans the present collections in place (per-entry warnings).</summary>
    public void Sanitize(Action<string>? warn = null)
    {
        if (PropertyMultipliers != null)
            PropertyMultipliers = ConfigCollectionSanitizer.SanitizeMultipliers(PropertyMultipliers, warn);
        if (DisplayNameOverrides != null)
            DisplayNameOverrides = ConfigCollectionSanitizer.SanitizeDisplayNames(DisplayNameOverrides, warn);
        if (WeekendBonusCategories != null)
            WeekendBonusCategories = ConfigCollectionSanitizer.SanitizeWeekendCategories(WeekendBonusCategories, warn);
    }

    /// <summary>
    /// Copies the PRESENT (non-null) collections onto <paramref name="cfg"/> as fresh,
    /// case-insensitive containers. A null field leaves the existing default untouched; an
    /// empty field clears the collection. Collections are copied, never aliased.
    /// </summary>
    public void ApplyTo(BusinessIncomeConfig cfg, Action<string>? warn = null)
    {
        if (cfg == null) throw new ArgumentNullException(nameof(cfg));
        Sanitize(warn);

        if (PropertyMultipliers != null)
            cfg.PropertyMultipliers = new Dictionary<string, float>(PropertyMultipliers, StringComparer.OrdinalIgnoreCase);
        if (DisplayNameOverrides != null)
            cfg.DisplayNameOverrides = new Dictionary<string, string>(DisplayNameOverrides, StringComparer.OrdinalIgnoreCase);
        if (WeekendBonusCategories != null)
            cfg.WeekendBonusCategories = new List<string>(WeekendBonusCategories);
    }

    /// <summary>
    /// Tolerant parse. Returns false ONLY for structurally invalid JSON / a non-object root;
    /// a bad individual entry (wrong type, float overflow, non-string array element) is
    /// reported through <paramref name="warn"/> and dropped, never rejects the whole sidecar.
    /// Unknown scalar fields (a legacy full-config sidecar) are ignored; the collections are
    /// still honored. Missing and explicit-null fields both leave the property null so the
    /// caller can distinguish them from an empty collection.
    /// </summary>
    public static bool TryParse(string? json, out ConfigSidecarData? data, out string? error, Action<string>? warn = null)
    {
        data = null;
        error = null;

        if (string.IsNullOrWhiteSpace(json))
        {
            error = "sidecar JSON was empty";
            return false;
        }

        JsonDocument doc;
        try
        {
            doc = JsonDocument.Parse(json, ParseOptions);
        }
        catch (JsonException ex)
        {
            error = "invalid JSON: " + ex.Message;
            return false;
        }

        using (doc)
        {
            JsonElement root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
            {
                error = "sidecar root must be a JSON object";
                return false;
            }

            var result = new ConfigSidecarData();

            if (TryGetPropertyIgnoreCase(root, "SchemaVersion", out JsonElement schema))
            {
                if (schema.ValueKind == JsonValueKind.Number && schema.TryGetInt32(out int v))
                {
                    result.SchemaVersion = v;
                }
                else if (schema.ValueKind != JsonValueKind.Null)
                {
                    warn?.Invoke($"Ignoring non-integer SchemaVersion ({schema.ValueKind}).");
                }
            }

            result.PropertyMultipliers = ReadNumberMap(root, "PropertyMultipliers", warn);
            result.DisplayNameOverrides = ReadStringMap(root, "DisplayNameOverrides", warn);
            result.WeekendBonusCategories = ReadStringArray(root, "WeekendBonusCategories", warn);

            data = result;
            return true;
        }
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement obj, string name, out JsonElement value)
    {
        foreach (JsonProperty p in obj.EnumerateObject())
        {
            if (string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                value = p.Value;
                return true;
            }
        }
        value = default;
        return false;
    }

    private static Dictionary<string, float>? ReadNumberMap(JsonElement root, string name, Action<string>? warn)
    {
        if (!TryGetPropertyIgnoreCase(root, name, out JsonElement el))
            return null;
        if (el.ValueKind == JsonValueKind.Null)
            return null;
        if (el.ValueKind != JsonValueKind.Object)
        {
            warn?.Invoke($"{name} must be a JSON object - ignored (default kept).");
            return null;
        }

        var map = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
        foreach (JsonProperty p in el.EnumerateObject())
        {
            if (p.Value.ValueKind != JsonValueKind.Number)
            {
                warn?.Invoke($"Ignored {name}['{p.Name}']: not a number.");
                continue;
            }

            float value;
            try
            {
                value = p.Value.GetSingle();
            }
            catch (FormatException)
            {
                warn?.Invoke($"Ignored {name}['{p.Name}']: value outside the float range.");
                continue;
            }
            catch (InvalidOperationException)
            {
                warn?.Invoke($"Ignored {name}['{p.Name}']: not a number.");
                continue;
            }

            if (!float.IsFinite(value))
            {
                warn?.Invoke($"Ignored {name}['{p.Name}']: value outside the float range.");
                continue;
            }

            map[p.Name] = value;
        }
        return map;
    }

    private static Dictionary<string, string>? ReadStringMap(JsonElement root, string name, Action<string>? warn)
    {
        if (!TryGetPropertyIgnoreCase(root, name, out JsonElement el))
            return null;
        if (el.ValueKind == JsonValueKind.Null)
            return null;
        if (el.ValueKind != JsonValueKind.Object)
        {
            warn?.Invoke($"{name} must be a JSON object - ignored (default kept).");
            return null;
        }

        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (JsonProperty p in el.EnumerateObject())
        {
            if (p.Value.ValueKind != JsonValueKind.String)
            {
                warn?.Invoke($"Ignored {name}['{p.Name}']: not a string.");
                continue;
            }
            map[p.Name] = p.Value.GetString()!;
        }
        return map;
    }

    private static List<string>? ReadStringArray(JsonElement root, string name, Action<string>? warn)
    {
        if (!TryGetPropertyIgnoreCase(root, name, out JsonElement el))
            return null;
        if (el.ValueKind == JsonValueKind.Null)
            return null;
        if (el.ValueKind != JsonValueKind.Array)
        {
            warn?.Invoke($"{name} must be a JSON array - ignored (default kept).");
            return null;
        }

        var list = new List<string>();
        foreach (JsonElement item in el.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.String)
            {
                warn?.Invoke($"Ignored {name} element: not a string.");
                continue;
            }
            list.Add(item.GetString()!);
        }
        return list;
    }
}

/// <summary>Outcome of reading the JSON sidecar - distinct from a parse error.</summary>
public enum SidecarLoadStatus
{
    /// <summary>No sidecar and no backup exist; defaults apply.</summary>
    Missing,
    /// <summary>The main sidecar parsed and its schema is supported.</summary>
    Valid,
    /// <summary>The main sidecar was absent/unreadable/unsupported and a supported backup was used.</summary>
    Backup,
    /// <summary>A file exists but declares a schema this build does not understand; preserved, not applied.</summary>
    UnsupportedSchema,
    /// <summary>Files exist but none could be parsed; defaults preserved.</summary>
    Unreadable,
}

/// <summary>
/// Pure classification of sidecar file state, separated from file I/O so the
/// missing / valid / backup / unsupported-schema / unreadable decision is unit-testable
/// without touching the filesystem.
/// </summary>
public static class SidecarLoad
{
    public static SidecarLoadStatus Classify(
        bool mainExists, string? mainJson,
        bool backupExists, string? backupJson,
        out ConfigSidecarData? data,
        Action<string>? warn = null)
    {
        data = null;

        if (!mainExists && !backupExists)
            return SidecarLoadStatus.Missing;

        ConfigSidecarData? mainData = null;
        bool mainParsed = mainExists && mainJson != null
            && ConfigSidecarData.TryParse(mainJson, out mainData, out _, warn);
        if (mainParsed && !mainData!.HasUnsupportedSchema)
        {
            data = mainData;
            return SidecarLoadStatus.Valid;
        }

        ConfigSidecarData? backupData = null;
        bool backupParsed = backupExists && backupJson != null
            && ConfigSidecarData.TryParse(backupJson, out backupData, out _, warn);
        if (backupParsed && !backupData!.HasUnsupportedSchema)
        {
            data = backupData;
            return SidecarLoadStatus.Backup;
        }

        if (mainParsed)
        {
            data = mainData;
            return SidecarLoadStatus.UnsupportedSchema;
        }
        if (backupParsed)
        {
            data = backupData;
            return SidecarLoadStatus.UnsupportedSchema;
        }

        return SidecarLoadStatus.Unreadable;
    }
}
