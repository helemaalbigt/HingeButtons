using System;
using UnityEngine;

[RequireComponent(typeof(VrButton))]
public class KeyInput : MonoBehaviour, ISettableKeyValue
{
    public event Action<string> OnKeyPressed;

    public string keyValue;

    private VrButton _button;

    private void Start() {
        _button = GetComponent<VrButton>();
        _button.OnClick += ButtonClicked;
    }

    private void ButtonClicked() {
        OnKeyPressed?.Invoke(keyValue);
    }

    public void SetKeyValue(string value) {
        keyValue = value;
    }
}
