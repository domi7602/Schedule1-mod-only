using System;

namespace GoldenHour;

/// <summary>
/// Clock weight for the warm bias. The game sun color does not track time of day, so a
/// constant multiply paints noon gold. Peaks at 06:00 and 18:00 and falls to zero across
/// 90 minutes, so noon and night stay the game's own light. An out-of-range clock is not
/// a peak: callers must treat an unreadable time as weight 0 rather than full gold.
/// </summary>
public static class DaylightPolicy
{
    public const int MorningPeakMinutes = 6 * 60;
    public const int EveningPeakMinutes = 18 * 60;
    public const int HalfWidthMinutes = 90;
    public const int MinutesPerDay = 1440;

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
