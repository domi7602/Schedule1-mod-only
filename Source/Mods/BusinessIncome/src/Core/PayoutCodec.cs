using System;
using System.Collections.Generic;
using System.Text.Json;
using BusinessIncome.Models;

namespace BusinessIncome.Core;

/// <summary>
/// Pure JSON encode/decode + validation for the payout state and the write-ahead marker.
/// All validation fails closed: any structural problem yields Corrupt/SchemaMismatch/
/// IdentityMismatch and the caller must NOT silently seed fresh state. The single tolerated
/// gap is a MISSING SchemaVersion on an otherwise valid file - that is a pre-schema (v0) file
/// from a build that predates the marker; it is accepted as legacy and upgraded on disk by
/// <see cref="MigrateLegacySchema"/>.
/// </summary>
public static class PayoutCodec
{
    public const int CurrentSchemaVersion = 1;

    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public static string EncodeState(PayoutState state) => JsonSerializer.Serialize(state, Options);
    public static string EncodePending(PendingPayoutState marker) => JsonSerializer.Serialize(marker, Options);

    /// <summary>
    /// Reads/validates the payout state at <paramref name="path"/>.
    /// Fail closed: corrupt main + absent/invalid backup => Corrupt (no fresh seed);
    /// identity mismatch => IdentityMismatch; schema drift => SchemaMismatch;
    /// null/absent dictionary or an out-of-range day => Corrupt.
    /// A PRE-SCHEMA (v0) file - no SchemaVersion property but an otherwise valid required
    /// shape - is accepted as legacy (Ok); it is upgraded on disk by
    /// <see cref="MigrateLegacySchema"/>. A schema that is PRESENT but wrong stays SchemaMismatch.
    /// </summary>
    public static StateLoadResult DecodeState(IPayoutStorage storage, string path, string expectedIdentity)
    {
        bool mainExists = storage.Exists(path);
        string? main = storage.ReadText(path);

        if (main != null)
        {
            var parsed = TryParseState(main, expectedIdentity, out string why, out StateStatus status, out bool preSchema);
            if (parsed != null)
                return new StateLoadResult(StateStatus.Ok, parsed,
                    preSchema ? "ok - pre-schema state accepted, on-disk upgrade pending" : "ok");
            // Main present but invalid: only a VALID backup may recover it.
            string? backupText = storage.BackupExists(path) ? storage.ReadText(path + ".bak") : null;
            if (backupText != null)
            {
                var recovered = TryParseState(backupText, expectedIdentity, out _, out _, out _);
                if (recovered != null)
                    return new StateLoadResult(StateStatus.RecoveredFromBackup, recovered, $"main invalid ({why}), backup recovered");
            }
            return new StateLoadResult(status, null, why);
        }

        if (mainExists)
            return new StateLoadResult(StateStatus.Corrupt, null, "state file unreadable");

        // Main absent: distinguish a genuinely fresh slot from a broken/partial one. A
        // surviving backup or staging artifact means persistence was interrupted — fail closed
        // instead of silently seeding fresh state over a payout that may already have run.
        if (storage.BackupExists(path))
        {
            string? backupText = storage.ReadText(path + ".bak");
            if (backupText == null)
                return new StateLoadResult(StateStatus.Corrupt, null, "main missing, backup unreadable");
            var recovered = TryParseState(backupText, expectedIdentity, out string why, out _, out _);
            return recovered != null
                ? new StateLoadResult(StateStatus.RecoveredFromBackup, recovered, "main missing, backup recovered")
                : new StateLoadResult(StateStatus.Corrupt, null, $"main missing, backup invalid: {why}");
        }
        if (storage.Exists(path + ".tmp"))
            return new StateLoadResult(StateStatus.Corrupt, null, "main missing but a staging .tmp artifact remains");

        return new StateLoadResult(StateStatus.Missing, null, "no state file");
    }

    private static PayoutState? TryParseState(string json, string expectedIdentity, out string why, out StateStatus status, out bool preSchema)
    {
        why = "";
        status = StateStatus.Corrupt;
        preSchema = false;

        // Structural gate: an object that lacks the required properties (e.g. "{}") is corrupt,
        // never a valid day-0 state. A MISSING SchemaVersion is the one tolerated gap - it marks
        // a pre-schema (v0) file from a build that predates the schema marker, which is accepted
        // here and upgraded on disk by MigrateLegacySchema. A PRESENT-but-wrong schema stays
        // SchemaMismatch below, so unknown future schemas still fail closed.
        if (!HasRequiredStateShape(json, out bool missingSchema, out string shapeWhy)) { why = shapeWhy; return null; }

        PayoutState? state;
        try { state = JsonSerializer.Deserialize<PayoutState>(json, Options); }
        catch (Exception ex) { why = $"parse error: {ex.Message}"; return null; }

        if (state == null) { why = "null state"; return null; }
        if (missingSchema)
        {
            // Pre-schema file: no property to read, so the value is ASSERTED, never taken from
            // disk (HasRequiredStateShape already proved no case-variant key exists).
            state.SchemaVersion = CurrentSchemaVersion;
        }
        else if (state.SchemaVersion != CurrentSchemaVersion) { why = $"schema {state.SchemaVersion} != {CurrentSchemaVersion}"; status = StateStatus.SchemaMismatch; return null; }
        if (state.LastPaidDayByBusiness == null) { why = "null per-business dictionary"; return null; }
        if (state.LastPaidElapsedDay < -1) { why = $"invalid last-paid day {state.LastPaidElapsedDay}"; return null; }
        if (!string.IsNullOrEmpty(state.SaveIdentity)
            && !string.IsNullOrEmpty(expectedIdentity)
            && !string.Equals(state.SaveIdentity, expectedIdentity, StringComparison.OrdinalIgnoreCase))
        { why = $"identity '{state.SaveIdentity}' != slot '{expectedIdentity}'"; status = StateStatus.IdentityMismatch; return null; }

        foreach (var kv in state.LastPaidDayByBusiness)
        {
            if (kv.Key == null || kv.Value < -1) { why = "invalid per-business entry"; return null; }
        }
        preSchema = missingSchema;
        return state;
    }

    /// <summary>
    /// Required-field check on the raw JSON. SaveIdentity may be an empty string (legacy
    /// compatibility) but the property itself must be present; the per-business dictionary
    /// must be present and non-null. SchemaVersion is OPTIONAL: its absence marks a pre-schema
    /// (v0) file and is reported through <paramref name="missingSchema"/> instead of failing the
    /// gate. It is matched case-insensitively (the serializer options are case-insensitive) so a
    /// differently-cased schema marker can never masquerade as a missing one.
    /// </summary>
    private static bool HasRequiredStateShape(string json, out bool missingSchema, out string reason)
    {
        missingSchema = false;
        reason = "";
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Object) { reason = "state root is not an object"; return false; }
            JsonElement root = doc.RootElement;
            missingSchema = !HasProperty(root, "SchemaVersion");
            if (!root.TryGetProperty("LastPaidElapsedDay", out _)) { reason = "missing LastPaidElapsedDay"; return false; }
            if (!root.TryGetProperty("SaveIdentity", out _)) { reason = "missing SaveIdentity"; return false; }
            if (!root.TryGetProperty("LastPaidDayByBusiness", out JsonElement dict) || dict.ValueKind == JsonValueKind.Null)
            { reason = "missing or null LastPaidDayByBusiness"; return false; }
            return true;
        }
        catch (JsonException ex) { reason = $"parse error: {ex.Message}"; return false; }
    }

    private static bool HasProperty(JsonElement obj, string name)
    {
        foreach (JsonProperty property in obj.EnumerateObject())
        {
            if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }

    /// <summary>
    /// One-shot, idempotent upgrade of a pre-schema (v0) payout state to
    /// <see cref="CurrentSchemaVersion"/>. The file is fully validated with the same rules as
    /// <see cref="DecodeState"/> BEFORE anything is written; the original bytes are then written
    /// to <c>&lt;path&gt;.bak</c> FIRST and only afterwards is the re-encoded state written
    /// atomically, so a failed upgrade can neither lose nor corrupt the original. A file that
    /// already carries a schema (whatever its value), an unreadable file, a structurally invalid
    /// file and a file whose identity does not match <paramref name="expectedIdentity"/> are
    /// NEVER rewritten.
    /// </summary>
    public static SchemaMigrationResult MigrateLegacySchema(IPayoutStorage storage, string path, string expectedIdentity)
    {
        bool exists = storage.Exists(path);
        string? json = storage.ReadText(path);
        if (json == null)
            return new SchemaMigrationResult(exists ? SchemaMigrationStatus.NotLegacy : SchemaMigrationStatus.MissingFile,
                exists ? "state file unreadable" : "no state file");

        if (!HasRequiredStateShape(json, out bool missingSchema, out string shapeWhy))
            return new SchemaMigrationResult(SchemaMigrationStatus.NotLegacy, $"not a migratable legacy state: {shapeWhy}");
        if (!missingSchema)
            return new SchemaMigrationResult(SchemaMigrationStatus.NotNeeded, "state already carries a schema");

        var parsed = TryParseState(json, expectedIdentity, out string why, out StateStatus status, out _);
        if (parsed == null)
        {
            return status == StateStatus.IdentityMismatch
                ? new SchemaMigrationResult(SchemaMigrationStatus.IdentityMismatch, why)
                : new SchemaMigrationResult(SchemaMigrationStatus.NotLegacy, $"legacy state invalid: {why}");
        }

        // Backup FIRST: the pre-migration bytes must be durable before the main file is replaced.
        if (!storage.WriteAtomic(path + ".bak", json))
            return new SchemaMigrationResult(SchemaMigrationStatus.WriteFailed, "backup write failed - original state left in place");
        if (!storage.WriteAtomic(path, EncodeState(parsed)))
            return new SchemaMigrationResult(SchemaMigrationStatus.WriteFailed, "migrated write failed - original state left in place");

        return new SchemaMigrationResult(SchemaMigrationStatus.Migrated,
            $"upgraded to schema {CurrentSchemaVersion}, original kept as backup");
    }

    /// <summary>
    /// Reads/validates the write-ahead marker. Valid/Backup/Corrupt all block booking.
    /// </summary>
    public static PendingLoadResult DecodePending(IPayoutStorage storage, string path)
    {
        bool mainExists = storage.Exists(path);
        string? main = storage.ReadText(path);
        if (main != null)
        {
            var parsed = TryParsePending(main, out string why);
            if (parsed != null) return new PendingLoadResult(PendingStatus.Valid, parsed, "valid");
            string? bak = storage.BackupExists(path) ? storage.ReadText(path + ".bak") : null;
            if (bak != null)
            {
                var recovered = TryParsePending(bak, out _);
                if (recovered != null) return new PendingLoadResult(PendingStatus.Backup, recovered, $"main invalid ({why}), backup recovered");
            }
            return new PendingLoadResult(PendingStatus.Corrupt, null, why);
        }

        if (mainExists)
            return new PendingLoadResult(PendingStatus.Corrupt, null, "marker unreadable");

        if (storage.BackupExists(path))
        {
            string? bak = storage.ReadText(path + ".bak");
            if (bak == null)
                return new PendingLoadResult(PendingStatus.Corrupt, null, "marker missing, backup unreadable");
            var recovered = TryParsePending(bak, out string why);
            if (recovered != null) return new PendingLoadResult(PendingStatus.Backup, recovered, "marker missing, backup recovered");
            return new PendingLoadResult(PendingStatus.Corrupt, null, $"marker missing, backup invalid: {why}");
        }

        if (storage.Exists(path + ".tmp"))
            return new PendingLoadResult(PendingStatus.Corrupt, null, "marker missing but a staging .tmp artifact remains");

        return new PendingLoadResult(PendingStatus.Missing, null, "no marker");
    }

    private static PendingPayoutState? TryParsePending(string json, out string why)
    {
        why = "";

        // Structural gate: a marker missing day/amount/count/start is corrupt, never a day-0 marker.
        if (!HasRequiredPendingShape(json, out string shapeWhy)) { why = shapeWhy; return null; }

        PendingPayoutState? marker;
        try { marker = JsonSerializer.Deserialize<PendingPayoutState>(json, Options); }
        catch (Exception ex) { why = $"parse error: {ex.Message}"; return null; }

        if (marker == null) { why = "null marker"; return null; }
        if (marker.Day < 0) { why = $"invalid day {marker.Day}"; return null; }
        if (!float.IsFinite(marker.Amount) || marker.Amount < 0f) { why = "invalid amount"; return null; }
        if (marker.BusinessCount < 0) { why = "invalid business count"; return null; }
        return marker;
    }

    /// <summary>Required-field check for the write-ahead marker on the raw JSON.</summary>
    private static bool HasRequiredPendingShape(string json, out string reason)
    {
        reason = "";
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.ValueKind != JsonValueKind.Object) { reason = "marker root is not an object"; return false; }
            JsonElement root = doc.RootElement;
            if (!root.TryGetProperty("Day", out _)) { reason = "missing Day"; return false; }
            if (!root.TryGetProperty("Amount", out _)) { reason = "missing Amount"; return false; }
            if (!root.TryGetProperty("BusinessCount", out _)) { reason = "missing BusinessCount"; return false; }
            if (!root.TryGetProperty("StartedUtc", out _)) { reason = "missing StartedUtc"; return false; }
            return true;
        }
        catch (JsonException ex) { reason = $"parse error: {ex.Message}"; return false; }
    }

    /// <summary>
    /// Robustly clears the marker: removes the resurrectable .bak and .tmp FIRST, then the
    /// main file LAST, and STOPS on the first failed deletion so the main marker is never
    /// removed while an artifact that could resurrect it survives. Returns true only when all
    /// three paths are gone; the caller must fail closed on false.
    /// </summary>
    public static bool ClearMarker(IPayoutStorage storage, string path)
    {
        if (!DeleteAndVerify(storage, path + ".bak")) return false;
        if (!DeleteAndVerify(storage, path + ".tmp")) return false;
        if (!DeleteAndVerify(storage, path)) return false;
        return !storage.Exists(path) && !storage.Exists(path + ".bak") && !storage.Exists(path + ".tmp");
    }

    private static bool DeleteAndVerify(IPayoutStorage storage, string path)
    {
        storage.DeleteFile(path);
        return !storage.Exists(path);
    }
}
