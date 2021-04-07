using System.Collections.Generic;
using UnityEngine;

//Based on Luca Mefisto's repo
//https://github.com/MephestoKhaan/HandPosing

/// Custom implementation for the Oculus plugin of the SkeletonDataProvider.
/// It is important to note that since OVRSkeleton gets executed at -80, we want
/// to update the data as soon as it is available (hence the -70)
[DefaultExecutionOrder(-70)]
public class HandDataProviderOVR : HandDataProvider
{
    public OVRSkeleton _ovrSkeletonLeft;
    public OVRSkeleton _ovrSkeletonRight;
    public OVRHand _ovrHandLeft;
    public OVRHand _ovrHandRight;

    private static readonly Dictionary<OVRSkeleton.BoneId, BoneId> OVRToPosingIDs =
        new Dictionary<OVRSkeleton.BoneId, BoneId>() {
                {OVRSkeleton.BoneId.Invalid , BoneId.Invalid},
                {OVRSkeleton.BoneId.Hand_Start , BoneId.Hand_Start},
                {OVRSkeleton.BoneId.Hand_Thumb0 , BoneId.Hand_Thumb0},
                {OVRSkeleton.BoneId.Hand_Thumb1 , BoneId.Hand_Thumb1},
                {OVRSkeleton.BoneId.Hand_Thumb2 , BoneId.Hand_Thumb2},
                {OVRSkeleton.BoneId.Hand_Thumb3 , BoneId.Hand_Thumb3},
                {OVRSkeleton.BoneId.Hand_Index1 , BoneId.Hand_Index1},
                {OVRSkeleton.BoneId.Hand_Index2 , BoneId.Hand_Index2},
                {OVRSkeleton.BoneId.Hand_Index3 , BoneId.Hand_Index3},
                {OVRSkeleton.BoneId.Hand_Middle1 , BoneId.Hand_Middle1},
                {OVRSkeleton.BoneId.Hand_Middle2 , BoneId.Hand_Middle2},
                {OVRSkeleton.BoneId.Hand_Middle3 , BoneId.Hand_Middle3},
                {OVRSkeleton.BoneId.Hand_Ring1 , BoneId.Hand_Ring1},
                {OVRSkeleton.BoneId.Hand_Ring2 , BoneId.Hand_Ring2},
                {OVRSkeleton.BoneId.Hand_Ring3 , BoneId.Hand_Ring3},
                {OVRSkeleton.BoneId.Hand_Pinky0 , BoneId.Hand_Pinky0},
                {OVRSkeleton.BoneId.Hand_Pinky1 , BoneId.Hand_Pinky1},
                {OVRSkeleton.BoneId.Hand_Pinky2 , BoneId.Hand_Pinky2},
                {OVRSkeleton.BoneId.Hand_Pinky3 , BoneId.Hand_Pinky3},
                {OVRSkeleton.BoneId.Hand_ThumbTip , BoneId.Hand_Thumb_Tip},
                {OVRSkeleton.BoneId.Hand_IndexTip , BoneId.Hand_Index_Tip},
                {OVRSkeleton.BoneId.Hand_MiddleTip , BoneId.Hand_Middle_Tip},
                {OVRSkeleton.BoneId.Hand_RingTip , BoneId.Hand_Ring_Tip},
                {OVRSkeleton.BoneId.Hand_PinkyTip , BoneId.Hand_Pinky_Tip},
        };

    private static readonly Dictionary<Finger, OVRHand.HandFinger> FingerToHandFinger = new Dictionary<Finger, OVRHand.HandFinger>() {
        {Finger.thumb , OVRHand.HandFinger.Thumb},
        {Finger.index , OVRHand.HandFinger.Index},
        {Finger.middle , OVRHand.HandFinger.Middle},
        {Finger.ring , OVRHand.HandFinger.Ring},
        {Finger.pinky , OVRHand.HandFinger.Pinky},
    };

    private Dictionary<BoneId, Transform> _bonesLeft = new Dictionary<BoneId, Transform>();
    private Dictionary<BoneId, Transform> _bonesRight = new Dictionary<BoneId, Transform>();

    private bool _bonesInitialized;
    private OVRPlugin.HandState _handStateLeft = new OVRPlugin.HandState();
    private OVRPlugin.HandState _handStateRight = new OVRPlugin.HandState();
    private float _scaleLeftHand;
    private float _scaleRightHand;

    private bool[] _leftPinchValues = new bool[5];
    private bool[] _rightPinchValues = new bool[5];

    private void Start() {
        InvokeRepeating("UpdateHandScales", 0f, 3f);
    }

    protected void Update() {
        if (CanInitialise) {
            Debug.Log("[HandDataProviderOVR] Initializing bones");
            InitializeBones(ref _bonesLeft, _ovrSkeletonLeft);
            InitializeBones(ref _bonesRight, _ovrSkeletonRight);
            _bonesInitialized = true;
        }
    }

    protected void LateUpdate() {
        if (IsActive && _bonesInitialized) {
            _leftPinchValues[(int)Finger.thumb] = IsPinching(Hand.left, Finger.thumb);
            _leftPinchValues[(int)Finger.index] = IsPinching(Hand.left, Finger.index);
            _leftPinchValues[(int)Finger.middle] = IsPinching(Hand.left, Finger.middle);
            _leftPinchValues[(int)Finger.ring] = IsPinching(Hand.left, Finger.ring);
            _leftPinchValues[(int)Finger.pinky] = IsPinching(Hand.left, Finger.pinky);

            _rightPinchValues[(int)Finger.thumb] = IsPinching(Hand.right, Finger.thumb);
            _rightPinchValues[(int)Finger.index] = IsPinching(Hand.right, Finger.index);
            _rightPinchValues[(int)Finger.middle] = IsPinching(Hand.right, Finger.middle);
            _rightPinchValues[(int)Finger.ring] = IsPinching(Hand.right, Finger.ring);
            _rightPinchValues[(int)Finger.pinky] = IsPinching(Hand.right, Finger.pinky);
        }
    }

    public override bool IsActive {
        get {
#if UNITY_ANDROID
            return OVRPlugin.GetHandTrackingEnabled();
#else
            return false;
#endif
        }
    }

    public override bool IsTracking(Hand hand) {
        var skeleton = hand == Hand.left ? _ovrSkeletonLeft : _ovrSkeletonRight;
        var ovrHand = hand == Hand.left ? _ovrHandLeft : _ovrHandRight;

        return ovrHand.IsTracked;
    }

    public virtual bool BonesInitialized() {
        return _bonesInitialized;
    }

    public override bool TryGetBone(Hand hand, BoneId Id, out Transform bone) {
        var bones = Hand.left == hand ? _bonesLeft : _bonesRight;
        return bones.TryGetValue(Id, out bone);
    }

    public override Transform GetBoneUnsafe(Hand hand, BoneId Id) {
        var bones = Hand.left == hand ? _bonesLeft : _bonesRight;
        return bones[Id];
    }

    public override Dictionary<BoneId, Transform> GetBones(Hand hand) {
        return hand == Hand.left ? _bonesLeft : _bonesRight;
    }

    public override float HandScale(Hand hand) {
        return hand == Hand.left ? _scaleLeftHand : _scaleRightHand;
    }

    private void UpdateHandScales() {
        _scaleLeftHand = GetUpdatedHandScale(Hand.left);
        _scaleRightHand = GetUpdatedHandScale(Hand.right);
    }

    private float GetUpdatedHandScale(Hand hand) {
        if (IsTracking(hand)) {
            var skeleton = hand == Hand.left ? _ovrSkeletonLeft : _ovrSkeletonRight;
            OVRPlugin.Hand handeness = skeleton.GetSkeletonType() == OVRSkeleton.SkeletonType.HandLeft ? OVRPlugin.Hand.HandLeft : OVRPlugin.Hand.HandRight;
            var handState = hand == Hand.left ? _handStateLeft : _handStateRight;

            if (OVRPlugin.GetHandState(OVRPlugin.Step.Render, handeness, ref handState)) {
                return handState.HandScale;
            } else {
                return 1f;
            }
        } else {
            return 1f;
        }
    }

    public override float GetTrackingConfidence(Hand hand) {
        return hand == Hand.left ? GetFrameConfidence(Hand.left) : GetFrameConfidence(Hand.right);
    }

    private float GetFrameConfidence(Hand hand) {
        return (GetFrameHandConfidence(hand) * 1.5f + GetFingerTrackingConfidence(hand, Finger.thumb) + GetFingerTrackingConfidence(hand, Finger.index)) / 3.5f;
    }

    private float GetFrameHandConfidence(Hand hand) {
        if (IsTracking(hand)) {
            var ovrHand = hand == Hand.left ? _ovrHandLeft : _ovrHandRight;
            switch (ovrHand.HandConfidence) {
                case OVRHand.TrackingConfidence.High:
                    return 1f;
                case OVRHand.TrackingConfidence.Low:
                    return 0.5f;
                default:
                    return 0f;
            }
        } else {
            return 0f;
        }
    }

    public override float GetFingerTrackingConfidence(Hand hand, Finger finger) {
        if (IsTracking(hand)) {
            var ovrHand = hand == Hand.left ? _ovrHandLeft : _ovrHandRight;
            FingerToHandFinger.TryGetValue(finger, out OVRHand.HandFinger handFinger);
            switch (ovrHand.GetFingerConfidence(handFinger)) {
                case OVRHand.TrackingConfidence.High:
                    return 1f;
                case OVRHand.TrackingConfidence.Low:
                    return 0.5f;
                default:
                    return 0f;
            }
        } else {
            return 0f;
        }
    }

    public override bool IsPinching(Hand hand, Finger finger) {
        var ovrHand = hand == Hand.left ? _ovrHandLeft : _ovrHandRight;
        FingerToHandFinger.TryGetValue(finger, out OVRHand.HandFinger handFinger);
        return ovrHand.GetFingerIsPinching(handFinger);
    }

    public override bool PinchDown(Hand hand, Finger finger) {
        var pinchValues = hand == Hand.left ? _leftPinchValues : _rightPinchValues;
        return IsPinching(hand, finger) && !pinchValues[(int)finger];
    }

    public override bool PinchUp(Hand hand, Finger finger) {
        var pinchValues = hand == Hand.left ? _leftPinchValues : _rightPinchValues;
        return !IsPinching(hand, finger) && pinchValues[(int)finger];
    }

    public override bool SystemGestureInProgress(Hand hand) {
        var ovrHand = hand == Hand.left ? _ovrHandLeft : _ovrHandRight;
        return ovrHand.IsSystemGestureInProgress;
    }

    private bool CanInitialise => IsActive
        && _ovrSkeletonLeft != null
        && _ovrSkeletonRight != null
        && _ovrSkeletonLeft.IsInitialized
        && _ovrSkeletonRight.IsInitialized
        && !_bonesInitialized;

    private void InitializeBones(ref Dictionary<BoneId, Transform> bones, OVRSkeleton ovrSkeleton) {
        foreach (var bone in ovrSkeleton.Bones) {
            if (OVRToPosingIDs.TryGetValue(bone.Id, out BoneId id)) {
                bones.Add(id, bone.Transform);
            }
        }
    }
}
