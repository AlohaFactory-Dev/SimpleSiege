using UnityEngine;
using Spine.Unity;

public class KingSkinController : MonoBehaviour
{
    // SkeletonMecanim 컴포넌트 참조
    private SkeletonMecanim skeletonMecanim;

    void Awake()
    {
        skeletonMecanim = GetComponentInChildren<SkeletonMecanim>();
    }

    public void ChangeSkin(string skinName)
    {
        skeletonMecanim.Skeleton.SetSkin(skinName);
        skeletonMecanim.Skeleton.SetSlotsToSetupPose();
    }
}