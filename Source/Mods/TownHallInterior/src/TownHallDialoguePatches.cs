extern alias il2cpp;

using System;
using System.Reflection;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppScheduleOne.Dialogue;
using MelonLoader;
using S1API.Money;

namespace TownHallInterior;

/// <summary>
/// Injects Town Hall as a legitimate purchase option into Ray's DialogueHandler_EstateAgent,
/// matching vanilla real estate behavior (balance check, pricing, clean money purchase).
/// </summary>
internal static class TownHallDialoguePatches
{
    private const string HarmonyId = "TownHallInterior.RayDialogue";
    private static HarmonyLib.Harmony? _harmony;
    private static bool _installed;

    internal static void Install()
    {
        if (_installed) return;

        try
        {
            _harmony = new HarmonyLib.Harmony(HarmonyId);

            // 1. ModifyChoiceList
            var modifyListMethod = typeof(ControlledDialogueHandler).GetMethod(
                "ModifyChoiceList",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var modifyListPostfix = typeof(TownHallDialoguePatches).GetMethod(
                nameof(OnModifyChoiceListPostfix),
                BindingFlags.Static | BindingFlags.NonPublic);

            if (modifyListMethod != null && modifyListPostfix != null)
            {
                _harmony.Patch(modifyListMethod, postfix: new HarmonyMethod(modifyListPostfix));
            }

            // 2. ModifyChoiceText
            var modifyTextMethod = typeof(DialogueHandler_EstateAgent).GetMethod(
                "ModifyChoiceText",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var modifyTextPostfix = typeof(TownHallDialoguePatches).GetMethod(
                nameof(OnModifyChoiceTextPostfix),
                BindingFlags.Static | BindingFlags.NonPublic);

            if (modifyTextMethod != null && modifyTextPostfix != null)
            {
                _harmony.Patch(modifyTextMethod, postfix: new HarmonyMethod(modifyTextPostfix));
            }

            // 3. CheckChoice
            var checkChoiceMethod = typeof(DialogueHandler_EstateAgent).GetMethod(
                "CheckChoice",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var checkChoicePrefix = typeof(TownHallDialoguePatches).GetMethod(
                nameof(OnCheckChoicePrefix),
                BindingFlags.Static | BindingFlags.NonPublic);

            if (checkChoiceMethod != null && checkChoicePrefix != null)
            {
                _harmony.Patch(checkChoiceMethod, prefix: new HarmonyMethod(checkChoicePrefix));
            }

            // 4. ChoiceCallback
            var choiceCallbackMethod = typeof(DialogueHandler_EstateAgent).GetMethod(
                "ChoiceCallback",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var choiceCallbackPrefix = typeof(TownHallDialoguePatches).GetMethod(
                nameof(OnChoiceCallbackPrefix),
                BindingFlags.Static | BindingFlags.NonPublic);

            if (choiceCallbackMethod != null && choiceCallbackPrefix != null)
            {
                _harmony.Patch(choiceCallbackMethod, prefix: new HarmonyMethod(choiceCallbackPrefix));
            }

            _installed = true;
            MelonLogger.Msg("TownHallInterior: Ray estate agent dialogue patches successfully installed.");
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"TownHallInterior: Failed to install Ray dialogue patches: {ex.Message}");
        }
    }

    internal static void Uninstall()
    {
        if (!_installed) return;
        try
        {
            _harmony?.UnpatchSelf();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"TownHallInterior: Failed to uninstall Ray dialogue patches: {ex.Message}");
        }
        finally
        {
            _harmony = null;
            _installed = false;
        }
    }

    private static void OnModifyChoiceListPostfix(
        ControlledDialogueHandler __instance,
        string dialogueLabel,
        ref il2cpp::Il2CppSystem.Collections.Generic.List<DialogueChoiceData> existingChoices)
    {
        try
        {
            if (__instance == null || __instance.Pointer == IntPtr.Zero) return;
            if (existingChoices == null || existingChoices.Pointer == IntPtr.Zero || existingChoices.Count == 0) return;

            // Only hook Ray's estate agent handler
            if (!__instance.GetIl2CppType().Name.Contains("EstateAgent")) return;

            // Only offer Town Hall in the allowed world when not yet owned
            if (Mod.Instance?.IsBusinessAllowedInCurrentWorld != true) return;
            if (Mod.Instance?.IsTownHallOwned == true) return;

            // Check if this choice list is the business selection menu
            bool isBusinessMenu = false;
            bool alreadyInjected = false;

            for (int i = 0; i < existingChoices.Count; i++)
            {
                var choice = existingChoices[i];
                if (choice == null || choice.Pointer == IntPtr.Zero) continue;
                string label = choice.ChoiceLabel ?? string.Empty;
                string text = choice.ChoiceText ?? string.Empty;

                if (label.Equals("TownHall", StringComparison.OrdinalIgnoreCase))
                {
                    alreadyInjected = true;
                    break;
                }

                if (label.IndexOf("Laundromat", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    label.IndexOf("PostOffice", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    label.IndexOf("Post Office", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    label.IndexOf("CarWash", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    label.IndexOf("Medical", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    label.IndexOf("Taco", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    label.IndexOf("Business", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    text.IndexOf("Laundromat", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    text.IndexOf("Post Office", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    text.IndexOf("Car Wash", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    text.IndexOf("Medical", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    text.IndexOf("Taco", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    isBusinessMenu = true;
                }
            }

            if (isBusinessMenu && !alreadyInjected)
            {
                var townHallChoice = new DialogueChoiceData
                {
                    Guid = Guid.NewGuid().ToString(),
                    ChoiceLabel = "TownHall",
                    ChoiceText = "The Town Hall"
                };

                // Insert before the last option ("Nevermind"):
                if (existingChoices.Count > 0)
                {
                    existingChoices.Insert(existingChoices.Count - 1, townHallChoice);
                }
                else
                {
                    existingChoices.Add(townHallChoice);
                }
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"TownHallInterior ModifyChoiceList notice: {ex.Message}");
        }
    }

    private static void OnModifyChoiceTextPostfix(
        string choiceLabel,
        string choiceText,
        ref string __result)
    {
        try
        {
            if (choiceLabel != null && choiceLabel.Equals("TownHall", StringComparison.OrdinalIgnoreCase))
            {
                __result = "The Town Hall ($250,000)";
            }
        }
        catch { }
    }

    private static bool OnCheckChoicePrefix(
        string choiceLabel,
        out string invalidReason,
        ref bool __result)
    {
        if (choiceLabel != null && choiceLabel.Equals("TownHall", StringComparison.OrdinalIgnoreCase))
        {
            float online = 0f;
            float cash = 0f;
            try { online = Money.GetOnlineBalance(); } catch { }
            try { cash = Money.GetCashBalance(); } catch { }

            float price = BusinessPurchaseManager.DefaultTownHallPrice;
            if (online >= price || cash >= price)
            {
                invalidReason = string.Empty;
                __result = true;
            }
            else
            {
                invalidReason = "INSUFFICIENT BALANCE";
                __result = false;
            }
            return false; // Skip original estate agent check
        }

        invalidReason = string.Empty;
        return true;
    }

    private static bool OnChoiceCallbackPrefix(
        DialogueHandler_EstateAgent __instance,
        string choiceLabel)
    {
        try
        {
            if (choiceLabel != null && choiceLabel.Equals("TownHall", StringComparison.OrdinalIgnoreCase))
            {
                var receipt = Mod.Instance?.TryPurchaseTownHall(PurchasePaymentMode.Auto);
                if (receipt != null && receipt.Success)
                {
                    try
                    {
                        var notifMgr = Il2CppScheduleOne.UI.NotificationsManager.Instance;
                        if (notifMgr != null)
                        {
                            notifMgr.SendNotification(
                                "Rathaus erworben!",
                                $"Erfolgreich für ${receipt.Amount:N0} gekauft! Verwaltungsgebäude steht dir nun offen.",
                                TownHallIcon.Get(),
                                6f,
                                true);
                        }
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        var notifMgr = Il2CppScheduleOne.UI.NotificationsManager.Instance;
                        if (notifMgr != null)
                        {
                            notifMgr.SendNotification(
                                "Kauf fehlgeschlagen",
                                $"Rathaus konnte nicht gekauft werden ({receipt?.StatusMessage ?? "Unzureichende Deckung"}).",
                                TownHallIcon.Get(),
                                5f,
                                false);
                        }
                    }
                    catch { }
                }

                try
                {
                    __instance.EndDialogue();
                }
                catch { }

                return false; // Handled
            }
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"TownHallInterior ChoiceCallback notice: {ex.Message}");
        }

        return true;
    }
}
