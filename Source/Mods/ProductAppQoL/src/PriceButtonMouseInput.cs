using System;
using Il2CppInterop.Runtime.Attributes;
using Il2CppScheduleOne.UI.Phone.ProductManagerApp;
using MelonLoader;
using UnityEngine;

namespace ProductAppQoL;

/// <summary>
/// Pointer handling for one asking-price button (Add/Remove). The vanilla Button
/// component stays untouched — this only ADDS mouse/touch behaviour: tap = one
/// ±$1 step, hold = repeat that accelerates, mirroring the vanilla gamepad
/// InputValueRamp feel.
///
/// Polling design (deliberate): Il2CppInterop exposes the EventSystem handler
/// interfaces (IPointerDownHandler ...) as classes, so a MonoBehaviour cannot
/// implement them — CS1721. Like MessagesPlusInputFocus, this watches the input
/// state instead: RectTransformUtility.RectangleContainsScreenPoint + Input
/// mouse/touch state, which works for touch too (Unity simulates mouse from it).
/// Registered in the il2cpp domain so it can live on the vanilla button objects.
/// </summary>
[RegisterTypeInIl2Cpp]
internal class PriceButtonMouseInput : MonoBehaviour
{
    private const float HoldDelay = 0.4f;      // seconds a hold must last before it repeats
    private const float RepeatStart = 0.13f;   // first repeat interval
    private const float RepeatMin = 0.05f;     // fastest repeat interval
    private const float Acceleration = 0.85f;  // repeat-interval multiplier per step

    private ProductAppDetailPanel _panel;
    private RectTransform _rect;
    private Camera _camera;
    private float _step;
    private bool _held;
    private float _repeatInterval = RepeatStart;
    private float _nextRepeat;

    public PriceButtonMouseInput(IntPtr ptr) : base(ptr) { }

    public void Setup(ProductAppDetailPanel panel, Transform button, float step)
    {
        _panel = panel;
        _step = step;
        _rect = button.GetComponent<RectTransform>();
        Canvas canvas = button.GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            _camera = canvas.worldCamera;
        }
    }

    private void Update()
    {
        try
        {
            if (_panel == null || _rect == null) return;
            if (!_rect.gameObject.activeInHierarchy) return;

            bool over = RectTransformUtility.RectangleContainsScreenPoint(_rect, Input.mousePosition, _camera);

            if (Input.GetMouseButtonDown(0) && over)
            {
                _held = true;
                _repeatInterval = RepeatStart;
                _nextRepeat = Time.unscaledTime + HoldDelay;
                Apply();
                return;
            }

            if (!_held) return;
            if (!Input.GetMouseButton(0) || !over)
            {
                _held = false;   // released or dragged off the button
                return;
            }

            if (Time.unscaledTime < _nextRepeat) return;
            Apply();
            _repeatInterval = Mathf.Max(RepeatMin, _repeatInterval * Acceleration);
            _nextRepeat = Time.unscaledTime + _repeatInterval;
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] input poll failed: {ex.Message}");
        }
    }

    private void Apply()
    {
        try
        {
            if (_panel == null) return;
            _panel.AdjustPrice(_step);
            _panel.UpdatePrice();
        }
        catch (Exception ex)
        {
            MelonLogger.Warning($"[ProductAppQoL] AdjustPrice failed: {ex.Message}");
        }
    }
}
