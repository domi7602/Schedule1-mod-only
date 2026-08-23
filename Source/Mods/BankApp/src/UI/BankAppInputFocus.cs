using System;
using MelonLoader;
using S1API.Input;
using UnityEngine;
using UnityEngine.UI;

namespace BankApp.UI;

/// <summary>
/// Monitors UI input focus within BankApp (e.g. custom amount input fields)
/// and toggles Controls.IsTyping to prevent player movement (WASD) while typing.
/// </summary>
[RegisterTypeInIl2Cpp]
public sealed class BankAppInputFocus : MonoBehaviour
{
    public BankAppInputFocus(IntPtr ptr) : base(ptr) { }

    public InputField? amountInput;
    private bool _lastTyping;

    private void Update()
    {
        bool typing = amountInput != null && amountInput.isFocused;
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
