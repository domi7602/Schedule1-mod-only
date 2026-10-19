namespace LightControl.Core;

public enum PersistenceLoadAction { Skip = 0, ConfirmMissing = 1, LoadJson = 2, ReadFailure = 3 }

/// <summary>
/// Unity-free save-slot policy. The save id is derived from the active slot number only; a
/// negative slot (menu/transition) never yields a file and never authorizes a write. A read that
/// cannot produce text is a failure, not "missing data", so existing bytes are never clobbered.
/// </summary>
public static class PersistenceSlotPolicy
{
    public const string SaveIdPrefix = "slot-";

    public static bool HasValidSlot(int slotNumber) => slotNumber >= 0;

    public static string? SaveIdForSlot(int slotNumber) => slotNumber >= 0 ? SaveIdPrefix + slotNumber : null;

    /// <summary>
    /// A file that does not exist (main and backup) is confirmed missing; existing storage that
    /// yields no text is a read failure (block writes and keep the file); anything else is parsed.
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
