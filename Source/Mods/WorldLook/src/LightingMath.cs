namespace GoldenHour
{
    // ==== WorldLook section: Argb.cs ====

    /// <summary>Unity-free linear RGBA value (0..1 typical, HDR channels allowed &gt; 1).</summary>
    public readonly record struct Argb(float R, float G, float B, float A)
    {
        /// <summary>Comparison tolerance for display/tests, never native write ownership.</summary>
        public const float DefaultTolerance = 0.0005f;

        public static Argb White { get; } = new(1f, 1f, 1f, 1f);

        public bool IsFinite =>
            float.IsFinite(R) && float.IsFinite(G) && float.IsFinite(B) && float.IsFinite(A);

        /// <summary>
        /// Per-channel closeness test for display/tests (tolerance 0.0005 by default).
        /// Ownership uses exact native readback equality instead. Non-finite on either
        /// side is never "close" — a NaN must not be treated as unchanged.
        /// </summary>
        public bool IsCloseTo(Argb other, float tolerance = DefaultTolerance) =>
            IsFinite && other.IsFinite &&
            System.Math.Abs(R - other.R) < tolerance &&
            System.Math.Abs(G - other.G) < tolerance &&
            System.Math.Abs(B - other.B) < tolerance &&
            System.Math.Abs(A - other.A) < tolerance;
    }
    // ==== end WorldLook section: Argb.cs ====
}

namespace GoldenHour
{
    // ==== WorldLook section: BlendPolicy.cs ====

    /// <summary>
    /// Pure blend math for the golden-hour bias: each channel multiplies the original by a
    /// warm tint that fades in with strength (0 = original, 1 = full bias). Non-finite
    /// original channels fall back to the tint value; non-finite tint channels act neutral.
    /// </summary>
    public static class BlendPolicy
    {
        public static double ClampStrength(double strength) =>
            double.IsFinite(strength) ? System.Math.Clamp(strength, 0.0, 1.0) : 0.0;

        public static Argb Blend(Argb original, Argb tint, double strength)
        {
            double s = ClampStrength(strength);

            return new Argb(
                Channel(original.R, tint.R, s),
                Channel(original.G, tint.G, s),
                Channel(original.B, tint.B, s),
                float.IsFinite(original.A) ? original.A : 1f);
        }

        private static float Channel(float original, float tint, double s)
        {
            if (!float.IsFinite(original)) return float.IsFinite(tint) ? tint : 0f;
            if (!float.IsFinite(tint)) return original;
            double mixed = 1.0 + s * ((double)tint - 1.0);
            double scaled = (double)original * mixed;
            if (!double.IsFinite(scaled) || scaled > float.MaxValue || scaled < -float.MaxValue) return original;
            return (float)scaled;
        }
    }
    // ==== end WorldLook section: BlendPolicy.cs ====
}

namespace GoldenHour
{
    // ==== WorldLook section: DaylightPolicy.cs ====
    using System;


    /// <summary>
    /// Clock weight for the warm bias. Peaks at 06:00 and 18:00 and falls to zero across
    /// 90 minutes, so noon and night stay the game's own light. An out-of-range clock is not
    /// a peak: callers must treat an unreadable time as weight 0 rather than full gold.
    /// </summary>
    public static class DaylightPolicy
    {
        public const int MorningPeakMinutes = 6 * 60;
        public const int EveningPeakMinutes = 18 * 60;
        public const int HalfWidthMinutes = 90;
        public const int MinutesPerDay = 1440;

        /// <summary>TimeManager.CurrentTime is HHMM (1954 = 19:54), not elapsed minutes.</summary>
        public static bool TryHhmmToMinutes(int hhmm, out int minutes)
        {
            minutes = -1;
            if (hhmm < 0 || hhmm > 2359 || hhmm % 100 >= 60) return false;
            minutes = hhmm / 100 * 60 + hhmm % 100;
            return true;
        }

        public static double Weight(int minutesSinceMidnight)
        {
            if (minutesSinceMidnight < 0 || minutesSinceMidnight >= MinutesPerDay) return 0.0;

            double morning = Lobe(CircularDistance(minutesSinceMidnight, MorningPeakMinutes));
            double evening = Lobe(CircularDistance(minutesSinceMidnight, EveningPeakMinutes));
            double weight = morning > evening ? morning : evening;
            if (!double.IsFinite(weight) || weight < 0.0) return 0.0;
            if (weight > 1.0) return 1.0;
            return weight;
        }

        public static int CircularDistance(int minutes, int peak)
        {
            int d = Math.Abs(minutes - peak);
            int wrap = MinutesPerDay - d;
            return d < wrap ? d : wrap;
        }

        private static double Lobe(int distanceMinutes)
        {
            if (distanceMinutes >= HalfWidthMinutes) return 0.0;
            double t = distanceMinutes / (double)HalfWidthMinutes;
            return 0.5 * (1.0 + Math.Cos(Math.PI * t));
        }
    }
    // ==== end WorldLook section: DaylightPolicy.cs ====
}
