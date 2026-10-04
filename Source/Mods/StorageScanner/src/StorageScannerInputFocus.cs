using System;
using MelonLoader;
using S1API.Input;
using UnityEngine;
using UnityEngine.UI;

namespace StorageScanner
{
    [RegisterTypeInIl2Cpp]
    internal sealed class StorageScannerInputFocus : MonoBehaviour
    {
        public InputField? searchInput;
        private bool _lastTyping;

        public StorageScannerInputFocus(IntPtr ptr) : base(ptr) { }

        private void Update()
        {
            bool typing = searchInput != null && searchInput.isFocused;
            if (typing == _lastTyping) return;
            _lastTyping = typing;
            Controls.IsTyping = typing;
        }

        private void OnDisable()
        {
            _lastTyping = false;
            Controls.IsTyping = false;
        }
    }
}
