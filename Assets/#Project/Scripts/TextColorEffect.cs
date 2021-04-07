using UnityEngine.UI;
using UnityEngine;

public class TextColorEffect : Effect
{
    public Text _text;

    public Color _normal;
    public Color _hovered;
    public Color _clicked;
    public Color _clickedHovered;

    protected override void OnStateChange(InteractableState state) {
        switch (state) {
            case InteractableState.normalHover:
                SetColor(_hovered);
                break;
            case InteractableState.clicked:
                SetColor(_clicked);
                break;
            case InteractableState.clickedHover:
                SetColor(_clickedHovered);
                break;
            default:
            case InteractableState.normal:
                SetColor(_normal);
                break;
        }
    }

    private void SetColor(Color color) {
        _text.color = color;
    }
}
