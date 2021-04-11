using System;
using UnityEngine;

//Key on a VrKeyboard. Value set in editor or by VrKeyboardSpawner. KeyboardInputController subscribes to press events.
public class KeyboardKey : MonoBehaviour, ISettableKeyValue, IKeyListenable
{
    public event Action<string> OnPressed;

    public string keyValue;
    public VrButton button;

    public void Start() {
        button.OnClick += KeyPressed;
    }

    public void SetKeyValue(string value) {
        keyValue = value;
    }

    private void KeyPressed() {
        if (!string.IsNullOrEmpty(keyValue)) {
            OnPressed?.Invoke(keyValue);
        }
    }
}
