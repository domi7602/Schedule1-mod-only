namespace LightControl.Core;

public enum PersistenceLoadAction { Skip = 0, ConfirmMissing = 1, LoadJson = 2, ReadFailure = 3 }

/// <summary>
/// Outcome of one attempted read. <see cref="Absent"/> is only reported when absence was
/// *proven* (the file was not found and its reachable parent directory was listed); an
/// unreadable or unreachable path is <see cref="Unreadable"/>, never missing.
/// </summary>
public enum PersistenceReadState { Absent = 0, Unreadable = 1, Present = 2 }

/// <summary>Text plus the proof state of a single file read.</summary>
public sealed record PersistenceReadOutcome(PersistenceReadState State, string? Text, string Detail)
{
    public static PersistenceReadOutcome Absent(string detail) => new(PersistenceReadState.Absent, null, detail);
    public static PersistenceReadOutcome Unreadable(string detail) => new(PersistenceReadState.Unreadable, null, detail);
    public static PersistenceReadOutcome Present(string text) => new(PersistenceReadState.Present, text, "Readable text was produced.");

    /// <summary>An unconsulted file carries no absence proof; any decision needing it stays blocked.</summary>
    public static PersistenceReadOutcome NotConsulted { get; } =
        new(PersistenceReadState.Unreadable, null, "Not consulted; absence is unproven.");
}

/// <summary>Which file the plan tells the adapter to parse and load.</summary>
public enum PersistenceSource { None = 0, Main = 1, Backup = 2 }

/// <summary>Pure load decision: nothing is written and no gate is changed by producing it.</summary>
public sealed record PersistenceLoadPlan(
    PersistenceSource Source,
    PersistenceLoadAction Action,
    PersistenceError Error,
    string Detail,
    string? Json);

/// <summary>
/// Unity-free save-slot policy. The save id is derived from the active slot number only; a
/// negative slot (menu/transition) never yields a file and never authorizes a write. A read that
/// cannot produce text is a failure, not "missing data", so existing bytes are never clobbered.
/// </summary>
public static class PersistenceSlotPolicy
{
    public const string SaveIdPrefix = "slot-";

    /// <summary>
    /// Decides what to load from the *proof states* of the two files, never from a bare
    /// existence boolean. Rules:
    /// <list type="bullet">
    /// <item>Absence (main and backup) is only confirmed when both reads proved it.</item>
    /// <item>An unreadable or unreachable main blocks: its bytes are unknown, so nothing is
    /// replaced and no backup is substituted.</item>
    /// <item>A readable main with an unknown schema or a foreign save id is intact data that
    /// must be retained; it blocks even when a valid backup exists.</item>
    /// <item>Only a main that was read successfully and failed validation as lamp data is
    /// treated as corrupt, and only then is the backup read; the backup must pass its own full
    /// validation (schema, save id and every lamp) before it may be loaded.</item>
    /// <item>The plan never writes, so no corrupt file is repaired during load.</item>
    /// </list>
    /// </summary>
    public static PersistenceLoadPlan PlanLoad(
        PersistenceReadOutcome main,
        PersistenceReadOutcome backup,
        Func<string, PersistenceDataCheck> inspect)
    {
        if (main is null) throw new ArgumentNullException(nameof(main));
        if (backup is null) throw new ArgumentNullException(nameof(backup));
        if (inspect is null) throw new ArgumentNullException(nameof(inspect));

        if (main.State == PersistenceReadState.Present)
        {
            var check = inspect(main.Text ?? string.Empty);
            switch (check.Status)
            {
                case PersistenceDataStatus.Valid:
                    return new(PersistenceSource.Main, PersistenceLoadAction.LoadJson, PersistenceError.None,
                        "The current save's lamp file is valid.", main.Text);
                case PersistenceDataStatus.UnsupportedSchema:
                    return Blocked(check,
                        "The current save's lamp file uses an unsupported schema. The existing data is retained; the backup is never substituted for it.");
                case PersistenceDataStatus.WrongSave:
                    return Blocked(check,
                        "The current save's lamp file belongs to another game save. The existing data is retained; the backup is never substituted for it.");
                default:
                    return Recover(backup, inspect, check.Error,
                        $"The current save's lamp file could not be used ({check.Detail})");
            }
        }

        if (main.State == PersistenceReadState.Unreadable)
            return new(PersistenceSource.None, PersistenceLoadAction.ReadFailure, PersistenceError.InvalidState,
                "The lamp file could not be read, so its absence is not proven. Writes stay blocked and the existing file is kept. " + main.Detail, null);

        // Proven absence of the main file: the backup becomes the only candidate source. Only
        // when both files are provably absent is this "no data yet" rather than a failure.
        if (backup.State == PersistenceReadState.Absent)
            return new(PersistenceSource.None, PersistenceLoadAction.ConfirmMissing, PersistenceError.None,
                "No lamp file and no backup exist for this slot; both parent directories were listed, so the data is confirmed missing.",
                null);
        return Recover(backup, inspect, PersistenceError.InvalidJson, "The main lamp file is absent.");
    }

    private static PersistenceLoadPlan Recover(
        PersistenceReadOutcome backup,
        Func<string, PersistenceDataCheck> inspect,
        PersistenceError fallbackError,
        string context)
    {
        if (backup.State == PersistenceReadState.Absent)
            return Blocked(fallbackError,
                context + " No readable backup exists, so no lamp data is loaded and writes stay blocked.");

        if (backup.State == PersistenceReadState.Unreadable)
            return Blocked(PersistenceError.InvalidState,
                context + " The backup could not be read, so writes stay blocked and every existing file is kept. " + backup.Detail);

        var check = inspect(backup.Text ?? string.Empty);
        switch (check.Status)
        {
            case PersistenceDataStatus.Valid:
                return new(PersistenceSource.Backup, PersistenceLoadAction.LoadJson, PersistenceError.None,
                    context + " Recovered from the backup after it was independently validated (schema, save id and every lamp). No file was repaired.",
                    backup.Text);
            case PersistenceDataStatus.UnsupportedSchema:
                return Blocked(check,
                    context + " The backup uses an unsupported schema; it is retained and no data is loaded.");
            case PersistenceDataStatus.WrongSave:
                return Blocked(check,
                    context + " The backup belongs to another game save; it is retained and no data is loaded.");
            default:
                return Blocked(check,
                    context + " The backup is also unusable; writes stay blocked and every existing file is kept.");
        }
    }

    private static PersistenceLoadPlan Blocked(PersistenceDataCheck check, string detail) =>
        Blocked(check.Error, detail);

    private static PersistenceLoadPlan Blocked(PersistenceError error, string detail) =>
        new(PersistenceSource.None, PersistenceLoadAction.ReadFailure, error, detail, null);

    public static bool HasValidSlot(int slotNumber) => slotNumber >= 0;

    public static string? SaveIdForSlot(int slotNumber) => slotNumber >= 0 ? SaveIdPrefix + slotNumber : null;

    /// <summary>
    /// Legacy classification for *proven* presence flags. Callers must pass the result of a read
    /// attempt, never <c>File.Exists</c>: that call reports false for unreadable or unreachable
    /// paths and would turn unknown bytes into "missing data". Prefer <see cref="PlanLoad"/>.
    /// </summary>
    public static PersistenceLoadAction DecideLoad(bool mainExists, bool backupExists, string? text)
    {
        if (!mainExists && !backupExists) return PersistenceLoadAction.ConfirmMissing;
        if (string.IsNullOrWhiteSpace(text)) return PersistenceLoadAction.ReadFailure;
        return PersistenceLoadAction.LoadJson;
    }

    /// <summary>A write is only authorized while the active slot still equals the captured slot.</summary>
    public static bool CanWrite(int loadedSlot, int activeSlot) => loadedSlot >= 0 && loadedSlot == activeSlot;
}
