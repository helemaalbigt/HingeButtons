using System.Collections.Generic;
using System;
using UnityEngine;

//Wrapper for hand tracking functionality
public class HandTracking : HandDataProvider
{
    public event Action<bool> OnActiveChanged;

    public HandDataProvider _handDataOvr;

    public static HandTracking I;

    private bool _isActive;

    void Awake() {
        if (I == null)
            I = this;
    }

    protected virtual void Update() {
        if (_isActive != IsActive) {
            _isActive = IsActive;
            OnActiveChanged?.Invoke(_isActive);
        }
    }

    public override bool IsActive {
        get { return GetData().IsActive; }
    }

    public override Dictionary<BoneId, Transform> GetBones(Hand hand) {
        return GetData().GetBones(hand);
    }

    public override bool BonesInitialized() {
        return GetData().BonesInitialized();
    }

    public override Transform GetBoneUnsafe(Hand hand, BoneId Id) {
        return GetData().GetBoneUnsafe(hand, Id);
    }

    public override bool TryGetBone(Hand hand, BoneId Id, out Transform bone) {
        return GetData().TryGetBone(hand, Id, out bone);
    }

    public override float GetTrackingConfidence(Hand hand) {
        return GetData().GetTrackingConfidence(hand);
    }

    public override float GetFingerTrackingConfidence(Hand hand, Finger finger) {
        return GetData().GetFingerTrackingConfidence(hand, finger);
    }

    public override float HandScale(Hand hand) {
        return GetData().HandScale(hand);
    }

    public override bool IsTracking(Hand hand) {
        return GetData().IsTracking(hand);
    }

    public virtual bool IsPinching(Hand hand, Finger finger) {
        return GetData().IsPinching(hand, finger);
    }

    public virtual bool PinchDown(Hand hand, Finger finger) {
        return GetData().PinchDown(hand, finger);
    }

    public virtual bool PinchUp(Hand hand, Finger finger) {
        return GetData().PinchUp(hand, finger);
    }

    public float GetDistanceToThumb(Hand hand, Finger finger) {
        var gotThumb = TryGetBone(hand, BoneId.Hand_Thumb_Tip, out Transform thumbTip);
        var gotFinger = TryGetBone(hand, FingerToFingerTipId(finger), out Transform fingerTip);
        if (gotThumb && gotFinger) {
            return Vector3.Distance(thumbTip.position, fingerTip.position);
        } else {
            return -1f;
        }
    }

    public Vector3 GetMidPointToThumb(Hand hand, Finger finger) {
        var gotThumb = TryGetBone(hand, BoneId.Hand_Thumb_Tip, out Transform thumbTip);
        var gotFinger = TryGetBone(hand, FingerToFingerTipId(finger), out Transform fingerTip);
        if (gotThumb && gotFinger) {
            return (thumbTip.position + fingerTip.position) / 2f;
        } else {
            return default(Vector3);
        }
    }

    protected BoneId FingerToFingerTipId(Finger finger) {
        switch (finger) {
            case Finger.index:
                return BoneId.Hand_Index_Tip;
            case Finger.middle:
                return BoneId.Hand_Middle_Tip;
            case Finger.ring:
                return BoneId.Hand_Ring_Tip;
            case Finger.pinky:
                return BoneId.Hand_Pinky_Tip;
            default:
            case Finger.thumb:
                return BoneId.Hand_Thumb_Tip;
        }
    }

    public override bool SystemGestureInProgress(Hand hand) {
        return GetData().SystemGestureInProgress(hand);
    }

    private HandDataProvider GetData() {
        //When supporting other HT tracking sources, set them here
        return _handDataOvr;
    }
}

public enum Hand
{
    left,
    right
}

public enum Finger
{
    thumb,
    index,
    middle,
    ring,
    pinky
}
