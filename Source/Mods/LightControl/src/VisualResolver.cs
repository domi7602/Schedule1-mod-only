using System.Text;

namespace LightControl.Core;

/// <summary>Normalized authored RGB. The adapter must confirm its target color-space convention.</summary>
public sealed record RgbColor
{
    public double R { get; }
    public double G { get; }
    public double B { get; }

    public RgbColor(double r, double g, double b)
    {
        if (!double.IsFinite(r) || !double.IsFinite(g) || !double.IsFinite(b) ||
            r < 0 || r > 1 || g < 0 || g > 1 || b < 0 || b > 1)
            throw new ArgumentOutOfRangeException(nameof(r), "RGB channels must be finite and between zero and one.");
        R = r; G = g; B = b;
    }
}

/// <summary>
/// ColorOverride == null means preserve the current unmodified game color. A factor of
/// one removes intensity influence. No instruction enables a lamp or changes gameplay.
/// </summary>
public sealed record VisualInstruction
{
    public RgbColor? ColorOverride { get; }
    public double IntensityFactor { get; }
    public bool HasTemporaryEffect { get; }

    public VisualInstruction(RgbColor? colorOverride, double intensityFactor, bool hasTemporaryEffect)
    {
        if (!double.IsFinite(intensityFactor) ||
            !(intensityFactor == 0.1 || intensityFactor == 0.25 || intensityFactor == 0.5 || intensityFactor == 0.75 ||
              intensityFactor == 1 || intensityFactor == 1.5 || intensityFactor == 2))
            throw new ArgumentOutOfRangeException(nameof(intensityFactor));
        ColorOverride = colorOverride;
        IntensityFactor = intensityFactor;
        HasTemporaryEffect = hasTemporaryEffect;
    }

    public bool RemovesModInfluence => ColorOverride is null && IntensityFactor == 1;

    /// <summary>Always pass a freshly sampled, unmodified native value, never previous output.</summary>
    public double ScaleCurrentNativeIntensity(double unmodifiedNativeIntensity)
    {
        if (!double.IsFinite(unmodifiedNativeIntensity) || unmodifiedNativeIntensity < 0)
            throw new ArgumentOutOfRangeException(nameof(unmodifiedNativeIntensity));
        var result = unmodifiedNativeIntensity * IntensityFactor;
        if (!double.IsFinite(result)) throw new ArgumentOutOfRangeException(nameof(unmodifiedNativeIntensity), "Scaled intensity overflowed.");
        return result;
    }
}

public static class VisualResolver
{
    public static VisualInstruction Resolve(string logicalLampKey, string? propertyId, LampBaseState state,
        EffectState effects, double synchronizedTimeSeconds, bool modEnabled = true,
        double rainbowPeriodSeconds = LightLimits.RainbowPeriodSeconds)
    {
        if (string.IsNullOrEmpty(logicalLampKey) || logicalLampKey.Length > 4096)
            throw new ArgumentException("A bounded logical lamp identity is required.", nameof(logicalLampKey));
        LightLimits.OptionalIdentifier(propertyId, nameof(propertyId));
        if (state is null) throw new ArgumentNullException(nameof(state));
        if (effects is null) throw new ArgumentNullException(nameof(effects));
        LightLimits.Time(synchronizedTimeSeconds, nameof(synchronizedTimeSeconds));
        if (!double.IsFinite(rainbowPeriodSeconds) || rainbowPeriodSeconds < LightLimits.MinRainbowPeriodSeconds ||
            rainbowPeriodSeconds > LightLimits.MaxRainbowPeriodSeconds)
            throw new ArgumentOutOfRangeException(nameof(rainbowPeriodSeconds));
        if (!modEnabled) return new(null, 1, false);
        var effect = effects.Resolve(propertyId);
        if (effect is not null)
        {
            var elapsed = Math.Max(0, synchronizedTimeSeconds - effect.StartTimeSeconds);
            var step = (ulong)Math.Floor(elapsed / effect.StepSeconds);
            ulong hash = Mix(StableHash(logicalLampKey) ^ effect.Seed ^ Mix(step));
            return new(FromHue((hash >> 11) * (1.0 / 9007199254740992.0)), state.IntensityFactor, true);
        }
        RgbColor? color = state.Color switch
        {
            ColorMode.Original => null,
            ColorMode.White => new(1, 1, 1),
            ColorMode.Red => new(1, 0, 0),
            ColorMode.Orange => new(1, 0.35, 0),
            ColorMode.Yellow => new(1, 1, 0),
            ColorMode.Green => new(0, 1, 0),
            ColorMode.Cyan => new(0, 1, 1),
            ColorMode.Blue => new(0, 0, 1),
            ColorMode.Purple => new(0.55, 0, 1),
            ColorMode.Pink => new(1, 0.15, 0.55),
            ColorMode.Rainbow => FromHue(synchronizedTimeSeconds / rainbowPeriodSeconds),
            _ => throw new ArgumentOutOfRangeException(nameof(state))
        };
        return new(color, state.IntensityFactor, false);
    }

    /// <summary>Specified UTF-8 FNV-1a, independent of process-randomized string hash codes.</summary>
    public static ulong StableHash(string value)
    {
        if (value is null || value.Length > 4096) throw new ArgumentException("A bounded value is required.", nameof(value));
        ulong hash = 14695981039346656037UL;
        Span<byte> utf8 = stackalloc byte[4];
        foreach (var rune in value.EnumerateRunes())
        {
            int byteCount = rune.EncodeToUtf8(utf8);
            for (int index = 0; index < byteCount; index++) hash = unchecked((hash ^ utf8[index]) * 1099511628211UL);
        }
        return hash;
    }

    private static ulong Mix(ulong value)
    {
        value = unchecked((value ^ (value >> 30)) * 0xbf58476d1ce4e5b9UL);
        value = unchecked((value ^ (value >> 27)) * 0x94d049bb133111ebUL);
        return value ^ (value >> 31);
    }

    private static RgbColor FromHue(double hue)
    {
        hue -= Math.Floor(hue);
        double scaled = hue * 6;
        int sector = (int)Math.Floor(scaled);
        double fraction = scaled - sector;
        return sector switch
        {
            0 => new(1, fraction, 0),
            1 => new(1 - fraction, 1, 0),
            2 => new(0, 1, fraction),
            3 => new(0, 1 - fraction, 1),
            4 => new(fraction, 0, 1),
            _ => new(1, 0, 1 - fraction)
        };
    }
}
