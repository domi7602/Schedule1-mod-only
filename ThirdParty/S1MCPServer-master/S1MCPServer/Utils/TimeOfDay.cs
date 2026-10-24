#nullable enable
using System;
using System.Globalization;

namespace S1MCPServer.Utils;

/// <summary>
/// Conversions for the native clock. <c>ScheduleOne.GameTime.TimeManager.CurrentTime</c> is a
/// 24-hour <b>HHMM</b> integer (1954 = 19:54), <b>not</b> minutes since midnight. Evidence: it is
/// compared directly against contract/schedule windows that S1API documents as <c>hhmm</c>
/// (e.g. <c>ContractInfo.DeliveryWindowStart</c>, <c>CustomerDataBuilder.WithOrderTime</c>), the
/// native helpers are <c>IsValid24HourTime(int)</c>/<c>Normalized24HourTime(int)</c> and
/// <c>AddMinutesTo24HourTime(int, int)</c>, and a live read-only capture on 2026-10-23 returned
/// 1954 while the in-game HUD showed 19:54. <c>DailyMinSum</c> is the "minutes since midnight"
/// member; <c>CurrentTime</c> is the display-shaped one.
///
/// Everything here is pure so it can be unit-tested without game assemblies.
/// </summary>
public static class TimeOfDay
{
    public const int MinutesPerDay = 1440;

    /// <summary>True for 0..2359 where the minute part is 0..59.</summary>
    public static bool IsValidHhmm(int hhmm)
        => hhmm >= 0 && hhmm <= 2359 && hhmm % 100 < 60;

    public static bool TryHhmmToMinutes(int hhmm, out int minutes)
    {
        minutes = -1;
        if (!IsValidHhmm(hhmm))
            return false;

        minutes = (hhmm / 100) * 60 + (hhmm % 100);
        return true;
    }

    /// <summary>Normalises any minute count onto the 24-hour day and returns it as HHMM.</summary>
    public static int MinutesToHhmm(int minutes)
    {
        int wrapped = ((minutes % MinutesPerDay) + MinutesPerDay) % MinutesPerDay;
        return (wrapped / 60) * 100 + (wrapped % 60);
    }

    public static bool TryMinutesToHhmm(int minutes, out int hhmm)
    {
        hhmm = -1;
        if (minutes < 0 || minutes > MinutesPerDay - 1)
            return false;

        hhmm = (minutes / 60) * 100 + (minutes % 60);
        return true;
    }

    /// <summary>
    /// Converts decimal hours (0-24, the legacy <c>time</c> parameter) to HHMM. 24:00 is the end
    /// of the day and wraps to 00:00, which is what the native clock does.
    /// </summary>
    public static bool TryHoursToHhmm(double hours, out int hhmm)
    {
        hhmm = -1;
        if (double.IsNaN(hours) || double.IsInfinity(hours) || hours < 0 || hours > 24)
            return false;

        hhmm = MinutesToHhmm((int)Math.Round(hours * 60.0, MidpointRounding.AwayFromZero));
        return true;
    }

    /// <summary>
    /// Parses <c>"1954"</c>, <c>"19:54"</c> or <c>"9:54"</c> style text into a validated HHMM.
    /// </summary>
    public static bool TryParseHhmmText(string? text, out int hhmm)
    {
        hhmm = -1;
        if (string.IsNullOrWhiteSpace(text))
            return false;

        string digits = text.Trim().Replace(":", string.Empty).Replace(".", string.Empty);
        if (!int.TryParse(digits, NumberStyles.None, CultureInfo.InvariantCulture, out int parsed))
            return false;

        if (!IsValidHhmm(parsed))
            return false;

        hhmm = parsed;
        return true;
    }

    /// <summary>Formats a validated HHMM as <c>HH:MM</c>; anything else is returned unchanged.</summary>
    public static string Format(int hhmm)
        => IsValidHhmm(hhmm)
            ? string.Format(CultureInfo.InvariantCulture, "{0:00}:{1:00}", hhmm / 100, hhmm % 100)
            : hhmm.ToString(CultureInfo.InvariantCulture);
}
