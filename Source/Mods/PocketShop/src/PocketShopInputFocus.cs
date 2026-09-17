using System;
using MelonLoader;
using S1API.Input;
using UnityEngine;
using UnityEngine.UI;

namespace PocketShop;

[RegisterTypeInIl2Cpp]
public sealed class PocketShopInputFocus : MonoBehaviour
{
    public PocketShopInputFocus(IntPtr ptr) : base(ptr) { }

    public InputField? inputField;

    private bool _lastTyping;

    private void Update()
    {
        bool typing = inputField != null && inputField.isFocused;
        if (typing != _lastTyping)
        {
            _lastTyping = typing;
            Controls.IsTyping = typing;
        }
    }

    private void OnDisable()
    {
        if (_lastTyping)
        {
            _lastTyping = false;
            Controls.IsTyping = false;
        }
    }

    private void OnDestroy()
    {
        if (_lastTyping)
        {
            _lastTyping = false;
            Controls.IsTyping = false;
        }
    }
}
