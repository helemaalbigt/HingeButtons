using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimationTrigger : MonoBehaviour
{
    public InteractionListener _listener;
    public Animator _animator;

    public string _clickedDown;
    public string _clickedUp;

    private void Start() {
        _listener.OnClickDown += data => SetTrigger(_clickedDown);
        _listener.OnClickUp += data => SetTrigger(_clickedUp);
    }

    private void SetTrigger(string triggerName) {
        if (!string.IsNullOrEmpty(triggerName))
            _animator.SetTrigger(triggerName);
    }
}
