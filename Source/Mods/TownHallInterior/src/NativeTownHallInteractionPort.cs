using System;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI;
using MelonLoader;
using UnityEngine;

namespace TownHallInterior;

/// <summary>
/// Engine adapter for player spatial position, E keypress detection, and HUD notifications.
/// </summary>
public sealed class NativeTownHallInteractionPort : ITownHallPlayerPort
{
    public bool TryGetPlayerPosition(out float x, out float y, out float z)
    {
        x = 0f;
        y = 0f;
        z = 0f;
        try
        {
            var player = Player.Local;
            if (player == null || player.Pointer == IntPtr.Zero)
                return false;

            var pos = player.transform.position;
            x = pos.x;
            y = pos.y;
            z = pos.z;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsInteractKeyPressed()
    {
        try
        {
            return Input.GetKeyDown(KeyCode.E);
        }
        catch
        {
            return false;
        }
    }

    public void ShowNotification(string title, string message, bool playSound = true)
    {
        try
        {
            var notifMgr = NotificationsManager.Instance;
            if (notifMgr != null && (UnityEngine.Object)notifMgr != null)
            {
                notifMgr.SendNotification(title, message, TownHallIcon.Get(), 5f, playSound);
                return;
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"TownHallInterior notification error: {ex.Message}");
        }

        MelonLogger.Msg($"[TownHall HUD] {title}: {message}");
    }
}
