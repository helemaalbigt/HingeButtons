using UnityEngine;

public class AnimationEffect : Effect
{
    public Animator _animator;

    public string _normal;
    public string _hovered;
    public string _clicked;
    public string _clickedHovered;

    protected override void OnStateChange(InteractableState state) {
        switch (state) {
            case InteractableState.normalHover:
                PlayAnim(_hovered);
                break;
            case InteractableState.clicked:
                PlayAnim(_clicked);
                break;
            case InteractableState.clickedHover:
                PlayAnim(_clickedHovered);
                break;
            default:
            case InteractableState.normal:
                PlayAnim(_normal);
                break;
        }
    }

    private void PlayAnim(string anim) {
        if (!string.IsNullOrEmpty(anim))
            _animator.Play(anim);
    }
}
