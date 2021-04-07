using UnityEngine;
using System;

public enum InteractableState
{
    normal,
    normalHover,
    clicked,
    clickedHover
}

public abstract class Interactable : MonoBehaviour
{
    public event Action<InteractableState> onButtonStateChange;

    public bool active = true;

    public InteractionListener cursorInteractions;

    public static Interactable focusedInteractable;

    [Header("Debug")]
    [SerializeField]
    private InteractableState _interactableState;
    public InteractableState interactableState {
        get {
            return _interactableState;
        }
        set {
            if (value != _interactableState) {
                _interactableState = value;

                if (onButtonStateChange != null)
                    onButtonStateChange(_interactableState);

                if (value == InteractableState.clicked && _interactableState != InteractableState.clickedHover)
                    focusedInteractable = this;
            }
        }
    }
}
