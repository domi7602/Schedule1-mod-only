using LightControl.Core;

namespace LightControl.Core.Tests;

/// <summary>
/// Unity-free checks for the native light ownership and LED surface release repairs (LC01, LC04) and
/// the bounded surface-refresh/logging guards (LC05, LC11). Every case exercises the production
/// policy the runtime calls; the source-wiring cases assert the adapter uses that policy.
/// </summary>
internal static class NativeOwnershipChecks
{
    public static IEnumerable<(string Name, Action Run)> Cases => new (string, Action)[]
    {
        ("N01 Late-update never rebases intensity on our own installed output", OwnOutputNeverRebases),
        ("N02 Late-update never rebases colour on our own installed output", OwnColorNeverRebases),
        ("N03 Original after a missed undo restores the real baseline", OriginalAfterMissedUndo),
        ("N04 A failed hand-back keeps intensity ownership and its baseline", FailedHandbackKeepsIntensityOwnership),
        ("N05 A failed hand-back keeps colour ownership and its baseline", FailedHandbackKeepsColorOwnership),
        ("N06 A hand-back never writes when nothing of ours is installed", HandbackWithoutOwnershipWritesNothing),
        ("N07 A failed apply keeps the previously installed ownership and baseline", FailedApplyKeepsPriorOwnership),
        ("N08 A failed first apply still claims nothing", FailedFirstApplyClaimsNothing),
        ("N09 A binding confirms each property's hand-back independently", BindingIndependentHandback),
        ("N10 A binding supersedes a dead hand-back on the next apply", BindingHandbackSupersededByApply),
        ("N11 A null neighbour slot is a readable empty element", NullNeighbourSlotIsReadable),
        ("N12 A restore releases only after a full re-read proves no reference", RestoreRequiresReread),
        ("N13 Installing into a new slot hands the previous slot back first", SlotRelocationDecisions),
        ("N14 A provably destroyed light drops its claim without a write", AbandonDropsOnlyClaims),
        ("N15 Runtime confirms native hand-backs and retains them past teardown", HandbackWiring),
        ("N16 Runtime releases a clone only after a full re-read", RereadWiring),
        ("N17 Runtime relocates our clone when MaterialIndex moves", RelocationWiring),
        ("N18 Runtime keys its surface cache on the slot and refreshes at a bounded cadence", SurfaceRefreshWiring),
        ("N19 Runtime guards the apply breadcrumb before the native read-back", LoggingGuardWiring)
    };

    // ---- shared helpers --------------------------------------------------------------------

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void Equal<T>(T expected, T actual, string? context = null)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new InvalidOperationException($"{context ?? "value"}: expected {expected}, actual {actual}");
    }

    private static void Throws<TException>(Action action) where TException : Exception
    {
        try { action(); }
        catch (TException) { return; }
        throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
    }

    private static VisualInstruction Twice() => new(null, 2, false);
    private static VisualInstruction Original(double factor = 1) => new(null, factor, false);

    // ---- LC01: a missed undo must never compound our own output ------------------------------

    private static void OwnOutputNeverRebases()
    {
        var policy = new NativeLightPolicy();
        Equal(new NativeLightStep(true, 20f, true), policy.Apply(10, Twice()));
        Equal(10.0, policy.Baseline);

        // The update-phase undo could not run (its native read failed), so the late-update phase
        // samples our own installed output again. It is not a new native baseline.
        NativeLightStep second = policy.Apply(20, Twice());
        Require(second.ShouldWrite, "own output must be re-affirmed, not skipped while unwritten");
        Equal(20f, second.Intensity);
        Equal(10.0, policy.Baseline);

        // And it never compounds across further missed undos.
        NativeLightStep third = policy.Apply(20, Twice());
        Equal(20f, third.Intensity);
        Equal(10.0, policy.Baseline);

        // The next real native sample is a baseline again.
        policy.UndoOwned(20);
        Equal(10.0, policy.Baseline);
        Equal(new NativeLightStep(true, 34f, true), policy.Apply(17, Twice()));
    }

    private static void OwnColorNeverRebases()
    {
        var policy = new NativeColorPolicy();
        var red = new RgbColor(1, 0, 0);
        Equal(new NativeColorStep(true, 1f, 0f, 0f, 1f, true), policy.Apply(0.2f, 0.3f, 0.4f, 1f, red));
        // Our own colour is sampled again after a missed undo: the baseline stays the native one.
        policy.Apply(1f, 0f, 0f, 1f, red);
        Equal(new NativeColorStep(true, 0.2f, 0.3f, 0.4f, 1f, false), policy.UndoOwned(1f, 0f, 0f, 1f));
        Equal(false, policy.Owned);
    }

    private static void OriginalAfterMissedUndo()
    {
        var policy = new NativeLightPolicy();
        policy.Apply(10, Twice());
        // Our own output is still installed and the instruction now removes mod influence: the
        // exact native baseline comes back, never 20 (which would silently keep our brightness).
        Equal(new NativeLightStep(true, 10f, false), policy.Apply(20, Original()));
        Equal(false, policy.Owned);
    }

    private static void FailedHandbackKeepsIntensityOwnership()
    {
        var policy = new NativeLightPolicy();
        policy.Apply(10, Twice());

        // The update-phase hand-back emits the baseline but the adapter must not have released
        // ownership yet: the setter may still throw and our 20 would stay installed.
        Equal(new NativeLightStep(true, 10f, false), policy.BeginHandback(20));
        Require(policy.Owned, "ownership was cleared before the hand-back write was confirmed");
        Require(policy.HandbackPending, "the hand-back was not tracked as pending");

        // The native setter threw: our own output is still installed, so ownership and the native
        // baseline must both survive so a later frame can retry the hand-back.
        policy.RejectHandback();
        Require(policy.Owned, "ownership was lost when the hand-back setter failed");
        Equal(false, policy.HandbackPending);
        Equal(10.0, policy.Baseline);

        // The missed hand-back must not compound either.
        Equal(20f, policy.Apply(20, Twice()).Intensity);
        Equal(10.0, policy.Baseline);

        // A later frame retries and only then releases.
        Equal(new NativeLightStep(true, 10f, false), policy.BeginHandback(20));
        policy.ConfirmHandback();
        Equal(false, policy.Owned);
        Equal(false, policy.HandbackPending);
        Equal(new NativeLightStep(false, 0, false), policy.BeginHandback(10));
    }

    private static void FailedHandbackKeepsColorOwnership()
    {
        var policy = new NativeColorPolicy();
        var red = new RgbColor(1, 0, 0);
        policy.Apply(0.2f, 0.3f, 0.4f, 1f, red);

        Equal(new NativeColorStep(true, 0.2f, 0.3f, 0.4f, 1f, false), policy.BeginHandback(1f, 0f, 0f, 1f));
        Require(policy.Owned, "colour ownership was cleared before the hand-back write was confirmed");
        Require(policy.HandbackPending, "the colour hand-back was not tracked as pending");

        policy.RejectHandback();
        Require(policy.Owned, "colour ownership was lost when the hand-back setter failed");
        Equal(new NativeColorStep(true, 0.2f, 0.3f, 0.4f, 1f, false), policy.BeginHandback(1f, 0f, 0f, 1f));
        policy.ConfirmHandback();
        Equal(false, policy.Owned);
        Equal(new NativeColorStep(false, 0, 0, 0, 0, false), policy.BeginHandback(0.2f, 0.3f, 0.4f, 1f));
    }

    private static void HandbackWithoutOwnershipWritesNothing()
    {
        var intensity = new NativeLightPolicy();
        Equal(new NativeLightStep(false, 0, false), intensity.BeginHandback(7));
        Equal(false, intensity.HandbackPending);

        var color = new NativeColorPolicy();
        Equal(new NativeColorStep(false, 0, 0, 0, 0, false), color.BeginHandback(0f, 1f, 0f, 1f));
        Equal(false, color.HandbackPending);

        // An external write is adopted and released without any native write, exactly as before.
        var drifted = new NativeLightPolicy();
        drifted.Apply(10, Twice());
        Equal(new NativeLightStep(false, 0, false), drifted.BeginHandback(7));
        Equal(false, drifted.Owned);
        Equal(false, drifted.HandbackPending);
        Equal(7.0, drifted.Baseline);
    }

    private static void FailedApplyKeepsPriorOwnership()
    {
        var policy = new NativeLightPolicy();
        var half = new VisualInstruction(null, 1.5, false);
        policy.Apply(10, Twice());

        // The next late-update rebases on the real native value and claims the new output; the
        // native setter then throws, so nothing changed on the light: 20 is still installed.
        Equal(new NativeLightStep(true, 15f, true), policy.Apply(10, half));
        policy.RejectWrite();
        Require(policy.Owned, "a failed setter dropped ownership of our still-installed output");
        Equal(10.0, policy.Baseline);

        // The retry must still rebase on the retained native baseline, never on our own 20.
        Equal(15f, policy.Apply(20, half).Intensity);
        Equal(10.0, policy.Baseline);

        // The colour property keeps its own prior ownership for the same reason.
        var color = new NativeColorPolicy();
        color.Apply(0.2f, 0.3f, 0.4f, 1f, new RgbColor(1, 0, 0));
        color.Apply(0.2f, 0.3f, 0.4f, 1f, new RgbColor(0, 0, 1));
        color.RejectWrite();
        Require(color.Owned, "a failed colour setter dropped ownership of our still-installed colour");
        Equal(new NativeColorStep(true, 0.2f, 0.3f, 0.4f, 1f, false), color.BeginHandback(0f, 0f, 1f, 1f));
    }

    private static void FailedFirstApplyClaimsNothing()
    {
        // Nothing of ours was installed before the failed write, so nothing may be claimed.
        var policy = new NativeLightPolicy();
        policy.Apply(10, Twice());
        policy.RejectWrite();
        Equal(false, policy.Owned);
        Equal(new NativeLightStep(false, 0, false), policy.BeginHandback(10));

        var color = new NativeColorPolicy();
        color.Apply(0f, 0f, 0f, 1f, new RgbColor(0, 0, 1));
        color.RejectWrite();
        Equal(false, color.Owned);
        Equal(new NativeColorStep(false, 0, 0, 0, 0, false), color.BeginHandback(0f, 0f, 1f, 1f));
    }

    private static void BindingIndependentHandback()
    {
        var binding = new NativeLightBinding();
        var red = new RgbColor(1, 0, 0);
        binding.Apply(10, 0.2f, 0.3f, 0.4f, 1f, new VisualInstruction(red, 2, false));
        Require(binding.IntensityOwned && binding.ColorOwned, "the apply did not claim both properties");

        NativeLightApply handback = binding.BeginHandback(20f, 1f, 0f, 0f, 1f);
        Equal(new NativeLightStep(true, 10f, false), handback.Intensity);
        Equal(new NativeColorStep(true, 0.2f, 0.3f, 0.4f, 1f, false), handback.Color);
        Require(binding.HandbackPending, "the binding did not report a pending hand-back");
        Require(binding.OwnsAny, "the binding released ownership before any write was confirmed");

        // Only the colour setter failed: intensity is confirmed and released, colour is retained.
        binding.ConfirmIntensityHandback();
        binding.RejectColorHandback();
        Equal(false, binding.IntensityOwned);
        Require(binding.ColorOwned, "a failed colour hand-back dropped the colour we still own");
        Equal(false, binding.HandbackPending);

        // The next apply must not rebase colour on our own still-installed red.
        binding.Apply(10, 1f, 0f, 0f, 1f, new VisualInstruction(new RgbColor(0, 0, 1), 1, false));
        NativeLightApply retry = binding.BeginHandback(10f, 0f, 0f, 1f, 1f);
        Equal(new NativeColorStep(true, 0.2f, 0.3f, 0.4f, 1f, false), retry.Color);
        binding.ConfirmColorHandback();
        Equal(false, binding.ColorOwned);
        Equal(true, retry.Intensity.ShouldWrite);
        binding.ConfirmIntensityHandback();
        Equal(false, binding.OwnsAny);
    }

    private static void BindingHandbackSupersededByApply()
    {
        var binding = new NativeLightBinding();
        binding.Apply(10, 0.2f, 0.3f, 0.4f, 1f, new VisualInstruction(new RgbColor(1, 0, 0), 2, false));
        binding.BeginHandback(20f, 1f, 0f, 0f, 1f);
        Require(binding.HandbackPending, "the hand-back was not pending");

        // A new late-update instruction supersedes the unfinished hand-back; leaving it pending
        // would let an old confirmation release ownership of the new output.
        binding.Apply(10, 0.2f, 0.3f, 0.4f, 1f, new VisualInstruction(new RgbColor(0, 0, 1), 2, false));
        Equal(false, binding.HandbackPending);
        binding.ConfirmIntensityHandback();
        Require(binding.IntensityOwned, "a stale hand-back confirmation released the new output");
        Require(binding.ColorOwned && binding.OwnsAny, "the new apply lost its ownership");
    }

    private static void NullNeighbourSlotIsReadable()
    {
        // A material array may legitimately contain a null element (an empty neighbour slot). The
        // read succeeded, so the scan is still a complete proof that our clone is absent.
        var scan = new NativeMaterialArrayScan();
        scan.RecordRead(0x1000, 0x2000);
        scan.RecordRead(0, 0x2000);
        Equal(true, scan.FullyReadable);
        Equal(false, scan.ReferencesClone);
        Equal(true, scan.ProvesDisconnected);
        Equal(LedReleaseOutcome.Disconnected, LedSurfaceOwnership.DecideRelease(true, true, scan.FullyReadable, scan.ReferencesClone, false, true, false));

        // A null element that is itself our clone id (an unreadable clone) stays unprovable.
        var unknownClone = new NativeMaterialArrayScan();
        unknownClone.RecordRead(0x1000, 0);
        Equal(false, unknownClone.FullyReadable);
        Equal(false, unknownClone.ProvesDisconnected);

        // A read that really threw still makes the scan incomplete.
        var partial = new NativeMaterialArrayScan();
        partial.RecordRead(0x1000, 0x2000);
        partial.RecordUnreadable();
        Equal(false, partial.FullyReadable);
        Equal(false, partial.ProvesDisconnected);

        // The proven case is unaffected by an empty slot next to our clone.
        var ours = new NativeMaterialArrayScan();
        ours.RecordRead(0, 0x2000);
        ours.RecordRead(0x2000, 0x2000);
        Equal(true, ours.ReferencesClone);
        Equal(true, ours.FullyReadable);
        Equal(false, ours.ProvesDisconnected);
    }

    private static void RestoreRequiresReread()
    {
        // A successful setter is not a release proof on its own: the clone may still sit in another
        // slot of the same array. Only a full re-read that no longer references it proves release.
        Equal(LedReleaseOutcome.Deferred, LedSurfaceOwnership.ConfirmRestore(rereadReadable: false, stillReferencesClone: false));
        Equal(LedReleaseOutcome.Deferred, LedSurfaceOwnership.ConfirmRestore(rereadReadable: true, stillReferencesClone: true));
        Equal(LedReleaseOutcome.Restored, LedSurfaceOwnership.ConfirmRestore(rereadReadable: true, stillReferencesClone: false));
        // A deferred restore is never converged.
        Equal(false, LedSurfaceOwnership.RestoreConverged(LedSurfaceOwnership.ConfirmRestore(false, false)));
    }

    private static void SlotRelocationDecisions()
    {
        // Nothing installed yet: no relocation work.
        Equal(false, LedSurfaceOwnership.RequiresRelocation(-1, 0));
        Equal(false, LedSurfaceOwnership.RequiresRelocation(-1, 3));
        // Same slot: the installation is already where it belongs.
        Equal(false, LedSurfaceOwnership.RequiresRelocation(1, 1));
        // The MaterialIndex moved: the previous slot must be handed back before installing elsewhere.
        Equal(true, LedSurfaceOwnership.RequiresRelocation(0, 1));
        Equal(true, LedSurfaceOwnership.RequiresRelocation(2, 0));
    }

    private static void AbandonDropsOnlyClaims()
    {
        // Only a provably destroyed native object is abandoned: there is no value left to hand back,
        // so the claim is dropped instead of being retried forever against a dead handle.
        var binding = new NativeLightBinding();
        binding.Apply(10, 0.2f, 0.3f, 0.4f, 1f, new VisualInstruction(new RgbColor(1, 0, 0), 2, false));
        binding.BeginHandback(20f, 1f, 0f, 0f, 1f);
        Require(binding.OwnsAny, "the binding lost its claims before the abandon");
        binding.Abandon();
        Equal(false, binding.OwnsAny);
        Equal(false, binding.HandbackPending);
        Equal(false, binding.IntensityOwned);
        Equal(false, binding.ColorOwned);

        // An abandoned policy claims nothing on the next apply either.
        var policy = new NativeLightPolicy();
        policy.Apply(10, Twice());
        policy.Abandon();
        Equal(false, policy.Owned);
        Equal(new NativeLightStep(false, 0, false), policy.BeginHandback(20));
    }

    // ---- adapter source contracts ----------------------------------------------------------
    //
    // These read the production adapter text because the Unity/IL2CPP adapter cannot execute in this
    // game-free host. They prove the wiring, not the native behaviour the policy cases above prove.

    private static string RuntimeSource()
    {
        string? overridden = Environment.GetEnvironmentVariable("LIGHTCONTROL_RUNTIME_SOURCE");
        if (!string.IsNullOrEmpty(overridden) && File.Exists(overridden)) return File.ReadAllText(overridden);
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            string path = Path.Combine(directory.FullName, "Source", "Mods", "LightControl", "src", "LightRuntime.cs");
            if (File.Exists(path)) return File.ReadAllText(path);
            directory = directory.Parent;
        }
        throw new InvalidOperationException("Cannot locate LightRuntime.cs for the native wiring checks.");
    }

    private static string Body(string signature)
    {
        string source = RuntimeSource();
        int start = source.IndexOf(signature, StringComparison.Ordinal);
        Require(start >= 0, "Missing runtime method: " + signature);
        int open = source.IndexOf('{', start);
        int depth = 1;
        int end = open + 1;
        for (; end < source.Length && depth > 0; end++)
        {
            if (source[end] == '{') depth++;
            if (source[end] == '}') depth--;
        }
        return source.Substring(open, end - open);
    }

    private static void HandbackWiring()
    {
        string undo = Body("private static void UndoLights()");
        Equal(true, undo.Contains("BeginHandback(", StringComparison.Ordinal), "the undo still uses the unconfirmed single-shot form");
        Equal(true, undo.Contains("WriteHandbackNative(", StringComparison.Ordinal), "the undo does not confirm its writes");

        string restore = Body("private static void RestoreLightSource(OwnedLightSource source)");
        Equal(true, restore.Contains("BeginHandback(", StringComparison.Ordinal), "teardown still uses the unconfirmed restore");
        Equal(true, restore.Contains("RetainLightRestore(", StringComparison.Ordinal), "an unconfirmed hand-back is not retained");
        Equal(true, restore.Contains("IsProvablyDestroyed(", StringComparison.Ordinal), "a destroyed handle is retained instead of released");
        Equal(true, restore.Contains("Abandon()", StringComparison.Ordinal), "a destroyed handle keeps its claim");

        string retry = Body("public static void RetryPendingLightRestores()");
        Equal(true, retry.Contains("PendingLightRestores", StringComparison.Ordinal), "the retry does not walk the retained set");
        Equal(true, retry.Contains("RestoreLightSource(", StringComparison.Ordinal), "the retry does not re-attempt the hand-back");

        string update = Body("public static void UpdatePhase()");
        int retryCall = update.IndexOf("RetryPendingLightRestores();", StringComparison.Ordinal);
        int readyGate = update.IndexOf("if (!Ready) return;", StringComparison.Ordinal);
        Require(retryCall >= 0, "UpdatePhase never retries a retained native hand-back");
        Require(readyGate > retryCall, "the native retry runs only after the session gate, so it dies with the session");

        string teardown = Body("public static void Teardown()");
        Equal(false, teardown.Contains("PendingLightRestores.Clear", StringComparison.Ordinal), "teardown discards retained native hand-backs");

        string write = Body("private static bool WriteNativeStep(OwnedLightSource source, NativeLightApply step, bool handback)");
        Equal(true, write.Contains("ConfirmIntensityHandback()", StringComparison.Ordinal), "intensity is written without confirmation");
        Equal(true, write.Contains("RejectIntensityHandback()", StringComparison.Ordinal), "a failed intensity hand-back is not rejected");
        Equal(true, write.Contains("ConfirmColorHandback()", StringComparison.Ordinal), "colour is written without confirmation");
        Equal(true, write.Contains("RejectColorHandback()", StringComparison.Ordinal), "a failed colour hand-back is not rejected");

        // The switched-off late-update path hands the light back too; it must use the confirmed form,
        // otherwise a throwing setter drops ownership of a value that is still installed.
        string applySource = Body("private static bool ApplyLightSource(OwnedLightSource source, bool on, VisualInstruction instruction)");
        Equal(true, applySource.Contains("BeginHandback(", StringComparison.Ordinal), "a switched-off lamp still uses the unconfirmed restore");
        Equal(true, applySource.Contains("WriteHandbackNative(", StringComparison.Ordinal), "a switched-off lamp does not confirm its hand-back");
    }

    private static void RereadWiring()
    {
        string restore = Body("private static LedReleaseOutcome RestoreSurface(OwnedSurface surface, Material? preferredNative)");
        Equal(true, restore.Contains("ConfirmRestore(", StringComparison.Ordinal), "the restore releases on the setter's word alone");
        int write = restore.IndexOf("writeSucceeded = true;", StringComparison.Ordinal);
        int reread = restore.IndexOf("ScanForClone(renderer!.sharedMaterials, clone!)", StringComparison.Ordinal);
        Require(write >= 0 && reread > write, "the release proof is not taken after the successful write");

        string scan = Body("private static NativeMaterialArrayScan ScanForClone(Il2CppReferenceArray<Material>? materials, Material clone)");
        Equal(true, scan.Contains("RecordRead(", StringComparison.Ordinal), "a readable null neighbour slot is still treated as a failure");
        Equal(true, scan.Contains("RecordUnreadable()", StringComparison.Ordinal), "a genuinely unreadable element is not recorded");
        Equal(false, scan.Contains("scan.Record(", StringComparison.Ordinal), "the legacy zero-as-error form is still used for a failed read");
    }

    private static void RelocationWiring()
    {
        string install = Body("private static bool InstallSurface(OwnedSurface surface, MeshRenderer renderer, int slot,");
        int relocation = install.IndexOf("LedSurfaceOwnership.RequiresRelocation(", StringComparison.Ordinal);
        int needsClone = install.IndexOf("LedSurfaceOwnership.NeedsClone(", StringComparison.Ordinal);
        Require(relocation >= 0, "the installer ignores a moved MaterialIndex");
        Require(needsClone > relocation, "the previous slot is not handed back before the clone is reused");
        Equal(true, install.Contains("RestoreSurface(surface, preferredNative)", StringComparison.Ordinal), "the previous slot is never restored");
    }

    private static void SurfaceRefreshWiring()
    {
        string apply = Body("private static bool ApplySurface(OwnedLamp lamp, OwnedSurface surface, ToggleableLight toggle, int slot, bool on, VisualInstruction instruction)");
        Equal(true, apply.Contains("surface.LastSlot == slot", StringComparison.Ordinal), "the converged cache ignores a MaterialIndex change");
        Equal(true, apply.Contains("surface.NextRefreshAt", StringComparison.Ordinal), "the converged cache never expires");
        Equal(true, apply.Contains("SurfaceRefreshSeconds", StringComparison.Ordinal), "the refresh cadence is not bounded");
        Equal(true, apply.Contains("MaterialConflictCount", StringComparison.Ordinal), "a foreign visible material is not counted");
        Equal(true, apply.Contains("PreferredNative(toggle, on)", StringComparison.Ordinal), "the installer cannot restore the native material");
    }

    private static void LoggingGuardWiring()
    {
        string lamp = Body("private static void ApplyLamp(OwnedLamp lamp, VisualInstruction instruction)");
        Equal(true, lamp.Contains("MaybeLogApplied(", StringComparison.Ordinal), "the breadcrumb is built unconditionally");

        string guard = Body("private static void MaybeLogApplied(OwnedLamp lamp, int slot, VisualInstruction instruction)");
        int comparison = guard.IndexOf("LastLoggedInstruction", StringComparison.Ordinal);
        int expensive = guard.IndexOf("LogApplied(", StringComparison.Ordinal);
        Require(comparison >= 0, "the guard does not compare the last logged instruction");
        Require(expensive > comparison, "the expensive breadcrumb runs before the cheap guard");

        string log = Body("private static void LogApplied(OwnedLamp lamp, int slot, VisualInstruction instruction)");
        Equal(true, log.Contains("LastLoggedInstruction = instruction;", StringComparison.Ordinal), "the guard state is never recorded");
        Equal(true, log.Contains("LastLoggedSlot = slot;", StringComparison.Ordinal), "the guarded slot is never recorded");
    }
}
