using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TownHallInterior;

/// <summary>An unpaid operation, measured in native game minutes.</summary>
public sealed class ActiveLaunderingOperationRecord
{
    public string OperationId { get; set; } = string.Empty;
    public float Amount { get; set; }
    public int RemainingMinutes { get; set; } = 1440;
    public int TotalDurationMinutes { get; set; } = 1440;
    public int PayoutAttempts { get; set; }
    public bool PayoutUnresolved { get; set; }

    public int GetMinutesSinceStarted() => TotalDurationMinutes - RemainingMinutes;
}

public sealed record UnresolvedTownHallPayment(string OperationId, float Amount, string Reason);

public sealed class TownHallBusinessData
{
    public const int CurrentSchemaVersion = 2;

    public int SchemaVersion { get; set; } = CurrentSchemaVersion;
    public bool IsOwned { get; set; }
    public float PurchasePrice { get; set; }
    public string? PurchaseTimestampUtc { get; set; }
    public string? PaymentMode { get; set; }
    public float TotalLaundered { get; set; }
    public int CompletedLaunderingOperations { get; set; }
    public List<ActiveLaunderingOperationRecord> ActiveOperations { get; set; } = new();
    public UnresolvedTownHallPayment? UnresolvedPayment { get; set; }
}

public readonly record struct TownHallNativeValues(float Bank, float Cash, int TimeOfDay, int ElapsedDays);

public sealed record TownHallNativeCheckpoint(string Fingerprint, TownHallNativeValues Values)
{
    public static TownHallNativeCheckpoint Read(string slotPath, string playerFolder, string organisation)
    {
        if (string.IsNullOrWhiteSpace(playerFolder) || playerFolder is "." or ".." ||
            playerFolder.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
            playerFolder.Contains('/') || playerFolder.Contains('\\'))
        {
            throw new InvalidDataException("Invalid native player save folder.");
        }

        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

        JsonDocument ReadFile(string relativePath)
        {
            using var stream = File.OpenRead(Path.Combine(slotPath, relativePath));
            if (stream.Length is <= 0 or > 8 * 1024 * 1024)
                throw new InvalidDataException($"Unexpected native file size for {relativePath}.");
            byte[] bytes = new byte[(int)stream.Length];
            int offset = 0;
            while (offset < bytes.Length)
            {
                int read = stream.Read(bytes, offset, bytes.Length - offset);
                if (read == 0) throw new EndOfStreamException(relativePath);
                offset += read;
            }
            hash.AppendData(SHA256.HashData(bytes));
            return JsonDocument.Parse(bytes.AsMemory(bytes.AsSpan().StartsWith(Encoding.UTF8.Preamble) ? 3 : 0));
        }

        using var game = ReadFile("Game.json");
        if (game.RootElement.GetProperty("OrganisationName").GetString() != organisation)
            throw new InvalidDataException("Native organisation does not match the loaded world.");
        using var metadata = ReadFile("Metadata.json");
        if (metadata.RootElement.GetProperty("CreationDate").ValueKind != JsonValueKind.Object ||
            metadata.RootElement.GetProperty("LastPlayedDate").ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException("Missing native save dates.");
        }

        using var money = ReadFile("Money.json");
        float bank = money.RootElement.GetProperty("OnlineBalance").GetSingle();
        using var inventory = ReadFile(Path.Combine("Players", playerFolder, "Inventory.json"));
        float? cash = null;
        foreach (JsonElement entry in inventory.RootElement.GetProperty("Items").EnumerateArray())
        {
            if (entry.ValueKind == JsonValueKind.Null) continue;
            string? item = entry.GetString();
            if (string.IsNullOrWhiteSpace(item)) continue;
            using var itemData = JsonDocument.Parse(item);
            if (!itemData.RootElement.TryGetProperty("DataType", out var itemType) || itemType.GetString() != "CashData")
                continue;
            if (cash.HasValue || itemData.RootElement.GetProperty("ID").GetString() != "cash")
                throw new InvalidDataException("Ambiguous native cash record.");
            cash = itemData.RootElement.GetProperty("CashBalance").GetSingle();
        }

        using var time = ReadFile("Time.json");
        int hhmm = time.RootElement.GetProperty("TimeOfDay").GetInt32();
        int days = time.RootElement.GetProperty("ElapsedDays").GetInt32();
        if (!float.IsFinite(bank) || cash is not float cashBalance || !float.IsFinite(cashBalance) ||
            cashBalance < 0f || days < 0 || hhmm < 0 || hhmm > 2359 || hhmm % 100 >= 60)
        {
            throw new InvalidDataException("Invalid native wallet or clock.");
        }

        return new TownHallNativeCheckpoint(Convert.ToHexString(hash.GetHashAndReset()),
            new TownHallNativeValues(bank, cashBalance, hhmm, days));
    }
}

/// <summary>A loaded save generation and its immutable, native-bound Town Hall snapshots.</summary>
public sealed class TownHallSaveSession
{
    private sealed record SavedState(string Fingerprint, TownHallBusinessData Data);
    private sealed record PendingSave(string PreviousFingerprint, TownHallNativeValues Expected, TownHallBusinessData Data);
    private sealed record LegacyAdoption(string NativeFingerprint, string LegacySha256);

    private readonly string _archive;
    private readonly string _playerFolder;
    private readonly string _organisation;
    private PendingSave? _pending;
    private string? _pendingPath;

    private TownHallSaveSession(string slotPath, string archive, string playerFolder, string organisation,
        TownHallNativeCheckpoint native, TownHallBusinessData data)
    {
        SlotPath = slotPath;
        _archive = archive;
        _playerFolder = playerFolder;
        _organisation = organisation;
        Fingerprint = native.Fingerprint;
        NativeValues = native.Values;
        Data = data;
    }

    public string SlotPath { get; }
    public string Fingerprint { get; private set; }
    public TownHallNativeValues NativeValues { get; private set; }
    public TownHallBusinessData Data { get; }
    public bool HasPendingSave => _pending != null;

    public static string ResolveSlotPath(string path)
    {
        if (!Path.IsPathFullyQualified(path))
            throw new InvalidDataException("The native save path is not absolute.");
        return Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
    }

    public static TownHallSaveSession Open(ConstructionContext context, string baseDirectory, string playerFolder)
    {
        string? denied = ConstructionGate.Check(context);
        if (denied != null) throw new InvalidDataException(denied);

        string slotPath = ResolveSlotPath(context.ActiveSavePath!);
        string organisation = context.OrganisationName!;
        var native = TownHallNativeCheckpoint.Read(slotPath, playerFolder, organisation);
        string slotKey = OperatingSystem.IsWindows() ? slotPath.ToUpperInvariant() : slotPath;
        string archive = Path.Combine(baseDirectory, "Checkpoints",
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(slotKey))));

        string checkpointPath = Path.Combine(archive, native.Fingerprint + ".json");
        byte[]? checkpointBytes = ReadIfPresent(checkpointPath);
        if (checkpointBytes != null)
        {
            using var document = JsonDocument.Parse(checkpointBytes);
            if (document.RootElement.GetProperty("Fingerprint").GetString() != native.Fingerprint)
                throw new InvalidDataException("Rathausstand gehört zu einem anderen Spielstand.");
            var data = ReadCurrent(document.RootElement.GetProperty("Data"));
            return new TownHallSaveSession(slotPath, archive, playerFolder, organisation, native, data);
        }

        if (PathExists(archive))
            throw new InvalidDataException("Für diesen nativen Speicherstand fehlt ein passender Rathausstand.");

        var initial = ReadLegacy(baseDirectory, organisation, Path.GetFileName(slotPath), native.Fingerprint);
        var session = new TownHallSaveSession(slotPath, archive, playerFolder, organisation, native, initial);
        // Preserve the initial checkpoint before any purchase or deposit can change the wallet.
        session.WriteCheckpoint(native.Fingerprint, initial);
        return session;
    }

    public void Prepare(TownHallNativeValues expected)
    {
        if (_pending != null) throw new InvalidOperationException("A Rathaus save is already pending.");

        byte[] snapshot = JsonSerializer.SerializeToUtf8Bytes(Data);
        using var document = JsonDocument.Parse(snapshot);
        var frozen = new PendingSave(Fingerprint, expected, ReadCurrent(document.RootElement));
        string path = Path.Combine(_archive, "pending-" + Guid.NewGuid().ToString("N") + ".json");
        WriteImmutable(path, JsonSerializer.SerializeToUtf8Bytes(frozen));
        _pendingPath = path;
        _pending = frozen;
    }

    public void Complete(bool nativeSaveSucceeded)
    {
        var frozen = _pending ?? throw new InvalidOperationException("Missing Rathaus save snapshot.");
        string pendingPath = _pendingPath!;
        _pending = null;
        _pendingPath = null;

        if (!nativeSaveSucceeded)
            throw new InvalidDataException("Spielsave fehlgeschlagen oder unvollständig. Rathausstand gesichert, Zuordnung ausstehend.");

        var native = TownHallNativeCheckpoint.Read(SlotPath, _playerFolder, _organisation);
        if (native.Values != frozen.Expected)
            throw new InvalidDataException("Geld oder Spielzeit im Spielsave weicht vom vorbereiteten Rathausstand ab.");

        WriteCheckpoint(native.Fingerprint, frozen.Data);
        Fingerprint = native.Fingerprint;
        NativeValues = native.Values;

        // A failed cleanup can leave a harmless pending file beside the committed checkpoint.
        try { File.Delete(pendingPath); }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
    }

    private void WriteCheckpoint(string fingerprint, TownHallBusinessData data)
    {
        string path = Path.Combine(_archive, fingerprint + ".json");
        WriteImmutable(path, JsonSerializer.SerializeToUtf8Bytes(new SavedState(fingerprint, data)));
    }

    private static void WriteImmutable(string path, byte[] bytes)
    {
        byte[]? existing = ReadIfPresent(path);
        if (existing != null)
        {
            if (!existing.AsSpan().SequenceEqual(bytes))
                throw new InvalidDataException("Ein anderer Rathausstand ist diesem nativen Save bereits zugeordnet.");
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        string temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                stream.Write(bytes);
                stream.Flush(flushToDisk: true);
            }
            File.Move(temporary, path, overwrite: false);
        }
        finally
        {
            try { File.Delete(temporary); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }

    private static bool PathExists(string path)
    {
        try { _ = File.GetAttributes(path); return true; }
        catch (FileNotFoundException) { return false; }
        catch (DirectoryNotFoundException) { return false; }
    }

    private static byte[]? ReadIfPresent(string path)
    {
        try { return File.ReadAllBytes(path); }
        catch (FileNotFoundException) { return null; }
        catch (DirectoryNotFoundException) { return null; }
    }

    private static TownHallBusinessData ReadCurrent(JsonElement root)
    {
        if (root.GetProperty("SchemaVersion").GetInt32() != TownHallBusinessData.CurrentSchemaVersion)
            throw new InvalidDataException("Rathaus-Schemaversion wird nicht unterstützt.");
        var data = root.Deserialize<TownHallBusinessData>() ?? throw new InvalidDataException("Leerer Rathausstand.");
        _ = root.GetProperty("IsOwned").GetBoolean();
        _ = root.GetProperty("PurchasePrice").GetSingle();
        _ = root.GetProperty("TotalLaundered").GetSingle();
        _ = root.GetProperty("CompletedLaunderingOperations").GetInt32();
        _ = root.GetProperty("UnresolvedPayment");
        foreach (var operation in root.GetProperty("ActiveOperations").EnumerateArray())
        {
            _ = operation.GetProperty("RemainingMinutes").GetInt32();
            _ = operation.GetProperty("TotalDurationMinutes").GetInt32();
            _ = operation.GetProperty("PayoutAttempts").GetInt32();
            _ = operation.GetProperty("PayoutUnresolved").GetBoolean();
        }
        Validate(data);
        return data;
    }

    private static TownHallBusinessData ReadLegacy(string baseDirectory, string organisation, string slot, string fingerprint)
    {
        string folder = Path.Combine(baseDirectory, $"{organisation}_{slot}");
        byte[]? primary = ReadIfPresent(Path.Combine(folder, "townhall_business.json"));
        byte[]? backup = ReadIfPresent(Path.Combine(folder, "townhall_business.json.bak"));
        if (primary == null && backup == null) return new TownHallBusinessData();
        if (primary == null) throw new InvalidDataException("Nur eine alte Rathaus-Sicherung ist vorhanden. Manuelle Zuordnung erforderlich.");

        // A paid legacy state can be adopted only by explicitly pairing these exact bytes
        // with a verified native checkpoint. The old JSON and backup are kept intact.
        var data = ConvertLegacy(primary);
        bool pristine = !data.IsOwned && (backup == null || !ConvertLegacy(backup).IsOwned);
        if (pristine) return data;

        var approvalBytes = ReadIfPresent(Path.Combine(folder, "legacy-adoption.json"));
        var approval = approvalBytes == null ? null : JsonSerializer.Deserialize<LegacyAdoption>(
            approvalBytes.AsSpan(approvalBytes.AsSpan().StartsWith(Encoding.UTF8.Preamble) ? 3 : 0));
        string primaryHash = Convert.ToHexString(SHA256.HashData(primary));
        if (approval == null || !string.Equals(approval.NativeFingerprint, fingerprint, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(approval.LegacySha256, primaryHash, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidDataException($"Alter Rathausstand ohne bestätigte Save-Zuordnung (Spielstand {fingerprint}, Rathausdatei {primaryHash}).");
        }
        return data;
    }

    private static TownHallBusinessData ConvertLegacy(byte[] bytes)
    {
        using var document = JsonDocument.Parse(bytes.AsMemory(bytes.AsSpan().StartsWith(Encoding.UTF8.Preamble) ? 3 : 0));
        var root = ReadLegacyFields(document.RootElement);
        if (root["SchemaVersion"].GetInt32() != 1)
            throw new InvalidDataException("Alte Rathaus-Schemaversion wird nicht unterstützt.");
        var data = new TownHallBusinessData
        {
            IsOwned = root["IsOwned"].GetBoolean(),
            PurchasePrice = root["PurchasePrice"].GetSingle(),
            PurchaseTimestampUtc = root.TryGetValue("PurchaseTimestampUtc", out var timestamp) ? timestamp.GetString() : null,
            PaymentMode = root.TryGetValue("PaymentMode", out var mode) ? mode.GetString() : null,
            TotalLaundered = root["TotalLaundered"].GetSingle(),
            CompletedLaunderingOperations = root["CompletedLaunderingOperations"].GetInt32()
        };

        if (root.TryGetValue("ActiveOperations", out var operations))
        {
            foreach (var entry in operations.EnumerateArray())
            {
                var operation = ReadLegacyFields(entry);
                float seconds = operation["RemainingSeconds"].GetSingle();
                float total = operation["TotalDurationSeconds"].GetSingle();
                if (!float.IsFinite(seconds) || !float.IsFinite(total) || seconds < 0f || total <= 0f || seconds > total)
                    throw new InvalidDataException("Ungültige Restzeit im alten Rathausstand.");

                data.ActiveOperations.Add(new ActiveLaunderingOperationRecord
                {
                    OperationId = operation["OperationId"].GetString() ?? "",
                    Amount = operation["Amount"].GetSingle(),
                    RemainingMinutes = (int)Math.Ceiling((double)seconds / total * 1440),
                    // A due v1 job could have been paid before the old adapter reported an error.
                    PayoutAttempts = seconds == 0f ? 1 : 0,
                    PayoutUnresolved = seconds == 0f
                });
            }
        }
        Validate(data);
        return data;
    }

    private static Dictionary<string, JsonElement> ReadLegacyFields(JsonElement element)
    {
        var fields = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in element.EnumerateObject())
        {
            if (!fields.TryAdd(property.Name, property.Value))
                throw new InvalidDataException("Doppeltes Feld im alten Rathausstand.");
        }
        return fields;
    }

    private static void Validate(TownHallBusinessData data)
    {
        if (!float.IsFinite(data.PurchasePrice) || !float.IsFinite(data.TotalLaundered) ||
            data.PurchasePrice < 0f || data.TotalLaundered < 0f || data.CompletedLaunderingOperations < 0 ||
            data.ActiveOperations == null ||
            (data.IsOwned && (data.PurchasePrice <= 0f ||
                data.PaymentMode is not ("Bank" or "Cash") ||
                !DateTimeOffset.TryParse(data.PurchaseTimestampUtc, out _))) ||
            (!data.IsOwned && (data.PurchasePrice != 0f || data.TotalLaundered != 0f ||
                data.CompletedLaunderingOperations != 0 || data.ActiveOperations.Count != 0)) ||
            (data.UnresolvedPayment is { } unresolved && (string.IsNullOrWhiteSpace(unresolved.OperationId) ||
                string.IsNullOrWhiteSpace(unresolved.Reason) || !float.IsFinite(unresolved.Amount) || unresolved.Amount <= 0f)))
        {
            throw new InvalidDataException("Ungültiger Rathausstand.");
        }

        var ids = new HashSet<string>(StringComparer.Ordinal);
        double total = 0;
        foreach (var operation in data.ActiveOperations)
        {
            if (operation == null || string.IsNullOrWhiteSpace(operation.OperationId) || !ids.Add(operation.OperationId) ||
                !float.IsFinite(operation.Amount) || operation.Amount <= 0f ||
                operation.TotalDurationMinutes != BusinessLaunderingManager.DefaultDurationMinutes ||
                operation.RemainingMinutes < 0 || operation.RemainingMinutes > operation.TotalDurationMinutes ||
                operation.PayoutAttempts is < 0 or > BusinessLaunderingManager.MaxPayoutAttempts ||
                (operation.PayoutUnresolved && operation.PayoutAttempts == 0) ||
                ((operation.PayoutAttempts > 0 || operation.PayoutUnresolved) && operation.RemainingMinutes != 0))
            {
                throw new InvalidDataException("Ungültiger Geldwäscheauftrag im Rathausstand.");
            }
            total += operation.Amount;
        }
        if (total > BusinessLaunderingManager.DefaultCapacity)
            throw new InvalidDataException("Offene Geldwäscheaufträge überschreiten die Rathauskapazität.");
    }
}
