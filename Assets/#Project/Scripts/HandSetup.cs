using UnityEngine;

public class HandSetup : MonoBehaviour
{
    public GameObject _fingerTipPrefab;
    public OVRSkeleton _leftSkeleton;
    public OVRSkeleton _rightSkeleton;

    private GameObject _leftTip;
    private GameObject _rightTip;

    void Update() {
        if (HandTracking.I.IsActive && HandTracking.I.BonesInitialized() && !TipsSpawned()) {
            HandTracking.I.TryGetBone(Hand.left, BoneId.Hand_Index_Tip, out Transform leftBone);
            HandTracking.I.TryGetBone(Hand.right, BoneId.Hand_Index_Tip, out Transform rightBone);

            if (leftBone == null || rightBone == null)
                return;

            _leftTip = Instantiate(_fingerTipPrefab, leftBone);
            _rightTip = Instantiate(_fingerTipPrefab, rightBone);
        }

        _leftTip.transform.localPosition = new Vector3(0.0075f, 0f, 0f);
        _rightTip.transform.localPosition = new Vector3(-0.0075f, 0, 0);

    }

    private bool TipsSpawned() {
        return _leftTip != null && _rightTip != null;
    }
}
