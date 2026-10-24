// ---------------------------------------------------------------------------
// Core/NativePolicies.cs
//
// Mechanically consolidated from the former per-type source files. Each original
// file keeps its own block namespace with its own using directives, so name
// binding is unchanged; the bodies differ from the pre-consolidation sources only
// by the block-namespace indent level. Machine markers delimit the section that
// came from each original file so the source-contract checks and an independent
// reconstruction proof can still read that file's exact content.
//
//   Core/NativeLightPolicy.cs          -> section NativeLightPolicy.cs
//   Core/NativeColorPolicy.cs          -> section NativeColorPolicy.cs
//   Core/NativeCouplingPolicy.cs       -> section NativeCouplingPolicy.cs
//   Core/NativeLightBinding.cs         -> section NativeLightBinding.cs
//   Core/NativeMaterialIdentity.cs     -> section NativeMaterialIdentity.cs
// ---------------------------------------------------------------------------

namespace LightControl.Core
{
    // ==== LightControl section: NativeLightPolicy.cs ====
    using System;


    /// <summary>
    /// One phase step for a single native light source. <see cref="ShouldWrite"/> false means the
    /// adapter must leave the native value untouched (invalid sample, overflow or nothing owned).
    /// <see cref="Intensity"/> is the exact float the adapter must write, so the value can be compared
    /// back after a native float round-trip.
    /// </summary>
    public readonly record struct NativeLightStep(bool ShouldWrite, float Intensity, bool Owned);

    /// <summary>
    /// Unity-free ownership model for one native light source. The adapter runs two isolated phases:
    /// <see cref="UndoOwned"/> in the update phase (so the game's own writer sees its value again and
    /// our output can never be re-scaled) and <see cref="Apply"/> in the late-update phase against a
    /// freshly sampled, unmodified native value. Ownership is only ever claimed for a write that
    /// actually happened; <see cref="RejectWrite"/> releases it when the native setter throws.
    /// No game, Unity or shader dependency.
    /// </summary>
    public sealed class NativeLightPolicy
    {
        private bool _owned;
        private double _baseline;
        private float _lastWritten;
        private bool _handbackPending;
        private bool _snapshotTaken;
        private bool _snapshotOwned;
        private double _snapshotBaseline;
        private float _snapshotLastWritten;

        /// <summary>True while the adapter's own output is currently installed on the native light.</summary>
        public bool Owned => _owned;

        /// <summary>The last freshly sampled, unmodified native intensity (never our own output).</summary>
        public double Baseline => _baseline;

        /// <summary>
        /// Update phase: hand the light back to the game's own writer by restoring the last sampled
        /// baseline. Writes nothing when the adapter owns nothing.
        /// </summary>
        /// <remarks>
        /// LC01: single-shot convenience form for callers that cannot confirm the setter. It is
        /// <see cref="BeginHandback"/> plus <see cref="ConfirmHandback"/>; the adapter uses the
        /// confirmed form so a throwing setter cannot drop ownership of a value still installed.
        /// </remarks>
        public NativeLightStep UndoOwned(double currentNativeIntensity)
        {
            NativeLightStep step = BeginHandback(currentNativeIntensity);
            ConfirmHandback();
            return step;
        }

        /// <summary>
        /// Late-update phase: rebase on a freshly sampled native value and apply the resolved
        /// instruction. A factor of one releases ownership and restores the latest sampled baseline;
        /// an invalid or overflowing sample writes nothing.
        /// </summary>
        /// <remarks>
        /// LC01: the update-phase undo can legitimately fail to run (its native getter threw, the source
        /// was not alive for one frame). The late-update sample is then <i>this adapter's own output</i>,
        /// not a native baseline, and scaling it would compound 10 → 20 → 40 even though nothing else
        /// wrote. While the sampled value still equals the exact float we installed, the retained
        /// baseline is the only legal base, so a missed undo can never compound.
        /// </remarks>
        public NativeLightStep Apply(double sampledNativeIntensity, VisualInstruction instruction)
        {
            if (instruction is null) throw new ArgumentNullException(nameof(instruction));
            if (!IsValidSample(sampledNativeIntensity)) return new(false, 0, _owned);
            TakeSnapshot();
            // A new instruction supersedes an unfinished hand-back: a stale confirmation must not be
            // able to release ownership of the output this apply is about to install.
            _handbackPending = false;
            bool ownOutputInstalled = _owned && MatchesOwnedOutput(sampledNativeIntensity);
            double native = ownOutputInstalled ? _baseline : sampledNativeIntensity;
            if (instruction.RemovesModInfluence)
            {
                _baseline = native;
                _owned = false;
                return new(true, (float)native, false);
            }
            double output;
            try { output = instruction.ScaleCurrentNativeIntensity(native); }
            catch (ArgumentOutOfRangeException) { return new(false, 0, _owned); }
            _baseline = native;
            _lastWritten = (float)output;
            _owned = true;
            return new(true, _lastWritten, true);
        }

        /// <summary>Teardown: restore the latest baseline once and release ownership.</summary>
        public NativeLightStep Restore(double currentNativeIntensity) => UndoOwned(currentNativeIntensity);

        /// <summary>
        /// Update/teardown phase, first half: the exact native baseline to write back, emitted while
        /// ownership is still held. Nothing is written when nothing of ours is installed, and an
        /// external write is preserved (adopted as the new baseline) without any native write.
        /// </summary>
        public NativeLightStep BeginHandback(double currentNativeIntensity)
        {
            _snapshotTaken = false;
            if (!_owned) return new(false, 0, false);
            if (!MatchesOwnedOutput(currentNativeIntensity))
            {
                // An external writer changed the value: preserve it, release ownership, write nothing.
                if (IsValidSample(currentNativeIntensity)) _baseline = currentNativeIntensity;
                _owned = false;
                _handbackPending = false;
                return new(false, 0, false);
            }
            _handbackPending = true;
            return new(true, (float)_baseline, false);
        }

        /// <summary>Second half: the hand-back write actually happened, so our output is gone.</summary>
        public void ConfirmHandback()
        {
            if (!_handbackPending) return;
            _handbackPending = false;
            _owned = false;
        }

        /// <summary>
        /// Second half: the hand-back setter threw. Our own output is <i>still installed</i>, so the
        /// ownership and the retained native baseline must both survive and the hand-back is retried
        /// on a later frame instead of being silently abandoned.
        /// </summary>
        public void RejectHandback() => _handbackPending = false;

        /// <summary>
        /// Drops every claim without writing. Only for a native object that is provably destroyed, where
        /// there is no value left to hand back and retaining it would leak a dead handle forever.
        /// </summary>
        public void Abandon()
        {
            _owned = false;
            _handbackPending = false;
            _snapshotTaken = false;
        }

        /// <summary>The adapter's native setter threw: never claim ownership of a value we did not write.</summary>
        /// <remarks>
        /// LC01: the failed setter changed nothing on the light, so the state before this apply is
        /// restored exactly. A source that already carried our earlier output keeps its ownership and
        /// baseline; a source that carried nothing of ours keeps claiming nothing.
        /// </remarks>
        public void RejectWrite()
        {
            if (!_snapshotTaken) { _owned = false; return; }
            _snapshotTaken = false;
            _owned = _snapshotOwned;
            _baseline = _snapshotBaseline;
            _lastWritten = _snapshotLastWritten;
        }

        /// <summary>True between <see cref="BeginHandback"/> and its confirmation or rejection.</summary>
        public bool HandbackPending => _handbackPending;

        private void TakeSnapshot()
        {
            _snapshotTaken = true;
            _snapshotOwned = _owned;
            _snapshotBaseline = _baseline;
            _snapshotLastWritten = _lastWritten;
        }

        /// <summary>
        /// True only while the native value still equals the exact float we wrote, compared as floats so
        /// a float-to-double round-trip is never mistaken for an external write.
        /// </summary>
        private bool MatchesOwnedOutput(double currentNativeIntensity) =>
            IsValidSample(currentNativeIntensity) && (float)currentNativeIntensity == _lastWritten;

        public static bool IsValidSample(double value) => double.IsFinite(value) && value >= 0;
    }
    // ==== end LightControl section: NativeLightPolicy.cs ====
}

namespace LightControl.Core
{
    // ==== LightControl section: NativeColorPolicy.cs ====
    using System;


    /// <summary>
    /// One color phase step. <see cref="ShouldWrite"/> false leaves the native color untouched.
    /// The RGB(A) fields are the exact floats to write, comparable after a native round-trip.
    /// </summary>
    public readonly record struct NativeColorStep(bool ShouldWrite, float R, float G, float B, float A, bool Owned);

    /// <summary>
    /// Unity-free ownership model for the native light color, mirroring <see cref="NativeLightPolicy"/>.
    /// Color is absolute (never scaled) but it is still only restored while the native value still
    /// equals our own last write: an external color change is preserved and releases ownership so the
    /// next <see cref="Apply"/> adopts the fresh native color.
    /// </summary>
    public sealed class NativeColorPolicy
    {
        private bool _owned;
        private float _baseR, _baseG, _baseB, _baseA;
        private float _outR, _outG, _outB, _outA;
        private bool _handbackPending;
        private bool _snapshotTaken;
        private bool _snapshotOwned;
        private float _snapshotR, _snapshotG, _snapshotB, _snapshotA;

        public bool Owned => _owned;

        /// <summary>Update phase: restore the last sampled baseline color only while we still own it.</summary>
        public NativeColorStep UndoOwned(float r, float g, float b, float a)
        {
            NativeColorStep step = BeginHandback(r, g, b, a);
            ConfirmHandback();
            return step;
        }

        /// <summary>
        /// LC01, colour half of the confirmed hand-back: the exact native baseline colour is emitted
        /// while ownership is still held, so a throwing setter can never drop it.
        /// </summary>
        public NativeColorStep BeginHandback(float r, float g, float b, float a)
        {
            _snapshotTaken = false;
            if (!_owned) return new(false, 0, 0, 0, 0, false);
            if (!MatchesOwnedOutput(r, g, b, a))
            {
                // An external writer changed the color: preserve it, release ownership, write nothing.
                if (IsValid(r, g, b, a)) { _baseR = r; _baseG = g; _baseB = b; _baseA = a; }
                _owned = false;
                _handbackPending = false;
                return new(false, 0, 0, 0, 0, false);
            }
            _handbackPending = true;
            return new(true, _baseR, _baseG, _baseB, _baseA, false);
        }

        /// <summary>Second half: the colour hand-back write actually happened, so our output is gone.</summary>
        public void ConfirmHandback()
        {
            if (!_handbackPending) return;
            _handbackPending = false;
            _owned = false;
        }

        /// <summary>
        /// Second half: the colour setter threw, so our own colour is still installed and ownership
        /// (with its baseline) is retained for a later retry.
        /// </summary>
        public void RejectHandback() => _handbackPending = false;

        /// <summary>True between <see cref="BeginHandback"/> and its confirmation or rejection.</summary>
        public bool HandbackPending => _handbackPending;

        /// <summary>Late-update phase: sample a fresh native color and install the resolved override.</summary>
        /// <remarks>
        /// LC01, colour half: like intensity, a sampled colour that still equals our own installed output
        /// is not a native baseline and must never replace the retained one.
        /// </remarks>
        public NativeColorStep Apply(float r, float g, float b, float a, RgbColor? overrideColor)
        {
            if (overrideColor is null || !IsValid(r, g, b, a)) return new(false, 0, 0, 0, 0, _owned);
            TakeSnapshot();
            _handbackPending = false;
            bool ownOutputInstalled = _owned && MatchesOwnedOutput(r, g, b, a);
            if (!ownOutputInstalled) { _baseR = r; _baseG = g; _baseB = b; _baseA = a; }
            _outR = (float)overrideColor.R; _outG = (float)overrideColor.G; _outB = (float)overrideColor.B;
            _outA = ownOutputInstalled ? _baseA : a;
            _owned = true;
            return new(true, _outR, _outG, _outB, _outA, true);
        }

        /// <summary>Teardown: restore the last sampled baseline color once and release ownership.</summary>
        public NativeColorStep Restore(float r, float g, float b, float a) => UndoOwned(r, g, b, a);

        /// <summary>
        /// Drops every claim without writing. Only for a native object that is provably destroyed, where
        /// there is no colour left to hand back.
        /// </summary>
        public void Abandon()
        {
            _owned = false;
            _handbackPending = false;
            _snapshotTaken = false;
        }

        /// <summary>The native color setter threw: never claim ownership of a value we did not write.</summary>
        /// <remarks>
        /// LC01, colour half: a failed setter changed nothing, so the pre-apply ownership and baseline
        /// colour are restored instead of dropping a colour that is still installed.
        /// </remarks>
        public void RejectWrite()
        {
            if (!_snapshotTaken) { _owned = false; return; }
            _snapshotTaken = false;
            _owned = _snapshotOwned;
            _baseR = _snapshotR; _baseG = _snapshotG; _baseB = _snapshotB; _baseA = _snapshotA;
        }

        private void TakeSnapshot()
        {
            _snapshotTaken = true;
            _snapshotOwned = _owned;
            _snapshotR = _baseR; _snapshotG = _baseG; _snapshotB = _baseB; _snapshotA = _baseA;
        }

        /// <summary>True only while the native color still equals the exact floats we wrote.</summary>
        private bool MatchesOwnedOutput(float r, float g, float b, float a) =>
            IsValid(r, g, b, a) && r == _outR && g == _outG && b == _outB && a == _outA;

        public static bool IsValid(float r, float g, float b, float a) =>
            float.IsFinite(r) && float.IsFinite(g) && float.IsFinite(b) && float.IsFinite(a);
    }
    // ==== end LightControl section: NativeColorPolicy.cs ====
}

namespace LightControl.Core
{
    // ==== LightControl section: NativeCouplingPolicy.cs ====
    using System;


    /// <summary>
    /// Whether one discovered grow lamp may be registered for coupled panel + emitted-light control.
    /// </summary>
    public enum LampCouplingState
    {
        /// <summary>At least one output is not resolvable yet and the lamp is not on: retry later, never enable by force.</summary>
        Pending = 0,
        /// <summary>Both the LED panel slot and the lamp's own native light source are resolvable.</summary>
        Ready = 1,
        /// <summary>The lamp is on but an output can never be resolved: report it, never fall back to one output.</summary>
        Unsupported = 2
    }

    /// <summary>
    /// Unity-free readiness model for the coupled grow-lamp registration. The owner's requirement is that
    /// the LED panel and the light it emits always show the same colour, so a lamp is only ever controlled
    /// while <i>both</i> outputs are resolvable - the mode fails closed instead of silently falling back to
    /// the panel alone.
    /// <para>
    /// A grow lamp that is switched off has not necessarily resolved its light yet: the shipped LEDLight
    /// prefab's <c>OptimizedLight._Light</c> reference is null in the asset and resolves at runtime, so an
    /// off lamp may legitimately expose no source on this frame. That case is
    /// <see cref="LampCouplingState.Pending"/> and is retried at a bounded cadence. It is never resolved by
    /// forcing the lamp on: the native switch state is not the mod's to change.
    /// </para>
    /// No game, Unity or shader dependency.
    /// </summary>
    public static class NativeCouplingPolicy
    {
        /// <summary>
        /// Decides whether a lamp may be registered, once the verified LEDLight family gate has passed.
        /// </summary>
        /// <param name="hasSurface">At least one verified emitting LED panel slot was found.</param>
        /// <param name="hasLightSource">At least one of the lamp's own native light sources resolved to a live Unity Light.</param>
        /// <param name="lampOn">The lamp's live native switch state.</param>
        public static LampCouplingState Decide(bool hasSurface, bool hasLightSource, bool lampOn)
        {
            if (hasSurface && hasLightSource) return LampCouplingState.Ready;
            // Nothing is resolvable and the lamp is off: its light may simply not exist yet. Retry.
            if (!lampOn) return LampCouplingState.Pending;
            // The lamp is on, so its source list is live; a missing output is a real, reportable gap.
            return LampCouplingState.Unsupported;
        }
    }
    // ==== end LightControl section: NativeCouplingPolicy.cs ====
}

namespace LightControl.Core
{
    // ==== LightControl section: NativeLightBinding.cs ====
    using System;


    /// <summary>
    /// One coordinated phase step for the two native properties of a single light source. Each half is the
    /// exact step of its own proven policy, so the adapter can perform the two native writes independently
    /// and reject only the one whose setter failed.
    /// </summary>
    public readonly record struct NativeLightApply(NativeLightStep Intensity, NativeColorStep Color);

    /// <summary>
    /// Unity-free coordination of the two proven ownership policies for one native light source:
    /// <see cref="NativeLightPolicy"/> for intensity (relative - the chosen factor scales the lamp's own
    /// freshly sampled baseline) and <see cref="NativeColorPolicy"/> for colour (absolute - the chosen hue
    /// replaces the native colour, because a colour factor would be meaningless).
    /// <para>
    /// This type invents no algorithm: it only sequences the two policies that already carry the
    /// exact-output ownership guarantee, so an external write on either property is preserved, ownership is
    /// claimed only for a write that actually happened, and each property can be released on its own when
    /// its native setter throws.
    /// </para>
    /// <para>
    /// Colour needs the update-phase undo to be safe: <see cref="NativeColorPolicy.Apply"/> with no override
    /// deliberately leaves ownership untouched, so a released colour output must be handed back in
    /// <see cref="Undo"/> before the late-update phase can leave it alone, and again in
    /// <see cref="Restore"/> on teardown.
    /// </para>
    /// No game, Unity or shader dependency.
    /// </summary>
    public sealed class NativeLightBinding
    {
        private readonly NativeLightPolicy _intensity = new();
        private readonly NativeColorPolicy _color = new();

        /// <summary>True while our own intensity output is currently installed on the native light.</summary>
        public bool IntensityOwned => _intensity.Owned;

        /// <summary>True while our own colour output is currently installed on the native light.</summary>
        public bool ColorOwned => _color.Owned;

        /// <summary>True while either property still carries this adapter's own output.</summary>
        public bool OwnsAny => _intensity.Owned || _color.Owned;

        /// <summary>Original at 100% needs no native read/write unless a prior output still needs hand-back.</summary>
        public bool NeedsApply(VisualInstruction instruction) => OwnsAny || !instruction.RemovesModInfluence;

        /// <summary>
        /// Update phase: hand both native properties back to the game's own writer, so the game's value can
        /// never be re-scaled and a released output is not left installed. Writes nothing for a property the
        /// adapter does not own, and preserves (and adopts) any external change instead of clobbering it.
        /// </summary>
        public NativeLightApply Undo(double currentIntensity, float r, float g, float b, float a) =>
            new(_intensity.UndoOwned(currentIntensity), _color.UndoOwned(r, g, b, a));

        /// <summary>
        /// Late-update phase: rebase both properties on freshly sampled native values and install the
        /// resolved instruction. Intensity scales the sampled native value by the chosen factor (never our
        /// own output, so it cannot compound); colour is written absolutely. An instruction that removes all
        /// mod influence releases both properties.
        /// </summary>
        public NativeLightApply Apply(double sampledIntensity, float r, float g, float b, float a, VisualInstruction instruction)
        {
            if (instruction is null) throw new ArgumentNullException(nameof(instruction));
            return new(
                _intensity.Apply(sampledIntensity, instruction),
                _color.Apply(r, g, b, a, instruction.ColorOverride));
        }

        /// <summary>
        /// Teardown, demote, removal or rebind: restore the latest sampled baseline of both properties once,
        /// but only while each still equals this adapter's own output. An external change is preserved.
        /// </summary>
        public NativeLightApply Restore(double currentIntensity, float r, float g, float b, float a) =>
            new(_intensity.Restore(currentIntensity), _color.Restore(r, g, b, a));

        /// <summary>The native intensity setter threw: never claim ownership of a value we did not write.</summary>
        public void RejectIntensityWrite() => _intensity.RejectWrite();

        /// <summary>True while either property still owes a confirmed hand-back write.</summary>
        public bool HandbackPending => _intensity.HandbackPending || _color.HandbackPending;

        /// <summary>
        /// Update-phase and teardown hand-back, first half: the exact native baseline of both
        /// properties is emitted while ownership is still held. The adapter performs the two writes
        /// independently and must confirm or reject each one, so a failed setter can never drop
        /// ownership of a value that is still installed.
        /// </summary>
        public NativeLightApply BeginHandback(double sampledIntensity, float r, float g, float b, float a) =>
            new(_intensity.BeginHandback(sampledIntensity), _color.BeginHandback(r, g, b, a));

        /// <summary>The native intensity hand-back write actually happened.</summary>
        public void ConfirmIntensityHandback() => _intensity.ConfirmHandback();

        /// <summary>The native intensity hand-back setter threw: keep ownership and retry later.</summary>
        public void RejectIntensityHandback() => _intensity.RejectHandback();

        /// <summary>The native colour hand-back write actually happened.</summary>
        public void ConfirmColorHandback() => _color.ConfirmHandback();

        /// <summary>The native colour hand-back setter threw: keep ownership and retry later.</summary>
        public void RejectColorHandback() => _color.RejectHandback();

        /// <summary>The native colour setter threw: never claim ownership of a value we did not write.</summary>
        public void RejectColorWrite() => _color.RejectWrite();

        /// <summary>Both native setters are unusable for this source: release every claimed output.</summary>
        public void RejectWrite()
        {
            _intensity.RejectWrite();
            _color.RejectWrite();
        }

        /// <summary>
        /// The native light is provably destroyed: drop every claim without writing, so a retained
        /// hand-back never holds a dead handle forever (LC01).
        /// </summary>
        public void Abandon()
        {
            _intensity.Abandon();
            _color.Abandon();
        }
    }
    // ==== end LightControl section: NativeLightBinding.cs ====
}

namespace LightControl.Core
{
    // ==== LightControl section: NativeMaterialIdentity.cs ====
    using System;


    /// <summary>
    /// Unity-free identity model for a native material handle.
    /// <para>
    /// IL2CPP hands out a fresh managed wrapper around the same native object on every array or property
    /// read, so managed reference equality is never proof that two reads are the same native material: it
    /// fails for the same material read twice, and it can only ever be a fast path. The durable identity
    /// is the nonzero native pointer. A handle whose pointer cannot be read — a disposed or collected
    /// wrapper throws on <c>Pointer</c> — is <i>unknown</i>: it is never equal to anything, and it can
    /// never prove that a renderer stopped referencing our material.
    /// </para>
    /// <para>
    /// No <c>operator==</c> is used anywhere here. Unity's overloaded equality performs a fake-null check
    /// that treats a present-but-destroyed object as null, which would silently turn an identity question
    /// into a liveness question.
    /// </para>
    /// </summary>
    public static class NativeMaterialIdentity
    {
        /// <summary>A native id is usable only when it is nonzero; zero means "unreadable/unknown".</summary>
        public static bool IsUsable(long nativeId) => nativeId != 0;

        /// <summary>True only when both native ids are usable and identical.</summary>
        public static bool SameNative(long leftId, long rightId) => leftId != 0 && leftId == rightId;

        /// <summary>
        /// Generic adapter seam: true only when both handles are non-null and resolve to the same usable
        /// native id. A managed reference match is accepted as a fast path without reading the pointer; a
        /// missing handle, a zero id or a reader that throws all resolve to "not the same material".
        /// </summary>
        /// <param name="left">The first handle.</param>
        /// <param name="right">The second handle.</param>
        /// <param name="nativeId">Reads a handle's native id; may throw for a disposed wrapper.</param>
        /// <param name="referenceEquals">Optional managed fast path; defaults to <see cref="object.ReferenceEquals"/>.</param>
        public static bool SameNative<T>(T? left, T? right, Func<T, long> nativeId, Func<T?, T?, bool>? referenceEquals = null) where T : class
        {
            if (left is null || right is null) return false;
            Func<T?, T?, bool> sameReference = referenceEquals ?? (static (a, b) => ReferenceEquals(a, b));
            if (sameReference(left, right)) return true;
            long leftId, rightId;
            try
            {
                leftId = nativeId(left);
                rightId = nativeId(right);
            }
            catch { return false; }
            return SameNative(leftId, rightId);
        }
    }

    /// <summary>
    /// Unity-free accumulator for one renderer material-array scan that looks for our own clone.
    /// <para>
    /// A scan can only prove the clone absent when every element's handle was readable. One unreadable
    /// (disposed or collected) element makes the scan incomplete, and an incomplete scan must never be
    /// treated as a disconnect proof — otherwise a material the renderer still points at would be
    /// destroyed. A clone id that cannot be read is equally unprovable.
    /// </para>
    /// </summary>
    public sealed class NativeMaterialArrayScan
    {
        /// <summary>True while every element inspected so far could be read.</summary>
        public bool FullyReadable { get; private set; } = true;

        /// <summary>True once an element resolved to our own clone.</summary>
        public bool ReferencesClone { get; private set; }

        /// <summary>True only when the whole array was readable and no element is our clone.</summary>
        public bool ProvesDisconnected => FullyReadable && !ReferencesClone;

        /// <summary>Marks the scan incomplete: a whole array could not be read at all.</summary>
        public void MarkUnreadable() => FullyReadable = false;

        /// <summary>
        /// Records one element: its readable native id (0 when the handle is unreadable or disposed) and
        /// the native id of the clone we are looking for.
        /// </summary>
        /// <remarks>
        /// Legacy single-id form: a zero id is treated as an unreadable element. Use
        /// <see cref="RecordRead"/> when the adapter knows the element read succeeded, so a legitimate
        /// null neighbour slot is not mistaken for a failed read (LC04).
        /// </remarks>
        public void Record(long candidateNativeId, long cloneNativeId)
        {
            if (!NativeMaterialIdentity.IsUsable(candidateNativeId) || !NativeMaterialIdentity.IsUsable(cloneNativeId))
            {
                MarkUnreadable();
                return;
            }
            if (candidateNativeId == cloneNativeId) ReferencesClone = true;
        }

        /// <summary>
        /// Records one element whose handle read <i>succeeded</i>. A material array may legitimately
        /// contain a null (empty) element: that is a readable id of zero, not a failure, and it must
        /// not block the disconnect proof. Only an unidentified <paramref name="cloneNativeId"/>
        /// (our own handle unreadable) still makes the scan unable to prove anything.
        /// </summary>
        public void RecordRead(long candidateNativeId, long cloneNativeId)
        {
            if (!NativeMaterialIdentity.IsUsable(cloneNativeId))
            {
                MarkUnreadable();
                return;
            }
            if (NativeMaterialIdentity.SameNative(candidateNativeId, cloneNativeId)) ReferencesClone = true;
        }

        /// <summary>Records one element whose handle read threw or whose wrapper was disposed.</summary>
        public void RecordUnreadable() => MarkUnreadable();
    }
    // ==== end LightControl section: NativeMaterialIdentity.cs ====
}

namespace LightControl.Core
{
    /// <summary>Backpressure blocks new native ownership, never drops responsibility for existing output.</summary>
    public static class NativeLightAdmissionPolicy
    {
        public static bool MayApply(bool alreadyOwned, int pendingCount, int softLimit) =>
            alreadyOwned || (softLimit > 0 && pendingCount >= 0 && pendingCount < softLimit);
    }

    // ==== LightControl section: PanelFamilyPolicy.cs ====
    /// <summary>
    /// Which native on/off panel pairs this build may recolour. Unity-free.
    /// The LED pair is the placeable LED grow light. The halogen pair is the built-in light of a
    /// grow tent (<c>halogen light on mat</c> / <c>halogen light off mat</c> in sharedassets0).
    /// Full-spectrum and every other name stay rejected.
    /// </summary>
    public static class PanelFamilyPolicy
    {
        public const string LedOn = "ledlight_light_on";
        public const string LedOff = "ledlight_light_off";
        public const string HalogenOn = "halogen light on mat";
        public const string HalogenOff = "halogen light off mat";
        public const string GrowTentItemId = "growtent";

        public static bool IsVerifiedPair(string? onName, string? offName)
        {
            string on = Strip(onName);
            string off = Strip(offName);
            return Pair(on, off, LedOn, LedOff) || Pair(on, off, HalogenOn, HalogenOff);
        }

        public static bool IsVerifiedPanelName(string? name)
        {
            string stripped = Strip(name);
            return string.Equals(stripped, LedOn, StringComparison.Ordinal)
                || string.Equals(stripped, LedOff, StringComparison.Ordinal)
                || string.Equals(stripped, HalogenOn, StringComparison.Ordinal)
                || string.Equals(stripped, HalogenOff, StringComparison.Ordinal);
        }

        /// <summary>Only a grow tent contributes its built-in light. A loose pot does not.</summary>
        public static bool IsGrowTent(string? itemId) =>
            string.Equals(itemId?.Trim(), GrowTentItemId, StringComparison.OrdinalIgnoreCase);

        private static bool Pair(string on, string off, string expectedOn, string expectedOff) =>
            string.Equals(on, expectedOn, StringComparison.Ordinal)
            && string.Equals(off, expectedOff, StringComparison.Ordinal);

        private static string Strip(string? name)
        {
            string value = (name ?? string.Empty).Trim();
            const string suffix = " (Instance)";
            return value.EndsWith(suffix, StringComparison.Ordinal) ? value.Substring(0, value.Length - suffix.Length) : value;
        }
    }
    // ==== end LightControl section: PanelFamilyPolicy.cs ====
}

namespace LightControl.Core
{
    // ==== LightControl section: DestroyedPropertyPolicy.cs ====
    /// <summary>
    /// The prologue RV is never tracked. It is only playable in the prologue, and after that quest
    /// it explodes while its lights stay alive, so waiting for <c>IsDestroyed</c> is the wrong gate.
    /// A normal owned property is tracked. Unity-free.
    /// </summary>
    public static class DestroyedPropertyPolicy
    {
        public const string ProloguePropertyCode = "rv";

        public static bool IsPrologueOnly(string? propertyCode, bool isRvInstance)
        {
            if (isRvInstance) return true;
            return string.Equals(propertyCode?.Trim(), ProloguePropertyCode, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPermanentlyGone(bool lampDestroyed, bool parentUntracked) =>
            lampDestroyed || parentUntracked;
    }
    // ==== end LightControl section: DestroyedPropertyPolicy.cs ====
}
