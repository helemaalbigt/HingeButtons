using System.Collections.Generic;
using UnityEngine;

//Interface to get main data from hand tracking input
public abstract class HandDataProvider : MonoBehaviour
{
    public abstract bool IsActive { get; } //takes into account some systems may switch between HT and controllers

    public abstract bool IsTracking(Hand hand);
    public abstract Dictionary<BoneId, Transform> GetBones(Hand hand);
    public abstract bool TryGetBone(Hand hand, BoneId Id, out Transform bone);
    public abstract Transform GetBoneUnsafe(Hand hand, BoneId Id);

    public virtual float HandScale(Hand hand) {
        return 1f;
    }

    public virtual bool BonesInitialized() {
        return true;
    }

    public virtual float GetTrackingConfidence(Hand hand) {
        return 1f;
    }

    public virtual float GetFingerTrackingConfidence(Hand hand, Finger finger) {
        return 1f;
    }

    public virtual bool IsPinching(Hand hand, Finger finger) {
        return false;
    }

    public virtual bool PinchDown(Hand hand, Finger finger) {
        return false;
    }

    public virtual bool PinchUp(Hand hand, Finger finger) {
        return false;
    }

    public virtual bool SystemGestureInProgress(Hand hand) {
        return false;
    }

    public virtual float GetFingerCurl(Hand hand, Finger finger) {
        TryGetBone(hand, BoneId.Hand_Start, out Transform root);

        switch (finger) {
            case Finger.thumb:
                TryGetBone(hand, BoneId.Hand_Thumb0, out Transform t0);
                TryGetBone(hand, BoneId.Hand_Thumb1, out Transform t1);
                TryGetBone(hand, BoneId.Hand_Thumb2, out Transform t2);
                TryGetBone(hand, BoneId.Hand_Thumb3, out Transform t3);
                TryGetBone(hand, BoneId.Hand_Thumb_Tip, out Transform t4);
                return GetCumulativeAngle(new Transform[] { t0, t1, t2, t3, t4 });
            case Finger.middle:
                TryGetBone(hand, BoneId.Hand_Middle1, out Transform m0);
                TryGetBone(hand, BoneId.Hand_Middle2, out Transform m1);
                TryGetBone(hand, BoneId.Hand_Middle3, out Transform m2);
                TryGetBone(hand, BoneId.Hand_Index_Tip, out Transform m3);
                return GetCumulativeAngle(new Transform[] { root, m0, m1, m2, m3 });
            case Finger.ring:
                TryGetBone(hand, BoneId.Hand_Ring1, out Transform r0);
                TryGetBone(hand, BoneId.Hand_Ring2, out Transform r1);
                TryGetBone(hand, BoneId.Hand_Ring3, out Transform r2);
                TryGetBone(hand, BoneId.Hand_Ring_Tip, out Transform r3);
                return GetCumulativeAngle(new Transform[] { root, r0, r1, r2, r3 });
            case Finger.pinky:
                TryGetBone(hand, BoneId.Hand_Pinky0, out Transform p0);
                TryGetBone(hand, BoneId.Hand_Pinky1, out Transform p1);
                TryGetBone(hand, BoneId.Hand_Pinky2, out Transform p2);
                TryGetBone(hand, BoneId.Hand_Pinky_Tip, out Transform p3);
                return GetCumulativeAngle(new Transform[] { root, p0, p1, p2, p3 });
            default:
            case Finger.index:
                TryGetBone(hand, BoneId.Hand_Index1, out Transform i0);
                TryGetBone(hand, BoneId.Hand_Index2, out Transform i1);
                TryGetBone(hand, BoneId.Hand_Index3, out Transform i2);
                TryGetBone(hand, BoneId.Hand_Index_Tip, out Transform i3);
                return GetCumulativeAngle(new Transform[] { root, i0, i1, i2, i3 });
        }
    }

    protected float GetCumulativeAngle(Transform[] points) {
        if (points.Length < 3 || !BonesInitialized()) {
            return 0;
        }

        float totalAngle = 0;
        for (int i = 0; i < points.Length - 2; i++) {
            var p1 = points[i].position;
            var p2 = points[i + 1].position;
            var p3 = points[i + 2].position;
            totalAngle += Vector3.Angle(p3 - p2, p2 - p1);
        }

        return totalAngle;
    }
}