using System;
using MelonLoader;
using S1API.Input;
using UnityEngine;
using UnityEngine.UI;

namespace StorageScanner
{
    /// <summary>Flags the phone's text input focus to the game while the search field is active.</summary>
    [RegisterTypeInIl2Cpp]
    internal sealed class StorageScannerInputFocus : MonoBehaviour
    {
        public InputField? searchInput;

        private bool _lastTyping;

        public StorageScannerInputFocus(IntPtr ptr)
            : base(ptr)
        {
        }

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
}
