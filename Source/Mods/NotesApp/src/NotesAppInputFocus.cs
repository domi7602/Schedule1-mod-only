using System;
using MelonLoader;
using S1API.Input;
using UnityEngine;
using UnityEngine.UI;

namespace NotesApp;

[RegisterTypeInIl2Cpp]
internal sealed class NotesAppInputFocus : MonoBehaviour
{
    public NotesAppInputFocus(IntPtr ptr) : base(ptr) { }

    public InputField titleInput = null!;
    public InputField textInput = null!;
    public InputField searchInput = null!;

    private bool _lastTyping;

    private void OnEnable()
    {
        if (UnityEngine.EventSystems.EventSystem.current == null)
        {
            return;
        }

        var target = titleInput != null ? titleInput : textInput;
        if (target == null)
        {
            return;
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(target.gameObject);
        target.Select();
        target.ActivateInputField();
        target.MoveTextEnd(false);
    }

    private void Update()
    {
        bool typing = (titleInput != null && titleInput.isFocused)
                      || (textInput != null && textInput.isFocused)
                      || (searchInput != null && searchInput.isFocused);
        if (typing != _lastTyping)
        {
            _lastTyping = typing;
            Controls.IsTyping = typing;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (titleInput != null && titleInput.isFocused && textInput != null)
            {
                textInput.Select();
                textInput.ActivateInputField();
            }
            else if (textInput != null && textInput.isFocused && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && titleInput != null)
            {
                titleInput.Select();
                titleInput.ActivateInputField();
            }
        }
    }

    private void OnDisable()
    {
        _lastTyping = false;
        Controls.IsTyping = false;
    }
}
