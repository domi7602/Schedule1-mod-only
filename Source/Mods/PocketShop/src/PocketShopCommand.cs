using System;
using System.Collections.Generic;
using System.Text;
using MelonLoader;
using S1API.Console;

namespace PocketShop.Services;

/// <summary>
/// v0.3.1 diagnostics: dumps every registered shop with its vanilla payment rule
/// (EPaymentType as assigned by TVGS in the Unity scene data), so the cash-vs-card
/// mapping can be verified in-game against any external source.
/// Usage: pshop shops | pshop help
/// </summary>
public sealed class PocketShopCommand : BaseConsoleCommand
{
    public override string CommandWord => "pshop";
    public override string CommandDescription => "PocketShop: shops (payment rules), help";
    public override string ExampleUsage => "pshop shops";

    public override void ExecuteCommand(List<string> args)
    {
        try
        {
            string sub = (args != null && args.Count > 0) ? args[0].ToLowerInvariant() : "shops";
            if (sub == "shops" || sub == "s")
            {
                DumpShops();
                return;
            }
            MelonLogger.Msg("Usage: pshop shops");
        }
        catch (Exception ex)
        {
            MelonLogger.Msg($"<color=#ff6060>Error executing pshop command: {ex.Message}</color>");
            MelonLogger.Error($"pshop command failed: {ex}");
        }
    }

    private static void DumpShops()
    {
        var shops = ShopCatalog.Shops;
        var sb = new StringBuilder();
        sb.AppendLine("<color=#60f080>=== PocketShop: registered shops & payment rules ===</color>");

        if (shops.Count == 0)
        {
            sb.AppendLine("  (no shops registered — load a save first)");
            MelonLogger.Msg(sb.ToString());
            return;
        }

        foreach (var shop in shops)
        {
            string rule = shop.PaymentType switch
            {
                global::Il2CppScheduleOne.UI.Shop.ShopInterface.EPaymentType.Cash => "<color=#60f080>CASH</color>",
                global::Il2CppScheduleOne.UI.Shop.ShopInterface.EPaymentType.Online => "<color=#60c8f0>CARD</color>",
                global::Il2CppScheduleOne.UI.Shop.ShopInterface.EPaymentType.PreferCash => "<color=#e6c84f>CARD+CASH (pref. CASH)</color>",
                global::Il2CppScheduleOne.UI.Shop.ShopInterface.EPaymentType.PreferOnline => "<color=#e6c84f>CARD+CASH (pref. CARD)</color>",
                _ => shop.PaymentType.ToString()
            };
            sb.AppendLine($"  {shop.Name} <color=#888888>[{shop.ShopCode}]</color> — {rule} <color=#888888>({shop.ItemCount} items)</color>");
        }
        MelonLogger.Msg(sb.ToString());
    }
}
