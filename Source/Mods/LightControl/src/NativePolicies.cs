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

        /// <summary>True while the adapter's own output is currently installed on the native light.</summary>
        public bool Owned => _owned;

        /// <summary>The last freshly sampled, unmodified native intensity (never our own output).</summary>
        public double Baseline => _baseline;

        /// <summary>
        /// Update phase: hand the light back to the game's own writer by restoring the last sampled
        /// baseline. Writes nothing when the adapter owns nothing.
        /// </summary>
        public NativeLightStep UndoOwned(double currentNativeIntensity)
        {
            if (!_owned) return new(false, 0, false);
            if (!MatchesOwnedOutput(currentNativeIntensity))
            {
                // An external writer changed the value: preserve it, release ownership, write nothing.
                if (IsValidSample(currentNativeIntensity)) _baseline = currentNativeIntensity;
                _owned = false;
                return new(false, 0, false);
            }
            _owned = false;
            return new(true, (float)_baseline, false);
        }

        /// <summary>
        /// Late-update phase: rebase on a freshly sampled native value and apply the resolved
        /// instruction. A factor of one releases ownership and restores the latest sampled baseline;
        /// an invalid or overflowing sample writes nothing.
        /// </summary>
        public NativeLightStep Apply(double sampledNativeIntensity, VisualInstruction instruction)
        {
            if (instruction is null) throw new ArgumentNullException(nameof(instruction));
            if (!IsValidSample(sampledNativeIntensity)) return new(false, 0, _owned);
            if (instruction.RemovesModInfluence)
            {
                _baseline = sampledNativeIntensity;
                _owned = false;
                return new(true, (float)sampledNativeIntensity, false);
            }
            double output;
            try { output = instruction.ScaleCurrentNativeIntensity(sampledNativeIntensity); }
            catch (ArgumentOutOfRangeException) { return new(false, 0, _owned); }
            _baseline = sampledNativeIntensity;
            _lastWritten = (float)output;
            _owned = true;
            return new(true, _lastWritten, true);
        }

        /// <summary>Teardown: restore the latest baseline once and release ownership.</summary>
        public NativeLightStep Restore(double currentNativeIntensity)
        {
            if (!_owned) return new(false, 0, false);
            if (!MatchesOwnedOutput(currentNativeIntensity))
            {
                // An external writer changed the value: preserve it, release ownership, write nothing.
                if (IsValidSample(currentNativeIntensity)) _baseline = currentNativeIntensity;
                _owned = false;
                return new(false, 0, false);
            }
            _owned = false;
            return new(true, (float)_baseline, false);
        }

        /// <summary>The adapter's native setter threw: never claim ownership of a value we did not write.</summary>
        public void RejectWrite() => _owned = false;

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

        public bool Owned => _owned;

        /// <summary>Update phase: restore the last sampled baseline color only while we still own it.</summary>
        public NativeColorStep UndoOwned(float r, float g, float b, float a)
        {
            if (!_owned) return new(false, 0, 0, 0, 0, false);
            if (!MatchesOwnedOutput(r, g, b, a))
            {
                // An external writer changed the color: preserve it, release ownership, write nothing.
                if (IsValid(r, g, b, a)) { _baseR = r; _baseG = g; _baseB = b; _baseA = a; }
                _owned = false;
                return new(false, 0, 0, 0, 0, false);
            }
            _owned = false;
            return new(true, _baseR, _baseG, _baseB, _baseA, false);
        }

        /// <summary>Late-update phase: sample a fresh native color and install the resolved override.</summary>
        public NativeColorStep Apply(float r, float g, float b, float a, RgbColor? overrideColor)
        {
            if (overrideColor is null || !IsValid(r, g, b, a)) return new(false, 0, 0, 0, 0, _owned);
            _baseR = r; _baseG = g; _baseB = b; _baseA = a;
            _outR = (float)overrideColor.R; _outG = (float)overrideColor.G; _outB = (float)overrideColor.B; _outA = a;
            _owned = true;
            return new(true, _outR, _outG, _outB, _outA, true);
        }

        /// <summary>Teardown: restore the last sampled baseline color once and release ownership.</summary>
        public NativeColorStep Restore(float r, float g, float b, float a)
        {
            if (!_owned) return new(false, 0, 0, 0, 0, false);
            if (!MatchesOwnedOutput(r, g, b, a))
            {
                // An external writer changed the color: preserve it, release ownership, write nothing.
                if (IsValid(r, g, b, a)) { _baseR = r; _baseG = g; _baseB = b; _baseA = a; }
                _owned = false;
                return new(false, 0, 0, 0, 0, false);
            }
            _owned = false;
            return new(true, _baseR, _baseG, _baseB, _baseA, false);
        }

        /// <summary>The native color setter threw: never claim ownership of a value we did not write.</summary>
        public void RejectWrite() => _owned = false;

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

        /// <summary>The native colour setter threw: never claim ownership of a value we did not write.</summary>
        public void RejectColorWrite() => _color.RejectWrite();

        /// <summary>Both native setters are unusable for this source: release every claimed output.</summary>
        public void RejectWrite()
        {
            _intensity.RejectWrite();
            _color.RejectWrite();
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
        public void Record(long candidateNativeId, long cloneNativeId)
        {
            if (!NativeMaterialIdentity.IsUsable(candidateNativeId) || !NativeMaterialIdentity.IsUsable(cloneNativeId))
            {
                MarkUnreadable();
                return;
            }
            if (candidateNativeId == cloneNativeId) ReferencesClone = true;
        }
    }
    // ==== end LightControl section: NativeMaterialIdentity.cs ====
}
