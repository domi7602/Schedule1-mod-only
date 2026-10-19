using System;

namespace LightControl.Core;

/// <summary>
/// What the adapter must do with one owned surface-material clone after a restore attempt. This is
/// the adapter's release proof: a clone is destroyed only when the outcome proves the renderer no
/// longer references it (<see cref="Disconnected"/> or <see cref="Restored"/>).
/// </summary>
public enum LedReleaseOutcome
{
    /// <summary>No owned clone exists; there is nothing to release.</summary>
    Nothing = 0,
    /// <summary>The renderer is readable and provably no longer references the clone: destroy is safe.</summary>
    Disconnected = 1,
    /// <summary>The exact owned slot held our clone and was written back to the native material: destroy is safe.</summary>
    Restored = 2,
    /// <summary>The clone may still be referenced (unreadable/dead renderer, dead source, failed write, or a clone outside the exact owned slot): retain and retry, never destroy.</summary>
    Deferred = 3
}

/// <summary>
/// Unity-free ownership, source-choice and release model for one owned grow-lamp LED surface clone.
/// <para>
/// It answers the two questions the adapter's material lifecycle turns on: which native material a
/// clone must come from (so the clone chain can never drift onto the clone itself), and whether an
/// owned clone is proven unreferenced and may therefore be destroyed. The adapter never writes a
/// foreign slot; a slot that no longer holds our clone simply means we are disconnected.
/// </para>
/// </summary>
public static class LedSurfaceOwnership
{
    /// <summary>
    /// The material a clone must be created from: when the slot already holds our own clone, the
    /// clone's own installed native source (never the clone itself), otherwise the slot's current
    /// native material. Keeping this value stable across changes is what stops the source from
    /// drifting and the clones from chaining.
    /// </summary>
    public static T? CloneSource<T>(bool ownClone, T? currentSlot, T? installedSource) where T : class =>
        ownClone ? installedSource : currentSlot;

    /// <summary>
    /// True when a clone must be created: we own none yet, or the native source the clone was built
    /// from is no longer the current source. The same native source always reuses the existing clone.
    /// </summary>
    public static bool NeedsClone<T>(T? clone, T? source, T? installedSource) where T : class =>
        clone is null || (source is not null && !ReferenceEquals(installedSource, source));

    /// <summary>
    /// Native-identity variant: a clone must be created when we own none yet, or when the source the
    /// clone was built from is a <i>different native material</i>. Distinct managed wrappers around the
    /// same native material are the same source, so the existing clone is reused instead of being
    /// recreated on every read (which is what a managed reference comparison did).
    /// </summary>
    public static bool NeedsClone<T>(T? clone, T? source, T? installedSource, Func<T, long> nativeId,
        Func<T?, T?, bool>? referenceEquals = null) where T : class =>
        clone is null || (source is not null && !NativeMaterialIdentity.SameNative(source, installedSource, nativeId, referenceEquals));

    /// <summary>
    /// True only when a restore outcome proves the surface settled. A <see cref="LedReleaseOutcome.Deferred"/>
    /// restore must be retried: reporting it as converged would silently drop the pending Original
    /// restore and the slot would never be handed back.
    /// </summary>
    public static bool RestoreConverged(LedReleaseOutcome outcome) => outcome is not LedReleaseOutcome.Deferred;

    /// <summary>
    /// Decides whether an owned clone may be destroyed. Only <see cref="LedReleaseOutcome.Disconnected"/>
    /// (the renderer is readable and references the clone nowhere) and <see cref="LedReleaseOutcome.Restored"/>
    /// (the exact owned slot held the clone and was written back to a live native material) prove the
    /// clone is unreferenced. Every uncertainty — unreadable renderer, dead renderer wrapper, dead
    /// source, failed setter, or the clone sitting outside the exact owned slot — retains it.
    /// </summary>
    public static LedReleaseOutcome DecideRelease(
        bool hasClone,
        bool rendererAlive,
        bool slotReadable,
        bool arrayReferencesClone,
        bool exactSlotHoldsClone,
        bool nativeSourceAlive,
        bool writeSucceeded)
    {
        if (!hasClone) return LedReleaseOutcome.Nothing;
        if (!rendererAlive) return LedReleaseOutcome.Deferred;
        if (!slotReadable) return LedReleaseOutcome.Deferred;
        if (!arrayReferencesClone) return LedReleaseOutcome.Disconnected;
        if (!exactSlotHoldsClone) return LedReleaseOutcome.Deferred;
        if (!nativeSourceAlive) return LedReleaseOutcome.Deferred;
        if (!writeSucceeded) return LedReleaseOutcome.Deferred;
        return LedReleaseOutcome.Restored;
    }
}
