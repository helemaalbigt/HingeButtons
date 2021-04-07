using System;
using UnityEngine;

public class InteractionListener : MonoBehaviour
{
    public event Action<InteractionData> OnHoverOn;
    public event Action<InteractionData> OnHoverOff;
    public event Action<InteractionData> OnClickDown;
    public event Action<InteractionData> OnClickUp;

    protected InteractionData _lastInteractionData;

    [Header("Debug")]
    [SerializeField]
    private bool _hovered;
    [SerializeField]
    private bool _logInteractions;

    private void OnDisable() {
        ForceRemoveInteractions();
    }

    public bool IsHovered() {
        return _hovered;
    }

    public void HoverOn(InteractionData data) {
        if (_logInteractions)
            Debug.Log(gameObject.name + " Hovered on by " + data.interactingTransform.name);

        _hovered = true;
        _lastInteractionData = data;

        OnHoverOn?.Invoke(data);
    }

    public void HoverOff(InteractionData data) {
        if (_logInteractions)
            Debug.Log(gameObject.name + " Hovered off by " + data.interactingTransform.name);

        _hovered = false;
        _lastInteractionData = data;

        OnHoverOff?.Invoke(data);
    }

    public void ClickDown(InteractionData data) {
        if (_logInteractions)
            Debug.Log(gameObject.name + " clicked down by " + data.interactingTransform.name);

        _lastInteractionData = data;

        OnClickDown?.Invoke(data);
    }

    public void ClickUp(InteractionData data) {
        if (_logInteractions)
            Debug.Log(gameObject.name + " clicked up by " + data.interactingTransform.name);

        _lastInteractionData = data;

        OnClickUp?.Invoke(data);
    }

    public void ForceRemoveInteractions() {
        if (_logInteractions)
            Debug.Log(gameObject.name + " Force removed all interactions");

        _hovered = false;
    }
}