using UnityEngine;

public class MaterialSwapEffect : Effect
{
    public Renderer _renderer;

    public Material _normal;
    public Material _hovered;
    public Material _clicked;
    public Material _clickedHovered;

    protected override void OnStateChange(InteractableState state) {
        switch (state) {
            case InteractableState.normalHover:
                SetMaterial(_hovered);
                break;
            case InteractableState.clicked:
                SetMaterial(_clicked);
                break;
            case InteractableState.clickedHover:
                SetMaterial(_clickedHovered);
                break;
            default:
            case InteractableState.normal:
                SetMaterial(_normal);
                break;
        }
    }

    private void SetMaterial(Material mat) {
        if (mat != null)
            _renderer.material = mat;
    }
}
