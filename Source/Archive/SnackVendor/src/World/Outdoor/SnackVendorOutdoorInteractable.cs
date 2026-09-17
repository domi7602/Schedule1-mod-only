using System;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Attributes;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.EntityFramework;
using Il2CppScheduleOne.ItemFramework;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.Tiles;
using MelonLoader;
using S1Mods.Shared;
using UnityEngine;

namespace SnackVendor.World.Outdoor;

/// <summary>
/// Component attached to outdoor-placed SnackVendor stations.
/// Manages footprint/griditem suppression, hover detection, and RMB-hold pack-up.
/// </summary>
[RegisterTypeInIl2Cpp]
public class SnackVendorOutdoorInteractable : MonoBehaviour
{
    public SnackVendorOutdoorInteractable(IntPtr ptr) : base(ptr) { }

    private float _interactionRange = 3.0f;
    private bool _isHovered = false;
    private float _rmbHoldProgress = 0f;
    private float _nextHoverCheckTime = 0f;
    private const float HoverCheckInterval = 0.15f;

    [HideFromIl2Cpp]
    public string Guid { get; set; } = string.Empty;

    // IMGUI cached styles
    private static GUIStyle? _promptStyle;
    private static GUIStyle? _barBgStyle;
    private static GUIStyle? _barFillStyle;

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

            var buildables = GetComponentsInChildren<BuildableItem>(true);
            foreach (var b in buildables)
            {
                if (b != null && b.Pointer != IntPtr.Zero)
                {
                    b.enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            Mod.Log.Debug($"SnackVendorOutdoorInteractable.Start error: {ex.Message}");
        }
    }

    private void Update()
    {
        try
        {
            bool doHoverCheck = Time.time >= _nextHoverCheckTime;
            if (doHoverCheck)
            {
                _nextHoverCheckTime = Time.time + HoverCheckInterval;
            }

            if (Cursor.lockState != CursorLockMode.Locked || SnackVendorPanel.IsOpen)
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

            if (doHoverCheck)
            {
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
            }

            if (_isHovered)
            {
                // If RMB held, accumulate pack-up progress
                if (Input.GetMouseButton(1))
                {
                    _rmbHoldProgress += Time.deltaTime / 0.40f;
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
            Mod.Log.Debug($"SnackVendorOutdoorInteractable.Update error: {ex.Message}");
        }
    }

    private void OnGUI()
    {
        if (!_isHovered || SnackVendorPanel.IsOpen) return;

        try
        {
            EnsureStyles();

            float screenW = Screen.width;
            float screenH = Screen.height;
            float scale = Mathf.Clamp(screenH / 900f, 0.80f, 1.25f);

            float boxW = 220f * scale;
            float boxH = _rmbHoldProgress > 0.01f ? 48f * scale : 26f * scale;
            float boxX = (screenW - boxW) * 0.5f;
            float boxY = screenH - boxH - (90f * scale);

            GUI.Box(new Rect(boxX, boxY, boxW, boxH), GUIContent.none, _barBgStyle);

            string text = _rmbHoldProgress > 0.01f
                ? $"Packing Up... ({Mathf.RoundToInt(_rmbHoldProgress * 100f)}%)"
                : "<color=#b0b0b8>Hold RMB</color> <color=#e0e0e0>Pack Up</color>";

            GUI.Label(new Rect(boxX, boxY + 2f * scale, boxW, 22f * scale), text, _promptStyle);

            if (_rmbHoldProgress > 0.01f && _barFillStyle != null)
            {
                float barPad = 8f * scale;
                float barW = boxW - (barPad * 2f);
                float barH = 6f * scale;
                float fillW = barW * Mathf.Clamp01(_rmbHoldProgress);
                GUI.Box(new Rect(boxX + barPad, boxY + 26f * scale, fillW, barH), GUIContent.none, _barFillStyle);
            }
        }
        catch { }
    }

    [HideFromIl2Cpp]
    public void PackUp()
    {
        try
        {
            var ctrl = GetComponent<SnackVendorController>();
            if (ctrl != null)
            {
                ctrl.RefundStockAndDismantle();
                return;
            }

            var playerInv = PlayerSingleton<PlayerInventory>.Instance;
            if (playerInv != null && playerInv.Pointer != IntPtr.Zero)
            {
                // 1. Refund any stocked items inside this station
                if (ctrl != null)
                {
                    var stock = ctrl.Stock;
                    if (stock != null)
                    {
                        for (int i = 0; i < stock.Count; i++)
                        {
                            var slot = stock[i];
                            if (slot != null && slot.Quantity > 0 && !string.IsNullOrEmpty(slot.IngredientId))
                            {
                                var itemDef = Il2CppScheduleOne.Registry.GetItem(slot.IngredientId);
                                if (itemDef != null && itemDef.Pointer != IntPtr.Zero)
                                {
                                    var inst = itemDef.GetDefaultInstance(slot.Quantity);
                                    if (inst != null && inst.Pointer != IntPtr.Zero)
                                    {
                                        playerInv.AddItemToInventory(inst);
                                    }
                                }
                            }
                        }
                    }
                }

                // 2. Refund 1x SnackVendor item itself
                var vendorDef = Il2CppScheduleOne.Registry.GetItem(Mod.CurrentConfig.StationItemId);
                if (vendorDef != null && vendorDef.Pointer != IntPtr.Zero)
                {
                    var inst = vendorDef.GetDefaultInstance(1);
                    if (inst != null && inst.Pointer != IntPtr.Zero)
                    {
                        playerInv.AddItemToInventory(inst);
                    }
                }
            }

            // 3. Unregister from outdoor manager
            SnackVendorOutdoorManager.UnregisterOutdoorStation(gameObject);
            Mod.Log.Info($"Packed up outdoor SnackVendor station ({gameObject.name}).");

            // 4. Destroy GameObject
            Destroy(gameObject);
        }
        catch (Exception ex)
        {
            Mod.Log.Error("PackUp failed", ex);
        }
    }

    private static void EnsureStyles()
    {
        if (_promptStyle != null) return;

        _promptStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };
        _promptStyle.normal.textColor = Color.white;

        var bgTex = new Texture2D(1, 1);
        bgTex.SetPixel(0, 0, new Color(0.08f, 0.08f, 0.10f, 0.85f));
        bgTex.Apply();

        _barBgStyle = new GUIStyle(GUI.skin.box);
        _barBgStyle.normal.background = bgTex;

        var fillTex = new Texture2D(1, 1);
        fillTex.SetPixel(0, 0, new Color(0.92f, 0.55f, 0.15f, 0.95f));
        fillTex.Apply();

        _barFillStyle = new GUIStyle(GUI.skin.box);
        _barFillStyle.normal.background = fillTex;
    }
}
