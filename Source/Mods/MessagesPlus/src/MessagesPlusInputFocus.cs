using System;
using MelonLoader;
using S1API.Input;
using UnityEngine;
using UnityEngine.UI;

namespace MessagesPlus;

/// <summary>
/// InputFocus guard (Key Rule 5) for the MessagesPlus search field: while the
/// InputField is focused, `Controls.IsTyping` suppresses player movement
/// (WASD). Registered via ClassInjector in Mod.OnInitializeMelon — the public
/// IntPtr constructor is mandatory (without it AddComponent crashes the IL2CPP
/// bridge). Mirrors NotesAppInputFocus, minus the editor auto-focus.
/// </summary>
[RegisterTypeInIl2Cpp]
internal sealed class MessagesPlusInputFocus : MonoBehaviour
{
    public MessagesPlusInputFocus(IntPtr ptr) : base(ptr) { }

    public InputField searchInput = null!;

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
