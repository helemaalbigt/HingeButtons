using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

//Makes info about hovering colliders available to other scripts
public class HoverHelper : MonoBehaviour
{
    public LayerMask interactableLayers;
    public List<InteractionListener> interactingListeners = new List<InteractionListener>();
    public event Action<Collider> OnTriggerEntered;

    private readonly List<InteractionListener> _stayingListeners = new List<InteractionListener>();

    private void OnTriggerEnter(Collider other) {
        if (interactableLayers == (interactableLayers | (1 << other.gameObject.layer))) {
            OnTriggerEntered?.Invoke(other);
        }
    }

    private void OnTriggerStay(Collider aOther) {
        if (interactableLayers == (interactableLayers | (1 << aOther.gameObject.layer))) HoverStay(aOther);
    }

    private void FixedUpdate() {
        interactingListeners = _stayingListeners;
        _stayingListeners.Clear();
    }

    public bool HoveringOverLayer(LayerMask layer, out InteractionListener[] hoveredListener) {
        return HoveringOverLayerTrigger(layer, out hoveredListener);
    }

    private bool HoveringOverLayerTrigger(LayerMask layer, out InteractionListener[] hoveredListener) {
        hoveredListener = interactingListeners.Where(IL => layer == (layer | (1 << IL.gameObject.layer))).ToArray();
        return !hoveredListener.All(x => x == null);
    }

    private void HoverStay(Collider other) {
        var listener = other.GetComponentInParent<InteractionListener>();
        if (listener == null)
            return;

        if (!interactingListeners.Contains(listener)) {
            _stayingListeners.Add(listener);
        }
    }
}
