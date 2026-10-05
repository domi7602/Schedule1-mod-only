using System;
using System.Collections.Generic;
using BusinessIncome.Core;

namespace BusinessIncome.Tests;

/// <summary>
/// In-memory <see cref="IPayoutStorage"/> used to exercise every write/delete failure path
/// with no disk and no Unity. Paths are compared case-insensitively (as Windows does).
/// </summary>
internal sealed class FakeStorage : IPayoutStorage
{
    private readonly Dictionary<string, string> _files = new(StringComparer.OrdinalIgnoreCase);

    public readonly HashSet<string> FailWrite = new(StringComparer.OrdinalIgnoreCase);
    public readonly HashSet<string> FailDelete = new(StringComparer.OrdinalIgnoreCase);
    public readonly HashSet<string> FailRead = new(StringComparer.OrdinalIgnoreCase);
    public readonly List<string> WriteOrder = new();
    public readonly List<string> DeleteOrder = new();

    public int WriteCount { get; private set; }
    public int DeleteCount { get; private set; }
    public int FileCount => _files.Count;

    public void Seed(string path, string content) => _files[path] = content;

    public bool Has(string path) => _files.ContainsKey(path);

    public string? Peek(string path) => _files.TryGetValue(path, out var v) ? v : null;

    public bool Exists(string path) => _files.ContainsKey(path);

    public bool BackupExists(string path) => _files.ContainsKey(path + ".bak");

    public string? ReadText(string path) =>
        FailRead.Contains(path) ? null : (_files.TryGetValue(path, out var v) ? v : null);

    public bool WriteAtomic(string path, string content)
    {
        WriteCount++;
        if (FailWrite.Contains(path)) return false;
        _files[path] = content;
        WriteOrder.Add(path);
        return true;
    }

    public bool DeleteFile(string path)
    {
        DeleteCount++;
        if (FailDelete.Contains(path)) return false;
        DeleteOrder.Add(path);
        _files.Remove(path);
        return true;
    }
}

/// <summary>
/// Scriptable <see cref="IPayoutBank"/>: ready/down, accept/reject, mutate-then-throw,
/// and re-entrancy via a custom handler.
/// </summary>
internal sealed class FakeBank : IPayoutBank
{
    public bool Ready { get; set; } = true;
    public Func<string, float, string, bool>? Handler { get; set; }
    public int Calls { get; private set; }
    public float LastAmount { get; private set; }

    public bool IsReady() => Ready;

    public bool RequestTransfer(string transactionName, float unitAmount, string note)
    {
        Calls++;
        LastAmount = unitAmount;
        return Handler != null ? Handler(transactionName, unitAmount, note) : true;
    }
}
