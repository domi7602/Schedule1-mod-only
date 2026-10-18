using System;

namespace BusinessIncome.Core;

/// <summary>
/// Pure storage seam. The production adapter forwards to <c>S1Mods.Shared.SafeStorage</c>
/// (atomic write + .bak copy + .tmp staging); unit tests inject fakes that simulate
/// per-path write/delete failures without touching disk or Unity.
/// </summary>
public interface IPayoutStorage
{
    /// <summary>True when the main file at <paramref name="path"/> exists.</summary>
    bool Exists(string path);

    /// <summary>True when the <c>.bak</c> sidecar at <paramref name="path"/> exists.</summary>
    bool BackupExists(string path);

    /// <summary>Raw text of the file, or null when missing/unreadable. Never throws.</summary>
    string? ReadText(string path);

    /// <summary>Atomically writes text. Returns false when the durable write failed.</summary>
    bool WriteAtomic(string path, string content);

    /// <summary>Deletes a single file. Returns true when the file is gone afterwards (or never existed). Never throws.</summary>
    bool DeleteFile(string path);
}

/// <summary>
/// Pure bank seam. Mirrors the <c>IGameWallet</c> guidance in money-api-semantics.md:
/// a clean return means "request accepted", not "settlement confirmed"; the call may
/// mutate-then-throw, in which case the outcome is UNKNOWN and must never be retried.
/// </summary>
public interface IPayoutBank
{
    /// <summary>Live money-manager readiness probe (not cached).</summary>
    bool IsReady();

    /// <summary>
    /// Requests an online transaction. Returns true when the request was handed off cleanly
    /// (request accepted, NOT confirmed settlement). May throw after mutating.
    /// </summary>
    bool RequestTransfer(string transactionName, float unitAmount, string note);
}

/// <summary>Logging seam so the pure core stays free of MelonLoader/Unity.</summary>
public delegate void PayoutLog(string message);
