using System;
namespace TaxiDriver;

internal readonly record struct HeroText(string Title, string Meta, string Value, bool IsFare);

/// <summary>All displayed hero state comes from the same change-guarded snapshot.</summary>
internal static class HeroView
{
    internal static HeroText Build(HeroSnapshot s)
    {
        string title;
        string meta;
        string value = string.Empty;
        bool isFare = false;

        if (s.RideActive)
        {
            if (s.AwaitingDestination)
            {
                title = "ON BOARD";
                meta = "Pick a destination below";
            }
            else if (s.Arrived)
            {
                title = s.GaveUp ? "COULDN'T GET THERE" : "ARRIVED";
                meta = s.GaveUp
                    ? "Press E to get out — or STOP to despawn"
                    : "Press E to exit the taxi";
                value = $"FARE ${s.Fare}";
                isFare = true;
            }
            else
            {
                // Package 7: while driving, the hero names the ACTIVE trip —
                // the picker may have been cleared or re-picked since dispatch.
                string trip = s.HasActiveTrip && s.ActiveTripName.Length > 0
                    ? s.ActiveTripName
                    : s.Destination;
                bool showing = s.Picked || trip != s.Destination;
                title = showing ? $"RIDING TO {trip.ToUpperInvariant()}" : "RIDING";
                string dropOff = ShortDropOff(s.DropOff);
                meta = showing
                    ? (string.IsNullOrEmpty(dropOff) ? "On the way" : $"Drop-off: {dropOff}")
                    : "On the way";
                value = $"FARE ${s.Fare}";
                isFare = true;
            }
        }
        else if (s.AwaitingBoard)
        {
            title = "BOARD NOW";
            meta = "Press E to get in";
            value = s.Picked ? $"TO {s.Destination}" : "Or pick a destination below";
        }
        else if (s.ToPlayer || s.ToPlayerPolling)
        {
            title = "TAXI ON THE WAY";
            meta = "Driving to your position — wait outside";
            if (s.Picked)
                value = $"NEXT: {s.Destination}";
        }
        else if (s.AutoRunning || s.PendingSpawn)
        {
            title = "SPAWNING…";
            meta = "The taxi is being dispatched";
        }
        else if (s.Vehicle)
        {
            title = "TAXI WAITING";
            meta = "Press STOP to despawn it";
            value = s.Picked ? $"TO {s.Destination}" : "No destination picked";
        }
        else
        {
            title = "CALL A TAXI";
            meta = "Orders a cab to your position";
            value = s.Picked ? $"TO {s.Destination}" : "No destination picked";
        }

        if (s.Override.Length > 0)
            meta = s.Override;

        return new(title, meta, value, isFare);
    }

    private static string ShortDropOff(string? dropOff)
    {
        if (string.IsNullOrEmpty(dropOff))
            return string.Empty;
        int paren = dropOff.IndexOf('(');
        return paren > 0 ? dropOff.Substring(0, paren).Trim() : dropOff;
    }

}
