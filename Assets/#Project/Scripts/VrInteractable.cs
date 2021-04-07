using System;

//generic interaction events
public class VrInteractable : Interactable
{
    public event Action OnHoverOn;
    public event Action OnHoverOff;
    public event Action OnClick;

    private bool _hovered = false;

    public void Awake() {
        cursorInteractions.OnHoverOn += OnCursorEnter;
        cursorInteractions.OnHoverOff += OnCursorExit;
        cursorInteractions.OnClickDown += OnClickDown;
    }

    protected void OnCursorEnter(InteractionData data) {
        OnCursorEnter();
    }

    protected void OnCursorEnter() {
        if (!_hovered) {
            _hovered = true;
            interactableState = InteractableState.normalHover;
            if (OnHoverOn != null)
                OnHoverOn();
        }
    }

    protected void OnCursorExit(InteractionData data) {
        OnCursorExit();
    }

    protected void OnCursorExit() {
        if (_hovered) {
            _hovered = false;
            interactableState = InteractableState.normal;
            if (OnHoverOff != null)
                OnHoverOff();
        }
    }

    protected void OnClickDown(InteractionData data) {
        if (OnClick != null)
            OnClick();
    }
}