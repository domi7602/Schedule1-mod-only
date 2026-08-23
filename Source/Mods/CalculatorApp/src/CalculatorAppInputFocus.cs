using System;
using MelonLoader;
using S1API.Input;
using UnityEngine;
using UnityEngine.UI;

namespace CalculatorApp;

/// <summary>
/// Monitors UI input focus within CalculatorApp (e.g., the history search input)
/// and toggles Controls.IsTyping to prevent player movement or game hotkey triggers while typing.
/// </summary>
[RegisterTypeInIl2Cpp]
public sealed class CalculatorAppInputFocus : MonoBehaviour
{
    public CalculatorAppInputFocus(IntPtr ptr) : base(ptr) { }

    public InputField? searchInput;
    private bool _lastTyping;

    private void Update()
    {
        bool typing = searchInput != null && searchInput.isFocused;
        if (typing != _lastTyping)
        {
            _lastTyping = typing;
            Controls.IsTyping = typing;
        }
    }

    private void OnDisable()
    {
        _lastTyping = false;
        Controls.IsTyping = false;
    }
}
