using System;
using MelonLoader;
using S1API.Input;
using UnityEngine;
using UnityEngine.UI;

namespace BankApp
{
    /// <summary>
    /// Flags the phone's text-input focus to the game while the amount field is active, so
    /// WASD does not drive the player while typing a transfer amount. Re-introduced alongside
    /// the guarded direct-input field (the mod previously shipped without any text field).
    /// </summary>
    [RegisterTypeInIl2Cpp]
    internal sealed class BankAppInputFocus : MonoBehaviour
    {
        public InputField? amountInput;

        private bool _lastTyping;

        public BankAppInputFocus(IntPtr ptr)
            : base(ptr)
        {
        }

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
}
