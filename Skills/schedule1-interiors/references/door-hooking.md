# Door Hooking & Interior Transitions — Reference Guide

This reference details how to connect the exterior map of Hyland Point with custom interior spaces using Harmony patches on vanilla door classes.

---

## 1. The Vanilla Door System in Schedule I

Schedule I uses three primary classes for doors and building interactions:
- `StaticDoor`: Standard exterior building doors.
- `DoorKnocker`: Interactive doorbells/knockers that prompt the player to summon NPCs.
- `NpcSummonMenu`: The UI dialog that pops up when knocking or interacting with an entrance.

---

## 2. Harmony Hook Strategy

```csharp
[HarmonyPatch]
public static class DoorHookPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(StaticDoor), nameof(StaticDoor.Interacted))]
    public static void StaticDoor_Interacted_Postfix(StaticDoor __instance)
    {
        if (IsTargetBuildingDoor(__instance))
        {
            DoorTransitionService.Instance.OnDoorInteracted(__instance);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(NpcSummonMenu), nameof(NpcSummonMenu.Open))]
    public static void NpcSummonMenu_Open_Postfix(NpcSummonMenu __instance)
    {
        DoorTransitionService.Instance.InjectCustomMenuOption(__instance);
    }
}
```

---

## 3. Injecting Custom Options into NpcSummonMenu

When `NpcSummonMenu.Open` is called:
1. Find a reference button inside the menu hierarchy (e.g. `transform.Find("Container/ButtonTemplate")`).
2. Instantiate a clone:
   ```csharp
   GameObject customButton = UnityEngine.Object.Instantiate(templateButton, templateButton.transform.parent);
   customButton.SetActive(true);
   customButton.name = "Btn_EnterCustomInterior";
   
   var text = customButton.GetComponentInChildren<TextMeshProUGUI>();
   if (text != null) text.text = "Enter Building";
   
   var btn = customButton.GetComponent<Button>();
   btn.onClick.RemoveAllListeners();
   btn.onClick.AddListener((UnityAction)(() => {
       DoorTransitionService.Instance.TeleportToInterior();
   }));
   ```

---

## 4. Teleportation & Input Safety Checklist

| Step | Action | Why? |
|---|---|---|
| 1 | `_returnPos = player.transform.position;` | Saves exterior exit point |
| 2 | `CharacterController.enabled = false;` | Prevents Unity physics snap-back |
| 3 | `player.transform.position = interiorSpawnPos;` | Moves player to room |
| 4 | `CharacterController.enabled = true;` | Restores movement physics |
| 5 | `Cursor.lockState = CursorLockMode.Locked;` | Restores FPS mouse look |
| 6 | `Cursor.visible = false;` | Hides UI cursor |
| 7 | `NpcSummonMenu.Close();` | Cleans up UI |
