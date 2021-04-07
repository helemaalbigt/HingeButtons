using UnityEngine;

public class HandSetup : MonoBehaviour
{
    public GameObject _fingerTipPrefab;
    public OVRSkeleton _leftSkeleton;
    public OVRSkeleton _rightSkeleton;

    private bool _setupComplete;

    void Update() {
        if (!_setupComplete && HandTracking.I.IsActive && HandTracking.I.BonesInitialized()) {


            var leftTip = Instantiate(_fingerTipPrefab, HandTracking.I.GetBoneUnsafe(Hand.left, BoneId.Hand_Index_Tip));
            var rightTip = Instantiate(_fingerTipPrefab, HandTracking.I.GetBoneUnsafe(Hand.right, BoneId.Hand_Index_Tip));
            leftTip.transform.localPosition = new Vector3(0.0075f, 0, 0);
            rightTip.transform.localPosition = new Vector3(-0.0075f, 0, 0);

            _setupComplete = true;
        }
    }
}
