using System.Collections;
using UnityEngine;

public class ButtonPusher : MonoBehaviour
{
    public bool active = true; //set to false to stop from interacting, while still casting into the scene

    public LayerMask _pushableLayer;

    [Header("Debug")]
    [SerializeField]
    private InteractionListener _hoverZoneListener;
    [SerializeField]
    private InteractionListener _actionZoneListener;

    private Vector3 _moveDirection;
    private Vector3 _prevPos;

    private const float MinPushAngle = 50f;


    private void Update() {
        _moveDirection = transform.position - _prevPos;
        _prevPos = transform.position;
    }

    private void OnDisable() {
        ClearInteractions();
    }

    private void ClearInteractions() {
        if (_hoverZoneListener != null) {
            _hoverZoneListener.HoverOff(GetInteractionData());
            _hoverZoneListener = null;
        } else if (_actionZoneListener != null) {
            _actionZoneListener.HoverOff(GetInteractionData());
            _actionZoneListener = null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (_pushableLayer == (_pushableLayer | (1 << other.gameObject.layer))) {
            var triggerZone = other.GetComponent<TriggerZone>();
            var listener = other.GetComponentInParent<InteractionListener>();

            if (triggerZone != null && listener != null) {
                if (triggerZone.depth == InteractionDepth.proximity) {
                    _hoverZoneListener = listener;

                    if (listener != _actionZoneListener) {
                        _hoverZoneListener.HoverOn(GetInteractionData());
                    }
                }

                if (triggerZone.depth == InteractionDepth.action) {
                    var buttonDown = -listener.transform.up;
                    var pushAngle = Vector3.Angle(buttonDown, _moveDirection);
                    var validPushAngle = pushAngle <= MinPushAngle;

                    if (listener == _hoverZoneListener && validPushAngle) {
                        _actionZoneListener = listener;
                        _actionZoneListener.ClickDown(GetInteractionData());
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (_pushableLayer == (_pushableLayer | (1 << other.gameObject.layer))) {
            var triggerZone = other.GetComponent<TriggerZone>();
            var listener = other.GetComponentInParent<InteractionListener>();

            if (triggerZone != null && listener != null) {
                if (triggerZone.depth == InteractionDepth.action) {

                    if (_actionZoneListener == listener) {
                        _actionZoneListener = null;
                        listener.ClickUp(GetInteractionData());

                        if (_hoverZoneListener == null || listener != _hoverZoneListener) {
                            listener.HoverOff(GetInteractionData());
                        }
                    }
                }

                if (triggerZone.depth == InteractionDepth.proximity) {
                    if (_hoverZoneListener == listener) {
                        _hoverZoneListener = null;
                    }

                    if (_actionZoneListener != listener || _actionZoneListener == null) {
                        listener.HoverOff(GetInteractionData());
                    }
                }
            }
        }
    }

    private InteractionData GetInteractionData() {
        return new InteractionData() {
            interactingTransform = transform,
        };
    }
}

[System.Serializable]
public class InteractionData
{
    public Transform interactingTransform;
}