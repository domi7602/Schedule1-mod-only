using System;
using CustomSkateboard.Config;
using CustomSkateboard.Items;
using Il2CppScheduleOne.Dialogue;
using Il2CppScheduleOne.ItemFramework;

namespace CustomSkateboard.Dialogue;

/// <summary>
/// Injects the custom skateboard into Jeff Gilmore's DialogueController_SkateboardSeller options.
/// </summary>
public static class SkateboardSellerInjector
{
    public static void TryInject(DialogueController_SkateboardSeller seller, SkateboardConfig config)
    {
        try
        {
            if (seller == null || seller.Pointer == IntPtr.Zero)
            {
                return;
            }

            ItemDefinition? itemDef = SkateboardItemFactory.CustomSkateboardItem;
            if (itemDef == null || itemDef.Pointer == IntPtr.Zero)
            {
                return;
            }

            var options = seller.Options;
            if (options == null || options.Pointer == IntPtr.Zero)
            {
                return;
            }

            // Check if already injected
            for (int i = 0; i < options.Count; i++)
            {
                var opt = options[i];
                if (opt != null && opt.Pointer != IntPtr.Zero && opt.Item != null && opt.Item.Pointer != IntPtr.Zero && opt.Item.ID.Equals(config.SkateboardId, StringComparison.OrdinalIgnoreCase))
                {
                    return; // Already present
                }
            }

            DialogueController_SkateboardSeller.Option newOption = new DialogueController_SkateboardSeller.Option
            {
                Name = config.SkateboardName,
                Price = config.Price,
                IsAvailable = true,
                NotAvailableReason = string.Empty,
                Item = itemDef
            };

            options.Add(newOption);
            Mod.Log.Info($"Successfully injected '{config.SkateboardName}' (${config.Price}) into Jeff Gilmore's Skateboard Shop dialogue!");
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"Dialogue injection notice: {ex.Message}");
        }
    }

    /// <summary>
    /// Safely hook when dialogue choices are requested by the player in-game.
    /// </summary>
    public static void OnModifyChoiceListPrefix(DialogueController_SkateboardSeller __instance)
    {
        try
        {
            var cfg = Mod.CurrentConfig;
            if (cfg != null && cfg.AutoInjectToJeffGilmore)
            {
                TryInject(__instance, cfg);
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"ModifyChoiceList hook notice: {ex.Message}");
        }
    }
}
