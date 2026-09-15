using System;
using HarmonyLib;

namespace BackpackMod.Patches
{
    /// <summary>
    /// Guards vanilla ClothingItemUI.UpdateUI against NRE when a custom clothing item
    /// (backpack) is added via AddItemToInventory. Vanilla UpdateUI dereferences
    /// clothing visuals that may be null on cloned definitions — swallow NRE so the
    /// item still lands in inventory and the game stays alive. Logged throttled
    /// for diagnostics; real fix is in BackpackDefinitions to ensure visuals are set.
    /// Gatekeeper-hotfix 2026-08-30 for console `additem backpack_t1` crash.
    /// Fix 2026-09-11: the NRE crosses the IL2CPP boundary wrapped in
    /// Il2CppInterop.Runtime.Il2CppException, so a plain `is NullReferenceException`
    /// check never matched and the/Log spam continued — unwrap the chain instead.
    /// </summary>
    public static class ClothingItemUIPatch
    {
        private static int _swallowedCount;
        private static int _lastWarnTick;

        public static Exception? Finalizer(Exception? __exception)
        {
            if (IsNre(__exception))
            {
                _swallowedCount++;
                int now = Environment.TickCount;
                if (now - _lastWarnTick > 30000 || _lastWarnTick == 0)
                {
                    _lastWarnTick = now;
                    try { MelonLoader.MelonLogger.Warning($"[BackpackMod][ClothingItemUI] UpdateUI NRE swallowed (x{_swallowedCount} total, backpack item UI)."); } catch { }
                }
                return null; // swallow — keep game alive, item stays in inventory
            }
            return __exception;
        }

        private static bool IsNre(Exception? ex)
        {
            if (ex == null) return false;
            int depth = 0;
            for (Exception? e = ex; e != null && depth < 6; e = e.InnerException, depth++)
            {
                if (e is NullReferenceException) return true;
            }
            // Il2CppException embeds the inner type name in Message when InnerException
            // is null — but only trust the text fallback for that wrapper type, never
            // for arbitrary exceptions (a vanilla UpdateUI bug mentioning "NullReference"
            // in prose must NOT be swallowed silently).
            try
            {
                string? typeName = ex.GetType().FullName;
                if (typeName == null || typeName.IndexOf("Il2CppException", StringComparison.Ordinal) < 0)
                    return false;
                return ex.Message != null && ex.Message.Contains("NullReferenceException");
            }
            catch { return false; }
        }
    }
}
