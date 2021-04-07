using UnityEngine;

//Base class for interaction effects which visualize an interactables state
public abstract class Effect : MonoBehaviour
{
    public Interactable interactable;

    public virtual void Start() {
        interactable.onButtonStateChange += OnStateChange;

        OnStateChange(interactable.interactableState);
    }

    protected abstract void OnStateChange(InteractableState state);
}
