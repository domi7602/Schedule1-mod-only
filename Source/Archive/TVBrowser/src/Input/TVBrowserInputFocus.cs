using System;
using Il2CppTMPro;
using MelonLoader;
using S1API.Input;
using UnityEngine;

namespace TVBrowser.Input;

[RegisterTypeInIl2Cpp]
public sealed class TVBrowserInputFocus : MonoBehaviour
{
    public TVBrowserInputFocus(IntPtr ptr) : base(ptr) { }

    public TMP_InputField? tmpUrlInput;
    public Action<string>? OnSubmitUrl;
    public Action? OnEscapePressed;

    private bool _lastTyping;

    private void Update()
    {
        var input = tmpUrlInput;
        bool typing = input != null && input.Pointer != IntPtr.Zero && !input.WasCollected && input.isFocused;
        if (typing != _lastTyping)
        {
            _lastTyping = typing;
            Controls.IsTyping = typing;
        }

        if (typing && input != null && input.Pointer != IntPtr.Zero && !input.WasCollected)
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Return) || UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (!string.IsNullOrWhiteSpace(input.text))
                {
                    string target = input.text.Trim();
                    OnSubmitUrl?.Invoke(target);
                }
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                if (UnityEngine.EventSystems.EventSystem.current != null)
                {
                    UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                }
                OnEscapePressed?.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        _lastTyping = false;
        Controls.IsTyping = false;
    }
}
