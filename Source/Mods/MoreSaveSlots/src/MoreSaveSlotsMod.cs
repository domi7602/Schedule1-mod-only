using System;
using Il2CppScheduleOne.Persistence;
using Il2CppTMPro;
using MelonLoader;
using MoreSaveSlots.Config;
using MoreSaveSlots.UI;
using UnityEngine;
using UnityEngine.EventSystems;

[assembly: MelonInfo(typeof(MoreSaveSlots.MoreSaveSlotsMod), "MoreSaveSlots", "1.0.2", "Dominik")]
[assembly: MelonGame("TVGS", "Schedule I")]
[assembly: MelonGame("TVGS", "Schedule 1")]

namespace MoreSaveSlots;

public class MoreSaveSlotsMod : MelonMod
{
    public static MoreSaveSlotsMod Instance { get; private set; } = null!;
    public static MoreSaveSlotsConfig Config { get; private set; } = null!;

    public override void OnInitializeMelon()
    {
        Instance = this;

        // Load configuration
        Config = MoreSaveSlotsConfig.Load();
        PaginationController.TotalSlots = Config.TotalSlots;

        // Set game slot count
        try
        {
            SaveManager.SAVE_SLOT_COUNT = Config.TotalSlots;
        }
        catch
        {
            // Will be set by Harmony patch once SaveManager class initializes
        }

        MelonLogger.Msg($"Initialized (v1.0.2) with {Config.TotalSlots} slots ({PaginationController.TotalPages} pages).");
    }

    public override void OnUpdate()
    {
        if (RenameDialog.IsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                RenameDialog.Submit();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                RenameDialog.Close();
            }
            return;
        }

        if (DeleteDialog.IsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                DeleteDialog.Confirm();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeleteDialog.Close();
            }
            return;
        }

        if (!Config.EnableKeyboardNavigation) return;
        if (IsUserTypingInInputField()) return;
        // Nur im MainMenu (Save-Screens) navigieren — im Gameplay-Scene ("Main")
        // niemals Tastatur-Shortcuts hijacken (Arrow/Q/E/F2/R sind dort belegt).
        if (IsInGameplayScene()) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.PageUp) || Input.GetKeyDown(KeyCode.Q))
        {
            PaginationController.PreviousPage();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.E))
        {
            PaginationController.NextPage();
        }

        if (Config.EnableRenameFeature && (Input.GetKeyDown(KeyCode.F2) || Input.GetKeyDown(KeyCode.R)))
        {
            RenameDialog.OpenForHoveredOrSelected();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteDialog.OpenForHoveredOrSelected();
        }
    }

    private static bool IsInGameplayScene()
    {
        try
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            // Workspace-Konvention: Gameplay-Scene heißt exakt "Main".
            return scene.name == "Main";
        }
        catch
        {
            return false;
        }
    }

    public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
    {
        RenameDialog.ResetForSceneReload();
        DeleteDialog.ResetForSceneReload();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        try
        {
            if (RenameDialog.IsOpen)
                RenameDialog.Close();
            if (DeleteDialog.IsOpen)
                DeleteDialog.Close();
        }
        catch { }
    }

    private static bool IsUserTypingInInputField()
    {
        try
        {
            var es = EventSystem.current;
            if (es != null && es.currentSelectedGameObject != null)
            {
                var tmpInput = es.currentSelectedGameObject.GetComponent<TMP_InputField>();
                if (tmpInput != null && tmpInput.isFocused) return true;

                var legacyInput = es.currentSelectedGameObject.GetComponent<UnityEngine.UI.InputField>();
                if (legacyInput != null && legacyInput.isFocused) return true;
            }
        }
        catch
        {
            // Ignore if EventSystem is not ready
        }
        return false;
    }
}
