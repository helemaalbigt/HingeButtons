using System.Collections;
using UnityEngine;

//Send interaction events to interactables by pushing them with a finger
//Stripped down from Cubism's ButtonPusher script
public class ButtonPusher : MonoBehaviour
{
    public bool active = true; //set to false to stop from interacting, while still casting into the scene

    public LayerMask _pushableLayer;
    public HoverHelper _hoverHelper;

    private Ray _ray;
    private Vector3 _castDirection;
    private bool _hitThisFrame;
    private RaycastHit _hit = new RaycastHit();

    private const float CastDepth = 0.08f;

    [Header("Debug")]
    [SerializeField]
    private InteractionListener _hoveringListener; // listener we can push

    private void Start() {
        _castDirection = -SceneFinder.I.vrui.up;
        _hoverHelper.OnTriggerEntered += TriggerEnteredFinger;
    }

    public float GetInteractionDistance() {
        if (_hitThisFrame) {
            return _hit.distance;
        } else {
            return float.PositiveInfinity;
        }
    }

    public Vector3 GetInteractionPoint() {
        if (_hitThisFrame) {
            return _hit.point;
        } else {
            return _ray.origin;// + (_ray.direction * Globals.ButtonCastDepth);
        }
    }

    private void Update() {
        _ray.origin = transform.position;
        _ray.direction = GetCastDirection();

        //VrDebugPlugin.VrDebug.DrawLine(_ray.origin, _ray.origin + _ray.direction * CastDepth, active ? IsHovering() ? Color.green : Color.blue : Color.red);

        if (Physics.Raycast(_ray, out _hit, CastDepth, _pushableLayer)) {
            _hitThisFrame = true;

            if (active) {
                var listener = _hit.transform.GetComponentInParent<InteractionListener>();
                if (_hoveringListener != null && (_hoveringListener != listener)) {
                    _hoveringListener.HoverOff(GetInteractionData());
                }

                if (_hoveringListener != listener || !_hoveringListener.IsHovered()) {
                    _hoveringListener = listener;
                    _hoveringListener.HoverOn(GetInteractionData());
                }
            } else if (_hoveringListener != null) {
                _hoveringListener.HoverOff(GetInteractionData());
                _hoveringListener = null;
            }

        } else {
            _hitThisFrame = false;
            if (_hoveringListener != null) {
                _hoveringListener.HoverOff(GetInteractionData());
                _hoveringListener = null;
            }
        }
    }

    public Vector3 GetCastDirection() {
        return _castDirection;
    }

    private void OnDisable() {
        if (_hoveringListener != null) {
            _hoveringListener.HoverOff(GetInteractionData());
            _hoveringListener = null;
        }
    }

    public bool IsHovering() {
        return _hoveringListener != null;
    }

    private InteractionData GetInteractionData() {
        return new InteractionData() {
            interactingTransform = transform,
        };
    }

    private void TriggerEnteredFinger(Collider aOther) {
        if (_pushableLayer == (_pushableLayer | (1 << aOther.gameObject.layer))) CheckForPush(aOther);
    }

    private void CheckForPush(Collider other) {
        if (_hoveringListener != null) {
            _hoveringListener.ClickDown(GetInteractionData());
            StartCoroutine(WaitAndClickUp(_hoveringListener));

            _hoveringListener.HoverOff(GetInteractionData());
            _hoveringListener = null;
        }
    }

    private IEnumerator WaitAndClickUp(InteractionListener listener) {
        yield return null;
        listener.ClickUp(GetInteractionData());
    }
}

[System.Serializable]
public class InteractionData
{
    public Transform interactingTransform;
}