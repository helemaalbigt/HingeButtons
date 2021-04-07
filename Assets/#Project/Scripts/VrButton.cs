using System;

public class VrButton : Interactable
{
    public event Action OnClick;

    void Start() {
        cursorInteractions.OnHoverOn += OnCursorEnter;
        cursorInteractions.OnHoverOff += OnCursorExit;
        cursorInteractions.OnClickDown += ClickDown;
        cursorInteractions.OnClickUp += ClickUp;
    }

    private void OnCursorEnter(InteractionData data) {
        if (active) {
            interactableState = InteractableState.normalHover;
        }
    }

    private void OnCursorExit(InteractionData data) {
        if (active) {
            interactableState = InteractableState.normal;
        }
    }

    private void ClickDown(InteractionData data) {
        if (active && interactableState != InteractableState.clicked) {
            interactableState = InteractableState.clicked;
            FireOnClick();
        }
    }

    private void ClickUp(InteractionData data) {
        if (active) {
            if (cursorInteractions.IsHovered()) {
                interactableState = InteractableState.normalHover;
            } else {
                interactableState = InteractableState.normal;
            }
        }
    }

    private void OnDisable() {
        interactableState = InteractableState.normal;
    }

    private void FireOnClick() {
        if (OnClick != null)
            OnClick();
    }
}
