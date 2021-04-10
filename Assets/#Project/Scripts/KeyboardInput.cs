using UnityEngine.UI;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public GameObject _keyWrapper;
    public Text _uiText;

    private string _text;

    private void Start() {
        var keys = _keyWrapper.GetComponentsInChildren<KeyInput>();
        foreach (var key in keys) {
            key.OnKeyPressed += ProcessKeyStroke;
        }

        _text = "";
        UpdateUiText();
    }

    private void ProcessKeyStroke(string input) {
        switch (input) {
            case "BACKSPACE":
                BackSpace();
                break;

            default:
                AddString(input);
                break;
        }
    }

    private void BackSpace() {
        if (_text.Length > 0) {
            _text = _text.Substring(0, _text.Length - 1);
            UpdateUiText();
        }
    }

    private void AddString(string value) {
        if (_text.Length < 4) {
            _text += value;
            UpdateUiText();
        }
    }

    private void UpdateUiText() {
        var uiTextValue = _text;
        while (uiTextValue.Length < 4) {
            uiTextValue += "_";
        }
        _uiText.text = uiTextValue;
    }
}
