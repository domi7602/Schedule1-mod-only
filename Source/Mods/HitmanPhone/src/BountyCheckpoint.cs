using System;

namespace HitmanPhone.Persistence;

/// <summary>Content identity of the native world files, excluding other mods' sidecars.</summary>
public static class BountyWorldCheckpoint
{
    public static string ResolveDirectory(string path)
    {
        string full = System.IO.Path.GetFullPath(path);
        if (System.IO.File.Exists(full) && string.Equals(System.IO.Path.GetFileName(full), "Game.json", StringComparison.OrdinalIgnoreCase))
            full = System.IO.Path.GetDirectoryName(full)!;
        if (!System.IO.Directory.Exists(full)) throw new System.IO.InvalidDataException("World save directory is unavailable: " + path);
        return full.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
    }

    public static string Fingerprint(string directory)
    {
        if (!System.IO.Directory.Exists(directory))
            throw new System.IO.DirectoryNotFoundException(directory);
        var files = new System.Collections.Generic.List<string>();
        Collect(directory, directory, files);
        files.Sort(StringComparer.Ordinal);
        if (files.Count == 0) throw new System.IO.InvalidDataException("No native world JSON checkpoint found.");
        using var hash = System.Security.Cryptography.IncrementalHash.CreateHash(System.Security.Cryptography.HashAlgorithmName.SHA256);
        long bytes = 0;
        foreach (string file in files)
        {
            var info = new System.IO.FileInfo(file);
            bytes += info.Length;
            if (bytes > 128 * 1024 * 1024) throw new System.IO.InvalidDataException("Native checkpoint exceeds 128 MiB safety limit.");
            string relative = System.IO.Path.GetRelativePath(directory, file).Replace('\\', '/');
            hash.AppendData(System.Text.Encoding.UTF8.GetBytes(relative + "\0"));
            using var stream = System.IO.File.OpenRead(file);
            using var fileHash = System.Security.Cryptography.SHA256.Create();
            hash.AppendData(fileHash.ComputeHash(stream));
        }
        return Convert.ToHexString(hash.GetHashAndReset());
    }

    private static void Collect(string root, string directory, System.Collections.Generic.List<string> files)
    {
        foreach (string file in System.IO.Directory.EnumerateFiles(directory, "*.json"))
        {
            if (string.Equals(System.IO.Path.GetFileName(file), "Metadata.json", StringComparison.OrdinalIgnoreCase)) continue;
            if ((System.IO.File.GetAttributes(file) & System.IO.FileAttributes.ReparsePoint) != 0)
                throw new System.IO.InvalidDataException("Linked checkpoint file is not supported.");
            files.Add(file);
            if (files.Count > 10000) throw new System.IO.InvalidDataException("Too many native checkpoint files.");
        }
        foreach (string child in System.IO.Directory.EnumerateDirectories(directory))
        {
            string name = System.IO.Path.GetFileName(child);
            if (name.Equals("Modded", StringComparison.OrdinalIgnoreCase) ||
                name.Equals("Backups", StringComparison.OrdinalIgnoreCase)) continue;
            if ((System.IO.File.GetAttributes(child) & System.IO.FileAttributes.ReparsePoint) != 0)
                throw new System.IO.InvalidDataException("Linked checkpoint folder is not supported.");
            if (System.IO.Path.GetRelativePath(root, child).Split(System.IO.Path.DirectorySeparatorChar).Length > 32)
                throw new System.IO.InvalidDataException("Checkpoint nesting exceeds safety limit.");
            Collect(root, child, files);
        }
    }
}

/// <summary>A save-start snapshot is committed only to its originally bound world.</summary>
public sealed class BountyCheckpointSession
{
    public string Identity { get; private set; } = "";
    public string Path { get; private set; } = "";
    public bool Dirty { get; private set; }
    public bool Saving { get; private set; }
    private string? _snapshot;
    public void Bind(string identity, string path)
    {
        Identity = identity;
        Path = path;
        Dirty = false;
        Saving = false;
        _snapshot = null;
    }
    public void MarkDirty() { Dirty = true; }
    public void Prepare(string snapshot)
    {
        if (string.IsNullOrEmpty(Identity) || string.IsNullOrEmpty(Path))
            throw new InvalidOperationException("No confirmed bounty save context.");
        _snapshot = snapshot;
        Saving = true;
    }
    public bool Commit(string identity, bool worldSaved, Func<string, string, bool> writer)
    {
        if (!worldSaved || !Saving || _snapshot == null ||
            !string.Equals(Identity, identity, StringComparison.Ordinal)) return false;
        if (!writer(Path, _snapshot)) return false;
        _snapshot = null;
        Saving = false;
        Dirty = false;
        return true;
    }
}

/// <summary>Validation performed before a loaded state is exposed to gameplay.</summary>
public static class BountySaveValidation
{
    public static BountySaveData CreateReset(BountySaveData previous) => new()
    {
        SaveIdentity = previous.SaveIdentity,
        NextEvidenceToken = previous.NextEvidenceToken
    };

    public static bool MatchesCheckpoint(BountySaveData data, string identity, string fingerprint)
        => !string.IsNullOrEmpty(identity) && !string.IsNullOrEmpty(fingerprint) &&
            data.SaveIdentity == identity && data.WorldCheckpoint == fingerprint;

    public static void Normalize(BountySaveData data)
    {
        data.Active ??= new();
        data.History ??= new();
        data.CallerCooldowns ??= new();
        data.KillEvents ??= new();
        data.HeatState ??= new();
        data.CallSchedule ??= new();
        data.CallSchedule.Slots ??= new();
        data.Active.RemoveAll(c => c == null);
        data.History.RemoveAll(c => c == null);
        data.KillEvents.RemoveAll(c => c == null);
        data.CallSchedule.Slots.RemoveAll(c => c == null);
        data.LastUsedDropId ??= "";
        data.SaveIdentity ??= "";
        foreach (var c in data.Active) NormalizeContract(c);
        foreach (var c in data.History) NormalizeContract(c);
    }

    private static void NormalizeContract(BountyContract c)
    {
        c.Id ??= "";
        c.CallerId ??= "";
        c.CallerStyle ??= "";
        c.TargetNpcId ??= "";
        c.TargetNpcName ??= "";
        c.AssignedDropName ??= "";
        c.RewardTier ??= "";
    }
}
