using System;
using HomelessMod.Quests;
using MelonLoader;
using Il2CppScheduleOne;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Tiles;
using S1API.Console;
using S1API.Items;
using UnityEngine;

namespace HomelessMod.Building;

/// <summary>
/// Attached to any generic item placed on the street (e.g. pots, workstations, drying racks, tables)
/// enabling holding RMB (Right Mouse Button) or pressing [F] to dismantle and return the item to inventory.
/// </summary>
[RegisterTypeInIl2Cpp]
public class OutdoorItemInteractable : MonoBehaviour
{
    public OutdoorItemInteractable(IntPtr ptr) : base(ptr) { }

    private float _interactionRange = 3.0f;
    private bool _isHovered = false;
    private float _rmbHoldProgress = 0f;
    private string _itemId = "";
    private float _spawnCooldown = 0.6f;

    public string ItemId
    {
        get => _itemId;
        set => _itemId = value;
    }

    private void Start()
    {
        try
        {
            var footprintTiles = GetComponentsInChildren<FootprintTile>(true);
            foreach (var ft in footprintTiles)
            {
                if (ft != null && ft.Pointer != IntPtr.Zero)
                {
                    ft.gameObject.SetActive(false);
                }
            }

            var gridItems = GetComponentsInChildren<GridItem>(true);
            foreach (var gi in gridItems)
            {
                if (gi != null && gi.Pointer != IntPtr.Zero)
                {
                    try { gi.SetFootprintTileVisiblity(false); } catch { }
                    gi.enabled = false;
                }
            }

            var renderers = GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers)
            {
                if (r != null && r.Pointer != IntPtr.Zero)
                {
                    if (r.GetComponentInParent<FootprintTile>() != null ||
                        r.GetComponentInParent<TileAppearance>() != null)
                    {
                        r.enabled = false;
                        continue;
                    }
                    r.enabled = true;
                }
            }
            Mod.Log.Info($"[OutdoorItem] Start: '{_itemId}' on '{gameObject.name}' at {transform.position}");
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"OutdoorItemInteractable Start error: {ex.Message}");
        }
    }

    // Log-Spam vermeiden: OnDisable/OnDestroy feuern bei jedem erwarteten Pack-Up,
    // Scene-Unload oder SetActive(false). Warn+Stacktrace nur bei Debug-Builds sinnvoll.
    private void OnDisable()
    {
        try
        {
            Mod.Log.Debug($"[OutdoorItem] OnDisable: '{_itemId}' ({gameObject.name})");
        }
        catch { }
    }

    private void OnDestroy()
    {
        try
        {
            Mod.Log.Debug($"[OutdoorItem] OnDestroy: '{_itemId}' ({gameObject.name})");
        }
        catch { }
    }

    private void Update()
    {
        try
        {
            if (_spawnCooldown > 0f)
            {
                _spawnCooldown -= Time.deltaTime;
                _isHovered = false;
                _rmbHoldProgress = 0f;
                return;
            }

            // Strict input guard — block when cursor unlocked or typing (H6, schedule1-persistence §2)
            bool isTyping = false;
            try { isTyping = S1Mods.Shared.HotkeyManager.IsInputFieldFocused(); } catch { }
            if (isTyping || Cursor.lockState != CursorLockMode.Locked || this.WasCollected || gameObject.WasCollected)
            {
                _isHovered = false;
                _rmbHoldProgress = 0f;
                return;
            }

            var player = PlayerSingleton<PlayerMovement>.Instance;
            if (player == null || player.Pointer == IntPtr.Zero)
            {
                _isHovered = false;
                _rmbHoldProgress = 0f;
                return;
            }

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > _interactionRange)
            {
                _isHovered = false;
                _rmbHoldProgress = 0f;
                return;
            }

            // Resolve player camera
            Transform? camTransform = null;
            try
            {
                var playerCam = PlayerSingleton<PlayerCamera>.Instance;
                if (playerCam != null && playerCam.Pointer != IntPtr.Zero && playerCam.Camera != null)
                {
                    camTransform = playerCam.Camera.transform;
                }
            }
            catch { }

            if (camTransform == null && Camera.main != null)
            {
                camTransform = Camera.main.transform;
            }

            if (camTransform != null)
            {
                Vector3 toItem = (transform.position - camTransform.position).normalized;
                float dot = Vector3.Dot(camTransform.forward, toItem);

                bool hitMatch = false;
                Ray ray = new Ray(camTransform.position, camTransform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, _interactionRange + 1.0f))
                {
                    if (hit.collider != null && (hit.collider.gameObject == gameObject || hit.collider.transform.IsChildOf(transform)))
                    {
                        hitMatch = true;
                    }
                }

                _isHovered = hitMatch || (dot > 0.85f && distance <= 1.8f);
            }
            else
            {
                _isHovered = distance <= 1.8f;
            }

            if (_isHovered)
            {
                // Focus guard: don't allow RMB-Hold to dismantle while typing in phone/backpack or paused.
                if (HomelessInputFocus.IsBlockingInput)
                {
                    _rmbHoldProgress = 0f;
                    return;
                }

                // Hold Right Mouse Button (RMB) to Pack Up (Vanilla-style dismantle).
                // F-key path was removed in v0.1.2 — F is the vanilla flashlight and we must not steal it.
                if (Input.GetMouseButton(1))
                {
                    _rmbHoldProgress += Time.deltaTime / 0.40f; // 0.4s hold duration (vanilla-style)
                    if (_rmbHoldProgress >= 1.0f)
                    {
                        PackUp();
                        _rmbHoldProgress = 0f;
                    }
                }
                else
                {
                    _rmbHoldProgress = 0f;
                }
            }
            else
            {
                _rmbHoldProgress = 0f;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"OutdoorItemInteractable Update error: {ex.Message}");
        }
    }

    private static GUIStyle? _cachedLabelStyle;
    private static readonly Color BoxBgColor = new(0.08f, 0.09f, 0.11f, 0.92f);

    private static GUIStyle GetLabelStyle(int fontSize)
    {
        if (_cachedLabelStyle == null)
        {
            _cachedLabelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold
            };
            _cachedLabelStyle.normal.textColor = Color.white;
        }
        _cachedLabelStyle.fontSize = fontSize;
        return _cachedLabelStyle;
    }

    private void OnGUI()
    {
        if (!_isHovered) return;
        if (!Mod.CurrentConfig.ShowInteractionHud) return;

        // Compact, discreet interaction hint at bottom-center of screen
        float screenW = Screen.width;
        float screenH = Screen.height;
        float scale = Mathf.Clamp(screenH / 900f, 0.80f, 1.25f);
        float boxW = 190f * scale;
        float boxH = 26f * scale;
        int fontSize = Mathf.RoundToInt(12f * scale);
        Rect boxRect = new Rect((screenW - boxW) * 0.5f, screenH - boxH - (90f * scale), boxW, boxH);

        Color prevBg = GUI.backgroundColor;
        Color prevContent = GUI.contentColor;

        GUI.backgroundColor = BoxBgColor;
        GUI.Box(boxRect, "");

        var style = GetLabelStyle(fontSize);

        if (_rmbHoldProgress > 0.05f)
        {
            int filled = Mathf.RoundToInt(_rmbHoldProgress * 10);
            int empty = 10 - filled;
            string bar = new string('█', filled) + new string('░', Mathf.Max(0, empty));
            GUI.Label(boxRect, $"<color=#e67e22>[{bar}]</color> Packing Up...", style);
        }
        else
        {
            GUI.Label(boxRect, "<color=#b0b0b8>Hold RMB</color> <color=#e0e0e0>Pack Up</color>", style);
        }

        GUI.backgroundColor = prevBg;
        GUI.contentColor = prevContent;
    }

    public void PackUp()
    {
        if (string.IsNullOrEmpty(_itemId))
        {
            // Try to deduce from object name or BuildableItem if empty
            var bItem = GetComponent<BuildableItem>();
            if (bItem != null && bItem.ItemInstance != null && bItem.ItemInstance.Definition != null)
            {
                _itemId = bItem.ItemInstance.Definition.ID;
            }
        }

        if (string.IsNullOrEmpty(_itemId))
        {
            Mod.Log.Warn("Cannot pack up item with empty ItemId.");
            StreetPropertyManager.UnregisterStreetItem(gameObject);
            Destroy(gameObject);
            return;
        }

        Mod.Log.Info($"Packing up outdoor item '{_itemId}'...");

        try
        {
            var itemDef = Registry.GetItem(_itemId);
            var inv = PlayerInventory.Instance;
            if (itemDef != null && inv != null && inv.Pointer != IntPtr.Zero && !inv.WasCollected)
            {
                var inst = itemDef.GetDefaultInstance(1);
                if (inst != null && inst.Pointer != IntPtr.Zero && !inst.WasCollected)
                {
                    if (!inv.CanItemFitInInventory(inst, 1))
                    {
                        Mod.Log.Warn($"PackUp blocked — inventory full for '{_itemId}'. Free a slot first.");
                        return;
                    }
                    inv.AddItemToInventory(inst);
                    Mod.Log.Info($"Returned 1x '{_itemId}' to PlayerInventory.");
                }
                else
                {
                    Mod.Log.Warn($"Could not create item instance for '{_itemId}'.");
                    return;
                }
            }
            else
            {
                Mod.Log.Warn($"Cannot pack up '{_itemId}' — item definition or PlayerInventory unavailable.");
                return;
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Warn($"PackUp error: {ex.Message}");
            return;
        }

        StreetPropertyManager.UnregisterStreetItem(gameObject);
        Destroy(gameObject);
    }
}
