using System;

namespace LightControl.Core;

/// <summary>
/// What the adapter observed in the one material slot it is allowed to touch on a grow-lamp
/// surface mesh. The adapter never inspects or writes any other slot.
/// </summary>
public enum LedSlotState
{
    /// <summary>The slot index is out of range or holds no material.</summary>
    Empty = 0,
    /// <summary>The slot holds a native, verified LED panel material (the lamp's own emitting surface).</summary>
    NativePanel = 1,
    /// <summary>The slot already holds this policy's own material clone.</summary>
    OwnClone = 2,
    /// <summary>The slot holds anything else (unsupported shader, another writer, native off material).</summary>
    Foreign = 3
}

/// <summary>What the adapter must do with the target slot this frame.</summary>
public enum LedSurfaceAction
{
    /// <summary>Leave the slot exactly as it is.</summary>
    None = 0,
    /// <summary>Ensure the policy's own clone occupies the slot with the given emission colour.</summary>
    Install = 1,
    /// <summary>Put the exact native material back, but only while the slot still holds our clone.</summary>
    Restore = 2
}

/// <summary>
/// One surface phase step. <see cref="LedSurfaceAction.None"/> means the adapter must not write.
/// The RGB fields are the exact floats to install as <c>_EmissionColor</c>.
/// </summary>
public readonly record struct LedSurfaceStep(LedSurfaceAction Action, float R, float G, float B, bool Owned);

/// <summary>
/// Unity-free ownership and colour model for the one emitting LED surface slot of a grow lamp.
/// <para>
/// The lamp's visible LED panel is a URP material whose glow is its HDR <c>_EmissionColor</c>. This
/// policy never touches a Unity <c>Light</c>: it only decides what colour the lamp's own emitting
/// surface must show. The adapter installs an owned clone of the native material into the exact
/// verified slot, so shared/world materials are never mutated and the plant/world illumination is
/// left completely untouched.
/// </para>
/// <para>
/// Gating rules (all enforced here, not in the adapter):
/// <list type="bullet">
/// <item>a switched-off lamp is never recoloured — the native off material always wins;</item>
/// <item>an unsupported or foreign slot is never replaced;</item>
/// <item>an empty slot is never created;</item>
/// <item>the value always scales a freshly sampled native baseline, so it can never compound;</item>
/// <item>the model "Original" colour at 100% removes every owned influence (restore).</item>
/// </list>
/// </para>
/// </summary>
public sealed class LedSurfacePolicy
{
    private bool _owned;
    private float _outR, _outG, _outB;

    /// <summary>True while this policy's own material clone is the source of the surface colour.</summary>
    public bool Owned => _owned;

    /// <summary>
    /// Late-update phase: classify the observed slot, gate on the lamp's switch state, and resolve
    /// the emission colour from a freshly sampled native baseline.
    /// </summary>
    /// <param name="lightOn">The lamp's live native switch state.</param>
    /// <param name="slot">What the adapter found in the one target material slot.</param>
    /// <param name="baseR">The native baseline emission red (never our own output).</param>
    /// <param name="baseG">The native baseline emission green.</param>
    /// <param name="baseB">The native baseline emission blue.</param>
    /// <param name="instruction">The resolved visual instruction from the pure state model.</param>
    public LedSurfaceStep Apply(bool lightOn, LedSlotState slot, float baseR, float baseG, float baseB, VisualInstruction instruction)
    {
        if (instruction is null) throw new ArgumentNullException(nameof(instruction));

        // Original at 100%: no owned material influence at all. Restore only what we installed.
        if (instruction.RemovesModInfluence)
        {
            bool restore = _owned || slot == LedSlotState.OwnClone;
            _owned = false;
            return restore ? new(LedSurfaceAction.Restore, 0, 0, 0, false) : new(LedSurfaceAction.None, 0, 0, 0, false);
        }

        // Our own clone in the slot while the lamp is off: hand the native material back.
        if (slot == LedSlotState.OwnClone && !lightOn)
        {
            _owned = false;
            return new(LedSurfaceAction.Restore, 0, 0, 0, false);
        }

        // Only an on lamp whose target slot is either the verified native panel or our own clone
        // may be recoloured. A foreign slot is never replaced; an empty slot is never created.
        if (!lightOn || (slot != LedSlotState.NativePanel && slot != LedSlotState.OwnClone))
        {
            _owned = false;
            return new(LedSurfaceAction.None, 0, 0, 0, false);
        }

        if (!IsValid(baseR, baseG, baseB))
        {
            _owned = false;
            return new(LedSurfaceAction.None, 0, 0, 0, false);
        }

        float factor = (float)instruction.IntensityFactor;
        float outR, outG, outB;
        if (instruction.ColorOverride is RgbColor color)
        {
            // Scale the chosen hue by the native emission magnitude (its brightest channel).
            float magnitude = Max(baseR, baseG, baseB) * factor;
            outR = (float)color.R * magnitude;
            outG = (float)color.G * magnitude;
            outB = (float)color.B * magnitude;
        }
        else
        {
            // Original: keep the native hue and scale brightness only.
            outR = baseR * factor;
            outG = baseG * factor;
            outB = baseB * factor;
        }

        if (!IsValid(outR, outG, outB))
        {
            _owned = false;
            return new(LedSurfaceAction.None, 0, 0, 0, false);
        }

        _owned = true;
        _outR = outR; _outG = outG; _outB = outB;
        return new(LedSurfaceAction.Install, outR, outG, outB, true);
    }

    /// <summary>
    /// Teardown / unload: restore the exact native material into the slot once and release ownership.
    /// Idempotent — a second call writes nothing.
    /// </summary>
    public LedSurfaceStep Restore()
    {
        if (!_owned) return new(LedSurfaceAction.None, 0, 0, 0, false);
        _owned = false;
        return new(LedSurfaceAction.Restore, 0, 0, 0, false);
    }

    /// <summary>The adapter's native material write threw: never claim ownership of a value we did not install.</summary>
    public void RejectWrite() => _owned = false;

    /// <summary>True only while the slot still holds exactly the emission floats this policy installed.</summary>
    public bool MatchesOwnedOutput(float r, float g, float b) =>
        _owned && IsValid(r, g, b) && r == _outR && g == _outG && b == _outB;

    /// <summary>Maps the adapter's raw observations to the slot classification this policy consumes.</summary>
    public static LedSlotState Classify(bool hasMaterial, bool isOwnClone, bool isSupportedPanel)
    {
        if (!hasMaterial) return LedSlotState.Empty;
        if (isOwnClone) return LedSlotState.OwnClone;
        return isSupportedPanel ? LedSlotState.NativePanel : LedSlotState.Foreign;
    }

    /// <summary>An emission sample is usable only when finite and nonnegative.</summary>
    public static bool IsValid(float r, float g, float b) =>
        float.IsFinite(r) && float.IsFinite(g) && float.IsFinite(b) && r >= 0 && g >= 0 && b >= 0;

    private static float Max(float a, float b, float c)
    {
        float value = a;
        if (b > value) value = b;
        if (c > value) value = c;
        return value;
    }
}
